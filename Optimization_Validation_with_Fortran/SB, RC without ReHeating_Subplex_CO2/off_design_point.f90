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
! This file contains the the module 'off_design_point', which defines five system-level subroutines:
!   off_design -- the main off-design cycle model
!   target_off_design -- given a power or heat addition target, iterates on P1 to match it (returns an error if target not possible)
!   target_off_design_alt -- given a power or heat addition target, iterates on P1 to match it or return the nearest possible value
!   optimal_off_design -- iterates on inputs to off_design in order to maximize power output or thermal efficiency
!   optimal_target_off_design -- determines the maximum efficiency of the cycle for a give target
!
! Notes:
!   1) The optimization routines often need a bit of tweaking (initial guess values, variable scales, etc.) and improvements
!      could be made w.r.t. handling invalid inputs.  There are likely better techniques that could be developed in
!      order to provide fast and stable optimization (especially in regards to the optimal_target_off_design subroutine).
!   2) Allowing surge and supersonic tip speeds makes convergence easier and the optimal results typically end of being valid, so
!      the values for the parameters 'surge_allowed' and 'supersonic_tip_speed_allowed' are .true. by default.
!   3) The optimal_target_off_design subroutine uses max W_dot_net as proxy for max_Q_dot if the target is not possible.
!   4) The target_off_design and optimal_target_off_design subroutines allow high-side pressures slightly above the high-pressure
!      limit to help the optimization routines; calls to these subroutines should be checked for satisfactory pressures.
!
! Cycle State Points:
!   1)  mc in           / PC out
!   2)  LT in (cold)    / mc out
!   3)  mixing valve in / LT out (cold)
!   4)  HT in (cold)    / mixing valve out
!   5)  PHX in          / HT out (cold)
!   6)  turbine in      / PHX out
!   7)  HT in (hot)     / turbine out
!   8)  LT in (hot)     / HT out (hot)
!   9)  PC and rc in    / LT out (hot)
!   10) mixing valve in / recomp out
!
! Author: John Dyreby, Solar Energy Laboratory, University of Wisconsin-Madison <jjdyreby@uwalumni.com>
!
! Last Modified: August 20, 2014
!
!-----------------------------------------------------------------------------------------------------------------------------------

module off_design_point

use core
use compressors
use turbines
use heat_exchangers
implicit none
private
public :: off_design, target_off_design, optimal_off_design, optimal_target_off_design

logical, parameter :: surge_allowed = .true.
logical, parameter :: supersonic_tip_speed_allowed = .true.


contains


