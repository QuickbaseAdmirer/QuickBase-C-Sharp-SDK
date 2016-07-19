/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using Intuit.QuickBase.Core;

namespace Intuit.QuickBase.Client
{
    public class QTable : IQTable
    {
        // Constructors
        internal QTable(QColumnFactoryBase columnFactory, QRecordFactoryBase recordFactory, IQApplication application, string tableId)
        {
            CommonConstruction(columnFactory, recordFactory, application, tableId);
            Load();
        }

        internal QTable(QColumnFactoryBase columnFactory, QRecordFactoryBase recordFactory, IQApplication application, string tableName, string pNoun)
        {
            var createTable = new CreateTable.Builder(application.Client.Ticket, application.Token, application.Client.AccountDomain, application.ApplicationId)
                .SetTName(tableName)
                .SetPNoun(pNoun)
                .Build();
            var xml = createTable.Post().CreateNavigator();
            var tableId = xml.SelectSingleNode("/qdbapi/newdbid").Value;

            TableName = tableName;
            RecordNames = pNoun;
            CommonConstruction(columnFactory, recordFactory, application, tableId);
        }

        private void CommonConstruction(QColumnFactoryBase columnFactory, QRecordFactoryBase recordFactory, IQApplication application, string tableId)
        {
            ColumnFactory = columnFactory;
            RecordFactory = recordFactory;
            Application = application;
            TableId = tableId;
            KeyFID = -1;
            Records = new QRecordCollection(Application, this);
            Columns = new QColumnCollection(Application, this);
        }

        // Properties
        private IQApplication Application { get; set; }

        public string TableId { get; private set; }

        public string TableName { get; private set; }

        public string RecordNames { get; private set; }

        public QRecordCollection Records { get; private set; }

        public QColumnCollection Columns { get; private set; }

        private QColumnFactoryBase ColumnFactory { get; set; }

        private QRecordFactoryBase RecordFactory { get; set; }

        public int KeyFID { get; private set; }

        // Methods
        public void Clear()
        {
            Records.Clear();
            Columns.Clear();
        }

        public string GenCsv()
        {
            var genResultsTable = new GenResultsTable.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetOptions("csv")
                .Build();
            var xml = genResultsTable.Post().CreateNavigator();
            return xml.SelectSingleNode("/response_data").Value;
        }

        public string GenCsv(int queryId)
        {
            var genResultsTable = new GenResultsTable.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQid(queryId)
                .SetOptions("csv")
                .Build();
            var xml = genResultsTable.Post().CreateNavigator();
            return xml.SelectSingleNode("/response_data").Value;
        }

        public string GenCsv(Query query)
        {
            var genResultsTable = new GenResultsTable.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .SetOptions("csv")
                .Build();
            var xml = genResultsTable.Post().CreateNavigator();
            return xml.SelectSingleNode("/response_data").Value;
        }

        public string GenHtml(string options = "", string clist = "a")
        {
            var genResultsTable = new GenResultsTable.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetOptions(options)
                .SetCList(clist)
                .Build();
            var xml = genResultsTable.Post().CreateNavigator();
            return xml.SelectSingleNode("/response_data").Value;
        }

        public string GenHtml(int queryId, string options = "", string clist = "a")
        {
            var genResultsTable = new GenResultsTable.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQid(queryId)
                .SetOptions(options)
                .SetCList(clist)
                .Build();
            var xml = genResultsTable.Post().CreateNavigator();
            return xml.SelectSingleNode("/response_data").Value;
        }

        public string GenHtml(Query query, string options = "", string clist = "a")
        {
            var genResultsTable = new GenResultsTable.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .SetOptions(options)
                .SetCList(clist)
                .Build();
            var xml = genResultsTable.Post().CreateNavigator();
            return xml.SelectSingleNode("/response_data").Value;
        }

        public XPathDocument GetTableSchema()
        {
            var tblSchema = new GetSchema(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId);
            return tblSchema.Post();
        }

