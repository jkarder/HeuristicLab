﻿namespace HeuristicLab.Hive.Server
{
    partial class MainForm
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
          this.label1 = new System.Windows.Forms.Label();
          this.lblAddress1 = new System.Windows.Forms.Label();
          this.label2 = new System.Windows.Forms.Label();
          this.lblAddress2 = new System.Windows.Forms.Label();
          this.SuspendLayout();
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(18, 18);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(113, 13);
          this.label1.TabIndex = 0;
          this.label1.Text = "Hive server running @";
          // 
          // lblAddress1
          // 
          this.lblAddress1.AutoSize = true;
          this.lblAddress1.Location = new System.Drawing.Point(18, 43);
          this.lblAddress1.Name = "lblAddress1";
          this.lblAddress1.Size = new System.Drawing.Size(44, 13);
          this.lblAddress1.TabIndex = 1;
          this.lblAddress1.Text = "address";
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(18, 76);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(195, 13);
          this.label2.TabIndex = 2;
          this.label2.Text = "Hive Server Console Facade running @";
          // 
          // lblAddress2
          // 
          this.lblAddress2.AutoSize = true;
          this.lblAddress2.Location = new System.Drawing.Point(18, 102);
          this.lblAddress2.Name = "lblAddress2";
          this.lblAddress2.Size = new System.Drawing.Size(44, 13);
          this.lblAddress2.TabIndex = 3;
          this.lblAddress2.Text = "address";
          // 
          // MainForm
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(304, 128);
          this.Controls.Add(this.lblAddress2);
          this.Controls.Add(this.label2);
          this.Controls.Add(this.lblAddress1);
          this.Controls.Add(this.label1);
          this.Name = "MainForm";
          this.Text = "Hive Server Console";
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAddress2;
    }
}