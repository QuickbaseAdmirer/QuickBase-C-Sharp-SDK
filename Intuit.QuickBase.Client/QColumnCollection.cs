/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Collections.Generic;
using Intuit.QuickBase.Core;

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

        public new void Add(IQColumn column)
        {
            if (column.ColumnId == 0)
            {
                var addCol = new AddField(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, Table.TableId, column.ColumnName, column.ColumnType);
                var xml = addCol.Post().CreateNavigator();
                var columnId = int.Parse(xml.SelectSingleNode("/qdbapi/fid").Value);
                column.ColumnId = columnId;
            }
            base.Add(column);
        }

        public new bool Remove(IQColumn column)
        {
            var deleteField = new DeleteField(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, Table.TableId, column.ColumnId);
            deleteField.Post();
            return base.Remove(column);
        }

        public new void RemoveAt(int index)
        {
            Remove(this[index]);
        }
    }
}
