gfortran Netlib/subplex/*.f Netlib/fmin.f module_CO2_properties.f90 core.f90 design_point.f90 
snl_compressor.f90 snl_radial_turbine.f90 scaling_hxr.f90 off_design_point.f90 
program_CO2_offdesign.f90 -shared -mrtd -o CO2_offdesign.dll
pause

!Esta siguiente con las REFPROP No me Funciona bien
gfortran Netlib/subplex/*.f Netlib/fmin.f CO2_RP_module.f90 fortran/*.for core.f90 design_point.f90 program_CO2.f90 -shared -mrtd -o CO2.dll
pause