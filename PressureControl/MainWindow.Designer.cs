namespace PressureControl
{
    partial class MainWindow
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
            this.CheckBox_TakeControl = new System.Windows.Forms.CheckBox();
            this.Label_GlobalPressureText = new System.Windows.Forms.Label();
            this.GlobalPressureControl = new System.Windows.Forms.NumericUpDown();
            this.Label_MeasuredPressureText = new System.Windows.Forms.Label();
            this.MeasuredPressureViewer = new System.Windows.Forms.NumericUpDown();
            this.Label_VentValveStateText = new System.Windows.Forms.Label();
            this.VentValveStateViewer = new System.Windows.Forms.ComboBox();
            this.Label_AirPumpStateText = new System.Windows.Forms.Label();
            this.AirPumpStateViewer = new System.Windows.Forms.ComboBox();
            this.RemoteControlStatusViewer = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.GlobalPressureControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasuredPressureViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // CheckBox_TakeControl
            // 
            this.CheckBox_TakeControl.AutoSize = true;
            this.CheckBox_TakeControl.Location = new System.Drawing.Point(12, 12);
            this.CheckBox_TakeControl.Name = "CheckBox_TakeControl";
            this.CheckBox_TakeControl.Size = new System.Drawing.Size(101, 17);
            this.CheckBox_TakeControl.TabIndex = 0;
            this.CheckBox_TakeControl.Text = "Control Enabled";
            this.CheckBox_TakeControl.UseVisualStyleBackColor = true;
            this.CheckBox_TakeControl.CheckedChanged += new System.EventHandler(this.CheckBox_TakeControl_CheckedChanged);
            // 
            // Label_GlobalPressureText
            // 
            this.Label_GlobalPressureText.AutoSize = true;
            this.Label_GlobalPressureText.Location = new System.Drawing.Point(9, 32);
            this.Label_GlobalPressureText.Name = "Label_GlobalPressureText";
            this.Label_GlobalPressureText.Size = new System.Drawing.Size(106, 13);
            this.Label_GlobalPressureText.TabIndex = 1;
            this.Label_GlobalPressureText.Text = "Global Pressure (Bar)";
            // 
            // GlobalPressureControl
            // 
            this.GlobalPressureControl.BackColor = System.Drawing.Color.Red;
            this.GlobalPressureControl.Enabled = false;
            this.GlobalPressureControl.ForeColor = System.Drawing.Color.Black;
            this.GlobalPressureControl.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.GlobalPressureControl.Location = new System.Drawing.Point(153, 30);
            this.GlobalPressureControl.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.GlobalPressureControl.Name = "GlobalPressureControl";
            this.GlobalPressureControl.Size = new System.Drawing.Size(171, 20);
            this.GlobalPressureControl.TabIndex = 3;
            this.GlobalPressureControl.ValueChanged += new System.EventHandler(this.GlobalPressureControl_ValueChanged);
            // 
            // Label_MeasuredPressureText
            // 
            this.Label_MeasuredPressureText.AutoSize = true;
            this.Label_MeasuredPressureText.Location = new System.Drawing.Point(9, 57);
            this.Label_MeasuredPressureText.Name = "Label_MeasuredPressureText";
            this.Label_MeasuredPressureText.Size = new System.Drawing.Size(139, 13);
            this.Label_MeasuredPressureText.TabIndex = 4;
            this.Label_MeasuredPressureText.Text = "Measured Pressure (milliBar)";
            // 
            // MeasuredPressureViewer
            // 
            this.MeasuredPressureViewer.BackColor = System.Drawing.Color.Red;
            this.MeasuredPressureViewer.Enabled = false;
            this.MeasuredPressureViewer.ForeColor = System.Drawing.Color.Black;
            this.MeasuredPressureViewer.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MeasuredPressureViewer.Location = new System.Drawing.Point(153, 55);
            this.MeasuredPressureViewer.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.MeasuredPressureViewer.Name = "MeasuredPressureViewer";
            this.MeasuredPressureViewer.Size = new System.Drawing.Size(171, 20);
            this.MeasuredPressureViewer.TabIndex = 5;
            // 
            // Label_VentValveStateText
            // 
            this.Label_VentValveStateText.AutoSize = true;
            this.Label_VentValveStateText.Location = new System.Drawing.Point(9, 84);
            this.Label_VentValveStateText.Name = "Label_VentValveStateText";
            this.Label_VentValveStateText.Size = new System.Drawing.Size(87, 13);
            this.Label_VentValveStateText.TabIndex = 6;
            this.Label_VentValveStateText.Text = "Vent Valve State";
            // 
            // VentValveStateViewer
            // 
            this.VentValveStateViewer.BackColor = System.Drawing.Color.Red;
            this.VentValveStateViewer.Enabled = false;
            this.VentValveStateViewer.FormattingEnabled = true;
            this.VentValveStateViewer.Location = new System.Drawing.Point(153, 81);
            this.VentValveStateViewer.Name = "VentValveStateViewer";
            this.VentValveStateViewer.Size = new System.Drawing.Size(171, 21);
            this.VentValveStateViewer.TabIndex = 7;
            // 
            // Label_AirPumpStateText
            // 
            this.Label_AirPumpStateText.AutoSize = true;
            this.Label_AirPumpStateText.Location = new System.Drawing.Point(9, 111);
            this.Label_AirPumpStateText.Name = "Label_AirPumpStateText";
            this.Label_AirPumpStateText.Size = new System.Drawing.Size(77, 13);
            this.Label_AirPumpStateText.TabIndex = 8;
            this.Label_AirPumpStateText.Text = "Air Pump State";
            // 
            // AirPumpStateViewer
            // 
            this.AirPumpStateViewer.BackColor = System.Drawing.Color.Red;
            this.AirPumpStateViewer.Enabled = false;
            this.AirPumpStateViewer.FormattingEnabled = true;
            this.AirPumpStateViewer.Location = new System.Drawing.Point(153, 108);
            this.AirPumpStateViewer.Name = "AirPumpStateViewer";
            this.AirPumpStateViewer.Size = new System.Drawing.Size(171, 21);
            this.AirPumpStateViewer.TabIndex = 9;
            // 
            // RemoteControlStatusViewer
            // 
            this.RemoteControlStatusViewer.AutoSize = true;
            this.RemoteControlStatusViewer.Enabled = false;
            this.RemoteControlStatusViewer.Location = new System.Drawing.Point(192, 12);
            this.RemoteControlStatusViewer.Name = "RemoteControlStatusViewer";
            this.RemoteControlStatusViewer.Size = new System.Drawing.Size(132, 17);
            this.RemoteControlStatusViewer.TabIndex = 10;
            this.RemoteControlStatusViewer.Text = "Remote Control Status";
            this.RemoteControlStatusViewer.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 144);
            this.Controls.Add(this.RemoteControlStatusViewer);
            this.Controls.Add(this.AirPumpStateViewer);
            this.Controls.Add(this.Label_AirPumpStateText);
            this.Controls.Add(this.VentValveStateViewer);
            this.Controls.Add(this.Label_VentValveStateText);
            this.Controls.Add(this.MeasuredPressureViewer);
            this.Controls.Add(this.Label_MeasuredPressureText);
            this.Controls.Add(this.GlobalPressureControl);
            this.Controls.Add(this.Label_GlobalPressureText);
            this.Controls.Add(this.CheckBox_TakeControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Decompression Chamber Utility";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GlobalPressureControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeasuredPressureViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CheckBox_TakeControl;
        private System.Windows.Forms.Label Label_GlobalPressureText;
        private System.Windows.Forms.NumericUpDown GlobalPressureControl;
        private System.Windows.Forms.Label Label_MeasuredPressureText;
        private System.Windows.Forms.NumericUpDown MeasuredPressureViewer;
        private System.Windows.Forms.Label Label_VentValveStateText;
        private System.Windows.Forms.ComboBox VentValveStateViewer;
        private System.Windows.Forms.Label Label_AirPumpStateText;
        private System.Windows.Forms.ComboBox AirPumpStateViewer;
        private System.Windows.Forms.CheckBox RemoteControlStatusViewer;
    }
}

