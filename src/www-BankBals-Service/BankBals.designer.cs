﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace www.BankBals.Service
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="BankBals")]
	public partial class BankBalsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertA_AGG(A_AGG instance);
    partial void UpdateA_AGG(A_AGG instance);
    partial void DeleteA_AGG(A_AGG instance);
    partial void InsertW_AGG_COMP(W_AGG_COMP instance);
    partial void UpdateW_AGG_COMP(W_AGG_COMP instance);
    partial void DeleteW_AGG_COMP(W_AGG_COMP instance);
    partial void InsertA_BANK(A_BANK instance);
    partial void UpdateA_BANK(A_BANK instance);
    partial void DeleteA_BANK(A_BANK instance);
    partial void InsertA_DATE(A_DATE instance);
    partial void UpdateA_DATE(A_DATE instance);
    partial void DeleteA_DATE(A_DATE instance);
    #endregion
		
		public BankBalsDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["BankBalsConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public BankBalsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BankBalsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BankBalsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BankBalsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<A_AGG> A_AGGs
		{
			get
			{
				return this.GetTable<A_AGG>();
			}
		}
		
		public System.Data.Linq.Table<W_AGG_COMP> W_AGG_COMPs
		{
			get
			{
				return this.GetTable<W_AGG_COMP>();
			}
		}
		
		public System.Data.Linq.Table<A_BANK> A_BANKs
		{
			get
			{
				return this.GetTable<A_BANK>();
			}
		}
		
		public System.Data.Linq.Table<A_DATE> A_DATEs
		{
			get
			{
				return this.GetTable<A_DATE>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.A_AGGS")]
	public partial class A_AGG : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _AggID;
		
		private string _FullName;
		
		private string _FullNameRUS;
		
		private string _FullNameENG;
		
		private bool _IsHidden;
		
		private EntitySet<W_AGG_COMP> _W_AGG_COMPs;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnAggIDChanging(int value);
    partial void OnAggIDChanged();
    partial void OnFullNameChanging(string value);
    partial void OnFullNameChanged();
    partial void OnFullNameRUSChanging(string value);
    partial void OnFullNameRUSChanged();
    partial void OnFullNameENGChanging(string value);
    partial void OnFullNameENGChanged();
    partial void OnIsHiddenChanging(bool value);
    partial void OnIsHiddenChanged();
    #endregion
		
		public A_AGG()
		{
			this._W_AGG_COMPs = new EntitySet<W_AGG_COMP>(new Action<W_AGG_COMP>(this.attach_W_AGG_COMPs), new Action<W_AGG_COMP>(this.detach_W_AGG_COMPs));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AggID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int AggID
		{
			get
			{
				return this._AggID;
			}
			set
			{
				if ((this._AggID != value))
				{
					this.OnAggIDChanging(value);
					this.SendPropertyChanging();
					this._AggID = value;
					this.SendPropertyChanged("AggID");
					this.OnAggIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullName", DbType="NVarChar(150) NOT NULL", CanBeNull=false)]
		public string FullName
		{
			get
			{
				return this._FullName;
			}
			set
			{
				if ((this._FullName != value))
				{
					this.OnFullNameChanging(value);
					this.SendPropertyChanging();
					this._FullName = value;
					this.SendPropertyChanged("FullName");
					this.OnFullNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullNameRUS", DbType="NVarChar(150) NOT NULL", CanBeNull=false)]
		public string FullNameRUS
		{
			get
			{
				return this._FullNameRUS;
			}
			set
			{
				if ((this._FullNameRUS != value))
				{
					this.OnFullNameRUSChanging(value);
					this.SendPropertyChanging();
					this._FullNameRUS = value;
					this.SendPropertyChanged("FullNameRUS");
					this.OnFullNameRUSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullNameENG", DbType="NVarChar(150) NOT NULL", CanBeNull=false)]
		public string FullNameENG
		{
			get
			{
				return this._FullNameENG;
			}
			set
			{
				if ((this._FullNameENG != value))
				{
					this.OnFullNameENGChanging(value);
					this.SendPropertyChanging();
					this._FullNameENG = value;
					this.SendPropertyChanged("FullNameENG");
					this.OnFullNameENGChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsHidden", DbType="Bit NOT NULL")]
		public bool IsHidden
		{
			get
			{
				return this._IsHidden;
			}
			set
			{
				if ((this._IsHidden != value))
				{
					this.OnIsHiddenChanging(value);
					this.SendPropertyChanging();
					this._IsHidden = value;
					this.SendPropertyChanged("IsHidden");
					this.OnIsHiddenChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="A_AGG_W_AGG_COMP", Storage="_W_AGG_COMPs", ThisKey="AggID", OtherKey="AggID")]
		public EntitySet<W_AGG_COMP> W_AGG_COMPs
		{
			get
			{
				return this._W_AGG_COMPs;
			}
			set
			{
				this._W_AGG_COMPs.Assign(value);
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
		
		private void attach_W_AGG_COMPs(W_AGG_COMP entity)
		{
			this.SendPropertyChanging();
			entity.A_AGG = this;
		}
		
		private void detach_W_AGG_COMPs(W_AGG_COMP entity)
		{
			this.SendPropertyChanging();
			entity.A_AGG = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.W_AGG_COMP")]
	public partial class W_AGG_COMP : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _AggID;
		
		private int _BankID;
		
		private bool _F101IsWithTO;
		
		private bool _F134;
		
		private EntityRef<A_AGG> _A_AGG;
		
		private EntityRef<A_BANK> _A_BANK;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnAggIDChanging(int value);
    partial void OnAggIDChanged();
    partial void OnBankIDChanging(int value);
    partial void OnBankIDChanged();
    partial void OnF101IsWithTOChanging(bool value);
    partial void OnF101IsWithTOChanged();
    partial void OnF134Changing(bool value);
    partial void OnF134Changed();
    #endregion
		
		public W_AGG_COMP()
		{
			this._A_AGG = default(EntityRef<A_AGG>);
			this._A_BANK = default(EntityRef<A_BANK>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AggID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int AggID
		{
			get
			{
				return this._AggID;
			}
			set
			{
				if ((this._AggID != value))
				{
					if (this._A_AGG.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnAggIDChanging(value);
					this.SendPropertyChanging();
					this._AggID = value;
					this.SendPropertyChanged("AggID");
					this.OnAggIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BankID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int BankID
		{
			get
			{
				return this._BankID;
			}
			set
			{
				if ((this._BankID != value))
				{
					if (this._A_BANK.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnBankIDChanging(value);
					this.SendPropertyChanging();
					this._BankID = value;
					this.SendPropertyChanged("BankID");
					this.OnBankIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_F101IsWithTO", DbType="Bit NOT NULL")]
		public bool F101IsWithTO
		{
			get
			{
				return this._F101IsWithTO;
			}
			set
			{
				if ((this._F101IsWithTO != value))
				{
					this.OnF101IsWithTOChanging(value);
					this.SendPropertyChanging();
					this._F101IsWithTO = value;
					this.SendPropertyChanged("F101IsWithTO");
					this.OnF101IsWithTOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_F134", DbType="Bit NOT NULL")]
		public bool F134
		{
			get
			{
				return this._F134;
			}
			set
			{
				if ((this._F134 != value))
				{
					this.OnF134Changing(value);
					this.SendPropertyChanging();
					this._F134 = value;
					this.SendPropertyChanged("F134");
					this.OnF134Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="A_AGG_W_AGG_COMP", Storage="_A_AGG", ThisKey="AggID", OtherKey="AggID", IsForeignKey=true)]
		public A_AGG A_AGG
		{
			get
			{
				return this._A_AGG.Entity;
			}
			set
			{
				A_AGG previousValue = this._A_AGG.Entity;
				if (((previousValue != value) 
							|| (this._A_AGG.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._A_AGG.Entity = null;
						previousValue.W_AGG_COMPs.Remove(this);
					}
					this._A_AGG.Entity = value;
					if ((value != null))
					{
						value.W_AGG_COMPs.Add(this);
						this._AggID = value.AggID;
					}
					else
					{
						this._AggID = default(int);
					}
					this.SendPropertyChanged("A_AGG");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="A_BANK_W_AGG_COMP", Storage="_A_BANK", ThisKey="BankID", OtherKey="BankID", IsForeignKey=true)]
		public A_BANK A_BANK
		{
			get
			{
				return this._A_BANK.Entity;
			}
			set
			{
				A_BANK previousValue = this._A_BANK.Entity;
				if (((previousValue != value) 
							|| (this._A_BANK.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._A_BANK.Entity = null;
						previousValue.W_AGG_COMPs.Remove(this);
					}
					this._A_BANK.Entity = value;
					if ((value != null))
					{
						value.W_AGG_COMPs.Add(this);
						this._BankID = value.BankID;
					}
					else
					{
						this._BankID = default(int);
					}
					this.SendPropertyChanged("A_BANK");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.A_BANKS")]
	public partial class A_BANK : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _BankID;
		
		private string _FullName;
		
		private string _FullNameRUS;
		
		private string _FullNameENG;
		
		private int _BankTTypeID;
		
		private bool _IsHidden;
		
		private bool _IsHaveData;
		
		private System.Nullable<int> _IssuerID;
		
		private EntitySet<W_AGG_COMP> _W_AGG_COMPs;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnBankIDChanging(int value);
    partial void OnBankIDChanged();
    partial void OnFullNameChanging(string value);
    partial void OnFullNameChanged();
    partial void OnFullNameRUSChanging(string value);
    partial void OnFullNameRUSChanged();
    partial void OnFullNameENGChanging(string value);
    partial void OnFullNameENGChanged();
    partial void OnBankTTypeIDChanging(int value);
    partial void OnBankTTypeIDChanged();
    partial void OnIsHiddenChanging(bool value);
    partial void OnIsHiddenChanged();
    partial void OnIsHaveDataChanging(bool value);
    partial void OnIsHaveDataChanged();
    partial void OnIssuerIDChanging(System.Nullable<int> value);
    partial void OnIssuerIDChanged();
    #endregion
		
		public A_BANK()
		{
			this._W_AGG_COMPs = new EntitySet<W_AGG_COMP>(new Action<W_AGG_COMP>(this.attach_W_AGG_COMPs), new Action<W_AGG_COMP>(this.detach_W_AGG_COMPs));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BankID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int BankID
		{
			get
			{
				return this._BankID;
			}
			set
			{
				if ((this._BankID != value))
				{
					this.OnBankIDChanging(value);
					this.SendPropertyChanging();
					this._BankID = value;
					this.SendPropertyChanged("BankID");
					this.OnBankIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullName", DbType="NVarChar(150)")]
		public string FullName
		{
			get
			{
				return this._FullName;
			}
			set
			{
				if ((this._FullName != value))
				{
					this.OnFullNameChanging(value);
					this.SendPropertyChanging();
					this._FullName = value;
					this.SendPropertyChanged("FullName");
					this.OnFullNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullNameRUS", DbType="NVarChar(150)")]
		public string FullNameRUS
		{
			get
			{
				return this._FullNameRUS;
			}
			set
			{
				if ((this._FullNameRUS != value))
				{
					this.OnFullNameRUSChanging(value);
					this.SendPropertyChanging();
					this._FullNameRUS = value;
					this.SendPropertyChanged("FullNameRUS");
					this.OnFullNameRUSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullNameENG", DbType="NVarChar(150)")]
		public string FullNameENG
		{
			get
			{
				return this._FullNameENG;
			}
			set
			{
				if ((this._FullNameENG != value))
				{
					this.OnFullNameENGChanging(value);
					this.SendPropertyChanging();
					this._FullNameENG = value;
					this.SendPropertyChanged("FullNameENG");
					this.OnFullNameENGChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BankTTypeID", DbType="Int NOT NULL")]
		public int BankTTypeID
		{
			get
			{
				return this._BankTTypeID;
			}
			set
			{
				if ((this._BankTTypeID != value))
				{
					this.OnBankTTypeIDChanging(value);
					this.SendPropertyChanging();
					this._BankTTypeID = value;
					this.SendPropertyChanged("BankTTypeID");
					this.OnBankTTypeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsHidden", DbType="Bit NOT NULL")]
		public bool IsHidden
		{
			get
			{
				return this._IsHidden;
			}
			set
			{
				if ((this._IsHidden != value))
				{
					this.OnIsHiddenChanging(value);
					this.SendPropertyChanging();
					this._IsHidden = value;
					this.SendPropertyChanged("IsHidden");
					this.OnIsHiddenChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsHaveData", DbType="Bit NOT NULL")]
		public bool IsHaveData
		{
			get
			{
				return this._IsHaveData;
			}
			set
			{
				if ((this._IsHaveData != value))
				{
					this.OnIsHaveDataChanging(value);
					this.SendPropertyChanging();
					this._IsHaveData = value;
					this.SendPropertyChanged("IsHaveData");
					this.OnIsHaveDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IssuerID", DbType="Int")]
		public System.Nullable<int> IssuerID
		{
			get
			{
				return this._IssuerID;
			}
			set
			{
				if ((this._IssuerID != value))
				{
					this.OnIssuerIDChanging(value);
					this.SendPropertyChanging();
					this._IssuerID = value;
					this.SendPropertyChanged("IssuerID");
					this.OnIssuerIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="A_BANK_W_AGG_COMP", Storage="_W_AGG_COMPs", ThisKey="BankID", OtherKey="BankID")]
		public EntitySet<W_AGG_COMP> W_AGG_COMPs
		{
			get
			{
				return this._W_AGG_COMPs;
			}
			set
			{
				this._W_AGG_COMPs.Assign(value);
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
		
		private void attach_W_AGG_COMPs(W_AGG_COMP entity)
		{
			this.SendPropertyChanging();
			entity.A_BANK = this;
		}
		
		private void detach_W_AGG_COMPs(W_AGG_COMP entity)
		{
			this.SendPropertyChanging();
			entity.A_BANK = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.A_DATE")]
	public partial class A_DATE : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _DateID;
		
		private System.DateTime _Date;
		
		private bool _IsQuartal;
		
		private bool _IsVisible;
		
		private bool _F101;
		
		private bool _F102;
		
		private bool _F123;
		
		private bool _F134;
		
		private bool _F135;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDateIDChanging(int value);
    partial void OnDateIDChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    partial void OnIsQuartalChanging(bool value);
    partial void OnIsQuartalChanged();
    partial void OnIsVisibleChanging(bool value);
    partial void OnIsVisibleChanged();
    partial void OnF101Changing(bool value);
    partial void OnF101Changed();
    partial void OnF102Changing(bool value);
    partial void OnF102Changed();
    partial void OnF123Changing(bool value);
    partial void OnF123Changed();
    partial void OnF134Changing(bool value);
    partial void OnF134Changed();
    partial void OnF135Changing(bool value);
    partial void OnF135Changed();
    #endregion
		
		public A_DATE()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int DateID
		{
			get
			{
				return this._DateID;
			}
			set
			{
				if ((this._DateID != value))
				{
					this.OnDateIDChanging(value);
					this.SendPropertyChanging();
					this._DateID = value;
					this.SendPropertyChanged("DateID");
					this.OnDateIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="Date NOT NULL")]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsQuartal", DbType="Bit NOT NULL")]
		public bool IsQuartal
		{
			get
			{
				return this._IsQuartal;
			}
			set
			{
				if ((this._IsQuartal != value))
				{
					this.OnIsQuartalChanging(value);
					this.SendPropertyChanging();
					this._IsQuartal = value;
					this.SendPropertyChanged("IsQuartal");
					this.OnIsQuartalChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsVisible", DbType="Bit NOT NULL")]
		public bool IsVisible
		{
			get
			{
				return this._IsVisible;
			}
			set
			{
				if ((this._IsVisible != value))
				{
					this.OnIsVisibleChanging(value);
					this.SendPropertyChanging();
					this._IsVisible = value;
					this.SendPropertyChanged("IsVisible");
					this.OnIsVisibleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_F101", DbType="Bit NOT NULL")]
		public bool F101
		{
			get
			{
				return this._F101;
			}
			set
			{
				if ((this._F101 != value))
				{
					this.OnF101Changing(value);
					this.SendPropertyChanging();
					this._F101 = value;
					this.SendPropertyChanged("F101");
					this.OnF101Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_F102", DbType="Bit NOT NULL")]
		public bool F102
		{
			get
			{
				return this._F102;
			}
			set
			{
				if ((this._F102 != value))
				{
					this.OnF102Changing(value);
					this.SendPropertyChanging();
					this._F102 = value;
					this.SendPropertyChanged("F102");
					this.OnF102Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_F123", DbType="Bit NOT NULL")]
		public bool F123
		{
			get
			{
				return this._F123;
			}
			set
			{
				if ((this._F123 != value))
				{
					this.OnF123Changing(value);
					this.SendPropertyChanging();
					this._F123 = value;
					this.SendPropertyChanged("F123");
					this.OnF123Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_F134", DbType="Bit NOT NULL")]
		public bool F134
		{
			get
			{
				return this._F134;
			}
			set
			{
				if ((this._F134 != value))
				{
					this.OnF134Changing(value);
					this.SendPropertyChanging();
					this._F134 = value;
					this.SendPropertyChanged("F134");
					this.OnF134Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_F135", DbType="Bit NOT NULL")]
		public bool F135
		{
			get
			{
				return this._F135;
			}
			set
			{
				if ((this._F135 != value))
				{
					this.OnF135Changing(value);
					this.SendPropertyChanging();
					this._F135 = value;
					this.SendPropertyChanged("F135");
					this.OnF135Changed();
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