subroutine off_design( &
    recomp_cycle,      &  ! [input/output] a RecompCycle object with design-point variables set
    T_mc_in,           &  ! [input] compressor inlet temperature (K)
    T_t_in,            &  ! [input] turbine inlet temperature (K)
    P_mc_in,           &  ! [input] compressor inlet pressure (kPa)
    recomp_frac,       &  ! [input] fraction of flow that bypasses the precooler and main compressor
    N_mc,              &  ! [input] main compressor shaft speed (rpm)
    N_t,               &  ! [input] turbine shaft speed (rpm)
    N_sub_hxrs,        &  ! [input] number of sub-heat exchangers to use when calculating UA value for a heat exchanger
    tol,               &  ! [input] convergence tolerance
    error_trace        &  ! [output] an ErrorTrace object
    )

    use CO2_Properties, only: CO2_TP, CO2_PH

    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    real(dp), intent(in) :: T_mc_in, T_t_in, P_mc_in, recomp_frac, N_mc, N_t, tol
    integer, intent(in)  :: N_sub_hxrs
    type(ErrorTrace), intent(out) :: error_trace

    ! Parameters
    integer, parameter :: max_iter = 100
    real(dp), parameter :: temperature_tolerance = 1.0e-6_dp  ! temperature differences below this are considered zero

    ! Local Variables
    integer  :: m_dot_iter, T9_iter, T8_iter, error_code, index
    real(dp) :: rho_in, C_dot_min, Q_dot_max, m_dot_t_allowed, m_dot_residual, partial_phi, tip_speed
    real(dp) :: m_dot_lower_bound, m_dot_upper_bound, m_dot_mc_guess, m_dot_mc_max, last_m_dot_guess, last_m_dot_residual
    real(dp) :: T9_lower_bound, T9_upper_bound, T8_lower_bound, T8_upper_bound, last_LT_residual, last_T9_guess
    real(dp) :: last_HT_residual, last_T8_guess, secant_guess
    real(dp) :: m_dot_t, m_dot_mc, m_dot_rc, UA_LT, UA_HT, w_mc, w_rc, w_t
    real(dp) :: min_DT_LT, min_DT_HT, UA_LT_calc, UA_HT_calc, Q_dot_LT, Q_dot_HT, UA_HT_residual, UA_LT_residual
    real(dp), dimension(10) :: temp, pres, enth, entr, dens
    real(dp), dimension(2) :: DP_LT, DP_HT, DP_PC, DP_PHX
    logical :: first_pass

    ! Initialize a few variables.
    temp(1) = T_mc_in
    pres(1) = P_mc_in
    temp(6) = T_t_in
    recomp_cycle%mc%N = N_mc
    recomp_cycle%t%N = N_t
    recomp_cycle%conv_tol = tol

    ! Prepare the mass flow rate iteration loop.
    call CO2_TP(T=temp(1), P=pres(1), error_code=error_code, dens=rho_in)
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 113
        error_trace%files(1) = 3
        return
    end if
    tip_speed = recomp_cycle%mc%D_rotor * 0.5_dp * N_mc * 0.10471975512_dp  ! main compressor tip speed in m/s
    partial_phi = rho_in * recomp_cycle%mc%D_rotor**2 * tip_speed           ! reduces computation on next two lines
    m_dot_mc_guess = recomp_cycle%mc%phi_design * partial_phi               ! mass flow rate corresponding to design-point phi in main compressor
    m_dot_mc_max = recomp_cycle%mc%phi_max * partial_phi * 1.2_dp           ! largest possible mass flow rate in main compressor (with safety factor)
    m_dot_t = m_dot_mc_guess / (1.0_dp - recomp_frac)                       ! first guess for mass flow rate through turbine
    m_dot_upper_bound = m_dot_mc_max / (1.0_dp - recomp_frac)               ! largest possible mass flow rate through turbine
    m_dot_lower_bound = 0.0_dp                                              ! this lower bound allows for surge (checked after iteration)
    first_pass = .true.

    ! Enter the mass flow rate iteration loop.
    m_dot_loop: do m_dot_iter = 1, max_iter
        m_dot_rc = m_dot_t * recomp_frac  ! mass flow rate through recompressing compressor
        m_dot_mc = m_dot_t - m_dot_rc     ! mass flow rate through compressor

        ! Calculate the pressure rise through the main compressor.
        call off_design_compressor(    &
            comp = recomp_cycle%mc,    &
            T_in = temp(1),            &
            P_in = pres(1),            &
            m_dot = m_dot_mc,          &
            N = N_mc,                  &
            error_trace = error_trace, &
            T_out = temp(2),           &
            P_out = pres(2)            &
            )
        if (error_trace%code == 1) then  ! m_dot is too high because the given shaft speed is not possible
            m_dot_upper_bound = m_dot_t
            m_dot_t = (m_dot_lower_bound + m_dot_upper_bound) * 0.5_dp  ! use bisection for new mass flow rate guess
            cycle
        else if (error_trace%code == 2) then  ! m_dot is too low because P_out is (likely) above properties limits
            m_dot_lower_bound = m_dot_t
            m_dot_t = (m_dot_lower_bound + m_dot_upper_bound) * 0.5_dp  ! use bisection for new mass flow rate guess
            cycle
        else if (error_trace%code /= 0) then  ! unexpected error
            index = next_trace_index(error_trace)
            error_trace%lines(index) = 135
            error_trace%files(index) = 3
            return
        end if

        ! Calculate scaled pressure drops through heat exchangers.
        DP_LT  = hxr_pressure_drops(hxr=recomp_cycle%LT, m_dots=[m_dot_mc, m_dot_t])
        DP_HT  = hxr_pressure_drops(hxr=recomp_cycle%HT, m_dots=[m_dot_t, m_dot_t])
        DP_PHX = hxr_pressure_drops(hxr=recomp_cycle%PHX, m_dots=[m_dot_t, 0.0_dp])  ! not concerned with hot stream of PHX
        DP_PC  = hxr_pressure_drops(hxr=recomp_cycle%PC, m_dots=[0.0_dp, m_dot_mc])  ! not concerned with cold stream of precooler

        ! Apply pressure drops to heat exchangers, fully defining the pressures at all states.
        pres(3)  = pres(2) - DP_LT(1)   ! LT recuperator (cold stream)
        pres(4)  = pres(3)              ! assume no pressure drop in mixing valve
        pres(10) = pres(3)              ! assume no pressure drop in mixing valve
        pres(5)  = pres(4) - DP_HT(1)   ! HT recuperator (cold stream)
        pres(6)  = pres(5) - DP_PHX(1)  ! PHX
        pres(9)  = pres(1) + DP_PC(2)   ! precooler
        pres(8)  = pres(9) + DP_LT(2)   ! LT recuperator (hot stream)
        pres(7)  = pres(8) + DP_HT(2)   ! HT recuperator (hot stream)

        ! Calculate the mass flow rate through the turbine.
        call off_design_turbine(       &
            turb = recomp_cycle%t,     &
            T_in = temp(6),            &
            P_in = pres(6),            &
            P_out = pres(7),           &
            N = N_t,                   &
            error_trace = error_trace, &
            m_dot = m_dot_t_allowed,   &
            T_out = temp(7)            &
            )
        if (error_trace%code /= 0) then  ! unexpected error
            index = next_trace_index(error_trace)
            error_trace%lines(index) = 177
            error_trace%files(index) = 3
            return
        end if        

        ! Determine the mass flow rate residual and prepare the next iteration.
        m_dot_residual = m_dot_t - m_dot_t_allowed
        secant_guess = m_dot_t - m_dot_residual * (last_m_dot_guess - m_dot_t) / (last_m_dot_residual - m_dot_residual)  ! next guess predicted using secant method
        if (m_dot_residual > 0.0_dp) then  ! pressure rise is too small, so m_dot_t is too big
            if (m_dot_residual / m_dot_t < tol) exit m_dot_loop  ! residual is positive; check for convergence
            m_dot_upper_bound = m_dot_t   ! reset upper bound
        else  ! pressure rise is too high, so m_dot_t is too small
            if (-m_dot_residual / m_dot_t < tol) exit m_dot_loop ! residual is negative; check for convergence
            m_dot_lower_bound = m_dot_t   ! reset lower bound
        end if
        last_m_dot_residual = m_dot_residual                                ! reset last stored residual value
        last_m_dot_guess = m_dot_t                                    ! reset last stored guess value

        ! Check if the secant method overshoots and fall back to bisection if it does.
        if (first_pass) then
            m_dot_t = (m_dot_upper_bound + m_dot_lower_bound) * 0.5_dp 
            first_pass = .false.
        else if (secant_guess < m_dot_lower_bound .or. secant_guess > m_dot_upper_bound) then  ! secant method overshot, use bisection
            m_dot_t = (m_dot_upper_bound + m_dot_lower_bound) * 0.5_dp 
        else
            m_dot_t = secant_guess
        end if

    end do m_dot_loop
    
    ! Check for convergence.
    if (m_dot_iter >= max_iter) then
        error_trace%code = 42
        error_trace%lines(1) = 220
        error_trace%files(1) = 3
        return
    end if

    ! Fully define known states.
    call CO2_TP(T=temp(1), P=pres(1), error_code=error_code, enth=enth(1), entr=entr(1), dens=dens(1))
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 228
        error_trace%files(1) = 3
        return
    end if
    call CO2_TP(T=temp(2), P=pres(2), error_code=error_code, enth=enth(2), entr=entr(2), dens=dens(2))
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 235
        error_trace%files(1) = 3
        return
    end if
    call CO2_TP(T=temp(6), P=pres(6), error_code=error_code, enth=enth(6), entr=entr(6), dens=dens(6))
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 242
        error_trace%files(1) = 3        
        return
    end if
    call CO2_TP(T=temp(7), P=pres(7), error_code=error_code, enth=enth(7), entr=entr(7), dens=dens(7))
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 249
        error_trace%files(1) = 3
        return
    end if
    
    ! Get the recuperator conductances corresponding to the converged mass flow rates.
    UA_LT = hxr_conductance(hxr=recomp_cycle%LT, m_dots=[m_dot_mc, m_dot_t])
    UA_HT = hxr_conductance(hxr=recomp_cycle%HT, m_dots=[m_dot_t, m_dot_t])

    ! Outer iteration loop: temp(8), checking against UA_HT.
    if (UA_HT < 1.0e-12_dp) then  ! no high-temperature recuperator
        T8_lower_bound = temp(7)  ! no iteration necessary
        T8_upper_bound = temp(7)  ! no iteration necessary
        temp(8) = temp(7)
        UA_HT_calc = 0.0_dp
        last_HT_residual = 0.0_dp
        last_T8_guess = temp(7)
    else
        T8_lower_bound = temp(2)    ! the absolute lowest temp(8) could be
        T8_upper_bound = temp(7)    ! the absolutely highest temp(8) could be
        temp(8) = (T8_lower_bound + T8_upper_bound) * 0.5_dp  ! bisect bounds for first guess
        UA_HT_calc = -1.0_dp
        last_HT_residual = UA_HT    ! know a priori that with T8 = T7, UA_calc = 0 therefore residual is UA_HT - 0.0
        last_T8_guess = temp(7)
    end if
    T8_loop: do T8_iter = 1,max_iter

        ! Fully define state 8.
        call CO2_TP(T=temp(8), P=pres(8), error_code=error_code, enth=enth(8), entr=entr(8), dens=dens(8))
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 280
            error_trace%files(1) = 3        
            return
        end if

        ! Inner iteration loop: temp(9), checking against UA_LT.
        if (UA_LT < 1.0e-12_dp) then  ! no low-temperature recuperator
            T9_lower_bound = temp(8)  ! no iteration necessary
            T9_upper_bound = temp(8)  ! no iteration necessary
            temp(9) = temp(8)
            UA_LT_calc = 0.0_dp
            last_LT_residual = 0.0_dp
            last_T9_guess = temp(8)
        else
            T9_lower_bound = temp(2)    ! the absolute lowest temp(9) could be
            T9_upper_bound = temp(8)    ! the absolutely highest temp(9) could be
            temp(9) = (T9_lower_bound + T9_upper_bound) * 0.5_dp  ! bisect bounds for first guess
            UA_LT_calc = -1.0_dp
            last_LT_residual = UA_LT    ! know a priori that with T9 = T8, UA_calc = 0 therefore residual is UA_LT - 0
            last_T9_guess = temp(8)
        end if
        T9_loop: do T9_iter = 1,max_iter

                call CO2_TP(T=temp(9), P=pres(9), error_code=error_code, enth=enth(9), entr=entr(9), dens=dens(9))  ! fully define state 9
                if (error_code /= 0) then
                    error_trace%code = error_code
                    error_trace%lines(1) = 306
                    error_trace%files(1) = 3
                    return
                end if

                if (recomp_frac >= 1.0e-12_dp) then  ! determine the required shaft speed for the recompressing compressor
                    call off_design_recompressor(  &
                        comp = recomp_cycle%rc,    &
                        T_in = temp(9),            &
                        P_in = pres(9),            &
                        m_dot = m_dot_rc,          &
                        P_out = pres(10),          &
                        error_trace = error_trace, &
                        T_out = temp(10)           &
                        )
                    if (error_trace%code /= 0) then
                        index = next_trace_index(error_trace)
                        error_trace%lines(index) = 315
                        error_trace%files(index) = 3
                        return
                    end if
                    call CO2_TP(T=temp(10), P=pres(10), error_code=error_code, enth=enth(10), entr=entr(10), dens=dens(10))  ! fully define state 10
                    if (error_code /= 0) then
                        error_trace%code = error_code
                        error_trace%lines(1) = 330
                        error_trace%files(1) = 3
                        return
                    end if
                else
                    temp(10) = temp(9)  ! assume state 10 is the same as state 9
                    enth(10) = enth(9)
                    entr(10) = entr(9)
                    dens(10) = dens(9)
                end if

                ! Calculate the UA value of the low-temperature recuperator.
                if (UA_LT < 1.0e-12_dp) then  ! no low-temp recuperator (this check is necessary to prevent pressure drops with UA=0 from causing problems)
                    Q_dot_LT = 0.0_dp
                else
                    Q_dot_LT = m_dot_t * (enth(8) - enth(9))
                end if
                call calculate_hxr_UA(         &
                    N_sub_hxrs = N_sub_hxrs,   &
                    Q_dot = Q_dot_LT,          &
                    m_dot_c = m_dot_mc,        &
                    m_dot_h = m_dot_t,         &
                    T_c_in = temp(2),          &
                    T_h_in = temp(8),          &
                    P_c_in = pres(2),          &
                    P_c_out = pres(3),         &
                    P_h_in = pres(8),          &
                    P_h_out = pres(9),         &
                    error_trace = error_trace, &
                    UA = UA_LT_calc,           &
                    min_DT = min_DT_LT         &
                    )
                if (error_trace%code > 0) then
                    if (error_trace%code == 11) then  ! second-law violation in hxr, therefore temp(9) is too low
                        T9_lower_bound = temp(9)
                        temp(9) = (T9_lower_bound + T9_upper_bound) * 0.5_dp  ! bisect bounds for next guess
                        error_trace%code = 0  ! reset error trace
                        error_trace%lines = 0
                        error_trace%files = 0
                        cycle T9_loop
                    else
                        index = next_trace_index(error_trace)
                        error_trace%lines(index) = 350
                        error_trace%files(index) = 3
                        return
                    end if
                end if

                ! Check for convergence and adjust T9 appropriately.
                UA_LT_residual = UA_LT - UA_LT_calc
                if (abs(UA_LT_residual) < 1.0e-12_dp) exit T9_loop  ! catches no LT case
                secant_guess = temp(9) - UA_LT_residual * (last_T9_guess - temp(9)) / (last_LT_residual - UA_LT_residual)  ! next guess predicted using secant method
                if (UA_LT_residual < 0.0_dp) then  ! UA_LT_calc is too big, temp(9) needs to be higher
                    if (abs(UA_LT_residual)/UA_LT < tol) exit T9_loop  ! UA_LT converged (residual is negative)
                    T9_lower_bound = temp(9)
                else  ! UA_LT_calc is too small, temp(9) needs to be lower
                    if (UA_LT_residual/UA_LT < tol) exit T9_loop  ! UA_LT converged
                    if (min_DT_LT < temperature_tolerance) exit T9_loop  ! UA_calc is still too low but there isn't anywhere to go so it's ok (catches huge UA values)
                    T9_upper_bound = temp(9)
                end if  
                last_LT_residual = UA_LT_residual  ! reset last stored residual value
                last_T9_guess = temp(9)  ! reset last stored guess value

                ! Check if the secant method overshoots and fall back to bisection if it does.
                if (secant_guess <= T9_lower_bound .or. secant_guess >= T9_upper_bound .or. secant_guess /= secant_guess) then  ! secant method overshot (or is NaN), use bisection
                    temp(9) = (T9_lower_bound + T9_upper_bound) * 0.5_dp
                else
                    temp(9) = secant_guess
                end if

        end do T9_loop

        ! Check that T9_loop converged.
        if (T9_iter >= max_iter) then
            error_trace%code = 31
            error_trace%lines(1) = 406
            error_trace%files(1) = 3
            return
        end if

        ! State 3 can now be fully defined.
        enth(3) = enth(2) + Q_dot_LT / m_dot_mc  ! energy balance on cold stream of low-temp recuperator
        call CO2_PH(P=pres(3), H=enth(3), error_code=error_code, temp=temp(3), entr=entr(3), dens=dens(3))
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 415
            error_trace%files(1) = 3        
            return
        end if

        ! Go through mixing valve.
        if (recomp_frac >= 1.0e-12_dp) then
            enth(4) = (1.0_dp - recomp_frac) * enth(3) + recomp_frac * enth(10)  ! conservation of energy (both sides divided by m_dot_t)
            call CO2_PH(P=pres(4), H=enth(4), error_code=error_code, temp=temp(4), entr=entr(4), dens=dens(4))
            if (error_code /= 0) then
                error_trace%code = error_code
                error_trace%lines(1) = 426
                error_trace%files(1) = 3        
                return
            end if 
        else  ! no mixing valve, therefore state 4 is equal to state 3
            temp(4) = temp(3)
            enth(4) = enth(3)
            entr(4) = entr(3)
            dens(4) = dens(3)
        end if

        ! Check for a second law violation at the outlet of the high-temp recuperator.
        if (temp(4) >= temp(8)) then  ! temp(8) is not valid and it must be increased
            T8_lower_bound = temp(8)
            temp(8) = (T8_lower_bound + T8_upper_bound) * 0.5_dp
            cycle T8_loop
        end if

        ! Calculate the UA value of the high-temperature recuperator.
        if (UA_HT < 1.0e-12_dp) then  ! no high-temp recuperator (this check is necessary to prevent pressure drops with UA=0 from causing problems)
            Q_dot_HT = 0.0_dp
        else
            Q_dot_HT = m_dot_t * (enth(7) - enth(8))
        end if
        call calculate_hxr_UA(         &
            N_sub_hxrs = N_sub_hxrs,   &
            Q_dot = Q_dot_HT,          &
            m_dot_c = m_dot_t,         &
            m_dot_h = m_dot_t,         &
            T_c_in = temp(4),          &
            T_h_in = temp(7),          &
            P_c_in = pres(4),          &
            P_c_out = pres(5),         &
            P_h_in = pres(7),          &
            P_h_out = pres(8),         &
            error_trace = error_trace, &
            UA = UA_HT_calc,           &
            min_DT = min_DT_HT         &
            )
        if (error_trace%code > 0) then
            if (error_trace%code == 11) then  ! second-law violation in hxr, therefore temp(8) is too low
                T8_lower_bound = temp(8)
                temp(8) = (T8_lower_bound + T8_upper_bound) * 0.5_dp  ! bisect bounds for next guess
                error_trace%code = 0  ! reset error trace
                error_trace%lines = 0
                error_trace%files = 0
                cycle T8_loop
            else
                index = next_trace_index(error_trace)
                error_trace%lines(index) = 453
                error_trace%files(index) = 3
                return            
            end if
        end if

        ! Check for convergence and adjust T8 appropriately.
        UA_HT_residual = UA_HT - UA_HT_calc
        if (abs(UA_HT_residual) < 1.0e-12_dp) exit T8_loop  ! catches no HT case
        secant_guess = temp(8) - UA_HT_residual * (last_T8_guess - temp(8)) / (last_HT_residual - UA_HT_residual)  ! next guess predicted using secant method
        if (UA_HT_residual < 0.0_dp) then  ! UA_HT_calc is too big, temp(8) needs to be higher
            if (abs(UA_HT_residual)/UA_HT < tol) exit T8_loop  ! UA_HT converged (residual is negative)
            T8_lower_bound = temp(8)
        else  ! UA_HT_calc is too small, temp(8) needs to be lower
            if (UA_HT_residual/UA_HT < tol) exit T8_loop  ! UA_HT converged
            if (min_DT_HT < temperature_tolerance) exit T8_loop  ! UA_calc is still too low but there isn't anywhere to go so it's ok (catches huge UA values)
            T8_upper_bound = temp(8)
        end if  
        last_HT_residual = UA_HT_residual  ! reset last stored residual value
        last_T8_guess = temp(8)  ! reset last stored guess value

        ! Check if the secant method overshoots and fall back to bisection if it does.
        if (secant_guess <= T8_lower_bound .or. secant_guess >= T8_upper_bound) then  ! secant method overshot, use bisection
            temp(8) = (T8_lower_bound + T8_upper_bound) * 0.5_dp
        else
            temp(8) = secant_guess
        end if

    end do T8_loop

    ! Check that T8_loop converged.
    if (T8_iter >= max_iter) then
        error_trace%code = 35
        error_trace%lines(1) = 509
        error_trace%files(1) = 3
        return
    end if

    ! State 5 can now be fully defined.
    enth(5) = enth(4) + Q_dot_HT / m_dot_t  ! energy balance on cold stream of high-temp recuperator
    call CO2_PH(P=pres(5), H=enth(5), error_code=error_code, temp=temp(5), entr=entr(5), dens=dens(5))
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 518
        error_trace%files(1) = 3
        return
    end if

    ! Set cycle state point properties.
    recomp_cycle%temp = temp
    recomp_cycle%pres = pres
    recomp_cycle%enth = enth
    recomp_cycle%entr = entr
    recomp_cycle%dens = dens

    ! Calculate performance metrics for low-temperature recuperator.
    recomp_cycle%LT%C_dot_hot = m_dot_t * (enth(8) - enth(9)) / (temp(8) - temp(9))    ! LT recuperator hot stream capacitance rate
    recomp_cycle%LT%C_dot_cold = m_dot_mc * (enth(3) - enth(2)) / (temp(3) - temp(2))  ! LT recuperator cold stream capacitance rate
    C_dot_min = min(recomp_cycle%LT%C_dot_hot, recomp_cycle%LT%C_dot_cold)
    Q_dot_max = C_dot_min * (temp(8) - temp(2))
    recomp_cycle%LT%eff = Q_dot_LT / Q_dot_max  ! definition of effectiveness
    recomp_cycle%LT%Q_dot = Q_dot_LT
    recomp_cycle%LT%min_DT = min_DT_LT
    recomp_cycle%LT%N_sub = N_sub_hxrs

    ! Calculate performance metrics for high-temperature recuperator.
    recomp_cycle%HT%C_dot_hot = m_dot_t * (enth(7) - enth(8)) / (temp(7) - temp(8))   ! HT recuperator hot stream capacitance rate
    recomp_cycle%HT%C_dot_cold = m_dot_t * (enth(5) - enth(4)) / (temp(5) - temp(4))  ! HT recuperator cold stream capacitance rate
    C_dot_min = min(recomp_cycle%HT%C_dot_hot, recomp_cycle%HT%C_dot_cold)
    Q_dot_max = C_dot_min * (temp(7) - temp(4))
    recomp_cycle%HT%eff = Q_dot_HT / Q_dot_max  ! definition of effectiveness
    recomp_cycle%HT%UA_design = UA_HT_calc
    recomp_cycle%HT%DP_design = [pres(4) - pres(5), pres(7) - pres(8)]
    recomp_cycle%HT%m_dot_design = [m_dot_t, m_dot_t]
    recomp_cycle%HT%Q_dot = Q_dot_HT
    recomp_cycle%HT%min_DT = min_DT_HT
    recomp_cycle%HT%N_sub = N_sub_hxrs

    ! Set relevant values for other heat exchangers.
    recomp_cycle%PHX%Q_dot = m_dot_t * (enth(6) - enth(5))
    recomp_cycle%PC%Q_dot = m_dot_mc * (enth(9) - enth(1))

    ! Calculate cycle performance metrics.
    w_mc = enth(1) - enth(2)  ! specific work of compressor (kJ/kg) [negative]
    w_t = enth(6) - enth(7)   ! specific work of turbine (kJ/kg) [positive]
    if (recomp_frac > 0.0_dp) then
        w_rc = enth(9) - enth(10)  ! specific work of recompressor (kJ/kg) [negative]
    else
        w_rc = 0.0_dp
    end if
    recomp_cycle%W_dot_net = w_mc * m_dot_mc + w_rc * m_dot_rc + w_t * m_dot_t
    recomp_cycle%eta_thermal = recomp_cycle%W_dot_net / recomp_cycle%PHX%Q_dot
    recomp_cycle%recomp_frac = recomp_frac
    recomp_cycle%m_dot_turbine = m_dot_t

