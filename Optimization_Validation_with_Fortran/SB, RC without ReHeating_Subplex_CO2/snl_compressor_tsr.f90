!-----------------------------------------------------------------------------------------------------------------------------------
!
! This is free and unencumbered software released into the public domain.
! 
! Anyone is free to copy, modify, publish, use, compile, sell, or distribute this software, either in source code form or as a
! compiled binary, for any purpose, commercial or non-commercial, and by any means.
! 
! THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
! MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM,
! DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
! SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
!
!-----------------------------------------------------------------------------------------------------------------------------------
!
! This file contains the the module 'compressors', which defines a number of subroutines based on the radial compressor being
! studied at Sandia National Laboratory.  The recompressor is assumed to be two-stage; this is the only difference between this
! file and 'snl_compressor.f90'.
!
! Notes:
!   1) The recompressor is modeled using two SNL compressors in series.
!
! Author: John Dyreby, Solar Energy Laboratory, University of Wisconsin-Madison <jjdyreby@uwalumni.com>
!
! Last Modified: September 25, 2014
!
!-----------------------------------------------------------------------------------------------------------------------------------

module compressors

use core
implicit none
private
public :: compressor_sizing, recompressor_sizing, off_design_compressor, off_design_recompressor

real(dp), parameter :: snl_phi_design = 0.02971_dp  ! design-point flow coefficient for Sandia compressor (corresponds to max eta)
real(dp), parameter :: snl_phi_min = 0.02_dp        ! approximate surge limit for SNL compressor
real(dp), parameter :: snl_phi_max = 0.05_dp        ! approximate x-intercept for SNL compressor


contains


subroutine compressor_sizing(recomp_cycle, error_trace)
    ! Determine the compressor rotor diameter and design-point shaft speed
    ! and store values in recomp_cycle%mc.
    !
    ! Arguments:
    !   recomp_cycle -- a RecompCycle object that defines the simple/recompression cycle at the design point
    !   error_trace -- an ErrorTrace object
    
    use CO2_properties, only: CO2_TD, CO2_PS

    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    type(ErrorTrace), intent(out) :: error_trace

    ! Local Variables
    integer  :: error_code
    real(dp) :: D_in, h_in, s_in, T_out, P_out, h_out, D_out, ssnd_out, h_s_out, psi_design, m_dot, w_i, U_tip, N_rad_s

    ! Create references to cycle state properties for clarity.
    D_in = recomp_cycle%dens(1)
    h_in = recomp_cycle%enth(1)
    s_in = recomp_cycle%entr(1)
    T_out = recomp_cycle%temp(2)
    P_out = recomp_cycle%pres(2)
    h_out = recomp_cycle%enth(2)
    D_out = recomp_cycle%dens(2)
    call CO2_TD(T=T_out, D=D_out, error_code=error_code, ssnd=ssnd_out)  ! speed of sound at outlet
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 69
        error_trace%files(1) = 4
        return
    end if    
    call CO2_PS(P=P_out, S=s_in, error_code=error_code, enth=h_s_out)  ! outlet specific enthalpy after isentropic compression
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 76
        error_trace%files(1) = 4
        return
    end if

    ! Calculate psi at the design-point phi using Horner's method 
    psi_design = ((((-498626.0_dp * snl_phi_design) + 53224.0_dp) * snl_phi_design - 2505.0_dp) * snl_phi_design + 54.6_dp) &
                 * snl_phi_design + 0.04049_dp  ! from dimensionless modified head curve (at design-point, psi and modified psi are equal)
   
    ! Determine required size and speed of compressor.
    m_dot = recomp_cycle%m_dot_turbine * (1.0_dp - recomp_cycle%recomp_frac)  ! mass flow rate through compressor (kg/s)
    w_i = h_s_out - h_in  ! positive isentropic specific work of compressor (kJ/kg)
    U_tip = sqrt(1000.0_dp * w_i / psi_design)  ! rearranging definition of head coefficient and converting kJ to J
    recomp_cycle%mc%D_rotor = sqrt(m_dot / (snl_phi_design * D_in * U_tip))  ! rearranging definition of flow coefficient
    N_rad_s = U_tip * 2.0_dp / recomp_cycle%mc%D_rotor   ! shaft speed in rad/s
    recomp_cycle%mc%N_design = N_rad_s * 9.549296590_dp  ! shaft speed in rpm

    ! Set other compressor variables.
    recomp_cycle%mc%w_tip_ratio = U_tip / ssnd_out     ! ratio of the tip speed to local (comp outlet) speed of sound
    recomp_cycle%mc%eta_design = w_i / (h_out - h_in)  ! definition of isentropic efficiency
    recomp_cycle%mc%eta = recomp_cycle%mc%eta_design
    recomp_cycle%mc%phi_design = snl_phi_design
    recomp_cycle%mc%phi = snl_phi_design
    recomp_cycle%mc%phi_min = snl_phi_min
    recomp_cycle%mc%phi_max = snl_phi_max
    recomp_cycle%mc%N = recomp_cycle%mc%N_design
    recomp_cycle%mc%surge = .false.

