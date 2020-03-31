c  Begin file REFPROP.FOR

c  This file contains the three key routines that allow access to nearly all
c    properties available in Refprop.  These are the subroutines REFPROP,
c    ALLPROPS, and FLAGS.  Two shorter routines are available with more limited
c    inputs, these are REFPROP1 and ALLPROPS1.  A number of other routines are
c    also contained here that are called by these three or that could be used
c    in other situations by the user.


c  Contained here are:
c
c     subroutine REFPROP    (hFld,hIn,hOut,iUnits,iMass,iFlag,a,b,z,Output,hUnits,iUCode,x,y,x3,q,ierr,herr)
c     subroutine REFPROP1   (hIn,hOut,iUnits,iMass,a,b,z,c,q,ierr,herr)
c     subroutine REFPROP2   (hFld,hIn,hOut,iUnits,iFlag,a,b,z,Output,q,ierr,herr)
c     subroutine ALLPROPS   (hOut,iUnits,iMass,iFlag,T,D,z,Output,hUnitsArray,iUCodeArray,ierr,herr)
c     subroutine ALLPROPS0  (iIn,iOut,iFlag,T,D,z,Output,ierr,herr)
c     subroutine ALLPROPS1  (hOut,iUnits,T,D,z,c,ierr,herr)
c     subroutine ABFLSH     (ab,a,b,z,iFlag,T,P,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c     subroutine GETENUM    (iFlag,hEnum,iEnum,ierr,herr)
c     subroutine ERRMSG     (ierr,herr)
c     subroutine PHASE      (xPhase,hPhase)
c     subroutine SETFLUIDS  (hfld,ierr)
c     subroutine SETMIXTURE (hMixNme,z,ierr)
c     subroutine SETPATH    (hpth)
c     subroutine FLAGS      (hFlag,jFlag,kFlag,ierr,herr)
c     function CONVUNIT     (xIn,iUnits,iUnitID,z,hUnits,ierr,herr)
c     function CONVUNITS    (xIn,hUnitType,hOldUnit,hNewUnit,wM,ierr,herr)
c
c  Internal routines that should not be called outside of this file:
c     subroutine FLDSETUP   (hFld,iMass,iFlag,z,zm,ierr,herr)
c     subroutine GETUNITS   (iUnits,iUCode,iFlag,ic,z,ha,xIn,xOut,hUnits,ierr,herr)
c     subroutine REFSET1    (hFld,iUnits,io,a,b,Output,hUnits,iUCode,ierr,herr)
c     subroutine REFSET2    (io,iMass,x,y,x3,q,Output,hU,ierr,herr)
c     subroutine REFSAV     (io,iUnits,iMass,iFlag,x,y,x3,q,Dl,Dv,ierr,herr)
c     subroutine REFPRPUNITS()
c     subroutine READFLUIDSTRING (hFldIn,hFld,iMassX,iMix,iNew,ncc,z,ierr,herr)

c
c ======================================================================
c ======================================================================
c
c
      subroutine REFPROP (hFld,hIn,hOut,iUnits,iMass,iFlag,a,b,z,
     &                    Output,hUnits,iUCode,x,y,x3,q,ierr,herr)

c    Calculate the properties identified in the hOut string for the inputs specified in the hIn string for the
c    fluid or mixture given in the hFld string.  The unit identifier for the properties should be passed in
c    the iUnits variable (as described below).  Compositions can be sent as mole fractions or mass
c    fractions in the z array depending on the value of iMass.
c
c    Several items must be considered before using this routine.  The most important is the speed of calculations.
c    The original fortran code that called dedicated functions such as TPRHO, TPFLSH, PHFLSH, and so on (mostly
c    given in FLSH_SUB.FOR) and the non-iterative functions such as THERM and TRNPRP requires very few (or no)
c    string comparisons and are quite fast.  Multiple string comparisons are made to determine the inputs and outputs
c    the user has selected.  Due to the limitation of Fortran in string parsing, this will cause a dramatic
c    increase in the time required to make the calculations, such as two to three times as long as the dedicated
c    functions.  Thus the ease of use of this REFPROP subroutine versus the speed of calculation from the older
c    routines must be considered before developing any application.
c
c    **Information on hFld**
c
c    For a pure fluid, hfld contains the name of the fluid file (with a path if needed).
c
c    For a mixture, it contains the names of the constituents in the mixture separated by semicolons
c    or asterisks.  Once the routine has been called with hFld set to the desired fluids, a space can be
c    sent for all other calls that use the same fluid(s).  For a predefined mixture, the extension ".mix"
c    must be included.  If the composition is included in the hFld variable, or if a predefined mixture is
c    selected, the composition will be returned in the z array (on a molar or mass basis depending on iMass).
c    That composition (or other compositions) must be sent in z in all subsequent calls to this routine.
c    See subroutines SETFLUIDS and SETMIXTURE further below for additional information and examples.
c
c    *Note*:  The speed of the program will be increased (sometimes substantially) if you call this routine
c    only once with the name of the fluid and then never again unless your fluid or mixture changes.
c    If your composition changes, send the new composition in the z array rather then sending
c    a new string in the hfld variable.
c
c    Examples::
c
c        hFld='NITROGEN'
c        hFld='C:/Program Files (x86)/REFPROP/FLUIDS/R1234YF.FLD'
c        hFld='CARBON DIOXIDE'
c        hFld='METHANE;ETHANE;PROPANE;BUTANE;ISOBUTANE'
c        hFld='METHANE*ETHANE*PROPANE*BUTANE*ISOBUTANE'
c        hFld='R134a;0.3; R1234yf;0.3; R1234ze(Z);0.4'
c        hFld='CO2;0.2 * isobutane;0.3 * propadiene;0.5'
c        hFld='Nitrogen;Oxygen;Argon|0.4;0.3;0.3'
c        hFld='Nitrogen; Oxygen; Argon   ~   0.4; 0.3; 0.3'
c        hFld='R410A.MIX'
c
c    **Information on hIn**
c
c    Valid codes are T, P, D, E, H, S, and Q
c    (temperature, pressure, density, energy, enthalpy, entropy, and quality).  Two of these should be
c    sent together to identify the contents of the a and b variables.  For example, 'TP' would indicate
c    inputs of temperature and pressure, and 'TQ' would indicate inputs of temperature and quality.
c    A value of 0 for the quality will return a saturated liquid state, and a value of 1 will return
c    a saturated vapor state.  A value between 0 and 1 will return a two-phase state.
c    Valid inputs are:  TP, TD, TE, TH, TS, TQ, PD, PE, PH, PS, PQ, DE, DH, DS, DQ, ES, EQ, HS, HQ, SQ
c    (or the inverse of any of these, e.g., QT) (hIn is not case sensitive, e.g., 'TQ' = 'tq').
c    When q is >0 and <1, then the quality uses a molar basis when iMass=0, and a mass basis
c    when iMass=1.  The value of iUnits has no effect on the value of q (as either an input or output).
c    The shortcuts Tsat and Psat can be used to specify a saturation state for the liquid for a pure fluid.
c    To return, for example, the saturated vapor density, Dvap would be used as an output variable.
c    The order of the properties being sent to the routine in the variables a and b has to be the
c    same as the letters sent to hIn; for example, if hIn is 'QT', then a=q and b=T.
c
c    The ABFLSH routine is called to determine the phase of the inputs (liquid, vapor, or 2-phase), and
c    then the appropriate iterative routine will be called to obtain the independent properties of the
c    equations of state: temperature and density.  For subsequent calculations for
c    properties that are in the single phase, use the code TD&, where the symbol & indicates the single
c    phase state.  The time required with the use of TD& is negligible compared to that required for
c    the iterative solution called by ABFLSH.  However, the properties sent to this routine and the calculated
c    outputs are cached to avoid additional iterative calls when the solution has already been determined.
c    Be sure to read the warnings at the top of the ALLPROPS routine for additional information.
c
c    Flags to specify certain phases are listed below, for example, 'TD>' would specify an input state
c    in the liquid phase but which would normally be two-phase.  Those available are:
c
c    - ``**>`` or ``**L``:  When the letter 'L' is attached after the two letters that specify the input properties
c      (such as 'TP'), the routine will assume that the input properties are in the single phase
c      liquid region, or are within the two-phase area as a metastable state.  For example:
c      TP>, PH>, HSL.
c
c    - ``**<`` or ``**V``: The letter 'V' (or the sign '<') specifies that the input state for the properties listed
c      in the first two letters is in the single phase vapor (including metastable states).  For
c      example:  TP<, PH<, HSV.
c
c    - ``TH<`` or ``TH>``: Inputs of temperature and enthalpy (or occasionally temperature and internal energy) generally
c      have two valid states.  To obtain the root with the higher pressure, use TH> or TE>,
c      and for the lower pressure use TH< or TE<.
c
c    - ``*MELT``: Return properties at the melting point where the input property is specified by the ``*``,
c      for example TMELT requires the temperature for the input variable a, PMELT requires the
c      pressure for input variable a, and so on.
c
c    - ``*SUBL``: Return properties at the sublimation point, as described above for the melting point.
c
c    - ``CRIT``: Return properties at the critical point (for example, hIn='CRIT' and hOut='S' would return
c      the entropy at Tc and Dc).  For a mixture, the critical point defined by the equation of state
c      is only available if the SATSPLN routine has been called, otherwise an estimated value
c      is returned.
c
c    - ``TRIP``: Return liquid phase properties at the triple point.
c
c    - ``NBP``: Return properties at the normal boiling point.
c
c    - ``DSAT``: Return the saturation properties for the input density.
c
c    - ``HSAT``, ``HSAT2``: Enthalpy can be doubled valued in the vapor phase for some fluids.  In such a situation,
c      HSAT2 will return the root with the lower temperature.
c
c    - ``SSAT``, ``SSAT2``, ``SSAT3``: Entropy can be doubled or triple valued in the vapor phase for some fluids (see butane
c      for example).  In such a situation, SSAT will return the root at the highest temperature,
c      SSAT2 will return the middle root, and SSAT3 will return the root with the lowest
c      temperature.
c
c    Various flags are available that can be sent to this routine in the variable hIn to gain access to all other
c    features of the Refprop program.  These cannot be combined as multiple inputs in hIn:
c
c    - ``FLAGS``: Call the FLAGS routine at the bottom of this file to initialize the options available for
c      controlling certain aspects of the Refprop program.  Some of these include caching properties,
c      turning on/off different types of equations of state (Peng-Robinson, GERG-2008, and AGA-8), the
c      calorie to Joule definition, and so on.  The flag string (the first input to the FLAGS routine)
c      should be sent in hOut and the flag option should be sent in iFlag.  The output (3rd variable in the
c      routine) is returned in iUCode.  See the FLAGS routine for further information.  The variable hFld
c      for the REFPROP routine should be left blank when using this option.
c
c    - ``EOSMIN``: Return the property specified in hOut at the minimum temperature allowed in the equation
c      of state.  This is generally at the triple point in the liquid phase.  Note that an input
c      of P or D will not return the obvious minimum (zero), but the pressure and density at the
c      liquid phase triple point (or lower temperature limit for a mixture).  For water, the
c      triple point T is still returned, even though lower temperatures are possible.
c
c    - ``EOSMAX``: Return the maximum temperature, pressure, or density (as specified in hOut) for the
c      equation of state.  The maximum density of the equation of state does not occur at the
c      maximum pressure and temperature.  Only T, P, or D can be returned one at a time to
c      emphasize that properties at Tmax and Pmax are not the same as at Tmax and Dmax.
c
c    - ``SETREF``: Call the SETREF routine.  The reference state (DEF, NBP, IIR, ASH, OTH, OT0, or NA)
c      should be sent in hOut.  For the OTH and OT0 options, the values of h0, s0, T0, and P0
c      should be included in the hOut variable, separated by semicolons.  For example::
c
c               hOut='OTH;10.;1.;323.15;101.325'
c
c      This would set the enthalpy to 10 J/mol and the entropy to 1 J/mol-K at 323.15 K
c      and 101.325 kPa.  In the GUI is an option for mixtures to set the reference state to
c      either the composition in use or to each pure fluid.  To set this option through the DLL,
c      a value of either 1 or 2 should be sent to this routine in the variable a.
c      This will set the variable labeled ixflag in subroutione SETREF in the SETUP.FOR file.
c      All other options for this command are also explained in the SETUP.FOR file.
c
c    - ``SETREFOFF``: Turn off the inputs that were sent in the option above.
c
c    - ``PATH``: Call the SETPATH routine with the path given in hFld.
c
c    - ``SATSPLN``: Call the SATSPLN routine for the input composition (as described in the SAT_SUB.FOR
c      file).  The fluid name or mixture names and the composition must be sent with this
c      command or have already been setup before this is called.  This command is identical to
c      calling this routine with iFlag=1, except that it can be issued at any time.
c
c    **Information on hOut**
c
c    String output is returned in hUnits.  Numerical output is returned in Output(1).  For flags used to obtain a value
c    of a particular fluid in a mixture, the component number should be added after the command, such as NAME(3)
c    or FDIR(1).  Only one string output can be requested at a time for the following flags, down to the line
c    that says DLL#.  Use the ALLPROPS routine to return multiple strings for all the components in the mixture.
c    This is done without using the component number, e.g., sending "NAME" to that routine.  For numerical values,
c    multiple inputs can be requested here, and must be separated by spaces, commas, semicolons, or bars, but these
c    separators should not be mixed.  See subroutine ALLPROPS (which follows this routine) for further information.
c
c    - ``ALTID``: Return the alternative fluid whose mixing rules are used when others are not available.
c    - ``CAS#``: Return the CAS number.
c    - ``CHEMFORM``: Return the short chemical formula.
c    - ``SYNONYM``: Return the synonym found on the fifth line in the fluid files.
c    - ``FAMILY``: Return the family class used for several predictive schemes.
c    - ``FLDNAME``: Return the fluid file name sent to the SETUP routine.
c    - ``HASH``: Return the hash number.
c    - ``INCHI``: Return the InChI string.
c    - ``INCHIKEY``: Return the InChI key.
c    - ``LONGNAME``: Return the long fluid name given in the 3rd line of the fluid files.
c    - ``SAFETY``: Return the ASHRAE 34 classification.
c    - ``NAME``: Return the fluid short name.
c    - ``NCOMP``: Return the number of components.
c    - ``UNNUMBER``: Return the UN number.
c    - ``DOI_###(#)``: Return the DOI of the equation given by the three letters following the underscore, where the valid letters
c      are EOS for equation of state, VIS for viscosity, TCX for thermal conductivity, STN for surface tension,
c      DIE for dielectic constant, MLT for melting line, and SBL for sublimation line.  For example, DOI_EOS would
c      return the DOI for the equation of state.  For mixtures, the component must be specified at the end in the parenthesis,
c      for example, DOI_VIS(3).
c    - ``WEB_###(#)``: Return the web address for the equation given by the three letters following the underscore, as explained
c      in the DOI section.
c    - ``REFSTATE``: Return the reference state in use (NBP, IIR, ASH, OTH, etc.).
c    - ``GWP``: Return the global warming potential (found in the fluid file header).
c    - ``ODP``: Return the ozone depletion potential (found in the fluid file header).
c    - ``FDIR``: Return the location (directory) of the fluid file.  The directory is returned in both the hUnits string and in
c      herr if no other error occurred (paths that are more than 50 characters long are truncated in hUnits).
c      For mixtures, send FDIR(2), etc., to get the path of the second fluid and so on.
c    - ``UNITSTRING``: Return the units of the property (e.g., K, psia, kg/m^3, J/mol, etc.) identified in hIn for the unit system
c      defined in hFld (e.g., SI, E, etc.).  The input values for hIn are the labels described in the ALLPROPS
c      routine.  For example, 'D2DDP2' would return '(kg/m^3)/MPa^2' for 'SI' inputs.
c    - ``UNITNUMB``: Return in iUCode the integer value associated with a particular set of units defined in hFld (SI, E, etc.).
c      This integer value can then be used in subsequent calls for the iUnits variable.
c    - ``UNITS``: Perform both operations in UNITSTRING and UNITNUMB.
c    - ``UNITCONV``: Convert the property contained in the variable a from units given in hFLD to units given in hIn.
c      The unit strings are given much further below.  When converting from mole to mass units
c      (or vice versa), the molar mass must be sent in the variable b.  The type of property
c      (as specified in the CONVUNITS subroutine) must be appended to the string in hOut, for example,
c      hOut='UNITCONV_T' or hOut='UNITCONV_D'.
c    - ``UNITUSER``, ``UNITUSER2``: Set a predefined set of units based on the user's need.
c      Two different sets can be assigned depending on the input sent to the routine.  The variable hIn
c      contains the numbers that are specified by the enumerations in the CONSTS.INC file, separated by semicolons.
c      For example, hIn='0;157;0;0;0;403;0;0;0;0' would set the pressure to use units of atm and the
c      speed of sound to use units of km/h. The numbers are listed in the order of T, P, D, H, S, W, I, E, K, and N
c      (temperature, pressure, density, enthalpy, entropy, speed of sound, kinematic viscosity, viscosity,
c      thermal conductivity, and surface tension).  Because the enumerations might change, it is best to build this
c      string with the enumerations listed in the CONSTS.INC file rather than hard coding the numbers as shown above.
c    - ``DLL#``: Return the version number of the DLL in iUCode and the string value in hUnits.
c    - ``PHASE``: Return the phase of the state for the input fluids and properties.  See subroutine PHASE for a
c      listing of all possibilities.  The output is sent back in the hUnits variable.  No other command
c      can be sent with this one since hUnits is not an array.
c    - ``FULLCHEMFORM``: Return the long chemical formula.
c    - ``HEATINGVALUE``: Return the upper heating value.
c    - ``LIQUIDFLUIDSTRING``: Return a string that contains the fluid names and compositions for the liquid phase of a two-phase state.
c    - ``VAPORFLUIDSTRING``: Return a string that contains the fluid names and compositions for the vapor phase of a two-phase state.
c      For example, "R32;R125|0.25;0.75".  The string is passed back in hUnits.
c    - ``QMOLE``: Return the molar quality for 2-phase states.
c    - ``QMASS``: Return the mass quality for 2-phase states.
c    - ``XMASS``: Return the mass compositions in the Output array as with the X command.  See comment about ``Qmass``.
c    - ``XLIQ``: Return the mass or molar liquid compositions (depending on the value of iMass) for 2-phase states.
c    - ``XVAP``: Return the mass or molar vapor  compositions (depending on the value of iMass) for 2-phase states.
c    - ``XMOLELIQ``: Return the liquid compositions for 2-phase states on a mole basis regardless of the iMass variable.
c    - ``XMOLEVAP``: Return the vapor  compositions for 2-phase states on a mole basis regardless of the iMass variable.
c    - ``XMASSLIQ``: Return the liquid compositions for 2-phase states on a mass basis regardless of the iMass variable.
c    - ``XMASSVAP``: Return the vapor  compositions for 2-phase states on a mass basis regardless of the iMass variable.
c    - ``*LIQ``: (where * is T, P, D, etc.)
c      Return the liquid saturation properties for the property listed as the first letter.
c      This is only valid for saturation states or 2-phase states.
c    - ``*VAP``: (where * is T, P, D, etc.)
c      Return the vapor saturation properties for the property listed as the first letter.
c      This is only valid for saturation states or 2-phase states.
c    - ``FIJMIX``: Return the mixing parameters in the first six slots of the variable Output for the binary mixture identified
c      by the values in the a and b variables (i.e., integer values are sent in double precision variables).
c      The mixing rule is returned in the hUnits string.
c
c    **Information on iUnits**
c
c    Multiple unit systems are available for use in property values, such as the
c    SI system, English system, mixed sets, and so forth.  Each set is identified
c    with an enumerated value, which is sent as an input code in iUnits.
c
c    <FORTRAN ONLY> The enumerated value for the different unit systems are listed below and in
c    the ``CONSTS.INC`` file, which can be included in your FORTRAN program, as such::
c
c        include 'CONSTS.INC'
c
c    .. warning::
c
c        Do NOT include any other INC file in your programs
c
c    The enumerated values for the unit systems are given by the parameters
c
c    - ``iUnitsMolSI``
c    - ``iUnitsSI``
c    - ...
c
c    </FORTRAN ONLY>
c
c    In all environments other than FORTRAN, the iUnits variable should be retrieved from the GETENUM function
c    with a call like::
c
c        GETENUMdll(0,'MOLAR BASE SI',iEnum,ierr,herr)
c
c    .. warning::
c
c        The integer values for iUnits given below should **NEVER** be used directly, you should always retrieve the enumerated
c        value from GETENUM.  This is to allow the developers of Refprop flexibility in the future.
c
c    The unit systems used in Refprop are as follows::
c
c
c                         DEFAULT     MOLAR SI    MASS SI       SI WITH C
c        iUnits --->      0           1           2             3
c        Temperature      K           K           K             C
c        Pressure         kPa         MPa         MPa           MPa
c        Density          mol/dm^3    mol/dm^3    kg/m^3        kg/m^3
c        Enthalpy         J/mol       J/mol       J/g           J/g
c        Entropy          (J/mol)/K   (J/mol)/K   (J/g)/K       (J/g)/K
c        Speed            m/s         m/s         m/s           m/s
c        Kinematic vis.   cm^2/s      cm^2/s      cm^2/s        cm^2/s
c        Viscosity        uPa-s       uPa-s       uPa-s         uPa-s
c        Thermal cond.    W/(m-K)     mW/(m-K)    mW/(m-K)      mW/(m-K)
c        Surface tension  N/m         mN/m        mN/m          mN/m
c        Molar Mass       g/mol       g/mol       g/mol         g/mol
c
c                         MOLAR       MASS
c                         BASE SI     BASE SI     ENGLISH       MOLAR ENGLISH
c        iUnits --->      100         101         5             6
c        Temperature      K           K           F             F
c        Pressure         Pa          Pa          psia          psia
c        Density          mol/m^3     kg/m^3      lbm/ft^3      lbmol/ft^3
c        Enthalpy         J/mol       J/kg        Btu/lbm       Btu/lbmol
c        Entropy          (J/mol)/K   (J/kg)/K    (Btu/lbm)/R   (Btu/lbmol)/R
c        Speed            m/s         m/s         ft/s          ft/s
c        Kinematic vis.   m^2/s       m^2/s       ft^2/s        ft^2/s
c        Viscosity        Pa-s        Pa-s        lbm/(ft-s)    lbm/(ft-s)
c        Thermal cond.    W/(m-K)     W/(m-K)     Btu/(h-ft-R)  Btu/(h-ft-R)
c        Surface tension  N/m         N/m         lbf/ft        lbf/ft
c        Molar Mass       kg/mol      kg/mol      lbm/lbmol     lbm/lbmol
c
c                         MKS         CGS         MIXED         MEUNITS
c        iUnits --->      7           8           9             10
c        Temperature      K           K           K             C
c        Pressure         kPa         MPa         psia          bar
c        Density          kg/m^3      g/cm^3      g/cm^3        g/cm^3
c        Enthalpy         J/g         J/g         J/g           J/g
c        Entropy          (J/g)/K     (J/g)/K     (J/g)/K       (J/g)/K
c        Speed            m/s         cm/s        m/s           cm/s
c        Kinematic vis.   cm^2/s      cm^2/s      cm^2/s        cm^2/s
c        Viscosity        uPa-s       uPa-s       uPa-s         cpoise
c        Thermal cond.    W/(m-K)     mW/(m-K)    mW/(m-K)      mW/(m-K)
c        Surface tension  mN/m        dyne/cm     mN/m          mN/m
c        Molar Mass       g/mol       g/mol       g/mol         g/mol
c
c                         USER (can be changed by calling the REFPROP subroutine)
c        iUnits --->      11
c        Temperature      C
c        Pressure         psig
c        Density          kg/m^3
c        Enthalpy         J/g
c        Entropy          (J/g)/K
c        Speed            m/s
c        Kinematic vis.   cm^2/s
c        Viscosity        mPa-s
c        Thermal cond.    W/(m-K)
c        Surface tension  N/m
c        Molar Mass       g/mol
c
c    **Information on iUCode output**
c
c    The iUCode variable uses a four digit code that specifies the units of the property:
c
c    - Left digit         : Energy unit in J/mol or kJ/kg
c    - Left middle digit  : Density unit in mol/dm^3 or kg/m^3
c    - Right middle digit : Pressure unit in kPa
c    - Right digit        : Temperature unit in K
c
c    Each digit indicates the power of the unit, for example, a
c    value of 2 for the temperature digit corresponding to K^2.
c    Values from 6 to 9 specify a negative power digit, for
c    example, a value of 8 would be 1/kPa^2.
c
c    The following values give other examples::
c
c        1000    J/mol
c        0100    mol/dm^3
c        0010    kPa
c        0001    K
c        0000    -  (a value of zero assumes a dimensionless unit)
c        9000    1/(J/mol)
c        0910    kPa/(mol/dm^3)
c        0190    (mol/dm^3)/kPa
c        8765    K^5/[(J/mol)^2*(mol/dm^3)^3*kPa^4]
c        2082    (J/mol)^2*K^2/kPa^2
c        0830    kPa^3/(mol/dm^3)^2
c        9281    (mol/dm^3)^2*K/[(J/mol)*kPa^2]
c        8139    (mol/dm^3)*kPa^3/[(J/mol)^2*K]
c        2288    (J/mol)^2*(mol/dm^3)^2/[kPa^2*K^2]
c        1764    (J/mol)*K^4/[(mol/dm^3)^3*kPa^4]
c        4857    (J/mol)^4*kPa^5/[(mol/dm^3)^2*K^3]
c        2730    (J/mol)^2*kPa^3/(mol/dm^3)^3
c        6666    1/[(J/mol)^4*(mol/dm^3)^4*kPa^4*K^4]
c
c    Negative values represent special units not built on these four property types:
c
c    ========================== ============== =====================================
c    Property                   Parameter      Current Value (but subject to change)
c    -------------------------- -------------- -------------------------------------
c    Speed of sound             iUTypeW        -9
c    Viscosity                  iUTypeU        -10
c    Thermal conductivity       iUTypeK        -11
c    Surface tension            iUTypeN        -12
c    Quality                    iUType0        -13
c    Molar mass                 iUTypeM        -14
c    Kinematic viscosity        iUTypeI        -17
c    Mass flux                  iUTypeF        -27
c    Heating value (volume)     iUTypeG        -37
c    Dipole moment              iUTypeB        -38
c    ========================== ============== =====================================
c
c    The dimension statements for these variables are (in Fortran)::
c
c        parameter (ncmax=20)                       !Maximum number of components in the mixture
c        parameter (iPropMax=200)                   !Number of output properties available in ALLPROPS.
c        character*255 hFld,hIn,hOut,hUnits,herr              !hFld, hIn, and hOut can actually be of any length.
c        integer iUnits,iMass,iFlag,ierr,iUCode               !Note: as integer*4
c        double precision a,b,q,Output(iPropMax),z(ncmax),x(ncmax),y(ncmax),x3(ncmax)
c
c  Inputs:
c     hFld--Fluid string.  See above
c      hIn--Input string of properties being sent to the routine.
c     hOut--Various flags are available to gain access to all other features of the Refprop program.
c   iUnits--The unit system to be used for the input and output properties (such as SI, English, etc.)
c             See the details much further below for a complete description of the iUnits input value.
c             **NOTE** A mass based value for iUnits does not imply that the input and output compositions are
c             on a mass basis, this is specified with the iMass variable.
c    iMass--Specifies if the input composition is mole or mass based:
c             0 - Input compositions given in mole fractions, quality on a molar basis.
c             1 - Input compositions given in mass fractions, quality on a mass basis.
c           For two-phase states, the values in x and y will be returned on a mass basis if iMass=1.
c           **NOTE**  If the fluid string sent to this routine contains the word "mass" at the end (and thus contains the
c                       composition as well as the names of the fluids), this will have preference over the value of iMass
c                       when converting those compositions from a mass to a molar basis.  However, compositions sent
c                       back will still be based on the value in iMass.
c    iFlag--Flag to specify if the routine SATSPLN should be called (where a value of 1 activates the call).  (Eventually this
c             variable may be used to send multiple flags combined in this flag.)
c        a--First input property as specified in the hIn variable
c        b--Second input property as specified in the hIn variable
c        z--Composition on a mole or mass basis depending on the value sent in iMass (array of size ncmax=20).
c
c  Outputs:
c   Output--Array of properties specified by the hOut string (array of size 200 dimensioned as double precision).
c             The number -9999970 will be returned when errors occur, and the number -9999990 will be returned when
c             nothing was calculated. Read the comments in the ALLPROPS routine to fully understand the contents of this array.
c   hUnits--The units for the first property in the Output array.  Strings such as a fluid name may also be passed back in
c             this position. To obtain the units for all of the properties sent to the string, call the ALLPROPS routine instead.
c   iUCode--Unit code that represents the units of the first property in the Output array. See below for further details.
c        x--Composition of the liquid phase (array of mole fractions of size 20) for two-phase states on a mole or mass basis
c             depending on the value of iMass.
c        y--Composition of the vapor phase (array of mole fractions of size 20) for two-phase states on a mole or mass basis
c             depending on the value of iMass.
c       x3--Reserved for returning the composition of a second liquid phase for LLE or VLLE.
c        q--Vapor quality on a mole or mass basis depending on the value of iMass.  (See subroutine ABFLSH for the definitions
c             of values returned for this variable).  To obtain the molar quality regardless of iMass, send "qmole" as
c             an input in hIn, and vice-versa for "qmass".
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)



c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  08-01-17 EWL, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character hFld*(*),hIn*(*),hOut*(*),hUnitsArray(iPropMax)*50
      character*255 herr,hDummy,hIn1,hOut1,hU,hUnits,hSwap
      character*255 herrA(11),UCASE
      dimension Output(iPropMax),iUCodeArray(iPropMax),ierrA(11)
      dimension zx(ncmax),x(ncmax),y(ncmax),x3(ncmax)
      dimension z(ncmax),xkg(ncmax),ykg(ncmax)
      common /REFSETUP/ zx,aa,bb,T,D,P,iFlg,iEHT,hIn1,hOut1
      common /ALLPRPQ/ qsv,qmolesv,qmasssv,P2psv,e2psv,h2psv,s2psv,
     &                 iSwap,hSwap
      common /ALLPRPX/ iEnumA(iPropMax),iCompA(iPropMax),
     &                 iDlDv(iPropMax),nProp,iAllWarn

      q=xnotc
      iEHT=0
      ierr=0
      ierrA=0
      T=a
      D=b
      iSwap=0
      iSetup=0
      iSetRef=0
      iFlg=iFlag
      hIn1=UCASE(hIn)
      hOut1=UCASE(hOut)
      iRefSv=1
      if (.not.lcache2) iRefSv=0
      iDlDv=0
      Dl=0d0
      Dv=0d0
      if (nc.gt.0) then
        x(1:nc)=0d0
        y(1:nc)=0d0
        x3(1:nc)=0d0
      else
        x(1)=0d0
        x(2)=0d0
        y(1)=0d0
        x3(1)=0d0
      endif

      if (iDebug.ne.0) then
        inumber=inumber+1
        open (unit=22,file='input.dat',access='append')
        write (22,22) inumber,iUnits,iMass,iFlag,a,b,z(1),z(2),    !Write all inputs to a file called input.dat.
     &              hIn1(1:50),hOut1(1:30),hFld
        close (22)
 22     format (4i5,4f15.8,2x,a50,2x,a30,2x,a250)
      endif


c  Check for units, DLL#, flags, or path statements.
      call REFSET1 (hFld,iUnits,1,a,b,Output,hUnits,iUCode,hUnitsArray,
     &              ierrA(10),herrA(10))
      if (ierrA(10).ge.100 .or. iFlg.ne.0) goto 800

c  Check if SETUP needs to be called (by checking if the fluid file has changed).
      call FLDSETUP (hFld,iMass,iFlag,zx,z,ierrA(4),herrA(4))
      if (iSetup.ne.0) then
        x(1:nc)=0d0
        y(1:nc)=0d0
        x3(1:nc)=0d0
      endif
      if (ierrA(4).ge.100 .or. iFlg.ne.0) goto 800
      if (hin.ne.' ' .and. hOut.eq.' ') goto 806
      if (hIn.eq.' ' .and. hOut.eq.' ') goto 1000
      if (nc.eq.0) goto 104

c  Check for requested inputs that are not property related (also includes XMOLE and XMASS).
      call REFSET1 (hFld,iUnits,2,a,b,Output,hUnits,iUCode,hUnitsArray,
     &              ierrA(7),herrA(7))
      if (ierrA(7).ge.100 .or. iFlg.ne.0) goto 800
      if (hin.eq.' ' .and. hOut.ne.' ') goto 807

c  Convert input values to Refprop SI units.
      call GETENUM (2,hIn1(1:1),j1,i,hDummy)
      call GETENUM (2,hIn1(2:2),j2,i,hDummy)
      if (j1.eq.iUTypeT .and. (j2.eq.iUTypeH.or.j2.eq.iUTypeE))iEHT=999        !Check for TH or TE inputs.
      if (j2.eq.iUTypeT .and. (j1.eq.iUTypeH.or.j1.eq.iUTypeE))iEHT=999
      aa=CONVUNIT(a,-iUnits,j1,zx,hU,i,hDummy)
      bb=CONVUNIT(b,-iUnits,j2,zx,hU,i,hDummy)

c  Check for inputs that request metastable or saturation states.
      call REFSET2 (1,iMass,x,y,x3,q,Output,hUnits,ierrA(8),herrA(8))
      if (ierrA(8).ge.100 .or. iFlg.ne.0) goto 800
      if (iMass.eq.1 .and. iEHT.lt.100) iEHT=iEHT+200      !For inputs sent on a mass basis, specify that q is also on a mass basis if being sent.

c  Retrieve the saved state if the routine is called with the same inputs as in the previous call.
      call REFSAV (1,iUnits,iMass,iFlag,x,y,x3,q,Dl,Dv,i,hDummy)
      if (i.ge.100 .or. iFlg.ne.0) goto 800
      if (hIn1.eq.' ' .or. iRefSv.eq.1) goto 600

c-----------------------------------------------------------------------
c  Call ABFLSH to determine the phase of the input state and the
c    temperature, density, quality, and composition (for a 2-phase state).
c-----------------------------------------------------------------------
      call ABFLSH (hIn1(1:2),aa,bb,zx,iEHT,T,P,D,Dl,Dv,
     &             x,y,q,e,h,s,Cv,Cp,w,ierrA(1),herrA(1))
      if (ierrA(1).ge.100) goto 555
      call CHECKLIMITS (hIn1(1:2),T,D,P,zx,ierrA(2),herrA(2))    !Check if the state appears to be correct.
      if (ierrA(2).ge.100) goto 555

c  Save the calculated point for use if the routine is called again with the same inputs.
      call REFSAV (2,iUnits,iMass,iFlag,x,y,x3,q,Dl,Dv,i,hDummy)

c  Check for composition output.
 600  call REFSET2 (2,iMass,x,y,x3,q,Output,hUnits,ierrA(9),herrA(9))
      if (ierrA(9).ge.100 .or. iFlg.ne.0) goto 800

c  Set up flags for 2-phase states if needed.
      qsv=xnotc
      if (q.ge.0. .and. q.le.1) then
        l2phase=.true.       !For 2-phase states, return only values that are thermodynamically defined for this region.
        call GETPROPS (1,i,q,T,P2psv,D,Dl,Dv,zx,x,y,qkg,
     &                 e2psv,h2psv,s2psv,Cv,Cp,w)
        call QMASS (q,x,y,qkg,xkg,ykg,wliq,wvap,i,hDummy)
        if (hIn1(1:1).eq.'P' .or. hIn1(2:2).eq.'P') then
          if (XDIV(ABS(P2psv-P),P2psv).lt.0.001d0) P2psv=P   !Check if P was an input, and if so use that instead of the calculated value (but do a sanity check to make sure the two are close).
        endif
        qmolesv=q
        qmasssv=qkg
        qsv=q
        if (iMass.ne.0) qsv=qkg
      endif

c-----------------------------------------------------------------------
c  Call the ALLPROPS routine for single phase properties.
c-----------------------------------------------------------------------
      iPass=2
      call ALLPROPS (hOut1,-iUnits,0,-1,T,D,zx,
     &               Output,hUnitsArray,iUCodeArray,ierrA(3),herrA(3))
      l2phase=.false.
      if (ierrA(3).ge.100 .or. iFlg.ne.0) goto 800
      hUnits=hUnitsArray(1)
      iUCode=iUCodeArray(1)

c  Get liquid and vapor saturation properties for 2-phase or saturation states.
      if ((q.ge.0 .and. q.le.1) .or. SUM(iDlDv(1:nProp)).gt.0) then
        iDlDvFlg=1
        iPass=3
        call ALLPROPS (hOut1,-iUnits,0,-1,T,Dl,x,
     &                 Output,hUnitsArray,iUCodeArray,ierrA(5),herrA(5))
        if (ierrA(5).ge.100 .or. iFlg.ne.0) goto 800
        iDlDvFlg=2
        iPass=3
        call ALLPROPS (hOut1,-iUnits,0,-1,T,Dv,y,
     &                 Output,hUnitsArray,iUCodeArray,ierrA(6),herrA(6))
        if (ierrA(6).ge.100 .or. iFlg.ne.0) goto 800
        iDlDvFlg=0
      endif

c  Update and format error messages.
 555  if (Output(1).eq.xerr) hUnits=' '
 998  i=INDEX(hUnits,'{')
      if (i.gt.0) then
        hUnits=hUnits(i+1:)
        i=INDEX(hUnits,'}')
        if (i.gt.0) hUnits=hUnits(1:i-1)
      endif
      if (q.gt.0. .and. q.lt.1. .and. iMass.ne.0) then
        call QMASS (q,x,y,qkg,xkg,ykg,wliq,wvap,i,hDummy)
        x(1:nc)=xkg(1:nc)
        y(1:nc)=ykg(1:nc)
        q=qkg
      endif
      qsv=0

 1000 if (SUM(ierrA).eq.0) then
        ierr=0
        herr=' '
      else
        do i=1,11
          call SWAPERR (ierr,ierrA(i),herr,herrA(i))       !Swap errors starting with the most important, and keeping that unless a more serious error occurs.
          if (ierr.eq.-999) ierr=0                         !Remove internal error number.
        enddo
      endif
      if (iDebug.ne.0) then
        open (unit=23,file='output.dat',access='append')
        write(23,23)inumber,ierr,iucode,output(1),output(2),zx(1),zx(2),        !Write all outputs to a file called output.dat.
     &             x(1),x(2),q,hUnits(1:20),herr
        close (23)
  23    format (3i5,7f25.8,2x,a20,2x,a255)
      endif

      RETURN


 800  qsv=0d0
      if (iFlg.eq.999) goto 998
      if (iFlg.eq.998) goto 998
      if (iFlg.eq.555) goto 555
      if (iFlg.eq.600) goto 600
      if (iFlg.eq.852) goto 852
      if (iFlg.eq.801) goto 801
      if (iFlg.eq.802) goto 802
      if (iFlg.eq.803) goto 803
      if (iFlg.eq.804) goto 804
      if (iFlg.eq.808) goto 808
      goto 998


 104  call ERRNUM ( 104,0,' ',      ' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 852  call ERRNUM (-852,0,' ',      ' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 801  call ERRNUM ( 801,0,'REFPROP',' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 802  call ERRNUM ( 802,0,'REFPROP',' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 803  call ERRNUM (-803,0,'REFPROP',' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 804  call ERRNUM ( 804,0,'REFPROP',' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 806  call ERRNUM (-806,0,'REFPROP',' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 807  call ERRNUM (-807,0,'REFPROP',' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000
 808  call ERRNUM (-808,0,'REFPROP',' ',0d0,0d0,0d0,ierrA(11),herrA(11))
      goto 1000

      end                                             subroutine REFPROP
c
c ======================================================================
c
      subroutine REFPROP1 (hIn,hOut,iUnits,iMass,a,b,z,c,q,ierr,herr)
c
c    Short version of subroutine REFPROP that eliminates the arrays and the
c    need to send the fluid names each time.
c
c    The variable Output (which is an array) is not included here,
c    rather the variable c returns the calculated value as a double
c    precision variable, and thus only one value can be returned at a time.
c    If the error number (ierr) is zero, the string contained in hUnits
c    will be sent back in herr.
c
c    If q (quality) returns a value between zero and one (and thus
c    the state is two-phase), the REFPROP routine will be needed to obtain
c    the equilibrium compositions.
c
c    Inputs:
c       hFld--Fluid string.  See above
c        hIn--Input string of properties being sent to the routine.
c       hOut--Output string of properties to be calculated.
c     iUnits--The unit system to be used for the input and output properties (such as SI, English, etc.)
c               See the details in the REFPROP subroutine for a complete description of the iUnits input value.
c               **NOTE**: A mass based value for iUnits does not imply that the input and output compositions are
c               on a mass basis, this is specified with the iMass variable.
c      iMass--Specifies if the input composition is mole or mass based:
c               0 - Input compositions given in mole fractions, quality on a molar basis.
c               1 - Input compositions given in mass fractions, quality on a mass basis.
c             For two-phase states, the values in x and y will be returned on a mass basis if iMass=1.
c             **NOTE**  If the fluid string sent to this routine contains the word "mass" at the end (and thus contains the
c                         composition as well as the names of the fluids), this will have preference over the value of iMass
c                         when converting those compositions from a mass to a molar basis.  However, compositions sent
c                         back will still be based on the value in iMass.
c      iFlag--Flag to specify if the routine SATSPLN should be called (where a value of 1 activates the call).  (Eventually this
c               variable may be used to send multiple flags combined in this flag.)
c          a--First input property as specified in the hIn variable.
c          b--Second input property as specified in the hIn variable.
c          z--Composition on a mole or mass basis depending on the value sent in iMass (array of size ncmax=20).
c
c    Outputs:
c          c--Output value. The number -9999970 will be returned when errors occur, and the number -9999990 will be returned when
c               nothing was calculated. Read the comments in the ALLPROPS routine for more information.
c          q--Vapor quality on a mole or mass basis depending on the value of iMass.  (See subroutine ABFLSH for the definitions
c               of values returned for this variable).  To obtain the molar quality regardless of iMass, send "qmole" as
c               an input in hIn, and vice-versa for "qmass".
c       ierr--Error flag:  Output values are identical to those in the REFPROP subroutine
c       herr--Error string (character*255)
c
      include 'COMMONS.INC'
      character hIn*(*),hOut*(*),herr*255,hUnits*255
      dimension Output(iPropMax)
      dimension z(ncmax),x(ncmax),y(ncmax),x3(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::REFPROP1

      call REFPROP (' ',hIn,hOut,iUnits,iMass,0,a,b,z,
     &              Output,hUnits,iUCode,x,y,x3,q,ierr,herr)
      c=Output(1)
      if (ierr.eq.0) herr=hUnits
      end                                            subroutine REFPROP1
c
c ======================================================================
c
      subroutine REFPROP2 (hFld,hIn,hOut,iUnits,iFlag,a,b,z,Output,q,
     &                     ierr,herr)
c
c    Short version of subroutine REFPROP that eliminates the less used variables such as the x and y composition
c    arrays. If the error number (ierr) is zero, the string contained in hUnits will be sent back in herr.
c
c    If q (quality) returns a value between zero and one (and thus the state is two-phase), the REFPROP routine
c    will be needed to obtain the equilibrium compositions.
c
c    See subroutine REFPROP for further information on the input and output variables below.
c
c    Inputs:
c       hFld--Fluid string.
c        hIn--Input string of properties being sent to the routine.
c       hOut--Output string of properties to be calculated.
c     iUnits--The unit system to be used for the input and output properties (such as SI, English, etc.)
c      iFlag--Flag to specify if the routine SATSPLN should be called (where a value of 1 activates the call).
c          a--First input property as specified in the hIn variable.
c          b--Second input property as specified in the hIn variable.
c          z--Molar composition (array of size ncmax=20).
c
c    Outputs:
c     Output--Array of properties specified by the hOut string (array of size 200 dimensioned as double precision).
c             The number -9999970 will be returned when errors occur, and the number -9999990 will be returned when
c             nothing was calculated. Read the comments in the ALLPROPS routine to fully understand the contents of this array.
c          q--Vapor quality on a mole basis.
c       ierr--Error flag:  Output values are identical to those in the REFPROP subroutine
c       herr--Error string (character*255)
c
      include 'COMMONS.INC'
      character hFld*(*),hIn*(*),hOut*(*),herr*255,hUnits*255
      dimension Output(iPropMax),z(ncmax),x(ncmax),y(ncmax),x3(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::REFPROP2

      call REFPROP (hFld,hIn,hOut,iUnits,0,iFlag,a,b,z,
     &              Output,hUnits,iUCode,x,y,x3,q,ierr,herr)
      if (ierr.eq.0) herr=hUnits
      end                                            subroutine REFPROP2
c
c ======================================================================
c
      subroutine ALLPROPS1 (hOut,iUnits,T,D,z,c,ierr,herr)

c    Short version of subroutine ALLPROPS that eliminates the arrays but
c    allows the calculation of only one property at a time.
c    All inputs and outputs are described in the ALLPROPS routine.
c
c    Inputs:
c       hOut--Input string of properties to calculate (of any length).
c             Inputs can be separated by spaces, commas, semicolons,
c               or bars, but should not be mixed.  For example, a proper
c               string would be hOut='T,P,D,H,E,S', whereas an improperly
c               defined string would be hOut='T,P;D H|E,S'.
c             Use of lower or upper case is not important.
c             Some properties will return multiple values, for example,
c               hOut='F,Fc,XMOLE' will return 12 properties for a four
c               component system, these being F(1), F(2), F(3),
c               F(4), Fc(1), Fc(2), etc.
c             To retrieve the property of a single component, use,
c               for example, hOut='XMOLE(2),XMOLE(3)'
c     iUnits--See subroutine REFPROP for a complete description of the
c               iUnits input value.
c             A negative value for iUnits indicates that the input
c               temperature is given in K and density in mol/dm^3,
c               (Refprop default units), otherwise T and D will be
c               converted first to K and mol/dm^3.  Do not use the
c               negative value for the iUnits parameter everywhere,
c               only in this one situation.
c         T--Temperature, with units based on the value of iUnits.
c         D--Density, with units based on the value of iUnits.
c         z--Composition on a mole or mass basis (array of size ncmax=20)
c
c    Outputs:
c     c--Output value (array of size 200 dimensioned as double precision).
c               The number -9999970 will be returned when errors occur
c               or no input was requested.
c       ierr--Error flag:  0 - Successful
c       herr--Error string (character*255)

      include 'COMMONS.INC'
      character*255 herr,hOut*(*),hUnitsArray(iPropMax)*50
      dimension OutputArray(iPropMax),iUCodeArray(iPropMax),z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ALLPROPS1

      call ALLPROPS (hOut,iUnits,0,0,T,D,z,
     &               OutputArray,hUnitsArray,iUCodeArray,ierr,herr)
      c=OutputArray(1)
      end                                           subroutine ALLPROPS1
c
c ======================================================================
c
      subroutine ALLPROPS (hOut,iUnits,iMass,iFlag,T,D,z,
     &                     Output,hUnitsArray,iUCodeArray,ierr,herr)
c
c    Calculate the single-phase properties identified in the hOut string
c    at the temperature, density, and composition sent to the routine.
c    Return the properties in mass or molar units depending on iUnits.
c
c    .. warning::
c
c        Do NOT call this routine for two-phase states,
c        otherwise it will return metastable states if near but inside the
c        phase boundary, and complete nonsense at other conditions.  The
c        value of q that is returned from the flash routines will indicate
c        a two phase state by returning a value between 0 and 1.  In such
c        a situation, properties can only be calculated for the saturated
c        liquid and vapor states.  For example, when calling PHFLSH::
c
c            call PHFLSH (P,h,z,T,D,Dl,Dv,x,y,q,e,s,Cv,Cp,w,ierr,herr)
c
c        If q>0 and q<1, then values of the liquid and vapor compositions will
c        be returned in the x and y arrays, and the properties of the
c        liquid and vapor states can be calculated, for example::
c
c            call ENTRO (T,Dl,x,sliq)
c            call ENTRO (T,Dv,x,svap)
c
c    ALLPROPS was the name of a program developed at the University of
c    Idaho under the direction of R.B. Stewart and R.T Jacobsen at the
c    Center for Applied Thermodynamic Studies (CATS), with
c    S.G. Penoncello and S.W. Beyerlein as professors at this institution.
c    The software was distributed for about 10 years until around
c    2000 when it was officially replaced by the Refprop program.
c    Some of the techniques from ALLPROPS was used in the development of
c    Version 6 of Refprop, and were in some ways its forerunner.  The
c    original code was DOS based and distributed on 3 1/2" floppy disks
c    by regular mail.  A Visual Basic version of ALLPROPS was developed
c    in about 1995, and, although rarely distributed, inspired the
c    graphical interface included with Version 7.0 and above of Refprop.
c
c    The ALLPROPS code was used to develop equations of state at the
c    University of Idaho, and many of these are still in use today, such
c    as ethanol, neon, R-11, R-12, R-22, R-23, R-143a, and the mixture
c    air, along with the architecture behind the GERG-2008 mixture
c    model.  The equations of state for ethylene, nitrogen, and oxygen
c    were developed in conjunction with the Ruhr University in Bochum,
c    Germany, including a six month stay by R. Span from Bochum with
c    E.W. Lemmon at Idaho while both worked on their upper degrees.
c    The underlying code in the fitting program developed at CATS is
c    still in use today, and has been used in nearly all equations of
c    state developed over the last 20 years.
c
c    The name ALLPROPS was revived at NIST in 2017 in memory of the old but
c    not forgotten program whose roots still form the foundation of much
c    that goes on behind the scenes in the development of equations of
c    state and property software.
c
c    **Calling from the DLL**
c
c    Two routines are available in the DLL, these are ALLPROPSdll and
c    ALLPRP200dll.  Both compress the ``hUnitsArray`` array so that it can be passed
c    back as a single string.  The segments are divided by the character '|'.
c    Both routines use the same list of arguments::
c
c        (hOut,iUnits,iMass,iFlag,T,D,z,Output,hUnits,iUCode,ierr,herr)
c
c    In ALLPROPSdll, the hOut string is 255 characters long, the hUnits string is
c    1000 characters long, and the Output and iUCode arrays each have a length of 20.
c    In ALLPRP200dll, the hOut and hUnits strings are 10000 characters long, and
c    the Output and iUCode arrays each have a length of 200.
c
c    Below are the labels that can be sent in the hOut string and a very short
c    description of the property and units based on either a SI molar system
c    (iUnits=1) or SI mass system (iUnits=2, or 3 with temperature in C).
c
c    **Note about criticals**: The items TC,PC,DC will return the critical point of a pure fluid, or, when SATSPLN
c    has been called, the critical point of the mixture (or a very close approximation).
c    When the splines have not been set up, the values are the same as TCEST below.
c    For the critical points of the pure fluids in a mixture, use TCRIT, etc., explained
c    below, which is useful when multiple fluids have been loaded.
c    Parameters in the HMX.BNC file, which, for a binary mixture, are close for Type I
c    mixtures, but for a multi-component or non-Type I mixture, can be significantly wrong.
c
c    ========== ============================================== =======================  ========================
c    Label      Description                                    SI Molar Units           SI Mass Units
c    ---------- ---------------------------------------------- -----------------------  ------------------------
c
c     Regular properties
c    -----------------------------------------------------------------------------------------------------------
c    T          Temperature                                    [K]                      [K]
c    P          Pressure                                       [kPa]                    [kPa]
c    D          Density                                        [mol/dm^3]               [kg/m^3]
c    V          Volume                                         [dm^3/mol]               [m^3/kg]
c    E          Internal energy                                [J/mol]                  [kJ/kg]
c    H          Enthalpy                                       [J/mol]                  [kJ/kg]
c    S          Entropy                                        [J/(mol*K)]              [(kJ/kg)/K]
c    CV         Isochoric heat capacity                        [J/(mol*K)]              [(kJ/kg)/K]
c    CP         Isobaric heat capacity                         [J/(mol*K)]              [(kJ/kg)/K]
c    CP/CV      Heat capacity ratio                            [-]                      [-]
c    W          Speed of sound                                 [m/s]                    [m/s]
c    Z          Compressibility factor                         [-]                      [-]
c    JT         Isenthalpic Joule-Thomson coefficient          [K/kPa]                  [K/kPa]
c    A          Helmholtz energy                               [J/mol]                  [kJ/kg]
c    (which is equal to A0+AR below, or R*T*(PHIG00+PHIR00)
c    G          Gibbs energy                                   [J/mol]                  [kJ/kg]
c    R          Gas constant                                   [J/(mol*K)]              [(kJ/kg)/K]
c    M          Molar mass (or of the mixture)                 [g/mol]                  [g/mol]
c    QMASS      Quality (not implemented, q not known)         N.A.                     [kg/kg]
c    QMOLE      Quality (not implemented, q not known)         [mol/mol]                N.A.
c
c    Not so regular properties
c    -----------------------------------------------------------------------------------------------------------
c    KAPPA      Isothermal compressibility                     [1/kPa]                  [1/kPa]
c    BETA       Volume expansivity                             [1/K]                    [1/K]
c    ISENK      Isentropic expansion coefficient               [-]                      [-]
c    KT         Isothermal expansion coefficient               [-]                      [-]
c    BETAS      Adiabatic compressibility                      [1/kPa]                  [1/kPa]
c    BS         Adiabatic bulk modulus                         [kPa]                    [kPa]
c    KKT        Isothermal bulk modulus                        [kPa]                    [kPa]
c    THROTT     Isothermal throttling coefficient              [dm^3/mol]               [m^3/kg]
c
c    Derivatives
c    -----------------------------------------------------------------------------------------------------------
c    DPDD       dP/dD at constant T                            [kPa/(dm^3/mol)]         [kPa/(m^3/kg)]
c    DPDT       dP/dT at constant D                            [kPa/K]                  [kPa/K]
c    DDDP       dD/dP at constant T                            [(mol/dm^3)/kPa]         [(kg/m^3)/kPa]
c    DDDT       dD/dT at constant P                            [(mol/dm^3)/K]           [(kg/m^3)/K]
c    DTDP       dT/dP at constant D                            [K/kPa]                  [K/kPa]
c    DTDD       dT/dD at constant P                            [(dm^3/mol)*K]           [(m^3/kg)*K]
c    D2PDD2     d^2P/dD^2 at constant T                        [kPa/(dm^3/mol)^2]       [kPa/(m^3/kg)^2]
c    D2PDT2     d^2P/dT^2 at constant D                        [kPa/K^2]                [kPa/K^2]
c    D2PDTD     d^2P/dTdD                                      [kPa/(dm^3/mol)/K]       [kPa/(m^3/kg)/K]
c    D2DDP2     d^2D/dP^2 at constant T                        [(mol/dm^3)/kPa^2]       [(kg/m^3)/kPa^2]
c    D2DDT2     d^2D/dT^2 at constant P                        [(mol/dm^3)/K^2]         [(kg/m^3)/K^2]
c    D2DDPT     d^2D/dPdT                                      [(mol/dm^3)/(kPa*K)]     [(kg/m^3)/[kPa*K]]
c    D2TDP2     d^2T/dP^2 at constant D                        [K/kPa^2]                [K/kPa^2]
c    D2TDD2     d^2T/dD^2 at constant P                        [(dm^3/mol)^2*K]         [(m^3/kg)^2*K]
c    D2TDPD     d^2T/dPdD                                      [K/(dm^3/mol)/kPa]       [K/(m^3/kg)/kPa]
c
c    Enthalpy derivatives
c    -----------------------------------------------------------------------------------------------------------
c    DHDT_D     dH/dT at constant D                            [(J/mol)/K]              [(kJ/kg)/K]
c    DHDT_P     dH/dT at constant P                            [(J/mol)/K]              [(kJ/kg)/K]
c    DHDD_P     dH/dD at constant P                            [(J/mol)*(dm^3/mol)]     [(kJ/kg)*(m^3/kg)]
c    DHDD_T     dH/dD at constant T                            [(J/mol)*(dm^3/mol)]     [(kJ/kg)*(m^3/kg)]
c    DHDP_T     dH/dP at constant T                            [(J/mol)/kPa]            [(kJ/kg)/kPa]
c    DHDP_D     dH/dP at constant D                            [(J/mol)/kPa]            [(kJ/kg)/kPa]
c
c    Entropy derivatives
c    -----------------------------------------------------------------------------------------------------------
c    DSDT_D     dS/dT at constant D                            [(J/mol)/K^2]            [(kJ/kg)/K^2]
c    DSDT_P     dS/dT at constant P                            [(J/mol)/K^2]            [(kJ/kg)/K^2]
c    DSDD_T     dS/dD at constant T                            [(J/mol)*(dm^3/mol)/K]   [(kJ/kg)*(m^3/kg)/K]
c    DSDD_P     dS/dD at constant P                            [(J/mol)*(dm^3/mol)/K]   [(kJ/kg)*(m^3/kg)/K]
c    DSDP_T     dS/dP at constant T                            [(J/mol)/(kPa*K)]        [(kJ/kg)/[kPa*K]]
c    DSDP_D     dS/dP at constant D                            [(J/mol)/(kPa*K)]        [(kJ/kg)/[kPa*K]]
c
c    Virial Coefficients
c    -----------------------------------------------------------------------------------------------------------
c    Bvir       Second virial coefficient                      [dm^3/mol]               [m^3/kg]
c    Cvir       Third virial coefficient                       [(dm^3/mol)^2]           [(m^3/kg)^2]
c    Dvir       Fourth virial coefficient                      [(dm^3/mol)^3]           [(m^3/kg)^3]
c    Evir       Fifth virial coefficient                       [(dm^3/mol)^4]           [(m^3/kg)^4]
c    dBvirdT    1st derivative of B with respect to T          [(dm^3/mol)/K]           [(m^3/kg)/K]
c    d2BvirdT2  2nd derivative of B with respect to T          [(dm^3/mol)/K^2]         [(m^3/kg)/K^2]
c    dCvirdT    1st derivative of C with respect to T          [(dm^3/mol)^2/K]         [(m^3/kg)^2/K]
c    d2CvirdT2  2nd derivative of C with respect to T          [(dm^3/mol)^2/K^2]       [(m^3/kg)^2/K^2]
c    dDvirdT    1st derivative of D with respect to T          [(dm^3/mol)^3/K]         [(m^3/kg)^3/K]
c    d2DvirdT2  2nd derivative of D with respect to T          [(dm^3/mol)^3/K^2]       [(m^3/kg)^3/K^2]
c    BA         Second acoustic virial coefficient             [dm^3/mol]               [m^3/kg]
c    CA         Third acoustic virial coefficient              [(dm^3/mol)^2]           [(m^3/kg)^2]
c
c    EOS testing properties
c    -----------------------------------------------------------------------------------------------------------
c    GRUN       Gruneisen  parameter                           [-]                      [-]
c    PIP        Phase identification parameter                 [-]                      [-]
c    RIEM       Thermodyn. curvature (nm^3/molecule)
c    (Z-1)/D    (Z-1) over the density                         [dm^3/mol]               [m^3/kg]
c    (Z-1)/P    (Z-1) over the pressure                        [1/kPa]                  [1/kPa]
c    P*V        Pressure times volume                          [(dm^3/mol)*kPa]         [(m^3/kg)*kPa]
c    S*D        Entropy times density                          [J/(mol*K)*(mol/dm^3)]   [(kJ/kg)*(kg/m^3)/K]
c    N1/T       Negative reciprocal temperature                [1/K]                    [1/K]
c    RD         Rectilinear diameter (Dl+Dv)/2                 [mol/dm^3]               [kg/m^3]
c
c    Properties from ancillary equations
c    -----------------------------------------------------------------------------------------------------------
c    ANC-TP     Vapor pressure from ancillary given T          [kPa]                    [kPa]
c    ANC-TDL    Sat. liquid dens. from ancillary given T       [mol/dm^3]               [kg/m^3]
c    ANC-TDV    Sat. vapor dens. from ancillary given T        [mol/dm^3]               [kg/m^3]
c    ANC-PT     Vapor temp. from ancillary given P             [K]                      [K]
c    ANC-DT     Vapor temp. from ancillary given D             [K]                      [K]
c    MELT-TP    Melting pressure given T                       [kPa]                    [kPa]
c    MELT-PT    Melting temperature given P                    [K]                      [K]
c    SUBL-TP    Sublimation pressure given T                   [kPa]                    [kPa]
c    SUBL-PT    Sublimation temperature given P                [K]                      [K]
c
c    Less common saturation properties
c    -----------------------------------------------------------------------------------------------------------
c    CSAT       Saturated heat capacity                        [J/(mol*K)]              [(kJ/kg)/K]
c    CV2P       Isochoric two-phase heat capacity              [J/(mol*K)]              [(kJ/kg)/K]
c    DPDTSAT    dP/dT along the saturation line                [kPa/K]                  [kPa/K]
c    DHDZSAT    dH/dZ along the sat. line (Waring)             [J/mol]                  [kJ/kg]
c    LIQSPNDL   Density at the liquid spinodal                 [mol/dm^3]               [kg/m^3]
c    VAPSPNDL   Density at the vapor spinodal                  [mol/dm^3]               [kg/m^3]
c
c    Excess properties
c    -----------------------------------------------------------------------------------------------------------
c    VE         Excess volume                                  [dm^3/mol]               [m^3/kg]
c    EE         Excess energy                                  [J/mol]                  [kJ/kg]
c    HE         Excess enthalpy                                [J/mol]                  [kJ/kg]
c    SE         Excess entropy                                 [J/(mol*K)]              [(kJ/kg)/K]
c    AE         Excess Helmholtz energy                        [J/mol]                  [kJ/kg]
c    GE         Excess Gibbs energy                            [J/mol]                  [kJ/kg]
c    B12        B12                                            [dm^3/mol]               [m^3/kg]
c
c    Ideal gas properties
c    -----------------------------------------------------------------------------------------------------------
c    P0         Ideal gas pressure                             [kPa]                    [kPa]
c    E0         Ideal gas internal energy                      [J/mol]                  [kJ/kg]
c    H0         Ideal gas enthalpy                             [J/mol]                  [kJ/kg]
c    S0         Ideal gas entropy                              [J/(mol*K)]              [(kJ/kg)/K]
c    CV0        Ideal gas isochoric heat capacity              [J/(mol*K)]              [(kJ/kg)/K]
c    CP0        Ideal gas isobaric heat capacity               [J/(mol*K)]              [(kJ/kg)/K]
c    CP0/CV0    Ideal gas heat capacity ratio                  [-]                      [-]
c    W0         Ideal gas speed of sound                       [m/s]                    [m/s]
c    A0         Ideal gas Helmholtz energy                     [J/mol]                  [kJ/kg]
c    G0         Ideal gas Gibbs energy                         [J/mol]                  [kJ/kg]
c    P-P0       Pressure minus ideal gas pressure              [kPa]                    [kPa]
c
c    Residual properties
c    -----------------------------------------------------------------------------------------------------------
c    PR         Residual pressure (P-D*Rxgas*T)                [kPa]                    [kPa]
c    ER         Residual internal energy                       [J/mol]                  [kJ/kg]
c    HR         Residual enthalpy                              [J/mol]                  [kJ/kg]
c    SR         Residual entropy                               [J/(mol*K)]              [(kJ/kg)/K]
c    CVR        Residual isochoric heat capacity               [J/(mol*K)]              [(kJ/kg)/K]
c    CPR        Residual isobaric heat capacity                [J/(mol*K)]              [(kJ/kg)/K]
c    AR         Residual Helmholtz energy                      [J/mol]                  [kJ/kg]
c    GR         Residual Gibbs energy                          [J/mol]                  [kJ/kg]
c
c    Ideal-gas contributions to the Helmholtz energy
c    -----------------------------------------------------------------------------------------------------------
c    PHIG00     Red. IG Helmholtz energy A0/RT                 [-]                      [-]
c    PHIG10     tau*[d(A0/RT)/d(tau)]                          [-]                      [-]
c    PHIG20     tau^2*[d^2(A0/RT)/d(tau)^2]                    [-]                      [-]
c    PHIG30     tau^3*[d^3(A0/RT)/d(tau)^3]                    [-]                      [-]
c    PHIG01     del*[d(A0/RT)/d(del)]                          [-]                      [-]
c    PHIG02     del^2*[d^2(A0/RT)/d(del)^2]                    [-]                      [-]
c    PHIG03     del^3*[d^3(A0/RT)/d(del)^3]                    [-]                      [-]
c    PHIG11     tau*del*[d^2(A0/RT)/d(tau)d(del)]              [-]                      [-]
c    PHIG12     tau*del^2*[d^3(A0/RT)/d(tau)d(del)^2]          [-]                      [-]
c    PHIG21     tau^2*del*[d^3(A0/RT)/d(tau)^2d(del)]          [-]                      [-]
c
c    Residual contributions to the Helmholtz energy
c    -----------------------------------------------------------------------------------------------------------
c    PHIR00     Red. resid. Helmholtz energy Ar/RT             [-]                      [-]
c    PHIR10     tau  *[d  (Ar/RT)/d(tau)]                      [-]                      [-]
c    PHIR20     tau^2*[d^2(Ar/RT)/d(tau)^2]                    [-]                      [-]
c    PHIR30     tau^3*[d^3(Ar/RT)/d(tau)^3]                    [-]                      [-]
c    PHIR01     del  *[d  (Ar/RT)/d(del)]                      [-]                      [-]
c    PHIR02     del^2*[d^2(Ar/RT)/d(del)^2]                    [-]                      [-]
c    PHIR03     del^3*[d^3(Ar/RT)/d(del)^3]                    [-]                      [-]
c    PHIR11     tau*del    *[d^2(Ar/RT)/d(tau)  d(del)]        [-]                      [-]
c    PHIR12     tau*del^2  *[d^3(Ar/RT)/d(tau)  d(del)^2]      [-]                      [-]
c    PHIR13     tau*del^3  *[d^4(Ar/RT)/d(tau)  d(del)^3]      [-]                      [-]
c    PHIR21     tau^2*del  *[d^3(Ar/RT)/d(tau)  d(del)]        [-]                      [-]
c    PHIR22     tau^2*del^2*[d^4(Ar/RT)/d(tau)^2d(del)^2]      [-]                      [-]
c    PHIR23     tau^2*del^3*[d^5(Ar/RT)/d(tau)^3d(del)^3]      [-]                      [-]
c    PHIR31     tau^3*del  *[d^4(Ar/RT)/d(tau)  d(del)]        [-]                      [-]
c    PHIR32     tau^3*del^2*[d^5(Ar/RT)/d(tau)^2d(del)^2]      [-]                      [-]
c    PHIR33     tau^3*del^3*[d^6(Ar/RT)/d(tau)^3d(del)^3]      [-]                      [-]
c
c    Critical point and P,T maximums along isopleth (see above)
c    -----------------------------------------------------------------------------------------------------------
c    TC         Critical temperature of a pure fluid           [K]                      [K]
c    PC         Critical pressure of a pure fluid              [kPa]                    [kPa]
c    DC         Critical density of a pure fluid               [mol/dm^3]               [kg/m^3]
c    TCEST      Estimated critical temperature                 [K]                      [K]
c    PCEST      Estimated critical temperature                 [kPa]                    [kPa]
c    DCEST      Estimated critical density                     [mol/dm^3]               [kg/m^3]
c    TMAXT      Temperature at cricondentherm                  [K]                      [K]
c    PMAXT      Pressure at cricondentherm                     [kPa]                    [kPa]
c    DMAXT      Density at cricondentherm                      [mol/dm^3]               [kg/m^3]
c    TMAXP      Temperature at cricondenbar                    [K]                      [K]
c    PMAXP      Pressure at cricondenbar                       [kPa]                    [kPa]
c    DMAXP      Density at cricondenbar                        [mol/dm^3]               [kg/m^3]
c
c    Reducing parameters
c    -----------------------------------------------------------------------------------------------------------
c    TRED       Reducing temperature                           [K]                      [K]
c    DRED       Reducing density                               [mol/dm^3]               [kg/m^3]
c    TAU        Tc/T (or Tred/T)                               [-]                      [-]
c    DEL        D/Dc (or D/Dred)                               [-]                      [-]
c
c    Limits
c    -----------------------------------------------------------------------------------------------------------
c    TMIN       Minimum temperature of the EOS                 [K]                      [K]
c    TMAX       Maximum temperature of the EOS                 [K]                      [K]
c    DMAX       Maximum density of the EOS                     [mol/dm^3]               [kg/m^3]
c    PMAX       Maximum pressure of the EOS                    [kPa]                    [kPa]
c
c    Transport, etc.
c    -----------------------------------------------------------------------------------------------------------
c    VIS        Viscosity                                      [uPa*s]                  [uPa*s]
c    TCX        Thermal conductivity                           [W/(m*K)]                [W/(m*K)]
c    PRANDTL    Prandlt number                                 [-]                      [-]
c    TD         Thermal diffusivity                            [cm^2/s]                 [cm^2/s]
c    KV         Kinematic Viscosity                            [cm^2/s]                 [cm^2/s]
c    STN        Surface tension                                [mN/m]                   [mN/m]
c    DE         Dielectric constant                            [-]                      [-]
c
c
c    Heating values
c    -----------------------------------------------------------------------------------------------------------
c    SPHT       Specific heat input                            [J/mol]                  [kJ/kg]
c    HFRM       Heat of formation                              [J/mol]                  [kJ/kg]
c    HG         Gross (or superior) heating value              [J/mol]                  [kJ/kg]
c    HN         Net (or inferior) heating value                [J/mol]                  [kJ/kg]
c    HGLQ       Gross heating value (liquid)                   [J/mol]                  [kJ/kg]
c    HNLQ       Net heating value (liquid)                     [J/mol]                  [kJ/kg]
c    HGVOL      Gross HV (ideal gas volume basis)              [MJ/m^3]                 [MJ/m^3]
c    HNVOL      Net HV (ideal gas volume basis)                [MJ/m^3]                 [MJ/m^3]
c    HEATVAPZ   Heat of vaporization (for pure fluids)         [J/mol]                  [kJ/kg]
c    HEATVAPZ_T ...at constant temperature (for mixtures)      [J/mol]                  [kJ/kg]
c    HEATVAPZ_P ...at constant pressure (for mixtures)         [J/mol]                  [kJ/kg]
c    HEATCOMB   Heat of combustion at 25 C and 1 atm           [J/mol]                  [kJ/kg]
c
c    Other properties
c    -----------------------------------------------------------------------------------------------------------
c    PINT       Internal pressure                              [kPa]                    [kPa]
c    PREP       Repulsive part of pressure                     [kPa]                    [kPa]
c    PATT       Attractive part of pressure                    [kPa]                    [kPa]
c    EXERGY     Flow Exergy                                    [J/mol]                  [kJ/kg]
c    CEXERGY    Closed System Exergy                           [J/mol]                  [kJ/kg]
c    CSTAR      Critical flow factor                           [-]                      [-]
c    TMF        Throat mass flux                               [kg/(m^2*s)]             [kg/(m^2*s)]
c    FPV        Supercompressibility                           [-]                      [-]
c    SUMFACT    Summation Factor                               [-]                      [-]
c    RDAIR      Relative Density in air (specific gravity)     [-]                      [-]
c    RDH2O      Relative Density in water (specific gravity)   [-]                      [-]
c    API        API Gravity                                    [-]                      [-]
c
c    Fluid fixed points for mixtures
c    -----------------------------------------------------------------------------------------------------------
c    At the "true" critical point of the EOS dP/dD=0 and d^P/dD^2=0 at constant temperature
c    -----------------------------------------------------------------------------------------------------------
c    TCRIT      Critical temperature of component i            [K]                      [K]
c    PCRIT      Critical pressure of component i               [kPa]                    [kPa]
c    DCRIT      Critical density of component i                [mol/dm^3]               [kg/m^3]
c    TCTRUE     True EOS critical temp. of component i         [K]                      [K]
c    DCTRUE     True EOS critical density of component i       [mol/dm^3]               [kg/m^3]
c    TTRP       Triple point temperature of component i        [K]                      [K]
c    PTRP       Triple point pressure of component i           [kPa]                    [kPa]
c    DTRP       Triple point density of component i            [mol/dm^3]               [kg/m^3]
c    TNBP       Normal boiling point temp. of comp. i          [K]                      [K]
c    REOS       Gas constant of component i for EOS            [J/(mol*K)]              [(kJ/kg)/K]
c    MM         Molar mass of component i                      [g/mol]                  [g/mol]
c    ACF        Acentric factor of component i                 [-]                      [-]
c    DIPOLE     Dipole moment of component i                   [debye]                  [debye]
c    TREF       Ref. state temperature of component i          [K]                      [K]
c    DREF       Ref. state pressure of component i             [kPa]                    [kPa]
c    HREF       Ref. state enthalpy of comp. i at T0 and P0    [J/mol]                  [kJ/kg]
c    SREF       Ref. state entropy of comp. i at T0 and P0     [J/(mol*K)]              [(kJ/kg)/K]
c
c    Transport properties as a function of component number
c    -----------------------------------------------------------------------------------------------------------
c    Viscosity=ETA0+ETAB2+ETAR+ETAC
c    -----------------------------------------------------------------------------------------------------------
c    Thermal conductivity=TCX0+TCXR+TCXC
c    -----------------------------------------------------------------------------------------------------------
c    ETA0       Dilute gas viscosity of component i            [uPa*s]                  [uPa*s]
c    ETAB2      2nd virial viscosity of component i            [uPa*s]                  [uPa*s]
c    ETAR       Residual viscosity of component i              [uPa*s]                  [uPa*s]
c    ETAC       Viscosity critical enhance. of comp. i         [uPa*s]                  [uPa*s]
c    TCX0       Dilute gas thermal cond. of comp. i            [W/(m*K)]                [W/(m*K)]
c    TCXR       Residual (background) cond. of comp. i         [W/(m*K)]                [W/(m*K)]
c    TCXC       Cond. crit. enhancement of comp. i             [W/(m*K)]                [W/(m*K)]
c
c    Mixture properties as a function of component number
c    -----------------------------------------------------------------------------------------------------------
c    K          K value (y/x) (not implemented, y unknown)     [-]                      [-]
c    F          Fugacities                                     [kPa]                    [kPa]
c    FC         Fugacity coefficients                          [-]                      [-]
c    CPOT       Chemical potentials                            [J/mol]                  [kJ/kg]
c    DADN       n*partial(alphar)/partial(ni)                  [-]                      [-]
c    DNADN      partial(n*alphar)/partial(ni)                  [-]                      [-]
c    XMOLE      Composition on a mole basis                    [-]                      [-]
c    XMASS      Composition on a mass basis                    [-]                      [-]
c    FIJMIX     Binary parameters (see REFPROP)
c    ========== ============================================== =======================  ========================
c
c    The dimension statements for these variables are (in Fortran)::
c
c        parameter (ncmax=20)      ! Maximum number of components in the mixture
c        parameter (iPropMax=200)  ! Number of output properties available in ALLPROPS.
c        character*10000 hOut      ! hOut can actually be of any length.
c        character herr*255,hUnitsArray(iPropMax)*50
c        integer ierr,iUnits,iMass,iFlag,iUCodeArray(iPropMax) ! Note: as integer*4
c        double precision T,D,z(ncmax),Output(iPropMax)
c
c    Inputs:
c       hOut--Input string of properties to calculate.
c             Inputs can be separated by spaces, commas, semicolons,
c               or bars, but should not be mixed.  For example, a proper
c               string would be hOut='T,P,D,H,E,S', whereas an improperly
c               defined string would be hOut='T,P;D H|E,S'.
c             Use of lower or upper case is not important.
c             Some properties will return multiple values, for example,
c               hOut='F,Fc,XMOLE' will return 12 properties for a four
c               component system, these being F(1), F(2), F(3),
c               F(4), Fc(1), Fc(2), etc.
c             To retrieve the property of a single component, use,
c               for example, hOut='XMOLE(2),XMOLE(3)'.
c     iUnits--See subroutine REFPROP for a complete description of the
c               iUnits input value.
c             A negative value for iUnits indicates that the input
c               temperature is given in K and density in mol/dm^3,
c               (Refprop default units), otherwise T and D will be
c               converted first to K and mol/dm^3.  Do not use the
c               negative value for the iUnits parameter everywhere,
c               only in this one situation.
c      iMass--Specifies if the input composition is mole or mass based:
c               0 - Input compositions given in mole fractions
c               1 - Input compositions given in mass fractions
c      iFlag--Turn on or off writing of labels and units to hUnitsArray (eventually may be multiple flags combined into one variable, similar to ABFLSH):
c               0 - Do not write anything to the hUnitsArray array, thus
c                   increasing the calculation speed.  (String handling
c                   in Fortran is very computationally expensive.)
c               1 - Write labels and units to the hUnitsArray array.
c               2 - Return only the string number described under
c                   "iUCodeArray" below and the units.
c                   (No properties will be calculated.)
c              -1 - Write labels and units for only the first item.
c          T--Temperature, with units based on the value of iUnits.
c          D--Density, with units based on the value of iUnits.
c          z--Composition on a mole or mass basis (array of size ncmax=20)
c
c    Outputs:
c     Output--Array of properties that were specified in the hOut string
c               (array of size 200 dimensioned as double precision).
c               The number -9999970 will be returned when errors occur
c               or no input was requested.
c     hUnitsArray--Array with the label names from the hOut string followed by
c               the units of the property that are in the Output array.
c               (character array of size 200 with 50 characters each)
c               Will also contain error messages when necessary.
c               The array will be empty if iFlag=0.
c     iUCodeArray--Array (of size 200) with the values of iUCode(n) described
c               in the REFPROP subroutine.
c       ierr--Error flag:  0 - Successful
c       herr--Error string (character*255)
c
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-22-17 EWL, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'

      character*255 herr,herr1,ha,hb,hOut*(*)
      character*255 hFmix,hbinp,hmxrul,hSwap
      character*50 hUnitsArray(iPropMax),LTRIM*255
      character*10000 UCASE10000,hin,hmodij*3,hfij(nmxpar)*8
      dimension Output(iPropMax),iOut(iPropMax),iUCodeArray(iPropMax)
      dimension zx(ncmax),z(ncmax),fij(nmxpar)
      common /ALLPRPQ/ qsv,qmolesv,qmasssv,P2psv,e2psv,h2psv,s2psv,
     &                 iSwap,hSwap
      common /ALLPRPX/ iEnumA(iPropMax),iCompA(iPropMax),
     &                 iDlDv(iPropMax),nProp,iAllWarn
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ALLPROPS

      n=0
      herr=' '
      herr1=' '
      ierr=0
      ierr1=0
      ncc=nc
      ip2=0
      iOut=0
      iSwap=0
      if (nc.eq.0) ncc=1
      iFlag2=ABS(iFlag)
      if (iFlag.eq.-1) iFlag2=0                       !Check for writing units and labels only.
      iRefExit=0
      Tx=T
      Dx=D
      zx(1:ncc)=z(1:ncc)
      ha=' '
      if (iMass.ge.1) call XMOLE (z,zx,wmix)          !Convert compositions from a mole to a mass basis depending on the value of iMass.
      if (iUnits.gt.1) then
        Tx=CONVUNIT(T,-iUnits,1,zx,ha,ierr1,herr1)    !Convert temperature to K
        Dx=CONVUNIT(D,-iUnits,3,zx,ha,ierr1,herr1)    !Convert density to mol/dm^3
      endif
      iAllWarn=0
      if (iPass.le.1) then
        iAllWarn=0           !Flag to specify at the end to send back a warning.
        nProp=0              !Total number of properties, which may exceed the number of strings sent in the variable hOut.
        iUCodeArray=0        !Reset arrays, but only if this is the first pass through.
        Output=xnotc
        hIn=UCASE10000(hOut)
      endif

c -----Start of main loop, which loops more than once for mixtures that have component dependent inputs-----
      do
      ic=0         !Component index for when multiple component properties are being sent back.
      icl=xnotc    !Component index for when a single component property is being sent back (for example, Tcrit(3) is sent in).
      iccnt=0      !Component counter, similar to ic for multiple properties, used as a flag for a single property (to exit the loop).
      n=n+1        !Property counter
      if (iPass.ne.1 .and. n.gt.nProp .and. nProp.ne.0) EXIT         !Exit the loop when all requested properties have been calculated.
      if (n.gt.iPropMax) EXIT          !Check if current property number is more than available array space.

c  Convert the string sent to the routine to integer enumerated values (which are specified in the CONST.INC and COMMONS.INC files).
      if (iPass.le.1) then             !Only do this for either the first pass through this, or when only one pass is made.
        if (hIn.eq.' ') EXIT           !Exit when all strings in the input array have been processed.
        i=INDEX(hIn,',')               !Labels can be separated by commas, semicolons, bars, or spaces.  Do not use a mixed set of separators.
        if (i.eq.0) i=INDEX(hIn,';')
        if (i.eq.0) i=INDEX(hIn,'|')
        if (i.eq.0) i=INDEX(hIn,' ')
        if (i.gt.0) i=i-1
        ha=hIn(1:i)          !Pull out the next property.
        hIn=hIn(i+2:)        !Remove that input from the full string sent in.
        iCompA(n)=0

c  Search for labels with array indices and extract the number [e.g., "XMOLE(3)"].
        j=INDEX(ha,'(')
        if (j.gt.1) then
          k=INDEX(ha,')')
          if (k.gt.j+1) then
            i=j-1
            read (ha(j+1:k-1),*,end=1,err=1) icl           !Read the number out of the label.
  1         ha=ha(:i)                                      !Remove the number and the parentheses.
            if (icl.lt.0 .or. icl.gt.nq) then              !Set the icl value when out of range so that the array slot is left blank.
              icl=xnotd
              call ERRNUM (-804,0,' ',' ',0d0,0d0,0d0,ierr,herr)     !Warning that component number is out of range.
            endif
          endif
        endif

c  Check if the input string has "LIQ" or "VAP" in it to indicate a saturation state.
        iDlDv(n)=0
        if (i.ge.4) then
          if (ha(i-2:i).eq.'LIQ') iDlDv(n:n+ncc)=1         !Flag to indicate the need to come back and calculate saturated liquid and vapor properties with a different composition.
          if (ha(i-2:i).eq.'VAP') iDlDv(n:n+ncc)=2         !  The current value is set to 1 or 2, plus additional ones in case composition is requested.  Otherwise these will be overwritten.
          if (iDlDv(n).gt.0) ha=ha(:i-3)         !Remove the three letters from the string.
        endif

        call GETENUM (2,ha,ip,ierr1,herr1)       !Get the enumerated values for properties that are a function of T and D.
        if (icl.eq.xnotd) ip=0                   !Don't compute anything if the component number in the label is out of range.
        n1=n+ncc-1
        if (n1.gt.200) n1=200
        iEnumA(n:n1)=ip                          !Add the enumerated value to the iEnumA array, plus enough to cover all nc components for properties that are component dependent.
        if ((ip.eq.0 .or. ip.eq.xerr) .and. icl.ne.xnotd) then
          call GETENUM (3,ha,ip,ierr1,herr1)     !Repeat for properties that are fixed values for the fluid or mixture.
          if (icl.eq.-1) ip=0
          if (ip.ne.0) iEnumA(n:n+ncc-1)=-ip     !Use a negative value to indicate that the above sentence is true.
          if (ip.ne.0 .and. iDlDv(n).ne.0) iEnumA(n:n+ncc-1)=ip      !Reset above if input is a saturated or 2-phase state (for example, xmass vs. xmassliq, where the latter requires calculation of the input state)/
        endif
        if (ip.eq.0 .and. ierr1.gt.0) goto 200
      endif

c  Check for saturated liquid or vapor states.
      ip2=iEnumA(n)
      if (iPass.ne.1) then
        i=1
        if(iDlDv(n).eq.1.and.iDlDvFlg.eq.2.and.qsv.ge.0.and.qsv.le.1)i=0       !Check if liquid was requested, but state is vapor, or vice-versa.
        if(iDlDv(n).eq.2.and.iDlDvFlg.eq.1.and.qsv.ge.0.and.qsv.le.1)i=0
        if(iDlDv(n).eq.0.and.iDlDvFlg.ne.0) i=0                                !Check for single phase states but the requested input is saturated or 2-phase.
        if(iDlDv(n).ne.0.and.iDlDvFlg.eq.0) i=0
        if(i.eq.0 .and. ABS(ip2).gt.iBlockVAR .and. icl.le.0)n=n+ncc-1       !Skip forward for component dependent properties when one of the above items is true.
        if(iFlag2.ne.2 .and. i.eq.0) ip2=0
      endif

c -----Start of inner loop, where looping is only needed for situations where multiple properties are sent back for a single entry, such as XLIQ-----
 50   ip=ip2
      if (ABS(ip).eq.ABS(xerr)) ip=0
      iUCodeArray(n)=iUnitTag(ABS(ip))           !Set up the unit number.
      if (iPass.eq.1 .and. ip.gt.0) ip=0         !For first pass through, only do negative values of ip (fixed points that are not dependent on T and D).
      if (iPass.ge.2 .and. ip.lt.0) ip=0         !For second pass through, only do positive values of ip.  If iPass=0 (one pass only), do nothing.
      iOut(n)=ip             !Flag to tell the ALLPROPS0 routine which properties to calculate.  For multiple passes, it is set to zero except for those to be calculated on the specified pass.
      ip=ABS(ip)

c  Check for component dependent outputs, which are identified by the enumerated value being larger than iBlockVAR.
      if (iPass.le.1) then
        hUnitsArray(n)=ha              !Save the property name.
        if (ABS(ip2).gt.iBlockVAR .and. ABS(ip2).lt.100000) then
          if (icl.ge.0) then           !Only one pass is needed for inputs such as Tcrit(3).
            ic=icl                     !Set the component index in use in this pass.
            icl=xnotd                  !Say goodbye to icl.
            iccnt=ncc+1                !Set the counter to more than the number of components to include the single pass.
          else                         !Multiple passes through for inputs such as Tcrit.   !icl will be negative for compositions out of range.
            if (ncc.gt.1) iAllWarn=1   !Flag to specify at the end to send back a warning.
            iccnt=iccnt+1              !Counter for the component number to use in this pass.
            ic=iccnt                   !Useless variable since we already have a counter.
          endif
          iCompA(n)=ic
        endif
      else
        ic=iCompA(n)                   !Retrieve the component number.
      endif

      if (ip.eq.0) goto 200
      iOut(n)=iEnumA(n)+10000*ic       !Combine the property index and the component index into one variable for use on multiple passes (which is the property enumerated value plus 10000 times the component number ic).
      if (iFlag2.eq.2) goto 200        !Exit if only property names and units are requested.

c  2-phase properties
      if (ip.eq.iprpQMOLE .or. ip.eq.iprpQMASS) then
        if ((qsv.gt.1. .or. qsv.lt.0.) .and. herr1.eq.' ') then
          if (nprop.eq.1) ierr1=-998
          herr1='State is single phase, quality not calculated'   !Send back a simple message if no other text is in the herr variable.
        endif
      endif
      if (l2phase) then
        if (qsv.gt.0 .and. qsv.lt.1) then      !Check if the state point is 2-phase (the quality is >0 and <1) or saturated (quality equal to zero[liquid] or one[vapor]).
          if (iDlDvFlg.eq.0 .and. ip.ne.iprpX) then        !iprpX indicates that the composition was requested.
            if (ip.ne.iprpXMOLE .and. ip.ne.iprpXMASS) Output(n)=xnotc         !Reset calculated properties if the state is two-phase, and only return properties that are thermodynamically defined in the two-phase.
          endif
          if (ip.eq.iprpT)      Output(n)=Tx
          if (ip.eq.iprpP)      Output(n)=p2psv
          if (ip.eq.iprpD)      Output(n)=Dx
          if (ip.eq.iprpV)      Output(n)=XDIV3(1d0,Dx)
          if (ip.eq.iprpE)      Output(n)=e2psv
          if (ip.eq.iprpH)      Output(n)=h2psv
          if (ip.eq.iprpS)      Output(n)=s2psv
          iOut(n)=0
        endif
        if (qsv.ge.0. .and. qsv.le.1.) then
          if (ip.eq.iprpQMOLE)  Output(n)=qmolesv
          if (ip.eq.iprpQMASS)  Output(n)=qmasssv
          if (ip.eq.iprpQMOLE)  iOut(n)=0
          if (ip.eq.iprpQMASS)  iOut(n)=0
        endif
        if (Output(n).eq.xnotc .and. qsv.gt.0 .and. qsv.lt.1) then
         if (ip.eq.iprpQ) then
           call ERRNUM(812,0,' ',' ',0d0,0d0,0d0,ierr,herr)
         elseif (ip.lt.iBlockOTHER .or. ip.gt.iBlockVAR) then
           if (nProp.eq.1) then
             call ERRNUM(810,0,'error 810',' ',0d0,0d0,0d0,ierr1,herr1)
           else
             call ERRNUM(-810,0,'warning -810',' ',
     &                   0d0,0d0,0d0,ierr1,herr1)
           endif
         endif
        endif
      endif

c  Check for string requests that cannot be retrieved from ALLPROPS0
      if (ip.gt.iBlockCOMP .and. ip.lt.iBlockSTRINGS) then
        ij=ic
        if (ncc.eq.1) ij=1
        Output(n)=xFlg       !Use as a flag to the routine calling the DLL that the only output is a string.
        if (ij.gt.ncc) goto 804
        if (ip.eq.iprpNAME)         hUnitsArray(n)=hname(ij)
        if (ip.eq.iprpLONGNAME)     hUnitsArray(n)=hnam80(ij)
        if (ip.eq.iprpSYNONYM)      hUnitsArray(n)=hsyn(ij)
        if (ip.eq.iprpFAMILY)       hUnitsArray(n)=hfamily(ij)
        if (ip.eq.iprpCAS)          hUnitsArray(n)=hcas(ij)
        if (ip.eq.iprpHASH)         hUnitsArray(n)=hash(ij)
        if (ip.eq.iprpALTID)        hUnitsArray(n)=hAltID(ij)
        if (ip.eq.iprpSAFETY)       hUnitsArray(n)=hSafety(ij)
        if (ip.eq.iprpUNNUMBER)     hUnitsArray(n)=hUNNumb(ij)
        if (ip.eq.iprpREFSTATE)     hUnitsArray(n)=hrefdef(ij)
        if (ip.eq.iprpINCHIKEY)     hUnitsArray(n)=hInChiKey(ij)
        if (ip.eq.iprpCHEMFORM)     hUnitsArray(n)=hChemF2(ij)
        if (ip.eq.iprpFULLCHEMFORM) hUnitsArray(n)=hChemF1(ij)
        if (ip.eq.iprpFLDNAME)      hUnitsArray(n)=hFldFileName(ij)
        if (ip.ge.iprp_EOS .and. ip.le.iBlockStrings) then          !Special flags such as DOI and WEB
          if (ip.lt.iprp_EOS+iprpFlags) then
            iSwap=n
            hSwap=hDOI(ij,ip-iprp_EOS+1)
            hUnitsArray(n)=hSwap
            if (herr.eq.' ') herr=hSwap
          elseif (ip.lt.iprp_EOS+iprpFlags*2) then
            iSwap=n
            hSwap=hWEB(ij,ip-iprp_EOS-iprpFlags+1)
            hUnitsArray(n)=hSwap
            if (herr.eq.' ') herr=hSwap           !Save in herr as well.
          endif
        elseif (ip.eq.iprpFDIR) then
          iSwap=n
          hSwap=hDir(ij)
          hUnitsArray(n)=hSwap                    !Send back the directory in both hUnitsArray and herr (because the length of hUnitsArray
          if (herr.eq.' ') herr=hSwap             ! is 50 characters long), but if an error occurred, herr will be overwritten.
        elseif (ip.eq.iprpINCHI) then
          iSwap=n
          hSwap=hInChi(ij)
          hUnitsArray(n)=hSwap
          if (herr.eq.' ') herr=hSwap
        elseif (ip.eq.iprpFIJMIX) then            !Fij and mixing parameters
          i=INT(T)
          j=INT(D)
          if (i.le.0 .or. i.gt.ncc .or. j.le.0 .or. j.gt.ncc) goto 804
          call GETKTV(i,j,hmodij,fij,hFmix,hfij,hbinp,hmxrul)
          Output(1:nmxpar)=fij(1:nmxpar)
          hUnitsArray(n)=hmodij
        endif
      endif

 200  if (ierr1.ne.0) then
        call SWAPERR (ierr,ierr1,herr,herr1)
        if ((ierr.lt.0 .and. hUnitsArray(n).eq.' ') .or. ierr.gt.0) then
          call ErrMsg(ierr,herr)       !Put the error message into hUnitsArray if empty or if ierr>0
          i=INDEX(herr,']')
          hUnitsArray(n)=LTRIM(herr(i+1:))       !Remove the part up to the character "]" because the length is only 50 characters long.
        endif
      endif
      if (iccnt.gt.0 .and. iccnt.lt.ncc .and. iPass.le.1) then       !Check if a loop is needed when multiple component properties were requested.
        n=n+1
        if (n.gt.iPropMax) EXIT        !More inputs were sent than available to store in the output array.
        goto 50
      endif
      enddo
      if (iPass.le.1) nProp=n-1
      i=SUM(ABS(iOut(1:nProp)))

      if (iFlag2.ne.2 .and. i.ne.0) then
c----------------------------------------------------------------------
c  Calculate all single-phase properties.

      call ALLPROPS0 (nProp,iOut,iFlag,Tx,Dx,zx,Output,ierr1,herr1)

c----------------------------------------------------------------------
      call SWAPERR (ierr,ierr1,herr,herr1)
      endif

c  Convert to the requested units.
      iCheck=0
      do k=1,nProp
        if (iPass.eq.1 .and. iEnumA(k).gt.0) iCheck=1      !Check if more properties need to be calculated on subsequent passes.
        iE=ABS(iEnumA(k))
        if (iE.lt.iBlockComp) then                         !Skip this for inputs where a string was returned.
          l2p=.false.
          if (l2Phase .and. iE.lt.iBlockPHIR) then
            if (iE.ne.iprpM .and. iE.ne.iprpR) l2p=.true.  !Check for properties that can be calculated if a state is 2-phase (excluding M and R, which were calculated on the first pass through).
          endif
          if (iOut(k).ne.0 .or. l2P) then                  !Add the units if a property was calculated.
            iStr=iFlag2
            if (k.eq.1 .and. iFlag.ne.0) iStr=1
            hb=hUnitsArray(k)
            call GETUNITS (ABS(iUnits),iUCodeArray(k),     !Convert the property from SI to the requested units, and get the unit string.
     &      iStr,ic,zx,hb,(Output(k)),xOut,ha,ierr1,herr1) !Do not remove the extra parenthesis around (Output(n)).
            Output(k)=xOut                                 !Don't put this variable on the previous line (causes threaded processes to fail).
            hUnitsArray(k)=ha
          endif
        endif
      enddo
      if (iCheck.eq.0) iRefExit=1
      if (nProp.gt.iPropMax) goto 851
      if (iAllWarn.ne.0) goto 852      !If inputs that require multiple outputs were sent, send back a warning about this.

c  Loop is finished, check for errors and exit if requested.
 600  call SWAPERR (ierr,ierr1,herr,herr1)
      ipass=0
      RETURN
 851  call ERRNUM (-851,0,' ',' ',0d0,0d0,0d0,ierr1,herr1)           !Warning that too many properties were requested.
      goto 600
 852  call ERRNUM (-852,0,' ',' ',0d0,0d0,0d0,ierr1,herr1)           !Warning about more outputs than inputs
      goto 600
 853  if (ip.ne.0) call ERRNUM (853,0,ha,' ',0d0,0d0,0d0,ierr1,herr1)!Error that the input string was not recognized.
      goto 600
 804  call ERRNUM (804,0,' ',' ',0d0,0d0,0d0,ierr1,herr1)            !Error that T and/or D are out of bounds.
      goto 600

      end                                            subroutine ALLPROPS
c
c ======================================================================
c
      subroutine ALLPROPS0 (iIn,iOut,iFlag,T,D,z,Output,ierr,herr)

c  Calculate any single phase property defined in the iOut array and
c  return the values in the Output array.  This routine should NOT
c  be called for two-phase states!
c
c  The output array is not reset so that several passes can be made to
c  fill in holes left by the previous pass (such as entries at different
c  T, D, or z).  The caller can zero out this array if so desired.
c
c  This routine is designed with the "superuser" in mind.  It removes all
c  string comparisons to approach the speed that could be obtained by
c  calling the dedicated functions (such as THERM), but making it easy
c  by allowing all inputs to be calculated with one routine.  Since the
c  units are not returned here, look in the ALLPROPS documentation under
c  the molar column.
c
c  These values of iOut are defined in the COMMONS.INC file
c  and are obtained by a call to GETENUM, as such for the enthalpy::
c
c      call GETENUM (0,'H',iEnum,ierr,herr)
c
c  To obtain the pure fluid value for some of the inputs,
c  add 10000*ic (where ic is the component number) to the value
c  of the enumerated value.  The properties that can be used for this are
c  given the bottom of the comments section in the ALLPROPS routine.
c
c
c  Inputs:
c      iIn--Number of properties to calculate.
c     iOut--Array of enumerated values that identify the property to be
c             calculated (see above)
c    iFlag--Not yet used.
c        T--Temperature [K]
c        D--Density [mol/L]
c        z--Overall composition (array of mole fractions)
c
c  Outputs:
c   Output--Values of the calculated properties.
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)

c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-18-18 EWL, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character*255 herr,herr1
      dimension iOut(iPropMax),Output(iPropMax)
      dimension x(ncmax),y(ncmax),z(ncmax),zkg(ncmax)
      dimension phi(ncmax),dadn(ncmax),dnadn(ncmax)

ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ALLPROPS0

      ierr1=iFlag            !This line keeps the compiler happy about unused variables.
      ierr1=0
      lcrt=.true.
      ldhd1=.true.
      ldsd1=.true.
      lheat=.true.
      lredx=.true.
      lcstar=.true.
      lcv2pk=.true.
      lexcess=.true.
      lfgcty2=.true.
      lfugcof=.true.
      lcrthmx=.true.
      ltherm0=.true.
      ltherm3=.true.
      ltrnprp=.true.
      llimits=.true.
      lchempot=.true.
      ldervpvt=.true.
      ldptsatk=.true.
      lphiderv=.true.
      lresidual=.true.
      lvirbcd12=.true.

c  Basic calculations
      if (iIn.gt.0) then     !The flag iIn<0 indicates that no properties as a function of T and D are required [but ABS(iIn) is still the number of properties to be calculated].
        call THERM (T,D,z,P,e,h,s,Cv,Cp,w,hjt)
        Ds=D
        if (ABS(D).lt.1.d-12) Ds=1.d-12          !Some properties will fail (divide by zero) if D=0.
      endif
      call ISPURE (z,icomp)
      call RMIX (z)
      wm=WMOL(z)

      do n=1,ABS(iIn)
      if (n.gt.iPropMax) EXIT
      ic=1
      ip=ABS(iOut(n))
      if (ip.gt.5000) then   !The flag can be less than or greater than 10000 depending on the pass number (i.e., first pass through with Tcrit: ip=10000-302, where 302 represents Dcrit)
        ic=INT((iOut(n)+5000)/10000)
        ip=ABS(iOut(n)-ic*10000)
      endif
      ierr=0
      if (ip.le.0) then

c  Regular properties
      elseif (ip.lt.iBlockReg) then
        if (ABS(D*T).gt.1.d-16) Zz=P/(D*T*Rxgas)
        if (ip.eq.iprpT)      Output(n)=T                       !Temperature                                    [K]                      [K]
        if (ip.eq.iprpP)      Output(n)=P                       !Pressure                                       [kPa]                    [kPa]
        if (ip.eq.iprpD)      Output(n)=D                       !Density                                        [mol/dm^3]               [kg/m^3]
        if (ip.eq.iprpV)      Output(n)=1d0/Ds                  !Volume                                         [dm^3/mol]               [m^3/kg]
        if (ip.eq.iprpE)      Output(n)=e                       !Internal energy                                [J/mol]                  [kJ/kg]
        if (ip.eq.iprpH)      Output(n)=h                       !Enthalpy                                       [J/mol]                  [kJ/kg]
        if (ip.eq.iprpS)      Output(n)=s                       !Entropy                                        [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCV)     Output(n)=Cv                      !Isochoric heat capacity                        [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCP)     Output(n)=Cp                      !Isobaric heat capacity                         [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCPCV)   Output(n)=XDIV3(Cp,Cv)            !Heat capacity ratio                            [-]                      [-]
        if (ip.eq.iprpW)      Output(n)=w                       !Speed of sound                                 [m/s]                    [m/s]
        if (ip.eq.iprpZ)      Output(n)=P/(Ds*T*Rxgas)          !Compressibility factor (Z=p*v/(RT))            [-]                      [-]
        if (ip.eq.iprpJT)     Output(n)=hjt                     !Isenthalpic Joule-Thomson coefficient          [K/kPa]                  [K/kPa]
        if (ip.eq.iprpR)      Output(n)=Rxgas                   !Gas constant                                   [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpM)      Output(n)=wm                      !Molar mass (of the mixture)                    [g/mol]                  [g/mol]
        if (ip.eq.iprpQMOLE)  Output(n)=xnotc                   !Quality (can't calculate here, send back error number) [mol/mol]        [kg/kg]
        if (ip.eq.iprpQMASS)  Output(n)=xnotc                   !Quality (can't calculate here, send back error number) [mol/mol]        [kg/kg]
        if (ip.eq.iprpA .or. ip.eq.iprpG) then
          call AG (T,D,z,a,g)
          if (ip.eq.iprpA)    Output(n)=a                       !Helmholtz energy                               [J/mol]                  [kJ/kg]
          if (ip.eq.iprpG)    Output(n)=g                       !Gibbs energy                                   [J/mol]                  [kJ/kg]
        endif
        if (ip.eq.iprpNCOMP)  Output(n)=DBLE(nc)
        if (ip.eq.iprpQ)call ERRNUM(812,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        if (ip.eq.iprpQMOLE .or. ip.eq.iprpQMASS) then
          if (herr.eq.' ') herr='Quality not available for calculation'
        endif

c  Irregular properties
      elseif (ip.lt.iBlockIrreg) then
        if (ltherm3) call THERM3 (T,D,z,xkappa,beta,xisenk,xkt,
     &               betas,bs,xkkt,thrott,pi,spht)
        ltherm3=.false.
        if (ip.eq.iprpKAPPA)  Output(n)=xkappa                  !Isothermal compressibility                     [1/kPa]                  [1/kPa]
        if (ip.eq.iprpBETA)   Output(n)=beta                    !Volume expansivity                             [1/K]                    [1/K]
        if (ip.eq.iprpISENK)  Output(n)=xisenk                  !Isentropic expansion coefficient               [-]                      [-]
        if (ip.eq.iprpKT)     Output(n)=xkt                     !Isothermal expansion coefficient               [-]                      [-]
        if (ip.eq.iprpBETAS)  Output(n)=betas                   !Adiabatic compressibility                      [1/kPa]                  [1/kPa]
        if (ip.eq.iprpBS)     Output(n)=bs                      !Adiabatic bulk modulus                         [kPa]                    [kPa]
        if (ip.eq.iprpKKT)    Output(n)=xkkt                    !Isothermal bulk modulus                        [kPa]                    [kPa]
        if (ip.eq.iprpTHROTT) Output(n)=thrott                  !Isothermal throttling coefficient              [dm^3/mol]               [m^3/kg]
        if (ip.eq.iprpSPHT)   Output(n)=spht                    !Specific heat input                            [J/mol]                  [kJ/kg]

c  Derivatives
      elseif (ip.lt.iBlockDeriv) then
        if (ldervpvt) call DERVPVT(T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
        ldervpvt=.false.
        if (ip.eq.iprpDPDD)   Output(n)=dPdD                    !dP/dD at constant T                            [(dm^3/mol)*kPa]         [(m^3/kg)*kPa]
        if (ip.eq.iprpDPDT)   Output(n)=dPdT                    !dP/dT at constant D                            [kPa/K]                  [kPa/K]
        if (ip.eq.iprpDDDP)   Output(n)=dDdP                    !dD/dP at constant T                            [(mol/dm^3)/kPa]         [(kg/m^3)/kPa]
        if (ip.eq.iprpDDDT)   Output(n)=dDdT                    !dD/dT at constant P                            [(mol/dm^3)/K]           [(kg/m^3)/K]
        if (ip.eq.iprpDTDP)   Output(n)=dTdP                    !dT/dP at constant D                            [K/kPa]                  [K/kPa]
        if (ip.eq.iprpDTDD)   Output(n)=dTdD                    !dT/dD at constant P                            [(dm^3/mol)*K]           [(m^3/kg)*K]
        if (ip.eq.iprpD2PDD2) Output(n)=d2PdD2                  !d^2P/dD^2 at constant T                        [(dm^3/mol)^2*kPa]       [(m^3/kg)^2*kPa]
        if (ip.eq.iprpD2PDT2) Output(n)=d2PdT2                  !d^2P/dT^2 at constant D                        [kPa/K^2]                [kPa/K^2]
        if (ip.eq.iprpD2PDTD) Output(n)=d2PdTD                  !d^2P/dTdD                                      [(dm^3/mol)*kPa/K]       [(m^3/kg)*kPa/K]
        if (ip.eq.iprpD2DDP2) Output(n)=d2DdP2                  !d^2D/dP^2 at constant T                        [(mol/dm^3)/kPa^2]       [(kg/m^3)/kPa^2]
        if (ip.eq.iprpD2DDT2) Output(n)=d2DdT2                  !d^2D/dT^2 at constant P                        [(mol/dm^3)/K^2]         [(kg/m^3)/K^2]
        if (ip.eq.iprpD2DDPT) Output(n)=d2DdPT                  !d^2D/dPdT                                      [(mol/dm^3)/(kPa*K)]     [(kg/m^3)/[kPa*K]]
        if (ip.eq.iprpD2TDP2) Output(n)=d2TdP2                  !d^2T/dP^2 at constant D                        [K/kPa^2]                [K/kPa^2]
        if (ip.eq.iprpD2TDD2) Output(n)=d2TdD2                  !d^2T/dD^2 at constant P                        [(dm^3/mol)^2*K]         [(m^3/kg)^2*K]
        if (ip.eq.iprpD2TDPD) Output(n)=d2TdPD                  !d^2T/dPdD                                      [(dm^3/mol)*K/kPa]       [(m^3/kg)*K/kPa]

c  Enthalpy derivatives
      elseif (ip.lt.iBlockDHDx) then
        if (ldhd1) call DHD1 (T,D,z,dhdtd,dhdtp,dhddt,dhddp,dhdpt,dhdpd)
        ldhd1=.false.
        if (ip.eq.iprpDHDT_D) Output(n)=dHdTD                   !dH/dT at constant D                            [(J/mol)/K]              [(kJ/kg)/K]
        if (ip.eq.iprpDHDT_P) Output(n)=dHdTP                   !dH/dT at constant P                            [(J/mol)/K]              [(kJ/kg)/K]
        if (ip.eq.iprpDHDD_T) Output(n)=dHdDT                   !dH/dD at constant P                            [(J/mol)*(dm^3/mol)]     [(kJ/kg)*(m^3/kg)]
        if (ip.eq.iprpDHDD_P) Output(n)=dHdDP                   !dH/dD at constant T                            [(J/mol)*(dm^3/mol)]     [(kJ/kg)*(m^3/kg)]
        if (ip.eq.iprpDHDP_T) Output(n)=dHdPT                   !dH/dP at constant T                            [(J/mol)/kPa]            [(kJ/kg)/kPa]
        if (ip.eq.iprpDHDP_D) Output(n)=dHdPD                   !dH/dP at constant D                            [(J/mol)/kPa]            [(kJ/kg)/kPa]

c  Entropy derivatives
      elseif (ip.lt.iBlockDSDx) then
        if (ldsd1) call DSD1 (T,D,z,dsdtd,dsdtp,dsddt,dsddp,dsdpt,dsdpd)
        ldsd1=.false.
        if (ip.eq.iprpDSDT_D) Output(n)=dSdTD                   !dS/dT at constant D                            [(J/mol)/K^2]            [(kJ/kg)/K^2]
        if (ip.eq.iprpDSDT_P) Output(n)=dSdTP                   !dS/dT at constant P                            [(J/mol)/K^2]            [(kJ/kg)/K^2]
        if (ip.eq.iprpDSDD_T) Output(n)=dSdDT                   !dS/dD at constant T                            [(J/mol)*(dm^3/mol)/K]   [(kJ/kg)*(m^3/kg)/K]
        if (ip.eq.iprpDSDD_P) Output(n)=dSdDP                   !dS/dD at constant P                            [(J/mol)*(dm^3/mol)/K]   [(kJ/kg)*(m^3/kg)/K]
        if (ip.eq.iprpDSDP_T) Output(n)=dSdPT                   !dS/dP at constant T                            [(J/mol)/(kPa*K)]        [(kJ/kg)/[kPa*K]]
        if (ip.eq.iprpDSDP_D) Output(n)=dSdPD                   !dS/dP at constant D                            [(J/mol)/(kPa*K)]        [(kJ/kg)/[kPa*K]]

c  Virial Coefficients
      elseif (ip.lt.iBlockB) then
        if (lvirbcd12)
     &    call VIRBCD12 (T,z,o0,o1,o2,o3,o4,o5,o6,o7,o8,o9,Ba,Ca)
        lvirbcd12=.false.
        if (ip.eq.iprpBvir)   Output(n)=o0                      !Second virial coefficient                      [dm^3/mol]               [m^3/kg]
        if (ip.eq.iprpCvir)   Output(n)=o1                      !Third virial coefficient                       [(dm^3/mol)^2]           [(m^3/kg)^2]
        if (ip.eq.iprpDvir)   Output(n)=o2                      !Fourth virial coefficient                      [(dm^3/mol)^3]           [(m^3/kg)^3]
        if (ip.eq.iprpEvir)   Output(n)=o3                      !Fifth virial coefficient                       [(dm^3/mol)^4]           [(m^3/kg)^4]
        if (ip.eq.iprpdBvirdT)   Output(n)=o4                   !1st derivative of B with respect to T          [(dm^3/mol)/K]           [(m^3/kg)/K]
        if (ip.eq.iprpd2BvirdT2) Output(n)=o5                   !2nd derivative of B with respect to T          [(dm^3/mol)/K^2]         [(m^3/kg)/K^2]
        if (ip.eq.iprpdCvirdT)   Output(n)=o6                   !1st derivative of C with respect to T          [(dm^3/mol)^2/K]         [(m^3/kg)^2/K]
        if (ip.eq.iprpd2CvirdT2) Output(n)=o7                   !2nd derivative of C with respect to T          [(dm^3/mol)^2/K^2]       [(m^3/kg)^2/K^2]
        if (ip.eq.iprpdDvirdT)   Output(n)=o8                   !1st derivative of D with respect to T          [(dm^3/mol)^3/K]         [(m^3/kg)^3/K]
        if (ip.eq.iprpd2DvirdT2) Output(n)=o9                   !2nd derivative of D with respect to T          [(dm^3/mol)^3/K^2]       [(m^3/kg)^3/K^2]
        if (ip.eq.iprpBA)     Output(n)=Ba                      !Second acoustic virial coefficient             [dm^3/mol]               [m^3/kg]
        if (ip.eq.iprpCA)     Output(n)=Ca                      !Third acoustic virial coefficient              [(dm^3/mol)^2]           [(m^3/kg)^2]

c  Excess Properties
      elseif (ip.lt.iBlockE) then
        dd=0d0
        if (lexcess) call EXCESS(T,P,z,0,dd,vE,eE,hE,sE,aE,gE,ierr,herr)
        lexcess=.false.
        if (ip.eq.iprpVE)     Output(n)=vE                      !Excess volume                                  [dm^3/mol]               [m^3/kg]
        if (ip.eq.iprpEE)     Output(n)=eE                      !Excess energy                                  [J/mol]                  [kJ/kg]
        if (ip.eq.iprpHE)     Output(n)=hE                      !Excess enthalpy                                [J/mol]                  [kJ/kg]
        if (ip.eq.iprpSE)     Output(n)=sE                      !Excess entropy                                 [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpAE)     Output(n)=aE                      !Excess Helmholtz energy                        [J/mol]                  [kJ/kg]
        if (ip.eq.iprpGE)     Output(n)=gE                      !Excess Gibbs energy                            [J/mol]                  [kJ/kg]

c  Ideal gas properties
      elseif (ip.lt.iBlock0) then
        if (ltherm0) call THERM0 (T,D,z,P0,e0,h0,s0,Cv0,Cp00,w0,a0,g0)
        ltherm0=.false.
        if (ip.eq.iprpP0)     Output(n)=P0                      !Ideal gas pressure                             [kPa]                    [kPa]
c       if (ip.eq.iprpD0)     Output(n)=D0                      !Ideal gas density                              [mol/dm^3]               [kg/m^3]
        if (ip.eq.iprpE0)     Output(n)=e0                      !Ideal gas internal energy                      [J/mol]                  [kJ/kg]
        if (ip.eq.iprpH0)     Output(n)=h0                      !Ideal gas enthalpy                             [J/mol]                  [kJ/kg]
        if (ip.eq.iprpS0)     Output(n)=s0                      !Ideal gas entropy                              [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCV0)    Output(n)=Cv0                     !Ideal gas isochoric heat capacity              [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCP0)    Output(n)=Cp00                    !Ideal gas isobaric heat capacity               [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCP0CV0) Output(n)=XDIV3(Cp00,Cv0)         !Ideal gas heat capacity ratio                  [-]                      [-]
        if (ip.eq.iprpW0)     Output(n)=w0                      !Ideal gas speed of sound                       [m/s]                    [m/s]
        if (ip.eq.iprpA0)     Output(n)=a0                      !Ideal gas Helmholtz energy                     [J/mol]                  [kJ/kg]
        if (ip.eq.iprpG0)     Output(n)=g0                      !Ideal gas Gibbs energy                         [J/mol]                  [kJ/kg]
        if (ip.eq.iprpP_P0)   Output(n)=P-P0                    !Pressure minus ideal gas pressure              [kPa]                    [kPa]

c  Residual properties
      elseif (ip.lt.iBlockR) then
        if (lresidual) call RESIDUAL (T,D,z,Pr,er,hr,sr,Cvr,Cpr,ar,gr)
        lresidual=.false.
        if (ip.eq.iprpPR)     Output(n)=Pr                      !Residual pressure                              [kPa]  (P-D*Rxgas*T)     [kPa]
        if (ip.eq.iprpER)     Output(n)=er                      !Residual internal energy                       [J/mol]                  [kJ/kg]
        if (ip.eq.iprpHR)     Output(n)=hr                      !Residual enthalpy                              [J/mol]                  [kJ/kg]
        if (ip.eq.iprpSR)     Output(n)=sr                      !Residual entropy                               [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCVR)    Output(n)=Cvr                     !Residual isochoric heat capacity               [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpCPR)    Output(n)=Cpr                     !Residual isobaric heat capacity                [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpAR)     Output(n)=ar                      !Residual Helmholtz energy                      [J/mol]                  [kJ/kg]
        if (ip.eq.iprpGR)     Output(n)=gr                      !Residual Gibbs energy                          [J/mol]                  [kJ/kg]

c  Ideal-gas contributions to the Helmholtz energy
      elseif (ip.lt.iBlockPHI0) then
        if (ip.eq.iprpPHIG00) Output(n)=PHI0 (0,0,T,D,z)        !Ideal gas A/RT                                 [-]                      [-]
        if (ip.eq.iprpPHIG10) Output(n)=PHI0 (1,0,T,D,z)        !tau*[d(A0/RT)/d(tau)]                          [-]                      [-]
        if (ip.eq.iprpPHIG20) Output(n)=PHI0 (2,0,T,D,z)        !tau^2*[d^2(A0/RT)/d(tau)^2]                    [-]                      [-]
        if (ip.eq.iprpPHIG30) Output(n)=PHI0 (3,0,T,D,z)        !tau^3*[d^3(A0/RT)/d(tau)^3]                    [-]                      [-]
        if (ip.eq.iprpPHIG01) Output(n)=PHI0 (0,1,T,D,z)        !del*[d(A0/RT)/d(del)]                          [-]                      [-]
        if (ip.eq.iprpPHIG02) Output(n)=PHI0 (0,2,T,D,z)        !del^2*[d^2(A0/RT)/d(del)^2]                    [-]                      [-]
        if (ip.eq.iprpPHIG03) Output(n)=PHI0 (0,3,T,D,z)        !del^3*[d^3(A0/RT)/d(del)^3]                    [-]                      [-]
        if (ip.eq.iprpPHIG11) Output(n)=PHI0 (1,1,T,D,z)        !tau*del*[d^2(A0/RT)/d(tau)d(del)]              [-]                      [-]
        if (ip.eq.iprpPHIG12) Output(n)=PHI0 (1,2,T,D,z)        !tau*del^2*[d^3(A0/RT)/d(tau)d(del)^2]          [-]                      [-]
        if (ip.eq.iprpPHIG21) Output(n)=PHI0 (2,1,T,D,z)        !tau^2*del*[d^3(A0/RT)/d(tau)^2d(del)]          [-]                      [-]

c  Residual contributions to the Helmholtz energy               !Assume nomenclature of a02=[partial^2(alphar)/partial(del)^2]
      elseif (ip.lt.iBlockPHIR) then
        if (lredx) call REDX (z,Tred0,Dred0)
        lredx=.false.
        tau=XDIV(Tred0,T)
        del=XDIV(D,Dred0)
        if (ip.eq.iprpPHIR00) Output(n)=PHIX (0,0,tau,del,z)    !Reduced residual Helmholtz energy (A-A0)/RT    [-]                      [-]
        if (ip.eq.iprpPHIR10) Output(n)=PHIX (1,0,tau,del,z)    !tau  *[d  (Ar/RT)/d(tau)]                      [-]                      [-]
        if (ip.eq.iprpPHIR20) Output(n)=PHIX (2,0,tau,del,z)    !tau^2*[d^2(Ar/RT)/d(tau)^2]                    [-]                      [-]
        if (ip.eq.iprpPHIR30) Output(n)=PHIX (3,0,tau,del,z)    !tau^3*[d^3(Ar/RT)/d(tau)^3]                    [-]                      [-]
        if (ip.eq.iprpPHIR01) Output(n)=PHIX (0,1,tau,del,z)    !del  *[d  (Ar/RT)/d(del)]                      [-]                      [-]
        if (ip.eq.iprpPHIR02) Output(n)=PHIX (0,2,tau,del,z)    !del^2*[d^2(Ar/RT)/d(del)^2]                    [-]                      [-]
        if (ip.eq.iprpPHIR03) Output(n)=PHIX (0,3,tau,del,z)    !del^3*[d^3(Ar/RT)/d(del)^3]                    [-]                      [-]
        if (ip.eq.iprpPHIR11) Output(n)=PHIX (1,1,tau,del,z)    !tau*del    *[d^2(Ar/RT)/d(tau)  d(del)]        [-]                      [-]
        if (ip.eq.iprpPHIR12) Output(n)=PHIX (1,2,tau,del,z)    !tau*del^2  *[d^3(Ar/RT)/d(tau)  d(del)^2]      [-]                      [-]
        if (ip.eq.iprpPHIR13) Output(n)=PHIX (1,3,tau,del,z)    !tau*del^3  *[d^4(Ar/RT)/d(tau)  d(del)^3]      [-]                      [-]
        if (ip.eq.iprpPHIR21) Output(n)=PHIX (2,1,tau,del,z)    !tau^2*del  *[d^3(Ar/RT)/d(tau)  d(del)]        [-]                      [-]
        if (ip.eq.iprpPHIR22) Output(n)=PHIX (2,2,tau,del,z)    !tau^2*del^2*[d^4(Ar/RT)/d(tau)^2d(del)^2]      [-]                      [-]
        if (ip.eq.iprpPHIR23) Output(n)=PHIX (2,3,tau,del,z)    !tau^2*del^3*[d^5(Ar/RT)/d(tau)^3d(del)^3]      [-]                      [-]
        if (ip.eq.iprpPHIR31) Output(n)=PHIX (3,1,tau,del,z)    !tau^3*del  *[d^4(Ar/RT)/d(tau)  d(del)]        [-]                      [-]
        if (ip.eq.iprpPHIR32) Output(n)=PHIX (3,2,tau,del,z)    !tau^3*del^2*[d^5(Ar/RT)/d(tau)^2d(del)^2]      [-]                      [-]
        if (ip.eq.iprpPHIR33) Output(n)=PHIX (3,3,tau,del,z)    !tau^3*del^3*[d^6(Ar/RT)/d(tau)^3d(del)^3]      [-]                      [-]

c  Critical region states
      elseif (ip.lt.iBlockCrt) then
        if (ip.ge.iprpTC .and. ip.le.iprpDC) then
          if (lcrt) call CRTHMX (z,Tc,Pc,Dc,ierr,herr)
          lcrt=.false.
          if (ip.eq.iprpTC)     Output(n)=Tc                    !Critical temperature                           [K]                      [K]
          if (ip.eq.iprpPC)     Output(n)=Pc                    !Critical pressure                              [kPa]                    [kPa]
          if (ip.eq.iprpDC)     Output(n)=Dc                    !Critical density                               [mol/dm^3]               [kg/m^3]
        elseif (ip.ge.iprpTMAXT .and. ip.le.iprpDMAXT) then
          call MAXT (z,TmaxT,PmaxT,DmaxT,ierr,herr)
          if (ip.eq.iprpTMAXT)  Output(n)=TmaxT                 !Temperature at cricondentherm                  [K]                      [K]
          if (ip.eq.iprpPMAXT)  Output(n)=PmaxT                 !Pressure at cricondentherm                     [kPa]                    [kPa]
          if (ip.eq.iprpDMAXT)  Output(n)=DmaxT                 !Density at cricondentherm                      [mol/dm^3]               [kg/m^3]
        elseif (ip.ge.iprpTMAXP .and. ip.le.iprpDMAXP) then
          call MAXP (z,TmaxP,PmaxP,DmaxP,ierr,herr)
          if (ip.eq.iprpTMAXP)  Output(n)=TmaxP                 !Temperature at cricondenbar                    [K]                      [K  ]
          if (ip.eq.iprpPMAXP)  Output(n)=PmaxP                 !Pressure at cricondenbar                       [kPa]                    [kPa]
          if (ip.eq.iprpDMAXP)  Output(n)=DmaxP                 !Density at cricondenbar                        [mol/dm^3]               [kg/m^3]
        elseif (ip.ge.iprpTCEST .and. ip.le.iprpDCEST) then
          ierr=-999999
          if (lcrthmx) call CRTHMX (z,Tcest,Pcest,Dcest,ierr,herr)
          lcrthmx=.false.                                       !The following are calculated from the TC1 and VC1 parameters in the HMX.BNC file.
          if (ip.eq.iprpTCEST)  Output(n)=Tcest                 !Estimated mixture critical temperature         [K]                      [K]
          if (ip.eq.iprpPCEST)  Output(n)=Pcest                 !Estimated mixture critical pressure            [kPa]                    [kPa]
          if (ip.eq.iprpDCEST)  Output(n)=Dcest                 !Estimated mixture critical density             [mol/dm^3]               [kg/m^3]
        elseif (ip.ge.iprpTRED .and. ip.le.iprpDEL) then
          if (lredx) call REDX (z,Tred0,Dred0)
          lredx=.false.
          tau=XDIV(Tred0,T)
          del=XDIV(D,Dred0)
          if (ip.eq.iprpTRED)   Output(n)=Tred0                 !Reducing temperature                           [K]                      [K]
          if (ip.eq.iprpDRED)   Output(n)=Dred0                 !Reducing density                               [mol/dm^3]               [kg/m^3]
          if (ip.eq.iprpTAU)    Output(n)=tau                   !Tc/T (or Tred/T)                               [-]                      [-]
          if (ip.eq.iprpDEL)    Output(n)=del                   !D/Dc (or D/Dred)                               [-]                      [-]
        endif

c  Limits
      elseif (ip.lt.iBlockLim) then
        if (llimits) call LIMITS ('EOS',z,Tmin,Tmax,Dmax,Pmax)
        llimits=.false.
        if (ip.eq.iprpTMIN)   Output(n)=Tmin                    !Minimum temperature of the EOS                 [K]                      [K]
        if (ip.eq.iprpTMAX)   Output(n)=Tmax                    !Maximum temperature of the EOS                 [K]                      [K]
        if (ip.eq.iprpDMAX)   Output(n)=Dmax                    !Maximum density of the EOS                     [mol/dm^3]               [kg/m^3]
        if (ip.eq.iprpPMAX)   Output(n)=Pmax                    !Maximum pressure of the EOS                    [kPa]                    [kPa]

c  Transport, etc.
      elseif (ip.lt.iBlockTrn) then
        if (ltrnprp) call TRNPRP (T,D,z,eta,tcx,ierr,herr)
        ltrnprp=.false.
        if (ip.eq.iprpVIS)    Output(n)=eta                          !Viscosity                                 [uPa*s]
        if (ip.eq.iprpTCX)    Output(n)=tcx                          !Thermal conductivity                      [W/(m*K)]                [W/(m*K)]
        if (ip.eq.iprpPRANDTL)Output(n)=XDIV (eta*Cp,tcx*wm*1000d0)  !Prandlt number                            [-]                      [-]
        if (ip.eq.iprpTD)     Output(n)=tcx*10d0/(Ds*Cp)             !Thermal diffusivity                       [cm^2/s]                 [cm^2/s]
        if (ip.eq.iprpKV)     Output(n)=eta/(Ds*wm*100d0)            !Kinematic Viscosity                       [cm^2/s]                 [cm^2/s]
        if (ip.eq.iprpTRNTJ)  Output(n)=TRNTJ                        !Conformal temp. (for debugging only)      [K]                      [K]
        if (ip.eq.iprpTRNDJ)  Output(n)=TRNDJ                        !Conformal density (for debugging only)    [mol/dm^3]               [kg/m^3]

c  Properties from ancillary equations
      elseif (ip.lt.iBlockAnc) then
        if (ip.eq.iprpANC_TP) call PSATT (T,z,Output(n),ierr,herr)   !Vapor pressure from anc. given T          [kPa]                    [kPa]
        if (ip.eq.iprpANC_TDL)call DLSATT(T,z,Output(n),ierr,herr)   !Sat. liquid density from anc. given T     [mol/dm^3]               [kg/m^3]
        if (ip.eq.iprpANC_TDV)call DVSATT(T,z,Output(n),ierr,herr)   !Sat. vapor density from anc. given T      [mol/dm^3]               [kg/m^3]
        if (ip.eq.iprpANC_PT) call TSATP (D,z,Output(n),ierr,herr)   !Vapor temp. from anc. given P             [K]                      [K]            Assume input pressure is in the density slot.
        if (ip.eq.iprpANC_DT) call TSATD (D,z,Output(n),ierr,herr)   !Vapor temperature from anc. given D       [K]                      [K]
        if (ip.eq.iprpMELT_TP)call MELTT (T,z,Output(n),ierr,herr)   !Melting pressure given T                  [kPa]                    [kPa]
        if (ip.eq.iprpMELT_PT)call MELTP (D,z,Output(n),ierr,herr)   !Melting temperature given P               [K]                      [K]               "      "     "                  "      "
        if (ip.eq.iprpSUBL_TP)call SUBLT (T,z,Output(n),ierr,herr)   !Sublimation pressure given T              [kPa]                    [kPa]
        if (ip.eq.iprpSUBL_PT)call SUBLP (D,z,Output(n),ierr,herr)   !Sublimation temperature given P           [K]                      [K]               "      "     "                  "      "

c  EOS testing properties
      elseif (ip.lt.iBlockEOS) then
        if (ldervpvt) call DERVPVT(T,D,z,dPdD,dPdT,d2PdD2,d2PdT2,d2PdTD,
     &                dDdP,dDdT,d2DdP2,d2DdT2,d2DdPT,
     &                dTdP,dTdD,d2TdP2,d2TdD2,d2TdPD)
        ldervpvt=.false.
        if (ip.eq.iprpGRUN) Output(n)=dPdT/(Ds*Cv)                   !Gruneisen  parameter                      [-]                      [-]
        if (ip.eq.iprpPIP)                                           !Phase identification parameter            [-]                      [-]
     &    Output(n)=2d0-(XDIV(d2PdTD,dPdT)-XDIV(d2PdD2,dPdD))*D
        if (D.gt.1d-7) then
          Zz=P/(Ds*T*Rxgas)
          if (Zz.eq.xinf) Zz=1d0
          if (ip.eq.iprpZ1_D) Output(n)=(Zz-1d0)/D                   !(Z-1) over the density                    [dm^3/mol]               [m^3/kg]
          if (ip.eq.iprpZ1_P) Output(n)=(Zz-1d0)/P                   !(Z-1) over the pressure                   [1/kPa]                  [1/kPa]
          if (ip.eq.iprpPV)   Output(n)=P/D                          !Pressure times volume                     [(dm^3/mol)*kPa]         [(m^3/kg)*kPa]
        else
          call VIRBCD (T,z,bbx,ccx,ddx,eex)
          if (ip.eq.iprpZ1_D) Output(n)=bbx
          if (ip.eq.iprpZ1_P) Output(n)=bbx/(Rxgas*T)
          if (ip.eq.iprpPV)   Output(n)=Rxgas*T
        endif
        if (ip.eq.iprpSD)     Output(n)=S*D                          !Entropy times density                     [J/(mol*K)*(mol/dm^3)]   [(kJ/kg)*(kg/m^3)/K]
        if (ip.eq.iprp_1T)    Output(n)=-1d0/T                       !Negative reciprocal temperature           [1/K]                    [1/K]
        if (ip.eq.iprpRIEM) then                                     !Thermodynamic curvature (cubic nm/molecule)
          call RIEM (T,D,z,riemc)
          if (riemc.ne.xnotc) riemc=riemc*1d24
          Output(n)=riemc
        elseif (ip.eq.iprpRD) then                                   !Rectilinear diameter (Dl+Dv)/2            [mol/dm^3]               [kg/m^3]
          Output(n)=xnotc
          call SATT (T,z,1,pp,Dl,Dv,x,y,ierr,herr)
          if (.not.lEqual(Dl,Dv)) Output(n)=(Dl+Dv)/2d0
        endif

c  Saturation properties
      elseif (ip.lt.iBlockCSAT) then
        if (ip.eq.iprpLIQSPNDL) then                                 !Density at the liquid spinodal            [mol/dm^3]               [kg/m^3]
          call LIQSPNDL (T,z,Output(n),ierr,herr)
        elseif (ip.eq.iprpVAPSPNDL) then                             !Density at the vapor spinodal             [mol/dm^3]               [kg/m^3]
          call VAPSPNDL (T,z,Output(n),ierr,herr)
        elseif (nc.ne.1) then
          call ERRNUM (854,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        else
          if (ip.eq.iprpCSAT) then                                   !Saturated heat capacity                   [J/(mol*K)]              [(kJ/kg)/K]
            if (lcv2pk) call CV2PK (1,T,D,Cv2p,Csat,ierr,herr)
            lcv2pk=.false.
            Output(n)=Csat
          elseif (ip.eq.iprpCV2P) then                               !Isochoric two-phase heat capacity         [J/(mol*K)]              [(kJ/kg)/K]
            if (lcv2pk) call CV2PK (1,T,D,Cv2p,Csat,ierr,herr)
            lcv2pk=.false.
            Output(n)=Cv2p
          elseif (ip.eq.iprpDPDTSAT) then                            !dP/dT along the saturation line           [kPa/K]                  [kPa/K]
            if(ldptsatk)call DPTSATK(1,T,1,pp,dd,Csat,dPdTsat,ierr,herr)
            ldptsatk=.false.
            Output(n)=dPdTsat
          elseif (ip.eq.iprpDHDZSAT) then                            !dH/dZ along the sat. line (Waring func.)  [J/mol]                  [kJ/kg]
            if(ldptsatk)call DPTSATK(1,T,1,pp,dd,Csat,dPdTsat,ierr,herr)
            ldptsatk=.false.
            Output(n)=0d0
            if (P.gt.0.) Output(n)=Rxgas*T**2/P*dPdTsat
          endif
        endif

c  Heating values
      elseif (ip.lt.iBlockHEAT) then
        if (ip.eq.iprpHFRM) then
          call HEATFRM (T,D,z,Output(n),ierr,herr)                   !Heat of formation                         [J/mol]                  [kJ/kg]
        elseif (ip.eq.iprpHG) then
          if (lheat) call HEAT (T,D,z,hg,hn,ierr,herr)               !Gross (or superior) heating value         [J/mol]                  [kJ/kg]
          lheat=.false.
          Output(n)=hg
        elseif (ip.eq.iprpHN) then
          if (lheat) call HEAT (T,D,z,hg,hn,ierr,herr)               !Net (or inferior) heating value           [J/mol]                  [kJ/kg]
          lheat=.false.
          Output(n)=hn
        endif

c  Heat of vaporization
      elseif (ip.lt.iBlockHEATV) then
        ierr1=0
        if (ip.ne.iprpHEATVAPZ_P) then
          call HVAPZTN (T,P,z,1,hvap,hvap0,ierr,herr)                !For a pure fluid or at constant temperature for mixtures.
        else
          call HVAPZTN (T,P,z,2,hvap,hvap0,ierr,herr)                !At constant pressure for mixtures.
        endif
        ierr2=ierr
        if (ip.eq.iprpHEATVAPZ) then
          if (nc.eq.1) Output(n)=hvap                                !Heat of vaporization                           [J/mol]                  [kJ/kg]
          if (nc.gt.1) call ERRNUM (854,0,' ',' ',0d0,0d0,0d0,ierr,herr)
        elseif (ip.eq.iprpHEATVAPZ_T .or. ip.eq.iprpHEATVAPZ_P) then
          Output(n)=hvap
        else
          ierr=0
          if (lheat) call HEAT (T,D,z,hg,hn,ierr1,herr1)
          lheat=.false.
          if (ip.eq.iprpHGVOL .or. ip.eq.iprpHNVOL) then             !Gross HV (Ideal gas volume basis)              [MJ/m^3]                 [MJ/m^3]
            if (ip.eq.iprpHNVOL) hg=hn      !Even if state point is in the liquid, the heating value is multiplied
            Output(n)=hg*P/Rxgas/T/1000d0   ! by the ideal gas density (generally at 1 atm) to convert to a volume basis.
          else
            if (ip.eq.iprpHGLQ) Output(n)=hg-hvap0                   !Gross Heating Value (Liquid)                   [J/mol]                  [kJ/kg]
            if (ip.eq.iprpHNLQ) Output(n)=hn-hvap0                   !Net Heating Value (Liquid)                     [J/mol]                  [kJ/kg]
            if (lEQ0(hvap0)) ierr=999
          endif
        endif
        if (ierr.eq.999 .and. ierr2.gt.100) ierr=ierr2
        if (ierr.gt.100) Output(n)=xnotc
        if (ierr1.ne.0) call SWAPERR (ierr,ierr1,herr,herr1)

c  Other properties
      elseif (ip.lt.iBlockOTHER) then
        if (ltherm3) call THERM3 (T,D,z,xkappa,beta,xisenk,xkt,
     &               betas,bs,xkkt,thrott,pi,spht)
        ltherm3=.false.
        if (ip.eq.iprpSTN)   call SURFT (T,D,z,Output(n),ierr,herr)  !Surface tension                                [mN/m]                   [mN/m]
        if (ip.eq.iprpDE)    call DIELEC (T,D,z,Output(n))           !Dielectric constant                            [-]                      [-]
        if (ip.eq.iprpB12)   call B12 (T,z,Output(n))                !B12                                            [dm^3/mol]               [m^3/kg]
        if (ip.eq.iprpFPV)   call FPV (T,D,P,z,Output(n))            !FPV                                            [-]                      [-]
        if (ip.eq.iprpPINT)  Output(n)=pi                            !Internal pressure                              [kPa]                    [kPa]
        if (ip.eq.iprpPREP)  Output(n)=p+pi                          !Repulsive part of pressure                     [kPa]                    [kPa]
        if (ip.eq.iprpPATT)  Output(n)=-pi                           !Attractive part of pressure                    [kPa]                    [kPa]
        if (ip.eq.iprpRDAIR) Output(n)=wm/28.96546d0                 !Relative Density in air (specific gravity)     [-]                      [-]          (Ratio of molar mass of the fluid to the molar mass of air)
        if (ip.eq.iprpRDH2O) Output(n)=D*wm/999.102621466559d0       !Relative Density in water (specific gravity)   [-]                      [-]
        if (ip.eq.iprpEXERGY .or. ip.eq.iprpCEXERGY) then
          Output(n)=xinf
          if (D.gt.1d-20) then
            T0e=298.15d0
            P0e=101.325d0
            call TPFLSH (T0e,P0e,z,D0e,Dl,Dv,x,y,q0e,e0e,h0e,s0e,
     &                   Cv0e,Cp0e,w0e,ierr,herr)
            if (ip.eq.iprpEXERGY) Output(n)=h-h0e-T0e*(s-s0e)        !Flow Exergy                                    [J/mol]                  [kJ/kg]
            if (ip.eq.iprpCEXERGY)                                   !Closed Sys. Exergy                             [J/mol]                  [kJ/kg]
     &         Output(n)=e-e0e-T0e*(s-s0e)+P0e*(1d0/D-1d0/D0e)
          endif
        elseif (ip.eq.iprpCSTAR) then                                !Critical flow factor                           [-]                      [-]
          if (lcstar) call CSTAR (T,P,0d0,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
          lcstar=.false.
          Output(n)=Cs
        elseif (ip.eq.iprpTMF) then                                  !Throat mass flux                               [kg/(m^2*s)]             [kg/(m^2*s)]
          if (lcstar) call CSTAR (T,P,0d0,z,Cs,Ts,Ds,Ps,ws,ierr,herr)
          lcstar=.false.
          if (Cs.gt.0.) Output(n)=1000d0*Cs*P*XSQRT(wm/Rxgas/T/1000d0)
        elseif (ip.eq.iprpSUMFACT) then                              !Summation Factor                               [-]                      [-]
          if (lvirbcd12)
     &      call VIRBCD12 (T,z,o0,o1,o2,o3,o4,o5,o6,o7,o8,o9,Ba,Ca)
          lvirbcd12=.false.
          Output(n)=XSQRT(-o0*P/Rxgas/T)
        elseif (ip.eq.iprpAPI .and. D.gt.0.) then                    !API Gravity                                    [-]                      [-]
          Output(n)=141.5d0/(D*wm/999.102621466559d0)-131.5d0
        endif

c  Variables in the program that can be sent through the interface
      elseif (ip.lt.iBlockVAR) then
        if (ip.eq.iprpISETUP)  Output(n)=DBLE(iSETUP)  !Return a flag that indicates if SETUP was called during this call to REFPOP.
        if (ip.eq.iprpISETREF) Output(n)=DBLE(iSETREF) !Return a flag that indicates if SETREF was called during this call to REFPOP.

c  -----Inputs that will send back multiple properties for mixtures-----
c  Fluid fixed points
      elseif (ip.lt.iBlockCOMP) then
        if (ic.le.0 .or. ic.gt.nc) goto 200
        if (ip.eq.iprpXMASS) then                                    !Composition on a mole basis                    [-]                      [-]
          call XMASS (z,zkg,wmix)
          Output(n)=zkg(ic)
          if (nc.eq.1) Output(n)=1
          goto 200
        elseif (ip.eq.iprpXMOLE) then                                !Composition on a mole basis                    [-]                      [-]
          Output(n)=z(ic)
          if (nc.eq.1) Output(n)=1
          goto 200
        endif
        if (ip.eq.iprpTCRIT) Output(n)=Tcrit(ic)                     !Critical temperature of component i            [K]                      [K]
        if (ip.eq.iprpPCRIT) Output(n)=Pcrit(ic)                     !Critical pressure of component i               [kPa]                    [kPa]
        if (ip.eq.iprpDCRIT) Output(n)=Dcrit(ic)                     !Critical density of component i                [mol/dm^3]               [kg/m^3]
        if (ip.eq.iprpTCTRUE)Output(n)=Tctrue(ic)                    !True EOS critical temperature of component i   [K]                      [K]        (where dP/dD=0 and d^P/dD^2=0 at constant temperature)
        if (ip.eq.iprpDCTRUE)Output(n)=Dctrue(ic)                    !True EOS critical density of component i       [mol/dm^3]               [kg/m^3]   (where dP/dD=0 and d^P/dD^2=0 at constant temperature)
        if (ip.eq.iprpTTRP)  Output(n)=Ttpn(ic)                      !Triple point temperature of component i        [K]                      [K]
        if (ip.eq.iprpPTRP)  Output(n)=Ptpn(ic)                      !Triple point pressure of component i           [kPa]                    [kPa]
        if (ip.eq.iprpDTRP)  Output(n)=Dtpn(ic)                      !Triple point density of component i            [mol/dm^3]               [kg/m^3]
        if (ip.eq.iprpTNBP)  Output(n)=Tnbp(ic)                      !Normal boiling point temperature of comp. i    [K]                      [K]
        if (ip.eq.iprpREOS)  Output(n)=Reos(ic)                      !Gas constant of component i for EOS            [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpMM)    Output(n)=wmas(ic)                      !Molar mass of component i (molecular weight)   [g/mol]                  [g/mol]
        if (ip.eq.iprpACF)   Output(n)=accen(ic)                     !Acentric factor of component i                 [-]                      [-]
        if (ip.eq.iprpGWP)   Output(n)=gwpot(ic)                     !Global warming potential of component i        [-]                      [-]
        if (ip.eq.iprpODP)   Output(n)=odpot(ic)                     !Ozone depletion facotr of component i          [-]                      [-]
        if (ip.eq.iprpTREF)  Output(n)=Tref(ic)                      !Ref. state temperature of component i          [K]                      [K]
        if (ip.eq.iprpDREF)  Output(n)=Dref(ic)                      !Ref. state pressure of component i             [kPa]                    [kPa]
        if (ip.eq.iprpHREF)  Output(n)=href(ic)                      !Ref. state enthalpy of comp. i at T0 and P0    [J/mol]                  [kJ/kg]
        if (ip.eq.iprpSREF)  Output(n)=sref(ic)                      !Ref. state entropy of comp. i at T0 and P0     [J/(mol*K)]              [(kJ/kg)/K]
        if (ip.eq.iprpDIPOLE)Output(n)=Dipole(ic)                    !Dipole moment of component i                   [debye]                  [debye]
        if (ip.eq.iprpHEATCOMB)Output(n)=xCmbst(ic)                  !Heat of combustion at 25 C and 1 atm           [J/mol]                  [kJ/kg]

c  Transport properties
        if (ip.ge.iprpETA0 .and. ip.le.iprpTCXC) then
          if (ltrnprp) call TRNPRP (T,D,z,eta,tcx,ierr,herr)
          ltrnprp=.false.
c  Viscosity=xeta0+xetaB2+xetar+xetac
          if (ip.eq.iprpETA0)  Output(n)=xeta0(ic)                   !Dilute gas viscosity of component i            [uPa*s]                  [uPa*s]
          if (ip.eq.iprpETAB2) Output(n)=xetaB2(ic)                  !Second virial viscosity of component i         [uPa*s]                  [uPa*s]
          if (ip.eq.iprpETAR)  Output(n)=xetar(ic)                   !Residual viscosity of component i              [uPa*s]                  [uPa*s]
          if (ip.eq.iprpETAC)  Output(n)=xetac(ic)                   !Crit. enhancement viscosity of component i     [uPa*s]                  [uPa*s]
c  Thermal conductivity=xtcx0+xtcxr+xtcxc
          if (ip.eq.iprpTCX0)  Output(n)=xtcx0(ic)                   !Dilute gas thermal conductivity of comp. i     [W/(m*K)]                [W/(m*K)]
          if (ip.eq.iprpTCXR)  Output(n)=xtcxr(ic)                   !Background thermal conductivity of comp. i     [W/(m*K)]                [W/(m*K)]
          if (ip.eq.iprpTCXC)  Output(n)=xtcxc(ic)                   !Thermal cond. crit. enhancement of comp. i     [W/(m*K)]                [W/(m*K)]

c  Mixture properties as a function of component number
        elseif (ip.eq.iprpK) then                                    !K value (y/x)                                  [-]                      [-]
          ierr=999           !This is not available without the liquid and vapor compositions.
        elseif (ip.eq.iprpF) then                                    !Fugacities                                     [kPa]                    [kPa]
          call FGCTY2 (T,D,z,phi,ierr,herr)
          lfgcty2=.false.
          Output(n)=phi(ic)
        elseif (ip.eq.iprpFC) then                                   !Fugacity coefficients                          [-]                      [-]
          call FUGCOF (T,D,z,phi,ierr,herr)
          lfugcof=.false.
          Output(n)=phi(ic)
        elseif (ip.eq.iprpCPOT) then                                 !Chemical potentials                            [J/mol]                  [kJ/kg]
          call CHEMPOT (T,D,z,phi,ierr,herr)
          lchempot=.false.
          Output(n)=phi(ic)
        elseif (ip.eq.iprpDADN) then                                 !n*partial(alphar)/partial(ni)                  [-]                      [-]
          if (lphiderv) call PHIDERV (1,T,D,z,dadn,dnadn,ierr,herr)
          lphiderv=.false.
          Output(n)=dadn(ic)
        elseif (ip.eq.iprpDNADN) then                                !partial(n*alphar)/partial(ni)                  [-]                      [-]
          if (lphiderv) call PHIDERV (1,T,D,z,dadn,dnadn,ierr,herr)
          lphiderv=.false.
          Output(n)=dnadn(ic)
        endif
      endif

      if ((ierr.lt.0   .and. ierr1.eq.0) .or.
     &    (ierr.gt.0   .and. ierr1.le.0) .or.
     &    (ierr.gt.100 .and. ierr1.le.100)) then           !Replace out of bound errors with more important errors.
        ierr1=ierr           !Save most important errors to be return on exit from the subroutine.
        herr1=herr
      endif
 200  enddo

 900  ierr=ierr1
      if (ierr.ne.0) herr=herr1

      end                                           subroutine ALLPROPS0
c
c ======================================================================
c
      subroutine ABFLSH (ab,a,b,z,iFlag,T,P,D,Dl,Dv,
     &                   x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
c
c    General flash calculation that handles all inputs of T, P, D, h, e, s, and q.
c
c    Includes both blind flash calculations, and situations where
c    the phase is known to be liquid, vapor, or 2-phase, and thus the
c    calculation time will be much faster.
c
c    Many of the 2-phase flash routines can accept initial estimates to
c    decrease calculation time and improve convergence.  ABFLSH does
c    not accept these, and ABFL2 or other routines will need to be called
c    to use the initial estimates.  These routines end in the letters FL2.
c
c    Notes:
c
c    * Cp and w are not defined for 2-phase states; the flag -9999980 is returned.
c    * Cv for 2-phase states is not calculated (use CV2PK); the flag -9999990 is returned.
c
c    **Information on ab**
c
c    Valid character codes for ab are:
c
c    - T - Temperature [K]
c    - P - Pressure [kPa]
c    - D - Density [mol/L or kg/m^3]
c    - E - Internal energy [J/mol or kJ/kg]
c    - H - Enthalpy [J/mol or kJ/kg]
c    - S - Entropy [J/mol-K or kJ/kg]
c    - Q - Quality [mol/mol or kJ/kg]
c
c    * For example, 'PH' indicates pressure and enthalpy inputs.
c    * For saturation properties, use codes of 'TQ' or 'PQ' for ab, and send b=1.
c    * The order of the letters does not matter, for example 'DH' = 'HD'
c      for saturated vapor values and b=0 for saturated liquid values.
c
c    **Information on iFlags**
c
c    Three flags are currently allowed, and are sent combined in a three digit
c    integer value.  The digit on the right is the mass flag (iMass) defined below, the
c    middle digit is the phase flag (kph), and the digit on the left specifies other flags (k).
c
c    * iMass: Molar or mass flag
c        - 0 - All inputs and outputs are given on a mole basis.
c        - 1 - All inputs and outputs are given on a mass basis.
c        - 2 - All inputs and outputs are given on a mass basis except
c          composition, which is given on a mole basis.
c    * kph: Phase flag (except for inputs of Q)
c        - 0 - Unknown phase, the saturation routines will be called to determine the phase, which
c          adds a substantial amount of time needed to calculate the properties.
c        - 1 - State point is in the liquid phase, do not call saturation routine to determine state.
c        - 2 - State point is in the vapor phase, do not call saturation routine to determine state.
c        - 3 - State point is in the two-phase region.
c    * kr,kq: Other flags for inputs of quality and either temperature or pressure (kq flag)
c        - 0 - Default
c        - 1 - Quality on a molar basis (moles vapor/total moles) (default, the value of 1 is not necessarily needed).
c        - 2 - Quality on a mass basis (mass vapor/total mass);
c          for inputs of T and either h or e (kr flag)
c        - 3 - Return lower density root.
c        - 4 - Return higher density root.
c
c    Examples:
c
c    * 000 - Default - Phase of state is unknown, molar units will be used everywhere,
c      higher density root will be returned.
c    * 002 - Use mass based properties for everything except composition.
c    * 011 - State is in the liquid, properties are mass based.
c    * 300 - Return the lower density root for TH or TE inputs.
c    * 200 - All inputs are on a mole basis, but quality is sent on a mass basis.
c
c  Inputs:
c       ab--Character string composed of two letters that indicate
c           the input properties.
c        a--Value of the property identified by the first letter in ab
c        b--Value of the property identified by the second letter in ab
c        z--Composition (array of mole fractions)
c        For TQ and PQ inputs, send b=-99 for melting line states and b=-98 for sublimation line states.
c    iFlag--Multiple flags combined into one variable (see above)
c
c  Outputs:
c        T--Temperature [K]
c        P--Pressure [kPa]
c        D--Density [mol/L or kg/m^3]
c       Dl--Molar density of the liquid phase [mol/L or kg/m^3]
c       Dv--Molar density of the vapor phase [mol/L or kg/m^3];
c           if only one phase is present, Dl = Dv = D.
c        x--Composition of the liquid phase (array of mole or mass fractions)
c        y--Composition of the vapor phase (array of mole or mass fractions);
c           if only one phase is present, x = y = z.
c        q--Vapor quality on a MOLAR basis (moles of vapor/total moles):
c           q > 0 and q < 1 - indicates a 2-phase state
c           q < 0     -  Subcooled (compressed) liquid
c           q = 0     -  Saturated liquid
c           q = 1     -  Saturated vapor
c           q > 1     -  Superheated vapor
c           q = -998  -  Subcooled liquid, but quality not defined  (usually P > Pc)
c           q =  998  -  Superheated vapor, but quality not defined (usually T > Tc)
c           q =  999  -  Supercritical state (T>Tc and P>Pc)
c        e--Overall internal energy [J/mol or kJ/kg]
c        h--Overall enthalpy [J/mol or kJ/kg]
c        s--Overall entropy [J/mol-K or kJ/kg-K]
c       Cv--Isochoric (constant D) heat capacity [J/mol-K or kJ/kg-K]
c       Cp--Isobaric (constant P) heat capacity [J/mol-K or kJ/kg-K]
c        w--Speed of sound [m/s]
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-11-17 EWL, original version

      include 'COMMONS.INC'
      character herr*255,herr1*255,ab*2,ab2*2,ab3*2,at*1,bt*1
      dimension x(ncmax),y(ncmax),z(ncmax),xl(ncmax),xv(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ABFLSH

      T=0d0
      P=0d0
      D=0d0
      Dl=0d0
      Dv=0d0
      x(1:nc)=0d0
      y(1:nc)=0d0
      q=0d0
      call GETINPUTS (ab,ab2,at,bt,iat,ibt,a,b,aa,bb,T,D,P,ierr,herr)
      if (ierr.gt.0) RETURN
      ab3=ab2
      if (ibt.eq.iLetterE .or. ibt.eq.iLetterH .or. ibt.eq.iLetterS)then
        if (ab2.ne.'SH' .and. ab2.ne.'SE') ab2(2:2)='B'    !Change the E, H, or S to B for use in some routines below.
      endif

c  Separate iFlag into iMass, kph, kr, and kq
      x(1:nc)=z(1:nc)
      y(1:nc)=z(1:nc)
      xl(1:nc)=z(1:nc)
      xv(1:nc)=z(1:nc)
      q=xnotc
      i=iFlag
      kr=i/100
      i=i-kr*100
      kph=i/10
      iMass=i-kph*10
      if (kr.le.2) then
        kq=kr
        kr=0
      else
        kr=kr-2
        kq=0
      endif

c  Convert from mass to molar basis if needed.
      if (iMass.eq.1 .or. iMass.eq.2) then
        wmm=WMOL(z)
        call QMOLE ((q),x,y,q,xl,xv,wliq,wvap,ierr,herr)
        if (iat.eq.iLetterD .or. ibt.eq.iLetterD) D=D/wmm
        if (iat.eq.iLetterE .or. iat.eq.iLetterH .or. iat.eq.iLetterS)
     &      a=a*wmm/1000d0
        if (ibt.eq.iLetterE .or. ibt.eq.iLetterH .or. iat.eq.iLetterS)
     &      b=b*wmm/1000d0
      endif
      ierr=0
      herr=' '

      if (ab2.eq.'TD&' .or. ab2.eq.'DT&') then
        if (ab2.eq.'DT&') call SWAPV (D,T)
        call CRITP (z,Tc,Pc,Dc,ierr1,herr1)
        call GETQQ (T,P,D,Tc,Pc,Dc,q)

c  Make calls for single-phase states.
      elseif (kph.eq.1 .or. kph.eq.2) then
        if (ab2.eq.'TD') then          !Temperature-density inputs
          call PRESS (T,D,z,P)
        elseif (ab2.eq.'TP') then      !Temperature-pressure inputs, call TPRHO without a guess value for D.
          call TPRHO (T,P,z,kph,0,D,ierr,herr)
        elseif (ab2.eq.'DP') then      !Pressure-density inputs
          call DBFL1 (D,P,z,ab3,T,pp,ierr,herr)
        elseif (ab2.eq.'DB') then      !Density input with either E, H, or S
          call DBFL1 (D,bb,z,ab3,T,P,ierr,herr)
        else                           !Temperature input with either E, H, S
          Dmin=0d0
          if (iat.eq.iLetterT) then
            call SATGUESS (kph,1,0,0,z,T,pp,Dsat,h,s,dd,y,ierr,herr)
            if (kph.eq.2) Dmax=Dsat*1.02d0
            if (kph.eq.1) Dmin=Dsat/1.02d0
          endif
          call ABFL1 (ab3,aa,bb,z,kph,0,Dmin,Dmax,T,P,D,ierr,herr)
        endif
        call CRITP (z,Tc,Pc,Dc,ierr1,herr1)
        call GETQQ (T,P,D,Tc,Pc,Dc,q)

c  Make calls for two-phase states.
      elseif (kph.eq.3) then
        if (ab2.eq.'TP') then          !Temperature-pressure inputs.
          call TPFL2 (T,P,z,Dl,Dv,x,y,q,ierr,herr)
          call GETPROPS (2,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)
        elseif (ab2.eq.'TD') then      !Temperature-density inputs.
          call TDFL2 (T,D,z,P,Dl,Dv,x,y,q,ierr,herr)
          call GETPROPS (0,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)
        else
          call ABFL2 (ab3,aa,bb,z,kq,0,Tbub,Tdew,Pbub,Pdew,
     &                Dlbub,Dvdew,xl,xv,T,P,Dl,Dv,x,y,q,ierr,herr)
          call GETPROPS (2,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)
        endif

c  Make calls for states where the phase is not known or for saturation states.
      else
        if (ab2.eq.'TD') then
          call TPDFLSH (T,P,D,z,1,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
        elseif (ab2.eq.'TP') then
          call TPFLSH (T,P,z,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
        elseif (ab2.eq.'DP') then
          call TPDFLSH (T,P,D,z,3,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
        elseif (ab2.eq.'TB') then
          call TBFLSH (T,bb,z,kr,ab3,P,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,
     &    ierr,herr)
        elseif (ab2.eq.'PB') then
         call PBFLSH(P,bb,z,ab3,T,D,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
        elseif (ab2.eq.'DB') then
         call DBFLSH(D,bb,z,ab3,T,P,Dl,Dv,x,y,q,e,h,s,Cv,Cp,w,ierr,herr)
        elseif (ab2.eq.'SH') then
          call HSFLSH (bb,aa,z,T,P,D,Dl,Dv,x,y,q,e,Cv,Cp,w,ierr,herr)
        elseif (ab2.eq.'SE') then
          ieflg=1
          call HSFLSH (bb,aa,z,T,P,D,Dl,Dv,x,y,q,e,Cv,Cp,w,ierr,herr)
          ieflg=0
        elseif (ab2.eq.'TQ') then
          q=bb
          if (q.eq.-99.) then          !Calculate melting line
            call MELTT (T,z,P,ierr1,herr1)
            call TPRHO (T,P,z,1,0,D,ierr,herr)
            call SWAPERR (ierr,ierr1,herr,herr1)
          elseif (q.eq.-98.) then      !Calculate sublimation line
            call SUBLT (T,z,P,ierr1,herr1)
            call TPRHO (T,P,z,2,0,D,ierr,herr)
            call SWAPERR (ierr,ierr1,herr,herr1)
          else
            call AQFLSH (T,P,q,z,1,kq,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,qmol,
     &           ierr,herr)
            q=qmol
          endif
        elseif (ab2.eq.'PQ') then
          q=bb
          if (q.eq.-99.) then          !Calculate melting line
            call MELTP (P,z,T,ierr1,herr1)
            call TPRHO (T,P,z,1,0,D,ierr,herr)
            call SWAPERR (ierr,ierr1,herr,herr1)
          elseif (q.eq.-98.) then      !Calculate sublimation line
            call SUBLP (P,z,T,ierr1,herr1)
            call TPRHO (T,P,z,2,0,D,ierr,herr)
            call SWAPERR (ierr,ierr1,herr,herr1)
          else
           call AQFLSH (T,P,q,z,2,kq,D,Dl,Dv,x,y,e,h,s,Cv,Cp,w,qmol,
     &                 ierr,herr)
           q=qmol
          endif
        elseif (ab2.eq.'DQ') then
          if (leq0(bb) .or. leq0(bb-1d0)) then
            call SATD (D,z,0,kr,T,P,Dl,Dv,x,y,ierr,herr)
          else
            call ABFL2 (ab2,aa,bb,z,kq,0,Tbub,Tdew,Pbub,Pdew,
     &                Dlbub,Dvdew,xl,xv,T,P,Dl,Dv,x,y,q,ierr,herr)
          endif
        elseif (ab2.eq.'HQ') then
          call SATH (a,z,0,nroot,k1,T,P,D,k2,T2,P2,D2,ierr,herr)
          if (b.eq.1. .and. nroot.eq.2) then
            T=T2
            P=P2
            D=D2
          endif
        elseif (ab2.eq.'EQ') then
          call SATE (a,z,kph,nroot,k1,T,P,D,k2,T2,P2,D2,ierr,herr)
          if (b.eq.1. .and. nroot.eq.2) then
            T=T2
            P=P2
            D=D2
          endif
        elseif (ab2.eq.'SQ') then
          call SATS (a,z,0,nroot,k1,T,P,D,k2,T2,P2,D2,
     &                 k3,T3,P3,D3,ierr,herr)
          if (b.eq.1. .and. nroot.ge.2) then
            T=T2
            P=P2
            D=D2
          elseif (b.eq.2. .and. nroot.eq.3) then
            T=T3
            P=P3
            D=D3
          endif
        else
          call ERRNUM (601,0,'ABFLSH',ab,0d0,0d0,0d0,ierr,herr)
        endif
      endif
      if (ierr.eq.0) herr=' '
      call GETPROPS (0,ierr,q,T,P,D,Dl,Dv,z,x,y,qkg,e,h,s,Cv,Cp,w)

c  Convert from mass to molar basis if needed.
      if (iMass.eq.1 .or. iMass.eq.2) then
        qmol=q
        wmm=WMOL(z)
        if (q.ge.0 .and. q.le.1.) then
          call QMASS (qmol,xl,xv,q,x,y,wliq,wvap,ierr1,herr1)
          wmm=qmol*wvap+(1d0-qmol)*wliq
        endif
        D=D*wmm
        e=e*wmm/1000d0
        h=h*wmm/1000d0
        if (s.ne.xinf) s=s*wmm/1000d0
        Cv=Cv*wmm/1000d0
        Cp=Cp*wmm/1000d0
      endif

      end                                              subroutine ABFLSH
c
c ======================================================================
c
      subroutine REFSET1 (hFld,iUnits,io,a,b,Output,hUnits,iUCode,
     &                    hUnitsArray,ierr,herr)

c  Internal routine to the REFPROP subroutine.  Most variables are defined
c  there and this routine is not meant to be called elsewhere.

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character hFld*(*),hUnitsArray(iPropMax)*50,CHANGEALL*255
      character*255 herr,hIn1,hOut1,hUnits,hU,hfld2(ncmax),UCASE,hSwap
      dimension Output(iPropMax),iUCodeArray(iPropMax),iutmp(20)
      dimension z(ncmax)
      common /REFSETUP/ z,aa,bb,T,D,P,iFlg,iEHT,hIn1,hOut1
      common /ALLPRPQ/ qsv,qmolesv,qmasssv,P2psv,e2psv,h2psv,s2psv,
     &                 iSwap,hSwap

      ierr=0
      iUCode=0
      herr=' '
      hUnits=' '
      Output(1:20)=xnotc

      iIn=INDEX(hIn1, '    ')-1
      if (iUnitTag(1).eq.0) call REFPRPUNITS
      if (io.eq.2) goto 200

c  Checks for inputs that do not require a fluid to have been specified.
 100  if (iIn.eq.4 .or. iIn.eq.5) then
        if (hIn1.eq.'PATH') then
          iRefSv=0
          call SETPATH (hFld)          !hOut1 not used here since it has been uppercased.
          goto 550
        elseif (hIn1.eq.'FLAGS') then
          iRefSv=0
          call FLAGS (hOut1,iFlg,iUCode,ierr,herr)
          Output(1)=iUCode
          if (hfld.ne.' ') goto  808
          goto 550
        endif

c  Send back values of the nodes that were setup by the call to SATSPLN.
c  hIn should contain the code:  SPLINENODES
c  Send the desired node number in the variable iFlag in the REFPROP routine.
c  hFld and hOut should be blank.  All other inputs are ignored.
c  The number of nodes is sent back in iUCode (and will be zero if the splines are not setup).
c  The Output array contains the density of the node (mol/dm^3) in the first slot,
c  and the following calculated values at that density in the subsequent slots:
c  temperature (K), pressure (kPa), equilibrium density (mol/dm^3), and the
c  equilibrium compositions in slots 5 to nc (number of components).
      elseif (iIn.eq.11) then
        if (hIn1.eq.'SPLINENODES') then
          iUCode=nArr
          if (iFlg.gt.0 .and. iFlg.le.nArr) then
            Output(1)=EXP5(xArr(iFlg,0))
            Output(2)=EXP5(xArr(iFlg,nc+1))
            Output(3)=EXP5(xArr(iFlg,nc+2))
            Output(4)=EXP5(xArr(iFlg,nc+3))
            do i=1,nc
              Output(4+i)=EXP5(xArr(iFlg,i))
            enddo
          endif
          goto 550
        endif
      endif
      iFlg=0
      if (INDEX(hOut1,'    ').le.4) RETURN

c  Check if the DLL number has been requested.
      if (hOut1.eq.'DLL#') then
        call SETUP (-1,hFld2,hu,'DEF',ierr,herr)
        hUnits=herr
        iUCode=ierr
        ierr=0
        Output(1)=xflg
        goto 550

c  Check for unit options.
      elseif (hOut1(1:4).eq.'UNIT') then
        iRefSv=0
        iUCode=iUnits
        if (hOut1(1:8).eq.'UNITCONV') then
          i=INDEX(hOut1,'_')
          if (i.gt.0) then
            hU=hOut1(i+1:)
            Output(1)=CONVUNITS(a,hU,hFld,hIn1,b,ierr,herr)
            if (ierr.eq.0) hUnits=herr           !Transfer unit string to hFld
          endif
          goto 550
        elseif (hOut1(1:8).eq.'UNITUSER') then
          iutmp=0
          hIn1=CHANGEALL ((hIn1),';',',')
          read (hIn1,*,err=20,end=20) (iutmp(i),i=1,13)
 20       iUCode=iUnitsUser
          if (hOut1(9:9).eq.'2') iUCode=iUnitsUser2
          if (iutmp( 1).ne.0) iUUnits(iUTypeT,iUCode)=iutmp(1)
          if (iutmp( 2).ne.0) iUUnits(iUTypeP,iUCode)=iutmp(2)
          if (iutmp( 3).ne.0) then
            iUUnits(iUTypeD,iUCode)=iutmp(3)
            iUUnits(iUTypeV,iUCode)=iutmp(3)+50
          endif
          if (iutmp( 4).ne.0) iUUnits(iUTypeH,iUCode)=iutmp(4)
          if (iutmp( 5).ne.0) iUUnits(iUTypeS,iUCode)=iutmp(5)
          if (iutmp( 6).ne.0) iUUnits(iUTypeW,iUCode)=iutmp(6)
          if (iutmp( 7).ne.0) iUUnits(iUTypeI,iUCode)=iutmp(7)
          if (iutmp( 8).ne.0) iUUnits(iUTypeU,iUCode)=iutmp(8)
          if (iutmp( 9).ne.0) iUUnits(iUTypeK,iUCode)=iutmp(9)
          if (iutmp(10).ne.0) iUUnits(iUTypeN,iUCode)=iutmp(10)
          if (iutmp(11).ne.0) iUUnits(iUTypeM,iUCode)=iutmp(11)
          if (iutmp(12).ne.0) iUUnits(iUTypeF,iUCode)=iutmp(12)
          if (iutmp(13).ne.0) iUUnits(iUTypeG,iUCode)=iutmp(13)
          goto 550
        elseif (hOut1.ne.'UNITSTRING' .or. hFld.ne.' ') then
          iUCode=-1
          hU=UCASE(hFld)
          call GETENUM (1,hU,iUCode,ierr,herr)
          if (ABS(iUCode).gt.1000) iUCode=2
          if (iUCode.eq.-1) goto 801
        endif
        if (hOut1.eq.'UNITNUMB') goto 550
        if (hIn1.eq.' ') goto 801
        call ALLPROPS (hIn1,-iUCode,0,2,300d0,0d0,z,
     &                Output,hUnitsArray,iUCodeArray,ierr,herr)
        hUnits=hUnitsArray(1)
        iFlg=555
      endif
      RETURN




c  ---Check for inputs that require a fluid to be loaded---

c  Check for inputs of SETREF and SETREFOFF used to change the reference state.
 200  iFlg=0
      if (iIn.eq.6) then
        if (hIn1.eq.'SETREF') then
          iRefSv=0
          iSetRef2=INT(a)
          if (iSetRef2.ne.1 .and. iSetRef2.ne.2) iSetRef2=2
          hRef3=hOut1(1:3)
          i=INDEX(hOut1,';')
          if (i.gt.0) then             !If hOut1 contains a semicolon, extract T0, P0, H0, and S0
            hOut1=hOut1(i+1:)
            hOut1=CHANGEALL(hOut1,',','.')       !Change all commas to periods for European settings.
            hOut1=CHANGEALL(hOut1,';',',')       !Change all semicolons to commas.
            read (hOut1,*,err=801,end=801) h0Ref3,s0Ref3,t0Ref3,p0Ref3
          endif
          hFldOld=' '
          isrf=iSetRef2
         call SETREF(hRef3,isrf,z,h0Ref3,s0Ref3,T0Ref3,P0Ref3,ierr,herr)
         goto 550

c  Maximum limits of equation of state.
        elseif (hIn1.eq.'EOSMAX') then
          call LIMITS ('EOS',z,Tmin,T,D,P)
          if (hOut1.eq.'T') then
            Output(1)=CONVUNIT(T,iUnits,iUTypeT,z,hU,ierr,herr)
          elseif (hOut1.eq.'P') then
            Output(1)=CONVUNIT(P,iUnits,iUTypeP,z,hU,ierr,herr)
          elseif (hOut1.eq.'D') then
            Output(1)=CONVUNIT(D,iUnits,iUTypeD,z,hU,ierr,herr)
          endif
          iFlg=803
          RETURN
        endif

c  Call the SATSPLN routine.
      elseif (iIn.eq.7) then
        if (hIn1.eq.'SATSPLN') then
          call SATSPLN (z,ierr,herr)
          goto 550
        endif

c  Turn off the settings from a previous request to set the reference state.
      elseif (iIn.eq.9) then
        if (hIn1.eq.'SETREFOFF') then
          iRefSv=0
          iSetRef2=0
          hFldOld=' '
          hRef3='DEF'
          call SETREF (hRef3,2,z,0d0,0d0,0d0,0d0,ierr,herr)
          goto 550
        endif
      endif




c  Call the ALLPROPS routine to get all properties that are not T and D dependent (and to make the first pass through ALLPROPS that is required for the next steps).
 500  iPass=1
      call ALLPROPS (hOut1,-iUnits,0,-1,T,D,z,
     &               Output,hUnitsArray,iUCodeArray,ierr,herr)
      hUnits=hUnitsArray(1)
      iUCode=iUcodeArray(1)
      if (iSwap.eq.1) hUnits=hSwap   !For long strings, insert the saved string hSwap into hUnits if the requested input for this is in the first position of hIn.
      if (iRefExit.eq.1) iFlg=999
      RETURN

 550  iFlg=999
      RETURN
 801  iFlg=801
      RETURN
 808  iFlg=808
      RETURN

      end                                             subroutine REFSET1
c
c ======================================================================
c
      subroutine REFSET2 (io,iMass,x,y,x3,q,Output,hU,ierr,herr)

c  Internal routine to the REFPROP subroutine.  Most variables are defined
c  there and this routine is not meant to be called elsewhere.
c  z,x,y must be molar compositions.

      include 'COMMONS.INC'
      character*255 herr,herr2,hIn1,hOut1,h3,LTRIM,hU
      character hnam*12,hcasn*12,hn80*80
      dimension Output(iPropMax)
      dimension z(ncmax),x(ncmax),y(ncmax),x3(ncmax),xy(ncmax)
      common /REFSETUP/ z,aa,bb,T,D,P,iFlg,iEHT,hIn1,hOut1

      iFlg=0
      ierr=0
      if (io.eq.2) goto 200

c  Check for inputs that request metastable or saturation states.
      if (INDEX(hIn1 ,'  ').lt.4) RETURN
      if (hIn1.eq.'TD&') then          !Metastable or unstable state.
        T=aa
        D=bb
        goto 600
      elseif (hIn1.eq.'DT&') then          !Metastable or unstable state.
        T=bb
        D=aa
        goto 600
      elseif (hIn1(3:3).eq.'>' .or. hIn1(3:3).eq.'L') then           !State in the liquid phase (or metastable liquid phase)
        if (iEHT.ne.999) iEHT=10
        if (iEHT.eq.999) iEHT=400
      elseif (hIn1(3:3).eq.'<' .or. hIn1(3:3).eq.'V') then           !State in the vapor phase (or metastable vapor phase)
        if (iEHT.ne.999) iEHT=20
        if (iEHT.eq.999) iEHT=600
      elseif (hIn1.eq.'NBP') then      !Normal boiling point
        aa=101.325d0
        bb=0d0
        hIn1='PQ'
      elseif (hIn1(2:).eq.'LIQ') then  !Saturated liquid state
        hIn1(2:)='Q'
        bb=0
      elseif (hIn1(2:).eq.'VAP') then  !Saturated vapor state
        hIn1(2:)='Q'
        bb=1
      elseif (hIn1(2:4).eq.'SAT') then !Saturated states
        bb=0       !For T or P inputs, assume saturated liquid.  However, the user can input "Dliq" or "Dvap" as the outputs to get properties at both saturation states.
        if (hIn1(1:1).eq.'D') then
          call CRITP (z,Tc,Pc,Dc,ierr,herr)
          if (aa.lt.Dc) bb=1
        elseif (hIn1(1:1).eq.'S') then
          if (hIn1(1:5).eq.'SSAT2') bb=1
          if (hIn1(1:5).eq.'SSAT3') bb=2
        elseif (hIn1(1:1).eq.'H') then
          if (hIn1(1:5).eq.'HSAT2') bb=1
        endif
        hIn1(2:)='Q'
      elseif (hIn1(2:).eq.'MELT') then           !Melting state
        hIn1(2:)='Q'
        bb=-99
      elseif (hIn1(2:).eq.'SUBL') then           !Sublimation state
        hIn1(2:)='Q'
        bb=-98
      elseif (hIn1.eq.'CRIT') then               !Critical point
        call CRTHMX (z,T,P,D,ierr,herr)
        goto 600
      elseif (hIn1.eq.'TRIP' .or. hIn1.eq.'EOSMIN') then             !Triple point
        call ISPURE (z,icomp)
        aa=Ttpn(icomp)
        bb=0d0
        if (icomp.eq.0) call LIMITS ('EOS',z,aa,Tmax,Dmax,Pmax)
        hIn1='TQ'
      endif
      RETURN

c  Check for special outputs that are greater than 2 characters long (i.e., not T, P, D, Cv, etc.)
c    These all deal with saturation or 2-phase inputs that cannot be handled by the call to ALLPROPS.
 200  continue
      if (INDEX(hOut1 ,'  ').le.4) RETURN
      if (hOut1.eq.'PHASE') then
        call PHASE (q,hU)
        Output(1)=xFlg
        goto 550
      elseif (hOut1.eq.'LIQUIDFLUIDSTRING') then
        iphs=1
        goto 300
      elseif (hOut1.eq.'VAPORFLUIDSTRING') then
        iphs=2
        goto 300
      endif
      RETURN


c  Returns a fluid string similar to that which would be sent to this routine in the hFld string,
c     but with the fluid names and compositions for either the liquid or the vapor phase for a
c     2-phase state.  When iPhs=1, the liquid phase is returned, when iPhs=2, the vapor phase is returned.
 300  h3=' '
      herr=' '
      xy(1:nc)=x(1:nc)
      if (iMass.ne.1 .and. iphs.eq.2) xy(1:nc)=y(1:nc)
      if (iMass.eq.1 .and. iphs.ne.2) call XMASS (x,xy,wm)
      if (iMass.eq.1 .and. iphs.eq.2) call XMASS (y,xy,wm)
      do i=1,nc
        call NAME (-i,hnam,hn80,hcasn)
        j=INDEX(hn80,'.FLD    ')
        if (j.gt.0) hn80=hn80(1:j-1)
        herr2=' '
        write (herr2,'(f16.9)') xy(i)
        herr2=LTRIM(herr2)
        if (i.eq.1) then
          h3=hn80
          herr=herr2
        else
          k=INDEX(h3,'               ')
          j=INDEX(herr,'               ')
          if (j.eq.0 .or. k.eq.0) goto 801
          h3=h3(1:k-1)//';'//hn80
          herr=herr(1:j-1)//';'//herr2
        endif
      enddo
      j=INDEX(h3,'               ')
      hU=h3(1:j-1)//'|'//herr
      j=INDEX(hU,'               ')
      if (iMass.eq.1) hU=hU(1:j-1)//' mass'
      Output(1)=xFlg

 550  iFlg=999
      RETURN
 555  iFlg=555
      RETURN
 600  iFlg=600
      q=xnotc
      RETURN
 801  iFlg=801
      RETURN
 998  if (nc.gt.1) then
        iFlg=852
        RETURN
      endif

      end                                             subroutine REFSET2
c
c ======================================================================
c
      subroutine REFSAV (io,iUnits,iMass,iFlag,x,y,x3,q,Dl,Dv,ierr,herr)

      include 'COMMONS.INC'
      character*255 herr,hIn1,hOut1,herrSavRef,hIn1SavRef
      dimension z(ncmax),x(ncmax),y(ncmax),x3(ncmax),x3SavRef(ncmax)
      dimension zSavRef(ncmax),xSavRef(ncmax),ySavRef(ncmax)
      common /REFSV/ aaSavRef,bbSavRef,zSavRef,TSavRef,PSavRef,DSavRef,
     &               DlSavRef,DvSavRef,xSavRef,ySavRef,x3SavRef,qSavRef,
     &               ierrSavRef,iFlgSavRef,iFlg2SavRef,
     &               iFlg3SavRef,hIn1SavRef,herrSavRef
      common /REFSETUP/ z,aa,bb,T,D,P,iFlg,iEHT,hIn1,hOut1

      ierr=0
      if (nc.eq.0) iRefSv=0
      if (io.eq.1) then
        if (iUnits.ne.iUnitNumbRef)   iRefSv=0
        if (iMass.ne.iFlgSavRef)      iRefSv=0
        if (iFlag.ne.iFlg2SavRef)     iRefSv=0
        if (iEHT.ne.iFlg3SavRef)      iRefSv=0
        if (.not.lEqual(aa,aaSavRef)) iRefSv=0
        if (.not.lEqual(bb,bbSavRef)) iRefSv=0
        if (iRefSv.eq.0)              RETURN
        if (hIn1.ne.hIn1SavRef)       iRefSv=0
        if (nc.gt.1) then
          do i=1,nc
            if (.not.lEqual(z(i),zSavRef(i))) iRefSv=0
          enddo
        endif
        if (iRefSv.eq.1) then
          T=TSavRef
          P=PSavRef
          D=DSavRef
          Dl=DlSavRef
          Dv=DvSavRef
          x(1:nc)=xSavRef(1:nc)
          y(1:nc)=ySavRef(1:nc)
          x3(1:nc)=x3SavRef(1:nc)
          q=qSavRef
          ierr=ierrSavRef
          herr=herrSavRef
        endif
      elseif (io.eq.2 .and. nc.gt.0) then
        hIn1SavRef    =hIn1
        iUnitNumbRef  =iUnits
        iFlgSavRef    =iMass
        iFlg2SavRef   =iFlag
        iFlg3SavRef   =iEHT
        aaSavRef      =aa
        bbSavRef      =bb
        zSavRef(1:nc) =z(1:nc)
        TSavRef       =T
        PSavRef       =P
        DSavRef       =D
        DlSavRef      =Dl
        DvSavRef      =Dv
        xSavRef(1:nc) =x(1:nc)
        ySavRef(1:nc) =y(1:nc)
        x3SavRef(1:nc)=x3(1:nc)
        qSavRef       =q
        ierrSavRef    =ierr
        herrSavRef    =herr
      endif

      end                                              subroutine REFSAV
c
c ======================================================================
c
      subroutine GETUNITS (iUnits,iUCode,iFlag,ic,z,ha,xIn,xOut,hUnits,
     &                     ierr,herr)
c
c  Convert the value in xIn from one unit system to another and return
c  the value in xOut.
c
c  Convert the numerical unit code in the iUCode variable into a string
c  with the corresponding text containing the units for a property.
c
c  Units that are built on energy, density, pressure, and temperature can
c  stacked, for example, d^2P/dT^2 would be MPa/K^2 (see the examples
c  in the REFPROP subroutine.)  Other units such as those for viscosity and
c  thermal conductivity cannot be combined or duplicated in this routine.
c  These include:  speed of sound, viscosity, kinematic viscosity,
c  thermal conductivity, surface tension, molar mass, debye, mass flux,
c  and heating value.
c
c  Inputs:
c   iUnits--Integer identifier for the unit system currently defined as
c             explained elsewhere.
c   iUCode--See the REFPROP subroutine.
c    iFlag--Write the unit string to hUnits if not equal to zero.
c        z--Composition array (array of mole fractions)
c      xIn--Input property in molar units similar to all functions in Refprop.
c           (See the PROP_SUB.FOR file for the best examples.)
c
c  Outputs:
c     xOut--Converted property with units based on iUCode.
c   hUnits--String containing the units of the properties in the xOut variable.
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character*255 herr,hUnits,hU,hUn,ha
      character hUnt(0:5)*20,hp*3,hb*5
      dimension iprp(0:4),ispc(0:4),z(ncmax)

      i=0
      ierr=0
      herr=' '
      hUnits=' '
      xOut=xIn
      if (ABS(iUnits).gt.iUUmax) goto 610
      if (iUnitTag(1).eq.0) call REFPRPUNITS     !Initialize all necessary parameters if this is the first time through.

c  Check for properties that are not made of temperature, pressure, density, or energy.
      if (iUCode.lt.0) then
        if (iUCode.eq.iUnitTag(iprpW))     i=iUTypeW
        if (iUCode.eq.iUnitTag(iprpVIS))   i=iUTypeU
        if (iUCode.eq.iUnitTag(iprpTCX))   i=iUTypeK
        if (iUCode.eq.iUnitTag(iprpSTN))   i=iUTypeN
        if (iUCode.eq.iUnitTag(iprpQmole)) i=iUTypeQ
        if (iUCode.eq.iUnitTag(iprpQmass)) i=iUTypeQ
        if (iUCode.eq.iUnitTag(iprpM))     i=iUTypeM
        if (iUCode.eq.iUnitTag(iprpKV))    i=iUTypeI
        if (iUCode.eq.iUnitTag(iprpTMF))   i=iUTypeF       !hUnits='kg/(m^2*s)'
        if (iUCode.eq.iUnitTag(iprpHGVOL)) i=iUTypeG       !hUnits='MJ/m^3'
        if (iUCode.eq.iUnitTag(iprpDIPOLE))i=iUTypeB
        if (i.gt.0) then
          if (iFlag.gt.0) hUnits=hUUnit(iUUnits(i,ABS(iUnits)))           !If found, set the units to the correct values for the input properties.
          if (xIn.eq.xerr .or. xIn.eq.xFlg .or. xIn.eq.xnotc) goto 10     !But first exit if the output variable contains an error code.
          xOut=CONVUNIT(xIn,i,iUUnits(i,iUnits),z,hU,ierr,herr)           !Convert the property.
        endif
        goto 10
      endif

c     Separate the iUCode flag to retrieve each digit as an individual entry in the iprp array.
      iut=ABS(iUCode)
      i=10000
      iprp=0
      do k=4,1,-1
        i=i/10
        iprp(k)=iut/i
        iut=iut-iprp(k)*i
        if (iprp(k).ge.6) iprp(k)=iprp(k)-10
      enddo

c  Use the powers on energy, density, etc. to convert units from molar to mass, etc.
      if (xIn.ne.xerr .and. xIn.ne.xFlg .and. xIn.ne.xnotc) then
        i=iUnits
        k1=iUUnits(iUTypeT,i)
        k2=iUUnits(iUTypeP,i)
        if (k1.eq.iTunitC) k1=iTunitK            !Flags for inputs based on C or F
        if (k1.eq.iTunitF) k1=iTunitR
        if (k2.eq.iPunitPSIG) k2=iPunitPSIA      !Flag for pressure given on a gage scale.
        a1=CONVUNIT(1d0,-ABS(i),k1                ,z,hU,ierr,herr)   !Get the unit string for each property type.
        a2=CONVUNIT(1d0,-ABS(i),k2                ,z,hU,ierr,herr)
        a3=CONVUNIT(1d0,-ABS(i),iUUnits(iUTypeD,i),z,hU,ierr,herr)
        a4=CONVUNIT(1d0,-ABS(i),iUUnits(iUTypeH,i),z,hU,ierr,herr)

        if (iUCode.eq.1 .and. iUUnits(iUTypeT,i).eq.iTUnitC) then    !Special check for degrees C or F conversions that require addition rather than multiplication.
          xOut=xIn-CtoK                !Convert to C
        elseif (iUCode.eq.1 .and. iUUnits(iUTypeT,i).eq.iTUnitF) then
          xOut=(xIn-CtoK)/RtoK+32d0    !Convert to F
        elseif (iUCode.eq.10 .and. iUUnits(iUTypeP,i).eq.iPUnitPSIG)then       !Special check for psig
          xOut=(xIn/1000d0-ATMtoMPa)/PSIAtoMPA             !Convert to psig
        else
          xConv=a1**iprp(1)*a2**iprp(2)*a3**iprp(3)*a4**iprp(4)      !For all others, multiple the conversion values by the power of the property sent in (for example, d^2P/dT^2 would require one pressure conversion factor, and two temperature conversion values, thus meaning it should be squared.)
          xOut=XDIV(xIn,xConv)         !Divide the input value by the conversion value.
        endif
      endif

      if (iFlag.eq.0) RETURN           !Exit if unit strings are not requested.

      if (iprp(3).lt.0) then           !Checks for volume inputs.
        iprp(0)=-iprp(3)               !Turn off the value flag and turn on the density flag to the same but negative power.
        iprp(3)=0
      endif

      hun=' '
      itop=0
      ibot=0
      hUnt(1)=hUUnit(iUUnits(iUTypeT,iUnits))    !Get the units for the five property types.
      hUnt(2)=hUUnit(iUUnits(iUTypeP,iUnits))
      hUnt(3)=hUUnit(iUUnits(iUTypeD,iUnits))
      hUnt(4)=hUUnit(iUUnits(iUTypeH,iUnits))
      hUnt(0)=hUUnit(iUUnits(iUTypeV,iUnits))
      if (hUnt(1).eq.'C' .and. iUCode.ne.1) hUnt(1)='K'              !Switch delta(C) to delta(K) for entropy and heat capacity calculations, for example, kJ/(kg-C) --> kJ/(kg-K).
      if (hUnt(1).eq.'F' .and. iUCode.ne.1) hUnt(1)='R'
      if (hUnt(2).eq.'psig' .and. iUCode.ne.10) hUnt(2)='psia'
      do k=0,4
        i=INDEX(hUnt(k),' ')-1         !Find the first space in the string.
        j=INDEX(hUnt(k),'/')           !Check if a slash is used in the string.
        if (j.gt.0) hUnt(k)='('//hUnt(k)(1:i)//')'         !Add parentheses around the parts in the denominator.
        if (j.gt.0) i=i+2
        ispc(k)=i
        if (iprp(k).gt.0) itop=itop+1            !Add up the number of properties in the numerator and the denominator.
        if (iprp(k).lt.0) ibot=ibot+1
      enddo

      if (itop.eq.0 .and. ibot.gt.0) hun='1'               !If no units are on top, simply start with "1/".
      do j=1,2               !The j loop does the numerator first (j=1) and denominator last (j=2).
        do k=4,0,-1          !The k loop adds the five different type of units.
          if((j.eq.1.and.iprp(k).gt.0).or.(j.eq.2.and.iprp(k).lt.0))then       !Check if a unit needs to be added.
            hp=' '
            if (ABS(iprp(k)).lt.5 .and. ABS(iprp(k)).ne.1)
     &          hp='^'//CHAR(ABS(iprp(k))+48)//'*'         !Add a power sign and its value if necessary.
            i=INDEX(hun,' ')-1                             !Find the first occurrence of a space.
            if (hp.eq.' ') hp='*'                          !Add a multiply sign if no power was needed.
            hun=hun(1:i)//hUnt(k)(1:ispc(k))//hp           !Combine the string already being built with the new unit, its power, and the multiply sign.
          endif
        enddo
        i=INDEX(hun,' ')
        if (j.eq.1 .and. itop.gt.0) hun(i-1:i-1)=' '       !Remove the extra multiply sign at the end for the top section...
        if (j.eq.2 .and. ibot.gt.0) hun(i-1:i-1)=' '       !  ...and then the bottom.
        i=INDEX(hun,' ')
        if (j.eq.2 .and. ibot.gt.1) hun(i:i)=']'           !Add a ']' at the end for the bottom section if more than one unit is in the denominator.
        i=INDEX(hun,' ')-1
        if (j.eq.1 .and. ibot.eq.1) hun=hun(1:i)//'/'      !Add a slash at the end after the first loop...
        if (j.eq.1 .and. ibot.gt.1) hun=hun(1:i)//'/['     !  ...and a bracket if more than one unit is in the denominator.
      enddo
      if (iUCode.eq.1000) hun=hUnt(4)(2:ispc(4)-1)         !Remove parenthesis if output units is only J/mol or kJ/kg.
      if (iUCode.eq.0100) hun=hUnt(3)(2:ispc(3)-1)         !Same for density (mol/dm^3 or kg/m^3).
      if (iUCode.eq.0900) hun=hUnt(0)(2:ispc(0)-1)         !Same for volume.
      if (hun.eq.' ') hun='-'
      hUnits=hun


c  Setup property information in hUnitsArray
  10  if (iFlag.gt.0) then

c  If the property is component specific, write the component number after the label, surrounded by parentheses.
        if (ic.gt.0) then
          i=INDEX(ha,'   ')-1
          if (i.le.0) i=0
          write (hb,'(i5)') ic
          j=5
          if (ic.gt.9) j=4
          ha=ha(1:i)//'('//hb(j:5)//')'
        endif

c  Append the unit to the characters in the hUnitsArray array.
        i=INDEX(ha,'   ')-1
        if (i.eq.50) i=0
        if (i.le.0) i=0
        j=INDEX(hUnits,'  ')
        if (j.eq.0) j=49
        hUnits=ha(1:i)//' {'//hUnits(1:j-1)//'}'
      endif
      RETURN

 610  call ERRNUM (610,0,' ',' ',0d0,0d0,0d0,ierr,herr)

      end                                            subroutine GETUNITS
c
c ======================================================================
c
      function CONVUNIT (xIn,iUnits,iUnitID,z,hUnits,ierr,herr)
c
c  Convert numbers from or to molar SI units for use in Refprop calculations.
c
c  Conversions are based solely on exact unit definitions, thus there
c    is no roundoff due to a lack of significant digits.  For example,
c        BTUtoKJ=CALtoJ*LBMtoKG*RtoK
c    Thus the conversion from BTU to kJ is defined by three exact conversion
c    values, and thus all 16 digits in a double precision variable are significant.
c    However, note that there are several definitions of the calorie, and it
c    may be important that the correct definition be made for your application.
c    This is discussed in the CONSTS.INC file and in the FLAGS routine at the
c    bottom of this file.
c
c  Inputs:
c        xIn--Input value to be converted.
c     iUnits--Unit system, as described in the REFPROP subroutine.
c               Use a negative value to convert to Refprop units, and a positive
c               value to convert to the desired unit.
c    iUnitID--Unit identifier for the input value (e.g., K, F, C, etc.).
c             If iUnits<100 then the identifier is taken from the predefined
c               sets of units, for example, iUnits=1 indicates the molar SI system,
c               iUnits=2 uses a mass based SI system, iUnits=5 uses the English
c               system, and so on.  The enumerations are given at the bottom of the
c               CONSTS.INC file.
c             If iUnits>100 (where the values are taken from the identifiers in
c               the CONSTS.INC file), then conversions are simply based on
c               which identifier is sent to this routine, see examples below.
c               In this case, only a positive or negative value for iUnits matters.
c          z--Composition array (array of mole fractions)
c
c  Outputs:
c   CONVUNIT--Converted output value
c     hUnits--Units of the output value
c       ierr--Error flag:  0 - Successful
c                        601 - Invalid input
c       herr--Error string (character*255)
c
c  The enumerations for the different conversions are listed in the CONSTS.INC
c    file, which can be included in your program, as such:
c      include 'CONSTS.INC'
c  *** Do NOT include any other INC file in your programs ***
c
c  The definitions for iUnitID (as given in CONSTS.INC) for temperature are:
c     iTunitK  --> K
c     iTunitC  --> C
c     iTunitR  --> R
c     iTunitF  --> F
c
c  Others are similar, such as iPunitPSIA, iDunitKG_M3, iSunitBTU_LBM_R,
c    and so on, and are given in the CONSTS.INC file.
c
c  As an example, the following converts 2500 cal/g to BTU/lbm:
c     xJM=CONVUNIT (2500d0,-1,iHunitCAL_G  ,z,hUnits,ierr,herr)      !Convert cal/g to J/mol
c     xBL=CONVUNIT (xJM   , 1,iHunitBTU_LBM,z,hUnits,ierr,herr)      !Convert J/mol to BTU/lbm
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-01-17 EWL, original version, but taken from the CONVUNITS routine.

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character*255 herr*255,hUnits
      dimension z(ncmax)
      parameter (o =1d3)
      parameter (oo=1d6)


      CONVUNIT=xIn
      ierr=0
      herr=' '
      hUnits=' '
      xOut=xIn
      if (ABS(iUnits).gt.iUTMax) goto 610
      wm=WMOL(z)
      iDrct=SIGN(1,-iUnits)
      xDrct=DBLE(iDrct)
      iU=iUnitID                                 !If iU>100 then use its value for the checks below.
      if (iU.le.0) iU=1
      if (iU.lt.100) then                        !If iU<100 then use the values from the predefined unit systems (SI, MOLAR SI, E, etc.) for the checks below.
        if (iU.gt.iUUMax) goto 610
        if (iU.eq.iUTypeE) iU=iUTypeH
        iU=iUUnits(iU,ABS(iUnits))
      endif
      if (iU.lt.1 .or. iU.gt.ihUMax) goto 610
      hUnits=hUUnit(iU)

c-----------------------------------------------------------------------
c   Temperature Conversion
c-----------------------------------------------------------------------
        if (iU.eq.iTunitK) then
        elseif (iU.eq.iTunitC) then
          xOut=xOut+xDrct*CtoK
        elseif (iU.eq.iTunitR) then
          xOut=xOut*RtoK**iDrct
        elseif (iU.eq.iTunitF) then
          if (iDrct.eq.1) then
            xOut=(xOut-32d0)*RtoK+CtoK
          else
            xOut=(xOut-CtoK)/RtoK+32d0
          endif

c-----------------------------------------------------------------------
c   Pressure Conversion
c-----------------------------------------------------------------------
        elseif (iU.eq.iPunitPA) then
          xOut=xOut/oo**iDrct*o**iDrct
        elseif (iU.eq.iPunitKPA) then
          xOut=xOut/o**iDrct*o**iDrct
        elseif (iU.eq.iPunitMPA) then
          xOut=xOut*o**iDrct
        elseif (iU.eq.iPunitGPA) then
          xOut=xOut*o**iDrct*o**iDrct
        elseif (iU.eq.iPunitBAR) then
          xOut=xOut*BARtoMPA**iDrct*o**iDrct
        elseif (iU.eq.iPunitKBAR) then
          xOut=xOut*(BARtoMPA*o)**iDrct*o**iDrct
        elseif (iU.eq.iPunitATM) then
          xOut=xOut*ATMtoMPa**iDrct*o**iDrct
        elseif (iU.eq.iPunitPSIA) then
          xOut=xOut*PSIAtoMPA**iDrct*o**iDrct
        elseif (iU.eq.iPunitPSF) then
          xOut=xOut*(PSIAtoMPA/144d0)**iDrct*o**iDrct
        elseif (iU.eq.iPunitMMHG) then
          xOut=xOut*MMHGtoMPA**iDrct*o**iDrct
        elseif (iU.eq.iPunitINHG) then
          xOut=xOut*(MMHGtoMPA*INtoM*o)**iDrct*o**iDrct
        elseif (iU.eq.iPunitPSIG) then
          if (iDrct.eq.1) then
            xOut=(PSIAtoMPA*xOut+ATMtoMPa)*o
          else
            xOut=(xOut/o-ATMtoMPa)/PSIAtoMPA
          endif

c-----------------------------------------------------------------------
c   Density Conversion
c-----------------------------------------------------------------------
        elseif (iU.eq.iDunitMOL_DM3) then
        elseif (iU.eq.iDunitMOL_CM3) then
          xOut=xOut*o**iDrct
        elseif (iU.eq.iDunitMOL_M3) then
          xOut=xOut/o**iDrct
        elseif (iU.eq.iDunitKG_M3) then
          xOut=xOut/wm**iDrct
        elseif (iU.eq.iDunitKG_DM3) then
          xOut=xOut*(o/wm)**iDrct
        elseif (iU.eq.iDunitG_DM3) then
          xOut=xOut*(1/wm)**iDrct
        elseif (iU.eq.iDunitG_CM3) then
          xOut=xOut*(o/wm)**iDrct
        elseif (iU.eq.iDunitLBM_FT3) then
          xOut=xOut*(LBMtoKG/FT3toM3/wm)**iDrct
        elseif (iU.eq.iDunitLBMOL_FT3) then
          xOut=xOut*(LBMtoKG/FT3toM3)**iDrct
        elseif (iU.eq.iDunitLBMOL_GAL) then
          xOut=xOut*(LBMtoKG/GALLONtoM3)**iDrct
        elseif (iU.eq.iDunitLBM_GAL) then
          xOut=xOut*(LBMtoKG/GALLONtoM3/wm)**iDrct

c-----------------------------------------------------------------------
c   Specific Volume Conversion
c-----------------------------------------------------------------------
        elseif (iU.eq.iVunitDM3_MOL) then
          xOut=XDIV(1d0,xOut)
        elseif (iU.eq.iVunitCM3_MOL) then
          xOut=XDIV(o,xOut)
        elseif (iU.eq.iVunitM3_MOL) then
          xOut=XDIV(1d0,xOut*o)
        elseif (iU.eq.iVunitM3_KG) then
          xOut=XDIV(1d0,xOut*wm)
        elseif (iU.eq.iVunitDM3_KG) then
          xOut=XDIV(o,xOut*wm)
        elseif (iU.eq.iVunitCM3_G) then
          xOut=XDIV(o,xOut*wm)
        elseif (iU.eq.iVunitDM3_G) then
          xOut=XDIV(1d0,xOut*wm)
        elseif (iU.eq.iVunitFT3_LBM) then
          xOut=XDIV(1d0,xOut*wm)*LBMtoKG/FT3toM3
        elseif (iU.eq.iVunitFT3_LBMOL) then
          xOut=XDIV(1d0,xOut)*LBMtoKG/FT3toM3
        elseif (iU.eq.iVunitGAL_LBMOL) then
          xOut=XDIV(1d0,xOut)*LBMtoKG/GALLONtoM3
        elseif (iU.eq.iVunitGAL_LBM) then
          xOut=XDIV(1d0,xOut*wm)*LBMtoKG/GALLONtoM3

c-----------------------------------------------------------------------
c   Specific Energy and Enthalpy Conversions
c-----------------------------------------------------------------------
        elseif (iU.eq.iHunitJ_MOL) then
        elseif (iU.eq.iHunitKJ_MOL) then
          xOut=xOut*o**iDrct
        elseif (iU.eq.iHunitMJ_MOL) then
          xOut=xOut*oo**iDrct
        elseif (iU.eq.iHunitJ_G) then
          xOut=xOut*wm**iDrct
        elseif (iU.eq.iHunitJ_KG) then
          xOut=xOut*(wm/o)**iDrct
        elseif (iU.eq.iHunitCAL_MOL) then
          xOut=xOut*CALtoJ**iDrct
        elseif (iU.eq.iHunitCAL_G) then
          xOut=xOut*(CALtoJ*wm)**iDrct
        elseif (iU.eq.iHunitBTU_LBM) then
          xOut=xOut*(BTUtoKJ/LBMtoKG*wm)**iDrct
        elseif (iU.eq.iHunitBTU_LBMOL) then
          xOut=xOut*(BTUtoKJ/LBMtoKG)**iDrct
c       else
c         goto 610
c       endif

c-----------------------------------------------------------------------
c   Entropy and Heat Capacity Conversions
c-----------------------------------------------------------------------
        elseif (iU.eq.iSunitJ_MOL_K) then
          xOut=xOut
        elseif (iU.eq.iSunitKJ_MOL_K) then
          xOut=xOut*o**iDrct
        elseif (iU.eq.iSunitJ_G_K) then
          xOut=xOut*wm**iDrct
        elseif (iU.eq.iSunitJ_KG_K) then
          xOut=xOut*(wm/o)**iDrct
        elseif (iU.eq.iSunitBTU_LBM_R) then
          xOut=xOut*(BTUtoKJ/LBMtoKG/RtoK*wm)**iDrct
        elseif (iU.eq.iSunitBTU_LBMOL_R) then
          xOut=xOut*(BTUtoKJ/LBMtoKG/RtoK)**iDrct
        elseif (iU.eq.iSunitCAL_G_K) then
          xOut=xOut*(CALtoJ*wm)**iDrct
        elseif (iU.eq.iSunitCAL_MOL_K) then
          xOut=xOut*CALtoJ**iDrct
        elseif (iU.eq.iSunitFT_LBF_LBMOL_R) then
          xOut=xOut*(FTLBFtoJ/LBMtoKG/RtoK/o)**iDrct
        elseif (iU.eq.iSunitCP_R) then
          xOut=xOut*xRgas**iDrct*o

c-----------------------------------------------------------------------
c   Speed of Sound Conversion
c-----------------------------------------------------------------------
        elseif (iU.eq.iWunitM_S) then
        elseif (iU.eq.iWunitCM_S) then
          xOut=xOut/100d0**iDrct
        elseif (iU.eq.iWunitKM_H) then
          xOut=xOut*(o/HtoS)**iDrct
        elseif (iU.eq.iWunitFT_S) then
          xOut=xOut*FTtoM**iDrct
        elseif (iU.eq.iWunitIN_S) then
          xOut=xOut*INtoM**iDrct

c-----------------------------------------------------------------------
c   Viscosity Conversion
c-----------------------------------------------------------------------
        elseif (iU.eq.iVISunitPA_S) then
          xOut=xOut*oo**iDrct
        elseif (iU.eq.iVISunitMPA_S) then        !Note:  This is milliPa-s, not MPa-s
          xOut=xOut/o**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitUPA_S) then
          xOut=xOut/oo**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitG_CM_S) then       !Poise
          xOut=xOut/10**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitCPOISE) then
          xOut=xOut/o**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitMPOISE) then
          xOut=xOut/10000d0**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitUPOISE) then
          xOut=xOut/10000000d0**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitLBM_FT_S) then
          xOut=xOut*(LBMtoKG/FTtoM)**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitLBF_FT2_S) then
          xOut=xOut*(LBFtoN/FTtoM**2)**iDrct*oo**iDrct
        elseif (iU.eq.iVISunitLBM_FT_H) then
          xOut=xOut*(LBMtoKG/FTtoM/HtoS)**iDrct*oo**iDrct

c-----------------------------------------------------------------------
c   Thermal Conductivity Conversion
c-----------------------------------------------------------------------
        elseif (iU.eq.iTCXunitMW_M_K) then
          xOut=xOut/o**iDrct
        elseif (iU.eq.iTCXunitW_M_K) then
          xOut=xOut*o**iDrct/o**iDrct
        elseif (iU.eq.iTCXunitG_CM_S3_K) then
          xOut=xOut/100d0**iDrct/o**iDrct
        elseif (iU.eq.iTCXunitKG_M_S3_K) then
          xOut=xOut*o**iDrct/o**iDrct
        elseif (iU.eq.iTCXunitCAL_S_CM_K) then
          xOut=xOut*(CALtoJ*100000d0)**iDrct/o**iDrct
        elseif (iU.eq.iTCXunitKCAL_H_M_K) then
          xOut=xOut*(CALtoJ*100000d0*o/100d0/3600d0)**iDrct/o**iDrct
        elseif (iU.eq.iTCXunitLBM_FT_S3_F) then
          xOut=xOut*(o*LBMtoKG*FTtoM/RtoK)**iDrct/o**iDrct
        elseif (iU.eq.iTCXunitLBF_S_F) then
          xOut=xOut*(o*LBFtoN/RtoK)**iDrct/o**iDrct
        elseif (iU.eq.iTCXunitBTU_H_FT_F) then
          xOut=xOut*(oo*BTUtoKJ/HtoS/FTtoM/RtoK)**iDrct/o**iDrct

c-----------------------------------------------------------------------
c   Surface Tension Conversion
c-----------------------------------------------------------------------
        elseif (iU.eq.iSTNunitN_M) then
        elseif (iU.eq.iSTNunitMN_M) then
          xOut=xOut/o**iDrct
        elseif (iU.eq.iSTNunitDYNE_CM) then
          xOut=xOut/o**iDrct
        elseif (iU.eq.iSTNunitLBF_FT) then
          xOut=xOut*(LBFtoN/FTtoM)**iDrct

c-----------------------------------------------------------------------
c   Kinematic viscosity
c-----------------------------------------------------------------------
        elseif (iU.eq.iKVunitCM2_S) then
        elseif (iU.eq.iKVunitM2_S) then
          xOut=xOut*100d0**iDrct
        elseif (iU.eq.iKVunitFT2_S) then
          xOut=xOut*(FTtoM**2*10000d0)**iDrct

c-----------------------------------------------------------------------
c   Volumetric heating value
c-----------------------------------------------------------------------
        elseif (iU.eq.iGunitMJ_M3) then
        elseif (iU.eq.iGunitBTU_FT3) then
          xOut=xOut*(BTUtoKJ/FT3toM3/1000d0)**iDrct

c-----------------------------------------------------------------------
c   Molar mass
c-----------------------------------------------------------------------
        elseif (iU.eq.iMunitG_MOL) then
        elseif (iU.eq.iMunitLBM_LBMOL) then
        elseif (iU.eq.iMunitKG_MOL) then
          xOut=xOut*o**iDrct
        endif

      CONVUNIT=Xout
      RETURN

 610  call ERRNUM (610,0,' ',' ',0d0,0d0,0d0,ierr,herr)

      end                                              function CONVUNIT
c
c ======================================================================
c
      function CONVUNITS (xIn,hUnitType,hOldUnit,hNewUnit,wM,ierr,herr)

c  Convert a property value from one unit system to another.  The variable
c  xIn is the value to be converted from hOldUnit to hNewUnit.
c  hUnitType is one of the following letters (one character only in most cases):
c
c  hUnitType     Unit name                          SI units
c    T         Temperature                            K
c    P         Pressure                               MPa
c    D         Density or specific volume             mol/dm^3 (molar) or kg/km^3 (mass)    (for volume, m^3/mol or m^3/kg)
c    H         Enthalpy or specific energy            J/mol (molar) or J/kg (mass)
c    S         Entropy or heat capacity               J/(mol-K) (molar) or J/(kg-K) (mass)
c    W         Speed of sound                         m/s
c    U         Viscosity                              Pa-s
c    K         Thermal conductivity                   mW/m-K
c    JT        Joule Thomson                          K/Pa
c    L         Length                                 m
c    A         Area                                   m^2
c *  V         Volume                                 m^3
c    M         Mass                                   kg
c    F         Force                                  N
c    E         Energy                                 J
c    Q         Power                                  W
c    N         Surface tension                        N/m
c *Caution:  V is volume, not specific volume or inverse density.  For specific volume,
c            simply use D for hUnitType and the code will take of the rest, including
c            converting from specific volume to density or vice versa.
c
c   Gage pressures can be used by adding '_g' to the unit, e.g., 'MPa_g'
c   The optional 5th variable, mW, is used to send the molar mass (molecular weight) if needed.
c   The different inputs for hOldUnit and hNewUnit can be found scattered in the text below
c     or in the REFPROP.XLS Excel file included with the program.
c
c   Examples:
c     T=CONVUNITS(323.15d0,'T','K','F',0,ierr,herr)
c     P=CONVUNITS(1.01325d0,'P','bar','mmHg',0,ierr,herr)
c     V=CONVUNITS(1000.d0,'D','kg/m^3','cm^3/mol',123.456d0,ierr,herr)
c
c   The subroutine works in a two step process.  In the first loop through the code, the input value is
c     converted to SI (shown above).  In the second loop, the SI value is converted to the desired unit.
c     if ierr=0, the unit string for the output variable used by this routine in the comparisons below
c     is returned in herr.
c
c   This routine is not actually used by Refprop but is included for convenience for those needing
c     a simple way of converting from one unit system to another.  It's only use in the Refprop
c     program is through the REFPROP subroutine, which is called by Excel and other applications.
c     In this situation, Excel uses it only for the same purpose, i.e., as a simple conversion tool.
c     All property conversions done by Refprop use the CONVUNIT routine instead to avoid the need
c     for string comparisons.

c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-01-17 EWL, original version (taken from code written by Lemmon in 1989)

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character*3 hTpe
      character*255 herr,hUnit1,hUnit2,hUnit3,UCASE
      character hUnitType*(*),hOldUnit*(*),hNewUnit*(*)

      ierr=0
      herr=' '
      xValue=xIn
      CONVUNITS=xIn
      if (BTUtoKJ.eq.0d0) call FLAGS ('Calorie',0,j,ierr,herr)

      hTpe=UCASE(hUnitType)
      hUnit1=UCASE(hOldUnit)
      hUnit2=UCASE(hNewUnit)
      o=1d3
      oo=1d6
      do k=1,2               !Strip parenthesis, etc., from units.
        if (k.eq.1) hUnit3=hUnit1
        if (k.eq.2) hUnit3=hUnit2
        do i=1,100
          j=INDEX(hUnit3,')/')         !Change (kJ/kg)/K to (kJ/kg)-K, etc.
          if (j.gt.0) then
            hUnit3=hUnit3(:j)//'-'//hUnit3(j+2:)
          else
            j=INDEX(hUnit3,'(')
            if (j.eq.0) j=INDEX(hUnit3,')')
            if (j.gt.0) then
              hUnit3=hUnit3(:j-1)//hUnit3(j+1:)
            else
              j=INDEX(hUnit3,'/')      !Change kJ/kg-K to kJ/kg/K, etc.
              jj=0
              if (j.gt.0) jj=INDEX(hUnit3(j+1:),'-')+j
              if (jj.gt.j) hUnit3=hUnit3(:jj-1)//'/'//hUnit3(jj+1:)
              if (j.ge.jj) j=0
            endif
          endif
          if (j.eq.0) EXIT
        enddo
        j=INDEX(hUnit3,'    ')
        if (j.gt.2) then
          if (hUnit3(j-2:j-1).eq.'/F') hUnit3(j-1:j-1)='R'
          if (hUnit3(j-2:j-1).eq.'/C') hUnit3(j-1:j-1)='K'
        endif
        if (k.eq.1) hUnit1=hUnit3
        if (k.eq.2) hUnit2=hUnit3
      enddo

      WtMol=wm
      If (WtMol.eq.0) WtMol=1d+100     !This will trigger and error below if the molar mass was required but a value of zero was sent.

      do iDrct=1,-1,-2
        xDrct=DBLE(iDrct)
c-----------------------------------------------------------------------
c   Temperature Conversion
c-----------------------------------------------------------------------
        if (hTpe.eq.'T') then
          if (hUnit1.eq.'K') then
          elseif (hUnit1.eq.'C') then
            xValue=xValue+xDrct*CtoK
          elseif (hUnit1.eq.'R') then
            xValue=xValue*RtoK**iDrct
          elseif (hUnit1.eq.'F') then
            if (iDrct.eq.1) then
              xValue=(xValue-32d0)*RtoK+CtoK
            else
              xValue=(xValue-CtoK)/RtoK+32d0
            endif
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Pressure Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'P') then
          iGage=INDEX(hUnit1,'GAGE')
          iVacm=INDEX(hUnit1,'VACM')
          if (iGage.eq.0) iGage=INDEX(hUnit1,'_G')
          if (iVacm.eq.0) iVacm=INDEX(hUnit1,'_V')
          if (iGage.ne.0 .and. iDrct.eq.-1) xValue=xValue-ATMtoMPa
          if (iVacm.ne.0 .and. iDrct.eq.-1) xValue=ATMtoMPa-xValue
          if (iGage.ne.0) hUnit1=hUnit1(1:iGage-1)
          if (iVacm.ne.0) hUnit1=hUnit1(1:iVacm-1)
          i=INDEX(hUnit1,'_ ')
          if (i.gt.0) hUnit1=hUnit1(:i-1)
          if (hUnit1.eq.'PA') then
            xValue=xValue/oo**iDrct
          elseif (hUnit1.eq.'KPA') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'MPA') then
            xValue=xValue
          elseif (hUnit1.eq.'GPA') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'BAR') then
            xValue=xValue*BARtoMPA**iDrct
          elseif (hUnit1.eq.'KBAR') then
            xValue=xValue*(BARtoMPA*o)**iDrct
          elseif (hUnit1.eq.'ATM') then
            xValue=xValue*ATMtoMPa**iDrct
          elseif (hUnit1.eq.'KGF/CM^2' .or. hUnit1.eq.'KG/CM^2' .or.
     &            hUnit1.eq.'ATA' .or. hUnit1.eq.'AT') then
            xValue=xValue*(KGFtoN/100d0)**iDrct
          elseif (hUnit1.eq.'PSI' .or. hUnit1.eq.'PSIA') then
            xValue=xValue*PSIAtoMPA**iDrct
          elseif (hUnit1.eq.'PSF') then
            xValue=xValue*(PSIAtoMPA/144d0)**iDrct
          elseif (hUnit1.eq.'MMHG' .or. hUnit1.eq.'TORR') then
            xValue=xValue*MMHGtoMPA**iDrct
          elseif (hUnit1.eq.'CMHG') then
            xValue=xValue*(MMHGtoMPA*10d0)**iDrct
          elseif (hUnit1.eq.'INHG') then
            xValue=xValue*(MMHGtoMPA*INtoM*o)**iDrct
          elseif (hUnit1.eq.'INH2O') then
            xValue=xValue*INH2OtoMPA**iDrct
          elseif (hUnit1.eq.'PSIG') then
            if (iDrct.eq.1) then
              xValue=PSIAtoMPA*xValue+ATMtoMPa
            else
              xValue=(xValue-ATMtoMPa)/PSIAtoMPA
            endif
          else
            goto 610
          endif
          if (iGage.ne.0 .and. iDrct.eq.1) xValue=xValue+ATMtoMPa
          if (iVacm.ne.0 .and. iDrct.eq.1) xValue=ATMtoMPa-xValue

c-----------------------------------------------------------------------
c   Density Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'D') then
          if (xValue.eq.0) xValue=1d-50
          if (hUnit1.eq.'MOL/DM^3' .or. hUnit1.eq.'MOL/L' .or.
     &        hUnit1.eq.'KMOL/M^3') then
          elseif (hUnit1.eq.'MOL/CM^3' .or. hUnit1.eq.'MOL/CC') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'MOL/M^3') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'KG/M^3') then
            xValue=xValue/WtMol**iDrct
          elseif (hUnit1.eq.'KG/DM^3' .or. hUnit1.eq.'KG/L') then
            xValue=xValue*(o/WtMol)**iDrct
          elseif (hUnit1.eq.'G/DM^3' .or. hUnit1.eq.'G/L') then
            xValue=xValue*(1/WtMol)**iDrct
          elseif (hUnit1.eq.'G/CC' .or. hUnit1.eq.'G/CM^3' .or.
     &            hUnit1.eq.'G/ML') then
            xValue=xValue*(o/WtMol)**iDrct
          elseif (hUnit1.eq.'G/DM^3') then
            xValue=xValue*(1/WtMol)**iDrct
          elseif (hUnit1.eq.'LBM/FT^3' .or. hUnit1.eq.'LB/FT^3') then
            xValue=xValue*(LBMtoKG/FT3toM3/WtMol)**iDrct
          elseif (hUnit1.eq.'LBMOL/FT^3') then
            xValue=xValue*(LBMtoKG/FT3toM3)**iDrct
          elseif (hUnit1.eq.'SLUG/FT^3') then
            xValue=xValue*(LBMtoKG/FT3toM3/WtMol*KGFtoN/FTtoM)**iDrct
          elseif (hUnit1.eq.'LBMOL/GAL') then
            xValue=xValue*(LBMtoKG/GALLONtoM3)**iDrct
          elseif (hUnit1.eq.'LB/GAL' .or. hUnit1.eq.'LBM/GAL') then
            xValue=xValue*(LBMtoKG/GALLONtoM3/WtMol)**iDrct

c-----------------------------------------------------------------------
c   Specific Volume Conversion
c-----------------------------------------------------------------------
          elseif (hUnit1.eq.'DM^3/MOL' .or. hUnit1.eq.'L/MOL' .or.
     &            hUnit1.eq.'M^3/KMOL') then
            xValue=1/xValue
          elseif (hUnit1.eq.'CM^3/MOL' .or. hUnit1.eq.'CC/MOL' .or.
     &            hUnit1.eq.'ML/MOL') then
            xValue=o/xValue
          elseif (hUnit1.eq.'M^3/MOL') then
            xValue=1/xValue/o
          elseif (hUnit1.eq.'M^3/KG') then
            xValue=1/xValue/WtMol
          elseif (hUnit1.eq.'DM^3/KG' .or. hUnit1.eq.'L/KG') then
            xValue=o/xValue/WtMol
          elseif (hUnit1.eq.'CC/G' .or. hUnit1.eq.'CM^3/G' .or.
     &            hUnit1.eq.'ML/G') then
            xValue=o/xValue/WtMol
          elseif (hUnit1.eq.'DM^3/G') then
            xValue=1/xValue/WtMol
          elseif (hUnit1.eq.'FT^3/LBM' .or. hUnit1.eq.'FT^3/LB') then
            xValue=1/xValue*(LBMtoKG/FT3toM3/WtMol)
          elseif (hUnit1.eq.'FT^3/LBMOL') then
            xValue=1/xValue*(LBMtoKG/FT3toM3)
          elseif (hUnit1.eq.'FT^3/SLUG') then
            xValue=1/xValue*(LBMtoKG/FT3toM3/WtMol*KGFtoN/FTtoM)
          elseif (hUnit1.eq.'GAL/LBMOL') then
            xValue=1/xValue*(LBMtoKG/GALLONtoM3)
          elseif (hUnit1.eq.'GAL/LB' .or. hUnit1.eq.'GAL/LBM') then
            xValue=1/xValue*(LBMtoKG/GALLONtoM3/WtMol)
          else
            goto 610
          endif
c         if (Abs(xValue).lt.1d-30) xValue=0d0

c-----------------------------------------------------------------------
c   Specific Energy and Enthalpy Conversions
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'H') then
          if (hUnit1.eq.'J/MOL' .or. hUnit1.eq.'KJ/KMOL') then
          elseif (hUnit1.eq.'KJ/MOL') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'MJ/MOL') then
            xValue=xValue*oo**iDrct
          elseif (hUnit1.eq.'KJ/KG' .or. hUnit1.eq.'J/G') then
            xValue=WtMol**iDrct*xValue
          elseif (hUnit1.eq.'J/KG') then
            xValue=(WtMol/o)**iDrct*xValue
          elseif (hUnit1.eq.'M^2/S^2') then
            xValue=(WtMol/o)**iDrct*xValue
          elseif (hUnit1.eq.'FT^2/S^2') then
            xValue=(WtMol/o*FTtoM**2)**iDrct*xValue
          elseif (hUnit1.eq.'CAL/MOL' .or. hUnit1.eq.'KCAL/KMOL') then
            xValue=CALtoJ**iDrct*xValue
          elseif (hUnit1.eq.'CAL/G' .or. hUnit1.eq.'KCAL/KG') then
            xValue=(CALtoJ*WtMol)**iDrct*xValue
          elseif (hUnit1.eq.'BTU/LBM' .or. hUnit1.eq.'BTU/LB') then
            xValue=(BTUtoKJ/LBMtoKG*WtMol)**iDrct*xValue
          elseif (hUnit1.eq.'BTU/LBMOL') then
            xValue=(BTUtoKJ/LBMtoKG)**iDrct*xValue
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Entropy and Heat Capacity Conversions
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'S') then
          if (hUnit1.eq.'J/MOL/K' .or. hUnit1.eq.'KJ/KMOL/K') then             !This can be entered as J/(mol-K) or (J/mol)/K
            xValue=xValue
          elseif (hUnit1.eq.'KJ/MOL/K') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'KJ/KG/K' .or. hUnit1.eq.'J/G/K') then
            xValue=WtMol**iDrct*xValue
          elseif (hUnit1.eq.'J/KG/K') then
            xValue=(WtMol/o)**iDrct*xValue
          elseif (hUnit1.eq.'BTU/LBM/R' .or. hUnit1.eq.'BTU/LB/R') then
            xValue=(BTUtoKJ/LBMtoKG/RtoK*WtMol)**iDrct*xValue
          elseif(hUnit1.eq.'BTU/LBMOL/R')then
            xValue=(BTUtoKJ/LBMtoKG/RtoK)**iDrct*xValue
          elseif (hUnit1.eq.'CAL/G/K' .or. hUnit1.eq.'KCAL/KG/K') then
            xValue=(CALtoJ*WtMol)**iDrct*xValue
          elseif (hUnit1.eq.'CAL/MOL/K') then
            xValue=CALtoJ**iDrct*xValue
          elseif (hUnit1.eq.'FT/LBF/LBMOL/R') then
            xValue=(FTLBFtoJ/LBMtoKG/RtoK/o)**iDrct*xValue
          elseif (hUnit1.eq.'CP/R') then
            xValue=xRgas**iDrct*xValue*o
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Speed of Sound Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'W') then
          if (hUnit1.eq.'M/S') then
          elseif (hUnit1.eq.'M^2/S^2') then
            xValue=XSQRT(xValue)
          elseif (hUnit1.eq.'CM/S') then
            xValue=xValue/100d0**iDrct
          elseif (hUnit1.eq.'KM/H') then
            xValue=xValue*(o/HtoS)**iDrct
          elseif (hUnit1.eq.'FT/S') then
            xValue=xValue*FTtoM**iDrct
          elseif (hUnit1.eq.'IN/S') then
            xValue=xValue*INtoM**iDrct
          elseif (hUnit1.eq.'MILE/H' .or. hUnit1.eq.'MPH') then
            xValue=xValue*(INtoM*63360d0/HtoS)**iDrct
          elseif (hUnit1.eq.'KNOT') then
            xValue=xValue*0.5144444444d0**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Viscosity Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'U' .or. hTpe.eq.'VIS') then
          if (hUnit1.eq.'PA-S' .or. hUnit1.eq.'KG/M/S') then
          elseif (hUnit1.eq.'MPA-S') then        !Note:  This is milliPa-s, not MPa-s
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'UPA-S') then
            xValue=xValue/oo**iDrct
          elseif (hUnit1.eq.'G/CM/S' .or. hUnit1.eq.'POISE') then
            xValue=xValue/10**iDrct
          elseif (hUnit1.eq.'CENTIPOISE') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'MILLIPOISE' .or. hUnit1.eq.'MPOISE') then
            xValue=xValue/10000d0**iDrct
          elseif (hUnit1.eq.'MICROPOISE' .or. hUnit1.eq.'UPOISE') then
            xValue=xValue/10000000d0**iDrct
          elseif (hUnit1.eq.'LBM/FT/S' .or. hUnit1.eq.'LB/FT/S') then
            xValue=xValue*(LBMtoKG/FTtoM)**iDrct
          elseif (hUnit1.eq.'LBF-S/FT^2') then
            xValue=xValue*(LBFtoN/FTtoM**2)**iDrct
          elseif (hUnit1.eq.'LBM/FT/H' .or. hUnit1.eq.'LB/FT/H') then
            xValue=xValue*(LBMtoKG/FTtoM/HtoS)**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Thermal Conductivity Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'K' .or. hTpe.eq.'TCX') then
          if (hUnit1.eq.'MW/M/K') then
          elseif (hUnit1.eq.'W/M/K') then                  !This can be entered as W/M-K or W/(M-K)
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'G-CM/S^3/K') then
            xValue=xValue/100d0**iDrct
          elseif (hUnit1.eq.'KG-M/S^3/K') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'CAL/S/CM/K') then
            xValue=xValue*(CALtoJ*100000d0)**iDrct
          elseif (hUnit1.eq.'KCAL/H/M/K') then
            xValue=xValue*(CALtoJ*100000d0*o/100d0/3600d0)**iDrct
          elseif (hUnit1.eq.'LBM-FT/S^3/R' .or.
     &            hUnit1.eq.'LB-FT/S^3/R') then
            xValue=xValue*(o*LBMtoKG*FTtoM/RtoK)**iDrct
          elseif (hUnit1.eq.'LBF/S/R') then                          !This can be entered as Lbf/s/F (and all others with /R)
            xValue=xValue*(o*LBFtoN/RtoK)**iDrct
          elseif (hUnit1.eq.'BTU/H/FT/R') then
            xValue=xValue*(oo*BTUtoKJ/HtoS/FTtoM/RtoK)**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Joule-Thomson Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'JT') then
          if (hUnit1.eq.'K/MPA' .or. hUnit1.eq.'C/MPA') then
          elseif (hUnit1.eq.'K/KPA' .or. hUnit1.eq.'C/KPA') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'K/PA' .or. hUnit1.eq.'C/PA') then
            xValue=xValue*oo**iDrct
          elseif (hUnit1.eq.'K/ATM' .or. hUnit1.eq.'C/ATM') then
            xValue=xValue/ATMtoMPa**iDrct
          elseif (hUnit1.eq.'K/BAR' .or. hUnit1.eq.'C/BAR') then
            xValue=xValue/BARtoMPA**iDrct
          elseif (hUnit1.eq.'K/PSI' .or. hUnit1.eq.'K/PSIA') then
            xValue=xValue/PSIAtoMPA**iDrct
          elseif (hUnit1.eq.'F/PSI' .or. hUnit1.eq.'F/PSIA' .or.
     &            hUnit1.eq.'R/PSIA') then
            xValue=xValue/(PSIAtoMPA/RtoK)**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Length Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'L') then
          if (hUnit1.eq.'METER' .or. hUnit1.eq.'METRE' .or.
     &        hUnit1.eq.'M') then
          elseif (hUnit1.eq.'DM') then
            xValue=xValue/10**iDrct
          elseif (hUnit1.eq.'CM') then
            xValue=xValue/100d0**iDrct
          elseif (hUnit1.eq.'MM') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'KM') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'INCH' .or. hUnit1.eq.'IN') then
            xValue=xValue*INtoM**iDrct
          elseif (hUnit1.eq.'FOOT' .or. hUnit1.eq.'FT') then
            xValue=xValue*FTtoM**iDrct
          elseif (hUnit1.eq.'YARD' .or. hUnit1.eq.'YD') then
            xValue=xValue*(INtoM*36d0)**iDrct
          elseif (hUnit1.eq.'MILE' .or. hUnit1.eq.'MI') then
            xValue=xValue*(INtoM*63360d0)**iDrct
          elseif (hUnit1.eq.'LIGHT YEAR') then
            xValue=xValue*9.46055d+15**iDrct
          elseif (hUnit1.eq.'ANGSTROM') then
            xValue=xValue/10000000000d0**iDrct
          elseif (hUnit1.eq.'FATHOM') then
            xValue=xValue*(FTtoM*6d0)**iDrct
          elseif (hUnit1.eq.'MIL') then
            xValue=xValue*(INtoM/o)**iDrct
          elseif (hUnit1.eq.'PARSEC') then
            xValue=xValue*(30837400000000d0*o)**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Area Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'A') then
          if (hUnit1.eq.'METER^2' .or. hUnit1.eq.'METRE^2' .or.
     &         hUnit1.eq.'M^2') then
          elseif (hUnit1.eq.'CM^2') then
            xValue=xValue/10000d0**iDrct
          elseif (hUnit1.eq.'MM^2') then
            xValue=xValue/oo**iDrct
          elseif (hUnit1.eq.'KM^2') then
            xValue=xValue*oo**iDrct
          elseif (hUnit1.eq.'INCH^2' .or. hUnit1.eq.'IN^2') then
            xValue=xValue*(INtoM**2)**iDrct
          elseif (hUnit1.eq.'FOOT^2' .or. hUnit1.eq.'FT^2') then
            xValue=xValue*(FTtoM**2)**iDrct
          elseif (hUnit1.eq.'YARD^2' .or. hUnit1.eq.'YD^2') then
            xValue=xValue*((INtoM*36d0)**2)**iDrct
          elseif (hUnit1.eq.'MILE^2' .or. hUnit1.eq.'MI^2') then
            xValue=xValue*((INtoM*63360d0)**2)**iDrct
          elseif (hUnit1.eq.'ACRE') then
            xValue=xValue*((INtoM*36d0)**2*4840d0)**iDrct
          elseif (hUnit1.eq.'HECTARE') then
            xValue=xValue*10000d0**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Volume Conversion (Note: not specific volume)
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'V') then
          if (hUnit1.eq.'METER^3' .or. hUnit1.eq.'METRE^3' .or.
     &        hUnit1.eq.'M^3') then
          elseif (hUnit1.eq.'CM^3' .or. hUnit1.eq.'ML') then
            xValue=xValue/oo**iDrct
          elseif (hUnit1.eq.'LITER' .or. hUnit1.eq.'LITRE' .or.
     &            hUnit1.eq.'L' .or. hUnit1.eq.'DM^3') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'INCH^3' .or. hUnit1.eq.'IN^3') then
            xValue=xValue*IN3toM3**iDrct
          elseif (hUnit1.eq.'FOOT^3' .or. hUnit1.eq.'FT^3') then
            xValue=xValue*(IN3toM3*12d0**3)**iDrct
          elseif (hUnit1.eq.'YARD^3' .or. hUnit1.eq.'YD^3') then
            xValue=xValue*(IN3toM3*36d0**3)**iDrct
          elseif (hUnit1.eq.'GALLON' .or. hUnit1.eq.'GAL') then
            xValue=xValue*GALLONtoM3**iDrct
          elseif (hUnit1.eq.'IMPGALLON' .or. hUnit1.eq.'IMPGAL') then          !An imperial gallon of liquid is defined as 4.54609 litres.
            xValue=xValue*4.54609d-3**iDrct
          elseif (hUnit1.eq.'QUART' .or. hUnit1.eq.'QT') then
            xValue=xValue*(GALLONtoM3/4d0)**iDrct
          elseif (hUnit1.eq.'PINT' .or. hUnit1.eq.'PT') then
            xValue=xValue*(GALLONtoM3/8d0)**iDrct
          elseif (hUnit1.eq.'CUP') then
            xValue=xValue*(GALLONtoM3/16d0)**iDrct
          elseif (hUnit1.eq.'OUNCE') then
            xValue=xValue*(GALLONtoM3/128d0)**iDrct
          elseif (hUnit1.eq.'TABLESPOON' .or. hUnit1.eq.'TBSP') then
            xValue=xValue*(GALLONtoM3/256d0)**iDrct
          elseif (hUnit1.eq.'TEASPOON' .or. hUnit1.eq.'TSP') then
            xValue=xValue*(GALLONtoM3/768d0)**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Mass Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'M') then
          if (hUnit1.eq.'KG') then
          elseif (hUnit1.eq.'G') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'MG') then                     !milligram
            xValue=xValue/oo**iDrct
          elseif (hUnit1.eq.'LBM' .or. hUnit1.eq.'LB') then
            xValue=xValue*LBMtoKG**iDrct
          elseif (hUnit1.eq.'SLUG') then
            xValue=xValue*(KGFtoN*LBMtoKG/FTtoM)**iDrct
          elseif (hUnit1.eq.'TON') then
            xValue=xValue*(LBMtoKG*2000d0)**iDrct
          elseif (hUnit1.eq.'TONNE') then
            xValue=xValue*o**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Force Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'F') then
          if (hUnit1.eq.'NEWTON' .or. hUnit1.eq.'N') then
          elseif (hUnit1.eq.'MN') then           !milliNewtons
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'KGF') then
            xValue=xValue*KGFtoN**iDrct
          elseif (hUnit1.eq.'DYNE') then
            xValue=xValue/100000d0**iDrct
          elseif (hUnit1.eq.'LBF') then
            xValue=xValue*LBFtoN**iDrct
          elseif (hUnit1.eq.'POUNDAL') then
            xValue=xValue*(LBMtoKG*FTtoM)**iDrct
          elseif (hUnit1.eq.'OZF') then
            xValue=xValue*(LBFtoN/16d0)**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Energy Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'E') then
          if (hUnit1.eq.'JOULE' .or. hUnit1.eq.'J') then
          elseif (hUnit1.eq.'KJ') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'MJ') then
            xValue=xValue*oo**iDrct
          elseif (hUnit1.eq.'KW-H') then
            xValue=xValue*(HtoS*o)**iDrct
          elseif (hUnit1.eq.'CAL') then
            xValue=CALtoJ**iDrct*xValue
          elseif (hUnit1.eq.'KCAL') then
            xValue=xValue*(CALtoJ*o)**iDrct
          elseif (hUnit1.eq.'ERG') then
            xValue=xValue/10000000d0**iDrct
          elseif (hUnit1.eq.'BTU') then
            xValue=xValue*(BTUtoKJ*o)**iDrct
          elseif (hUnit1.eq.'FT-LBF') then
            xValue=xValue*FTLBFtoJ**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Power Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'Q') then
          if (hUnit1.eq.'WATT' .or. hUnit1.eq.'W') then
          elseif (hUnit1.eq.'KWATT' .or. hUnit1.eq.'KW') then
            xValue=xValue*o**iDrct
          elseif (hUnit1.eq.'BTU/S') then
            xValue=xValue*(BTUtoKJ*o)**iDrct
          elseif (hUnit1.eq.'BTU/MIN') then
            xValue=xValue*(BTUtoKJ*o/60d0)**iDrct
          elseif (hUnit1.eq.'BTU/H') then
            xValue=xValue*(BTUtoKJ*o/HtoS)**iDrct
          elseif (hUnit1.eq.'CAL/S') then
            xValue=xValue*CALtoJ**iDrct
          elseif (hUnit1.eq.'KCAL/S') then
            xValue=xValue*(CALtoJ*o)**iDrct
          elseif (hUnit1.eq.'CAL/MIN') then
            xValue=xValue*(CALtoJ/60d0)**iDrct
          elseif (hUnit1.eq.'KCAL/MIN') then
            xValue=xValue*(CALtoJ/60d0*o)**iDrct
          elseif (hUnit1.eq.'FT-LBF/S') then
            xValue=xValue*FTLBFtoJ**iDrct
          elseif (hUnit1.eq.'FT-LBF/MIN') then
            xValue=xValue*(FTLBFtoJ/60d0)**iDrct
          elseif (hUnit1.eq.'FT-LBF/H') then
            xValue=xValue*(FTLBFtoJ/HtoS)**iDrct
          elseif (hUnit1.eq.'HP') then
            xValue=xValue*HPtoW**iDrct
          else
            goto 610
          endif

c-----------------------------------------------------------------------
c   Surface Tension Conversion
c-----------------------------------------------------------------------
        elseif (hTpe.eq.'N' .or. hTpe.eq.'STN') then
          if (hUnit1.eq.'N/M') then
          elseif (hUnit1.eq.'MN/M') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'DYNE/CM' .or. hUnit1.eq.'DYN/CM') then
            xValue=xValue/o**iDrct
          elseif (hUnit1.eq.'LBF/FT') then
            xValue=xValue*(LBFtoN/FTtoM)**iDrct
          else
            goto 610
          endif
        endif
        hUnit1=hUnit2
      enddo
      CONVUNITS=xValue
      if (ABS(xValue).gt.1d50) goto 611
      if (ABS(xValue).lt.1d-50 .and. xValue.ne.0) goto 611
      if (ierr.eq.0) herr=hUnit1
 100  RETURN

 610  call ERRNUM (610,0,'QMASS',' ',0d0,0d0,0d0,ierr,herr)
 611  call ERRNUM (611,0,'QMASS',' ',0d0,0d0,0d0,ierr,herr)

      end                                             function CONVUNITS
c
c ======================================================================
c
      subroutine GETENUM (iFlag,hEnum,iEnum,ierr,herr)
c
c  Translate a string of letters into an integer value that can be used
c  in calls to ALLPROPS0 to increase the speed of property calculations
c  by eliminating string comparisons (which are time expensive in Fortran).
c  This can be done once at the beginning of a program for all properties
c  that will be used, and stored for later use as needed.
c
c  The input strings possible are described in subroutines ALLPROPS and
c  GETUNIT.
c
c  Inputs:
c    iFlag--Flag to specify which type of enumerated value to return:
c           0 - Check all strings possible.
c           1 - Check strings for property units only (e.g., SI, English, etc.).
c           2 - Check property strings and those in #3 only.
c           3 - Check property strings only that are not functions of T and D
c             (for example, the critical point, acentric factor,
c             limits of the EOS, etc.).
c    hEnum--The string that will be used to return the enumerated value.
c             Only uppercase letters are allowed to decrease the time
c             required to process the values.
c
c  Outputs:
c    iEnum--The enumerated value that matches the string sent in hEnum.
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-15-18 EWL, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character hEnum*(*),herr*255,hEnm*255,UCASE*255
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::GETENUM

      ierr=0
      iEnum=xerr
      herr=' '
      hEnm=hEnum
      if (hEnum.eq.' ') RETURN
      i=INDEX(hEnm      ,CHAR(0))-1
      if (i.gt.0) hEnm=hEnm(:i)                  !Get rid of the NUL character and everything else after it.
      i=INDEX(hEnm,'  ')-1
      if (i.le.0) i=LEN(hEnm)
      hEnm=UCASE(hEnm)
c  Check strings for property units only (e.g., SI, English, etc.).
      if (iFlag.eq.0 .or. iFlag.eq.1) then
        if (i.le.2) then
          if (hEnm.eq.'C')            iEnum=iUnitsC
          if (hEnm.eq.'E')            iEnum=iUnitsE
          if (hEnm.eq.'U')            iEnum=iUnitsUser
          if (hEnm.eq.'SI')           iEnum=iUnitsSI
        endif
        if (i.eq.0 .or. (i.ge.3 .and. i.le.5)) then
          if (hEnm.eq.'MKS')          iEnum=iUnitsMKS
          if (hEnm.eq.'CGS')          iEnum=iUnitsCGS
          if (hEnm.eq.'USER')         iEnum=iUnitsUser
          if (hEnm.eq.'USER2')        iEnum=iUnitsUser2
          if (hEnm.eq.'MIXED')        iEnum=iUnitsMix
        endif
        if (i.eq.0 .or. (i.ge.6 .and. i.le.7)) then
          if (hEnm.eq.'MASS SI')      iEnum=iUnitsSI
          if (hEnm.eq.'DEFAULT')      iEnum=iUnitsDefault
          if (hEnm.eq.'MEUNITS')      iEnum=iUnitsMechE
          if (hEnm.eq.'ENGLISH')      iEnum=iUnitsE
          if (hEnm.eq.'MOLARSI')      iEnum=iUnitsMolSI
          if (hEnm.eq.'SIWITHC')      iEnum=iUnitsC
        endif
        if (i.eq.0 .or. i.ge.8) then
          if (hEnm.eq.'MOLAR BASE SI')iEnum=iUnitsBaseSImolar
          if (hEnm.eq.'MOLARBASESI')  iEnum=iUnitsBaseSImolar
          if (hEnm.eq.'MASSBASESI')   iEnum=iUnitsBaseSImass
          if (hEnm.eq.'MASS BASE SI') iEnum=iUnitsBaseSImass
          if (hEnm.eq.'MOLAR SI')     iEnum=iUnitsMolSI
          if (hEnm.eq.'SI WITH C')    iEnum=iUnitsC
          if (hEnm.eq.'MOLAR ENGLISH')iEnum=iUnitsMolE
        endif
        if (iEnum.ne.xerr) goto 100
      endif

c  Check property strings that are a function of T and D.
      j=ICHAR(hEnm(1:1))-64            !Convert the first letter of the string to an integer, starting with 1 for A.
      if (iFlag.eq.0 .or. iFlag.eq.2) then
        if (i.le.1) then
          if (j.le.iLetterH) then
            if (hEnm.eq.'A') iEnum=iprpA
            if (hEnm.eq.'D') iEnum=iprpD
            if (hEnm.eq.'E') iEnum=iprpE
            if (hEnm.eq.'F') iEnum=iprpF
            if (hEnm.eq.'G') iEnum=iprpG
            if (hEnm.eq.'H') iEnum=iprpH
          else
            if (hEnm.eq.'K') iEnum=iprpK
            if (hEnm.eq.'P') iEnum=iprpP
            if (hEnm.eq.'Q') iEnum=iprpQ
            if (hEnm.eq.'S') iEnum=iprpS
            if (hEnm.eq.'T') iEnum=iprpT
            if (hEnm.eq.'V') iEnum=iprpV
            if (hEnm.eq.'W') iEnum=iprpW
            if (hEnm.eq.'Z') iEnum=iprpZ
          endif
        elseif (i.eq.0 .or. i.eq.2) then
          if (j.le.iLetterB) then
            if (hEnm.eq.'A0') iEnum=iprpA0
            if (hEnm.eq.'AE') iEnum=iprpAE
            if (hEnm.eq.'AR') iEnum=iprpAR
            if (hEnm.eq.'BA') iEnum=iprpBA
            if (hEnm.eq.'BS') iEnum=iprpBS
          elseif (j.le.iLetterE) then
            if (hEnm.eq.'CA') iEnum=iprpCA
            if (hEnm.eq.'CP') iEnum=iprpCP
            if (hEnm.eq.'CV') iEnum=iprpCV
            if (hEnm.eq.'D0') iEnum=iprpD0
            if (hEnm.eq.'DE') iEnum=iprpDE
            if (hEnm.eq.'E0') iEnum=iprpE0
            if (hEnm.eq.'EE') iEnum=iprpEE
            if (hEnm.eq.'ER') iEnum=iprpER
          elseif (j.le.iLetterH) then
            if (hEnm.eq.'FC') iEnum=iprpFC
            if (hEnm.eq.'G0') iEnum=iprpG0
            if (hEnm.eq.'GE') iEnum=iprpGE
            if (hEnm.eq.'GR') iEnum=iprpGR
            if (hEnm.eq.'H0') iEnum=iprpH0
            if (hEnm.eq.'HE') iEnum=iprpHE
            if (hEnm.eq.'HG') iEnum=iprpHG
            if (hEnm.eq.'HN') iEnum=iprpHN
            if (hEnm.eq.'HR') iEnum=iprpHR
          elseif (j.le.iLetterP) then
            if (hEnm.eq.'JT') iEnum=iprpJT
            if (hEnm.eq.'KT') iEnum=iprpKT
            if (hEnm.eq.'KV') iEnum=iprpKV
            if (hEnm.eq.'P0') iEnum=iprpP0
            if (hEnm.eq.'PR') iEnum=iprpPR
          elseif (j.le.iLetterT) then
            if (hEnm.eq.'RD') iEnum=iprpRD
            if (hEnm.eq.'S0') iEnum=iprpS0
            if (hEnm.eq.'SE') iEnum=iprpSE
            if (hEnm.eq.'SR') iEnum=iprpSR
            if (hEnm.eq.'TD') iEnum=iprpTD
          else
            if (hEnm.eq.'VE') iEnum=iprpVE
            if (hEnm.eq.'W0') iEnum=iprpW0
          endif
        endif
        if (i.eq.0 .or. i.eq.3) then
          if (j.le.iLetterF) then
            if (hEnm.eq.'API') iEnum=iprpAPI
            if (hEnm.eq.'B12') iEnum=iprpB12
            if (hEnm.eq.'CP0') iEnum=iprpCP0
            if (hEnm.eq.'CPR') iEnum=iprpCPR
            if (hEnm.eq.'CV0') iEnum=iprpCV0
            if (hEnm.eq.'CVR') iEnum=iprpCVR
            if (hEnm.eq.'DEL') iEnum=iprpDEL
            if (hEnm.eq.'ETA') iEnum=iprpVIS
            if (hEnm.eq.'FPV') iEnum=iprpFPV
          else
            if (hEnm.eq.'KKT') iEnum=iprpKKT
            if (hEnm.eq.'P*V') iEnum=iprpPV
            if (hEnm.eq.'PIP') iEnum=iprpPIP
            if (hEnm.eq.'S*D') iEnum=iprpSD
            if (hEnm.eq.'STN') iEnum=iprpSTN
            if (hEnm.eq.'TAU') iEnum=iprpTAU
            if (hEnm.eq.'TCX') iEnum=iprpTCX
            if (hEnm.eq.'TMF') iEnum=iprpTMF
            if (hEnm.eq.'VIS') iEnum=iprpVIS
          endif
        endif
        if (i.eq.0 .or. i.eq.4) then
          if (j.le.iLetterC) then
            if (hEnm.eq.'BETA') iEnum=iprpBETA
            if (hEnm.eq.'BVIR') iEnum=iprpBVIR
            if (hEnm.eq.'CVIR') iEnum=iprpCVIR
            if (hEnm.eq.'CPOT') iEnum=iprpCPOT
            if (hEnm.eq.'CSAT') iEnum=iprpCSAT
            if (hEnm.eq.'CV2P') iEnum=iprpCV2P
          elseif (j.le.iLetterD) then
            if (hEnm.eq.'DADN') iEnum=iprpDADN
            if (hEnm.eq.'DDDP') iEnum=iprpDDDP
            if (hEnm.eq.'DDDT') iEnum=iprpDDDT
            if (hEnm.eq.'DPDD') iEnum=iprpDPDD
            if (hEnm.eq.'DPDT') iEnum=iprpDPDT
            if (hEnm.eq.'DREF') iEnum=iprpDREF
            if (hEnm.eq.'DTDD') iEnum=iprpDTDD
            if (hEnm.eq.'DTDP') iEnum=iprpDTDP
            if (hEnm.eq.'DTRP') iEnum=iprpDTRP
            if (hEnm.eq.'DVIR') iEnum=iprpDVIR
          elseif (j.le.iLetterH) then
            if (hEnm.eq.'ETA0') iEnum=iprpETA0
            if (hEnm.eq.'ETAB') iEnum=iprpETAB2
            if (hEnm.eq.'ETAC') iEnum=iprpETAC
            if (hEnm.eq.'ETAR') iEnum=iprpETAR
            if (hEnm.eq.'EVIR') iEnum=iprpEVIR
            if (hEnm.eq.'GRUN') iEnum=iprpGRUN
            if (hEnm.eq.'HGLQ') iEnum=iprpHGLQ
            if (hEnm.eq.'HNLQ') iEnum=iprpHNLQ
            if (hEnm.eq.'HREF') iEnum=iprpHREF
          elseif (j.le.iLetterS) then
            if (hEnm.eq.'N1/T') iEnum=iprp_1T
            if (hEnm.eq.'P-P0') iEnum=iprpP_P0
            if (hEnm.eq.'PATT') iEnum=iprpPATT
            if (hEnm.eq.'PINT') iEnum=iprpPINT
            if (hEnm.eq.'PREP') iEnum=iprpPREP
            if (hEnm.eq.'RIEM') iEnum=iprpRIEM
            if (hEnm.eq.'SPHT') iEnum=iprpSPHT
            if (hEnm.eq.'SREF') iEnum=iprpSREF
          else
            if (hEnm.eq.'TCX0') iEnum=iprpTCX0
            if (hEnm.eq.'TCXC') iEnum=iprpTCXC
            if (hEnm.eq.'TCXR') iEnum=iprpTCXR
          endif
        endif
        if (i.eq.0 .or. i.eq.5) then
          if (j.le.iLetterH) then
            if (hEnm.eq.'BETAS') iEnum=iprpBETAS
            if (hEnm.eq.'CP/CV') iEnum=iprpCPCV
            if (hEnm.eq.'CSTAR') iEnum=iprpCSTAR
            if (hEnm.eq.'DNADN') iEnum=iprpDNADN
            if (hEnm.eq.'ETAB2') iEnum=iprpETAB2
            if (hEnm.eq.'HGVOL') iEnum=iprpHGVOL
            if (hEnm.eq.'HNVOL') iEnum=iprpHNVOL
          elseif (j.le.iLetterQ) then
            if (hEnm.eq.'ISENK') iEnum=iprpISENK
            if (hEnm.eq.'KAPPA') iEnum=iprpKAPPA
            if (hEnm.eq.'PHASE') iEnum=iprpPHASE
c           if (hEnm.eq.'PMELT') iEnum=iprpMELT_PT
c           if (hEnm.eq.'PSUBL') iEnum=iprpSUBL_PT
            if (hEnm.eq.'QMASS') iEnum=iprpQMASS
            if (hEnm.eq.'QMOLE') iEnum=iprpQMOLE
          else
            if (hEnm.eq.'RDAIR') iEnum=iprpRDAIR
            if (hEnm.eq.'RDH2O') iEnum=iprpRDH2O
c           if (hEnm.eq.'TMELT') iEnum=iprpMELT_TP
c           if (hEnm.eq.'TSUBL') iEnum=iprpSUBL_TP
            if (hEnm.eq.'TRNDJ') iEnum=iprpTRNDJ
            if (hEnm.eq.'TRNTJ') iEnum=iprpTRNTJ
          endif
        endif
        if (i.eq.0 .or. i.eq.6) then
          if (j.le.iLetterE) then
            if (hEnm.eq.'D2DDP2') iEnum=iprpD2DDP2
            if (hEnm.eq.'D2DDPT') iEnum=iprpD2DDPT
            if (hEnm.eq.'D2DDT2') iEnum=iprpD2DDT2
            if (hEnm.eq.'D2PDD2') iEnum=iprpD2PDD2
            if (hEnm.eq.'D2PDT2') iEnum=iprpD2PDT2
            if (hEnm.eq.'D2PDTD') iEnum=iprpD2PDTD
            if (hEnm.eq.'D2TDD2') iEnum=iprpD2TDD2
            if (hEnm.eq.'D2TDP2') iEnum=iprpD2TDP2
            if (hEnm.eq.'D2TDPD') iEnum=iprpD2TDPD
            if (hEnm.eq.'DHDD_P') iEnum=iprpDHDD_P
            if (hEnm.eq.'DHDD_T') iEnum=iprpDHDD_T
            if (hEnm.eq.'DHDP_D') iEnum=iprpDHDP_D
            if (hEnm.eq.'DHDP_T') iEnum=iprpDHDP_T
            if (hEnm.eq.'DHDT_D') iEnum=iprpDHDT_D
            if (hEnm.eq.'DHDT_P') iEnum=iprpDHDT_P
            if (hEnm.eq.'DSDD_P') iEnum=iprpDSDD_P
            if (hEnm.eq.'DSDD_T') iEnum=iprpDSDD_T
            if (hEnm.eq.'DSDP_D') iEnum=iprpDSDP_D
            if (hEnm.eq.'DSDP_T') iEnum=iprpDSDP_T
            if (hEnm.eq.'DSDT_D') iEnum=iprpDSDT_D
            if (hEnm.eq.'DSDT_P') iEnum=iprpDSDT_P
            if (hEnm.eq.'EXERGY') iEnum=iprpEXERGY
          elseif (j.le.iLetterP) then
            if (hEnm.eq.'ISETUP') iEnum=iprpISETUP
            if (hEnm.eq.'PHIG00') iEnum=iprpPHIG00
            if (hEnm.eq.'PHIG01') iEnum=iprpPHIG01
            if (hEnm.eq.'PHIG02') iEnum=iprpPHIG02
            if (hEnm.eq.'PHIG03') iEnum=iprpPHIG03
            if (hEnm.eq.'PHIG10') iEnum=iprpPHIG10
            if (hEnm.eq.'PHIG11') iEnum=iprpPHIG11
            if (hEnm.eq.'PHIG12') iEnum=iprpPHIG12
            if (hEnm.eq.'PHIG20') iEnum=iprpPHIG20
            if (hEnm.eq.'PHIG21') iEnum=iprpPHIG21
            if (hEnm.eq.'PHIG30') iEnum=iprpPHIG30
            if (hEnm.eq.'PHIR00') iEnum=iprpPHIR00
            if (hEnm.eq.'PHIR10') iEnum=iprpPHIR10
            if (hEnm.eq.'PHIR20') iEnum=iprpPHIR20
            if (hEnm.eq.'PHIR30') iEnum=iprpPHIR30
            if (hEnm.eq.'PHIR01') iEnum=iprpPHIR01
            if (hEnm.eq.'PHIR02') iEnum=iprpPHIR02
            if (hEnm.eq.'PHIR03') iEnum=iprpPHIR03
            if (hEnm.eq.'PHIR11') iEnum=iprpPHIR11
            if (hEnm.eq.'PHIR12') iEnum=iprpPHIR12
            if (hEnm.eq.'PHIR13') iEnum=iprpPHIR13
            if (hEnm.eq.'PHIR21') iEnum=iprpPHIR21
            if (hEnm.eq.'PHIR22') iEnum=iprpPHIR22
            if (hEnm.eq.'PHIR23') iEnum=iprpPHIR23
            if (hEnm.eq.'PHIR31') iEnum=iprpPHIR31
            if (hEnm.eq.'PHIR32') iEnum=iprpPHIR32
            if (hEnm.eq.'PHIR33') iEnum=iprpPHIR33
          else
            if (hEnm.eq.'THROTT') iEnum=iprpTHROTT
          endif
        endif
        if (i.eq.0 .or. i.ge.7) then
            if (hEnm.eq.'(Z-1)/D') iEnum=iprpZ1_D
            if (hEnm.eq.'(Z-1)/P') iEnum=iprpZ1_P
            if (hEnm.eq.'CEXERGY') iEnum=iprpCEXERGY
            if (hEnm.eq.'CP0/CV0') iEnum=iprpCP0CV0
            if (hEnm.eq.'DBVIRDT') iEnum=iprpdBvirdT
            if (hEnm.eq.'DCVIRDT') iEnum=iprpdCvirdT
            if (hEnm.eq.'DDVIRDT') iEnum=iprpdDvirdT
            if (hEnm.eq.'DHDZSAT') iEnum=iprpDHDZSAT
            if (hEnm.eq.'DPDTSAT') iEnum=iprpDPDTSAT
            if (hEnm.eq.'ISETREF') iEnum=iprpISETREF
            if (hEnm.eq.'PRANDTL') iEnum=iprpPRANDTL
            if (hEnm.eq.'SUMFACT') iEnum=iprpSUMFACT
            if (hEnm.eq.'LIQSPNDL')  iEnum=iprpLIQSPNDL
            if (hEnm.eq.'VAPSPNDL')  iEnum=iprpVAPSPNDL
            if (hEnm.eq.'HEATVAPZ')  iEnum=iprpHEATVAPZ
            if (hEnm.eq.'D2BVIRDT2') iEnum=iprpd2BvirdT2
            if (hEnm.eq.'D2CVIRDT2') iEnum=iprpd2CvirdT2
            if (hEnm.eq.'D2DVIRDT2') iEnum=iprpd2DvirdT2
            if (hEnm.eq.'HEATVAPZ_T') iEnum=iprpHEATVAPZ_T
            if (hEnm.eq.'HEATVAPZ_P') iEnum=iprpHEATVAPZ_P
            if (hEnm.eq.'VAPORFLUIDSTRING')  iEnum=iprpVAPFLUIDSTRING
            if (hEnm.eq.'LIQUIDFLUIDSTRING') iEnum=iprpLIQFLUIDSTRING
        endif
      endif



c  Check property strings that are not function of T and D
c   (except ancillary equations, i.e., non-EOS properties).
      if (iFlag.eq.0 .or. iFlag.eq.3) then
        if (i.eq.0 .or. i.eq.1) then
          if (hEnm.eq.'M') iEnum=iprpM
          if (hEnm.eq.'R') iEnum=iprpR
          if (hEnm.eq.'X') iEnum=iprpX
        endif
        if (i.eq.0 .or. i.eq.2) then
          if (hEnm.eq.'DC') iEnum=iprpDC
          if (hEnm.eq.'MM') iEnum=iprpMM
          if (hEnm.eq.'PC') iEnum=iprpPC
          if (hEnm.eq.'TC') iEnum=iprpTC
        endif
        if (i.eq.0 .or. i.eq.3 .or. i.eq.4) then
          if (j.le.iLetterO) then
          if (hEnm.eq.'ACF')  iEnum=iprpACF
          if (hEnm.eq.'CAS#') iEnum=iprpCAS
          if (hEnm.eq.'DRED') iEnum=iprpDRED
          if (hEnm.eq.'DMAX') iEnum=iprpDMAX
          if (hEnm.eq.'FDIR') iEnum=iprpFDIR
          if (hEnm.eq.'GWP')  iEnum=iprpGWP
          if (hEnm.eq.'HASH') iEnum=iprpHASH
          if (hEnm.eq.'HFRM') iEnum=iprpHFRM
          if (hEnm.eq.'NAME') iEnum=iprpNAME
          if (hEnm.eq.'ODP')  iEnum=iprpODP
          else
          if (hEnm.eq.'PMAX') iEnum=iprpPMAX
          if (hEnm.eq.'PTRP') iEnum=iprpPTRP
          if (hEnm.eq.'REOS') iEnum=iprpREOS
          if (hEnm.eq.'TMAX') iEnum=iprpTMAX
          if (hEnm.eq.'TMIN') iEnum=iprpTMIN
          if (hEnm.eq.'TNBP') iEnum=iprpTNBP
          if (hEnm.eq.'TRED') iEnum=iprpTRED
          if (hEnm.eq.'TREF') iEnum=iprpTREF
          if (hEnm.eq.'TTRP') iEnum=iprpTTRP
          endif
        endif
        if (i.eq.0 .or. i.eq.5) then
          if (j.le.iLetterI) then
          if (hEnm.eq.'ALTID') iEnum=iprpALTID
          if (hEnm.eq.'DCEST') iEnum=iprpDCEST
          if (hEnm.eq.'DCRIT') iEnum=iprpDCRIT
          if (hEnm.eq.'DMAXP') iEnum=iprpDMAXP
          if (hEnm.eq.'DMAXT') iEnum=iprpDMAXT
          if (hEnm.eq.'INCHI') iEnum=iprpINCHI
          elseif (j.le.iLetterP) then
          if (hEnm.eq.'NCOMP') iEnum=iprpNCOMP
          if (hEnm.eq.'PCEST') iEnum=iprpPCEST
          if (hEnm.eq.'PCRIT') iEnum=iprpPCRIT
          if (hEnm.eq.'PMAXP') iEnum=iprpPMAXP
          if (hEnm.eq.'PMAXT') iEnum=iprpPMAXT
          else
          if (hEnm.eq.'TCEST') iEnum=iprpTCEST
          if (hEnm.eq.'TCRIT') iEnum=iprpTCRIT
          if (hEnm.eq.'TMAXP') iEnum=iprpTMAXP
          if (hEnm.eq.'TMAXT') iEnum=iprpTMAXT
          if (hEnm.eq.'XMASS') iEnum=iprpXMASS
          if (hEnm.eq.'XMOLE') iEnum=iprpXMOLE
          endif
        endif
        if (i.eq.0 .or. i.ge.6) then
          if (hEnm(4:4).eq.'_') then         !Check for flags such as DOI_EOS or WEB_TCX.
            k=-1
            if (hEnm(1:3).eq.'DOI') k=0
            if (hEnm(1:3).eq.'WEB') k=iprpFlags
            if (k.ge.0) then
              if (hEnm(5:7).eq.'EOS') iEnum=iprp_EOS+k
              if (hEnm(5:7).eq.'VIS') iEnum=iprp_VIS+k
              if (hEnm(5:7).eq.'TCX') iEnum=iprp_TCX+k
              if (hEnm(5:7).eq.'ECS') iEnum=iprp_ECS+k
              if (hEnm(5:7).eq.'STN') iEnum=iprp_STN+k
              if (hEnm(5:7).eq.'DIE') iEnum=iprp_DIE+k
              if (hEnm(5:7).eq.'MLT') iEnum=iprp_MLT+k
              if (hEnm(5:7).eq.'SBL') iEnum=iprp_SBL+k
            endif
          elseif (j.le.iLetterE) then
          if (hEnm.eq.'ANC-DT')      iEnum=iprpANC_DT
          if (hEnm.eq.'ANC-PT')      iEnum=iprpANC_PT
          if (hEnm.eq.'ANC-TDL')     iEnum=iprpANC_TDL
          if (hEnm.eq.'ANC-TDV')     iEnum=iprpANC_TDV
          if (hEnm.eq.'ANC-TP')      iEnum=iprpANC_TP
          if (hEnm.eq.'CHEMFORM')    iEnum=iprpCHEMFORM
          if (hEnm.eq.'DCTRUE')      iEnum=iprpDCTRUE
          if (hEnm.eq.'DIPOLE')      iEnum=iprpDIPOLE
          elseif (j.le.iLetterJ) then
          if (hEnm.eq.'FAMILY')      iEnum=iprpFAMILY
          if (hEnm.eq.'FIJMIX')      iEnum=iprpFIJMIX
          if (hEnm.eq.'FLDNAME')     iEnum=iprpFLDNAME
          if (hEnm.eq.'FULLCHEMFORM')iEnum=iprpFULLCHEMFORM
          if (hEnm.eq.'HEATCOMB')    iEnum=iprpHEATCOMB
          if (hEnm.eq.'INCHIKEY')    iEnum=iprpINCHIKEY
          elseif (j.le.iLetterR) then
          if (hEnm.eq.'LONGNAME')    iEnum=iprpLONGNAME
          if (hEnm.eq.'MELT-PT')     iEnum=iprpMELT_PT
          if (hEnm.eq.'MELT-TP')     iEnum=iprpMELT_TP
          if (hEnm.eq.'REFSTATE')    iEnum=iprpREFSTATE
          else
          if (hEnm.eq.'SAFETY')      iEnum=iprpSAFETY
          if (hEnm.eq.'SUBL-PT')     iEnum=iprpSUBL_PT
          if (hEnm.eq.'SUBL-TP')     iEnum=iprpSUBL_TP
          if (hEnm.eq.'SYNONYM')     iEnum=iprpSYNONYM
          if (hEnm.eq.'TCTRUE')      iEnum=iprpTCTRUE
          if (hEnm.eq.'UNNUMBER')    iEnum=iprpUNNUMBER
          endif
        endif
      endif

 100  if (ABS(iEnum).gt.100000) then
        call ERRNUM (811,0,hEnm,' ',0d0,0d0,0d0,ierr,herr)
      endif

      end                                             subroutine GETENUM
c
c ======================================================================
c
      subroutine REFPRPUNITS ()
      include 'COMMONS.INC'
      include 'COMREFP.INC'

c  Set tags for properties that are not made solely of temperature,
c   pressure, density, or energy.
      iUnitTag=0
      iUnitTag(iprpW            ) = -iUTypeW
      iUnitTag(iprpVIS          ) = -iUTypeU
      iUnitTag(iprpTCX          ) = -iUTypeK
      iUnitTag(iprpSTN          ) = -iUTypeN
      iUnitTag(iprpM            ) = -iUTypeM
      iUnitTag(iprpKV           ) = -iUTypeI
      iUnitTag(iprpTMF          ) = -iUTypeF
      iUnitTag(iprpHGVOL        ) = -iUTypeG
      iUnitTag(iprpDIPOLE       ) = -iUTypeB

c  Set tags for properties that are based only on temperature,
c   pressure, density, or energy.
      iUnitTag(iprpT            ) = 0001
      iUnitTag(iprpP            ) = 0010
      iUnitTag(iprpD            ) = 0100
      iUnitTag(iprpV            ) = 0900
      iUnitTag(iprpE            ) = 1000
      iUnitTag(iprpH            ) = 1000
      iUnitTag(iprpS            ) = 1009
      iUnitTag(iprpCV           ) = 1009
      iUnitTag(iprpCP           ) = 1009
      iUnitTag(iprpCPCV         ) = 0
      iUnitTag(iprpCP0CV0       ) = 0
      iUnitTag(iprpZ            ) = 0
      iUnitTag(iprpJT           ) = 0091
      iUnitTag(iprpA            ) = 1000
      iUnitTag(iprpG            ) = 1000
      iUnitTag(iprpR            ) = 1009
      iUnitTag(iprpKAPPA        ) = 0090
      iUnitTag(iprpBETA         ) = 0009
      iUnitTag(iprpISENK        ) = 0
      iUnitTag(iprpKT           ) = 0
      iUnitTag(iprpBETAS        ) = 0090
      iUnitTag(iprpBS           ) = 0010
      iUnitTag(iprpKKT          ) = 0010
      iUnitTag(iprpTHROTT       ) = 0900
      iUnitTag(iprpDPDD         ) = 0910
      iUnitTag(iprpDPDT         ) = 0019
      iUnitTag(iprpDDDP         ) = 0190
      iUnitTag(iprpDDDT         ) = 0109
      iUnitTag(iprpDTDP         ) = 0091
      iUnitTag(iprpDTDD         ) = 0901
      iUnitTag(iprpD2PDD2       ) = 0810
      iUnitTag(iprpD2PDT2       ) = 0018
      iUnitTag(iprpD2PDTD       ) = 0919
      iUnitTag(iprpD2DDP2       ) = 0180
      iUnitTag(iprpD2DDT2       ) = 0108
      iUnitTag(iprpD2DDPT       ) = 0199
      iUnitTag(iprpD2TDP2       ) = 0081
      iUnitTag(iprpD2TDD2       ) = 0801
      iUnitTag(iprpD2TDPD       ) = 0991
      iUnitTag(iprpDHDT_D       ) = 1009
      iUnitTag(iprpDHDT_P       ) = 1009
      iUnitTag(iprpDHDD_T       ) = 1900
      iUnitTag(iprpDHDD_P       ) = 1900
      iUnitTag(iprpDHDP_T       ) = 1090
      iUnitTag(iprpDHDP_D       ) = 1090
      iUnitTag(iprpDSDT_D       ) = 1008
      iUnitTag(iprpDSDT_P       ) = 1008
      iUnitTag(iprpDSDD_T       ) = 1909
      iUnitTag(iprpDSDD_P       ) = 1909
      iUnitTag(iprpDSDP_T       ) = 1099
      iUnitTag(iprpDSDP_D       ) = 1099
      iUnitTag(iprpBVir         ) = 0900
      iUnitTag(iprpCVir         ) = 0800
      iUnitTag(iprpDVir         ) = 0700
      iUnitTag(iprpEVir         ) = 0600
      iUnitTag(iprpdBvirdT      ) = 0909
      iUnitTag(iprpd2BvirdT2    ) = 0908
      iUnitTag(iprpdCvirdT      ) = 0809
      iUnitTag(iprpd2CvirdT2    ) = 0808
      iUnitTag(iprpdDvirdT      ) = 0709
      iUnitTag(iprpd2DvirdT2    ) = 0708
      iUnitTag(iprpBA           ) = 0900
      iUnitTag(iprpCA           ) = 0800
      iUnitTag(iprpGRUN         ) = 0
      iUnitTag(iprpPIP          ) = 0
      iUnitTag(iprpRIEM         ) = 0
      iUnitTag(iprpZ1_D         ) = 0900
      iUnitTag(iprpZ1_P         ) = 0090
      iUnitTag(iprpPV           ) = 0910
      iUnitTag(iprpSD           ) = 1109
      iUnitTag(iprp_1T          ) = 0009
      iUnitTag(iprpRD           ) = 0100
      iUnitTag(iprpANC_TP       ) = 0010
      iUnitTag(iprpANC_TDL      ) = 0100
      iUnitTag(iprpANC_TDV      ) = 0100
      iUnitTag(iprpANC_PT       ) = 0001
      iUnitTag(iprpANC_DT       ) = 0001
      iUnitTag(iprpMELT_TP      ) = 0010
      iUnitTag(iprpMELT_PT      ) = 0001
      iUnitTag(iprpSUBL_TP      ) = 0010
      iUnitTag(iprpSUBL_PT      ) = 0001
      iUnitTag(iprpCSAT         ) = 1009
      iUnitTag(iprpCV2P         ) = 1009
      iUnitTag(iprpDPDTSAT      ) = 0019
      iUnitTag(iprpDHDZSAT      ) = 1000
      iUnitTag(iprpLIQSPNDL     ) = 0100
      iUnitTag(iprpVAPSPNDL     ) = 0100
      iUnitTag(iprpVE           ) = 0900
      iUnitTag(iprpEE           ) = 1000
      iUnitTag(iprpHE           ) = 1000
      iUnitTag(iprpSE           ) = 1009
      iUnitTag(iprpAE           ) = 1000
      iUnitTag(iprpGE           ) = 1000
      iUnitTag(iprpB12          ) = 0900
      iUnitTag(iprpP0           ) = 0010
      iUnitTag(iprpD0           ) = 0100
      iUnitTag(iprpE0           ) = 1000
      iUnitTag(iprpH0           ) = 1000
      iUnitTag(iprpS0           ) = 1009
      iUnitTag(iprpCV0          ) = 1009
      iUnitTag(iprpCP0          ) = 1009
      iUnitTag(iprpW0           ) = iUnitTag(iprpW)
      iUnitTag(iprpA0           ) = 1000
      iUnitTag(iprpG0           ) = 1000
      iUnitTag(iprpPR           ) = 0010
      iUnitTag(iprpER           ) = 1000
      iUnitTag(iprpHR           ) = 1000
      iUnitTag(iprpSR           ) = 1009
      iUnitTag(iprpCVR          ) = 1009
      iUnitTag(iprpCPR          ) = 1009
      iUnitTag(iprpAR           ) = 1000
      iUnitTag(iprpGR           ) = 1000
      iUnitTag(iprpPHIG00       ) = 0
      iUnitTag(iprpPHIG10       ) = 0
      iUnitTag(iprpPHIG20       ) = 0
      iUnitTag(iprpPHIG30       ) = 0
      iUnitTag(iprpPHIG01       ) = 0
      iUnitTag(iprpPHIG02       ) = 0
      iUnitTag(iprpPHIG03       ) = 0
      iUnitTag(iprpPHIG11       ) = 0
      iUnitTag(iprpPHIG12       ) = 0
      iUnitTag(iprpPHIG21       ) = 0
      iUnitTag(iprpPHIR00       ) = 0
      iUnitTag(iprpPHIR10       ) = 0
      iUnitTag(iprpPHIR20       ) = 0
      iUnitTag(iprpPHIR30       ) = 0
      iUnitTag(iprpPHIR01       ) = 0
      iUnitTag(iprpPHIR02       ) = 0
      iUnitTag(iprpPHIR03       ) = 0
      iUnitTag(iprpPHIR11       ) = 0
      iUnitTag(iprpPHIR12       ) = 0
      iUnitTag(iprpPHIR13       ) = 0
      iUnitTag(iprpPHIR21       ) = 0
      iUnitTag(iprpPHIR22       ) = 0
      iUnitTag(iprpPHIR23       ) = 0
      iUnitTag(iprpPHIR31       ) = 0
      iUnitTag(iprpPHIR32       ) = 0
      iUnitTag(iprpPHIR33       ) = 0
      iUnitTag(iprpTC           ) = 0001
      iUnitTag(iprpPC           ) = 0010
      iUnitTag(iprpDC           ) = 0100
      iUnitTag(iprpTMAXT        ) = 0001
      iUnitTag(iprpPMAXT        ) = 0010
      iUnitTag(iprpDMAXT        ) = 0100
      iUnitTag(iprpTMAXP        ) = 0001
      iUnitTag(iprpPMAXP        ) = 0010
      iUnitTag(iprpDMAXP        ) = 0100
      iUnitTag(iprpTCEST        ) = 0001
      iUnitTag(iprpPCEST        ) = 0010
      iUnitTag(iprpDCEST        ) = 0100
      iUnitTag(iprpTRED         ) = 0001
      iUnitTag(iprpDRED         ) = 0100
      iUnitTag(iprpTAU          ) = 0
      iUnitTag(iprpDEL          ) = 0
      iUnitTag(iprpTMIN         ) = 0001
      iUnitTag(iprpTMAX         ) = 0001
      iUnitTag(iprpDMAX         ) = 0100
      iUnitTag(iprpPMAX         ) = 0010
      iUnitTag(iprpPRANDTL      ) = 0
      iUnitTag(iprpTD           ) = iUnitTag(iprpKV)
      iUnitTag(iprpDE           ) = 0
      iUnitTag(iprpSPHT         ) = 1000
      iUnitTag(iprpHEATVAPZ     ) = 1000
      iUnitTag(iprpHEATVAPZ_T   ) = 1000
      iUnitTag(iprpHEATVAPZ_P   ) = 1000
      iUnitTag(iprpHEATCOMB     ) = 1000
      iUnitTag(iprpHFRM         ) = 1000
      iUnitTag(iprpHG           ) = 1000
      iUnitTag(iprpHN           ) = 1000
      iUnitTag(iprpHGLQ         ) = 1000
      iUnitTag(iprpHNLQ         ) = 1000
      iUnitTag(iprpHNVOL        ) = iUnitTag(iprpHGVOL)
      iUnitTag(iprpPINT         ) = 0010
      iUnitTag(iprpPREP         ) = 0010
      iUnitTag(iprpPATT         ) = 0010
      iUnitTag(iprpP_P0         ) = 0010
      iUnitTag(iprpEXERGY       ) = 1000
      iUnitTag(iprpCEXERGY      ) = 1000
      iUnitTag(iprpCSTAR        ) = 0
      iUnitTag(iprpFPV          ) = 0
      iUnitTag(iprpSUMFACT      ) = 0
      iUnitTag(iprpRDAIR        ) = 0
      iUnitTag(iprpRDH2O        ) = 0
      iUnitTag(iprpAPI          ) = 0
      iUnitTag(iprpTCRIT        ) = 0001
      iUnitTag(iprpPCRIT        ) = 0010
      iUnitTag(iprpDCRIT        ) = 0100
      iUnitTag(iprpTCTRUE       ) = 0001
      iUnitTag(iprpDCTRUE       ) = 0100
      iUnitTag(iprpTTRP         ) = 0001
      iUnitTag(iprpPTRP         ) = 0010
      iUnitTag(iprpDTRP         ) = 0100
      iUnitTag(iprpTNBP         ) = 0001
      iUnitTag(iprpREOS         ) = 1009
      iUnitTag(iprpMM           ) = iUnitTag(iprpM)
      iUnitTag(iprpACF          ) = 0
      iUnitTag(iprpGWP          ) = 0
      iUnitTag(iprpODP          ) = 0
      iUnitTag(iprpSAFETY       ) = 0
      iUnitTag(iprpTREF         ) = 0001
      iUnitTag(iprpDREF         ) = 0100
      iUnitTag(iprpHREF         ) = 1000
      iUnitTag(iprpSREF         ) = 1009
      iUnitTag(iprpETA0         ) = iUnitTag(iprpVIS)
      iUnitTag(iprpETAB2        ) = iUnitTag(iprpVIS)
      iUnitTag(iprpETAR         ) = iUnitTag(iprpVIS)
      iUnitTag(iprpETAC         ) = iUnitTag(iprpVIS)
      iUnitTag(iprpTCX0         ) = iUnitTag(iprpTCX)
      iUnitTag(iprpTCXR         ) = iUnitTag(iprpTCX)
      iUnitTag(iprpTCXC         ) = iUnitTag(iprpTCX)
      iUnitTag(iprpTRNTJ        ) = 0001
      iUnitTag(iprpTRNDJ        ) = 0100
      iUnitTag(iprpK            ) = 0000
      iUnitTag(iprpF            ) = 0010
      iUnitTag(iprpFC           ) = 0
      iUnitTag(iprpCPOT         ) = 1000
      iUnitTag(iprpDADN         ) = 0
      iUnitTag(iprpDNADN        ) = 0
      iUnitTag(iprpX            ) = 0000
      iUnitTag(iprpXMOLE        ) = 0000
      iUnitTag(iprpXMASS        ) = 0000
      iUnitTag(iprpQMOLE        ) = iUnitTag(iprpQ)
      iUnitTag(iprpQMASS        ) = iUnitTag(iprpQ)

c-----------------------------------------------------------------------
c   Set up unit systems.
c-----------------------------------------------------------------------

c     iUTypeT --> Temperature
c     iUTypeP --> Pressure
c     iUTypeD --> Density
c     iUTypeV --> Volume
c     iUTypeH --> Energy or enthalpy
c     iUTypeS --> Entropy
c     iUTypeW --> Speed of sound
c     iUTypeI --> Kinematic viscosity
c     iUTypeU --> Viscosity
c     iUTypeK --> Thermal conductivity
c     iUTypeN --> Surface tension
c     iUType0 --> No units (e.g., Z)

      iUUnits=0d0

c  Refprop default units - Do not change the next 16 lines!
      iUUnits(iUTypeT,0)=iTunitK
      iUUnits(iUTypeP,0)=iPunitKPA
      iUUnits(iUTypeD,0)=iDunitMOL_DM3
      iUUnits(iUTypeV,0)=iVunitDM3_MOL
      iUUnits(iUTypeH,0)=iHunitJ_MOL
      iUUnits(iUTypeS,0)=iSunitJ_MOL_K
      iUUnits(iUTypeW,0)=iWunitM_S
      iUUnits(iUTypeI,0)=iKVunitCM2_S
      iUUnits(iUTypeU,0)=iVISunitUPA_S
      iUUnits(iUTypeK,0)=iTCXunitW_M_K
      iUUnits(iUTypeN,0)=iSTNunitN_M
      iUUnits(iUType0,0)=i0unit0
      iUUnits(iUTypeM,0)=iMunitG_MOL
      iUUnits(iUTypeQ,0)=iQunitMole
      iUUnits(iUTypeB,0)=iBunitDEBYE
      iUUnits(iUTypeF,0)=iFunitKG_M2_S
      iUUnits(iUTypeG,0)=iGunitMJ_M3

c  Common units
      do i=1,30
        iUUnits(iUTypeT,i)=iTunitK
        iUUnits(iUTypeP,i)=iPunitMPA
        iUUnits(iUTypeD,i)=iDunitKG_M3
        iUUnits(iUTypeV,i)=iVunitM3_KG
        iUUnits(iUTypeH,i)=iHunitJ_G
        iUUnits(iUTypeS,i)=iSunitJ_G_K
        iUUnits(iUTypeW,i)=iWunitM_S
        iUUnits(iUTypeI,i)=iKVunitCM2_S
        iUUnits(iUTypeU,i)=iVISunitUPA_S
        iUUnits(iUTypeK,i)=iTCXunitMW_M_K
        iUUnits(iUTypeN,i)=iSTNunitMN_M
        iUUnits(iUType0,i)=i0unit0
        iUUnits(iUTypeM,i)=iMunitG_MOL
        iUUnits(iUTypeQ,i)=iQunitMass
        iUUnits(iUTypeB,i)=iBunitDEBYE
        iUUnits(iUTypeF,i)=iFunitKG_M2_S
        iUUnits(iUTypeG,i)=iGunitMJ_M3
      enddo

c  SI molar units
      iUUnits(iUTypeD,iUnitsMolSI)=iDunitMOL_DM3
      iUUnits(iUTypeV,iUnitsMolSI)=iVunitDM3_MOL
      iUUnits(iUTypeH,iUnitsMolSI)=iHunitJ_MOL
      iUUnits(iUTypeS,iUnitsMolSI)=iSunitJ_MOL_K

c  SI base units (molar)
      iUUnits(iUTypeP,iUnitsBaseSImolar)=iPunitPA
      iUUnits(iUTypeD,iUnitsBaseSImolar)=iDunitMOL_M3
      iUUnits(iUTypeV,iUnitsBaseSImolar)=iVunitM3_MOL
      iUUnits(iUTypeH,iUnitsBaseSImolar)=iHunitJ_MOL
      iUUnits(iUTypeS,iUnitsBaseSImolar)=iSunitJ_MOL_K
      iUUnits(iUTypeI,iUnitsBaseSImolar)=iKVunitM2_S
      iUUnits(iUTypeU,iUnitsBaseSImolar)=iVISunitPA_S
      iUUnits(iUTypeK,iUnitsBaseSImolar)=iTCXunitW_M_K
      iUUnits(iUTypeN,iUnitsBaseSImolar)=iSTNunitN_M
      iUUnits(iUTypeM,iUnitsBaseSImolar)=iMunitKG_MOL

c  SI base units (mass)
      iUUnits(iUTypeP,iUnitsBaseSImass)=iPunitPA
      iUUnits(iUTypeD,iUnitsBaseSImass)=iDunitKG_M3
      iUUnits(iUTypeV,iUnitsBaseSImass)=iVunitM3_KG
      iUUnits(iUTypeH,iUnitsBaseSImass)=iHunitJ_KG
      iUUnits(iUTypeS,iUnitsBaseSImass)=iSunitJ_KG_K
      iUUnits(iUTypeI,iUnitsBaseSImass)=iKVunitM2_S
      iUUnits(iUTypeU,iUnitsBaseSImass)=iVISunitPA_S
      iUUnits(iUTypeK,iUnitsBaseSImass)=iTCXunitW_M_K
      iUUnits(iUTypeN,iUnitsBaseSImass)=iSTNunitN_M
      iUUnits(iUTypeM,iUnitsBaseSImass)=iMunitKG_MOL

c  SI mass units
c     See units above under "Typical units"

c  SI units with celsius
      iUUnits(iUTypeT,iUnitsC)=iTunitC

c  English units
      iUUnits(iUTypeT,iUnitsE)=iTunitF
      iUUnits(iUTypeP,iUnitsE)=iPunitPSIA
      iUUnits(iUTypeD,iUnitsE)=iDunitLBM_FT3
      iUUnits(iUTypeV,iUnitsE)=iVunitFT3_LBM
      iUUnits(iUTypeH,iUnitsE)=iHunitBTU_LBM
      iUUnits(iUTypeS,iUnitsE)=iSunitBTU_LBM_R
      iUUnits(iUTypeW,iUnitsE)=iWunitFT_S
      iUUnits(iUTypeI,iUnitsE)=iKVunitFT2_S
      iUUnits(iUTypeU,iUnitsE)=iVISunitLBM_FT_S
      iUUnits(iUTypeK,iUnitsE)=iTCXunitBTU_H_FT_F
      iUUnits(iUTypeN,iUnitsE)=iSTNunitLBF_FT
      iUUnits(iUTypeM,iUnitsE)=iMunitLBM_LBMOL
      iUUnits(iUTypeG,iUnitsE)=iGunitBTU_FT3

c  Molar English units
      iUUnits(iUTypeT,iUnitsMolE)=iTunitF
      iUUnits(iUTypeP,iUnitsMolE)=iPunitPSIA
      iUUnits(iUTypeD,iUnitsMolE)=iDunitLBMOL_FT3
      iUUnits(iUTypeV,iUnitsMolE)=iVunitFT3_LBMOL
      iUUnits(iUTypeH,iUnitsMolE)=iHunitBTU_LBMOL
      iUUnits(iUTypeS,iUnitsMolE)=iSunitBTU_LBMOL_R
      iUUnits(iUTypeW,iUnitsMolE)=iWunitFT_S
      iUUnits(iUTypeI,iUnitsMolE)=iKVunitFT2_S
      iUUnits(iUTypeU,iUnitsMolE)=iVISunitLBM_FT_S
      iUUnits(iUTypeK,iUnitsMolE)=iTCXunitBTU_H_FT_F
      iUUnits(iUTypeN,iUnitsMolE)=iSTNunitLBF_FT
      iUUnits(iUTypeM,iUnitsMolE)=iMunitLBM_LBMOL
      iUUnits(iUTypeQ,iUnitsMolE)=iQunitMole
      iUUnits(iUTypeG,iUnitsMolE)=iGunitBTU_FT3

c  mks units
      iUUnits(iUTypeP,iUnitsMKS)=iPunitKPA
      iUUnits(iUTypeK,iUnitsMKS)=iTCXunitW_M_K

c  cgs units
      iUUnits(iUTypeD,iUnitsCGS)=iDunitG_CM3
      iUUnits(iUTypeV,iUnitsCGS)=iVunitCM3_G
      iUUnits(iUTypeH,iUnitsCGS)=iHunitJ_G
      iUUnits(iUTypeS,iUnitsCGS)=iSunitJ_G_K
      iUUnits(iUTypeW,iUnitsCGS)=iWunitCM_S
      iUUnits(iUTypeN,iUnitsCGS)=iSTNunitDYNE_CM

c  Mixed units
      iUUnits(iUTypeP,iUnitsMix)=iPunitPSIA
      iUUnits(iUTypeD,iUnitsMix)=iDunitG_CM3
      iUUnits(iUTypeV,iUnitsMix)=iVunitCM3_G

c  Mechanical engineering units
      iUUnits(iUTypeT,iUnitsMechE)=iTunitC
      iUUnits(iUTypeP,iUnitsMechE)=iPunitBAR
      iUUnits(iUTypeD,iUnitsMechE)=iDunitG_CM3
      iUUnits(iUTypeV,iUnitsMechE)=iVunitCM3_G
      iUUnits(iUTypeW,iUnitsMechE)=iWunitCM_S
      iUUnits(iUTypeU,iUnitsMechE)=iVISunitCPOISE

c-----------------------------------------------------------------------
c   Set up unit strings.
c-----------------------------------------------------------------------

      hUUnit=' '
      hUUnit(iTunitK)             = 'K'
      hUUnit(iTunitC)             = 'C'
      hUUnit(iTunitR)             = 'R'
      hUUnit(iTunitF)             = 'F'
      hUUnit(iPunitPA)            = 'Pa'
      hUUnit(iPunitKPA)           = 'kPa'
      hUUnit(iPunitMPA)           = 'MPa'
      hUUnit(iPunitGPA)           = 'GPa'
      hUUnit(iPunitBAR)           = 'bar'
      hUUnit(iPunitKBAR)          = 'kbar'
      hUUnit(iPunitATM)           = 'atm'
      hUUnit(iPunitPSIA)          = 'psia'
      hUUnit(iPunitPSF)           = 'psf'
      hUUnit(iPunitMMHG)          = 'mmHg'
      hUUnit(iPunitINHG)          = 'inHg'
      hUUnit(iPunitPSIG)          = 'psig'
      hUUnit(iDunitMOL_DM3)       = 'mol/dm^3'
      hUUnit(iDunitMOL_CM3)       = 'mol/cm^3'
      hUUnit(iDunitMOL_M3)        = 'mol/m^3'
      hUUnit(iDunitKG_M3)         = 'kg/m^3'
      hUUnit(iDunitKG_DM3)        = 'kg/dm^3'
      hUUnit(iDunitG_DM3)         = 'g/dm^3'
      hUUnit(iDunitG_CM3)         = 'g/cm^3'
      hUUnit(iDunitLBM_FT3)       = 'lbm/ft^3'
      hUUnit(iDunitLBMOL_FT3)     = 'lbmol/ft^3'
      hUUnit(iDunitLBMOL_GAL)     = 'lbmol/gal'
      hUUnit(iDunitLBM_GAL)       = 'lbm/gal'
      hUUnit(iVunitDM3_MOL)       = 'dm^3/mol'
      hUUnit(iVunitCM3_MOL)       = 'cm^3/mol'
      hUUnit(iVunitM3_MOL)        = 'm^3/mol'
      hUUnit(iVunitM3_KG)         = 'm^3/kg'
      hUUnit(iVunitDM3_KG)        = 'dm^3/kg'
      hUUnit(iVunitCM3_G)         = 'cm^3/g'
      hUUnit(iVunitDM3_G)         = 'dm^3/g'
      hUUnit(iVunitFT3_LBM)       = 'ft^3/lbm'
      hUUnit(iVunitFT3_LBMOL)     = 'ft^3/lbmol'
      hUUnit(iVunitGAL_LBMOL)     = 'gal/lbmol'
      hUUnit(iVunitGAL_LBM)       = 'gal/lbm'
      hUUnit(iHunitJ_MOL)         = 'J/mol'
      hUUnit(iHunitKJ_MOL)        = 'kJ/mol'
      hUUnit(iHunitMJ_MOL)        = 'MJ/mol'
      hUUnit(iHunitJ_G)           = 'kJ/kg'
      hUUnit(iHunitJ_KG)          = 'J/kg'
      hUUnit(iHunitCAL_MOL)       = 'cal/mol'
      hUUnit(iHunitCAL_G)         = 'cal/g'
      hUUnit(iHunitBTU_LBM)       = 'Btu/lbm'
      hUUnit(iHunitBTU_LBMOL)     = 'Btu/lbmol'
      hUUnit(iSunitJ_MOL_K)       = 'J/(mol-K)'
      hUUnit(iSunitKJ_MOL_K)      = 'kJ/(mol-K)'
      hUUnit(iSunitJ_G_K)         = 'kJ/(kg-K)'
      hUUnit(iSunitJ_KG_K)        = 'J/(kg-K)'
      hUUnit(iSunitBTU_LBM_R)     = 'Btu/(lbm-R)'
      hUUnit(iSunitBTU_LBMOL_R)   = 'Btu/(lbmol-R)'
      hUUnit(iSunitCAL_G_K)       = 'cal/(g-K)'
      hUUnit(iSunitCAL_MOL_K)     = 'cal/(mol-K)'
      hUUnit(iSunitFT_LBF_LBMOL_R)= 'ft/(lbf-lbmol-R)'
      hUUnit(iSunitCP_R)          = 'Cp/R'
      hUUnit(iWunitM_S)           = 'm/s'
      hUUnit(iWunitCM_S)          = 'cm/s'
      hUUnit(iWunitKM_H)          = 'km/h'
      hUUnit(iWunitFT_S)          = 'ft/s'
      hUUnit(iWunitIN_S)          = 'in/s'
      hUUnit(iKVunitM2_S)         = 'm^2/s'
      hUUnit(iKVunitCM2_S)        = 'cm^2/s'
      hUUnit(iKVunitFT2_S)        = 'ft^2/s'
      hUUnit(iVISunitPA_S)        = 'Pa-s'
      hUUnit(iVISunitMPA_S)       = 'mPa-s'
      hUUnit(iVISunitUPA_S)       = 'uPa-s'
      hUUnit(iVISunitG_CM_S)      = 'g/(cm-s)'
      hUUnit(iVISunitCPOISE)      = 'cpoise'
      hUUnit(iVISunitMPOISE)      = 'mpoise'
      hUUnit(iVISunitUPOISE)      = 'upoise'
      hUUnit(iVISunitLBM_FT_S)    = 'lbm/(ft-s)'
      hUUnit(iVISunitLBM_FT_H)    = 'lbm/(ft-h)'
      hUUnit(iVISunitLBF_FT2_S)   = 'lbf/(ft2-s)'
      hUUnit(iTCXunitMW_M_K)      = 'mW/(m-K)'
      hUUnit(iTCXunitW_M_K)       = 'W/(m-K)'
      hUUnit(iTCXunitG_CM_S3_K)   = 'g-cm/(s^3-K)'
      hUUnit(iTCXunitKG_M_S3_K)   = 'kg-m/(s^3-K)'
      hUUnit(iTCXunitCAL_S_CM_K)  = 'cal/(s-cm-K)'
      hUUnit(iTCXunitKCAL_H_M_K)  = 'kcal/(h-m-K)'
      hUUnit(iTCXunitLBM_FT_S3_F) = 'lbm-ft/(s^3-R)'
      hUUnit(iTCXunitLBF_S_F)     = 'lbf/(s-F)'
      hUUnit(iTCXunitBTU_H_FT_F)  = 'Btu/(h-ft-R)'
      hUUnit(iSTNunitN_M)         = 'N/m'
      hUUnit(iSTNunitMN_M)        = 'mN/m'
      hUUnit(iSTNunitDYNE_CM)     = 'dyne/cm'
      hUUnit(iSTNunitLBF_FT)      = 'lbf/ft'
      hUUnit(i0unit0)             = '-'
      hUUnit(iMunitG_MOL)         = 'g/mol'
      hUUnit(iMunitKG_MOL)        = 'kg/mol'
      hUUnit(iMunitLBM_LBMOL)     = 'lbm/lbmol'
      hUUnit(iQunitMole)          = 'mole fraction'
      hUUnit(iQunitMass)          = 'mass fraction'
      hUUnit(iBunitDEBYE)         = 'debye'
      hUUnit(iFunitKG_M2_S)       = 'kg/(m^2*s)'
      hUUnit(iGunitMJ_M3)         = 'MJ/m^3'
      hUUnit(iGunitBTU_FT3)       = 'BTU/ft^3'

      end                                         subroutine REFPRPUNITS
c
c ======================================================================
c
      subroutine PHASE (xPhase,hPhase)

c  Use the quality or other integer identifier to return a more human
c   understandable definition of the fluid state.

      double precision xPhase
      character*255 hPhase

      if (xPhase.gt.0d0 .and. xPhase.lt.1d0) then
        hPhase='Two-phase'
      elseif (xPhase.gt.1d0 .and. xPhase.lt.100d0) then
        hPhase='Superheated gas'
      elseif (xPhase.lt.0d0 .and. xPhase.gt.-100d0) then
        hPhase='Subcooled liquid'
      elseif (xPhase.eq.0d0) then
        hPhase='Saturated liquid'
      elseif (xPhase.eq.1d0) then
        hPhase='Saturated vapor'
      elseif (xPhase.eq. 999) then
        hPhase='Supercritical'
      elseif (xPhase.eq. 998) then
        hPhase='Superheated gas'
      elseif (xPhase.eq.-998) then
        hPhase='Subcooled liquid'
      elseif (xPhase.eq. 997) then
        hPhase='Saturated'
      elseif (xPhase.eq.-997) then
        hPhase='Supercritical'
      elseif (xPhase.eq. 920) then
        hPhase='Metastable'
      else
        write (hPhase,'(i9)') INT(xPhase)
        hPhase='Error code missing: '//hPhase
      endif

      end                                               subroutine PHASE
c
c ======================================================================
c
      subroutine READFLUIDSTRING (hFldIn,hFld,iMassX,iMix,iNew,ncc,z,
     &                            ierr,herr)

c  This is an internal routine not meant for use other than from
c  calls by the REFPROP subroutine.  The routine extracts the fluid
c  names and compositions from the hFldIn string and sends the fluid
c  names back in hFld, and the compositions in the z array.  If the
c  word "mass" in included after the text in the hFldIn string, the
c  parameter iMassX is set to 1.  Be careful to see the Notes below.
c
c  Input:
c   hFldIn--Input string containing the fluid names and compositions.
c           Examples:
c           'methane'
c           'methane;ethane'
c           'methane;0.5;ethane;0.5'
c           'methane;0.5;ethane;0.5 mass'
c           'methane;ethane|0.5;0.5'
c           'methane;ethane ~ 0.5;0.5 mass'    (note that either of the symbols | or ~ can be used)
c           'methane;ethane;isobutane;1,2-butadiene|0.8;0.1;0.07;0.03'
c           ***Note:  Once this routine (i.e., the REFPROP routine)
c             has been called, if compositions are sent as well as fluid
c             names, the routine will require a "significant" amount of
c             time to do the string to number conversions.  It is best
c             to not include the compositions once they are known if
c             possible.
c           The fastest method if compositions are not sent is to place
c             the symbol | or ~ at the end of the fluid string to avoid
c             attempting to read compositions.
c  Outputs:
c     hFLD--String containing only the fluid names found in hFldIn,
c           which can be used directly in calls to SETFLUIDS.
c   iMassX--0 for molar composition, 1 for mass composition
c           (which depends solely on whether the word "mass" is
c           included in the input string).
c     iMix--1 for a *.MIX file, otherwise 0.
c     iNew--Flag to indicate one of three things:
c           0 - No change from last call.
c               Caution #1:  The composition array does not get updated,
c                 so if z has changed outside this routine, it remains the
c                 same and does not contain the compositions sent in hFldIn.
c               Caution #2:  No other outputs are set (aside from being
c                 initialized to zero) when the input string has not changed.
c           1 - New input string is different from previous, unless:
c           2 - New input string contains the same fluids as before,
c               but different compositions, which have been loaded into
c               the z array.
c             ----Do NOT call SETUP or SETFLUIDS if iNew=0 or 2----
c      ncc--Number of components in the mixture.
c        z--Composition array for the fluids in the mixture (array of mole fractions).
c           Compositions are sent back exactly as contained in the input string,
c           the conversion to a mass or molar basis must be done outside this routine.
c           ***Note:  Compositions sent to this routine are overwritten by the
c           compositions in the hFldIn string.
c     ierr--Error flag:  0 - No errors.
c                      906 - Error in input string.
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-22-17 EWL, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character herr*255,hFldIn*(*),UCASE
      character*10000 hFldX,hFld,UCASE10000,htemp
      dimension z(ncmax),zz(ncmax)

      ierr=0
      herr=' '
      ncc=nc
      iNew=-1
      iMassX=0
      iMix=0
      hFld=hFldIn
      zz=0
      if (ncPDmix.ne.0 .and. nc.eq.ncPDmix .and. hFld.ne.' ') then !Check if a predefined mixture has been loaded.
        htemp=UCASE10000(hFld)
        i=INDEX(htemp,'.MIX')
        if (i.gt.0) htemp=htemp(1:i-1)
        if (htemp.eq.hPDmix) then
          if (SUM(ABS(z(1:nc))).le.0.d0) then
            z(1:nc)=zPDmix(1:nc)
          else
            zSum=SUM(ABS(z(1:nc)-zPDmix(1:nc)))
            if (.not.lEQ0(zSum)) then
              call ERRNUM (813,0,'REFPROP',' ',0d0,0d0,0d0,ierr,herr)
              RETURN
            endif
          endif
        endif
      endif
      if (hFld.eq.hFldOld .and. iReset.eq.0) RETURN
      hFldOld=hFld
      if (hFld.eq.' ') RETURN
      hFldX=hFldIn
      iFldMix=0
      iNew=1
      ncc=0
      htemp=UCASE10000(hFldX)
      if (INDEX(htemp,'.MIX').ne.0) iMix=1       !Check for a predefined mixture.
      j=INDEX(htemp,'MASS')            !Check for the word "mass" at the end of the input line.
      if (j.gt.0) then
        iMassX=1
        hFldX=hFldX(:j-1)
      endif
      i=INDEX(hFldX,'|')
      if (i.eq.0) i=INDEX(hFldX,'~')
      if (i.gt.0) then
        hFld=hFldX(1:i-1)
        hFldX=hFldX(i+1:)
        if (hFldX.ne.' ') then
          do j=1,ncmax
            ncc=ncc+1
            i=INDEX(hFldX,';')
            m=INDEX(hFldX,'*')
            if (i.eq.0 .or. (m.lt.i .and. m.gt.0)) i=m
            if (i.eq.0) then
              read (hFldX,*,err=906,end=906) zz(ncc)
              EXIT
            endif
            read (hFldX(:i-1),*,err=906,end=906) zz(ncc)
            hFldX=hFldX(i+1:)
          enddo
        endif
      elseIf (INDEX(hFldX,';').gt.0 .or. INDEX(hFldX,'*').gt.0) then
        k=0
        hFld=' '
        do j=1,ncmax
          i=INDEX(hFldX,';')
          m=INDEX(hFldX,'*')
          if (i.eq.0 .or. (m.lt.i .and. m.gt.0)) i=m
          if (i.eq.0) i=INDEX(hFldX,'         ')
          ncc=ncc+1
          hFld=hFld(1:k)//hFldX(1:i-1)//';'
          k=k+i
          hFldX=hFldX(i+1:)
          i=INDEX(hFldX,';')
          m=INDEX(hFldX,'*')
          if (i.eq.0 .or. (m.lt.i .and. m.gt.0)) i=m
          if (i.eq.0) i=INDEX(hFldX,'         ')
          zz(ncc)=0
          read (hFldX(:i-1),*,err=642,end=642) zz(ncc)
          hFldX=hFldX(i+1:)
 642      if (hFldX.eq.' ') EXIT
        enddo
        if (k.gt.0) hFld(k:k)=' '
      else
        ncc=1
        j=INDEX(hFldX,'.')
        if (j.eq.0) then               !Check for predefined refrigerant mixtures (e.g., R410A, R501, etc.) that do not have '.mix' appended.
          i=INDEX(hFldX,'    ')
          if (i.eq.5 .or. i.eq.6) then           !Make sure the user has not entered something like R41 (a pure fluid).
            if (hFldX(1:2).eq.'R4' .or. hFldX(1:2).eq.'R5' .or.
     &          hFldX(1:2).eq.'r4' .or. hFldX(1:2).eq.'r5') then
              hFldX=hFldX(1:i-1)//'.MIX'
              iMix=1
            endif
          endif
        endif
        hFld=hFldX
      endif
c     if (hFld.eq.hFldOld) iNew=2

      if (ncc.gt.1) then
        zSum=SUM(zz(1:ncc))
        if (ABS(zSum-1d0).gt.1d-4) then
          if (ABS(zSum-100d0).lt.1.d-2) then
            zz(1:ncc)=zz(1:ncc)/100d0
          elseif (zSum.eq.0d0) then
                   !Assume that the composition was sent in the z array, not in the fluid string.
          else
            hFldOld=' '
            call ERRNUM (805,0,'REFPROP',' ',0d0,0d0,0d0,ierr,herr)
          endif
        endif
        if (zSum.gt.0) z(1:ncc)=zz(1:ncc)
      endif
      RETURN

 906  call ERRNUM (906,0,' ',' ',0d0,0d0,0d0,ierr,herr)
      ncc=0
      hFldOld=' '
      end                                     subroutine READFLUIDSTRING
c
c ======================================================================
c
      subroutine FLDSETUP (hFld,iMass,iFlag,z,zm,ierr,herr)

c  This is an internal routine and not meant for use other than from
c  calls by the REFPROP subroutine.
c
c  Input:
c     hFld--Input string containing the fluid names and compositions
c           (see subroutine READFLUIDSTRING for examples and for
c           information on calculation speed).
c    iMass--Set to 0 when zm is sent on a molar basis, and to 1 when
c           zm is sent on a mass basis.
c    iFlag--If set to one, call SATSPLN after doing other setup calls.
c       zm--Input composition array.
c           ----Warning:  If hFld has changed from the last call, the values
c           in zm are replaced with those in the hFld string.
c
c  Outputs:
c       zm--Fluid composition array (see Warning above).
c        z--The z array is set equal to the zm array, and converted
c           to a molar basis if iMass=1.  For a predefined mixture,
c           the z array is set to the composition of the mixture as
c           defined in the *.MIX file.
c     ierr--Error flag:  0 - No errors.
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  11-22-17 EWL, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      character*255 herr,herr2,herr3,hFl
      character hFld*(*),hFLdX*10000
      dimension z(ncmax),zm(ncmax)

      ierr=0
      if (iMass.ne.iMassOld .and. iMassOld.ne.-1) iReset=1
      iMassOld=iMass
      call FLAGS ('PR',-999,iPReos,ierr3,herr3)

c  Check if the fluid has changed and load the new composition.
      call READFLUIDSTRING(hFld,hFldX,iMassX,iMix,iNew,ncc,zm,ierr,herr)
      if (ierr.gt.0 .and. ierr.ne.805) RETURN
      z(1:ncc)=zm(1:ncc)               !Set z to the input composition prior to doing other checks that might reset it.
      if (iNew.eq.-1) then
        if (iMass.eq.1 .or. iMassX.eq.1) call XMOLE (zm,z,wmix)
      endif

      if (ncc.gt.1) then
        zSum=SUM(z(1:ncc))
        if (ABS(zSum-1d0).gt.1d-4) then
          if (ABS(zSum-100d0).lt.1.d-2) then
            z(1:ncc)=z(1:ncc)/100d0
          elseif (zSum.eq.0d0) then
                   !Assume that the composition will be sent later.
          else
            call ERRNUM (805,0,'REFPROP',' ',0d0,0d0,0d0,ierr,herr)
          endif
        endif
      endif
      if (iNew.eq.-1) then
        if (iReset.eq.0 .or. hFld.eq.' ') goto 110
      endif

c  Setup various flags.
      iRefSv=0
      iPRold=0

c  Call SETUP if a new fluid has been requested (not just a new composition).
      hFl=hFldOld
      if (iMix.gt.0) then    !Check if a predefined mixture was sent.
        call SETMIXTURE (hFldX,z,ierr)
        if (ierr.ne.0) call ErrMsg (ierr,herr)
        iFldMix=1
        zm(1:nc)=z(1:nc)
        if (iMass.eq.1) call XMASS (z,zm,wmix)   !If requested with iMass=1, but the composition in the hFld string was sent on a mole basis, convert it to a mass basis.
        iMassOld=iMass
      else
        call SETFLUIDS (hFldX,ierr)              !Call the routine that sets up the variables required for the call to SETUP.
        if (iMassX.eq.0 .and. iMass.eq.1) call XMASS (z,zm,wmix)     !  ...and then use those values to overwrite the input composition sent in zm with the molar values if iMass=0.
        if (iMassX.eq.1) call XMOLE (zm,z,wmix)  !Convert the composition values to a molar basis that are now in the zm array, and that were in the input string sent to REFPROP, along with the word "mass" at the end of the string.
        if (iMassX.eq.1 .and. iMass.eq.0) zm(1:ncc)=z(1:ncc)   !  ...and then use those values to overwrite the input composition sent in zm with the molar values if iMass=0.
        iFldMix=0
      endif
      hFldOld=' '
      if (ierr.ne.0) call ERRMSG (ierr,herr)
      if (ierr.gt.0) RETURN     !Exit if SETUP failed.
      hFldOld=hFl

c  Set up default outputs.
      iRefSv=0
      if (iSetRef2.gt.0) then          !Call SETREF
        call SETREF (hRef3,iSetRef2,z,
     &               h0Ref3,s0Ref3,T0Ref3,P0Ref3,ierr2,herr2)
        ierr2=-ABS(ierr2)    !Don't quit due to errors from SETREF.
        call SWAPERR (ierr,ierr2,herr,herr2)
      endif

c  Reset various options that would have been reset if SETUP was called.
 100  if (iPReos.ne.iPRold) then
        iRefSv=0
        call FLAGS ('PR',iPReos,i,ierr3,herr3)   !Call PR if it was in use before.
      endif
      iPRold=iPReos
 110  if (iFlag.eq.1 .and. (hFld.ne.' '.or.SUM(zSpln(1:nc)).eq.0.)) then
        call SATSPLN (z,ierr2,herr2)
        call SWAPERR (ierr,ierr2,herr,herr2)
      endif

      end                                            subroutine FLDSETUP
c
c ======================================================================
c
      subroutine SETFLUIDS (hfld,ierr)
c
c  Call the SETUP routine without the need to pass ncomp, hrf, hFmix, or herr,
c  or to declare the length of hfld as 255 or 10000 bytes long.  For a pure
c  fluid, hfld simply contains the name of the fluid file (with a path if
c  needed).  For a mixture, it contains the names of the constituents in the
c  mixture separated by a ``|``, a semicolon, or an asterisk.  To load a
c  predefined mixture, call the SETMIXTURE subroutine (which must return
c  the composition array and thus cannot be included here).  If it is
c  necessary to set the reference state, call SETUP instead.  If ierr
c  comes back non-zero, call the ERRMSG routine to obtain it.
c
c  Examples::
c
c      call SETFLUIDS ('ARGON',ierr)    (load argon as a pure fluid)
c      call SETFLUIDS ('FLUIDS/NITROGEN.FLD|FLUIDS/ARGON.FLD|FLUIDS/OXYGEN.FLD|',ierr)  (for the air mixture, but giving a path as well)
c      call SETFLUIDS ('AIR.PPF',ierr)  (load the air mixture, but read from the pseudo-pure file; properties will be slightly different from the *.mix file since they are different models)
c      call SETFLUIDS ('methane * ethane * propane * butane',ierr)
c
c  Inputs:
c     hfld--String of any character length containing the fluid file names
c
c  Outputs:
c     ierr--Error flag:  0 - Successful
c           (Values are identical to SETUP; a 109 is returned if the number
c           of fluids in hfld is less than icomp.)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  02-28-17 EWL, original version

      include 'COMMONS.INC'
      character hFld*(*),hrf*3,hfl*10000
      character*255 hf(ncmax),hFmix,herr
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETFLUIDS

      hf=' '
      hrf=' '
      hfl=hfld
      hFmix='HMX.BNC'
      call STRINGSPLIT ((ncmax),hfl,';*#|',k,hf,ierr,herr)
      if (k.lt.1 .or. k.gt.ncmax .or. ierr.gt.100) then
        if (ierr.gt.100) k=-1
        call ERRNUM (109,k,'SETFLUIDS',(herr),0d0,0d0,0d0,ierr,herr)
      else
        call SETUP (k,hf,hFmix,hrf,ierr,herr)
      endif
      RETURN

      end                                           subroutine SETFLUIDS
c
c ======================================================================
c
      subroutine SETMIXTURE (hMixNme,z,ierr)
c
c  Call the SETMIX routine for a predefined mixture without the need to
c  pass hFmix, hrf, ncc, hf, or herr.  It is not necessary to declare the
c  length of hMixNme as 255 bytes long.  A path can be included if needed.
c  The extension ".mix" is not required.  If it is necessary to set the
c  reference state, call subroutine FLAGS first.  The composition of the
c  mixture will be returned in the z array.  If ierr comes back non-zero,
c  call the ERRMSG routine to obtain it.
c
c  Examples::
c
c      call SETMIXTURE ('AIR.MIX',z,ierr) ! load the air mixture from the AIR.MIX file
c      call SETMIXTURE ('C:/REFPROP/MIXTURES/AIR.MIX',z,ierr)    read the AIR.MIX file from the C:/REFPROP/MIXTURES directory
c      call SETMIXTURE ('R410A.MIX',z,ierr) ! load the R410A mixture, the composition will be returned on a mole percent basis in the z array.
c      call SETMIXTURE ('R410A',z,ierr)  ! works the same as above for predefined refrigerant mixtures that start with R4 or R5.
c
c  Inputs:
c   hMixNme--String of any character length containing the mixture file name
c
c  Outputs:
c        z--Composition array (mole fractions)
c     ierr--Error flag:  0 - Successful
c           (Values are identical to SETMIX)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  03-17-17 EWL, original version

      include 'COMMONS.INC'
      character hMixNme*(*),hrf*3
      character*255 hf(ncmax),hFmix,hfl,herr,herr2,UCASE,LTRIM
      dimension z(ncmax)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETMIXTURE

      hrf=' '
      hFmix='HMX.BNC'
      hfl=hMixNme
      i=INDEX(hfl,CHAR(0))             !Check for null character on the last component.
      if (i.ne.0) hfl(i:)=' '
      hfl=LTRIM(hfl)
      i=INDEX(UCASE(hfl),".MIX")
      if (i.eq.0) then
        i=INDEX(hfl,'             ')
        if (i.gt.0) hfl=hfl(1:i-1)//'.mix'
      endif
      call SETMIX (hfl,hFmix,hrf,ncc,hf,z,ierr,herr)
      if (iSetRef2.gt.0) then          !Call SETREF
        call SETREF (hRef3,iSetRef2,z,
     &               h0Ref3,s0Ref3,T0Ref3,P0Ref3,ierr2,herr2)
        ierr2=-ABS(ierr2)    !Don't quit due to errors from SETREF.
        call SWAPERR (ierr,ierr2,herr,herr2)
      endif


      end                                          subroutine SETMIXTURE
c
c ======================================================================
c
      subroutine SETPATH (hpth)
c
c  Set the path where the fluid files are located.
c
c  Inputs:
c     hpth--Location of the fluid files (character*255)
c           The path does not need to contain the ending "/" and it can
c           point directly to the location where the DLL is stored if a
c           fluids subdirectory (with the corresponding fluid files) is
c           located there, for example, hpth='C:/Program Files (x86)/REFPROP'
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  09-22-06 EWL, original version
c  03-07-07 EWL, check for null character

      include 'COMMONS.INC'
      character hpth*(*)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::SETPATH

      hpath=hpth
      i=INDEX(hpath,CHAR(0))
      if (i.ne.0) hpath(i:)=' '
      end                                             subroutine SETPATH
c
c ======================================================================
c
      subroutine ERRMSG (ierr,herr)
c
c  Retrieve the last error message saved in calls to ERRNUM (but
c  only if the ierr variable is not equal to zero).  Write
c  error messages to default output if iErrPrnt is active.
c  The variable iErrPrnt in the common blocks must always be zero
c  when compiling the DLL.
c
c  Outputs depend on variable iErrPrnt in the common blocks
c
c  * iErrPrnt= 0 -   Error string not written (default)
c  * iErrPrnt=-1 -   Error string written to screen
c  * iErrPrnt= 1 -   Error string written to screen only if ierr is positive
c  * iErrPrnt=3,-3 - Same as 1 and -1, but program also pauses
c
c  Input:
c     ierr--Error number from the last call to ERRNUM
c
c  Output:
c     herr--Associated error string (character*255)

      include 'COMMONS.INC'
      character herr*255,hx*13,hy*13,hz*13
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::ERRMSG

      if (iErrNum.gt.0) RETURN         !Exit if error messages are currently not being written.
      if (ierr.eq.0) then
        herr=' '
        RETURN     !If the input error number sent to this routine is zero, exit.
      endif
      if (iersav3.eq.999 .and. ierr.eq.iersav2) then       !If a message from SETUP has been saved and a user is requesting the error message, send that back and exit.
        herr=hersav2
        RETURN
      endif
      if (ierr.ne.iersav .and. ierr.eq.iersav2) then       !If the current value of the error message is not equal to the last time this was called,
        herr=hersav2                                       ! but is equal to the value saved for the most important message (in iersav2), return the
        RETURN                                             ! message generated by the call that saved the important message.
      endif
      if (iersav.eq.0)  RETURN
      if (ierr.ne.iersav) RETURN       !Return if the input error number does not match the one used to generate the error.
      call IDHCONV (xersav,0,0,i1,j1,hx)
      call IDHCONV (yersav,0,0,i2,j2,hy)
      call IDHCONV (zersav,0,0,i3,j3,hz)

      if (ierr.eq.17 .and. zersav.eq.0d0) then
        hz='NA'
        i3=1
        j3=2
      endif

      call ERRNUM2 (ierr,ihrsav,hasav(1:ihasav),hbsav(1:ihbsav),
     &              hx(i1:j1),hy(i2:j2),hz(i3:j3),herr)

      if (herr(1:1).eq.'[') then
        i=INDEX(herr,';    ')          !Change the semicolon at end of the line to a period.
        if (i.gt.2) herr(i:i)='.'
      endif

c  Print out the error message to the screen if the iErrPrnt flag has been activated.
      if ((ierr.gt.0 .and. iErrPrnt.ne.0) .or.
     &    (ierr.ne.0 .and. iErrPrnt.lt.0)) then
        write (*,'(a255)') herr
         if (ABS(iErrPrnt).eq.3) pause           !If your compiler complains about this line, just delete it.
      endif

c     if (lNan) then
c       if (INDEX(herr,'NaN').gt.0) then
c         write (*,*) herr
c         pause 'NaN'
c       endif
c     endif

      end                                              subroutine ERRMSG
c
c ======================================================================
c
      subroutine FLAGS (hFlag,jFlag,kFlag,ierr,herr)
c
c    Set flags for desired behavior from the program.
c
c    .. table:: Table of flags in FLAGS function
c        :class: longtable
c
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``hFlag``           | ``jFlag``                                                                                           |
c        +=====================+=====================================================================================================+
c        | ``Return errors``   | *  0 - Return only final messages (default).                                                        |
c        |                     | *  1 - Return all intermediate messages.                                                            |
c        |                     | *  2 - Do not return messages.                                                                      |
c        |                     |                                                                                                     |
c        |                     | This flag is not reset with a new call to SETUP.                                                    |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Write errors``    | *  0 - Error strings not written to the screen (default).                                           |
c        |                     | * -1 - Error string written to the screen.                                                          |
c        |                     | *  1 - Error string written to the screen only if ierr is positive.                                 |
c        |                     | * 3,-3 - Same as 1 and -1, but program also pauses.                                                 |
c        |                     |                                                                                                     |
c        |                     | This flag is not reset with a new call to SETUP.                                                    |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Dir search``      | * 0 - Search for fluid files in alternate directories (as defined in OPENFL) (default).             |
c        |                     | * 1 - Do not search in directories other than the one set by the call to SETPATH,                   |
c        |                     |   except for a 'fluids' subdirectory within the folder given in SETPATH.                            |
c        |                     |   If the fluid files for the reference fluid(s) are not in the SETPATH directory,                   |
c        |                     |   then transport properties may not be calculated.                                                  |
c        |                     | * 2 - Make no additional checks if the fluid file is not found after the first attempt to open      |
c        |                     |   the file (for example, checking upper and lower case).                                            |
c        |                     |                                                                                                     |
c        |                     | This flag is never reset.                                                                           |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Cp0Ph0``          | * 1 - Change the ideal gas equation to Cp0.                                                         |
c        |                     | * 2 - Change the ideal gas equation to PH0.                                                         |
c        |                     |                                                                                                     |
c        |                     | The default is set by the value in the fluid file.                                                  |
c        |                     | Calling SETUP resets the equation to its default state as given in the fluid file.                  |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``PX0``             | * 0 - Use the fluid file as is for the ideal gas equation (default).                                |
c        |                     | * 1 - Use the PX0 (or PH0 when no PX0 is available) for all calculations and turn off the effect    |
c        |                     |   of SETREF.  For mixtures, the reference state of "each pure component" will be used.  Reference   |
c        |                     |   states cannot be changed with this option on, the state is preset by values in the fluid file.    |
c        |                     |                                                                                                     |
c        |                     | This flag is never reset.  When setting up the fluids through a call to the REFPROP subroutine,     |
c        |                     | the SETREF flag described below will have no effect if the PX0 option has already been turned on.   |
c        |                     | Warning:  Don't use the CP0PH0 flag to switch back to Cp0, recall this routine with jFlag=0.        |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Skip SETREF``     | * 0 - Call the SETREF routine during the call to SETUP to set the reference state (default).        |
c        |                     |   The only reason to call this would be to increase the speed of setting up new fluids              |
c        |                     |   because the SETREF routine has to make complex saturation calculations.                           |
c        |                     | * 1 - Skip the call to SETREF.  However, this means energy, enthalpy, and entropy will not be       |
c        |                     |   correct (but only by an offset to their usual values).                                            |
c        |                     |                                                                                                     |
c        |                     | This must be called before the call to SETUP, and is never reset.                                   |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        |``Mixture reference``| * 0 - Do nothing (default).                                                                         |
c        |                     | * 1 - When calling subroutine REFPROP, call SETREF first with a value of 1 for the second entry.    |
c        | or ``SETREF``       | * 2 - When calling subroutine REFPROP, call SETREF first with a value of 2 for the second entry.    |
c        |                     |   (See subroutine SETREF for details.)                                                              |
c        |                     |                                                                                                     |
c        |                     |  This must be called before the call to REFPROP (the subroutine in PROP_SUB.FOR), and is            |
c        |                     |  never reset.  The call does nothing except during calls to SETUP.  It does not affect the current  |
c        |                     |  state of the reference states.  Call SETREF to do that.                                            |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        |  ``Skip ECS``       | * 0 - Load the ECS fluids required for transport properties (for pure fluids in slots 21-40, and    |
c        |                     |   mixtures in slot 41) (default).                                                                   |
c        |                     | * 1 - Don't load the ECS fluids, only the requested fluids (this may deactivate pure fluid transport|
c        |                     |   properties, and will deactivate all mixture transport calculations.                               |
c        |                     |                                                                                                     |
c        |                     | This must be called before the call to SETUP, and is never reset.                                   |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        |  ``Splines off``    | * 1 - Turn the splines off (assuming that they were turned on initially by a call to SATSPLN).      |
c        |                     |                                                                                                     |
c        |                     | Calling SETUP again will also turn off the splines.   The SATSPLN routine must be called after      |
c        |                     | every call to SETUP to keep the splines active.  The value of kFlag will return the current status  |
c        |                     | of the splines:  0 for off, 1 for on.                                                               |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Ignore bounds``   | * 0 - Check all errors and respond accordingly (default).                                           |
c        |                     | * 1 - Ignore bounds for certain situations, such as calling SATT                                    |
c        | or ``Bounds``       |   below the triple point or states above the melting line.                                          |
c        |                     |                                                                                                     |
c        |                     | This flag is never reset.                                                                           |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Cache``           | * 0 - Cache all calculated values (default).                                                        |
c        |                     | * 1 - Cache only low level calculations, such as derivatives calculated in PHIFEQ.                  |
c        |                     | * 2 - Cache only calculated properties in major subroutines such as SATT and SATP.                  |
c        |                     | * 3 - No caching.                                                                                   |
c        |                     |                                                                                                     |
c        |                     | This flag is never rest.  Option #0 is the combination of options #1 and #2.                        |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Reset all``       | * 2 - Call RESETA to reset cached values and all flags set by calls to this routine, except for the |
c        |                     |   use of a pure fluid in a mixture or to decrease the number of fluids being used in the mixture    |
c        |                     |   by changing nc.  Resetting some of the flags has no effect until SETUP is called again.           |
c        |                     |   The HMX option is not reset since it is, by itself, a reset flag.                                 |
c        |                     |                                                                                                     |
c        |                     | Subroutine RESETA is always called by SETUP, but does not reset most of the flags set here, such    |
c        |                     | as the use of GERG-2008, which requires a call to SETUP to return to the original state..           |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Reset HMX``       | * 1 - Reset the caching flag so that the HMX.BNC file is read again on the next call to SETUP.      |
c        |                     |   This option is only useful during fitting mixture models or modifying the HMX.BNC file            |
c        | or ``HMX``          |   to add new interaction parameters, otherwise this flag will only slow down the program            |
c        |                     |   by forcing a reread of the mixture file.  The output variable kFlag will be 0 or 1 to indicate    |
c        |                     |   whether or not the HMX.BNC will be read on the next call to SETUP.                                |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Pure fluid``      | * 0 - Use full mixture equation of state loaded (default).                                          |
c        |                     | * <>0 - Use the pure fluid loaded in the slot specified by jFlag.                                   |
c        |                     |                                                                                                     |
c        |                     | This option is reset during the call to SETUP.                                                      |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Component number``| * nc - Reduce the number of fluids being used.  See SETNC routine for details.                      |
c        |                     |   The output in kFlag will give the number of fluids in use,                                        |
c        | or ``nc``           |   which can be useful even if this option has not been called to set nc.                            |
c        |                     |   Calling this option with jFlag=0 is a convenient way to force SETUP to reread the fluid files.    |
c        |                     |                                                                                                     |
c        |                     | This option is reset during the call to SETUP.  This option does not unload the fluids, it just     |
c        |                     | changes the variable nc to the number of fluids requested.  There is no option to change nc         |
c        |                     | back to the number of fluids originally read in - you must keep track of that number if needed.     |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Peng-Robinson``   | * 0 - Turn off the Peng-Robinson equation of state (default).                                       |
c        |                     | * 2 - Use Peng-Robinson equation for all calculations.                                              |
c        | or ``PR``           | * 3 - Use Peng-Robinson with translation term deactivated.                                          |
c        |                     |                                                                                                     |
c        |                     | This option is never reset.  Calling SETUP again will turn the PR back on after it loads all of     |
c        |                     | the other equations of state and transport equations.  Thus you can at any time recall this routine |
c        |                     | to turn off the PR and go back to whatever equations were loaded during the last call to SETUP.     |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``kij Zero``        | * 0 - Use the fitted kij values found in the HMX.BNC file on the lines with PR1 (default).          |
c        |                     | * 1 - Set all kij values to those estimated in ESTPR (thus ignoring the ones on the PR1             |
c        |                     |   lines in the HMX.BNC file).                                                                       |
c        |                     | * 2 - Set all kij values to zero.                                                                   |
c        |                     |                                                                                                     |
c        |                     | This option is never reset.                                                                         |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``AGA8``            | * 0 - Turn off AGA8 and return to the fluids loaded from the call to SETUP (default)                |
c        |                     | * 1 - Turn on the use of the AGA8 DETAIL equation of state.                                         |
c        |                     |                                                                                                     |
c        |                     | If the AGA8 option is active, it overrides all other models.  Unlike the GERG 2008 option, this     |
c        |                     | model is activated (or deactivated) immediately upon calling this routine.  If the GERG 2008 option |
c        |                     | is on, you must turn it off before calling this routine.                                            |
c        |                     | The AGA8 flag is never reset.  Calling SETUP again will turn the AGA8 back on after it loads all of |
c        |                     | the other equations of state and transport equations.  Thus you can at any time recall this routine |
c        |                     | to turn off AGA8 and go back to whatever equations were loaded during the last call to SETUP.       |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``GERG 2008``       | * 0 - Set a flag to turn off GERG 2008 next time SETUP is called.                                   |
c        |                     | * 1 - Turn on a flag that will cause the GERG 2008 equation to be loaded next time SETUP is called. |
c        | or ``GERG``         |                                                                                                     |
c        |                     | This option MUST be called before SETUP.   When turning off the GERG, call the SETUP routine again  |
c        |                     | after calling this routine.  Because the GERG model is not activated until SETUP is called,         |
c        |                     | the value of kflag will be 1 until the next call to SETUP, at which time it will be set to 2 to     |
c        |                     | indicate that it is fully active.    When turning off the GERG model, the value of kflag will be -1 |
c        |                     | until the next call to SETUP, and then it will be reset to zero.  The -1 indicates that it is still |
c        |                     | in use but waiting to be reset.  This flag itself is never reset.                                   |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Gas constant``    | * 0 - Use the most current gas constant for all fluids except nitrogen, argon, oxygen,              |
c        |                     |   ethylene, CO2, methane, and ethane. (default)                                                     |
c        | or ``R``            | * 1 - Use the most current gas constant for all fluids (must be called after call to SETUP).        |
c        |                     | * 2 - Use the gas constant from the fluid files for each equation of state (must be called          |
c        |                     |   after call to SETUP).                                                                             |
c        |                     |                                                                                                     |
c        |                     | This option is reset during the call to SETUP.                                                      |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Calorie``         | * 0 - Use a calorie to joule conversion value of 4.184 cal/J (default).                             |
c        |                     | * 1 - Use the IT value of 4.1868 cal/J.                                                             |
c        |                     |                                                                                                     |
c        |                     | This option is never reset.                                                                         |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c        | ``Debug``           | * 0 - Turn off all debugging.                                                                       |
c        |                     | * 1 - In the REFPROP subroutine, write all input variables to a file called input.dat, and all      |
c        |                     |   output values to a file called output.dat                                                         |
c        |                     | * 2 - In SETUP, write out the full path of the files that were either opened or tried to open.      |
c        |                     |                                                                                                     |
c        |                     | This option is never reset.  This option is useful for sending back to NIST the process used        |
c        |                     | to get to a point where a failure occurred.                                                         |
c        +---------------------+-----------------------------------------------------------------------------------------------------+
c
c  Inputs:
c    hFlag--String value given above in quotes to indicate which option to set or check (letters in the string are case insensitive).
c    jFlag--Flag to choose what to do in each option.  Send -999 to just obtain the current value of the flag.
c
c  Outputs:
c    kFlag--Current setting of the flag for the option identified by hFlag.
c           (Returned regardless of the value of jFlag.)
c     ierr--Error flag:  0 - Successful
c     herr--Error string (character*255)
c
c  Written by E.W. Lemmon, NIST Thermophysics Division, Boulder, Colorado
c  01-01-17 EWL, original version

      include 'COMMONS.INC'
      include 'COMREFP.INC'
      include 'COMFEQ.INC'
      character*255 herr,hf,UCASE,hFlag*(*)
ccDEC$ ATTRIBUTES DLLEXPORT, Decorate::FLAGS

      ierr=0
      herr=' '
      kFlag=-999
      hf=UCASE(hFlag)
      lResetAll=.false.
      if (hf.eq.'RESET ALL') lResetAll=.true.

      if (lResetAll) then
        goto 100

      elseif (hf.eq.'RETURN ERRORS') then                                !Method of dealing with error messages.
        kFlag=0
        if (jFlag.eq.0) iErrNum=0
        if (jFlag.eq.1) iErrNum=-1000
        if (jFlag.eq.2) iErrNum= 1000
        if (iErrNum.lt.0) kFlag=1
        if (iErrNum.ge.100) kFlag=2

      elseif (hf.eq.'WRITE ERRORS') then                             !Write messages to screen.
        if (ABS(jFlag).lt.10) iErrPrnt=jFlag
        kFlag=iErrPrnt

      elseif (hf.eq.'DIR SEARCH') then                               !Set the ihPath flag to indicate whether or not to search directories other than that set by SETPATH.
        if (jFlag.ge.0) ihPath=jFlag
        if (jFlag.ge.0) iReset=1
        kFlag=ihPath

      elseif (hf.eq.'CP0PH0') then                                   !Change the ideal gas equation between the Cp0 and Ph0 options.
        kFlag=-1
        if (jFlag.ge.0) then
          kFlag=jFlag
          iph0=jFlag
          goto 400
        endif

      elseif (hf.eq.'PX0') then                                      !Use the PX0 (or PH0) equations for all calculations and turn off the call to SETREF.
        kFlag=iPX0flg                                                !This does the same thing as the CP0PH0 option, except that it leaves it on when SETUP is called again.
        if (jFlag.ge.0) then
          iPX0flg=jFlag
          kFlag=iPX0flg
          iph0=jFlag+1
          goto 400
        endif

      elseif (hf.eq.'SKIP SETREF') then                              !Skip the call to SETREF.
        kFlag=0
        if (jFlag.eq.0) lSkipRef=.false.
        if (jFlag.eq.1) lSkipRef=.true.
        if (jFlag.ge.0) iReset=1
        if (lSkipRef) then
          kFlag=1
          tref=300d0
          Dref=1d-3
          href=0d0
          sref=0d0
          iSetRf=0
          ixfsav=-1
        endif

      elseif (hf.eq.'MIXTURE REFERENCE' .or. hf.eq.'SETREF') then    !When using subroutine REFPROP to make all property calls, call SETREF
        if (jFlag.eq.0) iSetRef2=0                                   ! with a value of 1 or 2 for the second entry before the call to SETUP.
        if (jFlag.eq.1) iSetRef2=1                                   ! This is the option that the GUI and Excel files use, and mainly affects
        if (jFlag.eq.2) iSetRef2=2                                   ! refrigerant mixtures, or mixtures with the same reference state.
        if (jFlag.ge.0) iReset=1
        kFlag=iSetRef2

      elseif (hf.eq.'SKIP ECS') then                                 !Don't load the ECS fluids.
        kFlag=0
        if (jFlag.eq.0) lSkipECS=.false.
        if (jFlag.eq.1) lSkipECS=.true.
        if (jFlag.ge.0) iReset=1
        if (lSkipECS) kFlag=1

      elseif (hf.eq.'SPLINES OFF') then                              !Turn off splines (only if SATSPLN had been called).
        kFlag=0
        if (jFlag.eq.1) call RESETA (1)
        if (iSpline.eq.1) kFlag=1

      elseif (hf.eq.'IGNORE BOUNDS' .or. hf.eq.'BOUNDS') then        !Ignore boundaries.
        if (jFlag.ge.0) ignbnd=jFlag
        kFlag=ignbnd

      elseif (hf.eq.'CACHE') then                                    !Property caching.
        kFlag=0
        if (jFlag.ge.0) then
          lCache1=.true.
          lCache2=.true.
          if (jFlag.eq.1 .or. jFlag.eq.3) lCache2=.false.
          if (jFlag.eq.2 .or. jFlag.eq.3) lCache1=.false.
        endif
        if (lCache1 .and. .not.lCache2) kFlag=1
        if (lCache2 .and. .not.lCache1) kFlag=2
        if (.not.lCache1 .and. .not.lCache2) kFlag=3

      elseif (hf.eq.'RESET HMX' .or. hf.eq.'HMX') then               !Reset flag to re-read the HMX.BNC file.
        if (jFlag.ge.0) iReset=1
        if (jFlag.eq.1)  then
          iRDbnc=0
          iRDmxm=0
          nMx_hmx=0
        endif
        kFlag=iRDbnc

      elseif (hf.eq.'PURE FLUID') then                               !Use one of the components in the mixture as a pure fluid, and turn all others off.
        if (jFlag.ge.0 .and. jFlag.le.nc) call PUREFLD (jFlag)
        kFlag=icnc

      elseif (hf.eq.'COMPONENT NUMBER' .or. hf.eq.'NC') then         !Reduce the number of components in the mixture.
        if (jFlag.eq.0) nc=0      !Can be used to force SETUP to reread the fluid files.
        if (jFlag.gt.0 .and. jFlag.le.ncmax) call SETNC (jFlag)
        kFlag=nc

      elseif (hf.eq.'PENG-ROBINSON' .or. hf.eq.'PR') then            !Turn Peng-Robinson calculations on/off.
        if (jFlag.ge.0 .and. jFlag.le.3) then
          call RESETA (0)
          call PREOS (jFlag)
        endif
        kFlag=0
        if (iEOSmx.eq.iEOSPR)  kFlag=2
        if (iEOSmx.eq.iEOSPR3) kFlag=3

      elseif (hf.eq.'KIJ ZERO') then                                 !Use/don't use Peng-Robinson kij values from the HMX.BNC file.
        if (jFlag.ge.0 .and. jFlag.le.10) then
          kijzero=jFlag
          call RESETA (0)
          if (kijzero.eq.0) then
            fPRkij=fPRkij2             !If the flag to zero out all PR coefficients is off, transfer the values read in to the values that will be used.
          elseif (kijzero.eq.1) then
            do i=1,nc
              do j=i,nc
                call ESTPR (i,j,0,fPRkijx,ierr,herr)
                fPRkij(1,i,j)=fPrkijx
                fPRkij(1,j,i)=fPrkijx
              enddo
            enddo
          elseif (kijzero.eq.2) then
            fPRkij=0d0
          endif
        endif
        kFlag=kijzero

      elseif (hf.eq.'TRNFLAG') then
        itrnflag=jFlag
        kFlag=itrnflag

      elseif (hF.eq.'NAN') then
        lNan=.true.          !Check for NaN

      elseif (hF.eq.'DEBUG') then
        if (jFlag.eq.0) then
          iDebug=0
        elseif (jFlag.ge.1 .and. jFlag.le.10) then
          if (jFlag.ne.iDebug .and. jFlag.eq.1) then
            open(unit=2,iostat=istat,file='input.dat',status='old')       !Delete the input and output files if jFlag=1.
            if (istat.eq.0) close(2,status='delete',err=1)
            open(unit=2,iostat=istat,file='output.dat',status='old')
            if (istat.eq.0) close(2,status='delete',err=1)
          elseif (jFlag.ne.iDebug .and. jFlag.eq.2) then
            open(unit=2,iostat=istat,file='filename.dat',status='old')    !Delete the old output file if jFlag=2.
            if (istat.eq.0) close(2,status='delete',err=1)
          endif
 1        iDebug=jFlag   !To not delete the files, send any number between 3 and 10.
        endif
        kFlag=iDebug

      elseif (hf.eq.'CALORIE' .or. hf.eq.'AGA8') then
        goto 100
      elseif (hf.eq.'GAS CONSTANT' .or. hf.eq.'R') then
        goto 100
      elseif (hf.eq.'GERG 2008' .or. hf.eq.'GERG') then
        goto 100
      else
        ierr=601
        call ERRNUM (601,0,'FLAGS',hF,0d0,0d0,0d0,ierr,herr)
      endif
      RETURN


 100  continue

      if (hf.eq.'GAS CONSTANT' .or. hf.eq.'R' .or. lResetAll) then        !Use either a constant or variable gas constant.
        kFlag=0
        if ((jFlag.ge.0 .and. jFlag.le.10) .or. lResetAll) then
          call RESETA (0)
          Rxgas=xRgas
          lRcnst=.true.
          lRcnstWag=.true.
        endif
        if (jFlag.eq.0 .or. lResetAll) then
          if (lWagner) lRcnstWag=.false.
          Reos=ReosDef
          credCp0(1:nc)=RCp0Def(1:nc)
        elseif (jFlag.eq.1) then
          Reos=xRgas
          credCp0(1:nc)=RxCp0(1:nc)
        elseif (jFlag.eq.2) then
          lRcnst=.false.
          lRcnstWag=.false.
          Reos=ReosOrg
          credCp0(1:nc)=RCp0Org(1:nc)
        endif
        if (lRcnst .and. lRcnstWag) kFlag=1
        if (.not.lRcnst .and. .not.lRcnstWag) kFlag=2
      endif

      if (hf.eq.'AGA8') then                                     !Turn AGA8 mode on/off.
        if ((jFlag.eq.0 .or. lResetAll) .and. lAGA8) then
          iAGA8=-1
          call UNSETAGA
        elseif (jFlag.eq.1 .and. .not.lAGA8 .and. .not.lResetAll) then
          iAGA8=1
          call SETAGA (ierr,herr)
        endif
        kFlag=0
        if (lAGA8) kFlag=1
        if (jFlag.eq.1 .and. nc.eq.0) kFlag=1  !Send back a positive value even though it is not yet active because no fluids have been loaded.
      endif

      if (hf.eq.'GERG 2008' .or. hf.eq.'GERG' .or. lResetAll) then        !Turn GERG-2008 mode on/off.
        if (jFlag.eq.0 .or. lResetAll) then
          call GERG08 (0,0,ierr,herr)
        elseif (jFlag.eq.1) then
          call GERG08 ((ncmax),1,ierr,herr)
        endif
        kFlag=iGERG08
      endif

      if (hf.eq.'CALORIE' .or. lResetAll) then                            !Select the calorie to Joule conversion value.
        kFlag=0
        if (jFlag.eq.0 .or. lResetAll)       CaltoJ=4.184d0
        if (jFlag.eq.1 .and. .not.lResetAll) CaltoJ=4.1868d0
        BTUtoKJ=CALtoJ*LBMtoKG*RtoK
        if (lEq0(CaltoJ-4.1868d0)) kFlag=1
      endif

      if (lResetAll) then                                                 !Call RESETA to reset all temporary flags.
        iph0=1
        kFlag=jFlag
        if (jFlag.ge.0) call RESETA (jFlag)
        goto 400
      endif

      RETURN

c  Reset the Cp0 equations to remove any PX0 options.
 400  do i=1,nc
        n1=nCp0Poly(i)+nCp0Exp(i)+nCp0Cosh(i)+nCp0Sinh(i)
        n2=nPhiLog(i)+nPhiTau(i)+nPhiExp(i)+nPhiCosh(i)+nPhiSinh(i)
        if (iph0.eq.1 .and. n1.gt.0) then
          iCP0xx(i)=iCP0CP
          if (hf.eq.'CP0PH0') kFlag=1
          call RESETA (0)
        elseif (iph0.eq.2 .and. n2.gt.0) then
          iCP0xx(i)=iCP0PH
          if (hf.eq.'CP0PH0') kFlag=2
          call RESETA (0)
        endif
      enddo

      end                                               subroutine FLAGS


c        1         2         3         4         5         6         7
c23456789012345678901234567890123456789012345678901234567890123456789012
c
c ======================================================================
c                                                   End file REFPROP.FOR
c ======================================================================
