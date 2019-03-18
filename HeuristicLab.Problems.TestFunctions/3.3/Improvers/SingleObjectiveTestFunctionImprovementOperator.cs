﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Operators;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.TestFunctions {
  /// <summary>
  /// An operator that improves test functions solutions.
  /// </summary>
  /// <remarks>
  /// It is implemented as described in Laguna, M. and Martí, R. (2003). Scatter Search: Methodology and Implementations in C. Operations Research/Computer Science Interfaces Series, Vol. 24. Springer.<br />
  /// The operator uses an implementation of the Nelder-Mead method with adaptive parameters as described in Gao, F. and Han, L. (2010). Implementing the Nelder-Mead simplex algorithm with adaptive parameters. Computational Optimization and Applications, Vol. 51. Springer. and conducts relection, expansion, contraction and reduction on the test functions solution.
  /// </remarks>
  [Item("SingleObjectiveTestFunctionImprovementOperator", "An operator that improves test functions solutions. It is implemented as described in Laguna, M. and Martí, R. (2003). Scatter Search: Methodology and Implementations in C. Operations Research/Computer Science Interfaces Series, Vol. 24. Springer.")]
  [StorableClass]
  public sealed class SingleObjectiveTestFunctionImprovementOperator : SingleSuccessorOperator, ISingleObjectiveTestFunctionImprovementOperator {
    #region Parameter properties
    public IValueParameter<DoubleValue> AlphaParameter {
      get { return (IValueParameter<DoubleValue>)Parameters["Alpha"]; }
    }
    public IValueParameter<DoubleValue> BetaParameter {
      get { return (IValueParameter<DoubleValue>)Parameters["Beta"]; }
    }
    public IValueLookupParameter<DoubleMatrix> BoundsParameter {
      get { return (IValueLookupParameter<DoubleMatrix>)Parameters["Bounds"]; }
    }
    public IValueParameter<DoubleValue> DeltaParameter {
      get { return (IValueParameter<DoubleValue>)Parameters["Delta"]; }
    }
    public IValueLookupParameter<ISingleObjectiveTestFunction> TestFunctionParameter {
      get { return (IValueLookupParameter<ISingleObjectiveTestFunction>)Parameters["TestFunction"]; }
    }
    public IValueParameter<DoubleValue> GammaParameter {
      get { return (IValueParameter<DoubleValue>)Parameters["Gamma"]; }
    }
    public IValueLookupParameter<IntValue> ImprovementAttemptsParameter {
      get { return (IValueLookupParameter<IntValue>)Parameters["ImprovementAttempts"]; }
    }
    public IValueLookupParameter<IItem> SolutionParameter {
      get { return (IValueLookupParameter<IItem>)Parameters["Solution"]; }
    }
    #endregion

    #region Properties
    private DoubleValue Alpha {
      get { return AlphaParameter.Value; }
    }
    private DoubleValue Beta {
      get { return BetaParameter.Value; }
    }
    private DoubleValue Delta {
      get { return DeltaParameter.Value; }
    }
    private DoubleValue Gamma {
      get { return GammaParameter.Value; }
    }
    #endregion

    [StorableConstructor]
    private SingleObjectiveTestFunctionImprovementOperator(bool deserializing) : base(deserializing) { }
    private SingleObjectiveTestFunctionImprovementOperator(SingleObjectiveTestFunctionImprovementOperator original, Cloner cloner) : base(original, cloner) { }
    public SingleObjectiveTestFunctionImprovementOperator()
      : base() {
      #region Create parameters
      Parameters.Add(new ValueParameter<DoubleValue>("Alpha", new DoubleValue(1.0)));
      Parameters.Add(new ValueParameter<DoubleValue>("Beta", new DoubleValue(2.0)));
      Parameters.Add(new ValueParameter<DoubleValue>("Delta", new DoubleValue(0.5)));
      Parameters.Add(new ValueParameter<DoubleValue>("Gamma", new DoubleValue(0.5)));
      Parameters.Add(new ValueLookupParameter<ISingleObjectiveTestFunction>("TestFunction", "The operator used to evaluate solutions."));
      Parameters.Add(new ValueLookupParameter<DoubleMatrix>("Bounds", "The lower and upper bounds in each dimension."));
      Parameters.Add(new ValueLookupParameter<IntValue>("ImprovementAttempts", "The number of improvement attempts the operator should perform.", new IntValue(100)));
      Parameters.Add(new ValueLookupParameter<IItem>("Solution", "The solution to be improved. This parameter is used for name translation only.")); // TODO: Problematic, this cannot be wired! IImprovementOperators need to be generic
      #endregion
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SingleObjectiveTestFunctionImprovementOperator(this, cloner);
    }

    public override IOperation Apply() {
      RealVector bestSol = ExecutionContext.Scope.Variables[SolutionParameter.ActualName].Value as RealVector;
      if (bestSol == null)
        throw new ArgumentException("Cannot improve solution because it has the wrong type.");

      var bounds = BoundsParameter.ActualValue;
      var function = TestFunctionParameter.ActualValue;
      var maxIterations = ImprovementAttemptsParameter.ActualValue.Value;

      double bestSolQuality = function.Evaluate(bestSol);

      // create perturbed solutions
      RealVector[] simplex = new RealVector[bestSol.Length];
      for (int i = 0; i < simplex.Length; i++) {
        simplex[i] = bestSol.Clone() as RealVector;
        simplex[i][i] += 0.1 * (bounds[0, 1] - bounds[0, 0]);
        if (simplex[i][i] > bounds[0, 1]) simplex[i][i] = bounds[0, 1];
        if (simplex[i][i] < bounds[0, 0]) simplex[i][i] = bounds[0, 0];
      }

      // improve solutions
      for (int i = 0; i < maxIterations; i++) {
        // order according to their objective function value
        Array.Sort(simplex, (x, y) => function.Evaluate(x).CompareTo(function.Evaluate(y)));

        // calculate centroid
        RealVector centroid = new RealVector(bestSol.Length);
        foreach (var vector in simplex)
          for (int j = 0; j < centroid.Length; j++)
            centroid[j] += vector[j];
        for (int j = 0; j < centroid.Length; j++)
          centroid[j] /= simplex.Length;

        // reflection
        RealVector reflectionPoint = new RealVector(bestSol.Length);
        for (int j = 0; j < reflectionPoint.Length; j++)
          reflectionPoint[j] = centroid[j] + Alpha.Value * (centroid[j] - simplex[simplex.Length - 1][j]);
        double reflectionPointQuality = function.Evaluate(reflectionPoint);
        if (function.Evaluate(simplex[0]) <= reflectionPointQuality
            && reflectionPointQuality < function.Evaluate(simplex[simplex.Length - 2]))
          simplex[simplex.Length - 1] = reflectionPoint;

        // expansion
        if (reflectionPointQuality < function.Evaluate(simplex[0])) {
          RealVector expansionPoint = new RealVector(bestSol.Length);
          for (int j = 0; j < expansionPoint.Length; j++)
            expansionPoint[j] = centroid[j] + Beta.Value * (reflectionPoint[j] - centroid[j]);
          simplex[simplex.Length - 1] = function.Evaluate(expansionPoint) < reflectionPointQuality ? expansionPoint : reflectionPoint;
        }

        // contraction
        if (function.Evaluate(simplex[simplex.Length - 2]) <= reflectionPointQuality
            && reflectionPointQuality < function.Evaluate(simplex[simplex.Length - 1])) {
          RealVector outsideContractionPoint = new RealVector(bestSol.Length);
          for (int j = 0; j < outsideContractionPoint.Length; j++)
            outsideContractionPoint[j] = centroid[j] + Gamma.Value * (reflectionPoint[j] - centroid[j]);
          if (function.Evaluate(outsideContractionPoint) <= reflectionPointQuality) {
            simplex[simplex.Length - 1] = outsideContractionPoint;
            if (function.Evaluate(reflectionPoint) >= function.Evaluate(simplex[simplex.Length - 1])) {
              RealVector insideContractionPoint = new RealVector(bestSol.Length);
              for (int j = 0; j < insideContractionPoint.Length; j++)
                insideContractionPoint[j] = centroid[j] - Gamma.Value * (reflectionPoint[j] - centroid[j]);
              if (function.Evaluate(insideContractionPoint) < function.Evaluate(simplex[simplex.Length - 1])) simplex[simplex.Length - 1] = insideContractionPoint;
            }
          }
        }

        // reduction
        for (int j = 1; j < simplex.Length; j++)
          for (int k = 0; k < simplex[j].Length; k++)
            simplex[j][k] = simplex[0][k] + Delta.Value * (simplex[j][k] - simplex[0][k]);
      }

      for (int i = 0; i < simplex[0].Length; i++) {
        if (simplex[0][i] > bounds[0, 1]) simplex[0][i] = bounds[0, 1];
        if (simplex[0][i] < bounds[0, 0]) simplex[0][i] = bounds[0, 0];
      }

      ExecutionContext.Scope.Variables[SolutionParameter.ActualName].Value = simplex[0];
      ExecutionContext.Scope.Variables.Add(new Variable("LocalEvaluatedSolutions", new IntValue(maxIterations)));

      return base.Apply();
    }
  }
}
