﻿using HeuristicLab.Encodings.RealVector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;

namespace HeuristicLab.Encodings.RealVector_33.Tests {


  /// <summary>
  ///This is a test class for BlendAlphaBetaCrossoverTest and is intended
  ///to contain all BlendAlphaBetaCrossoverTest Unit Tests
  ///</summary>
  [TestClass()]
  public class BlendAlphaBetaCrossoverTest {


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
    ///A test for Cross
    ///</summary>
    [TestMethod()]
    [DeploymentItem("HeuristicLab.Encodings.RealVector-3.3.dll")]
    public void BlendAlphaBetaCrossoverCrossTest() {
      BlendAlphaBetaCrossover_Accessor target = new BlendAlphaBetaCrossover_Accessor(new PrivateObject(typeof(BlendAlphaBetaCrossover)));
      ItemArray<DoubleArray> parents;
      TestRandom random = new TestRandom();
      bool exceptionFired;
      // The following test checks if there is an exception when there are more than 2 parents
      random.Reset();
      parents = new ItemArray<DoubleArray>(new DoubleArray[] { new DoubleArray(5), new DoubleArray(6), new DoubleArray(4) });
      exceptionFired = false;
      try {
        DoubleArray actual;
        actual = target.Cross(random, parents);
      } catch (System.ArgumentException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
      // The following test checks if there is an exception when there are less than 2 parents
      random.Reset();
      parents = new ItemArray<DoubleArray>(new DoubleArray[] { new DoubleArray(4) });
      exceptionFired = false;
      try {
        DoubleArray actual;
        actual = target.Cross(random, parents);
      } catch (System.ArgumentException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
    }

    /// <summary>
    ///A test for Apply
    ///</summary>
    [TestMethod()]
    public void BlendAlphaBetaCrossoverApplyTest() {
      TestRandom random = new TestRandom();
      DoubleArray parent1, parent2, expected, actual;
      DoubleValue alpha;
      DoubleValue beta;
      bool exceptionFired;
      // The following test is not based on published examples
      random.Reset();
      random.DoubleNumbers = new double[] { 0.5, 0.5, 0.5, 0.5, 0.5 };
      alpha = new DoubleValue(0.5);
      beta = new DoubleValue(0.5);
      parent1 = new DoubleArray(new double[] { 0.2, 0.2, 0.3, 0.5, 0.1 });
      parent2 = new DoubleArray(new double[] { 0.4, 0.1, 0.3, 0.2, 0.8 });
      expected = new DoubleArray(new double[] { 0.3, 0.15, 0.3, 0.35, 0.45 });
      actual = BlendAlphaBetaCrossover.Apply(random, parent1, parent2, alpha, beta);
      Assert.IsTrue(Auxiliary.RealVectorIsAlmostEqualByPosition(actual, expected));
      // The following test is not based on published examples
      random.Reset();
      random.DoubleNumbers = new double[] { 0.25, 0.75, 0.25, 0.75, 0.25 };
      alpha = new DoubleValue(-0.25); // negative values for alpha are not allowed
      parent1 = new DoubleArray(new double[] { 0.2, 0.2, 0.3, 0.5, 0.1 });
      parent2 = new DoubleArray(new double[] { 0.4, 0.1, 0.3, 0.2, 0.8 });
      exceptionFired = false;
      try {
        actual = BlendAlphaBetaCrossover.Apply(random, parent1, parent2, alpha, beta);
      } catch (System.ArgumentException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
      // The following test is not based on published examples
      random.Reset();
      random.DoubleNumbers = new double[] { 0.25, 0.75, 0.25, 0.75, 0.25, .75 };
      alpha = new DoubleValue(0.25);
      parent1 = new DoubleArray(new double[] { 0.2, 0.2, 0.3, 0.5, 0.1, 0.9 }); // this parent is longer
      parent2 = new DoubleArray(new double[] { 0.4, 0.1, 0.3, 0.2, 0.8 });
      exceptionFired = false;
      try {
        actual = BlendAlphaBetaCrossover.Apply(random, parent1, parent2, alpha, beta);
      } catch (System.ArgumentException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
    }

    /// <summary>
    ///A test for BlendAlphaBetaCrossover Constructor
    ///</summary>
    [TestMethod()]
    public void BlendAlphaBetaCrossoverConstructorTest() {
      BlendAlphaBetaCrossover target = new BlendAlphaBetaCrossover();
    }
  }
}
