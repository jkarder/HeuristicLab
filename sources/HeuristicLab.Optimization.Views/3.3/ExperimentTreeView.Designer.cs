﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2011 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

namespace HeuristicLab.Optimization.Views {
  partial class ExperimentTreeView {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.optimizersGroupBox = new System.Windows.Forms.GroupBox();
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.showDetailsCheckBox = new System.Windows.Forms.CheckBox();
      this.removeButton = new System.Windows.Forms.Button();
      this.moveUpButton = new System.Windows.Forms.Button();
      this.moveDownButton = new System.Windows.Forms.Button();
      this.addButton = new System.Windows.Forms.Button();
      this.optimizerTreeView = new System.Windows.Forms.TreeView();
      this.imageList = new System.Windows.Forms.ImageList(this.components);
      this.detailsGroupBox = new System.Windows.Forms.GroupBox();
      this.detailsViewHost = new HeuristicLab.MainForm.WindowsForms.ViewHost();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.optimizersGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.detailsGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // optimizersGroupBox
      // 
      this.optimizersGroupBox.Controls.Add(this.splitContainer);
      this.optimizersGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.optimizersGroupBox.Location = new System.Drawing.Point(0, 0);
      this.optimizersGroupBox.Name = "optimizersGroupBox";
      this.optimizersGroupBox.Size = new System.Drawing.Size(627, 458);
      this.optimizersGroupBox.TabIndex = 16;
      this.optimizersGroupBox.TabStop = false;
      this.optimizersGroupBox.Text = "Optimizers";
      // 
      // splitContainer
      // 
      this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer.Location = new System.Drawing.Point(3, 16);
      this.splitContainer.Name = "splitContainer";
      // 
      // splitContainer.Panel1
      // 
      this.splitContainer.Panel1.Controls.Add(this.showDetailsCheckBox);
      this.splitContainer.Panel1.Controls.Add(this.removeButton);
      this.splitContainer.Panel1.Controls.Add(this.moveUpButton);
      this.splitContainer.Panel1.Controls.Add(this.moveDownButton);
      this.splitContainer.Panel1.Controls.Add(this.addButton);
      this.splitContainer.Panel1.Controls.Add(this.optimizerTreeView);
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.detailsGroupBox);
      this.splitContainer.Size = new System.Drawing.Size(621, 439);
      this.splitContainer.SplitterDistance = 198;
      this.splitContainer.TabIndex = 0;
      // 
      // showDetailsCheckBox
      // 
      this.showDetailsCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
      this.showDetailsCheckBox.Checked = true;
      this.showDetailsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.showDetailsCheckBox.Image = HeuristicLab.Common.Resources.VSImageLibrary.Properties;
      this.showDetailsCheckBox.Location = new System.Drawing.Point(124, 4);
      this.showDetailsCheckBox.Name = "showDetailsCheckBox";
      this.showDetailsCheckBox.Size = new System.Drawing.Size(24, 24);
      this.showDetailsCheckBox.TabIndex = 15;
      this.toolTip.SetToolTip(this.showDetailsCheckBox, "Show/Hide Details");
      this.showDetailsCheckBox.UseVisualStyleBackColor = true;
      this.showDetailsCheckBox.CheckedChanged += new System.EventHandler(this.showDetailsCheckBox_CheckedChanged);
      // 
      // removeButton
      // 
      this.removeButton.Enabled = false;
      this.removeButton.Image = HeuristicLab.Common.Resources.VSImageLibrary.Remove;
      this.removeButton.Location = new System.Drawing.Point(94, 4);
      this.removeButton.Name = "removeButton";
      this.removeButton.Size = new System.Drawing.Size(24, 24);
      this.removeButton.TabIndex = 14;
      this.toolTip.SetToolTip(this.removeButton, "Remove");
      this.removeButton.UseVisualStyleBackColor = true;
      this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
      // 
      // moveUpButton
      // 
      this.moveUpButton.Enabled = false;
      this.moveUpButton.Image = HeuristicLab.Common.Resources.VSImageLibrary.ArrowUp;
      this.moveUpButton.Location = new System.Drawing.Point(34, 4);
      this.moveUpButton.Name = "moveUpButton";
      this.moveUpButton.Size = new System.Drawing.Size(24, 24);
      this.moveUpButton.TabIndex = 12;
      this.toolTip.SetToolTip(this.moveUpButton, "Move Up");
      this.moveUpButton.UseVisualStyleBackColor = true;
      this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
      // 
      // moveDownButton
      // 
      this.moveDownButton.Enabled = false;
      this.moveDownButton.Image = HeuristicLab.Common.Resources.VSImageLibrary.ArrowDown;
      this.moveDownButton.Location = new System.Drawing.Point(64, 4);
      this.moveDownButton.Name = "moveDownButton";
      this.moveDownButton.Size = new System.Drawing.Size(24, 24);
      this.moveDownButton.TabIndex = 13;
      this.toolTip.SetToolTip(this.moveDownButton, "Move Down");
      this.moveDownButton.UseVisualStyleBackColor = true;
      this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
      // 
      // addButton
      // 
      this.addButton.Image = HeuristicLab.Common.Resources.VSImageLibrary.Add;
      this.addButton.Location = new System.Drawing.Point(4, 4);
      this.addButton.Name = "addButton";
      this.addButton.Size = new System.Drawing.Size(24, 24);
      this.addButton.TabIndex = 11;
      this.toolTip.SetToolTip(this.addButton, "Add");
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new System.EventHandler(this.addButton_Click);
      // 
      // optimizerTreeView
      // 
      this.optimizerTreeView.AllowDrop = true;
      this.optimizerTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.optimizerTreeView.HideSelection = false;
      this.optimizerTreeView.ImageIndex = 0;
      this.optimizerTreeView.ImageList = this.imageList;
      this.optimizerTreeView.Location = new System.Drawing.Point(4, 34);
      this.optimizerTreeView.Name = "optimizerTreeView";
      this.optimizerTreeView.SelectedImageIndex = 0;
      this.optimizerTreeView.Size = new System.Drawing.Size(191, 402);
      this.optimizerTreeView.TabIndex = 10;
      this.optimizerTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.optimizerTreeView_ItemDrag);
      this.optimizerTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.optimizerTreeView_DragDrop);
      this.optimizerTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.optimizerTreeView_DragEnter);
      this.optimizerTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.optimizerTreeView_DragOver);
      this.optimizerTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.optimizerTreeView_KeyDown);
      this.optimizerTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.optimizerTreeView_MouseDown);
      this.optimizerTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.optimizerTreeview_NodeMouseClick);
      this.optimizerTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(optimizerTreeView_NodeMouseDoubleClick);
      // 
      // imageList
      // 
      this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      this.imageList.ImageSize = new System.Drawing.Size(16, 16);
      this.imageList.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // detailsGroupBox
      // 
      this.detailsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.detailsGroupBox.Controls.Add(this.detailsViewHost);
      this.detailsGroupBox.Location = new System.Drawing.Point(3, 34);
      this.detailsGroupBox.Name = "detailsGroupBox";
      this.detailsGroupBox.Size = new System.Drawing.Size(413, 405);
      this.detailsGroupBox.TabIndex = 1;
      this.detailsGroupBox.TabStop = false;
      this.detailsGroupBox.Text = "Details";
      // 
      // detailsViewHost
      // 
      this.detailsViewHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.detailsViewHost.Caption = "View";
      this.detailsViewHost.Content = null;
      this.detailsViewHost.Enabled = false;
      this.detailsViewHost.Location = new System.Drawing.Point(6, 19);
      this.detailsViewHost.Name = "detailsViewHost";
      this.detailsViewHost.ReadOnly = false;
      this.detailsViewHost.Size = new System.Drawing.Size(401, 380);
      this.detailsViewHost.TabIndex = 0;
      this.detailsViewHost.ViewsLabelVisible = true;
      this.detailsViewHost.ViewType = null;
      // 
      // ExperimentTreeView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.optimizersGroupBox);
      this.Name = "ExperimentTreeView";
      this.Size = new System.Drawing.Size(627, 458);
      this.optimizersGroupBox.ResumeLayout(false);
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
      this.splitContainer.ResumeLayout(false);
      this.detailsGroupBox.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion

    private System.Windows.Forms.GroupBox optimizersGroupBox;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.TreeView optimizerTreeView;
    private System.Windows.Forms.GroupBox detailsGroupBox;
    private MainForm.WindowsForms.ViewHost detailsViewHost;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.CheckBox showDetailsCheckBox;
    private System.Windows.Forms.Button removeButton;
    private System.Windows.Forms.Button moveUpButton;
    private System.Windows.Forms.Button moveDownButton;
    private System.Windows.Forms.Button addButton;
    private System.Windows.Forms.ImageList imageList;

  }
}
