﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2012 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [StorableClass]
  [Item(Name = "CovarianceSEard", Description = "Squared exponential covariance function with automatic relevance determination for Gaussian processes.")]
  public class CovarianceSEard : Item, ICovarianceFunction {
    [Storable]
    private double[,] x;
    [Storable]
    private double[,] xt;
    [Storable]
    private double sf2;
    [Storable]
    private double[] l;

    private double[,] sd;
    private bool symmetric;

    public int GetNumberOfParameters(int numberOfVariables) {
      return numberOfVariables + 1;
    }
    [StorableConstructor]
    protected CovarianceSEard(bool deserializing) : base(deserializing) { }
    protected CovarianceSEard(CovarianceSEard original, Cloner cloner)
      : base(original, cloner) {
      // note: using shallow copies here!
      this.x = original.x;
      this.xt = original.xt;
      this.sf2 = original.sf2;
      this.l = original.l;
    }
    public CovarianceSEard()
      : base() {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new CovarianceSEard(this, cloner);
    }

    public void SetParameter(double[] hyp, double[,] x) {
      SetParameter(hyp, x, x);
      this.symmetric = true;
    }

    public void SetParameter(double[] hyp, double[,] x, double[,] xt) {
      this.x = x;
      this.xt = xt;
      this.symmetric = false;

      this.l = hyp.Take(hyp.Length - 1).Select(Math.Exp).ToArray();
      this.sf2 = Math.Exp(2 * hyp[hyp.Length - 1]);
      sf2 = Math.Min(10E6, sf2); // upper limit for the scale

      sd = null;
    }

    public double GetCovariance(int i, int j) {
      if (sd == null) CalculateSquaredDistances();
      return sf2 * Math.Exp(-sd[i, j] / 2.0);
    }


    public double[] GetDiagonalCovariances() {
      if (x != xt) throw new InvalidOperationException();
      int rows = x.GetLength(0);
      var sd = new double[rows];
      for (int i = 0; i < rows; i++) {
        sd[i] = Util.SqrDist(
          Util.GetRow(x, i).Select((e, k) => e / l[k]),
          Util.GetRow(xt, i).Select((e, k) => e / l[k]));
      }
      return sd.Select(d => sf2 * Math.Exp(-d / 2.0)).ToArray();
    }

    public double[] GetGradient(int i, int j) {
      var res = new double[l.Length + 1];
      for (int k = 0; k < l.Length; k++) {
        double sqrDist = Util.SqrDist(x[i, k] / l[k], xt[j, k] / l[k]);

        res[k] = sf2 * Math.Exp(-sd[i, j] / 2.0) * sqrDist;
      }
      res[res.Length - 1] = 2.0 * sf2 * Math.Exp(-sd[i, j] / 2.0);
      return res;
    }


    private void CalculateSquaredDistances() {
      if (x.GetLength(1) != xt.GetLength(1)) throw new InvalidOperationException();
      int rows = x.GetLength(0);
      int cols = xt.GetLength(0);
      sd = new double[rows, cols];
      if (symmetric) {
        for (int i = 0; i < rows; i++) {
          for (int j = i; j < cols; j++) {
            sd[i, j] = Util.SqrDist(Util.GetRow(x, i).Select((e, k) => e / l[k]),
                                    Util.GetRow(xt, j).Select((e, k) => e / l[k]));
            sd[j, i] = sd[i, j];
          }
        }
      } else {
        for (int i = 0; i < rows; i++) {
          for (int j = 0; j < cols; j++) {
            sd[i, j] = Util.SqrDist(Util.GetRow(x, i).Select((e, k) => e / l[k]),
                                    Util.GetRow(xt, j).Select((e, k) => e / l[k]));
          }
        }
      }
    }
  }
}
