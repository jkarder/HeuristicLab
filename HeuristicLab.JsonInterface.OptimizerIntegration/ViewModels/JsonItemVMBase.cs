﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeuristicLab.JsonInterface.OptimizerIntegration {
  public abstract class JsonItemVMBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
    public IJsonItem Item { get; set; }

    protected void OnPropertyChange(object sender, string propertyName) {
      // Make a temporary copy of the event to avoid possibility of
      // a race condition if the last subscriber unsubscribes
      // immediately after the null check and before the event is raised.
      // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-raise-base-class-events-in-derived-classes
      
      PropertyChangedEventHandler tmp = PropertyChanged;
      tmp?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual Type JsonItemType => typeof(JsonItem);

    //protected IJsonItemValueParser Parser { get; set; }
    //child tree
    //private IList<JsonItemVM> nodes = new List<JsonItemVM>();

    //public IEnumerable<JsonItemVM> Nodes { get => nodes; }
    //public JsonItemVM Parent { get; private set; }


    private bool selected = true;
    public bool Selected {
      get => selected;
      set {
        selected = value;
        OnPropertyChange(this, nameof(Selected));
      }
    }

    public string Name {
      get => Item.Name;
      set {
        Item.Name = value;
        OnPropertyChange(this, nameof(Name));
      }
    }

    public string ActualName {
      get => Item.ActualName;
      set {
        Item.ActualName = value;
        OnPropertyChange(this, nameof(ActualName));
      }
    }
    public virtual JsonItemBaseControl GetControl() {
      return new JsonItemBaseControl(this);
    }

  }
}