end subroutine off_design


subroutine target_off_design(  &
    recomp_cycle,              &  ! [input/output] a RecompCycle object with design-point variables set
    T_mc_in,                   &  ! [input] compressor inlet temperature (K)
    T_t_in,                    &  ! [input] turbine inlet temperature (K)
    recomp_frac,               &  ! [input] recompression fraction
    N_mc,                      &  ! [input] main compressor shaft speed
    N_t,                       &  ! [input] turbine shaft speed
    target,                    &  ! [input] what value to aim for
    target_code,               &  ! [input] type of target: 1) W_dot 2) Q_dot_PHX
    lowest_pressure,           &  ! [input] lowest pressure to check
    highest_pressure,          &  ! [input] highest pressure to check
    N_sub_hxrs,                &  ! [input] number of sub-heat exchangers to use when calculating UA value for a heat exchanger
    tol,                       &  ! [input] convergence tolerance
    error_trace                &  ! [output] an ErrorTrace object
    )
    
    ! Given a target and a target_code, iterate on pressure to match the target.  This subroutine returns an error if the
    ! target is not possible given the other inputs.  Alternatively, 'target_off_design_alt' subroutine can be used to match the
    ! target or get as close as possible to the value. (e.g., if the target is 10 MW but the maximum power output of the cycle is
    ! 8.5 MW, the alternative subroutine will return 8.5 MW, while this subroutine will return an error.)

    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    real(dp), intent(in) :: T_mc_in, T_t_in, recomp_frac, N_mc, N_t, target, lowest_pressure, highest_pressure, tol
    integer, intent(in)  :: target_code, N_sub_hxrs
    type(ErrorTrace), intent(out) :: error_trace

    ! Parameters
    integer, parameter :: max_iter = 100
    integer, parameter :: search_intervals = 20  ! number of intervals to check for valid bounds before starting secant loop

    ! Local Variables
    type(RecompCycle) :: biggest_cycle
    real(dp) :: P_low, P_high, P_guess, left_residual, right_residual, residual, last_P_guess, last_residual, P_secant
    real(dp) :: target_value, biggest_value
    real(dp), dimension(0:search_intervals) :: P_guesses
    integer :: i, iter
    logical :: lower_bound_found, upper_bound_found

    ! Determine the interval containing the solution.
    lower_bound_found = .false.
    upper_bound_found = .false.
    left_residual = -1.0e12_dp  ! initialized to large negative value
    right_residual = 1.0e12_dp  ! initialized to large positive value
    P_low = lowest_pressure
    P_high = highest_pressure
    P_guesses = [ ( P_low + i * (P_high - P_low) / real(search_intervals,dp) , i = 0, search_intervals ) ]  ! create linear vector of guesses
    biggest_value = 0.0_dp
    biggest_cycle = recomp_cycle
    do i = 0, search_intervals
        P_guess = P_guesses(i)
        call off_design(                 &
            recomp_cycle = recomp_cycle, &
            T_mc_in = T_mc_in,           &
            T_t_in = T_t_in,             &
            P_mc_in = P_guess,           &
            recomp_frac = recomp_frac,   &
            N_mc = N_mc,                 &
            N_t = N_t,                   &
            N_sub_hxrs = N_sub_hxrs,     &
            tol = tol,                   &
            error_trace = error_trace    &
            )
        if (error_trace%code == 0) then
            if (recomp_cycle%pres(2) > recomp_cycle%high_pressure_limit * 1.2_dp) exit  ! compressor inlet pressure is getting too big
            select case (target_code)
                case (1); target_value = recomp_cycle%W_dot_net
                case (2); target_value = recomp_cycle%PHX%Q_dot
            end select
            residual = target_value - target
            if (target_value > biggest_value) then  ! keep track of the largest value seen
                biggest_cycle = recomp_cycle
                biggest_value = target_value
            end if
            if (residual >= 0.0_dp) then  ! value is above target
                if (residual < right_residual) then  ! first right bound or a better bound; use it
                    P_high = P_guess
                    right_residual = residual
                    upper_bound_found = .true.
                end if
            else  ! value is below target
                if (residual > left_residual) then  ! note: residual and left_residual are negative
                    P_low = P_guess
                    left_residual = residual
                    lower_bound_found = .true.
                end if
            end if
        end if
        if (lower_bound_found .and. upper_bound_found) exit
    end do

    if (.not. (lower_bound_found .and. upper_bound_found)) then  ! solution not found in interval; return cycle with largest target
        error_trace%code = 26  ! this is a specific code that is used by optimal_target_off_design
        error_trace%lines(1) = 667
        error_trace%files(1) = 3
        recomp_cycle = biggest_cycle
        return
    end if

    ! Enter secant / bisection loop.
    P_guess = (P_low + P_high) * 0.5_dp  ! start with bisection (note: could use left and right bounds and residuals to get a better first guess)
    do iter = 1, max_iter

        call off_design(                 &
            recomp_cycle = recomp_cycle, &
            T_mc_in = T_mc_in,           &
            T_t_in = T_t_in,             &
            P_mc_in = P_guess,           &
            recomp_frac = recomp_frac,   &
            N_mc = N_mc,                 &
            N_t = N_t,                   &
            N_sub_hxrs = N_sub_hxrs,     &
            tol = tol,                   &
            error_trace = error_trace    &
            )
        if (error_trace%code /= 0) then  ! results not valid; choose a random value between P_low and P_high for next guess
            call random_number(P_guess)  ! 0 <= P_guess < 1
            P_guess = P_low + (P_high - P_low) * P_guess
            cycle
        end if

        ! Check residual
        select case (target_code)
            case (1); residual = recomp_cycle%W_dot_net - target  ! W_dot
            case (2); residual = recomp_cycle%PHX%Q_dot - target  ! Q_dot_PHX
        end select
        if (residual >= 0.0_dp) then  ! value is above target
            if (residual / target <= tol) exit  ! converged
            P_high = P_guess
        else  ! value is below target
            if (-residual / target <= tol) exit  ! converged (residual is negative)
            P_low = P_guess
        end if
        if (abs(P_high-P_low) < 0.1_dp) exit  ! interval is tiny; consider it converged

        ! Determine next guess.
        P_secant = P_guess - residual * (last_P_guess - P_guess) / (last_residual - residual)  ! next guess predicted using secant method            
        last_P_guess = P_guess
        last_residual = residual
        P_guess = P_secant
        if (P_guess <= P_low .or. P_guess >= P_high) P_guess = (P_low + P_high) * 0.5_dp  ! secant overshot, use bisection

    end do

    ! Check for convergence.
    if (iter >= max_iter) then
        error_trace%code = 82
        error_trace%lines(1) = 721
        error_trace%files(1) = 3
        return
    end if

