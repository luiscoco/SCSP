c  Begin file SAT_SUB.FOR
c
c  This file contains routines for saturation properties.
c
c  Contained here are:
c     subroutine SATT    (T,z,kph,P,Dl,Dv,x,y,ierr,herr)
c     subroutine SATP    (P,z,kph,T,Dl,Dv,x,y,ierr,herr)
c     subroutine SATD    (D,z,kph,kr,T,P,Dl,Dv,x,y,ierr,herr)
c     subroutine SATE    (e,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
c     subroutine SATH    (h,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
c     subroutine SATS    (s,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,k3,T3,P3,D3,ierr,herr)
c     subroutine SATTP   (T,P,z,iFlsh,iGuess,D,Dl,Dv,x,y,q,ierr,herr)
c     subroutine SATSPLN (z,ierr,herr)
c     subroutine SATGV   (T,P,z,vf,b,ipv,ityp,isp,Dx,Dy,x,y,ierr,herr)
c     subroutine HEMAXX  (z,ieflag,hemax,Themax,Dhemax,ierr,herr)
c     subroutine SMAXX   (z,ierr,herr)
c     subroutine HVAPZTN (T,P,z,iTP,hvap,hvap0,ierr,herr)
c     subroutine CSATK   (icomp,T,kph,P,D,Csat,ierr,herr)
c     subroutine DPTSATK (icomp,T,kph,P,D,Csat,dPdT,ierr,herr)
c     subroutine CV2PK   (icomp,T,D,Cv2p,Csat,ierr,herr)
c     subroutine SATGUESS(kph,iprop,iFlag,ispl,x,T,P,D,h,s,Dy,y,ierr,herr)
c     subroutine SATEST  (iFlash,T,P,z,x,y,ierr,herr)
c     subroutine SAT0EST (T,P,z,x,y,ierr,herr)
c     subroutine PTEST   (inp,T,P,alpha,z,calc,zout,ierr,herr)
c     subroutine SATMAX  (icomp,z,Tcmax,Pcmax,Dcmax,ierr,herr)
c     subroutine FNCRPT  (z,D,P,T,ierr,herr)
c     subroutine CRTPNT  (z,Tc,Pc,Dc,ierr,herr)
c     subroutine REDLINEAR (iFlag,z,Tred,Pred,Dred,ierr,herr)
c
c
c ======================================================================
c ======================================================================
c
      subroutine SATT (T,z,kph,P,Dl,Dv,x,y,ierr,herr)
c
c  Iterate for saturated liquid and vapor states given temperature
c  and the composition of one phase.
c
c  Inputs:
c        T--Temperature [K]
c           If T is negative, all other variables are used as initial guesses at ABS(T).
c        z--Composition (array of mole fractions) (phase specified by kph)
c      kph--Phase flag:  1 - Input z is liquid composition (bubble point)
c                       -1 - Force calculation in the liquid phase even if T<Ttrp
c                        2 - Input z is vapor composition (dew point)
c                       -2 - Force calculation in the vapor phase even if T<Ttrp
c                        3 - Input z is liquid composition along the freezing line (melting line)
c                        4 - Input z is vapor composition along the sublimation line
c
c  Outputs:
c        P--Pressure [kPa]
c       Dl--Molar density of saturated liquid [mol/L]
c       Dv--Molar density of saturated vapor [mol/L]
c           For a pseudo pure fluid, the density of the equilibrium phase
c           is not returned.  Call SATT twice, once with kph=1 to get
c           Pliq and Dl, and once with kph=2 to get Pvap and Dv.
c        x--Liquid phase composition (array of mole fractions)
c        y--Vapor phase composition  (array of mole fractions)
c     ierr--Error flag:  0 - Successful
c                      121 - T > Tcrit
c                      124 - Pure fluid iteration did not converge
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  07-11-95  MM, original version
c  12-08-16 EWL, rewrite code for better efficiency and reading.
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),yOrg(ncmax),ys(ncmax)
      dimension f1(ncmax),f2(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATT


      if (iSatFlg1.gt.0) RETURN        !Don't do recursive calls.
      iErrNum=iErrNum+1
      sumx=1d0
      iexit=0
      ikph=0
      v3=0
      Dv=0d0
      Dl=0d0
      y(1:nc)=z(1:nc)
      if (lAGA8) call SETEOSMX (heox)   !Save the state of the AGA-8 flag and then turn it off so that the saturation pressures will come from the FEQ models.
      kph2=ABS(kph)
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      if (T.le.0.) goto 121

c  Check if melting or sublimation line requested and call appropriate routines.
      iw=0
      if (iFluidID(icomp).eq.1 .or. iFluidID(icomp).eq.3) then
       iw=iFluidID(icomp)
       if (icomp.ne.0 .and. kph.eq.1 .and. T+1d-8.lt.Ttpn(icomp)) kph2=3                 !Check for water and T<Ttrp
      endif
      if (kph2.eq.3) then    !Liquid/solid (melting line)
        Dl=Dtpn(icomp)
        ierr=999
        call MELTT (T,z,P,ierr,herr)
        if (icomp.eq.1 .and. iw.eq.1) call MLTH2O (T,pp,P)           !Check for water.
        if (icomp.eq.1 .and. iw.eq.3) call MLTD2O (T,pp,P)           !Check for heavy water.
        if (ignbnd.eq.1 .or. ignbnd2.eq.1) goto 10
        if (ierr.le.0) call TPRHO (T,P,z,1,1,Dl,ierr,herr)
        if (ierr.gt.0) Dl=0d0
        Dv=Dl
        goto 500
      endif
 10   if (kph.eq.4 .or. (icomp.ne.0 .and. kph.eq.2 .and.
     &    T+1d-6.lt.Ttpn(icomp))) then           !Vapor/solid (sublimation line)
        Dv=0d0
        call SUBLT (T,z,P,ierr,herr)
        if (ignbnd.eq.0 .and. ignbnd2.eq.0) then
          if (ierr.le.0) then
            call TPRHO (T,P,z,2,0,Dv,ierr,herr)
            if (ierr.le.0) ikph=1      !Sublimation calculation succeeded, do not do temperature checks.
            Dl=Dv
            goto 500
          endif
          if (kph.eq.4) goto 500
        endif
      endif

      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      if (T.gt.Tc+1d-8 .and. icomp.ne.0) goto 121          !T is above maxcondentherm for mixtures or above Tc for pure fluids.
      if (T.gt.Tc-1d-8 .and. icomp.ne.0) goto 199          !Input temperature is nearly equal to Tc, return critical parameters for P and D.
      herr=' '

      ierr=0
      call SATSAVE (1,icomp,kph,T,P,D,Dl,Dv,q,x,y,z,ierr,iexit)      !Check if this calculation has already been done for the input conditions.
      if (iexit.eq.1) GOTO 500

      tolr=1d-10
      itmax=25
      InitFlg=0
      kguess=1
      if (T.lt.0. .and. T.gt.-1d4) then          !To use the input parameters as initial guesses for mixtures, send a negative temperature to this routine.
        InitFlg=1
        T=ABS(T)
      else
        call SATGUESS (kph,1,0,0,z,T,P,Dl,h,s,Dv,x,ierr,herr)        !Get initial estimates.
        if (ierr.gt.0) kguess=0                  !Use values from SATGUESS as initial guesses to TPRHO.
        if (kph2.eq.2) call SWAPV (Dl,Dv)
        y(1:nc)=x(1:nc)
        if (iAnc(icomp).ne.0) goto 400           !Check for pseudo-pure fluid.
      endif
      fd=1
      if (T/Tc.lt.0.99d0) fd=1.2d0               !Very conservative estimates for the saturated density divided by the critical density (or vice versa).
      if (T/Tc.lt.0.95d0) fd=1.5d0

c  Check for mixture---------------------------------------
      if (icomp.eq.0) goto 200

      if (T.gt.0.99d0*Tc) goto 100     !Use alternative method for T close to Tc.
c  Call new Maxwell routine of Matthias Kunick.
c     call MAXWELL (icomp,T,P,Dl,Dv,ierr,herr)
c     if (ierr.le.0) goto 400



c
c --------------------Start pure fluid iteration--------------------
c  Iterate for saturated liquid and vapor states given temperature with the use of a simple
c   successive substitution method.  The independent variable in the iteration is the vapor pressure.
c   The convergence criteria is the equality of Gibbs free energy in both phases.
      Pdel2=1d6
      if (T/Tc.lt..5d0) itmax=itmax*2
      do it=1,itmax
        call TPRHO (T,P,z,1,kguess,Dl,ierr,herr)           !Get liquid density.
        if (ierr.gt.0 .or. Dl.le.Dc*fd) then               !Don't let the liquid density go more than Dc*fd into the 2-phase region.
          kguess=0
          P=P*1.01d0                   !Root not found, increase pressure at a continuing rate as the number of iterations continues.
          if (it.gt.7)  P=P*1.1d0
          if (it.gt.14) P=P*1.2d0
        else
          call TPRHO (T,P,z,2,kguess,Dv,ierr,herr)         !Get vapor density.
          call PRESS (T,Dv,z,pp)
          if (ierr.gt.0 .or. Dv.le.0. .or. Dv.gt.Dc/fd) then         !Don't let the vapor density go more than Dc/fd into the 2-phase region.
            kguess=0
            P=P*0.8d0
            if (it.gt.7)  P=P*0.7d0
            if (it.gt.14) P=P*0.5d0
          else
            kguess=1         !Use previous densities as initial guesses for calls to TPRHO after first iteration.
            call AG (T,Dl,z,Aliq,Gliq)
            call AG (T,Dv,z,Avap,Gvap)

c  Check for convergence.
            if (ABS(Dl-Dv).lt.1d-8) EXIT         !Check that liquid and vapor densities are different.
            Pdel=(Gliq-Gvap)/(1d0/Dl-1d0/Dv)
            if (ABS(Pdel).gt.0.4d0*P) Pdel=SIGN(0.4d0*P,Pdel)        !Check that Pdel will not result in negative [or very small] pressure.
            P=P-Pdel
            call TOLRNC (tolr,it,15,20,0,0,0)    !Loosen tolerance if not converging.
            if (P.lt.1d-5) call TOLRNC (tolr,it,18,23,0,0,0)
            if (ABS(Pdel/P).lt.tolr .or.
     &          ABS((Pdel-Pdel2)/Pdel2).lt.1d-11) then     !The Pdel-Pdel2 check is only important for very low pressures on the liquid surface (propane or R124).  See comments in TPRHO.
              call TPRHO (T,P,z,2,1,Dv,ierr,herr)          !Get vapor density.
              if (ierr.gt.0 .or. P.gt.Pc .or. Dv.gt.Dc) EXIT
              call TPRHO (T,P,z,1,1,Dl,ierr,herr)          !Get liquid density.
              D=Dl                     !Call THERM again to get exact P at T and Dl.  This is only important for very low pressures.
              if (P.lt.1.) D=Dv        !...(12-13-06 EWL)  Call THERM with Dv if P < 1 kPa.  This was put in place because some of the pressures
              call PRESS (T,D,z,P1)    !                   were coming back erratically from the PR model (for example, butane at T<160 K)
              if (ABS(P-P1)/P.gt.1d-5) call PRESS (T,Dv,z,P1)
              if (ABS(P-P1)/P.lt.1d-5) P=P1
              if (ierr.gt.0 .or. P.gt.Pc .or. Dl.lt.Dc) EXIT
              goto 400       !Iteration is complete, exit routine.
            endif
            Pdel2=Pdel
         endif
        endif
        if (P.gt.1d5) EXIT
      enddo
c  Iteration has not converged, go to alternative method.



c  Alternative method for finding saturation boundaries.  This routine work best near the
c   critical point by finding the vapor and liquid spinodals, which then bound the iteration.
 100  call LIQSPNDL (T,z,Dli,ierr1,herr)
      call VAPSPNDL (T,z,Dvi,ierr2,herr)
      if (Dli.le.0) Dli=Dc*1.5d0
      if (Dvi.le.0) Dvi=Dc/2d0
      if ((ierr1.le.100 .and. ierr2.le.100) .or. ABS(T-Tc).lt.1d-2) then

c  Get the spinodal pressures and find the densities in the opposite phase at those pressures.
c   The liquid pressure could be negative.  These new densities will then bound the iteration,
c   i.e., the liquid density will be between Dli and Dlj, and likewise for the vapor density.
        call PRESS (T,Dli,z,Pl)
        call PRESS (T,Dvi,z,Pv)
        call TPRHOB (T,Pv,Dli,Dli*2d0,z,Dlj,ierr,herr)     !Find density between Dli and Dli*2 at the pressure of the vapor side.
        Dvj=0d0
        if (Pl.gt.0.) call TPRHOB (T,Pl,0d0,Dvi,z,Dvj,ierr,herr)     !Same for the pressure of the liquid side, with the density in the vapor.

c  Find the saturation pressure.  Use the midpoint of the spinodal pressures for the
c   initial guess. Get both densities, and call AG to get the Gibbs energy.
        P=(Pl+Pv)/2d0
        if (P.lt.0.) P=Pv/2d0
        do it=1,20
          call TPRHOB (T,P,Dvi,Dvj,z,Dv,ierr,herr)
          if (ierr.gt.0 .or. Dv.lt.0.) then
            P=P*1.001d0
          else
            call TPRHOB (T,P,Dli,Dlj,z,Dl,ierr,herr)
            Pdel=P/10d0
            if (ierr.le.0 .and. Dl.gt.0.) then
              ddiv=1d0/Dl-1d0/Dv
              if (ABS(ddiv).gt.1d-20) then
                call AG (T,Dl,z,Aliq,Gliq)
                call AG (T,Dv,z,Avap,Gvap)
                Pdel=(Gliq-Gvap)/ddiv
                if (ABS(Pdel).gt.P/2d0) Pdel=SIGN(P/2d0,Pdel)
              endif
            endif
            P=ABS(P-Pdel)
            if (ABS(Pdel/P)*100d0.lt.tolr) goto 400        !Exit when solved.
          endif
        enddo
      endif
      goto 124     !Iteration has not converged.

c  End of pure fluid iteration.




c --------------------Start mixture iteration--------------------

c  Start by calling new routines that converge better.  If these fail, then
c   revert back to original method further below.
 200  if (iSpline.ne.0) then
        call MAXT (z,TmaxT,TmaxP,TmaxD,ierr,herr)
        if (ierr.eq.0 .and. T.gt.TmaxT+1d0 .and.
     &      MixTyp.ne.2 .and. MixTyp.ne.3) goto 121        !T is above maxcondentherm.
      endif
      iTcdiff=0
      do i=1,nc
        do j=1,nc
          if (ABS(Tcrit(i)-Tcrit(j)).gt.100.) iTcdiff=1
        enddo
      enddo

      if (icomp.eq.0 .and. iSatFlg.eq.0) then
        iSatFlg1=iSatFlg1+1            !A value of 0 for iSatFlg1 means it is safe to call SATT or SATP non-recursively
        call SATTP (T,P,z,kph2,0,D,Dl,Dv,x,y,q,ierr,herr)
        iSatFlg1=iSatFlg1-1
        if (ierr.eq.0 .and. kph2.eq.2 .and. Dl.lt.Dv*0.8d0) then
          call SATGV (T,P,z,1d0,d,1,1,1,Dl,Dv,x,y,ierr,herr)
        endif
        if (ierr.le.0) goto 400
      endif

c  Iterate for the pressure and the composition of the incipient phase (vapor phase
c   for a bubble point calculation, liquid for dew point) given temperature and the
c   composition of the parent phase.  Iteration is generally based on the algorithm
c   given by Smith and Van Ness (Intro to Chem. Engr. Thermo., McGraw-Hill, 1975);
c   convergence criteria is the equality of fugacity for each component in both phases.
      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.
      kgues1=1
      kgues2=1
      if (InitFlg.eq.0) then
        if (iSpline.gt.0) then
          call SATGUESS (kph2,1,0,1,z,T,P,Dl,h,s,Dv,x,ierr,herr)
          if (kph2.eq.2) call SWAPV (Dl,Dv)
          y(1:nc)=x(1:nc)
        else
          call SATEST (kph2,T,P,z,x,y,ierr,herr)           !Use Raoult's law as initial guesses.
          kgues1=0           !For first calls to TPRHO.
          kgues2=0
        endif
        if (P.gt.0.99d0*Pc) P=0.99d0*Pc
      endif
      if (kph2.eq.1) then
        kph2=2               !kph2 specifies whether the incipient phase (y) is liquid (1) or vapor (2).
        prtio1=1.05d0        !pratio is the multiplier for pressure to use when TPRHO does not converge
        prtio2=1.02d0        ! such that new guesses for pressure are further into the single-phase region.
      else
        kph2=1
        prtio1=0.95d0        !Choose different pratio for the dew and bubble sides to avoid loops.
        prtio2=0.98d0
        call SWAPV (Dl,Dv)
        call SWAPA (x,y)     !y is the incipient phase, not the vapor phase.
      endif
      tolr=1d-8
      yOrg(1:nc)=y(1:nc)
      v(2)=P
      jr=1
      ibad1=0
      ibad2=0
      tbad2=0d0

c  Begin main mixture iteration--Start outer loop for pressure with Raoult's Law results as first guess.
      do it=1,itmax
        call TOLRNC (tolr,it,8,12,15,0,0)        !Loosen tolerance if not converging.
        jkph=ABS(kph)
        if (jkph.le.2) jkph=-kph
        call TPRHO (T,P,z,jkph,kgues1,Dl,ierr,herr)        !Compute density and fugacities for parent phase.
        kgues1=1
        if (kph.eq.1) then
          if (Dl.lt.Dc/2d0) ierr=999
          if (Dl.lt.Dc*1.2d0 .and. it.eq.1 .and. InitFlg.eq.0) ierr=999
        endif
        if (ierr.gt.0) then
          ibad1=ibad1+1
          if(ibad1.lt.6.and.kph.eq.1)call DLDV(T,z,y,P,Dl,Dv,ierr,herr)
          if(ibad1.lt.6.and.kph.eq.2)call DLDV(T,y,z,P,Dv,Dl,ierr,herr)
          if (ierr.gt.0) then
            v(2)=v(2)*prtio1           !Try another pressure and use up one loop
            kgues1=0                   !Do not reuse faulty density as guess
            goto 240
          endif
          v(2)=P
          kgues2=1
        endif
        sum1=2d0
        iconvrg=0            !Flag for convergence of inner loop.
        call FGCTY2 (T,Dl,z,f1,ierr,herr)

c  Begin inner iteration loop for composition of phase 2.
        do itx=1,itmax*2
          ys(1:nc)=y(1:nc)
          call TPRHO (T,P,y,-kph2,kgues2,Dv,ierr,herr)     !Compute density of incipient phase.
          if (itx.gt.1 .and. ABS(Dv-Dc).lt.1 .and. ABS(Dl-Dc).gt.10)then
            Dv2=Dv/2d0       !Jump away from critical point when a bad root was found.
            call TPRHO (T,P,y,-kph2,1,Dv2,ierr,herr)
            if (ierr.le.0 .and. ABS(Dl-Dv).gt.0.1d0) Dv=Dv2
          endif
          kgues2=0
          if (ierr.le.0) kgues2=1
          itx1=0
 220      itx1=itx1+1
          if (ierr.gt.0 .or. ABS(Dl-Dv).lt.0.1d0) then
            ibad2=ibad2+1
            if (ibad2.lt.15) then
              if (ABS(Dl-Dv).lt.0.1d0 .and. iTcdiff.eq.1) then
                y(1:nc)=(tbad2*z(1:nc)+yOrg(1:nc))/(1d0+tbad2)
                tbad2=tbad2+0.2d0
              endif
              if (ABS(kph).eq.1) call DLDV (T,z,y,P,Dl,Dv,ierr,herr)
              if (ABS(kph).eq.2) call DLDV (T,y,z,P,Dv,Dl,ierr,herr)
              call FGCTY2 (T,Dl,z,f1,ierr,herr)
              v(2)=P
              if (ierr.gt.0) goto 124
            endif
            if (ierr.gt.0 .or. ABS(Dl-Dv).lt.0.1d0) then
              y(1:nc)=yOrg(1:nc)
              kgues2=0                 !Do not reuse faulty density as guess.
              v(2)=v(2)/prtio2         !Try another pressure and use up one iteration (to prevent an infinite loop).
              goto 240
            endif
          endif
          call FGCTY2 (T,Dv,y,f2,ierr,herr)
          call XYFUG (y,f1,f2,tolr,sum1,sumx,iconvrg,ierr,herr)
          if (iconvrg.gt.0) EXIT
          if (INT(itx/10)*10.eq.itx) y(1:nc)=(y(1:nc)+ys(1:nc))/2d0            !Occasionally average out the compositions on two successive iterations to stop bouncing.
        enddo

c  Outer loop has converged when the y's sum to one, i.e., when the fugacities of each component in each phase are equal.
        f(2)=1d0-sumx
        call DPDTX (T,Dv,y,dPdT)
        if ((dPdT.le.0. .or. dPdT.gt.1d+10) .and. sumx.gt.20) goto 124         !dp/dT should never be negative
        if (ABS(f(2)).lt.tolr .and. iconvrg.gt.0) goto 260           !Iteration has converged.
        v3=v(2)*sumx                   !New guess for bubble-point pressure.
        if (kph2.eq.2) v3=v(2)/sumx    !New guess for dew-point pressure.
        call ROOTSEARCH (jr,0,0,v,f,v3,it,ib,lneg,lpos)
        v(2)=v3
        jr=1234
 240    P=v(2)
      enddo
      goto 124     !Outer iteration loop has not converged.

c  Solution found, assign final compositions and densities for parent (z,Dl) and incipient (y,Dv) phases (Dl and Dv do not necessarily mean liquid and vapor here).
 260  ierr=0
      x(1:nc)=z(1:nc)
      if (kph.eq.2) call SWAPV (Dl,Dv)           !Dew point
      if (kph.eq.2) call SWAPA (x,y)
      if (Dv.gt.Dl*0.99d0 .and. ABS(x(1)-y(1)).lt.0.1d0) then
        if (T.lt.Tc) goto 124
        goto 121
      endif

c --------------------End of mixture iteration--------------------



 400  call SATSAVE (2,icomp,kph,T,P,D,Dl,Dv,q,x,y,z,ierr,iexit)      !Save the results if the calculation was successful.

 500  continue
      if (lAGA8) then   !Save the state of the AGA-8 flag and then turn it off so that the saturation pressures will come from the FEQ models.
        call SETEOSMX ('AGA')
        if (ierr.le.0) call TPRHO (T,P,y,2,1,Dv,ierr,herr)  !Recall TPRHO to get the density from the AGA8 equation, not from the full Refprop equations.
      endif
      iErrNum=iErrNum-1
      D=Dl
      if (kph.eq.2 .or. kph.eq.4) D=Dv
c  Check if limits have been exceeded, return that message if no other error has occurred.
      if (kph.gt.0 .and. ikph.eq.0)
     &    call CHECKLIMITS ('S1',T,D,P,z,ierr,herr)
      if (iErrNum.le.0 .and. iexit.eq.0) call ERRMSG (ierr,herr)

      RETURN

 121  call ERRNUM (121,0,'SATT',' ',T,Tc,0d0,ierr,herr)
      goto 199
 124  call ERRNUM (124,0,' ',' ',T,0d0,0d0,ierr,herr)
      goto 199
 199  P=Pc
      Dl=Dc
      Dv=Dc
      x(1:nc)=z(1:nc)
      if (kph.eq.2) call SWAPV (Dl,Dv)           !Dew point
      if (kph.eq.2) call SWAPA (x,y)
      goto 500

      end                                                subroutine SATT
c
c ======================================================================
c
      subroutine SATP (P,z,kph,T,Dl,Dv,x,y,ierr,herr)
c
c  Iterate for saturated liquid and vapor states given pressure
c  and the composition of one phase.
c
c  Inputs:
c        P--Pressure [kPa]
c           If T is negative, all other variables are used as initial guesses at ABS(T).
c        z--Composition (array of mole fractions) (phase specified by kph)
c      kph--Phase flag:  1 - Input z is liquid composition (bubble point)
c                        2 - Input z is vapor composition (dew point)
c                        3 - Input z is liquid composition (freezing point)
c                        4 - Input z is vapor composition (sublimation point)
c
c  Outputs:
c        T--Temperature [K]
c       Dl--Molar density of saturated liquid [mol/L]
c       Dv--Molar density of saturated vapor [mol/L]
c           For a pseudo pure fluid, the density of the equilibrium phase
c           is not returned.  Call SATP twice, once with kph=1 to get
c           Tliq and Dl, and once with kph=2 to get Tvap and Dv.
c        x--Liquid phase composition (array of mole fractions)
c        y--Vapor phase composition  (array of mole fractions)
c     ierr--Error flag:  0 - Successful
c                      141 - P > Pcrit
c                      144 - Pure fluid iteration did not converge
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  07-13-95  MM, original version
c  12-08-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character*255 herr,herr1
      dimension x(ncmax),y(ncmax),z(ncmax),yOrg(ncmax),ys(ncmax)
      dimension f1(ncmax),f2(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATP


      if (iSatFlg1.gt.0) RETURN        !Don't do recursive calls.
      iErrNum=iErrNum+1
      iexit=0
      ikph=0
      v3=0d0
      if (lAGA8) call SETEOSMX (heox)   !Save the state of the AGA-8 flag and then turn it off so that the saturation pressures will come from the FEQ models.

c  Check if melting or sublimation line requested and call appropriate routines.
      call ISPURE (z,icomp)
      if (kph.eq.3) then               !Liquid/solid (melting line)
        Dl=0d0
        call MELTP (P,z,T,ierr,herr)
        if (ierr.le.0) call TPRHO (T,P,z,1,1,Dl,ierr,herr)
        if (ierr.gt.0) Dl=0d0
        Dv=Dl
        goto 500
      elseif (kph.eq.4) then           !Vapor/solid (sublimation line)
        Dv=0d0
        call SUBLP (P,z,T,ierr,herr)
        if (ierr.le.0) then
          call TPRHO (T,P,z,2,0,Dv,ierr,herr)
        else
          if (icomp.ne.0)call SETPNTS(1,icomp,z,tt,pp,Dl,Dv,ierr1,herr1)       !If no sublimation line, get triple point vapor density.
          ierr=-ierr
          i=INDEX(herr,'.           ')
          if (i.eq.0) i=INDEX(herr,'           ')
          herr=herr(1:i-1)//'; triple-point vapor density returned.'
        endif
        if (ierr.le.0) ikph=1          !Sublimation calculation succeeded, do not do temperature checks.
        Dl=Dv
        goto 500
      endif

      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)                       !Get pure fluid critical point or mixture maximum P and T (but labeled here simply as Pc and Tc).
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      if (icomp.ne.0 .or. MixTyp.le.1) then
        if (P.gt.Pc+1d-4 .or. P.le.0.) goto 141            !P is above maxcondenbar for mixtures or above Pc for pure fluids.
        if (P.gt.Pc-1d-4 .and. icomp.ne.0) goto 199        !Input pressure is nearly equal to Pc, return critical parameters for T and D.
      endif
      herr=' '

      ierr=0
      call SATSAVE (1,icomp,kph+2,T,P,D,Dl,Dv,q,x,y,z,ierr,iexit)    !Check if this calculation has already been done for the input conditions.
      if (iexit.eq.1) goto 500

      tolr=1d-10
      itmax=35
      InitFlg=0
      kguess=1
      if (T.lt.0. .and. T.gt.-1d4) then          !To use the input parameters as initial guesses for mixtures, send a negative temperature to this routine.
        InitFlg=1
        T=ABS(T)
      else
        call SATGUESS (kph,2,0,0,z,T,P,Dv,h,s,Dl,x,ierr,herr)        !Get initial estimates.
        if (ierr.gt.0) kguess=0        !Use values from SATGUESS as initial guesses to TPRHO.
        if (kph.eq.1) call SWAPV (Dl,Dv)
        y(1:nc)=x(1:nc)
      endif
      fd=1
      if (P/Pc.lt.0.99d0) fd=1.1d0     !Very conservative estimates for the saturated density divided by the critical density (or vice versa).
      if (P/Pc.lt.0.95d0) fd=1.3d0

c  Check for mixture---------------------------------------
      if (icomp.eq.0) goto 200

c  Check for pseudo-pure fluid.
      if (iAnc(icomp).ne.0) then
        call TPRHO (T,P,z,kph,0,Dl,ierr,herr)
        Dv=Dl                !Don't return equilibrium phase density for a pseudo-pure fluid.
        goto 400
      endif




c --------------------Start pure fluid iteration--------------------
c  Iterate for saturated liquid and vapor states given pressure with the use of a simple
c   successive substitution method.  The independent variable in the iteration is the temperature.
c   The convergence criteria is the equality of Gibbs free energy in both phases.
      do it=1,itmax
        call TPRHO (T,P,z,1,kguess,Dl,ierr,herr)           !Get liquid density.
        if (ierr.gt.0 .or. Dl.le.Dc*fd) then     !Don't let the liquid density go more than Dc*fd into the 2-phase region.
          kguess=0
          T=T*0.995d0                  !Root not found, increase temperature at a continuing rate as the number of iterations continues.
          if (it.gt.7)  T=T*0.99d0
          if (it.gt.14) T=T*0.95d0
        else
          call TPRHO (T,P,z,2,kguess,Dv,ierr,herr)         !Get vapor density.
          if (ierr.gt.0 .or. Dv.le.0. .or. Dv.gt.Dc/fd) then         !Don't let the vapor density go more than Dc/fd into the 2-phase region.
            kguess=0
            T=0.9*T+0.1*Tc
            if (it.gt.7)  T=0.9*T+0.1*Tc
            if (it.gt.14) T=0.9*T+0.1*Tc
          else
            kguess=1         !Use previous densities as initial guesses for calls to TPRHO after first iteration.
            call THERM1 (T,Dl,z,pp,e,h,sliq,Gliq)
            call THERM1 (T,Dv,z,pp,e,h,svap,Gvap)

c  Check for convergence.
            if (ABS(sliq-svap).lt.1d-8) EXIT               !Check that liquid and vapor entropies are different.
            Tdel=(Gliq-Gvap)/(sliq-svap)
            if (ABS(Tdel).gt.0.2d0*T) Tdel=SIGN(0.2d0*T,Tdel)        !Check that Tdel will not result in a nonsense number.
            T=T+Tdel
            call TOLRNC (tolr,it,15,20,0,0,0)              !Loosen tolerance if not converging.
            if (ABS(Tdel/T).lt.tolr .and. T.le.Tc) then
              call TPRHO (T,P,z,2,1,Dv,ierr,herr)          !Get vapor density.
              if (ierr.gt.0 .or. Dv.gt.Dc) EXIT
              call TPRHO (T,P,z,1,1,Dl,ierr,herr)          !Get liquid density.
              if (ierr.gt.0 .or. Dl.lt.Dc) EXIT
              goto 400       !Iteration is complete, exit routine.
            endif
         endif
        endif
        if (T.gt.1d5) EXIT
        if (T.gt.Tc) T=Tc*0.95d0
        if (T.lt.Ttpn(icomp) .and. P.gt.Ptpn(icomp))       !Do not perform check if user asked for P<Ptrp.
     &      T=0.8d0*Ttpn(icomp)+0.2d0*Tc
      enddo
c  Iteration has not converged.

c  In case of failure, try calling SATT iteratively to find the saturated
c   temperature.  This takes advantage of the alternate method in SATT used
c   at temperatures very close to the critical point.
      T1=Tc*0.999d0
      call SATT (T1,z,kph,P1,Dl,Dv,x,y,ierr,herr)
      T=Tc*0.9995d0
      T2=T
      do it=1,20
        call SATT (T,z,kph,P2,Dl,Dv,x,y,ierr,herr)
        if (ABS(P2-P)/P.lt.tolr .and. ierr.eq.0) goto 400            !Converged
        if (ABS(P2-P1).lt.1d-12) EXIT
        T=T1-(P1-P)/(P2-P1)*(T2-T1)
        if (T.gt.Tc .and. T2.gt.T1) T=(T2+Tc)/2d0
        if (T.gt.Tc .and. T1.gt.T2) T=(T1+Tc)/2d0
        P1=P2
        T1=T2
        T2=T
      enddo
      goto 144     !Iteration has not converged.

c  End of pure fluid iteration.




c --------------------Start mixture iteration--------------------
c
c  Start by calling new routine that converges better.  If it fails, then
c   revert back to original method further below.
 200  if (iSpline.ne.0) then
        call MAXP (z,PmaxT,PmaxP,PmaxD,ierr,herr)
        if (ierr.eq.0 .and. P.gt.PmaxP+10d0 .and. MixTyp.le.1) goto 141   !P is above maxcondenbar.
      endif

      if (icomp.eq.0 .and. iSatFlg.eq.0) then
        iSatFlg1=iSatFlg1+1            !A value of 0 for iSatFlg1 means it is safe to call SATT or SATP non-recursively
        call SATTP (T,P,z,kph+2,0,D,Dl,Dv,x,y,q,ierr,herr)
        if (ierrPmx.gt.0.and.kph.eq.1.and.p.gt.5d4.and.ierr.ge.100) then
          call SATTP (T,P,z,kph+3,0,D,Dl,Dv,x,y,q,ierr,herr)
          if (ierr.ge.100) then
            iSatFlg1=iSatFlg1-1
            goto 500
          endif
        endif
        iSatFlg1=iSatFlg1-1
        if (ierr.le.0) goto 400
      endif

c  Iterate for the temperature and the composition of the incipient phase (vapor phase
c   for a bubble point calculation, liquid for dew point) given pressure and the
c   composition of the parent phase.  Iteration is generally based on the algorithm
c   given by Smith and Van Ness (Intro to Chem. Engr. Thermo., McGraw-Hill, 1977);
c   convergence criteria is the equality of fugacity for each component in both phases.
      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.
      kgues1=1
      kgues2=1
      if (InitFlg.eq.0) then
        if (iSpline.gt.0) then
          call SATGUESS (kph,2,0,1,z,T,P,Dl,h,s,Dv,x,ierr,herr)
          if (kph.eq.2) call SWAPV (Dl,Dv)
          y(1:nc)=x(1:nc)
        else
          call SATEST (kph+2,T,P,z,x,y,ierr,herr)          !Use Raoult's law as initial guesses.
          kgues1=0           !For first calls to TPRHO.
          kgues2=0
        endif
        if (T.gt.0.99d0*Tc) T=0.99d0*Tc
      endif
      if (kph.eq.1) then
        kph2=2               !kph2 specifies whether the incipient phase (y) is liquid (1) or vapor (2).
        trtio1=0.995d0       !tratio is the multiplier for temperature to use when TPRHO does not converge
        trtio2=0.988d0       ! such that new guesses for temperature are further into the single-phase region.
      else
        kph2=1
        trtio1=1.005d0       !Choose different tratio for the dew and bubble sides to avoid loops.
        trtio2=1.012d0
        if (Dv.lt.Dl) then
          call SWAPV (Dl,Dv)
          call SWAPA (x,y)   !y is the incipient phase, not the vapor phase.
        endif
      endif
      tolr=1d-8
      yOrg(1:nc)=y(1:nc)
      v(2)=T
      jr=1

c  Begin main mixture iteration--Start outer loop for temperature with Raoult's Law results as first guess.
      do it=1,itmax
        call TOLRNC (tolr,it,8,12,15,0,0)             !Loosen tolerance if not converging.
        call TPRHO (T,P,z,-kph,kgues1,Dl,ierr,herr)   !Compute density and fugacities for parent phase.
        kgues1=1
        if (ABS(Dl-Dc).lt.1 .and. ABS(Dv-Dc).gt.4) then
          Dl=Dl*1.5d0
          call TPRHO (T,P,z,-kph,1,Dl,ierr,herr)
        endif
        if (kph.eq.1) then
          if (Dl.lt.Dc/2d0) ierr=999
          if (Dl.lt.Dc*1.2d0 .and. it.eq.1 .and. InitFlg.eq.0) ierr=999
        endif
        if (ierr.gt.0) then
          v(2)=T*trtio1      !Try another temperature and use up one iteration (to prevent infinite loop)
          kgues1=0           !Do not reuse faulty density as guess
          goto 240
        endif
        sum1=2d0
        iconvrg=0            !Flag for convergence of inner loop.
        call FGCTY2 (T,Dl,z,f1,ierr,herr)

c  Begin inner iteration loop for composition of phase 2.
        do itx=1,itmax*2
          ys(1:nc)=y(1:nc)
          call TPRHO (T,P,y,-kph2,kgues2,Dv,ierr,herr)     !Compute density of incipient phase.
          kgues2=1
          if (ierr.gt.0 .or. ABS(Dl-Dv).lt.0.01d0 .or.
     &       (Dv.lt.0.01d0 .and. kph2.eq.1)) then
            y(1:nc)=yOrg(1:nc)
            v(2)=T/trtio2    !Try another temperature and use up one iteration (to prevent infinite loop).
            kgues2=0         !Do not reuse faulty density as guess.
            goto 240
          elseif (Dv.gt.1.2*Dmax) then
            Dv=Dmax
            call TPRHO (T,P,y,-kph2,1,Dv,ierr,herr)
          endif
          if (ierr.gt.0) kgues2=0
          call FGCTY2 (T,Dv,y,f2,ierr,herr)
          call XYFUG (y,f1,f2,tolr,sum1,sumx,iconvrg,ierr,herr)
          if (iconvrg.gt.0) EXIT
          if (INT(itx/10)*10.eq.itx) y(1:nc)=(y(1:nc)+ys(1:nc))/2d0  !Occasionally average out the compositions on two successive iterations to stop bouncing.
        enddo

c  Outer loop has converged when the y's sum to one, i.e., when the fugacities of each component in each phase are equal.
        f(2)=1d0-sumx
        call DPDTX (T,Dv,y,dPdT)
        if (dPdT.le.0.) goto 144                                     !dp/dT should never be negative
        if (ABS(f(2)).lt.tolr .and. iconvrg.gt.0) goto 260           !Iteration has converged.
        v3=v(2)-Tc/100d0     !Next guess for first iteration.
        call ROOTSEARCH (jr,0,0,v,f,v3,it,ib,lneg,lpos)
        v(2)=v3
        jr=1234
 240    T=v(2)
      enddo
      goto 144     !Outer iteration loop has not converged.

c  Solution found, assign final compositions and densities for parent (z,Dl) and incipient (y,Dv) phases (Dl and Dv do not necessarily mean liquid and vapor here).
 260  ierr=0
      x(1:nc)=z(1:nc)
      if (kph.eq.2) call SWAPV (Dl,Dv)           !Dew point
      if (kph.eq.2) call SWAPA (x,y)
      if (Dv.gt.Dl*0.99d0 .and. ABS(x(1)-y(1)).lt.0.1d0) then
        if (P.lt.Pc) goto 144
        goto 141
      endif

c --------------------End of mixture iteration--------------------





 400  call SATSAVE (2,icomp,kph+2,T,P,D,Dl,Dv,q,x,y,z,ierr,iexit)    !Save the results if the calculation was successful.

 500  continue
      if (lAGA8) then   !Save the state of the AGA-8 flag and then turn it off so that the saturation pressures will come from the FEQ models.
        call SETEOSMX ('AGA')
        if (ierr.le.0) call TPRHO (T,P,y,2,1,Dv,ierr,herr)  !Recall TPRHO to get the density from the AGA8 equation, not from the full Refprop equations.
      endif
      iErrNum=iErrNum-1
      D=Dl
      if (kph.eq.2 .or. kph.eq.4) D=Dv
c  Check if limits have been exceeded, return that message if no other error has occurred.
      if (kph.gt.0 .and. ikph.eq.0)
     &    call CHECKLIMITS ('S1',T,D,P,z,ierr,herr)
      if (iErrNum.le.0 .and. iexit.eq.0) call ERRMSG (ierr,herr)
      RETURN

 141  call ERRNUM (141,0,'SATP',' ',P/1000d0,Pc/1000d0,0d0,ierr,herr)
      goto 199
 144  call ERRNUM (144,0,' ',' ',P/1000d0,0d0,0d0,ierr,herr)         !Don't change error number, used elsewhere.
      goto 199
 199  T=Tc
      Dl=Dc
      Dv=Dc
      goto 500


      end                                                subroutine SATP
c
c ======================================================================
c
      subroutine SATD (D,z,kph,kr,T,P,Dl,Dv,x,y,ierr,herr)
c
c  Iterate for temperature and pressure given density along the
c  saturation boundary (including the sublimation and melting
c  lines) and the composition.
c
c  Either (Dl,x) or (Dv,y) will correspond to the input state with
c  the other pair corresponding to the other phase in equilibrium
c  with the input state.
c
c  The flag kph is for use only with water at densities near the
c  triple point (between 0 and 4 C).
c
c  Inputs:
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c      kph--Flag specifying desired root for multi-valued inputs (typically only water):
c         0,1 - Return upper root
c          -1 - Return middle root
c           3 - Return melting line
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c       Dl--Molar density of saturated liquid [mol/L]
c       Dv--Molar density of saturated vapor [mol/L]
c        x--Liquid phase composition (array of mole fractions)
c        y--Vapor phase composition  (array of mole fractions)
c       kr--Phase flag:  1 - Input state is liquid in equilibrium with vapor.
c                        2 - Input state is vapor in equilibrium with liquid.
c                        3 - Input state is liquid in equilibrium with solid. (only for pure fluids)
c                        4 - Input state is vapor in equilibrium with solid.  (only for pure fluids)
c     ierr--Error flag:  0 - Successful
c                        2 - D>Dtrp of the liquid
c                        3 - D<Dtrp of the vapor
c                      160 - SATD did not converge
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  11-22-99  MM, original version
c  12-04-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATD

      call ISPURE (z,icomp)
      if (iSpline.ne.0 .and. icomp.eq.0) then
        call SATGV (T,P,z,0d0,D,1,6,1,Dl,Dv,x,y,ierr,herr)           !Call the more efficient SATGV if the splines have been calculated for a mixture.
        if (ierr.le.100) then
          if (Dv.gt.Dl) then
            call SWAPV (Dl,Dv)
            call SWAPA (x,y)
          endif
          RETURN
        endif
      endif
      D2=0
      iw=0
      ierr=0
      icnt=0
      iSatFlg=0
      tolr=1d-6
      v3=0
c     Dwatermax=55.504316178d0
      iErrNum=iErrNum+1
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)   !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS  ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      if (icomp.ne.0) call SETPNTS (0,icomp,z,T,P,Dl,Dv,ierr,herr)
      if (lAGA8) call SETEOSMX (heox)   !Save the state of the AGA-8 flag and then turn it off so that the saturation pressures will come from the FEQ models.

c  Determine region.
      ki=0
      kr=2                                                           !Vapor
      if (D.ge.Dc) kr=1                                              !Liquid
      if (icomp.ne.0) then
        if (D.lt.Dtpv(icomp)) kr=4                                   !Vapor/Solid
        if (D.gt.Dtpn(icomp) .and. Dtpn(icomp).gt.Dc) kr=3           !Liquid/Solid
        if (iFluidID(icomp).eq.1 .or. iFluidID(icomp).eq.3) then               !Check if pure water.
          iw=iFluidID(icomp)
          Dm=DmaxWater
          if (iFluidID(icomp).eq.3) Dm=DmaxD2O
          if (kph.ne. 3 .and. D.gt.Dc          .and. D.le.Dm) kr=1   !Upper temperature root for liquid water.
          if (kph.eq.-1 .and. D.gt.Dtpn(icomp) .and. D.le.Dm) ki=1   !Lower temperature root between 0 and 4 K.
        endif
        if (ABS(Dc-D).lt.1d-3) goto 199          !Nearly the same as the critical density for a pure fluid, just exit.
      endif
      if (D.le.0.) goto 103            !Returns with kr=2 (or kr=4 if a sublimation line is available).

c  Get a guess for T at the input D.
      call SATGUESS (kr,3,0,0,z,T,P,D,h,s,Dy,y,ierr,herr)
      if (T.gt.Tc*1.5d0 .and. P.gt.Pc*10d0) icnt=1         !For Type III mixtures, add up number of errors
      if(ierr.gt.0.and.(ABS(T).gt.Tc.or.ABS(T).lt.Tmin))T=(Tmin+Tc)/2d0
      if (ABS(T).lt.Tmin) T=(ABS(T)+Tmin)/2d0
      if (ki.eq.1) T=Ttpn(icomp)+2d0   !Find root between 0 and 4 K.

      if (icomp.eq.0.or.kr.gt.2.or.ki.eq.1.or.iAnc(icomp).ne.0) goto 200       !Use alternate method for mixtures or states not vapor or liquid.

c  Begin VLE iteration for pure fluids.
      d1=0.001d0
      T2=0d0
      stp1=1.0001d0
      stp2=1.0001d0
      tolr=1d-8
      do it=1,40
        call PRESS (T,D,z,P)           !Calculate pressure, if less than zero, increase T and try again.
        if (P.gt.1.d15) goto 160
        ierr=0
        if (it.lt.5) call CHECKSTATE (0,0,T,D,z,Err,ierr,herr)       !Check the state of the inputs for the first 4 iterations.
        if (P.lt.0 .or. ierr.gt.0) then
          T=T*stp1
          if (T.gt.Tc) then
            stp1=SQRT(stp1)
            T=T/SQRT(stp1)
          else
            stp1=1d0+(stp1-1d0)*1.5d0
          endif
        else
          call TPRHO (T,P,z,3-kr,0,D2,ierr,herr)           !Calculate density, if error, decrease T and try again.
          if (ABS(D-D2).lt.1d-7 .and. icomp.ne.0) then
            if (T.gt.Tc) T=Tc-ABS(Tc-T)
            if (D.gt.Dc) call DVSATK (icomp,T,D2,ierr,herr)
            if (D.le.Dc) call DLSATK (icomp,T,D2,ierr,herr)
          endif
          if (ierr.ne.0) then
            T=T/stp2
            stp2=1d0+(stp2-1d0)*2d0
          else
            if (ABS(D-D2).lt.1d-7) then
              stp1=SQRT(stp1)          !Back up if state was found in wrong phase.
              T=T/stp1
            else
              stp1=1.0001d0
              stp2=1.0001d0
              Dl=D
              Dv=D2
              if (kr.ne.1) call SWAPV (Dl,Dv)
              call THERM1 (T,D ,z,pp,e,h,s1,g1)            !Use Maxwell criterion to generate next guess for T.
              call THERM1 (T,D2,z,pp,e,h,s2,g2)
              if (T2.le.0.) T2=T+0.00001d0
              dd=d1
              ff=1d0
              if (ABS(s1-s2).gt.1d-11) d1=(g1-g2)/(s1-s2)
              if (ABS(d1-dd).gt.1d-11) ff=-(T2-T)/(dd-d1)
              T2=T
              T=T+ff*d1
              if (T.le.0.) T=T2*0.95d0
              if (ABS(d1).le.tolr .and. T.lt.Tc) goto 500            !Iteration has converged.
            endif
          endif
        endif
      enddo
      call SATGUESS (kr,3,0,0,z,T,P,D,h,s,Dy,y,ierr,herr)  !Recalculate initial guesses.
c  Method failed (generally at very low temperatures in the liquid), try alternate method.



c  Iterate for temperature with a combination of Newton's method and Reguli-falsi.
 200  ierr=0
      if (icomp.ne.0 .and. ignbnd.eq.0 .and. ignbnd2.eq.0) then
        if (hmeltk(icomp).eq.'NBS' .and. D.gt.Dtpn(icomp)) goto 102  !No melting line.
        if (hsublk(icomp).eq.'NBS' .and. D.lt.Dtpv(icomp)) goto 103  !No sublimation line.
      endif
      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.
      tolr=1d-7

c  Get initial point.
      if (kr.eq.3) then
        T=Ttpn(icomp)
        D1=Dtpn(icomp)
        if (iw.ge.1) T=Tmin+1d-10      !Calculate P and D at slightly higher than the lowest possible temperature for water.
        call MELTT (T,z,P,ierr,herr)                       !Call melting routine in case Dtpn(2) is not exactly eq. to D(Ttrp).
        call TPRHO (T,P,z,1,1,D1,ierr,herr)
      elseif (kr.eq.4) then
        T=Ttpn(icomp)
        D1=Dtpv(icomp)
      else
        do i=1,5
          call SATT (T,z,kr,P,Dl,D1,x,y,ierr,herr)         !For mixtures at Tc in the vapor phase, the value of Dc is not correct.
          if (icnt.gt.0 .and. ierr.ge.100) icnt=icnt+1
          if (ierr.lt.100) EXIT
          T=T*0.999d0
        enddo
        if (kr.eq.1) D1=Dl
      endif
      v(2)=T
      f(2)=XLOG(XDIV(D,D1))
      call ROOTSEARCH (1,0,0,v,f,v3,0,ib,lneg,lpos)

c  Generate second guess for temperature.
      if (kr.eq.1 .or. kr.eq.2) then
        if (icomp.eq.0) T=T*0.95d0
        if (icomp.ne.0) T=T-(Tc-T)/10d0
        if (icomp.ne.0 .and. T.lt.Tmin) T=Tmin*1.1d0
      endif
      if (kr.eq.3) T=Ttpn(icomp)*1.05d0
      if (kr.eq.4) T=Ttpn(icomp)*0.95d0
      if (iw.ge.1 .and. kr.eq.3) T=Ttpn(icomp)*0.99d0
      v(2)=T

c  Main loop.
      Told=0d0
      Pold=0d0
      do it=1,25
        call TOLRNC (tolr,it,10,15,20,0,0)                 !Loosen tolerance if not converging.
        if (kr.eq.3) then                                  !Melting line.
          call MELTT (T,z,P,ierr,herr)
          if (iw.eq.1 .and. D.lt.D1) call MLTH2O (T,pp,P)  !Check for water.
          if (iw.eq.3 .and. D.lt.D1) call MLTD2O (T,pp,P)  !Check for heavy water.
          if (P.le.0.) then
            ierr=999
          else
            dd=Dtpn(icomp)
            if (iw.ge.1 .and. D.gt.D1) dd=D1+5d0
            call TPRHO (T,P,z,1,1,dd,ierr,herr)            !Get density on melting line.
            Dl=dd
            Dv=dd
          endif
        else
          call SATT (T,z,kr,P,Dl,Dv,x,y,ierr,herr)
          if (icnt.gt.0 .and. ierr.ge.100) icnt=icnt+1
          if (icnt*nc.gt.100) goto 160           !Exit if too many errors occur, taking into account the number of components (more components means less allowable errors).
          dd=Dv
          if (kr.eq.1) dd=Dl
          if (ierr.eq.0) then
            dpdt=ABS(XDIV(p-pold,T-Told))        !Get dP/dT along the saturation line.  If very high, and not converging, search as a function of P instead of T (in the code below this section starting with the number 250).
            Told=T
            pold=p
            if(dpdt.gt.1000 .and. it.gt.10 .and. f(2).gt.0.01d0)goto 250
          endif
        endif
        if (ierr.ne.0 .or. dd.le.0.) then
          T=0.8d0*v(1)+0.2d0*v(2)      !Calculation failed, try point that is 80% of the good value and 20% of the bad one.
        else
          f(2)=XLOG(D/dd)
          call ROOTSEARCH (1234,0,0,v,f,T,it,ib,lneg,lpos)           !Get next guess.
          if (ABS(f(2)).lt.tolr) goto 500                  !Check for convergence.
          if (T.le.0.) T=Tmin
        endif
        v(2)=T
      enddo
c     goto 160     !Iteration failed.

c  Iterate for pressure with a combination of Newton's method and Reguli-falsi.
 250  continue
      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.
      tolr=1d-7

c  Get initial point.
      P=Pc/5d0
      do i=1,5
        call SATP (P,z,kr,T,Dl,D1,x,y,ierr,herr)
        if (ierr.lt.100) EXIT
        P=P*1.5d0
      enddo
      if (kr.eq.1) D1=Dl
      v(2)=P
      f(2)=XLOG(XDIV(D,D1))
      call ROOTSEARCH (1,0,0,v,f,v3,0,ib,lneg,lpos)

c  Generate second guess for temperature.
      P=P-(Pc-P)/10d0
      v(2)=P

c  Main loop.
      do it=1,25
        call TOLRNC (tolr,it,10,15,20,0,0)       !Loosen tolerance if not converging.
        call SATP (P,z,kr,T,Dl,Dv,x,y,ierr,herr)
        if (icnt.gt.0 .and. ierr.ge.100) icnt=icnt+1
        if (icnt*nc.gt.100) goto 160
        dd=Dv
        if (kr.eq.1) dd=Dl
        if (ierr.gt.100 .or. dd.le.0.) then
          P=0.8d0*v(1)+0.2d0*v(2)      !Calculation failed, try point that is 80% of the good value and 20% of the bad one.
        else
          f(2)=XLOG(D/dd)
          call ROOTSEARCH (1234,0,0,v,f,P,it,ib,lneg,lpos)           !Get next guess.
          if (ABS(f(2)).lt.tolr) goto 500                  !Check for convergence.
        endif
        v(2)=P
      enddo
      goto 160     !Iteration failed.

 102  call ERRNUM (2,4,'SATD',' ',D,Dtpn(icomp),0d0,ierr,herr)
      goto 199
 103  call ERRNUM (2,2,'SATD',' ',D,Dtpv(icomp),0d0,ierr,herr)
      goto 199
 160  call ERRNUM (160,0,' ',' ',D,0d0,0d0,ierr,herr)
      goto 199
 199  T=Tc
      P=Pc
      Dl=Dc
      Dv=Dc
      if (kr.eq.3) then
        T=Ttpn(icomp)
        P=Ptpn(icomp)
        Dl=Dtpn(icomp)
        Dv=Dtpv(icomp)
      endif

 500  continue
      if (lAGA8) then   !Save the state of the AGA-8 flag and then turn it off so that the saturation pressures will come from the FEQ models.
        call SETEOSMX ('AGA')
        if (ierr.le.0) call TPRHO (T,P,y,2,1,Dv,ierr,herr)  !Recall TPRHO to get the density from the AGA8 equation, not from the full Refprop equations.
      endif
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS ('S3',T,D,P,z,ierr,herr)
      RETURN

      end                                                subroutine SATD
c
c ======================================================================
c
      subroutine SATH (h,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
c
c  Iterate for temperature, pressure, and density given enthalpy along
c  the saturation boundary and the composition.
c
c  The second root is always set as the root in the vapor at temperatures
c  below the maximum enthalpy on the vapor saturation line.  If kph is
c  set to 2, and only one root is found in the vapor (this occurs when h<hcrit)
c  the state point will be placed in k2,T2,P2,D2.  If kph=0 and this situation
c  occurred, the first root (k1,T1,P1,d1) would be in the liquid (k1=1, k2=2).
c
c  Inputs:
c        h--Molar enthalpy [J/mol]
c        z--Composition (array of mole fractions)
c      kph--Flag specifying desired root:
c           0 - Return all roots along the liquid-vapor line
c           1 - Return only liquid VLE root
c           2 - Return only vapor VLE roots
c           3 - Return liquid SLE root (melting line)
c           4 - Return vapor SVE root (sublimation line)
c           kph = 3,4 presently working only for pure components
c
c  Outputs:
c    nroot--Number of roots.  Value is set to one for kph=1,3,4 if ierr=0
c       k1--Phase of first root (1-liquid, 2-vapor, 3-melt, 4-subl)
c       T1--Temperature of first root [K]
c       P1--Pressure of first root [kPa]
c       d1--Molar density of first root [mol/L]
c       k2--Phase of second root (1-liquid, 2-vapor, 3-melt, 4-subl)
c       T2--Temperature of second root [K]
c       P2--Pressure of second root [kPa]
c       D2--Molar density of second root [mol/L]
c     ierr--Error flag:  0 - Successful
c                      181 - SATH did not converge for one of the roots
c                       54 - h < hmin
c                       55 - h > hmax
c                       56 - h > htrp (for sublimation inputs)
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-22-00 EWL, original version
c  12-11-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character herr*255
      dimension x(ncmax),y(ncmax),z(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATH


      T=300d0
      ibt=iLetterH
      if (ieflg.eq.1) ibt=iLetterE
      iErrNum=iErrNum+1
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)                       !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      if (icomp.ne.0) call SETPNTS (1,icomp,z,T,P,Dl,Dv,ierr,herr)
      if (iFluidID(icomp).eq.1.or.iFluidID(icomp).eq.3) Tmin=Ttpn(icomp)       !Resets Tmin from 251 to 273 K for water.
      tolr=1d-6
      itmax=20
      if (icomp.ne.0) itmax=30
      iSatFlg=0
      nroot=0
      T1=0d0
      P1=0d0
      D1=0d0
      T2=0d0
      P2=0d0
      D2=0d0
      k1=0
      k2=0
      ierr=0
      ierr1=0

c  Determine region.
      kphs=1       !Liquid or vapor with a density higher than that at the maximum enthalpy.
      if (kph.gt.2 .and. icomp.eq.0) goto 500              !Mixture, melting line not available.
      if (kph.gt.2 .and. icomp.ne.0) kphs=kph              !Liquid/solid or Vapor/solid for a pure fluid

c  Get maximum enthalpy (or energy) along the saturated vapor dome.
      call HEMAXX(z,ieflg,xmax,Themax,Dhemax,ierr,herr)
      if (ierr.ge.100) goto 500
      if (h.gt.xmax .and. kphs.ne.3) goto 55    !Exit if there is no solution (h>hmax).

c  Calculate enthalpy at Tmin.
      Tl=Tmin
      Tv=Tmin
      Dliq=0d0
      Dvap=0d0
      dd=0d0
      if (icomp.ne.0 .and. iAnc(icomp).eq.0) then
        Dliq=Dtpn(icomp)
        Dvap=Dtpv(icomp)
      else                   !Check for mixture or pseudo-pure fluid.
        ierrl=999
        ierrv=999
        it=0
        do while ((ierrl.gt.0.or.ierrv.gt.0).and.Tl.lt.Tc.and.Tv.lt.Tc)
         it=it+1
         if(ierrl.gt.0)call SATTP(Tl,P,z,1,0,D,Dliq,dd,x,y,q,ierrl,herr)
         if(ierrv.gt.0)call SATTP(Tv,P,z,2,0,D,dd,Dvap,x,y,q,ierrv,herr)
         if(ierrl.gt.0)Tl=Tl+(Tc-Tl)/20d0
         if(ierrv.gt.0)Tv=Tv+(Tc-Tv)/10d0
         if (it.gt.10) EXIT
        enddo
      endif
      call HESCALC (ibt,Tl,Dliq,z,hliq)
      call HESCALC (ibt,Tv,Dvap,z,hvap)
      ierr1=0

c  Check for inputs that are out of bounds.
      if (h.gt.hvap .and. kphs.eq.4) goto 56
      if (h.lt.hliq .and. kphs.ne.4) goto 54

c  Store density and enthalpy for root bounding during iteration.
 200  call ROOTSEARCH (0,0,0,v,f,T,0,ib,lneg,lpos)         !Initialize root finding variables.
      if (kphs.eq.1 .or. kphs.eq.3) then
        v(2)=Dliq
        f(2)=hliq-h
      else
        v(2)=Dvap
        f(2)=hvap-h
      endif
      call ROOTSEARCH (1,0,0,v,f,T,0,ib,lneg,lpos)
      if (kphs.ne.3 .and. kphs.ne.4) then                  !Store second bound at hmax when iterating for liquid or vapor.
        v(2)=Dhemax
        f(2)=xmax-h
        call ROOTSEARCH (1,0,0,v,f,T,0,ib,lneg,lpos)
      endif

c  Generate next guess for density.
      D=Dhemax/2d0
      if (kphs.eq.1) D=(Dc+Dliq)/2d0
      if (kphs.eq.3) D=Dtpn(icomp)*1.1d0
      if (kphs.eq.4) D=Dtpv(icomp)*0.8d0

c  ---Main iterative loop to find root---
      do it=1,itmax
        call TOLRNC (tolr,it,10,15,18,0,0)       !Loosen tolerance if not converging.
        if (iSpline.ne.0 .and. ieflg.eq.0) then
          call SATGV (T,P,z,0d0,D,1,6,1,Dl,Dv,x,y,ierr,herr)
        else
          call SATD (D,z,kphs,kr,T,P,Dl,Dv,x,y,ierr,herr)
        endif
        if (ierr.lt.100) ierr=0        !Ignore boundary errors.
        if (ierr.le.0) then
          call HESCALC (ibt,T,D,z,h1)
          f(2)=h1-h
          v(2)=D
          call ROOTSEARCH (1234,0,0,v,f,D,it,ib,lneg,lpos)           !Compute next guess for temperature.
          if (ABS(f(2)).lt.tolr) goto 300        !Check for convergence.
          if (icomp.ne.0 .and. it.gt.10) then
            if (ABS(T-Tc).lt.1d-6 .and. ABS(D-Dc).lt.01d0) goto 300            !Routine is bouncing around very close to the critical point, so just exit.
          endif
        else
          D=D*0.9d0
        endif
      enddo

c Iteration failed.
      ierr=0
      if (icomp.ne.0) then
        if (ABS(v(4)-v(5)).lt.0.1d0 .and. ABS(T-Tc).lt.0.01d0) goto 300        !Check for close answer very near the critical point.
      endif
      ierr1=181
      ierr=ierr1

c Iteration converged.
 300  if (ierr.le.0 .or. (kphs.ge.3 .and. ierr.lt.100)) then
        nroot=nroot+1
        if (nroot.eq.1) then
          T1=T               !First root found.
          P1=P
          D1=D
          k1=kphs
          if (kphs.lt.3) k1=1
          if (kphs.lt.3 .and. D.lt.Dc) k1=2
        else
          T2=T               !Second root found.
          P2=P
          D2=D
          k2=kphs
        endif
      endif

c  Check if second root exists.
      if (kphs.eq.1 .and. h.gt.hvap) then
        kphs=2
        goto 200
      endif

 500  if (ierr1.ne.0) ierr=ierr1
      if (ierr1.eq.181) call ERRNUM(181,0,' ',' ',h,0d0,0d0,ierr,herr)

c  Check if limits have been exceeded, return that message if no other error has occurred.
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS ('S4',T,D,P,z,ierr,herr)
      RETURN

 54   call ERRNUM (54,0,' ',' ',h,hliq,0d0,ierr,herr)
      goto 500
 55   call ERRNUM (55,0,' ',' ',h,xmax,0d0,ierr,herr)
      goto 500
 56   call ERRNUM (56,0,' ',' ',h,hvap,0d0,ierr,herr)
      goto 500

      end                                                subroutine SATH
c
c ======================================================================
c
      subroutine SATS (s,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,
     &                 k3,T3,P3,D3,ierr,herr)
c
c  Iterate for temperature, pressure, and density given entropy along
c  the saturation boundary and the composition.
c
c  The second root is always set as the root in the vapor at temperatures
c  below the maximum entropy on the vapor saturation line.  If kph is
c  set to 2, and only one root is found in the vapor (this occurs when s<scrit)
c  the state point will be placed in k2,T2,P2,D2.  If kph=0 and this situation
c  occurred, the first root (k1,T1,P1,D1) would be in the liquid (k1=1, k2=2).
c
c  The third root is the root with the lowest temperature.  For fluids
c  with multiple roots, when only one root is found in the vapor phase
c  (this happens only at very low temperatures past the region where three
c  roots are located), the value of the root is still placed in
c  k3,T3,P3,D3.  For fluids that never have more than one root (when there
c  is no maximum entropy along the saturated vapor line), the value of the
c  root is always placed in k1,T1,P1,D1.
c
c  Inputs:
c        s--Molar entropy [J/mol-K]
c        z--Composition (array of mole fractions)
c      kph--Flag specifying desired root:
c           0 - Return all roots along the liquid-vapor line
c           1 - Return only liquid VLE root
c           2 - Return only vapor VLE roots
c           3 - Return liquid SLE root (melting line)
c           4 - Return vapor SVE root (sublimation line)
c           kph = 3,4 presently working only for pure components
c
c  Outputs:
c    nroot--Number of roots.  Set to one for kph=1,3,4 if ierr=0
c       k1--Phase of first root (1-liquid, 2-vapor, 3-melt, 4-subl)
c       T1--Temperature of first root [K]
c       P1--Pressure of first root [kPa]
c       D1--Molar density of first root [mol/L]
c       k2--Phase of second root (1-liquid, 2-vapor, 3-melt, 4-subl)
c       T2--Temperature of second root [K]
c       P2--Pressure of second root [kPa]
c       D2--Molar density of second root [mol/L]
c       k3--Phase of third root (1-liquid, 2-vapor, 3-melt, 4-subl)
c       T3--Temperature of third root [K]
c       P3--Pressure of third root [kPa]
c       D3--Molar density of third root [mol/L]
c     ierr--Error flag:  0 - Successful
c                      192 - SATS did not converge for one or more roots
c                       66 - s < smin
c                       67 - s > smax
c                       68 - s > strp (for sublimation inputs)
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-24-00 EWL, original version
c  12-12-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character herr*255
      dimension x(ncmax),y(ncmax),z(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATS


      ibt=iLetterS
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      call SETPNTS (1,icomp,z,T,P,Dl,Dv,ierr,herr)
      iw=0
      if (iFluidID(icomp).eq.1) iw=1
      if (iFluidID(icomp).eq.3) iw=3
      if (iw.ge.1) Tmin=Ttpn(icomp)    !Resets Tmin from 251 to 273 K for water or heavy water.
      call ENTRO (Tc,Dc,z,sc)

      tolr=1d-6
      itmax=20
      if (icomp.ne.0) itmax=30
      iErrNum=iErrNum+1
      iSatFlg=0
      ierr1=0
      nroot=0
      T1=0d0
      P1=0d0
      D1=0d0
      T2=0d0
      P2=0d0
      D2=0d0
      T3=0d0
      P3=0d0
      D3=0d0
      k1=0
      k2=0
      k3=0
      kr=0

c  Get maximum and minimum entropy along the saturated vapor dome, and
c   the minimum entropy along the melting line.
      call SMAXX (z,ierr,herr)

c  Calculate entropy at Tmin.
      Tl=Tmin
      Tv=Tmin
      dd=0d0
      Dliq=0d0
      Dvap=0d0
      if (icomp.ne.0 .and. iAnc(icomp).eq.0) then
        Dliq=Dtpn(icomp)
        Dvap=Dtpv(icomp)
        if (iw.eq.1) Dliq=DmaxWater
        if (iw.eq.3) Dliq=DmaxD2O
      else                   !Check for mixture or pseudo-pure fluid.
        ierrl=999
        ierrv=999
        do while ((ierrl.gt.0.or.ierrv.gt.0).and.Tl.lt.Tc.and.Tv.lt.Tc)
         if(ierrl.gt.0)call SATTP(Tl,P,z,1,0,D,Dliq,dd,x,y,q,ierrl,herr)
         if(ierrv.gt.0)call SATTP(Tv,P,z,2,0,D,dd,Dvap,x,y,q,ierrv,herr)
         if(ierrl.gt.0)Tl=Tl+5d0
         if(ierrv.gt.0)Tv=Tv+5d0
        enddo
      endif
      call HESCALC (ibt,Tl,Dliq,z,sliq)
      call HESCALC (ibt,Tv,Dvap,z,svap)

c  Determine region.
                                        kphs= 1            !Liquid or vapor for densities higher than that at the maximum entropy.
      if (Tsmax.le.0.)                  kphs=-1            !No maximum entropy in the vapor phase.
      if (Tsmax.gt.0 .and. s.gt.Ssmax)  kphs=-2            !Only one vapor root available, which is above the maximum entropy.
      if (s.lt.sc .and. kph.eq.1)       kphs= 1
      if (s.lt.sc .and. kphs.eq.-1)     kphs= 1

      if (kph .ge.3 .and. icomp.ne.0)   kphs=kph           !Liquid/solid or Vapor/solid.
      if (kphs.eq.3 .and. Tsmin2.gt.0.) then
        if (s.lt.sliq) kphs=-3         !Two melting line roots exist.
        if (s.ge.sliq) Dliq=Dsmin2     !The system has two roots, but only one available at the input entropy.
        if (s.ge.sliq) sliq=Ssmin2
      endif

c  Check for inputs that are out of bounds.
      if (s.lt.svap   .and. ABS(kphs).eq.4) goto 68       !Sublimation line check.
      if (s.lt.sliq   .and. kphs.eq. 3)     goto 66        !Melting line check.
      if (s.lt.Ssmin2 .and. kphs.eq.-3)     goto 66        !Check if melting line is double valued.
      if (s.gt.Ssmax  .and. s.gt.svap .and. ABS(kphs).le.2) goto 67

c  Store density and entropy for root bounding during iteration.
 200  call ROOTSEARCH (0,0,0,v,f,T,0,ib,lneg,lpos)         !Initialize root finding variables.
      v(2)=Dliq
      f(2)=sliq-s
      if (kphs.eq.2) then
        v(2)=Dsmin
        f(2)=Ssmin-s
      elseif (kphs.eq.-2.or.kphs.eq.4.or.(kphs.eq.-1.and.s.gt.sc)) then
        v(2)=Dvap
        f(2)=svap-s
      endif
      call ROOTSEARCH (1,0,0,v,f,T,0,ib,lneg,lpos)

c  Store second bound.
      v(2)=0d0
      if (kphs.eq.1 .or. kphs.eq.2) then
        if (.not.lEq0(Dsmax)) then
          v(2)=Dsmax
          f(2)=Ssmax-s
        endif
      elseif (kphs.eq.-1) then
        v(2)=Dc
        f(2)=sc-s
      elseif (kphs.eq.-2 .and. s.gt.Ssmax) then
        v(2)=Dsmin
        f(2)=Ssmin-s
      elseif (kphs.eq.-3) then
        v(2)=Dsmin2
        f(2)=Ssmin2-s
      endif
      if (v(2).ne.0.) call ROOTSEARCH (1,0,0,v,f,T,0,ib,lneg,lpos)

c  Generate next guess for density.
      D=Dc
      if (kphs.eq. 1) D=(Dc+Dliq)/2d0
      if (kphs.eq.-1) D=(Dc+Dvap)/2d0
      if (kphs.eq. 2) D=(Dsmin+Dsmax)/2d0
      if (kphs.eq.-2) D=(Dsmin+Dvap)/2d0
      if (kphs.eq. 3) D=Dliq*1.01d0
      if (kphs.eq.-3) D=Dliq*1.01d0
      if (kphs.eq. 4) D=Dvap*0.8d0

c  ---Main iterative loop to find root---
      it=0
      kphd=0
      if (kphs.eq.3) kphd=3
      do
        it=it+1
        if (it.gt.itmax) EXIT
        call TOLRNC (tolr,it,10,15,18,0,0)       !Loosen tolerance if not converging.
        if (iSpline.ne.0) then
          call SATGV (T,P,z,0d0,D,1,6,1,Dl,Dv,x,y,ierr,herr)
        else
          if (icomp.ne.0 .and. it.eq.20 .and. iw.ge.1) then
            if (kphd.eq.0 .and. D.gt.55.4d0) then          !For water, if not converging at densities near Dtrp, then the point is most likely between the triple point and the temperature at the maximum density, so change the bounds and send SATD a kph of -1.
              kphd=-1
              call HESCALC (ibt,Ttpn(icomp),Dtpn(icomp),z,s1)
              v(4)=Dtpn(icomp)
              f(4)=s1-s
              D=55.5d0
              it=1
              itmax=19
            endif
          endif
          call SATD (D,z,kphd,kr,T,P,Dl,Dv,x,y,ierr,herr)
        endif
        if (ierr.lt.100) ierr=0        !Ignore boundary errors.
        if (ierr.le.0) then
          call HESCALC (ibt,T,D,z,s1)
          f(2)=s1-s
          v(2)=D
          call ROOTSEARCH (1234,0,0,v,f,D,it,ib,lneg,lpos)           !Compute next guess for temperature.
          if (ABS(f(2)).lt.tolr) goto 300        !Check for convergence.
        else
          D=D*0.9d0
        endif
      enddo

c Iteration failed.
      ierr=0
      if (icomp.ne.0) then
        if (ABS(v(4)-v(5)).lt.0.1d0 .and. ABS(T-Tc).lt.0.01d0) goto 300        !Check for close answer very near the critical point.
      endif
      if (ierr1.eq.193) ierr1=194
      if (ierr1.eq.192) ierr1=193
      if (ierr1.eq.0  ) ierr1=192
      ierr=ierr1

c Iteration converged.
 300  if (ierr.le.0) then
        nroot=nroot+1
        if (nroot.eq.1) then
          T1=T               !First root found.
          P1=P
          D1=D
          k1=ABS(kphs)
          if (ABS(kphs).lt.3) k1=1
          if (ABS(kphs).lt.3 .and. D.lt.Dc) k1=2
          if (kr.eq.3) k1=3
        elseif (nroot.eq.2) then
          T2=T               !Second root found.
          P2=P
          D2=D
          k2=ABS(kphs)
          if (kr.eq.3) k2=3
        else
          T3=T               !Third root found.
          P3=P
          D3=D
          k3=ABS(kphs)
        endif
      endif

c  Check if additional roots exist.
      if (kphs.eq.1 .and. s.gt.Ssmin .and. Ssmin.ne.0.) then
        kphs=2
        goto 200
      elseif (kphs.eq.2) then          !Check if third root exists.
        kphs=-2
        goto 200
      elseif (kphs.eq.-3) then         !Check if a second melting point root exists.
        Dliq=Dsmin2
        sliq=Ssmin2
        kphs=3
        goto 200
      endif
      if (k1+k2+k3.eq.0 .and. ierr.eq.0) ierr1=192

 500  if (ierr1.ge.192) ierr=ierr1
      if (ierr1.ge.192) call ERRNUM(192,0,' ',' ',0d0,0d0,0d0,ierr,herr)

c  Check if limits have been exceeded, return that message if no other error has occurred.
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS ('S5',T,D,P,z,ierr,herr)
      RETURN

 66   smx=sliq
      if (kphs.eq.-3) smx=Ssmin2
      call ERRNUM (66,0,' ',' ',s,smx,0d0,ierr,herr)
      goto 500
 67   smx=Ssmax
      if (svap.gt.Ssmax) smx=svap
      call ERRNUM (67,0,' ',' ',s,smx,0d0,ierr,herr)
      goto 500
  68  call ERRNUM (68,0,' ',' ',s,svap,0d0,ierr,herr)
      goto 500

      end                                                subroutine SATS
c
c ======================================================================
c
      subroutine SATE (e,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
c
c  Iterate for temperature, pressure, and density given energy along
c  the saturation boundary and the composition.
c
c  Inputs:
c        e--Molar energy [J/mol]
c        z--Composition (array of mole fractions)
c      kph--Flag specifying desired root:
c           0 - Return all roots along the liquid-vapor line
c           1 - Return only the liquid VLE root
c           2 - Return only the vapor VLE roots
c           3 - Return liquid SLE root (melting line)
c           4 - Return vapor SVE root (sublimation line)
c
c  Outputs:
c  (See subroutine SATH for the description of outputs.)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  05-16-05 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATE


      ieflg=1
      call SATH (e,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
      ieflg=0
      if (ierr.ne.0) then
        i=INDEX(herr,'SATH')
        if (i.gt.0) herr=herr(1:i+2)//'E'//herr(i+4:255)
        i=INDEX(herr,'h =')
        if (i.gt.0) herr=herr(1:i-1)//'e'//herr(i+1:255)
        i=INDEX(herr,'enthalpy')
        if (i.gt.0) herr=herr(1:i-1)//'energy'//herr(i+8:255)
      endif
      end                                                subroutine SATE
c
c ======================================================================
c
      subroutine SATTP (T,P,z,iFlsh,iGuess,D,Dl,Dv,x,y,q,ierr,herr)
c
c  Calculate saturation properties for bubble, dew, or 2-phase states
c  with the use of analytical derivatives of the Helmholtz energy with
c  respect to composition.
c
c  Inputs:
c        T--Temperature [K] (input or output)
c        P--Pressure [kPa] (input or output)
c        z--Composition (array of mole fractions)
c    iFlsh--Phase flag:  0 - Flash calculation (T and P known)
c                        1 - T and x known, P and y returned
c                        2 - T and y known, P and x returned
c                        3 - P and x known, T and y returned
c                        4 - P and y known, T and x returned
c                            If this value is negative, the retrograde point will be returned.
c   iGuess--If set to 1, the parameters Dl, Dv, x, and y are used as initial guesses for the calculation.
c           If Dl and Dv are set to zero when iGuess=1, then the densities are obtained from the first call to TPRHO.
c           If Dl and Dv are not zero when iGuess=1, those values are used as initial values.
c           If set to 2 and splines have been calculated, use inputs rather than spline values.
c
c  Outputs:
c        D--Overall density [mol/L]
c       Dl--Molar density of saturated liquid [mol/L]
c       Dv--Molar density of saturated vapor [mol/L]
c        x--Liquid phase composition (array of mole fractions)
c        y--Vapor phase composition  (array of mole fractions)
c        q--Quality
c     ierr--Error flag:  0 - Successful
c                      121 - T>Tmax (maxcondentherm)
c                      141 - P>Pmax (maxcondenbar)
c                      151 - Iteration failed
c                      156 - Probable Type III mixture with no liquid solution
c                      159 - Wrong input value for iFlsh
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by Andreas Jaeger, Johannes Gernert, Diego Ortiz, and Eric Lemmon
c  06-20-11 EWL, original version
c  08-28-13 EWL, check for T<0
c  10-22-13 EWL, check for Dliq<Dvap when iFlsh=0
c  03-26-13 EWL, add additional checks and modifications for better convergence
c  05-13-14 EWL, add check for T>Tmax or P>Pmax
c  06-05-14 EWL, check for divide by zero

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension x(ncmax),y(ncmax),z(ncmax),ix(ncmax),ij(ncmax)
      character herr*255
      dimension dadn(ncmax),dnadn(ncmax)
      dimension xvap_new(ncmax),xliq_new(ncmax)
      dimension xJacMatrix(50,50),Delta_X(50),GibbsEQN(50)
      dimension xlnfiliq(ncmax),xlnfivap(ncmax),
     &          dphidxjliq(ncmax,ncmax),dphidxjvap(ncmax,ncmax),
     &          dphidTliq(ncmax),dphidPliq(ncmax),
     &          dphidTvap(ncmax),dphidPvap(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATTP


      ierr=0
      iErrNum=iErrNum+1
      iFlash=ABS(iFlsh)
      call ISPURE (z,icomp)
      call SATSAVE (1,icomp,iFlash,T,P,D,Dl,Dv,q,x,y,z,ierr,iexit)   !Check if this calculation has already been done for the input conditions.
      if (iexit.eq.1) GOTO 500

      erMaxOld=0d0
      iGues=iGuess
      if (iGues.gt.1) iGues=1
      iGues1=iGues
      iGues2=iGues
      if (Dl.lt.1d-100) iGues1=0
      if (Dv.lt.1d-100) iGues2=0
      iErBad=0
      it0=0
      it2=0
      nc2=nc
      ij=1
      ix=0
      m=0

c  Sort the compositions in order of largest to smallest with the pointer array ix, removing
c   any that are zero, so that every call to SATTP is independent of the order of the fluids.
      do i=1,nc
        if (z(i).le.0.) nc2=nc2-1      !Decrease the number of components by 1 for zero compositions.
        k=1
        do j=1,nc
          if (ij(k).eq.0) k=j
          if (z(j).gt.z(k) .and. ij(j).eq.1) k=j           !Find the slot that has the largest composition of those that have not yet been set.
        enddo
        ij(k)=0
        if (z(k).gt.0.) ix(i)=k        !Set the ix array to the slot found above.
        if (z(i).le.0.) m=i            !If the composition is zero, save this location for use below.
      enddo
      do i=1,nc
        if (ix(i).eq.0.) ix(i)=m       !If ix has not been set, use any location where the composition was zero.
      enddo

      call CRITP (z,Tc,Pc,Dc,ierr1,herr)
      if (iGues.eq.0) then
        if (iFlash.eq.1 .or. iFlash.eq.2) P=Pc
        if (iFlash.eq.3 .or. iFlash.eq.4) T=Tc
      endif
      iSatFlg=0    !A value of 0 for iSatFlg means it's safe to call SATTP non-recursively.
      irdxerr=0
      erMax=0
      iretrog=0
      xvap_new(1:nc)=0
      xliq_new(1:nc)=0
      if (iFlsh.lt.0) iretrog=1
      if (iFlash.lt.0 .or. iFlash.gt.4) goto 159
      if (iFlash.gt.2 .and. iGues.eq.0) T=300d0

c  If SATSPLN was called, use the splines to get very good estimates of the phase boundaries.
      if (iSpline.ne.0 .and. iFlash.gt.0 .and. iGuess.ne.2) then
        call MAXT (z,TmaxT,TmaxP,TmaxD,ierr1,herr)         !Cricondentherm
        call MAXP (z,PmaxT,PmaxP,PmaxD,ierr2,herr)         !Cricondenbar
        if (iFlash.eq.1 .or. iFlash.eq.2) then
          if (ierr1.eq.0 .and. T.gt.TmaxT+1d0 .and. MixTyp.le.1)goto 121       !T is above maxcondentherm, exit with error.
        elseif (iFlash.eq.3 .or. iFlash.eq.4) then
          if (ierr2.eq.0 .and. P.gt.PmaxP+10d0.and. MixTyp.le.1)goto 141       !P is above maxcondenbar, exit with error.
        endif

        x(1:nc)=z(1:nc)
        y(1:nc)=z(1:nc)
        iprop=1
        if (iFlash.eq.3 .or. iFlash.eq.4) iprop=2
        if (iFlash.eq.1 .or. iFlash.eq.3) then
          call SATGUESS (1,iprop,0,0,x,T,P,Dl,h,s,Dv,y,ierr,herr)    !Use SATGUESS to get the estimates from the splines.
          if (iFlash.eq.3 .and. ierrPmx.gt.0 .and. Dl.le.Dv) goto 156
        elseif (iFlash.eq.2 .or. iFlash.eq.4) then
          call SATGUESS (2,iprop,0,0,y,T,P,Dv,h,s,Dl,x,ierr,herr)
        endif
        iGues1=1
        iGues2=1
        iGues=1
      endif

c  Try SATT or SATP if SATTP fails.  The parameter irdxerr indicates if failure happened at the end of this routine, and it returns here to call SATT or SATP.
 10   if (icomp.ne.0 .or. irdxerr.ne.0 .or.
     &    iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) then
        if (ierr.lt.100) ierr=999
        iSatFlg=1            !A value of 0 for iSatFlg means it's safe to call SATTP non-recursively.
        if (iFlash.eq.1 .or. iFlash.eq.2) then             !Temperature was sent as the input.
          if (iFlash.eq.1 .and. iSpline.ne.0) then
            tt=0d0
            call SATGUESS (1,-1,0,0,z,tt,P,D,h,s,Dy,y,ierr,herr)
            if (ierr.eq.0 .and. D.gt.Dc*1.5d0) then
              call SATGUESS (1,21,0,0,z,tt,P,D,h,s,d2,y,ierr,herr)             !2nd derivative at root.
              if (T.lt.tt .and. d2.gt.0) goto 450          !Exit if splines show that the input temperature in the liquid phase is below that of all possible roots.
            endif
          endif
          if (ierr.lt.100) ierr=999
          P=ABS(P)
          if (iSatFlg1.eq.0)call SATT (T,z,iFlash,P,Dl,Dv,x,y,ierr,herr)
        elseif (iFlash.eq.3 .or. iFlash.eq.4) then         !Pressures was sent as the input.
          T=ABS(T)
          if(iSatFlg1.eq.0)call SATP(P,z,iFlash-2,T,Dl,Dv,x,y,ierr,herr)
        else
          iSatFlg=1          !A value of 0 for iSatFlg means it's safe to call SATTP non-recursively.
          if (iTPflg.eq.0)
     &    call TPFLSH(T,P,z,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)   !Both T and P were sent as inputs, x and y are the outputs (2-phase flash inputs).
          iSatFlg=0          !A value of 0 for iSatFlg means it's safe to call SATTP non-recursively.
        endif
        if (iFlash.eq.1 .or. iFlash.eq.3) then
          q=0
          D=Dl
        elseif (iFlash.eq.2 .or. iFlash.eq.4) then
          q=1
          D=Dv
        endif
        if (ierr.le.0 .and. iAnc(icomp).eq.0) then
          if (ABS(Dl-Dv).lt.0.1d0 .and. ABS(Dl-Dc).gt.0.1d0) ierr=999
          if (Dl+Dv.lt.Dc) ierr=999
        endif
        if (ierr.eq.151 .or. ierr.ge.700) goto 450
        goto 500
      endif

c  If Splines are not known, use SATEST or SAT0EST to get estimates, which could be significantly in error for mixtures such as those with water and air.
 150  if (iGues.eq.0) then
        Dv=Dc
        Dl=Dc
        if (iFlash.eq.3 .or. iFlash.eq.4) T=300
        if (iFlash.ne.0) call SATEST (iFlash,T,P,z,x,y,ierr,herr)    !Saturation inputs
        if (iFlash.eq.0) call SAT0EST (T,P,z,x,y,ierr,herr)          !Flash inputs
        if (it2.ne.0) then
          if (iFlash.eq.1 .or. iFlash.eq.3) then           !If the routine failed because the estimates were bad, significantly increase the mole fraction
            yMax=MAXVAL(y(1:nc))                           !  of the heaviest component and try again.
            do i=1,nc
              if (y(i).gt.yMax*0.99d0) y(i)=y(i)+(1d0-y(i))*0.9d0
            enddo
            y(1:nc)=y(1:nc)/SUM(y(1:nc))
          else
            xMax=MAXVAL(x(1:nc))
            do i=1,nc
              if (x(i).gt.xMax*0.99d0) x(i)=10d0*x(i)
            enddo
            x(1:nc)=x(1:nc)/SUM(x(1:nc))
          endif
        endif
      endif

      tol1=1d-12
      tol2=1d-10
      ierr=0
      herr=' '
      Dvap=Dv
      Dliq=Dl
      ieqn=nc2
      if (iFlash.eq.0) ieqn=2*(nc2-1)

      it0=it0+1
      do it=it0,30
        GibbsEQN=0d0
c...Vapor
        call TPRHO (T,P,y,2,iGues2,Dvap,ierr,herr)
        if (ierr.gt.0) then
          P0=P
          T0=T
          do i=1,5
            if (ierr.gt.0 .or. Dvap.gt.Dliq) then
              do j=1,INT(-0.1d0+i/2d0)+1
                if (iFlash.eq.1 .or. iFlash.eq.2) P=P/1.2d0
                if (iFlash.eq.3 .or. iFlash.eq.4) T=T+2d0
              enddo
              call TPRHO (T,P,y,2,iGues2,Dvap,ierr,herr)
              if (i.ge.3) iGues2=0
            endif
          enddo
          if (ierr.gt.0) then
            if (iFlash.eq.1 .or. iFlash.eq.2) P=P0
            if (iFlash.eq.3 .or. iFlash.eq.4) T=T0
            call TPRHO (T,P,y,1,0,Dvap,ierr,herr)
            if (ierr.gt.0) goto 1000
          endif
        endif
        iGues2=1
        call PHIDERV (2,T,Dvap,y,dadn,dnadn,ierr,herr)
        if (ierr.eq.191) then          !Derivatives of the Helmholtz equation are not available, use SATT or SATP.
          irdxerr=ierr
          goto 10
        endif
        if (ierr.gt.0) goto 1000
        do j=1,nc2
          xlnfivap(j)=xlnfi(ix(j))
          dphidtvap(j)=dphidt(ix(j))
          dphidpvap(j)=dphidp(ix(j))
          do k=1,nc2
          dphidxjvap(j,k)=dphidxj(ix(j),ix(k))-dlnphiidxj(ix(nc2),ix(k))
          enddo
          if (y(ix(nc2)).gt.1d-50)
     &        dphidxjvap(j,nc2)=dphidxjvap(j,nc2)-1d0/y(ix(nc2))
        enddo

c...Liquid
        call TPRHO (T,P,x,1,iGues1,Dliq,ierr,herr)
        if (Dliq.lt.Dc/10d0) call TPRHO (T,P,x,1,0,Dliq,ierr,herr)
        if (ierr.gt.0) then
          P0=P
          T0=T
          do i=1,10
            if (ierr.gt.0 .or. Dliq.lt.Dvap) then
              do j=1,INT(-0.1d0+i/2d0)+1
                if (iFlash.eq.1 .or. iFlash.eq.2) P=P*1.2d0
                if (iFlash.eq.3 .or. iFlash.eq.4) T=T-2d0
              enddo
              call TPRHO (T,P,x,1,iGues1,Dliq,ierr,herr)
              if (i.ge.3) iGues1=0
            endif
          enddo
          if (ierr.gt.0) then
            if (iFlash.eq.1 .or. iFlash.eq.2) P=P0
            if (iFlash.eq.3 .or. iFlash.eq.4) T=T0
            call TPRHO (T,P,x,2,0,Dliq,ierr,herr)
            if (ierr.gt.0) goto 1000
          endif
        endif
        iGues1=1
        call PHIDERV (2,T,Dliq,x,dadn,dnadn,ierr,herr)
        if (ierr.gt.0) goto 1000
        do j=1,nc2
          xlnfiliq(j)=xlnfi(ix(j))
          dphidtliq(j)=dphidt(ix(j))
          dphidpliq(j)=dphidp(ix(j))
          do k=1,nc2
          dphidxjliq(j,k)=dphidxj(ix(j),ix(k))-dlnphiidxj(ix(nc2),ix(k))
          enddo
          if (x(ix(nc2)).gt.0d0)
     &        dphidxjliq(j,nc2)=dphidxjliq(j,nc2)-1d0/x(ix(nc2))
        enddo

        do j=1,nc2
          GibbsEQN(j)=xlnfivap(j)-xlnfiliq(j)
        enddo
        if (iFlash.eq.0) then
          do j=1,nc2-2
            dxkdxj_vap=0d0
            dxkdxj_liq=0d0
            if (y(ix(j))-z(ix(j)).ne.0.)
     &      dxkdxj_vap=(y(ix(nc2-1))-z(ix(nc2-1)))/
     &                 (y(ix(j))-z(ix(j)))
            if (x(ix(j))-z(ix(j)).ne.0.)
     &      dxkdxj_liq=(x(ix(nc2-1))-z(ix(nc2-1)))/
     &                 (x(ix(j))-z(ix(j)))
            GibbsEQN(nc2+j)=dxkdxj_vap-dxkdxj_liq
          enddo
        endif

        call TOLRNC (tol1,it,8,12,16,20,0)       !Loosen tolerance if not converging.
        call TOLRNC (tol2,it,8,12,16,20,0)
        erMax=maxval(ABS(GibbsEQN))
        if (erMax.lt.tol1 .and. it.ne.1) goto 1000

c..Set up Jacobian matrix.
        xJacMatrix(1:ieqn,1:ieqn)=0d0
        if (iFlash.eq.0) then
          do j=1,nc2-1
            do k=1,nc2
              xJacMatrix(k,j)=dphidxjvap(j,k)
              xJacMatrix(k,nc2-1+j)=-dphidxjliq(j,k)
            enddo
          enddo
          do j=1,nc2-2
            xJacMatrix(nc2+j,j)=0d0
            xJacMatrix(nc2+j,nc2-1)=0d0
            xJacMatrix(nc2+j,nc2-1+j)=0d0
            if (y(ix(j))-z(ix(j)).ne.0.) then
              xJacMatrix(nc2+j,j)=
     &                 -(y(ix(nc2-1))-z(ix(nc2-1)))/
     &                 ((y(ix(j))-z(ix(j)))**2)
              xJacMatrix(nc2+j,nc2-1)=1d0/(y(ix(j))-z(ix(j)))
            endif
            if (x(ix(j))-z(ix(j)).ne.0.) then
              xJacMatrix(nc2+j,nc2-1+j)=
     &                  (x(ix(nc2-1))-z(ix(nc2-1)))/
     &                 ((x(ix(j))-z(ix(j)))**2)
              xJacMatrix(nc2+j,2*(nc2-1))=-1d0/(x(ix(j))-z(ix(j)))
            endif
          enddo
        else
          do k=1,nc2
            do j=1,nc2-1
              if (iFlash.eq.1 .or. iFlash.eq.3) then
                xJacMatrix(k,j)=dphidxjvap(j,k)
              elseif (iFlash.eq.2 .or. iFlash.eq.4) then
                xJacMatrix(k,j)=-dphidxjliq(j,k)
              endif
            enddo
            if (iFlash.eq.1 .or. iFlash.eq.2) then
              xJacMatrix(k,nc2)=dphidpvap(k)-dphidpliq(k)
            elseif (iFlash.eq.3 .or. iFlash.eq.4) then
              xJacMatrix(k,nc2)=dphidtvap(k)-dphidtliq(k)
            endif
          enddo
        endif
c..Solve Jacobian matrix.
        Delta_X=-GibbsEQN
        call LUDECOMP (ieqn,50,xJacMatrix,Delta_X,ierr,herr)
        if (ierr.gt.0) goto 1000
        erMax=maxval(ABS(Delta_X))
        if (it.gt.1) then
c         if (erMax.lt.tol2) goto 1000
          if (erMax.lt.erMaxOld) ierBad=0
          if (erMax.gt.erMaxOld) ierBad=ierBad+1                     !The convergence criteria is slowly increasing, indicating that the estimated
          if (ierBad.gt.4 .and. it2.eq.0 .and. iFlash.ne.0) then     !  composition was probably bad, so go back and re-estimate with new estimates.
            if (erMax/tol2.gt.1000 .or. ierBad.gt.10) then
              it2=1
              iGues=0
              goto 150
            endif
          endif
        endif
        erMaxOld=erMax

c  Update unknown properties.
 100    jj=0
        if (iFlash.eq.0) jj=nc2-1
        sum_liq=0d0
        sum_vap=0d0
        sum_vsmall=0d0
        sum_lsmall=0d0

c  Add the changes determined in the call above to LUDECOMP to the unknown properties.
        do j=1,nc2-1
          xvap_new(ix(j))=y(ix(j))+Delta_X(j)
          xliq_new(ix(j))=x(ix(j))+Delta_X(j+jj)
        enddo
        do j=1,nc2-1         !These two loops are kept separate to avoid parallelization issues when the compiler optimizes the code.
          if (xvap_new(ix(j)).lt.1d-10) then
            sum_vsmall=sum_vsmall+xvap_new(ix(j))          !Required for loss in numerical precision when summed with 1.
          else
            sum_vap=sum_vap+xvap_new(ix(j))
          endif
          if (xliq_new(ix(j)).lt.1d-10) then
            sum_lsmall=sum_lsmall+xliq_new(ix(j))          !Required for loss in numerical precision when summed with 1.
          else
            sum_liq=sum_liq+xliq_new(ix(j))
          endif
        enddo
        xvap_new(ix(nc2))=1d0-sum_vap-sum_vsmall
        xliq_new(ix(nc2))=1d0-sum_liq-sum_lsmall
        if (xvap_new(ix(nc2)).lt.0. .and. ABS(y(ix(nc2))).lt.1d-12)            !Catch extremely small cases.  This needs a better solution.
     &      xvap_new(ix(nc2))=y(ix(nc2))
        T_new=T+Delta_X(nc2)
        P_new=P+Delta_X(nc2)

c  Make checks on new values, set k equal to 1 if the test fails.
        k=0
        if (ABS(Delta_X(nc2)).gt.P/50d0 .and. P.gt.10000) k=1        !For large P, don't let it change too rapidly (Type III mixtures with large dp/dT).
        do j=1,nc2
          if (iFlash.eq.1 .or. iFlash.eq.3) then
            if (xvap_new(ix(j)).le.0..or.xvap_new(ix(j)).ge.1.) k=2
            if (ABS(xvap_new(ix(j))-y(ix(j))).ge.0.03d0) k=3         !Don't allow large changes in composition.
            if (P_new.lt.0) k=4
          elseif (iFlash.eq.2 .or. iFlash.eq.4) then
            if (xliq_new(ix(j)).le.0..or.xliq_new(ix(j)).ge.1.) k=5
            if (ABS(xliq_new(ix(j))-x(ix(j))).ge.0.03d0) k=6         !Don't allow large changes in composition.
            if (T_new.lt.0) k=7
          else
            if ((xvap_new(ix(j))-z(ix(j)))*
     &          (xliq_new(ix(j))-z(ix(j))).gt.0.) then
              if (MAXVAL(ABS(Delta_X)).gt.1d-2) k=8
            endif
            if (xliq_new(ix(j)).le.0. .or. xliq_new(ix(j)).ge.1.) k=9
            if (xvap_new(ix(j)).le.0. .or. xvap_new(ix(j)).ge.1.) k=10
          endif

c  If k=1, the tests above failed, so cut the delta in half and try again.
          if (k.ge.1) then
            Delta_X=Delta_X/2d0
            if (MAXVAL(ABS(Delta_X)).lt.1d-10) goto 151
            goto 100
          endif
        enddo

        if (Dliq.le.0.) Dliq=Dc*3d0
        if (Dvap.le.0.) Dvap=Dc/3d0
        Dl=Dliq
        Dv=Dvap
        if (iFlash.eq.1) then
          q=0
          y(ix(1:nc2))=xvap_new(ix(1:nc2))
          P=P_new
          D=Dliq
        elseif (iFlash.eq.2) then
          q=1
          x(ix(1:nc2))=xliq_new(ix(1:nc2))
          P=P_new
          D=Dvap
        elseif (iFlash.eq.3) then
          q=0
          y(ix(1:nc2))=xvap_new(ix(1:nc2))
          T=T_new
          D=Dliq
        elseif (iFlash.eq.4) then
          q=1
          x(ix(1:nc2))=xliq_new(ix(1:nc2))
          T=T_new
          D=Dvap
        elseif (iFlash.eq.0) then
          if (Dv.gt.Dl) then
            call SWAPV (Dliq,Dvap)
            call SWAPV (Dl,Dv)
            call SWAPA (xliq_new,xvap_new)
          endif

          q=XDIV1(z(ix(1))-xliq_new(ix(1)),
     &          xvap_new(ix(1))-xliq_new(ix(1)))
          if (q.gt.1 .or. q.lt.0) then
            Delta_X=Delta_X/2d0
            if (MAXVAL(ABS(Delta_X)).lt.1d-10) goto 151
            goto 100
          endif

          x(ix(1:nc2))=xliq_new(ix(1:nc2))
          y(ix(1:nc2))=xvap_new(ix(1:nc2))
          q=XDIV1(z(ix(1))-x(ix(1)),y(ix(1))-x(ix(1)))
          if (q.lt.-1d0 .or. q.gt.2d0)
     &      call GETXYZQ(2,ix(1),q,z,xliq_new,xvap_new,x,y)
c         q=ABS(q)
          D=1d0/((1d0-q)/Dliq+q/Dvap)
        endif
        if (erMax.lt.tol2) goto 1000
        if (P.le.0.) goto 1000
        if (T.gt.1d10) EXIT
      enddo
      goto 151     !Iteration failed.

1000  continue
      if (Dv.le.0) ierr=151
      if (Dl.le.0) ierr=151
      if (P.le.0) ierr=151
      if (P.lt.20000.) then  !Checks for non-Type III mixtures
        if (ABS(Dl-Dv).lt.0.1d0 .and. ABS(Dl-Dc).gt.0.1d0) ierr=151
      else
        if (ABS(Dl-Dv).lt.1d-6 .and. ABS(Dl-Dc).gt.0.1d0) ierr=151
      endif
      if (ierr.le.0) call CHECKSTATE (0,0,T,Dl,x,Err,ierr,herr)
      if (ierr.le.0) call CHECKSTATE (0,0,T,Dv,y,Err,ierr,herr)
      if (ierr.gt.0) goto 151

 400  if (ierr.gt.0 .and. iSatFlg2.eq.0) then    !Don't do the following while SATSPLN is running.
        irdxerr=ierr
        goto 10
      endif
 450  if(ierr.gt.0)call ERRNUM (151,0,'SATTP',' ',0d0,0d0,0d0,ierr,herr)

c  Check if limits have been exceeded, return that message if no other error has occurred.
 500  iSatFlg=0
      call SATSAVE (2,icomp,iFlash,T,P,D,Dl,Dv,q,x,y,z,ierr,iexit)   !Save the results if the calculation was successful.
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS ('S6',T,D,P,z,ierr,herr)
      RETURN

 121  call ERRNUM (121,0,'SATTP',' ',T,TmaxT,0d0,ierr,herr)
      goto 400
 141  call ERRNUM(141,0,'SATTP',' ',P/1000d0,PmaxP/1000d0,0d0,ierr,herr)
      goto 400
 151  call ERRNUM (151,0,'SATTP',' ',0d0,0d0,0d0,ierr,herr)
      goto 400
 156  call ERRNUM (156,0,' ',' ',P/1000d0,0d0,0d0,ierr,herr)         !Don't change the error number, used elsewhere.
      goto 400
 159  call ERRNUM (159,0,'SATTP','iFlash',0d0,0d0,0d0,ierr,herr)
      goto 400

      end                                               subroutine SATTP
c
c ======================================================================
c
      subroutine SATSPLN (z,ierr,herr)
c
c  Calculates the phase boundary of a mixture at a given composition,
c  along with the critical point, cricondentherm, and cricondenbar.
c
c  Inputs:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      355 - Saturation routine failed
c     herr--Error string (character*255)
      include 'COMMONS.INC'
      dimension z(ncmax),Rsave(ncmax)
      character herr*255
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATSPLN

      ierr=0
      call ISPURE (z,icomp)
      if (icomp.ne.0) RETURN
      zSum=SUM(z(1:nc))
      if (zSum.lt.1.d-5) then
        call ERRNUM (312,0,'SATSPLN',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif
      zSum=SUM(ABS(z(1:nc)-zSpln(1:nc)))
      if (lEQ0(zSum)) RETURN

      Rsave(1:nc)=Reos(1:nc)           !Make all the gas constants the same to avoid critical
      Reos(1:nc)=xRgas                 ! region errors in the calculation of the splines.
      call SATSPLN2 (1,0d0,0d0,0d0,0d0,z,ierr,herr)
      Reos(1:nc)=Rsave(1:nc)

      end                                             subroutine SATSPLN
c
c ======================================================================
c
      subroutine SATSPLN2 (iFlg,Pstart,Dincr,Dlimit,Plimit,z,ierr,herr)
c
c  Calculates the phase boundary of a mixture at a given composition,
c  along with the critical point, cricondentherm, and cricondenbar.
c  Additional inputs over those in the original SATSPLN routine are
c  allowed in this new routine to increase the speed or number of points
c  being calculated.
c
c  Inputs:
c     iFlg--not used
c           (send zero for the first four values listed below to use defaults)
c   Pstart--Initial pressure for spline generator [kPa]
c    Dincr--Density increment [mol/L]
c   Dlimit--Upper density limit [mol/L]
c   Plimit--Upper pressure limit [kPa]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      355 - Saturation routine failed
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-20-11 EWL, original version
c  12-01-12 EWL, change to density marching method
c  07-10-13 EWL, make better initial guesses at very low densities
c  02-10-15 EWL, add additional inputs and rename to SATSPLN2
c  05-27-15 EWL, add ierr=-101 for a pure fluid
c  02-17-16 EWL, add one additional check if SATGV fails by calling SATTP with P=P*2 instead of P=P*10
c  03-09-16 EWL, if Dl and Dv are almost equal, do not set fail flag when at high densities for Type III mixtures
c  04-19-16 EWL, add code to find the densities at the minimum and maximum values of temperature
c  05-16-18 EWL, exit if the AGA8 routine is currently in use

      include 'COMMONS.INC'
      dimension xPnts(1000),yPnts(1000),coefs(1000),cMatrix(10)
      dimension x(ncmax),y(ncmax),z(ncmax)
      character herr*255

      if (lAGA8) RETURN !Exit if AGA8 is in use.
      ierr=iFlg         !Keep the compiler happy since iFlg is not yet used.
      iex=0
      itt=0
      itp=0
      ierr=0
      herr=' '
      nArr=0
      iNxt=0
      MixTyp=0
      iSpline=0
      xArr=0d0
      xPnts=0
      yPnts=0
      coefs=0
      ioff=0
      T=300
      D=0d0
      P=0.0001d0
      e=1.d-6
      zSpln=0d0
      icnt=0
      Dx=0
      Dy=0
      T_est=300d0

      call ISPURE (z,icomp)
      if (icomp.ne.0) RETURN
      iErrNum=iErrNum+1
      call CRITP (z,Tc,Pc,Dc,ierr,herr)
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      call RDXHMX (1,1,0,z,tr01,dr01,ierr,herr)            !Call RDXHMX to make sure that analytical derivatives are available for the mixture.
      if (ierr.gt.0.and.iEOSmx.ne.iEOSPR.and.iEOSmx.ne.iEOSPR3) goto 500
      if (Pstart.gt.0.) P=Pstart

      pp=1d0
      call SATEST (4,T_est,pp,z,x,y,ierr,herr)            !Get an estimated saturated dew point temperature at 1 kPa.
      ierr=999
      do while (ierr.ge.100 .and. P.lt.Pmax .and. P.lt.1d6)          !Search for a temperature starting point in the vapor as a function of pressure.
        P=P*10d0                       !Increment by a factor of 10 up to 10 kPa...
        if (P.ge.10d0-e) P=P/5d0       !...then increment by a factor of 2.
        call SATTP (T,P,z,4,0,D,Dx,Dy,x,y,q,ierr,herr)
        if (Dy.gt.Dx) call SWAPV (Dx,Dy)
        if (T.lt.Tmin*0.95d0) ierr=999
        if (Dx.gt.Dc*1.5d0 .and. Dy.gt.Dc*1.5d0) ierr=999
        if (Dy.gt.Dc .and. P.lt.Pc/5d0) ierr=999
        if (ierr.gt.100 .and. ixWat.gt.0 .and. P.gt.1d3) then
          i=ixWat
          ixWat=0
          call SATTP (T,P,z,4,0,D,Dx,Dy,x,y,q,ierr,herr)
          ixWat=i
          if (Dx.gt.Dc*1.5d0 .and. Dy.gt.Dc*1.5d0) ierr=999
          if (Dy.gt.Dc .and. P.lt.Pc/5d0) ierr=999
        endif
      enddo


      do icnt=0,8                      !Continue the search if the above failed, with dew point temperatures as inputs, and calculating dew point pressures.
        if (ierr.lt.100) EXIT
        T=T_est+DBLE(icnt*20)
        Dx=0.
        Dy=0.
        call SATTP(T,pp,z,2,0,D,Dx,Dy,x,y,q,ierr,herr)
        if (ierr.lt.100) P=pp
      enddo

      do icnt=0,8                      !Continue the search if the above failed, looking for bubble point pressures or temperatures to get dew point pressures.
        if (ierr.lt.100) EXIT
        T=T_est+DBLE(icnt*20)
                         call SATTP(T,pp,z,1,0,D,Dy,Dx,y,x,q,ierr,herr)
        if (ierr.lt.100) P=pp
        if (ierr.ge.100) call SATTP(T,P,z,3,0,D,Dy,Dx,y,x,q,ierr,herr)
      enddo

      if (ierr.ge.100) goto 355

      if (Tmin.gt.T) Tmin=T
      iSatFlg2=1
      vf=1d0
      Dinc=0.1d0
      Dmult=10d0
      if (Dincr.gt.0) Dinc=Dincr
      D0=1d-12
      do n=1,20
        xArr(n,0)=XLOG(D0)   !Add several very low density states to spline arrays and fill in values later.
        D0=D0*10d0
        if (D0.gt.D) EXIT
      enddo
      n1=n
      idir=0
      ijmp=0
      Pold=P

c  Save solution to data arrays.
 10   do
        if (ijmp.eq.1) goto 20
        call CHECKSTATE (0,0,T,D,z,Err,ierr,herr)
        if (ierr.gt.100 .and. idir.eq.1) goto 20
        if (idir.eq.0) idir=1
        if (n+1.ge.narm) EXIT
        call THERM1 (T,D,z,pp,u,h,s,g)
        iex=0
        n=n+1
        if (idir.eq.1) nb=n            !The variable nb is the location where data will be loaded into arrays.
        if (idir.eq.-1) xArr(nb+1:n+1,0:nc+5)=xArr(nb:n,0:nc+5)      !Move all entries above the current density in the spline arrays one slot up.
        do i=1,nc                      !Add compositions to spline arrays.
          xArr(nb,i)=XLOG(x(i))
        enddo
        xArr(nb,   0)=XLOG(D)          !Add all calculations to spline arrays.
        xArr(nb,nc+1)=XLOG(T)
        xArr(nb,nc+2)=XLOG(P)
        xArr(nb,nc+3)=XLOG(Dx)
        xArr(nb,nc+4)=h
        xArr(nb,nc+5)=s
        iNxt=iNxt+1
        if (iNxt.gt.3) iNxt=3

 20     ijmp=0
        if ((T.lt.Tmin*0.999d0 .and. D/Dc.gt.0.5d0) .and. idir.eq.1)EXIT
        if (n+1.ge.narm) EXIT

c  Search to find any location where the first pass failed to find a saturation state.
        if (idir.eq.-1 .and. ierr.lt.100) then
          dlg=XLOG(D)
          if (dlg.gt.xArr(n,0)+e .and. xArr(n,0).ne.0.) goto 30                !Check if D>Dmax of the array, if so, exit this section and calculate the next point.
          do i=nb,2,-1
            if (dlg.gt.xArr(i-1,0)-e .and. dlg.lt.xArr(i,0)+e) EXIT            !Find the location that bounds the density.
          enddo
          if (i.le.2) EXIT             !If not bounded, routine is finished.
          nb=i                         !nb stores the current working location.
          D0=EXP(xArr(nb,0))
          Dn=EXP(xArr(nb-1,0))
          D=D0
          if (D.lt.Dinc) EXIT          !If density is less than the density increment, routine is finished.

c  Check if two adjacent points differ by more than the density increment.
          if (D0-Dn.gt.Dinc+e) then
            D=DBLE(INT(D0*10d0+0.002d0))/10d0
            if (D0-D.lt.0.02d0) D=D-Dinc
            goto 30
          else
            nb=nb-1
            D=Dn
            dif1=ABS(xArr(nb+1,nc+1)-xArr(nb,nc+1))        !Differences between the current point and the one below it.
            dif0=SUM(ABS(xArr(-3+nb:-1+nb,nc+1)-xArr(-4+nb:-2+nb,nc+1)))       !Get average of the deviations between 3 adjacent points.
            if (dif1.gt.dif0*2d0 .and. dif0.ne.0d0) then   !Determine if the point at nb has a bigger difference than the ones around it.
              call NEXTPNT (nb,-2,iNxt,0,D,T,P,Dx,x)
              goto 30
            endif
            goto 20
          endif
        endif

 25     if (D.lt.Dinc/10d0 .and. idir.eq.1) then
          D=D*Dmult          !Increase very low densities by a factor of 10 until it reaches the value of Dinc.
        else
          D=D+DBLE(idir)*Dinc
          D=DBLE(INT(D*10d0+0.49d0))/10d0        !Make nice even increments of 0.1.
        endif
        ijmp=1
        if (ABS(XLOG(D)-xArr(nb,0)).lt.1.d-8) goto 10
 30     if (idir.eq.-1) ioff=-2
        call NEXTPNT (nb,ioff,iNxt,0,D,T,P,Dx,x)
        if (lEQ0(T)) goto 40
        if (D.lt.Dc/2d0 .and. P.gt.Pc*5) P=Pc/2d0
        Tnext=T
        Dy=D






c  Density marching algorithm of G. Venkatarathnam, Ind. Eng. Chem. Res., 53:3723-3730, 2014.
c  Call with inputs of density with the pressure based algorithm.
        itt=itt+1
        call SATGV (T,P,z,vf,D,1,6,0,Dx,Dy,x,y,ierr,herr)
        if (ierr.eq.805) goto 500 !sum(z)<>1
        if (ierr.eq.151 .and. D.lt.Dc/10d0 .and. itp.lt.15) then     !If failure happens at the very beginning, go back to SATTP to get a few more points in order for NEXTPNT to work.
          itp=itp+1
          P=Pold*1.1
          call SATTP (T,P,z,4,0,Dd,Dx,Dy,x,y,q,ierr,herr)
          if (Dd.gt.Dx) ierr=999
          if (ierr.gt.0 .and. P.lt.1.) then
            P=Pold*10d0
            call SATTP (T,P,z,4,0,Dd,Dx,Dy,x,y,q,ierr,herr)
            if (ierr.gt.0 .and. P.lt.1.) then
              P=Pold*100d0
              call SATTP (T,P,z,4,0,Dd,Dx,Dy,x,y,q,ierr,herr)
            endif
          endif
          if (ierr.lt.100) D=Dd
        endif

        if (ierr.lt.100) then
          if (P.gt.Pold) Pold=P
c  Check for a possible bad point.
          if (ABS(T-Tnext)/Tnext.gt.0.2d0 .and. D.gt.Dc*1.2d0) then
            k=1
            dlg=XLOG(D)
            do i=1,7
              j=n-i-5+1
              if (j.gt.0) then
                xPnts(1:6)=xArr(j:6,0)
                yPnts(1:6)=xArr(j:6,nc+1)
                call FITL(6,3,10,xPnts,yPnts,dlg,yval,cmatrix,ierr,herr)       !Fit a polynomial to 6 data points.
                Tn=EXP(yval)
                if (ABS(T-Tn)/Tn.gt.0.2d0) k=k+1
              endif
            enddo
            if (k.gt.5) ierr=999       !If more than 5 bad hits out of 7, then the point is probably bad.
          endif

c  Solution failed, look for other ways to get next root.
        else
          if (itt.eq.2) then
            D=0.11d0
            goto 30
          elseif (itt.eq.3) then
            D=0.13d0
            goto 30
          endif
          if (iex.gt.3 .and. P.gt.500000d0) EXIT           !Once it starts failing for Type III mixtures at high pressures, just exit.
          if (iex.gt.30) EXIT
          iex=iex+1                                        !Increment the error counter
          if (iex.gt.4 .and. idir.eq.1) then
            T=EXP(xArr(nb,nc+1))
            ifl=2
            iNxt=1
            do j=1,10
              if (D.gt.Dc) ifl=1
              if (ifl.eq.2) T=T+DBLE(idir)*30d0
              if (ifl.eq.1) T=T-DBLE(idir)*30d0
              if (ifl.eq.2) then
                call SATTP (T,P,z,ifl,0,Ds,Dx,Dy,x,y,q,ierr,herr)
              else
                call SATTP (T,P,z,ifl,0,Ds,Dy,Dx,y,x,q,ierr,herr)
              endif
              if (ierr.lt.100 .and. Ds.le.D) ierr=999
              if (ierr.ge.100 .and. ifl.eq.2) T=T+20d0
              if (ierr.ge.100 .and. j.eq.3) ifl=1
              if (ierr.lt.100) D=Ds
              ijmp=0
              if (ierr.lt.100 .and. XLOG(D).gt.xArr(n,0)) goto 10
            enddo
            if (ierr.ge.100 .and. D.gt.Dc*2.5d0) EXIT
          endif
          if (D.le.Dinc*2d0+0.000001d0 .and. idir.eq.-1) EXIT
          ijmp=1
          if (D.lt.Dc*4d0 .and. T.gt.Tmin*0.999d0) goto 10
          if (D/Dc.lt.0.3d0) goto 10
          EXIT
        endif
        if (D.le.Dinc-0.000001d0 .and. idir.eq.-1) EXIT
        if (P.gt.1d7) EXIT   !For Type III mixtures, quit at 10 GPa.
        if (D.gt.Dlimit .and. Dlimit.gt.0.) EXIT
        if (P.gt.Plimit .and. Plimit.gt.0.) EXIT
        ijmp=0
      enddo
 40   if (idir.eq.1 .and. n.gt.10) then
        idir=-1
        nb=n-1
        D=EXP(xArr(nb,0))
        ijmp=1
        goto 10
      endif






c  Routine is finished, finalize splines and calculate fixed points.
      nArr=n
      if (nArr.lt.20) goto 355

c  Update first few points created for low densities.
      if (n1.gt.0) then
        if (xArr(n1,nc+1).eq.0) n1=n1+1
        dvs=xArr(n1+1,0)-xArr(n1+2,0)
        dvs=xEq0(dvs)
        dldv2=(xArr(n1+1,nc+3)-xArr(n1+2,nc+3))/dvs
        tdv =(xArr(n1+1,nc+1)-xArr(n1+2,nc+1))/dvs
        do j=n1,1,-1
          dvs0=xArr(j,0)-xArr(j+1,0)
          xArr(j,nc+1)=xArr(j+1,nc+1)+tdv*dvs0
          xArr(j,nc+3)=xArr(j+1,nc+3)+dldv2*dvs0
          D0=EXP1(xArr(j,0))
          T0=EXP3(xArr(j,nc+1))
          call THERM1 (T0,D0,z,pp,e,h,s,g)
          xArr(j,nc+2)=XLOG(pp)
          xArr(j,nc+4)=h
          xArr(j,nc+5)=s
          do i=1,nc
            xdv=(xArr(n1+1,i)-xArr(n1+2,i))/dvs
            xnew=xArr(j+1,i)+xdv*dvs0
            xArr(j,i)=xnew
            if (xnew.ge.0.) xArr(j,i)=xArr(j+1,i)*0.3d0
          enddo
        enddo
      endif

c  Go backwards through the points to search for ones that appear to be
c    wrong and delete them.
      do j=nArr-5,12,-1
        dif1=ABS(xArr(j+1,nc+2)-xArr(j,nc+2))    !Differences between the middle point and
        dif2=ABS(xArr(j-1,nc+2)-xArr(j,nc+2))    ! the ones right next to it.
        dif0=0
        do i=-5,4
          if (i.ne.0 .and. i.ne.-1)
     &        dif0=dif0+ABS(xArr(j+i,nc+2)-xArr(j+i+1,nc+2))         !Get average of the deviations between adjacent points of the 5 points to the left and the 5 to the right.
        enddo
        if (dif1.gt.dif0 .or. dif2.gt.dif0) then           !Determine if the middle point has a difference 8 times larger than the ones around it.
          D=EXP(xArr(j,0))
c         call NEXTPNT (j+3,0,3,0,D,T,P,Dx,x)    !Use points at higher densities to estimate the bad middle point.
c         call SATGV (T,P,z,vf,D,1,6,0,Dx,Dy,x,y,ierr,herr)          !Try to replace that point with a good one by calling SATGV again.
c         if (ierr.gt.0) then
            xArr(j:nArr-1,0:nc+4)=xArr(j+1:nArr,0:nc+4)    !Delete the bad point.
            nArr=nArr-1
c         endif
        endif
      enddo

c  Create splines.
      xPnts(1:nArr)=xArr(1:nArr,0)
      do k=1,nc+5
        yPnts(1:nArr)=xArr(1:nArr,k)
        call CSPLINE (nArr,xPnts,yPnts,coefs)
        cArr(1:nArr,k)=coefs(1:nArr)
      enddo
      iSpline=1
      zSpln(1:nc)=z(1:nc)

c  Find maximum and minimum of T vs. D.
      T1=0d0
      T2=0d0
      DatTmaxSpln=0d0
      DatTminSpln=0d0
      call SPLNVAL (nc+1,-2,z,0d0,Dmax,ierr,herr)          !Density upper limit of the spline curves.
      do id=0,INT(Dmax*10),1
        D=DBLE(id)/10d0
        call SPLNVAL (nc+1,0,z,D,T,ierr,herr)
        if (T.lt.T1 .and. T1.lt.T2 .and. DatTmaxSpln.le.0.) then
          DatTmaxSpln=D
          call SPLNROOT (nc+1,1,z,0d0,DatTmaxSpln,ierr,herr)         !Density at the maximum value of the temperature.
        elseif (T.gt.T1 .and. T1.gt.T2 .and. DatTmaxSpln.gt.0.) then
          DatTminSpln=D
          call SPLNROOT (nc+1,1,z,0d0,DatTminSpln,ierr,herr)         !Density at minimum value of the temperature.
          EXIT                                                       !dtmax is often less than dtmin, so don't get confused!
        endif
        T2=T1
        T1=T
      enddo

c  Determine the type of phase envelope.
      if (nArr.gt.30) then
        ip1=0
        ip2=0
        itt=0
        i100=0
        p1=-1d20
        p2=-1d20
        t1=-1d20
        t2=-1d20
        do i=1,nArr
          t=EXP(xArr(i,nc+1))
          p=EXP(xArr(i,nc+2))
          if (p.gt.1d5) i100=1
          if (p.lt.1d6) then
            if (t.lt.t1 .and. t1.lt.t2) itt=itt+1                    !Count the number of times that the temperature goes down.
            if (p.lt.p1 .and. p1.lt.p2 .and. p.lt.1d6) ip1=ip1+1     !Count the number of times that the pressure goes down.
            if (p.gt.p1 .and. p1.gt.p2 .and. ip1.gt.3) ip2=ip2+1     !Count the number of times that the pressure goes up after it has started going down.
          endif
          p2=p1
          p1=p
          t2=t1
          t1=t
        enddo

        iTflg=1
        iPflg=1
        n2=5
        if (p.gt.1d5) n2=25
        do i=nArr,nArr-n2,-1
          if (xArr(i,nc+1).gt.xArr(i-1,nc+1)) iTflg=0
          if (xArr(i,nc+2).gt.xArr(i-1,nc+2)) iPflg=0
        enddo
        if (iTflg.eq.1 .and. iPflg.eq.1) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.eq.0) MixTyp=1       !Regular, boring mixture.
        endif
        if (iTflg.eq.0 .and. iPflg.eq.0) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.ge.1) MixTyp=2       !Starts off like a regular mixture in the vapor, crossing through a critical point, and dipping down in the liquid as normal, but then turns and goes to high pressures at low liquid temperatures.
        endif
        if (iTflg.eq.0 .and. iPflg.eq.0) then
          if (itt.ge.1 .and. ip1.le.3 .and. ip2.eq.0) MixTyp=3       !Similar to a 2, but does not have a minimum in the liquid after it crosses the critical point.  Looks like an S curve.
        endif
        if (iTflg.eq.0 .and. iPflg.eq.0) then
          if (itt.eq.0 .and. ip1.eq.0 .and. ip2.eq.0) MixTyp=4       !Regular type III mixture with no negative slope anywhere on a P-T diagram.
        endif
        if (iTflg.eq.1 .and. iPflg.eq.0 .and. i100.eq.0) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.ge.1) MixTyp=5       !Somewhere between a MixTyp=1 and =2 mixture, but where it goes to much lower pressures than a 2 before starting to curve up very deep in the liquid phase.
        endif
        if (iTflg.eq.1 .and. iPflg.eq.0) then
          if (itt.ge.1 .and. ip1.eq.0 .and. ip2.eq.0) MixTyp=6       !Like a MixTyp=3 mixture, but does not complete the upper end of the S at high pressures.
        endif
        if (iTflg.eq.1 .and. iPflg.eq.0 .and. i100.eq.1) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.ge.1) MixTyp=2       !Similar to a 2, except that the temperature keeps dropping as the pressures rises.  Only happens with a few, see methane/octane.
        endif
        if (iTflg.eq.0 .and. iPflg.eq.1 .and. i100.eq.1) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.eq.0) MixTyp=3       !Similar to a 3, but the end wraps back on itself with the pressure decreasing.  Only happens with R152a/nitrogen and R22/oxygen.
        endif
        if (iTflg.eq.1 .and. iPflg.eq.1 .and. i100.eq.0) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.ge.1) MixTyp=1       !Similar to a 1, but with a funny bump in the critical region.  Only happens with a few, see toluene/cyclohexane.
        endif
        if (iTflg.eq.1 .and. iPflg.eq.0 .and. i100.eq.0) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.eq.0) MixTyp=5       !The pressure just barely starts to increase at low T, only happens with about 10 binaries.
        endif
        if (iTflg.eq.0 .and. iPflg.eq.0 .and. i100.eq.1) then
          if (itt.ge.1 .and. ip1.ge.1 .and. ip2.eq.0) MixTyp=2       !An oddball, but similar to a 2.   (R22/CO, R13/mlinolea)
        endif
        if (iTflg.eq.0 .and. iPflg.eq.0 .and. i100.eq.1) then
          if (itt.eq.0 .and. ip1.ge.1 .and. ip2.eq.0) MixTyp=4       !Similar to a 4, but eventually the pressure starts to drop.  See nitrogen and Gao's new ammonia equation.
        endif
      else
        MixTyp=-1
      endif
      if (MixTyp.eq.0) MixTyp=111

