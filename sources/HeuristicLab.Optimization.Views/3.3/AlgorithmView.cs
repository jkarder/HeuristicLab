#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;
using HeuristicLab.Persistence.Default.Xml;

namespace HeuristicLab.Optimization.Views {
  /// <summary>
  /// The base class for visual representations of items.
  /// </summary>
  [View("Algorithm View")]
  [Content(typeof(Algorithm), true)]
  [Content(typeof(IAlgorithm), false)]
  public partial class AlgorithmView : NamedItemView {
    private TypeSelectorDialog problemTypeSelectorDialog;

    public new IAlgorithm Content {
      get { return (IAlgorithm)base.Content; }
      set { base.Content = value; }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ItemBaseView"/>.
    /// </summary>
    public AlgorithmView() {
      InitializeComponent();
    }
    /// <summary>
    /// Intializes a new instance of <see cref="ItemBaseView"/> with the given <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item that should be displayed.</param>
    public AlgorithmView(IAlgorithm content)
      : this() {
      Content = content;
    }

    protected override void OnInitialized(EventArgs e) {
      // Set order of tab pages according to z order.
      // NOTE: This is required due to a bug in the VS designer.
      List<Control> tabPages = new List<Control>();
      for (int i = 0; i < tabControl.Controls.Count; i++) {
        tabPages.Add(tabControl.Controls[i]);
      }
      tabControl.Controls.Clear();
      foreach (Control control in tabPages)
        tabControl.Controls.Add(control);

      base.OnInitialized(e);
    }

    protected override void DeregisterContentEvents() {
      Content.ExceptionOccurred -= new EventHandler<EventArgs<Exception>>(Content_ExceptionOccurred);
      Content.ExecutionStateChanged -= new EventHandler(Content_ExecutionStateChanged);
      Content.ExecutionTimeChanged -= new EventHandler(Content_ExecutionTimeChanged);
      Content.Prepared -= new EventHandler(Content_Prepared);
      Content.ProblemChanged -= new EventHandler(Content_ProblemChanged);
      base.DeregisterContentEvents();
    }
    protected override void RegisterContentEvents() {
      base.RegisterContentEvents();
      Content.ExceptionOccurred += new EventHandler<EventArgs<Exception>>(Content_ExceptionOccurred);
      Content.ExecutionStateChanged += new EventHandler(Content_ExecutionStateChanged);
      Content.ExecutionTimeChanged += new EventHandler(Content_ExecutionTimeChanged);
      Content.Prepared += new EventHandler(Content_Prepared);
      Content.ProblemChanged += new EventHandler(Content_ProblemChanged);
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content == null) {
        parameterCollectionView.Content = null;
        problemViewHost.Content = null;
        resultsView.Content = null;
        tabControl.Enabled = false;
        startButton.Enabled = pauseButton.Enabled = stopButton.Enabled = resetButton.Enabled = false;
        executionTimeTextBox.Text = "-";
        executionTimeTextBox.Enabled = false;
      } else {
        parameterCollectionView.Content = Content.Parameters;
        saveProblemButton.Enabled = Content.Problem != null;
        problemViewHost.ViewType = null;
        problemViewHost.Content = Content.Problem;
        resultsView.Content = Content.Results.AsReadOnly();
        tabControl.Enabled = true;
        EnableDisableButtons();
        executionTimeTextBox.Text = Content.ExecutionTime.ToString();
        executionTimeTextBox.Enabled = true;
      }
    }

    protected override void OnClosed(FormClosedEventArgs e) {
      if ((Content != null) && (Content.ExecutionState == ExecutionState.Started)) Content.Stop();
      base.OnClosed(e);
    }

