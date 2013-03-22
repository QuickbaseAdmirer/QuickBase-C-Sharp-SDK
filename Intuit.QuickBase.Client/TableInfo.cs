/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public class TableInfo : IDBInfo
    {
        public TableInfo(string dbName, long lastRecModTime, long lastModifiedTime, long createTime,
                         int numRecords, string mgrId, string mgrName, string version)
        {
            DbName = dbName;
            LastRecModtime = lastRecModTime;
            LastModifiedTime = lastModifiedTime;
            CreateTime = createTime;
            NumRecords = numRecords;
            MgrId = mgrId;
            MgrName = mgrName;
            Version = version;
        }

        public string DbName { get; private set; }
        public string Version { get; private set; }
        public string MgrName { get; private set; }
        public string MgrId { get; private set; }
        public int NumRecords { get; private set; }
        public long CreateTime { get; private set; }
        public long LastModifiedTime { get; private set; }
        public long LastRecModtime { get; private set; }
    }
}