c  Begin file TRNS_TCX.FOR
c
c  This file contains the core routines for thermal conductivity.
c
c  Contained here are:
c
c     subroutine TCXK   (icomp,T,D,tcx,ierr,herr)
c     subroutine TCXK0  (icomp,T,tcx0,ierr,herr)
c     subroutine TCXKB  (icomp,T,D,tcxb,ierr,herr)
c     subroutine TCXKC  (icomp,T,D,tcxc,ierr,herr)
c     subroutine SETTCX (nread,icomp,htype,ierr,herr)
c     subroutine SETTCE (nread,icomp,htype,ierr,herr)
c     function TCX0HC   (icomp,T,D,ierr,herr)
c     function TCX1DG   (icomp,ifunc,T)
c     function TCX1BK   (icomp,T,D)
c     function TCX3CR   (icomp,T,D)
c     function TCXM1C   (T,D,z,ierr,herr)
c     function TCXH2O   (icomp,T,D)
c     function TCCR_2010(T,D,QD)
c
c  No longer in use:
c     function TCCNH3   (icomp,T,D)
c     function TCCCH4   (icomp,T,D)
c     function TCXH2    (icomp,T,D)
c     function TCXHE    (icomp,T,D)
c     function TCXETY   (icomp,T,D)
c     function TCXR23   (icomp,T,D)
c     function TCXD2O   (icomp,T,D)
c     function TCXBKold (icomp,ifunc,T,D)
c     function TCXCRold (icomp,ifunc,icr2,T,D)
c
c
c ======================================================================
c ======================================================================
c
      subroutine TCXK (icomp,T,D,tcx,ierr,herr)
c
c  Thermal conductivity of a pure fluid as a function of temperature
c  and density.
c
c  The thermal conductivity is the sum of the dilute-gas, background
c  (or residual), and critical enhancement terms (separate functions
c  for the three terms are used because of the requirements of certain
c  mixture models, e.g. ECS).
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c      tcx--Thermal conductivity [W/m-K]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TCXK

      call TCXK0 (icomp,T,tcx0,ierr,herr)
      call TCXKB (icomp,T,D,tcxb,ierr,herr)
      call TCXKC (icomp,T,D,tcxc,ierr,herr)
      tcx=tcx0+tcxb+tcxc
      if (icomp.le.ncmax) then
        xtcx0(icomp)=tcx0    !Save for use in ALLPROPS routine.
        xtcxr(icomp)=tcxb
        xtcxc(icomp)=tcxc
      endif
      end                                                subroutine TCXK
c
c ======================================================================
c
      subroutine TCXK0 (icomp,T,tcx0,ierr,herr)
c
c  Dilute-gas part of the thermal conductivity of a pure fluid as a
c  function of temperature and density.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c
c  Outputs:
c     tcx0--Thermal conductivity [W/m-K]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown model specified
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TCXK0

      ierr=0
      herr=' '
      tcx0=0d0
      if (icomp.le.0) RETURN

      if (htcxk(icomp).eq.'TC0') then
        tcx0=TCX0HC(icomp,T,0d0,ierr,herr)
      elseif (htcxk(icomp).eq.'TC1') then
        tcx0=TCX1DG(icomp,1,T)
      elseif (htcxk(icomp).eq.'TC2') then
        tcx0=TCX1DG(icomp,2,T)
      elseif (htcxk(icomp).eq.'TC3') then
        tcx0=TCX1DG(icomp,3,T)
      elseif (htcxk(icomp).eq.'TC5') then
        tcx0=TCX1DG(icomp,5,T)
      elseif (htcxk(icomp).eq.'TC7') then
        tcx0=TCX1DG(icomp,7,T)
      elseif (htcxk(icomp).eq.'ECS') then
        ierr=999   !Do nothing, ECS is in effect and no ETAKR is available.
      elseif (htcxk(icomp).eq.'NUL') then
        ierr=999
      elseif (htcxk(icomp).eq.'NBS') then
        ierr=999
      else
        call ERRNUM (502,0,'TCXK0',htcxk(icomp),0d0,0d0,0d0,ierr,herr)
      endif

      end                                               subroutine TCXK0
c
c ======================================================================
c
      subroutine TCXKB (icomp,T,D,tcxb,ierr,herr)
c
c  Background part of the thermal conductivity of a pure fluid as a
c  function of temperature and density.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c     tcxb--Thermal conductivity [W/m-K]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown model specified
c     herr--Error string (character*255)

      include 'COMMONS.INC'
c     include 'COMANC.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TCXKB

      ierr=0
      herr=' '
      tcxb=0d0
      if (icomp.le.0) RETURN

      if (htcxk(icomp).eq.'TC0') then
        tcxb=TCX0HC(icomp,T,D,ierr,herr)         !Need to subtract off value at zero density for hard coded models
        tcxb=tcxb-TCX0HC(icomp,T,0d0,ierr,herr)  ! since it is included in call to tcxkb.
      elseif (htcxk(icomp).eq.'TC1') then
        tcxb=TCX1BK(icomp,T,D)
      elseif (htcxk(icomp).eq.'TC2') then
        tcxb=TCXBKold(icomp,2,T,D)
      elseif (htcxk(icomp).eq.'TC3') then
        tcxb=TCXBKold(icomp,3,T,D)
      elseif (htcxk(icomp).eq.'TC5') then
        tcxb=TCXBKold(icomp,5,T,D)
      elseif (htcxk(icomp).eq.'TC7') then
        z(1:nc)=0d0
        z(icomp)=1d0
        eta=0d0
        if (l7eta) call ETAK (icomp,T,D,eta,ierr,herr)
        tcxb=     RPNEQN(0,icomp,8,T,D,z,eta,ierr,herr)
        tcxb=tcxb+RPNEQN(0,icomp,9,T,D,z,eta,ierr,herr)
      else
        call ERRNUM (502,0,'TCXKB',htcxk(icomp),0d0,0d0,0d0,ierr,herr)
      endif

      end                                               subroutine TCXKB
c
c ======================================================================
c
      subroutine TCXKC (icomp,T,D,tcxc,ierr,herr)
c
c  Critical enhancement for the thermal conductivity of a pure fluid as
c  a function of temperature and density.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c     tcxc--Thermal conductivity [W/m-K]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown model specified
c     herr--Error string (character*255)

      include 'COMMONS.INC'
c     include 'COMANC.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TCXKC

      ierr=0
      herr=' '
      tcxc=0d0
      if (icomp.le.0) RETURN

      z(1:nc)=0d0
      z(icomp)=1d0
      if (htcxcr(icomp).eq.'TK3') then
        tcxc=TCX3CR(icomp,T,D)
      elseif (htcxcr(icomp).eq.'TK6') then
        etacal=0d0
        tcxc=TCXM1C(T,D,z,ierr,herr)
      elseif (htcxcr(icomp).eq.'TK7') then
        eta=0d0
        if (l7eta) call ETAK (icomp,T,D,eta,ierr,herr)
        tcxc=RPNEQN(0,icomp,10,T,D,z,eta,ierr,herr)


c...Code no longer in use...
      elseif (htcxcr(icomp).eq.'TK1') then
        tcxc=TCXCRold(icomp,1,0,T,D)
      elseif (htcxcr(icomp).eq.'TK2') then
        tcxc=TCXCRold(icomp,2,2,T,D)        !Hydrocarbon model of Younglove and Ely (critical part integral w/ TC2).
      elseif (htcxcr(icomp).eq.'TK4') then
        tcxc=TCXCRold(icomp,2,4,T,D)
      elseif (htcxcr(icomp).eq.'NH3') then
        tcxc=TCCNH3(icomp,T,D)              !Special function for ammonia; model of Tufeu.
      elseif (htcxcr(icomp).eq.'CH4') then
        tcxc=TCCCH4(icomp,T,D)              !Special function for methane; model of Friend et al.
      elseif (htcxcr(icomp).eq.'NUL') then
        tcxc=0d0                            !No critical enhancement is used.
      else
        call ERRNUM (502,0,'TCXKC',htcxcr(icomp),0d0,0d0,0d0,ierr,herr)
      endif

      end                                               subroutine TCXKC
c
c ======================================================================
c
      function TCX0HC (icomp,T,D,ierr,herr)
c
c  Model for the hardcoded thermal conductivity models.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCX0HC--Thermal conductivity [W/m-K]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-29-00 EWL, original version
c  12-21-09 MLH, added D2, T2, HE3 models

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr

      ierr=0
      herr=' '
      TCX0HC=0d0
      if (htcxhc(icomp).eq.'H2O') then
        TCX0HC=TCXH2O(icomp,T,D)


c...Code no longer in use...
      elseif (htcxhc(icomp).eq.'ETY') then
        TCX0HC=TCXETY(icomp,T,D)
      elseif (htcxhc(icomp).eq.'R23') then
        TCX0HC=TCXR23(icomp,T,D)
      elseif (htcxhc(icomp).eq.'D2O') then
        TCX0HC=TCXD2O(icomp,T,D)
      elseif (htcxhc(icomp).eq.'HE') then
        TCX0HC=TCXHE(icomp,T,D)
      elseif (htcxhc(icomp).eq.'H2') then
        TCX0HC=TCXH2(icomp,T,D)
      elseif (htcxhc(icomp).eq.'D2') then
        ratlam=0.031d0*D
        if (T.ge.Tcrit(icomp)) ratlam=ratlam*(Tcrit(icomp)/T)**5
        ratlam=ratlam+1d0/SQRT(2d0)         !1/SQRT(2) is theoretical value for gas.
        if (ratlam.gt.0.92d0) ratlam=0.92d0 !Match Powers 1954 for liquid.
        TCX0HC=TCXH2(icomp,T,D)*ratlam
      elseif (htcxhc(icomp).eq.'T2') then   !Scale to H2 with theoretical limit for gas.
        TCX0HC=TCXH2(icomp,T,D)/SQRT(3d0)
      elseif (htcxhc(icomp).eq.'HE3') then  !Scale to He4 with theoretical limit for gas.
        TCX0HC=TCXHE(icomp,T,D)/SQRT(3d0/4d0)
      else
        call ERRNUM (502,0,'TCX0HC',htcxhc(icomp),0d0,0d0,0d0,ierr,herr)
      endif

      end                                                function TCX0HC
