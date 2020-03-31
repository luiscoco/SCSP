c  Begin file CORE_PR.FOR
c
c  This file contains the routines implementing the Peng-Robinson and
c    other similar cubic equations of state.
c
c  Contained here are:
c     function PHIPR     (icomp,itau,idel,tau,del)
c     function PHIPRX    (itau,idel,tau,del,z)
c     subroutine REDPR   (z,Tred,Dred)
c     subroutine SETPR   (mode,nread,icomp,ierr,herr)
c     subroutine SETPRCO (i)
c     subroutine PREOS   (i)
c     subroutine TPRHOPR (T,P,z,D1,D2)
c     subroutine ESTPR   (i,j,ibin,fPRkijx,ierr,herr)
c     subroutine PHIDERVPR (iderv,t,rho,z,dadn,dnadn,ierr,herr)
c
c ======================================================================
c ======================================================================
c
      function PHIPR (icomp,itau,idel,tau,del)
c
c  Compute the reduced Helmholtz energy or one of its derivatives as
c  functions of the dimensionless temperature and density for the
c  Peng-Robinson or SRK equations of state. See function PHIPRX for
c  other information.

      include 'COMMONS.INC'
      dimension z(ncmax)
      z(1:nc)=0d0
      z(icomp)=1d0
      phipr=PHIPRX(itau,idel,tau,del,z)
      end                                                 function PHIPR
c
c ======================================================================
c
      function PHIPRX (itau,idel,tau,del,z)
c
c  Compute the reduced Helmholtz energy or one of its derivatives as
c  functions of the dimensionless temperature and density for a
c  volume-translated modification of the Peng-Robinson equation of state:
c     P=RT/(v+T-b)-a/((v+T)*(v+T+b)+b(v+T-b))
c  The original translation concept is given in Peneloux and Rauzy,
c  Fluid Phase Equilib. 8:7-23, 1982 (although it was applied to SRK).
c  The translation parameter is considered a constant (see Pfohl,
c  Fluid Phase Equilib. 163:157-159, 1999) to avoid crossing of isotherms
c  at high densities or near critical conditions.  If translation is set
c  to zero, the original Peng Robinson equation is recovered.
c
c  Inputs:
c    icomp--Pointer specifying component (1..nc)
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           When itau = 0 and idel = 0, compute A/RT
c           When itau = 0 and idel = 1, compute 1st density derivative
c           When itau = 1 and idel = 1, compute cross derivative
c           etc.
c      tau--Dimensionless temperature (Tc/T)
c      del--Dimensionless density (D/Dc)
c  Output (as the function value):
c      phi--Residual (real-gas) part of the Helmholtz energy, or one
c           of its derivatives (as specified by itau and idel),
c           in reduced form (A/RT)
c           itau  idel   Output (dimensionless for all cases)
c             0    0      A/RT
c             1    0      tau*[d(A/RT)/d(tau)]
c             2    0      tau^2*[d^2(A/RT)/d(tau)^2]
c             0    1      del*[d(A/RT)/d(del)]
c             0    2      del^2*[d^2(A/RT)/d(del)^2]
c             0    3      del^3*[d^3(A/RT)/d(del)^3]
c             1    1      tau*del*[d^2(A/RT)/d(tau)d(del)]
c                         etc.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-11-04 EWL, original version
c  08-07-13 EWL, add PR3

      include 'COMMONS.INC'
      double precision z(ncmax),ai(ncmax),bi(ncmax),ki(ncmax)
      dimension atci(ncmax),daidt(ncmax),d2aidt2(ncmax),aij(ncmax,ncmax)

      PHIPRX=0d0
      if (del.le.1d-10) RETURN    !Trivial solution at zero density
      if (tau.le.0.) RETURN       ! for any and all derivatives.

      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        Tred=Tzred(icomp)
        Dred=Dzred(icomp)
      else
        call REDX (z,Tred,Dred)
      endif
      T=Tred/tau
      D=Dred*del
      v=XDIV(1d0,D)

c  Set volume translation factor.
      Tshift=0d0
      if (iEOSmx.ne.iEOSPR3) then
        if (icomp.ne.0) Tshift=PRcoef(1,icomp)
        if (icomp.eq.0) Tshift=SUM(z(1:nc)*PRcoef(1,1:nc))
      endif
      RT=Rxgas*T
c     u=1d0   !Parameters for the SRK equation.
c     w=0d0
      u=2d0   !Parameters for the Peng-Robinson equation.
      w=-1d0

      do i=1,nc
        if (z(i).gt.0) then
          bi(i)=XDIV(0.0778d0*Rxgas*TcPR(i),PcPR(i))
          ki(i)=0.37464d0+1.54226d0*acnPR(i)-0.26992d0*acnPR(i)**2
          atci(i)=0.45724d0*Rxgas**2*XDIV(TcPR(i)**2,PcPR(i))
          ai(i)=atci(i)*(1d0+ki(i)*(1d0-SQRT(XDIV(T,TcPR(i)))))**2
        endif
      enddo

      if (icomp.ne.0) then
        a=ai(icomp)
        b=bi(icomp)
      else
        a=0d0
        b=0d0
        do i=1,nc
          do j=1,nc
            if (z(i).gt.0 .and. z(j).gt.0) then
              aij(i,j)=SQRT(ai(i)*ai(j))*(1d0-fPRkij(1,i,j))
              a=a+z(i)*z(j)*aij(i,j)
            endif
          enddo
          b=b+z(i)*bi(i)
        enddo
      endif
      if (ABS(a).gt.1.d5) RETURN  !Check for errors that may have been caused by the setup routines.

      vb=v-b
      q=b*SQRT(u**2-4d0*w)
      v1=2d0*v+u*b+q
      v2=2d0*v+u*b-q
      v1n=v1+2d0*Tshift
      v2n=v2+2d0*Tshift
      dts=D*Tshift
      b2=b**2
      bu=b*u
      b2dw=b2*D*w
      arg1=vb+Tshift
      arg2=v2n/v1n
      if (arg1.lt.1d-9) arg1=1d-9 !Prevent negative log arguments.
      if (arg2.lt.1d-9) arg2=1d-9
      if (   v.lt.1d-9)    v=1d-9
      if (   q.lt.1d-9)    q=1d-9
      t1=D*(b-Tshift)/(1d0-b*D+dts)
      T2=((1d0+dts)*(1d0+D*(Tshift+bu))+b2dw*D)/D

