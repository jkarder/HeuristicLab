﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2016 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Runtime.CompilerServices;
using System.Threading;
using HeuristicLab.Algorithms.MemPR.Interfaces;
using HeuristicLab.Algorithms.MemPR.Util;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.PermutationEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.Random;

namespace HeuristicLab.Algorithms.MemPR.Permutation {
  [Item("MemPR (permutation)", "MemPR implementation for permutations.")]
  [StorableClass]
  [Creatable(CreatableAttribute.Categories.PopulationBasedAlgorithms, Priority = 999)]
  public class PermutationMemPR : MemPRAlgorithm<ISingleObjectiveHeuristicOptimizationProblem, Encodings.PermutationEncoding.Permutation, PermutationMemPRPopulationContext, PermutationMemPRSolutionContext> {
#if DEBUG
    private const bool VALIDATE = true;
#else
    private const bool VALIDATE = false;
#endif

    [StorableConstructor]
    protected PermutationMemPR(bool deserializing) : base(deserializing) { }
    protected PermutationMemPR(PermutationMemPR original, Cloner cloner) : base(original, cloner) { }
    public PermutationMemPR() {
      foreach (var trainer in ApplicationManager.Manager.GetInstances<ISolutionModelTrainer<PermutationMemPRPopulationContext>>())
        SolutionModelTrainerParameter.ValidValues.Add(trainer);

      if (SolutionModelTrainerParameter.ValidValues.Count > 0) {
        var unbiased = SolutionModelTrainerParameter.ValidValues.FirstOrDefault(x => !x.Bias);
        if (unbiased != null) SolutionModelTrainerParameter.Value = unbiased;
      }

      foreach (var localSearch in ApplicationManager.Manager.GetInstances<ILocalSearch<PermutationMemPRSolutionContext>>()) {
        LocalSearchParameter.ValidValues.Add(localSearch);
      }
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new PermutationMemPR(this, cloner);
    }

    protected override bool Eq(Encodings.PermutationEncoding.Permutation a, Encodings.PermutationEncoding.Permutation b) {
      return new PermutationEqualityComparer().Equals(a, b);
    }

    protected override double Dist(ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> a, ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> b) {
      return Dist(a.Solution, b.Solution);
    }

    private static double Dist(Encodings.PermutationEncoding.Permutation a, Encodings.PermutationEncoding.Permutation b) {
      return 1.0 - HammingSimilarityCalculator.CalculateSimilarity(a, b);
    }

    protected override ISolutionSubspace<Encodings.PermutationEncoding.Permutation> CalculateSubspace(IEnumerable<Encodings.PermutationEncoding.Permutation> solutions, bool inverse = false) {
      var solutionsIter = solutions.GetEnumerator();
      if (!solutionsIter.MoveNext()) throw new ArgumentException("Cannot calculate sub-space when no solutions are given.");
      var first = solutionsIter.Current;

      var N = solutionsIter.Current.Length;
      var type = solutionsIter.Current.PermutationType;
      var subspace = new bool[N, type == PermutationTypes.Absolute ? 1 : N];
      switch (type) {
        case PermutationTypes.Absolute: {
            if (inverse) {
              for (var i = 0; i < subspace.GetLength(0); i++)
                subspace[i, 0] = true;
            }
            while (solutionsIter.MoveNext()) {
              var s = solutionsIter.Current;
              for (var i = 0; i < s.Length; i++) {
                if (first[i] != s[i]) subspace[i, 0] = !inverse;
              }
            }
          }
          break;
        case PermutationTypes.RelativeDirected: {
            if (inverse) {
              for (var i = 0; i < subspace.GetLength(0); i++)
                for (var j = 0; j < subspace.GetLength(1); j++)
                  subspace[i, j] = true;
            }
            var placedFirst = new int[first.Length];
            for (var i = 0; i < first.Length; i++) {
              placedFirst[first[i]] = i;
            }
            while (solutionsIter.MoveNext()) {
              var s = solutionsIter.Current;
              for (var i = 0; i < s.Length; i++) {
                if (placedFirst[s[i]] - placedFirst[s.GetCircular(i + 1)] != -1)
                  subspace[i, 0] = !inverse;
              }
            }
          }
          break;
        case PermutationTypes.RelativeUndirected: {
            if (inverse) {
              for (var i = 0; i < subspace.GetLength(0); i++)
                for (var j = 0; j < subspace.GetLength(1); j++)
                  subspace[i, j] = true;
            }
            var placedFirst = new int[first.Length];
            for (var i = 0; i < first.Length; i++) {
              placedFirst[first[i]] = i;
            }
            while (solutionsIter.MoveNext()) {
              var s = solutionsIter.Current;
              for (var i = 0; i < s.Length; i++) {
                if (Math.Abs(placedFirst[s[i]] - placedFirst[s.GetCircular(i + 1)]) != 1)
                  subspace[i, 0] = !inverse;
              }
            }
          }
          break;
        default:
          throw new ArgumentException(string.Format("Unknown permutation type {0}", type));
      }
      return new PermutationSolutionSubspace(subspace);
    }

