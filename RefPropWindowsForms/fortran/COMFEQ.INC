c-----------------------------------------------------------------------
c  Begin file COMFEQ.INC
c-----------------------------------------------------------------------

      character*255 hmodmx,hmfile,hbin,hbin_hmx,hmodmx_hmx,hinfo_mxm
      character hpar_mxm*8,htype_mxm*3,hmixsav*16

c..Enumerators
      parameter (iTc1=1)    !Don't change these, TC1 needs to be 1, TC7 needs to be 7, etc.
      parameter (iTc7=7)
      parameter (iVc1=1)
      parameter (iVc7=7)
      parameter (iMxLIN  =1)
      parameter (i2MxLIN =1)
      parameter (iMxiEQj =2)
      parameter (i2MxiEQj=2)
      parameter (iMxTR   =3)
      parameter (i2MxTR  =3)
      parameter (iMxLJ   =1000)
      parameter (iMxLM   =2000)
      parameter (iMxKW   =3000)
      parameter (iMxXR   =4000)
      parameter (iMxB    =5000)
      parameter (i2MxLJ  =1000)
      parameter (i2MxLM  =2000)
      parameter (i2MxKW  =3000)
      parameter (i2MxXR  =4000)
      parameter (i2MxB   =5000)
      parameter (iCP0CP =101)
      parameter (iCP0PH =102)
      parameter (iCP0PHG=103)

c..FEQ blocks
      common /FEQ/    aFEQ    (mxtrm,nq), giFEQ   (mxtrm,nq),   !Coefficients and exponents in the normal Helmholtz energy equation.
     &                tiFEQ   (mxtrm,nq), diFEQ   (mxtrm,nq),   ! This includes both the regular type of terms, and the
     &                dliFEQ  (mxtrm,nq), tliFEQ  (mxtrm,nq),   ! Gaussian-bell shaped terms first introduced by Wagner.
     &                alphaFEQ(mxtrm,nq), betaFEQ (mxtrm,nq),
     &                gammaFEQ(mxtrm,nq), deltaFEQ(mxtrm,nq),
     &                etaf    (mxtrm,nq), eidf    (mxtrm,nq),
     &                eitf    (mxtrm,nq),
     &                idlmax(nq),idliFEQ (mxtrm,nq),
     &                nFEQterms(nq), nFEQpe  (nq), nFEQcrt(nq), !Number of terms in the EOS functions.
     &                nFEQpoly (nq), nFEQgaus(nq), nFEQnan(nq),
     &                nFEQkg(nq), ltliflg(nq)                   !Logical varibles to indicate if the Span-Wagner terms as given in the CO2 and water publications are used.
      common /NONANA/ extd  (mxtrm,nq),   !Variables in the non-analytical terms that would never be used in current fitting practices, so keep separate from the others that are used for multi-core fitting.
     &                extt  (mxtrm,nq), extdt (mxtrm,nq),
     &                extt2 (mxtrm,nq), extd2 (mxtrm,nq)
      common /EOSSAV/ phisvf(mxtrm,nq), extf  (mxtrm,nq),   !Cached variables to increase calculation speed of properties.
     &                taup  (mxtrm,nq), delp  (mxtrm,nq),
     &                delb  (mxtrm,nq), taua  (mxtrm,nq),
     &                txpf  (mxtrm,nq), hxpf  (mxtrm,nq),
     &                axpf  (mxtrm,nq), dxpf  (mxtrm,nq),
     &                delli (mxtrm,nq), dellid(mxtrm,nq),
     &                feqsav(16,nq), tausvf(nq), delsvf(nq),
     &                phir01(mxtrm), phir10(mxtrm),
     &                phir02(mxtrm), phir20(mxtrm), lfeqsav(16,nq)

c..CP0 blocks
      common /CPP/    aCp0  (ncppmx,nx),TiCp0 (ncppmx,nx),   !Cp0 coefficients and exponents.
     &                aCp0h (ncppmx,nx),TiCp0h(ncppmx,nx),   !cosh and sinh terms
     &                csCp0h(ncppmx,nx),                     ! "         "
     &                aPhi0 (nph0mx,nx),TiPhi0(nph0mx,nx),   !PH0 coefficients and exponents.
     &                TredCp0 (nx),cRedCp0 (nx),             !Reducing parameters
     &                RxCp0(nx),RCp0Def(nx),RCp0Org(nx),
     &                nCp0poly(nx),nCp0Exp (nx),             !Number of terms
     &                nCp0Cosh(nx),nCp0Sinh(nx),
     &                nPhiCosh(nx),nPhiSinh(nx),
     &                nPhiLog (nx),nPhiTau (nx),nPhiExp(nx),
     &                iCp0xx(nx)
      common /CPPSAV/ Tsvph0(nx),Dsvph0(nx),TsvCp0(nx), !Variables for caching calculations that can be used when Cp0 is called again with the same T.
     &                Cp0Sav(nx),CpiSav(nx),CptSav(nx),
     &                PH0Sav(nx),PH1Sav(nx),PH2Sav(nx),
     &                Cp3Sav(nx),PH3Sav(nx)

