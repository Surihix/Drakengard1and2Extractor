namespace Drakengard1and2Extractor.FileRepack
{
    partial class RepackForm
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
            this.BinRepackGroupBox = new System.Windows.Forms.GroupBox();
            this.RpkBinBtn = new System.Windows.Forms.Button();
            this.BINFileRpkGrp = new System.Windows.Forms.GroupBox();
            this.Drk2RadioButton = new System.Windows.Forms.RadioButton();
            this.Drk1RadioButton = new System.Windows.Forms.RadioButton();
            this.FileRpkGrp = new System.Windows.Forms.GroupBox();
            this.RpkKpsBtn = new System.Windows.Forms.Button();
            this.RpkDpkBtn = new System.Windows.Forms.Button();
            this.RpkFpkBtn = new System.Windows.Forms.Button();
            this.RepackStatusDelBtn = new System.Windows.Forms.Button();
            this.RepackStatusTextBox = new System.Windows.Forms.TextBox();
            this.RepackStatusLabel = new System.Windows.Forms.Label();
            this.BINRpkToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RpkFpkToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RpkDpkToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RpkKpsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BinRepackGroupBox.SuspendLayout();
            this.BINFileRpkGrp.SuspendLayout();
            this.FileRpkGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // BinRepackGroupBox
            // 
            this.BinRepackGroupBox.Controls.Add(this.RpkBinBtn);
            this.BinRepackGroupBox.Controls.Add(this.BINFileRpkGrp);
            this.BinRepackGroupBox.Location = new System.Drawing.Point(13, 13);
            this.BinRepackGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.BinRepackGroupBox.Name = "BinRepackGroupBox";
            this.BinRepackGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.BinRepackGroupBox.Size = new System.Drawing.Size(224, 226);
            this.BinRepackGroupBox.TabIndex = 3;
            this.BinRepackGroupBox.TabStop = false;
            this.BinRepackGroupBox.Text = "BIN Repack :";
            // 
            // RpkBinBtn
            // 
            this.RpkBinBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RpkBinBtn.Location = new System.Drawing.Point(40, 158);
            this.RpkBinBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RpkBinBtn.Name = "RpkBinBtn";
            this.RpkBinBtn.Size = new System.Drawing.Size(137, 48);
            this.RpkBinBtn.TabIndex = 0;
            this.RpkBinBtn.Text = "Repack BIN";
            this.RpkBinBtn.UseVisualStyleBackColor = true;
            this.RpkBinBtn.Click += new System.EventHandler(this.RpkBinBtn_Click);
            this.RpkBinBtn.MouseHover += new System.EventHandler(this.RpkBinBtn_MouseHover);
            // 
            // BINFileRpkGrp
            // 
            this.BINFileRpkGrp.Controls.Add(this.Drk2RadioButton);
            this.BINFileRpkGrp.Controls.Add(this.Drk1RadioButton);
            this.BINFileRpkGrp.Location = new System.Drawing.Point(19, 38);
            this.BINFileRpkGrp.Margin = new System.Windows.Forms.Padding(4);
            this.BINFileRpkGrp.Name = "BINFileRpkGrp";
            this.BINFileRpkGrp.Padding = new System.Windows.Forms.Padding(4);
            this.BINFileRpkGrp.Size = new System.Drawing.Size(187, 97);
            this.BINFileRpkGrp.TabIndex = 0;
            this.BINFileRpkGrp.TabStop = false;
            this.BINFileRpkGrp.Text = "Game Selection :";
            // 
            // Drk2RadioButton
            // 
            this.Drk2RadioButton.AutoSize = true;
            this.Drk2RadioButton.Location = new System.Drawing.Point(17, 55);
            this.Drk2RadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.Drk2RadioButton.Name = "Drk2RadioButton";
            this.Drk2RadioButton.Size = new System.Drawing.Size(110, 20);
            this.Drk2RadioButton.TabIndex = 1;
            this.Drk2RadioButton.TabStop = true;
            this.Drk2RadioButton.Text = "Drakengard 2";
            this.Drk2RadioButton.UseVisualStyleBackColor = true;
            // 
            // Drk1RadioButton
            // 
            this.Drk1RadioButton.AutoSize = true;
            this.Drk1RadioButton.Location = new System.Drawing.Point(17, 27);
            this.Drk1RadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.Drk1RadioButton.Name = "Drk1RadioButton";
            this.Drk1RadioButton.Size = new System.Drawing.Size(110, 20);
            this.Drk1RadioButton.TabIndex = 0;
            this.Drk1RadioButton.TabStop = true;
            this.Drk1RadioButton.Text = "Drakengard 1";
            this.Drk1RadioButton.UseVisualStyleBackColor = true;
            // 
            // FileRpkGrp
            // 
            this.FileRpkGrp.Controls.Add(this.RpkKpsBtn);
            this.FileRpkGrp.Controls.Add(this.RpkDpkBtn);
            this.FileRpkGrp.Controls.Add(this.RpkFpkBtn);
            this.FileRpkGrp.Location = new System.Drawing.Point(251, 13);
            this.FileRpkGrp.Margin = new System.Windows.Forms.Padding(4);
            this.FileRpkGrp.Name = "FileRpkGrp";
            this.FileRpkGrp.Padding = new System.Windows.Forms.Padding(4);
            this.FileRpkGrp.Size = new System.Drawing.Size(224, 170);
            this.FileRpkGrp.TabIndex = 4;
            this.FileRpkGrp.TabStop = false;
            this.FileRpkGrp.Text = "File Repack :";
            // 
            // RpkKpsBtn
            // 
            this.RpkKpsBtn.Location = new System.Drawing.Point(56, 111);
            this.RpkKpsBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RpkKpsBtn.Name = "RpkKpsBtn";
            this.RpkKpsBtn.Size = new System.Drawing.Size(112, 34);
            this.RpkKpsBtn.TabIndex = 3;
            this.RpkKpsBtn.Text = "Repack KPS";
            this.RpkKpsBtn.UseVisualStyleBackColor = true;
            this.RpkKpsBtn.Click += new System.EventHandler(this.RpkKpsBtn_Click);
            this.RpkKpsBtn.MouseHover += new System.EventHandler(this.RpkKpsBtn_MouseHover);
            // 
            // RpkDpkBtn
            // 
            this.RpkDpkBtn.Location = new System.Drawing.Point(56, 69);
            this.RpkDpkBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RpkDpkBtn.Name = "RpkDpkBtn";
            this.RpkDpkBtn.Size = new System.Drawing.Size(112, 34);
            this.RpkDpkBtn.TabIndex = 1;
            this.RpkDpkBtn.Text = "Repack DPK";
            this.RpkDpkBtn.UseVisualStyleBackColor = true;
            this.RpkDpkBtn.Click += new System.EventHandler(this.RpkDpkBtn_Click);
            this.RpkDpkBtn.MouseHover += new System.EventHandler(this.RpkDpkBtn_MouseHover);
            // 
            // RpkFpkBtn
            // 
            this.RpkFpkBtn.Location = new System.Drawing.Point(56, 27);
            this.RpkFpkBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RpkFpkBtn.Name = "RpkFpkBtn";
            this.RpkFpkBtn.Size = new System.Drawing.Size(112, 34);
            this.RpkFpkBtn.TabIndex = 0;
            this.RpkFpkBtn.Text = "Repack FPK";
            this.RpkFpkBtn.UseVisualStyleBackColor = true;
            this.RpkFpkBtn.Click += new System.EventHandler(this.RpkFpkBtn_Click);
            this.RpkFpkBtn.MouseHover += new System.EventHandler(this.RpkFpkBtn_MouseHover);
            // 
            // RepackStatusDelBtn
            // 
            this.RepackStatusDelBtn.Location = new System.Drawing.Point(193, 432);
            this.RepackStatusDelBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RepackStatusDelBtn.Name = "RepackStatusDelBtn";
            this.RepackStatusDelBtn.Size = new System.Drawing.Size(100, 28);
            this.RepackStatusDelBtn.TabIndex = 13;
            this.RepackStatusDelBtn.Text = "Clear Status";
            this.RepackStatusDelBtn.UseVisualStyleBackColor = true;
            this.RepackStatusDelBtn.Click += new System.EventHandler(this.RepackStatusDelBtn_Click);
            // 
            // RepackStatusTextBox
            // 
            this.RepackStatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RepackStatusTextBox.Location = new System.Drawing.Point(13, 298);
            this.RepackStatusTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.RepackStatusTextBox.Multiline = true;
            this.RepackStatusTextBox.Name = "RepackStatusTextBox";
            this.RepackStatusTextBox.ReadOnly = true;
            this.RepackStatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RepackStatusTextBox.Size = new System.Drawing.Size(462, 116);
            this.RepackStatusTextBox.TabIndex = 12;
            // 
            // RepackStatusLabel
            // 
            this.RepackStatusLabel.AutoSize = true;
            this.RepackStatusLabel.Location = new System.Drawing.Point(13, 278);
            this.RepackStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RepackStatusLabel.Name = "RepackStatusLabel";
            this.RepackStatusLabel.Size = new System.Drawing.Size(47, 16);
            this.RepackStatusLabel.TabIndex = 11;
            this.RepackStatusLabel.Text = "Status:";
            // 
            // RepackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 479);
            this.Controls.Add(this.RepackStatusDelBtn);
            this.Controls.Add(this.RepackStatusTextBox);
            this.Controls.Add(this.RepackStatusLabel);
            this.Controls.Add(this.FileRpkGrp);
            this.Controls.Add(this.BinRepackGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "RepackForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Repack Tools (Experimental)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RepackForm_FormClosing);
            this.Shown += new System.EventHandler(this.RepackForm_Shown);
            this.BinRepackGroupBox.ResumeLayout(false);
            this.BINFileRpkGrp.ResumeLayout(false);
            this.BINFileRpkGrp.PerformLayout();
            this.FileRpkGrp.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox BinRepackGroupBox;
        private System.Windows.Forms.Button RpkBinBtn;
        private System.Windows.Forms.GroupBox BINFileRpkGrp;
        private System.Windows.Forms.RadioButton Drk2RadioButton;
        private System.Windows.Forms.RadioButton Drk1RadioButton;
        private System.Windows.Forms.GroupBox FileRpkGrp;
        private System.Windows.Forms.Button RpkKpsBtn;
        private System.Windows.Forms.Button RpkDpkBtn;
        private System.Windows.Forms.Button RpkFpkBtn;
        private System.Windows.Forms.Button RepackStatusDelBtn;
        private System.Windows.Forms.TextBox RepackStatusTextBox;
        private System.Windows.Forms.Label RepackStatusLabel;
        private System.Windows.Forms.ToolTip BINRpkToolTip;
        private System.Windows.Forms.ToolTip RpkFpkToolTip;
        private System.Windows.Forms.ToolTip RpkDpkToolTip;
        private System.Windows.Forms.ToolTip RpkKpsToolTip;
    }
}