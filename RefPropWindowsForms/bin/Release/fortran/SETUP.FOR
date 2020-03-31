c  Begin file SETUP.FOR
c
c  This file contains the routines that initialize the models, fluid-
c    specific parameters and coefficients, etc.  The subroutine SETUP must
c    be called before any of the other routines (except SETMOD).
c    Call(s) to SETMOD and SETREF are optional and may be used to specify
c    non-standard models and reference states.
c
c  Subroutine SETUP0 calls subroutine SETUP with the same techniques as
c    those used by the graphical interface and the Excel spreadsheet.
c
c  Subroutine SETMIX reads the *.MIX files and calls SETUP.
c
c  Subroutine SETPATH sets the path where the *.fld files can be found.
c
c  Subroutine PUREFLD allows the user to calculate the properties of a pure
c    fluid when a mixture has been loaded and the fluid is one of the
c    constituents in the mixture.
c
c  Description of all common variables (contained in the common blocks)
c    is in the files COMMONS.INC and COMTRN.INC.
c
c  Contained here are:
c     subroutine SETUP   (ncomp,hFiles,hFmix,hrf,ierr,herr)
c     subroutine SETUP0  (ncomp,hfld,hFmix,hrf,ierr,herr)
c     subroutine SETFLD  (icomp,hFile,jFluidID,ierr,herr)
c     subroutine SETFLD2 (nread,i,hstar,hflag,htype,hcite,heqn,hreff,hetemp,httemp,ierr,herr)
c     subroutine SETMOD  (ncomp,htype,hmix,hcomp,ierr,herr)
c     subroutine SETREF  (hrf,ixflag,x0,h0,s0,T0,P0,ierr,herr)
c     subroutine SETMIX  (hMixNme,hFmix,hrf,ncc,hFiles,z,ierr,herr)
c     subroutine SETNC   (ncomp)
c     subroutine GETMOD  (icomp,htype,hcode,hcite)
c     subroutine PUREFLD (icomp)
c     subroutine GERG04  (ncomp,iFlag,ierr,herr)
c     subroutine GERG08  (ncomp,iFlag,ierr,herr)
c     subroutine RFFILE  (hfilei,hflref)
c     subroutine OPENFL  (icomp,nread,hflIn,hflOut,idir,ierr,herr)
c     subroutine OPENFL2 (nread,iFLDarray,iulc,ioerr,hfilei,hFLDarray)
c     subroutine RESETA  (iFlag)
c     subroutine SETREFDIR (hpth)
c     subroutine GETREFDIR (hpth)
c     block data BDSET
c
c ======================================================================
c ======================================================================
c
      subroutine SETUP (ncomp,hFiles,hFmix,hrf,ierr,herr)