c  Calculate cricondentherm (maximum temperature) (search for dtdx=0).
      dtdx=0d0
      D=Dc/4d0
      call SPLNROOT (nc+1,1,z,dtdx,D,ierr,herr)
      call SPLNVAL  (nc+1,0,z,D,TmxT,ierr1,herr)
      call SPLNVAL  (nc+2,0,z,D,TmxP,ierr1,herr)
      Tmxd=D
      ierrTmx=0
      if (ierr.gt.0 .or. TmxT.lt.1. .or. TmxT.gt.5000.) then
        ierrTmx=992
        call REDLINEAR (1,z,TmxT,TmxP,Tmxd,ierr1,herr)
      endif

c  Calculate cricondenbar (maximum pressure) (search for dpdx=0).
      dpdx=0d0
      D=Dc
      call SPLNROOT (nc+2,1,z,dpdx,D,ierr,herr)
      if (ierr.gt.0) then
        D=Dc/2d0
        call SPLNROOT (nc+2,1,z,dpdx,D,ierr,herr)
      endif
      call SPLNVAL (nc+1,0,z,D,PmxT,ierr1,herr)
      call SPLNVAL (nc+2,0,z,D,PmxP,ierr1,herr)
      Pmxd=D
      ierrPmx=0
      if (ierr.gt.0 .or. PmxT.lt.1. .or. PmxT.gt.5000.) then
        ierrPmx=993
        call REDLINEAR (1,z,PmxT,PmxP,Pmxd,ierr1,herr)
      endif

