﻿namespace HeuristicLab.MainForm {
  partial class MultipleDocumentMainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      this.MdiChildActivate -= new System.EventHandler(MultipleDocumentFormActivated);
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.SuspendLayout();
      // 
      // MultipleDocumentMainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(596, 407);
      this.IsMdiContainer = true;
      this.Name = "MultipleDocumentMainForm";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
  }
}