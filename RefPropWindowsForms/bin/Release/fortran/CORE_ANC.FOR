c  Begin file CORE_ANC.FOR
c
c  This file contains the core routines for the ancillary equations for
c    vapor pressure, saturated liquid density, and saturated vapor density.
c    Additionally, the code from CORE_MLT.FOR was added 12/16/2016, which
c    contains the core routines for melting lines and sublimation lines.
c
c  This file also contains core routines for the surface tension and
c    dielectric constant.
c
c  Contained here are:
c
c  Pure fluids routines (and which are the only ones containing the equations)
c     subroutine PSATK  (icomp,T,P,ierr,herr)
c     subroutine DPxSATK(icomp,T,dPxdTx,ierr,herr)
c     subroutine DLSATK (icomp,T,D,ierr,herr)
c     subroutine DVSATK (icomp,T,D,ierr,herr)
c     subroutine MELTK  (icomp,T,P,ierr,herr)
c     subroutine SUBLK  (icomp,T,P,ierr,herr)
c     subroutine MLTH2O (T,P1,P2)
c     subroutine MLTD2O (T,P1,P2)
c     subroutine MLTPH2 (icomp,T,P)
c
c  Mixture routines
c     subroutine PSATT  (T,z,P,ierr,herr)
c     subroutine DLSATT (T,z,D,ierr,herr)
c     subroutine DVSATT (T,z,D,ierr,herr)
c     subroutine MELTT  (T,z,P,ierr,herr)
c     subroutine SUBLT  (T,z,P,ierr,herr)
c
c  Iterative routines to find temperature when it is unknown
c     subroutine TSATP  (P,z,T,ierr,herr)
c     subroutine TSATD  (D,z,T,ierr,herr)
c     subroutine MELTP  (P,z,T,ierr,herr)
c     subroutine SUBLP  (P,z,T,ierr,herr)
c
c  Initialize routines by calling subroutine SETANC
c     subroutine SETANC (nread,icomp,iFlg,ierr,herr)
c
c  Surface tension and dielectric constant
c     subroutine SURFT  (T,Dl,z,sigma,ierr,herr)
c     subroutine SURTEN (T,Dl,Dv,x,y,sigma,ierr,herr)
c     subroutine STN    (T,Dl,Dv,x,y,sigma,ierr,herr)
c     subroutine STNK   (icomp,tau,sigma,ierr,herr)
c     subroutine DIELEC (T,D,z,de)
c     subroutine DEK    (icomp,T,D,de,ierr,herr)
c
c  Internal
c     subroutine TLSATHES (iat,a,z,T,D,ierr,herr)
c     subroutine ANCERR1  (icomp,T,hk,hancnme,ierr,herr)
c     subroutine ANCERR2  (icomp,b,janc,hk,hancnme,ierr,herr)
c     subroutine CRITF    (zeta,z,Tc,Pc,Dc,ierr,herr)
c
c ======================================================================
c
      subroutine PSATK (icomp,T,P,ierr,herr)
c
c  Compute pure fluid vapor or liquid pressures with the appropriate
c  ancillary equation.
c
c  Inputs:
c    icomp--Component i.  For liquid pressure equations (pseudo-pure
c           fluids only), send -i.
c        T--Temperature [K]
c
c  Outputs:
c        P--Vapor or liquid pressure [kPa]
c     ierr--Error flag:  0 - Successful
c                      121 - Temperature greater than critical point temperature
c     herr--Error string (character*255)
c
c  03-13-01 EWL, original version
c  06-03-15 EWL, renumber error numbers throughout all ancillary routines to be consistent with other code.
c  06-01-16 EWL, shorten and remove old code no longer in use in all routines in CORE_ANC.FOR

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PSATK

      j=icomp
      i=ABS(icomp)
      if (iAnc(i).eq.0) j=i
      P=Pcrit(i)
      if (j.ge.0) then
        ip=ipstyp(i)
        call ANCERR1 (i,T,hpsk(i),'PSATK',ierr,herr)
        Tr=ABS(1d0-T/psTrd(i))
      else
        ip=ipltyp(i)
        call ANCERR1 (i,T,hplk(i),'PSATK',ierr,herr)
        Tr=ABS(1d0-T/plTrd(i))
      endif

      if (ierr.gt.0) RETURN
      if (Tr.le.0.) RETURN
      if (MOD(ip,2).eq.0) Tr=SQRT(Tr)       !Even values of ip
      trlog=XLOG(Tr)

      if (j.ge.0) then
        Pr=SUM(pska(1:nps1(i),i)*EXP(psexp(1:nps1(i),i)*trlog))
        u=psTrd(i)/T*Pr
      else
        Pr=SUM(plka(1:npl1(i),i)*EXP(plexp(1:npl1(i),i)*trlog))
        u=plTrd(i)/T*Pr
      endif

      if (u.gt.10.) u=10d0
      if (Pr.gt.10. .and. ip.gt.2) Pr=10d0
      if (ip.eq.1 .or. ip.eq.2) Pr=1d0+Pr
      if (ip.eq.3 .or. ip.eq.4) Pr=EXP4(Pr)
      if (ip.eq.5 .or. ip.eq.6) Pr=EXP(u)
      P=psPrd(i)*Pr
      if (j.lt.0) P=plPrd(i)*Pr

      end                                               subroutine PSATK
c
c ======================================================================
c
      subroutine DPxSATK (icomp,T,dPxdTx,ierr,herr)
c
c  Compute the first or second derivative of the vapor pressure with the
c  appropriate equation.
c
c  Inputs:
c    icomp--Component i.  Send -i to calculate the second derivative.
c        T--Temperature [K]
c
c  Outputs:
c   dPxdTx--dP/dT [kPa/K] for icomp>0
c           d^2P/dT^2 [kPa/K^2] for icomp<0
c     ierr, herr; See error codes in the ANCERR1 routine.
c
c  05-30-01 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DPxSATK

      i=ABS(icomp)
      ip=ipstyp(i)
      dPxdTx=0d0
      call ANCERR1 (i,T,hpsk(i),'DPxSATK',ierr,herr)
      if (ierr.gt.0) RETURN
      Tr=ABS(1d0-T/psTrd(i))
      if (Tr.lt.0.) Tr=0d0
      if (MOD(ip,2).eq.0) Tr=SQRT(Tr)       !Even values of ip
      Pr=0d0
      dpr=0d0
      d2pr=0d0
      do k=1,nps1(i)
        pe=psexp(k,i)
        Pr=Pr+pska(k,i)*Tr**pe
        if (MOD(ip,2).eq.1) then            !Odd values of ip
          dpr=dpr-pe*pska(k,i)*XPOWR(Tr,pe-1d0)/psTrd(i)
          d2pr=d2pr+pe*(pe-1d0)*pska(k,i)*XPOWR(Tr,pe-2d0)/
     &         psTrd(i)**2
        else                                !Even values of ip
          dpr=dpr-pe/2d0*pska(k,i)*XPOWR(Tr,pe-2d0)/psTrd(i)
          d2pr=d2pr+pe*(pe-2d0)/4d0*pska(k,i)*XPOWR(Tr,pe-4d0)/
     &         psTrd(i)**2
        endif
      enddo

      Tr=psTrd(i)/T
      if (icomp.ge.0) then
        if (ip.eq.1 .or. ip.eq.2) Pr=dpr
        if (ip.eq.3 .or. ip.eq.4) Pr=EXP4(Pr)*dpr
        if (ip.eq.5 .or. ip.eq.6) Pr=EXP4(Tr*Pr)*Tr*(dpr-Pr/T)
      else
        if (ip.eq.1 .or. ip.eq.2) Pr=d2pr
        if (ip.eq.3 .or. ip.eq.4) Pr=EXP4(Pr)*(dpr**2+d2pr)
        if (ip.eq.5 .or. ip.eq.6) Pr=EXP4(Tr*Pr)*
     &     ((Tr*(dpr-Pr/T))**2+2d0*Tr/T**2*Pr-2d0*Tr/T*dpr+Tr*d2pr)
      endif
      dPxdTx=psPrd(i)*Pr

      end                                             subroutine DPxSATK