c  Calculate the true critical point [the point at which x(i)=y(i) where i is the fluid with max x].
      i=1
      do j=1,nc
        if (z(j).gt.z(i)) i=j
      enddo
      y1=z(i)
      D=Dc
      call SPLNROOT (i,0,z,y1,D,ierr,herr)
      if (ierr.gt.0) then
        D=Dc/2d0
        call SPLNROOT (i,0,z,y1,D,ierr,herr)
      endif
      call SPLNVAL (nc+1,0,z,D,crtT,ierr1,herr)
      call SPLNVAL (nc+2,0,z,D,crtP,ierr1,herr)
      crtD=D
      if (crtP.lt.1.d-10) crtP=1000d0
      ierrCrt=0
      if (ierr.gt.0.or.crtP.gt.50000.or.crtT.lt.1..or.crtT.gt.5000.)then
        ierrCrt=999
        call REDLINEAR (1,z,crtT,crtP,crtD,ierr1,herr)
      endif

      ierr=0
      herr=' '
      goto 500

 355  call ERRNUM (355,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      nArr=0

c  Finish and exit routine.
 500  iSatFlg=0
      iSatFlg2=0
      iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      RETURN

      end                                            subroutine SATSPLN2
c
c ======================================================================
c
      subroutine SATGV (T,P,z,vf,b,ipv,ityp,isp,Dx,Dy,x,y,ierr,herr)
c
c  Calculates the bubble or dew point state with the entropy or density method
c  of GV.  The calculation method is similar to the volume based algorithm of GERG.
c  The cricondenbar and cricondentherm are estimated with the method in
c  Michelsen, Saturation Point Calculations, Fluid Phase Equilibria, 23:181, 1985.
c
c  Equations to be solved simultaneously are
c
c  Pressure based:
c
c  * f(1:n) - LOG(y/x)-LOG((fxi/nxi)/(fyi/nyi))=0
c  * f(n+1) - SUM(y(i)-x(i))=0
c  * f(n+2) - b/binput-1=0, where b = P, T, D, or s
c
c  Volume based:
c
c  * f(1:n) - LOG(y/x)-LOG((fxi/nxi)/(fyi/nyi))=0
c  * f(n+1) - SUM(y(i)-x(i))=0
c  * f(n+2) - py=px
c  * f(n+3) - b/binput-1=0, where b = P, T, D, or s
c
c  Variables:
c
c  * 1 to nc - LOG(k(i))
c  * nc+1    - LOG(T)
c  * nc+2    - LOG(P) or LOG(Dx)
c  * nc+3    -           LOG(Dy)
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        z--Overall composition (array of mole fractions)
c       vf--Vapor fraction (0>=vf>=1; the input value of density can be in either state and does not affect the outputs in Dx, Dy, x, and y):
c              vf=0,ityp=0,1 - Dew phase inputs, state in equilibrium returned in Dy and y
c              vf=1,ityp=0,1 - Liquid phase inputs, state in equilibrium returned in Dx and x
c              vf=0,ityp=6 - Inputs are returned in Dx and the x array
c                            Outputs are returned in Dy and the y array
c              vf=1,ityp=6 - Inputs are returned in Dy and the y array
c                            Outputs are returned in Dx and the x array
c        b--Input value, either entropy [J/mol-K] or density [mol/L]
c      ipv--Pressure or volume based algorithm:
c            1 - Pressure based
c            2 - Volume based
c     ityp--Input values:
c            0 - Given P, calculate T
c            1 - Given T, calculate P
c            2 - Cricondentherm condition, calculate T,P (ipv=1 only)
c            3 - Cricondenbar condition, calculate T,P (ipv=1 only)
c            5 - Given entropy, calculate T,P
c            6 - Given density, calculate T,P
c      isp--Use values from Splines as initial guesses if set to 1
c
c  Outputs:
c        (initial guesses must be sent in all variables (unless isp=1))
c        T--Temperature [K]
c        P--Pressure [kPa]
c       Dx--Density of x phase [mol/L]
c       Dy--Density of y phase [mol/L]
c        x--Composition of the x array (array of mole fractions)
c        y--Composition of the y array (array of mole fractions)
c     ierr--Error flag:  0 - Successful
c                        2 - Input D<=0
c                      151 - No convergence
c                      172 - vf<0 or vf>1
c                      191 - Derivatives are not available in PR or RDXHMX
c                      321 - Trivial solution
c                      200 - Density out of range
c                      (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c
c
c  written by G. Venkatarathnam, Indian Institute of Technology Madras
c  00-00-12  GV, original version
c  10-29-12 EWL, adapt code for use in Refprop
c  03-12-15 EWL, change calculation of f(nvars) for ityp=6

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*255 herr
      dimension xjacob(50,50),f(50)
      dimension phx(ncmax),phy(ncmax),vrb(ncmax+3)
      dimension x(ncmax),y(ncmax),z(ncmax),phi(ncmax),vrb0(ncmax+3)
      dimension xo(ncmax),yo(ncmax),drhoydnj(ncmax)
      dimension dfxdp(ncmax),dfxdt(ncmax),dfxdnj(ncmax,ncmax)
      dimension dfydp(ncmax),dfydt(ncmax),dfydnj(ncmax,ncmax)
      dimension dfxdv(ncmax),dfydv(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATGV


c     dimension x2(ncmax),y2(ncmax),tjcb(10,10)
c     dimension xincr(ncmax+3),phz1(ncmax),phx1(ncmax),phy1(ncmax)

c  Check for pure fluids.
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        if (ityp.eq.0) then
          call SATP (P,z,1,T,Dx,Dy,x,y,ierr,herr)
        elseif (ityp.eq.1) then
          call SATT (T,z,1,P,Dx,Dy,x,y,ierr,herr)
        elseif (ityp.eq.5) then
          call ERRNUM (175,0,'SATGV','entropy',0d0,0d0,0d0,ierr,herr)
        elseif (ityp.eq.6) then
          call SATD (b,z,0,kr,T,P,Dx,Dy,x,y,ierr,herr)
        endif
        if (vf.eq.0.) call SWAPV(Dx,Dy)
        RETURN
      endif

      iErrNum=iErrNum+1
      ierr=0
      iters=0
      herr=' '
      xo(1:nc)=x(1:nc)
      yo(1:nc)=y(1:nc)
      sum0=0d0
      nvars=nc+2                       ! Number of variables
      il=2
      if (ipv.eq.2) then
        il=3
        nvars=nc+3
      endif
      cf=1d0-vf
      error2=1d-12
      P1=1d0
!c  Use less strict convergence tolerance for cricondenbar and cricondentherm calculations.
!      if (ityp.eq.2) error2=1d-12
!      if (ityp.eq.2) error3=1d-5
!c  Cricondenbar can occur very close to critical point. Lower convergence criteria is therefore used.
!      if (ityp.eq.3) error2=1d-12
!      if (ityp.eq.3) error3=1d-4

c  Get values from splines if already set up.
      if (iSpline.ne.0 .and. isp.ne.0) then
        iprop=ityp
        if (ityp.eq.0) iprop=2
        if (ityp.eq.6) iprop=3
        x(1:nc)=z(1:nc)
        y(1:nc)=z(1:nc)
        Dx=b
        Dy=b
        if (vf.eq.0.) then
          call SATGUESS (1,iprop,0,0,x,T,P,Dx,h,s,Dy,y,ierr,herr)
        elseif (vf.eq.1.) then
          call SATGUESS (2,iprop,0,0,y,T,P,Dy,h,s,Dx,x,ierr,herr)
        endif
      endif

c  Check input for errors.
      sumz=SUM(z(1:nc))
      if (vf.gt.1. .or. vf.lt.0.)  goto 172
      if (ABS(sumz-1d0).ge.1d-4) then
        if (sumz.lt.1.d-10) goto 312
        goto 805
      endif
      if (Dx.le.0.) goto 102
      if (Dy.le.0.) goto 103

c  Use values for ixyzphase from previous call, unless D is >2*Dc or <Dc/10.
      if (vf.eq.0.) then
        call CRITP (x,Tc,Pc,Dc,ierr,herr)
      else
        call CRITP (y,Tc,Pc,Dc,ierr,herr)
      endif
      D=vf*Dy+cf*Dx
      if (Dx.gt.Dc*2d0 .or. Dy.lt.0.1d0)  ixpg=1
      if (Dy.gt.Dc*2d0 .or. Dx.lt.0.1d0)  iypg=1
      if (D .gt.Dc*2d0) izpg=1
      if (D .lt.Dc/5d0) izpg=2
      if (Dx.lt.Dc/5d0) ixpg=2
      if (Dy.lt.Dc/5d0) iypg=2

c  Set up variables in equations to be solved.
c  Calculate initial log of K values from input x and y.
      do i=1,nc
        vrb(i)=XLOG(XDIV(y(i),x(i)))
      enddo
      vrb(nc+1)=XLOG(T)
      vrb(nc+2)=XLOG(P)
      if (p.le.1d-10) goto 151
      Dx1=Dx
      Dy1=Dy
      if (ipv.eq.2) then
        vrb(nc+2)=XLOG(Dx1)
        vrb(nc+3)=XLOG(Dy1)
      endif
      if (ityp.eq.0) s=XLOG(P)              !LOG P
      if (ityp.eq.0 .and. ipv.eq.2) s=P     !P
      if (ityp.eq.1) s=XLOG(T)              !LOG T
      if (ityp.eq.5) s=b                    !Entropy
      if (ityp.eq.6) s=XLOG(b)              !LOG D (of y phase)
      if (s.eq.0) s=1d-10

      iters=0
      if (ABS(vrb(nc+1)).gt.1.d15) goto 151
      if (ABS(vrb(nc+1)).gt.1.d15) goto 151
      do while (iters.lt.20)
        iters=iters+1
        T1=EXP3(vrb(nc+1))
        if (T1.le.0.01d0) T1=Tc*0.8d0
        if (ipv.ne.2) then
          P1=EXP5(vrb(nc+2))
          if (P1.lt.1d-100) goto 151
        elseif (ipv.eq.2) then
          Dx1=EXP4(vrb(nc+2))
          Dy1=EXP4(vrb(nc+3))
        endif

c  Check if the guess values for the next iteration are reasonable.
        do i=1,nc
          if (vrb(i).gt.100 .or. vrb(i).lt.-200) goto 151
        enddo
        if (T1.gt.2000. .or. T1.lt.2.)    goto 151         !Temperature
        if (P1.gt.500000. .and. ipv.eq.1) goto 151         !Pressure
        if (Dx1.gt.100. .and. ipv.eq.2)   goto 151         !Density
        if (Dy1.gt.100. .and. ipv.eq.2)   goto 151         !Density

c  Call routine to analytically get all derivatives (including A with respect to n and x).
        call GETXY (z,vf,vrb,phi,x,y)
        call LGPHI (il,ixpg,T1,Dx1,P1,x,phx,dpxt,dpxd,dtx,ierr,herr)
        if (ierr.gt.0) goto 500
        if (ipv.ne.2) then
          dfxdnj(1:nc,1:nc)=dphidnj(1:nc,1:nc)
          dfxdt(1:nc)=T1*dphidt(1:nc)
          dfxdp(1:nc)=0d0
          if (P1.gt.1d-8) dfxdp(1:nc)=P1*dphidp(1:nc)
        else
          if (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) goto 191
          dfxdnj(1:nc,1:nc)=d2adnn(1:nc,1:nc)
          dfxdt(1:nc)=T1*dlnfinidt(1:nc)
          dfxdv(1:nc)=   dlnfinidv(1:nc)
          px1=P1
        endif
        call LGPHI (il,iypg,T1,Dy1,P1,y,phy,dpyt,dpyd,dty,i,herr)
        if (ipv.ne.2) then
          dfydnj(1:nc,1:nc)=dphidnj(1:nc,1:nc)
          dfydt(1:nc)=T1*dphidt(1:nc)
          dfydp(1:nc)=0d0
          if (P1.gt.1d-8) dfydp(1:nc)=P1*dphidp(1:nc)
          drhoydnj(1:nc)=dDdn(1:nc)
        else
          dfydnj(1:nc,1:nc)=d2adnn(1:nc,1:nc)
          dfydt(1:nc)=T1*dlnfinidt(1:nc)
          dfydv(1:nc)=   dlnfinidv(1:nc)
          py1=P1
        endif

c  Set up f matrix with variables in equations to be solved.
        f(1:nc)=phy(1:nc)-phx(1:nc)+vrb(1:nc)                                  !ln(fugacity coefficients)=LOG (y/x) [which equals: ln(x*phix/(y*phiy))=0]
        f(nc+1)=SUM(z(1:nc)*(phi(1:nc)-1d0)/(cf+vf*phi(1:nc)))                 !SUM(y-x)=0
        if (ipv.eq.2) f(nc+2)=(px1-py1)/1000d0
        if (ityp.eq.0) then            !Pressure input
          f(nvars)=XLOG(P1)/s-1d0
          if (ipv.eq.2) f(nvars)=s-(vf*px1+cf*py1)
        elseif (ityp.eq.1) then        !Temperature input
          f(nvars)=XLOG(T1)/s-1d0
        elseif (ityp.eq.5) then        !Entropy input
          call ENTRO (T1,Dx1,x,sx)
          call ENTRO (T1,Dy1,y,sy)
          f(nvars)=(vf*sy+cf*sx)/s-1d0
        elseif (ityp.eq.6) then        !Density input
          s1=vf*Dy1+cf*Dx1
          f(nvars)=XLOG(s1)/s-1d0
c       elseif (ityp.eq.2 .or. ityp.eq.3) then   !cricondentherm/bar input
c         izpg=0
c         call LGPHI (2,izpg,T1,Dz1,P1,z,phz,d1,d2,d3,ierr,herr)
c         term1=0d0
c         term2=0d0
c         do i=1,nc
c           if (ityp.eq.2) then
c             term1=term1+x(i)*(dfxdp(i)/P1-dphidP(i))
c             term2=term2+y(i)*(dfydp(i)/P1-dphidP(i))
c           else
c             term1=term1+x(i)*(dfxdt(i)/T1-dphidT(i))
c             term2=term2+y(i)*(dfydt(i)/T1-dphidT(i))
c           endif
c         enddo
c         f(nvars)=cf*term2+vf*term1
        endif

c  Determine Jacobian of functions f(1) to f(n+3) with analytical derivatives.
        xjacob(1:nvars,1:nvars)=0d0
        if (ipv.eq.2) then
          xjacob(nc+2,nc+1)=T1*(dpxt-dpyt)/1000d0
          xjacob(nc+2,nc+2)= Dx1*dpxd/1000d0
          xjacob(nc+2,nc+3)=-Dy1*dpyd/1000d0
        endif
        do i=1,nc
          zeta=z(i)*phi(i)/(cf+vf*phi(i))**2                         !=y(i) for vf=0 and x(i) for vf=1
          if (zeta.gt.1d20) goto 151
          xjacob(nc+1,i)=zeta                                        !d(sum(y-x))/d(ln(Ki))
          xjacob(1:nc,i)=zeta*(cf*dfydnj(1:nc,i)+vf*dfxdnj(1:nc,i))  !d(ln(fy/fx))/d(ln(Ki))
          xjacob(i,i)   =1d0+xjacob(i,i)
          xjacob(i,nc+1)=dfydt(i)-dfxdt(i)                           !d(ln(fy/fx))/d(ln(T))
          if (ipv.ne.2) xjacob(i,nc+2)=dfydp(i)-dfxdp(i)             !d(ln(fy/fx))/d(ln(P))
          if (ipv.eq.2) then
           xjacob(nc+2,i)=Rxgas*T1*zeta*(cf*dfydv(i)+vf*dfxdv(i))/1000d0
            if (ityp.eq.0)
     &      xjacob(nc+3,i)=Rxgas*T1*zeta*(cf*dfydv(i)-vf*dfxdv(i))
            xjacob(i,nc+2)= dfxdv(i)/Dx1                             !d(ln(fy/fx))/d(ln(Dx))
            xjacob(i,nc+3)=-dfydv(i)/Dy1                             !d(ln(fy/fx))/d(ln(Dy))
          endif
        enddo

c  Set up the last section in the Jacobian for the input variables.
        if (ityp.eq.1) then            !Temperature
          xjacob(nvars,nc+1)=1d0/s
        elseif (ipv.ne.2) then         !Pressure based method
          if (ityp.eq.0) then          !Pressure
            xjacob(nvars,nvars)=1d0/s
          elseif (ityp.eq.5) then      !Entropy
            call THERM (T,Dx1,x,pp,ee,hh,ss,Cv,Cpx,w,hjt)
            call THERM (T,Dy1,y,pp,ee,hh,ss,Cv,Cpy,w,hjt)
            xjacob(nvars,nc+1)=(vf*Cpy+cf*Cpx)/s                     !d(ln(s))/d(ln(T))
            xjacob(nvars,nc+2)=P1*(vf*dty/Dy1**2+cf*dtx/Dx1**2)/s    !d(ln(s))/d(ln(P))
            if (vf.gt.0. .and. vf.lt.1.) then
              do j=1,nc
                zeta=z(j)*phi(j)*cf*vf/(cf+vf*phi(j))**2
                dsdyi=-Rxgas*(XLOG(y(j))+phy(j)+dfydt(j))
                dsdxi= Rxgas*(XLOG(x(j))+phx(j)+dfxdt(j))
                xjacob(nvars,j)=zeta*(dsdyi+dsdxi)/s
              enddo
            endif
          elseif (ityp.eq.6) then      !Density
            xjacob(nvars,nc+1)=T1*(cf* dtx/Dx1+vf* dty/Dy1)/s        !d(ln(D))/d(ln(T))
            xjacob(nvars,nc+2)=P1*(cf/dpxd/Dx1+vf/dpyd/Dy1)/s        !d(ln(D))/d(ln(P))
c  added by GV 2/18/2014
c  SATGV works at vf between 0 and 1 when the density of one of the phases
c   is specified instead of the feed density.
c           do i=1,nc
c             zeta=z(i)*phi(i)/(cf+vf*phi(i))**2
c             xjacob(nvars,i)=zeta*vf*cf*drhoydnj(i)/Dy1/s
c           enddo
c           xjacob(nvars,nc+1)=T1*dty/Dy1/s
c           xjacob(nvars,nc+2)=P1/dpyd/Dy1/s
          endif
        else
          if (ityp.eq.0) then          !Pressure
            xjacob(nvars,nc+1)=-T1*(vf*dpxt+cf*dpyt)
            xjacob(nvars,nc+2)=-vf*Dx1*dpxd
            xjacob(nvars,nc+3)=-cf*Dy1*dpyd
          elseif (ityp.eq.5) then      !Not implemented for entropy
          elseif (ityp.eq.6) then      !Density
            xjacob(nvars,nc+3)=vf*1d0/s
            xjacob(nvars,nc+2)=cf*1d0/s
          endif
        endif

c  Uncomment the following lines to test the calculation of Jacobian elements
c   by numerical perturbation of variables vrb on the functions - d(f(1..nc+3))/d(vrb)
!        do i=1,nc
!          xincr(i)=1d-4               ! delta ln(Ki)
!        enddo
!        xincr(nc+1)=1d-4              ! delta T
!        xincr(nc+2)=1d-4              ! delta P or delta Dx
!        xincr(nc+3)=1d-4              !            delta Dy
!        do i=1,nvars
!          phio=EXP4(vrb(i))           !old value of fugacity coef.
!          vrb(i)=vrb(i)+xincr(i)
!          T2=EXP3(vrb(nc+1))
!          P2=EXP4(vrb(nc+2))
!          if (ipv.ne.2) then
!            call GETXY (z,vf,vrb,phi,x2,y2)
!            rhox2=Dx1
!            rhoy2=Dy1
!            call LGPHI(2,iypg,T2,rhoy2,P2,y2,phy,d1,d2,d3,ierr,herr)
!            call LGPHI(2,ixpg,T2,rhox2,P2,x2,phx,d1,d2,d3,ierr,herr)
!          else
!            call GETXY (z,vf,vrb,phi,x2,y2)
!            rhox2=EXP1(vrb(nc+2))
!            rhoy2=EXP1(vrb(nc+3))
!            if (i.le.nc) then
!c             phi(1:nc)=EXP4(vrb(1:nc))
!c             x2(1:nc)=z(1:nc)/(1d0-vf+vf*phi(1:nc))
!c             y2(1:nc)=x2(1:nc)*phi(1:nc)
!              dxki=phi(i)-phio
!              if (vf.eq.1) then
!c               dnxdn=-dxki*x(i)/(1d0-x(i))/phio
!                dnxdn=-x(i)**2/y(i)*dxki        !This is not working quite right
!                rhox2=(1d0+dnxdn)*rhox2
!              else
!                dnydn=dxki*x(i)/(1d0-y(i))
!                rhoy2=(1d0+dnydn)*rhoy2
!              endif
!            endif
!           call LGPHI(3,iypg,T2,rhoy2,py2,y2,phy,d1,d2,d3,ierr,herr)
!           call LGPHI(3,ixpg,T2,rhox2,px2,x2,phx,d1,d2,d3,ierr,herr)
!          endif
!          do j=1,nvars
!            if (j.le.nc) then
!              tjcb(j,i)=(phy(j)-phx(j)+vrb(j)-f(j))/xincr(i)
!            elseif (j.eq.nc+1) then
!              sumx=SUM(z(1:nc)*(phi(1:nc)-1d0)/(cf+vf*phi(1:nc)))
!              tjcb(j,i)=(sumx-f(j))/xincr(i)
!            elseif (j.eq.nvars) then
!              if (ipv.eq.2) then
!                P2=px2
!                if (i.eq.nvars .and. j.eq.nvars) P2=py2
!              endif
!              if (ityp.eq.0) tjcb(j,i)=(P2/s-1d0-f(j))/xincr(i)
!              if (ityp.eq.0 .and. ipv.eq.2)
!     &                       tjcb(j,i)=(s-P2-f(j))/xincr(i)
!              if (ityp.eq.1) tjcb(j,i)=(XLOG(T2)/s-1d0-f(j))/xincr(i)
!              if (ityp.eq.5) then
!                call ENTRO (T2,rhox2,x2,sx)
!                call ENTRO (T2,rhoy2,y2,sy)
!                tjcb(j,i)=((vf*sy+cf*sx)/s-1d0-f(j))/xincr(i)
!              elseif (ityp.eq.6) then
!                tjcb(j,i)=((vf*XLOG(rhoy2)+cf*XLOG(rhox2))/s-
!     &                    1d0-f(j))/xincr(i)
!              endif
!            elseif (j.eq.nc+2) then
!              tjcb(j,i)=((px2-py2)/1000d0-f(j))/xincr(i)
!            endif
!          enddo
!          vrb(i)=vrb(i)-xincr(i)
!        enddo

!c  Calculation of Jacobian by numerical perturbation of
!c   variables (x,T) of functions f(x,T).
!        if (ityp.eq.2 .or. ityp.eq.3) then
!          dt=0
!          dp=0
!          if (ityp.eq.3) dt=1d-4
!          if (ityp.eq.2) dp=1d-4
!          dd=dt+dp
!          xincr(1:nc)=1d-5
!          xincr(nc+1)=1d-4            ! K
!          xincr(nc+2)=1d-4            ! kPa
!          do i=1,nvars
!            vrb(i)=vrb(i)+xincr(i)
!            call GETXY (z,vf,vrb,phi,x,y)
!            P1=EXP4(vrb(nc+2))
!            T1=EXP3(vrb(nc+1))
!            rhox2=Dx1
!            rhoy2=Dy1
!            iz=0
!            call LGPHI (1,ixpg,T1,rhox2,P1,x,phx,d1,d2,d3,j,herr)
!            call LGPHI (1,iypg,T1,rhoy2,P1,y,phy,d1,d2,d3,j,herr)
!            call LGPHI (1,iz,   T1,Dz2,P1,z,phz,d1,d2,d3,j,herr)
!            iz=0
!           call LGPHI(1,ixpg,T1+dt,rhox2,P1+dp,x,phx1,d1,d2,d3,j,hr)
!           call LGPHI(1,iypg,T1+dt,rhoy2,P1+dp,y,phy1,d1,d2,d3,j,hr)
!           call LGPHI(1,iz,   T1+dt,Dz2,P1+dp,z,phz1,d1,d2,d3,j,hr)
!            sumx=0d0
!            do k=1,nc
!              fuggy=(phy1(k)-phy(k))/dd
!              fuggx=(phx1(k)-phx(k))/dd
!              fuggz=(phz1(k)-phz(k))/dd
!              sumx=sumx+cf*y(k)*(fuggy-fuggz)+vf*x(k)*(fuggx-fuggz)
!            enddo
!            xjacob(nc+2,i)=(sumx-f(nc+2))/xincr(i)
!            vrb(i)=vrb(i)-xincr(i)
!          enddo
!        endif

c  Check for trivial solution.
        itest=0
        do i=1,nc
          if (ABS(vrb(i)).le.1d-6) itest=itest+1
        enddo
        if (itest.eq.nc) goto 321

c  Solve linearized equations by LU decomposition.
c  f is the solution vector [xnew-xold].
        f(1:nvars)=-f(1:nvars)
        call LUDECOMP (nvars,50,xjacob,f,ierr,herr)
        if (ierr.gt.0) goto 500
        vrb0(1:nvars)=vrb(1:nvars)
        vrb(1:nvars)=vrb(1:nvars)+f(1:nvars)
        sum0=DOT_PRODUCT(f(1:nvars),f(1:nvars))/nvars

        if (ipv.eq.2) then
          iter1=0
11        tt=T1
          if (vrb(nc+1).gt.100 .or. vrb(nc+2).gt.100
     &        .or. vrb(nc+3).gt.100) goto 151
          T1=T
          Dx1=Dx
          Dy1=Dy
          T1=EXP3(vrb(nc+1))
          if (T1.le.0.01d0) T1=Tc*0.7d0
          Dx1=EXP1(vrb(nc+2))
          Dy1=EXP1(vrb(nc+3))
          call GETXY (z,vf,vrb,phi,x,y)
          call PRESS (T1,Dx1,x,px2)
          call PRESS (T1,Dy1,y,py2)
          pp=(cf*py2+vf*px2)
          pp1=(cf*py1+vf*px1)
          if (pp.le.0 .or. pp/pp1.gt.100 .or. pp/pp1.lt.0.01 .or.
     &        T1/tt.lt.0.9 .or. T1/tt.gt.1.1) then
            f(1:nvars)=f(1:nvars)/2d0
            vrb(1:nvars)=vrb0(1:nvars)+f(1:nvars)
            iter1=iter1+1
            if (iter1.lt.10) goto 11
          endif
        endif

        lconv=sum0.le.error2
c       if (ityp.eq.2 .or. ityp.eq.3) then
c         sum1=DOT_PRODUCT(f(1:nc+1),f(1:nc+1))/(nc+1)
c         lconv=sum1.le.error2 .and. ABS(f(nvars)).le.error3
c       endif

c  Iteration has converged.
        if ((lconv).and.iters.ne.1) then
          if (ityp.ne.1) T=EXP3(vrb(nc+1))
          if (ityp.ne.0) P=EXP4(vrb(nc+2))
          if (p.le.1d-10) goto 151
          call GETXY (z,vf,vrb,phi,x,y)
          if (ipv.ne.2) then
            Dx=Dx1
            Dy=Dy1
            call TPRHO (T,P,x,ixpg,1,Dx,ierr,herr)
            if (ierr.ne.0) call TPRHO (T,P,x,3-ixpg,0,Dx,ierr,herr)
            call TPRHO (T,P,y,iypg,1,Dy,ierr,herr)
            if (ierr.ne.0) call TPRHO (T,P,y,3-iypg,0,Dy,ierr,herr)
          else
            Dx=EXP1(vrb(nc+2))
            Dy=EXP1(vrb(nc+3))
            if (ityp.ne.0) then
              call PRESS (T,Dx,x,px1)
              call PRESS (T,Dy,y,py1)
              P=ABS((px1+py1)/2d0)
            endif
          endif
          itest=0
          do i=1,nc+1
            if (ABS(f(i)).le.1d-5) itest=itest+1
          enddo
          if (itest.ne.nc+1) goto 151
          if (ixpg.eq.2 .and. iypg.eq.2) goto 151
          goto 550
        endif
      enddo
      goto 151

 500  x(1:nc)=xo(1:nc)
      y(1:nc)=yo(1:nc)
 550  iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      RETURN

 102  call ERRNUM (2,1,'SATGV',' ',Dx,0d0,0d0,ierr,herr)
      goto 500
 103  call ERRNUM (2,1,'SATGV',' ',Dy,0d0,0d0,ierr,herr)
      goto 500
 151  call ERRNUM (151,0,'SATGV',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 172  call ERRNUM (172,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 191  call ERRNUM (191,0,'SATGV',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 312  call ERRNUM (312,0,'SATGV',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 321  call ERRNUM (321,0,'SATGV',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 805  call ERRNUM (805,0,'SATGV',' ',0d0,0d0,0d0,ierr,herr)
      goto 500

      end                                               subroutine SATGV
c
c ======================================================================
c
      subroutine DLDV (T,x,y,P,Dl,Dv,ierr,herr)
c
c  Find a state where the pressures at the liquid and vapor densities
c  (calculated at the x and y input compositions) are equal.
c  T, x, and y are inputs, P, Dl, and Dv are outputs.
c  This is an internal routine to the subroutine SATT, but could be
c  used elsewhere.

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax)


      iErrNum=iErrNum+1
      tolr=1d-4
      call ISPURE (x,icomp)
      call SATMAX (icomp,x,Tc,Pc,Dc,ierr,herr)
      Dl=Dc*1.1d0
      Dv=Dc*0.95d0
      do it=1,30

c  Check liquid density for valid state.
        do it2=1,100
          call PRESS (T,Dl,x,Pl)
          call DPDDX (T,Dl,x,dPdDl)
          call DPDD2 (T,Dl,x,dPdDl2)
          if (Dl.gt.Dc*5d0) then
            if (it2.gt.10 .and. Pl.gt.100000d0) EXIT
            Dl=Dl/3d0
          elseif (dPdDl.gt.1d4 .and. ABS(Pl).gt.Pc*1.5d0) then
            Dl=Dl*1.3d0
          elseif (dPdDl2.lt.0. .or. dPdDl.lt.0.) then      !Check for proper curvature in the liquid (should be positive).
            Dl=Dl*1.05d0
          else
            EXIT
          endif
        enddo

c  Check vapor density for valid state.
        do it3=1,100
          call PRESS (T,Dv,y,Pv)
          call DPDDX (T,Dv,y,dPdDv)
          call DPDD2 (T,Dv,y,dPdDv2)
          if (ABS(dPdDv).gt.1d5 .and. ABS(Pv).gt.Pc*1.5d0) then
            Dv=Dv/1.6d0
          elseif (dPdDv2.gt.0. .or. dPdDv.lt.0.) then      !Check for proper curvature in the vapor (should be negative).
            Dv=Dv*0.95d0
          else
            EXIT
          endif
        enddo

c  Get deltas for Dl and Dv, update the densities, and check new pressures for convergence.
        P=(Pl+Pv)/2d0
        if (XDIV(ABS(Pl-Pv),P).lt.tolr) goto 500
        delDv=Dv/10d0
        delDl=Dl/20d0
        if (ABS(Pv-P).gt.1d-10 .and. ABS(dPdDv).gt.1d-10)
     &      delDv=1d0/(-dPdDv/(Pv-P)+dPdDv2/2d0/dPdDv)
        if (ABS(Pl-P).gt.1d-10 .and. ABS(dPdDl).gt.1d-10)
     &      delDl=1d0/(-dPdDl/(Pl-P)+dPdDl2/2d0/dPdDl)
        if (ABS(XDIV(delDv,Dv)).gt.0.5d0) delDv=SIGN(0.2d0,delDv)
        if (ABS(XDIV(delDl,Dl)).gt.1.)    delDl=SIGN(1d0,delDl)
        Dv=Dv+delDv
        Dl=Dl+delDl
        if (it.eq.10 .and. ABS(Dl-Dv).lt.0.1d0) then
          Dv=Dv/2d0
          Dl=Dl*2d0
        endif
        if (it.gt.10 .and. P.gt.1d5 .and. ABS(Pl-Pv).gt.1d4) EXIT
      enddo
      ierr=151
 500  iErrNum=iErrNum-1
      RETURN

      end                                                subroutine DLDV
c
c ======================================================================
c
      subroutine HEMAXX (z,ieflag,hemax,Themax,Dhemax,ierr,herr)
c
c  Find the maximum enthalpy or energy on the saturated vapor line.
c
c  Inputs:
c        z--Composition (array of mole fractions)
c   ieflag--Set to 0 for enthalpy, 1 for energy
c
c  Outputs:
c    hemax--Maximum enthalpy or energy [J/mol]
c   Themax--Temperature at maximum enthalpy or energy [K]
c   Dhemax--Density at maximum enthalpy or energy [mol/L]
c     ierr--Error flag:  0 - Successful
c                        (See subroutine LIMITX for other possible error numbers.)
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  06-15-11 EWL, original version, remove piece from SATH and place here
c  12-12-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character herr*255
      dimension x(ncmax),y(ncmax),z(ncmax),v(20),f(20)


c  Check if values have already been calculated.  If so, load them and return.
      ierr=0
      herr=' '
      hemax=0d0
      Themax=300d0
      Dhemax=0d0
      if (ieflag.eq.0 .and. ABS(Hhmax).gt.1d-15) goto 500
      if (ieflag.eq.1 .and. ABS(Eemax).gt.1d-15) goto 500

      T=300d0
      ibt=iLetterH
      if (ieflag.eq.1) ibt=iLetterE
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call ROOTSEARCH (0,0,0,v,f,T,0,ib,lneg,lpos)         !Initialize root finding variables.
      call LIMITS  ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      jr=1
      T=Tc*0.9d0
      dt=0.1d0

c  Use values from Splines if set up by call to SATSPLN.
      if (iSpline.ne.0 .and. ieflag.eq.0) then
        call SATGUESS (2,-4,0,0,z,T,P,D,h,s,Dv,y,ierr,herr)
        if (T.lt.Tmin) T=Tc*0.8d0
      endif

c  ---Start of main loop---
      Thigh=0
      iercnt=0
      Told=T
      do it=1,40
        if (it.eq.20 .or. it.eq.25 .or. it.eq.30) then
          if (.not.lneg .or. .not.lpos) T=(Thigh+Tc)/2d0   !If not converging, go half way between highest T and Tc to look for root.
        endif
        T1=T+dt
        T2=T-dt
        call SATT (T1,z,2,P,D,Dvp,x,y,ierr,herr)
        if (ierr.lt.100) then
          call SATT (T2,z,2,P,D,Dvn,x,y,ierr,herr)
          if (ierr.lt.100) then
            Told=T
            if (T.gt.Thigh) Thigh=T
            call HESCALC (ibt,T1,Dvp,z,hp)
            call HESCALC (ibt,T2,Dvn,z,hn)
            v(2)=T
            f(2)=hp-hn                           !Search for the location where the difference in enthalpy (i.e., slope) is zero.
            call ROOTSEARCH (jr,0,0,v,f,T,it,ib,lneg,lpos)
            jr=1234
            if (it.eq.1 .or. it-1.le.iercnt) T=T+Tc/100d0
            if (ABS(f(2)).lt.1d-5 .and. f(2).ne.0.) EXIT   !Check for convergence.
            iercnt=0
          endif
        endif
        if (ierr.ge.100) then
          iercnt=iercnt+1
          if (MOD(iercnt,2).eq.0) T=Told*(1d0+0.01d0*DBLE(iercnt))
          if (MOD(iercnt,2).ne.0) T=Told*(1d0-0.02d0*DBLE(iercnt))
          if (T.gt.Tc .and. icomp.ne.0) T=T-50d0
          if (T.gt.Tc+50 .and. icomp.eq.0) T=T-50d0
        endif
      enddo

      if (ierr.le.0 .and. ABS(f(2)).lt.1d-3) then
        if (ieflag.eq.0) then
          Thmax=T
          Dhmax=(Dvp+Dvn)/2d0
          Hhmax=(hp+hn)/2d0*1.00001d0  !Increase slightly due to numerical calculations.
        elseif (ieflag.eq.1) then
          Temax=T
          Demax=(Dvp+Dvn)/2d0
          Eemax=(hp+hn)/2d0*1.00001d0
        endif
      endif

 500  if (ieflag.eq.0 .and. ABS(Hhmax).gt.1d-15) then
        hemax=Hhmax
        Themax=Thmax
        Dhemax=Dhmax
      elseif (ieflag.eq.1 .and. ABS(Eemax).gt.1d-15) then
        hemax=Eemax
        Themax=Temax
        Dhemax=Demax
      endif

      end                                              subroutine HEMAXX

c
c ======================================================================
c
      subroutine SMAXX (z,ierr,herr)
c
c  Find the maximum and minimum entropy along the saturated vapor dome, and
c  the minimum entropy along the melting line.
c
c  Inputs:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c    All outputs are stored in the common blocks except ierr and herr.
c    Tsmax--Temperature at maximum entropy [K]
c    Dsmax--Density at maximum entropy [mol/L]
c    Ssmax--Maximum entropy [J/mol-K]
c    Tsmin--Temperature at minimum entropy [K]
c    Dsmix--Density at minimum entropy [mol/L]
c    Ssmin--Minimum entropy [J/mol-K]
c   Tsmin2--Temperature at minimum entropy along melting line [K]
c   Dsmin2--Density at minimum entropy along melting line [mol/L]
c   Ssmin2--Minimum entropy along melting line [J/mol-K]
c  Values of -1 for the temperatures will be returned if no root exists.
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  06-15-11 EWL, original version, remove piece from SATS and place here
c  12-12-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),v(20),f(20)


      ierr=0
      herr=' '
      if (Tsmax.gt.0.) RETURN

c  Check if values have already been calculated.  If so, load them and return.
      iSatFlg=0
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS  ('EOS',z,Tmin,Tmax,Dmax,Pmax)

c  Check first to see if melting line is double valued.
      istart=1
      if (icomp.ne.0) then
        if (iFluidID(icomp).eq.1 .or. iFluidID(icomp).eq.3) then     !Check for water or heavy water.
          T=-998d0
          call MLTH2O (T,p,D)
          call ENTRO (T,D,z,s)
          Tsmin2=T           !Save to common blocks.
          Dsmin2=D
          Ssmin2=s
        else
          T=Ttpn(icomp)
          call SATT (T,z,1,P,Dl,Dv,x,y,ierr,herr)
          if (ierr.le.0) then
            call ENTRO (T,Dl,z,s1)
            T1=T+1d0
            call SATT (T1,z,3,P,Dl,Dv,x,y,ierr,herr)
            call ENTRO (T1,Dl,z,s2)
            if (s2.lt.s1 .and. ABS(P).gt.1d-10) istart=0             !Melting line is doubled valued if s2<s1.
          endif
        endif
      endif

c  ---Start of main loop---
      do i=istart,2
        k=2
        jr=1
        dt=0.1d0
        if (i.eq.0) k=3
        T=Tmin+Tmin/20d0                         !Starting points for roots.
        if (i.eq.1) T=Tc-Tc/20d0
        if (i.eq.2) T=0.7*Tmin+0.3*Tc
        call ROOTSEARCH (0,0,0,v,f,T,0,ib,lneg,lpos)       !Initialize root finding variables.
        do it=1,15
          T1=T+dt
          T2=T-dt
          call SATT (T1,z,k,P,D,Dvp,x,y,ierr,herr)
          if (ierr.le.0) then
            call SATT (T2,z,k,P,D,Dvn,x,y,ierr,herr)
            if (ierr.le.0) then
              call ENTRO (T1,Dvp,z,sp)
              call ENTRO (T2,Dvn,z,sn)
              v(2)=T
              f(2)=sp-sn               !Search for the location where the difference in entropy (i.e., slope) is zero.
              call ROOTSEARCH (jr,0,0,v,f,T,it,ib,lneg,lpos)
              jr=1234
              if (it.eq.1) T=T+Tc/100d0
              if (ABS(f(2)).lt.1d-7) EXIT        !Check for convergence.
            endif
          endif
          if (ierr.gt.0) T=T+Tc/100d0
          if (T.gt.Tc) T=T-50d0
          if (T.le.0.) T=Tc/2d0
        enddo

        if (ierr.le.0 .and. ABS(f(2)).lt.1d-5) then        !If the iteration failed, then generally there are no roots in the vapor.
          if (i.eq.1) then
            Ssmax=(sp+sn)/2d0*1.00001d0          !Increase slightly due to numerical calculations.
            Tsmax=T
            Dsmax=(Dvp+Dvn)/2d0
          elseif (i.eq.2) then
            Ssmin=(sp+sn)/2d0*0.99999d0
            Tsmin=T
            Dsmin=(Dvp+Dvn)/2d0
          elseif (i.eq.0) then
            Ssmin2=(sp+sn)/2d0
            Tsmin2=T
            Dsmin2=(Dvp+Dvn)/2d0
          endif
        endif
      enddo

      end                                               subroutine SMAXX
c
c ======================================================================
c
      subroutine HVAPZTN (T,P,z,iTP,hvap,hvap0,ierr,herr)
c
c  Compute the heat of vaporization as a function of temperature or
c  pressure, and composition.  For a mixture, the calculation can be
c  done at either constant temperature or constant pressure through
c  the 2-phase, resulting in different values.  The enthalpy change
c  from liquid to zero pressure is returned in hvap0.
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        z--Composition (array of mole fractions)
c      iTP--If set to 1, hvap is calculated at constant temperature for a mixture.
c           If set to 2, hvap is calculated at constant pressure for a mixture.
c
c  Outputs:
c     hvap--Heat of vaporization [J/mol]
c    hvap0--Heat of vaporization plus enthalpy change to zero pressure [J/mol]
c     ierr--Error flag:  0 - Successful
c                      151 - Saturation routine failed to converge
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  03-20-17 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xy(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::HVAPZTN


      hvap=0d0
      hvap0=0d0
      ierr2=0
      call ISPURE (z,icomp)
      if (iTP.ne.2) then
                        call SATT (T,z,1,pp,Dl,Dv,x,y ,ierr1,herr)
        if (icomp.eq.0) call SATT (T,z,2,pp,dd,Dv,x,xy,ierr2,herr)
      else
                        call SATP (P,z,1,tt,Dl,Dv,x,y ,ierr1,herr)
        if (icomp.eq.0) call SATP (P,z,2,tt,dd,Dv,x,xy,ierr2,herr)
      endif
      if (ierr1.gt.0 .or. ierr2.gt.0) then
        call ERRNUM (151,0,'HVAPZTN',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif
      call HESCALC (iLetterH,T,Dl,z,hl)
      call HESCALC (iLetterH,T,Dv,z,hv)
      call HESCALC (iLetterH,T,0d0,z,h0)
      hvap =hv-hl
      hvap0=h0-hl

      end                                             subroutine HVAPZTN
c
c ======================================================================
c
      subroutine CSATK (icomp,T,kph,P,D,Csat,ierr,herr)
c
c  Compute the heat capacity along the saturation line as a function of
c  temperature for a given component.
c
c  Csat can be calculated in several ways
c  Csat = T*(dS/dT[sat])
c  Csat = Cp - T*(dV/dT)(dP/dT[sat]) with dVdT at constant pressure
c  Csat = Cp - beta/D*hvap/(vliq - vvap)
c  where beta is the volume expansivity
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c      kph--Phase flag:  1 - Liquid calculation
c                        2 - Vapor calculation
c
c  Outputs:
c        P--Saturated pressure [kPa]
c        D--Saturated molar density [mol/L]
c     Csat--Saturated heat capacity [J/mol-K]
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-30-98 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CSATK


      Csat=0d0
      z(1:nc)=0d0
      z(icomp)=1d0
      call SATT (T,z,kph,P,Dl,Dv,x,y,ierr,herr)
      if (ABS(Dl-Dv).lt.1.d-5) ierr=999
      if (ierr.gt.0 .or. Dl.le.0.) then
        call ERRNUM (151,0,'CSATK',' ',0d0,0d0,0d0,ierr,herr)
        P=0d0
        D=0d0
        RETURN
      endif
      D=Dl
      if (kph.eq.2) D=Dv
      call DPDTX (T,D,z,dPdT)
      call DPDDX (T,D,z,dPdD)
      call THERM (T,Dv,z,pp,e,hv,s,Cv,Cp,w,hjt)
      call THERM (T,Dl,z,pp,e,hl,s,Cv,Cp,w,hjt)
      call THERM (T,D ,z,pp,e,h ,s,Cv,Cp,w,hjt)
      beta=dPdT/dPdD/D
      Csat=Cp-beta/D*(hl-hv)/(1d0/Dl-1d0/Dv)

      end                                               subroutine CSATK
c
c ======================================================================
c
      subroutine DPTSATK (icomp,T,kph,P,D,Csat,dPdT,ierr,herr)
c
c  Compute the heat capacity and dP/dT along the saturation line as a
c  function of temperature for a given component.  See also subroutine CSATK.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c      kph--Phase flag:  1 - Liquid calculation
c                        2 - Vapor calculation
c
c  Outputs:
c        P--Saturated pressure [kPa]
c        D--Saturated molar density [mol/L]
c     Csat--Saturated heat capacity [J/mol-K] (same as that called from CSATK)
c     dPdT--dP/dT along the saturation line [kPa/K]
c           (this is not dP/dT at the saturation line for the single phase
c            state, but the change in saturated vapor pressure as the
c            saturation temperature changes.)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-25-06 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DPTSATK


      Csat=0d0
      dPdT=0d0
      z(1:nc)=0d0
      z(icomp)=1d0
      call SATT (T,z,kph,P,Dl,Dv,x,y,ierr,herr)
      if (ABS(Dl-Dv).lt.1.d-5) ierr=999
      if (ierr.gt.0 .or. Dl.le.0. .or. Dv.le.0.) then
        call ERRNUM (151,0,'DPTSATK',' ',0d0,0d0,0d0,ierr,herr)
        P=0d0
        D=0d0
        RETURN
      endif
      D=Dl
      if (kph.eq.2) D=Dv
      call DPDTX (T,D,z,dPdT)
      call DPDDX (T,D,z,dPdD)
      call THERM (T,Dv,z,pp,e,hv,s,Cv,Cp,w,hjt)
      call THERM (T,Dl,z,pp,e,hl,s,Cv,Cp,w,hjt)
      call THERM (T,D ,z,pp,e,h ,s,Cv,Cp,w,hjt)
      beta=dPdT/dPdD/D
      Csat=Cp-beta/D*(hl-hv)/(1d0/Dl-1d0/Dv)
      dPdT=          (hl-hv)/(1d0/Dl-1d0/Dv)/T

      end                                             subroutine DPTSATK
c
c ======================================================================
c
      subroutine CV2PK (icomp,T,D,Cv2p,Csat,ierr,herr)
c
c  Compute the isochoric heat capacity in the two phase (liquid+vapor) region.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Density [mol/L] if known
c           If D=0, then a saturated liquid state is assumed.
c
c  Outputs:
c     Cv2p--Isochoric two-phase heat capacity [J/mol-K]
c     Csat--Saturation heat capacity [J/mol-K]
c           (Although there is already a Csat routine in Refprop,
c            it is also returned here.  However, the calculation
c            speed is slower than Csat.)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  03-30-05 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CV2PK


      Cv2p=0d0
      dT=0.01d0
      i=1
      T1=T+dT
      T2=T-dT
      z(1:nc)=0d0
      z(icomp)=1d0
      call SATT (T ,z,i,P ,Dl ,Dv ,x,y,ierr,herr)
      if (ierr.ge.100) goto 151
      call SATT (T1,z,i,P1,Dl1,Dv1,x,y,ierr,herr)
      if (ierr.ge.100) goto 151
      call SATT (T2,z,i,P2,Dl2,Dv2,x,y,ierr,herr)
      if (ierr.ge.100) goto 151
      if (D.le.0.) D=Dl
      if (ABS(Dl-Dv).lt.1.d-5) goto 151
      if (Dl.le.0. .or. Dv.le.0.) goto 151
      call DPDTX (T,Dl,z,dPdT)
      call DPDDX (T,Dl,z,dPdD)
      call THERM (T,Dv,z,pp,e,hv,s,Cv,Cp,w,hjt)
      call THERM (T,Dl,z,pp,e,hl,s,Cv,Cp,w,hjt)
      beta=dPdT/dPdD/Dl
      dPdTsat=(hv-hl)/T/(1d0/Dv-1d0/Dl)          ! d(P)/d(T) at sat. liq.
      dDdTsat=(Dl1-Dl2)/2d0/dT                   ! d(D)/d(T) at sat. liq.
      d2PdTsat=(P2+P1-2d0*P)/dT**2               ! d^2(P)/d(T)^2 at sat. liq.
      Csat=Cp-beta/Dl*dPdTsat*T
      Cv2p=Csat+T/Dl**2*dDdTsat*dPdTsat+T*(1d0/D-1d0/Dl)*d2PdTsat
      RETURN

 151  call ERRNUM (151,0,'CV2PK',' ',0d0,0d0,0d0,ierr,herr)

      end                                               subroutine CV2PK
c
c ======================================================================
c
      subroutine TPRHOB (T,P,rho1,rho2,z,D,ierr,herr)
c
c  Iterate for density given temperature, pressure, and an upper and lower
c  bound for the density.  This routine is only meant to replace TPRHO
c  in special cases near the critical point (and is only used by SATT).
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c     rho1--First bound on density [mol/L]
c     rho2--Second bound on density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        D--Molar density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      151 - Failed to converge (no error message returned)
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-08-01 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)


      ierr=0
      herr=' '
      tolr=1d-9
      D1=rho1
      D2=rho2
      if (D1.gt.D2) call SWAPV (D1,D2)
      D=(D1+D2)/2d0
      do it=1,25
        call PRESS (T,D,z,P1)
        call DPDDX (T,D,z,dPdD)
        D0=D
        if (dPdD.gt.1d-20) then
          D=D+(P-P1)/dPdD              !Use false position to get next root.
        else
          D=(D0+D1+D2)/3d0
        endif
        if (D.lt.D1) D=(D0+D1)/2d0     !Keep root within bounds.
        if (D.gt.D2) D=(D0+D2)/2d0
        if (ABS(P1-P).lt.tolr) RETURN
        call TOLRNC (tolr,it,10,15,0,0,0)        !Loosen tolerance if not converging.
      enddo

      ierr=151
      end                                              subroutine TPRHOB
c
c ======================================================================
c
      subroutine GETXY (z,vf,philn,phi,x,y)
c
c  Calculate the liquid and vapor compositions with the known values
c  of the overall composition, the vapor fraction, and the log of the
c  fugacity coefficients.
c
c  Inputs:
c        z--Overall composition (array of mole fractions)
c       vf--Vapor fraction (0>=vf>=1)
c           Set vf=0 for liquid and vf=1 for vapor.
c    philn--Natural log of the fugacity coefficients [-]
c
c  Outputs:
c      phi--Fugacity coefficients [-]
c        x--Liquid composition (array of mole fractions)
c        y--Vapor composition (array of mole fractions)
c
c  written by G. Venkatarathnam, Indian Institute of Technology Madras
c  00-00-12  GV, original version
c  10-29-12 EWL, adapt code for use in Refprop
c  07-28-14 EWL, add check for y calculation with large philn

      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax),philn(ncmax+3),phi(ncmax)

      x(1:nc)=z(1:nc)
      y(1:nc)=z(1:nc)
      do i=1,nc
        phi(i)=1d10
        phi(i)=EXP4(philn(i))
        xdiv=1d0-vf+vf*phi(i)
        if (ABS(xdiv).gt.1d-10 .and. xdiv.lt.1d+20) then
          x(i)=z(i)/xdiv
          y(i)=x(i)*phi(i)
        endif
      enddo
      sumx=SUM(x(1:nc))
      sumy=SUM(y(1:nc))
      if (ABS(sumx).gt.0.) x(1:nc)=x(1:nc)/sumx
      if (ABS(sumy).gt.0.) y(1:nc)=y(1:nc)/sumy
      end                                               subroutine GETXY
c
c ======================================================================
c
      subroutine XYFUG (y,f1,f2,tolr,sum1,sumx,iconvrg,ierr,herr)
c
c  Internal routine to SATT and SATP, not meant to be called elsewhere.

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),f1(ncmax),f2(ncmax)

      iconvrg=0
      ierr=0
      herr=' '
      x(1:nc)=0d0
      do i=1,nc              !Calculate new y's with ratio of fugacities; inner loop has converged when y's change by less than a convergence tolerance.
        if (y(i).gt.0.) then           !Check for components not in use.
          if (f2(i).gt.1d-20 .and. f1(i).gt.0.) then
            x(i)=y(i)*f1(i)/f2(i)
          else
            x(i)=x(i)+0.0001d0         !Add a little to keep sum<>1.
          endif
        endif
      enddo
      sumx=xEq0(SUM(x(1:nc)))
      if (sumx.le.0. .or. sumx.gt.1d6) ierr=999
      x(1:nc)=x(1:nc)/sumx             !Normalize the y compositions.
      sum0=SUM(ABS(y(1:nc)-x(1:nc)))   !Calculate change in compositions.
      y(1:nc)=x(1:nc)
      if (sum0.lt.tolr .or. ABS(sum0-sum1).lt.tolr*100d0) iconvrg=1            !Compositions and fugacities are in agreement.
      sum1=sum0
      end                                               subroutine XYFUG
c
c ======================================================================
c
      subroutine LGPHI (iderv,iphase,T,D,P,z,philn,dPdT,dPdD,dDdT,
     &                  ierr,herr)
c
c  Internal routine to calculate various properties needed by the
c  VLE iterative routines.
c
c  Inputs:
c    iderv--Set to 1 for calculation of fugacity coefficient (philn) only.
c           Set to 2 for full calculations (when calling PHIDERV) for use in other places.
c           Set to 3 to calculate P as function of T and D,
c                     and for full calculations of everything else.
c        T--Temperature [K]
c        D--Density [mol/L] (if iderv=3)
c        P--Pressure [kPa] (if iderv<>3)
c        z--Composition (array of mole fractions)
c   iphase--Input phase: 1 - Liquid
c                        2 - Vapor
c                        0 - Phase with minimum gibbs energy
c
c  Outputs:
c   iphase--Correct phase, computed with GV's PIP criteria [2 for vapor, 1 for liquid]
c        D--Density [mol/L] (input value should be an initial guess)
c        P--Pressure [kPa] (if iderv=3)
c    philn--ln of fugacity coefficient [-]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  written by G. Venkatarathnam, Indian Institute of Technology Madras
c  00-00-12  GV, original version
c  10-29-12 EWL, adapt code for use in Refprop
c  10-17-13  GV, change phi(i)=1 to philn(i)=1
c  01-10-14  GV, change philn for iderv=1 to avoid errors with LOG(phi) for negative or zero phi

      include 'COMMONS.INC'
      dimension z(ncmax),philn(ncmax)
      dimension dadn(ncmax),dnadn(ncmax)
      character herr*255


      philn(1:nc)=1d0
      if (D.le.0d0) then
        ierr=999
        RETURN
      endif
      iErrNum=iErrNum+1
      call CRITP (z,Tc,Pc,Dc,ierr1,herr)
      if (iderv.eq.3) then
        call PRESS (T,D,z,P)
      else
        if (iphase.ne.0 .and. iphase.ne.999) then
          call TPRHO (T,P,z,iphase,1,D,ierr,herr)
          if (ierr.ne.0) call TPRHO (T,P,z,3-iphase,0,D,ierr,herr)
          if (D.eq.0.) D=1d-20
        else       !Phase with minimum gibbs energy.
          grv=1d36           !No convergence
          grl=1d36
          call TPRHO (T,P,z,1,0,Dl,ierr,herr)
          if (ierr.le.0) call GIBBS (T,Dl,z,arl,grl)
          call TPRHO (T,P,z,2,0,Dv,ierr,herr)
          if (ierr.le.0) call GIBBS (T,Dv,z,arv,grv)
          D=Dl
          if (grv.le.grl) D=Dv
          if (D.eq.0.) D=1d-20
        endif
      endif
      if (D.lt.1d-20) D=Dc+1d-20

      zz=P/D/Rxgas/T
      if (ABS(zz).gt.100.) goto 500
      if (iderv.eq.1) then
        call PHIDERV (iderv,T,D,z,dadn,dnadn,ierr,herr)
        philn(1:nc)=dnadn(1:nc)-LOG(zz)          !(5.43 of GERG)
      else
        call PHIDERV (iderv,T,D,z,dadn,dnadn,ierr,herr)
        if (ierr.gt.0) goto 500
        RTrho=Rxgas*T*D
        do i=1,nc
          philn(i)=1d0
          if (ABS(dnadn(i)).lt.100.) then
c  Correct calculation of LOG(phi) is
c           if (P.gt.0) philn(i)=LOG(RTrho/P)+dnadn(i)
c  But use this instead since P can be negative, and in the solution
c  of VLE the P's must be equal, so they cancel out.  The 1d3 keeps the
c  value of RTrho on the same scale as dnadn (and also cancels out).
            philn(i)=LOG(RTrho/1d3)+dnadn(i)
          endif
        enddo
      endif

      if (D.gt.Dc*2d0) then
        iphase=1
        call DERVPVT1 (T,D,z,P,dPdD,dPdT,dDdP,dDdT,dTdP,dTdD)
      elseif (D.lt.Dc/5d0) then
        iphase=2
        call DERVPVT1 (T,D,z,P,dPdD,dPdT,dDdP,dDdT,dTdP,dTdD)
      else
        call DERVPVT (T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                    dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                    dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
        pifactor=2d0
        if (dPdT.ne.0. .and. dPdD.ne.0.)
     &    pifactor=2d0-D*(d2pdtd/dPdT-d2PdD2/dPdD)
        iphase=2
        if (pifactor.gt.1.) iphase=1
        if (D.gt.Dc*2d0) iphase=1      !Special case for mixtures with water where pifactor<0 in the liquid
      endif
 500  iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      RETURN

      end                                               subroutine LGPHI
c
c ======================================================================
c
      subroutine SATGUESS (kph,iprop,iFlag,ispl,
     &                     x,T,P,D,h,s,Dy,y,ierr,herr)
c
c  For a pure fluid, call the ancillary equations to obtain close
c  estimates for the saturation boundaries.  The difference between
c  these values and those from SATT, SATP, etc., depend on how well
c  the ancillary equation was fitted, but generally they are within
c  0.1%, except for the saturation densities within several degrees
c  of the critical temperature.
c
c  For a mixture, calculate approximate values from the spline curves for
c  the saturation boundary.  Subroutine SATSPLN must be called in order
c  for this to work.
c
c  The input property should be placed in the corresponding variable
c  for T, P, D, h, or s.  Inputs of h and s only work for mixtures
c  when SATSPLN has been called.
c
c  Inputs:
c      kph--Input phase; 1-liquid, 2-vapor
c           When maximum in the property does not occur near the critical point,
c           then kph=1 returns the root at the higher density and kph=2
c           returns the root at the lower density.
c    iprop--Input property:
c           1 - Temperature
c           2 - Pressure
c           3 - Density
c           4 - Enthalpy
c           5 - Entropy
c           6 - Energy (currently only working for pure fluids)
c           11 to 15 - 1st derivative of the property (for iprop-10)
c                      returned in Dy with respect to density (only for iFlag=0)
c           21 to 25 - 2nd derivative of the property (for iprop-20)
c                      returned in Dy with respect to density (only for iFlag=0)
c           101 - Check if the x array is identical to those sent to SATSPLN (only for iFlag=0)
c                 (for negative values of 1 to 5, find the location of zero slope
c                 of the property with respect to D) (only for iFlag=0)
c    iFlag--Flag indicating which method to use to obtain the outputs:
c           0 - Use default and best methods, generally ancillary equations for
c               pure fluids and splines (calculated from call to SATSPLN) for mixtures
c          11 - Use Rackett technique to get density from T and P
c               (value of kph and iprop ignored)
c          12 - Use initial guess equations of Lemmon
c     ispl--Dummy flag
c        x--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        D--Density [mol/L]
c        h--Enthalpy or energy [J/mol] (not returned for a pure fluid)
c        s--Entropy [J/mol-K] (not returned for a pure fluid)
c       Dy--Equilibrium phase density [mol/L]
c        y--Equilibrium phase composition (array of mole fractions)
c        (h, s, and y are only returned when splines are used to calculate values.)
c     ierr--Error flag:  0 - Successful
c                      331 - Splines not available for saturation calculations
c                      332 - Initialize variable d72l first before calling SATGUESS
c                     -311 - Compositions not identical to that used in the call to SATSPLN
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-28-13 EWL, original version

      include 'COMMONS.INC'
      character*255 herr,herr1,hes*3
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATGUESS


      if (T.lt.1 .and. iprop.eq.1) RETURN
      iErrNum=iErrNum+1
      if (ispl.eq.0) ierr=0            !ispln is not yet used, this line keeps the compiler happy.
      ierr=0
      ierrpv=0
      iTPrho=0
      herr=' '
      iFlg=iFlag
      iphase=ABS(kph)
      iFlash=-1
      if (kph.eq.1 .and. iprop.eq.1) iFlash=1
      if (kph.eq.2 .and. iprop.eq.1) iFlash=2
      if (kph.eq.1 .and. iprop.eq.2) iFlash=3
      if (kph.eq.2 .and. iprop.eq.2) iFlash=4

c  Check for composition called from SATSPLN (kph is used to pass the spline array number).
      if (iprop.eq.101) then
        zSum=SUM(ABS(x(1:nc)-zSpln(1:nc)))
        if (.not.lEQ0(zSum))
     &    call ERRNUM(-311,0,'SATGUESS',' ',0d0,0d0,0d0,ierr,herr)
        goto 500
      endif

      call ISPURE (x,icomp)
      if (.not.lRcnst .or. .not.lRcnstWag) call RMIX (x)
      call CRITP (x,Tc,Pc,Dc,ierr1,herr)
      if (iprop.ne.1) T=Tc
      if (iprop.ne.2) P=Pc
      if (iprop.ne.3) D=Dc
      if (iprop.ne.4 .and. iprop.ne.6) h=0d0
      if (iprop.ne.5) s=0d0
      Dy=0d0
      y(1:nc)=x(1:nc)
      if (T.gt.Tc .and. icomp.ne.0) goto 500

c  Calculate guess from ancillary equations for pure fluids.
      if (icomp.ne.0 .and. iFlg.eq.0) then
        if (iprop.eq.1) then
          if (T.lt.Ttpn(icomp) .and. kph.eq.2) then
            call SUBLT (T,x,P,ierr,herr)         !Sublimation state
            if (ierr.ge.100) call PSATK (icomp,T,P,ierr,herr)        !Vapor pressure
          elseif (iAnc(icomp).ne.0 .and. iphase.eq.1) then
            call PSATK (-icomp,T,P,ierr,herr)    !Liquid pressure for pseudo-pure fluids
          else
            call PSATK (icomp,T,P,ierrpv,herr)   !Vapor pressure
          endif
        elseif (iprop.eq.2) then
          if (P.lt.Ptpn(icomp)) then
            call SUBLP (P,x,T,ierr,herr)         !Sublimation state
            if (ierr.ge.100) call TSATP (P,x,T,ierr,herr)            !Vapor pressure
          elseif (iAnc(icomp).ne.0 .and. iphase.eq.1) then
            call TSATP (-P,x,T,ierr,herr)        !Liquid pressure for pseudo-pure fluids
          else
            call TSATP (P,x,T,ierr,herr)         !Vapor pressure
          endif
        elseif (iprop.eq.3) then
          call TSATD (D,x,T,ierr,herr)
          if (ierr.gt.0) then
            iFlg=13          !Ancillary equation for density not available, goto method 13 below.
            goto 50
          endif
          call PSATK (icomp,T,P,ierr1,herr1)
        elseif (iprop.ge.4 .and. iprop.le.6) then
          if (iprop.eq.4) then
            hes='HLA'
            hs=h
            j=iLetterH
          elseif (iprop.eq.5) then
            hes='SLA'
            hs=s
            j=iLetterS
          else
            hes='ELA'
            hs=h
            j=iLetterE
          endif
          call ROOTFIND(hes,j,0,hs,x,kph,0,1,21,0,0,T,D,hh,ierr,herr)
          goto 500
        endif
        if (iAnc(icomp).eq.1) then
          itprho=1
          goto 100
        endif
        call DLSATK (icomp,T,Dx,ierr1,herr1)
        call DVSATK (icomp,T,Dy,ierr2,herr1)
        if (ierr1.gt.0 .or. ierr2.gt.0) goto 100
        goto 450
      endif
 50   continue



c  Initial guess for sub-critical liquid density based on modified
c   Rackett technique, except that Zra = Zc. See
c   Reid, Prausnitz, and Poling (1987), Properties of Gases and Liquids, 4th edition.
      if (iFlg.eq.11) then
        Rtp=Rxgas*Tc/Pc
        Zc=Pc/(Tc*Dc*Rxgas)
        D=1d0/(Rtp*Zc**(1d0+(ABS(1d0-T/Tc))**(2d0/7d0)))
        call LIMITS ('EOS',x,Tmin,Tmax,Dmax,Pmax)
        if (D.gt.Dmax) D=Dmax
        do i=1,8
          call DERVPVT (T,D,x,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                        dDdP,  dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                        dTdP,  dTdD,d2TdP2,d2TdD2,d2TdPD)
          if (dPdD.lt.0. .or.
     &       (d2PdD2.lt.0. .and. T.lt.Tc) .or.
     &       (d2PdTD.lt.0. .and. ABS(d2PdTD+9999990d0).gt..1d0)) then
            D=1.1d0*D        !Initial guess is in two-phase region, set to higher density.
          else
            EXIT
          endif
        enddo
        if (D.gt.Dmax) D=Dmax
        goto 500
      endif

      if (iFlg.eq.12) then
        acf=accen(icomp)
        if (icomp.eq.0) acf=SUM(x(1:nc)*accen(1:nc))
        P=Pc*10d0**(-2.333333d0*(1d0+acf)*(Tc/T-1d0))

c  For near-critical states, generate initial guesses for density with correlation developed by E.W. Lemmon, NIST.
        theta=(ABS(1d0-T/Tc))**(1d0/3d0)*(1.5d0+acf)
        D=Dc*(1d0+1.113614d0*theta+0.080400d0*theta**2)
        Dy=Dc*(1d0-1.078683d0*theta+5.014057d-2*theta**2)
        if (Dy.lt.0.) Dy=1d-6
        goto 500
      endif


      if (iFlg.eq.13) then
        Dy=0d0
        if (d72l.lt.1d-8) then
          call ERRNUM (332,0,' ',' ',0d0,0d0,0d0,ierr,herr)
          goto 500
        endif
        if (iphase.eq.1) then
          dd=D/Dc+(1.2d0-d72l/Dc)*(D-Dc)/(d72l-Dc)-1d0
          T=9.272d0*XLOG(dd+1d0)-5.195d0*dd
          T=Tc*(1d0-ABS(T)**(1d0/0.337d0))
        elseif (iphase.eq.2) then
          T=Tc
          if (d72v.gt.0.) then
            dx=XLOG(d72v/Dc)
            del=XLOG(D/Dc)
            dd=(3.5d0+dx)*(del/dx)**1.3d0-del
            T=XLOG(dd+1d0)/2d0-0.026d0*SQRT(dd)
            if (T.le.0d0) T=Tc/2d0
          endif
          T=Tc/(1d0+T**(1d0/0.37d0))
        endif
        if (T.le.0.) T=Tc/2d0
        call PRESS (T,D,x,P)
        goto 500
      endif




c  Get estimates from spline curves setup during the call to SATSPLN.
      if (iSpline.ne.0) then
        if (iprop.eq.1) prop=T
        if (iprop.eq.2) prop=P
        if (iprop.eq.4 .or. iprop.eq.6) prop=h
        if (iprop.eq.5) prop=s
        if (iprop.ne.3 .and. iprop.le.10) then
          call SPLNVAL (nc,-2,x,0d0,Dmax,ierr,herr)        !Density upper limit of the spline curves.
          if (iprop.eq.1 .and. DatTminSpln.gt.0.) Dmax=DatTminSpln
          if (iprop.eq.1 .and. DatTmaxSpln.gt.0.) then
            Droot=DatTmaxSpln
            if (iphase.eq.1 .and. DatTminSpln.gt.Droot)
     &          Droot=DatTminSpln*0.9d0
          else
            Droot=Dmax/3d0
            call SPLNROOT (nc+ABS(iprop),1,x,0d0,Droot,ierr1,herr)   !Density value at maximum of prop vs. D [e.g., dT/dD=0].
            if (ierr1.gt.0) Droot=1d0
          endif
          if (iprop.lt.0) then
            D=Droot          !Requested output is for maximum value of prop vs. D
          else
            if (Droot.le.0.) Droot=1d0
            D=Droot/10d0
            if (iphase.eq.1) D=(Droot+Dmax)/2d0
            call SPLNROOT (nc+iprop,0,x,prop,D,ierr,herr)  !Get density at input property.
            if (D.lt.Dc/10 .and. kph.eq.1) ierr=999

c  If spline has failed, move to a much higher density and try again.
            if (ierr.gt.0) then
              call SPLNVAL (nc,-2,x,0d0,Dmax,ierr,herr)    !Maximum value of density in the spline curves.
              D=Dmax*thrd2
              if (DatTminSpln.gt.0.) D=(Dmax+DatTminSpln)/2d0
              if (D.lt.Dmax*thrd2) D=Dmax*thrd2
              call SPLNROOT (nc+ABS(iprop),0,x,prop,D,ierr,herr)
            endif

c  If spline has two roots and no solution in the liquid phase, use the second root (the one with positive curvature) as the solution.
            if (D.lt.Droot .and. iphase.eq.1 .and. ierr.gt.0) then
              D=Dmax
              call SPLNROOT (nc+ABS(iprop),1,x,0d0,D,ierr,herr)      !Density value at minimum of prop vs. D [e.g., dT/d(D)=0].
            endif


c  If spline has failed for the liquid phase because of an increase in temperature in the phase boundary deep in the liquid, move to a density just below the point of zero slope and try again.
            if (ierr.gt.0 .and. iphase.eq.1 .and. Droot.gt.Dc) then
              D=Droot*0.95d0
              call SPLNROOT (nc+iprop,0,x,prop,D,ierr,herr)          !Get density at input property.
              if (D.gt.Dc*1.5d0 .and. ierr.le.0) goto 70   !Exit without root checks.
              ierr=999
            endif
            if (ierr.gt.0) goto 500

c  Check for wrong root.
c  (The 1.1 and 0.9 below are arbitrary and may need to be refined.)
            if (ierr1.eq.0) then
              if (iphase.eq.1 .and. D.lt.Droot) then
                D=Droot*1.1d0
                call SPLNROOT (nc+iprop,0,x,prop,D,ierr,herr)
              elseif (iphase.eq.2 .and. D.gt.Droot) then
                D=Droot*0.9d0
                call SPLNROOT (nc+iprop,0,x,prop,D,ierr,herr)
              endif
            endif
          endif
        endif

        if (iprop.ge.11 .and. iprop.lt.15) then
          call SPLNVAL (nc+iprop-10,1,x,D,Dy,ierr,herr)              !1st derivative
          goto 500
        elseif (iprop.ge.21 .and. iprop.lt.25) then
          call SPLNVAL (nc+iprop-20,2,x,D,Dy,ierr,herr)              !2nd derivative
          goto 500
        endif


 70     if (iprop.ne.1) call SPLNVAL (nc+1,0,x,D,T,ierr,herr)
        if (iprop.ne.2) call SPLNVAL (nc+2,0,x,D,P,ierr,herr)
        if (iprop.ne.4) call SPLNVAL (nc+4,0,x,D,h,ierr,herr)
        if (iprop.ne.5) call SPLNVAL (nc+5,0,x,D,s,ierr,herr)
                        call SPLNVAL (nc+3,0,x,D,Dy,ierr,herr)
        do i=1,nc
          y(i)=0d0
          if (x(i).gt.0) call SPLNVAL (i,0,x,D,y(i),ierr,herr)
        enddo
        sumy=xEq0(SUM(y(1:nc)))
        y(1:nc)=y(1:nc)/sumy
      else
        if (iFlash.gt.0) then
          iTPrho=2
          call SATEST (iFlash,T,P,x,z,y,ierr,herr)
          if (kph.eq.2) y(1:nc)=z(1:nc)
          if (ierr.lt.100) goto 100
        endif
        call ERRNUM (331,0,'SATGUESS',' ',0d0,0d0,0d0,ierr,herr)     !Don't change error number, used elsewhere.
      endif
      goto 500

c  Saturation densities based off propane liquid ancillary equation but scaled to the critical point of the substance.
 100  if ((iprop.eq.1 .and. icomp.ne.0) .or. iAnc(icomp).eq.1 .or.
     &     iTPrho.eq.2) then
        Tr=ABS(1d0-T/Tc)
        Dx=1d0+1.82205d0*Tr**0.345d0+0.65802d0 *Tr**0.74d0+
     &         0.21109d0*Tr**2.6d0  +0.083973d0*Tr**7.2d0
        Dy=-2.4887d0*Tr**0.3785d0-5.1069d0*Tr**1.07d0-
     &      12.174d0*Tr**2.7d0   -30.495d0*Tr**5.5d0-
     &      52.192d0*Tr**10d0    -134.89d0*Tr**20d0
        Dx=Dc*Dx*(1d0+accen(icomp))*0.85d0
        Dy=Dc*EXP1(Dy)
        if (ierrpv.gt.0) then
          P=-6.7722*Tr+1.6938*Tr**1.5d0-1.3341*Tr**2.2d0-
     &       3.1876*Tr**4.8d0+0.94937d0*Tr**6.2
          P=Pc*EXP4(Tc/T*P)
        endif
      endif
      if (iTPrho.ge.1) then
        if (iphase.eq.1 .or. iTPrho.eq.2) then
          call TPRHOB (T,P,Dx*1.5d0,Dx*0.8d0,x,Dxx,ierr,herr)
          if (ierr.ge.100)call TPRHOB(T,P,Dx*2.5,Dx*0.2,x,Dxx,ierr,herr)
          if (ierr.lt.100)Dx=Dxx
        endif
        if (iphase.eq.2 .or. iTPrho.eq.2) then
          call TPRHOB (T,P,Dy*2.,Dy/100.,x,Dyy,ierr,herr)
          if (ierr.ge.100)call TPRHOB(T,P,Dy*3.,Dy/10.,x,Dyy,ierr,herr)        !These values probably need to be changed.
          if (ierr.lt.100)Dy=Dyy
        endif
        ierr=0
      endif

 450  if (Dx.gt.100.) Dx=Dc*2d0
      if (Dy.gt.100.) Dy=Dc/2d0
      if (iprop.eq.3) then
        if (D.lt.Dc) Dy=Dx
      else
        if (iphase.eq.2) call SWAPV (Dx,Dy)
        D=Dx
      endif
      if (iAnc(icomp).eq.1) Dy=D

 500  if (iprop.ne.1 .and. (T.lt.1. .or. T.gt.2000.))  T=300d0
      if (iprop.ne.2 .and. (P.lt.1d-12 .or. P.gt.1d6)) P=0d0
      if (iprop.ne.3 .and. (D.lt.0. .or. D.gt.100.))   D=0.1d0
      if (Dy.lt.0. .or. Dy.gt.100.) Dy=0.1d0
      if (nc.gt.1) then
        do i=1,nc
          if ((y(i).lt.1.d-50 .and. y(i).ne.0d0) .or. y(i).gt.1.) then         !Includes negative values of y(i)
            if (y(i).lt.0) ierr=999
            y(1:nc)=ABS(y(1:nc))/SUM(ABS(y(1:nc)))
            EXIT
          endif
        enddo
      endif
      iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      RETURN

c  Find a pressure that can be used to indicate a single-phase state.  In nearly
c  all cases the value could be extremely low based on the conditions.
c     Psat=0d0
c     tcex=1d0
c     pcex=1d0
c     if (icomp.eq.0) then
c       tcex=1.5d0           !Extra amount to add to Tc for mixtures
c       pcex=1.5d0
c     endif
c     if (ierrCrt.eq.0 .and. imaxp.eq.1) pcex=100d0        !Possible Type III mixture
c     if (T.le.Tc) Psat=EXP4(13.85d0*(T/Tc)-14.073d0)*Pc
c     if (T.le.Tc*0.7d0) Psat=Psat/10d0          !Some fluids have lower pressures than this equation below 70% of Tc.
c     if (T.le.Tc*0.6d0) Psat=Psat/100d0
c     if (T.le.Tc*0.5d0) Psat=Psat/100d0
c     if (T.le.Tc*0.4d0) Psat=Psat/100d0
c     if (icomp.ne.0 .and. T.le.Tc) then         !Get Psat from ancillaries if they exist.
c       ierr=999
c       if (hpsk(icomp).ne.'NBS') call PSATT (T,z,psat2,ierr,herr1)
c       if (ierr.eq.0) Psat=0.95d0*psat2         !drop by 5% to allow for error
c     else
c       Psat=Psat/10d0                           !Drop cutoff for single phase vapor check by factor of 10.
c       if (nc.gt.1) Psat=Psat/100d0
c     endif

      end                                            subroutine SATGUESS
c
c ======================================================================
c
      subroutine SATEST (iFlash,T,P,z,x,y,ierr,herr)
c
c  Estimate temperature, pressure, and compositions to be used
c  as initial guesses to SATTP.
c
c  Inputs:
c   iFlash--Phase flag:  0 - Flash calculation (T and P known)
c                        1 - T and x known, P and y returned
c                        2 - T and y known, P and x returned
c                        3 - P and x known, T and y returned
c                        4 - P and y known, T and x returned
c                            If this value is negative, the retrograde point will be returned.
c        T--Temperature [K] (input or output)
c        P--Pressure [kPa] (input or output)
c        z--Composition (array of mole fractions)
c           The composition for the known x or y array should be sent in this z array,
c           not in the output arrays shown below.
c
c  Outputs:
c        T--Temperature [K] (input or output)
c        P--Pressure [kPa] (input or output)
c        x--Liquid phase composition (array of mole fractions)
c        y--Vapor phase composition (array of mole fractions)
c     ierr--Error flag:  0 - Successful
c                      999 - Unsuccessful
c     herr--Error string (character*255)
c
c  written by Diego Ortiz, NIST Thermophysics Division, Boulder, Colorado
c  07-01-10 DOO, original version
c  06-04-15 EWL, optimize and shorten code

      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax),dw(ncmax),v(20),f(20)
      character herr*255
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SATEST

      ierr=0
      herr=' '
      alpha=1d0
      tol=0.00000001d0
      y(1:nc)=z(1:nc)
      x(1:nc)=z(1:nc)
      if (iFlash.lt.1 .or. iFlash.gt.4) ierr=999
      if (ierr.eq.999) RETURN
      if (iFlash.ge.3 .and. T.le.0.) T=300
      v3=0d0
      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)      !Initialize root finding variables.


      if (iFlash.eq.3.or.iFlash.eq.4) then
        if (P.le.1d-20.or.P.gt.1d6) goto 500
      endif
      if ((T.gt.-2000d0.and.T.le.0) .or. iFlash.eq.3.or.iFlash.eq.4)then       !Negative T used later.
        call PTEST (-3,1d0,(P),alpha,z,P,dw,m,herr)
        T=SUM(z(1:nc)*dw(1:nc))
        if (T.lt.1d0 .or. T.gt.2000d0) goto 500
      else
        if (T.lt.1d0 .or. T.gt.2000d0) goto 500
      endif

c  Do not remove parenthesis around P on the calls to PTEST.  It needs to send the value, not the memory location.
      if (iFlash.eq.1) then
        call PTEST (1,T,(P),alpha,z,P,y,m,herr)
        call PTEST (5,T,(P),alpha,z,P,y,m,herr)

      elseif (iFlash.eq.2) then
        call PTEST (2,T,(P),alpha,z,P,x,m,herr)
        call PTEST (6,T,(P),alpha,z,P,x,m,herr)

c  Make initial guesses to bound root.
      elseif (iFlash.eq.3 .or. iFlash.eq.4) then
        jr=1
        v(2)=T
        do it=1,100
          T=v(2)
          call PTEST (iFlash,T,P,alpha,z,f(2),y,m,herr)
          if (ABS(f(2)).gt.1d20) EXIT
          if (ABS(f(2)).lt.tol) EXIT
          call ROOTSEARCH (jr,0,0,v,f,v3,it,ib,lneg,lpos)            !Get next guess, except for first iteration, which is given on the line below.
          if (jr.eq.1) v3=T*0.97d0
          v(2)=v3
          jr=1234
        enddo
        if (it.gt.100) ierr=999

c  Get equilibrium compositions after iteration converged.
        if (iFlash.eq.3) then
          call PTEST (5,T,P,alpha,z,fn,y,m,herr)
        elseif (iFlash.eq.4) then
          call PTEST (6,T,P,alpha,z,fn,x,m,herr)
        endif
      endif

 500  if ((T.gt.2000d0 .or. T.lt.1d0) .and. iFlash.ge.3) then
        T=300d0
        ierr=999
      elseif ((P.lt.1d-20 .or. P.gt.1d6) .and. iFlash.le.2) then
        P=1d0
        ierr=999
      elseif (ABS(f(2)).gt.1d20) then
        ierr=999
      endif
      do i=1,nc
        if ((y(i).lt.1.d-50 .and. y(i).ne.0d0) .or. y(i).gt.1.) then           !Includes negative values of y(i)
          if (y(i).lt.0) ierr=999
          y(1:nc)=ABS(y(1:nc))/SUM(ABS(y(1:nc)))
          EXIT
        endif
      enddo
      do i=1,nc
        if ((x(i).lt.1.d-50 .and. x(i).ne.0d0) .or. x(i).gt.1.) then           !Includes negative values of x(i)
          if (x(i).lt.0) ierr=999
          x(1:nc)=ABS(x(1:nc))/SUM(ABS(x(1:nc)))
          EXIT
        endif
      enddo

      end                                              subroutine SATEST
c
c ======================================================================
c
      subroutine SAT0EST (T,P,z,x,y,ierr,herr)
c
c  Estimate x and y for 2-phase flash calculations.
c
c  Inputs:
c        T--Temperature [K] (input or output)
c        P--Pressure [kPa] (input or output)
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K] (input or output)
c        P--Pressure [kPa] (input or output)
c        x--Liquid phase composition (array of mole fractions)
c        y--Vapor phase composition (array of mole fractions)
c     ierr--Error flag:  0 - Successful
c                      999 - Unsuccessful
c                      321 - No solution
c     herr--Error string (character*255)
c
c  written by Diego Ortiz, NIST Thermophysics Division, Boulder, Colorado
c  07-01-10 DOO, original version
c  06-04-15 EWL, optimize and shorten code

      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax),w(ncmax)
      character herr*255
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SAT0EST

      ierr=0
      herr=' '
      tol=0.00000001d0

c  Check if (T,P) is in the two-phase region.
      Tb=300d0
      Td=300d0
      call SATEST (1,T,Pb,z,x,y,ierr,herr)
      call SATEST (2,T,Pd,z,x,y,ierr,herr)
      call SATEST (3,Tb,P,z,x,y,ierr,herr)
      call SATEST (4,Td,P,z,x,y,ierr,herr)
      y(1:nc)=z(1:nc)
      x(1:nc)=z(1:nc)
      if ((T-Td)*(T-Tb).gt.0. .or. (P-Pb)*(P-Pd).gt.0.) then
        call ERRNUM (321,0,'SAT0EST',' ',0d0,0d0,0d0,ierr,herr)
        goto 500
      endif

      fn1=0d0
      alpha1=0d0
      alpha2=0.999999999d0
      alpha=0.5d0
      call PTEST (4,T,P,alpha ,z,fn ,y,ierr,herr)
      call PTEST (4,T,P,alpha2,z,fn2,y,ierr,herr)

c  PTEST routine with iFlash=4 returns fn=0 at alpha=0 and has a maximum (or minimum).
c  Use bisection to get past maximum so that Newton's method will converge correctly.
      do i=1,5
        if (fn*fn2.gt.0.) then
          alpha2=alpha
          fn2=fn
        else
          alpha1=alpha
        endif
        alpha=(alpha1+alpha2)/2d0
        call PTEST(4,T,P,alpha,z,fn,y,ierr,herr)
      enddo

c  Use Newton's method to quickly find solution.
      do j=1,100
        aold=alpha
        fnold=fn

c  Calculate derivative.
        fnprime=0d0
        call PTEST(-1,T,P,alpha,z,fn,w,ierr,herr)
        fnprime=SUM(z(1:nc)/(1d0-alpha+alpha*w(1:nc)/P)**2*
     &          (w(1:nc)/P-1d0))

c  Use slope to get next guess.
        if (lNE0(fnprime)) alpha=aold-(fn/fnprime)
        if (alpha.gt.1.) alpha=(1d0+aold)/2d0
        call PTEST(4,T,P,alpha,z,fn,y,ierr,herr)
        deva=(ABS(aold-alpha)/aold)*100
        if (ABS(fn).lt.tol .and. deva.lt.1) EXIT
      enddo
      if (j.ge.100) ierr=999

c  Get liquid and vapor compositions.
      call PTEST (6,T,P,alpha,z,fn,x,ierr,herr)
      y(1:nc)=(z(1:nc)-x(1:nc)*(1d0-alpha))/alpha

 500  do i=1,nc
        if (z(i).gt.0) then
          if ((y(i).lt.1.d-50 .and. y(i).ne.0d0) .or. y(i).gt.1.) then         !Includes negative values of y(i)
            if (y(i).lt.0.) ierr=999
            y(1:nc)=ABS(y(1:nc))/SUM(ABS(y(1:nc)))
            EXIT
          endif
        endif
      enddo
      do i=1,nc
        if (z(i).gt.0) then
          if ((x(i).lt.1.d-50 .and. x(i).ne.0d0) .or. x(i).gt.1.) then
            if (x(i).lt.0.) ierr=999
            x(1:nc)=ABS(x(1:nc))/SUM(ABS(x(1:nc)))
            EXIT
          endif
        endif
      enddo
      end                                             subroutine SAT0EST
c
c ======================================================================
c
      subroutine PTEST (inp,T,P,alpha,z,calc,zout,ierr,herr)
c
c  Use the Wilson K-factor approximation to obtain P, x(i), or y(i).
c  Internal routine for the SATEST or SAT0EST routines.
c
c  Inputs:
c      inp--See comments in code below
c        T--Temperature [K]
c        P--Pressure [kPa]
c    alpha--vapor liquid fraction
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     calc--See comments below
c     zout--x(i) or y(i) depending on inp
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  written by Diego Ortiz, NIST Thermophysics Division, Boulder, Colorado
c  07-01-10 DOO, original version

      include 'COMMONS.INC'
      dimension z(ncmax),zout(ncmax),P2(ncmax),ac(ncmax)
      character herr*255


      ierr=0
      herr=' '
      if (T.le.0.) RETURN

      sum0=0
      ac(1:nc)=5.373d0*(1d0+accen(1:nc))
      P2(1:nc)=Pcrit(1:nc)*EXP(ac(1:nc)*(1d0-Tcrit(1:nc)/T))         !=K(i)*P  --  See Eq. 7.130 of GERG-2004
      do i=1,nc
        if (P2(i).le.1.d-50) P2(i)=Pcrit(i)
      enddo
      if (inp.eq.-1) then
        zout(1:nc)=P2(1:nc)                      !Wilson factor
      elseif (inp.eq.-2) then
        zout(1:nc)=ac(1:nc)*Tcrit(1:nc)/T**2     !Part of derivative of Wilson factor with respect to T.
      elseif (inp.eq.1) then
        calc=SUM(z(1:nc)*P2(1:nc))               !=P*SUM(y(i))   [y=vapor, x=liquid]
      elseif (inp.eq.2) then
        do i=1,nc
          if (lNE0(P2(i))) sum0=sum0+z(i)/P2(i)  !=P/SUM(x(i))
        enddo
        calc=XDIV(1d0,sum0)
      elseif (inp.eq.-3 .and. P.gt.0.) then
        if (P.le.0d0) RETURN
        zout(1:nc)=Tcrit(1:nc)/(1d0-LOG(P/Pcrit(1:nc))/ac(1:nc))     !Inverse of Wilson factor to get initial T
      elseif (inp.eq.3) then
        if (P.le.0d0) RETURN
c       calc=1d0-P/SUM(z(1:nc)*P2(1:nc))         !=1-1/SUM(y(i))
        calc=1d0-SUM(z(1:nc)*P2(1:nc))/P         !=1-SUM(y(i))
      elseif (inp.eq.4) then
        if (P.le.0d0) RETURN
        Pinv=1d0/P
        do i=1,nc
          if (z(i).gt.0) then
            divs=1d0-alpha+alpha*P2(i)*Pinv
            if (divs.gt.0.) sum0=sum0+z(i)/divs
          endif
        enddo
        calc=1d0-sum0                            !=1-SUM(x(i)) when alpha=1
      elseif (inp.eq.5) then
        if (P.le.0d0) RETURN
        zout(1:nc)=z(1:nc)/P*P2(1:nc)                   !=y(i)
      elseif (inp.eq.6) then
        if (P.le.0d0) RETURN
        do i=1,nc
          h=0d0
                   !Temporary Henry's constant model
          if (ixWat.ne.0) then
            if (z(ixWat).gt.0 .and. z(i).gt.0) then
              ib=0
              if (INDEX(hnam80(i),'benzene').ne.0) ib=1    !Check if the name "benzene" is in the fullname, such as "methylbenzene"
              if (INDEX(hsyn(i),'benzene').ne.0) ib=1
              dip=dipole(i)
              if (dip.ne.-1.) then
                h=4.5d0-2.5d0*dip+0.01d0*wmas(i)
                if (ib.eq.1) h=h-2.6d0+2.3d0*dip
                if (iFluidID(i).eq.1 .or. iFluidID(i).eq.3) h=2d0    !Check if the mixture contains water or heavy water.
              endif
            endif
          endif
          zout(i)=XDIV(z(i),1d0-alpha+alpha*P2(i)/P)/10d0**h      !=x(i)
c         if (z(i).le.1.d-3 .and. z(i).gt.0.)
c    &        zout(i)=zout(i)*10**((LOG10(z(i))+3d0)*2d0)
        enddo
      endif
      if (inp.eq.5 .or. inp.eq.6) then
        s=SUM(zout(1:nc))
        if (ABS(s).gt.0) zout(1:nc)=zout(1:nc)/s
        do i=1,nc
          if (zout(i).lt.0.000001 .and. z(i).gt.0.) zout(i)=0.000001d0
          if (zout(i).gt.0.999999)                  zout(i)=0.999999d0
        enddo
        s=SUM(zout(1:nc))
        if (ABS(s).gt.0) zout(1:nc)=zout(1:nc)/s
      endif

      end                                               subroutine PTEST
c
c ======================================================================
c
      subroutine SATMAX (icomp,z,Tcmax,Pcmax,Dcmax,ierr,herr)
c
c  Return the critical point for a pure fluid, or the VLE limits
c  for a mixture.  When splines are not known, Tcmax and Pcmax
c  are multiplied by 1.5 to account for VLE states beyond the simple
c  methods used to calculate the mixture critical point.
c
c  The values returned are not those at a single state point, but are
c  the T and P maximums possible along the saturation line.
c
c  Inputs:
c    icomp--Value from calling ISPURE; set to 1 for a pure fluid
c        z--Composition (array of mole fractions)
c
c  Outputs:
c    Tcmax--Maximum temperature (i.e., T at the maxcondentherm) [K]
c    Pcmax--Maximum pressure (i.e., P at the maxcondenbar) [kPa]
c    Dcmax--Density at the cricondentherm (i.e., the value at Tcmax, but not Pcmax)
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)

      ierr=0
      call MAXP (z,T,PmaxP,D,imaxp,herr)                   !Get maxcondenbar pressure (maximum pressure possible for all saturation states).
      call MAXT (z,Tcmax,Pcmax,Dcmax,imaxt,herr)           !Get maxcondentherm values (maximum temperature possible for all saturation states).
      if (imaxt.gt.0) call CRITP (z,Tcmax,Pcmax,Dcmax,ierr1,herr)    !If no splines, get critical point.
      if (imaxp.le.0 .and. PmaxP.gt.0.) Pcmax=PmaxP        !Replace Pcmax returned from MAXT with that at MAXP.
      if (ierrCrt.eq.0 .and. imaxp.eq.1) Pcmax=Pcmax*100d0           !Possible Type III mixture
      if (iSpline.eq.0 .and. icomp.eq.0) then
        Tcmax=Tcmax*1.5d0    !Increase Tc and Pc for mixtures when splines are not known.
        Pcmax=Pcmax*1.5d0
      endif
      if (Tcmax.eq.300d0 .and. (Pcmax.le.0d0 .or. Dcmax.le.0d0))
     &    call REDLINEAR (1,z,Tcmax,Pcmax,Dcmax,ierr,herr)

      end                                              subroutine SATMAX
c
c ======================================================================
c
      subroutine SATSAVE(iFlag,icomp,kph,T,P,D,Dl,Dv,q,x,y,z,ierr,iexit)
c
c  Internal routine to the saturation routines to either save a successful
c  calculation or restore it if the routines are called again with the
c  same inputs.

      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax)


      iexit=0
      if (.not.lCache2) RETURN
      if (iAnc(icomp).ne.0) RETURN
      if (kph.lt.1 .or. kph.gt.4) RETURN
      if (ierr.gt.0) then
        lSatTP(kph)=.false.
        RETURN
      endif

