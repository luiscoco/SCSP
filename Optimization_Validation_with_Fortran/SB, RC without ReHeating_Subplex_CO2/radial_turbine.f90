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
! This file contains the the module 'turbines', defining a number of subroutines based on a generic, low-reaction radial turbine.
!
! Author: John Dyreby, Solar Energy Laboratory, University of Wisconsin-Madison <jjdyreby@uwalumni.com>
!
! Last Modified: August 14, 2014
!
!-----------------------------------------------------------------------------------------------------------------------------------

module turbines

use core
implicit none
private
public :: turbine_sizing, off_design_turbine

real(dp), parameter :: nu_design = 0.707_dp  ! design-point ratio of tip speed to spouting velocity at maximum efficiency


contains


subroutine turbine_sizing(recomp_cycle, error_trace)
    ! Determine the turbine rotor diameter, effective nozzle area, and design-point shaft
    ! speed and store values in recomp_cycle%t.
    !
    ! Arguments:
    !   recomp_cycle -- a RecompCycle object that defines the simple/recompression cycle at the design point
    !   error_trace -- an ErrorTrace object
    !
    ! Notes:
    !   1) The value for recomp_cycle%t%N_design is required to be set.  If it is <= 0.0 then
    !      the value for recomp_cycle%mc%N_design is used (i.e., link the compressor and turbine
    !      shafts).  For this reason, turbine_sizing must be called after compressor_sizing if
    !      the shafts are to be linked.
    
    use CO2_properties, only: CO2_TD, CO2_PS

    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    type(ErrorTrace), intent(out) :: error_trace

    ! Local Variables
    integer :: error_code
    real(dp) :: T_in, D_in, h_in, s_in, P_out, h_out, D_out, ssnd_in, h_s_out, w_i, C_s, U_tip

    ! Check if a design-point shaft speed is available.
    if (recomp_cycle%t%N_design <= 0.0_dp) then  ! link shafts
        recomp_cycle%t%N_design = recomp_cycle%mc%N_design
        if (recomp_cycle%mc%N_design <= 0.0_dp) then
            error_trace%code = 7
            error_trace%lines(1) = 61
            error_trace%files(1) = 5
            return
        end if        
    end if

    ! Create references to cycle state properties for clarity.
    T_in = recomp_cycle%temp(6)
    D_in = recomp_cycle%dens(6)
    h_in = recomp_cycle%enth(6)
    s_in = recomp_cycle%entr(6)
    P_out = recomp_cycle%pres(7)
    h_out = recomp_cycle%enth(7)
    D_out = recomp_cycle%dens(7)
    call CO2_TD(T=T_in, D=D_in, error_code=error_code, ssnd=ssnd_in)  ! speed of sound at inlet
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 79
        error_trace%files(1) = 5
        return
    end if
    call CO2_PS(P=P_out, S=s_in, error_code=error_code, enth=h_s_out)  ! outlet specific enthalpy after isentropic expansion
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 86
        error_trace%files(1) = 5
        return
    end if

    ! Determine necessary turbine parameters.
    recomp_cycle%t%nu = nu_design
    w_i = h_in - h_s_out  ! isentropic specific work of turbine (kJ/kg)
    C_s = sqrt(2.0_dp * w_i * 1000.0_dp)  ! spouting velocity in m/s
    U_tip = recomp_cycle%t%nu * C_s  ! rearrange definition of nu
    recomp_cycle%t%D_rotor = U_tip / (0.5_dp * recomp_cycle%t%N_design * 0.104719755_dp)  ! turbine diameter in m
    recomp_cycle%t%A_nozzle = recomp_cycle%m_dot_turbine / (C_s * D_out)  ! turbine effective nozzle area in m2

    ! Set other turbine variables.
    recomp_cycle%t%w_tip_ratio = U_tip / ssnd_in  ! ratio of the tip speed to local (turbine inlet) speed of sound
    recomp_cycle%t%eta_design = (h_in - h_out) / w_i  ! definition of isentropic efficiency
    recomp_cycle%t%eta = recomp_cycle%t%eta_design
    recomp_cycle%t%N = recomp_cycle%t%N_design

end subroutine turbine_sizing


subroutine off_design_turbine(turb, T_in, P_in, P_out, N, error_trace, m_dot, T_out)
    ! Solve for the outlet state of 'turb' given its inlet conditions, outlet pressure, and shaft speed.
    !
    ! Inputs:
    !   turb -- a Turbine object, with design-point values and sizing set
    !   T_in -- turbine inlet temperature (K)
    !   P_in -- turbine inlet pressure (kPa)
    !   P_out -- turbine outlet pressure (kPa)
    !   N -- shaft speed of turbine (rpm)
    !
    ! Outputs:
    !   error_trace -- an ErrorTrace object
    !   m_dot -- allowable mass flow rate through the turbine (kg/s)
    !   T_out -- turbine outlet temperature (K)
    !
    ! Notes:
    !   1) This subroutine also sets the following values in 'turb': nu, eta, m_dot, w, w_tip_ratio

    use CO2_Properties, only: CO2_TP, CO2_PS, CO2_PH

    ! Arguments
    type(Turbine), intent(inout) :: turb
    real(dp), intent(in) :: T_in, P_in, P_out, N
    type(ErrorTrace), intent(out) :: error_trace
    real(dp), intent(out) :: m_dot, T_out
    
    ! Local Variables
    integer  :: error_code
    real(dp) :: h_in, s_in, ssnd_in, U_tip, h_s_out, h_out, D_out, C_s, eta_0

    call CO2_TP(T=T_in, P=P_in, error_code=error_code, enth=h_in, entr=s_in, ssnd=ssnd_in)  ! properties at inlet of turbine
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 141
        error_trace%files(1) = 5
        return
    end if
    call CO2_PS(P=P_out, S=s_in, error_code=error_code, enth=h_s_out)  ! enthalpy at the turbine outlet if the expansion is isentropic
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 148
        error_trace%files(1) = 5
        return
    end if

    ! Apply the radial turbine equations for efficiency.
    C_s = sqrt(2.0_dp * (h_in - h_s_out) * 1000.0_dp)  ! spouting velocity (m/s)
    U_tip = turb%D_rotor * 0.5_dp * N * 0.104719755_dp  ! turbine tip speed (m/s)
    turb%nu = U_tip / C_s  ! ratio of tip speed to spouting velocity
    if (turb%nu < 1.0_dp) then
        eta_0 = 2.0_dp * turb%nu * sqrt(1.0_dp - turb%nu**2)  ! efficiency from Baines (1.0 at design point)
    else
        eta_0 = 0.0_dp  ! catches nu values just over 1, which leads to sqrt of negative number
    end if
    turb%eta = eta_0 * turb%eta_design  ! actual turbine efficiency
    
    ! Calculate the outlet state and allowable mass flow rate.
    h_out = h_in - turb%eta * (h_in - h_s_out)  ! enthalpy at turbine outlet
    call CO2_PH(P=P_out, H=h_out, error_code=error_code, temp=T_out, dens=D_out)
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 169
        error_trace%files(1) = 5
        return
    end if
    m_dot = C_s * turb%A_nozzle * D_out  ! mass flow through turbine (kg/s)
    turb%w_tip_ratio = U_tip / ssnd_in  ! ratio of the tip speed to the local (turbine inlet) speed of sound
    turb%N = N

end subroutine off_design_turbine


end module turbines
