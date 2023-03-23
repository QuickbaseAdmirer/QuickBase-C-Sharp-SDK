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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        public new void RemoveRange(int start, int count)
        {
            for (int idx = start; idx < start + count; idx++)
            {
                IQRecord record = this[idx];
                if (record.IsOnServer)
                {
                    _recordsToRemove.Add(record.RecordId);
                }
            }
            base.RemoveRange(start, count);
        }

        public new void RemoveAll(Predicate<IQRecord> predicate)
        {
            int idx;
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
                            lstQry.Add($"{{'{keyfield}'.GTE.'{rangeStart}'}} AND {{'{keyfield}'.LTE.'{rangeEnd}'}}");
                            rangeStart = -1;
                            rangeEnd = -1;
                        }
                        else
                        {
                            lstQry.Add($"{{'{keyfield}'.EQ.'{lastVal}'}}");   
                        }
                    }
                    lastVal = val;
                }

                lstQry.Add(rangeStart != -1
                    ? $"{{'{keyfield}'.GTE.'{rangeStart}'}} AND {{'{keyfield}'.LTE.'{rangeEnd}'}}"
                    : $"{{'{keyfield}'.EQ.'{lastVal}'}}");
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
            PurgeRecords.Builder prBuild = new PurgeRecords.Builder(Application.Client.Ticket, Application.Token,
                Application.Client.AccountDomain, Table.TableId);
            prBuild.SetQuery(qry);
            XElement xml = prBuild.Build().Post();
            if (xml != null)
            {
                int result = int.Parse(xml.Element("errcode").Value);
                if (result != 0)
                {
                    string errMsg = xml.Element("errtxt")?.Value;
                    throw new ApplicationException("Error in RemoveRecords: '" + errMsg + "'");
                }
            }
            else
            {
                throw new InvalidDataException("No XML returned from table query!");
            }
        }
    }
}