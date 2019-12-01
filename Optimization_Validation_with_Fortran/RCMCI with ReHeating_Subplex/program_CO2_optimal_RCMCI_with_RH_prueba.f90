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
! This file is an example of how the design-point recompression cycle models could be used in a Fortran program.  It can easily be
! expanded to also model off-design performance, but the compiling and linking steps get slightly more complicated.  In general,
! each of the desired component modules must be compiled and linked to, possibly using the -L flag to indicate the folder containing
! the .mod file for that particular module (the 'make' utility may be useful here).  If this is unclear, please er to the
! 'create_python_interface.py' file as an example of the linking process or feel free to send John an email at the address below.
!
! For an example of how to size the components after running a design-point subroutine (necessary before running the off-design
! models), er to the 'initialize' subroutine in the 'python_interface.f90' file.
!
! To compile this program, use:
!   gfortran -O3 Netlib/subplex/*.f Netlib/fmin.f Properties/FIT/module_CO2_properties.f90 Brayton/core.f90 Brayton/design_point.f90 example_fortran_program.f90
! (this command will create a number of .mod files in the main folder that can be deleted after the program is compiled)
!
! To run the compiled program, use:
!   ./a.out (Linux/OSX)
!   a.exe (Windows)
!
! Author: John Dyreby, Solar Energy Laboratory, University of Wisconsin-Madison <jjdyreby@uwalumni.com>
!
! Last Modified: September 25, 2014
!
!------------------------------------------------------------w
      program main

      use core
      use design_point
      implicit none
	  
      type(RecompCycle) :: recomp_cycle
      type(ErrorTrace)  :: error_trace
      integer :: i
	  
! Define and optimize a design point.
      call optimal_design( &
           W_dot_net = 50000.0_dp, &         ! power output of cycle (kW)
           T_mc1_in = 32.0_dp + 273.15_dp, &             ! compressor inlet temperature (K)
           T_mc2_in = 32.0_dp + 273.15_dp, &             ! compressor inlet temperature (K)
           T_t_in = 550.0_dp + 273.15_dp, &               ! turbine inlet temperature (K)
           P_mc1_in_guess= 7400.0_dp, &
           fixed_P_mc1_in= .false., &
           P_rhx_in_guess = 11000.0_dp, & 
           fixed_P_rhx_in = .false., &
           T_rht_in = 550.0_dp + 273.15_dp, &
           DP_LT = [0.0_dp,0.0_dp],  &  ! pressure drops in low-temperature recuperator (kPa if positive values)
           DP_HT = [0.0_dp,0.0_dp],  &  ! pressure drops in high-temperature recuperator (kPa if positive values)
           DP_PC1 = [0.0_dp,0.0_dp],  &  ! pressure drops in precooler (kPa if positive values)
           DP_PC2 = [0.0_dp,0.0_dp],  &  ! pressure drops in precooler (kPa if positive values)
           DP_PHX = [0.0_dp,0.0_dp], &  ! pressure drops in primary heat exchanger (kPa if positive values)
           DP_RHX = [0.0_dp,0.0_dp], &  ! pressure drops in primary heat exchanger (kPa if positive values)
           UA_rec_total = 10000.0_dp,  &  ! total recuperator conductance (kW/K)
           eta_mc1 = 0.89_dp,              &  ! design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
           eta_mc2 = 0.89_dp,              &  ! design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
           eta_rc = 0.89_dp,              &  ! design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
           eta_t = 0.93_dp,                &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative
           eta_trh = 0.93_dp,                &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative 
           N_sub_hxrs = 15,      &  ! number of sub-heat exchangers to use in each recuperator
           P_high_limit = 25000.0_dp,  &  ! highest allowable pressure in cycle (kPa)
           P_mc2_out_guess = 25000.0_dp, &  ! compressor outlet temperature (kPa)
           fixed_P_mc2_out = .true.,       &  ! do not vary P_mc_out
           PR_mc2_guess = 2.0_dp,    &  ! initial guess for optimal pressure ratio
           fixed_PR_mc2 = .false.,         &  ! vary PR to optimize efficiency
           recomp_frac_guess = 0.1_dp,  &  ! initial guess for optimal recompression ratio
           fixed_recomp_frac = .false.,   &  ! vary recomp_frac to optimize efficiency
           LT_frac_guess = 0.1_dp,        &  ! initial guess for UA distribution in recuperators
           fixed_LT_frac = .false.,       &  ! vary LT_frac to optimize efficiency
           tol = 1.0e-5_dp,               &  ! convergence tolerance to use
           opt_tol = 1.0e-6_dp,           &  ! optimization tolerance to use
           error_trace = error_trace,     &
           recomp_cycle = recomp_cycle    &
           )

		       ! Print the results.
       print *, ''
       print *, '  Eta_Thermal', recomp_cycle%eta_thermal
       print *, '  state-point temperatures (C):'
       do i = 1, 14
        write(*, '(AI0,A,F7.2)') '    T', i, ': ', recomp_cycle%temp(i) - 273.15_dp
       end do
       print *, '  state-point pressures (kPa):'
      do i = 1, 14
       write(*, '(AI0,A,F8.1)') '    P', i, ': ', recomp_cycle%pres(i)
      end do
      print *, ''
      read *

      end program main