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
     
      subroutine carbondioxide(w_dot_net1,t_mc_in1,t_t_in1,p_rhx_in1,  & 
	                 t_rht_in1,ua_rec_total1,eta_mc1,eta_rc1,eta_t1,eta_trh1,  &
	                 n_sub_hxrs1,p_high_limit1,tol1,opt_tol1, eta_thermal1,  &
					 dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2,  &
					 dp_rhx1,dp_rhx2,temp1,temp2,temp3,temp4,temp5,temp6,temp7,  &
					 temp8,temp9,temp10,temp11,temp12,pres1,pres2,pres3,pres4,   &
					 pres5,pres6,pres7,pres8,pres9,pres10,pres11,pres12)

      !DEC$ ATTRIBUTES DLLEXPORT, ALIAS: 'carbondioxide'

      use core
      use design_point1
      implicit none
	  
      type(RecompCycle) :: recomp_cycle
      type(ErrorTrace)  :: error_trace
	  integer :: i
      real(dp) , INTENT(INOUT):: w_dot_net1,t_mc_in1,t_t_in1,ua_rec_total1,eta_mc1,eta_rc1,eta_t1,eta_trh1,eta_thermal1
      real(dp) , INTENT(INOUT):: p_high_limit1,tol1,opt_tol1
      real(dp) , INTENT(INOUT):: p_rhx_in1,t_rht_in1
      real(dp) , INTENT(INOUT):: dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2,dp_rhx1,dp_rhx2
      integer , INTENT (INOUT):: n_sub_hxrs1
      real(dp) , INTENT(INOUT):: temp1,temp2,temp3,temp4,temp5,temp6,temp7,temp8,temp9,temp10,temp11,temp12
	  real(dp) , INTENT(INOUT):: pres1,pres2,pres3,pres4,pres5,pres6,pres7,pres8,pres9,pres10,pres11,pres12
	  
! Define and optimize a design point.
      call auto_optimal_design( &
           W_dot_net = w_dot_net1, &         ! power output of cycle (kW)
           T_mc_in = t_mc_in1, &             ! compressor inlet temperature (K)
           T_t_in = t_t_in1, &               ! turbine inlet temperature (K)
           P_rhx_in = p_rhx_in1, & 
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
           tol = tol1,               &  ! convergence tolerance to use
           opt_tol = opt_tol1,           &  ! optimization tolerance to use
           error_trace = error_trace,     &
           recomp_cycle = recomp_cycle    &
           )

         ! Send back the results.
         eta_thermal1=recomp_cycle%eta_thermal
         !recomp_cycle%W_dot_net
         temp1=recomp_cycle%temp(1)
         temp2=recomp_cycle%temp(2)
		 temp3=recomp_cycle%temp(3)
		 temp4=recomp_cycle%temp(4)
		 temp5=recomp_cycle%temp(5)
         temp6=recomp_cycle%temp(6)
         temp7=recomp_cycle%temp(7)
         temp8=recomp_cycle%temp(8)
         temp9=recomp_cycle%temp(9)
         temp10=recomp_cycle%temp(10)
         temp11=recomp_cycle%temp(11)		 
         temp12=recomp_cycle%temp(12)
		 
		 pres1=recomp_cycle%pres(1)
         pres2=recomp_cycle%pres(2)
		 pres3=recomp_cycle%pres(3)
		 pres4=recomp_cycle%pres(4)
		 pres5=recomp_cycle%pres(5)
         pres6=recomp_cycle%pres(6)
         pres7=recomp_cycle%pres(7)
         pres8=recomp_cycle%pres(8)
         pres9=recomp_cycle%pres(9)
         pres10=recomp_cycle%pres(10)
         pres11=recomp_cycle%pres(11)		 
         pres12=recomp_cycle%pres(12)
		 
         !do i = 1, 12
         !temp(i)=recomp_cycle%temp(i)
		 !pres(i)=recomp_cycle%pres(i)
		 !enth(i)=recomp_cycle%enth(i)
		 !entr(i)=recomp_cycle%entr(i)
		 !dens(i)=recomp_cycle%dens(i)
         !end do

       end subroutine carbondioxide

