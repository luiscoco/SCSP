using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using sc.net;

using NumericalMethods;
using NumericalMethods.FourthBlog;

namespace RefPropWindowsForms
{
    public partial class Receiver_Forristall : Form
    {
        public MainWindow puntero;

        public RefrigerantCategory category;
        public ReferenceState referencestate;
        public Refrigerant refrigerante1;
        public Refrigerant refrigerante2;

        //R.Forristal One dimension problem variables
        public double g = 9.81;
        public double[] x = new double[4];

        Parameter T2 = new Parameter(500.0);
        Parameter T3 = new Parameter(600.0);
        Parameter T4 = new Parameter(700.0);
        Parameter T5 = new Parameter(800.0);

        public int[] ICON1 = new int[4];
        public double[] Y = new double[4];
        public int INFO;
        public int NITER;
        public int n;
        public int MAXIT;
        public double TOL;
        public int IDER;

        public double[,] Tabella = new double[200, 200];
        public double[] p_media = new double[500];
        public double[] T_media = new double[500];
        public double[] P_out = new double[500];
        public double[] T_out = new double[500];
        public double P_ingresso;
        public double T_ingresso;
        public double[] P_out_loop = new double[500];
        public double[] T_out_loop = new double[500];
        public double[] eta_th = new double[500];
        public double[] v_media_cumulata = new double[500];

        public double v_media_ricevitore;
        public double p_corrente;
        public double T1_corrente;
        public double m_corrente;
        public double v_corrente;
        public double T_in_corrente;
        public double P_in_corrente;
        public double d3_corrente;
        public double d2_corrente;
        public double d4_corrente;
        public double d5_corrente;
        public double Tamb_corrente;
        public double DNI_corrente;
        public double apertura_corrente;
        public double eta_ott_corrente;
        public double S_tot_corrente;
        public double n_loop_corrente;
        public double eps_glass_corrente;
        public double alfa_env_corrente;
        public double alfa_asb_corrente;
        public double tau_env_corrente;
        public double N_tratti_corrente;
        public double L_tot_corrente;
        public double L_tratto_corrente;
        public double N_sez_corrente;
        public string fluido_corrente;

        public Receiver_Forristall(MainWindow puntero1)
        {
            puntero = puntero1;

            InitializeComponent();
        }

        public void Parametri_input_primo_loop()
        {

            T_in_corrente = Convert.ToDouble(textBox81.Text);
            P_in_corrente = Convert.ToDouble(textBox82.Text);
            d3_corrente = Convert.ToDouble(textBox83.Text);
            d2_corrente = Convert.ToDouble(textBox84.Text);
            d4_corrente = Convert.ToDouble(textBox85.Text);
            d5_corrente = Convert.ToDouble(textBox86.Text);
            Tamb_corrente = Convert.ToDouble(textBox87.Text);
            v_corrente = Convert.ToDouble(textBox88.Text);
            DNI_corrente = Convert.ToDouble(textBox93.Text);
            apertura_corrente = Convert.ToDouble(textBox92.Text);
            eta_ott_corrente = Convert.ToDouble(textBox91.Text);
            S_tot_corrente = Convert.ToDouble(textBox90.Text);
            n_loop_corrente = Convert.ToDouble(textBox89.Text);
            eps_glass_corrente = Convert.ToDouble(textBox100.Text);
            alfa_env_corrente = Convert.ToDouble(textBox99.Text);
            alfa_asb_corrente = Convert.ToDouble(textBox98.Text);
            tau_env_corrente = Convert.ToDouble(textBox97.Text);
            N_tratti_corrente = Convert.ToDouble(textBox96.Text);
            N_sez_corrente = Convert.ToDouble(textBox95.Text);
            //fluido_corrente = Convert.ToDouble(textBox94.Text);

            if (comboBox9.Text == "DEF")
            {
                referencestate = ReferenceState.DEF;
            }
            if (comboBox9.Text == "ASH")
            {
                referencestate = ReferenceState.ASH;
            }
            if (comboBox9.Text == "IIR")
            {
                referencestate = ReferenceState.IIR;
            }
            if (comboBox9.Text == "NBP")
            {
                referencestate = ReferenceState.NBP;
            }

            //PureFluid
            if (comboBox6.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                //fluido_corrente
                refrigerante1 = new Refrigerant(category, this.comboBox5.Text, referencestate);
            }

            //NewMixture
            if (comboBox6.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                //fluido_corrente
                refrigerante1 = new Refrigerant(category, this.comboBox5.Text + "=" + textBox94.Text + "," + this.comboBox4.Text + "=" + textBox101.Text + "," + this.comboBox8.Text + "=" + textBox102.Text + "," + this.comboBox7.Text + "=" + textBox103.Text, referencestate);
            }

            if (comboBox6.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
                //fluido_corrente
                //refrigerante1 = new Refrigerant(category, this.comboBox2.Text, referencestate);
            }

            if (comboBox6.Text == "PseudoPureFluid")
            {
                category = RefrigerantCategory.PseudoPureFluid;
            }

            refrigerante1.Category = category;
            refrigerante1.reference = referencestate;
        }

        public void Parametri_input_longitudinale()
        { 
            m_corrente = Convert.ToDouble(textBox106.Text);
            L_tot_corrente = Convert.ToDouble(textBox104.Text);
            L_tratto_corrente = Convert.ToDouble(textBox105.Text);
        }

