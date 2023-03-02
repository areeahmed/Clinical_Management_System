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

namespace Clinical_Management_System
{
    public partial class Admin_ReciptionView : Form
    {
        private Point mouseLocation;
        private bool isMouseDown = false;
        bool sideBarExpand = false;
        bool isFormOpened = false;
        bool isBarcodeOpen = false;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice CaptureDevice;
        bool capDev = false;
        public Admin_ReciptionView(FormWindowState formWindowState)
        {
            InitializeComponent();
            this.WindowState = formWindowState;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            run_cam_qr_timer.Stop();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            AdminDashboard adminDashboard = new AdminDashboard(windowState: this.WindowState);
            adminDashboard.Show();
            this.Hide();
        }

        private void ClinicButton_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Admin_ClinicView admin_ClinicView = new Admin_ClinicView(this.WindowState);
            admin_ClinicView.Show();
            this.Hide();
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Admin_AdminView admin_AdminView = new Admin_AdminView(windowState: this.WindowState);
            admin_AdminView.Show();
            this.Hide();
        }

        private void DoctorButton_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Admin_DoctorView admin_DoctorView = new Admin_DoctorView(WindowState);
            admin_DoctorView.Show();
            this.Hide();
        }

        private void ReciptionButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Admin_PatientView admin_PatientView = new Admin_PatientView(windowState: this.WindowState);
            admin_PatientView.Show();
            this.Hide();
        }

        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }

        private void Admin_ReciptionView_Load(object sender, EventArgs e)
        {
            recp_add_user_btn.Enabled = false;

            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                comboBox2.Items.Add(filterInfo.Name);
                comboBox2.SelectedIndex = 0;
            }

            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, recp_profile_pic.Width, recp_profile_pic.Height);
            Region region = new Region(obj);
            recp_profile_pic.Region = region;
            Adding_Doctor_Form_panel.Visible = false;
            doctor_barcode_panel.Visible = false;
            // for making it fraggable
            ControlExtension.Draggable(doctor_barcode_panel, true);
            ControlExtension.Draggable(Adding_Doctor_Form_panel, true);
            dateTimeTimer.Start();
        }

        private void Admin_ReciptionView_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Admin_ReciptionView_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseLocation = e.Location;
        }

        private void Admin_ReciptionView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - mouseLocation.X) + e.X, (this.Location.Y - mouseLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void cancel_Form_Button_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
            docIDtxt.Clear();
            docUsernameTxt.Clear();
            docPasswordTxt.Clear();
            docFullNameTxt.Clear();
            docPhoneTxt.Clear();
            docAddressTxt.Clear();
            Adding_Doctor_Form_panel.Visible = true;
        }

        private void openFormTimer_Tick(object sender, EventArgs e)
        {
            if (isFormOpened)
            {
                Adding_Doctor_Form_panel.Height -= 30;
                if (Adding_Doctor_Form_panel.Height == Adding_Doctor_Form_panel.MinimumSize.Height)
                {
                    isFormOpened = false;
                    openFormTimer.Stop();
                }
            }
            else
            {
                Adding_Doctor_Form_panel.Height += 30;
                if (Adding_Doctor_Form_panel.Height == Adding_Doctor_Form_panel.MaximumSize.Height)
                {
                    isFormOpened = true;
                    openFormTimer.Stop();
                }
            }
        }

        private void editDocBtn_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
            Adding_Doctor_Form_panel.Visible = true;
        }

        private void barcodeTimer_Tick(object sender, EventArgs e)
        {
            if (isBarcodeOpen)
            {
                doctor_barcode_panel.Height -= 30;
                if (doctor_barcode_panel.Height == doctor_barcode_panel.MinimumSize.Height)
                {
                    isBarcodeOpen = false;
                    barcodeTimer.Stop();
                }
            }
            else
            {
                doctor_barcode_panel.Height += 30;
                if (doctor_barcode_panel.Height == doctor_barcode_panel.MaximumSize.Height)
                {
                    isBarcodeOpen = true;
                    barcodeTimer.Stop();
                }
            }
        }

        private void barcodeDocBtn_Click(object sender, EventArgs e)
        {
            if (recp_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                admin_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                admin_show_qr_pic.Image = codeQr.Draw(recp_ID.Text, 200);

                barcodeTimer.Start();
                doctor_barcode_panel.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                admin_read_qr_pic.Image = null;
                CaptureDevice.Stop();
            }
            barcodeTimer.Start();
            doctor_barcode_panel.Visible = true;
            
        }

        private void copyDocBtn_Click(object sender, EventArgs e)
        {
            admin_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            admin_show_qr_pic.Image = codeQr.Draw(recp_ID.Text, 200);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(picLogoPrint.Image, 10, 10, 100, 100);
            e.Graphics.DrawString(print_krd_lbl.Text, new Font("RudawRegular", 24), Brushes.Gray, 125, 30);
            e.Graphics.DrawString(print_hl_lbl.Text, new Font("RudawRegular", 20), Brushes.Gray, 125, 60);
            e.Graphics.DrawString(print_cr_lbl.Text, new Font("RudawRegular", 20), Brushes.ForestGreen, 200, 60);
            e.Graphics.DrawString(recp_lbl.Text, new Font("RudawRegular", 25), Brushes.Black, 700, 30);
            e.Graphics.DrawImage(print_pic_op.Image, 180, 300, 500, 500);
            e.Graphics.DrawImage(recp_profile_pic.Image, 320, 150, 200, 200);

            // QR Code
            e.Graphics.DrawImage(admin_show_qr_pic.Image, 70, 900, 150, 150);

            // ID
            e.Graphics.DrawString(recp_ID.Text, new Font("RudawRegular", 24), Brushes.Black, 630, 500);
            e.Graphics.DrawString(recp_ID_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 500);

            // NAME
            e.Graphics.DrawString(recp_Name.Text, new Font("RudawRegular", 24), Brushes.Black, 480, 550);
            e.Graphics.DrawString(recp_Name_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 550);

            // PHONE
            e.Graphics.DrawString(recp_Phone.Text, new Font("RudawRegular", 24), Brushes.Black, 440, 600);
            e.Graphics.DrawString(recp_phone_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 600);

            // CLINIC
            e.Graphics.DrawString(recp_clinic.Text, new Font("RudawRegular", 24), Brushes.Black, 550, 650);
            e.Graphics.DrawString(recp_clinic_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 650);

            // ADDRESS
            e.Graphics.DrawString(recp_address.Text, new Font("RudawRegular", 24), Brushes.Black, 450, 700);
            e.Graphics.DrawString(recp_address_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 700);

            e.Graphics.DrawString(WeekDay.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 1000);
            e.Graphics.DrawString(time.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 970);
            e.Graphics.DrawString(DayWeekYear.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 1030);
        }

        private void recp_profile_pic_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                recp_profile_pic.ImageLocation = openFileDialog1.FileNames[0];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            admin_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            admin_show_qr_pic.Image = codeQr.Draw(recp_ID.Text, 200);
            admin_show_qr_pl.Visible = true;
            admin_read_qr_pl.Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
             admin_show_qr_pl.Visible = false;
             admin_read_qr_pl.Visible = true;
            
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

        private void button6_Click(object sender, EventArgs e)
        {
            if(capDev)
            {
                CaptureDevice.Stop();
                capDev = false;
            }
            if(!capDev)
            {
                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox2.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                run_cam_qr_timer.Start();
                capDev = true;
            }
        }

        private void run_cam_qr_timer_Tick(object sender, EventArgs e)
        {
            if (admin_read_qr_pic.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)admin_read_qr_pic.Image);
                if (result != null)
                {
                    recp_search_txt.Text = result.ToString();
                    CaptureDevice.Stop();
                    barcodeTimer.Start();
                    run_cam_qr_timer.Stop();
                    admin_read_qr_pic.Image = null;
                    CaptureDevice.Start();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            login_Form.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                recp_add_user_btn.Enabled = true;
                recp_edit_user_btn.Enabled = false;
            }
            else
            {
                recp_edit_user_btn.Enabled = true;
                recp_add_user_btn.Enabled = false;
            }    
        }
    }
}
