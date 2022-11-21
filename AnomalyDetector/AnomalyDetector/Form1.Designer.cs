namespace AnomalyDetector
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.종료ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.시작ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.기록ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEBUGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.이미지불러오기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_left = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_right = new System.Windows.Forms.TableLayoutPanel();
            this.imageBox_center = new Emgu.CV.UI.ImageBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_center)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.종료ToolStripMenuItem,
            this.toolStripTextBox1,
            this.시작ToolStripMenuItem,
            this.기록ToolStripMenuItem,
            this.dEBUGToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1008, 30);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 종료ToolStripMenuItem
            // 
            this.종료ToolStripMenuItem.Name = "종료ToolStripMenuItem";
            this.종료ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.종료ToolStripMenuItem.Text = "종료";
            this.종료ToolStripMenuItem.Click += new System.EventHandler(this.종료ToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolStripTextBox1.Enabled = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(129, 24);
            this.toolStripTextBox1.Text = "　　　　　";
            // 
            // 시작ToolStripMenuItem
            // 
            this.시작ToolStripMenuItem.Name = "시작ToolStripMenuItem";
            this.시작ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.시작ToolStripMenuItem.Text = "시작";
            this.시작ToolStripMenuItem.Click += new System.EventHandler(this.시작ToolStripMenuItem_Click);
            // 
            // 기록ToolStripMenuItem
            // 
            this.기록ToolStripMenuItem.Name = "기록ToolStripMenuItem";
            this.기록ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.기록ToolStripMenuItem.Text = "기록";
            this.기록ToolStripMenuItem.Click += new System.EventHandler(this.기록ToolStripMenuItem_Click);
            // 
            // dEBUGToolStripMenuItem
            // 
            this.dEBUGToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.이미지불러오기ToolStripMenuItem});
            this.dEBUGToolStripMenuItem.Name = "dEBUGToolStripMenuItem";
            this.dEBUGToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.dEBUGToolStripMenuItem.Text = "DEBUG";
            // 
            // 이미지불러오기ToolStripMenuItem
            // 
            this.이미지불러오기ToolStripMenuItem.Name = "이미지불러오기ToolStripMenuItem";
            this.이미지불러오기ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.이미지불러오기ToolStripMenuItem.Text = "이미지 불러오기";
            this.이미지불러오기ToolStripMenuItem.Click += new System.EventHandler(this.이미지불러오기ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 722);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(1008, 26);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 20);
            this.toolStripStatusLabel1.Text = "준비";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel_left, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel_right, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.imageBox_center, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1003, 696);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // tableLayoutPanel_left
            // 
            this.tableLayoutPanel_left.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_left.ColumnCount = 2;
            this.tableLayoutPanel_left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_left.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel_left.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel_left.Name = "tableLayoutPanel_left";
            this.tableLayoutPanel_left.RowCount = 3;
            this.tableLayoutPanel_left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_left.Size = new System.Drawing.Size(325, 686);
            this.tableLayoutPanel_left.TabIndex = 3;
            // 
            // tableLayoutPanel_right
            // 
            this.tableLayoutPanel_right.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_right.ColumnCount = 2;
            this.tableLayoutPanel_right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_right.Location = new System.Drawing.Point(673, 5);
            this.tableLayoutPanel_right.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel_right.Name = "tableLayoutPanel_right";
            this.tableLayoutPanel_right.RowCount = 3;
            this.tableLayoutPanel_right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_right.Size = new System.Drawing.Size(325, 686);
            this.tableLayoutPanel_right.TabIndex = 3;
            // 
            // imageBox_center
            // 
            this.imageBox_center.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox_center.BackColor = System.Drawing.SystemColors.Control;
            this.imageBox_center.Location = new System.Drawing.Point(338, 4);
            this.imageBox_center.Name = "imageBox_center";
            this.imageBox_center.Size = new System.Drawing.Size(327, 688);
            this.imageBox_center.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox_center.TabIndex = 4;
            this.imageBox_center.TabStop = false;
            this.imageBox_center.OnZoomScaleChange += new System.EventHandler(this.imageBox_center_OnZoomScaleChange);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = " Image File|*.JPG;*.BMP;*.PNG; ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1008, 748);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Anomaly Detector";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_center)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 시작ToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem 종료ToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel_left;
        private TableLayoutPanel tableLayoutPanel_right;
        private ToolStripMenuItem 기록ToolStripMenuItem;
        private ToolStripMenuItem dEBUGToolStripMenuItem;
        private ToolStripMenuItem 이미지불러오기ToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private Emgu.CV.UI.ImageBox imageBox_center;
        private ToolStripTextBox toolStripTextBox1;
    }
}