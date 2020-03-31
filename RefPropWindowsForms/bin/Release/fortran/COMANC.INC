c-----------------------------------------------------------------------
c  Begin file COMANC.INC
c-----------------------------------------------------------------------

c..Ancillary equations.

c     Vapor pressure
      common /PSAT/   pska(nanc,nx),psexp(nanc,nx),
     &                pstrd(nx),psprd(nx),
     &                nps1(nx),nps2(nx),nps3(nx),
     &                nps4(nx),nps5(nx),nps6(nx),ipstyp(nx)

c     Liquid pressure (for PPF files)
      common /PLSAT/  plka(nanc,nx),plexp(nanc,nx),
     &                pltrd(nx),plprd(nx),
     &                npl1(nx),npl2(nx),npl3(nx),
     &                npl4(nx),npl5(nx),npl6(nx),ipltyp(nx)

c     Saturated liquid density
      common /DLSAT/  dlka(nanc,nx),dlexp(nanc,nx),
     &                dltrd(nx),dldrd(nx),
     &                ndl1(nx),ndl2(nx),ndl3(nx),
     &                ndl4(nx),ndl5(nx),ndl6(nx),idltyp(nx)

c     Saturated vapor density
      common /DVSAT/  dvka(nanc,nx),dvexp(nanc,nx),
     &                dvtrd(nx),dvdrd(nx),
     &                ndv1(nx),ndv2(nx),ndv3(nx),
     &                ndv4(nx),ndv5(nx),ndv6(nx),idvtyp(nx)

c     Melting line
      common /MELT/   pmeltk(nanc,nx),pmltex(nanc,nx),
     &                trdmlt(nx),prdmlt(nx),
     &                ntrmm1(nx),ntrmm2(nx),ntrmm3(nx),
     &                ntrmm4(nx),ntrmm5(nx),ntrmm6(nx),imlttyp(nx)

c     Sublimation line
      common /SUBL/   psublk(nanc,nx),psblex(nanc,nx),
     &                trdsbl(nx),prdsbl(nx),itypsbl(nx),
     &                ntrmb1(nx),ntrmb2(nx),ntrmb3(nx),
     &                ntrmb4(nx),ntrmb5(nx),ntrmb6(nx)

c     Surface tension
      common /SFTN/   fstkij(nx,nx,nmxstb),sigmak(nsigk,nx),
     &                sigexp(nsigk,nx),ntermu(nx)

c     Dielectric constant
      common /DECNST/ decf(ndecf,nx),deexpt(ndecf,nx),
     &                deexpd(ndecf,nx),deexpp(ndecf,nx),
     &                tredde(nx),dredde(nx),predde(nx),
     &                ntrmd1(nx),ntrmd2(nx),ntrmd3(nx),
     &                ntrmd4(nx),ntrmd5(nx),ntrmd6(nx)

c..Variables for heat of fusion
      common /FUSION/ dcpfus(nx),dhfus(nx),dvfus(nx),
     &                pfusref(nx),tfusref(nx)

c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                    End file COMANC.INC
c ======================================================================
