#region License Information
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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HeuristicLab.Core.Views;
using HeuristicLab.Data;
using HeuristicLab.Encodings.PermutationEncoding;
using HeuristicLab.MainForm;

namespace HeuristicLab.Problems.TravelingSalesman.Views {
  /// <summary>
  /// The base class for visual representations of a path tour for a TSP.
  /// </summary>
  [View("TSP Solution View")]
  [Content(typeof(ITSPSolution), true)]
  public partial class TSPSolutionView : ItemView {
    public new ITSPSolution Content {
      get { return (ITSPSolution)base.Content; }
      set { base.Content = value; }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TSPSolutionView"/>.
    /// </summary>
    public TSPSolutionView() {
      InitializeComponent();
    }

    protected override void DeregisterContentEvents() {
      Content.PropertyChanged -= ContentOnPropertyChanged;
      base.DeregisterContentEvents();
    }
    protected override void RegisterContentEvents() {
      base.RegisterContentEvents();
      Content.PropertyChanged += ContentOnPropertyChanged;
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content == null) {
        qualityViewHost.Content = null;
        pictureBox.Image = null;
        tourViewHost.Content = null;
      } else {
        qualityViewHost.Content = Content.TourLength;
        GenerateImage();
        tourViewHost.Content = Content.Tour;
      }
    }

    protected override void SetEnabledStateOfControls() {
      base.SetEnabledStateOfControls();
      qualityGroupBox.Enabled = Content != null;
      pictureBox.Enabled = Content != null;
      tourGroupBox.Enabled = Content != null;
    }

    protected virtual void GenerateImage() {
      if ((pictureBox.Width > 0) && (pictureBox.Height > 0)) {
        if (Content == null) {
          pictureBox.Image = null;
        } else {
          DoubleMatrix coordinates = Content.Coordinates;
          Permutation permutation = Content.Tour;
          Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);

          if ((coordinates != null) && (coordinates.Rows > 0) && (coordinates.Columns == 2)) {
            double xMin = double.MaxValue, yMin = double.MaxValue, xMax = double.MinValue, yMax = double.MinValue;
            for (int i = 0; i < coordinates.Rows; i++) {
              if (xMin > coordinates[i, 0]) xMin = coordinates[i, 0];
              if (yMin > coordinates[i, 1]) yMin = coordinates[i, 1];
              if (xMax < coordinates[i, 0]) xMax = coordinates[i, 0];
              if (yMax < coordinates[i, 1]) yMax = coordinates[i, 1];
            }

            int border = 20;
            double xStep = xMax != xMin ? (pictureBox.Width - 2 * border) / (xMax - xMin) : 1;
            double yStep = yMax != yMin ? (pictureBox.Height - 2 * border) / (yMax - yMin) : 1;

            Point[] points = new Point[coordinates.Rows];
            for (int i = 0; i < coordinates.Rows; i++)
              points[i] = new Point(border + ((int)((coordinates[i, 0] - xMin) * xStep)),
                                    bitmap.Height - (border + ((int)((coordinates[i, 1] - yMin) * yStep))));

            using (Graphics graphics = Graphics.FromImage(bitmap)) {
              if (permutation != null && permutation.Length > 1) {
                Point[] tour = new Point[permutation.Length];
                for (int i = 0; i < permutation.Length; i++) {
                  if (permutation[i] >= 0 && permutation[i] < points.Length)
                    tour[i] = points[permutation[i]];
                }
                graphics.DrawPolygon(Pens.Black, tour);
              }
              for (int i = 0; i < points.Length; i++)
                graphics.FillRectangle(Brushes.Red, points[i].X - 2, points[i].Y - 2, 6, 6);
            }
          } else {
            using (Graphics graphics = Graphics.FromImage(bitmap)) {
              graphics.Clear(Color.White);
              Font font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
              string text = "No coordinates defined or in wrong format.";
              SizeF strSize = graphics.MeasureString(text, font);
              graphics.DrawString(text, font, Brushes.Black, (float)(pictureBox.Width - strSize.Width) / 2.0f, (float)(pictureBox.Height - strSize.Height) / 2.0f);
            }
          }
          pictureBox.Image = bitmap;
        }
      }
    }

    protected virtual void ContentOnPropertyChanged(object sender, PropertyChangedEventArgs e) {
      if (InvokeRequired)
        Invoke((Action<object, PropertyChangedEventArgs>)ContentOnPropertyChanged, sender, e);
      else {
        switch (e.PropertyName) {
          case nameof(Content.Coordinates):
            GenerateImage();
            break;
          case nameof(Content.Tour):
            GenerateImage();
            tourViewHost.Content = Content.Tour;
            break;
          case nameof(Content.TourLength):
            qualityViewHost.Content = Content.TourLength;
            break;
        }
      }
    }

    private void pictureBox_SizeChanged(object sender, EventArgs e) {
      GenerateImage();
    }
  }
}