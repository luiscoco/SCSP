c-----------------------------------------------------------------------
c  Begin file COMREFP.INC
c-----------------------------------------------------------------------

c..RPN constants
      parameter (maxop=250)                        !Maximum number of operations
      parameter (maxopst=15)                       !Maximum number of operation sets
      parameter (maxoptrm=300)                     !Maximum number of coefficients

c..Variables for mathematical operators.
      character*15 h7oper
      common /RPNCF/  cfoper(maxoptrm,12,nq),i7oper(maxop,maxopst,nq),
     &                icfoper(maxoptrm,nq),n7cf(maxop,maxopst,nq),
     &                n7oper(maxopst,nq),n7st(maxopst,nq),n7total,
     &                h7oper(maxop)


c..Units and variables in the REFPROP routine
      parameter (iUTMax=40)                        !Maximum number of unit types
      parameter (iUUMax=40)                        !Maximum number of units in each type
      parameter (ihUMax=1000)                      !Maximum number of units

      character hUUnit*20,hFldOld*10000
      common /IUNITS/ CALtoJ,BTUtoKJ,
     &                iUUnits(iUTMax,0:iUUMax),iUnitTag(0:500),
     &                iFldmix,iPass,iRefExit,hUUnit(0:ihUMax),hFldOld



c..Property indices for the ALLPROPS routine
      parameter (iprpT               =  1)  !The first 8 are set to match the values of iUTypeT,
      parameter (iprpP               =  2)  ! iUTypeP, iUTypeD, iUTypeV, iUTypeH, iUTypeS, and iUTypeQ.
      parameter (iprpD               =  3)
      parameter (iprpV               =  4)
      parameter (iprpE               =  5)
      parameter (iprpH               =  6)
      parameter (iprpS               =  7)
      parameter (iprpQ               =  8)
      parameter (iprpQMOLE           =  9)
      parameter (iprpQMASS           = 10)
      parameter (iprpCV              = 11)
      parameter (iprpCP              = 12)
      parameter (iprpCPCV            = 13)
      parameter (iprpW               = 14)
      parameter (iprpZ               = 15)
      parameter (iprpA               = 16)
      parameter (iprpG               = 17)
      parameter (iprpR               = 18)
      parameter (iprpM               = 19)
      parameter (iprpJT              = 20)
      parameter (iprpNCOMP           = 21)  !The parameters on the indented lines are simply used to divide the sections and group like
         parameter (iBlockReg        = 40)  ! parameters together for increased calculation speed.  They do not represent any property.
                                            ! The value of these parameters can be increased if needed, but must always be larger than
      parameter (iprpKAPPA           = 41)  ! the ones before it (and smaller than the ones after it).
      parameter (iprpBETA            = 42)
      parameter (iprpISENK           = 43)
      parameter (iprpKT              = 44)
      parameter (iprpBETAS           = 45)
      parameter (iprpBS              = 46)
      parameter (iprpKKT             = 47)
      parameter (iprpTHROTT          = 48)
      parameter (iprpSPHT            = 49)
         parameter (iBlockIrreg      = 60)

      parameter (iprpDPDD            = 61)
      parameter (iprpDPDT            = 62)
      parameter (iprpDDDP            = 63)
      parameter (iprpDDDT            = 64)
      parameter (iprpDTDP            = 65)
      parameter (iprpDTDD            = 66)
      parameter (iprpD2PDD2          = 67)
      parameter (iprpD2PDT2          = 68)
      parameter (iprpD2PDTD          = 69)
      parameter (iprpD2DDP2          = 70)
      parameter (iprpD2DDT2          = 71)
      parameter (iprpD2DDPT          = 72)
      parameter (iprpD2TDP2          = 73)
      parameter (iprpD2TDD2          = 74)
      parameter (iprpD2TDPD          = 75)
         parameter (iBlockDeriv      = 90)

      parameter (iprpDHDT_D          = 91)
      parameter (iprpDHDT_P          = 92)
      parameter (iprpDHDD_T          = 93)
      parameter (iprpDHDD_P          = 94)
      parameter (iprpDHDP_T          = 95)
      parameter (iprpDHDP_D          = 96)
         parameter (iBlockDHDx       = 100)

      parameter (iprpDSDT_D          = 101)
      parameter (iprpDSDT_P          = 102)
      parameter (iprpDSDD_T          = 103)
      parameter (iprpDSDD_P          = 104)
      parameter (iprpDSDP_T          = 105)
      parameter (iprpDSDP_D          = 106)
         parameter (iBlockDSDx       = 120)

      parameter (iprpBvir            = 121)
      parameter (iprpCvir            = 122)
      parameter (iprpDvir            = 123)
      parameter (iprpEvir            = 124)
      parameter (iprpdBvirdT         = 125)
      parameter (iprpd2BvirdT2       = 126)
      parameter (iprpdCvirdT         = 127)
      parameter (iprpd2CvirdT2       = 128)
      parameter (iprpdDvirdT         = 129)
      parameter (iprpd2DvirdT2       = 130)
      parameter (iprpBA              = 131)
      parameter (iprpCA              = 132)
         parameter (iBlockB          = 150)

      parameter (iprpVE              = 151)
      parameter (iprpEE              = 152)
      parameter (iprpHE              = 153)
      parameter (iprpSE              = 154)
      parameter (iprpAE              = 155)
      parameter (iprpGE              = 156)
         parameter (iBlockE          = 160)

      parameter (iprpP0              = 161)
      parameter (iprpD0              = 162)
      parameter (iprpE0              = 163)
      parameter (iprpH0              = 164)
      parameter (iprpS0              = 165)
      parameter (iprpCV0             = 166)
      parameter (iprpCP0             = 167)
      parameter (iprpCP0CV0          = 168)
      parameter (iprpW0              = 169)
      parameter (iprpA0              = 170)
      parameter (iprpG0              = 171)
      parameter (iprpP_P0            = 172)
         parameter (iBlock0          = 180)

      parameter (iprpPR              = 181)
      parameter (iprpER              = 182)
      parameter (iprpHR              = 183)
      parameter (iprpSR              = 184)
      parameter (iprpCVR             = 185)
      parameter (iprpCPR             = 186)
      parameter (iprpAR              = 187)
      parameter (iprpGR              = 188)
         parameter (iBlockR          = 200)

      parameter (iprpPHIG00          = 201)
      parameter (iprpPHIG10          = 202)
      parameter (iprpPHIG20          = 203)
      parameter (iprpPHIG30          = 204)
      parameter (iprpPHIG01          = 205)
      parameter (iprpPHIG02          = 206)
      parameter (iprpPHIG03          = 207)
      parameter (iprpPHIG11          = 208)
      parameter (iprpPHIG12          = 209)
      parameter (iprpPHIG21          = 210)
         parameter (iBlockPHI0       = 220)

      parameter (iprpPHIR00          = 221)
      parameter (iprpPHIR10          = 222)
      parameter (iprpPHIR20          = 223)
      parameter (iprpPHIR30          = 224)
      parameter (iprpPHIR01          = 225)
      parameter (iprpPHIR02          = 226)
      parameter (iprpPHIR03          = 227)
      parameter (iprpPHIR11          = 228)
      parameter (iprpPHIR12          = 229)
      parameter (iprpPHIR13          = 230)
      parameter (iprpPHIR21          = 231)
      parameter (iprpPHIR22          = 232)
      parameter (iprpPHIR23          = 233)
      parameter (iprpPHIR31          = 234)
      parameter (iprpPHIR32          = 235)
      parameter (iprpPHIR33          = 236)
         parameter (iBlockPHIR       = 240)

      parameter (iprpTC              = 241)
      parameter (iprpPC              = 242)  !The pressure must remain between T and D for all of these.
      parameter (iprpDC              = 243)
      parameter (iprpTMAXT           = 244)
      parameter (iprpPMAXT           = 245)
      parameter (iprpDMAXT           = 246)
      parameter (iprpTMAXP           = 247)
      parameter (iprpPMAXP           = 248)
      parameter (iprpDMAXP           = 249)
      parameter (iprpTCEST           = 250)
      parameter (iprpPCEST           = 251)
      parameter (iprpDCEST           = 252)
      parameter (iprpTRED            = 253)  !These four must stay together in this order.
      parameter (iprpDRED            = 254)
      parameter (iprpTAU             = 255)
      parameter (iprpDEL             = 256)
         parameter (iBlockCRT        = 270)

      parameter (iprpTMIN            = 271)
      parameter (iprpTMAX            = 272)
      parameter (iprpDMAX            = 273)
      parameter (iprpPMAX            = 274)
         parameter (iBlockLIM        = 280)

      parameter (iprpVIS             = 281)
      parameter (iprpTCX             = 282)
      parameter (iprpPRANDTL         = 283)
      parameter (iprpTD              = 284)
      parameter (iprpKV              = 285)
      parameter (iprpTRNTJ           = 286)
      parameter (iprpTRNDJ           = 287)
         parameter (iBlockTRN        = 300)

      parameter (iprpANC_TP          = 301)
      parameter (iprpANC_TDL         = 302)
      parameter (iprpANC_TDV         = 303)
      parameter (iprpANC_PT          = 304)
      parameter (iprpANC_DT          = 305)
      parameter (iprpMELT_TP         = 306)
      parameter (iprpMELT_PT         = 307)
      parameter (iprpSUBL_TP         = 308)
      parameter (iprpSUBL_PT         = 309)
         parameter (iBlockANC        = 320)

      parameter (iprpGRUN            = 321)
      parameter (iprpPIP             = 322)
      parameter (iprpZ1_D            = 323)
      parameter (iprpZ1_P            = 324)
      parameter (iprpPV              = 325)
      parameter (iprpSD              = 326)
      parameter (iprp_1T             = 327)
      parameter (iprpRIEM            = 328)
      parameter (iprpRD              = 329)
         parameter (iBlockEOS        = 340)

      parameter (iprpCSAT            = 341)
      parameter (iprpCV2P            = 342)
      parameter (iprpDPDTSAT         = 343)
      parameter (iprpDHDZSAT         = 344)
      parameter (iprpLIQSPNDL        = 345)
      parameter (iprpVAPSPNDL        = 346)
         parameter (iBlockCSAT       = 350)

      parameter (iprpHFRM            = 351)
      parameter (iprpHG              = 352)
      parameter (iprpHN              = 353)
         parameter (iBlockHEAT       = 360)

      parameter (iprpHEATVAPZ        = 361)
      parameter (iprpHEATVAPZ_T      = 362)
      parameter (iprpHEATVAPZ_P      = 363)
      parameter (iprpHGLQ            = 364)
      parameter (iprpHNLQ            = 365)
      parameter (iprpHGVOL           = 366)
      parameter (iprpHNVOL           = 367)
         parameter (iBlockHEATV      = 380)

      parameter (iprpB12             = 381)
      parameter (iprpSTN             = 382)
      parameter (iprpDE              = 383)
      parameter (iprpPINT            = 384)
      parameter (iprpPREP            = 385)
      parameter (iprpPATT            = 386)
      parameter (iprpEXERGY          = 387)
      parameter (iprpCEXERGY         = 388)
      parameter (iprpCSTAR           = 389)
      parameter (iprpTMF             = 390)
      parameter (iprpFPV             = 391)
      parameter (iprpSUMFACT         = 392)
      parameter (iprpRDAIR           = 393)
      parameter (iprpRDH2O           = 394)
      parameter (iprpAPI             = 395)
         parameter (iBlockOTHER      = 410)

      parameter (iprpISETUP          = 411)  !This section is used for passing variables that are being used
      parameter (iprpISETREF         = 412)  ! in the program that are not typically considered of interest.
         parameter (iBlockVAR        = 420)

      parameter (iprpTCRIT           = 421)
      parameter (iprpPCRIT           = 422)
      parameter (iprpDCRIT           = 423)
      parameter (iprpTCTRUE          = 424)
      parameter (iprpDCTRUE          = 425)
      parameter (iprpTTRP            = 426)
      parameter (iprpPTRP            = 427)
      parameter (iprpDTRP            = 428)
      parameter (iprpTNBP            = 429)
      parameter (iprpREOS            = 430)
      parameter (iprpMM              = 431)
      parameter (iprpACF             = 432)
      parameter (iprpTREF            = 433)
      parameter (iprpDREF            = 434)
      parameter (iprpHREF            = 435)
      parameter (iprpSREF            = 436)
      parameter (iprpDIPOLE          = 437)
      parameter (iprpHEATCOMB        = 438)
      parameter (iprpGWP             = 439)
      parameter (iprpODP             = 440)
      parameter (iprpETA0            = 441)  !Keep these together because
      parameter (iprpETAB2           = 442)  !they are checked as a range
      parameter (iprpETAR            = 443)  !in the ALLPROP0 routine,
      parameter (iprpETAC            = 444)  !with ETA0 first and TCXC last.
      parameter (iprpTCX0            = 445)  !
      parameter (iprpTCXR            = 446)  !
      parameter (iprpTCXC            = 447)  !
      parameter (iprpK               = 448)
      parameter (iprpF               = 449)
      parameter (iprpFC              = 450)
      parameter (iprpCPOT            = 451)
      parameter (iprpDADN            = 452)
      parameter (iprpDNADN           = 453)
      parameter (iprpX               = 454)
      parameter (iprpXMOLE           = 455)
      parameter (iprpXMASS           = 456)
         parameter (iBlockCOMP       = 470)

      parameter (iprpCAS             = 471)
      parameter (iprpHASH            = 472)
      parameter (iprpNAME            = 473)
      parameter (iprpFDIR            = 474)
      parameter (iprpPHASE           = 475)
      parameter (iprpALTID           = 476)
      parameter (iprpINCHI           = 477)
      parameter (iprpFAMILY          = 478)
      parameter (iprpSAFETY          = 479)
      parameter (iprpFIJMIX          = 480)
      parameter (iprpFLDNAME         = 481)
      parameter (iprpSYNONYM         = 482)
      parameter (iprpCHEMFORM        = 483)
      parameter (iprpINCHIKEY        = 484)
      parameter (iprpLONGNAME        = 485)
      parameter (iprpUNNUMBER        = 486)
      parameter (iprpREFSTATE        = 487)
      parameter (iprpFULLCHEMFORM    = 488)
      parameter (iprpVAPFLUIDSTRING  = 489)
      parameter (iprpLIQFLUIDSTRING  = 490)
      parameter (iprpFlags=8) !Number of flags available below
      parameter (iprp_EOS            = 491)
      parameter (iprp_VIS            = 492)
      parameter (iprp_TCX            = 493)
      parameter (iprp_ECS            = 494)
      parameter (iprp_STN            = 495)
      parameter (iprp_DIE            = 496)
      parameter (iprp_MLT            = 497)
      parameter (iprp_SBL            = 498)
c                                    = xxx)  !Do not use values between 491 and 510 (or rather between 490+iprpFlags and 490+###).
         parameter (iBlockSTRINGS    = 510)

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                   End file COMREFP.INC
c ======================================================================
