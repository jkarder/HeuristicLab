﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HeuristicLab.Common;
using HeuristicLab.Data;
using HeuristicLab.MainForm;

namespace HeuristicLab.Problems.DataAnalysis.Views {
  [View("Variable Impacts")]
  [Content(typeof(IRegressionSolution))]
  public partial class RegressionSolutionVariableImpactsView : DataAnalysisSolutionEvaluationView {
    private CancellationTokenSource cancellationToken = new CancellationTokenSource();
    private enum SortingCriteria {
      ImpactValue,
      Occurrence,
      VariableName
    }
    private List<Tuple<string, double>> rawVariableImpacts = new List<Tuple<string, double>>();

    public new IRegressionSolution Content {
      get { return (IRegressionSolution)base.Content; }
      set {
        base.Content = value;
      }
    }

    public RegressionSolutionVariableImpactsView()
      : base() {
      InitializeComponent();

      //Set the default values
      this.dataPartitionComboBox.SelectedIndex = 0;
      this.replacementComboBox.SelectedIndex = 0;
      this.factorVarReplComboBox.SelectedIndex = 0;
      this.sortByComboBox.SelectedItem = SortingCriteria.ImpactValue;
    }

    protected override void RegisterContentEvents() {
      base.RegisterContentEvents();
      Content.ModelChanged += new EventHandler(Content_ModelChanged);
      Content.ProblemDataChanged += new EventHandler(Content_ProblemDataChanged);
    }

    protected override void DeregisterContentEvents() {
      base.DeregisterContentEvents();
      Content.ModelChanged -= new EventHandler(Content_ModelChanged);
      Content.ProblemDataChanged -= new EventHandler(Content_ProblemDataChanged);
    }

    protected virtual void Content_ProblemDataChanged(object sender, EventArgs e) {
      OnContentChanged();
    }

    protected virtual void Content_ModelChanged(object sender, EventArgs e) {
      OnContentChanged();
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content == null) {
        variableImactsArrayView.Content = null;
      } else {
        UpdateVariableImpact();
      }
    }

    private void RegressionSolutionVariableImpactsView_VisibleChanged(object sender, EventArgs e) {
      cancellationToken.Cancel();
    }


    private void dataPartitionComboBox_SelectedIndexChanged(object sender, EventArgs e) {
      UpdateVariableImpact();
    }

    private void replacementComboBox_SelectedIndexChanged(object sender, EventArgs e) {
      UpdateVariableImpact();
    }

    private void sortByComboBox_SelectedIndexChanged(object sender, EventArgs e) {
      //Update the default ordering (asc,desc), but remove the eventHandler beforehand (otherwise the data would be ordered twice)
      ascendingCheckBox.CheckedChanged -= ascendingCheckBox_CheckedChanged;
      ascendingCheckBox.Checked = (SortingCriteria)sortByComboBox.SelectedItem != SortingCriteria.ImpactValue;
      ascendingCheckBox.CheckedChanged += ascendingCheckBox_CheckedChanged;

      UpdateOrdering();
    }

    private void ascendingCheckBox_CheckedChanged(object sender, EventArgs e) {
      UpdateOrdering();
    }


    private async void UpdateVariableImpact() {
      IProgress progress;

      //Check if the selection is valid
      if (Content == null) { return; }
      if (replacementComboBox.SelectedIndex < 0) { return; }
      if (dataPartitionComboBox.SelectedIndex < 0) { return; }
      if (factorVarReplComboBox.SelectedIndex < 0) { return; }

      //Prepare arguments
      var mainForm = (MainForm.WindowsForms.MainForm)MainFormManager.MainForm;
      var replMethod = (RegressionSolutionVariableImpactsCalculator.ReplacementMethodEnum)replacementComboBox.Items[replacementComboBox.SelectedIndex];
      var factorReplMethod = (RegressionSolutionVariableImpactsCalculator.FactorReplacementMethodEnum)factorVarReplComboBox.Items[factorVarReplComboBox.SelectedIndex];
      var dataPartition = (RegressionSolutionVariableImpactsCalculator.DataPartitionEnum)dataPartitionComboBox.SelectedItem;

      variableImactsArrayView.Caption = Content.Name + " Variable Impacts";
      progress = mainForm.AddOperationProgressToView(this, "Calculating variable impacts for " + Content.Name);
      progress.ProgressValue = 0;

      cancellationToken = new CancellationTokenSource();
      //Remember the original ordering of the variables
      try {
        var impacts = await Task.Run(() => RegressionSolutionVariableImpactsCalculator.CalculateImpacts(Content, dataPartition, replMethod, factorReplMethod,
          (i, s) => {
            progress.ProgressValue = i;
            progress.Status = s;
            return cancellationToken.Token.IsCancellationRequested;
          }), cancellationToken.Token);

        if (cancellationToken.Token.IsCancellationRequested) { return; }
        var problemData = Content.ProblemData;
        var inputvariables = new HashSet<string>(problemData.AllowedInputVariables.Union(Content.Model.VariablesUsedForPrediction));
        var originalVariableOrdering = problemData.Dataset.VariableNames.Where(v => inputvariables.Contains(v)).Where(problemData.Dataset.VariableHasType<double>).ToList();

        rawVariableImpacts.Clear();
        originalVariableOrdering.ForEach(v => rawVariableImpacts.Add(new Tuple<string, double>(v, impacts.First(vv => vv.Item1 == v).Item2)));
        UpdateOrdering();
      } finally {
        ((MainForm.WindowsForms.MainForm)MainFormManager.MainForm).RemoveOperationProgressFromView(this);
      }
    }

    /// <summary>
    /// Updates the <see cref="variableImactsArrayView"/> according to the selected ordering <see cref="ascendingCheckBox"/> of the selected Column <see cref="sortByComboBox"/>
    /// The default is "Descending" by "VariableImpact" (as in previous versions)
    /// </summary>
    private void UpdateOrdering() {
      //Check if valid sortingCriteria is selected and data exists
      if (sortByComboBox.SelectedIndex == -1) { return; }
      if (rawVariableImpacts == null) { return; }
      if (!rawVariableImpacts.Any()) { return; }

      var selectedItem = (SortingCriteria)sortByComboBox.SelectedItem;
      bool ascending = ascendingCheckBox.Checked;

      IEnumerable<Tuple<string, double>> orderedEntries = null;

      //Sort accordingly
      switch (selectedItem) {
        case SortingCriteria.ImpactValue:
          orderedEntries = rawVariableImpacts.OrderBy(v => v.Item2);
          break;
        case SortingCriteria.Occurrence:
          orderedEntries = rawVariableImpacts;
          break;
        case SortingCriteria.VariableName:
          orderedEntries = rawVariableImpacts.OrderBy(v => v.Item1, new NaturalStringComparer());
          break;
        default:
          throw new NotImplementedException("Ordering for selected SortingCriteria not implemented");
      }

      if (!ascending) { orderedEntries = orderedEntries.Reverse(); }

      //Write the data back
      var impactArray = new DoubleArray(orderedEntries.Select(i => i.Item2).ToArray()) {
        ElementNames = orderedEntries.Select(i => i.Item1)
      };

      //Could be, if the View was closed
      if (!variableImactsArrayView.IsDisposed) {
        variableImactsArrayView.Content = (DoubleArray)impactArray.AsReadOnly();
      }
    }
  }
}