c
c ======================================================================
c
      function TCX1DG (icomp,ifunc,T)
c
c  Dilute-gas contribution to the thermal conductivity.
c
c  All models are contained here, and are described below.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c    ifunc--Function identifier (1-TC1, 2-TC2, etc.)
c        T--Temperature [K]
c
c  Output (as the function value):
c   TCX1DG--The dilute-gas part of the thermal conductivity [W/m-K]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-06-97  MM, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax)

      TCX1DG=0d0
      if (T.le.0.) RETURN
      i=icomp

c  The composite model (TC1).
      if (ifunc.eq.1) then
        k=ndgnum(i)
        n=ndgden(i)
        tau=1d0
        if (k+n.ge.1) tau=T/Trddgt(i)       !Compute tau only if dilute-gas terms exist, otherwise treddg may not be defined.
        if (k.ge.1) then
          do j=1,k   !Sum the dilute-gas terms, first numerator then denominator.
            if (ctcx(i,j,2).lt.-90.) then
              if (ABS(ctcx(i,j,2)+99d0).lt.1d-6) then                     !Flag:  exponent -99 indicates: multiply numerator term by [1 + coeff*(Cp0 - 2.5*R)]
                TCX1DG=TCX1DG*(1d0+ctcx(i,j,1)*(CP0K(i,T)-2.5d0*xRgas))   ! This is the Cv_internal as used by Vesovic, et al. (1990) for carbon dioxide.
              elseif (ABS(ctcx(i,j,2)+98d0).lt.1d-6) then                 !Flag:  exponent -98 indicates: add Cv0*eta0 to numerator.
                call ETAK00 (i,T,eta,ierr,herr)
                TCX1DG=TCX1DG+ctcx(i,j,1)*(CP0K(i,T)-xRgas)*eta
              elseif (ABS(ctcx(i,j,2)+97d0).lt.1d-6) then                 !Flag:  exponent -97 indicates: add eta0 to numerator.
                call ETAK00 (i,T,eta,ierr,herr)
                TCX1DG=TCX1DG+ctcx(i,j,1)*eta
              elseif (ABS(ctcx(i,j,2)+96d0).lt.1d-6) then                 !Flag:  exponent -96 indicates: add TCX1DG*eta0/M*(Cp0-2.5R)+15/4*R*eta0/M where TCX1DG was created by the previous terms.
                call ETAK00 (i,T,eta,ierr,herr)
                TCX1DG=xRgas*eta/wmas(i)*(TCX1DG*
     &                (CP0K(i,T)/xRgas-2.5d0)+15d0/4d0)
              endif
            else
              TCX1DG=TCX1DG+ctcx(i,j,1)*tau**ctcx(i,j,2)
            endif
          enddo
        endif
        denom=1d0
        if (n.ge.1) denom=SUM(ctcx(i,k+1:k+n,1)*tau**ctcx(i,k+1:k+n,2))   !Denominator
        TCX1DG=TCX1DG/denom*tcxdgt(i)  !Divide numerator by denominator and multiply by reducing parameter (to convert units, etc.).

c  Dilute-gas contribution to the thermal conductivity with the
c    model of Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987; Eqs 19, 27.
c  There are two terms missing from Eq 27 in the JPCRD article.
c    The 15R/4 is missing and a factor of 1/(mol wt) is needed to convert units.
c  Model is no longer used.  See other routines for def. of inputs and outputs.
      elseif (ifunc.eq.2) then
        sigma=ctcx(i,1,1)
        epsk=ctcx(i,2,1)     !Compute dilute-gas viscosity.
        eta0=ctcx(i,3,1)*SQRT(T)/(OMEGA(i,T,epsk,hmdtcx(i))*sigma**2)
        Cp00=CP0K(i,T)/xRgas !Ideal-gas heat capacity
        Gt1=ctcx(i,4,1)
        Gt2=ctcx(i,5,1)
        TCX1DG=1d-3*eta0/wmas(i)*xRgas*(3.75d0+(Cp00-2.5d0)*
     &         (Gt1+Gt2*epsk/T))

c  Dilute-gas contribution to the thermal conductivity with the model of
c    Younglove, J. Phys. Chem. Ref. Data, Volume 11, Supplement 1, 1982.
c  Model is no longer used.  See other routines for def. of inputs and outputs.
      elseif (ifunc.eq.3) then
        omgsum=0d0
        ekt3=(ctcx(i,2,1)/T)**thrd
        do j=4,12
          omgsum=omgsum+ctcx(i,j,1)*ekt3**(7-j)
        enddo
        TCX1DG=ctcx(i,3,1)*T**ctcx(i,3,2)/(ctcx(i,1,1)**2/omgsum)

c  Dilute-gas contribution to the thermal conductivity with the model of
c    Chung, Ajlan, Lee, and Starling, Ind. Eng. Chem. Res. 27:671-679, 1988.
c    See also Reid, Prausnitz, and Poling, Chapter 9.

      elseif (ifunc.eq.5) then
        z(1:nc)=0d0
        z(i)=1d0
c       vc=(sigchk(i)*10d0/0.809d0)**3 !Compute vc in cm3/mol from Chung sigma.
        call THERM (T,0d0,z,pp,e,h,s,Cv,Cp,w,hjt)
        alpha=Cv/xRgas-1.5d0
        beta=0.7862d0-0.7109d0*acchk(i)+1.3168d0*acchk(i)**2
        w=2d0+10.5d0*(T/(1.2593d0*epschk(i)))**2
        psich=1d0+alpha*(0.215d0+0.28288*alpha-1.061d0*beta+
     &        0.26665d0*w)/(0.6366d0+beta*w+1.061d0*alpha*beta)
        call ETAK00 (i,T,eta00,ierr,herr)
        TCX1DG=3.75d0*psich*eta00*xRgas/wmas(i)/1000d0
        tcx0ch=TCX1DG        !Used in TCX5BK

c  Dilute-gas contribution through the use of mathematical operators
c    stored in the fluid file.
      elseif (ifunc.eq.7) then
        z(1:nc)=0d0
        z(icomp)=1d0
        TCX1DG=RPNEQN(0,i,7,T,0d0,z,eta,ierr,herr)
      endif

      end                                                function TCX1DG
c
c ======================================================================
c
      function TCX1BK (icomp,T,D)
c
c  Background contribution to the thermal conductivity with the
c  composite model (TC1).
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCX1BK--The background part of the thermal conductivity [W/m-K]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-06-97  MM, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      TCX1BK=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp

c  Compute tau only if residual terms exist, otherwise tredbk and Dredbk may not be defined.
      tau=1d0
      del=1d0
      if (nbknum(i)+nbkden(i).ge.1) then
        tau=T/Trdbkt(i)
        del=D/Drdbkt(i)
      endif

c  Sum the background terms, first numerator then denominator.
      nterm=ndgnum(i)+ndgden(i)
      if (nbknum(i).ge.1) then
        do j=nterm+1,nterm+nbknum(i)
          if (ctcx(i,j,4).lt.-90) then                !Special cases
            if (ABS(ctcx(i,j,4)+99d0).lt.1d-6) then   !Methane
              if (T.lt.Trdbkt(i) .and. D.lt.Drdbkt(i)) then
                th=(1d0-T/Trdbkt(i))**thrd
                dsat=-1.8802840d0*th**1.062d0-2.8526531d0*th**2.50d0-
     &                3.0006480d0*th**4.500d0-5.2511690d0*th**7.50d0-
     &                13.191869d0*th**12.50d0-37.553961d0*th**23.5d0
                TCX1BK=TCX1BK/EXP5(dsat)
              endif
            endif
          else
            hexp=1d0
            if (ABS(ctcx(i,j,4)).gt.1d-20) hexp=EXP5(-del**ctcx(i,j,4))
            TCX1BK=TCX1BK+
     &             ctcx(i,j,1)*tau**ctcx(i,j,2)*del**ctcx(i,j,3)*hexp
          endif
        enddo
      endif
      if (nbkden(i).ge.1) then
        denom=0d0
        if (del.gt.0.) then
          do j=nterm+nbknum(i)+1,nterm+nbknum(i)+nbkden(i)
            denom=denom+ctcx(i,j,1)*tau**ctcx(i,j,2)*del**ctcx(i,j,3)
          enddo
        endif
        if (ABS(denom).gt.1d-20) TCX1BK=TCX1BK/denom  !Divide numerator by denominator.
      endif
      TCX1BK=TCX1BK*tcxbkt(i)     !Multiply by reducing parameter (to convert units, etc.).

      end                                                function TCX1BK
c
c ======================================================================
c
      function TCX3CR (icomp,T,D)
c
c  Model #3 for the thermal conductivity critical enhancement
c  Olchowy and Sengers, Int. J. Thermophys., 10:417-426, 1989.
c
c  Also applied to CO2 by
c  Vesovic, Wakeham, Olchowy, Sengers,  Watson, and Millat
c  J. Phys. Chem. Ref. Data, 19:763-808, 1990.
c  Equation numbers in comments refer to Vesovic paper.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCX3CR--The critical enhancement to the thermal conductivity [W/m-K]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  06-08-97  MM, original version
c  07-01-08 MLH, do not use viscosity critical enhancement in Olchowy-Sengers model
c  03-09-10 MLH, high T cutoff
c  04-13-10 MLH, do use viscosity critical enhancement in Olchowy-Sengers model
c  09-20-10 MLH, insert special code for new iapws water, more digits on pi
c  11-20-10 MLH, replace call to etak with trnprp to remove problem w ECS only eta

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension x0(ncmax)

      TCX3CR=0d0
      if (T.le.0. .or. D.lt.1d-6) RETURN         !Critical enhancement is zero for low densities (avoid divide by zero).
      i=icomp
      x0=0d0
      x0(i)=1d0
