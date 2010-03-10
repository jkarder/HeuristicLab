﻿using System;
using HeuristicLab.Persistence.Interfaces;
using HeuristicLab.Persistence.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System.Text;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Persistence.Default.CompositeSerializers {

  [EmptyStorableClass]
  public class CompactNumberArray2StringSerializer : ICompositeSerializer {

    public int Priority {
      get { return 200; }
    }

    private static readonly Number2StringSerializer numberConverter =
      new Number2StringSerializer();

    public bool CanSerialize(Type type) {
      return
        (type.IsArray || type == typeof(Array)) &&
        numberConverter.CanSerialize(type.GetElementType());
    }

    public string JustifyRejection(Type type) {
      if (!type.IsArray && type != typeof(Array))
        return "not an array";
      return string.Format("number converter cannot serialize elements: " +
        numberConverter.JustifyRejection(type.GetElementType()));
    }

    public IEnumerable<Tag> CreateMetaInfo(object obj) {
      Array a = (Array)obj;
      int[] lengths = new int[a.Rank];
      int[] lowerBounds = new int[a.Rank];
      StringBuilder sb = new StringBuilder();
      sb.Append(a.Rank).Append(';');
      for (int i = 0; i < a.Rank; i++) {
        sb.Append(a.GetLength(i)).Append(';');
        lengths[i] = a.GetLength(i);
      }
      for (int i = 0; i < a.Rank; i++) {
        sb.Append(a.GetLowerBound(i)).Append(';');
        lowerBounds[i] = a.GetLowerBound(i);
      }
      int[] positions = (int[])lowerBounds.Clone();
      while (positions[a.Rank - 1] < lengths[a.Rank - 1] + lowerBounds[a.Rank - 1]) {
        sb.Append(numberConverter.Format(a.GetValue(positions))).Append(';');
        positions[0] += 1;
        for (int i = 0; i < a.Rank - 1; i++) {
          if (positions[i] >= lengths[i] + lowerBounds[i]) {
            positions[i] = lowerBounds[i];
            positions[i + 1] += 1;
          } else {
            break;
          }
        }
      }
      yield return new Tag("compact array", sb.ToString());
    }

    public IEnumerable<Tag> Decompose(object obj) {
      return new Tag[] { };
    }

    public object CreateInstance(Type type, IEnumerable<Tag> metaInfo) {
      try {
        var tagIter = metaInfo.GetEnumerator();
        tagIter.MoveNext();
        var valueIter = ((string)tagIter.Current.Value)
          .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
          .GetEnumerator();
        valueIter.MoveNext();
        int rank = int.Parse((string)valueIter.Current);
        int[] lengths = new int[rank];
        int[] lowerBounds = new int[rank];
        for (int i = 0; i < rank; i++) {
          valueIter.MoveNext();
          lengths[i] = int.Parse((string)valueIter.Current);
        }
        for (int i = 0; i < rank; i++) {
          valueIter.MoveNext();
          lowerBounds[i] = int.Parse((string)valueIter.Current);
        }
        Type elementType = type.GetElementType();
        Array a = Array.CreateInstance(elementType, lengths, lowerBounds);
        int[] positions = (int[])lowerBounds.Clone();
        while (valueIter.MoveNext()) {
          a.SetValue(
            numberConverter.Parse((string)valueIter.Current, elementType),
            positions);
          positions[0] += 1;
          for (int i = 0; i < rank - 1; i++) {
            if (positions[i] >= lengths[i] + lowerBounds[i]) {
              positions[i + 1] += 1;
              positions[i] = lowerBounds[i];
            } else {
              break;
            }
          }
        }
        return a;
      } catch (InvalidOperationException e) {
        throw new PersistenceException("Insufficient data to deserialize compact array", e);
      } catch (InvalidCastException e) {
        throw new PersistenceException("Invalid element data during compact array deserialization", e);
      }
    }

    public void Populate(object instance, IEnumerable<Tag> tags, Type type) {
      // Nothing to do: Compact arrays are already populated during instance creation.
    }

  }

}