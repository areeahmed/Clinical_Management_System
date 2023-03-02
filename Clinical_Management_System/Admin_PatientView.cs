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
        //
        //
        // Needed for the QR Code Variable + 3 Library
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice CaptureDevice;
        bool capDev = false;
        //
        //
        //
        public Admin_PatientView(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }

        private void Admin_PatientView_Load(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            // needed for reading QR Code Form Load
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                pay_choose_cam_cmb.Items.Add(filterInfo.Name);
                pay_choose_cam_cmb.SelectedIndex = 0;
            }

            pay_barcode_panel.Visible = false;
            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, pa_profile_pic.Width, pa_profile_pic.Height);
            Region region = new Region(obj);
            pa_profile_pic.Region = region;
            // using this code to make a panel movable later and for creating report
            ControlExtension.Draggable(pay_barcode_panel, true);
            dateTimeTimer.Start();
        }


        // Closing Application Needed for Stoping QR Code
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

        // ALL navigations 
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
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Admin_ReciptionView admin_ReciptionView = new Admin_ReciptionView(WindowState);
            admin_ReciptionView.Show();
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

        // First Button Clicked to Open Barcode
        private void barcodeDocBtn_Click(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            pay_read_qr_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            if (pay_id.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                pay_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                pay_show_qr_pic.Image = codeQr.Draw(pay_id.Text, 200);

                barcodeTimer.Start();
                pay_barcode_panel.Visible = true;
            }
        }


        // Timmer tick to open barcode panel
        private void barcodeTimer_Tick(object sender, EventArgs e)
        {
            if (isBarcodeOpen)
            {
                pay_barcode_panel.Height -= 30;
                if (pay_barcode_panel.Height == pay_barcode_panel.MinimumSize.Height)
                {
                    isBarcodeOpen = false;
                    barcodeTimer.Stop();
                }
            }
            else
            {
                pay_barcode_panel.Height += 30;
                if (pay_barcode_panel.Height == pay_barcode_panel.MaximumSize.Height)
                {
                    isBarcodeOpen = true;
                    barcodeTimer.Stop();
                }
            }
        }

        // Closing QR Code panel needed to stop QR Code Reader
        private void button7_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                pay_read_qr_pic.Image = null;
                CaptureDevice.Stop();
                qr_code_is_active_lbl.Visible = false;
            }
            barcodeTimer.Start();
            pay_barcode_panel.Visible = true;
        }

        private void copyDocBtn_Click(object sender, EventArgs e)
        {
            if (pay_id.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                pay_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                pay_show_qr_pic.Image = codeQr.Draw(pay_id.Text, 200);
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }

            }
        }

        // needed for generating QR Code
        private void button7_Click_1(object sender, EventArgs e)
        {
            pay_show_qr_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            pay_show_qr_pic.Image = codeQr.Draw(pay_id.Text, 200);
            pay_show_qr_pl.Visible = true;
            pay_read_qr_pl.Visible = false;
        }

        // Show the QR Code Reader panel
        private void Show_QRcode_reader(object sender, EventArgs e)
        {
            pay_show_qr_pl.Visible = false;
            pay_read_qr_pl.Visible = true;
        }

        // check closing QR Code if Worked before
        private void button3_Click(object sender, EventArgs e)
        {
            if(capDev)
            {
                CaptureDevice.Stop();
                capDev = false;
            }
            if(!capDev)
            {
                qr_code_is_active_lbl.Visible = true;
                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[pay_choose_cam_cmb.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                run_cam_qr_timer.Start();
                capDev = true;
            }
            
        }

        
        // new frame needed for the even above
        /// <summary>
        /// this part of code will get the result from qr code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pay_read_qr_pic.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(picLogoPrint.Image, 10, 10, 100, 100);
            e.Graphics.DrawString(print_krd_lbl.Text, new Font("RudawRegular", 24), Brushes.Gray, 125, 30);
            e.Graphics.DrawString(print_hl_lbl.Text, new Font("RudawRegular", 20), Brushes.Gray, 125, 60);
            e.Graphics.DrawString(print_cr_lbl.Text, new Font("RudawRegular", 20), Brushes.ForestGreen, 200, 60);
            e.Graphics.DrawString(pay_lbl.Text, new Font("RudawRegular", 25), Brushes.Black, 700, 30);
            e.Graphics.DrawImage(print_pic_op.Image, 180, 300, 500, 500);
            e.Graphics.DrawImage(pa_profile_pic.Image, 320, 150, 200, 200);


            // ID
            // ID res
            e.Graphics.DrawString(pay_id.Text, new Font("RudawRegular", 24), Brushes.Black, 630, 500);
            // ID lbl
            e.Graphics.DrawString(pay_id_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 500);

            // NAME
            // Name res
            e.Graphics.DrawString(pay_name.Text, new Font("RudawRegular", 24), Brushes.Black, 380, 550);
            // Name lbl
            e.Graphics.DrawString(pay_name_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 550);

            // PHONE
            // Phone res
            e.Graphics.DrawString(pay_phone.Text, new Font("RudawRegular", 24), Brushes.Black, 440, 600);
            // Phone lbl
            e.Graphics.DrawString(pay_phone_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 600);

            // GENDER
            // Gender res
            e.Graphics.DrawString(pay_gender.Text, new Font("RudawRegular", 24), Brushes.Black, 630, 650);
            // Gender lbl
            e.Graphics.DrawString(pay_gender_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 650);

            // Age
            // Age res
            e.Graphics.DrawString(pay_age.Text, new Font("RudawRegular", 24), Brushes.Black, 400, 650);
            // Age lbl
            e.Graphics.DrawString(pay_age_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 450, 650);


            // ADDRESS
            // address res
            e.Graphics.DrawString(pay_address.Text, new Font("RudawRegular", 24), Brushes.Black, 350, 700);
            // address lbl
            e.Graphics.DrawString(pay_address_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 700);

            // QR Code
            e.Graphics.DrawImage(pay_show_qr_pic.Image, 350, 900, 150, 150);
        }

        // Timmer tick for capturing the QR Code and returning data to search txt
        private void run_cam_qr_timer_Tick(object sender, EventArgs e)
        {
            if (pay_read_qr_pic.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)pay_read_qr_pic.Image);
                if (result != null)
                {
                    pay_search_txt.Text = result.ToString();
                    CaptureDevice.Stop();
                    barcodeTimer.Start();
                    run_cam_qr_timer.Stop();
                    pay_read_qr_pic.Image = null;
                    CaptureDevice.Start();
                }

            }
        }
    }
}
