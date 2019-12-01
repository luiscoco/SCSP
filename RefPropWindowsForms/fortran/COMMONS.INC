c-----------------------------------------------------------------------
c  Begin file COMMONS.INC
c-----------------------------------------------------------------------

c     implicit double precision (a-h,o-z)
c     implicit integer (i-k,m,n)
c     implicit logical (l)

      include 'CONSTS.INC'

      character*255 hname,hnam80,hchemf1,hchemf2,hsyn,hfamily,
     &              hUNNumb,hinpnme,hpath,hieos,hicp0,hDOI,hWEB,
     &              hieta,hietac,hitcx,hitcxc,histn,hidiel,
     &              himelt,hisubl,hips,hipl,hidl,hidv,hdir,
     &              hsvfld,hsvmix,hasav,hbsav,hersav2,hPDmix,
     &              hrefdir,hFldFileName,hSafety,
     &              hInChi,hInChiKey,hAltFluidName,hAltFluidName2
      character*3   heosk,hcpk,hetak,htcxk,hstnk,hdilk,hmeltk,hsublk,
     &              hpsk,hplk,hdlk,hdvk,hsveosk,hsvvisk,hsvtcxk,hsvstnk,
     &              heosmx,hvismx,htcxmx,hstnmx,hdilmx,heox,
     &              hsveos,hsvvis,hsvtcx,hsvstn,hsvref,hRef3,
     &              hsavprk,hsavpr,hrefdef
      character     hcas*12,hAltID*15,hash*10

c  The first three lines below are variables storing the information
c    at the top of the fluid file.
c  Variables starting with "hi" store the citation of the equation.
c  Variables ending in a "k" indicate pure fluid information,
c    whereas those ending with "mx" are a mixture indicator.
c  Varibles that start with "hsv" contain the 3 character strings indicating
c    which equation or mixture model was read in on the last call.
c  Those without the "hsv" are the models to be read in.

      common /STRNGS/ hname(0:nq),hnam80(0:nq),hcas(0:nq),hAltID(nq), !Fluid file header
     &  hChemF1(nq),hChemF2(nq),hsyn(nq),hrefdef(nq),hfamily(nq),
     &  hash(nq),hUNNumb(nq),hinpnme(nq),hpath,
     &  hInChi(nq),hInChiKey(nq),hDOI(nq,10),hWEB(nq,10),
     &  hieos(nx),hicp0(nx),hieta(nq),hietac(nx),                    !Citations
     &  hitcx(nq),hitcxc(nq),histn(nx),hidiel(nx),
     &  himelt(nx),hisubl(nx),hips(nx),hipl(nx),
     &  hidl(nx),hidv(nx),hdir(nq),
     &  heosk(nq),hcpk(nq),hetak(nq),htcxk(nq),                      !Pure fluid equations to read in.
     &  hstnk(nx),hdilk(nx),hmeltk(0:nx),hsublk(0:nx),
     &  hpsk(0:nx),hplk(0:nx),hdlk(0:nx),hdvk(0:nx),
     &  hsafety(0:nq),hAltFluidName,hAltFluidName2,hRef3,            !Alternative fluid hash numbers for mixing parameters and ASHRAE 34 safety classification
     &  hrefdir,hFldFileName(nq),                                    !hrefdir is NOT !$omp threadprivate(/REFDIR/)
     &  hsveosk(nx),hsvvisk(nx),hsvtcxk(nx),hsvstnk(nx),             !Pure fluid equations already loaded.
     &  heosmx,hvismx,htcxmx,hstnmx,hdilmx,heox,                     !Mixture models to read in.
     &  hsveos,hsvvis,hsvtcx,hsvstn,hsvref,                          !Mixture models already loaded.
     &  hsvfld(nx),hPDmix,                                           !Fluid files sent to SETUP on the last call.
     &  hsvmix,                                                      !Mixture file sent to SETUP on the last call (generally "hmx.bnc").
     &  hsavprk(nx),hsavpr                                           !Saves the mixture and EOS names before switching to PR.

c
c..Pure fluid fixed points.
      common /IDENT/  Tcrit(nq),Pcrit(nq),Dcrit(nq),                 !Critical point
     &                Ttpn(0:nq),Ptpn(0:nq),Dtpn(0:nq),Dtpv(0:nq),   !Triple point    xxx remove 0: ?
     &                Tnbp(nq),wmas(nq),                             !Normal boiling point and molar mass
     &                Tmeos(0:nq),Txeos(nq),Pxeos(nq),Dxeos(nq),     !EOS limits
     &                accen(0:nq),dipole(nq),                        !Acentric factor and Dipole moment
     &                gwpot(0:nq),odpot(nq),                         !Global warming and ozone depletion potentials
     &                Reos(nq),ReosOrg(nq),ReosDef(nq),              !Gas constant (R)
     &                xCmbst(nq),VerFL(nq),                          !Heat of combustion, FLD file version number,
     &                iFluidID(0:nq),ihash(0:nq),iAlthash(0:nq),nc   !Fluid ID number, hash number, and number of components

