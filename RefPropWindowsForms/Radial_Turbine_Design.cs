using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using sc.net;

namespace RefPropWindowsForms
{
    public partial class Radial_Turbine_Design : Form
    {
        // Rotor Inlet Static (o1); Rotor Inlet Total (o4); Rotor Inlet Static (4); Rotor Outlet Total (o6); Rotor Outlet Static (6) 

        //Radial_Turbine Thermodynamic Operation Conditions at Design Point
        public Double P_total_inlet, T_total_inlet, Rho_total_inlet, H_total_inlet, S_total_inlet, S_total_inlet_molar, Speed_of_Sound_Rotor_inlet, Speed_of_Sound_Rotor_outlet;
        public Double P_total_outlet, T_total_outlet, Rho_total_outlet, H_total_outlet, S_total_outlet, S_total_outlet_molar;
        public Double Rho_outlet_isoentropic, H_total_outlet_isoentropic, S_total_outlet_isoentropic;
        public Double total_entalphy_difference, static_enthalpy_difference, Objective_Pressure_ratio, P_ratio_outlet_static_inlet_total;
        public Double eta_static, eta_total, Molar_mass;
        public Double viscosity_inlet, cp_inlet, cv_inlet, ganma_inlet;

        //Rotor Design_input_data
        public Double power, mass_flow_rate;
        public Double flow_coefficient, head_coefficient;
        public Double ksi=1; //Cm4/Cm6 or Cm_inlet/Cm_outlet
        public Double rotational_speed;
        public Double specific_gas_constant;
        public Double universal_gas_constant = 8.3144722;
        public Double r_ratio = 0.3;

        //Rotor Inlet_state (4)
        public Double U_inlet, Ctheta_inlet, Cm_inlet, C_inlet, alpha_inlet, beta_inlet, radius_inlet, area_critical_inlet;
        public Double W_inlet, H_static_inlet, H_static_inlet_isoentropic, P_static_inlet, T_static_inlet, rho_static_inlet;
        public Double area_inlet, blade_height_inlet, Mach_absolute_inlet, Mach_relative_inlet;
        public Double tt=0.001; //rotor_trailing_edge_thickness_from_File
        public Double number_blades = 9;

        //Rotor Outlet_state (6)
        public Double U_outlet, Ctheta_outlet, Cm_outlet, C_outlet, alpha_outlet, beta_outlet, beta_hub_outlet, radius_outlet, U_hub_outlet, W_hub_outlet;
        public Double U_tip_outlet, U_rms_outlet, W_tip_outlet, W_rms_outlet,beta_tip_outlet,beta_rms_outlet;
        public Double W_outlet, H_static_outlet, H_static_outlet_isoentropic, P_static_outlet, T_static_outlet, rho_static_outlet;
        public Double area_outlet, blade_height_outlet, Mach_absolute_outlet, Mach_relative_tip_outlet, Mach_relative_rms_outlet, Mach_relative_hub_outlet, area_critical_outlet;
        public Double radius_hub_outlet, radius_tip_outlet, radius_rms_outlet, radius_ratio_inlet_outlet_tip;
        public Double P_static_outlet_in_pascales;
        public Double rotor_outlet_area_total, rotor_outlet_area, rotor_outlet_rms_length;

        public Double A, B, C, D;
        public Double Vol_flow_rate;
        public Double omega;
        public Double delta_h0;
        public Double specific_speed; 
        public Double specific_diameter;
        public Double velocity_ratio;
        public Double NsDs;

        public Double rotor_tip_inlet_tip_outlet_length;
        public Double rotor_radius_mean_outlet;
        public Double rotor_chord;
        public Double rotor_full_length;
        public Double rotor_meridional_length;
        public Double rotor_outlet_length_rms_total, curvature_radius, axial_clearance, radial_clearance;
        public Double axial_clearance_From_File, radial_clearance_From_File;
        public Double back_face_clearance_From_File, back_face_clearance;
        public Double rotor_trailing_edge_thickness;
        public Double shroud_meridional_distance, ratio_meridional_distance_throat;

        //ROTOR_THROAT_CALCULATION return variables: 
        public Double ksi_rotor_throat = 0;
        public Double omega_rotor_throat = 0;
        public Double rotor_throat_length = 0;
        public Double rotor_throat_area = 0;
        public Double distance_A_C_rotor = 0;
        public Double rotor_preliminary_feasibility_check = 0;
        public Double r_tip_throat = 0;
        public Double r_hub_throat = 0;

        public Double phi_wheel=0;
        public Double throat_radius = 0;
        public Double Reynolds_Number_Rotor = 0;
        
        //STATOR
        public Double stator_blade_height, stator_alpha_inlet, delta_radius, stator_radius_outlet;
        public Double stator_solidity_input = 1.35;  // THIS IS THE INITIAL VALUE USED IN THE NASA CODE. REFERENCE FOR STATOR-VANE SOLIDITY: Modern engineering for design of liquid-propellant rocket engines By Dieter K. Huzel, David H. Huang, Harry Arbit && Fundamentals of jet propulsion with applications By Ronald D. Flack
        public Double stator_radius_inlet_radius_outlet_ratio = 1.25;
        public Double stator_radius_outlet_rotor_inlet_ratio  = 1.09170305677;
        public Double stator_radius_inlet;
        public Double stator_alpha_outlet;
        public Double stator_T_total_inlet;
        public Double stator_T_static_inlet;
        public Double stator_P_total_inlet;
        public Double stator_P_static_inlet;
        public Double stator_rho_total_inlet;
        public Double stator_rho_static_outlet;
        public Double stator_rho_total_outlet;
        public Double stator_blade_inclination;
        public Double b_line_stator;
        public Double A_quadratic_function_stator, B_quadratic_function_stator, C_quadratic_function_stator;
        public Double y_positive_stator, y_negative_stator;
        public Double stator_inlet_y_coordinate, stator_inlet_x_coordinate;
        public Double stator_blade_length, stator_chord, stator_T_total_outlet, stator_T_static_outlet;
        public Double stator_P_total_outlet, stator_P_static_outlet;
        public Double stator_rho_static_inlet, stator_Cm_inlet;
        public Double stator_Ctheta_inlet, stator_C_inlet, stator_T_static_inlet_New;
        public Double stator_number_vanes = 11; //STATOR NUMBER SHOULE BETWEEEN 13 AND 17
        public Double stator_solidity, stator_outlet_length_total, stator_outlet_length;
        public Double stator_outlet_area_total, stator_outlet_area;
        public Double stator_inlet_length_total, stator_inlet_length;
        public Double stator_area_inlet_total, stator_area_inlet;
        public Double stator_C_outlet, stator_Cm_outlet, stator_Ctheta_outlet;
        public Double Reynolds_Number_Stator, stator_trailing_edge_thickness;
        public Double stator_throat_radius;
        public Double stator_critical_area;

        //STATOR_THROAT_CALCULATION return variables: 
        public Double ksi_stator_throat, omega_stator_throat, stator_throat_length;
        public Double stator_throat_area, distance_A_C_stator, stator_preliminary_feasibility_check;
        public Double stator_throat_radius_tip, stator_throat_radius_hub;

        //Windage_Enthalpy_Loss
        public Double Kf;
        public Double average_rho;
        public Double Windage_Torque_Loss;
        public Double turbine_rotational_speed;
        public Double Windage_Enthalpy_Loss;
        public Double Windage_Enthalpy_Loss_2;
        public Double dmdt;

        //Tip_Clearance_Loss
        public Double Kx, Kr, Kxr;
        public Double Cx, Cr;
        public Double Tip_Clearance_Loss, axial_clearance_ratio, radial_clearance_ratio;

        //Exit Energy Loss
        public Double Exit_Energy_Loss;

        //Trailing edge loss model 
        public Double G;
        public Double delta_P_total_relative;
        public Double trailing_edge_loss;
        public Double P_total_relative_rms_outlet;

        //Incidence loss model
        public Double optimum_beta;
        public Double incidence_loss;

        //Passage loss model
        public Double passage_loss;
        public Double blade_height_throat;
        public Double average_radius_throat;
        public Double radius_rms_throat;
        public Double U_rms_throat;
        public Double W_rms_throat;
        public Double passage_loss_Moustapha_et_al;
        public Double Dh, Lh;
        public Double passage_loss_Musgrave_and_Rodgers;
        public Double Cf, Cf_Laminar, Cf_Turbulent, laminar_turbulent_percentage;
        public Double Cfc_turbo, Cfc;
        public Double mean_relative_velocity, secondary_flow_loss, friction_loss;

        //Total Enthalpy Loos
        public Double enthalpy_loss;
        public Double Incidence_Loss_Contribution, Passage_Loss_Contribution, Trailing_Edge_Loss_Contribution;
        public Double Exit_Energy_Loss_Contribution, Tip_Clearance_Loss_Contribution, Windage_Loss_Contribution;

        //Efficiency_Total_Static
        public Double turbine_efficiency;

        //Feasibility_Check
        public Double Material_Density, Yield_Stress, Youngs_Modulus, Poissons_Ratio;
        public Double Kappa, elastic_stress_back_face, elastic_stress_trailing_edge, k_ratio;
        public Double Length_A, lambda_square_polynomial;
        public Double first_bending_mode, rotor_frequency;

        //Efficiency_Loop
        public Double efficiency_total_static, underrelaxation, residual_efficiency, residual;
        public int count_efficiency_iterations, max_iterations;
        public Double eta_static_reference;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox13.Enabled = true;
            }

