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
! This file contains a module with multiple subroutines that call the REFPROP flash routines for carbon dioxide.
!
! For each subroutine, the first two arguments are the known properties:
!   T -- temperature (K)
!   D -- density (kg/m3)
!   P -- pressure (kPa)
!   H -- enthalpy (kJ/kg)
!   S -- entropy (kJ/kg-K)
!
! All the outputs except 'error_code' are optional:
!   temp -- temperature (K)
!   pres -- pressure (kPa)
!   dens -- density (kg/m3)
!   enth -- enthalpy (kJ/kg)
!   entr -- entropy (kJ/kg-K)
!   ssnd -- speed of sound in the fluid (m/s)
!
! Notes:
!   1) The REFPROP source code is not provided and must be purchased from http://www.nist.gov/srd/nist23.cfm and
!      linked to during compilation of this module.  To use REFPROP with the program "generate_paper_results.py", the
!      easiest way to do this is to copy all the REFPROP source code files into the directory containing this file; the
!      "create_python_interface.py" program will then find and compile them automatically.
!   2) The parameter 'fluid' is the full path to the CO2.FLD fluid definition file (syntax is platform specific).
!   3) If an error occurs during initialization, the calling program will be stopped.
!
! Author: John Dyreby, Solar Energy Laboratory, University of Wisconsin-Madison <jjdyreby@uwalumni.com>
!
! Last Modified: July 10, 2014
!
!-----------------------------------------------------------------------------------------------------------------------------------


module CO2_properties

implicit none

! Parameters
integer,  parameter :: nc = 1                               ! number of components in the mixture
integer, parameter  :: dp = selected_real_kind(15)          ! double precision
real(dp), parameter :: comp_array = 1.0_dp                  ! composition of the mixture
character(len=3),   parameter :: reference_state = 'DEF'    ! use the default reference state for each fluid
character(len=255), parameter :: mixture_file = 'HMX.BNC'   ! default mixture coefficients
character(len=255), parameter :: fluid = 'METHANE.FLD'  ! path to CO2.FLD

! Module Variables
logical, save :: initialized = .false.
character(len=255) :: error_message
integer :: error_code
real(dp) :: wmm


contains


subroutine initialize()
    real(dp), external :: wmol
    call setup(nc, fluid, mixture_file, reference_state, error_code, error_message)
    if (error_code /= 0) then
        write (*,*) 'The following error occurred during REFPROP initialization:'
        write (*,*) error_message
        stop
    end if
    wmm = wmol(comp_array)
    initialized = .true.
end subroutine initialize


subroutine CO2_TD(T, D, error_code, temp, pres, dens, enth, entr, ssnd)
    real(dp), intent(in) :: T, D
    integer, intent(out) :: error_code
    real(dp), intent(out), optional :: temp, pres, dens, enth, entr, ssnd
    real(dp) :: pres_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap
    real(dp) :: qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP
    if (.not. initialized) call initialize()
    dens_mol = D / wmm  ! convert density to molar basis
    call TDFLSH(T, dens_mol, comp_array, pres_RP, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap, &
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
    call TPFLSH(T, P, comp_array, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap, &
                qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP, error_code, error_message)
    if (present(temp)) temp = T
    if (present(pres)) pres = P
    if (present(dens)) dens = dens_mol * wmm
    if (present(enth)) enth = enth_mol / wmm
    if (present(entr)) entr = entr_mol / wmm
    if (present(ssnd)) ssnd = ssnd_RP
end subroutine CO2_TP


subroutine CO2_PH(P, H, error_code, temp, pres, dens, enth, entr, ssnd)
    real(dp), intent(in) :: P, H
    integer, intent(out) :: error_code
    real(dp), intent(out), optional :: temp, pres, dens, enth, entr, ssnd
    real(dp) :: temp_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap
    real(dp) :: qual, inte_mol, enth_mol, entr_mol, cv_mol, cp_mol, ssnd_RP
    if (.not. initialized) call initialize()
    enth_mol = H * wmm  ! convert enthalpy to molar basis
    call PHFLSH(P, enth_mol, comp_array, temp_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap, &
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
    call PSFLSH(P, entr_mol, comp_array, temp_RP, dens_mol, dens_liq_mol, dens_vap_mol, comp_array_liq, comp_array_vap, &
                qual, inte_mol, enth_mol, cv_mol, cp_mol, ssnd_RP, error_code, error_message)
    if (present(temp)) temp = temp_RP
    if (present(pres)) pres = P
    if (present(dens)) dens = dens_mol * wmm
    if (present(enth)) enth = enth_mol / wmm
    if (present(entr)) entr = S
    if (present(ssnd)) ssnd = ssnd_RP
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
    call HSFLSH(enth_mol, entr_mol, comp_array, temp_RP, pres_RP, dens_mol, dens_liq_mol, dens_vap_mol, &
                comp_array_liq, comp_array_vap, qual, inte_mol, cv_mol, cp_mol, ssnd_RP, error_code, error_message)
    if (present(temp)) temp = temp_RP
    if (present(pres)) pres = pres_RP
    if (present(dens)) dens = dens_mol * wmm
    if (present(enth)) enth = H
    if (present(entr)) entr = S
    if (present(ssnd)) ssnd = ssnd_RP
end subroutine CO2_HS


end module CO2_properties
