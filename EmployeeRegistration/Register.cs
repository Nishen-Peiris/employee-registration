using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace EmployeeRegistration
{
    public partial class Register : Form
    {
        SqlConnection cnn;
        string connectionString = null;

        public Register()
        {
            InitializeComponent();
        }

        //Initialize Variables...

        string RegDate, RC;
        string filepath = "No Path";
        string MessageBoxContent;
        string MessageBoxTitle = "Confirm your action.";

        private byte[] ImageToStream(string fileName)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                Bitmap image = new Bitmap(fileName);
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch
            {

            }

            return stream.ToArray();
        }

        //KeyDown Event...

        private void Register_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtFullName.Text != "")
                {
                    //Get Date & Time of Registration...

                    RegDate = DateTime.Now.ToString();

                    if (filepath == "No Path")
                    {
                    //if no Photo...

                    tryagain1:
                        try
                        {
                            connectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                            cnn = new SqlConnection(connectionString);

                            cnn.Open();

                            SqlCommand cmd = new SqlCommand("insert into tbl_EmployeeData (FullName, NICNo, DOB, Address, HomePhone, MobilePhone, Comments, RegisteredOn) values (@FullName, @NICNo, @DOB, @Address, @HomePhone, @MobilePhone, @Comments, @RegisteredOn)", cnn);
                            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                            cmd.Parameters.AddWithValue("@NICNo", txtNICNo.Text);
                            cmd.Parameters.AddWithValue("@DOB", dtpDOB.Value);
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@HomePhone", txtHomePhone.Text);
                            cmd.Parameters.AddWithValue("@MobilePhone", txtMobilePhone.Text);
                            cmd.Parameters.AddWithValue("@Comments", txtComments.Text);
                            cmd.Parameters.AddWithValue("@RegisteredOn", RegDate);
                            cmd.ExecuteNonQuery();

                            cnn.Close();
                        }
                        catch
                        {
                            goto tryagain1;
                        }

                        //Done...

                        MessageBox.Show(txtFullName.Text + " was registered successfully at " + RegDate + "!");

                        Signin_and_Select_Task nf = new Signin_and_Select_Task();
                        nf.Show();

                        Close();
                    }
                    else
                    {
                        //Check Photo...

                        if (File.Exists(filepath))
                        {
                        tryagain2:
                            try
                            {
                                connectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                                cnn = new SqlConnection(connectionString);

                                byte[] content = ImageToStream(filepath);
                                cnn.Open();

                                SqlCommand cmd = new SqlCommand("insert into tbl_EmployeeData (Photo, FullName, NICNo, DOB, Address, HomePhone, MobilePhone, Comments, RegisteredOn) values (@Photo, @FullName, @NICNo, @DOB, @Address, @HomePhone, @MobilePhone, @Comments, @RegisteredOn)", cnn);
                                cmd.Parameters.AddWithValue("@Photo", content);
                                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                                cmd.Parameters.AddWithValue("@NICNo", txtNICNo.Text);
                                cmd.Parameters.AddWithValue("@DOB", dtpDOB.Value);
                                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                                cmd.Parameters.AddWithValue("@HomePhone", txtHomePhone.Text);
                                cmd.Parameters.AddWithValue("@MobilePhone", txtMobilePhone.Text);
                                cmd.Parameters.AddWithValue("@Comments", txtComments.Text);
                                cmd.Parameters.AddWithValue("@RegisteredOn", RegDate);
                                cmd.ExecuteNonQuery();

                                cnn.Close();
                            }
                            catch
                            {
                                goto tryagain2;
                            }

                            //Done...

                            MessageBox.Show(txtFullName.Text + " was registered successfully at " + RegDate + "!");

                            Signin_and_Select_Task nf = new Signin_and_Select_Task();
                            nf.Show();

                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Photo can't be found.");

                            Register nf = new Register();
                            nf.Show();

                            Close();
                        }
                    }
                }
            }
        }

        //Loading...

        private void Register_Load(object sender, EventArgs e)
        {
            pbMinimizeM.Visible = false;
            pbCloseM.Visible = false;

            pbPhoto.Visible = false;
            pbAddPhotoM.Visible = false;
            pbChangePhoto.Visible = false;
            pbChangePhotoM.Visible = false;

            pbRegister.Visible = false;
            pbRegisterM.Visible = false;

            //Fade in Effect...

            timFade.Enabled = true;

            /* Set Form's Transperancy 100 % */
            this.Opacity = 0;

            /* Start the Timer To Animate Form */
            timFade.Enabled = true;

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

            //No of employees...

            if (Convert.ToInt16(RC) == 0)
            {
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
            MessageBoxContent = "You haven't registered any employee. Are you sure you want to exit?";

            DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        //Add Photo Button...

        private void pbAddPhoto_MouseEnter(object sender, EventArgs e)
        {
            pbAddPhotoM.Visible = true;
        }

        private void pbAddPhotoM_MouseLeave(object sender, EventArgs e)
        {
            pbAddPhotoM.Visible = false;
        }

        private void pbAddPhotoM_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        //OpenFileDialogBox...

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            filepath = openFileDialog1.FileName;

            pbPhoto.Image = Image.FromFile(filepath);

            pbPhoto.Visible = true;

            pbChangePhoto.Visible = true;
        }

        //ChangePhoto Button...

        private void pbChangePhoto_MouseEnter(object sender, EventArgs e)
        {
            pbChangePhotoM.Visible = true;
        }

        private void pbChangePhotoM_MouseLeave(object sender, EventArgs e)
        {
            pbChangePhotoM.Visible = false;
        }

        private void pbChangePhotoM_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        //txtFullName...

        private void txtFullName_TextChanged(object sender, EventArgs e)
        {
            if (txtFullName.Text == "")
            {
                pbRegister.Visible = false;
            }
            else
            {
                pbRegister.Visible = true;
            }
        }

        //Register Button...

        private void pbRegister_MouseEnter(object sender, EventArgs e)
        {
            pbRegisterM.Visible = true;
        }

        private void pbRegisterM_MouseLeave(object sender, EventArgs e)
        {
            pbRegisterM.Visible = false;
        }

        private void pbRegisterM_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            string sql = "SELECT * FROM tbl_EmployeeData WHERE FullName = '" + txtFullName.Text + "'";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "Table1");
            connection.Close();

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBoxContent = "Are you sure you want to save these details?";

                DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //Get Date & Time of Registration...

                    RegDate = DateTime.Now.ToString();

                    if (filepath == "No Path")
                    {
                    //if no Photo...

                    tryagain1:
                        try
                        {
                            connectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                            cnn = new SqlConnection(connectionString);

                            cnn.Open();

                            SqlCommand cmd = new SqlCommand("insert into tbl_EmployeeData (FullName, NICNo, DOB, Address, HomePhone, MobilePhone, Comments, RegisteredOn) values (@FullName, @NICNo, @DOB, @Address, @HomePhone, @MobilePhone, @Comments, @RegisteredOn)", cnn);
                            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                            cmd.Parameters.AddWithValue("@NICNo", txtNICNo.Text);
                            cmd.Parameters.AddWithValue("@DOB", dtpDOB.Value);
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@HomePhone", txtHomePhone.Text);
                            cmd.Parameters.AddWithValue("@MobilePhone", txtMobilePhone.Text);
                            cmd.Parameters.AddWithValue("@Comments", txtComments.Text);
                            cmd.Parameters.AddWithValue("@RegisteredOn", RegDate);
                            cmd.ExecuteNonQuery();

                            cnn.Close();
                        }
                        catch (Exception ex)
                        {
                            goto tryagain1;
                        }

                        //Done...

                        MessageBox.Show(txtFullName.Text + " was registered successfully at " + RegDate + "!");

                        Close();

                        Signin_and_Select_Task nf = new Signin_and_Select_Task();
                        nf.Show();
                    }
                    else
                    {
                        //Check Photo...

                        if (File.Exists(filepath))
                        {
                        tryagain2:
                            try
                            {
                                connectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                                cnn = new SqlConnection(connectionString);

                                byte[] content = ImageToStream(filepath);
                                cnn.Open();

                                SqlCommand cmd = new SqlCommand("insert into tbl_EmployeeData (Photo, FullName, NICNo, DOB, Address, HomePhone, MobilePhone, Comments, RegisteredOn) values (@Photo, @FullName, @NICNo, @DOB, @Address, @HomePhone, @MobilePhone, @Comments, @RegisteredOn)", cnn);
                                cmd.Parameters.AddWithValue("@Photo", content);
                                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                                cmd.Parameters.AddWithValue("@NICNo", txtNICNo.Text);
                                cmd.Parameters.AddWithValue("@DOB", dtpDOB.Value);
                                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                                cmd.Parameters.AddWithValue("@HomePhone", txtHomePhone.Text);
                                cmd.Parameters.AddWithValue("@MobilePhone", txtMobilePhone.Text);
                                cmd.Parameters.AddWithValue("@Comments", txtComments.Text);
                                cmd.Parameters.AddWithValue("@RegisteredOn", RegDate);
                                cmd.ExecuteNonQuery();

                                cnn.Close();
                            }
                            catch
                            {
                                goto tryagain2;
                            }

                            //Done...

                            MessageBox.Show(txtFullName.Text + " was registered successfully at " + RegDate + "!");

                            Signin_and_Select_Task nf = new Signin_and_Select_Task();
                            nf.Show();

                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Photo can't be found.");

                            Close();

                            Register nf = new Register();
                            nf.Show();
                        }
                    }
                }
                else if (dialogResult == DialogResult.No)
                {

                }    
            }
            else
            {
                MessageBox.Show("An employee with the name " + txtFullName.Text + " is allready registered. Please check the fullname and try again");
            }        
        }

        //Switch to link...

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBoxContent = "You haven't registered any employee. Are you sure you want to exit?";

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
