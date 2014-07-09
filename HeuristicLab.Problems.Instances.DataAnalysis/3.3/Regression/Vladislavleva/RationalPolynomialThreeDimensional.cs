﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2014 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

namespace HeuristicLab.Problems.Instances.DataAnalysis {
  public class RationalPolynomialThreeDimensional : ArtificialRegressionDataDescriptor {

    public override string Name { get { return "Vladislavleva-5 F5(X1, X2, X3) = 30 * ((X1 - 1) * (X3 -1)) / (X2² * (X1 - 10))"; } }
    public override string Description {
      get {
        return "Paper: Order of Nonlinearity as a Complexity Measure for Models Generated by Symbolic Regression via Pareto Genetic Programming " + Environment.NewLine
        + "Authors: Ekaterina J. Vladislavleva, Member, IEEE, Guido F. Smits, Member, IEEE, and Dick den Hertog" + Environment.NewLine
        + "Function: F5(X1, X2, X3) = 30 * ((X1 - 1) * (X3 -1)) / (X2² * (X1 - 10))" + Environment.NewLine
        + "Training Data: 300 points X1, X3 = Rand(0.05, 2), X2 = Rand(1, 2)" + Environment.NewLine
        + "Test Data: (14*12*14) points X1, X3 = (-0.05:0.15:2.05), X2 = (0.95:0.1:2.05)" + Environment.NewLine
        + "Function Set: +, -, *, /, square, x^eps, x + eps, x * eps";
      }
    }
    protected override string TargetVariable { get { return "Y"; } }
    protected override string[] VariableNames { get { return new string[] { "X1", "X2", "X3", "Y" }; } }
    protected override string[] AllowedInputVariables { get { return new string[] { "X1", "X2", "X3" }; } }
    protected override int TrainingPartitionStart { get { return 0; } }
    protected override int TrainingPartitionEnd { get { return 300; } }
    protected override int TestPartitionStart { get { return 300; } }
    protected override int TestPartitionEnd { get { return 300 + (15*12*15); } }

    protected override List<List<double>> GenerateValues() {
      List<List<double>> data = new List<List<double>>();

      int n = 300;
      data.Add(ValueGenerator.GenerateUniformDistributedValues(n, 0.05, 2).ToList());
      data.Add(ValueGenerator.GenerateUniformDistributedValues(n, 1, 2).ToList());
      data.Add(ValueGenerator.GenerateUniformDistributedValues(n, 0.05, 2).ToList());

      List<List<double>> testData = new List<List<double>>() { 
        ValueGenerator.GenerateSteps(-0.05, 2.05, 0.15).ToList(), 
        ValueGenerator.GenerateSteps( 0.95, 2.05, 0.1).ToList(),
        ValueGenerator.GenerateSteps(-0.05, 2.05, 0.15).ToList()
      };

      var combinations = ValueGenerator.GenerateAllCombinationsOfValuesInLists(testData).ToList<IEnumerable<double>>();

      for (int i = 0; i < AllowedInputVariables.Count(); i++) {
        data[i].AddRange(combinations[i]);
      }

      double x1, x2, x3;
      List<double> results = new List<double>();
      for (int i = 0; i < data[0].Count; i++) {
        x1 = data[0][i];
        x2 = data[1][i];
        x3 = data[2][i];
        results.Add(30 * ((x1 - 1) * (x3 - 1)) / (Math.Pow(x2, 2) * (x1 - 10)));
      }
      data.Add(results);

      return data;
    }
  }
}
