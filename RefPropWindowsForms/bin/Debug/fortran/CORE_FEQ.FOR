c  Begin file CORE_FEQ.FOR
c
c  This file contains the routines implementing the Helmholtz form of
c    the pure fluid equation of state (the so-called Fundamental Equation).
c    This includes the ideal-gas and real-gas parts of the Helmholtz energy
c    (with the ideal-gas part often based on the ideal-gas heat capacity).
c
c  The code to read the BWR parameters is included here since the parameters
c    are converted to the FEQ form after being read in.
c
c  This file also contains routines implementing an extended corresponding
c    states model with temperature- and density-dependent shape factors.
c
c  Contained here are:
c
c     function PHIFEQ (icomp,itau,idel,tau,del)
c     function PHIECS (icomp,itau,idel,tau,del)
c     function PHI0   (itau,idel,T,D,z)
c     function CP0    (T,z)
c
c  The following are internal routines and should not be called outside this file:
c     function CP0K   (icomp,T)
c     function PHI0K  (icomp,itau,idel,T,D)
c     function PH0CPP (icomp,itau,idel,T,D)
c     function PH0PH0 (icomp,itau,idel,T,D)
c     subroutine FJ   (icomp,T,D,f,dfdt,d2fdt2,dfdd,d2fdd2,d2fdtd)
c     subroutine HJ   (icomp,T,D,h,dhdt,d2hdt2,dhdd,d2hdd2,d2hdtd)
c     subroutine CP0CPX (icomp,T,CpP,CpI,CpT,Cp3)
c     subroutine SETFEQ (nread,icomp,ierr,herr)
c     subroutine SETBWR (nread,icomp,ierr,herr)
c     subroutine SETCPP (nread,icomp,iFlg,ierr,herr)
c     subroutine SETECS (nread,icomp,hreff,heqn,ierr,herr)
c     block data BDCPP
c
c  Functions CPI and CPT were removed on 6-27-16 as they were not
c    being used and several others were combined to shorten the code
c    and increase the calculation speed.
c
c ======================================================================
c ======================================================================
c
      function PHIFEQ (icomp,itau,idel,tau,del)
c
c  Compute pure-fluid reduced Helmholtz energy or a derivative as functions
c  of the dimensionless temperature and density for the Helmholtz-explicit
c  equation of state.
c
c  The Helmholtz energy consists of ideal and residual (real-gas) terms; this
c  routine calculates only the residual part.
c
c  The reducing parameters are often, but not necessarily, equal to the
c  critical temperature and density.
c
c  Based on Lemmon, E.W., McLinden, M.O., and Wagner, W.
c  Thermodynamic Properties of Propane.  III.  A Reference Equation of State for
c  Temperatures from the Melting Line to 650 K and Pressures up to 1000 MPa
c  J. Chem. Eng. Data, 54:3141-3180, 2009.
c
c  Inputs:
c    icomp--Pointer specifying component (1..nc)
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           When itau = 0 and idel = 0, compute A/RT
c           When itau = 0 and idel = 1, compute 1st density derivative
c           When itau = 1 and idel = 1, compute cross derivative
c           etc.
c      tau--Dimensionless temperature (To/T)
c      del--Dimensionless density (D/Do)
c
c  Output (as the function value):
c      phi--Residual (real-gas) part of the Helmholtz energy, or one of its
c           derivatives (as specified by itau and idel) in reduced form (A/RT)
c           itau  idel   Output (dimensionless for all cases)
c             0    0      A/RT
c             1    0      tau*[  d(A/RT)/d(tau)]
c             2    0    tau^2*[d^2(A/RT)/d(tau)^2]
c             0    1      del*[  d(A/RT)/d(del)]
c             0    2    del^2*[d^2(A/RT)/d(del)^2]
c             1    1  tau*del*[d^2(A/RT)/d(tau)d(del)]
c                         etc.
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  07-26-95  MM, Original version
c  06-22-16 EWL, Shorten code, remove all comments about changes no longer necessary after rewrite of code throughout this entire file..
c  08-01-17 EWL, Add new terms developed by Kehui Gao for ammonia.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension dd(0:20),phir03(mxtrm),phir30(mxtrm)

      n=icomp
      np=nFEQpe(n)
      ng=nFEQgaus(n)
      nn=nFEQnan(n)
      nk=nFEQkg(n)
      npx=np            !End of polynomials.
      ngx=npx+ng        !End of Gaussian terms
      nnx=ngx+nn        !End of non-analytical terms.
      nkx=nnx+nk        !End of new terms.
      nps=1             !Starting point.
      ngs=npx+1         !Start of Gaussian terms.
      nns=ngx+1         !Start of non-analytical terms.
      nks=nnx+1         !Start of new terms.
      nt=nkx            !Total number of terms.
      phisum=0d0
      PHIFEQ=0d0
      if (del.le.1d-20 .or. del.gt.1d10 .or. tau.lt.1d-10) RETURN !Trivial solution at zero density for any and all derivatives.
      PHIFEQ=xnotc      !Error message for not calculated.

c  Check if this routine has already been called with the same del and tau.
c  If it has, check to see if the requested derivative has already been calculated,
c   and send it back if so (without doing further calculations).
      ncode=idel*4+itau+1
      lt=ABS(tau-tausvf(n)).lt.1d-12   !Check for change in tau.
      ld=ABS(del-delsvf(n)).lt.1d-16   !Check for change in del.
      if (lt.and.ld) then
        if (lFEQsav(ncode,n)) then     !Retrieve value from previous call.
          PHIFEQ=FEQsav(ncode,n)
          RETURN
        endif
      else
        lFEQsav(1:16,n)=.false.        !Reset flags that indicate which derivatives have been calculated.

c  ----- Residual Helmholtz energy dissected -----

      !-----Temperature dependent pieces
        if (.not.lt) then              !Check if either del or tau was sent in the previous call.  If different, calculate common pieces needed elsewhere.
          if (lCache1) tausvf(n)=tau   !Cache the value of tau for use in the next call to this routine.
          elntau=LOG(tau)*xLOG2x       !Calculate the log of tau to decrease the amount of computation needed below.
          taup(1:nt,n)=tiFEQ(1:nt,n)*elntau !Calculate the temperature parts required further below, and save them for future use.
        endif

      !-----Density dependent pieces
        if (.not.ld) then
          if (lCache1) delsvf(n)=del   !Cache the value of del
          elndel=LOG(del)*xLOG2x       !Do as above with tau
          delp(1:nt,n)=diFEQ(1:nt,n)*elndel
          dd(0)=0d0
          dd(1)=del
          do i=2,idlmax(n)
            dd(i)=dd(i-1)*del          !Precalculate del, del^2, del^3, etc., up to the value of idlmax for the EXP(-del^l) terms.
          enddo
          delli (1:np,n)= giFEQ(1:np,n)*dd(idliFEQ(1:np,n))
          dellid(1:np,n)=dliFEQ(1:np,n)*delli(1:np,n)
        endif

      !-----Gaussian terms
        if (ng.gt.0) then
          if (.not.ld) delb(ngs:ngx,n)=del-deltaFEQ(ngs:ngx,n)
          if (.not.lt) taua(ngs:ngx,n)=tau-gammaFEQ(ngs:ngx,n)
          if (.not.ltliflg(n)) then    !Check for regular terms first (those with integer exponents) and use the following code for increased calculation speed.
            if (.not.ld) then
              hxpf(ngs:ngx,n)=alphaFEQ(ngs:ngx,n)*delb(ngs:ngx,n)**2*del
              dxpf(ngs:ngx,n)=   diFEQ(ngs:ngx,n)+
     &                    2d0*alphaFEQ(ngs:ngx,n)*delb(ngs:ngx,n)*del
            endif
            if (.not.lt) then
              txpf(ngs:ngx,n)=betaFEQ(ngs:ngx,n)*taua(ngs:ngx,n)**2*tau
              axpf(ngs:ngx,n)=  tiFEQ(ngs:ngx,n)+
     &                    2d0*betaFEQ(ngs:ngx,n)*taua(ngs:ngx,n)*tau
            endif
          else
            do i=ngs,ngx               !Equation contains non-regular terms (e.g., R125), so must use IF statements to check for the non-integer powers.
              if (.not.ld) then
                db=delb(i,n)
                if (idliFEQ(i,n).ne.2) db=db**(idliFEQ(i,n)-1)
                deldb=del*db
                hxpf(i,n)=alphaFEQ(i,n)*delb(i,n)*deldb
                dxpf(i,n)=diFEQ(i,n)+alphaFEQ(i,n)*dliFEQ(i,n)*deldb
              endif
              if (.not.lt) then
                tb=taua(i,n)
                if (tliFEQ(i,n).ne.2.) tb=tb**(tliFEQ(i,n)-1d0)
                tautb=tau*tb
                txpf(i,n)=betaFEQ(i,n)*taua(i,n)*tautb
                axpf(i,n)=tiFEQ(i,n)+betaFEQ(i,n)*tliFEQ(i,n)*tautb
              endif
            enddo
          endif
        endif

      !-----Non-analytical terms
        if (nn.gt.0) then
          extf(nns:nnx,n)=1d0
          del1=del-1d0
          tau1=1d0-tau
          if (ABS(del1).lt.1d-10) del1=SIGN(1d-10,del1)
          if (ABS(tau1).lt.1d-10) tau1=SIGN(1d-10,tau1)
          s=SIGN(1d0,del1)
          da1=ABS(del1)
          del2=del**2
          do i=nns,nnx
            b1=1d0/betaFEQ(i,n)
            a0=alphaFEQ(i,n)
            da11=da1**(b1-1d0)
            g2  =2d0*gammaFEQ(i,n)*da11*b1
            tdg=tau1+gammaFEQ(i,n)*da11*da1
            tdg2=tdg**2
            if (eitf(i,n).eq.3.) then  !eitf is always either 3 or 3.5, so this implementation speeds things up by about 5%.
              del12=da1**6
              e3=eidf(i,n)*6d0*da1**5
            else
              del12=da1**7
              e3=eidf(i,n)*7d0*da1**6
            endif
            ex=tdg2+eidf(i,n)*del12
            exo=1d0/ex
            extf(i,n)=ex**a0
            ex1=a0*exo
            ex2=ex1*exo*(a0-1d0)
            ex3=s*(tdg*g2+e3)
            extdt(i,n)=-(ex2*ex3*tdg*2d0+ex1*g2*s)*del*tau
            ex4=(tdg*g2*(b1-1d0)+e3*(2d0*eitf(i,n)-1d0))/da1+g2**2/2d0
            extd(i,n)=ex3*ex1*del
            extd2(i,n)=(ex2*ex3**2+ex1*ex4)*del2
            extt(i,n)=-tdg*2d0*ex1*tau
            extt2(i,n)=ex2*tdg2*4d0+ex1*2d0
            delb(i,n)= del1
            hxpf(i,n)=-deltaFEQ(i,n)*del1*del    !Was originally this:  del1**(INT(dliFEQ(k,n))-1), but dliFEQ is always 2 for these terms.
            taua(i,n)=-tau1
            txpf(i,n)= etaf(i,n)*tau1*tau        !tau1**(INT(dliFEQ(k,n))-1)
          enddo
        endif

      !-----New terms
        if (nk.gt.0) then
          if (.not.ld) then
            delb(nks:nkx,n)=del-deltaFEQ(nks:nkx,n)
            hxpf(nks:nkx,n)=alphaFEQ(nks:nkx,n)*delb(nks:nkx,n)**2
          endif
          if (.not.lt) then
            taua(nks:nkx,n)=tau-gammaFEQ(nks:nkx,n)
            txpf(nks:nkx,n)= betaFEQ(nks:nkx,n)*taua(nks:nkx,n)**2
          endif
        endif


