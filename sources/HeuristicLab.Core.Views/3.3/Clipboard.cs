#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HeuristicLab.MainForm;
using HeuristicLab.Persistence.Default.Xml;

namespace HeuristicLab.Core.Views {
  [View("Clipboard")]
  public sealed partial class Clipboard<T> : HeuristicLab.MainForm.WindowsForms.View where T : class, IItem {
    private TypeSelectorDialog typeSelectorDialog;
    private Dictionary<T, ListViewItem> itemListViewItemTable;

    private string itemsPath;
    public string ItemsPath {
      get { return itemsPath; }
      private set {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException(string.Format("Invalid items path \"{0}\".", value));
        itemsPath = value;
        try {
          if (!Directory.Exists(itemsPath)) {
            Directory.CreateDirectory(itemsPath);
            // directory creation might take some time -> wait until it is definitively created
            while (!Directory.Exists(itemsPath)) {
              Thread.Sleep(100);
              Directory.CreateDirectory(itemsPath);
            }
          }
        }
        catch (Exception ex) {
          throw new ArgumentException(string.Format("Invalid items path \"{0}\".", itemsPath), ex);
        }
      }
    }

    public Clipboard() {
      InitializeComponent();
      ItemsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                  Path.DirectorySeparatorChar + "HeuristicLab" + Path.DirectorySeparatorChar + "Clipboard";
      itemListViewItemTable = new Dictionary<T, ListViewItem>();
    }
    public Clipboard(string itemsPath) {
      InitializeComponent();
      ItemsPath = itemsPath;
      itemListViewItemTable = new Dictionary<T, ListViewItem>();
    }

    protected override void OnInitialized(EventArgs e) {
      base.OnInitialized(e);
      Enabled = false;
      infoLabel.Text = "Loading ...";
      progressBar.Value = 0;
      infoPanel.Visible = true;
      ThreadPool.QueueUserWorkItem(new WaitCallback(LoadItems));
    }
    protected override void OnClosing(FormClosingEventArgs e) {
      base.OnClosing(e);
      if (e.CloseReason == CloseReason.UserClosing) {
        e.Cancel = true;
        this.Hide();
      }
    }

    public void AddItem(T item) {
      if (InvokeRequired)
        Invoke(new Action<T>(AddItem), item);
      else {
        if (!itemListViewItemTable.ContainsKey(item)) {
          if (!listView.SmallImageList.Images.ContainsKey(item.GetType().FullName))
            listView.SmallImageList.Images.Add(item.GetType().FullName, item.ItemImage);

          ListViewItem listViewItem = new ListViewItem(item.ToString());
          listViewItem.ToolTipText = item.ItemName + ": " + item.ItemDescription;
          listViewItem.ImageIndex = listView.SmallImageList.Images.IndexOfKey(item.GetType().FullName);
          listViewItem.Tag = item;
          listView.Items.Add(listViewItem);
          itemListViewItemTable.Add(item, listViewItem);
          item.ToStringChanged += new EventHandler(Item_ToStringChanged);
          sortAscendingButton.Enabled = sortDescendingButton.Enabled = listView.Items.Count > 1;
          saveButton.Enabled = listView.Items.Count > 0;
        }
      }
    }
    private void RemoveItem(T item) {
      if (InvokeRequired)
        Invoke(new Action<T>(RemoveItem), item);
      else {
        if (itemListViewItemTable.ContainsKey(item)) {
          item.ToStringChanged -= new EventHandler(Item_ToStringChanged);
          itemListViewItemTable[item].Remove();
          itemListViewItemTable.Remove(item);
          sortAscendingButton.Enabled = sortDescendingButton.Enabled = listView.Items.Count > 1;
          saveButton.Enabled = listView.Items.Count > 0;
        }
      }
    }
    private void Save() {
      if (InvokeRequired)
        Invoke(new Action(Save));
      else {
        Enabled = false;
        infoLabel.Text = "Saving ...";
        progressBar.Value = 0;
        infoPanel.Visible = true;
        ThreadPool.QueueUserWorkItem(new WaitCallback(SaveItems));
      }
    }

