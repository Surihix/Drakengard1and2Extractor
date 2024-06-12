namespace Drakengard1and2Extractor
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
            this.components = new System.ComponentModel.Container();
            this.BatchFPKBtn = new System.Windows.Forms.Button();
            this.BatchDPKBtn = new System.Windows.Forms.Button();
            this.BatchExtractFPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BatchExtractDPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BatchStatusLabel = new System.Windows.Forms.Label();
            this.BatchStatusTextBox = new System.Windows.Forms.TextBox();
            this.BatchKPSBtn = new System.Windows.Forms.Button();
            this.BatchStatusDelBtn = new System.Windows.Forms.Button();
            this.BatchExtractKPStoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.FileExtGrp = new System.Windows.Forms.GroupBox();
            this.ImgConvtGroupBox = new System.Windows.Forms.GroupBox();
            this.BatchZIMBtn = new System.Windows.Forms.Button();
            this.BatchSPK0Btn = new System.Windows.Forms.Button();
            this.BatchZIMtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BatchSPK0toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.FileExtGrp.SuspendLayout();
            this.ImgConvtGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BatchFPKBtn
            // 
            this.BatchFPKBtn.Location = new System.Drawing.Point(73, 19);
            this.BatchFPKBtn.Name = "BatchFPKBtn";
            this.BatchFPKBtn.Size = new System.Drawing.Size(92, 31);
            this.BatchFPKBtn.TabIndex = 0;
            this.BatchFPKBtn.Text = "Extract FPK";
            this.BatchFPKBtn.UseVisualStyleBackColor = true;
            this.BatchFPKBtn.Click += new System.EventHandler(this.BatchExtractFPKBtn_Click);
            this.BatchFPKBtn.MouseHover += new System.EventHandler(this.BatchExtractFPKBtn_MouseHover);
            // 
            // BatchDPKBtn
            // 
            this.BatchDPKBtn.Location = new System.Drawing.Point(73, 56);
            this.BatchDPKBtn.Name = "BatchDPKBtn";
            this.BatchDPKBtn.Size = new System.Drawing.Size(92, 31);
            this.BatchDPKBtn.TabIndex = 1;
            this.BatchDPKBtn.Text = "Extract DPK";
            this.BatchDPKBtn.UseVisualStyleBackColor = true;
            this.BatchDPKBtn.Click += new System.EventHandler(this.BatchExtractDPKBtn_Click);
            this.BatchDPKBtn.MouseHover += new System.EventHandler(this.BatchExtractDPKBtn_MouseHover);
            // 
            // BatchStatusLabel
            // 
            this.BatchStatusLabel.AutoSize = true;
            this.BatchStatusLabel.Location = new System.Drawing.Point(13, 299);
            this.BatchStatusLabel.Name = "BatchStatusLabel";
            this.BatchStatusLabel.Size = new System.Drawing.Size(40, 13);
            this.BatchStatusLabel.TabIndex = 2;
            this.BatchStatusLabel.Text = "Status:";
            // 
            // BatchStatusTextBox
            // 
            this.BatchStatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BatchStatusTextBox.Location = new System.Drawing.Point(12, 318);
            this.BatchStatusTextBox.Multiline = true;
            this.BatchStatusTextBox.Name = "BatchStatusTextBox";
            this.BatchStatusTextBox.ReadOnly = true;
            this.BatchStatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.BatchStatusTextBox.Size = new System.Drawing.Size(241, 91);
            this.BatchStatusTextBox.TabIndex = 10;
            // 
            // BatchKPSBtn
            // 
            this.BatchKPSBtn.Location = new System.Drawing.Point(73, 93);
            this.BatchKPSBtn.Name = "BatchKPSBtn";
            this.BatchKPSBtn.Size = new System.Drawing.Size(92, 31);
            this.BatchKPSBtn.TabIndex = 13;
            this.BatchKPSBtn.Text = "Extract KPS";
            this.BatchKPSBtn.UseVisualStyleBackColor = true;
            this.BatchKPSBtn.Click += new System.EventHandler(this.BatchExtractKPSBtn_Click);
            this.BatchKPSBtn.MouseHover += new System.EventHandler(this.BatchExtractKPSBtn_MouseHover);
            // 
            // BatchStatusDelBtn
            // 
            this.BatchStatusDelBtn.Location = new System.Drawing.Point(95, 427);
            this.BatchStatusDelBtn.Name = "BatchStatusDelBtn";
            this.BatchStatusDelBtn.Size = new System.Drawing.Size(75, 23);
            this.BatchStatusDelBtn.TabIndex = 14;
            this.BatchStatusDelBtn.Text = "Clear Status";
            this.BatchStatusDelBtn.UseVisualStyleBackColor = true;
            this.BatchStatusDelBtn.Click += new System.EventHandler(this.BatchStatusDelBtn_Click);
            // 
            // FileExtGrp
            // 
            this.FileExtGrp.Controls.Add(this.BatchFPKBtn);
            this.FileExtGrp.Controls.Add(this.BatchDPKBtn);
            this.FileExtGrp.Controls.Add(this.BatchKPSBtn);
            this.FileExtGrp.Location = new System.Drawing.Point(12, 12);
            this.FileExtGrp.Name = "FileExtGrp";
            this.FileExtGrp.Size = new System.Drawing.Size(241, 142);
            this.FileExtGrp.TabIndex = 15;
            this.FileExtGrp.TabStop = false;
            this.FileExtGrp.Text = "File Extraction :";
            // 
            // ImgConvtGroupBox
            // 
            this.ImgConvtGroupBox.Controls.Add(this.BatchSPK0Btn);
            this.ImgConvtGroupBox.Controls.Add(this.BatchZIMBtn);
            this.ImgConvtGroupBox.Location = new System.Drawing.Point(12, 170);
            this.ImgConvtGroupBox.Name = "ImgConvtGroupBox";
            this.ImgConvtGroupBox.Size = new System.Drawing.Size(241, 112);
            this.ImgConvtGroupBox.TabIndex = 16;
            this.ImgConvtGroupBox.TabStop = false;
            this.ImgConvtGroupBox.Text = "Image Conversion :";
            // 
            // BatchZIMBtn
            // 
            this.BatchZIMBtn.Location = new System.Drawing.Point(78, 29);
            this.BatchZIMBtn.Name = "BatchZIMBtn";
            this.BatchZIMBtn.Size = new System.Drawing.Size(84, 28);
            this.BatchZIMBtn.TabIndex = 1;
            this.BatchZIMBtn.Text = "Convert ZIM";
            this.BatchZIMBtn.UseVisualStyleBackColor = true;
            this.BatchZIMBtn.Click += new System.EventHandler(this.BatchConvertZIMBtn_Click);
            this.BatchZIMBtn.MouseHover += new System.EventHandler(this.BatchConvertZIMBtn_MouseHover);
            // 
            // BatchSPK0Btn
            // 
            this.BatchSPK0Btn.Location = new System.Drawing.Point(78, 67);
            this.BatchSPK0Btn.Name = "BatchSPK0Btn";
            this.BatchSPK0Btn.Size = new System.Drawing.Size(84, 28);
            this.BatchSPK0Btn.TabIndex = 2;
            this.BatchSPK0Btn.Text = "Convert SPK0";
            this.BatchSPK0Btn.UseVisualStyleBackColor = true;
            this.BatchSPK0Btn.Click += new System.EventHandler(this.BatchConvertSPK0Btn_Click);
            this.BatchSPK0Btn.MouseHover += new System.EventHandler(this.BatchConvertSPK0Btn_MouseHover);
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 467);
            this.Controls.Add(this.ImgConvtGroupBox);
            this.Controls.Add(this.FileExtGrp);
            this.Controls.Add(this.BatchStatusDelBtn);
            this.Controls.Add(this.BatchStatusTextBox);
            this.Controls.Add(this.BatchStatusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Mode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BatchForm_FormClosing);
            this.Shown += new System.EventHandler(this.BatchForm_Shown);
            this.FileExtGrp.ResumeLayout(false);
            this.ImgConvtGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BatchFPKBtn;
        private System.Windows.Forms.Button BatchDPKBtn;
        private System.Windows.Forms.ToolTip BatchExtractFPKtoolTip;
        private System.Windows.Forms.ToolTip BatchExtractDPKtoolTip;
        private System.Windows.Forms.Label BatchStatusLabel;
        private System.Windows.Forms.TextBox BatchStatusTextBox;
        private System.Windows.Forms.Button BatchKPSBtn;
        private System.Windows.Forms.Button BatchStatusDelBtn;
        private System.Windows.Forms.ToolTip BatchExtractKPStoolTip;
        private System.Windows.Forms.GroupBox FileExtGrp;
        private System.Windows.Forms.GroupBox ImgConvtGroupBox;
        private System.Windows.Forms.Button BatchZIMBtn;
        private System.Windows.Forms.Button BatchSPK0Btn;
        private System.Windows.Forms.ToolTip BatchZIMtoolTip;
        private System.Windows.Forms.ToolTip BatchSPK0toolTip;
    }
}