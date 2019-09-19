﻿#region License Information
/* HeuristicLab
 * Copyright (C) Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.IO;

namespace HeuristicLab.Problems.Instances.TSPLIB {
  public abstract class TSPLIBPTSPInstanceProvider : TSPLIBInstanceProvider<PTSPData> {

    public override string Description {
      get { return "Traveling Salesman Problem Library (adapted for generating PTSP instances)"; }
    }

    protected override string FileExtension { get { return "tsp"; } }

    protected override PTSPData LoadInstance(TSPLIBParser parser, IDataDescriptor descriptor = null) {
      parser.Parse();
      if (parser.FixedEdges != null) throw new InvalidDataException("TSP instance " + parser.Name + " contains fixed edges which are not supported by HeuristicLab.");
      var instance = new PTSPData();
      instance.Dimension = parser.Dimension;
      instance.Coordinates = parser.Vertices != null ? parser.Vertices : parser.DisplayVertices;
      instance.Distances = parser.Distances;
      switch (parser.EdgeWeightType) {
        case TSPLIBEdgeWeightTypes.ATT:
          instance.DistanceMeasure = DistanceMeasure.Att; break;
        case TSPLIBEdgeWeightTypes.CEIL_2D:
          instance.DistanceMeasure = DistanceMeasure.UpperEuclidean; break;
        case TSPLIBEdgeWeightTypes.EUC_2D:
          instance.DistanceMeasure = DistanceMeasure.RoundedEuclidean; break;
        case TSPLIBEdgeWeightTypes.EUC_3D:
          throw new InvalidDataException("3D coordinates are not supported.");
        case TSPLIBEdgeWeightTypes.EXPLICIT:
          instance.DistanceMeasure = DistanceMeasure.Direct; break;
        case TSPLIBEdgeWeightTypes.GEO:
          instance.DistanceMeasure = DistanceMeasure.Geo; break;
        case TSPLIBEdgeWeightTypes.MAN_2D:
          instance.DistanceMeasure = DistanceMeasure.Manhattan; break;
        case TSPLIBEdgeWeightTypes.MAN_3D:
          throw new InvalidDataException("3D coordinates are not supported.");
        case TSPLIBEdgeWeightTypes.MAX_2D:
          instance.DistanceMeasure = DistanceMeasure.Maximum; break;
        case TSPLIBEdgeWeightTypes.MAX_3D:
          throw new InvalidDataException("3D coordinates are not supported.");
        default:
          throw new InvalidDataException("The given edge weight is not supported by HeuristicLab.");
      }

      instance.Name = parser.Name;
      instance.Description = parser.Comment
        + Environment.NewLine + Environment.NewLine
        + GetInstanceDescription();

      instance.Probabilities = GetProbabilities(descriptor, instance);
      return instance;
    }

    protected abstract double[] GetProbabilities(IDataDescriptor descriptor, PTSPData instance);

    public override PTSPData LoadData(IDataDescriptor id) {
      var tsplibId = (id as TSPLIBDataDescriptor);
      if (tsplibId != null) tsplibId.SolutionIdentifier = null;
      return base.LoadData(tsplibId ?? id);
    }

    protected override void LoadSolution(TSPLIBParser parser, PTSPData instance) {
      // solutions are not defined for pTSP
    }

    protected override void LoadQuality(double? bestQuality, PTSPData instance) {
      // best known qualities are not defined for pTSP
    }
  }
}