c
c ======================================================================
c
      subroutine DLSATK (icomp,T,D,ierr,herr)
c
c  Compute pure fluid saturated liquid density with appropriate equation.
c
c  Inputs:
c    icomp--Component i
c        T--Temperature [K]
c
c  Outputs:
c        D--Saturated liquid density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      121 - Temperature greater than critical point temperature
c                      501 - No equation available
c     herr--Error string (character*255)
c
c  03-13-01 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DLSATK

      i=idltyp(icomp)
      D=Dcrit(icomp)
      call ANCERR1 (icomp,T,hdlk(icomp),'DLSATK',ierr,herr)
      if (ierr.gt.0) RETURN

      Tr=ABS(1d0-T/dltrd(icomp))
      if (Tr.le.0.) RETURN
      if (MOD(i,2).eq.0) Tr=Tr**thrd        !Even values of i
      trlog=XLOG(Tr)
      Dr=SUM(dlka(1:ndl1(icomp),icomp)*
     &       EXP(dlexp(1:ndl1(icomp),icomp)*trlog))

      if (i.eq.1 .or. i.eq.2) Dr=1d0+Dr
      if (i.eq.3 .or. i.eq.4) Dr=EXP1(Dr)
      if (i.eq.5 .or. i.eq.6) Dr=EXP1(dltrd(icomp)/T*Dr)
      D=dldrd(icomp)*Dr

      end                                              subroutine DLSATK
c
c ======================================================================
c
      subroutine DVSATK (icomp,T,D,ierr,herr)
c
c  Compute pure fluid saturated vapor density with appropriate equation.
c
c  Inputs:
c    icomp--Component i
c        T--Temperature [K]
c
c  Outputs:
c        D--Saturated vapor density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      121 - Temperature greater than critical point temperature
c                      501 - No equation available
c     herr--Error string (character*255)
c
c  03-13-01 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DVSATK

      D=Dcrit(icomp)
      call ANCERR1 (icomp,T,hdvk(icomp),'DVSATK',ierr,herr)
      if (ierr.gt.0) RETURN

      i=idvtyp(icomp)
      Tr=ABS(1d0-T/dvtrd(icomp))
      if (Tr.le.0.) RETURN
      if (MOD(i,2).eq.0) Tr=Tr**thrd        !Even values of i
      trlog=XLOG(Tr)
      Dr=SUM(dvka(1:ndv1(icomp),icomp)*
     &       EXP(dvexp(1:ndv1(icomp),icomp)*trlog))

      if (Dr.gt.100 .and. i.gt.2) Dr=100
      if (i.eq.1 .or. i.eq.2) Dr=1d0+Dr
      if (i.eq.3 .or. i.eq.4) Dr=EXP(Dr)
      if (i.eq.5 .or. i.eq.6) Dr=EXP1(dvtrd(icomp)/T*Dr)
      D=dvdrd(icomp)*Dr

      end                                              subroutine DVSATK
c
c ======================================================================
c
      subroutine MELTK (icomp,T,P,ierr,herr)
c
c  Compute melting line with appropriate core model.
c
c  Inputs:
c    icomp--Component i (for water and heavy water, send -icomp to obtain
c           the root with the lower pressure at T<Ttrp)
c        T--Temperature [K]
c
c  Outputs:
c        P--Melting line pressure [kPa]
c     ierr--Error flag:  0 - Successful
c                        1 - T<Ttrp
c                       -4 - P<Ptrp (for MELTP routine)
c                      501 - No equation available
c                      502 - Unknown melting line equation
c     herr--Error string (character*255)
c
c
c
c  There are two functional forms for the melting line, labeled
c    in the fluid files as ML1 and ML2:
c       ML1:  P=Pred*Pr
c       ML2:  P=Pred*Exp(Pr)
c    where:
c       Pr=Sum[Nk*Tr^tk]+Sum[Nk*(Tr-1)^tk]+Sum[Nk*(Log Tr)^tk]
c       Tr=T/Tred
c    In the fluid file, Tred and Pred (the reducing values) are given
c      first, followed by the number of terms in each of the summations,
c      and then followed by the coefficients Nk and exponents tk (one
c      term with Nk and tk listed per line).
c
c
c  11-16-98 EWL, original version
c  06-03-15 EWL, return an error number if fluid does not have a melting line

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MELTK

      ierr=0
      i=ABS(icomp)
      P=0d0
      Tlow=Ttpn(i)
      if (hmeltk(i).eq.'NBS') then          !No melting line available.
        call ERRNUM (501,0,'MELTK',' ',0d0,0d0,0d0,ierr,herr)
        goto 500
      elseif (hmeltk(i).eq.'MLW') then      !Get water and heavy water's lower temperature limit.
        Tlow=-999d0
        call MLTH2O (Tlow,P1,P2)
      elseif (hmeltk(i).eq.'MLH') then
        Tlow=-999d0
        call MLTD2O (Tlow,P1,P2)
      endif
      if (T.lt.Tlow-1.d-8) then             !T<Ttrp (or Tlow for water and D2O)
        call ERRNUM (1,4,'MELTK','below',T,Ttpn(i),0d0,ierr,herr)
        goto 500
      endif

      if (hmeltk(i).eq.'MLW') then
        call MLTH2O (T,P,P2)                !Water
        if (icomp.lt.0) P=P2
        if (T.lt.Ttpn(i) .and. icomp.gt.0)
     &    call ERRNUM(-503,0,'MELTK',' ',0d0,0d0,0d0,ierr,herr)
      elseif (hmeltk(i).eq.'MLH') then
        call MLTD2O (T,P,P2)                !Heavy Water
        if (icomp.lt.0) P=P2
        if (T.lt.Ttpn(i) .and. icomp.gt.0)
     &    call ERRNUM(-503,0,'MELTK',' ',0d0,0d0,0d0,ierr,herr)
      elseif (hmeltk(i).eq.'MLP') then
        call MLTPH2 (i,T,P)                 !Parahydrogen
      elseif (hmeltk(i)(1:2).eq.'ML') then
        n=ntrmm1(i)
        n2=ntrmm2(i)+n
        n3=ntrmm3(i)+n2
        Tr=T/trDmlt(i)
        Pr=SUM(pmeltk(1:n,i)*Tr**pmltex(1:n,i))
        if (Tr.gt.1.)
     &        Pr=Pr+SUM(pmeltk(n+1:n2,i)*(Tr-1d0)**pmltex(n+1:n2,i))
        Pr=Pr+SUM(pmeltk(n2+1:n3,i)*XLOG(Tr)**pmltex(n2+1:n3,i))
        P=Pr
        if (imlttyp(i).eq.1) P=prDmlt(i)*Pr
        if (imlttyp(i).eq.2) P=prDmlt(i)*EXP4(Pr)
      else
        call ERRNUM (502,0,'MELTK',' ',0d0,0d0,0d0,ierr,herr)
      endif

 500  RETURN

      end                                               subroutine MELTK
c
c ======================================================================
c
      subroutine SUBLK (icomp,T,P,ierr,herr)
