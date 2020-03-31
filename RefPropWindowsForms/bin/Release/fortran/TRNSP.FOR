c  Begin file TRNSP.FOR
c
c  This file contains the transport property routines, including the
c    routines implementing the extended corresponding states (ECS)
c    method for the transport properties.
c
c  Contained here are:
c     subroutine TRNPRP (T,D,z,eta,tcx,ierr,herr)
c     subroutine SETCI (nread,icomp,htype,ierr,herr)
c     subroutine SETTRN (nread,icomp,hreff,heoss,ierr,herr)
c     subroutine TRNECS (T,D,z,eta,tcx,ierr,herr)
c     subroutine TRNEC (m,icomp,T,D,z,eta,tcx,ierr,herr,irefn,T0,D0,amx,zmix,fx,hx)
c     subroutine CONFTD (j,m,amx,zmix,T,D,Tj,Dj,fx,hx,fxx,hxx,fjj0,hjj0,ierr,herr)
c     subroutine GXTDG (m,i,j,Dj,z,xmj,fj,hj,gj,etadg,tcxdg,tcxin,gxsum,tdgMSW)
c     subroutine CHECKTRN(iFlag,icomp,T,D,z,eta,tcx,ierr,ierreta,ierrtcx,ierrecs,herr,herreta,herrtcx,herrecs)
c     function OMEGA (icomp,T,epsk,hmodci)
c     function OMEGAS (il,is,tau)
c     function PSI (icomp,Tr,Dr)
c     function CHI (icomp,Tr,Dr)
c     function FINT (icomp,T)
c
c ======================================================================
c ======================================================================
c
      subroutine TRNPRP (T,D,z,eta,tcx,ierr,herr)
