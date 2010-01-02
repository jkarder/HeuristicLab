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

using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.DataAnalysis;
using HeuristicLab.GP.Interfaces;
using HeuristicLab.Modeling;
using System.Linq;

namespace HeuristicLab.GP.StructureIdentification {
  public class SimpleEvaluator : GPEvaluatorBase {
    public SimpleEvaluator()
      : base() {
      AddVariableInfo(new VariableInfo("Values", "Target vs. predicted values", typeof(DoubleMatrixData), VariableKind.New | VariableKind.Out));
    }

    public override void Evaluate(IScope scope, IFunctionTree tree, ITreeEvaluator evaluator, Dataset dataset, int targetVariable, int start, int end) {
      DoubleMatrixData values = GetVariableValue<DoubleMatrixData>("Values", scope, false, false);
      if (values == null) {
        values = new DoubleMatrixData();
        IVariableInfo info = GetVariableInfo("Values");
        if (info.Local)
          AddVariable(new HeuristicLab.Core.Variable(info.ActualName, values));
        else
          scope.AddVariable(new HeuristicLab.Core.Variable(scope.TranslateName(info.FormalName), values));
      }

      double[,] v = Matrix<double>.Create(
        dataset.GetVariableValues(targetVariable, start, end),
        evaluator.Evaluate(dataset, tree, Enumerable.Range(start, end - start)).ToArray());
      values.Data = v;
    }
  }
}