c  Check if derivatives are requested.
      if (itau.eq.0 .and. idel.eq.0) then
        PHIPRX=XLOG(v)-XLOG(arg1)+a/RT*XLOG(arg2)/q

c     elseif (itau.eq.-1) then
c       do i=1,nc
c         daidt(i)=-atci(i)*ki(i)/SQRT(T*TcPR(i))*
c    &             (1d0+ki(i)*(1d0-SQRT(T/TcPR(i))))
c       enddo
c       dadt=0d0
c       do i=1,nc
c         do j=1,nc
c           da=SQRT(ai(i)/ai(j))*daidt(j)/2d0+
c    &         SQRT(ai(j)/ai(i))*daidt(i)/2d0
c           dadt=dadt+z(i)*z(j)*(1d0-fPRkij(1,i,j))*da
c         enddo
c       enddo
c
c       vb=v-b
c       vpb=v+b
c       vbb=v*vpb+b*vb
c       vb2=vbb**2
c       sq2=SQRT(2d0)
c       sq2p=1d0+sq2
c       sq2m=1d0-sq2
c       bsq2=2d0*b*sq2*Rxgas*T
c       v2bp=v+sq2p*b
c       v2bm=v+sq2m*b
c       vx=sq2m/v2bm
c       vy=sq2p/v2bp
c       arg=v2bm/v2bp
c       argl=XLOG(arg)
c       dpdv=-RT/vb**2+2d0*a*vpb/vb2
c       dPdT=Rxgas/vb-dadt/vbb
c       P=D*RT*(1d0+t1-a/RT/T2)

c       do i=1,nc
c         bip=bi(i)+b
c         bim=bi(i)-b
c         suma=0d0
c         saa=0d0
c         do j=1,nc
c           da=0
c           if (ai(i).ne.0 .and. ai(j).ne.0)
c    &      da=SQRT(ai(i)/ai(j))*daidt(j)/2d0+
c    &         SQRT(ai(j)/ai(i))*daidt(i)/2d0
c           saa=saa+z(j)*(1d0-fPRkij(1,i,j))*da
c           suma=suma+z(j)*aij(i,j)
c         enddo
c         daddn(i)=XLOG(v/vb)+bi(i)/vb+     !Store dnadn temporarily in daddn d(nar)/d(ni) eq. 13.
c    &             ((a-a*bip/b+2d0*suma)*argl+a*bi(i)*(vx-vy))/bsq2
c         d2adbn(i)=1d0/bsq2*(dadt-a/T)*argl+
c    &         argl/bsq2/b*(a*bip/T-2d0*b/T*suma+2d0*b*saa-dadt*bip)+
c    &         bi(i)/bsq2*(dadt-a/T)*(vx-vy)
c         dpdn(i)=RT*(v+bim)/vb**2+2d0*a*bi(i)*vb/vb2-2d0*suma/vbb
c         vrt=-dpdn(i)/dpdv/RT
c         dphidT(i)=d2adbn(i)+1d0/T-vrt*dPdT
c         dphidP(i)=vrt-1d0/P
c       enddo
c       do i=1,nc
c         do j=1,nc
c           vs=0d0
c           do k=1,nc
c             vs=vs+z(k)*(bi(j)*aij(i,k)+bi(i)*aij(j,k))
c           enddo
c           bij=bi(i)*bi(j)
c           d2adnn(i,j)=(bi(i)+bi(j))/vb+bij/vb**2+
c    &         2d0/bsq2*(argl*(a*bij/b**2+aij(i,j)-vs/b)-
c    &         (vx-vy)*(a*bij/b-vs)+(vy**2-vx**2)*a*bij/2d0)
c           dphidnj(i,j)=d2adnn(i,j)+1d0+dpdn(j)*dpdn(i)/dpdv/RT
c         enddo
c       enddo
c       PHIPRX=-1d0

c  Compute cross derivative with terms from 1st derivatives.
      elseif (itau.eq.1 .and. idel.eq.1) then
        do i=1,nc
          if (z(i).gt.0) then
            daidt(i)=-atci(i)*ki(i)/SQRT(T*TcPR(i))*
     &             (1d0+ki(i)*(1d0-SQRT(T/TcPR(i))))
          endif
        enddo
        if (icomp.ne.0) then
          dadt=daidt(icomp)
        else
          dadt=0d0
          do i=1,nc
          do j=1,nc
            if (z(i).gt.0 .and. z(j).gt.0) then
              da=SQRT(ai(i)/ai(j))*daidt(j)/2d0+
     &           SQRT(ai(j)/ai(i))*daidt(i)/2d0
              dadt=dadt+z(i)*z(j)*(1d0-fPRkij(1,i,j))*da
            endif
          enddo
          enddo
        endif
        trm=2d0*(1d0+b*D+D*Tshift)
        PHIPRX=4d0*D/Rxgas*(dadt-a/T)/(trm-D*q)/(trm+D*q)

c  Compute cross derivatives
      elseif (itau.eq.2 .and. idel.eq.1) then
        PHIPRX=0d0           !Not yet implemented

      elseif (itau.eq.1 .and. idel.eq.2) then
        PHIPRX=0d0           !Not yet implemented

      elseif (itau.eq.2 .and. idel.eq.2) then
        PHIPRX=0d0           !Not yet implemented

c  Compute derivative w.r.t. del (dimensionless density).
      elseif (idel.eq.1) then
        PHIPRX=t1-XDIV(a,RT*T2)
c-------Alternative method
c       PHIPRX=-1d0+1d0/(1d0-b*D+D*Tshift)+
c    &          a/RT/q*(2d0/v1n-2d0/v2n)/D

c  Compute 2nd derivative w.r.t. del (dimensionless density).
      elseif (idel.eq.2) then
        PHIPRX=t1**2+XDIV(a,RT*T2**2)*
     &         (bu+2d0*(Tshift+dts*Tshift+b*dts*u+b2dw))
c-------Alternative method
c       bdt=1d0-b*D+D*Tshift
c       PHIPRX=1d0-1d0/bdt-(Tshift*dred-b*dred)*del/bdt**2+
c    &         4d0*a/RT/q/D*((1d0/v2n-1d0/v1n)+
c    &         (1d0/v1n**2-1d0/v2n**2)/D)

c  Compute 3rd derivative w.r.t. del (dimensionless density).
      elseif (idel.eq.3) then
        PHIPRX=2d0*t1**3+2d0*D*XDIV(a,RT*T2**2)*
     &         (Tshift**2+b*Tshift*u+b2*w)-2d0*XDIV(a,RT*T2**3)*
     &         (bu+2d0*(Tshift+dts*Tshift+b*dts*u+b2dw))**2