    protected override void AdaptiveWalk(ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> scope, int maxEvals, CancellationToken token, ISolutionSubspace<Encodings.PermutationEncoding.Permutation> subspace = null) {
      var quality = scope.Fitness;
      try {
        TabuWalk(Context.Random, scope.Solution, Context.Evaluate, token, ref quality, maxEvals, subspace != null ? ((PermutationSolutionSubspace)subspace).Subspace : null);
      } finally {
        scope.Fitness = quality;
      }
    }

    public void TabuWalk(IRandom random, Encodings.PermutationEncoding.Permutation perm, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, ref double quality, int maxEvals = int.MaxValue, bool[,] subspace = null) {
      switch (perm.PermutationType) {
        case PermutationTypes.Absolute:
          TabuWalkSwap(random, perm, eval, token, ref quality, maxEvals, subspace);
          break;
        case PermutationTypes.RelativeDirected:
          TabuWalkShift(random, perm, eval, token, ref quality, maxEvals, subspace);
          break;
        case PermutationTypes.RelativeUndirected:
          TabuWalkOpt(random, perm, eval, token, ref quality, maxEvals, subspace);
          break;
        default: throw new ArgumentException(string.Format("Permutation type {0} is not known", perm.PermutationType));
      }
      if (VALIDATE && !perm.Validate()) throw new ArgumentException("TabuWalk produced invalid child");
    }

    public int TabuWalkSwap(IRandom random, Encodings.PermutationEncoding.Permutation perm, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, ref double quality, int maxEvals = int.MaxValue, bool[,] subspace = null) {
      var evaluations = 0;
      var maximization = Context.Maximization;
      if (double.IsNaN(quality)) {
        quality = eval(perm, token);
        evaluations++;
      }
      Encodings.PermutationEncoding.Permutation bestOfTheWalk = null;
      double bestOfTheWalkF = double.NaN;
      var current = (Encodings.PermutationEncoding.Permutation)perm.Clone();
      var currentF = quality;
      var overallImprovement = false;
      var tabu = new double[current.Length, current.Length];
      for (var i = 0; i < current.Length; i++) {
        for (var j = i; j < current.Length; j++) {
          tabu[i, j] = tabu[j, i] = maximization ? double.MinValue : double.MaxValue;
        }
        tabu[i, current[i]] = currentF;
      }

      var steps = 0;
      var stepsUntilBestOfWalk = 0;
      for (var iter = 0; iter < int.MaxValue; iter++) {
        var allTabu = true;
        var bestOfTheRestF = double.NaN;
        Swap2Move bestOfTheRest = null;
        var improved = false;
        foreach (var swap in ExhaustiveSwap2MoveGenerator.Generate(current).Shuffle(random)) {
          if (subspace != null && !(subspace[swap.Index1, 0] && subspace[swap.Index2, 0]))
            continue;

          var h = current[swap.Index1];
          current[swap.Index1] = current[swap.Index2];
          current[swap.Index2] = h;
          var q = eval(current, token);
          evaluations++;
          if (FitnessComparer.IsBetter(maximization, q, quality)) {
            overallImprovement = true;
            quality = q;
            for (var i = 0; i < current.Length; i++) perm[i] = current[i];
          }
          // check if it would not be an improvement to swap these into their positions
          var isTabu = !FitnessComparer.IsBetter(maximization, q, tabu[swap.Index1, current[swap.Index1]])
                    && !FitnessComparer.IsBetter(maximization, q, tabu[swap.Index2, current[swap.Index2]]);
          if (!isTabu) allTabu = false;
          if (FitnessComparer.IsBetter(maximization, q, currentF) && !isTabu) {
            if (FitnessComparer.IsBetter(maximization, q, bestOfTheWalkF)) {
              bestOfTheWalk = (Encodings.PermutationEncoding.Permutation)current.Clone();
              bestOfTheWalkF = q;
              stepsUntilBestOfWalk = steps;
            }
            steps++;
            improved = true;
            // perform the move
            currentF = q;
            // mark that to move them to their previous position requires to make an improvement
            tabu[swap.Index1, current[swap.Index2]] = maximization ? Math.Max(q, tabu[swap.Index1, current[swap.Index2]])
                                                                   : Math.Min(q, tabu[swap.Index1, current[swap.Index2]]);
            tabu[swap.Index2, current[swap.Index1]] = maximization ? Math.Max(q, tabu[swap.Index2, current[swap.Index1]])
                                                                   : Math.Min(q, tabu[swap.Index2, current[swap.Index1]]);
          } else { // undo the move
            if (!isTabu && FitnessComparer.IsBetter(maximization, q, bestOfTheRestF)) {
              bestOfTheRest = swap;
              bestOfTheRestF = q;
            }
            current[swap.Index2] = current[swap.Index1];
            current[swap.Index1] = h;
          }
          if (evaluations >= maxEvals) break;
        }
        if (!allTabu && !improved && bestOfTheRest != null) {
          tabu[bestOfTheRest.Index1, current[bestOfTheRest.Index1]] = maximization ? Math.Max(currentF, tabu[bestOfTheRest.Index1, current[bestOfTheRest.Index1]])
                                                                                   : Math.Min(currentF, tabu[bestOfTheRest.Index1, current[bestOfTheRest.Index1]]);
          tabu[bestOfTheRest.Index2, current[bestOfTheRest.Index2]] = maximization ? Math.Max(currentF, tabu[bestOfTheRest.Index2, current[bestOfTheRest.Index2]])
                                                                                   : Math.Min(currentF, tabu[bestOfTheRest.Index2, current[bestOfTheRest.Index2]]);

          var h = current[bestOfTheRest.Index1];
          current[bestOfTheRest.Index1] = current[bestOfTheRest.Index2];
          current[bestOfTheRest.Index2] = h;

          currentF = bestOfTheRestF;
          steps++;
        } else if (allTabu) break;
        if (evaluations >= maxEvals) break;
      }
      Context.IncrementEvaluatedSolutions(evaluations);
      if (!overallImprovement && bestOfTheWalk != null) {
        quality = bestOfTheWalkF;
        for (var i = 0; i < current.Length; i++) perm[i] = bestOfTheWalk[i];
      }
      return stepsUntilBestOfWalk;
    }

