﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2016 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace HeuristicLab.Problems.Instances.DataAnalysis {
  public class MibaFrictionRegressionInstanceProvider : ResourceRegressionInstanceProvider {
    public override string Name {
      get { return "Miba Friction Datasets"; }
    }
    public override string Description {
      get {
        return "A set of datasets for regression modelling of friction characteristics of friction plate systems. The data stem from tests of friction plates with commercial test benches for wet friction plate systems. Friction characteristics such as the coefficient of friction, wear, and temperatures are measured at different loads. The goal is to predict these values given load parameters. Data have been kindly provided by Miba frictec company";
      }
    }
    public override Uri WebLink {
      get { return null; }
    }
    public override string ReferencePublication {
      get { return "E. Lughofer, G. K. Kronberger, M. Kommenda, S. Saminger-Platz, A. Promberger, F. Nickel, S. M. Winkler, M. Affenzeller - Robust Fuzzy Modeling and Symbolic Regression for Establishing Accurate and Interpretable Prediction Models in Supervising Tribological Systems - Proceedings of the 8th International Joint Conference on Computational Intelligence, Porto, Portugal, 2016, pp. 51-63"; }
    }

    protected override string FileName { get { return "MibaFriction"; } }

    public override IEnumerable<IDataDescriptor> GetDataDescriptors() {
      List<ResourceRegressionDataDescriptor> descriptorList = new List<ResourceRegressionDataDescriptor>();
      descriptorList.Add(new CF1());
      // descriptorList.Add(new CF2());
      // descriptorList.Add(new CF3());
      // descriptorList.Add(new CF4());
      // descriptorList.Add(new NvhRating());
      // descriptorList.Add(new Temperature1());
      // descriptorList.Add(new Temperature2());
      // descriptorList.Add(new Wear1());
      // descriptorList.Add(new Wear2());
      var solutionsArchiveName = GetResourceName(FileName + @"\.zip");
      if (!String.IsNullOrEmpty(solutionsArchiveName)) {
        using (var solutionsZipFile = new ZipArchive(GetType().Assembly.GetManifestResourceStream(solutionsArchiveName), ZipArchiveMode.Read)) {
          IList<string> entries = new List<string>();
          foreach (var curEntry in solutionsZipFile.Entries) {
            entries.Add(curEntry.Name);
          }
          foreach (var entry in entries.OrderBy(x => x)) {
            string prettyName = Path.GetFileNameWithoutExtension(entry);
            ResourceRegressionDataDescriptor desc = descriptorList.Where(x => x.Name.Equals(prettyName)).FirstOrDefault();
            if (desc != null) {
              desc.ResourceName = entry;
              yield return desc;
            } else
              throw new ArgumentNullException("No descriptor could be found for this entry.");
          }
        }
      }
    }
  }
}
