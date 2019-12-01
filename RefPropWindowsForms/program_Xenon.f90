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
! the .mod file for that particular module (the 'make' utility may be useful here).  If this is unclear, please refer to the
! 'create_python_interface.py' file as an example of the linking process or feel free to send John an email at the address below.
!
! For an example of how to size the components after running a design-point subroutine (necessary before running the off-design
! models), refer to the 'initialize' subroutine in the 'python_interface.f90' file.
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
     
      subroutine xenon(w_dot_net1,t_mc_in1,t_t_in1,p_rhx_in_guess1,fixed_P_rhx_in1,  & 
	                 t_rht_in1,ua_rec_total1,eta_mc1,eta_rc1,eta_t1,eta_trh1,  &
	                 n_sub_hxrs1,p_high_limit1,p_mc_out_guess1,fixed_p_mc_out1,  &
					 pr_mc_guess1,fixed_pr_mc1,recomp_frac_guess1,fixed_recomp_frac1, &
					 lt_frac_guess1,fixed_lt_frac1,tol1,opt_tol1, eta_thermal1,  &
					 dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2,  &
					 dp_rhx1,dp_rhx2)

      !DEC$ ATTRIBUTES DLLEXPORT, ALIAS: 'xenon'

      use core
      use design_point
      implicit none
	  
      type(RecompCycle) :: recomp_cycle
      type(ErrorTrace)  :: error_trace
      real(dp) , INTENT(INOUT):: w_dot_net1,t_mc_in1,t_t_in1,ua_rec_total1,eta_mc1,eta_rc1,eta_t1,eta_trh1,eta_thermal1
      real(dp) , INTENT(INOUT):: p_high_limit1,p_mc_out_guess1,pr_mc_guess1, recomp_frac_guess1,lt_frac_guess1,tol1,opt_tol1
	  real(dp) , INTENT(INOUT):: p_rhx_in_guess1,t_rht_in1
	  real(dp) , INTENT(INOUT):: dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2,dp_rhx1,dp_rhx2
      integer , INTENT (INOUT):: n_sub_hxrs1
      logical , INTENT (INOUT):: fixed_p_mc_out1,fixed_pr_mc1,fixed_recomp_frac1, fixed_lt_frac1,fixed_P_rhx_in1
	  
! Define and optimize a design point.
      call optimal_design( &
           W_dot_net = w_dot_net1, &         ! power output of cycle (kW)
           T_mc_in = t_mc_in1, &             ! compressor inlet temperature (K)
           T_t_in = t_t_in1, &               ! turbine inlet temperature (K)
           P_rhx_in_guess = p_rhx_in_guess1, & 
           fixed_P_rhx_in = fixed_P_rhx_in1, &
           T_rht_in = t_rht_in1,              &
           DP_LT = [-dp_lt1, -dp_lt2],  &  ! pressure drops in low-temperature recuperator (kPa if positive values)
           DP_HT = [-dp_ht1, -dp_ht2],  &  ! pressure drops in high-temperature recuperator (kPa if positive values)
           DP_PC = [-dp_pc1, -dp_pc2],  &  ! pressure drops in precooler (kPa if positive values)
           DP_PHX = [-dp_phx1, -dp_phx2], &  ! pressure drops in primary heat exchanger (kPa if positive values)
           DP_RHX = [-dp_rhx1, -dp_rhx2], &  ! pressure drops in primary heat exchanger (kPa if positive values)
           UA_rec_total = ua_rec_total1,  &  ! total recuperator conductance (kW/K)
           eta_mc = eta_mc1,              &  ! design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
           eta_rc = eta_rc1,              &  ! design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
           eta_t = eta_t1,                &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative
           eta_trh = eta_trh1,                &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative 
		   N_sub_hxrs = n_sub_hxrs1,      &  ! number of sub-heat exchangers to use in each recuperator
           P_high_limit = p_high_limit1,  &  ! highest allowable pressure in cycle (kPa)
           P_mc_out_guess = p_mc_out_guess1, &  ! compressor outlet temperature (kPa)
           fixed_P_mc_out = fixed_p_mc_out1,       &  ! do not vary P_mc_out
           PR_mc_guess = pr_mc_guess1,    &  ! initial guess for optimal pressure ratio
           fixed_PR_mc = fixed_pr_mc1,         &  ! vary PR to optimize efficiency
           recomp_frac_guess = recomp_frac_guess1,  &  ! initial guess for optimal recompression ratio
           fixed_recomp_frac = fixed_recomp_frac1,   &  ! vary recomp_frac to optimize efficiency
           LT_frac_guess = lt_frac_guess1,        &  ! initial guess for UA distribution in recuperators
           fixed_LT_frac = fixed_lt_frac1,       &  ! vary LT_frac to optimize efficiency
           tol = tol1,               &  ! convergence tolerance to use
           opt_tol = opt_tol1,           &  ! optimization tolerance to use
           error_trace = error_trace,     &
           recomp_cycle = recomp_cycle    &
           )

         ! Send back the results.
         eta_thermal1=recomp_cycle%eta_thermal
		 
       end subroutine xenon

