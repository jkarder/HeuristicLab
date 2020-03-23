﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HeuristicLab.JsonInterface {
  public interface IValueLookupJsonItem : ILookupJsonItem {
    [JsonIgnore]
    IJsonItem ActualValue { get; set; }
  }
}