c  Recover the parameters from the storage array
      qd=XDIV(1d0,ctck(i,8,1))    !qd_inverse (cutoff dia, order 10d-9 m)
      R0=ctck(i,3,1)              !R0 (universal amplitude, 1.01 +/- 0.04)
      xi0=ctck(i,6,1)             !xi0 (amplitude, order 1d-10 m)
      gnu=ctck(i,1,1)             !nu (universal exponent, approximately 0.63)
      gam0=ctck(i,7,1)            !gam0 (amplitude, order 0.05 - 0.06)
      gamm=ctck(i,2,1)            !gamma (universal exponent, approximately 1.24)
      treff=ctck(i,9,1)           !tref (reference temperature, 1.5 - 2.0 * Tc)
c  Following two parameters not used here, but in file for future use.
c     z=ctck(i,4,1)               !z (universal exponent, 0.065 +/- 0.005)
c     cvisc=ctck(i,5,1)           !Viscosity constant, approximately 1.075, but often 1)
      if (T.gt.treff) RETURN      !If T is too high, there is no enhancement, so return.

      if (iFluidID(i).eq.1) then  !Check for water.
        TCX3CR=TCCR_2010(T,D,qd)
        RETURN
      endif
      call CRITP (x0,Tc,Pc,Dc,ierr,herr)
      call DPDDX (T,D,x0,dPdD)
      call DPDDX (treff,D,x0,dPdDr)
      call THERM (T,D,x0,pp,e,h,s,Cv,Cp,w,hjt)
c  Function chi (Eq 40 in Vesovic) evaluated at T,D and tref,D.  (Vesovic introduces a T/Tc term which is absent in other papers.)
      chi   =Pc/(Dc*Dc)*D/dPdD              !Vesovic form:  chi=Pc/(Dc*Dc*Tc)*D*T/dPdD
      chiref=Pc/(Dc*Dc)*D/dPdDr*treff/T     !Vesovic form:  chiref=Pc/(Dc*Dc*Tc)*D*tref/dPdD*tref/T
      delchi=chi-chiref
      if (delchi.le.1d-2.and.d.gt.Dc*1.5)delchi=1d-2*2d0**(delchi-1d-2)   !Delchi can go negative far from critical.
      if (delchi.le.0) RETURN
c  Function xi (Eq 46).  Olchowy, Vesovic put gam0 inside exponent, Krauss (R134a)
c   puts this term outside, but this yields incorrect values.     !Krauss' form - xi=xi0/gam0*delchi**(gnu/gamma)
      xi=xi0*(delchi/gam0)**(gnu/gamm)      !Vesovic form
      xomg =2d0/xPi*((Cp-Cv)/Cp*ATAN(qd*xi)+Cv/Cp*qd*xi)   !Functions omega and omega_zero (Eqs 59 and 60)
      xomg0=2d0/xPi*(1d0-EXP5(-1d0/(1d0/qd/xi+
     &      ((qd*xi*Dc/D)**2)/3d0)))
c  Factor of 1d9 below to convert from mol/L -> mol/m**3 and from uPa-s to Pa-s.
      call ETAK (i,T,D,eta,ierr,herr)
      if (ierr.ne.0) call TRNECS (T,D,x0,eta,ttcx,ierr,herr)    !If only have ECS for viscosity must call here.
c  Factor of 1d9 in next equation to convert from mol/L -> mol/m**3
c   and from micro-Pa-s to Pa-s.
      TCX3CR=D*Cp*R0*xBoltz*T*(xomg-xomg0)*XDIV(1d9,xPi*6d0*eta*xi)
      TCX3CR=TCX3CR*tcxred(i)     !Multiply by reducing parameter (to convert units, etc.).

      end                                                function TCX3CR
c
c ======================================================================
c
      function TCXBKold (icomp,ifunc,T,D)
c
c  Old thermal conductivity routines for the background function.

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension x(ncmax)

      dimension B0(7),B1(7),B2(7),B3(7),BB(7)
      data B0/2.41657d0,-0.50924d0,6.61069d0,14.5425d0,0.79274d0,
     &       -5.8634d0,81.171d0/
      data B1/0.74824d0,-1.50936d0,5.62073d0,-8.91387d0,
     &        0.82019d0,12.8005d0,114.158d0/
      data B2/-0.91858d0,-49.9912d0,64.7599d0,-5.63794d0,-0.69369d0,
     &        9.58926d0,-60.841d0/
      data B3/121.721d0,69.9834d0,27.0389d0,74.3435d0,6.31734d0,
     &       -65.52920d0,466.775d0/

      TCXBKold=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp

      if (ifunc.eq.2) then
c  Background contribution to the thermal conductivity with the model of
c    Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987; Eqs 26, 28-30.
c  The powers given in the JPCRD article are incorrect for Eqs 29 and 30.
c    They should be (4-n) and (7-n), respectively.
c    There is an incorrect sign in Eq 26 [(1 + F2*D), not (1 - F2*D)]
c  Model is no longer used.  See other routines for def. of inputs and outputs.
        tinv=1d0/T    !Compute functions given as Eqs 28-30.
        tinv2=tinv*tinv
        F0=ctcx(i, 6,1)+ctcx(i, 7,1)*tinv+ctcx(i, 8,1)*tinv2
        F1=ctcx(i, 9,1)+ctcx(i,10,1)*tinv+ctcx(i,11,1)*tinv2
        F2=ctcx(i,12,1)+ctcx(i,13,1)*tinv
        TCXBKold=(F0+F1*D)*D/(1d0+F2*D)     !Background term is given by Eq 26 in Younglove and Ely.

      elseif (ifunc.eq.3) then
c  Residual contribution to the thermal conductivity with the model of
c    Younglove, B.A. (1982). Thermophysical properties of
c    fluids. I. Argon, ethylene, parahydrogen, nitrogen, nitrogen trifluoride,
c    and oxygen. J. Phys. Chem. Ref. Data, Volume 11, Supplement 1.
c  Although this correlation has a separate initial density term,
c    the initial density term is combined with the residual term.
c  Model is no longer used.  See other routines for def. of inputs and outputs.
c  Compute the residual thermal conductivity (thermal conductivity minus
c   the dilute gas and initial density terms).
        if (D.le.0) RETURN
        tcx1=D*(ctcx(i,13,1)+ctcx(i,14,1)*            !Initial density term for thermal conductivity.
     &       (ctcx(i,15,1)-XLOG(T/ctcx(i,16,1)))**2)  !Eq 21
        G=ctcx(i,17,1)+ctcx(i,18,1)/T                 !Eq 23
        H=SQRT(D)*(D-ctcx(i,24,1))/ctcx(i,24,1)       !Eq 25
        F=G+(ctcx(i,19,1)+ctcx(i,20,1)/T**1.5d0)*D**0.1d0+
     &    (ctcx(i,21,1)+ctcx(i,22,1)/T+ctcx(i,23,1)/(T*T))*H
        tcx2=EXP5(F)-EXP5(G)                          !Eq 22
        x(1:nc)=0d0
        x(i)=1d0
        call DPDDX (T,D,x,dPdD)
        call DPDTX (T,D,x,dPdT)
        dPdD=dPdD/wmas(i)*1d7
        dPdT=dPdT*1d4
        ff=ctcx(i,25,1)
        rm=ctcx(i,26,1)
        bk=1.38054d-16
        dd=D*wmas(i)/1000d0
        call ETAK (i,T,D,eta,ierr,herr)
        bl=ff*SQRT(rm**5*dd*xAvog/wmas(i)*ctcx(i,2,1)/T)
        y=6d0*xPi*eta/1d5*bl*SQRT(bk*T*dd*xAvog/wmas(i))
        dl=0d0
        if (dPdD.ge.0) dl=bk*(T*dPdT)**2/SQRT(dd*dPdD)/y
        tcx3=dl*EXP5(-4.25d0*((D-Dcrit(i))/Dcrit(i))**4-
     &       18.66d0*((T-Tcrit(i))/Tcrit(i))**2)/1d5
        TCXBKold=tcx1+tcx2+tcx3

      elseif (ifunc.eq.5) then
c  Residual contribution to the thermal conductivity with the model of
c    Chung, Ajlan, Lee, and Starling, Ind. Eng. Chem. Res. 27:671-679, 1988.
c    See also Reid, Prausnitz, and Poling, Chapter 9.
c  This version allows the parameters for the residual piece to
c    be independent from the dilute gas and fit as free parameters.
c  Model is no longer used.  See other routines for def. of inputs and outputs.
c  Nomenclature for comparison with manuscript
c        sigchk(i)=cctcx(i,1,1)
c        epschk(i)=cctcx(i,1,2)
c        acchk(i)=cctcx(i,1,3)
c        ddipk(i)=cctcx(i,1,4)
c        hbkk(i)=cctcx(i,1,5)
        if (D.le.0) RETURN
        Cv=CP0K(i,T)-xRgas
        call ETAK00 (i,T,eta00,ierr,herr)
        vc=(cctcx(i,1,1)*10d0/0.809d0)**3        !Compute vc in cm3/mol from Chung sigma.
        y=D*vc/6d3
        BB(1:7)=B0(1:7)+B1(1:7)*cctcx(i,1,3)+B2(1:7)*cctcx(i,1,4)**4+
     &          B3(1:7)*cctcx(i,1,5)
        g1=(1d0-y/2d0)/(1d0-y)**3
        g2=(BB(1)/y*(1d0-EXP5(-BB(4)*y))+BB(2)*g1*EXP5(BB(5)*y)+
     &      BB(3)*g1)/(BB(1)*BB(4)+BB(2)+BB(3))
        alpha=Cv/xRgas-1.5d0
        beta=0.7862d0-0.7109d0*cctcx(i,1,3)+1.3168d0*cctcx(i,1,3)**2
        tr=T/(1.2593d0*cctcx(i,1,2))
        z=2d0+10.5d0*tr**2
        psich=1d0+
     &      alpha*(0.215d0+0.28288d0*alpha-1.061d0*beta+0.26665d0*z)/
     &            (0.6366d0+beta*z+1.061d0*alpha*beta)
        qq=3.586d-3*
     &     SQRT(1.2593d0*cctcx(i,1,2)/(wmas(i)/1d3))/(vc**thrd2)
        TCXBKold=31.2d0*eta00*1d-3*psich*(1d0/g2+BB(6)*y)/wmas(i)+
     &         qq*BB(7)*y**2*g2*SQRT(tr)
        TCXBKold=TCXBKold-tcx0ch
      endif

      end                                              function TCXBKold