c..Mixtures (from the hmx.bnc file)
      common /HMX/    amix  (nmxtrm,nmxcof,nx,nx),               !Coefficients and exponents for the binary mixing rule for the i,j pair.
     &                fmix  (nmxpar,nx,nx),f2mix (nmxpar,nx,nx),
     &                fmix2 (nmxpar,nx,nx),f2mix2(nmxpar,nx,nx),
     &                ftcij(nmxtc, nx,nx),fvcij(nmxvc, nx,nx),   !Coefficients for Type I critical line estimation equations.
     &                iBckwrd(nx,nx),                            !Identifies if a binary mixture is reverse of that in the hmx.bnc file.
     &                Tc12ij(nx,nx),Vc12ij(nx,nx),cT12ij(nx,nx),
     &                cV12ij(nx,nx), namix(nx,nx),namix2(nx,nx), !Number of aFEQ(i,j,k,1..3) terms
     &                namix3(nx,nx),idlmix(nx,nx)
      common /HMX2/   aamix  (nmxtrm,nmxcof),                    !Coefficients and exponents for the binary mixing rule for the i,j pair.
     &                ffmix  (nmxpar),ff2mix (nmxpar),
     &                ffmix2 (nmxpar),ff2mix2(nmxpar)
      common /HMXRUL/ ncode2(nx,nx),iMxRule(nx,nx),
     &                iModTc(nx,nx),iModVc(nx,nx),
     &                iModMx(nx,nx),iModMx2(nx,nx),iAltFlg(nx,nx),
     &                hmodmx(nx,nx),hmfile(0:nbin),hbin(nbin)
      common /HMXSAV/ phisav(nmsav,16),phisvm(0:nmxtrm,nmsav),
     &                dellim(nmxtrm,nmsav),tausvm(nmsav),delsvm(nmsav),
     &                phix01(nmxtrm),phix10(nmxtrm),hmixsav(nmsav)
c     common /HMXSAV2/pphisav(16),pphisvm(0:nmxtrm),
c    &                ddellim(nmxtrm),ttausvm,ddelsvm,lmixsav(16)
      common /EOSSV2/ phisv2(0:3,0:3,nq),phism2(0:3,0:3,nmsav),
     &                tausv2,delsv2

c..Derivatives of the Helmholtz energy with respect to x and n.
      common /PHIDR/  daddn(nx),d2adnn(nx,nx),d2addn(nx),
     &                d2adtn(nx),d2adbn(nx),
     &                dadxiTV(nx),daddxiTV(nx),
     &                dtdn(nx),dvdn(nx),dadxi(nx),
     &                dpdn(nx),daddx(nx),dadtx(nx),
     &                d2adxn(nx,nx),d2adxnTV(nx,nx),
     &                dadxij(nx,nx),dlnphiidxj(nx,nx),
     &                ddrdxn(nx,nx),dtrdxn(nx,nx),
     &                dpdxi(nx),xlnfi(nx),
     &                dphidp(nx),dphidt(nx),dphidxj(nx,nx),
     &                dlnfinidT(nx),dlnfinidV(nx),
     &                dphidnj(nx,nx),dmurdt(nx),dDdn(nx)

c..All information from the HMX.BNC file
      common /HMXBNC/ fTcij_hmx(6,nBNCmax),fVcij_hmx(6,nBNCmax),
     &                fmix_hmx(nmxpar,nBNCmax),fGG8_hmx(nmxpar,nBNCmax),
     &                fSTk_hmx(nmxprb,nBNCmax),fPRk_hmx(nmxstb,nBNCmax),
     &                xECS_hmx(9,nBNCmax),
     &                iIDij_hmx(2,nBNCmax),iGG8_hmx(nBNCmax),
     &                iModTc_hmx(nBNCmax),iModVc_hmx(nBNCmax),
     &                iModMx_hmx(4,nBNCmax),nMx_hmx,
     &                hModMx_hmx(nBNCmax),hbin_hmx(nBNCmax)
      common /MXMBNC2/aterm_mxm(nmxtrm,nmxcof,nbrule),
     &                fideal_mxm(nmxpar,nbrule),
     &                nterm_mxm(7,nbrule),nrule_mxm,
     &                iRuleXR0,iRuleLJ6,hpar_mxm(nmxpar,nbrule),
     &                htype_mxm(nbrule),hinfo_mxm(nbrule)


c..Turn these on for multi-core applications, such as fitting equations of state.
!$omp threadprivate (/EOSSAV/)
!$omp threadprivate (/EOSSV2/)
!$omp threadprivate (/NONANA/)
!$omp threadprivate (/CPPSAV/)

c..These are only needed when fitting mixtures.
!$omp threadprivate (/HMXSAV/)
!$omp threadprivate (/PHIDR/)

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                    End file COMFEQ.INC
c ======================================================================