c  ----- Residual Helmholtz energy pieced back together-----

      !-----Polynomial terms
        do i=1,np
          ex=taup(i,n)+delp(i,n)-delli(i,n)*xLOG2x
          if (ex.gt. 100.) ex= 100d0
          if (ex.lt.-100.) ex=-100d0
          phisvf(i,n)=aFEQ(i,n)*2d0**ex
        enddo

      !-----Gaussian and non-analytical terms
        if (ng+nn.gt.0) then
          delx=1d0/del
          taux=1d0/tau
          do i=ngs,nnx
            if (i.le.ngx) then
              ex=taup(i,n)+delp(i,n)+
     &               (hxpf(i,n)*delx+txpf(i,n)*taux)*xLOG2x
            else
              ex=taup(i,n)+delp(i,n)+(hxpf(i,n)*delb(i,n)*
     &           delx+txpf(i,n)*taux*taua(i,n)+LOG(extf(i,n)))*xLOG2x
            endif
            if (ex.gt. 100.) ex= 100d0
            if (ex.lt.-100.) ex=-100d0
            phisvf(i,n)=aFEQ(i,n)*2d0**ex
          enddo
        endif

      !-----New terms
        if (nk.gt.0) then
          do i=nks,nkx
            ex=taup(i,n)+delp(i,n)+
     &         xLOG2x*(hxpf(i,n)+1d0/(txpf(i,n)+etaf(i,n)))
            if (ex.gt. 100.) ex= 100d0
            if (ex.lt.-100.) ex=-100d0
            phisvf(i,n)=aFEQ(i,n)*2d0**ex
          enddo
        endif

        PHIFEQ=SUM(phisvf(1:nt,n))
        lFEQsav(1,n)=.true.
        FEQsav(1,n)=PHIFEQ
      endif




c  Check if derivatives are requested.  Calculations make use of fact
c   that terms in derivative summations are very similar to A/RT terms.
c_______________________________________________________________________

c  ----- 1st derivative of alpha with respect to density -----
c  _____ del * d(alpha) / d(del) _____

      if (idel.eq.1) then

      !-----Polynomial terms
        phir01(1:np)=diFEQ(1:np,n)-dellid(1:np,n)

      !-----Gaussian terms
        if (ng.gt.0) phir01(ngs:ngx)=dxpf(ngs:ngx,n)

      !-----Non-analytical terms
        if (nn.gt.0) then
          phir01(nns:nnx)=1d0+extd(nns:nnx,n)+hxpf(nns:nnx,n)*2d0
        endif

      !-----New terms
        if (nk.gt.0) then
            phir01(nks:nkx)=diFEQ(nks:nkx,n)+
     &             2d0*alphaFEQ(nks:nkx,n)*(del-deltaFEQ(nks:nkx,n))*del
        endif

        PHIFEQ=SUM(phisvf(1:nt,n)*phir01(1:nt))

c  ----- 2nd derivative of alpha with respect to density -----
c  _____ del^2 * d^2(alpha) / d(del)^2 _____

      elseif (idel.eq.2) then
        D2=del**2

      !-----Polynomial terms
        phir02(1:np)=(diFEQ(1:np,n)-dellid(1:np,n))*(diFEQ(1:np,n)-
     &      1d0-dellid(1:np,n))-dliFEQ(1:np,n)*dellid(1:np,n)

      !-----Gaussian terms
        if (ng.gt.0) then
          if (.not.ltliflg(n)) then
            phir02(ngs:ngx)=dxpf(ngs:ngx,n)**2-diFEQ(ngs:ngx,n)+
     &          2d0*D2*alphaFEQ(ngs:ngx,n)
          else
            phir02(ngs:ngx)=dxpf(ngs:ngx,n)**2-
     &                      diFEQ(ngs:ngx,n)-D2*alphaFEQ(ngs:ngx,n)*
     &                      delb(ngs:ngx,n)**(idliFEQ(ngs:ngx,n)-2)*
     &                     (dliFEQ(ngs:ngx,n)-dliFEQ(ngs:ngx,n)**2)
          endif
        endif

      !-----Non-analytical terms
        if (nn.gt.0) then
          do i=nns,nnx
            dde=1d0+2d0*hxpf(i,n)
            phir02(i)=dde**2+2d0*del*hxpf(i,n)/delb(i,n)-1d0+
     &                extd2(i,n)+2d0*dde*extd(i,n)
          enddo
        endif

      !-----New terms
        if (nk.gt.0) then
          do i=nks,nkx
            di=diFEQ(i,n)
            dA =2d0*alphaFEQ(i,n)*(del-deltaFEQ(i,n))
            d2A=2d0*alphaFEQ(i,n)+dA**2
            phir02(i)=di*(di-1d0)+del*(2d0*di*dA+d2A*del)
          enddo
        endif

        PHIFEQ=SUM(phisvf(1:nt,n)*phir02(1:nt))

c  ----- 3rd derivative of alpha with respect to density -----
c  _____ del^3 * d^3(alpha) / d(del)^3 _____

      elseif (idel.eq.3) then

      !-----Polynomial terms
        do i=1,np
          dli=dliFEQ(i,n)
          dx2=diFEQ(i,n)-dellid(i,n)
          phir03(i)=dx2*(dx2-1d0)*(dx2-2d0)-
     &              dellid(i,n)*(3d0*dx2-3d0+dli)*dli
        enddo

      !-----Gaussian terms
        if (ng.gt.0) then
          D2=2d0*del**2
          phir03(nns:nnx)=0d0          !Zero out non-analytical terms
          if (.not.ltliflg(n)) then
            phir03(ngs:ngx)=dxpf(ngs:ngx,n)**3+2d0*diFEQ(ngs:ngx,n)-
     &           3d0*dxpf(ngs:ngx,n)*(diFEQ(ngs:ngx,n)-
     &           D2*alphaFEQ(ngs:ngx,n))
          else
            do i=ngs,ngx               !Non-analytical terms are not included here.
              dli=dliFEQ(i,n)
              dlb=del*dli*hxpf(i,n)/delb(i,n)**3*(dli-1d0)
              dlb2=dlb*del*(dli-2d0)
              dlb=3d0*(diFEQ(i,n)-dlb*delb(i,n))
              phir03(i)=dxpf(i,n)*(dxpf(i,n)**2-dlb)+2d0*diFEQ(i,n)+dlb2
            enddo
          endif
        endif

      !-----New terms
        if (nk.gt.0) then
          do i=nks,nkx
            a=alphaFEQ(i,n)
            di=diFEQ(i,n)
            dA=2d0*a*(del-deltaFEQ(i,n))
            d2A=dA**2+2d0*a
            d3A=dA**3+6d0*dA*a
            phir03(i)=di*(di-1d0)*(di-2d0)+3d0*di*(di-1d0)*dA*del+
     &                del**2*(3d0*di*d2A+d3A*del)
          enddo
        endif
        PHIFEQ=SUM(phisvf(1:nt,n)*phir03(1:nt))
      endif








c_______________________________________________________________________

c  ----- 1st derivative of alpha with respect to temperature -----
c  _____ tau * d(alpha) / d(tau) _____

      if (itau.eq.1) then

      !-----Polynomial terms
        phir10(1:np)=tiFEQ(1:np,n)

      !-----Gaussian terms
        if (ng.gt.0) phir10(ngs:ngx)=axpf(ngs:ngx,n)

      !-----Non-analytical terms
        if (nn.gt.0) phir10(nns:nnx)=(tiFEQ(nns:nnx,n)+
     &          extt(nns:nnx,n)+tliFEQ(nns:nnx,n)*txpf(nns:nnx,n))

      !-----New terms
        if (nk.gt.0) then
          do i=nks,nkx
            tg=tau-gammaFEQ(i,n)
            b =betaFEQ(i,n)
            f =b*tg**2+etaf(i,n)
            phir10(i)=tiFEQ(i,n)-2d0*b*tg/f**2*tau
          enddo
        endif

        PHIFEQ=SUM(phisvf(1:nt,n)*phir10(1:nt))


c  ----- 2nd derivative of alpha with respect to temperature -----
c  _____ tau^2 * d^2(alpha) / d(tau)^2 _____

      elseif (itau.eq.2) then

      !-----Polynomial terms
        phir20(1:np)=tiFEQ(1:np,n)*(tiFEQ(1:np,n)-1d0)
        T2=tau**2

      !-----Gaussian terms
        if (ng.gt.0) then
          if (.not.ltliflg(n)) then
            phir20(ngs:ngx)=(axpf(ngs:ngx,n)**2-
     &          tiFEQ(ngs:ngx,n)+2d0*T2*betaFEQ(ngs:ngx,n))
          else
            phir20(ngs:ngx)=(axpf(ngs:ngx,n)**2-
     &          tiFEQ(ngs:ngx,n)-T2*betaFEQ(ngs:ngx,n)*taua(ngs:ngx,n)**
     &          (tliFEQ(ngs:ngx,n)-2d0)*(tliFEQ(ngs:ngx,n)-
     &           tliFEQ(ngs:ngx,n)**2))
          endif
        endif

      !-----Non-analytical terms
        if (nn.gt.0) then
          phir20(nns:nnx)=T2*extt2(nns:nnx,n)+
     &        txpf(nns:nnx,n)*(4d0*txpf(nns:nnx,n)+
     &        2d0*tau/taua(nns:nnx,n)+4d0*extt(nns:nnx,n))
        endif

      !-----New terms
        if (nk.gt.0) then
          do i=nks,nkx
            b =betaFEQ(i,n)
            ti=tiFEQ(i,n)
            tg=tau-gammaFEQ(i,n)
            f =1d0/(b*tg**2+etaf(i,n))
            df=2d0*b*tg
            dA =-df*f**2
            d2A=dA**2+2d0*(df**2*f-b)*f**2
            phir20(i)=ti*(ti-1d0)+2d0*ti*dA*tau+d2A*T2
          enddo
        endif

        PHIFEQ=SUM(phisvf(1:nt,n)*phir20(1:nt))


