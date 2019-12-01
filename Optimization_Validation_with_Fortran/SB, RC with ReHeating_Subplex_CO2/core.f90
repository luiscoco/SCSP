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
! This file contains the the module 'core', which holds a number of user-defined types, functions, and subroutines required by the
! other SCO2 Brayton cycle modeling modules.  This module also exposes the integer parameter 'dp', which should be used when
! declaring or setting double precision values (e.g., real(dp) :: value = 42.0_dp).
!
! Author: John Dyreby, Solar Energy Laboratory, University of Wisconsin-Madison <jjdyreby@uwalumni.com>
!
! Last Modified: August 20, 2014
!
!-----------------------------------------------------------------------------------------------------------------------------------

module core

implicit none

integer, parameter :: dp = selected_real_kind(15)  ! corresponds to double precision

type Compressor
    real(dp) :: D_rotor = 0.0_dp      ! rotor diameter (m)
    real(dp) :: D_rotor_2 = 0.0_dp    ! secondary rotor diameter (m) [used for two-stage recompressor, if necessary]
    real(dp) :: N_design = 0.0_dp     ! design-point shaft speed (rpm)
    real(dp) :: eta_design = 0.0_dp   ! design-point isentropic efficiency (-) [or stage efficiency in two-stage recompressor]
    real(dp) :: phi_design = 0.0_dp   ! design-point flow coefficient (-)
    real(dp) :: phi_min = 0.0_dp      ! surge limit (-)
    real(dp) :: phi_max = 0.0_dp      ! choke limit / zero pressure rise limit / x-intercept (-)
    real(dp) :: N = 0.0_dp            ! shaft speed (rpm)
    real(dp) :: eta = 0.0_dp          ! isentropic efficiency (-)
    real(dp) :: phi = 0.0_dp          ! dimensionless flow coefficient (-)
    real(dp) :: phi_2 = 0.0_dp        ! secondary dimensionless flow coefficient (-) [used for second stage phi, if necessary]
    real(dp) :: w_tip_ratio = 0.0_dp  ! ratio of the local (comp outlet) speed of sound to the tip speed (-)
    logical  :: surge = .false.       ! true if the compressor is in the surge region
end type Compressor

type Turbine
    real(dp) :: D_rotor = 0.0_dp      ! rotor diameter (m)
    real(dp) :: A_nozzle = 0.0_dp     ! effective nozzle area (m2)
    real(dp) :: N_design = 0.0_dp     ! design-point shaft speed (rpm)
    real(dp) :: eta_design = 0.0_dp   ! design-point isentropic efficiency (-)
    real(dp) :: N = 0.0_dp            ! shaft speed (rpm)
    real(dp) :: eta = 0.0_dp          ! isentropic efficiency (-)
    real(dp) :: nu = 0.0_dp           ! ratio of tip speed to spouting velocity (-)
    real(dp) :: w_tip_ratio = 0.0_dp  ! ratio of the local (turbine inlet) speed of sound to the tip speed (-)
end type Turbine

type HeatExchanger
    ! Under design conditions, streams are defined as cold (1) and hot (2).
    real(dp) :: UA_design = 0.0_dp                   ! design-point conductance (kW/K)
    real(dp), dimension(2) :: DP_design = 0.0_dp     ! design-point pressure drops across the heat exchanger (kPa)
    real(dp), dimension(2) :: m_dot_design = 0.0_dp  ! design-point mass flow rates of the two streams (kg/s)
    real(dp) :: Q_dot = 0.0_dp                       ! heat transfer rate (kW)
    real(dp) :: UA = 0.0_dp                          ! conductance (kW/K)
    real(dp) :: min_DT = 0.0_dp                      ! minimum temperature difference in hxr (K)
    real(dp) :: eff = 0.0_dp                         ! heat exchanger effectiveness (-)
    real(dp) :: C_dot_cold = 0.0_dp                  ! cold stream capacitance rate (kW/K)
    real(dp) :: C_dot_hot = 0.0_dp                   ! hot stream capacitance rate (kW/K)
    integer  :: N_sub = 1                            ! number of sub-heat exchangers used in model
    real(dp) :: T_c_in,T_h_in,P_c_in,P_h_in,P_c_out,P_h_out 