        public TableInfo GetTableInfo()
        {
            var getTblInfo = new GetDbInfo(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId);
            var xml = getTblInfo.Post().CreateNavigator();

            var dbName = xml.SelectSingleNode("/qdbapi/dbname").Value;
            var lastRecModTime = long.Parse(xml.SelectSingleNode("/qdbapi/lastRecModTime").Value);
            var lastModifiedTime = long.Parse(xml.SelectSingleNode("/qdbapi/lastModifiedTime").Value);
            var createTime = long.Parse(xml.SelectSingleNode("/qdbapi/createdTime").Value);
            var numRecords = int.Parse(xml.SelectSingleNode("/qdbapi/numRecords").Value);
            var mgrId = xml.SelectSingleNode("/qdbapi/mgrID").Value;
            var mgrName = xml.SelectSingleNode("/qdbapi/mgrName").Value;
            var version = xml.SelectSingleNode("/qdbapi/version").Value;

            return new TableInfo(dbName, lastRecModTime, lastModifiedTime, createTime, numRecords, mgrId, mgrName,
                                    version);
        }

        public int GetServerRecordCount()
        {
            var tblRecordCount = new GetNumRecords(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId);
            var xml = tblRecordCount.Post().CreateNavigator();
            return int.Parse(xml.SelectSingleNode("/qdbapi/num_records").Value);
        }

        public void Query()
        {
            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetCList("a")
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml); //Must be done each time, incase the schema changes due to another user, or from a previous query that has a difering subset of columns
            LoadRecords(xml);
        }

