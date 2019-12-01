c  Begin file TRNS_VIS.FOR
c
c  This file contains the core routines for viscosity.
c
c  Contained here are:
c
c     subroutine ETAK   (icomp,T,D,eta,ierr,herr)
c     subroutine ETAK00 (icomp,T,eta,ierr,herr)
c     subroutine ETAKB  (icomp,T,D,etab,ierr,herr)
c     subroutine ETAK0  (icomp,T,eta0,ierr,herr)
c     subroutine ETAK1  (icomp,T,etaB2,ierr,herr)
c     subroutine ETAKR  (icomp,T,D,etar,ierr,herr)
c     subroutine SETVIS (nread,icomp,htype,ierr,herr)
c     function   ETA1DG (icomp,ifunc,i12,T)
c     function   ETA1B2 (icomp,iB2,T)
c     function   ETA1RS (icomp,T,D)
c     function   ETA4RS (icomp,T,D)
c     function   ETAMIX (T,z)
c
c  Hard-coded routines:
c     function ETA0HC (icomp,T,D,ierr,herr)
c     function ETAH2O (icomp,T,D)
c     function XI_FUN (tk,D,icomp)
c     function ETA_C2 (xi)
c
c  No longer in use:
c     function ETAETY (icomp,T,D)
c     function ETANEO (icomp,T,D)
c     function ETAR23 (icomp,T,D)
c     function ETASF6 (icomp,T,D)
c     function ETAH2mct (icomp,T,D)
c     function ETAH2  (icomp,T,D,ih2)
c     function ETAHE  (icomp,T,Dm)
c     function ETAMEO (icomp,T,Dm)
c     function ETACO2 (icomp,T,D)
c
c ======================================================================
c ======================================================================
c
      subroutine ETAK (icomp,T,D,eta,ierr,herr)
c
c  Viscosity of a pure fluid as a function of temperature and density.
c  The viscosity is the sum of the dilute-gas, initial-density, and
c  residual terms (separate functions for the three terms are used
c  because of the requirements of certain mixture models, e.g. ECS).
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c      eta--Viscosity [uPa-s]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ETAK

      call ETAK0 (icomp,T,eta0,ierr,herr)
      call ETAK1 (icomp,T,etaB2,ierr,herr)
      call ETAKR (icomp,T,D,etar,ierr,herr)
      eta=eta0*(1d0+etaB2*D)+etar
      if (icomp.le.ncmax) then
        xeta0 (icomp)=eta0   !Save for use in ALLPROPS routine.
        xetaB2(icomp)=eta0*etaB2*D
        xetar (icomp)=etar
        xetac (icomp)=0d0
      endif
      end                                                subroutine ETAK
c
c ======================================================================
c
      subroutine ETAK00 (icomp,T,eta,ierr,herr)
c
c  Dilute-gas contribution to the viscosity of a pure fluid as a
c  function of temperature.  This subroutine differs from ETAK0
c  in that it calls ETAKR with a density of zero to obtain the
c  dilute gas if ETA0 returns only zero (only a few fluids have
c  been coded in such manner).
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c      eta--Viscosity [uPa-s]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      call ETAK0 (icomp,T,eta,ierr,herr)
      if (eta.le.0d0) call ETAKR (icomp,T,0d0,eta,ierr,herr)
      end                                              subroutine ETAK00
c
c ======================================================================
c
      subroutine ETAKB (icomp,T,D,etab,ierr,herr)
c
c  Background contribution to the viscosity of a pure fluid as
c  a function of temperature and density; this includes both the
c  residual term and any initial density (second virial) terms.
c
c  The viscosity is the sum of the dilute-gas, initial-density, and
c  residual terms (separate functions for the three terms are used
c  because of the requirements of certain mixture models, e.g. ECS);
c  this routine returns the sum of the initial-density and residual terms.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c     etab--Background viscosity [uPa-s]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ETAKB

      call ETAK0 (icomp,T,eta0,ierr,herr)
      call ETAK1 (icomp,T,etaB2,ierr,herr)
      call ETAKR (icomp,T,D,etar,ierr,herr)
      if (eta0.le.0d0) then
        call ETAKR (icomp,T,0d0,eta0,ierr,herr)  !Added 5/12/2017 because hard coded models may include the dilute contribution in the residual piece.
        etab=etar-eta0
      else
        etab=eta0*etaB2*D+etar
      endif
      end                                               subroutine ETAKB
c
c ======================================================================
c
      subroutine ETAK0 (icomp,T,eta0,ierr,herr)
c
c  Dilute-gas contribution to the viscosity of a pure fluid as a
c  function of temperature and density.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c
c  Outputs:
c     eta0--Viscosity [uPa-s]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown model specified
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ETAK0

      ierr=0
      eta0=0d0
      herr=' '
      if (icomp.le.0) RETURN

      if (hetak(icomp)(1:2).eq.'EC') then
        eta0=ETA1DG(icomp,0,0,T)
      elseif (hetak(icomp).eq.'VS0') then
c       eta0=ETA1DG(icomp,0,0,T)  !Old version from 9.1, do not use.  It can be substantially different from that contained in the hard coded model.
        eta0=ETA0HC(icomp,T,0d0,ierr,herr)
      elseif (hetak(icomp).eq.'VS1') then
        eta0=ETA1DG(icomp,1,1,T)
      elseif (hetak(icomp).eq.'VS2') then
        eta0=ETA1DG(icomp,2,0,T)
      elseif (hetak(icomp).eq.'VS3') then
        eta0=ETA1DG(icomp,3,0,T)
      elseif (hetak(icomp).eq.'VS4') then
        eta0=ETA1DG(icomp,4,2,T)
      elseif (hetak(icomp).eq.'VS5') then
        eta0=ETA1DG(icomp,5,0,T)
      elseif (hetak(icomp).eq.'VS6') then
        eta0=ETA1DG(icomp,6,1,T)
      elseif (hetak(icomp).eq.'VS7') then
        z(1:nc)=0d0
        z(icomp)=1d0
        eta0=RPNEQN(0,icomp,2,T,0d0,z,eta,ierr,herr)
      else
        call ERRNUM (502,0,'ETAK0',hetak(icomp),0d0,0d0,0d0,ierr,herr)
      endif

      end                                               subroutine ETAK0
c
c ======================================================================
c
      subroutine ETAK1 (icomp,T,etaB2,ierr,herr)
c
c  Second viscosity virial coefficient (initial-density contribution)
c  of a pure fluid as a function of temperature.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c
c  Outputs:
c    etaB2--Second viscosity virial coefficient [L/(mol-uPa-s)]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown model specified
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ETAK1

      ierr=0
      herr=' '
      etaB2=0d0
      if (icomp.le.0) RETURN

      if (hetak(icomp).eq.'VS0') then
        etaB2=0d0
      elseif (hetak(icomp).eq.'VS1') then
        etaB2=ETA1B2(icomp,0,T)
      elseif (hetak(icomp).eq.'VS2') then
        etaB2=0d0                           !No initial-density term for Y+E
      elseif (hetak(icomp).eq.'VS3') then
        etaB2=ETA1B2(icomp,3,T)
      elseif (hetak(icomp).eq.'VS4') then
        etaB2=ETA1B2(icomp,0,T)
      elseif (hetak(icomp).eq.'VS5') then
        etaB2=0d0
      elseif (hetak(icomp).eq.'VS6') then
        etaB2=ETA1B2(icomp,0,T)
      elseif (hetak(icomp).eq.'VS7') then
        z(1:nc)=0d0
        z(icomp)=1d0
        etaB2=RPNEQN(0,icomp,3,T,0d0,z,eta,ierr,herr)
      elseif (hetak(icomp).eq.'ECS') then
        ierr=999   !Do nothing, ECS is in effect and no ETAK1 is available.
      else
        call ERRNUM (502,0,'ETAK1',hetak(icomp),0d0,0d0,0d0,ierr,herr)
      endif

      end                                               subroutine ETAK1
c
c ======================================================================
c
      subroutine ETAKR (icomp,T,D,etar,ierr,herr)
c
c  Residual contribution to the viscosity of a pure fluid as a function
c  of temperature and density.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Outputs:
c     etar--Viscosity [uPa-s]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown model specified
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ETAKR

      ierr=0
      herr=' '
      etar=0d0
      if (icomp.le.0) RETURN

      if (hetak(icomp).eq.'VS0') then
        etar=ETA0HC(icomp,T,D,ierr,herr)
        etar=etar-ETA0HC(icomp,T,0d0,ierr,herr)  !eta0hc provides full viscosities; subtract dilute to get residual.
c       etar=etar-ETA1DG(icomp,0,0,T)            !This is the old method in 9.1, do NOT use!  ETA1DG uses a LJ method to get the dilute gas, but the hard coded models usually do not use that method so the difference can be huge.
      elseif (hetak(icomp).eq.'VS1') then
        etar=ETA1RS(icomp,T,D)
      elseif (hetak(icomp).eq.'VS2') then
        etar=ETA1RS(icomp,T,D)
      elseif (hetak(icomp).eq.'VS3') then
        etar=ETA1RS(icomp,T,D)
      elseif (hetak(icomp).eq.'VS4') then
        etar=ETA4RS(icomp,T,D)
      elseif (hetak(icomp).eq.'VS5') then
        etar=ETA1RS(icomp,T,D)
      elseif (hetak(icomp).eq.'VS6') then
        etar=ETA1RS(icomp,T,D)
      elseif (hetak(icomp).eq.'VS7') then
        z(1:nc)=0d0
        z(icomp)=1d0
        etar=RPNEQN(0,icomp,4,T,D,z,eta,ierr,herr)
        etar=etar+RPNEQN(0,icomp,5,T,D,z,eta,ierr,herr)
      elseif (hetak(icomp).eq.'ECS') then
        ierr=999   !Do nothing, ECS is in effect and no ETAKR is available.
      else
        call ERRNUM (502,0,'ETAKR',hetak(icomp),0d0,0d0,0d0,ierr,herr)
      endif

      end                                               subroutine ETAKR
