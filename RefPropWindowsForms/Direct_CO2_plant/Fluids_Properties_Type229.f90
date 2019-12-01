SUBROUTINE TYPE229 (TIME,XIN,OUT,T,DTDT,PAR,INFO,ICNTRL,*) 
!************************************************************************
! Object: Substance Properties
! Simulation Studio Model: Type229
! 
! Author: Michael J Wagner
! Editor: MJW
! Date:	 February 02, 2009 
! Last modified: October 18, 2010
!       Note, MJW: The nested property functions for the trough fluids were consolidated. Expected input units 
!                  were changed to [K] for all functions. 
!       Note, MJW: The property subroutines that contain range checks were moved to a dedicated error checking subroutine 9.8.2010
!             MJW: When consolidating property functions, the user-specified property function wasn't converted from Kelvin to 
!                  Celsius (interface table is specified in Celsius). This is fixed after 10/18/2010. 
! COPYRIGHT 2010 NATIONAL RENEWABLE ENERGY LABORATORY

! Doc. tables updated 7/1/2010 - MJW
!--------------------------------------------------------------------------------------------------------------------------------------------
! Nb  | Variable                         | Description                                                       | Input units      | Local units      
!--------------------------------------------------------------------------------------------------------------------------------------------
!Parameters
!    1| Fluid_number                     | Integer flag denoting HTF fluid                                   | none             | none             
!    2| LU_FL                            | Fluid data logical unit                                           | none             | none             
!    3| LuFlEr                           | Fluid property error file unit                                    | none             | none             

!--------------------------------------------------------------------------------------------------------------------------------------------
! Nb  | Variable                         | Description                                                       | Input units      | Local units      
!--------------------------------------------------------------------------------------------------------------------------------------------
!Inputs
!    1| Temperature                      | Temperature of the HTF                                            | C                | K                

!--------------------------------------------------------------------------------------------------------------------------------------------
! Nb  | Variable                         | Description                                                       | Input units      | Local units      
!--------------------------------------------------------------------------------------------------------------------------------------------
!Outputs
!    1| c                                | Fluid specific heat                                               | kJ/kg.K          | kJ/kg.K          
!    2| rho                              | Fluid density                                                     | kg/m3            | kg/m3            
!    3| mu                               | Fluid viscosity                                                   | Pa.s             | Pa.s             
!    4| k                                | Fluid thermal conductivity                                        | W/m.K            | W/m.K            


!************************************************************************

!    TRNSYS acess functions (allow to acess TIME etc.) 
USE TrnsysConstants
USE TrnsysFunctions
!    Special access functions for fluid properties
use global_props      

!-----------------------------------------------------------------------------------------------------------------------
!    REQUIRED BY THE MULTI-DLL VERSION OF TRNSYS
!DEC$ATTRIBUTES DLLEXPORT :: TYPE229				!SET THE CORRECT TYPE NUMBER HERE
!-----------------------------------------------------------------------------------------------------------------------
!-----------------------------------------------------------------------------------------------------------------------
!    TRNSYS DECLARATIONS
IMPLICIT NONE			!REQUIRES THE USER TO DEFINE ALL VARIABLES BEFORE USING THEM

DOUBLE PRECISION XIN	!THE ARRAY FROM WHICH THE INPUTS TO THIS TYPE WILL BE RETRIEVED
DOUBLE PRECISION OUT	!THE ARRAY WHICH WILL BE USED TO STORE THE OUTPUTS FROM THIS TYPE
DOUBLE PRECISION TIME	!THE CURRENT SIMULATION TIME - YOU MAY USE THIS VARIABLE BUT DO NOT SET IT!
DOUBLE PRECISION PAR	!THE ARRAY FROM WHICH THE PARAMETERS FOR THIS TYPE WILL BE RETRIEVED
DOUBLE PRECISION STORED !THE STORAGE ARRAY FOR HOLDING VARIABLES FROM TIMESTEP TO TIMESTEP
DOUBLE PRECISION T		!AN ARRAY CONTAINING THE RESULTS FROM THE DIFFERENTIAL EQUATION SOLVER
DOUBLE PRECISION DTDT	!AN ARRAY CONTAINING THE DERIVATIVES TO BE PASSED TO THE DIFF.EQ. SOLVER
INTEGER*4 INFO(15)		!THE INFO ARRAY STORES AND PASSES VALUABLE INFORMATION TO AND FROM THIS TYPE
INTEGER*4 NP,NI,NOUT,ND	!VARIABLES FOR THE MAXIMUM NUMBER OF PARAMETERS,INPUTS,OUTPUTS AND DERIVATIVES
INTEGER*4 NPAR,NIN,NDER	!VARIABLES FOR THE CORRECT NUMBER OF PARAMETERS,INPUTS,OUTPUTS AND DERIVATIVES
INTEGER*4 IUNIT,ITYPE	!THE UNIT NUMBER AND TYPE NUMBER FOR THIS COMPONENT
INTEGER*4 ICNTRL		!AN ARRAY FOR HOLDING VALUES OF CONTROL FUNCTIONS WITH THE NEW SOLVER
INTEGER*4 NSTORED		!THE NUMBER OF VARIABLES THAT WILL BE PASSED INTO AND OUT OF STORAGE
CHARACTER*3 OCHECK		!AN ARRAY TO BE FILLED WITH THE CORRECT VARIABLE TYPES FOR THE OUTPUTS
CHARACTER*3 YCHECK		!AN ARRAY TO BE FILLED WITH THE CORRECT VARIABLE TYPES FOR THE INPUTS
! ----------------------------------------------------------------------------------------------------------------------

! ----------------------------------------------------------------------------------------------------------------------
!    USER DECLARATIONS - SET THE MAXIMUM NUMBER OF PARAMETERS (NP), INPUTS (NI),
!    OUTPUTS (NOUT), AND DERIVATIVES (ND) THAT MAY BE SUPPLIED FOR THIS TYPE
PARAMETER (NP=3,NI=1,NOUT=4,ND=0,NSTORED=1)
! ----------------------------------------------------------------------------------------------------------------------

! ----------------------------------------------------------------------------------------------------------------------
!    REQUIRED TRNSYS DIMENSIONS
DIMENSION XIN(NI),OUT(NOUT),PAR(NP),YCHECK(NI),OCHECK(NOUT),STORED(NSTORED),T(ND),DTDT(ND)
INTEGER NITEMS
! ----------------------------------------------------------------------------------------------------------------------
! ----------------------------------------------------------------------------------------------------------------------
!    ADD DECLARATIONS AND DEFINITIONS FOR THE USER-VARIABLES HERE


!    PARAMETERS
DOUBLE PRECISION Fluid_number 
integer::LU_FL

!    INPUTS
DOUBLE PRECISION Temperature

!     Outputs
DOUBLE PRECISION c, rho, mu, k

!     Locals
DOUBLE PRECISION specheat, Density, Viscosity, Conductivity 
!      DOUBLE PRECISION,ALLOCATABLE:: fprop(:,:)
!      CHARACTER*40 Coolist(2)
CHARACTER checkname*500, tc*1
INTEGER ios,ntot, nufl, i, j, m, actb  !fl_bounds(25)
LOGICAL Is_there

      
! ----------------------------------------------------------------------------------------------------------------------
!       READ IN THE VALUES OF THE PARAMETERS IN SEQUENTIAL ORDER
Fluid_number=PAR(1)
LU_FL=int(PAR(2))
LuFlEr = int(par(3)) !Note LuFlEr is stored globally in the global_props module
ttime=time  !Store time as a global variable
! ----------------------------------------------------------------------------------------------------------------------
!    RETRIEVE THE CURRENT VALUES OF THE INPUTS TO THIS MODEL FROM THE XIN ARRAY IN SEQUENTIAL ORDER