        //Sub Longitudinale_vba()
        private void Button17_Click(object sender, EventArgs e)
        {
        
            for (int i = 0; i <T_out.Length; i++)
            {
                T_out[i] = 0.0;
            }

            for (int i = 0; i<T_media.Length; i++)
            {
                T_media[i] = 0.0;
            }

            Parametri_input_primo_loop();

            Parametri_input_longitudinale();

            double m = m_corrente;
            T_ingresso = T_in_corrente;
            P_ingresso = P_in_corrente;
            double L_tot = L_tot_corrente;
            double L_tratto = L_tratto_corrente;
            double N_tratti = L_tot / L_tratto;

            double d2 = d2_corrente;

            double Delta_p;
            double Delta_p_ball_joints;
            double Delta_p_distr;

            int k = 1;

            //fluido = fluido_corrente;

            for (int i = 1; i <= Convert.ToInt64(N_tratti); i++)
            {
                T1_corrente = T_ingresso;
                p_corrente = P_ingresso;

                double T_ingresso_tratto = T_ingresso;
                double P_ingresso_tratto = P_ingresso;

                Tabella[i - 1, 0] = i;
                Tabella[i - 1, 5] = T_ingresso_tratto;
                Tabella[i - 1, 11] = P_ingresso_tratto;

                //T_ingresso_tratto, P_ingresso_tratto Conditions
                refrigerante1.FindStateWithTP(T_ingresso_tratto, P_ingresso_tratto);
                //Fluid Density inlet
                double ro_in = refrigerante1.Density;
                //Entlaphy Density inlet
                double h_in = refrigerante1.Enthalpy;

                double v_in = 4 * m / (ro_in * Math.PI * Math.Pow(d2, 2));

                T_media[0] = 0;
                T_media[1] = T1_corrente;
                
                p_media[0] = 0;
                p_media[1] = p_corrente;

                k = 1;

                do
                {
                    //1D Heat Looses calculation
                    Monodimensionale_vba();

                    //Temperatura T1
                    Tabella[i - 1, 1] = T2.Value;
                    //Temperatura T2
                    Tabella[i - 1, 2] = T3.Value;
                    //Temperatura T3
                    Tabella[i - 1, 3] = T4.Value;
                    //Temperatura T4
                    Tabella[i - 1, 4] = T5.Value;
                    //q12conv(T1):
                    Tabella[i - 1, 14] = q12conv(T2.Value);
                    //q23cond(T1,T2):
                    Tabella[i - 1, 15] = q23cond(T2.Value, T3.Value);
                    //q34cond(T2,T3):
                    Tabella[i - 1, 16] = q34conv(T3.Value, T4.Value);
                    //q34rad(T1,T2,T3):
                    Tabella[i - 1, 17] = q34rad(T2.Value, T3.Value, T4.Value);
                    //q45cond(T3,T4):
                    Tabella[i - 1, 18] = q45cond(T4.Value, T5.Value);
                    //q56conv(T4):
                    Tabella[i - 1, 19] = q56conv(T5.Value);
                    //q57rad(T4):
                    Tabella[i - 1, 20] = q57rad(T5.Value);
                    //
                    Tabella[i - 1, 21] = qcond_brac(T3.Value);
                    //
                    Tabella[i - 1, 22] = q34conv(T3.Value, T4.Value) + q34rad(T2.Value, T3.Value, T4.Value) + qcond_brac(T3.Value);
                    //
                    double q_in = q12conv(T2.Value);

                    //Total Heat Losses calculation
                    double Perdite = q34conv(T3.Value, T4.Value) + q34rad(T2.Value, T3.Value, T4.Value) + qcond_brac(T3.Value);


                    Tabella[i - 1, 8] = Perdite;

                    //Thermal Efficiency calculation
                    double eta_termico = (q3SolAbs() - Perdite) / q3SolAbs();

                    Tabella[i - 1, 9] = eta_termico;

                    //T_media(k), p_media(k) Conditions
                    //refrigerante1.FindStateWithTP(T_media[k], p_media[k]);
                    refrigerante1 = null;
                    refrigerante1 = new Refrigerant(RefrigerantCategory.PureFluid, "CO2", ReferenceState.DEF);
                    if (comboBox9.Text == "DEF")
                    {
                        referencestate = ReferenceState.DEF;
                    }
                    if (comboBox9.Text == "ASH")
                    {
                        referencestate = ReferenceState.ASH;
                    }
                    if (comboBox9.Text == "IIR")
                    {
                        referencestate = ReferenceState.IIR;
                    }
                    if (comboBox9.Text == "NBP")
                    {
                        referencestate = ReferenceState.NBP;
                    }

                    //PureFluid
                    if (comboBox6.Text == "PureFluid")
                    {
                        category = RefrigerantCategory.PureFluid;
                        //fluido_corrente
                        refrigerante1 = new Refrigerant(category, this.comboBox5.Text, referencestate);
                    }

                    //NewMixture
                    if (comboBox6.Text == "NewMixture")
                    {
                        category = RefrigerantCategory.NewMixture;
                        //fluido_corrente
                        refrigerante1 = new Refrigerant(category, this.comboBox5.Text + "=" + textBox94.Text + "," + this.comboBox4.Text + "=" + textBox101.Text + "," + this.comboBox8.Text + "=" + textBox102.Text + "," + this.comboBox7.Text + "=" + textBox103.Text, referencestate);
                    }

                    if (comboBox6.Text == "PredefinedMixture")
                    {
                        category = RefrigerantCategory.PredefinedMixture;
                        //fluido_corrente
                        //refrigerante1 = new Refrigerant(category, this.comboBox2.Text, referencestate);
                    }

                    if (comboBox6.Text == "PseudoPureFluid")
                    {
                        category = RefrigerantCategory.PseudoPureFluid;
                    }
                    refrigerante1.Category = category;
                    refrigerante1.reference = referencestate;


                    refrigerante1.FindStateWithTP(T_media[k], p_media[k]);
                    //Fluid Density Average
                    double ro_ave = refrigerante1.Density;
                    //Fliud Viscosity
                    double mu_ave = refrigerante1.Viscosity;

                    //Average Fluid velocity
                    double v_ave = 4 * m / (ro_ave * Math.PI * Math.Pow(d2, 2));
                    //Fluid velocity outlet
                    double v_out = 2 * v_ave - v_in;
                    //Fluid Density Outlet
                    double ro_out = 4 * m / (v_out * Math.PI * Math.Pow(d2, 2));

                    Tabella[i - 1, 10] = v_ave;

                    //Número de Reynold'
                    double Red2_ave = ro_ave * v_ave * d2 / (mu_ave * 10e-7);

                    //Darcy Coefficient: Pressure Drop
                    double f = Math.Pow((1 / (-2 * Math.Log(5.8 / (Math.Pow(Red2_ave, 0.9)) + 0.0000015 / (3.71 * d2)) / Math.Log(10))), 2);

                    //Cálculo de la caida de presión en cada Tramo'
                    //Si hay Un Solo Tramo'
                    if (i == 1)
                    {
                        Delta_p_ball_joints = 3 * 8.69 * ro_in * (Math.Pow(v_in, 2)) / 2;
                        Delta_p_distr = (f * L_tratto * Math.Pow((4 * m / (Math.PI * Math.Pow(d2, 2))), 2)) / (2 * d2 * ro_ave);
                        Delta_p = Delta_p_ball_joints + Delta_p_distr;

                    }
                    else
                    {
                        //Si hay N Tramos'
                        if (i == N_tratti)
                        {
                            Delta_p_ball_joints = 3 * 8.69 * ro_out * Math.Pow(v_out,2) / 2;
                            Delta_p_distr = (f * L_tratto * Math.Pow((4 * m / (Math.PI * Math.Pow(d2, 2))), 2)) / (2 * d2 * ro_ave);
                            Delta_p = Delta_p_ball_joints + Delta_p_distr;
                        }
                        else
                        {
                            Delta_p = (f * L_tratto * Math.Pow((4 * m / (Math.PI * Math.Pow(d2, 2))), 2)) / (2 * d2 * ro_ave);
                        }
                    }
                    
                    P_out[k - 1] = P_ingresso_tratto - (Delta_p / 10e2);
                    double h_out = h_in + q_in * L_tratto / (m * 1000) + (Math.Pow(v_in, 2) - Math.Pow(v_out, 2)) / 2000;

                    //P_out(k - 1), h_out Conditions
                    double wmm = refrigerante1.MolecularWeight;
                    refrigerante1.FindStatueWithPH(P_out[k - 1], h_out * wmm);
                    T_out[k - 1] = refrigerante1.Temperature;

                    p_media[k + 1] = (P_ingresso_tratto + P_out[k - 1]) / 2;
                    T_media[k + 1] = (T_ingresso_tratto + T_out[k - 1]) / 2;
                    p_corrente = p_media[k + 1];
                    T1_corrente = T_media[k + 1];

                    Tabella[i - 1, 6] = T_media[k + 1];
                    Tabella[i - 1, 7] = T_out[k - 1];
                    Tabella[i - 1, 12] = p_media[k + 1];
                    Tabella[i - 1, 13] = P_out[k - 1];

                    k = k + 1;

                } while ((p_media[k] - p_media[k - 1]) > (10e-4) || (T_media[k] - T_media[k - 1]) > 0.05);

                T_ingresso = T_out[k - 2];
                P_ingresso = P_out[k - 2];

            }//end for

            //Outputs (results)
            textBox108.Text = Convert.ToString(T_out[k - 2]);
            textBox107.Text = Convert.ToString(P_out[k - 2]);

            //Sheets("OUTPUT Longitudinale").Range("A7:W45") = Tabella
            textBox109.Text = Convert.ToString(Tabella[0, 0]);

            dataGridView1.Rows.Clear();

            for (int counter7 = 0; counter7 < N_tratti_corrente; counter7++)
            {
                dataGridView1.Rows.Add();

                for (int counter8 = 0; counter8 < 14; counter8++)
                {                    
                    dataGridView1.Rows[counter7].Cells[counter8].Value = decimal.Round(Convert.ToDecimal(Tabella[counter7, counter8]), 1, MidpointRounding.AwayFromZero);
                }
            }

            dataGridView2.Rows.Clear();

            for (int counter71 = 0; counter71 < N_tratti_corrente; counter71++)
            {
                dataGridView2.Rows.Add();

                for (int counter81 = 0; counter81 < 9; counter81++)
                {
                    dataGridView2.Rows[counter71].Cells[counter81].Value = decimal.Round(Convert.ToDecimal(Tabella[counter71, counter81 + 14]), 1, MidpointRounding.AwayFromZero);
                }
            }
        }