c-------Alternative method
c       dts=D*Tshift
c       db=D*b
c       bt2=2d0*(b+Tshift)
c       bqtp=bt2+q
c       bqtm=bt2-q
c       term2n=-2d0+(2d0+6d0*db**2+6d0*dts+6d0*dts**2-
c    &          6d0*db-12d0*db*dts)/(1d0-db+dts)**3
c       terma=2d0+D*bqtm
c       termb=2d0+D*bqtp
c       termc=4d0*(db*b+b+Tshift*(1d0+dts+2d0*db))-D*q**2
c       term3n=-8d0*a/RT*(q**2-
c    &         4d0*b**2-8d0*b*Tshift-4d0*Tshift**2+
c    &         2d0*bqtp*termc/termb+
c    &         2d0*bqtm*termc/terma)/terma**2/termb**2
c       PHIPRX=term2n+term3n*D**3

c  Compute derivative w.r.t. tau (dimensionless temperature).
      elseif (itau.eq.1) then
        do i=1,nc
          if (z(i).gt.0) then
            daidt(i)=-atci(i)*ki(i)/SQRT(T*TcPR(i))*
     &             (1d0+ki(i)*(1d0-SQRT(XDIV(T,TcPR(i)))))
          endif
        enddo
        if (icomp.ne.0) then
          dadt=daidt(icomp)
        else
          dadt=0d0
          do i=1,nc
          do j=1,nc
            if (z(i).gt.0 .and. z(j).gt.0) then
              da=SQRT(ai(i)/ai(j))*daidt(j)/2d0+
     &           SQRT(ai(j)/ai(i))*daidt(i)/2d0
              dadt=dadt+z(i)*z(j)*(1d0-fPRkij(1,i,j))*da
            endif
          enddo
          enddo
        endif
        PHIPRX=(a/T-dadt)*XLOG(arg2)/Rxgas*XDIV(1d0,q)

c  Compute 2nd derivative w.r.t. tau (dimensionless temperature).
      elseif (itau.eq.2) then
        do i=1,nc
          if (z(i).gt.0) then
            daidt(i)=-atci(i)*ki(i)/SQRT(T*TcPR(i))*
     &               (1d0+ki(i)*(1d0-SQRT(T/TcPR(i))))
            d2aidt2(i)=atci(i)*ki(i)/SQRT(TcPR(i))/2d0*
     &               (1d0+ki(i))/T**1.5d0
          endif
        enddo
        if (icomp.ne.0) then
          dadt=daidt(icomp)
          d2adt2=d2aidt2(icomp)
        else
          dadt=0d0
          d2adt2=0d0
          do i=1,nc
          do j=1,nc
            if (z(i).gt.0 .and. z(j).gt.0) then
              da=SQRT(ai(i)/ai(j))*daidt(j)/2d0+
     &           SQRT(ai(j)/ai(i))*daidt(i)/2d0
              daij=0.5d0*(daidt(i)/SQRT(ai(i)*ai(j))-
     &                    daidt(j)*SQRT(ai(i))/(ai(j)**1.5d0))
              daji=0.5d0*(daidt(j)/SQRT(ai(j)*ai(i))-
     &                    daidt(i)*SQRT(ai(j))/(ai(i)**1.5d0))
              aaij=SQRT(ai(i)/ai(j))
              aaji=SQRT(ai(j)/ai(i))
              d2a=(aaij*d2aidt2(j)+daidt(j)*daij+
     &             aaji*d2aidt2(i)+daidt(i)*daji)/2d0
              dadt=dadt+z(i)*z(j)*(1d0-fPRkij(1,i,j))*da
              d2adt2=d2adt2+z(i)*z(j)*(1d0-fPRkij(1,i,j))*d2a
            endif
          enddo
          enddo
        endif
        PHIPRX=d2adt2*T*XLOG(arg2)/Rxgas/q

c  Compute 3rd derivative w.r.t. tau (dimensionless temperature).
      elseif (itau.eq.3) then
        PHIPRX=0d0           !Not yet implemented
      endif

      end                                                function PHIPRX
c
c ======================================================================
c
      subroutine REDPR (z,Tred,Dred)
c
c  Returns reducing parameters associated with the PR model.
c
c  Input:
c        z--Composition array (array of mole fractions)
c
c  Outputs:
c     Tred--Reducing temperature [K]
c     Dred--Reducing molar density [mol/L]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-24-04 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax)

      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        Tred=Tzred(icomp)
        Dred=Dzred(icomp)
      else
        Tsum=SUM(z(1:nc)*Tzred(1:nc))
        Vsum=SUM(z(1:nc)*Vzred(1:nc))
        if (ABS(Tsum).gt.0.5d-12 .and. ABS(Vsum).gt.0.5d-12) then
          Tred=Tsum
          Dred=1d0/Vsum
        else
          Tred=100d0
          Dred=10d0
        endif
      endif

      end                                               subroutine REDPR
c
c ======================================================================
c
      subroutine SETPR (mode,nread,icomp,ierr,herr)
c
c  Set up working arrays for use with cubic equations of state.
c
c  Inputs:
c     mode--Indicates what/where to read: 0 = Read from file.
c                                         1 = Get tpr coefficients from fluid file, but nothing else.
c                                         2 = Put constants into PR common block if PRT not in FLD file.
c    nread--File to read data from
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-10-04 EWL, original version

      include 'COMMONS.INC'
      character herr*255

      ierr=0
      herr=' '

      if (mode.eq.2) then
        acnPR(icomp)=accen(icomp)
        TcPR(icomp)=Tcrit(icomp)
        PcPR(icomp)=Pcrit(icomp)
        RETURN
      endif

