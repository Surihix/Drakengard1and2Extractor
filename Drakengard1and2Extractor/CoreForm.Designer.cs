﻿namespace Drakengard1and2Extractor
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
            this.ExtDpkBtn = new System.Windows.Forms.Button();
            this.ExtFpkBtn = new System.Windows.Forms.Button();
            this.BatchModeBtn = new System.Windows.Forms.Button();
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
            this.ImgConvtGroupBox = new System.Windows.Forms.GroupBox();
            this.BinExtractionGroupBox = new System.Windows.Forms.GroupBox();
            this.BatchModeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.StatusTextBox = new System.Windows.Forms.TextBox();
            this.StatusDelBtn = new System.Windows.Forms.Button();
            this.KPStoolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RpkToolsBtn = new System.Windows.Forms.Button();
            this.RpkToolsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BINFileExtGrp.SuspendLayout();
            this.FileExtGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppPictureBox)).BeginInit();
            this.ImgConvtGroupBox.SuspendLayout();
            this.BinExtractionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BINFileExtGrp
            // 
            this.BINFileExtGrp.Controls.Add(this.Drk2RadioButton);
            this.BINFileExtGrp.Controls.Add(this.Drk1RadioButton);
            this.BINFileExtGrp.Location = new System.Drawing.Point(19, 38);
            this.BINFileExtGrp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BINFileExtGrp.Name = "BINFileExtGrp";
            this.BINFileExtGrp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BINFileExtGrp.Size = new System.Drawing.Size(187, 97);
            this.BINFileExtGrp.TabIndex = 0;
            this.BINFileExtGrp.TabStop = false;
            this.BINFileExtGrp.Text = "Game Selection :";
            // 
            // Drk2RadioButton
            // 
            this.Drk2RadioButton.AutoSize = true;
            this.Drk2RadioButton.Location = new System.Drawing.Point(17, 55);
            this.Drk2RadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.Drk1RadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Drk1RadioButton.Name = "Drk1RadioButton";
            this.Drk1RadioButton.Size = new System.Drawing.Size(110, 20);
            this.Drk1RadioButton.TabIndex = 0;
            this.Drk1RadioButton.TabStop = true;
            this.Drk1RadioButton.Text = "Drakengard 1";
            this.Drk1RadioButton.UseVisualStyleBackColor = true;
            // 
            // ExtBinBtn
            // 
            this.ExtBinBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExtBinBtn.Location = new System.Drawing.Point(40, 158);
            this.ExtBinBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExtBinBtn.Name = "ExtBinBtn";
            this.ExtBinBtn.Size = new System.Drawing.Size(137, 48);
            this.ExtBinBtn.TabIndex = 0;
            this.ExtBinBtn.Text = "Extract BIN";
            this.ExtBinBtn.UseVisualStyleBackColor = true;
            this.ExtBinBtn.Click += new System.EventHandler(this.ExtBinBtn_Click);
            this.ExtBinBtn.MouseHover += new System.EventHandler(this.ExtBinBtn_MouseHover);
            // 
            // FileExtGrp
            // 
            this.FileExtGrp.Controls.Add(this.ExtKpsBtn);
            this.FileExtGrp.Controls.Add(this.ExtDpkBtn);
            this.FileExtGrp.Controls.Add(this.ExtFpkBtn);
            this.FileExtGrp.Location = new System.Drawing.Point(267, 175);
            this.FileExtGrp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FileExtGrp.Name = "FileExtGrp";
            this.FileExtGrp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FileExtGrp.Size = new System.Drawing.Size(224, 170);
            this.FileExtGrp.TabIndex = 1;
            this.FileExtGrp.TabStop = false;
            this.FileExtGrp.Text = "File Extraction :";
            // 
            // ExtKpsBtn
            // 
            this.ExtKpsBtn.Location = new System.Drawing.Point(56, 111);
            this.ExtKpsBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExtKpsBtn.Name = "ExtKpsBtn";
            this.ExtKpsBtn.Size = new System.Drawing.Size(112, 34);
            this.ExtKpsBtn.TabIndex = 3;
            this.ExtKpsBtn.Text = "Extract KPS";
            this.ExtKpsBtn.UseVisualStyleBackColor = true;
            this.ExtKpsBtn.Click += new System.EventHandler(this.ExtKpsBtn_Click);
            this.ExtKpsBtn.MouseHover += new System.EventHandler(this.ExtKpsBtn_MouseHover);
            // 
            // ExtDpkBtn
            // 
            this.ExtDpkBtn.Location = new System.Drawing.Point(56, 69);
            this.ExtDpkBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExtDpkBtn.Name = "ExtDpkBtn";
            this.ExtDpkBtn.Size = new System.Drawing.Size(112, 34);
            this.ExtDpkBtn.TabIndex = 1;
            this.ExtDpkBtn.Text = "Extract DPK";
            this.ExtDpkBtn.UseVisualStyleBackColor = true;
            this.ExtDpkBtn.Click += new System.EventHandler(this.ExtDpkBtn_Click);
            this.ExtDpkBtn.MouseHover += new System.EventHandler(this.ExtDpkBtn_MouseHover);
            // 
            // ExtFpkBtn
            // 
            this.ExtFpkBtn.Location = new System.Drawing.Point(56, 27);
            this.ExtFpkBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExtFpkBtn.Name = "ExtFpkBtn";
            this.ExtFpkBtn.Size = new System.Drawing.Size(112, 34);
            this.ExtFpkBtn.TabIndex = 0;
            this.ExtFpkBtn.Text = "Extract FPK";
            this.ExtFpkBtn.UseVisualStyleBackColor = true;
            this.ExtFpkBtn.Click += new System.EventHandler(this.ExtFpkBtn_Click);
            this.ExtFpkBtn.MouseHover += new System.EventHandler(this.ExtFpkBtn_MouseHover);
            // 
            // BatchModeBtn
            // 
            this.BatchModeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchModeBtn.Location = new System.Drawing.Point(323, 480);
            this.BatchModeBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BatchModeBtn.Name = "BatchModeBtn";
            this.BatchModeBtn.Size = new System.Drawing.Size(121, 44);
            this.BatchModeBtn.TabIndex = 2;
            this.BatchModeBtn.Text = "Batch Mode";
            this.BatchModeBtn.UseVisualStyleBackColor = true;
            this.BatchModeBtn.Click += new System.EventHandler(this.BatchModeBtn_Click);
            this.BatchModeBtn.MouseHover += new System.EventHandler(this.BatchModeBtn_MouseHover);
            // 
            // ConvertSPK0Btn
            // 
            this.ConvertSPK0Btn.Location = new System.Drawing.Point(55, 79);
            this.ConvertSPK0Btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ConvertSPK0Btn.Name = "ConvertSPK0Btn";
            this.ConvertSPK0Btn.Size = new System.Drawing.Size(112, 34);
            this.ConvertSPK0Btn.TabIndex = 1;
            this.ConvertSPK0Btn.Text = "Convert SPK0";
            this.ConvertSPK0Btn.UseVisualStyleBackColor = true;
            this.ConvertSPK0Btn.Click += new System.EventHandler(this.ConvertSPK0Btn_Click);
            this.ConvertSPK0Btn.MouseHover += new System.EventHandler(this.ConvertSPK0Btn_MouseHover);
            // 
            // ConvertZIMBtn
            // 
            this.ConvertZIMBtn.Location = new System.Drawing.Point(53, 37);
            this.ConvertZIMBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ConvertZIMBtn.Name = "ConvertZIMBtn";
            this.ConvertZIMBtn.Size = new System.Drawing.Size(112, 34);
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
            this.AboutlinkLabel.Location = new System.Drawing.Point(19, 807);
            this.AboutlinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AboutlinkLabel.Name = "AboutlinkLabel";
            this.AboutlinkLabel.Size = new System.Drawing.Size(46, 18);
            this.AboutlinkLabel.TabIndex = 6;
            this.AboutlinkLabel.TabStop = true;
            this.AboutlinkLabel.Text = "About";
            this.AboutlinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AboutlinkLabel_LinkClicked);
            // 
            // HelpLinkLabel
            // 
            this.HelpLinkLabel.AutoSize = true;
            this.HelpLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpLinkLabel.Location = new System.Drawing.Point(447, 807);
            this.HelpLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HelpLinkLabel.Name = "HelpLinkLabel";
            this.HelpLinkLabel.Size = new System.Drawing.Size(38, 18);
            this.HelpLinkLabel.TabIndex = 8;
            this.HelpLinkLabel.TabStop = true;
            this.HelpLinkLabel.Text = "Help";
            this.HelpLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HelpLinkLabel_LinkClicked);
            // 
            // Versionlabel
            // 
            this.Versionlabel.AutoSize = true;
            this.Versionlabel.Location = new System.Drawing.Point(235, 810);
            this.Versionlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Versionlabel.Name = "Versionlabel";
            this.Versionlabel.Size = new System.Drawing.Size(31, 16);
            this.Versionlabel.TabIndex = 7;
            this.Versionlabel.Text = "v2.5";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(19, 596);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(47, 16);
            this.StatusLabel.TabIndex = 4;
            this.StatusLabel.Text = "Status:";
            // 
            // AppPictureBox
            // 
            this.AppPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("AppPictureBox.Image")));
            this.AppPictureBox.Location = new System.Drawing.Point(17, 16);
            this.AppPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AppPictureBox.Name = "AppPictureBox";
            this.AppPictureBox.Size = new System.Drawing.Size(475, 132);
            this.AppPictureBox.TabIndex = 1;
            this.AppPictureBox.TabStop = false;
            // 
            // ImgConvtGroupBox
            // 
            this.ImgConvtGroupBox.Controls.Add(this.ConvertZIMBtn);
            this.ImgConvtGroupBox.Controls.Add(this.ConvertSPK0Btn);
            this.ImgConvtGroupBox.Location = new System.Drawing.Point(23, 420);
            this.ImgConvtGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ImgConvtGroupBox.Name = "ImgConvtGroupBox";
            this.ImgConvtGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ImgConvtGroupBox.Size = new System.Drawing.Size(223, 142);
            this.ImgConvtGroupBox.TabIndex = 3;
            this.ImgConvtGroupBox.TabStop = false;
            this.ImgConvtGroupBox.Text = "Image Conversion :";
            // 
            // BinExtractionGroupBox
            // 
            this.BinExtractionGroupBox.Controls.Add(this.ExtBinBtn);
            this.BinExtractionGroupBox.Controls.Add(this.BINFileExtGrp);
            this.BinExtractionGroupBox.Location = new System.Drawing.Point(23, 175);
            this.BinExtractionGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BinExtractionGroupBox.Name = "BinExtractionGroupBox";
            this.BinExtractionGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BinExtractionGroupBox.Size = new System.Drawing.Size(224, 226);
            this.BinExtractionGroupBox.TabIndex = 2;
            this.BinExtractionGroupBox.TabStop = false;
            this.BinExtractionGroupBox.Text = "BIN Extraction :";
            // 
            // StatusTextBox
            // 
            this.StatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusTextBox.Location = new System.Drawing.Point(17, 617);
            this.StatusTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StatusTextBox.Multiline = true;
            this.StatusTextBox.Name = "StatusTextBox";
            this.StatusTextBox.ReadOnly = true;
            this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StatusTextBox.Size = new System.Drawing.Size(474, 116);
            this.StatusTextBox.TabIndex = 9;
            // 
            // StatusDelBtn
            // 
            this.StatusDelBtn.Location = new System.Drawing.Point(204, 756);
            this.StatusDelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StatusDelBtn.Name = "StatusDelBtn";
            this.StatusDelBtn.Size = new System.Drawing.Size(100, 28);
            this.StatusDelBtn.TabIndex = 10;
            this.StatusDelBtn.Text = "Clear Status";
            this.StatusDelBtn.UseVisualStyleBackColor = true;
            this.StatusDelBtn.Click += new System.EventHandler(this.StatusDelBtn_Click);
            // 
            // RpkToolsBtn
            // 
            this.RpkToolsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RpkToolsBtn.Location = new System.Drawing.Point(305, 384);
            this.RpkToolsBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RpkToolsBtn.Name = "RpkToolsBtn";
            this.RpkToolsBtn.Size = new System.Drawing.Size(153, 44);
            this.RpkToolsBtn.TabIndex = 12;
            this.RpkToolsBtn.Text = "Repack tools";
            this.RpkToolsBtn.UseVisualStyleBackColor = true;
            this.RpkToolsBtn.Click += new System.EventHandler(this.RpkToolsBtn_Click);
            this.RpkToolsBtn.MouseHover += new System.EventHandler(this.RpkToolsBtn_MouseHover);
            // 
            // CoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 841);
            this.Controls.Add(this.RpkToolsBtn);
            this.Controls.Add(this.StatusDelBtn);
            this.Controls.Add(this.BatchModeBtn);
            this.Controls.Add(this.StatusTextBox);
            this.Controls.Add(this.BinExtractionGroupBox);
            this.Controls.Add(this.ImgConvtGroupBox);
            this.Controls.Add(this.FileExtGrp);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.Versionlabel);
            this.Controls.Add(this.HelpLinkLabel);
            this.Controls.Add(this.AboutlinkLabel);
            this.Controls.Add(this.AppPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "CoreForm";
            this.Text = "Drakengard 1 & 2 - File Extractor";
            this.Shown += new System.EventHandler(this.CoreForm_Shown);
            this.BINFileExtGrp.ResumeLayout(false);
            this.BINFileExtGrp.PerformLayout();
            this.FileExtGrp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AppPictureBox)).EndInit();
            this.ImgConvtGroupBox.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox ImgConvtGroupBox;
        private System.Windows.Forms.GroupBox BinExtractionGroupBox;
        private System.Windows.Forms.ToolTip BatchModeToolTip;
        private System.Windows.Forms.TextBox StatusTextBox;
        private System.Windows.Forms.Button ExtKpsBtn;
        private System.Windows.Forms.Button StatusDelBtn;
        private System.Windows.Forms.ToolTip KPStoolTip;
        private System.Windows.Forms.Button RpkToolsBtn;
        private System.Windows.Forms.ToolTip RpkToolsToolTip;
    }
}