    public int TabuWalkShift(IRandom random, Encodings.PermutationEncoding.Permutation perm, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, ref double quality, int maxEvals = int.MaxValue, bool[,] subspace = null) {
      return 0;
    }

    public int TabuWalkOpt(IRandom random, Encodings.PermutationEncoding.Permutation perm, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, ref double quality, int maxEvals = int.MaxValue, bool[,] subspace = null) {
      var maximization = Context.Maximization;
      var evaluations = 0;
      if (double.IsNaN(quality)) {
        quality = eval(perm, token);
        evaluations++;
      }
      Encodings.PermutationEncoding.Permutation bestOfTheWalk = null;
      var bestOfTheWalkF = double.NaN;
      var current = (Encodings.PermutationEncoding.Permutation)perm.Clone();
      var currentF = quality;
      var overallImprovement = false;
      var tabu = new double[current.Length, current.Length];
      for (var i = 0; i < current.Length; i++) {
        for (var j = i; j < current.Length; j++) {
          tabu[i, j] = tabu[j, i] = double.MaxValue;
        }
        tabu[current[i], current.GetCircular(i + 1)] = currentF;
        tabu[current.GetCircular(i + 1), current[i]] = currentF;
      }

      var steps = 0;
      var stepsUntilBestOfWalk = 0;
      for (var iter = 0; iter < int.MaxValue; iter++) {
        var allTabu = true;
        var bestOfTheRestF = double.NaN;
        InversionMove bestOfTheRest = null;
        var improved = false;
        
        foreach (var opt in ExhaustiveInversionMoveGenerator.Generate(current).Shuffle(random)) {
          var prev = opt.Index1 - 1;
          var next = (opt.Index2 + 1) % current.Length;
          if (prev < 0) prev += current.Length;
          if (subspace != null && !(subspace[current[prev], current[opt.Index1]] && subspace[current[opt.Index2], current[next]]))
            continue;

          current.Reverse(opt.Index1, opt.Index2 - opt.Index1 + 1);

          var q = eval(current, token);
          evaluations++;
          if (FitnessComparer.IsBetter(maximization, q, quality)) {
            overallImprovement = true;
            quality = q;
            for (var i = 0; i < current.Length; i++) perm[i] = current[i];
          }
          // check if it would not be an improvement to opt these into their positions
          var isTabu = !FitnessComparer.IsBetter(maximization, q, tabu[current[prev], current[opt.Index1]])
                    && !FitnessComparer.IsBetter(maximization, q, tabu[current[opt.Index2], current[next]]);
          if (!isTabu) allTabu = false;
          if (!isTabu && FitnessComparer.IsBetter(maximization, q, currentF)) {
            if (FitnessComparer.IsBetter(maximization, q, bestOfTheWalkF)) {
              bestOfTheWalk = (Encodings.PermutationEncoding.Permutation)current.Clone();
              bestOfTheWalkF = q;
              stepsUntilBestOfWalk = steps;
            }

            steps++;
            improved = true;
            // perform the move
            currentF = q;
            // mark that to move them to their previous position requires to make an improvement
            if (maximization) {
              tabu[current[prev], current[opt.Index2]] = Math.Max(q, tabu[current[prev], current[opt.Index2]]);
              tabu[current[opt.Index2], current[prev]] = Math.Max(q, tabu[current[opt.Index2], current[prev]]);
              tabu[current[opt.Index1], current[next]] = Math.Max(q, tabu[current[opt.Index1], current[next]]);
              tabu[current[next], current[opt.Index1]] = Math.Max(q, tabu[current[next], current[opt.Index1]]);
            } else {
              tabu[current[prev], current[opt.Index2]] = Math.Min(q, tabu[current[prev], current[opt.Index2]]);
              tabu[current[opt.Index2], current[prev]] = Math.Min(q, tabu[current[opt.Index2], current[prev]]);
              tabu[current[opt.Index1], current[next]] = Math.Min(q, tabu[current[opt.Index1], current[next]]);
              tabu[current[next], current[opt.Index1]] = Math.Min(q, tabu[current[next], current[opt.Index1]]);
            }
          } else { // undo the move
            if (!isTabu && FitnessComparer.IsBetter(maximization, q, bestOfTheRestF)) {
              bestOfTheRest = opt;
              bestOfTheRestF = q;
            }
            
            current.Reverse(opt.Index1, opt.Index2 - opt.Index1 + 1);
          }
          if (evaluations >= maxEvals) break;
        }
        if (!allTabu && !improved && bestOfTheRest != null) {
          var prev = bestOfTheRest.Index1 - 1;
          var next = (bestOfTheRest.Index2 + 1) % current.Length;
          if (prev < 0) prev += current.Length;

          if (maximization) {
            tabu[current[prev], current[bestOfTheRest.Index1]] = Math.Max(currentF, tabu[current[prev], current[bestOfTheRest.Index1]]);
            tabu[current[bestOfTheRest.Index1], current[prev]] = Math.Max(currentF, tabu[current[bestOfTheRest.Index1], current[prev]]);
            tabu[current[bestOfTheRest.Index2], current[next]] = Math.Max(currentF, tabu[current[bestOfTheRest.Index2], current[next]]);
            tabu[current[next], current[bestOfTheRest.Index2]] = Math.Max(currentF, tabu[current[next], current[bestOfTheRest.Index2]]);
          } else {
            tabu[current[prev], current[bestOfTheRest.Index1]] = Math.Min(currentF, tabu[current[prev], current[bestOfTheRest.Index1]]);
            tabu[current[bestOfTheRest.Index1], current[prev]] = Math.Min(currentF, tabu[current[bestOfTheRest.Index1], current[prev]]);
            tabu[current[bestOfTheRest.Index2], current[next]] = Math.Min(currentF, tabu[current[bestOfTheRest.Index2], current[next]]);
            tabu[current[next], current[bestOfTheRest.Index2]] = Math.Min(currentF, tabu[current[next], current[bestOfTheRest.Index2]]);
          }
          current.Reverse(bestOfTheRest.Index1, bestOfTheRest.Index2 - bestOfTheRest.Index1 + 1);

          currentF = bestOfTheRestF;
          steps++;
        } else if (allTabu) break;
        if (evaluations >= maxEvals) break;
      }
      Context.IncrementEvaluatedSolutions(evaluations);
      if (!overallImprovement && bestOfTheWalk != null) {
        quality = bestOfTheWalkF;
        for (var i = 0; i < current.Length; i++) perm[i] = bestOfTheWalk[i];
      }
      return stepsUntilBestOfWalk;
    }