!$omp single
      if (iFlag.eq.2) then             !Save saturation calculation for a given input of T or P.
        Tsav(kph)=T
        Psav(kph)=P
        icsav(kph)=icomp
        Dlsav(kph)=Dl
        Dvsav(kph)=Dv
        xsav(1:nc,kph)=x(1:nc)
        ysav(1:nc,kph)=y(1:nc)
        zsav(1:nc,kph)=z(1:nc)
        lSatTP(kph)=.true.

      else                             !Get saved calculation.
        if (.not.lSatTP(kph)) RETURN
        if (icomp.ne.icsav(kph)) RETURN
        if (kph.le.2 .and. ABS(T-Tsav(kph)).gt.1d-9) RETURN
        if (kph.ge.3 .and. ABS(P-Psav(kph)).gt.1d-9) RETURN
        if (icomp.eq.0) then
          do i=1,nc
            if (ABS(z(i)-zsav(i,kph)).gt.1d-9) RETURN
          enddo
        endif
        if (kph.le.2) P=Psav(kph)
        if (kph.ge.3) T=Tsav(kph)
        q=0d0
        if (kph.eq.2 .or. kph.eq.4) q=1d0
        Dl=Dlsav(kph)
        Dv=Dvsav(kph)
        D=Dl
        if (q.eq.1d0) D=Dv
        x(1:nc)=xsav(1:nc,kph)
        y(1:nc)=ysav(1:nc,kph)
        iexit=1
      endif