c  Read data from file.
      if (mode.eq.0) then
        read (nread,*,err=102) Tmeos(icomp) !Lower temperature limit
        read (nread,*,err=102) Txeos(icomp) !Upper temperature limit
        read (nread,*,err=102) Pxeos(icomp) !Upper pressure limit
        read (nread,*,err=102) Dxeos(icomp) !Upper density limit
        call READHA (nread,3,hcpk(icomp))   !Pointer to Cp0 model
        read (nread,*,err=102) wmas(icomp)  !Molar mass
      else
        do i=1,6
          read (nread,*,err=102)
        enddo
      endif
      read (nread,*,err=102) acnPR(icomp)   !Acentric factor
      read (nread,*,err=102) TcPR(icomp)    !Critical temperature
      read (nread,*,err=102) PcPR(icomp)    !Critical pressure
      read (nread,*,err=102) Dc             !Critical density
      read (nread,*,err=102) Rr             !Gas constant
      if (mode.eq.0) then
        ReosOrg(icomp)=Rr
        Reos(icomp) =xRgas
        Dcrit(icomp)=Dc
        accen(icomp)=acnPR(icomp)
        Tcrit(icomp)=TcPR(icomp)
        Pcrit(icomp)=PcPR(icomp)
        Tzred(icomp)=TcPR(icomp)
        Dzred(icomp)=Dc
        Vzred(icomp)=1d0/Dc
        Dtpn(icomp)=Dxeos(icomp)
        Ptpn(icomp)=0d0
        Dtpv(icomp)=0d0
        if (nc.eq.1 .and. icomp.eq.1) Rxgas=Reos(icomp)
      endif
      read (nread,*,err=102) nj
      if (icomp.ge.0) then
        PRcoef(1,icomp)=0d0
        if (nj.gt.0) then
          iPRflag(icomp)=1   !Flag to indicate that values were read in.
          read (nread,*,err=102) (PRcoef(j,icomp),j=1,nj)
        endif
      endif
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine SETPR
c
c ======================================================================
c
      subroutine SETPRCO (i)
c
c  Compute a value for the translation factor in the translated Peng Robinson
c  EOS for component i.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado

      include 'COMMONS.INC'

c  Compute translation factor if not pre-loaded.
c    Default prescription is the constant portion of translation in
c    Magoulas and Tassios, Fluid Phase Equilib. 56:119-140, 1990.
c    Do not use a T-dependent translation- see Pfohl,
c    Fluid Phase Equilib. 163:157-159, 1999.

      if (iPRflag(i).lt.1 .and. PcPR(i).gt.0.) then
        tr0=-0.014471d0+0.067498d0*acnPR(i)-0.084852d0*acnPR(i)**2+
     &      0.067298d0*acnPR(i)**3-0.017366d0*acnPR(i)**4
        PRcoef(1,i)=(Rxgas*TcPR(i)/PcPR(i))*tr0
      endif

      end                                             subroutine SETPRCO
c
c ======================================================================
c
      subroutine PREOS (i)
