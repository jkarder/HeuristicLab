﻿using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HEAL.Attic;
using System.IO;

namespace HeuristicLab.JsonInterface {
  /// <summary>
  /// Class to generate json interface templates.
  /// </summary>
  public class JCGenerator {

    public static void GenerateTemplate(string path, IOptimizer optimizer) =>
      GenerateTemplate(path, optimizer.Name, optimizer);

    public static void GenerateTemplate(string path, string templateName, IOptimizer optimizer) =>
      GenerateTemplate(path, templateName, optimizer, JsonItemConverter.Extract(optimizer));

    public static void GenerateTemplate(string path, string templateName, IOptimizer optimizer, IJsonItem rootItem) {
      #region Init
      JObject template = JObject.Parse(Constants.Template);
      JArray parameterItems = new JArray();
      JArray resultItems = new JArray();
      IList<IJsonItem> jsonItems = new List<IJsonItem>();
      string fullPath = Path.GetFullPath(path);
      #endregion

      // recursively filter items with values/ranges/actualNames
      PopulateJsonItems(rootItem, jsonItems);

      #region Serialize HL File
      ProtoBufSerializer serializer = new ProtoBufSerializer();
      string hlFilePath = fullPath + @"\" + templateName + ".hl";
      serializer.Serialize(optimizer, hlFilePath);
      #endregion

      #region Filter Items
      foreach (var item in jsonItems) {
        if (item is IResultJsonItem)
          resultItems.Add(item.GenerateJObject());
        else
          parameterItems.Add(item.GenerateJObject());
      }
      #endregion

      #region Set Template Data
      template[Constants.Metadata][Constants.TemplateName] = templateName;
      template[Constants.Metadata][Constants.HLFileLocation] = hlFilePath;
      template[Constants.Parameters] = parameterItems;
      template[Constants.Results] = resultItems;
      #endregion

      #region Serialize and write to file
      File.WriteAllText(fullPath + @"\" + templateName + ".json", SingleLineArrayJsonWriter.Serialize(template));
      #endregion
    }

    #region Helper    
    private static void PopulateJsonItems(IJsonItem item, IList<IJsonItem> jsonItems) {
      foreach(var x in item) {
        if (x.Active && !(x is EmptyJsonItem)) {
          jsonItems.Add(x);
        }
      }
    }
    #endregion
  }
}