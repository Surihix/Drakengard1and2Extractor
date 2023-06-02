namespace Drakengard1and2Extractor
{
    partial class BatchMode
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
            this.BatchExtractFPKBtn = new System.Windows.Forms.Button();
            this.BatchExtractDPKBtn = new System.Windows.Forms.Button();
            this.BatchExtractFPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BatchExtractDPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BatchStatusListBox = new System.Windows.Forms.ListBox();
            this.BatchStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BatchExtractFPKBtn
            // 
            this.BatchExtractFPKBtn.Location = new System.Drawing.Point(62, 17);
            this.BatchExtractFPKBtn.Name = "BatchExtractFPKBtn";
            this.BatchExtractFPKBtn.Size = new System.Drawing.Size(92, 31);
            this.BatchExtractFPKBtn.TabIndex = 0;
            this.BatchExtractFPKBtn.Text = "Extract FPK";
            this.BatchExtractFPKBtn.UseVisualStyleBackColor = true;
            this.BatchExtractFPKBtn.Click += new System.EventHandler(this.BatchExtractFPKBtn_Click);
            this.BatchExtractFPKBtn.MouseHover += new System.EventHandler(this.BatchExtractFPKBtn_MouseHover);
            // 
            // BatchExtractDPKBtn
            // 
            this.BatchExtractDPKBtn.Location = new System.Drawing.Point(62, 55);
            this.BatchExtractDPKBtn.Name = "BatchExtractDPKBtn";
            this.BatchExtractDPKBtn.Size = new System.Drawing.Size(92, 31);
            this.BatchExtractDPKBtn.TabIndex = 1;
            this.BatchExtractDPKBtn.Text = "Extract DPK";
            this.BatchExtractDPKBtn.UseVisualStyleBackColor = true;
            this.BatchExtractDPKBtn.Click += new System.EventHandler(this.BatchExtractDPKBtn_Click);
            this.BatchExtractDPKBtn.MouseHover += new System.EventHandler(this.BatchExtractDPKBtn_MouseHover);
            // 
            // BatchStatusListBox
            // 
            this.BatchStatusListBox.FormattingEnabled = true;
            this.BatchStatusListBox.Location = new System.Drawing.Point(12, 118);
            this.BatchStatusListBox.Name = "BatchStatusListBox";
            this.BatchStatusListBox.Size = new System.Drawing.Size(194, 56);
            this.BatchStatusListBox.TabIndex = 3;
            // 
            // BatchStatusLabel
            // 
            this.BatchStatusLabel.AutoSize = true;
            this.BatchStatusLabel.Location = new System.Drawing.Point(13, 102);
            this.BatchStatusLabel.Name = "BatchStatusLabel";
            this.BatchStatusLabel.Size = new System.Drawing.Size(40, 13);
            this.BatchStatusLabel.TabIndex = 2;
            this.BatchStatusLabel.Text = "Status:";
            // 
            // BatchMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 188);
            this.Controls.Add(this.BatchStatusLabel);
            this.Controls.Add(this.BatchStatusListBox);
            this.Controls.Add(this.BatchExtractDPKBtn);
            this.Controls.Add(this.BatchExtractFPKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Mode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BatchExtractFPKBtn;
        private System.Windows.Forms.Button BatchExtractDPKBtn;
        private System.Windows.Forms.ToolTip BatchExtractFPKtoolTip;
        private System.Windows.Forms.ToolTip BatchExtractDPKtoolTip;
        private System.Windows.Forms.ListBox BatchStatusListBox;
        private System.Windows.Forms.Label BatchStatusLabel;
    }
}