    protected override ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> Breed(ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p1, ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p2, CancellationToken token) {
      ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> child = null;

      if (p1.Solution.PermutationType == PermutationTypes.Absolute) {
        child = CrossAbsolute(p1, p2, token);
      } else if (p1.Solution.PermutationType == PermutationTypes.RelativeDirected) {
        child = CrossRelativeDirected(p1, p2, token);
      } else if (p1.Solution.PermutationType == PermutationTypes.RelativeUndirected) {
        child = CrossRelativeUndirected(p1, p2, token);
      } else throw new ArgumentException(string.Format("Unknown permutation type {0}", p1.Solution.PermutationType));

      if (VALIDATE && !child.Solution.Validate()) throw new ArgumentException("Cross produced invalid child");
      return child;
    }

    private ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> CrossAbsolute(ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p1, ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p2, CancellationToken token) {
      var cache = new HashSet<Encodings.PermutationEncoding.Permutation>(new PermutationEqualityComparer());
      cache.Add(p1.Solution);
      cache.Add(p2.Solution);

      var cacheHits = new Dictionary<int, int>() { { 0, 0 }, { 1, 0 }, { 2, 0 } };
      var evaluations = 0;
      ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> offspring = null;
      var probe = Context.ToScope((Encodings.PermutationEncoding.Permutation)p1.Solution.Clone());
      while (evaluations < p1.Solution.Length) {
        Encodings.PermutationEncoding.Permutation c = null;
        var xochoice = cacheHits.SampleRandom(Context.Random).Key;
        switch (xochoice) {
          case 0: c = CyclicCrossover2.Apply(Context.Random, p1.Solution, p2.Solution); break;
          case 1: c = PartiallyMatchedCrossover.Apply(Context.Random, p1.Solution, p2.Solution); break;
          case 2: c = UniformLikeCrossover.Apply(Context.Random, p1.Solution, p2.Solution); break;
        }
        if (cache.Contains(c)) {
          cacheHits[xochoice]++;
          if (cacheHits[xochoice] > 10) {
            cacheHits.Remove(xochoice);
            if (cacheHits.Count == 0) break;
          }
          continue;
        }
        probe.Solution = c;
        Context.Evaluate(probe, token);
        evaluations++;
        cache.Add(c);
        if (offspring == null || Context.IsBetter(probe, offspring)) {
          offspring = probe;
          if (Context.IsBetter(offspring, p1) && Context.IsBetter(offspring, p2))
            break;
        }
      }
      Context.IncrementEvaluatedSolutions(evaluations);
      return offspring ?? p1;
    }

