using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EmployeeRegistration
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }

        string SignedIn;

        public string GetConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["EmployeeRegistration.Properties.Settings.Database1ConnectionString"].ConnectionString;
        }

        private void ExecuteUpdate(string SignedIn)
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            string sql = "UPDATE tbl_Data SET SignedIn = @SignedIn WHERE (ID = 1)";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlParameter[] param = new SqlParameter[1];
                //param[0] = new SqlParameter("@id", SqlDbType.Int, 20);
                param[0] = new SqlParameter("@SignedIn", SqlDbType.VarChar, 50);

                param[0].Value = SignedIn;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            timFade.Enabled = true;

            //Fade in Effect...

            /* Set Form's Transperancy 100 % */
            this.Opacity = 0;

            /* Start the Timer To Animate Form */
            timFade.Enabled = true;

            //Notification...

            NotifyForm nf = new NotifyForm();
            nf.Show();

            //Single Instance...

            Process[] _process = null;
            _process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (_process.Length > 1)
            {
                Process[] _proceses = null;
                _proceses = Process.GetProcessesByName("EmployeeRegistration");
                foreach (Process proces in _proceses)
                {
                    proces.Kill();
                }
            }
            
            //Reset tbl_Data...

            SignedIn = "No";

        tryagain:
            try
            {
                ExecuteUpdate(SignedIn);
            }
            catch (Exception ex)
            {
                goto tryagain;
            }

            //Timer1 Start...

            timer1.Start();
        }

        //timFade Tick

        private void timFade_Tick(object sender, EventArgs e)
        {
            /* Make Form Visible a Bit more on Every timer Tick */
            this.Opacity += 0.01;
        }

        //Timer1...

        private void timer1_Tick(object sender, EventArgs e)
        {
            Close();

            Signin_and_Select_Task nf = new Signin_and_Select_Task();
            nf.Show();
        }        
    }
}
