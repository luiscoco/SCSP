c-----------------------------------------------------------------------
c  Begin file CONSTS.INC
c-----------------------------------------------------------------------

      implicit double precision (a-h,o-z)
      implicit integer (i-k,m,n)
      implicit logical (l)

      parameter (ncmax=20)                         !Maximum number of components in the mixture
      parameter (mxecs=ncmax)                      !Maximum number of reference fluids for pure fluid ECS
      parameter (mxref=1)                          !Maximum number of reference fluids for mixture ECS
      parameter (mxtrm=56)                         !Maximum number of Helmholtz terms
      parameter (mxcrt=20)                         !Maximum number of critical region terms in FEQ

c..Use for fitting fluids:
c     parameter (ncmax=2)                          !Maximum number of components in the mixture
c     parameter (mxecs=0)                          !Maximum number of reference fluids for pure fluid ECS
c     parameter (mxref=0)                          !Maximum number of reference fluids for mixture ECS
c     parameter (mxtrm=20)                         !Maximum number of Helmholtz terms
c     parameter (mxcrt=15)                         !Maximum number of critical region terms in FEQ

c..Parameters common to all routines
      parameter (ipropmax=200)                     !Number of output properties available in ALLPROPS.
      parameter (nx=ncmax)                         !Number of slots to store all the components in a mixture.
      parameter (ns=ncmax+mxecs)                   !Number of slots to store the components and their ECS files.
      parameter (nq=ncmax+mxecs+mxref)             !Number of slots to store all possible fluid files needed.
      parameter (nr=ncmax+mxecs+1)                 !Location of the reference slot for the mixture transport properties.
                                                   !  (which is the same as nq when only one reference fluid is used.)
c..Constants
      parameter (thrd   =1d0/3d0)                  !=1/3
      parameter (thrd2  =2d0/3d0)                  !=2/3
      parameter (xPi    =3.14159265358979323846d0) !Pi (all digits are required for PR calculations)
      parameter (xRgas  =8.3144598d0)              !Current value of the ideal-gas constant (J/mol-K)
      parameter (xAvog  =6.022140857d23)           !Avogadro's number (1/mol)
      parameter (xPlanck=6.62607004d-34)           !Planck constant (J/s)
      parameter (xBoltz =xRgas/xAvog)              !Boltzmann's constant (=1.38064852d-23 J/K)
      parameter (xPerm  =8.854187817d-12)          !Permittivity of free space (F.m-1)
      parameter (xLOG2  =0.6931471805599453d0)     !LN(2)
      parameter (xLOG2x =1d0/xLOG2)                !1/LN(2)
      parameter (xLOG10 =2.3025850929940458d0)     !LN(10)
      parameter (DmaxWater=55.504316178d0)         !Maximum possible density for liquid water (mol/l)
      parameter (DmaxD2O  =55.215501986d0)         !Maximum possible density for liquid heavy water (mol/l)  (occurs at 284.978415 K)

c..Mixture constants
      parameter (nBNCmax=1500)
      parameter (nbin=(nx*(nx-1)/2))               !Number of possible binary pairs
      parameter (nmxpar=6)                         !Number of binary mixture parameters that can be placed on a line (after the KW0, etc.) in the hmx.bnc file
      parameter (nmxtrm=20)                        !Number of terms allowed for any binary mixing function
      parameter (nmxcof=12)                        !Number of coefficients and exponents that are allowed for any term in the mixing function.
      parameter (nmsav=55)                         !Number of mixture models allowed (KW0-KW9, KWA-KWZ, etc.)
      parameter (nbrule=40)                        !Number of binary mixing rules
      parameter (nmxtc=11,nmxvc=11)                !Number of coefficients for critical line estimation equations.
      parameter (narm=600,nx5=nx+5)                !Maximum number of points allowed in the splines.

      parameter (ncppmx=20,nph0mx=20)              !Maximum number of Cp0 and PHI0 terms
      parameter (nfhe=4)                           !Number of ECS parameters
      parameter (nmxprb=6)                         !Number of coefficients for PR EOS

c..Ancillary constants
      parameter (nanc=8)                           !Maximum number of terms in ancillary equations
      parameter (ndecf=15)                         !Maximum number of terms in dielectric constant equation
      parameter (nsigk=5)                          !Maximum number of terms in surface tension equation
      parameter (nmxstb=6)                         !Maximum number of coefficients for surface tension mixture model

