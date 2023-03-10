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
    public partial class Login_Form : Form
    {
        // this variable is used to darg and drop the form and change the location of form acording mouse location
        private Point mouseLocation;
        private bool isMouseDown = false;

        
        public Login_Form()
        {
            InitializeComponent();
        }

        // hide password
        //open eye button
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            password_txt.UseSystemPasswordChar = true;
            closeEye.Visible = true;
            openEye.Visible = false;
        }

        // show password
        // close eye button
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            password_txt.UseSystemPasswordChar = false;
            closeEye.Visible = false;
            openEye.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // when mouse is clicked but don't move 
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseLocation = e.Location;
        }

        // when mouse is'nt clickable
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        // when mouse is clicked and move
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - mouseLocation.X) + e.X, (this.Location.Y - mouseLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Backend work will be here use ( comboBox1 , textBox1 , textBox2 )
        }

        // close application icon
        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // maxizimize and get to normal icon
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        // minimize icon
        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