    private ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> CrossRelativeDirected(ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p1, ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p2, CancellationToken token) {
      var cache = new HashSet<Encodings.PermutationEncoding.Permutation>(new PermutationEqualityComparer());
      cache.Add(p1.Solution);
      cache.Add(p2.Solution);

      var cacheHits = new Dictionary<int, int>() { { 0, 0 }, { 1, 0 }, { 2, 0 } };
      var evaluations = 0;
      ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> offspring = null;
      var probe = Context.ToScope((Encodings.PermutationEncoding.Permutation)p1.Solution.Clone());
      while (evaluations < p1.Solution.Length) {
        Encodings.PermutationEncoding.Permutation c = null;
        var xochoice = cacheHits.SampleRandom(Context.Random).Key;
        switch (xochoice) {
          case 0: c = OrderCrossover2.Apply(Context.Random, p1.Solution, p2.Solution); break;
          case 1: c = PartiallyMatchedCrossover.Apply(Context.Random, p1.Solution, p2.Solution); break;
          case 2: c = MaximalPreservativeCrossover.Apply(Context.Random, p1.Solution, p2.Solution); break;
        }
        if (cache.Contains(c)) {
          cacheHits[xochoice]++;
          if (cacheHits[xochoice] > 10) {
            cacheHits.Remove(xochoice);
            if (cacheHits.Count == 0) break;
          }
          continue;
        }
        probe.Solution = c;
        Context.Evaluate(probe, token);
        evaluations++;
        cache.Add(c);
        if (offspring == null || Context.IsBetter(probe, offspring)) {
          offspring = probe;
          if (Context.IsBetter(offspring, p1) && Context.IsBetter(offspring, p2))
            break;
        }
      }
      Context.IncrementEvaluatedSolutions(evaluations);
      return offspring ?? p1;
    }

    private ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> CrossRelativeUndirected(ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p1, ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> p2, CancellationToken token) {
      var cache = new HashSet<Encodings.PermutationEncoding.Permutation>(new PermutationEqualityComparer());
      cache.Add(p1.Solution);
      cache.Add(p2.Solution);

      var cacheHits = new Dictionary<int, int>() { { 0, 0 }, { 1, 0 }, { 2, 0 } };
      var evaluations = 0;
      ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> offspring = null;
      var probe = Context.ToScope((Encodings.PermutationEncoding.Permutation)p1.Solution.Clone());
      while (evaluations <= p1.Solution.Length) {
        Encodings.PermutationEncoding.Permutation c = null;
        var xochoice = cacheHits.SampleRandom(Context.Random).Key;
        switch (xochoice) {
          case 0: c = OrderCrossover2.Apply(Context.Random, p1.Solution, p2.Solution); break;
          case 1: c = EdgeRecombinationCrossover.Apply(Context.Random, p1.Solution, p2.Solution); break;
          case 2: c = MaximalPreservativeCrossover.Apply(Context.Random, p1.Solution, p2.Solution); break;
        }
        if (cache.Contains(c)) {
          cacheHits[xochoice]++;
          if (cacheHits[xochoice] > 10) {
            cacheHits.Remove(xochoice);
            if (cacheHits.Count == 0) break;
          }
          continue;
        }
        probe.Solution = c;
        Context.Evaluate(probe, token);
        evaluations++;
        cache.Add(c);
        if (offspring == null || Context.IsBetter(probe, offspring)) {
          offspring = probe;
          if (Context.IsBetter(offspring, p1) && Context.IsBetter(offspring, p2))
            break;
        }
      }
      Context.IncrementEvaluatedSolutions(evaluations);
      return offspring ?? p1;
    }

    protected override ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> Link(ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> a, ISingleObjectiveSolutionScope<Encodings.PermutationEncoding.Permutation> b, CancellationToken token, bool delink = false) {
      double quality;
      return Context.ToScope(Relink(Context.Random, a.Solution, b.Solution, Context.Evaluate, token, delink, out quality));
    }

    public Encodings.PermutationEncoding.Permutation Relink(IRandom random, Encodings.PermutationEncoding.Permutation p1, Encodings.PermutationEncoding.Permutation p2, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, bool delink, out double best) {
      if (p1.PermutationType != p2.PermutationType) throw new ArgumentException(string.Format("Unequal permutation types {0} and {1}", p1.PermutationType, p2.PermutationType));
      switch (p1.PermutationType) {
        case PermutationTypes.Absolute:
          return delink ? DelinkSwap(random, p1, p2, eval, token, out best) : RelinkSwap(random, p1, p2, eval, token, out best);
        case PermutationTypes.RelativeDirected:
          return RelinkShift(random, p1, p2, eval, token, delink, out best);
        case PermutationTypes.RelativeUndirected:
          return delink ? DelinkOpt(random, p1, p2, eval, token, out best) : RelinkOpt(random, p1, p2, eval, token, out best);
        default: throw new ArgumentException(string.Format("Unknown permutation type {0}", p1.PermutationType));
      }
    }

