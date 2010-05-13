﻿#region License Information
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

#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4200
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HeuristicLab.Services.Deployment.DataAccess
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="HeuristicLab.PluginStore")]
	public partial class PluginStoreClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertDependency(Dependency instance);
    partial void UpdateDependency(Dependency instance);
    partial void DeleteDependency(Dependency instance);
    partial void InsertProductPlugin(ProductPlugin instance);
    partial void UpdateProductPlugin(ProductPlugin instance);
    partial void DeleteProductPlugin(ProductPlugin instance);
    partial void InsertPlugin(Plugin instance);
    partial void UpdatePlugin(Plugin instance);
    partial void DeletePlugin(Plugin instance);
    partial void InsertPluginPackage(PluginPackage instance);
    partial void UpdatePluginPackage(PluginPackage instance);
    partial void DeletePluginPackage(PluginPackage instance);
    partial void InsertProduct(Product instance);
    partial void UpdateProduct(Product instance);
    partial void DeleteProduct(Product instance);
    #endregion
		
		public PluginStoreClassesDataContext() : 
				base(global::HeuristicLab.Services.Deployment.DataAccess.Properties.Settings.Default.HeuristicLab_PluginStoreConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PluginStoreClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PluginStoreClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PluginStoreClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PluginStoreClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Dependency> Dependencies
		{
			get
			{
				return this.GetTable<Dependency>();
			}
		}
		
		public System.Data.Linq.Table<ProductPlugin> ProductPlugins
		{
			get
			{
				return this.GetTable<ProductPlugin>();
			}
		}
		
		public System.Data.Linq.Table<Plugin> Plugins
		{
			get
			{
				return this.GetTable<Plugin>();
			}
		}
		
		public System.Data.Linq.Table<PluginPackage> PluginPackages
		{
			get
			{
				return this.GetTable<PluginPackage>();
			}
		}
		
		public System.Data.Linq.Table<Product> Products
		{
			get
			{
				return this.GetTable<Product>();
			}
		}
	}
	
	[Table(Name="dbo.Dependencies")]
	public partial class Dependency : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _PluginId;
		
		private long _DependencyId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPluginIdChanging(long value);
    partial void OnPluginIdChanged();
    partial void OnDependencyIdChanging(long value);
    partial void OnDependencyIdChanged();
    #endregion
		
		public Dependency()
		{
			OnCreated();
		}
		
		[Column(Storage="_PluginId", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long PluginId
		{
			get
			{
				return this._PluginId;
			}
			set
			{
				if ((this._PluginId != value))
				{
					this.OnPluginIdChanging(value);
					this.SendPropertyChanging();
					this._PluginId = value;
					this.SendPropertyChanged("PluginId");
					this.OnPluginIdChanged();
				}
			}
		}
		
		[Column(Storage="_DependencyId", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long DependencyId
		{
			get
			{
				return this._DependencyId;
			}
			set
			{
				if ((this._DependencyId != value))
				{
					this.OnDependencyIdChanging(value);
					this.SendPropertyChanging();
					this._DependencyId = value;
					this.SendPropertyChanged("DependencyId");
					this.OnDependencyIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.ProductPlugin")]
	public partial class ProductPlugin : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _ProductId;
		
		private long _PluginId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnProductIdChanging(long value);
    partial void OnProductIdChanged();
    partial void OnPluginIdChanging(long value);
    partial void OnPluginIdChanged();
    #endregion
		
		public ProductPlugin()
		{
			OnCreated();
		}
		
		[Column(Storage="_ProductId", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long ProductId
		{
			get
			{
				return this._ProductId;
			}
			set
			{
				if ((this._ProductId != value))
				{
					this.OnProductIdChanging(value);
					this.SendPropertyChanging();
					this._ProductId = value;
					this.SendPropertyChanged("ProductId");
					this.OnProductIdChanged();
				}
			}
		}
		
		[Column(Storage="_PluginId", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long PluginId
		{
			get
			{
				return this._PluginId;
			}
			set
			{
				if ((this._PluginId != value))
				{
					this.OnPluginIdChanging(value);
					this.SendPropertyChanging();
					this._PluginId = value;
					this.SendPropertyChanged("PluginId");
					this.OnPluginIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Plugin")]
	public partial class Plugin : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Name;
		
		private string _Version;
		
		private string _ContactName;
		
		private string _ContactEmail;
		
		private string _License;
		
		private EntityRef<PluginPackage> _PluginPackage;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnVersionChanging(string value);
    partial void OnVersionChanged();
    partial void OnContactNameChanging(string value);
    partial void OnContactNameChanged();
    partial void OnContactEmailChanging(string value);
    partial void OnContactEmailChanged();
    partial void OnLicenseChanging(string value);
    partial void OnLicenseChanged();
    #endregion
		
		public Plugin()
		{
			this._PluginPackage = default(EntityRef<PluginPackage>);
			OnCreated();
		}
		
		[Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="NVarChar(300) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_Version", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				if ((this._Version != value))
				{
					this.OnVersionChanging(value);
					this.SendPropertyChanging();
					this._Version = value;
					this.SendPropertyChanged("Version");
					this.OnVersionChanged();
				}
			}
		}
		
		[Column(Storage="_ContactName", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string ContactName
		{
			get
			{
				return this._ContactName;
			}
			set
			{
				if ((this._ContactName != value))
				{
					this.OnContactNameChanging(value);
					this.SendPropertyChanging();
					this._ContactName = value;
					this.SendPropertyChanged("ContactName");
					this.OnContactNameChanged();
				}
			}
		}
		
		[Column(Storage="_ContactEmail", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string ContactEmail
		{
			get
			{
				return this._ContactEmail;
			}
			set
			{
				if ((this._ContactEmail != value))
				{
					this.OnContactEmailChanging(value);
					this.SendPropertyChanging();
					this._ContactEmail = value;
					this.SendPropertyChanged("ContactEmail");
					this.OnContactEmailChanged();
				}
			}
		}
		
		[Column(Storage="_License", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string License
		{
			get
			{
				return this._License;
			}
			set
			{
				if ((this._License != value))
				{
					this.OnLicenseChanging(value);
					this.SendPropertyChanging();
					this._License = value;
					this.SendPropertyChanged("License");
					this.OnLicenseChanged();
				}
			}
		}
		
		[Association(Name="Plugin_PluginPackage", Storage="_PluginPackage", ThisKey="Id", OtherKey="PluginId", IsUnique=true, IsForeignKey=false)]
		public PluginPackage PluginPackage
		{
			get
			{
				return this._PluginPackage.Entity;
			}
			set
			{
				PluginPackage previousValue = this._PluginPackage.Entity;
				if (((previousValue != value) 
							|| (this._PluginPackage.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._PluginPackage.Entity = null;
						previousValue.Plugin = null;
					}
					this._PluginPackage.Entity = value;
					if ((value != null))
					{
						value.Plugin = this;
					}
					this.SendPropertyChanged("PluginPackage");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.PluginPackage")]
	public partial class PluginPackage : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _PluginId;
		
		private System.Data.Linq.Binary _Data;
		
		private EntityRef<Plugin> _Plugin;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPluginIdChanging(long value);
    partial void OnPluginIdChanged();
    partial void OnDataChanging(System.Data.Linq.Binary value);
    partial void OnDataChanged();
    #endregion
		
		public PluginPackage()
		{
			this._Plugin = default(EntityRef<Plugin>);
			OnCreated();
		}
		
		[Column(Storage="_PluginId", DbType="BigInt NOT NULL", IsPrimaryKey=true)]
		public long PluginId
		{
			get
			{
				return this._PluginId;
			}
			set
			{
				if ((this._PluginId != value))
				{
					if (this._Plugin.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnPluginIdChanging(value);
					this.SendPropertyChanging();
					this._PluginId = value;
					this.SendPropertyChanged("PluginId");
					this.OnPluginIdChanged();
				}
			}
		}
		
		[Column(Storage="_Data", DbType="Image NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				if ((this._Data != value))
				{
					this.OnDataChanging(value);
					this.SendPropertyChanging();
					this._Data = value;
					this.SendPropertyChanged("Data");
					this.OnDataChanged();
				}
			}
		}
		
		[Association(Name="Plugin_PluginPackage", Storage="_Plugin", ThisKey="PluginId", OtherKey="Id", IsForeignKey=true)]
		public Plugin Plugin
		{
			get
			{
				return this._Plugin.Entity;
			}
			set
			{
				Plugin previousValue = this._Plugin.Entity;
				if (((previousValue != value) 
							|| (this._Plugin.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Plugin.Entity = null;
						previousValue.PluginPackage = null;
					}
					this._Plugin.Entity = value;
					if ((value != null))
					{
						value.PluginPackage = this;
						this._PluginId = value.Id;
					}
					else
					{
						this._PluginId = default(long);
					}
					this.SendPropertyChanged("Plugin");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Product")]
	public partial class Product : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Name;
		
		private string _Version;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnVersionChanging(string value);
    partial void OnVersionChanged();
    #endregion
		
		public Product()
		{
			OnCreated();
		}
		
		[Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="NVarChar(300) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_Version", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				if ((this._Version != value))
				{
					this.OnVersionChanging(value);
					this.SendPropertyChanging();
					this._Version = value;
					this.SendPropertyChanged("Version");
					this.OnVersionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
