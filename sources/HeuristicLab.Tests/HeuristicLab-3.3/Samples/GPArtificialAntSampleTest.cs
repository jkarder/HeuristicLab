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

using System.IO;
using System.Linq;
using HeuristicLab.Algorithms.GeneticAlgorithm;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.Xml;
using HeuristicLab.Problems.ArtificialAnt;
using HeuristicLab.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HeuristicLab.Tests {
  /// <summary>
  /// Summary description for GPArtificialAntSampleTest
  /// </summary>
  [TestClass]
  public class GPArtificialAntSampleTest {
    private const string samplesDirectory = SamplesUtils.Directory;
    [ClassInitialize]
    public static void MyClassInitialize(TestContext testContext) {
      if (!Directory.Exists(samplesDirectory))
        Directory.CreateDirectory(samplesDirectory);
    }

    [TestMethod]
    [TestCategory("Samples.Create")]
    [TestProperty("Time", "medium")]
    public void CreateGpArtificialAntSampleTest() {
      var ga = CreateGpArtificialAntSample();
      XmlGenerator.Serialize(ga, @"Samples\SGP_SantaFe.hl");
    }

    [TestMethod]
    [TestCategory("Samples.Execute")]
    [TestProperty("Time", "long")]
    public void RunGpArtificialAntSampleTest() {
      var ga = CreateGpArtificialAntSample();
      ga.SetSeedRandomly.Value = false;
      SamplesUtils.RunAlgorithm(ga);
      Assert.AreEqual(81, SamplesUtils.GetDoubleResult(ga, "BestQuality"));
      Assert.AreEqual(48.19, SamplesUtils.GetDoubleResult(ga, "CurrentAverageQuality"));
      Assert.AreEqual(0, SamplesUtils.GetDoubleResult(ga, "CurrentWorstQuality"));
      Assert.AreEqual(50950, SamplesUtils.GetIntResult(ga, "EvaluatedSolutions"));
    }

    public GeneticAlgorithm CreateGpArtificialAntSample() {
      GeneticAlgorithm ga = new GeneticAlgorithm();
      #region Problem Configuration
      ArtificialAntProblem antProblem = new ArtificialAntProblem();
      antProblem.BestKnownQuality.Value = 89;
      antProblem.MaxExpressionDepth.Value = 10;
      antProblem.MaxExpressionLength.Value = 100;
      antProblem.MaxFunctionArguments.Value = 3;
      antProblem.MaxFunctionDefinitions.Value = 3;
      antProblem.MaxTimeSteps.Value = 600;
      #endregion
      #region Algorithm Configuration
      ga.Name = "Genetic Programming - Artificial Ant";
      ga.Description = "A standard genetic programming algorithm to solve the artificial ant problem (Santa-Fe trail)";
      ga.Problem = antProblem;
      SamplesUtils.ConfigureGeneticAlgorithmParameters<TournamentSelector, SubtreeCrossover, MultiSymbolicExpressionTreeArchitectureManipulator>(
        ga, 1000, 1, 50, 0.15, 5);
      var mutator = (MultiSymbolicExpressionTreeArchitectureManipulator)ga.Mutator;
      mutator.Operators.SetItemCheckedState(mutator.Operators
        .OfType<FullTreeShaker>()
        .Single(), false);
      mutator.Operators.SetItemCheckedState(mutator.Operators
        .OfType<OnePointShaker>()
        .Single(), false);
      mutator.Operators.SetItemCheckedState(mutator.Operators
        .OfType<ArgumentDeleter>()
        .Single(), false);
      mutator.Operators.SetItemCheckedState(mutator.Operators
        .OfType<SubroutineDeleter>()
        .Single(), false);
      #endregion
      return ga;
    }
  }
}