c
c ======================================================================
c
      function ETA0HC (icomp,T,D,ierr,herr)
c
c  Model for the hardcoded viscosity models.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   eta0hc--Viscosity [uPa-s]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-29-00 EWL, original version
c  12-22-09 MLH, added D2, T2, HE3 models
c  04-06-11 MLH, added Altunin SF6 vis model
c  11-16-12 MLH, allow two VS0 models for viscosity
c  02-28-13 MLH, added D2
c  12-18-15 MLH, added CO2

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr

      ierr=0
      herr=' '
      ETA0HC=0d0
      if (hetahc(icomp).eq.'H2O') then
        ETA0HC=ETAH2O(icomp,T,D)
      elseif (hetahc(icomp).eq.'ETY') then
        ETA0HC=ETAETY(icomp,T,D)
      elseif (hetahc(icomp).eq.'NEO') then
        ETA0HC=ETANEO(icomp,T,D)
      elseif (hetahc(icomp).eq.'R23') then
        ETA0HC=ETAR23(icomp,T,D)
      elseif (hetahc(icomp).eq.'HE') then
        ETA0HC=ETAHE(icomp,T,D)
      elseif (hetahc(icomp).eq.'H2') then
        ETA0HC=ETAH2mct(icomp,T,D)               !Old McCarty model
      elseif (hetahc(icomp).eq.'H2A') then
        ETA0HC=ETAH2(icomp,T,D,1)                !Alternate H2 model of Muzny
      elseif (hetahc(icomp).eq.'D2') then
        ETA0HC=ETAH2(icomp,T,D,2)                !Scaled H2 model of Muzny for D2
      elseif (hetahc(icomp).eq.'T2') then
        ETA0HC=ETAH2(icomp,T,D,1)*SQRT(3d0)      !Scale to H2 with theoretical limit for gas.
      elseif (hetahc(icomp).eq.'HE3') then
        ETA0HC=ETAHE(icomp,T,D)*SQRT(3d0/4d0)    !Scale to He4 with theoretical limit for gas.
      elseif (hetahc(icomp).eq.'MEO') then
        ETA0HC=ETAMEO(icomp,T,D)
      elseif (hetahc(icomp).eq.'SF6') then
        ETA0HC=ETASF6(icomp,T,D)
      elseif (hetahc(icomp).eq.'CO2') then
        ETA0HC=ETACO2(icomp,T,D)
      else
        call ERRNUM (502,0,'ETA0HC',hetahc(icomp),0d0,0d0,0d0,ierr,herr)
      endif
      end                                                function ETA0HC
c
c ======================================================================
c
      function ETA1DG (icomp,ifunc,i12,T)
c
c  Calculate the dilute-gas contribution to the viscosity.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c    ifunc--Model number
c      i12--Index for the ceta array (either 1 or 2)
c        T--Temperature [K]
c
c  Output (as the function value):
c   ETA1DG--The dilute-gas part of the viscosity [uPa-s]

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      ETA1DG=0d0
      if (T.le.0. .or. T.gt.1d8) RETURN
      i=icomp
      j=0
      k=0

c  Dilute-gas contribution to the viscosity for use with ECS model.
      if (ifunc.eq.0) then
        if (ABS(epsktr(i)).lt.1d-20) RETURN
        if (ABS(sigtrn(i)).lt.1d-20) RETURN
        tau=T/epsktr(i)      !The dilute gas is simply the Chapman-Enskog term.
        om=OMEGAS(2,2,tau)
        ETA1DG=fcecs(i)*26.692d-3*SQRT(wmas(i)*T)/(sigtrn(i)**2*om)
        RETURN
      endif

c  Dilute-gas contribution to the viscosity with the hydrocarbon model of
c    Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987.
      if (ifunc.eq.2) then   !(old VS2 model)
        om=OMEGA(i,T,epskv(i),hmdeta(i))
        ETA1DG=ceta(i,1,1)*T**ceta(i,1,2)/(sigmav(i)**2*om)*etadge(i)     !Multiply by reducing parameter (to convert units, etc.).
        RETURN
      endif

c  Dilute-gas contribution to the viscosity with the Chung model (VS5)
c    Chung, Ajlan, Lee, and Starling, Ind. Eng. Chem. Res. 27:671-679, 1988.
      if (ndgchg(i).ge.1) then    !Sum the dilute-gas terms.
        tau=XDIV(T,epschv(i))
        vc=(sigchv(i)/8.09d0)**3
        ETA5=0.00040785d0*SQRT(T*wmas(i))/(vc**thrd2*OMEGAS(2,2,tau))     !First term is always the Chapman-Enskog term.
        if (ndgchg(i).ge.2) then  !Possibility for additional, empirical terms.
          j=k+1
          k=k+ndgchg(i)
          ETA5=ETA5+SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2))
        endif
        fchung=1d0-0.2756d0*acchv(i)+0.059035d0*ddipv(i)**4+hbvk(i)  !Multiply by Chung factor and reducing parameter
        ETA1DG=ETA1DG+ETA5*fchung*etadge(i)                          !  for viscosity (to convert units, etc.).
      endif

c  Dilute-gas contribution to the viscosity with the composite model (VS1)
c   or the friction theory model (VS4)
      ETA1=0d0
      tau=XDIV(T,Trddge(i))
      if (ndgeta(i).gt.0) then              !Sum the dilute-gas terms.
        k=k+1
        ETA1=ceta(i,1,1)*XDIV(SQRT(tau),    !First term is always the Chapman-Enskog term.
     &        sigmav(i)**2*OMEGA(i,T,epskv(i),hmdeta(i)))
        if (ndgeta(i).ge.2) then            !Possibility for additional, empirical terms
          j=k+i12-1
          k=k+ndgeta(i)-1
          ETA1=ETA1+SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2))
        endif
      endif
      if (ndgeta2(i).gt.0) then
        if (ETA1.eq.0d0) ETA1=1d0
        j=k+1
        k=k+ndgeta2(i)
        ETA1=ETA1*SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2))
      endif
      if (ndgeta3(i).gt.0) then
        j=k+1
        k=k+ndgeta3(i)
        denom=SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2))
        ETA1=XDIV(ETA1,denom)
      endif
      ETA1DG=ETA1DG+ETA1*etadge(i)


c  Dilute-gas contribution to the viscosity with the composite model (VS3).
      ETA3=0d0
      if (ndgnm3(i).ge.1) then         !Sum the dilute-gas terms, first numerator then denominator.
        j=k+1
        k=k+ndgnm3(i)
        ETA3=SUM(ceta(i,j:k,1)*(T/Trddg3(i))**ceta(i,j:k,2))
      endif
      if (ndgdn3(i).ge.1) then
        if (ETA3.eq.0.) ETA3=1d0
        j=k+1
        k=k+ndgdn3(i)
        denom=SUM(ceta(i,j:k,1)*(T/Trddg3(i))**ceta(i,j:k,2))
        ETA3=XDIV(ETA3,denom)          !Divide numerator by denominator.
      endif
      ETA1DG=ETA1DG+ETA3*etadg3(i)     !Multiply by reducing parameter (to convert units, etc.).

      end                                                function ETA1DG
c
c ======================================================================
c
      function ETA1B2 (icomp,iB2,T)
c
c  Second viscosity virial coefficient with the composite model (VS1).
c  This model implements the initial-density dependence of the
c  Rainwater-Friend theory.  It returns a viscosity virial coefficient
c  that must be multiplied by the dilute-gas viscosity and the density
c  to yield the initial-density dependence of viscosity.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c      iB2--When set to 3, use this as old ETA1B3 routine
c        T--Temperature [K]
c
c  Output (as the function value):
c   ETA1B2--The second viscosity virial coefficient [L/(mol-uPa-s)]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-17-97  MM, original version
c  02-23-16 MLH, created version for vs3 model
c  07-17-16 EWL, combine ETA1B2 and ETA3B2 and add input variable iB2

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      ETA1B2=0d0
      if (T.le.0.) RETURN
      i=icomp
      nterm=ndgeta(i)+ndgeta2(i)+ndgeta3(i)
      if (iB2.eq.3) nterm=ndgnm3(i)+ndgdn3(i)    !+nbknm3(i)+nbkdn3(i)
      if (nB2eta(i).ge.1) then                   !Sum the terms comprising the viscosity virial.
        tau=T/TredB2(i)
        j=nterm+1
        k=nterm+nB2eta(i)
        ETA1B2=SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2))*etarB2(i)  !Multiply by reducing parameter (to convert units, etc.).
      endif

      end                                                function ETA1B2
c
c ======================================================================
c
      function ETA1RS (icomp,T,D)
