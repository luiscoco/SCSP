c ======================================================================
c  Begin file PASS_FTN.FOR
c
c  This file defines the DLL-callable routines that are used by
c  Visual Basic, Excel, and other applications to access the main
c  Fortran code.  It is not needed when compiling Fortran applications.
c  It is only used when compiling the DLL.  Commands specific
c  to the Lahey/Fujitsu Fortran 95 Compiler have been commented out;
c  when using this compiler, remove the comment character in the lines
c  'dll_export' to activate them.
c
c  The calling sequence is identical (except for the SETUPdll routine) to
c  the main Fortran routines, with the letters 'dll' appended to the routine
c  name.  See the Excel sample file or the Visual Basic 'SAMPLE.BAS'
c  file for usage information.
c
c ======================================================================
c
      subroutine REFPROPdll (hFld,hIn,hOut,iUnits,iMass,iFlag,a,b,z,
     &                     Output,hUnits,iUCode,x,y,x3,q,ierr,herr)
c
c  This new routine (Sept. 2017) makes possible the ability to obtain all
c  properties in the Refprop program given two inputs from the following
c  variables:  T, P, D, E, H, S, and Q.  The routine is documented in
c  the PROP_SUB.FOR file at the top.  Basically, the fluid or mixture
c  names are sent in hFLd, the inputs are sent in hIn as a string
c  (e.g., 'PH', 'TS', 'PQ'), the desired output is sent in  hOut (e.g.,
c  'Cv', 'W', 'CSAT'), a flag for the desired units is sent in
c  iUnits, a flag to specify if the compositions are mass or molar
c  is sent in iFlg, the two inputs (T, P, etc.) are sent in the variables
c  a and b, and the molar or mass composition is sent in z.  The output
c  is sent back in the Output array, a unit string is sent in hUnits,
c  and the liquid and vapor compositions for two-phase states are sent
c  back in x and y (along with the quality q).

      include 'CONSTS.INC'
      character, intent(in) :: hFld*10000, hIn*255, hOut*255
      integer, intent(in) :: iUnits, iMass, iFlag
      double precision, intent(in) :: a, b
      double precision, intent(inout) :: z(ncmax)
      double precision, intent(out) :: Output(iPropMax)
      character, intent(out) :: hUnits*255
      integer, intent(out) :: iUCode
      double precision, intent(out) :: x(ncmax), y(ncmax), x3(ncmax), q
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
      character hFld2*10000, hIn2*255, hOut2*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "REFPROPdll"::REFPROPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::REFPROPdll
c     dll_export REFPROPdll
      i1=INDEX(hFLD,CHAR(0))-1
      i2=INDEX(hIn, CHAR(0))-1
      i3=INDEX(hOut,CHAR(0))-1
      if (i1.lt.0) i1=LEN(hFld)
      if (i2.lt.0) i2=LEN(hIn)
      if (i3.lt.0) i3=LEN(hOut)
      hFld2=hFld(:i1)
      hIn2 =hIn (:i2)
      hOut2=hOut(:i3)
      call REFPROP (hFld2,hIn2,hOut2,
     &              iUnits,iMass,iFlag,a,b,z,
     &              Output,hUnits,iUCode,x,y,x3,q,ierr,herr)
      hUnits(255:255)=CHAR(0)
      herr(255:255)  =CHAR(0)
      end
c ======================================================================
      subroutine REFPROP1dll (hIn,hOut,iUnits,iMass,a,b,z,c,q,ierr,herr)
c
      include 'CONSTS.INC'
      character, intent(in) :: hIn*255, hOut*255
      integer, intent(in) :: iUnits, iMass
      double precision, intent(in) :: a, b
      double precision, intent(inout) :: z(ncmax)
      double precision, intent(out) :: c, q
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
      character hIn2*255, hOut2*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "REFPROP1dll"::REFPROP1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::REFPROP1dll