end subroutine target_off_design


subroutine target_off_design_alt( &
    recomp_cycle,              &  ! [input/output] a RecompCycle object with design-point variables set
    T_mc_in,                   &  ! [input] compressor inlet temperature (K)
    T_t_in,                    &  ! [input] turbine inlet temperature (K)
    recomp_frac,               &  ! [input] recompression fraction
    N_mc,                      &  ! [input] main compressor shaft speed
    N_t,                       &  ! [input] turbine shaft speed
    target,                    &  ! [input] what value to aim for
    target_code,               &  ! [input] type of target: 1) W_dot 2) Q_dot_PHX
    lowest_pressure,           &  ! [input] lowest pressure to check
    highest_pressure,          &  ! [input] highest pressure to check
    N_sub_hxrs,                &  ! [input] number of sub-heat exchangers to use when calculating UA value for a heat exchanger
    tol,                       &  ! [input] convergence tolerance
    error_trace                &  ! [output] an ErrorTrace object
    )
    
    ! Given a target and a target_code, iterate on pressure to find the minimum residual between the actual and calculated.

    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    real(dp), intent(in) :: T_mc_in, T_t_in, recomp_frac, N_mc, N_t, target, lowest_pressure, highest_pressure, tol
    integer, intent(in)  :: target_code, N_sub_hxrs
    type(ErrorTrace), intent(out) :: error_trace

    ! Parameters
    real(dp), parameter :: fmin_tol = 0.01_dp  ! absolute pressure tolerance

    ! External Functions
    real(dp), external :: fmin

    ! Local Variables
    type(RecompCycle) :: best_recomp_cycle
    real(dp) :: best_residual
    logical  :: solution_found

    solution_found = .false.
    best_residual = 1.0e12_dp
    best_residual = fmin(lowest_pressure, highest_pressure, target_residual, fmin_tol)
    if (solution_found) then
        recomp_cycle = best_recomp_cycle
    else
        error_trace%code = 999
        error_trace%lines(1) = 768
        error_trace%files(1) = 3
    end if

    contains

        real(dp) function target_residual(P_mc_in)
            ! Return the absolute value of the residual between the target and
            ! its calculated value.  No validity checking is performed.
            real(dp), intent(in) :: P_mc_in

            call off_design(                 &
                recomp_cycle = recomp_cycle, &
                T_mc_in = T_mc_in,           &
                T_t_in = T_t_in,             &
                P_mc_in = P_mc_in,           &
                recomp_frac = recomp_frac,   &
                N_mc = N_mc,                 &
                N_t = N_t,                   &
                N_sub_hxrs = N_sub_hxrs,     &
                tol = tol,                   &
                error_trace = error_trace    &
                )
            if (error_trace%code /= 0) then
                target_residual = 1.0e15_dp
                return
            end if

            select case (target_code)
                case (1); target_residual = recomp_cycle%W_dot_net - target  ! W_dot
                case (2); target_residual = recomp_cycle%PHX%Q_dot - target  ! Q_dot_PHX
            end select

            if (abs(target_residual) < abs(best_residual)) then
                solution_found = .true.
                best_residual = target_residual
                best_recomp_cycle = recomp_cycle
            end if

            target_residual = abs(target_residual)

        end function target_residual        

