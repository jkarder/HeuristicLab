﻿#region License Information
/* HeuristicLab
 * Copyright (C) Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using HeuristicLab.MainForm;
using HeuristicLab.Problems.VehicleRouting.ProblemInstances;

namespace HeuristicLab.Problems.VehicleRouting.Views {
  [View("CVRPTW Evaluation")]
  [Content(typeof(CVRPTWEvaluation), IsDefaultView = true)]
  public partial class CVRPTWEvaluationView : CVRPEvaluationView {

    public new CVRPTWEvaluation Content {
      get => (CVRPTWEvaluation)base.Content;
      set => base.Content = value;
    }

    public CVRPTWEvaluationView() {
      InitializeComponent();
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      tardinessTextBox.Text = Content?.Tardiness.ToString() ?? string.Empty;
      travelTimeTextBox.Text = Content?.TravelTime.ToString() ?? string.Empty;
    }
  }
}