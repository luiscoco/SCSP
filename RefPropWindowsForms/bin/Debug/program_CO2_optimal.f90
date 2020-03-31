      subroutine carbondioxidemixturesubplex(w_dot_net1,t_mc_in1,t_t_in1, & 
	                 ua_rec_total1,eta_mc1,eta_rc1,eta_t1,  &
	                 n_sub_hxrs1,p_high_limit1,p_mc_out_guess1,fixed_p_mc_out1,  &
					 pr_mc_guess1,fixed_pr_mc1,recomp_frac_guess1,fixed_recomp_frac1, &
					 lt_frac_guess1,fixed_lt_frac1,tol1,opt_tol1, eta_thermal1,  &
					 dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2,  &
					 temp1,temp2,temp3,temp4,temp5,temp6,temp7,  &
					 temp8,temp9,temp10,pres1,pres2,pres3,pres4,   &
					 pres5,pres6,pres7,pres8,pres9,pres10,m_dot_turbine1, &
					 LT_mdoth,LT_mdotc,LT_Tcin,LT_Thin,LT_Pcin,LT_Phin, &
					 LT_Pcout,LT_Phout,LT_Q,HT_mdoth,HT_mdotc,HT_Tcin,HT_Thin, &
					 HT_Pcin,HT_Phin,HT_Pcout,HT_Phout,HT_Q,LT_UA,HT_UA,LT_Effc,HT_Effc, &
					 N_design,PHX_Q1,PC_Q1)

      !DEC$ ATTRIBUTES DLLEXPORT, ALIAS: 'carbondioxidemixturesubplex'

      use core
      use design_point
      implicit none
	  
      type(RecompCycle) :: recomp_cycle
      type(ErrorTrace)  :: error_trace
	  integer :: i
      real(dp) , INTENT(INOUT):: w_dot_net1,t_mc_in1,t_t_in1,ua_rec_total1,eta_mc1,eta_rc1,eta_t1,eta_thermal1,N_design
      real(dp) , INTENT(INOUT):: p_high_limit1,p_mc_out_guess1,pr_mc_guess1, recomp_frac_guess1,lt_frac_guess1,tol1,opt_tol1
      real(dp) , INTENT(INOUT):: m_dot_turbine1,LT_UA,HT_UA,LT_Effc,HT_Effc	  
      real(dp) , INTENT(INOUT):: dp_lt1,dp_lt2,dp_ht1,dp_ht2,dp_pc1,dp_pc2,dp_phx1,dp_phx2
      real(dp) , INTENT(INOUT):: LT_mdoth,LT_mdotc,LT_Tcin,LT_Thin,LT_Pcin,LT_Phin,LT_Pcout,LT_Phout,LT_Q
      real(dp) , INTENT(INOUT):: HT_mdoth,HT_mdotc,HT_Tcin,HT_Thin,HT_Pcin,HT_Phin,HT_Pcout,HT_Phout,HT_Q
      integer , INTENT (INOUT):: n_sub_hxrs1
      logical , INTENT (INOUT):: fixed_p_mc_out1,fixed_pr_mc1,fixed_recomp_frac1, fixed_lt_frac1
      real(dp) , INTENT(INOUT):: temp1,temp2,temp3,temp4,temp5,temp6,temp7,temp8,temp9,temp10
      real(dp) , INTENT(INOUT):: pres1,pres2,pres3,pres4,pres5,pres6,pres7,pres8,pres9,pres10
      real(dp) , INTENT(INOUT):: PHX_Q1,PC_Q1
	  
! Define and optimize a design point.
      call optimal_design( &
           W_dot_net = w_dot_net1, &         ! power output of cycle (kW)
           T_mc_in = t_mc_in1, &             ! compressor inlet temperature (K)
           T_t_in = t_t_in1, &               ! turbine inlet temperature (K)
           DP_LT = [-dp_lt1, -dp_lt2],  &  ! pressure drops in low-temperature recuperator (kPa if positive values)
           DP_HT = [-dp_ht1, -dp_ht2],  &  ! pressure drops in high-temperature recuperator (kPa if positive values)
           DP_PC = [-dp_pc1, -dp_pc2],  &  ! pressure drops in precooler (kPa if positive values)
           DP_PHX = [-dp_phx1, -dp_phx2], &  ! pressure drops in primary heat exchanger (kPa if positive values)
           UA_rec_total = ua_rec_total1,  &  ! total recuperator conductance (kW/K)
           eta_mc = eta_mc1,              &  ! design-point efficiency of the main compressor; isentropic if positive, polytropic if negative
           eta_rc = eta_rc1,              &  ! design-point efficiency of the recompressor; isentropic if positive, polytropic if negative
           eta_t = eta_t1,                &  ! design-point efficiency of the turbine; isentropic if positive, polytropic if negative
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
		 
		 m_dot_turbine1=recomp_cycle%m_dot_turbine
		 p_mc_out_guess1=recomp_cycle%pres(2)
		 pr_mc_guess1=(recomp_cycle%pres(2))/(recomp_cycle%pres(1))
		 recomp_frac_guess1=recomp_cycle%recomp_frac
		 lt_frac_guess1=recomp_cycle%LT%UA/(recomp_cycle%LT%UA+recomp_cycle%HT%UA)
		 LT_UA = recomp_cycle%LT%UA
		 HT_UA = recomp_cycle%HT%UA
		 
        LT_mdoth=recomp_cycle%LT%m_dot_design(2)
        LT_mdotc=recomp_cycle%LT%m_dot_design(1)
        LT_Tcin=recomp_cycle%LT%T_c_in
        LT_Thin=recomp_cycle%LT%T_h_in
        LT_Pcin=recomp_cycle%LT%P_c_in
        LT_Phin=recomp_cycle%LT%P_h_in
        LT_Pcout=recomp_cycle%LT%P_c_out
        LT_Phout=recomp_cycle%LT%P_h_out
        LT_Q=recomp_cycle%LT%Q_dot
        LT_Effc=recomp_cycle%LT%eff
		
        HT_mdoth=recomp_cycle%HT%m_dot_design(1)
        HT_mdotc=recomp_cycle%HT%m_dot_design(2)
        HT_Tcin=recomp_cycle%HT%T_c_in
        HT_Thin=recomp_cycle%HT%T_h_in
        HT_Pcin=recomp_cycle%HT%P_c_in
        HT_Phin=recomp_cycle%HT%P_h_in
        HT_Pcout=recomp_cycle%HT%P_c_out
        HT_Phout=recomp_cycle%HT%P_h_out
        HT_Q=recomp_cycle%HT%Q_dot
        HT_Effc=recomp_cycle%HT%eff
		
        N_design= recomp_cycle%mc%N

        PHX_Q1=recomp_cycle%PHX%Q_dot
        PC_Q1=recomp_cycle%PC%Q_dot

       end subroutine carbondioxidemixturesubplex

