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

using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Problems.DataAnalysis.Regression.Symbolic;
using HeuristicLab.Problems.DataAnalysis.Symbolic;
using System.Collections.Generic;
using HeuristicLab.Problems.DataAnalysis.Symbolic.Symbols;
using HeuristicLab.Problems.DataAnalysis;
using HeuristicLab.Problems.DataAnalysis.Evaluators;

namespace HeuristicLab.Problems.DataAnalysis.Regression.Symbolic.Analyzers {
  [Item("BestSymbolicRegressionSolutionAnalyzer", "An operator for analyzing the best solution of symbolic regression problems given in symbolic expression tree encoding.")]
  [StorableClass]
  public sealed class BestSymbolicRegressionSolutionAnalyzer : SingleSuccessorOperator, ISymbolicRegressionAnalyzer {
    private const string SymbolicExpressionTreeParameterName = "SymbolicExpressionTree";
    private const string SymbolicExpressionTreeInterpreterParameterName = "SymbolicExpressionTreeInterpreter";
    private const string ProblemDataParameterName = "ProblemData";
    private const string QualityParameterName = "Quality";
    private const string UpperEstimationLimitParameterName = "UpperEstimationLimit";
    private const string LowerEstimationLimitParameterName = "LowerEstimationLimit";
    private const string BestSolutionParameterName = "BestSolution";
    private const string BestSolutionQualityParameterName = "BestSolutionQuality";
    private const string ResultsParameterName = "Results";
    private const string BestSolutionResultName = "Best solution (on validiation set)";
    private const string BestSolutionInputvariableCountResultName = "Variables used by best solution";
    private const string BestSolutionTrainingRSquared = "Best solution R� (training)";
    private const string BestSolutionTestRSquared = "Best solution R� (test)";
    private const string BestSolutionTrainingMse = "Best solution mean squared error (training)";
    private const string BestSolutionTestMse = "Best solution mean squared error (test)";
    private const string BestSolutionTrainingRelativeError = "Best solution average relative error (training)";
    private const string BestSolutionTestRelativeError = "Best solution average relative error (test)";

    public ScopeTreeLookupParameter<SymbolicExpressionTree> SymbolicExpressionTreeParameter {
      get { return (ScopeTreeLookupParameter<SymbolicExpressionTree>)Parameters[SymbolicExpressionTreeParameterName]; }
    }
    public IValueLookupParameter<ISymbolicExpressionTreeInterpreter> SymbolicExpressionTreeInterpreterParameter {
      get { return (IValueLookupParameter<ISymbolicExpressionTreeInterpreter>)Parameters[SymbolicExpressionTreeInterpreterParameterName]; }
    }
    public IValueLookupParameter<DataAnalysisProblemData> ProblemDataParameter {
      get { return (IValueLookupParameter<DataAnalysisProblemData>)Parameters[ProblemDataParameterName]; }
    }
    public ScopeTreeLookupParameter<DoubleValue> QualityParameter {
      get { return (ScopeTreeLookupParameter<DoubleValue>)Parameters[QualityParameterName]; }
    }
    public IValueLookupParameter<DoubleValue> UpperEstimationLimitParameter {
      get { return (IValueLookupParameter<DoubleValue>)Parameters[UpperEstimationLimitParameterName]; }
    }
    public IValueLookupParameter<DoubleValue> LowerEstimationLimitParameter {
      get { return (IValueLookupParameter<DoubleValue>)Parameters[LowerEstimationLimitParameterName]; }
    }
    public ILookupParameter<SymbolicRegressionSolution> BestSolutionParameter {
      get { return (ILookupParameter<SymbolicRegressionSolution>)Parameters[BestSolutionParameterName]; }
    }
    public ILookupParameter<DoubleValue> BestSolutionQualityParameter {
      get { return (ILookupParameter<DoubleValue>)Parameters[BestSolutionQualityParameterName]; }
    }
    public ILookupParameter<ResultCollection> ResultsParameter {
      get { return (ILookupParameter<ResultCollection>)Parameters[ResultsParameterName]; }
    }

