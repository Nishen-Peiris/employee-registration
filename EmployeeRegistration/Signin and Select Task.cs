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

namespace EmployeeRegistration
{
    public partial class Signin_and_Select_Task : Form
    {
        public Signin_and_Select_Task()
        {
            InitializeComponent();
        }

        string MessageBoxContent;
        string MessageBoxTitle = "Confirm your action.";
        string SignedIn, RC;

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

        //KeyDown event...

        private void Signin_and_Select_Task_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SignedIn == "Yes")
                {
                    Close();

                    if (radioButton1.Checked == true)
                    {
                        Register nf = new Register();
                        nf.Show();
                    }
                    else if (radioButton2.Checked == true)
                    {
                        Add_Comment nf = new Add_Comment();
                        nf.Show();
                    }
                    else
                    {
                        View_All nf = new View_All();
                        nf.Show();
                    }
                }
                else
                {
                    if (txtUserName.Text != "" && txtPassword.Text != "")
                    {
                        txtUserName.Text = txtUserName.Text.ToLower();
                        txtPassword.Text = txtPassword.Text.ToLower();

                        if ((txtUserName.Text == "a") && (txtPassword.Text == "a"))
                        {
                            //Enable Switch Pane...

                            radioButton1.Enabled = true;
                            radioButton2.Enabled = true;
                            radioButton3.Enabled = true;

                            radioButton1.Checked = true;

                            pbSwitch.Visible = true;

                            //Signed In...

                            SignedIn = "Yes";

                        tryagain:
                            try
                            {
                                ExecuteUpdate(SignedIn);
                            }
                            catch (Exception ex)
                            {
                                goto tryagain;
                            }

                            //Lock Controls...

                            txtUserName.Enabled = false;
                            txtPassword.Enabled = false;
                            pbSignin.Visible = false;

                            MessageBox.Show("You are signed in now!");
                        }
                        else
                        {
                            MessageBox.Show("Username & Passsword didn't match. Please try again!");

                            txtUserName.Text = "";
                            txtPassword.Text = "";
                        }
                    }

                }
            }
        }

        //Loading...

        private void Signin_and_Select_Task_Load(object sender, EventArgs e)
        {
            pbMinimizeM.Visible = false;
            pbCloseM.Visible = false;

            listBox1.Visible = false;

            //Fade in Effect...

            timFade.Enabled = true;

            /* Set Form's Transperancy 100 % */
            this.Opacity = 0;

            /* Start the Timer To Animate Form */
            timFade.Enabled = true;

            //Check SignedIn Status...

            string connetionStringSignedIn = null;
            SqlConnection connectionSignedIn;
            SqlCommand commandSignedIn;
            SqlDataAdapter adapterSignedIn = new SqlDataAdapter();
            DataSet dsSignedIn = new DataSet();
            string sqlSignedIn = null;

            connetionStringSignedIn = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            sqlSignedIn = "select ID, SignedIn from tbl_Data";

            connectionSignedIn = new SqlConnection(connetionStringSignedIn);

            tryagainSignedIn:
            try
            {
                connectionSignedIn.Open();
                commandSignedIn = new SqlCommand(sqlSignedIn, connectionSignedIn);
                adapterSignedIn.SelectCommand = commandSignedIn;
                adapterSignedIn.Fill(dsSignedIn);
                adapterSignedIn.Dispose();
                commandSignedIn.Dispose();
                connectionSignedIn.Close();

                listBox1.DataSource = dsSignedIn.Tables[0];
                listBox1.ValueMember = "ID";
                listBox1.DisplayMember = "SignedIn";

            }
            catch (Exception ex)
            {
                goto tryagainSignedIn;
            }

            SignedIn = listBox1.Text;

            if (SignedIn == "No")
            {
                pbSignin.Visible = false;
                pbSigninM.Visible = false;

                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;

                pbSwitch.Visible = false;
                pbSwitchM.Visible = false;
            }
            else
            {
                txtUserName.Text = "sampath";
                txtPassword.Text = "sanuri1971";
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;

                pbSignin.Visible = false;
                pbSigninM.Visible = false;

                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;

                pbSwitch.Visible = true;
                pbSwitchM.Visible = false;
            }

            //Row Count...

            string connetionStringRC = null;
            SqlConnection connectionRC;
            SqlCommand commandRC;
            SqlDataAdapter adapterRC = new SqlDataAdapter();
            DataSet dsRC = new DataSet();
            string sqlRC = null;

            connetionStringRC = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            sqlRC = "select * from tbl_EmployeeData";
            connectionRC = new SqlConnection(connetionStringRC);

        tryagainRC:
            try
            {
                connectionRC.Open();
                commandRC = new SqlCommand(sqlRC, connectionRC);
                adapterRC.SelectCommand = commandRC;
                adapterRC.Fill(dsRC, "tbl_EmployeeData");
                adapterRC.Dispose();
                commandRC.Dispose();
                connectionRC.Close();

                RC = Convert.ToString(dsRC.Tables[0].Rows.Count);
            }
            catch
            {
                goto tryagainRC;
            }

            //if no records...

            if (Convert.ToInt16(RC) == 0)
            {
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;

                txtRC.Text = "No employee has been registered yet.";
            }
            else if (Convert.ToInt16(RC) == 1)
            {
                txtRC.Text = "1 employee has been regisetered.";
            }
            else
            {
                txtRC.Text = RC + " employees have been registered.";
            }

            this.ShowInTaskbar = false;
            this.ShowInTaskbar = true;
        }

        //timFade Tick

        private void timFade_Tick(object sender, EventArgs e)
        {
            /* Make Form Visible a Bit more on Every timer Tick */
            this.Opacity += 0.01;
        }

        //Minimize Button...

        private void pbMinimize_MouseEnter(object sender, EventArgs e)
        {
            pbMinimizeM.Visible = true;
        }

        private void pbMinimizeM_MouseLeave(object sender, EventArgs e)
        {
            pbMinimizeM.Visible = false;
        }

        private void pbMinimizeM_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //Close Button...

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbCloseM.Visible = true;
        }

        private void pbCloseM_MouseLeave(object sender, EventArgs e)
        {
            pbCloseM.Visible = false;
        }

        private void pbCloseM_Click(object sender, EventArgs e)
        {
            MessageBoxContent = "Are you sure you want to exit?";

            DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        //Username & Password...

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                pbSignin.Visible = false;
            }
            else
            {
                if (txtPassword.Text == "")
                {
                    pbSignin.Visible = false;
                }
                else
                {
                    pbSignin.Visible = true;
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                pbSignin.Visible = false;
            }
            else
            {
                if (txtUserName.Text == "")
                {
                    pbSignin.Visible = false;
                }
                else
                {
                    pbSignin.Visible = true;
                }
            }
        }

        //Signin Button...

        private void pbSignin_MouseEnter(object sender, EventArgs e)
        {
            pbSigninM.Visible = true;
        }

        private void pbSigninM_MouseLeave(object sender, EventArgs e)
        {
            pbSigninM.Visible = false;
        }

        private void pbSigninM_Click(object sender, EventArgs e)
        {
            txtUserName.Text = txtUserName.Text.ToLower();
            txtPassword.Text = txtPassword.Text.ToLower();

            if ((txtUserName.Text == "a") && (txtPassword.Text == "a"))
            {
                //Enable Switch Pane...

                radioButton1.Enabled = true;

                //if no records...

                if (Convert.ToInt16(RC) == 0)
                {
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                }
                else
                {
                    radioButton2.Enabled = true;
                    radioButton3.Enabled = true;
                }

                radioButton1.Checked = true;

                pbSwitch.Visible = true;

                //Signed In...

                SignedIn = "Yes";

            tryagain:
                try
                {
                    ExecuteUpdate(SignedIn);
                }
                catch (Exception ex)
                {
                    goto tryagain;
                }

                //Lock Controls...

                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                pbSignin.Visible = false;

                MessageBox.Show("You are signed in now!");
            }
            else
            {
                MessageBox.Show("Username & Passsword didn't match. Please try again!");

                txtUserName.Text = "";
                txtPassword.Text = "";
            }
        }

        //Switch Button...

        private void pbSwitch_MouseEnter(object sender, EventArgs e)
        {
            pbSwitchM.Visible = true;
        }

        private void pbSwitchM_MouseLeave(object sender, EventArgs e)
        {
            pbSwitchM.Visible = false;
        }

        private void pbSwitchM_Click(object sender, EventArgs e)
        {
            Close();

            if (radioButton1.Checked == true)
            {
                Register nf = new Register();
                nf.Show();
            }
            else if (radioButton2.Checked == true)
            {
                Add_Comment nf = new Add_Comment();
                nf.Show();
            }
            else
            {
                View_All nf = new View_All();
                nf.Show();
            }
        }        
    }
}