end subroutine compressor_sizing


subroutine recompressor_sizing(recomp_cycle, error_trace)
    ! Determine the recompressor rotor diameter and design-point shaft speed
    ! and store values in recomp_cycle%rc.
    !
    ! Arguments:
    !   recomp_cycle -- a RecompCycle object that defines the simple/recompression cycle at the design point
    !   error_trace -- an ErrorTrace object
    
    use CO2_properties, only: CO2_TD, CO2_PS, CO2_PH

    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    type(ErrorTrace), intent(out) :: error_trace

    ! Parameters
    integer, parameter  :: max_iter = 100
    real(dp), parameter :: tolerance = 1.0e-8_dp  ! absolute tolerance for phi and stage efficiency

    ! Local Variables
    integer  :: iter, error_code
    real(dp) :: P_in, D_in, h_in, s_in, T_out, P_out, h_out, D_out, ssnd_out, h_s_out, psi_design, m_dot, w_i, U_tip_1, U_tip_2
    real(dp) :: eta_design, P_int, D_int, h_int, s_int, ssnd_int, lower_bound, upper_bound, N_design, D_rotor_1, D_rotor_2, w, phi
    real(dp) :: eta_stage, eta_2_req, residual, last_residual, P_secant, last_P_int, secant_step, N_rad_s

    ! Create references to cycle state properties for clarity.
    P_in = recomp_cycle%pres(9)
    D_in = recomp_cycle%dens(9)
    h_in = recomp_cycle%enth(9)
    s_in = recomp_cycle%entr(9)
    T_out = recomp_cycle%temp(10)
    P_out = recomp_cycle%pres(10)
    h_out = recomp_cycle%enth(10)
    D_out = recomp_cycle%dens(10)

    ! Set a few variables that apply to the whole recompressor.
    call CO2_TD(T=T_out, D=D_out, error_code=error_code, ssnd=ssnd_out)  ! speed of sound at outlet
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 145
        error_trace%files(1) = 4
        return
    end if
    call CO2_PS(P=P_out, S=s_in, error_code=error_code, enth=h_s_out)  ! ideal specific enthalpy after compression
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 152
        error_trace%files(1) = 4
        return
    end if
    eta_design = (h_s_out - h_in) / (h_out - h_in)  ! overall isentropic efficiency
    m_dot = recomp_cycle%m_dot_turbine * recomp_cycle%recomp_frac  ! mass flow rate through recompressor (kg/s)
    psi_design = ((((-498626.0_dp * snl_phi_design) + 53224.0_dp) * snl_phi_design - 2505.0_dp) * snl_phi_design + 54.6_dp) &
                 * snl_phi_design + 0.04049_dp  ! from dimensionless modified head curve (at design-point, psi and modified psi are equal)

    ! Prepare intermediate pressure iteration loop.
    last_residual = 0.0_dp
    last_P_int = 1.0e12_dp  ! ensures bisection will be used for first step
    lower_bound = P_in + 1e-6_dp
    upper_bound = P_out - 1e-6_dp
    P_int = (lower_bound + upper_bound) * 0.5_dp
    eta_stage = eta_design  ! first guess for stage efficiency
    do iter = 1, max_iter

        ! First stage
        call CO2_PS(P=P_int, S=s_in, error_code=error_code, enth=h_s_out)  ! ideal outlet specific enthalpy after first stage
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 174
            error_trace%files(1) = 4
            return
        end if        
        w_i = h_s_out - h_in  ! positive isentropic specific work of first stage
        U_tip_1 = sqrt(1000.0_dp * w_i / psi_design)  ! rearranging definition of head coefficient and converting kJ to J
        D_rotor_1 = sqrt(m_dot / (snl_phi_design * D_in * U_tip_1))  ! rearranging definition of flow coefficient
        N_rad_s = U_tip_1 * 2.0_dp / D_rotor_1   ! shaft speed in rad/s
        N_design = N_rad_s * 9.549296590_dp  ! shaft speed in rpm
        w = w_i / eta_stage  ! actual first-stage work
        h_int = h_in + w  ! energy balance on first stage
        call CO2_PH(P=P_int, H=h_int, error_code=error_code, dens=D_int, entr=s_int, ssnd=ssnd_int)
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 188
            error_trace%files(1) = 4
            return
        end if

        ! Second stage
        call CO2_PS(P=P_out, S=s_int, error_code=error_code, enth=h_s_out)  ! ideal outlet specific enthalpy after second stage
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 197
            error_trace%files(1) = 4
            return
        end if        
        w_i = h_s_out - h_int  ! positive isentropic specific work of second stage
        U_tip_2 = sqrt(1000.0_dp * w_i / psi_design)  ! rearranging definition of head coefficient and converting kJ to J
        D_rotor_2 = 2.0_dp * U_tip_2 / (N_design * 0.104719755_dp)  ! required second-stage diameter
        phi = m_dot / (D_int * U_tip_2 * D_rotor_2**2)  ! required flow coefficient
        eta_2_req = w_i / (h_out - h_int)  ! required second stage efficiency to achieve overall eta_design

        ! Check convergence and update guesses.
        residual = snl_phi_design - phi
        if (residual < 0.0_dp) then  ! P_int guess is too high
            if (-residual <= tolerance .and. abs(eta_stage-eta_2_req) <= tolerance) exit
            upper_bound = P_int
        else  ! P_int guess is too low
            if (residual <= tolerance .and. abs(eta_stage-eta_2_req) <= tolerance) exit
            lower_bound = P_int
        end if
        secant_step = -residual * (last_P_int - P_int) / (last_residual - residual)
        P_secant = P_int + secant_step
        last_P_int = P_int
        last_residual = residual
        if (P_secant <= lower_bound .or. P_secant >= upper_bound) then  ! secant method overshot
            P_int = (lower_bound + upper_bound) * 0.5_dp
        else if (abs(secant_step) > abs((upper_bound - lower_bound) * 0.5_dp)) then  ! take the smaller step to ensure convergence
            P_int = (lower_bound + upper_bound) * 0.5_dp
        else
            P_int = P_secant  ! use secant guess
        end if
        eta_stage = 0.5_dp * (eta_stage + eta_2_req)  ! update guess for stage efficiency
    end do

    ! Check for convergence.
    if (iter >= max_iter) then  ! did not converge
        error_trace%code = 1
        error_trace%lines(1) = 234
        error_trace%files(1) = 4
        return
    end if

    ! Set recompressor variables.
    recomp_cycle%rc%D_rotor = D_rotor_1
    recomp_cycle%rc%D_rotor_2 = D_rotor_2
    recomp_cycle%rc%N_design = N_design
    recomp_cycle%rc%eta_design = eta_stage
    recomp_cycle%rc%phi_design = snl_phi_design
    recomp_cycle%rc%phi_min = snl_phi_min
    recomp_cycle%rc%phi_max = snl_phi_max
    recomp_cycle%rc%N = N_design
    recomp_cycle%rc%eta = eta_design
    recomp_cycle%rc%phi = snl_phi_design
    recomp_cycle%rc%phi_2 = snl_phi_design
    recomp_cycle%rc%w_tip_ratio = max(U_tip_1 / ssnd_int, U_tip_2 / ssnd_out)
    recomp_cycle%rc%surge = .false.

