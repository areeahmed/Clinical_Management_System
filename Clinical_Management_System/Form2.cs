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
    public partial class patient: Form
    {
        // Open Form Panel 
        bool isFormOpened = false;

        // Open Barcode Read and Show Panel
        bool isBarcodeOpen = false;
        // Open Sidebar FlowLayoutPanel
        bool sideBarExpand = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();

        public patient(FormWindowState windowState)
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
            appointment f3 = new appointment();
            f3.Show();
        }

        private void patient_Load(object sender, EventArgs e)
        {
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
            pay_openQR.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pay_openQR.Start();
        }
    }
}
