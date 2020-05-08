﻿
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
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Analysis;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Optimization;
using HeuristicLab.Optimization.Operators;

namespace HeuristicLab.Encodings.BinaryVectorEncoding {
  [StorableType("2F6FEB34-BD19-47AF-9484-7F48565C0C43")]
  public abstract class BinaryVectorProblem : SingleObjectiveProblem<BinaryVectorEncoding, BinaryVector> {
    [Storable] protected IResultParameter<ISingleObjectiveSolutionContext<BinaryVector>> BestResultParameter { get; private set; }
    public IResultDefinition<ISingleObjectiveSolutionContext<BinaryVector>> BestResult { get { return BestResultParameter; } }

    public int Length {
      get { return Encoding.Length; }
      set { Encoding.Length = value; }
    }

    [StorableConstructor]
    protected BinaryVectorProblem(StorableConstructorFlag _) : base(_) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEventHandlers();
    }

    protected BinaryVectorProblem(BinaryVectorProblem original, Cloner cloner)
      : base(original, cloner) {
      BestResultParameter = cloner.Clone(original.BestResultParameter);
      RegisterEventHandlers();
    }

    protected BinaryVectorProblem() : this(new BinaryVectorEncoding() { Length = 10 }) { }
    protected BinaryVectorProblem(BinaryVectorEncoding encoding) : base(encoding) {
      EncodingParameter.ReadOnly = true;
      Parameters.Add(BestResultParameter = new ResultParameter<ISingleObjectiveSolutionContext<BinaryVector>>("Best Solution", "The best solution."));

      Operators.Add(new HammingSimilarityCalculator());
      Operators.Add(new QualitySimilarityCalculator());
      Operators.Add(new PopulationSimilarityAnalyzer(Operators.OfType<ISolutionSimilarityCalculator>()));

      Parameterize();
      RegisterEventHandlers();
    }

    public override void Analyze(ISingleObjectiveSolutionContext<BinaryVector>[] solutionContexts, ResultCollection results, IRandom random) {
      var best = GetBest(solutionContexts);
      var currentBest = BestResultParameter.ActualValue;
      if (currentBest == null || IsBetter(best.EvaluationResult.Quality, currentBest.EvaluationResult.Quality))
        BestResultParameter.ActualValue = (ISingleObjectiveSolutionContext<BinaryVector>)best.Clone();
    }

    protected override void OnEncodingChanged() {
      base.OnEncodingChanged();
      Parameterize();
    }

    private void Parameterize() {
      foreach (var similarityCalculator in Operators.OfType<ISolutionSimilarityCalculator>()) {
        similarityCalculator.SolutionVariableName = Encoding.Name;
        similarityCalculator.QualityVariableName = Evaluator.QualityParameter.ActualName;
      }
    }

    private void RegisterEventHandlers() {
      Encoding.LengthParameter.Value.ValueChanged += LengthParameter_ValueChanged;
    }

    protected virtual void LengthParameter_ValueChanged(object sender, EventArgs e) { }
  }
}