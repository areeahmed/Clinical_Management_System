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
    public partial class Admin_DoctorView : Form
    {
        private Point mouseLocation;
        private bool isMouseDown = false;
        bool sideBarExpand = false;
        bool isFormOpened = false;
        bool isBarcodeOpen = false;
        
        public Admin_DoctorView()
        {
            InitializeComponent();
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
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Hide();
        }

        // cancel form and Hide it
        private void button6_Click(object sender, EventArgs e)
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
        
        // the functionality of showing and Hidding Adding Doctor Form 
        private void openFormTimer_Tick(object sender, EventArgs e)
        {
            if(isFormOpened)
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

        // show and hide Adding Doctor Form
        private void button3_Click(object sender, EventArgs e)
        {
            openFormTimer.Start();
            Adding_Doctor_Form_panel.Visible = true;
        }


        // form load
        private void Admin_DoctorView_Load(object sender, EventArgs e)
        {
            Adding_Doctor_Form_panel.Visible = false;
            doctor_barcode_panel.Visible = false;
            // for making it fraggable
            ControlExtension.Draggable(doctor_barcode_panel, true);
            ControlExtension.Draggable(Adding_Doctor_Form_panel, true);
        }

        // Barcode timer tik
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

        // open barcode panel
        private void button6_Click_1(object sender, EventArgs e)
        {
            barcodeTimer.Start();
            doctor_barcode_panel.Visible = true;
        }

        // DONE USING BARCODE FORM
        private void button7_Click(object sender, EventArgs e)
        {
            barcodeTimer.Start();
            doctor_barcode_panel.Visible = true;
        }
    }
}
