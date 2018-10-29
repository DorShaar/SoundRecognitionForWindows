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
               this.BarcodePictureBox.Location = new System.Drawing.Point(228, 54);
               this.BarcodePictureBox.Name = "BarcodePictureBox";
               this.BarcodePictureBox.Size = new System.Drawing.Size(301, 267);
               this.BarcodePictureBox.TabIndex = 0;
               this.BarcodePictureBox.TabStop = false;
               // 
               // ScanBarcodeButton
               // 
               this.ScanBarcodeButton.Location = new System.Drawing.Point(26, 171);
               this.ScanBarcodeButton.Name = "ScanBarcodeButton";
               this.ScanBarcodeButton.Size = new System.Drawing.Size(175, 36);
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
               this.NextImageButton.Location = new System.Drawing.Point(420, 327);
               this.NextImageButton.Name = "NextImageButton";
               this.NextImageButton.Size = new System.Drawing.Size(44, 30);
               this.NextImageButton.TabIndex = 2;
               this.NextImageButton.Text = ">";
               this.NextImageButton.UseVisualStyleBackColor = true;
               this.NextImageButton.Click += new System.EventHandler(this.NextImageButton_Click);
               // 
               // AddNewItemButton
               // 
               this.AddNewItemButton.Location = new System.Drawing.Point(26, 67);
               this.AddNewItemButton.Name = "AddNewItemButton";
               this.AddNewItemButton.Size = new System.Drawing.Size(175, 36);
               this.AddNewItemButton.TabIndex = 1;
               this.AddNewItemButton.Text = "Add New Item";
               this.AddNewItemButton.UseVisualStyleBackColor = true;
               this.AddNewItemButton.Click += new System.EventHandler(this.AddNewItemButton_Click);
               // 
               // PreviousImageButton
               // 
               this.PreviousImageButton.Location = new System.Drawing.Point(301, 327);
               this.PreviousImageButton.Name = "PreviousImageButton";
               this.PreviousImageButton.Size = new System.Drawing.Size(44, 30);
               this.PreviousImageButton.TabIndex = 2;
               this.PreviousImageButton.Text = "<";
               this.PreviousImageButton.UseVisualStyleBackColor = true;
               this.PreviousImageButton.Click += new System.EventHandler(this.PreviousImageButton_Click);
               // 
               // BarcodeNameLable
               // 
               this.BarcodeNameLable.AutoSize = true;
               this.BarcodeNameLable.Location = new System.Drawing.Point(297, 21);
               this.BarcodeNameLable.Name = "BarcodeNameLable";
               this.BarcodeNameLable.Size = new System.Drawing.Size(149, 20);
               this.BarcodeNameLable.TabIndex = 3;
               this.BarcodeNameLable.Text = "No Picture Available";
               // 
               // CloseButton
               // 
               this.CloseButton.Location = new System.Drawing.Point(26, 285);
               this.CloseButton.Name = "CloseButton";
               this.CloseButton.Size = new System.Drawing.Size(78, 36);
               this.CloseButton.TabIndex = 1;
               this.CloseButton.Text = "Close";
               this.CloseButton.UseVisualStyleBackColor = true;
               this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
               // 
               // ScannerUI
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(541, 379);
               this.Controls.Add(this.BarcodeNameLable);
               this.Controls.Add(this.PreviousImageButton);
               this.Controls.Add(this.NextImageButton);
               this.Controls.Add(this.AddNewItemButton);
               this.Controls.Add(this.CloseButton);
               this.Controls.Add(this.ScanBarcodeButton);
               this.Controls.Add(this.BarcodePictureBox);
               this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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