c-----------------------------------------------------------------------
c  Begin file COMTRN.INC
c-----------------------------------------------------------------------

c..Transport equations--------------------------------------------------
      character*3 hetahc,htcxhc,hetacr,htcxcr,htcxcrecs,hmdeta,hmdtcx
      common /HTRNS/  hetahc(nq),htcxhc(nq),               !Pointer to hardcoded models
     &                hetacr(nq),htcxcr(nq),htcxcrecs(nq)  !Pointer to critical enhancement auxiliary functions

c..Dilute gas
      common /OMG/    comg  (nq,mxomg,2),comg2(nq,mxomg,2),
     &                cEuck (nq,mxtrn,4),
     &                ntomg (nq),icomg (nq,mxomg),
     &                ntomg2(nq),icomg2(nq,mxomg),
     &                hmdeta(nq),hmdtcx(nq)

c..Viscosity------------------------------------------------------------
      common /VIS/    ceta   (nq,mxeta,4),                 !Coefficients of the viscosity model
     &                Tmeta  (nq),Txeta  (nq),Pxeta  (nq),
     &                Dxeta  (nq),sigmav (nq),epskv  (nq),
     &                ChEns1 (nq),ChEns2 (nq),
     &                Trddge (nq),etadge (nq),             !Limits and reducing parameters
     &                TredB2 (nq),etarB2 (nq),
     &                Tredeta(nq),Dredeta(nq),etared (nq),
     &                Trddg3 (nq),etadg3 (nq),
     &                etacal,     ieta   (nq,mxeta),
     &                ndgeta (nq),ndgeta2(nq),ndgeta3(nq), !Numbers of terms for the various parts of the model:  dilute gas,
     &                ndgchg (nq),ndgnm3 (nq),ndgdn3 (nq), ! second viscosity virial (initial density dependence), residual part
     &                nB2eta (nq),ndel0  (nq),ndgye  (nq),
     &                npoly  (nq),nnumeta(nq),ndeneta(nq),
     &                nexpn  (nq),nexpd  (nq),nchg5  (nq),
     &                nbknm3 (nq),nbkdn3 (nq)
      common /TRNFT/  ASftm(nx,0:4),BSftm(nx,0:4),         !Common storing residual coefficients to the FT visc model
     &                CSftm(nx,0:4),ABftm(nx,0:4),
     &                BBftm(nx,0:4),CBftm(nx,0:4),
     &                DBftm(nx,0:4),EBftm(nx,0:4)

c..Thermal conductivity-------------------------------------------------
      common /TCX/    ctcx  (nq,mxtcx,4),                  !Coefficients of the thermal conductivity model
     &                Tmtcx (nq),Txtcx (nq),Pxtcx (nq),
     &                Dxtcx (nq),Trddgt(nq),tcxdgt(nq),
     &                Trdbkt(nq),Drdbkt(nq),tcxbkt(nq),
     &                itcx  (nq,mxtcx),
     &                ndgnum(nq),ndgden(nq),               !Numbers of terms for the various parts of the model:
     &                nbknum(nq),nbkden(nq)                !Numerator and denominator for dilute gas and background parts.

c..Thermal conductivity critical enhancement----------------------------
      common /TCXCRT/ ctcke  (nq,mxtck,5),ctck(nq,mxtck,5),!Coefficients of the model
     &                Tmtck  (nq),Txtck  (nq),
     &                Pxtck  (nq),Dxtck  (nq),
     &                Trtck  (nq),Drtck  (nq),Prtck (nq),
     &                tcxred (nq),Tredex (nq),Dredex(nq),
     &                itck   (nq,mxtck,0:5),
     &                nnumtck(nq),ndentck(nq),             !Numbers of terms for the various parts of the model: polynomial (numerator & denominator),
     &                nexptck(nq),nsparek(nq)              ! exponential, spare.  The "CO2" terms are stored in the numerator area

c..ECS transport--------------------------------------------------------
      common /ECSTRN/ cpsi  (nq,mxtrn,4),cchi(nq,mxtrn,4),
     &                xecsljs (nx,nx),xecslje(nx,nx),xecskij (nx,nx),
     &                xecslij (nx,nx),xecsaji(nx,nx),xecskijk(nx,nx),
     &                xecslijk(nx,nx),xecsdij(nx,nx),xecsdij2(nx,nx),
     &                Tmecst(nq),Txecst (nq),Pxecst(nq),
     &                Dxecst(nq),sigtrn (nq),epsktr(nq),   !iLJflag:  flag for L-J parameters (if 0, estimate)
     &                fcecs (nq),nEuck  (nq),iLJflag(nq),  !nEuck:  factor f_int in Eucken correlation
     &                npsi1 (nq),npsi2  (nq),npsi3 (nq),   !npsi (viscosity shape factor):  polynomial term, 2nd poly, spare
     &                nchi1 (nq),nchi2  (nq),nchi3 (nq)    !nchi (conductivity shape factor):  polynomial term, 2nd poly, spare

c..Parameters for Chung method------------------------------------------
      common /CHUNG/  cctcx (nx,mxtcxc,mtcxr),
     &                cceta (nx,mxetac,metar),
     &                acchk (nx),ddipk (nx),sigchk(nx),
     &                epschk(nx),acchv (nx),ddipv (nx),
     &                hbkk  (nx),hbvk  (nx),
     &                sigchv(nx),epschv(nx),tcx0ch,
     &                naddv (nx),naddk (nx)

c
c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                    End file COMTRN.INC
c ======================================================================
