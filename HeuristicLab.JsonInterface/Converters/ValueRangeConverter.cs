﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;

namespace HeuristicLab.JsonInterface {

  public class IntRangeConverter : BaseConverter {
    public override int Priority => 1;
    public override Type ConvertableType => typeof(IntRange);

    public override void Inject(IItem item, IJsonItem data, IJsonItemConverter root) {
      IntRange range = item as IntRange;
      IntArrayJsonItem cdata = data as IntArrayJsonItem;
      range.Start = cdata.Value[0];
      range.End = cdata.Value[1];
    }

    public override IJsonItem Extract(IItem value, IJsonItemConverter root) {
      IntRange range = value as IntRange;
      return new IntArrayJsonItem() {
        Name = "[OverridableParamName]",
        Value = new int[] { range.Start, range.End },
        Range = new int[] { int.MinValue, int.MaxValue }
      };
    }
  }

  public class DoubleRangeConverter : BaseConverter {
    public override int Priority => 1;
    public override Type ConvertableType => typeof(DoubleRange);

    public override void Inject(IItem item, IJsonItem data, IJsonItemConverter root) {
      DoubleRange range = item as DoubleRange;
      DoubleArrayJsonItem cdata = data as DoubleArrayJsonItem;
      range.Start = cdata.Value[0];
      range.End = cdata.Value[1];
    }

    public override IJsonItem Extract(IItem value, IJsonItemConverter root) {
      DoubleRange range = value as DoubleRange;
      return new DoubleArrayJsonItem() {
        Name = "[OverridableParamName]",
        Value = new double[] { range.Start, range.End },
        Range = new double[] { double.MinValue, double.MaxValue }
      };
    }
  }
}