c
c  Residual contribution to the viscosity with the composite model (VS1).
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   ETA1RS--The background part of the viscosity [uPa-s]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-16-97  MM, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension a0(10),a1(10),a2(10),a3(10), aa(10)
      data a0/6.32402d0,0.12102d-2,5.28346d0,6.62263d0,19.74540d0,
     &   -1.89992d0,24.2745d0,0.79716d0,-0.23816d0,0.68629d-1/
      data a1/50.4119d0,-0.11536d-2,254.209d0,38.0957d0,7.63034d0,
     &   -12.5367d0,3.44945d0,1.11764d0,0.67695d-1,0.34793d0/
      data a2/-51.6801d0,-0.62571d-2,-168.481d0,-8.46414d0,-14.3544d0,
     &   4.98529d0,-11.2913d0,0.12348d-1,-0.8163d0,0.59256d0/
      data a3/1189.02d0,0.37283d-1,3898.27d0,31.4178d0,31.5267d0,
     &   -18.1507d0,69.3466d0,-4.11661d0,4.02528d0,-0.72663d0/

      ETA1RS=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      k=0
      nsum=ABS(ndel0(i))+npoly(i)+nnumeta(i)+ndeneta(i)+nexpn(i)+
     &     nexpd(i)+nbknm3(i)+nbkdn3(i)
      tau=1d0
      del=1d0
      expdel=1d0
      if (nsum.ge.1) then    !Compute tau and del only if terms present.
        tau=T/Tredeta(i)     ! (otherwise reducing parameters were not read in).
        del=D/Dredeta(i)
        expdel=del           !Define the density to be used in exponential multipliers.
        if (ABS(Dredeta(i)-1d0).lt.1d-3) expdel=D/Dcrit(i) ! If the reducing density is 1.0, must divide by the critical density.
      endif
      nterm=ndgeta(i)+nB2eta(i)+ndgnm3(i)+ndgdn3(i)+ndgchg(i)+
     &      ndgeta2(i)+ndgeta3(i)


c  Residual contribution to the viscosity with the hydrocarbon model of
c    Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987.
c  Although this correlation has a separate initial density term, it is
c    not of the form required by ETAK1; thus the initial density term is
c    combined with the residual term.
      if (ndgye(i).gt.0) then
        eta1=D*(ceta(i,2,1)+ceta(i,3,1)*                   !Initial density term for viscosity.
     &         (ceta(i,4,1)-XLOG(T/ceta(i,5,1)))**2)       !Eq 21
        G=ceta(i,6,1)+ceta(i,7,1)/T                        !Eq 23
        H=SQRT(D)*(D-ceta(i,13,1))/ceta(i,13,1)            !Eq 25
        F=G+(ceta(i,8,1)+ceta(i,9,1)/T**1.5d0)*D**0.1d0+   !Compute the residual viscosity (viscosity minus the dilute gas and initial density terms).
     &      (ceta(i,10,1)+ceta(i,11,1)/T+ceta(i,12,1)/T**2)*H
        ETA1RS=eta1+EXP5(F)-EXP5(G)                        !Eq 22
        RETURN
      endif


c  Compute the various parts of the residual model.
c  These are taken in the order
c    Close-packed density function;
c    Simple polynomials in T, D, D_0, EXP(D/Dc);
c    Numerator of rational polynomial; Denominator of rational polynomial;
c    Numerator of exponential term; Denominator of exponential term
c  The coefficients themselves are given in the order
c    Constant multiplier; Temperature exponents;
c    Density exponents; Close-packed density exponents (all except del0 terms);
c    Power of density inside exponential (0 indicates no exponential)

c  Compute the close-packed density at the given temperature, if applicable.
      del0=1d0
      j=nterm+1
      k=k+nterm+ABS(ndel0(i))
      if (ndel0(i).gt.0) then
        del0=SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2))
      elseif (ndel0(i).lt.0) then           !Alternative formulation for del0
        del0=del0+SUM(ceta(i,j+1:k,1)*tau**ceta(i,j+1:k,2))
        del0=ceta(i,j,1)/del0
      endif
      nterm=nterm+ABS(ndel0(i))

c  Sum the very simple polynomial terms.
      if (nbknm3(i)+nbkdn3(i).gt.0) then
        ETA3=0d0
        if (nbknm3(i).ge.1) then
          j=nterm+1
          k=nterm+nbknm3(i)
          ETA3=SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2)*del**ceta(i,j:k,3))
        endif
        if (nbkdn3(i).ge.1 .and. del.gt.0.) then
          j=nterm+nbknm3(i)+1
          k=nterm+nbknm3(i)+nbkdn3(i)
          denom=SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2)*del**ceta(i,j:k,3))
          ETA3=XDIV(ETA3,denom)             !Divide numerator by denominator.
        endif
        ETA1RS=ETA1RS+ETA3
        nterm=nterm+nbknm3(i)+nbkdn3(i)     !Sum the background terms, first numerator then denominator.
      endif

c  Sum the simple polynomial terms.
      if (npoly(i).ge.1) then
        do k=nterm+1,nterm+npoly(i)
          visci=ceta(i,k,1)*tau**ceta(i,k,2)*XPOWR(del,ceta(i,k,3))*
     &          del0**ceta(i,k,4)
          if (ieta(i,k).ge.1) visci=visci*EXP5(-expdel**ieta(i,k))
          ETA1RS=ETA1RS+visci
        enddo
        nterm=nterm+npoly(i)
      endif

c  Calculate the numerator of the rational polynomial.
      xnum=1d0
      if (nnumeta(i).ge.1) then
        xnum=0d0
        do k=nterm+1,nterm+nnumeta(i)
          xnum=xnum+ceta(i,k,1)*tau**ceta(i,k,2)*XPOWR(del,ceta(i,k,3))*
     &         del0**ceta(i,k,4)
          if (ieta(i,k).ge.1) xnum=xnum*EXP5(-expdel**ieta(i,k))
        enddo
        nterm=nterm+nnumeta(i)
      endif

c  Calculate the denominator of the rational polynomial.
      xden=1d0
      if (ndeneta(i).ge.1) then
        xden=0d0
        do k=nterm+1,nterm+ndeneta(i)
          xden=xden+ceta(i,k,1)*tau**ceta(i,k,2)*XPOWR(del,ceta(i,k,3))*
     &         del0**ceta(i,k,4)
          if (ieta(i,k).ge.1) xden=xden*EXP5(-expdel**ieta(i,k))
        enddo
        if (ABS(xden).le.1.d-20) xden=1.d0
        nterm=nterm+ndeneta(i)
      endif

c  Combine the two parts of the rational polynomial, if applicable.
      if (nnumeta(i).ge.1 .or. ndeneta(i).ge.1) ETA1RS=ETA1RS+xnum/xden

c  Calculate the numerator of the exponential term.
      xnum=1d0
      if (nexpn(i).ge.1) then
        j=nterm+1
        k=nterm+nexpn(i)
        xnum=SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2)*del**ceta(i,j:k,3)*
     &           del0**ceta(i,j:k,4))
        nterm=nterm+nexpn(i)
      endif

c  Calculate the denominator of the exponential term.
      xden=1d0
      if (nexpd(i).ge.1) then
        j=nterm+1
        k=nterm+nexpd(i)
        xden=SUM(ceta(i,j:k,1)*tau**ceta(i,j:k,2)*del**ceta(i,j:k,3)*
     &           del0**ceta(i,j:k,4))
      endif
c  Combine the two parts of the exponential term, if applicable.
      if (nexpn(i).ge.1.or.nexpd(i).ge.1) ETA1RS=ETA1RS+EXP5(xnum/xden)

c  Residual contribution to the viscosity for the Chung model (VS5) of
c    Chung, Ajlan, Lee, and Starling, Ind. Eng. Chem. Res. 27:671-679, 1988.
c  This version allows the parameters for the residual piece to be independent from the dilute gas
c    and fit as free parameters.  Nomenclature for comparison with manuscript.
c    sigchv(i)=cceta(i,1,1)   epschv(i)=cceta(i,1,2)  acchv(i)=cceta(i,1,3)
c    ddipv(i)=cceta(i,1,4)    hbvk(i)=cceta(i,1,5)
      if (nchg5(i).gt.0 .and. D.gt.0d0) then
        Tstar=T/cceta(i,1,2)
        vc=(cceta(i,1,1)*10d0/0.809d0)**3   !Compute vc in cm3/mol from Chung sigma.
        fchung=1d0-0.2756d0*cceta(i,1,3)+0.059035d0*cceta(i,1,4)**4+
     &         cceta(i,1,5)
        aa(1:10)=a0(1:10)+a1(1:10)*cceta(i,1,3)+a2(1:10)*
     &           cceta(i,1,4)**4+a3(1:10)*cceta(i,1,5)
        y=D*vc/6d3
        g1=(1d0-y/2d0)/(1d0-y)**3
        g2=(aa(1)*(1d0-EXP5(-aa(4)*y))/y+aa(2)*g1*EXP5(aa(5)*y)+
     &      aa(3)*g1)/(aa(1)*aa(4)+aa(2)+aa(3))
        omegav=OMEGAS(2,2,Tstar)
        eta0chung=4.0785d-5*SQRT(wmas(i)*T)*fchung/(omegav*vc**thrd2)
        etakap=eta0chung*(1d0/g2+aa(6)*y)
        etap=(36.344d-6*SQRT(wmas(i)*1.2593d0*cceta(i,1,2))/vc**thrd2)*
     &       aa(7)*y**2*g2*EXP5(aa(8)+aa(9)/Tstar+aa(10)/Tstar**2)
        etach=etakap+etap              !In poise
        ETA1RS=(etach-eta0chung)*1d5   !Convert to uPa.s
      endif

      ETA1RS=ETA1RS*etared(i)          !Multiply by reducing parameter (to convert units, etc.).

      end                                                function ETA1RS
c
c ======================================================================
c
      function ETA4RS (icomp,T,D)