Temperature=XIN(1)
IUNIT=INFO(1)
ITYPE=INFO(2)

! ----------------------------------------------------------------------------------------------------------------------
!    SET THE VERSION INFORMATION FOR TRNSYS
IF(INFO(7).EQ.-2) THEN
    INFO(12)=16
    RETURN 1
ENDIF
! ----------------------------------------------------------------------------------------------------------------------

! ----------------------------------------------------------------------------------------------------------------------
!    DO ALL THE VERY LAST CALL OF THE SIMULATION MANIPULATIONS HERE
IF (INFO(8).EQ.-1) THEN
    if(allocated(fprop)) deallocate(fprop)

        inquire(unit=LuFlEr,opened=is_there)
        if(is_there) close(LuFlEr)

    RETURN 1
ENDIF
! ----------------------------------------------------------------------------------------------------------------------

! ----------------------------------------------------------------------------------------------------------------------
!    PERFORM ANY 'AFTER-ITERATION' MANIPULATIONS THAT ARE REQUIRED HERE
!    e.g. save variables to storage array for the next timestep
IF (INFO(13).GT.0) THEN
    STORED(1)=Gjsav
    CALL setStorageVars(STORED,NSTORED,INFO)
    RETURN 1
ENDIF

! ----------------------------------------------------------------------------------------------------------------------

! ----------------------------------------------------------------------------------------------------------------------
!    DO ALL THE VERY FIRST CALL OF THE SIMULATION MANIPULATIONS HERE
IF (INFO(7).EQ.-1) THEN

    !       SET SOME INFO ARRAY VARIABLES TO TELL THE TRNSYS ENGINE HOW THIS TYPE IS TO WORK
    INFO(6)=NOUT				
    INFO(9)=1				
    INFO(10)=0	!STORAGE FOR VERSION 16 HAS BEEN CHANGED				

    !       SET THE REQUIRED NUMBER OF INPUTS, PARAMETERS AND DERIVATIVES THAT THE USER SHOULD SUPPLY IN THE INPUT FILE
    !       IN SOME CASES, THE NUMBER OF VARIABLES MAY DEPEND ON THE VALUE OF PARAMETERS TO THIS MODEL....
    NIN=NI
    NPAR=NP
    NDER=ND

    !       CALL THE TYPE CHECK SUBROUTINE TO COMPARE WHAT THIS COMPONENT REQUIRES TO WHAT IS SUPPLIED IN 
    !       THE TRNSYS INPUT FILE
    CALL TYPECK(1,INFO,NIN,NPAR,NDER)

    !       SET THE NUMBER OF STORAGE SPOTS NEEDED FOR THIS COMPONENT
    CALL setStorageSize(NSTORED,INFO)

    !MJW 7/09 Open the warnings file
    inquire(unit=LuFlEr,name=checkname,opened=is_there)
    !open the file for writing
    if(.not.is_there) then
        open(unit=LuFlEr,file=trim(checkname),status="REPLACE",position="REWIND")
    endif


    !       RETURN TO THE CALLING PROGRAM
    RETURN 1

ENDIF
! ----------------------------------------------------------------------------------------------------------------------

! ----------------------------------------------------------------------------------------------------------------------
!    DO ALL OF THE INITIAL TIMESTEP MANIPULATIONS HERE - THERE ARE NO ITERATIONS AT THE INTIAL TIME
      IF (TIME .LT. (getSimulationStartTime() + getSimulationTimeStep()/2.D0)) THEN

!       SET THE UNIT NUMBER FOR FUTURE CALLS
         IUNIT=INFO(1)
         ITYPE=INFO(2)

        !On the initial timestep call, read in the fluid property file

        call readFluidPropFile(LU_FL)       
        fl_flag = 0. !Always reset the fluid warning flag to zero
        fl_ct = 0.  !and the warning counter too


!       PERFORM ANY REQUIRED CALCULATIONS TO SET THE INITIAL VALUES OF THE OUTPUTS HERE
!		 Specific heat capacity
			OUT(1)=1.53
!		 Density
			OUT(2)=1750.
!		 Viscosity
			OUT(3)=1e-5
!		 Conductivity
			OUT(4)=.5

!       PERFORM ANY REQUIRED CALCULATIONS TO SET THE INITIAL STORAGE VARIABLES HERE
	   STORED(1)=0. !Gjsav

!       PUT THE STORED ARRAY IN THE GLOBAL STORED ARRAY
         CALL setStorageVars(STORED,NSTORED,INFO)

!       RETURN TO THE CALLING PROGRAM
         RETURN 1

      ENDIF
! ----------------------------------------------------------------------------------------------------------------------

! ----------------------------------------------------------------------------------------------------------------------
!    *** ITS AN ITERATIVE CALL TO THIS COMPONENT ***
! ----------------------------------------------------------------------------------------------------------------------


! The following scheme is used to define the substance number.  Some fluids are
! blocked off for the trough model, and some are blocked off for future use. 
! Any fluid number specified greater than 35 will be a user-defined fluid.
!    1.) Air
!    2.) Stainless_AISI316
!    3.) Water (liquid)
!    4.) Steam
!    5.) CO2
!    6.) Salt (68% KCl, 32% MgCl2)
!    7.) Salt (8% NaF, 92% NaBF4)
!    8.) Salt (25% KF, 75% KBF4)
!    9.) Salt (31% RbF, 69% RbBF4)
!    10.) Salt (46.5% LiF, 11.5%NaF, 42%KF)
!    11.) Salt (49% LiF, 29% NaF, 29% ZrF4)
!    12.) Salt (58% KF, 42% ZrF4)
!    13.) Salt (58% LiCl, 42% RbCl)
!    14.) Salt (58% NaCl, 42% MgCl2)
!    15.) Salt (59.5% LiCl, 40.5% KCl)
!    16.) Salt (59.5% NaF, 40.5% ZrF4)
!    17.) Salt (60% NaNO3, 40% KNO3)
!    18.) Nitrate Salt**
!    19.) Caloria HT 43**
!    20.) Hitec XL**
!    21.) Therminol VP-1**
!    22.) Hitec**
!    23.) Dowtherm Q**
!    24.) Dowtherm RP**
!    26.) Argon (ideal gas properties)
!    27.) Hydrogen (ideal gas properties)
!    28.) -blank-
!    29.) Therminol 66
!    30.) Therminol 59
!    31.) -blank-
!    32.) -blank-
!    33.) -blank-
!    34.) -blank-
!    35.) -blank-
!    36+) User specified (lookup tables)

!     Note that the fluid properties are stored in the FPROP array in the following order:
!*****************************************************************
!|  #    |   1   |   2   |   3   |   4   |   5   |   6   |   7   |
!|-------|-------|-------|-------|-------|-------|-------|-------|
!| Name  |   T   |   Cp  |  rho  |   Mu  |   Nu  |   k   |   h   |
!| Units |   C   |kJ/kg-K| kg/m3 |  Pa-s |  m2-s | W/m-K |  J/kg |
!|-------|-------|-------|-------|-------|-------|-------|-------|
!|   1   |   :   |   :   |   :   |   :   |   :   |   :   |   :   |
!|   2   |   :   |   :   |   :   |   :   |   :   |   :   |   :   |
!*****************************************************************

     

!       GET THE STORED ARRAY FROM THE GLOBAL STORED ARRAY
         CALL getStorageVars(STORED,NSTORED,INFO)
	   Gjsav=STORED(1)  !Gjsav is a module variable, shared between this and the subfunctions


      c=specheat(Fluid_number,Temperature+273.15,101325.d0)
      rho=Density(Fluid_number,Temperature+273.15,dble(1.))
      mu=Viscosity(Fluid_number,Temperature+273.15,101325.d0)
      k=Conductivity(Fluid_number,Temperature+273.15,101325.d0)




