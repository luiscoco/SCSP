!  Console1.f90 
!
!  FUNCTIONS:
!  Console1 - Entry point of console application.
!

!****************************************************************************
!
!  PROGRAM: Console1
!
!  PURPOSE:  Entry point for the console application.
!
!****************************************************************************

program main

use core
use design_point
implicit none

type(RecompCycle) :: recomp_cycle
type(ErrorTrace)  :: error_trace
integer :: i

! Define and optimize a design point.
call    design( &
    W_dot_net = 50000.0_dp,        &  ! power output of cycle (kW)
    T_mc_in = 50.0_dp + 273.15_dp, &  ! compressor inlet temperature (K)
    T_t_in = 550.0_dp + 273.15_dp, &  ! turbine inlet temperature (K)
    P_mc_in = 7812.5_dp,           &
    P_mc_out = 25000.0_dp,         &
    DP_LT = [-0.0_dp, -0.0_dp],  &  ! pressure drops in low-temperature recuperator (kPa if positive values)
    DP_HT = [-0.0_dp, -0.0_dp],  &  ! pressure drops in high-temperature recuperator (kPa if positive values)
    DP_PC = [-0.0_dp, -0.0_dp],  &  ! pressure drops in precooler (kPa if positive values)
    DP_PHX = [-0.0_dp, -0.0_dp], &  ! pressure drops in primary heat exchanger (kPa if positive values)
    UA_LT = 5000.37_dp,             &  ! total recuperator conductance (kW/K)
    UA_HT = 5000.62_dp,             &  ! total recuperator conductance (kW/K)
    recomp_frac = 0.35_dp,         &
    eta_mc = 0.89_dp,              &  ! design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
    eta_rc = 0.89_dp,              &  ! design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
    eta_t = 0.93_dp,               &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative
    N_sub_hxrs = 15,               &  ! number of sub-heat exchangers to use in each recuperator
    tol = 1.0e-5_dp,               &  ! convergence tolerance to use
    error_trace = error_trace,     &
    recomp_cycle = recomp_cycle    &
    )

! Print the results.
print *, ''
print *, '  Design Point Values:'
print *, '  power output (kW)', recomp_cycle%W_dot_net
print *, '  Main Compressor Inlet pressure (kPa):', recomp_cycle%pres(1)
print *, '  ReHeating Turbine Inlet pressure (kPa):', recomp_cycle%pres(10)
print *, '  Main Turbine Inlet pressure (kPa):', recomp_cycle%pres(2)
print *, '  low-temperature recuperator UA (kW/K):', recomp_cycle%LT%UA_design
print *, '  high-temperature recuperator UA (kW/K):', recomp_cycle%HT%UA_design
print *, '  turbine mass flow rate (kg/s):', recomp_cycle%m_dot_turbine
print *, '  recompression fraction:', recomp_cycle%recomp_frac
print *, '  thermal efficiency:', recomp_cycle%eta_thermal
print *, '  Turbine Rotor Diameter (m):', recomp_cycle%t%D_rotor
print *, '  Main Compressor Rotor Diameter (m):', recomp_cycle%mc%D_rotor
print *, '  ReCompressor Rotor1 Diameter (m):', recomp_cycle%rc%D_rotor
print *, '  ReCompressor Rotor2 Diameter (m):', recomp_cycle%rc%D_rotor_2
print *, '  Main Compressor velocity (rpm):', recomp_cycle%mc%N

print *, '  Pressure P1 (kPa):', recomp_cycle%pres(1)
print *, '  Temperature T1 (K):', recomp_cycle%temp(1)
print *, '  Pressure P2 (kPa):', recomp_cycle%pres(2)
print *, '  Temperature T2 (K):', recomp_cycle%temp(2)
print *, '  Pressure P3 (kPa):', recomp_cycle%pres(3)
print *, '  Temperature T3 (K):', recomp_cycle%temp(3)
print *, '  Pressure P4 (kPa):', recomp_cycle%pres(4)
print *, '  Temperature T4 (K):', recomp_cycle%temp(4)
print *, '  Pressure P5 (kPa):', recomp_cycle%pres(5)
print *, '  Temperature T5 (K):', recomp_cycle%temp(5)
print *, '  Pressure P6 (kPa):', recomp_cycle%pres(6)
print *, '  Temperature T6 (K):', recomp_cycle%temp(6)
print *, '  Pressure P7 (kPa):', recomp_cycle%pres(7)
print *, '  Temperature T7 (K):', recomp_cycle%temp(7)
print *, '  Pressure P8 (kPa):', recomp_cycle%pres(8)
print *, '  Temperature T8 (K):', recomp_cycle%temp(8)
print *, '  Pressure P9 (kPa):', recomp_cycle%pres(9)
print *, '  Temperature T9 (K):', recomp_cycle%temp(9)
print *, '  Pressure P10 (kPa):', recomp_cycle%pres(10)
print *, '  Temperature T10 (K):', recomp_cycle%temp(10)
pause
end program main