    #region Loading/Saving Items
    private void LoadItems(object state) {
      string[] items = Directory.GetFiles(ItemsPath);
      foreach (string filename in items) {
        try {
          T item = XmlParser.Deserialize<T>(filename);
          OnItemLoaded(item, progressBar.Maximum / items.Length);
        }
        catch (Exception) { }
      }
      OnAllItemsLoaded();
    }
    private void OnItemLoaded(T item, int progress) {
      if (InvokeRequired)
        Invoke(new Action<T, int>(OnItemLoaded), item, progress);
      else {
        AddItem(item);
        progressBar.Value += progress;
      }
    }
    private void OnAllItemsLoaded() {
      if (InvokeRequired)
        Invoke(new Action(OnAllItemsLoaded));
      else {
        Enabled = true;
        if (listView.Items.Count > 0) {
          for (int i = 0; i < listView.Columns.Count; i++)
            listView.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        infoPanel.Visible = false;
      }
    }
    private void SaveItems(object param) {
      Directory.Delete(ItemsPath, true);
      Directory.CreateDirectory(ItemsPath);
      // directory creation might take some time -> wait until it is definitively created
      while (!Directory.Exists(ItemsPath)) {
        Thread.Sleep(100);
        Directory.CreateDirectory(ItemsPath);
      }

      int i = 0;
      T[] items = itemListViewItemTable.Keys.ToArray();
      foreach (T item in items) {
        try {
          i++;
          XmlGenerator.Serialize(item, ItemsPath + Path.DirectorySeparatorChar + i.ToString("00000000") + ".hl", 9);
          OnItemSaved(item, progressBar.Maximum / listView.Items.Count);
        }
        catch (Exception) { }
      }
      OnAllItemsSaved();
    }
    private void OnItemSaved(T item, int progress) {
      if (item != null) {
        if (InvokeRequired)
          Invoke(new Action<T, int>(OnItemLoaded), item, progress);
        else
          progressBar.Value += progress;
      }
    }
    private void OnAllItemsSaved() {
      if (InvokeRequired)
        Invoke(new Action(OnAllItemsLoaded));
      else {
        Enabled = true;
        infoPanel.Visible = false;
      }
    }
    #endregion

    #region ListView Events
    private void listView_SelectedIndexChanged(object sender, EventArgs e) {
      removeButton.Enabled = listView.SelectedItems.Count > 0;
    }
    private void listView_SizeChanged(object sender, EventArgs e) {
      if (listView.Columns.Count > 0)
        listView.Columns[0].Width = Math.Max(0, listView.Width - 25);
    }
    private void listView_KeyDown(object sender, KeyEventArgs e) {
      if (e.KeyCode == Keys.Delete) {
        if (listView.SelectedItems.Count > 0) {
          foreach (ListViewItem item in listView.SelectedItems)
            RemoveItem((T)item.Tag);
        }
      }
    }
    private void listView_DoubleClick(object sender, EventArgs e) {
      if (listView.SelectedItems.Count == 1) {
        T item = (T)listView.SelectedItems[0].Tag;
        IView view = MainFormManager.CreateDefaultView(item);
        if (view != null) view.Show();
      }
    }
    private void listView_ItemDrag(object sender, ItemDragEventArgs e) {
      ListViewItem listViewItem = (ListViewItem)e.Item;
      T item = (T)listViewItem.Tag;
      DataObject data = new DataObject();
      data.SetData("Type", item.GetType());
      data.SetData("Value", item);
      DragDropEffects result = DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move);
      if ((result & DragDropEffects.Move) == DragDropEffects.Move)
        RemoveItem(item);
    }
    private void listView_DragEnterOver(object sender, DragEventArgs e) {
      e.Effect = DragDropEffects.None;
      Type type = e.Data.GetData("Type") as Type;
      T item = e.Data.GetData("Value") as T;
      if ((type != null) && (item != null)) {
        if ((e.KeyState & 8) == 8) e.Effect = DragDropEffects.Copy;  // CTRL key
        else if (((e.KeyState & 4) == 4) && !itemListViewItemTable.ContainsKey(item)) e.Effect = DragDropEffects.Move;  // SHIFT key
        else if (((e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link) && !itemListViewItemTable.ContainsKey(item)) e.Effect = DragDropEffects.Link;
        else if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy) e.Effect = DragDropEffects.Copy;
        else if (((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move) && !itemListViewItemTable.ContainsKey(item)) e.Effect = DragDropEffects.Move;
      }
    }
    private void listView_DragDrop(object sender, DragEventArgs e) {
      if (e.Effect != DragDropEffects.None) {
        T item = e.Data.GetData("Value") as T;
        if ((e.Effect & DragDropEffects.Copy) == DragDropEffects.Copy) item = (T)item.Clone();
        AddItem(item);
      }
    }
    #endregion

    #region Button Events
    private void addButton_Click(object sender, EventArgs e) {
      if (typeSelectorDialog == null) {
        typeSelectorDialog = new TypeSelectorDialog();
        typeSelectorDialog.Caption = "Select Item";
        typeSelectorDialog.TypeSelector.Caption = "Available Items";
        typeSelectorDialog.TypeSelector.Configure(typeof(T), false, false);
      }

      if (typeSelectorDialog.ShowDialog(this) == DialogResult.OK)
        AddItem((T)typeSelectorDialog.TypeSelector.CreateInstanceOfSelectedType());
    }
    private void sortAscendingButton_Click(object sender, EventArgs e) {
      listView.Sorting = SortOrder.None;
      listView.Sorting = SortOrder.Ascending;
    }
    private void sortDescendingButton_Click(object sender, EventArgs e) {
      listView.Sorting = SortOrder.None;
      listView.Sorting = SortOrder.Descending;
    }
    private void removeButton_Click(object sender, EventArgs e) {
      if (listView.SelectedItems.Count > 0) {
        foreach (ListViewItem item in listView.SelectedItems)
          RemoveItem((T)item.Tag);
      }
    }
    private void saveButton_Click(object sender, EventArgs e) {
      Save();
    }
    #endregion

    #region Item Events
    private void Item_ToStringChanged(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Item_ToStringChanged), sender, e);
      else {
        T item = (T)sender;
        itemListViewItemTable[item].Text = item.ToString();
        listView.Sort();
      }
    }
    #endregion
  }
}
