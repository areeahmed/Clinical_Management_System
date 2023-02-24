using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace Clinical_Management_System
{
    public partial class Admin_PatientView : Form
    {
        private Point mouseLocation;
        private bool isMouseDown = false;
        bool sideBarExpand = false;
        bool isBarcodeOpen = false;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice CaptureDevice;
        bool capDev = false;
        public Admin_PatientView(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }

        private void Admin_PatientView_Load(object sender, EventArgs e)
        {
            // for reading barcode
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                comboBox1.Items.Add(filterInfo.Name);
                comboBox1.SelectedIndex = 0;
            }

            doctor_barcode_panel.Visible = false;
            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, pa_profile_pic.Width, pa_profile_pic.Height);
            Region region = new Region(obj);
            pa_profile_pic.Region = region;
            // using this code to make a panel movable later and for creating report
            ControlExtension.Draggable(doctor_barcode_panel, true);
            dateTimeTimer.Start();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
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

        private void Admin_PatientView_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseLocation = e.Location;
        }

        private void Admin_PatientView_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Admin_PatientView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - mouseLocation.X) + e.X, (this.Location.Y - mouseLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard(windowState: this.WindowState);
            adminDashboard.Show();
            this.Hide();
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            Admin_AdminView admin_AdminView = new Admin_AdminView(windowState: this.WindowState);
            admin_AdminView.Show();
            this.Hide();
        }

        private void DoctorButton_Click(object sender, EventArgs e)
        {
            Admin_DoctorView admin_DoctorView = new Admin_DoctorView(windowState: this.WindowState);
            admin_DoctorView.Show();
            this.Hide();
        }

        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }

        private void ReciptionButton_Click(object sender, EventArgs e)
        {
            Admin_ReciptionView admin_ReciptionView = new Admin_ReciptionView(WindowState);
            admin_ReciptionView.Show();
            this.Hide();
        }

        private void ClinicButton_Click(object sender, EventArgs e)
        {
            Admin_ClinicView admin_ClinicView = new Admin_ClinicView(this.WindowState);
            admin_ClinicView.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pa_profile_pic.ImageLocation = openFileDialog1.FileNames[0];
            }
        }

        private void barcodeDocBtn_Click(object sender, EventArgs e)
        {
            if (pay_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                pa_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                pa_qr_pic.Image = codeQr.Draw(pay_ID.Text, 200);

                barcodeTimer.Start();
                doctor_barcode_panel.Visible = true;
            }
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

        private void button7_Click(object sender, EventArgs e)
        {
            barcodeTimer.Start();
            doctor_barcode_panel.Visible = true;
            if(capDev)
            {
                CaptureDevice.Stop();
            }
        }

        private void copyDocBtn_Click(object sender, EventArgs e)
        {
            if (pay_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                pa_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                pa_qr_pic.Image = codeQr.Draw(pay_ID.Text, 200);
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }

            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            pa_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            pa_qr_pic.Image = codeQr.Draw(pay_ID.Text, 200);
            admin_show_qr_pl.Visible = true;
            admin_read_qr_pl.Visible = false;
        }

        // check if the button pressed for first time or not
        bool isFirst = true;
        private void button3_Click(object sender, EventArgs e)
        {
            if (isFirst)
            {
                admin_show_qr_pl.Visible = false;
                admin_read_qr_pl.Visible = true;
                isFirst = false;
            }
            else
            {
                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBox1.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                run_cam_qr_timer.Start();
                capDev = true;
                isFirst = false;
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

        private void run_cam_qr_Tick(object sender, EventArgs e)
        {
            if (admin_read_QR_pic.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)admin_read_QR_pic.Image);
                if (result != null)
                {
                    pa_search_txt.Text = result.ToString();
                    CaptureDevice.Stop();
                    barcodeTimer.Start();

                    run_cam_qr_timer.Stop();

                }
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(picLogoPrint.Image, 10, 10, 100, 100);
            e.Graphics.DrawString(print_krd_lbl.Text, new Font("RudawRegular", 24), Brushes.Gray, 125, 30);
            e.Graphics.DrawString(print_hl_lbl.Text, new Font("RudawRegular", 20), Brushes.Gray, 125, 60);
            e.Graphics.DrawString(print_cr_lbl.Text, new Font("RudawRegular", 20), Brushes.ForestGreen, 200, 60);
            e.Graphics.DrawString(pa_lbl.Text, new Font("RudawRegular", 25), Brushes.Black, 700, 30);
            e.Graphics.DrawImage(print_pic_op.Image, 180, 300, 500, 500);
            e.Graphics.DrawImage(pa_profile_pic.Image, 320, 150, 200, 200);
        }
    }
}