        //Calculate button
        private void Button2_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "DEF")
            {
                referencestate = ReferenceState.DEF;
            }
            if (comboBox3.Text == "ASH")
            {
                referencestate = ReferenceState.ASH;
            }
            if (comboBox3.Text == "IIR")
            {
                referencestate = ReferenceState.IIR;
            }
            if (comboBox3.Text == "NBP")
            {
                referencestate = ReferenceState.NBP;
            }

            //PureFluid
            if (comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
                refrigerante1 = new Refrigerant(category, this.comboBox2.Text, referencestate);               
            }

            //NewMixture
            if (comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
                refrigerante1 = new Refrigerant(category, this.comboBox2.Text + "=" + textBox35.Text + "," + this.comboBox16.Text + "=" + textBox36.Text + "," + this.comboBox18.Text + "=" + textBox69.Text + "," + this.comboBox17.Text + "=" + textBox70.Text, referencestate);
            }

            if (comboBox1.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
                //refrigerante1 = new Refrigerant(category, this.comboBox2.Text, referencestate);
            }

            if (comboBox1.Text == "PseudoPureFluid")
            {
                category = RefrigerantCategory.PseudoPureFluid;
            }
                        
            refrigerante1.Category = category;
            refrigerante1.reference = referencestate;

            textBox34.Text = Convert.ToString(refrigerante1.CriticalPressure);
            textBox68.Text = Convert.ToString(refrigerante1.CriticalTemperature);
            textBox51.Text = Convert.ToString(refrigerante1.CriticalDensity);          
        }

        private void Button3_Click(object sender, EventArgs e)
        {           
            refrigerante2 = new Refrigerant(RefrigerantCategory.PureFluid, "CO2", ReferenceState.DEF);
            textBox1.Text = Convert.ToString(refrigerante2.CriticalPressure);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            referencestate = ReferenceState.DEF;
            category = RefrigerantCategory.NewMixture;
            refrigerante1 = new Refrigerant(category, "nitrogen" + "=" + "0.7809" + "," + "OXYGEN" + "=" + "0.2095" + "," + "ARGON" + "=" + "0.0093" + "," + "CO2" + "=" + "0.0004", referencestate);
            textBox2.Text = Convert.ToString(refrigerante1.CriticalPressure);
        }

        //Solve Nonlinear Equations System
        private void Button5_Click(object sender, EventArgs e)
        {
            Parameter E1 = new Parameter(1.0);
            Parameter E2 = new Parameter(1.0);

            Func<double>[] functions = new Func<double>[]
           {
                () => (Math.Pow(E1, 2))+E1*E2-10,
                () => E2+(3*E1*(Math.Pow(E2, 2)))-57
           };
            
            //FX(1) = q12conv(x(1)) - q23cond(x(1), x(2))
            //FX(2) = q3SolAbs() - q34conv(x(2), x(3)) - q34rad(x(1), x(2), x(3)) - q23cond(x(1), x(2)) - qcond_brac(x(2))
            //FX(3) = q34conv(x(2), x(3)) + q34rad(x(1), x(2), x(3)) - q45cond(x(3), x(4))
            //FX(4) = q45cond(x(3), x(4)) + q5SolAbs() - q56conv(x(4)) - q57rad(x(4))

            Parameter[] parameters = new Parameter[] { E1, E2 };

            NewtonRaphson nr = new NewtonRaphson(parameters, functions);
            for (int i = 0; i < 15; i++)
            {
                nr.Iterate();
            }

            textBox3.Text = Convert.ToString(E1.Value);
            textBox4.Text = Convert.ToString(E2.Value);
        }

        // q12conv Validation (VALIDADA con Excel de Luca Moreti)
        private void Button6_Click(object sender, EventArgs e)
        {
            double q12conv_resultado = 0.0;

            q12conv_resultado = q12conv(Convert.ToDouble(textBox6.Text));
            textBox5.Text = Convert.ToString(q12conv_resultado);
        }

        //q12conv Calculation (VALIDADA con Excel de Luca Moreti)
        public double q12conv(double T2)
        {
            double q12conv_result = 0.0;
            double Nud2 = 0.0;
            double T1, m, d2, P;

            //T1 = Convert.ToDouble(textBox8.Text); // K
            //P = Convert.ToDouble(textBox7.Text); // kPa
            //m = Convert.ToDouble(textBox9.Text); // kg/s
            //d2 = Convert.ToDouble(textBox10.Text); //meters

            T1 = T1_corrente;
            m = m_corrente;
            P = p_corrente;
            d2 = d2_corrente;

            //Set Refrigerant
            //refrigerante2 = new Refrigerant(RefrigerantCategory.PureFluid, "CO2", ReferenceState.DEF);
            
            //T1 Conditions
            refrigerante1.FindStateWithTP(T1, P);
            double ro1 = refrigerante1.Density; //kg/m3
            double mu1 = refrigerante1.Viscosity; //microPascales.s
            double k1 = refrigerante1.thermalconductivity * 1000; // W/m.K
            //Cp kJ/kmol.K -> if we multiply Cp (kJ/kmol.K) * Molecular weight = Cp (kJ/kg.K) 
            double Pr1 = (refrigerante1.Viscosity * (refrigerante1.Cp / refrigerante1.MolecularWeight) / (refrigerante1.thermalconductivity * 1000));
                     
            //T2 Conditions
            refrigerante1.FindStateWithTP(T2, P);
            double Pr2 = (refrigerante1.Viscosity * (refrigerante1.Cp / refrigerante1.MolecularWeight) / (refrigerante1.thermalconductivity * 1000));

            //Velocity calculation            
            double v1 = 4 * m / (ro1 * Math.PI * Math.Pow(d2, 2)); // m/s
            //Reynolds number calculation
            double Red2 = ((ro1 * v1 * d2) / mu1)/0.000001;
            //Darcy factor calculation
            double f2 = Math.Pow((1.82 * Math.Log(Red2) / Math.Log(10) - 1.64), -2);
            
            //Laminar flow
            if (Red2< 2300) 
            {
                Nud2 = 4.36;
            }

            //Turbulent flow
            else{
                //Correlación de Gnielinsky'
                Nud2 = (f2 / 8 * (Red2 - 1000) * Pr1) * (Math.Pow((Pr1 / Pr2), 0.11)) / (1 + 12.7 * (Math.Pow((f2 / 8), 0.5)) * ((Math.Pow(Pr1 ,(2 / 3))) - 1));
            }

            //HTC calculation
            double h1 = Nud2 * k1 / d2 / 1000; // W/m2.K

            textBox26.Text = Convert.ToString(h1);

            //q12conv calculation
            q12conv_result = h1 * d2 * Math.PI * (T2 - T1); // W/m

            return q12conv_result;
        }

        //q23cond Calculation (VALIDADA con Excel de Luca Moreti)
        private void Button7_Click(object sender, EventArgs e)
        {
            double q23cond_resultado = 0.0;

            q23cond_resultado = q23cond(Convert.ToDouble(textBox16.Text), Convert.ToDouble(textBox14.Text));
            textBox15.Text = Convert.ToString(q23cond_resultado);
        }

        //q23cond Calculation (VALIDADA con Excel de Luca Moreti)
        public double q23cond(double T2, double T3)
        {
            double q23cond_result = 0.0;
            double T23;

            double d2, d3;

            //d2 = Convert.ToDouble(textBox11.Text); //meters
            //d3 = Convert.ToDouble(textBox13.Text); //meters

            d2 = d2_corrente;
            d3 = d3_corrente;

            T23 = (T2 + T3) / 2; // K

            double k23 = 0.013 * (T23 - 273.15) + 15.2;
            q23cond_result = 2 * Math.PI * k23 * (T3 - T2) / Math.Log(d3 / d2);

            textBox27.Text = Convert.ToString(k23);

            return q23cond_result;
        }
        
        //q34conv Calculation (VALIDADA con Excel de Luca Moreti)
        private void Button8_Click(object sender, EventArgs e)
        {
            double q34conv_resultado = 0.0;

            q34conv_resultado = q34conv(Convert.ToDouble(textBox18.Text), Convert.ToDouble(textBox20.Text));
            textBox19.Text = Convert.ToString(q34conv_resultado);
        }

        //
        public double q34conv(double T3, double T4)
        {
            double q34conv_result = 0.0;

            double d3, d4, kstd, B, P_vacio, d, T34;
            //d3 = 0.07;
            //d4 = 0.119;
            d3 = d3_corrente;
            d4 = d4_corrente;

            kstd = 0.02551;
            B = 1.571;
            P_vacio = 0.00012;
            d = 3.53 * 10e-9;
            T34 = (T3+T4)/ 2;

            double lambda = (2.331 * 10e-21 * T34) / (P_vacio*d*d);
            double h34 = kstd / (d3/2 * Math.Log( d4 / d3) + B * lambda*(d3 / d4 + 1));
            q34conv_result = Math.PI * d3 * h34 * (T3 - T4);

            textBox24.Text = Convert.ToString(h34);

            return q34conv_result;
        }

        //q34rad Calculation (VALIDADA con Excel de Luca Moreti)
        private void Button9_Click(object sender, EventArgs e)
        {
            double q34rad_resultado = 0.0;

            q34rad_resultado = q34rad(Convert.ToDouble(textBox37.Text), Convert.ToDouble(textBox31.Text), Convert.ToDouble(textBox33.Text));
            textBox32.Text = Convert.ToString(q34rad_resultado);
        }

        public double q34rad(double T2, double T3, double T4)
        {
            double q34rad_result = 0.0;

            double d3, d4, sigma, T23;
            //d3 = 0.07;
            //d4 = 0.119;

            d3 = d3_corrente;
            d4 = d4_corrente;

            sigma = 5.67 * 10e-9;
        
            T23 = (T2 + T3) / 2;

            double eps3 = 2.64 * 10e-8 * Math.Pow((T23-273.15), 2) + 0.0000125 * (T23 - 273.15) + 0.054;

            textBox25.Text = Convert.ToString(eps3);

            double eps4 = 0.86;

            q34rad_result = sigma * Math.PI*d3*(Math.Pow(T3,4)- Math.Pow(T4, 4))/(1/eps3+(1-eps4)*d3/(eps4*d4));

            return q34rad_result;
        }

        //q45cond Calculation (VALIDADA con Excel de Luca Moreti)
        private void Button10_Click(object sender, EventArgs e)
        {
            double q45cond_resultado = 0.0;

            q45cond_resultado = q45cond(Convert.ToDouble(textBox40.Text), Convert.ToDouble(textBox42.Text));
            textBox41.Text = Convert.ToString(q45cond_resultado);
        }

        public double q45cond(double T4, double T5)
        {
            double d4, d5;
            double q45cond_resul = 0.0;

            //d4 = Convert.ToDouble(textBox38.Text);
            //d5 = Convert.ToDouble(textBox39.Text);

            d4 = d4_corrente;
            d5 = d5_corrente;

            //Conductividad de la Envolvente de Vídrio
            double k45 = 1.04;
            q45cond_resul = 2 * Math.PI * k45 * (T4 - T5) / Math.Log(d5 / d4);

            return q45cond_resul;
        }

        //q56conv Calculation (VALIDADA con Excel de Luca Moreti)
        private void Button11_Click(object sender, EventArgs e)
        {
            double q56conv_resultado = 0.0;

            q56conv_resultado = q56conv(Convert.ToDouble(textBox48.Text));
            textBox47.Text = Convert.ToString(q56conv_resultado);
        }

        //q56conv Calculation (VALIDADA con Excel de Luca Moreti)
        public double q56conv(double T5)
        {

            referencestate = ReferenceState.DEF;
            category = RefrigerantCategory.NewMixture;
            refrigerante2 = new Refrigerant(category, "nitrogen" + "=" + "0.7809" + "," + "OXYGEN" + "=" + "0.2095" + "," + "ARGON" + "=" + "0.0093" + "," + "CO2" + "=" + "0.0004", referencestate);

            double n_for_Pr, T56, beta, T6, v6, d5;
            double c = 0;
            double m = 0;
            double q56conv_result = 0.0;;

            //double T6 = Convert.ToDouble(textBox46.Text);
            //double v6 = Convert.ToDouble(textBox43.Text);

            T6 = Tamb_corrente;
            v6 = v_corrente;
            d5 = d5_corrente;

            //Presión Atmosférica'
            double P6 = 0.101325 * 1000;
            //T6,P6 Conditions
            refrigerante2.FindStateWithTP(T6, P6);
            double k6 = refrigerante2.thermalconductivity * 1000; // W/m.K
            //double d5 = Convert.ToDouble(textBox45.Text);

            //Velocidad del Viento'
            if (v6 > 0.1)
            {
                //T5,P6 Conditions
                refrigerante2.FindStateWithTP(T5, P6);
                double Pr5 = (refrigerante2.Viscosity * (refrigerante2.Cp / refrigerante2.MolecularWeight) / (refrigerante2.thermalconductivity * 1000));

                //T6,P6 Conditions
                refrigerante2.FindStateWithTP(T6, P6);
                double Pr6 = (refrigerante2.Viscosity * (refrigerante2.Cp / refrigerante2.MolecularWeight) / (refrigerante2.thermalconductivity * 1000));

                if (Pr6 < 10)
                {
                    n_for_Pr = 0.37;
                }
                else
                {
                    n_for_Pr = 0.36;
                }

                double ro6 = refrigerante2.Density;
                double mu6 = refrigerante2.Viscosity;
                double Red5 = ro6 * v6 * d5 / (mu6 * 10e-7);

                if (Red5 < 40)
                {
                    c = 0.75;
                    m = 0.4;
                }
                else
                {
                    if(Red5 < 1000)
                    {
                        c = 0.51;
                        m = 0.5;
                    }

                    if(Red5 < 200000)
                    {
                        c = 0.26;
                        m = 0.6;
                    }

                    if (Red5 < 1000000)
                    {
                        c = 0.076;
                        m = 0.7;
                    }
                }

                //Ecuación 2.24, página 16, Zhukauskas' correlation, Incropera and DeWitt 1990'
                double Nud5 = c * (Math.Pow(Red5, m)) * (Math.Pow(Pr6, n_for_Pr)) * Math.Pow((Pr6 / Pr5), 0.25);
                double h56 = k6 * Nud5 / d5 / 1000;
                q56conv_result = h56 * Math.PI * d5 * (T5 - T6);
                return q56conv_result;
            }
            //No Velocidad del Viento'
            else
            { 
                T56 = (T5 + T6) / 2;
                beta = 1 / T56;

                //T6,P6 Conditions
                refrigerante2.FindStateWithTP(T56, P6);
                double mu56 = refrigerante2.Viscosity;
                double ro56 = refrigerante2.Density;
                double nu56 = mu56 / ro56 / 10e5;
                double Pr56 = (refrigerante2.Viscosity * (refrigerante2.Cp / refrigerante2.MolecularWeight) / (refrigerante2.thermalconductivity * 1000));
                double alfa56 = nu56 / Pr56;

                //Ecuación 2.20, página 15, Rayleigh number'
                double Rad5 = 9.81 * beta * Math.Abs(T5 - T6) * (Math.Pow(d5,3)) / (alfa56 * nu56);

                //Ecuación 2.21, página 15, correlación Churchill and Chu'                
                double Nud5 = Math.Pow( 0.6 + ((0.387 * (Math.Pow(Rad5, (1.0 / 6.0))))/(Math.Pow((1 + Math.Pow((0.559 / Pr56), (9.0 / 16.0))), (8.0 / 27.0)))), 2);
                
                double h56 = k6 * Nud5 / d5 / 1000;

                q56conv_result = h56 * Math.PI * d5 * (T5 - T6);

                return q56conv_result;
            }
        }

        //q57rad Calculation (VALIDADA con Excel de Luca Moreti)
        private void Button12_Click(object sender, EventArgs e)
        {
            double q57rad_resultado = 0.0;

            q57rad_resultado = q57rad(Convert.ToDouble(textBox50.Text));
            textBox52.Text = Convert.ToString(q57rad_resultado);
        }

        //q56conv Calculation (VALIDADA con Excel de Luca Moreti)
        public double q57rad(double T5)
        {
            double q57rad_result = 0.0;
            double sigma = 0.0000000567;

            double d5, T6, eps5;

            //double d5 = Convert.ToDouble(textBox49.Text);
            //double T6 = Convert.ToDouble(textBox53.Text);

            d5 = d5_corrente;
            T6 = Tamb_corrente;

            double T7 = T6 - 8;
            //double eps5 = Convert.ToDouble(textBox44.Text);
            eps5 = eps_glass_corrente;

            q57rad_result = sigma * d5 * Math.PI * eps5 * (Math.Pow(T5, 4) - Math.Pow(T7, 4));
            return q57rad_result;
        }

        //q3SolAbs Validated with Luca Moretti excel sheet
        private void Button14_Click(object sender, EventArgs e)
        {
            double q3SolAbs_resultado = 0.0;

            q3SolAbs_resultado = q3SolAbs();
            textBox61.Text = Convert.ToString(q3SolAbs_resultado);
        }

        //q3SolAbs Validated with Luca Moretti excel sheet
        public double q3SolAbs()
        {
            double q3SolAbs_result = 0.0;

            //double DNI = Convert.ToDouble(textBox59.Text);
            //double eta = Convert.ToDouble(textBox58.Text);
            //double apertura = Convert.ToDouble(textBox60.Text);

            double DNI, eta_ott, apertura;

            DNI = DNI_corrente;
            //Eficiencia óptica efectiva del Tubo Absorbedor'
            eta_ott = eta_ott_corrente;
            //Longitud de Apertura (m) del colector solar '
            apertura = apertura_corrente;

            double qsi = DNI * apertura;

            q3SolAbs_result = qsi * eta_ott;

            return q3SolAbs_result;
        }

        //q5SolAbs Validated with Luca Moretti excel sheet
        private void Button15_Click(object sender, EventArgs e)
        {
            double q5SolAbs_resultado = 0.0;

            q5SolAbs_resultado = q5SolAbs();
            textBox62.Text = Convert.ToString(q5SolAbs_resultado);
        }

        //q5SolAbs Validated with Luca Moretti excel sheet
        public double q5SolAbs()
        {
            double q5SolAbs_result = 0.0;
            //double DNI = Convert.ToDouble(textBox64.Text);
            //double eta = Convert.ToDouble(textBox63.Text);
            //double apertura = Convert.ToDouble(textBox65.Text);
            //double qsi = DNI * apertura;

            //double alfa_env = Convert.ToDouble(textBox66.Text);
            //double alfa_abs = Convert.ToDouble(textBox67.Text);
            //double tau_env = Convert.ToDouble(textBox71.Text);

            double DNI, eta_ott, apertura, qsi, alfa_env, alfa_abs, tau_env;

            DNI = DNI_corrente;
            //Eficiencia óptica efectiva en la Envolvente de Vidrio'
            eta_ott = eta_ott_corrente;
            //Longitud de Apertura (m) del colector solar '
            apertura = apertura_corrente;
            //Irradiación solar por medtro de tubo absorbedor (W/m)'
            qsi = DNI * apertura;
            //Absortancia de la Envolvente de Vidrio'
            alfa_env = alfa_env_corrente;
            //Absortancia del Tubo Absorbedor'
            alfa_abs = alfa_asb_corrente;
            //Transmitancia de la Envolvente de Vidrio'
            tau_env = tau_env_corrente;

            q5SolAbs_result = qsi * alfa_env * eta_ott / tau_env / alfa_abs;

            return q5SolAbs_result;
        }

        //qcond_brac Validated with Luca Moretti excel sheet
        private void Button16_Click(object sender, EventArgs e)
        {
            double qcond_brac_resultado = 0.0;

            qcond_brac_resultado = qcond_brac(Convert.ToDouble(textBox77.Text));
            textBox75.Text = Convert.ToString(qcond_brac_resultado);
        }

        public double qcond_brac(double T3)
        {
            double n_for_Pr;
            double c = 0.0;
            double m = 0.0;
            double qcond_brac_result = 0.0;

            double Pb = 0.2032;
            double Acs_b = 0.0001613;
            double kb = 48;
            double Db = 2 * 0.0254;
            double L_HCE = 4;

            referencestate = ReferenceState.DEF;
            category = RefrigerantCategory.NewMixture;
            refrigerante2 = new Refrigerant(category, "nitrogen" + "=" + "0.7809" + "," + "OXYGEN" + "=" + "0.2095" + "," + "ARGON" + "=" + "0.0093" + "," + "CO2" + "=" + "0.0004", referencestate);

            //Temperatura ambiente'
            //double T6 = Convert.ToDouble(textBox79.Text);
            //Velocidad del viento'
            //double v6 = Convert.ToDouble(textBox80.Text);

            double T6, v6;
            //Temperatura ambiente'
            T6 = Tamb_corrente;
            //Velocidad del viento'
            v6 = v_corrente;

            //Presión del ambiente'
            double P6 = 0.101325 * 1000;
            //Conductividad del Aire'
            //T6,P6 Conditions
            refrigerante2.FindStateWithTP(T6, P6);
            double k6 = refrigerante2.thermalconductivity * 1000; // W/m.K
            double Tbase = T3 - 10;
            double Tmedia = (T6 + Tbase) / 3;

            //Velocidad del Viento'
            if (v6 > 0.1)
            {
                //T6,P6 Conditions
                refrigerante2.FindStateWithTP(T6, P6);
                double Pr6 = (refrigerante2.Viscosity * (refrigerante2.Cp / refrigerante2.MolecularWeight) / (refrigerante2.thermalconductivity * 1000));

                if (Pr6 < 10)
                {
                    n_for_Pr = 0.37;
                }
                else
                {
                    n_for_Pr = 0.36;
                }

                double ro6 = refrigerante2.Density;
                double mu6 = refrigerante2.Viscosity;

                //T6,P6 Conditions
                refrigerante2.FindStateWithTP(Tmedia, P6);
                double Prmedio = (refrigerante2.Viscosity * (refrigerante2.Cp / refrigerante2.MolecularWeight) / (refrigerante2.thermalconductivity * 1000));

                double ReDb = ro6 * v6 * Db / (mu6 * 10e-7);

                if (ReDb< 40)
                {
                    c = 0.75;
                    m = 0.4;
                }
                else
                {
                    if (ReDb < 1000)
                    {
                        c = 0.51;
                        m = 0.5;
                    }

                    if (ReDb < 200000)
                    {
                        c = 0.26;
                        m = 0.6;
                    }

                    if (ReDb < 1000000)
                    {
                        c = 0.076;
                        m = 0.7;
                    }
                }
                //Zhukauskas' correlation, ecuación 2.24, página 16'
                double NuDb = c * (Math.Pow(ReDb, m)) * (Math.Pow(Pr6, n_for_Pr)) * Math.Pow((Pr6 / Prmedio),0.25);
                double hDb = k6 * NuDb / Db / 1000;

                //Ecuación 2.31, página 21'
                qcond_brac_result = (Math.Pow((hDb * Pb * kb * Acs_b), 0.5)) * (Tbase - T6) / L_HCE;

                return qcond_brac_result;
            }

            //No Viento'
            else
            {
                double Tb6 = (Tmedia + T6) / 2;

                double beta = 1 / Tb6;

                //T6,P6 Conditions
                refrigerante2.FindStateWithTP(Tb6, P6);
                double mu_b6 = refrigerante2.Viscosity;
                double ro_b6 = refrigerante2.Density;
                double nu_b6 = mu_b6 / ro_b6 / (10e5);
                double Pr_b6 = (refrigerante2.Viscosity * (refrigerante2.Cp / refrigerante2.MolecularWeight) / (refrigerante2.thermalconductivity * 1000));
                double alfa_b6 = nu_b6 / Pr_b6;

                //Rayleigh number, ecuación 2.21, página 15'
                double RaDb = 9.81 * beta * Math.Abs(Tmedia - T6) * Math.Pow(Db, 3) / (alfa_b6 * nu_b6);

                //Churchill correlation, ecuación 2.20,página 15'
                double NuDb = Math.Pow((0.6 + (0.387 * (Math.Pow(RaDb, (1.0 / 6.0))) / Math.Pow((1 + Math.Pow((0.559 / Pr_b6), (9.0 / 16.0))), (8.0 / 27.0)))), 2);
                double hDb = k6 * NuDb / Db / 1000;

                //Ecuación 2.31, página 21'
                qcond_brac_result = (Math.Pow((hDb * Pb * kb * Acs_b), 0.5)) * (Tbase - T6) / L_HCE;

                return qcond_brac_result;
            }
        }

        public void Monodimensionale_vba()
        {
            //Parameter T2 = new Parameter(500.0);
            //Parameter T3 = new Parameter(600.0);
            //Parameter T4 = new Parameter(700.0);
            //Parameter T5 = new Parameter(800.0);

            Func<double>[] USERFUNC = new Func<double>[]
           {
                () => q12conv(T2) - q23cond(T2, T3),
                () => q3SolAbs() - q34conv(T3, T4) - q34rad(T2, T3, T4) - q23cond(T2, T3) - qcond_brac(T3),
                () => q34conv(T3, T4) + q34rad(T2, T3, T4) - q45cond(T4, T5),
                () => q45cond(T4, T5) + q5SolAbs() - q56conv(T5) - q57rad(T5)
           };          

            Parameter[] parameters = new Parameter[] { T2, T3, T4, T5 };

            NewtonRaphson nr = new NewtonRaphson(parameters, USERFUNC);
            for (int i = 0; i < 15; i++)
            {
                nr.Iterate();
            }          
        }        

        //Solve Receiver Heat Losses
        private void Button13_Click(object sender, EventArgs e)
        {
            Monodimensionale_vba();

            textBox56.Text = Convert.ToString(T2.Value);
            textBox57.Text = Convert.ToString(T3.Value);
            textBox54.Text = Convert.ToString(T4.Value);
            textBox55.Text = Convert.ToString(T5.Value);
        }

        //Pressure drop calcuation
        double PressureDrop(double m_dot, double T, double P, double D, double Rough, double L_pipe,
        double Nexp, double Ncon, double Nels, double Nelm, double Nell, double Ngav, double Nglv,
        double Nchv, double Nlw, double Nlcv, double Nbja)
        {

            double rho, v_dot, mu, nu, u_fluid, Re, f, DP_pipe, DP_exp, DP_con, DP_els, DP_elm, DP_ell, DP_gav,
                      DP_glv, DP_chv, DP_lw, DP_lcv, DP_bja, HL_pm;

            //Calculate fluid properties and characteristics
            refrigerante1.FindStateWithTP(T, P);

            rho = refrigerante1.Density;
            mu = refrigerante1.Viscosity; //convert to 
            nu = mu / rho;
            v_dot = m_dot / rho;   //fluid volumetric flow rate
            u_fluid = v_dot / (Math.PI * (D / 2.0) * (D / 2.0));  //Fluid mean velocity

            //Dimensionless numbers
            Re = u_fluid * D / nu;
            //if(Re<2300.) then
            //    f = 64./max(Re,1.0)
            //else
            f = FricFactor(Rough / D, Re);
            if (f == 0) return 0.0; //std::numeric_limits<double>::quiet_NaN();
            //}

            //Calculation of pressure loss from pipe length
            HL_pm = f * u_fluid * u_fluid / (2.0 * D * g);
            DP_pipe = HL_pm * rho * g * L_pipe;

            //Calculation of pressure loss from Fittings
            DP_exp = 0.25 * rho * u_fluid * u_fluid * Nexp;
            DP_con = 0.25 * rho * u_fluid * u_fluid * Ncon;
            DP_els = 0.9 * D / f * HL_pm * rho * g * Nels;
            DP_elm = 0.75 * D / f * HL_pm * rho * g * Nelm;
            DP_ell = 0.6 * D / f * HL_pm * rho * g * Nell;
            DP_gav = 0.19 * D / f * HL_pm * rho * g * Ngav;
            DP_glv = 10.0 * D / f * HL_pm * rho * g * Nglv;
            DP_chv = 2.5 * D / f * HL_pm * rho * g * Nchv;
            DP_lw = 1.8 * D / f * HL_pm * rho * g * Nlw;
            DP_lcv = 10.0 * D / f * HL_pm * rho * g * Nlcv;
            DP_bja = 8.69 * D / f * HL_pm * rho * g * Nbja;

            return DP_pipe + DP_exp + DP_con + DP_els + DP_elm + DP_ell + DP_gav + DP_glv + DP_chv + DP_lw + DP_lcv + DP_bja;
        }


        /**************************************************************************************************
         Friction factor (taken from Piping loss model)
        ***************************************************************************************************
         Uses an iterative method to solve the implicit friction factor function.
         For more on this method, refer to Fox, et al., 2006 Introduction to Fluid Mechanics.			 */
        double FricFactor(double Rough, double Reynold)
        {
            double Test, TestOld, X, Xold, Slope;
            double Acc = .01; //0.0001
            int NumTries;

            if (Reynold < 2750.0)
            {
                return 64.0 / Math.Max(Reynold, 1.0);
            }

            X = 33.33333;  //1. / 0.03
            TestOld = X + 2.0 * Math.Log10(Rough / 3.7 + 2.51 * X / Reynold);
            Xold = X;
            X = 28.5714;  //1. / (0.03 + 0.005)
            NumTries = 0;

            while (NumTries < 21)
            {
                NumTries++;
                Test = X + 2 * Math.Log10(Rough / 3.7 + 2.51 * X / Reynold);
                if (Math.Abs(Test - TestOld) <= Acc)
                {
                    return 1.0 / (X * X);
                }

                Slope = (Test - TestOld) / (X - Xold);
                Xold = X;
                TestOld = Test;
                X = Math.Max((Slope * X - Test) / Slope, 1.0E-5);
            }
            //call Messages(-1," Could not find friction factor solution",'Warning',0,250) 
            return 0;
        }

        private void TextBox104_TextChanged(object sender, EventArgs e)
        {
            textBox90.Text = Convert.ToString(Convert.ToDouble(textBox104.Text)* Convert.ToDouble(textBox92.Text));
            textBox96.Text = Convert.ToString(Convert.ToDouble(textBox104.Text) / Convert.ToDouble(textBox105.Text));
        }

        private void Receiver_Forristall_Load(object sender, EventArgs e)
        {
            textBox90.Text = Convert.ToString(Convert.ToDouble(textBox104.Text) * Convert.ToDouble(textBox92.Text));
            textBox96.Text = Convert.ToString(Convert.ToDouble(textBox104.Text) / Convert.ToDouble(textBox105.Text));
        }



        /***************************************************************************************************
        *********************************************************************
        * PipeFlow_turbulent:                                               *
        * This procedure calculates the average Nusselt number and friction *
        * factor for turbulent flow in a pipe given Reynolds number (Re),   *
        * Prandtl number (Pr), the pipe length diameter ratio (LoverD) and  *
        * the relative roughness}                                           *
        *********************************************************************
        */ /*
        //subroutine PipeFlow(Re, Pr, LoverD,relRough, Nusselt, f)
        void PipeFlow(double Re, double Pr, double LoverD, double relRough, double &Nusselt, double &f){


            double f_fd,Nusselt_L, Gz, Gm, Nusselt_T, Nusselt_H,fR,X;

            //Correlation for laminar flow.. Note that no transitional effects are considered
            if (Re < 2300.) {
                //This procedure calculates the average Nusselt number and friction factor for laminar flow in a pipe 
                //..given Reynolds number (Re), Prandtl number (Pr), the pipe length diameter ratio (LoverD) 
                //..and the relative roughness}
                Gz=Re*Pr/LoverD;
                X=LoverD/Re;
                fR=3.44/sqrt(X)+(1.25/(4*X)+16-3.44/sqrt(X))/(1+0.00021*pow(X,-2));
                f=4.*fR/Re;
                //{f$='Shah' {Shah, R.K.  and London, A.L. "Laminar Flow Forced Convection in Ducts", 
                //..Academic Press, 1978 ,Eqn 192, p98}}
                Gm=pow(Gz,1./3.);
                Nusselt_T=3.66+((0.049+0.02/Pr)*pow(Gz,1.12))/(1+0.065*pow(Gz,0.7));
                Nusselt_H=4.36+((0.1156 +0.08569 /pow(Pr,0.4))*Gz)/(1+0.1158*pow(Gz,0.6));
                //{Nusselt$='Nellis and Klein fit to Hornbeck'  {Shah, R.K.  and London, A.L. "Laminar Flow Forced Convection in Ducts",
                //..Academic Press, 1978 ,Tables  20 and 22}}
                Nusselt = Nusselt_T;  //Constant temperature Nu is better approximation
            }
            else { //Correlation for turbulent flow
                f_fd = pow(0.79*log(Re)-1.64, -2); //Petukhov, B.S., in Advances in Heat Transfer, Vol. 6, Irvine and Hartnett, Academic Press, 1970
                Nusselt_L= ((f_fd/8.)*(Re-1000)*Pr)/(1.+12.7*sqrt(f_fd/8.)*(pow(Pr, 2/3.)-1.)); //Gnielinski, V.,, Int. Chem. Eng., 16, 359, 1976

                if (relRough > 1e-5) {

                  //f=8.*((8./Re)**12+((2.457*log(1./((7./Re)**0.9+0.27*(RelRough))))**16+(37530./Re)**16)**(-1.5))**(1./12.)
                  //mjw 8.30.2010 :: not used  

                  f_fd=pow(-2.*log10(2*relRough/7.4-5.02*log10(2*relRough/7.4+13/Re)/Re), -2);

                  Nusselt_L= ((f_fd/8.)*(Re-1000.)*Pr)/(1.+12.7*sqrt(f_fd/8.)*(pow(Pr, 2/3.)-1.)); //Gnielinski, V.,, Int. Chem. Eng., 16, 359, 1976}
                }
                f=f_fd*(1.+pow(1./LoverD, 0.7)); //account for developing flow
                Nusselt= Nusselt_L*(1.+pow(1./LoverD, 0.7));  //account for developing flow
            }

        }  */


    }
}