!     Send the updated shared Gjsav back to storage
      STORED(1)=Gjsav
      call setStorageVars(STORED,NSTORED,INFO)



    !-----------------------------------------------------------------------------------------------------------------------

    !    SET THE OUTPUTS FROM THIS MODEL IN SEQUENTIAL ORDER AND GET OUT

    !Specific heat capacity
    OUT(1)=c
    !Density
    OUT(2)=rho
    !Viscosity
    OUT(3)=mu
    !Conductivity
    OUT(4)=k

!-----------------------------------------------------------------------------------------------------------------------
!    EVERYTHING IS DONE - RETURN FROM THIS SUBROUTINE AND MOVE ON
      RETURN 1
      END
!-----------------------------------------------------------------------------------------------------------------------

subroutine check_htf(fnumd,T)

implicit none

real(8),intent(in):: T, fnumd
real(8):: xlo, xhi
integer:: dum, t_warn, fnum

fnum = int(fnumd)

select case(fnum)
case(1:5)
  !No information
  continue
case(6)   !    6.) Salt (68% KCl, 32% MgCl2)
    xlo=699.; xhi=1691.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (68% KCl, 32% MgCl2)")
case(7)   !    7.) Salt (8% NaF, 92% NaBF4)
    xlo=658.; xhi=969.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (8% NaF, 92% NaBF4)")
case(8)   !    8.) Salt (25% KF, 75% KBF4)
    xlo=699.; xhi=1691.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (25% KF, 75% KBF4)")
case(9)   !    9.) Salt (31% RbF, 69% RbBF4)
    xlo=715.; xhi=1343.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (31% RbF, 69% RbBF4)")
case(10)   !    10.) Salt (46.5% LiF, 11.5%NaF, 42%KF)
    xlo=727.; xhi=1843.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (46.5% LiF, 11.5%NaF, 42%KF)")
case(11)   !    11.) Salt (49% LiF, 29% NaF, 29% ZrF4)
    xlo=709.; xhi=1673.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (49% LiF, 29% NaF, 29% ZrF4)")
case(12)   !    12.) Salt (58% KF, 42% ZrF4)
    xlo=773.; xhi=1623.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (58% KF, 42% ZrF4)")
case(13)   !    13.) Salt (58% LiCl, 42% RbCl)
    xlo=586.; xhi=1323.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (58% LiCl, 42% RbCl)")
case(14)   !    14.) Salt (58% NaCl, 42% MgCl2)
    xlo=718.; xhi=1738.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (58% NaCl, 42% MgCl2)")
case(15)   !    15.) Salt (59.5% LiCl, 40.5% KCl)
    xlo=628.; xhi=1673.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (59.5% LiCl, 40.5% KCl)")
case(16)   !    16.) Salt (59.5% NaF, 40.5% ZrF4)
    xlo=773.; xhi=1623.;
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Salt (59.5% NaF, 40.5% ZrF4)")
case(17)   !    17.) Salt (60% NaNO3, 40% KNO3)
    xlo=493.; xhi=866.;     !MJW 10.21.2010: Reduced freezing temp to 220C (493.15K), reference http://www.nrel.gov/csp/troughnet/pdfs/40028.pdf
    if(T.lt.xlo.or.T.gt.xhi)then 
        dum=t_warn(T,xlo,xhi,"Salt (60% NaNO3, 40% KNO3)")
    endif
case(18)    !Nitrate Salt, [kg/m3]
    xlo=493.; xhi=866.;     !MJW 10.21.2010: Reduced freezing temp to 220C (493.15K), reference http://www.nrel.gov/csp/troughnet/pdfs/40028.pdf
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Nitrate Salt (60% NaNO3, 40% KNO3)")
case(19)    !   19.) Caloria HT 43
    xlo=274.; xhi=588.;
    if(T.lt.xlo.or.T.gt.xhi)then 
        dum=t_warn(T,xlo,xhi,"Caloria HT-43")
    endif
case(20)    !   20.) HitecXL 
    xlo=393.; xhi=773.;     !TWN 9.9.2012: Added. Reference: "LOW MELTING POINT MOLTEN SALT HEAT TRANSFER FLUID WITH REDUCED COST", Raade et al.
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"HitecXL")
case(21)    !   21.) Therminol VP-1
    xlo=285.; xhi=673.;     !TWN 9.4.2012: Added. Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp 
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Therminol VP-1")
case(22)    !   22.) Hitec
    xlo=415.; xhi=811.;     !TWN 9.9.2012: Added. Reference: "LOW MELTING POINT MOLTEN SALT HEAT TRANSFER FLUID WITH REDUCED COST", Raade et al.    
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Hitec")
case(23)    !   23.) DowTherm Q                  
    xlo=238.; xhi=603.;     !TWN 9.9.2012: Added. Reference: http://www.dow.com/heattrans/products/synthetic/dowtherm.htm
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"DowTherm Q")
case(24)    !   24.) DowTherm RP
    ! ***** Low temperature is a place-holder, the correct value is not provided by the reference
    xlo=273.; xhi=623.;     !TWN 9.9.2012: Added. Reference: http://www.dow.com/heattrans/products/synthetic/dowtherm.htm
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"DowTherm RP")  
case(25:28)
    continue  
case(29)    !   29.) Therminol 66
    xlo=273.; xhi=618.;     !TWN 9.4.2012: Added. Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Therminol 66")  
case(30)    !   30.) Therminol 59
    xlo=228.; xhi=588.;     !TWN 9.4.2012: Added. Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
    if(T.lt.xlo.or.T.gt.xhi) dum=t_warn(T,xlo,xhi,"Therminol 59")  
case(31:)
    continue !no informaion
end select


end subroutine


double precision function Density(fnumd,T,P)
use global_props

implicit none
!This function accepts as inputs temperature [K] and pressure [Pa]
!This function outputs in units of [kg/m^3]
double precision::xlo,xhi, Dens_fluid, Td
double precision::T,P,fnumd
double precision,dimension(size(fprop(1,:)))::dxx,dyy !Create dummy arrays
integer::fnum,lb,ub,dum,t_warn
!Density=1.
fnum=nint(fnumd)
Td=T-273.15             !Convert from K to C

select case(fnum)
case(1)   !    1.) Air
Density = P/(287.*T)
case(2)   !    2.) Stainless_AISI316
  Density=8349.38 - 0.341708*T - 0.0000865128*T*T  !EES
case(3)   !    3.) Water (liquid)
  Density = 1000 
case(4)   !    4.) Steam
  continue
case(5)   !    5.) CO2
  continue
