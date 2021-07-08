/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Xml.Linq;

namespace Intuit.QuickBase.Client
{
    public interface IQTable
    {
        string TableName { get; }
        string TableId { get; }
        int KeyFID { get; }
        int KeyCIdx { get; }
        QRecordCollection Records { get; }
        QColumnCollection Columns { get; }
        void Clear();
        string GenCsv();
        string GenCsv(int queryId);
        string GenCsv(Query query);
        string GenHtml(string options = "", string colList = "a");
        string GenHtml(int queryId, string options = "", string colList = "a");
        string GenHtml(Query query, string options = "", string colList = "a");
        XElement GetTableSchema();
        TableInfo GetTableInfo();
        int GetServerRecordCount();
        void Query(bool clearRecords = true);
        void Query(string options, bool clearRecords = true);
        void Query(int[] colList, bool clearRecords = true);
        void Query(int[] colList, string options, bool clearRecords = true);
        void Query(Query query, bool clearRecords = true);
        void Query(Query query, string options, bool clearRecords = true);
        void Query(Query query, int[] colList, bool clearRecords = true);
        void Query(Query query, int[] colList, int[] sortList, bool clearRecords = true);
        void Query(Query query, int[] colList, int[] sortList, string options, bool clearRecords = true);
        void Query(int queryId, bool clearRecords = true);
        void Query(int queryId, string options, bool clearRecords = true);
        int QueryCount(Query query);
        int QueryCount(int queryId);
        void PurgeRecords();
        void PurgeRecords(int queryId);
        void PurgeRecords(Query query);
        void AcceptChanges();
        IQRecord NewRecord();
        void RefreshColumns();
        string ToString();
    }
}