!$omp end single nowait
      end                                             subroutine SATSAVE
c
c ======================================================================
c
      subroutine FNCRPT (z,D,P,T,ierr,herr)
c
c  Find the critical point of a pure fluid based on the EOS.
c  This is only for pure fluids!
c
c  Inputs:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        D--Density [mol/L]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-01-96 EWL, original version taken from code in SATT

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::FNCRPT


      ierr=0
      call CRITP (z,T,pc2,D,ierr1,herr)
      iter=0
      ac=1d-10
c     ac=1d-8
c  Get first point.
      told=T+.1d0
      D=CRTDEN(z,told,D)
      if (D.le.0) goto 30
      call DPDDX (told,D,z,fold)
      fold=fold/1000d0
 20   D=crtden(z,T,D)
      if (D.le.0) goto 30
      call DPDDX (T,D,z,f)
      f=f/1000d0
      iter=iter+1
      if (iter.gt.20) goto 30
      if (ABS(f).lt.ac) then
        call PRESS (T,D,z,P)
        RETURN
      endif
c  Get next point with the slope of the previous two points.
      if (fold.eq.f) goto 30
      T1=T-f*(told-T)/(fold-f)
      told=T
      fold=f
      T=T1
      if (T.gt.0) goto 20
 30   end                                              subroutine FNCRPT
      function CRTDEN (z,T,D)
      include 'COMMONS.INC'
      dimension z(ncmax)
      iter=0
 10   iter=iter+1
      call DPDD2 (T,D,z,df0)
      crtden=D
      if(ABS(df0).lt.1d-12) RETURN
      if (iter.gt.30) RETURN
      call DPDD2 (T,D+0.0001d0,z,df1)
      call DPDD2 (T,D-0.0001d0,z,df2)
      fp=(df1-df2)/0.0002d0
      fpp=(df1+df2-2d0*df0)/0.00000001d0
      Dinc=XDIV(-fp,df0)+XDIV(fpp/2d0,fp)
      D=D+XDIV(1d0,Dinc)
      if (D.le.0) RETURN
      goto 10

      end                                                function CRTDEN