c
c  Compute sublimation line with appropriate core model.
c
c  Inputs:
c    icomp--Component i
c        T--Temperature [K]
c
c  Outputs:
c        P--Sublimation line pressure [kPa]
c     ierr--Error flag:  0 - Successful
c                        1 - T>Ttrp
c                       -4 - P>Ptrp
c                      501 - No equation available
c                      502 - Unknown sublimation line equation
c     herr--Error string (character*255)
c
c  02-12-99 EWL, original version
c  06-03-15 EWL, return an error number if fluid does not have a melting line

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SUBLK

      ierr=0
      herr=' '
      i=icomp
      P=0d0
      if (hsublk(i).eq.'NBS') then
        call ERRNUM(501,0,'SUBLK',' ',0d0,0d0,0d0,
     &              ierr,herr)
      elseif (T.le.0.) then
        P=0d0
      elseif (T.gt.Ttpn(i)) then
        call ERRNUM (1,4,'SUBLK','above',T,Ttpn(i),0d0,ierr,herr)
      elseif (hsublk(i)(1:2).eq.'SB') then
        n=ntrmb1(i)
        n2=ntrmb2(i)+n
        n3=ntrmb3(i)+n2
        Tr=T/trDsbl(i)
        Pr=SUM(psublk(1:n,i)*Tr**psblex(1:n,i))
        Pr=Pr+SUM(psublk(n+1:n2,i)*(ABS(1d0-Tr))**psblex(n+1:n2,i))
        Pr=Pr+SUM(psublk(n2+1:n3,i)*XLOG(Tr)**psblex(n2+1:n3,i))
        if (itypsbl(i).eq.1) P=prDsbl(i)*Pr
        if (Pr.lt.200) then
          if (itypsbl(i).eq.2) P=prDsbl(i)*EXP4(Pr)
          if (itypsbl(i).eq.3) P=prDsbl(i)*EXP4(trDsbl(i)/T*Pr)      !Used for CO2
        endif
      else
        call ERRNUM (502,0,'SUBLK',' ',0d0,0d0,0d0,ierr,herr)
      endif

      end                                               subroutine SUBLK
c
c ======================================================================
c
      subroutine MLTH2O (T,P1,P2)
c
c  Compute melting line of water, see fluid file for reference.
c
c  Inputs:
c        T--Temperature [K]
c
c  Outputs:
c       P1--Higher melting line pressure [kPa]
c       P2--Lower melting line pressure [kPa]
c       Above 273.16 K, only P1 returns a physical answer.
c       Between 251.165 and 273.16 K, two pressures are returned.
c
c  If flags of -998 or -999 are sent for the temperature, the value of
c    the lowest temperature possible (251.165 K) is sent back in T,
c    the pressure at that point is sent back in P1, and the density
c    at that point is sent back in P2 if the flag -998 is used.
c
c  11-17-98 EWL, original version
c  10-22-10 EWL, update with latest equation of Wagner
c  12-12-16 EWL, add -998 and -999 codes

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MLTH2O

c  Set the minimum temperature where water can be a fluid (not including
c   metastable phases), and set the melting pressure at that state.
      if (T.eq.-999. .or. T.eq.-998.) then
        z(1:nc)=1d0
        iflag=0
        if (T.eq.-998.) iflag=1
        T=251.165d0
        P1=208566d0
        P2=0d0
        if (iflag.eq.1) call TPRHO (T,P1,z,1,0,P2,ierr,herr)
        RETURN
      endif

      P1=0d0
      P2=0d0
c     Ice I
      if (T.gt.251.165d0 .and. T.le.273.1600000001d0) then
        th=T/273.16d0
        P2=0.611657d0*(1d0+
     &     1195393.37d0*(1d0-th**3d0)+
     &     80818.3159d0*(1d0-th**25.75d0)+
     &     3338.26860d0*(1d0-th**103.75d0))
      endif

c     Ice III
      if (T.ge.251.165d0 .and. T.lt.256.164d0) then
        th=T/251.165d0
        P1=208566d0*(1d0-0.299948d0*(1d0-th**60d0))
      endif

c     Ice V
      if (T.ge.256.164d0 .and. T.lt.273.31d0) then
        th=T/256.164d0
        P1=350100d0*(1d0-1.18721d0*(1d0-th**8d0))
      endif

c     Ice VI
      if (T.ge.273.31d0 .and. T.lt.355.) then
        th=T/273.31d0
        P1=632400d0*(1d0-1.07476d0*(1d0-th**4.6d0))
      endif

c     Ice VII
      if (T.ge.355. .and. T.lt.637.8d0) then
        th=T/355d0
        P1=2216000d0*EXP4(0.173683d+1*(1d0-th**(-1d0))-
     &                    0.544606d-1*(1d0-th**5d0)+
     &                    0.806106d-7*(1d0-th**22d0))
      endif

c  The Wagner melting equation for water above its upper limit of 715 K
c    quickly drops and becomes negative above 840 K.  The following patch
c    corrects this problem.  Although the Wagner equation is valid up to 715 K,
c    the equation is used starting at 637.8 K, where the patch and Wagner's
c    equation cross.  Differences between the two equations within these two
c    temperatures is less than 1% up to 693 K, at which point the Wagner
c    equation starts to turn downward.  The maximum 1% error is well within
c    the uncertainty of the original equation in this region, which is about 5%.

      if (T.ge.637.8d0) P1=1000d0*EXP4(0.00805246d0*T+4.27074d0)
      if (P1.le.0.) P1=0d0
      if (P2.le.0.) P2=P1

      end                                              subroutine MLTH2O
c
c ======================================================================
c
      subroutine MLTD2O (T,P1,P2)
c
c  Compute melting line of heavy water, see fluid file for reference.
c
c  Inputs:
c    icomp--Component i
c        T--Temperature [K]
c
c  Outputs:
c       P1--Higher melting line pressure [kPa]
c       P2--Lower melting line pressure [kPa]
c       Above 276.969 K, only P1 returns a physical answer.
c       Between 254.415 and 276.969 K, two pressures are returned.
c
c  If flags of -998 or -999 are sent for the temperature, the value of
c    the lowest temperature possible (254.415 K) is sent back in T,
c    the pressure at that point is sent back in P1, and the density
c    at that point is sent back in P2 if the flag -998 is used.
c
c  Written by S. Herrig, NIST Thermophysics Division, Boulder, Colorado
c  07-19-17  SH, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MLTD2O

c  Set the minimum temperature where heavy water can be a fluid (not including
c   metastable phases), and the melting pressure at that state.
      if (T.eq.-999. .or. T.eq.-998.) then
        z(1:nc)=1d0
        iflag=0
        if (T.eq.-998.) iflag=1
        T=254.415d0
        P1=220160d0
        P2=0d0
        if (iflag.eq.1) call TPRHO (T,P1,z,1,0,P2,ierr,herr)
        RETURN
      endif

      P1=0d0
      P2=0d0
c     Ice I
      if (T.gt.254.415d0 .and. T.le.276.9690000001d0) then
        th=T/276.969d0
        P2=0.66159d0*(1d0-30153d0*(1d0-th**5.5d0)+
     &      692503d0*(1d0-th**8.2d0))
      endif

c     Ice III
      if (T.ge.254.415d0 .and. T.lt.258.661d0) then
        th=T/254.415d0
        P1=222.41d3*(1d0-0.802871d0*(1d0-th**33))
      endif

c     Ice V
      if (T.ge.258.661d0 .and. T.lt.275.748d0) then
        th=T/258.661d0
        P1=352.19d3*(1d0-1.280388d0*(1d0-th**7.6d0))
      endif

c     Ice VI
      if (T.ge.275.748d0 .and. T.lt.355d0) then
        th=T/275.748d0
        P1=634.53d3*(1d0-1.276026d0*(1d0-th**4))
      endif

c     Ice VII
      if (T.ge.355. .and. T.lt.637.8d0) then
        th=T/355d0
        P1=2049.06231059726d3*EXP4(0.173683d1*(1d0-th**(-1))-        !This is the equation for normal water, except for the first coefficient,
     &                        0.544606d-1*(1d0-th**5)+               ! which is equal to the pressure at 355 K for the Ice VI region.
     &                        0.806106d-7*(1d0-th**22))

      endif

      if (T.ge.637.8d0) P1=1000d0*EXP4(0.00805246d0*T+4.27074d0)+    !Equation for normal water.
     &                     11251794.2370338d0-12168481.1136408d0     !Add the difference between normal water and heavy water at 637.8 K.

      if (P1.le.0.) P1=0d0
      if (P2.le.0.) P2=P1

      end                                              subroutine MLTD2O
