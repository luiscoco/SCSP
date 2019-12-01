c  Begin file FLSH_SUB.FOR
c
c  This file contains iterative flash routines that call the
c    intermediate-level routines.
c
c  The subroutines that are of interest in this file are:
c     subroutine ABFL1 (ab,a,b,z,kph,iGuess,Dmin,Dmax,T,P,D,ierr,herr)
c     subroutine ABFL2 (ab,a,b,z,kq,ksat,Tbub,Tdew,Pbub,Pdew,
c    &                  Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,q,ierr,herr)
c     subroutine TPRHO (T,P,z,kph,kguess,D,ierr,herr)
c     subroutine TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
c     subroutine TPFLSH (T,P,z,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c     subroutine TPDFLSH (T,P,D,z,iFlag,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c     subroutine LIQSPNDL (T,x,D,ierr,herr)
c     subroutine VAPSPNDL (T,x,D,ierr,herr)
c
c  Many other routines are contained in this file, but are internal
c    routines to the those listed above.  Additionally all of the old
c    routines from versions 9.1 and earlier such as PHFLSH, PHFL1,
c    PHFL2, etc. are contained here, but they are no longer necessary
c    since the routine ABFLSH now calls all of these and is the preferred
c    method for performing flash calculations.  TPRHO, TPFL2, and TPDFLSH
c    are listed above because of their importance, but can also be
c    called through the ABFLSH routine.
c
c ======================================================================
c ======================================================================
c
      subroutine ABFL1 (ab,a,b,z,kph,iGuess,Dmin,Dmax,T,P,D,ierr,herr)
c
c  General single-phase flash routine given two inputs and composition.
c  Valid input properties are temperature, pressure, density, energy,
c  enthalpy, or entropy.  The character string ab specifies the inputs,
c  which can be T, P, D, E, H, S.  An input of 'EH' (or 'HE') is
c  not supported.  The letters in this string must be uppercase.
c
c  Care must be taken when sending inputs of T, P, or D, so that the same
c  variable is not sent twice.   For example, the following would be wrong::
c
c      call ABFL1 ('TH',T,H,z,kph,0,0,Dmin,Dmax,T,P,D,ierr,herr)
c
c  Rather, the following are examples of correct inputs::
c
c      call ABFL1 ('TH',T,H,z,kph,0,0,Dmin,Dmax,tt,P, D,ierr,herr)
c      call ABFL1 ('TP',T,P,z,kph,0,0,Dmin,Dmax,tt,pp,D,ierr,herr)
c      call ABFL1 ('DS',D,S,z,kph,0,0,Dmin,Dmax,T, P,dd,ierr,herr)
c
c  This routine accepts only single-phase inputs, it is intended primarily
c  for use with the more general flash routine ABFLSH, but can be called
c  independently for increased calculation speed if the inputs are
c  know to be single-phase.  This will avoid the call to the flash routines
c  to determine the phase of the inputs.  If this routine is called, but
c  the inputs are 2-phase, either an incorrect root or a metastable state
c  will be returned (which is OK if the metastable state is desired).
c
c  Inputs:
c        a--First property (either temperature, pressure, density, entropy)
c        b--Second property (pressure, density, energy, enthalpy, or entropy)
c        Possible inputs for these two variables are
c             T--Temperature [K]
c             P--Pressure [kPa]
c             D--Molar density [mol/L]
c             h--Enthalpy [J/mol]
c             e--Energy [J/mol]
c             s--Entropy [J/mol-K]
c        z--Composition (array of mole fractions)
c      kph--Phase flag:  1 - Liquid  (kph is only needed for TP inputs)
c                        2 - Vapor
c   iGuess--When set to one, the density is used as the initial guess
c           for P or S inputs
c       ab--Character*2 string defining the inputs, e.g., 'TH' or 'PS'
c           Valid characters are T, P, D, E, H, S
c     Dmin--Lower bound on density [mol/L] (for T inputs)
c     Dmax--Upper bound on density [mol/L] (for T inputs)
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        D--Molar density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      248 - Single-phase iteration did not converge
c     herr--Error string (character*255)
c
c  rewritten by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-18-01 EWL, original version, rewritten by combining code of McLinden
c  09-15-11  MK, add additional code for Reguli-falsi
c  11-02-16 EWL, rewrite code for version 10 of Refprop, and allow any properties as inputs

      include 'COMMONS.INC'
      character at*1,bt*1,ab*2,ab2*2,herr*255,herr1*255
      dimension z(ncmax),y(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ABFL1

      tolr=1d-12
      itmax=50
      imethod=-1
      ipassx=1
      Tt=T
      v3=0
      Dmin2=Dmin
      Dmax2=Dmax
      dhdd_s=1d0
      call GETINPUTS (ab,ab2,at,bt,iat,ibt,a,b,aa,bb,T,Dd,P,ierr,herr)
      if (ierr.gt.0) RETURN
      if (iGuess.ne.1) D=Dd
      if (iGuess.ne.0 .and. iat.ne.iLetterT) T=Tt
      if (Dmin2.lt.0.) Dmin2=0d0
      if (Dmax2.lt.0.) Dmax2=100d0
      if (Dmin2.gt.1000.) Dmin2=0d0
      if (Dmax2.gt.1000.) Dmax2=1000d0


c  Check for inputs of only T, P, D, or any input with D, and call appropriate routine.
      if (ab2.eq.'TD') then            !Temperature-density inputs
        call PRESS (T,D,z,P)
        RETURN
      elseif (ab2.eq.'TP') then        !Temperature-pressure inputs, call TPRHO without a guess value for D.
        call TPRHO (T,P,z,kph,0,D,ierr,herr)
        RETURN
      elseif (ab2.eq.'DP') then        !Pressure-density inputs
        call DBFL1 (D,P,z,'DP',T,pp,ierr,herr)
        RETURN
      elseif (iat.eq.iLetterD) then    !Density input with either E, H, or S
        call DBFL1 (D,bb,z,ab2,T,P,ierr,herr)
        RETURN
      endif

      iErrNum=iErrNum+1
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS ('EOS',z,Tmin,Tmax,Dmx,Pmax)
 50   call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.
      iw=iFluidID(icomp)
      D2=Dc
      b1=1d0
      T1=300d0
      if(ibt.eq.iLetterH.or.ibt.eq.iLetterS.or.ibt.eq.iLetterE)b1=1000d0
      if (ABS(bb).gt.b1) b1=bb
      aj1=0d0

c  Temperature input
      if (iat.eq.iLetterT) then
        itmax=200
        v(6)=Dmin2
        v(7)=Dmax2
        D=Dmin2
        if (Dmin2.le.0. .and. ab2.eq.'TS') then
          D=1d-20            !Avoid infinite entropy at zero density.
          v(6)=D
        endif
        do i=1,2
          if (i.eq.2) D=Dmax2
          call HESCALC (ibt,T,D,z,bj)                      !Calculate the states at Dmin2 and Dmax2.
          v(2)=D
          f(2)=bb-bj
          ierr=0
          if (ABS(f(2)/b1).lt.tolr) goto 500               !Check if it's the actual state.
          call ROOTSEARCH (1,0,0,v,f,v3,0,ib,lneg,lpos)    !Store calculated values.
        enddo
        D =0.7d0*Dmin2+0.3d0*Dmax2
        D2=0.5d0*Dmin2+0.5d0*Dmax2                         !Second guess on density.

c  Pressure input
      elseif (iat.eq.iLetterP) then
        call HESCALC (ibt,Tc,Dc,z,bcrit)
        if (iGuess.ne.1) then
         if (aa.lt.Pc/20 .and. b.gt.bcrit .and.
     &      (ibt.eq.iLetterH .or. ibt.eq.iLetterE)) then
            T=300d0                    !For low pressures, call ROOTFIND to get an ideal gas temperature at the input
            D=0d0                      ! enthalpy or energy.  Then use that to get an ideal gas density as the starting point.
            call ROOTFIND (' ',ibt,0,b,z,2,0,1,0,0,0,T,D,o,ierr,herr)
            D=aa/Rxgas/T
            imethod=1
          elseif (icomp.ne.0 .and. iAnc(icomp).eq.0) then
            D=Dc
            if (bb.lt.bcrit .and. kph.ne.2) then           !All states with b<bc (including high pressure).
              call TLSATHES (ibt,bb,z,T,D,ierr,herr)       !Find the temperature at the input H, E, or S from the ancillary equations and EOS.
              if (D.le.0) D=Dc
            elseif (aa.lt.Pc) then                         !All other states with P<Pc (i.e., vapor phase).
              call TSATP (aa,z,T,ierr,herr)
              call DVSATT (T,z,D,ierr,herr)
            elseif (XDIV(ABS(bb-bcrit),bcrit).lt.0.2d0) then
              D=Dc*1.5d0     !Enthalpy is in the vicinity of the critical point, so density is probably higher than Dc.
            endif
          else
            call HESCALC (ibt,Tmin,Dmx,z,bmina)            !Rough estimate of the minimum value of E, H, or S.
            D=Dc*5d0                   !Set initial guess.
            if (D.gt.Dmx) D=Dmx
            if (bb.gt.bcrit .and. P.lt.Pc) D=Dc/5d0        !Check for liquid or vapor phase.
            if (P.gt.Pc) D=Dc*3d0
            bx=XDIV(bb-bmina,bcrit-bmina)
            if (bb.lt.bcrit.and.bx.gt.0.and.bx.lt.1)D=(1d0-bx)*Dmx+bx*Dc
          endif
        elseif(aa.lt.Pc/20.and.(ibt.eq.iLetterH.or.ibt.eq.iLetterE).and.
     &          kph.eq.2) then
          T=300d0            !For low pressures, call ROOTFIND to get an ideal gas temperature at the input
          D=0d0              ! enthalpy or energy.  Then use that to get an ideal gas density as the starting point.
          call ROOTFIND (' ',ibt,0,b,z,2,0,1,0,0,0,T,D,o,ierr,herr)
          D=aa/Rxgas/T
          imethod=1
        endif
        if (imethod.eq.-1) then
          call DHD1 (T,D,z,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,dhdp_d)          !dH/dP at constant temperature is generally negative, except in the liquid near the triple point.
          if (dhdp_t.gt.-1 .or. P.gt.Pc) imethod=1                             !  At that point, the slope becomes too steep to solve as done with all other inputs, so use D and P as inputs, where D is the unknown variable.
          if (icomp.gt.0 .and. D.gt.45.) then
            if (iFluidID(icomp).eq.1 .or. iFluidID(icomp).eq.3)imethod=1
          endif
        endif
        D2=D*1.01d0          !Set value for second iteration.
        if (bb.gt.bcrit) D2=D*0.9d0

c  Entropy input
      elseif (iat.eq.iLetterS) then
        if (ibt.eq.iLetterE) goto 601
        if (iGuess.ne.1) then
          call ENTRO (Tc,Dc,z,scrit)
          D=Dc*4d0
          if (aa.gt.scrit) D=Dc
          if (Tc.lt.100.) T1=Tc*2d0
        endif
        D2=D*1.1d0
      endif

c  Start of main loop.
 100  ir=1
      isu=1
      f2old=1d20
      do 300 it=1,itmax
        call TOLRNC (tolr,it,10,13,16,21,30)     !Loosen tolerance if not converging.
        if (ABS(f(2)).lt.0.5d0*f2old) isu=isu+1
        if (ABS(f(2)).gt.5d0*f2old) isu=0
        f2old=ABS(f(2))
        if (iat.eq.iLetterT) then
          if (INT(it/10)*10.eq.it) then          !Use bisection method(s) every 10th iteration to help with double values for TH inputs.
            ib=INT(it/10)
            if (ib.gt.10) ib=ib-10
            call ROOTSEARCH (7,0,0,v,f,D,it,ib,lneg,lpos)
            if (INT(it/50)*50.eq.it) then        !On each 50th iteration, scrap the upper and lower limits, and let it find new limits.
              lpos=.false.
              lneg=.false.
            endif
          endif
        elseif (iat.eq.iLetterP) then
          Dstrt=D
          if (it.eq.35 .and. isu.lt.5) then      !Reset bounds if not converging.
            D=1d-4
            lpos=.false.
            lneg=.false.
          endif
          jx=0
          do j=1,10
            do j1=1,3
            if (imethod.ge.0) call DBFL1 (D,bb,z,'D'//bt,T,bj,ierr,herr)       !Call with inputs of D and P when the input state is deep in the liquid.  In some situations, can also be E, H, or S.
            if (imethod.lt.0) call DBFL1 (D,aa,z,'D'//at,T,bj,ierr,herr)       !Call with inputs of D and either P, E, H, or S.  bj is the iterative variable.
            if (ierr.eq.207) goto 500                                          !Density is zero and inputs are DP, so there is no solution.
            if (ierr.lt.100) call CHECKSTATE (0,0,T,D,z,Err,ierr,herr)
            if (ierr.eq.702 .or. ierr.eq.703) then
              if (j1.lt.3) D=D*2d0
              Dstrt=Dstrt*1.05d0
            else
              EXIT
            endif
            enddo
            if (ierr.lt.100 .or. (ierr.eq.708 .and. T.gt.1500)) EXIT           !CO2+Water can trip the 708 error at high temperatures.
            if (ierr.eq.707 .and. (iw.eq.1 .or. iw.eq.3)) EXIT                 !Water can do opposite of other fluids
            if (ib.eq.10) then
              lneg=.false.
              lpos=.false.
              f(4)=-1d20
              f(5)= 1d20
              ib=1
            endif


            if (lneg .and. lpos) then
              call ROOTSEARCH (6,0,0,v,f,D,it,ib,lneg,lpos)          !Use bisection to get next guess.
            elseif (j.eq.1 .and. isu.gt.2) then
              D=(D+v(2))/2d0
            elseif (j.eq.4) then
              D=(Dstrt+v(2))/2d0
            elseif (j.eq.7 .and. P.lt.Pc/1000d0) then      !Try a really low density for very low pressures.
              D=D/1d4
            elseif (j.eq.8 .and. P.lt.Pc/10d0) then        !Try a high density for low pressures.
              D=Dc*3d0
            else
              isu=isu-2
              if (isu.lt.0) isu=0
              D=Dstrt*(1d0+0.1d0*DBLE(j)/DBLE(isu+1))      !When in an unstable region, bounce D back and forth with larger and larger swings until a root is found.
              if(D.ge.Dc.and.INT(j/2)*2.eq.j)
     &           D=Dstrt*(1d0-0.1d0*DBLE(j)/DBLE(isu+1))
              if(D.lt.Dc.and.INT(j/2)*2.eq.j)D=Dstrt/10**(DBLE(j)/4d0)
              if(D.lt.Dc.and.kph.eq.1)           !Check for vapor root when liquid phase has been specified.
     &           D=Dc*(1d0+0.2d0*DBLE(j)+0.05d0*DBLE(it))
            endif

!            if (lneg .and. lpos) then
!              call ROOTSEARCH (6,0,0,v,f,D,it,ib,lneg,lpos)         !Use bisection to get next guess.
!            elseif (j.eq.1 .and. isu.gt.2) then
!              D=(D+v(2))/2d0
!            elseif (j.eq.4 .and. v(2).ne.0.) then
!              D=(Dstrt+v(2))/2d0
!            elseif (j.eq.7 .and. P.lt.Pc/1000d0) then     !Try a really low density for very low pressures.
!              D=D/1d4
!            elseif (j.eq.8 .and. P.lt.Pc/10d0) then       !Try a high density for low pressures.
!              D=Dc*3d0
!            else
!              jx=jx+1
!              isu=isu-2
!              if (isu.lt.0) isu=0
!              D=Dstrt*(1d0+0.05d0*DBLE(jx)/DBLE(isu+1))   !When in an unstable region, bounce D back and forth with larger and larger swings until a root is found.
!              if (INT(jx/2)*2.eq.jx) then
!                if (D.ge.Dc) then
!                  D=Dstrt*(1d0-0.05d0*DBLE(jx)/DBLE(isu+1))
!                else
!                  D=Dstrt/10**(DBLE(jx)/4d0)
!                endif
!              endif
!              if (D.lt.Dc.and.kph.eq.1)         !Check for vapor root when liquid phase has been specified.
!     &           D=Dc*(1d0+0.2d0*DBLE(jx)+0.05d0*DBLE(it))
!            endif



          enddo
          if (j.gt.10) D=Dstrt
        elseif (iat.eq.iLetterS) then
          call DBFL1 (D,aa,z,'DS',T,P,ierr,herr)
          call DHD1 (T,D,z,dhdt_d,dhdt_p,dhdd_t,dhdd_p,dhdp_t,dhdp_d)
          call DSD1 (T,D,z,dsdt_d,dsdt_p,dsdd_t,dsdd_p,dsdp_t,dsdp_d)
          dhdd_s=dhdd_t-XDIV(dhdt_d*dsdd_t,dsdt_d)         !Calculate the change in enthalpy with respect to density at constant entropy.
          if (ierr.lt.100 .and. dhdd_s.lt.0.)  ierr=999    !d(h)/d(D)_s must be positive for a real root.
          if (ierr.lt.100 .and. T/T1.lt.0.2d0) ierr=999
          if (ierr.ge.100) then
            if (D.lt.dc*1.5d0) then
              D=D*0.5d0      !Decrease the density in the vapor.
            elseif (it.le.2 .or. v(2).eq.0. .or. v(1).lt.v(2)) then
              D=D*1.2d0      !Increase the density in the liquid.
            else
              D=(v(1)+v(2))/2d0        !Go back to 50% of old value if DBFL1 failed.
            endif
            if (D.gt.100.) D=Dc*2d0
            i=1
            if (INT(it/2)*2.eq.it) i=-1
            D=D*(1d0+DBLE(i*it)/100d0)           !Make D bounce around faster and faster.
            goto 300
          else
            T1=T
          endif
        endif

        v(2)=D
        if (imethod.ge.0) then
          if (iat.ne.iLetterP) call HESCALC (iat,T,D,z,bj)
          f(2)=aa-bj
        else
          if (ibt.ne.iLetterP) call HESCALC (ibt,T,D,z,bj)
          f(2)=bb-bj
        endif
        if (ierr.ne.208 .or. T.ne.300d0) then
        ierr=0
        if (ABS(f(2)/b1).lt.tolr .and. it.gt.1) goto 500             !------------Iteration has converged------------
        if (ABS(f(2)/b1).lt.tolr*1.d3.and.d.lt.1d-4.and.it.gt.1)goto 500       !Assume converged when density is extremely small.
        if (ABS(f(2)/b1).lt.tolr*1.d6.and.d.lt.1d-6.and.it.gt.1)goto 500       !Assume converged when density is extremely small.
        endif
        call ROOTSEARCH (ir,0,0,v,f,D,it,ib,lneg,lpos)     !Update Reguli-Falsi values and get next guess with the Secant method.
        ir=1234
        if (it.eq.1) D=D2
        if (it.gt.4 .and. iat.eq.iLetterS .and. ibt.eq.iLetterH) then          !Use Newton's method for input entropy calculations.
          D2=v(2)
          if (D.gt.Dc*2d0 .and. T.lt.Tc) then
            D=D2-(bj-bb)/dhdd_s                  !In the liquid phase, use Dinc=d(D)/d(H)*delta(H)
          else
            Dx=D2-EXP1(XLOG(D2)-(bj-bb)/dhdd_s/D2)         !In the vapor phase, use the log of D to help convergence.
            if (ABS(Dx/D2).gt.0.99d0) Dx=SIGN(0.99*D2,Dx)
            D=D2-Dx
          endif
        endif
        if (D.gt.10000.) D=D/100.d0
 300  enddo
      if (iat.eq.iLetterP) then
        imethod=-2*imethod
        if (ABS(imethod).le.2) goto 50
      endif

c   ---------Error Messages---------
      oa=1d0
      ob=1d0
      if (iat.eq.iLetterP) oa=1000d0
      if (ibt.eq.iLetterP) ob=1000d0
      call ERRNUM (248,0,ab2,' ',a/oa,b/ob,0d0,ierr,herr)

c  Finalize routine.
 500  if (iat.ne.iLetterP .and. ibt.ne.iLetterP) call PRESS (T,D,z,P)
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS (ab2,T,D,P,z,ierr,herr)
      if (iat.eq.iLetterP .and. icomp.ne.0. and. ipassx.eq.1) then    !Check if D>Dmax2 and dPdD<0, meaning that the EOS has two liquid roots and the wrong one was found.
        if (D.gt.DxEOS(icomp)) then
          call DPDDX (T,D,z,dpdd)
          if (dPdD.lt.0) then
            ipassx=2
            bh=b
            bs=b
            j=4
            if (ibt.eq.iLetterS) j=5
            if (ibt.eq.iLetterE) j=6
            call SATGUESS (1,j,0,0,z,T,pp,D,bh,bs,dd,y,ierr1,herr1)  !Use ancillaries to find the liquid root.
            call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)            !Initialize root finding variables.
            if (ierr1.le.0) goto 100
          endif
        endif
      endif
      RETURN

 601  iErrNum=iErrNum-1
      call ERRNUM (601,0,'ABFL1',ab,0d0,0d0,0d0,ierr,herr)

      end                                               subroutine ABFL1
c
c ======================================================================
c
      subroutine TPRHO (T,P,z,kph,kguess,D,ierr,herr)
c
c  Iterate for density as a function of temperature, pressure, and
c  composition of a specified phase.
c
c  .. warning:
c
c      Invalid densities will be returned for T and P outside the range of
c      validity, e.g., P>Pmelt, P<Psat for kph=1, etc.
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        z--Composition (array of mole fractions)
c      kph--Phase flag:
c           1 - Liquid phase
c           2 - Vapor phase
c           0 - Stable phase - NOT ALLOWED (use TPFLSH)
c               (Unless an initial guess is supplied for D)
c          -1 - Force the search in the liquid phase (for metastable points)
c          -2 - Force the search in the vapor phase (for metastable points)
c   kguess--Guess flag:
c           0 - No first guess for D provided
c           1 - First guess for D provided
c        D--Initial guess for molar density [mol/L] if kguess = 1
c
c  Outputs:
c        D--Molar density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      201 - Illegal input (kph <= 0)
c                      202 - Liquid-phase iteration did not converge
c                      203 - Vapor-phase iteration did not converge
c     herr--Error string (character*255)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  04-03-95  MM, original version
c  10-01-09 EWL, change 0.1d0 to 0.11d0 to avoid D=D2, resulting in unreported errors
c  04-27-10 EWL, add check for d(P)/d(D)/d(T)<0
c  07-29-14 EWL, add check for D>Dmax after check for d2PdTD<0 since d2PdTD can go negative for water in the liquid
c  06-02-15 EWL, add check for D>100
c  10-14-15 EWL, call DERVPVT instead of THERM2 to avoid recursive calls with reactive mixtures
c  03-30-16 EWL, shorten code and remove code that is no longer in use in all routines in FLSH_SUB.FOR
c                remove comments about changes before the year 2009

      include 'COMMONS.INC'
      character*255 herr,herr1
      dimension z(ncmax),y(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TPRHO

c  Check for ideal gas state
      if (P.le.1d-100 .or. (P.lt.1d-8 .and. kph.ne.1)) then          !Nearly all pure fluids have vapor pressures greater than this at the triple point.
        D=0d0      !Return zero for liquid
        if (kph.ne.1 .and. T.gt.0.) D=P/(Rxgas*T)
        RETURN
      endif
      if (ABS(D).le.1d-100) D=0d0

      ierr=0
      herr=' '
      itmax=40
      iErrNum=iErrNum+1
      if (.not.lRcnst .or. .not.lRcnstWag) call RMIX (z)
      call ISPURE (z,icomp)
      call CRITP (z,Tc,Pc,Dc,ierr1,herr1)
      call ROOTSEARCH (0,0,0,v,f,D,0,ib,lneg,lpos)         !Initialize root finding variables.
      call REDX (z,T0,D0)
      tolr=1d-8
      v3=0
c     if (icomp.gt.0) tolr=1d-12
      if (T.le.0d0) goto 203
      RT=Rxgas*T

c  Call Peng-Robinson routines if in use.
      if (iEOSmx.eq.iEOSPR .or. iEOSmx.eq.iEOSPR3) then
        Din=D
        call TPRHOPR (T,P,z,D1,D2)
        if (D1.gt.1d8) goto 202
        D=D1
        if (ABS(Din-D1).gt.ABS(Din-D2) .and. ABS(kph).ne.1) D=D2     !Determine which root to use.
        if (ABS(kph).eq.2) D=D2
        if (D2.lt.1d-12) D=D1
        if (P.gt.1. .or. kph.eq.2) goto 400      !If the pressure is greater than 1 kPa or the density is in the vapor phase, calculations are finished.
        if (kph.eq.1 .and. D.lt.1d-6) D=0d0      !Otherwise, continue with iterative routine to make sure that the cubic solver got the right answer.
      endif

c  Determine phase if possible.
      lliq=.false.
      if (kph.eq.1) lliq=.true.        !Use input phase specification to find solution.
      if (kph.eq.-1) then
        lliq=.true.          !User specified to search liquid phase only, don't do more tests.
      elseif (kph.eq.-2) then
        lliq=.false.         !User specified to search vapor phase only, don't do more tests.
      elseif (P.lt.0.99d0*Pc*(1d0+7d0*(T/Tc-1d0)) .and. T.gt.Tc) then          !Approximates the pressure along the critical isochore
        lliq=.false.         !Vapor phase iteration for low densities          ! at supercritical states to aid near-critical convergence.
      elseif (P.ge.Pc*1.01d0 .or. T.gt.Tc) then
        lliq=.true.          !Liquid phase iteration for P>Pc and T>Tc
      elseif (kph.le.0) then
        if (kguess.ne.0) then
          lliq=.true.        !Initial guess supplied.
          if (D.lt.Dc) lliq=.false.
        else
          goto 201
        endif
      endif

c  Set initial guess for density.
      kph1=2
      if (lliq) kph1=1
      if (kph.eq.-2) then
        Dinit=P/RT           !User requested vapor phase only, use ideal gas as initial starting point.
      elseif (kguess.eq.1 .and. D.gt.0.) then
        Dinit=D              !Use input density as initial guess.
      elseif (T.gt.Tc) then
        pp=0.99d0*Pc*(1d0+7d0*(T/Tc-1d0))        !Approximates the pressure along the critical isochore.
        Dinit=Dc*P/pp/(1d0+(P-pp)/pp*0.35d0)     !Approximates the density with the use of the critical isochore and its pressure. (equation of EWL, Jan. 20, 2017).
      elseif (P.lt.Pc*0.8d0 .and. kph1.eq.2) then
        Dinit=P/RT           !Use the ideal gas pressure when in the gas phase.
      else
        Dsat=0d0
        Dinit=0d0
        if (icomp.ne.0 .and. T.le.Tc+1d-12) then           !For a pure fluid, get ancillary values.
          call SATGUESS (kph1,1,0,0,z,T,pp,Dsat,h,s,dd,y,ierr1,herr1)
          if (ABS(kph1).eq.1 .and. Dsat.lt.1.2d0*Dc) Dsat=1.2d0*Dc   !Move initial guess a little bit away from the critical density.
          if (ABS(kph1).eq.2 .and. Dsat.gt.0.8d0*Dc) Dsat=0.8d0*Dc
          if (ierr1.gt.0) Dsat=0d0
        endif
        if (Dsat.gt.0.) then
          Dinit=Dsat         !Use value from ancillary equations when near the critical point or in the liquid phase.
        elseif (lliq) then
          call SATGUESS (1,1,11,0,z,T,pp,Dinit,h,s,Dinit,y,ierr1,herr1)        !Use Rackett technique to get initial guess if ancillaries are not available.
          if (Dinit.lt.1.5d0*Dc) Dinit=1.5d0*Dc
          if (Dinit.lt.2d0*Dc .and. T.lt.0.95d0*Tc) Dinit=2d0*Dc
        else
          Dinit=P/RT
        endif
      endif




c  ---------Begin density iteration with Newton's method---------
c  For liquids (or fluids above the critical pressure), the iteration is carried out in transformed coordinates of log(V).
c  For vapor or supercritical temperatures with P<Pc, iterate in terms of LOG(V) and LOG(P).

      plog=XLOG(P)
      vlog=-XLOG(Dinit)
      tau=T0/T
      do it=1,itmax
        call TOLRNC (tolr,it,10,15,20,25,30)     !Loosen tolerance if not converging.
        if (-vlog.gt.20..or.vlog.gt.100..or.ABS(vlog).lt.1d-20)goto 202
        D=EXP(-vlog)
        Dinv=1d0/D
        del=D/D0
        phi01=PHIX(0,1,tau,del,z)      !Real-gas terms
        phi02=PHIX(0,2,tau,del,z)
        phi03=PHIX(0,3,tau,del,z)
        P2=RT*D*(1d0+phi01)
        P2Inv=1d0/P2
        dPdD=RT*(1d0+2d0*phi01+phi02)
        d2PdD2=RT*Dinv*(2d0*phi01+4d0*phi02+phi03)
        if (ABS(P2).lt.1d-20 .or. ABS(dPdD).lt.1d-20) goto 202
        if (dPdD.gt.-0.1d0 .and. dPdD.lt.1d10) then
c         if (D.gt.Dm2 .and. D.lt.Dc) Dm2=D      !Keep track of the last best guess for density for cases where no root is available.  This point will be near the spinodal.
c         if (D.lt.Dm1 .and. D.gt.Dc) Dm1=D
          f(2)=P-P2
          v(2)=vlog
          call ROOTSEARCH (1,1,0,v,f,v3,0,ib,lneg,lpos)              !Store calculated values for Reguli-Falsi use.
          if (lliq .or. P2.le.0.) then
            d2PdD2=d2PdD2+dPdD*Dinv
            fvdp=(P-P2)/(d2PdD2*(P-P2)*0.5d0/dPdD+dPdD)*Dinv         !2nd order Newton's method
            if (ABS(fvdp).gt.100.) fvdp=(P-P2)/(D*dPdD)              !Revert to 1st order Newton's method if step is too large from 2nd order method.
            if (ABS(fvdp/vlog).gt.0.2d0) fvdp=(P-P2)/(D*dPdD)
          else
            d2PdD2=d2PdD2*P2Inv-(dPdD*P2Inv)**2+dPdD*Dinv*P2Inv
            plog2=plog-LOG(P2)
            fvdp=plog2/(P2*d2PdD2*plog2*0.5d0/dPdD+dPdD*P2Inv)*Dinv  !2nd order Newton's method
            fvdpx=fvdp
            if (ABS(fvdp).gt.1.) fvdp=-plog2*P2/(-D*dPdD)            !Revert to 1st order Newton's method if step is too large from 2nd order method.
            if (ABS(fvdp).gt.2d0*ABS(fvdpx)) fvdp=2d0*fvdpx
            if (ABS(fvdp).gt.25.) fvdp=0.1d-5
          endif
          vlog=vlog-fvdp               !Next guess
          if (ABS(f(4))+ABS(f(5)).lt.1d-10.and.ABS(fvdp).lt.tolr)fvdp=0.       !If not fully converging but the root is tightly bounded, set fvdp to zero to cause the next line to be true.
          if (ABS(fvdp/vlog).lt.1d-4*tolr) then
            if (it.gt.1 .and. ABS(P-P2).lt.0.001d0) then
c  ---Iteration has converged---
              D=EXP1(-vlog)
              goto 400
            endif
          endif
        else
          call ROOTSEARCH (6,1,0,v,f,vlog,it,ib,lneg,lpos)           !Use bisection if dP/dD < 0 to get next guess.
          if (ABS(dPdD).gt.1d10 .and. kph1.eq.1) vlog=vlog*1.2d0
        endif
        call ROOTSEARCH (3,1,0,v,f,vlog,it,ib,lneg,lpos)             !Check if new value is further from solution.  If so, use reguli-falsi.
        if (it.eq.8 .or. it.eq.12 .or. it.eq.18 .or. it.eq.24) then
        if(lneg.and.lpos)call ROOTSEARCH(6,1,0,v,f,vlog,it,ib,lneg,lpos)       !Use bisection to get next guess for it=8, 12, 18, or 24.
        elseif (it.ge.5.or.vlog.lt.-5..or.vlog.gt.10..or.dPdD.le.0.)then
          if (p.gt.pc/5d0 .and. p.lt.pc*5d0 .and. t.lt.tc*1.5d0)
     &        call VFBOUNDS (T,P,z,kph1,v,f,vlog,Tc,Dc,lneg,lpos)
          if (vlog.lt.-5.) vlog=vlog/2d0         !Check for large densities.
          if (vlog.lt.-5.) vlog=vlog/5d0
        endif
      enddo
      if (ABS(kph).eq.2) goto 203
      goto 202

c   ---------Iteration has converged---------


 400  if (T.lt.Tc-1d-6) then
        if (kph.eq.-1 .and. D.lt.Dc) goto 202    !Check for root in wrong phase, and goto error sections if so.
        if (kph.eq.-2 .and. D.gt.Dc) goto 203
      endif
c  If solution is less than Tc and the densities are within a region
c   that is semi-critical, check d(P)/d(D) and d^2(P)/[d(D)*d(T)]
c   to make sure that root is valid.
      if (T.lt.Tc*1.2d0 .and. D.gt.Dc*0.2d0 .and. D.lt.Dc*2d0) then
        call CHECKSTATE (0,0,T,D,z,Err,ierr1,herr1)
        if (ABS(kph).eq.1 .and. ierr1.ge.100) goto 202     !Solution is not valid in the liquid phase.
        if (ierr1.ge.100) goto 203                         !Solution is not valid in the vapor phase.
        if (ierr.eq.0) ierr=ierr1
      endif
 500  iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      RETURN




c   ---------Error Messages---------
 201  call ERRNUM (201,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 202  if (D.lt.Dc) D=Dc
      call ERRNUM (202,0,' ',' ',T,P/1000d0,0d0,ierr,herr)
      goto 500
 203  if (D.gt.Dc) D=Dc
      call ERRNUM (203,0,' ',' ',T,P/1000d0,0d0,ierr,herr)
      goto 500

      end                                               subroutine TPRHO
c
c ======================================================================
c
      subroutine TPFLSH (T,P,z,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given temperature, pressure, and bulk composition.
c  This routine accepts both single-phase and two-phase states as inputs;
c  for single-phase calculations, the subroutine TPRHO is much faster.
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        z--Overall composition (array of mole fractions)
c
c  Outputs:
c        D--Density [mol/L]
c       Dl--Molar density of the liquid phase [mol/L]
c       Dv--Molar density of the vapor phase [mol/L]
c        x--Composition of the liquid phase (array of mole or mass fractions)
c        y--Composition of the vapor phase (array of mole or mass fractions)
c     ierr--Error flag:  0 - Successful
c                      213 - Density calculation did not converge
c                      215 - Supercritical density calculation did not converge
c                      223 - Bubble point did not converge
c                      224 - Dew point did not converge
c     herr--Error string (character*255)
c  (See subroutine ABFLSH for the description of all other output variables.)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  09-11-95  MM, original version
c  02-23-11 EWL, add check for CO2 with d2P/dD/dT < 0 near the critical point
c  02-11-15 EWL, check for open ended phase boundaries
c  10-14-15 EWL, if call to TPFL2 fails, recall it with x and y inputs from saturation routines
c  03-30-16 EWL, shorten and/or remove code that is no longer in use,

      include 'COMMONS.INC'
      character herr*255,herr1*255
      dimension x(ncmax),y(ncmax),z(ncmax),xy(ncmax)
      dimension xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TPFLSH

      if (iTPflg.gt.0) RETURN
      iErrNum=iErrNum+1
      call SETCOMP (icomp,z,x,y)                           !Set output liquid and vapor compositions to input values.
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
c     if (ierr.eq.313 .and. iSpline.eq.1) Tc=Tmax*2d0      !If splines are on and critical point not available, mixtures may be type III, so set Tc very high.
      iFlg1=3      !Do saturation checks as a function of pressure when T>Tc
      call CRITP (z,Tcc,Pcc,Dcc,ierr,herr)       !Call backup critical point if splines are not available.
      if (iSpline.eq.0 .and. T.lt.Tcc) iFlg1=1   !If T<Tc (not expanded Tc from SATMAX with splines off), do saturation checks as a function of temperature.
      kph=1
      Tbub=T
      Tdew=T
      Pbub=P
      Pdew=P
      dd=0d0
      Dv=0.d0
      Dl=0.d0
      q=999d0
      ierr2=0
      ierrx=0
      ierry=0
      if (T.le.0.) goto 143
      if (P.lt.0.) goto 143
      D=P/(Rxgas*T)
      if (P.lt.1d-10) goto 500                   !Check for ideal-gas state.
      if (icomp.ne.0 .and. T.gt.Tc) goto 350     !Checks for pure fluid.
      if (T.gt.Tc .and. P.gt.Pc) goto 350        !For mixtures, if both T>Tc and P>Pc, most likely single phase.
      if (iSpline.ne.0 .and. T.gt.Tc+1)  goto 350
      if (iSpline.ne.0 .and. P.gt.Pc+10 .and. MixTyp.eq.1) goto 350

c  Check for supercritical or single-phase states for pure fluids, or for mixtures when the splines have been turned on.
      if (icomp.ne.0 .or. iSpline.ne.0) then
        call SATGUESS(1,1,0,0,z,Tbub,Pbub,Dl,h,s,Dv,ybub,ierr1,herr)           !Get estimated saturation pressures and densities from ancillaries or splines.
        if (icomp.eq.0 .or. iAnc(icomp).ne.0) then
          call SATGUESS(2,1,0,0,z,Tdew,Pdew,Dv,h,s,dd,xdew,ierr2,herr)
          if (ABS(Dl-Dv).lt.1.d-8) ierr2=999     !Check that the same solution was not found.  Don't change number or name, used below.
        endif
        Pdew=xEq0(Pdew)
        Pbub=xEq0(Pbub)
        if (ierr1.lt.100 .and. ierr2.lt.100) then
          diff=0.01d0
          if (iSpline.ne.0) diff=0.05d0          !Set the difference to 5% for mixtures with the splines turned on.
          if (iAnc(icomp).ne.0) diff=0d0         !Ancillary equations for pseudo-pure fluid are exact.
          if ((P-Pbub)/Pbub.ge.diff) goto 350
          kph=2
          if ((Pdew-P)/Pdew.ge.diff) goto 350
          if ((Pbub-P)/Pbub.gt.diff .and. (P-Pdew)/Pdew.gt.diff)goto 250       !For a mixture, go straight to 2-phase routines if not near the saturation line.
        endif
      endif

c  Check for single-phase vapor (pure or mixture); call dew-point saturation routine to determine phase.
      Pdew=P
      Tdew=T
      kph=2
      toltp=1.0000001d0
      if (icomp.ne.0) toltp=1d0
      if (iSatFlg.eq.0) then
       call SATTP(Tdew,Pdew,z,iFlg1+1,0,dd,Dl,Dv,xdew,ybub,qq,ierr,herr)
       if (ierr.eq.141) goto 350       !P>Pc
       if (ierr.ge.100) then           !If SATTP failed, switch to P inputs when starting with T inputs, or vice versa.
         Pdew=P
         Tdew=T
         iFlg1=4-iFlg1
         call SATTP (Tdew,Pdew,z,iFlg1+1,0,dd,Dl,Dv,xdew,ybub,qq,
     &               ierr,herr)
         if (ierr.eq.141) goto 350     !P>Pc
       endif
       if (ierr.ge.100) then
         if (icomp.ne.0) goto 224
         if (T.lt.Tc-5) Pdew=Pc        !Do not quit yet for mixtures, check if it's a valid liquid state.
       else
         if (P.lt.Pdew*toltp .and. iFlg1+1.le.2) goto 350            !No need to call saturation routine to get liquid state.
         if (T.gt.Tdew/toltp .and. iFlg1+1.ge.3) goto 350
       endif
      endif

c  Calculate bubble point to determine if liquid or 2-phase (for a mixture or pseudo-pure fluid only).
      kph=1        !Single-phase liquid (pure or mixture).
      if (icomp.ne.0) goto 350         !Go to single phase liquid routines for a pure fluid.
      Pbub=P
      Tbub=T
      ddv=0d0
      dd=0
      call SATTP (Tbub,Pbub,z,iFlg1,0,dd,Dl,ddv,xy,ybub,qq,ierr,herr)
      if (ierr.ge.100) then
        Tbub=T
        Pbub=P
        iFlg1=4-iFlg1
        call SATTP(Tbub,Pbub,z,iFlg1,0,dd,Dl,Dv,xy,ybub,qq,ierr,herr)
      endif
c     if (ierrPmx.ne.0 .or. ierrCrt.ne.0 .or. T.le.crtT) goto 223    !If the maximum pressure is not known, but the SATSPLN routine has been called, check to see if P>Pcrit (when Pcrit is known). This happens for open ended phase boundaries, with the pressure continuously increasing.
      if (ABS(Tbub-Tdew).gt.1.d-8 .or. ABS(Pbub-Pdew).gt.1.d-8)ierr2=0
      if (ierr2.eq.999) goto 250       !If both splines and SATTP return the same answer for liquid and vapor, state is most likely 2-phase.
      if (ierr.le.100) then
        if (P.gt.Pbub/toltp .and. iFlg1.le.2) goto 350
        if (T.lt.Tbub*toltp .and. iFlg1.ge.3) goto 350
      endif

c  Two-phase states.
 250  q=0.5d0      !Possible azeotrope if the next two don't happen.
      if (ABS(Pbub-Pdew).gt.1.d-6) q=1d0-(P-Pdew)/(Pbub-Pdew)
      if (ABS(Tbub-Tdew).gt.1.d-6) q=1d0-(T-Tdew)/(Tbub-Tdew)
      if (T.gt.Tc .or. q.le.0. .or. q.ge.1.) then
        q=0.97d0             !For temperatures above Tc, the value of the quality is generally quite high, so reset it to 0.97 and set the vapor density to the density at maxT.
        Dv=Dc
        xdew(1:nc)=(z(1:nc)+xdew(1:nc))/2d0
      endif
      qin=q
      call SAT0EST (T,P,z,x,y,ierre,herr)
      if (ierre.ge.100) call GETXYZQ (1,1,q,z,xdew,ybub,x,y)
c     iSatFlg=1
      call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)             !Call two-phase flash routine.
c     iSatFlg=0
      if (ierr.eq.2) ierr=0            !For D>Dmax error, let CHECKLIMITS below determine this since the error may be for DL, not for the 2-phase density.
      if (ierr.ge.100) then
        if (ierre.lt.100) q=(q+qin)/2d0                    !Use a different q to call GETXYZQ for the second attempt; thus, if SAT0EST failed, use the original q, otherwise use the bad q from the call to TPFL2.
        if (ABS(q-qin).lt.1d-10) q=0.5d0
        if (q.lt.0.01d0 .or. q.gt.0.99d0) q=0.5d0
        call GETXYZQ (1,1,q,z,xdew,ybub,x,y)
        call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)           !Call two-phase flash routine.
        if (ierr.ge.100) then
          q=1d0-q
          if (q.lt.0.01d0 .or. q.gt.0.99d0) q=0.5d0
          if (ABS(q-0.5d0).lt.0.1d0) q=q/2d0
          call GETXYZQ (1,1,q,z,xdew,ybub,x,y)
          call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)         !Call two-phase flash routine.
        endif
      endif
      if (ierry.ge.100 .and. ierr.gt.0) then
        x(1:nc)=xdew(1:nc)
        y(1:nc)=ybub(1:nc)
        call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)           !Try again with different inputs.
      endif
      if (ABS(Dl-Dv).lt.0.01d0 .and. ierr2.eq.999) goto 213
      if (ierr.ge.100) D=Dc
      ierry=ierr
      if (ierrx.ne.0) goto 500
      if (ierr.eq.226 .and. T/Tc.gt.0.67d0 .and. iSpline.eq.0) goto 350        !Tc is about 1.5 times the predicted value without the splines active.  1.0/1.5=0.6667
      if (ierr.ne.121) goto 500                  !T>Tc for mixture, go to single-phase routines.

c  Call single-phase routine to get fluid properties.
 350  ierr=0
      D=Dl
      if (kph.eq.2) D=Dv*P/Pdew        !Initial guess for density.
      call TPRHO (T,P,z,kph,1,D,ierr,herr1)
      if (D.gt.Dmax)   call TPRHO (T,P,z,kph,0,D,ierr,herr1)
      if (ierr.ge.100) call TPRHO (T,P,z,kph,0,D,ierr,herr1)
      if (ierr.ge.100) call TPRHO (T,P,z,3-kph,0,D,ierr,herr1)
      if (ierr.ge.100 .and. T.gt.Tc) goto 215
      if (ierr.ge.100) goto 213
      call CHECKSTATE (0,0,T,D,z,Err,ierr,herr)
      ierrx=ierr
      if (ierry.eq.0 .and. ierrx.ge.100 .and. icomp.eq.0) goto 250
      Dl=D
      Dv=D
      if (T.lt.Tc) call GETQ (2,T,P,D,Tc,Pc,Dc,D,Dl,Dv,q)
      if (iAnc(icomp).ne.0) call GETQ (2,T,P,D,Tc,Pc,Dc,P,Pbub,Pdew,q)
      if (q.gt.0d0 .and. q.lt.1.d-6) q=0d0
      if (q.lt.1d0 .and. q.gt.1d0-1.d-6) q=1d0
      if (q.gt.0. .and. q.lt.1.) q=999d0         !Should be single phase.

c  Check if limits have been exceeded, return that message if no other error has occurred.
 500  if (ierr.ne.0 .and. herr.eq.' ') herr=herr1
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS ('TP',T,D,P,z,ierr,herr)
      call GETQQ (T,P,D,Tc,Pc,Dc,q)
      if (q.eq.-998 .and. D.lt.Dcc) q=998
      call GETPROPS (2,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)             !Calculate all other properties.
      RETURN



c   ---------Error Messages---------
 143  call ERRNUM (143,0,'TPFLSH',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 213  call ERRNUM (213,0,'TPFLSH',herr1,0d0,0d0,0d0,ierr,herr)
      goto 500
 215  call ERRNUM (215,0,'TPFLSH',herr1,0d0,0d0,0d0,ierr,herr)
      goto 500
 224  call ERRNUM (224,0,'TPFLSH',herr1,0d0,0d0,0d0,ierr,herr)
      goto 500

      end                                              subroutine TPFLSH
c
c ======================================================================
c
      subroutine TPDFLSH (T,P,D,z,iFlag,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,
     &                    ierr,herr)
c
c  Flash calculation given bulk composition and two of the following
c     T (temperature), P (pressure), or D (density)
c  This routine accepts both single-phase and two-phase states as inputs;
c  for single-phase calculations, subroutines TPRHO, THERM, or PDFL1 are faster.
c
c  Inputs:
c        T--Temperature [K]       (except for DP flash)
c        P--Pressure [kPa]        (except for TD flash)
c        D--Molar density [mol/L] (except for TP flash)
c        z--Overall composition (array of mole fractions)
c    iFlag--Flag to specify inputs:
c           1 - P(T,D) - Inputs are temperature and density, pressure is returned.
c           2 - D(T,P) - Inputs are temperature and pressure, density is returned.
c                        (call TPFLSH directly if desired, which is what this routine does.)
c           3 - T(D,P) - Inputs are density and pressure, temperature is returned.
c
c  Outputs:
c        T--Temperature [K]       (for DP flash)
c        P--Pressure [kPa]        (for TD flash)
c        D--Molar density [mol/L] (for TP flash)
c     ierr--Error flag:  0 - Successful
c                      213 - Density calculation did not converge
c                      215 - Supercritical density calculation did not converge
c                      223 - Bubble point did not converge
c                      224 - Dew point did not converge
c     herr--Error string (character*255)
c  (See subroutine ABFLSH for the description of all other output variables.)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  09-20-95  MM, original version for TDFLSH
c  03-03-99 EWL, original version for PDFLSH
c  06-01-16 EWL, combine the above two routines into this single routine

      include 'COMMONS.INC'
      character herr*255,herr1*255,ab*2
      dimension x(ncmax),y(ncmax),z(ncmax),xy(ncmax)
      dimension xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TPDFLSH

      if (iFlag.eq.2) then
        call TPFLSH (T,P,z,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
        RETURN
      endif

      iErrNum=iErrNum+1
      call SETCOMP (icomp,z,x,y)                 !Set output liquid and vapor compositions to input values.
      call CRITP (z,Tcr,Pcr,Dcr,ierr,herr)       !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      dd=0d0
      Dl=0d0
      Dv=0d0
      ddv=0d0
      Tbub=T
      Pbub=P
      Tdew=T
      Pdew=P
      q=999d0
      ierr=0
      ierr2=0
      ierrx=0
      ierry=0
      ierrv=0
      ierrl=0
      isp=0
      kph=1
      icrt=0
      diff=0.01d0
      ksat=1
      herr1=' '

c  Set initial estimates.
      if (iFlag.eq.1) then
        iFlg1=1              !iFlg1 is either 1 (when T is an input) or 3 (when T is unknown).
        iFlg2=1
        P=0d0
        if (T.le.0. .or. D.lt.0.) goto 143
        P=D*Rxgas*T
        ab='TD'
        if (D.lt.1d-10) goto 550       !Check for ideal-gas state.
        a=T
        if (icomp.ne.0 .and. T.gt.Tc) goto 350
        if (iSpline.ne.0 .and. T.ge.Tc+1)  goto 350        !Assume a small error in the critical temperature from the splines.
        call CHECKSTATE (0,0,T,D,z,Err,ierrstate,herr)
        if (ierrstate.gt.700 .and. D.lt.Dc*2d0 .and. icomp.eq.0) then          !Subroutine CHECKSTATE determined that the state is 2-phase, skip everything else and go to 2-phase section, sending it a flag that initial estimates are not known.
          if (iSpline.eq.0) ksat=0
          goto 250
        endif
        if (iSpline.eq.0 .and. ierrstate.eq.0 .and. T.gt.Tc
     &        .and. ixwat.eq.0) goto 350         !If splines are off, Tc is already 50% larger than the estimated value, so assume single phase for large T.
      elseif (iFlag.eq.3) then
        diff=diff*2d0
        iFlg1=3
        iFlg2=2
        T=300d0
        ab='PD'
        if (D.le.0.) goto 207
        if (P.lt.0.) goto 350
        if (P.le.0.) goto 207
        if (P.lt.1d-10) goto 500       !Check for ideal-gas state.
        if (D.lt.1d-10) goto 550       !Check for ideal-gas state.
        if (D.lt.1. .and. P.lt.1.) T=P/(Rxgas*D)
        if (T.lt.200.) T=200d0
        if (icomp.ne.0 .and. T.lt.Tmeos(icomp)) T=Tmeos(icomp)
        if (T.gt.Tc .and. Tc.gt.50d0) T=INT(Tc-10d0)
        a=P
        if (icomp.ne.0 .and. P.gt.Pc) goto 350
        if (iSpline.ne.0 .and. P.ge.Pc+10) goto 350
      endif

c  Get estimated saturation pressures and densities from ancillaries or splines.
c  Check for supercritical or single-phase states for pure fluids, or for mixtures when the splines have been called.
      if (icomp.ne.0 .or. iSpline.ne.0) then
        call SATGUESS(1,iFlg2,0,0,z,Tbub,Pbub,Dl,h,s,Dv,ybub,ierr1,herr)
        if (icomp.eq.0 .or. iAnc(icomp).ne.0)
     &  call SATGUESS(2,iFlg2,0,0,z,Tdew,Pdew,Dv,h,s,dd,xdew,ierr2,herr)
        if (ierr1.eq.0 .and. ierr2.eq.0) then
          if (iAnc(icomp).ne.0) diff=0d0         !Ancillary equations for pseudo-pure fluid are exact.
          if (iSpline.ne.0) diff=0.05d0          !Set the difference for mixtures with the splines on to 5%.
          if (iFlag.eq.1 .and. (Tc-T)/Tc.gt.diff/10d0) icrt=1
          if (iFlag.eq.3 .and. (Pc-P)/Pc.gt.diff     ) icrt=1
          if ((D-Dl)/Dl.gt.diff .and. icrt.eq.1) goto 350            !All ancillary equations in versions 10 and higher are within 1% of the saturation state except within 0.5% of the critical point.
          if ((Dv-D)/Dv.gt.diff .and. icrt.eq.1) goto 350            !This makes it possible to avoid a call to SATT or SATP under these conditions.
          if (iAnc(icomp).ne.0) goto 142         !Exit for pseudo-pure fluid in the 2-phase.
          if ((Dl-D)/Dl.gt.diff .and. (D-Dv)/Dv.gt.diff .and.
     &         icomp.eq.0 .and. icrt.eq.1) goto 250        !Go straight to 2-phase routines if not near the saturation line for mixtures.
        endif
      endif

c  Check for subcritical state; call dew-point saturation routine to determine phase.
      ignbnd2=1
      call SATTP(Tdew,Pdew,z,iFlg1+1,0,dd,Dl,Dv,xdew,ybub,qq,ierr,herr)
      ierrv=ierr
      ignbnd2=0
      if (iFlag.eq.3) T=Tdew
      if (iFlag.eq.1) P=Pdew
      if (ierrv.ge.100 .and. icomp.ne.0) goto 224
      if (ierrv.ge.100 .and. (iFlag.eq.3 .or. T.lt.Tc-5)) Pdew=0d0   !Do not exit yet for mixtures, check if it's a valid liquid state.
      kph=2        !Single-phase vapor (pure or mixture).  No need to call saturation routine to get liquid state.
      if (D.le.Dv+1d-12 .and. Pdew.gt.0.) goto 350

c  Calculate bubble point to determine if liquid or 2-phase (for a mixture or pseudo-pure fluid only).
      kph=1        !Single-phase liquid (pure or mixture).
      ierr=0
      if (icomp.eq.0) then
        call SATTP(Tbub,Pbub,z,iFlg1,0,dd,Dl,ddv,xy,ybub,qq,ierr,herr)
        if(ABS(Dl-ddv).lt.0.05d0*Dl.and.ierrv.eq.124.and.ierr.eq.0)isp=1       !If vapor phase failed and liquid phase succeeded, but difference in densities is small, point could be single-phase, which is checked below.
      endif
      ierrl=ierr
c     if (ierrPmx.ne.0 .or. ierrCrt.ne.0 .or. T.le.crtT) goto 223    !If the maximum pressure is not known, but the SATSPLN routine has been called, check to see if P>Pcrit (when Pcrit is known). This happens for open ended phase boundaries, with the pressure continuously increasing.
      if (D.ge.Dl-1d-12 .and. ierr.lt.100) goto 350                  !For a pure fluid, this uses the ierr from about 17 lines above, be careful.
      if (ierr.ge.100 .and. P.gt.Pc .and. iFlag.eq.3) goto 350       !If both saturation calls failed, goto to single phase calculation, which is followed by a call to CHECKSTATE to look for bad roots.
      if (ierr.ge.100 .and. iFlag.eq.1) then
         !Fix for type III mixtures where SATTP for liquid fails.
        if (T.gt.Tcr .and. ierrstate.eq.0 .and. ixWat.eq.0) goto 350 !If both saturation calls failed and the call to CHECKSTATE returns zero, assume the state is single phase.
      endif

c  Mixture two-phase states.
 250  q=0.5d0
      ierr=0
      if (icomp.ne.0) goto 500         !Pure-fluid 2-phase state.
      Dlbub=Dl
      Dvdew=Dv
      if (ierrv.gt.100 .or. ierrl.gt.100) ksat=0
      call ABFL2 (ab,a,D,z,1,ksat,Tbub,Tdew,Pbub,Pdew,     !Call FL2 routine for temperature-density or pressure-density inputs.
     &           Dlbub,Dvdew,ybub,xdew,tt,pp,Dl,Dv,x,y,q,ierr,herr)
      ierry=ierr
      if (ierr.eq.999) goto 350        !State is single phase.
      if (ierr.gt.100 .and. P.gt.Pc*0.9d0) goto 350        !ABFL2 failed, but pressure is high but not quite above Pc, assume single phase.
      if (iFlag.eq.1) P=pp
      if (iFlag.eq.3) T=tt
      if (isp.eq.1 .and. ierr.gt.100) ierr=121
      if (ierr.ne.121 .or. ierrx.ne.0) goto 500            !T>Tc for mixture, go to single-phase routines.

c  Single-phase states (pure fluids or mixtures).
 350  ierr=0
      if (iFlag.eq.1) then             !T and D inputs.
        call PRESS (T,D,z,P)
      elseif (iFlag.eq.3) then         !P and D inputs.
        call DBFL1 (D,P,z,'DP',T,pb,ierr,herr1)
        if (ierr.ge.100 .and. P.gt.Pc) goto 215
        if (ierr.ge.100) goto 213
      endif
      call CHECKSTATE (0,0,T,D,z,Err,ierrx,herr)
      if (ierry.eq.0 .and. ierrx.ge.100) goto 250
      if (T.lt.Tc) call GETQ (2,T,P,D,Tc,Pc,Dc,D,Dl,Dv,q)  !This calculates a pseudo-q for single phase mixture states.
      if (q.gt.0d0 .and. q.lt.1.d-6) q=0d0
      if (q.lt.1d0 .and. q.gt.1d0-1.d-6) q=1d0
      if (q.gt.0. .and. q.lt.1.) q=999d0         !Should be single phase.

c  Check if limits have been exceeded, return that message if no other error has occurred.
 500  if (icomp.ne.0) then
        call GETQ (2,T,P,D,Tc,Pc,Dc,D,Dl,Dv,q)
        if (iAnc(icomp).ne.0) then
          if (ABS(Pbub-Pdew).gt.1d-12) then
            q=1d0-(P-Pdew)/(Pbub-Pdew)
            if (iflag.eq.3 .and. q.gt.0. .and. q.lt.1.) then         !Get T for pseudo-pure fluids.
              T=(1d0-q)*Tbub+q*Tdew
              call PTANC (T,P,q,D,iLetterQ,Dl,Dv)
            endif
          endif
        endif
      endif
      if (ierr.ne.0 .and. herr.eq.' ') herr=herr1
 550  iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS (ab,T,D,P,z,ierr,herr)
      call GETQQ (T,P,D,Tc,Pc,Dc,q)
      call GETPROPS (0,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)
      RETURN



c   ---------Error Messages---------
 143  call ERRNUM (143,0,'TPDFLSH',' ',0d0,0d0,0d0,ierr,herr)
      goto 550
 142  call ERRNUM (142,0,'TPDFLSH',' ',0d0,0d0,0d0,ierr,herr)
      goto 550
 207  call ERRNUM (207,0,'TPDFLSH',' ',P,0d0,0d0,ierr,herr)
      goto 550
 213  call ERRNUM (213,0,ab//'FLSH',herr1,0d0,0d0,0d0,ierr,herr)
      if (iflag.eq.3) T=300d0
      goto 550
 215  call ERRNUM (215,0,ab//'FLSH',herr1,0d0,0d0,0d0,ierr,herr)
      goto 550
 223  call ERRNUM (223,0,ab//'FLSH',herr1,0d0,0d0,0d0,ierr,herr)
      goto 550
 224  call ERRNUM (224,0,ab//'FLSH',herr1,0d0,0d0,0d0,ierr,herr)
      goto 550

      end                                             subroutine TPDFLSH
c
c ======================================================================
c
      subroutine TBFLSH (T,b,z,kr,ab,P,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,
     &                   ierr,herr)
c
c  Flash calculation given temperature, bulk composition, and either
c  enthalpy, entropy, or energy.
c
c  Inputs:
c        T--Temperature [K]
c        b--Second property (energy, enthalpy, or entropy)
c        z--Composition (array of mole fractions)
c       kr--Flag specifying desired root for multi-valued inputs:
c           1 - Return lower density root
c           2 - Return higher density root
c       ab--Character*2 string defining the three inputs 'TH', 'TS', or 'TE'
c
c  Outputs:
c        P--Pressure [kPa]
c        D--Overall molar density [mol/L]
c     ierr--Error flag:  0 - Successful
c                       17 - Inputs out of range
c                     -238 - Two roots are possible, the higher density root was returned
c                     -239 - Two roots are possible, the lower density root was returned
c                      238 - No root exists
c                      (See subroutines ABFL1 and ABFL2 for other possible error numbers.)
c     herr--Error string (character*255)
c  (See subroutine ABFLSH for the description of all other output variables.)
c
c  rewritten by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-18-01 EWL, original version, rewritten by combining code of McLinden
c                from the THFLSH routine to make one unified routine
c  12-04-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character bt*1,ab*2,herr*255
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TBFLSH

      bt=ab(2:2)
      if (bt.eq.'H') ibt=iLetterH
      if (bt.eq.'S') ibt=iLetterS
      if (bt.eq.'E') ibt=iLetterE
      q=998d0
      D=0d0
      P=0d0
      Dl=0d0
      Dv=0d0
      iFlg=0
      kr2=kr
      tt=300d0
      iErrNum=iErrNum+1

      call SETCOMP (icomp,z,x,y)                 !Set output liquid and vapor compositions to input values.
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).

c  Get limits and check if two roots exist.
 100  call DHESLMTS (0,kr2,T,Tc,b,z,ibt,Dmin,Dmax,Dmid,
     &               bMin,bMax,bv,Pdew,Dl,Dv,xdew,ierr,herr)

c  Supercritical state - Check for two solutions and then determine density.
      if (T.ge.Tc) then
        if (Dmid.gt.0. .and. kr2.ne.2) Dmax=Dmid           !Upper limit for root search of the lower-density root
        goto 350
      endif

c  Single-phase vapor (The input H, E, or S is greater than that at the dew point).
      if (b.ge.bv .and. kr2.ne.2) goto 350

c  Liquid or two-phase state.
      call DHESLMTS (1,kr2,T,Tc,b,z,ibt,Dmin,Dmax,Dmid,
     &               bMin,bMax,bl,Pbub,Dl,dd,ybub,ierr,herr)
      if (ibt.ne.iLetterS .and. b.lt.bMin) goto 238        !No root exists for E and H inputs.

c  Check for the possibility of two roots; a two-phase solution and a high pressure single-phase state.
      if (kr2.eq.2) goto 350           !High pressure single-phase liquid.
      if (b.lt.bl) then                !Low pressure single-phase liquid.
        Dmin=Dl
        if (Dmid.gt.0.) Dmax=Dmid
        goto 350
      endif

c  Two-phase solution for a pure fluid.
      ierr=0
      iFlg=2
      if (icomp.ne.0 .and. iAnc(icomp).eq.0) then
        q=(b-bl)/(bv-bl)
        P=Pdew
        goto 400

c  Two-phase solution for a mixture.
      else
        Dlbub=Dl
        Dvdew=Dv
        call ABFL2 (ab,T,b,z,1,1,Tbub,Tdew,Pbub,Pdew,
     &              Dlbub,Dvdew,ybub,xdew,tt,P,Dl,Dv,x,y,q,ierr,herr)
        goto 400
      endif

c  Get single-phase density.
 350  if (ibt.eq.iLetterS) then        !Check for very low densities for S input.
        if (b.gt.bMax) then
          Dmin=Dmin*1d-8
          Dmax=Dmin
        endif
      else
        if (kr2.eq.2 .and. Dmid.gt.0.) Dmin=Dmid           !Lower limit for root search of the higher-density root
      endif
      if (Dmin.ge.Dmax) Dmax=Dmin*1.2d0
      call ABFL1 (ab,T,b,z,0,0,Dmin,Dmax,tt,P,D,ierr,herr)
      do i=1,2
        if (ierr.ge.100 .and. (Dmin.gt.Dc .or. T.gt.Tc)) then        !Check for states beyond Dmax.
          Dmax=Dmax*2d0
          call ABFL1 (ab,T,b,z,0,0,Dmin,Dmax,tt,P,D,ierr,herr)
        endif
      enddo
      call GETQ (2,T,P,D,Tc,Pc,Dc,D,Dl,Dv,q)     !Base q on density, not enthalpy, because the enthalpy increases in the liquid phase, and thus it would look like a 2-phase state.

c  Check if limits have been exceeded, return that message if no other error has occurred.
 400  if (ierr.le.0) then
        if (ibt.ne.iLetterS) then
          if (Dmid.gt.0. .or. D.gt.Dc .or. (q.gt.0. .and. q.lt.1.) .or.
     &          (kr.eq.2 .and. kr2.eq.1)) then   !Issue warning that two roots exist.
            if(kr2.eq.2)call ERRNUM(-238,0,ab,' ',0d0,0d0,0d0,ierr,herr)
            if(kr2.ne.2)call ERRNUM(-239,0,ab,' ',0d0,0d0,0d0,ierr,herr)
          endif
        else
          if (b.lt.bMin-1d-6*ABS(bmin)) goto 17
        endif
      endif

c  Finalize routine.
 500  iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS (ab,T,D,P,z,ierr,herr)
      if (kr2.eq.2 .and. ierr.ge.2 .and. ierr.lt.16) then
        iErrNum=iErrNum+1
        kr2=1
        goto 100   !If upper root is out of bounds, search for lower root.
      endif
      call GETQQ (T,P,D,Tc,Pc,Dc,q)
      call GETPROPS (iFlg,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)          !Calculate all other properties.
      RETURN


c   ---------Error Messages---------
  17  call ERRNUM (17,0,ab,bt,b,bMin,bMax,ierr,herr)
      goto 500
 238  D=0d0
      call ERRNUM (238,0,ab,bt,b,bMin,T,ierr,herr)
      goto 500


      end                                              subroutine TBFLSH
c
c ======================================================================
c
      subroutine PBFLSH (P,b,z,ab,T,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,
     &                   ierr,herr)
c
c  Flash calculation given pressure, bulk composition, and either
c  enthalpy, entropy, or energy.
c
c  Inputs:
c        P--Pressure [kPa]
c        b--Second property (energy, enthalpy, or entropy)
c        z--Composition (array of mole fractions)
c       ab--Character*2 string defining the three inputs 'PH', 'PS', or 'PE'
c
c  Outputs:
c        T--Temperature [K]
c        D--Overall molar density [mol/L]
c     ierr--Error flag:  0 - Successful
c                       17 - e, h, or s out of range
c                      223 - SATP did not converge at bubble point
c                      224 - SATP did not converge at dew point
c                      (See subroutines ABFL1 and ABFL2 for other possible error numbers.)
c     herr--Error string (character*255)
c  (See subroutine ABFLSH for the description of all other output variables.)
c
c  rewritten by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-18-01 EWL, original version, rewritten by combining code of McLinden
c                from PHFLSH and PSFLSH routines to make one unified routine
c  08-15-11 EWL, add check for b<bmin for water mixtures
c  06-09-14 EWL, add check for b<bvdew when determining if phase is liquid (single-phase)
c  12-04-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character bt*1,ab*2,herr*255
      dimension x(ncmax),y(ncmax),z(ncmax),xy(ncmax)
      dimension xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PBFLSH

      ig=0
      bt=ab(2:2)
      if (bt.eq.'H') ibt=iLetterH
      if (bt.eq.'S') ibt=iLetterS
      if (bt.eq.'E') ibt=iLetterE
      T=300d0
      Tbub=300d0
      D=0d0
      q=999d0
      ierrl=0
      ierrv=0
      xdew(1:nc)=z(1:nc)
      ybub(1:nc)=z(1:nc)
      iErrNum=iErrNum+1
      if (ibt.eq.iLetterS) D=1d-6
      call SETCOMP (icomp,z,x,y)                           !Set output liquid and vapor compositions to input values.
      call CRITP (z,Tcc,Pcc,Dcc,ierr,herr)                 !Get estimated critical point in case SATSPLN was not called.
      call SATMAX  (icomp,z,Tc,Pc,Dc,ierr,herr)            !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS  ('EOS',z,Tmin,Tmax,Dmax,Pmax)
      call HESCALC (ibt,Tmax*1.5d0,D,z,bMax)               !Maximum possible value of b (at Tmax, zero density).

c   Get minimum possible value of b (at saturated Tmin).
      P0=1000d0    !Needs to be high enough to include CO2, where 100 kPa is in the vapor at the triple point.
      do
        dd=0d0
        call TPRHO (Tmin,P0,z,1,0,dd,ierr,herr)
        if (ierr.lt.100 .or. Tmin.gt.Tmax) EXIT
        Tmin=Tmin+5d0
      enddo
      call HESCALC (ibt,Tmin,dd,z,bmin)
      if (icomp.ne.0) then
        D=Dtpn(icomp)
        call HESCALC (ibt,Tmin,D,z,bmin2)        !Calculate b at the triple point.
        if (ABS(bmin2).gt.1d-20 .and. bmin2.lt.bmin) bmin=bmin2
      endif
      if (ibt.eq.iLetterS .and. b.lt.bmin .and. icomp.ne.0) then     !Check melting line for rare cases where bmin is lower than at saturated Tmin.
        call MELTT (Ttpn(icomp),z,pp,ierr,herr)
        if (pp.gt.0. .and. ierr.lt.100) then
                   !tmlt=251.165 Water
                   !tmlt=244.8   CO2
                   !tmlt=200.2   Ammonia
          iw=iFluidID(icomp)
          if (iw.eq.1 .or. iw.eq.3 .or. iw.eq.10) then     !Check for water or helium.
            if (iw.eq.1 .or. iw.eq.3) then
              tmlt=-999d0
              if (iw.eq.1) call MLTH2O (tmlt,pp,p2)        !Get water's melting pressure at Tlow.
              if (iw.eq.3) call MLTD2O (tmlt,pp,p2)        !Get heavy water's melting pressure at Tlow.
            else
              tmlt=Ttpn(icomp)                   !Get helium's melting pressure.
              call MELTT (tmlt,z,pp,ierr,herr)
            endif
            call TPRHO (tmlt,pp,z,1,0,dd,ierr,herr)
            call HESCALC (ibt,tmlt,dd,z,bmin2)
          else
            tmlt=Ttpn(icomp)+5d0
            call ROOTFIND ('ML',0,iLetterS,0d0,x,1,1,1,0,0,0,tmlt,dd,
     &                     bmin2,ierr,herr)
          endif
          if (ABS(bmin2).gt.1d-20 .and. bmin2.lt.bmin .and. ierr.lt.100)
     &            bmin=bmin2
        endif
      endif
      if (b.lt.bmin) then
        tminw=Tmin
        do
          tminw=tminw+1d0                                  !For mixtures with water, the H/E/S values can loop back and the minimum b will not be at the low T.
          call TPRHO (tminw,P0,z,1,1,dd,ierr,herr)         ! The code here will not return the exact point since there will be even lower values of b at high pressures.
          call HESCALC (ibt,tminw,dd,z,bminw)
          if (tminw.gt.tmax) ierr=999
          if (bminw.gt.bmin .or. ierr.ge.100) EXIT
          bmin=bminw
        enddo
        if (ABS(tminw-Tmin).gt.1.1d0) bmin=bmin-2d0*(bminw-bmin)     !Decrease slightly to include saturated state points missed with an increment of 1 K.
      endif

c  Supercritical states.
      kph=0
      if (P.ge.Pc .or. P.lt.1d-12) goto 350                !Supercritical state or P=0.

c  Single-phase vapor states.
      kph=2
      call TPRHO (Tc,P,z,kph,0,dd,ierr,herr)               !If the input b is greater than b at the critical temperature and given pressure, the inputs are single phase.
      call HESCALC (ibt,Tc,dd,z,bb)
      if (b.gt.bb .and. ierr.lt.100) goto 350

c  Subcritical state, first call the vapor saturation routine to determine the phase.
      kph=2
      if (icomp.ne.0 .and. P.lt.Ptpn(icomp)) kph=4         !Check for sublimation state.
      call SATP (P,z,kph,T,Dl,Dv,xdew,xy,ierrv,herr)
      if (ierrv.gt.100 .and. iEOSmx.eq.iEOSPR3) then       !If the vapor phase iteration fails when the PR EOS is in use with volume translation,
        call PREOS(0)                                      ! try turning off the PR and recalling SATP to get the saturation temperature and then turn the PR back on.
        call SATP (P,z,kph,T,Dl,Dv,xdew,xy,ierrv,herr)
        call PREOS(3)
        call TPRHO (T,P,z,kph,1,Dv,i,herr)                 !Recalculate Dv with the PR back on.
      endif
      call HESCALC (ibt,T,Dv,z,bdew)
      if (kph.eq.4 .and. b.lt.bdew) goto 66                !Entropy below sublimation value.
      ig=1
      D=Dv
      if (ierrv.lt.100) then
        if (icomp.ne.0 .and. iAnc(icomp).eq.0) then        !For a pure fluid, no need to call SATP again.
          call HESCALC (ibt,T,Dl,z,bbub)
          call GETQ (3,T,P,D,Tc,Pc,Dc,b,bbub,bdew,q)       !Get q in the vapor phase.
          if (b.gt.bdew-ABS(bdew)/1d7) goto 350            !Single-phase vapor.
          kph=1
          D=Dl
          if (b.lt.bbub+ABS(bbub)/1d7) goto 350            !Single-phase liquid.
          goto 400           !Two-phase pure fluid.
        endif
        bminx=(bmin+bdew)/2d0
        if (q.eq.999.) q=XDIV(b-bminx,bdew-bminx)          !For a mixture in the single-phase vapor, the saturated liquid state will not be calculated, so this gives an approximate value when near the saturation line.
        if (ABS(1d0-q).lt.1.d-6) q=1d0-ABS(1d0-q)
        if (b.gt.bdew-ABS(bdew)/1d7) goto 350              !Single-phase vapor.
      endif
      Dvdew=Dv
      Tdew=T

c  Check for single-phase liquid states.
      call SATP (P,z,1,Tbub,Dlbub,dd,xy,ybub,ierrl,herr)
      call HESCALC (ibt,Tbub,Dlbub,z,bbub)
      call GETQ (3,T,P,D,Tc,Pc,Dc,b,bbub,bdew,q)           !Get q if in the liquid single-phase.
      ig=0
      kph=1
      D=Dlbub
      if (b.lt.bbub+ABS(bbub)/1d8 .and. ierrl.lt.100) goto 350
      if (iSpline.eq.0 .and. P.gt.Pcc*1.05d0) goto 350     !Check if splines are not available and point is possibly above Pc.
      if (P.lt.1d-6 .and. ierrl.ge.100) goto 350           !If P is extremely small and SATP failed, assume single phase state.
      if (ierrv.ge.100 .or. (ierrl.ge.100 .and. ixAir.eq.0)) then
        if (P.gt.Pcc/2d0) goto 350     !Saturation boundary unknown, but try single phase routine and then call CHECKSTATE.
        if (ierrl.ge.100) then
          call HESCALC (ibt,Tcc,Dcc,z,ehsc)
          if (P.lt.Pcc/20d0 .and. b.gt.ehsc) then          !For low pressures with energy, enthalpy, or entropy greater than that at the critical point, the point is 2-phase (unless SATP failed for the vapor and then it is unknown).
            call SATGUESS(1,2,0,0,z,Tbub,P,Dlbub,h,s,dd,ybub,ierr,herr)
            goto 100
          endif
          goto 223
        endif
        goto 224
      endif

c  Check for two-phase pseudo-pure fluid states.
 100  if (iAnc(icomp).ne.0) then
        call GETQ (3,T,P,D,Tc,Pc,Dc,b,bbub,bdew,q)
        T=q*Tdew+(1d0-q)*Tbub
        call PTANC (T,P,q,b,ibt,Dl,Dv)
      else

c  Two-phase mixture states.
        call ABFL2 (ab,P,b,z,1,1,Tbub,Tdew,Pbub,Pdew,
     &              Dlbub,Dvdew,ybub,xdew,T,pp,Dl,Dv,x,y,q,ierr,herr)
      endif
      goto 400

c  Call the iterative routine ABFL1 to find the single-phase state point at P and b.
 350  call ABFL1 (ab,P,b,z,kph,ig,1d0,1d0,T,pp,D,ierr,herr)
      if (q.gt.0d0 .and. q.lt.1.d-8) q=0d0
      if (q.lt.1d0 .and. q.gt.1d0-1.d-6) q=1d0
      if (q.gt.0. .and. q.lt.1.) q=999d0         !Should be single phase.
      if (ierr.eq.0 .and. (ierrl.ge.100 .or. ierrv.ge.100)) then
        call CHECKSTATE (0,0,T,D,z,Err,ierr,herr)
        if (ierr.lt.100) goto 235
        if (ierrl.ge.100) goto 223
        goto 224
      endif


c  Check if limits have been exceeded, return that message if no other error has occurred.
 400  if (b.lt.bmin-1d-6*ABS(bmin).or.b.gt.bmax+1d-6*ABS(bmax)) goto 17

 500  iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS (ab,T,D,P,z,ierr,herr)
      call GETQQ (T,P,D,Tc,Pc,Dc,q)
      call GETPROPS (2,ierr,q,T,pp,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)            !Calculate all other properties.
      RETURN



c   ---------Error Messages---------
  17  if (ierr.lt.100) call ERRNUM (17,0,ab,bt,b,bmin,bmax,ierr,herr)
      goto 500
 66   call ERRNUM (66,0,ab//'FLSH',herr,b,bdew,0d0,ierr,herr)
      goto 500
 223  call ERRNUM (223,0,ab//'FLSH',herr,0d0,0d0,0d0,ierr,herr)
      goto 500
 224  call ERRNUM (224,0,ab//'FLSH',herr,0d0,0d0,0d0,ierr,herr)
      goto 500
 235  call ERRNUM (-235,0,ab//'FLSH',herr,0d0,0d0,0d0,ierr,herr)
      goto 400

      end                                              subroutine PBFLSH
c
c ======================================================================
c
      subroutine DBFLSH (D,b,z,ab,T,P,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,
     &                   ierr,herr)
c
c  Flash calculation given density, bulk composition, and either
c  enthalpy, entropy, or energy.
c
c  This routine accepts both single-phase and two-phase states as inputs;
c  for single-phase calculations, the subroutines xxFL1 are faster,
c  where xx is DH, DS, etc.
c
c  Inputs:
c        D--Overall molar density [mol/L]
c        b--Second property (energy, enthalpy, or entropy)
c        z--Overall composition (array of mole fractions)
c       ab--Character*2 string defining the three inputs 'DH', 'DS', or 'DE'
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c     ierr--Error flag:  0 - Successful
c                       17 - Input value is outside limits
c                      226 - 2-phase iteration did not converge
c                      (See subroutines ABFL1 and ABFL2 for other possible error numbers.)
c     herr--Error string (character*255)
c  (See subroutine ABFLSH for the description of all other output variables.)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  05-13-99 EWL, original version
c  12-08-15 EWL, check for failure in SATD with d>dtrp, and set T=Ttrp if so
c  12-07-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character bt*1,ab*2,herr*255
      dimension x(ncmax),y(ncmax),z(ncmax),ybub(ncmax),xdew(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DBFLSH

      bt=ab(2:2)
      if (bt.eq.'H') ibt=iLetterH
      if (bt.eq.'S') ibt=iLetterS
      if (bt.eq.'E') ibt=iLetterE
      q=999d0
      iErrNum=iErrNum+1
      call SETCOMP (icomp,z,x,y)                 !Set output liquid and vapor compositions to input values.
      call SATMAX  (icomp,z,Tc,Pc,Dc,ierr,herr)            !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      kr=1
      if (D.lt.Dc) kr=2
      ybub(1:nc)=z(1:nc)
      xdew(1:nc)=z(1:nc)

c  Check if the point is 2-phase by comparing the input property with that at the critical point.
c  If the input value is less than about half that at critical, then the point is 2-phase assuming
c   that the critical value is not near zero (for example, argon).
      bcrit=-1d20
      bsat=1d20
      if (D.gt.Dc/2. .and. D.lt.Dc*1.5d0)call HESCALC(ibt,Tc,Dc,z,bcrit)
      if ((b.lt.bcrit/2d0 .and. bcrit.gt.0) .or.
     &    (b.lt.bcrit*2d0 .and. bcrit.lt.0)) then
        call HESCALC (ibt,Tc,0d0,z,b0)
        bdiff=b0-bcrit
        bx=bcrit-bdiff
        if (b.lt.bx) then    !For critical point values near zero, check that the input value is less than the value at Tc minus the distance between Tc,Dc and Tc,0.
          bd=bdiff/(bcrit-b)*1.4d0
          if (bd.lt.0.35d0) bd=0.35d0
          T=Tc*bd
          Dlbub=Dc/bd
          Dvdew=Dc*bd/10d0
          if (icomp.ne.0) then
            call SATGUESS (0,3,0,0,z,T,P,Dlbub,h,s,Dvdew,ybub,ierr,herr)
          else
            call SATEST (1,T,P,z,xdew,ybub,ierr,herr)
          endif
          goto 10
        endif
      endif

c  Find the intersection of the input density and the saturation lines.
      call SATD (D,z,0,kr,T,P,Dlbub,Dvdew,ybub,xdew,ierr,herr)
      if (D.le.0 .or. (ierr.eq.2 .and. D.lt.0.001d0)) then
        if (D.lt.0 .or. ibt.eq.iLetterS) goto 207          !No solution for P or S if density is zero.
        T=300d0              !For E or H, call ROOTFIND to get the ideal gas temperature.
        call ROOTFIND (' ',ibt,0,b,z,2,0,1,0,0,0,T,D,o,ierr,herr)
        if (ierr.le.0) goto 500
      endif
      if(ierr.gt.0 .and. icomp.ne.0 .and. D.gt.Dtpn(icomp))T=Ttpn(icomp)

c  Get saturation energy, enthalpy, or entropy.
      call HESCALC (ibt,T,D,z,bsat)
      if (kr.eq.3 .and. b.lt.bsat-1d-6*ABS(bsat)) then
        call ABFL1 (ab,D,b,z,0,0,0d0,0d0,T,P,dd,ierr,herr)           !Check for solid phase, but calculate point anyway for metastable states.
        goto 17
      endif

c  Call single or 2-phase routines to find the unknown temperature.
 10   if (b.ge.bsat .or. D.le.0.) then
        call ABFL1 (ab,D,b,z,0,0,0d0,0d0,T,P,dd,ierr,herr)
      else
        Tbub=T
        Tdew=T
        call ABFL2 (ab,D,b,z,1,1,Tbub,Tdew,Pbub,Pdew,
     &              Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,q,ierr,herr)
        if (ABS(q)    .lt.1d-10) Dl=D
        if (ABS(q-1d0).lt.1d-10) Dv=D
        if ((q.lt.0. .or. q.gt.1.) .and. ierr.lt.100) goto 226
      endif

c  Check if limits have been exceeded, return that message if no other error has occurred.
 500  iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS (ab,T,D,P,z,ierr,herr)
      call GETQQ (T,P,D,Tc,Pc,Dc,q)
      call GETPROPS (0,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)
      if (ibt.eq.iLetterH) h=b         !Set e, h, or s to input variable.
      if (ibt.eq.iLetterE) e=b
      if (ibt.eq.iLetterS) s=b
      RETURN



c   ---------Error Messages---------
  17  call ERRNUM (17,0,ab,bt,b,bsat,0d0,ierr,herr)
      goto 500
 207  call ERRNUM (207,0,ab//'FLSH',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 226  call ERRNUM (226,0,ab//'FLSH',' ',0d0,0d0,0d0,ierr,herr)
      goto 500

      end                                              subroutine DBFLSH
c
c ======================================================================
c
      subroutine HSFLSH (h,s,z,T,P,D,Dl,Dv,x,y,q,e,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given bulk enthalpy, entropy, and composition.
c
c  Inputs:
c        h--Overall enthalpy [J/mol]
c        e--Overall internal energy [J/mol]
c           But only if iflag in common blocks has been set to 1, in
c           which case the value of the internal energy should be sent
c           in h, and the value of the enthalpy will be returned in e.
c        s--Overall entropy [J/mol-K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        D--Overall molar density [mol/L]
c     ierr--Error flag:  0 - Successful
c                      260 - Iterative routine is not available to find a solution.
c     herr--Error string (character*255)
c  (See subroutine ABFLSH for the description of all other output variables.)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  12-17-01 EWL, original version
c  05-08-04 OBD, (Concepts NREC) add routine to determine 2-phase states
c  12-07-16 EWL, rewrite code for better efficiency and reading.

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::HSFLSH

c  Locate roots of the isenthalp and the saturation lines.  One or two roots may exist.
      iErrNum=iErrNum+1
      call SATH (h,z,0,nroot,k1,T1,P1,D1,k2,T2,P2,D2,ierr,herr)
      call SETCOMP (icomp,z,x,y)                 !Set output liquid and vapor compositions to input values.
      call SATMAX  (icomp,z,Tc,Pc,Dc,ierr,herr)            !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      T=300
      D=1d-6
      P=0d0
      q=999d0

c  Supercritical state (Isenthalp does not pass through a saturation state).
      if (nroot.eq.0) goto 300

c  Check if isenthalp is a liquid, vapor, or 2-phase state.
      s1=-1d20
      s2= 1d20
      if (nroot.ge.1) call ENTRO (T1,D1,z,s1)
      if (nroot.ge.2) call ENTRO (T2,D2,z,s2)

c  Liquid phase state.
      if (s.lt.s1) goto 300

c  Vapor phase state.
      if (s.gt.s2 .and. nroot.eq.2) goto 300

c  2-phase state.
c     if (icomp.eq.0) goto 260
      P=P1
      do i=1,25
        call PSFLSH (P,s,z,T,D,Dl,Dv,x,y,q,ee,hh,Cv,Cp,w,ierr,herr)
        if (ieflg.eq.1) hh=ee
        if (i.gt.1) then
          dh=hh-h
          if (ABS(dh).lt.1.d-8) EXIT
          dP=10
          if (ABS(hx-hh).gt.1.d-6) dP=(Px-P)*dh/(hx-hh)
          Px=P
          P=P-dP
          if (P.lt.0.5d0*Px) P=0.5d0*Px
        else
          Px=P
          P=0.95d0*P
        endif
        hx=hh
      enddo
      if (ierr.lt.100 .and. i.lt.25) goto 500
      goto 260

c  Call single phase iterative search given h and s.
 300  call HSFL1 (h,s,z,0d0,0d0,T,D,ierr,herr)

c  Check if limits have been exceeded, return that message if no other error has occurred.
 500  iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS ('HS',T,D,P,z,ierr,herr)
      call GETQQ (T,P,D,Tc,Pc,Dc,q)
      call GETPROPS (1,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,hh,ss,Cv,Cp,w)           !Calculate all other properties.
      if (ieflg.eq.1) e=hh   !Return the enthalpy in the energy spot if the energy flag is active.
      RETURN



c   ---------Error Messages---------

 260  call ERRNUM (260,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      goto 500

      end                                              subroutine HSFLSH
c
c ======================================================================
c
      subroutine AQFLSH (T,P,q,z,iFlag,kq,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,
     &                   qmol,ierr,herr)
c
c  Flash calculation given quality, bulk composition, and either
c  temperature or pressure depending on iFlag.
c  This routine accepts saturation or two-phase states as inputs.
c
c  Inputs:
c        T--Temperature [K]  (if iFlag=1)
c        P--Pressure [kPa]   (if iFlag=2)
c        q--Vapor quality (basis specified by the flag kq)
c           The definitions of the values for q are given in the ABFLSH routine.
c           q < 0 and q > 1 are not allowed.
c        z--Overall composition (array of mole fractions)
c    iFlag--Flag specifying the inputs:
c           1 - Inputs are T, q, and z
c           2 - Inputs are P, q, and z
c       kq--Flag specifying units for input quality:
c           1 - Quality on molar basis (moles vapor/total moles)
c           2 - Quality on mass basis (mass vapor/total mass)
c
c  Outputs:
c        T--Temperature [K]  (if iFlag=2)
c        P--Pressure [kPa]   (if iFlag=1)
c        D--Overall molar density [mol/L]
c     ierr--Error flag:  0 - Successful
c                       19 - q out of range
c                      121 - T > Tcrit [or Tmax(sat) in the case of mixtures when SATSPLN has been called]
c                      141 - P > Pcrit [or Pmax(sat) in the case of mixtures when SATSPLN has been called]
c                      159 - iFlag not set correctly
c     herr--Error string (character*255)
c  (See subroutine ABFLSH for the description of all other output variables.)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  04-08-96  MM, original version, based on TPFLSH
c  09-02-16 EWL, combine TQFLSH and PQFLSH into this one routine

      include 'COMMONS.INC'
      character ab*2,herr*255
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::AQFLSH

      iErrNum=iErrNum+1
      call SETCOMP (icomp,z,x,y)       !Set output liquid and vapor compositions to input values.
      call SATMAX  (icomp,z,Tc,Pc,Dc,ierr,herr)            !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      D=0d0
      if (iFlag.eq.1) then
        ab='TQ'
        ifl=1
        P=0d0
        a=T
        T2=T
        if (T.gt.Tc+1.d-8) goto 121    !T>Tc or T>Tmax(sat)
      elseif (iFlag.eq.2) then
        ab='PQ'
        ifl=3
        T=300d0
        a=P
        P2=P
        if (P.gt.Pc+1.d-6 .or. P.le.0.) goto 141           !P>Pc or P>Pmax(sat)
      else
        goto 159
      endif
      if (q.lt.0. .or. q.gt.1.) goto 19          !Quality out of range

c  Call saturation states for q=0 or q=1.
      if (q.le.1d-8) then    !If q is very small, assume saturated liquid.
        call SATTP (T,P,z,ifl,0,D,Dl,Dv,x,y,qq,ierr,herr)            !Saturated liquid
        if (ierr.gt.0) goto 500
      elseif (1d0-q.le.1d-8) then
        call SATTP (T,P,z,ifl+1,0,D,Dl,Dv,x,y,qq,ierr,herr)          !Saturated vapor
        if (ierr.gt.0) goto 500

c  Two-phase states for a pure fluid or pseudo-pure fluid.
      elseif (icomp.ne.0) then
        call SATTP (T,P,z,ifl,0,D,Dl,Dv,x,y,qq,ierr,herr)
        if (ierr.gt.0) goto 500
        if (iAnc(icomp).ne.0) then     !Check for pseudo-pure fluid.
          call SATTP (T2,P2,z,ifl+1,0,D,dd,Dv,x,y,qq,ierr,herr)      !Get pressure and density of vapor.  Liquid properties calculated several lines above.
          if (iFlag.eq.1) P=(1d0-q)*P+q*P2                           !The overall pressure is the average of liquid and vapor pressures, but is only approximate for pseudo-pure fluids.
          if (iFlag.eq.2) T=(1d0-q)*T+q*T2
          if (iFlag.eq.2) call PTANC (T,P,q,D,iLetterQ,Dl,Dv)                  !Call PTANC to find T that is in agreement with iFlag=1.  See details in PTANC routine.
          if (ierr.gt.0) goto 500
        endif

c  Two-phase states for mixtures.
      else
        call ABFL2 (ab,a,q,z,kq,0,Tbub,Tdew,Pbub,Pdew,
     &              Dlbub,Dvdew,ybub,xdew,tt,pp,Dl,Dv,x,y,qq,ierr,herr)
        if (iFlag.eq.1) P=pp
        if (iFlag.eq.2) T=tt
      endif

c  Bulk properties are weighted average of liquid and vapor phases.  For a pure fluid, quality on mass or molar basis is the same.
 500  qmol=q                 !qmol is vapor fraction (molar basis).
      if (kq.eq.2) then
        wmliq=WMOL(x)        !Convert mass quality to molar quality.
        wmvap=WMOL(y)
        if (wmvap.gt.0. .and. wmliq.gt.0.) then
          den=q/wmvap+(1d0-q)/wmliq
          if (lNE0(den)) qmol=q/wmvap/den
        endif
      endif
c  If the limits check results in a warning (as opposed to an error), return that message if no other error has occurred.
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS (ab,T,D,P,z,ierr,herr)
      if (ABS(qmol).lt.1.d-12) qmol=0d0
      if (ABS(1d0-qmol).lt.1.d-12) qmol=1d0
      pp=0d0
      call GETPROPS (2,ierr,qmol,T,pp,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)        !Calculate all other properties.
      RETURN



c   ---------Error Messages---------
 121  call ERRNUM (121,0,'TQFLSH',' ',T,Tc,0d0,ierr,herr)
      goto 500
 141  call ERRNUM (141,0,'PQFLSH',' ',P/1000d0,Pc/1000d0,0d0,ierr,herr)
      goto 500
 159  call ERRNUM (159,0,'AQFLSH','iFlag',0d0,0d0,0d0,ierr,herr)
      goto 500
 19   call ERRNUM (19,0,ab,' ',q,0d0,0d0,ierr,herr)
      goto 500

      end                                              subroutine AQFLSH
c
c ======================================================================
c
      subroutine TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given temperature, pressure, and bulk composition.
c  This routine accepts only two-phase states as inputs; if the phase is
c  not known use TPFLSH.  Use TPRHO for single-phase states.
c
c  Inputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        z--Overall composition (array of mole fractions)
c       Dl--Initial guess for molar density of the liquid phase [mol/L]
c       Dv--Initial guess for molar density of the vapor phase [mol/L]
c        x--Initial guess for composition of the liquid phase (array of mole fractions)
c        y--Initial guess for composition of the vapor phase (array of mole fractions)
c        q--Initial guess for vapor quality on a MOLAR basis (moles vapor/total moles)
c
c  Outputs:
c       Dl--Molar density of the liquid phase [mol/L]
c       Dv--Molar density of the vapor phase [mol/L]
c        x--Composition of the liquid phase (array of mole fractions)
c        y--Composition of the vapor phase (array of mole fractions)
c        q--Vapor quality on a MOLAR basis (moles vapor/total moles)
c     ierr--Error flag:  0 - Successful
c                      213 - TPRHO did not converge
c                      226 - Iteration did not converge
c     herr--Error string (character*255)
c
c  written by M. McLinden, NIST Thermophysics Division, Boulder, Colorado
c  12-29-95  MM, original version, extracted from TPFLSH
c  12-04-09 EWL, add call to new routine of Diego Ortiz
c  12-04-12 EWL, add xinc and xdir to flip values of x(1) back and forth instead of just up
c  03-30-16 EWL, optimize code and remove code that is no longer in use, remove comments about changes before the year 2007

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xs(ncmax),ys(ncmax)
      dimension xk(ncmax),yk(ncmax),fliq(ncmax),fvap(ncmax)
      dimension v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TPFL2

c  Call new code first to see if it finds a root.
      iErrNum=iErrNum+1
      xs(1:nc)=x(1:nc)
      ys(1:nc)=y(1:nc)
      D=0d0
      Dls=Dl
      Dvs=Dv
      if (iSatFlg.eq.0) then
        iTPflg=iTPflg+1
        qin=q
        call SATTP (T,P,z,0,1,D,Dl,Dv,x,y,q,ierr,herr)     !Try SATTP with the initial values sent to this routine.
        if (ierr.gt.0) then
          q=qin
          call SATTP (T,P,z,0,0,D,Dl,Dv,x,y,q,ierr,herr)   !Try SATTP again, letting it calculate the initial guesses.
        endif
        iTPflg=iTPflg-1
        if (ierr.le.0) goto 500
        q=qin
        Dl=Dls
        Dv=Dvs
        x(1:nc)=xs(1:nc)
        y(1:nc)=ys(1:nc)
      endif

c  Begin outer iteration loop for the vapor composition.
      call REDLINEAR (1,z,Tc,Pc,Dc,ierr,herr)    !Call REDLINEAR to get a very quick estimate of the density in the critical region.
      itmax=80
      tolr=1d-9
      xdir=1d0
      xinc=0.01d0
      kguess=0
      do 100 iy=1,itmax
        if (Dl.lt.Dc/10d0) kguess=0
        call TOLRNC (tolr,iy,12,24,50,0,0)            !Loosen tolerance if not converging.
        call TPRHO (T,P,x,-1,kguess,Dl,ierr,herr)     !Compute liquid density.
        call DPDDX (T,Dl,x,dPdD)
        iTPflg=iTPflg+1
        if (ABS(dPdD).gt.1d7)
     &      call SATP (P,x,1,tt,Dl,Dvdew,xk,yk,ierr,herr)
        iTPflg=iTPflg-1
        if (ierr.gt.0) then                           !Try modifying the composition to get a workable solution.
          x(1)=x(1)+xdir*xinc
          x(2:nc)=x(2:nc)-xdir*xinc/DBLE(nc-1)
          xinc=xinc+0.01d0                            !As the composition is slowly modified, do one iteration higher and the next lower than the original values.
          xdir=-xdir
          if (x(1).le.0. .or. x(1).ge.1.) goto 213    !Iteration failed.
          goto 100
        endif
        xdir=1d0
        xinc=0.01d0
        call TPRHO (T,P,y,2,kguess,Dv,ierr,herr)      !Compute vapor density.
        if (ierr.ge.100) then
          y(1:nc)=0.8d0*y(1:nc)+0.2d0*ys(1:nc)        !Try 80% of the current values of y and 20% of the original values.
          goto 100
        endif
        if (Dv.gt.Dl) then
          call SWAPV (Dv,Dl)
          xk(1:nc)=x(1:nc)
          x(1:nc)=y(1:nc)
          y(1:nc)=xk(1:nc)
        endif
        call FGCTY2 (T,Dl,x,fliq,ierr,herr)                                    !Calculate fugacities.
        call FGCTY2 (T,Dv,y,fvap,ierr,herr)
        xk=0d0
        do i=1,nc
          if (fliq(i).eq.xerr .or. fvap(i).eq.xerr) goto 226
          if (fvap(i).eq.0.) fvap(i)=1d0
          if (x(i).gt.0.) xk(i)=fliq(i)*y(i)/(x(i)*fvap(i))                    !Compute k-factors.
        enddo

c  Set upper and lower limits for v (from zero to one).  Allow small tolerance for slop
c   in TPRHO.  New v may go out of bounds when the slope of f vs. x is nearly flat.
        a=0
        call ROOTSEARCH (0,0,0,v,f,a,0,ib,lneg,lpos)                 !Initialize root finding variables.
        v(6)=0d0-1d2*tolr
        v(7)=1d0+1d2*tolr

c  Begin inner iteration loop for the liquid fraction (v).
        ir=1
        v(2)=1d0-q
        a=v(2)*0.99d0
        do it=1,itmax
          do i=1,nc
            xsum=ABS(v(2)+xk(i)*(1d0-v(2)))
            if (xsum.gt.1d-7) x(i)=z(i)/xsum     !Compute new guess for liquid composition and normalize.
          enddo
          xsum=SUM(x(1:nc))
          if (xsum.lt.1d-7) goto 226
          x(1:nc)=x(1:nc)/xsum
          f(2)=1d0-xsum
          if (ABS(f(2)).lt.1d-4*tolr) EXIT                 !Inner loop has converged (tolerance on inner loop must be tighter than outer loop).
          if (ABS(f(2)-f(1)).lt.1d-12 .and. ABS(f(2)).lt.1d-2*tolr) EXIT       !Avoid bisection in ROOTSEARCH routine.
          call ROOTSEARCH (ir,0,11,v,f,a,it,ib,lneg,lpos)                      !Update Reguli-Falsi values and get next guess with the Secant method.
          if (ABS(v(2)-a).lt.1d-15 .and. it.gt.2)
     &       call ROOTSEARCH (6,0,0,v,f,a,it,ib,lneg,lpos)
          ir=1234
          v(2)=a
          q=1d0-a
          if (it.eq.1) v(2)=v(1)+SIGN(0.05d0,0.5d0-v(1))             !Generate next guess for liquid fraction for the first iteration.
        enddo
        ierr=0

c  Compute next guess for vapor composition, with x(i) from inner loop, and normalize.
        y(1:nc)=ABS(y(1:nc)*fliq(1:nc)/fvap(1:nc))
        ysum=SUM(y(1:nc))
        if (ysum.le.0.) goto 226
        y(1:nc)=y(1:nc)/ysum
        if (ABS(1d0-ysum).lt.tolr .and. iy.gt.1) goto 400            !------------Outer loop has converged------------
 100  continue

c   ---------Error Messages---------
 226  call ERRNUM (226,0,'TPFL2',' ',0d0,0d0,0d0,ierr,herr)
      goto 500
 213  call ERRNUM (213,0,'TPFL2',' ',0d0,0d0,0d0,ierr,herr)
      goto 500

 400  if (ABS(Dl-Dv)/Dl.lt.1d-3 .and. ABS(q).lt.1d-9) goto 226

c  Finalize routine.
 500  iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS ('TP',T,Dl,P,z,ierr,herr)
      RETURN

      end                                               subroutine TPFL2
c
c ======================================================================
c
      subroutine TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given temperature, density, and bulk composition.
c  This routine accepts only two-phase states as inputs; if the phase is
c  not known use TDFLSH.  Use TDFL1 for single-phase states.
c
c  Inputs:
c        T--Temperature [K]
c        D--Density [mol/L]
c        z--Overall composition (array of mole fractions)
c
c  Outputs:
c        P--Pressure [kPa]
c       Dl--Density of the liquid phase [mol/L]
c       Dv--Density of the vapor phase [mol/L]
c        x--Composition of the liquid phase (array of mole fractions)
c        y--Composition of the vapor phase (array of mole fractions)
c        q--Vapor quality on a MOLAR basis (moles vapor/total moles)
c     ierr--Error flag:  0 - Successful
c                      226 - Iteration did not converge
c     herr--Error string (character*255)

      include 'COMMONS.INC'
      character*255 herr,herr1
      dimension x(ncmax),y(ncmax),z(ncmax),xTDFL2(ncmax),yTDFL2(ncmax)
      dimension xdewTDFL2(ncmax),ybubTDFL2(ncmax),v(20),f(20)
      common /TDFL2save/ PTDFL2,DlTDFL2,DvTDFL2,xTDFL2,yTDFL2,qTDFL2,
     &                   xdewTDFL2,ybubTDFL2,PbubTDFL2,PdewTDFL2
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TDFL2

      iErrNum=iErrNum+1
      ierr=0
      itmax=40
      tolr=1d-8
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      lInit=.false.
      if (T.lt.0.) then
        lInit=.true.
        T=ABS(T)
        P=PTDFL2
        x(1:nc)=xTDFL2(1:nc)
        y(1:nc)=yTDFL2(1:nc)
        Dl=DlTDFL2
        Dv=DvTDFL2
        q=qTDFL2
      else
        q=0d0
      endif
      if (D.le.0. .or. T.le.0.) goto 143
      iFlg1=0
      iFlg2=0
      it1=0
      it2=1

c  Calculate saturated liquid properties, PbubTDFL2 is the upper pressure limit.
 10   ierr1=0
      if (.not.lInit) then
        x(1:nc)=z(1:nc)
        Dl=D
        call SATGUESS (1,1,0,0,z,T,P,Dl,h,s,Dv,ybubTDFL2,ierr1,herr1)
        if (Dv.gt.Dl) call SWAPV(Dv,Dl)
        call GETQ (2,T,P,D,Tc,Pc,Dc,D,Dl,Dv,q)
      endif
      iSkip=1
      if (ABS(q).lt.1d-3 .or. ierr1.ge.100 .or. iFlg1.eq.1) iSkip=0            !For the first test, inputs to SATGUESS are too close to saturation, call SATT to get better values.
      if (lInit) iSkip=0
      if (icomp.ne.0) iSkip=0
      if (iSkip.eq.0) then
        call SATT (T,z,1,P,Dl,Dv,x,ybubTDFL2,ierr,herr)
        call GETQ (2,T,P,D,Tc,Pc,Dc,D,Dl,Dv,qq)
        if (ABS(qq).lt.100.) q=qq      !Check for bad q from Dl and Dv.
        if (icomp.ne.0 .and. iAnc(icomp).eq.0) goto 500    !Routine is finished for a pure fluid.
        iFlg1=2
      endif
      if (ABS(q).lt.tolr) goto 500     !Input conditions are nearly saturated liquid.
      if (D.gt.Dl .or. Dv.le.0.) goto 226

c  Calculate saturated vapor properties, PdewTDFL2 is the lower pressure limit.
      if (.not.lInit .or. iAnc(icomp).ne.0) then
        PbubTDFL2=P
        call SATGUESS (2,1,0,0,z,T,PdewTDFL2,Dv,h,s,Dll,xdewTDFL2,
     &                 ierr1,herr1)
        if (ierr1.ge.100 .or. iAnc(icomp).ne.0 .or. iFlg2.eq.1 .or.
     &     ABS(D-Dv).lt.D/10d0) then
          call SATT (T,z,2,PdewTDFL2,Dll,Dv,xdewTDFL2,y,ierr,herr)   !Inputs to SATGUESS are too close to saturation, call SATT to get better values.
          iFlg2=2
        endif
        call GETQ (2,T,P,D,Tc,Pc,Dc,D,Dl,Dv,q)
        if (lEQ0(PbubTDFL2)) PbubTDFL2=1d3
        if (lEQ0(PdewTDFL2)) PdewTDFL2=PbubTDFL2
        P=(1d0-q)*PbubTDFL2+q*PdewTDFL2
        if (ABS(q-1d0).lt.tolr) then
          y(1:nc)=z(1:nc)
          goto 500           !Input conditions are nearly saturated vapor.
        endif
        if (iAnc(icomp).ne.0) goto 500           !Exit for a pseudo-pure fluid.
        if (D.lt.Dv .or. Dv.le.0.) goto 226
        call SAT0EST (T,P,z,x,y,ierr1,herr1)     !Get initial estimates
        if (ierr1.gt.0) call GETXYZQ (1,1,q,z,xdewTDFL2,ybubTDFL2,x,y)         !Get x and y estimates from the current values of q, xdewTDFL2, and ybubTDFL2.
      endif
      lInit=.false.

      pp=0
      call ROOTSEARCH (0,0,0,v,f,pp,0,ib,lneg,lpos)        !Initialize root finding variables.
      Da=-XLOG(D)
      P2=0.99d0*P+0.01d0*PdewTDFL2
      if (iFlg1.eq.2 .and. iFlg2.eq.2) then
        if (ABS(PbubTDFL2-PdewTDFL2)/PdewTDFL2.gt.1d-7) then
          v(6)=PdewTDFL2     !Minimum value stored in the 6th slot
          v(7)=PbubTDFL2     !Maximum value stored in the 7th slot
          v(2)=PbubTDFL2
          f(2)=Da+XLOG(Dl)
          call ROOTSEARCH (1,0,0,v,f,pp,0,ib,lneg,lpos)    !Store value of objective function at Min
        else
          P2=P2*1.1d0
        endif
        v(2)=PdewTDFL2
        f(2)=Da+XLOG(Dv)
        call ROOTSEARCH (1,0,0,v,f,pp,0,ib,lneg,lpos)      !Store value of objective function at Max
      endif
      Pint=P

c  Start of main loop.
      ir=1
      qin=q
      Pdiff=ABS(PbubTDFL2-PdewTDFL2)*1.d-5
      if (Pdiff.lt.1d-6) Pdiff=1.d-5
      do it=it1+1,itmax
        it1=it
        call TOLRNC (tolr,it,10,15,20,0,0)       !Loosen tolerance if not converging.
        call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
        if (q.ge.1d0 .and. (q-1d0).lt.1d-10 .and. ierr.eq.0) then
          P=(P+v(2))/2d0
          call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
          if (q.ge.1d0 .and. (q-1d0).lt.1d-10 .and. ierr.eq.0) ierr=999
        endif
        if (P.lt.PdewTDFL2 .and. ierr.ge.100) then
          P=PdewTDFL2*(1.d0+DBLE(it)/5d0)
          call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
        endif
        if (ierr.ge.100) then
          if (q.le.-1d0 .or. q.ge.2d0) goto 1218
          it2=it2+1
          if (it2.gt.3 .and. (iFlg1.le.1 .or. iFlg2.le.1)) then
            if (iFlg1.eq.0) iFlg1=1
            if (iFlg2.eq.0) iFlg2=1
            goto 10
          endif
          i=1
          if (INT(it2/2)*2.eq.it2) i=-1
          qinc=0.002d0
          if (q.lt.0.1d0) qinc=0.02d0
          q=qin*(1d0+qinc*DBLE(i*(it2**3)))
          if (q.le.0..or.q.ge.1.) q=ABS(qin*(1d0-qinc*DBLE(i*(it2**2))))
          P=Pint*(1d0+Pdiff*DBLE(i*it2))         !Make P bounce around faster and faster.
          call SAT0EST (T,P,z,x,y,ierr1,herr1)
          if(ierr1.gt.100) call GETXYZQ(1,1,q,z,xdewTDFL2,ybubTDFL2,x,y)       !Get x and y estimates from the current values of q, xdewTDFL2, and ybubTDFL2.
          goto 150
        endif
        Dd=1d0/(q/Dv+(1d0-q)/Dl)
        v(2)=P
        if (Dd.gt.0) then
          f(2)=Da+XLOG(Dd)
        else
          f(2)=Da-10d0+Dd    !Density went negative, add (actually subtract) a big penalty to push it positive again.
        endif
        if (ABS(f(2)).lt.tolr .and. it.gt.1) goto 500                          !------------Iteration has converged------------
        iFlag=0
        if (ABS(f(1)-f(2)).lt.1d-15 .and. it.gt.2) iFlag=1
        call ROOTSEARCH (ir,0,0,v,f,P,it,ib,lneg,lpos)               !Update Reguli-Falsi values and get next guess with the Secant method.
        if (ABS(P-v(2)).lt.1d-15 .and. it.gt.2) iFlag=1
        if (iFlag.eq.1) call ROOTSEARCH (6,0,0,v,f,P,it,ib,lneg,lpos)          !Use bisection for bad roots.
        if (ABS(q).lt.1d-9 .and. it.gt.8) q=qin
        ir=1234
        if (it.eq.1) P=P2
 150  enddo
      goto 226     !Iteration failed.

c  Finalize routine.
 500  if (ABS(q)    .lt.1d-12) q=0d0
      if (ABS(q-1d0).lt.1d-12) q=1d0
      iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      if (ierr.lt.100) then
        PTDFL2=P
        xTDFL2(1:nc)=x(1:nc)
        yTDFL2(1:nc)=y(1:nc)
        DlTDFL2=Dl
        DvTDFL2=Dv
        qTDFL2=q
      endif
      RETURN

c   ---------Error Messages---------
 1218 herr(3:3)='D'
      goto 500
 143  call ERRNUM (143,0,'TDFL2',' ',D,0d0,0d0,ierr,herr)
      goto 500
 226  call ERRNUM (226,0,'TDFL2',' ',0d0,0d0,0d0,ierr,herr)
      goto 500

      end                                               subroutine TDFL2
c
c ======================================================================
c
      subroutine DBFL1 (D,b,z,ab,T,P,ierr,herr)
c
c  General single-phase calculation given density, composition, and either
c  pressure, energy, enthalpy, or entropy.  The character string ab
c  specifies the inputs.  This routine should ONLY be called by ABFL1.
c
c  Inputs:
c        D--Molar density [mol/L]
c        b--Second property (pressure, energy, enthalpy, or entropy)
c        z--Composition (array of mole fractions)
c       ab--Character*2 string defining the four inputs 'DH', 'DS', 'DE', or 'DP'
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c     ierr--Error flag:  0 - Successful
c                      207 - Density or pressure equal to zero, no solution available
c                      208 - Iteration did not converge
c     herr--Error string (character*255)
c
c  rewritten by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-18-01 EWL, original version
c  05-25-12 EWL, move calculation of P and dPdT inside IF block

      include 'COMMONS.INC'
      character at*1,bt*1,ab*2,ab2*2,herr*255
      dimension z(ncmax),v(20),f(20)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DBFL1

      a=0d0
      iErrNum=iErrNum+1
      call GETINPUTS (ab,ab2,at,bt,iat,ibt,a,b,aa,bb,T,Dd,P,ierr,herr)
      if (D.le.0) then
        if (D.lt.0) goto 207
        if (ibt.eq.iLetterP .or. ibt.eq.iLetterS) goto 207           !No solution for P or S if density is zero.
        T=300d0              !For E or H, call ROOTFIND to get the ideal gas temperature.
       call ROOTFIND (' ',ibt,0,b,z,2,0,1,0,0,0,T,D,o,ierr,herr)
        goto 500
      endif
      if (ibt.eq.iLetterP .and. b.lt.1d-14 .and. b.ge.0d0) goto 207  !Check for ideal-gas state (but for positives pressures only).
      call Rmix (z)
      call ISPURE (z,icomp)
      call SATMAX (icomp,z,Tc,Pc,Dc,ierr,herr)             !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      ierr=0
      herr=' '
      itmax=30
      tolr=1d-10
      b2=b
      v3=0
      if (ABS(b).lt.1.) b2=1d0
      T=Tc*1.1d0                                 !Set initial guess for temperature.
      if (iat.ne.iLetterD .and. ibt.eq.iLetterP) goto 100

c  Enter Newton's method iteration.
      do it=1,itmax
        call TOLRNC (tolr,it,10,20,0,0,0)        !Loosen tolerance if not converging.
        if (ibt.eq.iLetterP) then
          call PRESS (T,D,z,b1)
          call DPDTX (T,D,z,dbdt)
        else
          call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
          if (ibt.eq.iLetterH) then
            b1=h
            call DPDTX  (T,D,z,dPdT)
            dbdt=Cv
            if (D.gt.0.) dbdt=Cv+dPdT/D          !dh/dT at constant D
          elseif (ibt.eq.iLetterE) then
            b1=e
            dbdt=Cv                              !de/dT at constant D
          elseif (ibt.eq.iLetterS) then
            b1=s
            dbdt=Cv/T                            !ds/dT at constant D
          else
            goto 601
          endif
        endif
        if (ABS(dbdt).lt.1d-10) dbdt=1d-10
        Tinc=(b-b1)/dbdt
        if(T.lt.Tc*1.5d0.and.ABS(Tinc/T).gt.0.25d0)Tinc=SIGN(T/4d0,Tinc)
        if (Tinc.gt.500) Tinc=500d0
        T=T+Tinc
        if (T.le.0.1d0) T=Tc/2d0

c  Iteration has converged.
        if (ABS((b-b1)/b2).lt.tolr) goto 500
      enddo

c  Iteration has not converged.
      call ERRNUM (208,0,ab//'FL1',' ',D,0d0,0d0,ierr,herr)
      goto 400
 207  call ERRNUM (207,0,ab//'FL1',' ',0d0,0d0,0d0,ierr,herr)
      goto 400
 601  call ERRNUM (601,0,'DBFL1',bt,0d0,0d0,0d0,ierr,herr)
 400  T=300d0
      P=0d0

 500  call PRESS (T,D,z,P)
      iErrNum=iErrNum-1
      if (iErrNum.le.0) call CHECKLIMITS (ab,T,D,P,z,ierr,herr)
      RETURN




 100  continue

      if (b.gt.Pc*100d0) T=Tc*2
      if (D.lt.Dc/10d0)  T=b/(D*Rxgas)

      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.
      do it=1,itmax
        call TOLRNC (tolr,it,10,15,20,25,30)     !Loosen tolerance if not converging.
        call PRESS (T,D,z,b1)
        call DERVPVT (T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdDT,
     &                      dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                      dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
        iFl=0
        if (dPdD.gt.0.) iFl=1
        if (icomp.ne.0 .and. T.gt.Tc) iFl=1
        if (lEq0(d2PdT2/D) .or. lEq0(dPdT)) iFl=0
        if (ABS(b-b1).lt.1d-20 .or. lEq0(b2)) iFl=0
        if (XDIV(b-b1,b2).lt.tolr/100d0 .and. it.gt.1) goto 500
        if (iFl.eq.1) then
          f(2)=b-b1
          v(2)=T
          call ROOTSEARCH (1,0,0,v,f,T,0,ib,lneg,lpos)     !Store calculated values for Reguli-Falsi use.
          Tinc=1d0/(d2PdT2/2d0/dPdT-dPdT/(b-b1))           !2nd order Newton's method.
          if (ABS(Tinc).gt.100.)  Tinc=-(b-b1)/dPdT        !If 2nd order Newton's method gives unreasonable result, revert to first order method.
          if (ABS(d2PdT2).gt.10.) Tinc=-(b-b1)/dPdT
          do while (ABS(Tinc).gt.ABS(0.3d0*T))
            Tinc=Tinc/2d0
          enddo
          T=T-Tinc
          if (ABS((b-b1)/b2).lt.tolr) goto 500
          v(2)=T
          call ROOTSEARCH (3,0,0,v,f,T,it,ib,lneg,lpos)              !Check if new value is further from solution.  If so, use reguli-falsi.
          if (it.gt.10) call ROOTSEARCH (6,0,0,v,f,T,it,ib,lneg,lpos)          !Use bisection if dP/dD < 0 to get next guess.
        else
          call ROOTSEARCH (6,0,0,v,f,T,it,ib,lneg,lpos)    !Use bisection if dP/dD < 0 to get next guess.
        endif
      enddo

      goto 500

      end                                               subroutine DBFL1
c
c ======================================================================
c
      subroutine TOLRNC (tolr,i,i1,i2,i3,i4,i5)
c
c  Internal routine to other subroutines in this file.  Not meant to be
c  used elsewhere.
c
      include 'COMMONS.INC'

      if (i.eq.i1) tolr=tolr*10d0
      if (i.eq.i2) tolr=tolr*10d0
      if (i.eq.i3) tolr=tolr*10d0
      if (i.eq.i4) tolr=tolr*10d0
      if (i.eq.i5) tolr=tolr*10d0
      end                                              subroutine TOLRNC
c
c ======================================================================
c
      subroutine SETCOMP (icomp,z,x,y)
c
c  Set output liquid and vapor compositions to input values.
c
      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax)

      call ISPURE (z,icomp)
      if (icomp.eq.0) then
        x(1:nc)=z(1:nc)
        y(1:nc)=z(1:nc)
      else
        x(1:nc)=0d0
        y(1:nc)=0d0
        x(icomp)=1d0
        y(icomp)=1d0
      endif
      end                                             subroutine SETCOMP
c
c ======================================================================
c
      subroutine HESCALC (ibt,T,D,z,hes)
c
c  Internal routine to other subroutines in this file.  Not meant to be
c  used elsewhere.
c
      include 'COMMONS.INC'
      dimension z(ncmax)

      hes=0d0
      if (T.le.0) RETURN
      call THERM1 (T,D,z,P,e,h,s,g)
      if (ibt.eq.iLetterH) then
        hes=h
      elseif (ibt.eq.iLetterE) then
        hes=e
      elseif (ibt.eq.iLetterS) then
        hes=s
      endif
      end                                             subroutine HESCALC
c
c ======================================================================
c
      subroutine GETXYZQ (iFlg,ix,q,z,x2,y2,x,y)
c
c  Internal routine to other subroutines in this file.  Not meant to be
c  used elsewhere.
c
      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax),x2(ncmax),y2(ncmax)

      if (iFlg.eq.1) then
        if (q.gt.1.) q=0.95d0
        if (q.lt.0.) q=0.05d0
        x(1:nc)=(1d0-q)*z(1:nc)+q*x2(1:nc)
        y(1:nc)=q*z(1:nc)+(1d0-q)*y2(1:nc)
        xsum=SUM(x(1:nc))
        ysum=SUM(y(1:nc))
        if (xsum.gt.0.) x(1:nc)=x(1:nc)/xsum
        if (ysum.gt.0.) y(1:nc)=y(1:nc)/ysum
      elseif (iFlg.eq.2) then
        xd=0.8d0
        cn=1d0/DBLE(nc)
        q2=q
        do j=1,3
          do i=1,nc
            if (xd.lt.1) then
              if (x2(i).gt.cn) x(i)=1d0-(1d0-x2(i))*xd
              if (x2(i).le.cn) x(i)=x2(i)*xd
c             if (y2(i).gt.cn) y(i)=1d0-(1d0-y2(i))/xd
              if (y2(i).le.cn) y(i)=y2(i)/xd
            else
              if (x2(i).gt.cn) x(i)=x2(i)/xd
              if (x2(i).le.cn) x(i)=1d0-(1d0-x2(i))/xd
              if (y2(i).gt.cn) y(i)=y2(i)*xd
c             if (y2(i).le.cn) y(i)=1d0-(1d0-y2(i))*xd
            endif
          enddo
          x(1:nc)=x(1:nc)/SUM(x(1:nc))
          y(1:nc)=y(1:nc)/SUM(y(1:nc))
          q=XDIV1(z(ix)-x(ix),y(ix)-x(ix))
          if (q.gt.-1d0 .and. q.lt.2d0) EXIT
          xd=1d0-(1d0-xd)*ABS(XDIV1(q2,q2-q))*SIGN(1d0,ABS(q2)-ABS(q))
          if (xd.lt.0.6d0) xd=0.6d0
          if (xd.gt.1.5d0) xd=1.5d0
        enddo
      endif
      end                                             subroutine GETXYZQ
c
c ======================================================================
c
      subroutine GETQ (iFlag,T,P,D,Tc,Pc,Dc,b,bl,bv,q)
c
c  Determine the value of q, based off the following criteria
c  Quality undefined - P>Pc and T>Tc
c  Subcooled - T<Tc and (P>Pc or D>Dc)
c  Superheated - P<Pc and (T>Tc or D<Dc)

      include 'COMMONS.INC'


c  If iFlag=1, calculate q based on enthalpy, entropy, etc.
      if (iFlag.eq.1 .and. T.lt.Tc) then
        if (ABS(bl-bv).gt.1d-12) then
          q=(b-bl)/(bv-bl)
          RETURN
        endif

c  If iFlag=2, calculate q based on density.
      elseif (iFlag.eq.2) then
        if (bv.le.0.) bv=Dc
        if (bl.le.0.) bl=Dc
        if (bv.gt.bl) call SWAPV (bv,bl)
        q=999d0
        if(lNE0(ABS(bv-bl)).and.lNE0(b))q=(1d0/b-1d0/bl)/(1d0/bv-1d0/bl)

c  If iFlag=3, calculate q based on energy, enthalpy, or entropy.
      elseif (iFlag.eq.3) then
        q=999d0
        if (ABS(bv-bl).gt.1d-12) q=(b-bl)/(bv-bl)
      endif

      call GETQQ (T,P,D,Tc,Pc,Dc,q)
c  Calculate q based on T, P, and D.
      if (ABS(q).lt.100.) then
         !2-phase or single phase near saturation conditions.
      elseif (P.ge.Pc .and. T.ge.Tc) then
        q=999d0                        !Quality undefined
      elseif (P.ge.Pc) then
        q=-998d0                       !Subcooled
      elseif (T.ge.Tc) then
        q=998d0                        !Superheated
      elseif (D.ge.Dc) then
        q=-998d0                       !Subcooled
      elseif (D.le.Dc) then
        q=998d0                        !Superheated
      else
        q=999d0
      endif
      end                                                subroutine GETQ
c
c ======================================================================
c
      subroutine GETQQ (T,P,D,Tc,Pc,Dc,q)
c
c  Determine the value of q, based off the following criteria
c  Quality undefined - P>Pc and T>Tc
c  Subcooled - T<Tc and (P>Pc or D>Dc)
c  Superheated - P<Pc and (T>Tc or D<Dc)

      include 'COMMONS.INC'

c  Calculate q based on T, P, and D.
      if (ABS(q).lt.100.) then
         !2-phase or single phase near saturation conditions.
      elseif (P.ge.Pc .and. T.ge.Tc) then
        q=999d0                        !Quality undefined
      elseif (P.ge.Pc) then
        q=-998d0                       !Subcooled
      elseif (T.ge.Tc) then
        q=998d0                        !Superheated
      elseif (D.ge.Dc) then
        q=-998d0                       !Subcooled
      elseif (D.le.Dc) then
        q=998d0                        !Superheated
      else
        q=999d0
      endif
      end                                               subroutine GETQQ
c
c ======================================================================
c
      subroutine GETPROPS (iFlg,i,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)
c
c  Compute properties for single or 2-phase states.  Bulk properties are
c  weighted average of liquid and vapor phases.
c
c  Internal routine to other subroutines in this file.  Not fully meant
c  to be used elsewhere.
c
c  Input:
c
c  iFlg -- 0 - Calculate properties based on q, but don't return P or D.
c        1 - Same as zero, but return P as well.
c        2 - Same as zero, but return D as well.
c        3 - Return mass based values (The input q should always be
c            sent on a mole basis).  Both P and D are returned based
c            on T, Dl, Dv, x, and y.

      include 'COMMONS.INC'
      dimension x(ncmax),y(ncmax),z(ncmax)

      if (ABS(q)     .lt.1d-10 .and. D.le.0.) D=Dl
      if (ABS(q-1.d0).lt.1d-10 .and. D.le.0.) D=Dv
      qkg=q
      if (q.gt.0. .and. q.lt.1. .and. i.lt.100) then
        call THERM1 (T,Dl,x,pl,el,hl,sl,g)
        call THERM1 (T,Dv,y,pv,ev,hv,sv,g)
        a=1d0-q              !q is vapor fraction and a is liquid fraction.
        dd=(Dl+Dv)/2d0       !Return a default for D incase Dl or Dv is negative.
        if (Dl.gt.0. .and. Dv.gt.0.) dd=1d0/(a/Dl+q/Dv)
        pp=q*pv+a*pl         !pl and pv should be the same for a pure fluid, but not a pseudo-pure fluid.
        e=q*ev+a*el
        h=q*hv+a*hl
        s=q*sv+a*sl
        w=xnotd              !Cp and w not defined for 2-phase states
        Cp=xnotd
        Cv=xnotc
        if (iFlg.eq.2) D=dd
        if (iFlg.eq.3) THEN
          wliq=WMOL(x)
          wvap=WMOL(y)
          qq=q*wvap+(1d0-q)*wliq
          if (lNE0(qq)) then
            qkg=q*wvap/qq
            D=1d0/(XDIV(qkg,Dv*wvap)+XDIV(1d0-qkg,Dl*wliq))
            e=qkg*ev/wvap+(1d0-qkg)*el/wliq
            h=qkg*hv/wvap+(1d0-qkg)*hl/wliq
            s=qkg*sv/wvap+(1d0-qkg)*sl/wliq
c           g=qkg*gv/wvap+(1d0-qkg)*gl/wliq
            P=pp
          endif
        endif
      else
        if (i.ge.100) q=999d0          !Quality undefined
        if (q.lt.-1d-100 .or. q.gt.1+1d-100) then          !Don't reset this if at saturation, only in the single phase.
          x(1:nc)=z(1:nc)
          y(1:nc)=z(1:nc)
          Dl=D
          Dv=D
        endif
        if (nc.eq.1) x(1)=1d0
        if (nc.eq.1) y(1)=1d0
        call THERM (T,D,z,pp,e,h,s,Cv,Cp,w,hjt)
      endif
      if (iFlg.eq.1) P=pp
      end                                            subroutine GETPROPS
c
c ======================================================================
c
      subroutine VFBOUNDS (T,P,x,kph,v,f,vlog,Tc,Dc,lneg,lpos)
c
c  This is an internal routine to the TPRHO subroutine, and used to find
c  a second root that bounds the input state.  All input variables
c  are as used in TPRHO.

      include 'COMMONS.INC'
      dimension x(ncmax),v(20),f(20)


      if (lneg.and.lpos) RETURN        !Exit if already bounded.
      Din=EXP1(-v(2))                  !Input density is obtained from the v array.
      v3=0
      do i=1,5
        if (T.gt.Tc-1d-6) then         !For T>Tc, use large jumps.
          if (i.eq.1) D=Din/2d0
          if (i.eq.2) D=Din*2d0
          if (i.eq.3) D=Din*3d0
          if (i.eq.4) D=Din/3d0
          if (i.eq.5) D=Din/5d0
        else
          if (i.eq.1) D=Din*1.02d0
          if (i.eq.2) D=Din*0.98d0
          if (i.eq.3) D=Din*1.1d0      !For T<Tc, use small jumps initially to stay within the same phase...
          if (i.eq.4 .and. kph.eq.1) D=Din*1.5d0           ! ...and then take more drastic measures in case root is within the two phase.
          if (i.eq.4 .and. kph.eq.2) D=Din/2d0
          if (i.eq.5 .and. kph.eq.1) D=Din*2d0
          if (i.eq.5 .and. kph.eq.2) D=Din/5d0
        endif
        call PRESS (T,D,x,P2)
        call DPDDX (T,D,x,dPdD)
        iFlag=1
        if (dPdD.lt.0.) iFlag=0        !Check for bad roots.
        if (T.lt.Tc-1d-6 .and. D.lt.Dc .and. kph.eq.1) iFlag=0
        if (T.lt.Tc-1d-6 .and. D.gt.Dc .and. kph.eq.2) iFlag=0
        if (iFlag.eq.1 .and. D.gt.0.) then
          f(2)=P-P2                    !Good root found, place in v and f arrays.
          v(2)=-XLOG(D)
          ib=1
          call ROOTSEARCH (1,1,0,v,f,v3,0,ib,lneg,lpos)     !Store bounds.
          if (lneg.and.lpos) then      !Exit if input state is now bounded.
            vlog=-XLOG((D+Din)/2d0)    !Return a new solution that is half way between the new root and the input density.
            RETURN
          endif
        endif
      enddo

      end                                            subroutine VFBOUNDS
c
c ======================================================================
c
      subroutine DHESLMTS (iFlag,kr,T,Tc,b,z,ibt,Dmin,Dmax,Dmid,
     &                     bMin,bMax,bsat,Psat,Dl,Dv,zsat,ierr,herr)
c
c  This is an internal routine to other subroutines in this file.  It
c  locates the bounds for energy, enthalpy, and entropy.
c  The minimum value of E and H is a physical limit, not just set by the EOS.
c  No root exists below that value.
c
      include 'COMMONS.INC'
      character herr*255
      dimension z(ncmax),zsat(ncmax),xy(ncmax),yx(ncmax)

      iRflg=0
      Dmid=0d0
      DPmax=0d0
      bsat=0d0
      call ISPURE (z,icomp)
      if (iFlag.eq.0) then
        call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)          !Get EOS Dmax
        call TPRHO (T,Pmax*2d0,z,1,0,DPmax,ierrp,herr)     !Get D at T and Pmax.
        call SATT (T,z,3,Pmelt,Dmelt,dd,xy,yx,ierrm,herr)  !Get D at melting line.
        if (Dmelt.le.0.) Dmelt=Dmax
        if (ierrm.gt.0) Dmelt=Dmax
        if (ierrp.gt.0) DPmax=Dmax

c  Get minimum and maximum densities, along with E, H, and S at these points.
        Dmin=0d0
        if (ibt.eq.iLetterS) Dmin=1d-4           !Limit Dmin to very small value for entropy (to avoid an infinite value at D=0.)
        if (Dmax.gt.Dmelt) Dmax=Dmelt            !Limit Dmax to the density at the melting line at the input T.
        if (Dmax.gt.DPmax) Dmax=DPmax            !Limit Dmax to the density at the upper pressure limit at the input T.
        call HESCALC (ibt,T,Dmin,z,bDmin)        !b at Dmin
        call HESCALC (ibt,T,Dmax,z,bDmax)        !b at Dmax

c  Get minimum values for E, H, and S.
        bMin=bDmax                               !This is always true for entropy.
        if (bDmin.lt.bMin) bMin=bDmin            !The minimum b for E or H occurs at zero density when the temperature is high.

c  Get maximum values for E, H, and S.
        bMax=bDmin                               !Maximum for S always occurs at very low densities (actually at zero density where it is infinite).
        if (bDmax.gt.bMax) bMax=bDmax            !Maximum for E or H occurs at zero density for low temperatures, and at the maximum density for high temperatures.  (strange stuff... Look at a TH diagram to see this.)

        if (T.lt.Tc) then
          call SATT (T,z,2,Psat,Dl,Dv,zsat,xy,ierr,herr)             !Saturated vapor
          call HESCALC (ibt,T,Dv,z,bsat)
          if (b.ge.bsat .and. kr.ne.2) Dmax=Dv
        endif

c  Find the minimum H or E when two roots exist.
        if (ibt.ne.iLetterS .and. T.gt.Tc) then
          call HESCALC (ibt,T,0.001d0,z,bb)      !Check if a minimum in H or E exists, which happens when
          if (bb.lt.bDmin*1.01d0)  iRflg=1       ! the slope (dH/dD) is negative at zero density.
          dd=Dmax/2d0        !Initial guess
        endif

c  Find Dmax in the liquid and the minimum H or E when two root exist.
      else
        if (T.lt.Tc) then
          if (icomp.eq.0 .or. iAnc(icomp).ne.0)
     &      call SATT (T,z,1,Psat,Dl,Dv,xy,zsat,ierr,herr)           !Saturated liquid
          call HESCALC (ibt,T,Dl,z,bsat)
          if (b.lt.bsat) Dmin=Dl
          if (bsat.lt.bMin) bMin=bsat
          if (ibt.ne.iLetterS) then
            call HESCALC (ibt,T,Dl+1d-6,z,bb1)   !Check slope of b vs. D.
            if (bb1.lt.bsat) iRflg=1
            if (bb1.ge.bsat) Dmin=Dl
            dd=(Dl+Dmax)/2d0           !Initial guess
          endif
        endif
      endif

c  Two roots exist - find actual minimum b to bound solution.
      if (iRflg.eq.1) then
        call ROOTFIND (' ',ibt,0,0d0,z,1,1,0,0,0,0,T,dd,bb,ierr,herr)
        if (ierr.lt.100) Dmid=dd
        if (ierr.lt.100 .and. bb.lt.bsat) Bmin=bb
      endif

      end                                            subroutine DHESLMTS
c
c ======================================================================
c
      subroutine LIQSPNDL (T,z,D,ierr,herr)
c
c  Find the liquid spinodal density for a given temperature.  If no
c  spinodal exists, return the point of zero curvature.  This only
c  happens with a few of the older equations, these being argon, ethane,
c  nitrogen, R22, and R124.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        D--Density at liquid spinodal [mol/L]
c     ierr--Error flag:  0 - Successful
c                      121 - T>Tc
c                      633 - Failed to converge
c                     -638 - Spinodal not found, point of zero curvature returned
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-28-06 EWL, original version taken from code in SATT
c  09-15-11  MK, split subroutine SPNDL into liquid and vapor routines
c  08-11-16 EWL, return point of zero curvature if spinodal not found
c  03-11-17 EWL, rewrite code so that the main loop will find either the spinodal or point of zero curvature simultaneously

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax),y(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::LIQSPNDL

      iErrNum=iErrNum+1
      call ISPURE (z,icomp)
      call CRITP (z,Tc,Pc,Dc,ierr,herr)
      ierr=0
      D=Dc
      if (T.gt.Tc+1d-8 .and. icomp.ne.0) goto 121
      if (T.gt.Tc-1d-5 .and. icomp.ne.0) goto 500          !For a pure fluid very near the critical point, just return Dc.
      call SATGUESS (1,1,0,0,z,T,P,D,h,s,Dv,y,ierr,herr)

      Ds=D
      Dmin=0d0
      Dmax=1000d0
      Dinc=10d0    !Start at 10 in case first value is smaller than spinodal and need to increase D.
      tolr=1d-8
      do it=1,100
        call TOLRNC (tolr,it,10,15,20,25,30)               !Loosen tolerance if not converging.
        if (D.le.0.) goto 633
        call DPDDX (T,D,z,dPdD)
        call DPDD2 (T,D,z,d2PdD2)
        if (dPdD.gt.0. and. d2PdD2.gt.0. .and. D.lt.Dmax) Dmax=D     !Location where a valid root still exists.
        if (d2PdD2.gt.0.) then                             !Check for positive curvature.  If negative, the isotherm may not have a spinodal.
          Ds=D
          Dinc=-dPdD/d2PdD2
          if (ABS(Dinc).gt.D/10d0) Dinc=SIGN(D/10d0,Dinc)
          if (D+Dinc.gt.Dmax) Dinc=(Dmax-D)/2d0            !Keep less than the known valid state.
          if (D+Dinc.lt.Dmin) Dinc=(Dmin-D)/2d0            !Keep more than the point with negative curvature.
          if (INT(it/10)*10.eq.it) Dinc=Dinc/2d0
          D=ABS(D+Dinc)
        else
          if (D.gt.Dmin) Dmin=D                  !Set the location just found that has negative curvature (which is thus a state point beyond the desired root).
          Dinc=Dinc/2d0
          D=ABS(Ds+Dinc)                         !Go back towards the known good root.
        endif
        if (ABS(dPdD)  .lt.tolr) goto 400        !Converged on a spinodal.
        if (ABS(d2PdD2).lt.tolr) goto 638        !Converged on a change in curvature from positive to negative (thus no spinodol exists).
      enddo
      goto 633

 121  call ERRNUM (121,0,'LIQSPNDL',' ',T,Tc,0d0,ierr,herr)
      goto 500
 633  call ERRNUM (633,0,'LIQSPNDL',' ',T,0d0,0d0,ierr,herr)
      goto 500
 638  call ERRNUM (-638,0,'LIQSPNDL',' ',0d0,0d0,0d0,ierr,herr)

 400  if (icomp.ne.0) then             !Make check for pure fluids.
        if (D.lt.Dc) goto 633
        if (D.gt.2.2d0*Dc .and. T.gt.0.80d0*Tc) goto 633             !Density found far beyond limits, most likely an error in the EOS.
        if (D.lt.1.4d0*Dc .and. T.lt.0.94d0*Tc) goto 633
      endif

 500  iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      if (ierr.gt.0) D=Dc

      end                                            subroutine LIQSPNDL
c
c ======================================================================
c
      subroutine VAPSPNDL (T,z,D,ierr,herr)
c
c  Find the vapor spinodal density for a given temperature.  If no
c  spinodal exists, return the point of zero curvature.  This only
c  happens with a few of the older equations, these being D5, methanol,
c  and nitrogen.
c
c  Inputs:
c        T--Temperature [K]
c        z--Composition (array of mole fractions)
c
c  Outputs:
c        D--Density at vapor spinodal [mol/L]
c     ierr--Error flag:  0 - Successful
c                      121 - T>Tc
c                      633 - Failed to converge
c                     -638 - Spinodal not found, point of zero curvature returned
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-28-06 EWL, original version taken from code in SATT
c  09-15-11  MK, split subroutine SPNDL into liquid and vapor routines
c  08-11-16 EWL, return point of zero curvature if not spinodal found
c  03-11-17 EWL, rewrite code so that the main loop will find either the spinodal or point of zero curvature simultaneously

      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax),y(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::VAPSPNDL

      iErrNum=iErrNum+1
      call ISPURE (z,icomp)
      call CRITP (z,Tc,Pc,Dc,ierr,herr)
      ierr=0
      D=Dc
      if (T.gt.Tc+1d-8 .and. icomp.ne.0) goto 121
      if (T.gt.Tc-1d-5 .and. icomp.ne.0) goto 500          !For a pure fluid very near the critical point, just return Dc.
      call SATGUESS (2,1,0,0,z,T,P,D,h,s,Dv,y,ierr,herr)

      Ds=D
      Dmin=0d0
      Dmax=1000d0
      Dinc=-1d0    !Start at -1 in case first value is larger than spinodal and need to decrease D.
      tolr=1d-8
      do it=1,100
        call TOLRNC (tolr,it,10,15,20,25,30)     !Loosen tolerance if not converging.
        if (D.le.0.) goto 633
        call DPDDX (T,D,z,dPdD)
        call DPDD2 (T,D,z,d2PdD2)
        if (dPdD.gt.0. and. d2PdD2.lt.0. .and. D.gt.Dmin) Dmin=D     !Location where a valid root still exists.
        if (d2PdD2.lt.0.) then                   !Check for negative curvature.  If positive, the isotherm may not have a spinodal.
          Ds=D
          Dinc=-dPdD/d2PdD2
          if (D.gt.Dc/10d0 .or. D.gt.1d0) then
            if (ABS(Dinc).gt.D/10d0) Dinc=SIGN(D/10d0,Dinc)
          elseif (Dinc.gt.0) then
            DL10=-LOG10(D)
            DLinc=-LOG10(Dinc)
            if (DLinc.lt.DL10*2d0) DLinc=DL10/2d0
            Dinc2=10**(-DLinc)
            if (Dinc2.lt.Dinc) Dinc=Dinc2
          endif
          if (D+Dinc.lt.Dmin) Dinc=(D-Dmin)/2d0  !Keep more than the known valid state.
          if (D+Dinc.gt.Dmax) Dinc=(Dmax-D)/2d0  !Keep less than the point with positive curvature.
          D=ABS(D+Dinc)
        else
          if (D.lt.Dmax .and. it.ne.1) Dmax=D    !Set the location just found that has positive curvature (which is thus a state point beyond the desired root).
          Dinc=Dinc/2d0
          D=ABS(Ds+Dinc)                         !Go back towards the known good root.
        endif
        if (ABS(dPdD)  .lt.tolr) goto 400        !Converged on a spinodal.
        if (ABS(d2PdD2).lt.tolr) goto 638        !Converged on a change in curvature from negative to positive (thus no spinodol exists).
      enddo
      goto 633

 121  call ERRNUM (121,0,'VAPSPNDL',' ',T,Tc,0d0,ierr,herr)
      goto 500
 633  call ERRNUM (633,0,'VAPSPNDL',' ',T,0d0,0d0,ierr,herr)
      goto 500
 638  call ERRNUM (-638,0,'VAPSPNDL',' ',0d0,0d0,0d0,ierr,herr)

 400  if (icomp.ne.0) then             !Make check for pure fluids.
        if (D.gt.Dc) goto 633
        if (D.lt.0.2d0*Dc .and. T.gt.0.87d0*Tc) goto 633             !Density found far beyond limits, most likely an error in the EOS.
        if (D.gt.0.7d0*Dc .and. T.lt.0.96d0*Tc) goto 633
      endif

 500  iErrNum=iErrNum-1
      call ERRMSG (ierr,herr)
      if (ierr.gt.0) D=Dc
      end                                            subroutine VAPSPNDL
c
c ======================================================================
c






c  ---Single phase flash routines---
c
c ======================================================================
c
      subroutine PDFL1 (P,D,z,T,ierr,herr)
c
c  Iterate for single-phase temperature as a function of pressure,
c  density, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PDFL1

      call ABFL1 ('DP',D,P,z,0,0,0d0,0d0,T,pp,dd,ierr,herr)
      end                                               subroutine PDFL1
c
c ======================================================================
c
      subroutine PHFL1 (P,h,z,kph,T,D,ierr,herr)
c
c  Iterate for single-phase temperature and density as a function of
c  pressure, enthalpy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PHFL1

      call ABFL1 ('PH',P,h,z,kph,0,0d0,0d0,T,pp,D,ierr,herr)
      end                                               subroutine PHFL1
c
c ======================================================================
c
      subroutine PSFL1 (P,s,z,kph,T,D,ierr,herr)
c
c  Iterate for single-phase temperature and density as a function of
c  pressure, entropy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PSFL1

      call ABFL1 ('PS',P,s,z,kph,0,0d0,0d0,T,pp,D,ierr,herr)
      end                                               subroutine PSFL1
c
c ======================================================================
c
      subroutine PEFL1 (P,e,z,kph,T,D,ierr,herr)
c
c  Iterate for single-phase temperature and density as a function of
c  pressure, energy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PEFL1

      call ABFL1 ('PE',P,e,z,kph,0,0d0,0d0,T,pp,D,ierr,herr)
      end                                               subroutine PEFL1
c
c ======================================================================
c
      subroutine THFL1 (T,h,z,Dmin,Dmax,D,ierr,herr)
c
c  Iterate for single-phase density as a function of temperature,
c  enthalpy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THFL1

      call ABFL1 ('TH',T,h,z,0,0,Dmin,Dmax,tt,P,D,ierr,herr)
      end                                               subroutine THFL1
c
c ======================================================================
c
      subroutine TSFL1 (T,s,z,Dmin,Dmax,D,ierr,herr)
c
c  Iterate for single-phase density as a function of temperature,
c  entropy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TSFL1

      tt=300d0
      call ABFL1 ('TS',T,s,z,0,0,Dmin,Dmax,tt,P,D,ierr,herr)
      end                                               subroutine TSFL1
c
c ======================================================================
c
      subroutine TEFL1 (T,e,z,Dmin,Dmax,D,ierr,herr)
c
c  Iterate for single-phase density as a function of temperature,
c  energy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TEFL1

      tt=300d0
      call ABFL1 ('TE',T,e,z,0,0,Dmin,Dmax,tt,P,D,ierr,herr)
      end                                               subroutine TEFL1
c
c ======================================================================
c
      subroutine DHFL1 (D,h,z,T,ierr,herr)
c
c  Iterate for single-phase temperature as a function of density,
c  enthalpy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DHFL1

      call ABFL1 ('DH',D,h,z,0,0,0d0,0d0,T,pp,dd,ierr,herr)
      end                                               subroutine DHFL1
c
c ======================================================================
c
      subroutine DSFL1 (D,s,z,T,ierr,herr)
c
c  Iterate for single-phase temperature as a function of density,
c  entropy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DSFL1

      call ABFL1 ('DS',D,s,z,0,0,0d0,0d0,T,pp,dd,ierr,herr)
      end                                               subroutine DSFL1
c
c ======================================================================
c
      subroutine DEFL1 (D,e,z,T,ierr,herr)
c
c  Iterate for single-phase temperature as a function of density,
c  energy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DEFL1

      call ABFL1 ('DE',D,e,z,0,0,0d0,0d0,T,pp,dd,ierr,herr)
      end                                               subroutine DEFL1
c
c ======================================================================
c
      subroutine HSFL1 (h,s,z,Dmin,Dmax,T,D,ierr,herr)
c
c  Iterate for single-phase temperature and density as a function of
c  enthalpy, entropy, and composition.
c  (See subroutine ABFL1 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::HSFL1

      if (ieflg.ne.1) then
        call ABFL1 ('SH',s,h,z,0,0,Dmin,Dmax,T,P,D,ierr,herr)
      else
        call ABFL1 ('SE',s,h,z,0,0,Dmin,Dmax,T,P,D,ierr,herr)
      endif
      end                                               subroutine HSFL1
c
c ======================================================================
c






c  ---Two-phase flash routines---
c
c ======================================================================
c
      subroutine PDFL2 (P,D,z,ksat,Tbub,Tdew,Dlbub,Dvdew,ybub,xdew,
     &                  T,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given pressure, bulk density, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PDFL2

      call ABFL2 ('DP',D,P,z,1,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,pp,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine PDFL2
c
c ======================================================================
c
      subroutine PHFL2 (P,h,z,ksat,Tbub,Tdew,Dlbub,Dvdew,ybub,xdew,
     &                  T,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given pressure, bulk enthalpy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PHFL2

      call ABFL2 ('PH',P,h,z,1,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,pp,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine PHFL2
c
c ======================================================================
c
      subroutine PSFL2 (P,s,z,ksat,Tbub,Tdew,Dlbub,Dvdew,ybub,xdew,
     &                  T,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given pressure, bulk entropy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PSFL2

      call ABFL2 ('PS',P,s,z,1,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,pp,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine PSFL2
c
c ======================================================================
c
      subroutine PEFL2 (P,e,z,ksat,Tbub,Tdew,Dlbub,Dvdew,ybub,xdew,
     &                  T,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given pressure, bulk energy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PEFL2

      call ABFL2 ('PE',P,e,z,1,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,pp,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine PEFL2
c
c ======================================================================
c
      subroutine PQFL2 (P,q,z,kq,ksat,Tbub,Tdew,Dlbub,Dvdew,ybub,xdew,
     &                  T,Dl,Dv,x,y,ierr,herr)
c
c  Flash calculation given pressure, quality, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PQFL2

      call ABFL2 ('PQ',P,q,z,kq,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,pp,Dl,Dv,x,y,qq,ierr,herr)
      end                                               subroutine PQFL2
c
c ======================================================================
c
      subroutine TQFL2 (T,q,z,kq,ksat,pbub,pdew,Dlbub,Dvdew,ybub,xdew,
     &                  P,Dl,Dv,x,y,ierr,herr)
c
c  Flash calculation given temperature, quality, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TQFL2

      call ABFL2 ('TQ',T,q,z,kq,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,tt,P,Dl,Dv,x,y,qq,ierr,herr)
      end                                               subroutine TQFL2
c
c ======================================================================
c
      subroutine THFL2 (T,h,z,ksat,pbub,pdew,Dlbub,Dvdew,ybub,xdew,
     &                  P,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given temperature, bulk enthalpy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THFL2

      call ABFL2 ('TH',T,h,z,1,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,tt,P,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine THFL2
c
c ======================================================================
c
      subroutine TSFL2 (T,s,z,ksat,pbub,pdew,Dlbub,Dvdew,ybub,xdew,
     &                  P,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given temperature, bulk entropy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TSFL2

      call ABFL2 ('TS',T,s,z,1,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,tt,P,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine TSFL2
c
c ======================================================================
c
      subroutine TEFL2 (T,e,z,ksat,pbub,pdew,Dlbub,Dvdew,ybub,xdew,
     &                  P,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given temperature, bulk energy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TEFL2

      call ABFL2 ('TE',T,e,z,1,ksat,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,tt,P,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine TEFL2
c
c ======================================================================
c
      subroutine DHFL2 (D,h,z,T,P,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given bulk density, enthalpy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DHFL2

      call ABFL2 ('DH',D,h,z,1,0,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine DHFL2
c
c ======================================================================
c
      subroutine DSFL2 (D,s,z,T,P,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given bulk density, entropy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DSFL2

      call ABFL2 ('DS',D,s,z,1,0,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine DSFL2
c
c ======================================================================
c
      subroutine DEFL2 (D,e,z,T,P,Dl,Dv,x,y,q,ierr,herr)
c
c  Flash calculation given bulk density, energy, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DEFL2

      call ABFL2 ('DE',D,e,z,1,0,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,q,ierr,herr)
      end                                               subroutine DEFL2
c
c ======================================================================
c
      subroutine DQFL2 (D,q,z,kq,T,P,Dl,Dv,x,y,ierr,herr)
c
c  Flash calculation given bulk density, quality, and composition.
c  (See subroutine ABFL2 for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax),xdew(ncmax),ybub(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DQFL2

      call ABFL2 ('DQ',D,q,z,kq,0,Tbub,Tdew,pbub,pdew,
     &            Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,qq,ierr,herr)

      end                                               subroutine DQFL2
c
c ======================================================================
c






c  ---Unknown phase flash routines---
c
c ======================================================================
c
      subroutine TDFLSH (T,D,z,P,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given temperature, bulk density, and bulk composition.
c  This routine accepts both single-phase and two-phase states as inputs;
c  for single-phase calculations, the subroutine THERM is much faster.
c  (See subroutines ABFLSH or TPDFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TDFLSH

      call TPDFLSH (T,P,D,z,1,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      end                                              subroutine TDFLSH
c
c ======================================================================
c
      subroutine PDFLSH (P,D,z,T,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given density, pressure, and bulk composition.
c  This routine accepts both single-phase and two-phase states as inputs;
c  for single-phase calculations, the subroutine PDFL1 is faster.
c  (See subroutines ABFLSH or TPDFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PDFLSH

      call TPDFLSH (T,P,D,z,3,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
      end                                              subroutine PDFLSH
c
c ======================================================================
c
      subroutine PHFLSH (P,h,z,T,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given pressure, bulk enthalpy, and bulk composition.
c  (See subroutines ABFLSH or PBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PHFLSH

      call PBFLSH (P,h,z,'PH',T,D,Dl,Dv,x,y,q,e,hh,s,Cv,Cp,w,ierr,herr)
      end                                              subroutine PHFLSH
c
c ======================================================================
c
      subroutine PSFLSH (P,s,z,T,D,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given pressure, bulk entropy, and bulk composition.
c  (See subroutines ABFLSH or PBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PSFLSH

      call PBFLSH (P,s,z,'PS',T,D,Dl,Dv,x,y,q,e,h,ss,Cv,Cp,w,ierr,herr)
      end                                              subroutine PSFLSH
c
c ======================================================================
c
      subroutine PEFLSH (P,e,z,T,D,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given pressure, bulk energy, and bulk composition.
c  (See subroutines ABFLSH or PBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PEFLSH

      call PBFLSH (P,e,z,'PE',T,D,Dl,Dv,x,y,q,ee,h,s,Cv,Cp,w,ierr,herr)
      end                                              subroutine PEFLSH
c
c ======================================================================
c
      subroutine THFLSH (T,h,z,kr,P,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given temperature, bulk enthalpy, and bulk composition.
c  Often in the liquid, two solutions exist, one of them in the two phase.
c  If this is the case, call THFLSH with kr=2 to get the single-phase state.
c  (See subroutines ABFLSH or TBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::THFLSH

      call TBFLSH (T,h,z,kr,'TH',P,D,Dl,Dv,x,y,q,e,hh,s,Cv,Cp,w,
     &             ierr,herr)
      end                                              subroutine THFLSH
c
c ======================================================================
c
      subroutine TSFLSH (T,s,z,kr,P,D,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given temperature, bulk entropy, and bulk composition.
c  (See subroutines ABFLSH or TBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TSFLSH

      call TBFLSH (T,s,z,kr,'TS',P,D,Dl,Dv,x,y,q,e,h,ss,Cv,Cp,w,
     &             ierr,herr)
      end                                              subroutine TSFLSH
c
c ======================================================================
c
      subroutine TEFLSH (T,e,z,kr,P,D,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given temperature, bulk energy, and bulk composition.
c  (See subroutines ABFLSH or TBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TEFLSH

      call TBFLSH (T,e,z,kr,'TE',P,D,Dl,Dv,x,y,q,ee,h,s,Cv,Cp,w,
     &             ierr,herr)
      end                                              subroutine TEFLSH
c
c ======================================================================
c
      subroutine DHFLSH (D,h,z,T,P,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given density, enthalpy, and bulk composition.
c  (See subroutines ABFLSH or DBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DHFLSH

      call DBFLSH (D,h,z,'DH',T,P,Dl,Dv,x,y,q,e,hh,s,Cv,Cp,w,ierr,herr)
      end                                              subroutine DHFLSH
c
c ======================================================================
c
      subroutine DSFLSH (D,s,z,T,P,Dl,Dv,x,y,q,e,h,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given density, entropy, and bulk composition.
c  (See subroutines ABFLSH or DBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DSFLSH

      call DBFLSH (D,s,z,'DS',T,P,Dl,Dv,x,y,q,e,h,ss,Cv,Cp,w,ierr,herr)
      end                                              subroutine DSFLSH
c
c ======================================================================
c
      subroutine DEFLSH (D,e,z,T,P,Dl,Dv,x,y,q,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given density, energy, and bulk composition.
c  (See subroutines ABFLSH or DBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::DEFLSH

      call DBFLSH (D,e,z,'DE',T,P,Dl,Dv,x,y,q,ee,h,s,Cv,Cp,w,ierr,herr)
      end                                              subroutine DEFLSH
c
c ======================================================================
c
      subroutine ESFLSH (e,s,z,T,P,D,Dl,Dv,x,y,q,h,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given bulk energy, entropy, and composition.
c  (See subroutines ABFLSH or DBFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ESFLSH

      ieflg=1
      call HSFLSH (e,s,z,T,P,D,Dl,Dv,x,y,q,h,Cv,Cp,w,ierr,herr)
      ieflg=0
      end                                              subroutine ESFLSH
c
c ======================================================================
c
      subroutine PQFLSH (P,q,z,kq,T,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given pressure, quality, and bulk composition.
c  This routine accepts saturation or two-phase states as inputs.
c  (See subroutines ABFLSH or AQFLSH for the description of all variables.)

      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::PQFLSH

      call AQFLSH (T,P,q,z,2,kq,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,qml,ierr,herr)
      end                                              subroutine PQFLSH
c
c ======================================================================
c
      subroutine TQFLSH (T,q,z,kq,P,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,ierr,herr)
c
c  Flash calculation given temperature, quality, and bulk composition.
c  This routine accepts saturation or two-phase states as inputs.
c  (See subroutines ABFLSH or AQFLSH for the description of all variables.)
c
      include 'COMMONS.INC'
      character*255 herr
      dimension x(ncmax),y(ncmax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::TQFLSH


      call AQFLSH (T,P,q,z,1,kq,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,qml,ierr,herr)

      end                                              subroutine TQFLSH
c
c ======================================================================
c
      subroutine ABFL2 (ab,a,b,z,kq,ksat,Tbub,Tdew,Pbub,Pdew,
     &                  Dlbub,Dvdew,ybub,xdew,T,P,Dl,Dv,x,y,q,ierr,herr)
c
c  General flash calculation given two inputs and composition.  Valid
c  properties for the first input are temperature, pressure, and density.
c  Valid properties for the second are pressure, density, energy, enthalpy,
c  entropy, or quality.  The character string ab specifies the inputs.
c
c  This routine accepts only two-phase states as inputs; it is intended
c  primarily for use by the general flash routines such as THFLSH or
c  TSFLSH.  It may be called independently if the state is known to be
c  two-phase.  But beware - this routine does not check limits, and it
c  will be significantly faster than TSFLSH, etc., when the bubble
c  and dew point limits can be provided (ksat=1 option).
c
c  This routine calls TPFL2 within a secant-method iteration to find
c  a solution.  Initial guesses are based on the liquid density
c  at the bubble point and the vapor density at the dew point.
c
c  Inputs:
c       ab--Character*2 string defining the inputs, e.g., 'TD' or 'PQ'
c        a--First property (either temperature, pressure, or density)
c        b--Second property (pressure, density, energy, enthalpy,
c           entropy, or quality)
c        z--Overall composition (array of mole fractions)
c       kq--Flag specifying units for input quality when b=quality:
c           1 - Quality on molar basis (moles vapor/total moles)
c           2 - Quality on mass basis (mass vapor/total mass)
c     ksat--Flag for bubble and dew point limits:
c           0 - Dew and bubble point limits computed here
c           1 - Must provide values for the following:
c               For pressure and density inputs
c                  Tbub--Bubble point temperature [K] at (P and x=z)
c                  Tdew--Dew point temperature [K] at (P and y=z)
c               For temperature inputs
c                  Pbub--Bubble point pressure [kPa] at (T and x=z)
c                  Pdew--Dew point pressure [kPa] at (T and y=z)
c               For either case
c                 Dlbub--Liquid density [mol/L] at bubble point
c                 Dvdew--Vapor density [mol/L] at dew point
c                  ybub--Vapor composition (array of mole fractions) at bubble point
c                  xdew--Liquid composition (array of mole fractions) at dew point
c
c  Outputs:
c        T--Temperature [K] (if not an input)
c        P--Pressure [kPa] (if not an input)
c       Dl--Liquid density [mol/L] at bubble point
c       Dv--Vapor density [mol/L] at dew point
c        x--Liquid composition (array of mole fractions)
c        y--Vapor composition (array of mole fractions)
c        q--Vapor quality, the definitions of the values for q are given in the ABFLSH routine.
c     ierr--Error flag:  0 - Successful
c                      223 - Bubble point calculation did not converge
c                      224 - Dew point calculation did not converge
c                      226 - 2-phase iteration did not converge
c     herr--Error string (character*255)
c
c  rewritten by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  12-12-00 EWL, original version, rewritten by combining code of McLinden
c                from several different versions of FL2 routines to make
c                one unified routine
c  12-12-16 EWL, rewrite and optimize all code

      include 'COMMONS.INC'
      character at*1,bt*1,ab*2,ab2*2,herr*255,herr1*255
      dimension x(ncmax),y(ncmax),z(ncmax),v(20),f(20)
      dimension x2(ncmax),y2(ncmax),xdew(ncmax),ybub(ncmax)
      dimension xs(ncmax),ys(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ABFL2

c  Check for inputs of only T-P or T-D and call appropriate routine.
      call GETINPUTS (ab,ab2,at,bt,iat,ibt,a,b,aa,bb,T,D,P,ierr,herr)
      if (ierr.gt.0) RETURN
      if (ibt.eq.iLetterQ) q=b
      if (ab2.eq.'TP') then                      !Temperature-pressure inputs.
        call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
        RETURN
      elseif (ab2.eq.'TD') then                  !Temperature-density inputs.
        call TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)
        RETURN
      endif

      iErrNum=iErrNum+1
      ajT=T
      ajP=P
      iFailFlg=0
      v3=0
      Tliq=300d0
      Tvap=300d0
      v0=1.01d0
      call SETCOMP (icomp,z,x,y)                           !Set output liquid and vapor compositions to input values.
      call SATMAX  (icomp,z,Tc,Pc,Dc,ierr,herr)            !Get pure fluid critical point or mixture maximum T and P (but labeled here simply as Tc and Pc).
      call LIMITS  ('EOS',z,Tmin,Tmax,Dm,Pm)
      call ROOTSEARCH (0,0,0,v,f,v3,0,ib,lneg,lpos)        !Initialize root finding variables.

c  Calculate bubble and dew points to establish bounds on iteration if ksat<>1.
      if (ksat.ne.1) then
        if (iat.eq.iLetterT) then
          call SATT (T,z,1,Pbub,Dlbub,Dvdew,xdew,ybub,ierr,herr)
          Pdew=Pbub
          if(ierr.gt.0) goto 223
          if(icomp.eq.0)call SATT(T,z,2,Pdew,dd,Dvdew,xdew,y2,ierr,herr)
          if(ierr.gt.0) goto 224
        elseif (iat.eq.iLetterP) then
          call SATP (P,z,1,Tbub,Dlbub,Dvdew,xdew,ybub,ierr,herr)
          if (ierr.ge.100 .and. ixWat.ge.1) then
            iFailFlg=1
            call SATEST (3,Tbub,P,z,xdew,ybub,ierr,herr)   !If water is in the mixture, it is most likely a Type III
            call TPRHO (Tbub,P,xdew,1,0,Dlbub,ierr2,herr)  ! mixture, so use SATEST to get an estimate of the liquid side.
          endif
          Tdew=Tbub
          if(ierr.gt.100) goto 223
          if(icomp.eq.0)call SATP(P,z,2,Tdew,dd,Dvdew,xdew,y2,ierr,herr)
          if(ierr.gt.100) goto 224
        else       !Inputs of density.
          call SATD (D,z,0,kr,Tbub,P,Dlbub,Dvdew,xdew,ybub,ierr,herr)
          if (Dlbub.lt.Dvdew) call SWAPV (Dlbub,Dvdew)
          if (ierr.ge.100 .or. Tbub.eq.0.) then
            Tbub=Tc
          elseif (ABS(D-Dlbub).lt.0.01d0) then
            ierr=999
            if (bb.gt.P .and. ibt.eq.iLetterP) RETURN
          endif
          Tdew=Tbub
        endif
      endif
      xs(1:nc)=xdew(1:nc)
      ys(1:nc)=ybub(1:nc)
      if (lEq0(DvDew)) DvDew=Dc/2d0
      if (lEq0(Dlbub)) Dlbub=Dc*2d0

c  Set up variables for the first input.
      ierrq=0
      Pvap=0d0
      Tvap=0d0
      Dl=Dlbub
      Dv=Dvdew
      q2=0.5d0
      if (Dl.le.0) Dl=Dc*2d0
      if (Dv.le.0) Dv=Dc*0.2d0
      if (iat.eq.iLetterT) then
        P=Pbub
        Tbub=T
        Tdew=T
        z1=Pbub
        z2=Pdew
        aratio=XDIV(z1,z2)-1d0         !Base convergence tolerance on abub/adew ratio; otherwise, near-azeotropes may not converge (yet initial guesses may be very good).
      elseif (iat.eq.iLetterP .or. ab2.eq.'DP') then
        T=Tbub*0.5d0+Tdew*0.5d0
        z1=Tbub
        z2=Tdew
        aratio=z2/z1-1d0
      else         !Density inputs.
        Tdew=Tbub
        T=Tbub
        x(1:nc)=xdew(1:nc)
        y(1:nc)=ybub(1:nc)
        aratio=Dl
        v(6)=Tmin
        v(7)=T*1.01d0        !Upper temperature limit is the saturation state.
        if (v(7).gt.Tc .and. icomp.ne.0) v(7)=Tc
        z1=Tbub
        z2=Tdew
        if (ibt.eq.iLetterQ .and.
     &     (ABS(q).lt.1d-12 .or. ABS(q-1d0).lt.1d-9)) then
          if (q.gt.0.99 .and. aa.ge.Dc) goto 223           !No solution if q=1 and D<Dc
          if (q.lt.0.01 .and. aa.le.Dc) goto 224           !No solution if q=0 and D>Dc
          goto 500
        endif
      endif
      if (ABS(aratio).lt.1d-8) aratio=1d-8
      tolr=5d-8
      tolr=MAX(tolr/1d2,MIN(tolr*1d2,0.02d0*tolr/aratio))

c  Set up variables for the second input.
      hvap=1d0
      q=0.5d0
      if (ibt.eq.iLetterP) then
        f1=1d0-T/Tbub        !Calculate the values of the objective functions at the saturation states.
        f2=1d0-T/Tdew
        if (ABS(Tdew-Tbub).gt.1.) q=(T-Tbub)/(Tdew-Tbub)             !Approximate value for q.
      elseif (ibt.eq.iLetterD) then
        f1=1d0-bb/Dl         !Calculate the values of the objective functions at the saturation states.
        f2=1d0-bb/Dv
        if (ABS(Dl-Dv).gt.0.001d0) q=(1d0/bb-1d0/Dl)/(1d0/Dv-1d0/Dl)
      elseif (ibt.eq.iLetterH .or. ibt.eq.iLetterS .or.
     &        ibt.eq.iLetterE) then
        call HESCALC (ibt,Tbub,Dl,z,bl)
        call HESCALC (ibt,Tdew,Dv,z,bv)
        hvap=bv-bl
        if (ABS(hvap).gt.1d7) goto 226
        if (ABS(hvap).lt.1.) hvap=1d0
        f1=(bb-bl)/hvap                !Calculate the values of the objective functions at the saturation sates.
        f2=(bb-bv)/hvap                !Normalize by a pseudo enthalpy of vaporization.
        if (ABS(bv-bl).gt.0.001d0) q=(bb-bl)/(bv-bl)                 !Approximate value for q.
        if (ABS(f2).lt.tolr) goto 500            !Inputs are near vapor solution, so exit.
        T=Tbub
        if (ABS(f1).lt.tolr) goto 500            !Inputs are near liquid solution, so exit.
      else         !Inputs of Q
        q=bb
        f1=q
        f2=q-1d0
        if (kq.eq.2) then    !Input is on mass basis.
          call QMASS (q,xdew,ybub,qkg,x2,y2,wliq,wvap,ierr1,herr)
          f1=qkg
          f2=qkg-1d0
        endif
      endif
      if (q.le.0. or. q.ge.1.) then
        if (q.gt.1.) q=0.95d0+q/100d0
        if (q.gt.1.) q=0.99d0
        if (q.lt.0.) q=0.05d0+q/100d0
        if (q.lt.0.) q=0.01d0
      endif

c  Store the saturation states in the root searching variables.
      if (iat.eq.iLetterD) then
        v(2)=Tbub*0.95d0
        if (v(2).lt.Tmin) v(2)=(Tmin+Tbub)/2d0
        if (icomp.ne.0 .and. ibt.ne.iLetterQ) then
          v(2)=z1            !Enter the values from the known saturation state,
          f(2)=f1            ! which will also set up one of the bounds.
          if (D.lt.Dc) f(2)=f2
          call ROOTSEARCH (1,0,0,v,f,v3,0,ib,lneg,lpos)    !Store values.
        endif
      else
        if (icomp.ne.0) then           !For a pure fluid (or pseudo-pure fluid), the routine is finished (except for density inputs).
          if (iAnc(icomp).ne.0) then             !Check for pseudo-pure fluid.
            if (iat.eq.iLetterP) call PTANC(T,P,q,bb,ibt,Dl,Dv)
            if (iat.eq.iLetterT) P=q*Pdew+(1d0-q)*Pbub
          endif
          goto 500
        endif
        if (ABS(f1).lt.tolr .or. ABS(f2).lt.tolr) goto 350           !Check if initial guess is near solution for mixtures.
        v(2)=z1    !Enter the values from the known saturation state,
        f(2)=f1    ! which will also set up one of the bounds.
        if (iFailFlg.eq.0) call ROOTSEARCH (1,0,0,v,f,v3,0,ib,lneg,lpos)       !Store values.
        v(2)=z2    !For inputs other than density, the other saturation state is known, call ROOTSEARCH to store its value.
        f(2)=f2
        call ROOTSEARCH (1,0,0,v,f,v3,0,ib,lneg,lpos)
        v(2)=(v(3)+v(2))/2d0
        if (iat.eq.iLetterP) then
          T=Tbub*(1d0-q)+q*Tdew
          v(2)=T
          if (ABS(Tbub-Tdew).lt.0.001d0) goto 400          !For very near azeotropes or when q is nearly 0 or 1, ABFL2 may not converge, so return a simple solution.
        endif
      endif
      nfail=-1
      ajT=T
      ajP=P
      q2=q
      T2=T
      Dly=0d0
      Dvy=0d0
      i2=0
      q3=q

c---------------Start of main loop---------------
      do it=1,40
      call TOLRNC (tolr,it,10,15,22,30,35)       !Loosen tolerance if not converging.
      if (iat.eq.iLetterT) P=v(2)
      if (iat.eq.iLetterP .or. iat.eq.iLetterD) T=v(2)
      if (iat.ne.iLetterD .and. (nfail.gt.0 .or. it.eq.1)) then
        if (ierrq.gt.0) then
          qinc=0.002d0                                     !If TPFL2 or TDFL2 failed, modify q and try again.
          if (q2.lt.0.1d0) qinc=0.02d0
          if (INT(ierrq/2)*2.eq.ierrq) qinc=-qinc          !Bounce back and forth to try to find a solution.
          q=q2*(1d0+qinc*DBLE(ierrq)**3)
          if (q.le.0. .or. q.ge.1.) q=q2*(1d0-qinc*DBLE(ierrq)**2)
        endif
        call SAT0EST (T,P,z,x,y,ierr1,herr1)
        if (ierr1.gt.0) call GETXYZQ (1,1,q,z,xdew,ybub,x,y)         !Get x and y estimates from the current values of q, xdew, and ybub.
        if (iat.eq.iLetterT) then
                         call SATT (T,x,1,Pliq,Dlx,Dvx,x2,ybub,i1,herr)        !Find the liquid saturation state at x determined in the line above and T or P.
          if (i1.lt.100) call SATT (T,y,2,Pvap,Dly,Dvy,xdew,y2,i2,herr)        !If the above call to SATT did not fail, calculate the properties for the opposite saturation state.
          P=(1d0-q)*Pliq+q*Pvap
          v(2)=P
        else
          call SATP (P,x,1,Tliq,Dlx,Dvx,x2,ybub,i1,herr)
          call SATP (P,y,2,Tvap,Dly,Dvy,xdew,y2,i2,herr)
          if (i1.ge.100) call SATEST (3,Tliq,P,x,x2,ybub,ierr3,herr)
          if (i2.ge.100) call SATEST (4,Tvap,P,y,xdew,y2,ierr4,herr)
          T=(1d0-q)*Tliq+q*Tvap
          if (ierrq.gt.0 .and. i1.ge.100) then
            Tinc=-0.005d0                                  !If the saturation routines failed, modify T and try again.
            if (INT(ierrq/3)*3.eq.ierrq) Tinc=-Tinc        !Bounce back and forth to try to find a solution.
            T=T2*(1d0+Tinc*DBLE(ierrq))
          endif
          v(2)=T
        endif
        if (Dlx.le.1.d-20 .or. Dlx.gt.100. .or. i1.ge.100) Dlx=Dlbub
        if (Dly.le.1.d-20 .or. Dly.gt.100. .or. i2.ge.100) Dly=Dlbub
        if (Dvx.le.1.d-20 .or. Dvx.gt.100. .or. i1.ge.100) Dvx=Dvdew
        if (Dvy.le.1.d-20 .or. Dvy.gt.100. .or. i2.ge.100) Dvy=Dvdew
        if (ibt.eq.iLetterD .and. bb.lt.Dv .and. nfail.lt.5 .and.
     &    (q.lt.0.05d0 .or. q.gt.0.95d0)) then
          if (q.lt.0.01d0) q=q*20d0    !Initial q is not very good (usually very small).
          if (q.gt.0.95d0) q=1d0-(1d0-q)*5d0
          if (q.gt.1d0) q=0.95d0
          if (q.lt.0.05d0) q=q/2d0
          nfail=nfail+1
          goto 150
        endif
        nfail=0
        Dl=1d0/((1d0-q)/Dlx+q/Dly)     !Initial guesses for use in 2-phase routines, based on volumes.
        Dv=1d0/((1d0-q)/Dvx+q/Dvy)
      endif

c  Find densities and compositions of each phase.
      if (iat.eq.iLetterD) then
        if (ABS(f(2)).lt.0.1d0 .and. it.gt.1) T=-T         !If close, use initial values as estimates.
        Tsave=T
        call TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)
        if (Tsave.lt.0 .and. ierr.gt.100)
     &      call TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)     !Call again if initial values did not work.
        if (ierr.gt.100 .and. ABS(f(2)).lt.1.d-4) then
          if (lpos.and.lneg) then
            ib=0
            do ix=1,10
              call ROOTSEARCH(6,0,0,v,f,T,it,ib,lneg,lpos)           !Use option #6 to bounce around inside the area bounded by the current upper and lower bounds when really close to the solution, but TDFL2 failed.
              T=-T
              call TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)
              if (ierr.lt.100) EXIT
            enddo
          endif
        endif
      else
        call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
      endif
      if (q.gt.   -1.d-6 .and. q.lt.0.) q=ABS(q)
      if (q.lt.1d0+1.d-6 .and. q.gt.1.) q=1d0-ABS(1d0-q)
      if (q.eq.0 .or. q.eq.1) then
        call SAT0EST (T,P,z,x,y,ierr1,herr1)
        if (ierr1.gt.0) call GETXYZQ (1,1,q,z,xs,ys,x,y)
        if (lNE0(q)) then
          if (iat.eq.iLetterD)call TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)
          if (iat.ne.iLetterD)call TPFL2 (T,P,z,  Dl,Dv,x,y,q,ierr,herr)
        endif
      endif
      if (ierr.ge.100) then
        ierrq=ierrq+1
        qinc=0.0002d0                                      !If TPFL2 or TDFL2 failed, modify q and try again.
        if (q2.lt.0.1d0) qinc=0.002d0
        if (INT(ierrq/2)*2.eq.ierrq) qinc=-qinc            !Bounce back and forth to try to find a solution.
        q=q2*(1d0+qinc*DBLE(ierrq)**3)
        if (q.le.0. .or. q.ge.1.) q=ABS(q2*(1d0-qinc*DBLE(ierrq)**2))
        do j=1,10
          if (q.lt.1) EXIT
          q=q/2d0
        enddo
        call GETXYZQ (1,1,q,z,xs,ys,x,y)
        T=T2*(1.d0-qinc*DBLE(ierrq))
        if (ABS(f(2)).lt.1.d-4 .and. q.gt.0.9999 .and. q.lt.1.d0 .and.
     &      iat.ne.iLetterT) then
          T=T*(1.d0-(1.d0-q)*100d0)    !Drop T just slightly based on q with a few less 9's.
          call GETXYZQ (1,1,q,z,xs,ys,x,y)
          if (iat.eq.iLetterD)call TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)
          if (iat.ne.iLetterD)call TPFL2 (T,P,z,  Dl,Dv,x,y,q,ierr,herr)
        endif
        v(2)=T
        if (ierrq.lt.3 .and. ierr.gt.0)
     &      call ROOTSEARCH(11,0,0,v,f,v0,it,ib,lneg,lpos)       !Increase bounds if failed.
      endif
      if (ABS(XDIV(v(4)-v(5),v(2))).lt.1.d-8.and.ABS(f(2)).gt.1.d-5)then
        call ROOTSEARCH(11,0,0,v,f,v0,it,ib,lneg,lpos)           !Increase bounds if stuck.
      endif

      if (ierr.ge.100 .and. iat.ne.iLetterD) then          !Check for errors, except for out of bound errors.
        if (iat.eq.iLetterP) T=T*1.0001d0
        nfail=nfail+1
        q=q2
      elseif (ierr.lt.100) then
        T2=T
        q2=q
        xs(1:nc)=x(1:nc)
        ys(1:nc)=y(1:nc)
        ierrq=0
        if (ibt.eq.iLetterP) then
          f(2)=1d0-P/bb
        elseif (ibt.eq.iLetterD) then
          f(2)=1d0-bb*((1d0-q)/Dl+q/Dv)                    !Objective function  (Input density times current bulk volume.)
        elseif (ibt.eq.iLetterQ) then
          qkg=q
          if (kq.eq.2) call QMASS (q,x,y,qkg,x2,y2,wliq,wvap,ierr1,herr)       !Input is on a mass basis.
          f(2)=bb-qkg
        else       !Inputs of E, H, or S
          call HESCALC (ibt,T,Dl,x,bl)
          call HESCALC (ibt,T,Dv,y,bv)
          f(2)=(bb-(1d0-q)*bl-q*bv)/hvap         !Objective function.
          if (ABS(f(2)-f(1)).lt.1.d-10 .and. v(2).lt.1.d-10) then
            nfail=nfail+1
            lneg=.false.
            lpos=.false.
            f(4)=-1d20
            f(5)= 1d20
            if (iat.eq.iLetterT) v(2)=ajP
            if (iat.eq.iLetterP) v(3)=ajT
            goto 150
          endif
        endif

c    Iteration has converged.
        if (ABS(f(2)).lt.tolr .and. it.gt.1) goto 500

c    Update Reguli-Falsi values and get next guess with Secant or other methods.
        call ROOTSEARCH (1234,0,0,v,f,v3,it,ib,lneg,lpos)
c       call ROOTSEARCH (8,0,0,v,f,v3,it,ib,lneg,lpos)
        if (it.eq.1 .and. iat.eq.iLetterD) v3=v(2)*0.95d0
        v(2)=v3
      endif
 150  enddo
      goto 226               !Iteration failed to converged.


c   ---------Error Messages---------
 223  call ERRNUM (223,0,ab2//'FL2',herr,0d0,0d0,0d0,ierr,herr)
      goto 500
 224  call ERRNUM (224,0,ab2//'FL2',herr,0d0,0d0,0d0,ierr,herr)
      goto 500
 226  x(1:nc)=z(1:nc)        !Return bulk composition.
      y(1:nc)=z(1:nc)
      Dl=Dlbub
      Dv=Dvdew
      q=q2
      if (iat.eq.iLetterT) P=ajP
      if (iat.eq.iLetterP) T=ajT
      if (icomp.eq.0 .and. (q.le.0.005d0 .or. q.ge.0.995d0)) goto 400          !Initial guesses are very close to saturation.
      call ERRNUM (226,0,ab2//'FL2',' ',0d0,0d0,0d0,ierr,herr)
      goto 500

 350  if (ABS(f1).lt.tolr) then        !Initial guess near solution, return best guesses.
        if (iat.eq.iLetterT) P=z1
        if (iat.eq.iLetterP) T=z1
        x(1:nc)=z(1:nc)
        y(1:nc)=ybub(1:nc)
      elseif (ABS(f2).lt.tolr) then
        if (iat.eq.iLetterT) P=z2
        if (iat.eq.iLetterP) T=z2
        x(1:nc)=xdew(1:nc)
        y(1:nc)=z(1:nc)
      endif
      Dl=Dlbub
      Dv=Dvdew

c  Finalize routine.
 400  ierr=0
 500  if (q.gt.1. .and. q.lt.1d0+1d-6) q=1d0               !Catch very small differences from 0 or 1.
      if (q.lt.1.d-100 .and. q.gt.-1d-6) q=0d0
      if (ierr.ge.100) q=999d0         !Quality undefined
      if (ibt.eq.iLetterQ) q=bb                  !Reset q if it was an input.
      iErrNum=iErrNum-1
      call GETQQ (T,P,D,Tc,Pc,Dc,q)
      call GETPROPS (1,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)   !Calculate all other properties.
      call GETQQ (T,P,D,Tc,Pc,Dc,q)           !Recall this to get q since P may not have been known above and is used here in just a few cases.
      call ERRMSG (ierr,herr)
      RETURN

      end                                               subroutine ABFL2
c
c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                  End file FLSH_SUB.FOR
c ======================================================================
