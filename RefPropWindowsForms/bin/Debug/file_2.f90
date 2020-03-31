integer, parameter  :: dp = selected_real_kind(15)          ! double precision
real(dp), parameter :: comp_array = 1.0_dp                  ! composition of the mixture
dimension x(nc)
character(len=3),   parameter :: reference_state = 'DEF'    ! use the default reference state for each fluid
character(len=255), parameter :: mixture_file = 'C:\sCO2 Heat Balances C#\sCO2 Heat Balances C#\sCO2 Heat Balances C#\sCO2 Heat Balances C#\RefPropWindowsForms\RefPropWindowsForms\bin\Debug\fluids\HMX.BNC'   ! default mixture coefficients
!character(len=255), parameter :: fluid = 'C:\Users\luis\Desktop\Dyreby-LUIS COCO\Dyreby-LUIS COCO\SB and RC sCO2_brayton_mode - without ReHeating\Properties\REFPROP\fluids\CO2.FLD'  ! path to CO2.FLD
!character(len=255), parameter :: fluid ='CO2.FLD' 
character*255 hf(nc)

! Module Variables
logical, save :: initialized = .false.
character(len=255) :: error_message
integer :: error_code
real(dp) :: wmm

contains

      subroutine initialize()
      real(dp), external :: wmol
      