end type HeatExchanger

type RecompCycle
    real(dp) :: W_dot_net                                    ! net power output of the cycle (kW)
    real(dp) :: eta_thermal                                  ! thermal efficiency of the cycle (-)
    real(dp) :: recomp_frac                                  ! amount of flow that bypasses the precooler and is compressed in the recompressor (-)
    real(dp) :: m_dot_turbine                                ! mass flow rate through the turbine (kg/s)
    real(dp) :: high_pressure_limit                          ! maximum allowable high-side pressure (kPa)
    real(dp) :: conv_tol                                     ! relative convergence tolerance used during iteration loops involving this cycle (-)
    type(Turbine) :: t,t_rh                                  ! turbine user-defined type
    type(Compressor) :: mc, rc                               ! compressor and recompressor user-defined types
    type(HeatExchanger) :: LT, HT, PHX, RHX, PC              ! heat exchanger user-defined types
    real(dp), dimension(12) :: temp, pres, enth, entr, dens  ! thermodynamic properties at the state points of the cycle (K, kPa, kJ/kg, kJ/kg-K, kg/m3)
end type RecompCycle

type ErrorTrace
    integer :: code = 0                 ! the generated error code
    integer, dimension(4) :: lines = 0  ! the lines of the calls that generated the error (warning: these are hard-coded and need to be updated if file changes)
    integer, dimension(4) :: files = 0  ! the files of the calls that generated the error, using:
end type ErrorTrace                     !   1: core, 2: design_point, 3: off_design_point, 4: compressors, 5: turbines, 6: heat_exchangers, 7+: user-defined


contains


subroutine calculate_turbomachine_outlet( &
    T_in, P_in, P_out, eta, is_comp, error_trace, enth_in, entr_in, dens_in, temp_out, enth_out, entr_out, dens_out, spec_work &
    )
    ! Determine the outlet state of a compressor or turbine using isentropic efficiency and outlet pressure.
    !
    ! Inputs:
    !   T_in -- inlet temperature (K)
    !   P_in -- inlet pressure (kPa)
    !   P_out -- outlet pressure (kPa)
    !   eta -- isentropic efficiency (-)
    !   is_comp -- if .true., model a compressor (w = w_s / eta); if .false., model a turbine (w = w_s * eta)
    !
    ! Outputs:
    !   error_trace -- an ErrorTrace object
    !   enth_in -- inlet specific enthalpy (kJ/kg) [optional]
    !   entr_in -- inlet specific entropy (kJ/kg-K) [optional]
    !   dens_in -- inlet fluid density (kg/m3) [optional]
    !   temp_out -- outlet fluid temperature (K) [optional]
    !   enth_out -- outlet specific enthalpy (kJ/kg) [optional]
    !   entr_out -- outlet specific entropy (kJ/kg-K) [optional]
    !   dens_out -- outlet fluid density (kg/m3) [optional]
    !   spec_work -- specific work of the turbomachine (kJ/kg) [optional]
    !
    ! Notes:
    !   1) The specific work of the turbomachine is positive for a turbine and negative for a compressor.
    !   2) No error checking is performed on the inlet and outlet pressures; valid pressure ratios are assumed.

    use CO2_Properties, only: CO2_TP, CO2_PS, CO2_PH

    ! Arguments
    real(dp), intent(in) :: T_in, P_in, P_out, eta
    logical, intent(in)  :: is_comp
    type(ErrorTrace), intent(out) :: error_trace
    real(dp), intent(out), optional :: enth_in, entr_in, dens_in, temp_out, enth_out, entr_out, dens_out, spec_work
    
    ! Local Variables
    real(dp) :: h_in, s_in, h_s_out, w_s, w, h_out
    integer  :: error_code

    call CO2_TP(T=T_in, P=P_in, error_code=error_code, enth=h_in, entr=s_in, dens=dens_in)  ! properties at the inlet conditions
     !print *, T_in
	 !print *, P_in
	 !print *, h_in
	 !print *, s_in
	 !print *, dens_in 
	 
	if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 134
        error_trace%files(1) = 1
    
     print *, 'Luis Coco Error en la funcion CO2_TP calculation.'
	 print *, error_code
	 !print *, T_in
	 !print *, P_in
	 !print *, h_in
	 !print *, s_in
	 !print *, dens_in 
	 
      return
    end if

    call CO2_PS(P=P_out, S=s_in, error_code=error_code, enth=h_s_out)  ! outlet enthalpy if compression/expansion is isentropic
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 142
        error_trace%files(1) = 1
        return
    end if
    
    w_s = h_in - h_s_out  ! specific work if process is isentropic (negative for compression, positive for expansion)
    if (is_comp) then
        w = w_s / eta     ! actual specific work of compressor (negative value)
    else
        w = w_s * eta     ! actual specific work of turbine (positive value)
    end if
    h_out = h_in - w      ! energy balance on turbomachine
    
    call CO2_PH(P=P_out, H=h_out, error_code=error_code, temp=temp_out, entr=entr_out, dens=dens_out)  ! properties at the outlet
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 158
        error_trace%files(1) = 1
        return
    end if

    if (present(enth_in)) enth_in = h_in
    if (present(entr_in)) entr_in = s_in
    if (present(enth_out)) enth_out = h_out
    if (present(spec_work)) spec_work = w

