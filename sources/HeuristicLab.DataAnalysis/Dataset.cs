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
using System.Xml;
using HeuristicLab.Core;
using HeuristicLab.Data;
using System.Globalization;
using System.Text;

namespace HeuristicLab.DataAnalysis {
  public class Dataset : ItemBase {

    private string name;
    public string Name {
      get { return name; }
      set { name = value; }
    }

    private double[] samples;
    private int rows;

    public int Rows {
      get { return rows; }
      set { rows = value; }
    }
    private int columns;

    public int Columns {
      get { return columns; }
      set { columns = value; }
    }
    private Dictionary<int, double[]>[] ranges;
    private Dictionary<int, double[]>[] means;

    public double GetValue(int i, int j) {
      return samples[columns * i + j];
    }

    public void SetValue(int i, int j, double v) {
      if(v != samples[columns * i + j]) {
        samples[columns * i + j] = v;
        FireChanged();
      }
    }

    public double[] Samples {
      get { return samples; }
      set { 
        samples = value;
        CreateDictionaries();
        FireChanged();
      }
    }

    private string[] variableNames;
    public string[] VariableNames {
      get { return variableNames; }
      set { variableNames = value; }
    }

    public Dataset() {
      Name = "-";
      VariableNames = new string[] {"Var0"};
      Columns = 1;
      Rows = 1;
      Samples = new double[1];
    }

    void samples_Changed(object sender, EventArgs e) {
      CreateDictionaries();
    }

    private void CreateDictionaries() {
      // keep a means and ranges dictionary for each column (possible target variable) of the dataset.

      means = new Dictionary<int, double[]>[columns];
      ranges = new Dictionary<int, double[]>[columns];

      for(int i = 0; i < columns; i++) {
        means[i] = new Dictionary<int, double[]>();
        ranges[i] = new Dictionary<int, double[]>();
      }
    }

    public override IView CreateView() {
      return new DatasetView(this);
    }

    public override object Clone(IDictionary<Guid, object> clonedObjects) {
      Dataset clone = new Dataset();
      clonedObjects.Add(Guid, clone);
      double[] cloneSamples = new double[rows * columns];
      Array.Copy(samples, cloneSamples, samples.Length);
      clone.rows = rows;
      clone.columns = columns;
      clone.Samples = cloneSamples;
      clone.Name = Name;
      clone.VariableNames = new string[VariableNames.Length];
      Array.Copy(VariableNames, clone.VariableNames, VariableNames.Length);
      return clone;
    }

    public override XmlNode GetXmlNode(string name, XmlDocument document, IDictionary<Guid, IStorable> persistedObjects) {
      XmlNode node = base.GetXmlNode(name, document, persistedObjects);
      XmlAttribute problemName = document.CreateAttribute("Name");
      problemName.Value = Name;
      node.Attributes.Append(problemName);
      XmlAttribute dim1 = document.CreateAttribute("Dimension1");
      dim1.Value = rows.ToString(CultureInfo.InvariantCulture.NumberFormat);
      node.Attributes.Append(dim1);
      XmlAttribute dim2 = document.CreateAttribute("Dimension2");
      dim2.Value = columns.ToString(CultureInfo.InvariantCulture.NumberFormat);
      node.Attributes.Append(dim2);

      XmlAttribute variableNames = document.CreateAttribute("VariableNames");
      variableNames.Value = GetVariableNamesString();
      node.Attributes.Append(variableNames);

      node.InnerText = ToString(CultureInfo.InvariantCulture.NumberFormat);
      return node;
    }

    public override void Populate(XmlNode node, IDictionary<Guid, IStorable> restoredObjects) {
      base.Populate(node, restoredObjects);
      Name = node.Attributes["Name"].Value;
      rows = int.Parse(node.Attributes["Dimension1"].Value, CultureInfo.InvariantCulture.NumberFormat);
      columns = int.Parse(node.Attributes["Dimension2"].Value, CultureInfo.InvariantCulture.NumberFormat);
      
      VariableNames = ParseVariableNamesString(node.Attributes["VariableNames"].Value);

      string[] tokens = node.InnerText.Split(';');
      if(tokens.Length != rows * columns) throw new FormatException();
      samples = new double[rows * columns];
      for(int row = 0; row < rows; row++) {
        for(int column = 0; column < columns; column++) {
          if(double.TryParse(tokens[row * columns + column], NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out samples[row*columns + column]) == false) {
            throw new FormatException("Can't parse " + tokens[row * columns + column] + " as double value.");
          }
        }
      }
      CreateDictionaries();
    }