c
c ======================================================================
c
      subroutine MLTPH2 (icomp,T,P)
c
c  Compute melting line of parahydrogen.
c
c  Inputs:
c    icomp--Component i
c        T--Temperature [K]
c
c  Output:
c        P--Melting line pressure [kPa]
c
c  11-17-98 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'

      Tr=T/trDmlt(icomp)
      Pr=0d0
      if (T.gt.22) then
        Pr=pmeltk(1,icomp)+pmeltk(2,icomp)*Tr**pmltex(2,icomp)
      else
        Pr=pmeltk(3,icomp)+pmeltk(4,icomp)*Tr**pmltex(4,icomp)
      endif
      P=prDmlt(icomp)*Pr
      end                                              subroutine MLTPH2
c
c ======================================================================
c
      subroutine PSATT (T,z,P,ierr,herr)
c
c  Compute mixture or pure fluid vapor pressure with appropriate
c  ancillary equation.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        P--Vapor pressure [kPa]
c     ierr, herr; See error codes in the ANCERR1 routine.

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PSATT

      ierr=0
      P=0d0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then   !Pure fluid
        call PSATK (icomp,T,P,ierr,herr)
      else
        call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)
        do i=1,nc            !Mixture
          call PSATK (i,T/Tc*Tcrit(i),pp,ierr,herr)
          if (ierr.gt.0) EXIT
          P=P+z(i)*pp/Pcrit(i)
        enddo
        P=P*Pc
      endif
      if (ierr.gt.0) P=0d0
      end                                               subroutine PSATT
c
c ======================================================================
c
      subroutine DLSATT (T,z,D,ierr,herr)
c
c  Compute mixture or pure fluid saturated liquid density with appropriate
c  ancillary equation.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        D--Saturated liquid density [mol/L]
c     ierr, herr; See error codes in the ANCERR1 routine.

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DLSATT

      ierr=0
      D=0d0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then   !Pure fluid
        call DLSATK (icomp,T,D,ierr,herr)
      else
        call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)
        do i=1,nc            !Mixture
          Ti=T/Tc*Tcrit(i)
          call DLSATK (i,Ti,dd,ierr,herr)
          if (ierr.gt.0) EXIT
          D=D+z(i)*dd/Dcrit(i)
        enddo
        D=D*Dc
      endif
      if (ierr.gt.0) D=0d0
      end                                              subroutine DLSATT
c
c ======================================================================
c
      subroutine DVSATT (T,z,D,ierr,herr)
c
c  Compute mixture or pure fluid saturated vapor density with appropriate
c  ancillary equation.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        D--Saturated vapor density [mol/L]
c     ierr, herr; See error codes in the ANCERR1 routine.

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DVSATT

      ierr=0
      D=0d0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then   !Pure fluid
        call DVSATK (icomp,T,D,ierr,herr)
      else
        call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)
        do i=1,nc            !Mixture
          call DVSATK (i,T/Tc*Tcrit(i),dd,ierr,herr)
          if (ierr.gt.0) EXIT
          D=D+z(i)*dd/Dcrit(i)
        enddo
        D=D*Dc
      endif
      if (ierr.gt.0) D=0d0
      end                                              subroutine DVSATT
c
c ======================================================================
c
      subroutine MELTT (T,z,P,ierr,herr)
c
c  Compute the melting line pressure as a function of temperature
c  and composition.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        P--Melting line pressure [kPa]
c     ierr--Error flag:  0 - Successful
c                      501 - No equation available
c     herr--Error string (character*255)
c
c  11-16-98 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MELTT

      ierr=0
      herr=' '
      P=0d0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then   !Pure fluid
        call MELTK (icomp,T,P,ierr,herr)
      else
        call REDLINEAR (-1,z,Tred,Pred,Dred,ierr,herr)
        do i=1,nc            !Mixture
          T2=XDIV(T,Tred)*Ttpn(i)
          if (T2.lt.Ttpn(i)) T2=Ttpn(i)
          call MELTK (i,T2,pp,ierr,herr)
          if (ierr.gt.0 .or. pp.lt.1d-15) goto 500    !Do not return a value if any of the fluids does not have a melting line.
          P=P+z(i)*XLOG(XDIV(pp,Ptpn(i)))
        enddo
        P=EXP4(P)*Pred
        if (nc.gt.1) call ERRNUM (-271,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      endif

 500  if (ierr.gt.0) P=0d0

      end                                               subroutine MELTT
c
c ======================================================================
c
      subroutine SUBLT (T,z,P,ierr,herr)
c
c  Compute the sublimation line pressure as a function of temperature
c  and composition.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        P--Sublimation line pressure [kPa]
c     ierr--Error flag:  0 - Successful
c                      501 - No equation available
c     herr--Error string (character*255)
c
c  02-12-99 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SUBLT

      ierr=0
      P=0d0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then   !Pure fluid
        call SUBLK (icomp,T,P,ierr,herr)
      else
        call REDLINEAR (-1,z,Tred,Pred,Dred,ierr,herr)
        do i=1,nc            !Mixture
          call SUBLK (i,T/Tred*Ttpn(i),pp,ierr,herr)
          if (ierr.gt.0 .or. pp.lt.1d-15) EXIT        !Do not return a value if any of the fluids does not have a melting line.
          P=P+z(i)*XLOG(XDIV(pp,Ptpn(i)))
        enddo
        P=EXP4(P)*Pred
      endif

 500  if (ierr.gt.0) P=0d0

      end                                               subroutine SUBLT
c
c ======================================================================
c
      subroutine TSATP (P,z,T,ierr,herr)
c
c  Compute the vapor temperature as a function of pressure
c  and composition.
c
c  If the input pressure is negative, compute the liquid temperature as a
c  function of liquid pressure and composition (used only for
c  pseudo-pure fluids).
c
c  Inputs:
c        P--Vapor pressure [kPa]
c           If negative, liquid pressure [kPa] (the negative sign is only used as a flag).
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr, herr; See error codes in the ANCERR2 routine.
c
      include 'COMMONS.INC'
      include 'COMANC.INC'
      character herr*255,hpvl*3
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TSATP


      if (P.gt.0.) then
        i=1
        hpvl='PVA'
      else
        i=2
        hpvl='PLA'
      endif
      PP=ABS(P)
      call ISPURE (z,icomp)
      call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)
      T=Tc
      if (ABS(PP-Pc).lt.1d-6) RETURN
      T=0.8d0*Tc
      call ANCERR2 (icomp,PP,i,hpsk(icomp),'TSATP',ierr,herr)
      if (ierr.ge.100) RETURN
      dd=0d0
      call ROOTFIND (hpvl,0,0,PP,z,0,0,1,1,2,1,T,dd,ot,ierr,herr)
      end                                               subroutine TSATP
c
c ======================================================================
c
      subroutine TSATD (D,z,T,ierr,herr)
c
c  Compute the saturated temperature as a function of saturated density
c  and composition.
c
c  Inputs:
c        D--Saturated density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr--Error flag:  0 - Successful
c                        2 - Density greater than triple point density
c                      501 - No equation available
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TSATD

      call ISPURE (z,icomp)
      call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)
      T=Tc
      T=0.8d0*Tc
      dd=0d0
      ot=0d0
      if (D.ge.Dc) then
        call ANCERR2 (icomp,D,6,hdlk(icomp),'DLSATK',ierr,herr)
        if (ierr.ge.100) RETURN
        call ROOTFIND ('DLA',0,0,D,z,0,0,1,11,0,0,T,dd,ot,ierr,herr)
      else
        call ANCERR2 (icomp,D,5,hdvk(icomp),'DVSATK',ierr,herr)
        if (ierr.ge.100) RETURN
        call ROOTFIND ('DVA',0,0,D,z,0,0,1,12,2,1,T,dd,ot,ierr,herr)
      endif
      end                                               subroutine TSATD
