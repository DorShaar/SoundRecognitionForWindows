namespace SoundRecognition
{
    partial class ScannerUserInput
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
            this.AddButton = new System.Windows.Forms.Button();
            this.ProductNameLable = new System.Windows.Forms.Label();
            this.ProductNameTextBox = new System.Windows.Forms.TextBox();
            this.MaxHeatingTimeLable = new System.Windows.Forms.Label();
            this.MaxHeatTimeTextBoxUpDown = new System.Windows.Forms.NumericUpDown();
            this.MaxHeatTimeTextBoxUpDown.Maximum = decimal.MaxValue;
            this.RecognitionTypeLabel = new System.Windows.Forms.Label();
            this.CategoryLabel = new System.Windows.Forms.Label();
            this.CategoryTextBox = new System.Windows.Forms.TextBox();
            this.RecognitionTypeComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.MaxHeatTimeTextBoxUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(184, 212);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(198, 36);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // ProductNameLable
            // 
            this.ProductNameLable.AutoSize = true;
            this.ProductNameLable.Location = new System.Drawing.Point(39, 32);
            this.ProductNameLable.Name = "ProductNameLable";
            this.ProductNameLable.Size = new System.Drawing.Size(114, 20);
            this.ProductNameLable.TabIndex = 1;
            this.ProductNameLable.Text = "Product Name:";
            // 
            // ProductNameTextBox
            // 
            this.ProductNameTextBox.Location = new System.Drawing.Point(159, 32);
            this.ProductNameTextBox.Name = "ProductNameTextBox";
            this.ProductNameTextBox.Size = new System.Drawing.Size(358, 26);
            this.ProductNameTextBox.TabIndex = 2;
            this.ProductNameTextBox.TextChanged += new System.EventHandler(this.ProductNameTextBox_TextChanged);
            // 
            // MaxHeatingTimeLable
            // 
            this.MaxHeatingTimeLable.AutoSize = true;
            this.MaxHeatingTimeLable.Location = new System.Drawing.Point(39, 78);
            this.MaxHeatingTimeLable.Name = "MaxHeatingTimeLable";
            this.MaxHeatingTimeLable.Size = new System.Drawing.Size(197, 20);
            this.MaxHeatingTimeLable.TabIndex = 3;
            this.MaxHeatingTimeLable.Text = "Max Heating Time (sec):";
            // 
            // MaxHeatTimeTextBoxUpDown
            // 
            this.MaxHeatTimeTextBoxUpDown.Location = new System.Drawing.Point(252, 78);
            this.MaxHeatTimeTextBoxUpDown.Name = "MaxHeatTimeTextBoxUpDown";
            this.MaxHeatTimeTextBoxUpDown.Size = new System.Drawing.Size(76, 26);
            this.MaxHeatTimeTextBoxUpDown.TabIndex = 5;
            this.MaxHeatTimeTextBoxUpDown.ValueChanged += new System.EventHandler(this.MaxHeatTimeTextBoxUpDown_ValueChanged);
            // 
            // RecognitionTypeLabel
            // 
            this.RecognitionTypeLabel.AutoSize = true;
            this.RecognitionTypeLabel.Location = new System.Drawing.Point(39, 120);
            this.RecognitionTypeLabel.Name = "RecognitionTypeLabel";
            this.RecognitionTypeLabel.Size = new System.Drawing.Size(136, 20);
            this.RecognitionTypeLabel.TabIndex = 1;
            this.RecognitionTypeLabel.Text = "Recognition Type:";
            // 
            // CategoryLabel
            // 
            this.CategoryLabel.AutoSize = true;
            this.CategoryLabel.Location = new System.Drawing.Point(39, 164);
            this.CategoryLabel.Name = "CategoryLabel";
            this.CategoryLabel.Size = new System.Drawing.Size(77, 20);
            this.CategoryLabel.TabIndex = 1;
            this.CategoryLabel.Text = "Category:";
            // 
            // CategoryTextBox
            // 
            this.CategoryTextBox.Location = new System.Drawing.Point(184, 164);
            this.CategoryTextBox.Name = "CategoryTextBox";
            this.CategoryTextBox.Size = new System.Drawing.Size(206, 26);
            this.CategoryTextBox.TabIndex = 2;
            this.CategoryTextBox.TextChanged += new System.EventHandler(this.CategoryTextBox_TextChanged);
            // 
            // RecognitionTypeComboBox
            // 
            this.RecognitionTypeComboBox.FormattingEnabled = true;
            this.RecognitionTypeComboBox.Items.AddRange(new object[] {
            "SpecificSoundRequired",
            "Popcorn"});
            this.RecognitionTypeComboBox.Location = new System.Drawing.Point(184, 120);
            this.RecognitionTypeComboBox.Name = "RecognitionTypeComboBox";
            this.RecognitionTypeComboBox.Size = new System.Drawing.Size(206, 28);
            this.RecognitionTypeComboBox.TabIndex = 7;
            this.RecognitionTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.RecognitionTypeComboBox_SelectedIndexChanged);
            // 
            // ScannerUserInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 263);
            this.Controls.Add(this.RecognitionTypeComboBox);
            this.Controls.Add(this.MaxHeatTimeTextBoxUpDown);
            this.Controls.Add(this.MaxHeatingTimeLable);
            this.Controls.Add(this.CategoryTextBox);
            this.Controls.Add(this.ProductNameTextBox);
            this.Controls.Add(this.CategoryLabel);
            this.Controls.Add(this.RecognitionTypeLabel);
            this.Controls.Add(this.ProductNameLable);
            this.Controls.Add(this.AddButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ScannerUserInput";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ScannerUserInput";
            ((System.ComponentModel.ISupportInitialize)(this.MaxHeatTimeTextBoxUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Label ProductNameLable;
        private System.Windows.Forms.TextBox ProductNameTextBox;
        private System.Windows.Forms.Label MaxHeatingTimeLable;
        private System.Windows.Forms.NumericUpDown MaxHeatTimeTextBoxUpDown;
        private System.Windows.Forms.Label RecognitionTypeLabel;
        private System.Windows.Forms.Label CategoryLabel;
        private System.Windows.Forms.TextBox CategoryTextBox;
        private System.Windows.Forms.ComboBox RecognitionTypeComboBox;
    }
}