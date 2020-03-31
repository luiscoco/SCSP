c  Begin file MIX_HMX.FOR
c
c  This file contains the routines implementing the mixture Helmholtz
c    equation of state of Lemmon and Jacobsen and of Kunz and Wagner.
c
c  The second half of this file file contains routines implementing the
c    AGA 8 Part 1 equation of state for natural gas mixtures.
c
c  Contained here are:
c     function   PHIHMX  (itau,idel,tau,del,z)
c     function   PHIMIX  (icomp,jcomp,itau,idel,tau,delta,z)
c     subroutine CRTHMX  (z,Tc,Pc,Dc,ierr,herr)
c     subroutine RDXHMX  (ix,icmp,icmp2,z,Tred,Dred,ierr,herr)
c     subroutine SETHMX  (hFmix,ierr,herr)
c     subroutine RDBNC   (nread,ierr,herr)
c     subroutine RDMIX   (nread,ierr,herr)
c     subroutine GETMIXCF(ierr,herr)
c     subroutine ESTBNC  (i,j,ibin,ierr,herr)
c     subroutine HMXORDER(i,j,hIDi,hIDj,iFlag,ierr,herr)
c     subroutine INITMIX
c     subroutine SETKTV  (icomp,jcomp,hmodij,fij,hFmix,ierr,herr)
c     subroutine GETKTV  (icomp,jcomp,hmodij,fij,hFmix,hfij,hbinp,hmxrul)
c     subroutine GETFIJ  (hmodij,fij,hfij,hmxrul)
c
c     function PHIAGA    (itau,idel,tau,del,z)
c     subroutine SETAGA  (ierr,herr)
c     subroutine UNSETAGA
c
c ======================================================================
c ======================================================================
c
      function PHIHMX (itau,idel,tau,delta,z)
c
c  Compute reduced Helmholtz energy or its derivative as functions
c  of dimensionless temperature and density for the mixture Helmholtz
c  equation of state.
c
c  See notes in function PHIMIX.
c
c  Inputs:
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           when itau = 0 and idel = 0, compute A/RT
c           when itau = 0 and idel = 1, compute 1st density derivative
c           when itau = 1 and idel = 1, compute cross derivative
c           etc.
c      tau--Dimensionless temperature (Tr/T)
c    delta--Dimensionless density (D/Dr)
c        z--Composition array (mole fractions)
c
c  Output (as the function value):
c      phi--Residual (real-gas) part of the Helmholtz energy, or one
c           of its derivatives (as specified by itau and idel),
c           in reduced form (A/RT)

      include 'COMMONS.INC'
      dimension z(ncmax)

      PHIHMX=0d0
      if (delta.le.1d-20) RETURN       !Trivial solution at zero density
      if (tau.le.0.)      RETURN

      phisum=0d0
      do i=1,nc
        if (z(i).gt.1d-12)
     &  phisum=phisum+z(i)*PHIK(i,itau,idel,tau,delta)     !Calculate ideal solution term.
      enddo
      do i=1,nc-1
        if (z(i).gt.1d-12) then
        do j=i+1,nc
          if (z(j).gt.1d-12)
     &    phisum=phisum+PHIMIX(i,j,itau,idel,tau,delta,z)  !Calculate mixing term.
        enddo
        endif
      enddo
      PHIHMX=phisum

      end                                                function PHIHMX
c
c ======================================================================
c
      function PHIMIX (icomp,jcomp,itau,idel,tau,delta,z)
c
c  Compute reduced Helmholtz energy of mixing (or its derivatives)
c  for the binary interaction of components i and j as a function of
c  composition and dimensionless temperature and density for the
c  mixture Helmholtz equation of state.
c
c  The Helmholtz energy consists of ideal-gas and residual (real-gas) terms.
c  The residual term consists of ideal-solution and mixing terms.  This
c  routine calculates only the residual term.
c
c  Inputs:
c    icomp--Component i
c    jcomp--Component j
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           when itau = 0 and idel = 0, compute Amix/RT
c           when itau = 0 and idel = 1, compute 1st density derivative
c           when itau = 1 and idel = 1, compute cross derivative
c           etc.
c      tau--Dimensionless temperature (Tr/T)
c    delta--Dimensionless density (D/Dr)
c        z--Composition array (mole fractions)
c
c  Output (as the function value):
c      phi--Mixture interaction (excess) part of the Helmholtz energy,
c           or one of its derivatives (as specified by itau and idel),
c           in reduced form (Amix/RT)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  11-02-95  MM, original version
c  08-17-06 EWL, update program for GERG-2004 equation of Kunz and Wagner
c  10-19-06 EWL, change calculation of ncode2 to work with KW
c  04-16-10 EWL, increase nmsav from 25 to 55 to allow KWA, KWB, KWC, etc.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension z(ncmax),phix02(nmxtrm),phix20(nmxtrm),phix03(nmxtrm)
      dimension dd(0:20)
      del=delta

      i=icomp
      j=jcomp
      n=ncode2(i,j)
      PHIMIX=0d0
      phisum=0d0
      if (n.lt.0 .or. n.gt.nmsav) goto 500
      if (del.le.1d-20 .or. tau.le.0.) goto 500            !Trivial solution at zero density
      if (n.eq.0) goto 500  !Check if the model uses a departures function [only the reducing parameters (i.e., Kt and/or Kv) affect the properties].

      n1=namix(i,j)
      n2=n1+1
      n3=n1+namix2(i,j)
      n4=n3+1
      n5=n3+namix3(i,j)
      if (.not.lRcnst .or. .not.lRcnstWag) call RMIX (z)

c  Lemmon-Jacobsen, Kunz-Wagner, or Gaussian mixing functions.
      ncode=idel*4+itau+1
      ifij=3
      if (iModMx2(i,j).eq.i2MxKW .or. iModMx2(i,j).eq.i2MxB) ifij=5
      if (ABS(tau-tausvm(n)).lt.1d-12 .and.
     &    ABS(del-delsvm(n)).lt.1d-12) then
        if (hmixsav(n)(ncode:ncode).eq.'1') then      !Retrieve value from previous call.
          PHIMIX=z(i)*z(j)*fmix(ifij,i,j)*phisav(n,ncode)
          goto 500
        endif

c  Compute new values and save for possible future use
      else
        hmixsav(n)='0000000000000000'
        elntau=LOG(tau)*xLOG2x
        elndel=LOG(del)*xLOG2x
        dd(0)=0d0
        dd(1)=del
        do ii=2,idlmix(i,j)
          dd(ii)=dd(ii-1)*del     !Precalculate del, del^2, del^3, etc., up to the value of idlmix.
        enddo
        do k=1,n1  !Polynomial and simple exponential-polynomial terms
         dellim(k,n)=dd(INT(amix(k,4,i,j)))
c        dellim(k,n)=0d0
c        if (amix(k,4,i,j).gt.0d0)
c    &     dellim(k,n)=del**amix(k,4,i,j)   !Use this piece if the density exponents are not integers.
         ex=amix(k,2,i,j)*elntau+amix(k,3,i,j)*elndel-dellim(k,n)*xLOG2x
         if (ex.gt. 100.) ex= 100d0
         if (ex.lt.-100.) ex=-100d0
         phisvm(k,n)=amix(k,1,i,j)*2d0**ex
        enddo
        if (namix2(i,j).gt.0) then     !Kunz-Wagner terms
          do k=n2,n3
            ex=amix(k,2,i,j)*elntau+amix(k,3,i,j)*elndel-
     &         (amix(k,4,i,j)*(del-amix(k,5,i,j))**2+
     &          amix(k,6,i,j)*(del-amix(k,7,i,j)))*xLOG2x
            if (ex.gt. 100.) ex= 100d0
            if (ex.lt.-100.) ex=-100d0
            phisvm(k,n)=amix(k,1,i,j)*2d0**ex
          enddo
        endif
        if (namix3(i,j).gt.0) then     !Bell-shaped Gaussian terms
          do k=n4,n5
            ex=amix(k,2,i,j)*elntau+amix(k,3,i,j)*elndel+
     &        (amix(k,6,i,j)*(del-amix(k,9,i,j))**2+
     &         amix(k,7,i,j)*(tau-amix(k,8,i,j))**2)*xLOG2x
            if (ex.gt. 100.) ex= 100d0
            if (ex.lt.-100.) ex=-100d0
            phisvm(k,n)=amix(k,1,i,j)*2d0**ex
          enddo
        endif
        phisum=SUM(phisvm(1:n5,n))
        if (lCache1) tausvm(n)=tau
        if (lCache1) delsvm(n)=del
        phisav(n,1)=phisum
        hmixsav(n)(1:1)='1'
      endif

c  Check if derivatives are requested, calculations make use of fact
c   that terms in derivative summations are very similar to A/RT terms.
c_______________________________________________________________________

c  ----- 1st derivative of alpha with respect to density -----
c  _____ del * d(alpha) / d(del) _____

      if (idel.eq.1) then

      !-----Polynomial terms
        phix01(1:n1)=amix(1:n1,3,i,j)-amix(1:n1,4,i,j)*dellim(1:n1,n)

      !-----KW terms
        if (namix2(i,j).gt.0) then
          phix01(n2:n3)=amix(n2:n3,3,i,j)-del*amix(n2:n3,6,i,j)-
     &              2d0*amix(n2:n3,4,i,j)*del*(del-amix(n2:n3,5,i,j))
        endif

      !-----Gaussian terms
        if (namix3(i,j).gt.0) then
          phix01(n4:n5)=amix(n4:n5,3,i,j)+
     &              2d0*amix(n4:n5,6,i,j)*del*(del-amix(n4:n5,9,i,j))
        endif

        phisum=SUM(phisvm(1:n5,n)*phix01(1:n5))


c  ----- 2nd derivative of alpha with respect to density -----
c  _____ del^2 * d^2(alpha) / d(del)^2 _____
      elseif (idel.eq.2) then

      !-----Polynomial terms
        do k=1,n1
          di=amix(k,3,i,j)
          dell=amix(k,4,i,j)*dellim(k,n)
          phix02(k)=(di-dell)*(di-1d0-dell)-amix(k,4,i,j)*dell
        enddo

      !-----KW terms
        if (namix2(i,j).gt.0) then
          D2=del**2
          do k=n2,n3
            di=amix(k,3,i,j)
            alph=amix(k,4,i,j)
            dxpfx=di-del*(2d0*alph*(del-amix(k,5,i,j))+amix(k,6,i,j))
            phix02(k)=dxpfx**2-di-2d0*D2*alph
          enddo
        endif

      !-----Gaussian terms
        if (namix3(i,j).gt.0) then
          D2=del**2
          do k=n4,n5
            di=amix(k,3,i,j)
            alph=amix(k,6,i,j)
            dxpfx=di+2d0*alph*del*(del-amix(k,9,i,j))
            phix02(k)=dxpfx**2-di+2d0*D2*alph
          enddo
        endif

        phisum=SUM(phisvm(1:n5,n)*phix02(1:n5))


c  ----- 3rd derivative of alpha with respect to density -----
c  _____ del^3 * d^3(alpha) / d(del)^3 _____

      elseif (idel.eq.3) then

      !-----Polynomial terms
        do k=1,n1
          dli=amix(k,4,i,j)
          dell=dli*dellim(k,n)
          di2=amix(k,3,i,j)-dell
          phix03(k)=di2*(di2-1d0)*(di2-2d0)-dell*(3d0*di2-3d0+dli)*dli
        enddo

      !-----KW terms
        if (namix2(i,j).gt.0) then
          D2=del**2
          do k=n2,n3
            di=amix(k,3,i,j)
            a4=amix(k,4,i,j)
            dii=di-del*(2d0*a4*(del-amix(k,5,i,j))+amix(k,6,i,j))
            phix03(k)=dii**3+2d0*di-3d0*dii*(di+2d0*a4*D2)
          enddo
        endif

      !-----Gaussian terms
        if (namix3(i,j).gt.0) then
          D2=del**2
          do k=n4,n5
            di=amix(k,3,i,j)
            dii=di+2d0*del*amix(k,6,i,j)*(del-amix(k,9,i,j))
            phix03(k)=dii**3+2d0*di-3d0*dii*(di-2d0*amix(k,6,i,j)*D2)
          enddo
        endif

        phisum=SUM(phisvm(1:n5,n)*phix03(1:n5))
      endif

c_______________________________________________________________________

c  ----- 1st derivative of alpha with respect to temperature -----
c  _____ tau * d(alpha) / d(tau) _____

      if (itau.eq.1) then

      !-----Polynomial and KW terms
        phix10(1:n3)=amix(1:n3,2,i,j)

      !-----Gaussian terms
        if (namix3(i,j).gt.0)
     &    phix10(n4:n5)=amix(n4:n5,2,i,j)+
     &         2d0*amix(n4:n5,7,i,j)*tau*(tau-amix(n4:n5,8,i,j))

        phisum=SUM(phisvm(1:n5,n)*phix10(1:n5))


c  ----- 2nd derivative of alpha with respect to temperature -----
c  _____ tau^2 * d^2(alpha) / d(tau)^2 _____

      elseif (itau.eq.2) then
        T2=tau**2

      !-----Polynomial and KW terms
        phix20(1:n3)=amix(1:n3,2,i,j)*(amix(1:n3,2,i,j)-1d0)

      !-----Gaussian terms
        if (namix3(i,j).gt.0) then
          do k=n4,n5
            ti=amix(k,2,i,j)
            beta=amix(k,7,i,j)
            axpfx=ti+2d0*beta*tau*(tau-amix(k,8,i,j))
            phix20(k)=axpfx**2-ti+2d0*T2*beta
          enddo
        endif
        phisum=SUM(phisvm(1:n5,n)*phix20(1:n5))


c  ----- 3rd derivative of alpha with respect to temperature -----
c  _____ tau^3 * d^3(alpha) / d(tau)^3 _____

      elseif (itau.eq.3) then

      !-----Polynomial and KW terms
        phisum=SUM(phisvm(1:n3,n)*amix(1:n3,2,i,j)*
     &         (amix(1:n3,2,i,j)-1d0)*(amix(1:n3,2,i,j)-2d0))

      !-----Gaussian terms
        if (namix3(i,j).gt.0) then
          T2=tau**2
          do k=n4,n5
            ti=amix(k,2,i,j)
            tii=ti+2d0*tau*amix(k,7,i,j)*(tau-amix(k,8,i,j))
            phisum=phisum+phisvm(k,n)*(tii**3+2d0*ti-
     &             3d0*tii*(ti-2d0*amix(k,7,i,j)*T2))
          enddo
        endif
      endif


c  ----- Partial derivatives with respect to both temperature and density-----
c  _____ del*tau * d^2(alpha) / [d(tau) d(del)] _____

      if (itau.eq.1 .and. idel.eq.1) then
        phisum=SUM(phisvm(1:n5,n)*phix10(1:n5)*phix01(1:n5))

c  _____ del*tau^2 * d^3(alpha) / [d(tau)^2 d(del)] _____
      elseif (itau.eq.2 .and. idel.eq.1) then
        phisum=SUM(phisvm(1:n5,n)*phix20(1:n5)*phix01(1:n5))

c  _____ del^2*tau * d^3(alpha) / [d(tau) d(del)^2] _____
      elseif (itau.eq.1 .and. idel.eq.2) then
        phisum=SUM(phisvm(1:n5,n)*phix10(1:n5)*phix02(1:n5))

c  _____ del^2*tau^2 * d^4(alpha) / [d(tau)^2 d(del)^2] _____
      elseif (itau.eq.2 .and. idel.eq.2) then
        phisum=SUM(phisvm(1:n5,n)*phix20(1:n5)*phix02(1:n5))
      endif


c  Store derivatives for later use.
      phisav(n,ncode)=phisum
      hmixsav(n)(ncode:ncode)='1'
      PHIMIX=z(i)*z(j)*fmix(ifij,i,j)*phisum     !fmix(3,i,j) is Fij for LJ.  For KW, fmix(5,i,j) is Fij.