c..Reference states
      common /REFERN/ Tref(nx),Dref(nx),href(nx),sref(nx),           !Reference state values for the default specified in the fluid file (NBP, IIR, or ASH)
     &                Tdef(nq),Pdef(nq),hdef(nq),sdef(nq),           !Reference state values defined by the user (OTH or OT0)
     &                x0sav(nx),h0sav,s0sav,T0sav,P0sav,             !Reference state set by previous call to SETUP, used to check if SETREF must be called again.
     &                h0Ref3,s0Ref3,T0Ref3,P0Ref3,iPX0Flg,
     &                iSetRef2,iSetRf,ixfsav,lSkipRef,lSkipECS

c..Internal flags
      common /IFLAGS/ VrsNm,iAnc(0:nx),iEOSk(nq),iV10(nq),iErrPrnt,
     &                iAmWat,ixWat,ixCO2,ixAlc,ixAir,iGERG08,        !Various flags (e.g., ammonia/water mixture loaded, the GERG-2008 status, etc.)
     &                icnc,ncSet,iFitter,ihPath,iNumber,iDebug,kRPx,
     &                iPRold,iMassOld,iEOSmx,iDlDvFlg,
     &                itrnflag,iCpFlg,iPhi0Flg,iAGA8,iReset,
     &                iRefSv,iUnitNumbRef,iRDbnc,iRDmxm,
     &                iSetup,iSetRef,
     &                lGERG08,lAGA8,lAGA8sv,lAGA8Flg,
     &                lRcnst,lRCnstWag,lWagner,l7eta,
     &                lReset,lRst,lCache1,lCache2,lNan,lTest,l2phase


C..Thread private variables    (Keep separate from those above for multi-core use)
      common /THREAD/ zsav(nx,4),xsav(nx,4),ysav(nx,4),              !Variables for caching calculations that can be used if SATT or SATP is called with the same T or P.
     &                Tsav(4),Psav(4),Dlsav(4),Dvsav(4),Rxgas,
     &                xersav,yersav,zersav,iord(100),icsav(4),
     &                iersav,ihrsav,ihasav,ihbsav,ieflg,
     &                iersav2,iersav3,ihrsav2,
     &                ignbnd,ignbnd2,iErrNum,ixpg,iypg,izpg,
     &                iSatFlg,iSatFlg1,iSatFlg2,iTPFlg,
     &                lSatTP(4),lErrSwt,leta,ltcx
      common /THREADH/hasav,hbsav,hersav2

c..VLE routines
      common /VLE/    Tzred(nq),Dzred(nq),Vzred(nq),
     &                Tctrue(nq),Dctrue(nq),
     &                zPDmix(nq),PDmixTc,PDmixPc,PDmixDc,            !Pre-defined mixture critical point and composition from *.mix file
     &                crtT,crtP,crtD,TmxT,TmxP,TmxD,PmxT,PmxP,PmxD,  !Critical point, maxcondentherm, maxcondenbar
     &                d72l,d72v,Thmax,Dhmax,Hhmax,Temax,Demax,Eemax, !Points along the saturation line at the maximum or minimum in E, H, or S
     &                Tsmax,Tsmin,Tsmin2,Dsmax,Dsmin,Dsmin2,
     &                Ssmax,Ssmin,Ssmin2,
     &                ncPDmix,MixTyp,ierrCrt,ierrTmx,ierrPmx

c..Variables for spline curves
      common /SATSPL/ xArr(0:narm,0:nx5),cArr(narm,nx5),
     &                zSpln(nx),DatTmaxSpln,DatTminSpln,nArr,iSpline

c..ECS block for thermo
      common /ECS/    fecs (nx,nfhe,2),hecs (nx,nfhe,2),
     &                fdecs(nx,nfhe,2),hdecs(nx,nfhe,2),
     &                acfecs(nx),acfref(nx),Zcref(nx),
     &                nfecs (nx),nhecs (nx),
     &                nfdecs(nx),nhdecs(nx),TrnTj,TrnDj

c..Transport saved properties
      common /TRNVAR/ xeta0(nx),xetaB2(nx),xetar(nx),xetac(nx),
     &                xtcx0(nx),xtcxr (nx),xtcxc(nx)

c..Peng-Robinson equations
      common /PR_EOS/ fPRkij(nmxprb,nx,nx),fPRkij2(nmxprb,nx,nx),
     &                PcPR(nx),TcPR(nx),acnPR(nx),PRcoef(20,nx),
     &                iPRflag(nx),kijzero

c..Turn these on for multi-core applications, such as fitting equations of state.
!$omp threadprivate (/THREAD/)
!$omp threadprivate (/THREADH/)

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                   End file COMMONS.INC
c ======================================================================
