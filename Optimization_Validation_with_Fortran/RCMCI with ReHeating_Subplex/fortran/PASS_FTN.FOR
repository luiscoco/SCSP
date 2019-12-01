      subroutine RPVersion (v)
      character v*255
      v='8.01c'
      end
c ======================================================================
c     subroutine WRTREFdll(fxname)
c     implicit double precision (a-h,o-z)
c     implicit integer (i-n)
c     character*255 fxname
c     dll_export WRTREFdll
c     call WRTREF (fxname,-1,1,0,0,0)
c     end
c
c     subroutine MINIMIZEdll(fxname,idfile,mn,i)
c     implicit double precision (a-h,o-z)
c     implicit integer (i-n)
c     character*255 fxname,idfile
c     double precision mn(200)
c     dll_export MINIMIZEdll
c     call minimize(fxname,idfile,mn,i)
c     end
c
c     subroutine FOFXdll(mn,i,fofxx,ierr,herr)
c     implicit double precision (a-h,o-z)
c     implicit integer (i-n)
c     character*255 herr
c     double precision mn(200)
c     dll_export FOFXdll
c     call fofx(mn,i,fofxx,ierr,herr)
c     end
c ======================================================================

c ======================================================================
c  begin file pass_ftn.for
c
c  This file defines the DLL-callable routines which would be used by
c  Visual Basic, Excel, and other applications to access the main
c  Fortran code.  It is not needed when compiling Fortran applications.
c  It is only used when compiling the DLL and contains commands specific
c  to the Lahey/Fujitsu Fortran 95 Compiler.  When using other compilers,
c  the lines with 'dll_export' should be commented out.
c
c  The calling sequence is identical (except for the SETUPdll routine) to
c  the main Fortran routines, with the letters 'dll' appended to the routine
c  name.  See the Excel sample file or the Visual Basic 'SAMPLE.BAS'
c  file for usage information.
c
c ======================================================================
c
      subroutine SETUPdll (i,hfld,hfm,hrf,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-k,m,n)
      character hfld*10000,hfm*255,hrf*3,herr*255
      call SETUP0 (i,hfld,hfm,hrf,ierr,herr)
      end
