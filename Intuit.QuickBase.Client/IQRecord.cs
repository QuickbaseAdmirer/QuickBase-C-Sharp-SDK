/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public interface IQRecord
    {
        int RecordId { get; }
        RecordState RecordState { get; }
        bool IsOnServer { get; }
        object this[int index] { get; set; }
        object this[string columnName] { get; set; }
        void AcceptChanges();
        void UploadFile(string columnName, string filePath);
        void DownloadFile(string columnName, string path, int versionId);
        void ChangeOwnerTo(string newOwner);
        string GetAsCSV(string clist);
        bool Equals(IQRecord record);
        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }

    internal interface IQRecord_int
    {
        void ForceUpdateState(int recId);
        void ForceUpdateState();
        int GetColumnIndex(string colName);
    }
}