    public Encodings.PermutationEncoding.Permutation RelinkSwap(IRandom random, Encodings.PermutationEncoding.Permutation p1, Encodings.PermutationEncoding.Permutation p2, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, out double best) {
      var maximization = Context.Maximization;
      var evaluations = 0;
      var child = (Encodings.PermutationEncoding.Permutation)p1.Clone();

      best = double.NaN;
      Encodings.PermutationEncoding.Permutation bestChild = null;

      var options = Enumerable.Range(0, child.Length).Where(x => child[x] != p2[x]).ToList();
      var invChild = new int[child.Length];
      for (var i = 0; i < child.Length; i++) invChild[child[i]] = i;
      
      while (options.Count > 0) {
        int bestOption = -1;
        var bestChange = double.NaN;
        for (var j = 0; j < options.Count; j++) {
          var idx = options[j];
          if (child[idx] == p2[idx]) {
            options.RemoveAt(j);
            j--;
            continue;
          }
          Swap(child, invChild[p2[idx]], idx);
          var moveF = eval(child, token);
          evaluations++;
          if (FitnessComparer.IsBetter(maximization, moveF, bestChange)) {
            bestChange = moveF;
            bestOption = j;
          }
          // undo
          Swap(child, invChild[p2[idx]], idx);
        }
        if (!double.IsNaN(bestChange)) {
          var idx1 = options[bestOption];
          var idx2 = invChild[p2[idx1]];
          Swap(child, idx1, idx2);
          invChild[child[idx1]] = idx1;
          invChild[child[idx2]] = idx2;
          if (FitnessComparer.IsBetter(maximization, bestChange, best)) {
            if (Dist(child, p2) > 0) {
              best = bestChange;
              bestChild = (Encodings.PermutationEncoding.Permutation)child.Clone();
            }
          }
          options.RemoveAt(bestOption);
        }
      }
      if (bestChild == null) {
        best = eval(child, token);
        evaluations++;
      }
      Context.IncrementEvaluatedSolutions(evaluations);

      if (VALIDATE && bestChild != null && !bestChild.Validate()) throw new ArgumentException("Relinking produced invalid child");
      if (VALIDATE && Dist(child, p2) > 0) throw new InvalidOperationException("Child is not equal to p2 after relinking");

      return bestChild ?? child;
    }

    public Encodings.PermutationEncoding.Permutation DelinkSwap(IRandom random, Encodings.PermutationEncoding.Permutation p1, Encodings.PermutationEncoding.Permutation p2, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, out double best) {
      var maximization = Context.Maximization;
      var evaluations = 0;
      var child = (Encodings.PermutationEncoding.Permutation)p1.Clone();

      best = double.NaN;
      Encodings.PermutationEncoding.Permutation bestChild = null;

      var options = Enumerable.Range(0, child.Length).Where(x => child[x] == p2[x]).ToList();
      
      while (options.Count > 0) {
        int bestOption = -1;
        int bestCompanion = -1;
        var bestChange = double.NaN;
        for (var j = 0; j < options.Count; j++) {
          var idx = options[j];
          if (child[idx] != p2[idx]) {
            options.RemoveAt(j);
            j--;
            continue;
          }
          for (var k = 0; k < child.Length; k++) {
            if (k == idx) continue;
            Swap(child, k, idx);
            var moveF = eval(child, token);
            evaluations++;
            if (FitnessComparer.IsBetter(maximization, moveF, bestChange)) {
              bestChange = moveF;
              bestOption = j;
              bestCompanion = k;
            }
            // undo
            Swap(child, k, idx);
          }
        }
        if (!double.IsNaN(bestChange)) {
          var idx1 = options[bestOption];
          Swap(child, idx1, bestCompanion);
          if (FitnessComparer.IsBetter(maximization, bestChange, best)) {
            if (!Eq(child, p2)) {
              best = bestChange;
              bestChild = (Encodings.PermutationEncoding.Permutation)child.Clone();
            }
          }
          options.RemoveAt(bestOption);
        }
      }
      if (bestChild == null) {
        best = eval(child, token);
        evaluations++;
      }
      Context.IncrementEvaluatedSolutions(evaluations);

      if (VALIDATE && bestChild != null && !bestChild.Validate()) throw new ArgumentException("Delinking produced invalid child");

      return bestChild ?? child;
    }

    public Encodings.PermutationEncoding.Permutation RelinkShift(IRandom random, Encodings.PermutationEncoding.Permutation p1, Encodings.PermutationEncoding.Permutation p2, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, bool delink, out double best) {
      var maximization = Context.Maximization;
      var evaluations = 0;
      var child = (Encodings.PermutationEncoding.Permutation)p1.Clone();

      best = double.NaN;
      Encodings.PermutationEncoding.Permutation bestChild = null;

      var invChild = new int[child.Length];
      for (var i = 0; i < child.Length; i++) invChild[child[i]] = i;

      var bestChange = double.NaN;
      do {
        int bestFrom = -1, bestTo = -1;
        bestChange = double.NaN;
        for (var j = 0; j < child.Length; j++) {
          var c = invChild[p2[j]];
          var n = invChild[p2.GetCircular(j + 1)];
          if (n - c == 1 || c == child.Length - 1 && n == 0) continue;

          if (c < n) Shift(child, from: n, to: c + 1);
          else Shift(child, from: c, to: n);
          var moveF = eval(child, token);
          evaluations++;
          if (FitnessComparer.IsBetter(maximization, moveF, bestChange)) {
            bestChange = moveF;
            bestFrom = c < n ? n : c;
            bestTo = c < n ? c + 1 : n;
          }
          // undo
          if (c < n) Shift(child, from: c + 1, to: n);
          else Shift(child, from: n, to: c);
        }
        if (!double.IsNaN(bestChange)) {
          Shift(child, bestFrom, bestTo);
          for (var i = Math.Min(bestFrom, bestTo); i < Math.Max(bestFrom, bestTo); i++) invChild[child[i]] = i;
          if (FitnessComparer.IsBetter(maximization, bestChange, best)) {
            best = bestChange;
            bestChild = (Encodings.PermutationEncoding.Permutation)child.Clone();
          }
        }
      } while (!double.IsNaN(bestChange));

      if (bestChild == null) {
        best = eval(child, token);
        evaluations++;
      }
      Context.IncrementEvaluatedSolutions(evaluations);

      if (VALIDATE && bestChild != null && !bestChild.Validate()) throw new ArgumentException("Relinking produced invalid child");
      if (VALIDATE && Dist(child, p2) > 0) throw new InvalidOperationException("Child is not equal to p2 after relinking");

      return bestChild ?? child;
    }

