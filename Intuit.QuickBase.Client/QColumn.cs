/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */

using System.Collections.Generic;
using System.Linq;
using Intuit.QuickBase.Core;
using Intuit.QuickBase.Core.Exceptions;

namespace Intuit.QuickBase.Client
{
    public class QColumn : IQColumn, IQColumn_int
    {
        internal class Choice
        {
            public object value;
            public bool inServer;

            internal Choice(object obj, bool inServ)
            {
                value = obj;
                inServer = inServ;
            }
        }

        // Constructors
        internal QColumn()
        {
            choices = new List<Choice>();
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

        internal QColumn(int columnId, string columnName, FieldType columnType, bool columnVirtual, bool columnLookup, bool columnSummary, bool isHidden)
            : this(columnName, columnType)
        {
            ColumnVirtual = columnVirtual;
            ColumnLookup = columnLookup;
            ColumnSummary = columnSummary;
            IsHidden = isHidden;
            ColumnId = columnId;
        }

        // Properties
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public FieldType ColumnType { get; set; }
        public bool ColumnVirtual { get; set; }
        public bool ColumnSummary { get; set; }
        public bool IsHidden { get; set; }
        public bool ColumnLookup { get; set; }
        internal List<Choice> choices;
        internal Dictionary<string,int> composites; 
        public string CurrencySymbol { get; set; }

        // Methods
        public bool Equals(IQColumn column)
        {
            if (column is null) return false;
            if (ReferenceEquals(this, column)) return true;
            return Equals(column.ColumnName, ColumnName);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (QColumn)) return false;
            return Equals((IQColumn) obj);
        }

        public override int GetHashCode()
        {
            return ColumnName != null ? ColumnName.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return ColumnName;
        }

        public List<object> GetChoices()
        {
            List<object> retVal = new List<object>(choices.Count);
            foreach (Choice ch in choices)
                retVal.Add(ch.value);
            return retVal;
        }

        public void AddChoice(object obj)
        {
            AddChoice(obj, false);
        }

        public void AddChoice(object obj, bool inServer)
        {
            Choice ch;
            switch (ColumnType)
            {
                case FieldType.rating:
                    if (obj.GetType() != typeof(int))
                        throw new InvalidChoiceException($"Column {ColumnName} presented with invalid type");
                    ch = new Choice(obj, inServer);
                    choices.Add(ch);
                    break;
                case FieldType.multitext:
                case FieldType.text:
                    if (obj.GetType() != typeof(string))
                        throw new InvalidChoiceException($"Column {ColumnName} presented with invalid type");
                    ch = new Choice(obj, inServer);
                    choices.Add(ch);
                    break;
                default:
                    throw new InvalidChoiceException($"Column {ColumnName} does not support choices.");
            }
        }

        public Dictionary<string,int> GetComposites()
        {
            return composites;
        }

        public void AcceptChanges(IQApplication Application, string tbid)
        {
            if (ColumnSummary || ColumnVirtual || ColumnLookup || choices.All(c => c.inServer)) return;
            
            List<string> changeList = new List<string>(choices.Count(c => c.inServer == false));
            foreach (Choice ch in choices)
            {
                if (!ch.inServer)
                {
                    changeList.Add(ch.value.ToString());
                    ch.inServer = true;
                }
            }
            var fac = new FieldAddChoices(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, tbid, ColumnId, changeList);
            var xml = fac.Post();
        }
    }
}
