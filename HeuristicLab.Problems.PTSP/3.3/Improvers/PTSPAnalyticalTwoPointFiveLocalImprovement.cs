﻿#region License Information
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
using System.Threading;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.PermutationEncoding;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;

namespace HeuristicLab.Problems.PTSP {
  /// <summary>
  /// An operator that improves probabilistic traveling salesman solutions.
  /// </summary>
  /// <remarks>
  /// The operator tries to improve the probabilistic traveling salesman solution by swapping two randomly chosen edges for a certain number of times.
  /// </remarks>
  [Item("PTSP Analytical 2.5 Local Improvement", "An operator that improves probabilistic traveling salesman solutions. The operator tries to improve the probabilistic traveling salesman solution by swapping two randomly chosen edges for a certain number of times.")]
  [StorableType("6e07195e-0da7-45ea-9385-0c66594127db")]
  public sealed class PTSPAnalyticalTwoPointFiveLocalImprovement : SingleSuccessorOperator, IAnalyticalPTSPOperator, ILocalImprovementOperator {

    public ILookupParameter<IntValue> LocalIterationsParameter {
      get { return (ILookupParameter<IntValue>)Parameters["LocalIterations"]; }
    }

    public IValueLookupParameter<IntValue> MaximumIterationsParameter {
      get { return (IValueLookupParameter<IntValue>)Parameters["MaximumIterations"]; }
    }

    public ILookupParameter<IntValue> EvaluatedSolutionsParameter {
      get { return (ILookupParameter<IntValue>)Parameters["EvaluatedSolutions"]; }
    }

    public ILookupParameter<ResultCollection> ResultsParameter {
      get { return (ILookupParameter<ResultCollection>)Parameters["Results"]; }
    }

    public ILookupParameter<Permutation> PermutationParameter {
      get { return (ILookupParameter<Permutation>)Parameters["Permutation"]; }
    }

    public ILookupParameter<DoubleValue> QualityParameter {
      get { return (ILookupParameter<DoubleValue>)Parameters["Quality"]; }
    }

    public ILookupParameter<BoolValue> MaximizationParameter {
      get { return (ILookupParameter<BoolValue>)Parameters["Maximization"]; }
    }

    public ILookupParameter<IProbabilisticTSPData> ProbabilisticTSPDataParameter {
      get { return (ILookupParameter<IProbabilisticTSPData>)Parameters["PTSP Data"]; }
    }

    [StorableConstructor]
    private PTSPAnalyticalTwoPointFiveLocalImprovement(StorableConstructorFlag _) : base(_) { }
    private PTSPAnalyticalTwoPointFiveLocalImprovement(PTSPAnalyticalTwoPointFiveLocalImprovement original, Cloner cloner) : base(original, cloner) { }
    public PTSPAnalyticalTwoPointFiveLocalImprovement()
      : base() {
      Parameters.Add(new LookupParameter<Permutation>("Permutation", "The solution as permutation."));
      Parameters.Add(new LookupParameter<IntValue>("LocalIterations", "The number of iterations that have already been performed."));
      Parameters.Add(new ValueLookupParameter<IntValue>("MaximumIterations", "The maximum amount of iterations that should be performed (note that this operator will abort earlier when a local optimum is reached).", new IntValue(10000)));
      Parameters.Add(new LookupParameter<IntValue>("EvaluatedSolutions", "The amount of evaluated solutions (here a move is counted only as 4/n evaluated solutions with n being the length of the permutation)."));
      Parameters.Add(new LookupParameter<ResultCollection>("Results", "The collection where to store results."));
      Parameters.Add(new LookupParameter<DoubleValue>("Quality", "The quality value of the assignment."));
      Parameters.Add(new LookupParameter<BoolValue>("Maximization", "True if the problem should be maximized or minimized."));
      Parameters.Add(new LookupParameter<IProbabilisticTSPData>("PTSP Data", "The main parameters of the p-TSP."));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new PTSPAnalyticalTwoPointFiveLocalImprovement(this, cloner);
    }

    public static void Improve(Permutation assignment, IProbabilisticTSPData data, DoubleValue quality, IntValue localIterations, IntValue evaluatedSolutions, bool maximization, int maxIterations, CancellationToken cancellation) {
      for (var i = localIterations.Value; i < maxIterations; i++) {
        TwoPointFiveMove bestMove = null;
        var bestQuality = quality.Value; // we have to make an improvement, so current quality is the baseline
        var evaluations = 0.0;
        foreach (var move in ExhaustiveTwoPointFiveMoveGenerator.Generate(assignment)) {
          var moveQuality = PTSPAnalyticalTwoPointFiveMoveEvaluator.EvaluateMove(assignment, move, data);
          evaluations++;
          if (maximization && moveQuality > bestQuality
            || !maximization && moveQuality < bestQuality) {
            bestQuality = moveQuality;
            bestMove = move;
          }
        }
        evaluatedSolutions.Value += (int)Math.Ceiling(evaluations);
        if (bestMove == null) break;
        TwoPointFiveMoveMaker.Apply(assignment, bestMove);
        quality.Value = bestQuality;
        localIterations.Value++;
        cancellation.ThrowIfCancellationRequested();
      }
    }

    public override IOperation Apply() {
      var maxIterations = MaximumIterationsParameter.ActualValue.Value;
      var assignment = PermutationParameter.ActualValue;
      var maximization = MaximizationParameter.ActualValue.Value;
      var quality = QualityParameter.ActualValue;
      var localIterations = LocalIterationsParameter.ActualValue;
      var evaluations = EvaluatedSolutionsParameter.ActualValue;
      var data = ProbabilisticTSPDataParameter.ActualValue;
      if (localIterations == null) {
        localIterations = new IntValue(0);
        LocalIterationsParameter.ActualValue = localIterations;
      }

      Improve(assignment, data, quality, localIterations, evaluations, maximization, maxIterations, CancellationToken);

      localIterations.Value = 0;
      return base.Apply();
    }
  }
}