case(6)   !    6.) Salt (68% KCl, 32% MgCl2)
Density = 1E-10*T*T*T - 3E-07*T*T - 0.4739*T + 2384.2
case(7)   !    7.) Salt (8% NaF, 92% NaBF4)
Density = 8E-09*T*T*T - 2E-05*T*T - 0.6867*T + 2438.5
case(8)   !    8.) Salt (25% KF, 75% KBF4)
Density = 2E-08*T*T*T - 6E-05*T*T - 0.7701*T + 2466.1
case(9)   !    9.) Salt (31% RbF, 69% RbBF4)
Density = -1E-08*T*T*T + 4E-05*T*T - 1.0836*T + 3242.6
case(10)   !    10.) Salt (46.5% LiF, 11.5%NaF, 42%KF)
Density =  -2E-09*T*T*T + 1E-05*T*T - 0.7427*T + 2734.7
case(11)   !    11.) Salt (49% LiF, 29% NaF, 29% ZrF4)
Density = -2E-11*T*T*T + 1E-07*T*T - 0.5172*T + 3674.3
case(12)   !    12.) Salt (58% KF, 42% ZrF4)
Density =  -6E-10*T*T*T + 4E-06*T*T - 0.8931*T + 3661.3
case(13)   !    13.) Salt (58% LiCl, 42% RbCl)
Density = -8E-10*T*T*T + 1E-06*T*T - 0.689*T + 2929.5
case(14)   !    14.) Salt (58% NaCl, 42% MgCl2)
Density = -5E-09*T*T*T + 2E-05*T*T - 0.5298*T + 2444.1
case(15)   !    15.) Salt (59.5% LiCl, 40.5% KCl)
Density = 1E-09*T*T*T - 5E-06*T*T - 0.864*T + 2112.6
case(16)   !    16.) Salt (59.5% NaF, 40.5% ZrF4)
Density =  -5E-09*T*T*T + 2E-05*T*T - 0.9144*T + 3837.
case(17)   !    17.) Salt (60% NaNO3, 40% KNO3)
Density = dmax1(-1E-07*T*T*T + 0.0002*T*T - 0.7875*T + 2299.4,1000.d0)
case(18)
!Density of Nitrate Salt, [kg/m3]
Density = dmax1(2090 - 0.636 * (T-273.15),1000.d0)
case(19)
!Density of Caloria HT 43 [kg/m3]
Density = dmax1(885 - 0.6617 * Td - 0.0001265 * Td*Td,100.d0)
case(20)
!Density of HITEC XL Nitrate Salt, [kg/m3]
Density = dmax1(2240 - 0.8266 * Td,800.d0)
case(21)
!Density of Therminol Oil [kg/m3]
Density = dmax1(1074.0 - 0.6367 * Td - 0.0007762 * Td*Td,400.d0)
case(22)
!Density of HITEC Salt, [kg/m3]
Density = dmax1(2080 - 0.733 * Td,1000.d0)
case(23)
!Density of Dowtherm Q [kg/m3]
Density = dmax1(-0.757332 * Td + 980.787,100.d0)                               ! Russ 10-2-03
case(24)
!Density of Dowtherm RP [kg/m3]
Density = dmax1(-0.000186495 * Td*Td - 0.668337 * Td + 1042.11,200.d0)  !Russ 10-2-03
case(25)
!Density of HITEC XL Nitrate Salt, [kg/m^3]
Density = dmax1(2240 - 0.8266 * Td,800.d0)
case(26) !Argon
Density = dmax1(P/(208.13*T),1.e-10)
case(27) !Hydrogen
Density = dmax1(P/(4124.*T),1.e-10)
case(28)    !T-91 Steel: "Thermo hydraulic optimisation of the EURISOL DS target" - Paul Scherrer Institut
Density = -0.3289*Td + 7742.5
case(29)    !Therminol 66: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
Density = -0.7146*Td + 1024.8
case(30)    !Therminol 59: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
Density = -0.0003*Td*Td - 0.6963*Td + 988.44
case(31:35) 
continue !no informaion
case(36:) !Any integer greater than 35
!Call the user-defined property table
lb=fl_bounds(fnum-35)
ub=fl_bounds(fnum-35+1)-1
if(ub.lt.lb) ub=size(fprop(1,:))
dxx(:)=fprop(1,lb:ub)
dyy(:)=fprop(3,lb:ub)
call interp(Td,size(dxx),dxx,dyy,Gjsav,Density)
if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(Td,dxx(lb),dxx(ub),"User-specified fluid")
end select

end function

!&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

double precision function specheat(fnumd,T,P)
use global_props
implicit none
!This function accepts as inputs temperature [K] and pressure [Pa]
!This function outputs in units of [kJ/kg-K]
double precision::xlo,xhi, Td
double precision,intent(in)::T,P,fnumd
double precision,dimension(size(fprop(1,:)))::dxx,dyy !Create dummy arrays
integer::fnum,lb,ub,dum,t_warn
specheat=1.
fnum=nint(fnumd)
Td = T - 273.15

select case(fnum)
case(1)   !    1.) Air
specheat = 1.03749 - 0.000305497*T + 7.49335E-07*T*T - 3.39363E-10*T*T*T
!specheat = 1.03749 - 0.000305497*T + 7.49335E-07*T*T - 3.39363E-10*T*T*T
case(2)   !    2.) Stainless_AISI316
specheat = 0.368455 + 0.000399548*T - 1.70558E-07*T*T !EES
case(3)   !    3.) Water (liquid)
specheat = 4.181d0  !mjw 8.1.11 
case(4)   !    4.) Steam
  continue
case(5)   !    5.) CO2
  continue
case(6)   !    6.) Salt (68% KCl, 32% MgCl2)
specheat = 1.156
case(7)   !    7.) Salt (8% NaF, 92% NaBF4)
specheat = 1.507
case(8)   !    8.) Salt (25% KF, 75% KBF4)
specheat = 1.306
case(9)   !    9.) Salt (31% RbF, 69% RbBF4)
specheat = 9.127
case(10)   !    10.) Salt (46.5% LiF, 11.5%NaF, 42%KF)
specheat = 2.010
case(11)   !    11.) Salt (49% LiF, 29% NaF, 29% ZrF4)
specheat = 1.239
case(12)   !    12.) Salt (58% KF, 42% ZrF4)
specheat = 1.051
case(13)   !    13.) Salt (58% LiCl, 42% RbCl)
specheat = 8.918
case(14)   !    14.) Salt (58% NaCl, 42% MgCl2)
specheat = 1.080
case(15)   !    15.) Salt (59.5% LiCl, 40.5% KCl)
specheat = 1.202
case(16)   !    16.) Salt (59.5% NaF, 40.5% ZrF4)
specheat = 1.172
case(17)   !    17.) Salt (60% NaNO3, 40% KNO3)
specheat = -1E-10*T*T*T + 2E-07*T*T + 5E-06*T + 1.4387
case(18) !Heat Capacity of Nitrate Salt, [J/kg/K]
specheat = (1443. + 0.172 * (T-273.15))/1000.d0
case(19)
!Specific Heat of Caloria HT 43 [J/kgC]
specheat = (3.88 * (T-273.15) + 1606.0)/1000.
case(20)
!Heat Capacity of HITEC XL Nitrate Salt, [J/kg/K]
specheat = dmax1(1536 - 0.2624 * Td - 0.0001139 * Td * Td,1000.d0)/1000.
case(21)
!Specific Heat of Therminol Oil, J/kg/K
specheat = (1.509 + 0.002496 * Td + 0.0000007888 * Td*Td)
case(22)
!Heat Capacity of HITEC Salt, [J/kg/K]
specheat = (1560 - 0.0 * Td)/1000.
case(23)
!Specific Heat of Dowtherm Q, J/kg/K
specheat = (-0.00053943 * Td*Td + 3.2028 * Td + 1589.2)/1000.               ! Russ 10-2-03
case(24)
!Specific Heat of Dowtherm RP, J/kg/K
specheat = (-0.0000031915 * Td**2 + 2.977 * Td + 1560.8)/1000.       !Russ 10-2-03
case(25)
!Heat Capacity of HITEC XL Nitrate Salt, [J/kg/K]
specheat = dmax1(1536 - 0.2624 * Td - 0.0001139 * Td * Td,1000.d0)/1000.
case(26)    ! Argon
specheat = 0.5203 !Cp only, Cv is different
case(27)    ! Hydrogen
specheat = dmin1(dmax1(-45.4022 + 0.690156*T - 0.00327354*T*T + 0.00000817326*T*T*T - 1.13234E-08*T*T*T*T + 8.24995E-12*T*T*T*T*T - 2.46804E-15*T*T*T*T*T*T,11.3d0),14.7d0)
case(28)    !T-91 Steel: "Thermo hydraulic optimisation of the EURISOL DS target" - Paul Scherrer Institut
specheat = 0.0004*Td*Td + 0.2473*Td + 450.08
case(29)    !Therminol 66: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
specheat = 0.0036*Td + 1.4801   
case(30)    !Therminol 59: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
specheat = 0.0033*Td + 1.6132
case(31:35)
continue
case(36:) !Any integer greater than 35
!Call the user-defined property table
lb=fl_bounds(fnum-35)
ub=fl_bounds(fnum-35+1)-1
if(ub.lt.lb) ub=size(fprop(1,:))
dxx(:)=fprop(1,lb:ub)
dyy(:)=fprop(2,lb:ub)
call interp(Td,size(dxx),dxx,dyy,Gjsav,specheat)
        if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(Td,dxx(lb),dxx(ub),"User-specified fluid")