    public override string ToString() {
      return ToString(CultureInfo.CurrentCulture.NumberFormat);
    }

    private string ToString(NumberFormatInfo format) {
      StringBuilder builder = new StringBuilder();
      for(int row = 0; row < rows; row++) {
        for(int column = 0; column < columns; column++) {
          builder.Append(";");
          builder.Append(samples[row*columns+column].ToString(format));
        }
      }
      if(builder.Length > 0) builder.Remove(0, 1);
      return builder.ToString();
    }

    private string GetVariableNamesString() {
      string s = "";
      for (int i = 0; i < variableNames.Length; i++) {
        s += variableNames[i] + "; ";
      }

      if (variableNames.Length > 0) {
        s = s.TrimEnd(';', ' ');
      }
      return s;
    }

    private string[] ParseVariableNamesString(string p) {
      p = p.Trim();
      string[] tokens = p.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
      return tokens;
    }

    public double GetMean(int column) {
      return GetMean(column, 0, Rows-1);
    }

    // return value of GetMean should be memoized because it is called repeatedly in Evaluators
    public double GetMean(int column, int from, int to) {
      Dictionary<int, double[]> columnMeans = means[column];
      if(columnMeans.ContainsKey(from)) {
        double[] fromMeans = columnMeans[from];
        if(fromMeans[to-from] >= 0.0) {
          // already calculated
          return fromMeans[to-from];
        } else {
          // not yet calculated => calculate
          fromMeans[to-from] = CalculateMean(column, from, to);
          return fromMeans[to-from];
        }
      } else {
        // never saw this from-index => create a new array, initialize and recalculate for to-index
        double[] fromMeans = new double[rows - from];
        // fill with negative values to indicate which means have already been calculated
        for(int i=0;i<fromMeans.Length;i++) {fromMeans[i] = -1.0;}
        // store new array in the dictionary
        columnMeans[from] = fromMeans;
        // calculate for specific to-index
        fromMeans[to-from] = CalculateMean(column, from, to);
        return fromMeans[to-from];
      }
    }

    private double CalculateMean(int column, int from, int to) {
      double[] values = new double[to - from +1];
      for(int sample = from; sample <= to; sample++) {
        values[sample - from] = GetValue(sample, column);
      }

      return Statistics.Mean(values);
    }

    public double GetRange(int column) {
      return GetRange(column, 0, Rows-1);
    }

    // return value of GetRange should be memoized because it is called repeatedly in Evaluators
    public double GetRange(int column, int from, int to) {
      Dictionary<int, double[]> columnRanges = ranges[column];
      if(columnRanges.ContainsKey(from)) {
        double[] fromRanges = columnRanges[from];
        if(fromRanges[to-from] >= 0.0) {
          // already calculated
          return fromRanges[to-from];
        } else {
          // not yet calculated => calculate
          fromRanges[to-from] = CalculateRange(column, from, to);
          return fromRanges[to-from];
        }
      } else {
        // never saw this from-index => create a new array, initialize and recalculate for to-index
        double[] fromRanges = new double[rows - from];
        // fill with negative values to indicate which means have already been calculated
        for(int i = 0; i < fromRanges.Length; i++) { fromRanges[i] = -1.0; }
        // store in dictionary
        columnRanges[from] = fromRanges;
        // calculate for specific to-index
        fromRanges[to-from] = CalculateRange(column, from, to);
        return fromRanges[to-from];
      }
    }

    private double CalculateRange(int column, int from, int to) {
      double[] values = new double[to - from + 1];
      for(int sample = from; sample <= to; sample++) {
        values[sample - from] = GetValue(sample, column);
      }

      return Statistics.Range(values);
    }
  }
}
