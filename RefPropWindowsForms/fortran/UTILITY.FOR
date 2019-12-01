c  Begin file UTILITY.FOR
c
c  This file contains various utility subroutines to retrieve information
c    about the components, solve system of equations, and so on.
c
c  Contained here are:
c
c     function WMOL (z)
c     function WMOLI (icomp)
c     subroutine INFO (icomp,wmm,Ttrp,Tnbpt,Tc,Pc,Dc,Zc,acf,dip,Rgas)
c     subroutine NAME (icomp,hnam,hn80,hcasn)
c     subroutine XMASS (xmol,xkg,wmix)
c     subroutine XMOLE (xkg,xmol,wmix)
c     subroutine QMASS (qmol,xl,xv,qkg,xlkg,xvkg,wliq,wvap,ierr,herr)
c     subroutine QMOLE (qkg,xlkg,xvkg,qmol,xl,xv,wliq,wvap,ierr,herr)
c     subroutine ISPURE (z,icomp)
c     subroutine SWAPV (a,b)
c     subroutine SWAPI (i,j)
c     subroutine SWAPA (x,y)
c     function XDIV (a,b)
c     function XLOG (g)
c     function EXP1 (e)
c     function EXP3 (e)
c     function EXP4 (e)
c     function EXP5 (e)
c     function XSQRT (s)
c     function XCBRT (c)
c     function UCASE (b)
c     function LCASE (b)
c     subroutine STRINGSPLIT (k,a,b,n,hf,ierr,herr)
c     subroutine CUBIC (a,z)
c     subroutine FITL (n,m,maxn,xPnts,yPnts,xval,yval,cmatrix,ierr,herr)
c     subroutine POLYN (n,xPnts,yPnts,xval,yval,ierr,herr)
c     subroutine NEXTPNT (n,iex,npnts,D,T,P,Dx,x)
c     subroutine LUDECOMP (n,maxn,aMatrix,cMatrix,ierr,herr)
c     subroutine PIVOT (n,maxn,j,aMatrix,sdecomp)
c     subroutine MINEIGEN (n,a,u,xlamda,ierr,herr)
c     subroutine TRIDIAG (j,d,e,c,b,z)
c     subroutine CSPLINE (n,xi,fi,coeff)
c     subroutine CSPLNVAL (isp,x,f,ierr,herr)
c     subroutine SPLNROOT (isp,iderv,z,f,a,ierr,herr)
c     subroutine SPLNVAL (isp,iderv,z,a,f,ierr,herr)
c     subroutine ROOTFIND (hfunc,ibt,jbt,b,z,kph,iderv,itd,iib,ilg1,ilg2,T,D,ot,ierr,herr)
c     subroutine ROOTSEARCH (iFlag,ipn,v,f,v3,it,ib,lneg,lpos)
c     subroutine IDCRV (D,z,T,ierr,herr)
c     subroutine BLCRV (D,z,T,ierr,herr)
c     subroutine JTCRV (D,z,T,ierr,herr)
c     subroutine JICRV (D,z,T,ierr,herr)
c     subroutine PTANC (T,P,q,b,ibt,Dl,Dv)
c     subroutine PASSCMN (hvr,iset,icomp,jcomp,hstr,ilng,dbl,arr,ierr,herr)
c     subroutine SETPNTS (iFlag,icomp,z,T,P,Dl,Dv,ierr,herr)
c     subroutine FLUIDID (hcasi,hashi,iFldID)
c     subroutine GETINPUTS (ab,ab2,at,bt,iat,ibt,a,b,aa,bb,T,D,P,ierr,herr)
c     subroutine CHECKSTATE (iFlag,mFlag,T,D,z,Err,ierr,herr)
c     subroutine CHECKLIMITS (ab,T,D,P,z,ierr,herr)
c     subroutine LIMITX (htyp,T,D,P,z,Tmin,Tmax,Dmax,Pmax,ierr,herr)
c     subroutine LIMITK (htyp,icomp,T,D,P,Tmin,Tmax,Dmax,Pmax,ierr,herr)
c     subroutine LIMITS (htyp,z,Tmin,Tmax,Dmax,Pmax)
c     subroutine ERRNUM2 (ierr,ihr,ha,hb,hx,hy,hz,herr)
c     subroutine IDHCONV(v,k,kflg,i,j,hx)
c     subroutine ERRNUM (i,ihr,ha,hb,x,y,z,ierr,herr)
c
c ======================================================================
c ======================================================================
c
      function WMOL (z)
c
c  Return the molar mass (molecular weight) for a mixture of
c  a specified composition.
c
c  Input:
c        z--Composition array (array of mole fractions)
c
c  Output (as the function value):
c     WMOL--Molar mass [g/mol], a.k.a. molecular weight

      include 'COMMONS.INC'
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::WMOL

      call ISPURE (z,icomp)
      if (icomp.ne.0) WMOL=wmas(icomp)
      if (icomp.eq.0) WMOL=SUM(z(1:nc)*wmas(1:nc))
      WMOL=xEq0(WMOL)
      end                                                  function WMOL
c
c ======================================================================
c
      function WMOLI (icomp)
c
c  Return the molar mass (molecular weight) of a component in a mixture.
c
c  Input:
c    icomp--Component number in the mixture
c
c  Output (as the function value):
c    WMOLI--Molar mass [g/mol]

      include 'COMMONS.INC'
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::WMOLI

      WMOLI=wmas(icomp)
      end                                                 function WMOLI
c
c ======================================================================
c
      subroutine INFO (icomp,wmm,Ttrp,Tnbpt,Tc,Pc,Dc,Zc,acf,dip,Rgas)
c
c  Provides fluid constants for the specified component.
c
c  Input:
c    icomp--Component number in mixture; 1 for pure fluid
c
c  Outputs:
c      wmm--Molar mass (molecular weight) [g/mol]
c     Ttrp--Triple point temperature [K]
c    Tnbpt--Normal boiling point temperature [K]
c       Tc--Critical temperature [K]
c       Pc--Critical pressure [kPa]
c       Dc--Critical density [mol/L]
c       Zc--Compressibility factor at critical point [Pc/(Rgas*Tc*Dc)]
c      acf--Acentric factor [-]
c      dip--Dipole moment [debye]
c     Rgas--Gas constant [J/mol-K]

      include 'COMMONS.INC'
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::INFO

      wmm=1d0
      Ttrp=1d0
      Tnbpt=1d0
      Tc=1d0
      Pc=1d0
      Dc=1d0
      Zc=1d0
      acf=1d0
      dip=1d0
      Rgas=xRgas
      if (ABS(icomp).le.nc) then
        wmm=wmas(icomp)
        Ttrp=Ttpn(icomp)
        Tnbpt=Tnbp(icomp)
        Tc=Tcrit(icomp)
        Pc=Pcrit(icomp)
        Dc=Dcrit(icomp)
        Zc=Pcrit(icomp)/(Reos(icomp)*Tcrit(icomp)*Dcrit(icomp))
        acf=accen(icomp)
        dip=dipole(icomp)
        Rgas=Reos(icomp)
      endif
      end                                                subroutine INFO
c
c ======================================================================
c
      subroutine NAME (icomp,hnam,hn80,hcasn)
c
c  Provides name information for the specified component.
c
c  Input:
c    icomp--Component number in mixture; 1 for pure fluid
c
c  Outputs:
c     hnam--Component name (character*12) (send icomp+1000 to get the fluid hash)
c     hn80--Component name - long form (character*80)
c           To return the file name used when SETUP was called (without path), send -icomp.
c           If path is also needed, use PASSCMN.  For example:
c           call PASSCMN ('hdir',0,1,0,hfl,i,xx,arr,ierr,herr)
c    hcasn--ID (Chemical Abstracts Service) number (character*12)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  02-06-97  MM, original version
c  05-28-15 EWL, return file name if icomp<0
c  09-02-15 EWL, remove comments in the hn80 variable (those that start with '!')
c  09-02-15 EWL, terminate strings with null character

      include 'COMMONS.INC'
      character hnam*12,hcasn*12,hn80*80
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::NAME

      hnam='not defined'
      hcasn='not defined'
      hn80='not defined'
      if (ABS(icomp).ge.0 .and. ABS(icomp).le.nq) then
        hnam=hname(ABS(icomp))
        hcasn=hcas(ABS(icomp))
        hn80=hnam80(ABS(icomp))
        if (icomp.lt.0) then
          hn80=hinpnme(ABS(icomp))(1:80)
          i=INDEX(hinpnme(ABS(icomp)),CHAR(92),.true.)
          if (i.eq.0) i=INDEX(hinpnme(ABS(icomp)),'/',.true.)
          if (i.eq.0) i=INDEX(hinpnme(ABS(icomp)),':',.true.)
          if (i.gt.0) hn80=hinpnme(ABS(icomp))(i+1:)
        endif
      endif
      if (icomp.gt.1000 .and. icomp.le.1000+nq) then
        hnam=hash(icomp-1000)
      endif

      end                                                subroutine NAME
c
c ======================================================================
c
      subroutine XMASS (xmol,xkg,wmix)
