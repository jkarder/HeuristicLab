#region License Information
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

using System.Text;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;

namespace HeuristicLab.Data {
  [Item("DoubleArray", "Represents an array of double values.")]
  [StorableType("BFB2A7A4-A79B-462D-99FE-488484891361")]
  public class DoubleArray : StringConvertibleArray<double> {
    [StorableConstructor]
    protected DoubleArray(StorableConstructorFlag _) : base(_) { }
    protected DoubleArray(DoubleArray original, Cloner cloner)
      : base(original, cloner) {
    }
    public DoubleArray() : base() { }
    public DoubleArray(int length) : base(length) { }
    public DoubleArray(double[] elements, bool @readonly = false) : base(elements, @readonly) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new DoubleArray(this, cloner);
    }

    protected override bool Validate(string value, out string errorMessage) {
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
    protected override string GetValue(int index) {
      return this[index].ToString("r");
    }
    protected override bool SetValue(string value, int index) {
      double val;
      if (double.TryParse(value, out val)) {
        this[index] = val;
        return true;
      } else {
        return false;
      }
    }

    public new DoubleArray AsReadOnly() {
      return (DoubleArray)base.AsReadOnly();
    }
  }
}
