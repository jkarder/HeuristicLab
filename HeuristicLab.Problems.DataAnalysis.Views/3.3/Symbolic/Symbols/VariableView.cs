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

using System;
using System.Windows.Forms;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding.Views;
using HeuristicLab.MainForm;
using HeuristicLab.MainForm.WindowsForms;
using HeuristicLab.Problems.DataAnalysis.Symbolic.Symbols;

namespace HeuristicLab.Problems.DataAnalysis.Views.Symbolic.Symbols {
  [View("Variable View")]
  [Content(typeof(Variable), true)]
  public partial class VariableView : SymbolView {
    public new Variable Content {
      get { return (Variable)base.Content; }
      set { base.Content = value; }
    }

    public VariableView() {
      InitializeComponent();
    }

    protected override void RegisterContentEvents() {
      base.RegisterContentEvents();
      Content.Changed += new EventHandler(Content_Changed);
    }

    protected override void DeregisterContentEvents() {
      base.DeregisterContentEvents();
      Content.Changed -= new EventHandler(Content_Changed);
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      UpdateControl();
    }

    protected override void SetEnabledStateOfControls() {
      base.SetEnabledStateOfControls();
      weightInitializationMuTextBox.Enabled = Content != null;
      weightInitializationMuTextBox.ReadOnly = ReadOnly;
      weightInitializationSigmaTextBox.Enabled = Content != null;
      weightInitializationSigmaTextBox.ReadOnly = ReadOnly;
      additiveWeightChangeSigmaTextBox.Enabled = Content != null;
      additiveWeightChangeSigmaTextBox.ReadOnly = ReadOnly;
      multiplicativeWeightChangeSigmaTextBox.Enabled = Content != null;
      multiplicativeWeightChangeSigmaTextBox.ReadOnly = ReadOnly;
    }

    #region content event handlers
    private void Content_Changed(object sender, EventArgs e) {
      UpdateControl();
    }
    #endregion

    #region control event handlers
    private void weightMuTextBox_TextChanged(object sender, EventArgs e) {
      double nu;
      if (double.TryParse(weightInitializationMuTextBox.Text, out nu)) {
        Content.WeightMu = nu;
        errorProvider.SetError(weightInitializationMuTextBox, string.Empty);
      } else {
        errorProvider.SetError(weightInitializationMuTextBox, "Invalid value");
      }
    }
    private void weightSigmaTextBox_TextChanged(object sender, EventArgs e) {
      double sigma;
      if (double.TryParse(weightInitializationSigmaTextBox.Text, out sigma) && sigma >= 0.0) {
        Content.WeightSigma = sigma;
        errorProvider.SetError(weightInitializationSigmaTextBox, string.Empty);
      } else {
        errorProvider.SetError(weightInitializationSigmaTextBox, "Invalid value");
      }
    }

    private void additiveWeightChangeSigmaTextBox_TextChanged(object sender, EventArgs e) {
      double sigma;
      if (double.TryParse(additiveWeightChangeSigmaTextBox.Text, out sigma) && sigma >= 0.0) {
        Content.WeightManipulatorSigma = sigma;
        errorProvider.SetError(additiveWeightChangeSigmaTextBox, string.Empty);
      } else {
        errorProvider.SetError(additiveWeightChangeSigmaTextBox, "Invalid value");
      }
    }

    private void multiplicativeWeightChangeSigmaTextBox_TextChanged(object sender, EventArgs e) {
      double sigma;
      if (double.TryParse(multiplicativeWeightChangeSigmaTextBox.Text, out sigma) && sigma >= 0.0) {
        Content.MultiplicativeWeightManipulatorSigma = sigma;
        errorProvider.SetError(multiplicativeWeightChangeSigmaTextBox, string.Empty);
      } else {
        errorProvider.SetError(multiplicativeWeightChangeSigmaTextBox, "Invalid value");
      }
    }
    #endregion

    #region helpers
    private void UpdateControl() {
      if (Content == null) {
        weightInitializationMuTextBox.Text = string.Empty;
        weightInitializationSigmaTextBox.Text = string.Empty;
        additiveWeightChangeSigmaTextBox.Text = string.Empty;
        multiplicativeWeightChangeSigmaTextBox.Text = string.Empty;
      } else {
        weightInitializationMuTextBox.Text = Content.WeightMu.ToString();
        weightInitializationSigmaTextBox.Text = Content.WeightSigma.ToString();
        additiveWeightChangeSigmaTextBox.Text = Content.WeightManipulatorSigma.ToString();
        multiplicativeWeightChangeSigmaTextBox.Text = Content.MultiplicativeWeightManipulatorSigma.ToString();
      }
      SetEnabledStateOfControls();
    }
    #endregion
  }
}