c
c ======================================================================
c
      subroutine CRTPNT (z,Tc,Pc,Dc,ierr,herr)
c
c  Subroutine for the determination of the true critical point of a
c  mixture with the use of the method of Michelsen (1984).
c
c  The routine requires good initial guess values of Pc and Tc.
c
c  On convergence, the values of bb and cc should be close to zero
c  and dd > 0 for a two-phase critical point.
c  bb=0, cc=0, and dd <= 0 for an unstable critical point.
c
c  Inputs:
c        z--Composition [array of mole fractions]
c       Tc--Initial guess for the critical temperature [K]
c       Pc--Initial guess for the critical pressure [kPa]
c
c  Outputs:
c       Tc--Critical temperature [K]
c       Pc--Critical pressure [kPa]
c       Dc--Critical density [mol/l]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  written by G. Venkatarathnam, Indian Institute of Technology Madras
c  10-15-11  GV, original version
c  01-07-13 EWL, adapt code for use in Refprop

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension z(ncmax),y(ncmax),yn(ncmax),v(ncmax),g(ncmax),u(ncmax)
      dimension a(ncmax,ncmax),dfds(2),dpht(ncmax),dphp(ncmax)
      dimension phy(ncmax),phz(ncmax),dft2(2),dfp2(2)
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CRTPNT


      ierr=0
      herr=' '
      eps=1d-6               !Numerical perturbation of z
      bb=0
      cc=0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        call CRITP (z,Tc,Pc,Dc,ierr1,herr)        !Exit if a pure fluid.
        RETURN
      endif
      if (Tc.le.0. .or. tc.gt.5000d0) call CRITP (z,Tc,Pc,Dc,ierr1,herr)

 10   v(1:nc)=SQRT(z(1:nc))
      Pc=ABS(Pc)
      Tc=ABS(Tc)
      Dc=ABS(Dc)
      if (Tc.lt.0.) Tc=300d0
      tol=1d-8
      iz=1
      do k=1,20
        call TOLRNC (tolr,k,10,12,15,18,0)
        call LGPHI (2,iz,Tc,Dc,Pc,z,phz,a1,a2,a3,ierr,herr)
        if (ierr.gt.0) RETURN
        if (k.gt.1 .and. ABS(bb)+ABS(cc/100d0).lt.tol) RETURN        !Convergence obtained, return.

        do i=1,nc
          a(i,1:nc)=v(i)*v(1:nc)*dphidnj(i,1:nc)
          a(1:nc,i)=a(i,1:nc)
          a(i,i)=a(i,i)+1d0
        enddo
        xlamda=0d0
        call MINEIGEN (nc,a,u,xlamda,ierr,herr)

        if (k.eq.10) tol=tol*100d0
        dpht(1:nc)=dphidt(1:nc)
        dphp(1:nc)=dphidp(1:nc)
        do j=1,2
          Dy=Dc
          s=eps
          if (j.eq.1) s=-s
          y(1:nc)=ABS(s*u(1:nc)*v(1:nc)+z(1:nc))
          sumy=SUM(y(1:nc))
          if (ABS(sumy).gt.1d-10) yn(1:nc)=y(1:nc)/sumy
          call LGPHI (2,iz,Tc,Dy,Pc,yn,phy,a1,a2,a3,ierr,herr)
          do i=1,nc
            if (z(i).gt.0.) g(i)=XLOG(y(i)/z(i))+phy(i)-phz(i)
          enddo
          dfds(j)=SUM(v(1:nc)*u(1:nc)*g(1:nc))
          dft2(j)=SUM(v(1:nc)*u(1:nc)*(dpht(1:nc)-dphidt(1:nc)))
          dfp2(j)=SUM(v(1:nc)*u(1:nc)*(dphp(1:nc)-dphidp(1:nc)))
        enddo

        bb=xlamda/2d0
        bp=(dfp2(2)-dfp2(1))/eps/4d0
        bt=(dft2(2)-dft2(1))/eps/4d0
        cc=(dfds(2)+dfds(1))/eps**2/6d0
        cp=(dfp2(2)+dfp2(1))/eps**2/6d0
        ct=(dft2(2)+dft2(1))/eps**2/6d0

        denom=(bp*ct-bt*cp)
        if (ABS(denom).lt.1d-20) goto 151
        dp=(cc*bt-bb*ct)/denom
        dt=(cp*bb-cc*bp)/denom
        do i=1,25
          if (ABS(dt).lt.2. .and. ABS(dp).lt.1000.) EXIT
          xmt=10d0
          if (tc.lt.100) xmt=1d0
          dt=dt/Tc*xmt
          dp=dp/Pc*xmt
        enddo
        Tc=Tc-dt
        Pc=Pc-dp
        if (k.ge.20 .or. Pc.lt.0d0 .or. Tc.lt.0d0 .or.
     &      Pc.gt.100d3 .or. Tc.gt.1d3) then
          if (eps.lt.0.099d0) then
            call CRITP (z,Tc,Pc,Dc,ierr1,herr)
            eps=eps*10d0
            goto 10
          endif
          goto 151
        endif
      enddo
      RETURN

 151  call ERRNUM (151,0,'CRTPNT',' ',0d0,0d0,0d0,ierr,herr)
      end                                              subroutine CRTPNT
