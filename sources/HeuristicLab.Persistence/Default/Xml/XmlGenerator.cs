﻿using System.Collections.Generic;
using System;
using System.Text;
using HeuristicLab.Persistence.Interfaces;
using HeuristicLab.Persistence.Core;
using System.IO;

namespace HeuristicLab.Persistence.Default.Xml {

  struct XmlStrings {
    public const string PRIMITIVE = "PRIMITVE";
    public const string COMPOSITE = "COMPOSITE";
    public const string REFERENCE = "REFERENCE";
    public const string NULL = "NULL";
    public const string TYPECACHE = "TYPCACHE";
    public const string TYPE = "TYPE";
  }

  public abstract class Generator<T> {
    public T Format(ISerializationToken token) {
      Type type = token.GetType();
      if (type == typeof(BeginToken))
        return Format((BeginToken)token);
      if (type == typeof(EndToken))
        return Format((EndToken)token);
      if (type == typeof(PrimitiveToken))
        return Format((PrimitiveToken)token);
      if (type == typeof(ReferenceToken))
        return Format((ReferenceToken)token);
      if (type == typeof(NullReferenceToken))
        return Format((NullReferenceToken)token);
      throw new ApplicationException("Invalid token of type " + type.FullName);
    }
    protected abstract T Format(BeginToken beginToken);
    protected abstract T Format(EndToken endToken);
    protected abstract T Format(PrimitiveToken primitiveToken);
    protected abstract T Format(ReferenceToken referenceToken);
    protected abstract T Format(NullReferenceToken nullReferenceToken);
  }

  public class XmlGenerator : Generator<string> {
    
    private int depth;

    public XmlGenerator() {
      depth = 0;
    }

    private enum NodeType { Start, End, Inline } ;

    private static string FormatNode(string name,
        Dictionary<string, object> attributes,
        NodeType type) {
      return FormatNode(name, attributes, type, " ");
    }

    private static string FormatNode(string name,
        Dictionary<string, object> attributes,
        NodeType type, string space) {
      StringBuilder sb = new StringBuilder();
      sb.Append('<');
      if (type == NodeType.End)
        sb.Append('/');
      sb.Append(name);      
      foreach (var attribute in attributes) {
        if (attribute.Value != null && !string.IsNullOrEmpty(attribute.Value.ToString())) {
          sb.Append(space);
          sb.Append(attribute.Key);
          sb.Append("=\"");
          sb.Append(attribute.Value);
          sb.Append('"');
        }
      }
      if (type == NodeType.Inline)
        sb.Append('/');
      sb.Append(">");
      return sb.ToString();      
    }

    private string Prefix {
      get { return new string(' ', depth * 2); }
    }

    protected override string Format(BeginToken beginToken) {            
      var attributes = new Dictionary<string, object> {
        {"name", beginToken.Name},
        {"typeId", beginToken.TypeId},
        {"id", beginToken.Id}};
      string result = Prefix +
                      FormatNode(XmlStrings.COMPOSITE, attributes, NodeType.Start) + "\n";
      depth += 1;
      return result;
    }

    protected override string Format(EndToken endToken) {      
      depth -= 1;
      return Prefix + "</" + XmlStrings.COMPOSITE + ">\n";
    }

    protected override string Format(PrimitiveToken dataToken) {      
      var attributes =
        new Dictionary<string, object> {
            {"typeId", dataToken.TypeId},
            {"name", dataToken.Name},
            {"id", dataToken.Id}};
      return Prefix +
        FormatNode(XmlStrings.PRIMITIVE, attributes, NodeType.Start) +
        dataToken.SerialData + "</" + XmlStrings.PRIMITIVE + ">\n";      
    }

    protected override string Format(ReferenceToken refToken) {      
      var attributes = new Dictionary<string, object> {
        {"ref", refToken.Id},
        {"name", refToken.Name}};                                       
      return Prefix + FormatNode(XmlStrings.REFERENCE, attributes, NodeType.Inline) + "\n";  
    }

    protected override string Format(NullReferenceToken nullRefToken) {      
      var attributes = new Dictionary<string, object>{
        {"name", nullRefToken.Name}};      
      return Prefix + FormatNode(XmlStrings.NULL, attributes, NodeType.Inline) + "\n";
    }

    public IEnumerable<string> Format(List<TypeMapping> typeCache) {
      yield return "<" + XmlStrings.TYPECACHE + ">";
      foreach (var mapping in typeCache)
        yield return "  " + FormatNode(
          XmlStrings.TYPE,
          mapping.GetDict(),
          NodeType.Inline,
          "\n    ");
      yield return "</" + XmlStrings.TYPECACHE + ">";
    }

    public static void Serialize(object o, string basename) {      
      Serialize(o, basename, ConfigurationService.Instance.GetDefaultConfig(XmlFormat.Instance));
    }

    public static void Serialize(object o, string basename, Configuration configuration) {
      Serializer s = new Serializer(o, configuration);
      XmlGenerator xmlGenerator = new XmlGenerator();
      StreamWriter writer = new StreamWriter(basename + ".xml");
      foreach (ISerializationToken token in s) {
        string line = xmlGenerator.Format(token);
        writer.Write(line);
        Console.Out.Write(line);
      }
      writer.Close();
      writer = new StreamWriter(basename + "-types.xml");
      foreach (string line in xmlGenerator.Format(s.TypeCache)) {
        writer.WriteLine(line);
        Console.Out.WriteLine(line);
      }
      writer.Close();      
    }

  }
}