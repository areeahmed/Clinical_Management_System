namespace Clinical_Management_System
{
    partial class WelcomeScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeScreen));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.slaw = new System.Windows.Forms.Label();
            this.slawTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::Clinical_Management_System.Properties.Resources._7502672;
            this.pictureBox1.Location = new System.Drawing.Point(334, 42);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(206, 207);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panel1.Location = new System.Drawing.Point(0, 488);
            this.panel1.MaximumSize = new System.Drawing.Size(909, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(15, 28);
            this.panel1.TabIndex = 9;
            // 
            // timer1
            // 
            this.timer1.Interval = 190;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // slaw
            // 
            this.slaw.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.slaw.AutoSize = true;
            this.slaw.BackColor = System.Drawing.Color.Transparent;
            this.slaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.slaw.Font = new System.Drawing.Font("RudawRegular", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slaw.ForeColor = System.Drawing.Color.White;
            this.slaw.Location = new System.Drawing.Point(82, 271);
            this.slaw.Name = "slaw";
            this.slaw.Size = new System.Drawing.Size(701, 77);
            this.slaw.TabIndex = 7;
            this.slaw.Text = "سیستەمی بەڕێوەبردنی کلینیک";
            // 
            // slawTimer
            // 
            this.slawTimer.Interval = 1;
            this.slawTimer.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // WelcomeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(907, 513);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.slaw);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WelcomeScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WelcomeScreen";
            this.Load += new System.EventHandler(this.WelcomeScreen_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WelcomeScreen_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WelcomeScreen_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WelcomeScreen_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label slaw;
        private System.Windows.Forms.Timer slawTimer;
    }
}