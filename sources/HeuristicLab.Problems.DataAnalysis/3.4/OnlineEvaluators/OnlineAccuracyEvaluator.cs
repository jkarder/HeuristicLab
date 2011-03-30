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
using HeuristicLab.Common;
using System.Collections.Generic;

namespace HeuristicLab.Problems.DataAnalysis {
  public class OnlineAccuracyEvaluator : IOnlineEvaluator {

    private int correctlyClassified;
    private int n;
    public double Accuracy {
      get {
        return correctlyClassified / (double)n;
      }
    }

    public OnlineAccuracyEvaluator() {
      Reset();
    }

    #region IOnlineEvaluator Members
    private OnlineEvaluatorError errorState;
    public OnlineEvaluatorError ErrorState {
      get { return errorState; }
    }
    public double Value {
      get { return Accuracy; }
    }
    public void Reset() {
      n = 0;
      correctlyClassified = 0;
      errorState = OnlineEvaluatorError.InsufficientElementsAdded;
    }

    public void Add(double original, double estimated) {
      // ignore cases where original is NaN completly 
      if (!double.IsNaN(original)) {
        // increment number of observed samples
        n++;
        if (original.IsAlmost(estimated)) {
          // original = estimated = +Inf counts as correctly classified
          // original = estimated = -Inf counts as correctly classified
          correctlyClassified++;
        }
        errorState = OnlineEvaluatorError.None; // number of (non-NaN) samples >= 1
      }
    }
    #endregion

    public static double Calculate(IEnumerable<double> first, IEnumerable<double> second, out OnlineEvaluatorError errorState) {
      IEnumerator<double> firstEnumerator = first.GetEnumerator();
      IEnumerator<double> secondEnumerator = second.GetEnumerator();
      OnlineAccuracyEvaluator accuracyEvaluator = new OnlineAccuracyEvaluator();

      // always move forward both enumerators (do not use short-circuit evaluation!)
      while (firstEnumerator.MoveNext() & secondEnumerator.MoveNext()) {
        double estimated = secondEnumerator.Current;
        double original = firstEnumerator.Current;
        accuracyEvaluator.Add(original, estimated);
      }

      // check if both enumerators are at the end to make sure both enumerations have the same length
      if (secondEnumerator.MoveNext() || firstEnumerator.MoveNext()) {
        throw new ArgumentException("Number of elements in first and second enumeration doesn't match.");
      } else {
        errorState = accuracyEvaluator.ErrorState;
        return accuracyEvaluator.Accuracy;
      }
    }
  }
}