c     dll_export REFPROP1dll
      i2=INDEX(hIn, CHAR(0))-1
      i3=INDEX(hOut,CHAR(0))-1
      if (i2.lt.0) i2=LEN(hIn)
      if (i3.lt.0) i3=LEN(hOut)
      hIn2 =hIn (:i2)
      hOut2=hOut(:i3)
      call REFPROP1 (hIn2,hOut2,iUnits,iMass,a,b,z,c,q,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine REFPROP2dll (hFld,hIn,hOut,iUnits,iFlag,a,b,z,Output,q,
     &                        ierr,herr)
c
      include 'CONSTS.INC'
      character, intent(in) :: hFld*10000, hIn*255, hOut*255
      integer, intent(in) :: iUnits, iFlag
      double precision, intent(in) :: a, b
      double precision, intent(inout) :: z(ncmax)
      double precision, intent(out) :: Output(iPropMax), q
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
      character hFld2*10000, hIn2*255, hOut2*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "REFPROP2dll"::REFPROP2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::REFPROP2dll
c     dll_export REFPROP2dll
      i1=INDEX(hFld,CHAR(0))-1
      i2=INDEX(hIn, CHAR(0))-1
      i3=INDEX(hOut,CHAR(0))-1
      if (i1.lt.0) i1=LEN(hFld)
      if (i2.lt.0) i2=LEN(hIn)
      if (i3.lt.0) i3=LEN(hOut)
      hFld2=hFld(:i1)
      hIn2 =hIn (:i2)
      hOut2=hOut(:i3)
      call REFPROP2 (hFld2,hIn2,hOut2,iUnits,iFlag,a,b,z,Output,q,
     &               ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine ALLPROPSdll (hOut,iUnits,iMass,iFlag,T,D,z,
     &                        Output,hUnitsArray,iUCodeArray,ierr,herr)
c
c  This routine is similiar to ALLPROPS20dll, except that
c  200 properties can be returned.  This is useful for obtaining
c  individual component properties.  Thus, for a mixture of 20 components,
c  the routine could be called with this input:
c    hOut='Tcrit,Pcrit,Dcrit,Ttrp,Ptrp,Dtrp,Tnbp,Mm,Acf,Dip'
c  and all 200 slots would be used, with the critical temperatures in
c  slots 1 to 20, Pc in slots 21 to 40, and so on.
c
c  This routine compresses the hUnitsArray array that is described under the
c  ALLPROPS subroutine in the REFPROP.FOR file so that it can be passed
c  back as a single string.  The segments are divided by the character '|'.
c
c  The hOut and hUnitsArray strings are 10000 characters long, and the Output and
c  iUCodeArray arrays each have a length of 200.
c
      include 'CONSTS.INC'
      character, intent(in) :: hOut*10000
      integer, intent(in) :: iUnits, iMass, iFlag
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: Output(iPropMax)
c Set iPropMax=200 (which comes from the CONSTS.INC file).
      integer, intent(out) :: iUCodeArray(iPropMax), ierr
      character, intent(out) :: hUnitsArray*10000, herr*255
      character hUnits2(iPropMax)*50, hOut2*10000
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias:"ALLPROPSdll"::ALLPROPSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ALLPROPSdll
c     dll_export ALLPROPSdll
      i=INDEX(hOut,CHAR(0))-1
      if (i.lt.0) i=LEN(hOut)
      hOut2=hOut(:i)
      call ALLPROPS (hOut2,iUnits,iMass,iFlag,T,D,z,
     &               Output,hUnits2,iUCodeArray,ierr,herr)
      k=0
      hUnitsArray=' '
      do i=1,iPropMax
        do j=50,1,-1
          if (hUnits2(i)(j:j).ne.' ') EXIT
        enddo
        hUnitsArray=hUnitsArray(1:k)//hUnits2(i)(1:j)//'|'
        k=k+j+1
        if (k.gt.10000) k=10000
      enddo
      hUnitsArray(10000:10000)=CHAR(0)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine ALLPROPS0dll (iIn,iOut,iFlag,T,D,z,Output,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: iIn, iOut(ipropmax), iFlag
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: Output(ipropmax)
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT,Decorate, Alias: "ALLPROPS0dll"::ALLPROPS0dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ALLPROPS0dll
c     dll_export ALLPROPS0dll
      call ALLPROPS0 (iIn,iOut,iFlag,T,D,z,Output,ierr,herr)
      end
c ======================================================================
      subroutine ALLPROPS1dll (hOut,iUnits,T,D,z,c,ierr,herr)
      include 'CONSTS.INC'
      character, intent(in) :: hOut*255
      integer, intent(in) :: iUnits
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: c
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
      character hOut2*255
cDEC$ ATTRIBUTES DLLEXPORT,Decorate, Alias: "ALLPROPS1dll"::ALLPROPS1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ALLPROPS1dll
c     dll_export ALLPROPS1dll
      i=INDEX(hOut,CHAR(0))-1
      if (i.lt.0) i=LEN(hOut)
      hOut2=hOut(:i)
      call ALLPROPS1 (hOut2,iUnits,T,D,z,c,ierr,herr)
      end
c ======================================================================
      subroutine ALLPROPS20dll (hOut,iUnits,iMass,iFlag,T,D,z,
     &                        Output,hUnitsArray,iUCodeArray,ierr,herr)
c
c  This routine is similiar to ALLPROPSdll, except that the arrays
c  are limited to 20 properties only, thus decreased the memory passed
c  between the calling routine and the DLL.
c
c  This routine compresses the hUnitsArray array that is described in the
c  ALLPROPS subroutine in the REFPROP.FOR file so that it can be passed
c  back as a single string.  The segments are divided by the character '|'.
c
c  The hOut string is 255 characters long, the hUnitsArray string is 1000 characters
c  long, and the Output and iUCodeArray arrays each have a length of 20.
c
      include 'CONSTS.INC'
      parameter (ipmax=20)
      character, intent(in) :: hOut*255
      integer, intent(in) :: iUnits, iMass, iFlag
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: Output(ipmax)
      character, intent(out) :: hUnitsArray*1000
      integer, intent(out) :: iUCodeArray(ipmax), ierr
      character, intent(out) :: herr*255
      character hUnits2(iPropMax)*50, hOut2*255
      double precision Output2(iPropMax)
      integer iUnits2(iPropMax)
cDEC$ ATTRIBUTES DLLEXPORT,Decorate,Alias:"ALLPROPS20dll"::ALLPROPS20dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ALLPROPS20dll
c     dll_export ALLPROPS20dll
      i=INDEX(hOut,CHAR(0))-1
      if (i.lt.0) i=LEN(hOut)
      hOut2=hOut(:i)
      call ALLPROPS (hOut2,iUnits,iMass,iFlag,T,D,z,
     &               Output2,hUnits2,iUnits2,ierr,herr)
      Output(1:ipmax)=Output2(1:ipmax)
      iUCodeArray(1:ipmax)=iUnits2(1:ipmax)
      k=0
      hUnitsArray=' '
      if (iFlag.eq.1) then
        do i=1,ipmax
          do j=50,1,-1
            if (hUnits2(i)(j:j).ne.' ') EXIT
          enddo
          hUnitsArray=hUnitsArray(1:k)//hUnits2(i)(1:j)//'|'
          k=k+j+1
          if (k.gt.1000) k=1000
        enddo
      endif
      hUnitsArray(1000:1000)=CHAR(0)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine ABFLSHdll (ab,a,b,z,iFlag,T,P,D,Dl,Dv,
     &                      x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      character, intent(in) :: ab*2
      double precision, intent(in) :: a, b, z(ncmax)
      integer, intent(in) :: iFlag
      double precision, intent(out) :: T,P,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias:"ABFLSHdll"::ABFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ABFLSHdll
c     dll_export ABFLSHdll
      call ABFLSH (ab,a,b,z,iFlag,T,P,D,Dl,Dv,
     &             x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine FLAGSdll (hFlag,jFlag,kFlag,ierr,herr)
      include 'CONSTS.INC'
      character*255, intent(in) :: hFlag
      integer, intent(in) :: jFlag
      integer, intent(out) :: kFlag, ierr
      character*255, intent(out) :: herr
      character*255 hFlag2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "FLAGSdll"::FLAGSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::FLAGSdll
c     dll_export FLAGSdll
      i=INDEX(hFlag,CHAR(0))-1
      if (i.lt.0) i=LEN(hFlag)
      hFlag2=hFlag(:i)
      call FLAGS (hFlag2,jFlag,kFlag,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine GETENUMdll (iFlag,hEnum,iEnum,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: iFlag
      character*255, intent(in) :: hEnum
      integer, intent(out) :: iEnum, ierr
      character*255, intent(out) :: herr
      character*255 hEnum2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "GETENUMdll"::GETENUMdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GETENUMdll
c     dll_export GETENUMdll
      i=INDEX(hEnum,CHAR(0))-1
      if (i.lt.0) i=LEN(hEnum)
      hEnum2=hEnum(:i)
      call GETENUM (iFlag,hEnum2,iEnum,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine ABFL1dll (a,b,z,kph,ab,Dmin,Dmax,T,P,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: a, b, z(ncmax)
      integer, intent(in) :: kph
      character, intent(in) :: ab*2
      double precision, intent(in) :: Dmin, Dmax
      double precision, intent(out) :: T,P,D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "ABFL1dll"::ABFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ABFL1dll
c     dll_export ABFL1dll
      call ABFL1 (ab,a,b,z,kph,0,Dmin,Dmax,T,P,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine ABFL2dll (a,b,z,kq,ksat,ab,
     &                     Tbub,Tdew,Pbub,Pdew,Dlbub,Dvdew,ybub,xdew,
     &                     T,P,Dl,Dv,x,y,q,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: a, b, z(ncmax)
      integer, intent(in) :: kq, ksat
      character, intent(in) :: ab*2
      double precision, intent(out) :: Tbub,Tdew,Pbub,Pdew,Dlbub,
     &                                 Dvdew,ybub(ncmax),xdew(ncmax),T,
     &                                 P,Dl,Dv,x(ncmax),y(ncmax),q
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "ABFL2dll"::ABFL2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ABFL2dll
c     dll_export ABFL2dll
      call ABFL2 (ab,a,b,z,kq,ksat,
     &            Tbub,Tdew,Pbub,Pdew,Dlbub,Dvdew,ybub,xdew,
     &            T,P,Dl,Dv,x,y,q,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c
c ======================================================================
c
      subroutine RPVersion (hv)
      include 'CONSTS.INC'
      character*3 hrf
      character*255 hFiles(ncmax),hFmix,herr
      character*255, intent(out) :: hv
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "RPVersion"::RPVersion
cDEC$ ATTRIBUTES STDCALL, REFERENCE::RPVersion
c     dll_export RPVersion
      call SETUP (-1,hFiles,hFmix,hrf,ierr,herr)
      write (hv,'(f6.4)') 1d-4*ierr
      if (hv(6:7).eq.'0 ') hv(6:6)=' '
      if (hv(5:6).eq.'0 ') hv(5:5)=' '
      if (hv(4:5).eq.'0 ') hv(4:4)=' '
      end
c ======================================================================
      subroutine SETUPdll (ncomp,hFiles,hFmix,hrf,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: ncomp
      character, intent(in) :: hFiles*10000, hFmix*255, hrf*3
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
      character hFiles2*10000, hFmix2*255, hrf2*3
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETUPdll"::SETUPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETUPdll
c     dll_export SETUPdll
      i1=INDEX(hFiles,CHAR(0))-1
      i2=INDEX(hFmix, CHAR(0))-1
      i3=INDEX(hrf,   CHAR(0))-1
      if (i1.lt.0) i1=LEN(hFiles)
      if (i2.lt.0) i2=LEN(hFmix)
      if (i3.lt.0) i3=LEN(hrf)
      hFiles2=hFiles(:i1)
      hFmix2 =hFmix (:i2)
      hrf2   =hrf   (:i3)
      call SETUP0 (ncomp,hFiles2,hFmix2,hrf2,ierr,herr)
      if (ncomp.ge.0) herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SETFLUIDSdll (hFld,ierr)
      include 'CONSTS.INC'
      character, intent(in) :: hFld*10000
      integer, intent(out) :: ierr
      character*10000 hFld2
cDEC$ ATTRIBUTES DLLEXPORT,Decorate, Alias: "SETFLUIDSdll"::SETFLUIDSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETFLUIDSdll
c     dll_export SETFLUIDSdll
      i=INDEX(hFld,CHAR(0))-1
      if (i.lt.0) i=LEN(hFld)
      hFld2=hFld(:i)
      call SETFLUIDS (hFld2,ierr)
      end
c ======================================================================
      subroutine SETREFdll (hrf,ixflag,x0,h0,s0,T0,P0,ierr,herr)
      include 'CONSTS.INC'
      character, intent(in) :: hrf*3
      integer, intent(in) :: ixflag
      double precision, intent(in) :: x0(ncmax), h0, s0, T0, P0
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETREFdll"::SETREFdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETREFdll
c     dll_export SETREFdll
      call SETREF (hrf,ixflag,x0,h0,s0,T0,P0,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SETREFDIRdll (hpth)
      include 'CONSTS.INC'
      character, intent(in) :: hpth*255
      character*255 hpth2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias:"SETREFDIRdll"::SETREFDIRdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETREFDIRdll
c     dll_export SETREFDIRdll
      i=INDEX(hpth,CHAR(0))-1
      if (i.lt.0) i=LEN(hpth)
      hpth2=hpth(:i)
      call SETREFDIR (hpth2)
      end
c ======================================================================
      subroutine GETREFDIRdll (hpth)
      include 'CONSTS.INC'
      character, intent(out) :: hpth*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias:"GETREFDIRdll"::GETREFDIRdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GETREFDIRdll
c     dll_export GETREFDIRdll
      call GETREFDIR (hpth)
      hpth(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SETMIXdll (hMixNme,hFmix,hrf,ncc,hFiles,z,ierr,herr)
      include 'CONSTS.INC'
      character, intent(in) :: hMixNme*255, hFmix*255, hrf*3
      integer, intent(out) :: ncc
      character, intent(out) :: hFiles*10000
      double precision, intent(out) :: z(ncmax)
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
      character*255 hMixNme2*255, hFmix2*255, hrf2*3, hfile(ncmax)
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETMIXdll"::SETMIXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETMIXdll
c     dll_export SETMIXdll
      i1=INDEX(hMixNme,CHAR(0))-1
      i2=INDEX(hFmix,  CHAR(0))-1
      i3=INDEX(hrf,    CHAR(0))-1
      if (i1.lt.0) i1=LEN(hMixNme)
      if (i2.lt.0) i2=LEN(hFmix)
      if (i3.lt.0) i3=LEN(hrf)
      hMixNme2=hMixNme(:i1)
      hFmix2  =hFmix  (:i2)
      hrf2    =hrf    (:i3)
      call SETMIX (hMixNme2,hFmix2,hrf2,ncc,hFile,z,ierr,herr)
      hFiles=hFile(1)
      j=INDEX(hFiles,'   ')
      hFiles=hFiles(1:j-1)//'|'
      do i=2,ncc
        j=INDEX(hFiles,'   ')
        hFiles=hFiles(1:j-1)//hFile(i)
        j=INDEX(hFiles,'   ')
        hFiles=hFiles(1:j-1)//'|'
      enddo
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SETMIXTUREdll (hMixNme,z,ierr)
      include 'CONSTS.INC'
      character, intent(in) :: hMixNme*10000
      double precision, intent(in) :: z(ncmax)
      integer, intent(out) :: ierr
      character*10000 hMixNme2
cDEC$ ATTRIBUTES DLLEXPORT,Decorate,Alias:"SETMIXTUREdll"::SETMIXTUREdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETMIXTUREdll
c     dll_export SETMIXTUREdll
      i=INDEX(hMixNme,CHAR(0))-1
      if (i.lt.0) i=LEN(hMixNme)
      hMixNme2=hMixNme(:i)
      call SETMIXTURE (hMixNme2,z,ierr)
      end
c ======================================================================
      subroutine SETNCdll (ncomp)
      include 'CONSTS.INC'
      integer, intent(in) :: ncomp
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETNCdll"::SETNCdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETNCdll
c     dll_export SETNCdll
      call SETNC (ncomp)
      end
c ======================================================================
      subroutine SETPATHdll (hpth)
      include 'CONSTS.INC'
      character, intent(in) :: hpth*255
      character*255 hpth2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETPATHdll"::SETPATHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETPATHdll
c     dll_export SETPATHdll
      i=INDEX(hpth,CHAR(0))-1
      if (i.lt.0) i=LEN(hpth)
      hpth2=hpth(:i)
      call SETPATH (hpth2)
      end
c ======================================================================
      subroutine ERRMSGdll (ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "ERRMSGdll"::ERRMSGdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ERRMSGdll
c     dll_export ERRMSGdll
      call ERRMSG (ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SETMODdll (ncomp,htype,hmix,hcomp,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: ncomp
      character, intent(in) :: htype*3, hmix*3, hcomp*60
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
      character hcomp2(1:ncmax)*3
      character*255 htype2*3, hmix2*3, hcomp1*60
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETMODdll"::SETMODdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETMODdll
c     dll_export SETMODdll
      i1=INDEX(htype,CHAR(0))-1
      i2=INDEX(hmix, CHAR(0))-1
      i3=INDEX(hcomp,CHAR(0))-1
      if (i1.lt.0) i1=LEN(htype)
      if (i2.lt.0) i2=LEN(hmix)
      if (i3.lt.0) i3=LEN(hcomp)
      htype2=htype(:i1)
      hmix2 =hmix (:i2)
      hcomp1=hcomp(:i3)
      do i=1,ncmax
        hcomp2(i)=hcomp1(i*3-2:i*3)
      enddo
      call SETMOD (ncomp,htype2,hmix2,hcomp2,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine GETMODdll (icomp,htype,hcode,hcite)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      character, intent(in) :: htype*3
      character, intent(out) :: hcite*255,hcode*3
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "GETMODdll"::GETMODdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GETMODdll
c     dll_export GETMODdll
      call GETMOD (icomp,htype,hcode,hcite)
      hcite(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SETKTVdll (icomp,jcomp,hmodij,fij,hFmix,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp, jcomp
      character, intent(in) :: hmodij*3
      double precision, intent(in) :: fij(nmxpar)
      character, intent(in) :: hFmix*255
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
      character hFmix2*255,hmodij2*3
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETKTVdll"::SETKTVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETKTVdll
c     dll_export SETKTVdll
      i1=INDEX(hFmix,CHAR(0))-1
      i2=INDEX(hmodij,CHAR(0))-1
      if (i1.lt.0) i1=LEN(hFmix)
      if (i2.lt.0) i2=LEN(hmodij)
      hFmix2=hFmix(:i1)
      hmodij2=hmodij(:i2)
      call SETKTV (icomp,jcomp,hmodij2,fij,hFmix2,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine GETKTVdll (icomp,jcomp,hmodij,fij,hFmix,hfij,hbinp,
     &                      hmxrul)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp, jcomp
      character, intent(out) :: hmodij*3
      double precision, intent(out) :: fij(nmxpar)
      character*255, intent(out) :: hFmix, hfij ,hbinp, hmxrul
      character hfij2(nmxpar)*8
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "GETKTVdll"::GETKTVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GETKTVdll
c     dll_export GETKTVdll
      call GETKTV (icomp,jcomp,hmodij,fij,hFmix,hfij2,hbinp,hmxrul)
      hfij=hfij2(1)//hfij2(2)//hfij2(3)//hfij2(4)//hfij2(5)//hfij2(6)
      hFmix (255:255)=CHAR(0)
      hfij  (255:255)=CHAR(0)
      hbinp (255:255)=CHAR(0)
      hmxrul(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine GETFIJdll (hmodij,fij,hfij,hmxrul)
      include 'CONSTS.INC'
      character, intent(in) :: hmodij*3
      double precision, intent(out) :: fij(nmxpar)
      character, intent(out) :: hfij*255,hmxrul*255

      character hfij2(nmxpar)*8
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "GETFIJdll"::GETFIJdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GETFIJdll
c     dll_export GETFIJdll
      call GETFIJ (hmodij,fij,hfij2,hmxrul)
      hfij=hfij2(1)//hfij2(2)//hfij2(3)//hfij2(4)//hfij2(5)//hfij2(6)
      hmxrul(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SETAGAdll (ierr,herr)
      include 'CONSTS.INC'
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SETAGAdll"::SETAGAdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SETAGAdll
c     dll_export SETAGAdll
      call SETAGA (ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine UNSETAGAdll
      include 'COMMONS.INC'
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "UNSETAGAdll"::UNSETAGAdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::UNSETAGAdll
c  dll_export UNSETAGAdll
      iAGA8=-1
      call UNSETAGA
      end
c ======================================================================
      subroutine PUREFLDdll (icomp)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PUREFLDdll"::PUREFLDdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PUREFLDdll
c     dll_export PUREFLDdll
      call PUREFLD (icomp)
      end
c ======================================================================
      subroutine GERG04dll (ncomp,iFlag,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: ncomp, iFlag
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "GERG04dll"::GERG04dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GERG04dll
c     dll_export GERG04dll
      call GERG04 (ncomp,iFlag,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine GERG08dll (ncomp,iFlag,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: ncomp, iFlag
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "GERG08dll"::GERG08dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GERG08dll
c     dll_export GERG08dll
      call GERG08 (ncomp,iFlag,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PREOSdll (i)
      include 'CONSTS.INC'
      integer, intent(in) :: i
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PREOSdll"::PREOSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PREOSdll
c     dll_export PREOSdll
      call PREOS (i)
      end
c ======================================================================
      subroutine HMXORDERdll (i,j,hcasi,hcasj,iFlag,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: i, j
      character*255, intent(out) :: hcasi,hcasj
      integer, intent(out) :: iFlag, ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "HMXORDERdll"::HMXORDERdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::HMXORDERdll
c     dll_export HMXORDERdll
      call HMXORDER (i,j,hcasi,hcasj,iFlag,ierr,herr)
      hcasi(255:255)=CHAR(0)
      hcasj(255:255)=CHAR(0)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine REDXdll (z,Tred,Dred)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: Tred, Dred
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "REDXdll"::REDXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::REDXdll
c     dll_export REDXdll
      call REDX (z,Tred,Dred)
      end
c ======================================================================
      subroutine RDXHMXdll (ix,icmp,icmp2,z,Tred,Dred,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: ix, icmp, icmp2
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: Tred, Dred
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "RDXHMXdll"::RDXHMXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::RDXHMXdll
c     dll_export RDXHMXdll
      call RDXHMX (ix,icmp,icmp2,z,Tred,Dred,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine RMIX2dll (z,Rgas)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: Rgas
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "RMIX2dll"::RMIX2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::RMIX2dll
c     dll_export RMIX2dll
      call RMIX2 (z,Rgas)
      end
c ======================================================================
      subroutine INFOdll (icomp,wmm,Ttrp,Tnbpt,Tc,Pc,Dc,Zc,acf,dip,Rgas)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(out) :: wmm,Ttrp,Tnbpt,Tc,Pc
      double precision, intent(out) :: Dc,Zc,acf,dip,Rgas
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "INFOdll"::INFOdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::INFOdll
c     dll_export INFOdll
      call INFO (icomp,wmm,Ttrp,Tnbpt,Tc,Pc,Dc,Zc,acf,dip,Rgas)
      end
c ======================================================================
      subroutine NAMEdll (icomp,hnam,hn80,hcasn)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      character, intent(out) :: hnam*12,hn80*80,hcasn*12
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "NAMEdll"::NAMEdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::NAMEdll
c     dll_export NAMEdll
      call NAME (icomp,hnam,hn80,hcasn)
      if (hnam(12:12).eq.' ') hnam(12:12)=CHAR(0)
      if (hcasn(12:12).eq.' ') hcasn(12:12)=CHAR(0)
      hn80(80:80)=CHAR(0)
      end
c ======================================================================
      subroutine XMASSdll (xmol,xkg,wmix)
      include 'CONSTS.INC'
      double precision, intent(in) :: xmol(ncmax)
      double precision, intent(out) :: xkg(ncmax), wmix
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "XMASSdll"::XMASSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::XMASSdll
c     dll_export XMASSdll
      call XMASS (xmol,xkg,wmix)
      end
c ======================================================================
      subroutine XMOLEdll (xkg,xmol,wmix)
      include 'CONSTS.INC'
      double precision, intent(in) :: xkg(ncmax)
      double precision, intent(out) :: xmol(ncmax), wmix
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "XMOLEdll"::XMOLEdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::XMOLEdll
c     dll_export XMOLEdll
      call XMOLE (xkg,xmol,wmix)
      end
c ======================================================================
      subroutine QMASSdll (qmol,xl,xv,qkg,xlkg,xvkg,wliq,wvap,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: qmol,xl(ncmax),xv(ncmax)
      double precision, intent(out) :: qkg,xlkg(ncmax),xvkg(ncmax),wliq,
     & wvap
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "QMASSdll"::QMASSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::QMASSdll
c     dll_export QMASSdll
      call QMASS (qmol,xl,xv,qkg,xlkg,xvkg,wliq,wvap,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine QMOLEdll (qkg,xlkg,xvkg,qmol,xl,xv,wliq,wvap,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: qkg,xlkg(ncmax),xvkg(ncmax)
      double precision, intent(out) :: qmol,xl(ncmax),xv(ncmax),
     & wliq,wvap
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "QMOLEdll"::QMOLEdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::QMOLEdll
c     dll_export QMOLEdll
      call QMOLE (qkg,xlkg,xvkg,qmol,xl,xv,wliq,wvap,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine WMOLdll (z,wmm)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: wmm
      double precision WMOL
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "WMOLdll"::WMOLdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::WMOLdll
c     dll_export WMOLdll
      wmm=WMOL(z)
      end
c ======================================================================
      subroutine WMOLIdll (icomp,wmm)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(out) :: wmm
      double precision WMOLI
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "WMOLIdll"::WMOLIdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::WMOLIdll
c     dll_export WMOLIdll
      wmm=WMOLI(icomp)
      end
c ======================================================================
      subroutine LIMITXdll (htyp,T,D,P,z,Tmin,Tmax,Dmax,Pmax,ierr,herr)
      include 'CONSTS.INC'
      character, intent(in) :: htyp*3
      double precision, intent(in) :: T,D,P,z(ncmax)
      double precision, intent(out) :: Tmin,Tmax,Dmax,Pmax
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "LIMITXdll"::LIMITXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::LIMITXdll
c     dll_export LIMITXdll
      call LIMITX (htyp,T,D,P,z,Tmin,Tmax,Dmax,Pmax,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine LIMITKdll (htyp,icomp,T,D,P,Tmin,Tmax,Dmax,Pmax,
     &                      ierr,herr)
      include 'CONSTS.INC'
      character, intent(in) :: htyp*3
      integer, intent(in) :: icomp
      double precision, intent(in) :: T,D,P
      double precision, intent(out) :: Tmin,Tmax,Dmax,Pmax
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "LIMITKdll"::LIMITKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::LIMITKdll
c     dll_export LIMITKdll
      call LIMITK (htyp,icomp,T,D,P,Tmin,Tmax,Dmax,Pmax,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine LIMITSdll (htyp,z,Tmin,Tmax,Dmax,Pmax)
      include 'CONSTS.INC'
      character, intent(in) :: htyp*3
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: Tmin,Tmax,Dmax,Pmax
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "LIMITSdll"::LIMITSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::LIMITSdll
c     dll_export LIMITSdll
      call LIMITS (htyp,z,Tmin,Tmax,Dmax,Pmax)
      end
c ======================================================================
      subroutine CRTPNTdll (z,Tc,Pc,Dc,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      double precision, intent(inout) :: Tc, Pc
      double precision, intent(out) :: Dc
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CRTPNTdll"::CRTPNTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CRTPNTdll
c     dll_export CRTPNTdll
      call CRTPNT (z,Tc,Pc,Dc,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine CRITPdll (z,Tc,Pc,Dc,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: Tc, Pc, Dc
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CRITPdll"::CRITPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CRITPdll
c     dll_export CRITPdll
      call CRITP (z,Tc,Pc,Dc,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine MAXTdll (z,Tm,Pm,Dm,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: Tm, Pm, Dm
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "MAXTdll"::MAXTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::MAXTdll
c     dll_export MAXTdll
      call MAXT (z,Tm,Pm,Dm,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine MAXPdll (z,Tm,Pm,Dm,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      double precision, intent(out) :: Tm, Pm, Dm
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "MAXPdll"::MAXPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::MAXPdll
c     dll_export MAXPdll
      call MAXP (z,Tm,Pm,Dm,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PHIXdll (itau,idel,tau,delta,z,phixx)
      include 'CONSTS.INC'
      integer, intent(in) :: itau, idel
      double precision, intent(in) :: tau, delta, z(ncmax)
      double precision, intent(out) :: phixx
      double precision PHIX
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHIXdll"::PHIXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHIXdll
c     dll_export PHIXdll
      phixx=PHIX (itau,idel,tau,delta,z)
      end
c ======================================================================
      subroutine PHI0dll (itau,idel,T,D,z,phi00)
      include 'CONSTS.INC'
      integer, intent(in) :: itau, idel
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: phi00
      double precision PHI0
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHI0dll"::PHI0dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHI0dll
c     dll_export PHI0dll
      phi00=PHI0 (itau,idel,T,D,z)
      end
c ======================================================================
      subroutine PRESSdll (T,D,z,P)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: P
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PRESSdll"::PRESSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PRESSdll
c     dll_export PRESSdll
      call PRESS (T,D,z,P)
      end
c ======================================================================
      subroutine THERMdll (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: P,e,h,s,Cv,Cp,w,hjt
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "THERMdll"::THERMdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::THERMdll
c     dll_export THERMdll
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      end
c ======================================================================
      subroutine THERM0dll (T,D,z,P0,e0,h0,s0,Cv0,Cp00,w0,a0,g0)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: P0,e0,h0,s0,Cv0,Cp00,w0,a0,g0
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "THERM0dll"::THERM0dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::THERM0dll
c     dll_export THERM0dll
      call THERM0 (T,D,z,P0,e0,h0,s0,Cv0,Cp00,w0,a0,g0)
      end
c ======================================================================
      subroutine THERM2dll (T,D,z,P,e,h,s,Cv,Cp,w,zz,hjt,a,g,xkappa,
     &                   beta,dPdD,d2PdD2,dPdT,dDdT,dDdP,
     &                   d2PdT2,d2PdTD,spare3,spare4)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: P,e,h,s,Cv,Cp,w,zz,hjt,a,g,
     &                   xkappa,beta,dPdD,d2PdD2,dPdT,dDdT,dDdP,
     &                   d2PdT2,d2PdTD,spare3,spare4

cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "THERM2dll"::THERM2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::THERM2dll
c     dll_export THERM2dll
      call THERM2 (T,D,z,P,e,h,s,Cv,Cp,w,zz,hjt,a,g,xkappa,beta,
     &             dPdD,d2PdD2,dPdT,dDdT,dDdP,
     &             d2PdT2,d2PdTD,spare3,spare4)
      end
c ======================================================================
      subroutine THERM3dll (T,D,z,
     &           xkappa,beta,xisenk,xkt,betas,bs,xkkt,thrott,pi,spht)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: xkappa,beta,xisenk,xkt,betas,bs,
     & xkkt,thrott,pi,spht
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "THERM3dll"::THERM3dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::THERM3dll
c     dll_export THERM3dll
      call THERM3 (T,D,z,
     &           xkappa,beta,xisenk,xkt,betas,bs,xkkt,thrott,pi,spht)
      end
c ======================================================================
      subroutine ENTROdll (T,D,z,s)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: s
      double precision P,e,h,Cv,Cp,w,hjt
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "ENTROdll"::ENTROdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ENTROdll
c     dll_export ENTROdll
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      end
c ======================================================================
      subroutine ENTHALdll (T,D,z,h)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: h
      double precision P,e,s,Cv,Cp,w,hjt
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "ENTHALdll"::ENTHALdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ENTHALdll
c     dll_export ENTHALdll
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      end
c ======================================================================
      subroutine CVCPdll (T,D,z,Cv,Cp)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: Cv,Cp
      double precision P,e,h,s,w,hjt
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CVCPdll"::CVCPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CVCPdll
c     dll_export CVCPdll
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      end
c ======================================================================
      subroutine CVCPKdll (icomp,T,D,Cv,Cp)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T,D
      double precision, intent(out) :: Cv,Cp
      double precision P,e,h,s,w,hjt
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CVCPKdll"::CVCPKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CVCPKdll
c     dll_export CVCPKdll
      double precision z(ncmax)
      z=0d0
      z(icomp)=1d0
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      end
c ======================================================================
      subroutine CP0dll (T,z,Cp)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: Cp
      double precision D,P,e,h,s,Cv,w,hjt
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CP0dll"::CP0dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CP0dll
c     dll_export CP0dll
      D=0d0
      call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
      end
c ======================================================================
      subroutine RESIDUALdll (T,D,z,Pr,er,hr,sr,Cvr,Cpr,ar,gr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: Pr,er,hr,sr,Cvr,Cpr,ar,gr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "RESIDUALdll"::RESIDUALdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::RESIDUALdll
c     dll_export RESIDUALdll
      call RESIDUAL (T,D,z,Pr,er,hr,sr,Cvr,Cpr,ar,gr)
      end
c ======================================================================
      subroutine GIBBSdll (T,D,z,ar,gr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: ar,gr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "GIBBSdll"::GIBBSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::GIBBSdll
c     dll_export GIBBSdll
      call GIBBS (T,D,z,ar,gr)
      end
c ======================================================================
      subroutine AGdll (T,D,z,a,g)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: a,g
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "AGdll"::AGdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::AGdll
c     dll_export AGdll
      call AG (T,D,z,a,g)
      end
c ======================================================================
      subroutine DPDDdll (T,D,z,dPdD)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dPdD
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DPDDdll"::DPDDdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DPDDdll
c     dll_export DPDDdll
      call DPDDX (T,D,z,dPdD)
      end
c ======================================================================
      subroutine DPDD2dll (T,D,z,d2PdD2)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: d2PdD2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DPDD2dll"::DPDD2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DPDD2dll
c     dll_export DPDD2dll
      call DPDD2 (T,D,z,d2PdD2)
      end
c ======================================================================
      subroutine DPDTdll (T,D,z,dPdT)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dPdT
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DPDTdll"::DPDTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DPDTdll
c     dll_export DPDTdll
      call DPDTX (T,D,z,dPdT)
      end
c ======================================================================
      subroutine DDDPdll (T,D,z,dDdP)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dDdP
      double precision
     &              dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                   dDdT,d2DdP2,d2DdT2,d2DdPT,
     &              dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DDDPdll"::DDDPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DDDPdll
c     dll_export DDDPdll
      call DERVPVT (T,D,z,
     &              dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &              dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &              dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
      end
c ======================================================================
      subroutine DDDTdll (T,D,z,dDdT)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dDdT
      double precision
     &              dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &              dDdP,     d2DdP2,d2DdT2,d2DdPT,
     &              dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DDDTdll"::DDDTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DDDTdll
c     dll_export DDDTdll
      call DERVPVT (T,D,z,
     &              dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &              dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &              dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
      end
c ======================================================================
      subroutine DERVPVTdll (T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                             dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                             dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                             dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                             dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DERVPVTdll"::DERVPVTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DERVPVTdll
c     dll_export DERVPVTdll
      call DERVPVT (T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                    dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                    dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
      end
c ======================================================================
      subroutine PHIDERVdll (iderv,T,D,z,dadn,dnadn,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: iderv
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dadn(ncmax),dnadn(ncmax)
      integer, intent(out) :: ierr
      character, intent(out) :: herr*255
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHIDERVdll"::PHIDERVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHIDERVdll
c     dll_export PHIDERVdll
      call PHIDERV (iderv,T,D,z,dadn,dnadn,ierr,herr)
      end
c ======================================================================
      subroutine DHD1dll (T,D,z,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,
     &                    dhdp_d)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dhdt_d,dhdt_p,dhdd_t,dhdd_p,
     & dhdp_t, dhdp_d
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DHD1dll"::DHD1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DHD1dll
c     dll_export DHD1dll
      call DHD1 (T,D,z,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,dhdp_d)
      end
c ======================================================================
      subroutine DSD1dll (T,D,z,dsdt_d,dsdt_p,dsdd_t,dsdd_p,dsdp_t,
     &                    dsdp_d)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: dsdt_d,dsdt_p,dsdd_t,dsdd_p,
     &                    dsdp_t,dsdp_d
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DSD1dll"::DSD1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DSD1dll
c     dll_export DSD1dll
      call DSD1 (T,D,z,dsdt_d,dsdt_p,dsdd_t,dsdd_p,dsdp_t,dsdp_d)
      end
c ======================================================================
      subroutine RIEMdll (T,D,z,riemc)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,D,z(ncmax)
      double precision, intent(out) :: riemc
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "RIEMdll"::RIEMdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::RIEMdll
c     dll_export RIEMdll
      call RIEM (T,D,z,riemc)
      end
c ======================================================================
      subroutine VIRBCDdll (T,z,B,C,D,E)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: B,C,D,E
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "VIRBCDdll"::VIRBCDdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::VIRBCDdll
c     dll_export VIRBCDdll
      call VIRBCD (T,z,B,C,D,E)
      end
c ======================================================================
      subroutine VIRBdll (T,z,B)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: B
      double precision C,D,E
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "VIRBdll"::VIRBdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::VIRBdll
c     dll_export VIRBdll
      call VIRBCD (T,z,B,C,D,E)
      end
c ======================================================================
      subroutine DBDTdll (T,z,dBT)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: dBT
      double precision B,C,D,E,Bx2,Cx1,Cx2,Dx1,Dx2,Ba,Ca
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DBDTdll"::DBDTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DBDTdll
c     dll_export DBDTdll
      call VIRBCD12 (T,z,B,C,D,E,dBT,Bx2,Cx1,Cx2,Dx1,Dx2,Ba,Ca)
      end
c ======================================================================
      subroutine VIRCdll (T,z,C)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: C
      double precision B,D,E
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "VIRCdll"::VIRCdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::VIRCdll
c     dll_export VIRCdll
      call VIRBCD (T,z,B,C,D,E)
      end
c ======================================================================
      subroutine VIRBAdll (T,z,Ba)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: Ba
      double precision B,C,D,E,Bx1,Bx2,Cx1,Cx2,Dx1,Dx2,Ca
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "VIRBAdll"::VIRBAdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::VIRBAdll
c     dll_export VIRBAdll
      call VIRBCD12 (T,z,B,C,D,E,Bx1,Bx2,Cx1,Cx2,Dx1,Dx2,Ba,Ca)
      end
c ======================================================================
      subroutine VIRCAdll (T,z,Ca)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: Ca
      double precision B,C,D,E,Bx1,Bx2,Cx1,Cx2,Dx1,Dx2,Ba
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "VIRCAdll"::VIRCAdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::VIRCAdll
c     dll_export VIRCAdll
      call VIRBCD12 (T,z,B,C,D,E,Bx1,Bx2,Cx1,Cx2,Dx1,Dx2,Ba,Ca)
      end
c ======================================================================
      subroutine B12dll (T,z,B)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,z(ncmax)
      double precision, intent(out) :: B
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "B12dll"::B12dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::B12dll
c     dll_export B12dll
      call B12 (T,z,B)
      end
c ======================================================================
      subroutine EXCESSdll (T,P,z,kph,D,vE,eE,hE,sE,aE,gE,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T,P,z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(inout) :: D
      double precision, intent(out) :: vE,eE,hE,sE,aE,gE
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "EXCESSdll"::EXCESSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::EXCESSdll
c     dll_export EXCESSdll
      call EXCESS (T,P,z,kph,D,vE,eE,hE,sE,aE,gE,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine CSATKdll (icomp,T,kph,P,D,Csat,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T
      integer, intent(in) :: kph
      double precision, intent(out) :: P,D,Csat
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CSATKdll"::CSATKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CSATKdll
c     dll_export CSATKdll
      call CSATK (icomp,T,kph,P,D,Csat,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DPTSATKdll (icomp,T,kph,P,D,Csat,dPdT,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T
      integer, intent(in) :: kph
      double precision, intent(out) :: P,D,Csat,dPdT
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DPTSATKdll"::DPTSATKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DPTSATKdll
c     dll_export DPTSATKdll
      call DPTSATK (icomp,T,kph,P,D,Csat,dPdT,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine CV2PKdll (icomp,T,D,Cv2p,Csat,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T, D
      double precision, intent(out) :: Cv2p,Csat
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CV2PKdll"::CV2PKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CV2PKdll
c     dll_export CV2PKdll
      call CV2PK (icomp,T,D,Cv2p,Csat,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine CSTARdll (T,P,v,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, P, v, z(ncmax)
      double precision, intent(out) :: Cs,Ts,Ds,Ps,ws
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CSTARdll"::CSTARdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CSTARdll
c     dll_export CSTARdll
      call CSTAR (T,P,v,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine CCRITdll (T,P,v,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, P, v, z(ncmax)
      double precision, intent(out) :: Cs,Ts,Ds,Ps,ws
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CCRITdll"::CCRITdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CCRITdll
c     dll_export CCRITdll
      call CSTAR (T,P,v,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine FGCTYdll (T,D,z,f)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: f(ncmax)
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "FGCTYdll"::FGCTYdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::FGCTYdll
c     dll_export FGCTYdll
      call FGCTY (T,D,z,f)
      end
c ======================================================================
      subroutine FGCTY2dll (T,D,z,f,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: f(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "FGCTY2dll"::FGCTY2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::FGCTY2dll
c     dll_export FGCTY2dll
      call FGCTY2 (T,D,z,f,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine FUGCOFdll (T,D,z,phi,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: phi(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "FUGCOFdll"::FUGCOFdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::FUGCOFdll
c     dll_export FUGCOFdll
      call FUGCOF (T,D,z,phi,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine CHEMPOTdll (T,D,z,u,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: u(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "CHEMPOTdll"::CHEMPOTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::CHEMPOTdll
c     dll_export CHEMPOTdll
      call CHEMPOT (T,D,z,u,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine HEATdll (T,D,z,hg,hn,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: hg,hn
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "HEATdll"::HEATdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::HEATdll
c     dll_export HEATdll
      call HEAT (T,D,z,hg,hn,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine HEATFRMdll (T,D,z,hFrm,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: hFrm
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "HEATFRMdll"::HEATFRMdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::HEATFRMdll
c     dll_export HEATFRMdll
      call HEATFRM (T,D,z,hFrm,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine MASSFLUXdll (Tm,P,z,beta,rf,fluxm,Cs,T0,P0,xMach,u,
     &                        Ts,Ps,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: Tm, P, z(ncmax), beta, rf
      double precision, intent(out) :: fluxm, Cs, T0, P0, xMach,
     &                                 u, Ts, Ps
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "MASSFLUXdll"::MASSFLUXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::MASSFLUXdll
c     dll_export MASSFLUXdll
      call MASSFLUX (Tm,P,z,beta,rf,fluxm,Cs,T0,P0,xMach,u,Ts,Ps,
     &               ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine FPVdll (T,D,P,z,Fpvx)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, P, z(ncmax)
      double precision, intent(out) :: Fpvx
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "FPVdll"::FPVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::FPVdll
c     dll_export FPVdll
      call FPV (T,D,P,z,Fpvx)
      end
c ======================================================================
      subroutine TRNPRPdll (T,D,z,eta,tcx,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: eta, tcx
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TRNPRPdll"::TRNPRPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TRNPRPdll
c     dll_export TRNPRPdll
      call TRNPRP (T,D,z,eta,tcx,ierr,herr)
      if (tcx.gt.1d50) tcx=0                !Avoid NaN (not a number)
      if (eta.gt.1d50) eta=0                !Avoid NaN (not a number)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DIELECdll (T,D,z,de)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: de
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DIELECdll"::DIELECdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DIELECdll
c     dll_export DIELECdll
      call DIELEC (T,D,z,de)
      end
c ======================================================================
      subroutine SURFTdll (T,Dl,z,sigma,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, Dl, z(ncmax)
      double precision, intent(out) :: sigma
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SURFTdll"::SURFTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SURFTdll
c     dll_export SURFTdll
      call SURFT (T,Dl,z,sigma,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SURTENdll (T,Dl,Dv,x,y,sigma,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, Dl, Dv, x(ncmax),y(ncmax)
      double precision, intent(out) :: sigma
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SURTENdll"::SURTENdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SURTENdll
c     dll_export SURTENdll
      call SURTEN (T,Dl,Dv,x,y,sigma,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine STNdll (T,Dl,Dv,x,y,sigma,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, Dl, Dv, x(ncmax),y(ncmax)
      double precision, intent(out) :: sigma
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "STNdll"::STNdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::STNdll
c     dll_export STNdll
      call STN (T,Dl,Dv,x,y,sigma,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine IDCRVdll (D,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, z(ncmax)
      double precision, intent(inout) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "IDCRVdll"::IDCRVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::IDCRVdll
c     dll_export IDCRVdll
      call IDCRV (D,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine BLCRVdll (D,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, z(ncmax)
      double precision, intent(inout) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "BLCRVdll"::BLCRVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::BLCRVdll
c     dll_export BLCRVdll
      call BLCRV (D,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine JTCRVdll (D,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, z(ncmax)
      double precision, intent(inout) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "JTCRVdll"::JTCRVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::JTCRVdll
c     dll_export JTCRVdll
      call JTCRV (D,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine JICRVdll (D,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, z(ncmax)
      double precision, intent(inout) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "JICRVdll"::JICRVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::JICRVdll
c     dll_export JICRVdll
      call JICRV (D,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PASSCMNdll (hvr,iset,icomp,jcomp,
     &                       hstr,ilng,dbl,arr,ierr,herr)
      include 'CONSTS.INC'
      character, intent(in) :: hvr*255
      integer, intent(in) :: iset, icomp, jcomp
      character*255, intent(inout) :: hstr
      integer, intent(inout) :: ilng
      double precision, intent(inout) :: dbl
      double precision, intent(inout) :: arr(100)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
      character*255 hvr2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PASSCMNdll"::PASSCMNdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PASSCMNdll
c     dll_export PASSCMNdll
      i=INDEX(hvr,CHAR(0))-1
      if (i.lt.0) i=LEN(hvr)
      hvr2=hvr(:i)           !This is required for mixed language programming.
      call PASSCMN (hvr2,iset,icomp,jcomp,
     &              hstr,ilng,dbl,arr,ierr,herr)
      herr(255:255)=CHAR(0)
      hstr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SPLNVALdll (isp,iderv,a,f,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: isp, iderv
      double precision, intent(in) :: a
      double precision, intent(out) :: f
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
      double precision z(ncmax)
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SPLNVALdll"::SPLNVALdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SPLNVALdll
c     dll_export SPLNVALdll
      z=0d0
      call SPLNVAL (isp,iderv,z,a,f,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SPLNROOTdll (isp,iderv,f,a,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: isp, iderv
      double precision, intent(in) :: f
      double precision, intent(out) :: a
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
      double precision z(ncmax)
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SPLNROOTdll"::SPLNROOTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SPLNROOTdll
c     dll_export SPLNROOTdll
      z=0d0
      call SPLNROOT (isp,iderv,z,f,a,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine LIQSPNDLdll (T,z,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, z(ncmax)
      double precision, intent(out) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "LIQSPNDLdll"::LIQSPNDLdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::LIQSPNDLdll
c     dll_export LIQSPNDLdll
      call LIQSPNDL (T,z,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine VAPSPNDLdll (T,z,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, z(ncmax)
      double precision, intent(out) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "VAPSPNDLdll"::VAPSPNDLdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::VAPSPNDLdll
c     dll_export VAPSPNDLdll
      call VAPSPNDL (T,z,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TPRHOdll (T,P,z,kph,kguess,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, P, z(ncmax)
      integer, intent(in) :: kph, kguess
      double precision, intent(inout) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TPRHOdll"::TPRHOdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TPRHOdll
c     dll_export TPRHOdll
      call TPRHO (T,P,z,kph,kguess,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TPRHOPRdll (T,P,z,D1,D2)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, P, z(ncmax)
      double precision, intent(out) :: D1, D2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TPRHOPRdll"::TPRHOPRdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TPRHOPRdll
c     dll_export TPRHOPRdll
      call TPRHOPR (T,P,z,D1,D2)
      end
c ======================================================================
      subroutine DSFL1dll (D,s,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, s, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DSFL1dll"::DSFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DSFL1dll
c     dll_export DSFL1dll
      call DSFL1 (D,s,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DHFL1dll (D,h,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, h, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DHFL1dll"::DHFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DHFL1dll
c     dll_export DHFL1dll
      call DHFL1 (D,h,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DEFL1dll (D,e,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, e, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DEFL1dll"::DEFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DEFL1dll
c     dll_export DEFL1dll
      call DEFL1 (D,e,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DBFL1dll (D,b,z,hab,T,P,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, b, z(ncmax)
      character*2, intent(in) :: hab
      double precision, intent(out) :: T, P
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DBFL1dll"::DBFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DBFL1dll
c     dll_export DBFL1dll
      call DBFL1 (D,b,z,hab,T,P,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PDFL1dll (P,D,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, D, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PDFL1dll"::PDFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PDFL1dll
c     dll_export PDFL1dll
      call PDFL1 (P,D,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PHFL1dll (P,h,z,kph,T,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, h, z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(out) :: T, D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHFL1dll"::PHFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHFL1dll
c     dll_export PHFL1dll
      call PHFL1 (P,h,z,kph,T,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PEFL1dll (P,e,z,kph,T,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, e, z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(out) :: T, D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PEFL1dll"::PEFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PEFL1dll
c     dll_export PEFL1dll
      call PEFL1 (P,e,z,kph,T,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PSFL1dll (P,s,z,kph,T,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, s, z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(out) :: T, D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PSFL1dll"::PSFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PSFL1dll
c     dll_export PSFL1dll
      call PSFL1 (P,s,z,kph,T,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine THFL1dll (T,h,z,Dmin,Dmax,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, h, z(ncmax), Dmin, Dmax
      double precision, intent(out) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "THFL1dll"::THFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::THFL1dll
c     dll_export THFL1dll
      call THFL1 (T,h,z,Dmin,Dmax,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TSFL1dll (T,s,z,Dmin,Dmax,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, s, z(ncmax), Dmin, Dmax
      double precision, intent(out) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TSFL1dll"::TSFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TSFL1dll
c     dll_export TSFL1dll
      call TSFL1 (T,s,z,Dmin,Dmax,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TEFL1dll (T,e,z,Dmin,Dmax,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, e, z(ncmax), Dmin, Dmax
      double precision, intent(out) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TEFL1dll"::TEFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TEFL1dll
c     dll_export TEFL1dll
      call TEFL1 (T,e,z,Dmin,Dmax,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine HSFL1dll (h,s,z,Dmin,Dmax,T,D,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: h, s, z(ncmax), Dmin, Dmax
      double precision, intent(out) :: T, D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "HSFL1dll"::HSFL1dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::HSFL1dll
c     dll_export HSFL1dll
      call HSFL1 (h,s,z,Dmin,Dmax,T,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DBFL2dll (D,b,z,kq,ab,T,P,Dl,Dv,x,y,q,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, b, z(ncmax)
      integer, intent(in) :: kq
      character, intent(in) :: ab*2
      double precision, intent(out) :: T,P,Dl,Dv,x(ncmax),y(ncmax),q
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DBFL2dll"::DBFL2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DBFL2dll
c     dll_export DBFL2dll
      double precision ybub(ncmax),xdew(ncmax),Tbub,Tdew,Pbub,Pdew,
     &            Dlbub,Dvdew
      call ABFL2 (ab,D,b,z,kq,0,Tbub,Tdew,Pbub,Pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,q,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DQFL2dll (D,q,z,kq,T,P,Dl,Dv,x,y,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, q, z(ncmax)
      integer, intent(in) :: kq
      double precision, intent(out) :: T,P,Dl,Dv,x(ncmax),y(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DQFL2dll"::DQFL2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DQFL2dll
c     dll_export DQFL2dll
      double precision ybub(ncmax),xdew(ncmax),Tbub,Tdew,Pbub,Pdew,
     &            Dlbub,Dvdew,qq
      call ABFL2 ('DQ',D,q,z,kq,0,Tbub,Tdew,Pbub,Pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,qq,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TPFL2dll (T,P,z,Dl,Dv,x,y,q,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, P, z(ncmax)
      double precision, intent(out) :: Dl,Dv,x(ncmax),y(ncmax),q
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TPFL2dll"::TPFL2dll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TPFL2dll
c     dll_export TPFL2dll
      call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TPFLSHdll (T,P,z,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, P, z(ncmax)
      double precision, intent(out) :: D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TPFLSHdll"::TPFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TPFLSHdll
c     dll_export TPFLSHdll
      if (T.gt.0d0)
     &    call TPFLSH (T,P,z,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TDFLSHdll (T,D,z,P,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, D, z(ncmax)
      double precision, intent(out) :: P,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TDFLSHdll"::TDFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TDFLSHdll
c     dll_export TDFLSHdll
      call TDFLSH (T,D,z,P,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PDFLSHdll (P,D,z,T,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, D, z(ncmax)
      double precision, intent(out) :: T,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PDFLSHdll"::PDFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PDFLSHdll
c     dll_export PDFLSHdll
      call PDFLSH (P,D,z,T,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PHFLSHdll (P,h,z,T,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, h, z(ncmax)
      double precision, intent(out) :: T,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHFLSHdll"::PHFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHFLSHdll
c     dll_export PHFLSHdll
      call PHFLSH (P,h,z,T,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PSFLSHdll (P,s,z,T,D,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, s, z(ncmax)
      double precision, intent(out) :: T,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,h,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PSFLSHdll"::PSFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PSFLSHdll
c     dll_export PSFLSHdll
      call PSFLSH (P,s,z,T,D,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PEFLSHdll (P,e,z,T,D,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, e, z(ncmax)
      double precision, intent(out) :: T,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PEFLSHdll"::PEFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PEFLSHdll
c     dll_export PEFLSHdll
      call PEFLSH (P,e,z,T,D,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine THFLSHdll (T,h,z,kr,P,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,
     &                      ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, h, z(ncmax)
      integer, intent(in) :: kr
      double precision, intent(out) :: P,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "THFLSHdll"::THFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::THFLSHdll
c     dll_export THFLSHdll
      call THFLSH (T,h,z,kr,P,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TSFLSHdll (T,s,z,kr,P,D,Dl,Dv,x,y,q,e,h,Cv,Cp,w,
     &                      ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, s, z(ncmax)
      integer, intent(in) :: kr
      double precision, intent(out) :: P,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,h,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TSFLSHdll"::TSFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TSFLSHdll
c     dll_export TSFLSHdll
      call TSFLSH (T,s,z,kr,P,D,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TEFLSHdll (T,e,z,kr,P,D,Dl,Dv,x,y,q,h,s,Cv,Cp,w,
     &                      ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, e, z(ncmax)
      integer, intent(in) :: kr
      double precision, intent(out) :: P,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TEFLSHdll"::TEFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TEFLSHdll
c     dll_export TEFLSHdll
      call TEFLSH (T,e,z,kr,P,D,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DHFLSHdll (D,h,z,T,P,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, h, z(ncmax)
      double precision, intent(out) :: T,P,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DHFLSHdll"::DHFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DHFLSHdll
c     dll_export DHFLSHdll
      call DHFLSH (D,h,z,T,P,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DSFLSHdll (D,s,z,T,P,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, s, z(ncmax)
      double precision, intent(out) :: T,P,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,h,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DSFLSHdll"::DSFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DSFLSHdll
c     dll_export DSFLSHdll
      call DSFLSH (D,s,z,T,P,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DEFLSHdll (D,e,z,T,P,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, e, z(ncmax)
      double precision, intent(out) :: T,P,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DEFLSHdll"::DEFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DEFLSHdll
c     dll_export DEFLSHdll
      call DEFLSH (D,e,z,T,P,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine HSFLSHdll (h,s,z,T,P,D,Dl,Dv,x,y,q,e,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: h, s, z(ncmax)
      double precision, intent(out) :: T,P,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 e,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "HSFLSHdll"::HSFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::HSFLSHdll
c     dll_export HSFLSHdll
      call HSFLSH (h,s,z,T,P,D,Dl,Dv,x,y,q,e,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine ESFLSHdll (e,s,z,T,P,D,Dl,Dv,x,y,q,h,Cv,Cp,w,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: e, s, z(ncmax)
      double precision, intent(out) :: T,P,D,Dl,Dv,x(ncmax),y(ncmax),q,
     &                                 h,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "ESFLSHdll"::ESFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::ESFLSHdll
c     dll_export ESFLSHdll
      call ESFLSH (e,s,z,T,P,D,Dl,Dv,x,y,q,h,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TQFLSHdll (T,q,z,kq,P,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,
     &                      ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, q, z(ncmax)
      integer, intent(in) :: kq
      double precision, intent(out) :: P,D,Dl,Dv,x(ncmax),y(ncmax),
     &                                 e,h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TQFLSHdll"::TQFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TQFLSHdll
c     dll_export TQFLSHdll
      call TQFLSH (T,q,z,kq,P,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PQFLSHdll (P,q,z,kq,T,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,
     &                      ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, q, z(ncmax)
      integer, intent(in) :: kq
      double precision, intent(out) :: T,D,Dl,Dv,x(ncmax),y(ncmax),
     &                                 e,h,s,Cv,Cp,w
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PQFLSHdll"::PQFLSHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PQFLSHdll
c     dll_export PQFLSHdll
      call PQFLSH (P,q,z,kq,T,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATSPLNdll (z,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATSPLNdll"::SATSPLNdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATSPLNdll
c     dll_export SATSPLNdll
      call SATSPLN (z,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATGVdll (T,P,z,vf,b,ipv,ityp,isp,Dx,Dy,x,y,
     &                     ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(inout) :: T, P
      double precision, intent(in) :: z(ncmax)
      double precision, intent(in) :: vf, b
      integer, intent(in) :: ipv, ityp, isp
      double precision, intent(inout) :: Dx, Dy, x(ncmax),y(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATGVdll"::SATGVdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATGVdll
c     dll_export SATGVdll
      call SATGV (T,P,z,vf,b,ipv,ityp,isp,Dx,Dy,x,y,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATTPdll (T,P,z,iFlsh,iGuess,D,Dl,Dv,x,y,q,
     &                     ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(inout) :: T, P
      double precision, intent(in) :: z(ncmax)
      integer, intent(in) :: iFlsh, iGuess
      double precision, intent(inout) :: D, Dl, Dv, x(ncmax), y(ncmax)
      double precision, intent(out) :: q
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATTPdll"::SATTPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATTPdll
c     dll_export SATTPdll
      call SATTP (T,P,z,iFlsh,iGuess,D,Dl,Dv,x,y,q,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATTdll (T,z,kph,P,Dl,Dv,x,y,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(out) :: P, Dl, Dv, x(ncmax), y(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATTdll"::SATTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATTdll
c     dll_export SATTdll
      call SATT (T,z,kph,P,Dl,Dv,x,y,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATPdll (P,z,kph,T,Dl,Dv,x,y,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(out) :: T, Dl, Dv, x(ncmax), y(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATPdll"::SATPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATPdll
c     dll_export SATPdll
      call SATP (P,z,kph,T,Dl,Dv,x,y,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATDdll (D,z,kph,kr,T,P,Dl,Dv,x,y,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, z(ncmax)
      integer, intent(in) :: kph
      integer, intent(out) :: kr
      double precision, intent(out) :: T, P, Dl, Dv, x(ncmax), y(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATDdll"::SATDdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATDdll
c     dll_export SATDdll
      call SATD (D,z,kph,kr,T,P,Dl,Dv,x,y,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATHdll (h,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,
     &                    ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: h, z(ncmax)
      integer, intent(in) :: kph
      integer, intent(out) :: nroot, k1
      double precision, intent(out) :: T1, P1, D1
      integer, intent(out) :: k2
      double precision, intent(out) :: T2, P2, D2
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATHdll"::SATHdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATHdll
c     dll_export SATHdll
      call SATH (h,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATEdll (e,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,
     &                    ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: e, z(ncmax)
      integer, intent(in) :: kph
      integer, intent(out) :: nroot, k1
      double precision, intent(out) :: T1, P1, D1
      integer, intent(out) :: k2
      double precision, intent(out) :: T2, P2, D2
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATEdll"::SATEdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATEdll
c     dll_export SATEdll
      call SATE (e,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATSdll (s,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,
     &                 k3,T3,P3,D3,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: s, z(ncmax)
      integer, intent(in) :: kph
      integer, intent(out) :: nroot, k1
      double precision, intent(out) :: T1, P1, D1
      integer, intent(out) :: k2
      double precision, intent(out) :: T2, P2, D2
      integer, intent(out) :: k3
      double precision, intent(out) :: T3, P3, D3
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATSdll"::SATSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATSdll
c     dll_export SATSdll
      call SATS (s,z,kph,nroot,k1,T1,P1,D1,k2,T2,P2,D2,
     &                 k3,T3,P3,D3,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATGUESSdll (kph,iprop,x,T,P,D,h,s,Dy,y,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: kph, iprop
      double precision, intent(in) :: x(ncmax)
      double precision, intent(inout) :: T,P,D,h,s,Dy,y(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATGUESSdll"::SATGUESSdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATGUESSdll
c     dll_export SATGUESSdll
      call SATGUESS (kph,iprop,0,0,x,T,P,D,h,s,Dy,y,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATTESTdll (T,z,kph,P,x2,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(out) :: P, x2(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATTESTdll"::SATTESTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATTESTdll
c     dll_export SATTESTdll
      double precision y(ncmax)
      call SATEST (kph,T,P,z,x2,y,ierr,herr)
      if (kph.eq.1) x2=y
      if (ierr.eq.0) then
        ierr=-1
        herr='SATTESTdll has been replaced with the routine SATESTdll.'
      endif
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATPESTdll (P,z,kph,T,x2,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, z(ncmax)
      integer, intent(in) :: kph
      double precision, intent(out) :: T, x2(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATPESTdll"::SATPESTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATPESTdll
c     dll_export SATPESTdll
      double precision y(ncmax)
      call SATEST (kph+2,T,P,z,x2,y,ierr,herr)
      if (kph.eq.3) x2=y
      if (ierr.eq.0) then
        ierr=-1
        herr='SATPESTdll has been replaced with the routine SATESTdll.'
      endif
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SATESTdll (iFlash,T,P,z,x,y,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: z(ncmax)
      integer, intent(in) :: iFlash
      double precision, intent(inout) :: T, P
      double precision, intent(out) :: x(ncmax), y(ncmax)
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SATESTdll"::SATESTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SATESTdll
c     dll_export SATESTdll
      call SATEST (iFlash,T,P,z,x,y,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PSATKdll (icomp,T,P,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T
      double precision, intent(out) :: P
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PSATKdll"::PSATKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PSATKdll
c     dll_export PSATKdll
      call PSATK (icomp,T,P,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DLSATKdll (icomp,T,D,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T
      double precision, intent(out) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DLSATKdll"::DLSATKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DLSATKdll
c     dll_export DLSATKdll
      call DLSATK (icomp,T,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine DVSATKdll (icomp,T,D,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T
      double precision, intent(out) :: D
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "DVSATKdll"::DVSATKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::DVSATKdll
c     dll_export DVSATKdll
      call DVSATK (icomp,T,D,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TSATPdll (P,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TSATPdll"::TSATPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TSATPdll
c     dll_export TSATPdll
      call TSATP (P,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine TSATDdll (D,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: D, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "TSATDdll"::TSATDdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::TSATDdll
c     dll_export TSATDdll
      call TSATD (D,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine MELTTdll (T,z,P,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, z(ncmax)
      double precision, intent(out) :: P
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "MELTTdll"::MELTTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::MELTTdll
c     dll_export MELTTdll
      call MELTT (T,z,P,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine MELTKdll (icomp,T,P,ierr,herr)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp
      double precision, intent(in) :: T
      double precision, intent(out) :: P
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "MELTKdll"::MELTKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::MELTKdll
c     dll_export MELTKdll
      call MELTK (icomp,T,P,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine MLTH2Odll (T,P1,P2)
      include 'CONSTS.INC'
      double precision, intent(in) :: T
      double precision, intent(out) :: P1, P2
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "MLTH2Odll"::MLTH2Odll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::MLTH2Odll
c     dll_export MLTH2Odll
      call MLTH2O (T,P1,P2)
      end
c ======================================================================
      subroutine MELTPdll (P,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "MELTPdll"::MELTPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::MELTPdll
c     dll_export MELTPdll
      call MELTP (P,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SUBLTdll (T,z,P,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: T, z(ncmax)
      double precision, intent(out) :: P
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SUBLTdll"::SUBLTdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SUBLTdll
c     dll_export SUBLTdll
      call SUBLT (T,z,P,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine SUBLPdll (P,z,T,ierr,herr)
      include 'CONSTS.INC'
      double precision, intent(in) :: P, z(ncmax)
      double precision, intent(out) :: T
      integer, intent(out) :: ierr
      character*255, intent(out) :: herr
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "SUBLPdll"::SUBLPdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::SUBLPdll
c     dll_export SUBLPdll
      call SUBLP (P,z,T,ierr,herr)
      herr(255:255)=CHAR(0)
      end
c ======================================================================
      subroutine PHIKdll (icomp,itau,idel,tau,delta,phi)
      include 'CONSTS.INC'
      integer, intent(in) :: icomp, itau, idel
      double precision, intent(in) :: tau, delta
      double precision, intent(out) :: phi
      double precision PHIK
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHIKdll"::PHIKdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHIKdll
c     dll_export PHIKdll
      phi=PHIK(icomp,itau,idel,tau,delta)
      end
c ======================================================================
      subroutine PHIMIXdll (i,j,itau,idel,tau,delta,z,phi)
      include 'CONSTS.INC'
      integer, intent(in) :: i, j, itau, idel
      double precision, intent(in) :: tau, delta, z(ncmax)
      double precision, intent(out) :: phi
      double precision PHIMIX
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHIMIXdll"::PHIMIXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHIMIXdll
c     dll_export PHIMIXdll
      phi=PHIMIX(i,j,itau,idel,tau,delta,z)
      end
c ======================================================================
      subroutine PHIHMXdll (itau,idel,tau,delta,z,phi)
      include 'CONSTS.INC'
      integer, intent(in) :: itau, idel
      double precision, intent(in) :: tau, delta, z(ncmax)
      double precision, intent(out) :: phi
      double precision PHIHMX
cDEC$ ATTRIBUTES DLLEXPORT, Decorate, Alias: "PHIHMXdll"::PHIHMXdll
cDEC$ ATTRIBUTES STDCALL, REFERENCE::PHIHMXdll
c     dll_export PHIHMXdll
      phi=PHIHMX(itau,idel,tau,delta,z)
      end