end subroutine target_off_design_alt


subroutine optimal_off_design( &
    recomp_cycle,              &  ! [input/output] a RecompCycle object with design-point variables set
    T_mc_in,                   &  ! [input] compressor inlet temperature (K)
    T_t_in,                    &  ! [input] turbine inlet temperature (K)
    value_code,                &  ! [input] value to maximize: 1) eta, 2) W_dot
    N_sub_hxrs,                &  ! [input] number of sub-heat exchangers to use when calculating UA value for a heat exchanger
    P_mc_in_guess,             &  ! [input] initial guess for P_mc_in when iterating to hit target, or set P_mc_in if value_code is 0
    fixed_P_mc_in,             &  ! [input] if .true., P_mc_in is fixed at P_mc_in_guess
    recomp_frac_guess,         &  ! [input] initial guess for recompression fraction
    fixed_recomp_frac,         &  ! [input] if .true., recomp_frac is fixed at recomp_frac_guess
    N_mc_guess,                &  ! [input] initial guess for main compressor shaft speed
    fixed_N_mc,                &  ! [input] if .true., N_mc is fixed at N_mc_guess
    N_t_guess,                 &  ! [input] initial guess for turbine shaft speed (negative value links it to N_mc)
    fixed_N_t,                 &  ! [input] if .true., N_t is fixed at N_t_guess
    tol,                       &  ! [input] convergence tolerance
    opt_tol,                   &  ! [input] optimization convergence tolerance
    error_trace                &  ! [output] an ErrorTrace object
    )

    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    real(dp), intent(in) :: T_mc_in, T_t_in, P_mc_in_guess, recomp_frac_guess, N_mc_guess, N_t_guess, tol, opt_tol
    logical, intent(in)  :: fixed_P_mc_in, fixed_recomp_frac, fixed_N_mc, fixed_N_t
    integer, intent(in)  :: value_code, N_sub_hxrs
    type(ErrorTrace), intent(out)  :: error_trace

    ! Subplex Parameters and Variables
    integer, parameter :: maxf = 200
    integer, parameter :: max_free_vars = 4
    integer  :: iflag, iwork(50), mode, nfe
    real(dp) :: subplex_fmin, scale(max_free_vars), work(50), x(max_free_vars)

    ! Local Variables
    real(dp) :: largest_value, N_t_local
    integer  :: n, index
    logical  :: solution_found
    type(RecompCycle) :: optimal_cycle

    ! Initialize guess array.
    x = 0.0_dp
    index = 1
    if (.not. fixed_P_mc_in) then
        x(index) = P_mc_in_guess
        scale(index) = 50.0_dp  ! P_mc_in scale (may need to be adjusted if optimal results are not satisfactory)
        index = index + 1
    end if    
    if (.not. fixed_recomp_frac) then
        x(index) = recomp_frac_guess
        scale(index) = 0.01_dp   ! recomp scale (may need to be adjusted if optimal results are not satisfactory)
        index = index + 1
    end if
    if (.not. fixed_N_mc) then
        x(index) = N_mc_guess
        scale(index) = 100.0_dp   ! N_mc_scale (may need to be adjusted if optimal results are not satisfactory)
        index = index + 1
    end if    
    if (.not. fixed_N_t) then
        x(index) = N_t_guess
        scale(index) = 100.0_dp   ! N_t_scale (may need to be adjusted if optimal results are not satisfactory)
        index = index + 1
    end if
    n = index - 1  

    if (n > 0) then  ! need to call subplex
        solution_found = .false.
        largest_value = 0.0_dp
        mode = 0
        call subplx(off_design_point_value, n, opt_tol, maxf, mode, scale, x, subplex_fmin, nfe, work, iwork, iflag)
        if (solution_found) then
            recomp_cycle = optimal_cycle
            error_trace%code = 0
            error_trace%lines = 0
            error_trace%files = 0
        else
            error_trace%code = 111
            error_trace%lines(1) = 886
            error_trace%files(1) = 3
            return
        end if
    else  ! just call off_design subroutine (with fixed inputs)
        if (N_t_guess <= 0.0_dp) then
            N_t_local = N_mc_guess  ! link turbine and main compressor shafts
        else
            N_t_local = N_t_guess
        end if
        call off_design(                     &
            recomp_cycle = recomp_cycle,     &
            T_mc_in = T_mc_in,               &
            T_t_in = T_t_in,                 &
            P_mc_in = P_mc_in_guess,         &
            recomp_frac = recomp_frac_guess, &
            N_mc = N_mc_guess,               &
            N_t = N_t_local,                 &
            N_sub_hxrs = N_sub_hxrs,         &
            tol = tol,                       &
            error_trace = error_trace        &
            )
        if (error_trace%code == 0) then  ! check validity of results
            solution_found = recomp_cycle%pres(2) <= recomp_cycle%high_pressure_limit  ! high-pressure limit
            if (.not. surge_allowed) then
                if (recomp_cycle%mc%surge) solution_found = .false.
                if (recomp_cycle%recomp_frac > 0.0_dp .and. recomp_cycle%rc%surge) solution_found = .false.
            end if
            if (.not. supersonic_tip_speed_allowed) then
                if (recomp_cycle%mc%w_tip_ratio > 1.0_dp) solution_found = .false.
                if (recomp_cycle%recomp_frac > 0.0_dp .and. recomp_cycle%rc%w_tip_ratio > 1.0_dp) solution_found = .false.
                if (recomp_cycle%t%w_tip_ratio > 1.0_dp) solution_found = .false.
            end if
            if (.not. solution_found) then
                error_trace%code = 112
                error_trace%lines(1) = 904
                error_trace%files(1) = 3
            end if
        end if
    end if

    contains

        real(dp) function off_design_point_value(n, x)
            ! Call the off_design subroutine with inputs contained in the x array.
            ! Returns the power output or thermal efficiency, depending on the value code.
            integer, intent(in)  :: n     ! number of inputs that are varied during optimization
            real(dp), intent(in) :: x(n)  ! inputs with order: recomp_frac, N_mc, N_t (some can be missing)
            real(dp) :: P_mc_in_local, recomp_frac_local, N_mc_local, N_t_local

            ! Extract input variables from x.
            index = 1
            if (.not. fixed_P_mc_in) then
                P_mc_in_local = x(index)
                index = index + 1
            else
                P_mc_in_local = P_mc_in_guess
            end if        
            if (.not. fixed_recomp_frac) then
                recomp_frac_local = x(index)
                index = index + 1
            else
                recomp_frac_local = recomp_frac_guess
            end if            
            if (.not. fixed_N_mc) then
                N_mc_local = x(index)
                index = index + 1
            else
                N_mc_local = N_mc_guess
            end if
            if (.not. fixed_N_t) then
                N_t_local = x(index)
                index = index + 1
            else
                N_t_local = N_t_guess
            end if
            if (N_t_local <= 0.0_dp) N_t_local = N_mc_local  ! link turbine and main compressor shafts

            ! Check inputs.
            if (recomp_frac_local < 0.0_dp) then
                off_design_point_value = 0.0_dp
                return
            end if

            ! Call off_design subroutine.
            call off_design(                     &
                recomp_cycle = recomp_cycle,     &
                T_mc_in = T_mc_in,               &
                T_t_in = T_t_in,                 &
                P_mc_in = P_mc_in_local,         &
                recomp_frac = recomp_frac_local, &
                N_mc = N_mc_local,               &
                N_t = N_t_local,                 &
                N_sub_hxrs = N_sub_hxrs,         &
                tol = tol,                       &
                error_trace = error_trace        &
                )
            if (error_trace%code /= 0) then
                off_design_point_value = 0.0_dp 
                return
            end if
            select case (value_code)
                case (1); off_design_point_value = -recomp_cycle%eta_thermal
                case (2); off_design_point_value = -recomp_cycle%W_dot_net
            end select

            ! Check validity.
            if (recomp_cycle%pres(2) > recomp_cycle%high_pressure_limit) then  ! above high-pressure limit; provide optimizer with more information
                off_design_point_value = off_design_point_value / (10_dp + recomp_cycle%pres(2) - recomp_cycle%high_pressure_limit)
            end if
            if (.not. surge_allowed) then
                if (recomp_cycle%mc%surge) off_design_point_value = 0.0_dp
                if (recomp_cycle%recomp_frac > 0.0_dp .and. recomp_cycle%rc%surge) off_design_point_value = 0.0_dp
            end if
            if (.not. supersonic_tip_speed_allowed) then
                if (recomp_cycle%mc%w_tip_ratio > 1.0_dp) off_design_point_value = 0.0_dp
                if (recomp_cycle%recomp_frac > 0.0_dp .and. recomp_cycle%rc%w_tip_ratio > 1.0_dp) off_design_point_value = 0.0_dp
                if (recomp_cycle%t%w_tip_ratio > 1.0_dp) off_design_point_value = 0.0_dp
            end if

            ! Check if this is the optimal cycle.
            if (abs(off_design_point_value) > largest_value) then
                solution_found = .true.
                optimal_cycle = recomp_cycle
                largest_value = abs(off_design_point_value)
            end if

        end function off_design_point_value