c
c ======================================================================
c
      function TCXCRold (icomp,ifunc,icr2,T,D)
c
c  Old thermal conductivity routines for the critical enhancement function.
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-24-97  MM, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension c(mxtcx),x(ncmax)

      TCXCRold=0d0
      if (T.le.0. .or. D.le.1d-6) RETURN
      i=icomp

      if (ifunc.eq.1) then
c  Model #1 for the thermal conductivity critical enhancement
c    the empirical model used by Perkins and Laesecke
c  Compute the various parts of the critical enhancement.
c  These are taken in the order
c    rational polynomial in T, D (first numerator, then denominator)
c    exponential term
c    spare for future use
c  The coefficients themselves are given in the order
c    constant multiplier
c    additive term to temperature, exponent for (T + Tadd)
c    additive term to density, exponent for (D + Dadd)
c    spare 1 (if = 99 for denominator take MAX(T, T+Tadd)
c    spare 2
        nterm=0
        if (nnumtck(i).ge.1) then
          tau=T/Trtck(i)
          if (Trtck(i).le.0.) tau=-Trtck(i)/T    !Negative Trtck indicates reverse order.
          del=D/Drtck(i)
          if (nnumtck(i).ge.1) then
            do j=nterm+1,nterm+nnumtck(i)        !Numerator terms
              TCXCRold=TCXCRold+ctck(i,j,1)*(tau+ctck(i,j,2))**
     &              ctck(i,j,3)*(del+ctck(i,j,4))**ctck(i,j,5)
            enddo
            nterm=nterm+nnumtck(i)
          endif
        endif
        if (nexptck(i).ge.1) then
          tau=T/Tredex(i)
          if (Tredex(i).le.0.) tau=-Tredex(i)/T  !Negative Tred indicates reverse order.
          del=D/Dredex(i)
          xexp=0d0
          do j=nterm+1,nterm+nexptck(i)          !Exponential terms
            xexp=xexp+ctck(i,j,1)*(tau+ctck(i,j,2))**itck(i,j,2)*
     &           (del+ctck(i,j,3))**itck(i,j,3)
          enddo
          TCXCRold=TCXCRold*EXP5(xexp)
        endif
        TCXCRold=TCXCRold*tcxred(i)              !Multiply by reducing parameter (to convert units, etc.).

      elseif (ifunc.eq.2) then
c  Critical enhancement to the thermal conductivity with the model of
c    Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987; Eqs D1-D4
c  There are numerous errors in the equations presented in the
c    Younglove and Ely paper.  The present code is derived from the
c    code of Younglove used to generate the tables in JPCRD.
c  Model is no longer used.  See other routines for def. of inputs and outputs.
c  The array ctcx contains, in order
c       1-2 - Lennard-Jones sigma and epsilon/kappa
c         3 - constant in Eq 19 (5/16*(k*MW/1000/pi/Na)**0.5*1d12)
c       4-5 - dilute gas terms, Gt(1), Gt(2)
c      6-13 - background terms, Et(1) - Et(8)
c     14-17 - critical enhancement terms, X(1) - X(4)
c        18 - critical enhancement term  Z
c        19 - Boltzmann's constant, k
c  And the following terms from the Younglove and Ely viscosity model
c     20-23 - initial density dependence terms, Fv(1) - Fv(4)
c     24-30 - residual viscosity terms, Ev(1) - Ev(7)
        c=ctcx(i,1:mxtcx,1)
        e4=4d0
        if (icr2.eq.4) then
          e4=2d0
          c(14)=ctck(i,4,1)  !Set up parameters for old TK4 model.
          c(15)=ctck(i,5,1)
          c(16)=ctck(i,3,1)
          c(17)=ctck(i,1,1)
          c(18)=ctck(i,6,1)
          c(19)=ctck(i,2,1)*1d18
        endif
        x(1:nc)=0d0
        x(i)=1d0
        call DPDDX (T,D,x,dPdD)
        call DPDTX (T,D,x,dPdT)
        xi=0d0
        if (dPdD.gt.0.) xi=(Pcrit(i)*D/Dcrit(i)**2/dPdD)**c(16)       !Eq D3
        dellam=c(17)*c(19)/Pcrit(i)*(T*dPdT*Dcrit(i)/D)**2*xi*1d3     !Eq D2
        delD=D/Dcrit(i)-1d0
        eterm=c(14)*ABS(T/Tcrit(i)-1d0)**e4+c(15)*ABS(delD)**4
        eterm=EXP5(-eterm)
        if (icr2.eq.4) then
          call ETAK (i,T,D,visc,ierr,herr)
        else
          eta0=c(3)*SQRT(T)/(OMEGA(i,T,c(2),hmdtcx(i))*c(1)**2) !Dilute gas viscosity contribution.
          eta1=D*(c(20)+c(21)*(c(22)-XLOG(T/c(23)))**2)         !Initial density term for viscosity, Eq 21
          G=c(24)+c(25)/T                                       !Eq 23
          F=G+(c(26)+c(27)/T**1.5d0)*D**0.1d0+
     &      (c(28)+c(29)/T+c(30)/T**2)*SQRT(D)*delD
          eta2=EXP5(F)-EXP5(G)                                  !Residual viscosity (viscosity minus the dilute gas and initial density terms), Eq 22
          visc=(eta0+eta1+eta2)*1d-6
        endif
        TCXCRold=dellam*eterm/(6d0*xPi*c(18)*visc)*tcxred(i)    !Critical enhancement (the denominator is missing from the Younglove and Ely paper).
      endif

      end                                              function TCXCRold
c
c ======================================================================
c
      function TCXH2O (icomp,T,D)
c
c  Thermal conductivity model for water.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCXH2O--Thermal conductivity [W/m-K]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-07-00 EWL, original version
c  05-10-08 MLH, fix bug dreta instead of dr, allow for new vis model

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension x(ncmax)

      TCXH2O=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      tr=T/Trdbkt(i)
      dr=D/Drdbkt(i)
      tau=1d0/tr-1d0
      del=dr-1d0
      if (ABS(tau).lt.1d-12) tau=1d-12
      if (ABS(del).lt.1d-12) del=1d-12
      j=ndgnum(i)
      k=ndgnum(i)+nbknum(i)
      s1=SUM(ctcx(i,1:j,1)/tr**ctcx(i,1:j,2))
      s2=SUM(ctcx(i,j+1:k,1)*tau**INT(ctcx(i,j+1:k,2))*
     &                       del**INT(ctcx(i,j+1:k,3)))
      tcx1=1d10
      tcx0=SQRT(tr)/s1
      tcx1=EXP5(dr*s2)
      treta=T/Tredeta(i)          !Calculate water viscosity.
      dreta=D/Dredeta(i)
      taueta=1d0/treta-1d0
      deleta=dreta-1d0            !Bug fix, was dr
      if (ABS(taueta).lt.1d-12) taueta=1d-12
      if (ABS(deleta).lt.1d-12) deleta=1d-12
      j=ndel0(i)
      k=ndel0(i)+npoly(i)
      s1=SUM(ceta(i,1:j,1)/treta**ceta(i,1:j,2))
      s2=SUM(ceta(i,j+1:k,1)*taueta**INT(ceta(i,j+1:k,2))*
     &                       deleta**INT(ceta(i,j+1:k,3)))
      eta1=xEq0(EXP5(dreta*s2))
      eta0=SQRT(treta)/s1
      if (VerFL(i).gt.8.0) eta0=eta0/55.071d-2   !Allow new viscosity model.
      tcx2=0d0
      if (VerFL(i).le.8.1) then   !Critical region contribution to Tcx; skip this beyond version 8.1 - use TK3.
        x(1:nc)=0d0
        x(i)=1d0
        call DPDDX (T,D,x,dPdD)
        call DPDTX (T,D,x,dPdT)
        if (dr.gt.0. .and. dPdD.gt.0.) then
          x0=dr/dPdD/Drdbkt(i)*22115d0
          tcx2=0.0013848d0*(T/dr)**2/eta0/eta1*(dPdT/22115d0)**2*
     &         x0**0.4678d0*SQRT(dr)*EXP5(-18.66d0*(tr-1d0)**2-del**4)
        endif
      endif
      TCXH2O=tcxbkt(i)*(tcx0*tcx1+tcx2)

      end                                                function TCXH2O
c
c ======================================================================
c
      function TCCR_2010 (T,D,QD)
c
c  Critical enhancement term for water, based on simplified Olchowy Sengers.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c       qd--Cutoff wave number [m]
c
c  Output:
c     TCCR--The critical enhancement to the thermal conductivity [W/m-K]
c
c  09-30-10 MLH, based on code from R. Perkins
c  08-20-13 MLH, move call to THERM before call to xi_fun to make use of stored calculations
c  04-10-14 MLH, fix bug when water is a pure fluid in a mixture calculation

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension x(ncmax)

c  If water is in the mixture, find the component number and set others to zero.
      x(1:nc)=0d0
      iww=1
      if (nc.ne.1) then
        do j=1,nc
          if (iFluidID(j).eq.1) iww=j       !Water
        enddo
      endif
      x(iww)=1d0
      TCCR_2010=0d0
      if (T.le.0. .or. D.lt.1d-6) RETURN    !Critical enhancement is zero for low densities (avoid divide by zero).
      xi=XI_FUN(T,D,iww)                    !Calculate correlation length xi in m.
      if (xi.le.0.) RETURN
      call THERM (T,D,x,P,e,h,s,Cv,Cp,w,hjt)!Calculate Cv and Cp at T and D.
      call ETAK (iww,T,D,eta,ierr,herr)     !eta is in uPa-s
      tau=T/Tcrit(iww)
      del=D/Dcrit(iww)
      cpr=Cp/wmas(iww)/0.46151805d0         !Cp/R
      rkappa=Cp/Cv
      yval=qd*xi
      if (xi.gt.1.5d-12) then               !Smooth transition at low xi.
        xomg =2d0/xPi*((1d0-1d0/rkappa)*ATAN(yval)+yval/rkappa)
        xomg0=2d0/xPi*(1d0-EXP5(-1d0/(1d0/yval+(yval/del)**2/3d0)))
        z=(xomg-xomg0)/yval
      else
        z=2d0/xPi*(yval/2d0+yval**2*(thrd/rkappa-0.5d0)+
     &    yval**3*(thrd/del**2+1d0/24d0)-
     &    yval**4*(thrd/del**2+1d0/5d0/rkappa-3d0/120d0)+
     &    yval**5*(1d0/6d0/del**2+1d0/270d0))
      endif
      TCCR_2010=177.8514d0*del*cpr*tau/eta*z*1d-3

      end                                             function TCCR_2010
c
c ======================================================================
c
      function TCXM1C (T,D,z,ierr,herr)
c
c  Calculate critical enhancement portion needed for mixtures.
c
c  Based on the simplified critical enhancement of
c  Olchowy and Sengers, Int. J. Thermophys., 10:417-426, 1989.
c
c  Also applied to CO2 by
c  Vesovic, Wakeham, Olchowy, Sengers, Watson, and Millat
c  J. Phys. Chem. Ref. Data, 19:763-808, 1990.
c  Equation numbers in comments refer to Vesovic paper
c
c  Model has been adapted to use for a mixture.  All coefficients except qd are
c  fixed. qd is a mole fraction sum of the components qd's. If qd has not been supplied,
c  a value of 0.5d-9 is used. The actual mixture T and D (not a scaled value) is used.
c
c  This routine is designed to be called from TRNS_ECS.FOR. The routine TRNS_ECS.FOR is
c  used to supply the viscosity of the fluid at T and D by placing it in
c  common block CRITENH before a call to this subroutine is made.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition array (array of mole fractions)
c
c  Output (as the function value):
c   TCXM1C--The critical enhancement to the thermal conductivity [W/m-K]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  06-08-97  MM, original version
c  06-13-06 EWL, modify how test for pure fluids is done
c  10-06-08 MLH, prevent ridiculous numbers for qd in TK6 model
c  06-01-11 EWL, remove common block SHAPES which was not used
c  04-18-14 MLH, catch error when div of near zero due to xomg and xomg0 almost the same

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax),x0(ncmax)

      ierr=0
      herr=' '
      TCXM1C=0d0
      if (T.le.0. .or. D.lt.1d-6 .or. ctck(1,8,1).eq.0.) RETURN  !Critical enhancement is zero for low densities (avoid divide by zero).
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        x0(1:nc)=0d0
        x0(icomp)=1d0
      else
        x0(1:nc)=z(1:nc)
      endif

