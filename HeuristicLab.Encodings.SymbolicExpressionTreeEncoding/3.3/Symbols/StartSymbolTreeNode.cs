#region License Information
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
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
namespace HeuristicLab.Encodings.SymbolicExpressionTreeEncoding.GeneralSymbols {
  [StorableClass]
  public sealed class StartSymbolTreeNode : SymbolicExpressionTreeTopLevelNode {
    // copy constructor
    private StartSymbolTreeNode(StartSymbolTreeNode original)
      : base(original) {
    }

    public StartSymbolTreeNode(StartSymbol startSymbol) : base(startSymbol) { }

    public override object Clone() {
      return new StartSymbolTreeNode(this);
    }
  }
}