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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HeuristicLab.Collections;
using HeuristicLab.Common;
using HeuristicLab.MainForm;

namespace HeuristicLab.Core.Views {
  public partial class ItemSetView<T> : ItemCollectionView<T> where T : class, IItem {
    public IObservableSet<T> ItemSet {
      get { return (IObservableSet<T>)Object; }
      set { base.Object = value; }
    }

    private Dictionary<T, ListViewItem> listViewItemDictionary;
    protected Dictionary<T, ListViewItem> ListViewItemDictionary {
      get { return listViewItemDictionary; }
    }

    public ItemSetView() {
      listViewItemDictionary = new Dictionary<T, ListViewItem>();
      InitializeComponent();
      Caption = "Item Set";
    }

    protected override void OnObjectChanged() {
      base.OnObjectChanged();
      Caption = "Item Set";
      if (ItemSet != null)
        Caption += " (" + ItemCollection.GetType().Name + ")";
    }

    protected override void AddListViewItem(ListViewItem listViewItem) {
      ListViewItemDictionary.Add((T)listViewItem.Tag, listViewItem);
      base.AddListViewItem(listViewItem);
    }
    protected override void RemoveListViewItem(ListViewItem listViewItem) {
      base.RemoveListViewItem(listViewItem);
      ListViewItemDictionary.Remove((T)listViewItem.Tag);
    }
    protected override IEnumerable<ListViewItem> GetListViewItemsForItem(T item) {
      return new ListViewItem[] { listViewItemDictionary[item] };
    }
  }
}
