cd Desktop
cd sCO2 Heat Balances C#
cd sCO2 Heat Balances C#
cd sCO2 Heat Balances C#
cd sCO2 Heat Balances C#
cd RefPropWindowsForms
cd RefPropWindowsForms
cd Interfaces in Fortran
cd PCRC without ReHeating_Subplex
pause

gfortran Netlib/subplex/*.f Netlib/fmin.f module_CO2_properties.f90 core.f90 design_point.f90 program_CO2_optimal_PCRC.f90 -shared -mrtd -o PCRC_CO2_Optimal_Subplex.dll
pause