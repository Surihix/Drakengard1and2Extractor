namespace Drakengard1and2Extractor
{
    partial class SideSPK0
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
            this.ConvertSPK0ImgBtn = new System.Windows.Forms.Button();
            this.Spk0SaveAsComboBox = new System.Windows.Forms.ComboBox();
            this.Spk0SaveAsLabel = new System.Windows.Forms.Label();
            this.Spk0AlphaCompNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Spk0AlphaCompLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Spk0AlphaCompNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ConvertSPK0ImgBtn
            // 
            this.ConvertSPK0ImgBtn.Location = new System.Drawing.Point(117, 151);
            this.ConvertSPK0ImgBtn.Name = "ConvertSPK0ImgBtn";
            this.ConvertSPK0ImgBtn.Size = new System.Drawing.Size(75, 23);
            this.ConvertSPK0ImgBtn.TabIndex = 0;
            this.ConvertSPK0ImgBtn.Text = "Convert";
            this.ConvertSPK0ImgBtn.UseVisualStyleBackColor = true;
            this.ConvertSPK0ImgBtn.Click += new System.EventHandler(this.ConvertSPK0ImgBtn_Click);
            // 
            // Spk0SaveAsComboBox
            // 
            this.Spk0SaveAsComboBox.FormattingEnabled = true;
            this.Spk0SaveAsComboBox.Items.AddRange(new object[] {
            "Bitmap (.bmp)",
            "Portable Network Graphics (.png)"});
            this.Spk0SaveAsComboBox.Location = new System.Drawing.Point(38, 50);
            this.Spk0SaveAsComboBox.Name = "Spk0SaveAsComboBox";
            this.Spk0SaveAsComboBox.Size = new System.Drawing.Size(229, 21);
            this.Spk0SaveAsComboBox.TabIndex = 1;
            this.Spk0SaveAsComboBox.SelectionChangeCommitted += new System.EventHandler(this.Spk0SaveAsComboBox_SelectionChangeCommitted);
            // 
            // Spk0SaveAsLabel
            // 
            this.Spk0SaveAsLabel.AutoSize = true;
            this.Spk0SaveAsLabel.Location = new System.Drawing.Point(37, 33);
            this.Spk0SaveAsLabel.Name = "Spk0SaveAsLabel";
            this.Spk0SaveAsLabel.Size = new System.Drawing.Size(87, 13);
            this.Spk0SaveAsLabel.TabIndex = 2;
            this.Spk0SaveAsLabel.Text = "Save Images As:";
            // 
            // Spk0AlphaCompNumericUpDown
            // 
            this.Spk0AlphaCompNumericUpDown.Location = new System.Drawing.Point(165, 100);
            this.Spk0AlphaCompNumericUpDown.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.Spk0AlphaCompNumericUpDown.Name = "Spk0AlphaCompNumericUpDown";
            this.Spk0AlphaCompNumericUpDown.Size = new System.Drawing.Size(67, 20);
            this.Spk0AlphaCompNumericUpDown.TabIndex = 3;
            // 
            // Spk0AlphaCompLabel
            // 
            this.Spk0AlphaCompLabel.AutoSize = true;
            this.Spk0AlphaCompLabel.Location = new System.Drawing.Point(55, 103);
            this.Spk0AlphaCompLabel.Name = "Spk0AlphaCompLabel";
            this.Spk0AlphaCompLabel.Size = new System.Drawing.Size(107, 13);
            this.Spk0AlphaCompLabel.TabIndex = 4;
            this.Spk0AlphaCompLabel.Text = "Alpha Compensation:";
            // 
            // SideSPK0
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 188);
            this.Controls.Add(this.Spk0AlphaCompLabel);
            this.Controls.Add(this.Spk0AlphaCompNumericUpDown);
            this.Controls.Add(this.Spk0SaveAsLabel);
            this.Controls.Add(this.Spk0SaveAsComboBox);
            this.Controls.Add(this.ConvertSPK0ImgBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SideSPK0";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SPK0 Converter";
            ((System.ComponentModel.ISupportInitialize)(this.Spk0AlphaCompNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConvertSPK0ImgBtn;
        private System.Windows.Forms.ComboBox Spk0SaveAsComboBox;
        private System.Windows.Forms.Label Spk0SaveAsLabel;
        private System.Windows.Forms.NumericUpDown Spk0AlphaCompNumericUpDown;
        private System.Windows.Forms.Label Spk0AlphaCompLabel;
    }
}