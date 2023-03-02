using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Clinical_Management_System
{
    public partial class Admin_AdminView : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////
        ///
        /// Variables
        ///
        private Point mouseLocation;
        private bool isMouseDown = false;   
        // Open Sidebar FlowLayoutPanel
        bool sideBarExpand = false;
        // Open Form Panel 
        bool isFormOpened = false;
        // Open Barcode Read and Show Panel
        bool isBarcodeOpen = false;
        // check if capture device opened or not
        bool capDev = false;
        // choosing the camera and capturing
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice CaptureDevice;
        ///
        ///  Variables
        ///
        ///////////////////////////////////////////////////////
        /// 
        /// Form Loads And Other operation START here
        /// 

        /// <summary>
        /// Form Constructor
        /// </summary>
        /// <param name="windowState"> Windows status </param>
        public Admin_AdminView(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }



        // this code below is the connection between app and Database
        // SqlConnection con = new SqlConnection(@"");

        /// <summary>
        /// Form Load
        /// filter info collection here is initilized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_AdminView_Load(object sender, EventArgs e)
        {

            /////////////////////////////////////////

            //DataTable
            //
            //DataTable dt = new DataTable();
            //SqlDataAdapter sda = new SqlDataAdapter("select *from  Hay1", con);
            //sda.Fill(dt);
            //
            // docListDGV.DataSource = dt;
            //
            //
            //
            // <<<<<<  INSERT  >>>>>>>
            /*DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("insert into tableName(field1, field2)values('" + textBox1.Text + "','" + textBox2.Text + "')", con);
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            */
            //
            //
            // <<<<<<  DELETE  >>>>>>>
            /*
             DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("delete from Hay1 where ID ='"+textBox1.Text +"'",con);
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            MessageBox.Show("Delete data");

             */
            //
            //
            // <<<<<<  UPDATE  >>>>>>>
            /*
             DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("Update student set ID ='"+textBox1.Text +"',Name='"+textBox2.Text+"'",con);
            sda.Fill(dt);
            dataGridView1.DataSource = dt;	
            MessageBox.Show("Update data");

             */
            //




            add_user_btn.Enabled = false;
            //////////////////////////////////////////
            qr_code_is_active_lbl.Visible = false;
            // for reading barcode
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                admin_choose_camera_cmb.Items.Add(filterInfo.Name);
                admin_choose_camera_cmb.SelectedIndex = 0;
            }


            // this part of code is for making picture look like circle
            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, admin_prof_pic.Width, admin_prof_pic.Height);
            Region region = new Region(obj);
            admin_prof_pic.Region = region;
            // until here is for profile picture circing

            // invisibling the panels at first
            adding_admin_form_panel.Visible = false;
            admin_barcode_panel.Visible = false;

            // for making it fraggable
            ControlExtension.Draggable(admin_barcode_panel, true);
            ControlExtension.Draggable(adding_admin_form_panel, true);

            // timer for the date time
            dateTimeTimer.Start();
        }

        ///
        ///  Form Movement Code STARTS HERE
        ///

        /// <summary>
        /// this is for moving form by clicking on the form
        /// isMouseDown variable changed to true
        /// mouseLocation will be equal to mouse current location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_AdminView_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseLocation = e.Location;
        }

        /// <summary>
        /// this function also to change the form location
        /// isMouseDown will change the value to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_AdminView_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        /// <summary>
        /// this is also to change the location of form
        /// chacking isMouseDown or not
        /// location of form will be equal to the mouse location
        /// then updating form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_AdminView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - mouseLocation.X) + e.X, (this.Location.Y - mouseLocation.Y) + e.Y);

                this.Update();
            }
        }

        ///
        ///  Form Movement Code ENDS HERE
        ///

        /// 
        /// Form Loads And Other operation ENDS here
        /// 
        /////////////////////////////////////////////////// 
        /// 
        /// Click Operations STARTS here
        /// 

        ///
        ///  Maximize , Minimize , Normal STARTS here
        /// 

        /// <summary>
        /// Application Exit
        /// Exit Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApp_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
            }
            run_cam_qr_timer.Stop();
            Application.Exit();
        }

        /// <summary>
        /// Maximize to Normal , Normal to Maximize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// Windows Status to Minimize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }
        
        ///
        ///  Maximize , Minimize , Normal ENDS here
        ///

        /// <summary>
        /// picture box click even to open sidebare
        /// timmer of sidebare start here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSideBar_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        /// <summary>
        /// Clearing the text box of the form
        /// if user canceled the panel form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Form_Button_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
            admin_id_txt.Clear();
            admin_username_txt.Clear();
            admin_password_txt.Clear();
            admin_fullname_txt.Clear();
            admin_phone_txt.Clear();
            admin_address_txt.Clear();
            adding_admin_form_panel.Visible = true;
        }

        /// <summary>
        /// open form panel of admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editDocBtn_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
            adding_admin_form_panel.Visible = true;
        }

        /// <summary>
        /// There is no problem here
        /// barcode generating from user ID
        /// check if user ID is available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowBarcode_Click(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            admin_read_qr_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            if (admin_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                admin_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr =  Zen.Barcode.BarcodeDrawFactory.CodeQr;
                admin_show_qr_pic.Image = codeQr.Draw(admin_ID.Text, 200);
                barcodeTimer.Start();
                admin_barcode_panel.Visible = true;
            }
        }

        /// <summary>
        /// button function click to open barcode panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenBarcodePanel_Click(object sender, EventArgs e)
        {
            barcodeTimer.Start();
            admin_show_qr_pl.Visible = true;
        }

        /// <summary>
        /// this method used to close capture Device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Barcode_Panel(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            admin_read_qr_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            barcodeTimer.Start();
            admin_barcode_panel.Visible = true;
        }
        ///
        /// Navigation Between forms STARTS here
        ///

        /// <summary>
        /// open patient form by admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPatientForm_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
            }
            Admin_PatientView admin_PatientView = new Admin_PatientView(windowState: this.WindowState);
            admin_PatientView.Show();
            this.Hide();
        }

        /// <summary>
        /// open reciption form by admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenReciptionForm_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
            }
            Admin_ReciptionView admin_ReciptionView = new Admin_ReciptionView(this.WindowState);
            admin_ReciptionView.Show();
            this.Hide();
        }

        /// <summary>
        /// open clinic view by admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenClinicForm_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
            }
            Admin_ClinicView admin_ClinicView = new Admin_ClinicView(this.WindowState);
            admin_ClinicView.Show();
            this.Hide();
        }

        /// <summary>
        /// open Doctor form by admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDoctorForm_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
            }
            Admin_DoctorView doctorForm = new Admin_DoctorView(windowState: this.WindowState);
            doctorForm.Show();
            this.Hide();
        }

        /// <summary>
        /// do not open admin form by admin twice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenAdminForm_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// open Dashboard form by admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDashboardForm_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
            }
            AdminDashboard adminDashboard = new AdminDashboard(windowState: this.WindowState);
            adminDashboard.Show();
            this.Hide();
        }

        ///
        /// Navigation Between forms ENDS here
        ///

        /// <summary>
        /// user profile picture open dialog()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserProfilePictureAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                admin_prof_pic.ImageLocation = openFileDialog1.FileNames[0];
            }
        }

        /// <summary>
        /// Copy Admin Info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyDocBtn_Click(object sender, EventArgs e)
        {

            if (admin_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                admin_QrPic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                admin_QrPic.Image = codeQr.Draw(admin_ID.Text, 200);
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
                
            }
        }

        /// <summary>
        /// this function is the button Show QR Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowQRCode_Click_1(object sender, EventArgs e)
        {
            admin_QrPic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            admin_QrPic.Image = codeQr.Draw(admin_ID.Text, 200);
            admin_show_qr_pl.Visible = true;
            admin_read_qr_pic.Visible = false;
        }

        /// <summary>
        /// i have used this because there was a simple issus with closing my application
        /// it was like the app was still runing in the background
        /// hide and show the 2 panel 
        /// 1st panel is for show qr code
        /// 2nd panel is for read qr code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadQRCode_Click(object sender, EventArgs e)
        {
           
            admin_show_qr_pl.Visible = false;
            admin_read_qr_pic.Visible = true;
                
        }

        /// <summary>
        /// this part of code will get the result from qr code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            admin_read_qr_pic.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        /// 
        /// Click Operations ENDS here
        /// 

        //////////////////////////////////////////////////

        /// 
        /// Ticks of timers STARTS here
        ///


        /// <summary>
        /// this is a ( run_cam_qr ) timmer tick for QR Code capturing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void run_cam_qr_Tick(object sender, EventArgs e)
        {
            if(admin_read_qr_pic.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)admin_read_qr_pic.Image);
                if(result != null)
                {
                    admin_search_txt.Text = result.ToString();
                    CaptureDevice.Stop();
                    barcodeTimer.Start();
                    run_cam_qr_timer.Stop();
                    admin_read_qr_pic.Image = null;
                    CaptureDevice.Start();
                }
            }
        }

        /// <summary>
        /// get date and time from timmer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }
        
        /// <summary>
        /// this is a tick barcode timmer to open barcode form panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barcodeTimer_Tick(object sender, EventArgs e)
        {
            if (isBarcodeOpen)
            {
                qr_code_is_active_lbl.Visible = false;
                admin_barcode_panel.Height -= 30;
                if (admin_barcode_panel.Height == admin_barcode_panel.MinimumSize.Height)
                {
                    isBarcodeOpen = false;
                    barcodeTimer.Stop();
                }
            }
            else
            {
                admin_barcode_panel.Height += 30;
                if (admin_barcode_panel.Height == admin_barcode_panel.MaximumSize.Height)
                {
                    isBarcodeOpen = true;
                    barcodeTimer.Stop();
                }
            }
        }

        /// <summary>
        /// open Form of adding editing Admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFormTimer_Tick(object sender, EventArgs e)
        {
            if (isFormOpened)
            {
                adding_admin_form_panel.Height -= 30;
                if (adding_admin_form_panel.Height == adding_admin_form_panel.MinimumSize.Height)
                {
                    isFormOpened = false;
                    openFormTimer.Stop();
                }
            }
            else
            {
                adding_admin_form_panel.Height += 30;
                if (adding_admin_form_panel.Height == adding_admin_form_panel.MaximumSize.Height)
                {
                    isFormOpened = true;
                    openFormTimer.Stop();
                }
            }
        }

        /// <summary>
        /// Tick of Sizebar timmer to open sidebar
        /// here the width of sidebar will change
        /// the font of the day , datetime , date changed
        /// check if sidebar opened or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sideBarExpand)
            {
                sidebar.Width -= 10;
                time.Font = new Font("RudawRegular", 11);
                WeekDay.Font = new Font("RudawRegular", 11);
                DayWeekYear.Font = new Font("RudawRegular", 11);
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sideBarExpand = false;
                    timer1.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sideBarExpand = true;
                    timer1.Stop();
                    time.Font = new Font("RudawRegular", 20);
                    WeekDay.Font = new Font("RudawRegular", 20);
                    DayWeekYear.Font = new Font("RudawRegular", 20);
                }
            }
        }

        /// <summary>
        /// Design of the Print Preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // this is same for all forms except print_admin_lbl
            e.Graphics.DrawImage(picLogoPrint.Image, 10, 10, 100, 100);
            e.Graphics.DrawString(print_krd_lbl.Text, new Font("RudawRegular", 24), Brushes.Gray, 125, 30);
            e.Graphics.DrawString(print_hl_lbl.Text, new Font("RudawRegular", 20), Brushes.Gray, 125, 60);
            e.Graphics.DrawString(print_cr_lbl.Text, new Font("RudawRegular", 20), Brushes.ForestGreen, 200, 60);
            e.Graphics.DrawString(admin_lbl.Text, new Font("RudawRegular", 25), Brushes.Black, 700, 30);
            e.Graphics.DrawImage(print_pic_op.Image, 180, 300, 500, 500);
            e.Graphics.DrawImage(admin_prof_pic.Image, 320, 150, 200, 200);
            e.Graphics.DrawImage(admin_QrPic.Image, 70, 900, 150, 150);
            // ID
            // ID res
            e.Graphics.DrawString(admin_ID.Text, new Font("RudawRegular", 24), Brushes.Black, 630, 500);
            // ID lbl
            e.Graphics.DrawString(admin_ID_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 500);

            // NAME
            // Name res
            e.Graphics.DrawString(admin_Name.Text, new Font("RudawRegular", 24), Brushes.Black, 480, 550);
            // Name lbl
            e.Graphics.DrawString(admin_Name_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 550);

            // PHONE
            // Phone res
            e.Graphics.DrawString(admin_Phone.Text, new Font("RudawRegular", 24), Brushes.Black, 440, 600);
            // Phone lbl
            e.Graphics.DrawString(admin_Phone_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 600);

            // GENDER
            // Gender res
            e.Graphics.DrawString(admin_Gender.Text, new Font("RudawRegular", 24), Brushes.Black, 630, 650);
            // Gender lbl
            e.Graphics.DrawString(admin_Gender_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 650);

            // ADDRESS
            // address res
            e.Graphics.DrawString(admin_Address.Text, new Font("RudawRegular", 24), Brushes.Black, 450, 700);
            // address lbl
            e.Graphics.DrawString(admin_Address_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 700);

            e.Graphics.DrawString(WeekDay.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 1000);
            e.Graphics.DrawString(time.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 970);
            e.Graphics.DrawString(DayWeekYear.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 1030);
        }

        private void PatientButton_Click(object sender, EventArgs e)
        {

        }


        private void label28_Click(object sender, EventArgs e)
        {
            if (admin_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                admin_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                admin_show_qr_pic.Image = codeQr.Draw(admin_ID.Text, 200);
                barcodeTimer.Start();
                admin_barcode_panel.Visible = true;
            }
        }

        private void start_qr_code_Click(object sender, EventArgs e)
        {
            if(capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
                capDev = false;
            }
            if(!capDev)
            {
                qr_code_is_active_lbl.Visible = true;
                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[admin_choose_camera_cmb.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                run_cam_qr_timer.Start();
                capDev = true;
            }
        }

        // Update Data
        private void edit_user_button_Click(object sender, EventArgs e)
        {
            // <<<<<<  UPDATE SAMPLE CODE >>>>>>>
            /*
             DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("Update student set ID ='"+textBox1.Text +"',Name='"+textBox2.Text+"'",con);
            sda.Fill(dt);
            dataGridView1.DataSource = dt;	
            MessageBox.Show("Update data");

             */
        }

        // Insert Data
        private void Add_user_button_Click(object sender, EventArgs e)
        {
            // <<<<<<  INSERT  >>>>>>>
            /*DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("insert into tableName(field1, field2)values('" + textBox1.Text + "','" + textBox2.Text + "')", con);
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
            */
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(add_new_user_chk.Checked)
            {
                edit_user_btn.Enabled = false;
                add_user_btn.Enabled = true;
            }
            else
            {
                edit_user_btn.Enabled = true;
                add_user_btn.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            login_Form.Show();
            this.Hide();
        }

        ///
        /// 
        /// 
        /// Ticks of timers ENDS here
        /// 
        /// 
        ////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
