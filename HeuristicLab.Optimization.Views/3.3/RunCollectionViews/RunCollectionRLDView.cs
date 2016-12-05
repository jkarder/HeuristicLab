﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2016 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HeuristicLab.Analysis;
using HeuristicLab.Collections;
using HeuristicLab.Core.Views;
using HeuristicLab.Data;
using HeuristicLab.MainForm;
using HeuristicLab.MainForm.WindowsForms;

namespace HeuristicLab.Optimization.Views {
  [View("Run-length Distribution View")]
  [Content(typeof(RunCollection), false)]
  public partial class RunCollectionRLDView : ItemView {
    private List<Series> invisibleTargetSeries;

    private const string AllRuns = "All Runs";

    private static readonly Color[] colors = new[] {
      Color.FromArgb(0x40, 0x6A, 0xB7),
      Color.FromArgb(0xB1, 0x6D, 0x01),
      Color.FromArgb(0x4E, 0x8A, 0x06),
      Color.FromArgb(0x75, 0x50, 0x7B),
      Color.FromArgb(0x72, 0x9F, 0xCF),
      Color.FromArgb(0xA4, 0x00, 0x00),
      Color.FromArgb(0xAD, 0x7F, 0xA8),
      Color.FromArgb(0x29, 0x50, 0xCF),
      Color.FromArgb(0x90, 0xB0, 0x60),
      Color.FromArgb(0xF5, 0x89, 0x30),
      Color.FromArgb(0x55, 0x57, 0x53),
      Color.FromArgb(0xEF, 0x59, 0x59),
      Color.FromArgb(0xED, 0xD4, 0x30),
      Color.FromArgb(0x63, 0xC2, 0x16),
    };
    private static readonly ChartDashStyle[] lineStyles = new[] {
      ChartDashStyle.Solid,
      ChartDashStyle.Dash,
      ChartDashStyle.DashDot,
      ChartDashStyle.Dot
    };
    private static readonly DataRowVisualProperties.DataRowLineStyle[] hlLineStyles = new[] {
      DataRowVisualProperties.DataRowLineStyle.Solid,
      DataRowVisualProperties.DataRowLineStyle.Dash,
      DataRowVisualProperties.DataRowLineStyle.DashDot,
      DataRowVisualProperties.DataRowLineStyle.Dot
    };

    public new RunCollection Content {
      get { return (RunCollection)base.Content; }
      set { base.Content = value; }
    }

    private double[] targets;
    private double[] budgets;

    private bool suppressUpdates;
    private readonly IndexedDataTable<double> byCostDataTable;
    public IndexedDataTable<double> ByCostDataTable {
      get { return byCostDataTable; }
    }

    public RunCollectionRLDView() {
      InitializeComponent();
      invisibleTargetSeries = new List<Series>();

      targetChart.CustomizeAllChartAreas();
      targetChart.ChartAreas[0].CursorX.Interval = 1;
      targetChart.SuppressExceptions = true;
      byCostDataTable = new IndexedDataTable<double>("ECDF by Cost", "A data table containing the ECDF of each of a number of groups.") {
        VisualProperties = {
          YAxisTitle = "Proportion of unused budgets",
          YAxisMinimumFixedValue = 0,
          YAxisMinimumAuto = false,
          YAxisMaximumFixedValue = 1,
          YAxisMaximumAuto = false
        }
      };
      byCostViewHost.Content = byCostDataTable;
      suppressUpdates = false;
    }

    #region Content events
    protected override void RegisterContentEvents() {
      base.RegisterContentEvents();
      Content.ItemsAdded += Content_ItemsAdded;
      Content.ItemsRemoved += Content_ItemsRemoved;
      Content.CollectionReset += Content_CollectionReset;
      Content.UpdateOfRunsInProgressChanged += Content_UpdateOfRunsInProgressChanged;
      Content.OptimizerNameChanged += Content_AlgorithmNameChanged;
    }
    protected override void DeregisterContentEvents() {
      Content.ItemsAdded -= Content_ItemsAdded;
      Content.ItemsRemoved -= Content_ItemsRemoved;
      Content.CollectionReset -= Content_CollectionReset;
      Content.UpdateOfRunsInProgressChanged -= Content_UpdateOfRunsInProgressChanged;
      Content.OptimizerNameChanged -= Content_AlgorithmNameChanged;
      base.DeregisterContentEvents();
    }

