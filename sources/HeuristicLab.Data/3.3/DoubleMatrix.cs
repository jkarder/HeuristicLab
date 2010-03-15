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

using System.Text;
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Data {
  [Item("DoubleMatrix", "Represents a matrix of double values.")]
  [Creatable("Test")]
  [StorableClass]
  public sealed class DoubleMatrix : ValueTypeMatrix<double>, IStringConvertibleMatrix {
    public DoubleMatrix() : base() { }
    public DoubleMatrix(int rows, int columns) : base(rows, columns) { }
    public DoubleMatrix(double[,] elements) : base(elements) { }
    private DoubleMatrix(DoubleMatrix elements) : base(elements) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      DoubleMatrix clone = new DoubleMatrix(this);
      cloner.RegisterClonedObject(this, clone);
      return clone;
    }

    #region IStringConvertibleMatrix Members
    int IStringConvertibleMatrix.Rows {
      get { return Rows; }
      set { Rows = value; }
    }
    int IStringConvertibleMatrix.Columns {
      get { return Columns; }
      set { Columns = value; }
    }

    bool IStringConvertibleMatrix.Validate(string value, out string errorMessage) {
      double val;
      bool valid = double.TryParse(value, out val);
      errorMessage = string.Empty;
      if (!valid) {
        StringBuilder sb = new StringBuilder();
        sb.Append("Invalid Value (Valid Value Format: \"");
        sb.Append(FormatPatterns.GetDoubleFormatPattern());
        sb.Append("\")");
        errorMessage = sb.ToString();
      }
      return valid;
    }
    string IStringConvertibleMatrix.GetValue(int rowIndex, int columIndex) {
      return this[rowIndex, columIndex].ToString();
    }
    bool IStringConvertibleMatrix.SetValue(string value, int rowIndex, int columnIndex) {
      double val;
      if (double.TryParse(value, out val)) {
        this[rowIndex, columnIndex] = val;
        return true;
      } else {
        return false;
      }
    }
    #endregion
  }
}
