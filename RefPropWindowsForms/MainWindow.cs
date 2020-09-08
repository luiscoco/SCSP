﻿using System;
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
    public partial class MainWindow : Form
    {
        public About About_dialog;
        public PTC_Solar_Field PTC_Solar_Field_dialog;

        public Configurations_Summary Configurations_Summary_dialog;
        public WizardOne Wizard_dialog;
        public WizardTwo WizardDos_dialog;
        public WizardThree WizardTres_dialog;
        public WizardFour WizardCuatro_dialog;
        public WizardFive WizardCinco_dialog;
        public WizardSix WizardSeis_dialog;
        public WizardSeven WizardSiete_dialog;
        public WizardEight WizardOcho_dialog;
        public WizardNine WizardNueve_dialog;
        public WizardTen WizardDiez_dialog;
        public WizardEleven WizardOnce_dialog;
        public WizardTwelve WizardDoce_dialog;
        public WizardThirteen WizardTrece_dialog;
        public WizardFourteen WizardCatorce_dialog;
        public WizardFifteen WizardQuince_dialog;
        public WizardSixteen WizardDieciseis_dialog;
        public WizardSeventeen WizardDiecisiete_dialog;
        public WizardEighteen WizardDieciocho_dialog;
        public WizardNineteen WizardDiecinueve_dialog;
        public WizardTwenty WizardVeinte_dialog;
        public WizardTwentyOne WizardVeinteUno_dialog;
        public WizardTwentyTwo WizardVeinteDos_dialog;
        public WizardTwentyThree WizardVeinteTres_dialog;
        public WizardTwentyFour WizardVeinteCuatro_dialog;
        public WizardTwentyFive WizardVeinteCinco_dialog;
        public WizardTwentySix WizardVeinteSeis_dialog;
        public WizardTwentySeven WizardVeinteSiete_dialog;
        public WizardTwentyEight WizardVeinteOcho_dialog;
        public WizardTwentyNine WizardVeinteNueve_dialog;
        public WizardThirty WizardTreinta_dialog;
        public WizardThirtyOne WizardTreintaUno_dialog;
        public WizardThirtyTwo WizardTreintaDos_dialog;
        public WizardThirtyThree WizardTreintaTres_dialog;
        public WizardThirtyFour WizardTreintaCuatro_dialog;
        public WizardThirtyFive WizardTreintaCinco_dialog;

        public AdobePDFViewer AdobePDFViewer_dialog;
        public ChartsExample ChartsExample_dialog;
        public Receiver_Forristall Receiver_Forristall_dialog;

        public Configuration_Form Configuration_window;
        public Recompression_Brayton_Power_Cycle RCwindow;
        public RC_Optimization RC_optimization_window;
        
        public RC_without_ReHeating_new_proposed_configuration RC_without_ReHeating_new_configuration_window;
        public PCRC_without_ReHeating_new_proposed_configuration PCRC_without_ReHeating_new_proposed_configuration_window;
        public RCMCI_without_ReHeating_new_proposed_configuration RCMCI_without_ReHeating_new_proposed_configuration_window;

        public RC_withReHeating_new_proposed_configuration RC_withReHeating_new_configuration_window;
        public PCRC_withReHeating_new_proposed_configuration PCRC_withReHeating_new_configuration_window;
        public RCMCI_with_ReHeating_new_proposed_configuration RCMCI_with_ReHeating_new_configuration_window;

        public RC_with_Two_ReHeating_new_proposed_configuration RC_with_Two_ReHeating_new_configuration_window;
        public PCRC_with_Two_ReHeating_new_proposed_configuration PCRC_with_Two_ReHeating_new_configuration_window;
        public RCMCI_with_Two_ReHeating_new_proposed_configuration RCMCI_with_Two_ReHeating_new_configuration_window;

        public Effec_Recomp_Fract Sensing_Effec_Recomp_Frac;
        public Effec_CIT Sensing_Effect_CIT;
        public Effec_TIP Sensing_Effect_TIP;
        public Effec_TIT Effec_TIT_RC_withReHeating_Dialog;
        public Effec_TIT_withoutReHeating Effec_TIT_RC_withoutReHeating_Dialog;
        public Effec_Recomp_Fract_RCMCI_withReHeating Effec_Recomp_Fract_RCMCI_withReHeating_Dialog;
        public Effec_CIT_RCMCI_withReHeating Effec_CIT_RCMCI_withReHeating_Dialog;
        public Effec_TIP_RCMCI_withReHeating Effec_TIP_RCMCI_withReHeating_Dialog;
        public Effec_CIT_RCMCI_withoutReHeating Effec_CIT_RCMCI_withoutReHeating_Dialog;
        public Effec_TIP_RCMCI_withoutReHeating Effec_TIP_RCMCI_withoutReHeating_Dialog;
        public Effec_Recomp_Fract_RCMCI_withoutReHeating Effec_Recomp_Fract_RCMCI_withoutReHeating_Dialog;
        public Off_design_from_Design_Point off_design_from_design;
        public PCRC PCRC_design_dialog;
        public RCMCI RCMCI_design_dialog;
        public RC_without_ReHeating RC_without_ReHeating;
        public RC_optimal_without_ReHeating RC_optimal_without_ReHeating;
        public Off_design_without_ReHeating RC_off_design_without_ReHeating;
        public Target_off_design_without_ReHeating Target_off_design_without_ReHeating_Dialog;
        public off_design_from_optimization off_design_from_optimization_dialog;
        public Target_offdesign_from_optimization Target_offdesign_fromOptimization;
        public Optimal_Alt_Off_Design_from_Design Optimal_Alt_Off_Design_from_Design_Dialog;
        public PCRC_without_ReHeating PCRC_without_ReHeating_Dialog;
        public PCRC_with_ReHeating PCRC_with_ReHeating_Dialog;
        public PCRC_optimal_withoutReHeating PCRC_optimal_withoutReHeating_Dialog;
        public RCMCI_without_ReHeating RCMCI_without_ReHeating_Dialog;
        public RCMCI_optimal RCMCI_optimal_dialog;
        public RCMCI_optimal_without_RH RCMCI_optimal_without_RH_dialog;
        public Effec_Recomp_Fract_withoutReHeating Effec_Recomp_Fract_withoutReHeating_Dialog;
        public Effec_CIT_withoutReHeating Effec_CIT_withoutReHeating_Dialog;
        public Effec_TIP_withoutReHeating Effec_TIP_withoutReHeating_Dialog;
        public Effec_Recomp_Fract_PCRC_withoutReHeating Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog;
        public Effec_CIT_PCRC_withoutReHeating Effec_CIT_PCRC_withoutReHeating_Dialog;
        public Effec_Recomp_Fract_PCRC_withReHeating Effec_Recomp_Fract_PCRC_withReHeating_Dialog;
        public Effec_TIP_PCRC_withReHeating Effec_TIP_PCRC_withReHeating_Dialog;
        public Effec_CIT_PCRC_withReHeating Effec_CIT_PCRC_withReHeating_Dialog;
        public RC_with_Two_ReHeating RC_with_Two_ReHeating_dialog;
        public RC_with_Three_ReHeating RC_with_Three_ReHeating_dialog;
        public RC_with_Four_ReHeating RC_with_Four_ReHeating_dialog;
        public RC_with_Five_ReHeating RC_with_Five_ReHeating_dialog;
        public PCRC_with_Two_ReHeating PCRC_with_Two_ReHeating_dialog;
        public PCRC_with_Three_ReHeating PCRC_with_Three_ReHeating_dialog;
        public PCRC_with_Four_ReHeatings PCRC_with_Four_ReHeating_dialog;
        public PCRC_with_Five_ReHeatings PCRC_with_Five_ReHeating_dialog;
        public RCMCI_with_Two_Reheatings RCMCI_with_Two_ReHeating_dialog;
        public RCMCI_with_Three_Reheatings RCMCI_with_Three_ReHeating_dialog;
        public RCMCI_with_Four_Reheatings RCMCI_with_Four_ReHeating_dialog;
        public RCMCI_with_Five_Reheatings RCMCI_with_Five_ReHeating_dialog;
        public PCRCMCI_withoutReHeating PCRCMCI_without_ReHeating_dialog;
        public PCRCMCI PCRCMCI_withReHeating_dialog;
        public PCRCMCI_with_Two_ReHeating PCRCMCI_with_Two_ReHeating_dialog;
        public PCRCMCI_with_Three_ReHeating PCRCMCI_with_Three_ReHeating_dialog;
        public PCRC_with_Two_Intercooling_without_ReHeating PCRC_with_Two_Intercooling_without_ReHeating_dialog;
        public PCRC_with_Two_Intercooling_with_ReHeating PCRC_with_Two_Intercooling_with_ReHeating_dialog;
        public PCRC_with_Two_Intercooling_with_Two_ReHeating PCRC_with_Two_Intercooling_with_Two_ReHeating_dialog;
        public PCRC_with_Two_Intercooling_with_Three_ReHeating PCRC_with_Two_Intercooling_with_Three_ReHeating_dialog;
        //public PCRC_with_Two_Intercooling_with_Three_ReHeating PCRC_with_Two_Intercooling_with_Three_ReHeating_dialog;
        public RCMCI_with_Two_Intercooling_without_Reheating RCMCI_with_Two_Intercooling_without_ReHeating_dialog;
        public RCMCI_with_Two_Intercooling_with_Reheating RCMCI_with_Two_Intercooling_with_ReHeating_dialog;
        public RCMCI_with_Two_Intercooling_with_Two_Reheating RCMCI_with_Two_Intercooling_with_Two_ReHeating_dialog;
        public RCMCI_with_Two_Intercooling_with_Three_Reheating RCMCI_with_Two_Intercooling_with_Three_ReHeating_dialog;
        //public RCMCI_with_Two_Intercooling_with_Three_Reheating RCMCI_with_Two_Intercooling_with_Three_ReHeating_dialog;
        public Two_PC_RCMCI_withoutReheating Two_PC_RCMCI_withoutReheating_dialog;
        public PC_Two_RCMCI_withoutReheating PC_Two_RCMCI_withoutReheating_dialog;
        public Two_PC_Two_RCMCI_withoutReheating Two_PC_Two_RCMCI_withoutReheating_dialog;
        public Two_PC_RCMCI_withReheating Two_PC_RCMCI_withReheating_dialog;
        public PC_Two_RCMCI_withReheating PC_Two_RCMCI_withReheating_dialog;
        public Two_PC_Two_RCMCI_withReheating Two_PC_Two_RCMCI_withReheating_dialog;
        public PC_Two_RCMCI_with_Two_Reheating PC_Two_RCMCI_with_Two_Reheating_dialog;
        public Two_PC_RCMCI_with_Two_Reheating Two_PC_RCMCI_with_Two_Reheating_dialog;
        public Two_PC_Two_RCMCI_with_Two_Reheating Two_PC_Two_RCMCI_with_Two_Reheating_dialog;
        public PC_Two_RCMCI_with_Three_Reheating PC_Two_RCMCI_with_Three_Reheating_dialog;
        public Two_PC_RCMCI_with_Three_Reheating Two_PC_RCMCI_with_Three_Reheating_dialog;
        public Two_PC_Two_RCMCI_with_Three_Reheating Two_PC_Two_RCMCI_with_Three_Reheating_dialog;


        public SB_with_PreHeating_without_Reheating SB_with_PreHeating_without_Reheating_dialog;
        public SB_with_Three_Recuperators_without_ReHeating SB_with_Three_Recuperators_without_ReHeating_dialog;
        public SB_with_Two_Recuperators_and_Additional_HX_without_ReHeating SB_with_Two_Recuperators_and_Additional_HX_without_ReHeating_dialog;
        public SB_with_Two_Recuperators_without_ReHeating SB_with_Two_Recuperators_without_ReHeating_dialog;
        public SB_without_Reheating SB_without_Reheating_dialog;
        public SB_with_Four_Recuperators_without_ReHeating SB_with_Four_Recuperators_without_ReHeating_dialog;
        public SB_with_Three_Recuperators_and_One_Recompressors_without_ReHeating SB_with_Three_Recuperators_and_One_Recompressor_without_ReHeating_dialog;
        public SB_with_Three_Recuperators_and_Two_Recompressors_without_ReHeating SB_with_Three_Recuperators_and_Two_Recompressor_without_ReHeating_dialog;
        public SB_with_Three_Recuperators_and_Two_Recompressors_with_ReHeating SB_with_Three_Recuperators_and_Two_Recompressors_with_ReHeating_dialog;
        public SB_with_Four_Recuperators_and_Three_Recompressors_without_ReHeating SB_with_Four_Recuperators_and_Three_Recompressors_without_ReHeating_dialog;
        public SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating SB_with_Two_Recuperators_with_LTR_PreHeating_without_ReHeating_dialog;
        
        public RC_with_LTR_PreHeating_without_ReHeating RC_with_LTR_PreHeating_without_ReHeating_dialog;
        public RC_with_HTR_PreHeating_without_ReHeating RC_with_HTR_PreHeating_without_ReHeating_dialog;
        public RC_with_HTR_and_LTR_PreHeating_without_ReHeating RC_with_HTR_and_LTR_PreHeating_without_ReHeating_dialog;

        public RC_with_LTR_PreHeating_with_ReHeating RC_with_LTR_PreHeating_with_ReHeating_dialog;
        public RC_with_HTR_PreHeating_with_ReHeating RC_with_HTR_PreHeating_with_ReHeating_dialog;
        public RC_with_HTR_and_LTR_PreHeating_with_ReHeating RC_with_HTR_and_LTR_PreHeating_with_ReHeating_dialog;

        public snl_compressor_tsr SNL_Compressor;
        public snl_radial_turbine SNL_Turbine;
        public Radial_Turbine RadialTurbine;
        public TurboMachineOutlet TurboMachine_Outlet;
        public IsoentropicEfficiency Isoentropic_effc;
        public REFPROP_Interface REFPROP_properties;
        public REFPROP_Interface_Mixture REFPROP_Interface_Mixture_dialogue;
        public Radial_Turbine_Design Radial_Turbine_Design_dialogue;
        public TOPGEN_V3 TOPGEN_V3_dialogue;
        public Optimization Simplex_dialog;

        public HeatExchangerUA HX_Conductance;

        public core CoreHX = new core();
        public RefrigerantCategory category;
        public ReferenceState referencestate;

        public String Fluids_Path_LCE;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        //Recompression (RC) Brayton Power cycle Design-Point.
        public void DesignPoint_Click(object sender, EventArgs e)
        {
            //Create a new Form for the RC Design-Point
            RCwindow = new Recompression_Brayton_Power_Cycle(this);
            RCwindow.MdiParent = this;
            RCwindow.Show();

        }

        //Heat Exchanger (HX) Conductance (UA) calculation
        private void heatExchangerConductanceUACalculationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create a new Form for Heat Exchangers Conductance (UA) calculation
            HX_Conductance = new HeatExchangerUA();
            HX_Conductance.MdiParent = this;
            HX_Conductance.Show();

            //Refrigerant Category
            if (HX_Conductance.comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
            }
             if (HX_Conductance.comboBox1.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
            }
             if (HX_Conductance.comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;         
            }
             if (HX_Conductance.comboBox1.Text == "PseudoPureFluid")
            {
                category = RefrigerantCategory.PseudoPureFluid;
            }


            //Refrigerant State
             if (HX_Conductance.comboBox3.Text == "DEF")
             {
                 referencestate = ReferenceState.DEF;
             }
             if (HX_Conductance.comboBox3.Text == "ASH")
             {
                 referencestate = ReferenceState.ASH;
             }
             if (HX_Conductance.comboBox3.Text == "IIR")
             {
                 referencestate = ReferenceState.IIR;
             }
             if (HX_Conductance.comboBox3.Text == "NBP")
             {
                 referencestate = ReferenceState.NBP;
             }

             CoreHX.core1(HX_Conductance.comboBox2.Text, category);
             CoreHX.working_fluid.Category = category;
             CoreHX.working_fluid.reference = referencestate;

             HX_Conductance.HeatExchangerUA1(CoreHX); 
        }

        // Optimal_Design Menu Option
        public void optimizationdesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create a new Form for the RC Opimizing the Design-Point
            RC_optimization_window = new RC_Optimization();
            RC_optimization_window.MdiParent = this;
            RC_optimization_window.Show();
        }

        // Auto_Optimal_Design Menu Option
        public void autooptimaldesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create a new Form for the RC Opimizing the Design-Point
            //RC_AutoOptimalwindow = new RC_Auto_Optimal();
            //RC_AutoOptimalwindow.MdiParent = this;
            //RC_AutoOptimalwindow.Show();
        }

        //SNL_Compressor dialog show
        public void sToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //SNL_Radial_Turbine dialog show (with a ONE Stage Recompressor)
        public void sandiaLaboratoryTurbineSizingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SNL_Turbine = new snl_radial_turbine();
            SNL_Turbine.MdiParent = this;
            SNL_Turbine.Show();
        }

        //Radial_Turbine dialog show
        public void radialTurbineSizingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadialTurbine = new Radial_Turbine();
            RadialTurbine.MdiParent = this;
            RadialTurbine.Show();
        }

        //SNL_Compressor dialog show (with a TWO Stages Recompressor)
        public void compressorSizingAndRecompressorWithTwoStagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SNL_Compressor = new snl_compressor_tsr();
            SNL_Compressor.MdiParent = this;
            SNL_Compressor.Show();
        }

        //TurboMachine Outlet Conditions
        public void turboMachinesOutletDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurboMachine_Outlet = new TurboMachineOutlet();
            TurboMachine_Outlet.MdiParent = this;
            TurboMachine_Outlet.Show();
        }

        //Isoentropic Efficiency calculation 
        public void isoentropicPolitropicEfficienciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Isoentropic_effc = new IsoentropicEfficiency();
            Isoentropic_effc.MdiParent = this;
            Isoentropic_effc.Show();
        }

        //REFPROP properties interface
        public void rEFPROPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REFPROP_properties = new REFPROP_Interface();
            REFPROP_properties.MdiParent = this;
            REFPROP_properties.Show();
        }

        public void optimizationSimplexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Simplex_dialog = new Optimization();
            Simplex_dialog.MdiParent = this;
            Simplex_dialog.Show();
        }

        //RefProp Path Configuration
        public void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuration_window = new Configuration_Form(this);
            Configuration_window.MdiParent = this;
            Configuration_window.Show();
        }

        //Sensing Recompression Fraction Dialog
        public void recompFractionAndUAVariationsNetEfficiencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sensing_Effec_Recomp_Frac = new Effec_Recomp_Fract();
            Sensing_Effec_Recomp_Frac.MdiParent = this;
            Sensing_Effec_Recomp_Frac.Show();
        }

        //Show CIT Sensing Dialog
        public void netEfficiencyVersusCompressorInletTemperatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sensing_Effect_CIT = new Effec_CIT();
            Sensing_Effect_CIT.MdiParent = this;
            Sensing_Effect_CIT.Show();
        }

        //Show TIP Sensing Dialog
        public void recompFracctionAndUAVariationsNetEfficiencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sensing_Effect_TIP = new Effec_TIP();
            Sensing_Effect_TIP.MdiParent = this;
            Sensing_Effect_TIP.Show();
        }

        public void offDesignSubroutineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            off_design_from_design = new Off_design_from_Design_Point();
            off_design_from_design.MdiParent = this;
            off_design_from_design.Show();
        }

        public void designPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PCRC_design_dialog = new PCRC();
            PCRC_design_dialog.MdiParent = this;
            PCRC_design_dialog.Show();
        }

        public void designPointToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RCMCI_design_dialog = new RCMCI();
            RCMCI_design_dialog.MdiParent = this;
            RCMCI_design_dialog.Show();
        }

        public void designPointToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RC_without_ReHeating = new RC_without_ReHeating();
            RC_without_ReHeating.MdiParent = this;
            RC_without_ReHeating.Show();
        }

        public void optimaldesignToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RC_optimal_without_ReHeating = new RC_optimal_without_ReHeating();
            RC_optimal_without_ReHeating.MdiParent = this;
            RC_optimal_without_ReHeating.Show();
        }

        public void offdesignfromDesignPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RC_off_design_without_ReHeating = new Off_design_without_ReHeating();
            RC_off_design_without_ReHeating.MdiParent = this;
            RC_off_design_without_ReHeating.Show();
        }

        public void targetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Target_off_design_without_ReHeating_Dialog = new Target_off_design_without_ReHeating();
            Target_off_design_without_ReHeating_Dialog.MdiParent = this;
            Target_off_design_without_ReHeating_Dialog.Show();
        }

        public void offdesignfromOptimizationDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            off_design_from_optimization_dialog = new off_design_from_optimization();
            off_design_from_optimization_dialog.MdiParent = this;
            off_design_from_optimization_dialog.Show();
        }

        public void targetOffdesignfromOptimizationDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Target_offdesign_fromOptimization = new Target_offdesign_from_optimization();
            Target_offdesign_fromOptimization.MdiParent = this;
            Target_offdesign_fromOptimization.Show();
        }

        //Optimal Off-Design from Design Point without ReHeating
        public void optimalOffdesignfromDesignPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Optimal_Alt_Off_Design_from_Design_Dialog = new Optimal_Alt_Off_Design_from_Design();
            Optimal_Alt_Off_Design_from_Design_Dialog.MdiParent = this;
            Optimal_Alt_Off_Design_from_Design_Dialog.Show();
        }

        //Optimal Off-Design from Optmization-Design Point without ReHeating
        public void optimalOffdesignfromOptimizationDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //PCRC without ReHeating at Design-Point
        public void designPointToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            PCRC_without_ReHeating_Dialog = new PCRC_without_ReHeating();
            PCRC_without_ReHeating_Dialog.MdiParent = this;
            PCRC_without_ReHeating_Dialog.Show();
        }

        //PCRC without ReHeating Optimization Design
        public void optimaldesignToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            PCRC_optimal_withoutReHeating_Dialog = new PCRC_optimal_withoutReHeating();
            PCRC_optimal_withoutReHeating_Dialog.MdiParent = this;
            PCRC_optimal_withoutReHeating_Dialog.Show();
        }

        //PCRC with ReHeating Optimization Design
        public void optimaldesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PCRC_with_ReHeating_Dialog = new PCRC_with_ReHeating();
            PCRC_with_ReHeating_Dialog.MdiParent = this;
            PCRC_with_ReHeating_Dialog.Show();
        }

        //RCMCI without ReHeating at Design-Point
        public void designPointToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            RCMCI_without_ReHeating_Dialog = new RCMCI_without_ReHeating();
            RCMCI_without_ReHeating_Dialog.MdiParent = this;
            RCMCI_without_ReHeating_Dialog.Show();
        }

        //RCMCI with ReHeating Optimal conditions
        public void optimaldesignToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RCMCI_optimal_dialog = new RCMCI_optimal();
            RCMCI_optimal_dialog.MdiParent = this;
            RCMCI_optimal_dialog.Show();
        }

        //Sensing Recompression Flow Fraction in RC without ReHeating
        public void recompressionFractionSensingAnalysisToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_withoutReHeating_Dialog = new Effec_Recomp_Fract_withoutReHeating();
            Effec_Recomp_Fract_withoutReHeating_Dialog.MdiParent = this;
            Effec_Recomp_Fract_withoutReHeating_Dialog.Show();
        }

        //Sensing CIT in RC without ReHeating
        public void compressorInletTemperatureSensingAnalysisToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Effec_CIT_withoutReHeating_Dialog = new Effec_CIT_withoutReHeating();
            Effec_CIT_withoutReHeating_Dialog.MdiParent = this;
            Effec_CIT_withoutReHeating_Dialog.Show();
        }

        //Sensing TIP in RC without ReHeating
        public void turbineInletPressureSensingAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Effec_TIP_withoutReHeating_Dialog = new Effec_TIP_withoutReHeating();
            Effec_TIP_withoutReHeating_Dialog.MdiParent = this;
            Effec_TIP_withoutReHeating_Dialog.Show();
        }

        //Sensing Recompression Flow Fraction in PCRC without ReHeating
        public void recompressionFractionSensingAnalysisToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog = new Effec_Recomp_Fract_PCRC_withoutReHeating();
            Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog.MdiParent = this;
            Effec_Recomp_Fract_PCRC_withoutReHeating_Dialog.Show();
        }

        //Sensing CIT in PCRC without ReHeating
        public void compressorInletTemperatureSensingAnalysisToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Effec_CIT_PCRC_withoutReHeating_Dialog = new Effec_CIT_PCRC_withoutReHeating();
            Effec_CIT_PCRC_withoutReHeating_Dialog.MdiParent=this;
            Effec_CIT_PCRC_withoutReHeating_Dialog.Show();
        }

        //Sensing Recompression Flow Fraction in PCRC with ReHeating
        public void recompressionFractionSensingAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_PCRC_withReHeating_Dialog = new Effec_Recomp_Fract_PCRC_withReHeating();
            Effec_Recomp_Fract_PCRC_withReHeating_Dialog.MdiParent = this;
            Effec_Recomp_Fract_PCRC_withReHeating_Dialog.Show();
        }

        //Sensing TIP in PCRC with ReHeating
        public void turbineInletPressureSensingAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Effec_TIP_PCRC_withReHeating_Dialog = new Effec_TIP_PCRC_withReHeating();
            Effec_TIP_PCRC_withReHeating_Dialog.MdiParent = this;
            Effec_TIP_PCRC_withReHeating_Dialog.Show();
        }

        //Sensing CIT in PCRC with ReHeating
        public void compressorInletTemperatureSensingAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Effec_CIT_PCRC_withReHeating_Dialog = new Effec_CIT_PCRC_withReHeating();
            Effec_CIT_PCRC_withReHeating_Dialog.MdiParent = this;
            Effec_CIT_PCRC_withReHeating_Dialog.Show();
        }

        public void optimaldesignToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            RCMCI_optimal_without_RH_dialog = new RCMCI_optimal_without_RH();
            RCMCI_optimal_without_RH_dialog.MdiParent = this;
            RCMCI_optimal_without_RH_dialog.Show();
        }

        public void sensingAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        public void MainWindow_Load(object sender, EventArgs e)
        {
            this.aboutToolStripMenuItem_Click(this, e);

            //Configurations_Summary_dialog = new Configurations_Summary(this);
            //Configurations_Summary_dialog.MdiParent = this;
            //Configurations_Summary_dialog.Show();

            Wizard_dialog = new WizardOne(this);
            Wizard_dialog.MdiParent = this;
            Wizard_dialog.Show();
        }

        //PTC Solar Field Design
        public void parabolicCollectorDetailDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PTC_Solar_Field_dialog = new PTC_Solar_Field();
            PTC_Solar_Field_dialog.MdiParent = this;
            PTC_Solar_Field_dialog.Show();

            //Refrigerant Category
            if (PTC_Solar_Field_dialog.comboBox1.Text == "PureFluid")
            {
                category = RefrigerantCategory.PureFluid;
            }
            if (PTC_Solar_Field_dialog.comboBox1.Text == "PredefinedMixture")
            {
                category = RefrigerantCategory.PredefinedMixture;
            }
            if (PTC_Solar_Field_dialog.comboBox1.Text == "NewMixture")
            {
                category = RefrigerantCategory.NewMixture;
            }
            if (PTC_Solar_Field_dialog.comboBox1.Text == "PseudoPureFluid")
            {
                category = RefrigerantCategory.PseudoPureFluid;
            }

            //Refrigerant State
            if (PTC_Solar_Field_dialog.comboBox3.Text == "DEF")
            {
                referencestate = ReferenceState.DEF;
            }
            if (PTC_Solar_Field_dialog.comboBox3.Text == "ASH")
            {
                referencestate = ReferenceState.ASH;
            }
            if (PTC_Solar_Field_dialog.comboBox3.Text == "IIR")
            {
                referencestate = ReferenceState.IIR;
            }
            if (PTC_Solar_Field_dialog.comboBox3.Text == "NBP")
            {
                referencestate = ReferenceState.NBP;
            }

            CoreHX.core1(PTC_Solar_Field_dialog.comboBox2.Text, category);
            CoreHX.working_fluid.Category = category;
            CoreHX.working_fluid.reference = referencestate;

            PTC_Solar_Field_dialog.PTC_Solar_Field_uno(CoreHX);
        }

        //About window
        public void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_dialog = new About(this);
            About_dialog.ShowDialog();
        }

        //Wizard
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Wizard_dialog = new WizardOne(this);
            Wizard_dialog.MdiParent = this;
            Wizard_dialog.Show();
        }

        //REFPROP_Mixtures_Dialogue
        private void rEFPROPMixtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            REFPROP_Interface_Mixture_dialogue = new REFPROP_Interface_Mixture();
            REFPROP_Interface_Mixture_dialogue.MdiParent = this;
            REFPROP_Interface_Mixture_dialogue.Show();
        }

        //TOPGEN_V2_Radial_Turbine_Design
        private void tOPGENV2rRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Radial_Turbine_Design_dialogue = new Radial_Turbine_Design();
            Radial_Turbine_Design_dialogue.MdiParent = this;
            Radial_Turbine_Design_dialogue.Show();
        }

        //TOPGEN_V3
        private void tOPGENV3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TOPGEN_V3_dialogue = new TOPGEN_V3();
            TOPGEN_V3_dialogue.MdiParent = this;
            TOPGEN_V3_dialogue.Show();
        }

        private void pTCSolarFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PTC_SF_Calculation PTC_SF_Calculation_window = new PTC_SF_Calculation();
            PTC_SF_Calculation_window.Show();
        }

        private void lFSolarFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LF_SF_Calculation LF_SF_Calculation_window = new LF_SF_Calculation();
            LF_SF_Calculation_window.Show();
        }

        private void dualLoopSolarFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dual_Loop_SF_Calculation Dual_Loop_SF_Calculation_window = new Dual_Loop_SF_Calculation();
            Dual_Loop_SF_Calculation_window.Show();
        }

        private void subcriticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PCRCMCI PCRCMCI_window = new PCRCMCI();
            PCRCMCI_window.Show();
        }

        //Wizard Configurations 1-6
        private void configurations16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Wizard_dialog = new WizardOne(this);
            Wizard_dialog.MdiParent = this;
            Wizard_dialog.Show();
        }

        //Wizard Configurations 7-12
        private void configurations712ToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            WizardDos_dialog = new WizardTwo(this);
            WizardDos_dialog.MdiParent = this;
            WizardDos_dialog.Show();
        }

        //Wizard Configurations 13-18
        private void configurations1318ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardTres_dialog = new WizardThree(this);
            WizardTres_dialog.MdiParent = this;
            WizardTres_dialog.Show();
        }

        //Wizard Configurations 19-24
        private void configurations1924ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardCuatro_dialog = new WizardFour(this);
            WizardCuatro_dialog.MdiParent = this;
            WizardCuatro_dialog.Show();
        }

        private void configurations2224ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardCinco_dialog = new WizardFive(this);
            WizardCinco_dialog.MdiParent = this;
            WizardCinco_dialog.Show();
        }

        private void configurations2527ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardSeis_dialog = new WizardSix(this);
            WizardSeis_dialog.MdiParent = this;
            WizardSeis_dialog.Show();
        }

        private void configurations2830ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardSiete_dialog = new WizardSeven(this);
            WizardSiete_dialog.MdiParent = this;
            WizardSiete_dialog.Show();
        }

        private void configurations3133ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardOcho_dialog = new WizardEight(this);
            WizardOcho_dialog.MdiParent = this;
            WizardOcho_dialog.Show();
        }

        private void configurations3437ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardNueve_dialog = new WizardNine(this);
            WizardNueve_dialog.MdiParent = this;
            WizardNueve_dialog.Show();
        }

        private void configurations3840ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardDiez_dialog = new WizardTen(this);
            WizardDiez_dialog.MdiParent = this;
            WizardDiez_dialog.Show();
        }

        private void turbineInletTemperatureSensingAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Effec_TIT_RC_withReHeating_Dialog = new Effec_TIT();
            Effec_TIT_RC_withReHeating_Dialog.MdiParent = this;
            Effec_TIT_RC_withReHeating_Dialog.Show();
        }

        private void turbineInletTemperatureSensingAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Effec_TIT_RC_withoutReHeating_Dialog = new Effec_TIT_withoutReHeating();
            Effec_TIT_RC_withoutReHeating_Dialog.MdiParent = this;
            Effec_TIT_RC_withoutReHeating_Dialog.Show();
        }

        private void recompressionFractionSensingAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_RCMCI_withReHeating_Dialog = new Effec_Recomp_Fract_RCMCI_withReHeating();
            Effec_Recomp_Fract_RCMCI_withReHeating_Dialog.MdiParent = this;
            Effec_Recomp_Fract_RCMCI_withReHeating_Dialog.Show();
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Effec_CIT_RCMCI_withReHeating_Dialog = new Effec_CIT_RCMCI_withReHeating();
            Effec_CIT_RCMCI_withReHeating_Dialog.MdiParent = this;
            Effec_CIT_RCMCI_withReHeating_Dialog.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Effec_TIP_RCMCI_withReHeating_Dialog = new Effec_TIP_RCMCI_withReHeating();
            Effec_TIP_RCMCI_withReHeating_Dialog.MdiParent = this;
            Effec_TIP_RCMCI_withReHeating_Dialog.Show();
        }

        //Recompression Fraction
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Effec_Recomp_Fract_RCMCI_withoutReHeating_Dialog = new Effec_Recomp_Fract_RCMCI_withoutReHeating();
            Effec_Recomp_Fract_RCMCI_withoutReHeating_Dialog.MdiParent = this;
            Effec_Recomp_Fract_RCMCI_withoutReHeating_Dialog.Show();
        }

        //Compressor Inlet Temperature
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Effec_CIT_RCMCI_withoutReHeating_Dialog = new Effec_CIT_RCMCI_withoutReHeating();
            Effec_CIT_RCMCI_withoutReHeating_Dialog.MdiParent = this;
            Effec_CIT_RCMCI_withoutReHeating_Dialog.Show();
        }

        //TIT
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Effec_TIP_RCMCI_withoutReHeating_Dialog = new Effec_TIP_RCMCI_withoutReHeating();
            Effec_TIP_RCMCI_withoutReHeating_Dialog.MdiParent = this;
            Effec_TIP_RCMCI_withoutReHeating_Dialog.Show();
        }

        private void configurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardOnce_dialog = new WizardEleven(this);
            WizardOnce_dialog.MdiParent = this;
            WizardOnce_dialog.Show();
        }

        private void configurations5257ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardDoce_dialog = new WizardTwelve(this);
            WizardDoce_dialog.MdiParent = this;
            WizardDoce_dialog.Show();
        }

        private void adobePDFViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdobePDFViewer_dialog = new AdobePDFViewer();
            AdobePDFViewer_dialog.MdiParent = this;
            AdobePDFViewer_dialog.Show();

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            if (dlg.FileName != "")
            {
                AdobePDFViewer_dialog.axAcroPDF1.src = dlg.FileName;
            }
        }

        private void chartsExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartsExample_dialog = new ChartsExample();
            ChartsExample_dialog.MdiParent = this;
            ChartsExample_dialog.Show();
        }

        private void ReceiverForristalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Receiver_Forristall_dialog = new Receiver_Forristall(this);
            Receiver_Forristall_dialog.MdiParent = this;
            Receiver_Forristall_dialog.Show();
        }

        private void newProposedConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RC_without_ReHeating_new_configuration_window = new RC_without_ReHeating_new_proposed_configuration();
            RC_without_ReHeating_new_configuration_window.MdiParent = this;
            RC_without_ReHeating_new_configuration_window.Show();
        }

        private void configurations58ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardTrece_dialog = new WizardThirteen(this);
            WizardTrece_dialog.MdiParent = this;
            WizardTrece_dialog.Show();
        }

        private void configuration6469ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardCatorce_dialog = new WizardFourteen(this);
            WizardCatorce_dialog.MdiParent = this;
            WizardCatorce_dialog.Show();
        }

        private void configuration7075ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardQuince_dialog = new WizardFifteen(this);
            WizardQuince_dialog.MdiParent = this;
            WizardQuince_dialog.Show();
        }

        private void configurations7681ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardDieciseis_dialog = new WizardSixteen(this);
            WizardDieciseis_dialog.MdiParent = this;
            WizardDieciseis_dialog.Show();
        }

        private void configurations8287ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardDiecisiete_dialog = new WizardSeventeen(this);
            WizardDiecisiete_dialog.MdiParent = this;
            WizardDiecisiete_dialog.Show();
        }

        private void configurations8893ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardDieciocho_dialog = new WizardEighteen(this);
            WizardDieciocho_dialog.MdiParent = this;
            WizardDieciocho_dialog.Show();
        }

        private void configurations9499ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardDiecinueve_dialog = new WizardNineteen(this);
            WizardDiecinueve_dialog.MdiParent = this;
            WizardDiecinueve_dialog.Show();
        }

        private void configurations100105ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardVeinte_dialog = new WizardTwenty(this);
            WizardVeinte_dialog.MdiParent = this;
            WizardVeinte_dialog.Show();
        }

        private void tesisDeFranciscoCrespiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdobePDFViewer_dialog = new AdobePDFViewer();
            AdobePDFViewer_dialog.MdiParent = this;
            AdobePDFViewer_dialog.axAcroPDF1.src = "C:\\SCSP\\PhD-Crespi-Final-1.pdf";
            AdobePDFViewer_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardVeinteUno_dialog = new WizardTwentyOne();
            WizardVeinteUno_dialog.MdiParent = this;
            WizardVeinteUno_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WizardVeinteDos_dialog = new WizardTwentyTwo();
            WizardVeinteDos_dialog.MdiParent = this;
            WizardVeinteDos_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            WizardVeinteTres_dialog = new WizardTwentyThree();
            WizardVeinteTres_dialog.MdiParent = this;
            WizardVeinteTres_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            WizardVeinteCuatro_dialog = new WizardTwentyFour();
            WizardVeinteCuatro_dialog.MdiParent = this;
            WizardVeinteCuatro_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            WizardVeinteCinco_dialog = new WizardTwentyFive();
            WizardVeinteCinco_dialog.MdiParent = this;
            WizardVeinteCinco_dialog.Show();
        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardVeinteSeis_dialog = new WizardTwentySix();
            WizardVeinteSeis_dialog.MdiParent = this;
            WizardVeinteSeis_dialog.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            WizardVeinteSiete_dialog = new WizardTwentySeven();
            WizardVeinteSiete_dialog.MdiParent = this;
            WizardVeinteSiete_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            WizardVeinteOcho_dialog = new WizardTwentyEight();
            WizardVeinteOcho_dialog.MdiParent = this;
            WizardVeinteOcho_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            WizardVeinteNueve_dialog = new WizardTwentyNine();
            WizardVeinteNueve_dialog.MdiParent = this;
            WizardVeinteNueve_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            WizardTreinta_dialog = new WizardThirty();
            WizardTreinta_dialog.MdiParent = this;
            WizardTreinta_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            WizardTreintaUno_dialog = new WizardThirtyOne();
            WizardTreintaUno_dialog.MdiParent = this;
            WizardTreintaUno_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            WizardTreintaDos_dialog = new WizardThirtyTwo();
            WizardTreintaDos_dialog.MdiParent = this;
            WizardTreintaDos_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            WizardTreintaTres_dialog = new WizardThirtyThree();
            WizardTreintaTres_dialog.MdiParent = this;
            WizardTreintaTres_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            WizardTreintaCuatro_dialog = new WizardThirtyFour();
            WizardTreintaCuatro_dialog.MdiParent = this;
            WizardTreintaCuatro_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem12_Click(object sender, EventArgs e)
        {
            WizardTreintaCinco_dialog = new WizardThirtyFive();
            WizardTreintaCinco_dialog.MdiParent = this;
            WizardTreintaCinco_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem13_Click(object sender, EventArgs e)
        {
            WizardTrece_dialog = new WizardThirteen(this);
            WizardTrece_dialog.MdiParent = this;
            WizardTrece_dialog.Show();
        }

        private void configurationsXXXXToolStripMenuItem14_Click(object sender, EventArgs e)
        {

        }

        private void configurationsXXXXToolStripMenuItem15_Click(object sender, EventArgs e)
        {

        }

        private void configurationsXXXXToolStripMenuItem16_Click(object sender, EventArgs e)
        {

        }

        private void configurationsXXXXToolStripMenuItem17_Click(object sender, EventArgs e)
        {

        }

        private void configurationsXXXXToolStripMenuItem18_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            WizardTreintaTres_dialog = new WizardThirtyThree();
            WizardTreintaTres_dialog.MdiParent = this;
            WizardTreintaTres_dialog.Show();
        }

        private void adobePDFViewerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AdobePDFViewer_dialog = new AdobePDFViewer();
            AdobePDFViewer_dialog.MdiParent = this;
            AdobePDFViewer_dialog.Show();

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            if (dlg.FileName != "")
            {
                AdobePDFViewer_dialog.axAcroPDF1.src = dlg.FileName;
            }
        }

        private void chartsExampleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChartsExample_dialog = new ChartsExample();
            ChartsExample_dialog.MdiParent = this;
            ChartsExample_dialog.Show();
        }

        private void tesisFrancescoCrespiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdobePDFViewer_dialog = new AdobePDFViewer();
            AdobePDFViewer_dialog.MdiParent = this;
            AdobePDFViewer_dialog.axAcroPDF1.src = "C:\\SCSP\\References\\PhD-Crespi-Final-1.pdf";
            AdobePDFViewer_dialog.Show();
        }

        private void tesisToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
