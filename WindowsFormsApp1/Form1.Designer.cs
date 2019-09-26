namespace WindowsFormsApp1
{
    partial class SUN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SUN));
            this.bntOpen = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelSeting = new System.Windows.Forms.Panel();
            this.radioKamera = new System.Windows.Forms.RadioButton();
            this.radioVidieo = new System.Windows.Forms.RadioButton();
            this.pBox = new System.Windows.Forms.PictureBox();
            this.panelSeting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // bntOpen
            // 
            this.bntOpen.Location = new System.Drawing.Point(24, 35);
            this.bntOpen.Margin = new System.Windows.Forms.Padding(2);
            this.bntOpen.Name = "bntOpen";
            this.bntOpen.Size = new System.Drawing.Size(117, 43);
            this.bntOpen.TabIndex = 2;
            this.bntOpen.Text = "Deteksi Citra ";
            this.bntOpen.UseVisualStyleBackColor = true;
            this.bntOpen.Click += new System.EventHandler(this.bntOpen_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(17, 53);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(159, 43);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(725, 97);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Frame Perdetik";
            // 
            // btnClose
            // 
            this.btnClose.Enabled = false;
            this.btnClose.Location = new System.Drawing.Point(180, 54);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(162, 42);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Stop";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // panelSeting
            // 
            this.panelSeting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSeting.Controls.Add(this.radioKamera);
            this.panelSeting.Controls.Add(this.btnClose);
            this.panelSeting.Controls.Add(this.radioVidieo);
            this.panelSeting.Controls.Add(this.btnPlay);
            this.panelSeting.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelSeting.Location = new System.Drawing.Point(897, 13);
            this.panelSeting.Name = "panelSeting";
            this.panelSeting.Size = new System.Drawing.Size(360, 103);
            this.panelSeting.TabIndex = 7;
            // 
            // radioKamera
            // 
            this.radioKamera.AutoSize = true;
            this.radioKamera.Location = new System.Drawing.Point(180, 21);
            this.radioKamera.Name = "radioKamera";
            this.radioKamera.Size = new System.Drawing.Size(61, 17);
            this.radioKamera.TabIndex = 1;
            this.radioKamera.TabStop = true;
            this.radioKamera.Text = "Kamera";
            this.radioKamera.UseVisualStyleBackColor = true;
            this.radioKamera.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioVidieo
            // 
            this.radioVidieo.AutoSize = true;
            this.radioVidieo.Location = new System.Drawing.Point(30, 21);
            this.radioVidieo.Name = "radioVidieo";
            this.radioVidieo.Size = new System.Drawing.Size(90, 17);
            this.radioVidieo.TabIndex = 0;
            this.radioVidieo.TabStop = true;
            this.radioVidieo.Text = "Pemutar Vidio";
            this.radioVidieo.UseVisualStyleBackColor = true;
            this.radioVidieo.CheckedChanged += new System.EventHandler(this.radioVidieo_CheckedChanged);
            // 
            // pBox
            // 
            this.pBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pBox.Location = new System.Drawing.Point(9, 121);
            this.pBox.Margin = new System.Windows.Forms.Padding(2);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(1249, 699);
            this.pBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBox.TabIndex = 0;
            this.pBox.TabStop = false;
            // 
            // SUN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 831);
            this.Controls.Add(this.panelSeting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bntOpen);
            this.Controls.Add(this.pBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SUN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scene Understanding";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SUN_FormClosing);
            this.panelSeting.ResumeLayout(false);
            this.panelSeting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.Button bntOpen;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelSeting;
        private System.Windows.Forms.RadioButton radioVidieo;
        private System.Windows.Forms.RadioButton radioKamera;
    }
}