        public void Query(string options)
        {
            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetCList("a")
                .SetFmt(true)
                .SetOptions(options)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(int[] clist)
        {
            var colList = GetColumnList(clist);

            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetCList(colList)
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(int[] clist, string options)
        {
            var colList = GetColumnList(clist);

            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetCList(colList)
                .SetOptions(options)
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(Query query)
        {
            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .SetCList("a")
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(Query query, string options)
        {
            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .SetCList("a")
                .SetOptions(options)
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(Query query, int[] clist)
        {
            var colList = GetColumnList(clist);

            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .SetCList(colList)
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(Query query, int[] clist, int[] slist)
        {
            var solList = GetSortList(slist);
            var colList = GetColumnList(clist);

            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .SetCList(colList)
                .SetSList(solList)
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(Query query, int[] clist, int[] slist, string options)
        {
            var solList = GetSortList(slist);
            var colList = GetColumnList(clist);

            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .SetCList(colList)
                .SetSList(solList)
                .SetOptions(options)
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(int queryId)
        {
            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQid(queryId)
                .SetFmt(true)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public void Query(int queryId, string options)
        {
            var doQuery = new DoQuery.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQid(queryId)
                .SetFmt(true)
                .SetOptions(options)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            LoadColumns(xml);
            LoadRecords(xml);
        }

        public int QueryCount(Query query)
        {
            var doQuery = new DoQueryCount.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            return int.Parse(xml.SelectSingleNode("/qdbapi/numMatches").Value);
        }

        public int QueryCount(int queryId)
        {
            var doQuery = new DoQueryCount.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQid(queryId)
                .Build();
            var xml = doQuery.Post().CreateNavigator();
            return int.Parse(xml.SelectSingleNode("/qdbapi/numMatches").Value);
        }

        public void PurgeRecords()
        {
            var purge = new PurgeRecords.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId).Build();
            purge.Post();
            Records.Clear();
        }

        public void PurgeRecords(int queryId)
        {
            var purge = new PurgeRecords.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQid(queryId)
                .Build();
            purge.Post();
            Records.Clear();
        }

        public void PurgeRecords(Query query)
        {
            var purge = new PurgeRecords.Builder(Application.Client.Ticket, Application.Token, Application.Client.AccountDomain, TableId)
                .SetQuery(query.ToString())
                .Build();
            purge.Post();
            Records.Clear();
        }

        public void AcceptChanges()
        {
            Records.RemoveRecords();
            //optimize record uploads
            List<IQRecord> addList = Records.Where(record => record.RecordState == RecordState.New).ToList();
            List<IQRecord> modList = Records.Where(record => record.RecordState == RecordState.Modified).ToList();
            int acnt = addList.Count;
            int mcnt = modList.Count;
            if (acnt + mcnt > 0)
            {
                List<String> csvLines = new List<string>(acnt + mcnt);
                String clist = String.Join(".", KeyFID == -1 ? Columns.Where(col => (col.ColumnVirtual == false && col.ColumnLookup == false) || col.ColumnName == "Record ID#").Select(col => col.ColumnId.ToString())
                                                             : Columns.Where(col => (col.ColumnVirtual == false && col.ColumnLookup == false) || col.ColumnId == KeyFID).Select(col => col.ColumnId.ToString()));
                if (acnt > 0)
                {
                    csvLines.AddRange(addList.Select(record => record.GetAsCSV(clist)));
                }
                if (mcnt > 0)
                {
                    csvLines.AddRange(modList.Select(record => record.GetAsCSV(clist)));
                }
                var csvBuilder = new ImportFromCSV.Builder(Application.Client.Ticket, Application.Token,
                    Application.Client.AccountDomain, TableId, String.Join("\r\n", csvLines.ToArray()));
                csvBuilder.SetCList(clist);
                var csvUpload = csvBuilder.Build();

                var xml = csvUpload.Post().CreateNavigator();

                XPathNodeIterator xNodes = xml.Select("/qdbapi/rids/rid");
                //set records as in server now
                foreach (IQRecord rec in addList)
                {
                    xNodes.MoveNext();
                    ((IQRecord_int)rec).ForceUpdateState(Int32.Parse(xNodes.Current.Value));
                }
                foreach (IQRecord rec in modList)
                {
                    ((IQRecord_int)rec).ForceUpdateState();
                }
            }
        }

        public IQRecord NewRecord()
        {
            return RecordFactory.CreateInstance(Application, this, Columns);
        }

        public override string ToString()
        {
            return TableName;
        }

        internal void Load()
        {
            TableName = GetTableInfo().DbName;
            RefreshColumns();
        }

        private void LoadRecords(XPathNavigator xml)
        {
            Records.Clear();
            var recordNodes = xml.Select("/qdbapi/table/records/record");
            foreach (XPathNavigator recordNode in recordNodes)
            {
                var record = RecordFactory.CreateInstance(Application, this, Columns, recordNode);
                Records.Add(record);
            }
        }

        public void RefreshColumns()
        {
            LoadColumns(GetTableSchema().CreateNavigator());
        }

        private void LoadColumns(XPathNavigator xml)
        {
            Columns.Clear();
            var keyFidNode = xml.SelectSingleNode("/qdbapi/table/original/key_fid");
            if (keyFidNode != null) { KeyFID = keyFidNode.ValueAsInt; }
            var columnNodes = xml.Select("/qdbapi/table/fields/field");
            foreach (XPathNavigator columnNode in columnNodes)
            {
                var columnId = int.Parse(columnNode.GetAttribute("id", String.Empty));
                var type =
                    (FieldType)Enum.Parse(typeof(FieldType), columnNode.GetAttribute("field_type", String.Empty), true);
                var label = columnNode.SelectSingleNode("label").Value;
                bool hidden = false;
                var hidNode = columnNode.SelectSingleNode("appears_by_default");
                if (hidNode != null && hidNode.Value == "0") hidden = true;
                bool virt = columnNode.GetAttribute("mode", String.Empty) == "virtual";
                bool lookup = columnNode.GetAttribute("mode", String.Empty) == "lookup";
                IQColumn col = ColumnFactory.CreateInstace(columnId, label, type, virt, lookup, hidden);
                foreach (XPathNavigator choicenode in columnNode.Select("choices/choice"))
                {
                    object value;
                    switch (type)
                    {
                        case FieldType.rating:
                            value = Int32.Parse(choicenode.Value);
                            break;
                        default:
                            value = choicenode.Value;
                            break;
                    }
                    ((IQColumn_int)col).AddChoice(value);
                }
                Dictionary<string, int> colComposites = ((IQColumn_int)col).GetComposites();
                foreach (XPathNavigator compositenode in columnNode.Select("compositeFields/compositeField"))
                {
                    colComposites.Add(compositenode.GetAttribute("key", String.Empty), Int32.Parse(compositenode.GetAttribute("id", String.Empty)));
                }
                Columns.Add(col);
            }
        }

        private static string GetColumnList(ICollection<int> clist)
        {
            if (clist.Count > 0)
            {
                const int RECORDID_COLUMN_ID = 3;
                var columns = String.Empty;
                var columnList = new List<int>(clist.Count + 1) { RECORDID_COLUMN_ID };
                columnList.AddRange(clist.Where(columnId => columnId != RECORDID_COLUMN_ID));

                // Seed the list with the column ID of Record#ID

                columns = columnList.Aggregate(columns, (current, columnId) => current + (columnId + "."));
                return columns.TrimEnd('.');
            }
            return "a";
        }

        private static string GetSortList(IEnumerable<int> slist)
        {
            var solList = slist.Aggregate(String.Empty, (current, sol) => current + (sol + "."));
            return solList.TrimEnd('.');
        }
    }
}