using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace Clinical_Management_System
{
    public partial class WelcomeScreen : Form
    {
        public WelcomeScreen()
        {
            InitializeComponent();
        }
        private Point mouseLocation;
        private bool isMouseDown = false;
        private void WelcomeScreen_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            
            slaw.Visible = true;

            slawTimer.Start();
            timer1.Start();

            slaw.ForeColor = Color.FromArgb(this.BackColor.R, this.BackColor.G, this.BackColor.B);
        }

        private void WelcomeScreen_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseLocation = e.Location;
        }

        private void WelcomeScreen_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void WelcomeScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - mouseLocation.X) + e.X, (this.Location.Y - mouseLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            panel1.Visible = true;
            panel1.Width += 30;
            if(panel1.Width == panel1.MaximumSize.Width)
            {
                timer1.Stop();
                Login_Form login_Form = new Login_Form();
                login_Form.Show();
                this.Hide();
            }
        }

        int counter = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter > 1 && counter < 200)
                fadeIn(slaw);
            else if (counter <= 400 && counter >= 200)
                fadeOut(slaw);
            if (counter == 400)
                counter = 0;
        }
        void fadeOut(Label label)
        {
            int[] targetColor = { 0, 0, 0 }; //Black
            int[] fadeRGB = new int[3];
            fadeRGB[0] = label.ForeColor.R;
            fadeRGB[1] = label.ForeColor.G;
            fadeRGB[2] = label.ForeColor.B;

            if (fadeRGB[0] > this.BackColor.R)
                fadeRGB[0]--;
            else if (fadeRGB[0] < this.BackColor.R)
                fadeRGB[0]++;
            if (fadeRGB[1] > this.BackColor.G)
                fadeRGB[1]--;
            else if (fadeRGB[1] < this.BackColor.G)
                fadeRGB[1]++;
            if (fadeRGB[2] > this.BackColor.B)
                fadeRGB[2]--;
            else if (fadeRGB[2] < this.BackColor.B)
                fadeRGB[2]++;
            label.ForeColor = Color.FromArgb(fadeRGB[0], fadeRGB[1], fadeRGB[2]);
        }

        void fadeIn(Label label)
        {
            int[] targetColor = { 0, 0, 0 }; //Black
            int[] fadeRGB = new int[3];
            fadeRGB[0] = label.ForeColor.R;
            fadeRGB[1] = label.ForeColor.G;
            fadeRGB[2] = label.ForeColor.B;

            if (fadeRGB[0] > targetColor[0])
                fadeRGB[0]--;
            else if (fadeRGB[0] < targetColor[0])
                fadeRGB[0]++;
            if (fadeRGB[1] > targetColor[1])
                fadeRGB[1]--;
            else if (fadeRGB[1] < targetColor[1])
                fadeRGB[1]++;
            if (fadeRGB[2] > targetColor[2])
                fadeRGB[2]--;
            else if (fadeRGB[2] < targetColor[2])
                fadeRGB[2]++;

            label.ForeColor = Color.FromArgb(fadeRGB[0], fadeRGB[1], fadeRGB[2]);
        }

    }
}
