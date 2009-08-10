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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace HeuristicLab.MainForm {
  public partial class SingleDocumentMainForm : MainFormBase {
    public SingleDocumentMainForm()
      : base() {
      InitializeComponent();
    }

    public SingleDocumentMainForm(Type userInterfaceItemType)
      : base(userInterfaceItemType) {
      InitializeComponent();
    }

    public override void ShowView(IView view) {
      if (InvokeRequired) Invoke((Action<IView>)ShowView, view);
      else {
        base.ShowView(view);
        DocumentForm form = new DocumentForm(view);
        form.Activated += new EventHandler(DockFormActivated);
        form.FormClosing += new FormClosingEventHandler(view.FormClosing);
        form.FormClosed += new FormClosedEventHandler(DockFormClosed);
        foreach (IToolStripItem item in ViewChangedToolStripItems)
          view.StateChanged += new EventHandler(item.ViewChanged);
        form.Show(this);
      }
    }

    private void DockFormClosed(object sender, FormClosedEventArgs e) {
      DocumentForm form = (DocumentForm)sender;
      views.Remove(form.View);
      if (views.Count == 0)
        ActiveView = null;
      form.Activated -= new EventHandler(DockFormActivated);
      form.FormClosing -= new FormClosingEventHandler(form.View.FormClosing);
      form.FormClosed -= new FormClosedEventHandler(DockFormClosed);
      foreach (IToolStripItem item in ViewChangedToolStripItems)
        form.View.StateChanged -= new EventHandler(item.ViewChanged);
    }

    private void DockFormActivated(object sender, EventArgs e) {
      base.ActiveView = ((DocumentForm)sender).View;
      base.StatusStripText = ((DocumentForm)sender).View.Caption;
    }
  }
}