c
c ======================================================================
c
      subroutine MELTP (P,z,T,ierr,herr)
c
c  Compute the melting line temperature as a function of pressure
c  and composition.
c
c  Inputs:
c        P--Melting line pressure [kPa]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr--Error flag:  0 - Successful
c                       -4 - Pressure below triple point pressure
c                      501 - No equation available
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MELTP

      call ISPURE (z,icomp)
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      T=Tmin
      call ANCERR2 (icomp,P,3,hmeltk(icomp),'MELTP',ierr,herr)
      if (ierr.ne.0) RETURN
      T=1.05d0*Tmin
      dd=0d0
      call ROOTFIND ('PMA',0,0,P,z,0,0,1,3,0,0,T,dd,ot,ierr,herr)
      end                                               subroutine MELTP
c
c ======================================================================
c
      subroutine SUBLP (P,z,T,ierr,herr)
c
c  Compute the sublimation line temperature as a function of pressure
c  and composition.
c
c  Inputs:
c        P--Sublimation line pressure [kPa]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr--Error flag:  0 - Successful
c                       -4 - Pressure above triple point pressure
c                      501 - No equation available
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SUBLP

      call ISPURE (z,icomp)
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      if (icomp.ne.0) Tmin=Ttpn(icomp)           !Overwrite the 251 K of water with 273 K.
      T=Tmin
      call ANCERR2 (icomp,P,4,hsublk(icomp),'SUBLP',ierr,herr)
      if (ierr.ge.100) RETURN
      T=0.95d0*Tmin
      dd=0d0
      call ROOTFIND ('PSA',0,0,P,z,0,0,1,4,2,1,T,dd,ot,ierr,herr)

      end                                               subroutine SUBLP
c
c ======================================================================
c
      subroutine TLSATHES (iat,a,z,T,D,ierr,herr)
c
c  Compute the liquid temperature as a function of composition and
c  enthalpy, energy, or entropy from the ancillary equations.
c  The output temperature is determined from a procedure that
c  iteratively finds a temperature and its associated liquid density
c  from the ancillary equation that agrees with H, E, or S calculated
c  from the equation of state.
c
c  Inputs:
c      iat--One of the enumerations iLetterH, iLetterE, or iLetterS
c           depending on the input properties
c        a--Molar energy, enthalpy, or entropy [J/mol or J/(mol-K)]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c        D--Density [mol/l]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c     (See subroutine ROOTFIND for error numbers.)

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character herr*255,at*3
      dimension z(ncmax)

      call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)
      T=Tc*0.8d0
      at=' '
      if (iat.eq.iLetterE) at='ELA'
      if (iat.eq.iLetterH) at='HLA'
      if (iat.eq.iLetterS) at='SLA'
      call ROOTFIND (at,iat,0,a,z,0,0,1,21,0,0,T,D,ot,ierr,herr)
      end                                            subroutine TLSATHES
c
c ======================================================================
c
      subroutine SETANC (nread,icomp,iFlg,ierr,herr)
c
c  Set up working arrays for use with the different ancillary equations
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in mixture (1.nc); 1 for pure fluid
c     iFlg--Flag indicating which ancillary to setup:
c           1 - Liquid pressure
c           2 - Vapor pressure
c           3 - Liquid density
c           4 - Vapor density
c           5 - Melting pressure
c           6 - Sublimation pressure
c           7 - Surface tension
c           8 - Dielectric constant
c
c  Outputs:
c     ierr--Equals 0 always

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr

      ierr=0
      herr=' '
      i=icomp
      read (nread,*,err=102) !The upper and lower limits
      read (nread,*,err=102) !  are not currently used,
      read (nread,*,err=102) !  and have been replaced
      read (nread,*,err=102) !  by dummy values.

c  Read vapor pressure equation.
      if (iFlg.eq.2) then
        read (nread,*,err=102) psTrd(i),psPrd(i)    !Reducing parameters
        read (nread,*,err=102) nps1(i),nps2(i),nps3(i)
        do k=1,nps1(i)
          read (nread,*,err=102) pska(k,i),psexp(k,i)
        enddo

c  Read liquid pressure equation.
      elseif (iFlg.eq.1) then
        read (nread,*,err=102) plTrd(i),plPrd(i)    !Reducing parameters
        read (nread,*,err=102) npl1(i),npl2(i),npl3(i)
        do k=1,npl1(i)
          read (nread,*,err=102) plka(k,i),plexp(k,i)
        enddo

c  Read liquid density equations.
      elseif (iFlg.eq.3) then
        read (nread,*,err=102) dltrd(i),dldrd(i)    !Reducing parameters
        read (nread,*,err=102) ndl1(i),ndl2(i),ndl3(i)
        do k=1,ndl1(i)
          read (nread,*,err=102) dlka(k,i),dlexp(k,i)
        enddo

c  Read vapor density equations.
      elseif (iFlg.eq.4) then
        read (nread,*,err=102) dvtrd(i),dvdrd(i)    !Reducing parameters
        read (nread,*,err=102) ndv1(i),ndv2(i),ndv3(i)
        do k=1,ndv1(i)
          read (nread,*,err=102) dvka(k,i),dvexp(k,i)
        enddo

c  Read melting line parameters.
      elseif (iFlg.eq.5) then
        read (nread,*,err=102) trDmlt(i),prDmlt(i)  !Reducing parameters
        read (nread,*,err=102) ntrmm1(i),ntrmm2(i),ntrmm3(i)
        do k=1,ntrmm1(i)+ntrmm2(i)+ntrmm3(i)
          read (nread,*,err=102) pmeltk(k,i),pmltex(k,i)
        enddo

c  Read sublimation line parameters.
      elseif (iFlg.eq.6) then
        read (nread,*,err=102) trDsbl(i),prDsbl(i)  !Reducing parameters
        read (nread,*,err=102) ntrmb1(i),ntrmb2(i),ntrmb3(i)
        do k=1,ntrmb1(i)+ntrmb2(i)+ntrmb3(i)
          read (nread,*,err=102) psublk(k,i),psblex(k,i)
        enddo

c  Read surface tension parameters.
      elseif (iFlg.eq.7) then
        read (nread,*,err=102) ntermu(i)
        read (nread,*,err=102)       !Tc in original fit (not used)
        do k=1,ntermu(i)
          read (nread,*,err=102) sigmak(k,i),sigexp(k,i)
        enddo

c  Read dielectric constant parameters
      elseif (iFlg.eq.8) then
        read (nread,*,err=102) tredde(i),dredde(i),predde(i)
        read (nread,*,err=102) ntrmd1(i),ntrmd2(i),ntrmd3(i),ntrmd4(i)
        do k=1,ntrmd1(i)+ntrmd2(i)+ntrmd3(i)+ntrmd4(i)
          read (nread,*,err=102) decf(k,i),deexpt(k,i),
     &                           deexpd(k,i),deexpp(k,i)
        enddo
      endif
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                              subroutine SETANC
c
c ======================================================================
c
      subroutine ANCERR1 (icomp,T,hk,hancnme,ierr,herr)
c
c  Internal routine to the ancillary routines ending with the
c  letter 'P' or 'D'.  Input conditions are checked, and error
c  messages are returned if needed.
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      121 - Temperature greater than critical point temperature
c                      143 - Temperature less than zero
c                      501 - No equation available

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character hk*3,hancnme*(*),herr*255

      ierr=0
      herr=' '
      if (hk.eq.'NBS') then
        call ERRNUM (501,0,hancnme,' ',0d0,0d0,0d0,ierr,herr)
      elseif (T.gt.Tcrit(icomp)+1d-8) then
        ierr=121
        call ERRNUM (121,0,hancnme,' ',T,Tcrit(icomp),0d0,ierr,herr)
      endif
      if (T.le.0.) call ERRNUM (143,0,hancnme,' ',T,0d0,0d0,ierr,herr)

      end                                             subroutine ANCERR1
