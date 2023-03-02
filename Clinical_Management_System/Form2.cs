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
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Clinical_Management_System
{
    public partial class patient: Form
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

        public patient(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }


       

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_QR_pic.Image = null;
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_QR_pic.Image = null;
                CaptureDevice.Stop();
            }
            appointment f3 = new appointment(this.WindowState);
            f3.Show();
            this.Hide();
        }

        private void patient_Load(object sender, EventArgs e)
        {
            pay_add_user_btn.Enabled = false;

            dateTimeTimer.Start();
            qr_code_is_active_lbl.Visible = false;
            // for reading barcode
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                comboBox1.Items.Add(filterInfo.Name);
                comboBox1.SelectedIndex = 0;
            }

            // this part of code is for making picture look like circle
            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, pa_profile_pic.Width, pa_profile_pic.Height);
            Region region = new Region(obj);
            pa_profile_pic.Region = region;
            // until here is for profile picture circing

            doctor_barcode_panel.Visible = false;
            Adding_Doctor_Form_panel.Visible = false;
            ControlExtension.Draggable(doctor_barcode_panel, true);
            ControlExtension.Draggable(Adding_Doctor_Form_panel, true);
        }

        private void pa_profile_pic_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pa_profile_pic.ImageLocation = openFileDialog.FileName;
            }
        }

        // QR Form Timer tick
        private void pay_openQR_Tick(object sender, EventArgs e)
        {
            if(isBarcodeOpen)
            {
                
                doctor_barcode_panel.Height -= 30;
                if(doctor_barcode_panel.Height == doctor_barcode_panel.MinimumSize.Height)
                {
                    isBarcodeOpen = false;
                    doctor_barcode_panel.Visible = false;
                    pay_openQR.Stop();
                }
            }
            else
            {
                doctor_barcode_panel.Visible = true;
                doctor_barcode_panel.Height += 30;
                if (doctor_barcode_panel.Height == doctor_barcode_panel.MaximumSize.Height)
                {
                    isBarcodeOpen = true;
                    
                    pay_openQR.Stop();
                }
            }
        }

        private void editDocBtn_Click(object sender, EventArgs e)
        {
            pay_openForm.Start();
        }

        private void pay_openForm_Tick(object sender, EventArgs e)
        {
            if (isFormOpened)
            {
                
                Adding_Doctor_Form_panel.Height -= 30;
                if (Adding_Doctor_Form_panel.Height == Adding_Doctor_Form_panel.MinimumSize.Height)
                {
                    isFormOpened = false;
                    Adding_Doctor_Form_panel.Visible = false;
                    pay_openForm.Stop();
                }
            }
            else
            {
                Adding_Doctor_Form_panel.Visible = true;
                Adding_Doctor_Form_panel.Height += 30;
                if (Adding_Doctor_Form_panel.Height == Adding_Doctor_Form_panel.MaximumSize.Height)
                {
                    isFormOpened = true;
                    pay_openForm.Stop();
                }
            }
        }

        private void cancel_Form_Button_Click(object sender, EventArgs e)
        {
            pay_openForm.Start();
        }

        private void barcodeDocBtn_Click(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            admin_read_QR_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            if (pay_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                pay_qr_show_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                pay_qr_show_pic.Image = codeQr.Draw(pay_ID.Text, 200);
                pay_openQR.Start();
                doctor_barcode_panel.Visible = true;
            }

            // Generate the QR Code
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            admin_read_QR_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            pay_openQR.Start();
            doctor_barcode_panel.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pay_qr_show_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            pay_qr_show_pic.Image = codeQr.Draw(pay_ID.Text, 200);
            admin_show_qr_pl.Visible = true;
            admin_read_QR_pic.Visible = false;

            // Hide QR Reader
            // Show Generated QR
        }

        private void button5_Click(object sender, EventArgs e)
        {
            admin_show_qr_pl.Visible = false;
            admin_read_QR_pic.Visible = true;

            // show QR Reader Panel and Hide Generated QR
        }

        private void run_cam_qr_timer_Tick(object sender, EventArgs e)
        {
            if (admin_read_QR_pic.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)admin_read_QR_pic.Image);
                if (result != null)
                {
                    pay_search_txt.Text = result.ToString();
                    CaptureDevice.Stop();
                    pay_openQR.Start();
                    run_cam_qr_timer.Stop();
                    admin_read_QR_pic.Image = null;
                    CaptureDevice.Start();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox1.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                run_cam_qr_timer.Start();
                capDev = true;
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

        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            login_Form.Show();
            this.Hide();
        }

        private void add_new_user_chk_CheckedChanged(object sender, EventArgs e)
        {
            if(add_new_user_chk.Checked)
            {
                pay_add_user_btn.Enabled = true;
                pay_edit_user_btn.Enabled = false;
            }
            else
            {
                pay_edit_user_btn.Enabled = true;
                pay_add_user_btn.Enabled = false;
            }
        }
    }
}
