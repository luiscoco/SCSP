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
subroutine carbondioxideoffdesign(w_dot_net1,t_mc_in1,t_t_in1,p_mc_in1,p_mc_out1, &
                       p_rhx_in1,t_rht_in1,ua_lt1,ua_ht1,eta_mc1,eta_rc1,eta_t1,eta_trh1, &
                       n_sub_hxrs1,recomp_frac1,tol1,eta_thermal1, &
                       dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2, &
                       dp_rhx1,dp_rhx2,temp1,temp2,temp3,temp4,temp5,temp6,temp7, &
                       temp8,temp9,temp10,temp11,temp12,pres1,pres2,pres3,pres4, &
                       pres5,pres6,pres7,pres8,pres9,pres10,pres11,pres12,m_dot_turbine1, &
                       LT_mdoth,LT_mdotc,LT_Tcin,LT_Thin,LT_Pcin,LT_Phin, &
                       LT_Pcout,LT_Phout,LT_Q,HT_mdoth,HT_mdotc,HT_Tcin,HT_Thin, &
                       HT_Pcin,HT_Phin,HT_Pcout,HT_Phout,HT_Q,LT_UA,HT_UA,LT_Effc,HT_Effc, &
					   N_mc_design,t_mc_in_off,t_t_in_off,t_trh_in_off,p_trh_in_off,p_mc_in_off, &
					   recomp_frac_off,n_mc_off,n_t_off)

      !DEC$ ATTRIBUTES DLLEXPORT, ALIAS: 'carbondioxideoffdesign'
	  
      use core
      use design_point 
      use off_design_point
      use compressors_One_Stage
      use snl_turbines
      use heat_exchangers
      
      implicit none

      type(RecompCycle) :: recomp_cycle1
      type(ErrorTrace)  :: error_trace1
      integer :: i
	  
      real(dp) , INTENT(INOUT):: t_mc_in_off,t_t_in_off,t_trh_in_off,p_trh_in_off,p_mc_in_off
      real(dp) , INTENT(INOUT):: recomp_frac_off,n_mc_off,n_t_off,N_mc_design
      real(dp) , INTENT(INOUT):: w_dot_net1,t_mc_in1,t_t_in1,ua_lt1
      real(dp) , INTENT(INOUT):: ua_ht1,eta_mc1,eta_rc1,eta_t1,eta_trh1,eta_thermal1
      real(dp) , INTENT(INOUT):: recomp_frac1,tol1,p_mc_in1,p_mc_out1
      real(dp) , INTENT(INOUT):: p_rhx_in1,t_rht_in1,m_dot_turbine1,LT_UA,HT_UA,LT_Effc,HT_Effc	  
      real(dp) , INTENT(INOUT):: dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2,dp_rhx1,dp_rhx2
      real(dp) , INTENT(INOUT):: LT_mdoth,LT_mdotc,LT_Tcin,LT_Thin,LT_Pcin,LT_Phin,LT_Pcout,LT_Phout,LT_Q
      real(dp) , INTENT(INOUT):: HT_mdoth,HT_mdotc,HT_Tcin,HT_Thin,HT_Pcin,HT_Phin,HT_Pcout,HT_Phout,HT_Q
      integer , INTENT (INOUT):: n_sub_hxrs1
      real(dp) , INTENT(INOUT):: temp1,temp2,temp3,temp4,temp5,temp6,temp7,temp8,temp9,temp10,temp11,temp12
      real(dp) , INTENT(INOUT):: pres1,pres2,pres3,pres4,pres5,pres6,pres7,pres8,pres9,pres10,pres11,pres12

! Define and optimize a design point.
      call design(                         &
           W_dot_net = w_dot_net1,         &         ! power output of cycle (kW)
           T_mc_in = t_mc_in1,             &             ! compressor inlet temperature (K)
           T_t_in = t_t_in1,               &               ! turbine inlet temperature (K)
           P_mc_in = p_mc_in1,             &  ! [input] compressor inlet pressure (kPa)
           P_mc_out = p_mc_out1,           &  ! [input] compressor outlet pressure (kPa)
           P_rhx_in = p_rhx_in1,           & 
           T_rht_in = t_rht_in1,           &
           DP_LT = [-dp_lt1, -dp_lt2],     &  ! pressure drops in low-temperature recuperator (kPa if positive values)
           DP_HT = [-dp_ht1, -dp_ht2],     &  ! pressure drops in high-temperature recuperator (kPa if positive values)
           DP_PC = [-dp_pc1, -dp_pc2],     &  ! pressure drops in precooler (kPa if positive values)
           DP_PHX = [-dp_phx1, -dp_phx2],  &  ! pressure drops in primary heat exchanger (kPa if positive values)
           DP_RHX = [-dp_rhx1, -dp_rhx2],  &  ! pressure drops in primary heat exchanger (kPa if positive values)
           UA_LT = ua_lt1,                 &  ! [input] design-point UA value for the low-temperature recuperator (kW/K)
           UA_HT = ua_ht1,                 &  ! [input] design-point UA value for the high-temperature recuperator (kW/K)
           recomp_frac = recomp_frac1,     &  ! initial guess for optimal recompression ratio
           eta_mc = eta_mc1,               &  ! design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
           eta_rc = eta_rc1,               &  ! design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
           eta_t = eta_t1,                 &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative
           eta_trh = eta_trh1,             &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative 
           N_sub_hxrs = n_sub_hxrs1,       &  ! number of sub-heat exchangers to use in each recuperator
           tol = tol1,                     &  ! convergence tolerance to use
           error_trace = error_trace1,     &
           recomp_cycle = recomp_cycle1    &
           )

           N_mc_design = recomp_cycle1%mc%N
		   