end subroutine optimal_off_design


subroutine optimal_target_off_design( &
    recomp_cycle,                     &  ! [input/output] a RecompCycle object with design-point variables set
    T_mc_in,                          &  ! [input] compressor inlet temperature (K)
    T_t_in,                           &  ! [input] turbine inlet temperature (K)
    target,                           &  ! [input] target value for W_dot_net or Q_dot_PHX (kW)
    target_code,                      &  ! [input] type of optimization: 1) target W_dot (max eta), 2) target Q_dot_PHX (max eta)
    N_sub_hxrs,                       &  ! [input] number of sub-heat exchangers to use when calculating UA value for a hxr
    lowest_pressure,                  &  ! [input] the lowest pressure to check
    highest_pressure,                 &  ! [input] the highest pressure to check
    recomp_frac_guess,                &  ! [input] initial guess for recompression fraction
    fixed_recomp_frac,                &  ! [input] if .true., recomp_frac is fixed at recomp_frac_guess
    N_mc_guess,                       &  ! [input] initial guess for main compressor shaft speed
    fixed_N_mc,                       &  ! [input] if .true., N_mc is fixed at N_mc_guess
    N_t_guess,                        &  ! [input] initial guess for turbine shaft speed (negative value links it to N_mc)
    fixed_N_t,                        &  ! [input] if .true., N_t is fixed at N_t_guess
    tol,                              &  ! [input] convergence tolerance
    opt_tol,                          &  ! [input] optimization convergence tolerance
    error_trace                       &  ! [output] an ErrorTrace object
    )
    
    ! Arguments
    type(RecompCycle), intent(inout) :: recomp_cycle
    real(dp), intent(in) :: T_mc_in, T_t_in, target, lowest_pressure, highest_pressure, recomp_frac_guess, N_mc_guess, N_t_guess
    real(dp), intent(in) :: tol, opt_tol
    logical, intent(in)  :: fixed_recomp_frac, fixed_N_mc, fixed_N_t
    integer, intent(in)  :: target_code, N_sub_hxrs
    type(ErrorTrace), intent(out)  :: error_trace

    ! Subplex Parameters and Variables
    integer, parameter :: maxf = 200
    integer, parameter :: max_free_vars = 3
    integer  :: iflag, iwork(50), mode, nfe
    real(dp) :: subplex_fmin, scale(max_free_vars), work(50), x(max_free_vars)

    ! Local Variables
    type(RecompCycle) :: best_recomp_cycle
    real(dp) :: best_eta, biggest_target, P_low, unused_var
    integer  :: index, n
    logical  :: solution_found, point_found

    ! Determine the largest possible power output of the cycle.
    point_found = .false.
    P_low = lowest_pressure
    do
        call optimal_off_design(                   &
            recomp_cycle = recomp_cycle,           &
            T_mc_in = T_mc_in,                     &
            T_t_in = T_t_in,                       &
            value_code = 2,                        &  ! max W_dot
            N_sub_hxrs = N_sub_hxrs,               &
            P_mc_in_guess = P_low,                 &
            fixed_P_mc_in = .false.,               &
            recomp_frac_guess = recomp_frac_guess, &
            fixed_recomp_frac = fixed_recomp_frac, &
            N_mc_guess = N_mc_guess,               &
            fixed_N_mc = fixed_N_mc,               &
            N_t_guess = N_t_guess,                 &
            fixed_N_t = fixed_N_t,                 &
            tol = tol,                             &
            opt_tol = opt_tol,                     &
            error_trace = error_trace              &
            )
        if (error_trace%code == 0) then
            if (point_found) exit  ! exit only after testing two starting points (prevents optimization near-misses)
            point_found = .true.
        end if
        P_low = P_low + 500.0_dp
        if (P_low > highest_pressure) exit
    end do

    if (.not. point_found) then  ! this is an unexpected error
        error_trace%code = 99
        error_trace%lines(1) = 1096
        error_trace%files(1) = 3
        return
    end if

    select case (target_code)
        case (1); biggest_target = recomp_cycle%W_dot_net
        case (2); biggest_target = recomp_cycle%PHX%Q_dot
    end select
        
    ! If the target is not possible, return the cycle with the largest (based on power output).
    if (biggest_target <= target) then
        error_trace%code = 0  ! reset error code
        error_trace%lines = 0
        error_trace%files = 0
        return
    end if

    ! Initialize guess array.
    x = 0.0_dp
    index = 1  
    if (.not. fixed_recomp_frac) then
        x(index) = recomp_frac_guess
        scale(index) = 0.01_dp   ! recomp scale
        index = index + 1
    end if
    if (.not. fixed_N_mc) then
        x(index) = N_mc_guess
        scale(index) = 100.0_dp   ! N_mc_scale
        index = index + 1
    end if    
    if (.not. fixed_N_t) then
        x(index) = N_t_guess
        scale(index) = 100.0_dp   ! N_t_scale
        index = index + 1
    end if
    n = index - 1      
    solution_found = .false.
    best_eta = 0.0_dp
    if (n > 0) then  ! call subplex
        mode = 0
        call subplx(eta_at_target, n, opt_tol, maxf, mode, scale, x, subplex_fmin, nfe, work, iwork, iflag)
    else
        unused_var = eta_at_target(n, x)  ! necessary to get recomp_cycle at target (ignores x array) [warning: somewhat untested]
    end if
    if (.not. solution_found) then
        error_trace%code = 98
        error_trace%lines(1) = 1143
        error_trace%files(1) = 3
        return
    end if

    return

    contains

        real(dp) function eta_at_target(n, x)
            ! Call the target_off_design subroutine with inputs contained in the x array.
            ! Returns (negative) thermal efficiency.
            integer, intent(in)  :: n     ! number of inputs that are varied during optimization
            real(dp), intent(in) :: x(n)  ! inputs with order: recomp_frac, N_mc, N_t (some can be missing)
            real(dp) :: recomp_frac_local, N_mc_local, N_t_local

            ! Extract input variables from x.
            index = 1    
            if (.not. fixed_recomp_frac) then
                recomp_frac_local = x(index)
                index = index + 1
            else
                recomp_frac_local = recomp_frac_guess
            end if            
            if (.not. fixed_N_mc) then
                N_mc_local = x(index)
                index = index + 1
            else
                N_mc_local = N_mc_guess
            end if
            if (.not. fixed_N_t) then
                N_t_local = x(index)
                index = index + 1
            else
                N_t_local = N_t_guess
            end if
            if (N_t_local <= 0.0_dp) N_t_local = N_mc_local  ! link turbine and main compressor shafts if necessary

            ! Check inputs.
            if (recomp_frac_local < 0.0_dp) then
                eta_at_target = 0.0_dp
                return
            end if

            ! Call target_off_design subroutine.
            call target_off_design(                  &
                recomp_cycle = recomp_cycle,         &
                T_mc_in = T_mc_in,                   &
                T_t_in = T_t_in,                     &
                recomp_frac = recomp_frac_local,     &
                N_mc = N_mc_local,                   &
                N_t = N_t_local,                     &
                target = target,                     &
                target_code = target_code,           &
                lowest_pressure = lowest_pressure,   &
                highest_pressure = highest_pressure, &
                N_sub_hxrs = N_sub_hxrs,             &
                tol = tol,                           &
                error_trace = error_trace            &
                )
            if (error_trace%code == 26) then  ! could not hit target
                eta_at_target = 1.0_dp / (100.0_dp + abs(recomp_cycle%W_dot_net))  ! provides a directional hint to optimizer
                return
            else if (error_trace%code /= 0) then  ! uncaught error
                eta_at_target = 0.0_dp 
                return
            else
                eta_at_target = recomp_cycle%eta_thermal
            end if

            ! Check validity.
            if (recomp_cycle%pres(2) > recomp_cycle%high_pressure_limit) then
                eta_at_target = eta_at_target / (10.0_dp + recomp_cycle%pres(2) - recomp_cycle%high_pressure_limit)  ! provides a directional hint to optimizer
            end if
            if (.not. surge_allowed) then
                if (recomp_cycle%mc%surge) eta_at_target = 0.0_dp
                if (recomp_cycle%recomp_frac > 0.0_dp .and. recomp_cycle%rc%surge) eta_at_target = 0.0_dp
            end if
            if (.not. supersonic_tip_speed_allowed) then
                if (recomp_cycle%mc%w_tip_ratio > 1.0_dp) eta_at_target = 0.0_dp
                if (recomp_cycle%recomp_frac > 0.0_dp .and. recomp_cycle%rc%w_tip_ratio > 1.0_dp) eta_at_target = 0.0_dp
                if (recomp_cycle%t%w_tip_ratio > 1.0_dp) eta_at_target = 0.0_dp
            end if

            ! Check if this is the best solution.
            if (eta_at_target > best_eta) then
                best_eta = eta_at_target
                best_recomp_cycle = recomp_cycle
                solution_found = .true.
            end if

            eta_at_target = -eta_at_target  ! subplex is minimizer

        end function eta_at_target

end subroutine optimal_target_off_design


end module off_design_point
