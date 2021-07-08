/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Collections.Generic;
using System.Xml.Linq;

namespace Intuit.QuickBase.Client
{
    public interface IQApplication
    {
        string Token { get; }
        string ApplicationName { get; }
        string ApplicationId { get; }
        IQClient Client { get; }
        void Disconnect();
        AppInfo GetApplicationInfo();
        XElement GetApplicationSchema();
        string CloneApplication(string qbNewName, string qbNewDescription, CloneData cloneData);
        void RenameApplication(string qbNewName);
        void DeleteApplication();
        UserInfo GetUserInfo(string email);
        UserRoleInfo GetUserRole(string userId);
        List<UserRoleInfo> UserRoles();
        IQTable NewTable(string tableName, string pNoun);
        void DeleteTable(IQTable table);
        IQTable GetTable(string dbid);
        Dictionary<string, IQTable> GetTables();
        AppDtm GetApplicationDataTimeInfo();
        List<GrantedAppsInfo> GrantedDBs();
        string ToString();
        void ClearTables();
    }
}