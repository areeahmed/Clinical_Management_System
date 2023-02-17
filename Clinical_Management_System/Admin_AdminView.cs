using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinical_Management_System
{
    public partial class Admin_AdminView : Form
    {
        
        private Point mouseLocation;
        private bool isMouseDown = false;
        bool sideBarExpand = false;
        bool isFormOpened = false;
        bool isBarcodeOpen = false;
        public Admin_AdminView(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
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

        private void Admin_AdminView_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseLocation = e.Location;
        }

        private void Admin_AdminView_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Admin_AdminView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - mouseLocation.X) + e.X, (this.Location.Y - mouseLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void DoctorButton_Click(object sender, EventArgs e)
        {
            Admin_DoctorView doctorForm = new Admin_DoctorView(windowState: this.WindowState);
            doctorForm.Show();
            this.Hide();
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard(windowState: this.WindowState);
            adminDashboard.Show();
            this.Hide();
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

        private void Admin_AdminView_Load(object sender, EventArgs e)
        {
            // this part of code is for making picture look like circle
            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, pictureBox2.Width, pictureBox2.Height);
            Region region = new Region(obj);
            pictureBox2.Region = region;
            // until here is for profile picture circing

            // invisibling the panels at first
            Adding_Doctor_Form_panel.Visible = false;
            doctor_barcode_panel.Visible = false;
            // for making it fraggable
            ControlExtension.Draggable(doctor_barcode_panel, true);
            ControlExtension.Draggable(Adding_Doctor_Form_panel, true);

            // timer for the date time
            dateTimeTimer.Start();
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
            if(admin_ID.Text == "#")
            {
                MessageBox.Show("ببورە نتوانم هیچ بارکۆدێک پەخش بکەم چونکە هیچ بەکارهێنەرێکت دەستنیشان نەکردووە", "بەکارهێنان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                admin_QrPic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr =  Zen.Barcode.BarcodeDrawFactory.CodeQr;
                admin_QrPic.Image = codeQr.Draw(admin_ID.Text, 200);

                barcodeTimer.Start();
                doctor_barcode_panel.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            barcodeTimer.Start();
            doctor_barcode_panel.Visible = true;
        }

        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Admin_PatientView admin_PatientView = new Admin_PatientView(windowState: this.WindowState);
            admin_PatientView.Show();
            this.Hide();
        }

        private void ReciptionButton_Click(object sender, EventArgs e)
        {
            Admin_ReciptionView admin_ReciptionView = new Admin_ReciptionView(this.WindowState);
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
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = openFileDialog1.FileNames[0];
            }
        }
    }
}
