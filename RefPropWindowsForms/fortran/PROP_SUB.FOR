c  Begin file PROP_SUB.FOR
c
c  Below are the routines contained in this file.  All of these are
c  accessible through the ALLPROPS routine.  If called separately from
c  the REFPROP and ALLPROPS routines, the subroutine SETUP must first
c  be called to initialize the equations or state, transport equations,
c  and so on required for property calculation.
c
c     function PHIK (icomp,itau,idel,tau,del)
c     function PHIX (itau,idel,tau,del,z)
c     subroutine REDX (z,Tred,Dred)
c     subroutine RMIX (z)
c     subroutine MAXT (z,Tm,Pm,Dm,ierr,herr)
c     subroutine MAXP (z,Tm,Pm,Dm,ierr,herr)
c     subroutine CRITP (z,Tc,Pc,Dc,ierr,herr)
c     subroutine PRESS (T,D,z,P)
c     subroutine DPDDX (T,D,z,dPdD)
c     subroutine DPDD2 (T,D,z,d2PdD2)
c     subroutine DPDTX (T,D,z,dPdT)
c     subroutine THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
c     subroutine THERM0 (T,D,z,P0,e0,h0,s0,Cv0,Cp00,w0,a0,g0)
c     subroutine THERM2 (T,D,z,P,e,h,s,Cv,Cp,w,zz,hjt,a,g,
c    &                   xkappa,beta,dPdD,d2PdD2,dPdT,dDdT,dDdP,
c    &                   d2PdT2,d2PdTD,spare3,spare4)
c     subroutine THERM3 (T,D,z,xkappa,beta,xisenk,xkt,
c    &                   betas,bs,xkkt,thrott,pi,spht)
c     subroutine DERVPVT1 (T,D,z,P,dPdD,dPdT,dDdP,dDdT,dTdP,dTdD)
c     subroutine DERVPVT (T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
c    &                          dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
c    &                          dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
c     subroutine VIRBCD12 (T,z,B,C,D,E,Bx1,Bx2,Cx1,Cx2,Dx1,Dx2,Ba,Ca)
c     subroutine RESIDUAL (T,D,z,Pr,er,hr,sr,Cvr,Cpr,ar,gr)
c     subroutine PHIDERV (iderv,T,D,z,dadn,dnadn,ierr,herr)
c     subroutine VIRBCD (T,z,B,C,D,E)
c     subroutine GIBBS (T,D,z,ar,gr)
c     subroutine ENTRO (T,D,z,s)
c     subroutine DHD1 (T,D,z,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,dhdp_d)
c     subroutine DSD1 (T,D,z,dsdt_d,dsdt_p,dsdd_t,dsdd_p,dsdp_t,dsdp_d)
c     subroutine AG (T,D,z,a,g)
c     subroutine FPV (T,D,P,z,Fpvx)
c     subroutine RIEM (T,D,z,riemc)
c     subroutine FGCTY (T,D,z,f)
c     subroutine FGCTY2 (T,D,z,f,ierr,herr)
c     subroutine CHEMPOT (T,D,z,u,ierr,herr)
c     subroutine FUGCOF (T,D,z,phi,ierr,herr)
c     subroutine HEAT (T,D,z,hg,hn,ierr,herr)
c     subroutine B12 (T,z,B)
c     subroutine CSTAR (T,P,v,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
c     subroutine EXCESS (T,P,z,kph,D,vE,eE,hE,sE,aE,gE,ierr,herr)
c     subroutine ENTHHC (icmb,T1,T2,h)
c     subroutine HEATFRM (T,D,z,hFrm,ierr,herr)
c     subroutine SPECIES (k,Frml,aCrb,aHyd,aOxy,aNit,
c    &                    aSul,aHe,aArg,aChl,aFl,ierr,herr)
c     subroutine MASSFLUX (Tm,P,z,beta,rf,fluxm,Cs,T0,P0,xMach,u,Ts,Ps,
c    &                     ierr,herr)
c
c ======================================================================
c
      function PHIK (icomp,itau,idel,tau,delta)
c
c  Compute reduced Helmholtz energy or a derivative as functions
c  of dimensionless temperature and density.
c
c  The Helmholtz energy consists of ideal and residual (real-gas)
c  terms; this routine calculates only the residual part.
c
c  This function computes pure component properties only;
c  call PHIX instead for mixtures.
c
c  The reducing parameters Tr and Dr are often, but not necessarily,
c  equal to the critical temperature and density for pure fluids.
c
c  Inputs:
c    icomp--Pointer specifying component (1..nc)
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           When itau = 0 and idel = 0, compute A/RT.
c           When itau = 0 and idel = 1, compute 1st density derivative.
c           When itau = 1 and idel = 1, compute cross derivative.
c           etc.
c      tau--Dimensionless temperature (Tr/T)
c    delta--Dimensionless density (D/Dr)
c
c  Output (as the function value):
c      phi--Residual (real-gas) part of the Helmholtz energy, or one
c           of its derivatives (as specified by itau and idel),
c           in reduced form (A/RT)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-07-96  MM, original version
c  06-20-16 EWL, add call to RMIX so that it can be removed in all other routines calling this

      include 'COMMONS.INC'
      include 'COMFEQ.INC'

      if (iEOSk(icomp).eq.iEOSFEQ) then
        PHIK=PHIFEQ(icomp,itau,idel,tau,delta)     !Call Helmholtz-energy model.
      elseif (iEOSk(icomp).eq.iEOSECS) then
        PHIK=PHIECS(icomp,itau,idel,tau,delta)     !Call extended corresponding states model.
      elseif (iEOSk(icomp).eq.iEOSPR .or. iEOSk(icomp).eq.iEOSPR3) then
        PHIK=PHIPR(icomp,itau,idel,tau,delta)      !Call Peng-Robinson model.
      else
        PHIK=-9.99d99                            !Model not found, but no way to return an error from here.
      endif
      end                                                  function PHIK
c
c ======================================================================
c
      function PHIX (itau,idel,tau,delta,z)
c
c  Compute reduced Helmholtz energy or a derivative as functions
c  of dimensionless temperature and density by calling the appropriate
c  mixture model.
c
c  The Helmholtz energy consists of ideal-gas and residual (real-
c  gas) terms.  The residual term consists of ideal-solution and
c  mixing terms.  This routine calculates only the residual term.
c
c  Inputs:
c     itau--Flag specifying the order of the temperature derivative
c     idel--Flag specifying the order of the density derivative
c           When itau = 0 and idel = 0, compute A/RT.
c           When itau = 0 and idel = 1, compute 1st density derivative.
c           When itau = 1 and idel = 1, compute cross derivative.
c           etc.
c      tau--Dimensionless temperature (Tr/T)
c    delta--Dimensionless density (D/Dr)
c        z--Composition (array of mole fractions)
c
c  Output (as the function value):
c    phixx--Residual (real-gas) part of the Helmholtz energy, or one
c           of its derivatives (as specified by itau and idel),
c           in reduced form (A/RT)

      include 'COMMONS.INC'
      dimension z(ncmax)     !,phi(0:3,0:3)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PHIX

      PHIX=-9.99d99
      call ISPURE (z,icomp)
      if (lTest) then
!        lt=ABS(tau-tausv2).lt.1d-12   !Check for change in tau.
!        ld=ABS(del-delsv2).lt.1d-16   !Check for change in del.
!        if (.not.lt .or. .not.ld) then
!          do i=1,nc
!            if (z(i).gt.0) then
!c             call PHIFEQ3 (i,itau,idel,tau,del,phi)
!              call PHIFEQ3 (i,3,3,tau,del,phi)
!              phisv2(0:3,0:3,i)=phi(0:3,0:3)
!            endif
!            do j=i+1,nc
!              zij=z(i)*z(j)*fmix(5,i,j)         !Assumes KW only.
!              if (zij.gt.0) then
!c               call PHIMIX3 (itau,idel,tau,del,z,phi)
!                call PHIMIX3 (3,3,tau,del,z,phi)
!                phism2(0:3,0:3,ncode2(i,j))=phi(0:3,0:3)
!              endif
!            enddo
!          enddo
!        endif
!        if (icomp.ne.0) then
!          PHIX=phisv2(itau,idel,icomp)
!        else
!          PHIX=0d0
!          do i=1,nc
!            if (z(i).gt.0) PHIX=PHIX+z(i)*phisv2(itau,idel,i)
!            do j=i+1,nc
!              zij=z(i)*z(j)*fmix(5,i,j)         !Assumes KW only.
!              if (zij.gt.0) PHIX=PHIX+zij*phism2(itau,idel,ncode2(i,j))
!            enddo
!          enddo
!        endif
!        if (lCache1) then
!          tausv2=tau        !Cache the value of tau for use in the next call to this routine.
!          delsv2=delta
!        endif

      elseif (icomp.ne.0) then
        PHIX=PHIK(icomp,itau,idel,tau,delta)       !Pure fluid, call PHIK instead.
      elseif (iEOSmx.eq.iEOSHMX) then
        if (.not.lRcnst .or. .not.lRcnstWag) call RMIX (z)
        PHIX=PHIHMX(itau,idel,tau,delta,z)         !Call Helmholtz-energy model.
      elseif (lAGA8) then
        PHIX=PHIAGA(itau,idel,tau,delta,z)         !Call AGA8 model.
      elseif (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) then
        PHIX=PHIPRX(itau,idel,tau,delta,z)         !Call Peng-Robinson model.
      endif
      end                                                  function PHIX
c
c ======================================================================
c
      subroutine REDX (z,Tred,Dred)
c
c  Returns the reducing parameters associated with mixture EOS;
c  used to calculate the 'tau' and 'del', which are the independent
c  variables in the EOS.
c
c  Input:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     Tred--Reducing temperature [K]
c     Dred--Reducing molar density [mol/L]
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::REDX

      call ISPURE (z,icomp)
      Tred=300d0
      Dred=1d0
      if (icomp.ne.0) then
        Tred=Tzred(icomp)
        Dred=Dzred(icomp)
      elseif (iEOSmx.eq.iEOSHMX) then
        call RDXHMX (0,0,0,z,Tred,Dred,ierr,herr)          !Call Helmholtz-energy model.
      elseif (lAGA8) then
        call RDXHMX (0,0,0,z,Tred,Dred,ierr,herr)          !Call AGA8 model.
      elseif (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) then
        call REDPR (z,Tred,Dred)                           !Call Peng-Robinson model.
      endif
      if (Tred.le.0) Tred=300d0
      if (Dred.le.0) Dred=1d0
      if (lGERG08 .or. lAGA8) then
        Rxgas=8.314472d0
      elseif (lRcnst .and. lRcnstWag) then                 !Set R to current value if flags have been set.
        Rxgas=xRgas
      else
        if (icomp.ne.0) Rxgas=Reos(icomp)
      endif

      end                                                subroutine REDX
c
c ======================================================================
c
      subroutine RMIX (z)
c
c  Calculate the gas "constant" as a function of the molar composition
c  of the fluids in the mixture.  This constant changes over
c  time with new measurements.  However, each equation of state keeps
c  the value used at the time of its development, and thus the need
c  here to normalize it with respect to composition.
c
c  Input:
c        z--Composition (array of mole fractions)
c
c  Output:
c    Rxgas--Gas constant [J/(mol-K)] (loaded into common block)

      include 'COMMONS.INC'
      dimension z(ncmax)

      if (lGERG08 .or. lAGA8) then          !If the GERG is on, set R to the value used by that model.
        Rxgas=8.314472d0
      elseif (lRcnst .and. lRcnstWag) then  !Set R to current value if flags have been set.
        Rxgas=xRgas
      else
        if (nc.le.1) then
          Rxgas=Reos(1)
        elseif (icnc.gt.0) then
          Rxgas=Reos(icnc)
        else
          Rxgas=SUM(z(1:nc)*Reos(1:nc))
          if (Rxgas.lt.1d-10) Rxgas=xRgas          !Check for bad z(i).
        endif
      endif
      end                                                subroutine RMIX
c
c ======================================================================
c
      subroutine RMIX2 (z,R)
c
c  Mimic RMIX but return the gas constant as a parameter for use in the DLL.
c
c  Input:
c        z--Composition (array of mole fractions)
c
c  Output:
c        R--Gas constant [J/(mol-K)] (loaded into common block)

      include 'COMMONS.INC'
      dimension z(ncmax)
      call RMIX (z)
      R=Rxgas
      end                                               subroutine RMIX2
c
c ======================================================================
c
      subroutine MAXT (z,Tm,Pm,Dm,ierr,herr)
c
c  Calculate values at the maximum temperature along the saturation line;
c  these are returned from the call to SATSPLN and apply only to the
c  composition in the z() array sent to SATSPLN.
c
c  Input:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       Tm--Temperature [K]
c       Pm--Pressure [kPa]
c       Dm--Density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      331 - Splines not available for calculation
c                     -361 - Maximum temperature not known
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character*255 herr,herr2
      dimension z(ncmax),y(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MAXT

      Tm=300d0
      Pm=0d0
      Dm=0d0
      ierr=0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        call CRITP (z,Tm,Pm,Dm,ierr,herr)
      else
        if (iSpline.ne.0) then
          if (ierrTmx.eq.0) then
            zSum=SUM(ABS(z(1:nc)-zSpln(1:nc)))
            if (lEQ0(zSum)) then
              Tm=TmxT
              Pm=TmxP
              Dm=Tmxd
            endif
          endif
        else
          call ERRNUM (331,0,'MAXT',' ',0d0,0d0,0d0,ierr,herr)
        endif
        if (Dm.lt.0.01d0 .and. ierr.eq.0)
     &    call ERRNUM (-361,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      endif
      end                                                subroutine MAXT
c
c ======================================================================
c
      subroutine MAXP (z,Tm,Pm,Dm,ierr,herr)
c
c  Calculate values at the maximum pressure along the saturation line;
c  these are returned from the call to SATSPLN and apply only to the
c  composition in the z() array sent to SATSPLN.
c
c  Input:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       Tm--Temperature [K]
c       Pm--Pressure [kPa]
c       Dm--Density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      331 - Splines not available for calculation
c                     -362 - Maximum pressure not known
c     herr--Error string (character*255)
c
      include 'COMMONS.INC'
      character*255 herr,herr2
      dimension z(ncmax),y(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MAXP

      Tm=300d0
      Pm=0d0
      Dm=0d0
      ierr=0
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        call CRITP (z,Tm,Pm,Dm,ierr,herr)
      else
        if (iSpline.ne.0) then
          if (ierrPmx.eq.0) then
            zSum=SUM(ABS(z(1:nc)-zSpln(1:nc)))
            if (lEQ0(zSum)) then
              Tm=PmxT
              Pm=PmxP
              Dm=Pmxd
            endif
          endif
        else
          call ERRNUM (331,0,'MAXP',' ',0d0,0d0,0d0,ierr,herr)
        endif
        if (Dm.lt.0.01d0 .and. ierr.eq.0)
     &    call ERRNUM (-362,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      endif
      end                                                subroutine MAXP
c
c ======================================================================
c
      subroutine CRITP (z,Tc,Pc,Dc,ierr,herr)
c
c  Calculate critical parameters as a function of composition.
c  The critical parameters are estimates based on polynomial
c  fits to the binary critical lines.  For 3 or more components,
c  combining rules are applied to the constituent binaries.
c
c  If SATSPLN has been called and the input composition sent here is the
c  same as that sent to SATSPLN, the values calculated from the splines
c  are returned, which are nearly exact.  During the call to SATSPLN,
c  the true critical point, maximum pressure point, and maximum
c  temperature point along the saturation lines are determined.
c  Without the splines and for a system with three or more components,
c  the values from this routine are only rough estimates.
c
c  Input:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       Tc--Critical temperature [K]
c       Pc--Critical pressure [kPa]
c       Dc--Critical density [mol/L]
c     ierr--Error flag:  0 - Successful
c                        (See subroutine CRTHMX for error numbers.)
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CRITP

      ierr=0
      herr=' '
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        Tc=Tcrit(icomp)
        Pc=Pcrit(icomp)
        Dc=Dcrit(icomp)
      else
        call CRTHMX (z,Tc,Pc,Dc,ierr,herr)
        if (ierr.ge.313 .and. ierr.le.330) ierr=-ierr  !Don't send back an error number for errors between 313 and 330.  If the error number is needed, call CRTHMX instead.
      endif
      if (Tc.le.0) Tc=100d0
      if (Pc.le.0) Pc=1d0
      if (Dc.le.0) Dc=1d0
      end                                               subroutine CRITP
c
c ======================================================================
c
      subroutine PRESS (T,D,z,P)
c
c  Compute pressure as a function of temperature, density, and composition.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c        P--Pressure [kPa]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PRESS

      if (T.le.0. .or. D.lt.0.) RETURN
      call REDX (z,T0,D0)
      P=Rxgas*T*D*(1d0+PHIX(0,1,T0/T,D/D0,z))
      end                                               subroutine PRESS
c
c ======================================================================
c
      subroutine DPDDX (T,D,z,dPdD)
c
c  Compute partial derivative of pressure w.r.t. density at constant
c  temperature as a function of temperature, density, and composition.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c     dPdD--dP/dD [kPa-L/mol]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DPDDX

      if (T.le.0. .or. D.lt.0.) RETURN
      call REDX (z,T0,D0)
      phi01=PHIX(0,1,T0/T,D/D0,z)      !Real-gas terms
      phi02=PHIX(0,2,T0/T,D/D0,z)
      dPdD=Rxgas*T*(1d0+2d0*phi01+phi02)
      end                                               subroutine DPDDX
c
c ======================================================================
c
      subroutine DPDD2 (T,D,z,d2PdD2)
c
c  Compute second partial derivative of pressure w.r.t. density at constant
c  temperature as a function of temperature, density, and composition.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c   d2PdD2--d^2P/dD^2 [kPa-L^2/mol^2]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DPDD2

      if (T.le.0. .or. D.lt.0.) RETURN
      call REDX (z,T0,D0)
      phi01=PHIX(0,1,T0/T,D/D0,z)      !Real-gas terms
      phi02=PHIX(0,2,T0/T,D/D0,z)
      phi03=PHIX(0,3,T0/T,D/D0,z)
      if (D.gt.1d-8) then
        d2PdD2=Rxgas*T/D*(2d0*phi01+4d0*phi02+phi03)
      else
        call VIRB (T,z,B)
        d2PdD2=2d0*B*Rxgas*T
      endif
      end                                               subroutine DPDD2
c
c ======================================================================
c
      subroutine DPDTX (T,D,z,dPdT)
c
c  Compute partial derivative of pressure w.r.t. temperature at constant
c  density as a function of temperature, density, and composition.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c     dPdT--dP/dT [kPa/K]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DPDTX

      dPdT=0d0
      if (T.le.0. .or. D.lt.0.) RETURN
      call REDX (z,T0,D0)
      phi01=PHIX(0,1,T0/T,D/D0,z)      !Real-gas terms
      phi11=PHIX(1,1,T0/T,D/D0,z)
      dPdT=Rxgas*D*(1d0+phi01-phi11)
      end                                               subroutine DPDTX
c
c ======================================================================
c
      subroutine THERM0 (T,D,z,P0,e0,h0,s0,Cv0,Cp00,w0,a0,g0)
c
c  Compute ideal-gas thermal quantities as a function of temperature,
c  density, and composition from core functions.
c
c  This routine is the same as THERM, except it only calculates ideal
c  gas properties (Z=1) at any temperature and density.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       P0--Pressure [kPa]
c       e0--Internal energy [J/mol]
c       h0--Enthalpy [J/mol]
c       s0--Entropy [J/mol-K]
c      Cv0--Isochoric heat capacity [J/mol-K]
c     Cp00--Isobaric heat capacity [J/mol-K]
c       w0--Speed of sound [m/s]
c       a0--Helmholtz energy [J/mol]
c       g0--Gibbs free energy [J/mol]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THERM0

      if (T.le.0. .or. D.lt.0.) RETURN
      Ds=D
      if (D.lt.1d-40) Ds=1d-40         !Entropy calculation will crash if D = 0.

c  Get derivatives of the Helmholtz energy.
      call ISPURE (z,icomp)
      phig00=PHI0(0,0,T,Ds,z)          !Ideal-gas terms
      phig10=PHI0(1,0,T,D,z)
      phig20=PHI0(2,0,T,D,z)

c  Calculate properties from the derivatives.
      RT=Rxgas*T
      P0=RT*D
      e0=RT*phig10
      s0=Rxgas*(phig10-phig00)
      a0=RT*phig00
      h0=e0+RT
      g0=a0+RT
      Cv0=-Rxgas*phig20
      Cp00=Cv0+Rxgas
      w=0d0                  !If any of the factors in speed of sound are
      w2=0d0                 ! negative, return a value of zero.
      if (Cv0.gt.0.) w2=RT*Cp00/Cv0
      if (w2.gt.0.)  w0=SQRT(w2*1d3/WMOL(z))
      if (D.lt.1d-40) then
        s0=xinf
        a0=xinf
        g0=xinf
      endif

      end                                              subroutine THERM0
c
c ======================================================================
c
      subroutine THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
c
c  Compute thermal quantities as a function of temperature, density,
c  and composition from core functions (Helmholtz energy, ideal
c  gas heat capacity, and various derivatives and integrals).
c
c  .. warning::
c
c      Do NOT call this routine for two-phase states,
c      otherwise it will return a metastable state if near the phase
c      boundary or complete nonsense at other conditions.  The value of q
c      that is returned from the flash routines will indicate a two phase
c      state by returning a value between 0 and 1.  In such a situation,
c      properties can only be calculated for the saturated liquid
c      and vapor states.  For example, when calling PHFLSH::
c
c          call PHFLSH (P,h,z,T,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
c
c      If q>0 and q<1, then values of the liquid compositions will
c      be returned in the x and y arrays, and the properties of the
c      liquid and vapor states can be calculated, for example, as::
c
c          call ENTRO (T,Dl,x,sliq)
c          call ENTRO (T,Dv,x,svap)
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        P--Pressure [kPa]
c        e--Internal energy [J/mol]
c        h--Enthalpy [J/mol]
c        s--Entropy [J/mol-K]
c       Cv--Isochoric heat capacity [J/mol-K]
c       Cp--Isobaric heat capacity [J/mol-K]
c        w--Speed of sound [m/s]
c      hjt--Isenthalpic Joule-Thomson coefficient [K/kPa]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  10-11-94  MM, original version
c  06-20-16 EWL, shorten code by removing BWR section and comments from before the year 2006 or that are no longer necessary
c                The BWR equations can still be used, but the calculated values are returned by function PHIX.
c                Checks for pure fluids are no longer done, the routine PHIX already makes this check.

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THERM

      if (T.le.0. .or. T.gt.1d15 .or. D.lt.0.) RETURN
      Ds=D
      if (D.lt.1d-40) Ds=1d-40         !Entropy calculation will crash if D = 0.
      call RMIX(z)

c  Get derivatives of the Helmholtz energy.
      call ISPURE (z,icomp)
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phig00=PHI0(0,0,T,Ds,z)          !Ideal-gas terms
      phig10=PHI0(1,0,T,D,z)
      phig20=PHI0(2,0,T,D,z)
      phi00 =PHIX(0,0,tau,del,z)       !Real-gas terms
      phi01 =PHIX(0,1,tau,del,z)
      phi10 =PHIX(1,0,tau,del,z)
      phi11 =PHIX(1,1,tau,del,z)
      phi02 =PHIX(0,2,tau,del,z)
      phi20 =PHIX(2,0,tau,del,z)

c  Calculate properties from the derivatives.
      RT=Rxgas*T
      P=RT*D*(1d0+phi01)
      e=RT*(phig10+phi10)
      s=Rxgas*(phig10+phi10-phig00-phi00)
      h=e+RT*(1d0+phi01)
      Cv=-Rxgas*(phi20+phig20)
      Cp=Cv+Rxgas*(1d0+phi01-phi11)**2/(1d0+2d0*phi01+phi02)
      w=0d0                  !If any of the factors in speed of sound are
      w2=0d0                 ! negative, return a value of zero.
      wm=WMOL(z)
      if (Cv.gt.0.) w2=RT*Cp/Cv*(1d0+2d0*phi01+phi02)/wm
      if (w2.gt.0.) w=SQRT(w2*1d3)
      hjt=0d0
      if (D.gt.1d-8 .and. Cp.gt.1d-8) then
        hjt=-(phi01+phi02+phi11)/(1d0+2d0*phi01+phi02)/(Cp*D)
      else
        call VIRB (T,z,B)
        call DBDT (T,z,dBT)
        if (Cp.gt.1d-8) hjt=(dBT*T-B)/Cp
        if (D.lt.1.d-40) s=xinf
      endif

      end                                               subroutine THERM
c
c ======================================================================
c
      subroutine THERM1 (T,D,z,P,e,h,s,g)
c
c  Compute thermal quantities as a function of temperature, density,
c  and composition from core functions (Helmholtz energy, ideal
c  gas heat capacity, and various derivatives and integrals).
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        P--Pressure [kPa]
c        g--Gibbs free energy [J/mol]
c        e--Internal energy [J/mol]
c        h--Enthalpy [J/mol]
c        s--Entropy [J/mol-K]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THERM1

      if (T.le.0. .or. D.lt.0.) RETURN
      Ds=D
      if (D.lt.1d-40) Ds=1d-40         !Entropy calculation will crash if D = 0.

c  Get derivatives of the Helmholtz energy.
      call ISPURE (z,icomp)
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phig00=PHI0(0,0,T,Ds,z)          !Ideal-gas terms
      phig10=PHI0(1,0,T,D,z)
      phi00 =PHIX(0,0,tau,del,z)       !Real-gas terms
      phi01 =PHIX(0,1,tau,del,z)
      phi10 =PHIX(1,0,tau,del,z)

c  Calculate properties from the derivatives.
      RT=Rxgas*T
      P=RT*D*(1d0+phi01)
      e=RT*(phig10+phi10)
      s=Rxgas*(phig10+phi10-phig00-phi00)
      h=e+RT*(1d0+phi01)
      g=h-T*s
      if (D.lt.1.d-40) then
        s=xinf
        g=xinf
      endif
      end                                              subroutine THERM1
c
c ======================================================================
c
      subroutine THERM2 (T,D,z,P,e,h,s,Cv,Cp,w,zz,hjt,a,g,
     &                   xkappa,beta,dPdD,d2PdD2,dPdT,dDdT,dDdP,
     &                   d2PdT2,d2PdTD,spare3,spare4)
c
c  Compute thermal quantities as a function of temperature, density,
c  and composition.  This routine is the simply the combination of
c  several others.  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       zz--Compressibility factor (= PV/RT) [-]
c  (See subroutines THERM, THERM3, AG, and DERVPVT for the description of all variables.)

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THERM2

      if (T.le.0. .or. D.lt.0.) RETURN
      spare3=xnotc
      spare4=xnotc
      call AG (T,D,z,a,g)
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      call DERVPVT (T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                    dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                    dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
      if (D.gt.0d0) zz=P/D/Rxgas/T
      beta=1d0/T
      xkappa=xnotc
      if (D.gt.1d-20) beta=-dDdT/D
      if (D.gt.1d-20) xkappa=dDdP/D

      end                                              subroutine THERM2
c
c ======================================================================
c
      subroutine THERM3 (T,D,z,xkappa,beta,xisenk,xkt,
     &                   betas,bs,xkkt,thrott,pi,spht)
c
c  Compute miscellaneous thermodynamic properties.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c   xkappa--Isothermal compressibility [1/kPa]
c     beta--Volume expansivity [1/K]
c   xisenk--Isentropic expansion coefficient [-]
c      xkt--Isothermal expansion coefficient [-]
c    betas--Adiabatic compressibility [1/kPa]
c       bs--Adiabatic bulk modulus [kPa]
c     xkkt--Isothermal bulk modulus [kPa]
c   thrott--Isothermal throttling coefficient [L/mol]
c       pi--Internal pressure [kPa]
c     spht--Specific heat input [J/mol]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  06-16-06 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THERM3

      if (T.le.0. .or. D.lt.0.) RETURN
      call THERM2 (T,D,z,P,e,h,s,Cv,Cp,w,zz,hjt,A,G,xkappa,beta,
     &             dPdD,d2PdD2,dPdT,dDdT,dDdP,d2PdT2,d2PdTD,sp3,sp)
      wmm=WMOL(z)
      if (P.le.1.d-40) then
        xkt=1d0
        xisenk=w**2/Rxgas/T*wmm/1000d0
      else
        xkt=D/P*dPdD                   !Isothermal expansion coefficient
        xisenk=w**2*D/P*wmm/1000d0     !Isentropic expansion coefficient
      endif
      betas=xinf
      if (D.gt.1d-40 .and. w.gt.0.) betas=1d0/D/w**2/wmm*1000d0      !Adiabatic compressibility
      bs=xisenk*P            !Adiabatic bulk modulus
      xkkt=xkt*P             !Isothermal bulk modulus
      thrott=-hjt*Cp         !Isothermal throttling coef.
      if (ABS(dPdT).gt.1d-6) then
        pi=T*dPdT-P          !Internal pressure
        spht=D*Cp*dPdD/dPdT            !Specific heat input
      else
        pi=0d0
        spht=Cp*T
      endif
      end                                              subroutine THERM3
c
c ======================================================================
c
      subroutine DERVPVT1 (T,D,z,P,dPdD,dPdT,dDdP,dDdT,dTdP,dTdD)
c
c  Compute 1st order derivatives of temperature, pressure, and density
c  from core functions for Helmholtz energy equations only.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     dPdD--dP/dD at constant T [kPa/(mol/dm^3)]
c     dPdT--dP/dT at constant D [kPa/K]
c     dDdP--dD/dP at constant T [mol/(dm^3-kPa)]
c     dDdT--dD/dT at constant P [mol/(dm^3-K)]
c     dTdP--dT/dP at constant D [K/kPa]
c     dTdD--dT/dD at constant P [K/(mol/dm^3)]
c
      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DERVPVT1

      if (T.le.0. .or. D.lt.0.) RETURN
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phi01=PHIX(0,1,tau,del,z)
      phi02=PHIX(0,2,tau,del,z)
      phi11=PHIX(1,1,tau,del,z)

      u    = 1d0+phi01-phi11
      v    = 1d0+2d0*phi01+phi02
      RT   = Rxgas*T
      P    = RT*D*(1d0+phi01)
      dPdD = RT*v
      dPdT = Rxgas*D*u
      dDdT =-D*u/(T*v)
      dDdP = 1d0/dPdD
      if (D.gt.1d-20) then
        dTdP = 1d0/dPdT
        dTdD = 1d0/dDdT
      else
        dTdP = xinf
        dTdD = xinf
      endif

      end                                            subroutine DERVPVT1
c
c ======================================================================
c
      subroutine DERVPVT (T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                          dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                          dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
c
c  Compute 1st and 2nd order derivatives of temperature, pressure, and
c  density from core functions for Helmholtz energy equations only.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     dPdD--dP/dD at constant T [kPa/(mol/dm^3)]
c     dPdT--dP/dT at constant D [kPa/K]
c     dDdP--dD/dP at constant T [mol/(dm^3-kPa)]
c     dDdT--dD/dT at constant P [mol/(dm^3-K)]
c     dTdP--dT/dP at constant D [K/kPa]
c     dTdD--dT/dD at constant P [K/(mol/dm^3)]
c   d2PdD2--d^2P/dD^2 at constant T [kPa/(mol/dm^3)^2]
c   d2PdT2--d^2P/dT^2 at constant D [kPa/K^2]
c   d2PdTD--d^2P/dTdD [J/mol-K]     [kPa/K/(mol/dm^3)]
c   d2DdP2--d^2D/dP^2 at constant T [(mol/dm^3)/kPa^2]
c   d2DdT2--d^2D/dT^2 at constant P [(mol/dm^3)/K^2]
c   d2DdPT--d^2D/dPdT [J/mol-K]     [(mol/dm^3)/(kPa-K)]
c   d2TdP2--d^2T/dP^2 at constant D [K/kPa^2]
c   d2TdD2--d^2T/dD^2 at constant P [K/(mol/dm^3)^2]
c   d2TdPD--d^2T/dPdD [J/mol-K]     [K/kPa/(mol/dm^3)]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  08-21-12 EWL, original version; based on THERM2
c  06-20-16 EWL, see comments under THERM

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DERVPVT

      if (T.le.0. .or. D.lt.0.) RETURN
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phi01=PHIX(0,1,tau,del,z)
      if (ABS(phi01).gt.1d50) RETURN
      phi11=PHIX(1,1,tau,del,z)
      phi12=PHIX(1,2,tau,del,z)
      phi21=PHIX(2,1,tau,del,z)
      phi02=PHIX(0,2,tau,del,z)
      phi03=PHIX(0,3,tau,del,z)

      u    = 1d0+phi01-phi11
      dut  = phi21
      dud  = phi01+phi02-phi11-phi12
      v    = 1d0+2d0*phi01+phi02
      dvt  =-2d0*phi11-phi12
      dvd  = 2d0*phi01+4d0*phi02+phi03

      RT     = Rxgas*T
      dPdD   = RT*v
      dPdT   = Rxgas*D*u
      dDdT   =-D*u/(T*v)
      dDdP   = 1d0/dPdD
      d2PdT2 = Rxgas*D*dut/T
      d2PdTD = Rxgas*(v+dvt)
      d2DdPT =-(v+dvt-u/v*dvd)/T/RT/v**2
      d2DdT2 = D*u/v**2/T**2*(u+v-u/v*dvd+dvt+dud-v/u*dut)

      if (D.gt.1d-10) then   !Don't decrease, could result in d2PdD2 going to zero.
        dTdP   = 1d0/dPdT
        dTdD   = 1d0/dDdT
        d2PdD2 = RT/D*dvd
        d2DdP2 =-dvd/v**3/D/RT**2
        d2TdP2 =-dut/(Rxgas*D)**2/T/u**3
        d2TdD2 = T*v/D**2/u**2*(u+v-u/v*dvd+dvt+dud-v/u*dut)
        d2TdPD =-(u+dud-v/u*dut)/Rxgas/(D*u)**2
      else
        call VIRB (T,z,B)
        d2PdD2 = 2d0*B*Rxgas*T
        d2DdP2 =-2d0*B/RT**2
        dTdP   = xinf
        dTdD   = xinf
        d2TdP2 = xinf
        d2TdD2 = xinf
        d2TdPD = xinf
      endif

      end                                             subroutine DERVPVT
c
c ======================================================================
c
      subroutine RESIDUAL (T,D,z,Pr,er,hr,sr,Cvr,Cpr,ar,gr)
c
c  Compute the residual quantities as a function of temperature,
c  density, and composition (where the residual is the total property
c  minus the ideal gas portion).
c
c  This routine is the same as THERM2, except it only calculates the
c  residual portions at any temperature and density.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       Pr--Residual pressure [kPa]  (P-D*Rxgas*T)
c       er--Residual internal energy [J/mol]
c       hr--Residual enthalpy [J/mol]
c       sr--Residual entropy [J/mol-K]
c      Cvr--Residual isochoric heat capacity [J/mol-K]
c      Cpr--Residual isobaric heat capacity [J/mol-K]
c       ar--Residual Helmholtz energy [J/mol]
c       gr--Residual Gibbs free energy [J/mol]
c
      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::RESIDUAL

      if (T.le.0. .or. D.lt.0.) RETURN
c
c  Get derivatives of the Helmholtz energy.
      call ISPURE (z,icomp)
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phi00=PHIX(0,0,tau,del,z)        !Real-gas terms
      phi01=PHIX(0,1,tau,del,z)
      phi10=PHIX(1,0,tau,del,z)
      phi11=PHIX(1,1,tau,del,z)
      phi02=PHIX(0,2,tau,del,z)
      phi20=PHIX(2,0,tau,del,z)
c
c  Calculate properties from the derivatives.
      RT=Rxgas*T
      Pr=RT*D*phi01
      er=RT*phi10
      hr=er+RT*phi01
      sr=Rxgas*(phi10-phi00)
      Cvr=-Rxgas*phi20
      Cpr=Cvr+Rxgas*(1d0+phi01-phi11)**2/(1d0+2d0*phi01+phi02)-Rxgas
      ar=RT*phi00
      gr=ar+RT*phi01

      end                                            subroutine RESIDUAL
c
c ======================================================================
c
      subroutine PHIDERV (iderv,T,D,z,dadn,dnadn,ierr,herr)
c
c  Calculate various derivatives required in the calculation of VLE
c  for mixtures.  Most of these are based on equations in the GERG-2004
c  document for natural gas, and are given below on the lines where
c  the code corresponds directly to the equation in that document.
c
c  Only the partials of alpha or alpha*n with respect to mole number are
c  returned here.  All others are stored in the PHIDR common block for
c  access by subroutine SATGV.
c
c  Inputs:
c    iderv--Set to 1 for first order derivatives only (dadn and dnadn)
c           Set to 2 for full calculations
c        T--Temperature [K]
c        D--Density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c           (n is mole number)
c           (the listed equation numbers are those in the GERG manuscript)
c     dadn--n*partial(alphar)/partial(ni)                   Eq. 7.16
c    dnadn--partial(n*alphar)/partial(ni)                   Eq. 7.15
c     ierr--Error flag:  0 - Successful
c             Error numbers are not set here, but are returned
c             from either the PHIDERVPR (when Peng-Robinson
c             is active) or RDXHMX routines.
c     herr--Error string (character*255)
c
c  The outputs below are stored in the PHIDR common block,
c  and can be obtained by a call to PASSCMN.
c
c :text:
c      daddn--del*n*d[d(ar)/d(del)]/d(ni)         Eq. 7.17
c       dvdn--n*[d(Vred)/d(ni)]/Vred              Eq. 7.18
c             (=-n*[d(Dred)/d(ni)]/Dred)
c       dtdn--n*[d(Tred)/d(ni)]/Tred              Eq. 7.19
c      dadxi--d(ar)/d(xi)                         Eq. 7.21g
c     sdadxi--sum[xi*d(ar)/d(xi)]                 Eq. 7.21g
c     dadxij--d^2(ar)/d(xi)/d(xj)                 Eq. 7.21i
c      daddx--del*d^2(ar)/d(xi)/d(del)            Eq. 7.21j
c      dadtx--tau*d^2(ar)/d(xi)/d(tau)            Eq. 7.21k
c     dphidT--d(ln(phi))/d(T)|{P,n,z}             Eq. 7.29
c     dphidp--d(ln(phi))/d(P) (constant T,n,z)    Eq. 7.30
c    dphidnj--n*d[ln(phi(i))]/d(nj)|{T,P}         Eq. 7.31
c  dlnfinidT--d[ln(fi/ni)]/d(T)                   Eq. 7.36
c  dlnfinidV--n*d[ln(fi/ni)]/d(V)                 Eq. 7.37
c     d2adbn--d[d(n*ar)/d(ni)]/d(T)               Eq. 7.44
c     d2adnn--n*d^2(n*ar)/d(ni)/d(nj)             Eq. 7.46 and 7.47 (similar to 7.38)
c     d2addn--del*d[n*d(ar)/d(ni)]/d(del)         Eq. 7.50
c     d2adtn--tau*d[n*d(ar)/d(ni)]/d(tau)         Eq. 7.51
c     d2adxn--d[n*d(ar)/d(ni)]/d(xj)              Eq. 7.52
c     ddrdxn--d[n*d(Dred)/d(ni)]/d(xj)            Eq. 7.55
c     dtrdxn--d[n*d(Tred)/d(ni)]/d(xj)            Eq. 7.56
c       dPdn--n*d(P)/d(ni)|{T,V,nj}               Eq. 7.63
c      dPdxi--d(P)/d(xi)|{T,V}
c   d2adxnTV--d[n*d(ar)/d(ni)]/d(xj)|{T,V}
c    dadxiTV--d(ar)/d(xi)|{T,V}
c   daddxiTV--del*d^2(ar)/d(xi)/d(del)|{T,V}
c    dphidxj--d(ln(phi))/d(xj)|{T,P,z}
c    xlnfi--Log of modified fugacity
c :rst:
c
c :end:


c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-22-10 EWL, original version
c  12-09-12 EWL, fix equations for dlnfinidT and dlnfinidV
c  05-05-14 EWL, move check for ar01<0 into routines for iderv=2
c  06-20-16 EWL, see comments under THERM

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension z(ncmax)
      character*255 herr
      dimension dadn(ncmax),dnadn(ncmax),
     &          aok(ncmax),aok01(ncmax),aok10(ncmax),
     &          sdadxi(ncmax),sdaddxi(ncmax),sdadtxi(ncmax),
     &          tr01(ncmax),dr01(ncmax),sdadxx(ncmax),
     &          phimxk(ncmax,ncmax),
     &          phimxk01(ncmax,ncmax),phimxk10(ncmax,ncmax),
     &          dr11(ncmax,ncmax),tr11(ncmax,ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PHIDERV

      ierr=0
      herr=' '
      call ISPURE (z,icomp)
      dadn(1:nc)=0d0
      dnadn(1:nc)=0d0
      daddn(1:nc)=0d0
      d2addn(1:nc)=0d0
      d2adtn(1:nc)=0d0
      daddx(1:nc)=0d0
      dadtx(1:nc)=0d0
      dtdn(1:nc)=0d0
      dvdn(1:nc)=0d0
      dadxi(1:nc)=0d0
      aok(1:nc)=0d0
      aok01(1:nc)=0d0
      aok10(1:nc)=0d0
      sdadxi(1:nc)=0d0
      sdaddxi(1:nc)=0d0
      sdadtxi(1:nc)=0d0
      tr01(1:nc)=0d0
      dr01(1:nc)=0d0
      dPdn(1:nc)=0d0
      sdadxx(1:nc)=0d0
      d2adnn(1:nc,1:nc)=0d0
      ddrdxn(1:nc,1:nc)=0d0
      dtrdxn(1:nc,1:nc)=0d0
      d2adxn(1:nc,1:nc)=0d0
      dadxij(1:nc,1:nc)=0d0                      !If i=j, derivative=0.
      xlnfi(1:nc)=0d0
      dphidt(1:nc)=0d0
      dphidp(1:nc)=0d0
      dphidxj(1:nc,1:nc)=0d0
      dlnphiidxj(1:nc,1:nc)=0d0
      if (T.le.0. .or. D.lt.1.d-12) RETURN

      if (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) then
        call PHIDERVPR (-1,T,D,z,dadn,dnadn,ierr,herr)
        RETURN
      endif

      if (.not.lRcnst .or. .not.lRcnstWag) call RMIX (z)
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0

c  Get Helmholtz energy of pure fluid.
      ar  =PHIX(0,0,tau,del,z)
      ar01=PHIX(0,1,tau,del,z)
      ar10=PHIX(1,0,tau,del,z)
      Rt=Rxgas*T

      if (icomp.ne.0) then
        phimxk(icomp,icomp)=0d0
        call RDXHMX (1,icomp,0,z,tr01(icomp),dr01(icomp),ierr,herr)
        if (ierr.ne.0) RETURN
        aok(icomp)=PHIK(icomp,0,0,tau,del)
      else
        do i=1,nc
          phimxk(i,i)=0d0
          if (z(i).gt.0. .or. i.eq.icomp) then
            call RDXHMX (1,i,0,z,tr01(i),dr01(i),ierr,herr)
            if (ierr.ne.0) RETURN
            aok(i)=PHIK(i,0,0,tau,del)
            if (i.ne.nc) then
              do j=i+1,nc
                if (z(j).gt.0. .or. j.eq.icomp) then
                  phimxk(i,j)=PHIMIX(i,j,0,0,tau,del,z)
                  phimxk(j,i)=phimxk(i,j)
                endif
              enddo
            endif
          endif
        enddo
      endif

c  Calculate first order derivatives only.
      if (iderv.eq.1) then

      if (icomp.ne.0) then
        dadxi(icomp)=aok(icomp)
        dadn(icomp)=ar01
        dnadn(icomp)=ar+ar01
      else
        do i=1,nc
          if (z(i).gt.0.) then
          do j=1,nc
            if (z(j).gt.0.) then
c  Get derivatives of reducing parameters.
              dtdn(i)=dtdn(i)-z(j)*tr01(j)
              dvdn(i)=dvdn(i)-z(j)*dr01(j)
              sdadxi(i)=sdadxi(i)+z(j)*aok(j)
              if (i.eq.j) then
                dtdn(i)=dtdn(i)+tr01(j)
                dvdn(i)=dvdn(i)+dr01(j)
                dadxi(i)=dadxi(i)+aok(j)
              else
c  Add excess Helmholtz energy of i-j interaction.
                if (z(i).gt.1d-50) dadxi(i)=dadxi(i)+phimxk(i,j)/z(i)
              endif
              if (j.ne.nc) then
              do k=j+1,nc
c  Subtract excess Helmholtz energy of i-j interaction twice (to include j-i).
                if (z(k).ne.0.) sdadxi(i)=sdadxi(i)+phimxk(k,j)*2d0
              enddo
              endif
            endif
          enddo
          endif
        enddo

c  *** Do this only while testing numerical derivatives    !!! ***
c       call RDXHMX (-100,0,0,z,T0,D0,ierr,herr)

        do i=1,nc
          if (z(i).gt.0 .or. i.eq.icomp) then
            dtdn(i)=dtdn(i)/T0
            dvdn(i)=dvdn(i)*D0
            dadn(i)=ar01*(1d0+dvdn(i))+ar10*dtdn(i)+dadxi(i)-sdadxi(i)         !Eq. 7.16 in GERG
            dnadn(i)=ar+dadn(i)                                                !Eq. 7.15
          endif
        enddo
      endif




c  Calculate first and second order derivatives.
      else
        if (ABS(ar01+1d0).lt.1d-10) ar01=1
        if (ar01.le.-1. .or. icomp.ne.0) then
          ierr=321
          call ERRNUM (321,0,'PHIDERV',' ',0d0,0d0,0d0,ierr,herr)
          RETURN
        endif
        ar02=PHIX(0,2,tau,del,z)
        ar20=PHIX(2,0,tau,del,z)
        ar11=PHIX(1,1,tau,del,z)
        call DERVPVT1 (T,D,z,P,dPdD,dPdT,dDdP,dDdT,dTdP,dTdD)
        dPdV=-dPdD*D**2
        if (P.eq.0.) P=1d-20
        if (dPdV.eq.0.) dPdV=1d-20
        dVdT=0d0
        if (D.ne.0.) dVdT=-dDdT/D**2

        do i=1,nc
          phimxk01(i,i)=0d0
          phimxk10(i,i)=0d0
          if (z(i).gt.0.) then
            aok01(i)=PHIK(i,0,1,tau,del)
            aok10(i)=PHIK(i,1,0,tau,del)
            if (i.ne.nc) then
            do j=i+1,nc
              if (z(j).gt.0.) then
                phimxk01(i,j)=PHIMIX(i,j,0,1,tau,del,z)
                phimxk10(i,j)=PHIMIX(i,j,1,0,tau,del,z)
                phimxk01(j,i)=phimxk01(i,j)
                phimxk10(j,i)=phimxk10(i,j)
              endif
            enddo
            endif
            do j=1,nc
            if (z(j).gt.0.) then
              ij=11
              if (i.eq.j) ij=2
              call RDXHMX (ij,i,j,z,tr11(i,j),dr11(i,j),ierr,herr)
              dr11(i,j)=2*D0**3*dr01(j)*dr01(i)-D0**2*dr11(i,j)
              if (ierr.ne.0) RETURN
            endif
            enddo
          endif
        enddo

        do j=1,nc
          if (z(j).gt.0.) then
          do i=1,nc
          if (z(i).gt.0.) then
            ddrdxn(i,j)= dr01(j)*D0**2                               !Eq. 7.55
            dtrdxn(i,j)=-tr01(j)                                     !Eq. 7.56
            do k=1,nc
            if (z(k).gt.0.) then
              ddrdxn(i,j)=ddrdxn(i,j)-z(k)*dr11(k,j)
              dtrdxn(i,j)=dtrdxn(i,j)-z(k)*tr11(k,j)
              if (i.eq.k) then
                ddrdxn(i,j)=ddrdxn(i,j)+dr11(k,j)
                dtrdxn(i,j)=dtrdxn(i,j)+tr11(k,j)
              endif
            endif
            enddo
          endif
          enddo
          endif
        enddo

        do i=1,nc
        if (z(i).gt.0.) then
          do j=1,nc
          if (z(j).gt.0.) then
            dtdn(i)=dtdn(i)-z(j)*tr01(j)
            dvdn(i)=dvdn(i)-z(j)*dr01(j)
            sdadxi(i)=sdadxi(i)+z(j)*aok(j)
            sdaddxi(i)=sdaddxi(i)+z(j)*aok01(j)
            sdadtxi(i)=sdadtxi(i)+z(j)*aok10(j)
            if (i.eq.j) then
              daddx(i)=daddx(i)+aok01(j)
              dadtx(i)=dadtx(i)+aok10(j)
              dtdn(i)=dtdn(i)+tr01(j)
              dvdn(i)=dvdn(i)+dr01(j)
              dadxi(i)=dadxi(i)+aok(j)
            endif
c  Add excess Helmholtz energy of i-j interaction.
            if (z(i).gt.1d-50) then
              dadxi(i)=dadxi(i)+phimxk(i,j)/z(i)
              daddx(i)=daddx(i)+phimxk01(i,j)/z(i)
              dadtx(i)=dadtx(i)+phimxk10(i,j)/z(i)
            endif

c  Subtract excess Helmholtz energy of i-j interaction twice (to include j-i).
            if (j.ne.nc) then
              do k=j+1,nc
              if (z(k).gt.0.) then
                sdadxi(i) =sdadxi(i) +phimxk(k,j)*2d0
                sdaddxi(i)=sdaddxi(i)+phimxk01(k,j)*2d0
                sdadtxi(i)=sdadtxi(i)+phimxk10(k,j)*2d0
              endif
              enddo
            endif
          endif
          enddo
          dtdn(i)=dtdn(i)/T0
          dvdn(i)=dvdn(i)*D0
        endif
        enddo

        do i=1,nc
        if (z(i).gt.0.) then
          dvdn1=1d0+dvdn(i)
          dadn(i)=ar01*dvdn1+ar10*dtdn(i)+dadxi(i)-sdadxi(i)              !Eq. 7.16 in GERG
          daddn(i)=ar02*dvdn1+ar11*dtdn(i)+daddx(i)-sdaddxi(i)            !Eq. 7.17
          dnadn(i)=ar+dadn(i)                                             !Eq. 7.15
          d2addn(i)=(ar01+ar02)*dvdn1+ar11*dtdn(i)+daddx(i)-sdaddxi(i)    !Eq. 7.50
          d2adtn(i)=ar11*dvdn1+(ar10+ar20)*dtdn(i)+dadtx(i)-sdadtxi(i)    !Eq. 7.51
          d2adbn(i)=-1d0/T*(ar10+d2adtn(i))                               !Eq. 7.44
          xlnfi(i)=XLOG(z(i)/(1d0+ar01))+dnadn(i)                         !Log of modified fugacity
          dlnfinidT(i)=(1d0-ar10-d2adtn(i))/T                             !Eq. 7.36 with use of 7.44
          dlnfinidV(i)=-D*(1d0+ar01+d2addn(i))                            !Eq. 7.37 with use of 7.45 (this is really n*Eq7.37)
        endif
        enddo

        do i=1,nc
        if (z(i).gt.0.) then
          daddnk=ar02*(1d0+dvdn(i))+ar11*dtdn(i)+daddx(i)-sdaddxi(i)      !Eq. 7.17
          dPdn(i)=D*Rt*(1d0+ar01*(2d0+dvdn(i))+daddnk)                    !Eq. 7.63 in GERG
          dDdn(i)=D-dPdn(i)/dPdD                                          !n*par(D)/par(ni) at constant P, T, nj<>ni
          dadxiTV(i)=ar01*D0*dr01(i)+ar10/T0*tr01(i)+dadxi(i)             !par(ar)/par(xi) at constant T and V
          daddxiTV(i)=ar02*D0*dr01(i)+ar11/T0*tr01(i)+daddx(i)            !par^2(ar)/par(xi)/par(del) at constant T and V
          dPdxi(i)=D*Rt*(dr01(i)*D0*ar01+daddxiTV(i))                     !d(P)/d(xi) at constant T and V
          vrt=-dPdn(i)/dpdv/rt                                            !Eq. 7.32 divided by RT
          dphidT(i)=d2adbn(i)+1d0/T-vrt*dPdT                              !Eq. 7.29 d(ln(phi))/dT at constant P and z (or n)
          if (P.gt.1d-15) dphidp(i)=vrt-1d0/P                             !Eq. 7.30 d(ln(phi))/dP at constant T and z (or n)
          dmurdt(i)=Rxgas*dnadn(i)+rt*d2adbn(i)-dPdn(i)*dvdt              !Eq. 7.66 without the ideal-gas part
        endif
        enddo
        do j=1,nc
          if (z(j).gt.1d-50) then
          do i=1,nc
            if (z(i).gt.1d-50) then
              dadxij(i,j)=phimxk(i,j)/z(j)/z(i)
              sdadxx(j)=sdadxx(j)+z(i)*dadxij(i,j)
            endif
          enddo
          endif
        enddo
        do i=1,nc
          if (z(i).gt.0.) then
          do j=1,nc
            if (z(j).gt.0.) then
            d2adxn(i,j)=daddx(j)*(1d0+dvdn(i))-                           !Eq. 7.52
     &                 ar01/D0*(ddrdxn(i,j)-D0**2*dr01(j)*dvdn(i))+
     &                 dadtx(j)*dtdn(i)+
     &                 ar10/T0*(dtrdxn(i,j)-tr01(j)*dtdn(i))+
     &                 dadxij(i,j)-dadxi(j)-sdadxx(j)
            d2adxnTV(j,i)=d2addn(i)*D0*dr01(j)+                           !d^2(n*ar)/dxj/dni at constant T and V
     &                 d2adtn(i)/T0*tr01(j)+d2adxn(i,j)+dadxiTV(j)
            endif
          enddo
        endif
        enddo
        do i=1,nc
          if (z(i).gt.0.) then
          do j=1,nc
            if (z(j).gt.0.) then
              sd2adxn=0d0
              do k=1,nc
                sd2adxn=sd2adxn+z(k)*d2adxn(i,k)
              enddo
c              Eq. 7.38 times n is the same as Eq. 7.46 (d2adnn)
              d2adnn(i,j)=dadn(j)+d2addn(i)*(1d0+dvdn(j))+                !Eq. 7.46
     &                    d2adtn(i)*dtdn(j)+d2adxn(i,j)-sd2adxn
              dphidnj(i,j)=d2adnn(i,j)+1d0+dPdn(j)*dPdn(i)/dpdv/Rt        !Eq. 7.31
              dlnphiidxj(i,j)=d2adxnTV(i,j)+dPdn(j)/Rt*dPdxi(i)/dpdV
              dphidxj(i,j)=dlnphiidxj(i,j)
              if (i.eq.j .and. z(i).gt.1d-50)
     &            dphidxj(i,j)=dphidxj(i,j)+1d0/z(i)
            endif
          enddo
          endif
        enddo
      endif

      end                                             subroutine PHIDERV
c
c ======================================================================
c
      subroutine GIBBS (T,D,z,ar,gr)
c
c  Compute residual Helmholtz and Gibbs energies as functions of
c  temperature, density, and composition from core functions, calculated as::
c
c      G(T,D) - G0(T,P*) = G(T,D) - G0(T,D) + RTln(RTD/P*)
c
c  where G0 is the ideal-gas state and P* is a reference pressure that is equal
c  to the current pressure of interest.  Since Gr is used only as a difference
c  in phase equilibria calculations where the temperature and pressure of the
c  phases are equal, the (RT/P*) part of the log term will cancel and is omitted.
c  Normal (not residual) A and G are computed by subroutine AG.
c
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       ar--Residual Helmholtz energy [J/mol]
c       gr--Residual Gibbs free energy [J/mol]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GIBBS

      if (T.le.0. .or. D.lt.0.) RETURN
      Ds=D
      if (D.lt.1d-40) Ds=1d-40
      call REDX (z,T0,D0)
      phi00=PHIX(0,0,T0/T,D/D0,z)      !Real-gas terms
      phi01=PHIX(0,1,T0/T,D/D0,z)
      ar=Rxgas*T*phi00
      gr=ar+Rxgas*T*(1d0+phi01)+Rxgas*T*(-1d0+XLOG(Ds))
      end                                               subroutine GIBBS
c
c ======================================================================
c
c *** Use subroutine THERM or THERM1 instead of these. ***
c     (They are only kept for compatibility with previous versions.)
c
      subroutine ENTRO (T,D,z,s)
      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ENTRO
      call THERM1 (T,D,z,P,e,h,s,g)
      end                                               subroutine ENTRO
c =====
      subroutine ENTHAL (T,D,z,h)
      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ENTHAL
      call THERM1 (T,D,z,P,e,h,s,g)
      end                                              subroutine ENTHAL
c =====
      subroutine ENERGY (T,D,z,e)
      include 'COMMONS.INC'
      dimension z(ncmax)
      call THERM1 (T,D,z,P,e,h,s,g)
      end                                              subroutine ENERGY
c =====
      subroutine CVCP (T,D,z,Cv,Cp)
      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CVCP
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      end                                                subroutine CVCP
c
c ======================================================================
c
      subroutine DHD1 (T,D,z,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,dhdp_d)
c
c  Compute partial derivatives of enthalpy w.r.t. T, P, or D at constant
c  T, P, or D as a function of temperature, density, and composition.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c   dhdt_d--DH/dT at constant density [J/mol-K]
c   dhdt_p--dH/dT at constant pressure [J/mol-K]
c   dhdd_t--dH/dD at constant temperature [(J/mol)/(mol/L)]
c   dhdd_p--dH/dD at constant pressure [(J/mol)/(mol/L)]
c   dhdp_t--dH/dP at constant temperature [J/(mol-kPa)]
c   dhdp_d--dH/dP at constant density [J/(mol-kPa)]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-02-00 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DHD1

      if (T.le.0. .or. D.lt.0.) RETURN
      Ds=D
      if (D.lt.1d-40) Ds=1d-40         !Entropy calculation will crash if D = 0.
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phig20=PHI0(2,0,T,D,z)           !Ideal-gas terms
      phig11=PHI0(1,1,T,D,z)
      phi01=PHIX(0,1,tau,del,z)        !Real-gas terms
      phi11=PHIX(1,1,tau,del,z)
      phi20=PHIX(2,0,tau,del,z)
      phi02=PHIX(0,2,tau,del,z)

      call THERM2 (T,Ds,z,P,e,h,s,Cv,Cp,w,zz,hjt,A,G,xkappa,beta,
     &             dPdD,d2PdD2,dPdT,dDdT,dDdP,d2PdT2,d2PdTD,sp3,sp4)
      dhdt_p=Cp
      dhdt_d=Rxgas*(-phig20-phi20+phi01-phi11+1d0)
      if (D.gt.1d-15) then
        dhdp_t=1d0/D+T*dDdT/D**2
        dhdd_t=Rxgas*T/D*(phig11+phi11+phi01+phi02)
        dhdp_d=dhdp_t+dhdt_p/dPdT
        dhdd_p=dhdd_t+dhdt_d/dDdT
        if (D.lt.1d-10) dhdp_t=xinf
      else
        call VIRB (T,z,B)
        call DBDT (T,z,dBT)
        dhdd_t=-Rxgas*T**2*dBT+Rxgas*T*B
        dhdp_t=xinf
        dhdp_d=xinf
        dhdd_p=xinf
      endif

      end                                                subroutine DHD1
c
c ======================================================================
c
      subroutine DSD1 (T,D,z,dsdt_d,dsdt_p,dsdd_t,dsdd_p,dsdp_t,dsdp_d)
c
c  Compute partial derivatives of entropy w.r.t. T, P, or D at constant
c  T, P, or D as a function of temperature, density, and composition.
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c   dsdt_d--dS/dT at constant density [J/mol-K^2]
c   dsdt_p--dS/dT at constant pressure [J/mol-K^2]
c   dsdd_t--dS/dD at constant temperature [(J/mol-K)/(mol/L)]
c   dsdd_p--dS/dD at constant pressure [(J/mol-K)/(mol/L)]
c   dsdp_t--dS/dP at constant temperature [J/(mol-K-kPa)]
c   dsdp_d--dS/dP at constant density [J/(mol-K-kPa)]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-03-16 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DSD1

      if (T.le.0. .or. D.lt.0.) RETURN
      Ds=D
      if (D.lt.1d-40) Ds=1d-40         !Entropy calculation will crash if D = 0.
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phig20=PHI0(2,0,T,D,z)           !Ideal-gas terms
      phig11=PHI0(1,1,T,D,z)
      phi01=PHIX(0,1,tau,del,z)        !Real-gas terms
      phi11=PHIX(1,1,tau,del,z)
      phi20=PHIX(2,0,tau,del,z)
      phi02=PHIX(0,2,tau,del,z)

      dPdT=Ds*Rxgas*(1d0+phi01-phi11)
      dPdD=t*Rxgas*(1d0+2d0*phi01+phi02)
      dDdT=-Ds/T*(1d0+phi01-phi11)/(1d0+2d0*phi01+phi02)

      dsdt_d=-Rxgas/t*(phig20+phi20)
      dsdt_p=Rxgas/t*((1d0+phi01-phi11)**2/(1d0+2d0*phi01+phi02)-
     &       phig20-phi20)
      if (D.gt.1d-15) then
        dsdd_t=-Rxgas/Ds*(1d0+phi01-phi11)
        dsdp_t=XDIV(dsdd_t,dPdD)
        dsdp_d=XDIV(dsdt_d,dPdT)
        dsdd_p=XDIV(dsdt_p,dDdT)
      else
        dsdd_t=xinf
        dsdp_t=xinf
        dsdp_d=xinf
        dsdd_p=xinf
      endif

      end                                                subroutine DSD1
c
c ======================================================================
c
      subroutine AG (T,D,z,a,g)
c
c  Compute Helmholtz and Gibbs energies as functions of temperature,
c  density, and composition.  These are not residual values (those are
c  calculated by GIBBS).
c  See warning in subroutines THERM or ALLPROPS.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        a--Helmholtz energy [J/mol]
c        g--Gibbs free energy [J/mol]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::AG

      a=xinf
      g=xinf
      if (T.le.0. .or. D.le.1d-40) RETURN        !Entropy calculation will crash if D = 0.
      call ISPURE (z,icomp)
      call REDX (z,T0,D0)
      phig00=PHI0(0,0,T,D,z)           !Ideal-gas term
      phi00=PHIX(0,0,T0/T,D/D0,z)      !Real-gas terms
      phi01=PHIX(0,1,T0/T,D/D0,z)
      a=Rxgas*T*(phig00+phi00)
      g=a+Rxgas*T*(1d0+phi01)
      end                                                  subroutine AG
c
c ======================================================================
c
      subroutine FPV (T,D,P,z,Fpvx)
c
c  Compute the supercompressibility factor, Fpv.
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     Fpvx--Fpv = SQRT[Z(60 F, 14.73 psia)/Z(T,P)]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-07-02 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::FPV

      tfpv=288.705555555556d0     !60 F
      pfpv=101.55977492837d0      !14.73 psia
      dfpv=0d0
      call TPRHO (tfpv,pfpv,z,2,0,dfpv,ierr,herr)
      if (ierr.gt.0) call TPRHO (tfpv,pfpv,z,1,0,dfpv,ierr,herr)
      if (ierr.gt.0) then
        Fpvx=0d0
      elseif (P.gt.0. .and. D.gt.0.) then
        Fpvx=SQRT(pfpv/tfpv/dfpv*D*T/P)
      else
        Fpvx=SQRT(pfpv/tfpv/dfpv/Rxgas)
      endif

      end                                                 subroutine FPV
c
c ======================================================================
c
      subroutine RIEM (T,D,z,riemc)
c
c  RIEM is the thermodynamic curvature in cubic nanometers/molecule.
c  It has the magnitude of the correlation volume, is negative for attractive
c  interactions, and positive for repulsive interactions, except when its
c  magnitude gets smaller than the molecular volume.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c    riemc--RIEM [cubic nanometers/molecule]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  08-01-11 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::RIEM

      riemc=xnotc
      if (D.lt.1d-8 .or. T.le.0.) RETURN
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      a01=(PHI0(0,1,T,D,z)+PHIX(0,1,tau,del,z))/del
      if (ABS(a01).gt.1d80) RETURN
      a02=(PHI0(0,2,T,D,z)+PHIX(0,2,tau,del,z))/del**2
      a03=(PHI0(0,3,T,D,z)+PHIX(0,3,tau,del,z))/del**3
      a11=(PHI0(1,1,T,D,z)+PHIX(1,1,tau,del,z))/tau/del
      a12=(PHI0(1,2,T,D,z)+PHIX(1,2,tau,del,z))/tau/del**2
      a21=(PHI0(2,1,T,D,z)+PHIX(2,1,tau,del,z))/tau**2/del
      a20=(PHI0(2,0,T,D,z)+PHIX(2,0,tau,del,z))/tau**2
      a30=(PHI0(3,0,T,D,z)+PHIX(3,0,tau,del,z))/tau**3
      r  =-2d0*       a20**2*a01+
     &     4d0*del*   a30*a01*a11 +4d0*del   *a20*a11**2-
     &     4d0*del*   a20*a01*a21 -2d0*del**2*a01*a21**2-
     &     4d0*del*   a20**2 *a02 +2d0*del**2*a30*a11*a02-
     &     5d0*del**2*a20*a21*a02 -    del**3*a21**2*a02+
     &     2d0*del**2*a30*a01*a12 +4d0*del**2*a20*a11*a12+
     &         del**3*a30*a02*a12 +    del**3*a20*a12**2-
     &         del**2*a20**2 *a03 -    del**3*a20*a21*a03
      rr=2d0*xAvog*del**2*D0*a20**2*(2d0*a01+del*a02)**2
      riemc=XDIV(r,rr)

      end                                                subroutine RIEM
c
c ======================================================================
c
      subroutine FGCTY (T,D,z,f)
c
c  Old routine to compute fugacity for each of the nc components of a mixture
c  by numerical differentiation (with central differences) of the
c  dimensionless residual Helmholtz energy.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c        f--Array (1..nc) of fugacities [kPa]
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  12-15-95  MM, original version
c  11-20-09 BFT, change deln to delmol in check for z(i).gt.1d0-delmol

      include 'COMMONS.INC'
      dimension z(ncmax),f(ncmax)
      dimension xplus(ncmax),xminus(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::FGCTY

      f(1:nc)=0d0
      if (T.le.0. .or. D.lt.1d-40) RETURN
      call ISPURE (z,icomp)

c  Pure component
      if (icomp.ne.0) then
        tau=Tzred(icomp)/T
        del=D*Vzred(icomp)
        phi00=PHIK(icomp,0,0,tau,del)
        phi01=PHIK(icomp,0,1,tau,del)
        arg=phi00+phi01
c  Check for potential under- or over-flow (can happen in 2-phase, but
c   the fugacity is meaningless there anyway).
        f(icomp)=1d100
        f(icomp)=Rxgas*T*D*EXP4(arg)

c  Mixture
      else
        delmol=1d-4
        do i=1,nc
          delpp=delmol                 !Compute positive and negative increments to number of moles.
          deln=-delmol                 !General case:  deln < z(i) < 1 - deln
          if (z(i).gt.0.) then
          if (z(i).lt.delmol) then
            deln=-z(i)/2d0             !Composition of component i is nearly zero.
            delpp=-deln
          elseif (z(i).gt.1d0-delmol) then
            delpp=(1d0-z(i))/2d0       !Composition of component i is nearly one (pure fluid).
            deln=-delpp
          endif
          delp1=1d0/(1d0+delpp)
          deln1=1d0/(1d0+deln)
          xplus(1:nc)=z(1:nc)*delp1              !Since total number of moles is now 1 + (delp1 or deln1), all of the
          xminus(1:nc)=z(1:nc)*deln1             ! compositions have changed.
          xplus(i)=(z(i)+delpp)*delp1
          xminus(i)=(z(i)+deln)*deln1
          Dplus=D*(1d0+delpp)                    !Derivative is at constant volume, so must adjust density.
          Dminus=D*(1d0+deln)
c  Compute residual Helmholtz at 'plus' and 'minus' density and composition.
c   Could call subroutine GIBBS here, but more efficient to directly call
c   the core routines (via PHIX).
          call REDX (xplus,T0,D0)
          tau=T0/T
          del=Dplus/D0
          Aplus=PHIX(0,0,tau,del,xplus)          !Real-gas terms
          call REDX (xminus,T0,D0)
          tau=T0/T
          del=Dminus/D0
          Aminus=PHIX(0,0,tau,del,xminus)        !Real-gas terms
          dnadn0=((1d0+delpp)*Aplus-(1d0+deln)*Aminus)/(delpp-deln)
c  Check for potential under- or over-flow (can happen in 2-phase, but
c   the fugacity is meaningless there anyway).
          f(i)=xerr
          f(i)=z(i)*Rxgas*T*D*EXP4(dnadn0)       !A is dimensionless.
          endif
        enddo
      endif

      end                                               subroutine FGCTY
c
c ======================================================================
c
      subroutine FGCTY2 (T,D,z,f,ierr,herr)
c
c  Compute fugacity for each of the nc components of a mixture by
c  analytical differentiation of the dimensionless residual Helmholtz energy.
c  These are based on derivations in the GERG-2004 document for natural gas.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c        f--Array (1..nc) of fugacities [kPa]

      include 'COMMONS.INC'
      dimension z(ncmax),f(ncmax),dadn(ncmax),dnadn(ncmax)
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::FGCTY2

      ierr=0
      herr=' '
      f(1:nc)=0d0
      if (T.le.0. .or. D.lt.1d-40) goto 151
      call ISPURE (z,icomp)
      if (icomp.ne.0) then
        call FGCTY (T,D,z,f)           !Call old FGCTY routine for pure fluids.
      else
        call PHIDERV (1,T,D,z,dadn,dnadn,ierr,herr)
        if (ierr.gt.100) then
          call FGCTY (T,D,z,f)         !Call old FGCTY routine if PHIDERV fails.
        else
          do i=1,nc
            f(i)=z(i)*Rxgas*T*D*EXP5(dnadn(i))
          enddo
        endif
      endif
      if (ABS(f(1)).gt.1d10) goto 151
      RETURN

 151  ierr=151

      end                                              subroutine FGCTY2
c
c ======================================================================
c
      subroutine CHEMPOT (T,D,z,u,ierr,herr)
c
c  Compute the chemical potentials for each of the nc components of a mixture.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c        u--Array (1..nc) of the chemical potentials [J/mol]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  03-18-10 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax),u(ncmax),dadn(ncmax),dnadn(ncmax)
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CHEMPOT

      ierr=0
      herr=' '
      u(1:nc)=xinf
      if (T.le.0. .or. D.lt.1d-40) RETURN
      call ISPURE (z,icomp)

c  Pure component
      if (icomp.ne.0) then
        call AG (T,D,z,a,g)
        u(icomp)=g

c  Mixture
      else
        call PHIDERV (1,T,D,z,dadn,dnadn,ierr,herr)
        if (ierr.gt.0) RETURN
        do i=1,nc
          if (z(i).gt.0.) then
            da0dn=PHI0K(i,0,0,T,D)+1d0+XLOG(z(i))  !Ideal-gas terms  (Eq. 7.14 in GERG)
            u(i)=(dnadn(i)+da0dn)*Rxgas*T
          endif
        enddo
      endif

      end                                             subroutine CHEMPOT
c
c ======================================================================
c
      subroutine FUGCOF (T,D,z,phi,ierr,herr)
c
c  Compute the fugacity coefficient for each of the nc components of a mixture.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Output:
c      phi--Array (1..nc) of the fugacity coefficients [-]

      include 'COMMONS.INC'
      dimension z(ncmax),phi(ncmax),f(ncmax)
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::FUGCOF

      ierr=0
      herr=' '
      phi(1:nc)=1d0
      if (T.le.0. .or. D.lt.1d-40) RETURN
      call ISPURE (z,icomp)
      call FGCTY2 (T,D,z,f,ierr,herr)
      call PRESS (T,D,z,P)
      if (P.gt.0.) then
        if (icomp.ne.0) then
          phi(icomp)=f(icomp)/P
        else
          do i=1,nc
            if (z(i).gt.0.) phi(i)=f(i)/P/z(i)
          enddo
        endif
      endif

      end                                              subroutine FUGCOF
cc
cc ======================================================================
cc
c      subroutine ACTVY (T,D,z,actv,gamm,ierr,herr)
cc
cc  Compute the activity and activity coefficient for each of the nc
cc    components of a mixture.
cc
cc  Inputs:
cc        T--Temperature [K]
cc        D--Molar density [mol/L]
cc        z--Composition (array of mole fractions)
cc
cc  Outputs:
cc     actv--Array (1..nc) of the activities
cc     gamm--Array (1..nc) of the activity coefficients
cc
cc  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
cc  03-21-10 EWL, original version
c
c      include 'COMMONS.INC'
c      dimension x(ncmax),y(ncmax),z(ncmax),zz(ncmax)
c      dimension actv(ncmax),gamm(ncmax),f(ncmax),fp(ncmax)
c      character*255 herr
cccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ACTVY
c
c      ierr=0
c      herr=' '
c      actv(1:nc)=0d0                  !Pure fluid values
c      gamm(1:nc)=0d0
c      if (T.le.0. .or. D.lt.1d-40) RETURN
c
c      call ISPURE (z,icomp)
c      if (icomp.eq.0) then
c        call FGCTY2 (T,D,z,f,ierr,herr)
c        call PRESS (T,D,z,P)
c        do i=1,nc
c          if (z(i).gt.0.) then
c            zz=0d0
c            zz(i)=1d0
c            call TPFLSH (T,P,zz,dp,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c            call FGCTY2 (T,dp,zz,fp,ierr,herr)
c            if (fp(i).gt.0.) actv(i)=f(i)/fp(i)
c            gamm(i)=actv(i)/z(i)
c          endif
c        enddo
c      endif
c
c      end                                               subroutine ACTVY
c
c ======================================================================
c
      subroutine B12 (T,z,B)
c
c  Compute B12 as a function of temperature and composition for a binary mixture.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c        B--B12 [L/mol]

      include 'COMMONS.INC'
      dimension z(ncmax),xb(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::B12

      B=0d0
      if (nc.ne.2 .or. z(1).le.0. .or. z(2).le.0.) RETURN            !Only calculate B12 for a binary.
      call VIRB (T,z,Bx)
      xb(1)=1d0              !Do not use icnc here because it affects parallel code while fitting
      xb(nc)=0d0
      call VIRB (T,xb,B1)
      xb(1)=0d0
      xb(nc)=1d0
      call VIRB (T,xb,B2)
      B=(Bx-z(1)**2*B1-z(nc)**2*B2)/2d0/z(1)/z(nc)
      end                                                 subroutine B12
c
c ======================================================================
c
      subroutine CSTAR (T,P,v,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
c
c  Calculate the critical flow factor, C*, for nozzle flow of a gas
c  (subroutine was originally named CCRIT).
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        v--Plenum velocity [m/s] (Should generally be set to 0 for
c             calculating stagnation conditions.)
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       Cs--Critical flow factor [dimensionless]
c       Ts--Nozzle throat temperature [K]
c       Ds--Nozzle throat molar density [mol/L]
c       Ps--Nozzle throat pressure [kPa]
c       ws--Nozzle throat speed of sound [m/s]
c     ierr--Error flag:  0 - Successful
c                      151 - CSTAR did not converge
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-05-02 EWL, original version
c  12-19-07 EWL, implement new method proposed by Aaron Johnson

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::CSTAR

      Cs=0d0
      Ts=0d0
      Ds=0d0
      Ps=0d0
      ws=0d0
      itmax=20
      if (P.le.0.) RETURN
      wmm=WMOL(z)
      call CRITP (z,Tc,Pc,Dc,ierr,herr)
      call TPFLSH (T,P,z,Ds,Dl,Dv,x,y,q,e,h,s,Cv,Cp,ws,ierr,herr)
      Ts=T

c...Old method used in version 8.0, which didn't always converge
c     Dmax=Dc
c     if (T.gt.Tc) Dmax=4d0*Dc
c     if (T.lt.0.95*Tc .and. Ds.ltdc) Dmax=Dc/2d0
c     dh=0d0
c10   continue
c       i=i+1
c       dhold=dh
c       dh=(ws**2-v**2)/2d0*wmm/1000d0           !Calculate change in enthalpy.
c       hs=h-dh
c       ss=s
c       call HSFL1 (hs,ss,z,0d0,Dmax,Ts,Ds,ierr,herr)      !Flow is assumed isentropic, Ds=0.
c       call THERM (Ts,Ds,z,Ps,e,hs,ss,Cv,Cp,ws,hjt)
c     if (ABS(dh-dhold).gt.1d-4 .and. i.lt.20 .and. ierr.eq.0) goto 10
c
c...New method (which is about 10 times faster than the old method).
      dT=-1d0
      eta=0d0
      do it=1,itmax
        T1=Ts
        eta1=eta
        ss=s                 !Flow is assumed isentropic, Ds=0.
        Ts=Ts+dT
        Dmax=Dc
        Dmin=0d0
        if (Ts.gt.Tc) Dmax=4d0*Dc
        if (Ts.lt.0.95*Tc .and. Ds.lt.dc) Dmax=Dc/2d0
        if (Ts.lt.Tc .and. Ds.gt.dc) then
          Dmax=4d0*Dc
          Dmin=Dc
          if (Ts.lt.0.95*Tc) Dmin=Dc*2d0
        endif
        Dmax=5d0
        if (Ts.gt.1d10 .or. Ts.lt.1d-2) EXIT
        call TSFL1 (Ts,ss,z,Dmin,Dmax,Ds,ierr,herr)
        q=0d0
        if (ierr.gt.0) then
          kr=1
          call TSFLSH (Ts,ss,z,kr,Ps,Ds,Dl,Dv,x,y,q,e,hs,Cv,Cp,ws,
     &                 ierr,herr)
        else
          call THERM (Ts,Ds,z,Ps,e,hs,ss,Cv,Cp,ws,hjt)
        endif
        if (ierr.gt.0) RETURN
        if ((q.gt.0. .and. q.lt.1.) .or. ws.le.0. .or. Ps.le.0.)goto 500
        dh=(ws**2-v**2)/2d0*wmm/1000d0           !Calculate change from input state in enthalpy.
        eta=(hs+dh)/h-1d0
        if (ABS(eta1-eta).lt.1d-20) EXIT
        if (it.gt.1) dT=-eta/XDIV(eta1-eta,T1-Ts)          !Newton's first order method.
c  See NASA Tech Note D-2565 by R.C. Johnson for C* equation.
c  See also Stewart, D.G., Watson, J.T.R., Vaidya, A.M., Flow Measurement and Instrumentation, 10:27-34, 1999.
        if (ABS(eta).lt.1d-8) then
          Cs=Ds*ws*SQRT(T*Rxgas*wmm/1000d0)/P
          RETURN
        endif
      enddo

 500  call ERRNUM (151,0,'CSTAR',' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine CSTAR
c
c ======================================================================
c
      subroutine EXCESS (T,P,z,kph,D,vE,eE,hE,sE,aE,gE,ierr,herr)
c
c  Compute excess properties as a function of temperature, pressure,
c  and composition.
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        z--Composition (array of mole fractions)
c      kph--Phase flag:  1 - Liquid
c                        2 - Vapor
c                        0 - Stable phase
c
c  Outputs:
c        D--Molar density [mol/L]  (Send a negative density to the routine to use it as an initial guess.)
c       vE--Excess volume [L/mol]
c       eE--Excess energy [J/mol]
c       hE--Excess enthalpy [J/mol]
c       sE--Excess entropy [J/mol-K]
c       aE--Excess Helmholtz energy [J/mol]
c       gE--Excess Gibbs energy [J/mol]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  04-25-02 EWL, original version
c  03-21-10 EWL, add LOG(z(i)) to sE, gE, and aE
c  08-24-11 EWL, replace use of 'ic2=ic' with zz(i)=1 so that the fitter can parallelize the code

      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax),zz(ncmax)
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::EXCESS

      ierr=0
      herr=' '
      Din=D
      D=0d0
      vE=0d0
      eE=0d0
      hE=0d0
      sE=0d0
      aE=0d0
      gE=0d0
      call ISPURE (z,icomp)
      if (icomp.ne.0 .or. T.le.0. or. P.le.1d-6) RETURN

         !Use a constant value of R in the excess calculations.
      call Flags('R',-999,jr,ierr,herr)
      call Flags('R',1   ,jj,ierr,herr)

      kguess=0
      if (Din.lt.0) kguess=1
      D=ABS(D)
      if (kph.ne.0) then
        call TPRHO (T,P,z,kph,kguess,D,ierr,herr)
        if (ierr.ne.0) then
          D=D*2d0
          call TPRHO (T,P,z,kph,1,D,ierr,herr)
        endif
        if (ierr.ne.0)
     &  call TPFLSH (T,P,z,D,Dl,Dv,x,y,q,eE,hE,sE,Cv,Cp,w,ierr,herr)
        call THERM (T,D,z,pp,eE,hE,sE,Cv,Cp,w,hjt)
      else
        call TPFLSH (T,P,z,D,Dl,Dv,x,y,q,eE,hE,sE,Cv,Cp,w,ierr,herr)
      endif
      if (ierr.ne.0) goto 999
      call AG (T,D,z,aE,gE)
      if (D.gt.0.) vE=1d0/D

      do i=1,nc
        zz=0d0
        zz(i)=1d0
c...Do not use kph or call TPRHO since the pure fluids could be in either phase.
        call TPFLSH (T,P,zz,dd,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
        call AG (T,dd,zz,a,g)
        if (dd.gt.0.) vE=vE-z(i)/dd
        eE=eE-z(i)*e
        hE=hE-z(i)*h
        sE=sE-z(i)*(s-Rxgas*  XLOG(z(i)))
        aE=aE-z(i)*(a+Rxgas*T*XLOG(z(i)))
        gE=gE-z(i)*(g+Rxgas*T*XLOG(z(i)))
      enddo

 999  call Flags('R',jr,jj,ierr,herr)

      end                                              subroutine EXCESS
c
c ======================================================================
c
      subroutine ENTHHC (icmb,T1,T2,h)
c
c  Compute the ideal-gas enthalpy difference between temperatures T1 and
c  T2 for several combustion gases.  This is used in conjunction with
c  the heat of combustion subroutine so that the extra fluids do not
c  have to be loaded.  If the EOS ever changes, the coefficients here
c  should be updated.
c
c  Inputs:
c       T1--Temperature [K]
c       T2--Temperature [K]
c           If T2 is less than zero, then return the absolute enthalpy calculated at T1.
c     icmb--Fluid identifier:  0 - Saturated liquid water
c                              1 - Water
c                              2 - Oxygen
c                              3 - CO2
c                              4 - Nitrogen
c                              5 - SO2
c
c  Output:
c        h--Ideal-gas enthalpy difference [J/mol]
c           (for i=0, then saturated liquid enthalpy difference of water)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-09-08 EWL, original version
c  04-30-12 EWL, check for T>Tc(water)

      include 'COMMONS.INC'
      dimension xk(10),cpc(10)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ENTHHC

      h=0d0
      ntc=0
      nte=0
      RR=xRgas
      if (icmb.eq.0) then
        ntc=6
        cpc(1)= 0.8943272d+5           !Enthalpy of saturated liquid water.
        cpc(2)=-0.6138239d+5           !Fit by EWL on 1/9/2007
        cpc(3)=-0.4415242d+5           !Matches Wagner and Pruss equation to within
        cpc(4)=-0.1092092d+5           ! 0.02% over full saturation range.
        cpc(5)= 0.6703065d+5           !Extrapolation to 200 K is smooth.
        cpc(6)=-0.1763063d+6
        xk(1) = 0.032d0
        xk(2) = 0.078d0
        xk(3) = 0.825d0
        xk(4) = 4d0
        xk(5) = 9d0
        xk(6) =12d0
        tcwater=647.096d0
        if (T1.lt.tcwater .and. T2.lt.tcwater) then
          do i=1,ntc
                          h=h+cpc(i)*(1d0-T1/tcwater)**xk(i)
            if (T2.ge.0.) h=h-cpc(i)*(1d0-T2/tcwater)**xk(i)
          enddo
        endif
        RETURN
      elseif (icmb.eq.1) then
        ntc=1
        nte=5
        cpc(1)=0.400632d+1             !Water
        cpc(2)=0.124360d-1
        cpc(3)=0.973150d+0
        cpc(4)=0.127950d+1
        cpc(5)=0.969560d+0
        cpc(6)=0.248730d+0
        xk(1) =    0d0
        xk(2) =  833d0
        xk(3) = 2289d0
        xk(4) = 5009d0
        xk(5) = 5982d0
        xk(6) =17800d0
        RR=8.314371357587d0
c                         Value from above        Value from Refprop
c                         at 300 K.               at 300 K.
        if (T2.lt.0.) h=(-88471.35339670102d0+45964.71449803960d0)/RR
      elseif (icmb.eq.2) then
        ntc=1
        nte=5
        cpc(1)=3.51808732d0            !Oxygen
        cpc(2)=0.102323928d+01
        cpc(3)=0.784357918d+00
        cpc(4)=0.337183363d-02
        cpc(5)=-.170864084d-01
        cpc(6)=0.463751562d-01
        xk(1) =0d0
        xk(2) =0.224632440d+04
        xk(3) =0.112599763d+05
        xk(4) =0.120126209d+04
        xk(5) =0.690089445d+02
        xk(6) =0.532805445d+04
        RR=8.31434d0
        if (T2.lt.0.) h=(-56058.60590328777d0+8734.35384436554d0)/RR
      elseif (icmb.eq.3) then
        ntc=1
        nte=5
        cpc(1)=0.35d+01                !CO2
        cpc(2)=1.99427042d0
        cpc(3)=0.621052475d0
        cpc(4)=0.411952928d0
        cpc(5)=1.04028922d0
        cpc(6)=0.0832767753d0
        xk(1) =   0d0
        xk(2) = 958.49956d0
        xk(3) =1858.80115d0
        xk(4) =2061.10114d0
        xk(5) =3443.89908d0
        xk(6) =8238.20035d0
        RR=8.31451d0
        if (T2.lt.0.) h=(-43458.10456571817d0+22372.0720622156d0)/RR
      elseif (icmb.eq.4) then
        ntc=4
        nte=1
        cpc(1)= 3.5d0                  !Nitrogen
        cpc(2)= 3.066469d-6
        cpc(3)= 4.70124d-9
        cpc(4)=-3.987984d-13
        cpc(5)= 0.1012941d1
        xk(1) = 0d0
        xk(2) = 1d0
        xk(3) = 2d0
        xk(4) = 3d0
        xk(5) = 3364.011d0
        RR=8.31451d0
        if (T2.lt.0.) h=(-22898.14229042497d0+8723.88255738888d0)/RR
      elseif (icmb.eq.5) then
        ntc=2
        nte=2
        cpc(1)= 4d0                    !SO2
        cpc(2)= 0.72453d-4
        cpc(3)= 1.0620d0
        cpc(4)= 1.9401d0
        xk(1) =    0d0
        xk(2) =    1d0
        xk(3) =  775d0
        xk(4) = 1851d0
        RR=8.314472d0
        if (T2.lt.0.) h=(-28976.84624561172d0+26659.0278471527d0)/RR
      endif

      do i=1,ntc
        xki=xk(i)
        xk1=xki+1d0
                      h=h+cpc(i)*T1**xk1/xk1
        if (T2.ge.0.) h=h-cpc(i)*T2**xk1/xk1
      enddo
      do i=1,nte
        j=i+ntc
        expui1=0d0
        expui2=0d0
        expui1=EXP5(xk(j)/T1)
        expui2=EXP5(xk(j)/T2)
        if (T2.ge.0.)
     &  h=h-cpc(j)*(-xk(j)/2d0)*(1d0+expui2)/(1d0-expui2)
        h=h+cpc(j)*(-xk(j)/2d0)*(1d0+expui1)/(1d0-expui1)
      enddo
      h=h*RR
      end                                              subroutine ENTHHC
c
c ======================================================================
c
      subroutine HEAT (T,D,z,hg,hn,ierr,herr)
c
c  Compute the ideal-gas gross and net heating values.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c       hg--Gross (or superior) heating value [J/mol]
c       hn--Net (or inferior) heating value [J/mol]
c     ierr--Error flag:  0 - Successful
c                      662 - Not all heating values available
c                      665 - Error in chemical formula
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-09-08 EWL, original version
c  04-24-14 EWL, move the species count to a new subroutine

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax),v(6)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::HEAT

      if (D.gt.0) ierr=0     !D is not yet used, this line keeps the compiler happy.
      ierr=0
      call ISPURE (z,icomp)
      hg=0d0
      hn=0d0
      if (icomp.eq.0) then
        k1=1
        k2=nc
      else
        k1=icomp
        k2=icomp
      endif
      do k=k1,k2
        if (z(k).gt.0 .or. k.eq.icomp) then
          call SPECIES (k,' ',aCrb,aHyd,aOxy,aNit,
     &                        aSul,aHe,aArg,aChl,aFl,ierr,herr)
          if (ierr.gt.0) RETURN
          if (ABS(xCmbst(k)+1d0).lt.1d-12) goto 662
          hgk=xCmbst(k)
          v(1)=-aHyd/2d0                         !Water produced
          v(2)= aCrb+aHyd/4d0-aOxy/2d0+aSul      !Oxygen needed
          v(3)=-aCrb                             !CO2 produced
          v(4)=-aNit/2d0                         !Nitrogen
          v(5)=-aSul                             !SO2

          T25=298.15d0
          D0=0d0
          call THERM (T25,D0,z,P,e,h25,s,Cv,Cp,w,hjt)
          call THERM (T  ,D0,z,P,e,h  ,s,Cv,Cp,w,hjt)
          hgk=hgk-(h25-h)

          call ENTHHC (0,T25,T,h)                !Liquid water
          hgk=hgk-v(1)*h
          do i=2,5
            call ENTHHC (i,T25,T,h)
            hgk=hgk-v(i)*h
          enddo

          call ENTHHC (0,T,-1d0,h1)              !Liquid water
          call ENTHHC (1,T,-1d0,h2)              !Ideal-gas water
          hnk=hgk+v(1)*(h2-h1)

          if (k.eq.icomp) then
            hg=hgk
            hn=hnk
          else
            hg=hg+z(k)*hgk
            hn=hn+z(k)*hnk
          endif
        endif
      enddo

      ierr=0
      herr=' '
      RETURN

 662  call ERRNUM (662,0,'HEAT',' ',0d0,0d0,0d0,ierr,herr)
      hg=0d0
      hn=0d0
      end                                                subroutine HEAT
c
c ======================================================================
c
      subroutine HEATFRM (T,D,z,hFrm,ierr,herr)
c
c  Compute the heat of formation.
c
c  The heat of formation is the heat required to form a compound from its constituent
c  elements, with the standard state defined as 298.15 K for the ideal gas.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L] (not used)
c        z--Composition (array of mole fractions)
c
c  Outputs:
c     hFrm--Heat of formation [J/mol]
c     ierr--Error flag:  0 - Successful
c                      662 - Not all heating values available
c                      664 - Unknown species in chemical formula
c                      665 - Error in chemical formula
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  04-24-14 EWL, original version
c  03-02-15 EWL, add sulfur

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::HEATFRM

      call ISPURE (z,icomp)
      if (T.gt.0) ierr=0     !T is not yet used, this line keeps the compiler happy.
      if (D.gt.0) ierr=0     !ditto
      ierr=0
      herr=' '
      hFrm=0d0
      if (icomp.eq.0) then
        k1=1
        k2=nc
      else
        k1=icomp
        k2=icomp
      endif
      do k=k1,k2
        if (z(k).gt.0 .or. k.eq.icomp) then
          call SPECIES (k,' ',aCrb,aHyd,aOxy,aNit,
     &                      aSul,aHe,aArg,aChl,aFl,ierr,herr)
          if (aOxy.gt.0.or.aNit.gt.0.or.aChl.gt.0.or.aFl.gt.0) goto 662
          if (ierr.gt.0) goto 999
          if (ABS(xCmbst(k)+1d0).lt.1d-12) goto 662        !If -1, exit with an error.
          hgk=xCmbst(k)-(aCrb*393.511d3+aHyd/2d0*285.83d3+aSul*296.81d3)
          if (icomp.ne.0) then
            hFrm=hgk
          else
            hFrm=hFrm+z(k)*hgk
          endif
        endif
      enddo
      RETURN

 662  call ERRNUM (662,0,'HEATFRM',' ',0d0,0d0,0d0,ierr,herr)
 999  hFrm=xnotc
      end                                             subroutine HEATFRM
c
c ======================================================================
c
      subroutine SPECIES (k,Frml,aCrb,aHyd,aOxy,aNit,
     &                    aSul,aHe,aArg,aChl,aFl,ierr,herr)
c
c  Retrieve the number and type of atoms in the chemical formula.
c
c  Inputs:
c        k--Component number, only used if Frml is empty
c     Frml--Chemical Formula
c
c  Outputs:
c     aCrb--Number of carbon atoms
c     aHyd--Number of hydrogen atoms
c     aOxy--Number of oxygen atoms
c     aNit--Number of nitrogen atoms
c     aSul--Number of sulfur atoms
c      aHe--Number of helium atoms
c     aArg--Number of argon atoms
c     aChl--Number of chlorine atoms
c      aFl--Number of fluorine atoms
c             Although these are all integer numbers, they are sent back
c             as doubles so they can be used in calculations.
c     ierr--Error flag:  0 - Successful
c                      664 - Unknown species in chemical formula
c                      665 - Error in chemical formula
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  04-24-14 EWL, original version

      include 'COMMONS.INC'
      character*255 herr,hcf
      character Frml*(*)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SPECIES


      ierr=0
      herr=' '
      if (k.eq.0) RETURN
      aCrb=0d0               !Number of carbon atoms.
      aHyd=0d0
      aOxy=0d0
      aNit=0d0
      aSul=0d0
      aHe =0d0
      aArg=0d0
      aChl=0d0
      aFl =0d0
      hgk =0d0
      hnk =0d0

c  Extract the chemical formula from hchemf2, use the part in {} if available.
      hcf=Frml
      if (hcf.eq.' ') hcf=hchemf2(k)

c  Extract the number of carbon, hydrogen, oxygen, ..., atoms from
c   the chemical formula.
 10   if (hcf(2:2).lt.'a' .or. hcf(2:2).gt.'z')
     &    hcf=hcf(1:1)//' '//hcf(2:255)          !Add space in second slot.
      do i=3,5
        if (hcf(i:i).lt.'0' .or. hcf(i:i).gt.'9')
     &      hcf=hcf(1:i-1)//' '//hcf(i:255)      !Add space in ith slot.
      enddo

      read (hcf(3:5),'(i3)') j
      if (j.eq.0) j=1
      if (hcf(1:2).eq.'C ') then       !Carbon
        if (aCrb.gt.0) goto 665
        aCrb=real(j)
      elseif (hcf(1:2).eq.'H ') then   !Hydrogen
        if (aHyd.gt.0) goto 665
        aHyd=real(j)
      elseif (hcf(1:2).eq.'O ') then   !Oxygen
        if (aOxy.gt.0) goto 665
        aOxy=real(j)
      elseif (hcf(1:2).eq.'N ') then   !Nitrogen
        if (aNit.gt.0) goto 665
        aNit=real(j)
      elseif (hcf(1:2).eq.'S ') then   !Sulfur
        if (aSul.gt.0) goto 665
        aSul=real(j)
      elseif (hcf(1:2).eq.'He') then   !Helium
        if (aHe.gt.0) goto 665
        aHe=real(j)
      elseif (hcf(1:2).eq.'Ar') then   !Argon
        if (aArg.gt.0) goto 665
        aArg=real(j)
      elseif (hcf(1:2).eq.'Cl') then   !Chlorine
        if (aChl.gt.0) goto 665
        aChl=real(j)
      elseif (hcf(1:2).eq.'F') then    !Fluorine
        if (aFl.gt.0) goto 665
        aFl=real(j)

c  Other possibilities but not included in this routine
      elseif (hcf(1:2).eq.'Kr') then
      elseif (hcf(1:2).eq.'Si') then
      elseif (hcf(1:2).eq.'Ne') then
      elseif (hcf(1:2).eq.'I ') then
      elseif (hcf(1:2).eq.'D ') then
      elseif (hcf(1:2).eq.'Xe') then
      else
        call ERRNUM (664,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif

      hcf=hcf(6:255)
      if (hcf.ne.' ') goto 10
      RETURN

 665  call ERRNUM (665,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      end                                             subroutine SPECIES
c
c ======================================================================
c
      subroutine MASSFLUX (Tm,P,z,beta,rf,fluxm,Cs,T0,P0,xMach,u,Ts,Ps,
     &                     ierr,herr)
c
c  Calculate the theoretical mass flux for a CFV (critical flow venturi) of a gas.
c  This is required for high beta; CSTAR can be used for low beta.
c
c  Inputs:
c       Tm--Measured temperature [K]
c        P--Upstream (static) pressure [kPa]
c        z--Composition (array of mole fractions)
c     beta--Ratio of throat diameter to pipe diameter [-]
c       rf--Recovery factor [(Tm-T)/(T0-T)]
c             (T is static temperature, T0 is the stagnation temperature)
c
c  Outputs:
c    fluxm--Theoretical mass flux [kg/(m^2-s)]
c       Cs--Critical flow factor [-]
c       T0--Stagnation temperature [K]
c       P0--Stagnation pressure [kPa]
c    xMach--Mach number (u/speed of sound) [-]
c        u--Average axial velocity in approach pipe upstream of the CFV [m/s]
c       Ts--Temperature at throat [K]
c       Ps--Pressure at throat [kPa]
c     ierr--Error flag:  0 - Successful
c                      151 - Iteration failed to converge
c     herr--Error string (character*255)
c
c  written by A.N. Johnson, NIST, Gaithersburg, CO
c  07-21-14 ANJ, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::MASSFLUX


c  Initial values.
      if (ABS(rf-1d0).lt.1d-10) goto 600
      xMach=0.05d0           !Only used as an initial guess for P0 and T0.
      D=0
      call TPRHO (Tm,P,z,2,0,D,ierr,herr)
      call THERM3 (Tm,D,z,xkappa,bb,xi,xkt,betas,bs,xkkt,thrott,pi,spht)
      xj=(xi-1d0)*xMach**2/2d0
      P0=P*(1d0+xj)**(xi/(xi-1d0))
      T0=Tm*(1d0+xj)/(1d0+rf*xj)
      Ts=2d0*T0/(xi+1d0)
      itmax=20
      wmm=WMOL(z)
      eta=1d10
      eta0=1d10
      eta1=1d10
      iter0=0

 100  iter0=iter0+1
      D0=0d0
      call TPRHO (T0,P0,z,2,0,D0,ierr,herr)
      call THERM (T0,D0,z,pp,e,h0,s0,Cv,Cp,w,hjt)          !Stagnation enthalpy and entropy.
      ss=s0

c  Iterate to find Ts and Ps (T and P at CFV throat).
      tt0=0d0
      iter=0
 110  iter=iter+1
      if (Ts.gt.1d10 .or. Ts.lt.1d-2) goto 500
      call TSFLSH (Ts,ss,z,2,Ps,Ds,Dl,Dv,x,y,q,e,hs,Cv,Cp,ws,ierr,herr)
      eta1=(hs-h0)/wmm+ws**2/1000d0/2d0
      ts1=Ts                 !Iterate Ts until eta1=0.
      if (tt0.ne.0. .and. ABS(eta0-eta1).gt.1d-10) then
        slp=(tt0-Ts)/(eta0-eta1)
        Ts=Ts-eta1*slp
      else
        Ts=Ts+0.1d0
      endif
      tt0=ts1
      eta0=eta1
      if ((ABS(eta1).gt.1d-10 .and. iter.lt.20).or.iter.eq.1) goto 110

c  Iterate to find T0 and P0 (stagnation T and P).
      tt0=0d0
      iter=0
 120  iter=iter+1
      T=(Tm-rf*T0)/(1d0-rf)            !Static temperature.
      call TPRHO (T,P,z,2,0,D,ierr,herr)
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)     !Static entropy and enthalpy.
      if (T0.gt.1d10 .or. T0.lt.1d-2) goto 500
      call TSFLSH (T0,s,z,2,P0,D0,Dl,Dv,x,y,q,e,h0,Cv,Cp,w0,ierr,herr)
      u=0d0
      if (D.gt.0d0) u=Ds*ws*beta**2/D
      eta1=((h-h0)/wmm+u**2/2d0/1000d0)/(h/wmm+u**2/1000d0/2d0)
      tt1=T0                 !Iterate T0 until eta1=0.
      if (tt0.ne.0. .and. ABS(eta0-eta1).gt.1d-10) then
        slp=(tt0-T0)/(eta0-eta1)
        T0=T0-eta1*slp
      else
        T0=T0+0.1d0
      endif
      if (T0.gt.1d10 .or. T0.lt.1d-2) goto 500
      if (T0.lt.2. .or. T0.gt.10000.) T0=500d0
      tt0=tt1
      eta0=eta1
      if ((ABS(eta1).gt.1d-10 .and. iter.lt.20).or.iter.eq.1) goto 120

c  Recalculate hs and ws with new entropy, with same code as first iteration
c   loop above, to determine if overall iteration is finished.
      if (Ts.gt.1d10 .or. Ts.lt.1d-2) goto 500
      call TSFLSH (Ts,s,z,2,Ps,Ds,Dl,Dv,x,y,q,e,hs,Cv,Cp,ws,ierr,herr)
      eta=(hs-h0)/wmm+ws**2/1000d0/2d0
      if ((ABS(eta).gt.1d-10 .and. iter0.lt.20).or.iter0.eq.1) goto 100

 500  if (ABS(eta)+ABS(eta1).lt.1d-5) then
        ierr=0
        herr=' '
        Cs=Ds*ws*SQRT(T0*Rxgas*wmm/1000d0)/P0
        fluxm=P0*Cs*SQRT(wmm*1000d0)/SQRT(Rxgas*T0)
        if (w.le.0.) w=1d10
        xMach=u/w
        RETURN
      endif

 600  call ERRNUM (151,0,'MASSFLUX',' ',0d0,0d0,0d0,ierr,herr)
      Cs=0d0
      fluxm=0d0
      xMach=0d0
      u=0d0

      end                                            subroutine MASSFLUX
c
c ======================================================================
c
      subroutine VIRBCD (T,z,B,C,D,E)
c
c  Compute virial coefficients as a function of temperature and composition.
c  The routine currently works only for pure fluids and for the Helmholtz equation.
c  All values are computed exactly based on the terms in the EOS, not
c  as was done in VIRB by calculating properties at a density of 1d-8.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        B--Second virial coefficient [L/mol]       = a01
c        C-- Third virial coefficient [(L/mol)^2]   = a02
c        D--Fourth virial coefficient [(L/mol)^3]   = a03/2d0
c        E-- Fifth virial coefficient [(L/mol)^4]   = a04/6d0
c             assume nomenclature of a02=[partial^2(alphar)/partial(del)^2] above
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-01-10 EWL, original version
c  10-08-11 EWL, include Gaussian terms and change input parameters
c  08-20-14 EWL, add e as an input although not yet calculated
c  06-11-15 EWL, add temporary variables Bx0, Cx0, Dx0, and Ex0 to avoid problems with fitting and multiple cores
c  06-20-16 EWL, see comments under THERM

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::VIRBCD

      B=0d0
      C=0d0
      D=0d0
      E=0d0
      Bx0=0d0
      Cx0=0d0
      Dx0=0d0
      Ex0=0d0

      call ISPURE (z,n)
      do i=1,nc
        if (iEOSk(i).ne.iEOSFEQ) n=0
      enddo
      if (n.eq.0) then       !Mixtures are not yet supported in this routine.
        call VIRB (T,z,B)
        call VIRC (T,z,C)
        call VIRD (T,z,D)
        RETURN
      endif

      tau=Tzred(n)/T
      elntau=XLOG(tau)
      do k=1,nFEQterms(n)
        di=diFEQ(k,n)
        ti=tiFEQ(k,n)
        dl=dliFEQ(k,n)
        if (k.le.nFEQpe(n)) then
          bb=aFEQ(k,n)*EXP5(ti*elntau)
          cc=2d0*bb
          dd=6d0*bb
          if (di.ne.2) cc=-cc
          if (di.ne.3) dd=-dd
          if (di.eq.1) Bx0=Bx0+bb                !Second virial coefficient (B).
          if (di.eq.2 .or. di+dl.eq.2) then
            Cx0=Cx0+cc                           !Third virial coefficient (C).
            if (di.ne.2) Dx0=Dx0-dd/2d0          !di=1 and dl=1 contributes to D.
          endif
          if (di.eq.3 .or. di+dl.eq.3) Dx0=Dx0+dd          !Fourth virial coefficient (D).

        elseif (k.le.nFEQpe(n)+nFEQgaus(n)) then
          al=alphaFEQ(k,n)
          be=betaFEQ(k,n)
          de=-deltaFEQ(k,n)
          tl=tliFEQ(k,n)
          ta=tau-gammaFEQ(k,n)
          if (ta.eq.0) ta=1d-40
          tf=be*ta**tl
          bb=aFEQ(k,n)*EXP5(ti*elntau+tf+al*de**dl)
          cc=2d0*bb
          dd=6d0*bb
          if (di.eq.2) cc=-cc
          if (di.eq.1) then
            Bx0=Bx0+bb
            Cx0=Cx0+2d0*al*de*cc
            Dx0=Dx0+(al+2d0*(al*de)**2)*dd
          elseif (di.eq.2) then
            Cx0=Cx0-cc
            Dx0=Dx0+2d0*al*de*dd
          elseif (di.eq.3) then
            Dx0=Dx0+dd
          endif

        !-----New terms
        elseif (k.ge.nFEQterms(n)-nFEQkg(n) .and. nFEQkg(n).gt.0) then
          al=alphaFEQ(k,n)
          be=betaFEQ(k,n)
          de=-deltaFEQ(k,n)
          tl=tliFEQ(k,n)
          ct=etaf(k,n)
          cd=eidf(k,n)
          ta=tau-gammaFEQ(k,n)
          if (ta.eq.0) ta=1d-40

          tf=be*ta**tl+ct
          df=al*de**dl+cd

          dfd=0d0
          dfd2=0d0
          bb=0d0
          if (eitf(k,n).eq.1.) then
            dfd =2d0*al*de
            dfd2=2d0*al
            bb=aFEQ(k,n)*EXP5(ti*elntau+1.d0/tf+df)
          elseif (eitf(k,n).eq.2.) then
            dfd =-2.d0*al*de/(tf+df)**2
            dfd2= 8.d0*(al*de)**2/(tf+df)**3-2.d0*al/(tf+df)**2
            bb=aFEQ(k,n)*EXP5(ti*elntau+1.d0/(tf+df))
          elseif (eitf(k,n).eq.3.) then
            dfd =-2.d0*al*de/df**2/tf
            dfd2= (8.d0*(al*de)**2/df**3-2.d0*al/df**2)/tf
            bb=aFEQ(k,n)*EXP5(ti*elntau+1.d0/(tf*df))
          endif

          cc=2d0*bb
          dd=6d0*bb
          if (di.eq.1) then
            cc=dfd*cc
            dd=(dfd**2+dfd2)/2.d0*dd
            Bx0=Bx0+bb
            Cx0=Cx0+cc
            Dx0=Dx0+dd
          elseif (di.eq.2) then
            dd=dfd*dd
            Cx0=Cx0+cc
            Dx0=Dx0+dd
          elseif (di.eq.3) then
            Dx0=Dx0+dd
          endif

        endif
      enddo

c  Fifth virial coefficient.
c     Ex0=Ex0*Vzred(n)**4/6d0          !Add code here, similar to that above.

      B  =Bx0*Vzred(n)
      C  =Cx0*Vzred(n)**2
      D  =Dx0*Vzred(n)**3/2d0
      E  =Ex0      ! ....

      end                                              subroutine VIRBCD
c
c ======================================================================
c
      subroutine VIRBCD12 (T,z,B,C,D,E,Bx1,Bx2,Cx1,Cx2,Dx1,Dx2,Ba,Ca)
c
c  Compute virial coefficients as a function of temperature and composition.
c  The routine currently works only for pure fluids and for the Helmholtz equation.
c  All values are computed exactly based on the terms in the EOS, not
c  as was done in VIRB by calculating properties at a density of 1d-8.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        B--Second virial coefficient [L/mol]       = a01
c        C-- Third virial coefficient [(L/mol)^2]   = a02
c        D--Fourth virial coefficient [(L/mol)^3]   = a03/2d0
c        E-- Fifth virial coefficient [(L/mol)^4]   = a04/6d0
c             assume nomenclature of a02=[partial^2(alphar)/partial(del)^2] above
c      Bx1--1st derivative of B with respect to T [L/(mol-K)]
c      Bx2--2nd derivative of B with respect to T [L/mol/K^2]
c      Cx1--1st derivative of C with respect to T [(L/mol)^2/K]
c      Cx2--2nd derivative of C with respect to T [(L/mol)^2/K^2]
c       Ba--Second acoustic virial coefficient [L/mol]
c       Ca--Third acoustic virial coefficient [(L/mol)^2]
c           see Trusler and Zarari, J. Chem. Thermodyn., 28:329-335, 1996.
c               Gillis and Moldover, Int. J. Theromphys., 17(6):1305-1324, 1996.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-01-10 EWL, original version
c  10-08-11 EWL, include Gaussian terms and change input parameters
c  08-20-14 EWL, add e as an input although not yet calculated
c  06-11-15 EWL, add temporary variables Bx0, Cx0, Dx0, and Ex0 to avoid problems with fitting and multiple cores
c  06-20-16 EWL, see comments under THERM

      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::VIRBCD12

      B=0d0
      C=0d0
      D=0d0
      E=0d0
      Bx0=0d0
      Bx1=0d0
      Bx2=0d0
      Cx0=0d0
      Cx1=0d0
      Cx2=0d0
      Dx0=0d0
      Dx1=0d0
      Dx2=0d0
      Ex0=0d0
      Ba=0d0
      Ca=0d0
      if (T.le.1d-10) RETURN

      call ISPURE (z,n)
      do i=1,nc
        if (iEOSk(i).ne.iEOSFEQ) n=0
      enddo
      if (n.eq.0) then       !Mixtures are not yet supported in this routine.
        call VIRB  (T,z,B)
        call VIRC  (T,z,C)
        call VIRD  (T,z,D)
        call DBDT  (T,z,Bx1)
        call DBDT2 (T,z,Bx2)
        call DCDT  (T,z,Cx1)
        call DCDT2 (T,z,Cx2)
        call VIRBA (T,z,Ba)
        call VIRCA (T,z,Ca)
        RETURN
      endif

      tau=Tzred(n)/T
      elntau=XLOG(tau)
      do k=1,nFEQterms(n)
        di=diFEQ(k,n)
        ti=tiFEQ(k,n)
        dl=dliFEQ(k,n)
        if (k.le.nFEQpe(n)) then
          t1=ti/T
          t2=t1*(ti+1d0)/T
          bb=aFEQ(k,n)*EXP5(ti*elntau)
          cc=2d0*bb
          dd=6d0*bb
          if (di.ne.2) cc=-cc
          if (di.ne.3) dd=-dd
          if (di.eq.1) then
            Bx0=Bx0+bb                 !Second virial coefficient (B).
            Bx1=Bx1-bb*t1              !dB/dT
            Bx2=Bx2+bb*t2              !d^B/dT^2
          endif
          if (di.eq.2 .or. di+dl.eq.2) then
            Cx0=Cx0+cc                 !Third virial coefficient (C).
            Cx1=Cx1-cc*t1              !dC/dT
            Cx2=Cx2+cc*t2              !d^C/dT^2
            if (di.ne.2) Dx0=Dx0-dd/2d0          !di=1 and dl=1 contributes to D.
          endif
          if (di.eq.3 .or. di+dl.eq.3) then
            Dx0=Dx0+dd                 !Fourth virial coefficient (D).
            Dx1=Dx1-dd*t1              !dD/dT
            Dx2=Dx2+dd*t2              !d^D/dT^2
          endif

        elseif (k.le.nFEQpe(n)+nFEQgaus(n)) then
          al=alphaFEQ(k,n)
          be=betaFEQ(k,n)
          de=-deltaFEQ(k,n)
          tl=tliFEQ(k,n)
          ta=tau-gammaFEQ(k,n)
          if (ta.eq.0) ta=1d-40
          tf=be*ta**tl
          bb=aFEQ(k,n)*EXP5(ti*elntau+tf+al*de**dl)
          cc=2d0*bb
          dd=6d0*bb
          if (di.eq.2) cc=-cc
          t1=(ti+tl*tf/ta*tau)/T
          t2=(ti*(ti+1d0)+4d0*be*ta*(ti+1d0)*tau+
     &            be*(2d0+4d0*tf)*tau**2)/T**2
          if (di.eq.1) then
            cc=2d0*al*de*cc
            dd=(al+2d0*(al*de)**2)*dd
            Bx0=Bx0+bb
            Cx0=Cx0+cc
            Dx0=Dx0+dd
            Bx1=Bx1-bb*t1
            Bx2=Bx2+bb*t2
            Cx1=Cx1-cc*t1
            Cx2=Cx2+cc*t2
          elseif (di.eq.2) then
            dd=2d0*al*de*dd
            Cx0=Cx0-cc
            Cx1=Cx1+cc*t1
            Cx2=Cx2-cc*t2
            Dx0=Dx0+dd
          elseif (di.eq.3) then
            Dx0=Dx0+dd
          endif

         !-----New terms
        elseif (k.ge.nFEQterms(n)-nFEQkg(n) .and. nFEQkg(n).gt.0) then
          al=alphaFEQ(k,n)
          be=betaFEQ(k,n)
          de=-deltaFEQ(k,n)
          tl=tliFEQ(k,n)
          ct=etaf(k,n)
          cd=eidf(k,n)
          ta=tau-gammaFEQ(k,n)
          if (ta.eq.0) ta=1d-40

          tf=be*ta**tl+ct
          df=al*de**dl+cd

          dfd=0d0
          dfd2=0d0
          bb=0d0
          if (eitf(k,n).eq.1.) then
            dfd =2d0*al*de
            dfd2=2d0*al
            bb=aFEQ(k,n)*EXP5(ti*elntau+1.d0/tf+df)
          elseif (eitf(k,n).eq.2.) then
            dfd =-2.d0*al*de/(tf+df)**2
            dfd2= 8.d0*(al*de)**2/(tf+df)**3-2.d0*al/(tf+df)**2
            bb=aFEQ(k,n)*EXP5(ti*elntau+1.d0/(tf+df))
          elseif (eitf(k,n).eq.3.) then
            dfd =-2.d0*al*de/df**2/tf
            dfd2= (8.d0*(al*de)**2/df**3-2.d0*al/df**2)/tf
            bb=aFEQ(k,n)*EXP5(ti*elntau+1.d0/(tf*df))
          endif

          cc=2d0*bb
          dd=6d0*bb
          if (di.eq.1) then
            cc=dfd*cc
            dd=(dfd**2+dfd2)/2.d0*dd
            Bx0=Bx0+bb
            Cx0=Cx0+cc
            Dx0=Dx0+dd
          elseif (di.eq.2) then
            dd=dfd*dd
            Cx0=Cx0+cc
            Dx0=Dx0+dd
          elseif (di.eq.3) then
            Dx0=Dx0+dd
          endif

        endif
      enddo

c  Fifth virial coefficient.
c     Ex0=Ex0*Vzred(n)**4/6d0          !Add code here, similar to that above.

      B  =Bx0*Vzred(n)
      Bx1=Bx1*Vzred(n)
      Bx2=Bx2*Vzred(n)
      C  =Cx0*Vzred(n)**2
      Cx1=Cx1*Vzred(n)**2
      Cx2=Cx2*Vzred(n)**2
      D  =Dx0*Vzred(n)**3/2d0
      E  =Ex0      ! ....

c  Acoustic virial coefficients.
      call THERM0 (T,0d0,z,P0,e0,h0,s0,Cv0,Cp00,w0,a0,g0)
      if (Cp00.le.0.) RETURN
      gpg=XDIV(Cp00,Cp00-Rxgas)
      q=B+(2d0*gpg-1d0)*T*Bx1+(gpg-1d0)*T**2*Bx2
      Ca=(gpg-1d0)*q**2+(2d0*gpg+1d0)*C
      Ca=(Ca+(gpg**2-1d0)*T*Cx1+(gpg-1d0)**2/2d0*T**2*Cx2)/gpg
      Ba=2d0*B+2d0*(gpg-1d0)*T*Bx1+(gpg-1d0)**2/gpg*T**2*Bx2

      end                                            subroutine VIRBCD12
c
c ======================================================================
c
      subroutine VIRB (T,z,B)
c
c  Compute the second virial coefficient B (L/mol) as a function of
c  temperature T (K) and composition x (array of mole fractions).
c  This routine approximates B.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c        B--Second virial coefficient [L/mol]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::VIRB

      B=0d0
      Dx=1d-8
      if (T.le.0d0) RETURN
      call REDX (z,T0,D0)
      B=PHIX(0,1,T0/T,Dx/D0,z)/Dx
      end                                                subroutine VIRB
c
c ======================================================================
c
      subroutine DBDT (T,z,dBT)
c
c  Compute the 1st derivative of B [dBT (L/mol-K)] as a function of
c  temperature T (K) and composition x (array of mole fractions).
c  This routine approximates dBT.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c      dBT--1st derivative of B with respect to T [L/(mol-K)]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DBDT

      dBT=0d0
      Dx=1d-8
      if (T.le.0d0) RETURN
      call REDX (z,T0,D0)
      dBT=-PHIX(1,1,T0/T,Dx/D0,z)/Dx/T
      end                                                subroutine DBDT
c
c ======================================================================
c
      subroutine DBDT2 (T,z,dBT2)
c
c  Compute the 2nd derivative of B [d^2BdT^2 (L/mol-K^2)] as a function of
c  temperature T (K) and composition x (array of mole fractions).
c  This routine approximates dBT2.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c     dBT2--2nd derivative of B with respect to T [L/mol/K^2]

      include 'COMMONS.INC'
      dimension z(ncmax)

      dBT2=0d0
      Dx=1d-8
      if (T.le.0d0) RETURN
      call REDX (z,T0,D0)
      phi21=PHIX(2,1,T0/T,Dx/D0,z)
      phi11=PHIX(1,1,T0/T,Dx/D0,z)
      dBT2=(phi21+2d0*phi11)/Dx/T**2
      end                                               subroutine DBDT2
c
c ======================================================================
c
      subroutine VIRC (T,z,C)
c
c  Compute the third virial coefficient C (L/mol)^2 as a function of
c  temperature T (K) and composition x (array of mole fractions).
c  This routine approximates C.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c        C-- Third virial coefficient [(L/mol)^2]   = a02

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::VIRC

      C=0d0
      Dx=1d-6
      if (T.le.0d0) RETURN
      call REDX (z,T0,D0)
      C=PHIX(0,2,T0/T,Dx/D0,z)/Dx**2
      end                                                subroutine VIRC
c
c ======================================================================
c
      subroutine DCDT (T,z,dCT)
c
c  Compute the 1st derivative of C [dCT (L/mol)^2-K)] as a function of
c  temperature T (K) and composition x (array of mole fractions).
c  This routine approximates dCT.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c      dCT--1st derivative of C with respect to T [(L/mol)^2/K]

      include 'COMMONS.INC'
      dimension z(ncmax)

      dCT=0d0
      Dx=1d-8
      if (T.le.0d0) RETURN
      call REDX (z,T0,D0)
      dCT=-PHIX(1,2,T0/T,Dx/D0,z)/Dx**2/T
      end                                                subroutine DCDT
c
c ======================================================================
c
      subroutine DCDT2 (T,z,dCT2)
c
c  Compute the 2nd derivative of C [d^2CdT^2 (L/mol-K)^2)] as a function of
c  temperature T (K) and composition x (array of mole fractions).
c  This routine approximates dCT2.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c        B--Second virial coefficient [L/mol]
c     dCT2--2nd derivative of C with respect to T [(L/mol)^2/K^2]

      include 'COMMONS.INC'
      dimension z(ncmax)

      dCT2=0d0
      if (T.le.0d0) RETURN
      Dx=1d-8
      call REDX (z,T0,D0)
      phi22=PHIX(2,2,T0/T,Dx/D0,z)
      phi12=PHIX(1,2,T0/T,Dx/D0,z)
      dCT2=(phi22+2d0*phi12)/Dx**2/T**2
      end                                               subroutine DCDT2
c
c ======================================================================
c
      subroutine VIRD (T,z,D)
c
c  Compute the fourth virial coefficient D (L/mol)^3 as a function of
c  temperature T (K) and composition x (array of mole fractions).
c  This routine approximates D.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c        D--Fourth virial coefficient [(L/mol)^3]   = a03/2d0

      include 'COMMONS.INC'
      dimension z(ncmax)

      D=0d0
      Dx=1d-4
      if (T.le.0d0) RETURN
      call REDX (z,T0,D0)
      D=PHIX(0,3,T0/T,Dx/D0,z)/Dx**3/2d0
      end                                                subroutine VIRD
c
c ======================================================================
c
      subroutine VIRBA (T,z,Ba)
c
c  Compute the second acoustic virial coefficient Ba (L/mol) as a function
c  of temperature (K) and composition x (array of mole fractions).
c  For further information, see
c  Trusler and Zarari, J. Chem. Thermodyn., 28:329-335, 1996.
c  Gillis and Moldover, Int. J. Theromphys., 17(6):1305-1324, 1996.
c  This routine approximates Ba.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c       Ba--Second acoustic virial coefficient [L/mol]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::VIRBA

      Ba=0d0
      if (T.le.0d0) RETURN
      call VIRB (T,z,B)
      call DBDT (T,z,dBT)
      call DBDT2 (T,z,dBT2)
      Cp00=Cp0(T,z)
      gpg=Cp00/(Cp00-Rxgas)
      if (lNE0(gpg))
     &Ba=2d0*B+2d0*(gpg-1d0)*T*dBT+(gpg-1d0)**2/gpg*T**2*dBT2
      end                                               subroutine VIRBA
c
c ======================================================================
c
      subroutine VIRCA (T,z,Ca)
c
c  Compute the third acoustic virial coefficient Ca (L/mol)^2 as a function
c  of temperature (K) and composition x (array of mole fractions).
c  For further information, see
c  Estela-Uribe and Trusler, Int. J. Theromphys., 21(5):1033, 2000.
c  Gillis and Moldover, Int. J. Theromphys., 17(6):1305-1324, 1996.
c  This routine approximates Ca.  For pure fluids, the routine VIRBCD is exact.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Output:
c       Ca--Third acoustic virial coefficient [(L/mol)^2]

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::VIRCA

      Ca=0d0
      if (T.le.0d0) RETURN
      call VIRB (T,z,B)
      call VIRBA (T,z,Ba)
      call DBDT (T,z,dBT)
      call DBDT2 (T,z,dBT2)
      call VIRC (T,z,C)
      call DCDT (T,z,dCT)
      call DCDT2 (T,z,dCT2)
      Cp00=Cp0(T,z)
      gpg=Cp00/(Cp00-Rxgas)
      q=B+(2d0*gpg-1d0)*T*dBT+(gpg-1d0)*T**2*dBT2
      Ca=(gpg-1d0)*q**2+(2d0*gpg+1d0)*C
      Ca=Ca+(gpg**2-1d0)*T*dCT+(gpg-1d0)**2/2d0*T**2*dCT2
      Ca=XDIV(Ca,gpg)
      end                                               subroutine VIRCA
c
c ======================================================================
c
      function RPNEQN (iFlag,icomp,ivar,T,D,z,eta,ierr,herr)
c
c  Implement functions based on reverse Polish notation for any property.
c  The routine is currently only used for viscosity and thermal
c  conductivity, but could be used for anything based on mathematical
c  notation and operators.  The notation is placed in the fluid file
c  in reverse order, where each command places something on a stack,
c  acts only on the upper most item on the stack, or joins the top two
c  values on the stack, such as through addition, multiplication, etc.
c
c  Inputs:
c    iFlag--When not set to zero, particular sections from the fluid file
c           will be read in, these are:
c           -1 - Viscosity (uses array 1 for variables, 2 for dilute gas,
c                 3 for second virial equation, 4 for residual function,
c                 and 5 for critical enhancement).
c           -2 - Thermal conductivity (uses array 6 for variables, 7 for
c                 dilute gas, and 8 for residual function).
c           -3 - Thermal conductivity critical enhancement (uses array 9
c                 for variables and 10 for the enhancement (function).
c            1 and higher - Open for other needs.
c    icomp--Component number in the mixture
c     ivar--Array number identifying the location of the property to be
c             calculated (for example, 4 to calculate the viscosity
c             residual function).
c           When calling this routine to read a file, set ivar
c             to the unit number where the values will be read.
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition array (array of mole fractions)
c      eta--Viscosity [uPa-s] (only required if the viscosity is used
c           in the calculation of thermal conductivity).
c
c  Outputs:
c    RPNEQN--Value of the function
c     ierr--Error flag:  0 - Successful
c                      691 - Function error
c     herr--Error string (character*255)


c  General comments:
c    An exclamation point can be placed at the end of any line to add comments.
c    To add additional slots for new functions (such as $DG, $CI, $FN), the value of maxopst may need to be increased in the common
c      block RPNCF in the COMMONS.INC file.
c    Many examples are given in the fluid files as guides to new functions.  The most interesting are those in ammonia, hydrogen,
c      helium, R23, D2O, methane, and methanol.  Helium is by far the most interesting, and should be compared with the
c      routines TCXHE and ETAHE.  The fluid file for CO2 shows one of the best layouts for making new functions.
c    Viscosity has five functions available (for use in the fluid file):
c      $VR - Setup for variables common to the next four functions
c      $DG - Dilute gas function
c      $VV - Second virial viscosity function
c      $RF - Residual function
c      $CE - Critical enhancement (rarely used)
c    Thermal conductivity has four available functions:
c      $VR - Setup for variables common to the next three functions
c      $DG - Dilute gas function
c      $RF - Residual function
c      $CE - Critical enhancement
c    Thermal conductivity enhancement has one available function:
c      $CE - Critical enhancement
c    The critical enhancement piece for thermal conductivity can be contained in either the TC7 or TK7 parts of the fluid file.
c    If a particular set of variables is only required by a single function, do not include it in the $VR section since it will be
c      unnecessarily calculated with the call to each function.  The $VR section is only for those variables needed by all of the functions.
c    The start of the coefficient section is indicated by a line containing only the characters "$CF".
c    A user defined function, as explained below, starts with the characters "$FN".
c
c
c  ---Valid notation and operators---
c
c  Summation commands:
c    SUM:##         Sum the next ## terms located under the $CF line (the coefficient section).
c                     Each row in this section contains five numbers, labeled here
c                     as N, t, d, m, and l for convenience.  The sum would then be:
c                     S=SUM[ N * TR^t * DR^d * del^m EXP(-Dr^l) ]
c                     The notation ## indicates the number of terms in the sum.
c    SUMEX:##       After performing the SUM described above, the exponential of the total sum is taken.
c    SUMTAU:##      Same as SUM, except that the value of TR is replaced by TAU.
c    SUMDEL:##      Same as SUM, except that the value of DR is replaced by DEL.
c    SUMTAUDEL:##   Same as SUM, except that the value of TR is replaced by TAU and the value of Dr is replaced by DEL.
c    SUMDTTHRD:##   Use t/3 in the SUM command.
c    SUMDDTHRD:##   Use d/3 in the SUM command.
c    SUMLOGT:##     Use LN(TR) instead of TR in the SUM command.
c    SUMLOGD:##     Use LN(DR) instead of DR in the SUM command.
c    SUMLOGL:##     Use Dr*LN(DR) instead of DR in the SUM command.
c    CMPLX:##       Use a complex formula...  (not referring to complex numbers)  The lines in the $CF section containing the complex
c                   coefficients must be surrounded by ^12TERMS before the first line and ^END12TERMS after the last line to indicate
c                   that each line contains 12 coefficients instead of 5.
c
c  RED       Reads the values of the reducing parameters from the coefficient list in the order of function, temperature, and density,
c              and recalculates the reduced temperature (TR=T/TRED) and reduced density (DR=D/DRED).  The first item in the row on the
c              coefficient list (called "function" above) is multiplied by the final value before being sent back to the calling routine.
c
c  Common operators:
c    +         Add the top two values together, remove the top item from the stack, and replace the second item on the stack by the output.
c    -         Subtract the top value on the stack from the value beneath it.
c    *         Multiply the top two values together.
c    /         Divide the value in the second slot in the stack by the value on top of the stack.
c    POWR      Take the second item on the stack to the power of the top value.
c    AND       If the top two items and non-zero, remove the first item and make the one below it equal to 1, otherwise make it equal to 0.
c    OR        If either the top or second item is non-zero, remove the first item and make the one below it 1, otherwise make it zero (after removing the first item).
c    XOR       Either the top item or the one under it must be non-zero, but not both (i.e, one has to be zero).  If so, set the second item to 1 otherwise 0.
c    XAND      Set the second item to 1 if the top two items are both zero, otherwise zero if either one of them is non-zero.
c    NOT       If the top item is zero, make it one, otherwise make it zero.
c    >         If the top item is greater than the item below it, set the one below to one, otherwise to zero.  Both of the original values are lost.
c    <         If the top item is less than the item below it, set to one, ... (as above)
c    >=        If the top item is greater than or equal to the item below it, set to one, ...
c    <=        If the top item is less than or equal to the item below it, set to one, ...
c    =         If the top item is equal to the item below it, set to one, ...
c    <>        If the top item is not equal to the item below it, set to one, ...
c
c  Operators that act only on the top item of the stack:
c    SQR       Square (multiply by itself) the top value on the stack.
c    SQRT      Take the square root of the top item.
c    CUBE      Cube the top item.
c    CBRT      Take the cube root.
c    SIGN      Change the sign.
c    ABS       Take the absolute value.
c    INV       Invert the value on top (1/x).
c    EXP       Take the exponential of the top item.
c    LOG       Take the natural logarithm.
c    LOG10     Take the logarithm on base 10.
c    POWERTEN  Calculate 10^x.
c    SIN       Take the sine of the top item.
c    COS       Take the cosine.
c    TAN       Take the tangent.
c    SINH      Take the hyperbolic sine.
c    COSH      Take the hyperbolic cosine.
c    TANH      Take the hyperbolic tangent.
c
c  Notation that adds a value to the stack:
c    CNST      Add the next coefficient (from the $CF section) to the stack, ignoring the 4 values to the right of the coefficient
c                on the same row in the coefficient list.
c    THRD      Add the value of 1/3.
c    THRD2     Add the value of 2/3.
c    BOLTZ     Add Boltzmann's constant.
c    AVOG      Add Avogadro's number.
c    PI        Add the value of Pi.
c    RGAS      Add the gas constant to the stack.
c    WMAX      Add the molar mass of the fluid.
c    TEMP      Add the temperature.
c    DENS      Add the density.
c    TAU       Add the value of TAU.
c    DEL       Add the value of DEL.
c    TAU1      Add the value of TAU1.           TAU1 and DEL1 are a second set of variables that can be used solely as constants.  For example, the value of tau may change
c    DEL1      Add the value of DEL1.           in some situations, and this can be used to store that variable.  However, these cannot be used in the sums such as SUMTAU.  They are just constants.
c    TR        Add the reduced temperature (T/TRED).
c    DR        Add the reduced density (D/DRED).
c    TRED      Add the reducing temperature.
c    DRED      Add the reducing density.
c    TC        Add the critical temperature from the fluid file.
c    DC        Add the critical density from the fluid file.
c    PC        Add the critical pressure from the fluid file.
c
c  Notation that sets certain parameters (and removes the value from the stack):
c  (See comments above about the use of TAU vs. TR, etc.)
c    =DEL      Set the value of DEL with the top item on the stack.
c    =TAU      Set the value of TAU.
c    =DEL1     Set the value of DEL1 with the top item on the stack.
c    =TAU1     Set the value of TAU1.
c    =DRED     Set the reducing density and recalculate the reduced density (DR).
c    =TRED     Set the reducing temperature and recalculate the reduced density (TR).
c    =DR       Overwrites the reduced density with the value on the top of the stack.
c    =TR       Overwrites the reduced temperature with the value on the top of the stack.
c
c    Constants and variables:
c    =V##      Set a variable equal to the value on top of the stack and remove that value from the stack.  The ## indicates a one or
c                two digit number that identifies the variable.
c    =C##      Set a constant equal to the value on top of the stack and remove that value from the stack.  There is no difference between
c                C## and V## except to help the user remember which are simple constants and which were set based on a formula.
c                Both can be reset at any time.
c    V##       Put the value stored in =V## previously on top of the stack.  The value in V## is not erased and can be used again.
c    C##       Identical to V##.
c    ###       Place a one to three digit integer on the stack.  The number must be positive.  To make negative, use the
c              command SIGN to change the sign of the number.  Values greater than 999 must be placed in the coefficients section and
c              retrieved with the CNST command.
c
c  Special commands:
c    P         Calculate the pressure and add the value to the stack.
c    DPDD      Calculate dP/dD and add the value to the stack.
c    D2PDD2    Calculate d^2P/dD^2.
c    DPDT      Calculate dP/dT from the equation of state.
c    ETA       Add the value of eta sent to this routine to the stack.  The value of the
c                viscosity cannot be calculated here because some viscosity functions
c                also call this routine.
c    CI        Calculate the collision integral.
c    DUP       Duplicate the top value on the stack (thus the top two items will be identical).
c    POP       Delete the top item from the stack.
c    EXIT      Exit the loop (but don't exit the routine if both variables and the function are being calculated,
c                rather just exit the variable section and then calculate the function).
c    JUMP      Jump to the jumping point defined by JP## (see below) located on the top of the stack.
c
c  Conditional commands*:
c            The characters ^^ below indicate one of 6 conditional keys to compare two number:  =, <=, >=, <, >, or <> (where the last one is "not equal").
c               Two additional numbers must be place on the stack before one of these commands is entered.  Thus, the command:
c               100 300 POP<= would remove the item from the stack before the number 100 if 300 <= 100.  Whether or not
c               the command is true, the 100 and 300 would be removed from the stack after executing the POP<= command.
c               The JUMP^^ command requires three previously loaded numbers on the stack.  For example, the command:
c               JMP5 100 300 JUMP<= would jump to the pointer labeled somewhere else as "JP5" if 300 <= 100.  This jump command
c               removes the upper three items from the stack after executing the command and before jumping.  The pointer to the location
c               of the current coefficient row is changed to match the location where the jump is made.
c    POP^^      Pop the previous value on the stack if the condition is true.
c    EXIT^^     Exit the loop if the condition is true.
c    JUMP^^     Jump to the specified jump point if the condition is true.
c    JP##       Add to the stack a one or two digit number of the jumping point defined below.
c    JMP##      Location of the jump point.  This command does nothing to the stack, it just leaves a mark in the command list to identify the jumping point.
c
c  Examples:
c
c  $FN     -->  Multiply 10 by 30, divide by 2, subtract 50, take the exponential of 2, add it to the previous value, and take the natural log (LN) of the whole thing.
c                                       =LOG[(10*30)/2-50+EXP(2)]
c                                       Answer: 4.676458
c  $FN 1 2 3 4 5 6 * + / POWR +         -->  Add the numbers 1 through 6 to the stack, then multiply 6 by 5, add it to 4, divide 3 by this value, take the value
c                                            of 2 to the power of the value just obtained, and add 1.
c                                       =2^{3/[(6*5)+4]}+1
c                                       Answer: 2.063069
c  $FN CNST 4 POWR CNST / SIGN THRD * ABS INV PI * LOG   -->  Assume the first two items in the coefficient list are 5.5 and 6.6.  Take 5.5 to the power of 4, divide by 6.6,
c                                                             change the sign, multiply by 1/3, take the absolute value, invert the number, multiply by Pi, and take the natural log of the final result.
c                                       =LOG{1/ABS[-5.5^4/6.6*(1/3)]*PI}
c                                       Answer: -2.688581
c  $FN 33 44 - =V1 55 =C1 V1 C1 * =V99 66 =C99 V99 C99 * -->  Take 33 - 44 and assign to V1, assign 55 to C1, put the values of V1 and C1 on the stack, and then multiply together and assign to V99,
c                                                             assign 66 to C99, place V99 and C99 on the stack and multiply together.
c                                       Answer: -39930
c  $FN RED SUM:2 3 =TAU 4 =DEL SUMTAUDEL:1 +  -->  Get the reducing values from the coefficient list and set TR=T/TRED and DR=D/DRED.  Sum the next two items in the coefficient list
c  $CF                     with TR and DR as the input variables.  Set TAU as 3 and DEL as 4.  Sum the next item in the list (well, not sum, but...) with TAU and DEL
c  10. 200. 15. 0. 0       as the input variables.  Add the two sums together.  Multiple by the function reducing value (10 in this case) and send back that result.
c  321 2. 4. 0. 1          Five values should be included on each coefficient line, the first four should always include the decimal point because of the oddities of Fortran.
c  654 3. 3. 0. 0          The last value is always an integer.  The second sum would then be 0.789*TAU^5*DEL^2*EXP(-DEL^2)
c  0.789 5. 2. 0. 1                     Answer: 58898.9  (assuming T=300 and D=20)
c
c  $FN 987 654 + 321 123 - 0 1 POP>=    ---> Add 987 and 654 (now first item on stack), subtract 123 from 321 (now the second item on the stack), add 0 and 1 to the stack (now four high),
c                                            compare top two (is 1>=0?), remove the 0 and 1 from the stack.  If comparison was true, pop the top item off the stack (in this case then leaves only the first item),
c                                       Answer:  1641
c  $FN 987 654 + 321 123 - 0 1 POP<=    Answer:  198
c
c  Below is an example problem to find when the value of y will exceed x if y starts at 1 and x starts at 100, and at each step the value of y is incremented by 2
c    and the value of x is incremented by 1.  First 100 is loaded into the variable V1 and 1 into the variable V2.  A jumping point was loaded into the working array at this place,
c    but it is ignored here.  Thus, there is currently nothing on the stack.  The value of V1 is then put on the stack, incremented by 1, and the result placed back into V1.
c    Same is done for V2, except the increment is 2.  Then the number 5 is placed on the stack with the JMP5 command to indicate that the jump will go to JP5.  V1 and V2
c    are then loaded and the JUMP< command determines if V2<V1.  The three top values on the stack are then removed (leaving an empty stack), and if the condition is true,
c    the routine jumps back to the JP5 point and continues the loop.  When not equal, the routine continues by putting V2 on the stack and then quit.  The final
c    value on the stack is the value returned to the calling routine.  For this function, the answer is 199.

c  $FN 100 =V1 1 =V2 JP5 V1 1 + =V1 V2 2 + =V2 JMP5 V1 V2 JUMP< V2
c
c  These can all be run with the following example program.
c  The 11 in the second call indicates that your values were stored in the 11th slot in the array used to calculate the function value.
c  (Slots 1-10 are used by the transport routines.)
c     program EXAMPLE
c     implicit double precision (a-z)
c     dimension z(20)
c     character*255 herr
c     open (unit=3,file='fn.txt')      !Open the file containing one of the function commands listed above.
c     f=RPNEQN (1,1, 3,300.d0,20.d0,z,0.d0,ierr,herr)      !Read the contents of the file.
c     f=RPNEQN (0,1,11,300.d0,20.d0,z,0.d0,ierr,herr)      !Execute the function call.
c     end
c
      include 'COMMONS.INC'
      include 'COMTRN.INC'
      include 'COMREFP.INC'
      character*255 herr,ha,hb,hc,her,LTRIM,UCASE
      dimension st(0:99),Vcnst(0:99),Ccnst(0:99),z(ncmax),x(ncmax)

c  Commands for summations of multiple terms.
      parameter (iSUM      = 1)
      parameter (iSUMEX    = 2)
      parameter (iSUMTAU   = 4)
      parameter (iSUMDEL   = 3)
      parameter (iSUMTAUDEL= 7)
      parameter (iSUMDTHRD = 5)
      parameter (iSUMTTHRD = 6)
      parameter (iSUMLOGD  = 8)
      parameter (iSUMLOGT  = 9)
      parameter (iSUMLOGL  =10)
      parameter (iCMPLX    =51)

c  Commands that combine the top two items on the stack and remove the upper most value.
      parameter (iPlus     =101)
      parameter (iMinus    =102)
      parameter (iTimes    =103)
      parameter (iDiv      =104)
      parameter (iPOWR     =135)
      parameter (iAND      =111)
      parameter (iOR       =112)
      parameter (iXOR      =113)
      parameter (iXAND     =114)
      parameter (iGRTR     =121)
      parameter (iLESS     =122)
      parameter (iGRTREQ   =123)
      parameter (iLESSEQ   =124)
      parameter (iEQUAL    =125)
      parameter (iNOTEQUAL =126)

c  Commands that only act on the top item on the stack.
      parameter (iSQR      =201)
      parameter (iSQRT     =202)
      parameter (iCUBE     =203)
      parameter (iCBRT     =204)
      parameter (iSIGN     =205)
      parameter (iABS      =206)
      parameter (iINV      =207)
      parameter (iEXP      =221)
      parameter (iLOG      =222)
      parameter (iLOG10    =223)
      parameter (iPOWERTEN =224)
      parameter (iSIN      =225)
      parameter (iCOS      =226)
      parameter (iTAN      =227)
      parameter (iSINH     =228)
      parameter (iCOSH     =229)
      parameter (iTANH     =230)
      parameter (iNOT      =231)

c  Commands that add to the stack.
      parameter (iCNST     =301)
      parameter (iTHRD     =302)
      parameter (iTHRD2    =303)
      parameter (iBOLTZ    =304)
      parameter (iAVOG     =305)
      parameter (iPI       =306)
      parameter (iRGAS     =307)
      parameter (iWMAS     =308)

c  Commands that place values related to density or temperature on the stack.
      parameter (iTEMP     =401)
      parameter (iDENS     =402)
      parameter (iTAU      =403)
      parameter (iDEL      =404)
      parameter (iTAU1     =405)
      parameter (iDEL1     =406)
      parameter (iTR       =407)
      parameter (iDR       =408)
      parameter (iTC       =409)
      parameter (iDC       =410)
      parameter (iPC       =411)
      parameter (iTRED     =412)
      parameter (iDRED     =413)

c  Commands that set certain variables dealing with density or temperature.
      parameter (iSetTR    =451)
      parameter (iSetDR    =452)
      parameter (iSetTAU   =453)
      parameter (iSetDEL   =454)
      parameter (iSetTAU1  =455)
      parameter (iSetDEL1  =456)
      parameter (iSetTRED  =457)
      parameter (iSetDRED  =458)
      parameter (iGetRED   =459)

c  Refprop commands to call certain functions.
      parameter (iPress    =501)
      parameter (iDPDD     =502)
      parameter (iDPDD2    =503)
      parameter (iDPDT     =504)
      parameter (ixETA     =505)
      parameter (iCI       =506)
      parameter (iCI0      =507)
      parameter (iCI1      =508)
      parameter (iCI2      =509)
      parameter (iCI3      =510)
      parameter (iOMEGAS   =511)
      parameter (iCv       =512)
      parameter (iCp       =513)
      parameter (iSnd      =514)

c  Special commands.
      parameter (iDUP      =601)
      parameter (iPOP      =602)
      parameter (iEXIT     =603)
      parameter (iJUMP     =604)
      parameter (iPOPeq    =620)
      parameter (iEXTeq    =630)
      parameter (iJMPeq    =640)

c  Constants and Variables
      parameter (iV0      =1100)
      parameter (iC0      =1200)
      parameter (iSV0     =1300)
      parameter (iSC0     =1400)
      parameter (iJP0     =1500)
      parameter (iJMP     =1600)
      parameter (i0       =2000)

      v=z(1)       !Used only to keep the compiler from complaining about z not being used, delete this line if composition becomes part of this routine.
      RPNEQN=0d0
      i=icomp
      ierr=0
      herr=' '
      her=' '
      if (iFlag.ne.0) goto 700         !Read in coefficients from fluid file.

      if (T.le.0. .or. D.lt.0.) RETURN
      if (n7oper(ivar,i).eq.0)  RETURN           ! Check if any operations need to be done.
      tau=1d0
      del=1d0
      tau1=1d0
      del1=1d0
      Tred=1d0     !Reducing variables.
      Dred=1d0
      Vred=1d0
      Tr=T         !Reduced T and D
      Dr=D
      in1=1
      ivar1=ivar
      if (ivar.le.5) ivar1=1
      if (ivar.ge.7 .and. ivar.le.9) ivar1=6
      if (n7oper(ivar1,i).eq.0) in1=2            !Check if variables need to be calculated first (stored in the ivar1 slot of the array).
      if (ivar1.eq.ivar) in1=2
      do ivv=in1,2           !Loop two times, once for the variables, and once for the requested function (dilute, background, residual, etc.)
        iv=ivar1
        if (ivv.eq.2) iv=ivar
        k=n7st(iv,i)-1       !Coefficient counter
        m=0                  !Stack counter
        st=0d0               !Stack array
        n=0
        n1=SUM(n7oper(1:iv-1,i))
        do
          n=n+1              !Loop over the number of operations for DG, BF, RF, etc.
          n1=n1+1            !Used only to view h7Oper in debugging.
          if (n.gt.n7oper(iv,i)) EXIT
          iop=i7oper(n,iv,i)
          if (iop.eq.0) goto 621       !<--- To debug, put breakpoint here and watch the stack st(m).  m is the index of the top of the stack.
                                       !     The array h7Oper contains all the commands in it to show what will happen next at each step n.  Use h7Oper(n1) to see the current step.


c---SUM---
          if (iop.lt.50) then                    !Check for a summation command.
            xt=1d0
            xd=1d0
            xTr=Tr
            xDr=Dr
            lexp=.false.
            llog=.false.
            if (iop.ne.iSUM) then
              if (iop.eq.iSUMEX)     lexp=.true.           !Sum followed by EXP(SUM)
              if (iop.eq.iSUMTTHRD)  xt=3d0                !Sum with Tr^(1/3)
              if (iop.eq.iSUMDTHRD)  xd=3d0                !Sum with Dr^(1/3)
              if (iop.eq.iSUMTAU)    xTr=tau               !Sum with tau rather than Tr
              if (iop.eq.iSUMDEL)    xDr=del               !Sum with del rather than Dr
              if (iop.eq.iSUMTAUDEL) xTr=tau               !Sum with tau and del
              if (iop.eq.iSUMTAUDEL) xDr=del               !Sum with tau and del
              if (iop.eq.iSUMLOGT)   xTr=XLOG(Tr)          !Sum with LOG(Tr)
              if (iop.eq.iSUMLOGD)   xDr=XLOG(Dr)          !Sum with LOG(Dr)
              if (iop.eq.iSUMLOGL)   llog=.true.           !Sum with Dr*LOG(Dr)
            endif
            sumv=0d0
            do j=1,n7cf(n,iv,i)
              k=k+1          !Counter for the coefficients in the array.
              v=cfoper(k,1,i)*xTr**(cfoper(k,2,i)/xt)
              if (cfoper(k,3,i).ne.0.) v=v*xDr**(cfoper(k,3,i)/xd)
              if (cfoper(k,4,i).ne.0.) v=v*del**cfoper(k,4,i)
              if (icfoper(k,i).ge.1) then
                if (llog) then
                  v=v*XLOG(xDr**icfoper(k,i))
                else
                  v=v*EXP5(-xDr**icfoper(k,i))
                endif
              endif
              sumv=sumv+v
            enddo
            m=m+1
            st(m)=sumv       !Add sum to stack.
            if (lexp) st(m)=EXP5(sumv)
          elseif (iop.le.99) then
            if (iop.eq.iCMPLX) then    !Use the complex form for the sum.
              sumv=0
              do j=1,n7cf(n,iv,i)
                k=k+1
                xTr=Tr-cfoper(k,8,i)
                xDr=Dr-cfoper(k,9,i)
                if (cfoper(k,5,i).eq.1.) xTr=ABS(xTr)
                if (cfoper(k,4,i).eq.1.) xDr=ABS(xTr)
                v=cfoper(k,7,i)*xTr**cfoper(k,5,i)+
     &            cfoper(k,6,i)*xDr**cfoper(k,4,i)
                v=cfoper(k,1,i)*Tr**cfoper(k,2,i)*
     &            Dr**cfoper(k,3,i)*EXP5(v)
                sumv=sumv+v
              enddo
              m=m+1
              st(m)=sumv
            endif


c---RED---
          elseif (iop.eq.iGetRED) then           !Read in new reducing parameters.
            k=k+1
            Vred=cfoper(k,1,i)
            Tred=cfoper(k,2,i)
            Dred=cfoper(k,3,i)
            Tr=XDIV(T,Tred)
            Dr=XDIV(D,Dred)


c---100---
          elseif (iop.le.199) then
            if (m.gt.1) then           !Check for operations going below the stack, but don't return error (which can be used to obtain certain results sometimes).
              stt=0d0
              if (iop.eq.iPlus) then
                st(m-1)=st(m-1)+st(m)
              elseif (iop.eq.iMinus) then
                st(m-1)=st(m-1)-st(m)
              elseif (iop.eq.iTimes) then
                st(m-1)=st(m-1)*st(m)
              elseif (iop.eq.iDiv) then
                st(m-1)=XDIV(st(m-1),st(m))
              elseif (iop.eq.iPOWR) then
                st(m-1)=XPOWR(st(m-1),st(m))
              elseif (iop.eq.iAND) then
                if (st(m-1).ne.0. .and. st(m).ne.0.) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iOR) then
                if (st(m-1).ne.0. .or.  st(m).ne.0.) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iXOR) then
                if (st(m-1).ne.0. .and. st(m).eq.0.) stt=1d0
                if (st(m-1).eq.0. .and. st(m).ne.0.) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iXAND) then
                if (st(m-1).eq.0. .and. st(m).eq.0.) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iGRTR) then
                if (st(m).gt.st(m-1)) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iless) then
                if (st(m).lt.st(m-1)) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iGRTREQ) then
                if (st(m).ge.st(m-1)) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iLESSeq) then
                if (st(m).le.st(m-1)) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iEQUAL) then
                if (lEqual(st(m),st(m-1))) stt=1d0
                st(m-1)=stt
              elseif (iop.eq.iNOTEQUAL) then
                if (.not.lEqual(st(m),st(m-1))) stt=1d0
                st(m-1)=stt
              else
                goto 630
              endif
              m=m-1
            endif


c---200---
          elseif (iop.le.299) then
            if (iop.eq.iSQR) then
              st(m)=st(m)**2
            elseif (iop.eq.iSQRT) then
              st(m)=XSQRT(st(m))
            elseif (iop.eq.iCUBE) then
              st(m)=st(m)**3
            elseif (iop.eq.iCBRT) then
              st(m)=XCBRT(st(m))
            elseif (iop.eq.iSIGN) then
              st(m)=-st(m)
            elseif (iop.eq.iABS) then
              st(m)=ABS(st(m))
            elseif (iop.eq.iINV) then
              st(m)=XDIV(1d0,st(m))
            elseif (iop.eq.iEXP) then
              st(m)=EXP5(st(m))
            elseif (iop.eq.iLOG) then
              st(m)=XLOG(st(m))
            elseif (iop.eq.iLOG10) then
              if (st(m).gt.0) st(m)=LOG10(st(m))
            elseif (iop.eq.iPOWERTEN) then
              st(m)=10d0**st(m)
            elseif (iop.eq.iSIN) then
              st(m)=SIN(st(m))
            elseif (iop.eq.iCOS) then
              st(m)=COS(st(m))
            elseif (iop.eq.iTAN) then
              st(m)=TAN(st(m))
            elseif (iop.eq.iSINH) then
              st(m)=SINH(st(m))
            elseif (iop.eq.iCOSH) then
              st(m)=COSH(st(m))
            elseif (iop.eq.iTANH) then
              st(m)=TANH(st(m))
            elseif (iop.eq.iNOT) then
              stt=1d0
              if (st(m).ne.0.) stt=0d0
              st(m)=stt
            else
              goto 630
            endif


c---300---
          elseif (iop.le.399) then
            m=m+1
            if (iop.eq.iCNST) then     !Put the next coefficient on the stack.
              k=k+1
              st(m)=cfoper(k,1,i)
            elseif (iop.eq.iTHRD) then           !Add 1/3 to the stack.
              st(m)=thrd
            elseif (iop.eq.iTHRD2) then          !Add 2/3 to the stack.
              st(m)=thrd2
            elseif (iop.eq.iBOLTZ) then          !Boltzmann's constant=1.38064852d-23 J/K
              st(m)=xBoltz
            elseif (iop.eq.iAVOG) then           !Avogadro's number=6.022140857d+23 1/mol
              st(m)=xAVOG
            elseif (iop.eq.iPI) then             !Pi=3.14159265358979323846d0
              st(m)=xPi
            elseif (iop.eq.iRGAS) then           !Rgas=8.3144598 J/(mol-K)
              st(m)=xRgas
            elseif (iop.eq.iWMAS) then           !molar mass in g/mol
              st(m)=wmas(i)
            else
              goto 630
            endif


c---400---
          elseif (iop.le.449) then
            m=m+1
            if (iop.eq.iTEMP) then
              st(m)=T
            elseif (iop.eq.iDENS) then
              st(m)=D
            elseif (iop.eq.iTAU) then
              st(m)=tau
            elseif (iop.eq.iDEL) then
              st(m)=del
            elseif (iop.eq.iTAU1) then
              st(m)=tau1
            elseif (iop.eq.iDEL1) then
              st(m)=del1
            elseif (iop.eq.iTR) then
              st(m)=Tr
            elseif (iop.eq.iDR) then
              st(m)=Dr
            elseif (iop.eq.iTC) then
              st(m)=Tcrit(i)
            elseif (iop.eq.iDC) then
              st(m)=Dcrit(i)
            elseif (iop.eq.iPC) then
              st(m)=Pcrit(i)
            elseif (iop.eq.iTRED) then
              st(m)=Tred
            elseif (iop.eq.iDRED) then
              st(m)=Dred
            else
              goto 630
            endif

c---450---
          elseif (iop.le.499) then
            if (iop.eq.iSetTR) then
              Tr=st(m)
            elseif (iop.eq.iSetDR) then
              Dr=st(m)
            elseif (iop.eq.iSetTAU) then         !Reassign tau or del to the value on the stack.  This is not the same as the reducing values Tr and Dr.
              tau=st(m)
            elseif (iop.eq.iSetDEL) then
              del=st(m)
            elseif (iop.eq.iSetTAU1) then        !Reassign tau1 or del1 to the value on the stack.
              tau1=st(m)
            elseif (iop.eq.iSetDEL1) then
              del1=st(m)
            elseif (iop.eq.iSetTRED) then        !Reassign Dred or Tred and recalculate Dr or Tr.
              Tred=st(m)
              Tr=XDIV(T,Tred)
            elseif (iop.eq.iSetDRED) then
              Dred=st(m)
              Dr=XDIV(D,Dred)
            else
              goto 630
            endif
            if (m.gt.0) m=m-1


c---500---
          elseif (iop.le.599) then
            m=m+1
            x(1:nc)=0
            x(i)=1
            if (iop.eq.iPress) then
              call PRESS (T,D,x,P)
              st(m)=P
            elseif (iop.eq.iDPDD) then
              call DPDDX (T,D,x,dPdD)
              st(m)=dPdD
            elseif (iop.eq.iDPDD2) then
              call DPDD2 (T,D,x,d2PdD2)
              st(m)=d2PdD2
            elseif (iop.eq.iDPDT) then
              dPdT=1d0
              if (ABS(D).gt.1d-12) call DPDTX (T,D,x,dPdT)
              st(m)=dPdT
            elseif (iop.eq.iCv) then
              call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
              st(m)=Cv
            elseif (iop.eq.iCp) then
              call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
              st(m)=Cp
            elseif (iop.eq.iSnd) then
              call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
              st(m)=w
            elseif (iop.eq.ixETA) then
              st(m)=eta
            elseif (iop.eq.iCI) then             !Calculate the LJ function and place on stack.
              tauhc=XDIV(T,Trddge(i))
              st(m)=ChEns1(i)*tauhc**ChEns2(i)/
     &             (sigmav(i)**2*OMEGA(i,T,epskv(i),hmdeta(i)))
            elseif (iop.eq.iCI0) then            !Calculate the LJ function from the CI0 form and place on stack.
              st(m)=OMEGA(i,T,epskv(i),'CI0')
            elseif (iop.eq.iCI1) then            !Calculate the LJ function from the CI1.
              st(m)=OMEGA(i,T,epskv(i),'CI1')
            elseif (iop.eq.iCI2) then            !Calculate the LJ function from the CI2.
              st(m)=OMEGA(i,T,epskv(i),'CI2')
            elseif (iop.eq.iCI3) then            !Calculate the LJ function from the CI3.
              st(m)=OMEGA(i,T,epskv(i),'CI3')
            elseif (iop.eq.iOMEGAS) then         !Calculate the collision integral from OMEGAS and place on stack (based on Neufeld et al.)
              st(m)=OMEGAS(2,2,T/epskv(i))
            else
              goto 630
            endif


c---600---
          elseif (iop.le.699) then
            if (iop.eq.iDUP) then
              m=m+1
              st(m)=st(m-1)
            elseif (iop.eq.iPOP) then            !Pop off the top item on the stack.
              if (m.gt.0) m=m-1
            elseif (iop.eq.iEXIT) then
              EXIT
            elseif (iop.eq.iJUMP) then
              if (m.gt.0) m=m-1                  !Remove two things from the stack (if the stack has two things on it).
              if (m.gt.0) m=m-1
              do j=1,n7oper(iv,i)
                if (i7oper(j,iv,i).eq.st(m+1)) n=j
              enddo
            elseif (iop.ge.iPOPeq .and. iop.le.iJMPeq+9) then
              lPop=.false.
              lJump=.false.
              lExit=.false.
              lTrue=.false.
              if (iop.le.iPOPeq+9) then
                lPop=.true.
                ieq=iop-iPopeq
              elseif (iop.ge.iJMPeq) then
                lJump=.true.
                ieq=iop-iJMPeq
              else
                lExit=.true.
                ieq=iop-iEXTeq
              endif
              if (ieq.eq.1) then                 ! =
                if (lEqual(st(m),st(m-1))) lTrue=.true.
              elseif (ieq.eq.2) then             ! <=
                if (st(m).le.st(m-1)) lTrue=.true.
              elseif (ieq.eq.3) then             ! >=
                if (st(m).ge.st(m-1)) lTrue=.true.
              elseif (ieq.eq.4) then             ! <
                if (st(m).lt.st(m-1)) lTrue=.true.
              elseif (ieq.eq.5) then             ! >
                if (st(m).gt.st(m-1)) lTrue=.true.
              elseif (ieq.eq.6) then             ! <>
                if (st(m).ne.st(m-1)) lTrue=.true.
              endif
              if (m.gt.0) m=m-1
              if (m.gt.0) m=m-1
              if (lTrue .and. lExit) then
                EXIT
              elseif (lTrue .and. lPop) then
                if (m.gt.0) m=m-1
              elseif (lJump) then
                if (m.gt.0) m=m-1
                if (lTrue) then
                  do j=1,n7oper(iv,i)            !Search for the jumping point.
                    if (i7oper(j,iv,i).eq.st(m+1)) then
                      ij=1
                      if (j.lt.n) ij=-1          !Check if it's before or after the current point.
                      do jj=n,j,ij
                        k=k+ij*n7cf(jj,iv,i)     !Add or subtract the number of coefficients used between the current point and the jumping point.
                      enddo
                      n=j    !Reset the current location.
                      EXIT
                    endif
                  enddo
                endif
              endif
            endif


c---1000---
          elseif (iop.ge.1000 .and. iop.le.3000) then
            if (iop.ge.i0 .and. iop.le.i0+999) then        !Add an integer between 0 and 999 to the stack.
              m=m+1
              st(m)=iop-i0
            elseif (iop.ge.iV0 .and. iop.le.iV0+99) then   !Place the value of a variable on the stack.
              m=m+1
              st(m)=Vcnst(iop-iV0)
            elseif (iop.ge.iC0 .and. iop.le.iC0+99) then   !Place the value of a constant on the stack.
              m=m+1
              st(m)=Ccnst(iop-iC0)
            elseif (iop.ge.iSV0 .and. iop.le.iSV0+99) then !Assign a variable (similar to a memory button on a calculator).
              Vcnst(iop-iSV0)=st(m)
              if (m.gt.0) m=m-1
            elseif (iop.ge.iSC0 .and. iop.le.iSC0+99) then !Assign a constant.
              Ccnst(iop-iSC0)=st(m)
              if (m.gt.0) m=m-1
            elseif (iop.ge.iJMP .and. iop.le.iJMP+99) then !Add the jump point number to the stack.
              m=m+1
              st(m)=iop-iJMP+iJP0
            elseif (iop.ge.iJP0 .and. iop.le.iJP0+99) then
                   !Jumping point, just ignore it.
            else
              goto 630
            endif


c---End---
          else
            goto 630
          endif
          if (m.lt.0) goto 631
          st(m+1:m+3)=0
        enddo
        if (m.gt.0) RPNEQN=st(m)*Vred            !May not be at bottom of stack if POP has been used.
      enddo
c---Exit routine---
      RETURN

c-----------------------   Read operations and coefficients from fluid file   -----------------------
 700  continue
c  n7oper is the number of operations.
c  n7st is the beginning location of the coefficients for each operation set.
c  n7cf is the number of terms in each sum for each operation.
      i=icomp
      i12=0
      nread=ivar
      n=0
      n1=0
      if (iFlag.eq.-1) then            !Viscosity
        ni1=1
        ni2=5
      elseif (iFlag.eq.-2) then        !Thermal conductivity
        ni1=6
        ni2=9
      elseif (iFlag.eq.-3) then        !Thermal conductivity enhancement
        ni1=10
        ni2=10
      elseif (iFlag.ge.1) then
        ni1=10+iFlag
        ni2=10+iFlag
      else
        goto 601
      endif
      if (ni1.gt.maxopst) goto 606
      llx=.false.
      iv=0
      do
        do
          read (nread,'(a255)',err=640,end=640) ha         !Find the next line that starts with $
          ha=LTRIM(ha)
          j=INDEX(ha,'!')
          if (j.gt.0) ha=ha(:j-1)
          if (ha(1:1).eq.'$') EXIT
        enddo
        ha=UCASE(ha)
        ix=iv
        iv=0
        if (iFlag.eq.-1) then          !Viscosity functions.
          if (ha(1:3).eq.'$VR')  iv=1  !  Variables
          if (ha(1:3).eq.'$DG')  iv=2  !  Dilute gas
          if (ha(1:3).eq.'$VV')  iv=3  !  Second virial viscosity
          if (ha(1:3).eq.'$RF')  iv=4  !  Residual function
          if (ha(1:3).eq.'$CE')  iv=5  !  Critical enhancement
        elseif (iFlag.eq.-2) then      !Thermal conductivity functions.
          if (ha(1:3).eq.'$VR')  iv=6  !  Variables
          if (ha(1:3).eq.'$DG')  iv=7  !  Dilute gas
          if (ha(1:3).eq.'$RF')  iv=8  !  Residual function
          if (ha(1:3).eq.'$CE')  iv=9  !  Critical enhancement
        elseif (iFlag.eq.-3) then      !Thermal conductivity enhancement functions.
          if (ha(1:3).eq.'$CE')  iv=10 !  Critical enhancement
        elseif (iFlag.ge.1) then
          if (ha(1:3).eq.'$FN')  iv=11 !Any user defined function
        endif
        if (ha(1:3).eq.'$CF')  EXIT    !Coefficients.  When found, exit this section and then read in the values from the fluid file.
        if (ix.gt.iv .or. iv.eq.0) goto 602
        ha=ha(5:)
        if (n7oper(iv,i).eq.0) n=0

c  Read in commands from the input line.
        do
          if (ha.eq.' ') EXIT
          ha=LTRIM(ha)       !Remove leading spaces.
          if (ha(1:1).eq.'!' .or. ha.eq.'END') EXIT     !Text can be added to a line in the fluid file after an exclamation point.
          n=n+1
          n1=n1+1
          if (n.gt.maxop) goto 603
          j=INDEX(ha,' ')
          hb=ha(1:j-1)       !Get next command on the line.
          ha=ha(j+1:)
          h7oper(n1)=hb      !The array h7oper is used only for debugging (to view the command code).
          j=INDEX(hb,':')    !If a colon is found, the number after it indicates the number of terms in the summation.
          n7cf(n,iv,i)=0
          if (j.gt.0) then
            read (hb(j+1:),*,err=640) n7cf(n,iv,i)
            hb=hb(1:j-1)
          endif
          if (hb.eq.'CMPLX') llx=.true.
          if (hb(1:4).eq.'CNST') n7cf(n,iv,i)=1            !For CNST and RED, values are contained in the coefficient section, so increment the coefficient counter.
          if (hb.eq.'RED') n7cf(n,iv,i)=1
          if (hb.eq.'ETA') l7eta=.true.          !Keep track if the viscosity is requested.

c  Set the reverse polish variable once a command has been identified.
          irpn=0
          i1=ICHAR(hb(1:1))-64

          if (i1.eq.iLetterS) then
            if (hb.eq.'SUM')       irpn=iSUM       !This one has to go before the other "SUM"'s
            if (hb.eq.'SUMEX')     irpn=iSUMEX
            if (hb.eq.'SUMTAU')    irpn=iSUMTAU
            if (hb.eq.'SUMDEL')    irpn=iSUMDEL
            if (hb.eq.'SUMTAUDEL') irpn=iSUMTAUDEL
            if (hb.eq.'SUMTTHRD')  irpn=iSUMTTHRD
            if (hb.eq.'SUMDTHRD')  irpn=iSUMDTHRD
            if (hb.eq.'SUMLOGT')   irpn=iSUMLOGT
            if (hb.eq.'SUMLOGD')   irpn=iSUMLOGD
            if (hb.eq.'SUMLOGL')   irpn=iSUMLOGL
            if (hb.eq.'SIGN')    irpn=iSIGN
            if (hb.eq.'SIN')     irpn=iSIN
            if (hb.eq.'SINH')    irpn=iSINH
            if (hb.eq.'SND')     irpn=iSND
            if (hb.eq.'SQR')     irpn=iSQR
            if (hb.eq.'SQRT')    irpn=iSQRT

          elseif (i1.ge.iLetterA .and. i1.le.iLetterC) then
            if (hb.eq.'ABS')     irpn=iABS
            if (hb.eq.'AND')     irpn=iAND
            if (hb.eq.'BOLTZ')   irpn=iBOLTZ
            if (hb.eq.'CBRT')    irpn=iCBRT
            if (hb.eq.'CI')      irpn=iCI
            if (hb.eq.'CI0')     irpn=iCI0
            if (hb.eq.'CI1')     irpn=iCI1
            if (hb.eq.'CI2')     irpn=iCI2
            if (hb.eq.'CI3')     irpn=iCI3
            if (hb.eq.'CNST')    irpn=iCNST
            if (hb.eq.'CMPLX')   irpn=iCMPLX
            if (hb.eq.'COS')     irpn=iCOS
            if (hb.eq.'COSH')    irpn=iCOSH
            if (hb.eq.'CP')      irpn=iCP
            if (hb.eq.'CUBE')    irpn=iCUBE
            if (hb.eq.'CV')      irpn=iCV

          elseif (i1.eq.iLetterD) then
            if (hb.eq.'D2PDD2')  irpn=iDPDD2
            if (hb.eq.'DC')      irpn=iDC
            if (hb.eq.'DEL')     irpn=iDEL
            if (hb.eq.'DEL1')    irpn=iDEL1
            if (hb.eq.'DENS')    irpn=iDENS
            if (hb.eq.'DPDD')    irpn=iDPDD
            if (hb.eq.'DPDT')    irpn=iDPDT
            if (hb.eq.'DR')      irpn=iDR
            if (hb.eq.'DRED')    irpn=iDRED
            if (hb.eq.'DUP')     irpn=iDUP

          elseif (i1.ge.iLetterE .and. i1.le.iLetterM) then
            if (hb.eq.'ETA')     irpn=ixETA
            if (hb.eq.'EXIT')    irpn=iEXIT
            if (hb.eq.'EXP')     irpn=iEXP
            if (hb.eq.'INV')     irpn=iINV
            if (hb.eq.'JUMP')    irpn=iJUMP
            if (hb.eq.'LOG')     irpn=iLOG
            if (hb.eq.'LOG10')   irpn=iLOG10

          elseif (i1.ge.iLetterN .and. i1.le.iLetterR) then
            if (hb.eq.'NOT')     irpn=iNOT
            if (hb.eq.'OR')      irpn=iOR
            if (hb.eq.'OMEGAS')  irpn=iOMEGAS
            if (hb.eq.'P')       irpn=iPress
            if (hb.eq.'PC')      irpn=iPC
            if (hb.eq.'PI')      irpn=iPI
            if (hb.eq.'POP')     irpn=iPOP
            if (hb.eq.'POWERTEN')irpn=iPOWERTEN
            if (hb.eq.'POWR')    irpn=iPOWR
            if (hb.eq.'RED')     irpn=iGetRED
            if (hb.eq.'RGAS')    irpn=iRGAS

          elseif (i1.eq.iLetterT) then
            if (hb.eq.'TAN')     irpn=iTAN
            if (hb.eq.'TANH')    irpn=iTANH
            if (hb.eq.'TAU')     irpn=iTAU
            if (hb.eq.'TAU1')    irpn=iTAU1
            if (hb.eq.'TC')      irpn=iTC
            if (hb.eq.'TEMP')    irpn=iTEMP
            if (hb.eq.'THRD')    irpn=iTHRD
            if (hb.eq.'THRD2')   irpn=iTHRD2
            if (hb.eq.'TR')      irpn=iTR
            if (hb.eq.'TRED')    irpn=iTRED

          elseif (i1.ge.iLetterU .and. i1.le.iLetterZ) then
            if (hb.eq.'WMAS')    irpn=iWMAS
            if (hb.eq.'XAND')    irpn=iXAND
            if (hb.eq.'XOR')     irpn=iXOR

          else
            if (hb.eq.'*')       irpn=iTimes
            if (hb.eq.'+')       irpn=iPlus
            if (hb.eq.'-')       irpn=iMinus
            if (hb.eq.'/')       irpn=iDiv
            if (hb.eq.'<')       irpn=iLESS
            if (hb.eq.'<=')      irpn=iLESSEQ
            if (hb.eq.'<>')      irpn=iNOTEQUAL
            if (hb.eq.'=')       irpn=iEQUAL
            if (hb.eq.'=DEL')    irpn=iSetDEL
            if (hb.eq.'=DEL1')   irpn=iSetDEL1
            if (hb.eq.'=DR')     irpn=iSetDR
            if (hb.eq.'=DRED')   irpn=iSetDRED
            if (hb.eq.'=TAU')    irpn=iSetTAU
            if (hb.eq.'=TAU1')   irpn=iSetTAU1
            if (hb.eq.'=TR')     irpn=iSetTR
            if (hb.eq.'=TRED')   irpn=iSetTRED
            if (hb.eq.'>')       irpn=iGRTR
            if (hb.eq.'>=')      irpn=iGRTREQ
          endif

          if (irpn.eq.0) then
            if (hb(1:1).ge.'0' .and. hb(1:1).le.'9') then
              read (hb(1:3),*,err=640,end=640) j
              irpn=i0+j
            elseif (hb(1:1).eq.'V') then
              read (hb(2:3),*,err=640,end=640) j
              irpn=iV0+j
            elseif (hb(1:1).eq.'C') then
              read (hb(2:3),*,err=640,end=640) j
              irpn=iC0+j
            elseif (hb(1:2).eq.'=V') then
              read (hb(3:4),*,err=640,end=640) j
              irpn=iSV0+j
            elseif (hb(1:2).eq.'=C') then
              read (hb(3:4),*,err=640,end=640) j
              irpn=iSC0+j
            elseif (hb(1:2).eq.'JP') then
              read (hb(3:4),*,err=640,end=640) j
              irpn=iJP0+j
            elseif (hb(1:3).eq.'JMP') then
              read (hb(4:5),*,err=640,end=640) j
              irpn=iJMP+j


            elseif (hb(1:3).eq.'POP' .or.
     &              hb(1:4).eq.'EXIT' .or.
     &              hb(1:4).eq.'JUMP') then
              if (hb(1:3).eq.'POP')  irpn=iPOPeq
              if (hb(1:4).eq.'EXIT') irpn=iEXTeq
              if (hb(1:4).eq.'JUMP') irpn=iJMPeq
              hc=hb(5:6)
              if (hb(1:3).eq.'POP') hc=hb(4:5)
              ij=0
              if (hc.eq.'=')  ij=1
              if (hc.eq.'<=') ij=2
              if (hc.eq.'>=') ij=3
              if (hc.eq.'<')  ij=4
              if (hc.eq.'>')  ij=5
              if (hc.eq.'<>') ij=6
              if (ij.le.0) goto 604
              irpn=irpn+ij
            endif
          endif
          if (irpn.eq.0 .or. irpn.gt.3000) goto 605
          i7oper(n,iv,i)=irpn
        enddo
        n7oper(iv,i)=n       !Note: This code is repeated below under line 640 in case an end of file was reached.
        n7st(iv,i)=SUM(n7cf(1:n,iv,i))           !Temporarily store the number of terms in each set.
      enddo

 400  n7=n7total+SUM(n7st(ni1:ni2,i))
      do j=n7total+1,n7
        if (j.ge.maxoptrm) goto 607
        do
          hb=ha
          read (nread,'(a255)',err=420,end=420) ha         !Read next line, skipping commented lines.
          ha=LTRIM(ha)
          if (ha(1:1).ne.'!' .and. ha(1:1).ne.'^') EXIT
          if (ha(1:8) .eq.'^12TERMS')    i12=1
          if (ha(1:11).eq.'^END12TERMS') i12=0
        enddo
c       read (ha,*,err=632,end=632) (cfoper(j,k,i),k=1,4),icfoper(j,i)
c       if (llx) then
c         if (iFlag.eq.-1) then        !Read complex exponential terms (similar to Gaussian-bell shaped terms in FEQ model).
c           read (ha,*,err=410,end=410) (cfoper(j,k,i),k=1,12)
c410        icfoper(j,i)=INT(cfoper(j,5,i))
c         endif
c       endif
        ii=INDEX(ha,'!')
        if (ii.gt.0) ha=ha(:ii-1)
        if (i12.eq.0) then
          read (ha,*,err=632,end=632) (cfoper(j,k,i),k=1,4),icfoper(j,i)
        else
          read (ha,*,err=632,end=632) (cfoper(j,k,i),k=1,12)
          icfoper(j,i)=INT(cfoper(j,5,i))
        endif
      enddo
 420  do j=ni2,ni1+1,-1
        n7st(j,i)=n7total+1+SUM(n7st(ni1:j-1,i))           !Mark the beginning location of the coefficients for each operation set.
      enddo
      n7st(ni1,i)=n7total+1
      n7total=n7
      RETURN

 601  her='Incorrect input for iFlag.'
      goto 691
 602  her='Unknown function:  '//ha(1:3)
      goto 691
 603  her='Array size not large enough for number of commands.'
      goto 691
 604  her='Invalid input for conditional command.'
      goto 691
 605  her='Unknown input:  '//hb
      goto 691
 606  her='Array size not large enough for the input iFlag.'
      goto 691
 607  her='Array size not large enough for the number of coefficients.'
      goto 691
 621  her='Notation missing.'
      goto 691
 630  her='Unknown index.'
      goto 691
 631  her='Stack depleted.'
      goto 691
 632  her='Missing or incorrect entry in coefficients:  '//ha
      n7oper(ni1,i)=0        !Set the number of entries to zero.
      goto 691
 640  her='Premature end of file.'
      if (iFlag.le.0 .or. iv.le.0) goto 691
      n7oper(iv,i)=n
      n7st(iv,i)=SUM(n7cf(1:n,iv,i))   !Temporarily store the number of terms in each set.
      goto 400

 691  call ERRNUM (691,0,her,' ',0d0,0d0,0d0,ierr,herr)


      end                                                function RPNEQN

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                  End file PROP_SUB.FOR
c ======================================================================
