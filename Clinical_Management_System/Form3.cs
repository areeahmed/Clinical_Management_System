using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace Clinical_Management_System
{
    public partial class appointment : Form
    {

        // Open Form Panel 
        bool isFormOpened = false;

        // Open Barcode Read and Show Panel
        bool isBarcodeOpen = false;
        // Open Sidebar FlowLayoutPanel
        bool sideBarExpand = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();

        // check if capture device opened or not
        bool capDev = false;

        // choosing the camera and capturing
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice CaptureDevice;
        public appointment(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void appointment_Load(object sender, EventArgs e)
        {
            token_add_user_btn.Enabled = false;

            dateTimeTimer.Start();
            Adding_Doctor_Form_panel.Visible = false;
            doctor_barcode_panel.Visible = false;


            qr_code_is_active_lbl.Visible = false;
            // for reading barcode
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                comboBox3.Items.Add(filterInfo.Name);
                comboBox3.SelectedIndex = 0;
            }

            ControlExtension.Draggable(doctor_barcode_panel, true);
            ControlExtension.Draggable(Adding_Doctor_Form_panel, true);
        }

        private void barcodeDocBtn_Click(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            admin_read_QR_pic.Image = null;
                openQRTimer.Start();
                doctor_barcode_panel.Visible = true;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_QR_pic.Image = null;
                CaptureDevice.Stop();
                capDev = false;
            }
            if (!capDev)
            {
                qr_code_is_active_lbl.Visible = true;
                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox3.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                run_cam_qr_timer.Start();
                capDev = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            admin_read_QR_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            openQRTimer.Start();
            doctor_barcode_panel.Visible = true;
        }


        private void run_cam_qr_timer_Tick(object sender, EventArgs e)
        {
            if (admin_read_QR_pic.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)admin_read_QR_pic.Image);
                if (result != null)
                {
                    token_search_txt.Text = result.ToString();
                    docIDtxt.Text = result.ToString();
                    CaptureDevice.Stop();
                    openQRTimer.Start();
                    run_cam_qr_timer.Stop();
                    admin_read_QR_pic.Image = null;
                    CaptureDevice.Start();
                    if(!isFormOpened)
                    {
                        openFormTimer.Start();
                    }
                }
            }
        }

        /// <summary>
        /// this part of code will get the result from qr code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            admin_read_QR_pic.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void openQRTimer_Tick(object sender, EventArgs e)
        {
            if (isBarcodeOpen)
            {

                doctor_barcode_panel.Height -= 30;
                if (doctor_barcode_panel.Height == doctor_barcode_panel.MinimumSize.Height)
                {
                    isBarcodeOpen = false;
                    doctor_barcode_panel.Visible = false;
                    openQRTimer.Stop();
                }
            }
            else
            {
                doctor_barcode_panel.Visible = true;
                doctor_barcode_panel.Height += 30;
                if (doctor_barcode_panel.Height == doctor_barcode_panel.MaximumSize.Height)
                {
                    isBarcodeOpen = true;

                    openQRTimer.Stop();
                }
            }
        }

        private void openFormTimer_Tick(object sender, EventArgs e)
        {
            if (isFormOpened)
            {

                Adding_Doctor_Form_panel.Height -= 30;
                if (Adding_Doctor_Form_panel.Height == Adding_Doctor_Form_panel.MinimumSize.Height)
                {
                    isFormOpened = false;
                    Adding_Doctor_Form_panel.Visible = false;
                    openFormTimer.Stop();
                }
            }
            else
            {
                Adding_Doctor_Form_panel.Visible = true;
                Adding_Doctor_Form_panel.Height += 30;
                if (Adding_Doctor_Form_panel.Height == Adding_Doctor_Form_panel.MaximumSize.Height)
                {
                    isFormOpened = true;
                    openFormTimer.Stop();
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Application.Exit();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
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

        private void editDocBtn_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
        }

        private void cancel_Form_Button_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

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

        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            patient patient = new patient(this.WindowState);
            patient.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            login_Form.Show();
            this.Hide();
        }

        private void add_new_user_chk_CheckedChanged(object sender, EventArgs e)
        {
            if(add_new_user_chk.Checked)
            {
                token_add_user_btn.Enabled = true;
                token_edit_user_btn.Enabled = false;
            }
            else
            {
                token_add_user_btn.Enabled = false;
                token_edit_user_btn.Enabled = true;
            }
        }
    }
}