c  Check to see if it is exactly at critical point of a pure fluid
      if (icomp.ne.0) then
        if (ABS(T-Tcrit(icomp)).lt.1d-20 .and.
     &      ABS(D-Dcrit(icomp)).lt.1d-20) then
          call ERRNUM (-560,0,' ',' ',0d0,0d0,0d0,ierr,herr)     !Do not change from -560, this is used as a check elsewhere in the code.
        endif
      endif

c  Set up the coefficients.
      qd   =0d0
      R0   =0d0
      xi0  =0d0
      gnu  =0d0
      gam0 =0d0
      gamm =0d0
      treff=0d0

      do i=1,nc
        if (x0(i).gt.0.) then
          if (htcxcr(i).eq.'TK3' .or. htcxcrecs(i).eq.'TK3') then    !Use coefficient loaded for TK3 model
            qd   =qd   +XDIV(x0(i),ctck(i,8,1))  !qd_inverse (cutoff dia, order 10d-9 m)
            R0   =R0   +x0(i)*ctck(i,3,1)        !R0 (universal amplitude, 1.01 +/- 0.04)
            xi0  =xi0  +x0(i)*ctck(i,6,1)        !xi0 (amplitude, order 1d-10 m)
            gnu  =gnu  +x0(i)*ctck(i,1,1)        !nu (universal exponent, approximately 0.63)
            gam0 =gam0 +x0(i)*ctck(i,7,1)        !gam0 (amplitude, order 0.05 - 0.06)
            gamm =gamm +x0(i)*ctck(i,2,1)        !gamma (universal exponent, approximately 1.24)
            treff=treff+x0(i)*ctck(i,9,1)        !treff (reference temperature, 1.5 - 2.0 * Tc)
c  The following two parameters not used, but are in file for future use
c           zu   =zu   +x0(i)*ctck(i,4,1)        !zu (universal exponent, 0.065 +/- 0.005)
c           cvisc=cvisc+x0(i)*ctck(i,5,1)        !viscosity constant, approximately 1.075, but often 1)

          elseif (htcxcrecs(i).eq.'TK6') then    !coefficient for ECS enhancement
            qd   =qd   +XDIV(x0(i),ctcke(i,8,1)) !qd_inverse (cutoff dia, order 10d-9 m)
            R0   =R0   +x0(i)*ctcke(i,3,1)       !R0 (universal amplitude, 1.01 +/- 0.04)
            xi0  =xi0  +x0(i)*ctcke(i,6,1)       !xi0 (amplitude, order 1d-10 m)
            gnu  =gnu  +x0(i)*ctcke(i,1,1)       !nu (universal exponent, approximately 0.63)
            gam0 =gam0 +x0(i)*ctcke(i,7,1)       !gam0 (amplitude, order 0.05 - 0.06)
            gamm =gamm +x0(i)*ctcke(i,2,1)       !gamma (universal exponent, approximately 1.24)
            treff=treff+x0(i)*ctcke(i,9,1)       !treff (reference temperature, 1.5 - 2.0 * Tc)
                                                 !See above concerning zu and cvisc.
c  Set to generalized values, see Vesovik et al., J. Phys. Chem. Ref. Data, 19(3):762-808, 1990.
          else
            qd   =qd   +x0(i)/0.5d-9
            R0   =R0   +x0(i)*1.03d0
            xi0  =xi0  +x0(i)*1.94d-10
            gnu  =gnu  +x0(i)*0.63d0
            gam0 =gam0 +x0(i)*0.0496d0
            gamm =gamm +x0(i)*1.239d0
            treff=treff+x0(i)*1.5d0*Tcrit(i)
          endif
        endif
      enddo

      call CRITP (x0,Tc,Pc,Dc,ierr1,herr)
      call DPDDX (T,D,x0,dPdD)
      call DPDDX (treff,D,x0,dPdDr)
      call THERM (T,D,x0,pp,e,h,s,Cv,Cp,w,hjt)

c  Function chi (Eq 40 in Vesovic) evaluated at T,D and tref,D.  (Vesovic introduces a T/Tc term which is absent in other papers.)
      chi   =XDIV(Pc*D,Dc**2*dPdD)          !Vesovic form:  chi=Pc/(Dc*Dc*Tc)*D*T/dPdD
      chiref=XDIV(Pc*D,Dc**2*dPdDr)*treff/T !Vesovic form:  chiref=Pc/(Dc*Dc*Tc)*D*tref/dPdD*tref/T
      delchi=chi-chiref
      if (delchi.le.0.) RETURN              !delchi can go negative far from critical

c  Function xi (Eq 46).  Olchowy, Vesovic put gam0 inside exponent, Krauss (R134a)
c   puts this term outside, but this yields incorrect values.  Krauss' form - xi=xi0/gam0*delchi**(gnu/gamma)
      xi=xi0*XPOWR(delchi/gam0,gnu/gamm)    !Vesovic form
      Cpinv=XDIV(1d0,Cp)
      xomg =2d0/xPi*((Cp-Cv)*Cpinv*ATAN(qd*xi)+Cv*Cpinv*qd*xi)  !Functions omega and omega_zero (Eqs 59 and 60)
      xomg0=2d0/xPi*(1d0-EXP5(-1d0/(XDIV(1d0,qd*xi)+
     &      ((qd*xi*Dc/D)**2)/3d0)))

c  etacal is passed in through common block CRITENH, it is computed in TRNS_ECS.FOR with ECS code.
      eta=etacal        !For TK6 and general coefficient model
      if (icomp.ne.0 .and. eta.eq.0d0) then
        call ETAK (icomp,T,D,eta,ierr,herr) !Use recommended pure fluid correlation instead of ECS.
        if (ierr.ne.0) call TRNECS (T,D,x0,eta,ttcx,ierr,herr)  !If only have ECS for viscosity must call here.
      endif
c  Factor of 1d9 below to convert from mol/L -> mol/m**3 and from uPa-s to Pa-s.
      TCXM1C=D*1d9*Cp*xBoltz*T*(xomg-xomg0)*XDIV2(R0,xPi*6d0*eta*xi)

      end                                                function TCXM1C
c
c ======================================================================
c
      subroutine SETTCX (nread,icomp,htype,ierr,herr)