            else if (radioButton1.Checked == false)
            {
                textBox13.Enabled = false;
            }
        }

        public Radial_Turbine_Design()
        {
            InitializeComponent();
        }

        //Calculate REFPROP Properties
        private void button1_Click(object sender, EventArgs e)
        {
            //Efficiency_Loop variables Initialization
            efficiency_total_static = 0.0;
            eta_static_reference = Convert.ToDouble(textBox30.Text);
            eta_static = 0.8;
            eta_total = 0.8;
            enthalpy_loss = 0;
            count_efficiency_iterations = 0;
            underrelaxation = 0.5;
            residual = 0.01;
            max_iterations = 15;

            //&& (Math.Abs(efficiency_total_static - eta_static) > residual)

            while ((count_efficiency_iterations < max_iterations) )
            {
                residual_efficiency = Math.Abs(efficiency_total_static - eta_static);

                //1. Total_Enthalpy_Difference, inlet_state (4) and outlet_state (6) calculation.
                Refrigerant working_fluid = new Refrigerant(RefrigerantCategory.NewMixture, this.comboBox2.Text + "=" + textBox10.Text + "," + this.comboBox4.Text + "=" + textBox9.Text, ReferenceState.DEF);

                //Inlet_State (4) Thermodynamic Conditions
                P_total_inlet = Convert.ToDouble(this.textBox1.Text);
                T_total_inlet = Convert.ToDouble(this.textBox2.Text);

                working_fluid.FindStateWithTP(T_total_inlet, P_total_inlet);

                Molar_mass = working_fluid.MolecularWeight;
                this.textBox11.Text = working_fluid.MolecularWeight.ToString();

                this.textBox3.Text = working_fluid.Density.ToString();
                this.textBox4.Text = working_fluid.Enthalpy.ToString();
                this.textBox5.Text = working_fluid.Entropy.ToString();

                Rho_total_inlet = Convert.ToDouble(working_fluid.Density.ToString());
                H_total_inlet = Convert.ToDouble(working_fluid.Enthalpy.ToString());
                S_total_inlet = Convert.ToDouble(working_fluid.Entropy.ToString());
                cp_inlet = Convert.ToDouble(working_fluid.cp);
                cv_inlet = Convert.ToDouble(working_fluid.cv);
                ganma_inlet = cp_inlet / cv_inlet;
                viscosity_inlet = working_fluid.viscosity / 1000000;

                //Outlet_State (6) Thermodynamic conditions

                //Pressure_Ratio_input option
                if (radioButton1.Checked == true)
                {
                    Objective_Pressure_ratio = Convert.ToDouble(this.textBox13.Text);
                    P_total_outlet = P_total_inlet / Objective_Pressure_ratio;

                    if (count_efficiency_iterations > 0)
                    {
                      eta_static = (1.0 - underrelaxation) * eta_static_reference + underrelaxation * efficiency_total_static;
                    }

                    else
                    {
                        eta_static = Convert.ToDouble(this.textBox12.Text);
                    }

                    count_efficiency_iterations = count_efficiency_iterations + 1;
                   
                    this.textBox18.Text = Convert.ToString(P_total_outlet);

                    Refrigerant working_fluid1 = new Refrigerant(RefrigerantCategory.NewMixture, comboBox2.Text + "=" + textBox10.Text + "," + comboBox4.Text + "=" + textBox9.Text, ReferenceState.DEF);

                    S_total_inlet_molar = S_total_inlet * Molar_mass;
                    working_fluid.FindStatueWithPS(P_total_outlet, S_total_inlet_molar);
                    H_total_outlet_isoentropic = working_fluid.Enthalpy;
                    H_total_outlet = H_total_inlet - (eta_static * (H_total_inlet - H_total_outlet_isoentropic));

                    working_fluid.FindStatueWithPH(P_total_outlet, H_total_outlet * Molar_mass);

                    Rho_total_outlet = Convert.ToDouble(working_fluid.Density.ToString());
                    H_total_outlet = Convert.ToDouble(working_fluid.Enthalpy.ToString());
                    S_total_outlet = Convert.ToDouble(working_fluid.Entropy.ToString());
                    T_total_outlet = Convert.ToDouble(working_fluid.Temperature.ToString());

                    this.textBox16.Text = Convert.ToString(Rho_total_outlet);
                    this.textBox15.Text = Convert.ToString(H_total_outlet);
                    this.textBox14.Text = Convert.ToString(S_total_outlet);
                    this.textBox17.Text = Convert.ToString(T_total_outlet);

                    this.textBox7.Text = working_fluid.CriticalTemperature.ToString();
                    this.textBox6.Text = working_fluid.CriticalPressure.ToString();
                    this.textBox8.Text = working_fluid.CriticalDensity.ToString();
                }

                //Output_Pressure option
                else if (radioButton2.Checked == true)
                {
                    P_total_outlet = Convert.ToDouble(this.textBox18.Text);

                    if (count_efficiency_iterations > 0)
                    {
                        eta_static = (1.0 - underrelaxation) * eta_static_reference + underrelaxation * efficiency_total_static;
                    }

                    else
                    {
                        eta_static = Convert.ToDouble(this.textBox12.Text);
                    }

                    count_efficiency_iterations = count_efficiency_iterations + 1;

                    Refrigerant working_fluid1 = new Refrigerant(RefrigerantCategory.NewMixture, comboBox2.Text + "=" + textBox10.Text + "," + comboBox4.Text + "=" + textBox9.Text, ReferenceState.DEF);

                    S_total_inlet_molar = S_total_inlet * Molar_mass;
                    working_fluid.FindStatueWithPS(P_total_outlet, S_total_inlet_molar);
                    H_total_outlet_isoentropic = working_fluid.Enthalpy;
                    H_total_outlet = H_total_inlet - (eta_static * (H_total_inlet - H_total_outlet_isoentropic));

                    working_fluid.FindStatueWithPH(P_total_outlet, H_total_outlet * Molar_mass);

                    Rho_total_outlet = Convert.ToDouble(working_fluid.Density.ToString());
                    H_total_outlet = Convert.ToDouble(working_fluid.Enthalpy.ToString());
                    S_total_outlet = Convert.ToDouble(working_fluid.Entropy.ToString());
                    T_total_outlet = Convert.ToDouble(working_fluid.Temperature.ToString());

                    this.textBox16.Text = Convert.ToString(Rho_total_outlet);
                    this.textBox15.Text = Convert.ToString(H_total_outlet);
                    this.textBox14.Text = Convert.ToString(S_total_outlet);
                    this.textBox17.Text = Convert.ToString(T_total_outlet);

                    this.textBox7.Text = working_fluid.CriticalTemperature.ToString();
                    this.textBox6.Text = working_fluid.CriticalPressure.ToString();
                    this.textBox8.Text = working_fluid.CriticalDensity.ToString();
                }

                total_entalphy_difference = H_total_inlet - H_total_outlet;
                this.textBox19.Text = Convert.ToString(total_entalphy_difference * 1000);

                //2. Mass_flow_rate calculation
                power = Convert.ToDouble(this.textBox20.Text);
                mass_flow_rate = power / total_entalphy_difference;
                this.textBox21.Text = Convert.ToString(mass_flow_rate);

                //3. ROTOR INLET SECTION. Velocity Triangle deffinition (state 4). Input data: flow coefficient, head coefficient and total_entalphy_difference

                //Head and Flow coefficient user input
                head_coefficient = Convert.ToDouble(this.textBox23.Text);
                flow_coefficient = Convert.ToDouble(this.textBox22.Text);

                //Rotational speed user input (r.p.m.)
                rotational_speed = Convert.ToDouble(this.textBox24.Text);

                //3.1. Rotor inlet blade tip speed (U4 or U_inlet)
                U_inlet = Math.Sqrt(total_entalphy_difference * 1000 / head_coefficient);

                //3.2. Rotor inlet absolute meridional flow velocity (Cm6 or Cm_outlet) 
                Cm_outlet = flow_coefficient * U_inlet;

                //3.3. Rotor inlet absolute tangential flow velocity (Cu4 or Ctheta_inlet) 
                Ctheta_inlet = head_coefficient * U_inlet;

                //3.4. Rotor inlet absolute meridional flow velocity (Cm4 or Cm_inlet) 
                Cm_inlet = ksi * flow_coefficient * U_inlet;

                //3.5. Rotor inlet absolute flow velocity (C4 or C_inlet)
                C_inlet = Math.Sqrt(Math.Pow(Ctheta_inlet, 2) + Math.Pow(Cm_inlet, 2));

                //3.6. Rotor inlet absolute flow angle (alfa4 or alpha_inlet)
                alpha_inlet = Math.Atan(Ctheta_inlet / Cm_inlet) * 180 / Math.PI;

                //3.7. Rotor inlet relative flow angle (beta4 or beta_inlet)
                beta_inlet = Math.Atan((Ctheta_inlet - U_inlet) / Cm_inlet) * 180 / Math.PI;

                //3.8. Rotor inlet tip radius (r4 or radius_inlet):
                radius_inlet = U_inlet / (rotational_speed * 2 * Math.PI / 60);

                //3.9. Incidence Angle (i):

                //3.10.1. Rotor inlet relative flow velocity, option 1 (W4 or W_inlet)
                W_inlet = Math.Sqrt(Math.Pow((U_inlet - Ctheta_inlet), 2) + Math.Pow(Cm_inlet, 2));

                //3.10.2. Rotor inlet relative flow velocity, option 2 (W4 or W_inlet)
                //Pending

                //3.11. Rotor static inlet Enthalpy (H4 or H_static_inlet)
                H_static_inlet = (H_total_inlet * 1000 - (Math.Pow(C_inlet, 2) / 2)) / 1000;

                //3.12. Rotor static inlet Isentropic Enthalpy (H4s or H_static_inlet_isoentropic)
                H_static_inlet_isoentropic = H_static_inlet;

                //3.13. Rotor static inlet Pressure, P4(H4s;S4s=S01) (P4 or P_static_inlet)
                working_fluid.FindStatueWithHS(H_static_inlet_isoentropic * Molar_mass, S_total_inlet_molar);
                P_static_inlet = working_fluid.Pressure;

                //3.14. Rotor static inlet Temperature T(P4=P4s; H4)(T4 or T_static_inlet)
                working_fluid.FindStatueWithPH(P_static_inlet, H_static_inlet * Molar_mass);
                T_static_inlet = working_fluid.Temperature;
                Speed_of_Sound_Rotor_inlet = working_fluid.speedofsound;

                //3.15. Rotor static inlet Density (rho4 or rho_static_inlet)
                rho_static_inlet = working_fluid.Density;

                //3.16. Rotor inlet area (A4 or area_inlet):
                area_inlet = mass_flow_rate / (rho_static_inlet * Cm_inlet);

                //3.17. Rotor_trailing_edge_thickness_From_file (tt)
                tt = 0.001;

                //3.18. Rotor Blade height inlet (b4 or blade_height_inlet):
                blade_height_inlet = area_inlet / (2.0 * Math.PI * radius_inlet - tt * number_blades);
                //blade_height_inlet = area_inlet / (2.0 * Math.PI * radius_inlet);

                //3.19. Rotor inlet absolute Mach number (Ma4 or Mach_absolute_inlet)
                Mach_absolute_inlet = C_inlet / Speed_of_Sound_Rotor_inlet;

                //3.20. Rotor inlet relative Mach number (Ma_rel4 or Mach_relative_inlet)
                Mach_relative_inlet = W_inlet / Speed_of_Sound_Rotor_inlet;

                //3.21. Rotor area_critical_inlet
                //a.Obtaining the Entrophy and Enthalpy with P_total and T_total: So(P_total,T_total), Ho(P_total,T_total), Speed_of_Sound(P_total,T_total)
                //b.Calculate the new Enthalpy: H_new = Ho-[(Speed_of_Sound^2)*0.5]
                //c.Obtaining Rho_new with H_new and So: Rho_new(H_new,So)
                //d.Calculate the Critical_Area: Critical_area = mass_flow/(Rho_new*Speed_of_Sound)

                Double So_rotor_inlet, Ho_rotor_inlet, Speed_Soundo_rotor_inlet;
                Double H_new_rotor_inlet, Rho_new_rotor_inlet;
                working_fluid.FindStateWithTP(T_total_inlet, P_total_inlet);
                So_rotor_inlet = working_fluid.Entropy;
                Ho_rotor_inlet = working_fluid.Enthalpy;
                Speed_Soundo_rotor_inlet = working_fluid.speedofsound;
                H_new_rotor_inlet = ((Ho_rotor_inlet * 1000) - ((Speed_Soundo_rotor_inlet * Speed_Soundo_rotor_inlet) * 0.5)) / 1000;
                working_fluid.FindStatueWithHS(H_new_rotor_inlet * Molar_mass, So_rotor_inlet * Molar_mass);
                Rho_new_rotor_inlet = working_fluid.Density;
                area_critical_inlet = mass_flow_rate / (Rho_new_rotor_inlet * Speed_Soundo_rotor_inlet);

                //4. ROTOR OUTLET SECTION. Velocity triangle deffinition. 

                //4.1. Rotor outlet absolute meridional flow velocity (Cm6 or Cm_outlet)
                Cm_outlet = Cm_inlet / ksi;

                //4.2. Rotor outlet absolute tangential flow velocity (Cu6) 
                Ctheta_outlet = 0.0; //No Exit Swirl Assumption

                //4.3. Rotor outlet absolute flow velocity (C6)
                C_outlet = Math.Sqrt(Math.Pow(Ctheta_outlet, 2) + Math.Pow(Cm_outlet, 2));

                //4.4. Rotor static outlet Enthalpy (H6 or H_static_outlet)
                H_static_outlet = ((H_total_outlet * 1000) - (0.5 * C_outlet * C_outlet)) / 1000;

                //4.5. Rotor static outlet Pressure, P6(H6;S6=S06) (P6 or P_static_outlet)
                S_total_outlet_molar = S_total_outlet * Molar_mass;
                working_fluid.FindStatueWithHS(H_static_outlet * Molar_mass, S_total_outlet_molar);
                P_static_outlet = working_fluid.Pressure;

                //4.6. Rotor static outlet Temperature, T6(H6;S6=S06) (T6 or T_static_outlet)
                S_total_outlet_molar = S_total_outlet * Molar_mass;
                working_fluid.FindStatueWithHS(H_static_outlet * Molar_mass, S_total_outlet_molar);
                T_static_outlet = working_fluid.Temperature;

                //4.7. P_ratio_outlet_static_inlet_total=P_total_inlet/P_static_outlet
                P_ratio_outlet_static_inlet_total = P_total_inlet / P_static_outlet;

                //4.8. Rotor static outlet Density, rho6(H6;S6=S06) (rho6 or rho_static_outlet)
                S_total_outlet_molar = S_total_outlet * Molar_mass;
                working_fluid.FindStatueWithHS(H_static_outlet * Molar_mass, S_total_outlet_molar);
                rho_static_outlet = working_fluid.Density;

                //4.9. Total Efficiency calculation: (ho1-h6/ho1-h06); (H_total_inlet-H_static_outlet)/(H_total_inlet-H_total_outlet_isoentropic)(eta_total)
                eta_total = (H_total_inlet - H_static_outlet) / (H_total_inlet - H_total_outlet_isoentropic);

                //4.10.

                //4.11. Rotor Outlet Radius Hub (r6h or radius_hub_outlet)
                radius_hub_outlet = radius_inlet * r_ratio;

                //4.12. Rotor outlet Area (A6 or area_outlet)
                area_outlet = mass_flow_rate / (rho_static_outlet * C_outlet);

                //4.13. Rotor outlet blade tip speed, U6h-Hub component (U6h or U_hub_outlet) 
                U_hub_outlet = (rotational_speed * 2 * Math.PI / 60) * radius_hub_outlet;

                //4.14. Rotor outlet relative flow velocity, W6h-Hub component, (W6h or W_hub_outlet)
                W_hub_outlet = Math.Sqrt(Math.Pow(Cm_outlet, 2) + Math.Pow((U_hub_outlet - Ctheta_outlet), 2));

                //4.15. Rotor outlet relative flow angle (Beta6h-Hub component, (beta6h or beta_hub_outlet)
                beta_hub_outlet = (Math.Atan((-U_hub_outlet - Ctheta_outlet) / Cm_outlet)) * 180 / Math.PI;

                specific_gas_constant = universal_gas_constant / (Molar_mass / 1000);

                P_static_outlet_in_pascales = P_static_outlet * 1000;

                //4.16. Rotor outlet blade height (b6 or blade_height_outlet)
                blade_height_outlet = BLADE_OUTLET_CALCULATION(rotational_speed, area_outlet, Ctheta_outlet, Cm_outlet, radius_hub_outlet
               , number_blades, tt, mass_flow_rate, specific_gas_constant, T_total_inlet, P_static_outlet_in_pascales, radius_inlet);

                //Calling BLADE_OUTLET_CALCULATION function:
                //rotational_speed = 160000;
                //area_outlet = 0.000177;
                //Ctheta_outlet = 0;
                //Cm_outlet = 91.891482;
                //radius_hub_outlet = 0.006094;
                //number_blades = 9;
                //tt = 0.001;
                //mass_flow_rate = 1.040157;
                //universal_gas_constant = 8.3144722; //[J/(K.mol)]
                //Molar_mass = 0.04401;
                //specific_gas_constant = universal_gas_constant / Molar_mass;
                //T_total_inlet = 833.15;
                //P_static_outlet = 9009009.009;
                //radius_inlet = 0.020312;

                //blade_height_outlet = BLADE_OUTLET_CALCULATION(rotational_speed,area_outlet,Ctheta_outlet,Cm_outlet,radius_hub_outlet 
                //,number_blades,tt,mass_flow_rate,specific_gas_constant,T_total_inlet,P_static_outlet,radius_inlet);

                //
                radius_tip_outlet = radius_hub_outlet + blade_height_outlet;

                //
                radius_ratio_inlet_outlet_tip = radius_inlet / radius_tip_outlet;

                //
                radius_rms_outlet = Math.Sqrt((Math.Pow(radius_tip_outlet, 2) + Math.Pow(radius_hub_outlet, 2)) / 2);

                //
                U_tip_outlet = (rotational_speed * 2 * Math.PI / 60) * radius_tip_outlet;

                //
                U_rms_outlet = (rotational_speed * 2 * Math.PI / 60) * radius_rms_outlet;

                //
                W_tip_outlet = Math.Sqrt(Math.Pow(Cm_outlet, 2) + Math.Pow((U_tip_outlet - Ctheta_outlet), 2));

                //
                W_rms_outlet = Math.Sqrt(Math.Pow(Cm_outlet, 2) + Math.Pow((U_rms_outlet - Ctheta_outlet), 2));

                //
                beta_tip_outlet = (Math.Atan((-U_tip_outlet - Ctheta_outlet) / Cm_outlet)) * 180 / Math.PI;

                //
                beta_rms_outlet = (Math.Atan((-U_rms_outlet - Ctheta_outlet) / Cm_outlet)) * 180 / Math.PI;

                //Rotor static outlet Temperature T(P6=P6s; H6)(T6 or T_static_inlet)
                working_fluid.FindStatueWithPH(P_static_outlet, H_static_outlet * Molar_mass);

                Speed_of_Sound_Rotor_outlet = working_fluid.speedofsound;

                //
                Mach_absolute_outlet = C_outlet / Speed_of_Sound_Rotor_outlet;

                //
                Mach_relative_tip_outlet = W_tip_outlet / Speed_of_Sound_Rotor_outlet;

                Mach_relative_rms_outlet = W_rms_outlet / Speed_of_Sound_Rotor_outlet;

                Mach_relative_hub_outlet = W_hub_outlet / Speed_of_Sound_Rotor_outlet;

                //Rotor area_critical_outlet
                //a.Obtaining the Entrophy and Enthalpy with P_total and T_total: So(P_total,T_total), Ho(P_total,T_total), Speed_of_Sound(P_total,T_total)
                //b.Calculate the new Enthalpy: H_new = Ho -[(Speed_of_Sound^2)*0.5]
                //c.Obtaining Rho_new with H_new and So: Rho_new(H_new,So)
                //d.Calculate the Critical_Area: Critical_area = mass_flow/(Rho_new*Speed_of_Sound)

                Double So_rotor_outlet, Ho_rotor_outlet, Speed_Soundo_rotor_outlet;
                Double H_new_rotor_outlet, Rho_new_rotor_outlet;
                working_fluid.FindStateWithTP(T_total_outlet, P_total_outlet);
                So_rotor_outlet = working_fluid.Entropy;
                Ho_rotor_outlet = working_fluid.Enthalpy;
                Speed_Soundo_rotor_outlet = working_fluid.speedofsound;
                H_new_rotor_outlet = ((Ho_rotor_outlet * 1000) - ((Speed_Soundo_rotor_outlet * Speed_Soundo_rotor_outlet) * 0.5)) / 1000;
                working_fluid.FindStatueWithHS(H_new_rotor_outlet * Molar_mass, So_rotor_outlet * Molar_mass);
                Rho_new_rotor_outlet = working_fluid.Density;
                area_critical_outlet = mass_flow_rate / (Rho_new_rotor_outlet * Speed_Soundo_rotor_outlet);

                //Calculation of total relative temperature and pressure (PENDING ????)

                // ADIMENSIONAL COEFFICIENTS
                //

                Vol_flow_rate = mass_flow_rate / Rho_total_inlet;

                omega = rotational_speed * 2 * Math.PI / 60;

                delta_h0 = total_entalphy_difference * 1000;

                specific_speed = (omega * Math.Pow(Vol_flow_rate, 0.5)) / (Math.Pow(delta_h0, 0.75));

                specific_diameter = 2 * radius_inlet * Math.Pow(delta_h0, (1.0 / 4.0)) / (Math.Pow((mass_flow_rate / rho_static_inlet), (1.0 / 2.0)));

                velocity_ratio = specific_speed * specific_diameter * (Math.Pow(eta_static, (1.0 / 2.0)) / (2.0 * Math.Sqrt(2.0)));

                NsDs = specific_speed * specific_diameter;

                rotor_tip_inlet_tip_outlet_length = (radius_inlet - radius_tip_outlet);

                rotor_radius_mean_outlet = (radius_tip_outlet + radius_hub_outlet) / 2.0;

                rotor_chord = Math.Sqrt(Math.Pow((rotor_tip_inlet_tip_outlet_length + blade_height_inlet / 2.0), 2.0) + Math.Pow((radius_inlet - rotor_radius_mean_outlet), 2.0));

                rotor_full_length = blade_height_inlet + rotor_tip_inlet_tip_outlet_length;

                rotor_meridional_length = (Math.PI / 2.0) * Math.Sqrt(1.0 / 2.0) * rotor_chord;

                rotor_outlet_area_total = Math.PI * (Math.Pow(radius_tip_outlet, 2) - Math.Pow(radius_hub_outlet, 2));

                rotor_outlet_area = rotor_outlet_area_total / number_blades;

                rotor_outlet_length_rms_total = 2 * Math.PI * radius_rms_outlet;

                rotor_outlet_rms_length = rotor_outlet_length_rms_total / number_blades;

                curvature_radius = Math.Sqrt(Math.Pow((rotor_tip_inlet_tip_outlet_length + (blade_height_inlet / 2.0 + blade_height_outlet / 2.0) / 2.0), 2.0));

                // CALCULATION OF THE AXIAL, RADIAL AND BACK FACE CLEARANCES; TRAILING EDGE THICKNESS:
                axial_clearance = 0.09 * blade_height_inlet;

                radial_clearance = 0.04 * blade_height_outlet;

                rotor_trailing_edge_thickness = tt;

                //IMPLEMENTATION OF PRESCRIBED ROTOR TRAILING EDGE THICKNESS AND CLEARANCES FROM THE INPUT FILE with the NASA TN D-5513 methods:
                //Notice that the Axial_clearance_From_File and the Radial_clearance_From_File are not the real value of axial and raidal clearance,
                //They are the axial and radial clearance ratio/[%].
                //So the axial_clearance = axial_clearance_From_File/100 * blade_height_inlet / (1 - axial_clearance_From_File/100)
                //The same, radial_clearance = radial_clearance_From_File/100 * blade_height_outlet / (1 - radial_clearance_From_File/100)
                //if (rotor_trailing_edge_thickness_From_File != 0.0000):
                //rotor_trailing_edge_thickness   = rotor_trailing_edge_thickness_From_File
                //
                if (axial_clearance_From_File != 0.0000)
                {
                    if (axial_clearance_From_File > 0.0000)
                    {
                        axial_clearance = axial_clearance_From_File;
                    }
                    else
                    {
                        axial_clearance = Math.Abs(axial_clearance_From_File / 100) * blade_height_inlet / (1 - Math.Abs(axial_clearance_From_File / 100));
                    }
                }
                //
                if (radial_clearance_From_File != 0.0000)
                {
                    if (radial_clearance_From_File > 0.0000)
                    {
                        radial_clearance = radial_clearance_From_File;
                    }

                    else
                    {
                        radial_clearance = Math.Abs(radial_clearance_From_File / 100) * blade_height_outlet / (1 - Math.Abs(radial_clearance_From_File / 100));
                    }
                    // If you input "-7.0", that means the radial_clearance is equal to 0.07 times of passage height, and the passage height is euqal to the blade_height + clearance
                    if (back_face_clearance_From_File != 0.0000)
                    {
                        back_face_clearance = back_face_clearance_From_File;
                    }

                    // THE BACK FACE CLEARANCE IS CONSIDERED EQUAL TO THE AXIAL CLEARANCE:
                    back_face_clearance = axial_clearance;

                    // SETTING A REASONABLE MINIMUM AND MAXIMUM FOR THE AXIAL AND RADIAL CLEARANCES:
                    //
                    if (axial_clearance < 0.0001)
                    {
                        axial_clearance = 0.0001;
                    }
                    //   
                    if (radial_clearance < 0.0001)
                    {
                        radial_clearance = 0.0001;
                    }
                    //
                    if (axial_clearance > 0.0015)
                    {
                        axial_clearance = 0.001;
                    }
                    //
                    if (radial_clearance > 0.0015)
                    {
                        radial_clearance = 0.001;
                    }

                    //rotor_critical_area = area_critical_inlet;

                    //if (area_critical_outlet < rotor_critical_area)
                    //{
                    //    rotor_critical_area = area_critical_outlet;
                    //}
                }

                //Double critical_area = 0.000032;//PENDING area_critical_inlet
                //number_blades = 9;
                //blade_height_inlet = 0.000966;
                //radius_inlet = 0.020312;
                //radius_outlet = 0.010471;
                //radius_hub_outlet = 0.006094;
                //Double angle_outlet = -57.372173;
                //Double blade_height = 0.004377;
                //mass_flow_rate = 1.040157;
                //rho_static_outlet = 63.845977;
                //Double gama = 1.239186;
                //specific_gas_constant = 8.3144722 / 0.04401;
                //T_static_outlet = 742.397017;
                //Double outlet_length = 0.005981;
                //Double outlet_area = 0.000025;
                //Double trailing_edge_thickness = 0.001;
                //T_total_inlet = 833.15;
                //Double rho_total_inlet = 122.77559;
                //T_total_outlet = 746.224052;
                //Double rho_total_outlet = 65.397886;
                //Double Pi = 3.1416;

                ROTOR_THROAT_CALCULATION(area_critical_inlet, Math.PI, number_blades, blade_height_inlet, radius_inlet, radius_tip_outlet, radius_hub_outlet, beta_rms_outlet, blade_height_outlet,
                                   mass_flow_rate, rho_static_outlet, ganma_inlet, specific_gas_constant, T_static_outlet, rotor_outlet_rms_length, rotor_outlet_area, rotor_trailing_edge_thickness,
                                   T_total_inlet, Rho_total_inlet, T_total_outlet, Rho_total_outlet, ref ksi_rotor_throat, ref omega_rotor_throat, ref rotor_throat_length,
                                   ref rotor_throat_area, ref distance_A_C_rotor, ref rotor_preliminary_feasibility_check, ref r_tip_throat, ref r_hub_throat);

                shroud_meridional_distance = 2.0 * Math.PI * rotor_tip_inlet_tip_outlet_length / 4.0;

                ratio_meridional_distance_throat = distance_A_C_rotor / shroud_meridional_distance;

                phi_wheel = 90.0 * ratio_meridional_distance_throat;

                throat_radius = radius_tip_outlet + (rotor_tip_inlet_tip_outlet_length - (rotor_tip_inlet_tip_outlet_length * Math.Cos(phi_wheel * Math.PI / 180)));

                static_enthalpy_difference = (total_entalphy_difference * 1000) + Math.Pow(C_outlet, 2.0) / 2.0 - Math.Pow(C_inlet, 2.0) / 2.0;

                Reynolds_Number_Rotor = rho_static_inlet * W_inlet * rotor_chord / viscosity_inlet;

                //ROTOR Results:
                textBox25.Text = "#Fluid: " + Convert.ToString(comboBox2.Text) +
                Environment.NewLine + "Number_of_Turbine: " + "1" +
                Environment.NewLine + "Flow_Coefficient: " + Convert.ToString(decimal.Round(Convert.ToDecimal(flow_coefficient), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Head_Coefficient: " + Convert.ToString(decimal.Round(Convert.ToDecimal(head_coefficient), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Turbine_Rotor_Rotational_Speed: " + Convert.ToString(rotational_speed) +
                Environment.NewLine + "REFPROP_Treatment: " + "REAL" +
                Environment.NewLine + "total_enthalpy_difference: " + Convert.ToString(decimal.Round(Convert.ToDecimal(total_entalphy_difference * 1000), 3, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "static_enthalpy_difference: " + Convert.ToString(decimal.Round(Convert.ToDecimal(static_enthalpy_difference), 3, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "T_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_total_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "T_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_static_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "P_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_total_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "P_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_static_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Rho_total_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rho_static_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mass_Flow_Rate: " + Convert.ToString(decimal.Round(Convert.ToDecimal(mass_flow_rate), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Power: " + Convert.ToString(decimal.Round(Convert.ToDecimal(mass_flow_rate), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Cp: " + Convert.ToString(decimal.Round(Convert.ToDecimal(cp_inlet), 3, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Viscosity: " + Convert.ToString(decimal.Round(Convert.ToDecimal(viscosity_inlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "ganma: " + Convert.ToString(decimal.Round(Convert.ToDecimal(ganma_inlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Molar_Mass: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Molar_mass), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "U_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "C_intlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(C_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Ctheta_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Ctheta_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Cm_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Cm_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "alpha_inlet (deg): " + Convert.ToString(decimal.Round(Convert.ToDecimal(alpha_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_inlet (deg): " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_inlet (m): " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "blade_height_inlet (m): " + Convert.ToString(decimal.Round(Convert.ToDecimal(blade_height_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "area_inlet (m2): " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_inlet), 5, MidpointRounding.AwayFromZero)) +
                 Environment.NewLine + "area_critical_inlet (m2): " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_critical_inlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_absolute_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_absolute_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "T_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_total_outlet), 2, MidpointRounding.AwayFromZero)) +
                //Environment.NewLine + "T_total_relative_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_total_relative_rms_outlet), 2, MidpointRounding.AwayFromZero))
                Environment.NewLine + "T_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_static_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "P_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_total_outlet), 2, MidpointRounding.AwayFromZero)) +
                //Environment.NewLine + "P_total_relative_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_total_relative_rms_outlet), 2, MidpointRounding.AwayFromZero))+
                Environment.NewLine + "P_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_static_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Rho_total_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rho_static_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Total_static_pressure_ratio: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_ratio_outlet_static_inlet_total), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "U_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "U_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "U_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "C_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(C_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Ctheta_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Ctheta_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Cm_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Cm_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_radius_mean_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_radius_mean_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "blade_height_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(blade_height_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "area_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_area_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_area_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_rms_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_rms_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_length_rms_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_length_rms_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "area_critical_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_critical_outlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_absolute_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_absolute_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "number_blades: " + Convert.ToString(decimal.Round(Convert.ToDecimal(number_blades), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_chord: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_chord), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_full_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_full_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_meridional_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_meridional_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_trailing_edge_thickness: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_trailing_edge_thickness), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "axial_clearance: " + Convert.ToString(decimal.Round(Convert.ToDecimal(axial_clearance), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radial_clearance: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radial_clearance), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "back_face_clearance: " + Convert.ToString(decimal.Round(Convert.ToDecimal(back_face_clearance), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "curvature_radius: " + Convert.ToString(decimal.Round(Convert.ToDecimal(curvature_radius), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_ratio_inlet_outlet_tip: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_ratio_inlet_outlet_tip), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_tip_inlet_tip_outlet_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_tip_inlet_tip_outlet_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_throat_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_throat_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_throat_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_throat_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "throat_radius: " + Convert.ToString(decimal.Round(Convert.ToDecimal(throat_radius), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "r_tip_throat: " + Convert.ToString(decimal.Round(Convert.ToDecimal(r_tip_throat), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "r_hub_throat: " + Convert.ToString(decimal.Round(Convert.ToDecimal(r_hub_throat), 5, MidpointRounding.AwayFromZero)) +
                // Environment.NewLine + "rotor_critical_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_critical_area), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "specific_speed: " + Convert.ToString(decimal.Round(Convert.ToDecimal(specific_speed), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "specific_diameter: " + Convert.ToString(decimal.Round(Convert.ToDecimal(specific_diameter), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "velocity_ratio: " + Convert.ToString(decimal.Round(Convert.ToDecimal(velocity_ratio), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "NsDs: " + Convert.ToString(decimal.Round(Convert.ToDecimal(NsDs), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Reynolds_Number_Rotor: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Reynolds_Number_Rotor), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "eta_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(eta_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "turbine_efficiency: " + Convert.ToString(decimal.Round(Convert.ToDecimal(turbine_efficiency), 5, MidpointRounding.AwayFromZero));
                // Environment.NewLine + "turbine_efficiency: " + Convert.ToString(decimal.Round(Convert.ToDecimal(turbine_efficiency), 2, MidpointRounding.AwayFromZero)) +

                //5. STATOR INLET. 
                //STATOR_CALCULATION(fluid, blade_height_inlet, alpha_inlet, radius_inlet, inlet_total_temperature, P_static_inlet,
                //specific_gas_constant, rho_static_inlet, rho_total_inlet, inlet_total_pressure, T_static_inlet, mass_flow_rate, Ctheta_inlet,
                //Cm_inlet, Cp, gama, number_blades, Pi, stator_solidity_input, stator_radius_inlet_radius_outlet_ratio, stator_radius_outlet_rotor_inlet_ratio, 
                //radius_hub_outlet, area_critical_inlet, Input_Conditions)

                stator_blade_height = blade_height_inlet;
                stator_alpha_inlet = alpha_inlet;

                //stator_radius_outlet = stator_radius_outlet_rotor_inlet_ratio * radius_inlet #5% WAS ASSUMED FOR THE INTERSPACE (COMMON TO SEVERAL PROGRAMS = NASA); ALSO: GT 2008-50261 IS A GOOD REFERENCE TO JUSTIFY THIS.
                //OTHER WAY OF CALCULATING THE OUTLET RADIUS OF THE STATOR IS REFERRED IN WATANABE, I. ARIGA, I, MASHIMO, T., "Effect of Dimensional Parameters of Impellers on  Performance Characteristics of a Radial Inflow Turbine", Trans. ASME, Journ Eng for Power, 93, 81-102.
                delta_radius = 2.0 * stator_blade_height * Math.Cos(stator_alpha_inlet * Math.PI / 180);
                stator_radius_outlet = delta_radius + radius_inlet;
                stator_radius_inlet = stator_radius_inlet_radius_outlet_ratio * stator_radius_outlet; // 25% OF THE ROTOR RADIUS PLUS INTERSPACE IS ASSUMED FOR THE ROTOR RADIUS DIFFERENCE (COMMON TO THE NASA PROGRAM)
                stator_alpha_outlet = stator_alpha_inlet;
                //
                stator_T_total_inlet = T_total_inlet; //ISENTROPIC EXPANSION IS ASSUMED FOR THE STATOR
                stator_T_static_inlet = T_static_inlet; //initial guess T_static_inlet is from rotor
                stator_P_total_inlet = P_total_inlet;
                stator_P_static_inlet = P_static_inlet;
                //
                Double So_stator_outlet, Ho_stator_outlet, Speed_Soundo_stator_outlet;
                Double H_new_stator_outlet, Rho_new_stator_outlet;
                working_fluid.FindStateWithTP(stator_T_total_inlet, stator_P_total_inlet);
                stator_rho_total_inlet = working_fluid.Density;
                So_stator_outlet = working_fluid.Entropy;
                Ho_stator_outlet = working_fluid.Enthalpy;
                Speed_Soundo_stator_outlet = working_fluid.speedofsound;
                H_new_stator_outlet = ((Ho_stator_outlet * 1000) - ((Speed_Soundo_stator_outlet * Speed_Soundo_stator_outlet) * 0.5)) / 1000;
                working_fluid.FindStatueWithHS(H_new_stator_outlet * Molar_mass, So_stator_outlet * Molar_mass);
                Rho_new_stator_outlet = working_fluid.Density;
                stator_critical_area = mass_flow_rate / (Rho_new_stator_outlet * Speed_Soundo_stator_outlet);

                stator_rho_static_outlet = rho_static_inlet; //INITIALISATION FROM ROTOR:
                stator_rho_total_outlet = Rho_total_inlet;  //INITIALISATION FROM ROTOR:
                                                            //
                stator_blade_inclination = Math.Tan((90.0 - stator_alpha_outlet) * Math.PI / 180);
                b_line_stator = stator_radius_outlet;
                A_quadratic_function_stator = 1.0 + (1.0 / Math.Pow(stator_blade_inclination, 2));
                B_quadratic_function_stator = -(2.0 * b_line_stator / Math.Pow(stator_blade_inclination, 2));
                C_quadratic_function_stator = Math.Pow(b_line_stator, 2) / Math.Pow(stator_blade_inclination, 2) - Math.Pow(stator_radius_inlet, 2);
                y_positive_stator = (-B_quadratic_function_stator + Math.Sqrt(Math.Pow(B_quadratic_function_stator, 2) - 4.0 * A_quadratic_function_stator * C_quadratic_function_stator)) / (2 * A_quadratic_function_stator);
                y_negative_stator = (-B_quadratic_function_stator - Math.Sqrt(Math.Pow(B_quadratic_function_stator, 2) - 4.0 * A_quadratic_function_stator * C_quadratic_function_stator)) / (2 * A_quadratic_function_stator);
                //
                if (y_negative_stator > y_positive_stator)
                {
                    y_positive_stator = y_positive_stator;
                }
                //
                stator_inlet_y_coordinate = y_positive_stator;
                stator_inlet_x_coordinate = (stator_inlet_y_coordinate - b_line_stator) / stator_blade_inclination;
                //
                stator_blade_length = Math.Sqrt(Math.Pow((stator_inlet_x_coordinate - 0.0), 2.0) + Math.Pow((stator_inlet_y_coordinate - stator_radius_outlet), 2.0));
                stator_chord = stator_blade_length;
                stator_T_total_outlet = T_total_inlet;
                stator_T_static_outlet = T_static_inlet;
                stator_P_total_outlet = P_total_inlet;
                stator_P_static_outlet = P_static_inlet;
                //
                working_fluid.FindStateWithTP(stator_T_static_inlet, stator_P_static_inlet);
                stator_rho_static_inlet = working_fluid.Density;
                stator_Cm_inlet = mass_flow_rate / stator_rho_static_inlet / (2.0 * Math.PI * stator_radius_inlet * stator_blade_height);
                stator_Ctheta_inlet = stator_Cm_inlet * Math.Tan(Math.Atan(Ctheta_inlet / Cm_inlet));// THE ANGLE IS ASSUMED TO BE CONSTANT THROUGHOUT THE STATOR AND IT IS EQUAL TO THE ROTOR ABSOLUTE INLET ANGLE "ALPHA"
                stator_C_inlet = stator_Cm_inlet / (Math.Cos(Math.Atan(Ctheta_inlet / Cm_inlet)));
                //
                working_fluid.FindStateWithTP(stator_T_total_inlet, stator_P_total_inlet);
                Double So_stator_inlet, Ho_stator_inlet;
                Double H_new_stator_inlet;
                So_stator_inlet = working_fluid.Entropy;
                Ho_stator_inlet = working_fluid.Enthalpy;
                H_new_stator_inlet = ((Ho_stator_inlet * 1000) - ((stator_C_inlet * stator_C_inlet) * 0.5)) / 1000;
                working_fluid.FindStatueWithHS(H_new_stator_inlet * Molar_mass, So_stator_inlet * Molar_mass);
                stator_T_static_inlet_New = working_fluid.Temperature;

                while (Math.Abs(stator_T_static_inlet - stator_T_static_inlet_New) > 0.0001)
                {
                    stator_T_static_inlet = stator_T_static_inlet_New;
                    stator_rho_static_inlet = working_fluid.Density;
                    stator_Cm_inlet = mass_flow_rate / (stator_rho_static_inlet * 2.0 * Math.PI * stator_radius_inlet * stator_blade_height);
                    stator_C_inlet = stator_Cm_inlet / (Math.Cos(Math.Atan(Ctheta_inlet / Cm_inlet)));

                    H_new_stator_inlet = ((Ho_stator_inlet * 1000) - ((stator_C_inlet * stator_C_inlet) * 0.5)) / 1000;
                    working_fluid.FindStatueWithHS(H_new_stator_inlet * Molar_mass, So_stator_inlet * Molar_mass);
                    stator_T_static_inlet_New = working_fluid.Temperature;
                };

                stator_P_static_inlet = working_fluid.Pressure;
                stator_Ctheta_inlet = stator_Cm_inlet * Math.Tan(Math.Atan(Ctheta_inlet / Cm_inlet)); //THE ANGLE IS ASSUMED TO BE CONSTANT THROUGHOUT THE STATOR AND IT IS EQUAL TO THE ROTOR ABSOLUTE INLET ANGLE "ALPHA"
                stator_C_inlet = stator_Cm_inlet / (Math.Cos(Math.Atan(Ctheta_inlet / Cm_inlet)));

                stator_solidity = stator_blade_length / (Math.PI * 2.0 * stator_radius_outlet / stator_number_vanes);
                stator_outlet_length_total = 2 * Math.PI * stator_radius_outlet;
                stator_outlet_length = stator_outlet_length_total / stator_number_vanes;
                stator_outlet_area_total = stator_outlet_length_total * stator_blade_height;
                stator_outlet_area = stator_outlet_length * stator_blade_height;
                stator_inlet_length_total = 2 * Math.PI * stator_radius_inlet;
                stator_inlet_length = stator_inlet_length_total / stator_number_vanes;
                stator_area_inlet_total = stator_inlet_length_total * stator_blade_height;
                stator_area_inlet = stator_inlet_length * stator_blade_height;

                //THE FLOW VELOCITY AT THE STATOR OUTLET IS ASSUMED TO BE THE SAME AS FOR THE ROTOR INLET:
                stator_C_outlet = Math.Sqrt(Math.Pow(Ctheta_inlet, 2) + Math.Pow(Cm_inlet, 2));
                stator_Ctheta_outlet = Ctheta_inlet;
                stator_Cm_outlet = Cm_inlet;

                //CALCULATION OF REYNOLDS NUMBER BASED ON THE NASA REPORT TN D-8164:
                Reynolds_Number_Stator = stator_rho_static_inlet * stator_C_inlet * stator_chord / viscosity_inlet;

                //THE CALCULATION OF THE STATOR TRAILING EDGE THICKNESS IS BASED ON THE ASSUMPTIONS PRESENTED IN THE NASA REPORT TN D-8164
                stator_trailing_edge_thickness = 0.04 * stator_blade_height;

                // SETTING A REASONABLE MINIMUM AND MAXIMUM FOR THE STATOR TRAILING EDGE THICKNESS:
                //
                if (stator_trailing_edge_thickness < 0.0005)
                {
                    stator_trailing_edge_thickness = 0.0005;
                }
                //    
                if (stator_trailing_edge_thickness > 0.003)
                {
                    stator_trailing_edge_thickness = 0.003;
                }

                // IMPLEMENTATION OF PRESCRIBED STATOR TRAILING EDGE THICKNESS FROM THE INPUT FILE:
                //if (Geometry_Definitions == 1)
                //{
                //    if (stator_trailing_edge_thickness_From_File != 0.0000)
                //    {
                //        stator_trailing_edge_thickness = stator_trailing_edge_thickness_From_File;
                //    }
                //}

                //START CALCULATION TO FIND THE STATOR THROAT AREA:

                //ksi_stator_throat, omega_stator_throat, stator_throat_length, stator_throat_area, \
                //distance_A_C_stator, preliminary_feasibility_check, stator_throat_radius_tip, stator_throat_radius_hub = THROAT_CALCULATION(stator_critical_area, Pi, \
                //stator_number_vanes, stator_blade_height, stator_radius_inlet, stator_radius_outlet, \
                //radius_hub_outlet, stator_alpha_outlet, stator_blade_height, mass_flow_rate, stator_rho_static_outlet, gama, \
                //specific_gas_constant, stator_T_static_outlet, stator_outlet_length, stator_outlet_area, stator_trailing_edge_thickness, stator_T_total_inlet, stator_rho_total_inlet,\
                //stator_T_total_outlet, stator_rho_total_outlet, "STATOR")

                //Double stator_critical_area = 0.000032;//PENDING 
                //stator_number_vanes = 11;
                //stator_blade_height = 0.000966;
                //stator_radius_inlet = 0.026138;
                //stator_radius_outlet = 0.02091;
                //radius_hub_outlet = 0.006094;
                //stator_alpha_outlet = 71.980231;

                //mass_flow_rate = 1.040157;
                //stator_rho_static_outlet = 98.752819;
                //Double gama = 1.239186;
                //specific_gas_constant = 8.3144722 / 0.04401;
                //stator_T_static_outlet = 794.287503;
                //stator_outlet_length = 0.011944;
                //stator_outlet_area = 0.000012;
                //stator_trailing_edge_thickness = 0.0005;
                //stator_T_total_inlet = 833.15;
                //stator_rho_total_inlet = 122.77559;
                //stator_T_total_outlet = 833.15;
                //stator_rho_total_outlet = 122.77559;

                STATOR_THROAT_CALCULATION(stator_critical_area, Math.PI, stator_number_vanes, stator_blade_height, stator_radius_inlet, stator_radius_outlet, radius_hub_outlet, stator_alpha_outlet, stator_blade_height,
                                   mass_flow_rate, stator_rho_static_outlet, ganma_inlet, specific_gas_constant, stator_T_static_outlet, stator_outlet_length, stator_outlet_area, stator_trailing_edge_thickness,
                                   stator_T_total_inlet, stator_rho_total_inlet, stator_T_total_outlet, stator_rho_total_outlet, ref ksi_stator_throat, ref omega_stator_throat, ref stator_throat_length,
                                   ref stator_throat_area, ref distance_A_C_stator, ref stator_preliminary_feasibility_check, ref stator_throat_radius_tip, ref stator_throat_radius_hub);

                //ksi_stator_throat, omega_stator_throat, stator_throat_length, stator_throat_area, \
                //distance_A_C_stator, preliminary_feasibility_check, stator_throat_radius_tip, stator_throat_radius_hub =

                //THE STATOR THHROAT SHROUD (TIP) RADIUS AND STATOR THROAT HUB RADIUS ARE THE SAME DUE TO THE QUASI-2D CONFIGURATION OF THIS STATION.
                stator_throat_radius = stator_throat_radius_tip;

                //STATOR Results:
                textBox26.Text = "stator_T_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_total_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_T_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_static_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_total_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_static_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_total_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_static_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_C_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_C_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Ctheta_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Ctheta_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Cm_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Cm_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_alpha_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_alpha_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_radius_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_radius_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_blade_height: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_blade_height), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_blade_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_blade_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_chord: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_chord), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_inlet_x_coordinate: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_inlet_x_coordinate), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_inlet_y_coordinate: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_inlet_y_coordinate), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_area_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_area_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_area_inlet_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_area_inlet_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_T_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_total_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_T_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_static_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_total_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_static_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_total_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_static_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_C_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_C_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Ctheta_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Ctheta_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Cm_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Cm_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_alpha_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_alpha_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_radius_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_radius_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_length_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_length_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_area_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_area_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_number_vanes: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_number_vanes), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_solidity: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_solidity), 5, MidpointRounding.AwayFromZero)) +
                //Environment.NewLine + "stator_critical_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_critical_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_trailing_edge_thickness: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_trailing_edge_thickness), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_throat_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_throat_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_throat_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_throat_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_throat_radius: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_throat_radius), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Reynolds_Number_Stator: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Reynolds_Number_Stator), 5, MidpointRounding.AwayFromZero));


                //ENTHALPY LOSS MODELS

                //6. WINDAGE_LOSS
                dmdt = mass_flow_rate;
                back_face_clearance = 0.000096;
                turbine_rotational_speed = rotational_speed * 2 * Math.PI / 60;

                if (Reynolds_Number_Rotor < 100000)
                {
                    Kf = 3.7 * Math.Pow((back_face_clearance / radius_inlet), 0.1) / Math.Sqrt(Reynolds_Number_Rotor);
                }
                else
                {
                    Kf = 0.102 * Math.Pow((back_face_clearance / radius_inlet), 0.1) / Math.Pow(Reynolds_Number_Rotor, 0.2);
                }

                average_rho = (rho_static_inlet + rho_static_outlet) / 2.0;

                Windage_Torque_Loss = Kf * (1.0 / 4.0) * average_rho * Math.Pow(turbine_rotational_speed, 2.0) * Math.Pow(radius_inlet, 2.0);

                // UNITS OF Windage_Torque_Loss [-]*[kg/(m^3)]*[rad^2/s^2]*[m^2] = [kg/(m*s^2)] = [J/m^3] THEREFORE IF DIVIDED BY rho = [J/kg]

                Windage_Enthalpy_Loss = Windage_Torque_Loss / average_rho;

                Windage_Enthalpy_Loss_2 = 0.5 * average_rho * Math.Pow(U_inlet, 3) * Math.Pow(radius_inlet, 2) * Kf / (dmdt * Math.Pow(W_rms_outlet, 2));

                if (Windage_Enthalpy_Loss_2 > Windage_Enthalpy_Loss)
                {
                    Windage_Enthalpy_Loss = Windage_Enthalpy_Loss_2;
                }

                if (Windage_Enthalpy_Loss < 0.0)
                {
                    Windage_Enthalpy_Loss = 0.0;
                }

                //7. TIP_CLEARANCE_LOSS
                Kx = 0.40;
                Kr = 0.75;
                Kxr = -0.30;

                axial_clearance = 0.0001;
                axial_clearance_From_File = 0.0001;
                radial_clearance = 0.000182;

                Cx = (1.0 - (radius_tip_outlet / radius_inlet)) / (Cm_inlet * blade_height_inlet);
                Cr = (radius_tip_outlet / radius_inlet) * ((rotor_full_length - blade_height_inlet) / (Cm_outlet * radius_rms_outlet * blade_height_outlet));
                // The TOPGEN define using the same methods for calculation the tip_clearance loss. So just define the axial_clearance_From_File only.
                //if (axial_clearance_From_File >= 0.000)
                //{
                //    Tip_Clearance_Loss = Math.Pow(U_inlet, 3) * number_blades / (8.0 * Math.PI) * (Kx * axial_clearance * Cx + Kr * radial_clearance * Cr + Kxr * Math.Sqrt(axial_clearance * radial_clearance * Cx * Cr));
                //}
                // print "automatically deltah0", deltah0
                // print "Tip_Clearance_Loss with automatically", Tip_Clearance_Loss


                // This NASA tip_clearance_model is based on the radial_celarance ratio and axial_clearance_ratio.
                // The eta_loss= b*A + e*R+ f*R**2 +h*A*R = -0.0968 * A  - 0.1700 * R + 0.0968 * R**2 - 0.0338 * A * R 
                // A and R are not the real radiaus Axial_clearance and Radial_clearance, they are the ratio of axial and radiao clerance.
                // A = axial_clearance/passage_height = axial_clearance/(axial_clearance + blade_height_inlet)
                // same, B = radial_clearance/passage_height = radial_clearance/(radial_clearance + blade_height_outlet)
                // due to that the real clearance should have a limit value, like:
                // axial_clearance should large or euqal to 0.000005 m 
                //else
                //{
                // in order to set the limit value from the function for rotor calculation, here re-calculate the axial_clearance_From_File
                // and the radial_clearance_From_File
                // For example, if the axial_clearance_From_File is set to 1 %, the real axial_clearance is below 0.0001, such like 0.00007[m]
                // The rotor calculation function will reset the axial_clearance to 0.0001[m] due to the safeguard.
                // then the following code should be recalculate the axial_clearance_From_File, enhance the ratio to 2% or a larger value.
                axial_clearance_ratio = axial_clearance / (axial_clearance + blade_height_inlet) * 100;
                radial_clearance_ratio = radial_clearance / (radial_clearance + blade_height_outlet) * 100;
                Tip_Clearance_Loss = -(delta_h0 * ((-0.09678149 * axial_clearance_ratio) - 1.699972344 * radial_clearance_ratio + 0.096843787 * Math.Pow(radial_clearance_ratio, 2) - 0.033789711 * axial_clearance_ratio * radial_clearance_ratio)) / 100;
                // print "NASA deltah0", deltah0
                // print "Tip_Clearance_Loss with NASA", Tip_Clearance_Loss
                // That means we will calculate the tip_cleanrance_loss in the final efficiency calculation district. 
                // print axial_clearance_ratio
                // print Tip_Clearance_Loss
                // print Tip_Clearance_Loss/deltah0
                // This should be a safeguard for the TIp_Clearance_Loss
                //}

                if (Tip_Clearance_Loss < 0.0)
                {
                    Tip_Clearance_Loss = 0.0;
                }

                //8.EXIT_ENERGY_LOSS
                Exit_Energy_Loss = 1.0 / 2.0 * Math.Pow(C_outlet, 2);

                if (Exit_Energy_Loss < 0.0)
                {
                    Exit_Energy_Loss = 0.0;
                }

                //9.TRAILING_EDGE_LOSS_MODEL
                P_total_relative_rms_outlet = 9975436.724; //PENDING TO BE CALCULATED
                G = 9.80665; // GRAVITATIONAL ACCELERATION [m/s^2]
                             //THIS LOSS MODEL IS IMPLEMENTED AS IN Gosh, S. K. et al., "Mathematical Analysis for Off-Design Perfornamce of Cryogenic Turboexpander", Journal of Fluids Engineering, Vol. 133, 031001-(1-5) 
                delta_P_total_relative = rho_static_outlet * Math.Pow(W_rms_outlet, 2) / (2.0 * G) * Math.Pow((number_blades * rotor_trailing_edge_thickness) / (Math.PI * (radius_hub_outlet + radius_tip_outlet) * Math.Cos((beta_rms_outlet * Math.PI / 180))), 2);
                trailing_edge_loss = 2.0 / (ganma_inlet * Math.Pow(Mach_relative_rms_outlet, 2)) * delta_P_total_relative / P_total_relative_rms_outlet;

                if (trailing_edge_loss < 0.0)
                {
                    trailing_edge_loss = 0.0;
                }

                //9. INCIDENCE_LOSS_MODEL
                // NASA MODEL WITH optimum_beta = 0.0:
                optimum_beta = 0.0;
                incidence_loss = (1.0 / 2.0) * Math.Pow(W_inlet, 2) * Math.Pow(Math.Sin((beta_inlet * Math.PI / 180) - (optimum_beta * Math.PI / 180)), 2);

                if (incidence_loss < 0.0)
                {
                    incidence_loss = 0.0;
                }

                //10. PASSAGE_LOSS_MODEL
                Double k_friction;
                Double Dh;
                Double Lh;
                Double tol1;
                Double tol2;
                Double Cf_Turbulent_1;
                Double Cf_Turbulent_2;
                Double Kp;

                //INITIALIZATION:
                passage_loss = 0.0;
                k_friction = 0.0;
                Dh = 0.0;
                Lh = 0.0;
                tol1 = 0.0;
                tol2 = 0.0;
                Cf_Turbulent_1 = 0.0;
                Cf_Turbulent_2 = 0.0;

                Kp = 0.11;
                k_friction = 0.2E-03; // 0.2E-03 [m] IS THE HIGHEST VALUE FOR TURBINES.

                //LIMITS FOR Cf IN THE TURBULENT REGIME:
                Cf_Turbulent_1 = 1.0E-12;
                Cf_Turbulent_2 = 10000000;

                //DEFINE TOLERANCES FOR TURBULENT REGIME BISSECTION METHOD:
                tol1 = 1.0E-09;
                tol2 = 1.0E-09;

                //GEOMETRIC VARIABLES:
                blade_height_throat = r_tip_throat - r_hub_throat;
                average_radius_throat = (r_tip_throat + r_hub_throat) / 2.0;

                radius_rms_throat = Math.Sqrt((Math.Pow(r_tip_throat, 2.0) + Math.Pow(r_hub_throat, 2.0)) / 2.0);
                U_rms_throat = turbine_rotational_speed * radius_rms_throat;
                W_rms_throat = Math.Sqrt(Math.Pow(Cm_outlet, 2.0) + Math.Pow((U_rms_throat - Ctheta_outlet), 2));

                //BASED ON THE LOSS MODEL PROPOSED BY  Moustapha et al., "Axial and Radial Turbines", Concepts NREC, 2003
                //passage_loss_Moustapha_et_al, Dh, Lh = MOUSTAPHA_ET_AL_PASSAGE_LOSS_MODEL(Turbine_Dic, Input_Conditions);

                MOUSTAPHA_ET_AL_PASSAGE_LOSS_MODEL(Kp, radius_hub_outlet, radius_tip_outlet, rotor_meridional_length, radius_inlet, blade_height_inlet,
                                                   number_blades, beta_rms_outlet, blade_height_outlet, rotor_chord, W_inlet, W_rms_outlet, ref Lh, ref Dh,
                                                   ref passage_loss_Moustapha_et_al);

                //################################################################################################################################################    
                //COMBINATION OF THE SKIN FRICTION MODEL PROPOSED BY:
                //Musgrave, D. S., "The Prediction of Design and Off-Design Efficiency for Centrifugal Compressor Impellers",
                //ASME, 22nd Fluids Engineering Conference, 1980.
                //AND
                //SECONDARY FLOW MODEL PROPOSED BY:
                //"Rodgers, C.,"Performance of High-Efficiency Radial/Axial Turbine", Journal of Turbomachinery, ASME, New York, 1987.
                //AND PRESENTED IN:
                //Suhrmann et al., "Validation and Development of Loss Models for Small Size Radial Turbines", GT2010, 2010.

                MUSGRAVE_AND_RODGERS_PASSAGE_LOSS_MODEL(Cf_Turbulent_1, Cf_Turbulent_2, tol1, tol2, k_friction, ref passage_loss_Musgrave_and_Rodgers, ref Dh);

                // IN THIS FUNCTION, THE MAXIMUM VALUE BETWEEN BOTH MODELS IS CHOSEN AS A WORST CASE SCENARIO:
                //if (psi == 1.0 and phi == 0.15):
                // print "PASSAGE_LOSS_MUSGRAVE_AND_RODGERS %f " % passage_loss_Musgrave_and_Rodgers
                // print "PASSAGE_LOSS_MOUSTAPHA_ET_AL %f " % passage_loss_Moustapha_et_al

                if (passage_loss_Musgrave_and_Rodgers < passage_loss_Moustapha_et_al)
                {
                    //passage_loss = passage_loss_Musgrave_and_Rodgers
                    passage_loss = passage_loss_Moustapha_et_al;
                    //Modified in 02/27/2016  in order to change the model to avoid the fluctuation of the efficiency
                }
                else
                {
                    passage_loss = passage_loss_Moustapha_et_al;
                }

                if (passage_loss < 0.0)
                {
                    passage_loss = 0.0;
                }

                enthalpy_loss = incidence_loss + passage_loss + trailing_edge_loss + Exit_Energy_Loss + Tip_Clearance_Loss + Windage_Enthalpy_Loss;

                Incidence_Loss_Contribution = incidence_loss / enthalpy_loss;
                Passage_Loss_Contribution = passage_loss / enthalpy_loss;
                Trailing_Edge_Loss_Contribution = trailing_edge_loss / enthalpy_loss;
                Exit_Energy_Loss_Contribution = Exit_Energy_Loss / enthalpy_loss;
                Tip_Clearance_Loss_Contribution = Tip_Clearance_Loss / enthalpy_loss;
                Windage_Loss_Contribution = Windage_Enthalpy_Loss / enthalpy_loss;

                //11.ENTHALPY LOSSES OUTPUT
                textBox27.Text = "Enthalpy_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(enthalpy_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Incidence_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(incidence_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Incidence_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Incidence_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Passage_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(passage_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Passage_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Passage_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Trailing_Edge_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(trailing_edge_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Trailing_Edge_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Trailing_Edge_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Exit_Energy_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Exit_Energy_Loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Exit_Energy_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Exit_Energy_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Tip_Clearance_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Tip_Clearance_Loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Tip_Clearance_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Tip_Clearance_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Windage_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Windage_Enthalpy_Loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Windage_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Windage_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "Musgrave_and_Rogers_Passage_loss_model: " + Convert.ToString(decimal.Round(Convert.ToDecimal(passage_loss_Musgrave_and_Rodgers), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Moutapha_et_al_passage_loss_model: " + Convert.ToString(decimal.Round(Convert.ToDecimal(passage_loss_Moustapha_et_al), 5, MidpointRounding.AwayFromZero));


                turbine_efficiency = (total_entalphy_difference * 1000) / ((total_entalphy_difference * 1000) + enthalpy_loss);


                //11. FEASIBILITY_CHECK (from line 2693)

                // MATERIAL PROPERTIES FOR INCONEL IN718 - DATA AVAILABLE FROM ONLINE RESOURCES:
                Material_Density = 8220.0;   //[Kg/m^3]
                Yield_Stress = 480.0E06; //[Pa]
                Youngs_Modulus = 127.0E09; //[Pa]
                Poissons_Ratio = 0.284;    //[-]

                //CALCULATION FOR THE BACK FACE
                Kappa = 0.3; // As in  Moustapha et al., "Axial and Radial Turbines", Concepts NREC, 2003.

                //EQUATION FOR ELASTIC STRESS DUE TO CENTRIFUGAL LOADING:
                //Marscher, W. D., "Structural Analysis: Stresses due to Centrifugal, Pressure and Thermal Loads. In Radial Turbines." VKI LS 1992-05, 1992.
                //CALCULATION FOR THE BACK FACE
                elastic_stress_back_face = Kappa * Material_Density * Math.Pow(U_inlet, 2);
                //
                //CALCULATION FOR THE BACK FACE
                Kappa = 0.25;
                elastic_stress_trailing_edge = Kappa * Material_Density * Math.Pow(U_inlet, 2.0);
                //
                //THE MODIFIED FIRST BENDING MODE
                k_ratio = 0.1343;
                //print "feasibility 3.2"
                //k_ratio is the length ratio for the length of plate model, calculated by Jianhui Qi, the reference could be added soon
                Length_A = k_ratio * (radius_inlet - radius_tip_outlet);
                lambda_square_polynomial = 3.3788 + 0.16888 * Length_A / radius_tip_outlet + 3.37717 * (Math.Pow((Length_A / radius_tip_outlet), 2.0));
                //print "lambda_square_polynomial",lambda_square_polynomial
                //calculate the natural frequency
                first_bending_mode = lambda_square_polynomial / (2.0 * Math.PI * Math.Pow(blade_height_outlet, 2)) * Math.Sqrt(Youngs_Modulus * (Math.Pow((2 * rotor_trailing_edge_thickness), 2.0)) / (12.0 * Material_Density * (1.0 - Math.Pow(Poissons_Ratio, 2.0))));

                //print "blade_height_outlet",blade_height_outlet
                //print "first_bending_mode %f " % first_bending_mode
                rotor_frequency = turbine_rotational_speed / (2.0 * Math.PI) * stator_number_vanes;



                //RESULTS_SUMMARY
                textBox28.Text = "#Fluid: " + Convert.ToString(comboBox2.Text) +
                Environment.NewLine + "Number_of_Turbine: " + "1" +
                Environment.NewLine + "Flow_Coefficient: " + Convert.ToString(decimal.Round(Convert.ToDecimal(flow_coefficient), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Head_Coefficient: " + Convert.ToString(decimal.Round(Convert.ToDecimal(head_coefficient), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Turbine_Rotor_Rotational_Speed: " + Convert.ToString(rotational_speed) +
                Environment.NewLine + "REFPROP_Treatment: " + "REAL" +
                Environment.NewLine + "total_enthalpy_difference: " + Convert.ToString(decimal.Round(Convert.ToDecimal(total_entalphy_difference * 1000), 3, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "static_enthalpy_difference: " + Convert.ToString(decimal.Round(Convert.ToDecimal(static_enthalpy_difference), 3, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "T_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_total_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "T_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_static_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "P_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_total_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "P_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_static_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Rho_total_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rho_static_inlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mass_Flow_Rate: " + Convert.ToString(decimal.Round(Convert.ToDecimal(mass_flow_rate), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Power: " + Convert.ToString(decimal.Round(Convert.ToDecimal(mass_flow_rate), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Cp: " + Convert.ToString(decimal.Round(Convert.ToDecimal(cp_inlet), 3, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Viscosity: " + Convert.ToString(decimal.Round(Convert.ToDecimal(viscosity_inlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "ganma: " + Convert.ToString(decimal.Round(Convert.ToDecimal(ganma_inlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Molar_Mass: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Molar_mass), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "U_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "C_intlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(C_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Ctheta_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Ctheta_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Cm_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Cm_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "alpha_inlet (deg): " + Convert.ToString(decimal.Round(Convert.ToDecimal(alpha_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_inlet (deg): " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_inlet (m): " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "blade_height_inlet (m): " + Convert.ToString(decimal.Round(Convert.ToDecimal(blade_height_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "area_inlet (m2): " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_inlet), 5, MidpointRounding.AwayFromZero)) +
                 Environment.NewLine + "area_critical_inlet (m2): " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_critical_inlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_absolute_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_absolute_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_inlet (m/s): " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "T_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_total_outlet), 2, MidpointRounding.AwayFromZero)) +
                //Environment.NewLine + "T_total_relative_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_total_relative_rms_outlet), 2, MidpointRounding.AwayFromZero))
                Environment.NewLine + "T_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(T_static_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "P_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_total_outlet), 2, MidpointRounding.AwayFromZero)) +
                //Environment.NewLine + "P_total_relative_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_total_relative_rms_outlet), 2, MidpointRounding.AwayFromZero))+
                Environment.NewLine + "P_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_static_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Rho_total_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rho_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rho_static_outlet), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Total_static_pressure_ratio: " + Convert.ToString(decimal.Round(Convert.ToDecimal(P_ratio_outlet_static_inlet_total), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "U_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "U_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "U_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(U_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "C_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(C_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Ctheta_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Ctheta_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Cm_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Cm_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "W_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(W_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "beta_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(beta_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_radius_mean_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_radius_mean_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "blade_height_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(blade_height_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "area_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_area_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_area_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_rms_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_rms_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_outlet_length_rms_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_outlet_length_rms_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "area_critical_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(area_critical_outlet), 6, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_absolute_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_absolute_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_rms_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_rms_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_tip_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_tip_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Mach_relative_hub_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Mach_relative_hub_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "number_blades: " + Convert.ToString(decimal.Round(Convert.ToDecimal(number_blades), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_chord: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_chord), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_full_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_full_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_meridional_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_meridional_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_trailing_edge_thickness: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_trailing_edge_thickness), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "axial_clearance: " + Convert.ToString(decimal.Round(Convert.ToDecimal(axial_clearance), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radial_clearance: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radial_clearance), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "back_face_clearance: " + Convert.ToString(decimal.Round(Convert.ToDecimal(back_face_clearance), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "curvature_radius: " + Convert.ToString(decimal.Round(Convert.ToDecimal(curvature_radius), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "radius_ratio_inlet_outlet_tip: " + Convert.ToString(decimal.Round(Convert.ToDecimal(radius_ratio_inlet_outlet_tip), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_tip_inlet_tip_outlet_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_tip_inlet_tip_outlet_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_throat_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_throat_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "rotor_throat_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_throat_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "throat_radius: " + Convert.ToString(decimal.Round(Convert.ToDecimal(throat_radius), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "r_tip_throat: " + Convert.ToString(decimal.Round(Convert.ToDecimal(r_tip_throat), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "r_hub_throat: " + Convert.ToString(decimal.Round(Convert.ToDecimal(r_hub_throat), 5, MidpointRounding.AwayFromZero)) +
                // Environment.NewLine + "rotor_critical_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(rotor_critical_area), 2, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "specific_speed: " + Convert.ToString(decimal.Round(Convert.ToDecimal(specific_speed), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "specific_diameter: " + Convert.ToString(decimal.Round(Convert.ToDecimal(specific_diameter), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "velocity_ratio: " + Convert.ToString(decimal.Round(Convert.ToDecimal(velocity_ratio), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "NsDs: " + Convert.ToString(decimal.Round(Convert.ToDecimal(NsDs), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Reynolds_Number_Rotor: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Reynolds_Number_Rotor), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "eta_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(eta_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "turbine_efficiency: " + Convert.ToString(decimal.Round(Convert.ToDecimal(turbine_efficiency), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "Enthalpy_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(enthalpy_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Incidence_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(incidence_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Incidence_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Incidence_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Passage_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(passage_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Passage_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Passage_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Trailing_Edge_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(trailing_edge_loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Trailing_Edge_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Trailing_Edge_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Exit_Energy_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Exit_Energy_Loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Exit_Energy_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Exit_Energy_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Tip_Clearance_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Tip_Clearance_Loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Tip_Clearance_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Tip_Clearance_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Windage_Loss: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Windage_Enthalpy_Loss), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Windage_Loss_Contribution: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Windage_Loss_Contribution), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "Musgrave_and_Rogers_Passage_loss_model: " + Convert.ToString(decimal.Round(Convert.ToDecimal(passage_loss_Musgrave_and_Rodgers), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Moutapha_et_al_passage_loss_model: " + Convert.ToString(decimal.Round(Convert.ToDecimal(passage_loss_Moustapha_et_al), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine +
                Environment.NewLine + "stator_T_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_total_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_T_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_static_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_total_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_static_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_total_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_total_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_static_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_static_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_C_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_C_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Ctheta_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Ctheta_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Cm_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Cm_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_alpha_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_alpha_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_radius_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_radius_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_blade_height: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_blade_height), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_blade_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_blade_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_chord: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_chord), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_inlet_x_coordinate: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_inlet_x_coordinate), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_inlet_y_coordinate: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_inlet_y_coordinate), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_area_inlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_area_inlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_area_inlet_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_area_inlet_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_T_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_total_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_T_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_T_static_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_total_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_P_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_P_static_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_total_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_total_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_rho_static_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_rho_static_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_C_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_C_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Ctheta_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Ctheta_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_Cm_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_Cm_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_alpha_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_alpha_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_radius_outlet: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_radius_outlet), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_length_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_length_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_outlet_area_total: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_outlet_area_total), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_number_vanes: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_number_vanes), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_solidity: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_solidity), 5, MidpointRounding.AwayFromZero)) +
                //Environment.NewLine + "stator_critical_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_critical_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_trailing_edge_thickness: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_trailing_edge_thickness), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_throat_length: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_throat_length), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_throat_area: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_throat_area), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "stator_throat_radius: " + Convert.ToString(decimal.Round(Convert.ToDecimal(stator_throat_radius), 5, MidpointRounding.AwayFromZero)) +
                Environment.NewLine + "Reynolds_Number_Stator: " + Convert.ToString(decimal.Round(Convert.ToDecimal(Reynolds_Number_Stator), 5, MidpointRounding.AwayFromZero));

                efficiency_total_static = eta_total;

                residual_efficiency = Math.Abs(efficiency_total_static - eta_static);
            }

            textBox12.Text = Convert.ToString(eta_static);
        }

        //OK_Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
        }

        //Cancel Button
        private void button3_Click(object sender, EventArgs e)
        {

        }

        public Double BLADE_OUTLET_CALCULATION(Double rotational_speed1, Double area_outlet1, Double Ctheta_outlet1, 
        Double Cm_outlet1, Double radius_hub_outlet1, Double number_blades1, Double rotor_trailing_edge_thickness_From_File1, 
        Double mass_flow_rate1, Double specific_gas_constant1, Double inlet_total_temperature1, Double P_static_outlet1,
        Double raidius_inlet1)
        {
            Double rotational_omega, b_assume_min, b_assume_max, blade_height_outlet, radius_rms_outlet1;
            Double d, e;
            Double b_calculation, radius_tip_outlet1, U_rms_outlet, beta_rms_outlet;
            Double blade_outlet, blade_outlet_min, blade_outlet_max, blade_outlet_cal;
            Double left, right, result, Func_solution;            

            //radius_hub_outlet                                  =       radius_inlet     * r_ratio
            // rotational speed is changed in the main program
            rotational_omega = rotational_speed;
            // U_hub_outlet                                       =       rotational_omega * radius_hub_outlet
            // Initialize the iteration array:
            // Iteration_value                                     =       []

            // initialize the blade_height_outlet for biosection method
            b_assume_min = 0.0;
            b_assume_max = 0.05;
            blade_height_outlet = 0.0;
            d = 0;

            while (d <100)
            {
                b_calculation = 0.5 * (b_assume_max + b_assume_min);
                //print "b_assume_max",b_assume_max,"b_assume_min", b_assume_min, "b_calculation", b_calculation

                //calculate the beta_rms_outlet
                radius_tip_outlet1 = radius_hub_outlet1 + b_calculation;
                radius_rms_outlet1 = Math.Sqrt((Math.Pow(radius_tip_outlet1,2)+ Math.Pow(radius_hub_outlet1,2)) * 0.5);
                U_rms_outlet = radius_rms_outlet1 * (rotational_omega*2*Math.PI/60);

                //if (U_rms_outlet > 500)
                // U_rms_outlet = 500

                beta_rms_outlet = (Math.Atan((-U_rms_outlet - Ctheta_outlet1) / Cm_outlet))*180/Math.PI;

                //initail value for blade_outlet bi-section methods:
                blade_outlet = 0;
                blade_outlet_min = 0.2225; //12.75 degree. make sure X=0.02*blade_outlet-0.255 is large than zero
                blade_outlet_max = Math.PI/2;

                // print "blade_outlet_max",blade_outlet_max
                e = 0;

                while (e < 10000)
                {
                    blade_outlet_cal = 0.5 * (blade_outlet_min + blade_outlet_max);
                    left = Math.Abs(beta_rms_outlet) / (blade_outlet_cal*180/Math.PI);
                    // rhs = 1.0 + pow((dmdt * (sqrt(specific_gas_constant * inlet_total_temperature)/ \
                    //      (P_static_outlet * pow( 2.0 * stator_radius_inlet,2) * 2.0 * math.tan(beta_blade)))),(0.02 * beta_blade - 0.255)) * \
                    //      (3.0 * Pi/number_blades) + 7.85 * (axial_clearance/blade_height_outlet)
                    // it should be radial_clearance/balde_height_outlet
                    right = 1.0 + Math.Pow((mass_flow_rate * Math.Sqrt(specific_gas_constant1 * inlet_total_temperature1) /
                    (P_static_outlet1 * Math.Pow(2.0 * radius_inlet, 2) * (2.0 * Math.Tan(blade_outlet_cal) - 0.5))), (0.02 * (blade_outlet_cal *180/ Math.PI) - 0.255)) *
                    (3.0 * Math.PI / number_blades) + 7.85 * 0.0416666667; //(0.0416666667 should be the radial_clearance_ratio)

                    A = mass_flow_rate * Math.Sqrt(specific_gas_constant * T_total_inlet);
                    B = (P_static_outlet * Math.Pow(2.0 * radius_inlet, 2) * (2.0 * Math.Tan(blade_outlet_cal) - 0.5));
                    C = (0.02 * (blade_outlet_cal*180/Math.PI) - 0.255);
                    D = (3.0 * Math.PI / number_blades) + 7.85 * 0.0416666667;

// print left,right,blade_outlet_cal,number_blades
// print "NAN",(mass_flow_rate * sqrt(specific_gas_constant * inlet_total_temperature)/ \
//      (P_static_outlet * pow( 2.0 * radius_inlet,2) * (2.0 * math.tan(blade_outlet_cal)-0.5))),(0.02 * blade_outlet_cal - 0.255)

                    result = left - right;

                    if (result > 0)
                    {
                        blade_outlet_min = blade_outlet_cal;
                    }

                    else if (result < 0)
                    {
                        blade_outlet_max = blade_outlet_cal;
                    }

                    else
                    {
                        blade_outlet = blade_outlet_cal;
                        break;
                    }

                    if (Math.Abs(result) < 1E-8)
                    {
                        blade_outlet = blade_outlet_cal;
                        break;
                    }             
                        e = e + 1;
                } 
                        // A safeguard for calculate the balde outlet. That is from the 
                        // Extreme_1 = 1.0E-16
                        // Extreme_2 = 1.0
                        //
                        // DEFINE TOLERANCES FOR BISSECTION METHOD:
                        // tol1 = 1.0E-09
                        // tol2 = 1.0E-09
                        //
                        // beta_blade_outlet = BISSECTION_METHOD(Extreme_1, Extreme_2, tol1, tol2, DEVIATION_CORRELATION_FUNCTION, "DEVIATION_CORRELATION_FUNCTION", Turbine_Dic, Input_Conditions)
                        // beta_blade_outlet = math.degrees(beta_blade_outlet)
                        //if (beta_rms_outlet < 0):    
                        // beta_blade_outlet = -(90.0 - beta_blade_outlet)
                        //
                        // print "beta_blade_outlet %f " % beta_blade_outlet
                        // return beta_blade_outlet
                        // beta_blade_outlet                               =       DEVIATION(Turbine_Dic, Input_Conditions)
                        //if beta_rms_outlet < -85:
                        // beta_rms_outlet = -85
                        // print "blade_outlet", math.degrees(blade_outlet)
                        // print "beta6/b6",beta_rms_outlet/math.degrees(blade_outlet)
                        tt = rotor_trailing_edge_thickness_From_File1;
                    // print tt
                    // substitute the blade_height_outlet into this function and got the solution.
                    Func_solution = area_outlet1 + number_blades * b_calculation * tt / (Math.Cos(blade_outlet)) - Math.PI * (Math.Pow(radius_tip_outlet1, 2)) + Math.PI * (Math.Pow(radius_hub_outlet1, 2));

                    // append the Func_solution value into the interation array
                    // Iteration_value.append(Func_solution)
                    // print "beta_blade_outlet",blade_outlet
                    // print "tt",tt
                    // print "Cm_outlet",Cm_outlet
                    // print "U_rms_outlet",U_rms_outlet
                    // print "radius_rms_outlet",radius_rms_outlet
                    // print "math.pi",math.pi
                    // print "rotational_omega",rotational_omega
                    // print "radius_hub_outlet", radius_hub_outlet
                    // print "number_blades",number_blades
                    // print "area_outlet",area_outlet
                    // print "math.cos(math.radians(beta_rms_outlet))",math.cos(math.radians(beta_rms_outlet))
                    // print "beta_rms_outlet",beta_rms_outlet, "radius_hub_outlet",radius_hub_outlet
                    // print "Func_solution",Func_solution
                    //if n ==0:
                    // b_assume_max                                =       b_calculation
                    //else:
                    //if (abs(Func_solution) > abs(Iteration_value[n-1])):
                    
                    if (Func_solution > 0.0)
                    {
                        b_assume_min = b_calculation;
                    }
                    else if (Func_solution < 0.0)
                    {
                        b_assume_max = b_calculation;
                    }
                    else
                    {
                        blade_height_outlet = b_calculation;
                        break;
                    }

                    if (Math.Abs(Func_solution) < 1.0E-8)
                    {
                        blade_height_outlet = b_calculation;
                        // print "blade_height_outlet",blade_height_outlet
                        break;
                    }

                      d = d + 1;             

                    }  

                return blade_height_outlet;
            }

        public void ROTOR_THROAT_CALCULATION(Double critical_area, Double Pi, Double number_blades, Double blade_height_inlet, Double radius_inlet,
                      Double radius_outlet, Double radius_hub_outlet, Double angle_outlet, Double blade_height, Double mass_flow_rate, 
                      Double rho_static_outlet, Double gama, Double specific_gas_constant, Double T_static_outlet, Double outlet_length, 
                      Double outlet_area, Double trailing_edge_thickness, Double T_total_inlet, Double rho_total_inlet, Double T_total_outlet, 
                      Double rho_total_outlet, ref Double ksi_rotor_throat1, ref Double omega_rotor_throat1, ref Double rotor_throat_length1,
                      ref Double rotor_throat_area1, ref Double distance_A_C_rotor1, ref Double preliminary_feasibility_check1, ref Double r_tip_throat1, 
                      ref Double r_hub_throat1)
        {
            Double r_hub_throat, r_tip_throat;
            // FOR THE ROTOR ANGLE_OUTLET REFERS TO THE ANGLE BETA AT THE ROTOR OUTLET.
            Double A_blade_x = 0.0;
            Double A_blade_y = 0.0;
            Double B_blade_x = 0.0;
            Double B_blade_y = 0.0;
            Double m_throat;
            Double b_throat;
            Double m_h;
            Double b_h = 0.0;
            Double throat_length_shroud;
            Double C_x, C_y;
            Double ksi_throat = 0.0; //THIS PARAMETER IS NOT USED FOR THE ROTOR.
            Double omega_throat = 0.0; //THIS PARAMETER IS NOT USED FOR THE ROTOR.
            Double distance_A_C;
            Double B_blade_hub_x, B_blade_hub_y;
            Double throat_length_hub, throat_length, throat_area;
            Double throat_delta_z_coordinate, throat_z_coordinate;

            B_blade_y = 2.0 * Pi * radius_outlet / number_blades;
            m_throat = -1.0 / Math.Tan(angle_outlet * Pi / 180);
            b_throat = B_blade_y - (m_throat * B_blade_x);
            m_h = Math.Tan(angle_outlet * Pi / 180);
           
            //COORDINATES OF POINT C:
            C_x = (b_h - b_throat) / (m_throat - m_h);
            C_y = m_h * C_x + b_h;        

            //DISTANCE BETWEEN POINT B AND C: THROAT LENGTH AT SHROUD:

            throat_length_shroud = Math.Sqrt(Math.Pow((C_x - B_blade_x), 2) + Math.Pow((C_y - B_blade_y), 2));

            //DISTANCE BETWEEN POINT A AND C: AT SHROUD.
            distance_A_C = Math.Sqrt(Math.Pow((C_x - A_blade_x), 2) + Math.Pow((C_y - A_blade_y), 2));
            distance_A_C_rotor1 = distance_A_C;

            B_blade_hub_x = 0.0;
            B_blade_hub_y = 2.0 * Pi * radius_hub_outlet / number_blades;
            m_throat = -1.0 / Math.Tan(angle_outlet * Pi / 180);
            b_throat = B_blade_hub_y - (m_throat * B_blade_hub_x);
            m_h = Math.Tan(angle_outlet * Pi / 180);
            b_h = 0.0;

            //COORDINATES OF POINT C:
            C_x = (b_h - b_throat) / (m_throat - m_h);
            C_y = m_h * C_x + b_h;

            //DISTANCE BETWEEN POINT B AND C: THROAT LENGTH AT HUB:
            throat_length_hub = Math.Sqrt(Math.Pow((C_x - B_blade_hub_x), 2) + Math.Pow((C_y - B_blade_hub_y), 2));

            //THE AREA OF THE THROAT IS CALCULATED BY APPROXIMATING IT TO A TRAPEZOID FOR WHICH HEIGHT, H IS THE BLADE HEIGHT AT THE OUTLET, AND THE SHROUD AND HUB CURVED PROFILES ARE DISREGARDED.
            //THE THROAT LENGTH IS CONSIDERED TO BE THE AVERAGE BETWEEN THE THROAT LENGTH FOR THE HUB AND THE SHROUD.

            throat_length = (throat_length_hub + throat_length_shroud) / 2.0;
            throat_area = blade_height * ((throat_length_hub + throat_length_shroud) / 2.0 - trailing_edge_thickness);            
            //critical_area                           = (mass_flow_rate/number_blades)/ \
            //                                          (rho_static_outlet*sqrt(gama * specific_gas_constant * T_static_outlet))
            //FORCING ROTOR THROAT AREA = AREA_OUTLET * COS(ALPHA) AND CALCULATING ITS LENGTH, AREA, HUB AND TIP RADII:
            throat_area = outlet_area * Math.Cos(angle_outlet * Pi / 180);

            //SET REASONABLE LIMITS FOR THE THROAT AREA, WHEN COMPARED TO THE OUTLET AREA:
            if (throat_area > 0.85 * outlet_area)
            {
                throat_area = 0.85 * outlet_area;
            }

            if (throat_area < 0.25 * outlet_area)
            {
                throat_area = 0.25 * outlet_area;
            }

            throat_length = outlet_length * Math.Cos(angle_outlet * Pi / 180) - trailing_edge_thickness;

            rotor_throat_length1 = throat_length;

            rotor_throat_area1 = throat_area;

            //CALCULATING THE THROAT LOCATION AT THE LEADING EDGE: throat_z_coordinate - throat_delta_z_coordinate
            //(WHICH IS APPROXIMATED BY: outlet_length * sin(beta_rms_outlet)):
            throat_delta_z_coordinate = Math.Abs(outlet_length * Math.Sin(angle_outlet * Pi / 180));
            throat_z_coordinate = (blade_height_inlet + radius_inlet - radius_outlet) - throat_delta_z_coordinate;

            //INTERCEPTION BETWEEN CIRCLE AND z = throat_z_coordinate (CONSIDERING CENTRE OF SHROUD CIRCLE AS FOR THE NASA GEOMETRY):

            //CHECK IF throat_delta_z_coordinate > 0.5 * radius_inlet - radius_outlet
            if (throat_delta_z_coordinate > 0.5 * radius_inlet - radius_outlet)
            {
                //CHECK FOR DOMAIN PROBLEMS:
                if ((2.0 * radius_inlet - blade_height_inlet - 2.0 * radius_outlet - throat_z_coordinate) * (blade_height_inlet + throat_z_coordinate) > 0.0)
                {
                           r_tip_throat = Math.Sqrt((2.0 * radius_inlet - blade_height_inlet - 2.0 * radius_outlet - throat_z_coordinate) *
                          (blade_height_inlet + throat_z_coordinate)) + radius_inlet;
                    //INTERCEPTION BETWEEN ELIPSE AND z = throat_z_coordinate (CONSIDERING CENTRE OF HUB ELIPSE IN THE CENTRE OF SHROUD CIRCLE):
                    if ((radius_inlet + blade_height_inlet - radius_outlet != 0.0) && (radius_inlet - radius_hub_outlet != 0.0))
                    {
                                r_hub_throat = -(Math.Sqrt(2.0 * radius_inlet - 2.0 * radius_outlet - throat_z_coordinate) *
                                      Math.Abs(radius_inlet - radius_hub_outlet) *
                                      Math.Sqrt(2.0 * blade_height_inlet + throat_z_coordinate)
                                      - radius_inlet * (radius_inlet + blade_height_inlet - radius_outlet)) /
                                     (radius_inlet + blade_height_inlet - radius_outlet);

                        if (r_hub_throat < 0.0)
                        {
                            r_hub_throat = (Math.Sqrt(2.0 * radius_inlet - 2.0 * radius_outlet - throat_z_coordinate) *
                                          Math.Abs(radius_inlet - radius_hub_outlet) *
                                          Math.Sqrt(2.0 * blade_height_inlet + throat_z_coordinate)
                                          + radius_inlet * (radius_inlet + blade_height_inlet - radius_outlet)) /
                                          (radius_inlet + blade_height_inlet - radius_outlet);
                        }
                        if (r_hub_throat < 0.0)
                        {
                            r_hub_throat = radius_hub_outlet;
                            r_tip_throat = radius_outlet;
                        }
                    }

                    else
                    {
                        r_tip_throat = radius_outlet;
                        r_hub_throat = radius_hub_outlet;
                    }
                }

                else
                {
                    r_tip_throat = radius_outlet;
                    r_hub_throat = radius_hub_outlet;
                }
            }

            else
            {
                r_tip_throat = radius_outlet;
                r_hub_throat = radius_hub_outlet;
            }

            r_tip_throat1 = r_tip_throat;
            r_hub_throat1 = r_hub_throat;
        }

        public void STATOR_THROAT_CALCULATION(Double stator_critical_area, Double Pi, Double number_blades, Double blade_height_inlet, Double radius_inlet,
                      Double radius_outlet, Double radius_hub_outlet, Double angle_outlet, Double blade_height, Double mass_flow_rate,
                      Double stator_rho_static_outlet, Double gama, Double specific_gas_constant, Double stator_T_static_outlet, Double stator_outlet_length,
                      Double stator_outlet_area, Double trailing_edge_thickness, Double stator_T_total_inlet, Double stator_rho_total_inlet, Double stator_T_total_outlet,
                      Double stator_rho_total_outlet, ref Double ksi_throat, ref Double omega_throat, ref Double throat_length,
                      ref Double throat_area, ref Double distance_A_C, ref Double preliminary_feasibility_check1, ref Double r_tip_throat,
                      ref Double r_hub_throat)
        {

            Double phi_blade_number, B_blade_x, B_blade_y, alpha_line;
            Double theta_throat_stator, b_c, C_x, C_y;

            phi_blade_number = 360.0 /number_blades;  //ANGLE OF EACH PASSAGE
            B_blade_x = radius_outlet * Math.Sin(phi_blade_number*Math.PI/180);
            B_blade_y = radius_outlet * Math.Cos(phi_blade_number*Math.PI/180);
            ksi_throat = 90.0 - phi_blade_number;      //ANGLE BETWEEN HORIZONTAL AND THE LINE BETWEEN THE CENTRE OF THE WHEEL AND THE SECOND VANE OUTFLOW POINT
            alpha_line = 90.0 - angle_outlet;

            theta_throat_stator = (90.00 - angle_outlet);
            b_c = B_blade_y + B_blade_x / Math.Tan((theta_throat_stator - phi_blade_number)*Math.PI/180);
            C_x = (b_c - radius_outlet) / (Math.Tan(theta_throat_stator * Math.PI / 180) + 1.0 / (Math.Tan((theta_throat_stator - phi_blade_number) * Math.PI / 180)));
            C_y = (Math.Tan(theta_throat_stator*Math.PI/180) * C_x + radius_outlet);

            if (phi_blade_number == theta_throat_stator)
            {
                C_x = B_blade_x;
                C_y = (Math.Tan(theta_throat_stator * Math.PI / 180) * C_x + radius_outlet);
            }

            omega_throat = 0.0;
            throat_length = Math.Sqrt(Math.Pow((C_x - B_blade_x), 2) + Math.Pow((C_y - B_blade_y), 2)) - trailing_edge_thickness;
            //THROAT RADIUS:
            //THESE VARIABLES ARE THE SAME FOR THE STATOR. THEY REPRESENT THE RADIUS OF THE THROAT WHICH IS THE SAME FOR THE HUB AND SHROUD (TIP) DUE TO THE QUASI-2D GEOMETRY OF THIS STATION:
            r_tip_throat = Math.Sqrt(Math.Pow(C_x, 2) + Math.Pow(C_y, 2));
            r_hub_throat = r_tip_throat;
            //THROAT AREA
            throat_area = throat_length * blade_height;
            //DISTANCE BETWEEN POINT A AND C   
            distance_A_C = phi_blade_number / 360.0 * 2.0 * Pi * radius_outlet;
            //
            //IF THE THROAT RADIUS OF THE STATOR IS BIGGER THAT THE STATOR INLET RADIUS...THAT MEANS THAT THERE ARE VERY FEW VANES.
            //THEREFORE THE THROAT IS THE OUTLET SECTION OF THE STATOR:
            if (r_tip_throat > radius_inlet)
            {
                r_tip_throat = radius_outlet;
                throat_length = 2.0 * Pi * radius_outlet / number_blades;
                throat_area = throat_length * blade_height;
            }
         }

        public void MUSGRAVE_AND_RODGERS_PASSAGE_LOSS_MODEL(Double Cf_Turbulent_1, Double Cf_Turbulent_2, Double tol1, Double tol2,
                                                            Double k_friction, ref Double passage_loss_Musgrave_and_Rodgers, ref Double Dh)
        {
            //SKIN FRICTION MODEL:
            Lh = rotor_meridional_length;
            Dh = (1.0 / 2.0) * ((4.0 * Math.PI * radius_inlet * blade_height_inlet / (2.0 * Math.PI * radius_inlet + number_blades * blade_height_inlet)) + (2.0 * Math.PI * (Math.Pow(radius_tip_outlet, 2) - Math.Pow(radius_hub_outlet, 2))) / (Math.PI * (radius_tip_outlet - radius_hub_outlet) + number_blades * (radius_tip_outlet - radius_hub_outlet)));
            //print "Reynolds_Number_Rotor_MUSGRAVE_AND_RODGERS %f " % Reynolds_Number_Rotor

            if (Reynolds_Number_Rotor < 2100.0)
            {
                //LAMINAR REGIME (FANNING FRICTION FACTOR)
                Cf = 16.0 / Reynolds_Number_Rotor;
            }

            else if (Reynolds_Number_Rotor >= 2100.0 && Reynolds_Number_Rotor <= 4000.0)
            {
                //TRANSITION REGIME: WEIGHTING FUNCTION BETWEEN LAMINAR AND TURBULENT REGIME.
                //TURBULENT REGIME: Colebrook-White equation estimates the (dimensionless) Darcy-Weisbach
                //friction factor f for fluid flows in filled pipes. For transition regime of flow,
                //in which the friction factor varies with both R and e/D, the equation universally adopted
                //is due to Colebrook and White (1937.

                //Cf IN TURBULENT REGIME IS CALCULATED MAKING USE OF THE BISSECTION METHOD FOR Colebrook-White EQUATION:
                //FANNING FRICTION FACTOR:
                Cf_Laminar = 16.0 / Reynolds_Number_Rotor;

                //LIMITS OF THE SOLUTION:
                Cf_Turbulent = BISSECTION_METHOD(Cf_Turbulent_1, Cf_Turbulent_2, tol1, tol2, k_friction, Dh);

                //WEIGHTING FUNCTION:
                laminar_turbulent_percentage = (Reynolds_Number_Rotor - 2100.0) / (4000.0 - 2100.0);
                Cf = (1.0 - laminar_turbulent_percentage) * Cf_Laminar + laminar_turbulent_percentage * Cf_Turbulent;
            }

            else
            {
                //FULL TURBULENT REGIME
                Cf = BISSECTION_METHOD(Cf_Turbulent_1, Cf_Turbulent_2, tol1, tol2, k_friction, Dh);
            }

    //CALCULATING THE SKIN FRICTION COEFFICIENT FOR CURVED PIPES AS PER:
    //Schlichting, H. and Gersten, K., "Boundary Layer Theory", 8th Edition, Springer-Verlag Berlin Heidelberg, 2000.
    
    Cfc = Cf * (1.0 + 0.075 * Math.Pow(Reynolds_Number_Rotor, (1.0 / 4.0)) * Math.Sqrt(Dh / (2.0 * curvature_radius)));
    
    //CALCULATING THE MODIFICATION FOR THE FRICTION FACTOR (CONSIDERING ADDITIONAL TURBOMACHINERY CURVATURE EFFECTS) PRESENTED BY:
    //Musgrave, D. S., "The Prediction of Design and Off-Design Efficiency for Centrifugal Compressor Impellers",
    //ASME, 22nd Fluids Engineering Conference, 1980
    
    Cfc_turbo = Cfc * Math.Pow(Reynolds_Number_Rotor * Math.Pow((2.0 * radius_inlet / (2.0 * curvature_radius)), 2.0), 0.05);
    
    //CALCULATING THE MEAN RELATIVE VELOCITY THROUGH THE ROTOR PASSAGE AS IN:
    //Coppage et al., "Study of Supersonic Radial Compressor for Refrigeration and Pressurization Systems", WADC Report 55-257, 1957.
    
    mean_relative_velocity = (W_inlet + (W_tip_outlet + W_hub_outlet) / 2.0) / 2.0;
    
    //THE ENTHALPY DROP DUE TO FRICTION CAN BE CALCULATED THROUGH THE FOLLOWING EQUATION:
    //print "curvature_radius_MUSGRAVE_AND_RODGERS %f " % curvature_radius
    //print "Cfc_turbo_MUSGRAVE_AND_RODGERS %f " % Cfc_turbo
    //print "mean_relative_velocity_turbo_MUSGRAVE_AND_RODGERS %f " % mean_relative_velocity
    friction_loss = Cfc_turbo * Lh / Dh * Math.Pow(mean_relative_velocity, 2.0);
    
    //CALCULATING THE LOSSES DUE TO SECONDARY FLOW BY:
    //"Rodgers, C.,"Performance of High-Efficiency Radial/Axial Turbine", Journal of Turbomachinery, ASME, New York, 1987.
    
    secondary_flow_loss = Math.Pow(C_inlet, 2.0) * (2.0 * radius_inlet) / (number_blades * curvature_radius);
    
    passage_loss = friction_loss + secondary_flow_loss;

    passage_loss_Musgrave_and_Rodgers = passage_loss;

        }

        public Double BISSECTION_METHOD(Double xa_bissection,Double xb_bissection,Double tol1,Double tol2, Double k_friction, Double Dh)
        {
            int i=0;
            Double x_zero=0;

            if (COLEBROOK_WHITE_FUNCTION_FANNING(xa_bissection, k_friction, Dh) * COLEBROOK_WHITE_FUNCTION_FANNING(xb_bissection, k_friction, Dh) <= 0.0)
            {
                //IF WE HAVE A SOLUTION: PROCEED IMPROVING THE GUESS.
                while (i < 100)
                {
                    x_zero = 0.5 * (xa_bissection + xb_bissection);

                    if (COLEBROOK_WHITE_FUNCTION_FANNING(x_zero, k_friction, Dh) * COLEBROOK_WHITE_FUNCTION_FANNING(xa_bissection, k_friction, Dh) < 0.0)
                    {
                        xb_bissection = x_zero;
                    }

                    else
                    {
                        xa_bissection = x_zero;
                    }

                    if ((Math.Abs(xb_bissection - xa_bissection) < tol1) || (Math.Abs(COLEBROOK_WHITE_FUNCTION_FANNING(x_zero, k_friction, Dh)) < tol2))
                    { 
                        break;
                    }

                    i = i + 1;
                }

                return x_zero;
            }

            else
            {
                x_zero = 0.0;
                return x_zero;                
            }

            return x_zero;
        }

        public Double COLEBROOK_WHITE_FUNCTION_FANNING(Double Cf_Colebrook, Double k_friction, Double Dh)
        {
            Double lhs, rhs, result;

            lhs = 1.0 / Math.Sqrt(Cf_Colebrook);
            rhs = -4.0 * Math.Log10((k_friction / Dh) / 3.7 + (1.256 / (Reynolds_Number_Rotor * Math.Sqrt(Cf_Colebrook))));

            result = lhs - rhs;

            return result;
        }

        public void MOUSTAPHA_ET_AL_PASSAGE_LOSS_MODEL (Double Kp1, Double radius_hub_outlet1, Double radius_tip_outlet1, Double rotor_meridional_length1,
                                                        Double radius_inlet1, Double blade_height_inlet1, Double number_blades1, Double beta_rms_outlet1,
                                                        Double blade_height_outlet1, Double rotor_chord1, Double W_inlet1, Double W_rms_outlet1,
                                                        ref Double Lh1, ref Double Dh1, ref Double passage_loss_Moustapha_et_al1)
        {
            Double average_radius_outlet1;

            average_radius_outlet1 = (1.0 / 2.0) * (radius_hub_outlet1 + radius_tip_outlet1);

            // Lh REFERS TO THE MEAN PASSAGE HYDRAULIC LENGTH
            // Lh = Pi/4.0 * ((axial_length - blade_height_inlet/2.0) + (radius_inlet - r_tip_throat-(blade_height_throat)/2.0))
            //
            Lh1 = rotor_meridional_length1;
            //
            //Dh REFERS TO THE MEAN PASSAGE HYDRAULIC DIAMETER
            //
            //Dh = (1.0/2.0) * ((4.0 * Pi * radius_inlet * blade_height_inlet/(2.0 * Pi * radius_inlet + number_blades * blade_height_inlet)) + \
            //     (2.0 * Pi * (pow(r_tip_throat,2) - pow(r_hub_throat,2)))/(Pi * blade_height_throat + number_blades * blade_height_throat))
            //
            Dh1 = (1.0 / 2.0) * ((4.0 * Math.PI * radius_inlet1 * blade_height_inlet1 / (2.0 * Math.PI * radius_inlet1 + number_blades1 * blade_height_inlet1)) + (2.0 * Math.PI * (Math.Pow(radius_tip_outlet1, 2) - Math.Pow(radius_hub_outlet1, 2))) / (Math.PI * (radius_tip_outlet1 - radius_hub_outlet1) + number_blades1 * (radius_tip_outlet1 - radius_hub_outlet1)));
            //
            //print "DHHHHHHHHHHHHHHHHHJKSDKSDKLJASDJAKSDCKLASDASDSA %f" % Dh  
            //passage_loss = Kp * ((Lh/Dh) + 0.68 * (1.0 - pow((average_radius_throat/radius_inlet),2.0)) * \
            //               math.cos(math.radians(beta_rms_outlet))/(blade_height_throat/chord)) * 1.0/2.0 * (pow(W_inlet,2.0) + pow(W_rms_throat,2.0))
            //
            passage_loss_Moustapha_et_al1 = Kp1 * ((Lh1 / Dh1) + 0.68 * (1.0 - Math.Pow((average_radius_outlet1 / radius_inlet1), 2.0)) * Math.Cos((beta_rms_outlet1*Math.PI/180)) / (blade_height_outlet1 / rotor_chord1)) * 1.0 / 2.0 * (Math.Pow(W_inlet1, 2.0) + Math.Pow(W_rms_outlet1, 2.0));
            //
            if ((radius_inlet1 - radius_tip_outlet1) / blade_height_outlet1 < 0.2)
            {
                passage_loss_Moustapha_et_al1 = passage_loss_Moustapha_et_al1 * 2.0;
            }
        }
    }
}
