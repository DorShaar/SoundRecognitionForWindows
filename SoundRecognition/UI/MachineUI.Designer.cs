namespace SoundRecognition
{
    partial class MachineUI
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
            System.Windows.Forms.Label TitleLabel;
            this.TurnOnOffButton = new System.Windows.Forms.Button();
            this.WorkingDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.DBLabel = new System.Windows.Forms.Label();
            this.SetWorkingDirectoryButton = new System.Windows.Forms.Button();
            this.ScanItemButton = new System.Windows.Forms.Button();
            this.StartMachineButton = new System.Windows.Forms.Button();
            this.CurrentItemLabel = new System.Windows.Forms.Label();
            this.CurrentItemNameLabel = new System.Windows.Forms.Label();
            this.Banner = new System.Windows.Forms.Panel();
            this.mFFTVisual = new SoundRecognition.SoundVisualizationUserControl();
            TitleLabel = new System.Windows.Forms.Label();
            this.Banner.SuspendLayout();
            this.SuspendLayout();
            // 
            // TurnOnOffButton
            // 
            this.TurnOnOffButton.Location = new System.Drawing.Point(25, 110);
            this.TurnOnOffButton.Margin = new System.Windows.Forms.Padding(2);
            this.TurnOnOffButton.Name = "TurnOnOffButton";
            this.TurnOnOffButton.Size = new System.Drawing.Size(100, 25);
            this.TurnOnOffButton.TabIndex = 0;
            this.TurnOnOffButton.Text = "Turn-On";
            this.TurnOnOffButton.UseVisualStyleBackColor = true;
            this.TurnOnOffButton.Click += new System.EventHandler(this.TurnOnButton_Click);
            // 
            // WorkingDirectoryTextBox
            // 
            this.WorkingDirectoryTextBox.Location = new System.Drawing.Point(135, 84);
            this.WorkingDirectoryTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.WorkingDirectoryTextBox.Name = "WorkingDirectoryTextBox";
            this.WorkingDirectoryTextBox.Size = new System.Drawing.Size(603, 20);
            this.WorkingDirectoryTextBox.TabIndex = 1;
            // 
            // DBLabel
            // 
            this.DBLabel.AutoSize = true;
            this.DBLabel.Location = new System.Drawing.Point(22, 87);
            this.DBLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DBLabel.Name = "DBLabel";
            this.DBLabel.Size = new System.Drawing.Size(95, 13);
            this.DBLabel.TabIndex = 2;
            this.DBLabel.Text = "Working Directory:";
            // 
            // SetWorkingDirectoryButton
            // 
            this.SetWorkingDirectoryButton.Location = new System.Drawing.Point(751, 84);
            this.SetWorkingDirectoryButton.Margin = new System.Windows.Forms.Padding(2);
            this.SetWorkingDirectoryButton.Name = "SetWorkingDirectoryButton";
            this.SetWorkingDirectoryButton.Size = new System.Drawing.Size(54, 25);
            this.SetWorkingDirectoryButton.TabIndex = 3;
            this.SetWorkingDirectoryButton.Text = "Change";
            this.SetWorkingDirectoryButton.UseVisualStyleBackColor = true;
            this.SetWorkingDirectoryButton.Click += new System.EventHandler(this.SetWorkingDirectoryButton_Click);
            // 
            // ScanItemButton
            // 
            this.ScanItemButton.Location = new System.Drawing.Point(135, 110);
            this.ScanItemButton.Margin = new System.Windows.Forms.Padding(2);
            this.ScanItemButton.Name = "ScanItemButton";
            this.ScanItemButton.Size = new System.Drawing.Size(100, 25);
            this.ScanItemButton.TabIndex = 0;
            this.ScanItemButton.Text = "Scan Item";
            this.ScanItemButton.UseVisualStyleBackColor = true;
            this.ScanItemButton.Visible = false;
            this.ScanItemButton.Click += new System.EventHandler(this.ScanItemButton_Click);
            // 
            // StartMachineButton
            // 
            this.StartMachineButton.Location = new System.Drawing.Point(246, 110);
            this.StartMachineButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartMachineButton.Name = "StartMachineButton";
            this.StartMachineButton.Size = new System.Drawing.Size(100, 25);
            this.StartMachineButton.TabIndex = 0;
            this.StartMachineButton.Text = "Start";
            this.StartMachineButton.UseVisualStyleBackColor = true;
            this.StartMachineButton.Visible = false;
            this.StartMachineButton.Click += new System.EventHandler(this.StartMachineButton_Click);
            // 
            // CurrentItemLabel
            // 
            this.CurrentItemLabel.AutoSize = true;
            this.CurrentItemLabel.Location = new System.Drawing.Point(370, 120);
            this.CurrentItemLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentItemLabel.Name = "CurrentItemLabel";
            this.CurrentItemLabel.Size = new System.Drawing.Size(67, 13);
            this.CurrentItemLabel.TabIndex = 2;
            this.CurrentItemLabel.Text = "Current Item:";
            // 
            // CurrentItemNameLabel
            // 
            this.CurrentItemNameLabel.AutoSize = true;
            this.CurrentItemNameLabel.Location = new System.Drawing.Point(442, 120);
            this.CurrentItemNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentItemNameLabel.Name = "CurrentItemNameLabel";
            this.CurrentItemNameLabel.Size = new System.Drawing.Size(33, 13);
            this.CurrentItemNameLabel.TabIndex = 2;
            this.CurrentItemNameLabel.Text = "None";
            // 
            // Banner
            // 
            this.Banner.BackgroundImage = global::SoundRecognition.Properties.Resources.background_img;
            this.Banner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Banner.Controls.Add(TitleLabel);
            this.Banner.Dock = System.Windows.Forms.DockStyle.Top;
            this.Banner.Location = new System.Drawing.Point(4, 4);
            this.Banner.Margin = new System.Windows.Forms.Padding(4);
            this.Banner.Name = "Banner";
            this.Banner.Size = new System.Drawing.Size(816, 72);
            this.Banner.TabIndex = 7;
            // 
            // TitleLabel
            // 
            TitleLabel.BackColor = System.Drawing.SystemColors.Control;
            TitleLabel.Cursor = System.Windows.Forms.Cursors.Default;
            TitleLabel.Font = new System.Drawing.Font("Monotype Hadassah", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            TitleLabel.ForeColor = System.Drawing.Color.CadetBlue;
            TitleLabel.Image = global::SoundRecognition.Properties.Resources.background_img;
            TitleLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            TitleLabel.Location = new System.Drawing.Point(2, 0);
            TitleLabel.Margin = new System.Windows.Forms.Padding(3);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(810, 70);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "Auto Cooking Microwave Oven System";
            TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            TitleLabel.UseMnemonic = false;
            // 
            // mFFTVisual
            // 
            this.mFFTVisual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mFFTVisual.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mFFTVisual.Location = new System.Drawing.Point(4, 158);
            this.mFFTVisual.Margin = new System.Windows.Forms.Padding(4);
            this.mFFTVisual.Name = "mFFTVisual";
            this.mFFTVisual.Size = new System.Drawing.Size(816, 220);
            this.mFFTVisual.TabIndex = 6;
            // 
            // MachineUI
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 382);
            this.Controls.Add(this.Banner);
            this.Controls.Add(this.mFFTVisual);
            this.Controls.Add(this.SetWorkingDirectoryButton);
            this.Controls.Add(this.CurrentItemNameLabel);
            this.Controls.Add(this.CurrentItemLabel);
            this.Controls.Add(this.DBLabel);
            this.Controls.Add(this.WorkingDirectoryTextBox);
            this.Controls.Add(this.StartMachineButton);
            this.Controls.Add(this.ScanItemButton);
            this.Controls.Add(this.TurnOnOffButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MachineUI";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MachineUI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MachineUI_FormClosed);
            this.Banner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TurnOnOffButton;
        private System.Windows.Forms.TextBox WorkingDirectoryTextBox;
        private System.Windows.Forms.Label DBLabel;
        private System.Windows.Forms.Button SetWorkingDirectoryButton;
        private System.Windows.Forms.Button ScanItemButton;
        private System.Windows.Forms.Button StartMachineButton;
        private System.Windows.Forms.Label CurrentItemLabel;
        private System.Windows.Forms.Label CurrentItemNameLabel;
        private SoundVisualizationUserControl mFFTVisual;
        private System.Windows.Forms.Panel Banner;
    }
}