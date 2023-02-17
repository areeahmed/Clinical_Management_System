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
    public partial class Admin_PatientView : Form
    {
        private Point mouseLocation;
        private bool isMouseDown = false;
        bool sideBarExpand = false;
        bool isBarcodeOpen = false;
        public Admin_PatientView(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }

        private void Admin_PatientView_Load(object sender, EventArgs e)
        {
            doctor_barcode_panel.Visible = false;
            System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
            obj.AddEllipse(0, 0, pictureBox2.Width, pictureBox2.Height);
            Region region = new Region(obj);
            pictureBox2.Region = region;
            // using this code to make a panel movable later and for creating report
            ControlExtension.Draggable(doctor_barcode_panel, true);
            dateTimeTimer.Start();
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
                pictureBox2.ImageLocation = openFileDialog1.FileNames[0];
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
                pay_QrPic.SizeMode = PictureBoxSizeMode.AutoSize;
                Zen.Barcode.CodeQrBarcodeDraw codeQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                pay_QrPic.Image = codeQr.Draw(pay_ID.Text, 200);

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
        }
    }
}