c
c  Compute the transport properties thermal conductivity and
c  viscosity as functions of temperature, density, and composition.
c
c  .. warning::
c
c      Do NOT call this routine for two-phase states,
c      otherwise it will return a metastable state if near the phase boundary
c      or complete nonsense at other conditions.  The value of q that is
c      returned from the flash routines will indicate a two phase state
c      by returning a value between 0 and 1.  In such a situation, the
c      transport properties can only be calculated for the saturated liquid
c      and vapor states.  For example, when calling PHFLSH::
c
c          call PHFLSH (P,h,z,T,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
c
c      If q>0 and q<1, then values of the liquid compositions will be returned
c      in the x and y arrays, and the transport properties of the liquid and
c      vapor states can be calculated as follows::
c
c          call TRNPRP (T,Dl,x,etaliq,tcxliq,ierr,herr)
c          call TRNPRP (T,Dv,y,etavap,tcxvap,ierr,herr)
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition array (array of mole fractions)
c
c  Outputs:
c      eta--Viscosity [uPa-s]
c      tcx--Thermal conductivity [W/m-K]
c     ierr--Error flag:  0 - Successful
c                      502 - Unknown viscosity or thermal conductivity model specified
c                       73 - One or more inputs are out of bounds for the ECS model
c                       74 - Inputs to the vis. and th. cond. correlations are out 2of range
c                      540 - Transport equations are not available for one or more of the fluids
c                      541 - Transport equations are not available for mixtures with water at molar concentrations greater than 5%
c                      542 - Transport equations are not available for mixtures with alcohols
c                      543 - Transport equations are not available for the ammonia/water mixture
c                     -508 - Invalid region for viscosity of reference fluid 1
c                     -558 - 2-D Newton-Raphson method for conformal temperature and density did not converge
c                     -560 - Pure fluid is exactly at the critical point; thermal conductivity is infinite
c                      561 - Pure fluid correlation produced an erroneous value for either viscosity or thermal conductivity
c                      5## - One or more inputs to the thermal conductivity correlation are out of range; results may be in error
c                      5## - One or more inputs to the viscosity correlation are out of range; results may be in error
c                      5## - Pure fluid correlation out of range; attempted to use ECS method
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  10-15-96  MM, original version, supercedes older routine of same name
c  05-08-08 MLH, add check for viscosity exactly at critical point
c  03-09-09 EWL, allow water mixtures if interaction parameters are available
c  06-05-12 EWL, increase allowable water to 5%
c  01-24-13 MLH, don't allow transport with alcohol mixtures
c  06-27-16 EWL, rewrite and optimIze all code in TRNSP.FOR

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr,herreta,herrtcx,herrecs
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TRNPRP

      eta=xnotc
      tcx=xnotc
      ierr=0
      if (T.le.0. or. D.lt.0.) RETURN
      call ISPURE (z,icomp)
      call CHECKTRN (0,icomp,T,D,z,eta,tcx,ierr,
     &             ierreta,ierrtcx,ierrecs,herr,herreta,herrtcx,herrecs)
      if (ierr.ge.100) RETURN     !Transport equations are not available.

c  ---Pure fluid correlations---
      if (icomp.ne.0) then
        if (leta) call ETAK (icomp,T,D,eta,ierreta,herreta)     !Call dedicated pure fluid correlation for viscosity.
        if (ltcx) call TCXK (icomp,T,D,tcx,ierrtcx,herrtcx)     !Call dedicated pure fluid correlation for thermal conductivity.
        if (.not.leta .or. .not.ltcx) then
          call TRNECS (T,D,z,etaecs,tcxecs,ierrecs,herrecs)     !Call ECS routines for viscosity and thermal conductivity.
          if (.not.leta) eta=etaecs
          if (.not.ltcx) tcx=tcxecs
        endif

c  ---General mixture routines---
      else
        if (hvismx(1:2).eq.'EC' .or. htcxmx(1:2).eq.'EC') then
          call TRNECS (T,D,z,etaecs,tcxecs,ierrecs,herrecs)     !Call ECS routines for viscosity and thermal conductivity.
          if (hvismx(1:2).eq.'EC') eta=etaecs
          if (htcxmx(1:2).eq.'EC') tcx=tcxecs
        endif
      endif

c  Write out errors.
      call CHECKTRN(1,icomp,T,D,z,eta,tcx,ierr,
     &             ierreta,ierrtcx,ierrecs,herr,herreta,herrtcx,herrecs)

      end                                              subroutine TRNPRP
c
c ======================================================================
c
      function OMEGA (icomp,T,epsk,hmodci)
c
c  Collision integral for a pure fluid as a function of temperature and
c  the Lennard-Jones epsilon/k parameter.
c
c  CI1:
c  Empirical function of a form used by several authors including
c  Fenghour (1995) for ammonia, Krauss (1996) for R152a,
c  Laesecke (1997) for R134a, and Vesovic (1990) for carbon dioxide.
c
c  CI2:
c  Collision integral based on Equation (20) in
c  Younglove and Ely, J. Phys. Chem. Ref. Data, 16:577-798, 1987.
c  There is misprint in Younglove and Ely, the exponent is
c  ((4-n)/3) not ((n+2)/3).
c
c  CI3:
c  Empirical function of a form used by Tariq et al. (2014) for cyclohexane.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c     epsk--Lennard-Jones energy (epsilon/k) parameter [K]
c   hmodci--collision integral type (CI1, CI2, etc.)
c
c  Output (as the function value):
c    OMEGA--The collision integral

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*3 hmodci

      OMEGA=1d0
      if (T.lt.0.) RETURN

      omgsum=0d0
      Tstar=1d-20
      if (hmodci.eq.'CI1') then
        if (ABS(epsk).gt.1d-20) Tstar=XLOG(T/epsk)
      elseif (hmodci.eq.'CI2') then
        ekt3=1d0
        if (T.gt.0.) ekt3=(epsk/T)**thrd
        do n=1,ntomg2(icomp)
          omgsum=omgsum+comg2(icomp,n,1)*ekt3**(4-n)
        enddo
        if (ABS(omgsum).gt.1d-20) OMEGA=1d0/omgsum
      elseif (hmodci.eq.'CI3') then
        if (ABS(epsk).gt.1d-20) Tstar=T/epsk
      elseif (hmodci.eq.'CI0') then
        if (ABS(epsk).gt.1d-20) OMEGA=OMEGAS(2,2,T/epsk)
      else
        OMEGA=1d0       !Return 1, as omega appears in denominator.
      endif

      if (hmodci.eq.'CI1' .or. hmodci.eq.'CI3') then
        if (ABS(Tstar).lt.1d-20) Tstar=1d-20
        do n=1,ntomg(icomp)
          omgsum=omgsum+comg(icomp,n,1)*Tstar**icomg(icomp,n)
        enddo
        OMEGA=EXP3(omgsum)
      endif

      end                                                 function OMEGA
c
c ======================================================================
c
      function OMEGAS (il,is,tau)
c
c  Collision integral for a Lennard-Jones fluid.
c  Returns value for Omega_1,1 or Omega_2,2; based on
c  Neufeld, Janzen, and Aziz, J. Chem. Phys., 57:1100-1102, 1972.
c
c  Originally implemented in MIPROPS/SUPERTRAPP by J.F. Ely,
c  NIST Thermophysics Division, Boulder, Colorado
c
c  Inputs:
c    [Only inputs of (il = is = 1) and (il = is = 2) are valid.]
c       il--Order of integral
c       is--Order of integral
c      tau--Dimensionless temperature = T/epsktr, where epsktr is the Lennard-
c           Jones energy parameter (epsilon/k)
c
c  Output (as the function value):
c   OMEGAS--Value of the collision integral

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      OMEGAS=0d0
      if (tau.lt.1d-20) RETURN
      if (il.eq.1 .and. is.eq.1) then
        OMEGAS=1.06036d0/tau**0.15610d0+
     &         0.19300d0*EXP3(-0.47635d0*tau)+
     &         1.03587d0*EXP3(-1.52996d0*tau)+
     &         1.76474d0*EXP3(-3.89411d0*tau)
      else
        OMEGAS=1.16145d0/tau**0.14874d0+
     &         0.52487d0*EXP3(-0.77320d0*tau)+
     &         2.16178d0*EXP3(-2.43787d0*tau)
c    &         6.435d-4*SIN(18.0323d0*tau**(-0.7683d0)-7.27371d0)
      endif
      end                                                function OMEGAS
c
c ======================================================================
c
      subroutine SETCI (nread,icomp,htype,ierr,herr)
c
c  Initialize the collision integral.
c
c  CI1:
c  The empirical function in LOG(T), the form used by several
c  authors including Fenghour (1995) for ammonia and Krauss (1996)
c  for R152a; this same form is used for the reduced effective collision
c  cross-section used by Wilhelm and Vogel (1995) and Laesecke (1997) for R134a.
c
c  CI2:
c  The collision integral function of Younglove and Ely,
c  J. Phys. Chem. Ref. Data, 16:577-798, 1987.
c
c  CI3:
c  The collision integral as an empirical function in
c  LOG(eta) as a function of T instead of LOG(T) used by Tariq et al. (2014)
c  for cyclohexane (J. Phys. Chem. Ref. Data, 43:033101, 2014).
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
      character herr*255,htype*3
      ierr=0
      herr=' '
      if (nread.eq.0) RETURN

c  Read data from file (should have been opened by SETUP).
c  Limits are not used with collision integral, but retain for compatibility.
      read (nread,*,err=102)                   !Lower temperature limit (dummy)
      read (nread,*,err=102)                   !Upper temperature limit (dummy)
      read (nread,*,err=102)                   !Upper pressure limit (n/a)
      read (nread,*,err=102)                   !Upper density limit (n/a)

      if (htype.eq.'CI2') then
        read (nread,*,err=102) ntomg2(icomp)   !Number of terms
        if (ntomg2(icomp).ge.1) then
          do j=1,ntomg2(icomp)
            read (nread,*,err=102) comg2(icomp,j,1)
            comg2(icomp,j,2)=0d0       !The second element in coefficient array is not used for this model.
          enddo
        endif

      else
        read (nread,*,err=102) ntomg(icomp)    !Number of terms
        if (ntomg(icomp).ge.1) then
          do j=1,ntomg(icomp)
            read (nread,*,err=102) comg(icomp,j,1),icomg(icomp,j)       !First is numerical coefficient, second is power of T.
          enddo
        endif
      endif
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine SETCI
c
c ======================================================================
c
      subroutine SETTRN (nread,icomp,hreff,heoss,ierr,herr)
c
c  Set up working arrays for the ECS transport property model.
c
c  Inputs:
c    nread--File number containing data (should have been opened by SETUP)
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c
c  Outputs:
c    hreff--File containing reference fluid EOS (character*255)
c    heoss--Model ('FEQ', etc.) for reference fluid EOS (character*3)
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by S. Klein, NIST Thermophysics Division, Boulder, Colorado
c  12-14-95 SAK, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*3 heoss,hvs,htc,hbig
      character*255 hreff,herr,ha,LTRIM

      ierr=0
      herr=' '
      if (nread.eq.0) RETURN

c  Read data from file.
      hbig='NBS'
      fcecs(icomp)=1d0
      htcxcrecs(icomp)=' '
      read (nread,*,err=102) Tmecst(icomp)     !Lower temperature limit
      read (nread,*,err=102) Txecst(icomp)     !Upper temperature limit
      read (nread,*,err=102) Pxecst(icomp)     !Upper pressure limit
      read (nread,*,err=102) Dxecst(icomp)     !Upper density limit
      read (nread,'(a255)',err=102) ha
      ha=LTRIM(ha)
      read (ha,2083,err=102) heoss,hreff       !Reference fluid EOS and *.fld file
      call READHA (nread,3,hvs)        !Reference fluid viscosity model
      call READHA (nread,3,htc)        !Reference fluid conductivity model
      if (VerFL(icomp).ge.10.) call READHA (nread,3,hbig)
      if (hbig.ne.'NBS' .and. hbig.ne.'NUL')
     &    read (nread,*,err=102) fcecs(icomp)       !Big molecule modifier
      read (nread,*,err=102) iLJflag(icomp)    !Lennard-Jones flag
      read (nread,*,err=102) sigtrn(icomp)     !Lennard-Jones Sigma
      read (nread,*,err=102) epsktr(icomp)     !Lennard-Jones Epsilon
      nref=nr                          !Used to trick compiler when nr = 0 or 1
      if (mxref.gt.0) hetak(nref)=hvs  !hvs - model ('VS1', etc.) for reference fluid viscosity
      if (mxref.gt.0) htcxk(nref)=htc  !htc - model ('TC1', etc.) for reference fluid conductivity
      if (icomp.le.ncmax) hetak(ncmax+icomp)=hvs      !hvs - model ('VS1', etc.) for reference fluid viscosity
      if (icomp.le.ncmax) htcxk(ncmax+icomp)=htc      !htc - model ('TC1', etc.) for reference fluid conductivity

c  Read terms for f_int in Eucken correlation.
      nEuck(icomp)=0
      if (VerFL(icomp).ge.6.0009d0) then
        read (nread,*,err=102) nEuck(icomp)
        if (nEuck(icomp).ge.1) then
          do j=1,nEuck(icomp)
            read (nread,*,err=102) (cEuck(icomp,j,k),k=1,4)     !Read polynomial term(s) for f_int.
          enddo
        endif
      endif

c  Read terms for viscosity shape factors.
      read (nread,*,err=102) npsi1(icomp),npsi2(icomp),npsi3(icomp)
      if (npsi1(icomp)+npsi2(icomp).ge.1) then
        do j=1,npsi1(icomp)+npsi2(icomp)
          read (nread,*,err=102) (cpsi(icomp,j,k),k=1,4)        !Read coefficient, power of Tr, power of Dr.
        enddo
      endif

c  Read terms for thermal conductivity shape factors.
      read (nread,*,err=102) nchi1(icomp),nchi2(icomp),nchi3(icomp)
      if (nchi1(icomp)+nchi2(icomp).ge.1) then
        do j=1,nchi1(icomp)+nchi2(icomp)
          read (nread,*,err=102) (cchi(icomp,j,k),k=1,4)        !Read coefficient, power of Tr, power of Dr, spare.
        enddo
      endif
      call READHA (nread,3,htcxcrecs(icomp))     !Read pointer to enhancement model for ECS.
      RETURN

 102  call ERRNUM (102,icomp,hname(icomp),' ',0d0,0d0,0d0,ierr,herr)
 2083 format (a3,1x,a255)
      end                                              subroutine SETTRN
c
c ======================================================================
c
      subroutine TRNECS (T,D,z,eta,tcx,ierr,herr)
c
c  Compute the transport properties of thermal conductivity and
c  viscosity as functions of temperature, density, and composition.
c
c  Based on the modification of the Huber-Ely ECS method given by
c  Klein, McLinden, and Laesecke, Int. J. Refrig., 20:208-217, 1997.
c
c  Equation numbers below refer to this paper.
c  Factor of (ref fluid mol wt)**-0.5 is missing from Eq 31 in paper.
c  In Eq. 34 the reference fluid should be evaluated at (T0,D0),
c  not at (T/fj,D*hj).
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition array (array of mole fractions)
c
c  Outputs:
c      eta--Viscosity [uPa-s]
c      tcx--Thermal conductivity [W/m-K]
c     ierr--Error flag:  0 - Successful
c                        1 - Inputs are out of bounds
c                       73 - One or more inputs are out of bounds
c                     -508 - Ref. fluid viscosity correlation in invalid region
c                     -558 - ECS model did not converge
c                     -560 - Pure fluid is exactly at critical point; t.c. is infinite
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  NIST Thermophysics Division, Boulder, Colorado
c  based on the routine by S.A. Klein (in turn, based on Refprop5 routine)
c  03-05-97  MM, original version
c  11-30-06 MLH, remove discontinuity for size diff effects
c  12-24-06 MLH, add binary int parameters kij, lij for residual viscosity; deactivate hsdel
c  01-25-07 MLH, allow constant in Mason and Saxena mixing rule to vary depending on fluid system
c  01-28-07 MLH, allow binary interaction parameter for residual thermal conductivity
c  02-13-08 MLH, adjustment of region to calculate shape factors to fix problems in supercritical region of jp900
c  05-27-08 MLH, add one more binary int par for dilute gas viscosity xecsdij in trnbin
c  06-11-08 MLH, add second bin int for dilute gas vis xecsdij2
c  05-24-13 MLH, add estimated values to use when critp fails so that D0 is not zero
c  05-21-13 MLH, fixed index in Wasiljewa mix rule, partially fixed bug in mix tcx0 for hard coded models
c  05-30-13 MLH, damp contributions of gj in vapor phase for h2, h3, parah2 and deuterium
c  06-19-13 MLH, do not apply full gj factor to mixes with h2, he, parah2, and deuterium at dilute D
c  03-07-14 MLH, fixed bug for dilute gas in hard-coded thermal conductivity models TC0 (ethylene,he,R23,D20)
c  03-18-14 MLH, adapted mix rule fix for old versions with interaction parameters
c  03-08-16 MLH, change call to CRITP to CRTPRX for stability between versions
c  07-21-16 EWL, delete subroutine TCBKMX and incorporate here

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr,herrs,herret,herrsv,herr1
      dimension z(ncmax)
      dimension fj(ncmax),hj(ncmax)    !Array of temperature and density shape factors for the components
      dimension tcxin(nx),tcxdg(nx)    !Internal and dilute-gas parts
      dimension xmj(ncmax)             !Equivalent molar mass (Eq 33)
      dimension etadg(ncmax)           !Dilute gas viscosity

      call ISPURE (z,icomp)
      iErrNum=iErrNum+1
      ierr=0
      herr=' '

      eta=xnotc    !Set viscosity and t.c. to flags indicating 'not calculated' so that some
      tcx=xnotc    ! value is returned to the GUI in event of failure of routines.
      if (T.lt.0. .or. D.lt.0.) goto 500
      ierrs=0
      ierret=0
      ierrsv=0
      fj=0d0
      hj=0d0
      xmj=0d0
      i=icomp
      irefn=nr     !Reference fluid number.
      if (icomp.ne.0 .and. nc.gt.1) irefn=ncmax+i     !Pure fluid, but called as a mixture.
      if (irefn.gt.nq) goto 500
      if (wmas(irefn).eq.0d0) goto 512 !Use wmas=0 as a check to see if the pure fluid EOS was loaded.
      if (.not.leta) then
        xeta0 (1:nc)=0d0
        xetaB2(1:nc)=0d0
        xetar (1:nc)=0d0
        xetac (1:nc)=0d0
      endif
      if (.not.ltcx) then
        xtcx0 (1:nc)=0d0
        xtcxr (1:nc)=0d0
        xtcxc (1:nc)=0d0
      endif

c  Solve for the conformal state temperature and density.
      call TRNEC (1,i,T,D,z,etaj,tcxj,ierrsv1,herrsv,irefn,
     &            T0,D0,amx,zmix,fx,hx)
      if (ABS(T0).gt.1d6 ) ierrsv=ierrsv1
      if (ABS(T0).lt.1d-2 .or. ABS(T0).gt.1.d50) goto 558
      if (ABS(fx).gt.1.d50) goto 558

c  For a pure fluid, component f and h are same as mixture values.
      if (i.ne.0) then
        fj(i)=fx
        hj(i)=hx
        tpsi=T/Tcrit(i)
        Dpsi=D/Dcrit(i)
        D0=D0*PSI(i,tpsi,Dpsi)    !Eq 21

c  For a mixture, find conformal T and D for each of the components.
      else
        call REDLINEAR (1,z,tcmxec,pcmxec,dcmxec,ierr,herr)
        do j=1,nc
          if (z(j).gt.0.) then
          fjj0=fx*Tcrit(j)/tcmxec
          hjj0=hx*dcmxec/Dcrit(j)
          Tj=T*Tcrit(j)/tcmxec    !Initial guess for conformal temperature.
          Dj=D*Dcrit(j)/dcmxec    !Initial guess for conformal density.
          call CONFTD (j,1,amx,zmix,T,D,Tj,Dj,fj(j),hj(j),fx,hx,
     &                 fjj0,hjj0,ierr,herr)
          if (Tj.lt.0.1d0) goto 558
c  Override for supercritical region; linearly interpolate to value at 1000 K
c   to avoid discontinuity at tcmxec.
          if (zmix.gt.0.3d0) then           !Gas-like
            if (T.gt.tcmxec) then           !Supercritical
              fj(j)=fjj0+(T-1000d0)*(fjj0-fj(j))/(1000d0-tcmxec)
              hj(j)=hjj0+(T-1000d0)*(hjj0-hj(j))/(1000d0-tcmxec)
              if (T.gt.1000.) fj(j)=fjj0
              if (T.gt.1000.) hj(j)=hjj0
              ierr=0
              ierrsv=0
            endif
          endif
          endif
        enddo
      endif

c  Estimate the Lennard-Jones parameters, if necessary.
      do j=1,nc
        if (z(j).gt.0. .or. j.eq.i) then
          if (iLJflag(j).eq.0) then
            epsktr(j)=Tcrit(j)/Tcrit(irefn)*epsktr(irefn)
            sigtrn(j)=sigtrn(irefn)*(Dcrit(irefn)/Dcrit(j))**thrd
          elseif (iLJflag(j).eq.2) then
            epsktr(j)=epsktr(irefn)*fj(j)   !Estimation method of Huber and Ely, Fluid Phase Equilib. 80:239-248, 1992.
            sigtrn(j)=sigtrn(irefn)*hj(j)**thrd
          endif
        endif
      enddo

c  Calculate background viscosity of either pure fluid or reference fluid.

      if (lNan) call CheckNaN (t0)
      call ETAKB (irefn,T0,D0,eta0bk,ierret,herret)
      if (eta0bk.le.-1000.) then
        if (ierret.eq.0)
     &    call ERRNUM (-508,0,' ',' ',0d0,0d0,0d0,ierret,herret)     !In two-phase, solid, or other invalid region.
        eta0bk=0d0
      endif

c  Combine viscosity pieces for a pure fluid.
      if (i.ne.0) then
        gx=wmas(i)/wmas(irefn)
        Feta=SQRT(fj(i)*gx)/hj(i)**thrd2    !Eq 11
        etadg1=ETA1DG(i,0,0,T)              !Dilute gas viscosity
        eta=etadg1+eta0bk*Feta              !Eqs 5 and 10
        etacal=eta                          !For transmitting to critical enhancement routine TCXM1C in block critenh.
        if (.not.leta) then
          xeta0 (i)=etadg1
          xetaB2(i)=0d0
          xetar (i)=eta0bk*Feta
          xetac (i)=0d0
        endif

c  Calculate thermal conductivity for a pure fluid.
        D0=D*hj(i)*CHI(i,tpsi,Dpsi)
        call TCXKB (irefn,T0,D0,tcx0bk,ierrs,herrs)
        Flam=SQRT(fj(i)/gx)/hj(i)**thrd2                        !F-factor for t.c. has inverse power of gx as compared to viscosity.
        tcxxdg=15d-3*xRgas*etadg1/4d0/wmas(i)                   !Find dilute-gas parts from collisions and internal degrees of freedom.
        tcxINT=FINT(i,T)*etadg1/wmas(i)*(CP0K(i,T)-2.5d0*xRgas) !Apply ECS method to internal part of the fluid of interest.
        tcx=tcxxdg+tcxINT+tcx0bk*Flam
        if (.not.ltcx) xtcx0(i)=tcxxdg+tcxINT
        if (.not.ltcx) xtcxr(i)=tcx0bk*Flam

c  General (mixture) case; begin with mixture viscosity calculation.

c  Calculate equivalent mass gx with the pure fluid residual viscosities,
c   either from a pure fluid correlation or the pure fluid ECS method.
c   Find reference fluid residual viscosity.
      else
        do j=1,nc
          if (z(j).gt.0.) then
            ierrs=0

c  Use pure fluid correlation if available.
            if (hetak(j)(1:2).ne.'EC') then
              if (fx.le.0. .or. hj(j).le.0.) goto 558
              Tj=T*fj(j)/fx  !Eq 36
              Dj=D*hx/hj(j)  !Eq 37
              P=0d0
              call LIMITK('ETA',j,Tj,Dj,P,tmn,tmx,Dmx,pmx,ierr1,herr1)    !Check that pure fluid correlation is within its limits.
              if (Tj.lt.tmn) Tj=tmn                                       !If it isn't, continue calculations but set error flags.
              if (Dj.gt.Dmx) Dj=Dmx
              call ETAKB (j,Tj,Dj,etaj,ierrs,herrs)
              if (ierrs.gt.0 .or. ierret.eq.0) then
                ierret=ierrs
                herret=herrs
              endif

c  Otherwise must use ECS method to estimate.
            else
              tpsi=XDIV(T*fj(j),fx*Tcrit(j))
              Dpsi=XDIV(D*hx,hj(j)*Dcrit(j))
              D0j=D*hx*PSI(j,tpsi,Dpsi)                    !Eq 39
              if (T0.lt.Tmecst(j))      T0=Tmecst(j)
              if (T0.lt.Tmecst(irefn))  T0=Tmecst(irefn)   !Don't evaluate at T less than limits or D > limits.
              if (D0j.gt.Dxecst(j))     D0j=Dxecst(j)
              if (D0j.gt.Dxecst(irefn)) D0j=Dxecst(irefn)
              call ETAKB (irefn,T0,D0j,etarfj,ierrs,herrs)
              if (ierrs.gt.0 .or. ierret.eq.0) then
                ierret=ierrs
                herret=herrs
              endif

c  Compute the background viscosity of a pure fluid component in a mixture
c    with ECS, and evaluate with the individual fluid's reference
c    fluid, which may differ from that of the mixture.
c    Based on the modification of the Huber-Ely ECS method given by
c    Klein, McLinden, and Laesecke, Int. J. Refrig., 20:208-217, 1997.
              if (fx.le.0. .or. hj(j).le.0.) goto 558
              Tj=T*fj(j)/fx
              Dj=D*hx/hj(j)
              call TRNEC (0,j,Tj,Dj,z,etaj,tcxj,ierr,herr,ncmax+j,
     &                    T0,D0,amx,zmix,fx2,hx2)
            endif
            if (ABS(eta0bk).lt.1d-20 .and. D.gt.1.d-4) goto 558
            if (etaj.lt.1d-12)  etaj=1d-12                 !It is possible for residual viscosity to go through zero.
            if (eta0bk.lt.1d-6) eta0bk=1d-6
            gj=etaj/eta0bk/SQRT(fj(j))*hj(j)**thrd2        !Eq 35
            call GXTDG (3,irefn,j,Dj,z,xmj,fj,hj,gj,etadg,tcxdg,tcxin,
     &                  gxsum,zj)
          endif
        enddo
        call GXTDG(1,0,j,Dj,z,xmj,fj,hj,gj,etadg,tcxdg,tcxin,gxsum,zj)    !Eq 30 + 31 reduce to the following expression.
        Feta=gxsum/hx**2/SQRT(wmas(irefn))                                !Factor of 1/SQRT(reference fluid mol wt) is missing from Klein paper.
        etaxdg=ETAMIX(T,z)                  !Dilute gas viscosity of mixture.
        if (ABS(etaxdg).lt.1d-20) goto 558
        eta=etaxdg+eta0bk*Feta              !Eq 23
        etacal=eta                          !For transmitting to critical enhancement routine TCXM1C in block critenh.

c  Compute the background thermal conductivity of a mixture, which
c    is composed of the internal, dilute-gas, and residual
c    contributions (but not the critical enhancement).
c  Based on the modification of the Huber-Ely ECS method given by
c    Klein, McLinden, and Laesecke, Int. J. Refrig., 20:208-217, 1997.
        T0=T/fx                             !Conformal temperature and density for mixture.
        D0=D*hx
        call TCXKB (irefn,T0,D0,tcx0bk,ierr,herr)     !Reference fluid thermal conductivity at conformal conditions.
c  Check for very small value of tcx0bk. It can go through zero and
c   cause an asymptote in the mixing rule for gxsum (division by zero).
        if (tcx0bk.lt.1d-16) tcx0bk=1d-16

c  Calculate equivalent mass gx from pure fluid residual values,
c   either from a pure fluid correlation or from the pure fluid ECS method.
        do j=1,nc
          if (z(j).gt.0.) then
          if (fx.le.0. .or. hj(j).le.0.) goto 558
          Tj=T*fj(j)/fx
          Dj=D*hx/hj(j)
          etadg(j)=ETA1DG(j,0,0,T)                    !Dilute gas viscosity of component j from a generalized dilute-gas function.
          tcxin(j)=FINT(j,T)*etadg(j)/wmas(j)*(CP0K(j,T)-2.5d0*xRgas)     !This correlation also includes an internal contribution. Calculate this separately since a different mixing rule is used.

c  Pure fluid correlation.
          if (htcxk(j)(1:2).ne.'EC') then
            call TCXK0 (j,T,tcx0,ierr,herr)
            tcxdg(j)=tcx0-tcxin(j)
            call TCXKB (j,Tj,Dj,tcxj,ierr,herr)                      !Apply ECS method to background (residual) part of t.c.

c  Dilute-gas and internal contributions for component j.
          else
            tcxdg(j)=1d-3*15d0*xRgas*etadg(j)/(4d0*wmas(j))
            call TRNEC (0,j,Tj,Dj,z,etaj,tcxj,ierr,herr,ncmax+j,     !Get ECS value for pure fluid j with respect to its own reference fluid.
     &                  T0,D0,amx,zmix,fx2,hx2)                      !Not necessarily the same as the mixture; only residual part needed.
          endif
          if (tcxj.lt.1d-16) tcxj=1d-16                              !Check for very small value of tcxj, which can go through zero.
          gj=tcx0bk/tcxj*SQRT(fj(j))/hj(j)**thrd2
          call GXTDG (3,irefn,j,Dj,z,xmj,fj,hj,gj,etadg,tcxdg,tcxin,
     &                gxsum,zj)
          endif
        enddo
        call GXTDG(2,0,j,Dj,z,xmj,fj,hj,gj,etadg,tcxdg,tcxin,gxsum,
     &             tdgMSW)
        Flam=gxsum/hx**2*SQRT(wmas(irefn))  !Alternative interpretation from Ely and Hanley Tech Note (1981).
        tcx=tdgMSW+tcx0bk*Flam
      endif

c  End mixture case.

c  Calculate the thermal conductivity critical enhancement contribution.
      tcxcrit=TCXM1C(T,D,z,ierr,herr)
c  If model used for t.c. enhancement was other than 'NUL','TK3', or 'TK6'
c   force mixture continuity as z->1.
c   This is a linear interpolation used close to pure fluid limit.
      if (i.eq.0) then
        do j=1,nc
          if (htcxcr(j).eq.'NUL') then      !Use existing mixture models for TK3, TK6, and NUL.
          elseif (htcxcr(j).eq.'TK3' .or. htcxcr(j).eq.'TK6' .or.
     &            htcxcr(j).eq.'TK7') then
          else
c  A pure fluid correlation is available for component j but it is not in the
c   form of a 'TK3' or 'TK6' model.  Use its contribution to force mixture
c   calculation as z->1 to be continuous with pure fluid.
c   Get correlation value at pure fluid limit and blend linearly.
            call TCXKC (j,T,D,tcxjc,ierrs,herrs)
            if (ierrs.le.0)
     &        tcxcrit=100d0*((z(j)-0.99d0)*tcxjc+(1d0-z(j))*tcxcrit) !Blended value.
          endif
        enddo
      endif

c  Add the thermal conductivity enhancement term to the background term.
c  Limit the enhancement for mixtures to 100% of the background contribution.
      if (tcxcrit.gt.tcx) tcxcrit=tcx
      if (.not.ltcx .and. i.gt.0) xtcxc(i)=tcxcrit
      tcx=tcx+tcxcrit
      if (ierr.eq.-560) tcx=xnotc !Don't give a value if exactly at pure fluid critical point, k is infinite.
      if (ierret.ne.0) then  !Process warnings/errors for problems with calls to etakb.
        ierr=ierret          !etakb failed.
        herr=herret
        eta=xnotc            !Values used by GUI as non-convergence flag.
      endif
      if (ierrsv.ne.0) then  !Process warnings/errors for problems with CRITP routine.
        ierr=ierrsv          !CONFTD failed.
        herr=herrsv
        eta=xnotc            !Values used by GUI as non-convergence flag.
        tcx=xnotc
      endif
      goto 500

 512  call ERRNUM (512,0,'TRNECS',' ',0d0,0d0,0d0,ierr,herr)
      goto 510
 558  call ERRNUM (-558,0,'TRNECS',' ',0d0,0d0,0d0,ierr,herr)
 500  if (i.ne.0) then            !If a pure fluid, check limits, but send back calculated values.
        if (T.lt.Tmecst(i) .or. T.gt.Txecst(i)*1.5d0 .or.
     &      D.gt.Dxecst(i)) then
          call ERRNUM (73,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        endif
      endif
 510  iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)

      end                                              subroutine TRNECS
c
c ======================================================================
c
      subroutine TRNEC (m,icomp,T,D,z,eta,tcx,ierr,herr,irefn,
     &                  T0,D0,amx,zmix,fx,hx)
c
c  Compute the excess contribution in the thermal conductivity and
c  viscosity as functions of temperature and density for the pure
c  fluids in a mixture with their individual reference fluids.
c
c  Based on the modification of the Huber-Ely ECS method given by
c  Klein, McLinden, and Laesecke, Int. J. Refrig., 20:208-217, 1997.
c  See notes in TRNECS about errors in the equations in the paper.
c
c  Inputs:
c    irefn--Reference fluid number
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        m--Flag for doing setup only (m=1) for a mixture (which requires the same code as that for a pure fluid).
c
c  Outputs:
c      eta--Viscosity [uPa-s]
c      tcx--Thermal conductivity [W/m-K]
c     ierr--Error flag:  0 - Successful
c                     -558 - ECS model did not converge
c     herr--Error string (character*255)
c
c  Written by M.L. Huber, NIST Thermophysics Division, Boulder, Colorado
c  11-11-06 MLH, initial version
c  03-08-16 MLH, crit P calls change to PR version for stability btween versions

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr,herrs
      dimension fj(ncmax),hj(ncmax),zx(ncmax),z(ncmax)

      ierr=0
      herr=' '
      i=icomp
      zx(1:nc)=0d0
      if (i.gt.0) zx(i)=1d0
      if (m.eq.1) zx(1:nc)=z(1:nc)

c  Find amx, zmix, and conformal T and D for reference fluid.
      call REDLINEAR (1,zx,tcmxec,pcmxec,dcmxec,ierr,herr)
      call REDX (zx,Tred,Dred)
      tau=Tred/T
      del=D/Dred
      amx=PHIX(0,0,tau,del,zx)
      zmix=1d0+PHIX(0,1,tau,del,zx)
      if (ABS(amx).gt.1d10 .or. ABS(zmix).gt.1d10) goto 555
      T0=T*Tcrit(irefn)/tcmxec    !Initial guess for conformal temperature.
      D0=D*Dcrit(irefn)/dcmxec    !Initial guess for conformal density.
      fxx0=tcmxec/Tcrit(irefn)    !Default values in case of convergence failure.
      hxx0=Dcrit(irefn)/dcmxec

c  Find "exact" conformal T and D only if density is significant.
c   At zero density, use the initial guesses above (CONFTD can fail at
c   very low density, and the dilute-gas contribution is dominant anyway).
      call CONFTD (irefn,0,amx,zmix,T,D,T0,D0,fx,hx,fxx,hxx,
     &             fxx0,hxx0,ierr,herr)
      if (ABS(fx).gt.1.d20) RETURN
      if (m.eq.1) RETURN

c  For component i in a mixture, find conformal T and D for the component.
      fjj0=fx*Tcrit(i)/tcmxec
      hjj0=hx*dcmxec/Dcrit(i)
      Tj=T*Tcrit(i)/tcmxec        !Initial guess for conformal temperature
      Dj=D*Dcrit(i)/dcmxec        !Initial guess for conformal density
c  Find "exact" conformal T and D only if density is significant.
      call CONFTD (i,1,amx,zmix,T,D,Tj,Dj,fj(i),hj(i),fx,hx,
     &             fjj0,hjj0,ierrs,herrs)
      if (ierr.eq.0) then
        ierr=ierrs
        herr=herrs
      endif

      tt=300d0
      call LIMITK('ETA',irefn,tt,0d0,0d0,tminv,t1,Deta,P1,ierrs,herrs)    !Find correlation limits for the reference fluid.
      if (ierr.eq.0) then
        ierr=ierrs
        herr=herrs
      endif
      gxj=wmas(i)/xEQ0(wmas(ncmax+i))
      Flam=0d0
      Feta=0d0
      if (lNE0(hj(i)) .and. lNE0(gxj)) then
        Flam=XSQRT(fj(i)/gxj)/hj(i)**thrd2
        Feta=XSQRT(fj(i)*gxj)/hj(i)**thrd2
      endif
      if (lNE0(fj(i))) tpsi=T/fj(i)
      if (lNE0(hj(i))) Dpsi=D*hx/(hj(i)*Dcrit(i))
      D0j=D*hx*CHI(i,tpsi,Dpsi)
      call TCXKB (i+ncmax,tpsi,D0j,tcx0bk,ierrs,herrs)
      if (ierr.eq.0) then
        ierr=ierrs
        herr=herrs
      endif
      D0j=D*hx*PSI(i,tpsi,Dpsi)
      if (tpsi.lt.tminv) tpsi=tminv    !Do not let reference fluid for viscosity be evaluated beyond its limits
      if (D0j.gt.Deta) D0j=Deta        ! since the correlations generally do not extrapolate well.
      call ETAKB (ncmax+i,tpsi,D0j,eta0bk,ierrs,herrs)
      if (ierr.eq.0) then
        ierr=ierrs
        herr=herrs
      endif
      tcx=flam*tcx0bk
      eta=Feta*eta0bk
      RETURN

 555  ierr=555

      end                                               subroutine TRNEC
c
c ======================================================================
c
      subroutine CONFTD (j,m,amx,zmix,T,D,Tj,Dj,fx,hx,fxx,hxx,
     &                   fjj0,hjj0,ierr,herr)
c
c  Find the conformal temperature and density for component j (which
c  might be the reference fluid) that gives a reduced residual
c  Helmholtz energy and compressibility that matches the input value.
c  The system of equations to be solved is thus
c     aj(Tj,Dj) -  amx = 0
c     Zj(Tj,Dj) - zmix = 0
c
c  These are put into dimensionless temperature and density tau and del
c  and linearized to the form
c     (da/dtau)*deltau + (da/ddel)*deldel =  amx - aj(Tj,Dj)
c     (dZ/dtau)*deltau + (dZ/ddel)*deldel = zmix - Zj(Tj,Dj)
c  to allow a solution by the classic Newton-Raphson method in two dimensions for
c     a11*x1 + a12*x2 = fx1
c     a21*x1 + a22*x2 = fx2
c
c  Inputs:
c        j--Component number
c           j = 0 - Find conformal conditions for reference fluid
c           j > 0 - Find conformal conditions for component j
c      amx--Reduced residual Helmholtz energy (A - A*)/RT [-]
c     zmix--Compressibility factor (pV/RT) [-]
c       Tj--Initial guess for conformal temperature for component j [K]
c       Dj--Initial guess for conformal density for component j [mol/L]
c
c  Outputs:
c       Tj--Converged conformal temperature for component j [K]
c       Dj--Converged conformal density for component j [mol/L]
c     ierr--Error flag:  0 - Successful
c                     -558 - Did not converge
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  09-29-97  MM, original version, replaces MINH by S.A. Klein

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr

      ierr=0
      herr=' '
      itmax=200
      tolf=1d-7                   !Convergence tolerance for function evaluations.
      tolx=1d-12                  !Tolerance for step in independent variables.
      if (T.lt.1. .or. D.lt.0.) goto 500

      if (zmix.gt.0.3d0) then     !Do not find "exact" conformal T and D if density is insignificant (vapor phase).
        plim=zmix*xRgas*Tj*Dj
        if (plim.lt.1.1d0*Pcrit(j) .and. Dj.lt.Dcrit(j)) then
          fx=fjj0                 !Temperature shape factor.
          hx=hjj0                 !Density shape factor for the mixture.
          goto 500
        endif
      endif

      tjmax=1.1d0*Txecst(j)       !Set maximum acceptable temperature.  Allow small extrapolation above max T.
      tredj=Tzred(j)              !Reducing ratios for component j.
      Dredj=Dzred(j)
      x1=tredj/Tj                 !tauj (dimensionless temperature).
      x2=Dj/Dredj                 !delj (dimensionless density).

c  Begin iteration
      do 800 it=0,itmax
        phi00=PHIK(j,0,0,x1,x2)   !Check for reasonableness of guess by means of derivative dP/dD.
        phi01=PHIK(j,0,1,x1,x2)   ![Actually, the quantity calculated is (1/RT)*(dP/dD)].
        phi02=PHIK(j,0,2,x1,x2)
        dPdD=1d0+2d0*phi01+phi02
        if (dPdD.lt.0.) then
          if (x2*Dredj.gt.Dcrit(j)) then
            x2=1.04d0*x2          !Liquid-phase, go to higher density.
          else
            x1=0.96d0*x1          !Vapor-phase, go to higher temperature (lower tau).
          endif
          goto 800                !Use up one iteration (avoid infinite loop).
        endif
c  Evaluate objective function.
        fx1=amx-phi00
        fx2=zmix-1d0-phi01
        error1=ABS(fx1)+ABS(fx2)  !Check for convergence.
        if (error1.lt.tolf) goto 840   !Iteration has converged.

c  Calculate remaining derivatives and new guess for independent variables.
        phi10=PHIK(j,1,0,x1,x2)
        phi11=PHIK(j,1,1,x1,x2)
        if (lEQ0(x1) .or. lEQ0(x2)) goto 558
        a11=phi10/x1         !partial a w.r.t. tau
        a12=phi01/x2         !partial a w.r.t. del
        a21=phi11/x1         !partial Z w.r.t. tau
        a22=(phi02+phi01)/x2 !partial Z w.r.t. del
        denom=a11*a22-a21*a12
        if (ABS(denom).lt.1d-16) then
          delx1= 0.05d0*x1   !System has singularity, no solution for this guess, try another.
          delx2=-0.05d0*x2
        else
          delx1=(a22*fx1-a12*fx2)/denom
          delx2=(a11*fx2-a21*fx1)/denom
        endif
        sumdel=ABS(delx1)+ABS(delx2)
        x1=x1+SIGN(1d0,delx1)*MIN(ABS(delx1),0.20d0*x1)    !Do not allow too great a step (x1,x2 should always be positive).
        x2=x2+SIGN(1d0,delx2)*MIN(ABS(delx2),0.20d0*x2)
        if (sumdel.lt.tolx) goto 840                       !If step is within tolerance, iteration finished.
 800  continue

c  Iteration loop has not converged.
      call ERRNUM (-558,j,'CONFTD',' ',0d0,0d0,0d0,ierr,herr)

c  Iteration has converged, assign outputs.
 840  Tj=tredj/x1
      Dj=x2*Dredj
      if (Tj.gt.2d0*tjmax)                  !Check to make sure converged solution is reasonable.
     &  call ERRNUM (-558,j,'CONFTD',' ',0d0,0d0,0d0,ierr,herr)

      if (m.eq.0) then
        fx=T/Tj                             !Reference fluid.
        hx=Dj/D
      else
        fx=fxx*Tj/T
        hx=hxx*D/Dj
      endif

c  Check to make sure values are reasonable for this region.
 500  if (ierr.ne.0) then
        if (ABS(fjj0).lt.1.d-10 .or. ABS(hjj0).lt.1.d-10) goto 558
        if (ABS((fx-fjj0)/fjj0).gt.0.1d0) goto 558
        if (ABS((hx-hjj0)/hjj0).gt.0.1d0 .or. Tj.lt.1.) goto 558     !Allow nonconvergence to small deviations.
      endif
      TrnTj=Tj                              !Save for use in the ALLPROPS routine.
      TrnDj=Dj
      RETURN

 558  call ERRNUM (-558,j,'CONFTD',' ',0d0,0d0,0d0,ierr,herr)

      end                                              subroutine CONFTD
c
c ======================================================================
c
      subroutine GXTDG (m,i,j,Dj,z,xmj,fj,hj,gj,etadg,tcxdg,tcxin,
     &                  gxsum,tdgMSW)
c
c  Internal routines to the TRNECS subroutine.

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      dimension z(ncmax),fj(ncmax),hj(ncmax),xmj(ncmax)
      dimension etadg(ncmax),tcxdg(ncmax),tcxin(ncmax)

c  Don't use full mass shape factor for He, H2, paraH2, or deuterium at dilute D.
      if (m.eq.3) then
        if (iFluidID(j).eq.10 .or. iFluidID(j).eq.11 .or.            !Damp gj for He, H2, paraH2, and Deuterium at dilute conditions.
     &      iFluidID(j).eq.12 .or. iFluidID(j).eq.14) then
          if (Dj/Dcrit(j).le.1.) then
            damp=(1d0-(1d0-Dj/Dcrit(j))**5.86d0)
            gj=(gj-1d0)*damp+1d0
          endif
        endif
        xmj(j)=gj**2*wmas(i)                !Eq 33
        RETURN
      endif
c
      gxsum=0d0
      tdgMSW=0d0
      do j=1,nc
        if (z(j).gt.0.) then
        Ajisum=0d0
        do k=1,nc
          if (z(k).gt.0.) then
          Aji=1d0
          xmij=xmj(k)
          fij=fj(k)
          hij=hj(j)
          if (k.ne.j) then
            xmij=2d0*xmj(k)*xmj(j)/xEQ0(xmj(k)+xmj(j))
            if (m.eq.1) then
              fij=SQRT(fj(j)*fj(k))*(1d0-xecskij(k,j))                    !Eq 26
             hij=0.125d0*(hj(j)**thrd+hj(k)**thrd)**3*(1d0-xecslij(k,j))  !Eq 27
            else
              fij=SQRT(fj(j)*fj(k))*(1d0-xecskijk(k,j))
              hij=0.125d0*(hj(j)**thrd+hj(k)**thrd)**3*
     &            (1d0-xecslijk(k,j))
c  Mason and Saxena modification of Wassiljewa mixing rule.
c   Allow the constant to change depending on the fluid system.
              w=wmas(k)/wmas(j)
              if (VrsNm.lt.3. .and. ABS(xecsaji(j,k)).gt.1d-3)
     &            w=wmas(j)/wmas(k)         !This mix rule is incorrect, but if you have old interaction parameters you need it to be consistent.
              Aji=(1d0-xecsaji(j,k))/SQRT(8d0*(1d0+wmas(j)/wmas(k)))*
     &            (1d0+SQRT(XDIV(etadg(j),etadg(k))*SQRT(w)))**2
            endif
          endif
          if (m.eq.1) then
            gxsum=gxsum+z(k)*z(j)*SQRT(fij*xmij)*hij**(4d0/3d0)           !Eq 31
          else
            gxsum=gxsum+z(k)*z(j)*SQRT(fij/xEq0(xmij))*hij**(4d0/3d0)
            Ajisum=Ajisum+z(k)*Aji
          endif
          endif
        enddo
        if (Ajisum.ne.0.)tdgMSW=tdgMSW+z(j)*(tcxin(j)+tcxdg(j))/Ajisum
        endif
      enddo
      end                                               subroutine GXTDG
c
c ======================================================================
c
      function PSI (icomp,Tr,Dr)
c
c  Viscosity shape factor for a pure fluid or mixture component, as defined by
c  Klein, McLinden, and Laesecke, Int. J. Refrig., 20:208-217, 1997.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c       Tr--Reduced temperature - T/Tc for a pure fluid
c                               - (T/fx)/(tc_j/fj) for a mixture
c       Dr--Reduced density     - D/Dc for a pure fluid
c                               - D*hx/(hj*Dc_j) for a mixture
c
c  Output (as the function value):
c      PSI--The viscosity shape factor, i.e., an additional factor
c           entering into the definition of the conformal density in
c           the ECS method such that D_0 = D*hj*PSI
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  03-05-97  MM, original version
c
      include 'COMMONS.INC'
      include 'COMTRN.INC'

      i=icomp
      PSI=1d0
      if (npsi1(i).le.0) RETURN             !No transport shape factor is present.

c  A second polynomial term is present; check whether input (T or D)
c   is below the crossover value [k = 1,2 in cpsi(i,npsi1(i)+1,k)].
c   This term allows for a piece-wise fit of PSI.
      if (npsi2(i).ge.2) then
        m=npsi1(i)+1
        if (Tr.lt.cpsi(i,m,1) .or. Dr.lt.cpsi(i,m,2)) then
          m=m+1
          n=npsi1(i)+npsi2(i)
          PSI=SUM(cpsi(i,m:n,1)*Tr**cpsi(i,m:n,2)*Dr**cpsi(i,m:n,3))
          RETURN
        endif
      endif

c  Apply the first polynomial term (either it's the only one present
c   or the 2nd polynomial term does not apply).
      n=npsi1(i)
      if (Dr.gt.0.) then
        PSI=SUM(cpsi(i,1:n,1)*Tr**cpsi(i,1:n,2)*Dr**cpsi(i,1:n,3))
      else
        PSI=SUM(cpsi(i,1:n,1)*Tr**cpsi(i,1:n,2))
      endif

      end                                                   function PSI

c ======================================================================
c
      function CHI (icomp,Tr,Dr)
c
c  Thermal conductivity shape factor for a pure fluid or mixture
c  component (analogous to the viscosity shape factor) defined by
c  Klein, McLinden, and Laesecke, Int. J. Refrig., 20:208-217, 1997.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c       Tr--Reduced temperature - T/Tc for a pure fluid
c                               - (T/fx)/(tc_j/fj) for a mixture
c       Dr--Reduced density     - D/Dc for a pure fluid
c                               - D*hx/(hj*Dc_j) for a mixture
c
c  Output (as the function value):
c      CHI--The thermal conductivity shape factor, i.e., an additional factor
c           entering into the definition of the conformal density in
c           the ECS method such that D_0 = D*hj*psi
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  03-05-97  MM, original version
c
      include 'COMMONS.INC'
      include 'COMTRN.INC'

      i=icomp
      CHI=1d0
      if (nchi1(i).le.0) RETURN             !No transport shape factor is present.

c  A second polynomial term is present; check whether input (T or D)
c   is below the crossover value [k = 1,2 in cchi(i,nchi1(i)+1,k)].
c   This term allows for a piece-wise fit of CHI.
      if (nchi2(i).ge.2) then
        m=nchi1(i)+1
        if (Tr.lt.cchi(i,m,1) .or. Dr.lt.cchi(i,m,2)) then
          m=m+1
          n=nchi1(i)+nchi2(i)
          CHI=SUM(cchi(i,m:n,1)*Tr**cchi(i,m:n,2)*Dr**cchi(i,m:n,3))
          RETURN
        endif
      endif

c  Apply the first polynomial term (either it's the only one present
c   or the 2nd polynomial term does not apply).
      n=nchi1(i)
      if (Dr.gt.0.) then
        CHI=SUM(cchi(i,1:n,1)*Tr**cchi(i,1:n,2)*Dr**cchi(i,1:n,3))
      else
        CHI=SUM(cchi(i,1:n,1)*Tr**cchi(i,1:n,2))
      endif

      end                                                   function CHI
c
c ======================================================================
c
      function FINT (icomp,T)
c
c  Calculate the factor f_int appearing in the Eucken correlation for
c  thermal conductivity.
c
c  Inputs:
c    icomp--Component number in mixture (1..nc); 1 for pure fluid
c        T--Temperature [K]
c
c  Output (as the function value):
c     FINT--F_int factor

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      FINT=1.32d-3
c  If correlation for f_int is present, use value corresponding to
c   modified Eucken correlation.
      n=nEuck(icomp)
      if (n.gt.0) FINT=SUM(cEuck(icomp,1:n,1)*T**cEuck(icomp,1:n,2))
      end                                                  function FINT
c
c ======================================================================
c
      subroutine CHECKTRN(iFlag,icomp,T,D,z,eta,tcx,ierr,
     &             ierreta,ierrtcx,ierrecs,herr,herreta,herrtcx,herrecs)
c
c  Internal routine to the TRNPRP subroutine, which was separated here
c  to make the TRNPRP code easier to read while debugging.

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*255 herr,herreta,herrtcx,herrecs,herr1,herr2
      dimension z(ncmax)
c
      if (iFlag.eq.1) goto 500
c
      if (itrnflag.ne.-777) itrnflag=ierr
      ierr=0
      ierreta=0
      ierrtcx=0
      ierrecs=0
      eta=xnotc
      tcx=xnotc
      leta=.false.
      ltcx=.false.
      herr=' '
      herreta=' '
      herrtcx=' '
      herrecs=' '

c  Make sure that transport equations are available for all fluids.
      if (icomp.ne.0) then
        if (hetak(icomp).eq.'NUL' .or. hetak(icomp).eq.'   ') then
          if (htcxk(icomp).eq.'NUL' .or. htcxk(icomp).eq.'   ') then
            call ERRNUM (540,0,' ',' ',0d0,0d0,0d0,ierr,herr)
            RETURN
          endif
        endif
      else
        do i=1,nc
          if (z(i).gt.0. .or. nc.eq.1) then
            if (hetak(i).eq.'NUL' .or. hetak(i).eq.'   ') then
              if (htcxk(i).eq.'NUL' .or. htcxk(i).eq.'   ') then
                call ERRNUM (540,0,' ',' ',0d0,0d0,0d0,ierr,herr)
                RETURN
              endif
            endif
          endif
        enddo
      endif

c  Check to see if the mixture has interaction parameters.
      if (icomp.eq.0) then
        iw1=1
        do i=1,nc-1
          if (z(i).gt.0.) then
            iw2=0
            do j=i+1,nc
              if (z(j).gt.0.) then
                if (ABS( xecsljs(i,j)).gt.1d-20) iw2=iw2+1
                if (ABS( xecslje(i,j)).gt.1d-20) iw2=iw2+1
                if (ABS( xecskij(i,j)).gt.1d-20) iw2=iw2+1
                if (ABS(xecskijk(i,j)).gt.1d-20) iw2=iw2+1
                if (ABS(xecslijk(i,j)).gt.1d-20) iw2=iw2+1
              endif
            enddo
            iw1=iw1*iw2
          endif
        enddo

c  Do the following checks if no interaction parameters for the transport equations are available.
        if (iw1.eq.0 .and. itrnflag.eq.0) then
          if (ixWat.ne.0) then              !Check for water mixtures with z(water)>5%.
            if (z(ixWat).gt.0.05d0)
     &        call ERRNUM (541,0,' ',' ',0d0,0d0,0d0,ierr,herr)
          endif
          if (ixAlc.ne.0)  call ERRNUM (542,0,' ',' ',0d0,0d0,0d0,
     &                                  ierr,herr)    !Check for alcohols.
c         if (iAmWat.ne.0) call ERRNUM (543,0,' ',' ',0d0,0d0,0d0,
c    &                                  ierr,herr)    !Check for ammonia/water.
        endif
      endif
      if (ierr.gt.0) RETURN

c  Check if inputs are out of range and if hard coded models are available.
      if (icomp.ne.0) then
        call LIMITX ('ETA',T,D,0d0,z,Tmin,tm,dm,pm,ierr1,herr1)
        call LIMITX ('TCX',T,D,0d0,z,Tmin,tm,Dm,pm,ierr2,herr2)
        if (itrnflag.eq.-888 .or. itrnflag.eq.-777 .or. D.eq.0.) ierr1=0  !Special flag to ignore limits.
        if (itrnflag.eq.-888 .or. itrnflag.eq.-777 .or. D.eq.0.) ierr2=0
        if (hetak(icomp)(1:2).ne.'EC' .and. ierr1.le.0) leta=.true.
        if (htcxk(icomp)(1:2).ne.'EC' .and. ierr2.le.0) ltcx=.true.
      endif
      RETURN



c  Write ECS errors and check for bad calculations.
 500  ierr1=0
      ierr2=0
      herr1=' '
      herr2=' '
      if (ierrecs.eq.512) then
        ierr=ierrecs
        herr=herrecs
        RETURN
      endif
      if (icomp.ne.0) then
        call LIMITX ('ETA',T,D,0d0,z,Tmin,tm,dm,pm,ierr1,herr1)
        call LIMITX ('TCX',T,D,0d0,z,Tmin,tm,Dm,pm,ierr2,herr2)
        if (D.eq.0.) then
          ierr1=0
          ierr2=0
        endif
        herr1=herr1(38:)
        herr2=herr2(38:)
        ie=INDEX(herreta,'         ')+1
        it=INDEX(herrtcx,'         ')+1
        i1=INDEX(herr1  ,'         ')+1
        i2=INDEX(herr2  ,'         ')+1
        if (ierreta.gt.0) then                        !Error in viscosity calculation.
          ierr=ierreta
          herr=herreta
        elseif (ierrtcx.gt.0) then                    !Error in thermal conductivity calculation.
          ierr=ierrtcx
          herr=herrtcx
        elseif (ierr1.gt.0 .and. ierr2.gt.0) then     !Both viscosity and thermal conductivity out of range.
          call ERRNUM (74,0,' ',' ',0d0,0d0,0d0,ierr,herr)
          i=INDEX(herr,'         ')+1
          if (herr1.ne.herr2) then
            herr=herr(1:i)//herr1(1:i1)//herr2(1:i2)
          else
            herr=herr(1:i)//herr1(1:i1)
          endif
        elseif (ierr1.gt.0) then                      !Viscosity is out of range.
          call ERRNUM (75,0,'error 75',herr1,0d0,0d0,0d0,ierr,herr)
        elseif (ierr2.gt.0) then                      !Thermal conductivity is out of range.
          call ERRNUM (76,0,'error 76',herr2,0d0,0d0,0d0,ierr,herr)
        elseif (ierreta.lt.0) then                    !Warning in viscosity calculation.
          ierr=ierreta
          herr=herreta
        elseif (ierrtcx.lt.0) then                    !Warning in thermal conductivity calculation.
          ierr=ierrtcx
          herr=herrtcx
        elseif (ierr1.lt.0) then                      !Warning in viscosity ranges.
          call ERRNUM(-75,0,'warning -75',herr1,0d0,0d0,0d0,ierr,herr)
        elseif (ierr2.lt.0) then                      !Warning in thermal conductivity ranges.
          call ERRNUM(-76,0,'warning -76',herr2,0d0,0d0,0d0,ierr,herr)
        endif
      endif
      if ((ierrecs.gt.0 .and. ierr.le.0) .or.
     &    (ierrecs.lt.0 .and. ierr.eq.0)) then
        call ERRNUM (510,ierrecs,herrecs,' ',0d0,0d0,0d0,ierr,herr)
      endif

      if (eta.gt.1d6) eta=-eta    !Make negative to force errors below.
      if (tcx.gt.1d6) tcx=-tcx
      if (ierr.le.0 .and. (eta.lt.0. .or. tcx.lt.0.))
     &  call ERRNUM (561,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      if (eta.lt.0.) eta=xnotc
      if (tcx.lt.0.) tcx=xnotc

      end                                            subroutine CHECKTRN
c
c ======================================================================
c
      subroutine DERVTRN (T,D,z,iFlg1,iFlg2,
     &                        dVISdD,dVISdT,d2VISdD2,d2VISdT2,d2VISdTD,
     &                        dTCXdD,dTCXdT,d2TCXdD2,d2TCXdT2,d2TCXdTD)
c
c  Compute derivatives numerically of viscosity and thermal conductivity
c  as a function of temperature and/or density.
c
c  Inputs:
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition (array of mole fractions)
c    iFlg1--Flag to specify which properties to calculate (to save time):
c           0 - Viscosity and thermal conductivity
c           1 - Viscosity only
c           2 - Thermal conductivity only
c    iFlg2--Flag to specify which derivatives to calculate (to save time):
c           0 - All derivatives
c           1 - First derivatives only
c           2 - First derivatives with respect to temperature only
c           3 - First derivatives with respect to density only
c           4 - 1st and 2nd derivatives with respect to temperature only
c           5 - 1st and 2nd derivatives with respect to density only
c
c  Outputs:
c     dVISdD--dVIS/dD at constant T
c     dVISdT--dVIS/dT at constant D
c   d2VISdD2--d^2VIS/dD^2 at constant T
c   d2VISdT2--d^2VIS/dT^2 at constant D
c   d2VISdTD--d^2VIS/dTdD
c     dTCXdD--dTCX/dD at constant T
c     dTCXdT--dTCX/dT at constant D
c   d2TCXdD2--d^2TCX/dD^2 at constant T
c   d2TCXdT2--d^2TCX/dT^2 at constant D
c   d2TCXdTD--d^2TCX/dTdD (not yet implemented)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  07-20-17 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax)
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DERVTRN

      if (T.le.0. .or. D.lt.0.) RETURN
      dVISdD  =0d0
      dVISdT  =0d0
      d2VISdD2=0d0
      d2VISdT2=0d0
      d2VISdTD=0d0
      dTCXdD  =0d0
      dTCXdT  =0d0
      d2TCXdD2=0d0
      d2TCXdT2=0d0
      d2TCXdTD=0d0

      dT=0.001d0
      dD=0.00001d0
      Dx=D
      if (Dx.lt.1.d-5) Dx=1.d-5
      if (Dx.lt.dD) dD=Dx
      i=iFlg1
      i=iFlg2                     !Keeps the compiler happy.
      call ISPURE (z,i)
      call TRNPRP (T   ,Dx,z,eta, tcx, ierr,herr)
      call TRNPRP (T+dT,Dx,z,etap,tcxp,ierr,herr)
      call TRNPRP (T-dT,Dx,z,etan,tcxn,ierr,herr)
      dVISdT=(etap-etan)/(2d0*dT)
      dTCXdT=(tcxp-tcxn)/(2d0*dT)
      d2VISdT2=(etap+etan-2d0*eta)/dT**2
      d2TCXdT2=(tcxp+tcxn-2d0*tcx)/dT**2
      call TRNPRP (T,Dx+dD,z,etap,tcxp,ierr,herr)
      call TRNPRP (T,Dx-dD,z,etan,tcxn,ierr,herr)
      dVISdD=(etap-etan)/(2d0*dD)
      dTCXdD=(tcxp-tcxn)/(2d0*dD)
      d2VISdD2=(etap+etan-2d0*eta)/dD**2
      d2TCXdD2=(tcxp+tcxn-2d0*tcx)/dD**2

      end                                             subroutine DERVTRN
c
c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                     End file TRNSP.FOR
c ======================================================================