    #region Content Events
    protected virtual void Content_ProblemChanged(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Content_ProblemChanged), sender, e);
      else {
        problemViewHost.ViewType = null;
        problemViewHost.Content = Content.Problem;
        saveProblemButton.Enabled = Content.Problem != null;
      }
    }
    protected virtual void Content_Prepared(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Content_Prepared), sender, e);
      else
        resultsView.Content = Content.Results.AsReadOnly();
    }
    protected virtual void Content_ExecutionStateChanged(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Content_ExecutionStateChanged), sender, e);
      else {
        nameTextBox.Enabled = Content.ExecutionState != ExecutionState.Started;
        descriptionTextBox.Enabled = Content.ExecutionState != ExecutionState.Started;
        SaveEnabled = Content.ExecutionState != ExecutionState.Started;
        parameterCollectionView.Enabled = Content.ExecutionState != ExecutionState.Started;
        newProblemButton.Enabled = openProblemButton.Enabled = saveProblemButton.Enabled = Content.ExecutionState != ExecutionState.Started;
        problemViewHost.Enabled = Content.ExecutionState != ExecutionState.Started;
        resultsView.Enabled = Content.ExecutionState != ExecutionState.Started;
        EnableDisableButtons();
      }
    }
    protected virtual void Content_ExecutionTimeChanged(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Content_ExecutionTimeChanged), sender, e);
      else
        executionTimeTextBox.Text = Content.ExecutionTime.ToString();
    }
    protected virtual void Content_ExceptionOccurred(object sender, EventArgs<Exception> e) {
      if (InvokeRequired)
        Invoke(new EventHandler<EventArgs<Exception>>(Content_ExceptionOccurred), sender, e);
      else
        Auxiliary.ShowErrorMessageBox(e.Value);
    }
    #endregion

    #region Button events
    protected virtual void newProblemButton_Click(object sender, EventArgs e) {
      if (problemTypeSelectorDialog == null) {
        problemTypeSelectorDialog = new TypeSelectorDialog();
        problemTypeSelectorDialog.Caption = "Select Problem";
        problemTypeSelectorDialog.TypeSelector.Configure(Content.ProblemType, false, false);
      }
      if (problemTypeSelectorDialog.ShowDialog(this) == DialogResult.OK) {
        Content.Problem = (IProblem)problemTypeSelectorDialog.TypeSelector.CreateInstanceOfSelectedType();
      }
    }
    protected virtual void openProblemButton_Click(object sender, EventArgs e) {
      openFileDialog.Title = "Open Problem";
      if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
        this.Cursor = Cursors.AppStarting;
        newProblemButton.Enabled = openProblemButton.Enabled = saveProblemButton.Enabled = false;
        problemViewHost.Enabled = false;

        var call = new Func<string, object>(XmlParser.Deserialize);
        call.BeginInvoke(openFileDialog.FileName, delegate(IAsyncResult a) {
          IProblem problem = null;
          try {
            problem = call.EndInvoke(a) as IProblem;
          } catch (Exception ex) {
            Auxiliary.ShowErrorMessageBox(ex);
          }
          Invoke(new Action(delegate() {
            if (problem == null)
              MessageBox.Show(this, "The selected file does not contain a problem.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (!Content.ProblemType.IsInstanceOfType(problem))
              MessageBox.Show(this, "The selected file contains a problem type which is not supported by this algorithm.", "Invalid Problem Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
              Content.Problem = problem;
            problemViewHost.Enabled = true;
            newProblemButton.Enabled = openProblemButton.Enabled = saveProblemButton.Enabled = true;
            this.Cursor = Cursors.Default;
          }));
        }, null);
      }
    }
    protected virtual void saveProblemButton_Click(object sender, EventArgs e) {
      saveFileDialog.Title = "Save Problem";
      if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
        this.Cursor = Cursors.AppStarting;
        newProblemButton.Enabled = openProblemButton.Enabled = saveProblemButton.Enabled = false;
        problemViewHost.Enabled = false;

        var call = new Action<IProblem, string, int>(XmlGenerator.Serialize);
        int compression = 9;
        if (saveFileDialog.FilterIndex == 1) compression = 0;
        call.BeginInvoke(Content.Problem, saveFileDialog.FileName, compression, delegate(IAsyncResult a) {
          try {
            call.EndInvoke(a);
          }
          catch (Exception ex) {
            Auxiliary.ShowErrorMessageBox(ex);
          }
          Invoke(new Action(delegate() {
            problemViewHost.Enabled = true;
            newProblemButton.Enabled = openProblemButton.Enabled = saveProblemButton.Enabled = true;
            this.Cursor = Cursors.Default;
          }));
        }, null);
      }
    }
    protected virtual void startButton_Click(object sender, EventArgs e) {
      Content.Start();
    }
    protected virtual void pauseButton_Click(object sender, EventArgs e) {
      Content.Pause();
    }
    protected virtual void stopButton_Click(object sender, EventArgs e) {
      Content.Stop();
    }
    protected virtual void resetButton_Click(object sender, EventArgs e) {
      Content.Prepare();
    }
    #endregion

    #region Helpers
    private void EnableDisableButtons() {
      startButton.Enabled = (Content.ExecutionState == ExecutionState.Prepared) || (Content.ExecutionState == ExecutionState.Paused);
      pauseButton.Enabled = Content.ExecutionState == ExecutionState.Started;
      stopButton.Enabled = (Content.ExecutionState == ExecutionState.Started) || (Content.ExecutionState == ExecutionState.Paused);
      resetButton.Enabled = Content.ExecutionState != ExecutionState.Started;
    }
    #endregion
  }
}
