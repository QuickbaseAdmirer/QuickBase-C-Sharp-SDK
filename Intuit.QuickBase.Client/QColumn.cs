/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */

using System.Collections.Generic;
using Intuit.QuickBase.Core;

namespace Intuit.QuickBase.Client
{
    public class QColumn : IQColumn, IQColumn_int
    {
        // Constructors
        internal QColumn()
        {
            choices = new List<object>();
            composites = new Dictionary<string, int>();
        }

        public QColumn(string columnName, FieldType columnType) : this()
        {
            ColumnName = columnName;
            ColumnType = columnType;
        }

        internal QColumn(int columnId, string columnName, FieldType columnType)
            : this(columnName, columnType)
        {
            ColumnId = columnId;
        }

        internal QColumn(int columnId, string columnName, FieldType columnType, bool columnVirtual, bool columnLookup, bool isHidden)
            : this(columnName, columnType)
        {
            ColumnVirtual = columnVirtual;
            ColumnLookup = columnLookup;
            IsHidden = isHidden;
            ColumnId = columnId;
        }

        // Properties
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public FieldType ColumnType { get; set; }
        public bool ColumnVirtual { get; set; }
        public bool IsHidden { get; set; }
        public bool ColumnLookup { get; set; }
        internal List<object> choices;
        internal Dictionary<string,int> composites; 
        public string CurrencySymbol { get; set; }

        // Methods
        public bool Equals(IQColumn column)
        {
            if (ReferenceEquals(null, column)) return false;
            if (ReferenceEquals(this, column)) return true;
            return Equals(column.ColumnName, ColumnName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (QColumn)) return false;
            return Equals((IQColumn) obj);
        }

        public override int GetHashCode()
        {
            return (ColumnName != null ? ColumnName.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return ColumnName;
        }

        public object[] GetChoices()
        {
            return choices.ToArray();
        }

        public void AddChoice(object obj)
        {
            choices.Add(obj);
        }

        public Dictionary<string,int> GetComposites()
        {
            return composites;
        }
    }
}
