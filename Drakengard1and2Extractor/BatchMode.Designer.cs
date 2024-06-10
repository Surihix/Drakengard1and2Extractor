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
            this.BatchExtractFPKBtn = new System.Windows.Forms.Button();
            this.BatchExtractDPKBtn = new System.Windows.Forms.Button();
            this.BatchExtractFPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BatchExtractDPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BatchStatusLabel = new System.Windows.Forms.Label();
            this.BatchStatusTextBox = new System.Windows.Forms.TextBox();
            this.BatchExtractKPSBtn = new System.Windows.Forms.Button();
            this.BatchStatusDelBtn = new System.Windows.Forms.Button();
            this.BatchExtractKPStoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // BatchExtractFPKBtn
            // 
            this.BatchExtractFPKBtn.Location = new System.Drawing.Point(87, 17);
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
            this.BatchExtractDPKBtn.Location = new System.Drawing.Point(87, 55);
            this.BatchExtractDPKBtn.Name = "BatchExtractDPKBtn";
            this.BatchExtractDPKBtn.Size = new System.Drawing.Size(92, 31);
            this.BatchExtractDPKBtn.TabIndex = 1;
            this.BatchExtractDPKBtn.Text = "Extract DPK";
            this.BatchExtractDPKBtn.UseVisualStyleBackColor = true;
            this.BatchExtractDPKBtn.Click += new System.EventHandler(this.BatchExtractDPKBtn_Click);
            this.BatchExtractDPKBtn.MouseHover += new System.EventHandler(this.BatchExtractDPKBtn_MouseHover);
            // 
            // BatchStatusLabel
            // 
            this.BatchStatusLabel.AutoSize = true;
            this.BatchStatusLabel.Location = new System.Drawing.Point(13, 157);
            this.BatchStatusLabel.Name = "BatchStatusLabel";
            this.BatchStatusLabel.Size = new System.Drawing.Size(40, 13);
            this.BatchStatusLabel.TabIndex = 2;
            this.BatchStatusLabel.Text = "Status:";
            // 
            // BatchStatusTextBox
            // 
            this.BatchStatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BatchStatusTextBox.Location = new System.Drawing.Point(12, 176);
            this.BatchStatusTextBox.Multiline = true;
            this.BatchStatusTextBox.Name = "BatchStatusTextBox";
            this.BatchStatusTextBox.ReadOnly = true;
            this.BatchStatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.BatchStatusTextBox.Size = new System.Drawing.Size(241, 91);
            this.BatchStatusTextBox.TabIndex = 10;
            // 
            // BatchExtractKPSBtn
            // 
            this.BatchExtractKPSBtn.Location = new System.Drawing.Point(87, 93);
            this.BatchExtractKPSBtn.Name = "BatchExtractKPSBtn";
            this.BatchExtractKPSBtn.Size = new System.Drawing.Size(92, 31);
            this.BatchExtractKPSBtn.TabIndex = 13;
            this.BatchExtractKPSBtn.Text = "Extract KPS";
            this.BatchExtractKPSBtn.UseVisualStyleBackColor = true;
            this.BatchExtractKPSBtn.Click += new System.EventHandler(this.BatchExtractKPSBtn_Click);
            this.BatchExtractKPSBtn.MouseHover += new System.EventHandler(this.BatchExtractKPSBtn_MouseHover);
            // 
            // BatchStatusDelBtn
            // 
            this.BatchStatusDelBtn.Location = new System.Drawing.Point(95, 285);
            this.BatchStatusDelBtn.Name = "BatchStatusDelBtn";
            this.BatchStatusDelBtn.Size = new System.Drawing.Size(75, 23);
            this.BatchStatusDelBtn.TabIndex = 14;
            this.BatchStatusDelBtn.Text = "Clear Status";
            this.BatchStatusDelBtn.UseVisualStyleBackColor = true;
            this.BatchStatusDelBtn.Click += new System.EventHandler(this.BatchStatusDelBtn_Click);
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 323);
            this.Controls.Add(this.BatchStatusDelBtn);
            this.Controls.Add(this.BatchExtractKPSBtn);
            this.Controls.Add(this.BatchStatusTextBox);
            this.Controls.Add(this.BatchStatusLabel);
            this.Controls.Add(this.BatchExtractDPKBtn);
            this.Controls.Add(this.BatchExtractFPKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Mode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BatchForm_FormClosing);
            this.Shown += new System.EventHandler(this.BatchForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BatchExtractFPKBtn;
        private System.Windows.Forms.Button BatchExtractDPKBtn;
        private System.Windows.Forms.ToolTip BatchExtractFPKtoolTip;
        private System.Windows.Forms.ToolTip BatchExtractDPKtoolTip;
        private System.Windows.Forms.Label BatchStatusLabel;
        private System.Windows.Forms.TextBox BatchStatusTextBox;
        private System.Windows.Forms.Button BatchExtractKPSBtn;
        private System.Windows.Forms.Button BatchStatusDelBtn;
        private System.Windows.Forms.ToolTip BatchExtractKPStoolTip;
    }
}