c..Transport constants
      parameter (mxeta=100)                        !Maximum number of coefficients for viscosity
      parameter (mxetac=10)                        !Maximum number of additional parameters for chung
      parameter (mxtck=40)                         !Maximum number of coefficients for t.c. crit
      parameter (mxtcx=40)                         !Maximum number of coefficients for thermal cond
      parameter (mxtcxc=10)                        !Maximum number of additional parameters for chung
      parameter (metar=6)                          !Maximum additional residual viscosity parameters (chung)
      parameter (mtcxr=6)                          !Maximum additional residual tcx parameters for chung
      parameter (mxtrn=10)                         !Maximum number of coefficients for psi, chi function
      parameter (mxomg=15)                         !Maximum number of coefficients for collision integral

c..Error numbers
      parameter (xflg =-9999950d0)                 !Code for outside use, such as with Excel
      parameter (xinf =-9999960d0)                 !Error number to indicate 'infinite'
      parameter (xerr =-9999970d0)                 !Error number to indicate 'error'
      parameter (xnotd=-9999980d0)                 !Error number to indicate 'not defined'
      parameter (xnotc=-9999990d0)                 !Error number to indicate 'not calculated'

c..Letter enumerations
      parameter (iLetterA            =  1)
      parameter (iLetterB            =  2)
      parameter (iLetterC            =  3)
      parameter (iLetterD            =  4)
      parameter (iLetterE            =  5)
      parameter (iLetterF            =  6)
      parameter (iLetterG            =  7)
      parameter (iLetterH            =  8)
      parameter (iLetterI            =  9)
      parameter (iLetterJ            = 10)
      parameter (iLetterK            = 11)
      parameter (iLetterL            = 12)
      parameter (iLetterM            = 13)
      parameter (iLetterN            = 14)
      parameter (iLetterO            = 15)
      parameter (iLetterP            = 16)
      parameter (iLetterQ            = 17)
      parameter (iLetterR            = 18)
      parameter (iLetterS            = 19)
      parameter (iLetterT            = 20)
      parameter (iLetterU            = 21)
      parameter (iLetterV            = 22)
      parameter (iLetterW            = 23)
      parameter (iLetterX            = 24)
      parameter (iLetterY            = 25)
      parameter (iLetterZ            = 26)

c..Conversion factors:
      double precision KGFtoN,INtoM,LBMtoKG,MMHGtoMPA,INH2OtoMPA
      double precision LBFtoN,IN3toM3
      parameter (CtoK=273.15d0)             !Exact conversion
      parameter (FtoR=459.67d0)             !Exact conversion
      parameter (RtoK=5d0/9d0)              !Exact conversion
      parameter (HtoS=3600d0)               !Exact conversion
      parameter (ATMtoMPa=0.101325d0)       !Exact conversion
      parameter (BARtoMPA=0.1d0)            !Exact conversion
      parameter (KGFtoN=98.0665d-1)         !Exact conversion
      parameter (INtoM=0.0254d0)            !Exact conversion
      parameter (FTtoM=12d0*INtoM)          !Exact conversion
      parameter (LBMtoKG=0.45359237d0)      !Exact conversion
      parameter (MMHGtoMPA=ATMtoMPa/760d0)  !Exact conversion
      parameter (INH2OtoMPA=0.000249082d0)
c  The CALtoJ conversion factor is set in the SETUP.FOR file and can be
c   changed in the FLAGS subroutine.
c     parameter (CALtoJ=4.184d0)            !Exact conversion (tc)
c     parameter (CALtoJ=4.1868d0)           !Exact conversion (IT)

c     parameter (BTUtoKJ=CALtoJ*LBMtoKG*RtoK)
      parameter (LBFtoN=LBMtoKG*KGFtoN)
      parameter (IN3toM3=INtoM*INtoM*INtoM)
      parameter (FT3toM3=FTtoM*FTtoM*FTtoM)
      parameter (GALLONtoM3=IN3toM3*231d0)
      parameter (PSIAtoMPA=LBMtoKG/INtoM/INtoM*KGFtoN/1000000d0)
      parameter (FTLBFtoJ=FTtoM*LBFtoN)
      parameter (HPtoW=550d0*FTLBFtoJ)

      parameter (iUTypeT = 1)    !Temperature      !The first 8 are set to match the values of iprpT,
      parameter (iUTypeP = 2)    !Pressure         ! iprpP, iprpD, iprpV, iprpE, iprpH, iprpS, and iprpQ.
      parameter (iUTypeD = 3)    !Density
      parameter (iUTypeV = 4)    !Specific volume
      parameter (iUTypeE = 5)    !Energy (but not used in most places)
      parameter (iUTypeH = 6)    !Energy or enthalpy
      parameter (iUTypeS = 7)    !Entropy or heat capacity
      parameter (iUTypeQ = 8)    !Quality
      parameter (iUTypeW = 9)    !Speed of sound
      parameter (iUTypeU = 10)   !Viscosity
      parameter (iUTypeK = 11)   !Thermal conductivity
      parameter (iUTypeN = 12)   !Surface tension
      parameter (iUType0 = 13)   !No units (e.g., Z)
      parameter (iUTypeM = 14)   !Molar mass
      parameter (iUTypeI = 17)   !Kinematic viscosity
      parameter (iUTypeF = 27)   !Mass flux
      parameter (iUTypeG = 37)   !Heating value
      parameter (iUTypeB = 38)   !Dipole moment

      parameter (i0unit0             =  701)
      parameter (iMunitG_MOL         =  751)   !  g/mol
      parameter (iMunitKG_MOL        =  752)   !  kg/mol
      parameter (iMunitLBM_LBMOL     =  753)   !  lbm/lbmol
      parameter (iQunitMole          =  801)   !  mole
      parameter (iQunitMass          =  802)   !  mass
      parameter (iBunitDEBYE         =  841)   !  debye
      parameter (iFunitKG_M2_S       =  861)   !  kg/(m^2-s)
      parameter (iGunitMJ_M3         =  881)   !  MJ/m^3
      parameter (iGunitBTU_FT3       =  882)   !  BTU/ft^3

