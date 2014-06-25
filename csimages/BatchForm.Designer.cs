namespace csimages
{
    partial class BatchForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cannyBox = new System.Windows.Forms.CheckBox();
            this.blurBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.greyBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.AllowDrop = true;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(770, 459);
            this.listBox1.TabIndex = 0;
            this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            this.listBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 478);
            this.progressBar1.MarqueeAnimationSpeed = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(769, 20);
            this.progressBar1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cannyBox);
            this.panel1.Controls.Add(this.blurBox);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.greyBox);
            this.panel1.Location = new System.Drawing.Point(13, 505);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 53);
            this.panel1.TabIndex = 2;
            // 
            // cannyBox
            // 
            this.cannyBox.AutoSize = true;
            this.cannyBox.Location = new System.Drawing.Point(210, 4);
            this.cannyBox.Name = "cannyBox";
            this.cannyBox.Size = new System.Drawing.Size(80, 17);
            this.cannyBox.TabIndex = 3;
            this.cannyBox.Text = "checkBox1";
            this.cannyBox.UseVisualStyleBackColor = true;
            // 
            // blurBox
            // 
            this.blurBox.AutoSize = true;
            this.blurBox.Location = new System.Drawing.Point(123, 4);
            this.blurBox.Name = "blurBox";
            this.blurBox.Size = new System.Drawing.Size(80, 17);
            this.blurBox.TabIndex = 2;
            this.blurBox.Text = "checkBox2";
            this.blurBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Старт";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // greyBox
            // 
            this.greyBox.AutoSize = true;
            this.greyBox.Location = new System.Drawing.Point(4, 4);
            this.greyBox.Name = "greyBox";
            this.greyBox.Size = new System.Drawing.Size(112, 17);
            this.greyBox.TabIndex = 0;
            this.greyBox.Text = "Градации серого";
            this.greyBox.UseVisualStyleBackColor = true;
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 581);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listBox1);
            this.Name = "BatchForm";
            this.Text = "BatchForm";
            this.Load += new System.EventHandler(this.BatchForm_Load);
            this.Shown += new System.EventHandler(this.BatchForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BatchForm_Paint);
            this.Resize += new System.EventHandler(this.BatchForm_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox greyBox;
        private System.Windows.Forms.CheckBox blurBox;
        private System.Windows.Forms.CheckBox cannyBox;
    }
}