end select


end function

!&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

double precision function Cv(fnumd,T)
use global_props
implicit none
!This function accepts as inputs temperature [K] and pressure [Pa]
!This function outputs in units of kJ/kg-K
double precision::xlo,xhi
double precision,intent(in)::T,fnumd
double precision,dimension(size(fprop(1,:)))::dxx,dyy !Create dummy arrays
integer::fnum,lb,ub,dum,t_warn
Cv=1.
fnum=nint(fnumd)

select case(fnum)
case(1)     !Air
Cv = 0.750466 - 0.000305497*T + 7.49335E-07*T*T - 3.39363E-10*T*T*T
case(2:25) 
continue !no information
case(26)
Cv = 0.3122  !Argon
case(27)     !Hydrogen
Cv = dmin1(dmax1(-49.5264 + 0.690156*T - 0.00327354*T*T + 0.00000817326*T*T*T - 1.13234E-08*T**4 + 8.24995E-12*T**5 - 2.46804E-15*T**6,7.2d0),10.6d0)
case(28:35)
continue !no information
case(36:) !Any integer greater than 35
!Call the user-defined property table
!lb=fl_bounds(fnum-35)
!ub=fl_bounds(fnum-35+1)-1
!if(ub.lt.lb) ub=size(fprop(1,:))
!dxx(:)=fprop(1,lb:ub)
!dyy(:)=fprop(2,lb:ub)
!call interp(T,size(dxx),dxx,dyy,Gjsav,Cv)
!        if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(T,dxx(lb),dxx(ub),"User-specified fluid")

!NOTE: no column selected yet for Cv

end select


end function



!&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&


double precision function Viscosity(fnumd,T,P)
use global_props
implicit none
!This function accepts as inputs temperature [K] and pressure [Pa]
!This function outputs in units of [Pa-s]
double precision::Tx,xlo,xhi, density, Td
double precision,intent(in)::T,P,fnumd
double precision,dimension(size(fprop(1,:)))::dxx,dyy !Create dummy arrays
integer::fnum,lb,ub,dum,t_warn
viscosity=1.
fnum=nint(fnumd)
Td = T-273.15

select case(fnum)
case(1)   !    1.) Air
Viscosity = dmax1(0.0000010765 + 7.15173E-08*T - 5.03525E-11*T*T + 2.02799E-14*T*T*T,1.e-6)
case(2)   !    2.) Stainless_AISI316
  continue
case(3)   !    3.) Water (liquid)
  continue 
case(4)   !    4.) Steam
  continue
case(5)   !    5.) CO2
  continue
case(6)   !    6.) Salt (68% KCl, 32% MgCl2)
Viscosity = .0146*exp(2230./T)*0.001 !convert cP to kg/m-s
case(7)   !    7.) Salt (8% NaF, 92% NaBF4)
Viscosity = .0877*exp(2240./T)*0.001 !convert cP to kg/m-s
case(8)   !    8.) Salt (25% KF, 75% KBF4)
Viscosity = .0431*exp(3060./T)*0.001 !convert cP to kg/m-s
case(9)   !    9.) Salt (31% RbF, 69% RbBF4)
Viscosity = .0009
case(10)   !    10.) Salt (46.5% LiF, 11.5%NaF, 42%KF)
Viscosity = .0400*exp(4170./T)*0.001 !convert cP to kg/m-s
case(11)   !    11.) Salt (49% LiF, 29% NaF, 29% ZrF4)
Viscosity = .0069
case(12)   !    12.) Salt (58% KF, 42% ZrF4)
Viscosity = .0159*exp(3179./T)*0.001 !convert cP to kg/m-s
case(13)   !    13.) Salt (58% LiCl, 42% RbCl)
Viscosity = .0861*exp(2517./T)*0.001 !convert cP to kg/m-s          !
case(14)   !    14.) Salt (58% NaCl, 42% MgCl2)
Viscosity = .0286*exp(1441./T)*0.001 !convert cP to kg/m-s
case(15)   !    15.) Salt (59.5% LiCl, 40.5% KCl)
Viscosity = .0861*exp(2517./T)*0.001 !convert cP to kg/m-s          !
case(16)   !    16.) Salt (59.5% NaF, 40.5% ZrF4)
Viscosity = .0767*exp(3977./T)*0.001 !convert cP to kg/m-s
case(17)   !    17.) Salt (60% NaNO3, 40% KNO3)
Tx=T-273.15  !This particular equation is in terms of degrees celsius
Viscosity = dmax1(-1.473302E-10*Tx**3 + 2.279989E-07*Tx**2 - 1.199514E-04*Tx + 2.270616E-02,.0001d0)
case(18)
!Absolute Viscosity of Nitrate Salt, [Pa s]
Viscosity = dmax1((22.714 - 0.12 * Td + 0.0002281 * Td *Td - 0.0000001474 * Td*Td*Td) / 1000,1.e-6)
case(19)
!Absolute Viscosity of Caloria HT 43 [m2/s]
Viscosity = (0.040439268 * max(10.d0,Td)**-1.946401872) * density(19.d0, T, 0.d0)
case(20)  
!Absolute Viscosity of HITEC XL Nitrate Salt, [Pa s]
Viscosity = 1372000 * Td**-3.364
case(21)
!Absoute Viscosity of Therminol Oil [Pa s]
Viscosity = 0.001 * (10**0.8703 * dmax1(Td,20.)**(0.2877 + Log10(dmax1(Td,20.)**-0.3638)))
case(22)
!Absolute Viscosity of HITEC Salt, [Pa s]
Viscosity = dmax1(0.00622 - 0.0000102 * Td,1.e-6)
case(23)
!Absoute Viscosity of Dowtherm Q [Pa s]
Viscosity = 1 / (132.40658 + 4.36107 * Td + 0.0781417 * Td*Td - 0.00011035416 * Td*Td*Td) !Hank 10-2-03
case(24)
!Absoute Viscosity of Dowtherm RP [Pa s]
Viscosity = 1 / (4.523003 + 0.39156855 * Td + 0.028604206 * Td*Td)  !Hank 10-2-03
case(25)
!Absolute Viscosity of HITEC XL Nitrate Salt, [Pa s]
Viscosity = 1372000 * Td**-3.364
case(26)   !Argon 
Viscosity = 4.4997e-6 + 6.38920E-08*T - 1.24550E-11*T*T
case(27)  !Hydrogen
Viscosity=0.00000231 + 2.37842E-08*T - 5.73624E-12*T*T
case(28)
continue
case(29)    !Therminol 66: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
    IF(Td < 80.)THEN
        Viscosity = 1.31959963 - 0.171204729*Td + 0.0100351594*Td**2 - 0.000313556341*Td**3 + 0.0000053430666*Td**4 - 4.66597650E-08*Td**5 + 1.63046296E-10*Td**6
    ELSE
        Viscosity = 0.0490075884 - 0.00120478233*Td + 0.0000130162082*Td**2 - 7.58913847E-08*Td**3 + 2.47856063E-10*Td**4 - 4.26872345E-13*Td**5 + 3.01949160E-16*Td**6
    ENDIF