c
c ======================================================================
c
      subroutine REDLINEAR (iFlag,z,Tred,Pred,Dred,ierr,herr)
c
c  Calculate simple reducing functions for either the critical point (iFlag=1)
c  or the triple point (iFlag=2).  For a pure fluid, these are just
c  the EOS values.  For a mixture, they are the mole fraction averages
c  of the temperature, pressure, and density values at either the
c  critical point or triple point.
c
c  Inputs:
c    iFlag--Set to 1 to calculate a pseudo-critical point with density based on a molar density sum.
c           Set to 2 to calculate a pseudo-critical point with density based on a mass density sum.
c           Set to 3 to calculate a pseudo-critical point with density based on a molar volume sum.
c           Set to 4 to calculate a pseudo-critical point with density based on a mass volume sum.
c           Set to -1 to calculate a pseudo-triple point.
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     Tred--Temperature reducing value [K]
c     Pred--Pressure reducing value [kPa]
c     Dred--Density reducing value [mol/L]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-03-17 EWL, original version

      include 'COMMONS.INC'
      include 'COMANC.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::REDLINEAR

      ierr=0
      herr=' '
      Tred=300d0
      Dred=1d0
      Pred=1d3
      if (nc.le.0) RETURN
      call ISPURE (z,icomp)
      if (icomp.ne.0) then             !Pure fluids.
        if (iFlag.ge.0) then
          Tred=Tcrit(icomp)            !Critical point
          Dred=Dcrit(icomp)
          Pred=Pcrit(icomp)
        else
          Tred=Ttpn(icomp)             !Triple point
          Dred=Dtpn(icomp)
          Pred=Ptpn(icomp)
        endif
      else
        if (iFlag.ge.0) then
          Tred=SUM(z(1:nc)*Tcrit(1:nc))          !Use simple mixing rules for critical point.
          Pred=SUM(z(1:nc)*Pcrit(1:nc))
          Dred=SUM(z(1:nc)*Dcrit(1:nc))          !The transport properties are based on this one, don't change.
          if (iFlag.eq.2) then
            w=SUM(z(1:nc)*wmas(1:nc))
            Dred=SUM(z(1:nc)*Dcrit(1:nc)*wmas(1:nc))/w               !Mass density based critical density.
          elseif (iFlag.eq.3) then
            Dsum=0
            do i=1,nc
              if (z(i).gt.0) Dsum=Dsum+XDIV(z(i),Dcrit(i))           !Molar volume based critical density.
            enddo
            Dred=XDIV(1d0,Dsum)
          elseif (iFlag.eq.4) then
            w=SUM(z(1:nc)*wmas(1:nc))
            Dsum=0
            do i=1,nc
              if (z(i).gt.0) Dsum=Dsum+XDIV(z(i),wmas(i)*Dcrit(i))             !Mass volume based critical density.
            enddo
            Dred=XDIV(1d0,Dsum*w)
          endif
        else
          Tred=SUM(z(1:nc)*Ttpn(1:nc))           !Mixture "triple" point - use simple mixing rules.
          Dred=SUM(z(1:nc)*Dtpn(1:nc))
          Pred=0d0
          do i=1,nc
            Pred=Pred+z(i)*XLOG(Ptpn(i))
          enddo
          Pred=EXP4(Pred)
          if (Tred.le.0.) ierr=999
        endif
      endif
      if (lEQ0(Tred)) Tred=300d0
      if (lEQ0(Dred)) Dred=1d0
      if (lEQ0(Pred)) Pred=1d3
      end                                           subroutine REDLINEAR

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                   End file SAT_SUB.FOR
c ======================================================================
