﻿#region License Information
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding.Creators;
using HeuristicLab.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HeuristicLab.Encodings.SymbolicExpressionTreeEncoding_3._3.Tests {
  [TestClass]
  public class ProbabilisticTreeCreaterTest {
    private const int POPULATION_SIZE = 10000;
    private const int MAX_TREE_SIZE = 100;
    private const int MAX_TREE_HEIGHT = 10;
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    [TestMethod()]
    public void ProbabilisticTreeCreaterDistributionsTest() {
      var randomTrees = new List<SymbolicExpressionTree>();
      var grammar = Grammars.CreateSimpleArithmeticGrammar();
      var random = new MersenneTwister(31415);
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      for (int i = 0; i < POPULATION_SIZE; i++) {
        randomTrees.Add(ProbabilisticTreeCreator.Create(random, grammar, MAX_TREE_SIZE, MAX_TREE_HEIGHT, 0, 0));
      }
      stopwatch.Stop();

      foreach (var tree in randomTrees) {
        Util.IsValid(tree);
      }
      double msPerRandomTreeCreation = stopwatch.ElapsedMilliseconds / (double)POPULATION_SIZE;

      Console.WriteLine("ProbabilisticTreeCreator: " + Environment.NewLine +
        msPerRandomTreeCreation + " ms per random tree (~" + Math.Round(1000.0 / (msPerRandomTreeCreation)) + "random trees / s)" + Environment.NewLine +
        Util.GetSizeDistributionString(randomTrees, 105, 5) + Environment.NewLine +
        Util.GetFunctionDistributionString(randomTrees) + Environment.NewLine +
        Util.GetNumberOfSubTreesDistributionString(randomTrees) + Environment.NewLine +
        Util.GetTerminalDistributionString(randomTrees) + Environment.NewLine
        );
      Assert.IsTrue(Math.Round(1000.0 / (msPerRandomTreeCreation)) > 500); // must achieve more than 2000 random trees / s
    }


    [TestMethod()]
    public void ProbabilisticTreeCreaterWithAdfDistributionsTest() {
      var randomTrees = new List<SymbolicExpressionTree>();
      var grammar = Grammars.CreateArithmeticAndAdfGrammar();
      var random = new MersenneTwister(31415);
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      for (int i = 0; i < POPULATION_SIZE; i++) {
        randomTrees.Add(ProbabilisticTreeCreator.Create(random, grammar, MAX_TREE_SIZE, MAX_TREE_HEIGHT, 3, 3));
      }
      stopwatch.Stop();
      foreach (var tree in randomTrees)
        Util.IsValid(tree);

      double msPerRandomTreeCreation = stopwatch.ElapsedMilliseconds / (double)POPULATION_SIZE;

      Console.WriteLine("ProbabilisticTreeCreator: " + Environment.NewLine +
        msPerRandomTreeCreation + " ms per random tree (~" + Math.Round(1000.0 / (msPerRandomTreeCreation)) + "random trees / s)" + Environment.NewLine +
        Util.GetSizeDistributionString(randomTrees, 105, 5) + Environment.NewLine +
        Util.GetFunctionDistributionString(randomTrees) + Environment.NewLine +
        Util.GetNumberOfSubTreesDistributionString(randomTrees) + Environment.NewLine +
        Util.GetTerminalDistributionString(randomTrees) + Environment.NewLine
        );

      Assert.IsTrue(Math.Round(1000.0 / (msPerRandomTreeCreation)) > 300); // must achieve more than 2000 random trees / s
    }
  }
}