case(30)    !Therminol 59: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
    IF (Td < 25.)THEN
        Viscosity = 0.0137267822 - 0.000218740224*Td + 0.0000759248815*Td**2 - 0.00000473464744*Td**3 - 1.97083667E-07*Td**4 + 4.35487179E-09*Td**5 + 2.40243056E-10*Td**6
    ELSE
        Viscosity = 0.0114608807 - 0.000313431056*Td + 0.00000416778121*Td**2 - 3.04668508E-08*Td**3 + 1.23719006E-10*Td**4 - 2.60834697E-13*Td**5 + 2.22227675E-16*Td**6
    ENDIF
case(31:35)
continue  !no information
case(36:) !Any integer greater than 35
!Call the user-defined property table
lb=fl_bounds(fnum-35)
ub=fl_bounds(fnum-35+1)-1
if(ub.lt.lb) ub=size(fprop(1,:))
dxx(:)=fprop(1,lb:ub)
dyy(:)=fprop(4,lb:ub)
call interp(Td,size(dxx),dxx,dyy,Gjsav,Viscosity)
if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(Td,dxx(lb),dxx(ub),"User-specified fluid")
end select

continue
end function

!&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&

double precision function Conductivity(fnumd,T,P)
use global_props	
implicit none
!This function accepts as inputs temperature [K] and pressure [Pa]
!This function outputs in units of [W/m-K]
double precision::xlo,xhi, k_fluid, Td
double precision,intent(in)::T,P,fnumd
double precision,dimension(size(fprop(1,:)))::dxx,dyy !Create dummy arrays
integer::fnum,lb,ub,dum,t_warn
conductivity=1.
fnum=nint(fnumd)
Td = T - 273.15

select case(fnum)
case(1)   !    1.) Air
Conductivity = dmax1(0.00145453 + 0.0000872152*T - 2.20614E-08*T*T,1.e-4)
case(2)   !    2.) Stainless_AISI316
Conductivity = 3E-09*T*T*T - 8E-06*T*T + 0.0177*T + 7.7765
case(3)   !    3.) Water (liquid)
  continue 
case(4)   !    4.) Steam
  continue
case(5)   !    5.) CO2
  continue
case(6)   !    6.) Salt (68% KCl, 32% MgCl2)
Conductivity = 0.39
case(7)   !    7.) Salt (8% NaF, 92% NaBF4)
Conductivity = 0.5
case(8)   !    8.) Salt (25% KF, 75% KBF4)
Conductivity = 0.4
case(9)   !    9.) Salt (31% RbF, 69% RbBF4)
Conductivity = 0.28
case(10)   !    10.) Salt (46.5% LiF, 11.5%NaF, 42%KF)
Conductivity = 0.92
case(11)   !    11.) Salt (49% LiF, 29% NaF, 29% ZrF4)
Conductivity = 0.53
case(12)   !    12.) Salt (58% KF, 42% ZrF4)
Conductivity = 0.45
case(13)   !    13.) Salt (58% LiCl, 42% RbCl)
Conductivity = 0.39
case(14)   !    14.) Salt (58% NaCl, 42% MgCl2)
Conductivity = 0.43
case(15)   !    15.) Salt (59.5% LiCl, 40.5% KCl)
Conductivity = 0.43
case(16)   !    16.) Salt (59.5% NaF, 40.5% ZrF4)
Conductivity = 0.49
case(17)   !    17.) Salt (60% NaNO3, 40% KNO3)
Conductivity = -1E-11*T*T*T + 3E-08*T*T + 0.0002*T + 0.3922
case(18)
!Thermal Conductivity of Nitrate Salt, W/m/K
Conductivity = 0.443 + 0.00019 * Td
case(19)
!Conductivity of Caloria HT 43 [W/mK]
Conductivity = dmax1(-0.00014 * Td + 0.1245,.01d0)
case(20)
!Thermal Conductivity of HITEC XL Nitrate Salt, W/m/K
Conductivity = 0.519
case(21)
!Thermal conductivity of Therminol Oil [W/mK]
Conductivity = dmax1(0.1381 - 0.00008708 * Td - 0.0000001729 * Td*Td,.001d0)
case(22)
!Thermal Conductivity of HITEC Salt, W/m/K
Conductivity = 0.588 - 0.000647 * Td
case(23)
!Thermal conductivity of Dowtherm Q [W/mK]
Conductivity = dmax1(-0.0000000626555 * Td*Td - 0.000124864 * Td + 0.124379,1.e-5)    !Russ 10-2-03
case(24)
!Thermal conductivity of Dowtherm RP [W/mK]
Conductivity = -0.00012963 * Td + 0.13397                         !Russ 10-2-03
case(25)
!Thermal Conductivity of HITEC XL Nitrate Salt, W/m/K
Conductivity = 0.519
case(26)    ! Argon
Conductivity = 0.00548 + 0.0000438969*T - 6.81410E-09*T*T
case(27)   !Hydrogen
Conductivity = dmax1(0.0302888 + 0.00053634*T - 1.59604E-07*T*T,.01d0)
case(28)    !T-91 steel: "Thermo hydraulic optimisation of the EURISOL DS target" - Paul Scherrer Institut
Conductivity = -2.E-5*Td*Td + 0.017*Td + 25.535
case(29)    !Therminol 66: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
Conductivity = -2.E-7*Td*Td - 3.E-5*Td + 0.1183
case(30)    !Therminol 59: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
Conductivity = -1.E-7*Td*Td - 6.E-5*Td + 0.1227
case(31:35)
continue  !no information
case(36:) !Any integer greater than 35
!Call the user-defined property table
lb=fl_bounds(fnum-35)
ub=fl_bounds(fnum-35+1)-1
if(ub.lt.lb) ub=size(fprop(1,:))
dxx(:)=fprop(1,lb:ub)
dyy(:)=fprop(6,lb:ub)
call interp(Td,size(dxx),dxx,dyy,Gjsav,Conductivity)
if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(Td,dxx(lb),dxx(ub),"User-specified fluid")
end select


end function


!Kinematic Viscosity of Fluid [m2/s]
real(8) function kin_visc_fluid(fluidnum,T,P)
implicit none
real(8)::fluidnum,T,P, viscosity, density
    kin_visc_fluid = viscosity(fluidnum,T,P)/density(fluidnum,T,P)
end function

!Thermal diffusivity of the fluid
real(8) function diff_fluid(fluidnum, T)
implicit none
real(8) conductivity, density, specheat, T, fluidnum
    diff_fluid = Conductivity(fluidnum,T,0.d0) / (density(fluidnum, T, 0.d0) * specheat(fluidnum,T,0.d0))
end function

!Prandtl number of the fluid
real(8) function Pr_fluid(T, fluidnum)
    implicit none
    real(8):: viscosity, density, diff_fluid, T, fluidnum
    
    Pr_fluid = viscosity(fluidnum,T,0.d0) / (density(fluidnum, T, 0.d0) * diff_fluid(fluidnum,T))
end function

!Reynolds number
real(8) function Re_fluid(fluidnum, V, D, T)
implicit none
real(8):: V, D, T, fluidnum, viscosity, density
Re_fluid = density(fluidnum, T, 0.d0) * V * D / viscosity(fluidnum, T, 0.d0)
end function


!Enthalpy of Fluid [J/kg]
Double PRecision Function H_fluid(Td, fluidnum) !inputs in [K] MJW 8.26.2010
implicit none
Double Precision T, H_salt, H_caloria, H_salt_xl, H_therminol,&
                 H_salt_hitec, H_Dowtherm_Q, H_Dowtherm_RP, H_therminol_59, H_Therminol_66, H_user, Td
