/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Xml.XPath;

namespace Intuit.QuickBase.Client
{
    public interface IQTable
    {
        string TableName { get; }
        string TableId { get; }
        int KeyFID { get; }
        QRecordCollection Records { get; }
        QColumnCollection Columns { get; }
        void Clear();
        string GenCsv();
        string GenCsv(int queryId);
        string GenCsv(Query query);
        string GenHtml(string options = "", string clist = "a");
        string GenHtml(int queryId, string options = "", string clist = "a");
        string GenHtml(Query query, string options = "", string clist = "a");
        XPathDocument GetTableSchema();
        TableInfo GetTableInfo();
        int GetServerRecordCount();
        void Query();
        void Query(string options);
        void Query(int[] clist);
        void Query(int[] clist, string options);
        void Query(Query query);
        void Query(Query query, string options);
        void Query(Query query, int[] clist);
        void Query(Query query, int[] clist, int[] slist);
        void Query(Query query, int[] clist, int[] slist, string options);
        void Query(int queryId);
        void Query(int queryId, string options);
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