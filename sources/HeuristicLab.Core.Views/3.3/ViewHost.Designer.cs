#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

namespace HeuristicLab.Core.Views {
  partial class ViewHost {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing) {
        if (components != null) components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.viewPanel = new System.Windows.Forms.Panel();
      this.viewLabel = new System.Windows.Forms.Label();
      this.viewComboBox = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // viewPanel
      // 
      this.viewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.viewPanel.Location = new System.Drawing.Point(0, 27);
      this.viewPanel.Name = "viewPanel";
      this.viewPanel.Size = new System.Drawing.Size(227, 157);
      this.viewPanel.TabIndex = 2;
      // 
      // viewLabel
      // 
      this.viewLabel.AutoSize = true;
      this.viewLabel.Location = new System.Drawing.Point(3, 3);
      this.viewLabel.Name = "viewLabel";
      this.viewLabel.Size = new System.Drawing.Size(33, 13);
      this.viewLabel.TabIndex = 0;
      this.viewLabel.Text = "&View:";
      // 
      // viewComboBox
      // 
      this.viewComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.viewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.viewComboBox.FormattingEnabled = true;
      this.viewComboBox.Location = new System.Drawing.Point(44, 0);
      this.viewComboBox.Name = "viewComboBox";
      this.viewComboBox.Size = new System.Drawing.Size(183, 21);
      this.viewComboBox.TabIndex = 1;
      this.viewComboBox.SelectedIndexChanged += new System.EventHandler(this.viewComboBox_SelectedIndexChanged);
      // 
      // ViewHost
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.viewComboBox);
      this.Controls.Add(this.viewLabel);
      this.Controls.Add(this.viewPanel);
      this.Name = "ViewHost";
      this.Size = new System.Drawing.Size(227, 184);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel viewPanel;
    private System.Windows.Forms.Label viewLabel;
    private System.Windows.Forms.ComboBox viewComboBox;

  }
}