c
c ======================================================================
c
      subroutine ANCERR2 (icomp,b,janc,hk,hancnme,ierr,herr)
c
c  Internal routine to the ancillary routines ending with the
c  letter 'K'.  Input conditions are checked, and error messages
c  are returned if needed.
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                        2 - Density greater than triple point density
c                       -4 - Pressure above or below triple point pressure
c                      501 - No equation available

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character hk*3,hancnme*(*),herr*255

      ierr=0
      herr=' '

      o=1000d0
      if (icomp.ne.0) then
        if (hk.eq.'NBS') then
          call ERRNUM (501,0,hancnme,' ',0d0,0d0,0d0,ierr,herr)
        elseif (b.gt.Pcrit(icomp) .and. janc.le.2) then
        call ERRNUM (141,0,hancnme,' ',b/o,Pcrit(icomp)/o,0d0,ierr,herr)
        elseif (b.gt.Dtpn(icomp) .and. janc.ge.5) then
          call ERRNUM (2,4,hancnme,' ',b,Dtpn(icomp),0d0,
     &                 ierr,herr)
        elseif (b.lt.Ptpn(icomp) .and. janc.eq.3) then
          call ERRNUM (-4,2,'MELTP','below',b/o,Ptpn(icomp)/o,0d0,
     &                 ierr,herr)
        elseif (b.gt.Ptpn(icomp) .and. janc.eq.4) then
          call ERRNUM (-4,2,'SUBLP','above',b/o,Ptpn(icomp)/o,0d0,
     &                 ierr,herr)
        endif
      endif
      if (b.le.0.) call ERRNUM (143,0,hancnme,' ',b,0d0,0d0,ierr,herr)

      end                                             subroutine ANCERR2
c
c ======================================================================
c
      subroutine SURFT (T,Dl,z,sigma,ierr,herr)
c
c  Compute surface tension as a function of T.  SATT is called to obtain
c  the liquid density.  If this is already known then your calling
c  routines should use subroutine STN to greatly reduce the time
c  needed in the calculation of the surface tension.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition of liquid phase (array of mole fractions)
c
c  Outputs:
c       Dl--Molar density of liquid phase [mol/L] (only returned for mixtures)
c    sigma--Surface tension [N/m]
c     ierr--Error flag:  0 - Successful
c                        Other error messages returned from SATT or STN
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  03-24-96  MM, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr,herr1
      dimension z(ncmax),y(ncmax),xx(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SURFT

      ierr=0
      sigma=xnotc
      call ISPURE (z,icomp)

c  Because of model 'STH' in STN, it is necessary to provide Dv and y.
      if (icomp.eq.0) call SATT (T,z,-1,P,Dl,Dv,xx,y,ierr,herr)
      if (ierr.gt.0) then
        call SATGUESS (1,1,0,0,z,T,pp,Dl,h,s,Dv,y,ierr1,herr1)       !Use SATGUESS to approximate saturation line if SATT fails, but do not return this error message if it fails as well.
        if (Dl.lt.Dv) call SWAPV (Dl,Dv)
        if (ierr1.gt.0) RETURN
      endif

c  Calculate surface tension.
      call STN (T,Dl,Dv,z,y,sigma,ierr,herr)

      end                                               subroutine SURFT
c
c ======================================================================
c
      subroutine SURTEN (T,Dl,Dv,x,y,sigma,ierr,herr)
c
c  With version 10 of Refprop, this routine should no longer be used,
c  and STN or SURFT should be used instead.
c
c  (See subroutine STN for the description of all variables.)
c
      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),xx(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SURTEN

c  Calculate densities of saturated states, if required.
      sigma=xnotc
      if ((ixWat.gt.0 .or. ixAlc.gt.0) .and. ignbnd.eq.0) then
        if (nc.gt.2) goto 518
        if (fstkij(1,2,1).eq.0 .and. fstkij(2,1,1).eq.0) goto 518
      endif
      call ISPURE (x,icomp)
      if (icomp.eq.0 .and. (Dl.le.1d-20 .or. Dv.le.1d-20)) then
        call SATT (T,x,-1,P,Dl,Dv,xx,y,ierr,herr)
        if (ierr.gt.0) RETURN
      endif

c  Calculate surface tension.
      call STN (T,Dl,Dv,x,y,sigma,ierr,herr)
      RETURN

 518  call ERRNUM (518,0,'STN',' ',0d0,0d0,0d0,ierr,herr)

      end                                              subroutine SURTEN
c
c ======================================================================
c
      subroutine STN (T,Dl,Dv,x,y,sigma,ierr,herr)
c
c  Compute surface tension with appropriate core model. For mixtures, this
c  routine requires that the saturation densities and vapor compositions
c  be sent as inputs.  If these are not available, call SURFT.
c
c  The critical temperature used is that of the current equation of state.
c  This may differ slightly from that used in the original correlation of the
c  surface tension; this change is necessary to give proper behavior of surface
c  tension near the critical point and to avoid possible numerical crashes.
c
c  Inputs:
c        T--Temperature [K]
c       Dl--Molar density of liquid phase [mol/L]
c       Dv--Molar density of vapor phase [mol/L]
c        x--Composition of liquid phase (array of mole fractions)
c        y--Composition of vapor phase (array of mole fractions)
c
c  Outputs:
c    sigma--Surface tension [N/m]
c     ierr--Error flag:  0 - Successful
c                        1 - T > Tcrit
c                      502 - Unknown model
c                      151 - Failed to converge
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  03-27-96  MM, original version (skeleton only)
c  12-02-15 MLH, add parachor model for mixtures
c  05-10-16 EWL, shorten code
c  06-16-16 MLH, remove discontinuity for components near or above critical in a mix

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),parach(ncmax),cy(ncmax)
      dimension cx(ncmax)  !,zeta(ncmax),xcritf(ncmax),f(ncmax),zmol(ncmax)

      sigma=xnotc
      call ISPURE (x,icomp)
      call REDLINEAR (1,x,Tc,Pc,Dc,ierr,herr)
      call CRITP (x,tcritf,pcritf,Dcritf,ierr,herr)
      if (icomp.eq.0) Tc=Tc+1d0

c  Don't allow mixtures with water or alcohols unless an interaction parameter has been fitted.
      if ((ixWat.gt.0 .or. ixAlc.gt.0) .and. ignbnd.eq.0) then
        if (nc.gt.2) goto 518
        if (fstkij(1,2,1).eq.0 .and. fstkij(2,1,1).eq.0) goto 518
      endif

      if (T.gt.Tc) goto 121  !Check if T>Tc

c  Pure fluid calculation.
      if (icomp.ne.0) then
        tau=1d0-T/Tc
        call STNK (icomp,tau,sigma,ierr,herr)
        RETURN

c  Mixture calculation.

c  Parachor method of Weinaug C.F. and Kautz, D.L., Ind. Eng. Chem. 35, (1943) pp 239
c   see also Hugill, J.A. and Van Welsenes, A.J., Fluid Phase Equilib. 29, (1986) pp 383
      elseif (hstnmx.eq.'PAR') then
        const=1d0/3.87d0
        sigsum=0d0
        do j=1,nc                                          !Get pure component parachor at T
          z(1:nc)=0d0
          z(j)=1d0
          Tcal=T
          if (T.ge.0.9d0*Tcrit(j)) Tcal=0.9d0*Tcrit(j)     !If above 0.9*critical use tr=0.9.
          tau=1d0-Tcal/Tcrit(j)
          call STNK (j,tau,sigk,ierr,herr)                 !Get pure fluid surface tension.
          if (ierr.gt.0 .or. sigk.le.0) goto 151
          call SATT (Tcal,z,-1,pp,Dl1,Dv1,cx,cy,ierr,herr) !Get pure fluid densities.
          if (ierr.gt.0)
     &       call SATGUESS (-1,1,0,0,z,Tcal,pp,Dl1,h,s,Dv1,cy,ierr,herr)  !Get values from ancillaries when SATT fails.
          if (ABS(Dl1-Dv1).lt.1d-6) goto 151
          parach(j)=sigk**const/(Dl1-Dv1)
          sigsum=sigsum+x(j)*sigk                          !Used only for testing the output below.
        enddo

