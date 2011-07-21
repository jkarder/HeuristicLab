#region License Information
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
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.DataAnalysis;
using HeuristicLab.Problems.DataAnalysis.Symbolic;
using HeuristicLab.Problems.DataAnalysis.Symbolic.Regression;
using HeuristicLab.Parameters;

namespace HeuristicLab.Algorithms.DataAnalysis {
  /// <summary>
  /// Neural network regression data analysis algorithm.
  /// </summary>
  [Item("Neural Network Regression", "Neural network regression data analysis algorithm (wrapper for ALGLIB).")]
  [Creatable("Data Analysis")]
  [StorableClass]
  public sealed class NeuralNetworkRegression : FixedDataAnalysisAlgorithm<IRegressionProblem> {
    private const string DecayParameterName = "Decay";
    private const string HiddenLayersParameterName = "HiddenLayers";
    private const string NodesInFirstHiddenLayerParameterName = "NodesInFirstHiddenLayer";
    private const string NodesInSecondHiddenLayerParameterName = "NodesInSecondHiddenLayer";
    private const string RestartsParameterName = "Restarts";
    private const string NeuralNetworkRegressionModelResultName = "Neural network regression solution";

    #region parameter properties
    public IFixedValueParameter<DoubleValue> DecayParameter {
      get { return (IFixedValueParameter<DoubleValue>)Parameters[DecayParameterName]; }
    }
    public ConstrainedValueParameter<IntValue> HiddenLayersParameter {
      get { return (ConstrainedValueParameter<IntValue>)Parameters[HiddenLayersParameterName]; }
    }
    public IFixedValueParameter<IntValue> NodesInFirstHiddenLayerParameter {
      get { return (IFixedValueParameter<IntValue>)Parameters[NodesInFirstHiddenLayerParameterName]; }
    }
    public IFixedValueParameter<IntValue> NodesInSecondHiddenLayerParameter {
      get { return (IFixedValueParameter<IntValue>)Parameters[NodesInSecondHiddenLayerParameterName]; }
    }
    public IFixedValueParameter<IntValue> RestartsParameter {
      get { return (IFixedValueParameter<IntValue>)Parameters[RestartsParameterName]; }
    }
    #endregion

    #region properties
    public double Decay {
      get { return DecayParameter.Value.Value; }
      set {
        if (value < 0.001 || value > 100) throw new ArgumentException("The decay parameter should be set to a value between 0.001 and 100.", "Decay");
        DecayParameter.Value.Value = value;
      }
    }
    public int HiddenLayers {
      get { return HiddenLayersParameter.Value.Value; }
      set {
        if (value < 0 || value > 2) throw new ArgumentException("The number of hidden layers should be set to 0, 1, or 2.", "HiddenLayers");
        HiddenLayersParameter.Value = (from v in HiddenLayersParameter.ValidValues
                                       where v.Value == value
                                       select v)
                                      .Single();
      }
    }
    public int NodesInFirstHiddenLayer {
      get { return NodesInFirstHiddenLayerParameter.Value.Value; }
      set {
        if (value < 1) throw new ArgumentException("The number of nodes in the first hidden layer must be at least one.", "NodesInFirstHiddenLayer");
        NodesInFirstHiddenLayerParameter.Value.Value = value;
      }
    }
    public int NodesInSecondHiddenLayer {
      get { return NodesInSecondHiddenLayerParameter.Value.Value; }
      set {
        if (value < 1) throw new ArgumentException("The number of nodes in the first second layer must be at least one.", "NodesInSecondHiddenLayer");
        NodesInSecondHiddenLayerParameter.Value.Value = value;
      }
    }
    public int Restarts {
      get { return RestartsParameter.Value.Value; }
      set {
        if (value < 0) throw new ArgumentException("The number of restarts must be positive.", "Restarts");
        RestartsParameter.Value.Value = value;
      }
    }
    #endregion