c
c  Turn on or off the use of the PR cubic equation.  Should be called after
c  calling SETUP.
c
c  Inputs:
c        i--Flag specifying use of PR:
c           0 - Use full equation of state (Peng-Robinson off)
c           1 - Use full equation of state with Peng-Robinson for sat. conditions
c               (not currently working)
c           2 - Use Peng-Robinson equation for all calculations
c           3 - Peng-Robinson with translation term deactivated
c           if i=-1, then i is returned with the current status of the PR EOS.
c              A value of zero indicates that it is not in use. When in use,
c              a 2 or 3 will be returned, depending on which option above was
c              previously selected.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  07-21-03 EWL, original version
c  08-07-13 EWL, add PR3

      include 'COMMONS.INC'
      character heos*3,hrf*3,herr*255
      dimension x0(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PREOS

      k=1
      hrf='DEF'
      x0(1:nc)=1d0
      if (i.eq.-1) then
        i=0
        if (iEOSmx.eq.iEOSPR)  i=2
        if (iEOSmx.eq.iEOSPR3) i=3
      elseif (i.ge.1 .and. i.le.3) then
        call RESETA(0)
        lSatTP=.false.
        heos='PR'                 !This is for the full PR.
        if (i.eq.3) heos='PR3'    !This is PR with the translation term deactivated.
        if (iEOSmx.ne.iEOSPR .and. iEOSmx.ne.iEOSPR3) hsavpr=heosmx
        do j=1,nx
          if (iEOSk(j).ne.iEOSPR .and. iEOSk(j).ne.iEOSPR3)
     &        hsavprk(j)=heosk(j)
          heosk(j)=heos
          call SETEOSPURE (j,heos)
        enddo
        call SETEOSMX (heos)
        hsvref=' '
        call SETREF (hrf,k,x0,0d0,0d0,300d0,0d0,ierr,herr)
      elseif (hsavpr.ne.' ') then
        call RESETA(0)
        lSatTP=.false.
        do j=1,nx
          if (hsavprk(j).ne.' ') then
            heosk(j)=hsavprk(j)
            call SETEOSPURE (j,heosk(j))
          endif
        enddo
        call SETEOSMX (hsavpr)
        hsavpr=' '
        hsavprk=' '
        hsvref=' '
        call SETREF (hrf,k,x0,0d0,0d0,300d0,0d0,ierr,herr)
      endif

      end                                               subroutine PREOS
c
c ======================================================================
c
      subroutine TPRHOPR (T,P,z,D1,D2)
c
c  Compute density with a volume-translated modification of the
c  Peng-Robinson equation of state::
c
c      P=RT/(v+c+b)-a/((v+c)*(v+c+b)+b(v+c+b))
c
c  c is a translation constant, as given in Peneloux and Rauzy,
c  Fluid Phase Equilib. 8:7-23, 1982.
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       D1--Largest density root [mol/L]
c       D2--Smallest density root [mol/L]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-11-04 EWL, original version

      include 'COMMONS.INC'
      double precision z(ncmax),ai(ncmax),bi(ncmax),ki(ncmax)
      dimension atci(ncmax),az(3),zz(3)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TPRHOPR

      D1=0d0
      D2=0d0
      if (T.le.0. .or. P.le.0. .or. T.gt.1.d20 .or. P.gt.1.d20) RETURN

      call ISPURE (z,icomp)
      if (icomp.ne.0) Rxgas=xRgas
      RT=Rxgas*T

c  Set volume translation factor.
      Tshift=0d0
      if (iEOSmx.ne.iEOSPR3) then
        if (icomp.ne.0) Tshift=PRcoef(1,icomp)
        if (icomp.eq.0) Tshift=SUM(z(1:nc)*PRcoef(1,1:nc))
      endif

      do i=1,nc
        if (z(i).gt.0 .or. icomp.eq.i) then
          bi(i)=0.0778d0*Rxgas*XDIV(TcPR(i),PcPR(i))
          ki(i)=0.37464d0+1.54226d0*acnPR(i)-0.26992d0*acnPR(i)**2
          atci(i)=0.45724d0*Rxgas**2*XDIV(TcPR(i)**2,PcPR(i))
          ai(i)=atci(i)*(1d0+ki(i)*(1d0-SQRT(T/TcPR(i))))**2
        endif
      enddo
      if (icomp.ne.0) then
        a=ai(icomp)
        b=bi(icomp)
      else
        a=0d0
        b=0d0
        do i=1,nc
          do j=1,nc
            a=a+z(i)*z(j)*SQRT(ai(i)*ai(j))*(1d0-fPRkij(1,i,j))
          enddo
          b=b+z(i)*bi(i)
        enddo
      endif

      aa=a*P/(Rxgas*T)**2
      bb=b*P/(Rxgas*T)
      az(1)=(-aa+bb+bb**2)*bb
      az(2)=aa-bb*(3d0*bb+2d0)
      az(3)=bb-1d0
c  Solve the cubic equation to obtain the roots.
c  Only use largest and smallest roots; middle one does not have a physical meaning.
      call CUBIC (az,zz)
c  Compute the translation term.
      D1=XDIV(P,zz(1)*RT)    !Largest density root
      D2=XDIV(P,zz(2)*RT)    !Smallest density root
      v1=1d0/D1
      v2=1d0/D2
      D1=1d0/(v1-Tshift)
      D2=1d0/(v2-Tshift)

c  The following lines can be uncommented to demonstrate that the
c   shifted equation is equivalent to the non-shifted version.
c   If the shift is zero, it is identical to the PR EOS.

c      CC=Tshift*P/Rxgas/T
c      az(1)=(-AA*BB +BB*BB +BB*BB*BB) +
c     &      (-3.*BB*BB*CC + AA*CC + BB*CC*CC + CC*CC*CC -2.*BB*CC-CC*CC)
c      az(2)=(AA-3.*BB*BB-2.*BB) +
c     &      (2.*BB*CC + 3.*CC*CC - 2.*CC)
c      az(3)=(BB-1d0) +3.*CC
c      call CUBIC (az,zz)
c  Compute the translation term.
c      D1=P/(zz(1)*RT)       !Largest density root
c      D2=P/(zz(2)*RT)       !Smallest density root

      end                                             subroutine TPRHOPR
c
c ======================================================================
c
      subroutine ESTPR (i,j,ibin,fPRkijx,ierr,herr)
c
c  Estimate binary parameters for the Peng Robinson mixture model.
c
c  Estimates are based on the following references:
c  Nishiumi, Arai, and Takeuchi, Fluid Phase Equilib., 42:43-62, 1988.
c  Valderrama and Reyes, Fluid Phase Equilib., 13:195-202, 1983.
c
c  Binary interaction parameters fPRkij are stored in common block CFXPR.
c
c  Inputs:
c        i--Component i
c        j--Component j
c     ibin--Counter for binary pair (1,2 = 1; 1,3 = 2; 2,3 = 3; etc.)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                     -117 - No binary parameters found (warning)
c
c     original version 10.14.04
c  10-04-07  HH, change MXINF2 to MXINF1 as is the case in other code
c  05-21-10 EWL, do not set hbin in this routine, overwrites HMX file

      include 'COMMONS.INC'
      character*255 herr
      dimension ifam(nx),IJ(2)

c     Legend
c     igroup    Type
c     0         Not covered in the correlation
c     1         C1-C16 alkane, branched alkanes
c     2         Cycloalkane
c     3         Alkene
c     4         Aromatic
c     5         C18-C20 alkanes
c     6         CO2
c     7         N2
c     8         H2S
c     9         Acetylene
c               Mixtures with hydrogen are covered separately with the Valderram correlation

      dimension igroup(0:11,0:11),binco(5,14)

      parameter (igroup = reshape((/
     & 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
     & 0, 1, 1, 2, 3, 4, 5, 7, 8,10, 0, 0,
     & 0, 1, 1, 0, 3, 0, 5, 0, 0, 0, 0, 0,
     & 0, 2, 0, 2, 3, 4, 6, 7, 8, 9, 0, 0,
     & 0, 3, 3, 3, 3, 0, 6, 7,12,11, 0, 0,
     & 0, 4, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0,
     & 0, 5, 5, 6, 6, 4, 0,12,13, 0, 0, 0,
     & 0, 7, 0, 7, 7, 0,12, 0,14, 0, 0, 0,
     & 0, 8, 0, 8,12, 0,13,14, 0, 0, 0, 0,
     & 0,10, 0, 9,11, 0, 0, 0, 0, 0, 0, 0,
     & 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
     & 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0/), (/12,12/)))

      parameter (binco = reshape((/
     & 1.041, 0.110,-0.0403, 0.0367, 0.0000,
     & 1.017,-0.417,-0.0124, 0.0852, 0.0000,
     & 1.025, 0.317,-0.0385,-0.0258, 0.0000,
     & 0.823, 0.000, 0.0673, 0.0000,-0.0051,
     & 0.883, 0.000, 0.0023, 0.0000, 0.0000,
     & 0.948, 0.000,-0.0084, 0.0000, 0.0000,
     & 0.982, 0.000,-0.0241, 0.0000, 0.0000,
     & 0.907, 0.000, 0.0109, 0.0000, 0.0000,
     & 1.090, 0.000,-0.1435, 0.0000, 0.0000,
     & 0.855, 0.000, 0.0000, 0.0000, 0.0000,
     & 0.965, 0.000, 0.0000, 0.0000, 0.0000,
     & 1.016, 0.000, 0.0000, 0.0000, 0.0000,
     & 0.894, 0.000, 0.0000, 0.0000, 0.0000,
     & 0.848, 0.000, 0.0000, 0.0000, 0.0000/), (/5,14/)))

c  Assign family code to components i and j.
      ierr=ibin    !ibin is no longer used, this line keeps the compiler happy.
      ierr=0
      herr=' '
      fPRkijx=0d0
      ij(1)=i
      ij(2)=j
      if (i.ne.j) then
        do k=1,2
          iL=ij(k)
          ifam(iL)=0         !Fluids not covered
          if ((hfamily(iL).eq.'br-alkane').or.
     &        (hfamily(iL).eq.'n-alkane')) then
            ifam(iL)=5                                               !Treat like C17-C20; untested
            if (wmas(iL).lt.227) ifam(iL)=1                          !C1 to C16
            if (wmas(iL).ge.227 .and. wmas(iL).lt.283) ifam(iL)=5    !C17 to C20
          endif
          if (hfamily(iL).eq.'napthene')  ifam(iL)=2
          if (hfamily(iL).eq.'br-alkene') ifam(iL)=3
          if (hfamily(iL).eq.'n-alkene')  ifam(iL)=3
          if (hfamily(iL).eq.'aromatic')  ifam(iL)=4
          if (hfamily(iL).eq.'other')     ifam(iL)=10      !Other fluids
          if (iFluidID(iL).eq.31)         ifam(iL)=6       !CO2
          if (iFluidID(iL).eq.16)         ifam(iL)=7       !N2
          if (iFluidID(iL).eq.32)         ifam(iL)=8       !H2S
          if (iFluidID(iL).eq.30)         ifam(iL)=9       !Acetylene
        enddo

c  The following is the correlation of Nishiumi and Arai.
        if (igroup(ifam(i),ifam(j)).eq.0) then
          fPRkijx=0d0
          ierr=-117
        else
          wdif=ABS(acnPR(i)-acnPR(j))
          kk=igroup(ifam(i),ifam(j))
          bigc=binco(1,kk)+binco(2,kk)*wdif
          bigd=binco(3,kk)+binco(4,kk)*wdif
          bige=binco(5,kk)
          vcrat=Dcrit(i)/Dcrit(j)
          if (vcrat.lt.1.) vcrat=1d0/vcrat
          fPRkijx=1d0-(bigc+bigd*vcrat+bige*vcrat**2)
          ierr=0
        endif

c  The following is the correlation of Valderrama and Reyes
c   (Fluid Phase Equilib. 13:195-202, 1983) for mixtures with hydrogen,
c   but evaluated at the normal boiling point of the non-hydrogen
c   component instead of with full temperature dependence.
        if (iFluidID(i).eq.12 .or. iFluidID(j).eq.12) then
          if (iFluidID(i).eq.12) then
            wi2=acnPR(j)
            tr2=Tnbp(j)/Tcrit(j)
          else
            wi2=acnPR(i)
            tr2=Tnbp(i)/Tcrit(i)
          endif
          alpha2=0.1805d0+3.21d0*wi2+2.437d0*wi2**2
          beta2=0.1323d0+0.5507d0*wi2+3.5994d0*wi2**2
          fPRkijx=alpha2-beta2/tr2
          ierr=0
        endif
      else
        fPRkijx=0d0
        ierr=0
      endif

c  The following is based loosely upon the correlation of water+alkane given in
c   Daridon, Lagourette, Saint-Guirons, and Xans, Fluid Phase Equilib. 91:31-54, 1993.
c   Does not implement temperature or composition dependent contribution; thus just a rough estimate.
      if (iFluidID(i).eq.1 .or. iFluidID(i).eq.3) then
        if (hfamily(j).eq.'n-alkane') then
          nmet=(INT(wmas(j))-2)/14-2        !Number of -CH2- groups
          fPRkijx=0.5d0-0.00834d0*FLOAT(nmet)
          ierr=0
        endif
      elseif (iFluidID(j).eq.1 .or. iFluidID(j).eq.3) then
        if (hfamily(i).eq.'n-alkane') then
           nmet=(INT(wmas(i))-2)/14-2        !Number of -CH2- groups
          fPRkijx=0.5d0-0.00834d0*FLOAT(nmet)
          ierr=0
        endif
      endif

c  Set warning message, although currently it is not used outside this routine.
c     call ERRNUM (-905,0,'ESTPR',' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine ESTPR
c
c ======================================================================
c
      subroutine PHIDERVPR (iderv,T,rho,z,dadn,dnadn,ierr,herr)
c
c  Calculate various derivatives needed for VLE determination based
c  on Mollerup and Michelsen (1992) for the volume translated
c  Peng-Robinson equation of state.
c
c  Inputs:
c    iderv--Set to 1 for first order derivatives only (dadn and dnadn)
c           Set to 2 for full calculations
c        T--Temperature [K]
c      rho--Density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     dadn--n*d(ar)/d(ni)   (where n is mole number)
c    dnadn--d(n*ar)/d(ni)
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by G. Venkatarathnam, Indian Institute of Technology Madras
c  01-14-14  GV, Original version.
c  09-20-16  GV, Check for v+tsnt < b condition.

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      double precision z(ncmax),nt,n(ncmax),dxidni(ncmax)
      double precision ai(ncmax),bi(ncmax),ki(ncmax),atci(ncmax)
      double precision aij(ncmax,ncmax),di(ncmax),dF_dnidT(ncmax)
      double precision dF_dni(ncmax),dit(ncmax),dij(ncmax,ncmax)
      double precision dF_dnidnj(ncmax,ncmax),dP_dni(ncmax)
      double precision dV_dni(ncmax),dF_dnidV(ncmax),bij(ncmax,ncmax)
      double precision lnphi(ncmax),dlnphi_dt(ncmax),dlnphi_dp(ncmax)
      double precision dlnphi_dnj(ncmax,ncmax),dadn(ncmax),dnadn(ncmax)
      double precision sri(ncmax),hri(ncmax),gri(ncmax),B0i(ncmax)
      character*255 herr

      common /gvpr_lnphi/ lnphi,dlnphi_dt,dlnphi_dp,dlnphi_dnj
      common /gvpr_p/     p,dp_dni,dv_dni,dp_dt,dp_dv

c  Next two lines to be deleted after checking the results with fd method.
c     double precision dydx(20),h1(4),n1(20),P1(4),x1(20)
c     double precision n1total,ntotal

c  Notation used in this routine is same as that in Michelsen and Mollerup's work
c    and is different from that of Refprop.
c     F=Ar/RT=n*alphar (Refprop)
c     nt=n (Refprop)
c     n(1:nc)=n*z(i) (Refprop)

      ierr=0
      herr=' '

c  This routine can work when sum(z) <> 1 for taking numerical derivatives.
      n(1:nc)=z(1:nc)   !n are mole numbers.
      nt=SUM(n(1:nc))   !Total moles of feed
      V=XDIV(nt,rho)    !Important: Total volume, not specific volume.
      call RMIX (z)
      call ISPURE (z,icomp)

c  Set up volume translation factor.
      if (icomp.ne.0) Tshift=PRcoef(1,icomp)
      if (icomp.eq.0) Tshift=SUM(z(1:nc)*PRcoef(1,1:nc))
      i=-1
      call PREOS(i)
      if (i.eq.3) Tshift=0d0      !Use if translation is turned off.

c  Parameters for the SRK equation.
c     d1=1d0
c     d2=0d0
c  Parameters for the Peng-Robinson equation.
      d1=1d0+SQRT(2d0)
      d2=1d0-SQRT(2d0)

c  b(i), ki(i), atci(i) need to be determined once only at the beginning.
c  Should be removed from here.
      do i=1,nc
        if (z(i).gt.0) then
          bi(i)=0.0778d0*Rxgas*TcPR(i)/PcPR(i)
          ki(i)=0.37464d0+1.54226d0*acnPR(i)-0.26992d0*acnPR(i)**2
          atci(i)=0.45724d0*Rxgas**2*TcPR(i)**2/PcPR(i)
          ai(i)=atci(i)*(1d0+ki(i)*(1d0-SQRT(t/TcPR(i))))**2
        endif
      enddo

c  amix here is different from a in Refprop. Here amix is actually a*nt*nt. nt<=1.
c  Refprop does not allow nt < 1. This routine allows nt to be any value.
      amix1=0d0
      bmix1=0d0
      do i=1,nc
        do j=1,nc
          if (z(i).gt.0 .and. z(j).gt.0) then
            aij(i,j)=SQRT(ai(i)*ai(j))*(1d0-fPRkij(1,i,j))
            amix1=amix1+n(i)*n(j)*aij(i,j)
          endif
        enddo
        bmix1=bmix1+n(i)*bi(i)
      enddo

      D=amix1           !Actually amix*nt*nt
      B=bmix1           !Actually bmix*nt
      tsnt=Tshift*nt
      if (V+tsnt-B.le.0.) then
        call ERRNUM (290,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif
      g=XLOG(V+tsnt-B)-XLOG(V)    !g=f(V,B)
      f=XLOG((V+tsnt+d1*B)/(V+tsnt+d2*B))/(Rxgas*B*(d1-d2))     !f=f(V,B)
c  F0 must be the same as Ar calculated from Gibbs routine.
c     call GIBBS (t,rho,z,Ar,Gr)
      F0=-nt*g-D*f/T    !Ar/RT

c  First order partial derivatives of F, g, and f.
      gv= 1d0/(V+tsnt-B)-1d0/V                        !gv=diff(g,B)
      gb=-1d0/(V+tsnt-B)                              !gb=diff(g,V)
      fv=-1d0/(Rxgas*(V+tsnt+d1*B)*(V+tsnt+d2*B))     !fv=diff(f,V)
      fb=-(f+(V+tsnt)*fv)/B                           !fb=diff(f,B)

      F0n=-g                                          !nt=SUM(n(i)). diff(nt,n(i))=1
      F0t= D*f/T**2d0
      F0v=-nt*gv-D*fv/T
      F0b=-nt*gb-D*fb/T
      F0d=-f/T

c  Second order partial derivatives of F, g, and f.
      gvv=-1d0/(V+tsnt-B)**2d0+1d0/V**2d0             !gvv=diff(gv,V)
      gbv= 1d0/(V+tsnt-B)**2d0                        !gbv=diff(gb,V)
      gbb=-1d0/(V+tsnt-B)**2d0                        !gbb=diff(gb,B)

c     fvv= 1d0/(Rxgas*B*(d1-d2))*
c    &     (-1d0/(V+d1*B)**2d0+1d0/(V+d2*B)**2d0)
      fvv= (2d0*V+2d0*tsnt+d1*B+d2*B)/
     &     (Rxgas*(V+tsnt+d1*B)**2*(V+tsnt+d2*B)**2)  !fvv=diff(fv,V)
      fbv=-(2d0*fv+(V+tsnt)*fvv)/B                    !fbv=diff(fb,V)
      fbb=-(2d0*fb+(V+tsnt)*fbv)/B                    !fbb=diff(fb,B)

      F0nv=-gv
      F0nb=-gb
      F0tt=-2d0*F0t/T
      F0bt= D*fb/T**2d0
      F0dt= f/T**2d0
      F0bv=-nt*gbv-D*fbv/T
      F0bb=-nt*gbb-D*fbb/T
      F0dv=-fv/T
      F0bd=-fb/T
      F0tv= D*fv/T**2d0
      F0vv=-nt*gvv-D*fvv/T

c  Partial derivates of D and B.
c  Dij and Di should be rewritten to prevent calculation of ai(i)*ai(... twice.
c  aij calculation loop should also include Di calculation.
c  To be changed after validating the routine.
      do i=1,nc
        if (z(i).gt.0) B0i(i)=(SUM(n(1:nc)*(bi(i)+bi(1:nc)))-B)/nt
      enddo
      do i=1,nc
        if (z(i).gt.0) then
          Di(i)=2d0*SUM(n(1:nc)*SQRT(ai(i)*ai(1:nc))*
     &         (1d0-fPRkij(1,i,1:nc)))
          Dit(i)=GET_DIT(n,i,t,ai,ki)
          Dij(i,1:nc)=2d0*SQRT(ai(i)*ai(1:nc))*(1d0-fPRkij(1,i,1:nc))
          Bij(i,1:nc)=(bi(i)+bi(1:nc)-B0i(i)-B0i(1:nc))/nt
        endif
      enddo
      Dt =GET_DT (n,i,t,ai,ki)
      Dtt=GET_DTT(n,i,t,ai,ki)

c  First order partial derivatives
      dF_dni(1:nc)=F0n+F0b*Bi(1:nc)+F0d*Di(1:nc)
      dF_dT=F0t+F0d*Dt
      dF_dV=F0v

c  Calculate pressure, compressibility factor, and lnphi (with volume translation).
      P=-Rxgas*T*dF_dV+nt*Rxgas*T/V
      ZZ=P*(V+tsnt)/(nt*Rxgas*T)
      lnphi(1:nc)=dF_dni(1:nc)-XLOG(ZZ)

c  Transfer to Refprop common blocks.
      dnadn(1:nc)=dF_dni(1:nc)
      dadn (1:nc)=dnadn(1:nc)-F0
      if (iderv.eq.0) RETURN

c  Second order partial derivatives.
      do i=1,nc
        if (z(i).gt.0) then
          dF_dnidnj(i,1:nc)=F0nb*(Bi(i)+Bi(1:nc))+
     &      F0bd*(Bi(i)*Di(1:nc)+Bi(1:nc)*Di(i))+
     &      F0b*Bij(i,1:nc)+F0bb*Bi(i)*Bi(1:nc)+
     &      F0d*Dij(i,1:nc)
        endif
      enddo
      dF_dnidT(1:nc)=(F0bt+F0bd*Dt)*Bi(1:nc)+F0dt*Di(1:nc)+
     &    F0d*Dit(1:nc)
      dF_dnidV(1:nc)=F0nv+F0bv*Bi(1:nc)+F0dv*Di(1:nc)
      dF_dT2 =F0tt+2d0*F0dt*Dt+F0d*Dtt
      dF_dTdV=F0tv+F0dv*Dt
      dF_dV2 =F0vv

c  Pressure and its derivates.
c     P=RT/(v+t-b)-a/((v+t)*(v+t+b)+b(v+t-b))
      dP_dV=-Rxgas*T*dF_dV2-nt*Rxgas*T/V**2d0
      dP_dT=-Rxgas*T*dF_dTdV+P/T
      dP_dni(1:nc)=-Rxgas*T*dF_dnidV(1:nc)+Rxgas*T/V
      dV_dni(1:nc)=-dP_dni(1:nc)/dP_dV

c  Derivatives of fugacity coefficient.
      dlnphi_dT(1:nc)=dF_dnidT(1:nc)+1d0/T-dV_dni(1:nc)*dP_dT/(Rxgas*T)
      dlnphi_dP(1:nc)=dV_dni(1:nc)/(Rxgas*T)-XDIV(1d0,P)
      do i=1,nc
        if (z(i).gt.0) then
          dlnphi_dnj(i,1:nc)=dF_dnidnj(i,1:nc)+1d0/nt+
     &    dP_dni(1:nc)*dP_dni(i)/(Rxgas*T*dP_dV)
        endif
      enddo

c  Specific residual bulk properties at constant volume and temperature.
      Arvt =Rxgas*T*F0/nt
      Srvt =(-Rxgas*T*dF_dT-Rxgas*F0)/nt
      Cvrvt=(-Rxgas*T**2d0*dF_dT2-2d0*Rxgas*T*dF_dT)/nt
      Cprvt=(Cvrvt-T*dP_dT**2d0/dP_dV-Rxgas*nt)/nt
      Hrvt =(Arvt+T*Srvt+P*(V+tsnt)-nt*Rxgas*T)/nt
      Grvt =(Arvt+p*(V+tsnt)-nt*Rxgas*t)/nt      !Gr(T,V,n)

c  Specific residual bulk properties at constant pressure and temperature.
      Srpt=Srvt+Rxgas*XLOG(ZZ)
      Hrpt=Hrvt
      Grpt=GrVt-Rxgas*t*XLOG(ZZ)                 !Gr(p,T,n)

c  Partial derivatives of residual properties at constant pressure and temperature.
      Hri(1:nc)=-Rxgas*T**2d0*dlnphi_dT(1:nc)
      Gri(1:nc)= Rxgas*T*lnphi(1:nc)
      Sri(1:nc)=(Hri(1:nc)-Gri(1:nc))/T

c  Transfer to Refprop common blocks.
      dpdn(1:nc)=nt*dp_dni(1:nc)
      dphidt(1:nc)=dlnphi_dt(1:nc)
      dphidp(1:nc)=dlnphi_dp(1:nc)
      dphidnj(1:nc,1:nc)=dlnphi_dnj(1:nc,1:nc)*nt! *nt to be tested. 12/12/14
      d2adnn(1:nc,1:nc)=dF_dnidnj(1:nc,1:nc)     !Should this be "*nt"?

c  Added 02/10/2014
      dtdn(1:nc)=-dpdn(1:nc)/dP_dT
      dvdn(1:nc)=dV_dni(1:nc)*nt/V               !(V+tsnt) corrected to V on July 3, 2016.
      do j=1,nc
        if (1d0-z(j).ne.0.) then
          dphidxj(1:nc,j)=dphidnj(1:nc,j)/(1d0-z(j))
          dxidni(j)=1d0/nt*(1d0-z(j))
          dpdxi(j)=dpdn(j)/dxidni(j)
        else
          dphidxj(1:nc,j)=dphidnj(1:nc,j)        !Check once.
          dpdxi(j)=dpdn(j)
        endif
      enddo
      dlnfinidt(1:nc)= 1d0/T +dF_dnidT(1:nc)     !(7.36 of GERG 2008)
      dlnfinidV(1:nc)=-rho/nt+dF_dnidV(1:nc)     !(7.37)
      dmurdt(1:nc)=-Sri(1:nc)                    !At const P,n

c     call DPDDX (t,rho,z,dpdD1)                 !Check dPdD against that calculated by Refprop.
      dPdD=-nt/rho**2*dP_dV
      dDdn(1:nc)=rho-dpdn(1:nc)/dPdD             !Same as FEQ
      dDdn(1:nc)=dDdn(1:nc)+rho**2*PRcoef(1,1:nc)*SUM(z(1:nc))  !Volume translation term

c  Still need to add xlnfi
c     xlnfi(1:nc)=XLOG(z(1:nc)/ZZ)+dF_dni(1:nc)  !ln(fi)-ln(p)

      end                                           subroutine PHIDERVPR

      function GET_DIT (n,i,t,ai,ki)
      include 'COMMONS.INC'
      double precision n(ncmax),ai(ncmax),ki(ncmax)
      GET_DIT=2d0*SUM(n(1:nc)*SQRT(ai(i)*ai(1:nc))*
     &        (1d0-fPRkij(1,i,1:nc))*
     &        (ki(i)*ki(1:nc)/SQRT(TcPR(i)*TcPR(1:nc))-0.5d0*(ki(i)*
     &        (1d0+ki(1:nc))/SQRT(TcPR(i))+ki(1:nc)*(1d0+ki(i))/
     &        SQRT(TcPR(1:nc)))/SQRT(T)))
      end                                               function GET_DIT

      function GET_DT (n,i,t,ai,ki)
      include 'COMMONS.INC'
      double precision n(ncmax),ai(ncmax),ki(ncmax)
      Dt=0d0
      do i=1,nc
        Dt=Dt+n(i)*GET_DIT(n,i,t,ai,ki)
      enddo
      GET_DT=Dt/2d0
      end                                                function GET_DT

      function GET_DTT (n,i,t,ai,ki)
      include 'COMMONS.INC'
      double precision n(ncmax),ai(ncmax),ki(ncmax)
      Dtt=0d0
      do i=1,nc
        total=0d0
        do j=1,nc
          total=total+n(j)/4d0*SQRT(ai(i)*ai(j))*
     &          (1d0-fPRkij(1,i,j))*
     &          (ki(i)*(1d0+ki(j))/SQRT(TcPR(i))+ki(j)*(1d0+ki(i))/
     &          SQRT(TcPR(j)))*(T**(-1.5d0))
        enddo
        Dtt=Dtt+n(i)*total
      enddo
      GET_DTT=Dtt
      end                                               function GET_DTT

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                   End file CORE_PR.FOR
c ======================================================================
