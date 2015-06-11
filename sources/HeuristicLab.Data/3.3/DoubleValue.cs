#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2015 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Drawing;
using System.Text;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Data {
  [Item("DoubleValue", "Represents a double value.")]
  [StorableClass]
  public class DoubleValue : ValueTypeValue<double>, IComparable, IStringConvertibleValue {
    public static new Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Field; }
    }

    [StorableConstructor]
    protected DoubleValue(bool deserializing) : base(deserializing) { }
    protected DoubleValue(DoubleValue original, Cloner cloner)
      : base(original, cloner) {
    }
    public DoubleValue() : base() { }
    public DoubleValue(double value) : base(value) { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new DoubleValue(this, cloner);
    }

    public override string ToString() {
      return Value.ToString("G17");  // round-trip format
      // Note that the round-trip format "r" can still lead to different values when run on an 64-bit machine, therefore using G17 instead.
      // (https://msdn.microsoft.com/en-us/library/kfsatb94.aspx)
    }

    public virtual int CompareTo(object obj) {
      DoubleValue other = obj as DoubleValue;
      if (other != null)
        return Value.CompareTo(other.Value);
      else
        return Value.CompareTo(obj);
    }

    protected virtual bool Validate(string value, out string errorMessage) {
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
    protected virtual string GetValue() {
      return ToString();
    }
    protected virtual bool SetValue(string value) {
      double val;
      if (double.TryParse(value, out val)) {
        Value = val;
        return true;
      } else {
        return false;
      }
    }

    #region IStringConvertibleValue Members
    bool IStringConvertibleValue.Validate(string value, out string errorMessage) {
      return Validate(value, out errorMessage);
    }
    string IStringConvertibleValue.GetValue() {
      return GetValue();
    }
    bool IStringConvertibleValue.SetValue(string value) {
      return SetValue(value);
    }
    #endregion
  }
}
