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
! This file contains the the module 'design_point', which defines three system-level subroutines:
!   design -- the main design-point model
!   optimal_design -- calls 'design' and incorporates optimization in order to maximize thermal efficiency by varying model inputs
!   auto_optimal_design -- calls 'optimal_design' with multiple starting points in an effort to find the global maximum for thermal
!                          efficiency, but is significantly slower (if you have a good idea what the design point should be, it is
!                          better to use 'optimal_design' with appropriate initial guesses for the inputs)
!
! Notes:
!   1) W_dot_net must be positive.
!   2) Pressure drops are specified per heat exchanger, with stream 1 being the cold stream and stream 2 being the hot stream.
!      Positive values are absolute pressure drops and negative values are relative pressure drops: abs(rel_DP) * P_in = DP.
!   3) Positive values for turbomachinery efficiencies are treated as isentropic, while negative values are treated as polytropic
!      efficiencies (after taking the absolute value).  Using polytropic efficiencies is significantly slower than isentropic.
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
! Last Modified: August 15, 2014
!
!-----------------------------------------------------------------------------------------------------------------------------------

module design_point

use core
implicit none
private
public :: design, optimal_design, auto_optimal_design

contains

subroutine design( &
    W_dot_net,     &  ! [input] target net cycle power (kW)
    T_mc_in,       &  ! [input] compressor inlet temperature (K)
    T_t_in,        &  ! [input] turbine inlet temperature (K)
    P_mc_in,       &  ! [input] compressor inlet pressure (kPa)
    P_mc_out,      &  ! [input] compressor outlet pressure (kPa)
    DP_LT,         &  ! [input] pressure drops in low-temperature recuperator (kPa if positive values)
    DP_HT,         &  ! [input] pressure drops in high-temperature recuperator (kPa if positive values)
    DP_PC,         &  ! [input] pressure drops in precooler (kPa if positive values)
    DP_PHX,        &  ! [input] pressure drops in primary heat exchanger (kPa if positive values)
    UA_LT,         &  ! [input] design-point UA value for the low-temperature recuperator (kW/K)
    UA_HT,         &  ! [input] design-point UA value for the high-temperature recuperator (kW/K)
    recomp_frac,   &  ! [input] fraction of flow that bypasses the precooler and main compressor at the design point
    eta_mc,        &  ! [input] design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
    eta_rc,        &  ! [input] design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
    eta_t,         &  ! [input] design-point efficiency of the turbine; isentropic if positive, polytropic if negative
    N_sub_hxrs,    &  ! [input] number of sub-heat exchangers to use when calculating UA value for a heat exchanger
    tol,           &  ! [input] convergence tolerance
    error_trace,   &  ! [output] an ErrorTrace object
    recomp_cycle   &  ! [output] a RecompCycle object
    )

    use CO2_Properties, only: CO2_TP, CO2_PH

    ! Arguments
    real(dp), intent(in) :: W_dot_net, T_mc_in, T_t_in, P_mc_in, P_mc_out, UA_LT, UA_HT, recomp_frac
    real(dp), intent(in) :: eta_mc, eta_rc, eta_t, tol
    integer, intent(in)  :: N_sub_hxrs
    real(dp), dimension(2), intent(in) :: DP_LT, DP_HT, DP_PC, DP_PHX
    type(ErrorTrace), intent(out)  :: error_trace
    type(RecompCycle), intent(out) :: recomp_cycle

    ! Parameters
    integer, parameter :: max_iter = 500
    real(dp), parameter :: temperature_tolerance = 1.0e-6_dp  ! temperature differences below this are considered zero

    ! Local Variables
    integer  :: T9_iter, T8_iter, error_code, index
    real(dp) :: w_mc, w_rc, w_t, C_dot_min, Q_dot_max
    real(dp) :: T9_lower_bound, T9_upper_bound, T8_lower_bound, T8_upper_bound, last_LT_residual, last_T9_guess
    real(dp) :: last_HT_residual, last_T8_guess, secant_guess
    real(dp) :: m_dot_t, m_dot_mc, m_dot_rc, eta_mc_isen, eta_rc_isen, eta_t_isen
    real(dp) :: min_DT_LT, min_DT_HT, UA_LT_calc, UA_HT_calc, Q_dot_LT, Q_dot_HT, UA_HT_residual, UA_LT_residual
    real(dp), dimension(10) :: temp, pres, enth, entr, dens

    ! Initialize a few variables.
    m_dot_t = 0.0_dp
    m_dot_mc = 0.0_dp
    m_dot_rc = 0.0_dp
    Q_dot_LT = 0.0_dp
    Q_dot_HT = 0.0_dp
    UA_LT_calc = 0.0_dp
    UA_HT_calc = 0.0_dp
    temp(1) = T_mc_in
    pres(1) = P_mc_in
    pres(2) = P_mc_out
    temp(6) = T_t_in

    ! Apply pressure drops to heat exchangers, fully defining the pressures at all states.
    if (DP_LT(1) < 0.0_dp) then
        pres(3) = pres(2) - pres(2) * abs(DP_LT(1))   ! relative pressure drop specified for LT recuperator (cold stream)
    else
        pres(3) = pres(2) - DP_LT(1)                  ! absolute pressure drop specified for LT recuperator (cold stream)
    end if
    if (UA_LT < 1.0e-12_dp) pres(3) = pres(2)         ! if there is no LT recuperator, there is no pressure drop
    pres(4) = pres(3)                                 ! assume no pressure drop in mixing valve
    pres(10) = pres(3)                                ! assume no pressure drop in mixing valve
    if (DP_HT(1) < 0.0_dp) then
        pres(5) = pres(4) - pres(4) * abs(DP_HT(1))   ! relative pressure drop specified for HT recuperator (cold stream)
    else
        pres(5) = pres(4) - DP_HT(1)                  ! absolute pressure drop specified for HT recuperator (cold stream)
    end if
    if (UA_HT < 1.0e-12_dp) pres(5) = pres(4)         ! if there is no HT recuperator, there is no pressure drop
    if (DP_PHX(1) < 0.0_dp) then
        pres(6) = pres(5) - pres(5) * abs(DP_PHX(1))  ! relative pressure drop specified for PHX
    else
        pres(6) = pres(5) - DP_PHX(1)                 ! absolute pressure drop specified for PHX
    end if
    if (DP_PC(2) < 0.0_dp) then
        pres(9) = pres(1) / (1.0_dp - abs(DP_PC(2)))  ! relative pressure drop specified for precooler: P1=P9-P9*rel_DP => P1=P9*(1-rel_DP)
    else
        pres(9) = pres(1) + DP_PC(2)                  ! absolute pressure drop specified for precooler
    end if
    if (DP_LT(2) < 0.0_dp) then
        pres(8) = pres(9) / (1.0_dp - abs(DP_LT(2)))  ! relative pressure drop specified for LT recuperator (hot stream)
    else
        pres(8) = pres(9) + DP_LT(2)                  ! absolute pressure drop specified for LT recuperator (hot stream)
    end if
    if (UA_LT < 1.0e-12_dp) pres(8) = pres(9)         ! if there is no LT recuperator, there is no pressure drop
    if (DP_HT(2) < 0.0_dp) then
        pres(7) = pres(8) / (1.0_dp - abs(DP_HT(2)))  ! relative pressure drop specified for HT recuperator (hot stream)
    else
        pres(7) = pres(8) + DP_HT(2)                  ! absolute pressure drop specified for HT recuperator (hot stream)
    end if
    if (UA_HT < 1.0e-12_dp) pres(7) = pres(8)         ! if there is no HT recuperator, there is no pressure drop

    ! Determine equivalent isentropic efficiencies for main compressor and turbine, if necessary.
    if (eta_mc < 0.0_dp) then
        call isen_eta_from_poly_eta(   &
            T_in = temp(1),            &
            P_in = pres(1),            &
            P_out = pres(2),           &
            poly_eta = abs(eta_mc),    &
            is_comp = .true.,          &
            error_trace = error_trace, &
            isen_eta = eta_mc_isen     &
            )
        if (error_trace%code /= 0) then
            index = next_trace_index(error_trace)
            error_trace%lines(index) = 154
            error_trace%files(index) = 2
            return
        end if
    else
        eta_mc_isen = eta_mc
    end if
    if (eta_t < 0.0_dp) then
        call isen_eta_from_poly_eta(   &
            T_in = temp(6),            &
            P_in = pres(6),            &
            P_out = pres(7),           &
            poly_eta = abs(eta_t),     &
            is_comp = .false.,         &
            error_trace = error_trace, &
            isen_eta = eta_t_isen      &
            )
        if (error_trace%code /= 0) then
            index = next_trace_index(error_trace)
            error_trace%lines(index) = 173
            error_trace%files(index) = 2
            return
        end if
    else
        eta_t_isen = eta_t
    end if

    ! Determine the outlet state and specific work for the main compressor and turbine.
    call calculate_turbomachine_outlet( &  ! main compressor
        T_in = temp(1),                 &
        P_in = pres(1),                 &
        P_out = pres(2),                &
        eta = eta_mc_isen,              &
        is_comp = .true.,               &
        error_trace = error_trace,      &
        enth_in = enth(1),              &
        entr_in = entr(1),              &
        dens_in = dens(1),              &
        temp_out = temp(2),             &
        enth_out = enth(2),             &
        entr_out = entr(2),             &
        dens_out = dens(2),             &
        spec_work = w_mc                &
        )
    if (error_trace%code /= 0) then
        index = next_trace_index(error_trace)
        error_trace%lines(index) = 193
        error_trace%files(index) = 2
        return
    end if
    call calculate_turbomachine_outlet( &  ! turbine
        T_in = temp(6),                 &
        P_in = pres(6),                 &
        P_out = pres(7),                &
        eta = eta_t_isen,               &
        is_comp = .false.,              &
        error_trace = error_trace,      &
        enth_in = enth(6),              &
        entr_in = entr(6),              &
        dens_in = dens(6),              &
        temp_out = temp(7),             &
        enth_out = enth(7),             &
        entr_out = entr(7),             &
        dens_out = dens(7),             &
        spec_work = w_t                 &
        )
    if (error_trace%code /= 0) then
        index = next_trace_index(error_trace)
        error_trace%lines(index) = 215
        error_trace%files(index) = 2
        return
    end if

    ! Check that this cycle can produce power.
    if (recomp_frac >= 1.0d-12) then
        if (eta_rc < 0.0_dp) then  ! need to convert polytropic efficiency to isentropic efficiency
            call isen_eta_from_poly_eta(   &
                T_in = temp(2),            &  ! lowest possible recompressor work occurs when temp(9) == temp(2)
                P_in = pres(9),            &
                P_out = pres(10),          &
                poly_eta = abs(eta_rc),    &
                is_comp = .true.,          &
                error_trace = error_trace, &
                isen_eta = eta_rc_isen     &
                )
            if (error_trace%code /= 0) then
                index = next_trace_index(error_trace)
                error_trace%lines(index) = 241
                error_trace%files(index) = 2
                return
            end if
        else
            eta_rc_isen = eta_rc
        end if
        call calculate_turbomachine_outlet( &
            T_in = temp(2),                 &  ! lowest possible recompressor work occurs when temp(9) == temp(2)
            P_in = pres(9),                 &
            P_out = pres(10),               &
            eta = eta_rc_isen,              &
            is_comp = .true.,               &
            error_trace = error_trace,      &
            spec_work = w_rc                &
            )
        if (error_trace%code /= 0) then
            index = next_trace_index(error_trace)
            error_trace%lines(index) = 259
            error_trace%files(index) = 2
            return
        end if
    else
        w_rc = 0.0_dp
    end if
    if (w_mc + w_rc + w_t <= 0.0_dp) then  ! positive net power is impossible; return an error
        error_trace%code = 25
        error_trace%lines(1) = 277
        error_trace%files(1) = 2
        return
    end if

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
            error_trace%lines(1) = 303
            error_trace%files(1) = 2
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

                ! Determine the outlet state of the recompressing compressor and its specific work.
                if (recomp_frac >= 1.0e-12_dp) then
                    if (eta_rc < 0.0_dp) then  ! recalculate isentropic efficiency of recompressing compressor (because T9 changes)
                        call isen_eta_from_poly_eta(   &
                            T_in = temp(9),            &
                            P_in = pres(9),            &
                            P_out = pres(10),          &
                            poly_eta = abs(eta_rc),    &
                            is_comp = .true.,          &
                            error_trace = error_trace, &
                            isen_eta = eta_rc_isen     &
                            )
                        if (error_trace%code /= 0) then
                            index = next_trace_index(error_trace)
                            error_trace%lines(index) = 332
                            error_trace%files(index) = 2
                            return
                        end if
                    else
                        eta_rc_isen = eta_rc
                    end if
                    call calculate_turbomachine_outlet( &
                        T_in = temp(9),                 &
                        P_in = pres(9),                 &
                        P_out = pres(10),               &
                        eta = eta_rc_isen,              &
                        is_comp = .true.,               &
                        error_trace = error_trace,      &
                        enth_in = enth(9),              &
                        entr_in = entr(9),              &
                        dens_in = dens(9),              &
                        temp_out = temp(10),            &
                        enth_out = enth(10),            &
                        entr_out = entr(10),            &
                        dens_out = dens(10),            &
                        spec_work = w_rc                &
                        )
                    if (error_trace%code /= 0) then
                        index = next_trace_index(error_trace)
                        error_trace%lines(index) = 350
                        error_trace%files(index) = 2
                        return
                    end if
                else
                    w_rc = 0.0_dp  ! the recompressing compressor does not exist
                    call CO2_TP(T=temp(9), P=pres(9), error_code=error_code, enth=enth(9), entr=entr(9), dens=dens(9))  ! fully define state 9
                    if (error_code /= 0) then
                        error_trace%code = error_code
                        error_trace%lines(1) = 374
                        error_trace%files(1) = 2
                        return
                    end if
                    temp(10) = temp(9)  ! assume state 10 is the same as state 9
                    enth(10) = enth(9)
                    entr(10) = entr(9)
                    dens(10) = dens(9)
                end if

                ! Knowing the specific work of the the recompressing compressor, the required mass flow rate can be calculated.
                m_dot_t = W_dot_net / (w_mc * (1.0_dp - recomp_frac) + w_rc * recomp_frac + w_t)  ! required mass flow rate through turbine
                if (m_dot_t < 0.0_dp) then  ! positive power output is not possible with these inputs
                    error_trace%code = 29
                    error_trace%lines(1) = 389
                    error_trace%files(1) = 2
                    return
                end if
                m_dot_rc = m_dot_t * recomp_frac  ! apply definition of recompression fraction
                m_dot_mc = m_dot_t - m_dot_rc     ! mass balance

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
                        error_trace%lines(index) = 404
                        error_trace%files(index) = 2
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
            error_trace%lines(1) = 460
            error_trace%files(1) = 2
            return
        end if

        ! State 3 can now be fully defined.
        enth(3) = enth(2) + Q_dot_LT / m_dot_mc  ! energy balance on cold stream of low-temp recuperator
        call CO2_PH(P=pres(3), H=enth(3), error_code=error_code, temp=temp(3), entr=entr(3), dens=dens(3))
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 469
            error_trace%files(1) = 2        
            return
        end if

        ! Go through the mixing valve.
        if (recomp_frac >= 1.0e-12_dp) then
            enth(4) = (1.0_dp - recomp_frac) * enth(3) + recomp_frac * enth(10)  ! conservation of energy (both sides divided by m_dot_t)
            call CO2_PH(P=pres(4), H=enth(4), error_code=error_code, temp=temp(4), entr=entr(4), dens=dens(4))
            if (error_code /= 0) then
                error_trace%code = error_code
                error_trace%lines(1) = 480
                error_trace%files(1) = 2        
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
        if (UA_HT < 1.0e-12_dp) then  ! no high-temp recuperator
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
                error_trace%lines(index) = 507
                error_trace%files(index) = 2
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
        error_trace%lines(1) = 563
        error_trace%files(1) = 2
        return
    end if

    ! State 5 can now be fully defined.
    enth(5) = enth(4) + Q_dot_HT / m_dot_t  ! energy balance on cold stream of high-temp recuperator
    call CO2_PH(P=pres(5), H=enth(5), error_code=error_code, temp=temp(5), entr=entr(5), dens=dens(5))
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 572
        error_trace%files(1) = 2
        return
    end if

    ! Set cycle state point properties.
    recomp_cycle%temp = temp
    recomp_cycle%pres = pres
    recomp_cycle%enth = enth
    recomp_cycle%entr = entr
    recomp_cycle%dens = dens

    ! Calculate performance metrics for low-temperature recuperator.
    recomp_cycle%LT%C_dot_cold = m_dot_mc * (enth(3) - enth(2)) / (temp(3) - temp(2))  ! LT recuperator cold stream capacitance rate
    recomp_cycle%LT%C_dot_hot = m_dot_t * (enth(8) - enth(9)) / (temp(8) - temp(9))  ! LT recuperator cold stream capacitance rate
    C_dot_min = min(recomp_cycle%LT%C_dot_hot, recomp_cycle%LT%C_dot_cold)
    Q_dot_max = C_dot_min * (temp(8) - temp(2))
    recomp_cycle%LT%eff = Q_dot_LT / Q_dot_max  ! definition of effectiveness
    recomp_cycle%LT%UA_design = UA_LT_calc
    recomp_cycle%LT%UA = UA_LT_calc
    recomp_cycle%LT%DP_design = [pres(2) - pres(3), pres(8) - pres(9)]
    recomp_cycle%LT%m_dot_design = [m_dot_mc, m_dot_t]
    recomp_cycle%LT%T_c_in= temp(2)
    recomp_cycle%LT%T_h_in= temp(8)
    recomp_cycle%LT%P_c_in= pres(2)
    recomp_cycle%LT%P_h_in= pres(8)
    recomp_cycle%LT%P_c_out= pres(3)
    recomp_cycle%LT%P_h_out= pres(9)
    recomp_cycle%LT%Q_dot = Q_dot_LT
    recomp_cycle%LT%min_DT = min_DT_LT
    recomp_cycle%LT%N_sub = N_sub_hxrs

    ! Calculate performance metrics for HTR high-temperature recuperator.
    recomp_cycle%HT%C_dot_hot = m_dot_t * (enth(7) - enth(8)) / (temp(7) - temp(8))   ! HT recuperator hot stream capacitance rate
    recomp_cycle%HT%C_dot_cold = m_dot_t * (enth(5) - enth(4)) / (temp(5) - temp(4))  ! HT recuperator cold stream capacitance rate
    C_dot_min = min(recomp_cycle%HT%C_dot_hot, recomp_cycle%HT%C_dot_cold)
    Q_dot_max = C_dot_min * (temp(7) - temp(4))
    recomp_cycle%HT%eff = Q_dot_HT / Q_dot_max  ! definition of effectiveness
    recomp_cycle%HT%UA_design = UA_HT_calc
    recomp_cycle%HT%UA = UA_HT_calc
    recomp_cycle%HT%DP_design = [pres(4) - pres(5), pres(7) - pres(8)]
    recomp_cycle%HT%m_dot_design = [m_dot_t, m_dot_t]
    recomp_cycle%HT%T_c_in= temp(4)
    recomp_cycle%HT%T_h_in= temp(7)
    recomp_cycle%HT%P_c_in= pres(4)
    recomp_cycle%HT%P_h_in= pres(7)
    recomp_cycle%HT%P_c_out= pres(5)
    recomp_cycle%HT%P_h_out= pres(8)
    recomp_cycle%HT%Q_dot = Q_dot_HT
    recomp_cycle%HT%min_DT = min_DT_HT
    recomp_cycle%HT%N_sub = N_sub_hxrs

    ! Set relevant values for other heat exchangers.
    recomp_cycle%PHX%Q_dot = m_dot_t * (enth(6) - enth(5))
    recomp_cycle%PHX%DP_design = [pres(5) - pres(6), 0.0_dp]
    recomp_cycle%PHX%m_dot_design = [m_dot_t, 0.0_dp]
    recomp_cycle%PC%Q_dot = m_dot_mc * (enth(9) - enth(1))
    recomp_cycle%PC%DP_design = [0.0_dp, pres(9) - pres(1)]
    recomp_cycle%PC%m_dot_design = [0.0_dp, m_dot_mc]

    ! Calculate cycle performance metrics.
    recomp_cycle%recomp_frac = recomp_frac
    recomp_cycle%W_dot_net = w_mc * m_dot_mc + w_rc * m_dot_rc + w_t * m_dot_t
    recomp_cycle%eta_thermal = recomp_cycle%W_dot_net / recomp_cycle%PHX%Q_dot
    recomp_cycle%m_dot_turbine = m_dot_t
    recomp_cycle%conv_tol = tol

