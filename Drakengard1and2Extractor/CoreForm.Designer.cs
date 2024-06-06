namespace Drakengard1and2Extractor
{
    partial class CoreForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoreForm));
            this.BINFileExtGrp = new System.Windows.Forms.GroupBox();
            this.Drk2RadioButton = new System.Windows.Forms.RadioButton();
            this.Drk1RadioButton = new System.Windows.Forms.RadioButton();
            this.ExtBinBtn = new System.Windows.Forms.Button();
            this.FileExtGrp = new System.Windows.Forms.GroupBox();
            this.ExtKpsBtn = new System.Windows.Forms.Button();
            this.BatchModeBtn = new System.Windows.Forms.Button();
            this.ExtDpkBtn = new System.Windows.Forms.Button();
            this.ExtFpkBtn = new System.Windows.Forms.Button();
            this.ConvertSPK0Btn = new System.Windows.Forms.Button();
            this.ConvertZIMBtn = new System.Windows.Forms.Button();
            this.AboutlinkLabel = new System.Windows.Forms.LinkLabel();
            this.HelpLinkLabel = new System.Windows.Forms.LinkLabel();
            this.Versionlabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.BINtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.FPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.DPKtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ZIMtoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SPK0toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.AppPictureBox = new System.Windows.Forms.PictureBox();
            this.ImgConvertersGroupBox = new System.Windows.Forms.GroupBox();
            this.BinExtractionGroupBox = new System.Windows.Forms.GroupBox();
            this.BatchModeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.StatusTextBox = new System.Windows.Forms.TextBox();
            this.StatusDelBtn = new System.Windows.Forms.Button();
            this.KPStoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BINFileExtGrp.SuspendLayout();
            this.FileExtGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppPictureBox)).BeginInit();
            this.ImgConvertersGroupBox.SuspendLayout();
            this.BinExtractionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BINFileExtGrp
            // 
            this.BINFileExtGrp.Controls.Add(this.Drk2RadioButton);
            this.BINFileExtGrp.Controls.Add(this.Drk1RadioButton);
            this.BINFileExtGrp.Location = new System.Drawing.Point(12, 141);
            this.BINFileExtGrp.Name = "BINFileExtGrp";
            this.BINFileExtGrp.Size = new System.Drawing.Size(168, 79);
            this.BINFileExtGrp.TabIndex = 0;
            this.BINFileExtGrp.TabStop = false;
            this.BINFileExtGrp.Text = "Game Selection :";
            // 
            // Drk2RadioButton
            // 
            this.Drk2RadioButton.AutoSize = true;
            this.Drk2RadioButton.Location = new System.Drawing.Point(13, 45);
            this.Drk2RadioButton.Name = "Drk2RadioButton";
            this.Drk2RadioButton.Size = new System.Drawing.Size(90, 17);
            this.Drk2RadioButton.TabIndex = 1;
            this.Drk2RadioButton.TabStop = true;
            this.Drk2RadioButton.Text = "Drakengard 2";
            this.Drk2RadioButton.UseVisualStyleBackColor = true;
            this.Drk2RadioButton.CheckedChanged += new System.EventHandler(this.Drk2RadioButton_CheckedChanged);
            // 
            // Drk1RadioButton
            // 
            this.Drk1RadioButton.AutoSize = true;
            this.Drk1RadioButton.Location = new System.Drawing.Point(13, 22);
            this.Drk1RadioButton.Name = "Drk1RadioButton";
            this.Drk1RadioButton.Size = new System.Drawing.Size(90, 17);
            this.Drk1RadioButton.TabIndex = 0;
            this.Drk1RadioButton.TabStop = true;
            this.Drk1RadioButton.Text = "Drakengard 1";
            this.Drk1RadioButton.UseVisualStyleBackColor = true;
            this.Drk1RadioButton.CheckedChanged += new System.EventHandler(this.Drk1RadioButton_CheckedChanged);
            // 
            // ExtBinBtn
            // 
            this.ExtBinBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExtBinBtn.Location = new System.Drawing.Point(33, 38);
            this.ExtBinBtn.Name = "ExtBinBtn";
            this.ExtBinBtn.Size = new System.Drawing.Size(103, 39);
            this.ExtBinBtn.TabIndex = 0;
            this.ExtBinBtn.Text = "Extract BIN";
            this.ExtBinBtn.UseVisualStyleBackColor = true;
            this.ExtBinBtn.Click += new System.EventHandler(this.ExtBinBtn_Click);
            this.ExtBinBtn.MouseHover += new System.EventHandler(this.ExtBinBtn_MouseHover);
            // 
            // FileExtGrp
            // 
            this.FileExtGrp.Controls.Add(this.ExtKpsBtn);
            this.FileExtGrp.Controls.Add(this.BatchModeBtn);
            this.FileExtGrp.Controls.Add(this.ExtDpkBtn);
            this.FileExtGrp.Controls.Add(this.ExtFpkBtn);
            this.FileExtGrp.Location = new System.Drawing.Point(12, 226);
            this.FileExtGrp.Name = "FileExtGrp";
            this.FileExtGrp.Size = new System.Drawing.Size(168, 168);
            this.FileExtGrp.TabIndex = 1;
            this.FileExtGrp.TabStop = false;
            this.FileExtGrp.Text = "File Extraction :";
            // 
            // ExtKpsBtn
            // 
            this.ExtKpsBtn.Location = new System.Drawing.Point(42, 90);
            this.ExtKpsBtn.Name = "ExtKpsBtn";
            this.ExtKpsBtn.Size = new System.Drawing.Size(84, 28);
            this.ExtKpsBtn.TabIndex = 3;
            this.ExtKpsBtn.Text = "Extract KPS";
            this.ExtKpsBtn.UseVisualStyleBackColor = true;
            this.ExtKpsBtn.Click += new System.EventHandler(this.ExtKpsBtn_Click);
            this.ExtKpsBtn.MouseHover += new System.EventHandler(this.ExtKpsBtn_MouseHover);
            // 
            // BatchModeBtn
            // 
            this.BatchModeBtn.Location = new System.Drawing.Point(42, 124);
            this.BatchModeBtn.Name = "BatchModeBtn";
            this.BatchModeBtn.Size = new System.Drawing.Size(84, 28);
            this.BatchModeBtn.TabIndex = 2;
            this.BatchModeBtn.Text = "Batch Mode";
            this.BatchModeBtn.UseVisualStyleBackColor = true;
            this.BatchModeBtn.Click += new System.EventHandler(this.BatchModeBtn_Click);
            this.BatchModeBtn.MouseHover += new System.EventHandler(this.BatchModeBtn_MouseHover);
            // 
            // ExtDpkBtn
            // 
            this.ExtDpkBtn.Location = new System.Drawing.Point(42, 56);
            this.ExtDpkBtn.Name = "ExtDpkBtn";
            this.ExtDpkBtn.Size = new System.Drawing.Size(84, 28);
            this.ExtDpkBtn.TabIndex = 1;
            this.ExtDpkBtn.Text = "Extract DPK";
            this.ExtDpkBtn.UseVisualStyleBackColor = true;
            this.ExtDpkBtn.Click += new System.EventHandler(this.ExtDpkBtn_Click);
            this.ExtDpkBtn.MouseHover += new System.EventHandler(this.ExtDpkBtn_MouseHover);
            // 
            // ExtFpkBtn
            // 
            this.ExtFpkBtn.Location = new System.Drawing.Point(42, 22);
            this.ExtFpkBtn.Name = "ExtFpkBtn";
            this.ExtFpkBtn.Size = new System.Drawing.Size(84, 28);
            this.ExtFpkBtn.TabIndex = 0;
            this.ExtFpkBtn.Text = "Extract FPK";
            this.ExtFpkBtn.UseVisualStyleBackColor = true;
            this.ExtFpkBtn.Click += new System.EventHandler(this.ExtFpkBtn_Click);
            this.ExtFpkBtn.MouseHover += new System.EventHandler(this.ExtFpkBtn_MouseHover);
            // 
            // ConvertSPK0Btn
            // 
            this.ConvertSPK0Btn.Location = new System.Drawing.Point(45, 73);
            this.ConvertSPK0Btn.Name = "ConvertSPK0Btn";
            this.ConvertSPK0Btn.Size = new System.Drawing.Size(84, 28);
            this.ConvertSPK0Btn.TabIndex = 1;
            this.ConvertSPK0Btn.Text = "Convert SPK0";
            this.ConvertSPK0Btn.UseVisualStyleBackColor = true;
            this.ConvertSPK0Btn.Click += new System.EventHandler(this.ConvertSPK0Btn_Click);
            this.ConvertSPK0Btn.MouseHover += new System.EventHandler(this.ConvertSPK0Btn_MouseHover);
            // 
            // ConvertZIMBtn
            // 
            this.ConvertZIMBtn.Location = new System.Drawing.Point(45, 39);
            this.ConvertZIMBtn.Name = "ConvertZIMBtn";
            this.ConvertZIMBtn.Size = new System.Drawing.Size(84, 28);
            this.ConvertZIMBtn.TabIndex = 0;
            this.ConvertZIMBtn.Text = "Convert ZIM";
            this.ConvertZIMBtn.UseVisualStyleBackColor = true;
            this.ConvertZIMBtn.Click += new System.EventHandler(this.ConvertZIMBtn_Click);
            this.ConvertZIMBtn.MouseHover += new System.EventHandler(this.ConvertZIMBtn_MouseHover);
            // 
            // AboutlinkLabel
            // 
            this.AboutlinkLabel.AutoSize = true;
            this.AboutlinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboutlinkLabel.Location = new System.Drawing.Point(14, 562);
            this.AboutlinkLabel.Name = "AboutlinkLabel";
            this.AboutlinkLabel.Size = new System.Drawing.Size(38, 15);
            this.AboutlinkLabel.TabIndex = 6;
            this.AboutlinkLabel.TabStop = true;
            this.AboutlinkLabel.Text = "About";
            this.AboutlinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AboutlinkLabel_LinkClicked);
            // 
            // HelpLinkLabel
            // 
            this.HelpLinkLabel.AutoSize = true;
            this.HelpLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpLinkLabel.Location = new System.Drawing.Point(335, 562);
            this.HelpLinkLabel.Name = "HelpLinkLabel";
            this.HelpLinkLabel.Size = new System.Drawing.Size(33, 15);
            this.HelpLinkLabel.TabIndex = 8;
            this.HelpLinkLabel.TabStop = true;
            this.HelpLinkLabel.Text = "Help";
            this.HelpLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HelpLinkLabel_LinkClicked);
            // 
            // Versionlabel
            // 
            this.Versionlabel.AutoSize = true;
            this.Versionlabel.Location = new System.Drawing.Point(176, 564);
            this.Versionlabel.Name = "Versionlabel";
            this.Versionlabel.Size = new System.Drawing.Size(28, 13);
            this.Versionlabel.TabIndex = 7;
            this.Versionlabel.Text = "v2.3";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(14, 416);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(40, 13);
            this.StatusLabel.TabIndex = 4;
            this.StatusLabel.Text = "Status:";
            // 
            // AppPictureBox
            // 
            this.AppPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("AppPictureBox.Image")));
            this.AppPictureBox.Location = new System.Drawing.Point(13, 13);
            this.AppPictureBox.Name = "AppPictureBox";
            this.AppPictureBox.Size = new System.Drawing.Size(356, 107);
            this.AppPictureBox.TabIndex = 1;
            this.AppPictureBox.TabStop = false;
            // 
            // ImgConvertersGroupBox
            // 
            this.ImgConvertersGroupBox.Controls.Add(this.ConvertZIMBtn);
            this.ImgConvertersGroupBox.Controls.Add(this.ConvertSPK0Btn);
            this.ImgConvertersGroupBox.Location = new System.Drawing.Point(196, 259);
            this.ImgConvertersGroupBox.Name = "ImgConvertersGroupBox";
            this.ImgConvertersGroupBox.Size = new System.Drawing.Size(167, 135);
            this.ImgConvertersGroupBox.TabIndex = 3;
            this.ImgConvertersGroupBox.TabStop = false;
            this.ImgConvertersGroupBox.Text = "Convert Images :";
            // 
            // BinExtractionGroupBox
            // 
            this.BinExtractionGroupBox.Controls.Add(this.ExtBinBtn);
            this.BinExtractionGroupBox.Location = new System.Drawing.Point(195, 141);
            this.BinExtractionGroupBox.Name = "BinExtractionGroupBox";
            this.BinExtractionGroupBox.Size = new System.Drawing.Size(168, 112);
            this.BinExtractionGroupBox.TabIndex = 2;
            this.BinExtractionGroupBox.TabStop = false;
            this.BinExtractionGroupBox.Text = "BIN Extraction :";
            // 
            // StatusTextBox
            // 
            this.StatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusTextBox.Location = new System.Drawing.Point(13, 434);
            this.StatusTextBox.Multiline = true;
            this.StatusTextBox.Name = "StatusTextBox";
            this.StatusTextBox.ReadOnly = true;
            this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StatusTextBox.Size = new System.Drawing.Size(356, 71);
            this.StatusTextBox.TabIndex = 9;
            // 
            // StatusDelBtn
            // 
            this.StatusDelBtn.Location = new System.Drawing.Point(153, 517);
            this.StatusDelBtn.Name = "StatusDelBtn";
            this.StatusDelBtn.Size = new System.Drawing.Size(75, 23);
            this.StatusDelBtn.TabIndex = 10;
            this.StatusDelBtn.Text = "Clear Status";
            this.StatusDelBtn.UseVisualStyleBackColor = true;
            this.StatusDelBtn.Click += new System.EventHandler(this.StatusDelBtn_Click);
            // 
            // CoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 585);
            this.Controls.Add(this.StatusDelBtn);
            this.Controls.Add(this.StatusTextBox);
            this.Controls.Add(this.BinExtractionGroupBox);
            this.Controls.Add(this.ImgConvertersGroupBox);
            this.Controls.Add(this.FileExtGrp);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.Versionlabel);
            this.Controls.Add(this.HelpLinkLabel);
            this.Controls.Add(this.AboutlinkLabel);
            this.Controls.Add(this.BINFileExtGrp);
            this.Controls.Add(this.AppPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CoreForm";
            this.Text = "Drakengard 1 & 2 - File Extractor";
            this.BINFileExtGrp.ResumeLayout(false);
            this.BINFileExtGrp.PerformLayout();
            this.FileExtGrp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AppPictureBox)).EndInit();
            this.ImgConvertersGroupBox.ResumeLayout(false);
            this.BinExtractionGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox AppPictureBox;
        private System.Windows.Forms.GroupBox BINFileExtGrp;
        private System.Windows.Forms.Button ExtBinBtn;
        private System.Windows.Forms.RadioButton Drk2RadioButton;
        private System.Windows.Forms.RadioButton Drk1RadioButton;
        private System.Windows.Forms.GroupBox FileExtGrp;
        private System.Windows.Forms.Button ExtDpkBtn;
        private System.Windows.Forms.Button ExtFpkBtn;
        private System.Windows.Forms.LinkLabel AboutlinkLabel;
        private System.Windows.Forms.LinkLabel HelpLinkLabel;
        private System.Windows.Forms.Label Versionlabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ToolTip BINtoolTip;
        private System.Windows.Forms.ToolTip FPKtoolTip;
        private System.Windows.Forms.ToolTip DPKtoolTip;
        private System.Windows.Forms.Button ConvertZIMBtn;
        private System.Windows.Forms.Button ConvertSPK0Btn;
        private System.Windows.Forms.ToolTip ZIMtoolTip;
        private System.Windows.Forms.ToolTip SPK0toolTip;
        private System.Windows.Forms.Button BatchModeBtn;
        private System.Windows.Forms.GroupBox ImgConvertersGroupBox;
        private System.Windows.Forms.GroupBox BinExtractionGroupBox;
        private System.Windows.Forms.ToolTip BatchModeToolTip;
        private System.Windows.Forms.TextBox StatusTextBox;
        private System.Windows.Forms.Button ExtKpsBtn;
        private System.Windows.Forms.Button StatusDelBtn;
        private System.Windows.Forms.ToolTip KPStoolTip;
    }
}

