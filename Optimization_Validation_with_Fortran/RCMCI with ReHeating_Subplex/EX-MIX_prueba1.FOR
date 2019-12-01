c  Example program showing how to setup a mixture using the information
c  contained in a .mix file (located in the ...\Refprop\Mixtures directory).

      program EXAMPLE
      implicit double precision (a-h,o-z)
      implicit integer (i-k,m,n)
      parameter (ncmax=20)        !max number of components in mixture
      dimension x(ncmax),xliq(ncmax),xvap(ncmax)
      character hrf*3
      character*255 hf(ncmax),hfmix,hflnme,herr

c...If the fluid files are located in a directory that the code cannot
c.....find, make a call to SETPATH
      call SETPATH('\mixtures')

      hflnme='CO2ETHANE1.MIX'
      hfmix='hmx.bnc'
      hrf='DEF'

c...The SETMIX subroutine returns the number of fluids [ncc], fluid
c...names [hf()], and the composition of the mixture [x()].
      call SETMIX (hflnme,hfmix,hrf,ncc,hf,x,ierr,herr)
      if (ierr.ne.0) write (*,*) herr
      if (ierr.gt.0) stop

      t=350
      p=7500

c...If phase is not known, call TPFLSH
c...Calls to TPFLSH are much slower than TPRHO since SATT must be called first.
c.....(If two phase, quality is returned as q)
      call TPFLSH (t,p,x,d,dl,dv,xliq,xvap,q,e,h,s,cv,cp,w,ierr,herr)
      print *, '  Temperature:', t
      print *, '  Pressure:', p
      print *, '  Density:', d
      print *, '  Enthalpy:', h
      print *, '  CP:', cp

 1000 format (15f11.4)
      read(*,*)
      end

      