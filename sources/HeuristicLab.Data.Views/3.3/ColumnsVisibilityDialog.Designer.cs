﻿namespace HeuristicLab.Data.Views {
  partial class ColumnsVisibilityDialog {
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
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.checkedListBox = new System.Windows.Forms.CheckedListBox();
      this.SuspendLayout();
      // 
      // checkedListBox
      // 
      this.checkedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.checkedListBox.CheckOnClick = true;
      this.checkedListBox.FormattingEnabled = true;
      this.checkedListBox.Location = new System.Drawing.Point(12, 12);
      this.checkedListBox.Name = "checkedListBox";
      this.checkedListBox.Size = new System.Drawing.Size(171, 244);
      this.checkedListBox.TabIndex = 0;
      this.checkedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_ItemCheck);
      // 
      // ColumnsVisibilityDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(194, 273);
      this.Controls.Add(this.checkedListBox);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ColumnsVisibilityDialog";
      this.ShowInTaskbar = false;
      this.Text = "Show / Hide Columns";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckedListBox checkedListBox;
  }
}