c
c  Residual contribution to the viscosity for the friction theory model (VS4)
c  Quinones-Cisneros and Deiters, J. Phys. Chem. B, 110:12820-12834, 2006.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   ETA4RS--The background part of the viscosity [uPa-s]
c
c  12-28-06 MLH, original version
c  02-21-07 MLH, allow for higher order repulsive terms, check for 2-phase
c  04-28-08 MLH, increased array size in trnft to accommodate new H2 model
c  02-03-10 MLH, added HSQM term for n-hydrogen and parahydrogen only
c  12-28-10 MLH, added Boshkova -Deiters diameter
c  01-08-12 MLH, added d and e parameters
c  04-12-12 MLH, allowed extra terms except keep old methane form
c  07-11-12 MLH, added terms for 1234yf
c  11-06-12 MLH, revised to not use short equ at sat bdry
c  06-11-15 EWL, remove call to PUREFLD, it resets all of the initialization variables.

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax),xliq(ncmax),xvap(ncmax)

      ETA4RS=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      conb=100d0        !Set up constants.
      Rgas=xRgas/conb
      chs=3d0*SQRT(2d0)
      emm=3.34707d-27
      z(1:nc)=0d0
      z(i)=1d0

c  Get value of pressure for the pure fluid at T and D.
      call PRESS (T,D,z,P)
      P=P/conb                    !Convert to bar

c  For normal or para hydrogen, use special case here with hard spheres quantum mechanical term.
      if (iFluidID(i).eq.11 .or. iFluidID(i).eq.12) then   !Normal or parahydrogen
        tr=T/Tcrit(i)
        psi1=EXP5(1d0/tr   -1d0)-1d0
        psi2=EXP5(1d0/tr**2-1d0)-1d0
        psi3=EXP5(1d0/tr**3-1d0)-1d0
        psi4=1d0/tr-1d0
        sumkapa  =(ASftm(i,0)+ASftm(i,1)*psi1+ASftm(i,2)*psi2+
     &                        ASftm(i,3)*psi3+ASftm(i,4)*psi4)/tr
        sumkapaa =(ABftm(i,0)+ABftm(i,1)*psi1+ABftm(i,2)*psi2+
     &                        ABftm(i,3)*psi3+ABftm(i,4)*psi4)/tr
        sumkapr  =(BSftm(i,0)+BSftm(i,1)*psi1+BSftm(i,2)*psi2+
     &                        BSftm(i,3)*psi3+BSftm(i,4)*psi4)/tr
        sumkaprr =(BBftm(i,0)+BBftm(i,1)*psi1+BBftm(i,2)*psi2+
     &                        BBftm(i,3)*psi3+BBftm(i,4)*psi4)/tr
        sumkaprrr=(DBftm(i,0)+DBftm(i,1)*psi1+DBftm(i,2)*psi2+
     &                        DBftm(i,3)*psi3+DBftm(i,4)*psi4)/tr**3.0
        if (iFluidID(i).eq.12) then                  !Normal hydrogen
          sumkapaaa=9.750655931493327d-11*psi2/tr**3 !Normal
        else
          sumkapaaa=3.8478220629752735d-9*psi2/tr**3 !Para
        endif
c  The hard sphere quantum terms.
        sigg0=2.97d-10       !Specific to hydrogen
        epsi=30.41d0
        tsquig=T/epsi        !Revision for soft sphere
        dnumer=1d0+0.4378142d0*tsquig+0.009951898d0*tsquig**2
        ddenom=1d0+0.4745558d0*tsquig+0.01338717d0 *tsquig**2
        sigg=0.925d0*dnumer*sigg0/ddenom
        ppsi=D*1000d0*xPi/6d0*xAvog*sigg**3
        xlam=xPlanck/SQRT(2d0*xPi*emm*xBoltz*T)
        term1=ppsi*xlam*chs*(1d0+ppsi-ppsi**2/2d0)/((1d0-ppsi)**4)
        term1=term1/(sigg*100d0)
        term2=(1d0+ppsi+ppsi**2-ppsi**3)/(1d0-ppsi)**3
        zr=term1+term2
        prep =zr*D*Rgas*T    !bar
        patt =P-prep
        pid  =D*Rgas*T
        delpr=prep-pid
        ETA4RS=sumkapr*delpr+sumkapa*patt+sumkaprr*prep**2+     !In uPa.s
     &         sumkapaa*patt**2+sumkaprrr*prep**3+sumkapaaa*patt**3
        RETURN
      endif

c  For everything except normal hydrogen and parahydrogen, compute the residual term.
      tr=Tcrit(i)/T
      psi1=EXP5(tr)-1d0
      psi2=EXP5(tr**2)-1d0
      psi3=tr**(-0.25d0)
      fgam=psi2
      sumkapa =(ASftm(i,0)+ASftm(i,1)*psi1+ASftm(i,2)*psi2)*tr
      sumkapaa=(ABftm(i,0)+ABftm(i,1)*psi1+ABftm(i,2)*psi2)*tr**3
      sumkapr =(BSftm(i,0)+BSftm(i,1)*psi1+BSftm(i,2)*psi2)*tr
      sumkaprr=(BBftm(i,0)+BBftm(i,1)*psi1+BBftm(i,2)*psi2)*tr**3
      sumkapi =(CSftm(i,0)+CSftm(i,1)*psi1+CSftm(i,2)*fgam)*tr
      sumkapii=(CBftm(i,0)+CBftm(i,1)*psi1+CBftm(i,2)*psi2)*tr**3
      if (VerFL(i).gt.8.0) then        !Additional terms
        sumkapa  = sumkapa  + ASftm(i,3)*psi3*tr
        sumkapaa = sumkapaa + ABftm(i,3)*psi3*tr**3
        sumkapr  = sumkapr  + BSftm(i,3)*psi3*tr
        sumkaprr = sumkaprr + BBftm(i,3)*psi3*tr**3
        sumkapi  = sumkapi  + CSftm(i,3)*psi3*tr
        sumkapii = sumkapii + CBftm(i,3)*psi3*tr**3
      endif

c  If in the two-phase region, interpolate liquid and vapor endpoints to
c   avoid bizarre behavior caused by loops from the EOS.
      iloop=1
      if (T.lt.Tcrit(i)) then
        call PSATK  (i,T,psat,ierr1,herr)
        call DLSATK (i,T,Dl,ierr2,herr)
        call DVSATK (i,T,Dv,ierr3,herr)
        if (ierr1.gt.0 .or. ierr2.gt.0 .or. ierr3.gt.0)
     &      call SATT (T,z,1,psat,Dl,Dv,xliq,xvap,ierr,herr)    !Find sat Dl, Dv for this T and dPdT on the boundary.
        if (D.gt.Dv .and. D.lt.Dl) then     !Check for single phase liquid or vapor below Tc
          P=psat/conb                       !Two-phase region
          iloop=2
        endif
      endif

      ETA4RSv=0d0
      ETA4RSl=0d0
      do j=1,iloop
        dd=D
        if (iloop.eq.2 .and. j.eq.1) dd=Dl  !Get viscosity on liquid side.
        if (iloop.eq.2 .and. j.eq.2) dd=Dv  !Get viscosity on vapor side.
        call DPDTX (T,dd,z,dPdT)
        prep=T*dPdT/conb     !bar
        patt=P-prep
        pid=dd*Rgas*T
        delpr=prep-pid
        ETA4RS=sumkapr*delpr+sumkapa*patt+sumkaprr*delpr**2+
     &         sumkapaa*patt**2+sumkapi*pid+sumkapii*pid**2+
     &         DBftm(i,0)*prep**3*tr**2
        if (VerFL(i).gt.8.0 .and. iFluidID(i).ne.101) then !Extra terms except methane
          ETA4RS=ETA4RS+
     &     (DBftm(i,0)+DBftm(i,1)*psi1+DBftm(i,2)*psi2+DBftm(i,3)*psi3)*
     &      prep**3*tr**DBftm(i,4)+tr**EBftm(i,4)*
     &     (EBftm(i,0)+EBftm(i,1)*psi1+EBftm(i,2)*psi2)*patt**3
        endif
        if (iloop.eq.2 .and. j.eq.1) ETA4RSl=ETA4RS
        if (iloop.eq.2 .and. j.eq.2) ETA4RSv=ETA4RS
      enddo
      if (iloop.eq.2) then
        drat=(Dl-D)/(Dl-Dv)
        ETA4RS=ETA4RSl-drat*(ETA4RSl-ETA4RSv)
      endif
      ETA4RS=ETA4RS*1000d0        !Convert to uPa.s

      end                                                function ETA4RS
c
c ======================================================================
c
      function ETAH2O (icomp,T,D)
c
c  Viscosity model for water and heavy water.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   etaH2O--Viscosity [uPa-s]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-06-00 EWL, original version
c  05-08-08 MLH, allow for IAPWS 2008 critical enhancement term

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension z(ncmax)

      ETAH2O=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      i=icomp
      tr=T/Tredeta(i)
      drr=D/Dredeta(i)
      tau=1d0/tr-1d0
      del=drr-1d0
      if (ABS(tau).lt.1d-12) tau=1d-12
      if (ABS(del).lt.1d-12) del=1d-12
      k=ndel0(i)
      s=SUM(ceta(i,1:k,1)/tr**ceta(i,1:k,2))
      eta0=SQRT(tr)/s
      j=ndel0(i)+1
      k=ndel0(i)+npoly(i)
      s=SUM(ceta(i,j:k,1)*tau**INT(ceta(i,j:k,2))*
     &      del**INT(ceta(i,j:k,3)))
      eta1=EXP5(drr*s)