    public BestSymbolicRegressionSolutionAnalyzer()
      : base() {
      Parameters.Add(new ScopeTreeLookupParameter<SymbolicExpressionTree>(SymbolicExpressionTreeParameterName, "The symbolic expression trees to analyze."));
      Parameters.Add(new ValueLookupParameter<ISymbolicExpressionTreeInterpreter>(SymbolicExpressionTreeInterpreterParameterName, "The interpreter that should be used for the analysis of symbolic expression trees."));
      Parameters.Add(new ValueLookupParameter<DataAnalysisProblemData>(ProblemDataParameterName, "The problem data for which the symbolic expression tree is a solution."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>(UpperEstimationLimitParameterName, "The upper estimation limit that was set for the evaluation of the symbolic expression trees."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>(LowerEstimationLimitParameterName, "The lower estimation limit that was set for the evaluation of the symbolic expression trees."));
      Parameters.Add(new ScopeTreeLookupParameter<DoubleValue>(QualityParameterName, "The qualities of the symbolic regression trees which should be analyzed."));
      Parameters.Add(new LookupParameter<SymbolicRegressionSolution>(BestSolutionParameterName, "The best symbolic regression solution."));
      Parameters.Add(new LookupParameter<DoubleValue>(BestSolutionQualityParameterName, "The quality of the best symbolic regression solution."));
      Parameters.Add(new LookupParameter<ResultCollection>(ResultsParameterName, "The result collection where the best symbolic regression solution should be stored."));
    }

    public override IOperation Apply() {
      ItemArray<DoubleValue> qualities = QualityParameter.ActualValue;
      ResultCollection results = ResultsParameter.ActualValue;
      ISymbolicExpressionTreeInterpreter interpreter = SymbolicExpressionTreeInterpreterParameter.ActualValue;
      ItemArray<SymbolicExpressionTree> expressions = SymbolicExpressionTreeParameter.ActualValue;
      DataAnalysisProblemData problemData = ProblemDataParameter.ActualValue;
      DoubleValue upperEstimationLimit = UpperEstimationLimitParameter.ActualValue;
      DoubleValue lowerEstimationLimit = LowerEstimationLimitParameter.ActualValue;

      int i = qualities.Select((x, index) => new { index, x.Value }).OrderBy(x => x.Value).First().index;

      SymbolicRegressionSolution solution = BestSolutionParameter.ActualValue;
      if (solution == null) {
        var model = new SymbolicRegressionModel((ISymbolicExpressionTreeInterpreter)interpreter.Clone(), expressions[i], GetInputVariables(expressions[i]));
        solution = new SymbolicRegressionSolution(problemData, model, lowerEstimationLimit.Value, upperEstimationLimit.Value);
        BestSolutionParameter.ActualValue = solution;
        BestSolutionQualityParameter.ActualValue = qualities[i];
        results.Add(new Result(BestSolutionResultName, solution));
        results.Add(new Result(BestSolutionInputvariableCountResultName, new IntValue(model.InputVariables.Count())));
        #region calculate R2,MSE,Rel Error
        double[] trainingValues = problemData.Dataset.GetVariableValues(
          problemData.TargetVariable.Value,
          problemData.TrainingSamplesStart.Value,
          problemData.TrainingSamplesEnd.Value);
        double[] testValues = problemData.Dataset.GetVariableValues(
          problemData.TargetVariable.Value,
          problemData.TestSamplesStart.Value,
          problemData.TestSamplesEnd.Value);
        double trainingR2 = SimpleRSquaredEvaluator.Calculate(trainingValues, solution.EstimatedTrainingValues);
        double testR2 = SimpleRSquaredEvaluator.Calculate(testValues, solution.EstimatedTestValues);
        double trainingMse = SimpleMSEEvaluator.Calculate(trainingValues, solution.EstimatedTrainingValues);
        double testMse = SimpleMSEEvaluator.Calculate(testValues, solution.EstimatedTestValues);
        double trainingRelError = SimpleMeanAbsolutePercentageErrorEvaluator.Calculate(trainingValues, solution.EstimatedTrainingValues);
        double testRelError = SimpleMeanAbsolutePercentageErrorEvaluator.Calculate(testValues, solution.EstimatedTestValues);
        results.Add(new Result(BestSolutionTrainingRSquared, new DoubleValue(trainingR2)));
        results.Add(new Result(BestSolutionTestRSquared, new DoubleValue(testR2)));
        results.Add(new Result(BestSolutionTrainingMse, new DoubleValue(trainingMse)));
        results.Add(new Result(BestSolutionTestMse, new DoubleValue(testMse)));
        results.Add(new Result(BestSolutionTrainingRelativeError, new DoubleValue(trainingRelError)));
        results.Add(new Result(BestSolutionTestRelativeError, new DoubleValue(testRelError)));
        #endregion
      } else {
        if (BestSolutionQualityParameter.ActualValue.Value > qualities[i].Value) {
          var model = new SymbolicRegressionModel((ISymbolicExpressionTreeInterpreter)interpreter.Clone(), expressions[i], GetInputVariables(expressions[i]));
          solution = new SymbolicRegressionSolution(problemData, model, lowerEstimationLimit.Value, upperEstimationLimit.Value);
          BestSolutionParameter.ActualValue = solution;
          BestSolutionQualityParameter.ActualValue = qualities[i];
          results[BestSolutionResultName].Value = solution;
          results[BestSolutionInputvariableCountResultName].Value = new IntValue(model.InputVariables.Count());
          #region update R2,MSE, Rel Error
          double[] trainingValues = problemData.Dataset.GetVariableValues(
            problemData.TargetVariable.Value,
            problemData.TrainingSamplesStart.Value,
            problemData.TrainingSamplesEnd.Value);
          double[] testValues = problemData.Dataset.GetVariableValues(
            problemData.TargetVariable.Value,
            problemData.TestSamplesStart.Value,
            problemData.TestSamplesEnd.Value);
          double trainingR2 = SimpleRSquaredEvaluator.Calculate(trainingValues, solution.EstimatedTrainingValues);
          double testR2 = SimpleRSquaredEvaluator.Calculate(testValues, solution.EstimatedTestValues);
          double trainingMse = SimpleMSEEvaluator.Calculate(trainingValues, solution.EstimatedTrainingValues);
          double testMse = SimpleMSEEvaluator.Calculate(testValues, solution.EstimatedTestValues);
          double trainingRelError = SimpleMeanAbsolutePercentageErrorEvaluator.Calculate(trainingValues, solution.EstimatedTrainingValues);
          double testRelError = SimpleMeanAbsolutePercentageErrorEvaluator.Calculate(testValues, solution.EstimatedTestValues);
          results[BestSolutionTrainingRSquared].Value = new DoubleValue(trainingR2);
          results[BestSolutionTestRSquared].Value = new DoubleValue(testR2);
          results[BestSolutionTrainingMse].Value = new DoubleValue(trainingMse);
          results[BestSolutionTestMse].Value = new DoubleValue(testMse);
          results[BestSolutionTrainingRelativeError].Value = new DoubleValue(trainingRelError);
          results[BestSolutionTestRelativeError].Value = new DoubleValue(testRelError);
          #endregion
        }
      }

      return base.Apply();
    }

    private IEnumerable<string> GetInputVariables(SymbolicExpressionTree tree) {
      return (from varNode in tree.IterateNodesPrefix().OfType<VariableTreeNode>()
              select varNode.VariableName).Distinct();
    }
  }
}