c  Preliminary method with one binary interaction parameter.
        sumparal=0d0
        sumparav=0d0
        do i=1,nc
          do j=1,nc
            if (i.eq.j) then
              parachij=(parach(i)+parach(j))/2d0
            else
              parachij=(1d0-fstkij(i,j,1))*(parach(i)+parach(j))/2d0
            endif
            sumparal=sumparal+x(i)*x(j)*parachij
            sumparav=sumparav+y(i)*y(j)*parachij
          enddo
        enddo
        s=sumparal*Dl-sumparav*Dv
        if (s.ge.0.) sigma=s**3.87d0
        if (sigma/sigsum.lt.1d-2) goto 151
        RETURN
      endif
      goto 502


!c  Older models that are no longer used unless SETMOD has been called.
!
!c  Base surface tensions on critical parameters at same composition.
!      tau=1d0-T/Tc
!      tau126=tau**1.26d0
!      alpha=0.1d0
!      const=3.74d0**1.5d0*SQRT(Rxgas)*alpha*(1d0-alpha)*(2d0-alpha)
!c  Define effective parameter in function sigma = sigma0*tau**1.26.
!      do i=1,nc
!        call STNK (i,tau,sigk,ierr,herr)
!        if (ierr.gt.0 .or. sigk.lt.0) RETURN
!        sig0=0d0
!        if (tau126.gt.0.) sig0=sigk/tau126
!        cx(i)=sig0**1.5d0/(const*SQRT(Tcrit(i))*Pcrit(i))
!      enddo
!
!c  Models STX and STM
!c  Apply mixing rules to cx(i), compute fugacities and fugacity fraction.
!      if (hstnmx.eq.'STX' .or. hstnmx.eq.'STM') then
!        call FGCTY2 (T,Dl,x,f,ierr,herr)
!        sumx=XDIV(1d0,SUM(f(1:nc)))
!        zeta(1:nc)=f(1:nc)*sumx
!        xcritf(1:nc)=x(1:nc)                                    !Initial guess for critical composition at same zeta.
!        call CRITF (zeta,xcritf,tcritf,pcritf,Dcritf,ierr,herr) !Find critical parameters at same fugacity fraction.
!        if (ierr.gt.0) RETURN                                   !Error in CRITF - solution not possible
!
!c  Model STH
!c  Apply Holcomb and Higashi modification of Moldover and Rainwater (i.e. apply mixing
!c   rules at overall mass composition corresponding to a liquid volume fraction of 0.5).
!c   (M.R. Moldover and J.C. Rainwater, J. Chem. Phys., 88:7772-7780, 1988.)
!      elseif (hstnmx.eq.'STH') then
!        zmol(1:nc)=(x(1:nc)*Dl+y(1:nc)*Dv)/(Dl+Dv)
!        call XMASS (zmol,zeta,xmw)
!        call CRITP (zmol,tcritf,pcritf,Dcritf,ierr,herr)
!
!c  Unknown model
!      else
!        sigma=-9.999d6
!        goto 502
!      endif
!
!c  Models STM and STH -- Moldover and Rainwater or Holcomb and Higashi model
!c   of M and R method.  The difference is in the zeta defined above.
!      cmix=0d0
!      if (hstnmx.eq.'STM' .or. hstnmx.eq.'STH') then
!        cmix=SUM(zeta(1:nc)*cx(1:nc))
!
!c  Model STX -- Holcomb's modification of Moldover and Rainwater method.
!      elseif (hstnmx.eq.'STX') then
!       pcsum=SUM(zeta(1:nc)**2*Pcrit(1:nc))
!       do i=1,nc
!        cmix=cmix+zeta(i)**2*cx(i)
!        if (i.lt.nc) then
!         do j=i+1,nc
!          cmix=cmix+(pcritf-pcsum)*SQRT(cx(i)*cx(j)/(Pcrit(i)*Pcrit(j)))
!         enddo
!        endif
!       enddo
!      endif
!
!c  Recover mixture sigma parameter from cmix.
!c  This expression is based on critical parameters at same zeta.
!      if (cmix.lt.0.) cmix=0d0
!      sig0=(const*SQRT(tcritf)*pcritf*cmix)**thrd2
!      sigma=sig0*tau**1.26d0
!      RETURN

 121  call ERRNUM (121,0,'STN',' ',T,Tc,0d0,ierr,herr)
      RETURN

 151  sigma=xnotc
      call ERRNUM (151,0,'STN',' ',0d0,0d0,0d0,ierr,herr)
      RETURN

 502  call ERRNUM (502,0,'STN',' ',0d0,0d0,0d0,ierr,herr)
      RETURN

 518  call ERRNUM (518,0,'STN',' ',0d0,0d0,0d0,ierr,herr)

      end                                                 subroutine STN
c
c ======================================================================
c
      subroutine STNK (icomp,tau,sigma,ierr,herr)
c
c  Compute surface tension with appropriate core model for a pure fluid.
c  This should not be called outside of the CORE_STN file; call SURFT
c  or SURTEN instead.
c
c  Inputs:
c    icomp--Component i
c      tau--Dimensionless temperature (1-T/Tc)
c
c  Outputs:
c    sigma--Surface tension [N/m]
c     ierr--Error flag:  0 - Successful
c                      501 - Equation not available
c                      502 - Unknown model
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  04-17-97  MM, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr

      ierr=0
      herr=' '
      sigma=xnotc
      if (hstnk(icomp).eq.'ST1') then
        sigma=0d0
        do k=1,ntermu(icomp)
          sigma=sigma+sigmak(k,icomp)*tau**sigexp(k,icomp)
        enddo
      elseif (hstnk(icomp).eq.'NBS') then
       call ERRNUM(501,0,'STNK',' ',0d0,0d0,0d0,ierr,herr)
      else
       call ERRNUM(502,0,'STNK',' ',0d0,0d0,0d0,ierr,herr)
      endif

      end                                                subroutine STNK
!c
!c ======================================================================
!c
!      subroutine CRITF (zeta,z,Tc,Pc,Dc,ierr,herr)
!c
!c  Compute critical parameters as a function of fugacity fraction.
!c
!c  Inputs:
!c     zeta--Fugacity fraction (array of f/f)
!c        z--Initial guess for composition (array of mole fractions)
!c
!c  Outputs:
!c        z--Composition (array of mole fractions)
!c       Tc--Critical temperature [K]
!c       Pc--Critical pressure [kPa]
!c       Dc--Critical density [mol/L]
!c     ierr--Error flag:  0 - Successful
!c                      517 - Did not converge
!c     herr--Error string (character*255)
!c
!c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
!c  04-21-97  MM, original version
!c  05-10-16 EWL, shorten code
!
!      include 'COMMONS.INC'
!      include 'COMANC.INC'
!      character*255 herr
!      dimension z(ncmax),zeta(ncmax),zetaj(ncmax),xnew(ncmax),f(ncmax)
!
!      itmax=20
!      tolr=1d-5
!
!      do it=1,itmax
!        call CRITP (z,Tc,Pc,Dc,ierr,herr)
!        call FGCTY2 (Tc,Dc,z,f,ierr,herr)
!        zetaj(1:nc)=f(1:nc)/xEq0(SUM(f(1:nc)))
!        xnew(1:nc)=z(1:nc)-(zetaj(1:nc)-zeta(1:nc))        !Simple successive substitution.
!        do i=1,nc
!          if (xnew(i).lt.0.) xnew(i)=0d0
!        enddo
!        delx=SUM(ABS(xnew(1:nc)-z(1:nc)))
!        if (delx.lt.tolr) RETURN            !Iteration converged.
!        s=SUM(xnew(1:nc))
!        if (lEQ0(s)) EXIT
!        z(1:nc)=xnew(1:nc)/s
!      enddo
!      call ERRNUM (517,0,' ',' ',0d0,0d0,0d0,ierr,herr)
!
!      end                                               subroutine CRITF
c
c ======================================================================
c
      subroutine DIELEC (T,D,z,de)
