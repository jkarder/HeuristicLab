﻿#region License Information

/* HeuristicLab
 * Copyright (C) 2002-2019 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Analysis;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Optimization;

namespace HeuristicLab.Encodings.IntegerVectorEncoding {
  [StorableType("11916b0f-4c34-4ece-acae-e28d11211b43")]
  public abstract class IntegerVectorMultiObjectiveProblem : MultiObjectiveProblem<IntegerVectorEncoding, IntegerVector> {
    [Storable] protected IResultParameter<ParetoFrontScatterPlot<IntegerVector>> BestResultParameter { get; private set; }
    public IResultDefinition<ParetoFrontScatterPlot<IntegerVector>> BestResult { get { return BestResultParameter; } }

    public int Length {
      get { return Encoding.Length; }
      set { Encoding.Length = value; }
    }

    [StorableConstructor]
    protected IntegerVectorMultiObjectiveProblem(StorableConstructorFlag _) : base(_) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEventHandlers();
    }

    protected IntegerVectorMultiObjectiveProblem(IntegerVectorMultiObjectiveProblem original, Cloner cloner)
      : base(original, cloner) {
      BestResultParameter = cloner.Clone(original.BestResultParameter);
      RegisterEventHandlers();
    }

    protected IntegerVectorMultiObjectiveProblem() : this(new IntegerVectorEncoding() { Length = 10 }) { }
    protected IntegerVectorMultiObjectiveProblem(IntegerVectorEncoding encoding) : base(encoding) {
      EncodingParameter.ReadOnly = true;
      Parameters.Add(BestResultParameter = new ResultParameter<ParetoFrontScatterPlot<IntegerVector>>("Best Pareto Front", "The best Pareto front found."));

      Operators.Add(new HammingSimilarityCalculator());
      Operators.Add(new PopulationSimilarityAnalyzer(Operators.OfType<ISolutionSimilarityCalculator>()));

      Parameterize();
      RegisterEventHandlers();
    }

    public override void Analyze(IntegerVector[] individuals, double[][] qualities, ResultCollection results, IRandom random) {
      base.Analyze(individuals, qualities, results, random);

      var fronts = DominationCalculator.CalculateAllParetoFrontsIndices(individuals, qualities, Maximization);
      var plot = new ParetoFrontScatterPlot<IntegerVector>(fronts, individuals, qualities, Objectives, BestKnownFront);

      BestResultParameter.ActualValue = plot;
    }

    protected override void OnEncodingChanged() {
      base.OnEncodingChanged();
      Parameterize();
    }

    private void Parameterize() {
      foreach (var similarityCalculator in Operators.OfType<ISolutionSimilarityCalculator>()) {
        similarityCalculator.SolutionVariableName = Encoding.Name;
        similarityCalculator.QualityVariableName = Evaluator.QualitiesParameter.ActualName;
      }
    }

    private void RegisterEventHandlers() {
      Encoding.LengthParameter.Value.ValueChanged += LengthParameter_ValueChanged;
    }

    protected virtual void LengthParameter_ValueChanged(object sender, EventArgs e) { }
  }
}