end subroutine design


subroutine optimal_design( &
    W_dot_net,             &  ! [input] target net cycle power (kW)
    T_mc_in,               &  ! [input] compressor inlet temperature (K)
    T_t_in,                &  ! [input] turbine inlet temperature (K)
    DP_LT,                 &  ! [input] pressure drops in low-temperature recuperator (kPa if positive values)
    DP_HT,                 &  ! [input] pressure drops in high-temperature recuperator (kPa if positive values)
    DP_PC,                 &  ! [input] pressure drops in precooler (kPa if positive values)
    DP_PHX,                &  ! [input] pressure drops in primary heat exchanger (kPa if positive values)
    UA_rec_total,          &  ! [input] total design-point recuperator UA value (kW/K)
    eta_mc,                &  ! [input] design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
    eta_rc,                &  ! [input] design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
    eta_t,                 &  ! [input] design-point efficiency of the turbine; isentropic if positive, polytropic if negative
    N_sub_hxrs,            &  ! [input] number of sub-heat exchangers to use when calculating UA value for a heat exchanger
    P_high_limit,          &  ! [input] maximum allowable pressure in cycle (kPa)
    P_mc_out_guess,        &  ! [input] initial guess for main compressor outlet pressure (kPa)
    fixed_P_mc_out,        &  ! [input] if .true., P_mc_out is fixed at P_mc_out_guess
    PR_mc_guess,           &  ! [input] initial guess for ratio of P_mc_out to P_mc_in (-)
    fixed_PR_mc,           &  ! [input] if .true., ratio of P_mc_out to P_mc_in is fixed at PR_mc_guess
    recomp_frac_guess,     &  ! [input] initial guess for design-point recompression fraction
    fixed_recomp_frac,     &  ! [input] if .true., recomp_frac is fixed at recomp_frac_guess
    LT_frac_guess,         &  ! [input] initial guess for fraction of UA_rec_total that is in the low-temperature recuperator
    fixed_LT_frac,         &  ! [input] if .true., LT_frac is fixed at LT_frac_guess
    tol,                   &  ! [input] cycle convergence tolerance
    opt_tol,               &  ! [input] optimization convergence tolerance
    error_trace,           &  ! [output] an ErrorTrace object
    recomp_cycle           &  ! [output] a RecompCycle object
    )
    
    ! Arguments
    real(dp), intent(in) :: W_dot_net, T_mc_in, T_t_in, UA_rec_total, eta_mc, eta_rc, eta_t
    real(dp), intent(in) :: P_high_limit, P_mc_out_guess, PR_mc_guess, recomp_frac_guess, LT_frac_guess, tol, opt_tol
    logical, intent(in)  :: fixed_P_mc_out, fixed_PR_mc, fixed_recomp_frac, fixed_LT_frac
    integer, intent(in)  :: N_sub_hxrs
    real(dp), dimension(2), intent(in) :: DP_LT, DP_HT, DP_PC, DP_PHX
    type(ErrorTrace), intent(out)  :: error_trace
    type(RecompCycle), intent(out) :: recomp_cycle

    ! Subplex Parameters and Variables
    integer, parameter :: maxf = 200
    integer, parameter :: max_free_vars = 4
    integer, parameter :: mode = 0
    integer  :: iflag, iwork(50), nfe
    real(dp) :: subplex_fmin, scale(max_free_vars), work(50), x(max_free_vars)

    ! Local Variables
    type(RecompCycle) :: opt_recomp_cycle
    integer  :: n, index
    logical  :: solution_found

    ! Initialize guess array.
    x = 0.0_dp
    index = 1
    if (.not. fixed_P_mc_out) then
        x(index) = P_mc_out_guess
        scale(index) = 500.0_dp  ! pressure scale
        index = index + 1
    end if
    if (.not. fixed_PR_mc) then
        x(index) = PR_mc_guess
        scale(index) = 0.2_dp  ! pressure ratio scale
        index = index + 1
    end if    
    if (.not. fixed_recomp_frac) then
        x(index) = recomp_frac_guess
        scale(index) = 0.05_dp  ! recompression fraction scale
        index = index + 1
    end if    
    if (.not. fixed_LT_frac) then
        x(index) = LT_frac_guess
        scale(index) = 0.05_dp  ! recuperator split scale
        index = index + 1
    end if
    n = index - 1  

    ! Call subplex if any inputs can vary, or just call the design subroutine
    if (n > 0) then  ! call subplex
        solution_found = .false.
        opt_recomp_cycle%eta_thermal = 0.0_dp  ! ensure thermal efficiency is initialized to 0 (should be, but just to be sure)
        call subplx(design_point_eta, n, opt_tol, maxf, mode, scale, x, subplex_fmin, nfe, work, iwork, iflag)
        if (solution_found) then
            recomp_cycle = opt_recomp_cycle
        else
            error_trace%code = 1
            error_trace%lines(1) = 711
            error_trace%files(1) = 2
        end if
    else  ! no inputs vary; just call design subroutine
        call design(                                         &
            W_dot_net = W_dot_net,                           &
            T_mc_in = T_mc_in,                               &
            T_t_in = T_t_in,                                 &
            P_mc_in = P_mc_out_guess / PR_mc_guess,          &
            P_mc_out = P_mc_out_guess,                       &
            DP_LT = DP_LT,                                   &
            DP_HT = DP_HT,                                   &
            DP_PC = DP_PC,                                   &
            DP_PHX = DP_PHX,                                 &
            UA_LT = UA_rec_total * LT_frac_guess,            &
            UA_HT = UA_rec_total * (1.0_dp - LT_frac_guess), &
            recomp_frac = recomp_frac_guess,                 &
            eta_mc = eta_mc,                                 &
            eta_rc = eta_rc,                                 &
            eta_t = eta_t,                                   &
            N_sub_hxrs = N_sub_hxrs,                         &
            tol = tol,                                       &
            error_trace = error_trace,                       &
            recomp_cycle = recomp_cycle                      &
            )
        if (error_trace%code /= 0) then
            index = next_trace_index(error_trace)
            error_trace%lines(index) = 720
            error_trace%files(index) = 2
        end if
    end if
    recomp_cycle%high_pressure_limit = P_high_limit  ! store high pressure limit

    contains

        real(dp) function design_point_eta(n, x)
            ! Call the design subroutine with inputs contained in the x array.  Other required inputs are
            ! passed transparently because of the scope.
            integer, intent(in)  :: n     ! number of inputs that are varied during optimization
            real(dp), intent(in) :: x(n)  ! inputs with order: P_mc_out, PR_mc, recomp_frac, LT_frac (some can be missing)
            real(dp) :: P_mc_in_local, P_mc_out_local, PR_mc_local, recomp_frac_local, LT_frac_local

            ! Extract input variables from x.
            index = 1
            if (.not. fixed_P_mc_out) then
                P_mc_out_local = x(index)
                index = index + 1
            else
                P_mc_out_local = P_mc_out_guess
            end if
            if (.not. fixed_PR_mc) then
                PR_mc_local = x(index)
                index = index + 1
            else
                PR_mc_local = PR_mc_guess
            end if
            P_mc_in_local = P_mc_out_local / PR_mc_local
            if (.not. fixed_recomp_frac) then
                recomp_frac_local = x(index)
                index = index + 1
            else
                recomp_frac_local = recomp_frac_guess
            end if
            if (.not. fixed_LT_frac) then
                LT_frac_local = x(index)
                index = index + 1
            else
                LT_frac_local = LT_frac_guess
            end if

            ! Check inputs.
            if (recomp_frac_local < 0.0_dp) then
                design_point_eta = 0.0_dp
                return
            end if
            if (LT_frac_local < 0.0_dp .or. LT_frac_local > 1.0_dp) then
                design_point_eta = 0.0_dp
                return
            end if
            if (P_mc_out_local > P_high_limit) then
                design_point_eta = 0.0_dp
                return
            end if
            if (P_mc_in_local >= P_mc_out_local) then
                design_point_eta = 0.0_dp
                return
            end if
            if (P_mc_in_local < 100.0_dp) then  ! low-pressure limit
                design_point_eta = 0.0_dp
                return
            end if
            if (PR_mc_local > 50.0_dp) then  ! pressure ratio limit
                design_point_eta = 0.0_dp
                return
            end if

            ! Call design subroutine.
            call design(                                         &
                W_dot_net = W_dot_net,                           &
                T_mc_in = T_mc_in,                               &
                T_t_in = T_t_in,                                 &
                P_mc_in = P_mc_in_local,                         &
                P_mc_out = P_mc_out_local,                       &
                DP_LT = DP_LT,                                   &
                DP_HT = DP_HT,                                   &
                DP_PC = DP_PC,                                   &
                DP_PHX = DP_PHX,                                 &
                UA_LT = UA_rec_total * LT_frac_local,            &
                UA_HT = UA_rec_total * (1.0_dp - LT_frac_local), &
                recomp_frac = recomp_frac_local,                 &
                eta_mc = eta_mc,                                 &
                eta_rc = eta_rc,                                 &
                eta_t = eta_t,                                   &
                N_sub_hxrs = N_sub_hxrs,                         &
                tol = tol,                                       &
                error_trace = error_trace,                       &
                recomp_cycle = recomp_cycle                      &
                )
            if (error_trace%code == 0) then
                design_point_eta = -recomp_cycle%eta_thermal  ! subplex is a minimizer, so return negative efficiency
                if (recomp_cycle%eta_thermal > opt_recomp_cycle%eta_thermal) then
                    solution_found = .true.
                    opt_recomp_cycle = recomp_cycle
                end if
            else
                design_point_eta = 0.0_dp
            end if

        end function design_point_eta

