namespace PushoverTestHarness
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxAppKey = new System.Windows.Forms.TextBox();
            this.textBoxUserKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPriority = new System.Windows.Forms.ComboBox();
            this.comboBoxSound = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownRetry = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownExpire = new System.Windows.Forms.NumericUpDown();
            this.textBoxUrlTitle = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExpire)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxMessage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxUserKey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxAppKey);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(585, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mandatory Items";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Application Key";
            // 
            // textBoxAppKey
            // 
            this.textBoxAppKey.Location = new System.Drawing.Point(101, 29);
            this.textBoxAppKey.Name = "textBoxAppKey";
            this.textBoxAppKey.Size = new System.Drawing.Size(183, 20);
            this.textBoxAppKey.TabIndex = 1;
            this.textBoxAppKey.Text = "LJNNaBNdqGKaVQeT38V8Y58EjMqA4d";
            // 
            // textBoxUserKey
            // 
            this.textBoxUserKey.Location = new System.Drawing.Point(388, 29);
            this.textBoxUserKey.Name = "textBoxUserKey";
            this.textBoxUserKey.Size = new System.Drawing.Size(183, 20);
            this.textBoxUserKey.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Your User Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Message Body";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(101, 63);
            this.textBoxMessage.MaxLength = 512;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(470, 20);
            this.textBoxMessage.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Priority";
            // 
            // comboBoxPriority
            // 
            this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPriority.FormattingEnabled = true;
            this.comboBoxPriority.Items.AddRange(new object[] {
            "Normal",
            "High",
            "Low",
            "Emergency"});
            this.comboBoxPriority.Location = new System.Drawing.Point(113, 136);
            this.comboBoxPriority.Name = "comboBoxPriority";
            this.comboBoxPriority.Size = new System.Drawing.Size(183, 21);
            this.comboBoxPriority.TabIndex = 2;
            // 
            // comboBoxSound
            // 
            this.comboBoxSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSound.FormattingEnabled = true;
            this.comboBoxSound.Items.AddRange(new object[] {
            "(Device default sound)",
            "Pushover (default)",
            "Bike",
            "Bugle",
            "Cash Register",
            "Classical",
            "Cosmic",
            "Falling",
            "Gamelan",
            "Incoming",
            "Intermission",
            "Magic",
            "Mechanical",
            "Piano Bar",
            "Siren",
            "Space Alarm",
            "Tug Boat",
            "Alien Alarm (long)",
            "Climb (long)",
            "Persistent (long)",
            "Pushover Echo (long)",
            "Up Down (long)",
            "None (silent)"});
            this.comboBoxSound.Location = new System.Drawing.Point(400, 136);
            this.comboBoxSound.Name = "comboBoxSound";
            this.comboBoxSound.Size = new System.Drawing.Size(183, 21);
            this.comboBoxSound.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(314, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Sound Effect";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(400, 165);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(183, 20);
            this.textBoxTitle.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(314, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Message Title";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Retry/Expire";
            // 
            // numericUpDownRetry
            // 
            this.numericUpDownRetry.Location = new System.Drawing.Point(113, 165);
            this.numericUpDownRetry.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numericUpDownRetry.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownRetry.Name = "numericUpDownRetry";
            this.numericUpDownRetry.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownRetry.TabIndex = 8;
            this.numericUpDownRetry.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // numericUpDownExpire
            // 
            this.numericUpDownExpire.Location = new System.Drawing.Point(213, 165);
            this.numericUpDownExpire.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.numericUpDownExpire.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownExpire.Name = "numericUpDownExpire";
            this.numericUpDownExpire.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownExpire.TabIndex = 9;
            this.numericUpDownExpire.Value = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            // 
            // textBoxUrlTitle
            // 
            this.textBoxUrlTitle.Location = new System.Drawing.Point(400, 194);
            this.textBoxUrlTitle.Name = "textBoxUrlTitle";
            this.textBoxUrlTitle.Size = new System.Drawing.Size(183, 20);
            this.textBoxUrlTitle.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(314, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "URL Title";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(113, 194);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(183, 20);
            this.textBoxUrl.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "URL";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(187, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Send Message";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(317, 230);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Quit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 270);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxUrlTitle);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numericUpDownExpire);
            this.Controls.Add(this.numericUpDownRetry);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxSound);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxPriority);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(625, 308);
            this.MinimumSize = new System.Drawing.Size(625, 308);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Pushover.NET Test Harness";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExpire)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUserKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAppKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPriority;
        private System.Windows.Forms.ComboBox comboBoxSound;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownRetry;
        private System.Windows.Forms.NumericUpDown numericUpDownExpire;
        private System.Windows.Forms.TextBox textBoxUrlTitle;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}