c  ----- 3rd derivative of alpha with respect to temperature -----
c  _____ tau^3 * d^3(alpha) / d(tau)^3 _____

      elseif (itau.eq.3) then
        T2=tau**2

      !-----Polynomial terms
        phir30(1:np)=tiFEQ(1:np,n)*
     &        (tiFEQ(1:np,n)-1d0)*(tiFEQ(1:np,n)-2d0)

      !-----Gaussian terms
        if (ng.gt.0) then              !Non-analytical terms are not included here.
          T2=2d0*T2
          if (.not.ltliflg(n)) then
            phir30(ngs:ngx)=(axpf(ngs:ngx,n)**3+
     &       2d0*tiFEQ(ngs:ngx,n)-3d0*axpf(ngs:ngx,n)*(tiFEQ(ngs:ngx,n)-
     &       T2*betaFEQ(ngs:ngx,n)))
          else
            do i=ngs,ngx
              tli=tliFEQ(i,n)
              tlb=tau*tli*txpf(i,n)/taua(i,n)**3*(tli-1d0)
              tlb2=tlb*tau*(tli-2d0)
              tlb=3d0*(tiFEQ(i,n)-tlb*taua(i,n))
              phir30(i)=(axpf(i,n)*
     &               (axpf(i,n)**2-tlb)+2d0*tiFEQ(i,n)+tlb2)
            enddo
          endif
        endif

      !-----Non-analytical terms
        if (nn.gt.0) phir30(nns:nnx)=0d0

      !-----New terms
        if (nk.gt.0) then
          do i=nks,nkx
            b= betaFEQ(i,n)
            ti=tiFEQ(i,n)
            tg=tau-gammaFEQ(i,n)
            f =b*tg**2+etaf(i,n)
            df=2d0*b*tg
            dA =-df/f**2
            d2A=-dA*df*(1d0+2d0*f)-2d0*b
            d3A= 6d0*dA*f*(df**2-f*df*dA-b)+12d0*df*b
            phir30(i)=ti*(ti-1d0)*(ti-2d0+3d0*dA*tau) + (dA*tau)**3 +
     &             (3d0*ti*d2A*f+d3A*tau)*tau**2/f**3
          enddo
        endif

        PHIFEQ=SUM(phisvf(1:nt,n)*phir30(1:nt))
      endif


c  ----- Partial derivatives with respect to both temperature and density-----
c  _____ del*tau * d^2(alpha) / [d(tau) d(del)] _____

      if (itau.eq.1 .and. idel.eq.1) then
        phisum=SUM(phisvf(  1:ngx,n)*phir10(  1:ngx)*phir01(  1:ngx))     !Don't do non-analytical terms here because they cannot be separated into density and temperature independent pieces.
     &        +SUM(phisvf(nks:nkx,n)*phir10(nks:nkx)*phir01(nks:nkx))

      !-----Non-analytical terms
        if (nn.gt.0) then
          do i=nns,nnx
            t1=txpf(i,n)*2d0
            d1=1d0+hxpf(i,n)*2d0
            phisum=phisum+phisvf(i,n)*(t1*extd(i,n)+d1*extt(i,n)+t1*d1+
     &                    extdt(i,n))
          enddo
        endif
        PHIFEQ=phisum

c  _____ del*tau^2 * d^3(alpha) / [d(tau)^2 d(del)] _____
      elseif (itau.eq.2 .and. idel.eq.1) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir20(1:nt)*phir01(1:nt))

c  _____ del^2*tau * d^3(alpha) / [d(tau) d(del)^2] _____
      elseif (itau.eq.1 .and. idel.eq.2) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir10(1:nt)*phir02(1:nt))

c  _____ del^2*tau^2 * d^4(alpha) / [d(tau)^2 d(del)^2] _____
      elseif (itau.eq.2 .and. idel.eq.2) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir20(1:nt)*phir02(1:nt))

      elseif (itau.eq.3 .and. idel.eq.1) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir30(1:nt)*phir01(1:nt))

      elseif (itau.eq.1 .and. idel.eq.3) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir10(1:nt)*phir03(1:nt))

      elseif (itau.eq.3 .and. idel.eq.2) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir30(1:nt)*phir02(1:nt))

      elseif (itau.eq.2 .and. idel.eq.3) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir20(1:nt)*phir03(1:nt))

      elseif (itau.eq.3 .and. idel.eq.3) then
        PHIFEQ=SUM(phisvf(1:nt,n)*phir30(1:nt)*phir03(1:nt))

      endif

      FEQsav(ncode,n)=PHIFEQ      !Save for later use if routine is called again with same itau and idel.
      if (lCache1) lFEQsav(ncode,n)=.true.

      end                                                function PHIFEQ
c
c ======================================================================
c
      subroutine SETFEQ (nread,icomp,ierr,herr)
c
c  Set up working arrays for use with the fundamental equation of state.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  07-20-95  MM, Original version

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character herr*255

      ierr=0
      herr=' '
      n=icomp
      tausvf=0d0   !(Re)initialize saved variables when a new fluid is read in.
      delsvf=0d0
      phisvf=0d0
      tausv2=0d0
      delsv2=0d0
      phisv2=0d0
      taup  =0d0
      delp  =0d0
      delli =0d0

c  Read data from file.
c     if (n.gt.0 .and. imrs(n).ne.0) read (nread,*)   !Extra line included in MRS files to cause previous versions of Refprop to crash if they attempt to read an MRS file.
      read (nread,*,err=102) Tmeos(n)     !Lower temperature limit
      read (nread,*,err=102) Txeos(n)     !Upper temperature limit
      read (nread,*,err=102) Pxeos(n)     !Upper pressure limit
      read (nread,*,err=102) Dxeos(n)     !Upper density limit
      call READHA (nread,3,hcpk(n))!Pointer to Cp0 model
      read (nread,*,err=102) wmas(n)      !Molar mass
      read (nread,*,err=102) Ttpn(n)      !Triple point temperature
      read (nread,*,err=102) Ptpn(n)      !Pressure at triple point
      read (nread,*,err=102) Dtpn(n)      !Density at triple point
      read (nread,*,err=102) Tnbp(n)      !Normal boiling point temperature
      read (nread,*,err=102) accen(n)     !Acentric factor
      read (nread,*,err=102) Tcrit(n),Pcrit(n),Dcrit(n)  !Critical parameters
      Dtpv(n)=0d0
      read (nread,*,err=102) Tzred(n),Dzred(n) !Reducing parameters
      Vzred(n)=1d0/Dzred(n)
      read (nread,*,err=102) ReosOrg(n)        !Gas constant used in fit

c  Check for Wagner equations of state.
c   If the fluid being setup is not one of the fluids listed below, use a "constant" gas constant.
c   Otherwise the value of R will be that reported in the Wagner equation of state.
      iFlg=0
      Rdiff=ABS(ReosOrg(n)-xRgas)*1d6
      if (Rdiff.gt.8.) then            !The difference between all Wagner equations and the current constant is more than 10 ppm.  If less, EOS has been updated.
        if (iFluidID(n).eq.1)  iFlg=1  !Water
        if (iFluidID(n).eq.16) iFlg=1  !Nitrogen
        if (iFluidID(n).eq.19) iFlg=1  !Argon
        if (iFluidID(n).eq.20) iFlg=1  !Oxygen
        if (iFluidID(n).eq.26) iFlg=1  !Ethylene
        if (iFluidID(n).eq.31) iFlg=1  !CO2
        if (iFluidID(n).eq.101)iFlg=1  !Methane
        if (iFluidID(n).eq.102)iFlg=1  !Ethane
      endif
      ReosDef(n)=xRgas
      if (iFlg.eq.1) then
        lWagner=.true.
        lRcnstWag=.false.
        ReosDef(n)=ReosOrg(n)     !Use Wagner gas constant.
      endif
      Reos(n)=ReosDef(n)
      if (nc.eq.1 .and. n.eq.1) Rxgas=Reos(n)

      nFEQkg(n)=0
      read (nread,*,err=102)nFEQpe(n),ncoeff,nFEQcrt(n),ncfcrt,nFEQkg(n)
      nFEQterms(n)=nFEQpe(n)+nFEQcrt(n)+nFEQkg(n)
      nFEQpoly(n)=0
      nFEQgaus(n)=0
      nFEQnan (n)=0
      ltliflg (n)=.false.
      do j=1,nFEQpe(n)
        if (ncoeff.eq.5) then     !Check if fluid file has 4 or 5 coefficients and exponents on each line.
          read (nread,*,err=102) aFEQ(j,n),tiFEQ(j,n),diFEQ(j,n),  !The gi term is a multiplier for D or del in only the exponential
     &                   dliFEQ(j,n),giFEQ(j,n)            !terms; it is needed for the Bender EOS; set to 1.0 if not present.
        else
          read (nread,*,err=102) aFEQ(j,n),tiFEQ(j,n),
     &                   diFEQ(j,n),dliFEQ(j,n)
          giFEQ(j,n)=1d0
        endif
        idliFEQ(j,n)=INT(dliFEQ(j,n))
        if (dliFEQ(j,n).eq.0.) nFEQpoly(n)=j     !Number of polynomial terms with no exponential piece
      enddo
      if (nFEQcrt(n)+nFEQkg(n).gt.0) then
        do j=nFEQpe(n)+1,nFEQpe(n)+nFEQcrt(n)+nFEQkg(n)    !Number of Gaussian-Bell shaped terms + non-analytical terms + new terms
          giFEQ(j,n)=0d0
          read (nread,*,err=100,end=100) aFEQ(j,n),tiFEQ(j,n),
     &        diFEQ(j,n),dliFEQ(j,n),tliFEQ(j,n),
     &        alphaFEQ(j,n),betaFEQ(j,n),gammaFEQ(j,n),deltaFEQ(j,n),
     &        etaf(j,n),eidf(j,n),eitf(j,n)
          idliFEQ(j,n)=INT(dliFEQ(j,n))
          if (ABS(etaf(j,n)).gt.1d-20 .and.
     &        ABS(eidf(j,n)).gt.1d-20 .and.
     &        ABS(eitf(j,n)).gt.1d-20 .and.
     &            j.gt.nFEQpe(n)+nFEQkg(n)) nFEQnan(n)=nFEQnan(n)+1
          if (dliFEQ(j,n).ne.2.) ltliflg(n)=.true.
          if (tliFEQ(j,n).ne.2.) ltliflg(n)=.true.
        enddo
        nFEQgaus(n)=nFEQcrt(n)-nFEQnan(n)
      endif

      idlmax(n)=0  !Old SETEXP routine.
      do i=1,nFEQterms(n)
         if (idliFEQ(i,n).gt.idlmax(n)) idlmax(n)=idliFEQ(i,n)  !Find the maximum density exponent.
      enddo
      RETURN

 100  call ERRNUM (116,icomp,' ',' ',0d0,0d0,0d0,ierr,herr)
      RETURN
 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)

      end                                              subroutine SETFEQ