c
c  Compute dielectric constant as a function of temperature, density,
c  and composition.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c       de--Dielectric constant [-]
c
c  07-01-98 EWL, original version
c  06-13-16 EWL, shorten all code in CORE_DE.FOR

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DIELEC

      de=xnotc
      call ISPURE (z,icomp)

c  Pure fluid calculation
      if (icomp.ne.0) then
        call DEK (icomp,T,D,de,ierr,herr)

c  Mixture calculation (from Harvey and Lemmon, 2005)
      elseif (hdilmx.eq.'DEX' .or. hdilmx.eq.'DEM') then
        pm=0d0
        Vz=SUM(z(1:nc)*Vzred(1:nc))
        if (Vz.le.0.) RETURN
        Dr=D*Vz
        do i=1,nc
          call DEK (i,T,Dr*Dzred(i),dei,ierr,herr)
          if (ierr.gt.0 .or. dei.le.0.) RETURN
          P=(dei-1d0)*(2d0*dei+1d0)/9d0/dei
          pm=pm+z(i)*Vzred(i)/Vz*P
        enddo
        de=0.25d0*(1d0+9d0*pm+3d0*SQRT(9d0*pm**2+2d0*pm+1d0))
      endif

      end                                              subroutine DIELEC
c
c ======================================================================
c
      subroutine DEK (icomp,T,D,de,ierr,herr)
c
c  Compute dielectric constant with appropriate core model for a pure fluid.
c
c  Inputs:
c    icomp--Component number in mixture (1.nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c       de--Dielectric constant [-]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown dielectric model
c     herr--Error string (character*255)
c
c  04-17-97 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)

      de=xnotc
      ierr=0
      herr=' '
      i=icomp
      n=ntrmd1(i)

c  Return a value of 1 for the ideal gas.
      if (T.le.0 .or. D.lt.1d-12) then
        de=1d0

c  Simple model for the dielectric constant (not currently used).
c   The pressure is required to implement this model.
      elseif (hdilk(i).eq.'DE1') then
        del=D/dredde(i)
        tau=T/tredde(i)
        call PRESS (T,D,z,P)
        pi=P/1000d0/predde(i)
        if (pi.lt.0.) RETURN
        cm=SUM(decf(1:n,i)*tau**deexpt(1:n,i)*del**deexpd(1:n,i)*
     &          pi**deexpp(1:n,i))
        j=n+ntrmd2(i)
        cm=cm+SUM(decf(n+1:j,i)*tau**deexpt(n+1:j,i)*
     &     del**deexpd(n+1:j,i)*pi**deexpp(n+1:j,i)*XLOG(1d0+1d0/tau))
        de=(1d0+2d0*cm)/(1d0-cm)

c  Correlation of Fernandez et al. for water (see fluid file).
      elseif (hdilk(i).eq.'DE2') then
        del=D/dredde(i)
        an=6.0221367d23      !1/mol
        alpha=1.636d-40      !C^2.m^2/J
        xmu=6.138d-30        !C.m
        eps=8.854187817d-12  !C^2/J.m
        tau=tredde(i)/T
        g=1d0+SUM(decf(1:n,i)*tau**deexpt(1:n,i)*del**deexpd(1:n,i))
        do k=1,ntrmd2(i)
          j=k+n
          t1=T/deexpt(j,i)-1d0
          if (t1.le.0) RETURN
          g=g+decf(j,i)*del**deexpd(j,i)/t1**deexpp(j,i)
        enddo
        a=1000d0*an*D*g*xmu**2/(eps*xBoltz*T)    !Harris-Alder model
        b=1000d0*an*D*alpha/(3d0*eps)
        c=DSQRT(9d0+2d0*a+18d0*b+a**2+10d0*a*b+9d0*b**2)
        de=(1d0+a+5d0*b+c)/(4d0-4d0*b)


c  General form from Harvey and Lemmon that uses simple polynomials.
      elseif (hdilk(i).eq.'DE3' .or. hdilk(i).eq.'DE4') then
        del=D/dredde(i)
        tau=T/tredde(i)
        cm=SUM(decf(1:n,i)*tau**deexpt(1:n,i)*del**deexpd(1:n,i))
        do k=1,ntrmd2(i)
          j=k+n
          if (ABS(deexpt(j,i)).lt.1d-20) then
            cm=cm+decf(j,i)*del**deexpd(j,i)
          elseif (ABS(deexpt(j,i)-1d0).lt.1d-20) then
            cm=cm+decf(j,i)*(tau-1d0)*del**deexpd(j,i)
          else
            cm=cm+decf(j,i)*(tau-1d0)**deexpt(j,i)*del**deexpd(j,i)
          endif
        enddo
        do k=1,ntrmd3(i)
          j=k+n+ntrmd2(i)
          if (ABS(deexpt(j,i)).lt.1d-20) then
            cm=cm+decf(j,i)*del**deexpd(j,i)
          elseif (ABS(deexpt(j,i)-1d0).lt.1d-20) then
            cm=cm+decf(j,i)*(1d0/tau-1d0)*del**deexpd(j,i)
          else
           cm=cm+decf(j,i)*(1d0/tau-1d0)**deexpt(j,i)*del**deexpd(j,i)
          endif
        enddo
        if (hdilk(i).eq.'DE3') then
          de=(1d0+2d0*cm)/(1d0-cm)
        else
          de=0.25d0*(1d0+9d0*cm+3d0*SQRT(9d0*cm**2+2d0*cm+1d0))
        endif

c  Harvey and Mountain form for polar fluids, modified to include additional
c   parameters for future equations.
      elseif (hdilk(i).eq.'DE5') then
        del=D/dredde(i)
        tau=T/tredde(i)
        alpha=decf(1,i)
        xmu  =decf(2,i)
        cu   =decf(3,i)
        cg   =decf(4,i)
        n1=ntrmd2(i)+n
        n2=ntrmd3(i)+n1
        n3=ntrmd4(i)+n2
        f= 1d0-SUM( decf  ( n+1:n1,i)*del**deexpt( n+1:n1,i)*   !Density dependence for f.
     &         EXP(-deexpd( n+1:n1,i)*del**deexpp( n+1:n1,i)))
        g1=    SUM( decf  (n1+1:n2,i)*tau**deexpt(n1+1:n2,i)*   !Density dependence for f.
     &         EXP(-deexpd(n1+1:n2,i)*tau**deexpp(n1+1:n2,i)))
        g2=1d0-SUM( decf  (n2+1:n3,i)*del**deexpt(n2+1:n3,i)*   !Density dependence for f.
     &         EXP(-deexpd(n2+1:n3,i)*del**deexpp(n2+1:n3,i)))
        g=1d0+cg*g1*g2
        xmueff=xmu*(1d0+cu*f)
        Au=xAvog*(xmueff*3.335641d-30)**2/9d0/xPerm/xBoltz*1d6
        PK=D/1d3*(4d0*xPi*xAvog/3d0*alpha+g*Au/T)*predde(i)
        de=(1d0+9d0*PK+3d0*SQRT(9d0*PK**2+2d0*PK+1d0))/4d0

c  Unknown model
      else
        call ERRNUM (501,0,'DEK',' ',0d0,0d0,0d0,ierr,herr)
      endif

      end                                                 subroutine DEK

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                  End file CORE_ANC.FOR
c ======================================================================
