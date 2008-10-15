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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Charting;
using HeuristicLab.Charting.Data;

namespace HeuristicLab.Logging {
  public partial class PointXYChartView : ViewBase {
    private static int[] colors = new int[] {
      182,182,255, 
      218,255,182,
      255,182,218, 
      182,255,255, 
      218,182,255, 
      255,182,255,
      255,182,182, 
      255,218,182, 
      255,255,182, 
      182,255,182, 
      182,255,218, 
      182,218,255
    };

    public PointXYChart PointXYChart {
      get { return (PointXYChart)base.Item; }
      set { base.Item = value; }
    }

    public PointXYChartView() {
      InitializeComponent();
      Caption = "PointXYChart View";
    }
    public PointXYChartView(PointXYChart pointXYChart)
      : this() {
      PointXYChart = pointXYChart;
    }

    protected override void RemoveItemEvents() {
      if (PointXYChart != null) {
        PointXYChart.Values.ItemAdded -= new EventHandler<ItemIndexEventArgs>(Values_ItemAdded);
        PointXYChart.Values.ItemRemoved -= new EventHandler<ItemIndexEventArgs>(Values_ItemRemoved);
      }
      base.RemoveItemEvents();
    }
    protected override void AddItemEvents() {
      base.AddItemEvents();
      if (PointXYChart != null) {
        PointXYChart.Values.ItemAdded += new EventHandler<ItemIndexEventArgs>(Values_ItemAdded);
        PointXYChart.Values.ItemRemoved += new EventHandler<ItemIndexEventArgs>(Values_ItemRemoved);
      }
    }

    protected override void UpdateControls() {
      base.UpdateControls();
      Datachart datachart = new Datachart(-50, -5000, 1000, 55000);
      datachart.Title = "Point X/Y Chart";
      dataChartControl.ScaleOnResize = false;
      dataChartControl.Chart = datachart;
      datachart.Group.Clear();
      datachart.Group.Add(new Axis(datachart, 0, 0, AxisType.Both));
      double maxY = double.MinValue, minY = double.MaxValue;
      double maxX = double.MinValue, minX = double.MaxValue;
      if (PointXYChart != null) {
        datachart.UpdateEnabled = false;
        for (int i = 0; i < PointXYChart.Values.Count; i++) {
          int colorIndex = (i % 12) * 3;
          Color curCol = Color.FromArgb(colors[colorIndex], colors[colorIndex + 1], colors[colorIndex + 2]);
          Pen p = new Pen(curCol);
          SolidBrush b = new SolidBrush(curCol);
          datachart.AddDataRow(PointXYChart.ConnectDots.Data ? DataRowType.Lines : DataRowType.Points, p, b);
        }

        for (int i = 0; i < PointXYChart.Values.Count; i++) {
          ItemList<DoubleArrayData> list = (ItemList<DoubleArrayData>)PointXYChart.Values[i];
          for (int j = 0; j < list.Count; j++) {
            double[] value = ((DoubleArrayData)list[j]).Data;
            if (!double.IsInfinity(value[0]) && !double.IsNaN(value[0]) && !double.IsInfinity(value[1]) && !double.IsNaN(value[1])) {
              if (value[0] < minX) minX = value[0];
              if (value[0] > maxX) maxX = value[0];
              if (value[1] < minY) minY = value[1];
              if (value[1] > maxY) maxY = value[1];

              datachart.AddDataPoint(i, value[0], value[1]);
            }
          }
        }
        datachart.ZoomIn(minX - (minX * 0.1), minY - (minY * 0.1), maxX * 1.05 , maxY * 1.05);
        datachart.UpdateEnabled = true;
        datachart.EnforceUpdate();
      }
    }

    #region Values Events
    private delegate void ItemIndexDelegate(object sender, ItemIndexEventArgs e);
    private void Values_ItemRemoved(object sender, ItemIndexEventArgs e) {
      if (InvokeRequired) {
        Invoke(new ItemIndexDelegate(Values_ItemRemoved), sender, e);
      } else {
        Datachart datachart = dataChartControl.Chart;
      }
    }
    private void Values_ItemAdded(object sender, ItemIndexEventArgs e) {
      if (InvokeRequired) {
        Invoke(new ItemIndexDelegate(Values_ItemAdded), sender, e);
      } else {
        //Datachart datachart = dataChartControl.Chart;
        //ItemList list = (ItemList)e.Item;
        //datachart.UpdateEnabled = false;
        //for (int i = 0; i < list.Count; i++)
        //  datachart.AddDataPoint(i, e.Index, ((DoubleArrayData)list[i]).Data[0]);
        //datachart.UpdateEnabled = true;
        //datachart.EnforceUpdate();
      }
    }
    #endregion
  }
}
