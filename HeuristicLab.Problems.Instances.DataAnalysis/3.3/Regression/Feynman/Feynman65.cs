using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Random;

namespace HeuristicLab.Problems.Instances.DataAnalysis {
  public class Feynman65 : FeynmanDescriptor {
    private readonly int testSamples;
    private readonly int trainingSamples;

    public Feynman65() : this((int) DateTime.Now.Ticks, 10000, 10000, null) { }

    public Feynman65(int seed) {
      Seed            = seed;
      trainingSamples = 10000;
      testSamples     = 10000;
      noiseRatio      = null;
    }

    public Feynman65(int seed, int trainingSamples, int testSamples, double? noiseRatio) {
      Seed                 = seed;
      this.trainingSamples = trainingSamples;
      this.testSamples     = testSamples;
      this.noiseRatio      = noiseRatio;
    }

    public override string Name {
      get {
        return string.Format("II.11.28 1+n*alpha/(1-(n*alpha/3)) | {0}",
          noiseRatio == null ? "no noise" : string.Format(System.Globalization.CultureInfo.InvariantCulture, "noise={0:g}",noiseRatio));
      }
    }

    protected override string TargetVariable { get { return noiseRatio == null ? "theta" : "theta_noise"; } }

    protected override string[] VariableNames {
      get { return noiseRatio == null ? new[] { "n", "alpha", "theta" } : new[] { "n", "alpha", "theta", "theta_noise" }; }
    }

    protected override string[] AllowedInputVariables { get { return new[] {"n", "alpha"}; } }

    public int Seed { get; private set; }

    protected override int TrainingPartitionStart { get { return 0; } }
    protected override int TrainingPartitionEnd { get { return trainingSamples; } }
    protected override int TestPartitionStart { get { return trainingSamples; } }
    protected override int TestPartitionEnd { get { return trainingSamples + testSamples; } }

    protected override List<List<double>> GenerateValues() {
      var rand = new MersenneTwister((uint) Seed);

      var data  = new List<List<double>>();
      var n     = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 0, 1).ToList();
      var alpha = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 0, 1).ToList();

      var theta = new List<double>();

      data.Add(n);
      data.Add(alpha);
      data.Add(theta);

      for (var i = 0; i < n.Count; i++) {
        var res = 1 + n[i] * alpha[i] / (1 - n[i] * alpha[i] / 3);
        theta.Add(res);
      }

      var targetNoise = ValueGenerator.GenerateNoise(theta, rand, noiseRatio);
      if (targetNoise != null) data.Add(targetNoise);

      return data;
    }
  }
}