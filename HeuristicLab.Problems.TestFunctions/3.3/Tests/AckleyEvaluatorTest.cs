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

using HeuristicLab.Encodings.RealVectorEncoding;
using HeuristicLab.Problems.TestFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HeuristicLab.Problems.TestFunctions_33.Tests {


  /// <summary>
  ///This is a test class for AckleyEvaluatorTest and is intended
  ///to contain all AckleyEvaluatorTest Unit Tests
  ///</summary>
  [TestClass()]
  public class AckleyEvaluatorTest {


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

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion

    /// <summary>
    ///A test for EvaluateFunction
    ///</summary>
    [TestMethod()]
    [DeploymentItem("HeuristicLab.Problems.TestFunctions-3.3.dll")]
    public void AckleyEvaluateFunctionTest() {
      AckleyEvaluator_Accessor target = new AckleyEvaluator_Accessor();
      RealVector point = null;
      double expected = target.BestKnownQuality;
      double actual;
      for (int dimension = target.MinimumProblemSize; dimension <= System.Math.Min(10, target.MaximumProblemSize); dimension++) {
        point = target.GetBestKnownSolution(dimension);
        actual = target.EvaluateFunction(point);
        Assert.AreEqual(expected, actual);
      }
    }
  }
}