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
    public partial class Admin_ClinicView : Form
    {
        private Point mouseLocation;
        private bool isMouseDown = false;
        bool sideBarExpand = false;
        bool isFormOpened = false;
        public Admin_ClinicView(FormWindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard(this.WindowState);
            adminDashboard.Show();
            this.Hide();
        }

        private void ClinicButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("فۆڕمەکە کرایتەوە", "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            Admin_AdminView adminView = new Admin_AdminView(this.WindowState);
            adminView.Show();
            this.Hide();
        }

        private void DoctorButton_Click(object sender, EventArgs e)
        {
            Admin_DoctorView admin_DoctorView = new Admin_DoctorView(this.WindowState);
            admin_DoctorView.Show();
            this.Hide();
        }

        private void ReciptionButton_Click(object sender, EventArgs e)
        {
            Admin_ReciptionView admin_ReciptionView = new Admin_ReciptionView(this.WindowState);
            admin_ReciptionView.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Admin_PatientView admin_PatientView = new Admin_PatientView(this.WindowState);
            admin_PatientView.Show();
            this.Hide();
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

        private void editDocBtn_Click(object sender, EventArgs e)
        {
            Adding_Doctor_Form_panel.Visible = true;
            openFormTimer.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void dateTimeTimer_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToShortTimeString();
            WeekDay.Text = DateTime.Now.DayOfWeek.ToString();
            DayWeekYear.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }

        private void Admin_ClinicView_Load(object sender, EventArgs e)
        {
            
            Adding_Doctor_Form_panel.Visible = false;
            // for making it fraggable
            ControlExtension.Draggable(Adding_Doctor_Form_panel, true);
            dateTimeTimer.Start();
        }

        private void cancel_Form_Button_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
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
    }
}
