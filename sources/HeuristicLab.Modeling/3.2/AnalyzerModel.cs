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
using System.Text;
using HeuristicLab.Core;
using HeuristicLab.DataAnalysis;

namespace HeuristicLab.Modeling {
  public class AnalyzerModel : IAnalyzerModel {
    public AnalyzerModel() { } // for persistence

    #region IModel Members

    private Dataset dataset;
    public Dataset Dataset {
      get { return dataset; }
      set { dataset = value; }
    }

    private string targetVariable;
    public string TargetVariable {
      get { return targetVariable; }
      set { targetVariable = value; }
    }

    private List<string> inputVariables = new List<string>();
    public IEnumerable<string> InputVariables {
      get { return inputVariables; }
    }

    public int TrainingSamplesStart { get; set; }
    public int TrainingSamplesEnd { get; set; }
    public int ValidationSamplesStart { get; set; }
    public int ValidationSamplesEnd { get; set; }
    public int TestSamplesStart { get; set; }
    public int TestSamplesEnd { get; set; }

    public void AddInputVariable(string variableName) {
      if (!inputVariables.Contains(variableName))
        inputVariables.Add(variableName);
    }

    private Dictionary<string, double> results = new Dictionary<string, double>();
    public IEnumerable<KeyValuePair<string, double>> Results {
      get { return results; }
    }

    public void SetResult(string name, double value) {
      results.Add(name, value);
    }

    public double GetResult(string name) {
      return results[name];
    }

    private Dictionary<string, double> metadata = new Dictionary<string, double>();
    public IEnumerable<KeyValuePair<string, double>> MetaData {
      get { return metadata; }
    }

    public void SetMetaData(string name, double value) {
      metadata.Add(name, value);
    }

    public double GetMetaData(string name) {
      return metadata[name];
    }

    public double GetVariableQualityImpact(string variableName) {
      if (variableQualityImpacts.ContainsKey(variableName)) return variableQualityImpacts[variableName];
      else throw new ArgumentException("Impact of variable " + variableName + " is not available.");
    }

    public double GetVariableEvaluationImpact(string variableName) {
      if (variableEvaluationImpacts.ContainsKey(variableName)) return variableEvaluationImpacts[variableName];
      else throw new ArgumentException("Impact of variable " + variableName + " is not available.");
    }

    public IPredictor Predictor { get; set; }

    #endregion

    private Dictionary<string, double> variableQualityImpacts = new Dictionary<string, double>();
    public void SetVariableQualityImpact(string variableName, double impact) {
      variableQualityImpacts[variableName] = impact;
    }

    public void SetVariableQualityImpact(int variableIndex, double impact) {
      variableQualityImpacts[dataset.GetVariableName(variableIndex)] = impact;
    }

    private Dictionary<string, double> variableEvaluationImpacts = new Dictionary<string, double>();
    public void SetVariableEvaluationImpact(string variableName, double impact) {
      variableEvaluationImpacts[variableName] = impact;
    }

    public void SetVariableEvaluationImpact(int variableIndex, double impact) {
      variableEvaluationImpacts[dataset.GetVariableName(variableIndex)] = impact;
    }
  }
}
