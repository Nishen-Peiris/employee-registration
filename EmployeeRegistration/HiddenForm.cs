using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EmployeeRegistration
{
    public partial class HiddenForm : Form
    {
        public HiddenForm()
        {
            InitializeComponent();
        }

        private void HiddenForm_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Hide();

            Loading nf = new Loading();
            nf.Show();
        }
    }
}