end subroutine recompressor_sizing


subroutine off_design_compressor(comp, T_in, P_in, m_dot, N, error_trace, T_out, P_out)
    ! Solve for the outlet state of 'comp' given its inlet conditions, mass flow rate, and shaft speed.
    !
    ! Inputs:
    !   comp -- a Compressor object, with design-point values and sizing set
    !   T_in -- compressor inlet temperature (K)
    !   P_in -- compressor inlet pressure (kPa)
    !   m_dot -- mass flow rate through compressor (kg/s)
    !   N -- shaft speed of compressor (rpm)
    !
    ! Outputs:
    !   error_trace -- an ErrorTrace object
    !   T_out -- compressor outlet temperature (K)
    !   P_out -- compressor outlet pressure (kPa)
    !
    ! Notes:
    !   1) This subroutine also sets the following values in 'comp': surge, eta, w, w_tip_ratio, phi

    use CO2_Properties, only: CO2_TP, CO2_HS, CO2_PH

    ! Arguments
    type(Compressor), intent(inout) :: comp
    real(dp), intent(in) :: T_in, P_in, m_dot, N
    type(ErrorTrace), intent(out) :: error_trace
    real(dp), intent(out) :: T_out, P_out
    
    ! Local Variables
    integer  :: error_code
    real(dp) :: rho_in, h_in, s_in, U_tip, phi, phi_star, psi_star, eta_star, psi, eta_0, dh_s, dh, h_s_out, h_out, ssnd_out

    call CO2_TP(T=T_in, P=P_in, error_code=error_code, dens=rho_in, enth=h_in, entr=s_in)  ! fully define the inlet state of the compressor
    if (error_code /= 0) then
        error_trace%code = 1
        error_trace%lines(1) = 289
        error_trace%files(1) = 4
        return
    end if

    ! Calculate the modified flow and head coefficients and efficiency for the SNL compressor.
    U_tip = comp%D_rotor * 0.5_dp * N * 0.104719755_dp  ! tip speed in m/s
    phi = m_dot / (rho_in * U_tip * comp%D_rotor**2)    ! flow coefficient
    if (phi < comp%phi_min) then ! the compressor is operating in the surge region
        comp%surge = .true.  
        phi = comp%phi_min  ! reset phi to to its minimum value; this sets psi and eta to be fixed at the values at the surge limit
    else
        comp%surge = .false.
    end if
    phi_star = phi * (N / comp%N_design)**0.2_dp  ! modified flow coefficient
    psi_star = ((((-498626.0_dp * phi_star) + 53224.0_dp) * phi_star - 2505.0_dp) * phi_star + 54.6_dp) * phi_star + 0.04049_dp  ! from dimensionless modified head curve
    eta_star = ((((-1.638e6_dp * phi_star) + 182725.0_dp) * phi_star - 8089.0_dp) * phi_star + 168.6_dp) * phi_star - 0.7069_dp  ! from dimensionless modified efficiency curve
    psi = psi_star / ((comp%N_design / N)**((20.0_dp * phi_star)**3))
    eta_0 = eta_star * 1.47528_dp / ((comp%N_design / N)**((20.0_dp * phi_star)**5))  ! efficiency is normalized so it equals 1.0 at snl_phi_desig
    comp%eta = max(eta_0 * comp%eta_design, 0.0_dp)  ! the actual compressor efficiency, not allowed to go negative

    ! Check that the specified mass flow rate is possible with the compressor's current shaft speed.
    if (psi <= 0.0_dp) then  ! shaft speed is too low for the given m_dot
        error_trace%code = 1
        error_trace%lines(1) = 314
        error_trace%files(1) = 4
        return
    end if

    ! Calculate the compressor outlet state.
    dh_s = psi * U_tip**2 * 0.001_dp  ! ideal enthalpy rise in compressor, from definition of head coefficient (kJ/kg)
    dh = dh_s / comp%eta              ! actual enthalpy rise in compressor
    h_s_out = h_in + dh_s             ! ideal enthalpy at compressor outlet
    h_out = h_in + dh                 ! actual enthalpy at compressor outlet
    call CO2_HS(H=h_s_out, S=s_in, error_code=error_code, pres=P_out)  ! get the compressor outlet pressure
    if (error_code /= 0) then  ! most likely case is that the outlet pressure is above the high pressure limit of the property routine
        error_trace%code = 2
        error_trace%lines(1) = 326
        error_trace%files(1) = 4
        return
    end if
    call CO2_PH(P=P_out, H=h_out, error_code=error_code, temp=T_out, ssnd=ssnd_out)  ! determines compressor outlet temperature and speed of sound
    if (error_code /= 0) then  ! most likely case is that the outlet pressure is above the high pressure limit of the property routine
        error_trace%code = 2
        error_trace%lines(1) = 333
        error_trace%files(1) = 4
        return
    end if

    ! Set a few compressor variables.
    comp%phi = phi
    comp%w_tip_ratio = U_tip / ssnd_out     ! ratio of the tip speed to local (comp outlet) speed of sound