c
c ======================================================================
c ======================================================================
c
      subroutine SETBWR (nread,icomp,ierr,herr)
c
c  Set up working arrays for use with the MBWR equation of state.  The
c  coefficients and parameters are converted into the FEQ style
c  to remove the need for separate code dedicated to the BWR EOS.
c
c  All conversions are based on the equations in
c  Younglove and McLinden, J. Phys. Chem. Ref. Data, 23:731-779, 1994.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  07-20-95  MM, Original version
c  06-21-16 EWL, Remove all code in CORE_BWR.FOR, and replace it with the code
c                here that converts the coefficients and parameters to the
c                FEQ equation of state.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character herr*255
      dimension abwr(32),ebwr(32),gbwr(32),tbwr(32),dbwr(32)
      data tbwr/
     & -1d0,-0.5d0,0d0,1d0,2d0,-1d0,0d0,1d0,2d0,-1d0,0d0,
     & 1d0,0d0,1d0,2d0,1d0,1d0,2d0,2d0,2d0,3d0,2d0,4d0,
     & 2d0,3d0,2d0,4d0,2d0,3d0,2d0,3d0,4d0/
      data dbwr/
     & 2d0,2d0,2d0,2d0,2d0,3d0,3d0,3d0,3d0,4d0,4d0,4d0,
     & 5d0,6d0,6d0,7d0,8d0,8d0,9d0,3d0,3d0,5d0,5d0,7d0,
     & 7d0,9d0,9d0,11d0,11d0,13d0,13d0,13d0/

      ierr=0
      herr=' '

      n=icomp
      read (nread,*,err=102) Tmeos(n)     !Lower temperature limit
      read (nread,*,err=102) Txeos(n)     !Upper temperature limit
      read (nread,*,err=102) Pxeos(n)     !Upper pressure limit
      read (nread,*,err=102) Dxeos(n)     !Upper density limit
      call READHA (nread,3,hcpk(n)) !Pointer to Cp0 model
      read (nread,*,err=102) wmas(n)      !Molar mass
      read (nread,*,err=102) Ttpn(n)      !Triple point temperature
      read (nread,*,err=102) Ptpn(n)      !Pressure at triple point
      read (nread,*,err=102) Dtpn(n)      !Density at triple point
      read (nread,*,err=102) Tnbp(n)      !Normal boiling point temperature
      read (nread,*,err=102) accen(n)     !Acentric factor
      read (nread,*,err=102) Tcrit(n),Pcrit(n),Dcrit(n)  !Critical parameters
      read (nread,*,err=102) Tred,Dred    !Reducing parameters (same as crit)
      read (nread,*,err=102) gamm         !gamma (usually equal to Dc)
      read (nread,*,err=102) ReosOrg(n)   !EOS gas constant
      ReosOrg(n)=ReosOrg(n)*100d0 !MBWR uses pressure in bar.
      ReosDef(n)=xRgas
      Reos(n)=xRgas
      if (nc.eq.1 .and. n.eq.1) Rxgas=Reos(n)

      Dtpv(n) =0d0
      Tzred(n)=Tcrit(n)           !Reducing parameters
      Dzred(n)=Dcrit(n)
      Vzred(n)=1d0/Dzred(n)

      read (nread,*,err=102) nterm,ncoef  !Always equal to 32, 1
      do j=1,28,3
        read (nread,*,err=102,end=102) abwr(j),abwr(j+1),abwr(j+2)
      enddo
      read (nread,*,err=102) abwr(31),abwr(32)
      abwr=abwr*100d0        !Convert from L-bar/mol to J/mol

c  Set number of terms and initialize the BWR arrays.
c  The arrays tbwr, dbwr, abwr, ebwr, and gbwr contain the original BWR values.
      nFEQpe(n)=nterm
      nFEQterms(n)=nterm
      nFEQcrt(n)=0
      gamm=(Dcrit(n)/gamm)**2
      if (ABS(gamm-1d0).lt.1d-10) gamm=1d0
      ebwr=0d0
      gbwr=0d0
      ebwr(20:32)=2d0
      gbwr(20:32)=gamm

c  Convert temperature and density exponents, and calculate the new
c   coefficient for the FEQ equation of state.
      m=0
      do k=1,32
        m=m+1
        ti=tbwr(k)+1d0
        di=dbwr(k)-1d0
        ba=abwr(k)*Dcrit(n)**di/Tcrit(n)**ti/Reos(n)
        ei=ebwr(k)
        gi=gbwr(k)
        aFEQ(m,n)=ba/di
        tiFEQ(m,n)=ti
        diFEQ(m,n)=di
        dliFEQ(m,n)=ei
        idliFEQ(m,n)=INT(ei)
        giFEQ(m,n)=gi

c  Check for exponential terms in EOS, and convert to FEQ form.
        if (gi.gt.0.) then
          ba=-ba/2d0         !Coefficient is different than that above for EXP terms.
          di=di-2d0
          diFEQ(m,n)=di
          ba=ba/gi
          aFEQ(m,n)=ba
        endif

c  Expand the exponential terms since several terms can result from the integration
c   of the pressure explicit equation to the Helmholtz energy equation.
        bi=1d0
        bj=di/2d0
        do while (gi.gt.0.)
          if (di.gt.0.5d0) then
            bi=bi*bj         !Get coefficient for each density term as shown
            bj=bj-1d0        ! in the brackets of equation B6 of Younglove and McLinden.
          endif
          m=m+1
          di=di-2d0
          if (di.lt.-0.5d0 .and. gi.gt.0.001d0) then
            di=0d0           !Constant shown in equation B6
            gi=0d0           ! at the end of each line.
            ei=0d0
            ba=-ba
          else
            ba=ba/gi
          endif
          aFEQ(m,n)=ba*bi
          tiFEQ(m,n)=ti
          diFEQ(m,n)=di
          dliFEQ(m,n)=ei
          idliFEQ(m,n)=INT(ei)
          giFEQ(m,n)=gi

c  Search to see if a similar term has already been set up, and if so,
c   then add the two coefficients together.  This will result in 40 terms
c   total for the converted BWR to FEQ equation.
          do ix=1,m-1
            if (tiFEQ(ix,n).eq.ti .and. diFEQ(ix,n).eq.di .and.
     &         dliFEQ(ix,n).eq.ei .and. giFEQ(ix,n).eq.gi) then
              aFEQ(ix,n)=aFEQ(ix,n)+aFEQ(m,n)
              m=m-1
              EXIT
            endif
          enddo
        enddo
      enddo
      nFEQpe(n)=m
      nFEQterms(n)=m
      idlmax(n)=0  !Old SETEXP routine.
      do i=1,nFEQterms(n)
        if (idliFEQ(i,n).gt.idlmax(n)) idlmax(n)=idliFEQ(i,n)   !Find the maximum density exponent.
      enddo
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                              subroutine SETBWR
c
c ======================================================================
c ======================================================================
c
      function PHIECS (icomp,itau,idel,tau,del)
c
c  Compute reduced Helmholtz energy or its derivatives as functions
c  of dimensionless temperature and density for the ECS model.
c
c  The Helmholtz energy consists of ideal and residual (real-gas)
c  terms; this routine calculates only the residual part.
c
c  Inputs:
c    icomp--Pointer specifying component (1..nc)
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           When itau = 0 and idel = 0, compute A/RT
c           When itau = 0 and idel = 1, compute 1st density derivative
c           When itau = 1 and idel = 1, compute cross derivative
c           etc.
c      tau--Dimensionless temperature (To/T)
c      del--Dimensionless density (D/Do)
c
c  Output (as the function value):
c      phi--Residual (real-gas) part of the Helmholtz energy, or one
c           of its derivatives (as specified by itau and idel),
c           in reduced form (A/RT)
c           itau  idel    output (dimensionless for all cases)
c             0    0      A/RT
c             1    0      tau*[d(A/RT)/d(tau)]
c             2    0      tau^2*[d^2(A/RT)/d(tau)^2]
c             0    1      del*[d(A/RT)/d(del)]
c             0    2      del^2*[d^2(A/RT)/d(del)^2]
c             1    1      tau*del*[d^2(A/RT)/d(tau)d(del)]
c                         etc.
c
c  Cannot use the PHIK function here because it calls this one,
c         and recursive calls are not generally allowed in Fortran.
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-02-96  MM, Original version
c  06-14-16 EWL, Shorten code in all routines in CORE_ECS.FOR

      include 'COMMONS.INC'
      include 'COMFEQ.INC'

      PHIECS=0d0             !Initialize outputs and intermediate results.
      if (del.le.1d-10 .or. tau.gt.1000) RETURN  !Trivial solution at zero density

c  Recover the temperature and density from tau and del (need to compute
c   shape factors at actual temperature rather than reduced temperature).
      i=icomp
      T=Tcrit(i)/tau
      D=del*Dcrit(i)
      call FJ (i,T,D,f,dfdt,d2fdt2,dfdd,d2fdd2,d2fdtd)
      call HJ (i,T,D,h,dhdt,d2hdt2,dhdd,d2hdd2,d2hdtd)

c  Find the reducing temperature to compute tau for the reference fluid and
c   then calculate the reduced Helmholtz energy for the reference fluid at the
c   conformal temperature and density.
      iref=ncmax+icomp
      tred0=Tzred(iref)
      Dred0=Dzred(iref)
      tau0=tred0/T*f
      del0=XDIV(D*h,Dred0)
      trt=tred0/T
      hrd=h+D*dhdd
      ftd=f-T*dfdt
      if (ABS(h).lt.1d-10) RETURN
      dtlogh=dhdt/h          !Equal to temperature derivative of LOG(h).
      if (del0.le.0. .or. tau0.le.0.) RETURN

