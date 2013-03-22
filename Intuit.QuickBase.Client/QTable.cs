/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Collections.Generic;
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

        // Methods
        public void Clear()
        {
            Records.Clear();
            Columns.Clear();
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

        public void AcceptChanges()
        {
            Records.RemoveRecords();
            foreach (var record in Records)
            {
                record.AcceptChanges();
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
            LoadColumns();
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

        private void LoadColumns()
        {
            LoadColumns(GetTableSchema().CreateNavigator());
        }

        private void LoadColumns(XPathNavigator xml)
        {
            Columns.Clear();
            var columnNodes = xml.Select("/qdbapi/table/fields/field");
            foreach (XPathNavigator columnNode in columnNodes)
            {
                var columnId = int.Parse(columnNode.GetAttribute("id", String.Empty));
                var type =
                    (FieldType)Enum.Parse(typeof(FieldType), columnNode.GetAttribute("field_type", String.Empty), true);
                var label = columnNode.SelectSingleNode("label").Value;

                var col = ColumnFactory.CreateInstace(columnId, label, type);
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

                // Seed the list with the column ID of Record#ID
                foreach (var columnId in clist)
                {
                    if (columnId != RECORDID_COLUMN_ID)
                    {
                        columnList.Add(columnId);
                    }
                }

                foreach (var columnId in columnList)
                {
                    columns += columnId + ".";
                }
                return columns.TrimEnd('.');
            }
            return "a";
        }

        private static string GetSortList(IEnumerable<int> slist)
        {
            var solList = String.Empty;
            foreach (var sol in slist)
            {
                solList += sol + ".";
            }
            return solList.TrimEnd('.');
        }
    }
}
