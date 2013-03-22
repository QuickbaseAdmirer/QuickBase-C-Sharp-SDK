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
    // TODO: May want to compose List<IQRecord> to hide the complexity of the List type.
    public sealed class QRecordCollection : List<IQRecord>
    {
        private readonly List<int> _recordsToRemove = new List<int>();

        public QRecordCollection(IQApplication application, IQTable table)
        {
            Application = application;
            Table = table;
        }

        private IQApplication Application { get; set; }
        private IQTable Table { get; set; }

        public new bool Remove(IQRecord record)
        {
            if (record.IsOnServer)
            {
                _recordsToRemove.Add(record.RecordId);
            }
            return base.Remove(record);
        }

        internal void RemoveRecords()
        {
            foreach (var recordId in _recordsToRemove)
            {
                var deleteRecord = new DeleteRecord(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, Table.TableId, recordId);
                deleteRecord.Post();
            }
        }
    }
}
