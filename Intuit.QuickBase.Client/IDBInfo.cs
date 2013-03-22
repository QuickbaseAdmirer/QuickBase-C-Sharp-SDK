/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public interface IDBInfo
    {
        string DbName { get; }
        string Version { get; }
        string MgrName { get; }
        string MgrId { get; }
        int NumRecords { get; }
        long CreateTime { get; }
        long LastModifiedTime { get; }
        long LastRecModtime { get; }
    }
}