c  Previous 1985 IAPWS critical enhancement
      eta2=1d0
      if (hetacr(i).eq.'I85') then
        if (nnumeta(i).gt.0) then
          if (tr.gt.0.997d0 .and. tr.lt.1.0082d0) then
            if (drr.gt.0.755d0 .and. drr.lt.1.29d0) then
              z(1:nc)=0d0
              z(i)=1d0
              call DPDDX (T,D,z,dPdD)
              k=ndel0(i)+npoly(i)+1
              x=drr/dPdD*ceta(i,k,1)/Dredeta(i)
              if (x.ge.ceta(i,k+1,3))eta2=ceta(i,k+1,1)*x**ceta(i,k+1,2)
            endif
          endif
        endif

c  2008 IAPWS standard critical enhancement.
      elseif (hetacr(i).eq.'I08') then
        xival=0d0
        if (D.gt.1d-12) xival=XI_FUN(T,D,i)
        eta2=ETA_C2(xival)
      endif
      etaH2O=etared(i)*eta0*eta1*eta2

      end                                                function ETAH2O
c
c ======================================================================
c
      function XI_FUN (tk,D,icomp)
c
c  Auxiliary functions for computing the critical enhancement of viscosity of water from
c  New International Formulation for the viscosity of water
c  Huber, Perkins, Laesecke, Friend, Sengers, Assael, Metaxa, Vogel, Mares, and Miyagawa,
c  J. Phys. Chem. Ref. Data, 38:101-125, 2009.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c       tk--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   XI_FUN--Delta_xi_star
c
c  Written by M.L. Huber, NIST Thermophysics Division, Boulder, Colorado
c  05-08-08 MLH, original version, based on code from RAP
c  09-20-10 MLH, remove extrap limit on chi

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension z(ncmax)
      parameter (xi0=0.13d-9,gammaplus=0.06d0)
      parameter (exnu=0.63d0,exgamma=1.239d0)

      XI_FUN=0d0
      if (tk.le.0. .or. D.lt.0.) RETURN
      z(1:nc)=0d0
      z(icomp)=1d0
      tr=1.5d0*Tcrit(icomp)       !Calculate background chi at 1.5 * Tc.
      Denred=D/Dcrit(icomp)
      if (tr.lt.tk) tr=tk
      call DPDDX (tk,D,z,dPdD)    !Calculate isothermal compressibility at T and D in units of 1/MPa.
      call DPDDX (tr,D,z,dPdD_r)  !Calculate isothermal compressibility at TR and D in units of 1/MPa.
      tisocomp =1000d0/D/dPdD
      tisocompr=1000d0/D/dPdD_r
      delchi=Pcrit(icomp)*Denred**2*(tisocomp-tisocompr*tr/tk)/1000d0     !Calculate difference in reduced chi at (T,D) and (tr,D).
      if (delchi.ge.0.) XI_FUN=xi0*(delchi/gammaplus)**(exnu/exgamma)     !Calculate correlation length in meters.

      end                                                function XI_FUN
c
c ======================================================================
c
      function ETA_C2 (xi)
c
c  Auxiliary functions for computing the critical enhancement of viscosity of water from
c  New International Formulation for the viscosity of water
c  Huber, Perkins, Laesecke, Friend, Sengers, Assael, Metaxa, Vogel, Mares, and Miyagawa,
c  J. Phys. Chem. Ref. Data, 38:101-125, 2009.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c       tk--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   XI_FUN--Delta_xi_star
c
c  Written by M.L. Huber, NIST Thermophysics Division, Boulder, Colorado
c  05-08-08 MLH, original version, based on code from RAP

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      parameter (qcinv=1.9d-9,qdinv=1.1d-9)
      parameter (exeta=0.0680d0)            !New theoretical exponent for viscosity.

      ETA_C2=1d0
      if (xi.le.0.) RETURN
      qc=1d0/qcinv
      qd=1d0/qdinv
      psid=ACOS(SQRT(1d0/(1d0+(qd*xi)**2))) !Calculate the crossover function.
      w=SQRT(ABS((qc*xi-1d0)/(qc*xi+1d0)))*TAN(psid/2d0)
      if (qc*xi.gt.1.) then
        funL=XLOG((1d0+w)/(1d0-w))
      else
        funL=2d0*ATAN(ABS(w))
      endif
      qcxi2=(qc*xi)**2
      qcxi3=(qc*xi)**3
      if (xi.le.0.3734351887d-9) then
        Hcross=qc*xi*(qd*xi)**5/5d0*
     &        (1d0-qc*xi+qcxi2-765d0/504d0*(qd*xi)**2)
      else
        Hcross=SIN(3d0*psid)/12d0-SIN(2d0*psid)/(4d0*qc*xi)+
     &        (1d0-1.25d0*qcxi2)/qcxi2*SIN(psid)-
     &       ((1d0-1.5d0*qcxi2)*psid-ABS(qcxi2-1d0)**1.5d0*funL)/qcxi3
      endif
      ETA_C2=EXP5(exeta*Hcross)             !Calculate eta/eta_background (the enhancement).

      end                                                function ETA_C2
c
c ======================================================================
c
      function ETAMIX (T,z)
c
c  Compute the viscosity of a dilute-gas mixture assuming the components
c  are described by a Lennard-Jones potential.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition array (array of mole fractions)
c
c  Outputs:
c      eta--Viscosity [uPa-s]
c
c  Source of original version lost to history (possibly J.F. Ely).
c    Adopted for use in Refprop by S.A. Klein, January, 1996.
c                NIST Thermophysics Division, Boulder, Colorado
c  12-11-06 MLH, implement alternative method to match pure fluid ends
c  05-27-08 MLH, add one more binary int par for dilute gas viscosity xecsdij in trnbin
c  06-12-08 MLH, revised empirical correction for binaries
c  06-24-13 MLH, deactivate empirical correction for binaries

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr
      dimension z(ncmax),etai(nx),eok(nx,nx),sigm(nx,nx),rm(nx,nx)
      dimension h(nx,nx+1)

      ETAMIX=0d0
      nc1=nc+1
      h=1d0

      do i=1,nc
        if (z(i).gt.0.) then
          do j=1,nc
            if (z(j).gt.0.) then
              rm(i,j)=2d0*wmas(i)*wmas(j)/(wmas(i)+wmas(j))
              eok(i,j)=SQRT(epsktr(i)*epsktr(j))*(1d0-xecslje(i,j))
              sigm(i,j)=   (sigtrn(i)+sigtrn(j))*(1d0-xecsljs(i,j))/2d0
            endif
          enddo
        endif
      enddo

      do i=1,nc
        if (z(i).gt.0.) then
          do j=1,nc
            if (z(j).gt.0.) then
              suml=0d0
              do k=1,nc
                if (z(k).gt.0.) then
                  if (ABS(eok(i,k)).lt.1d-20) RETURN
                  term=SQRT(rm(i,k)*T)/sigm(i,k)**2
                  Ddil=0.320300d0*term/OMEGAS(1,1,T/eok(i,k))
                  etail=26.692d-3*term/OMEGAS(2,2,T/eok(i,k))
                  if (i.eq.k) etai(i)=etail
                  dij=0d0
                  djl=0d0
                  if (i.eq.j) dij=1d0
                  if (j.eq.k) djl=1d0
                  suml=suml+z(k)*rm(i,k)**2*((dij+djl)/etail/2d0+
     &                       rm(j,j)/rm(k,k)*(dij-djl)/Ddil)
                endif
              enddo
              h(i,j)=suml/(rm(i,i)*rm(j,j))
            endif
          enddo
        endif
      enddo

      do i=1,nc
        if (z(i).gt.0.) then
          h(i,i+1:nc1)=h(i,i+1:nc1)/h(i,i)
          h(i,i)=1d0
          do j=1,nc
            if (j.ne.i .and. z(j).gt.0.) then
              h(j,i+1:nc1)=h(j,i+1:nc1)-h(j,i)*h(i,i+1:nc1)
              h(j,i)=0d0
            endif
          enddo
        endif
      enddo

c  Additional empirical correction for binaries.
      if (VrsNm.lt.3.) then
        do i=1,nc
          if (z(i).gt.0.) then
            do j=1,nc
              if (i.ne.j .and. z(j).gt.0.) then
                xis=z(i)/(z(i)+z(j))        !Scaled binary composition
                ff=xecsdij(i,j)*xis-(xecsdij(i,j)+xecsdij2(i,j))*xis**2+
     &            xecsdij2(i,j)*xis**3
                h(i,nc1)=h(i,nc1)*(1d0+(z(i)+z(j))*ff)
              endif
            enddo
          endif
        enddo
      endif

c  Force the pure ends to match (individual correlations sometimes
c   do not use LJ but mixture always does).
      etam=0d0
      do i=1,nc
        if (z(i).gt.0.) then
          call ETAK00 (i,T,eta,ierr,herr)
          if (ierr.le.0 .and. etai(i).ne.0.) then
            etam=etam+z(i)*h(i,nc1)*eta/etai(i)
          else
            etam=etam+z(i)*h(i,nc1)
          endif
        endif
      enddo
      ETAMIX=etam

      end                                                function ETAMIX
c
c ======================================================================
c
      subroutine SETVIS (nread,icomp,htype,ierr,herr)
