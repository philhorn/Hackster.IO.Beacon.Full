namespace BeaconDeveloperDashboard
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstDevices = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.txtLogbox = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lstDisplays = new System.Windows.Forms.ComboBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtZipCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.lstDevices);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(719, 391);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LightBeacon Controls";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a LightBeacon";
            // 
            // lstDevices
            // 
            this.lstDevices.FormattingEnabled = true;
            this.lstDevices.Location = new System.Drawing.Point(10, 70);
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(344, 33);
            this.lstDevices.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Image = global::BeaconDeveloperDashboard.Properties.Resources.Double_Tap_icon;
            this.button3.Location = new System.Drawing.Point(360, 191);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(342, 76);
            this.button3.TabIndex = 3;
            this.button3.Text = "Clear Alerts";
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtLogbox
            // 
            this.txtLogbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtLogbox.ForeColor = System.Drawing.Color.Aqua;
            this.txtLogbox.Location = new System.Drawing.Point(822, 12);
            this.txtLogbox.Name = "txtLogbox";
            this.txtLogbox.Size = new System.Drawing.Size(970, 706);
            this.txtLogbox.TabIndex = 1;
            this.txtLogbox.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtZipCode);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lstDisplays);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Location = new System.Drawing.Point(12, 419);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(719, 299);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DisplayBeacon Controls";
            // 
            // button5
            // 
            this.button5.Image = global::BeaconDeveloperDashboard.Properties.Resources.Communication_Tower_icon;
            this.button5.Location = new System.Drawing.Point(12, 273);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(342, 76);
            this.button5.TabIndex = 4;
            this.button5.Text = "Start Sensors";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Select a DisplayBeacon";
            // 
            // lstDisplays
            // 
            this.lstDisplays.FormattingEnabled = true;
            this.lstDisplays.Location = new System.Drawing.Point(10, 70);
            this.lstDisplays.Name = "lstDisplays";
            this.lstDisplays.Size = new System.Drawing.Size(344, 33);
            this.lstDisplays.TabIndex = 1;
            // 
            // button9
            // 
            this.button9.Image = global::BeaconDeveloperDashboard.Properties.Resources.Communication_Tower_2_icon;
            this.button9.Location = new System.Drawing.Point(360, 273);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(342, 76);
            this.button9.TabIndex = 5;
            this.button9.Text = "Stop Sensors";
            this.button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Image = global::BeaconDeveloperDashboard.Properties.Resources.Weather_Storm_icon1;
            this.button7.Location = new System.Drawing.Point(360, 109);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(342, 76);
            this.button7.TabIndex = 2;
            this.button7.Text = "Test Weather Alert";
            this.button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button8.Image = global::BeaconDeveloperDashboard.Properties.Resources.Police_icon;
            this.button8.Location = new System.Drawing.Point(12, 109);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(342, 76);
            this.button8.TabIndex = 1;
            this.button8.Text = "Test Amber Alert";
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button4
            // 
            this.button4.Image = global::BeaconDeveloperDashboard.Properties.Resources.Gaugage_icon;
            this.button4.Location = new System.Drawing.Point(12, 191);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(342, 76);
            this.button4.TabIndex = 4;
            this.button4.Text = "Random Color Test";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Image = global::BeaconDeveloperDashboard.Properties.Resources.Weather_Storm_icon1;
            this.button2.Location = new System.Drawing.Point(360, 109);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(342, 76);
            this.button2.TabIndex = 2;
            this.button2.Text = "Test Weather Alert LEDS";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Image = global::BeaconDeveloperDashboard.Properties.Resources.Police_icon;
            this.button1.Location = new System.Drawing.Point(12, 109);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(342, 76);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test Amber Alert LEDS";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(602, 70);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(100, 31);
            this.txtZipCode.TabIndex = 6;
            this.txtZipCode.Text = "17001";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(416, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Enter a Zip Code:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1804, 913);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtLogbox);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Beacon Control Dashboard -Dev Mode-";
            this.Load += new System.EventHandler(this.Form1_LoadAsync);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstDevices;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox txtLogbox;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox lstDisplays;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtZipCode;
    }
}

