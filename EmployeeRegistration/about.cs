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
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        //Initializing Variables...

        int y = 0;

        private void about_Load(object sender, EventArgs e)
        {
            timSlide.Enabled = true;

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                    Screen.PrimaryScreen.WorkingArea.Height);
        }

        //TimSlide effect...

        private void timSlide_Tick(object sender, EventArgs e)
        {
            if (y == this.Height)
            {
                timSlide.Enabled = false;

                timClose.Enabled = true;
            }
            else
            {
                y = y + 1;

                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                    Screen.PrimaryScreen.WorkingArea.Height - y);
            }   
        }

        //timClose Tick...

        private void timClose_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