c
c  Setup the pure fluid viscosity models.
c
c  Model VS0:
c  Pure fluid viscosity model #0, the model that points to
c  all the hardcoded equations.
c
c  Model VS1:
c  Pure fluid viscosity model #1, the composite model.
c  This is written in a general form with terms designed to include several
c  correlations including those of Fenghour (1995) for ammonia,
c  Krauss (1996) for R152a, and Laesecke (1997) for R134a.
c
c  Model VS2:
c  Pure fluid viscosity model #2, the hydrocarbon model of
c  Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987.
c
c  Model VS4:
c  Pure fluid viscosity model #4, the friction theory model of
c  Quinones-Cisneros and Deiters, J. Phys. Chem. B, 110:12820-12834, 2006
c
c  Model VS5:
c  Pure fluid viscosity model #5, the Chung model of
c  Chung, Ajlan, Lee, and Starling, Ind. Eng. Chem. Res. 27:671-679, 1988.
c
c  Model VS6:
c  VS6 is same as VS1 except has new collision integral model.
c  Need to do this so old Refprop versions fail for VS6.
c
c  Model VS7:
c  Pure fluid viscosity model #7, which uses reverse Polish notation
c  (see helium.fld for a great example) to allow any functional form
c  to be used for the calculation of viscosity.  This prevents new
c  forms from having to be hardcoded individually, and prevents the
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

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr,htype*3
      dimension z(ncmax)

      ierr=0
      herr=' '
      if (nread.eq.0) RETURN      !Get coefficients from block data - this option not implemented.
      i=icomp
      j=0
      z(1:nc)=0d0
      z(1)=1d0

      l0=.false.
      l1=.false.
      l2=.false.
      l3=.false.
      l4=.false.
      l5=.false.
      l7=.false.
      if (htype(3:3).eq.'0') l0=.true.
      if (htype(3:3).eq.'1') l1=.true.
      if (htype(3:3).eq.'2') l2=.true.
      if (htype(3:3).eq.'3') l3=.true.
      if (htype(3:3).eq.'4') l4=.true.
      if (htype(3:3).eq.'5') l5=.true.
      if (htype(3:3).eq.'6') l1=.true. !Note: Use l1, not l6.
      if (htype(3:3).eq.'7') l7=.true.

c  Read data from file.
      hmdeta(i)=' '
      hetacr(i)='NUL'
      read (nread,*,err=102) Tmeta(i)     !Lower temperature limit
      read (nread,*,err=102) Txeta(i)     !Upper temperature limit
      read (nread,*,err=102) Pxeta(i)     !Upper pressure limit
      read (nread,*,err=102) Dxeta(i)     !Upper density limit
      if (l2.or.l3.or.l7) call READHA (nread,3,hmdeta(i))  !Pointer to omega model
      Dredeta(i)=0d0
      Tredeta(i)=0d0
      etared(i) =0d0
      Trddg3(i) =0d0
      etadg3(i) =0d0
      Trddge(i) =0d0
      etadge(i) =0d0
      ndgeta(i) =0
      ndgeta2(i)=0
      ndgeta3(i)=0
      ndgchg(i) =0
      nB2eta(i) =0      !# Viscosity virial terms.
      ndel0(i)  =0
      npoly(i)  =0
      nnumeta(i)=0
      ndeneta(i)=0
      nexpn(i)  =0
      nexpd(i)  =0
      ndgnm3(i) =0
      ndgdn3(i) =0
      nbknm3(i) =0
      nbkdn3(i) =0
      ndgye(i)  =0
      nchg5(i)  =0

c  Read in the VS7 form.
      if (l7) then
        if (hmdeta(i).ne.'NUL') then        !Pointer to omega model
          read (nread,*,err=102) sigmav(i)          !L-J sigma
          read (nread,*,err=102) epskv(i)           !L-J epsilon/kappa
          read (nread,*,err=102) Trddge(i),etadge(i)!Reducing parameters
          read (nread,*,err=102) ChEns1(i),ChEns2(i)!Chapman-Enskog terms
        endif
        oi=RPNEQN(-1,icomp,nread,0d0,0d0,z,0d0,ierr,herr)
        RETURN
      endif

c  Read in the Younglove and Ely 1987 form (VS2)
      if (l2) then
        ndgye(i)=1
        read (nread,*,err=102) sigmav(i)            !L-J sigma
        read (nread,*,err=102) epskv(i)             !L-J epsilon/kappa
        read (nread,*,err=102) ceta(i,1,1)          !Chapman-Enskog term
        ceta(i,1,2)=0.50d0                  !Chapman-Enskog exponent
        if (VerFL(i).ge.6.099d0) read (nread,*,err=102) ceta(i,1,2)
        do k=2,12                           !Read residual viscosity terms.
          read (nread,*,err=102) ceta(i,k,1)
        enddo
        ceta(i,13,1)=Dcrit(i)
        if (VerFL(i).ge.6.099d0) read (nread,*,err=102) ceta(i,13,1)
        Trddge(i)=1d0
        etadge(i)=1d0
        j=j+13
      endif

c  Read in pointer to the hardcoded model.
      if (l0) call READHA (nread,3,hetahc(i))

c  Read dilute gas terms.
      if (l0) read (nread,*,err=102)  ndgeta(i),nB2eta(i),ndel0(i),
     &            npoly(i),nnumeta(i),ndeneta(i),nexpn(i),nexpd(i)
      if (l5) read (nread,*,err=102)  ndgchg(i)     !# Dilute-gas terms for Chung model.
      if (l1) read (nread,*,err=102)  ndgeta(i)
      if (l4) read (nread,*,err=10) ndgeta(i),ndgeta2(i),ndgeta3(i)  !# Dilute-gas terms, 3 spares are available in the file.

 10   if (ndgeta(i)+ndgchg(i).ge.1) then
        call READHA (nread,3,hmdeta(i))     !Pointer to omega model
        read (nread,*,err=102) sigmav(i)            !L-J sigma
        read (nread,*,err=102) epskv(i)             !L-J epsilon/kappa
        read (nread,*,err=102) Trddge(i),etadge(i)  !Reducing parameters
        if (l5) sigchv(i)=sigmav(i)         !L-J sigma for Chung viscosity model
        if (l5) epschv(i)=epskv(i)          !L-J epsilon/kappa for Chung viscosity model
      endif

c  Read the dilute-gas terms.
 20   ndg=ndgeta(i)+ndgeta2(i)+ndgeta3(i)+ndgchg(i)+ndgnm3(i)+ndgdn3(i)
      if (ndg.ge.1) then
        if (.not.l4) then
        if (ndgeta2(i)+ndgeta3(i).ge.1)
     &                       read (nread,*,err=102)Trddg3(i),etadg3(i)  !Reducing parameters
        if (ndgnm3(i)+ndgdn3(i).ge.1)
     &                       read (nread,*,err=102)Trddg3(i),etadg3(i)  !Reducing parameters
        endif
        do n=1,ndg
          j=j+1
          read (nread,*,err=102) ceta(i,j,1),ceta(i,j,2)
        enddo
      endif

      if (l0) then
        if (ndel0(i)+npoly(i)+nnumeta(i)+ndeneta(i).gt.0)
     &      read (nread,*,err=20) Tredeta(i),Dredeta(i),etared(i)         !Reducing parameters
      endif

c  Viscosity virial terms
      if (.not.l0 .and. .not.l2) read (nread,*,err=102) nB2eta(i)
      if (nB2eta(i).ge.1) then
        if (.not.l0) read (nread,*,err=102) TredB2(i),etarB2(i)           !Reducing parameters
        do n=1,nB2eta(i)                    !Read viscosity virial terms.
          j=j+1
          read (nread,*,err=102) ceta(i,j,1),ceta(i,j,2)
        enddo
      endif

      if (l1) read (nread,*,err=102) ndel0(i),npoly(i),
     &      nnumeta(i),ndeneta(i),nexpn(i),nexpd(i)

c  Read the 18 parameters in the original generalized model for the VS4 model.
c  These are in the order a(0:2), b(0:2), c(0:2), A(0:2), B(0:2), C(0:2).
      if (l4) then
        if (VerFL(i).le.8.0) then
          read (nread,*,err=102) (ASftm(i,k),k=0,2)
          read (nread,*,err=102) (BSftm(i,k),k=0,2)
          read (nread,*,err=102) (CSftm(i,k),k=0,2)
          read (nread,*,err=102) (ABftm(i,k),k=0,2)
          read (nread,*,err=102) (BBftm(i,k),k=0,2)
          read (nread,*,err=102) (CBftm(i,k),k=0,2)
          read (nread,*,err=102) (DBftm(i,k),k=0,2) !One additional higher order term needed for high pressures (>500 MPa).
        else
          read (nread,*,err=102) (ASftm(i,k),k=0,4) !Terms in v8.1 and above.
          read (nread,*,err=102) (BSftm(i,k),k=0,4)
          read (nread,*,err=102) (CSftm(i,k),k=0,4)
          read (nread,*,err=102) (ABftm(i,k),k=0,4)
          read (nread,*,err=102) (BBftm(i,k),k=0,4)
          read (nread,*,err=102) (CBftm(i,k),k=0,4)
          read (nread,*,err=102) (DBftm(i,k),k=0,4)
        endif
        EBftm(i,0:4)=0d0
        if (VerFL(i).ge.9.0) read (nread,*,err=102) (EBftm(i,k),k=0,4)  !Read additional coefficients.
        call READHA (nread,3,hetacr(i))     !Pointer to critical enhancement model.
        RETURN
      endif

