﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Collections.Generic;
using System.Text;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Modeling.Database.SQLServerCompact {
  [Plugin("HeuristicLab.Modeling.Database.SQLServerCompact-3.2")]
  [PluginFile("HeuristicLab.Modeling.Database.SQLServerCompact-3.2.dll", PluginFileType.Assembly)]
  [PluginDependency("HeuristicLab.Core-3.2")]
  [PluginDependency("HeuristicLab.Data-3.2")]
  [PluginDependency("HeuristicLab.DataAnalysis-3.2")]
  [PluginDependency("HeuristicLab.Modeling-3.2")]
  [PluginDependency("HeuristicLab.Modeling.Database-3.2")]
  [PluginDependency("HeuristicLab.SQLServerCompact-3.2")]  
  public class HeuristicLabModelingDatabaseSQLServerCompactPlugin : PluginBase {
  }
}