c  Compute PHI and its derivatives.
      if (iEOSk(iref).eq.iEOSFEQ) then
        phixx=PHIFEQ(iref,itau,idel,tau0,del0)/del0**idel/tau0**itau
        phi01=PHIFEQ(iref,0,1,tau0,del0)/del0
        phi02=PHIFEQ(iref,0,2,tau0,del0)/del0**2
        phi10=PHIFEQ(iref,1,0,tau0,del0)/tau0
        phi20=PHIFEQ(iref,2,0,tau0,del0)/tau0**2
        phi11=PHIFEQ(iref,1,1,tau0,del0)/del0/tau0
      else
        RETURN
      endif

c  Check which derivative is requested.
      if (itau.eq.0) then
        if (idel.eq.0) then
          PHIECS=phixx                                                    !Dimensionless residual Helmholtz of reference fluid.
        elseif (idel.eq.1) then
          PHIECS=Dcrit(i)/Dred0*phixx*hrd+Dcrit(i)*tau0/f*phi10*dfdd      !1st derivative w.r.t. del (dimensionless density)
        elseif (idel.eq.2) then
          PHIECS=Dcrit(i)**2*(((phixx*hrd/Dred0+trt*phi11*dfdd)*hrd+      !2nd derivative w.r.t. del
     &     phi01*(2d0*dhdd+D*d2hdd2))/Dred0+
     &     trt*((phi11/Dred0*hrd+trt*phi20*dfdd)*dfdd+d2fdd2*phi10))
        endif
      elseif (itau.eq.1) then
        if (idel.eq.0) then
         PHIECS=tred0/Tcrit(i)*ftd*phixx-T**2*del0/Tcrit(i)*dtlogh*phi01  !1st derivative w.r.t. tau (dimensionless temperature)
        elseif (idel.eq.1) then
          PHIECS=Dcrit(i)/Dred0/Tcrit(i)*                                 !Cross derivative
     &          (tred0*ftd*(phixx*hrd+trt*Dred0*phi20*dfdd)+
     &           tred0*phi10*Dred0*(dfdd-T*d2fdtd)-
     &           T**2*(phi01*dhdt+D*dhdt*
     &          (phi02/Dred0*hrd+trt*phixx*dfdd)+D*phi01*d2hdtd))
        endif
      elseif (itau.eq.2 .and. idel.eq.0) then
        PHIECS=(1d0/Tcrit(i)**2)*(tred0**2*ftd**2*phixx-                  !2nd derivative w.r.t. tau.
     &         2d0*T**2*tred0*del0*ftd*dtlogh*phi11+
     &         T**4*del0**2*dtlogh**2*phi02+tred0*T**3*d2fdt2*phi10+
     &         T**3*del0/h*(T*d2hdt2+2d0*dhdt)*phi01)
      endif
      PHIECS=PHIECS*del**idel*tau**itau

      end                                                function PHIECS
c
c ======================================================================
c
      subroutine SETECS (nread,icomp,hreff,heqn,ierr,herr)
c
c  Set up working arrays for use with ECS model.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c
c  Outputs:
c    hreff--File containing reference fluid EOS (character*255)
c     heqn--Model ('BWR', etc) for reference fluid EOS (character*3)
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-06-95  MM, Original version

      include 'COMMONS.INC'
      character heqn*3,herr*255,hreff*255

      ierr=0
      herr=' '
      if (nread.eq.0) RETURN

c  Read data from file.
      Reos(icomp)=xRgas
      if (nc.eq.1 .and. icomp.eq.1) Rxgas=Reos(icomp)
      Ptpn(icomp)=0d0
      Dtpv(icomp)=0d0
      read (nread,*,err=102) Tmeos(icomp)      !Lower temperature limit
      read (nread,*,err=102) Txeos(icomp)      !Upper temperature limit
      read (nread,*,err=102) Pxeos(icomp)      !Upper pressure limit
      read (nread,*,err=102) Dxeos(icomp)      !Upper density limit
      Dtpn(icomp)=Dxeos(icomp)
      call READHA (nread,3,hcpk(icomp))!Pointer to Cp0 model
      call READHA (nread,255,hreff)    !Reference fluid *.fld file
      call READHA (nread,3,heqn)       !Reference fluid equation of state
      read (nread,*,err=102) acfref(icomp)     !Acentric factor for reference fluid
      read (nread,*,err=102) Zcref(icomp)      !Zc for reference fluid
      read (nread,*,err=102) acfecs(icomp)     !Acentric factor
      read (nread,*,err=102) Tcrit(icomp)      !Critical temperature [K]
      read (nread,*,err=102) Pcrit(icomp)      !Critical pressure [kPa]
      read (nread,*,err=102) Dcrit(icomp)      !Critical density [mol/L]
      Tzred(icomp)=Tcrit(icomp)
      Dzred(icomp)=Dcrit(icomp)
      Vzred(icomp)=1d0/Dzred(icomp)
      read (nread,*,err=102) nfecs(icomp)      !Number of temperature terms for 'f' ESRR
      do j=1,nfecs(icomp)
        read (nread,*,err=102) fecs(icomp,j,1),fecs(icomp,j,2)
      enddo
      read (nread,*,err=102) nfdecs(icomp)     !Number of density terms for 'f' ESRR
      if (nfdecs(icomp).ge.1) then
        do j=1,nfdecs(icomp)
          read (nread,*,err=102) fdecs(icomp,j,1),fdecs(icomp,j,2)
        enddo
      endif
      read (nread,*,err=102) nhecs(icomp)      !Number of temperature terms for 'h' ESRR
      do j=1,nhecs(icomp)
        read (nread,*,err=102) hecs(icomp,j,1),hecs(icomp,j,2)
      enddo
      read (nread,*,err=102) nhdecs(icomp)     !Number of density terms for 'h' ESRR
      if (nhdecs(icomp).ge.1) then
        do j=1,nhdecs(icomp)
          read (nread,*,err=102) hdecs(icomp,j,1),hdecs(icomp,j,2)
        enddo
      endif
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                              subroutine SETECS
c
c ======================================================================
c
      subroutine FJ (icomp,T,D,f,dfdt,d2fdt2,dfdd,d2fdd2,d2fdtd)
c
c  Compute the equivalent substance reducing ratio (for temperature)
c  and its temperature derivative for use in the ECS model.
c
c  This routine implements the function of Huber and Ely, Int. J. Refrig.
c  17:18-31, 1994 modified by additional general terms in (T/Tc) and by
c  density-dependent terms.  The beta1 and beta2 of Huber and Ely are
c  hecs(i,1,1) and hecs(i,2,1) here.
c
c  Inputs:
c    icomp--Pointer specifying component (1..nc)
c        T--Temperature [K]
c        D--Density [mol/L]
c
c  Outputs:
c        f--Equivalent substance reducing ratio (for temperature)
c           This is equal to (Tc,icomp/Tc,ref)*theta(T)
c           where theta is the energy (or temperature) shape factor.
c     dfdt--Temperature derivative of f
c   d2fdt2--Second temperature derivative of f
c     dfdd--Density derivative of f
c   d2fdd2--Second density derivative of f
c   d2fdtd--Temperature-density cross derivative of f
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-06-96  MM, Original version
c
      include 'COMMONS.INC'

      f=0d0
      dfdt=0d0
      d2fdt2=0d0
      dfdd=0d0
      d2fdd2=0d0
      d2fdtd=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      iref=ncmax+icomp
      Tr=T/Tcrit(i)
      Dr=D/Dcrit(i)
      domega=acfecs(i)-acfref(i)
      theta=1d0+domega*(fecs(i,1,1)+fecs(i,2,1)*LOG(Tr))   !The variable theta is the density shape factor.
c  First and second (reduced) temperature derivatives of theta.
      d1th=domega*fecs(i,2,1)/Tr       !First Tr derivative of theta
      d2th=-domega*fecs(i,2,1)/(Tr*Tr) !Second Tr derivative of theta
      if (nfecs(i).ge.3) then          !Sum terms beyond those in Huber and Ely function.
        do j=3,nfecs(i)
          theta=theta+fecs(i,j,1)*Tr**fecs(i,j,2)
          d1th=d1th+fecs(i,j,1)*fecs(i,j,2)*Tr**(fecs(i,j,2)-1d0)
          d2th=d2th+fecs(i,j,1)*fecs(i,j,2)*(fecs(i,j,2)-1d0)*
     &         Tr**(fecs(i,j,2)-2d0)
        enddo
      endif
      d1thd=0d0
      d2thd=0d0
      if (nfdecs(i).gt.0) then
        do j=1,nfdecs(i)
          theta=theta+fdecs(i,j,1)*Dr**fdecs(i,j,2)
          d1thd=d1thd+fdecs(i,j,1)*fdecs(i,j,2)*Dr**(fdecs(i,j,2)-1d0)
          d2thd=d2thd+fdecs(i,j,1)*fdecs(i,j,2)*(fdecs(i,j,2)-1d0)*
     &          Dr**(fdecs(i,j,2)-2d0)
        enddo
      endif
c
c  Convert theta and its derivatives to 'f' and its derivatives.
      Tcrat=Tcrit(icomp)/Tcrit(iref)
      f=Tcrat*theta
      dfdt=d1th/Tcrit(iref)
      d2fdt2=d2th/(Tcrit(icomp)*Tcrit(iref))
      dfdd=d1thd*Tcrat/Dcrit(icomp)
      d2fdd2=d2thd*Tcrat/Dcrit(icomp)**2
      d2fdtd=0d0        !Approximation. If needed, calculation of d1thdt is required.

      end                                                  subroutine FJ
c
c ======================================================================
c
      subroutine HJ (icomp,T,D,h,dhdt,d2hdt2,dhdd,d2hdd2,d2hdtd)
c
c  Compute the equivalent substance reducing ratio (for density)
c  and its temperature derivative for use in the ECS model.
c
c  This routine implements the function of Huber and Ely, Int. J. Refrig.,
c  17:18-31, 1994 modified by additional general terms in (T/Tc) and by
c  density-dependent terms.  The beta1 and beta2 of Huber and Ely are
c  hecs(i,1,1) and hecs(i,2,1) here.
c
c  Inputs:
c    icomp--Pointer specifying component (1..nc)
c        T--Temperature [K]
c        D--Density [mol/L]
c
c  Outputs:
c        h--Equivalent substance reducing ratio (for density)
c           This is equal to (Dc,ref/Dc,icomp)*phi(T)
c           where phi is the volume (or density) shape factor.
c     dhdt--Temperature derivative of h
c   d2hdt2--Second temperature derivative of h
c     dhdd--Density derivative of h
c   d2hdd2--Second density derivative of h
c   d2hdtd--Temperature-density cross derivative of h
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-06-96  MM, Original version

      include 'COMMONS.INC'

      h=0d0
      dhdt=0d0
      d2hdt2=0d0
      dhdd=0d0
      d2hdd2=0d0
      d2hdtd=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      Tr=T/Tcrit(i)
      Dr=D/Dcrit(i)
      domega=acfecs(i)-acfref(i)
      Zc=Pcrit(i)/(Reos(i)*Tcrit(i)*Dcrit(i))
      Zrat=Zcref(i)/Zc
