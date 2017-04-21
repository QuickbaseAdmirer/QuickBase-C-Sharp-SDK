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
using System.Text;

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

        public new void RemoveAt(int index)
        {
            IQRecord record = this[index];
            if (record.IsOnServer)
            {
                _recordsToRemove.Add(record.RecordId);
            }
            base.RemoveAt(index);
        }

        public new void RemoveAll(Predicate<IQRecord> predicate)
        {
            int idx = 0;
            int startIdx = 0;
            while ((idx = this.FindIndex(startIdx, predicate)) >= 0)
            {
                IQRecord record = this[idx];
                if (record.IsOnServer)
                {
                    _recordsToRemove.Add(record.RecordId);
                }
                startIdx = idx + 1;
            }
            base.RemoveAll(predicate);
        }

        internal void RemoveRecords()
        {
            if (_recordsToRemove.Count > 0)
            {
                int keyfield = Table.Columns.Single(c => c.ColumnType == FieldType.recordid).ColumnId;
                List<string> lstQry = new List<string>();
                _recordsToRemove.Sort();
                int lastVal = _recordsToRemove[0];
                int rangeStart = -1, rangeEnd = -1;
                for (int i = 1; i < _recordsToRemove.Count; i++)
                {
                    int val = _recordsToRemove[i];
                    if (val == lastVal + 1)
                    {
                        if (rangeStart == -1) rangeStart = lastVal;
                        rangeEnd = val;
                    }
                    else
                    {
                        if (rangeStart != -1)
                        {
                            lstQry.Add(string.Format("{{'{0}'.GTE.'{1}'}} AND {{'{0}'.LTE.'{2}'}}", keyfield, rangeStart, rangeEnd));
                            rangeStart = -1;
                            rangeEnd = -1;
                        }
                        else
                        {
                            lstQry.Add(string.Format("{{'{0}'.EQ.'{1}'}}", keyfield, lastVal));   
                        }
                    }
                    lastVal = val;
                }
                if (rangeStart != -1)
                {
                    lstQry.Add(string.Format("{{'{0}'.GTE.'{1}'}} AND {{'{0}'.LTE.'{2}'}}", keyfield, rangeStart, rangeEnd));
                }
                else
                {
                    lstQry.Add(string.Format("{{'{0}'.EQ.'{1}'}}", keyfield, lastVal));
                }
                StringBuilder qry = null;
                int cnt = 0;
                foreach (string addStr in lstQry)
                {
                    if (addStr.Contains("AND")) cnt += 2;
                    else cnt += 1;
                    if (qry == null)
                        qry = new StringBuilder(addStr);
                    else
                    {
                        qry.Append(" OR " + addStr);
                    }
                    if (cnt >= 98)
                    {
                        SendDelete(qry.ToString());
                        cnt = 0;
                        qry = null;
                    }
                }
                if (qry != null) SendDelete(qry.ToString());
            }
            _recordsToRemove.Clear();
        }

        private void SendDelete(string qry)
        {
            var prBuild = new PurgeRecords.Builder(Application.Client.Ticket, Application.Token,
                Application.Client.AccountDomain, Table.TableId);
            prBuild.SetQuery(qry.ToString());
            var xml = prBuild.Build().Post().CreateNavigator();
            int result = int.Parse(xml.SelectSingleNode("/qdbapi/errcode").Value);
            if (result != 0)
            {
                string errmsg = xml.SelectSingleNode("/qdbapi/errtxt").Value;
                throw new ApplicationException("Error in RemoveRecords: '" + errmsg + "'");
            }
        }
    }
}