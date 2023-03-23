/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Collections.Generic;
using System.Xml.Linq;
using Intuit.QuickBase.Core;
using Intuit.QuickBase.Core.Exceptions;

namespace Intuit.QuickBase.Client
{
    // TODO: May want to compose List<IQColumn> to hide the complexity of the List type.
    public sealed class QColumnCollection : List<IQColumn>
    {
        public QColumnCollection(IQApplication application, IQTable table)
        {
            Application = application;
            Table = table;
        }

        private IQApplication Application { get; set; }
        private IQTable Table { get; set; }

        public IQColumn this[string columnName]
        {
            get
            {
                int index = base.IndexOf(new QColumn { ColumnName = columnName });
                if (index == -1)
                {
                    throw new ColumnDoesNotExistInTableException($"Column '{columnName}' not found in table.");
                }

                return base[index];

            }
        }


        public new void Add(IQColumn column)
        {
            if (column.ColumnId == 0)
            {
                AddField addCol = new AddField(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, Table.TableId, column.ColumnName, column.ColumnType);
                XElement xml = addCol.Post();
                Http.CheckForException(xml);
                int columnId = int.Parse(xml.Element("fid").Value);
                column.ColumnId = columnId;
            }

            if (column.ColumnType == FieldType.multitext) column.CanAddChoices = true;
            base.Add(column);
        }

        public new bool Remove(IQColumn column)
        {
            DeleteField deleteField = new DeleteField(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, Table.TableId, column.ColumnId);
            deleteField.Post();
            return base.Remove(column);
        }

        public new void RemoveAt(int index)
        {
            Remove(this[index]);
        }
    }
}