c  The variable phi is the density shape factor, not to be confused with PHI=A/RT.
      phi=Zrat*(1d0+domega*(hecs(i,1,1)+hecs(i,2,1)*LOG(Tr)))

c  First and second (reduced) temperature derivatives of phi.
      d1phi= Zrat*domega*hecs(i,2,1)/Tr
      d2phi=-Zrat*domega*hecs(i,2,1)/Tr**2

c  Sum terms beyond those in Huber and Ely function.
      if (nhecs(i).ge.3) then
        do j=3,nhecs(i)
          phi=phi+hecs(i,j,1)*Tr**hecs(i,j,2)
          d1phi=d1phi+hecs(i,j,1)*hecs(i,j,2)*Tr**(hecs(i,j,2)-1d0)
          d2phi=d2phi+hecs(i,j,1)*hecs(i,j,2)*(hecs(i,j,2)-1d0)*
     &          Tr**(hecs(i,j,2)-2d0)
        enddo
      endif

      d1phid=0d0
      d2phid=0d0
      if (nhdecs(i).gt.0) then
        do j=1,nhdecs(i)
         phi=phi+hdecs(i,j,1)*Dr**hdecs(i,j,2)
         d1phid=d1phid+hdecs(i,j,1)*hdecs(i,j,2)*Dr**(hdecs(i,j,2)-1d0)
         d2phid=d2phid+hdecs(i,j,1)*hdecs(i,j,2)*(hdecs(i,j,2)-1d0)*
     &          Dr**(hdecs(i,j,2)-2d0)
        enddo
      endif

c  Convert phi and its derivatives to 'h' and its derivatives.
      Dcrat=Dcrit(ncmax+icomp)/Dcrit(icomp)
      h=Dcrat*phi
      dhdt=d1phi*Dcrat/Tcrit(icomp)
      d2hdt2=d2phi*Dcrat/Tcrit(icomp)**2
      dhdd=d1phid*Dcrat/Dcrit(icomp)
      d2hdd2=d2phid*Dcrat/Dcrit(icomp)**2

      end                                                  subroutine HJ
c
c ======================================================================
c
      function CP0 (T,z)
c
c  Calculate Cp0 for a mixture given temperature and composition.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition array (array of mole fractions)
c
c  Output (as the function value):
c      CP0--Ideal-gas heat capacity, Cp0 [J/(mol-K)]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  07-24-95  MM, original version
c  06-13-16 EWL, shorten code in all routines in IDEALGAS.FOR

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CP0

      Cp0=0d0
      if (T.le.0.) RETURN
      call ISPURE (z,i)
      if (.not.lRcnst .or. .not.lRcnstWag) call RMIX (z)
      if (i.ne.0) then
        Cp0i=0
        if (iCP0xx(i).eq.iCP0CP) call CP0CPX (i,T,Cp0i,CpI,CpT,Cp3)       !Polynomial fit of isobaric heat capacity for the ideal-gas state
        if (iCP0xx(i).eq.iCP0PH) Cp0i=Rxgas*(1d0-PH0PH0(i,2,0,T,0d0))     !Helmholtz form (fundamental equation)
        Cp0=Cp0i
      else
        Cp0sum=0d0
        do i=1,nc
          if (z(i).gt.1d-10) then      !Compute only if component composition >0.
            Cp0i=0d0
            if (iCP0xx(i).eq.iCP0CP)call CP0CPX (i,T,Cp0i,CpI,CpT,Cp3)    !Polynomial fit
            if (iCP0xx(i).eq.iCP0PH)Cp0i=Rxgas*(1d0-PH0PH0(i,2,0,T,0d0))  !Helmholtz form (fundamental equation)
            Cp0sum=Cp0sum+z(i)*Cp0i
          endif
        enddo
        CP0=Cp0sum
      endif

      end                                                   function CP0
c
c ======================================================================
c
      function CP0K (icomp,T)
c
c  Calculate Cp0 for a pure fluid given temperature.
c  This is only called by the transport routines.
c
      include 'COMMONS.INC'
      dimension z(ncmax)
      z(1:nc)=0d0
      z(icomp)=1d0
      CP0K=CP0(T,z)
      end                                                  function CP0K
c
c ======================================================================
c
      function PHI0 (itau,idel,T,D,z)
c
c  Compute the ideal-gas part of the reduced Helmholtz energy or its
c  derivatives as functions of temperature and density for a mixture.
c
c  While the real-gas part of the Helmholtz energy is calculated
c  in terms of dimensionless temperature and density, the ideal-
c  gas part is calculated in terms of absolute temperature and
c  density.  (This distinction is necessary for mixtures.)
c
c  The Helmholtz energy consists of ideal-gas and residual
c  (real-gas) terms; this routine calculates only the ideal part.
c
c  Inputs:
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           (The density derivatives are not used in the calculation
c           of any property.)
c           when itau = 0 and idel = 0, compute A0/RT
c           when itau = 1 and idel = 0, compute 1st temperature derivative
c           when itau = 2 and idel = 0, compute 2nd temperature derivative
c           when itau = 0 and idel = 1, compute 1st density derivative
c           (actually the derivatives are with respect to the dimensionless
c           quantities tau and del)
c        T--Temperature [K]
c        D--Density [mol/L]
c        z--Composition array (array of mole fractions)
c
c  Output (as the function value):
c    phi00--Ideal-gas part of the reduced Helmholtz energy (A/RT);
c           derivatives (as specified by itau and idel) are multiplied
c           by the corresponding power of tau or del; i.e.,
c           when itau = 1, the quantity returned is tau*[d(PHI0)/d(tau)]
c           when itau = 2, the quantity returned is tau^2*[d^2(PHI0)/d(tau)^2]
c           when itau = 3, the quantity returned is tau^3*d^3(ph0cpp)/d(tau)^3
c           where tau=Tc/T and del=D/Dc are evaluated for each component.
c           Similarly, the del derivatives (as specified by idel) are
c           multiplied by the corresponding power of del (the derivatives
c           usually appear with this factor and this approach neatly
c           avoids a possible divide by zero).
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  08-04-95  MM, original version
c  06-20-16 EWL, add call to RMIX so that it can be removed in all other routines calling this

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PHI0

      PHI0=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      call ISPURE (z,i)
      if (.not.lRcnst .or. .not.lRcnstWag) call RMIX (z)
      if (i.ne.0) then
        if (iCP0xx(i).eq.iCP0CP) PHI0=PH0CPP(i,itau,idel,T,D)             !Polynomial fit of isobaric heat capacity for the ideal-gas state
        if (iCP0xx(i).eq.iCP0PH) PHI0=PH0PH0(i,itau,idel,T,D)             !Helmholtz form (fundamental equation)
      else
        phisum=0d0
        do i=1,nc
          if (z(i).gt.1d-10 .and. z(i).le.1.0000000001d0) then            !Compute only if component composition >0 and <=1.
            PHI0i=0d0
            if (iCP0xx(i).eq.iCP0CP) PHI0i=PH0CPP(i,itau,idel,T,D)        !Polynomial fit of isobaric heat capacity for the ideal-gas state
            if (iCP0xx(i).eq.iCP0PH) PHI0i=PH0PH0(i,itau,idel,T,D)        !Helmholtz form (fundamental equation)
            phisum=phisum+z(i)*PHI0i
            if (itau.eq.0.and.idel.eq.0) phisum=phisum+z(i)*XLOG(z(i))    !Entropy of mixing
          endif
        enddo
        PHI0=phisum
      endif
      end                                                  function PHI0
c
c ======================================================================
c
      function PHI0K (icomp,itau,idel,T,D)
c
c  Compute the ideal-gas part or derivatives of the reduced Helmholtz
c  energy for a pure component.
c
      include 'COMMONS.INC'
      dimension z(ncmax)
      z(1:nc)=0
      z(icomp)=1
      PHI0K=PHI0(itau,idel,T,D,z)
      end                                                 function PHI0K
c
c ======================================================================
c
      function PH0CPP (icomp,itau,idel,T,D)
c
c  Compute the ideal-gas part of the reduced Helmholtz energy or its
c  derivatives as functions of temperature and density for the
c  component specified by icomp.
c
c  See function PHI0 for the description of all variables and additional
c  information.
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  08-04-95  MM, original version
c  08-16-07 NRN, add 3rd derivatives for COSH and SINH (Ryan Nannan, Delft University)
c  01-14-15 EWL, add check for high ttau for itau=3

      include 'COMMONS.INC'
      include 'COMFEQ.INC'

      PH0CPP=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      j=nCp0Poly(i)+nCp0Exp(i)+nCp0Cosh(i)+nCp0Sinh(i)
      if (j.eq.0) RETURN

c  Use values from previous call (values already in Cp0Sav, etc.).
c  Otherwise, compute new values and save for possible future use.
      R=Rxgas
      RT=R*T
      if (ABS(T-TsvCp0(i)).gt.1.d-12) then
        call CP0CPX(i,T,Cp0Sav(i),CpiSav(i),CptSav(i),Cp3Sav(i))
        if (lcache2) TsvCp0(i)=T
      endif


c  The three density derivatives are 1/del, -1/del^2, and 1/del^3.
c  The quantity returned is the derivative multiplied by the corresponding
c   power of del, avoiding a possible divide by zero.
      if (idel.gt.0) then
        PH0CPP=1d0      !idel=1
        if (idel.eq.2) PH0CPP=-1d0
        if (idel.eq.3) PH0CPP= 2d0
        if (itau.ne.0) PH0CPP= 0d0     !All cross derivatives are zero.
        RETURN
      endif

c  Compute reduced Helmholtz energy.
      if (itau.eq.0) then
        PH0CPP=xinf
        if (D.gt.0.) then
          xl=LOG(T*D/(Tref(i)*Dref(i)))
          PH0CPP=(CpiSav(i)-hRef(i))/RT - (CptSav(i)-sRef(i))/R-1d0 + xl
        endif

