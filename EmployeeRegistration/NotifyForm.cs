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
    public partial class NotifyForm : Form
    {
        public NotifyForm()
        {
            InitializeComponent();
        }

        //Initializing Variables...

        int y = 0;

        //Form Load...

        private void NotifyForm_Load(object sender, EventArgs e)
        {
            timSlide.Enabled = true;

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                    Screen.PrimaryScreen.WorkingArea.Height);
        }

        //TimSlide effect...

        private void timFade_Tick(object sender, EventArgs e)
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

        //about Link...

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();

            about nf = new about();
            nf.Show();
        }
    }
}