c
c  Setup the pure fluid thermal conductivity models.
c
c  TC0:
c  Initialize pure fluid thermal conductivity model #0, the model
c  that points to all the hardcoded equations.
c
c  TC1:
c  Initialize pure fluid thermal conductivity model #1.  This is the
c  composite model and written in a general form with terms designed
c  to include several recent correlations including those of Tufeu (1984)
c  for ammonia, Krauss (1996) for R152a, and Laesecke (1996) for R123.
c
c  TC2:
c  Initialize pure fluid thermal conductivity model #2, the hydrocarbon model of
c  Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987.
c  Younglove and Ely use a special scaled equation of state to compute derivatives
c  for the critical enhancement, the default EOS is used here.
c
c  TC3:
c  Initialize pure fluid thermal conductivity model #3, the model of
c  Younglove, J. Phys. Chem. Ref. Data, Volume 11, Supplement 1, 1982.
c  Model is no longer used.
c
c  TC5:
c  Initialize pure fluid thermal conductivity model #5, the model of
c  Chung, Ajlan, Lee, and Starling, Ind. Eng. Chem. Res. 27:671-679, 1988.
c  See also Reid, Prausnitz, and Poling, Chapter 9.
c  Model is no longer used.
c
c  TC7:
c  Pure fluid thermal conductivity model #7, which uses reverse Polish
c  notation (see helium.fld for a great example) to allow any functional
c  form to be used for the calculation.  This prevents new forms
c  from having to be hardcoded individually, and prevents the
c  need for a new DLL every time a correlation becomes available.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in mixture (0..nc)
c           1 for pure fluid; 0 for ECS reference fluid
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-06-97  MM, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr,htype*3
      dimension z(ncmax)

      ierr=0
      herr=' '
      if (nread.eq.0) RETURN
      l0=.false.
      l1=.false.
      l2=.false.
      l3=.false.
      l5=.false.
      l7=.false.
      if (htype(3:3).eq.'0') l0=.true.
      if (htype(3:3).eq.'1') l1=.true.
      if (htype(3:3).eq.'2') l2=.true.
      if (htype(3:3).eq.'3') l3=.true.
      if (htype(3:3).eq.'5') l5=.true.
      if (htype(3:3).eq.'7') l7=.true.

c  Read data from file.
      i=icomp
      jterm=0
      read (nread,*,err=102) Tmtcx(i)     !Lower temperature limit
      read (nread,*,err=102) Txtcx(i)     !Upper temperature limit
      read (nread,*,err=102) Pxtcx(i)     !Upper pressure limit
      read (nread,*,err=102) Dxtcx(i)     !Upper density limit
      htcxcr(i)=' '

      if (l7) then
        hmdtcx(i)=' '
        z(1:nc)=0d0
        z(icomp)=1d0
        oi=RPNEQN(-2,icomp,nread,0d0,0d0,z,0d0,ierr,herr)
        call READHA (nread,3,htcxcr(i))     !Pointer to critical enhancement model.
        RETURN
      endif

      if (l0) then
        call READHA (nread,3,htcxhc(i))     !Pointer to the hardcoded model.
        read (nread,*,err=102) ndgnum(i),ndgden(i),nbknum(i),nbkden(i)
        nrsum=ndgnum(i)+ndgden(i)+nbknum(i)+nbkden(i)
        if (nrsum.ge.1) then
          read (nread,*,err=102) Trdbkt(i),Drdbkt(i),tcxbkt(i)     !Reducing parameters.
          do j=1,nrsum
            read (nread,*,err=102) (ctcx(i,j,k),k=1,4),itcx(i,j)
          enddo
        endif
        call READHA (nread,3,htcxcr(i))     !Critical enhancement model.
        RETURN
      endif

      if (l2) then
        call READHA (nread,3,hmdtcx(i))!Pointer to omega model
        read (nread,*,err=102) ctcx(i,1,1)     !L-J sigma
        read (nread,*,err=102) ctcx(i,2,1)     !L-J epsilon/kappa
        read (nread,*,err=102) ctcx(i,3,1)     !Constant in Eq 19 = 5/16*(k*MW/1000/pi/Na)**0.5*1d12.
        read (nread,*,err=102) ctcx(i,4,1)     !Dilute-gas terms: Gt(1)-Gt(2)
        read (nread,*,err=102) ctcx(i,5,1)
        do j=6,13
          read (nread,*,err=102) ctcx(i,j,1)   !Background terms: Et(1)-Et(8)
        enddo
        call READHA (nread,3,htcxcr(i))
        if (htcxcr(i).ne.'NUL') then   !Critical enhancement is combined with the model for the dilute gas and background contributions.
          do j=14,17
            read (nread,*,err=102) ctcx(i,j,1) !Critical enhancement: X(1)-X(4)
          enddo
          read (nread,*,err=102) ctcx(i,18,1)  !Critical enhancement: Z
          read (nread,*,err=102) ctcx(i,19,1)  !Critical enhancement: k
          do j=20,23                   !Following coefficients are for the viscosity function of Younglove and Ely
            read (nread,*,err=102) ctcx(i,j,1) !Initial D terms:  Fv(1)-Fv(4)
          enddo
          do j=24,30
            read (nread,*,err=102) ctcx(i,j,1) !Residual viscosity:  Ev(1)-Ev(7)
          enddo
        endif
        RETURN
      endif

      if (l3) then
        read (nread,*,err=102) ctcx(i,1,1)     !L-J sigma
        read (nread,*,err=102) ctcx(i,2,1)     !L-J epsilon/kappa
        read (nread,*,err=102) ctcx(i,3,1)     !Leading coefficient
        read (nread,*,err=102) ctcx(i,3,2)     !Exponent on tau
        do j=4,12                      !Dilute gas thermal conductivity terms.
          read (nread,*,err=102) ctcx(i,j,1)
        enddo
        do j=13,24                     !Residual thermal conductivity terms.
          read (nread,*,err=102) ctcx(i,j,1)
        enddo
        read (nread,*,err=102) ctcx(i,25,1)    !F
        read (nread,*,err=102) ctcx(i,26,1)    !rm
        call READHA (nread,3,htcxcr(i))!Pointer to critical enhancement model.
        RETURN
      endif

      if (l5) then
        read (nread,*,err=102) sigchk(i)       !L-J sigma for Chung k model
        read (nread,*,err=102) epschk(i)       !L-J epsilon/kappa for Chung k model
        read (nread,*,err=102) acchk(i),ddipk(i),hbkk(i) !w, dip, kappa for Chung dilute
        cctcx(i,1,1)=sigchk(i)         !Set residual parameters.
        cctcx(i,1,2)=epschk(i)
        cctcx(i,1,3)=acchk(i)
        cctcx(i,1,4)=ddipk(i)
        cctcx(i,1,5)=hbkk(i)
        read (nread,*,err=102) naddk(i)        !Additional factors for Chung
        do j=1,naddk(i)                !Use these terms if residual parameters are not the same as dilute gas.
          read (nread,*,err=102) cctcx(i,j,1),cctcx(i,j,2),
     &                   cctcx(i,j,3),cctcx(i,j,4),cctcx(i,j,5)
        enddo
        call READHA (nread,3,htcxcr(i))!Pointer to critical enhancement model.
        RETURN
      endif

c  Read the number of terms in the numerator and denominator of the
c   dilute-gas function.
      read (nread,*,err=102) ndgnum(i),ndgden(i)
      if (ndgnum(i).ge.1) then
        read (nread,*,err=102) Trddgt(i),tcxdgt(i)  !Reducing parameters
        do j=1,ndgnum(i)                    !Dilute-gas terms (numerator).
          jterm=jterm+1
          read (nread,*,err=102) ctcx(i,jterm,1),ctcx(i,jterm,2)
        enddo
      endif
      if (ndgden(i).ge.1) then
        do j=1,ndgden(i)          !Dilute-gas terms (denominator).
          jterm=jterm+1
          read (nread,*,err=102) ctcx(i,jterm,1),ctcx(i,jterm,2)
        enddo
      endif

c  Read the number of terms in the numerator and denominator of the
c   background model.  The coefficients themselves are given in the order
c   constant multiplier, temperature exponent, density exponent, spare.
      read (nread,*,err=102) nbknum(i),nbkden(i)
      nbksum=nbknum(i)+nbkden(i)
      if (nbksum.ge.1) then  !Reducing parameters.
        read (nread,*,err=102) Trdbkt(i),Drdbkt(i),tcxbkt(i)
        if (nbknum(i).ge.1) then
          do j=1,nbknum(i)   !Numerator of rational polynomial
            jterm=jterm+1
            read (nread,*,err=102) (ctcx(i,jterm,k),k=1,4)
          enddo
        endif
        if (nbkden(i).ge.1) then
          do j=1,nbkden(i)   !Denominator of rational polynomial
            jterm=jterm+1
            read (nread,*,err=102) (ctcx(i,jterm,k),k=1,4)
          enddo
        endif
      endif
      call READHA (nread,3,htcxcr(i))       !Pointer to critical enhancement model.
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                              subroutine SETTCX
c
c ======================================================================
c
      subroutine SETTCE (nread,icomp,htype,ierr,herr)
c
c  Initialize the thermal conductivity critical enhancement models.
c
c  TK3, TK6:
c  Vesovic, Wakeham, Olchowy, Sengers,  Watson, and Millat
c  J. Phys. Chem. Ref. Data, 19:763-808, 1990.
c
c  TK7:
c  Reverse Polish notation model.
c
c  TK1:
c  Old model used only with R-123.
c
c  TK2:
c  Old hydrocarbon model of Younglove and Ely (no longer in use).
c
c  TK4:
c  Old model no longer in use.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in mixture (0..nc)
c           1 for pure fluid; 0 for ECS reference fluid
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  06-08-97  MM, original version
c  07-19-16 EWL, delete SETTK1 and SETTK4 and combine with SETTK3 by adding input variable itk3

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character herr*255,htype*3
      dimension z(ncmax)

      ierr=0
      herr=' '
      if (nread.eq.0) RETURN