    public Encodings.PermutationEncoding.Permutation RelinkOpt(IRandom random, Encodings.PermutationEncoding.Permutation p1, Encodings.PermutationEncoding.Permutation p2, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, out double best) {
      var maximization = Context.Maximization;
      var evaluations = 0;
      var child = (Encodings.PermutationEncoding.Permutation)p1.Clone();

      best = double.NaN;
      Encodings.PermutationEncoding.Permutation bestChild = null;

      var invChild = new int[child.Length];
      var invP2 = new int[child.Length];
      for (var i = 0; i < child.Length; i++) {
        invChild[child[i]] = i;
        invP2[p2[i]] = i;
      }

      var bestChange = double.NaN;
      var moveQueue = new Queue<Tuple<int, int>>();
      var undoStack = new Stack<Tuple<int, int>>();
      do {
        Queue<Tuple<int, int>> bestQueue = null;
        bestChange = double.NaN;
        for (var j = 0; j < p2.Length; j++) {
          if (IsUndirectedEdge(invChild, p2[j], p2.GetCircular(j + 1))) continue;

          var a = invChild[p2[j]];
          var b = invChild[p2.GetCircular(j + 1)];
          if (a > b) { var h = a; a = b; b = h; }
          var aprev = a - 1;
          var bprev = b - 1;
          while (IsUndirectedEdge(invP2, child.GetCircular(aprev), child.GetCircular(aprev + 1))) {
            aprev--;
          }
          while (IsUndirectedEdge(invP2, child.GetCircular(bprev), child.GetCircular(bprev + 1))) {
            bprev--;
          }
          var bnext = b + 1;
          var anext = a + 1;
          while (IsUndirectedEdge(invP2, child.GetCircular(bnext - 1), child.GetCircular(bnext))) {
            bnext++;
          }
          while (IsUndirectedEdge(invP2, child.GetCircular(anext - 1), child.GetCircular(anext))) {
            anext++;
          }
          aprev++; bprev++; anext--; bnext--;

          if (aprev < a && bnext > b) {
            if (aprev < 0) {
              moveQueue.Enqueue(Tuple.Create(a + 1, bnext));
              moveQueue.Enqueue(Tuple.Create(a + 1, a + 1 + (bnext - b)));
            } else {
              moveQueue.Enqueue(Tuple.Create(aprev, b - 1));
              moveQueue.Enqueue(Tuple.Create(b - 1 - (a - aprev), b - 1));
            }
          } else if (aprev < a && bnext == b && bprev == b) {
            moveQueue.Enqueue(Tuple.Create(a + 1, b));
          } else if (aprev < a && bprev < b) {
            moveQueue.Enqueue(Tuple.Create(a + 1, b));
          } else if (aprev == a && anext == a && bnext > b) {
            moveQueue.Enqueue(Tuple.Create(a, b - 1));
          } else if (aprev == a && anext == a && bnext == b && bprev == b) {
            moveQueue.Enqueue(Tuple.Create(a, b - 1));
          } else if (aprev == a && anext == a && bprev < b) {
            moveQueue.Enqueue(Tuple.Create(a + 1, b));
          } else if (anext > a && bnext > b) {
            moveQueue.Enqueue(Tuple.Create(a, b - 1));
          } else if (anext > a && bnext == b && bprev == b) {
            moveQueue.Enqueue(Tuple.Create(a, b - 1));
          } else /*if (anext > a && bprev < b)*/ {
            moveQueue.Enqueue(Tuple.Create(a, bprev - 1));
            moveQueue.Enqueue(Tuple.Create(bprev, b));
          }

          while (moveQueue.Count > 0) {
            var m = moveQueue.Dequeue();
            Opt(child, m.Item1, m.Item2);
            undoStack.Push(m);
          }
          var moveF = eval(child, token);
          evaluations++;
          if (FitnessComparer.IsBetter(maximization, moveF, bestChange)) {
            bestChange = moveF;
            bestQueue = new Queue<Tuple<int, int>>(undoStack.Reverse());
          }
          // undo
          while (undoStack.Count > 0) {
            var m = undoStack.Pop();
            Opt(child, m.Item1, m.Item2);
          }
        }
        if (!double.IsNaN(bestChange)) {
          while (bestQueue.Count > 0) {
            var m = bestQueue.Dequeue();
            Opt(child, m.Item1, m.Item2);
            for (var i = m.Item1; i <= m.Item2; i++) invChild[child[i]] = i;
          }
          if (FitnessComparer.IsBetter(maximization, bestChange, best)) {
            best = bestChange;
            bestChild = (Encodings.PermutationEncoding.Permutation)child.Clone();
          }
        }
      } while (!double.IsNaN(bestChange));

      if (bestChild == null) {
        best = eval(child, token);
        evaluations++;
      }
      Context.IncrementEvaluatedSolutions(evaluations);
      
      if (VALIDATE && bestChild != null && !bestChild.Validate()) throw new ArgumentException("Relinking produced invalid child");
      if (VALIDATE && Dist(child, p2) > 0) throw new InvalidOperationException("Child is not equal to p2 after relinking");
      return bestChild ?? child;
    }

