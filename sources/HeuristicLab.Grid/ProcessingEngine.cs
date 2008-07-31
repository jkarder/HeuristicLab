#region License Information
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
using System.Linq;
using System.Text;
using HeuristicLab.Core;
using System.Xml;
using System.Threading;
using System.Diagnostics;

namespace HeuristicLab.Grid {
  public class ProcessingEngine : EngineBase {
    private AtomicOperation initialOperation;
    public AtomicOperation InitialOperation {
      get { return initialOperation; }
    }

    private string errorMessage;
    public string ErrorMessage {
      get { return errorMessage; }
    }

    public ProcessingEngine()
      : base() {
    }

    public ProcessingEngine(IScope globalScope, AtomicOperation initialOperation)
      : base() {
      this.initialOperation = initialOperation;
      myGlobalScope = globalScope;
      myExecutionStack.Push(initialOperation);
    }
    public override XmlNode GetXmlNode(string name, XmlDocument document, IDictionary<Guid, IStorable> persistedObjects) {
      XmlNode node = base.GetXmlNode(name, document, persistedObjects);
      XmlAttribute canceledAttr = document.CreateAttribute("Canceled");
      canceledAttr.Value = Canceled.ToString();
      node.Attributes.Append(canceledAttr);
      if(errorMessage != null) {
        XmlAttribute errorMessageAttr = document.CreateAttribute("ErrorMessage");
        errorMessageAttr.Value = ErrorMessage;
        node.Attributes.Append(errorMessageAttr);
      }
      node.AppendChild(PersistenceManager.Persist("InitialOperation", initialOperation, document, persistedObjects));
      return node;
    }

    public override void Populate(XmlNode node, IDictionary<Guid, IStorable> restoredObjects) {
      base.Populate(node, restoredObjects);
      myCanceled = bool.Parse(node.Attributes["Canceled"].Value);
      if(node.Attributes["ErrorMessage"] != null) errorMessage = node.Attributes["ErrorMessage"].Value;
      initialOperation = (AtomicOperation)PersistenceManager.Restore(node.SelectSingleNode("InitialOperation"), restoredObjects);
    }

    protected override void ProcessNextOperation() {
      IOperation operation = myExecutionStack.Pop();
      if(operation is AtomicOperation) {
        AtomicOperation atomicOperation = (AtomicOperation)operation;
        IOperation next = null;
        try {
          next = atomicOperation.Operator.Execute(atomicOperation.Scope);
        } catch(Exception ex) {
          errorMessage = CreateErrorMessage(ex);
          Trace.TraceWarning(errorMessage);
          // push operation on stack again
          myExecutionStack.Push(atomicOperation);
          Abort();
        }
        if(next != null)
          myExecutionStack.Push(next);
        if(atomicOperation.Operator.Breakpoint) Abort();
      } else if(operation is CompositeOperation) {
        CompositeOperation compositeOperation = (CompositeOperation)operation;
        for(int i = compositeOperation.Operations.Count - 1; i >= 0; i--)
          myExecutionStack.Push(compositeOperation.Operations[i]);
      }
    }

    private string CreateErrorMessage(Exception ex) {
      StringBuilder sb = new StringBuilder();
      sb.Append("Sorry, but something went wrong!\n\n" + ex.Message + "\n\n" + ex.StackTrace);

      while(ex.InnerException != null) {
        ex = ex.InnerException;
        sb.Append("\n\n-----\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
      return sb.ToString();
    }
  }
}
