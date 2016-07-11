using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace EmployeeRegistration
{
    public partial class Details : Form
    {
        SqlConnection cnnPhoto;
        string connectionstringPhoto;

        public Details()
        {
            InitializeComponent();
        }

        //Initialize Variables...

        string RC, PhotoNumber, PPhotoNumber, NPhotoNumber;
        string MessageBoxContent;
        string MessageBoxTitle = "Confirm your action.";

        public string GetConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["EmployeeRegistration.Properties.Settings.Database1ConnectionString"].ConnectionString;
        }

        private void ExecuteUpdate(string PhotoNumber)
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            string sql = "UPDATE tbl_Data SET PhotoNumber = @PhotoNumber WHERE (ID = 1)";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlParameter[] param = new SqlParameter[1];
                //param[0] = new SqlParameter("@id", SqlDbType.Int, 20);
                param[0] = new SqlParameter("@PhotoNumber", SqlDbType.Int, 20);

                param[0].Value = PhotoNumber;

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

        //Photo Resizing...

        public void AddAnimation(PictureBox picturebox)
        {
            var expandTimer = new System.Windows.Forms.Timer();
            var contractTimer = new System.Windows.Forms.Timer();

            expandTimer.Interval = 10;//can adjust to determine the refresh rate
            contractTimer.Interval = 10;

            DateTime animationStarted = DateTime.Now;

            //TODO update as appropriate or make it a parameter
            TimeSpan animationDuration = TimeSpan.FromMilliseconds(250);
            int initialWidth = 100;
            int endWidth = 120;
            int initialheight = 100;
            int endheight = 120;

            int startx, starty;
            startx = picturebox.Location.X;
            starty = picturebox.Location.Y;

            picturebox.MouseHover += (_, args) =>
            {
                contractTimer.Stop();
                expandTimer.Start();
                animationStarted = DateTime.Now;

                picturebox.Location = new Point(startx - 5, starty - 5);
            };

            picturebox.MouseLeave += (_, args) =>
            {
                expandTimer.Stop();
                contractTimer.Start();
                animationStarted = DateTime.Now;

                picturebox.Location = new Point(startx, starty);
            };

            expandTimer.Tick += (_, args) =>
            {
                double percentComplete = (DateTime.Now - animationStarted).Ticks
                    / (double)animationDuration.Ticks;

                if (percentComplete >= 1)
                {
                    expandTimer.Stop();
                }
                else
                {
                    picturebox.Width = (int)(initialWidth + (endWidth - initialWidth) * percentComplete);

                    picturebox.Height = (int)(initialheight + (endheight - initialheight) * percentComplete);
                }
            };

            contractTimer.Tick += (_, args) =>
            {
                double percentComplete = (DateTime.Now - animationStarted).Ticks
                    / (double)animationDuration.Ticks;

                if (percentComplete >= 1)
                {
                    contractTimer.Stop();
                }
                else
                {
                    picturebox.Width = (int)(endWidth - (endWidth - initialWidth) * percentComplete);

                    picturebox.Height = (int)(endheight - (endheight - initialheight) * percentComplete);
                }
            };
        }

        //Loading...

        private void Details_Load(object sender, EventArgs e)
        {
            pbCloseM.Visible = false;
            PbMinimizeM.Visible = false;
            pictureBox1.Visible = false;
            listBox1.Visible = false;

            pbBackM.Visible = false;

            //Fade in Effect...

            timFade.Enabled = true;

            /* Set Form's Transperancy 100 % */
            this.Opacity = 0;

            /* Start the Timer To Animate Form */
            timFade.Enabled = true;

            //Call the method to resize picturebox...

            AddAnimation(pbPhotoP);
            AddAnimation(pbPhotoN);

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
            }
            catch
            {
                goto tryagainRC;
            }

            RC = Convert.ToString(dsRC.Tables[0].Rows.Count);

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

            //Get PhotoNumber...

            string connetionStringPhotoNumber = null;
            SqlConnection connectionPhotoNumber;
            SqlCommand commandPhotoNumber;
            SqlDataAdapter adapterPhotoNumber = new SqlDataAdapter();
            DataSet dsPhotoNumber = new DataSet();
            string sqlPhotoNumber = null;

            connetionStringPhotoNumber = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            sqlPhotoNumber = "select ID, PhotoNumber from tbl_Data";

            connectionPhotoNumber = new SqlConnection(connetionStringPhotoNumber);
tryagainPhotoNumber:
            try
            {
                connectionPhotoNumber.Open();
                commandPhotoNumber = new SqlCommand(sqlPhotoNumber, connectionPhotoNumber);
                adapterPhotoNumber.SelectCommand = commandPhotoNumber;
                adapterPhotoNumber.Fill(dsPhotoNumber);
                adapterPhotoNumber.Dispose();
                commandPhotoNumber.Dispose();
                connectionPhotoNumber.Close();

                listBox1.DataSource = dsPhotoNumber.Tables[0];
                listBox1.ValueMember = "ID";
                listBox1.DisplayMember = "PhotoNumber";

            }
            catch (Exception ex)
            {
                goto tryagainPhotoNumber;
            }

            PhotoNumber = listBox1.Text;

            PPhotoNumber = Convert.ToString(Convert.ToInt16(PhotoNumber) - 1);
            NPhotoNumber = Convert.ToString(Convert.ToInt16(PhotoNumber) + 1);

            //Loading Photo...

            connectionstringPhoto = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            cnnPhoto = new SqlConnection(connectionstringPhoto);

            try
            {
                MemoryStream streamPhoto = new MemoryStream();
                cnnPhoto.Open();
                SqlCommand commandPhoto = new SqlCommand("select Photo from tbl_EmployeeData where (ID = @ID)", cnnPhoto);
                commandPhoto.Parameters.AddWithValue("ID", PhotoNumber);
                byte[] imagePhoto = (byte[])commandPhoto.ExecuteScalar();
                streamPhoto.Write(imagePhoto, 0, imagePhoto.Length);
                cnnPhoto.Close();
                Bitmap bitmapPhoto = new Bitmap(streamPhoto);
                pbPhoto.Image = bitmapPhoto;
            }
            catch (Exception ex)
            {
                cnnPhoto.Close();
                pbPhoto.Image = pictureBox1.Image;
            }

            //Loading Previous Photo...

            if (Convert.ToInt16(PhotoNumber) == 1)
            {
                pbPhotoP.Visible = false;
            }
            else
            {
                try
                {
                    MemoryStream streamPPhoto = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand commandPPhoto = new SqlCommand("select Photo from tbl_EmployeeData where (ID = @ID)", cnnPhoto);
                    commandPPhoto.Parameters.AddWithValue("ID", PPhotoNumber);
                    byte[] imagePPhoto = (byte[])commandPPhoto.ExecuteScalar();
                    streamPPhoto.Write(imagePPhoto, 0, imagePPhoto.Length);
                    cnnPhoto.Close();
                    Bitmap bitmapPPhoto = new Bitmap(streamPPhoto);
                    pbPhotoP.Image = bitmapPPhoto;
                }
                catch (Exception ex)
                {
                    cnnPhoto.Close();
                    pbPhotoP.Image = pictureBox1.Image;
                }
            }

            //Loading Next Photo...

            if (Convert.ToInt16(PhotoNumber) < Convert.ToInt16(RC))
            {
                try
                {
                    MemoryStream streamNPhoto = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand commandNPhoto = new SqlCommand("select Photo from tbl_EmployeeData where (ID = @ID)", cnnPhoto);
                    commandNPhoto.Parameters.AddWithValue("ID", NPhotoNumber);
                    byte[] imageNPhoto = (byte[])commandNPhoto.ExecuteScalar();
                    streamNPhoto.Write(imageNPhoto, 0, imageNPhoto.Length);
                    cnnPhoto.Close();
                    Bitmap bitmapNPhoto = new Bitmap(streamNPhoto);
                    pbPhotoN.Image = bitmapNPhoto;
                }
                catch
                {
                    cnnPhoto.Close();
                    pbPhotoN.Image = pictureBox1.Image;
                }
            }
            else
            {
                pbPhotoN.Visible = false;
            }

            //Get Details...

            string connetionStringDetails = null;
            SqlConnection connectionDetails;
            SqlCommand commandDetails;
            SqlDataAdapter adapterDetails = new SqlDataAdapter();
            DataSet dsDetails = new DataSet();
            string sqlDetails = null;

            connetionStringDetails = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            sqlDetails = "select * from tbl_EmployeeData";

            connectionDetails = new SqlConnection(connetionStringDetails);

        tryagainDetails:
            try
            {
                connectionDetails.Open();
                commandDetails = new SqlCommand(sqlDetails, connectionDetails);
                adapterDetails.SelectCommand = commandDetails;
                adapterDetails.Fill(dsDetails);
                adapterDetails.Dispose();
                commandDetails.Dispose();
                connectionDetails.Close();

                listBox1.DataSource = dsDetails.Tables[0];
                listBox1.ValueMember = "ID";

                listBox1.DisplayMember = "FullName";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtFullName.Text = listBox1.Text;
                listBox1.DisplayMember = "NICNo";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtNICNo.Text = listBox1.Text;
                listBox1.DisplayMember = "DOB";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtDOB.Text = listBox1.Text;
                listBox1.DisplayMember = "Address";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtAddress.Text = listBox1.Text;
                listBox1.DisplayMember = "HomePhone";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtHomePhone.Text = listBox1.Text;
                listBox1.DisplayMember = "MobilePhone";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtMobilePhone.Text = listBox1.Text;
                listBox1.DisplayMember = "Comments";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtComments.Text = listBox1.Text;
                listBox1.DisplayMember = "RegisteredOn";
                listBox1.SelectedIndex = Convert.ToInt16(PPhotoNumber);
                txtRegDate.Text = "Registered on : " + listBox1.Text;

            }
            catch
            {
                goto tryagainDetails;
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

        //Minimize Button...

        private void PbMinimize_MouseEnter(object sender, EventArgs e)
        {
            PbMinimizeM.Visible = true;
        }

        private void PbMinimizeM_MouseLeave(object sender, EventArgs e)
        {
            PbMinimizeM.Visible = false;
        }

        private void PbMinimizeM_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //Previous Photo...

        private void pbPhotoP_MouseEnter(object sender, EventArgs e)
        {
            pbPhotoP.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbPhotoP_MouseLeave(object sender, EventArgs e)
        {
            pbPhotoP.BorderStyle = BorderStyle.None;
        }

        private void pbPhotoP_Click(object sender, EventArgs e)
        {
            //Set Photo Number...

            tryagainPPhotoNumber:
            try
            {
                ExecuteUpdate(PPhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainPPhotoNumber;
            }

            //Restart...

            Close();

            Details nf = new Details();
            nf.Show();
        }

        //Next Photo...

        private void pbPhotoN_MouseEnter(object sender, EventArgs e)
        {
            pbPhotoN.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbPhotoN_MouseLeave(object sender, EventArgs e)
        {
            pbPhotoN.BorderStyle = BorderStyle.None;
        }

        private void pbPhotoN_Click(object sender, EventArgs e)
        {
            //Set Photo Number...

            tryagainNPhotoNumber:
            try
            {
                ExecuteUpdate(NPhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainNPhotoNumber;
            }

            //Restart...

            Close();

            Details nf = new Details();
            nf.Show();
        }

        //Back Button...

        private void pbBack_MouseEnter(object sender, EventArgs e)
        {
            pbBackM.Visible = true;
        }

        private void pbBackM_MouseLeave(object sender, EventArgs e)
        {
            pbBackM.Visible = false;
        }

        private void pbBackM_Click(object sender, EventArgs e)
        {
            Close();

            View_All nf = new View_All();
            nf.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();

            Add_Comment nf = new Add_Comment();
            nf.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();

            Signin_and_Select_Task nf = new Signin_and_Select_Task();
            nf.Show();
        }

        //txtComments mouse click...

        private void txtComments_MouseClick(object sender, MouseEventArgs e)
        {
            Close();

            Add_Comment nf = new Add_Comment();
            nf.Show();
        }       
    }
}