end subroutine calculate_turbomachine_outlet


subroutine isen_eta_from_poly_eta(T_in, P_in, P_out, poly_eta, is_comp, error_trace, isen_eta)
    ! Calculate the isentropic efficiency that corresponds to a given polytropic efficiency
    ! for the expansion or compression from T_in and P_in to P_out.
    !
    ! Inputs:
    !   T_in -- inlet temperature (K)
    !   P_in -- inlet pressure (kPa)
    !   P_out -- outlet pressure (kPa)
    !   poly_eta -- polytropic efficiency (-)
    !   is_comp -- if .true., model a compressor (w = w_s / eta); if .false., model a turbine (w = w_s * eta)
    !
    ! Outputs:
    !   error_trace -- an ErrorTrace object
    !   isen_eta -- the equivalent isentropic efficiency (-)
    !
    ! Notes:
    !   1) Integration of small DP is approximated numerically by using 200 stages.
    !   2) No error checking is performed on the inlet and outlet pressures; valid pressure ratios are assumed.

    use CO2_Properties, only: CO2_TP, CO2_PS, CO2_PH

    ! Arguments
    real(dp), intent(in)  :: T_in, P_in, P_out, poly_eta
    logical, intent(in)   :: is_comp
    real(dp), intent(out) :: isen_eta
    type(ErrorTrace), intent(out) :: error_trace
    
    ! Parameters
    integer, parameter :: stages = 200

    ! Local Variables
    real(dp) :: h_in, s_in, h_s_out, w_s, w, stage_DP
    real(dp) :: stage_P_in, stage_P_out, stage_h_in, stage_s_in, stage_h_s_out, stage_h_out
    integer  :: error_code, stage

    call CO2_TP(T=T_in, P=P_in, error_code=error_code, enth=h_in, entr=s_in)  ! properties at the inlet conditions
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 209
        error_trace%files(1) = 1
        return
    end if
    call CO2_PS(P=P_out, S=s_in, error_code=error_code, enth=h_s_out)  ! outlet enthalpy if compression/expansion is isentropic
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 216
        error_trace%files(1) = 1    
        return
    end if

    stage_P_in = P_in   ! initialize first stage inlet pressure
    stage_h_in = h_in   ! initialize first stage inlet enthalpy
    stage_s_in = s_in   ! initialize first stage inlet entropy
    stage_DP = (P_out - P_in) / real(stages,dp)  ! pressure change per stage
    do stage = 1, stages
        stage_P_out = stage_P_in + stage_DP
        call CO2_PS(P=stage_P_out, S=stage_s_in, error_code=error_code, enth=stage_h_s_out)  ! outlet enthalpy if compression/expansion is isentropic
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 230
            error_trace%files(1) = 1
            return
        end if
        w_s = stage_h_in - stage_h_s_out  ! specific work if process is isentropic
        if (is_comp) then
            w = w_s / poly_eta            ! actual specific work of compressor (negative value)
        else
            w = w_s * poly_eta            ! actual specific work of turbine (positive value)
        end if
        stage_h_out = stage_h_in - w      ! energy balance on stage

        ! Reset next stage inlet values.
        stage_P_in = stage_P_out
        stage_h_in = stage_h_out
        call CO2_PH(P=stage_P_in, H=stage_h_in, error_code=error_code, entr=stage_s_in)
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 248
            error_trace%files(1) = 1
            return
        end if

    end do

    ! Note: last stage outlet enthalpy is equivalent to turbomachine outlet enthalpy.
    if (is_comp) then
        isen_eta = (h_s_out - h_in) / (stage_h_out - h_in)
    else
        isen_eta = (stage_h_out - h_in) / (h_s_out - h_in)
    end if

