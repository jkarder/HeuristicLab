﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using HeuristicLab.Common;
using HeuristicLab.Problems.BinPacking3D.ExtremePointPruning;
using HeuristicLab.Problems.BinPacking3D.Geometry;
using HeuristicLab.Problems.BinPacking3D.ResidualSpaceCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeuristicLab.Problems.BinPacking3D.ExtremePointCreation {

  /// <summary>
  /// This extreme point creation class uses the point projection based method by Crainic, T. G., Perboli, G., & Tadei, R. for creating extreme points.
  /// Each extreme point of an item is being projectet backwards, downwards and to the left. 
  /// A new extreme point will be created where this projections instersects with another item or the bin boundins.
  /// </summary>
  public class PointProjectionBasedEPCreator : ExtremePointCreator {
    /// <summary>
    /// This lock object is needed because of creating the extreme points in an parallel for loop.
    /// </summary>
    protected object _lockAddExtremePoint = new object();

    protected override void UpdateExtremePoints(BinPacking3D binPacking, PackingItem item, PackingPosition position) {
      binPacking.ExtremePoints.Clear();

      if (binPacking.Items.Count <= 0) {
        return;
      }
      
      // generate all new extreme points parallel. This speeds up the creator.      
      Parallel.ForEach(binPacking.Items, i => {
        PackingItem it = i.Value;
        PackingPosition pos = binPacking.Positions[i.Key];
        GenerateNewExtremePointsForItem(binPacking, it, pos);
      });
      
      // remove not needed extreme points.
      foreach (var extremePoint in binPacking.ExtremePoints.ToList()) {
        // check if a residual space can be removed
        foreach (var rs in extremePoint.Value.ToList()) {
          if (ResidualSpaceCanBeRemoved(binPacking, extremePoint.Key, rs)) {
            ((IList<ResidualSpace>)extremePoint.Value).Remove(rs);
          }
        }
        // if the current extreme point has no more residual spaces, it can be removed.
        if (((IList<ResidualSpace>)extremePoint.Value).Count <= 0) {
          binPacking.ExtremePoints.Remove(extremePoint);
        }
      }

    }

    /// <summary>
    /// Returns true if a given residual space can be removed.
    /// The given residual space can be removed if it is within another residual space and
    /// - neither the position of the other residual space and the current extreme point have an item below or
    /// - the current extreme point has an item below.
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="position"></param>
    /// <param name="rs"></param>
    /// <returns></returns>
    private bool ResidualSpaceCanBeRemoved(BinPacking3D binPacking, PackingPosition position, ResidualSpace rs) {
      foreach (var extremePoint in binPacking.ExtremePoints) {
        if (position.Equals(extremePoint.Key)) {
          continue;
        }
        if (IsWithinResidualSpaceOfAnotherExtremePoint(new Vector3D(position), rs, extremePoint.Key, extremePoint.Value)) {
          var itemBelowEp = LiesOnAnyItem(binPacking, extremePoint.Key);
          var itemBelowPos = LiesOnAnyItem(binPacking, position);

          if (itemBelowEp || !itemBelowEp && !itemBelowPos) {
            return true;
          }
        }
      }
      return false;
    }

    /// <summary>
    /// Returns true if the given position lies on an item or an the ground.
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool LiesOnAnyItem(BinPacking3D binPacking, PackingPosition position) {
      if (position.Y == 0) {
        return true;
      }

      var items = binPacking.Items.Where(x => {
        var itemPosition = binPacking.Positions[x.Key];
        var item = x.Value;
        int width = item.Width;
        int depth = item.Depth;

        return itemPosition.Y + item.Height == position.Y &&
               itemPosition.X <= position.X && position.X < itemPosition.X + width &&
               itemPosition.Z <= position.Z && position.Z < itemPosition.Z + depth;
      });

      return items.Count() > 0;
    }


    /// <summary>
    /// Adds a new extreme point an the related residual spaces to a given bin packing.
    /// - The given position has to be valid.
    /// - The extreme point does not exist in the bin packing.
    /// - There must be at minimum one valid residual space. A residual space is invalid if the space is zero.
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="position"></param>
    /// <returns>True = the given point and its related residual spaces were successfully added to the bin packing</returns>
    protected override bool AddExtremePoint(BinPacking3D binPacking, PackingPosition position) {
      if (position == null) {
        return false;
      }

      if (PointIsInAnyItem(binPacking, new Vector3D(position))) {
        return false;
      }

      // this is necessary if the extreme points are being created in a parallel loop.
      lock (_lockAddExtremePoint) {
        if (binPacking.ExtremePoints.ContainsKey(position)) {
          return false;
        }

        var rs = CalculateResidualSpace(binPacking, new Vector3D(position));

        if (rs.Count() <= 0) {
          return false;
        }

        binPacking.ExtremePoints.Add(position, rs);
        return true;
      }
    }

    /// <summary>
    /// Getnerates the extreme points for a given item.
    /// It creates extreme points by using a point projection based method and
    /// creates points by using an edge projection based method.
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="newItem"></param>
    /// <param name="position"></param>
    protected override void GenerateNewExtremePointsForItem(BinPacking3D binPacking, PackingItem newItem, PackingPosition position) {
      PointProjectionForNewItem(binPacking, newItem, position);
    }

    #region Extreme point creation by using a point projection based method

    /// <summary>
    /// This method creates extreme points by using a point projection based method.
    /// For each item there will be created three points and each of the points will be projected twice.
    /// The direction of the projection depends on position of the point.
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="newItem"></param>
    /// <param name="position"></param>
    protected void PointProjectionForNewItem(BinPacking3D binPacking, PackingItem newItem, PackingPosition position) {
      int newWidth = newItem.Width;
      int newDepth = newItem.Depth;
      var binShape = binPacking.BinShape;
      var sourcePoint = new PackingPosition(position.AssignedBin, position.X + newWidth, position.Y, position.Z);
      PointProjection(binPacking, sourcePoint, ProjectDown);
      PointProjection(binPacking, sourcePoint, ProjectBackward);

      sourcePoint = new PackingPosition(position.AssignedBin, position.X, position.Y + newItem.Height, position.Z);
      PointProjection(binPacking, sourcePoint, ProjectLeft);
      PointProjection(binPacking, sourcePoint, ProjectBackward);

      sourcePoint = new PackingPosition(position.AssignedBin, position.X, position.Y, position.Z + newDepth);
      PointProjection(binPacking, sourcePoint, ProjectDown);
      PointProjection(binPacking, sourcePoint, ProjectLeft);
    }


    /// <summary>
    /// Projects a given point by using the given projection method to the neares item.
    /// The given projection method returns a point which lies on a side of the nearest item in the direction. 
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="position"></param>
    /// <param name="projectionMethod"></param>
    private void PointProjection(BinPacking3D binPacking, PackingPosition position, Func<BinPacking3D, Vector3D, Vector3D> projectionMethod) {
      Vector3D sourcePoint = new Vector3D(position);
      if (sourcePoint.X < binPacking.BinShape.Width && sourcePoint.Y < binPacking.BinShape.Height && sourcePoint.Z < binPacking.BinShape.Depth) {
        Vector3D point = projectionMethod?.Invoke(binPacking, sourcePoint);
        if (point != null) {
          AddExtremePoint(binPacking, new PackingPosition(position.AssignedBin, point.X, point.Y, point.Z));
        }
      }
    }
    #endregion

    /// <summary>
    /// Updates the residual spaces.
    /// It removes not needed ones.
    /// A residual space will be removed if the space is a subspace of another one and 
    /// the current one has no item below or both have an item below. 
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="item"></param>
    /// <param name="position"></param>
    protected override void UpdateResidualSpace(BinPacking3D binPacking, PackingItem item, PackingPosition position) {
    }

    /// <summary>
    /// Returns true if any item in the bin packing encapsulates the given point
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool PointIsInAnyItem(BinPacking3D binPacking, Vector3D point) {
      foreach (var item in binPacking.Items) {
        PackingPosition position = binPacking.Positions[item.Key];
        var depth = item.Value.Depth;
        var width = item.Value.Width;
        if (position.X <= point.X && point.X < position.X + width &&
            position.Y <= point.Y && point.Y < position.Y + item.Value.Height &&
            position.Z <= point.Z && point.Z < position.Z + depth) {
          return true;
        }
      }
      return false;
    }


    /// <summary>
    /// Calculates the residual spaces for an extreme point.
    /// </summary>
    /// <param name="binPacking"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    protected override IEnumerable<ResidualSpace> CalculateResidualSpace(BinPacking3D binPacking, Vector3D pos) {
      return ResidualSpaceCalculatorFactory.CreateCalculator().CalculateResidualSpaces(binPacking, pos);
    }
  }
}