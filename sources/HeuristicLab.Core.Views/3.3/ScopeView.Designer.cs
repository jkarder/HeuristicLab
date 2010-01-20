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
  partial class ScopeView {
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
        if (scopesTreeView.Nodes.Count > 0) {
          RemoveTreeNode(scopesTreeView.Nodes[0]);
          scopesTreeView.Nodes.Clear();
        }
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
      this.scopesTreeView = new System.Windows.Forms.TreeView();
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.scopesGroupBox = new System.Windows.Forms.GroupBox();
      this.variableCollectionView = new HeuristicLab.Core.Views.VariableCollectionView();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.scopesGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // scopesTreeView
      // 
      this.scopesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.scopesTreeView.HideSelection = false;
      this.scopesTreeView.Location = new System.Drawing.Point(3, 16);
      this.scopesTreeView.Name = "scopesTreeView";
      this.scopesTreeView.ShowNodeToolTips = true;
      this.scopesTreeView.Size = new System.Drawing.Size(394, 181);
      this.scopesTreeView.TabIndex = 0;
      this.scopesTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.scopesTreeView_AfterCollapse);
      this.scopesTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.scopesTreeView_BeforeExpand);
      this.scopesTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scopesTreeView_MouseDown);
      this.scopesTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.scopesTreeView_ItemDrag);
      // 
      // splitContainer
      // 
      this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer.Location = new System.Drawing.Point(0, 0);
      this.splitContainer.Name = "splitContainer";
      this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer.Panel1
      // 
      this.splitContainer.Panel1.Controls.Add(this.scopesGroupBox);
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.variableCollectionView);
      this.splitContainer.Size = new System.Drawing.Size(400, 400);
      this.splitContainer.SplitterDistance = 200;
      this.splitContainer.TabIndex = 1;
      // 
      // scopesGroupBox
      // 
      this.scopesGroupBox.Controls.Add(this.scopesTreeView);
      this.scopesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.scopesGroupBox.Location = new System.Drawing.Point(0, 0);
      this.scopesGroupBox.Name = "scopesGroupBox";
      this.scopesGroupBox.Size = new System.Drawing.Size(400, 200);
      this.scopesGroupBox.TabIndex = 0;
      this.scopesGroupBox.TabStop = false;
      this.scopesGroupBox.Text = "&Scopes";
      // 
      // variableCollectionView
      // 
      this.variableCollectionView.Caption = "VariableCollection";
      this.variableCollectionView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.variableCollectionView.ItemCollection = null;
      this.variableCollectionView.Location = new System.Drawing.Point(0, 0);
      this.variableCollectionView.Name = "variableCollectionView";
      this.variableCollectionView.NamedItemCollection = null;
      this.variableCollectionView.Size = new System.Drawing.Size(400, 196);
      this.variableCollectionView.TabIndex = 0;
      // 
      // ScopeView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer);
      this.Name = "ScopeView";
      this.Size = new System.Drawing.Size(400, 400);
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.ResumeLayout(false);
      this.scopesGroupBox.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView scopesTreeView;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.GroupBox scopesGroupBox;
    private VariableCollectionView variableCollectionView;
  }
}