c..Unit conversion indices
  !Temperature
      parameter (iTunitK             =  101)   !  K
      parameter (iTunitC             =  102)   !  C
      parameter (iTunitR             =  103)   !  R
      parameter (iTunitF             =  104)   !  F

  !Pressure
      parameter (iPunitPA            =  151)   !  Pa
      parameter (iPunitKPA           =  152)   !  kPa
      parameter (iPunitMPA           =  153)   !  MPa
      parameter (iPunitGPA           =  154)   !  GPa
      parameter (iPunitBAR           =  155)   !  bar
      parameter (iPunitKBAR          =  156)   !  kbar
      parameter (iPunitATM           =  157)   !  atm
      parameter (iPunitPSIA          =  158)   !  psia
      parameter (iPunitPSF           =  159)   !  psf
      parameter (iPunitMMHG          =  160)   !  mmHg
      parameter (iPunitINHG          =  161)   !  inHg
      parameter (iPunitPSIG          =  162)   !  psig

  !Density
      parameter (iDunitMOL_DM3       =  201)   !  mol/dm^3
      parameter (iDunitMOL_CM3       =  202)   !  mol/cm^3
      parameter (iDunitMOL_M3        =  203)   !  mol/m^3
      parameter (iDunitKG_M3         =  204)   !  kg/m^3
      parameter (iDunitKG_DM3        =  205)   !  kg/dm^3
      parameter (iDunitG_CM3         =  206)   !  g/cm^3
      parameter (iDunitG_DM3         =  207)   !  g/dm^3
      parameter (iDunitLBM_FT3       =  208)   !  lbm/ft^3
      parameter (iDunitLBMOL_FT3     =  209)   !  lbmol/ft^3
      parameter (iDunitLBMOL_GAL     =  210)   !  lbmol/gal
      parameter (iDunitLBM_GAL       =  211)   !  lbm/gal

  !Specific Volume
  !These should not be changed unless the corresponding
  !density values are changed (e.g., kg/dm^3<-->dm^3/kg)
  !                           (          205<-->255    )
      parameter (iVunitDM3_MOL       =  251)   !  dm^3/mol
      parameter (iVunitCM3_MOL       =  252)   !  cm^3/mol
      parameter (iVunitM3_MOL        =  253)   !  m^3/mol
      parameter (iVunitM3_KG         =  254)   !  m^3/kg
      parameter (iVunitDM3_KG        =  255)   !  dm^3/kg
      parameter (iVunitCM3_G         =  256)   !  cm^3/g
      parameter (iVunitDM3_G         =  257)   !  dm^3/g
      parameter (iVunitFT3_LBM       =  258)   !  ft^3/lbm
      parameter (iVunitFT3_LBMOL     =  259)   !  ft^3/lbmol
      parameter (iVunitGAL_LBMOL     =  260)   !  gal/lbmol
      parameter (iVunitGAL_LBM       =  261)   !  gal/lbm

  !Enthalpy or energy
      parameter (iHunitJ_MOL         =  301)   !  J/mol
      parameter (iHunitKJ_MOL        =  302)   !  kJ/mol
      parameter (iHunitMJ_MOL        =  303)   !  MJ/mol
      parameter (iHunitJ_G           =  304)   !  J/g        or  kJ/kg
      parameter (iHunitJ_KG          =  305)   !  J/kg
      parameter (iHunitCAL_MOL       =  306)   !  cal/mol
      parameter (iHunitCAL_G         =  307)   !  cal/g
      parameter (iHunitBTU_LBM       =  308)   !  Btu/lbm
      parameter (iHunitBTU_LBMOL     =  309)   !  Btu/lbmol

  !Entropy or heat capacity
      parameter (iSunitJ_MOL_K       =  351)   !  J/(mol-K)
      parameter (iSunitKJ_MOL_K      =  352)   !  kJ/(mol-K)
      parameter (iSunitJ_G_K         =  353)   !  J/(g-K)     or  kJ/(kg-K)
      parameter (iSunitJ_KG_K        =  354)   !  J/(kg-K)
      parameter (iSunitCAL_MOL_K     =  355)   !  cal/(mol-K)
      parameter (iSunitCAL_G_K       =  356)   !  cal/(g-K)   or  kcal/(kg-K)
      parameter (iSunitBTU_LBM_R     =  357)   !  Btu/(lbm-R)
      parameter (iSunitBTU_LBMOL_R   =  358)   !  Btu/(lbmol-R)
      parameter (iSunitFT_LBF_LBMOL_R=  359)   !  ft/(lbf-lbmol-R)
      parameter (iSunitCP_R          =  360)   !  Cp/R

  !Speed of sound
      parameter (iWunitM_S           =  401)   !  m/s
      parameter (iWunitCM_S          =  402)   !  cm/s
      parameter (iWunitKM_H          =  403)   !  km/h
      parameter (iWunitFT_S          =  404)   !  ft/s
      parameter (iWunitIN_S          =  405)   !  in/s

  !Kinematic viscosity
      parameter (iKVunitM2_S         =  451)   !  m^2/s
      parameter (iKVunitCM2_S        =  452)   !  cm^2/s
      parameter (iKVunitFT2_S        =  453)   !  ft^2/s

  !Viscosity
      parameter (iVISunitPA_S        =  501)   !  Pa/s
      parameter (iVISunitMPA_S       =  502)   !  mPa/s
      parameter (iVISunitUPA_S       =  503)   !  uPa/s
      parameter (iVISunitG_CM_S      =  504)   !  g/(cm-s)
      parameter (iVISunitCPOISE      =  505)   !  cpoise
      parameter (iVISunitMPOISE      =  506)   !  mpoise
      parameter (iVISunitUPOISE      =  507)   !  upoise
      parameter (iVISunitLBM_FT_S    =  508)   !  lbm/(ft-s)
      parameter (iVISunitLBM_FT_H    =  509)   !  lbm/(ft-h)
      parameter (iVISunitLBF_FT2_S   =  510)   !  lbf/(ft^2-s)

  !Thermal conductivity
      parameter (iTCXunitMW_M_K      =  551)   !  mW/(m-K)
      parameter (iTCXunitW_M_K       =  552)   !  W/(m-K)
      parameter (iTCXunitG_CM_S3_K   =  553)   !  g/(cm-s^3-K)
      parameter (iTCXunitKG_M_S3_K   =  554)   !  kg/(m-s^3-K)
      parameter (iTCXunitCAL_S_CM_K  =  555)   !  cal/(s-cm-K)
      parameter (iTCXunitKCAL_H_M_K  =  556)   !  kcal/(h-m-K)
      parameter (iTCXunitLBM_FT_S3_F =  557)   !  lbm/(ft-s^3-F)
      parameter (iTCXunitLBF_S_F     =  558)   !  lbf/(s-F)
      parameter (iTCXunitBTU_H_FT_F  =  559)   !  Btu/(h-ft-F)  [Same as Btu/(h-ft-R)]

  !Surface tension
      parameter (iSTNunitN_M         =  601)   !  N/m
      parameter (iSTNunitMN_M        =  602)   !  mN/m
      parameter (iSTNunitDYNE_CM     =  603)   !  dyne/cm
      parameter (iSTNunitLBF_FT      =  604)   !  lbf/ft

c..EOS enumerations
      parameter (iEOSHMX             = 10)
      parameter (iEOSFEQ             = 20)
      parameter (iEOSBWR             = 30)
      parameter (iEOSAGA             = 40)
      parameter (iEOSPR              = 50)
      parameter (iEOSPR3             = 53)
      parameter (iEOSECS             = 60)
      parameter (iEOSNBS             = 70)

c..Enumerations for the eleven unit sets available.
      parameter (iUnitsDefault     =  0)
      parameter (iUnitsMolSI       =  1)
      parameter (iUnitsSI          =  2)
      parameter (iUnitsC           =  3)
      parameter (iUnitsE           =  5)
      parameter (iUnitsMolE        =  6)
      parameter (iUnitsMKS         =  7)
      parameter (iUnitsCGS         =  8)
      parameter (iUnitsMix         =  9)
      parameter (iUnitsMechE       = 10)
      parameter (iUnitsUser        = 11)
      parameter (iUnitsUser2       = 12)
      parameter (iUnitsBaseSImolar = 20)  !Molar, base SI
      parameter (iUnitsBaseSImass  = 21)  !Mass, base SI


c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                    End file CONSTS.INC
c ======================================================================
