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
using System.Configuration;

namespace EmployeeRegistration
{
    public partial class View_All : Form
    {
        SqlConnection cnnPhoto;
        string connectionStringPhoto;

        public View_All()
        {
            InitializeComponent();
        }

        //Initialize Variables...

        string a, b, c, d, f, g, RC, PhotoNumber;
        string MessageBoxContent;
        string MessageBoxTitle = "Confirm your action.";

        public string GetConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["EmployeeRegistration.Properties.Settings.Database1ConnectionString"].ConnectionString;
        }

        //ExecuteUpdateVa...

        private void ExecuteUpdateVa(string Value_a)
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            string sql = "UPDATE tbl_Data SET Value_a = @Value_a WHERE (ID = 1)";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlParameter[] param = new SqlParameter[1];
                //param[0] = new SqlParameter("@id", SqlDbType.Int, 20);
                param[0] = new SqlParameter("@Value_a", SqlDbType.Int, 20);

                param[0].Value = Value_a;

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

        //ExecuteUpdatePN...

        private void ExecuteUpdatePN(string PhotoNumber)
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
            int initialWidth = 140;
            int endWidth = 150;
            int initialheight = 140;
            int endheight = 150;

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

        private void View_All_Load(object sender, EventArgs e)
        {
            pbCloseM.Visible = false;
            pbMinimizeM.Visible = false;

            pbBackM.Visible = false;
            pbNextM.Visible = false;

            //Hide PictureBoxes...

            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;

            // Hide Textboxes...

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;

            //Fade in Effect...

            timFade.Enabled = true;

            /* Set Form's Transperancy 100 % */
            this.Opacity = 0;

            /* Start the Timer To Animate Form */
            timFade.Enabled = true;

            //Call the method to resize picturebox...

            AddAnimation(pictureBox1);
            AddAnimation(pictureBox2);
            AddAnimation(pictureBox3);
            AddAnimation(pictureBox4);
            AddAnimation(pictureBox5);
            AddAnimation(pictureBox6);

            //Get Value of a...

            string connetionStringVa = null;
            SqlConnection connectionVa;
            SqlCommand commandVa;
            SqlDataAdapter adapterVa = new SqlDataAdapter();
            DataSet dsVa = new DataSet();
            string sqlVa = null;

            connetionStringVa = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
            sqlVa = "select ID, Value_a from tbl_Data";

            connectionVa = new SqlConnection(connetionStringVa);

            tryagainVa:
            try
            {
                connectionVa.Open();
                commandVa = new SqlCommand(sqlVa, connectionVa);
                adapterVa.SelectCommand = commandVa;
                adapterVa.Fill(dsVa);
                adapterVa.Dispose();
                commandVa.Dispose();
                connectionVa.Close();

                listBox1.DataSource = dsVa.Tables[0];
                listBox1.ValueMember = "ID";
                listBox1.DisplayMember = "Value_a";

            }
            catch
            {
                goto tryagainVa;
            }

            a = listBox1.Text;

            if (a == "1")
            {
                pbBack.Visible = false;
            }

            b = Convert.ToString(Convert.ToInt16(a) + 1);
            c = Convert.ToString(Convert.ToInt16(a) + 2);
            d = Convert.ToString(Convert.ToInt16(a) + 3);
            f = Convert.ToString(Convert.ToInt16(a) + 4);
            g = Convert.ToString(Convert.ToInt16(a) + 5);

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

            //txtNo...

            if (a == "1")
            {
                int allp;

                if ((Convert.ToInt16(RC) % 6) != 0)
                {
                    allp = (Convert.ToInt16(RC) / 6) + 1;
                }
                else
                {
                    allp = Convert.ToInt16(RC) / 6;
                }

                txtNo.Text = "First page of " + allp.ToString() + " Pages";
            }
            else
            {
                int allp, currentp;

                currentp = (Convert.ToInt16(a) / 6) + 1;

                if ((Convert.ToInt16(RC) % 6) != 0)
                {
                    allp = (Convert.ToInt16(RC) / 6) + 1;
                }
                else
                {
                    allp = Convert.ToInt16(RC) / 6;
                }

                txtNo.Text = "Page " + currentp.ToString() + " of " + allp.ToString() + " pages.";
            }

            //if no records...

            if (Convert.ToInt16(RC) == 0)
            {
                txtRC.Text = "No employee has been registered yet.";
            }
            else
            {
                txtRC.Text = RC + " employees have been registered.";

                if (Convert.ToInt16(RC) <= Convert.ToInt16(g))
                {
                    pbNext.Visible = false;
                }

                //Get FullName(s)...

                string connetionStringFN = null;
                SqlConnection connectionFN;
                SqlCommand commandFN;
                SqlDataAdapter adapterFN = new SqlDataAdapter();
                DataSet dsFN = new DataSet();
                string sqlFN = null;

                connetionStringFN = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                sqlFN = "select ID, FullName from tbl_EmployeeData";

                connectionFN = new SqlConnection(connetionStringFN);

            tryagainFN:
                try
                {
                    connectionFN.Open();
                    commandFN = new SqlCommand(sqlFN, connectionFN);
                    adapterFN.SelectCommand = commandFN;
                    adapterFN.Fill(dsFN);
                    adapterFN.Dispose();
                    commandFN.Dispose();
                    connectionFN.Close();

                    listBox1.DataSource = dsFN.Tables[0];
                    listBox1.ValueMember = "ID";
                    listBox1.DisplayMember = "FullName";

                }
                catch (Exception ex)
                {
                    goto tryagainFN;
                }

                listBox1.SelectedIndex = Convert.ToInt16(a) - 1;
                textBox1.Text = listBox1.Text;
                textBox1.Visible = true;
                pictureBox1.Visible = true;
                try
                {
                    connectionStringPhoto = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;User Instance=True";
                    cnnPhoto = new SqlConnection(connectionStringPhoto);

                    MemoryStream stream1 = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand command1 = new SqlCommand("select Photo from tbl_EmployeeData where ID = @ID", cnnPhoto);
                    command1.Parameters.AddWithValue("@ID", a);
                    byte[] image1 = (byte[])command1.ExecuteScalar();
                    stream1.Write(image1, 0, image1.Length);
                    cnnPhoto.Close();
                    Bitmap bitmap1 = new Bitmap(stream1);
                    pictureBox1.Image = bitmap1;
                }
                catch
                {
                    cnnPhoto.Close();
                    pictureBox1.Image = pbPhoto.Image;
                }

                try
                {
                    listBox1.SelectedIndex = Convert.ToInt16(b) - 1;
                    textBox2.Text = listBox1.Text;
                    textBox2.Visible = true;
                    pictureBox2.Visible = true;

                    listBox1.SelectedIndex = Convert.ToInt16(c) - 1;
                    textBox3.Text = listBox1.Text;
                    textBox3.Visible = true;
                    pictureBox3.Visible = true;

                    listBox1.SelectedIndex = Convert.ToInt16(d) - 1;
                    textBox4.Text = listBox1.Text;
                    textBox4.Visible = true;
                    pictureBox4.Visible = true;

                    listBox1.SelectedIndex = Convert.ToInt16(f) - 1;
                    textBox5.Text = listBox1.Text;
                    textBox5.Visible = true;
                    pictureBox5.Visible = true;

                    listBox1.SelectedIndex = Convert.ToInt16(g) - 1;
                    textBox6.Text = listBox1.Text;
                    textBox6.Visible = true;
                    pictureBox6.Visible = true;
                }
                catch
                {

                }

                //Get Photo(s)...

                try
                {
                    MemoryStream stream2 = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand command2 = new SqlCommand("select Photo from tbl_EmployeeData where ID = @ID", cnnPhoto);
                    command2.Parameters.AddWithValue("@ID", b);
                    byte[] image2 = (byte[])command2.ExecuteScalar();
                    stream2.Write(image2, 0, image2.Length);
                    cnnPhoto.Close();
                    Bitmap bitmap2 = new Bitmap(stream2);
                    pictureBox2.Image = bitmap2;
                }
                catch (Exception ex)
                {
                    cnnPhoto.Close();
                    pictureBox2.Image = pbPhoto.Image;
                }

                try
                {
                    MemoryStream stream3 = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand command3 = new SqlCommand("select Photo from tbl_EmployeeData where ID = @ID", cnnPhoto);
                    command3.Parameters.AddWithValue("@ID", c);
                    byte[] image3 = (byte[])command3.ExecuteScalar();
                    stream3.Write(image3, 0, image3.Length);
                    cnnPhoto.Close();
                    Bitmap bitmap3 = new Bitmap(stream3);
                    pictureBox3.Image = bitmap3;
                }
                catch (Exception ex)
                {
                    cnnPhoto.Close();
                    pictureBox3.Image = pbPhoto.Image;
                }

                try
                {
                    MemoryStream stream4 = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand command4 = new SqlCommand("select Photo from tbl_EmployeeData where ID = @ID", cnnPhoto);
                    command4.Parameters.AddWithValue("@ID", d);
                    byte[] image4 = (byte[])command4.ExecuteScalar();
                    stream4.Write(image4, 0, image4.Length);
                    cnnPhoto.Close();
                    Bitmap bitmap4 = new Bitmap(stream4);
                    pictureBox4.Image = bitmap4;
                }
                catch (Exception ex)
                {
                    cnnPhoto.Close();
                    pictureBox4.Image = pbPhoto.Image;
                }

                try
                {
                    MemoryStream stream5 = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand command5 = new SqlCommand("select Photo from tbl_EmployeeData where ID = @ID", cnnPhoto);
                    command5.Parameters.AddWithValue("@ID", f);
                    byte[] image5 = (byte[])command5.ExecuteScalar();
                    stream5.Write(image5, 0, image5.Length);
                    cnnPhoto.Close();
                    Bitmap bitmap5 = new Bitmap(stream5);
                    pictureBox5.Image = bitmap5;
                }
                catch (Exception ex)
                {
                    cnnPhoto.Close();
                    pictureBox5.Image = pbPhoto.Image;
                }

                try
                {
                    MemoryStream stream6 = new MemoryStream();
                    cnnPhoto.Open();
                    SqlCommand command6 = new SqlCommand("select Photo from tbl_EmployeeData where ID = @ID", cnnPhoto);
                    command6.Parameters.AddWithValue("@ID", g);
                    byte[] image6 = (byte[])command6.ExecuteScalar();
                    stream6.Write(image6, 0, image6.Length);
                    cnnPhoto.Close();
                    Bitmap bitmap6 = new Bitmap(stream6);
                    pictureBox6.Image = bitmap6;
                }
                catch
                {
                    cnnPhoto.Close();
                    pictureBox6.Image = pbPhoto.Image;
                }
            }

            if (Convert.ToInt16(RC) == 1)
            {
                txtRC.Text = "1 employee has been regisetered.";
            }

            //Display empty for blank textboxes...

            if (textBox1.Text == "")
            {
                textBox1.Text = "empty";
            }
            if (textBox2.Text == "")
            {
                textBox2.Text = "empty";
            }
            if (textBox3.Text == "")
            {
                textBox3.Text = "empty";
            }
            if (textBox4.Text == "")
            {
                textBox4.Text = "empty";
            }
            if (textBox5.Text == "")
            {
                textBox5.Text = "empty";
            }
            if (textBox6.Text == "")
            {
                textBox6.Text = "empty";
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

        //Photo Click...

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PhotoNumber = a;
            tryagainPhotoNumber:
            try
            {
                ExecuteUpdatePN(PhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainPhotoNumber;
            }
            //Next Form...

            Details nf = new Details();
            nf.Show();

            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PhotoNumber = b;
        tryagainPhotoNumber:
            try
            {
                ExecuteUpdatePN(PhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainPhotoNumber;
            }

            //Next Form...

            Details nf = new Details();
            nf.Show();

            Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PhotoNumber = c;
        tryagainPhotoNumber:
            try
            {
                ExecuteUpdatePN(PhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainPhotoNumber;
            }

            //Next Form...

            Details nf = new Details();
            nf.Show();

            Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            PhotoNumber = d;
        tryagainPhotoNumber:
            try
            {
                ExecuteUpdatePN(PhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainPhotoNumber;
            }

            //Next Form...

            Details nf = new Details();
            nf.Show();

            Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            PhotoNumber = f;
        tryagainPhotoNumber:
            try
            {
                ExecuteUpdatePN(PhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainPhotoNumber;
            }

            //Next Form...

            Details nf = new Details();
            nf.Show();

            Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            PhotoNumber = g;
        tryagainPhotoNumber:
            try
            {
                ExecuteUpdatePN(PhotoNumber);
            }
            catch (Exception ex)
            {
                goto tryagainPhotoNumber;
            }

            //Next Form...

            Details nf = new Details();
            nf.Show();

            Close();
        }

        //Next Button...

        private void pbNext_MouseEnter(object sender, EventArgs e)
        {
            pbNextM.Visible = true;
        }

        private void pbNextM_MouseLeave(object sender, EventArgs e)
        {
            pbNextM.Visible = false;
        }

        private void pbNextM_Click(object sender, EventArgs e)
        {
            a = Convert.ToString(Convert.ToInt16(a) + 6);

            ExecuteUpdateVa(a);

            Close();

            View_All nf = new View_All();
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
            a = Convert.ToString(Convert.ToInt16(a) - 6);

            ExecuteUpdateVa(a);

            Close();

            View_All nf = new View_All();
            nf.Show();
        }

        //Switch to link...

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Signin_and_Select_Task nf = new Signin_and_Select_Task();
            nf.Show();

            Close();
        }        
    }
}