c  Compute derivative w.r.t. tau (dimensionless temperature).
      elseif (itau.eq.1) then
        PH0CPP=(CpiSav(i)-hRef(i))/RT-1d0   !Return tau*d(ph0cpp)/d(tau)

c  Compute 2nd derivative w. r.t. tau (dimensionless temperature).
      elseif (itau.eq.2) then
        PH0CPP=1d0-Cp0Sav(i)/R    !Return tau^2*d2(ph0cpp)/d(tau)^2.

c  Compute 3rd derivative w.r.t. tau (dimensionless temperature).
      elseif (itau.eq.3) then
        PH0CPP=-2d0+2d0*Cp0Sav(i)/R+Cp3Sav(i)/R

      endif

      end                                                function PH0CPP
c
c ======================================================================
c
      subroutine CP0CPX (icomp,T,CpP,CpI,CpT,Cp3)
c
c  Compute the following:
c
c  1) Cp0 as a function of temperature.
c
c  2) The integral of Cp0 over the limits of Tref to T for use
c     in enthalpy calculations.  Based on derivations in Younglove and
c     McLinden, J. Phys. Chem. Ref. Data, 23:731-779, 1994, equation C11.
c
c  3) The integral of Cp0/T over the limits of Tref to T for use in entropy
c     calculations.  Also based on derivations in Younglove and McLinden.
c
c  Cp0 is calculated with a polynomial correlation given by
c  Cp0/Cred = SUM {aCp0(k,i)*[T/Tred(i)]^TiCp0(k,i)}
c           + SUM {aCp0(k,i)*u(k,i)^2*EXP{u(k,i)/{1-EXP[u(k,i)]}^2}
c
c  where
c
c  aCp0(k,i) and TiCp0(k,i) are the k-th coefficients for component i
c  u(k,i) = TiCp0(k,i)/T
c  Cred is the reducing parameter for Cp0 (e.g., the gas constant R)
c  Tred is the reducing parameter for T (e.g., the critical temperature)
c  (The reducing parameters are specified in the *.fld file.)
c
c  Inputs:
c    icomp--Pointer specifying the component number (1..nc)
c        T--Temperature [K]
c
c  Output (as the function value):
c   CpP--Cp0 [J/(mol-K)]
c   CpI--Integral of Cp0.dT from T to Tref [J/mol]
c   CpT--Integral of Cp0/T.dT from T to Tref [J/(mol-K)]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  10-06-94  MM, original version
c  06-13-16 EWL, shorten code for all routines in CORE_CPP.FOR

      include 'COMMONS.INC'
      include 'COMFEQ.INC'

      CpP=0d0
      CpI=0d0
      CpT=0d0
      Cp3=0d0
      if (T.le.0. .or. T.gt.1.d20 .or. icomp.eq.0) RETURN

      n=0
      i=icomp
      tt=T/TredCp0(i)

c  Polynomial terms
      if (nCp0poly(i).ge.1) then
        do j=1,nCp0poly(i)
          n=n+1
          a=aCp0(n,i)
          Ti=TiCp0(n,i)
          xk1=Ti+1d0
          Trd=TredCp0(i)**Ti
          CpP=CpP+a*tt**Ti
          Cp3=Cp3+a*Ti*tt**Ti
          if (xk1.eq.0.) then
            CpI=CpI+a*TredCp0(i)*LOG(T/Tref(i))  !Any term with temperature exponent of -1 is handled differently.
          else
            CpI=CpI+a*(T**xk1-Tref(i)**xk1)/(xk1*Trd)
          endif
          if (Ti.eq.0.) then
            CpT=CpT+a*LOG(T/Tref(i))   !Any term with a temperature exponent of 0 is handled differently.
          else
            CpT=CpT+a*(T**Ti-Tref(i)**Ti)/(Ti*Trd)
          endif
        enddo
      endif

c  Exponential terms
      if (nCp0Exp(i).ge.1) then
        do j=1,nCp0Exp(i)
          n=n+1
          Ti=TiCp0(n,i)
          ui=Ti/T
          if (ui.ne.0. .and. ui.lt.200.) then
            a=aCp0(n,i)
            uir=Ti/Tref(i)
            expui=EXP(ui)
            exptr=EXP(uir)
            expu1=1d0-expui
            expt1=1d0-exptr
            expui2=expui/expu1**2
            CpP=CpP+a*expui2*ui**2
            Cp3=Cp3-a*Ti*expui2*ui*(2d0+ui*(1d0+2d0*expui2*expu1))/T
            CpI=CpI-0.5d0*a*Ti*((1d0+expui)/expu1-(1d0+exptr)/expt1)
            CpT=CpT+a*(LOG(expt1/expu1)-ui*expui/expu1+uir*exptr/expt1)
          endif
        enddo
      endif

c  COSH and SINH terms
      if (nCp0Cosh(i)+nCp0Sinh(i).ge.1) then
        do j=1,nCp0Cosh(i)+nCp0Sinh(i)
          n=n+1
          Ti=TiCp0(n,i)
          Tih=TiCp0h(n,i)
          ah=aCp0h(n,i)
          htau=ah*tt**Tih
          ttau=aCp0(n,i)*tt**Ti
          xtau=ah/T
          rtau=ah/Tref(i)
          cs=csCp0h(n,i)
          if (htau.gt.700.) htau=700d0
          if (xtau.gt.700.) xtau=700d0
          x=aCp0(n,i)/ah
          xtanh=TANH(xtau)
          rtanh=TANH(rtau)
          if (j.le.nCp0Cosh(i)) then
            cshcs=COSH(htau)**cs
            CpP=CpP+ttau*cshcs
            CpI=CpI-x*(xtanh-rtanh)
            CpT=CpT+x*(rtanh/Tref(i)-xtanh/T+
     &                  LOG(COSH(xtau)/COSH(rtau))/ah)
            Cp3=Cp3+ttau*cshcs*(Ti+cs*Tih*htau*TANH(htau))

          else
            cshcs=SINH(htau)**cs
            CpP=CpP+ttau*cshcs
            CpI=CpI+x*(1d0/xtanh-1d0/rtanh)
            CpT=CpT+x*(1d0/(xtanh*T)-1d0/(rtanh*Tref(i))+
     &                  LOG(SINH(rtau)/SINH(xtau))/ah)
            Cp3=Cp3+ttau*cshcs*(Ti+cs*Tih*htau/TANH(htau))

          endif
        enddo
      endif

      CpP=CpP*credCp0(i)
      CpI=CpI*credCp0(i)
      CpT=CpT*credCp0(i)
      Cp3=Cp3*credCp0(i)

      end                                              subroutine CP0CPX
c
c ======================================================================
c
      function PH0PH0 (icomp,itau,idel,T,D)
c
c  Compute the ideal-gas part of the reduced Helmholtz energy or one of its
c  derivatives as a function of temperature and density.
c  (See function PHI0 for the description of all inputs and outputs.)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  05-13-96  MM, original version, based (loosely) on PH0CPP
c  06-08-16 EWL, shorten and remove old code no longer in use

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*255 herr

      ierr=0
      herr=' '
      PH0PH0=0d0
      if (icomp.le.0) RETURN
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      j=nPhiLog(i)+nPhiTau(i)+nPhiExp(i)+nPhiCosh(i)+nPhiSinh(i)
      if (j.eq.0) RETURN

      if (idel.gt.0) then
        PH0PH0=1d0      !idel=1
        if (idel.eq.2) PH0PH0=-1d0
        if (idel.eq.3) PH0PH0= 2d0
        if (itau.ne.0) PH0PH0= 0d0     !All cross derivatives are zero.
        RETURN
      endif

c  Compute reduced Helmholtz energy; first check if the input T is the same as the previous call.
      if (ABS(T-Tsvph0(i)).lt.1d-16 .and.
     &    ABS(D-Dsvph0(i)).lt.1d-16) then
        if (itau.eq.0) PH0PH0=PH0sav(i)
        if (itau.eq.1) PH0PH0=PH1sav(i)
        if (itau.eq.2) PH0PH0=PH2sav(i)
        if (itau.eq.3) PH0PH0=PH3sav(i)
        RETURN
      endif

c  Sum terms of the form [ai*LOG(tau^ti)].
      n=1
      tau=Tzred(i)/T
      tau1=1d0/tau
      ph0=0d0
      ph1=0d0
      ph2=0d0
      ph3=0d0
      if (nPhiLog(i).ge.1) then
        do j=1,nPhiLog(i)
          a=aPhi0(n,i)
          Ti=TiPhi0(n,i)
          ph0=ph0+a*LOG(tau**Ti)
          ph1=ph1+a*Ti*tau1
          ph2=ph2-a*Ti*tau1**2
          ph3=ph3+a*Ti*tau1**3*2d0
          n=n+1
        enddo
      endif

c  Sum terms of the form [ai*tau^ti].
      if (nPhiTau(i).ge.1) then
        do j=1,nPhiTau(i)
          a=aPhi0(n,i)
          Ti=TiPhi0(n,i)
          ph0=ph0+a*tau**Ti
          ph1=ph1+a*Ti*tau**(Ti-1d0)
          ph2=ph2+a*Ti*(Ti-1d0)*tau**(Ti-2d0)
          ph3=ph3+a*Ti*(Ti-1d0)*(Ti-2d0)*tau**(Ti-3d0)
          n=n+1
        enddo
      endif

c  Sum terms of the form [ai*LOG(1-EXP(bi*tau))].
      if (nPhiExp(i).ge.1) then
        do j=1,nPhiExp(i)
          a=aPhi0(n,i)
          Ti=TiPhi0(n,i)
          Ti2=Ti*Ti
          exptau=EXP(Ti*tau)
          exptau1=1d0/(1d0-exptau)
          exptau2=exptau*exptau1
          exptau3=a*Ti2*exptau1*exptau2
          ph0=ph0+a*LOG(1d0-exptau)
          ph1=ph1-a*Ti*exptau2
          ph2=ph2-exptau3
          ph3=ph3-Ti*exptau3*(1d0+2d0*exptau2)
          n=n+1
        enddo
      endif

c  Sum terms of the form [ai*LOG(COSH(bi*tau))] and [ai*LOG(SINH(bi*tau))].
      if (nPhiCosh(i)+nPhiSinh(i).ge.1) then
        do j=1,nPhiCosh(i)+nPhiSinh(i)
          a=aPhi0(n,i)
          Ti=TiPhi0(n,i)
          ttau=Ti*tau
          x=a*Ti**2
          y=2d0*x*Ti
          ttanh=TANH(ttau)
          tcosh=COSH(ttau)
          tsinh=SINH(ttau)
          if (ABS(ttau).lt.500.) then
            if (j.le.nPhiCosh(i)) then
              tcosh3=1d0/tcosh**3
              ph0=ph0+a*XLOG(tcosh)
              ph1=ph1+a*Ti*ttanh
              ph2=ph2+x*tcosh3*tcosh
              ph3=ph3-y*tsinh*tcosh3
            else
              tsinh3=1d0/tsinh**3
              ph0=ph0+a*XLOG(tsinh)
              ph1=ph1+a*Ti/ttanh
              ph2=ph2-x*tsinh3*tsinh
              ph3=ph3+y*tcosh*tsinh3
            endif
          endif
          n=n+1
        enddo
      endif

c  Save information for possible use on next call to function.
      Ds=D
      if (Ds.lt.1.d-40) Ds=1.d-40
      PH0sav(i)=ph0+LOG(Ds*Vzred(i))
      PH1sav(i)=ph1*tau      !Return tau*d(ph0ph0)/d(tau).
      PH2sav(i)=ph2*tau**2   !Return tau^2*d^2(ph0ph0)/d(tau)^2.
      PH3sav(i)=ph3*tau**3
      if (lcache2) Dsvph0(i)=D
      if (lcache2) Tsvph0(i)=T
      if (itau.eq.0) PH0PH0=PH0sav(i)
      if (itau.eq.1) PH0PH0=PH1sav(i)
      if (itau.eq.2) PH0PH0=PH2sav(i)
      if (itau.eq.3) PH0PH0=PH3sav(i)
      end                                                function PH0PH0
c
c ======================================================================
c
      subroutine SETCPP (nread,icomp,iFlg,ierr,herr)
c
c  For iFlg=1:
c  Set up working arrays for the polynomial form of the ideal heat gas capacity.
c
c  For iFlg=2:
c  Set up working arrays for the ideal-gas part of the Helmholtz energy.
c  Implements an expression of the form
c    phi0 = Sum{ai*LOG(tau^ti)} + Sum{aj*tau^tj}
c         + Sum{ak*LOG[1-EXP(bk*tau)]}
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in the mixture (1..nc); 1 for pure fluid.
c           Numbers above ncmax designate ECS reference fluids.
c     iFlg--See above
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  07-20-95  MM, original version
c
      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character herr*255

c  Commons associated with the mxcpp fluids with FEQ equations stored in
c   block data BDCPP.
c     character jIDc*12
c     parameter (mxcpp=2,mxph0=2)      !Maximum number of fluids in block data for Cp0 and PH0
c     common /CPBLOCK/ cpca(mxcpp,ncppmx),xka(mxcpp,ncppmx),
c    &       treda(mxcpp),Creda(mxcpp),ntrmca(mxcpp),ntrmce(mxcpp),
c    &       jIDc(mxcpp)

      ierr=0
      herr=' '
      i=icomp

c  Initialize common block entries when a new fluid is read in.
      Cp0Sav=0d0
      CpiSav=0d0
      CptSav=0d0
      TsvCp0=0d0
      PH0sav=0d0
      PH1sav=0d0
      PH2sav=0d0
      Tsvph0=0d0
      Dsvph0=0d0

c  Get coefficients from block data.
c  The fluid is identified with the fluid ID number.
c     if (nread.le.0) then
c       do j=1,mxcpp
c         if (jFluidID.eq.jIDc(j)) then
c           TredCp0(i)=treda(j)
c           credCp0(i)=Creda(j)
c           nCp0poly(i)=ntrmca(j)
c           nCp0Exp(i)=ntrmce(j)
c           nCp0Cosh(i)=0
c           nCp0Sinh(i)=0
c           aCp0(i,1:ntrmca(j))=cpca(j,1:ntrmca(j))
c           TiCp0(i,1:ntrmca(j))=xka(j,1:ntrmca(j))
c           RETURN
c         endif
c       enddo
c       RETURN
c     endif

c  Read data from file.
      if (iFlg.ne.3) then
        read (nread,*,err=102)    !Lower temperature limit  (dummy values used for Cp0 equations)
        read (nread,*,err=102)    !Upper temperature limit
        read (nread,*,err=102)    !Upper pressure limit
        read (nread,*,err=102)    !Upper density limit
      endif

c  Read Cp0 equations
      if (iFlg.eq.1) then
        read (nread,*,err=102) TredCp0(i),RCp0Org(i)!Reducing parameters
        nCp0Cosh(i)=0   !These terms not used in files prior to v6.100.
        nCp0Sinh(i)=0
        read (nread,*,err=1) nCp0poly(i),nCp0Exp(i),nCp0Cosh(i),
     &                       nCp0Sinh(i)    !Read number of terms.
 1      do k=1,nCp0poly(i)+nCp0Exp(i)       !Read polynomial and exponential coefficients.
          read (nread,*,err=102) aCp0(k,i),TiCp0(k,i)
        enddo
        do k=1,nCp0Cosh(i)+nCp0Sinh(i)      !Read COSH and SINH coefficients.
          j=nCp0poly(i)+nCp0Exp(i)+k
          read (nread,*,err=102) aCp0(j,i),TiCp0(j,i),aCp0h(j,i),
     &                   TiCp0h(j,i),csCp0h(j,i)
        enddo

c  Check for Wagner equations of state.
c   If the fluid being setup is not one of the fluids listed below, use a "constant" gas constant.
c   Otherwise the value of R will be that reported in the Wagner equation of state.
      Rdiff=ABS(RCp0Org(i)-xRgas)*1d6
      if (Rdiff.gt.5000) then     !If the difference between the reducing value and the gas constant is more than 5000 ppm, then the reducing value is not R.
        RxCp0(i)  =RCp0Org(i)
        Rcp0Def(i)=RCp0Org(i)
        credCp0(i)=RCp0Org(i)
      else
        RxCp0(i)  =xRgas
        Rcp0Def(i)=xRgas
        if (lWagner .and. .not.lRcnstWag) Rcp0Def(i)=RCp0Org(i) !Use Wagner gas constant.
        credCp0(i)=Rcp0Def(i)
      endif

c  Read Phi0 equations
      else
        nPhiCosh(i)=0   !These terms not used in files prior to v8.0.
        nPhiSinh(i)=0
        read (nread,*,err=2) nPhiLog(i),nPhiTau(i),nPhiExp(i),  !Read number of  terms
     &                       nPhiCosh(i),nPhiSinh(i)
 2      j=nPhiLog(i)+nPhiTau(i)
        jterm=j+nPhiExp(i)+nPhiCosh(i)+nPhiSinh(i)

c  Read coefficients for terms of the form [ai*LOG(tau^ti)], [ai*tau^ti], and {ai*LOG[1-EXP(bi*tau)]}
        do k=1,jterm
          read (nread,*,err=102) aPhi0(k,i),TiPhi0(k,i)
        enddo

c  The equations labeled as PX contain coefficients that are identical to the Cp0
c   equations, and need to be reduced by Tc to be compatible with the PH equations.
        if (iFlg.eq.3 .and. (nPhiExp(i).gt.0 .or. nPhiTau(i).gt.2)) then
          do k=nPhiLog(i)+1,j         !Assume that each PX0 equation has one log term and two Tau terms for href and sref.
            if (TiPhi0(k,i).ne.0. .and. TiPhi0(k,i).ne.1.) then
              pj=TiPhi0(k,i)*(TiPhi0(k,i)-1d0)
              aPhi0(k,i)=-aPhi0(k,i)/Tcrit(i)**TiPhi0(k,i)/pj
            endif
          enddo
          do k=j+1,j+nPhiExp(i)
            TiPhi0(k,i)=-TiPhi0(k,i)/Tcrit(i)
          enddo
        endif
      endif

      if (lGERG08) then  !Apply Cp0 correction as explained in the GERG-2008 documentation.
        do j=1,nCp0Poly(i)+nCp0Exp(i)+nCp0Cosh(i)+nCp0Sinh(i)
          aCp0(j,i)=8.31451d0/8.314472d0*aCp0(j,i)
        enddo
        do j=1,nPhiLog(i)+nPhiTau(i)+nPhiExp(i)+nPhiCosh(i)+nPhiSinh(i)
          aPhi0(j,i)=8.31451d0/8.314472d0*aPhi0(j,i)
        enddo
      endif

      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                              subroutine SETCPP
cc
cc ======================================================================
cc
c      block data BDCPP
cc
cc  Data for the polynomial form of the ideal-gas heat capacity
cc
c      include 'COMMONS.INC'
c      parameter (mxcpp=2,mxph0=2)     !Maximum number of fluids in block data for Cp0 and PH0
c      common /CPBLOCK/ cpca(mxcpp,ncppmx),xka(mxcpp,ncppmx),
c     &       treda(mxcpp),Creda(mxcpp),ntrmca(mxcpp),ntrmce(mxcpp),
c     &       jIDc(mxcpp)
cc
cc  Explanation of variables in common block.
cc    ntrmca,ntrmce - Number of terms in polynomial fit
cc    treda,Creda   - Reducing parameters for temperature, Cp0 (often =1)
cc    cpca,xka      - Parameters of polynomial Cp0 fit
cc    jIDc          - ID number for fluid corresponding to equation "i"
cc
cc  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
cc  07-20-95  MM, original version
cc
cc  R134a  1,1,1,2-tetrafluoroethane
cc  fit of McLinden et al. (1989), ASHRAE Trans 95(pt 2):263-283
cc  also in Huber and McLinden, Int. Refrig. Conf., Purdue, 453-462, 1992.
c      data jIDc(1)/1/
c      data ntrmca(1),ntrmce(1)/3,0/
c      data treda(1),Creda(1)/1d0,1d0/
c      data (xka(1,i),i=1,3)/0d0,1d0,2d0/
c      data (cpca(1,i),i=1,3)/19.4006d0,0.258531d0,-1.29665d-4/
cc
cc  R123  2,2-dichloro-1,1,1-trifluoroethane
cc  fit of Younglove and McLinden, J. Phys. Chem. Ref. Data, 23:731-779, 1994.
c      data jIDc(2)/2/
c      data ntrmca(2),ntrmce(2)/4,0/
c      data treda(2),Creda(2) /456.831d0,8.31451d0/
c      data (xka(2,i),i=1,4)/0d0,1d0,2d0,3d0/
c      data (cpca(2,i),i=1,4)/2.046009d0,22.231991d0,-11.658491d0,2.691665/
cc
c      end                                               block data BDCPP
c
c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                  End file CORE_CPP.FOR
c ======================================================================
