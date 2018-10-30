namespace SoundRecognition
{
    partial class ScannerUI
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
            this.BarcodePictureBox = new System.Windows.Forms.PictureBox();
            this.ScanBarcodeButton = new System.Windows.Forms.Button();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.NextImageButton = new System.Windows.Forms.Button();
            this.AddNewItemButton = new System.Windows.Forms.Button();
            this.PreviousImageButton = new System.Windows.Forms.Button();
            this.BarcodeNameLable = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BarcodePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BarcodePictureBox
            // 
            this.BarcodePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BarcodePictureBox.Location = new System.Drawing.Point(152, 35);
            this.BarcodePictureBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BarcodePictureBox.Name = "BarcodePictureBox";
            this.BarcodePictureBox.Size = new System.Drawing.Size(201, 174);
            this.BarcodePictureBox.TabIndex = 0;
            this.BarcodePictureBox.TabStop = false;
            // 
            // ScanBarcodeButton
            // 
            this.ScanBarcodeButton.Location = new System.Drawing.Point(17, 62);
            this.ScanBarcodeButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ScanBarcodeButton.Name = "ScanBarcodeButton";
            this.ScanBarcodeButton.Size = new System.Drawing.Size(125, 23);
            this.ScanBarcodeButton.TabIndex = 1;
            this.ScanBarcodeButton.Text = "Scan Barcode";
            this.ScanBarcodeButton.UseVisualStyleBackColor = true;
            this.ScanBarcodeButton.Click += new System.EventHandler(this.ScanExistingBarcodeButton_Click);
            // 
            // ImageList
            // 
            this.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // NextImageButton
            // 
            this.NextImageButton.Location = new System.Drawing.Point(253, 213);
            this.NextImageButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NextImageButton.Name = "NextImageButton";
            this.NextImageButton.Size = new System.Drawing.Size(100, 25);
            this.NextImageButton.TabIndex = 2;
            this.NextImageButton.Text = ">";
            this.NextImageButton.UseVisualStyleBackColor = true;
            this.NextImageButton.Click += new System.EventHandler(this.NextImageButton_Click);
            // 
            // AddNewItemButton
            // 
            this.AddNewItemButton.Location = new System.Drawing.Point(17, 35);
            this.AddNewItemButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AddNewItemButton.Name = "AddNewItemButton";
            this.AddNewItemButton.Size = new System.Drawing.Size(125, 23);
            this.AddNewItemButton.TabIndex = 1;
            this.AddNewItemButton.Text = "Add New Item";
            this.AddNewItemButton.UseVisualStyleBackColor = true;
            this.AddNewItemButton.Click += new System.EventHandler(this.AddNewItemButton_Click);
            // 
            // PreviousImageButton
            // 
            this.PreviousImageButton.Location = new System.Drawing.Point(152, 213);
            this.PreviousImageButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PreviousImageButton.Name = "PreviousImageButton";
            this.PreviousImageButton.Size = new System.Drawing.Size(100, 25);
            this.PreviousImageButton.TabIndex = 2;
            this.PreviousImageButton.Text = "<";
            this.PreviousImageButton.UseVisualStyleBackColor = true;
            this.PreviousImageButton.Click += new System.EventHandler(this.PreviousImageButton_Click);
            // 
            // BarcodeNameLable
            // 
            this.BarcodeNameLable.AutoSize = true;
            this.BarcodeNameLable.Location = new System.Drawing.Point(198, 14);
            this.BarcodeNameLable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BarcodeNameLable.Name = "BarcodeNameLable";
            this.BarcodeNameLable.Size = new System.Drawing.Size(103, 13);
            this.BarcodeNameLable.TabIndex = 3;
            this.BarcodeNameLable.Text = "No Picture Available";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(17, 212);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(125, 25);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ScannerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 246);
            this.Controls.Add(this.BarcodeNameLable);
            this.Controls.Add(this.PreviousImageButton);
            this.Controls.Add(this.NextImageButton);
            this.Controls.Add(this.AddNewItemButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ScanBarcodeButton);
            this.Controls.Add(this.BarcodePictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MinimizeBox = false;
            this.Name = "ScannerUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ScannerUI";
            ((System.ComponentModel.ISupportInitialize)(this.BarcodePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox BarcodePictureBox;
        private System.Windows.Forms.Button ScanBarcodeButton;
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.Button NextImageButton;
        private System.Windows.Forms.Button AddNewItemButton;
        private System.Windows.Forms.Button PreviousImageButton;
        private System.Windows.Forms.Label BarcodeNameLable;
        private System.Windows.Forms.Button CloseButton;
    }
}