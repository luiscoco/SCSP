	  
      wmm = wmol(x)
      write (*,1000) 'Molar Weight: ',wmm

      call CRITP (x,tcrit,pcrit,Dcrit,error_code,error_message)
      write (*,1000) 'Critical Point (TCmix, PCmix, Dcmix): ',tcrit,pcrit,Dcrit
	  
      initialized = .true.
	  
1000  format (1x,a22,5f11.4)
      end subroutine initialize


subroutine CO2_TD(T, D, error_code, temp, pres, dens, enth, entr, ssnd)
    real(dp), intent(in) :: T, D
    integer, intent(out) :: error_code
    real(dp), intent(out), optional :: temp, pres, dens, enth, entr, ssnd
    real(dp) :: pres_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap
    real(dp) :: qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP
    if (.not. initialized) call initialize()
    dens_mol = D / wmm  ! convert density to molar basis
    call TDFLSH(T, dens_mol, x, pres_RP, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap, &
                qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP, error_code, error_message)
	if (present(temp)) temp = T
    if (present(pres)) pres = pres_RP
    if (present(dens)) dens = D
    if (present(enth)) enth = enth_mol / wmm
    if (present(entr)) entr = entr_mol / wmm
    if (present(ssnd)) ssnd = ssnd_RP
end subroutine CO2_TD


      subroutine CO2_TP(T, P, error_code, temp, pres, dens, enth, entr, ssnd)
       real(dp), intent(in) :: T, P
       integer, intent(out) :: error_code
       real(dp), intent(out), optional :: temp, pres, dens, enth, entr, ssnd
       real(dp) :: dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap
       real(dp) :: qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP
       if (.not. initialized) call initialize()
       call TPFLSH(T, P, x, dens_mol, dens_liq_mol, dens_vap_mol,&
                   comp_array_liq, comp_array_vap,&
                   qual, inte_mol, enth_mol, entr_mol, cv_mol,&
                   cp_mol, ssnd_RP, error_code, error_message)
       if (present(temp)) temp = T
       if (present(pres)) pres = P
       if (present(dens)) dens = dens_mol * wmm
       if (present(enth)) enth = enth_mol / wmm
       if (present(entr)) entr = entr_mol / wmm
       if (present(ssnd)) ssnd = ssnd_RP 
       
      print *,'Error_Code: ',error_code	 
      print *,'Error_message: ',error_message			 
      
       end subroutine CO2_TP


subroutine CO2_PH(P, H, error_code, temp, pres, dens, enth, entr, ssnd)
    real(dp), intent(in) :: P, H
    integer, intent(out) :: error_code
    real(dp), intent(out), optional :: temp, pres, dens, enth, entr, ssnd
    real(dp) :: temp_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap
    real(dp) :: qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP
    if (.not. initialized) call initialize()
    enth_mol = H * wmm  ! convert enthalpy to molar basis
    call PHFLSH(P, enth_mol, x, temp_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap, &
                qual, inte_mol, entr_mol, cv_mol, cp_mol, ssnd_RP, error_code, error_message)
    if (present(temp)) temp = temp_RP
    if (present(pres)) pres = P
    if (present(dens)) dens = dens_mol * wmm
    if (present(enth)) enth = H
    if (present(entr)) entr = entr_mol / wmm
    if (present(ssnd)) ssnd = ssnd_RP
end subroutine CO2_PH


subroutine CO2_PS(P, S, error_code, temp, pres, dens, enth, entr, ssnd)
    real(dp), intent(in) :: P, S
    integer, intent(out) :: error_code
    real(dp), intent(out), optional :: temp, pres, dens, enth, entr, ssnd
    real(dp) :: temp_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap
    real(dp) :: qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP
    if (.not. initialized) call initialize()
    entr_mol = S * wmm  ! convert entropy to molar basis
    call PSFLSH(P, entr_mol, x, temp_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap, &
                qual, inte_mol, enth_mol, cv_mol, cp_mol, ssnd_RP, error_code, error_message)
    if (present(temp)) temp = temp_RP
    if (present(pres)) pres = P
    if (present(dens)) dens = dens_mol * wmm
    if (present(enth)) enth = enth_mol / wmm
    if (present(entr)) entr = S
    if (present(ssnd)) ssnd = ssnd_RP
      print *,'CO2_PS call:'
      print *,'Error_Code: ',error_code	 
      print *,'Error_message: ',error_message	
end subroutine CO2_PS


subroutine CO2_HS(H, S, error_code, temp, pres, dens, enth, entr, ssnd)
    real(dp), intent(in) :: H, S
    integer, intent(out) :: error_code
    real(dp), intent(out), optional :: temp, pres, dens, enth, entr, ssnd
    real(dp) :: temp_RP, pres_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap
    real(dp) :: qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP
    if (.not. initialized) call initialize()
    enth_mol = H * wmm  ! convert enthalpy to molar basis
    entr_mol = S * wmm  ! convert entropy to molar basis
    call HSFLSH(enth_mol, entr_mol, x, temp_RP, pres_RP, dens_mol, dens_liq_mol, dens_vap_mol, &
                comp_array_liq, comp_array_vap, qual, inte_mol, cv_mol, cp_mol, ssnd_RP, error_code, error_message)
    if (present(temp)) temp = temp_RP
    if (present(pres)) pres = pres_RP
    if (present(dens)) dens = dens_mol * wmm
    if (present(enth)) enth = H
    if (present(entr)) entr = S
    if (present(ssnd)) ssnd = ssnd_RP
end subroutine CO2_HS


end module CO2_properties