Integer Fluidnum
    T = Td - 273.15
    select case(Fluidnum)
    case(1:17); H_fluid = 1; !no props for this model
    case(18); H_fluid = H_salt(T); 
    case(19); H_fluid = H_caloria(T); 
    case(20); H_fluid = H_salt_xl(T); 
    case(21); H_fluid = H_therminol(T);
    case(22); H_fluid = H_salt_hitec(T);
    case(23); H_fluid = H_Dowtherm_Q(T);
    case(24); H_fluid = H_Dowtherm_RP(T);
    case(25); H_fluid = H_salt_xl(T);
    case(26:28); H_fluid = 1; !no props for this model
    case(29)    !Therminol 66: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
        H_fluid = H_Therminol_66(T)
    case(30)    !Therminol 59: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
        H_fluid = H_Therminol_59(T)
    case(31:35); H_fluid = 1; !no props for this model
    case(36:); H_fluid = H_user(T,Fluidnum-35) ;
    end select

End Function

!Temperature of Fluid [K]
Double PRecision Function T_fluid(H, fluidnum) 
implicit none
Double Precision H, H_kJ, T_user
Integer Fluidnum

    select case(Fluidnum)
    case(1:17); T_fluid = 1; !no props for this model
    case(18)
        T_fluid = -0.0000000000262 * H**2 + 0.0006923 * H + 0.03058
    case(19)
        T_fluid = 6.4394E-17 * H**3 - 0.00000000023383 * H**2 + 0.0005821 * H + 1.2744
    case(20)
        T_fluid = 0.00000000005111 * H**2 + 0.0006466 * H + 0.2151
    case(21)
        T_fluid = 7.4333E-17 * H**3 - 0.00000000024625 * H**2 + 0.00063282 * H + 12.403
    case(22)
        T_fluid = -3.309E-24 * H**2 + 0.000641 * H + 0.000000000001364
    case(23)
        T_fluid = 6.186E-17 * H**3 - 0.00000000022211 * H**2 + 0.00059998 * H + 0.77742
    case(24)
        T_fluid = 6.6607E-17 * H**3 - 0.00000000023347 * H**2 + 0.00061419 * H + 0.77419
    case(25)
        T_fluid = 0.00000000005111 * H**2 + 0.0006466 * H + 0.2151
    case(26:28); T_fluid = 1; !no props for this model
    case(29)    !Therminol 66: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
        H_kJ = H / 1000.0
        T_fluid = -0.00018*H_kJ*H_kJ + 0.521*H_kJ + 7
    case(30)    !Therminol 59: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
        H_kJ = H / 1000.0
        T_fluid = -0.000204*H_kJ*H_kJ + 0.539*H_kJ - 0.094
    case(31:35); T_fluid = 1; !no props for this model
    case(36:); T_fluid = T_user(H,Fluidnum-35) ;
    End select
    
    T_fluid = T_fluid + 273.15  !MJW 8.26.2010
    
End Function

!
! **************** Caloria HT 43 *****************************
!


!Enthalpy of Caloria HT 43 [J/kg]
Double Precision Function H_caloria(T) !T[C]
implicit none
Double Precision T
H_caloria = 1.94 * T*T + 1606.0 * T
End Function

! ********************* Nitrate Salt ***********************
!
!Enthalpy of Salt (English Units), [Btu/lbm]
Double Precision Function H_engl_salt(T) !T [F]
implicit none
Double Precision T
H_engl_salt = 0.345 * T + 0.0000114 * T*T
End Function

!Enthalpy of Salt (Metric Units), [J/kg]
Double Precision Function H_salt(T)  !T[C]
implicit none
Double Precision T
H_salt = 1443 * T + 0.086 * T*T
End Function

!!Nondimensional Penetration Distance for Nitrate Salt, z/D
!Double Precision Function pen_salt(V, Tsalt, D, Ttube) !V=salt velocity [m/s], Tsalt=temperature of salt [C], D = tube inside diameter [m], Ttube = temperature of tube [C]
!implicit none
!Double Precision V,Tsalt,D,Ttube, Pr_salt, Re_salt, diff_salt, specheat
!pen_salt = 0.23 * Pr_salt(Tsalt)**0.5 * Re_salt(V, D, Tsalt)**0.75 * (diff_salt(Tsalt) / 0.000000151)**(1 / 9) * (161000 / (1481 * (221 - Ttube)))**(1 / 3) * (1 + 0.7 * specheat(18.,Tsalt,0.d0) * (Tsalt - 221) / 161000)
!End Function

!Volumetric Thermal Expansion Coefficient for Nitrate Salt, [1/C]
Double Precision Function beta_salt(T) !T [C]
implicit none
Double Precision T, density
beta_salt = 0.636 / density(18.d0, T, 0.d0)
End Function
!
! ********************** HITEC XL ***********************
!

!Enthalpy of HITEC XL Nitrate Salt, [J/kg]
Double Precision Function H_salt_xl(T) !T [C]
implicit none
Double Precision T
H_salt_xl = 1536 * T - 0.1312 * T*T - 0.0000379667 * T*T*T
End Function

!
! ********************************* HITEC Salt **************************
!

!Enthalpy of HITEC Salt, [J/kg]
Double Precision Function H_salt_hitec(T) !T [C]
implicit none
Double Precision T
H_salt_hitec = 1560 * T
End Function

!
! ********************* Therminol VP-1 **************************
!

!Enthalpy of Therminol Oil [J/kg]
Double Precision Function H_therminol(T) !T [C]
implicit none
Double Precision T
H_therminol = 1000 * (-18.34 + 1.498 * T + 0.001377 * T*T)
End Function

!Temperature of Therminol Oil [C]
Double Precision Function T_therminol(H) !H [J/kg]
implicit none
Double Precision H
T_therminol = -0.000000000158 * H**2 + 0.0006072 * H + 13.37
End Function

!Vapor Pressure of Therminol Oil [kPa]
Double Precision Function Pvap_therminol(T) !T [C]
implicit none
Double Precision T
Pvap_therminol = 10**-11.13 * T**5.448
End Function

!
! ******************** Dowtherm Q ****************************
!

!Enthalpy of Dowtherm Q [J/kg]
Double Precision Function H_Dowtherm_Q(T) !T [C]
implicit none
Double Precision T
H_Dowtherm_Q = (0.00151461 * T*T + 1.59867 * T - 0.0250596) * 1000    !Hank 10-2-03
End Function

!Temperature of Dowtherm Q [C]
Double Precision Function T_Dowtherm_Q(H) !H [J/kg]
implicit none
Double Precision H
T_Dowtherm_Q = -0.0000000001136 * H**2 + 0.000552 * H + 4.318    !Hank 10-8-03
End Function

!Vapor Pressure of Dowtherm Q [W/mK]
Double Precision Function Pvap_Dowtherm_Q(T) !T [C]
implicit none
Double Precision T
Pvap_Dowtherm_Q = -6238.9915 + 0.000000037388014 * T**5.5254072     !Hank 10-2-03
End Function

!
!*************** Dowtherm RP **************************
!

!Enthalpy of Dowtherm RP [J/kg]
Double Precision Function H_Dowtherm_RP(T) !T [C]
implicit none
Double Precision T
H_Dowtherm_RP = (0.0014879 * T*T + 1.5609 * T - 0.0024798) * 1000      !Hank 10-2-03
End Function

!Temperature of Dowtherm RP [C]
Double Precision Function T_Dowtherm_RP(H) !H [J/kg]
implicit none
Double Precision H
T_Dowtherm_RP = -0.0000000001192 * H**2 + 0.0005647 * H + 4.343 !Hank 10-8-03
End Function

!Vapor Pressure of Dowtherm RP [Pa]
Double Precision Function Pvap_Dowtherm_RP(T) !T [C]
implicit none
Double Precision T
Pvap_Dowtherm_RP = -2863.8678 + 2.7661827E-12 * T**6.894446     !Hank 10-2-03                       !Hank 10-2-03
End Function