end subroutine off_design_compressor


subroutine off_design_recompressor(comp, T_in, P_in, m_dot, P_out, error_trace, T_out)
    ! Solve for the outlet state (and shaft speed) of 'comp' given its inlet conditions, mass flow rate, and outlet pressure.
    !
    ! Inputs:
    !   comp -- a Compressor object, with design-point values and sizing set
    !   T_in -- compressor inlet temperature (K)
    !   P_in -- compressor inlet pressure (kPa)
    !   m_dot -- mass flow rate through compressor (kg/s)
    !   P_out -- compressor outlet pressure (kPa)
    !
    ! Outputs:
    !   error_trace -- an ErrorTrace object
    !   T_out -- compressor outlet temperature (K)
    !
    ! Notes:
    !   1) This subroutine also sets the following values in 'comp': N, surge, eta, w, w_tip_ratio, phi
    !   2) In order to solve the compressor, the value for flow coefficient (phi) is varied until convergence.
    !   3) Surge is not allowed; if the corresponding flow coefficient is not between phi_min and phi_max an error is raised.
    !   4) Two-stage recompressor; surge is true if either stages are in surge conditions; w_tip_ratio is max of the two stages.
    
    use CO2_Properties, only: CO2_TP, CO2_PS, CO2_PH, CO2_HS

    ! Arguments
    type(Compressor), intent(inout) :: comp
    real(dp), intent(in) :: T_in, P_in, m_dot, P_out
    type(ErrorTrace), intent(out) :: error_trace
    real(dp), intent(out) :: T_out

    ! Parameters
    integer, parameter :: max_iter = 100
    real(dp), parameter :: rel_tol = 1.0e-9_dp  ! relative tolerance for pressure

    ! Local Variables
    integer  :: iter, error_code
    logical  :: first_pass
    real(dp) :: rho_in, h_in, s_in, phi_1, U_tip_1, phi_star, psi_star, eta_star, psi, eta_0, dh_s, dh, h_s_out, h_out, ssnd_out
    real(dp) :: N, residual, next_phi, last_phi_1, last_residual, P_int, h_int, D_int, s_int, ssnd_int, eta_stage_1
    real(dp) :: eta_stage_2, phi_2, U_tip_2, P_out_calc

    call CO2_TP(T=T_in, P=P_in, error_code=error_code, dens=rho_in, enth=h_in, entr=s_in)  ! fully define the inlet state of the compressor
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 387
        error_trace%files(1) = 4
        return
    end if

	!print *, 'm_dot:'
	!print *, m_dot
	
    !print *, 'N:'
    !print *, N
	
	!print *, 'T_in:'
	!print *, T_in
	
	!print *, 'P_in:'
	!print *, p_in
	
	!pause 1
	
    ! Iterate on first-stage phi.
    phi_1 = comp%phi_design  ! start with design-point value
    first_pass = .true.
    do iter = 1, max_iter

        ! First stage - dh_s and eta_stage_1.
        U_tip_1 = m_dot / (phi_1 * rho_in * comp%D_rotor**2)  ! flow coefficient rearranged
        N = (U_tip_1 * 2.0_dp / comp%D_rotor) * 9.549296590_dp  ! shaft speed in rpm
        phi_star = phi_1 * (N / comp%N_design)**0.2_dp  ! modified flow coefficient
        psi_star = ((((-498626.0_dp * phi_star) + 53224.0_dp) * phi_star - 2505.0_dp) * phi_star + 54.6_dp) * phi_star + 0.04049_dp  ! from dimensionless modified head curve
        psi = psi_star / ((comp%N_design / N)**((20.0_dp * phi_star)**3))
        dh_s = psi * U_tip_1**2 * 0.001_dp  ! calculated ideal enthalpy rise in first stage of compressor, from definition of head coefficient (kJ/kg)
        eta_star = ((((-1.638e6_dp * phi_star) + 182725.0_dp) * phi_star - 8089.0_dp) * phi_star + 168.6_dp) * phi_star - 0.7069_dp  ! from dimensionless modified efficiency curve
        eta_0 = eta_star * 1.47528_dp / ((comp%N_design / N)**((20.0_dp * phi_star)**5))  ! stage efficiency is normalized so it equals 1.0 at snl_phi_design
        eta_stage_1 = max(eta_0 * comp%eta_design, 0.0_dp)  ! the actual stage efficiency, not allowed to go negative
        
        ! Calculate first-stage outlet (second-stage inlet) state.
        dh = dh_s / eta_stage_1  ! actual enthalpy rise in first stage
        h_s_out = h_in + dh_s    ! ideal enthalpy between stages
        h_int = h_in + dh        ! actual enthalpy between stages
        call CO2_HS(H=h_s_out, S=s_in, error_code=error_code, pres=P_int)  ! get the first-stage outlet pressure (second-stage inlet pressure)
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 415
            error_trace%files(1) = 4
            return
        end if
        call CO2_PH(P=P_int, H=h_int, error_code=error_code, dens=D_int, entr=s_int, ssnd=ssnd_int)  ! get second-stage inlet properties
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 422
            error_trace%files(1) = 4
            return
        end if

        ! Second stage - dh_s and eta_stage_2.
        U_tip_2 = comp%D_rotor_2 * 0.5_dp * N * 0.104719755_dp  ! second-stage tip speed in m/s
        phi_2 = m_dot / (D_int * U_tip_2 * comp%D_rotor_2**2)   ! second-stage flow coefficient
        phi_star = phi_2 * (N / comp%N_design)**0.2_dp  ! modified flow coefficient
        psi_star = ((((-498626.0_dp * phi_star) + 53224.0_dp) * phi_star - 2505.0_dp) * phi_star + 54.6_dp) * phi_star + 0.04049_dp  ! from dimensionless modified head curve
        psi = psi_star / ((comp%N_design / N)**((20.0_dp * phi_star)**3))
        dh_s = psi * U_tip_2**2 * 0.001_dp  ! calculated ideal enthalpy rise in second stage of compressor, from definition of head coefficient (kJ/kg)
        eta_star = ((((-1.638e6_dp * phi_star) + 182725.0_dp) * phi_star - 8089.0_dp) * phi_star + 168.6_dp) * phi_star - 0.7069_dp  ! from dimensionless modified efficiency curve
        eta_0 = eta_star * 1.47528_dp / ((comp%N_design / N)**((20.0_dp * phi_star)**5))  ! stage efficiency is normalized so it equals 1.0 at snl_phi_design
        eta_stage_2 = max(eta_0 * comp%eta_design, 0.0_dp)  ! the actual stage efficiency, not allowed to go negative
        
        ! Calculate second-stage outlet state.
        dh = dh_s / eta_stage_2  ! actual enthalpy rise in second stage
        h_s_out = h_int + dh_s   ! ideal enthalpy at compressor outlet
        h_out = h_int + dh       ! actual enthalpy at compressor outlet
        call CO2_HS(H=h_s_out, S=s_int, error_code=error_code, pres=P_out_calc)  ! get the calculated compressor outlet pressure
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 445
            error_trace%files(1) = 4
            return
        end if

        ! Check for convergence and adjust phi_1 guess.
        residual = P_out - P_out_calc
        if (abs(residual) / P_out <= rel_tol) exit  ! converged sufficiently
        if (first_pass) then
            next_phi = phi_1 * 1.0001_dp  ! take a small step
            first_pass = .false.
        else
            next_phi = phi_1 - residual * (last_phi_1 - phi_1) / (last_residual - residual)  ! next guess predicted using secant method
        end if
        last_phi_1 = phi_1
        last_residual = residual
        phi_1 = next_phi

    end do

    ! Check for convergence.
    if (iter >= max_iter) then  ! did not converge
        error_trace%code = 1
        error_trace%lines(1) = 469
        error_trace%files(1) = 4
        return
    end if
    
    ! Determine outlet temperature and speed of sound.
    call CO2_PH(P=P_out_calc, H=h_out, error_code=error_code, temp=T_out, ssnd=ssnd_out)
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 477
        error_trace%files(1) = 4
        return
    end if
    call CO2_PS(P=P_out_calc, S=s_in, error_code=error_code, enth=h_s_out)  ! outlet specific enthalpy after isentropic compression
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 484
        error_trace%files(1) = 4
        return
    end if

    ! Set relevant recompressor variables.
    comp%N = N
    comp%eta = (h_s_out - h_in) / (h_out - h_in)  ! use overall isentropic efficiency
    comp%phi = phi_1
    comp%phi_2 = phi_2
    comp%w_tip_ratio = max(U_tip_1 / ssnd_int, U_tip_2 / ssnd_out)  ! store maximum ratio
    comp%surge = (phi_1 < comp%phi_min .or. phi_2 < comp%phi_min)

end subroutine off_design_recompressor


end module compressors
