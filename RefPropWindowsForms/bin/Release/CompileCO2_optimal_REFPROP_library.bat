gfortran -ffree-line-length-0 Netlib/subplex/*.f Netlib/fmin.f fortran/*.for CO2_RP_module1.f90 core.f90 design_point.f90 program_CO2_optimal.f90 -shared -mrtd -o RC_CO2_Mixture_Subplex.dll

