namespace Test_SmartHome
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox_mail = new System.Windows.Forms.TextBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.serialPortESP32CAM = new System.IO.Ports.SerialPort(this.components);
            this.btnsend_mail = new System.Windows.Forms.Button();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.pictureBox1);
            this.groupBox8.Controls.Add(this.btnsend_mail);
            this.groupBox8.Controls.Add(this.textBox_mail);
            this.groupBox8.Controls.Add(this.pictureBox11);
            this.groupBox8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox8.Location = new System.Drawing.Point(12, 12);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(437, 135);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "E-MAIL";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(112, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(142, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // textBox_mail
            // 
            this.textBox_mail.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox_mail.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.textBox_mail.Location = new System.Drawing.Point(260, 22);
            this.textBox_mail.Name = "textBox_mail";
            this.textBox_mail.Size = new System.Drawing.Size(162, 24);
            this.textBox_mail.TabIndex = 4;
            this.textBox_mail.TabStop = false;
            // 
            // pictureBox11
            // 
            this.pictureBox11.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox11.Image")));
            this.pictureBox11.Location = new System.Drawing.Point(6, 22);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(100, 100);
            this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox11.TabIndex = 0;
            this.pictureBox11.TabStop = false;
            // 
            // btnsend_mail
            // 
            this.btnsend_mail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnsend_mail.BackgroundImage")));
            this.btnsend_mail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsend_mail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnsend_mail.ForeColor = System.Drawing.Color.White;
            this.btnsend_mail.Location = new System.Drawing.Point(284, 52);
            this.btnsend_mail.Name = "btnsend_mail";
            this.btnsend_mail.Size = new System.Drawing.Size(138, 25);
            this.btnsend_mail.TabIndex = 5;
            this.btnsend_mail.TabStop = false;
            this.btnsend_mail.Text = "SEND MAIL";
            this.btnsend_mail.UseVisualStyleBackColor = true;
            this.btnsend_mail.Click += new System.EventHandler(this.btnsend_mail_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(463, 160);
            this.Controls.Add(this.groupBox8);
            this.Name = "Form3";
            this.Text = "SEND PHOTO TO E-MAIL";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox textBox_mail;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.IO.Ports.SerialPort serialPortESP32CAM;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnsend_mail;
    }
}