c  Ammonia-water model of Tillner-Roth.
 400  if (iModMx2(i,j).eq.iMxTr) then
         psm=0d0
         m=itau
         n=idel
         do k=1,14
           tk=amix(k,2,i,j)
           dk=amix(k,3,i,j)
           pk=amix(k,4,i,j)
           xk=1d0
           if (k.ge.7 .and. iAmWat.gt.0)  xk=z(iAmWat)
           if (k.eq.14) xk=xk**2
           delik=-del**pk
           bk=1d0
           if (pk.gt.0.) bk=EXP1(delik)
           aka=xk*amix(k,1,i,j)*tau**tk*del**dk*bk
           if (m.eq.0 .and. n.eq.0) psm=psm+aka                      !No derivative
           if (m.eq.0 .and. n.eq.1) psm=psm+aka*(dk+pk*delik)        !1st derivative of del
           if (m.eq.1 .and. n.eq.0) psm=psm+tk*aka                   !1st derivative of tau
           if (m.eq.2 .and. n.eq.0) psm=psm+tk*(tk-1d0)*aka          !2nd derivative of tau
           if (m.eq.3 .and. n.eq.0) psm=psm+tk*(tk-1d0)*(tk-2d0)*aka !3rd derivative of tau
           if (m.eq.1 .and. n.eq.1) psm=psm+aka*tk*(dk+pk*delik)     !Derivative of tau and del
           if (m.eq.0 .and. n.eq.2) psm=psm+aka*(dk*(dk-1d0)+        !2nd derivative of del
     &              pk*delik*(pk-1d0+2d0*dk+pk*delik))
           if (m.eq.0 .and. n.eq.3) then
             dki=pk*delik
             psm=psm+(dk*(dk-1d0)*(dk-2d0)+                          !3rd deriv of del
     &          dki*(3d0*dk*((dk-2d0)+pk)+(pk-1d0)*(pk-2d0)+
     &          3d0*dki*(dk+(pk-1d0))+dki*dki))*aka
           endif
         enddo
         PHIMIX=psm*z(iAmWat)*(1d0-z(iAmWat)**fmix(5,i,j))
      endif
 500  RETURN

      end                                                function PHIMIX
c
c ======================================================================
c
      subroutine CRTHMX (z,Tc,Pc,Dc,ierr,herr)
c
c  Returns critical parameters associated with the mixture Helmholtz EOS.
c  The critical parameters are estimates based on polynomial
c  fits to the binary critical lines.  For 3 or more components,
c  combining rules are applied to the constituent binaries.
c
c  Binary pairs labeled as TC3 and VC3 in the HMX.BNC file indicate
c  that the critical line is not continuous across the composition axis.
c  These mixtures are generally Type III (or other types except Type I).
c  An error message will be returned in this situation to report that
c  the calculated values are not estimates of the critical point.
c
c  If SATSPLN has been called and the input composition sent here is the
c  same as that sent to SATSPLN, the values calculated from the splines
c  are returned, which are nearly exact.  During the call to SATSPLN,
c  the true critical point, maximum pressure point, and maximum
c  temperature point along the saturation lines are determined.
c  Without the splines and for a system with three or more components,
c  or TC3 and VC3 inputs, the values from this routine are only rough
c  estimates.
c
c  Inputs:
c        z--Composition array (mole fractions)
c     ierr--Can be set to -999999 to disable call to the spline values, but
c           must be sent as a variable since ierr is changed in this routine.
c
c  Outputs:
c       Tc--Critical temperature [K]
c       Pc--Critical pressure [kPa]
c       Dc--Molar density [mol/L] at critical point
c     ierr--Error flag:  0 - Successful
c                     -311 - Input x is not equal to that used to call SATSPLN.
c                            Estimated values from the HMX.BNC coefficients used instead.
c                      312 - sum(z)=0
c                      805 - sum(z)<>1
c                      313 - Coefficients are not available for critical line calculations
c                            (which generally occurs for binary mixtures that are not in
c                            the HMX.BNC, or for beta versions of the program where the
c                            critical lines have not yet been fitted).
c                      314 - Spline curves are available, but no solution was found.
c                      315 - Critical point is unknown (generally indicative of
c                            a Type III mixture).  The maximum temperature along
c                            the saturation lines was returned.
c                     -316 - The true critical point is unknown.  Estimated values based
c                            on previous fits of the critical lines (with the coefficients
c                            given in the HMX.BNC file) are returned.  For a Type I binary
c                            mixture, the values are generally close.  For a multi-component
c                            Type I mixture the values are very rough estimates.
c                      317 - Mixture appears to be Type III because the critical point,
c                            maximum temperature, and maximum pressure were not found, but
c                            the full spline curve was calculated.  Large values were
c                            returned for the critical point.
c                      318 - Mixture is most likely Type III as indicated by previous fits
c                            of the critical line.  Call the subroutine SATSPLN to determine
c                            the critical point before calling CRITP.  Linear mixing rules were
c                            used to calculate the critical point returned here.
c                     -319 - Values come from the *.mix file.
c
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  11-02-95  MM, original version
c  08-17-06 EWL, update program for GERG-2004 equation of Kunz and Wagner
c  08-21-06 EWL, if TC1 or VC1 are not available, return reducing parameters instead of an error message
c  02-13-13 EWL, return values from spline curves if available
c  03-03-15 EWL, add ierr=-999999 check
c  03-09-16 EWL, return the maxT value if no critical point

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*255 herr
      dimension z(ncmax)

      iErrNum=iErrNum+1
      ierrinp=ierr
      ierr=0
      herr=' '

c  Calculate the critical point for pure fluids.
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        Tc=Tcrit(icomp)
        Dc=Dcrit(icomp)
        Pc=Pcrit(icomp)
        goto 500
      endif

c  Calculate the critical point for mixtures.
      zsum2=SUM(z(1:nc))
      if (zsum2.lt.1d-12) goto 312
      call REDLINEAR (3,z,Tc,Pc,Dc,ierr1,herr)             !Calculate critical point values based on linear mixing rules for Tc and Vc.
      if (iSpline.ne.0 .and. ierrinp.ne.-999999) then      !Check if splines have been calculated.
        if (ierrCrt.ne.0) then
          if (ierrPmx.gt.0 .and. ierrTmx.eq.0) goto 315    !Only maximum temperature known.
          if (ierrPmx.gt.0 .and. narr.gt.50)   goto 317    !Possible Type III mixture.
          if (ierr.gt.0) goto 314
        endif
        zSum=SUM(ABS(z(1:nc)-zSpln(1:nc)))
        if (.not.lEQ0(zSum)) goto 311                      !Check for z equal to composition used when SATSPLN was called.
        Tc=crtT
        Pc=crtP
        Dc=crtD
        goto 500
      endif

 10   if (ncPDmix.gt.0) then
        diff=SUM(ABS(z(1:nc)-zPDmix(1:nc)))
        if (lEQ0(diff)) then
          Tc=PDmixTc
          Pc=PDmixPc
          Dc=PDmixDc
          goto 319
        endif
      endif
      tsum=0d0
      vsum=0d0
      zsum=0d0
c  Use the Tcij and Vcij binary critical line correlations to get the critical point.
c   For multi-component mixtures, these terms are evaluated at the pseudo-binary compositions.
      do i=1,nc-1
      if (z(i).gt.0.) then
        do j=i+1,nc
        if (z(j).gt.0.) then
          iii=i
          jjj=j
          if (iBckwrd(i,j).eq.1) then
            iii=j
            jjj=i
          endif
          if (imodTc(i,j).eq.0) goto 313         !Check that the binary mixture has a TC1, TC2, etc. line.
          if (lEQ0(ftcij(1,i,j))) then           !Check if coefficients are all zero (by just checking the first Tc value).
            if (imodTc(i,j).eq.iTc7) goto 318    !Tc7 usually indicates a Type III mixture.
            goto 313                             !Return error that coefficients have not yet been fitted.
          endif
          xi=(z(iii)+(1d0-z(jjj)))/2d0
          xj=(z(jjj)+(1d0-z(iii)))/2d0
          xij=xi*xj
          tcij=xi*Tcrit(iii)+xj*Tcrit(jjj)+xij*ftcij(1,i,j)+         !Calculate pseudo-critical temperature from parameters in the HMX.BNC.
     &          xj*xi*(ftcij(2,i,j)+(ftcij(3,i,j)+(ftcij(4,i,j)+
     &          (ftcij(5,i,j)+ftcij(6,i,j)*xi)*xi)*xi)*xi)*xi
          Vcij=xi/Dcrit(iii)+xj/Dcrit(jjj)+xij*fvcij(1,i,j)+         !Calculate pseudo-critical volume.
     &          xj*xi*((fvcij(2,i,j)+(fvcij(3,i,j)+(fvcij(4,i,j)+
     &          (fvcij(5,i,j)+fvcij(6,i,j)*xi)*xi)*xi)*xi)*xi)
          tsum=tsum+z(i)*z(j)*tcij
          vsum=vsum+z(i)*z(j)*Vcij
          zsum=zsum+z(i)*z(j)
        endif
        enddo
      endif
      enddo

      if (vsum.lt.1d-12) vsum=SUM(z(1:nc)/Dcrit(1:nc))
      if (zsum.lt.1d-12) zsum=1d0
      Tc=tsum/zsum
      Dc=zsum/vsum
      if (ABS(zsum2-1d0).gt.1.d-4) goto 805
      if (ierr.eq.0) goto 316   !Always return a warning when using TC1 and VC1 for the critical point calculations.

