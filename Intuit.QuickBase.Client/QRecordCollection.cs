/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */

using Intuit.QuickBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Intuit.QuickBase.Client
{
    // TODO: May want to compose List<IQRecord> to hide the complexity of the List type.
    public sealed class QRecordCollection : List<IQRecord>
    {
        private readonly List<dynamic> _recordsToRemove = new List<dynamic>();

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
                if (Table.KeyFID == -1) _recordsToRemove.Add(record.RecordId);
                else _recordsToRemove.Add(record[Table.KeyFID]);
            }
            return base.Remove(record);
        }

        public new void RemoveAt(int index)
        {
            IQRecord record = this[index];
            if (record.IsOnServer)
            {
                if (Table.KeyFID == -1) _recordsToRemove.Add(record.RecordId);
                else _recordsToRemove.Add(record[Table.KeyFID]);
            }
            base.RemoveAt(index);
        }

        internal void RemoveRecords()
        {
            if (_recordsToRemove.Count > 0)
            {
                int keyfield = Table.KeyFID;
                if (keyfield == -1) keyfield = Table.Columns.Single(c => c.ColumnName == "Record ID#").ColumnId;
                List<string> lstQry =
                    _recordsToRemove.Select(recordId => String.Format("{{'{0}'.EQ.'{1}'}}", keyfield, (object)recordId))
                        .ToList();
                //TODO: further optimization possible: sort and search through lstQry combining conjoining spans
                int cnt = lstQry.Count;
                for (int i = 0; i < cnt; i += 100)
                {
                    int k = Math.Min(100, cnt - i);
                    string qry = String.Join(" OR ", lstQry.Skip(i).Take(k));
                    var prBuild = new PurgeRecords.Builder(Application.Client.Ticket, Application.Token,
                        Application.Client.AccountDomain, Table.TableId);
                    prBuild.SetQuery(qry);
                    var xml = prBuild.Build().Post().CreateNavigator();
                    int result = int.Parse(xml.SelectSingleNode("/qdbapi/errcode").Value);
                    if (result != 0)
                    {
                        string errmsg = xml.SelectSingleNode("/qdbapi/errtxt").Value;
                        throw new ApplicationException("Error in purgeRecords: '" + errmsg + "'");
                    }
                }
            }
            _recordsToRemove.Clear();
        }
    }
}