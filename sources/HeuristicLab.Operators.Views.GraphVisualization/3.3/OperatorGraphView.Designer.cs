﻿namespace HeuristicLab.Operators.Views.GraphVisualization {
  partial class OperatorGraphView {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.graphVisualizationInfoView = new HeuristicLab.Operators.Views.GraphVisualization.GraphVisualizationInfoView();
      this.shapeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.openViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.initialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.breakPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.splitContainer = new System.Windows.Forms.SplitContainer();
      this.propertyGroupBox = new System.Windows.Forms.GroupBox();
      this.propertyViewHost = new HeuristicLab.Core.Views.ViewHost();
      this.shapeContextMenu.SuspendLayout();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.propertyGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // graphVisualizationInfoView
      // 
      this.graphVisualizationInfoView.Caption = null;
      this.graphVisualizationInfoView.Content = null;
      this.graphVisualizationInfoView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.graphVisualizationInfoView.Location = new System.Drawing.Point(0, 0);
      this.graphVisualizationInfoView.Name = "graphVisualizationInfoView";
      this.graphVisualizationInfoView.Size = new System.Drawing.Size(665, 279);
      this.graphVisualizationInfoView.TabIndex = 0;
      // 
      // shapeContextMenu
      // 
      this.shapeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openViewToolStripMenuItem,
            this.initialToolStripMenuItem,
            this.breakPointToolStripMenuItem});
      this.shapeContextMenu.Name = "shapeContextMenu";
      this.shapeContextMenu.Size = new System.Drawing.Size(154, 70);
      this.shapeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.shapeContextMenu_Opening);
      // 
      // openViewToolStripMenuItem
      // 
      this.openViewToolStripMenuItem.Name = "openViewToolStripMenuItem";
      this.openViewToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
      this.openViewToolStripMenuItem.Text = "Open View";
      this.openViewToolStripMenuItem.Click += new System.EventHandler(this.openViewToolStripMenuItem_Click);
      // 
      // initialToolStripMenuItem
      // 
      this.initialToolStripMenuItem.Name = "initialToolStripMenuItem";
      this.initialToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
      this.initialToolStripMenuItem.Text = "Initial Operator";
      this.initialToolStripMenuItem.Click += new System.EventHandler(this.initialOperatorToolStripMenuItem_Click);
      // 
      // breakPointToolStripMenuItem
      // 
      this.breakPointToolStripMenuItem.Name = "breakPointToolStripMenuItem";
      this.breakPointToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
      this.breakPointToolStripMenuItem.Text = "Break Point";
      this.breakPointToolStripMenuItem.Click += new System.EventHandler(this.breakPointToolStripMenuItem_Click);
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
      this.splitContainer.Panel1.Controls.Add(this.graphVisualizationInfoView);
      // 
      // splitContainer.Panel2
      // 
      this.splitContainer.Panel2.Controls.Add(this.propertyGroupBox);
      this.splitContainer.Size = new System.Drawing.Size(665, 444);
      this.splitContainer.SplitterDistance = 279;
      this.splitContainer.TabIndex = 1;
      // 
      // propertyGroupBox
      // 
      this.propertyGroupBox.Controls.Add(this.propertyViewHost);
      this.propertyGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.propertyGroupBox.Location = new System.Drawing.Point(0, 0);
      this.propertyGroupBox.Name = "propertyGroupBox";
      this.propertyGroupBox.Size = new System.Drawing.Size(665, 161);
      this.propertyGroupBox.TabIndex = 0;
      this.propertyGroupBox.TabStop = false;
      this.propertyGroupBox.Text = "Properties";
      // 
      // propertyViewHost
      // 
      this.propertyViewHost.Content = null;
      this.propertyViewHost.Dock = System.Windows.Forms.DockStyle.Fill;
      this.propertyViewHost.Location = new System.Drawing.Point(3, 16);
      this.propertyViewHost.Name = "propertyViewHost";
      this.propertyViewHost.Size = new System.Drawing.Size(659, 142);
      this.propertyViewHost.TabIndex = 0;
      this.propertyViewHost.ViewType = null;
      // 
      // OperatorGraphView
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer);
      this.Name = "OperatorGraphView";
      this.Size = new System.Drawing.Size(665, 444);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OperatorGraphView_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OperatorGraphView_DragEnter);
      this.shapeContextMenu.ResumeLayout(false);
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.ResumeLayout(false);
      this.propertyGroupBox.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion

    private GraphVisualizationInfoView graphVisualizationInfoView;
    private System.Windows.Forms.ContextMenuStrip shapeContextMenu;
    private System.Windows.Forms.ToolStripMenuItem openViewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem initialToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem breakPointToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer;
    private System.Windows.Forms.GroupBox propertyGroupBox;
    private HeuristicLab.Core.Views.ViewHost propertyViewHost;
  }
}