c  Base Pc on Tc and Dc.
 100  call PRESS (Tc,Dc,z,Pc)
      if (Pc.lt.0.) Pc=ABS(SUM(z(1:nc)*Pcrit(1:nc)))

 500  iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      RETURN

 315  Tc=TmxT
      Pc=TmxP
      Dc=Tmxd
      call ERRNUM ( 315,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 317  Tc=1000d0          !Set to large values for type III mixtures.
      Pc=100000d0
      Dc=1d0
      call ERRNUM ( 317,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 311  call ERRNUM ( 311,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 10
 312  call ERRNUM ( 312,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 100
 313  call ERRNUM ( 313,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 100
 314  call ERRNUM ( 314,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 10
 316  call ERRNUM (-316,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 100
 318  call ERRNUM ( 318,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 100
 319  call ERRNUM (-319,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 805  call ERRNUM ( 805,0,'CRTHMX',' ',0d0,0d0,0d0,ierr,herr)
      goto 100

      end                                              subroutine CRTHMX
c
c ======================================================================
c
      subroutine RDXHMX (ix,icmp,icmp2,z,Tred,Dred,ierr,herr)
c
c  Returns reducing parameters and their derivatives associated with
c  the mixture Helmholtz EOS; these are used to calculate the 'tau' and 'del'
c  that are the independent variables in the EOS.
c
c  Inputs:
c     icmp--Component number for which derivative will be calculated
c    icmp2--Second component number for which derivative will be calculated
c        z--Composition array (array of mole fractions)
c       ix--Flag specifying the order of the composition derivative to calculate,
c           when ix = 0, compute T(red) and D(red)
c         for icmp2=0
c           when ix = 1, compute 1st derivative with respect to z(icmp) or z(icmp2)
c           when ix = 2, compute 2nd derivative with respect to z(icmp) or z(icmp2)
c         for icmp<>0 and icmp2<>0
c           when ix = 11, compute cross derivative with respect
c                         to z(icmp) and z(icmp2)
c
c  Outputs:
c     Tred--Reducing temperature [K] or derivative
c     Dred--Reducing molar density [mol/L] or derivative of reducing volume [L/mol]
c           (ix=0 - Dc; ix=1 - dVc/dxi; ix=2 - d^2Vc/dxi^2; ix=11 - d^2Vc/dxidxj)
c     ierr--Error flag:  0 - Successful
c                      301 - Mixing rule not found for i,j
c                      191 - Derivative not available
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  11-02-95  MM, original version
c  08-17-06 EWL, update program for GERG-2004 equation of Kunz and Wagner
c  08-21-06 EWL, add Kunz and Wagner mixing rules
c  01-26-10 EWL, rename subroutine
c  01-26-10 EWL, add derivatives with respect to composition

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::RDXHMX

      ierr=0
      herr=' '
      fv=1d0
      ft=1d0
      xij=0d0
      tsum=0d0
      vsum=0d0
      vsum0=0d0
      vsumx=0d0
      vsumi=0d0
      vsumj=0d0
      onethrd=-thrd
      lNotPR=.true.
      if (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) lNotPr=.false.

      if (ix.le.0) then
        do i=1,nc
          if (z(i).gt.0.) then
          do j=i,nc
            if (z(j).gt.0.) then
c
c-----------------------------------------------------------------------
c  These are the full Kunz-Wagner relations, but should only be used
c    for numerical testing of analytical derivatives.  If used in combination
c    with other mixing rules (e.g., LJ6 or TR1), the computations will not
c    be correct.  These equations work when sum(z(k))<>0, as is the case
c    when taking a numerical derivative of a property with respect to z(i)
c    holding all other compositions constant.  This should only be called
c    for a binary mixture.
            if (ix.le.-100) then
              ixx=ix+100
              if (iModMx(1,nc).eq.iMxKW .or.
     &            iModMx(1,nc).eq.iMxXR .or.
     &            iModMx(1,nc).eq.iMxB) then
                if (i.eq.j) then
                  ftij=z(i)**2
                  fvij=ftij
                  ctij=Tc12ij(i,j)
                  cvij=Vc12ij(i,j)
                else
                  xij=z(i)*z(j)*(z(i)+z(j))
                  ft=fmix(1,i,j)**2*z(i)+fmix2(1,i,j)**2*z(j)
                  fv=fmix(3,i,j)**2*z(i)+fmix2(3,i,j)**2*z(j)
                  ftij=xij/ft
                  fvij=xij/fv
                  ctij=cT12ij(i,j)
                  cvij=cV12ij(i,j)
                endif

                if (ixx.lt.0) then
                  vsum0=vsum0+cvij*fvij
                  ftij=0d0
                  fvij=0d0
                  if (ixx.eq.-11) then      !Compute 2nd cross derivative
                    if (icmp.eq.i .and. icmp2.eq.j) then
                      dxi=(2d0*z(i)*z(j)+z(j)**2)
                      dxj=(2d0*z(i)*z(j)+z(i)**2)
                      ddxij=2d0*(z(i)+z(j))
                      dfi=(fmix (1,i,j)/ft)**2
                      dfj=(fmix2(1,i,j)/ft)**2
                      ftij=ddxij/ft-dxi*dfj-dxj*dfi+2d0*xij*ft*dfi*dfj
                      dfi=(fmix (3,i,j)/fv)**2
                      dfj=(fmix2(3,i,j)/fv)**2
                      fvij=ddxij/fv-dxi*dfj-dxj*dfi+2d0*xij*fv*dfi*dfj
                    endif
                    if (i.eq.j) then
                      if (i.eq.icmp ) vsumi=vsumi+cvij*2d0*z(i)
                      if (j.eq.icmp2) vsumj=vsumj+cvij*2d0*z(j)
                    else
                      dxij=2d0*z(i)*z(j)
                      fviji=(dxij+z(j)**2-xij*fmix(3,i,j)**2/fv)/fv
                      fvijj=(dxij+z(i)**2-xij*fmix2(3,i,j)**2/fv)/fv
                      vsumi=vsumi+cvij*fviji
                      vsumj=vsumj+cvij*fvijj
                    endif

                  elseif (i.eq.icmp .or. j.eq.icmp2 .or.
     &                    j.eq.icmp .or. i.eq.icmp2) then
                    if (i.eq.j) then
                      if (ixx.eq.-1) ftij=2d0*z(i)
                      if (ixx.eq.-2) ftij=2d0
                      fvij=ftij
                      fvijx=2d0*z(i)
                    else
                      k=j
                      df =fmix(1,i,j)
                      dfv=fmix(3,i,j)
                      if (j.eq.icmp .or. j.eq.icmp2) then
                        k=i
                        df =fmix2(1,i,j)
                        dfv=fmix2(3,i,j)
                      endif
                      df=df**2/ft
                      dxij=2d0*z(i)*z(j)+z(k)**2
                      if (ixx.eq.-1)ftij=(dxij-xij*df)/ft
                      if (ixx.eq.-2)ftij=2d0*(z(k)-dxij*df+xij*df**2)/ft
                      df=dfv**2/fv
                      fvijx=(dxij-xij*df)/fv
                      if (ixx.eq.-1)fvij=fvijx
                      if (ixx.eq.-2)fvij=2d0*(z(k)-dxij*df+xij*df**2)/fv
                    endif
                    vsumx=vsumx+cvij*fvijx
                  endif
                endif
                tsum=tsum+ctij*ftij
                vsum=vsum+cvij*fvij
c
c  Original Lemmon-Jacobsen rule with zeta and xi.
              elseif (iModMx(1,nc).eq.iMxLJ) then
                if (i.eq.j) then
                  tsum=tsum+z(i)*Tzred(i)
                  vsum=vsum+z(i)*Vzred(i)
                else
                  xij=z(i)*z(j)
                  xijt=z(i)**fmix(4,i,j)*z(j)**fmix(5,i,j)
                  tsum=tsum+xijt*fmix(1,i,j)
                  vsum=vsum+xij*fmix(2,i,j)
                endif
              endif
c-----------------------------------------------------------------------

c  Special case when i = j.
            elseif (i.eq.j) then
              tsum=tsum+z(i)*Tzred(i)
              vsum=vsum+z(i)*Vzred(i)

c  KW:  Kunz-Wagner rule.
c  XR:  Mixing rules only.
c  B:   Binary mixture model with Bell-shaped Gaussian terms.
            elseif (lNotPr .and. (iModMx(i,j).eq.iMxKW .or.
     &                            iModMx(i,j).eq.iMxXR .or.
     &                            iModMx(i,j).eq.iMxB)) then
              xij=z(i)*z(j)
              xijij=xij*(z(i)+z(j))
              ftij=xijij/(f2mix(1,i,j)*z(i)+f2mix2(1,i,j)*z(j))
              fvij=xijij/(f2mix(3,i,j)*z(i)+f2mix2(3,i,j)*z(j))
              tsum=tsum+cT12ij(i,j)*ftij
              vsum=vsum+cV12ij(i,j)*fvij

c  The following addition allows the use of linear mixture rules z(i)*Tc(i)
c   when the specified rule (as in this case for the GERG-2004 equation) is
c   not linear - z(i)^2*Tc(i).
              tsum=tsum-xij*(Tzred(i)+Tzred(j))
              vsum=vsum-xij*(Vzred(i)+Vzred(j))

c  Original Lemmon-Jacobsen rule with zeta and xi.
            elseif (iModMx(i,j).eq.iMxLJ .and. lNotPr)then
              xij=z(i)*z(j)
              xijt=z(i)**fmix(4,i,j)*z(j)**fmix(5,i,j)     !fmix(5,i,j) = gamma
              tsum=tsum+xijt*fmix(1,i,j)                   !fmix(1,i,j) = zeta(T)
              vsum=vsum+xij*fmix(2,i,j)                    !fmix(2,i,j) = xi  (z)

c  Mixing parameters for the Ammonia-Water Tillner-Roth model.
            elseif (iModMx(i,j).eq.iMxTr .and. lNotPr) then
              ij=i
              if (iAmWat.eq.2) ij=j
              tc12=Tzred(i)+Tzred(j)
              vc12=Vzred(i)+Vzred(j)
              tsum=tsum+z(ij)*(1d0-z(ij)**fmix(3,i,j))*
     &                  fmix(1,i,j)*tc12-z(i)*z(j)*tc12
              vsum=vsum+z(ij)*(1d0-z(ij)**fmix(4,i,j))*
     &                  fmix(2,i,j)*vc12-z(i)*z(j)*vc12

c  Simple linear mixing rules.
            elseif (iModMx(i,j).eq.iMxLIN) then            !Not used in HMX.BNC
              xij=z(i)*z(j)
              tc12=(Tzred(i)+Tzred(j))
              vc12=(Vzred(i)+Vzred(j))
              tsum=tsum+xij*(fmix(1,i,j)-1d0)*tc12         !fmix(1,i,j) = Kt(i,j)
              vsum=vsum+xij*(fmix(2,i,j)-1d0)*vc12         !fmix(2,i,j) = Kv(i,j)

c  Lemmon-Jacobsen rule modified to use Kt, Kv, rather than zeta, xi.
            elseif (iModMx(i,j).eq.i2MxLM) then           !Obsolete (from version 6)
              xij=z(i)*z(j)
              xijt=z(i)**fmix(4,i,j)*z(j)**fmix(5,i,j)     !fmix(4,i,j) = beta
              tc12=(Tzred(i)+Tzred(j))                     !fmix(5,i,j) = gamma
              vc12=(Vzred(i)+Vzred(j))
              tsum=tsum+xijt*(fmix(1,i,j)-1d0)*tc12        !fmix(1,i,j) = Kt(i,j)
              vsum=vsum+xij*(fmix(2,i,j)-1d0)*vc12         !fmix(2,i,j) = Kv(i,j)

c  Mixing rule not specified or not found - should not get here.
            else
              call ERRNUM (301,0,'RDXHMX',' ',0d0,0d0,0d0,ierr,herr)
            endif
            endif
          enddo
          endif
        enddo
        Tred=tsum
        Dred=XDIV(1d0,vsum)
        if (ix.lt.-100 .and. ABS(vsum0).gt.1.d-10) then
          Dred=-vsum/vsum0**2
          if (ix.eq.-102) Dred=Dred+2d0*vsumx**2   /vsum0**3
          if (ix.eq.-111) Dred=Dred+2d0*vsumi*vsumj/vsum0**3
        endif

c-----------------------------------------------------------------------
c  Calculate derivatives of Tc and Vc.
      else

        if (.not.lNotPr) goto 191
        if (z(icmp).gt.0.) then
        do k=1,nc
          if (k.le.icmp) then
            k1=k
            k2=icmp
          else
            k2=k
            k1=icmp
          endif
          if (z(k).gt.0.) then
          if (k.eq.icmp .and. ix.eq.1) then
            tsum=tsum+2d0*z(icmp)*Tzred(icmp)
            vsum=vsum+2d0*z(icmp)*Vzred(icmp)
          elseif (k.eq.icmp .and. ix.eq.2) then
            tsum=tsum+2d0*Tzred(icmp)
            vsum=vsum+2d0*Vzred(icmp)
          elseif (k.eq.icmp .and. ix.eq.11) then

c  Mixing rules for Kunz-Wagner or Bell-shaped Gaussian terms.
          elseif (iModMx(k1,k2).eq.iMxKW .or.
     &            iModMx(k1,k2).eq.iMxXR .or.
     &            iModMx(k1,k2).eq.iMxB) then
            if (z(k).gt.1.d-20) then
              xij=z(k)+z(icmp)
              f11=f2mix (1,k1,k2)
              f13=f2mix (3,k1,k2)
              f21=f2mix2(1,k1,k2)
              f23=f2mix2(3,k1,k2)
              if (k.lt.icmp) then
                ftij=1d0/(f11*z(k)+f21*z(icmp))
                fvij=1d0/(f13*z(k)+f23*z(icmp))
                st=f21*ftij
                sv=f23*fvij
              else
                ftij=1d0/(f21*z(k)+f11*z(icmp))
                fvij=1d0/(f23*z(k)+f13*z(icmp))
                st=f11*ftij
                sv=f13*fvij
              endif
              if (ix.eq.1) then
                ftij=z(k)*ftij*(xij+z(icmp)*(1d0-st*xij))
                fvij=z(k)*fvij*(xij+z(icmp)*(1d0-sv*xij))
              elseif (ix.eq.2) then
                ftij=z(k)*ftij*(1d0-st*xij)*(2d0-z(icmp)*2d0*st)
                fvij=z(k)*fvij*(1d0-sv*xij)*(2d0-z(icmp)*2d0*sv)
              elseif (ix.eq.11) then
                if (k.lt.icmp) then
                  ut=f11*ftij
                  uv=f13*fvij
                else
                  ut=f21*ftij
                  uv=f23*fvij
                endif
                if (k.eq.icmp2) then
                  ftij=(xij+z(k)*(1d0-ut*xij)+z(icmp)*(1d0-st*xij)-
     &                 z(icmp)*z(k)*(st+ut-2d0*st*ut*xij))*ftij
                  fvij=(xij+z(k)*(1d0-uv*xij)+z(icmp)*(1d0-sv*xij)-
     &                 z(icmp)*z(k)*(sv+uv-2d0*sv*uv*xij))*fvij
                endif
              endif
              if (ix.ne.11 .or. (ix.eq.11 .and. k.eq.icmp2)) then
                tsum=tsum+cT12ij(k1,k2)*ftij
                vsum=vsum+cV12ij(k1,k2)*fvij
              endif
            endif

c  Lemmon-Jacobsen mixing rules (not working due to linear vs. quadratic terms for i=j).
          elseif (iModMx(k1,k2).eq.iMxLJ) then
            goto 191

c  All others.
          elseif (iModMx(k1,k2).ne.iMxiEQj) then
            goto 191
          endif
          endif
        enddo
        endif
        Tred=tsum
        Dred=vsum
      endif

      if (Tred.le.0.) Tred=300d0
      RETURN

 191  call ERRNUM (191,0,'RDXHMX',' ',0d0,0d0,0d0,ierr,herr)    !Don't change error number, used elsewhere.
      end                                              subroutine RDXHMX
c
c ======================================================================
c
      subroutine SETHMX (hFmix,ierr,herr)
c
c  Read parameters for the HMX mixture model from the HMX.BNC file.
c
c  Input:
c    hFmix--File name containing mixture parameters (character*255)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      110 - Wrong components; model TR1 valid for ammonia + water only
c                      111 - Error in opening mixture file
c                      112 - Mixture file does not match model
c                      117 - Estimation not possible for input pair
c                     -117 - Binary pair not found, all parameters will be estimated
c     herr--Error string (character*255)
c     (fluid parameters, etc. returned via various common blocks)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-09-96  MM, original version
c  09-28-05  DT, change occurrences of a backslash to 'CHAR(92)' for compatibility
c                with some compilers that use F77
c  08-17-06 EWL, update program for GERG-2004 equation of Kunz and Wagner
c  09-06-06 EWL, change nbrule to 20
c  01-24-13 MLH, check for alcohols in mixtures
c  07-16-14 EWL, move conversion of LJ6 to KW0 before call to RDMIX
c  06-27-16 EWL, shorten all code in MIX_HMX.FOR and remove comments on changes before the year 2006.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      include 'COMANC.INC'
      include 'COMTRN.INC'
      character*3 htype
      character*255 hFmix,hfli,herr,herr1,herr2

c  Commons associated with the binary mixing rule(s)
c     amix(k,1..3,i,j) -- coefs for the (general) mixing term.
c  fmix(1..nmxpar,i,j) -- parameters for the i-j binary.
c           namix(i,j) -- number of aFEQ(1..3,k,i,j) terms.
c               hmfile -- files from which mixing rules originate (stored in order 1,2  1,3  2,3; element zero is file called in SETUP).
c                 hbin -- documentation for the current binary parameters.
c           hModMx_hmx -- descriptions of the currently available mixing rules.
c             hpar_mxm -- descriptions of the binary parameters (e.g. Kt, Kv) associated with the currently available mixing rules.

      ierr=0
      herr=' '
      hmfile(0)=hFmix
      onethrd=-thrd

c  (Re)initialize contents of /HMXSAV/ when a new mixture is read in.
      fPRkij=0d0
      fPRkij2=0d0
      fstkij=0d0
      tausvm=0d0
      delsvm=0d0
      phisvm=0d0
      tausv2=0d0
      delsv2=0d0
      phisv2=0d0
      ibckwrd=0

c  Initialize the diagonal terms (i = j).
      iModMx(1:nc,1:nc)=0
      iModMx2(1:nc,1:nc)=0
      imodTc(1:nc,1:nc)=0
      imodVc(1:nc,1:nc)=0
      hmodMx(1:nc,1:nc)='i=j'
      namix (1:nc,1:nc)=0
      namix2(1:nc,1:nc)=0
      namix3(1:nc,1:nc)=0
      fmix (1:4,1:nc,1:nc)=1d0    !The i = j terms in the fmix summation should not be called (neither
      fmix2(1:4,1:nc,1:nc)=1d0    ! the i > j), but set the terms to one as precaution.
      fmix (5:nmxpar,1:nc,1:nc)=0d0
      fmix2(5:nmxpar,1:nc,1:nc)=0d0
      ftcij(1:nmxtc,1:nc,1:nc) =0d0
      fvcij(1:nmxvc,1:nc,1:nc) =0d0
      xecsljs(1:nc,1:nc)=0d0
      xecslje(1:nc,1:nc)=0d0
      xecskij(1:nc,1:nc)=0d0
      xecslij(1:nc,1:nc)=0d0
      xecsaji(1:nc,1:nc)=0d0
      xecskijk(1:nc,1:nc)=0d0
      xecslijk(1:nc,1:nc)=0d0
      xecsdij(1:nc,1:nc)=0d0
      xecsdij2(1:nc,1:nc)=0d0
      fPRkij(1:nmxprb,1:nc,1:nc)=0d0
      fPRkij2(1:nmxprb,1:nc,1:nc)=0d0
      fstkij(1:nc,1:nc,1:nmxstb)=0d0
      hmfile=hFmix

c  Read parameters for the binary pairs from the file.
      if (iRDbnc.ne.1 .or. iRDmxm.ne.1) then
        nread=12               !Logical unit for file reads
        hAltFluidName=' '
        call OPENFL (0,nread,hFmix,hfli,1,ierr,herr)
        if (ierr.ne.0) goto 101
        read (nread,'(a3)',err=102,end=102) htype
        if (htype.ne.heosmx) then
          call ERRNUM (112,0,' ',' ',0d0,0d0,0d0,ierr,herr)
          RETURN
        endif
        read (nread,*,err=102) VrsNm
        if (VrsNm.lt.3.) VrsNm=2d0
        call RDBNC (nread,ierr,herr)
        if (ierr.gt.0) RETURN
        call RDMIX (nread,ierr,herr)
        if (ierr.gt.0) RETURN
        close (nread)
      endif

      call GETMIXCF (0,0,0,ierr,herr)
      if (ierr.gt.0) RETURN
      ibin=0                 !Initialize counter for binary pair number.
      do i=1,nc-1
        do j=i+1,nc
          ibin=ibin+1
c  Check if any binary parameters need to be estimated.
          if (iModMx(i,j).eq.iMxiEQj .or. iModMx(i,j).eq.0) then
            call ESTBNC (i,j,ibin,ierr1,herr1)
            hmfile(ibin)='estimated by ESTBNC'
            imodTc(i,j)=iTc1
            imodVc(i,j)=iVc1
            if (ierr1.ne.0 .and. ierr.le.0) then
              ierr=ierr1
              herr=herr1
            endif
          endif
c  Check to see if Peng-Robinson parameters need to be estimated.
          if (ABS(fPRkij2(1,i,j)).lt.1d-20) then
            call ESTPR (i,j,0,fPRkijx,k,herr2)
            fPRkij2(1,i,j)=fPrkijx
            fPRkij2(1,j,i)=fPrkijx
          endif
        enddo
      enddo

c  Convert LJ6 model to XR0.
      do i=1,nc-1
        do j=i+1,nc
          if (iModMx(i,j).eq.iMxLJ) then
            if (fmix(4,i,j).eq.1. .and. fmix(5,i,j).eq.1. .and.
     &          fmix(3,i,j).eq.0.) then     !Only do this last one if converting to XR0 (or KW0) to enforce no excess function.
              tc12=SQRT(Tzred(i)*Tzred(j))
              tc12=(fmix(1,i,j)+Tzred(i)+Tzred(j))/2d0/tc12
              vc12=(Dzred(i)**onethrd+Dzred(j)**onethrd)**3/8d0
              vc12=(fmix(2,i,j)+Vzred(i)+Vzred(j))/2d0/vc12
              fij=0d0
              fmix (1:4,i,j)=1d0
              fmix2(1:4,i,j)=1d0
              fmix(5,i,j)=fij
              fmix(2,i,j)=tc12
              fmix(4,i,j)=vc12
              hmodMx(i,j)='XR0'
              iMxRule(i,j)=iRuleXR0
              iModMx(i,j)=iMxXR
              iModMx2(i,j)=i2MxXR
            endif
          endif
        enddo
      enddo

      fPRkij=0d0
      if (kijzero.eq.0) then
        fPRkij=fPRkij2  !If the flag to zero out all PR coefficients is off, transfer the values read in to the values that will be used.
      elseif (kijzero.eq.1) then
        do i=1,nc
          do j=1,nc
            call ESTPR (i,j,0,fPRkijx,k,herr2)
            fPRkij(1,i,j)=fPrkijx
            fPRkij(1,j,i)=fPrkijx
          enddo
        enddo
      endif

c  Check for the presence of water or alcohols in the mixture.
      do i=1,nc
        if (iFluidID(i).eq.  1) ixWat=i     !Water
        if (iFluidID(i).eq. 31) ixCO2=i     !CO2
        if (iFluidID(i).eq.500) ixAlc=i     !Methanol
        if (iFluidID(i).eq.501) ixAlc=i     !Ethanol
        if (iFluidID(i).eq.502) ixAlc=i     !Propanol
        if (iFluidID(i).eq.503) ixAlc=i     !Isopropanol
        if (iFluidID(i).eq.504) ixAlc=i     !1-Butanol
        if (iFluidID(i).eq.505) ixAlc=i     !Isobutanol
        if (iFluidID(i).eq.506) ixAlc=i     !2-Butanol
        if (iFluidID(i).eq.507) ixAlc=i     !tert-Butanol
      enddo

c  Check for humid air mixtures
      ixAir=0
      iair=0
      do i=1,nc
        if (iFluidID(i).eq.1)  iair=iair+1  !Water
        if (iFluidID(i).eq.16) iair=iair+1  !Nitrogen
        if (iFluidID(i).eq.19) iair=iair+1  !Argon
        if (iFluidID(i).eq.20) iair=iair+1  !Oxygen
        if (iFluidID(i).eq.31) iair=iair+1  !CO2
      enddo
      if (iair.ge.4 .and. nc.le.7) ixAir=1  !Mixture has at least four of the main moist air components, assume it so.  If more than 7 components, it may be natural gas instead.


c  Check whether the mixture contains water and ammonia, and the order of the components.
      do i=1,nc-1
      do j=i+1,nc
        if (iFluidID(i).eq.2 .and. iFluidID(j).eq.1) iAmWat=1
        if (iFluidID(i).eq.1 .and. iFluidID(j).eq.2) iAmWat=2
        if (iModMx(i,j).eq.iMxTr .and. (iAmWat.eq.0 .or. nc.ne.2))
     &      call ERRNUM (908,0,' ',' ',0d0,0d0,0d0,ierr,herr)   !Check for the TR1 ammonia-water model.
      enddo
      enddo

      if (nc.gt.1) call INITMIX
      ihr=ihrsav
      call ERRNUM ((ierr),ihr,' ',' ',0d0,0d0,998d0,ierr,herr)  !Recall the ERRNUM routine to save the output message that may have been destroyed by other routines above that used ierr1 and herr1.
      RETURN

c  Error terminations.

 101  call ERRNUM (101,101,'SETHMX',hFmix(1:80),0d0,0d0,0d0,ierr,herr)
      RETURN
 907  call ERRNUM (-907,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      RETURN
 102  call ERRNUM (102,102,' ',' ',0d0,0d0,0d0,ierr,herr)

      end                                              subroutine SETHMX
c
c ======================================================================
c
      subroutine INITMIX ()
c
c  Setup parameters that are not a function of composition.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
c     include 'COMANC.INC'
      onethrd=-thrd
      do i=1,nc
      do j=i,nc
       Tc12ij(i,j)=SQRT(Tzred(i)*Tzred(j))
       Vc12ij(i,j)=(Dzred(i)**onethrd+Dzred(j)**onethrd)**3/8d0
       cT12ij(i,j)=2d0*fmix(1,i,j)*fmix2(1,i,j)*fmix(2,i,j)*Tc12ij(i,j)
       cV12ij(i,j)=2d0*fmix(3,i,j)*fmix2(3,i,j)*fmix(4,i,j)*Vc12ij(i,j)
       f2mix (1,i,j)=fmix (1,i,j)**2
       f2mix (3,i,j)=fmix (3,i,j)**2
       f2mix2(1,i,j)=fmix2(1,i,j)**2
       f2mix2(3,i,j)=fmix2(3,i,j)**2
      enddo
      enddo
      n=0
      do i=1,nc-1
        do j=i+1,nc
          ncode2(i,j)=0
          idlmix(i,j)=2
          if (iModMx2(i,j).eq.i2MxLJ .or. iModMx2(i,j).eq.i2MxKW .or.
     &      iModMx2(i,j).eq.i2MxB) then
            n=n+1
            ncode2(i,j)=n
            m=namix(i,j)+namix2(i,j)+namix3(i,j)
            idlmix(i,j)=INT(MAXVAL(amix(1:m,4,i,j)))  !Get maximum power of density in the EXP(-del^l) terms
            if (idlmix(i,j).lt.2) idlmix(i,j)=2
          endif
        enddo
      enddo
c
      end                                             subroutine INITMIX
c
c ======================================================================
c
      subroutine RDBNC (nread,ierr,herr)
c
c  Read binary parameters for the HMX mixture model and also the
c  critical line models from the HMX.BNC file.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                     -117 - Binary pair not found, all parameters will be estimated
c                      118 - Error in reading mixture file
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-10-96  MM, Original version
c  08-17-06 EWL, Redo logic to make more efficient and allow nonformatted inputs.
c  08-17-06 EWL, Update program for GERG-2004 equation of Kunz and Wagner.
c  12-24-06 MLH, Allow for 4 binary interaction parameters for viscosity.
c  01-09-07 EWL, Modify the code so that other entries than just 'KW#' or 'LJ#'
c                can be on the same line as the ID numbers (thus, the KW or LJ
c                entries do not have to exist).
c  01-28-07 MLH, Allow for 3 binary interaction parameters for thermal conductivity.
c  03-05-07 EWL, Add fmix2 array in case fluids are entered in opposite order as required by Kunz and Wagner.
c  05-27-08 MLH, Add one more binary int par for dilute gas viscosity xecsdij in trnbin.
c  06-11-08 MLH, Add second bin int for dilute gas vis xecsdij2.
c  07-08-10 EWL, Add check for official GERG2004 coefficients.
c  07-10-13 EWL, Change check for official GERG2004 coefficients so that the KW
c                and GE lines can be contained in the same block, rather than
c                as 2 separate blocks
c  05-04-18 EWL, Change the routines purpose to just read in and store all of the parameters in the HMX.BNC file.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      include 'COMANC.INC'
      include 'COMTRN.INC'
      character hstar*1,hb*1,hc*2,hd*3,hflag*3,hidijk*15,hidjik*15
      character*255 herr,hbinp,h255,LTRIM

c  Search for key characters in cols 1 and 2-4 and branch accordingly.
c   hstar - '#' or '@' indicates start of model specification.
c   hflag indicates type of model:
c       'BNC' - binary mixture coefficients
c       'MXM' - mixture model specification

      ierr=0
      herr=' '
      if (iRDbnc.eq.1) RETURN
      iGG8_hmx =0
      fTcij_hmx=0d0
      fvcij_hmx=0d0
      fPRk_hmx =0d0
      fSTk_hmx =0d0
      xECS_hmx =0d0
      iRuleXR0=0
      iRuleLJ6=0
      icrt=6
      h255=' '
c     if (VrsNm.ge.4.) icrt=11   !The arrays are big enough to handle 11 numbers, but only six are currently used.  The hmx.bnc file in version 10 contains 11 parameters on each line for KW, XR, LJ, etc.
      do
        read (nread,'(a1,a3)',end=118,err=118) hstar,hflag
        hd=' '
        if (hstar.eq.'#' .or. hstar.eq.'@') then
          if (hflag.eq.'END' .or. hflag.eq.'end') goto 118
          read (nread,'(a3)',err=118) hd
          do
            read (nread,'(a1)',end=118,err=118) hb
            if (hb.ne.'?') EXIT       !Read till the line starts with a question mark.
          enddo
        endif
        if (hflag.eq.'BNC' .and. hd.eq.'BNC') EXIT
      enddo

c  Begin reading sections starting with the HASH numbers
c   and ending with a line containing only an exclamation mark.
      k=0
      do
        hbinp=' '
        k=k+1
        do
          read (nread,'(a)',err=118,end=118) h255
          if (h255(1:4).eq.'#MXM') goto 500 !End of binary pairs has been reached, exit routine.
          if (h255(1:1).eq.'?') then
            if (h255(2:2).ne.'#') then
              if (hbinp.eq.' ') then     !If a question mark, add the lines to a buffer and then loop.
                hbinp=h255(2:)
              else
                i=INDEX(hbinp,'                     ')
                hbinp=hbinp(:i-1)//CHAR(13)//CHAR(10)//h255(2:)
              endif
            endif
          elseif (h255(1:1).eq.'!' .or. h255.eq.' ') then
            !Just skip lines with an exclamation point or that are blank.
          else
            i=INDEX(h255,'/')
            if (i.gt.0) EXIT       !Continue the loop until a line with hash characters or a CAS number is found.
          endif
        enddo
        hbin_hmx(k)=hbinp

c  Extract the ID numbers from the input line.
        hIDijk=LTRIM(h255(:i-1))
        h255=LTRIM(h255(i+1:))
        i=INDEX(h255,' ')
        hIDjik=h255(:i)
        h255=h255(i:)
        if (h255.eq.' ') read (nread,'(a)',err=118,end=118) h255
        iIDij_hmx(1,k)=iHEXtoDec(hIDijk)  !Convert from hex to integer.
        iIDij_hmx(2,k)=iHEXtoDec(hIDjik)

c  Read in the parameters for each binary mixture.
        do ij=1,20
          h255=LTRIM(h255)
          hb=h255(1:1)
          hc=h255(1:2)
          hd=h255(1:3)

c  For model TC1, the order i,j vs. j,i is significant.  The variable iBckwrd determines which coefficients should be used with which fluid.
          if (hc.eq.'TC') then
            read (h255(4:),*,err=200,end=118) (fTcij_hmx(m,k),m=1,icrt)
            imodTc_hmx(k)=ICHAR(h255(3:3))-48

c  For model VC1, the order i,j vs j,i is significant.    The variable iBckwrd determines which coefficients should be used with which fluid.
          elseif (hc.eq.'VC') then
            read (h255(4:),*,err=200,end=118) (fvcij_hmx(m,k),m=1,icrt)
            imodVc_hmx(k)=ICHAR(h255(3:3))-48

c  Read in Peng-Robinson interaction parameters.
          elseif (hc.eq.'PR') then
c           read(h255(4:),*,err=200,end=118)(fPRk_hmx(m,k),m=1,nmxprb)
            read(h255(4:),*,err=200,end=118) fPRk_hmx(1,k)  !Only the first parameter is used, so reading only one value will speed up this section.

c  Read in surface tension parameters.
          elseif (hc.eq.'ST') then
            read(h255(4:),*,err=200,end=118)(fSTk_hmx(m,k),m=1,nmxstb)

c  Read in transport interaction parameters.
          elseif (hd.eq.'TRN') then
            read(h255(4:),*,err=200,end=118)(xECS_hmx(m,k),m=1,9)

          else
            iModMx_hmx(1,k)=0
            iModMx_hmx(2,k)=0
            hmodmx_hmx(k)=hd
            !Currently there is no difference between iModMx and iModMx2 except in where they are used.
            ! Eventually Refprop can be set up so that the two contain different codes, with iModMx as the rule for
            ! the reducing parameters and iModMx2 is the code for the departure function.

            if (hc.eq.'KW') then
              iModMx_hmx(1,k)=iMxKW
              iModMx_hmx(2,k)=i2MxKW

            elseif (hc.eq.'XR') then
              iModMx_hmx(1,k)=iMxXR
              iModMx_hmx(2,k)=i2MxXR

            elseif (hc.eq.'GG') then
             read(h255(4:),*,err=200,end=118)(fGG8_hmx(m,k),m=1,nmxpar)
              iGG8_hmx(k)=ICHAR(hd(3:3))
              iModMx_hmx(3,k)=iMxKW
              iModMx_hmx(4,k)=i2MxKW

            elseif (hb.eq.'B') then
              iModMx_hmx(1,k)=iMxB
              iModMx_hmx(2,k)=i2MxB

            elseif (hc.eq.'LJ') then
              iModMx_hmx(1,k)=iMxLJ
              iModMx_hmx(2,k)=i2MxLJ

            elseif (hc.eq.'TR') then
              iModMx_hmx(1,k)=iMxTr
              iModMx_hmx(2,k)=i2MxTr

            elseif (hc.eq.'i=') then     !i=j
              iModMx_hmx(1,k)=iMxiEqj
              iModMx_hmx(2,k)=i2MxiEqj

            elseif (hc.eq.'GE') then
              goto 907

            elseif (h255.ne.' ') then

            else
              goto 118
            endif

            read(h255(4:),*,err=200,end=118)(fmix_hmx(m,k),m=1,nmxpar)
          endif

 200      read (nread,'(a)',err=118,end=118) h255
          if (h255(1:1).eq.'!' .or. h255.eq.' ') goto 300
        enddo
 300  enddo

 118  call ERRNUM (118,0,'RDBNC',' ',0d0,0d0,0d0,ierr,herr)!Abnormal termination
      RETURN
 907  call ERRNUM (907,0,' ',' ',0d0,0d0,0d0,ierr,herr)    !'GE' found in HMX.BNC.
      RETURN
 500  nMx_hmx=k
      backspace (nread)
      iRDbnc=1

      RETURN

      end                                               subroutine RDBNC
c
c ======================================================================
c
      subroutine RDMIX (nread,ierr,herr)
c
c  Read coefficients for mixing rule(s) from file.
c
c  See additional comments in SETHMX.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      118 - Error in reading mixture file
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  05-07-96  MM, original version (extracted from SETHMX)
c  08-17-06 EWL, update program for GERG-2004 equation of Kunz and Wagner
c  01-03-07 EWL, change the 2000 to 20000 in 'do iline=1,2000' to read in more lines

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character hstar*1,h1*1,hflag*3
      character hinfo*251,herr*255

      ierr=0
      herr=' '
      if (iRDmxm.eq.1) RETURN
      aterm_mxm=0d0
      nterm_mxm=0

c  Search for key characters in cols 1 and 2-4 and branch accordingly
c   hstar - '#' or '@' indicates start of model specification.
c   hflag indicates type of model:
c       'BNC' - binary mixture coefficients
c       'MXM' - mixture model specification
      k=0
      do iline=1,50000
      read (nread,'(a1,a3)',end=118,err=118) hstar,hflag
      if ((hstar.eq.'#' .or. hstar.eq.'@').and. hflag.eq.'MXM') then
        k=k+1
        read (nread,'(a3,a251)',err=118) htype_mxm(k),hinfo
        do m=1,1000          !Skip over source comments.
          read (nread,'(a1)',err=118) h1
          if (h1.ne.'?') EXIT               !Comment block terminated by '!'
        enddo
        hinfo_mxm(k)=htype_mxm(k)//hinfo
        if (htype_mxm(k).eq.'XR0') iRuleXR0=k
        if (htype_mxm(k).eq.'LJ6') iRuleLJ6=k
        read (nread,'(6(a8,1x))',err=118) (hpar_mxm(m,k),m=1,nmxpar)  !Read descriptors for binary-specific parameters.
        read (nread,*,err=102) (fideal_mxm(m,k),m=1,nmxpar)           !Read default (ideal-solution) parameters.
        nterm_mxm(1:7,k)=0
        read (nread,*,err=120) (nterm_mxm(ii,k),ii=1,7)               !nterm,ncoef,nspar,nterm2,ncoef2,nterm3,ncoef3  !Read coefficients for mixing rule.
 120    if (nterm_mxm(3,k).eq.1) nterm_mxm(2,k)=nterm_mxm(2,k)+1      !ncoef=ncoef+1       !Compatibility for version 7.0
        do m=1,nterm_mxm(1,k)
          read (nread,*,err=102) (aterm_mxm(m,n,k),n=1,nterm_mxm(2,k))
        enddo
        if (nterm_mxm(4,k).ne.0) then
          do m=1,nterm_mxm(4,k)
            read (nread,*,err=102) (aterm_mxm(m+nterm_mxm(1,k),n,k),
     *                              n=1,nterm_mxm(5,k))
          enddo
        endif
        if (nterm_mxm(6,k).ne.0) then
          do m=1,nterm_mxm(6,k)
            read (nread,*,err=102)(aterm_mxm(m+nterm_mxm(1,k)+
     &                 nterm_mxm(4,k),n,k),n=1,nterm_mxm(7,k))
          enddo
        endif
      elseif (hflag.eq.'END') then
        EXIT
      endif
      enddo
      nrule_mxm=k
      iRDmxm=1

      RETURN

c  Error terminations.

 102  call ERRNUM (102,102,' ',' ',0d0,0d0,0d0,ierr,herr)
      RETURN
 118  call ERRNUM (118,0,'RDMIX',' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine RDMIX
c
c ======================================================================
c
      subroutine GETMIXCF (icomp,jcomp,iMx,ierr,herr)
c
c  Input:
c    icomp--Component i; ignored if iMx=0
c    jcomp--Component j; ignored if iMx=0
c      iMx--Set to 0 to load all coefficients or set to 1 to load only
c           the mixture model coefficients for the mixture where iMxRule=iMx
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                     -117 - Binary pair not found, all parameters will be estimated
c                      118 - Error in reading mixture file
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon from code of M.O. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  05-04-18 EWL, original version, May the 4th be with you.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      include 'COMANC.INC'
      include 'COMTRN.INC'
      character*255 herr

      ierr=0
      herr=' '
      icrt=6
      iFlg=0
c     if (VrsNm.ge.4.) icrt=11   !The arrays are big enough to handle 11 numbers, but only six are currently used.  The hmx.bnc file in version 10 contains 11 parameters on each line for KW, XR, LJ, etc.

      do ii=1,nc-1
        do jj=ii+1,nc
          i=ii
          j=jj

          if (iMx.gt.0) then
            i=icomp
            j=jcomp
            k=iMx
            goto 200
          endif

          do k=1,nMx_hmx
            iFlg=0
            i1=iIDij_hmx(1,k)
            j1=iIDij_hmx(2,k)
            if (ihash(i).eq.i1 .and. ihash(j).eq.j1) iFlg=1
            if (ihash(i).eq.j1 .and. ihash(j).eq.i1) iFlg=2
            if (iFlg.gt.0) goto 100
          enddo

          do k=1,nMx_hmx
            i1=iIDij_hmx(1,k)
            j1=iIDij_hmx(2,k)
            if (iAlthash(i).eq.j1 .and. iAlthash(j).eq.i1) iFlg=8 !Don't change the order of these.
            if (iAlthash(i).eq.i1 .and. iAlthash(j).eq.j1) iFlg=7
            if (iAlthash(i).eq.j1 .and. ihash   (j).eq.i1) iFlg=6
            if (iAlthash(i).eq.i1 .and. ihash   (j).eq.j1) iFlg=5
            if (ihash   (i).eq.j1 .and. iAlthash(j).eq.i1) iFlg=4
            if (ihash   (i).eq.i1 .and. iAlthash(j).eq.j1) iFlg=3
            if (iFlg.gt.0) goto 100
          enddo
          goto 500

 100      ibin=nc*(i-1)-(i**2-i)/2+(j-i)
          iAltFlg(i,j)=iFlg
          iBckwrd(i,j)=1-MOD(iFlg,2)
          hbin(ibin)=hbin_hmx(k)

c  Don't do the following for the alternative mixing rules.
          if (iFlg.le.2) then
            fTcij(1:icrt,i,j)=fTcij_hmx(1:icrt,k)     !For models TC1 and VC1, the order i,j vs j,i is significant.
            fVcij(1:icrt,i,j)=fVcij_hmx(1:icrt,k)     ! The variable iBckwrd determines which coefficients should be used with which fluid.
            imodTc(i,j)=imodTc_hmx(k)
            imodVc(i,j)=imodVc_hmx(k)

c  Load Peng-Robinson interaction parameters.
            fPRkij2(1:nmxprb,i,j)=fPRk_hmx(1:nmxprb,k)
            fPRkij2(1:nmxprb,j,i)=fPRk_hmx(1:nmxprb,k)

c  Load surface tension parameters.
            fSTkij(i,j,1:nmxprb)=fSTk_hmx(1:nmxprb,k)
            fSTkij(j,i,1:nmxprb)=fSTk_hmx(1:nmxprb,k)

c  Load transport interaction parameters.
            xecsljs (i,j)=xECS_hmx(1,k)
            xecslje (i,j)=xECS_hmx(2,k)
            xecskij (i,j)=xECS_hmx(3,k)
            xecslij (i,j)=xECS_hmx(4,k)
            xecsaji (i,j)=xECS_hmx(5,k)
            xecskijk(i,j)=xECS_hmx(6,k)
            xecslijk(i,j)=xECS_hmx(7,k)
            xecsdij (i,j)=xECS_hmx(8,k)
            xecsdij2(i,j)=xECS_hmx(9,k)
            xecsljs (j,i)=xECS_hmx(1,k)
            xecslje (j,i)=xECS_hmx(2,k)
            xecskij (j,i)=xECS_hmx(3,k)
            xecslij (j,i)=xECS_hmx(4,k)
            xecsaji (j,i)=xECS_hmx(5,k)
            xecskijk(j,i)=xECS_hmx(6,k)
            xecslijk(j,i)=xECS_hmx(7,k)
            xecsdij (j,i)=xECS_hmx(8,k)
            xecsdij2(j,i)=xECS_hmx(9,k)
          endif

c  Load the parameters from the HMX.BNC file into the working arrays.
 200      continue
          iModMx (i,j)=iModMx_hmx(1,k)      !Currently there is no difference between iModMx and iModMx2 except in where they are used.
          iModMx2(i,j)=iModMx_hmx(2,k)      ! the reducing parameters and iModMx2 is the code for the departure function.
          hmodMx (i,j)=hmodmx_hmx(k)
          fmix(1:nmxpar,i,j) =fmix_hmx(1:nmxpar,k)
          fmix2(1:4,i,j)     =1d0           !These are not in the HMX.BNC file, manually set here.
          fmix2(5:nmxpar,i,j)=0d0
          if (iGERG08.gt.0 .and. iGG8_hmx(k).gt.0) then
            iModMx (i,j)=iModMx_hmx(3,k)
            iModMx2(i,j)=iModMx_hmx(4,k)
            hmodMx (i,j)='KW'//CHAR(iGG8_hmx(k))
            fmix(1:nmxpar,i,j)=fGG8_hmx(1:nmxpar,k)
          endif

c  Check if the specified order is opposite that in HMX.BNC file.
c  For the LJ models, pair j,i is the same as i,j except for the beta and gamma parameters, which depend on the order of the fluids.
c  For the KW, XR, and B models, pair j,i is the same as i,j except for the beta parameters, which depend on the order of the fluids.
          if (iBckwrd(i,j).gt.0) then
            if (iModMx(i,j).eq.iMxLJ .or. iModMx(i,j).eq.iMxLM) then
              call SWAPV (fmix(4,i,j),fmix(5,i,j))
            elseif (iModMx(i,j).eq.iMxKW .or. iModMx(i,j).eq.iMxXR .or.
     &              iModMx(i,j).eq.iMxB) then
              fmix2(1,i,j)=fmix(1,i,j)
              fmix2(3,i,j)=fmix(3,i,j)
              fmix(1,i,j)=1d0
              fmix(3,i,j)=1d0
            endif
          endif

c  Find mixing terms and load coefficients into working arrays.
          do k=1,nrule_mxm
            if (htype_mxm(k).eq.hmodMx(i,j)) then
              iMxrule(i,j)=k
              namix (i,j)=nterm_mxm(1,k)
              namix2(i,j)=nterm_mxm(4,k)
              namix3(i,j)=nterm_mxm(6,k)
              do m=1,namix(i,j)+namix2(i,j)+namix3(i,j)
                amix(m,1:nmxcof,i,j)=aterm_mxm(m,1:nmxcof,k)
              enddo
              EXIT
            endif
          enddo
          if (iMx.gt.0) goto 500
          if (nc.eq.2 .and. (iFlg.eq.1 .or. iFlg.eq.2)) goto 500   !Exit if interaction parameters found for a binary.
        enddo
      enddo

c  Check if alternative mixing rules from other similar substances were used.
 500  if (iMx.eq.0) then
        do i=1,nc-1
          do j=i+1,nc
            if (iAltFlg(i,j).gt.2) then
              call ERRNUM (-117,80,' ',' ',0d0,0d0,0d0,ierr,herr)
              RETURN
            endif
          enddo
        enddo
      endif
      RETURN

 118  call ERRNUM (118,0,'RDBNC',' ',0d0,0d0,0d0,ierr,herr)!Abnormal termination
      RETURN

      end                                            subroutine GETMIXCF
c
c ======================================================================
c
      subroutine ESTBNC (i,j,ibin,ierr,herr)
c
c  Estimate binary parameters for the HMX mixture model.
c
c  Inputs:
c        i--Component i
c        j--Component j
c     ibin--Counter for binary pair (1,2 = 1; 1,3 = 2; 2,3 = 3; etc.)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                     -117 - No binary parameters found (warning)
c                      117 - Estimation not possible for input pair
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-12-96  MM, original version
c  06-08-15 EWL, add preliminary estimation equation of Bell for normal alkanes
c  08-08-17 EWL, add preliminary estimation equation of Bell for nitrogen or CO2 with other fluids

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*255 herr,hf1,hf2

      ierr=0
      iFlg=0
      do k=1,2
        n=0
        if (k.eq.1) n=i
        if (k.eq.2) n=j
        if (iFluidID(n).eq.  1) iFlg=1 !Water
        if (iFluidID(n).eq.  3) iFlg=1 !Heavy water
        if (iFluidID(n).eq.  2) iFlg=1 !Ammonia
        if (iFluidID(n).eq. 10) iFlg=1 !Helium
        if (iFluidID(n).eq. 11) iFlg=1 !Parahydrogen
        if (iFluidID(n).eq. 12) iFlg=1 !Hydrogen
        if (iFluidID(n).eq. 14) iFlg=1 !Deuterium
        if (iFluidID(n).eq. 15) iFlg=1 !Neon
        if (iFluidID(n).eq.500) iFlg=1 !Methanol
        if (iFluidID(n).eq.501) iFlg=1 !Ethanol
      enddo
      if (iFlg.eq.1) then
        call ERRNUM (117,1,' ',' ',0d0,0d0,xerr,ierr,herr)
        hbin(ibin)='Estimation of mixing parameters is not possible '//
     &             'for this mixture.'
        hmodMx(i,j)='XR0'
        iMxRule(i,j)=iRuleXR0
        iModMx(i,j)=iMxXR
        iModMx2(i,j)=i2MxXR

c  Return warning message to GUI
      else
       call ERRNUM (-117,10,' ',' ',0d0,0d0,xerr,ierr,herr)
       hbin(ibin)='Mixture data have not been fitted for this binary '//
     &            'pair; the mixing parameters have been estimated.'
      endif

c  Set parameters to defaults
      fmix(1:4,i,j)=1d0
      fmix(5:nmxpar,i,j)=0d0
      fmix2(1:nmxpar,i,j)=fmix(1:nmxpar,i,j)
      iBckwrd(i,j)=0

c  Set up for different estimation schemes.
      if1=iFluidID(i)
      if2=iFluidID(j)
      hf1=hFamily(i)
      hf2=hFamily(j)

c  All possible families:
c     alcohol, halocb, other, FAME, siloxane, cryogen
c     n-alkane, n-alkene, br-alkane, br-alkene, alkyne, naphthene, aromatic, ether

      l1a=.false.
      l2a=.false.
      l1e=.false.
      l2e=.false.
      l1f1=.false.
      l2f1=.false.
      l1f2=.false.
      l2f2=.false.
      l1f3=.false.
      l2f3=.false.

      if (hf1.eq.'n-alkane' .or. hf1.eq.'br-alkane') l1a=.true.      !Check for normal or branched alkanes.
      if (hf2.eq.'n-alkane' .or. hf2.eq.'br-alkane') l2a=.true.
      if (hf1.eq.'n-alkene' .or. hf1.eq.'br-alkene') l1e=.true.      !Check for normal or branched alkenes.
      if (hf2.eq.'n-alkene' .or. hf2.eq.'br-alkene') l2e=.true.
      if (hf1.eq.'alkyne'   .or. hf1.eq.'naphthene') l1e=.true.      !For alkynes or naphthenes, use same method as alkenes.
      if (hf2.eq.'alkyne'   .or. hf2.eq.'naphthene') l2e=.true.
      if (                       hf1.eq.'aromatic')  l1e=.true.      !Include aromatics with alkenes.
      if (                       hf2.eq.'aromatic')  l2e=.true.
      if (l2a .or. l2e) then !Mixtures of a fluid below in class 1,2,3 with a hydrocarbon.
        if (if1.ge.16 .and. if1.le.20) l1f1=.true.    !Check for fluid class 1, which includes nitrogen, CO, fluorine, argon, and oxygen.
        if (if1.eq.101 .or. if1.eq.22) l1f2=.true.    !Check for fluid class 2, which includes methane and krypton.
        if (if1.eq.31) l1f3=.true.                    !Check for fluid class 3, which includes CO2 only.
      endif
      if (l1a .or. l1e) then !Reverse order (a hydrocarbon mixed with a fluid in class 1,2,3).
        if (if2.ge.16 .and. if2.le.20) l2f1=.true.
        if (if2.eq.101 .or. if2.eq.22) l2f2=.true.
        if (if2.eq.31) l2f3=.true.
      endif

c  Predictive scheme of Bell et al. (2018) for hydrocarbons.
c  Ordering of the components depends on molar mass.
      if ((l1a .or. l1e) .and. (l2a .or. l2e)) then
        hmodMx(i,j)='XR0'
        iMxRule(i,j)=iRuleXR0
        iModMx(i,j)=iMxXR
        iModMx2(i,j)=i2MxXR
        wm=wmas(i)/wmas(j)
        if (wm.lt.1.) wm=1d0/wm
        fmix(2,i,j)= 0.0788935d0 *wm+0.900948d0  !gammaT
        fbeta      =-0.00955707d0*wm+1.0133d0    !betaT
        if (wmas(i)/wmas(j).le.1.) then
          fmix(1,i,j)=fbeta                 !Fluids were entered in the correct order as used by the Bell estimation scheme.
        else
          fmix2(1,i,j)=fbeta                !Fluids were entered backwards, so the estimation is placed in the fmix2 array.
          iBckwrd(i,j)=1
        endif

c  Predictive scheme of Bell et al. (2018) for N2, CO2, or methane with other fluids.
c  Due to lack of data for mixtures with O2, AR, CO, and F2, these fluids are included in the nitrogen scheme.
c   gammaT = c[0] + c[1]*(mw/mw_target)^c[2]
c   betaT = c[0]*(mw/mw_target)^2 + c[1]*(mw/mw_target) + c[2]
      elseif (l1f1.or.l1f2.or.l1f3 .or. l2f1.or.l2f2.or.l2f3) then
        hmodMx(i,j)='XR0'
        iMxRule(i,j)=iRuleXR0
        iModMx(i,j)=iMxXR
        iModMx2(i,j)=i2MxXR
        w=wmas(j)/wmas(i)
        if (l2f1 .or. l2f2 .or. l2f3) w=1d0/w
        if (l1f1 .or. l2f1) then
          fmix(2,i,j)=0.44388944d0+0.64847841d0*w**0.47472039d0      !gammaT for nitrogen, etc.
          fbeta=-9.10115760d-5*w**2-6.55614848d-3*w+0.994532046d0    !betaT
        elseif (l1f2 .or. l2f2) then
          fmix(2,i,j)=0.83251516d0+0.13425537d0*w**0.80206535d0      !gammaT for methane and krypton
          fbeta=-2.29446041d-4*w**2-8.09427088d-3*w+1.01923440d0     !betaT
        else
          fmix(2,i,j)=0.77825848d0+0.13436697d0*w**0.89121753d0      !gammaT for CO2
          fbeta=-0.00371728d0*w**2+0.01011034d0*w+1.00956553d0       !betaT
        endif
        if (l1f1 .or. l1f2 .or. l1f3) then
          fmix(1,i,j)=fbeta                 !Fluids were entered in the correct order as used by the Bell estimation scheme.
        else
          fmix2(1,i,j)=fbeta                !Fluids were entered backwards, so the estimation is placed in the fmix2 array.
          iBckwrd(i,j)=1
        endif

c  Predictive scheme of Lemmon.
c  Ordering of the components depends on the ratio of the dipole moments.
      else
        fmix(2:3,i,j)=0d0    !k=2: xi (term in reducing volume);              k=3: Fij (multiplier for excess function)
        fmix(4:5,i,j)=1d0    !k=4: beta (exponent for first component);       k=5: gamma (exponent for second component)
        hmodMx(i,j)='LJ6'
        iMxRule(i,j)=iRuleLJ6
        iModMx(i,j)=iMxLJ
        iModMx2(i,j)=i2MxLJ

        k=0
        if (ABS(dipole(i)-dipole(j)).lt.1d-5) then
c  Special case for equal dipole moments.
          if (Tcrit(i)/(Pcrit(i)*xEq0(accen(i))).gt.
     &        Tcrit(j)/(Pcrit(j)*xEq0(accen(j)))) k=1
        else
          if (dipole(i).lt.dipole(j)) k=1
        endif
        if (k.eq.1) then
          tratio=Tcrit(i)/Tcrit(j)
          pratio=Pcrit(i)/Pcrit(j)
          accenr=XDIV(accen(i),accen(j))
        else
          tratio=XDIV(Tcrit(j),Tcrit(i))
          pratio=XDIV(Pcrit(j),Pcrit(i))
          accenr=XDIV(accen(j),accen(i))
        endif
        accenr=xEq0(accenr)
        if (tratio/pratio/accenr.gt.5 .and. ABS(accenr).lt.0.1d0) then
          if (tratio.gt.0.75d0 .and. tratio.lt.1.25d0) then
            tratio=1d0/tratio
            pratio=1d0/pratio
            accenr=1d0/accenr
          endif
        endif

c  Define the zeta parameter (the one applied to the reducing temperature).
       fmix(1,i,j)=(40.4d0-25.03d0*2d0**(tratio/pratio/accenr))/tratio
       if (ABS(fmix(1,i,j)).gt.200.) then
         if(ABS(fmix(1,i,j)).gt.500.)fmix(1,i,j)=SIGN(500d0,fmix(1,i,j))
         if (ierr.ne.117)
     &     call ERRNUM (117,2,' ',' ',0d0,0d0,xerr,ierr,herr)
        endif
      endif

      end                                              subroutine ESTBNC
c
c ======================================================================
c
      subroutine HMXORDER (i,j,hIDi,hIDj,iFlag,ierr,herr)
c
c  Return the ID numbers in the order given in the HMX.BNC file, and
c  a flag that indicates if the loaded fluids are in the same order.
c
c  Inputs:
c        i--Component i
c        j--Component j
c
c  Outputs:
c    hIDi--ID number of the first fluid stored in the HMX.BNC file for the i,j pair (character*255 variable)
c    hIDj--ID number of the second fluid stored in the HMX.BNC file for the i,j pair (character*255 variable)
c    iFlag--Flag to indicate if loaded fluids are in the same order as the i,j pair:
c           0 - Pair is backwards
c           1 - Pair is in correct order (or if i=j)
c           2 - Pair is not in HMX.BNC
c     ierr--Error number, not currently used here
c     herr--Error message, not currently used here (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-02-15 EWL, original version

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*255 herr,hIDi,hIDj
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::HMXORDER

      ierr=0
      herr=' '
      iFlag=1

      hIDi=hash(i)
      hIDj=hash(j)
      if (i.eq.j) then
      elseif (i.le.j) then
        iFlag=1-iBckwrd(i,j)
c       if (iAltFlg(i,j).ne.1 .and. iAltFlg(i,j).ne.2) iFlag=2
      else
        iFlag=iBckwrd(j,i)
c       if (iAltFlg(j,i).ne.1 .and. iAltFlg(j,i).ne.2) iFlag=2
      endif

      end                                            subroutine HMXORDER
c
c ======================================================================
c
      subroutine SETKTV (icomp,jcomp,hmodij,fij,hFmix,ierr,herr)
c
c  Set mixture model and/or parameters.
c
c  This subroutine must be called after SETUP, but before any call to
c  SETREF (for cases where energy, enthalpy, entropy, Gibbs energy, or
c  the Helmholtz energy are required); it need not be called at all if
c  the default mixture parameters (those read in by SETUP) are to be used.
c
c  The component numbers icomp and jcomp must match the order that is found
c  in the HMX.BNC file for each binary pair, or, in the case where
c  no interaction parameters are available in the HMX.BNC file, icomp
c  and jcomp must be in the same order as was used in the call to SETUP.
c  If the numbers in these two integers are backwards, an error number
c  and message will be returned, and nothing will be changed.  In this
c  situation, switch the numbers and call this routine again.
c
c  ========================   ==============================
c  Kunz-Wagner model (KW0)    Lemmon-Jacobsen model (LJ6)
c  ------------------------   ------------------------------
c  fij(1) = betaT             fij(1) = zeta
c  fij(2) = gammaT            fij(2) = xi
c  fij(3) = betaV             fij(3) = Fij
c  fij(4) = gammaV            fij(4) = beta
c  fij(5) = Fij               fij(5) = gamma
c  fij(6) = 'not used'        fij(6) = 'not used'
c  ========================   ==============================
c
c  Inputs:
c    icomp--Component i
c    jcomp--Component j
c   hmodij--Mixing rule for the binary pair i,j (e.g. LJ6, KW0, XR0, or LIN) (character*3)
c           If hmodij is 'RST', reset all pairs to values from the original call to SETUP
c           (all other inputs are ignored)
c      fij--Binary mixture parameters (array of dimension nmxpar; currently nmxpar is set to 6)
c           The parameters will vary depending on hmodij (see above)
c    hFmix--No longer used.
c           Info from previous versions: File name (character*255) containing
c           generalized parameters for the binary mixture model; this will usually
c           be the same as the corresponding input to SETUP (e.g.,'HMX.BNC')
c
c  Outputs:
c     ierr--Error flag:
c           0 - Successful
c           111 - Error in opening mixture file
c           902 - Routine not value for a pure fluid
c           903 - Illegal i,j specification (i=j or i>nc or j>nc)
c           904 - Order of fluids is backwards from that in HMX.BNC
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  05-07-96  MM, original version
c  09-09-15 EWL, check for input pair given backwards from that in the HMX.BNC, and set beta=1/beta if so
c  06-22-16 EWL, shorten code

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*3 hmodij
      character*255 hFmix,hfmix2,herr,hIDi,hIDj
      dimension fij(nmxpar)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETKTV

      ierr=0
      herr=' '
      i=icomp
      j=jcomp
      if (i.gt.j) call SWAPI (i,j)

c  Check if icomp or jcomp > nc.  Do not allow change of pure fluid or i=j parameters.
      if (nc.le.1) then
        herr=hFmix  !Keeps the compiler happy about hFmix not being used.
        call ERRNUM (902,0,'SETKTV',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      elseif (i.eq.j .or. i.gt.nc .or. j.gt.nc) then
        call ERRNUM (903,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif

      call RESETA (0)

c  Set Peng-Robinson mixing rules.
      if (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) then
        call SETEOSMX (hmodij)
        fPRkij2(1:nmxprb,icomp,jcomp)=fij(1:nmxprb)
        fPRkij2(1:nmxprb,jcomp,icomp)=fij(1:nmxprb)
        if (kijzero.eq.0) then
          fPRkij(1:nmxprb,icomp,jcomp)=fPRkij2(1:nmxprb,icomp,jcomp)
          fPRkij(1:nmxprb,jcomp,icomp)=fPRkij2(1:nmxprb,icomp,jcomp)
        endif
        RETURN
      endif

c  Set flag indicating that reference state needs to be recalculated
c   (for use with the ixflag=2 option of SETREF).
      ixfsav=0
      if (hmodij.eq.'RST' .or. hmodij.eq.'rst') then       !Reset parameters to original values.
        hfmix2=hmfile(0)                                   !Retrieve file specified on original call to SETUP.
        call SETHMX (hfmix2,ierr,herr)
c       hmfile(1:nbin)=hmfile(0)
        RETURN
      endif

c  Exit routine if component order is backwards.
      call HMXORDER (icomp,jcomp,hIDi,hIDj,iFlag,ierr,herr)
      if (iFlag.eq.0) then
        call ERRNUM (904,0,' ',' ',0d0,0d0,0d0,ierr,herr)  !If fluid order is backwards than what is stored in the HMX.BNC file, exit with an error number and message.
        RETURN
      endif

c  Insert the new numbers into the parameter arrays.
      do k=1,nmxpar
        f=fij(k)
        f2=1d0
        if (k.ge.5) f2=0d0
        if (k.eq.1 .or. k.eq.3) then
          if (iFlag.eq.0 .and. f.ne.0.) f=1d0/f
          if (icomp.gt.jcomp) call SWAPV (f,f2)
        endif
        fmix2(k,i,j)=f2
        fmix(k,i,j)=f
      enddo
      ii=INDEX(hname(i),'  ')-1
      jj=INDEX(hname(j),'  ')-1
      ibin=nc*(i-1)-(i**2-i)/2+(j-i)
      hbin(ibin)='The binary parameter(s) for ('//hname(i)(1:ii)//'+'//
     &hname(j)(1:jj)//') have been modified from their original values.'
      if (nc.gt.1) call INITMIX

c  Check if the mixture model has changed for the binary pair, if so,
c   the coefficients for that model must be re-read.
      if (hmodij.ne.hmodMx(i,j)) then
        hmodMx(i,j)=hmodij
        call GETMIXCF (i,j,iMxRule(i,j),ierr,herr)
      endif

      end                                              subroutine SETKTV
c
c ======================================================================
c
      subroutine GETKTV (icomp,jcomp,hmodij,fij,hFmix,hfij,hbinp,hmxrul)
c
c  Retrieve mixture model and parameters for a specified binary mixture.
c  This subroutine should not be called until after SETUP has been called.
c  The order of icomp and jcomp do not matter, the routine returns the
c  parameters as stored in the HMX.BNC file.  To determine if the
c  compositions are backwards, call HMXORDER.  If calling SETMIX with
c  the same parameters, an error will be returned if the components
c  are backwards.
c
c
c  ========================   ==============================
c  Kunz-Wagner model (KW0)    Lemmon-Jacobsen model (LJ6)
c  ------------------------   ------------------------------
c  fij(1) = betaT             fij(1) = zeta
c  fij(2) = gammaT            fij(2) = xi
c  fij(3) = betaV             fij(3) = Fij
c  fij(4) = gammaV            fij(4) = beta
c  fij(5) = Fij               fij(5) = gamma
c  fij(6) = 'not used'        fij(6) = 'not used'
c  ========================   ==============================
c
c  Inputs:
c    icomp--Component i
c    jcomp--Component j
c
c  Outputs:
c   hmodij--Mixing rule for the binary pair i,j (e.g., KW0, LJ6, XR0, or LIN) (character*3)
c      fij--Binary mixture parameters (array of dimension nmxpar; currently
c           nmxpar is set to 6); the parameters will vary depending on hmodij;
c    hFmix--File name (character*255) containing parameters for the
c           binary mixture model
c     hfij--Description of the binary mixture parameters
c           (character*8 array of dimension nmxpar)
c           The parameters will vary depending on hmodij.
c    hbinp--Documentation for the binary parameters (character*255)
c   hmxrul--Description of the mixing rule (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  05-08-96  MM, original version
c  09-09-15 EWL, check for input pair given backwards from that in the HMX.BNC, and set beta=1/beta if so
c  06-22-16 EWL, shorten code

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character hmodij*3,hfij(nmxpar)*8
      character*255 hFmix,hmxrul,hbinp,herr,hIDi,hIDj
      dimension fij(nmxpar)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GETKTV

      hfij=' '
      if (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) then
        hmodij=heosmx
        fij(1:nmxprb)=fPRkij2(1:nmxprb,icomp,jcomp)
        hFmix=hmfile(0)
        hfij(1)='kij'
        hbinp=' '
        hmxrul=' '
        RETURN
      endif

      fij(1:4)=1d0
      fij(5:nmxpar)=0d0
      i=icomp
      j=jcomp
      if (i.gt.j) call SWAPI (i,j)
      if (i.lt.1 .or. i.gt.nc .or. j.lt.1 .or. j.gt.nc) goto 301

      ibin=0
      hbinp='trivial case-identical components'
c  Match input i,j with binary pair number.
      if (j.gt.i) ibin=nc*(i-1)-(i**2-i)/2+(j-i)
      if (ibin.ne.0) hbinp=hbin(ibin)(1:251)  !Old GUI combines hbinp with 3-letter code, so can use only 251 characters.

c  Get mixture parameters.
      irule=iMxRule(i,j)
      hmodij=hmodMx(i,j)
      hFmix=hmfile(ibin)
      if (irule.eq.0) goto 301
      hmxrul=hinfo_mxm(irule)(1:254)
      hfij(1:nmxpar)=hpar_mxm(1:nmxpar,irule)
      fij(1:nmxpar)=fmix(1:nmxpar,i,j)
      call HMXORDER (icomp,jcomp,hIDi,hIDj,iFlag,ierr,herr)
      im=imodmx(i,j)
      if (im.eq.iMxKW .or. im.eq.iMxXR .or. im.eq.iMxB) then
        if ((jcomp.gt.icomp .and. iFlag.eq.0) .or.
     &      (icomp.gt.jcomp .and. iFlag.ne.0)) then
          fij(1)=fmix2(1,i,j)
          fij(3)=fmix2(3,i,j)
        endif
      endif
      RETURN

 301  call ERRNUM (301,0,'GETKTV',' ',0d0,0d0,0d0,ierr,herr)

      end                                              subroutine GETKTV
c
c ======================================================================
c
      subroutine GETFIJ (hmodij,fij,hfij,hmxrul)
c
c  Retrieve parameter info for a specified mixing rule.
c
c  Input:
c   hmodij--Mixing rule for the binary pair i,j (e.g., LJ6 or KW0) (character*3)
c
c  Outputs:
c      fij--Binary mixture parameters (array of dimension nmxpar; currently nmxpar
c            is set to 6). The parameters will vary depending on hmodij.
c     hfij--Description of the binary mixture parameters
c           (character*8 array of dimension nmxpar)
c   hmxrul--Description of the mixing rule (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-10-01  MM, original version
c  06-22-16 EWL, shorten code

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character hmodij*3,hfij(nmxpar)*8,hmxrul*255,herr*255
      dimension fij(nmxpar)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GETFIJ

      do k=1,nrule_mxm
        if (hmodij.eq.htype_mxm(k)(1:3)) then
          hmxrul=hinfo_mxm(k)
          hfij(1:nmxpar)=hpar_mxm(1:nmxpar,k)
          fij(1:nmxpar)=fideal_mxm(1:nmxpar,k)
          RETURN
        endif
      enddo
      call ERRNUM (301,0,'GETFIJ',' ',0d0,0d0,0d0,ierr,herr)

      end                                              subroutine GETFIJ
c
c ======================================================================
c
      function PHIAGA (itau,idel,tau,del,z)
c
c  Compute reduced Helmholtz energy or its derivatives as functions
c  of dimensionless temperature and density for the AGA8
c  equation of state.
c
c  Based on the DETAIL compressibility factor equation of
c  Thermodynamic Properties of Natural Gas and Related Gases from the
c  DETAIL and GROSS Equations of State
c  AGA Report No. 8, Part 1, Transmission Measurement Committee,
c  American Gas Association, 2016.
c
c  Inputs:
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           when itau = 0 and idel = 0, compute A/RT
c           when itau = 0 and idel = 1, compute 1st density derivative
c           when itau = 1 and idel = 1, compute cross derivative
c           etc.
c      tau--Dimensionless temperature (To/T)
c      del--Dimensionless density (D/Do)
c           use subroutine REDX (z,T0,D0) to get reducing values
c
c  Output (as the function value):
c      phi--Residual (real-gas) part of the AGA8 equation, or one
c           of its derivatives (as specified by itau and idel),
c           in reduced form (A/RT)
c           itau  idel    output (dimensionless for all cases)
c             0    0      A/RT
c             1    0      tau    *[d(A/RT)/d(tau)]
c             2    0      tau**2 *[d**2(A/RT)/d(tau)**2]
c             0    1      del    *[d(A/RT)/d(del)]
c             0    2      del**2 *[d**2(A/RT)/d(del)**2]
c             1    1      tau*del*[d**2(A/RT)/d(tau)d(del)]
c                         etc.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-31-02 EWL, original version
c  01-20-15 EWL, remove AGA8 parameters out of COMMONS.INC and into a new file called COMAGA8.INC
c  06-14-16 EWL, update with new code, with equations given in AGA 8, 2016, Part 1

      include 'COMMONS.INC'
      parameter (ntrmaga=58)
      parameter (nbtrmaga=18)
      character*255 herr
      dimension z(ncmax),dbn(0:9),expcdk(0:4),ar(0:3,0:3),a(0:3)
      dimension nkaga(ntrmaga),nbaga(ntrmaga),unaga(ntrmaga)

      common /AGADBL2/ bekan(nbtrmaga),csan(ntrmaga),tuna(ntrmaga),
     &                 zaga(ncmax),ddk3,tagaold

      parameter (nkaga=(/0,0,0,0,0,0,0,0,0,0,0,0,3,2,2,2,4,4,0,0,2,2,
     &                   2,4,4,4,4,0,1,1,2,2,3,3,4,4,4,0,0,2,2,2,4,4,
     &                   0,2,2,4,4,0,2,0,2,1,2,2,2,2/))
      parameter (nbaga=(/1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,
     &                   2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,
     &                   5,5,5,5,5,6,6,7,7,8,8,8,9,9/))
      parameter (unaga=
     & (/0d0,0.5d0,1d0,3.5d0,-0.5d0,4.5d0,0.5d0,7.5d0,9.5d0,6d0,
     & 12d0,12.5d0,-6d0,2d0,3d0,2d0,2d0,11d0,-0.5d0,0.5d0,0d0,4d0,6d0,
     & 21d0,23d0,22d0,-1d0,-0.5d0,7d0,-1d0,6d0,4d0,1d0,9d0,-13d0,21d0,
     & 8d0,-0.5d0,0d0,2d0,7d0,9d0,22d0,23d0,1d0,9d0,3d0,8d0,23d0,1.5d0,
     & 5d0,-0.5d0,4d0,7d0,3d0,0d0,1d0,0d0/))

      lAGA8Flg=.true.
      zaga(1:nc)=z(1:nc)
      call SETAGA (ierr,herr)     !Check if composition has changed and set composition dependent variables if so.
      call REDX (z,T0,D0)
      T=T0/tau     !Recover T and D from the tau and del sent to the routine.
      D=D0*del

      if (ABS(T-tagaold).gt.1d-10) tuna(1:58)=T**(-unaga(1:58))
      tagaold=T

      dbn(0)=1
      do n=1,9
        dbn(n)=ddk3*D*dbn(n-1)
      enddo
      expcdk(0)=1
      expcdk(1:4)=EXP(-dbn(1:4))

      ar=0d0
      do n=1,58
        a=0d0
        b=0d0
        c=0d0
        bkd=0d0
        if (n.le.18) then
          b=bekan(n)*tuna(n)*D
          if (n.ge.13) b=b-csan(n)*tuna(n)*D*ddk3
        endif
        if (n.ge.13) then
          c=csan(n)*dbn(nbaga(n))*tuna(n)*expcdk(nkaga(n))
          bkd=DBLE(nbaga(n))-DBLE(nkaga(n))*dbn(nkaga(n))
          ckd=DBLE(nkaga(n))**2*dbn(nkaga(n))
          d2=bkd*(bkd-1d0)-ckd
          a(2)=c*d2
          a(3)=c*((bkd-2d0)*d2+ckd*(1d0-DBLE(nkaga(n))-2d0*bkd))
        endif
        a(0)=c+b
        a(1)=c*bkd+b
        ar(0,0:3)=ar(0,0:3)+a(0:3)
        ar(1,0:3)=ar(1,0:3)+a(0:3)*unaga(n)
        ar(2,0:3)=ar(2,0:3)+a(0:3)*unaga(n)*(unaga(n)-1d0)
      enddo
      PHIAGA=ar(itau,idel)

      end                                                function PHIAGA
c
c ======================================================================
c
      subroutine UNSETAGA
c
c  Load original values into arrays changed in the call to SETAGA.  This
c  routine resets the values back to those loaded when SETUP was called.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-07-10 EWL, original version

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*3 hagasv
      common /AGASAV/  Ragasv(nx),wmagasv(nx),tragasv(nx),
     &   Cragasv(nx),Tfagasv(nx),Dfagasv(nx),hfagasv(nx),
     &   sfagasv(nx),xksv(5,nx),xthsv(5,nx),xhsv(5,nx),
     &   cpcsv(5,nx),cphsv(5,nx),Rasv,nagacsv(nx),
     &   nagaesv(nx),ncoshsv(nx),nsinhsv(nx),hagasv
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::UNSETAGA

      call RESETA (1)
      zSpln=0d0
      if (iAGA8.eq.-1 .or. iAGA8.eq.2) then
        lAGA8=.false.
        iAGA8=0
        call RESETA (0)
        if (lAGA8sv) then
          lAGA8sv=.false.
          call SETEOSMX (hagasv)
c         Rxgas=Rasv
          Reos(1:nc)       = Ragasv(1:nc)
          wmas(1:nc)       = wmagasv(1:nc)
          TredCp0(1:nc)    = tragasv(1:nc)
          credCp0(1:nc)    = Cragasv(1:nc)
          nCp0poly(1:nc)   = nagacsv(1:nc)
          nCp0Exp(1:nc)    = nagaesv(1:nc)
          nCp0Cosh(1:nc)   = ncoshsv(1:nc)
          nCp0Sinh(1:nc)   = nsinhsv(1:nc)
          tref(1:nc)       = Tfagasv(1:nc)
          Dref(1:nc)       = Dfagasv(1:nc)
          href(1:nc)       = hfagasv(1:nc)
          sref(1:nc)       = sfagasv(1:nc)
          TiCp0 (1:5,1:nc) = xksv(1:5,1:nc)
          TiCp0h(1:5,1:nc) = xthsv(1:5,1:nc)
          csCp0h(1:5,1:nc) = xhsv(1:5,1:nc)
          aCp0  (1:5,1:nc) = cpcsv(1:5,1:nc)
          aCp0h (1:5,1:nc) = cphsv(1:5,1:nc)
        endif
      elseif (iAGA8.eq.1) then
        if (.not.lAGA8sv) then
          lAGA8sv=.true.
          if (heosmx.ne.'AGA') hagasv=heosmx
          Rasv=Rxgas
          Ragasv(1:nc)    = Reos(1:nc)
          wmagasv(1:nc)   = wmas(1:nc)
          tragasv(1:nc)   = TredCp0(1:nc)
          Cragasv(1:nc)   = credCp0(1:nc)
          nagacsv(1:nc)   = nCp0poly(1:nc)
          nagaesv(1:nc)   = nCp0Exp(1:nc)
          ncoshsv(1:nc)   = nCp0Cosh(1:nc)
          nsinhsv(1:nc)   = nCp0Sinh(1:nc)
          Tfagasv(1:nc)   = tref(1:nc)
          Dfagasv(1:nc)   = Dref(1:nc)
          hfagasv(1:nc)   = href(1:nc)
          sfagasv(1:nc)   = sref(1:nc)
          xksv (1:5,1:nc) = TiCp0 (1:5,1:nc)
          xthsv(1:5,1:nc) = TiCp0h(1:5,1:nc)
          xhsv (1:5,1:nc) = csCp0h(1:5,1:nc)
          cpcsv(1:5,1:nc) = aCp0  (1:5,1:nc)
          cphsv(1:5,1:nc) = aCp0h (1:5,1:nc)
          Reos=8.31451d0
        endif
      endif

      end                                            subroutine UNSETAGA
c
c ======================================================================
c
      subroutine SETAGA (ierr,herr)
c
c  Set up working arrays for use with AGA8 equation of state.
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      108 - Error (e.g. fluid not found)
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-31-02 EWL, original version
c  09-30-11 EWL, delete calculation of href and sref so that h=0 and s=0
c                at 298.15 K and 101.325 kPa

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      parameter (nmxaga=21)
      parameter (ntrmaga=58)
      parameter (nbtrmaga=18)
      character*255 herr
      dimension ixfpa(ncmax)
      dimension nbaga(ntrmaga),nkaga(ntrmaga),ngaga(ntrmaga)
      dimension nqaga(ntrmaga),nfaga(ntrmaga),nsaga(ntrmaga)
      dimension anaga(ntrmaga),dkiaga(nmxaga),eiaga(nmxaga)
      dimension fiaga(nmxaga),giaga(nmxaga),wmiaga(nmxaga)
      dimension qiaga(nmxaga),siaga(nmxaga)
      dimension wiaga(nmxaga),unaga(ntrmaga),nwaga(ntrmaga)
      dimension dkijaga(nmxaga,nmxaga),eijaga(nmxaga,nmxaga)
      dimension gijaga(nmxaga,nmxaga),uijaga(nmxaga,nmxaga)
      dimension cp0aga(nmxaga),cp0haga(nmxaga,4),thaga(nmxaga,4)
      common/AGADBL3/bekaij(nmxaga,nmxaga,nbtrmaga),dki25(nmxaga),
     &               dkkij5(nmxaga,nmxaga),ggij(nmxaga,nmxaga),
     &               eiaga25(nmxaga),zagaold(nx),giaga2(nmxaga),
     &               qiaga2(nmxaga),ueij5(nmxaga,nmxaga),fiaga2(nmxaga)
      common/AGADBL2/bekan(nbtrmaga),csan(ntrmaga),tuna(ntrmaga),
     &               zaga(ncmax),ddk3,tagaold
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETAGA

      parameter (nkaga=(/0,0,0,0,0,0,0,0,0,0,0,0,3,2,2,2,4,4,0,0,2,2,
     &                   2,4,4,4,4,0,1,1,2,2,3,3,4,4,4,0,0,2,2,2,4,4,
     &                   0,2,2,4,4,0,2,0,2,1,2,2,2,2/))
      parameter (ngaga=(/0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
     &                   0,0,1,0,0,0,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,
     &                   0,0,0,0,0,0,1,0,0,1,0,1,0,0/))
      parameter (nqaga=(/0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,
     &                   0,0,0,1,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,
     &                   0,0,1,0,1,0,0,1,0,0,0,0,0,1/))
      parameter (nbaga=(/1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,
     &                   2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,
     &                   5,5,5,5,5,6,6,7,7,8,8,8,9,9/))
      parameter (nfaga=(/0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,
     &                   0,0,0,0,1,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,
     &                   0,0,0,0,0,0,0,0,0,0,0,0,0,0/))
      parameter (nsaga=(/0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,
     &                   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
     &                   0,0,0,0,0,0,0,0,0,0,0,0,0,0/))
      parameter (nwaga=(/0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,
     &                   0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
     &                   0,0,0,0,0,0,0,0,0,0,0,0,0,0/))

      parameter (anaga=(/0.1538326d0,  1.341953d0,
     & -2.998583d0,   -0.04831228d0,   0.3757965d0,   -1.589575d0,
     & -0.05358847d0,  0.88659463d0,  -0.71023704d0,  -1.471722d0,
     &  1.32185035d0, -0.78665925d0,   2.29129D-9,     0.1576724d0,
     & -0.4363864d0,  -0.04408159d0,  -0.003433888d0,  0.03205905d0,
     &  0.02487355d0,  0.07332279d0,  -0.001600573d0,  0.6424706d0,
     & -0.4162601d0,  -0.06689957d0,   0.2791795d0,   -0.6966051d0,
     & -0.002860589d0,-0.008098836d0,  3.150547d0,     0.007224479d0,
     & -0.7057529d0,   0.5349792d0,   -0.07931491d0,  -1.418465d0,
     & -5.99905D-17,   0.1058402d0,    0.03431729d0,  -0.007022847d0,
     &  0.02495587d0,  0.04296818d0,   0.7465453d0,   -0.2919613d0,
     &  7.294616d0,   -9.936757d0,    -0.005399808d0, -0.2432567d0,
     &  0.04987016d0,  0.003733797d0,  1.874951d0,     0.002168144d0,
     & -0.6587164d0,   0.000205518d0,  0.009776195d0, -0.02048708d0,
     &  0.01557322d0,  0.006862415d0, -0.001226752d0,  0.002850908d0/))

      parameter (unaga=
     & (/0d0,0.5d0,1d0,3.5d0,-0.5d0,4.5d0,0.5d0,7.5d0,9.5d0,6d0,
     & 12d0,12.5d0,-6d0,2d0,3d0,2d0,2d0,11d0,-0.5d0,0.5d0,0d0,4d0,6d0,
     & 21d0,23d0,22d0,-1d0,-0.5d0,7d0,-1d0,6d0,4d0,1d0,9d0,-13d0,21d0,
     & 8d0,-0.5d0,0d0,2d0,7d0,9d0,22d0,23d0,1d0,9d0,3d0,8d0,23d0,1.5d0,
     & 5d0,-0.5d0,4d0,7d0,3d0,0d0,1d0,0d0/))

      parameter (wmiaga=
     & (/16.0430d0,28.0135d0,44.0100d0,30.0700d0,44.0970d0,58.1230d0,
     &   58.1230d0,72.1500d0,72.1500d0,86.1770d0,100.204d0,114.231d0,
     &   128.258d0,142.285d0,2.01590d0,31.9988d0,28.0100d0,18.0153d0,
     &   34.0820d0,4.00260d0,39.9480d0/))

      parameter (eiaga=(/
     & 151.318300d0,99.7377800d0,241.960600d0,244.166700d0,298.118300d0,
     & 324.068900d0,337.638900d0,365.599900d0,370.682300d0,402.636293d0,
     & 427.722630d0,450.325022d0,470.840891d0,489.558373d0,26.9579400d0,
     & 122.766700d0,105.534800d0,514.015600d0,296.355000d0,2.61011100d0,
     & 119.629900d0/))

      parameter (dkiaga=(/
     & 0.4619255d0,0.4479153d0,0.4557489d0,0.5279209d0,0.5837490d0,
     & 0.6406937d0,0.6341423d0,0.6738577d0,0.6798307d0,0.7175118d0,
     & 0.7525189d0,0.7849550d0,0.8152731d0,0.8437826d0,0.3514916d0,
     & 0.4186954d0,0.4533894d0,0.3825868d0,0.4618263d0,0.3589888d0,
     & 0.4216551d0/))

      parameter (giaga=(/
     & 0.d0,      0.027815d0,0.189065d0,0.079300d0,0.141239d0,.256692d0,
     & 0.281835d0,0.332267d0,0.366911d0,0.289731d0,0.337542d0,.383381d0,
     & 0.427354d0,0.469659d0,0.034369d0,0.021000d0,0.038953d0,.332500d0,
     & 0.088500d0,0.d0,0.d0/))

      parameter (qiaga=(/
     & 0d0,0d0,0.69d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,
     & 0d0,0d0,0d0,1.06775d0,0.633276d0,0d0,0d0/))

      parameter (siaga=(/
     & 0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,
     & 0d0,1.5822d0,0.39d0,0d0,0d0/))

      parameter (fiaga=(/
     & 0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,1d0,0d0,
     & 0d0,0d0,0d0,0d0,0d0/))

      parameter (wiaga=(/
     & 0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,
     & 0d0,1d0,0d0,0d0,0d0/))
c
c..Binary interaction parameters
c..eijaga, uijaga, dkijaga, and gijaga are transposed (so that it is easy to use as parameters)
      parameter (eijaga=reshape((/
     & 1d0,0.97164d0,0.960644d0,1d0,0.994635d0,1.01953d0,0.989844d0,      !Methane
     & 1.00235d0,0.999268d0,1.107274d0,0.88088d0,0.880973d0,0.881067d0,
     & 0.881161d0,1.17052d0,1d0,0.990126d0,0.708218d0,.931484d0,1d0,1d0,
     & 1d0,1d0,1.02274d0,0.97012d0,0.945939d0,0.946914d0,0.973384d0,      !Nitrogen
     & 0.95934d0,0.94552d0,1d0,1d0,1d0,1d0,1d0,1.08632d0,1.021d0,
     & 1.00571d0,0.746954d0,0.902271d0,1d0,1d0,
     & 1d0,1d0,1d0,0.925053d0,0.960237d0,0.906849d0,0.897362d0,           !CO2
     & 0.726255d0,0.859764d0,0.855134d0,0.831229d0,0.80831d0,0.786323d0,
     & 0.765171d0,1.28179d0,1d0,1.5d0,0.849408d0,0.955052d0,1d0,1d0,
     & 1d0,1d0,1d0,1d0,1.02256d0,1d0,1.01306d0,1d0,1.00532d0,1d0,1d0,     !Ethane
     & 1d0,1d0,1d0,1.16446d0,1d0,1d0,0.693168d0,0.946871d0,1d0,1d0,
     & (1d0,j=1,6),1.0049d0,(1d0,j=8,14),1.034787d0,(1d0,j=16,21),        !Propane
     &(1d0,j=1,14),1.3d0,(1d0,j=16,21),(1d0,j=1,14),1.3d0,(1d0,j=16,21),  !Butanes
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,18),1.008692d0,(1d0,j=20,21),   !Pentanes, Hexane
     & (1d0,j=1,18),1.010126d0,(1d0,j=20,21),(1d0,j=1,18),1.011501d0,     !Heptane, Octane
     & (1d0,j=20,21),(1d0,j=1,18),1.012821d0,(1d0,j=20,21),               !Octane, Nonane
     & (1d0,j=1,18),1.014089d0,(1d0,j=20,21),(1d0,j=1,16),1.1d0,          !Decane, Hydrogen
     & (1d0,j=18,21),(1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),              !Hydrogen
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21)/),(/21,21/)))

      parameter (uijaga=reshape((/
     & 1d0,0.886106d0,0.963827d0,1d0,0.990877d0,1d0,0.992291d0,1d0,       !Methane
     & 1.00367d0,1.302576d0,1.191904d0,1.205769d0,1.219634d0,1.233498d0,
     & 1.15639d0,1d0,1d0,1d0,0.736833d0,1d0,1d0,
     & 1d0,1d0,0.835058d0,0.816431d0,0.915502d0,1d0,0.993556d0,1d0,1d0,   !Nitrogen
     & 1d0,1d0,1d0,1d0,1d0,0.408838d0,1d0,1d0,1d0,0.993476d0,1d0,1d0,
     & 1d0,1d0,1d0,0.96987d0,1d0,1d0,1d0,1d0,1d0,1.066638d0,1.077634d0,   !CO2
     & 1.088178d0,1.098291d0,1.108021d0,1d0,1d0,0.9d0,1d0,1.04529d0,
     & 1d0,1d0,
     & 1d0,1d0,1d0,1d0,1.065173d0,1.25d0,1.25d0,1.25d0,1.25d0,1d0,1d0,    !Ethane
     & 1d0,1d0,1d0,1.61666d0,1d0,1d0,1d0,0.971926d0,1d0,1d0,
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),                            !Propane, Butanes
     & (1d0,j=1,21),(1d0,j=1,21),                                         !Pentanes
     & (1d0,j=1,18),1.028973d0,(1d0,j=20,21),                             !Hexane
     & (1d0,j=1,18),1.033754d0,(1d0,j=20,21),                             !Heptane
     & (1d0,j=1,18),1.038338d0,(1d0,j=20,21),                             !Octane
     & (1d0,j=1,18),1.042735d0,(1d0,j=20,21),                             !Nonane
     & (1d0,j=1,18),1.046966d0,(1d0,j=20,21),                             !Decane
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),               !All others
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21)/),(/21,21/)))

      parameter (dkijaga=reshape((/
     & 1d0,1.00363d0,0.995933d0,1d0,1.007619d0,1d0,0.997596d0,1d0,        !Methane
     & 1.002529d0,0.982962d0,0.983565d0,0.982707d0,0.981849d0,
     & 0.980991d0,1.02326d0,1d0,1d0,1d0,1.00008d0,1d0,1d0,
     & 1d0,1d0,0.982361d0,1.00796d0,1d0,1d0,1d0,1d0,1d0,1d0,1d0,1d0,1d0,  !Nitrogen
     & 1d0,1.03227d0,1d0,1d0,1d0,0.942596d0,1d0,1d0,
     & 1d0,1d0,1d0,1.00851d0,1d0,1d0,1d0,1d0,1d0,0.910183d0,0.895362d0,   !CO2
     & 0.881152d0,0.86752d0,.854406d0,1d0,1d0,1d0,1d0,1.00779d0,1d0,1d0,
     & 1d0,1d0,1d0,1d0,.986893d0,1d0,1d0,1d0,1d0,1d0,1d0,1d0,1d0,         !Ethane
     & 1d0,1.02034d0,1d0,1d0,1d0,0.999969d0,1d0,1d0,
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),                            !Propane, Butanes
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,18),0.96813d0,                  !Pentanes, Hexane
     & (1d0,j=20,21),(1d0,j=1,18),0.96287d0, (1d0,j=20,21),               !Hexane, Heptane
     & (1d0,j=1,18),0.957828d0,(1d0,j=20,21),(1d0,j=1,18),0.952441d0,     !Octane, Nonane
     & (1d0,j=20,21),(1d0,j=1,18),0.948338d0,(1d0,j=20,21),               !Nonane, Decane
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),               !All others
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21)/),(/21,21/)))

      parameter (gijaga=reshape((/
     & (1d0,j=1,2),.807653d0,(1d0,j=4,14),1.95731d0,(1d0,j=16,21),        !Methane
     & (1d0,j=1,2),0.982746d0,(1d0,j=4,21),                               !Nitrogen
     & (1d0,j=1,3),0.370296d0,(1d0,j=5,17),1.67309d0,(1d0,j=18,21),       !CO2
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),               !All others
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),
     & (1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),(1d0,j=1,21),
     & (1d0,j=1,21),(1d0,j=1,21)/),(/21,21/)))

      parameter (cp0aga=(/
     & 4.00088d0,3.50031d0,3.50002d0,4.00263d0,4.02939d0,4.06714d0,
     & 4.33944d0,4d0,4d0,4d0,4d0,4d0,4d0,4d0,2.47906d0,3.50146d0,
     & 3.50055d0,4.00392d0,4d0,2.5d0,2.5d0/))

      parameter (cp0haga=reshape((/
     & 0.0046d0,-0.1466d0,-1.06044d0,1.23722d0,3.197d0,5.25156d0,
     & 6.89406d0,20.1101d0,21.836d0,26.8142d0,30.4707d0,33.8029d0,
     & 38.1235d0,43.4931d0,0.45444d0,1.01334d0,0.00493d0,0.98763d0,
     & 1.00243d0,0d0,0d0,
     & -4.46921d0,0d0,0.01393d0,-6.01989d0,-8.37267d0,16.1388d0,
     & 14.7824d0,0d0,0d0,0d0,0d0,0d0,0d0,0d0,-1.3756d0,0d0,0d0,0d0,
     & 0d0,0d0,0d0,
     & 0.76315d0,0.13732d0,2.04452d0,4.33939d0,6.60569d0,8.97575d0,
     & 9.44893d0,11.7618d0,8.95043d0,11.6977d0,13.7266d0,15.6865d0,
     & 18.0241d0,21.0069d0,0.95806d0,1.07558d0,1.02865d0,0.01059d0,
     & 3.11942d0,0d0,0d0,
     & 8.74432d0,0.90066d0,2.03366d0,13.1974d0,19.1921d0,25.1423d0,
     & 24.4618d0,33.1688d0,33.4032d0,38.6164d0,43.5561d0,48.1731d0,
     & 53.3415d0,58.3657d0,1.56039d0,0d0,0d0,3.06904d0,0d0,0d0,0d0/),
     & (/nmxaga,4/)))

      parameter (thaga=reshape((/
     & 178.41d0,680.562d0,865.07d0,223.284d0,200.893d0,198.018d0,
     & 183.636d0,910.237d0,840.538d0,859.207d0,836.195d0,815.064d0,
     & 814.882d0,836.264d0,326.843d0,1116.69d0,704.525d0,1141.41d0,
     & 847.181d0,1d0,1d0,
     & 1090.53d0,1d0,341.109d0,1071.29d0,1027.29d0,893.765d0,903.185d0,
     & 1d0,1d0,1d0,1d0,1d0,1d0,1d0,1671.69d0,1d0,1d0,1d0,1d0,1d0,1d0,
     & 820.659d0,662.738d0,919.306d0,559.314d0,479.856d0,438.27d0,
     & 468.27d0,292.503d0,178.67d0,182.326d0,169.789d0,158.922d0,
     & 156.854d0,164.947d0,228.734d0,2235.71d0,1550.45d0,268.795d0,
     & 1833.63d0,1d0,1d0,
     & 1062.82d0,1740.06d0,483.553d0,1031.38d0,955.312d0,1905.02d0,
     & 1914.1d0,1919.37d0,1774.25d0,1826.59d0,1760.46d0,1693.07d0,
     & 1693.79d0,1750.24d0,1651.71d0,1d0,1d0,2507.37d0,1d0,1d0,1d0/),
     & (/nmxaga,4/)))

c...Setup parameters needed for the AGA8 equations.
      ierr=0
      herr=' '
      zSpln=0d0
      if (nc.le.1) RETURN

c  Check if GERG08 is active.
      if (iGERG08.ne.0) then
        call ERRNUM (809,0,'SETAGA',' ',0d0,0d0,0d0,ierr,herr)
        iAGA8=0
        RETURN
      endif


c...Setup AGA8 parameters that are composition dependent.
      if (lAGA8Flg) then     !This is only true when being called from PHIMIX to set the composition dependent parameters.
        lAGA8Flg=.false.     ! Otherwise this is skipped and the code much further down is called to initialize all variables.
        i=0
        do j=1,nc
          if (ABS(zaga(j)-zagaold(j)).gt.1d-10) i=1
        enddo
        zagaold(1:nc)=zaga(1:nc)
        if (i.eq.0) RETURN   !Exit if composition has not changed.

        g=0d0
        f=0d0
        q=0d0
        u=0d0
        dk5=0d0
        bekan=0d0

        do i=1,nc
          if (zaga(i).gt.0.) then
            x2=zaga(i)**2
            dk5=dk5+zaga(i)*dki25(i)
            g=g+zaga(i)*giaga2(i)
            f=f+x2     *fiaga2(i)
            q=q+zaga(i)*qiaga2(i)
            u=u+zaga(i)*eiaga25(i)
            bekan(1:18)=bekan(1:18)+x2*bekaij(i,i,1:18)
          endif
        enddo
        dk5=dk5**2
        u=u**2

        do i=1,nc-1
        if (zaga(i).gt.0.) then
          do j=i+1,nc
          if (zaga(j).gt.0.) then
            xixj2=2d0*zaga(i)*zaga(j)
            dk5=dk5+xixj2*dkkij5(i,j)
            u=u+xixj2*ueij5(i,j)
            g=g+xixj2*ggij(i,j)
            bekan(1:18)=bekan(1:18)+xixj2*bekaij(i,j,1:18)
          endif
          enddo
        endif
        enddo
        ddk3=dk5**0.6d0
        if (u.lt.0.) u=0d0
        u=u**0.2d0

        q=q**2
        do n=13,58
          csan(n)=anaga(n)*XPOWR(u,unaga(n))
          if (ngaga(n).eq.1) csan(n)=csan(n)*g
          if (nqaga(n).eq.1) csan(n)=csan(n)*q
          if (nfaga(n).eq.1) csan(n)=csan(n)*f
        enddo
        RETURN
      endif

c  Check if loaded fluids can be used in the AGA8 methods, otherwise exit with an error.
      tagaold=0d0
      zagaold=0d0
      ixfpa=1
      do i=1,nc
        if (iFluidID(i).eq.101) ixfpa(i)=1  !Methane
        if (iFluidID(i).eq. 16) ixfpa(i)=2  !Nitrogen
        if (iFluidID(i).eq. 31) ixfpa(i)=3  !Carbon Dioxide
        if (iFluidID(i).eq.102) ixfpa(i)=4  !Ethane
        if (iFluidID(i).eq.103) ixfpa(i)=5  !Propane
        if (iFluidID(i).eq.140) ixfpa(i)=6  !Isobutane
        if (iFluidID(i).eq.104) ixfpa(i)=7  !Butane
        if (iFluidID(i).eq.150) ixfpa(i)=8  !Isopentane
        if (iFluidID(i).eq.105) ixfpa(i)=9  !Pentane
        if (iFluidID(i).eq.106) ixfpa(i)=10 !Hexane
        if (iFluidID(i).eq.107) ixfpa(i)=11 !Heptane
        if (iFluidID(i).eq.108) ixfpa(i)=12 !Octane
        if (iFluidID(i).eq.109) ixfpa(i)=13 !Nonane
        if (iFluidID(i).eq.110) ixfpa(i)=14 !Decane
        if (iFluidID(i).eq. 12) ixfpa(i)=15 !Hydrogen
        if (iFluidID(i).eq. 11) ixfpa(i)=15 !Hydrogen (para)
        if (iFluidID(i).eq. 20) ixfpa(i)=16 !Oxygen
        if (iFluidID(i).eq. 17) ixfpa(i)=17 !Carbon Monoxide
        if (iFluidID(i).eq.  1) ixfpa(i)=18 !Water
        if (iFluidID(i).eq. 32) ixfpa(i)=19 !Hydrogen Sulfide
        if (iFluidID(i).eq. 10) ixfpa(i)=20 !Helium
        if (iFluidID(i).eq. 19) ixfpa(i)=21 !Argon

        if (iFluidID(i).eq. 26) ixfpa(i)=4  !Set ethylene as ethane.
        if (iFluidID(i).eq.230) ixfpa(i)=5  !Set propylene as propane.
        if (iFluidID(i).eq.240) ixfpa(i)=7  !Set butene as butane.
        if (iFluidID(i).eq.151) ixfpa(i)=8  !Set neopentane as isopentane.
        if (iFluidID(i).eq.260) ixfpa(i)=10 !Set benzene as hexane.
        if (iFluidID(i).eq.270) ixfpa(i)=11 !Set toluene as heptane.

        if (ixfpa(i).eq.0) then
          call ERRNUM (108,0,hname(i),' ',0d0,0d0,0d0,ierr,herr)
          RETURN
        endif
      enddo

      if (.not.lAGA8) then
        iAGA8=1              !Save the current state before loading the AGA8 coefficients.
        call UNSETAGA        !This call is not un-setting AGA8 because it is not currently loaded (lAGA8 is false).
      endif                  !The name of this function is kept however to be backwards compatible.

      lAGA8=.true.  !Set a flag to indicate that AGA8 is in use.  SETUP does not need to be called to activate or deactivate this model.
      iAGA8=2
      call SETEOSMX ('AGA')
      Rxgas=8.31451d0
      TredCp0(1:nc)=1d0
      credCp0(1:nc)=Reos(1:nc)
      nCp0poly(1:nc)=1
      nCp0Exp(1:nc)=0
      nCp0Cosh(1:nc)=2
      nCp0Sinh(1:nc)=2
      tref(1:nc)=298.15d0
      Dref(1:nc)=101.325d0/xRgas/tref(1:nc)
      href(1:nc)=0d0
      sref(1:nc)=0d0
      TiCp0(1,1:nc)=0d0
      TiCp0(2:5,1:nc)=-2d0
      TiCp0h(2:5,1:nc)=-1d0
      csCp0h(2:5,1:nc)=-2d0
      do i=1,nc
        wmas(i)=wmiaga(ixfpa(i))
        aCp0(1,i)=cp0aga(ixfpa(i))
c       aCp0(1:nc,2:5)=cp0haga(ixfpa(1:nc),1:4)*thaga(ixfpa(1:nc),1:4)**2
c       aCp0h(1:nc,2:5)=thaga(ixfpa(1:nc),1:4)
        aCp0(2:5,i)=cp0haga(ixfpa(i),1:4)*thaga(ixfpa(i),1:4)**2
        aCp0h(2:5,i)=thaga(ixfpa(i),1:4)
        dki25(i)=dkiaga(ixfpa(i))**2.5d0
        eiaga25(i)=eiaga(ixfpa(i))**2.5d0
      enddo

      do i1=1,nc
        do j1=i1,nc
          i=ixfpa(i1)
          j=ixfpa(j1)
          if (i.gt.j) then
            j=ixfpa(i1)
            i=ixfpa(j1)
          endif
          do n=1,18
            b=1d0
            if (ngaga(n).eq.1)
     &              b=gijaga(j,i)*(giaga(i)+giaga(j))/2d0
            if (nqaga(n).eq.1) b=b*qiaga(i)*qiaga(j)
            if (nfaga(n).eq.1) b=b*fiaga(i)*fiaga(j)
            if (nsaga(n).eq.1) b=b*siaga(i)*siaga(j)
            if (nwaga(n).eq.1) b=b*wiaga(i)*wiaga(j)
            bekaij(j1,i1,n)=anaga(n)*b*(eijaga(j,i)*
     &                      SQRT(eiaga(i)*eiaga(j)))**unaga(n)*
     &                      (dkiaga(i)*dkiaga(j))**1.5d0
            bekaij(i1,j1,n)=bekaij(j1,i1,n)
          enddo
          dkkij5(j1,i1)=(dkijaga(j,i)**5-1d0)*dki25(i1)*dki25(j1)
          ueij5 (j1,i1)=(uijaga(j,i)**5-1d0)*eiaga25(i1)*eiaga25(j1)
          ggij  (j1,i1)=(gijaga(j,i)-1d0)*(giaga(i)+giaga(j))/2d0
          dkkij5(i1,j1)=dkkij5(j1,i1)
          ueij5 (i1,j1)=ueij5 (j1,i1)
          ggij  (i1,j1)=ggij  (j1,i1)
        enddo
      enddo
      do i=1,nc
        giaga2(i)=giaga(ixfpa(i))
        fiaga2(i)=fiaga(ixfpa(i))
        qiaga2(i)=qiaga(ixfpa(i))
      enddo

      end                                              subroutine SETAGA
c
c ======================================================================
c
      block data BDAGA
c
c  Initialize variables in the common blocks.
c
      include 'COMMONS.INC'
      parameter (ntrmaga=58)
      parameter (nbtrmaga=18)
      common /AGADBL2/ bekan(nbtrmaga),csan(ntrmaga),tuna(ntrmaga),
     &                 zaga(ncmax),ddk3,tagaold
      data csan,tuna /ntrmaga*0d0,ntrmaga*0d0/
      data bekan /nbtrmaga*0d0/
      data zaga /ncmax*0d0/
      data ddk3,tagaold /2*0d0/
      end

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                   End file MIX_HMX.FOR
c ======================================================================