c  Read data from file.
      i=icomp
      read (nread,*,err=102) Tmtck(i)     !Lower temperature limit
      read (nread,*,err=102) Txtck(i)     !Upper temperature limit
      read (nread,*,err=102) Pxtck(i)     !Upper pressure limit
      read (nread,*,err=102) Dxtck(i)     !Upper density limit

c  Initialize model #6 for the thermal conductivity critical enhancement
c    of Vesovic et al., 1990.
c  Only the ctcke parameters are needed, nothing else is saved into common
c    blocks in order to avoid overwriting any TK3 parameters loaded
c    previously for dedicated models.
      if (htype.eq.'TK6') then
        read (nread,*,err=102) ntck,ndck  !Number of terms
        if (ntck+ndck.ge.1) then
          read (nread,*,err=102) trt,Drt  !Reducing parameters
          if (ntck.ge.1) then
            do j=1,ntck
              read (nread,*,err=102) ctcke(icomp,j,1)         !Critical enhancement terms
            enddo
          endif
        endif
        RETURN
      endif

      if (htype.eq.'TK7') then
        z(1:nc)=0d0
        z(icomp)=1d0
        oi=RPNEQN(-3,icomp,nread,0d0,0d0,z,0d0,ierr,herr)
        RETURN
      endif

c  # terms in numerator and denominator of polynomial multiplier, and exponential term.
      read (nread,*,err=102) nnumtck(i),ndentck(i),nexptck(i)
      if (nnumtck(i).ge.1) then
        if (htype.eq.'TK4') then
          read (nread,*,err=102) Trtck(i),Prtck(i),Drtck(i),tcxred(i)   !Reducing parameters for TK4.
        else
          read (nread,*,err=102) Trtck(i),Drtck(i),tcxred(i)            !Reducing parameters for TK3 and TK1.
        endif
        if (nnumtck(i).ge.1) then
          do j=1,nnumtck(i)                                     !Numerator terms.
            if (htype.eq.'TK1') then
              read (nread,*,err=102) (ctck(i,j,k),k=1,5),itck(i,j,0)    !Parameters for TK1.
            else
              read (nread,*,err=102) ctck(i,j,1)                        !Parameters for TK3 and TK4.
            endif
          enddo
        endif
      endif

c  Extra code for the old TK1 model.
      if (htype.eq.'TK1') then
        jterm=nnumtck(i)
        if (nexptck(i).ge.1) then
          read (nread,*,err=102) Tredex(i),Dredex(i)
          do j=1,nexptck(i)       !Exponential terms.
            jterm=jterm+1
            read (nread,*,err=102) (ctck(i,jterm,k),k=1,2),
     &                              itck(i,jterm,2),ctck(i,jterm,3),
     &                              itck(i,jterm,3),itck(i,jterm,0)
          enddo
        endif
      endif
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)

c  As originally implemented for CO2, these terms are
c    ctck(i,1,1) = gnu (universal exponent, approximately 0.63)
c    ctck(i,2,1) = gamma (universal exponent, approximately 1.2145)
c    ctck(i,3,1) = R0 (universal amplitude, 1.01 +/- 0.04)
c    ctck(i,4,1) = z (universal exponent, 0.065 +/- 0.005)
c    ctck(i,5,1) = c (viscosity constant, approximately 1.075, but often set to 1)
c    ctck(i,6,1) = xi0 (amplitude, order 1d-10 m)
c    ctck(i,7,1) = gam0 (amplitude, order 0.05 - 0.06)
c    ctck(i,8,1) = qd_inverse (cutoff diameter, order 10d-9 m)
c    ctck(i,9,1) = tref (reference temperature, 1.5 - 2.0 * Tc)

      end                                              subroutine SETTCE
c
c ======================================================================
c
      function TCCNH3 (icomp,T,D)
c
c  Model for the thermal conductivity critical enhancement of ammonia
c  with the empirical model of Tufeu et al.
c  This is a special model only for ammonia with all constants hardwired.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid (not used)
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCCNH3--The critical enhancement to the thermal conductivity [W/m-K]
c
c  Written by S.A. Klein and M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-26-97  MM, original version (adapted from TCENHC of SAK)

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      TCCNH3=0d0
      if (T.le.0. .or. D.le.0.) RETURN
      Dkg=D*wmas(icomp)                          !Correlation in mass units
      tr=ABS(T-405.4d0)/405.4d0                  !405.4 is the old Tc
      trr=tr
      if (T.gt.404.4d0 .and. T.lt.406.5d0 .and.
     &   (D.lt.9.6d0 .or. D.gt.18d0)) trr=0.002d0
      etab=1d-5*(2.6d0+1.6d0*tr)                 !Viscosity - Eq 9
      dPdT=1d5*(2.18d0-0.12d0/EXP5(17.8d0*tr))   !dP/dT for D = Dc
      dtcid=1d20
      xcon=-1d20                                 !Eq 12
      if (ABS(trr).ge.1d-20) then
        tcrhoc=1.2d0*xBoltz*T**2*dPdT**2*0.423d-8/trr**1.24d0*  !Eq 9 of Tufeu for conductivity along critical isochore
     &    (1d0+1.429d0*SQRT(tr))/(6d0*xPi*etab*1.34d-10/
     &    trr**0.63d0*(1d0+SQRT(tr)))
        dtcid=tcrhoc*EXP5(-36d0*tr**2)           !Eq 10
        xcon=0.61d0*235d0+16.5d0*XLOG(trr)       !Eq 12
      endif
      if (D/Dcrit(icomp).lt.0.6d0) then
        tccsw=dtcid*xcon**2/(xcon**2+(141d0-0.96d0*235d0)**2)   !Eq 14 for D < 0.6*Dc (141 = 0.6*Dc)
        TCCNH3=tccsw*Dkg**2/141d0**2                            !SAK had 253 in denominator
      else
        TCCNH3=dtcid*xcon**2/(xcon**2+(Dkg-0.96d0*235d0)**2)    !Eq 11 for D > 0.6*Dc (SAK had 253)
      endif

      end                                                function TCCNH3
c
c ======================================================================
c
      function TCCCH4 (icomp,T,D)
c
c  Model for the thermal conductivity critical enhancement of methane.
c  with the empirical model of Friend et al. (1989).
c
c  This is a special model only for methane with all constants hardwired.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c           (not used - included to maintain parallel structure)
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCCCH4--The critical enhancement to the thermal conductivity [W/m-K]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-27-00 EWL, original version (adapted from TCENHC of SAK)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension x(ncmax)

      TCCCH4=0d0
      if (T.le.0. .or. D.le.0.) RETURN
      i=icomp
      x(1:nc)=0d0
      x(i)=1d0
      call ETAK (i,T,D,eta,ierr,herr)
      call DPDDX (T,D,x,dPdD)
      dPdT=1d0
      if (ABS(D).gt.1d-12) then
        call DPDTX (T,D,x,dPdT)
        dPdT=dPdT/xRgas/D
      endif
      tau=Tcrit(i)/T
      ts=1d0-T/Tcrit(i)
      ds=1d0-D/Dcrit(i)
      xt=0.28631d0*D/Dcrit(i)*Tcrit(i)/dPdD*xRgas
      if (xt.lt.0.) xt=1d5
      ftd=EXP5(-(2.646d0*SQRT(ABS(ts))+2.678d0*ds**2-0.637d0*ds))
      TCCCH4=91.855d0/eta/tau**2*dPdT**2*xt**0.4681d0*ftd*1d-3

      end                                                function TCCCH4
c
c ======================================================================
c
      function TCXH2 (icomp,T,D)
c
c  Model for the thermal conductivity of para and normal hydrogen.
c  Taken from NIST12, Version 3.1, and modified to work with Refprop.
c
c  Model is no longer used.  See other routines for def. of inputs and outputs.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-20-99 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension xf(ncmax),tnz(35),tpz(35),tt(35),g(3),e(8)
      data tnz/.0505,.0568,.0632,.0695,.0763,.0829,.0896,
     &  .0962,.1026,.1092,.1157,.1220,.1282,.1342,.1401,.1458,.1514,
     &  .1569,.1622,.1674,.1725,.1774,.1823,.1870,.1917,.1962,.2012,
     &  .2061,.2110,.2158,.2203,.2250,.2295,.2340,.2380/
      data tpz/.0529,.0617,.0714,.0816,.0924,.1028,.1125,
     &  .1213,.1294,.1365,.1427,.1482,.1530,.1574,.1614,.1651,.1687,
     &  .1723,.1758,.1793,.1828,.1863,.1899,.1935,.1972,.2010,.2048,
     &  .2088,.2129,.2169,.2208,.2249,.2290,.2330,.2370/
      data tt/70,80,90,100,110,120,130,140,150,160,170,180,190,200,210,
     &  220,230,240,250,260,270,280,290,300,310,320,330,340,350,360,370,
     &  380,390,400,410/
      data g/.1584312604d-02,.3861103193d-04,.1066433014d-06/

      TCXH2=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      xf=0d0
      xf(icomp)=1d0
      ther1=0d0
      if (T.le.90.) then
        dd=D
        call PRESS (T,D,xf,pe)
        if (pe.ge.12000.) call TPRHO (T,12000d0,xf,1,0,dd,ierr,herr)
        tf=T**thrd
        tff=T**(-4d0/3d0)
        s=0d0
        do j=1,9
          tff=tff*tf
          s=s+ctcx(icomp,j,1)*tff
        enddo
        excsh2=0d0
        if (D.gt.0.) then
          if (T.gt.80. .or. D.lt.0.001d0) then
            del=0.38611d-4+0.10664d-7*T
            excsh2=0.0015843d0*dd+del*(EXP5(2.1d0*dd**0.36d0)-1d0)
          else
            k=10
            e=ctcx(icomp,k+1:8,1)
            r2=0d0
            if ((dd-e(8))/e(8).lt.1000.) r2=dd**((dd-e(8))/e(8))
            x0=e(1)+e(6)/T+r2*(e(2)+e(4)/T**2+e(7)/T)+
     &         dd**0.1d0*(e(3)+e(5)/T**1.5d0)
            excsh2=(EXP5(x0)-EXP5(e(1)+e(6)/T))/10d0
          endif
        endif
        deld=dd-15.556d0-0.008229*ABS(T-32.938d0)**1.5d0
        rcrit=(6.35363d0-0.05863d0*T)*EXP5(-(0.138d0*deld)**2)
        if (rcrit.le.0.) rcrit=0d0
        ther1=s*100d0+excsh2*1000d0+rcrit
        ther1=EXP5(XLOG(ther1)+(D-dd)*0.041758537d0)
        TCXH2=ther1/1000d0
        if (T.lt.70.) RETURN
      endif
      dd=D
      call PRESS (T,D,xf,pe)
      if (pe.ge.60000.) call TPRHO (T,60000d0,xf,1,0,dd,ierr,herr)