    public Encodings.PermutationEncoding.Permutation DelinkOpt(IRandom random, Encodings.PermutationEncoding.Permutation p1, Encodings.PermutationEncoding.Permutation p2, Func<Encodings.PermutationEncoding.Permutation, CancellationToken, double> eval, CancellationToken token, out double best) {
      var evaluations = 0;
      var child = (Encodings.PermutationEncoding.Permutation)p1.Clone();

      best = double.NaN;
      Encodings.PermutationEncoding.Permutation bestChild = null;

      var invChild = new int[child.Length];
      var invP2 = new int[child.Length];
      for (var i = 0; i < child.Length; i++) {
        invChild[child[i]] = i;
        invP2[p2[i]] = i;
      }

      var order = Enumerable.Range(0, p2.Length).Where(x => IsUndirectedEdge(invP2, child[x], child.GetCircular(x + 1))).Shuffle(Context.Random).ToList();
      while (order.Count > 0) {
        var idx = order.First();
        var bestChange = double.NaN;
        var bestIdx = -1;
        for (var m = 0; m < p2.Length; m++) {
          if (Math.Abs(m - idx) <= 1 || Math.Abs(m - idx) >= p2.Length - 2) continue;
          if (m < idx) {
            if (IsUndirectedEdge(invP2, child.GetCircular(m - 1), child[idx])
              || IsUndirectedEdge(invP2, child[m], child.GetCircular(idx + 1))) continue;
            Opt(child, m, idx);
            var moveF = eval(child, token);
            evaluations++;
            if (Context.IsBetter(moveF, bestChange)) {
              bestChange = moveF;
              bestIdx = m;
            }
            // undo
            Opt(child, m, idx);
          } else {
            if (IsUndirectedEdge(invP2, child[idx], child[m])
              || IsUndirectedEdge(invP2, child.GetCircular(idx + 1), child.GetCircular(m + 1))) continue;
            Opt(child, idx + 1, m);
            var moveF = eval(child, token);
            evaluations++;
            if (Context.IsBetter(moveF, bestChange)) {
              bestChange = moveF;
              bestIdx = m;
            }
            // undo
            Opt(child, idx + 1, m);
          }
        }
        if (bestIdx >= 0) {
          if (bestIdx > idx)
            Opt(child, idx + 1, bestIdx);
          else Opt(child, bestIdx, idx);
          for (var i = Math.Min(idx, bestIdx); i <= Math.Max(idx, bestIdx); i++)
            invChild[child[i]] = i;

          order = Enumerable.Range(0, p2.Length).Where(x => IsUndirectedEdge(invP2, child[x], child.GetCircular(x + 1))).Shuffle(Context.Random).ToList();
          if (Context.IsBetter(bestChange, best)) {
            best = bestChange;
            bestChild = (Encodings.PermutationEncoding.Permutation)child.Clone();
          }
        }
      }

      if (bestChild == null) {
        best = eval(child, token);
        evaluations++;
      }
      Context.IncrementEvaluatedSolutions(evaluations);

      if (VALIDATE && bestChild != null && !bestChild.Validate()) throw new ArgumentException("Delinking produced invalid child");
      if (VALIDATE && Dist(child, p2) < 1) throw new InvalidOperationException("Child is not different from p2 after delinking");
      return bestChild ?? child;
    }

    private static bool IsUndirectedEdge(int[] invP, int a, int b) {
      var d = Math.Abs(invP[a] - invP[b]);
      return d == 1 || d == invP.Length - 1;
    }

    private static void Swap(Encodings.PermutationEncoding.Permutation child, int from, int to) {
      Swap2Manipulator.Apply(child, from, to);
    }

    private static void Shift(Encodings.PermutationEncoding.Permutation child, int from, int to) {
      TranslocationManipulator.Apply(child, from, from, to);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Opt(Encodings.PermutationEncoding.Permutation child, int from, int to) {
      if (from > to) child.Reverse(to, from - to + 1);
      else child.Reverse(from, to - from + 1);
    }
  }
}