! Define and optimize a design point.
      call off_design(                       &
               recomp_cycle = recomp_cycle1, &  ! power output of cycle (kW)
               T_mc_in = t_mc_in_off,        &  ! compressor inlet temperature (K); If we increasse T_mc_in is necesary to increase P_mc_in
               T_t_in = t_t_in_off,          &  ! turbine inlet temperature (K)
               T_trh_in = t_trh_in_off,      & !
               P_trh_in = p_trh_in_off,      &   ! Could vary
               P_mc_in = p_mc_in_off,        & ! Could vary
               recomp_frac = recomp_frac_off,& ! Could vary 
               N_mc = n_mc_off,              & ! Could vary
               N_t = n_t_off,                & ! Could vary
               N_sub_hxrs = n_sub_hxrs1,     &  ! number of sub-heat exchangers to use in each recuperator
               tol = tol1,                   &  ! convergence tolerance to use
               error_trace = error_trace1    &
			   )
	  
	    ! Send back the results.
        eta_thermal1=recomp_cycle1%eta_thermal
	
        !recomp_cycle%W_dot_net
        temp1=recomp_cycle1%temp(1)
        temp2=recomp_cycle1%temp(2)
        temp3=recomp_cycle1%temp(3)
        temp4=recomp_cycle1%temp(4)
        temp5=recomp_cycle1%temp(5)
        temp6=recomp_cycle1%temp(6)
        temp7=recomp_cycle1%temp(7)
        temp8=recomp_cycle1%temp(8)
        temp9=recomp_cycle1%temp(9)
        temp10=recomp_cycle1%temp(10)
        temp11=recomp_cycle1%temp(11)
        temp12=recomp_cycle1%temp(12)
		 
        pres1=recomp_cycle1%pres(1)
        pres2=recomp_cycle1%pres(2)
        pres3=recomp_cycle1%pres(3)
        pres4=recomp_cycle1%pres(4)
        pres5=recomp_cycle1%pres(5)
        pres6=recomp_cycle1%pres(6)
        pres7=recomp_cycle1%pres(7)
        pres8=recomp_cycle1%pres(8)
        pres9=recomp_cycle1%pres(9)
        pres10=recomp_cycle1%pres(10)
        pres11=recomp_cycle1%pres(11)
        pres12=recomp_cycle1%pres(12)
		 
		m_dot_turbine1=recomp_cycle1%m_dot_turbine
		p_mc_out1=recomp_cycle1%pres(2)
		recomp_frac1=recomp_cycle1%recomp_frac
		LT_UA = recomp_cycle1%LT%UA
		HT_UA = recomp_cycle1%HT%UA
		 
        LT_mdoth=recomp_cycle1%LT%m_dot_design(2)
        LT_mdotc=recomp_cycle1%LT%m_dot_design(1)
        LT_Tcin=recomp_cycle1%LT%T_c_in
        LT_Thin=recomp_cycle1%LT%T_h_in
        LT_Pcin=recomp_cycle1%LT%P_c_in
        LT_Phin=recomp_cycle1%LT%P_h_in
        LT_Pcout=recomp_cycle1%LT%P_c_out
        LT_Phout=recomp_cycle1%LT%P_h_out
        LT_Q=recomp_cycle1%LT%Q_dot
        LT_Effc=recomp_cycle1%LT%eff
		
        HT_mdoth=recomp_cycle1%HT%m_dot_design(1)
        HT_mdotc=recomp_cycle1%HT%m_dot_design(2)
        HT_Tcin=recomp_cycle1%HT%T_c_in
        HT_Thin=recomp_cycle1%HT%T_h_in
        HT_Pcin=recomp_cycle1%HT%P_c_in
        HT_Phin=recomp_cycle1%HT%P_h_in
        HT_Pcout=recomp_cycle1%HT%P_c_out
        HT_Phout=recomp_cycle1%HT%P_h_out
        HT_Q=recomp_cycle1%HT%Q_dot
        HT_Effc=recomp_cycle1%HT%eff
		
		w_dot_net1=recomp_cycle1%W_dot_net

      end subroutine carbondioxideoffdesign

