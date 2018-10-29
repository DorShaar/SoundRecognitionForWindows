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
               this.TurnOnOffButton = new System.Windows.Forms.Button();
               this.WorkingDirectoryTextBox = new System.Windows.Forms.TextBox();
               this.DBLabel = new System.Windows.Forms.Label();
               this.SetWorkingDirectoryButton = new System.Windows.Forms.Button();
               this.ScanItemButton = new System.Windows.Forms.Button();
               this.StartMachineButton = new System.Windows.Forms.Button();
               this.CurrentItemLabel = new System.Windows.Forms.Label();
               this.CurrentItemNameLabel = new System.Windows.Forms.Label();
               this.mFFTVisual = new SoundRecognition.SoundVisualizationUserControl();
               this.SuspendLayout();
               // 
               // TurnOnOffButton
               // 
               this.TurnOnOffButton.Location = new System.Drawing.Point(206, 82);
               this.TurnOnOffButton.Name = "TurnOnOffButton";
               this.TurnOnOffButton.Size = new System.Drawing.Size(156, 45);
               this.TurnOnOffButton.TabIndex = 0;
               this.TurnOnOffButton.Text = "Turn-On";
               this.TurnOnOffButton.UseVisualStyleBackColor = true;
               this.TurnOnOffButton.Click += new System.EventHandler(this.TurnOnButton_Click);
               // 
               // WorkingDirectoryTextBox
               // 
               this.WorkingDirectoryTextBox.Location = new System.Drawing.Point(202, 22);
               this.WorkingDirectoryTextBox.Name = "WorkingDirectoryTextBox";
               this.WorkingDirectoryTextBox.Size = new System.Drawing.Size(902, 26);
               this.WorkingDirectoryTextBox.TabIndex = 1;
               // 
               // DBLabel
               // 
               this.DBLabel.AutoSize = true;
               this.DBLabel.Location = new System.Drawing.Point(45, 22);
               this.DBLabel.Name = "DBLabel";
               this.DBLabel.Size = new System.Drawing.Size(138, 20);
               this.DBLabel.TabIndex = 2;
               this.DBLabel.Text = "Working Directory:";
               // 
               // SetWorkingDirectoryButton
               // 
               this.SetWorkingDirectoryButton.Location = new System.Drawing.Point(1126, 22);
               this.SetWorkingDirectoryButton.Name = "SetWorkingDirectoryButton";
               this.SetWorkingDirectoryButton.Size = new System.Drawing.Size(81, 38);
               this.SetWorkingDirectoryButton.TabIndex = 3;
               this.SetWorkingDirectoryButton.Text = "Change";
               this.SetWorkingDirectoryButton.UseVisualStyleBackColor = true;
               this.SetWorkingDirectoryButton.Click += new System.EventHandler(this.SetWorkingDirectoryButton_Click);
               // 
               // ScanItemButton
               // 
               this.ScanItemButton.Location = new System.Drawing.Point(452, 82);
               this.ScanItemButton.Name = "ScanItemButton";
               this.ScanItemButton.Size = new System.Drawing.Size(156, 45);
               this.ScanItemButton.TabIndex = 0;
               this.ScanItemButton.Text = "Scan Item";
               this.ScanItemButton.UseVisualStyleBackColor = true;
               this.ScanItemButton.Visible = false;
               this.ScanItemButton.Click += new System.EventHandler(this.ScanItemButton_Click);
               // 
               // StartMachineButton
               // 
               this.StartMachineButton.Location = new System.Drawing.Point(688, 82);
               this.StartMachineButton.Name = "StartMachineButton";
               this.StartMachineButton.Size = new System.Drawing.Size(156, 45);
               this.StartMachineButton.TabIndex = 0;
               this.StartMachineButton.Text = "Start";
               this.StartMachineButton.UseVisualStyleBackColor = true;
               this.StartMachineButton.Visible = false;
               this.StartMachineButton.Click += new System.EventHandler(this.StartMachineButton_Click);
               // 
               // CurrentItemLabel
               // 
               this.CurrentItemLabel.AutoSize = true;
               this.CurrentItemLabel.Location = new System.Drawing.Point(862, 94);
               this.CurrentItemLabel.Name = "CurrentItemLabel";
               this.CurrentItemLabel.Size = new System.Drawing.Size(102, 20);
               this.CurrentItemLabel.TabIndex = 2;
               this.CurrentItemLabel.Text = "Current Item:";
               // 
               // CurrentItemNameLabel
               // 
               this.CurrentItemNameLabel.AutoSize = true;
               this.CurrentItemNameLabel.Location = new System.Drawing.Point(970, 94);
               this.CurrentItemNameLabel.Name = "CurrentItemNameLabel";
               this.CurrentItemNameLabel.Size = new System.Drawing.Size(47, 20);
               this.CurrentItemNameLabel.TabIndex = 2;
               this.CurrentItemNameLabel.Text = "None";
               // 
               // mFFTVisual
               // 
               this.mFFTVisual.Location = new System.Drawing.Point(-3, 162);
               this.mFFTVisual.Name = "mFFTVisual";
               this.mFFTVisual.Size = new System.Drawing.Size(1210, 356);
               this.mFFTVisual.TabIndex = 6;
               // 
               // MachineUI
               // 
               this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
               this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(1220, 513);
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
               this.MaximizeBox = false;
               this.Name = "MachineUI";
               this.ShowIcon = false;
               this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
               this.Text = "MachineUI";
               this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MachineUI_FormClosed);
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
     }
}