    private void Content_ItemsAdded(object sender, CollectionItemsChangedEventArgs<IRun> e) {
      if (suppressUpdates) return;
      if (InvokeRequired) {
        Invoke(new CollectionItemsChangedEventHandler<IRun>(Content_ItemsAdded), sender, e);
        return;
      }
      UpdateGroupAndProblemComboBox();
      UpdateDataTableComboBox();
      foreach (var run in e.Items)
        RegisterRunEvents(run);
    }
    private void Content_ItemsRemoved(object sender, CollectionItemsChangedEventArgs<IRun> e) {
      if (suppressUpdates) return;
      if (InvokeRequired) {
        Invoke(new CollectionItemsChangedEventHandler<IRun>(Content_ItemsRemoved), sender, e);
        return;
      }
      UpdateGroupAndProblemComboBox();
      UpdateDataTableComboBox();
      foreach (var run in e.Items)
        DeregisterRunEvents(run);
    }
    private void Content_CollectionReset(object sender, CollectionItemsChangedEventArgs<IRun> e) {
      if (suppressUpdates) return;
      if (InvokeRequired) {
        Invoke(new CollectionItemsChangedEventHandler<IRun>(Content_CollectionReset), sender, e);
        return;
      }
      UpdateGroupAndProblemComboBox();
      UpdateDataTableComboBox();
      foreach (var run in e.OldItems)
        DeregisterRunEvents(run);
    }
    private void Content_AlgorithmNameChanged(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Content_AlgorithmNameChanged), sender, e);
      else UpdateCaption();
    }
    private void Content_UpdateOfRunsInProgressChanged(object sender, EventArgs e) {
      if (InvokeRequired) {
        Invoke(new EventHandler(Content_UpdateOfRunsInProgressChanged), sender, e);
        return;
      }
      suppressUpdates = Content.UpdateOfRunsInProgress;
      if (!suppressUpdates) {
        UpdateDataTableComboBox();
        UpdateGroupAndProblemComboBox();
        UpdateRuns();
      }
    }

    private void RegisterRunEvents(IRun run) {
      run.PropertyChanged += run_PropertyChanged;
    }
    private void DeregisterRunEvents(IRun run) {
      run.PropertyChanged -= run_PropertyChanged;
    }
    private void run_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      if (suppressUpdates) return;
      if (InvokeRequired) {
        Invoke((Action<object, PropertyChangedEventArgs>)run_PropertyChanged, sender, e);
      } else {
        if (e.PropertyName == "Visible")
          UpdateRuns();
      }
    }
    #endregion

    protected override void OnContentChanged() {
      base.OnContentChanged();
      dataTableComboBox.Items.Clear();
      groupComboBox.Items.Clear();
      targetChart.ChartAreas[0].AxisX.IsLogarithmic = false;
      targetChart.Series.Clear();
      invisibleTargetSeries.Clear();
      byCostDataTable.VisualProperties.XAxisLogScale = false;
      byCostDataTable.Rows.Clear();

      UpdateCaption();
      if (Content != null) {
        UpdateGroupAndProblemComboBox();
        UpdateDataTableComboBox();
      }
    }


    private void UpdateGroupAndProblemComboBox() {
      var selectedGroupItem = (string)groupComboBox.SelectedItem;

      var groupings = Content.ParameterNames.OrderBy(x => x).ToArray();
      groupComboBox.Items.Clear();
      groupComboBox.Items.Add(AllRuns);
      groupComboBox.Items.AddRange(groupings);
      if (selectedGroupItem != null && groupComboBox.Items.Contains(selectedGroupItem)) {
        groupComboBox.SelectedItem = selectedGroupItem;
      } else if (groupComboBox.Items.Count > 0) {
        groupComboBox.SelectedItem = groupComboBox.Items[0];
      }

      var problems = new HashSet<ProblemDescription>();
      foreach (var run in Content) {
        problems.Add(new ProblemDescription(run));
      }

      var problemTypesDifferent = problems.Select(x => x.ProblemType).Where(x => !string.IsNullOrEmpty(x)).Distinct().Count() > 1;
      var problemNamesDifferent = problems.Select(x => x.ProblemName).Where(x => !string.IsNullOrEmpty(x)).Distinct().Count() > 1;
      var evaluatorDifferent = problems.Select(x => x.Evaluator).Where(x => !string.IsNullOrEmpty(x)).Distinct().Count() > 1;
      var maximizationDifferent = problems.Select(x => x.Maximization).Distinct().Count() > 1;
      var allEqual = !problemTypesDifferent && !problemNamesDifferent && !evaluatorDifferent && !maximizationDifferent;

      var selectedProblemItem = (ProblemDescription)problemComboBox.SelectedItem;
      problemComboBox.Items.Clear();
      problemComboBox.Items.Add(ProblemDescription.MatchAll);
      if (selectedProblemItem == null || selectedProblemItem == ProblemDescription.MatchAll)
        problemComboBox.SelectedIndex = 0;
      foreach (var prob in problems.OrderBy(x => x.ToString()).ToList()) {
        prob.DisplayProblemType = problemTypesDifferent;
        prob.DisplayProblemName = problemNamesDifferent || allEqual;
        prob.DisplayEvaluator = evaluatorDifferent;
        prob.DisplayMaximization = maximizationDifferent;
        problemComboBox.Items.Add(prob);
        if (prob.Equals(selectedProblemItem)) problemComboBox.SelectedItem = prob;
      }
      SetEnabledStateOfControls();
    }

    private void UpdateDataTableComboBox() {
      string selectedItem = (string)dataTableComboBox.SelectedItem;

      dataTableComboBox.Items.Clear();
      var dataTables = (from run in Content
                        from result in run.Results
                        where result.Value is IndexedDataTable<double>
                        select result.Key).Distinct().ToArray();

      dataTableComboBox.Items.AddRange(dataTables);
      if (selectedItem != null && dataTableComboBox.Items.Contains(selectedItem)) {
        dataTableComboBox.SelectedItem = selectedItem;
      } else if (dataTableComboBox.Items.Count > 0) {
        dataTableComboBox.SelectedItem = dataTableComboBox.Items[0];
      }
    }

    protected override void SetEnabledStateOfControls() {
      base.SetEnabledStateOfControls();
      groupComboBox.Enabled = Content != null;
      problemComboBox.Enabled = Content != null && problemComboBox.Items.Count > 1;
      dataTableComboBox.Enabled = Content != null && dataTableComboBox.Items.Count > 1;
      addTargetsAsResultButton.Enabled = Content != null && targets != null && dataTableComboBox.SelectedIndex >= 0;
      addBudgetsAsResultButton.Enabled = Content != null && budgets != null && dataTableComboBox.SelectedIndex >= 0;
    }

    private Dictionary<string, Dictionary<ProblemDescription, Tuple<double, List<IRun>>>> GroupRuns() {
      var groupedRuns = new Dictionary<string, Dictionary<ProblemDescription, Tuple<double, List<IRun>>>>();

      var table = (string)dataTableComboBox.SelectedItem;
      if (string.IsNullOrEmpty(table)) return groupedRuns;

      var selectedGroup = (string)groupComboBox.SelectedItem;
      if (string.IsNullOrEmpty(selectedGroup)) return groupedRuns;

      var selectedProblem = (ProblemDescription)problemComboBox.SelectedItem;
      if (selectedProblem == null) return groupedRuns;

      var targetsPerProblem = CalculateBestTargetPerProblemInstance(table);

      foreach (var x in (from r in Content
                         where (selectedGroup == AllRuns || r.Parameters.ContainsKey(selectedGroup))
                           && selectedProblem.Match(r)
                           && r.Results.ContainsKey(table)
                           && r.Visible
                         group r by selectedGroup == AllRuns ? AllRuns : r.Parameters[selectedGroup].ToString() into g
                         select Tuple.Create(g.Key, g.ToList()))) {
        var pDict = new Dictionary<ProblemDescription, Tuple<double, List<IRun>>>();
        foreach (var y in (from r in x.Item2
                           let pd = new ProblemDescription(r)
                           group r by pd into g
                           select Tuple.Create(g.Key, g.ToList()))) {
          pDict[y.Item1] = Tuple.Create(targetsPerProblem[y.Item1], y.Item2);
        }
        groupedRuns[x.Item1] = pDict;
      }

      return groupedRuns;
    }

    #region Performance analysis by (multiple) target(s)
    private void UpdateResultsByTarget() {
      // necessary to reset log scale -> empty chart cannot use log scaling
      targetChart.ChartAreas[0].AxisX.IsLogarithmic = false;
      targetChart.Series.Clear();
      invisibleTargetSeries.Clear();

      var table = (string)dataTableComboBox.SelectedItem;
      if (string.IsNullOrEmpty(table)) return;

      if (targets == null) GenerateDefaultTargets();

      var groupedRuns = GroupRuns();
      if (groupedRuns.Count == 0) return;

      var xAxisTitles = new HashSet<string>();
      var colorCount = 0;
      var lineStyleCount = 0;

      // if the group contains multiple different problem instances we want to use the
      // minimal maximal observed effort otherwise we run into situations where we don't
      // have data for a certain problem instance anymore this is a special case when
      // aggregating over multiple problem instances      
      var maxEfforts = new Dictionary<ProblemDescription, double>();
      double minEff = double.MaxValue, maxEff = double.MinValue;
      foreach (var group in groupedRuns) {
        foreach (var problem in group.Value) {
          double problemSpecificMaxEff;
          if (!maxEfforts.TryGetValue(problem.Key, out problemSpecificMaxEff)) {
            problemSpecificMaxEff = 0;
          }
          var bestKnownTarget = problem.Value.Item1;
          var max = problem.Key.IsMaximization();
          var worstTarget = (max ? (1 - targets.Max()) : (1 + targets.Max())) * bestKnownTarget;
          var bestTarget = (max ? (1 - targets.Min()) : (1 + targets.Min())) * bestKnownTarget;
          foreach (var run in problem.Value.Item2) {
            var row = ((IndexedDataTable<double>)run.Results[table]).Rows.First().Values;
            var a = row.FirstOrDefault(x => max ? x.Item2 >= worstTarget : x.Item2 <= worstTarget);
            var b = row.FirstOrDefault(x => max ? x.Item2 >= bestTarget : x.Item2 <= bestTarget);
            var firstEff = (a == default(Tuple<double, double>)) ? row.Last().Item1 : a.Item1;
            var lastEff = (b == default(Tuple<double, double>)) ? row.Last().Item1 : b.Item1;
            if (minEff > firstEff) minEff = firstEff;
            if (maxEff < lastEff) maxEff = lastEff;
            if (problemSpecificMaxEff < lastEff) problemSpecificMaxEff = lastEff;
          }
          maxEfforts[problem.Key] = problemSpecificMaxEff;
        }
      }
      maxEff = Math.Min(maxEff, maxEfforts.Values.Min());

      var minZeros = (int)Math.Floor(Math.Log10(minEff));
      var maxZeros = (int)Math.Floor(Math.Log10(maxEff));
      var axisMin = (decimal)Math.Pow(10, minZeros);
      var axisMax = (decimal)Math.Pow(10, maxZeros);
      if (!targetLogScalingCheckBox.Checked) {
        var minAdd = (decimal)Math.Pow(10, minZeros - 1) * 2;
        var maxAdd = (decimal)Math.Pow(10, maxZeros - 1) * 2;
        while (axisMin + minAdd < (decimal)minEff) axisMin += minAdd;
        while (axisMax <= (decimal)maxEff) axisMax += maxAdd;
      } else axisMax = (decimal)Math.Pow(10, (int)Math.Ceiling(Math.Log10(maxEff)));
      targetChart.ChartAreas[0].AxisX.Minimum = (double)axisMin;
      targetChart.ChartAreas[0].AxisX.Maximum = (double)axisMax;

      foreach (var group in groupedRuns) {
        // hits describes the number of target hits at a certain time for a certain group
        var hits = new Dictionary<string, SortedList<double, int>>();
        // misses describes the number of target misses after a certain time for a certain group
        // for instance when a run ends, but has not achieved all targets, misses describes
        // how many targets have been left open at the point when the run ended
        var misses = new Dictionary<string, SortedList<double, int>>();
        var maxLength = 0.0;

        var noRuns = 0;
        foreach (var problem in group.Value) {
          foreach (var run in problem.Value.Item2) {
            var resultsTable = (IndexedDataTable<double>)run.Results[table];
            xAxisTitles.Add(resultsTable.VisualProperties.XAxisTitle);

            if (aggregateTargetsCheckBox.Checked) {
              var length = CalculateHitsForAllTargets(hits, misses, resultsTable.Rows.First(), problem.Key, group.Key, problem.Value.Item1, maxEff);
              maxLength = Math.Max(length, maxLength);
            } else {
              CalculateHitsForEachTarget(hits, misses, resultsTable.Rows.First(), problem.Key, group.Key, problem.Value.Item1, maxEff);
            }
            noRuns++;
          }
        }
        foreach (var list in hits) {
          var row = new Series(list.Key) {
            ChartType = SeriesChartType.StepLine,
            BorderWidth = 2,
            Color = colors[colorCount],
            BorderDashStyle = lineStyles[lineStyleCount],
          };
          var rowShade = new Series(list.Key + "-range") {
            IsVisibleInLegend = false,
            ChartType = SeriesChartType.Range,
            Color = Color.FromArgb(32, colors[colorCount]),
            YValuesPerPoint = 2
          };

          var ecdf = 0.0;
          var missedecdf = 0.0;
          var iter = misses[list.Key].GetEnumerator();
          var moreMisses = iter.MoveNext();
          var totalTargets = noRuns;
          if (aggregateTargetsCheckBox.Checked) totalTargets *= targets.Length;
          var movingTargets = totalTargets;
          foreach (var h in list.Value) {
            while (moreMisses && iter.Current.Key <= h.Key) {
              missedecdf += iter.Current.Value;
              movingTargets -= iter.Current.Value;
              if (row.Points.Count > 0 && row.Points.Last().XValue == iter.Current.Key)
                row.Points.Last().SetValueY(ecdf / movingTargets);
              else row.Points.AddXY(iter.Current.Key, ecdf / movingTargets);
              if (boundShadingCheckBox.Checked) {
                if (rowShade.Points.Count > 0 && rowShade.Points.Last().XValue == iter.Current.Key)
                  rowShade.Points.Last().SetValueY(ecdf / totalTargets, (ecdf + missedecdf) / totalTargets);
                else rowShade.Points.Add(new DataPoint(iter.Current.Key, new[] { ecdf / totalTargets, (ecdf + missedecdf) / totalTargets }));
              }
              moreMisses = iter.MoveNext();
            }
            ecdf += h.Value;
            if (row.Points.Count > 0 && row.Points.Last().XValue == h.Key)
              row.Points.Last().SetValueY(ecdf / movingTargets);
            else row.Points.AddXY(h.Key, ecdf / movingTargets);
            if (missedecdf > 0 && boundShadingCheckBox.Checked) {
              if (rowShade.Points.Count > 0 && rowShade.Points.Last().XValue == h.Key)
                rowShade.Points.Last().SetValueY(ecdf / totalTargets, (ecdf + missedecdf) / totalTargets);
              else rowShade.Points.Add(new DataPoint(h.Key, new[] { ecdf / totalTargets, (ecdf + missedecdf) / totalTargets }));
            }
          }

          while (moreMisses) {
            // if there are misses beyond the last hit we extend the shaded area
            missedecdf += iter.Current.Value;
            //movingTargets -= iter.Current.Value;
            if (row.Points.Count > 0 && row.Points.Last().XValue == iter.Current.Key)
              row.Points.Last().SetValueY(ecdf / movingTargets);
            else row.Points.AddXY(iter.Current.Key, ecdf / movingTargets);
            if (boundShadingCheckBox.Checked) {
              if (rowShade.Points.Count > 0 && rowShade.Points.Last().XValue == iter.Current.Key)
                rowShade.Points.Last().SetValueY(ecdf / totalTargets, (ecdf + missedecdf) / totalTargets);
              else rowShade.Points.Add(new DataPoint(iter.Current.Key, new[] { ecdf / totalTargets, (ecdf + missedecdf) / totalTargets }));
            }
            moreMisses = iter.MoveNext();
          }

          if (maxLength > 0 && (row.Points.Count == 0 || row.Points.Last().XValue < maxLength))
            row.Points.AddXY(maxLength, ecdf / movingTargets);

          if (row.Points.Count > 0) {
            var point = row.Points.Last();
            point.Label = row.Name;
            point.MarkerStyle = MarkerStyle.Cross;
            point.MarkerBorderWidth = 1;
          }

          ConfigureSeries(row);
          targetChart.Series.Add(rowShade);
          targetChart.Series.Add(row);
        }
        colorCount = (colorCount + 1) % colors.Length;
        if (colorCount == 0) lineStyleCount = (lineStyleCount + 1) % lineStyles.Length;
      }

      if (targets.Length == 1)
        targetChart.ChartAreas[0].AxisY.Title = "Probability to be " + (targets[0] * 100) + "% worse than best";
      else targetChart.ChartAreas[0].AxisY.Title = "Proportion of reached targets";
      targetChart.ChartAreas[0].AxisX.Title = string.Join(" / ", xAxisTitles);
      targetChart.ChartAreas[0].AxisX.IsLogarithmic = CanDisplayLogarithmic();
      targetChart.ChartAreas[0].CursorY.Interval = 0.05;
      UpdateErtTables(groupedRuns);
    }

    private void GenerateDefaultTargets() {
      targets = new[] { 0.1, 0.05, 0.02, 0.01, 0 };
      suppressTargetsEvents = true;
      targetsTextBox.Text = string.Join("% ; ", targets.Select(x => x * 100)) + "%";
      suppressTargetsEvents = false;
    }

    private void CalculateHitsForEachTarget(Dictionary<string, SortedList<double, int>> hits,
                                            Dictionary<string, SortedList<double, int>> misses,
                                            IndexedDataRow<double> row, ProblemDescription problem,
                                            string group, double bestTarget, double maxEffort) {
      foreach (var t in targets.Select(x => Tuple.Create((problem.IsMaximization() ? (1 - x) : (1 + x)) * bestTarget, x))) {
        var l = t.Item1;
        var key = group + "_" + (t.Item2 * 100) + "%_" + l;
        if (!hits.ContainsKey(key)) {
          hits.Add(key, new SortedList<double, int>());
          misses.Add(key, new SortedList<double, int>());
        }
        var hit = false;
        foreach (var v in row.Values) {
          if (v.Item1 > maxEffort) break;
          if (problem.IsMaximization() && v.Item2 >= l || !problem.IsMaximization() && v.Item2 <= l) {
            if (hits[key].ContainsKey(v.Item1))
              hits[key][v.Item1]++;
            else hits[key][v.Item1] = 1;
            hit = true;
            break;
          }
        }
        if (!hit) {
          var max = Math.Min(row.Values.Last().Item1, maxEffort);
          if (misses[key].ContainsKey(max))
            misses[key][max]++;
          else misses[key][max] = 1;
        }
      }
    }

    private double CalculateHitsForAllTargets(Dictionary<string, SortedList<double, int>> hits,
                                              Dictionary<string, SortedList<double, int>> misses,
                                              IndexedDataRow<double> row, ProblemDescription problem,
                                              string group, double bestTarget, double maxEffort) {
      var values = row.Values;
      if (!hits.ContainsKey(group)) {
        hits.Add(group, new SortedList<double, int>());
        misses.Add(group, new SortedList<double, int>());
      }

      var i = 0;
      var j = 0;
      while (i < targets.Length && j < values.Count) {
        var target = (problem.IsMaximization() ? (1 - targets[i]) : (1 + targets[i])) * bestTarget;
        var current = values[j];
        if (current.Item1 > maxEffort) break;
        if (problem.IsMaximization() && current.Item2 >= target
          || !problem.IsMaximization() && current.Item2 <= target) {
          if (hits[group].ContainsKey(current.Item1)) hits[group][current.Item1]++;
          else hits[group][current.Item1] = 1;
          i++;
        } else {
          j++;
        }
      }
      if (j == values.Count) j--;
      var effort = Math.Min(values[j].Item1, maxEffort);
      if (i < targets.Length) {
        if (misses[group].ContainsKey(effort))
          misses[group][effort] += targets.Length - i;
        else misses[group][effort] = targets.Length - i;
      }
      return effort;
    }

    private void UpdateErtTables(Dictionary<string, Dictionary<ProblemDescription, Tuple<double, List<IRun>>>> groupedRuns) {
      ertTableView.Content = null;
      var columns = 1 + targets.Length + 1;
      var matrix = new string[groupedRuns.Count * groupedRuns.Max(x => x.Value.Count) + groupedRuns.Max(x => x.Value.Count), columns];
      var rowCount = 0;

      var tableName = (string)dataTableComboBox.SelectedItem;
      if (string.IsNullOrEmpty(tableName)) return;

      var targetsPerProblem = CalculateBestTargetPerProblemInstance(tableName);

      var colNames = new string[columns];
      colNames[0] = colNames[colNames.Length - 1] = string.Empty;
      for (var i = 0; i < targets.Length; i++) {
        colNames[i + 1] = targets[i].ToString("0.0%");
      }
      var problems = groupedRuns.SelectMany(x => x.Value.Keys).Distinct().ToList();

      foreach (var problem in problems) {
        matrix[rowCount, 0] = problem.ToString();
        for (var i = 0; i < targets.Length; i++) {
          matrix[rowCount, i + 1] = (targetsPerProblem[problem] * (problem.IsMaximization() ? (1 - targets[i]) : (1 + targets[i]))).ToString(CultureInfo.CurrentCulture.NumberFormat);
        }
        matrix[rowCount, columns - 1] = "#succ";
        rowCount++;

        foreach (var group in groupedRuns) {
          matrix[rowCount, 0] = group.Key;
          if (!group.Value.ContainsKey(problem)) {
            matrix[rowCount, columns - 1] = "N/A";
            rowCount++;
            continue;
          }
          var runs = group.Value[problem].Item2;
          ErtCalculationResult result = default(ErtCalculationResult);
          for (var i = 0; i < targets.Length; i++) {
            result = ExpectedRuntimeHelper.CalculateErt(runs, tableName, (problem.IsMaximization() ? (1 - targets[i]) : (1 + targets[i])) * group.Value[problem].Item1, problem.IsMaximization());
            matrix[rowCount, i + 1] = result.ToString();
          }
          matrix[rowCount, columns - 1] = targets.Length > 0 ? result.SuccessfulRuns + "/" + result.TotalRuns : "-";
          rowCount++;
        }
      }
      ertTableView.Content = new StringMatrix(matrix) { ColumnNames = colNames };
      ertTableView.DataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
    }
    #endregion

    #region Performance analysis by (multiple) budget(s)
    private void UpdateResultsByCost() {
      // necessary to reset log scale -> empty chart cannot use log scaling
      byCostDataTable.VisualProperties.XAxisLogScale = false;
      byCostDataTable.Rows.Clear();

      var table = (string)dataTableComboBox.SelectedItem;
      if (string.IsNullOrEmpty(table)) return;

      if (budgets == null) GenerateDefaultBudgets(table);

      var groupedRuns = GroupRuns();
      if (groupedRuns.Count == 0) return;

      var colorCount = 0;
      var lineStyleCount = 0;

      var targetsPerProblem = CalculateBestTargetPerProblemInstance((string)dataTableComboBox.SelectedItem);

      foreach (var group in groupedRuns) {
        var hits = new Dictionary<string, SortedList<double, double>>();

        foreach (var problem in group.Value) {
          foreach (var run in problem.Value.Item2) {
            var resultsTable = (IndexedDataTable<double>)run.Results[table];

            if (eachOrAllBudgetsCheckBox.Checked) {
              CalculateHitsForEachBudget(hits, resultsTable.Rows.First(), group.Value.Count, problem.Key, group.Key, problem.Value.Item2.Count, targetsPerProblem[problem.Key]);
            } else {
              CalculateHitsForAllBudgets(hits, resultsTable.Rows.First(), group.Value.Count, problem.Key, group.Key, problem.Value.Item2.Count, targetsPerProblem[problem.Key]);
            }
          }
        }

        foreach (var list in hits) {
          var row = new IndexedDataRow<double>(list.Key) {
            VisualProperties = {
              ChartType = DataRowVisualProperties.DataRowChartType.StepLine,
              LineWidth = 2,
              Color = colors[colorCount],
              LineStyle = hlLineStyles[lineStyleCount],
              StartIndexZero = false
            }
          };

          var total = 0.0;
          foreach (var h in list.Value) {
            total += h.Value;
            row.Values.Add(Tuple.Create(h.Key, total));
          }

          byCostDataTable.Rows.Add(row);
        }
        colorCount = (colorCount + 1) % colors.Length;
        if (colorCount == 0) lineStyleCount = (lineStyleCount + 1) % lineStyles.Length;
      }

      byCostDataTable.VisualProperties.XAxisTitle = "Targets to Best-Known Ratio";
      byCostDataTable.VisualProperties.XAxisLogScale = byCostDataTable.Rows.Count > 0 && budgetLogScalingCheckBox.Checked;
    }

    private void GenerateDefaultBudgets(string table) {
      var runs = GroupRuns().SelectMany(x => x.Value.Values).SelectMany(x => x.Item2).ToList();
      var min = runs.Select(x => ((IndexedDataTable<double>)x.Results[table]).Rows.First().Values.Select(y => y.Item1).Min()).Min();
      var max = runs.Select(x => ((IndexedDataTable<double>)x.Results[table]).Rows.First().Values.Select(y => y.Item1).Max()).Max();

      var maxMagnitude = (int)Math.Ceiling(Math.Log10(max));
      var minMagnitude = (int)Math.Floor(Math.Log10(min));
      if (maxMagnitude - minMagnitude >= 3) {
        budgets = new double[maxMagnitude - minMagnitude];
        for (var i = minMagnitude; i < maxMagnitude; i++) {
          budgets[i - minMagnitude] = Math.Pow(10, i);
        }
      } else {
        var range = max - min;
        budgets = Enumerable.Range(0, 6).Select(x => min + (x / 5.0) * range).ToArray();
      }
      suppressBudgetsEvents = true;
      budgetsTextBox.Text = string.Join(" ; ", budgets);
      suppressBudgetsEvents = false;
    }

    private void CalculateHitsForEachBudget(Dictionary<string, SortedList<double, double>> hits, IndexedDataRow<double> row, int groupCount, ProblemDescription problem, string groupName, int problemCount, double bestTarget) {
      foreach (var b in budgets) {
        var key = groupName + "-" + b;
        if (!hits.ContainsKey(key)) hits.Add(key, new SortedList<double, double>());
        Tuple<double, double> prev = null;
        foreach (var v in row.Values) {
          if (v.Item1 >= b) {
            // the budget may be too low to achieve any target
            if (prev == null && v.Item1 != b) break;
            var tgt = ((prev == null || v.Item1 == b) ? v.Item2 : prev.Item2);
            tgt = problem.IsMaximization() ? bestTarget / tgt : tgt / bestTarget;
            if (hits[key].ContainsKey(tgt))
              hits[key][tgt] += 1.0 / (groupCount * problemCount);
            else hits[key][tgt] = 1.0 / (groupCount * problemCount);
            break;
          }
          prev = v;
        }
        if (hits[key].Count == 0) hits.Remove(key);
      }
    }

    private void CalculateHitsForAllBudgets(Dictionary<string, SortedList<double, double>> hits, IndexedDataRow<double> row, int groupCount, ProblemDescription problem, string groupName, int problemCount, double bestTarget) {
      var values = row.Values;
      if (!hits.ContainsKey(groupName)) hits.Add(groupName, new SortedList<double, double>());

      var i = 0;
      var j = 0;
      Tuple<double, double> prev = null;
      while (i < budgets.Length && j < values.Count) {
        var current = values[j];
        if (current.Item1 >= budgets[i]) {
          if (prev != null || current.Item1 == budgets[i]) {
            var tgt = (prev == null || current.Item1 == budgets[i]) ? current.Item2 : prev.Item2;
            tgt = problem.IsMaximization() ? bestTarget / tgt : tgt / bestTarget;
            if (!hits[groupName].ContainsKey(tgt)) hits[groupName][tgt] = 0;
            hits[groupName][tgt] += 1.0 / (groupCount * problemCount * budgets.Length);
          }
          i++;
        } else {
          j++;
          prev = current;
        }
      }
      var lastTgt = values.Last().Item2;
      lastTgt = problem.IsMaximization() ? bestTarget / lastTgt : lastTgt / bestTarget;
      if (i < budgets.Length && !hits[groupName].ContainsKey(lastTgt)) hits[groupName][lastTgt] = 0;
      while (i < budgets.Length) {
        hits[groupName][lastTgt] += 1.0 / (groupCount * problemCount * budgets.Length);
        i++;
      }
    }
    #endregion

    private void UpdateCaption() {
      Caption = Content != null ? Content.OptimizerName + " RLD View" : ViewAttribute.GetViewName(GetType());
    }

    private void groupComboBox_SelectedIndexChanged(object sender, EventArgs e) {
      UpdateRuns();
      SetEnabledStateOfControls();
    }
    private void problemComboBox_SelectedIndexChanged(object sender, EventArgs e) {
      UpdateRuns();
      SetEnabledStateOfControls();
    }
    private void dataTableComboBox_SelectedIndexChanged(object sender, EventArgs e) {
      if (dataTableComboBox.SelectedIndex >= 0)
        GenerateDefaultBudgets((string)dataTableComboBox.SelectedItem);
      UpdateRuns();
      SetEnabledStateOfControls();
    }

    private void logScalingCheckBox_CheckedChanged(object sender, EventArgs e) {
      UpdateResultsByTarget();
      byCostDataTable.VisualProperties.XAxisLogScale = byCostDataTable.Rows.Count > 0 && budgetLogScalingCheckBox.Checked;
    }

    private void boundShadingCheckBox_CheckedChanged(object sender, EventArgs e) {
      UpdateResultsByTarget();
    }

    #region Event handlers for target analysis
    private bool suppressTargetsEvents;
    private void targetsTextBox_Validating(object sender, CancelEventArgs e) {
      if (suppressTargetsEvents) return;
      var targetStrings = targetsTextBox.Text.Split(new[] { '%', ';', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
      var targetList = new List<decimal>();
      foreach (var ts in targetStrings) {
        decimal t;
        if (!decimal.TryParse(ts, out t)) {
          errorProvider.SetError(targetsTextBox, "Not all targets can be parsed: " + ts);
          e.Cancel = true;
          return;
        }
        targetList.Add(t / 100);
      }
      if (targetList.Count == 0) {
        errorProvider.SetError(targetsTextBox, "Give at least one target value!");
        e.Cancel = true;
        return;
      }
      e.Cancel = false;
      errorProvider.SetError(targetsTextBox, null);
      targets = targetList.Select(x => (double)x).OrderByDescending(x => x).ToArray();
      suppressTargetsEvents = true;
      try { targetsTextBox.Text = string.Join("% ; ", targets.Select(x => x * 100)) + "%"; } finally { suppressTargetsEvents = false; }

      UpdateResultsByTarget();
      SetEnabledStateOfControls();
    }

    private void aggregateTargetsCheckBox_CheckedChanged(object sender, EventArgs e) {
      SuspendRepaint();
      try {
        UpdateResultsByTarget();
      } finally { ResumeRepaint(true); }
    }

    private void generateTargetsButton_Click(object sender, EventArgs e) {
      decimal max = 1, min = 0, count = 10;
      if (targets != null) {
        max = (decimal)targets.Max();
        min = (decimal)targets.Min();
        count = targets.Length;
      } else if (Content.Count > 0 && dataTableComboBox.SelectedIndex >= 0) {
        var table = (string)dataTableComboBox.SelectedItem;
        max = (decimal)Content.Where(x => x.Results.ContainsKey(table)).Select(x => ((IndexedDataTable<double>)x.Results[table]).Rows.First().Values.Max(y => y.Item2)).Max();
        min = (decimal)Content.Where(x => x.Results.ContainsKey(table)).Select(x => ((IndexedDataTable<double>)x.Results[table]).Rows.First().Values.Min(y => y.Item2)).Min();
        count = 6;
      }
      using (var dialog = new DefineArithmeticProgressionDialog(false, min, max, (max - min) / count)) {
        if (dialog.ShowDialog() == DialogResult.OK) {
          if (dialog.Values.Any()) {
            targets = dialog.Values.Select(x => (double)x).ToArray();
            suppressTargetsEvents = true;
            targetsTextBox.Text = string.Join("% ; ", targets);
            suppressTargetsEvents = false;

            UpdateResultsByTarget();
            SetEnabledStateOfControls();
          }
        }
      }
    }

    private void addTargetsAsResultButton_Click(object sender, EventArgs e) {
      var table = (string)dataTableComboBox.SelectedItem;

      var targetsPerProblem = CalculateBestTargetPerProblemInstance(table);

      foreach (var run in Content) {
        if (!run.Results.ContainsKey(table)) continue;
        var resultsTable = (IndexedDataTable<double>)run.Results[table];
        var values = resultsTable.Rows.First().Values;
        var i = 0;
        var j = 0;
        var pd = new ProblemDescription(run);
        while (i < targets.Length && j < values.Count) {
          var target = (pd.IsMaximization() ? (1 - targets[i]) : (1 + targets[i])) * targetsPerProblem[pd];
          var current = values[j];
          if (pd.IsMaximization() && current.Item2 >= target
              || !pd.IsMaximization() && current.Item2 <= target) {
            run.Results[table + ".Target" + target] = new DoubleValue(current.Item1);
            i++;
          } else {
            j++;
          }
        }
      }
    }
    #endregion

    #region Event handlers for cost analysis
    private bool suppressBudgetsEvents;
    private void budgetsTextBox_Validating(object sender, CancelEventArgs e) {
      if (suppressBudgetsEvents) return;
      var budgetStrings = budgetsTextBox.Text.Split(new[] { ';', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
      var budgetList = new List<double>();
      foreach (var ts in budgetStrings) {
        double b;
        if (!double.TryParse(ts, out b)) {
          errorProvider.SetError(budgetsTextBox, "Not all targets can be parsed: " + ts);
          e.Cancel = true;
          return;
        }
        budgetList.Add(b);
      }
      if (budgetList.Count == 0) {
        errorProvider.SetError(budgetsTextBox, "Give at least one target value!");
        e.Cancel = true;
        return;
      }
      e.Cancel = false;
      errorProvider.SetError(budgetsTextBox, null);
      budgets = budgetList.ToArray();
      UpdateResultsByCost();
      SetEnabledStateOfControls();
    }

    private void eachOrAllBudgetsCheckBox_CheckedChanged(object sender, EventArgs e) {
      var each = eachOrAllBudgetsCheckBox.Checked;
      eachOrAllBudgetsCheckBox.Text = each ? "each" : "all";
      SuspendRepaint();
      try {
        UpdateResultsByCost();
      } finally { ResumeRepaint(true); }
    }

    private void generateBudgetsButton_Click(object sender, EventArgs e) {
      decimal max = 1, min = 0, count = 10;
      if (budgets != null) {
        max = (decimal)budgets.Max();
        min = (decimal)budgets.Min();
        count = budgets.Length;
      } else if (Content.Count > 0 && dataTableComboBox.SelectedIndex >= 0) {
        var table = (string)dataTableComboBox.SelectedItem;
        min = (decimal)Content.Where(x => x.Results.ContainsKey(table)).Select(x => ((IndexedDataTable<double>)x.Results[table]).Rows.First().Values.Min(y => y.Item1)).Min();
        max = (decimal)Content.Where(x => x.Results.ContainsKey(table)).Select(x => ((IndexedDataTable<double>)x.Results[table]).Rows.First().Values.Max(y => y.Item1)).Max();
        count = 6;
      }
      using (var dialog = new DefineArithmeticProgressionDialog(false, min, max, (max - min) / count)) {
        if (dialog.ShowDialog() == DialogResult.OK) {
          if (dialog.Values.Any()) {
            budgets = dialog.Values.OrderBy(x => x).Select(x => (double)x).ToArray();

            suppressBudgetsEvents = true;
            budgetsTextBox.Text = string.Join(" ; ", budgets);
            suppressBudgetsEvents = false;

            UpdateResultsByCost();
            SetEnabledStateOfControls();
          }
        }
      }
    }

    private void addBudgetsAsResultButton_Click(object sender, EventArgs e) {
      var table = (string)dataTableComboBox.SelectedItem;
      var budgetStrings = budgetsTextBox.Text.Split(new[] { ';', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
      if (budgetStrings.Length == 0) {
        MessageBox.Show("Define a number of budgets.");
        return;
      }
      var budgetList = new List<double>();
      foreach (var bs in budgetStrings) {
        double v;
        if (!double.TryParse(bs, out v)) {
          MessageBox.Show("Budgets must be a valid number: " + bs);
          return;
        }
        budgetList.Add(v);
      }
      budgetList.Sort();

      foreach (var run in Content) {
        if (!run.Results.ContainsKey(table)) continue;
        var resultsTable = (IndexedDataTable<double>)run.Results[table];
        var values = resultsTable.Rows.First().Values;
        var i = 0;
        var j = 0;
        Tuple<double, double> prev = null;
        while (i < budgetList.Count && j < values.Count) {
          var current = values[j];
          if (current.Item1 >= budgetList[i]) {
            if (prev != null || current.Item1 == budgetList[i]) {
              var tgt = (prev == null || current.Item1 == budgetList[i]) ? current.Item2 : prev.Item2;
              run.Results[table + ".Cost" + budgetList[i]] = new DoubleValue(tgt);
            }
            i++;
          } else {
            j++;
            prev = current;
          }
        }
      }
    }
    #endregion

    #region Helpers
    private Dictionary<ProblemDescription, double> CalculateBestTargetPerProblemInstance(string table) {
      return (from r in Content
              where r.Visible
              let pd = new ProblemDescription(r)
              let target = r.Parameters.ContainsKey("BestKnownQuality")
                           && r.Parameters["BestKnownQuality"] is DoubleValue
                ? ((DoubleValue)r.Parameters["BestKnownQuality"]).Value
                : ((IndexedDataTable<double>)r.Results[table]).Rows.First().Values.Last().Item2
              group target by pd into g
              select new { Problem = g.Key, Target = g.Key.IsMaximization() ? g.Max() : g.Min() })
        .ToDictionary(x => x.Problem, x => x.Target);
    }

    private void UpdateRuns() {
      if (InvokeRequired) {
        Invoke((Action)UpdateRuns);
        return;
      }
      SuspendRepaint();
      try {
        UpdateResultsByTarget();
        UpdateResultsByCost();
      } finally { ResumeRepaint(true); }
    }
    #endregion

    private void ConfigureSeries(Series series) {
      series.SmartLabelStyle.Enabled = true;
      series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No;
      series.SmartLabelStyle.CalloutLineAnchorCapStyle = LineAnchorCapStyle.None;
      series.SmartLabelStyle.CalloutLineColor = series.Color;
      series.SmartLabelStyle.CalloutLineWidth = 2;
      series.SmartLabelStyle.CalloutStyle = LabelCalloutStyle.Underlined;
      series.SmartLabelStyle.IsOverlappedHidden = false;
      series.SmartLabelStyle.MaxMovingDistance = 200;
      series.ToolTip = series.LegendText + " X = #VALX, Y = #VALY";
    }

    private void chart_MouseDown(object sender, MouseEventArgs e) {
      HitTestResult result = targetChart.HitTest(e.X, e.Y);
      if (result.ChartElementType == ChartElementType.LegendItem) {
        ToggleTargetChartSeriesVisible(result.Series);
      }
    }
    private void chart_MouseMove(object sender, MouseEventArgs e) {
      HitTestResult result = targetChart.HitTest(e.X, e.Y);
      if (result.ChartElementType == ChartElementType.LegendItem)
        this.Cursor = Cursors.Hand;
      else
        this.Cursor = Cursors.Default;
    }
    private void chart_CustomizeLegend(object sender, CustomizeLegendEventArgs e) {
      foreach (LegendItem legendItem in e.LegendItems) {
        var series = targetChart.Series[legendItem.SeriesName];
        if (series != null) {
          bool seriesIsInvisible = invisibleTargetSeries.Any(x => x.Name == series.Name);
          foreach (LegendCell cell in legendItem.Cells) {
            cell.ForeColor = seriesIsInvisible ? Color.Gray : Color.Black;
          }
        }
      }
    }

    private void ToggleTargetChartSeriesVisible(Series series) {
      var indexList = invisibleTargetSeries.FindIndex(x => x.Name == series.Name);
      var indexChart = targetChart.Series.IndexOf(series);
      if (targetChart.Series.Count == 1) targetChart.ChartAreas[0].AxisX.IsLogarithmic = false;
      targetChart.Series.RemoveAt(indexChart);
      var s = indexList >= 0 ? invisibleTargetSeries[indexList] : new Series(series.Name) {
        Color = series.Color,
        ChartType = series.ChartType,
        BorderWidth = series.BorderWidth,
        BorderDashStyle = series.BorderDashStyle
      };
      if (indexList < 0) {
        // hide
        invisibleTargetSeries.Add(series);
        var shadeSeries = targetChart.Series.FirstOrDefault(x => x.Name == series.Name + "-range");
        if (shadeSeries != null) {
          if (targetChart.Series.Count == 1) targetChart.ChartAreas[0].AxisX.IsLogarithmic = false;
          targetChart.Series.Remove(shadeSeries);
          invisibleTargetSeries.Add(shadeSeries);
          indexChart--;
        }
      } else {
        // show
        invisibleTargetSeries.RemoveAt(indexList);
        var shadeSeries = invisibleTargetSeries.FirstOrDefault(x => x.Name == series.Name + "-range");
        if (shadeSeries != null) {
          invisibleTargetSeries.Remove(shadeSeries);
          InsertOrAddSeries(indexChart, shadeSeries);
          indexChart++;
        }
      }
      InsertOrAddSeries(indexChart, s);
      targetChart.ChartAreas[0].AxisX.IsLogarithmic = CanDisplayLogarithmic();
    }

    private bool CanDisplayLogarithmic() {
      return targetLogScalingCheckBox.Checked
        && targetChart.Series.Count > 0 // must have a series
        && targetChart.Series.Any(x => x.Points.Count > 0) // at least one series must have points
        && targetChart.Series.All(s => s.Points.All(p => p.XValue > 0)); // all points must be positive
    }

    private void InsertOrAddSeries(int index, Series s) {
      if (targetChart.Series.Count <= index)
        targetChart.Series.Add(s);
      else targetChart.Series.Insert(index, s);
    }

    private class ProblemDescription {
      private readonly bool matchAll;
      public static readonly ProblemDescription MatchAll = new ProblemDescription() {
        ProblemName = "All with Best-Known"
      };

      private ProblemDescription() {
        ProblemType = string.Empty;
        ProblemName = string.Empty;
        Evaluator = string.Empty;
        Maximization = string.Empty;
        DisplayProblemType = false;
        DisplayProblemName = false;
        DisplayEvaluator = false;
        DisplayMaximization = false;
        matchAll = true;
      }

      public ProblemDescription(IRun run) {
        ProblemType = GetStringValueOrEmpty(run, "Problem Type");
        ProblemName = GetStringValueOrEmpty(run, "Problem Name");
        Evaluator = GetStringValueOrEmpty(run, "Evaluator");
        Maximization = GetMaximizationValueOrEmpty(run, "Maximization");
        DisplayProblemType = !string.IsNullOrEmpty(ProblemType);
        DisplayProblemName = !string.IsNullOrEmpty(ProblemName);
        DisplayEvaluator = !string.IsNullOrEmpty(Evaluator);
        DisplayMaximization = !string.IsNullOrEmpty(Maximization);
        matchAll = false;
      }

      public bool DisplayProblemType { get; set; }
      public string ProblemType { get; set; }
      public bool DisplayProblemName { get; set; }
      public string ProblemName { get; set; }
      public bool DisplayEvaluator { get; set; }
      public string Evaluator { get; set; }
      public bool DisplayMaximization { get; set; }
      public string Maximization { get; set; }

      public bool IsMaximization() {
        return Maximization == "MAX";
      }

      public bool Match(IRun run) {
        return matchAll ||
               GetStringValueOrEmpty(run, "Problem Type") == ProblemType
               && GetStringValueOrEmpty(run, "Problem Name") == ProblemName
               && GetStringValueOrEmpty(run, "Evaluator") == Evaluator
               && GetMaximizationValueOrEmpty(run, "Maximization") == Maximization;
      }

      private string GetStringValueOrEmpty(IRun run, string key) {
        return run.Parameters.ContainsKey(key) ? ((StringValue)run.Parameters[key]).Value : string.Empty;
      }

      private string GetMaximizationValueOrEmpty(IRun run, string key) {
        return run.Parameters.ContainsKey(key) ? (((BoolValue)run.Parameters[key]).Value ? "MAX" : "MIN") : string.Empty;
      }

      public override bool Equals(object obj) {
        var other = obj as ProblemDescription;
        if (other == null) return false;
        return ProblemType == other.ProblemType
               && ProblemName == other.ProblemName
               && Evaluator == other.Evaluator
               && Maximization == other.Maximization;
      }

      public override int GetHashCode() {
        return ProblemType.GetHashCode() ^ ProblemName.GetHashCode() ^ Evaluator.GetHashCode() ^ Maximization.GetHashCode();
      }

      public override string ToString() {
        return string.Join("  --  ", new[] {
          (DisplayProblemType ? ProblemType : string.Empty),
          (DisplayProblemName ? ProblemName : string.Empty),
          (DisplayEvaluator ? Evaluator : string.Empty),
          (DisplayMaximization ? Maximization : string.Empty)}.Where(x => !string.IsNullOrEmpty(x)));
      }
    }
  }
}