c
c  Define models and initialize arrays.
c
c  Inputs:
c    ncomp--Number of components (1 for pure fluid) (integer)
c           If called with ncomp=-1, the version number*10000 will be returned in ierr.
c   hFiles--Array of file names specifying the fluid or the mixture components (character*255);
c           e.g., 'fluids\r134a.fld' (DOS)
c                 ':fluids:r134a.fld' (Mac)
c                 '[full_path]/fluids/r134a.fld' (UNIX)
c    hFmix--Name of file containing mixture coefficients (character*255);
c           e.g., 'fluids\HMX.BNC'
c      hrf--Reference state for thermodynamic calculations (character*3). Other choices are possible, see SETREF:
c           'DEF' - default reference state as specified in fluid file
c           'NBP' - h,s = 0 at pure component normal boiling point
c           'ASH' - h,s = 0 for sat. liquid at -40 C (ASHRAE convention)
c           'IIR' - h = 200 kJ/kg and s = 1 kJ/kg-K for sat. liquid at 0 C (IIR convention)
c
c  Outputs:
c     ierr--Error flag:
c             0 - Successful
c            101 - Error in opening file
c            102 - Error in file or premature end of file
c           -107 - Unknown model encountered in file
c            105 - Specified model not found
c           -105 - Must use routine SETREF for (OTH) reference state choice
c            111 - Error in opening mixture file
c            112 - Mixture file of wrong type
c            114 - nc not equal to the nc sent to SETMOD
c           -117 - Binary pair not found, all parameters will be estimated
c            117 - Mixture parameters not available, mixture is outside the
c                  range of the model and calculations will not be made
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  11-19-94  MM, original version
c  09-28-05  DT, change occurrences of a backslash to 'CHAR(92)' for compatibility with some compilers
c  03-03-10 EWL, return version number when called with nc=-1
c  01-24-13 MLH, add flag for alcohols ixAlc
c  04-16-14 EWL, add call to RESETA at beginning
c  01-04-16 MLH, change default mixture model for surface tension to PAR
c  07-01-16 EWL, shorten and rework code; remove all unnecessary comments about changes

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      include 'COMFEQ.INC'
      include 'COMANC.INC'
      include 'COMTRN.INC'
      character*3 hrf,hrff,hcomp(1:ncmax)
      character*255 hFiles(ncmax),hFile(nq),hFmix
      character*255 herr,herr1,herr2,herr3,UCASE
      dimension x0(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETUP

c  Send back version number.

      if (ncomp.eq.-1) then
        ierr=100000               !Send back version number*10000 in ierr.
        herr='10.0'               !Send back version number in herr.
c       ierr=94444                !Send back version number*10000 in ierr.
c       herr='9.4.4.44 (004)'     !Send back version number in herr.
        RETURN
      endif

c  Send back the number of components.
      if (ncomp.eq.-2) then
        ierr=nc
        RETURN
      endif

c---To permanently turn on the full GERG equations, uncomment the following line.---
c     call GERG08 (ncomp,1,ierr,herr)

      ierr=0
      ierr2=0
      ierr3=0
      iersav2=0
      herr=' '
      x0=0d0
      x0(1)=1d0
      T0=0d0
      P0=0d0
      h0=0d0
      s0=0d0
      iSetup=0
      hrff=hrf

c  If lRst is true, then both SETMOD and SETUP were called simultaneously,
c   and this call to SETUP must reset the earlier call to SETMOD.
      if (lRst) goto 100

c  Compare inputs to previous values. If nothing has changed, SETUP can be bypassed.
      if (nc.ne.ncomp .or. ncomp.eq.0) goto 100
      if (hsveos.ne.heosmx .and. hsveos.ne.'NBS') goto 100  !EOS model
      if (hsvvis.ne.hvismx .and. hsvvis.ne.'NBS') goto 100  !Viscosity model
      if (hsvtcx.ne.htcxmx .and. hsvtcx.ne.'NBS') goto 100  !Thermal conductivity model
      if (hsvstn.ne.hstnmx .and. hsvstn.ne.'NBS') goto 100  !Surface tension model
c  Check that *.fld file and component models are the same as last call.
      do i=1,nc
        if (hsvfld (i).ne.hFiles(i)) goto 100
        if (hsveosk(i).ne.heosk (i) .and. hsveosk(i).ne.'NBS') goto 100 !EOS model
        if (hsvvisk(i).ne.hetak (i) .and. hsvvisk(i).ne.'NBS') goto 100 !Viscosity model
        if (hsvtcxk(i).ne.htcxk (i) .and. hsvtcxk(i).ne.'NBS') goto 100 !Thermal conductivity model
        if (hsvstnk(i).ne.hstnk (i) .and. hsvstnk(i).ne.'NBS') goto 100 !Surface tension model
      enddo

      if (hsvmix.ne.hFmix) goto 100     !Mixture coefficient file

      if (iGERG08.eq.-1 .or. iGERG08.eq.1) then  !Reset GERG model
        lReset=.true.
        goto 100
      endif

c  Check for previous call to SETREF with a different reference state.
      if (iPX0Flg.ne.0) RETURN    !PH0 equations are enabled, no need to check reference states.
      if (hrff.eq.' '   .and. hsvref.ne.' ')  RETURN  !If blank, exit without changing anything
      if (hrff.eq.'DEF' .and. hsvref.eq.hrff) RETURN  !Check if reference state is the default, both here and in the previous call.
      if (hrff.eq.' ') hrff='DEF'
      if (hrff.ne.hsvref) then
        call SETREF (hrff,(ixfsav),x0,h0,s0,T0,P0,ierr,herr)  !Don't remove the parenthesis around ixfsav.  It needs to send a value, not the memory location.
      elseif (ixfsav.eq.2) then
        call SETREF (hrff,1,x0,h0,s0,T0,P0,ierr,herr)
      endif
      RETURN  !Nothing has changed, just exit.


c-------Start of main code--------


 100  call RESETA (0)
      lRst=.false.
      if (.not.lReset) lRst=.true.          !Flag to indicate that SETMOD was called outside of SETUP.

c  Check state of flag lReset; a value of .true. indicates need to reset all models
c   to 'NBS'; this occurs when SETUP is called a second time in a given application.
      if (lReset) then
        hcomp(1:ncmax)=heosk(1:ncmax)
        call SETMOD (ncomp,'NBS',heosmx,hcomp,ierr,herr)
      endif
      lReset=.true.
      if (ierr.gt.0 .and. ierr.ne.-912) goto 1000
      if (ncomp.ne.ncSet .and. .not.lGERG08) then
        call ERRNUM (114,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        goto 1000
      endif
      lGERG08=.false.
      if (iGERG08.le.0) then                !If iGERG08 is less than zero, then the GERG was active and waiting to be turned off by a call to SETUP.
        iGERG08=0                           ! The negative value is just a flag for us humans, and has no impact in the code.
      else
        hcomp(1:ncomp)='FEK'
        call SETMOD (ncomp,'EOS','HMX',hcomp,ierr,herr)
        iGERG08=2                           !Make iGERG08 equal to 2 to show that the components have actually been loaded.
        lGERG08=.true.                      !Setting the lGERG08 flag shows that the GERG model is currently in use.
      endif

c  Set number of components.
      nc=ncomp

c  Set flags.  Do not add these to RESETA, they should only be reset when SETUP is called.
      iSetRf=0
      iAmWat=0
      ixWat=0
      ixCO2=0
      ixAlc=0
      ixAir=0
      iAnc=0
      iV10=0  !Currently not used, available for use.
      Tctrue=0d0
      Dctrue=0d0
      hsavprk =' '
      hsavpr=' '
      htcxhc=' '
      hetahc=' '
      htcxcr=' '
      htcxcrecs=' '
      hDOI=' '
      hWEB=' '
      iSpline=0
      xecsljs=0d0
      xecslje=0d0
      lRcnst=.true.
      lRcnstWag=.true.
      lWagner=.false.
      lAGA8sav=.false.
      lAGA8Flg=.false.

      nFEQterms=0  !Total number of terms
      nFEQpoly=0   !Number of polynomial only terms
      nFEQpe  =0   !Number of polynomial and exponential terms
      nFEQkg  =0   !Number of new terms from Gao
      nFEQgaus=0   !Number of Gaussian terms
      nFEQnan =0   !Number of non-analytical terms
      nFEQcrt =0   !Number of critical region terms (Gaussian and non-analytical)

      nPhiLog =0   !Reset Cp0 and PH0 number of terms
      nPhiTau =0
      nPhiExp =0
      nPhiCosh=0
      nPhiSinh=0
      nCp0poly=0
      nCp0Exp =0
      nCp0Cosh=0
      nCp0Sinh=0
c     iFluidID=0
      wmas=0
      Tcrit=300d0
      Ttpn=0d0
      Ptpn=0d0
      Dtpn=0d0
      Dtpv=0d0
      h7oper=' '
      i7oper=0
      n7oper=0
      n7cf=0
      n7st=0
      n7total=0
      l7eta=.false.
      hdir=' '
      hFldOld=' '
      nArr=0
      zSpln=0d0
      iAltFlg=0
      iMassOld=-1
      iMxRule=0
      xeta0 =0d0
      xetaB2=0d0
      xetar =0d0
      xetac =0d0
      xtcx0 =0d0
      xtcxr =0d0
      xtcxc =0d0
      PDmixTc=300d0
      PDmixPc=0d0
      PDmixDc=0d0
      zPDmix=0d0
      ncPDmix=0
      Reos=xRgas
      ReosDef=xRgas
      call FLAGS ('Calorie',0,j,ierr,herr)
      if (iUUnits(1,0).eq.0) call REFPRPUNITS ()

c  Set pointers to models.

c  Equation of state
      if (heosmx.eq.'NBS') then
        call SETEOSMX ('HMX')          !NIST recommended model
        if (nc.eq.1) call SETEOSMX (heosk(1))
      elseif (UCASE(heosmx).eq.'HMX') then
c        Mixture model is HMX, use component models as set in SETMOD.
      elseif (nc.gt.1) then
        ierr=999             !This condition should not be accessed; HMX is the only choice at present.
        call SETEOSMX ('HMX')
      endif

c  Viscosity
      if (hvismx.eq.'NBS') then
        hvismx='ECS'         !NIST recommended model
        if (nc.eq.1) hvismx=hetak(1)
      elseif (UCASE(hvismx).eq.'ECS') then
c        Mixture model is ECS, use component models as set in SETMOD.
      elseif (nc.gt.1) then
        hvismx='ECS'
      endif

c  Thermal conductivity
      if (htcxmx.eq.'NBS') then
        htcxmx='ECS'         !NIST recommended model
        if (nc.eq.1) htcxmx=htcxk(1)
      elseif (UCASE(htcxmx).eq.'ECS') then
c        Mixture model is ECS, use component models as set in SETMOD.
      elseif (nc.gt.1) then
        htcxmx='ECS'
      endif

c  Surface tension
      if (hstnmx.eq.'NBS') then
        hstnmx='PAR'                   !Jan. 4, 2016: Existing surface tension mixture models give bad results for
        if (nc.eq.1) hstnmx=hstnk(1)   ! anything other than refrigerants; new default mixture model PAR added.
      endif

c  Dielectric constant
      if (hdilmx.eq.'NBS') then
        hdilmx='DEM'
        if (nc.eq.1) hdilmx=hdilk(1)
      endif

c  Fill info arrays for unused, undefined, or reference fluids.
      hash  =' '
      hcas  =' '
      hAltID=' '
      hname =' '
      hieos ='NUL'
      hicp0 ='NUL'
      hieta ='NUL'
      hietac='NUL'
      hitcx ='NUL'
      hitcxc='NUL'
      histn ='NUL'
      hidiel='NUL'
      himelt='NUL'
      hisubl='NUL'
      hips  ='NUL'
      hipl  ='NUL'
      hidl  ='NUL'
      hidv  ='NUL'
      Tmtcx =0d0
      Txtcx =0d0
      Tmeta =0d0
      Txeta =0d0
      Tmecst=0d0
      Txecst=0d0
      Pxtcx =0d0
      Dxtcx =0d0
      Pxeta =0d0
      Dxeta =0d0
      Pxecst=0d0
      Dxecst=0d0
      hFile='NUL'
      hFile (1:nc)=hFiles(1:nc)
      hsvfld(1:nc)=hFiles(1:nc)
      hetak(ncmax+1:nx)='NUL'
      htcxk(ncmax+1:nx)='NUL'
      heosk(ncmax+1:nx)='FEQ'               !Set default in case *.fld file has no transport model.
      iEOSk(ncmax+1:nx)=iEOSFEQ
      ihash=0
      iAlthash=0

c  Read fluid files (*.fld) for main fluids.
      do i=1,nc
        call SETFLD (i,hFile,0,ierr,herr)
        if (INDEX(UCASE(hFile(i)),'.PPF').ne.0) iAnc(i)=1
        if (ierr.gt.0) goto 1000             !Error in opening file
        call SWAPERR (ierr3,ierr,herr3,herr)
      enddo

c  Read ECS fluid files.
      do i=1,nc
        if (i.le.mxecs .and. .not.lSkipECS) then
          j=i+ncmax
          if (hFile(j).ne.'NUL' .and. hFile(j).ne.' ' .and.
     &        hFile(j)(1:1).ne.CHAR(0)) then
            call SETFLD (j,hFile,0,ierr,herr)
            if (ierr.gt.0) goto 1000         !Error in opening file
            call SWAPERR (ierr2,ierr,herr2,herr)
          endif
        endif
      enddo

c  Read reference fluid.
      nref=nr      !Keeps the compiler from reporting an error when ncmax=1
      if (mxref.gt.0 .and. nref.le.nq .and. .not.lSkipECS) then
        if (hFile(nref).ne.'NUL') then
          if (nc.gt.1) hFile(nref)='NITROGEN.FLD'
          call RFFILE (hFile(1),hFile(nref))
          call SETFLD (nref,hFile,0,ierr,herr)
          if (ierr.gt.0) goto 1000           !Error in opening file
          call SWAPERR (ierr2,ierr,herr2,herr)
        endif
      endif

c  Set up mixture model if applicable.
      ierr=0
      call RFFILE (hFile(1),hFmix)
      if (iEOSmx.eq.iEOSHMX) call SETHMX (hFmix,ierr,herr)
      if (ierr.gt.0 .and. ierr.ne.117) goto 1000  !Error in opening file
      call SWAPERR (ierr2,ierr,herr2,herr)

c  Set reference state.
      ixflag=1               !Only 'pure fluid' reference state can be called here.
      if (hrff(1:2).eq.'OT') then
        hrff='DEF'           !Use the default reference state.
        if (ierr.eq.0) call ERRNUM(-105,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      endif
      ierr1=0
      call SETREF (hrff,ixflag,x0,h0,s0,T0,P0,ierr1,herr1)
      call SWAPERR (ierr,ierr1,herr,herr1)  !Any positive error from SETREF takes precedence over negative errors.

c  If nothing has changed on subsequent calls to SETUP, routine will exit code.
      if (ierr.le.0) then
        hsvmix=hFmix
        hsveos=heosmx
        hsvvis=hvismx
        hsvtcx=htcxmx
        hsvstn=hstnmx
        hsveosk(1:nc)=heosk(1:nc)
        hsvvisk(1:nc)=hetak(1:nc)
        hsvtcxk(1:nc)=htcxk(1:nc)
        hsvstnk(1:nc)=hstnk(1:nc)
      else
        hsveos=' '           !Reset one of the saved variables so the SETUP will not be bypassed on the next call.
      endif

      do i=1,nc
        call SETPRCO (i)     !Set Peng-Robinson parameters.
      enddo

      if (ierr.le.0 .and. ierr2.lt.0) then  !If -101 error from calling SETFLD has been set, overwrite any other warnings from other routines.
        ierr=ierr2
        herr=herr2
      endif
      call SWAPERR (ierr,ierr3,herr,herr3)  !If 109 error from calling SETFLD has been set, set ierr if it is still zero.
      call ERRNUM (0,0,' ',' ',0d0,0d0,999d0,ierr,herr)

      if (iPX0Flg.gt.0) call FLAGS ('CP0PH0',2,i,ierr2,herr2)  !Change use of Cp0 equations to PX0.

      if (iaga8.gt.0) then
        iaga8=1                  !Turn down the AGA flag so that the setup routine will work properly (which also turns it back on).
        call SETAGA (ierr,herr)  !Setup the AGA8 equation of state if the FLAGS routine or SETAGA routine has been called.
      endif
      iReset=0
      iSetup=-1
      if (ierr.gt.0) iSetup=ierr

      RETURN

c  Error occurred, set nc to zero so that SETUP will not be bypassed on next call.
 1000 nc=0
      end                                               subroutine SETUP
c
c ======================================================================
c
      subroutine SETUP0 (ncomp,hfld,hFmix,hrf,ierr,herr)
c
c  Call the SETUP routine with the same inputs as SETUP except for
c  the hfld variable.  This subroutine is generally used in calls
c  to the Refprop DLL since the call cannot handle a string array.
c  The hfld variable is a string of length 10000.  For a pure fluid,
c  it simply contains the name of the fluid file (with a path if needed).
c  For a mixture, it contains the names of the constituents in the
c  mixture separated by a |, #, or ;.  For the air mixture, this would be
c  something like (depends on the need for paths)
c  hfld='fluids\nitrogen.fld|fluids\argon.fld|fluids\oxygen.fld|'
c
c  Inputs:
c    ncomp--Number of components (integer)
c     hfld--String containing fluid file names (character*10000)
c    hFmix--Name of file containing mixture coefficients (character*255);
c           for example, 'fluids\HMX.BNC'
c      hrf--Reference state for thermodynamic calculations (character*3);
c           see SETUP for details
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c           (Values are identical to SETUP; a 109 is returned if the number
c           of fluids in hfld is not equal to icomp.)
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  04-07-05 EWL, original version

      include 'COMMONS.INC'
      character hfld*(*),hrf*3
      character*255 hf(ncmax),herr,hFmix
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETUP0

      if (ncomp.eq.-1) then
        call SETUP (ncomp,hf,hFmix,hrf,ierr,herr)
        RETURN
      endif

      nc2=ncomp
      if (nc2.eq.-3) then  !Use to force a reread of the fluid files.
        nc2=nc
        nc=0
      endif

      call STRINGSPLIT ((ncmax),hfld,';*#|',n,hf,ierr,herr)
      if (nc2.eq.0) nc2=n
      if (nc2.ne.n) goto 109   !Check if number of fluids sent matches ncomp.
      call SETUP (nc2,hf,hFmix,hrf,ierr,herr)
      RETURN

 109  i=n
      if (n.gt.nc2) i=-1
      j=nc2+1
      if (j.lt.1) j=1
      if (j.gt.ncmax) j=ncmax
      call ERRNUM (109,i,'SETUP0',hf(j),0d0,0d0,0d0,ierr,herr)

      end                                              subroutine SETUP0
c
c ======================================================================
c
      subroutine SETFLD (icomp,hFile,jFluidID,ierr,herr)
c
c  Open a fluid file and read model coefficients (or get from block data).
c
c  Inputs:
c    icomp--Pointer specifying component number;
c           zero and negative values are used for ECS reference fluid(s)
c    hFile--Array of file names specifying fluid/mixture components (character*255)
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      101 - Error in opening file
c                     -101 - Error in opening file for ECS transport properties
c                      102 - Error in file or premature end of file
c                     -107 - Unknown model encountered in file
c                      105 - Specified model not found
c                      107 - Cp0 equation not found
c     herr--Error string (character*255)
c
c  Variables used in this routine
c  hstar - '#' or '@' indicates start of model specification
c        # indicates NIST-recommended model
c        @ indicates other model(s)
c  hflag indicates type of model, for example,
c        'EOS' - equation of state
c        'TRN' - transport property model (i.e. ECS model)
c        'ETA' - pure fluid viscosity model
c        'TCX' - pure fluid thermal conductivity model
c        'AUX' - auxiliary model, such as ideal-gas heat capacity
c  htype is 3-letter key for a particular model + 1-line citation, for example,
c        'NBS' - NIST-recommended model
c        'BWR' - modified Benedict-Webb-Rubin equation of state
c        'FEQ' - fundamental (Helmholtz) equation of state
c        'ECS' - extended corresponding states model
c        'HMX' - mixture Helmholtz model
c        'CPP' - polynomial fit of ideal-gas heat capacity
c        'VSi' - pure fluid viscosity model #i
c        'TCi' - pure fluid thermal conductivity model #i
c
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  12-08-95  MM, original version - extracted from old subroutine SETUP
c  01-17-01 EWL, remove double backslash, which caused problems in Windows
c  07-22-05 MLH, allow for transport reference fluids
c  09-28-05  DT, change occurrences of a backslash to 'CHAR(92)' for compatibility with some compilers
c  04-18-06 EWL, make call to inquire to check for multiple use of nread=12
c  07-24-06 EWL, do not call setfld again if @FEQ found after #FE1, etc.
c  08-10-06 EWL, create new subroutine to open file and move appropriate code
c  10-30-06 EWL, check for FEK
c  11-08-06 EWL, after the call to SETFLD2, add href to hFiles() in the negative position so that the transport routines of the reference fluid will be loaded.
c  01-09-08 EWL, read in heat of combustion
c  02-26-09 EWL, check for missing '.fld' on file names
c  04-04-15  GV, new fields added to read fusion data into common blocks

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      include 'COMFEQ.INC'
      include 'COMANC.INC'
      include 'COMTRN.INC'
      character hstar*1,hcite*251,UCASE*255
      character*3 heqn,hetemp,httemp,hflag,htype
      character*255 hFile(nq),hfilei,hreff,hstr,herr,herr1,herr2,h2

      n7total=0
 10   if (jFluidID.eq.0) ierr=0   !jFluidID is not yet used, this line keeps the compiler happy.
      ierr1=0
      ierr2=0
      herr=' '
      herr1=' '
      hstar=' '
      hflag=' '
      hcite=' '
      htype=' '
      hetemp=' '
      httemp=' '
      hdir(icomp)='File not found'
      wmas(icomp)=0d0   !Used as a flag in the transport routines.
      i=icomp
      icpflg=0
      iphi0flg=0
      leosfl=.false.
      lprfl=.false.
      leta=.false.
      ltcx=.false.
      if (i.le.ncmax) then
        hInChi(i)   = 'Not available'
        hInChiKey(i)= 'Not available'
        hUNNumb(i)  = 'Not available'
        hFamily(i)  = 'Not available'
        hAltID(i)   = 'Not available'
        hSafety(i)  = 'Not available'
        xCmbst(i)   = -1d0
        gwpot(i)    = -1d0
        odpot(i)    = -1d0
        dcpfus(i)   = -1d0
        dhfus(i)    = -1d0
        dvfus(i)    = -1d0
        pfusref(i)  = -1d0
        tfusref(i)  = -1d0
      endif

c  Set initial PR coefficients for transport/ECS reference fluids for icomp>=0.
      if (i.gt.0 .and. i.le.ncmax) then
        PRcoef(1:20,i)=0d0
        iPRflag(i)=0
      endif

c  Get coefficients from common blocks.
      if (hFile(i)(1:5).eq.'BDATA' .or. hFile(i)(1:5).eq.'bdata') then
        lbdata=.true.
        nread=0

c  Read coefficients from file.
      else
        ierr2=ierr
        herr2=herr
        lbdata=.false.
        nread=12                       !Logical unit for reading files.
        call CHKNAMES (i,hFile(i),ierr,herr)
        call OPENFL (i,nread,hFile(i),hfilei,1,ierr,herr)  !Open fluid file for reading.
        if (ierr.eq.0) hdir(i)=hfilei
        if (i.le.nq) hinpnme(i)=hFile(i)
        ierropn=ierr
        if (ierr.ne.0 .and. i.gt.ncmax) goto 1101
        if (ierr.ne.0) goto 101

        read (nread,'(a60)',err=101,end=101) hname(i) !Short name
          j=INDEX(hname(i),'!')
          if (j.ne.0) hname(i)=hname(i)(:j-1)
        read (nread,'(a12)',err=101) hcas(i)   !CAS ID number
        read (nread,2080,err=101) hnam80(i)    !Full chemical name
          j=INDEX(hnam80(i),'!')
          if (j.gt.0) hnam80(i)=hnam80(i)(:j-1)
        read (nread,2080,err=101) hstr         !Chemical formula
          j=INDEX(hstr,'!')
          if (j.eq.0) j=255
          hChemF1(i)=hstr(:j-1)
          hChemF2(i)=hChemF1(i)
          j=INDEX(hstr,'{')
          if (j.ne.0) then
            hstr=hstr(j+1:)
            j=INDEX(hstr,'}')
            if (j.ne.0) hstr=hstr(:j-1)
            hChemF2(i)=hstr
          endif
        read (nread,2080,err=101) hsyn(i)      !Synonym
          j=INDEX(hsyn(i),'!')
          if (j.ne.0) hsyn(i)=hsyn(i)(:j-1)

c  Read in generic fluid constants; these may be reset by some SETeos routines.
        read (nread,*,err=102) wmas(i)         !Molar mass [g/mol]
        read (nread,*,err=102) Ttpn(i)         !Triple point temperature [K]
        read (nread,*,err=102) Tnbp(i)         !Normal boiling point [K]
        read (nread,*,err=102) Tcrit(i)        !Critical temperature [K]
        read (nread,*,err=102) Pcrit(i)        !Critical pressure [kPa]
        read (nread,*,err=102) Dcrit(i)        !Critical density [mol/L]
        read (nread,*,err=102) accen(i)        !Acentric factor [-]
        read (nread,*,err=102) dipole(i)       !Dipole moment [debye]
        read (nread,'(a3)',err=102) hrefdef(i) !Default reference state

c  For 'OTH' (other) reference states, read in reference T,P,h,s.
        if (hrefdef(i)(1:2).eq.'OT')
     &    read (nread,*,err=102)  Tdef(i),Pdef(i),hdef(i),sdef(i)
        read (nread,*,err=102) VerFL(i)        !Version number
        do k=1,100
 150      read (nread,2080,err=101) hstr
          if (hstr.eq.' ') EXIT
          j=INDEX(hstr,'!')
          m=j-1
          j1=INDEX(hstr,' :')
          if (m.lt.0) m=j1-1
          if (j1.ne.0) then
            j2=INDEX(hstr(j1:),': ')
            if (j2.ne.0) then
              h2=UCASE(hstr(j1+2:j1+j2-2))
              hstr=hstr(1:m)
              j2=INDEX(hstr,' :')
              if (j2.ne.0) hstr=hstr(:j2)
              if (hstr(1:4).eq.'????') hstr='Not available'
              if (h2.eq.'FAMILY')   hfamily(i)  = hstr               !Family
              if (h2.eq.'UN')       hUNNumb(i)  = hstr               !UN Number
              if (h2.eq.'INCHI')    hInChi(i)   = hstr               !InChI string
              if (h2.eq.'INCHIKEY') hInChiKey(i)= hstr               !InChI key
              if (h2.eq.'SAFETY')   hSafety(i)  = hstr               !ASHRAE 34 safety classification
              if (h2.eq.'HEAT')     read (hstr,*,err=150) xCmbst(i)  !Heat of combustion
              if (h2.eq.'GWP')      read (hstr,*,err=150) gwpot(i)   !Global warming potential
              if (h2.eq.'ODP')      read (hstr,*,err=150) odpot(i)   !Ozone depletion potential
              if (h2.eq.'ALTID')    then
                j=INDEX(hstr,'(')
                if (j.gt.0) hstr=hstr(:j-1)
                hAltID(i)  =hstr                 !Alternate fluid ID number for mixing values
                iAlthash(i)=iHEXtoDec(hAltID(i)) !Convert from hex to integer.
              endif
              if (h2.eq.'HASH')     then         !Hash number corresponding to a reduction of the fluid's InChI key
                hash(i)=hstr
                if (hstr(9:).ne.' ' .or. hash(i)(8:8).eq.' ') then
                  call ERRNUM (911,0,' ',' ',0d0,0d0,0d0,ierr,herr)
                endif
              endif
            endif
          elseif (j.ne.0) then
            if (hstr(j:j+6).eq.'!family') hfamily(i)=hstr(1:m)            !Family
            if (hstr(j:j+2).eq.'!UN')     hUNNumb(i)=hstr(1:m)            !UN Number
            if (hstr(j:j+4).eq.'!heat')   read (hstr(1:m),*) xCmbst(i)    !Heat of combustion
            if (i.le.ncmax) then
              if (hstr(j:j+6).eq.'!dcpfus') read (hstr(1:m),*)dcpfus(i)   !The following added by GV to read data required for SLE and SVE calculations
              if (hstr(j:j+4).eq.'!hfus')   read (hstr(1:m),*)dhfus(i)
              if (hstr(j:j+4).eq.'!vfus')   read (hstr(1:m),*)dvfus(i)
              if (hstr(j:j+7).eq.'!pfusref')read (hstr(1:m),*)pfusref(i)
              if (hstr(j:j+7).eq.'!tfusref')read (hstr(1:m),*)tfusref(i)
            endif
          endif
        enddo
      endif
      if (xCmbst(i).ne.-1d0) xCmbst(i)=xCmbst(i)*1000d0   !Convert from kJ/mol in the fluid files to J/mol
      if (hash(i).eq.' ') hash(i)=hcas(i)
      call FLUIDID (hcas(icomp),hash(icomp),iFluidID(icomp))
      ihash(i)=iHEXtoDec(hash(i))  !Convert from hex to integer.

c  Search for key characters in columns 1 (for # or @) and 2-4, and branch
c   to subsidiary setup routines for specified models.
 100  if (.not.lbdata) then                 !Read from file.
        do k=1,10000
          read (nread,'(a1,a3)',end=102,err=102) hstar,hflag
          if (hstar.eq.'#' .or. hstar.eq.'@') EXIT
        enddo
        read (nread,'(a3,a251)',end=102,err=102) htype,hcite
        hflag=UCASE(hflag)
        htype=UCASE(htype)
        call GETID (icomp,0,nread,hstar,htype,ierr)
        if (ierr.eq.102) goto 102
      else
        hflag='EOS'
        htype=heosmx
      endif

c  ---Set up equation of state---
      if (hflag.eq.'EOS') then

c  NIST-recommended model.
       if (hstar.eq.'#') then
         if (nc.eq.1  .and. heosmx  .eq.'NBS') call SETEOSMX (htype)
         if ( i.ge.1  .and. heosk(i).eq.'NBS') heosk(i)=htype
         if ( i.gt.ns .and. heosk(i).eq.'NBS') heosk(i)=htype   !Transport reference fluids
       endif

c  Fundamental (Helmholtz) equation of state.
       if (htype(1:2).eq.'FE') then
         if (heosk(i).eq.htype) then
           if (.not.leosfl) then
             call SETEOSPURE (i,heosk(i))
             call SETEOSMX (heosmx)
             call SETFEQ (nread,i,ierr1,herr1)
             call SWAPERR (ierr,ierr1,herr,herr1)
             if (ierr.gt.0) goto 198
             if (i.le.ncmax) hieos(i)=htype//hcite
             leosfl=.true.
           endif
         endif
c  Modified Benedict-Webb-Rubin equation of state.
       elseif (htype(1:2).eq.'BW') then
          if (heosk(i).eq.htype) then
            if (.not.leosfl) then
              heosk(i)='FEQ'
              call SETEOSPURE (i,heosk(i))
              if (heosmx(1:2).eq.'BW') call SETEOSMX ('FEQ')
              call SETEOSPURE (i,heosk(i))
              call SETBWR (nread,i,ierr1,herr1)
              if (i.le.ncmax) hieos(i)=htype//hcite
              leosfl=.true.
            endif
          endif
c  Extended corresponding states (ECS) thermo model.
        elseif (htype(1:2).eq.'EC') then
          if (heosk(i).eq.htype) then
            if (.not.leosfl) then
              heosk(i)='ECS'
              call SETEOSPURE (i,heosk(i))
              if (heosmx(1:2).eq.'EC') call SETEOSMX ('ECS')
              call SETECS (nread,i,hreff,heqn,ierr1,herr1)
              call RFFILE (hFile(i),hreff)  !Add directory information to reference fluid file.
              if (i.le.mxecs) then
                if (i.le.ncmax) then
                  hFile(ncmax+i)=hreff
                  heosk(ncmax+i)=heqn
                  hieos(i)=htype//hcite
                  leosfl=.true.
                endif
              endif
            endif
          endif
c  Cubic (Peng-Robinson, etc.) equation of state.
        elseif (htype(1:2).eq.'PR') then
          if (i.le.ncmax) then
            if (heosk(i)(1:2).eq.htype(1:2)) then
              heosk(i)='PR'
              call SETEOSPURE (i,heosk(i))
              if (heosmx(1:2).eq.'PR') call SETEOSMX ('PR ')
              call SETPR (0,nread,i,ierr1,herr1)
              hieos(i)=htype//hcite
              leosfl=.true.
            else
              call SETPR (1,nread,i,ierr1,herr1) !Get tpr coefficients from fluid file.
            endif
            lprfl=.true.
          endif
        else
c  Unidentified equation of state model found in file.
          call ERRNUM (-107,i,htype,' ',0d0,0d0,0d0,ierr,herr)
        endif
        if (ierr1.ne.0) goto 102
      endif

      if (lbdata) then
        hflag='AUX'
        htype=hcpk(i)
      endif

c  Set up auxiliary model(s).
c   (Model(s) are specified in call to appropriate SETmod routine.)
      if (hflag.eq.'AUX' .and. i.gt.0 .and. i.le.ncmax) then
        ierr1=0
        if (htype(1:2).eq.'CP' .and. hcpk(i).eq.htype) then
c  Ideal-gas heat capacity function.
          call SETCPP (nread,i,1,ierr1,herr1)
          iCP0xx(i)=iCP0CP
          hicp0(i)=htype//hcite
          icpflg=2
        elseif (htype.eq.'CPP' .and. icpflg.eq.0) then
          call SETCPP (nread,i,1,ierr1,herr1)
          icpflg=1
          if (iphi0flg.ne.2) then
            iCP0xx(i)=iCP0CP
            hicp0(i)=htype//hcite
          endif
        elseif (htype(1:2).eq.'PH' .and. hcpk(i).eq.htype .and.
     &          iphi0flg.ne.2) then
c  Helmholtz energy for ideal-gas state.
          call SETCPP (nread,i,2,ierr1,herr1)
          iCP0xx(i)=iCP0PH
          hicp0(i)=htype//hcite
          iphi0flg=2
        elseif (htype(1:2).eq.'PX' .and. hcpk(i).eq.htype) then
c  Helmholtz energy for ideal-gas state.
          call SETCPP (nread,i,3,ierr1,herr1)
          iCP0xx(i)=iCP0PH
          hicp0(i)=htype//hcite
          iphi0flg=2
          htype(1:2)='PH'
          hcpk(i)(1:2)='PH'
        elseif(htype.eq.'PX0'.and.iphi0flg.eq.0)then
          call SETCPP (nread,i,3,ierr1,herr1)
          iphi0flg=1
        elseif(htype.eq.'PH0'.and.iphi0flg.eq.0)then
          call SETCPP (nread,i,2,ierr1,herr1)
          iphi0flg=1
        endif
        if (ierr1.gt.0) goto 102
      endif

      if (lbdata) hflag='TRN'

c  If the transport models, dielectric models, and surface tension eqs.
c   (vis, tcx, st, de) are not required, and only the default values in the
c   fluid files are used, the following files do not have to be compiled and
c   linked with the other source code - CORE_DE, CORE_STN, SETUP2,
c   TRNS_ECS, TRNS_TCX, TRNS_VIS, and TRNSP.  In addition, the
c   following two lines should be commented out.
      call SETFLD2 (nread,i,hstar,hflag,htype,hcite,
     &              heqn,hreff,hetemp,httemp,ierr1,herr1)
      call SWAPERR (ierr,ierr1,herr,herr1)
      nref=nr      !Keeps the compiler from reporting an error when ncmax=1
      if (hreff.ne.' ' .and. i.le.ncmax) then    !Store info for ECS reference fluid (but not if current fluid is the reference fluid).
        call RFFILE (hFile(i),hreff)             !Add directory information to reference fluid file.
        hFile(nref)=hreff                        !*.fld file for reference fluid
        heosk(nref)=heqn                         !Model for reference fluid equation of state
        if (i.le.mxecs) then
          if (hFile(ncmax+i).eq.'NUL') then
            hFile(ncmax+i)=hreff
            heosk(ncmax+i)=heqn
          endif
        endif
      endif

c  Do not call the following models if component number less than 0.
      ierr1=0
      if (i.le.ncmax .and. hstar.eq.'#') then
        k=ICHAR(htype(3:3))-48    !Get the number following the 'PS', 'DL', etc. in the fld file.
        if (k.lt.0) k=0
        if (k.gt.9) k=0
c  Melting line models.
        if (hflag.eq.'MLT' .and. i.gt.0) then
          hmeltk(i)=htype
          call SETANC (nread,i,5,ierr1,herr1)
          himelt(i)=htype//hcite
          imlttyp(i)=k
c  Sublimation line models.
        elseif (hflag.eq.'SBL' .and. i.gt.0) then
          itypsbl(i)=k
          hsublk(i)=htype
          call SETANC (nread,i,6,ierr1,herr1)
          hisubl(i)=htype//hcite
c  Vapor pressure equations.
        elseif (hflag.eq.'PS ' .and. i.gt.0) then
          hpsk(i)=htype
          call SETANC (nread,i,2,ierr1,herr1)
          hips(i)=htype//hcite
          ipstyp(i)=k
c  Liquid pressure equations (for pseudo-pure fluids).
        elseif (hflag.eq.'PL ' .and. i.gt.0) then
          hplk(i)=htype
          call SETANC (nread,i,1,ierr1,herr1)
          hipl(i)=htype//hcite
          ipltyp(i)=k
c  Saturated liquid density equations.
        elseif (hflag.eq.'DL ' .and. i.gt.0) then
          hdlk(i)=htype
          call SETANC (nread,i,3,ierr1,herr1)
          hidl(i)=htype//hcite
          idltyp(i)=k
c  Saturated vapor density equations.
        elseif (hflag.eq.'DV ' .and. i.gt.0) then
          hdvk(i)=htype
          call SETANC (nread,i,4,ierr1,herr1)
          hidv(i)=htype//hcite
          idvtyp(i)=k
        endif
        if (ierr1.ne.0 .and. ierr.eq.0) goto 102
      endif

      if (lbdata) goto 500
      if (hflag.ne.'END') goto 100

c  End of file reached.
c   If fluid-specific 'ETA' and/or 'TCX' models have not been loaded, use
c   the 'TRN' model (e.g., the ECS model).
      if (i.le.nq) then
        if (.not.leta .and. hetak(i).ne.'NBS')
     &      call ERRNUM(105,100+i,htcxk(i),' ',0d0,0d0,0d0,ierr,herr) !User-specified thermal conductivity equation not found in fluid file.
        if (.not.ltcx .and. htcxk(i).ne.'NBS')
     &      call ERRNUM(105,100+i,htcxk(i),' ',0d0,0d0,0d0,ierr,herr) !User-specified thermal conductivity equation not found in fluid file.
        if (.not.leta) hetak(i)=hetemp
        if (.not.ltcx) htcxk(i)=httemp
      endif
      close (unit=nread,err=101)

c  If a pure fluid equation for the GERG-2008 model was requested, but does
c   not exist, use the default (FEQ) without returning an error message.
      if (.not.leosfl .and. i.le.ncmax) then
        if (heosk(i).eq.'FEK') then
          heosk(i)='FEQ'
          n7total=0
          n7st=0
          n7oper=0
          goto 10
        endif
       call ERRNUM(105,i,heosk(i),hFile(i)(1:100),0d0,0d0,0d0,ierr,herr) !Equation of state not found in fluid file.
       goto 500
      endif

c  Ideal-gas heat capacity equation not found in fluid file.
      if (i.le.ncmax) then
        if (i.gt.0) then
          if (icpflg.eq.0 .or. (icpflg.eq.1.and.hcpk(i).ne.'CPP')) then
            if (iphi0flg.ne.2)
     &        call ERRNUM (107,i,' ',' ',0d0,0d0,0d0,ierr,herr)
          endif
        endif
        if (.not.lprfl) call SETPR(2,999,i,ierr1,herr1)    !Call SETPR to put constants into PR common block if PRT not in FLD file.
      endif
      goto 500

c  Error encountered while reading fluid file.
 102  call ERRNUM (102,i,hname(i),' ',0d0,0d0,0d0,ierr,herr)
      if (i.le.ncmax) then
        if (.not.leta) hetak(i)=hetemp           !If fluid-specific 'ETA' and/or 'TCX' models have not been loaded, use the 'TRN' model.
        if (.not.ltcx) htcxk(i)=httemp
      endif
      goto 500

c  Error in opening fluid file.
 101  hfilei=hFile(i)                            !Fluid file name plus a possible path.
      call FINDLAST (hfilei,CHAR(92),1,j)        !Location of last directory separator in Windows.
      if (j.eq.0) call FINDLAST (hfilei,'/',1,j) !  in Linux.
      if (j.eq.0) call FINDLAST (hfilei,':',1,j) !  on a Mac.
      hstr=hfilei(j+1:)                          !File name.
      hfilei=hfilei(1:j)                         !Path sent with fluid file name.
      call FINDLAST (hstr,' ',-255,j)
      if (j.gt.0) then
        if (UCASE(hstr).ne.hAltFluidName .and. hAltFluidName.ne.' ')then
          hstr=hstr(1:j)//' or '//hAltFluidName  !Add alternate fluid file name to error message if not the same as the name sent to SETUP.
        endif
        call FINDLAST (hstr,' ',-255,j)
        if (hpath.ne.' ') hstr=hstr(1:j)//' in the path '//hpath          !Add the path to the error message.
        if (hfilei.ne.' ') hstr=hstr(1:j)//' in the path '//hfilei
      endif
      call ERRNUM (101,i,hstr,' ',0d0,0d0,0d0,ierr,herr)
      goto 198
 1101 call ERRNUM (-101,i,hFile(i)(1:80),' ',0d0,0d0,(xerr),ierr,herr)
      goto 198

 500  call SWAPERR (ierr,ierr2,herr,herr2)
 198  close (unit=nread,err=103)
 103  RETURN

 2080 format (a255)

      end                                              subroutine SETFLD
c
c ======================================================================
c
      subroutine SETFLD2 (nread,i,hstar,hflag,htype,hcite,
     &                   heqn,hreff,hetemp,httemp,ierr,herr)
c
c  Define transport, dielectric, and surface tension models.
c  See comments to SETFLD for additional information.
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  11-19-94  MM, original version (SETFLD)
c  11-08-06 EWL, load TCX and ETA coefficients into negative arrays
c  12-26-06 MLH, add VS4 model
c  11-02-07 MLH, add Chung model
c  10-24-14 MLH, add CI3 model for collision integral
c  12-07-15 MLH, add VS6 model for Velosovic type models that use CI3
c  06-22-16 EWL, shorten code

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character hstar*1,hcite*251
      character*3 heqn,hetemp,httemp,hflag,htype
      character*255 hreff,herr,herr1

      ierr =0
      ierr1=0
      heqn =' '
      herr =' '
      herr1=' '
      hreff=' '
      if (mxref.le.0) RETURN

c  Set up auxiliary models.
      if (hflag.eq.'AUX') then

c  Do not call collision integral model (or other auxiliary models
c    associated with the transport properties) if component number < 0.
c  There is only one reference fluid for transport properties so arrays
c    are not dimensioned for the negative component numbers to
c    accommodate the multiple reference fluids in the ECS-thermo model.

c  Functions for collision integral (used in transport correlations).
        if (htype(1:2).eq.'CI' .and.
     &     (htype.eq.hmdeta(i) .or. htype.eq.hmdtcx(i))) then
           call SETCI (nread,i,htype,ierr1,herr1)

c  Thermal conductivity critical enhancement models.
        else
          iFlg=0
          if (htype.eq.htcxcrecs(i) .and. htype.eq.'TK6')  iFlg=1
          if (htype.eq.htcxcr(i) .and. htype(1:2).eq.'TK') iFlg=1
          if (htype.eq.'TK3' .and. lEQ0(Tmtck(i)))         iFlg=1  !If no other critical enhancement has been loaded and TK3 is found, load it for use with ECS if needed.
          if (iFlg.eq.1) then
            call SETTCE (nread,i,htype,ierr1,herr1)
            hitcxc(i)=htype//hcite
          endif
        endif
        call SWAPERR (ierr,ierr1,herr,herr1)
      endif

c  TRN (ECS transport) property models.
c  The 'TRN' models apply to both viscosity and thermal conductivity.
      if (hflag.eq.'TRN') then
        if (hstar.eq.'#') then    !NIST-recommended model
          if (nc.eq.1) then
            if (hvismx.eq.'NBS') hvismx=htype
            if (htcxmx.eq.'NBS') htcxmx=htype
          endif
          if (i.ge.1 .and. hetak(i).eq.'NBS') hetak(i)=htype
          if (i.ge.1 .and. htcxk(i).eq.'NBS') htcxk(i)=htype
          leta=.true.   !When the TRN model is the NIST-recommended one, the ECS method is
          ltcx=.true.   ! equivalent to a fluid-specific model for purposes of the leta and ltcx flags.
        else
c  Load the 'TRN' model if a fluid-specific 'ETA' or 'TCX' has not yet been loaded.
c   This is indicated by the flags leta and ltcx.  If a fluid-specific
c   correlation is encountered later in the file this will be overwritten.
c   This is necessary to cover the case of a fluid-specific (and NIST-rec)
c   model available for only one of the transport properties.
          hetemp=htype
          httemp=htype
        endif

c  Extended corresponding states model for transport properties found.
c  Always load the ECS model for possible use if a pure fluid correlation is out of range.
        if (htype.eq.'ECS') then
          if (hvismx.ne.'NUL' .or. htcxmx.ne.'NUL') then
            call SETTRN (nread,i,hreff,heqn,ierr1,herr1)
            call SWAPERR (ierr,ierr1,herr,herr1)
            if (ierr.ne.0) goto 102
            if (i.gt.0) then
              if (hieta(i)(1:3).eq.'NUL') hieta(i)=htype//hcite
              if (hitcx(i)(1:3).eq.'NUL') hitcx(i)=htype//hcite
            endif
          endif
        else
          goto 107      !Unidentified model found in file.
        endif
      endif

c  Pure fluid viscosity models.
      if (i.le.nq) then
      if (hflag.eq.'ETA' .or. hflag.eq.'VIS') then
        if (hstar.eq.'#') then    !NIST-recommended model
          if (nc.eq.1 .and. hvismx  .eq.'NBS') hvismx=htype
          if ( i.ne.0 .and. hetak(i).eq.'NBS') hetak(i)=htype
        endif
        if (htype.eq.hetak(i) .and. htype(1:2).eq.'VS') then
          call SETVIS (nread,i,htype,ierr1,herr1)
          leta=.true.   !Flag that a fluid-specific viscosity model was loaded.
          hieta(i)=htype//hcite
          call SWAPERR (ierr,ierr1,herr,herr1)
        endif
      endif

c  Pure fluid thermal conductivity models.
      if (hflag.eq.'TCX') then
        if (hstar.eq.'#') then    !NIST-recommended model
          if (nc.eq.1 .and. htcxmx  .eq.'NBS') htcxmx=htype
          if ( i.ne.0 .and. htcxk(i).eq.'NBS') htcxk(i)=htype
        endif
        if (htype.eq.htcxk(i) .and. htype(1:2).eq.'TC') then
          call SETTCX (nread,i,htype,ierr1,herr1)
          ltcx=.true.   !Flag that a fluid-specific thermal conductivity model was loaded.
          hitcx(i)=htype//hcite
          call SWAPERR (ierr,ierr1,herr,herr1)
        endif
      endif
      endif

c  Pure fluid surface tension models.
c  Do not call surface tension models if component number greater than ncmax.

      if (i.le.ncmax) then
        if (hflag.eq.'STN') then
          if (hstar.eq.'#' .and. i.gt.0) then         !NIST-recommended model
            if (nc.eq.1 .and. hstnmx  .eq.'NBS') hstnmx  =htype
            if (hstnk(i).eq.'NBS') hstnk(i)=htype
          endif
          if (htype.eq.'ST1' .and. i.gt.0) then       !Surface tension model 1
            if (hstnk(i).eq.'ST1') then
              call SETANC (nread,i,7,ierr1,herr1)
              histn(i)=htype//hcite
            endif
          else
            goto 107                                  !Unidentified model found in file
          endif
        endif

c  Pure fluid dielectric constant models.
c  Do not call dielectric constant models if component number greater than ncmax.
        if (hflag.eq.'DE ') then
          if (hstar.eq.'#' .and. i.gt.0) then         !NIST-recommended model
            if (nc.eq.1 .and. hdilmx  .eq.'NBS') hdilmx  =htype
            if (hdilk(i).eq.'NBS') hdilk(i)=htype
          endif
          if (htype(1:2).eq.'DE' .and. i.gt.0) then   !Dielectric constant model
            if (hdilk(i)(1:2).eq.'DE') then
              call SETANC (nread,i,8,ierr1,herr1)
              hidiel(i)=htype//hcite
            endif
          else
            goto 107                                  !Unidentified model found in file
          endif
        endif
      endif
      RETURN

 107  if (hstar.eq.'#' .and. i.le.ncmax)
     &  call ERRNUM (-107,i,htype,' ',0d0,0d0,0d0,ierr,herr)
      RETURN
 102  call ERRNUM (102,i,hcite,' ',0d0,0d0,0d0,ierr,herr)
      RETURN

      end                                             subroutine SETFLD2
c
c ======================================================================
c
      subroutine SETMOD (ncomp,htype,hmix,hcomp,ierr,herr)
c
c  Set model(s) other than the NIST-recommended ('NBS') ones.
c
c  This subroutine must be called before SETUP; it need not be called
c  at all if the default (NIST-recommended) models are desired.
c
c  Inputs:
c    ncomp--Number of components (1 for pure fluid) (integer)
c    htype--Flag indicating which model to set (character*3):
c           'NBS' - Reset all of the above model types to 'NBS' (values of hmix and hcomp are ignored)
c           'EOS' - Equation of state
c           'ETA' - Viscosity
c           'TCX' - Thermal conductivity
c           'STN' - Surface tension
c     hmix--Mixture model for the property specified in htype (character*3) (ignored if number of components = 1):
c           'NBS' - Use NIST recommendation for specified fluid/mixture
c           'HMX' - Mixture Helmholtz model for thermodynamic properties
c           'ECS' - Extended corresponding states for viscosity or therm. cond.
c           'STX' - Surface tension mixture model
c    hcomp--Component model(s) for property specified in htype [array (1..ncomp) of character*3]:
c           'NBS' - NIST recommendation for specified fluid/mixture
c           'FEQ' - Helmholtz energy model
c           'BWR' - Pure fluid modified Benedict-Webb-Rubin (MBWR)
c           'ECS' - Pure fluid thermo extended corresponding states
c           'ECS' - Extended corresponding states (all fluids)
c           'PRT' - Peng-Robinson (PRT model from fluid file)
c           'VS1' - The 'composite' model for R134a, R152a, NH3, etc.
c           'VS2' - Younglove-Ely model for hydrocarbons
c           'VS4' - Generalized friction theory of Quinones-Cisneros and Deiters
c           'VS5' - Chung et al. (1988) predictive model
c           'VS6' - Vesovic form of VS1 model
c           'VS7' - Polynomial/exponential model
c           'ECS' - Extended corresponding states (all fluids)
c           'TC1' - The 'composite' model for R134a, R152a, etc.
c           'TC2' - Younglove-Ely model for hydrocarbons
c           'TC5' - Chung et al. (1988) predictive model
c           'ST1' - Surface tension as f(tau); tau = 1 - T/Tc
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c                      113 - ncomp outside of bounds
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  03-20-96  MM, original version
c  05-17-11 EWL, change nc to ncomp

      include 'COMMONS.INC'
      include 'COMTRN.INC'
      character*3 htype,htype2,hmix,hmix2,hcomp(1:ncmax),hcomp2(1:ncmax)
      character*255 herr,UCASE
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETMOD

      ierr=0
      herr=' '
      if (ncomp.lt.1 .or. ncomp.gt.ncmax) then
        call ERRNUM (113,0,'SETUP',' ',0d0,0d0,0d0,ierr,herr)
        RETURN
      endif

c  Repeat initialization of the following from SETUP in case SETMOD is called
c   before first call to SETUP.
      call RESETA (0)
      hsavpr=' '
      hsavprk=' '
      ncSet=ncomp
      htype2=UCASE(htype)
      hmix2=UCASE(hmix)
      do i=1,ncomp
        hcomp2(i)=UCASE(hcomp(i))
      enddo

      if (htype2.ne.'NBS') iReset=1
      if (htype2.eq.'NBS' .or. lReset) then
c  Reset all models.
        call SETEOSMX ('NBS')
        hvismx ='NBS'
        htcxmx ='NBS'
        hstnmx ='NBS'
        hdilmx ='NBS'
        heosk  ='NBS'
        hcpk   ='NBS'
        hetak  ='NBS'
        hetacr ='NBS'
        htcxk  ='NBS'
        htcxcr ='NBS'
        hmdeta ='NBS'
        hmdtcx ='NBS'
        iCP0xx = 0
        hpsk  ='NBS'   !Ancillary equations
        hplk  ='NBS'
        hdlk  ='NBS'
        hdvk  ='NBS'
        hmeltk='NBS'
        hsublk='NBS'
        hdilk ='NBS'
        hstnk ='NBS'
        hetacr(1:ncmax)='NUL'
        htcxcr(1:ncmax)='NUL'
        hmdeta(1:ncmax)='NUL'
        hmdtcx(1:ncmax)='NUL'
        lReset=.false.
        if (htype2.eq.'NBS') hcomp2='NBS'
      endif

c  Equation of state
      if (htype2.eq.'EOS') then
        call SETEOSMX (hmix2)
        if (ncomp.eq.1) call SETEOSMX (hcomp2(1))
        do i=1,ncomp
          if (hcomp2(i).ne.' ') heosk(i)=hcomp2(i)
        enddo

c  Viscosity
      elseif (htype2.eq.'ETA' .or. htype2.eq.'VIS') then
        hvismx=hmix2
        if (ncomp.eq.1) hvismx=hcomp2(1)
        do i=1,ncomp
          if (hcomp2(i).ne.' ') hetak(i)=hcomp2(i)
        enddo

c  Thermal conductivity
      elseif (htype2.eq.'TCX') then
        htcxmx=hmix2
        if (ncomp.eq.1) htcxmx=hcomp2(1)
        do i=1,ncomp
          if (hcomp2(i).ne.' ') htcxk(i)=hcomp2(i)
        enddo

c  Surface tension specification
      elseif (htype2.eq.'STN') then
        hstnmx=hmix2
        if (ncomp.eq.1) hstnmx=hcomp2(1)
        do i=1,ncomp
          if (hcomp2(i).ne.' ') hstnk(i)=hcomp2(i)
        enddo

c  Do nothing for NBS, but check for it before the next statement.
      elseif (htype2.eq.'NBS') then

      else
        call ERRNUM (504,0,'SETMOD',htype2,0d0,0d0,0d0,ierr,herr)
        RETURN
      endif

      call ERRNUM (-912,0,' ',' ',0d0,0d0,0d0,ierr,herr)

      end                                              subroutine SETMOD
c
c ======================================================================
c
      subroutine SETREF (hrf,ixflag,x0,h0,s0,T0,P0,ierr,herr)
c
c  Set reference state enthalpy and entropy.
c
c  This subroutine must be called after SETUP; it need not be called at all
c  if the reference state specified in the call to SETUP is to be used.
c
c  Inputs:
c      hrf--Reference state for thermodynamic calculations (character*3):
c           'NBP' - h,s = 0 at normal boiling point(s)
c           'ASH' - h,s = 0 for saturated liquid at -40 C (ASHRAE convention)
c           'IIR' - h = 200 kJ/kg, s = 1 kJ/kg-K for sat. liquid at 0 C (IIR convention)
c           'DEF' - Default reference state as specified in fluid file
c           'OTH' - Other, as specified by h0, s0, T0, P0 (real gas state)
c           'OT0' - Other, as specified by h0, s0, T0, P0 (ideal-gas state)
c           'NA'  - Not applicable, do not set up the reference state.
c                   The values of e, h, and s will have a random reference state.
c                   Do not use except for EOS testing.
c           '???' - Set hrf to the value of the current reference state and exit
c   ixflag--Composition flag:
c           1 - Reference state applied to pure components
c           2 - Reference state applied to mixture x0
c       x0--Composition for which h0 and s0 apply;
c           array(1:nc) (array of mole fractions)
c           This is useful for mixtures of a predefined composition,
c           e.g., refrigerant blends such as R410A.
c           Only has meaning if ixflag = 2
c       h0--Reference state enthalpy at T0, P0, and x0 [J/mol] (only has meaning if hrf = 'OTH' or 'OT0')
c       s0--Reference state entropy at T0, P0, and x0 [J/mol-K] (only has meaning if hrf = 'OTH' or 'OT0')
c       T0--Reference state temperature [K] (only has meaning if hrf = 'OTH' or 'OT0')
c           T0 = -1 indicates saturated liquid at normal boiling point (bubble point for a mixture)
c       P0--Reference state pressure [kPa] (only has meaning if hrf = 'OTH' or 'OT0')
c           P0 = -1 indicates saturated liquid at T0 (and x0)
c           P0 = -2 indicates saturated vapor at T0 (and x0)
c
c  Outputs:
c     ierr--Error flag:
c            0 - Successful
c           22 - Tmin > Tref for IIR reference state
c           23 - Tcrit < Tref for IIR reference state
c           24 - Tmin > Tref for ASHRAE reference state
c           25 - Tcrit < Tref for ASHRAE reference state
c           26 - Tmin > Tnbp for NBP reference state
c          -28 - Can't apply 'DEF' to mixture; will apply to pure components
c          -29 - Unknown reference state specified; will use 'DEF'
c          119 - Convergence failure in calculating reference state
c     herr--Error string (character*255)
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-11-96  MM, original version - extracted from old subroutine SETUP
c  05-31-06 EWL, add ability to find out what the reference state is when hrf='???'
c  12-06-12 EWL, remove calls to PUREFLD
c  07-04-16 EWL, shorten and rework code

      include 'COMMONS.INC'
      character*3 hrefi(nx),hrdef,hrf2,hrf*(*)
      character*255 herr,herr2,herrsv,UCASE
      dimension z(ncmax),x0(ncmax),xl(ncmax),xv(ncmax),pref(nx)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETREF

c  Initialize error codes, strings, and flags.
      ierr=0
      ierrsv=0
      iSetup=0
      iSetRef=0
      herr=' '
      herrsv=' '
      l1same=.false.         !Flag for initialization of pure components.
      l2same=.false.         !Flag for application of reference states to mixtures.
      ixflg=ixflag           !Copy ixflag to new variable (value is changed if input href is not valid).
      hrf2=UCASE(hrf)
      if (hrf2.eq.' ') then
        hrf2=hsvref
        if (hsvref.eq.'NBS') hrf2='DEF'
      elseif (hrf2.eq.'???') then
        herr=hrefdef(1)      !Return the default reference state for a pure fluid.
        RETURN
      endif
      if (lSkipRef) RETURN   !Exit if the flag to skip SETREF has been set in subroutine FLAGS.
      if (lAGA8) RETURN      !Exit if the AGA8 flag is on.

c  Check to see if reference state has changed, if so, exit without any calculations.
      if (hrf2.eq.hsvref) then
        l1same=.true.
        if (hrf2(1:2).eq.'OT') then     !Check h0, s0, T0, and P0 only if 'OTHer' reference state is used.
          if (ABS(s0-s0sav).gt.1d-10 .or. ABS(h0-h0sav).gt.1d-10 .or.
     &        ABS(T0-T0sav).gt.1d-10 .or. ABS(P0-P0sav).gt.1d-10)
     &    l1same=.false.
        endif
        if (ixflg.eq.ixfsav) then      !Check if composition has changed.
          l2same=.true.
          if (ixflg.eq.2) then         !Check x0 only if ixflag = 2.
            do i=1,nc
              if (ABS(x0(i)-x0sav(i)).gt.1d-10) l2same=.false.
            enddo
          endif
        endif
c  Return if all inputs are the same and also SETUP has not been executed
c   since last call to SETREF (as indicated by iSetRf).
        if (l1same .and. l2same .and. iSetRf.eq.1) RETURN
      endif

c  Exit for 'NA' ('Not Applicable').
      if (hrf2.eq.'NA' .or.
     &   (hrf2.eq.'DEF' .and. hrefdef(1).eq.'NA' .and. nc.eq.1)) then
        href=0d0
        sref=0d0
        goto 500
      endif

c  Check if all fluids have same reference state for a DEFault input.
      if (hrf2.eq.'DEF' .and. ixflg.eq.2 .and. nc.gt.1) then
        ldef=.true.
        do i=1,nc
        if (hrefdef(1).ne.hrefdef(i) .or.
     &      hrefdef(i)(1:2).eq.'OT') ldef=.false.
        enddo
        if (.not.ldef) then
          call ERRNUM (-28,0,' ',' ',0d0,0d0,0d0,ierrsv,herrsv)
          ixflg=1
        endif
      endif

      if (l1same .and. iSetRf.eq.1 .and. ixflg.ge.ixfsav) then
c  Can skip initialization of pure components if
c    Reference state is the same.
c    SETUP has not been called since last call to SETREF.
c    Value of ixflag is >= ixfsav, i.e., this call to SETREF applies the reference state to
c      a mixture while the previous call applied the reference state to each of the pure fluids.


c  Otherwise, check for valid input reference state and then calculate reference values.
      else
        if (hrf2.eq.'IIR' .or. hrf2.eq.'ASH' .or. hrf2.eq.'NBP' .or.
     &      hrf2(1:2).eq.'OT') then
          hrefi=hrf2
        else
c  DEFault or unknown reference state.
          hrefi(1:nc)=hrefdef(1:nc)
          ixflg=1            !DEFault reference state applied to mixture not allowed.
          if (hrf2.ne.'DEF')
     &      call ERRNUM (-29,0,' ',' ',0d0,0d0,0d0,ierrsv,herrsv)
        endif





c  Initialize reference temperature, entropy, and enthalpy.
        do i=1,nc
100       tref(i)=298.15d0
          pref(i)=1d0
          href(i)=0d0
          sref(i)=0d0
          Dref(i)=1d-3

c  IIR reference state.
          if (hrefi(i).eq.'IIR') then
            tref(i)=273.15d0
            pref(i)=-1d0          !Code for saturated liquid.
            href(i)=200d0*wmas(i) !Reference enthalpy is 200.0 kJ/kg
            sref(i)=wmas(i)       !Reference entropy is 1.0 kJ/(kg.K)

c  ASHRAE reference state.
          elseif (hrefi(i).eq.'ASH') then
            tref(i)=233.15d0
            pref(i)=-1d0          !Code for saturated liquid

c  NBP - Normal boiling point reference state.
          elseif (hrefi(i).eq.'NBP') then
            tref(i)=Tnbp(i)
            pref(i)=-1d0          !Code for saturated liquid

c  OTHer reference state - use input values of h0, s0, T0, P0 or values from fluid file.
          elseif (hrefi(i)(1:2).eq.'OT') then
            if (hrf2.eq.'DEF') then
              tref(i)=Tdef(i)     !If input to SETREF is 'DEF' reference T,P,h,s come from fluid file.
              pref(i)=Pdef(i)
              href(i)=hdef(i)
              sref(i)=sdef(i)
            else
              tref(i)=T0          !Otherwise, reference T,P,h,s are inputs to SETREF.
              pref(i)=P0
              href(i)=h0
              sref(i)=s0
            endif
          endif


c  Check limits of equation of state against reference state temperature.
          if (i.gt.0) then
            call LIMITK ('EOS',i,tref(i),0d0,0d0,Tmin,T,D,P,j,herr2)

c  IIR reference state.
            if (hrefi(i).eq.'IIR') then
              if (Tmin.gt.273.15d0) then    !Minimum temperature of EOS is above IIR reference state temperature.
                call ERRNUM (22,i,hrefdef(i),' ',Tmin,0d0,0d0,
     &                       ierrsv,herrsv)
              elseif (Tcrit(i).lt.274.15d0) then !Critical temperature is too low to use IIR reference state.
                call ERRNUM (23,i,hrefdef(i),' ',Tcrit(i),0d0,0d0,
     &                       ierrsv,herrsv)
              endif

c  ASHRAE reference state.
            elseif (hrefi(i).eq.'ASH') then
              if (Tmin.gt.233.15d0) then    !Minimum temperature of EOS is above reference state temperature.
                call ERRNUM (24,i,hrefdef(i),' ',Tmin,0d0,0d0,
     &                      ierrsv,herrsv)
              elseif (Tcrit( i).lt.234.15d0) then     !Critical temperature is too low to use IIR reference state.
                call ERRNUM (25,i,hrefdef(i),' ',Tcrit(i),0d0,0d0,
     &                       ierrsv,herrsv)
              endif

c  NBP - Normal Boiling Point reference state.
            elseif (hrefi(i).eq.'NBP') then
              if (Tmin.gt.Tnbp(i)) then     !Minimum temperature of EOS is above boiling point temperature (this is a problem with CO2).
                call ERRNUM (26,i,hrefdef(i),' ',Tmin,Tnbp(i),0d0,
     &                       ierrsv,herrsv)
              endif
            endif

c  If specified reference state is outside limits for component i,
c   use default state (read in from fluid file).
            if (ierrsv.ge.22 .and. ierrsv.le.26) then
              ixflg=1        !Apply reference state to only the pure fluids.
              if (hrefi(i).ne.hrefdef(i)) then
                hrefi(i)=hrefdef(i)
                goto 100
              endif
            endif
          endif
        enddo





c  Apply reference enthalpy and entropy to each component.
c  This is for ixflag = 1; also used as initialization for ixflag = 2.
        do i=1,nc
          z(1:nc)=0d0
          z(i)=1d0
          D=Dref(i)
          if (Tref(i).le.0.) Tref(i)=300d0

c  Find bubble point at one atmosphere.
          if (hrefi(i).eq.'NBP' .or. tref(i).eq.-1.) then
            pnbp=101.325d0
            call SATP (pnbp,z,1,tref(i),D,Dv,xl,xv,ierr,herr)

c  Find saturated liquid or vapor density (includes ASH and IIR states).
          elseif (pref(i).lt.0.) then
            if (pref(i).lt.-1.99d0) then
              call SATT (tref(i),z,2,P,Dl,D,xl,xv,ierr,herr)
            else
              call SATT (tref(i),z,1,P,D,Dv,xl,xv,ierr,herr)
            endif

c  Find real gas density at specified T and P.
          elseif (hrefi(i).eq.'OTH') then
            call TPFLSH (tref(i),pref(i),z,D,Dl,Dv,xl,xv,
     &                  q,e,h,s,Cv,Cp,w,ierr,herr)

c  Find ideal-gas density at specified T and P.
          elseif (hrefi(i).eq.'OT0') then
            D=pref(i)/Reos(i)/tref(i)
          endif
          Dref(i)=D

c  Error encountered in calculating reference state.
          if (ierr.ge.100 .or. Dref(i).le.0.) then
            Dref(i)=1d-3
            call ERRNUM (119,i,herr,' ',0d0,0d0,0d0,ierr,herr)
          elseif (ierr.gt.0) then      !Ignore out of bound errors.
            ierr=0
          endif
          TsvCp0=0d0         !Reset TsvCp0 to force recalculation of Cp0.
          if (hrefi(i).eq.'OT0') then
            call THERM0 (tref(i),Dref(i),z,ptherm,e,h,s,Cv,Cp,w,a,g)
          else
            call THERM (tref(i),Dref(i),z,ptherm,e,h,s,Cv,Cp,w,hjt)
          endif
          sref(i)=s          !Store reference entropy and enthalpy in common blocks.
          href(i)=h
          if (Tref(i).le.0.) Tref(i)=300d0
        enddo
      endif                  !End of initialization for pure components.





c  Reference state applied to a mixture of a specified composition.
      if (ixflg.eq.2 .and. nc.gt.1) then
        hmix=0d0
        smix=0d0
        if (hrf2(1:2).eq.'OT') then
          tmix=T0
          pmix=P0
          hmix=h0
          smix=s0
c  DEFault reference state - should get here only if all components have the
c   same default reference state stored in their respective fluid files.
        else
          hrdef='NUL'        !Initialize pointer to default reference state.
          if (hrf2.eq.'DEF') hrdef=hrefdef(1)
c  IIR reference state.
          if (hrf2.eq.'IIR' .or. hrdef.eq.'IIR') then
            tmix=273.15d0
            pmix=-1d0        !Code for saturated liquid
            wmm=WMOL(x0)
            hmix=200d0*wmm   !Reference enthalpy is 200.0 kJ/kg.
            smix=wmm         !Reference entropy is 1.0 kJ/(kg.K).
c  ASHRAE reference state.
          elseif (hrf2.eq.'ASH' .or.hrdef.eq.'ASH') then
            tmix=233.15d0
            pmix=-1d0        !Code for saturated liquid
          else
c  Boiling point or unknown reference state.  Find bubble point of specified mixture.
            pnbp=101.325d0
            T=300d0
            call SATP (pnbp,x0,1,T,Dmix,Dv,xl,xv,ierr,herr)
            tmix=T
            pmix=-1d0        !Code for saturated liquid
          endif
c  Check mixture reference temperature against limits.
c  If outside limits, take average of component Tref's.
          T=tmix
          call LIMITX ('EOS',T,0d0,0d0,x0,Tmin,Tmax,Dmx,pmx,j,herr2)
          if (T.lt.Tmin .or. T.gt.Tmax) tmix=SUM(x0(1:nc)*tref(1:nc))
        endif

c  Find liquid or vapor reference state density (includes IIR and ASHrae states).
        if (pmix.lt.0.) then
          if (pmix.lt.-1.99d0) then
            call SATT (tmix,x0,2,P,Dl,Dmix,xl,xv,ierr,herr)
          else
            call SATT (tmix,x0,1,P,Dmix,Dv,xl,xv,ierr,herr)
          endif
        else
c  Find density at specified T and P.
          if (tmix.lt.1) then
            Dmix=1d-10  !pmix/tmix/xRgas    !For T<1 K, assume the user wants h and s at a zero temperature reference state.
          else
            call TPFLSH (tmix,pmix,x0,Dmix,Dl,Dv,xl,xv,q,e,h,s,Cv,Cp,w,        !Get reference state density.
     &                   ierr,herr)
          endif
        endif

        TsvCp0=0d0      !Reset TsvCp0 to force recalculation of Cp0.
        call THERM (tmix,Dmix,x0,P,e,h,s,Cv,Cp,w,hjt)
c  Modify sref and href by amount needed for specified mixture to agree with s0 and h0.
        sref(1:nc)=sref(1:nc)+s-smix
        href(1:nc)=href(1:nc)+h-hmix
      endif





c  Flag iSetRf indicates that SETREF has been called.  It is reset to 0 each time SETUP is called.
 500  if (ierr.lt.100) then
        iSetRf=1
        ixfsav=ixflg
        hsvref=hrf2
        h0sav=h0
        s0sav=s0
        T0sav=T0
        P0sav=P0
        x0sav(1:nc)=x0(1:nc)
      else
        hsvref=' '
      endif

c  Set error flag/message with the more significant errors.
      call SWAPERR (ierr,ierrsv,herr,herrsv)
      iSetRef=-1
      if (ierr.gt.0) iSetRef=ierr

      end                                              subroutine SETREF
c
c ======================================================================
c
      subroutine SETMIX (hMixNme,hFmix,hrf,ncc,hFiles,z,ierr,herr)
c
c  Open a mixture file (e.g., R410A.mix) and read constituents and
c  mole fractions.
c
c  Inputs:
c  hMixNme--Mixture file name (character*255)
c    hFmix--File containing mixture coefficients (character*255)
c      hrf--Reference state (character*3); See subroutine SETUP for specifics.
c
c  Outputs:
c      ncc--Number of fluids in mixture
c   hFiles--Array of file names specifying mixture components
c           that were used to call setup (character*255)
c        z--Array of mole fractions for the specified mixture
c     ierr--Error flag:  0 - Successful
c                      101 - Error in opening file
c                     -102 - Mixture file contains mixing parameters
c                      805 - Sum of compositions not equal to one
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  03-27-06 EWL, original version

      include 'COMMONS.INC'
      character hrf*3,UCASE*255
      character*255 hMixNme,hFmix,herr,herr2,hMixName,hFiles(ncmax),hfli
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETMIX

      ierr=0
      ierr2=0
      herr=' '

      nread=12
      hAltFluidName=' '
      call OPENFL (0,nread,hMixNme,hfli,2,ierr,herr)
      if (ierr.ne.0) goto 101
      read (nread,'(a255)',err=101,end=101) hMixName
      hname(0)=hMixName
      hnam80(0)=hMixName
      read (nread,*,err=101,end=101) xmw,TC,Pc,Dc  !Read in the molar mass, Tc, Pc, and Dc of the mixture.
      read (nread,*,err=101,end=101) ncc
      do i=1,ncc
        read (nread,'(a255)',err=101,end=101) hFiles(i)
        call RFFILE (hFiles(i),hFmix)
      enddo
      read (nread,*,err=101,end=101) (z(i),i=1,ncc)
      sumx=SUM(z(1:ncc))
      if (ABS(sumx-1d0).gt.1d-4 .and. sumx.gt.0. .and. ncc.gt.1)then
        call ERRNUM (805,0,'SETMIX',' ',0d0,0d0,0d0,ierr2,herr2)
        z(1:ncc)=z(1:ncc)/sumx
      endif
      read (nread,*,err=101,end=101) i
      if (i.ne.0) call ERRNUM (-102,0,' ',' ',0d0,0d0,0d0,ierr2,herr2)
      close (nread)
      call SETUP (ncc,hFiles,hFmix,hrf,ierr,herr)
      call SWAPERR (ierr,ierr2,herr,herr2)  !If no error from SETUP, return errors set above.
      PDmixTc=Tc
      PDmixPc=Pc
      PDmixDc=Dc
      ncPDmix=ncc
      zPDmix(1:nc)=z(1:nc)
      hPDmix=UCASE(hMixNme)
      i=INDEX(hPDmix,'.MIX')
      if (i.gt.0) hPDmix=hPDmix(1:i-1)
      RETURN

 101  call ERRNUM (101,101,'SETMIX',hMixNme,0d0,0d0,0d0,ierr,herr)

      end                                              subroutine SETMIX
c
c ======================================================================
c
      subroutine SETNC (ncomp)
c
c  Allow the user to modify the value of nc (the number of components in
c  a mixture) so that a subset of the loaded mixtures can be used.
c  For example, a 4 component mixture could be set up, but nc could
c  be set to 3 to calculate properties of the mixture of the first three
c  components.  The last component can then be used as a pure fluid.
c  For example, SETUP could be called to load four fluids, R32.fld,
c  R125.fld, R134a.fld, and R407C.ppf, followed by a call to SETNC(3)
c  so that the 4th component is not used in mixture calculations.
c  The pseudo-pure fluid equation in R407C.ppf can be accessed by calling
c  PUREFLD(4) to get single-phase thermodynamic properties, and VLE
c  states or transport properties that require the first three components
c  can be obtained by calling PUREFLD(0) and setting the composition
c  array z with the appropriate values.
c
c  Input:
c    ncomp--number of components in the mixture
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-04-10 EWL, original version

      include 'COMMONS.INC'
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETNC

      nc=ncomp
      if (nc.gt.1 .and. heosmx.eq.'FEQ') call SETEOSMX ('HMX')
      end                                               subroutine SETNC
c
c ======================================================================
c
      subroutine GETMOD (icomp,htype,hcode,hcite)
c
c  Retrieve citation information for the property models used.
c
c  Inputs:
c    icomp--Pointer specifying component number;
c           zero and negative values are used for ECS reference fluid(s)
c    htype--Flag indicating which model is to be retrieved (character*3):
c           'EOS' - Equation of state
c           'CP0' - Ideal-gas heat capacity
c           'ETA' - Viscosity
c           'TCX' - Thermal conductivity
c           'TKK' - Thermal conductivity critical enhancement
c           'STN' - Surface tension
c           'DE ' - Dielectric constant
c           'MLT' - Melting line (i.e., freezing line)
c           'SBL' - Sublimation line
c           'PS ' - Vapor pressure equation
c           'DL ' - Saturated liquid density equation
c           'DV ' - Saturated vapor density equation
c
c  Outputs:
c    hcite--Component model used for property specified in htype;
c           the first 3 characters repeat the model designation of hcode
c           and the remaining are the citation for the source  (character*255)
c    hcode--Component model used for property specified in htype (character*3):
c           'FEQ' - Helmholtz energy model
c           'ECS' - Pure fluid thermo extended corresponding states
c           'ECS' - Extended corresponding states (all fluids)
c           'VS1' - The 'composite' model for R134a, R152a, NH3, etc.
c           'VS2' - Younglove-Ely model for hydrocarbons
c           'VS4' - Generalized friction theory of Quinones-Cisneros and Dieters
c           'VS5' - Chung et al. model
c           'VS6' - Vesovic form of VS1 model
c           'VS7' - Polynomial/exponential model
c           'ECS' - Extended corresponding states (all fluids)
c           'TC1' - The 'composite' model for R134a, R152a, etc.
c           'TC2' - Younglove-Ely model for hydrocarbons
c           'TC5' - Predictive model of Chung et al. (1988)
c           'ST1' - surface tension as f(tau); tau = 1 - T/Tc
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-26-00  MM, original version
c  06-22-16 EWL, shorten code

      include 'COMMONS.INC'
      character htype*3,htyp*3,hcode*3,hcite*255,UCASE*255
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GETMOD

      htyp=UCASE(htype)
      hcite=' '
      hcode=' '
      if (icomp.lt.1 .or. icomp.gt.nx) RETURN
      if (htyp.eq.'EOS') hcite=hieos(icomp) !Equation of state
      if (htyp.eq.'CP0') hcite=hicp0(icomp) !Ideal-gas heat capacity
      if (htyp.eq.'ETA') hcite=hieta(icomp) !Viscosity
      if (htyp.eq.'VIS') hcite=hieta(icomp) !Viscosity
      if (htyp.eq.'VSK') hcite=hietac(icomp)!Viscosity critical enhancement
      if (htyp.eq.'TCX') hcite=hitcx(icomp) !Thermal conductivity
      if (htyp.eq.'TKK') hcite=hitcxc(icomp)!Thermal conductivity critical enhancement
      if (htyp.eq.'STN') hcite=histn(icomp) !Surface tension
      if (htyp.eq.'DE ') hcite=hidiel(icomp)!Dielectric constant
      if (htyp.eq.'MLT') hcite=himelt(icomp)!Melting line
      if (htyp.eq.'SBL') hcite=hisubl(icomp)!Sublimation line
      if (htyp.eq.'PS ') hcite=hips(icomp)  !Vapor pressure ancillary
      if (htyp.eq.'PL ') hcite=hipl(icomp)  !Liquid pressure ancillary
      if (htyp.eq.'DL ') hcite=hidl(icomp)  !Saturated liquid density ancillary
      if (htyp.eq.'DV ') hcite=hidv(icomp)  !Saturated vapor density ancillary
      hcode=hcite(1:3)

      end                                              subroutine GETMOD
c
c ======================================================================
c
      subroutine PUREFLD (icomp)
c
c  Change the standard mixture setup so that the properties of one fluid
c  can be calculated as if SETUP had been called for a pure fluid.
c  Calling this routine will disable all mixture calculations.
c  To reset the mixture setup, call this routine with icomp=0.
c
c  Input:
c    icomp--fluid number in a mixture to use as a pure fluid
c          (set to zero to reset)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  06-13-06 EWL, original version
c  01-07-10 EWL, move most variables to subroutine RESETA
c  02-04-10 EWL, allow icomp to be set to values greater than nc, but not greater than ncmax; see subroutine SETNC

      include 'COMMONS.INC'
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PUREFLD

      call RESETA (0)
      icnc=0
      if (icomp.ge.0 .and. icomp.le.ncmax) icnc=icomp
      end                                             subroutine PUREFLD
c
c ======================================================================
c
      subroutine GERG04 (ncomp,iFlag,ierr,herr)
c
c  This is a duplicate of the GERG08 routine below, and is meant only for use
c  with older versions of Refprop.
c
      include 'COMMONS.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GERG04

      call GERG08 (ncomp,iFlag,ierr,herr)
      end                                              subroutine GERG04
c
c ======================================================================
c
      subroutine GERG08 (ncomp,iFlag,ierr,herr)
c
c  Use the GERG 2008 formulation for all pure fluid and mixture calculations.
c
c  This subroutine must be called before SETUP; it need not be called at all
c  if the default (NIST-recommended) models are desired.  To turn off
c  the GERG settings, call this routine again with iFlag=0, and then call
c  the SETUP routine to reset the parameters of the equations of state.
c  Once this routine is called, it need not be called again to keep the
c  GERG08 model active, even when calling SETUP.
c
c  Inputs:
c    ncomp--Number of components (no longer used)
c    iFlag--Set to 1 to load the GERG 2008 equations, set to 0 for defaults
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255) (returned from SETMOD)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-24-07 EWL, original version
c  07-10-13 EWL, change 'DEF' to 'NBS' when iFlag=0
c  04-04-16 EWL, copy the GERG04 routine and label it GERG08 here

      include 'COMMONS.INC'
      character*255 herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GERG08

      ierr=ncomp   !Keep the compiler happy since ncomp is no longer used.
      ierr=0
      herr=' '
      ig=iGERG08

c The code below defines the use of the iGERG08 variable.  For internal use only: Use the
c  logical flag lGERG08 to determine if the GERG model is active or not.

c  Turn GERG off
      if (iFlag.le.0) then
        if     (iGERG08.eq.2) then
          iGERG08=-1         !Set to -1 to indicate that the GERG is on, but will be turned off on the next call to SETUP.
        elseif (iGERG08.eq.1) then
          iGERG08=-1         !Set to -1 to indicate that the GERG was waiting to be turned on, but this call resets that request.
        elseif (iGERG08.eq.-1) then
                             !Do nothing, the code is waiting for SETUP to be called to deactivate the GERG model.
        endif

c  Turn GERG on
      else
        if     (iGERG08.ge.1) then
                             !Do nothing, the code is either waiting for SETUP to be called to activate the GERG model or it is currently active.
        elseif (iGERG08.eq.0) then
          iGERG08=1          !Set to 1 to indicate that the GERG will be turned on during the next call to SETUP.
        elseif (iGERG08.eq.-1) then
          iGERG08=1          !Set to 1 to indicate that the GERG was waiting to be turned off, but this call resets that request.
        endif

      endif

      if (iGERG08.eq.-1 .or. iGERG08.eq.1) iReset=1  !Turn on the iReset flag if the GERG08 equation will be turned on or off on the next call to SETUP.

c  Check if AGA8 is active.
      if (lAGA8 .and. (iGERG08.eq.-1 .or. iGERG08.eq.1)) then
        call ERRNUM (809,0,'GERG08',' ',0d0,0d0,0d0,ierr,herr)
        iGERG08=ig
      endif
      end                                              subroutine GERG08
c
c ======================================================================
c
      subroutine SETREFDIR (hpth)
c
c  Set a path to the location of original fluid files so that a user
c  can specify where their fluid file is located, but the reference
c  files needed for transport properties, etc., such as nitrogen.fld,
c  can be found.
c
c  Input:
c     hpth--Location of the fluid files (character*255)
c           The path does not need to contain the ending "\".
c           For example:  hpth='C:\Program Files\Refprop\fluids'
c
c  written by Tobias Loew, tobi@die-loews.de, to allow fluids in arbitrary directories.
c  02-10-18  TL, original version

      include 'COMMONS.INC'
      character hpth*(*),REMOVENUL*255
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETREFDIR

      hrefdir=hpth
      hrefdir=REMOVENUL(hrefdir)
      if (hrefdir.ne.' ') then
        call FINDLAST (hrefdir,' ',-255,kPTH)
        if (kPTH.ne.0 .and. kPTH.ne.255) then
          if (hrefdir(kPTH:kPTH).ne.CHAR(92) .and.
     &        hrefdir(kPTH:kPTH).ne.CHAR(58) .and.
     &        hrefdir(kPTH:kPTH).ne.CHAR(47)) then
              hrefdir(kPTH+1:kPTH+1)=CHAR(92)
          endif
        endif
      endif

      end                                           subroutine SETREFDIR
c
c ======================================================================
c
      subroutine GETREFDIR (hpth)
c
c  Get the path where the original fluid files are located.  See
c  SETREFDIR for more information.
c
c  Output:
c     hpth--Location of the original fluid files (character*255)
c
c  written by Tobias Loew, tobi@die-loews.de
c  02-10-18  TL, original version

      include 'COMMONS.INC'
      character hpth*(*)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GETREFDIR
      hpth=hrefdir
      end                                           subroutine GETREFDIR
c
c ======================================================================
c
      subroutine RFFILE (hfilei,hflref)
c
c  Provide a full path specification for the fluid file for the ECS
c  reference fluid, e.g., if 'C:\REFPROP\fluids\r123.fld' is the input,
c  and the reference fluid pointer stored in the file is 'r134a.fld',
c  then return 'C:\REFPROP\fluids\r134a.fld' as the output.
c
c  This routine assumes that the reference fluid file is in the same
c  directory as the component file(s).
c
c  Inputs:
c   hfilei--File name specifying the pure fluid or mixture component (character*255)
c   hflref--File name specifying the reference fluid (character*255)
c
c  Output:
c   hflref--Modified value of the input sent in hflref with the directory
c           information from hfilei.
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  12-14-95  MM, original version
c  02-22-07 EWL, check for null character in input string
c  02-10-18  TL, allow different reference fluid directory
c
      include 'COMMONS.INC'
      character*255 hfilei,hflref,UCASE,REMOVENUL

c  Special case if component data is read from block data.
      if (UCASE(hfilei(1:5)).eq.'BDATA') RETURN
      if (INDEX(hflref,CHAR(92)).ne.0 .or.
     &    INDEX(hflref,CHAR(58)).ne.0 .or.
     &    INDEX(hflref,CHAR(47)).ne.0) RETURN

c Added by T. Loew to allow an input fluid and a reference fluid to be
c  located in different directories.
      if (hrefdir.ne.' ') then
        call FINDLAST (hrefdir,' ',-255,kPTH)
        hflref=hrefdir(1:kPTH)//hflref
        RETURN
      endif

c  Search for directory delimiters ('\', ':', '/'  for DOS, Macintosh, and Unix, respectively).
      hfilei=REMOVENUL(hfilei)
      i=INDEX(hfilei,CHAR(92),.true.)
      if (i.eq.0) i=INDEX(hfilei,'/',.true.)
      if (i.eq.0) i=INDEX(hfilei,':',.true.)
      if (i.gt.0) hflref=hfilei(1:i)//hflref     !A file path was provided in the specification of the component *.fld file.  Attach this to the reference *.fld file name.

      end                                              subroutine RFFILE
c
c ======================================================================
c
      subroutine OPENFL (icomp,nread,hflIn,hflOut,idir,ierr,herr)
c
c  Open a file for input.
c
c  Inputs:
c    icomp--Component number.
c    nread--Unit number
c    hflIn--File name (character*255)
c     idir--File is a '.fld' file if set to 1
c           file is a '.mix' file if set to 2
c
c  Outputs:
c   hflOut--File name read in
c     ierr--Error flag:  0 - Successful
c                      101 - Error in opening file
c     herr--Error string (character*255)
c
c  The default location for the file is either the current directory
c    or the directory specified by SETPATH if it was called.  If not
c    found, then look in the Fluids subdirectory.  If still not found,
c    use the delimiters "/" and ":" to search in the same directories
c    on Unix and Mac machines.
c
c  If the file is still not found, the following sequence of locations
c    is searched to attempt to find the file
c      1 - Current directory
c      2 - Fluids\*.fld
c      3 - RPprefix\Fluids\*.fld     (where RPprefix is the string contained in the Windows's environment variable)
c      4 - \Refprop\Fluids\*.fld
c      5 - C:\Program files\Refprop\Fluids\*.fld
c      6 - C:\Program files (x86)\Refprop\Fluids\*.fld
c    ("Fluids" is replaced with "Mixtures" if the input variable idir is equal to 2.)
c
c  Although #1 and #2 appear to be the same as the initial search, they differ
c    in that the path is not used in the attempt to find the files.
c
c  If these fail, the last backslash character in options 1-8 above is replaced
c    first with the slash character and then with a colon to search on Unix
c    and Mac machines.
c
c  For case sensitive machines, the following upper and lower case scenarios
c    are tried for the fluid name.
c      All upper case.
c      All lower case.
c      File extension as upper case.
c      File extension as lower case.
c    (The case of the path is not changed when it is added before the file name.)
c
c  If the path is given, either in the input variable or by calling
c    SETPATH, then only search in the path and the subdirectory 'fluids'
c    in the path.  Assuming the file is found, the reference fluids are then
c    loaded, but do not have to be in the path but can be in any of the
c    directories discussed above.
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  08-10-06 EWL, original version (with code taken from old SETFLD)
c  10-17-13 EWL, add check for (x86) in directory name
c  01-15-15 IHB, add check for directory in RPprefix environment variable
c  07-11-15 EWL, rework code
c  02-28-18 EWL, rework code again...

      include 'COMMONS.INC'
      character b*1,bs*1,hbs*1,hfm(2)*8,hfmm*10
      character*255 herr,hflIn,hflOut,hfld,hInPath,hfilej,hTmp,RPprefix
      character*255 UCASE,LCASE,LTRIM,CHANGEALL,hFLDarray(150)

      ierr=0
      herr=' '
      hbs=CHAR(92) !Code character for backslash.  Use CHAR(92) everywhere to work with all compilers.
      hfld=hflIn
      iFLDarray=0

c  Check if requested file is a fluid file or a mixture file.
      im=8
      hfm(1)='fluids'
      hfm(2)='FLUIDS'
      if (idir.eq.2) then
        im=10
        hfm(1)='mixtures'
        hfm(2)='MIXTURES'
      endif
      b='?'

c  Check for ^M in filename caused by binary download to UNIX machines.
      j=INDEX(hfld,CHAR(13))
      if (j.ne.0) hfld=hfld(1:j-1)//hfld(j+1:)

c  Check the RPprefix environment variable as a potential place to get fluid
c   files.  The following two lines can be commented out if a specific compiler
c   does not recognize the code.
      if (kRPx.eq.0) then
        call GETENV ('RPprefix',RPprefix)
        RPprefix=LTRIM(RPprefix)
        call FINDLAST (RPprefix,'     ',-255,kRPx)    !Find the last occurrence of a character other than a space in the RPprefix environment variable.
        if (kRPx.ne.0) then                           !Strip the backslash from the end of the RPprefix variable.
          if (RPprefix(kRPx:kRPx).eq.hbs) kRPx=kRPx-1
        endif
        if (kRPx.eq.0) kRPx=-1    !Once done, if there is no RPprefix environment variable, set kRPX to -1 to not repeat this code.
      endif

c  Find the last occurrence of a character other than a space in the path (set
c   in the call to SETPATH).
      call FINDLAST (hpath,'     ',-255,kPTH)
      if (kPTH.gt.0) then         !Strip the slash or backslash from the end of the path.
        if (hpath(kPTH:kPTH).eq.'/' .or. hpath(kPTH:kPTH).eq.':' .or.
     &      hpath(kPTH:kPTH).eq.hbs) kPTH=kPTH-1
      endif

c  Extract the path out of the input string sent to the routine.
      hInPath=' '
      call FINDLAST (hfld,hbs,1,kIPTH)
      if (kIPTH.eq.0) call FINDLAST (hfld,'/',1,kIPTH)
      if (kIPTH.eq.0) call FINDLAST (hfld,':',1,kIPTH)
      if (kIPTH.gt.0) then
        kIPTH=kIPTH-1
        hInPath=hfld(:kIPTH)
        hfld=hfld(kIPTH+2:)       !Set hfld to the fluid file name without any path.
        if (icomp.le.nx) kPTH=0   !If path sent in input string, don't look at that sent to SETPATH.
      endif
      call FINDLAST (hfld,'      ',-255,kFLD)

c  Add ".FLD" if no extension is already given and there's no period in the last four characters of the file name.
      ii=kFLD
      if (ii.le.3) ii=4
      hTmp=UCASE(hfld(ii-3:kFLD))
      if(hTmp.ne.'.FLD' .and. hTmp.ne.'.BNC' .and.
     &   hTmp.ne.'.MIX'.and.INDEX(hTmp,'.').eq.0) hfld(kFLD+1:)='.FLD'

c  Open the file (but first add the paths, etc., to the file name).
      mm3=1
      if (hAltFluidName.ne.' ' .and. hAltFluidName.ne.hfld) mm3=2    !Check if an alternate fluid file name was found in CHKNAMES.
      if (icomp.gt.nx) mm3=1
      do m1=1,2         !Loop over fluids and FLUIDS subdirectories.
      do m2=1,3         !Loop over the various machine dependent delimiters.
      do m3=1,mm3       !Loop between the input file name and the alternate file name.

                      hfilej=hfld                !Use the input file name first.
        if (m3.eq.2)  hfilej=hAltFluidName       !Use the alternative name from CHKFLUIDS (such as n-pentane --> pentane) next.

c  Try all three machine dependent delimiters.
                      bs=hbs      !Backslash
        if (m2.eq.2)  bs='/'      !Try same options for a Unix system.
        if (m2.eq.3)  bs=':'      !Try same options for a Mac.
        hfmm=b//hfm(m1)(:im-2)//b

c  Search in the path set by the call to SETPATH.
        if (m1.eq.1) then         !Search only on the first pass through for m1.
          if (kIPTH.gt.0) then
            hflOut=hInPath(1:kIPTH)//bs//hfilej  !Add to the fluid file the directory sent to the routine.
            call OPENFL2 (nread,iFLDarray,0,ioerr,hflOut,hFLDarray)
            if (ioerr.eq.0) RETURN               !***File found, return to main routine***
          elseif (kPTH.gt.0) then
            hflOut=hpath(1:kPTH)//bs//hfilej     !Add to the fluid file the directory set by the call to SETPATH.
            call OPENFL2 (nread,iFLDarray,0,ioerr,hflOut,hFLDarray)
            if (ioerr.eq.0) RETURN
          endif
        endif

c  If file is not found, look in the Fluids\ (or 'Mixtures\') directory (and first add the path if SETPATH was called).
        if (kIPTH.gt.0) then
          hflOut=hInPath(:kIPTH)//hfmm(:im)//hfilej
        elseif (kPTH.gt.0) then
          hflOut=hPath(:kPTH)//hfmm(:im)//hfilej !Look in hpath\Fluids directory.
        else
          hflOut=hfmm(2:im)//hfilej              !Look in the Fluids subdirectory.
        endif
        hflOut=CHANGEALL(hflOut,b,bs)
        call OPENFL2 (nread,iFLDarray,m1-1,ioerr,hflOut,hFLDarray)
        if (ioerr.eq.0) RETURN
      enddo
      enddo

c  File was not found in default directory, try other locations to find the file by
c   looping over possible paths in Windows, UNIX, and Mac machines, and trying upper/lower
c   case scenarios.
      mm2=4
      if (ihPath.eq.2) goto 500                  !File not found and flag to quit when this happens has been activated.
      if (kPTH.gt.0 .or. kIPTH.gt.0) mm2=3       !File not found and a path was specified (i.e., don't look in other directories).
      if (icomp.gt.nx) mm2=4                     !Search everywhere for reference fluids.
      ik=6
      if (ihPath.eq.1 .or. kIPTH.gt.0 .or. kPTH.gt.0) ik=2 !If a path exists, don't check all the extra directories below.
      if (icomp.gt.nx) ik=6
      do m2=1,mm2  !Loop over the various machine dependent delimiters
      do m3=1,mm3  !Loop between the input file name and the alternate file name
                     bs=hbs
        if (m2.eq.2) bs='/'       !Try same options for a Unix system.
        if (m2.eq.3) bs=':'       !Try same options for a Mac.
        do k=1,ik
          if (m2.ne.4) then
                      hflOut=' '                                          !Current directory
            if(k.eq.2)hflOut=                                 hfmm(2:im)  !Fluids subdirectory
            if(k.eq.3.and.kRPx.gt.0)hflOut=  RPprefix(1:kRPx)//hfmm(:im)  !Use the RPprefix environmental variable appended with "Fluids\".
            if(k.eq.4)hflOut=                      '?Refprop'//hfmm(:im)  !\Refprop\Fluids\
            if(k.eq.5)hflOut=      'c:?Program Files?Refprop'//hfmm(:im)  !C:\Program files\Refprop\Fluids
            if(k.eq.6)hflOut='c:?Program Files (x86)?Refprop'//hfmm(:im)  !C:\Program files (x86)\Refprop\Fluids
            if (ik.le.2) then
              if (kPTH.gt.0) hflOut=hpath(1:kPTH)//b//hflOut
              if (kIPTH.gt.0) hflOut=hInPath(1:kIPTH)//b//hflOut
            endif
            hflOut=CHANGEALL(hflOut,b,bs)

            !Try the different directories listed above.
            call FINDLAST (hflOut,' ',-255,kl)   !Find last occurrence of a character other than a space.
            hflOut(kl+1:)=hfld                   !Append fluid name after the part specified in k=1 to 7 above.
            if (m3.eq.2) hflOut(kl+1:)=hAltFluidName
            hfilej=hflOut
            call OPENFL2 (nread,iFLDarray,0,ioerr,hflOut,hFLDarray)
            if (ioerr.eq.0) RETURN               !***File found, return to main routine***
          endif

c  Find last occurrence of the delimiter so that only the part after it gets
c   upper/lower cased when k=1.  This happens only when a path is sent in the
c   character string for the fluid name (e.g., 'c:\refprop\fluids\r134a.fld').
          if (k.eq.1) call FINDLAST (hflOut,bs,1,kl)

c  Checks for case sensitive machines.
          if (m2.ne.1) then
            do kk=1,5
              i=0
              if (kk.gt.1 .or. m1.gt.1) i=1
              if (m2.eq.4 .and. kPTH.ne.0) hflOut=hpath(1:kPTH)//hflOut
              call OPENFL2 (nread,iFLDarray,i,ioerr,hflOut,hFLDarray)
              if (ioerr.eq.0) RETURN             !***File found, return to main routine***

c  Try all upper case.
              if (kk.eq.1) then
                hflOut=hfilej(1:kl)//UCASE(hfilej(kl+1:))
c  Try all lower case.
              elseif (kk.eq.2) then
                hflOut=hfilej(1:kl)//LCASE(hfilej(kl+1:),255-kl)
c  Try upper case 'FLD', 'BNC', and 'MIX'.
              elseif (kk.eq.3) then
                hflOut=hfilej
                hflOut=CHANGEALL(hflOut,'.fld','.FLD')
                hflOut=CHANGEALL(hflOut,'.bnc','.BNC')
                hflOut=CHANGEALL(hflOut,'.mix','.MIX')
c  Try lower case 'fld', 'bnc', and 'mix'.
              elseif (kk.eq.4) then
                hflOut=hfilej
                hflOut=CHANGEALL(hflOut,'.FLD','.fld')
                hflOut=CHANGEALL(hflOut,'.BNC','.bnc')
                hflOut=CHANGEALL(hflOut,'.MIX','.mix')
              endif
            enddo
          endif
        enddo
      enddo
      enddo
      enddo

c  Routine failed to find the fluid file, exit with error number.
 500  ierr=101
      hflOut=hflIn
      end                                              subroutine OPENFL
c
c ======================================================================
c
      subroutine OPENFL2 (nread,iFLDarray,iulc,ioerr,hfilei,hFLDarray)
c
c  Save the file paths+names that have been tried, but failed, and
c  don't try opening those same files again.

      include 'COMMONS.INC'
      character*255 hfilei,hFLDarray(150),CHANGEALL,hb*1

      hb=CHAR(92)
      hfilei=CHANGEALL(hfilei,hb,'/')            !Change all backslashes to forward slashes if both exist.

      if (iulc.ge.1) then                        !Check if the test is for case sensitivity.
        if (hfilei(2:3).eq.':/') RETURN          !If it starts with "c:\", etc., then the machine is not case sensitive for the current input string.
      endif

      ioerr=1
      if (INDEX(hfilei(3:),':/').gt.0) RETURN    !Not a legitimate name if c:\ is in the middle of the filename.
      if (hfilei(1:3).eq.'c::') RETURN

      do i=1,iFLDarray
        if (hfilei.eq.hFLDarray(i)) RETURN       !Exit if name has already been tried.
      enddo

      if (iFLDarray.lt.150) then                 !Save the file name.
        iFLDarray=iFLDarray+1
        hFLDarray(iFLDarray)=hfilei
      endif

      if (iDebug.eq.2) then
        open (unit=22,file='filename.dat',access='append')
        write (22,'(a)') hfilei                  !Write all file names and paths to a file called filename.dat.
        close (22)
      endif

c Changed by T. Loew: Do not use INQUIRE/OPEN sequence since it is not thread-save
c (same unit-number may be returned multiple times BEFORE being blocked by OPEN).
c Use OPEN with NEWUNIT instead (this needs slight changes to the code, since the unit
c returned by NEWUNIT is always negative).

c  Check that unit #nread is not already being used.
c      do i=1,100
c        INQUIRE (unit=nread,opened=lread)
c        if (.not.lread) EXIT
c        nread=nread+1
c      enddo

c  For use in certain threaded applications:
c     open (newunit=nread,file=hfilei,status='old',iostat=ioerr,
c    &      action='READ',err=1)

c  If your compiler reports an error message on the following lines,
c   then try removing the action='READ' piece.
      open (unit=nread,file=hfilei,status='old',iostat=ioerr,
     &      action='READ',err=1)

 1    continue
      end                                             subroutine OPENFL2
c
c ======================================================================
c
      subroutine GETID (icomp,iFlag,nread,hstar,htype,ierr)
c
c  Internal routine to read the header in each section in the fluid file.
c
      include 'COMMONS.INC'
      character*255 hstr,UCASE,LTRIM
      character h15*15,htype*3,hcn*255,htp*2,hstar*1
      k=iFlag  !iFlag not used yet, keep the compiler happy.
      do k=1,10000
        read (nread,'(a)',end=102,err=102) hstr  !Read the next line in the file.
        hstr=LTRIM(hstr)
        h15=hstr(:1)
        if (h15.ne.':') EXIT      !Look for special flags that begin with a colon, such as :DOI:
        j=INDEX(hstr,'!')         !Remove anything after the explanation mark (which are just comments).
        if (j.ne.0) hstr=hstr(:j-1)
        j=INDEX(hstr(2:),':')     !Look for the end of the special flag (the location of the second colon).
        if (j.eq.0) EXIT
        hcn=UCASE(hstr(2:j))
        hstr=LTRIM(hstr(j+2:))
        htp=htype(1:2)
        if (hstar.eq.'#' .or. htp.eq.'EC') then
          i=0
          if (htp.eq.'BW') i=1    !Extract the type of section (EOS, Viscosity, etc.)
          if (htp.eq.'FE') i=1
          if (htp.eq.'VS') i=2
          if (htp.eq.'TC') i=3
          if (htp.eq.'EC') i=4
          if (htp.eq.'ST') i=5
          if (htp.eq.'DE') i=6
          if (htp.eq.'ML') i=7
          if (htp.eq.'SB') i=8
          if (i.gt.0 .and. hcn.eq.'DOI') hDOI(icomp,i)=hstr       !The flag :DOI: was found.
          if (i.gt.0 .and. hcn.eq.'WEB') hWEB(icomp,i)=hstr       !The flag :WEB: was found.
          if (i.gt.0 .and. hcn.eq.'TRUECRITICALPOINT') THEN
            read (hstr,*) TcTrue(icomp),DcTrue(icomp)              !Equation of state critical point (where dpdd=0 and d^2pdd^2=0 at constant pressure)
          endif
        endif
      enddo
      if (h15.eq.'?') then
        do k=1,10000                                !Skip over source comments.
          read (nread,'(a)',end=102,err=102) h15    !The question mark must be in the first 15 characters.
          h15=LTRIM(h15)
          if (h15(1:1).ne.'?') EXIT                 !Exit when the question marks stop.  The ending line without a question mark is not used for anything.
        enddo
      endif
      RETURN
 102  ierr=102
      end
c
c ======================================================================
c
      subroutine RESETA (iFlag)
c
c  Reset all variables that store saved properties so that new calculations
c  are made regardless of previous inputs, including the flag
c  in SATSPLN enable spline calculations.  Do not include variables
c  such as iAnc here that should be reset only when SETUP is called.
c
c  Input:
c    iFlag--if 0, reset values calculated when SATSPLN was called
c         --if 1, reset all except those calculated from the call to SATSPLN
c         --if 2, reset all flags including those set by the call to FLAGS
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-07-10 EWL, original version
c  12-17-15 EWL, add iFlag input variable

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      include 'COMFEQ.INC'
      include 'COMANC.INC'
      include 'COMTRN.INC'

      if (ABS(iErrNum).lt.100) iErrNum=0         !Only reset if the user has not requested to always or never print error messages.

      icnc     = 0
      ieflg    = 0
      iSatFlg  = 0
      iSatFlg1 = 0
      iSatFlg2 = 0
      iTPFlg   = 0
      ignbnd2  = 0
      MixTyp   = 0
      iDlDvFlg = 0
      iUnitNumbRef=-98765    !Use to reset the checks in REFSAV.

      d72l   = 0d0
      d72v   = 0d0
      delsvm = 0d0
      delsvf = 0d0
      tausvm = 0d0
      tausvf = 0d0
      T0sav  = 0d0

      Dsvph0 = 0d0
      Tsvph0 = 0d0
      x0sav  = 0d0
      Cp0Sav = 0d0
      CpiSav = 0d0
      CptSav = 0d0
      TsvCp0 = 0d0
      PH0sav = 0d0
      PH1sav = 0d0
      PH2sav = 0d0

      Thmax   = -1d0
      Temax   = -1d0
      Tsmax   = -1d0
      Tsmin   = -1d0
      Tsmin2  = -1d0
      Dhmax   = 0d0
      Demax   = 0d0
      Dsmax   = 0d0
      Dsmin   = 0d0
      Dsmin2  = 0d0
      Hhmax   = 0d0
      Eemax   = 0d0
      Ssmax   = 0d0
      Ssmin   = 0d0
      SSmin2  = 0d0
      lfeqsav = .false.
      lSatTP  = .false.
      l2phase = .false.
      itrnflag=0
      hmixsav = ' '
      DatTmaxSpln=0d0
      DatTminSpln=0d0
      Rxgas=xRgas
      hrefdir=' '

      if (iFlag.ge.1) then
        narr=0
        ierrCrt=0
        iSpline=0
        zSpln=0d0
        crtT=0d0
        crtP=0d0
        crtD=0d0
        PmxT=0d0
        PmxP=0d0
        Pmxd=0d0
        TmxT=0d0
        TmxP=0d0
        Tmxd=0d0
      endif

c  Turn off all flags set by the call to FLAGS.
      if (iFlag.ge.2) then
        lErrSwt =.false.
        lSkipRef=.false.
        lSkipECS=.false.
        lCache1 =.true.
        lCache2 =.true.
        hRef3='DEF'
        iReset  =1
        ihPath  =0
        ignbnd  =0
        iDebug  =0
        iPX0flg =0
        iErrNum =0
        kijzero =0
        iErrPrnt=0
        iSetRef2=0
        itrnflag=0
        call PREOS (0)
      endif

      end                                              subroutine RESETA
c
c ======================================================================
c
      block data BDSET
c
c  Initialize variables in the common blocks.
c
c  Written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  01-10-96  MM, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      data nc,icnc,ihPath,iPass,iRefExit,iDebug,kijzero,iRefSv /8*0/
      data lSkipRef,lSkipECS,lRst,lErrSwt,lNan,lGERG08,lAGA8,
     &     lTest,lRcnstWag,lAGA8sv /10*.false./
      data lReset,lCache1,lCache2,lRcnst /4*.true./
      data x0sav /nx*0d0/
      data h0sav,s0sav,T0sav,P0sav /4*0d0/
      data tref,Dref /nx*298.15d0,nx*1d-3/
      data href,sref /nx*0d0,nx*0d0/
      data TrnTj,TrnDj /2*0d0/
      data ixfsav,iSetRf,iSetRef2,iErrNum,ignbnd,iFldMix /6*0/
      data iErrPrnt,iSatFlg1,iSatFlg2,iGERG08,iFitter /5*0/
      data iersav,iersav2,iersav3,ihrsav2,iUUnits(1,0) /5*0/
      data iRDbnc,iRDmxm,kRPx,iPX0Flg,iAGA8,iReset /6*0/
      data iPRold,iNumber,iSetup,iSetRef,ncPDmix /5*0/
      data hsveos,hsvvis,hsvtcx,hsvstn, hsvmix,hsvref/6*'NBS'/
      data hsveosk,hsvvisk      /nx*'NBS',nx*'NBS'/
      data hsvstnk,hsvtcxk      /nx*'NBS',nx*'NBS'/
      data heosk,hsvfld,hrefdef /nq*'NBS',nx*'NBS',nq*'NBS'/
      data hpath,hFldOld,hRef3 /2*' ','DEF'/
      data h0Ref3,s0Ref3,t0Ref3,p0Ref3 /4*300d0/
      data iUnitTag/501*0/
      data CALtoJ/4.184d0/

      end                                               block data BDSET


c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                     End file SETUP.FOR
c ======================================================================