    [StorableConstructor]
    private NeuralNetworkRegression(bool deserializing) : base(deserializing) { }
    private NeuralNetworkRegression(NeuralNetworkRegression original, Cloner cloner)
      : base(original, cloner) {
    }
    public NeuralNetworkRegression()
      : base() {
      var validHiddenLayerValues = new ItemSet<IntValue>(new IntValue[] { new IntValue(0), new IntValue(1), new IntValue(2) });
      var selectedHiddenLayerValue = (from v in validHiddenLayerValues
                                      where v.Value == 1
                                      select v)
                                     .Single();
      Parameters.Add(new FixedValueParameter<DoubleValue>(DecayParameterName, "The decay parameter for the training phase of the neural network. This parameter determines the strengh of regularization and should be set to a value between 0.001 (weak regularization) to 100 (very strong regularization). The correct value should be determined via cross-validation.", new DoubleValue(1)));
      Parameters.Add(new ConstrainedValueParameter<IntValue>(HiddenLayersParameterName, "The number of hidden layers for the neural network (0, 1, or 2)", validHiddenLayerValues, selectedHiddenLayerValue));
      Parameters.Add(new FixedValueParameter<IntValue>(NodesInFirstHiddenLayerParameterName, "The number of nodes in the first hidden layer. This value is not used if the number of hidden layers is zero.", new IntValue(10)));
      Parameters.Add(new FixedValueParameter<IntValue>(NodesInSecondHiddenLayerParameterName, "The number of nodes in the second hidden layer. This value is not used if the number of hidden layers is zero or one.", new IntValue(10)));
      Parameters.Add(new FixedValueParameter<IntValue>(RestartsParameterName, "The number of restarts for learning.", new IntValue(2)));

      Problem = new RegressionProblem();
    }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new NeuralNetworkRegression(this, cloner);
    }

    #region neural network
    protected override void Run() {
      double rmsError, avgRelError;
      var solution = CreateNeuralNetworkRegressionSolution(Problem.ProblemData, HiddenLayers, NodesInFirstHiddenLayer, NodesInSecondHiddenLayer, Decay, Restarts, out rmsError, out avgRelError);
      Results.Add(new Result(NeuralNetworkRegressionModelResultName, "The neural network regression solution.", solution));
      Results.Add(new Result("Root mean square error", "The root of the mean of squared errors of the neural network regression solution on the training set.", new DoubleValue(rmsError)));
      Results.Add(new Result("Average relative error", "The average of relative errors of the neural network regression solution on the training set.", new PercentValue(avgRelError)));
    }

    public static IRegressionSolution CreateNeuralNetworkRegressionSolution(IRegressionProblemData problemData, int nLayers, int nHiddenNodes1, int nHiddenNodes2, double decay, int restarts,
      out double rmsError, out double avgRelError) {
      Dataset dataset = problemData.Dataset;
      string targetVariable = problemData.TargetVariable;
      IEnumerable<string> allowedInputVariables = problemData.AllowedInputVariables;
      IEnumerable<int> rows = problemData.TrainingIndizes;
      double[,] inputMatrix = AlglibUtil.PrepareInputMatrix(dataset, allowedInputVariables.Concat(new string[] { targetVariable }), rows);
      if (inputMatrix.Cast<double>().Any(x => double.IsNaN(x) || double.IsInfinity(x)))
        throw new NotSupportedException("Neural network regression does not support NaN or infinity values in the input dataset.");

      double targetMin = problemData.Dataset.GetEnumeratedVariableValues(targetVariable).Min();
      targetMin = targetMin - targetMin * 0.1; // -10%
      double targetMax = problemData.Dataset.GetEnumeratedVariableValues(targetVariable).Max();
      targetMax = targetMax + targetMax * 0.1; // + 10%

      alglib.multilayerperceptron multiLayerPerceptron = null;
      if (nLayers == 0) {
        alglib.mlpcreater0(allowedInputVariables.Count(), 1, targetMin, targetMax, out multiLayerPerceptron);
      } else if (nLayers == 1) {
        alglib.mlpcreater1(allowedInputVariables.Count(), nHiddenNodes1, 1, targetMin, targetMax, out multiLayerPerceptron);
      } else if (nLayers == 2) {
        alglib.mlpcreater2(allowedInputVariables.Count(), nHiddenNodes1, nHiddenNodes2, 1, targetMin, targetMax, out multiLayerPerceptron);
      } else throw new ArgumentException("Number of layers must be zero, one, or two.", "nLayers");
      alglib.mlpreport rep;
      int nRows = inputMatrix.GetLength(0);

      int info;
      // using mlptrainlm instead of mlptraines or mlptrainbfgs because only one parameter is necessary
      alglib.mlptrainlm(multiLayerPerceptron, inputMatrix, nRows, decay, restarts, out info, out rep);
      if (info != 2) throw new ArgumentException("Error in calculation of neural network regression solution");

      rmsError = alglib.mlprmserror(multiLayerPerceptron, inputMatrix, nRows);
      avgRelError = alglib.mlpavgrelerror(multiLayerPerceptron, inputMatrix, nRows);

      return new NeuralNetworkRegressionSolution(problemData, new NeuralNetworkModel(multiLayerPerceptron, targetVariable, allowedInputVariables));
    }
    #endregion
  }
}
