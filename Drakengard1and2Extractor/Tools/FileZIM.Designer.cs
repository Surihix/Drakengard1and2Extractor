namespace Drakengard1and2Extractor.Tools
{
    partial class FileZIM
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
            this.ConvertZIMImgBtn = new System.Windows.Forms.Button();
            this.ZimAlphaCompNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ZimAlphaCompLabel = new System.Windows.Forms.Label();
            this.UnSwizzleCheckBox = new System.Windows.Forms.CheckBox();
            this.ZimSaveAsComboBox = new System.Windows.Forms.ComboBox();
            this.ZimSaveAsLabel = new System.Windows.Forms.Label();
            this.ConvertZimImgBtnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.UnSwizzleChkBoxToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ZimAlphaCompNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ConvertZIMImgBtn
            // 
            this.ConvertZIMImgBtn.Location = new System.Drawing.Point(229, 45);
            this.ConvertZIMImgBtn.Name = "ConvertZIMImgBtn";
            this.ConvertZIMImgBtn.Size = new System.Drawing.Size(75, 23);
            this.ConvertZIMImgBtn.TabIndex = 2;
            this.ConvertZIMImgBtn.Text = "Convert";
            this.ConvertZIMImgBtn.UseVisualStyleBackColor = true;
            this.ConvertZIMImgBtn.Click += new System.EventHandler(this.ConvertZIMImgBtn_Click);
            this.ConvertZIMImgBtn.MouseHover += new System.EventHandler(this.ConvertZIMImgBtn_MouseHover);
            // 
            // ZimAlphaCompNumericUpDown
            // 
            this.ZimAlphaCompNumericUpDown.Location = new System.Drawing.Point(157, 77);
            this.ZimAlphaCompNumericUpDown.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.ZimAlphaCompNumericUpDown.Name = "ZimAlphaCompNumericUpDown";
            this.ZimAlphaCompNumericUpDown.Size = new System.Drawing.Size(44, 20);
            this.ZimAlphaCompNumericUpDown.TabIndex = 4;
            // 
            // ZimAlphaCompLabel
            // 
            this.ZimAlphaCompLabel.AutoSize = true;
            this.ZimAlphaCompLabel.Location = new System.Drawing.Point(12, 80);
            this.ZimAlphaCompLabel.Name = "ZimAlphaCompLabel";
            this.ZimAlphaCompLabel.Size = new System.Drawing.Size(143, 13);
            this.ZimAlphaCompLabel.TabIndex = 3;
            this.ZimAlphaCompLabel.Text = "Alpha Compensation (0-128):";
            // 
            // UnSwizzleCheckBox
            // 
            this.UnSwizzleCheckBox.AutoSize = true;
            this.UnSwizzleCheckBox.Location = new System.Drawing.Point(15, 107);
            this.UnSwizzleCheckBox.Name = "UnSwizzleCheckBox";
            this.UnSwizzleCheckBox.Size = new System.Drawing.Size(155, 17);
            this.UnSwizzleCheckBox.TabIndex = 5;
            this.UnSwizzleCheckBox.Text = "Unswizzle 8bpp image data";
            this.UnSwizzleCheckBox.UseVisualStyleBackColor = true;
            this.UnSwizzleCheckBox.MouseHover += new System.EventHandler(this.UnSwizzleCheckBox_MouseHover);
            // 
            // ZimSaveAsComboBox
            // 
            this.ZimSaveAsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ZimSaveAsComboBox.FormattingEnabled = true;
            this.ZimSaveAsComboBox.Items.AddRange(new object[] {
            "Bitmap (.bmp)",
            "Direct Draw Surface (.dds)",
            "Portable Network Graphics (.png)"});
            this.ZimSaveAsComboBox.Location = new System.Drawing.Point(13, 46);
            this.ZimSaveAsComboBox.Name = "ZimSaveAsComboBox";
            this.ZimSaveAsComboBox.Size = new System.Drawing.Size(209, 21);
            this.ZimSaveAsComboBox.TabIndex = 1;
            this.ZimSaveAsComboBox.SelectionChangeCommitted += new System.EventHandler(this.ZimSaveAsComboBox_SelectionChangeCommitted);
            // 
            // ZimSaveAsLabel
            // 
            this.ZimSaveAsLabel.AutoSize = true;
            this.ZimSaveAsLabel.Location = new System.Drawing.Point(12, 29);
            this.ZimSaveAsLabel.Name = "ZimSaveAsLabel";
            this.ZimSaveAsLabel.Size = new System.Drawing.Size(82, 13);
            this.ZimSaveAsLabel.TabIndex = 0;
            this.ZimSaveAsLabel.Text = "Save Image As:";
            // 
            // FileZIM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 144);
            this.Controls.Add(this.ZimAlphaCompLabel);
            this.Controls.Add(this.ZimAlphaCompNumericUpDown);
            this.Controls.Add(this.UnSwizzleCheckBox);
            this.Controls.Add(this.ZimSaveAsLabel);
            this.Controls.Add(this.ZimSaveAsComboBox);
            this.Controls.Add(this.ConvertZIMImgBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileZIM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ZIM Converter";
            ((System.ComponentModel.ISupportInitialize)(this.ZimAlphaCompNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ConvertZIMImgBtn;
        private System.Windows.Forms.NumericUpDown ZimAlphaCompNumericUpDown;
        private System.Windows.Forms.Label ZimAlphaCompLabel;
        private System.Windows.Forms.CheckBox UnSwizzleCheckBox;
        private System.Windows.Forms.ComboBox ZimSaveAsComboBox;
        private System.Windows.Forms.Label ZimSaveAsLabel;
        private System.Windows.Forms.ToolTip ConvertZimImgBtnToolTip;
        private System.Windows.Forms.ToolTip UnSwizzleChkBoxToolTip;
    }
}