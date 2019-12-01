cd Desktop
cd sCO2 Heat Balances C#
cd sCO2 Heat Balances C#
cd sCO2 Heat Balances C#
cd sCO2 Heat Balances C#
cd RefPropWindowsForms
cd RefPropWindowsForms
cd Interfaces in Fortran
cd RCMCI with ReHeating_Subplex
pause

gfortran -ffree-line-length-0 Netlib/subplex/*.f Netlib/fmin.f module_CO2_properties.f90 core.f90 design_point.f90 program_CO2_design_RCMCI_without_RH.f90 -shared -mrtd -o RCMCI_CO2_design_point_without_RH.dll
pause