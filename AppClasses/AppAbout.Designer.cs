﻿namespace Drakengard1and2Extractor
{
    partial class AppAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppAbout));
            this.AboutWindowBtn = new System.Windows.Forms.Button();
            this.Creditlabel1 = new System.Windows.Forms.Label();
            this.Creditlabel2 = new System.Windows.Forms.Label();
            this.Creditlabel3 = new System.Windows.Forms.Label();
            this.AppAboutPictureBox = new System.Windows.Forms.PictureBox();
            this.CreditLabel4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AppAboutPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AboutWindowBtn
            // 
            this.AboutWindowBtn.Location = new System.Drawing.Point(111, 267);
            this.AboutWindowBtn.Name = "AboutWindowBtn";
            this.AboutWindowBtn.Size = new System.Drawing.Size(75, 23);
            this.AboutWindowBtn.TabIndex = 0;
            this.AboutWindowBtn.Text = "OK";
            this.AboutWindowBtn.UseVisualStyleBackColor = true;
            this.AboutWindowBtn.Click += new System.EventHandler(this.AboutWindowBtn_Click);
            // 
            // Creditlabel1
            // 
            this.Creditlabel1.AutoSize = true;
            this.Creditlabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Creditlabel1.Location = new System.Drawing.Point(8, 17);
            this.Creditlabel1.Name = "Creditlabel1";
            this.Creditlabel1.Size = new System.Drawing.Size(144, 15);
            this.Creditlabel1.TabIndex = 2;
            this.Creditlabel1.Text = "App Programmer : Surihix";
            // 
            // Creditlabel2
            // 
            this.Creditlabel2.AutoSize = true;
            this.Creditlabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Creditlabel2.Location = new System.Drawing.Point(8, 48);
            this.Creditlabel2.Name = "Creditlabel2";
            this.Creditlabel2.Size = new System.Drawing.Size(132, 15);
            this.Creditlabel2.TabIndex = 3;
            this.Creditlabel2.Text = "Minilzo Library : Bartz24";
            // 
            // Creditlabel3
            // 
            this.Creditlabel3.AutoSize = true;
            this.Creditlabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Creditlabel3.Location = new System.Drawing.Point(8, 67);
            this.Creditlabel3.Name = "Creditlabel3";
            this.Creditlabel3.Size = new System.Drawing.Size(142, 15);
            this.Creditlabel3.TabIndex = 4;
            this.Creditlabel3.Text = "PS2 Palette Swizzle : Mugi";
            // 
            // AppAboutPictureBox
            // 
            this.AppAboutPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("AppAboutPictureBox.Image")));
            this.AppAboutPictureBox.Location = new System.Drawing.Point(23, 12);
            this.AppAboutPictureBox.Name = "AppAboutPictureBox";
            this.AppAboutPictureBox.Size = new System.Drawing.Size(59, 100);
            this.AppAboutPictureBox.TabIndex = 1;
            this.AppAboutPictureBox.TabStop = false;
            // 
            // CreditLabel4
            // 
            this.CreditLabel4.AutoSize = true;
            this.CreditLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditLabel4.Location = new System.Drawing.Point(8, 86);
            this.CreditLabel4.Name = "CreditLabel4";
            this.CreditLabel4.Size = new System.Drawing.Size(173, 15);
            this.CreditLabel4.TabIndex = 5;
            this.CreditLabel4.Text = "Folder Picker : Simon Mourier ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Creditlabel3);
            this.groupBox1.Controls.Add(this.Creditlabel1);
            this.groupBox1.Controls.Add(this.CreditLabel4);
            this.groupBox1.Controls.Add(this.Creditlabel2);
            this.groupBox1.Location = new System.Drawing.Point(12, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 110);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Credits :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(89, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Drakengard 1 and 2 - File Extractor";
            // 
            // AppAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 302);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.AppAboutPictureBox);
            this.Controls.Add(this.AboutWindowBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.AppAboutPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AboutWindowBtn;
        private System.Windows.Forms.PictureBox AppAboutPictureBox;
        private System.Windows.Forms.Label Creditlabel1;
        private System.Windows.Forms.Label Creditlabel2;
        private System.Windows.Forms.Label Creditlabel3;
        private System.Windows.Forms.Label CreditLabel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}