!
!*************** Therminol 59 **************************
!
!Therminol 59: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
!Enthalpy [J/kg]
Double Precision Function H_Therminol_59(T) !T [C]
implicit none
Double Precision T
H_Therminol_59 = 1000.*(0.0034*T*T + 1.5977*T - 0.0926)
End Function

!
!*************** Therminol 66 **************************
!
!Therminol 66: Reference: Therminol Reference Disk by Solutia: http://www.therminol.com/pages/tools/toolscd.asp
!Enthalpy [J/kg]
Double Precision Function H_Therminol_66(T) !T [C]
implicit none
Double Precision T
H_Therminol_66 = 1000.*(0.0038*T*T + 1.4363*T + 1.6142)
End Function


!************************* User Fluid ****************************
!| Note that the fluid properties are stored in the FPROP array  *
!| in the following order:                                       *
!*****************************************************************
!|  #    |   1   |   2   |   3   |   4   |   5   |   6   |   7   |
!|-------|-------|-------|-------|-------|-------|-------|-------|
!| Name  |   T   |   Cp  |  rho  |   Mu  |   Nu  |   k   |   h   |
!| Units |   C   |kJ/kg-K| kg/m3 |  Pa-s |  m2-s | W/m-K |  J/kg |
!|-------|-------|-------|-------|-------|-------|-------|-------|
!|   1   |   :   |   :   |   :   |   :   |   :   |   :   |   :   |
!|   2   |   :   |   :   |   :   |   :   |   :   |   :   |   :   |
!*****************************************************************
!Density of User Fluid, [kg/m3]
!Double Precision Function dens_user(T,fn) !T [C]
!use global_props	
!implicit none
!Double Precision:: T
!integer,intent(in)::fn
!double precision,dimension(size(fprop(1,fl_bounds(fn):(fl_bounds(fn+1)-1))))::dxx,dyy !Create dummy arrays
!integer::lb,ub,t_warn,dum
!    !Call the user-defined property table
!    lb=fl_bounds(fn)
!    ub=fl_bounds(fn+1)-1
!    if(ub.lt.lb) ub=size(fprop(1,:))
!    dxx(:)=fprop(1,lb:ub)
!    dyy(:)=fprop(3,lb:ub)
!    call interp(T,size(dxx),dxx,dyy,Gjsav,dens_user)
!    if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(T,dxx(lb),dxx(ub),"User-specified fluid")
!End Function
!
!!Heat Capacity of User Fluid, [J/kg/K]
!Double Precision Function Cp_user(T,fn) !T [C]
!use global_props	
!implicit none
!Double Precision T
!integer,intent(in)::fn
!double precision,dimension(size(fprop(1,fl_bounds(fn):(fl_bounds(fn+1)-1))))::dxx,dyy !Create dummy arrays
!integer::lb,ub,t_warn,dum
!    !Call the user-defined property table
!    lb=fl_bounds(fn)
!    ub=fl_bounds(fn+1)-1
!    if(ub.lt.lb) ub=size(fprop(1,:))
!    dxx(:)=fprop(1,lb:ub)
!    dyy(:)=fprop(2,lb:ub)
!    call interp(T,size(dxx),dxx,dyy,Gjsav,Cp_user)
!    !Convert from kJ/kg-K to J/kg-K
!    Cp_user = Cp_user*1000.
!    if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(T,dxx(lb),dxx(ub),"User-specified fluid")
!End Function

!Enthalpy of User Fluid, [J/kg]
Double Precision Function H_user(T,fn) !T [C]
use global_props	
implicit none
Double Precision T
integer,intent(in)::fn
double precision,dimension(size(fprop(1,fl_bounds(fn):(fl_bounds(fn+1)-1))))::dxx,dyy !Create dummy arrays
integer::lb,ub,t_warn,dum
    !Call the user-defined property table
    lb=fl_bounds(fn)
    ub=fl_bounds(fn+1)-1
    if(ub.lt.lb) ub=size(fprop(1,:))
    dxx(:)=fprop(1,lb:ub)
    dyy(:)=fprop(7,lb:ub)
    call interp(T,size(dxx),dxx,dyy,Gjsav,H_user)
    if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(T,dxx(lb),dxx(ub),"User-specified fluid")
End Function

!Temperature of User fluid, [C]
Double Precision Function T_user(H,fn) !H [J/kg-K]
use global_props	
implicit none
Double Precision H
integer,intent(in)::fn
double precision,dimension(size(fprop(1,fl_bounds(fn):(fl_bounds(fn+1)-1))))::dxx,dyy !Create dummy arrays
integer::lb,ub,t_warn,dum
    !Call the user-defined property table
    lb=fl_bounds(fn)
    ub=fl_bounds(fn+1)-1
    if(ub.lt.lb) ub=size(fprop(1,:))
    dxx(:)=fprop(7,lb:ub)
    dyy(:)=fprop(1,lb:ub)
    call interp(H,size(dxx),dxx,dyy,Gjsav,T_user)
    if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(H,dxx(lb),dxx(ub),"User-specified fluid")
end function
!
!!Thermal Conductivity of User Fluid, W/m/K
!Double Precision Function k_user(T,fn) !T [C]
!use global_props	
!implicit none
!Double Precision T
!integer,intent(in)::fn
!double precision,dimension(size(fprop(1,fl_bounds(fn):(fl_bounds(fn+1)-1))))::dxx,dyy !Create dummy arrays
!integer::lb,ub,t_warn,dum
!    !Call the user-defined property table
!    lb=fl_bounds(fn)
!    ub=fl_bounds(fn+1)-1
!    if(ub.lt.lb) ub=size(fprop(1,:))
!    dxx(:)=fprop(1,lb:ub)
!    dyy(:)=fprop(6,lb:ub)
!    call interp(T,size(dxx),dxx,dyy,Gjsav,k_user)
!    if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(T,dxx(lb),dxx(ub),"User-specified fluid")
!End Function
!
!!Absolute Viscosity of User Fluid, [Pa s]
!Double Precision Function visc_user(T,fn) !T [C] !Poor fit
!use global_props	
!implicit none
!Double Precision T
!integer,intent(in)::fn
!double precision,dimension(size(fprop(1,fl_bounds(fn):(fl_bounds(fn+1)-1))))::dxx,dyy !Create dummy arrays
!integer::lb,ub,t_warn,dum
!    !Call the user-defined property table
!    lb=fl_bounds(fn)
!    ub=fl_bounds(fn+1)-1
!    if(ub.lt.lb) ub=size(fprop(1,:))
!    dxx(:)=fprop(1,lb:ub)
!    dyy(:)=fprop(4,lb:ub)
!    call interp(T,size(dxx),dxx,dyy,Gjsav,visc_user)
!    if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(T,dxx(lb),dxx(ub),"User-specified fluid")
!End Function
!
!!Kinematic Viscosity of User Fluid, [m2/s]
!Double Precision Function kin_visc_user(T,fn) !T [C]
!use global_props	
!implicit none
!Double Precision T
!integer,intent(in)::fn
!double precision,dimension(size(fprop(1,fl_bounds(fn):(fl_bounds(fn+1)-1))))::dxx,dyy !Create dummy arrays
!integer::lb,ub,t_warn,dum
!    !Call the user-defined property table
!    lb=fl_bounds(fn)
!    ub=fl_bounds(fn+1)-1
!    if(ub.lt.lb) ub=size(fprop(1,:))
!    dxx(:)=fprop(1,lb:ub)
!    dyy(:)=fprop(5,lb:ub)
!    call interp(T,size(dxx),dxx,dyy,Gjsav,kin_visc_user)
!    if((Gjsav.eq.ub).or.(Gjsav.eq.lb)) dum=t_warn(T,dxx(lb),dxx(ub),"User-specified fluid")
!End Function
!************************************