end subroutine isen_eta_from_poly_eta


subroutine calculate_hxr_UA( &
    N_sub_hxrs, Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out, error_trace, UA, min_DT &
    )
    ! Calculate the conductance (UA value) and minimum temperature difference of a heat exchanger
    ! given its mass flow rates, inlet temperatures, and a rate of heat transfer.
    !
    ! Inputs:
    !   N_sub_hxrs -- the number of sub-heat exchangers to use for discretization
    !   Q_dot -- rate of heat transfer in the heat exchanger (kW)
    !   m_dot_c -- cold stream mass flow rate (kg/s)
    !   m_dot_h -- hot stream mass flow rate (kg/s)
    !   T_c_in -- cold stream inlet temperature (K)
    !   T_h_in -- hot stream inlet temperature (K)
    !   P_c_in -- cold stream inlet pressure (kPa)
    !   P_c_out -- cold stream outlet pressure (kPa)
    !   P_h_in -- hot stream inlet pressure (kPa)
    !   P_h_out -- hot stream outlet pressure (kPa)
    !
    ! Outputs:
    !   error_trace -- an ErrorTrace object
    !   UA -- heat exchanger conductance (kW/K)
    !   min_DT -- minimum temperature difference ("pinch point") between hot and cold streams in heat exchanger (K)
    !
    ! Notes:
    !   1) Total pressure drop for each stream is divided equally among the sub-heat exchangers (i.e., DP is a linear distribution).

    use CO2_Properties, only: CO2_TP, CO2_PH

    ! Arguments
    integer, intent(in)   :: N_sub_hxrs
    real(dp), intent(in)  :: Q_dot, m_dot_c, m_dot_h, T_c_in, T_h_in, P_c_in, P_c_out, P_h_in, P_h_out
    real(dp), intent(out) :: UA, min_DT
    type(ErrorTrace), intent(out) :: error_trace
    
    ! Local Variables
    integer  :: i, error_code
    real(dp) :: h_c_in, h_h_in, h_c_out, h_h_out
    real(dp), dimension(N_sub_hxrs+1) :: P_c, P_h, T_c, T_h, h_c, h_h
    real(dp), dimension(N_sub_hxrs) :: C_dot_c, C_dot_h, C_dot_min, C_dot_max, C_R, eff, NTU

    ! Check inputs.
    if (T_h_in < T_c_in) then
        error_trace%code = 5
        error_trace%lines(1) = 309
        error_trace%files(1) = 1
        return
    end if
    if (P_h_in < P_h_out) then
        error_trace%code = 6
        error_trace%lines(1) = 315
        error_trace%files(1) = 1
        return
    end if
    if (P_c_in < P_c_out) then
        error_trace%code = 7
        error_trace%lines(1) = 321
        error_trace%files(1) = 1
        return
    end if
    if (abs(Q_dot) <= 1d-12) then  ! very low Q_dot; assume it is zero
        UA = 0.0_dp
        min_DT = T_h_in - T_c_in
        return
    end if

    ! Assume pressure varies linearly through heat exchanger.
    P_c = [ ( P_c_out + i * (P_c_in - P_c_out) / real(N_sub_hxrs,dp) , i = 0, N_sub_hxrs ) ]  ! create linear vector of cold stream pressures, with index 1 at the cold stream outlet
    P_h = [ ( P_h_in - i * (P_h_in - P_h_out) / real(N_sub_hxrs,dp) , i = 0, N_sub_hxrs ) ]   ! create linear vector of hot stream pressures, with index 1 at the hot stream inlet

    ! Calculate inlet enthalpies from known state points.
    call CO2_TP(T=T_c_in, P=P_c(N_sub_hxrs+1), error_code=error_code, enth=h_c_in)
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 338
        error_trace%files(1) = 1
        return
    end if
    call CO2_TP(T=T_h_in, P=P_h(1), error_code=error_code, enth=h_h_in)
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 345
        error_trace%files(1) = 1
        return
    end if

    ! Calculate outlet enthalpies from energy balances.
    h_c_out = h_c_in + Q_dot / m_dot_c
    h_h_out = h_h_in - Q_dot / m_dot_h
           
    ! Set up the enthalpy vectors and loop through the sub-heat exchangers, calculating temperatures.
    h_c = [ ( h_c_out + i * (h_c_in - h_c_out) / real(N_sub_hxrs,dp) , i = 0, N_sub_hxrs ) ]  ! create linear vector of cold stream enthalpies, with index 1 at the cold stream outlet
    h_h = [ ( h_h_in - i * (h_h_in - h_h_out) / real(N_sub_hxrs,dp) , i = 0, N_sub_hxrs ) ]   ! create linear vector of hot stream enthalpies, with index 1 at the hot stream inlet
    T_h(1) = T_h_in  ! hot stream inlet temperature
    call CO2_PH(P=P_c(1), H=h_c(1), error_code=error_code, temp=T_c(1))  ! cold stream outlet temperature
    if (error_code /= 0) then
        error_trace%code = error_code
        error_trace%lines(1) = 361
        error_trace%files(1) = 1
        return
    end if
    if (T_c(1) >= T_h(1)) then  ! there was a second law violation in this sub-heat exchanger
        error_trace%code = 11
        error_trace%lines(1) = 368
        error_trace%files(1) = 1
        return
    end if    
    do i = 2,N_sub_hxrs+1
        call CO2_PH(P=P_h(i), H=h_h(i), error_code=error_code, temp=T_h(i))
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 375
            error_trace%files(1) = 1
            return
        end if
        call CO2_PH(P=P_c(i), H=h_c(i), error_code=error_code, temp=T_c(i))    
        if (error_code /= 0) then
            error_trace%code = error_code
            error_trace%lines(1) = 382
            error_trace%files(1) = 1
            return
        end if
        if (T_c(i) >= T_h(i)) then  ! there was a second law violation in this sub-heat exchanger
            error_trace%code = 11
            error_trace%lines(1) = 389
            error_trace%files(1) = 1
            return
        end if
    end do
               
    ! Perform effectiveness-NTU and UA calculations (note: the below are all array operations).
    C_dot_h = m_dot_h * (h_h(1:N_sub_hxrs) - h_h(2:N_sub_hxrs+1)) / (T_h(1:N_sub_hxrs) - T_h(2:N_sub_hxrs+1))  ! hot stream capacitance rate
    C_dot_c = m_dot_c * (h_c(1:N_sub_hxrs) - h_c(2:N_sub_hxrs+1)) / (T_c(1:N_sub_hxrs) - T_c(2:N_sub_hxrs+1))  ! cold stream capacitance rate

      !print *,'C_dot_h1'
      !print *,C_dot_h(1)
      !print *,'C_dot_h2'
      !print *,C_dot_h(2)
      !print *,'C_dot_h3'
      !print *,C_dot_h(3)
      !print *,'C_dot_h4'
      !print *,C_dot_h(4)
      !print *,'C_dot_h5'
      !print *,C_dot_h(5)
      !print *,'C_dot_h6'
      !print *,C_dot_h(6)
      !print *,'C_dot_h7'
      !print *,C_dot_h(7)
      !print *,'C_dot_h8'
      !print *,C_dot_h(8)
      !print *,'C_dot_h9'
      !print *,C_dot_h(9)
      !print *,'C_dot_h10'
      !print *,C_dot_h(10)
      !print *,'C_dot_h11'
      !print *,C_dot_h(11)
      !print *,'C_dot_h12'
      !print *,C_dot_h(12)
      !print *,'C_dot_h13'
      !print *,C_dot_h(13)
      !print *,'C_dot_h14'
      !print *,C_dot_h(14)
      !print *,'C_dot_h15'
      !print *,C_dot_h(15)
	  
      !print *,'C_dot_c1'
      !print *,C_dot_c(1)
      !print *,'C_dot_c2'
      !print *,C_dot_c(2)
      !print *,'C_dot_c3'
      !print *,C_dot_c(3)
      !print *,'C_dot_c4'
      !print *,C_dot_c(4)
      !print *,'C_dot_c5'
      !print *,C_dot_c(5)
      !print *,'C_dot_c6'
      !print *,C_dot_c(6)
      !print *,'C_dot_c7'
      !print *,C_dot_c(7)
      !print *,'C_dot_c8'
      !print *,C_dot_c(8)
      !print *,'C_dot_c9'
      !print *,C_dot_c(9)
      !print *,'C_dot_c10'
      !print *,C_dot_c(10)
      !print *,'C_dot_c11'
      !print *,C_dot_c(11)
      !print *,'C_dot_c12'
      !print *,C_dot_c(12)
      !print *,'C_dot_c13'
      !print *,C_dot_c(13)
      !print *,'C_dot_c14'
      !print *,C_dot_c(14)
      !print *,'C_dot_c15'
      !print *,C_dot_c(15)
	  
      C_dot_min = min(C_dot_h, C_dot_c)  ! minimum capacitance stream
      C_dot_max = max(C_dot_h, C_dot_c)  ! maximum capacitance stream
      C_R = C_dot_min / C_dot_max        ! capacitance ratio of sub-heat exchanger
      eff = Q_dot / ((N_sub_hxrs * C_dot_min * (T_h(1:N_sub_hxrs) - T_c(2:N_sub_hxrs+1))))  ! effectiveness of each sub-heat exchanger
      	   
      !print *,'eff_1'
      !print *,eff(1)
      !print *,'eff_2'
      !print *,eff(2)
      !print *,'eff_3'
      !print *,eff(3)
      !print *,'eff_4'
      !print *,eff(4)
      !print *,'eff_5'
      !print *,eff(5)
      !print *,'eff_6'
      !print *,eff(6)
      !print *,'eff_7'
      !print *,eff(7)
      !print *,'eff_8'
      !print *,eff(8)
      !print *,'eff_9'
      !print *,eff(9)
      !print *,'eff_10'
      !print *,eff(10)
      !print *,'eff_11'
      !print *,eff(11)
      !print *,'eff_12'
      !print *,eff(12)
      !print *,'eff_13'
      !print *,eff(13)
      !print *,'eff_14'
      !print *,eff(14)
      !print *,'eff_15'
      !print *,eff(15)
	  
      !print *,'C_dot_min_15'
      !print *,C_dot_min(15)
	  
      !print *,'C_R_1'
      !print *,C_R(1)
      !print *,'C_R_2'
      !print *,C_R(2)
      !print *,'C_R_3'
      !print *,C_R(3)
      !print *,'C_R_4'
      !print *,C_R(4)
      !print *,'C_R_5'
      !print *,C_R(5)
      !print *,'C_R_6'
      !print *,C_R(6)
      !print *,'C_R_7'
      !print *,C_R(7)
      !print *,'C_R_8'
      !print *,C_R(8)
      !print *,'C_R_9'
      !print *,C_R(9)
      !print *,'C_R_10'
      !print *,C_R(10)
      !print *,'C_R_11'
      !print *,C_R(11)
      !print *,'C_R_12'
      !print *,C_R(12)
      !print *,'C_R_13'
      !print *,C_R(13)
      !print *,'C_R_14'
      !print *,C_R(14)
      print *,'C_R_15'
      print *,C_R(15)
	  
      where (C_R /= 1.0_dp)
        NTU = log((1.0_dp - eff * C_R) / (1.0_dp - eff)) / (1.0_dp - C_R)  ! NTU if C_R does not equal 1
      elsewhere
        NTU = eff / (1.0_dp - eff)  ! NTU if C_R equals 1
      end where
	  
      !print *,'NTU_1'
      !print *,NTU(1)
      !print *,'NTU_2'
      !print *,NTU(2)
      !print *,'NTU_3'
      !print *,NTU(3)
      !print *,'NTU_4'
      !print *,NTU(4)
      !print *,'NTU_5'
      !print *,NTU(5)
      !print *,'NTU_6'
      !print *,NTU(6)
      !print *,'NTU_7'
      !print *,NTU(7)
      !print *,'NTU_8'
      !print *,NTU(8)
      !print *,'NTU_9'
      !print *,NTU(9)
      !print *,'NTU_10'
      !print *,NTU(10)
      !print *,'NTU_11'
      !print *,NTU(11)
      !print *,'NTU_12'
      !print *,NTU(12)
      !print *,'NTU_13'
      !print *,NTU(13)
      !print *,'NTU_14'
      !print *,NTU(14)
      !print *,'NTU_15'
      !print *,NTU(15)
	  
      UA = sum(NTU * C_dot_min)   ! calculate total UA value for the heat exchanger
	  
      !print *,'UA_1'
      !print *,(NTU(1)*C_dot_min(1))
      !print *,'UA_2'
      !print *,(NTU(2)*C_dot_min(2))
      !print *,'UA_3'
      !print *,(NTU(3)*C_dot_min(3))
      !print *,'UA_4'
      !print *,(NTU(4)*C_dot_min(4))
      !print *,'UA_5'
      !print *,(NTU(5)*C_dot_min(5))
      !print *,'UA_6'
      !print *,(NTU(6)*C_dot_min(6))
      !print *,'UA_7'
      !print *,(NTU(7)*C_dot_min(7))
      !print *,'UA_8'
      !print *,(NTU(8)*C_dot_min(8))
      !print *,'UA_9'
      !print *,(NTU(9)*C_dot_min(9))
      !print *,'UA_10'
      !print *,(NTU(10)*C_dot_min(10))
      !print *,'UA_11'
      !print *,(NTU(11)*C_dot_min(11))
      !print *,'UA_12'
      !print *,(NTU(12)*C_dot_min(12))
      !print *,'UA_13'
      !print *,(NTU(13)*C_dot_min(13))
      !print *,'UA_14'
      !print *,(NTU(14)*C_dot_min(14))
      !print *,'UA_15'
      !print *,(NTU(15)*C_dot_min(15))
	  
      min_DT = minval(T_h - T_c)  ! find the smallest temperature difference within the heat exchanger

    ! Check for NaNs.
      if (UA /= UA) then
        error_trace%code = 14
        error_trace%lines(1) = 413
        error_trace%files(1) = 1        
        return
      end if

      !print *,'UA(kW/K)'
      !print *,UA
	  
      end subroutine calculate_hxr_UA


integer function next_trace_index(error_trace)
    ! Return the next index that should be used for tracing the lines / files
    ! that generated an error.
    type(ErrorTrace), intent(in) :: error_trace
    next_trace_index = minloc(error_trace%lines, 1)  ! assumes no line numbers are negative; returns location of first 0 in array
end function next_trace_index


end module core
