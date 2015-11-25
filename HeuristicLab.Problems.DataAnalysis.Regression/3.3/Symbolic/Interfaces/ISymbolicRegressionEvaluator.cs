#region License Information
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

using System.Collections.Generic;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Problems.DataAnalysis.Symbolic;

namespace HeuristicLab.Problems.DataAnalysis.Regression.Symbolic {
  public interface ISymbolicRegressionEvaluator : ISingleObjectiveEvaluator {
    bool Maximization { get; }

    ILookupParameter<ISymbolicExpressionTreeInterpreter> SymbolicExpressionTreeInterpreterParameter { get; }
    ILookupParameter<SymbolicExpressionTree> SymbolicExpressionTreeParameter { get; }
    ILookupParameter<DataAnalysisProblemData> RegressionProblemDataParameter { get; }
    IValueLookupParameter<IntValue> SamplesStartParameter { get; }
    IValueLookupParameter<IntValue> SamplesEndParameter { get; }
    IValueLookupParameter<DoubleValue> UpperEstimationLimitParameter { get; }
    IValueLookupParameter<DoubleValue> LowerEstimationLimitParameter { get; }


    double Evaluate(ISymbolicExpressionTreeInterpreter interpreter, SymbolicExpressionTree tree,
          double lowerEstimationLimit, double upperEstimationLimit,
          Dataset dataset, string targetVariable, IEnumerable<int> rows);
  }
}