c  The following was taken from NBSIR 84-3006, HM Roder, May 86.
      i=0
      do j=35,1,-1
        if (T.lt.tt(j)) i=j
      enddo
      tczn=0.2380d0+0.0045d0*(T-410d0)/10d0      !Correction by EWL to account
      tczp=0.2370d0+0.0040d0*(T-410d0)/10d0      ! for high temperatures.
      if (i.ne.0) then
        if (i.eq.1) i=2
        tczn=tnz(i-1)+(tnz(i)-tnz(i-1))*(T-tt(i-1))/(tt(i)-tt(i-1))
        tczp=tpz(i-1)+(tpz(i)-tpz(i-1))*(T-tt(i-1))/(tt(i)-tt(i-1))
      endif
      crith2=0d0
      if (T.gt.77d0 .and. T.lt.108.35658d0) then
        Dcen=15.556d0-0.008229d0*(T-32.938d0)**1.5d0
        crith2=(0.00635363d0-T*5.863d-5)*EXP5(-(0.138d0*(dd-Dcen))**2)
      endif
      fact=1d0-0.028484d0+0.000070588d0*T
      tcz=tczn*fact+(tczp-tczn)/0.75d0*(ctcx(icomp,10,1)-0.25d0)+crith2
      ther2=tcz+g(1)*dd+(g(2)+g(3)*T)*(EXP5(2.1d0*dd**0.36d0)-1d0)
      ther2=EXP5(XLOG(ther2)+(D-dd)*0.041758537d0)
      TCXH2=ther2
      if (T.lt.90.)
     &  TCXH2=ther1/1000d0*(1d0-(T-70d0)/20d0)+ther2*(T-70d0)/20d0

      end                                                 function TCXH2
c
c ======================================================================
c
      function TCXHE (icomp,T,D)
c
c  Calculate the thermal conductivity of helium with the model of
c  Hands, B.A. and Arp, V.D.  A Correlation of Thermal Conductivity Data
c  for Helium.  Cryogenics, 21(12):697-703 (1981).
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c
c  Output (as the function value):
c    TCXHE--Thermal conductivity [W/m-K]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  07-06-98 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension x(ncmax)

      TCXHE=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      s=SUM(ctcx(i,2:ndgnum(i),1)*(T/Trdbkt(i))**ctcx(i,2:ndgnum(i),2))   !Sum the dilute gas terms.
      tcxdg=ctcx(i,1,1)*(T/Trdbkt(i))**ctcx(i,1,2)*EXP5(s)
      tau=(T/Trdbkt(i))**thrd
      del=D/Drdbkt(i)
      tcxbk=0d0
      do j=1,nbknum(i)  !Sum the background terms.
        j1=j+ndgnum(i)
        if (ABS(ctcx(i,j1,4)).lt.1d-12 .or. ABS(del).lt.1d-12) then
          tcxbk=tcxbk+ctcx(i,j1,1)*tau**ctcx(i,j1,2)*del**ctcx(i,j1,3)
        else
          tcxbk=tcxbk+ctcx(i,j1,1)*tau**ctcx(i,j1,2)*del**ctcx(i,j1,3)*
     &          XLOG(del**ctcx(i,j1,4))
        endif
      enddo
      tcxcr=0d0         !Add the critical enhancement contribution.
      if (T.ge.3.5d0 .and. T.le.12.) then
        x0=0.392d0
        e1=2.8461d0
        e2=0.27156d0
        beta=1d0/0.3554d0
        gamm=0.1743d0
        delta=4.304d0
        dcc=69.158d0/wmas(i)
        bkt=0d0
        x(1:nc)=0d0
        x(i)=1d0
        call ETAK (i,T,D,eta,ierr,herr)
        call DPDDX (T,D,x,dPdD)
        call DPDTX (T,D,x,dPdT)
        if (D.gt.0.) bkt=1d0/dPdD/D/1000d0
        deld=ABS(D/dcc-1d0)
        delt=ABS(T/5.18992d0-1d0)
        r2=(delt/0.2d0)**2+(deld/0.25d0)**2
        if (r2.lt.1. .and. D.gt.0.) then
          xx=delt/deld**beta
          x1=(xx+x0)/x0
          x2b=x1**(2d0/beta)
          x2be=(1d0+e2*x2b)**(gamm/2d0*beta)
          dhdx=e1*x2be/x0+e1*e2/x0*x2b*x2be/(1d0+e2*x2b)*gamm
          d2kt=(delta*e1*x1*x2be-xx*dhdx*beta)*deld**(delta-1d0)
          bkt=r2*bkt+(1d0-r2)*(dcc/D)**2/d2kt/227460d0
        endif
        tcxcr=0d0
        if (bkt.ge.0. .and. D.gt.0.)
     &    tcxcr=T**2*SQRT(bkt)/D/wmas(i)/eta*dPdT**2*3.726229668d0*
     &           EXP5(-18.66d0*delt**2-4.25d0*deld**4)*3.4685233d-5
      endif
      tcxhe=(tcxdg+tcxbk+tcxcr)*tcxbkt(i)        !Multiply by reducing parameter (to convert units, etc.).

      end                                                 function TCXHE
c
c ======================================================================
c
      function TCXETY (icomp,T,D)
c
c  Thermal conductivity model for ethylene by Holland et al. (1983).
c
c  Model is no longer used.  See other routines for def. of inputs and outputs.

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      double precision g(9),e(7)
      data g/-2.9034235280d5,4.6806249520d5,-1.8954783215d5,
     &    -4.8262235392d3,2.2434093720d4,-6.6206354818d3,8.9937717078d2,
     &    -6.0559143718d1,1.6370306422d0/
      data e/-1.3045033230d1,1.8214616599d1,-9.9030224960d3,
     &    7.4205216310d2,-3.0083271933d-1,9.6456068829d1,1.3502569620d4/
      TCXETY=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      dd=D*wmas(icomp)/1000d0
      tt=T**thrd
      tcx0 =g(1)/T+g(2)/tt**2+g(3)/tt+g(4)+g(5)*tt+
     &      g(6)*tt**2+g(7)*T+g(8)*tt**4+g(9)*tt**5
      tcxpr=EXP5(e(1)+e(4)/T)*(EXP5(dd**0.1d0*(e(2)+e(3)/T**1.5d0)+
     &      (dd/0.221d0-1d0)*SQRT(dd)*(e(5)+e(6)/T+e(7)/T**2))-1d0)
      TCXETY=(tcx0+tcxpr)/1000d0

      end                                                function TCXETY
c
c ======================================================================
c
      function TCXR23 (icomp,T,D)
c
c  Thermal conductivity model for R23.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCXR23--Thermal conductivity [W/m-K]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-01-00 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension c(7)
      TCXR23=0d0
      if (T.gt.1000. .or. D.lt.0.) RETURN
      c=ctcx(icomp,1:7,1)
      dd=c(1)-D
      del=D-Drdbkt(icomp)
      tau=T-Trdbkt(icomp)
      tcxdg=(c(2)+c(3)*T)*(ABS(dd)/c(1))**c(4)
      tcxrs=(D/c(1))**c(4)*c(5)*c(1)**2/dd*SQRT(T)*
     &      EXP5(D/dd*c(6)/xRgas/T)
      tcxct=4d0*c(7)/(EXP5(del)+EXP5(-del))/(EXP5(tau)+EXP5(-tau))
      TCXR23=(tcxdg+tcxrs+tcxct)/1000d0
      end                                                function TCXR23
c
c ======================================================================
c
      function TCXD2O (icomp,T,D)
c
c  Thermal conductivity model for heavy water.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   TCXD2O--Thermal conductivity [W/m-K]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-06-00 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension c(11)
      TCXD2O=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      j=ndgnum(i)
      k=ndgnum(i)+nbknum(i)
      c=ctcx(i,k+1:11,1)
      tr=T/Trdbkt(i)
      dr=D/Drdbkt(i)
      tau=tr/(ABS(tr-1.1d0)+1.1d0)
      f1=EXP5(c(5)*tr+c(6)*tr**2)
      f2=EXP5(c(7)*(dr-1d0)**2)+c(8)*EXP5(c(9)*(dr-c(10))**2)
      f3=1d0+EXP5( 60d0*(tau-1d0)+20d0)
      f4=1d0+EXP5(100d0*(tau-1d0)+15d0)
      tcx0=SUM(ctcx(i,1  :j,1)*tr**ctcx(i,1  :j,2))
      tcxr=SUM(ctcx(i,j+1:k,1)*dr**ctcx(i,j+1:k,2))
      tcxr=c(2)*(1d0-EXP5(c(1)*dr))+tcxr
      tcxc=c(3)*f1*f2*(1d0+f2**2*(c(4)*f1**4/f3+3.5d0*f2/f4))
      tcxl=c(11)*f1**1.2d0*(1d0-EXP5(-(dr/2.5d0)**10))
      TCXD2O=tcxbkt(i)*(tcx0+tcxr+tcxc+tcxl)
      end                                                function TCXD2O

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                  End file TRNS_TCX.FOR
c ======================================================================












