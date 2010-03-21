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
using System.Windows.Forms;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;
using HeuristicLab.Optimization.Views;

namespace HeuristicLab.Problems.TSP.Views {
  /// <summary>
  /// The base class for visual representations of items.
  /// </summary>
  [View("TSP View")]
  [Content(typeof(TSP), true)]
  public sealed partial class TSPView : ProblemView {
    private TSPLIBImportDialog tsplibImportDialog;

    public new TSP Content {
      get { return (TSP)base.Content; }
      set { base.Content = value; }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ItemBaseView"/>.
    /// </summary>
    public TSPView() {
      InitializeComponent();
    }
    /// <summary>
    /// Intializes a new instance of <see cref="ItemBaseView"/> with the given <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item that should be displayed.</param>
    public TSPView(TSP content)
      : this() {
      Content = content;
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content == null) {
        importButton.Enabled = false;
      } else {
        importButton.Enabled = true;
      }
    }

    private void importButton_Click(object sender, System.EventArgs e) {
      if (tsplibImportDialog == null) tsplibImportDialog = new TSPLIBImportDialog();

      if (tsplibImportDialog.ShowDialog(this) == DialogResult.OK) {
        try {
          if (tsplibImportDialog.Quality == null)
            Content.ImportFromTSPLIB(tsplibImportDialog.TSPFileName, tsplibImportDialog.TourFileName);
          else
            Content.ImportFromTSPLIB(tsplibImportDialog.TSPFileName, tsplibImportDialog.TourFileName, (double)tsplibImportDialog.Quality);
        }
        catch (Exception ex) {
          Auxiliary.ShowErrorMessageBox(ex);
        }
      }
    }
  }
}
