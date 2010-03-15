﻿using HeuristicLab.Encodings.RealVector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;

namespace HeuristicLab.Encodings.RealVector_33.Tests {


  /// <summary>
  ///This is a test class for DiscreteCrossoverTest and is intended
  ///to contain all DiscreteCrossoverTest Unit Tests
  ///</summary>
  [TestClass()]
  public class DiscreteCrossoverTest {


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
    public void DiscreteCrossoverCrossTest() {
      DiscreteCrossover_Accessor target = new DiscreteCrossover_Accessor(new PrivateObject(typeof(DiscreteCrossover)));
      ItemArray<DoubleArray> parents;
      TestRandom random = new TestRandom();
      bool exceptionFired;
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
    public void DiscreteCrossoverApplyTest() {
      TestRandom random = new TestRandom();
      DoubleArray parent1, parent2, expected, actual;
      ItemArray<DoubleArray> parents;
      bool exceptionFired;
      // The following test is not based on published examples
      random.Reset();
      random.IntNumbers = new int[] { 0, 0, 1, 0, 1 };
      parent1 = new DoubleArray(new double[] { 0.2, 0.2, 0.3, 0.5, 0.1 });
      parent2 = new DoubleArray(new double[] { 0.4, 0.1, 0.3, 0.2, 0.8 });
      parents = new ItemArray<DoubleArray>( new DoubleArray[] { parent1, parent2 } );
      expected = new DoubleArray(new double[] { 0.2, 0.2, 0.3, 0.5, 0.8 });
      actual = DiscreteCrossover.Apply(random, parents);
      Assert.IsTrue(Auxiliary.RealVectorIsAlmostEqualByPosition(actual, expected));
      // The following test is not based on published examples
      random.Reset();
      random.IntNumbers = new int[] { 0, 0, 1, 0, 1, 0 };
      parent1 = new DoubleArray(new double[] { 0.2, 0.2, 0.3, 0.5, 0.1, 0.9 }); // this parent is longer
      parent2 = new DoubleArray(new double[] { 0.4, 0.1, 0.3, 0.2, 0.8 });
      parents = new ItemArray<DoubleArray>(new DoubleArray[] { parent1, parent2 });
      exceptionFired = false;
      try {
        actual = DiscreteCrossover.Apply(random, parents);
      } catch (System.ArgumentException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
    }

    /// <summary>
    ///A test for DiscreteCrossover Constructor
    ///</summary>
    [TestMethod()]
    public void DiscreteCrossoverConstructorTest() {
      DiscreteCrossover target = new DiscreteCrossover();
    }
  }
}
