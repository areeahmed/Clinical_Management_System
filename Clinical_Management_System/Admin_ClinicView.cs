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
            clinic_add_new_btn.Enabled = false;
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

        private void copyDocBtn_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            // system logo picture
            e.Graphics.DrawImage(picLogoPrint.Image, 10, 10, 100, 100);
            // Kurdistan Label
            e.Graphics.DrawString(print_krd_lbl.Text, new Font("RudawRegular", 24), Brushes.Gray, 125, 30);
            // health label
            e.Graphics.DrawString(print_hl_lbl.Text, new Font("RudawRegular", 20), Brushes.Gray, 125, 60);
            // care label
            e.Graphics.DrawString(print_cr_lbl.Text, new Font("RudawRegular", 20), Brushes.ForestGreen, 200, 60);
            // clinic word label
            e.Graphics.DrawString(clinic_lbl.Text, new Font("RudawRegular", 25), Brushes.Black, 700, 30);
            // system logo picture low opacity
            e.Graphics.DrawImage(print_pic_op.Image, 180, 300, 500, 500);


            //
            // Until here location and some coponent will be same for all forms that have print button
            //



            // ID
            e.Graphics.DrawString(clinic_id.Text, new Font("RudawRegular", 24), Brushes.Black, 630, 400);
            e.Graphics.DrawString(clinic_id_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 400);

            // NAME
            e.Graphics.DrawString(clinic_name.Text, new Font("RudawRegular", 24), Brushes.Black, 480, 450);
            e.Graphics.DrawString(clinic_name_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 450);

            // INCOME
            e.Graphics.DrawString(clinic_income_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 340, 550);
            // TODAY
            e.Graphics.DrawString(clinic_today_income_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 240, 650);
            e.Graphics.DrawString(clinic_today_income.Text, new Font("RudawRegular", 22), Brushes.Black, 90, 650);
            // TOTAL
            e.Graphics.DrawString(clinic_total_income_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 240, 700);
            e.Graphics.DrawString(clinic_total_income.Text, new Font("RudawRegular", 22), Brushes.Black, 90, 700);



            // PATIENT
            e.Graphics.DrawString(clinic_patient_lbl.Text, new Font("RudawRegular", 24), Brushes.Black, 680, 550);
            // DONE
            e.Graphics.DrawString(clinic_descharged_pt.Text, new Font("RudawRegular", 22), Brushes.Black, 640, 650);
            e.Graphics.DrawString(clinic_descharged_pt_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 680, 650);
            // REMAIN
            e.Graphics.DrawString(clinic_remain_pt.Text, new Font("RudawRegular", 22), Brushes.Black, 640, 700);
            e.Graphics.DrawString(clinic_remain_pt_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 680, 700);
            // OTHER DAY
            e.Graphics.DrawString(clinic_other_day_pt.Text, new Font("RudawRegular", 22), Brushes.Black, 640, 750);
            e.Graphics.DrawString(clinic_other_day_pt_lbl.Text, new Font("RudawRegular", 22), Brushes.Black, 680, 750);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Adding_Doctor_Form_panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void clinic_add_new_chk_CheckedChanged(object sender, EventArgs e)
        {
            if(clinic_add_new_chk.Checked)
            {
                clinic_add_new_btn.Enabled = true;
                clinic_update_btn.Enabled = false;
            }
            else
            {
                clinic_add_new_btn.Enabled = false;
                clinic_update_btn.Enabled = true;
            }
        }
    }
}
