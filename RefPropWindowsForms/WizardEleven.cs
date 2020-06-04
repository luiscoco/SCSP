using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RefPropWindowsForms
{
    public partial class WizardEleven : Form
    {
        MainWindow puntero;

        public WizardEleven(MainWindow puntero1)
        {
            puntero = puntero1;
            InitializeComponent();
        }
    }
}
