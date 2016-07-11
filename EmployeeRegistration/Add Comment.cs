using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EmployeeRegistration
{
    public partial class Add_Comment : Form
    {
        SqlConnection cnnP;
        string connectionStringP;

        public Add_Comment()
        {
            InitializeComponent();
        }

        //Initialize Variables...

        string ID, RC;
        string MessageBoxContent;
        string MessageBoxTitle = "Confirm your action.";

        public string GetConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["EmployeeRegistration.Properties.Settings.Database1ConnectionString"].ConnectionString;
        }

        private void ExecuteUpdate(string Comments, string ID)
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            string sql = "UPDATE tbl_EmployeeData SET Comments = @Comments WHERE (ID = @ID)";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@ID", SqlDbType.Int, 20);
                param[0] = new SqlParameter("@Comments", SqlDbType.VarChar);
                param[1] = new SqlParameter("@ID", SqlDbType.Int, 20);

                param[0].Value = Comments;
                param[1].Value = ID;

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

        //KeyDown Event...

        private void Add_Comment_KeyDown(object sender, KeyEventArgs e)
        {
            if (pbAddComment.Visible == true)
            {
            //Execute Update...

tryagainUpdate:
                try
                {
                    ExecuteUpdate(txtComments.Text, ID);
                }
                catch
                {
                    goto tryagainUpdate;
                }

                //Done..

                MessageBox.Show("Comments were successfully added to " + comboBox1.Text + "'s profile!");

                Signin_and_Select_Task nf = new Signin_and_Select_Task();
                nf.Show();

                Close();
            }
        }

        //Loading...

        private void Add_Comment_Load(object sender, EventArgs e)
        {
            //Fade in Effect...

            timFade.Enabled = true;

            /* Set Form's Transperancy 100 % */
            this.Opacity = 0;

            /* Start the Timer To Animate Form */
            timFade.Enabled = true;

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
                txtRC.Text = "No employee has been registered yet.";
            }
            else
            {
                pbCloseM.Visible = false;
                pbMinimizeM.Visible = false;

                pbAddComment.Visible = false;
                pbAddCommentM.Visible = false;

                dateOfRegisterToolStripMenuItem.Checked = true;

                txtRC.Text = RC + " employees have been registered.";

                //ComboBox1 Databinding...

                string connetionStringCB = null;
                SqlConnection connectionCB;
                SqlCommand commandCB;
                SqlDataAdapter adapterCB = new SqlDataAdapter();
                DataSet dsCB = new DataSet();
                string sqlCB = null;

                connetionStringCB = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlCB = "select ID, FullName from tbl_EmployeeData order by ID";

                connectionCB = new SqlConnection(connetionStringCB);

            tryagainCB:
                try
                {
                    connectionCB.Open();
                    commandCB = new SqlCommand(sqlCB, connectionCB);
                    adapterCB.SelectCommand = commandCB;
                    adapterCB.Fill(dsCB);
                    adapterCB.Dispose();
                    commandCB.Dispose();
                    connectionCB.Close();

                    comboBox1.DataSource = dsCB.Tables[0];
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "FullName";

                }
                catch
                {
                    goto tryagainCB;
                }
            }

            if (Convert.ToInt16(RC) == 1)
            {
                txtRC.Text = "1 employee has been regisetered.";
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

        //Close Button...

        private void pbClose_MouseEnter(object sender, EventArgs e)
        {
            pbCloseM.Visible = true;
        }

        private void pbCloseM_MouseLeave(object sender, EventArgs e)
        {
            pbCloseM.Visible = false;
        }

        //Sort by...

        //Name...

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nameToolStripMenuItem.Checked == true)
            {
                //Databinding...

                string connetionStringCB = null;
                SqlConnection connectionCB;
                SqlCommand commandCB;
                SqlDataAdapter adapterCB = new SqlDataAdapter();
                DataSet dsCB = new DataSet();
                string sqlCB = null;

                connetionStringCB = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlCB = "select ID, FullName from tbl_EmployeeData order by FullName desc";

                connectionCB = new SqlConnection(connetionStringCB);

            tryagainCB:
                try
                {
                    connectionCB.Open();
                    commandCB = new SqlCommand(sqlCB, connectionCB);
                    adapterCB.SelectCommand = commandCB;
                    adapterCB.Fill(dsCB);
                    adapterCB.Dispose();
                    commandCB.Dispose();
                    connectionCB.Close();

                    comboBox1.DataSource = dsCB.Tables[0];
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "FullName";

                }
                catch
                {
                    goto tryagainCB;
                }

            }
            else
            {
                nameToolStripMenuItem.Checked = true;
                dateOfBirthToolStripMenuItem.Checked = false;
                dateOfRegisterToolStripMenuItem.Checked = false;

                //Databinding...

                string connetionStringCB = null;
                SqlConnection connectionCB;
                SqlCommand commandCB;
                SqlDataAdapter adapterCB = new SqlDataAdapter();
                DataSet dsCB = new DataSet();
                string sqlCB = null;

                connetionStringCB = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlCB = "select ID, FullName from tbl_EmployeeData order by FullName";

                connectionCB = new SqlConnection(connetionStringCB);

            tryagainCB:
                try
                {
                    connectionCB.Open();
                    commandCB = new SqlCommand(sqlCB, connectionCB);
                    adapterCB.SelectCommand = commandCB;
                    adapterCB.Fill(dsCB);
                    adapterCB.Dispose();
                    commandCB.Dispose();
                    connectionCB.Close();

                    comboBox1.DataSource = dsCB.Tables[0];
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "FullName";

                }
                catch
                {
                    goto tryagainCB;
                }

            }
        }

        //DOB...

        private void dateOfBirthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dateOfBirthToolStripMenuItem.Checked == true)
            {
                //Databinding...

                string connetionStringCB = null;
                SqlConnection connectionCB;
                SqlCommand commandCB;
                SqlDataAdapter adapterCB = new SqlDataAdapter();
                DataSet dsCB = new DataSet();
                string sqlCB = null;

                connetionStringCB = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlCB = "select ID, FullName from tbl_EmployeeData order by DOB desc";

                connectionCB = new SqlConnection(connetionStringCB);

            tryagainCB:
                try
                {
                    connectionCB.Open();
                    commandCB = new SqlCommand(sqlCB, connectionCB);
                    adapterCB.SelectCommand = commandCB;
                    adapterCB.Fill(dsCB);
                    adapterCB.Dispose();
                    commandCB.Dispose();
                    connectionCB.Close();

                    comboBox1.DataSource = dsCB.Tables[0];
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "FullName";

                }
                catch
                {
                    goto tryagainCB;
                }

            }
            else
            {
                dateOfBirthToolStripMenuItem.Checked = true;
                nameToolStripMenuItem.Checked = false;
                dateOfRegisterToolStripMenuItem.Checked = false;

                //Databinding...

                string connetionStringCB = null;
                SqlConnection connectionCB;
                SqlCommand commandCB;
                SqlDataAdapter adapterCB = new SqlDataAdapter();
                DataSet dsCB = new DataSet();
                string sqlCB = null;

                connetionStringCB = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlCB = "select ID, FullName from tbl_EmployeeData order by DOB";

                connectionCB = new SqlConnection(connetionStringCB);

            tryagainCB:
                try
                {
                    connectionCB.Open();
                    commandCB = new SqlCommand(sqlCB, connectionCB);
                    adapterCB.SelectCommand = commandCB;
                    adapterCB.Fill(dsCB);
                    adapterCB.Dispose();
                    commandCB.Dispose();
                    connectionCB.Close();

                    comboBox1.DataSource = dsCB.Tables[0];
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "FullName";

                }
                catch
                {
                    goto tryagainCB;
                }
            }
        }

        //DOR...

        private void dateOfRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dateOfRegisterToolStripMenuItem.Checked == true)
            {
                //Databinding...

                string connetionStringCB = null;
                SqlConnection connectionCB;
                SqlCommand commandCB;
                SqlDataAdapter adapterCB = new SqlDataAdapter();
                DataSet dsCB = new DataSet();
                string sqlCB = null;

                connetionStringCB = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlCB = "select ID, FullName from tbl_EmployeeData order by RegisteredOn desc";

                connectionCB = new SqlConnection(connetionStringCB);

            tryagainCB:
                try
                {
                    connectionCB.Open();
                    commandCB = new SqlCommand(sqlCB, connectionCB);
                    adapterCB.SelectCommand = commandCB;
                    adapterCB.Fill(dsCB);
                    adapterCB.Dispose();
                    commandCB.Dispose();
                    connectionCB.Close();

                    comboBox1.DataSource = dsCB.Tables[0];
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "FullName";

                }
                catch
                {
                    goto tryagainCB;
                }

            }
            else
            {
                dateOfRegisterToolStripMenuItem.Checked = true;
                nameToolStripMenuItem.Checked = false;
                dateOfBirthToolStripMenuItem.Checked = false;

                //Databinding...

                string connetionStringCB = null;
                SqlConnection connectionCB;
                SqlCommand commandCB;
                SqlDataAdapter adapterCB = new SqlDataAdapter();
                DataSet dsCB = new DataSet();
                string sqlCB = null;

                connetionStringCB = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlCB = "select ID, FullName from tbl_EmployeeData order by RegisteredOn";

                connectionCB = new SqlConnection(connetionStringCB);

            tryagainCB:
                try
                {
                    connectionCB.Open();
                    commandCB = new SqlCommand(sqlCB, connectionCB);
                    adapterCB.SelectCommand = commandCB;
                    adapterCB.Fill(dsCB);
                    adapterCB.Dispose();
                    commandCB.Dispose();
                    connectionCB.Close();

                    comboBox1.DataSource = dsCB.Tables[0];
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "FullName";

                }
                catch
                {
                    goto tryagainCB;
                }
            }
        }    

        private void pbCloseM_Click(object sender, EventArgs e)
        {
            MessageBoxContent = "You haven't edit any comment. Are you sure you want to exit?";

            DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
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
            WindowState = FormWindowState.Maximized;
        }

        //ComboBox1...

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get ID...

            ID = Convert.ToString(comboBox1.SelectedValue);

            //pbPhoto...

            try
            {
                connectionStringP = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                cnnP = new SqlConnection(connectionStringP);

                MemoryStream streamP = new MemoryStream();
                cnnP.Open();
                SqlCommand commandP = new SqlCommand("select Photo from tbl_EmployeeData where (ID = @ID)", cnnP);
                commandP.Parameters.AddWithValue("@ID", ID);
                byte[] imageP = (byte[])commandP.ExecuteScalar();
                streamP.Write(imageP, 0, imageP.Length);
                cnnP.Close();
                Bitmap bitmapP = new Bitmap(streamP);
                pbPhoto.Image = bitmapP;

                pbPhoto.Visible = true;
            }
            catch
            {
                pbPhoto.Visible = false;
            }

            //Comments...

            string connetionStringC = null;
            SqlConnection connectionC;
            SqlCommand commandC;
            SqlDataAdapter adapterC = new SqlDataAdapter();
            DataSet dsC = new DataSet();
            string sqlC = null;

            connetionStringC = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            sqlC = "select ID, Comments from tbl_EmployeeData";

            connectionC = new SqlConnection(connetionStringC);

            try
            {
                connectionC.Open();
                commandC = new SqlCommand(sqlC, connectionC);
                adapterC.SelectCommand = commandC;
                adapterC.Fill(dsC);
                adapterC.Dispose();
                commandC.Dispose();
                connectionC.Close();

                listBox1.DataSource = dsC.Tables[0];
                listBox1.ValueMember = "ID";
                listBox1.DisplayMember = "Comments";
            }
            catch
            {

            }

            listBox1.SelectedIndex = comboBox1.SelectedIndex;
        }

        //listBox1...

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtComments.Text = Convert.ToString(listBox1.Text);
        }

        //txtComments...

        private void txtComments_TextChanged(object sender, EventArgs e)
        {
            if (txtComments.Text != Convert.ToString(listBox1.Text))
            {
                pbAddComment.Visible = true;
            }
        }   

        //Add Comment Buttton...

        private void pbAddComment_MouseEnter(object sender, EventArgs e)
        {
            pbAddCommentM.Visible = true;
        }

        private void pbAddCommentM_MouseLeave(object sender, EventArgs e)
        {
            pbAddCommentM.Visible = false;
        }

        private void pbAddCommentM_Click(object sender, EventArgs e)
        {
            MessageBoxContent = "Are you sure you want to save these details?";

            DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
            //Execute Update...

tryagainUpdate:
                try
                {
                    ExecuteUpdate(txtComments.Text, ID);
                }
                catch
                {
                    goto tryagainUpdate;
                }

                //Done..

                MessageBox.Show("Comments were successfully added to " + comboBox1.Text + "'s profile!");

                Signin_and_Select_Task nf = new Signin_and_Select_Task();
                nf.Show();

                Close();
            }
            else if (dialogResult == DialogResult.No)
            {

            }            
        }

        //Switch to link...

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBoxContent = "You haven't edit any comment. Are you sure you want to exit?";

            DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();

                Signin_and_Select_Task nf = new Signin_and_Select_Task();
                nf.Show();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }    
    }
}
