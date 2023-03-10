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
    public partial class Admin_DoctorView : Form
    {
        private Point mouseLocation;
        private bool isMouseDown = false;
        bool sideBarExpand = false;
        bool isFormOpened = false;
        bool isBarcodeOpen = false;
        // Needed for the QR Code Variable + 3 Library
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice CaptureDevice;
        bool capDev = false;

        // the argument is to share the common windows status between forms
        public Admin_DoctorView(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }


        // application exit
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Application.Exit();
        }

        // maximize and normal windows state
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

        // minimizing the windows
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // the process of expanding sidebar
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

        // to expand the sidebar
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        // from here the program is to move the form
        private void Admin_DoctorView_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseLocation = e.Location;
        }

        private void Admin_DoctorView_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Admin_DoctorView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - mouseLocation.X) + e.X, (this.Location.Y - mouseLocation.Y) + e.Y);

                this.Update();
            }
        }
        //
        //
        // Until Here is for moving main form even if there is not tool for movement
        //
        //

        // warning user that he/she is now in current form that he/she want to open and not to open again
        private void DoctorButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // go to admin dashboard
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

        // cancel form and Hide it
        private void button6_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
            doc_id_txt.Clear();
            doc_username_txt.Clear();
            doc_password_txt.Clear();
            doc_fullname_txt.Clear();
            doc_phone_txt.Clear();
            doc_address_txt.Clear();
            doc_form_pnl.Visible = true;
        }
        
        // the functionality of showing and Hidding Adding Doctor Form 
        private void openFormTimer_Tick(object sender, EventArgs e)
        {
            if(isFormOpened)
            {
                doc_form_pnl.Height -= 30;
                if (doc_form_pnl.Height == doc_form_pnl.MinimumSize.Height)
                {
                    isFormOpened = false;
                    openFormTimer.Stop();
                }
            }
            else
            {
                doc_form_pnl.Height += 30;
                if (doc_form_pnl.Height == doc_form_pnl.MaximumSize.Height)
                {
                    isFormOpened = true;
                    openFormTimer.Stop();
                }
            }
        }

        // show and hide Adding Doctor Form
        private void button3_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
            doc_form_pnl.Visible = true;
        }


        // form load
        private void Admin_DoctorView_Load(object sender, EventArgs e)
        {
            doc_add_user_btn.Enabled = false;

            qr_code_is_active_lbl.Visible = false;
            // needed for reading QR Code Form Load
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                doc_choosing_cam_cmb.Items.Add(filterInfo.Name);
                doc_choosing_cam_cmb.SelectedIndex = 0;
            }


            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, doc_profile_pic.Width, doc_profile_pic.Height);
            Region region = new Region(obj);
            doc_profile_pic.Region = region;
            doc_form_pnl.Visible = false;
            doc_qr_pnl.Visible = false;
            // for making it fraggable
            ControlExtension.Draggable(doc_qr_pnl, true);
            ControlExtension.Draggable(doc_form_pnl, true);
            dateTimeTimer.Start();
        }

        // Barcode timer tik
        private void barcodeTimer_Tick(object sender, EventArgs e)
        {
            if (isBarcodeOpen)
            {
                doc_qr_pnl.Height -= 30;
                if (doc_qr_pnl.Height == doc_qr_pnl.MinimumSize.Height)
                {
                    isBarcodeOpen = false;
                    barcodeTimer.Stop();
                }
            }
            else
            {
                doc_qr_pnl.Height += 30;
                if (doc_qr_pnl.Height == doc_qr_pnl.MaximumSize.Height)
                {
                    isBarcodeOpen = true;
                    barcodeTimer.Stop();
                }
            }
        }

        // open barcode panel
        private void button6_Click_1(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            doc_qr_read_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            if (doc_id.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                doc_qr_show_pic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                doc_qr_show_pic.Image = codeQr.Draw(doc_id.Text, 200);

                barcodeTimer.Start();
                doc_qr_pnl.Visible = true;
            }
        }

        // DONE USING BARCODE FORM
        private void button7_Click(object sender, EventArgs e)
        {
            qr_code_is_active_lbl.Visible = false;
            doc_qr_read_pic.Image = null;
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            barcodeTimer.Start();
            doc_qr_pnl.Visible = true;
        }

        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString(); 
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

        private void ReciptionButton_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
            }
            Admin_PatientView admin_PatientView = new Admin_PatientView(windowState: this.WindowState);
            admin_PatientView.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                doc_profile_pic.ImageLocation = openFileDialog1.FileNames[0];
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Designing Part
            e.Graphics.DrawImage(picLogoPrint.Image, 10, 10, 100, 100);
            e.Graphics.DrawString(print_krd_lbl.Text, new Font("RudawRegular", 24), Brushes.Gray, 125, 30);
            e.Graphics.DrawString(print_hl_lbl.Text, new Font("RudawRegular", 20), Brushes.Gray, 125, 60);
            e.Graphics.DrawString(print_cr_lbl.Text, new Font("RudawRegular", 20), Brushes.ForestGreen, 200, 60);
            e.Graphics.DrawString(doc_lbl.Text, new Font("RudawRegular", 25), Brushes.Black, 700, 30);
            e.Graphics.DrawImage(print_pic_op.Image, 180, 300, 500, 500);
            e.Graphics.DrawImage(doc_profile_pic.Image, 320, 150, 200, 200);

            // ID
            // ID res
            e.Graphics.DrawString(doc_id.Text, new Font("RudawRegular", 24), Brushes.Black, 630, 450);
            // ID lbl
            e.Graphics.DrawString(doc_id_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 450);

            // NAME
            // Name res
            e.Graphics.DrawString(doc_name.Text, new Font("RudawRegular", 24), Brushes.Black, 480, 500);
            // Name lbl
            e.Graphics.DrawString(doc_name_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 500);

            // PROFITIONALLITY
            e.Graphics.DrawString(doc_profile.Text, new Font("RudawRegular", 24), Brushes.Black, 560, 550);
            e.Graphics.DrawString(doc_profile_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 550);

            // CERTIFICATE
            e.Graphics.DrawString(doc_certify.Text, new Font("RudawRegular", 24), Brushes.Black, 560, 600);
            e.Graphics.DrawString(doc_certify_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 600);

            // CLINIC
            e.Graphics.DrawString(doc_clinic.Text, new Font("RudawRegular", 24), Brushes.Black, 560, 650);
            e.Graphics.DrawString(doc_clinic_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 650);
            
            // QR CODE
            e.Graphics.DrawImage(doc_qr_show_pic.Image, 70, 500, 150, 150);


            // INCOME
            e.Graphics.DrawString(Income_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 340, 720);
            // TODAY
            e.Graphics.DrawString(doc_today_income_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 240, 820);
            e.Graphics.DrawString(doc_today_income.Text, new Font("RudawRegular", 22), Brushes.Black, 90, 820);
            // TOTAL
            e.Graphics.DrawString(doc_total_income_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 240, 870);
            e.Graphics.DrawString(doc_total_income.Text, new Font("RudawRegular", 22), Brushes.Black, 90, 870);



            // PATIENT
            e.Graphics.DrawString(Patient_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 720);
            // DONE
            e.Graphics.DrawString(doc_descharged_pt.Text, new Font("RudawRegular", 22), Brushes.Black, 640, 820);
            e.Graphics.DrawString(doc_descharged_pt_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 680, 820);
            // REMAIN
            e.Graphics.DrawString(doc_remain_pt.Text, new Font("RudawRegular", 22), Brushes.Black, 640, 870);
            e.Graphics.DrawString(cremain_pt_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 680, 870);
            // OTHER DAY
            e.Graphics.DrawString(doc_other_day_pt.Text, new Font("RudawRegular", 22), Brushes.Black, 640, 920);
            e.Graphics.DrawString(doc_other_day_pt_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 680, 920);


            e.Graphics.DrawString(WeekDay.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 1000);
            e.Graphics.DrawString(time.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 970);
            e.Graphics.DrawString(DayWeekYear.Text, new Font("RudawRegular", 14), Brushes.Black, 680, 1030);
        }

        private void copyDocBtn_Click(object sender, EventArgs e)
        {
            doc_qr_show_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            doc_qr_show_pic.Image = codeQr.Draw(doc_id.Text, 200);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void show_qr_gen_btn_Click(object sender, EventArgs e)
        {
            doc_qr_show_pic.SizeMode = PictureBoxSizeMode.AutoSize;
            Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            doc_qr_show_pic.Image = codeQr.Draw(doc_id.Text, 200);
            doc_qr_show_pl.Visible = true;
            doc_qr_read_pl.Visible = false;
        }

        private void show_qr_reader_btn_Click(object sender, EventArgs e)
        {
            doc_qr_show_pl.Visible = false;
            doc_qr_read_pl.Visible = true;
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            doc_qr_read_pic.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void run_cam_qr_timer_Tick(object sender, EventArgs e)
        {
            if (doc_qr_read_pic.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)doc_qr_read_pic.Image);
                if (result != null)
                {
                    doc_search_txt.Text = result.ToString();
                    CaptureDevice.Stop();
                    barcodeTimer.Start();
                    run_cam_qr_timer.Stop();
                    doc_qr_read_pic.Image = null;
                    CaptureDevice.Start();
                }
            }
        }

        private void start_read_qr_btn_Click(object sender, EventArgs e)
        {
            if (capDev)
            {
                CaptureDevice.Stop();
                capDev = false;
            }
            if (!capDev)
            {
                qr_code_is_active_lbl.Visible = true;
                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[doc_choosing_cam_cmb.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                run_cam_qr_timer.Start();
                capDev = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            login_Form.Show();
            this.Hide();
        }

        private void doc_add_user_chk_CheckedChanged(object sender, EventArgs e)
        {
            if(doc_add_user_chk.Checked)
            {
                doc_add_user_btn.Enabled = true;
                doc_edit_user_btn.Enabled = false;
            }
            else
            {
                doc_add_user_btn.Enabled = false;
                doc_edit_user_btn.Enabled = true;
            }
        }
    }
}
