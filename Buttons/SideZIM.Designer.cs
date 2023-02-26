namespace Drakengard1and2Extractor
{
    partial class SideZIM
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
            this.ConvertZIMImgBtn = new System.Windows.Forms.Button();
            this.ZimSaveAsComboBox = new System.Windows.Forms.ComboBox();
            this.ZimAlphaCompNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ZimAlphaCompLabel = new System.Windows.Forms.Label();
            this.ZimSaveAsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ZimAlphaCompNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ConvertZIMImgBtn
            // 
            this.ConvertZIMImgBtn.Location = new System.Drawing.Point(117, 151);
            this.ConvertZIMImgBtn.Name = "ConvertZIMImgBtn";
            this.ConvertZIMImgBtn.Size = new System.Drawing.Size(75, 23);
            this.ConvertZIMImgBtn.TabIndex = 1;
            this.ConvertZIMImgBtn.Text = "Convert";
            this.ConvertZIMImgBtn.UseVisualStyleBackColor = true;
            this.ConvertZIMImgBtn.Click += new System.EventHandler(this.ConvertZIMImgBtn_Click);
            // 
            // ZimSaveAsComboBox
            // 
            this.ZimSaveAsComboBox.FormattingEnabled = true;
            this.ZimSaveAsComboBox.Items.AddRange(new object[] {
            "Bitmap (.bmp)",
            "Portable Network Graphics (.png)"});
            this.ZimSaveAsComboBox.Location = new System.Drawing.Point(38, 50);
            this.ZimSaveAsComboBox.MaxDropDownItems = 2;
            this.ZimSaveAsComboBox.Name = "ZimSaveAsComboBox";
            this.ZimSaveAsComboBox.Size = new System.Drawing.Size(229, 21);
            this.ZimSaveAsComboBox.TabIndex = 3;
            this.ZimSaveAsComboBox.SelectionChangeCommitted += new System.EventHandler(this.ZimSaveAsComboBox_SelectionChangeCommitted);
            // 
            // ZimAlphaCompNumericUpDown
            // 
            this.ZimAlphaCompNumericUpDown.Location = new System.Drawing.Point(165, 100);
            this.ZimAlphaCompNumericUpDown.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.ZimAlphaCompNumericUpDown.Name = "ZimAlphaCompNumericUpDown";
            this.ZimAlphaCompNumericUpDown.Size = new System.Drawing.Size(67, 20);
            this.ZimAlphaCompNumericUpDown.TabIndex = 4;
            // 
            // ZimAlphaCompLabel
            // 
            this.ZimAlphaCompLabel.AutoSize = true;
            this.ZimAlphaCompLabel.Location = new System.Drawing.Point(55, 103);
            this.ZimAlphaCompLabel.Name = "ZimAlphaCompLabel";
            this.ZimAlphaCompLabel.Size = new System.Drawing.Size(107, 13);
            this.ZimAlphaCompLabel.TabIndex = 6;
            this.ZimAlphaCompLabel.Text = "Alpha Compensation:";
            // 
            // ZimSaveAsLabel
            // 
            this.ZimSaveAsLabel.AutoSize = true;
            this.ZimSaveAsLabel.Location = new System.Drawing.Point(37, 33);
            this.ZimSaveAsLabel.Name = "ZimSaveAsLabel";
            this.ZimSaveAsLabel.Size = new System.Drawing.Size(50, 13);
            this.ZimSaveAsLabel.TabIndex = 7;
            this.ZimSaveAsLabel.Text = "Save As:";
            // 
            // SideZIM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 188);
            this.Controls.Add(this.ZimSaveAsLabel);
            this.Controls.Add(this.ZimAlphaCompLabel);
            this.Controls.Add(this.ZimAlphaCompNumericUpDown);
            this.Controls.Add(this.ZimSaveAsComboBox);
            this.Controls.Add(this.ConvertZIMImgBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SideZIM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ZIM Converter";
            ((System.ComponentModel.ISupportInitialize)(this.ZimAlphaCompNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ConvertZIMImgBtn;
        private System.Windows.Forms.ComboBox ZimSaveAsComboBox;
        private System.Windows.Forms.NumericUpDown ZimAlphaCompNumericUpDown;
        private System.Windows.Forms.Label ZimAlphaCompLabel;
        private System.Windows.Forms.Label ZimSaveAsLabel;
    }
}