end subroutine optimal_design


subroutine auto_optimal_design( &
    W_dot_net,                  &  ! [input] target net cycle power (kW)
    T_mc_in,                    &  ! [input] compressor inlet temperature (K)
    T_t_in,                     &  ! [input] turbine inlet temperature (K)
    DP_LT,                      &  ! [input] pressure drops in low-temperature recuperator (kPa if positive values)
    DP_HT,                      &  ! [input] pressure drops in high-temperature recuperator (kPa if positive values)
    DP_PC,                      &  ! [input] pressure drops in precooler (kPa if positive values)
    DP_PHX,                     &  ! [input] pressure drops in primary heat exchanger (kPa if positive values)
    UA_rec_total,               &  ! [input] total design-point recuperator UA value (kW/K)
    eta_mc,                     &  ! [input] design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
    eta_rc,                     &  ! [input] design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
    eta_t,                      &  ! [input] design-point efficiency of the turbine; isentropic if positive, polytropic if negative
    N_sub_hxrs,                 &  ! [input] number of sub-heat exchangers to use when calculating UA value for a heat exchanger
    P_high_limit,               &  ! [input] maximum allowable pressure in cycle (kPa)
    tol,                        &  ! [input] cycle convergence tolerance
    opt_tol,                    &  ! [input] optimization convergence tolerance
    error_trace,                &  ! [output] an ErrorTrace object
    recomp_cycle                &  ! [output] a RecompCycle object
    )
    
    ! Arguments
    real(dp), intent(in) :: W_dot_net, T_mc_in, T_t_in, UA_rec_total, eta_mc, eta_rc, eta_t, P_high_limit, tol, opt_tol
    integer, intent(in)  :: N_sub_hxrs
    real(dp), dimension(2), intent(in) :: DP_LT, DP_HT, DP_PC, DP_PHX
    type(ErrorTrace), intent(out)  :: error_trace
    type(RecompCycle), intent(out) :: recomp_cycle
    
    ! Local Variables
    real(dp) :: best_P_high, PR_mc_guess
    type(RecompCycle) :: test_cycle
    
    ! External Functions
    real(dp), external :: fmin
    
    ! Outer optimization loop (best results are stored in recomp_cycle).
    recomp_cycle%eta_thermal = 0.0_dp  ! initialize for optimization loop
    best_P_high = fmin(P_high_limit*0.2_dp, P_high_limit, opt_eta, 1.0_dp)

    ! Check model with P_mc_out set at P_high_limit for a recompression and simple cycle and use the better configuration.
    PR_mc_guess = recomp_cycle%pres(2) / recomp_cycle%pres(1)  ! optimal pressure ratio from outer optimization loop
    call optimal_design(                 &  ! recompression cycle
        W_dot_net = W_dot_net,           &
        T_mc_in = T_mc_in,               &
        T_t_in = T_t_in,                 &
        DP_LT = DP_LT,                   &
        DP_HT = DP_HT,                   &
        DP_PC = DP_PC,                   &
        DP_PHX = DP_PHX,                 &
        UA_rec_total = UA_rec_total,     &
        eta_mc = eta_mc,                 &
        eta_rc = eta_rc,                 &
        eta_t = eta_t,                   &
        N_sub_hxrs = N_sub_hxrs,         &
        P_high_limit = P_high_limit,     &
        P_mc_out_guess = P_high_limit,   &
        fixed_P_mc_out = .true.,         &
        PR_mc_guess = PR_mc_guess,       &
        fixed_PR_mc = .false.,           &
        recomp_frac_guess = 0.3_dp,      &
        fixed_recomp_frac = .false.,     &
        LT_frac_guess = 0.5_dp,          &
        fixed_LT_frac = .false.,         &
        tol = tol,                       &
        opt_tol = opt_tol,               &
        error_trace = error_trace,       &
        recomp_cycle = test_cycle        &
        )
    if (error_trace%code == 0 .and. test_cycle%eta_thermal >= recomp_cycle%eta_thermal) recomp_cycle = test_cycle
    call optimal_design(                 &  ! simple cycle
        W_dot_net = W_dot_net,           &
        T_mc_in = T_mc_in,               &
        T_t_in = T_t_in,                 &
        DP_LT = DP_LT,                   &
        DP_HT = DP_HT,                   &
        DP_PC = DP_PC,                   &
        DP_PHX = DP_PHX,                 &
        UA_rec_total = UA_rec_total,     &
        eta_mc = eta_mc,                 &
        eta_rc = eta_rc,                 &
        eta_t = eta_t,                   &
        N_sub_hxrs = N_sub_hxrs,         &
        P_high_limit = P_high_limit,     &
        P_mc_out_guess = P_high_limit,   &
        fixed_P_mc_out = .true.,         &
        PR_mc_guess = PR_mc_guess,       &
        fixed_PR_mc = .false.,           &
        recomp_frac_guess = 0.0_dp,      &
        fixed_recomp_frac = .true.,      &
        LT_frac_guess = 0.5_dp,          &
        fixed_LT_frac = .true.,          &
        tol = tol,                       &
        opt_tol = opt_tol,               &
        error_trace = error_trace,       &
        recomp_cycle = test_cycle        &
        )
    if (error_trace%code == 0 .and. test_cycle%eta_thermal >= recomp_cycle%eta_thermal) recomp_cycle = test_cycle

    contains

        real(dp) function opt_eta(P_high)
            ! Call the optimal_design subroutine with fixed P_high.  Other required inputs are
            ! passed transparently because of the scope.
            real(dp), intent(in) :: P_high
            type(RecompCycle) :: local_simple_cycle, local_recomp_cycle
            if (P_high > P_pseudocritical(T_mc_in)) then  ! start with P_mc_in at pseudocritical pressure
                PR_mc_guess = P_high / P_pseudocritical(T_mc_in)  
            else
                PR_mc_guess = 1.1_dp
            end if

            call optimal_design(                  &  ! recompression cycle
                W_dot_net = W_dot_net,            &
                T_mc_in = T_mc_in,                &
                T_t_in = T_t_in,                  &
                DP_LT = DP_LT,                    &
                DP_HT = DP_HT,                    &
                DP_PC = DP_PC,                    &
                DP_PHX = DP_PHX,                  &
                UA_rec_total = UA_rec_total,      &
                eta_mc = eta_mc,                  &
                eta_rc = eta_rc,                  &
                eta_t = eta_t,                    &
                N_sub_hxrs = N_sub_hxrs,          &
                P_high_limit = P_high_limit,      &
                P_mc_out_guess = P_high,          &
                fixed_P_mc_out = .true.,          &
                PR_mc_guess = PR_mc_guess,        &
                fixed_PR_mc = .false.,            &
                recomp_frac_guess = 0.3_dp,       &
                fixed_recomp_frac = .false.,      &
                LT_frac_guess = 0.5_dp,           &
                fixed_LT_frac = .false.,          &
                tol = tol,                        &
                opt_tol = opt_tol,                &
                error_trace = error_trace,        &
                recomp_cycle = local_recomp_cycle &
                )
            if (error_trace%code == 0 .and. local_recomp_cycle%eta_thermal >= recomp_cycle%eta_thermal) then
                recomp_cycle = local_recomp_cycle
            end if
            
            call optimal_design(                  &  ! simple cycle
                W_dot_net = W_dot_net,            &
                T_mc_in = T_mc_in,                &
                T_t_in = T_t_in,                  &
                DP_LT = DP_LT,                    &
                DP_HT = DP_HT,                    &
                DP_PC = DP_PC,                    &
                DP_PHX = DP_PHX,                  &
                UA_rec_total = UA_rec_total,      &
                eta_mc = eta_mc,                  &
                eta_rc = eta_rc,                  &
                eta_t = eta_t,                    &
                N_sub_hxrs = N_sub_hxrs,          &
                P_high_limit = P_high_limit,      &
                P_mc_out_guess = P_high,          &
                fixed_P_mc_out = .true.,          &
                PR_mc_guess = PR_mc_guess,        &
                fixed_PR_mc = .false.,            &
                recomp_frac_guess = 0.0_dp,       &
                fixed_recomp_frac = .true.,       &
                LT_frac_guess = 0.5_dp,           &
                fixed_LT_frac = .true.,           &
                tol = tol,                        &
                opt_tol = opt_tol,                &
                error_trace = error_trace,        &
                recomp_cycle = local_simple_cycle &
                )
            if (error_trace%code == 0 .and. local_simple_cycle%eta_thermal >= recomp_cycle%eta_thermal) then
                recomp_cycle = local_simple_cycle
            end if
            
            opt_eta = -max(local_recomp_cycle%eta_thermal, local_simple_cycle%eta_thermal)  ! fmin is a minimizer, so return a negative value
        
        end function opt_eta

end subroutine auto_optimal_design


real(dp) function P_pseudocritical(T)
    ! Return the approximate pseudocritical pressure (kPa) as a function of
    ! temperature (K) for carbon dioxide using a curve fit.
    real(dp), intent(in) :: T
    P_pseudocritical = (0.191448_dp * T + 45.6661_dp) * T - 24213.3_dp
end function P_pseudocritical


end module design_point