c  Set residual parameters.
      if (l5) then
        etared(i)=1d0
        nchg5(i)=1
        read (nread,*,err=102) acchv(i),ddipv(i),hbvk(i)
        cceta(i,1,1)=sigchv(i)
        cceta(i,1,2)=epschv(i)
        cceta(i,1,3)=acchv(i)
        cceta(i,1,4)=ddipv(i)
        cceta(i,1,5)=hbvk(i)
        read (nread,*,err=102) naddv(i)   !Additional terms
        do n=1,naddv(i)           !Use these terms if residual parameters are not the same as dilute gas.
          read (nread,*,err=102) (cceta(i,n,k),k=1,5)
        enddo
      endif

      if (l1) read (nread,*,err=102) Tredeta(i),Dredeta(i),etared(i)    !Reducing parameters
      lx=.true.
      if (l0.or.l1) lx=.false.

      if (ndel0(i).ne.0) then
        if (lx) read (nread,*,err=102) Tredeta(i),Dredeta(i),etared(i)
        do n=1,ABS(ndel0(i))                          !Close-packed density term
          j=j+1
          read (nread,*,err=102) ceta(i,j,1),ceta(i,j,2)
        enddo
      endif
      if (nbknm3(i)+nbkdn3(i).ge.1) then
        read (nread,*,err=102) Tredeta(i),Dredeta(i),etared(i)!Reducing parameters
        do n=1,nbknm3(i)+nbkdn3(i)
          j=j+1
          read (nread,*,err=102) (ceta(i,j,k),k=1,4)
        enddo
      endif
      if (nexpn(i)+nexpd(i).ge.1) then
        do n=1,nexpn(i)+nexpd(i)                      !Numerator of exponential term
          j=j+1                                       ! and denominator of exponential term.
          read (nread,*,err=102) (ceta(i,j,k),k=1,3),ieta(i,j)
        enddo
      endif
      nrsum=npoly(i)+nnumeta(i)+ndeneta(i)
      if (nrsum.ge.1) then
        if (lx) read (nread,*,err=102) Tredeta(i),Dredeta(i),etared(i)
        do n=1,nrsum                                  !Simple polynomial terms,
          j=j+1                                       ! numerator of rational polynomial terms,
          read (nread,*,err=102) (ceta(i,j,k),k=1,4),ieta(i,j)! and denominator of rational poly terms.
        enddo
      endif
      call READHA (nread,3,hetacr(i))                 !Pointer to critical enhancement model.
      if (Trddge(i).eq.0d0) Trddge(i)=Tredeta(i)
      if (etadge(i).eq.0d0) etadge(i)=etared(i)
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                              subroutine SETVIS
c
c ======================================================================
c
      function ETAETY (icomp,T,D)
c
c  Viscosity model for ethylene by Holland et al. (1983).
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   ETAETY--Viscosity [uPa-s]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-29-00 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      data g1,g2,g3,g4,g5,g6,g7,g8,g9,v1,v2,v3,v4,v5,v6,v7 /
     & -3.5098225018d6,2.5008406184d6,-5.8365540744d5,4.5549146583d3,
     &  2.2881683403d4,-4.7318682077d3,4.5022249258d2,-2.1490688088d1,
     &  4.1649263233d-1,-4.8544486732d0,1.3033585236d1,2.7808928908d4,
     & -1.8241971308d3,1.5913024509d0,-2.0513573927d2,-3.9478454708d4/
      ETAETY=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      dd=D*wmas(icomp)/1000d0
      tt=T**thrd
      eta0=g1/T+g2/tt**2+g3/tt+g4+g5*tt+g6*tt**2+g7*T+g8*tt**4+g9*tt**5
      etapr=EXP5(v1+v4/T)*(EXP5(dd**0.1d0*(v2+v3/T**1.5d0)+
     &      (dd/0.221d0-1d0)*XSQRT(dd)*(v5+v6/T+v7/T**2))-1d0)
      ETAETY=(eta0+etapr)/10d0

      end                                                function ETAETY
c
c ======================================================================
c
      function ETANEO (icomp,T,D)
c
c  Viscosity model for neon of
c  Rabinovich, V.A., Vasserman, A.A., Nedostup, V.I. and Veksler, L.S.
c  Thermophysical Properties of Neon, Argon, Krypton, and Xenon,
c  Hemisphere Publishing Corp., 1988.
c
c  The numbers calculated here do not exactly match those given by Rabinovich.
c  The ECS model is currently the preferred model.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   ETANEO--Viscosity [uPa-s]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  03-27-00 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      data a0,a1,a2,a3,a4,a5,a6/17.67484d0,-2.78751d0,311498.7d0,
     &   -48826500d0,3938774000d0,-1.654629d11,2.86561d12/
      data b0,b1,b2,b3,b4,b5/
     &   0.48148d0,-1.18732d0,2.80277d0,-5.41058d0,7.04779d0,-3.76608d0/
      data c0,c1,c2,c3,c4/
     &   1.03010d0,-0.99175d0,2.47127d0,-3.11864d0,1.57066d0/
      data d1,d2,d3,d4,d5/
     &   0.27676d0,0.014355d0,2.6480d0,-1.9643d0,0.89161d0/

      ETANEO=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      tr=T/0.29944d0
      y=a0+a1*log10(tr)+a2/tr**2+a3/tr**3+a4/tr**4+a5/tr**5+a6/tr**6
      etat=266.93d0*SQRT(T*wmas(icomp))/(y*0.68321d0)
      om=D/(1673d0/wmas(icomp))
      s=c0+c1*om+c2*om**2+c3*om**3+c4*om**4-          !The sign has been changed in Eq. 5.23 to negative.
     & (b0+b1*om+b2*om**2+b3*om**3+b4*om**4+b5*om**5)*log10(T/122.1d0)
      bD=D*1000d0*thrd2*xPi*xAvog*(s*0.000000000305d0)**3
      etad=1d0+d1*bD+d2*bD**2+d3*bD**3+d4*bD**4+d5*bD**5
      ETANEO=etad*etat/100d0

      end                                                function ETANEO
c
c ======================================================================
c
      function ETAR23 (icomp,T,D)
c
c  Viscosity model for R23.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   etaR23--Viscosity [uPa-s]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-01-00 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      ETAR23=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      c1=ceta(icomp,3,1)
      c2=ceta(icomp,4,1)
      dg=ceta(icomp,5,1)
      Dl=ceta(icomp,2,1)
      etamax=ceta(icomp,6,1)
      ddd=Dl-D
      del=D-dredeta(icomp)
      tau=T-Tredeta(icomp)
      etadg=ETA1DG(icomp,1,0,T)*XPOWR(ddd/Dl,c1)
      etars=(D/Dl)**c1*c2*Dl**2/ddd*SQRT(T)*EXP5(D/ddd*dg/xRgas/T)
      etacrt=4d0*etamax/(EXP5(del)+
     &       EXP5(-del))/(EXP5(tau)+EXP5(-tau))
      ETAR23=etadg+etars+etacrt

      end                                                function ETAR23
c
c ======================================================================
c
      function ETASF6 (icomp,T,D)
c
c  Viscosity model for SF6 of
c  Altunin, V.V., Equation for the viscosity of sulfur hexafluoride
c  for a broad range of temperatures and pressures
c  Tr. Mosk. Energ. Inst. 1984, 622, pp12-18
c
c  Copy is very poor and difficult to read the coefficients.
c  This equation is no longer used.

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension a(1:5,0:1)
      data a/0.911990483d0,-0.362880599d1,0.468880707d1,-0.204822413d1,
     &       0.299289894d0,-0.798662631d0,0.548041104d1,-0.615316823d1,
     &       0.258614144d1,-0.369541671d0/
      ETASF6=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      tau=T/318.7d0
      del=D*wmas(icomp)/738d0
      eta0=0.486d0+0.17075d2*tau-0.13976d1*tau**2
      sumi=0d0
      do k=1,5
        do j=0,1
          sumi=sumi+(a(k,j)*del**k)/tau**j
        enddo
      enddo
      ETASF6=eta0*EXP5(sumi)
      end                                                function ETASF6
c
c ======================================================================
c
      function ETAH2mct (icomp,T,D)
c
c  Old model for the viscosity of para and normal hydrogen, no longer used.
c  Taken from NIST12, Version 3.1, and modified to work with Refprop.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-20-99 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      ETAH2mct=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      tt=T
      if (T.gt.100.) tt=100d0
      exvdil=0d0
      if (D.gt.0) then
       dx=D*2.0159d0/1000d0
       dzz=dx/0.07d0
       a=EXP5(5.7694d0+XLOG(dx)+
     &   0.65d2*dx**1.5d0-6d-6*EXP5(127.2d0*dx))
       b=1d1+7.2d0*(dzz**6-dzz**1.5d0)-17.63d0*EXP5(-58.75d0*dzz**3)
       exvdil=a*EXP5(b/tt)/10d0
      endif
      s=0d0
      tf=T**thrd
      tff=1d0/tf**4d0
      do i=1,9
        tff=tff*tf
        s=s+ceta(icomp,i,1)*tff
      enddo
      ETAH2mct=exvdil+s*100d0
      end                                              function ETAH2mct
c
c ======================================================================
c
      function ETAH2 (icomp,T,D,ih2)
c
c  Model for the viscosity of normal hydrogen from symbolic regression of
c  Muzny, Huber, and Kazakov, J. Chem. Eng. Data, 58:969-979, 2013.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c    ETAH2--Viscosity [uPa-s]
c
c  11-16-12 MLH, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension sma(0:4),e(3),b(9),a(6)
      data sma /0.209630d0,-0.455274d0,
     &          0.143602d0,-0.335325d-1,0.276981d-2/  !Coefficient to match Moldover
      data b   /0.6022137d0,-0.187d0,2.4871d0,3.7151d0,-11.0972d0,
     &          9.0965d0,-3.8292d0,0.5166d0,0.297d0/
      data a   /0.00643449673d0,0.0456334068d0,0.232797868d0,
     &          0.958326120d0,0.127941189d0,0.363576595d0/
      data e   /0.021357d0,2.01588d0,0.297d0/

      ETAH2=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      if (icomp.eq.0) ts=0   !icomp is not yet used, this line keeps the compiler happy.
      ts=T/30.41d0
      sum1=0d0
      do k=0,4
        sum1=sum1+sma(k)*XLOG(ts)**k
      enddo
      etadil=e(1)*SQRT(e(2)*T)/(e(3)**2*EXP5(sum1))
      if (ih2.eq.2) etadil=etadil*SQRT(2d0) !Multiply dilute by SQRT(2) (mw ratio) for deuterium.
      beta1=b(1)*(b(2)+b(3)/ts+b(4)/ts**2+b(5)/ts**3+
     &      b(6)/ts**4+b(7)/ts**5+b(8)/ts**6)*b(9)**3
      eta1=etadil*(1d0+beta1*D)             !uPa.s
      etares=0d0
      if (D.gt.0.) then
        Tred=T/33.145d0
        dred=D*2.01588d0*0.011d0
        ee=a(2)*Tred+a(3)/Tred+a(4)*dred**2/(a(5)+Tred)+a(6)*dred**6
        etares=1000d0*(a(1)*dred**2*EXP5(ee))
        if (ih2.eq.2) etares=etares*1.18d0  !Factor added to match van Itterbeek liquid for deuterium.
      endif
      ETAH2=eta1+etares

      END                                                 function ETAH2
