﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using HeuristicLab.Core;
using HeuristicLab.Optimization;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.VehicleRouting.Encodings.Alba;
using HeuristicLab.Parameters;
using System.Collections.Generic;
using HeuristicLab.Data;

namespace HeuristicLab.Problems.VehicleRouting.Encodings.Alba {
  [Item("StochasticSimpleLocalSearchMultiMoveGenerator", "Generates multiple random simple local search moves from a given Alba VRP encoding.")]
  [StorableClass]
  public sealed class StochasticSimpleLocalSearchMultiMoveGenerator : SimpleLocalSearchMoveGenerator, IStochasticOperator, IMultiMoveGenerator, IAlbaSimpleLocalSearchMoveOperator {
    public ILookupParameter<IRandom> RandomParameter {
      get { return (ILookupParameter<IRandom>)Parameters["Random"]; }
    }

    public IValueLookupParameter<IntValue> SampleSizeParameter {
      get { return (IValueLookupParameter<IntValue>)Parameters["SampleSize"]; }
    }
    
    [StorableConstructor]
    private StochasticSimpleLocalSearchMultiMoveGenerator(bool deserializing) : base(deserializing) { }

    public StochasticSimpleLocalSearchMultiMoveGenerator()
      : base() {
        Parameters.Add(new LookupParameter<IRandom>("Random", "The random number generator."));
        Parameters.Add(new ValueLookupParameter<IntValue>("SampleSize", "The number of moves to generate."));
    }

    protected override SimpleLocalSearchMove[] GenerateMoves(AlbaEncoding individual) {
      int sampleSize = SampleSizeParameter.ActualValue.Value;

      SimpleLocalSearchMove[] moves = new SimpleLocalSearchMove[sampleSize];
      for (int i = 0; i < sampleSize; i++) {
        moves[i] = StochasticSimpleLocalSearchSingleMoveGenerator.Apply(
          individual, Cities, RandomParameter.ActualValue);
      }

      return moves;
    }
  }
}
