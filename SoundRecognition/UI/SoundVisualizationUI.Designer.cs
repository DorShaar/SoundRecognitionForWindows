namespace ScottPlotMicrophoneFFT
{
    partial class SoundVisualizationUI
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
               this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
               this.mPCMVisual = new ScottPlot.SoundVisualizationUserControl("PCM");
               this.mFFTVisual = new ScottPlot.SoundVisualizationUserControl("FFT");
               this.statusStrip1 = new System.Windows.Forms.StatusStrip();
               this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
               this.menuStrip1 = new System.Windows.Forms.MenuStrip();
               this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
               this.autoScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
               this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
               this.websiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
               this.infoMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
               this.timer1 = new System.Windows.Forms.Timer(this.components);
               this.tableLayoutPanel1.SuspendLayout();
               this.statusStrip1.SuspendLayout();
               this.menuStrip1.SuspendLayout();
               this.SuspendLayout();
               // 
               // tableLayoutPanel1
               // 
               this.tableLayoutPanel1.ColumnCount = 1;
               this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
               this.tableLayoutPanel1.Controls.Add(this.mPCMVisual, 0, 0);
               this.tableLayoutPanel1.Controls.Add(this.mFFTVisual, 0, 1);
               this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
               this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
               this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
               this.tableLayoutPanel1.Name = "tableLayoutPanel1";
               this.tableLayoutPanel1.RowCount = 2;
               this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
               this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
               this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
               this.tableLayoutPanel1.Size = new System.Drawing.Size(1368, 1034);
               this.tableLayoutPanel1.TabIndex = 0;
               // 
               // mPCMVisual
               // 
               this.mPCMVisual.Dock = System.Windows.Forms.DockStyle.Fill;
               this.mPCMVisual.Location = new System.Drawing.Point(3, 3);
               this.mPCMVisual.Name = "mPCMVisual";
               this.mPCMVisual.Padding = new System.Windows.Forms.Padding(0, 31, 0, 0);
               this.mPCMVisual.Size = new System.Drawing.Size(1362, 511);
               this.mPCMVisual.TabIndex = 0;
               // 
               // mFFTVisual
               // 
               this.mFFTVisual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
               this.mFFTVisual.Dock = System.Windows.Forms.DockStyle.Fill;
               this.mFFTVisual.Location = new System.Drawing.Point(3, 520);
               this.mFFTVisual.Name = "mFFTVisual";
               this.mFFTVisual.Padding = new System.Windows.Forms.Padding(0, 0, 0, 31);
               this.mFFTVisual.Size = new System.Drawing.Size(1362, 511);
               this.mFFTVisual.TabIndex = 1;
               // 
               // statusStrip1
               // 
               this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
               this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
               this.statusStrip1.Location = new System.Drawing.Point(0, 1007);
               this.statusStrip1.Name = "statusStrip1";
               this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
               this.statusStrip1.Size = new System.Drawing.Size(1368, 27);
               this.statusStrip1.TabIndex = 1;
               this.statusStrip1.Text = "statusStrip1";
               // 
               // lblStatus
               // 
               this.lblStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.lblStatus.Name = "lblStatus";
               this.lblStatus.Size = new System.Drawing.Size(220, 22);
               this.lblStatus.Text = "toolStripStatusLabel1";
               // 
               // menuStrip1
               // 
               this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
               this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
               this.menuStrip1.Location = new System.Drawing.Point(0, 0);
               this.menuStrip1.Name = "menuStrip1";
               this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
               this.menuStrip1.Size = new System.Drawing.Size(1368, 35);
               this.menuStrip1.TabIndex = 2;
               this.menuStrip1.Text = "menuStrip1";
               // 
               // viewToolStripMenuItem
               // 
               this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoScaleToolStripMenuItem});
               this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
               this.viewToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
               this.viewToolStripMenuItem.Text = "View";
               // 
               // autoScaleToolStripMenuItem
               // 
               this.autoScaleToolStripMenuItem.Name = "autoScaleToolStripMenuItem";
               this.autoScaleToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
               this.autoScaleToolStripMenuItem.Text = "Auto-Scale";
               this.autoScaleToolStripMenuItem.Click += new System.EventHandler(this.AutoScaleToolStripMenuItem_Click);
               // 
               // helpToolStripMenuItem
               // 
               this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.websiteToolStripMenuItem,
            this.infoMessageToolStripMenuItem});
               this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
               this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
               this.helpToolStripMenuItem.Text = "Help";
               // 
               // websiteToolStripMenuItem
               // 
               this.websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
               this.websiteToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
               this.websiteToolStripMenuItem.Text = "Website";
               this.websiteToolStripMenuItem.Click += new System.EventHandler(this.WebsiteToolStripMenuItem_Click);
               // 
               // infoMessageToolStripMenuItem
               // 
               this.infoMessageToolStripMenuItem.Name = "infoMessageToolStripMenuItem";
               this.infoMessageToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
               this.infoMessageToolStripMenuItem.Text = "Info Message";
               this.infoMessageToolStripMenuItem.Click += new System.EventHandler(this.InfoMessageToolStripMenuItem_Click);
               // 
               // SoundVisualizationUI
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(1368, 1034);
               this.Controls.Add(this.menuStrip1);
               this.Controls.Add(this.statusStrip1);
               this.Controls.Add(this.tableLayoutPanel1);
               this.MainMenuStrip = this.menuStrip1;
               this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
               this.Name = "SoundVisualizationUI";
               this.Text = "ScottPlot Microphone FFT Demo";
               this.tableLayoutPanel1.ResumeLayout(false);
               this.statusStrip1.ResumeLayout(false);
               this.statusStrip1.PerformLayout();
               this.menuStrip1.ResumeLayout(false);
               this.menuStrip1.PerformLayout();
               this.ResumeLayout(false);
               this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem websiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoMessageToolStripMenuItem;
          private System.Windows.Forms.Timer timer1;
     }
}

