#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2019 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using HeuristicLab.Problems.DataAnalysis;
using HeuristicLab.Persistence;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [StorableType("8cc4503e-dec8-43d9-a966-3619ace0da41")]
  /// <summary>
  /// Interface to represent a neural network ensemble model for either regression or classification
  /// </summary>
  public interface INeuralNetworkEnsembleModel : IRegressionModel, IClassificationModel {
  }
}