c
c  Converts composition on a mole fraction basis to mass fractions.
c
c  Input:
c     xmol--Composition array (array of mole fractions)
c
c  Outputs:
c      xkg--Composition array (array of mass fractions)
c     wmix--Molar mass of the mixture [g/mol], a.k.a. molecular weight

      include 'COMMONS.INC'
      dimension xmol(ncmax),xkg(ncmax),xsumi(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::XMASS

      call ISPURE (xmol,icomp)
      if (icomp.ne.0) then
        wmix=wmas(icomp)
        xkg(1:nc)=0d0
        xkg(icomp)=1d0
      else
        xsumi(1:nc)=xmol(1:nc)*wmas(1:nc)
        wmix=SUM(xsumi(1:nc))
        if (ABS(wmix).le.1d-10) wmix=1d0
        xkg(1:nc)=xsumi(1:nc)/wmix
      endif
      end                                               subroutine XMASS
c
c ======================================================================
c
      subroutine XMOLE (xkg,xmol,wmix)
c
c  Converts composition on a mass fraction basis to mole fraction.
c
c  Input:
c      xkg--Composition array (array of mass fractions)
c
c  Outputs:
c     xmol--Composition array (array of mole fractions)
c     wmix--Molar mass of the mixture [g/mol], a.k.a. molecular weight

      include 'COMMONS.INC'
      dimension xmol(ncmax),xkg(ncmax),xsumi(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::XMOLE

      call ISPURE (xkg,icomp)
      if (icomp.ne.0) then
        wmix=wmas(icomp)
        xmol(1:nc)=0d0
        xmol(icomp)=1d0
      else
        xsumi=0d0
        do i=1,nc
          if (lNE0(wmas(i))) xsumi(i)=xkg(i)/wmas(i)
        enddo
        wmix=xEq0(SUM(xsumi(1:nc)))
        xmol(1:nc)=xsumi(1:nc)/wmix
        wmix=SUM(xmol(1:nc)*wmas(1:nc))
      endif
      end                                               subroutine XMOLE
c
c ======================================================================
c
      subroutine QMASS (qmol,xl,xv,qkg,xlkg,xvkg,wliq,wvap,ierr,herr)
c
c  Converts quality and composition on a molar basis to a mass basis.
c
c  Inputs:
c     qmol--Molar quality (moles of vapor/total moles)
c           qmol = 0 indicates saturated liquid
c           qmol = 1 indicates saturated vapor
c           0 < qmol < 1 indicates a two-phase state
c           qmol < 0 or qmol > 1 are not allowed and will result in warning
c       xl--Composition of liquid phase (array of mole fractions)
c       xv--Composition of vapor phase (array of mole fractions)
c
c  Outputs:
c      qkg--Quality on mass basis (mass of vapor/total mass)
c     xlkg--Mass composition of liquid phase (array of mass fractions)
c     xvkg--Mass composition of vapor phase (array of mass fractions)
c     wliq--Molar mass of liquid phase [g/mol]
c     wvap--Molar mass of vapor phase [g/mol]
c     ierr--Error flag:  0 - All inputs within limits
c                      -19 - Input q < 0 or > 1
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character*255 herr
      dimension xl(ncmax),xv(ncmax),xlkg(ncmax),xvkg(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::QMASS

      ierr=0
      herr=' '
      qkg=qmol
      eps=1d-8
      call XMASS (xl,xlkg,wliq)
      call XMASS (xv,xvkg,wvap)
      if (qmol.lt.-eps .or. qmol.gt.1d0+eps) then
        call ERRNUM (-19,0,'QMASS',' ',0d0,0d0,0d0,ierr,herr)
      else
        den=qmol*wvap+(1d0-qmol)*wliq
        if (lNE0(den)) qkg=qmol*wvap/den
      endif
      end                                               subroutine QMASS
c
c ======================================================================
c
      subroutine QMOLE (qkg,xlkg,xvkg,qmol,xl,xv,wliq,wvap,ierr,herr)
c
c  Converts quality and composition on a mass basis to a molar basis.
c
c  Inputs:
c      qkg--Quality on mass basis (mass of vapor/total mass)
c           qkg = 0 indicates saturated liquid
c           qkg = 1 indicates saturated vapor
c           0 < qkg < 1 indicates a two-phase state
c           qkg < 0 or qkg > 1 are not allowed and will result in warning
c     xlkg--Mass composition of liquid phase (array of mass fractions)
c     xvkg--Mass composition of vapor phase (array of mass fractions)
c
c  Outputs:
c     qmol--Quality on molar basis (moles of vapor/total mass)
c       xl--Molar composition of liquid phase (array of mole fractions)
c       xv--Molar composition of vapor phase (array of mole fractions)
c     wliq--Molar mass of liquid phase [g/mol]
c     wvap--Molar mass of vapor phase [g/mol]
c     ierr--Error flag:  0 - All inputs within limits
c                      -19 - Input q < 0 or > 1
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character*255 herr
      dimension xl(ncmax),xv(ncmax),xlkg(ncmax),xvkg(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::QMOLE

      ierr=0
      herr=' '
      qmol=qkg
      eps=1d-8
      call XMOLE (xlkg,xl,wliq)
      call XMOLE (xvkg,xv,wvap)
      if (qkg.lt.-eps .or. qkg.gt.1d0+eps) then
        call ERRNUM (-19,0,'QMOLE',' ',0d0,0d0,0d0,ierr,herr)
      else
        if (wliq.gt.0 .and. wvap.gt.0) then
          den=qkg/wvap+(1d0-qkg)/wliq
          if (lNE0(den)) qmol=qkg/wvap/den
        endif
      endif
      end                                               subroutine QMOLE
c
c ======================================================================
c
      subroutine ISPURE (z,icomp)
c
c  Determine if the user has requested the properties of a pure fluid.
c  This happens when the number of components (nc) is set to one, or
c  when one of the values in the composition is equal to one.
c
c  Inputs:
c        z--Composition (array of mole fractions)
c
c  Outputs:
c    icomp--Index set to one of the following:
c           1       - if nc=1
c           1 to nc - for a mixture but with one of the compositions set to 1
c           0       - for a mixture
c           If the flag icnc has been set, return that value (i.e., a pure fluid is being used)
c
      include 'COMMONS.INC'
      dimension z(ncmax)
      icomp=0
      if (nc.le.1) then
        icomp=1
      elseif (icnc.gt.0) then
        icomp=icnc
      else
        do i=1,nc
          if (ABS(z(i)-1d0).lt.1d-12) then
            icomp=i
            EXIT
          endif
        enddo
      endif
      end                                              subroutine ISPURE
c
c ======================================================================
c
      subroutine SWAPV (a,b)
      include 'CONSTS.INC'
      c=a
      a=b
      b=c
      end                                               subroutine SWAPV
c
c ======================================================================
c
      subroutine SWAPI (i,j)
      include 'CONSTS.INC'
      k=i
      i=j
      j=k
      end                                               subroutine SWAPI
c
c ======================================================================
c
      subroutine SWAPA (x,y)
      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax)
      z(1:nc)=x(1:nc)
      x(1:nc)=y(1:nc)
      y(1:nc)=z(1:nc)
      end                                               subroutine SWAPA
c
c ======================================================================
c
      subroutine SWAPERR (ierr,ierr2,herr,herr2)

c  Check if ierr is a warning and ierr2 is an error, and if so then
c  put the values of ierr2 and herr2 into ierr and herr2.
c  If ierr is zero and ierr2 is anything, then swap as above.

      include 'CONSTS.INC'
      character*255 herr,herr2
      if ((ierr.le.0 .and. ierr2.gt.0) .or.
     &    (ierr.eq.0 .and. ierr2.ne.0)) then
        ierr=ierr2
        herr=herr2
        ierr2=0
        herr2=' '
      endif
      end                                             subroutine SWAPERR
c
c ======================================================================
c
      function XDIV (a,b)

c  Divide the value of a by b, but check the value of b first to make sure
c  it is not zero (or very small).  If so, return a value of 1d+20.

      include 'CONSTS.INC'
      XDIV=1d+20
      if (ABS(b).gt.1d-20) XDIV=a/b
      end                                                  function XDIV
c
c ======================================================================
c
      function XDIV1 (a,b)

c  Divide the value of a by b, but check the value of b first to make sure
c  it is not very small (<1x10^-5).  If so, return a value of 1.

      include 'CONSTS.INC'
      XDIV1=1d0
      if (ABS(b).gt.1d-5) XDIV1=a/b
      end                                                 function XDIV1
c
c ======================================================================
c
      function XDIV2 (a,b)

c  Divide the value of a by b, but check the value of b first to make sure
c  it is not very small (<1x10^-20).  If so, return a value of 1.

      include 'CONSTS.INC'
      XDIV2=1d0
      if (ABS(b).gt.1d-20) XDIV2=a/b
      end                                                 function XDIV2
c
c ======================================================================
c
      function XDIV3 (a,b)

c  Divide the value of a by b, but check the value of b first to make sure
c  it is not very small (<1x10^-40).  If so, return the enumerator xinf.

      include 'CONSTS.INC'
      XDIV3=xinf
      if (ABS(b).gt.1d-40) XDIV3=a/b
      end                                                 function XDIV3
c
c ======================================================================
c
      function XLOG (g)

c  Take the LOG of a number only if it is positive.  If negative,
c  simply return that number.

      include 'CONSTS.INC'
      XLOG=g
      if (g.gt.0.) XLOG=LOG(g)
      end                                                  function XLOG
c
c ======================================================================
c
      function EXP1 (e)

c  Take the exponential of a number only if it produces a value
c  less than a million.

      include 'CONSTS.INC'
      if (e.gt.13.81551d0) then        !Exp of this number is nearly 1,000,000.
        EXP1=1d6
      else
        EXP1=EXP(e)
      endif
      end                                                  function EXP1
c
c ======================================================================
c
      function EXP3 (e)

c  Take the exponential of a number only if it produces a value
c  less than a billion.

      include 'CONSTS.INC'
      if (e.gt.20.72326d0) then        !Nearly equal to 10^9.
        EXP3=1d9
      else
        EXP3=EXP(e)
      endif
      end                                                  function EXP3
c
c ======================================================================
c
      function EXP4 (e)

c  Take the exponential of a number only if it produces a value
c  less than a trillion.
c  Check for extremely small values of e, and return 0 if found.

      include 'CONSTS.INC'
      if (e.gt.27.63102d0) then        !Nearly equal to 10^12.
        EXP4=1d12
      elseif (e.lt.-200) then
        EXP4=0d0
      else
        EXP4=EXP(e)
      endif
      end                                                  function EXP4
c
c ======================================================================
c
      function EXP5 (e)

c  Take the exponential of a number only if it produces a value
c  less than a ba-zillion.
c  Check for extremely small values of e, and return 0 if found.

      include 'CONSTS.INC'
      if (e.gt.230) then
        EXP5=1d100
      elseif (e.lt.-200) then
        EXP5=0d0
      else
        EXP5=EXP(e)
      endif
      end                                                  function EXP5
c
c ======================================================================
c
      function XSQRT (s)

c  Take the square root of a number only if it is positive.

      include 'CONSTS.INC'
      if (s.le.0.) then
        XSQRT=0d0
      else
        XSQRT=SQRT(s)
      endif
      end                                                 function XSQRT

c
c ======================================================================
c
      function XCBRT (c)
c
c  Cube root function - allows negative arguments.
c
c  Input:
c        c--value to be acted upon
c
c  Output (as the function value):
c    XCBRT--cube root of c

      include 'CONSTS.INC'
      xa=ABS(c)
      xathrd=xa**thrd
      XCBRT=xathrd
      if (c.lt.0.) XCBRT=-xathrd
      end                                                 function XCBRT
c
c ======================================================================
c
      function XPOWR (a,b)

c  Take the power of a number only if it is positive for a non-integer power.

      include 'CONSTS.INC'
      XPOWR=0d0
      if (a.lt.0.) then
        if (b.ne.INT(b)) RETURN
      elseif (a.eq.0d0 .and. b.le.0) then
        RETURN
      elseif (b.gt.1d10) then
        XPOWR=b
        if (a.gt.1) RETURN
      endif
      XPOWR=a**b
      end                                                 function XPOWR
c
c ======================================================================
c
      function iHEXtoDEC (ha)

c  Convert a hexadecimal number to an integer.  If string contains
c    dashes, remove them first.

      include 'COMMONS.INC'
      character ha*(*),hb*50,hc*10
      read (ha,'(z8)',err=100) ib !Input string must contain eight characters or less for this to work.
      iHEXtoDEC=ib                ! Otherwise the number will be larger than a LONG variable.
      RETURN
 100  hb=ha
      j=0
      do
        i=INDEX (hb,'-')          !If a CAS number was sent, remove the dashes and try again.
        if (i.eq.0) EXIT
        j=1
        hb=hb(:i-1)//hb(i+1:)
      enddo
      read (hb,*,err=200) ib
      if (j.eq.1) then
        hc=' '
        call FLUIDID (hb,hc,i)
        read (hc,'(z8)',err=200) ib
      endif
      iHEXtoDEC=ib
      RETURN
 200  iHEXtoDEC=0                 !Return zero for invalid input strings.
      end                                             function iHEXtoDEC
c
c ======================================================================
c
      function lNE0 (a)

c  Check if a double precision number is not equal to zero (greater than 1x10^-10)

      include 'CONSTS.INC'
      logical lNE0
      lNE0=.false.
      if (ABS(a).gt.1d-10) lNE0=.true.
      end                                                  function lNE0
c
c ======================================================================
c
      function lEq0 (a)

c  Check if a double precision number is equal to zero (less than 1x10^-10)

      include 'CONSTS.INC'
      logical lEQ0
      lEQ0=.false.
      if (ABS(a).le.1d-10) lEQ0=.true.
      end                                                  function lEq0
c
c ======================================================================
c
      function lEqual (a,b)

c  Check if the difference between two double precision numbers is within 1x10^-10

      include 'CONSTS.INC'
      logical lEqual
      lEqual=.false.
      if (ABS(a-b).lt.1d-10) lEqual=.true.
      end                                                function lEqual
c
c ======================================================================
c
      function lEqual2 (a,b)

c  Check if the deviation between two double precision numbers is within 1x10^-6

      include 'CONSTS.INC'
      logical lEqual2
      lEqual2=.false.
      if (ABS(a).lt.1.d-16) RETURN
      if (ABS((a-b)/a).lt.1d-6) lEqual2=.true.
      end                                               function lEqual2
c
c ======================================================================
c
      function XEQ0 (a)

c  Check if a double precision number is equal to zero (less than
c  1x10^-10); if it is, return a value of 1, otherwise simply return
c  the input value.  This is basically set up for dividing two numbers,
c  where the denominator can be checked earlier and assigned to a
c  variable before the division is made.  The value of one returns a
c  manageable number, rather than a near infinite value as done by XDIV.

      include 'CONSTS.INC'
      XEQ0=a
      if (ABS(a).le.1d-10) XEQ0=1d0
      end                                                  function XEQ0
c
c ======================================================================
c
      subroutine READHA (nread,il,hb)

c  Internal routine.

      include 'CONSTS.INC'
      character hb*(*),ha*255,LTRIM*255
      hb=' '
      read (nread,'(a255)',err=10,end=10) ha
      ha=LTRIM(ha)
      if (il.eq.  3) read (ha,'(a3)'  ) hb
      if (il.eq.255) read (ha,'(a255)') hb
 10   continue
      end                                              subroutine READHA
c
c ======================================================================
c
      function LTRIM (a)

c  Trim off all leading spaces.

      include 'CONSTS.INC'
      character LTRIM*255,a*(*)
      do i=1,LEN(a)
        if (a(i:i).ne.' ') EXIT
      enddo
      LTRIM=a(i:)
      end                                                 function LTRIM
c
c ======================================================================
c
      function LTRIM10000 (a)

c  Trim off all leading spaces.

      include 'CONSTS.INC'
      character LTRIM10000*10000,a*(*)
      do i=1,LEN(a)
        if (a(i:i).ne.' ') EXIT
      enddo
      LTRIM10000=a(i:)
      end                                            function LTRIM10000
c
c ======================================================================
c
      function UCASE (a)
c
c  Make all the characters in the string b uppercase, from the first
c    character to character k.  Allow inputs up to 255 characters long.

      include 'CONSTS.INC'
      character UCASE*255,b*255,a*(*)
      b=a
      i=INDEX(b,CHAR(0))
      if (i.ne.0) b=b(1:i-1)
      il=LEN(a)
      if (il.gt.255) il=255
      do i=1,il
        j=ICHAR(b(i:i))
        if (j.gt.96 .and. j.le.122) j=j-32
        b(i:i)=CHAR(j)
      enddo
      UCASE=b
      end                                                 function UCASE
c
c ======================================================================
c
      function UCASE10000 (a)
c
c  Make all the characters in the string b uppercase, from the first
c    character to character k.  Allow inputs up to 10000 characters long.

      include 'CONSTS.INC'
      character UCASE10000*10000,b*10000,a*(*)
      b=a
      i=INDEX(b,CHAR(0))
      if (i.ne.0) b=b(1:i-1)
      il=LEN(a)
      if (il.gt.10000) il=10000
      do i=1,il
        j=ICHAR(b(i:i))
        if (j.gt.96 .and. j.le.122) j=j-32
        b(i:i)=CHAR(j)
      enddo
      UCASE10000=b
      end                                            function UCASE10000
c
c ======================================================================
c
      function LCASE (b,k)
c
c  Make all the characters in the string b lowercase, from the first
c    character to character k.

      include 'CONSTS.INC'
      character LCASE*255,a*255,b*(*)
      a=b
      do i=1,k
        j=ICHAR(a(i:i))
        if (j.gt.64 .and. j.le.90) j=j+32
        a(i:i)=CHAR(j)
      enddo
      LCASE=a
      end                                                 function LCASE
c
c ======================================================================
c
      function REMOVENUL (a)
c
c  Search for the presence of a NUL character and remove it and all
c    characters behind it.

      include 'CONSTS.INC'
      character REMOVENUL*255,a*(*)
      i=INDEX(a,CHAR(0))
      if (i.eq.0) then
        REMOVENUL=a
      else
        REMOVENUL=a(1:i-1)
      endif
      end                                             function REMOVENUL
c
c ======================================================================
c
      function REMOVENUL10000 (a)
c
c  Search for the presence of a NUL character and remove it and all
c    characters behind it.  For use with strings up to 10000 characters long.

      include 'CONSTS.INC'
      character REMOVENUL10000*10000,a*(*)
      i=INDEX(a,CHAR(0))
      if (i.eq.0) then
        REMOVENUL10000=a
      else
        REMOVENUL10000=a(1:i-1)
      endif
      end                                        function REMOVENUL10000
c
c ======================================================================
c
      function CHANGEALL (a,b,c)
c
c  Search for the presence of string b in string a and change to string c.

      include 'CONSTS.INC'
      character CHANGEALL*255,d*255,a*(*),b*(*),c*(*)
      d=a
      do j=1,255
        i=INDEX(d,b)
        if (i.eq.0) EXIT
        d=d(1:i-1)//c//d(i+LEN(b):)
      enddo
      CHANGEALL=d
      end                                             function CHANGEALL
c
c ======================================================================
c
      subroutine FINDLAST (a,b,ilen,ilast)
c
c  Find the last occurrence of string b (using the first ilen characters
c  in that string) in the string a, and return the value in ilast.
c  If ilen is negative, find the end of the string (where only spaces are
c  left after any text or numbers) up to the length specified by -ilen.

      include 'CONSTS.INC'
      character a*(*),b*(*)
      ilast=0
      if (ilen.ge.0) then
        do j=1,100
          i=INDEX(a(ilast+1:),b(1:ilen))
          if (i.eq.0) EXIT
          ilast=ilast+i
        enddo
      else
        if (a.ne.' ') then
          do j=1,-ilen
            k=ICHAR(a(j:j))
            if (k.ne.32 .and. k.ne.0) ilast=j
          enddo
        endif
      endif
      end                                            subroutine FINDLAST
c
c ======================================================================
c
      subroutine STRINGSPLIT (k,a,b,n,hf,ierr,herr)
c
c  Split the contents in the first string into substrings that are placed
c  in hf array.  Up to 10 string separators can be sent in the variable b,
c  for example: '|#;,'   The search for the separators is in the same order
c  as found in the variable b.  The number of strings found in the input
c  string a is returned in n, up to length of the array, where the input
c  parameter k specifies the size of the array.  If the string contains too
c  many substrings, ierr is returned as 999, and the remainder of the string
c  is returned in herr.

      include 'CONSTS.INC'
      character a*(*),b*(*),h(10)*1,ha*10000
      character*255 herr,hf(k),LTRIM
      ierr=0
      herr=' '
      n=0
      hf=' '
      ha=a
      i=INDEX(ha,CHAR(0))    !Check for null character
      if (i.ne.0) ha=ha(:i-1)
      if (ha.eq.' ') RETURN  !Return if string is empty.
      nchar=LEN(b)
      do i=1,LEN(b)
        h(i)=b(i:i)
        if (h(i).eq.' ') EXIT
        nchar=i              !Number of string separators
      enddo
      do
        j=0
        do i=1,nchar
          j=INDEX(ha,h(i))   !Search for the string separators.
          if (j.ne.0) EXIT
        enddo
        n=n+1
        if (j.eq.0) then
          hf(n)=LTRIM(ha(:255))
          RETURN
        else
          hf(n)=LTRIM(ha(:j-1))
          ha=ha(j+1:)
          if (ha.eq.' ') RETURN
          if (n.ge.k) then
            if (ha.ne.' ') then
              ierr=999
              herr=ha        !String contains more information than the space available in the output array.
            endif
            RETURN
          endif
        endif
      enddo

      end                                         subroutine STRINGSPLIT
c
c ======================================================================
c
      subroutine CUBIC (a,z)
c
c  General purpose routine to solve a cubic equation of the form
c  z**3 + a(3)*z**2 +a(2)*z + a(1) = 0
c  The smallest root is returned in z(1) and the largest in z(2).
c  The middle root, if it exists, is discarded.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-24-04 EWL, original version

      include 'CONSTS.INC'
      dimension a(3),z(3)

      add1=2d0*xPi/3d0
      add2=2d0*add1
      a0=a(1)
      a1=a(2)
      a2=a(3)/3d0
      q=a1/3d0-a2*a2
      rr=(a1*a2-a0)/2d0-a2*a2*a2
      test=q*q*q+rr*rr
      if (test.lt.0.) then             !Three distinct roots
        test=SQRT(-test)
        q=2d0*XCBRT(SQRT(rr*rr+test*test))
        theta=xPi
        if (ABS(rr).gt.1d-20) theta=ATAN(test/rr)
        if (theta.lt.0.) theta=theta+xPi
        theta=theta/3d0
        z(1)=q*COS(theta)-a2
        z(2)=q*COS(theta+add1)-a2
        z(3)=q*COS(theta+add2)-a2
      else                             !Either one real, or three real w/ 2 identical.
        test=SQRT(test)
        s1=XCBRT(rr+test)
        s2=XCBRT(rr-test)
        z(1)=s1+s2-a2
        z(2)=z(1)
        z(3)=z(1)
        if (test.gt.0.) RETURN         !Only one real
        z(2)=-(s1+s2)/2d0-a2
        z(3)=z(2)                      !Three real, but 2 identical.
      endif
      zmin=MIN(z(1),z(2),z(3))
      z(2)=MAX(z(1),z(2),z(3))
      z(1)=zmin
      if (zmin.lt.0.) z(1)=z(2)

      end                                               subroutine CUBIC
c
c ======================================================================
c
      subroutine FITL (n,m,maxn,xPnts,yPnts,xval,yval,cmatrix,ierr,herr)
c
c  Linearly fit an equation to (x,y) data and return the value at the
c  specified x value.
c
c  Inputs:
c        n--Number of data points (n>=m)
c           Send -1 for n to calculate a new point without refitting.
c        m--Number of coefficients to fit in the equation y=a(0)+a(1)*x+a(2)*x^2+...+a(m-1)*x^(m-1)
c     maxn--amatrix and cmatrix array sizes (maxn>=m)
c    xPnts--Array of n data points for the x-axis data
c    yPnts--Array of n data points for the y-axis data
c     xval--Value of x where y will be calculated
c
c  Outputs:
c     yval--Value of y at xval
c  cmatrix--Coefficients of the fit
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  07-06-11 EWL, original version

      include 'COMMONS.INC'
      character herr*255
      dimension xPnts(*),yPnts(*),amatrix(maxn,maxn),cmatrix(maxn)

      ierr=0
      herr=' '
      yval=0.d0
      if (n.eq.-1) then      !If n=-1, reuse cMatrix from the last call to calculate another point.
        yval=0.d0
        do j=1,m
          yval=yval+cMatrix(j)*xval**(j-1.d0)
        enddo
      elseif (n.eq.0) then
      elseif (n.eq.1) then             !Return the value of y if only one point sent.
        yval=yPnts(1)
        cMatrix(1)=yval
      elseif (m.eq.1) then
        yval=SUM(yPnts(1:n))/DBLE(n)
        cMatrix(1)=yval
      elseif (n.eq.2) then             !For two points sent, calculate the slope and y-intercept.
        slope=(yPnts(1)-yPnts(2))/(xPnts(1)-xPnts(2))
        b=yPnts(1)-slope*xPnts(1)
        yval=slope*xval+b
        cMatrix(1)=b
        cMatrix(2)=slope
      elseif (n.gt.2 .and. n.ge.m) then
        do i=1,m
          do j=1,i
            k=i+j-2
            aMatrix(i,j)=SUM(xPnts(1:n)**k)      !Sum up i,j pairs in the aMatrix from the xPnts and yPnts data inputs.
            aMatrix(j,i)=aMatrix(i,j)
          enddo
          cMatrix(i)=SUM(yPnts(1:n)*xPnts(1:n)**(i-1))
        enddo
        call LUdecomp (m,maxn,aMatrix,cMatrix,ierr,herr)   !Invert the matrix to obtain the cMatrix vector.
        if (ierr.gt.0) RETURN
        yval=0.d0
        do j=1,m
          yval=yval+cMatrix(j)*xval**(j-1.d0)              !Calculate the y value at the x input data point.
        enddo
      else
        ierr=679
        herr='[FITL error 679] Invalid inputs.'
      endif
      RETURN
      end                                                subroutine FITL
c
c ======================================================================
c
      subroutine POLYN (n,xPnts,yPnts,xval,yval,ierr,herr)
c
c  Calculate the polynomial given the n data points and return the value
c  yval at xval.
c
c  Inputs:
c        n--Number of data points
c    xPnts--Array of n data points for the x-axis data
c    yPnts--Array of n data points for the y-axis data
c     xval--Value of x
c
c  Outputs:
c     yval--Value of y at x
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-28-12 EWL, original version

      include 'COMMONS.INC'
      character herr*255
      dimension xPnts(*),yPnts(*),fdd(20,20)

      ierr=0
      herr=' '
      yval=0d0
      if (n.le.0 .or. n.gt.20) RETURN
      fdd(1:n,1)=ypnts(1:n)
      do j=2,n
        do i=1,n-j+1
          fdd(i,j)=0d0
          xdiff=xpnts(i+j-1)-xpnts(i)
          if (ABS(xdiff).gt.1d-20)
     &        fdd(i,j)=(fdd(i+1,j-1)-fdd(i,j-1))/xdiff
        enddo
      enddo
      xt=1
      fa=fdd(1,1)
      do i=1,n-1
        xt=xt*(xval-xpnts(i))
        fa=fa+fdd(1,i+1)*xt
      enddo
      yval=fa
      end                                               subroutine POLYN
c
c ======================================================================
c
      subroutine NEXTPNT (n,iex,nPnts,iFlg,D,T,P,Dz,z)
c
c  Extrapolate the value of T and P given density with the use of
c  previously calculated points.  This routine is generally only for
c  use in the SATSPLN routine as it generates the initial estimates
c  used later by the saturation routines.
c
c  Inputs:
c        n--Location in the spline arrays containing the last stored point
c           (or some other point where the extrapolation will be performed)
c      iex--Offset to n if required
c    nPnts--Number of points to use in the extrapolation.
c     iFlg--Calculate all properties if set to 0, T only if set to 1,
c             T and P only if set to 2.
c        D--Saturation density [mol/l]
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c       Dz--Density of the equilibrium phase [mol/l]
c        z--Composition of the equilibrium phase (array of mole fractions)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  03-26-14 EWL, original version

      include 'COMMONS.INC'
      dimension z(ncmax),xPnts(1000),yPnts(1000)
      character*255 herr

      T=300d0
      P=0d0
      Dz=0d0
      if (n.gt.nPnts) then
        n3=n-nPnts+1-iex               !Starting location in the arrays.
        n4=n-iex                       !Ending location in the arrays (generally the last point calculated).
        if (n3.lt.0) n3=n
        if (n4.lt.0) n4=n
        xval=XLOG(D)                   !Known condition (density).
        xPnts(1:nPnts)=xArr(n3:n4,0)             !Transfer the known density values in the arrays to the xPnts array.
        yPnts(1:nPnts)=xArr(n3:n4,nc+1)          !Reset to get T from D.
        call POLYN (nPnts,xPnts,yPnts,xval,yval,ierr,herr)
        T=EXP3(yval)
        if (iFlg.eq.1) RETURN
        yPnts(1:nPnts)=xArr(n3:n4,nc+2)          !Reset to get P from D.
        call POLYN (nPnts,xPnts,yPnts,xval,yval,ierr,herr)
        P=EXP4(yval)
        if (iFlg.eq.2) RETURN
        yPnts(1:nPnts)=xArr(n3:n4,nc+3)          !Transfer the known equilibrium density values in the arrays to the yPnts array.
        call POLYN (nPnts,xPnts,yPnts,xval,yval,ierr,herr)           !Get the unknown condition (Dz) at the known value (D).
        Dz=EXP1(yval)
        do i=1,nc
          yPnts(1:nPnts)=xArr(n3:n4,i)           !Reset to get the equilibrium compositions from D.
          call POLYN (nPnts,xPnts,yPnts,xval,yval,ierr,herr)
          y=XLOG(z(i))
          if (ABS(yval-y).gt.5. .and. y.lt.-10) yval=y/2d0           !Keep small values small.
          z(i)=0
          if (yval.ne.0.) z(i)=EXP4(yval)
          if (z(i).lt.1d-100) z(i)=0d0
          if (z(i).gt.10d0) z(i)=10d0
        enddo
        sumz=SUM(z(1:nc))
        if (ABS(sumz).gt.1d-10) z(1:nc)=z(1:nc)/sumz
      endif

      end                                             subroutine NEXTPNT
c
c ======================================================================
c
      subroutine LUDECOMP (n,maxn,aMatrix,cMatrix,ierr,herr)
c
c  Use Lower-Upper decomposition to invert a 2-dimensional
c  array and solve for the roots, where [A][B]=[C].
c
c  Inputs:
c        n--Number of elements to solve
c     maxn--Array size
c  aMatrix--Input array [A] of size (maxn,maxn)
c  cMatrix--Input array [C] of size (maxn)
c     ierr--Send a value of -191 to skip the matrix inversion and
c           obtain a new cMatrix with the aMatrix obtained in a
c           previous call to this routine.  Must be sent as a variable,
c           the value of ierr will be overwritten.
c
c  Outputs:
c  cMatrix--Output array [B] of size (maxn)
c     ierr--Error flag:  0 - Successful
c                      621 - Singular matrix (No error message returned)
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-01-92 EWL, original version
c  12-18-12 EWL, reverse order of Jacobian matrix
c  08-26-14 EWL, move common block iord to COMMONS.INC

      include 'COMMONS.INC'
      character herr*255
      dimension amatrix(maxn,maxn),cmatrix(maxn)
      dimension ctemp(maxn),sdecomp(maxn)

      if (n.le.0) goto 621
      if (ierr.eq.-191) goto 10
      ierr=0
      herr=' '
      do i=1,n
        iord(i)=i
        sdecomp(i)=ABS(aMatrix(i,1))
        do j=2,n
          if (ABS(aMatrix(i,j)).gt.sdecomp(i))
     &        sdecomp(i)=ABS(aMatrix(i,j))
        enddo
        if (sdecomp(i).eq.0) goto 621            !Singular matrix
      enddo

      j=1
      call PIVOT(n,maxn,j,aMatrix,sdecomp)
      if (aMatrix(iord(1),1).eq.0) goto 621
      aMatrix(iord(1),2:n)=aMatrix(iord(1),2:n)/aMatrix(iord(1),1)
      do j=2,n-1
        do i=j,n
          sumx=SUM(aMatrix(iord(i),1:j-1)*aMatrix(iord(1:j-1),j))
          aMatrix(iord(i),j)=aMatrix(iord(i),j)-sumx
        enddo
        call PIVOT(n,maxn,j,aMatrix,sdecomp)
        do k=j+1,n
         sumx=SUM(aMatrix(iord(j),1:j-1)*aMatrix(iord(1:j-1),k))
         if (aMatrix(iord(j),j).eq.0) aMatrix(iord(j),j)=1E+20
         aMatrix(iord(j),k)=(aMatrix(iord(j),k)-sumx)/aMatrix(iord(j),j)
        enddo
      enddo
      sumx=SUM(aMatrix(iord(n),1:n-1)*aMatrix(iord(1:n-1),n))
      aMatrix(iord(n),n)=aMatrix(iord(n),n)-sumx
      If (aMatrix(iord(n),n).eq.0) aMatrix(iord(n),n)=1E+20

 10   ierr=0
      cMatrix(iord(1))=cMatrix(iord(1))/aMatrix(iord(1),1)
      do i=2,n
        sumx=SUM(aMatrix(iord(i),1:i-1)*cMatrix(iord(1:i-1)))
        cMatrix(iord(i))=(cMatrix(iord(i))-sumx)/aMatrix(iord(i),i)
      enddo

      do i=n-1,1,-1
        sumx=SUM(aMatrix(iord(i),i+1:n)*cMatrix(iord(i+1:n)))
        cMatrix(iord(i))=cMatrix(iord(i))-sumx
      enddo
      ctemp(1:n)=cMatrix(iord(1:n))
      cMatrix(1:n)=ctemp(1:n)
      RETURN

 621  ierr=621
      end                                            subroutine LUDECOMP
c
c ======================================================================
c
      subroutine PIVOT (n,maxn,j,aMatrix,sdecomp)
c
c  Part of the LUDECOMP routine.

      include 'COMMONS.INC'
      dimension amatrix(maxn,maxn),sdecomp(maxn)
      ipivt=j
      big=ABS(aMatrix(iord(j),j)/sdecomp(iord(j)))
      do i=j+1,n
        dummy=ABS(aMatrix(iord(i),j)/sdecomp(iord(i)))
        If (dummy.gt.big) then
          big=dummy
          ipivt=i
        endif
      enddo
      idummy=iord(ipivt)
      iord(ipivt)=iord(j)
      iord(j)=idummy

      end                                               subroutine PIVOT
c
c ======================================================================
c
      subroutine MINEIGEN (n,a,u,xlamda,ierr,herr)
c
c  Determine the smallest eigenvalue of the input array.
c
c  Inputs:
c        n--Number of elements to solve
c        a--[ncmax x ncmax] array
c
c  Outputs:
c        u--Unit vector of size ncmax
c   xlamda--Smallest eigenvalue
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by G. Venkatarathnam, Indian Institute of Technology Madras
c  10-15-11  GV, original version
c  01-07-13 EWL, adapt code for use in Refprop

      include 'COMMONS.INC'
      dimension a(ncmax,ncmax),u(ncmax),y(ncmax),a1(ncmax,ncmax)
      character*255 herr
      maxn=ncmax
      tol=1d-12
      maxiters=20
      a1(1:n,1:n)=a(1:n,1:n)-xlamda
      y(1:n)=0d0
      call LUdecomp (n,maxn,a1,y,ierr,herr)
      if (ierr.gt.0) RETURN
      iters=0
      xnorm=1d0
      y(2:n)=0d0
      y(1)=a1(1,1)
      do while (xnorm.gt.tol .and. iters.le.maxiters)
        iters=iters+1
        xlamdao=xlamda
        u(1:n)=y(1:n)/SQRT(SUM(y(1:n)**2))
        y(1:n)=u(1:n)
        ierr=-191
        call LUdecomp (n,maxn,a1,y,ierr,herr)
        if (ierr.gt.0) RETURN
        xlamda=DOT_PRODUCT(y(1:n),MATMUL(a(1:n,1:n),y(1:n)))/
     &       DOT_PRODUCT(y(1:n),y(1:n))
        xnorm=1d0
        if (xlamdao.ne.0d0) xnorm=(1d0-xlamda/xlamdao)**2
      enddo
      if (u(1).lt.0.) u(1:n)=SIGN(u(1:n),-u(1:n))
      if (iters.gt.maxiters)
     &    call ERRNUM (151,0,'MINEIGEN',' ',0d0,0d0,0d0,ierr,herr)

      end                                            subroutine MINEIGEN
c
c ======================================================================
c
      subroutine TRIDIAG (j,d,e,c,b,z)
c
c  Function to solve the tridiagonal linear equation set.
c
c  Written by Matthias Kunick, NIST Thermophysics Division, Boulder, Colorado
c  09-15-11  MK, original version

      include 'COMMONS.INC'
      dimension d(1000),e(1000),c(1000),b(1000)
      dimension z(1000),y(1000),w(1000),v(1000),T(1000)

c  Evaluate the elements in the LU decomposition.
      w(1)=d(1)
      v(1)=c(1)
      T(1)=0d0
      if (w(1).ne.0.) T(1)=e(1)/w(1)
      do i=2,j-1
        w(i)=d(i)-v(i-1)*T(i-1)
        v(i)=c(i)
        T(i)=0d0
        if (w(i).ne.0.) T(i)=e(i)/w(i)
      enddo
      w(j)=d(j)-v(j-1)*T(j-1)

c  Forward substitution to obtain y.
      y(1)=0d0
      if (w(1).ne.0.) y(1)=b(1)/w(1)
      do i=2,j
        y(i)=0d0
        if (w(i).ne.0.) y(i)=(b(i)-v(i-1)*y(i-1))/w(i)
      enddo

c  Backward substitution to obtain z.
      z(j)=y(j)
      do i=j-1,1,-1
        z(i)=y(i)-T(i)*z(i+1)
      enddo

      end                                             subroutine TRIDIAG
c
c ======================================================================
c
      subroutine CSPLINE (n,xi,fi,coeff)
c
c  Cubic-spline approximation.
c
c  Inputs:
c        n--Number of points
c    xi[n]--Abscissa values of nodes
c    fi[n]--Ordinate values of nodes
c
c  Outputs:
c coeff[n]--Spline coefficients (second-order derivatives)
c
c  Written by Matthias Kunick, NIST Thermophysics Division, Boulder, Colorado
c  09-15-11  MK, original version

      include 'COMMONS.INC'
      dimension xi(1000),fi(1000),coeff(1000)
      dimension g(1000),h(1000),d(1000),b(1000),c(1000)

c  Assign the intervals and function differences.
      do i=1,n-1
        h(i)=xi(i+1)-xi(i)
        g(i)=fi(i+1)-fi(i)
      enddo

c  Evaluate the coefficient matrix elements.
      do i=1,n-2
        d(i)=2d0*(h(i+1)+h(i))
        b(i)=0d0
        if (h(i).ne.0. .and. h(i+1).ne.0.)
     &  b(i)=6d0*(g(i+1)/h(i+1)-g(i)/h(i))
        c(i)=h(i+1)
      enddo

c  Obtain the second-order derivatives.
      call TRIDIAG (n-2,d,c,c,b,g)
      coeff(1)=0d0
      coeff(n)=0d0
      coeff(2:n-1)=g(1:n-2)

      end                                             subroutine CSPLINE
c
c ======================================================================
c
      subroutine CSPLNVAL (isp,x,f,ierr,herr)
c
c  Calculates the function value from the saturation splines set up
c  from the call to SATSPLN.
c
c  Inputs:
c      isp--Indicator for which spline to use:
c           0    - Molar density of the known phase
c           1-nc - Composition
c           nc+1 - Temperature
c           nc+2 - Pressure
c           nc+3 - Molar density of the equilibrium phase
c           nc+4 - Enthalpy
c           nc+5 - Entropy
c        x--Independent variable (abscissa)
c
c  Outputs:
c        f--Function value (ordinate)
c     ierr--Error flag:  0 - Successful
c                      631 - No data have been saved
c     herr--Error string (character*255)
c
c  Written by Matthias Kunick, NIST Thermophysics Division, Boulder, Colorado
c  09-15-11  MK, original version
c  03-25-14 EWL, check for splines starting at a high value and decreasing to a low value for the independent variable

      include 'COMMONS.INC'
      character*255 herr

      ierr=0
      herr=' '
      f=0d0
      n=nArr
      if (n.le.0) then
        call ERRNUM (631,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif

      if ((x.gt.xArr(1,0) .and. xArr(1,0).gt.xArr(2,0)) .or.
     &    (x.lt.xArr(1,0) .and. xArr(1,0).lt.xArr(2,0))) then
        f=xArr(1,isp)-(xArr(1,isp)-xArr(2,isp))/
     &    (xArr(1,0)-xArr(2,0))*(xArr(1,0)-x)              !Return extrapolated value beyond f(1).
        RETURN
      elseif ((x.lt.xArr(n,0) .and. xArr(n,0).lt.xArr(n-1,0)) .or.
     &        (x.gt.xArr(n,0) .and. xArr(n,0).gt.xArr(n-1,0))) then
        f=xArr(n,isp)-(xArr(n,isp)-xArr(n-1,isp))/
     &    (xArr(n,0)-xArr(n-1,0))*(xArr(n,0)-x)            !Return extrapolated value beyond f(n).
        RETURN
      endif

      k=1
      dx=x-xArr(1,0)
      dx1=dx
      do while (dx*dx1.gt.0. .and. k.lt.n-1)
        k=k+1
        dx=x-xArr(k,0)
      enddo
      k=k-1
      if (k.lt.1) k=1

      dx=xArr(k+1,0)-xArr(k,0)
      if (dx.eq.0.) RETURN
      alpha=cArr(k+1,isp)/(6d0*dx)
      beta =-cArr(k,isp)/(6d0*dx)
      gamm =xArr(k+1,isp)/dx-dx*cArr(k+1,isp)/6d0
      eta  =dx*cArr(k,isp)/6d0-xArr(k,isp)/dx
      f    =alpha*(x-xArr(k,0))*(x-xArr(k,0))*(x-xArr(k,0))+
     &      beta*(x-xArr(k+1,0))*(x-xArr(k+1,0))*(x-xArr(k+1,0))+
     &      gamm*(x-xArr(k,0))+eta*(x-xArr(k+1,0))

      end                                            subroutine CSPLNVAL
c
c ======================================================================
c
      subroutine CSPLNVAL2 (n,xi,fi,coeff,x,f,ierr,herr)
c
c  Calculates the function value of a spline function.
c
c  Inputs:
c        n--Number of nodes
c    xi[n]--Abscissa values of nodes
c    fi[n]--Ordinate values of nodes
c coeff[n]--Spline coefficients
c       x --Independent variable (abscissa)
c
c  Outputs:
c        f--Function value (ordinate)
c     ierr--Error flag:  0 - Successful
c                      631 - No data have been saved
c     herr--Error string (character*255)
c
c  Written by Matthias Kunick, NIST Thermophysics Division, Boulder, Colorado
c  09-15-11  MK, original version
c  03-25-14 EWL, check for splines starting at a high value and decreasing to a low value for the independent variable

      include 'COMMONS.INC'
      dimension xi(1000),fi(1000),coeff(1000)
      character*255 herr

      ierr=0
      herr=' '
      f=0d0
      if (n.le.0) then
        call ERRNUM (631,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif

      if ((x.gt.xi(1) .and. xi(1).gt.xi(2)) .or.
     &    (x.lt.xi(1) .and. xi(1).lt.xi(2))) then
        f=fi(1)-(fi(1)-fi(2))/(xi(1)-xi(2))*(xi(1)-x)                !Return extrapolated value beyond f(1).
        RETURN
      elseif ((x.lt.xi(n) .and. xi(n).lt.xi(n-1)) .or.
     &        (x.gt.xi(n) .and. xi(n).gt.xi(n-1))) then
        f=fi(n)-(fi(n)-fi(n-1))/(xi(n)-xi(n-1))*(xi(n)-x)            !Return extrapolated value beyond f(n).
        RETURN
      endif

      k=1
      dx=x-xi(1)
      dx1=dx
      do while (dx*dx1.gt.0. .and. k.lt.n-1)
        k=k+1
        dx=x-xi(k)
      enddo
      k=k-1
      if (k.le.1) k=2

      dx=xi(k+1)-xi(k)
      if (dx.eq.0.) RETURN
      alpha=coeff(k+1)/(6d0*dx)
      beta =-coeff(k)/(6d0*dx)
      gamm =fi(k+1)/dx-dx*coeff(k+1)/6d0
      eta  =dx*coeff(k)/6d0-fi(k)/dx
      f    =alpha*(x-xi(k))*(x-xi(k))*(x-xi(k))+
     &      beta*(x-xi(k+1))*(x-xi(k+1))*(x-xi(k+1))+
     &      gamm*(x-xi(k))+eta*(x-xi(k+1))

      end                                           subroutine CSPLNVAL2
c
c ======================================================================
c
      subroutine SPLNROOT (isp,iderv,z,f,a,ierr,herr)
c
c  Calculates the root of a given value of a spline function
c
c  Inputs:
c      isp--Indicator for which spline to use
c           1 to nc - Composition
c           nc+1 - Temperature
c           nc+2 - Pressure
c           nc+3 - Density
c           nc+4 - Enthalpy or Energy (depending on the value of ieflag).
c           nc+5 - Entropy
c    iderv--Values of -1 and -2 return lower and upper root values, a value
c           of 0 returns the spline root, a value of 1 returns the root where
c           the derivative of the spline with respect to D is equal to the
c           the value of f (set f=0 to find maximum or minimum).
c        z--Composition array (array of mole fractions)
c        f--Value of spline function
c
c  Outputs:
c        a--Root value (initial value required since some splines can be doubled valued)
c     ierr--Error flag:  0 - Successful
c                      151 - Routine did not converge.'
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-20-11 EWL, original version
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SPLNROOT

      include 'COMMONS.INC'
      dimension z(ncmax)
      character herr*255
      ierr=0

c  Get upper and lower bounds.
      call SPLNVAL (isp,-1,z,a,alow,ierr,herr)
      call SPLNVAL (isp,-2,z,a,aupp,ierr,herr)
      if (ierr.gt.0) RETURN
      a1=a
      a2=a1+0.1d0
      a3=a2
      call SPLNVAL (isp,iderv,z,a1,f1,ierr,herr)
      f2=1d8
      j=0
      if (nArr.lt.2) goto 20

c  Setup bounds for false-position if needed.
      al=alow
      au=aupp
      call SPLNVAL (isp,iderv,z,al,fl,ierr,herr)
      call SPLNVAL (isp,iderv,z,au,fu,ierr,herr)
      fl=fl-f
      fu=fu-f
      if (fl*fu.lt.0. .and. fu.lt.fl) then
        call SWAPV(fl,fu)
        call SWAPV(al,au)
      endif

      do while (ABS(f2).gt.1d-7)
 30     call SPLNVAL (isp,iderv,z,a2,f2,ierr,herr)
        f2=f2-f
        if (f2.gt.fl .and. f2.lt.0.) then
          al=a2
          fl=f2
        elseif (f2.lt.fu .and. f2.gt.0.) then
          au=a2
          fu=f2
        endif
        j=j+1
        if (ierr.gt.0 .or. j.gt.200 .or. ABS(f1-f2).lt.1d-20) goto 10
        if (j.eq.50 .or. j.eq.100 .or. j.eq.150) then
          a2=a2*0.9d0                  !If stuck, try new value for independent variable.
          goto 30
        endif
        da=-f2*XDIV(a1-a2,f1-f2)
        a1=a2
        f1=f2

c  Don't allow more than a 10% change.
        if (ABS(da).gt.ABS(a1/10d0)) da=da/10d0
        if (ABS(da).gt.ABS(a1/10d0) .and. a2.gt.0.1d0) da=da/10d0
        if (ABS(da).gt.ABS(a1/10d0) .and. a2.gt.0.1d0) da=da/10d0
        a2=a1+da
        if (ABS(f2).gt.1.d-2) then
          if (a2.lt.alow*0.98d0 .or. a2.gt.aupp*1.02d0) a2=a1+da/2d0
          if (a2.lt.alow*0.9d0  .or. a2.gt.aupp*1.1d0)  a2=a1+da/5d0
          if (a2.lt.alow*0.7d0  .or. a2.gt.aupp*1.3d0)  a2=a1+da/10d0
          if (a2.lt.alow*0.5d0  .or. a2.gt.aupp*1.5d0)  a2=a1+da/25d0
          if (a2.lt.alow*0.3d0  .or. a2.gt.aupp*1.7d0)  a2=a1+da/100d0
          if (a2.lt.alow*0.1d0  .or. a2.gt.aupp*2d0)   a2=a1+da/1000d0
        endif
      enddo
      a=a2
      RETURN

c  Iteration failed, try false-position.
 10   a3=a2
      if (fl*fu.ge.0.) then
        a2=au/4d0
        call SPLNVAL (isp,iderv,z,a2,f2,ierr,herr)
        f2=f2-f
        if (a.lt.a2) then
          au=a2
          fu=f2
        else
          al=a2
          fl=f2
        endif
      endif
      if (fl*fu.lt.0.) then
        j=0
        f2=fu
        a2=(al+au)/2d0
        do while (ABS(f2).gt.1d-7)
          call SPLNVAL (isp,iderv,z,a2,f2,ierr,herr)
          f2=f2-f
          if (f2.gt.fl .and. f2.lt.0.) then
            al=a2
            fl=f2
            a2=au-fu*(al-au)/(fl-fu)
          elseif (f2.lt.fu .and. f2.gt.0.) then
            au=a2
            fu=f2
            a2=au-fu*(al-au)/(fl-fu)
          elseif (j.eq.0) then
            a2=0.9*al+0.1*au
          elseif (j.eq.1) then
            a2=0.1*al+0.9*au
          else
            goto 20
          endif
          j=j+1
          if (ierr.gt.0 .or. j.gt.200) goto 20
        enddo
        a=a2
        RETURN
      endif

20    call ERRNUM (151,0,'SPLNROOT',' ',0d0,0d0,0d0,ierr,herr)
      a=alow
      if (a3.gt.aupp) a=aupp
      end                                            subroutine SPLNROOT
c
c ======================================================================
c
      subroutine SPLNVAL (isp,iderv,z,a,f,ierr,herr)
c
c  Calculates the value of a spline or the derivative of the spline
c  at the specified value.
c
c  Inputs:
c      isp--Indicator for which spline to use:
c           0    - Molar density of the known phase
c        1 to nc - Composition of the incipient phase
c           nc+1 - Temperature
c           nc+2 - Pressure
c           nc+3 - Molar density of the equilibrium phase
c           nc+4 - Enthalpy
c           nc+5 - Entropy
c    iderv--Values of -1 and -2 return lowest and highest density values
c           of the splines, a value of 0 returns spline function value,
c           a value of 1 returns the derivative of the spline with
c           respect to the input value, and a value of 2 returns the 2nd derivative.
c        z--Composition array (array of mole fractions)
c        a--Input value (molar density of the known phase)
c
c  Outputs:
c        f--Desired output value
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-20-11 EWL, original version
c  04-01-15 EWL, check if spline value for the independent variable is descending for max and min values

      include 'COMMONS.INC'
      dimension z(ncmax)
      character herr*255
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SPLNVAL

      if (z(1).gt.0) ierr=0            !z is not yet used, this line keeps the compiler happy.
      ierr=0
      herr=' '
      f=300
      if (nArr.lt.2) then
        call ERRNUM (631,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif
      if (iderv.eq.-1 .or. iderv.eq.-2) then
        f =EXP4(xArr(1,0))
        f1=EXP4(xArr(nArr,0))
        if (f1.lt.f .and. iderv.eq.-1) f=f1
        if (f1.gt.f .and. iderv.eq.-2) f=f1
      elseif (iderv.eq.0) then
        if (a.le.0) RETURN
        call CSPLNVAL(isp,LOG(a),b,i,herr)
        f=b
        if (isp.le.nc+3) f=EXP3(b)               !All values except h and s are on a log basis.
      else
        da=0.001d0                     !Numerical derivative with dx=0.001.
        if (a-da.gt.0.) then
         call CSPLNVAL(isp,LOG(a+da),bp,i,herr)
         call CSPLNVAL(isp,LOG(a   ),b0,i,herr)
         call CSPLNVAL(isp,LOG(a-da),bn,i,herr)
        else
         if (a.le.0) RETURN
         call CSPLNVAL(isp,LOG(a+2d0*da),bp,
     &                  i,herr)
         call CSPLNVAL(isp,LOG(a+da),b0,i,herr)
         call CSPLNVAL(isp,LOG(a),bn,i,herr)
        endif
        ierr=i
        if (isp.le.nc+3) then
          if (iderv.eq.1) f=(EXP4(bp)-EXP4(bn))/(2d0*da)
          if (iderv.eq.2) f=(EXP4(bp)+EXP4(bn)-2d0*EXP4(b0))/da**2
        else
          if (iderv.eq.1) f=(bp-bn)/(2d0*da)
          if (iderv.eq.2) f=(bp+bn-2d0*b0)/da**2
        endif
      endif
      end                                             subroutine SPLNVAL
c
c ======================================================================
c
      subroutine ROOTFIND(hfunc,ibt,jbt,b,z,kph,iderv,itd,iib,ilg1,ilg2,
     &                    T,D,ot,ierr,herr)
c
c  General root finding calculation given two inputs and composition.
c  The routine iterates on density (when used as the second variable)
c  to find the value sent in variable b.
c  Generally the variable a is the temperature.
c  An initial guess must be sent in the variable that is being solved.
c
c  Inputs:
c    hfunc--Identifier of routine that calculates function value for the root finder (character*2)
c           Common examples are
c           P  - pressure (use with inputs of T and P only to find D)
c           A  - residual Helmholtz energy given T and D
c           D1 - d(P)/d(D)     (use with inputs of T and D(guess) only to find D)
c           D2 - d^2(P)/d(D)^2 (use with inputs of T and D(guess) only to find D)
c           E, H, or S - Energy, enthalpy, or entropy
c           ML - Melting line
c           Other routines not working yet
c      ibt--When hfunc is E, H, S, or P, leave hfunc blank and set
c           ibt to the enumerated values iLetterE, iLetterH, iLetterS, or iLetterP.
c      jbt--Integer that may be required in the function calls below
c           to identify which property to use (same values as described in ibt).
c        b--Value of the function at the desired T or D.
c           For finding maximums or minimums, use 0d0.
c        z--Overall (bulk) composition (array of mole fractions)
c      kph--Phase flag:
c               1 - Liquid
c               2 - Vapor
c               3 - Sublimation (not yet necessary below)
c               4 - Melting line (not yet necessary below)
c    iderv--Derivative flag:
c               0 - Find root of function
c               1 - Find maximum or minimum of function
c               2 - Find zero curvature of function
c      itd--T/D Input flag:
c               0 - Search for density
c               1 - Search for temperature
c      iib--Initial bounds flag:
c               0 - Do nothing
c              11 - Bound T between 0 and Tc and D between Dtrpl and Dc  (Sat. liquid densities)
c              12 - Bound T between 0 and Tc and D between 0 and Dc      (Sat. vapor densities)
c              21 - Bound T between 0 and Tc and E, H, or S between values at the triple point and at Tc  (Sat. E, H, or S)
c               1 - Bound T between 0 and Tc and P between 0 and Pc      (Vapor pressures)
c               3 - Bound between Ttrp and a very large number           (Melting pressures)
c               4 - Bound between 0 and Ttrp                             (Sublimation pressures)
c     ilg1--LOG flag:
c               0 - Do nothing
c               1 - Take the LOG of the input value
c               2 - Take the reciprocal of the input value (e.g., 1/T)
c     ilg2--LOG flag:
c               0 - Do nothing
c               1 - Take the LOG of the calculated value
c               2 - Take the reciprocal of the calculated value (e.g., 1/T)
c
c        T--Initial guess for temperature [K]
c        D--Initial guess for density [mol/L]
c
c  Outputs:
c        T--Temperature [K]
c        D--Density [mol/L]
c       ot--Actual value of the function at T and D
c     ierr--Error flag:  0 - Successful
c                      601 - Input function not available
c                      151 - Iteration failed to converge
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  05-29-13 EWL, original version

      include 'COMMONS.INC'
      character hfunc*(*),herr*255
      dimension z(ncmax),cc(0:2),v(20),f(20)
c     dimension xgrs(3)

      if (kph.gt.0) ierr=0   !kph is not yet used, this line keeps the compiler happy.
      ierr=0
      herr=' '
      itmax=50
      tolr=1d-10
      idv=0
      jr=1
      Tin=T
      pwat=0d0
      TD=0d0
      TD0=0d0
      v3=0d0
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      if (ibt.eq.0 .and. LEN(hfunc).ge.3) then
        if (hfunc(3:3).eq.'A') call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)
      endif
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.
      iw=0
      if (iFluidID(icomp).eq.1) iw=1
      if (iFluidID(icomp).eq.3) iw=3

c  For vapor pressure, saturated densities, etc., set up initial bounds to keep the solver on the saturation line.
      if (iib.eq.11 .or. iib.eq.12 .or. iib.eq.21) then
        v2=Tc
        f2=Dc+1d-8
        if (iib.eq.21) call HESCALC (ibt,Tc,Dc,z,f2)
      elseif (iib.eq.1) then
        v2=Tc
        f2=Pc+1d-8
      elseif (iib.eq.3) then
        TD0=Tmin
        Twat=-999
        pwat=0d0
        if (iw.gt.0) then
          if (iw.eq.1) call MLTH2O (Twat,pwat,pp)
          if (iw.eq.3) call MLTD2O (Twat,pwat,pp)
          if (lEq0(b-Pwat)) then
            T=Tmin
            goto 500
          endif
        endif
        v(2)=1d5             !Add a monster value to bound the melting line solution.
        f(2)=1d15
        if (iw.ge.1 .and. b.lt.pwat) then
          v(2)=Ttpn(icomp)-TD0
          f(2)=Ptpn(icomp)-b
        endif
        call ROOTSEARCH (1,0,0,v,f,TD,0,ib,lneg,lpos)
        v2=0
        f2=1d-8
        if (b.lt.f2) f2=b*0.9999999999d0         !Keep f2 below b for melting pressures.
        if (iw.ge.1) f2=pwat           !If water, use 251 K and its melting pressure.
      elseif (iib.eq.4) then
        v(2)=1000.           !Equal to 1 K with ilg1=1
        f(2)=LOG(1d-25)      !Add a microscopic value to bound the sublimation line solution.
        call ROOTSEARCH (1,0,0,v,f,TD,0,ib,lneg,lpos)
        call SETPNTS (1,icomp,z,v2,f2,dl,dv,ierr,herr)
        if (icomp.eq.0) call SUBLT (v2,z,f2,ierr,herr)     !SETPNTS returns a liquid/vapor point, which does not work for mixture sublimation points.
        if (b.gt.f2) f2=b*1.0000000001d0    !Keep f2 above b for sublimation pressures.
      endif
      if (iib.ne.0) then
        bb=b
        if (bb.le.1d-100 .and. ilg2.eq.1) bb=1d-100
        if (v2.le.1d-100 .and. ilg1.gt.0) v2=1d-100
        if (f2.le.1d-100 .and. ilg2.eq.1) f2=1d-100
        if (f(2).le.1d-100 .and. ilg2.eq.2) f(2)=1d-100
        v(2)=v2
        f(2)=f2-bb
        if (ilg1.eq.1) v(2)=XLOG(v2)
        if (ilg2.eq.1) f(2)=XLOG(f2/bb)
        if (ilg1.eq.2) v(2)=1000d0/v2
        if (ilg2.eq.2) f(2)=1000d0/f(2)
        call ROOTSEARCH (1,0,0,v,f,TD,0,ib,lneg,lpos)
        T=T*0.99d0
        jr=1234
      endif

      c=0d0
      do it=1,itmax
        do id=2,0,-1
          if (id.eq.0 .or. iderv.ne.0) then
            dd=0.0001d0
            Di=D
            Ti=T
            if (itd.eq.0) then
              if (id.eq.1) Di=D+dd          !Set up variables for obtaining derivatives of the function with
              if (id.eq.2) Di=D-dd          ! respect to either density (here), or temperature (a few lines below).
            else
              if (id.eq.1) Ti=T+dd
              if (id.eq.2) Ti=T-dd
            endif

c  Determine which function the user has requested and evaluate that function
c   at the current temperature and density.
            if (hfunc.eq.'HLA') then        !Liquid enthalpy from ancillary
              call DLSATT (Ti,z,D,ierr,herr)
              call THERM1 (Ti,D,z,p,e,c,s,g)
            elseif (hfunc.eq.'ELA') then    !Liquid energy from ancillary
              call DLSATT (Ti,z,D,ierr,herr)
              call THERM1 (Ti,D,z,p,c,h,s,g)
            elseif (hfunc.eq.'SLA') then    !Liquid entropy from ancillary
              call DLSATT (Ti,z,D,ierr,herr)
              call THERM1 (Ti,D,z,p,e,h,c,g)
            elseif (ibt.eq.iLetterE) then
              call THERM1 (T,Di,z,p,c,h,s,g)
            elseif (ibt.eq.iLetterH) then
              call THERM1 (T,Di,z,p,e,c,s,g)
            elseif (ibt.eq.iLetterS) then
              call THERM1 (T,Di,z,p,e,h,c,g)
            elseif (ibt.eq.iLetterP) then   !Pressure
              call PRESS (T,Di,z,c)
              call DPDDX (T,Di,z,dcdb)
              call DPDD2 (T,Di,z,dcdb2)
              idv=2                    !Set to 2 since analytical derivatives are known.
              if (it.eq.1) tolr=1d-10
            elseif (hfunc.eq.'PVA') then    !Vapor pressure from ancillary
              call PSATT  (Ti,z,c,ierr,herr)
            elseif (hfunc.eq.'PLA') then    !Liquid pressure from ancillary (for pseudo-pure fluids)
              call PSATK (-icomp,Ti,c,ierr,herr)
            elseif (hfunc.eq.'PMA') then    !Melting pressure
              call MELTT  (Ti,z,c,ierr,herr)
              if (iw.ge.1 .and. b.lt.pwat) then
                if (Ti.gt.Ttpn(icomp)) then
                  Ti=thrd*v(2)+thrd2*Ttpn(icomp)
                  if (Ti.lt.Tmin) Ti=0.05d0*v(2)+0.95d0*Ttpn(icomp)
                  T=Ti
                endif
                if (iw.eq.1) call MLTH2O (Ti,P,c)
                if (iw.eq.3) call MLTD2O (Ti,P,c)
              endif
            elseif (hfunc.eq.'PSA') then    !Sublimation pressure
              call SUBLT  (Ti,z,c,ierr,herr)
            elseif (hfunc.eq.'DLA') then    !Liquid density from ancillary
              call DLSATT (Ti,z,c,ierr,herr)
            elseif (hfunc.eq.'DVA') then    !Vapor density from ancillary
              call DVSATT (Ti,z,c,ierr,herr)
            elseif (hfunc.eq.'D1') then     !d(P)/d(D)
              call DPDDX (T,Di,z,c)
            elseif (hfunc.eq.'D2') then     !d^2(P)/d(D)^2
              call DPDD2 (T,Di,z,c)
            elseif (hfunc.eq.'A') then
c             This can be used for any function stored in the fluid file as a
c             function of 2 inputs that were used in fitting the arbitrary function
              tau=Tc/T
              del=Di/Dc
              c=PHIFEQ (1,0,0,tau,del)
            elseif (hfunc.eq.'ML') then     !Density or other property along the melting line.
              call MELTT (Ti,z,P,ierr,herr)
              call TPRHO (Ti,P,z,1,0,D,ierr,herr)
              call THERM1 (Ti,D,z,pp,e,h,s,g)
              c=D
              if (jbt.eq.iLetterP) c=P
              if (jbt.eq.iLetterE) c=e
              if (jbt.eq.iLetterH) c=h
              if (jbt.eq.iLetterS) c=s
c           elseif (hfunc.eq.'A8') then
c             call PRESS (T,Di,z,P1)
c             call DPDDX (T,Di,z,dpd1)
c             call SETEOSMX ('AGA')
c             call SETAGA (ierr,herr)
c             call PRESS (T,Di,z,P2)
c             call SETEOSMX ('HMX')
c             call UNSETAGA
c             c=ABS(P1-P2)/P1*100d0         !%dev in P
c             c=ABS(P1-P2)/Di*100d0/dpd1    !%dev in Z or D
c           elseif (hfunc.eq.'GR') then
c             call PRESS (T,Di,z,P1)
c             call DPDDX (T,Di,z,dpd1)
c             call GrossVar(Nc,z,xGrs,xHv,xHCH)
c             call PGross(T,Di,xGrs,xHCH,P2,Z)
c             c=ABS(P1-P2)/Di*100d0/dpd1    !%dev in Z or D
c           elseif (hfunc.eq.'G8') then
c             call SETEOSMX ('AGA')
c             call PRESS (T,Di,z,P1)
c             call DPDDX (T,Di,z,dpd1)
c             call SETEOSMX ('HMX')
c             call GrossVar(Nc,z,xGrs,xHv,xHCH)
c             call PGross(T,Di,xGrs,xHCH,P2,Z)
c             c=ABS(P1-P2)/Di*100d0/dpd1    !%dev in Z or D
            else
              call ERRNUM (601,0,'ROOTFIND',hfunc,0d0,0d0,0d0,ierr,herr)
            endif
            cc(id)=c
          endif
        enddo

c  Calculate slope or curvature if searching for maximums, minimums, or zero curvature.
        ot=c
        if (iderv.eq.1) then
          c=(cc(1)-cc(2))/(2d0*dd)
        elseif (iderv.eq.2) then
          c=(cc(1)+cc(2)-2d0*cc(0))/dd**2
        endif

        TD=D
        if (itd.eq.1) TD=T-TD0

c  Calculate delta(D) or delta(T) with analytical derivatives and Newton's method.
        if (idv.eq.2) then
          fnewt=0.1d0
          if (fnewt.gt.TD) fnewt=TD/2d0
          fnewt=1d0/(dcdb2/2d0/dcdb-dcdb/(c-b))            !2nd order Newton's method.
          if (ABS(fnewt).gt.1.) fnewt=-(c-b)/dcdb          !If 2nd order Newton's method gives unreasonable result, revert to first order method.
          TD=TD+fnewt

c  Analytical derivatives not known, use Secant and Reguli-falsi root searching techniques.
        else
          bb=b
          if (bb.le.1d-100 .and. ilg2.eq.1) bb=1d-100
          if (c .le.1d-100 .and. ilg2.eq.1)  c=1d-100
          if (f(2).le.1d-100 .and. ilg2.eq.2) f(2)=1d-100
          if (TD.le.0) TD=300d0
          v(2)=TD
          f(2)=c-bb
          if (ilg1.eq.1) v(2)=XLOG(TD)
          if (ilg2.eq.1) f(2)=XLOG(XDIV(c,bb))
          if (ilg1.eq.2) v(2)=XDIV(1000d0,TD)
          if (ilg2.eq.2) f(2)=XDIV(1000d0,f(2))
          call ROOTSEARCH (jr,0,22,v,f,TD,it,ib,lneg,lpos)           !Get next guess for the independent variable.
          if (ilg1.eq.1) TD=EXP3(TD)
          if (ilg1.eq.2) TD=XDIV(1000d0,TD)
          if (jr.eq.1) TD=TD*1.01d0    !Next step, but only for the first iteration.
          jr=1234
          fnewt=f(2)
        endif

        if (itd.eq.0) then
          D=TD
          if (D.gt.100) fnewt=1d6                !Don't exit yet if D>100
        else
          T=TD+TD0
          if (T.lt.0.1d0) T=Tin+(-1d0)**it*DBLE(it)*5d0
        endif
        call TOLRNC (tolr,it,12,18,24,30,35)     !Loosen tolerance if not converging.
        if (TD.gt.0d0) then
          if (ABS(fnewt/(TD+TD0)).lt.tolr) goto 500        !Iteration has converged.
        endif
      enddo
      call ERRNUM (151,0,'ROOTFIND',' ',0d0,0d0,0d0,ierr,herr)
      if (itd.eq.1) T=300d0
 500  continue
      end                                            subroutine ROOTFIND
c
c ======================================================================
c
      subroutine ROOTSEARCH (iFlag,ipn,iOpt,v,f,v3,it,ib,lneg,lpos)
c
c  Code for saving and locating roots that are used in other iterative routines.
c
c  Inputs:
c    iFlag--Flag to determine the code in this routine that should be called:
c           0 - Initialize all variables; should be done at the beginning of
c               each iteration.  Do not set the minimum and maximum values
c               (slots 6 and 7 in the v array) before calling this option.
c           1 - Store the values of v(2) and f(2) as the new bounds when they
c               are closer to the root (or when the bounds have not yet been set).
c           2 - Calculate the next root based on the Secant method.
c           3 - Use Reguli-falsi if Secant method has yielded a guess further from the solution.
c           4 - Check if the new value in v3 is within the bounds [contained in v(6) and v(7)].
c       5,6,7 - Use bisection or other fixed ratios to get the next root.
c             - A value of 5 is the default, only perform the bisection if out of bounds (and bounds are known).
c             - If 6, then perform the bisection even if the root is inside the bounds.
c             - If 7, use the max and min only, not the current bounds.
c          11 - Expand bounds slightly by the value contained in v3.
c        1234 - Implement options 1, 2, 3, and 4.
c      ipn--If set to zero, do not allow negative roots (such as for density).
c     iOpt--Flag to turn on or off certain features.
c        v--Array of input values used to search for the root.
c        f--Array of calculated values at the states given in the v array.
c
c  Outputs:
c       v3--Next guess in the iteration, which should be assigned to the v(2) spot in the calling routine if used.
c       ib--Internal variable used only in this code, but needs to be saved between calls.
c     lneg--Logical variable indicating if a negative function value has been located.
c     lpos--Logical variable indicating if a positive function value has been located.
c
c  The arrays v and f contain the input state in v and the function value in f.  The slots are
c        v(1),f(1) - The values of v(2) and f(2) are loaded into this slot at the end of this
c                    routine, and then used later in the Secant method to determine the next
c                    root.  When the first calculated point is determined, call this routine
c                    with iFlag=1 to load this the initial point (but put that point in slot2).
c        v(2),f(2) - Contains the input and calculated points.  The calling routine should load
c                    these variables before calling this routine.
c        v(3),f(3) - The contents of v(1) and f(1) are moved into this slot at the end of this
c                    routine. They are not currently used, but saved for other possible use.
c        v(4),f(4) - The location and value at the lower end of the bounds. (Set in this routine.)
c        v(5),f(5) - The location and value at the upper end of the bounds.
c        v(6)      - Minimum value of v that is allowed.   The calling routine should set this.
c        v(7)      - Maximum value of v that is allowed.
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-11-16 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension iFlags(0:9),v(20),f(20),xPnts(20),yPnts(20)
      dimension xPdl(1000),yPdl(1000),Cfdl(1000)

c  Initialize all variables sent to the routine when a new iteration begins.
      if (iFlag.eq.0) then
        v=0d0
        f=0d0
        lneg=.false.
        lpos=.false.
        f(4)=-1d20
        f(5)= 1d20
        f(8)=-1d20
        f(9)= 1d02
        ib=0
        RETURN
      endif

c  Set an array of flags from the input iFlag variable.
      iFlags=0
      if (iFlag.lt.10) iFlags(iFlag)=1
      if (iFlag.eq.1234) then          !Code to indicate that methods 1, 2, 3, and 4 should all be called.
        iFlags(1)=1
        iFlags(2)=1
        iFlags(3)=1
        iFlags(4)=1
      endif
      v3old=v3

c  Store upper and lower bounds if new root is inside the old roots.
      if (iFlags(1).eq.1) then
        v(11:19)=v(10:18)    !Save old values
        f(11:19)=f(10:18)
        v(10)=v(2)
        f(10)=f(2)
        if (f(20).ge.1.) f(20)=f(20)+1d0         !Flag
        if (f(20).ge.6.) f(20)=0d0
        if (f(2).lt.0. .and. f(2).gt.f(4)) then
          lneg=.true.
          v(4)=v(2)
          f(4)=f(2)
          if (iFlag.eq.1) v(8)=v(4)    !Store the original values.
          if (iFlag.eq.1) f(8)=f(4)
        elseif (f(2).gt.0. .and. f(2).lt.f(5)) then
          lpos=.true.
          v(5)=v(2)
          f(5)=f(2)
          if (iFlag.eq.1) v(9)=v(5)    !Store the original values.
          if (iFlag.eq.1) f(9)=f(5)
        endif

c  Check if calculations have stalled and caused the bounds to no longer bound the root.
c   A check is first made to see if the difference in the independent variable
c   between two successive calls is nearly identical, as well as that from the
c   previous calls to get the function values (stored in f(zzz6) or f(zzz7)).
c   If they are nearly the same, but the function value is not converging towards zero,
c   the bounds are expanded by 2%.
c  Use similar logic to that above, but over a wider range of calculated values.
        if (it.gt.20 .and. iFlag.gt.1 .and. f(20).eq.0.) then
          if (ABS(f(2)).gt.1.d-5 .and. ABS(f(1)).gt.1.d-5) then      !Only make the checks if the objective function is not going to zero.
            ifl=0
            do i=0,4
              k=i+10
              if (i.eq.0) k=2
              if (ABS(f(k)).gt.1.d-10) then
                if (ABS(f(k)).lt.1d-4) ifl=ifl-2           !Subtract two from ifl if the objective function is small.
                do j=i+1,5
                  k1=j+10
                  if (i.eq.0) k1=j+10
                  den=f(k1)
                  if (ABS(f(k1)).gt.ABS(f(k))) den=f(k)    !Check for minimal changes in the previously stored values.
                  den=xEq0(den)
                  dev=ABS((f(k)-f(k1))/den)
                  dmx=10d0
                  if (iOpt.eq.20) dmx=1d0
                  if (iOpt.eq.21) dmx=0.1d0
                  if (iOpt.eq.22) dmx=0.01d0
                  if (iOpt.eq.25) dmx=0d0
                  if (dev.lt.dmx) ifl=ifl+1      !Add one to ifl if little change happened.
                  if (dev.gt.dmx) ifl=ifl-1      !Subtract one for a large change.
                enddo
              endif
            enddo
            if (ifl.gt.0.) then        !If number of small change more than big changes, assume it is stuck.
              f(20)=1d0      !Flag to allow this section to be implemented only after three iterations from the last one.
              if (f(2).gt.0 .and. it.gt.15 .and. it.lt.20) then
                lpos=.false.           !Solver is stuck, remove the positive bound.
                v(5)=0
                v(9)=0
                f(5)=1d20
                f(9)=1d20
              elseif (f(2).gt.0. .and. f(9).lt.1.d19) then
                v(5)=v(9)              !Solver is stuck, go back to the original bounds.
                f(5)=f(9)
              endif
              if (f(2).lt.0 .and. it.gt.15 .and. it.lt.20) then
                lneg=.false.           !Solver is stuck, remove the negative bound.
                v(4)=0
                v(8)=0
                f(4)=-1d20
                f(8)=-1d20
              elseif (f(2).lt.0 .and. f(8).gt.-1.d19) then
                v(4)=v(8)              !Solver is stuck, go back to the original bounds.
                f(4)=f(8)
              endif
              if (it.gt.15) iFlags(6)=1          !Use bisection to get the next root.
            endif
          endif
        endif
       endif

c  Calculate next root based on the Secant method
      if (iFlags(2).eq.1) then
        if (ABS(f(2)-f(1)).lt.1d-12) then        !Check for divide by zero.
          iFlags(6)=1        !Use different bisection methods to get the next
          iFlags(3)=0        ! root when the function values are nearly the same.
        else
          if (ABS(v(2)).lt.0.1d0 .and. ABS(v(1)).lt.0.1d0 .and.
     &            v(2).gt.0. .and. v(1).gt.0.) then                  !Find next guess based on a LOG scale for very low input values (usually densities, but perhaps 1/T).
            v2l=XLOG(v(2))
            v2i=f(2)*XLOG(v(2)/v(1))/(f(2)-f(1))
            if (ABS(v2i).gt.ABS(v2l/2d0)) v2i=SIGN(v2l/2d0,v2i)
            ee=v2l-v2i
            if (ee.lt.10.) then
              v3=EXP3(ee)
              if (ABS(v3).lt.1d-20) v3=v(2)+(v(2)-v(1))/2d0          !Avoid extremely small values.
            else
              iFlags(6)=1              !The value is very large, use bisection instead.
              iFlags(3)=0
            endif
          elseif (ABS(v(2)).gt.1.d-20) then
            vinc=-f(2)*(v(2)-v(1))/(f(2)-f(1))
            if(ABS(vinc).gt.0.5d0 .and. ABS(vinc).gt.ABS(v(2)-v(1))*2d0)       !Check for large increments.
     &        vinc=SIGN(v(2)-v(1),vinc)*2d0
            if (ABS(vinc/v(2)).lt.0.75d0) vinc=vinc*0.75d0
            v3=v(2)+vinc
            if (v3.lt.0. .and. ipn.eq.0) v3=v(2)*0.8d0     !Do not let input variable go negative.
          endif
        endif
      endif

c  Check if secant method has yielded a guess further from the solution.  If so, use Reguli-falsi.
      if (iFlags(3).eq.1) then
        if (lneg .and. lpos) then
          if ((v(5).gt.0. .and. v(4).gt.0.) .or. ipn.eq.1) then
            ixg=1
            if (ABS(f(5)-f(4)).lt.1d-12) ixg=0   !Check for divide by zero.
            if (v3.gt.MAX(v(5),v(4)) .or. v3.lt.MIN(v(5),v(4))) then
              if (ixg.eq.1) then
                if(ABS(v(2)).lt.1. .and. v(5).gt.0. .and. v(4).gt.0.
     &             .and. v(5).lt.1. .and. v(4).lt.1d-19) then
                v3=EXP3(XLOG(v(5))-f(5)*XLOG(v(5)/v(4))/(f(5)-f(4)))           !Find next guess based on a LOG scale for very low densities.
                else
                  v3=v(5)-f(5)*(v(5)-v(4))/(f(5)-f(4))
                endif
              else
                v3=v(5)+(v(5)-v(4))/2d0
              endif
              if (ABS(v3old-v3).lt.1.d-8) iflags(6)=1
            endif
          endif
        endif
      endif

c  Keep new value (v3) between v(6) and v(7) unless the min and max values have not been set.
c   Both values have to be set for this to work properly since a value of zero is valid for the minimum.
      if (iFlags(4).eq.1 .and. ABS(v(6)-v(7)).gt.1d-10) then
        amin=v(6)
        amax=v(7)
        if (lneg .and. lpos) then
          amin=MIN(v(4),v(5))
          amax=MAX(v(4),v(5))
        endif
        if (v3.gt.v(7)) then
          v3=(amax+v(7))/2d0
          if (ABS(amax-v(7)).lt.1d-5) v3=(amin+amax)/2d0
          ib=ABS(ib)+1
          if (ib.gt.3) iFlags(6)=1
        elseif (v3.lt.v(6)) then
          v3=(amin+v(6))/2d0
          if (ABS(amin-v(6)).lt.1d-5) v3=(amin+amax)/2d0
        endif
        if (ABS(v3-v(2)).lt.1d-12.and.ABS(v3).gt.1.d-7.and.iOpt.ne.11)
     &     iFlags(6)=1
      endif

c  Use bisection or other fixed ratios to get next root.
      if (iFlags(5).eq.1 .or. iFlags(6).eq.1 .or. iFlags(7).eq.1) then
        ixg1=0
        ixg2=0
        ixg3=0
        if (ABS(v(6)-v(7)).lt.1d-10) ixg1=1                !ixg1=0 specifies that the min and max values have been set.
        if (.not.lneg .or. .not.lpos) ixg2=1               !ixg2=0 specifies if the objective function limits have been found.
        if (ixg2.eq.0 .and. ABS(f(2)).gt.0.01d0) then                          !Only perform the bisection if iFlags(6) is on or the
          if (f(2).lt.f(4)*1.001d0 .or. f(2).gt.f(5)*1.001d0) ixg3=1           ! calculated objective function is outside the limits.
        endif                                                                  !ixg3=1 specifies that the current root is out of bounds.
        if (ixg3.eq.1 .or. iFlags(6).eq.1 .or. iFlags(7).eq.1) then
          if (ixg1.eq.1 .and. ixg2.eq.1) then
            if (ib.eq.0) ib=-1
            ib=-ib-ib/ABS(ib)                    !Change ib from 2,-3,4,-5,6,etc.
            if (ABS(ib).gt.20) ib=0
            v3=v3*(1d0-DBLE(ib)/100d0)           !If no limits or bounds have been stored, just go 1% higher or lower for each value of ib.
          else
            vposx=v(5)
            vnegx=v(4)
            if (iFlags(7).eq.1) ixg2=1           !For input of iFlags=7, use max and min values only.
            if (ixg2.eq.1 .and. ixg1.eq.0) then
              vposx=v(6)
              vnegx=v(7)
            endif
            ilog=0
            if (vposx.gt.0 .and. vnegx.gt.0) then
              ilog=1
              vposx=LOG(vposx)
              vnegx=LOG(vnegx)
            endif
            ib=ABS(ib)+1
            if (ib.gt.10) ib=1
            if (ib.eq.1)  v3=(     vposx+ 2d0*vnegx)/3d0
            if (ib.eq.2)  v3=( 2d0*vposx+     vnegx)/3d0
            if (ib.eq.3)  v3=(     vposx+ 5d0*vnegx)/6d0
            if (ib.eq.4)  v3=( 5d0*vposx+     vnegx)/6d0
            if (ib.eq.5)  v3=(     vposx+11d0*vnegx)/12d0
            if (ib.eq.6)  v3=(11d0*vposx+     vnegx)/12d0
            if (ib.eq.7)  v3=(     vposx+23d0*vnegx)/24d0
            if (ib.eq.8)  v3=(23d0*vposx+     vnegx)/24d0
            if (ib.eq.9)  v3=(     vposx+99d0*vnegx)/100d0
            if (ib.eq.10) v3=(99d0*vposx+     vnegx)/100d0
            if (ilog.eq.1) v3=EXP(v3)
          endif
        endif
      endif

c  If the root has been bounded, use spline curves to determine the next point from the
c    previous points already calculated.  The variable np2 specifies the number of points required.
      np2=4
      if (iFlags(8).eq.1 .and. it.gt.np2)  then
        if (lneg .and. lpos) then
          np=1
          xPnts(np)=v(10)    !Old points were saved in the v and f arrays starting in the 10th slot.
          yPnts(np)=f(10)
          do i=2,np2+2
            ii=i+9
            if (i.eq.np2+1) ii=4       !Add the upper and lower bounds to the points being fitted.
            if (i.eq.np2+2) ii=5
            ix=1
            do j=1,np
              if (ABS(xPnts(j)-v(ii)).lt.1d-8) ix=0        !Search for duplicate points and don't add them.
            enddo
            if (ix.eq.1 .and. v(ii).ne.0d0) then
              np=np+1
              xPnts(np)=v(ii)          !Add a new point to the vector.
              yPnts(np)=f(ii)
            endif
          enddo

          do i=1,np          !Sort the y-array data (keeping the x-array in line).
            k=i
            do j=1,np
              if (yPnts(j).lt.yPnts(k)) k=j
            enddo
            xpdl(i)=xPnts(k)
            ypdl(i)=yPnts(k)
            yPnts(k)=1d20
          enddo

c  bbc - call csplnval to set this
          if (np.gt.3) then            !Continue only if there are four or more points.
            call CSPLINE  (np,ypdl,xpdl,cfdl)    !Generate the splines with the x data as a function of y.
            call CSPLNVAL2 (np,ypdl,xpdl,cfdl,0d0,bbc,i,herr)        !Calculate the zero location of the splines (where the y-data pass through the zero), thus locating the next root.
            vmin=MIN(v(4),v(5))
            vmax=MAX(v(4),v(5))
            v3dif=MIN(ABS(v(4)-v3),ABS(v(5)-v3))
            if (bbc.gt.vmin .and. bbc.lt.vmax) then                  !Check if the root is within the bounds, and if so, use it.
              v3=bbc
            elseif (bbc.lt.vmin .and. vmin-bbc.lt.v3dif) then        !If not, but if closer to a bound than the value predicted elsewhere in this routine,
              v3=vmin+((vmin-bbc)+v3dif)/2d0                         ! use the average of the distance between these points and the bound.
            elseif (bbc.gt.vmax .and. bbc-vmax.lt.v3dif) then
              v3=vmax-((bbc-vmax)+v3dif)/2d0
            else
              do i=1,np-1
                if (ypdl(i)*ypdl(i+1).lt.0d0) then
                  xz=ypdl(i+1)/(ypdl(i+1)-ypdl(i))
                  v3=xpdl(i)*xz+xpdl(i+1)*(1d0-xz)
                  EXIT
                endif
              enddo
            endif
          endif
        endif
      endif

c  Expand bounds slightly by the value contained in v3.
      if (iFlag.eq.11) then
        vi=1
        if (v(4).lt.v(5)) vi=-1
        if (lneg) then
          f(4)=f(4)*v3
          v(4)=v(4)+vi*ABS(v(4)*v3/1000d0)
        endif
        if (lpos) then
          f(5)=f(5)*v3
          v(5)=v(5)-vi*ABS(v(5)*v3/1000d0)
        endif
      endif



c  Code that could be used to find a root given three states, not currently used or tested.
c     if (iFlags(zzz).eq.1) then
c       xPnts(1:3)=v(1:3)
c       yPnts(1:3)=f(1:3)
c       call MINP (3,xPnts,yPnts,xval,a,b,c,b1,b2,ierr,herr)
c       vv1=min(v(4),v(5))
c       vv2=max(v(4),v(5))
c       if (lneg .and. lpos) then
c         if (b1.gt.vv1 .and. b1.lt.vv2) v3=b1
c       endif
c     endif

 500  if (iFlags(1).eq.1 .or. iFlags(6).eq.1) then         !Move slots 2 and 3 to slots 1 and 2.
        v(3)=v(1)
        v(1)=v(2)
        f(3)=f(1)
        f(1)=f(2)
      endif

      end                                          subroutine ROOTSEARCH
c
c ======================================================================
c
      subroutine IDCRV (D,z,T,ierr,herr)
c
c  Calculate the temperature at the input density where the compressibility
c  factor crosses from less than 1 to greater than 1 (i.e., Z=1).  This
c  line starts at zero density at the temperature where B=0, and passes
c  into the liquid phase without crossing the two-phase.  The argument z
c  in this routine is an array with the mole fractions of the mixture.
c  If the input T is non-zero, it is used as the initial guess.
c
c  Inputs:
c        D--Density [mol/l]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr--Error flag:  0 - Successful
c                      151 - Iteration failed to converge
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character herr*255
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::IDCRV

      ierr=0
      herr=' '
      call REDX (z,Tred,Dred)
      if (T.eq.0.) T=Tred*1.5d0
c  New method that uses the slope dz/dT to get quick convergence with dT.
      zz=1d0
      do it=1,100
        call PRESS (T,D,z,P)
        zz=P/D/Rxgas/T-1d0
        da11=PHIX(1,1,Tred/T,D/Dred,z)
        dzdt=-da11/T
        if (ABS(dzdt).lt.1d-40) EXIT
        T=T-zz/dzdt
        if (T.le.0) EXIT
        if (ABS(zz).lt.1d-8) RETURN
      enddo
      T=300d0
      call ERRNUM (151,0,'IDCRV',' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine IDCRV
c
c ======================================================================
c
      subroutine BLCRV (D,z,T,ierr,herr)
c
c  Calculate the temperature along the Boyle curve for the input density.
c  This line starts at zero density at the temperature where B=0, and
c  passes into the liquid phase without crossing the two-phase.  It
c  ends at a saturated liquid state very close to the critical point.
c  The argument z in this routine is an array with the mole fractions
c  of the mixture.  If the input T is non-zero, it is used as the
c  initial guess.
c
c  Inputs:
c        D--Density [mol/l]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr--Error flag:  0 - Successful
c                      151 - Iteration failed to converge
c     herr--Error string (character*255)
c
      include 'COMMONS.INC'
      character herr*255
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::BLCRV

      ierr=0
      herr=' '
      call REDX (z,Tred,Dred)
      if (T.le.0) T=Tred*1.5d0
      func=0d0
      do it=1,100
        del=D/Dred
        tau=Tred/T
        phi01=PHIX(0,1,tau,del,z)
        phi02=PHIX(0,2,tau,del,z)
        phi11=PHIX(1,1,tau,del,z)
        phi12=PHIX(1,2,tau,del,z)
        func=phi01+phi02
        dfdt=-(phi11+phi12)/T
        if (ABS(dfdt).lt.1d-40) EXIT
        T=T-func/dfdt
        if (T.le.0) EXIT
        if (ABS(func).lt.1d-8) RETURN
      enddo
      T=300d0
      call ERRNUM (151,0,'BLCRV',' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine BLCRV
c
c ======================================================================
c
      subroutine JTCRV (D,z,T,ierr,herr)
c
c  Calculate the temperature along the Joule-Thomson curve for the input
c  density.  This line starts at zero density at the temperature where
c  the Joule-Thomson property (dH/dT) is zero, and passes into the
c  liquid phase without crossing the two-phase.  It ends at a saturated
c  liquid state far from the critical point.  The argument z in this
c  routine is an array with the mole fractions of the mixture.  If the
c  input T is non-zero, it is used as the initial guess.
c
c  Only the top part in the calculation of hjt is required, the other
c  parts do not go to zero and thus do not contribute to finding JT=0.
c
c  Inputs:
c        D--Density [mol/l]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr--Error flag:  0 - Successful
c                      151 - Iteration failed to converge
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character herr*255
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::JTCRV

      ierr=0
      herr=' '
      call REDX (z,Tred,Dred)
      if (T.eq.0) T=Tred*1.5d0
      do it=1,100
        tau=Tred/T
        del=D/Dred
        phi01=PHIX(0,1,tau,del,z)
        phi02=PHIX(0,2,tau,del,z)
        phi11=PHIX(1,1,tau,del,z)
        phi12=PHIX(1,2,tau,del,z)
        phi21=PHIX(2,1,tau,del,z)
        hjt=phi01+phi02+phi11
        dfdt=-(2d0*phi11+phi12+phi21)/T          !The derivative of phi11 with respect to T results in phi21+phi11, thus the '2d0*' is required below.
        if (ABS(dfdt).lt.1d-40) EXIT
        T=T-hjt/dfdt
        if (T.le.0) EXIT
        if (ABS(hjt).lt.1d-8) RETURN
      enddo
      T=300d0
      call ERRNUM (151,0,'JTCRV',' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine JTCRV
c
c =====================================================================
c
      subroutine JICRV (D,z,T,ierr,herr)
c
c  Calculate the temperature along the Joule-Inversion curve for the
c  input density.  This line starts at zero density at the temperature
c  where B is at a maximum, and passes into the liquid phase without
c  crossing the two-phase.  It ends at very high pressures.
c  The argument z in this routine is an array with the mole fractions
c  of the mixture.  If the input T is non-zero, it is used as the
c  initial guess.
c
c  JI is equal to
c  d(Z)/d(T) at constant D
c  del*d^2(alphar)/d(del)/d(T)
c  -del*tau*d^2(alphar)/d(del)/d(tau)/T (can ignore the /T for finding JI=0)
c
c  d(JI)/dT  -> tau**2*del*d^3(alphar)/d(del)/d(tau)**2/T**2
c               (One of the /T must be removed to match the one removed in the function.)
c
c  Inputs:
c        D--Density [mol/l]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c     ierr--Error flag:  0 - Successful
c                      151 - Iteration failed to converge
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character herr*255
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::JICRV

      ierr=0
      herr=' '
      call REDX (z,Tred,Dred)
      if (T.le.0) T=Tred*10d0
      do it=1,100
        func= PHIX(1,1,Tred/T,D/Dred,z)
        dfdt=-PHIX(2,1,Tred/T,D/Dred,z)/T
        if (ABS(dfdt).lt.1d-40) EXIT
        T=T-func/dfdt
        if (T.le.0) EXIT
        if (ABS(func).lt.1d-8) RETURN
      enddo
      T=300d0
      call ERRNUM (151,0,'JICRV',' ',0d0,0d0,0d0,ierr,herr)
      end                                               subroutine JICRV
c
c ======================================================================
c
      subroutine PTANC (T,P,q,b,ibt,Dl,Dv)
c
c  For a pseudo-pure fluid, find a saturated pressure that agrees with
c  calculations given temperature as an input.  This problem occurs since
c  the pseudo-pure fluid equations cannot calculate equilibrium phases.
c  Any given input temperature has an associated pbub and pdew.  At a
c  pressure between these two, there are associated values of tbub and tdew.
c  Thus, there are four state points that could be attributed to the two-phase
c  input.  To maintain consistency, the two points associated with tbub
c  and tdew are used through out Refprop, and an iterative solution is
c  required when P is one of the inputs to find the right state point.
c
c  Inputs:
c        T--Temperature [K] (output, but requires an initial guess)
c        P--Pressure [kPa] (input)
c        q--Vapor quality on a MOLAR basis [moles vapor/total moles]
c        b--Density, energy, enthalpy, or entropy, depending on ibt
c      ibt--Input of second property (Q, D, E, H, or S)
c
c  Outputs:
c        T--Temperature [K]
c       Dl--Molar density of the liquid phase [mol/L]
c       Dv--Molar density of the vapor phase [mol/L]
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  12-17-02 EWL, original version

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)

      T1=T*1.001d0
      P1=P*1.01d0
      i=0
      z(1)=1d0
 10   continue
        call SATT (T,z,1,pl,Dl,Dvbub,x,y,ierr,herr)
        call SATT (T,z,2,pv,Dldew,Dv,x,y,ierr,herr)
        if (ibt.eq.iLetterQ) then
        elseif (ibt.eq.iLetterD) then
          q=(1d0/b-1d0/Dl)/(1d0/Dv-1d0/Dl)
        else
          call THERM1 (T,Dl,z,pl,el,hl,sl,g)
          call THERM1 (T,Dv,z,pv,ev,hv,sv,g)
          if (ibt.eq.iLetterE) then
            if (ABS(ev-el).gt.1.d-10) q=(b-el)/(ev-el)
          elseif (ibt.eq.iLetterH) then
            if (ABS(hv-hl).gt.1.d-10) q=(b-hl)/(hv-hl)
          elseif (ibt.eq.iLetterS) then
            if (ABS(sv-sl).gt.1.d-10) q=(b-sl)/(sv-sl)
          endif
        endif
        pp=(1d0-q)*pl+q*pv
        dt=T1-T
        dp=P1-pp
        T1=T
        P1=pp
        if (ABS(dp).gt.1d-20) T=T+dt/dp*(P-pp)
        if (T.le.0) T=Tcrit(1)/2d0
        i=i+1
      if (i.lt.20 .and. ABS(dp).gt.1d-5) goto 10
      end                                               subroutine PTANC
c
c ======================================================================
c
      subroutine PASSCMN (hvr,iset,icomp,jcomp,
     &                    hstr,ilng,dbl,arr,ierr,herr)
c
c  Get or set values of variables in the common blocks.
c
c  Examples (in FORTRAN)::
c
c      call PASSCMN ('txeos',  0,3,0, h,i, tmx,z,   ierr,herr) ! get Tmax of component 3
c      call PASSCMN ('dxeos',  1,2,0, h,i, dmx,z,   ierr,herr) ! set Dmax of component 2
c      call PASSCMN ('tz',     0,1,0, h,i, Tc, z,   ierr,herr) ! get reducing temperature of component 1
c      call PASSCMN ('ntermf', 0,1,0, h,nt,v,  z,   ierr,herr) ! get number of terms in the Helmholtz equation for component 1
c      call PASSCMN ('coefhmx',1,1,0, h,i, v,  cf,  ierr,herr) ! set the coefficients in the Helmholtz equation for component 1
c      call PASSCMN ('acp0',   1,5,0, h,i, v,  cp0, ierr,herr) ! set the coefficients in the cp0 equation for component 5
c      call PASSCMN ('fPRkij', 1,1,2, h,i, v,  fpr, ierr,herr) ! set the PR coefficient for the 1,2 binary
c
c  Inputs:
c      hvr--Character string with the common variable's name
c     iset--Flag to indicate the get/set condition:
c             0 - Get variable value
c             1 - Set variable value
c    icomp--Component number
c    jcomp--Second component number for binary mixture variables
c     hstr--Input or output for a character string
c     ilng--Input or output for a long variable
c      dbl--Input or output for a double precision variable
c      arr--Input or output for a double precision array of length 100
c           Values in this array are not initialized to 0
c
c  Outputs:
c     hstr--Input or output for a character string
c     ilng--Input or output for a long variable
c      dbl--Input or output for a double precision variable
c      arr--Input or output for a double precision array
c     ierr--Error flag:  0 - Successful
c                      113 - Inputs out of bounds
c                      601 - Variable name not recognized
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-16-13 EWL, original version
c
      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      include 'COMANC.INC'
      character*255 herr,hvar,hstr,htemp,LCASE
      character hvr*(*)
      dimension arr(100),a(100)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PASSCMN

      ierr=0
      herr=' '
      l=.false.
      if (iset.eq.1) l=.true.

      ij=0
      ia=0
      v=0d0
      a=0d0
      htemp=' '
      if (.not.l) then
        hstr=' '
        ilng=0
        dbl=0d0
      endif

      hvar=hvr
      i=INDEX(hvr,CHAR(0))
      if (i.gt.1) hvar=hvr(1:i-1)
      i=INDEX(hvar,' ')
      if (i.gt.1) hvar=hvar(1:i-1)     !Get rid of anything beyond the first space since this will be sent from other programs.
      hvar=LCASE(hvar,i)

      if (ABS(icomp).gt.nq) then
        call ERRNUM (113,0,'PASSCMN',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      elseif (hvar.eq.'hdir') then     !hdir can vary from 1 to 41, not just 1 to 20 (assuming ncmax=20).
        htemp=hdir(icomp)
        if (l) hdir(icomp)=hstr
        goto 500
      endif

      if (ABS(icomp).gt.ncmax .or. ABS(jcomp).gt.ncmax) then
        call ERRNUM (113,0,'PASSCMN',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif


c  Character strings
      if (hvar.eq.'heos') then
        htemp=heosmx
        if (l) call SETEOSMX (hstr)
      elseif (hvar.eq.'hinpnme') then
        htemp=hinpnme(icomp)
        if (l) hinpnme(icomp)=hstr
      elseif (hvar.eq.'hpath') then
        htemp=hpath
        if (l) hpath=hstr

c  Long variables
      elseif (hvar.eq.'ntermf' .or. hvar.eq.'nfeqterms') then
        ij=nFEQterms(icomp)
        if (l) nFEQterms(icomp)=ilng
      elseif (hvar.eq.'ncrt' .or. hvar.eq.'nfeqcrt') then
        ij=nFEQcrt(icomp)
        if (l) nFEQcrt(icomp)=ilng
      elseif (hvar.eq.'ncp0poly') then
        ij=nCp0poly(icomp)
        if (l) nCp0poly(icomp)=ilng
      elseif (hvar.eq.'ncp0exp') then
        ij=nCp0Exp(icomp)
        if (l) nCp0Exp(icomp)=ilng
      elseif (hvar.eq.'ifluidid') then
        ij=iFluidID(icomp)
        if (l) iFluidID(icomp)=ilng
      elseif (hvar.eq.'mixtyp') then
        ij=MixTyp
        if (l) MixTyp=ilng
      elseif (hvar.eq.'ihmxbnc') then  !If 1, the hmx.bnc file is currently stored in memory.
        ij=0                           ! This option cannot be used to set any flags, just retrieve information.
        if (iRDbnc+iRDmxm.ge.2) ij=1
      elseif (hvar.eq.'lcache1') then
        if (lCache1) ij=1
        if (l) lCache1=ilng.ne.0
        call RESETA (0)
      elseif (hvar.eq.'lcache2') then
        if (lCache2) ij=1
        if (l) lCache2=ilng.ne.0
        call RESETA (0)
      elseif (hvar.eq.'lerrswt') then
        if (lErrSwt) ij=1
        if (l) lErrSwt=ilng.ne.0
        call RESETA (0)

c  Double precision variables
      elseif (hvar.eq.'accen') then
        v=accen(icomp)
        if (l) accen(icomp)=dbl
      elseif (hvar.eq.'acnpr') then
        v=acnPR(icomp)
        if (l) acnPR(icomp)=dbl
      elseif (hvar.eq.'credCp0') then
        v=credCp0(icomp)
        if (l) credCp0(icomp)=dbl
      elseif (hvar.eq.'dcrit' .or. hvar.eq.'rhocfeq' .or.
     &        hvar.eq.'rhocpr') then
        v=Dcrit(icomp)
        if (l) Dcrit(icomp)=dbl
      elseif (hvar.eq.'deos' .or. hvar.eq.'dxeos') then
        v=Dxeos(icomp)
        if (l) Dxeos(icomp)=dbl
      elseif (hvar.eq.'dipole') then
        v=dipole(icomp)
        if (l) dipole(icomp)=dbl
      elseif (hvar.eq.'dtpn') then
        v=Dtpn(icomp)
        if (l) Dtpn(icomp)=dbl
      elseif (hvar.eq.'dtpv') then
        v=Dtpv(icomp)
        if (l) Dtpv(icomp)=dbl
      elseif (hvar.eq.'dzfeq' .or.
     &        hvar.eq.'d0feq' .or. hvar.eq.'rho0feq' .or.
     &        hvar.eq.'dz'    .or. hvar.eq.'rhoz') then
        v=Dzred(icomp)
        if (l) Dzred(icomp)=dbl
      elseif (hvar.eq.'pcpr') then
        v=PcPR(icomp)
        if (l) PcPR(icomp)=dbl
      elseif (hvar.eq.'pcrit' .or. hvar.eq.'pcfeq') then
        v=Pcrit(icomp)
        if (l) Pcrit(icomp)=dbl
      elseif (hvar.eq.'peos' .or. hvar.eq.'pxeos') then
        v=Pxeos(icomp)
        if (l) Pxeos(icomp)=dbl
      elseif (hvar.eq.'ptpn') then
        v=Ptpn(icomp)
        if (l) Ptpn(icomp)=dbl
      elseif (hvar.eq.'reos') then
        v=Reos(icomp)
        if (l) Reos(icomp)=dbl
      elseif (hvar.eq.'rxgas' .or. hvar.eq.'r') then
        v=Rxgas
        if (l) Rxgas=dbl
      elseif (hvar.eq.'tcpr') then
        v=TcPR(icomp)
        if (l) TcPR(icomp)=dbl
      elseif (hvar.eq.'tcrit' .or. hvar.eq.'tcfeq') then
        v=Tcrit(icomp)
        if (l) Tcrit(icomp)=dbl
      elseif (hvar.eq.'tmeos') then
        v=Tmeos(icomp)
        if (l) Tmeos(icomp)=dbl
      elseif (hvar.eq.'tnbp') then
        v=Tnbp(icomp)
        if (l) Tnbp(icomp)=dbl
      elseif (hvar.eq.'ttpn') then
        v=Ttpn(icomp)
        if (l) Ttpn(icomp)=dbl
      elseif (hvar.eq.'tredcp0') then
        v=TredCp0(icomp)
        if (l) TredCp0(icomp)=dbl
      elseif (hvar.eq.'txeos') then
        v=Txeos(icomp)
        if (l) Txeos(icomp)=dbl
      elseif (hvar.eq.'tzred' .or.
     &        hvar.eq.'t0feq' .or. hvar.eq.'tz') then
        v=Tzred(icomp)
        if (l) Tzred(icomp)=dbl
      elseif (hvar.eq.'wmas') then
        v=wmas(icomp)
        if (l) wmas(icomp)=dbl

c  Arrays
      elseif (hvar.eq.'afeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=aFEQ(1:ia,icomp)
        if (l) aFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'tifeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=tiFEQ(1:ia,icomp)
        if (l) tiFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'difeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=diFEQ(1:ia,icomp)
        if (l) diFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'dlifeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=dliFEQ(1:ia,icomp)
        if (l) dliFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'alphafeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=alphaFEQ(1:ia,icomp)
        if (l) alphaFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'betafeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=betaFEQ(1:ia,icomp)
        if (l) betaFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'gammafeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=gammaFEQ(1:ia,icomp)
        if (l) gammaFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'deltafeq') then
        ia=nFEQterms(icomp)
        a(1:ia)=deltaFEQ(1:ia,icomp)
        if (l) deltaFEQ(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'acp0') then
        ia=nCp0poly(icomp)+nCp0Exp(icomp)
        a(1:ia)=aCp0(1:ia,icomp)
        if (l) aCp0(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'ticp0') then
        ia=nCp0poly(icomp)+nCp0Exp(icomp)
        a(1:ia)=TiCp0(1:ia,icomp)
        if (l) TiCp0(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'prcoef') then
        ia=20
        a(1:ia)=PRcoef(1:ia,icomp)
        if (l) PRcoef(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'fprkij') then
        ia=nmxprb
        a(1:ia)=fPRkij2(1:ia,icomp,jcomp)
        if (l) then
          fPRkij2(1:ia,icomp,jcomp)=arr(1:ia)
          fPRkij2(1:ia,jcomp,icomp)=arr(1:ia)
          if (kijzero.eq.0) then       !If the flag to zero out all PR coefficients is off, transfer the values read in to the values that will be used.
            fPRkij(1:ia,icomp,jcomp)=fPRkij2(1:ia,icomp,jcomp)
            fPRkij(1:ia,jcomp,icomp)=fPRkij2(1:ia,icomp,jcomp)
          endif
        endif
      elseif (hvar.eq.'daddn')then
        ia=nx
        a(1:ia)=daddn(1:ia)
        if (l) daddn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'d2adbn')then
        ia=nx
        a(1:ia)=d2adbn(1:ia)
        if (l) d2adbn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'d2addn')then
        ia=nx
        a(1:ia)=d2addn(1:ia)
        if (l) d2addn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'d2adtn')then
        ia=nx
        a(1:ia)=d2adtn(1:ia)
        if (l) d2adtn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'daddx')then
        ia=nx
        a(1:ia)=daddx(1:ia)
        if (l) daddx(1:ia)=arr(1:ia)
      elseif (hvar.eq.'daddxitv')then
        ia=nx
        a(1:ia)=daddxiTV(1:ia)
        if (l) daddxiTV(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dadtx')then
        ia=nx
        a(1:ia)=dadtx(1:ia)
        if (l) dadtx(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dadxi')then
        ia=nx
        a(1:ia)=dadxi(1:ia)
        if (l) dadxi(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dadxitv')then
        ia=nx
        a(1:ia)=dadxiTV(1:ia)
        if (l) dadxiTV(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dddn')then
        ia=nx
        a(1:ia)=dDdn(1:ia)
        if (l) dDdn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dlnfinidt')then
        ia=nx
        a(1:ia)=dlnfinidT(1:ia)
        if (l) dlnfinidT(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dlnfinidv')then
        ia=nx
        a(1:ia)=dlnfinidV(1:ia)
        if (l) dlnfinidV(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dmurdt')then
        ia=nx
        a(1:ia)=dmurdt(1:ia)
        if (l) dmurdt(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dpdn')then
        ia=nx
        a(1:ia)=dpdn(1:ia)
        if (l) dpdn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dpdxi')then
        ia=nx
        a(1:ia)=dpdxi(1:ia)
        if (l) dpdxi(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dphidp')then
        ia=nx
        a(1:ia)=dphidp(1:ia)
        if (l) dphidp(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dphidt')then
        ia=nx
        a(1:ia)=dphidt(1:ia)
        if (l) dphidt(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dtdn')then
        ia=nx
        a(1:ia)=dtdn(1:ia)
        if (l) dtdn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'dvdn')then
        ia=nx
        a(1:ia)=dvdn(1:ia)
        if (l) dvdn(1:ia)=arr(1:ia)
      elseif (hvar.eq.'xlnfi')then
        ia=nx
        a(1:ia)=xlnfi(1:ia)
        if (l) xlnfi(1:ia)=arr(1:ia)
      elseif (hvar.eq.'d2adnn')then
        ia=nx
        a(1:ia)=d2adnn(1:ia,icomp)
        if (l) d2adnn(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'d2adxn')then
        ia=nx
        a(1:ia)=d2adxn(1:ia,icomp)
        if (l) d2adxn(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'d2adxntv')then
        ia=nx
        a(1:ia)=d2adxnTV(1:ia,icomp)
        if (l) d2adxnTV(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'dadxij')then
        ia=nx
        a(1:ia)=dadxij(1:ia,icomp)
        if (l) dadxij(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'dlnphiidxj')then
        ia=nx
        a(1:ia)=dlnphiidxj(1:ia,icomp)
        if (l) dlnphiidxj(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'ddrdxn')then
        ia=nx
        a(1:ia)=ddrdxn(1:ia,icomp)
        if (l) ddrdxn(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'dtrdxn')then
        ia=nx
        a(1:ia)=dtrdxn(1:ia,icomp)
        if (l) dtrdxn(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'dphidxj')then
        ia=nx
        a(1:ia)=dphidxj(1:ia,icomp)
        if (l) dphidxj(1:ia,icomp)=arr(1:ia)
      elseif (hvar.eq.'dphidnj')then
        ia=nx
        a(1:ia)=dphidnj(1:ia,icomp)
        if (l) dphidnj(1:ia,icomp)=arr(1:ia)
        ia=nx
      else
        call ERRNUM (601,0,'PASSCMN',hvar,0d0,0d0,0d0,ierr,herr)
      endif

 500  if (l) then
        call RESETA (0)
      else
        if (ia.gt.0) then
          arr(1:ia)=a(1:ia)
        elseif (ij.ne.0) then
          ilng=ij
        elseif (v.ne.0.) then
          dbl=v
        elseif (htemp.ne.' ') then
          hstr=htemp
        endif
      endif
      end                                             subroutine PASSCMN
c
c ======================================================================
c
      subroutine SETPNTS (iFlag,icomp,z,T,P,Dl,Dv,ierr,herr)
c
c  Internal routine to calculate the density at 0.72*Tc (for a pure fluid
c  or mixture), or at the triple point for a pure fluid, both of which
c  are required for several other subroutines.

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)

      T=Ttpn(icomp)          !Return T(trpl) in case T is needed outside this routine.
      if (icomp.eq.0) call LIMITS ('EOS',z,T,Tmax,Dmax,Pmax)         !Set T=Tmin for a mixture.

c  Calculate liquid and vapor densities at 72% of the critical temperature if not set.
      if (iFlag.eq.0 .or. iFlag.eq.2) then
        P=0d0
        if (d72l.lt.1d-8) then         !Only calculate the density if it has not yet been set.
          call CRITP (z,Tc,Pc,Dc,ierr,herr)
          T=0.72d0*Tc
          call SATT (T,z,1,P,Dl,Dv,x,y,ierr,herr)
          if (ierr.le.0) d72l=Dl
          if (iAnc(icomp).ne.0) call SATT (T,z,2,P,dd,Dv,x,y,ierr,herr)
          if (ierr.le.0) d72v=Dv
        endif
        Dl=d72l
        Dv=d72v
      endif

c  Calculate vapor density at triple point if not set.
      if ((iFlag.eq.0 .or. iFlag.eq.1) .and. icomp.ne.0) then
        T=Tmeos(icomp)       !Return anything in case the T is needed outside this routine.
        if (Dtpv(icomp).lt.1d-15) THEN
          call SATT (T,z,2,P,Dl,Dv,x,y,ierr,herr)
          if (ierr.gt.0) then
            Dtpv(icomp)=1d-10
          else
            Dtpv(icomp)=Dv
            if (iAnc(icomp).ne.0) call SATT(T,z,1,P,Dl,Dv,x,y,ierr,herr)
            Dtpn(icomp)=Dl   !This and the next line overwrite the values read in from the call to SETUP.
            Ptpn(icomp)=P
          endif
        endif
        Dl=Dtpn(icomp)       !Return values even if SATT was not called.
        Dv=Dtpv(icomp)
        P=Ptpn(icomp)
      endif

      end                                             subroutine SETPNTS
c
c ======================================================================
c
      subroutine FLUIDID (hcasi,hashi,iFldID)
c
c  Assign fluids specific identification numbers.
c  These codes are mostly not needed except for the "special" fluids,
c  such as water, ammonia, helium, and CO2.

      include 'COMMONS.INC'
      character hcasi*(*),hashi*(*)

      iFldID=0
      if (hashi.eq.hcasi .or. hashi.eq.' ') call FLUIDID2(hcasi,hashi)   !Catch cases where the hash number was not included in the fluid file.

c  Worst offenders.
      if (hashi.eq.'b43a7600') iFldID=1      !Water
      if (hashi.eq.'e9847540') iFldID=2      !Ammonia
      if (hashi.eq.'3e131610') iFldID=3      !Heavy water
c
c  Fluids sorted by normal boiling point up to about 250 K.
      if (hashi.eq.'3b8eed30') iFldID=10     !Helium            4.2238  Normal boiling point
      if (hashi.eq.'8d757b4p') iFldID=11     !Parahydrogen     20.271
      if (hashi.eq.'8d757b40') iFldID=12     !Hydrogen         20.369
      if (hashi.eq.'8d757b4o') iFldID=13     !Orthohydrogen    20.380
      if (hashi.eq.'639b18a0') iFldID=14     !Deuterium        23.661
      if (hashi.eq.'b912fce0') iFldID=15     !Neon             27.104
      if (hashi.eq.'edbdc8e0') iFldID=16     !Nitrogen         77.355
      if (hashi.eq.'091cb7e0') iFldID=17     !CO               81.64
      if (hashi.eq.'cdc99fe0') iFldID=18     !Fluorine         85.0368
      if (hashi.eq.'ed016370') iFldID=19     !Argon            87.302
      if (hashi.eq.'edc36840') iFldID=20     !Oxygen           90.1878
c     if (hashi.eq.'8ae7a700') iFldID=       !Methane         111.667
      if (hashi.eq.'d7c8c510') iFldID=22     !Krypton         119.73
      if (hashi.eq.'        ') iFldID=23     !NO              121.4
      if (hashi.eq.'6db4e3b0') iFldID=24     !NF3             144.138
      if (hashi.eq.'71fd7fa0') iFldID=25     !Xenon           165.05
      if (hashi.eq.'9c1062b0') iFldID=26     !Ethylene        169.379
c     if (hashi.eq.'434e2a40') iFldID=       !Ethane          184.569
      if (hashi.eq.'e0647c00') iFldID=28     !N2O             184.68
      if (hashi.eq.'74b17450') iFldID=29     !HCl             188.179
c     if (hashi.eq.'        ') iFldID=30     !Acetylene       189.
      if (hashi.eq.'77c8bee0') iFldID=31     !CO2             194.686
      if (hashi.eq.'c6c03020') iFldID=32     !H2S             212.85
      if (hashi.eq.'e7f902e0') iFldID=33     !COS             222.99
      if (hashi.eq.'7a75e610') iFldID=34     !SF6             223.555
      if (hashi.eq.'828f1c80') iFldID=35     !Chlorine        239.198
      if (hashi.eq.'d26ca240') iFldID=36     !CF3I            251.3
      if (hashi.eq.'0b34bc40') iFldID=37     !Vinyl chloride  259.443
      if (hashi.eq.'7fad4b80') iFldID=38     !SO2             263.137
      if (hashi.eq.'557b13f0') iFldID=39     !Ethylene oxide  283.660
      if (hashi.eq.'8ae7a700') iFldID=40     !NO2             294.

c  Alkanes
      if (hashi.eq.'8ae7a700') iFldID=101    !Methane         111.667
      if (hashi.eq.'434e2a40') iFldID=102    !Ethane          184.569
      if (hashi.eq.'70c6aac0') iFldID=103    !Propane         231.036
      if (hashi.eq.'7b3b4080') iFldID=104    !Butane          272.660
      if (hashi.eq.'76bc0290') iFldID=105    !Pentane         309.21
      if (hashi.eq.'cb03ba40') iFldID=106    !Hexane          341.86
      if (hashi.eq.'b398a3b0') iFldID=107    !Heptane         371.53
      if (hashi.eq.'f174a9b0') iFldID=108    !Octane          398.77
      if (hashi.eq.'7f94dc20') iFldID=109    !Nonane          423.91
      if (hashi.eq.'111888d0') iFldID=110    !Decane          447.27
      if (hashi.eq.'47ade7e0') iFldID=140    !Isobutane       261.401
      if (hashi.eq.'86ee46a0') iFldID=150    !Isopentane      300.98
      if (hashi.eq.'5a410790') iFldID=151    !Neopentane      282.65

c  Other hydrocarbons
      if (hashi.eq.'56d778b0') iFldID=230    !Propylene       225.531
      if (hashi.eq.'cd7a06d0') iFldID=240    !Butene          266.84
      if (hashi.eq.'a49b8b50') iFldID=260    !Toluene         383.75
      if (hashi.eq.'f2cfb460') iFldID=270    !Benzene         353.219

c  Alcohols
      if (hashi.eq.'c41aa690') iFldID=500    !Methanol
      if (hashi.eq.'3d5f67e0') iFldID=501    !Ethanol
      if (hashi.eq.'ff1a4c80') iFldID=502    !n-Propanol    (1-Propanol)
      if (hashi.eq.'430d5ab0') iFldID=503    !Isopropanol   (2-Propanol)
      if (hashi.eq.'bb7ca950') iFldID=504    !n-Butanol     (1-Butanol)
      if (hashi.eq.'420202e0') iFldID=506    !2-Butanol     (sec-Butanol)
c     if (hashi.eq.'        ') iFldID=505    !Isobutanol    (2-methyl-1-propanol)
c     if (hashi.eq.'        ') iFldID=507    !tert-Butanol  (2-methyl-2-propanol)

      if (hashi.eq.' ') hashi=hcasi

      end                                             subroutine FLUIDID
c
c ======================================================================
c
      subroutine FLUIDID2 (hcasi,hashi)
c
c  Check if hash numbers have not been assigned in a fluid file, and if
c  so, check the CAS numbers below to assign the hash number.

      include 'COMMONS.INC'
      character hcasi*(*),hashi*(*),hcasnumb*12
      hcasnumb=hcasi
      if (INDEX(hcasi,'-').ne.0) then
        do
          i=INDEX (hcasnumb,'-')          !If a CAS number was sent, remove the dashes and try again.
          if (i.eq.0) EXIT
          hcasnumb=hcasnumb(:i-1)//hcasnumb(i+1:)
        enddo
      endif


      if (hcasnumb.eq.'106990'   ) hashi='cd07ca00'   !1,3-Butadiene
      if (hcasnumb.eq.'71363'    ) hashi='bb7ca950'   !1-Butanol
      if (hcasnumb.eq.'107006'   ) hashi='4bfbe360'   !1-Butyne
      if (hcasnumb.eq.'111273'   ) hashi='deac2bd0'   !1-Hexanol
      if (hcasnumb.eq.'592416'   ) hashi='180221b0'   !1-Hexene
      if (hcasnumb.eq.'71410'    ) hashi='2eb78360'   !1-Pentanol
      if (hcasnumb.eq.'109671'   ) hashi='1f061ca0'   !1-Pentene
      if (hcasnumb.eq.'71238'    ) hashi='ff1a4c80'   !1-Propanol
      if (hcasnumb.eq.'75832'    ) hashi='c9396950'   !2,2-Dimethylbutane
      if (hcasnumb.eq.'79298'    ) hashi='bca9ff20'   !2,3-Dimethylbutane
      if (hcasnumb.eq.'78922'    ) hashi='420202e0'   !2-Butanol
      if (hcasnumb.eq.'96140'    ) hashi='5bd6f470'   !3-Methylpentane
      if (hcasnumb.eq.'67641'    ) hashi='342481e0'   !Acetone
      if (hcasnumb.eq.'74862'    ) hashi='6283cfc0'   !Acetylene
      if (hcasnumb.eq.'7664417'  ) hashi='e9847540'   !Ammonia
      if (hcasnumb.eq.'7440371'  ) hashi='ed016370'   !Argon
      if (hcasnumb.eq.'71432'    ) hashi='f2cfb460'   !Benzene
      if (hcasnumb.eq.'106978'   ) hashi='7b3b4080'   !Butane
      if (hcasnumb.eq.'106989'   ) hashi='cd7a06d0'   !Butene
      if (hcasnumb.eq.'124389'   ) hashi='77c8bee0'   !Carbon dioxide
      if (hcasnumb.eq.'630080'   ) hashi='091cb7e0'   !Carbon monoxide
      if (hcasnumb.eq.'463581'   ) hashi='e7f902e0'   !Carbonyl sulfide
      if (hcasnumb.eq.'7782505'  ) hashi='828f1c80'   !Chlorine
      if (hcasnumb.eq.'108907'   ) hashi='2366d210'   !Chlorobenzene
      if (hcasnumb.eq.'590181'   ) hashi='411c4500'   !cis-Butene
      if (hcasnumb.eq.'493016'   ) hashi='d8980000'   !cis-Decalin
      if (hcasnumb.eq.'822355'   ) hashi='0b1a5d30'   !Cyclobutene
      if (hcasnumb.eq.'110827'   ) hashi='6e225350'   !Cyclohexane
      if (hcasnumb.eq.'287923'   ) hashi='43ab1810'   !Cyclopentane
      if (hcasnumb.eq.'75194'    ) hashi='bc9f5400'   !Cyclopropane
      if (hcasnumb.eq.'556672'   ) hashi='b6895470'   !D4
      if (hcasnumb.eq.'541026'   ) hashi='ccbc27e0'   !D5
      if (hcasnumb.eq.'540976'   ) hashi='1fad29d0'   !D6
      if (hcasnumb.eq.'124185'   ) hashi='111888d0'   !Decane
      if (hcasnumb.eq.'7782390'  ) hashi='639b18a0'   !Deuterium
      if (hcasnumb.eq.'107062'   ) hashi='042f81b0'   !Dichloroethane
      if (hcasnumb.eq.'60297'    ) hashi='46a701e0'   !Diethyl ether
      if (hcasnumb.eq.'616386'   ) hashi='859e6f80'   !Dimethyl carbonate
      if (hcasnumb.eq.'115106'   ) hashi='8f27e300'   !Dimethylether
      if (hcasnumb.eq.'629970'   ) hashi='508eb840'   !Docosane
      if (hcasnumb.eq.'112403'   ) hashi='8bc8dec0'   !Dodecane
      if (hcasnumb.eq.'3822682'  ) hashi='e8821e60'   !E125
      if (hcasnumb.eq.'421147'   ) hashi='bf6c1a00'   !E143a
      if (hcasnumb.eq.'112958'   ) hashi='a3a93300'   !Eicosane
      if (hcasnumb.eq.'74840'    ) hashi='434e2a40'   !Ethane
      if (hcasnumb.eq.'64175'    ) hashi='3d5f67e0'   !Ethanol
      if (hcasnumb.eq.'100414'   ) hashi='a4c64370'   !Ethylbenzene
      if (hcasnumb.eq.'74851'    ) hashi='9c1062b0'   !Ethylene
      if (hcasnumb.eq.'107211'   ) hashi='7d1768e0'   !Ethylene glycol
      if (hcasnumb.eq.'75218'    ) hashi='557b13f0'   !Ethylene oxide
      if (hcasnumb.eq.'7782414'  ) hashi='cdc99fe0'   !Fluorine
      if (hcasnumb.eq.'7789200'  ) hashi='3e131610'   !Heavy water
      if (hcasnumb.eq.'7440597'  ) hashi='3b8eed30'   !Helium
      if (hcasnumb.eq.'14762551' ) hashi='9f0f5870'   !Helium-3
      if (hcasnumb.eq.'629947'   ) hashi='1d0709f0'   !Heneicosane
      if (hcasnumb.eq.'629787'   ) hashi='19173320'   !Heptadecane
      if (hcasnumb.eq.'142825'   ) hashi='b398a3b0'   !Heptane
      if (hcasnumb.eq.'544763'   ) hashi='8cf1f140'   !Hexadecane
      if (hcasnumb.eq.'110543'   ) hashi='cb03ba40'   !Hexane
      if (hcasnumb.eq.'630068'   ) hashi='5ed02bf0'   !Hexatriacontane
      if (hcasnumb.eq.'302012'   ) hashi='506609c0'   !Hydrazine
      if (hcasnumb.eq.'1333740'  ) hashi='8d757b40'   !Hydrogen (normal)
      if (hcasnumb.eq.'7647010'  ) hashi='74b17450'   !Hydrogen chloride
      if (hcasnumb.eq.'7722841'  ) hashi='4efdc5a0'   !Hydrogen peroxide
      if (hcasnumb.eq.'7783064'  ) hashi='c6c03020'   !Hydrogen sulfide
      if (hcasnumb.eq.'75285'    ) hashi='47ade7e0'   !Isobutane
      if (hcasnumb.eq.'115117'   ) hashi='d287e4b0'   !Isobutene
      if (hcasnumb.eq.'4390049'  ) hashi='f6edce00'   !Isohexadecane
      if (hcasnumb.eq.'107835'   ) hashi='faa2db70'   !Isohexane
      if (hcasnumb.eq.'540841'   ) hashi='c2dd9ef0'   !Isooctane
      if (hcasnumb.eq.'78784'    ) hashi='86ee46a0'   !Isopentane
      if (hcasnumb.eq.'67630'    ) hashi='430d5ab0'   !Isopropanol
      if (hcasnumb.eq.'7439909'  ) hashi='d7c8c510'   !Krypton
      if (hcasnumb.eq.'108383'   ) hashi='0f99ff70'   !m-Xylene
      if (hcasnumb.eq.'141628'   ) hashi='63e16330'   !MD2M
      if (hcasnumb.eq.'141639'   ) hashi='7d394df0'   !MD3M
      if (hcasnumb.eq.'107528'   ) hashi='eabdcbe0'   !MD4M
      if (hcasnumb.eq.'107517'   ) hashi='4be67b80'   !MDM
      if (hcasnumb.eq.'74828'    ) hashi='8ae7a700'   !Methane
      if (hcasnumb.eq.'67561'    ) hashi='c41aa690'   !Methanol
      if (hcasnumb.eq.'112630'   ) hashi='b61b9eb0'   !Methyl linoleate
      if (hcasnumb.eq.'301008'   ) hashi='298c8c80'   !Methyl linolenate
      if (hcasnumb.eq.'112629'   ) hashi='f300b9e0'   !Methyl oleate
      if (hcasnumb.eq.'112390'   ) hashi='27c9ec60'   !Methyl palmitate
      if (hcasnumb.eq.'112618'   ) hashi='81403130'   !Methyl stearate
      if (hcasnumb.eq.'108872'   ) hashi='0429edd0'   !Methylcyclohexane
      if (hcasnumb.eq.'107460'   ) hashi='78082e80'   !MM
      if (hcasnumb.eq.'60344'    ) hashi='fa4584a0'   !Monomethylhydrazine
      if (hcasnumb.eq.'7440019'  ) hashi='b912fce0'   !Neon
      if (hcasnumb.eq.'463821'   ) hashi='5a410790'   !Neopentane
      if (hcasnumb.eq.'10102439' ) hashi='1fd67110'   !Nitric oxide
      if (hcasnumb.eq.'7727379'  ) hashi='edbdc8e0'   !Nitrogen
      if (hcasnumb.eq.'10102440' ) hashi='bab35440'   !Nitrogen dioxide
      if (hcasnumb.eq.'10544726' ) hashi='34aab260'   !Nitrogen tetroxide
      if (hcasnumb.eq.'7783542'  ) hashi='6db4e3b0'   !Nitrogen trifluoride
      if (hcasnumb.eq.'10024972' ) hashi='e0647c00'   !Nitrous oxide
      if (hcasnumb.eq.'629925'   ) hashi='30ebfcf0'   !Nonadecane
      if (hcasnumb.eq.'111842'   ) hashi='7f94dc20'   !Nonane
      if (hcasnumb.eq.'756138'   ) hashi='a9e22c00'   !Novec 649, 1230
      if (hcasnumb.eq.'95476'    ) hashi='32008e80'   !o-Xylene
      if (hcasnumb.eq.'593453'   ) hashi='74887150'   !Octadecane
      if (hcasnumb.eq.'111659'   ) hashi='f174a9b0'   !Octane
      if (hcasnumb.eq.'1333740o' ) hashi='8d757b4o'   !Orthohydrogen
      if (hcasnumb.eq.'7782447'  ) hashi='edc36840'   !Oxygen
      if (hcasnumb.eq.'106423'   ) hashi='01b0e650'   !p-Xylene
      if (hcasnumb.eq.'1333740p' ) hashi='8d757b4p'   !Parahydrogen
      if (hcasnumb.eq.'629629'   ) hashi='21e1c020'   !Pentadecane
      if (hcasnumb.eq.'109660'   ) hashi='76bc0290'   !Pentane
      if (hcasnumb.eq.'355259'   ) hashi='a8f85e00'   !Perfluorobutane
      if (hcasnumb.eq.'355420'   ) hashi='31895f40'   !Perfluorohexane
      if (hcasnumb.eq.'678262'   ) hashi='053a8820'   !Perfluoropentane
      if (hcasnumb.eq.'463490'   ) hashi='46c6b6e0'   !Propadiene
      if (hcasnumb.eq.'74986'    ) hashi='70c6aac0'   !Propane
      if (hcasnumb.eq.'1678928'  ) hashi='206f01b0'   !Propylcyclohexane
      if (hcasnumb.eq.'115071'   ) hashi='56d778b0'   !Propylene
      if (hcasnumb.eq.'57556'    ) hashi='b7273ed0'   !Propylene glycol
      if (hcasnumb.eq.'75569'    ) hashi='814f4990'   !Propylene oxide
      if (hcasnumb.eq.'74997'    ) hashi='3cbb9620'   !Propyne
      if (hcasnumb.eq.'75694'    ) hashi='6f9b71e0'   !R11
      if (hcasnumb.eq.'359115'   ) hashi='64555530'   !R1123
      if (hcasnumb.eq.'76131'    ) hashi='d2a09ee0'   !R113
      if (hcasnumb.eq.'76142'    ) hashi='b1698aa0'   !R114
      if (hcasnumb.eq.'76153'    ) hashi='f7285250'   !R115
      if (hcasnumb.eq.'354643'   ) hashi='5eb69c00'   !R115I1
      if (hcasnumb.eq.'76164'    ) hashi='04997260'   !R116
      if (hcasnumb.eq.'75718'    ) hashi='98829b70'   !R12
      if (hcasnumb.eq.'116154'   ) hashi='edc3a7b0'   !R1216
      if (hcasnumb.eq.'111512608') hashi='55d0af00'   !R1224yd(Z)
      if (hcasnumb.eq.'306832'   ) hashi='efdfede0'   !R123
      if (hcasnumb.eq.'102687650') hashi='bf17dfe0'   !R1233zd(E)
      if (hcasnumb.eq.'754121'   ) hashi='40377b40'   !R1234yf
      if (hcasnumb.eq.'29118249' ) hashi='9905ef70'   !R1234ze(E)
      if (hcasnumb.eq.'29118250' ) hashi='e23fd030'   !R1234ze(Z)
      if (hcasnumb.eq.'2837890'  ) hashi='36953260'   !R124
      if (hcasnumb.eq.'677214'   ) hashi='86479e20'   !R1243zf
      if (hcasnumb.eq.'354336'   ) hashi='25c5a3a0'   !R125
      if (hcasnumb.eq.'75729'    ) hashi='126c90a0'   !R13
      if (hcasnumb.eq.'692499'   ) hashi='8da378b0'   !R1336mzz(Z)
      if (hcasnumb.eq.'359353'   ) hashi='b44cfd10'   !R134
      if (hcasnumb.eq.'811972'   ) hashi='ff1c0560'   !R134a
      if (hcasnumb.eq.'75638'    ) hashi='52a67e60'   !R13B1
      if (hcasnumb.eq.'2314978'  ) hashi='d26ca240'   !R13I1
      if (hcasnumb.eq.'75730'    ) hashi='dba9f950'   !R14
      if (hcasnumb.eq.'1717006'  ) hashi='ba322c10'   !R141b
      if (hcasnumb.eq.'75683'    ) hashi='f2a8b2e0'   !R142b
      if (hcasnumb.eq.'420462'   ) hashi='0deae990'   !R143a
      if (hcasnumb.eq.'75376'    ) hashi='63f364b0'   !R152a
      if (hcasnumb.eq.'353366'   ) hashi='edb53660'   !R161
      if (hcasnumb.eq.'75434'    ) hashi='79ff2f80'   !R21
      if (hcasnumb.eq.'76197'    ) hashi='00bd3610'   !R218
      if (hcasnumb.eq.'75456'    ) hashi='57716470'   !R22
      if (hcasnumb.eq.'431890'   ) hashi='40091ee0'   !R227ea
      if (hcasnumb.eq.'75467'    ) hashi='20e26c20'   !R23
      if (hcasnumb.eq.'431630'   ) hashi='93fd5d40'   !R236ea
      if (hcasnumb.eq.'690391'   ) hashi='77fa2150'   !R236fa
      if (hcasnumb.eq.'679867'   ) hashi='ead82280'   !R245ca
      if (hcasnumb.eq.'460731'   ) hashi='5c322280'   !R245fa
      if (hcasnumb.eq.'75092'    ) hashi='9b267990'   !R30
      if (hcasnumb.eq.'75105'    ) hashi='7b05bb60'   !R32
      if (hcasnumb.eq.'662351'   ) hashi='ab37d9c0'   !R338mcc
      if (hcasnumb.eq.'406586'   ) hashi='9d316cb0'   !R365mfc
      if (hcasnumb.eq.'74873'    ) hashi='2b938200'   !R40
      if (hcasnumb.eq.'593533'   ) hashi='8ee31230'   !R41
      if (hcasnumb.eq.'163702087') hashi='f1711450'   !R7100
      if (hcasnumb.eq.'163702065') hashi='fa985a70'   !R7200 (HFE-569mccc) (This fluid has two CAS numbers)
      if (hcasnumb.eq.'163702054') hashi='993e7d80'   !R7200
      if (hcasnumb.eq.'115253'   ) hashi='9e4d8e90'   !RC318
      if (hcasnumb.eq.'1691174'  ) hashi='a7b53010'   !RE134
      if (hcasnumb.eq.'421147'   ) hashi='bf6c1a00'   !RE143a
      if (hcasnumb.eq.'22410442' ) hashi='82e9e4f0'   !RE245cb2
      if (hcasnumb.eq.'1885489'  ) hashi='a7275d70'   !RE245fa2
      if (hcasnumb.eq.'375031'   ) hashi='12934ef0'   !RE347mcc (HFE-7000)
      if (hcasnumb.eq.'7446095'  ) hashi='7fad4b80'   !Sulfur dioxide
      if (hcasnumb.eq.'2551624'  ) hashi='7a75e610'   !Sulfur hexafluoride
      if (hcasnumb.eq.'629594'   ) hashi='e4cf6d00'   !Tetradecane
      if (hcasnumb.eq.'108883'   ) hashi='a49b8b50'   !Toluene
      if (hcasnumb.eq.'624646'   ) hashi='b28337f0'   !trans-Butene
      if (hcasnumb.eq.'629505'   ) hashi='98575960'   !Tridecane
      if (hcasnumb.eq.'1120214'  ) hashi='62f46c40'   !Undecane
      if (hcasnumb.eq.'75014'    ) hashi='0b34bc40'   !Vinyl chloride
      if (hcasnumb.eq.'7732185'  ) hashi='b43a7600'   !Water
      if (hcasnumb.eq.'7440633'  ) hashi='71fd7fa0'   !Xenon

      if (hashi.eq.' ') hashi=hcasnumb

      end                                            subroutine FLUIDID2
c
c ======================================================================
c
      subroutine GETINPUTS (ab,ab2,at,bt,iat,ibt,a,b,aa,bb,T,D,P,
     &                      ierr,herr)
c
c  Internal routine to ABFLSH, ABFL1, and ABFL2.  The property order
c  specified in ab is checked for the correct order, and reversed
c  if necessary.  It is then split into two pieces, and T, D, and P
c  are assigned when they are included as one of the inputs.

      include 'COMMONS.INC'
      character at*1,bt*1,ab*2,ab2*2,abb*7,herr*255,UCASE*255
      parameter (abb='TPDEHSQ')
            ierr=0
      herr=' '
      T=300d0
      P=0d0
      D=0d0
      aa=a
      bb=b
      ab2=UCASE(ab)
      i=0
      if (ab2(1:1).ne.'T') then
        if (ab2.eq.'PT' .or. ab2.eq.'DT' .or. ab2.eq.'PD') i=1
        if (ab2(1:1).ne.'P') then
          if (ab2.eq.'EP' .or. ab2.eq.'HP' .or. ab2.eq.'SP') i=1
          if (ab2.eq.'ET' .or. ab2.eq.'HT' .or. ab2.eq.'ST') i=1
          if (ab2.eq.'ED' .or. ab2.eq.'HD' .or. ab2.eq.'SD') i=1
          if (ab2.eq.'HE' .or. ab2.eq.'ES' .or. ab2.eq.'HS') i=1
          if (ab2.eq.'QT' .or. ab2.eq.'QP' .or. ab2.eq.'QD') i=1
        endif
        if (i.eq.1) then
          ab2=ab2(2:2)//ab2(1:1)
          aa=b
          bb=a
        endif
      endif
      at=ab2(1:1)
      bt=ab2(2:2)
      iat=0
      ibt=0
      if (at.eq.'D') then
        iat=iLetterD
      elseif (at.eq.'E') then
        iat=iLetterE
      elseif (at.eq.'H') then
        iat=iLetterH
      elseif (at.eq.'P') then
        iat=iLetterP
      elseif (at.eq.'Q') then
        iat=iLetterQ
      elseif (at.eq.'S') then
        iat=iLetterS
      elseif (at.eq.'T') then
        iat=iLetterT
      endif

      if (bt.eq.'D') then
        ibt=iLetterD
      elseif (bt.eq.'E') then
        ibt=iLetterE
      elseif (bt.eq.'H') then
        ibt=iLetterH
      elseif (bt.eq.'P') then
        ibt=iLetterP
      elseif (bt.eq.'Q') then
        ibt=iLetterQ
      elseif (bt.eq.'S') then
        ibt=iLetterS
      elseif (bt.eq.'T') then
        ibt=iLetterT
      endif

      if (iat.eq.iLetterT) T=aa
      if (iat.eq.iLetterD) D=aa
      if (iat.eq.iLetterP) P=aa
      if (ibt.eq.iLetterD) D=bb
      if (ibt.eq.iLetterP) P=bb
      if (INDEX(abb,at).eq.0) then
        call ERRNUM (601,0,'GETINPUTS',ab,0d0,0d0,0d0,ierr,herr)
      elseif (INDEX(abb,bt).eq.0) then
        call ERRNUM (601,0,'GETINPUTS',ab,0d0,0d0,0d0,ierr,herr)
      elseif (ab2.eq.'EH') then
        call ERRNUM (601,0,'GETINPUTS',ab,0d0,0d0,0d0,ierr,herr)
      endif

      end                                           subroutine GETINPUTS
c
c ======================================================================
c
      subroutine CHECKSTATE (iFlag,mFlag,T,D,z,Err,ierr,herr)
c
c  Determine if the state at the input temperature and density is a
c  potential 2-phase state by applying various criteria, such as
c  a negative dP/d(rho).  However, some erroneous results could be
c  returned from bad equations of state, or fluids that behave
c  differently from other fluids (i.e., water, D2O, helium, and the
c  three hydrogens).
c
c  Inputs:
c    iFlag--When set to 1, use the pure fluid ancillary equations to check
c             for single-phase or two-phase states.  The error can be as high
c             as 1% from the true saturation state (always erroring on
c             the side of the metastable state near saturation.)
c           When set to 0, use the various criteria outlined below to
c             estimate the state of the input point.
c    mFlag--If set to 1, the mixture criteria will also be applied,
c             but these calculations are much slower than those for the
c             pure fluids, and only really contribute for non-ideal
c             mixtures.
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        z--Composition array (array of mole fractions)
c
c  Outputs:
c      Err--The value of the first criteria that triggers the possibility
c           of a two-phase state.  The value is only useful in comparing
c           to the values at other states.  Those closer to saturation
c           will generally have smaller values.  Only negative values
c           indicate two-phase states.
c     ierr--Error flag:  0 - Inputs appear to be single phase or in the metastable region
c                     >700 - Pure fluid density is within the 2-phase according to the ancillary equations.
c     herr--Error string (character*255)
c
c  Nonissue fluids:  Those that fail at densities 0% to 50% higher than the
c    triple point;  Methanol, Neon, Propyne, R11, R12, R152a, R21
c    (much, but not all, of these failures are below the melting line).
c
c  Fluids to watch:  Those that fail at densities somewhat less than the
c    triple point, but at higher temperatures;
c    Fluorine, NF3, R13, R22.
c
c  Bad fluids, errors will occur:  These fails in the single-phase region
c    around the liquid triple point;
c      Fluorine  (d2PdTD)
c      Propyne   (phi30+2d0*phi20-12d0)
c      NF3       (d2PdTD)
c      R123      (d2PdTD+4d0)
c
c  Vapor phase failures:
c    Water, D2O, and Methanol fail the (phi02+3d-3) test in parts of the vapor phase.
c
c  Fluids that should fail:
c    Water (d2TdPD-1d-3 up to 315 K and 64 mol/l, ending at the saturated liquid line or the melting line.)
c    Water (phi30+2d0*phi20-12d0 below 276 K down to the melting line, and between 59 and 67 mol/l.)
c    D2O (d2TdPD-1d-3 up to 330 K and 60 mol/l, ending at the saturated liquid line or the melting line.)
c    D2O (dDdT up to 285 K and 58 mol/l, ending at the saturated liquid line or the melting line.)
c    Helium (d2TdPD-1d-3 from the lambda temperature to 3.15 K and from the the saturated liquid line to 50 mol/l.)
c
c  For CO2, the nonanalytical terms cause (d2PdTD+4d0) to fail in the critical region at temperatures
c    below 304.2 K and densities between 9.5 and 12.4 mol/l.
c
      include 'COMMONS.INC'
      character herr*255,herrm*255
      dimension z(ncmax)     ! ,dadn(ncmax),dnadn(ncmax)

      Err=0d0
      ierr=0
      herr=' '

c  Nearly any mixture with a density of more than 50 mol/l will contain mostly water and will be in the single-phase liquid.
c  In such a situation, assume the state is valid and exit.
      call ISPURE (z,icomp)
      if (D.le.0. .or. T.le.0.) goto 500
      if (D.gt.50. .and. icomp.eq.0) goto 500

c  Pure fluid checks.
      if (icomp.ne.0) then
        Tt=Ttpn(icomp)
        Tc=Tcrit(icomp)
        Dt=Dtpn(icomp)
        Dc=Dcrit(icomp)
        if (D.ge.Dt) goto 500          !Return for any state above the triple point density.
        if (T.ge.Tc) goto 500          !Return for any state above the critical point temperature.

c  Use the two linear fits below for a pure fluid to estimate if a point is in the liquid phase.
        if (D.ge.Dc) then
          j=iFluidID(icomp)
          if (j.eq.1 .or. j.eq.3 .or. j.eq.10) then
                   !Oddball fluids:  Helium, Water, and D2O; the fits do not work for these.
          else
            Tr=(T-Tt)/(Tc-Tt)
            if (Tr.lt.0.63d0) then
              Dr=1.03d0-0.476d0*Tr
            else
              Dr=1.333d0-0.957d0*Tr
            endif
            Ds=Dr*(Dt-Dc)+Dc
            Err=Ds
            if (D.ge.Ds) goto 500
          endif
        endif

c  If iFlag is active, compare the densities with the ancillary equations to determine if a point is single or two-phase.
c  All ancillaries in Version 10 are within 1% of the saturation state (except very near the critical point).
        ipr=0
        if (iEOSmx.eq.iEOSPR)  ipr=2
        if (iEOSmx.eq.iEOSPR3) ipr=3
c       ipr=-1
c       call PREOS (ipr)     !Don't make checks when the P-R is on.
        if (iFlag.eq.1 .and. ipr.eq.0) then
          if (Tcrit(icomp)-T.gt.0.5d0) then      !The error in the ancillaries increases within 0.5 K of Tc.
            call DLSATK (icomp,T,Dl,ierrl,herr)
            if (ierrl.le.0 .and. D.gt.Dl*0.99d0) goto 500
            call DVSATK (icomp,T,Dv,ierrv,herr)
            if (ierrv.le.0 .and. D.lt.Dv*1.01d0) goto 500
            if (ierrl.le.0 .and. ierrv.le.0) then          !If errors from ancillaries, skip this section and go to the checks below.
              ierr=700       !Density is two phase, return with an error number.
              goto 500
            endif
          endif
        endif

c  Checks for some pure fluids that behave differently from other fluids.
        if (iFluidID(icomp).eq.1) then                     !Water
          if (T.lt.315d0 .and. D.gt.55d0)   goto 500
        elseif (iFluidID(icomp).eq.3) then                 !D2O
          if (T.lt.330d0 .and. D.gt.54.5d0) goto 500
        elseif (iFluidID(icomp).eq.10) then                !Helium
          if (T.lt.3.15d0 .and. D.gt.34d0)  goto 500
        elseif (iFluidID(icomp).eq.31) then                !CO2
          if (T.lt.304.2d0 .and. T.gt.303.8d0 .and.
     &        D.gt.9.5d0   .and. D.lt.12.4d0) goto 500
        endif
      endif

      call REDLINEAR (4,z,Tc,Pc,Dc,ierr,herr)
      call REDX (z,T0,D0)
      tau=T0/T
      del=D/D0
      phi01=PHIX(0,1,tau,del,z)
      phi02=PHIX(0,2,tau,del,z)
      phi11=PHIX(1,1,tau,del,z)
      phi12=PHIX(1,2,tau,del,z)
      phi20=PHIX(2,0,tau,del,z)
      phi30=PHIX(3,0,tau,del,z)

      R=Rxgas
      P=R*T*D*(1d0+phi01)
      u=xEq0(1d0+phi01-phi11)
      v=1d0+2d0*phi01+phi02
      dDdT   =-D*u/(T*v)
      dPdD   = R*T*v
      d2PdTD = R*(v-2d0*phi11-phi12)
      phi10=PHIX(1,0,tau,del,z)
      phi21=PHIX(2,1,tau,del,z)
      w  = phi01+phi02-phi11-phi12
      d2TdPD =-(u+w-v/u*phi21)/R/(D*u)**2

c  Pure fluid checks.
c  Do NOT change the order of these!
      ierr=0
      do i=701,711
        Err=0
        if (i.eq.701 .and. icomp.ne.0) then
          if (D.gt.Dc*0.8d0.and.D.lt.Dc*1.3d0.and.T.lt.Tc*0.965d0)Err=-1       !Do these for pure fluids only.
          if (D.gt.Dc*0.5d0.and.D.lt.Dc*1.6d0.and.T.lt.Tc*0.850d0)Err=-1
        endif
        if (i.eq.702) Err =  dPdD
        if (i.eq.703) Err = -dDdT           !The constants help raise the value enough so that small errors in the single phase for some fluids vanish.
        if (i.eq.704) Err =  d2PdTD          + 4d0         !Except R22, R13, R11, NF3, R1243zf, helium, fluorine at high densities.
        if (i.eq.705 .and. icomp.ne.0) Err = -phi30-2d0*phi20 + 12d0           !Except NF3 and propyne at high densities.  Also see 50/50 ethane/propane at low temperatures in the liquid.
        if (i.eq.706) Err = -phi20           + 6d0         !Except fluorine at high densities.
        if (i.eq.707) Err = -d2TdPD*1d3      + 1d0
        if (i.eq.708) Err = -phi10                         !There's a huge area at high densities where mixtures can trigger this one.
c       if (i.eq.709) err= 6+phi20+5*phi01
        if (i.eq.710) then
          Err = phi02*1d3 + 3d0                            !Except methanol and water at very low densities.
          if (icomp.eq.1 .and. iFluidID(icomp).eq.1) then
            if (D.lt.Dcrit(1)) Err = phi02*1d3 + 10d0      !Catch vapor phase issues with water.
          endif
          if (ixWat.gt.0.and.ixAlc.gt.0.and.D.lt.2.)Err=phi02*1d3+80d0 !Catch vapor phase issues with mixtures containing water and methanol.
        endif
        if (i.eq.711) Err = P                              !710 and 711 often indicate regions between the saturation state and the spinodal.
        if (Err.lt.0 .and. (iFlag.lt.700 .or. iFlag.eq.i)) then
          Err=-Err
          ierr=i
          if (ierr.ge.710) ierr=-i
          if (ierr.eq.705) ierr=-i
          if (icomp.eq.0) then
            if (ixWat.ne.0) then
              if (z(ixWat).ge.0.5d0 .and. ABS(ierr).ge.703) ierr=0        !A mixture with lots of water can run into this error in a valid state (which corresponds to the metastable region of water).
            endif
            if (ixCO2.ne.0) then
              Ttrpavg=SUM(z(1:nc)*Ttpn(1:nc))
              if (z(ixCO2).gt.0.25d0 .and. T.lt.0.85d0*Ttrpavg) ierr=0    !The equation of state for CO2 has interesting behavior below its triple point temperature.
            endif
          else
            if (iFluidID(icomp).eq.501 .and. ierr.eq.-705) ierr=0         !Ethanol has an area of negative d(Cv)/dT.
          endif
          if (ierr.ne.0) goto 500
        endif
      enddo
      if (icomp.ne.0) goto 500
      if (mFlag.eq.0) goto 500

c  Mixture checks.
      goto 500

c     call PHIDERV(2,T,D,z,dadn,dnadn,ierr1,herr)
c     do i=701,727
c       Err=0
c       if (i.eq.701) then
c          if (D.gt.Dc*0.8d0.and.D.lt.Dc*1.3d0.and.T.lt.Tc*0.85d0) Err=-1
c          if (D.gt.Dc*0.5d0.and.D.lt.Dc*1.6d0.and.T.lt.Tc*0.75d0) Err=-1
c       endif
c       if (i.eq.722) Err = 1-dlnfinidv(2)       !Falls in the same region as dP/dD
c       if (i.eq.723) Err = -z(1)*dlnfinidv(1)-z(2)*dlnfinidv(2)
c       if (i.eq.724) Err =  z(1)*dlnfinidt(1)+z(2)*dlnfinidt(2)
c       if (i.eq.725) Err =       dlnfinidt(1)
c       if (i.eq.726) Err =       dlnfinidt(2)
c       if (i.eq.727) Err = 1+z(1)*daddx(1)+z(2)*daddx(2)            !Essentially the same as a negative pressure.
c       if (Err.lt.0 .and. (iFlag.lt.700 .or. iFlag.eq.i)) then
c         Err=-Err
c         ierr=i
c         goto 500
c       endif
c     enddo

c     if (2+daddx(2)+daddx(1).lt.0. .and. ierr.eq.0) ierr=-721
c     if (dlnfinidt(2)       .lt.0. .and. ierr.eq.0) ierr=-722
c     if (dlnfinidt(1)       .lt.0. .and. ierr.eq.0) ierr=-723
c     if (1-dlnfinidv(1)     .lt.0. .and. ierr.eq.0) ierr=-724
c     if (1-dlnfinidv(2)     .lt.0. .and. ierr.eq.0) ierr=-725
c     Err=ABS(Err)

c  All of the conditions below work for 2-phase states, but are met
c    by the conditions above.
c     ierr=0
c     if (dsdt_d  .lt.0.) ...
c     if (dadtx(1).gt.0.)
c     if (bs      .lt.0.)
c     if (dhdt_d  .lt.0.)
c     if (xkkt    .lt.0.)
c     if (spht    .lt.0.)
c     if (dPdT    .lt.0.)
c     if (Cv      .lt.0.)
c     if (Cp      .lt.0.)
c     if (beta    .lt.0.)
c     if (xkt     .lt.0.)
c     if (dhdt_p  .lt.0.)
c     if (dsdt_p  .lt.0.)
c     if (dhdp_d  .lt.0.)
c     if (dsdp_d  .lt.0.)
c     if (xkappa  .lt.0.)
c     if (dsdt_d  .lt.0.)

 500  if (ierr.eq.0) RETURN
      herrm=' '
      if (ierr.eq. 700) herrm='Density is between ancillary values.'
      if (ierr.eq. 701) herrm='State is deep within 2-phase boundary.'
      if (ierr.eq. 702) herrm='dP/dD is negative.'
      if (ierr.eq. 703) herrm='dD/dT is positive.'
      if (ierr.eq. 704) herrm='d^2(P)/dTdD is negative.'
      if (ierr.eq. 706) herrm='Cv is negative.'
      if (ierr.eq. 707) herrm='d^2(T)/dPdD is positive.'
      if (ierr.eq. 708) herrm='Residual energy is positive.'
      if (ierr.eq.-710) herrm='d(ln(P))/d(ln(D)) is positive.'
      if (ierr.eq.-711) herrm='Pressure is negative.'
      if (ierr.eq.-705) herrm='d(Cv)/dT is positive.'
      call ERRNUM ((ierr),0,herrm,' ',0d0,0d0,0d0,ierr,herr)

      end                                          subroutine CHECKSTATE
c
c ======================================================================
c
      subroutine CHECKLIMITS (ab,T,D,P,z,ierr,herr)
c
c  Check if T, D, and P are within the range of validity.  For a warning
c  (such as T>Tmax but less than 1.5*Tmax), return the warning message
c  only if ierr=0.  For an error (such as P>2*Pmax), return the error
c  number and message only if ierr<=0.  Never overwrite ierr and herr
c  if ierr>0.
c
c  The input variable ab is used in the error message to specify which
c  routine called this one.
c
      include 'COMMONS.INC'
      character ab*2,herr*255,herr1*255,hsub*8
      dimension z(ncmax)

c  If the calling routine is not the first routine called by the user,
c    exit without checking limits or writing error messages.  Only the
c    outermost calling routine is allowed to check limits.  To change
c    this, call the FLAGS subroutine after calling SETUP.
      if (iErrNum.gt.0) RETURN
      if (ierr.lt.100) then            !Error numbers less than 100 are only used to report out of bounds messages or warnings.
        call LIMITX ('EOS',T,D,P,z,Tmin,Tmax,Dmax,Pmax,ierr1,herr1)
        if (ierr.ne.0 .or. ierr1.ne.0) then
          hsub=ab//'FLSH'    !Piece together the calling routine name.
          if (ab.eq.'S1') hsub='SATT'
          if (ab.eq.'S2') hsub='SATP'
          if (ab.eq.'S3') hsub='SATD'
          if (ab.eq.'S4') hsub='SATH'
          if (ab.eq.'S5') hsub='SATS'
          if (ab.eq.'S6') hsub='SATTP'
          j=INDEX(hsub,'  ')-1
          if (ierr.eq.0 .and. ierr1.lt.0) then             !Warning message; one or more inputs are outside limits but within valid extrapolation area.
           call ERRNUM(ierr1,777,hsub(1:j),herr1,0d0,0d0,0d0,ierr,herr)
          elseif (ierr.le.0 .and. ierr1.gt.0) then         !Error message; one or more inputs are outside the limits of the equation of state or transport routines.
           call ERRNUM(ierr1,888,hsub(1:j),herr1,0d0,0d0,0d0,ierr,herr)
          else
           call ERRMSG (ierr,herr)     !No error here, return error from previous occurrence if one exists.
           if (ierr.ne.0 .and. ierr1.ne.0 .and. herr.eq.' ') herr=herr1
          endif
        endif
      else
        call ERRMSG (ierr,herr)        !A previous error occurred with an error number >= 100, return that number and message.
      endif
      end                                         subroutine CHECKLIMITS
c
c ======================================================================
c
      subroutine CHKNAMES (icomp,hFl,ierr,herr)
c
c  Check fluid names that do not match the fluid file name, and return
c  the name if found.
c
c  Inputs:
c    icomp--Component number
c      hFl--Fluid name (character*255)
c
c  Outputs:
c      hFl--Fluid file name if match found
c     ierr--Error flag:  0 - Successful

      include 'COMMONS.INC'
      character*255 hFl,hFld,hFld2,UCASE,herr

      ierr=0
      herr=' '
      hFld=hFl
      hFld=UCASE(hFld)
      hAltFluidName=' '
      hFldFileName(icomp)=hFld
      i=INDEX(hFld,'.PPF')             !Return for pseudo-pure fluid files.
      if (i.gt.0) RETURN

      i=INDEX(hFld,'.FLD')             !Remove ".fld" from input file, which will be added back latter.
      if (i.gt.0) hFld=hFld(:i-1)
      k=INDEX(hfld,CHAR(92),.true.)              !Remove any directory structure
      if (k.eq.0) i=INDEX(hfld,'/',.true.)
      if (k.eq.0) i=INDEX(hfld,':',.true.)
      hFld =hFld(k+1:)
      hFld2=hFld
      if (hFld.eq.'AIR') then          !If just "Air" then send back air.ppf and a warning.
        hFld='AIR.PPF'
        hFl=hFl(1:k)//hFld
        call ERRNUM (-114,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        goto 510
      endif

c  Catch names that need only the parentheses removed.
      i=INDEX(hFld,'(')
      if (i.gt.0) then
        if (hFld2.eq.'R1233ZD(E)')      hFld2='R1233ZDE'
        if (hFld2.eq.'R1234ZE(E)')      hFld2='R1234ZEE'
        if (hFld2.eq.'R1234ZE(Z)')      hFld2='R1234ZEZ'
        if (hFld2.eq.'R1224YD(Z)')      hFld2='R1224YDZ'
        if (hFld2.eq.'R1336MZZ(Z)')     hFld2='R1336MZZZ'
        if (hFld2.ne.hFld) then
          hFld=hFld2
          goto 500
        endif
      endif

c  Remove special characters
      if (hFld(1:2).eq.'N-') hFld=hFld(3:)                 !Remove the "n-" at the beginning of normal alkanes.
      if (hFld(1:2).eq.'R-') hFld='R'//hFld(3:)            !Remove the "R-" at the beginning of refrigerants.
      do j=1,6
        i=INDEX(hFld,'(')
        if (i.ne.0) hFld=hFld(1:i-1)//hFld(i+1:)           !Remove parenthesis from input.
        i=INDEX(hFld,')')
        if (i.ne.0) hFld=hFld(1:i-1)//hFld(i+1:)
        i=INDEX(hFld,'-')
        if (i.ne.0) hFld=hFld(1:i-1)//hFld(i+1:)           !Remove dashes.
        i=INDEX(hFld,',')
        if (i.ne.0) hFld=hFld(1:i-1)//hFld(i+1:)           !Remove commas.
        i=INDEX(hFld,' ')
        if (i.ne.0) hFld=hFld(1:i-1)//hFld(i+1:)           !Remove spaces.
      enddo

c  Check if the input name needs to be changed to the correct fluid file name.
      i=INDEX(hFld,' ')-1
      if (i.eq.1) then
        if (hFld.eq.'B')                    hFld='BUTANE'
        if (hFld.eq.'E')                    hFld='ETHANE'
        if (hFld.eq.'M')                    hFld='METHANE'
        if (hFld.eq.'P')                    hFld='PROPANE'
        if (hFld.eq.'T')                    hFld='TOLUENE'
        if (hFld.eq.'W')                    hFld='WATER'
      elseif (i.eq.2) then
        if (hFld.eq.'AR')                   hFld='ARGON'
        if (hFld.eq.'C1')                   hFld='METHANE'
        if (hFld.eq.'C2')                   hFld='ETHANE'
        if (hFld.eq.'C3')                   hFld='PROPANE'
        if (hFld.eq.'C4')                   hFld='BUTANE'
        if (hFld.eq.'C5')                   hFld='PENTANE'
        if (hFld.eq.'C6')                   hFld='HEXANE'
        if (hFld.eq.'C7')                   hFld='HEPTANE'
        if (hFld.eq.'C8')                   hFld='OCTANE'
        if (hFld.eq.'C9')                   hFld='NONANE'
        if (hFld.eq.'H2')                   hFld='HYDROGEN'
        if (hFld.eq.'HE')                   hFld='HELIUM'
        if (hFld.eq.'N2')                   hFld='NITROGEN'
        if (hFld.eq.'O2')                   hFld='OXYGEN'
        if (hFld.eq.'YF')                   hFld='R1234YF'
        if (hFld.eq.'ZF')                   hFld='R1243ZF'
      elseif (i.eq.3) then
        if (hFld.eq.'H2O')                  hFld='WATER'
        if (hFld.eq.'C10')                  hFld='DECANE'
        if (hFld.eq.'C4I')                  hFld='ISOBUTAN'
        if (hFld.eq.'C5I')                  hFld='IPENTANE'
        if (hFld.eq.'ETO')                  hFld='ETHANOL'
        if (hFld.eq.'MEO')                  hFld='METHANOL'
        if (hFld.eq.'NH3')                  hFld='AMMONIA'
        if (hFld.eq.'YDZ')                  hFld='R1224YDZ'
        if (hFld.eq.'ZDE')                  hFld='R1233ZDE'
        if (hFld.eq.'ZEE')                  hFld='R1234ZEE'
        if (hFld.eq.'ZEZ')                  hFld='R1234ZEZ'
      elseif (i.eq.4) then
        if (hFld.eq.'MZZZ')                 hFld='R1336MZZZ'
      elseif (i.eq.5) then
        if (hFld.eq.'R1140')                hFld='VINYLCHLORIDE'
        if (hFld.eq.'R13I1')                hFld='CF3I'
        if (hFld.eq.'R7000')                hFld='RE347MCC'
        if (hFld.eq.'STEAM')                hFld='WATER'
      elseif (i.eq.6) then
        if (hFld.eq.'BUTENE')               hFld='1BUTENE'
        if (hFld.eq.'ETHENE')               hFld='ETHYLENE'
      elseif (i.eq.7) then
        if (hFld.eq.'IBUTANE')              hFld='ISOBUTAN'
        if (hFld.eq.'PENTENE')              hFld='1PENTENE'
        if (hFld.eq.'PROPENE')              hFld='PROPYLEN'
      elseif (i.eq.8) then
        if (hFld.eq.'1PENTENE')             hFld='1PENTENE'
        if (hFld.eq.'DOCOSANE')             hFld='C22'
        if (hFld.eq.'DODECANE')             hFld='C12'
        if (hFld.eq.'UNDECANE')             hFld='C11'
      elseif (i.eq.9) then
        if (hFld.eq.'1BUTYLENE')            hFld='1BUTENE'
        if (hFld.eq.'CISBUTENE')            hFld='C2BUTENE'
        if (hFld.eq.'DEUTERIUM')            hFld='D2'
        if (hFld.eq.'ISOBUTANE')            hFld='ISOBUTAN'
        if (hFld.eq.'ISOBUTENE')            hFld='IBUTENE'
        if (hFld.eq.'ISOHEXANE')            hFld='IHEXANE'
        if (hFld.eq.'ISOOCTANE')            hFld='IOCTANE'
        if (hFld.eq.'PROPANONE')            hFld='ACETONE'
        if (hFld.eq.'PROPYLENE')            hFld='PROPYLEN'
      elseif (i.eq.10) then
        if (hFld.eq.'CIS2BUTENE')           hFld='C2BUTENE'
        if (hFld.eq.'HEAVYWATER')           hFld='D2O'
        if (hFld.eq.'HEXADECANE')           hFld='C16'
        if (hFld.eq.'ISOPENTANE')           hFld='IPENTANE'
        if (hFld.eq.'NEOPENTANE')           hFld='NEOPENTN'
      elseif (i.eq.11) then
        if (hFld.eq.'CYCLOHEXANE')          hFld='CYCLOHEX'
        if (hFld.eq.'TRANSBUTENE')          hFld='T2BUTENE'
      elseif (i.eq.12) then
        if (hFld.eq.'CYCLOPENTANE')         hFld='CYCLOPEN'
        if (hFld.eq.'CYCLOPROPANE')         hFld='CYCLOPRO'
        if (hFld.eq.'DIETHYLETHER')         hFld='DEE'
        if (hFld.eq.'ETHYLALCOHOL')         hFld='ETHANOL'
        if (hFld.eq.'ETHYLBENZENE')         hFld='EBENZENE'
        if (hFld.eq.'FLUOROETHANE')         hFld='R161'
        if (hFld.eq.'METHYLOLEATE')         hFld='MOLEATE'
        if (hFld.eq.'NITROUSOXIDE')         hFld='N2O'
        if (hFld.eq.'NOVEC6491230')         hFld='NOVEC649'
        if (hFld.eq.'PARAHYDROGEN')         hFld='PARAHYD'
        if (hFld.eq.'TRANS2BUTENE')         hFld='T2BUTENE'
      elseif (i.eq.13) then
        if (hFld.eq.'CARBONDIOXIDE')        hFld='CO2'
        if (hFld.eq.'DIMETHYLETHER')        hFld='DME'
        if (hFld.eq.'FLUOROMETHANE')        hFld='R41'
        if (hFld.eq.'METHYLALCOHOL')        hFld='METHANOL'
        if (hFld.eq.'METHYLBENZENE')        hFld='TOLUENE'
        if (hFld.eq.'ORTHOHYDROGEN')        hFld='ORTHOHYD'
        if (hFld.eq.'SULFURDIOXIDE')        hFld='SO2'
      elseif (i.eq.14) then
        if (hFld.eq.'CARBONMONOXIDE')       hFld='CO'
        if (hFld.eq.'CHLOROETHYLENE')       hFld='VINYLCHLORIDE'
        if (hFld.eq.'DIETHANOLAMINE')       hFld='DEA'
        if (hFld.eq.'DEUTERIUMOXIDE')       hFld='D2O'
        if (hFld.eq.'DICHLOROETHANE')       hFld='R150'
        if (hFld.eq.'ETHYLENEGLYCOL')       hFld='EGLYCOL'
        if (hFld.eq.'HYDROGENNORMAL')       hFld='HYDROGEN'
        if (hFld.eq.'METHOXYMETHANE')       hFld='DME'
        if (hFld.eq.'METHYLCHLORIDE')       hFld='R40'
        if (hFld.eq.'METHYLSTEARATE')       hFld='MSTEARAT'
      elseif (i.eq.15) then
        if (hFld.eq.'CARBONYLSULFIDE')      hFld='COS'
        if (hFld.eq.'HYDROGENSULFIDE')      hFld='H2S'
        if (hFld.eq.'METHYLLINOLEATE')      hFld='MLINOLEA'
        if (hFld.eq.'METHYLPALMITATE')      hFld='MPALMITA'
        if (hFld.eq.'PERFLUOROBUTANE')      hFld='C4F10'
        if (hFld.eq.'PERFLUOROHEXANE')      hFld='C6F14'
        if (hFld.eq.'PROPYLENEGLYCOL')      hFld='PGLYCOL'
        if (hFld.eq.'RE347MCCHFE7000')      hFld='RE347MCC'
      elseif (i.eq.16) then
        if (hFld.eq.'HYDROGENCHLORIDE')     hFld='HCL'
        if (hFld.eq.'METHYLLINOLENATE')     hFld='MLINOLEN'
        if (hFld.eq.'MONOETHANOLAMINE')     hFld='MEA'
        if (hFld.eq.'PERFLUOROPENTANE')     hFld='C5F12'
      else
        if (hFld.eq.'DIMETHYLCARBONATE')    hFld='DMC'
        if (hFld.eq.'METHYLCYCLOHEXANE')    hFld='C1CC6'
        if (hFld.eq.'PROPYLCYCLOHEXANE')    hFld='C3CC6'
        if (hFld.eq.'SULFURHEXAFLUORIDE')   hFld='SF6'
        if (hFld.eq.'NITROGENTRIFLUORIDE')  hFld='NF3'
        if (hFld.eq.'TRIFLUOROIODOMETHANE') hFld='CF3I'
        if (hFld.eq.'HEXAMETHYLDISILOXANE')           hFld='MM'
        if (hFld.eq.'OCTAMETHYLTRISILOXANE')          hFld='MDM'
        if (hFld.eq.'DECAMETHYLTETRASILOXANE')        hFld='MD2M'
        if (hFld.eq.'DODECAMETHYLPENTASILOXANE')      hFld='MD3M'
        if (hFld.eq.'TETRADECAMETHYLHEXASILOXANE')    hFld='MD4M'
        if (hFld.eq.'OCTAMETHYLCYCLOTETRASILOXANE')   hFld='D4'
        if (hFld.eq.'DECAMETHYLCYCLOPENTASILOXANE')   hFld='D5'
        if (hFld.eq.'DODECAMETHYLCYCLOHEXASILOXANE')  hFld='D6'
      endif

c  Check for old files names that are no longer in use and have been renamed.
      if (hFld.eq.'R1234ZE') then
        call ERRNUM(901,0,hFld(1:7),'R1234ZEE',0d0,0d0,xerr,ierr,herr)
        hFld='R1234ZEE.FLD'
        hFl=hFl(1:k)//hFld
        goto 510
      elseif (hFld.eq.'R1336MZZ') then
        call ERRNUM(901,0,hFld(1:8),'R1336MZZZ',0d0,0d0,xerr,ierr,herr)
        hFld='R1336MZZZ.FLD'
        hFl=hFl(1:k)//hFld
        goto 510
      endif

c  Check for missing '.fld'.
 520  if (INDEX(UCASE(hFl),'.FLD').eq.0) then
        call FINDLAST (hFl,' ',-255,j)
        hFl=hFl(1:j)//'.FLD'
      endif
      hFldFileName(icomp)=hFl(k+1:)

c  Check if the input name hFl was renamed based on the checks above.
      if (hFld.eq.hFld2) RETURN

c  If changed, add the directory structure at the beginning and load into a common block variable.
 500  if (INDEX(hFld,'.FLD').eq.0) then
        call FINDLAST (hFld,' ',-255,j)
        hFld=hFld(1:j)//'.FLD'
      endif
 510  hFldFileName(icomp)=hFld
      hAltFluidName=hFld

      end                                            subroutine CHKNAMES
c
c ======================================================================
c
      subroutine LIMITX (htyp,T,D,P,z,Tmin,Tmax,Dmax,Pmax,ierr,herr)
c
c  Returns limits of a property model as a function of composition
c  and/or checks inputs T, D, and P against those limits.
c
c  Pure fluid limits are read in from the *.fld files; for mixtures, a
c  simple mole fraction weighting of the reduced variables is used.
c
c  Attempting calculations below the minimum temperature and/or above
c  the maximum density may result in an error.  These will often
c  correspond to a physically unreasonable state; also many equations of
c  state do not extrapolate reliably to lower T's and higher D's.
c
c  A warning is issued if the temperature is above the maximum but below
c  1.5 times the maximum.  Pressures up to twice the maximum
c  result in only a warning.  Most equations of state may be
c  extrapolated to higher T's and P's.  Temperatures and/or pressures
c  outside these extended limits will result in an error.
c
c  When calling with an unknown temperature, set T to -1 to avoid performing
c  the melting line check.  If inputs are not available, use T=300, P=0, and D=0.
c
c  If multiple inputs are outside limits, ierr=SUM(ABS(ierr)),
c  with a positive sign if any error greater than zero (calculations not
c  possible), or a negative sign for warnings only.
c
c  Inputs:
c     htyp--Flag indicating the model to check (character*3):
c           'EOS' - Equation of state
c           'ETA' - Viscosity
c           'TCX' - Thermal conductivity
c           'STN' - Surface tension
c        T--Temperature [K]
c        D--Molar density [mol/L]
c        P--Pressure [kPa]
c        z--Composition array (array of mole fractions)
c
c  Outputs:
c     Tmin--Minimum temperature for model specified by htyp [K]
c     Tmax--Maximum temperature [K]
c     Dmax--Maximum density [mol/L]
c     Pmax--Maximum pressure [kPa]
c     ierr--Error flag:  0 - All inputs within limits
c                       -1 - 1.5*Tmax > T > Tmax
c                        1 - T < Tmin or T > 1.5*Tmax
c                        2 - D > Dmax or D < 0
c                       -4 - 2*Pmax > P > Pmax
c                        4 - P < 0 or P > 2*Pmax
c                        8 - Component composition < 0 or > 1 and/or composition sum <> 1
c                       16 - P>Pmelt
c                      -16 - T<Ttrp (important for water)
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  05-30-96  MM, original version

      include 'COMMONS.INC'
      dimension z(ncmax)
      character*255 herrt,herrd,herrp,herrx,herr,hb,htyp*3
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::LIMITX

c  Initialize flags and strings.
      ierr =0
      ierrt=0
      ierrd=0
      ierrp=0
      ierrx=0
      herr=' '
      herrt=' '
      herrd=' '
      herrp=' '
      herrx=' '
      nchart=0
      nchard=0
      ncharp=0
      ncharx=0
      call ISPURE (z,icomp)
      if (nc.gt.1) then
        do i=1,nc
          if (z(i).eq.-999.) icomp=ncmax+i         !Special flag to specify that the reference fluid should be checked.
        enddo
      endif


c  General mixture case.
      if (icomp.eq.0) then
        zsum=SUM(z(1:nc))
        do i=1,nc
          if (z(i).lt.-1d-10 .or. z(i).gt.1.0000000001d0) ierrx=8
        enddo
        if (zsum.lt.0.99995d0 .or. zsum.gt.1.00005d0) ierrx=8
      endif

      call LIMITS (htyp,z,Tmin,Tmax,Dmax,Pmax)

c  Get melting and sublimation pressures.
      Pmlt=0d0
      Psbl=0d0
      if (icomp.ne.0 .and. (htyp.eq.'EOS' .or. htyp.eq.'eos')) then
        if (T.gt.Tmin .and. P.gt.1d-10 .and. hmeltk(icomp).ne.'NBS')
     &    call MELTK(icomp,T,Pmlt,ierr,herr)
        if (T.le.Ttpn(icomp) .and. hsublk(icomp).ne.'NBS')           !Must use Ttpn here to account for water.
     &    call SUBLK(icomp,T,Psbl,ierr,herr)
      endif

c  Check inputs against lower temperature limits.
      Tmn=Tmin
      iw=0
      if(iFluidID(icomp).eq.1.or.iFluidID(icomp).eq.3)iw=iFluidID(icomp)
      if (iw.ge.1 .and. P.lt.Ptpn(icomp)) Tmn=Ttpn(icomp)  !For water, use triple point temperature only for P<Ptrp.
      if (T.lt.Tmn-0.01d0 .and. ABS(T+1d0).gt.1d-15) then
        if (P.gt.Psbl*1.0001d0 .or. Psbl.eq.0d0) then      !Set error number if P>Psubl.
          if (icomp.eq.0) then
            do i=1,nc-1
              do j=i+1,nc
                if (ABS(Tcrit(i)-Tcrit(j)).gt.50) then
                  call REDX (z,Tred,Dred)
                  Tmn=Tmn-5d0
                  if (Tred.gt.200) Tmn=Tmn-20d0
                  EXIT
                endif
              enddo
            enddo
          endif
          if (T.lt.Tmn) then
            iextr=1          !Set EOS and transport error messages about extrapolating to lower temperatures.
            if (htyp.eq.'STN' .or. htyp.eq.'stn') iextr=-1           !Surface tension may be extrapolated to lower T's.
            call ERRNUM (iextr,4,' ','below',T,Tmn,0d0,ierrt,herrt)
            nchart=75
          endif
        endif

c  Check inputs against higher temperature limits.
      elseif (T.gt.1.5d0*Tmax+1d-2) then
        nchart=73
        call ERRNUM (1,8,' ',' ',T,Tmax,0d0,ierrt,herrt)
      elseif (T.gt.Tmax+1d-2) then
        nchart=73
        call ERRNUM (-1,7,' ',' ',T,Tmax,0d0,ierrt,herrt)
      endif

c  Check for cases where the temperature is less than the triple point temperature,
c   but still in a valid liquid region (e.g., water between 251 and 273 K).
      if (iw.ge.1) then
        if (T.lt.Ttpn(icomp)-1d-3 .and. D.gt.Dtpn(icomp)+1d-6) then
          if (iw.eq.1) call MLTH2O (T,P1,P2)
          if (iw.eq.3) call MLTD2O (T,P1,P2)
          if ((P.lt.P2*0.9999d0 .or. P.gt.P1/0.9999d0).and.P.ne.0.)then
            nchart=34
            call ERRNUM (16,2,' ',' ',0d0,0d0,0d0,ierrt,herrt)
          elseif (T.gt.Tmin .or. P.eq.0) then
            ierrt=0
            nchart=0
          endif
        endif
      endif

      if (D.lt.0.) then
        nchard=38
        call ERRNUM (2,1,' ',' ',D,0d0,0d0,ierrd,herrd)
      elseif (D.gt.Dmax+1d-6) then
        nchard=78
        call ERRNUM (2,5,' ',' ',D,Dmax,0d0,ierrd,herrd)
      endif

      if (P.lt.0.) then
        ncharp=44
        call ERRNUM (4,1,' ',' ',P/1000d0,0d0,0d0,ierrp,herrp)
      elseif (P.gt.2d0*Pmax+1d-2) then
        ncharp=79
        call ERRNUM (4,7,' ',' ',P/1000d0,Pmax/1000d0,0d0,ierrp,herrp)
      elseif (P.gt.Pmax+1d-2) then
        ncharp=79
        call ERRNUM (-4,6,' ',' ',P/1000d0,Pmax/1000d0,0d0,ierrp,herrp)
      endif

      if (P*0.9995d0.gt.Pmlt .and. ABS(Pmlt).gt.1d-20) then
        if (icomp.ne.0 .or. P.gt.10000) then
          ncharp=76
          call ERRNUM (4,5,' ',' ',P/1000d0,Pmlt/1000d0,0d0,ierrp,herrp)
        endif
      endif

      if (ierrx.ne.0) then
        ncharx=131
        write (hb,'(5f13.10)') (z(i),i=1,MIN(nc,5))
        call ERRNUM (8,0,' ',hb,zsum,0d0,0d0,ierrx,herrx)
      endif

c  Compose error string and compute overall value of ierr.
      ierr=ABS(ierrt)+ABS(ierrd)+ABS(ierrp)+ABS(ierrx)
      if (ignbnd.ne.0) ierr=-ABS(ierr)
      if (ierr.ne.0) then
        if (ierrt.le.0 .and. ierrd.le.0 .and.
     &      ierrp.le.0 .and. ierrx.le.0) ierr=-ierr
        if (nchart.gt.1) nchart=INDEX(herrt,';')
        if (nchard.gt.1) nchard=INDEX(herrd,';')
        if (ncharp.gt.1) ncharp=INDEX(herrp,';')
        herr='One or more inputs are out of range;'//herrt(1:nchart)//
     &        herrd(1:nchard)//herrp(1:ncharp)//herrx(1:ncharx)
        i=INDEX(herr,';    ')          !Change the semicolon at end of line to a period.
        if (i.gt.2) herr(i:i)='.'
      endif

      end                                              subroutine LIMITX
c
c ======================================================================
c
      subroutine LIMITK (htyp,icomp,T,D,P,Tmin,Tmax,Dmax,Pmax,ierr,herr)
c
c  This function is deprecated.  Use subroutine LIMITX instead.

      include 'COMMONS.INC'
      dimension z(ncmax)
      character htyp*3,herr*255

      z(1:nc)=0d0
      if (icomp.le.ncmax) then
        z(icomp)=1d0
      else
        z(icomp-ncmax)=-999d0          !If icomp is negative, set flag so that LIMITX knows to check the reference fluid.
      endif
      call LIMITX (htyp,T,D,P,z,Tmin,Tmax,Dmax,Pmax,ierr,herr)
      end                                              subroutine LIMITK
c
c ======================================================================
c
      subroutine LIMITS (htyp,z,Tmin,Tmax,Dmax,Pmax)
c
c  Returns limits of a property model as a function of composition.
c  Pure fluid limits were read in from the *.fld files; for mixtures, a
c  simple mole fraction weighting in reduced variables is used.
c
c  Inputs:
c     htyp--Flag indicating which models are to be checked (character*3)
c           'EOS' - Equation of state for thermodynamic properties
c           'ETA' - Viscosity
c           'TCX' - Thermal conductivity
c           'STN' - Surface tension
c        z--Composition array (array of mole fractions)
c
c  Outputs:
c     Tmin--Minimum temperature for model specified by htyp [K]
c     Tmax--Maximum temperature [K]
c     Dmax--Maximum density [mol/L]
c     Pmax--Maximum pressure [kPa]
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  10-22-99 EWL, original version

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      include 'COMANC.INC'
      character htyp*3,htyp1*3,UCASE*255
      dimension z(ncmax),Tmn(nq),Tmx(nq),Dmx(nq),Pmx(nq)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::LIMITS

      call ISPURE (z,icomp)
      if (nc.gt.1) then
        do i=1,nc
          if (z(i).eq.-999.) icomp=ncmax+i         !Special flag to specify that the reference fluid should be checked.
        enddo
      endif
      Tmin=0d0
      Tmax=0d0
      Dmax=0d0
      Pmax=0d0
      htyp1=UCASE(htyp)

c  Pure fluid EOS
      if (icomp.ne.0) then

c  Equation of state
        Tmin=Tmeos(icomp)
        Tmax=Txeos(icomp)
        Dmax=Dxeos(icomp)
        Pmax=Pxeos(icomp)

        if (htyp1.eq.'EOS') then
          if (iFluidID(icomp).eq.1 .or. iFluidID(icomp).eq.3) then   !Check for water
            Tmin=-999d0
            if (iFluidID(icomp).eq.1) call MLTH2O (Tmin,P1,D1)
            if (iFluidID(icomp).eq.3) call MLTD2O (Tmin,P1,D1)
          endif

c  Viscosity
        elseif (htyp1.eq.'ETA') then
          Tmin=Tmeta(icomp)
          Tmax=Txeta(icomp)
          Dmax=Dxeta(icomp)
          Pmax=Pxeta(icomp)
          if (hetak(icomp).eq.'ECS') then
            Tmin=Tmecst(icomp)
            Tmax=Txecst(icomp)
            Dmax=Dxecst(icomp)
            Pmax=Pxecst(icomp)
          endif

c  Thermal conductivity
        elseif (htyp1.eq.'TCX') then
          Tmin=Tmtcx(icomp)
          Tmax=Txtcx(icomp)
          Dmax=Dxtcx(icomp)
          Pmax=Pxtcx(icomp)
          if (htcxk(icomp).eq.'ECS') then
            Tmin=Tmecst(icomp)
            Tmax=Txecst(icomp)
            Dmax=Dxecst(icomp)
            Pmax=Pxecst(icomp)
          endif

c  Surface tension
        elseif (htyp1.eq.'STN') then
          Tmin=0d0
          Tmax=0d0
          Dmax=0d0
          Pmax=0d0
          if (ntermu(icomp).ne.0) Tmax=Tzred(icomp)
        end if
        RETURN
      endif


c --- Mixtures ---

c  Equation of state
      if (htyp1.eq.'EOS') then
        Tmn(1:nc)=Tmeos(1:nc)
        Tmx(1:nc)=Txeos(1:nc)
        Dmx(1:nc)=Dxeos(1:nc)
        Pmx(1:nc)=Pxeos(1:nc)
      elseif (htyp1.eq.'TRP') then     !Equation of state but with maximum density equal to the triple point density.
        do i=1,nc
          if (Dtpn(i).gt.0.1d0) Dmx(i)=Dtpn(i)
        enddo

c  Viscosity
      elseif (htyp1.eq.'ETA') then
        Tmn=Tmeta
        Tmx=Txeta
        Dmx=Dxeta
        Pmx=Pxeta
        do i=1,nc
          if (Txeta(i).eq.0d0) RETURN
          if (hetak(i).eq.'ECS') then
            Tmn(i)=Tmecst(i)
            Tmx(i)=Txecst(i)
            Dmx(i)=Dxecst(i)
            Pmx(i)=Pxecst(i)
          endif
        enddo

c  Thermal conductivity
      elseif (htyp1.eq.'TCX') then
        Tmn=Tmtcx
        Tmx=Txtcx
        Dmx=Dxtcx
        Pmx=Pxtcx
        do i=1,nc
          if (Txtcx(i).eq.0d0) RETURN
          if (htcxk(i).eq.'ECS') then
            Tmn(i)=Tmecst(i)
            Tmx(i)=Txecst(i)
            Dmx(i)=Dxecst(i)
            Pmx(i)=Pxecst(i)
          endif
        enddo

c  Surface tension
      elseif (htyp1.eq.'STN') then
        Tmn(1:nc)=0d0
        Tmx(1:nc)=Tzred(1:nc)
        Dmx(1:nc)=0d0
        Pmx(1:nc)=0d0
        do i=1,nc
          if (ntermu(i).eq.0) RETURN
        enddo
      endif


      do i=1,nc
        if (Tmn(i).le.0.) Tmn(i)=1d0
        if (Tmx(i).le.0.) Tmx(i)=1d0
      enddo

      taumin=SUM(z(1:nc)*Tzred(1:nc)/Tmn(1:nc))
      taumax=SUM(z(1:nc)*Tzred(1:nc)/Tmx(1:nc))
      delmax=SUM(z(1:nc)*Dmx(1:nc)*Vzred(1:nc))
      Pmax  =SUM(z(1:nc)*Pmx(1:nc))
      if (taumin.le.0.) taumin=100d0
      if (taumax.le.0.) taumax=300d0
      call REDX (z,trmix,Drmix)
      Tmin=trmix/taumin
      Tmax=trmix/taumax
      Dmax=Drmix*delmax
      if (Tmin.gt.10.) Tmin=Tmin-5d0
      if (Tmin.lt.0.)  Tmin=100d0
      if (Tmax.lt.0.)  Tmax=300d0

c  Calculate the lower temperature limit for the ammonia/water binary mixture.
      if (iAmWat.gt.0 .and. nc.eq.2) then
        i=iAmWat
        if (z(i).lt.0.33367d0) then
          Tmin=273.16d0*(1d0-0.3439823d0*z(i)-
     &         1.3274271d0*z(i)**2-274.973d0*z(i)**7)
        elseif (z(i).lt.0.58396d0) then
          Tmin=193.549d0*(1d0-4.987368d0*(z(i)-0.5d0)**2)
        elseif (z(i).lt.0.81473d0) then
          Tmin=194.380d0*(1d0-4.886151d0*(z(i)-2d0/3d0)**2+
     &         10.37298d0*(z(i)-2d0/3d0)**3)
        else
          Tmin=195.495d0*(1d0-0.323998d0*(1d0-z(i))-
     &         15.87560d0*(1d0-z(i))**4)
        endif
      endif

      do i=1,nc
        if (z(i).gt.0. .and. (Pmx(i).le.0. or. Dmx(i).le.0.)) then
          if (htyp1.ne.'STN') then
            Pmax=0d0
            Dmax=0d0
            Tmax=300d0
            Tmin=300d0
            EXIT
          endif
        endif
      enddo

      end                                              subroutine LIMITS
c
c ======================================================================
c
      subroutine SETEOSPURE (i,hmx)
      include 'COMMONS.INC'
      include 'COMFEQ.INC'
      character*3 hmx
      iEOSk(i)=0
      if (hmx.eq.'HMX') then
        iEOSk(i)=iEOSHMX
      elseif (hmx(1:2).eq.'FE') then
        iEOSk(i)=iEOSFEQ
      elseif (hmx.eq.'AGA') then
        iEOSk(i)=iEOSAGA
      elseif (hmx.eq.'PR ' .or. hmx.eq.'PRT') then
        iEOSk(i)=iEOSPR
      elseif (hmx.eq.'PR3') then
        iEOSk(i)=iEOSPR3
      elseif (hmx.eq.'ECS') then
        iEOSk(i)=iEOSECS
      elseif (hmx.eq.'NBS') then
        iEOSk(i)=iEOSNBS
      elseif (hmx.eq.'BWR') then
        iEOSk(i)=iEOSBWR
      endif
      end                                          subroutine SETEOSPURE
c
c ======================================================================
c
      subroutine SETEOSMX (hmx)
      include 'COMMONS.INC'
      character*3 hmx
      heosmx=hmx
      iEOSmx=0
      if (hmx.eq.'HMX') then
        iEOSmx=iEOSHMX
      elseif (hmx(1:2).eq.'FE') then
        iEOSmx=iEOSFEQ
      elseif (hmx.eq.'AGA') then
        iEOSmx=iEOSAGA
      elseif (hmx.eq.'PR ' .or. hmx.eq.'PRT') then
        iEOSmx=iEOSPR
      elseif (hmx.eq.'PR3') then
        iEOSmx=iEOSPR3
      elseif (hmx.eq.'ECS') then
        iEOSmx=iEOSECS
      elseif (hmx.eq.'NBS') then
        iEOSmx=iEOSNBS
      elseif (hmx.eq.'BWR') then
        iEOSmx=iEOSBWR
      endif
      if (hmx.ne.'AGA') heox=hmx
      end                                            subroutine SETEOSMX
c
c ======================================================================
c
      subroutine CHECKNAN (x)
      include 'CONSTS.INC'
      iNan=0
      if (x+1.d0.eq.x) iNan=1
      if (iNan.eq.1 .and. (ABS(x).gt.1d15 .and. x.lt.1d299)) iNan=0
      if (x.le.-1d37 .or. x.ge.1d37) iNan=0
      if (x.ne.x) iNan=1
      if (x*0.ne.0) iNan=1
c     if (isinf(x)) iNan=1
c     if (isnan(x).ne.0) iNan=1
c     if (iNan.eq.1) pause 'NaN'                 !If your compiler complains about this line, just delete it.
      end                                            subroutine CHECKNAN
c
c ======================================================================
c
      subroutine IDHCONV (v,k,kflg,i,j,hx)
c
c  Convert an integer or double precision value to a string.
c
c  Inputs:
c        v--Double precision value to be converted to a string.
c        k--Integer value to be converted to a string.
c     kflg--Set to 1 to convert the integer value.
c           Set to 2 to convert the double precision value.
c
c  Outputs:
c        i--Starting location in the string containing digits.
c        j--Ending location in the string containing digits.
c       hx--Output string containing the converted number.

      include 'COMMONS.INC'
      character hx*13

      i=2
      j=2
      if (kflg.eq.1) then
        write (hx,*) k                 !Convert the integer contained in k to a string.
        do m=12,1,-1
          j=m+1
          if (hx(m:m).eq.' ') EXIT     !Find the location of the last digit in the string.
        enddo
        RETURN
      endif

      if (v.ne.0.) then
        write (hx,'(g13.6)') v         !Convert the value in v to a string.
        do m=1,13
          i=m
          if (hx(m:m).ne.' ') EXIT     !Find the starting location of the first digit or character in the string.
        enddo
        do m=13,1,-1
          j=m
          if (hx(m:m).ne.' ') EXIT     !Find the location of the last digit.
        enddo
        if (i.gt.j) i=j
        if (hx(j:j).eq.'.') j=j-1
      else
        hx=' 0'
      endif
      end                                             subroutine IDHCONV
c
c ======================================================================
c
      subroutine ERRNUM (i,ihr,ha,hb,x,y,z,ierr,herr)
c
c  Write out the error message that corresponds to the error number sent
c  to this routine.
c
c  Inputs:
c        i--Error number
c      ihr--Sub-error number for certain error types, such as temperature
c           out of bounds.  Can also be used to send an integer that is
c           included in the error string.
c       ha--First string containing information to be included in the error message.
c       hb--Second string (both have no specified length)
c        x--Double precision number that will be included in the error message.
c        y--Second double that can be included.
c        z--Third double
c
c  Outputs:
c     ierr--Error number, set equal to the value of i
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-11-17 EWL, original version

      include 'COMMONS.INC'
      character ha*(*),hb*(*),herr*255

      if (z.eq.999.) then
        if (iersav2.ne.0) then
          iersav3=999                  !Permanently save the most important error until new Refprop routines are called.
          ierr=iersav2                 !This only happens when calling SETUP.
          herr=hersav2
        endif
        RETURN
      endif
      iersav3=0

c...Check for NaN.  Only a value of NaN will make the following true.
      if (lNan) then
        call CheckNaN (x)
        call CheckNaN (y)
        call CheckNaN (z)
      endif

c  No error message was sent.
      if (i.eq.0) then
        ierr=0
        herr=' '
        iersav=0
        RETURN
      endif

c  An error message was sent, save the pieces for later use.
      ierr=i
      iersav=ierr
      xersav=x
      yersav=y
      zersav=z
      ihrsav=ihr
      ihasav=len(ha)
      ihbsav=len(hb)
      hasav=ha
      hbsav=hb
      if (z.eq.998. .and. ierr.eq.iersav2 .and. ihrsav.eq.ihrsav2) then        !Special flag to tell the routine to send back an error message from a previously called section that asked for the error message to be saved.
        zersav=0d0
        herr=hersav2
        RETURN
      endif
      if (iErrNum.gt.0 .and. iErrPrnt.eq.0 .and. z.ne.xerr) RETURN   !An error message was sent, but is not the final message so just exit.

c  An error message was sent and should be written to the herr variable.
      call ERRMSG (ierr,herr)
      if (z.eq.xerr .and. ierr.ne.0) then        !Special flag to tell the routine to save the current error number and message for the code below.
        if (iersav2.eq.0 .or. iersav2.lt.ierr) then
          zersav=0d0
          iersav2=ierr
          ihrsav2=ihrsav
          hersav2=herr
        endif
      endif

      end                                              subroutine ERRNUM
c
c ======================================================================
c
      subroutine ERRNUM2 (ierr,ihr,ha,hb,hx,hy,hz,herr)
c
c  Part of the ERRNUM subroutine, not to be called by other routines.
c
      include 'COMMONS.INC'
      character ha*(*),hb*(*),hx*(*),hy*(*),hz*(*),hu1*9,hu2*9
      character herr*255,hj*13

      i1=1
      j1=1
      if (ierr.eq.248 .or. ierr.eq.17) then
        hu1=' '
        hu2=' '
        if (ha(1:1).eq.'T') hu1=' K'
        if (ha(1:1).eq.'P') hu1=' MPa'
        if (ha(1:1).eq.'D') hu1=' mol/l'
        if (ha(1:1).eq.'E') hu1=' J/mol'
        if (ha(1:1).eq.'H') hu1=' J/mol'
        if (ha(1:1).eq.'S') hu1=' J/mol-K'
        if (ha(2:2).eq.'T') hu2=' K'
        if (ha(2:2).eq.'P') hu2=' MPa'
        if (ha(2:2).eq.'D') hu2=' mol/l'
        if (ha(2:2).eq.'E') hu2=' J/mol'
        if (ha(2:2).eq.'H') hu2=' J/mol'
        if (ha(2:2).eq.'S') hu2=' J/mol-K'
        i1=INDEX(hu1(2:),' ')+1
        j1=INDEX(hu2(2:),' ')
      endif


c  Errors from CHECKLIMITS, which must be placed first because these two lines check ihr, not ierr.
c  These two check the value of ihr, not ierr.
      if (ihr.eq.777) then
        call IDHCONV (0d0,ierr,1,i,j,hj)
        herr='['//ha//' warning '//hj(j:12)//'] '//hb

      elseif (ihr.eq.888) then
        call IDHCONV (0d0,ierr,1,i,j,hj)
        herr='['//ha//' error '//hj(j:12)//'] '//hb

c  Error code 1  --  Temperature out of bounds.
      elseif (ABS(ierr).eq.1 .and. ihr.eq.4) then
        herr=' Temperature '//hb//' triple-point or minimum '//
     &       'temperature: T = '//hx//' K, Tmin = '//hy//' K;'
        if (ha.ne.' ') herr='['//ha//' error 1]'//herr

      elseif (ierr.eq.-1 .and. ihr.eq.7) then
        herr=' Temperature above upper limit: T = '//hx//' K, Tmax = '//
     &        hy//' K;'
        if (ha.ne.' ') herr='['//ha//' warning 1]'//herr

      elseif (ierr.eq.1 .and. ihr.eq.8) then
        herr=' Temperature above 1.5x(upper limit): T = '//hx//
     &       ' K, Tmax = '//hy//' K;'
        if (ha.ne.' ') herr='['//ha//' error 1]'//herr



c  Error code 2  --  Density out of bounds.
      elseif (ierr.eq.2 .and. ihr.eq.1) then
        herr=' Density below zero: D = '//hx//' mol/L;'
        if (ha.ne.' ') herr='['//ha//' error 2]'//herr

      elseif (ierr.eq.2 .and. ihr.eq.2) then
        herr=' Density below triple-point vapor '//
     &       'density: D = '//hx//' mol/L, Dtrp = '//hy//' mol/L.'
        if (ha.ne.' ') herr='['//ha//' error 2]'//herr

      elseif (ierr.eq.2 .and. ihr.eq.4) then
        herr=' Density above triple-point liquid density: '//
     &       'D = '//hx//' mol/L, Dtrp = '//hy//' mol/L.'
        if (ha.ne.' ') herr='['//ha//' error 2]'//herr

      elseif (ierr.eq.2 .and. ihr.eq.5) then
        herr=' Density above upper limit: D = '//hx//' mol/L, Dmax = '//
     &        hy//' mol/L;'
        if (ha.ne.' ') herr='['//ha//' error 2]'//herr

c  Error code 4  --  Pressure out of bounds.
      elseif (ierr.eq.4 .and. ihr.eq.1) then
        herr=' Pressure below zero: P = '//hx//' MPa;'
        if (ha.ne.' ') herr='['//ha//' error 4]'//herr

      elseif (ierr.eq.-4 .and. ihr.eq.2) then
        herr=' Pressure '//hb//' triple-point '//
     &       'pressure: P = '//hx//' MPa, Ptrp = '//hy//' MPa.'
        if (ha.ne.' ') herr='['//ha//' warning -4]'//herr

      elseif (ierr.eq.4 .and. ihr.eq.5) then
        herr=' Pressure above melting pressure: P = '//hx//
     &       ' MPa, Pmelt = '//hy//' MPa;'
        if (ha.ne.' ') herr='['//ha//' error 4]'//herr

      elseif (ierr.eq.-4 .and. ihr.eq.6) then
        herr=' Pressure above upper limit: P = '//hx//' MPa, Pmax = '//
     &        hy//' MPa;'
        if (ha.ne.' ') herr='['//ha//' warning -4]'//herr

      elseif (ierr.eq.4 .and. ihr.eq.7) then
        herr=' Pressure above 2x(upper limit): P = '//hx//
     &       ' MPa, Pmax = '//hy//' MPa;'
        if (ha.ne.' ') herr='['//ha//' error 4]'//herr


c  Errors 5-19   --  Other out of bound errors.

      elseif (ierr.eq.8) then
        herr=' Compositions out of range, xsum = '//hx//
     &          ' mol frac, x(i) = '//hb
        if (ha.ne.' ') herr='['//ha//' error 8]'//herr

      elseif (ierr.eq.16 .and. ihr.eq.2) then
        herr=' Inputs are within the solid phase;'

      elseif (ierr.eq.17) then
        herr='['//ha//'FLSH error 17] Input value of '//hx//hu2(1:j1)//
     &      ' for '//hb//' is outside limits: '//
     &       hb//'min = '//hy//hu2(1:j1)//' and '//hb//
     &      'max = '//hz//hu2(1:j1)//'.'

      elseif (ierr.eq.19) then
        herr='['//ha//'FLSH error 19] Input quality is out of '//
     &       'range (quality must be between 0 and 1): q = '//hx

      elseif (ierr.eq.-19) then
        herr='['//ha//' warning -19] Input quality out of range.'

c  ***  Other out of bound errors are given further down with codes between 50 and 100.
c       (This was done to keep the error codes for SETREF the same as version 9.1.)

c  Error codes 22-30  --  Errors in the SETREF routine.
      elseif (ierr.eq.22) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP warning 22] Minimum temperature for component '//
     &      hj(j:12)//' is greater than IIR reference temperature: '//
     &      'Tmin = '//hx//' K, Tref = 273.15 K; will use default: '//ha

      elseif (ierr.eq.23) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP warning 23] Critical temperature for component '//
     &     hj(j:12)//' is less than IIR reference temperature: '//
     &     'Tcrit = '//hx//' K, Tref = 273.15 K; will use default: '//ha

      elseif (ierr.eq.24) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP warning 24] Minimum temperature for component '//
     &   hj(j:12)//' is greater than ASHRAE reference temperature: '//
     &   'Tmin = '//hx//' K, Tref = 233.15 K; will use default: '//ha

      elseif (ierr.eq.25) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP warning 25] Critical temperature for component '//
     &     hj(j:12)//' is less than ASHRAE reference temperature: '//
     &     'Tcrit = '//hx//' K, Tref = 233.15 K; will use default: '//ha

      elseif (ierr.eq.26) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP warning 26] Minimum temperature for component '//
     &    hj(j:12)//' is greater than boiling point temperature: '//
     &    'Tmin = '//hx//' K, Tnbp = '//hy//' K; will use default: '//ha

      elseif (ierr.eq.-28) then
        herr='[SETREF warning -28] Cannot apply reference state '//
     &       '(DEF) at a specified mixture composition when '//
     &       'components have different default reference states; '//
     &       'will apply defaults to each pure component.'

      elseif (ierr.eq.-29) then
        herr='[SETREF warning -29] Reference state not found, '//
     &       'default reference state will be used.'



c  Error codes 50 to 99 -- Additional out of bound errors (except T>Tc and P>Pc, which are fatal).

      elseif (ierr.eq.54) then
        herr='[SATH error 54] Enthalpy below triple-point '//
     &       'value: h = '//hx//' J/mol, min = '//hy//' J/mol.'

      elseif (ierr.eq.55) then
        herr='[SATH error 55] Enthalpy greater than maximum '//
     &       'value: h = '//hx//' J/mol, max = '//hy//' J/mol.'

      elseif (ierr.eq.56) then
        herr='[SATH error 56] Enthalpy greater than triple point '//
     &       'value: h = '//hx//' J/mol, max = '//hy//' J/mol.'

      elseif (ierr.eq.66) then
        herr='[SATS error 66] Entropy below minimum entropy: '//
     &       's = '//hx//' J/mol-K, smin = '//hy//' J/mol-K.'

      elseif (ierr.eq.67) then
        herr='[SATS error 67] Entropy greater than maximum entropy: '//
     &       's = '//hx//' J/mol-K, smax = '//hy//' J/mol-K.'

      elseif (ierr.eq.68) then
        herr='[SATS error 68] Entropy less than triple point entropy'//
     &       ': s = '//hx//' J/mol-K, smin = '//hy//' J/mol-K.'

      elseif (ierr.eq.73) then
        herr='[TRNECS error 73] One or more inputs are out of '//
     &       'bounds for the ECS transport model.'

      elseif (ierr.eq.74) then
        herr='[TRNPRP error 74] Inputs to the viscosity and thermal '//
     &       'conductivity correlations are out of range.'

      elseif (ABS(ierr).eq.75) then
        herr='[TRNPRP '//ha//'] Inputs to the viscosity '//
     &    'correlation are out of range - results may be in error; '//hb

      elseif (ABS(ierr).eq.76) then
        herr='[TRNPRP '//ha//'] Inputs to the thermal conductivity '//
     &    'correlation are out of range - results may be in error; '//hb





c  Error codes 101 to 139, except 121 and 124  --  Setup errors.

      elseif (ierr.eq.101 .and. ihr.eq.101) then           !Error in reading *.mix file
        herr='['//ha//' error 101] Error in opening mixture file: '//hb

      elseif (ierr.eq.101) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP error 101] Error in opening file for component '//
     &        hj(j:12)//': filename = '//ha

      elseif (ierr.eq.-101) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP warning -101] Error in opening fluid file for '//
     &        'component '//hj(j:12)//', which may be required for '//
     &        'the calculation of transport properties: filename = '//ha

      elseif (ierr.eq.102 .and. ihr.eq.102) then
        herr='[SETUP error 102] Error in file or premature end of file'

      elseif (ierr.eq.102) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP error 102] Error in file or premature end of '//
     &       'file for component '//hj(j:12)//': '//ha

      elseif (ierr.eq.-102) then
        herr='[SETMIX warning -102] The mixture file contains '//
     &       'updated parameters, which have been ignored.'

      elseif (ierr.eq.-103) then
        herr='[SETUP warning -103] The HMX.BNC file may '//
     &       'contain data that cannot be read by this '//
     &       'version of Refprop.'

      elseif (ierr.eq.104) then
        herr='[REFPROP error 104] No fluid files have been loaded. '//
     &     'Please call SETUP, SETFLUIDS, SETMIX, or SETMIXTURE '//
     &     'to select the pure fluid or mixture of interest.'

      elseif (ierr.eq.105 .and. ihr.gt.100) then
        call IDHCONV (0d0,ihr-100,1,i,j,hj)
        herr='[SETUP error 105] Selected equation ('//ha//')'//
     &       ' not found for component #'//hj(j:12)

      elseif (ierr.eq.105) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP error 105] Selected equation of state ('//ha//')'//
     &       ' not found for component '//hj(j:12)//': '//hb

      elseif (ierr.eq.-105) then
        herr='[SETUP warning -105] Must use routine SETREF for (OTH) '//
     &       'reference state choice; default (DEF) will be used.'

      elseif (ierr.eq.107) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP error 107] Ideal-gas heat capacity equation not '//
     &       'found for component '//hj(j:12)//'.'

      elseif (ierr.eq.-107) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETUP warning -107] Unknown model '//ha//
     &       ' encountered in file for component '//hj(j:12)//'.'

      elseif (ierr.eq.108) then
        herr='[SETAGA error 108] Not all requested fluids are '//
     &       'available in AGA8; first one found: '//ha

      elseif (ierr.eq.109 .and. ihr.ge.0) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='['//ha//' error 109] Insufficient fluid names; n='//
     &        hj(j:12)//'.'

      elseif (ierr.eq.109) then
        herr='['//ha//' error 109] Too many fluid '//
     &      'names; additional text sent to routine: '//hb

      elseif (ierr.eq.112) then
        herr='[SETHMX error 112] Mixture file does not match model.'

      elseif (ierr.eq.113) then
        herr='['//ha//' error 113] Number of components out of bounds.'

      elseif (ierr.eq.114) then
        herr='[SETUP error 114] Number of components does not match '//
     &       'value sent to SETMOD.'

      elseif (ierr.eq.-114) then
        herr='[SETUP warning -114] Entry of "Air" for fluid name is '//
     &       'ambiguous; the pseudo-pure fluid file "Air.ppf" was '//
     &       'read in.  To use air as a mixture, send "Air.mix" '//
     &       'to the setup routines.'

      elseif (ierr.eq.116) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETFEQ error 116] Insufficient terms in EOS '//
     &       'for component '//hj(j:12)//'.'

      elseif (ierr.eq.117 .and. ihr.eq.1) then
        herr='[SETUP error 117] Estimation of mixing parameters is '//
     &       'not possible for most mixtures containing water, '//
     &       'helium, hydrogen, neon, carbon monoxide, '//
     &       'fluorine, methanol, or ethanol.'   !  However, the pure '//
c    &       'fluids were still loaded.'

      elseif (ierr.eq.117 .and. ihr.eq.2) then
        herr='[SETUP error 117] Mixture data have not been fitted '//
     &       'for one or more binary pairs in the specified mixture; '//
     &       'the mixture is outside the range of the model and '//
     &       'calculations will not be made.'    !  However, the pure '//
c    &       'fluids were still loaded.'

      elseif (ierr.eq.-117 .and. ihr.eq.10) then
        herr='[SETUP warning -117] Mixture data have not been fitted '//
     &       'for one or more binary pairs in the specified mixture; '//
     &       'the mixing parameters have been estimated.'

      elseif (ierr.eq.-117 .and. ihr.eq.80) then
        herr='[SETUP warning -117] Binary interaction parameters are '//
     &       'not presently available for this mixture; alternative '//
     &       'mixing rules from a similar substance have been used.'

      elseif (ierr.eq.118) then
        herr='['//ha//' error 118] Error in reading mixture file.'

      elseif (ierr.eq.119) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='[SETREF error 119] Convergence failure in calculating '//
     &       'reference state for component '//hj(j:12)//': '//ha

c  Error codes 121, 124, and 140 to 199  --  Errors in the saturation routines.
c  Errors 121 and 124 are kept here to be backwards compatible with 9.1.

      elseif (ierr.eq.121) then
        herr='['//ha//' error 121] Temperature input is greater than '//
     &       'the critical or maxcondentherm temperature: '//
     &       'T = '//hx//' K, Tc = '//hy//' K.'

      elseif (ierr.eq.124) then
        herr='[SATT error 124] Iteration for saturation state '//
     &       'did not converge for T = '//hx//' K.'

      elseif (ierr.eq.141) then
        herr='['//ha//' error 141] Pressure input to saturation '//
     &        'routine is greater than critical or maxcondentherm '//
     &        'pressure: P = '//hx//' MPa, Pc = '//hy//' MPa.'

      elseif (ierr.eq.142) then
        herr='['//ha//' error 142] Input values for pseudo-pure '//
     &       'fluid are two-phase; the full mixture model must be '//
     &       'used to calculate state properties.'

      elseif (ierr.eq.143) then
        herr='['//ha//' error 143] Input value equal to or less than '//
     &       'zero.'

      elseif (ierr.eq.144) then
        herr='[SATP error 144] Iteration for saturation state '//
     &       'did not converge for P = '//hx//' MPa.'

      elseif (ierr.eq.151) then
        herr='['//ha//' error 151] Iteration failed to converge.'

      elseif (ierr.eq.156) then
        herr='[SATTP error 156] Probable Type III mixture '//
     &       'with no liquid solution: P = '//hx//' MPa.'

      elseif (ierr.eq.159) then
        herr='['//ha//' error 159] Wrong input value for '//hb//'.'

      elseif (ierr.eq.160) then
        herr='[SATD error 160] Iteration for saturation state '//
     &       'did not converge for D = '//hx//' mol/L.'

      elseif (ierr.eq.172) then
        herr='[SATGV error 172] vf greater than 1 or less than 0.'

      elseif (ierr.eq.175) then
        herr='['//ha//' error 175] Method not yet implemented for '//hb

      elseif (ierr.eq.181) then
        herr='[SATH error 181] Iteration for saturation state did '//
     &     'not converge for h = '//hx//' J/mol.'

      elseif (ierr.eq.191) then
        herr='['//ha//' error 191] Derivative not available.'

      elseif (ierr.eq.192) then
        herr='[SATS error 192] Iteration for saturation state '//
     &       'given entropy did not converge for one or more roots.'





c  200's  --  Errors in the flash routines.

      elseif (ierr.eq.201) then
        herr='[TPRHO error 201] Illegal input to TPRHO (kph <= 0); '//
     &       'use TPFLSH instead.'

      elseif (ierr.eq.202) then
        herr='[TPRHO error 202] Liquid iteration has not converged '//
     &       'for T = '//hx//' K and P = '//hy//' MPa.'

      elseif (ierr.eq.203) then
        herr='[TPRHO error 203] Vapor iteration has not converged '//
     &       'for T = '//hx//' K and P = '//hy//' MPa.'

      elseif (ierr.eq.207) then
        herr='['//ha//' error 207] Density or pressure equal '//
     &       'to or less than zero, no solution available.'

      elseif (ierr.eq.208) then
        herr='['//ha//' error 208] Single-phase iteration has '//
     &       'not converged for D = '//hx//' mol/L.'

      elseif (ierr.eq.213) then
        herr='['//ha//' error 213] Density iteration did not '//
     &       'converge; '//hb

      elseif (ierr.eq.215) then
        herr='['//ha//' error 215] Supercritical density '//
     &       'iteration did not converge; '//hb

      elseif (ierr.eq.223) then
        herr='['//ha//' error 223] Bubble point calculation '//
     &       'did not converge; '//hb

      elseif (ierr.eq.224) then
        herr='['//ha//' error 224] Dew point calculation '//
     &       'did not converge; '//hb

      elseif (ierr.eq.226) then
        herr='['//ha//' error 226] 2-phase iteration did not '//
     &       'converge.'

      elseif (ierr.eq.-235) then
        herr='['//ha//' warning -235] Saturation state routine '//
     &      'failed to converge and thus the phase is unknown; '//
     &      'however, the state appears to be either single phase or '//
     &      'metastable.  Additional analysis is needed.'

      elseif (ierr.eq.238) then
        herr='['//ha//'FLSH error 238] No root exists in the liquid '//
     &       'phase for the input temperature '//hz//' K; '//hb//
     &       ' = '//hx//' and '//hb//'min = '//hy//' J/mol.'

      elseif (ierr.eq.-238) then
        herr='['//ha//'FLSH warning -238] Two solutions exist '//
     &       'for the input values; the higher density root has '//
     &       'been calculated.'

      elseif (ierr.eq.-239) then
        herr='['//ha//'FLSH warning -239] Two solutions exist '//
     &       'for the input values; the lower density root has '//
     &       'been calculated.'

      elseif (ierr.eq.248) then
        herr='['//ha//'FLSH error 248] Single-phase iteration did '//
     &      'not converge for '//ha(1:1)//' = '//hx//hu1(1:i1)//'and '//
     &       ha(2:2)//' = '//hy//hu2(1:j1)//'.'

      elseif (ierr.eq.260) then
        herr='[HSFLSH error 260] h-s inputs are two-phase or out of '//
     &  'bounds, iterative routine is not available to find a solution.'

      elseif (ierr.eq.-271) then
        herr='[MELTT warning -271] The properties returned from the '//
     &       'melting line routines for mixtures are NOT based on '//
     &       'actual measurements and do not represent the true '//
     &       'melting point.  They are for use only in bounding '//
     &       'the equation of state.'

      elseif (ierr.eq.290) then
        herr='[PHIDERVPR error 290] V+vtrans < b'




c  300's  --  Reducing line errors.

      elseif (ierr.eq.301) then
        herr='['//ha//' error 301] Mixing rule not found for i,j.'

      elseif (ierr.eq.311) then
        herr='['//ha//' error 311] Compositions are not '//
     &       'identical to that used in the call to SATSPLN.  '//
     &       'Estimated values returned instead.'

      elseif (ierr.eq.-311) then
        herr='['//ha//' warning -311] Compositions are not '//
     &       'identical to that used in the call to SATSPLN.'

      elseif (ierr.eq.312) then
        herr='['//ha//' error 312] sum(z)=0'

      elseif (ierr.eq.313) then
        herr='['//ha//' error 313] Coefficients are not available '//
     &       'for critical line calculations (which generally occurs '//
     &       'for binary mixtures that are not in the HMX.BNC, or '//
     &       'for beta versions of the program where the critical '//
     &       'lines have not yet been fitted).'

      elseif (ierr.eq.314) then
        herr='['//ha//' error 314] Failed to find critical point '//
     &       'from splines, estimated values based on previous fits '//
     &       'of the critical lines are returned.  '

      elseif (ierr.eq.315) then
       herr='['//ha//' error 315] The mixture may not have a critical'//
     &      ' point, the maximum saturation temperature was returned.'

      elseif (ierr.eq.-316) then
        herr='['//ha//' warning -316] The true critical point is '//
     &       'unknown.  Estimated values based on previous fits of '//
     &       'the critical lines were returned.  For a Type I binary '//
     &       'mixture, the values are generally close.  For other '//
     &       'Type I mixtures the values are rough estimates.'

      elseif (ierr.eq.317) then
        herr='['//ha//' error 317] Mixture appears to be Type III '//
     &       'because the critical point, maximum temperature, and '//
     &       'maximum pressure were not found, but the full spline '//
     &       'curve was calculated.  Large values were returned for '//
     &       'the critical point.'

      elseif (ierr.eq.318) then
        herr='['//ha//' error 318] Mixture is most likely Type III '//
     &       'as indicated by previous fits of the critical line. '//
     &       'Call the subroutine SATSPLN to determine the critical '//
     &       'point before calling CRITP.  Linear mixing rules were '//
     &       'used to return non-zero values.'

      elseif (ierr.eq.-319) then
        herr='['//ha//' warning -319] These values for the critical '//
     &       'point come from the predefined *.MIX file. If new pure '//
     &       'fluid or mixture models are used other than those from '//
     &       'the installed program, the values will not be '//
     &       'consistent with those models.'

      elseif (ierr.eq.321) then
        herr='['//ha//' error 321] No solution.'

      elseif (ierr.eq.331) then
        herr='['//ha//' error 331] Splines not available for '//
     &       'saturation calculations.'

      elseif (ierr.eq.332) then
        herr='[SATGUESS error 332] Initialize variable d72l first '//
     &       'before calling SATGUESS.'

      elseif (ierr.eq.355) then
        herr='[SATSPLN error 355] Spline routine failed.'

      elseif (ierr.eq.-361) then
        herr='[MAXT warning -361] Maximum temperature not known.'

      elseif (ierr.eq.-362) then
        herr='[MAXP warning -362] Maximum pressure not known.'





c  500's  --  Errors in the ancillary and transport routines.

      elseif (ierr.eq.501) then
        herr='['//ha//' error 501] Equation not available.'

      elseif (ierr.eq.502) then
        herr='['//ha//' error 502] Unknown model specified: '//hb

      elseif (ierr.eq.-503) then
        herr='['//ha//' warning -503] Two solutions exist for the '//
     &    'input property, the higher pressure state has been returned.'

      elseif (ierr.eq.504) then
        herr='['//ha//' error 504] Unknown model type specified: '//hb

      elseif (ierr.eq.-508) then
        herr='[TRNECS warning -508] Invalid region for viscosity of '//
     &       'reference fluid 1.'

      elseif (ierr.eq.510) then
        herr='[TRNPRP warning -510] Pure fluid correlation '//
     &       'out of range; attempted to use ECS method, it '//
     &       'generated error: '//ha

      elseif (ierr.eq.512) then
        herr='[TRNPRP error 512] Pure fluid correlation '//
     &       'for the reference fluid was not loaded, transport '//
     &       'properties will not be calculated.'

      elseif (ierr.eq.517) then
        herr='[SURFT error 517] CRITF did not converge.'

      elseif (ierr.eq.518) then
        herr='[SURFT error 518] Surface tension calculations for '//
     &       'mixtures with water or alcohols are not currently '//
     &       'supported.'

      elseif (ierr.eq.540) then
        herr='[TRNPRP error 540] Transport equations are not '//
     &       'available for one or more of the fluids.'

      elseif (ierr.eq.541) then
        herr='[TRNPRP error 541] Transport equations are not '//
     &       'available for mixtures with water at molar '//
     &       'concentrations greater than 5%.'

      elseif (ierr.eq.542) then
        herr='[TRNPRP error 542] Transport equations are not '//
     &       'available for mixtures with alcohols.'

      elseif (ierr.eq.543) then
        herr='[TRNPRP error 543] Transport equations are not '//
     &       'available for the ammonia/water mixture.'

      elseif (ierr.eq.-558) then
        call IDHCONV (0d0,ihr,1,i,j,hj)
        herr='['//ha//' warning -558] 2-D Newton-Raphson '//
     &       'method for conformal temperature and density did not '//
     &       'converge for the reference fluid.'

      elseif (ierr.eq.-560) then
        herr='[TCXM1C warning -560] Pure fluid is exactly at the '//
     &       'critical point; thermal conductivity is infinite.'

      elseif (ierr.eq.561) then
        herr='[TRNPRP error 561] Pure fluid correlation produced an '//
     &   'erroneous value for either viscosity or thermal conductivity.'





c  600's  --  Errors in the utility routines.

      elseif (ierr.eq.601) then
        herr='['//ha//' error 601] Invalid input: '//hb

      elseif (ierr.eq.610) then
        herr='[UNITCONV error 610] Requested unit is not '//
     &       'available or array indices are out of bounds.'

      elseif (ierr.eq.611) then
        herr='[UNITCONV error 611] Molar mass required '//
     &       'for conversion or input value is zero.'

c     #621 used in LUDECOMP

      elseif (ierr.eq.631) then
        herr='[CSPLNVAL error 631] No data have been saved.'

      elseif (ierr.eq.633) then
        herr='['//ha//' error 633] Iteration for spinodal did'//
     &       ' not converge for T = '//hx//' K.'

      elseif (ierr.eq.-638) then
        herr='['//ha//' warning -638] Spinodal not found, '//
     &       'point of zero curvature returned.'

      elseif (ierr.eq.662) then
        herr='['//ha//' error 662] Heating values are not '//
     &       'available for all species in the mixture.'

      elseif (ierr.eq.664) then
        herr='[SPECIES error 664] Unknown species in chemical formula.'

      elseif (ierr.eq.665) then
        herr='[SPECIES error 665] Error in chemical formula.'

      elseif (ierr.eq.691) then
        herr='[RPNEQN error 691] Error in setup of reverse Polish '//
     &  'notation equations:  '//ha

      elseif (ABS(ierr).ge.700 .and. ABS(ierr).le.720) then
        call IDHCONV (0d0,ierr,1,i,j,hj)
        herr='[CHECKSTATE warning '//hj(j:12)//'] State appears to be'//
     &       ' two-phase:  '//ha





c  800's  --  Errors in the REFPROP and ALLPROPS subroutine, along with
c             other higher level routines in the REFPROP.FOR file.

      elseif (ierr.eq.801) then
        herr='[REFPROP error 801] Invalid input.'

      elseif (ierr.eq.802) then
        herr='[REFPROP error 802] TH or TE inputs require a third '//
     &       'input to specify which solution should be returned '//
     &       '(use TH> to specify the root with the upper density '//
     &       'and TH< to specify the root with the lower density).'

      elseif (ierr.eq.-803) then
        herr='[REFPROP warning -803] The maximum density of the '//
     &        'equation of state does not occur at the maximum '//
     &        'pressure and temperature.  Only T, P, or D can be '//
     &        'returned one at a time.  Properties at Tmax and Pmax '//
     &        'are not the same as at Tmax and Dmax.'

      elseif (ierr.eq.804) then
        herr='[REFPROP error 804] Component number out of range.'

      elseif (ierr.eq.-804) then
        herr='[REFPROP warning -804] Component number out of range.'

      elseif (ierr.eq.805) then
        herr='['//ha//' error 805] The composition of the mixture '//
     &       'does not sum to one, further calculation of properties '//
     &       'will result in erroneous values or failure of the '//
     &       'routines and/or DLL.'

      elseif (ierr.eq.-806) then
        herr='[REFPROP warning -806] No output string was requested '//
     &       'even though an input string was specified.  The output '//
     &       'value has been set to a warning flag.'

      elseif (ierr.eq.-807) then
        herr='[REFPROP warning -807] An input string is required '//
     &       'for the requested output string.'

      elseif (ierr.eq.-808) then
        herr='[REFPROP warning -808] Fluids strings should not be '//
     &       'sent in the same command while setting flags.  If the '//
     &       'fluids were not already loaded, it was not done here. '//
     &       'Only the flag was set.'

      elseif (ierr.eq.809) then
        herr='['//ha//' error 809] The AGA8 and GERG08 models cannot '//
     &       'be simultaneously active - no change was made. '//
     &       '(SETUP must be recalled after resetting the GERG flag '//
     &       'to deactivate the model).'

      elseif (ABS(ierr).eq.810) then
        herr='[REFPROP '//ha//'] One or more of the input properties '//
     &       'is not thermophysically defined for two-phase states '//
     &       'and cannot be calculated.'

      elseif (ierr.eq.811) then
        herr='[GETENUM error 811] Input string not recognized: '//ha

      elseif (ierr.eq.812) then
        herr='[REFPROP error 812] The input code Q is invalid '//
     &       'because it is not always clear if the value is '//
     &       'given on a mass or mole basis.  Use QMOLE or QMASS '//
     &       'instead.'

      elseif (ierr.eq.813) then
        herr='[REFPROP error 813] The input composition must '//
     &       'match that of the predefined mixture when the mixture '//
     &       'name is included in the call to REFPROP.  To use a '//
     &       'different composition, leave the fluid name blank.'

      elseif (ierr.eq.-851) then
        herr='[ALLPROPS warning -851] Number of inputs exceeds '//
     &       'length of output array.'

      elseif (ierr.eq.-852) then
        herr='[ALLPROPS warning -852] Additional properties are '//
     &       'included in the output array, the input string and '//
     &       'output array do not line up.'

      elseif (ierr.eq.853) then
        herr='[ALLPROPS error 853] Unknown parameter:  '//ha

      elseif (ierr.eq.854) then
        herr='[ALLPROPS error 854] One or more properties cannot '//
     &       'be calculated for mixtures.'




c  900's  --  Additional setup errors not included in the 101 to 139 range.

      elseif (ierr.eq.901) then
        herr='[SETUP error 901] The file name '//ha//' is no '//
     &       'longer valid, please use '//hb//' instead.'

      elseif (ierr.eq.902) then
        herr='['//ha//' error 902] A pure fluid is currently in use. '//
     &       'This subroutine is valid only for mixtures.'

      elseif (ierr.eq.903) then
        herr='[SETKTV error 903] Binary parameters for i=j '//
     &       'cannot be changed, or component number is greater '//
     &       'than number of components in mixture.'

      elseif (ierr.eq.904) then
        herr='[SETKTV error 904] Fluid order is backwards than '//
     &       'that in the HMX.BNC file.  No changes have been made.'

      elseif (ierr.eq.-905) then
        herr='['//ha//' warning -905] Binary interaction parameters '//
     &   'are not presently available for this mixture; calculations '//
     &   'will be made assuming ideal solution behavior.'

      elseif (ierr.eq.906) then
        herr='[READFLUIDSTRING error 906] Error in input string.'

      elseif (ierr.eq.907) then
        herr='[SETUP error 907] Old HMX.BNC file found, '//
     &       'please obtain a new file for version 9.4.1 or later.'

      elseif (ierr.eq.908) then
        herr='[SETHMX error 908] Wrong components; '//
     &       'model TR1 valid for ammonia + water only.'

      elseif (ierr.eq.911) then
        herr='[SETFLD error 911] Hash number must be exactly '//
     &       'eight characters long.'

      elseif (ierr.eq.-912) then
        herr='[SETMOD warning -912] Models specified by a call to '//
     &       'SETMOD will not be active till the next call to SETUP.'

      endif

      end                                             subroutine ERRNUM2
c
c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                   End file UTILITY.FOR
c ======================================================================