c
c ======================================================================
c
      function ETAHE (icomp,T,Dm)
c
c  Viscosity of helium taken from
c  Arp, V.D., McCarty, R.D., and Friend, D.G.,
c  Thermophysical Properties of Helium-4 from 0.8 to 1500 K with
c  Pressures to 2000 MPa,
c  NIST Technical Note 1334, Boulder, CO 1998.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c       Dm--Molar density [mol/L]
c
c  Output (as the function value):
c   ETAHE --Viscosity [uPa-s]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  07-06-98 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'

      dimension bb(5),cc(5),dd(5),ee(5),ff(5)
      data bb,cc,dd,ee,ff/-47.5295259d0,87.6799309d0,-42.0741589d0,
     &   8.33128289d0,-0.589252385d0,547.309267d0,-904.870586d0,
     &   431.404928d0,-81.4504854d0,5.37008433d0,-1684.39324d0,
     &   3331.08630d0,-1632.19172d0,308.804413d0,-20.2936367d0,
     &   -0.135311743d0,1.00347841d0,1.20654649d0,-0.149564551d0,
     &   0.0125208416d0,196d0,0.71938d0,12.451d0,-295.67d0,-4.1249d0/
      ETAHE=0d0
      if (T.le.0. .or. Dm.lt.0.) RETURN
      x=5.7037825d0
      if (T.le.300.) x=LOG(T)
      b=bb(1)/x+bb(2)+bb(3)*x+bb(4)*x**2+bb(5)*x**3
      c=cc(1)/x+cc(2)+cc(3)*x+cc(4)*x**2+cc(5)*x**3
      d=dd(1)/x+dd(2)+dd(3)*x+dd(4)*x**2+dd(5)*x**3
      e=ee(1)/x+ee(2)+ee(3)*x+ee(4)*x**2+ee(5)*x**3
      eta0a=EXP5(e)
      dmm=Dm*wmas(icomp)/1000d0
      emm=b*dmm+c*dmm**2+d*dmm**3
      if (emm.gt.100.) emm=100d0
      etae=EXP5(emm)
      if (T.gt.100.) then
        eta0b=ff(1)*T**ff(2)*EXP5(ff(3)/T+ff(4)/T**2+ff(5))
        eta0=eta0b
        if (T.lt.110.) eta0=eta0a+(eta0b-eta0a)*(T-100d0)/10d0
        ETAHE=(eta0a*etae+eta0-eta0a)/10d0
      else
        ETAHE=eta0a*etae/10d0
      endif

      end                                                 function ETAHE
c
c ======================================================================
c
      function ETAMEO (icomp,T,Dm)
c
c  Viscosity model for methanol of
c  Xiang, Huber, and Laesecke, J. Phys. Chem. Ref. Data, 4:1597-1620, 2006.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c       Dm--Molar density [mol/L]
c
c  Output (as the function value):
c  ETAMEOH--Viscosity [uPa-s]
c
c  Written by M.L. Huber, NIST Thermophysics Division, Boulder, Colorado
c  11-09-05 MLH, original version, based on code from HWX

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension h(9),d(7),e(9),f(17)
      data h/-19.572881d0,219.73999d0,-1015.3226d0,2471.01251d0,
     &  -3375.1717d0,2491.6597d0,-787.26086d0,14.085455d0,-.34664158d0/
      data d/-1.181909d0,0.5031030d0,-0.6268461d0,0.5169312d0,
     &  -0.2351349d0,5.3980235d-2,-4.9069617d-3/
      data e/4.018368d0,-4.239180d0,2.245110d0,-0.5750698d0,2.3021026d-2
     &  ,2.5696775d-2,-6.8372749d-3,7.2707189d-4,-2.9255711d-5/
      data f/1.16145d0,0.14874d0,0.52487d0,0.77320d0,2.16178d0,
     & 2.43787d0,0.10225d0,0.97346d0,0.10657d0,0.34528d0,0.44557d0,
     & 2.58055d0,0.4575d0,0.95976d-3,18.6222085d-4,9.990338d0,0.3408d-9/

c  Constants used in the correlation.
      ETAMEO=0d0
      if (T.le.0. .or. Dm.lt.0.) RETURN
      Dd=Dm
      if (Dd.lt.1d-20) Dd=1d-20
      rr=Dd*wmas(icomp)/273d0
      rd=Dd*xAvog*xPi*1000d0
      tr=T/512.6d0
      trep=T/577.87d0
      da=0d0                 !Calculate the hard sphere diameter, da, Eq 16.
      do kk=1,7
        da=da+d(kk)/tr**(kk-1)
      enddo
      do mm=1,9
        da=da+e(mm)*rr**mm
      enddo
      ele0=f(1)/trep**f(2)+f(3)/EXP5(f(4)*trep)+f(5)/EXP5(f(6)*trep)
      eled=f(7)/trep**f(8)+
     &     f(9)/EXP5(f(10)*trep)-f(11)/EXP5(f(12)*trep)
      ele0=ele0*(1d0+f(13)**2/(1d0+f(14)*f(13)**6)*eled)
      bnr=h(1)+h(2)/trep**0.25d0+h(3)/SQRT(trep)+h(4)/trep**0.75d0+  !b-coefficient,subcript,s
     &    h(5)/trep+h(6)/trep**1.25d0+h(7)/trep**1.5d0+h(8)/trep**2.5d0+
     &    h(9)/trep**5.5d0
      da=da*(6d0*rr/rd)**thrd
      b =2d0*da**3*rd/3d0
      bn=f(17)**3*rd/xPi
      cetam=f(15)*EXP5(f(16)/SQRT(trep))*trep**3*bn**2     !c-coefficient,subcript,T
      u0=5d0/16d0/f(17)**2*SQRT(rr/rd*xRgas*T/xAvog*273d0)/ele0
      gd=(1d0-b/8d0)/(1d0-b/4d0)**3
      cf=1d0/(1d0+EXP5(5d0*(rr-1d0)))
      paa0=u0*(1d0+bnr*bn+cetam)
      paa1=u0*(1d0/gd+0.8d0*b+0.761d0*gd*b**2)
      ETAMEO=(cf*paa0+(1d0-cf)*paa1)*1d6

      end                                                function ETAMEO
c
c ======================================================================
c
      function ETACO2 (icomp,T,D)
c
c  Model for the viscosity of CO2 from symbolic regression.
c  A. Laesecke and C.D. Muzny, JPCRD 46, 013107 (2017)
c  http://dx.doi.org/10.1063/1.4977429
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c        D--Molar density [mol/L]
c
c  Output (as the function value):
c   ETACO2--Viscosity [uPa-s]
c
c  Written by M.L. Huber, NIST Thermophysics Division, Boulder, Colorado
c  12-18-15 MLH, original version
c  03-21-17 MLH, revised to match final publication

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension sma(0:6),beta(9),tt(9),c(0:2)
      data c/8.06282737481277d0,0.360603235428487d0,0.121550806591497d0/
      data tt/0d0,0.25d0,0.5d0,0.75d0,1d0,1.25d0,1.5d0,2.5d0,5.5d0/
      data sma/1749.354893188350d0,-369.069300007128d0,
     &      5423856.34887691d0,-2.21283852168356d0,-269503.247933569d0,
     &      73145.021531826d0,5.34368649509278d0/
      data eps,sig,etatl,Dtl
     &  /200.75967461d0,3.7842107472d-10,0.0943605819425392d0,1178.53d0/
      data beta/-19.572881d0, 219.73999d0,-1015.3226d0,2471.0125d0,
     &  -3375.1717d0,2491.6597d0,-787.26086d0,14.085455d0,-0.34664158d0/

      ETACO2=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      wm=44.0095d0
      denom1= sma(0)+sma(1)*T**(1d0/6d0)+sma(2)*EXP5(sma(3)*T**thrd)
      denom2=(sma(4)+sma(5)*T**thrd)/EXP5(T**thrd)+sma(6)*SQRT(T)
      denom=denom1+denom2
      etadil=1.0055d0*SQRT(T)/denom         !mPa.s
      ts=T/eps
      tr=T/Ttpn(icomp)
      dkg=D*wm
      Dr=dkg/Dtl
      sumb=beta(1)+SUM(beta(2:9)/ts**tt(2:9))
      eta1=etadil*sumb*dkg*xAvog*sig**3/wm*1000d0
      resid=etatl*(c(1)*tr*Dr**3+(Dr**2+Dr**c(0))/(tr-c(2)))
      ETACO2=(etadil+eta1+resid)*1000d0

      end                                                function ETACO2
c
c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                  End file TRNS_VIS.FOR
c ======================================================================