c ======================================================================
      subroutine SETREFdll (hrf,ixflag,x0,h0,s0,t0,p0,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      character*3 hrf
      dimension x0(ncmax)
      call SETREF (hrf,ixflag,x0,h0,s0,t0,p0,ierr,herr)
      end
c ======================================================================
      subroutine SETMIXdll (hmxnme,hfmix,hrf,ncc,hfile,x,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 hmxnme,hfmix,hfiles(ncmax),herr
      character hfile*10000,hrf*3
      dimension x(ncmax)
      call SETMIX (hmxnme,hfmix,hrf,ncc,hfiles,x,ierr,herr)
      hfile=hfiles(1)
      j=index(hfile,' ')
      hfile=hfile(1:j-1)//'|'
      do i=2,ncc
        j=index(hfile,' ')
        hfile=hfile(1:j-1)//hfiles(i)
        j=index(hfile,' ')
        hfile=hfile(1:j-1)//'|'
      enddo
      end
c ======================================================================
      subroutine SETMODdll (nc,htype,hmix,hcomp2,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      character*60 hcomp2
      character*3 htype,hmix,hcomp(1:ncmax)
      do i=1,ncmax
        hcomp(i)=hcomp2(i*3-2:i*3)
      enddo
      call SETMOD (nc,htype,hmix,hcomp,ierr,herr)
      end
c ======================================================================
      subroutine PUREFLDdll (icomp)
      implicit integer (i-n)
      call PUREFLD (icomp)
      end
c ======================================================================
      subroutine SETNCdll (ncomp)
      implicit integer (i-n)
      call SETNC (ncomp)
      end
c ======================================================================
      subroutine SETPATHdll (hpth)
      character hpth*255
      call SETPATH (hpth)
      end
c ======================================================================
      subroutine UNSETAGAdll
      call UNSETAGA
      end
c ======================================================================
      subroutine CRITPdll (x,tcrit,pcrit,Dcrit,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call CRITP (x,tcrit,pcrit,Dcrit,ierr,herr)
      end
c ======================================================================
      subroutine THERMdll (t,rho,x,p,e,h,s,cv,cp,w,hjt)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call THERM (t,rho,x,p,e,h,s,cv,cp,w,hjt)
      end
c ======================================================================
      subroutine THERM0dll (t,rho,x,p,e,h,s,cv,cp,w,a,g)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call THERM0 (t,rho,x,p,e,h,s,cv,cp,w,a,g)
      end
c ======================================================================
      subroutine THERM2dll (t,rho,x,p,e,h,s,cv,cp,w,Z,hjt,A,G,xkappa,
     &                   beta,dPdD,d2PdD2,dPdT,dDdT,dDdP,
     &                   d2PT2,d2PdTD,spare3,spare4)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call THERM2 (t,rho,x,p,e,h,s,cv,cp,w,Z,hjt,A,G,xkappa,beta,
     &                   dPdD,d2PdD2,dPdT,dDdT,dDdP,
     &                   d2PT2,d2PdTD,spare3,spare4)
      end
c ======================================================================
      subroutine THERM3dll (t,rho,x,
     &           xkappa,beta,xisenk,xkt,betas,bs,xkkt,thrott,pi,spht)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call THERM3 (t,rho,x,
     &           xkappa,beta,xisenk,xkt,betas,bs,xkkt,thrott,pi,spht)
      end
c ======================================================================
      subroutine RESIDUALdll (t,rho,x,pr,er,hr,sr,cvr,cpr,Ar,Gr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call RESIDUAL (t,rho,x,pr,er,hr,sr,cvr,cpr,Ar,Gr)
      end
c ======================================================================
      subroutine ENTROdll (t,rho,x,s)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call ENTRO (t,rho,x,s)
      end
c ======================================================================
      subroutine ENTHALdll (t,rho,x,h)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call ENTHAL (t,rho,x,h)
      end
c ======================================================================
      subroutine CVCPdll (t,rho,x,cv,cp)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call CVCP (t,rho,x,cv,cp)
      end
c ======================================================================
      subroutine CVCPKdll (icomp,t,rho,cv,cp)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      call CVCPK (icomp,t,rho,cv,cp)
      end
c ======================================================================
      subroutine GIBBSdll (t,rho,x,Ar,Gr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call GIBBS (t,rho,x,Ar,Gr)
      end
c ======================================================================
      subroutine AGdll (t,rho,x,a,g)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call AG (t,rho,x,a,g)
      end
c ======================================================================
      subroutine PRESSdll (t,rho,x,p)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call PRESS (t,rho,x,p)
      end
c ======================================================================
      subroutine DPDDdll (t,rho,x,dpdrho)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DPDD (t,rho,x,dpdrho)
      end
c ======================================================================
      subroutine DPDDKdll (icomp,t,rho,dpdrho)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      call DPDDK (icomp,t,rho,dpdrho)
      end
c ======================================================================
      subroutine DPDD2dll (t,rho,x,d2PdD2)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DPDD2 (t,rho,x,d2PdD2)
      end
c ======================================================================
      subroutine DPDTdll (t,rho,x,dpt)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DPDT (t,rho,x,dpt)
      end
c ======================================================================
      subroutine DPDTKdll (icomp,t,rho,dpt)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      call DPDTK (icomp,t,rho,dpt)
      end
c ======================================================================
      subroutine DDDPdll (t,rho,x,drhodp)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DDDP (t,rho,x,drhodp)
      end
c ======================================================================
      subroutine DDDTdll (t,rho,x,drhodt)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DDDT (t,rho,x,drhodt)
      end
c ======================================================================
      subroutine DHD1dll (t,rho,x,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,
     &                    dhdp_d)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DHD1 (t,rho,x,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,dhdp_d)
      end
c ======================================================================
      subroutine FGCTYdll (t,rho,x,f)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax),f(ncmax)
      call FGCTY (t,rho,x,f)
      end
c ======================================================================
      subroutine FGCTY2dll (t,rho,x,f,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),f(ncmax)
      call FGCTY2 (t,rho,x,f,ierr,herr)
      end
c ======================================================================
      subroutine FUGCOFdll (t,rho,x,f,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),f(ncmax)
      call FUGCOF (t,rho,x,f,ierr,herr)
      end
c ======================================================================
      subroutine CHEMPOTdll (t,rho,x,u,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),u(ncmax)
      call CHEMPOT (t,rho,x,u,ierr,herr)
      end
c ======================================================================
      subroutine ACTVYdll (t,rho,x,actv,gamma,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),actv(ncmax),gamma(ncmax)
      call ACTVY (t,rho,x,actv,gamma,ierr,herr)
      end
c ======================================================================
      subroutine VIRBdll (t,x,b)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call VIRB (t,x,b)
      end
c ======================================================================
      subroutine B12dll (t,x,b)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call B12 (t,x,b)
      end
c ======================================================================
      subroutine DBDTdll (t,x,b)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DBDT (t,x,b)
      end
c ======================================================================
      subroutine VIRCdll (t,x,c)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call VIRC (t,x,c)
      end
c ======================================================================
      subroutine VIRBAdll (t,x,b)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call VIRBA (t,x,b)
      end
c ======================================================================
      subroutine VIRCAdll (t,x,c)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call VIRCA (t,x,c)
      end
c ======================================================================
      subroutine HEATdll (t,rho,x,hg,hn,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      CALL HEAT (t,rho,x,hg,hn,ierr,herr)
      end
c ======================================================================
      subroutine SATTPdll (t,p,x,kph,iGuess,d,rhol,rhov,xliq,xvap,q,
     &                     ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),xliq(ncmax),xvap(ncmax)
      call SATTP (t,p,x,kph,iGuess,d,rhol,rhov,xliq,xvap,q,ierr,herr)
      end
c ======================================================================
      subroutine SATTdll (t,x,kph,p,rhol,rhov,xliq,xvap,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),xliq(ncmax),xvap(ncmax)
      call SATT (t,x,kph,p,rhol,rhov,xliq,xvap,ierr,herr)
      end
c ======================================================================
      subroutine SATPdll (p,x,kph,t,rhol,rhov,xliq,xvap,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),xliq(ncmax),xvap(ncmax)
      call SATP (p,x,kph,t,rhol,rhov,xliq,xvap,ierr,herr)
      end
c ======================================================================
      subroutine SATDdll (rho,x,kph,kr,t,p,rhol,rhov,xliq,xvap,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),xliq(ncmax),xvap(ncmax)
      call SATD (rho,x,kph,kr,t,p,rhol,rhov,xliq,xvap,i,herr)
      end
c ======================================================================
      subroutine SATHdll (h,x,kph,nroot,k1,t1,p1,d1,k2,t2,p2,d2,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call SATH (h,x,kph,nroot,k1,t1,p1,d1,k2,t2,p2,d2,i,herr)
      end
c ======================================================================
      subroutine SATEdll (e,x,kph,nroot,k1,t1,p1,d1,k2,t2,p2,d2,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call SATE (e,x,kph,nroot,k1,t1,p1,d1,k2,t2,p2,d2,i,herr)
      end
c ======================================================================
      subroutine SATSdll (s,x,kph,nroot,k1,t1,p1,d1,k2,t2,p2,d2,
     &                 k3,t3,p3,d3,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call SATS (s,x,kph,nroot,k1,t1,p1,d1,k2,t2,p2,d2,
     &                 k3,t3,p3,d3,ierr,herr)
      end
c ======================================================================
      subroutine SATTESTdll (t,x,kph,p,x2,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),x2(ncmax)
      call SATTEST (t,x,kph,p,x2,ierr,herr)
      end
c ======================================================================
      subroutine SATPESTdll (p,x,kph,t,x2,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),x2(ncmax)
      call SATPEST (p,x,kph,t,x2,ierr,herr)
      end
c ======================================================================
      subroutine CSATKdll (icomp,t,kph,p,rho,csat,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call CSATK (icomp,t,kph,p,rho,csat,ierr,herr)
      end
c ======================================================================
      subroutine DPTSATKdll (icomp,t,kph,p,rho,csat,dpt,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call DPTSATK (icomp,t,kph,p,rho,csat,dpt,ierr,herr)
      end
c ======================================================================
      subroutine CV2PKdll (icomp,t,rho,cv2p,csat,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call CV2PK (icomp,t,rho,cv2p,csat,ierr,herr)
      end
c ======================================================================
      subroutine PSATKdll (icomp,t,p,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call PSATK (icomp,t,p,ierr,herr)
      end
c ======================================================================
      subroutine DLSATKdll (icomp,t,d,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call DLSATK (icomp,t,d,ierr,herr)
      end
c ======================================================================
      subroutine DVSATKdll (icomp,t,d,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call DVSATK (icomp,t,d,ierr,herr)
      end
c ======================================================================
      subroutine SPNDLdll (t,x,rhol,rhov,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call SPNDL (t,x,rhol,rhov,ierr,herr)
      end
c ======================================================================
      subroutine TPRHOdll (t,p,x,j,i,rho,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call TPRHO (t,p,x,j,i,rho,ierr,herr)
      end
c ======================================================================
      subroutine TPRHOPRdll (t,p,x,rho1,rho2)
      implicit double precision (a-h,o-z)
      parameter (ncmax=20)
      dimension x(ncmax)
      call TPRHOPR (t,p,x,rho1,rho2)
      end
c ======================================================================
      subroutine TPFLSHdll (t,p,z,D,Dl,Dv,x,y,q,e,h,s,cv,cp,w,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      if (t.gt.0.d0)
     &    call TPFLSH (t,p,z,D,Dl,Dv,x,y,q,e,h,s,cv,cp,w,ierr,herr)
      end
c ======================================================================
      subroutine TDFLSHdll (t,D,x,p,Dl,Dv,xl,xv,q,e,h,s,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax),xl(ncmax),xv(ncmax)
      call TDFLSH (t,D,x,p,Dl,Dv,xl,xv,q,e,h,s,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine PDFLSHdll (p,D,z,t,Dl,Dv,x,y,q,e,h,s,cv,cp,w,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call PDFLSH (p,D,z,t,Dl,Dv,x,y,q,e,h,s,cv,cp,w,ierr,herr)
      end
c ======================================================================
      subroutine PHFLSHdll (p,h,z,t,D,Dl,Dv,x,y,q,e,s,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call PHFLSH (p,h,z,t,D,Dl,Dv,x,y,q,e,s,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine PSFLSHdll (p,s,z,t,D,Dl,Dv,x,y,q,e,h,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call PSFLSH (p,s,z,t,D,Dl,Dv,x,y,q,e,h,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine PEFLSHdll (p,e,z,t,D,Dl,Dv,x,y,q,h,s,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call PEFLSH (p,e,z,t,D,Dl,Dv,x,y,q,h,s,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine THFLSHdll (t,h,z,kr,p,D,Dl,Dv,x,y,q,e,s,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call THFLSH (t,h,z,kr,p,D,Dl,Dv,x,y,q,e,s,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine TSFLSHdll (t,s,z,kr,p,D,Dl,Dv,x,y,q,e,h,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call TSFLSH (t,s,z,kr,p,D,Dl,Dv,x,y,q,e,h,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine TEFLSHdll (t,e,z,kr,p,D,Dl,Dv,x,y,q,h,s,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call TEFLSH (t,e,z,kr,p,D,Dl,Dv,x,y,q,h,s,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine DHFLSHdll (D,h,z,t,p,Dl,Dv,x,y,q,e,s,cv,cp,w,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call DHFLSH (D,h,z,t,p,Dl,Dv,x,y,q,e,s,cv,cp,w,ierr,herr)
      end
c ======================================================================
      subroutine DSFLSHdll (D,s,z,t,p,Dl,Dv,x,y,q,e,h,cv,cp,w,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call DSFLSH (D,s,z,t,p,Dl,Dv,x,y,q,e,h,cv,cp,w,ierr,herr)
      end
c ======================================================================
      subroutine DEFLSHdll (D,e,z,t,p,Dl,Dv,x,y,q,h,s,cv,cp,w,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call DEFLSH (D,e,z,t,p,Dl,Dv,x,y,q,h,s,cv,cp,w,ierr,herr)
      end
c ======================================================================
      subroutine HSFLSHdll (h,s,z,t,p,D,Dl,Dv,x,y,q,e,cv,cp,w,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call HSFLSH (h,s,z,t,p,D,Dl,Dv,x,y,q,e,cv,cp,w,ierr,herr)
      end
c ======================================================================
      subroutine ESFLSHdll (e,s,z,t,p,D,Dl,Dv,x,y,q,h,cv,cp,w,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call ESFLSH (e,s,z,t,p,D,Dl,Dv,x,y,q,h,cv,cp,w,ierr,herr)
      end
c ======================================================================
      subroutine ABFL1dll (a,b,x,kph,ab,dmin,dmax,t,p,D,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*2 ab
      character*255 herr
      dimension x(ncmax)
      call ABFL1 (a,b,x,kph,ab,dmin,dmax,t,p,D,ierr,herr)
      end
c ======================================================================
      subroutine DBFL1dll (d,b,x,ab,t,p,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*2 ab
      character*255 herr
      dimension x(ncmax)
      call DBFL1 (d,b,x,ab,t,p,ierr,herr)
      end
c ======================================================================
      subroutine ABFL2dll (a,b,z,kq,ksat,ab,
     &                     tbub,tdew,pbub,pdew,Dlbub,Dvdew,ybub,xdew,
     &                     t,p,Dl,Dv,x,y,q,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*2 ab
      character*255 herr
      dimension z(ncmax),ybub(ncmax),xdew(ncmax),x(ncmax),y(ncmax)
      call ABFL2 (a,b,z,kq,ksat,ab,
     &            tbub,tdew,pbub,pdew,Dlbub,Dvdew,ybub,xdew,
     &            t,p,Dl,Dv,x,y,q,ierr,herr)
      end
c ======================================================================
      subroutine DBFL2dll (d,b,z,kq,ab,t,p,Dl,Dv,x,y,q,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*2 ab
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call DBFL2 (d,b,z,kq,ab,t,p,Dl,Dv,x,y,q,ierr,herr)
      end
c ======================================================================
      subroutine TQFLSHdll (t,q,z,kq,p,D,Dl,Dv,x,y,e,h,s,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call TQFLSH (t,q,z,kq,p,D,Dl,Dv,x,y,e,h,s,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine PQFLSHdll (p,q,z,kq,t,D,Dl,Dv,x,y,e,h,s,cv,cp,w,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension z(ncmax),x(ncmax),y(ncmax)
      call PQFLSH (p,q,z,kq,t,D,Dl,Dv,x,y,e,h,s,cv,cp,w,i,herr)
      end
c ======================================================================
      subroutine CSTARdll (t,p,v,x,cs,ts,Ds,ps,ws,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call CSTAR (t,p,v,x,cs,ts,Ds,ps,ws,ierr,herr)
      end
c ======================================================================
      subroutine CCRITdll (t,p,v,x,cs,ts,Ds,ps,ws,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call CSTAR (t,p,v,x,cs,ts,Ds,ps,ws,ierr,herr)
      end
c ======================================================================
      subroutine FPVdll (t,rho,p,x,f)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call FPV (t,rho,p,x,f)
      end
c ======================================================================
      subroutine CP0dll (t,x,cp)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      cp=CP0(t,x)
      end
c ======================================================================
      subroutine TRNPRPdll (t,rho,x,eta,tcx,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call TRNPRP (t,rho,x,eta,tcx,ierr,herr)
      if (tcx.gt.1.d50) tcx=0             !Avoid NaN (not a number)
      if (eta.gt.1.d50) eta=0             !Avoid NaN (not a number)
      end
c ======================================================================
      subroutine INFOdll (icomp,wmm,ttrp,tnbpt,tc,pc,Dc,Zc,acf,dip,Rgas)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      call INFO (icomp,wmm,ttrp,tnbpt,tc,pc,Dc,Zc,acf,dip,Rgas)
      end
c ======================================================================
      subroutine NAMEdll (icomp,hname,hn80,hcas)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*12 hcas,hname
      character*80 hn80
      call NAME (icomp,hname,hn80,hcas)
      end
c ======================================================================
      subroutine XMASSdll (xmol,xkg,wmix)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension xmol(ncmax),xkg(ncmax)
      call XMASS (xmol,xkg,wmix)
      end
c ======================================================================
      subroutine XMOLEdll (xkg,xmol,wmix)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension xmol(ncmax),xkg(ncmax)
      call XMOLE (xkg,xmol,wmix)
      end
c ======================================================================
      subroutine LIMITXdll (htyp,t,D,p,x,tmin,tmax,Dmax,pmax,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*3 htyp
      character*255 herr
      dimension x(ncmax)
      call LIMITX (htyp,t,D,p,x,tmin,tmax,Dmax,pmax,ierr,herr)
      end
c ======================================================================
      subroutine LIMITKdll (htyp,icomp,t,D,p,tmin,tmax,Dmax,pmax,i,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*3 htyp
      character*255 herr
      call LIMITK (htyp,icomp,t,D,p,tmin,tmax,Dmax,pmax,i,herr)
      end
c ======================================================================
      subroutine LIMITSdll (htyp,x,tmin,tmax,Dmax,pmax)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*3 htyp
      dimension x(ncmax)
      call LIMITS (htyp,x,tmin,tmax,Dmax,pmax)
      end
c ======================================================================
      subroutine QMASSdll (qmol,xl,xv,qkg,xlkg,xvkg,wliq,wvap,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension xl(ncmax),xv(ncmax),xlkg(ncmax),xvkg(ncmax)
      call QMASS (qmol,xl,xv,qkg,xlkg,xvkg,wliq,wvap,ierr,herr)
      end
c ======================================================================
      subroutine QMOLEdll (qkg,xlkg,xvkg,qmol,xl,xv,wliq,wvap,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension xl(ncmax),xv(ncmax),xlkg(ncmax),xvkg(ncmax)
      call QMOLE (qkg,xlkg,xvkg,qmol,xl,xv,wliq,wvap,ierr,herr)
      end
c ======================================================================
      subroutine WMOLdll (x, wm)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      wm=WMOL(x)
      end
c ======================================================================
      subroutine DIELECdll (t,rho,x,de)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      dimension x(ncmax)
      call DIELEC (t,rho,x,de)
      end
c ======================================================================
      subroutine SURFTdll (t,rho,x,sigma,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call SURFT (t,rho,x,sigma,ierr,herr)
      end
c ======================================================================
      subroutine EXCESSdll (t,p,x,kph,rho,vE,eE,hE,sE,aE,gE,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call EXCESS (t,p,x,kph,rho,vE,eE,hE,sE,aE,gE,ierr,herr)
      end
c ======================================================================
      subroutine SURTENdll (t,rhol,rhov,xl,xv,sigma,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension xl(ncmax),xv(ncmax)
      call SURTEN (t,rhol,rhov,xl,xv,sigma,ierr,herr)
      end
c ======================================================================
      subroutine PDFL1dll (p,rho,x,t,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call PDFL1 (p,rho,x,t,ierr,herr)
      end
c ======================================================================
      subroutine PHFL1dll (p,h,x,kph,t,D,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call PHFL1 (p,h,x,kph,t,D,ierr,herr)
      end
c ======================================================================
      subroutine PSFL1dll (p,s,x,kph,t,D,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call PSFL1 (p,s,x,kph,t,D,ierr,herr)
      end
c ======================================================================
      subroutine SETKTVdll (icomp,jcomp,hmodij,fij,hfmix,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (nmxpar=6)
      character*3 hmodij
      character*255 hfmix,herr
      dimension fij(nmxpar)
      call SETKTV (icomp,jcomp,hmodij,fij,hfmix,ierr,herr)
      end
c ======================================================================
      subroutine GETKTVdll (icomp,jcomp,hmodij,fij,hfmix,hfij2,hbinp,
     &                      hmxrul)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (nmxpar=6)
      character*3 hmodij
      character*8 hfij(nmxpar)
      character*255 hfmix,hmxrul,hbinp,hfij2
      dimension fij(nmxpar)
      call GETKTV (icomp,jcomp,hmodij,fij,hfmix,hfij,hbinp,hmxrul)
      hfij2=hfij(1)//hfij(2)//hfij(3)//hfij(4)//hfij(5)//hfij(6)
      end
c ======================================================================
      subroutine GETFIJdll (hmodij,fij,hfij2,hmxrul)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (nmxpar=6)
      character*3 hmodij
      character*8 hfij(nmxpar)
      character*255 hmxrul,hfij2
      dimension fij(nmxpar)
      call GETFIJ (hmodij,fij,hfij,hmxrul)
      hfij2=hfij(1)//hfij(2)//hfij(3)//hfij(4)//hfij(5)//hfij(6)
      end
c ======================================================================
      subroutine MELTTdll (t,x,p,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call MELTT (t,x,p,ierr,herr)
      end
c ======================================================================
      subroutine MLTH2Odll (t,p1,p2)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      call MLTH2O (t,p1,p2)
      end
c ======================================================================
      subroutine MELTPdll (p,x,t,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call MELTP (p,x,t,ierr,herr)
      end
c ======================================================================
      subroutine SUBLTdll (t,x,p,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call SUBLT (t,x,p,ierr,herr)
      end
c ======================================================================
      subroutine SUBLPdll (p,x,t,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call SUBLP (p,x,t,ierr,herr)
      end
c ======================================================================
      subroutine PREOSdll (i)
      implicit integer (i-n)
      call PREOS (i)
      end
c ======================================================================
      subroutine SETAGAdll (ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call SETAGA (ierr,herr)
      end
c ======================================================================
      subroutine GERG04dll (nc,iflag,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      character*255 herr
      call GERG04 (nc,iflag,ierr,herr)
      end
c ======================================================================
      subroutine DOTFILLdll (x,ptest,filrat,ierr,herr)
      implicit double precision (a-h,o-z)
      implicit integer (i-n)
      parameter (ncmax=20)
      character*255 herr
      dimension x(ncmax)
      call DOTFILL (x,ptest,filrat,ierr,herr)
      end
