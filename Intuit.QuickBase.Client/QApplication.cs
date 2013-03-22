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
using Intuit.QuickBase.Core.Exceptions;

namespace Intuit.QuickBase.Client
{
    public enum CloneData
    {
        No = 0,
        Yes
    }

    public class QApplication : IQApplication
    {
        // Instance fields
        private readonly Dictionary<string, IQTable> _qbDataTables = new Dictionary<string, IQTable>();

        // Constructors
        internal QApplication(QTableFactoryBase tableFactory, IQClient client, string applicationId, string token)
        {
            TableFactory = tableFactory;
            Client = client;
            ApplicationId = applicationId;
            Token = token;
            // Call these methods after setting the above properties.
            SetApplicationName();
            ClearTables();
            LoadTables();
        }

        // Properties
        public IQClient Client { get; private set; }
        public string ApplicationId { get; private set; }
        public string ApplicationName { get; private set; }
        public string Token { get; private set; }
        private QTableFactoryBase TableFactory { get; set; }

        // Methods
        public void Disconnect()
        {
            Client = null;
            ApplicationId = null;
            ApplicationName = null;
            Token = null;
            ClearTables();
        }

        public AppInfo GetApplicationInfo()
        {
            var getDbInfo = new GetDbInfo(Client.Ticket, Token, Client.AccountDomain, ApplicationId);
            var xml = getDbInfo.Post().CreateNavigator();

            var dbName = xml.SelectSingleNode("/qdbapi/dbname").Value;
            var lastRecModTime = long.Parse(xml.SelectSingleNode("/qdbapi/lastRecModTime").Value);
            var lastModifiedTime = long.Parse(xml.SelectSingleNode("/qdbapi/lastModifiedTime").Value);
            var createTime = long.Parse(xml.SelectSingleNode("/qdbapi/createdTime").Value);
            var numRecords = int.Parse(xml.SelectSingleNode("/qdbapi/numRecords").Value);
            var mgrId = xml.SelectSingleNode("/qdbapi/mgrID").Value;
            var mgrName = xml.SelectSingleNode("/qdbapi/mgrName").Value;
            var version = xml.SelectSingleNode("/qdbapi/version").Value;

            return new AppInfo(dbName, lastRecModTime, lastModifiedTime, createTime, numRecords, mgrId, mgrName,
                                    version);
        }

        public XPathDocument GetApplicationSchema()
        {
            return GetApplicationSchema(Client.ClientUserName, Client.ClientPassword, Client.AccountDomain);
        }

        private XPathDocument GetApplicationSchema(string username, string password, string accountDomain)
        {
            var signin = new Authenticate(username, password, accountDomain, 1);
            var xml = signin.Post().CreateNavigator();
            var adminTicket = xml.SelectSingleNode("/qdbapi/ticket").Value;

            var appSchema = new GetSchema(adminTicket, Token, accountDomain, ApplicationId);
            var xmlSchema = appSchema.Post();

            var signout = new SignOut(accountDomain);
            signout.Post();

            return xmlSchema;
        }

        public string CloneApplication(string qbNewName, string qbNewDescription, CloneData cloneData)
        {
            var cloneApp = new CloneDatabase.Builder(Client.Ticket, Token, Client.AccountDomain, ApplicationId, qbNewName, qbNewDescription)
                .SetKeepData(Convert.ToBoolean(cloneData))
                .Build();
            var xml = cloneApp.Post().CreateNavigator();

            return xml.SelectSingleNode("/qdbapi/newdbid").Value;
        }

        public void RenameApplication(string qbNewName)
        {
            var renameApp = new RenameApp(Client.Ticket, Token, Client.AccountDomain, ApplicationId, qbNewName);
            renameApp.Post();

            ApplicationName = qbNewName;
        }

        public void DeleteApplication()
        {
            var deleteApp = new DeleteDatabase(Client.Ticket, Token, Client.AccountDomain, ApplicationId);
            deleteApp.Post();

            Token = null;
            ApplicationId = null;
            ApplicationName = null;
            _qbDataTables.Clear();
        }

        public UserInfo GetUserInfo(string email)
        {
            var getUserInfo = new GetUserInfo(Client.Ticket, Token, Client.AccountDomain, email);
            var xml = getUserInfo.Post().CreateNavigator();
            var userId = xml.SelectSingleNode("/qdbapi/user").GetAttribute("id", String.Empty);
            var firstName = xml.SelectSingleNode("/qdbapi/user/firstName").Value;
            var lastName = xml.SelectSingleNode("/qdbapi/user/lastName").Value;
            var login = xml.SelectSingleNode("/qdbapi/user/login").Value;
            var emailAddress = xml.SelectSingleNode("/qdbapi/user/email").Value;
            var screenName = xml.SelectSingleNode("/qdbapi/user/screenName").Value;
            return new UserInfo(userId, firstName, lastName, login, emailAddress, screenName);
        }

        public UserRoleInfo GetUserRole(string userId)
        {
            var getUserRole = new GetUserRole(Client.Ticket, Token, Client.AccountDomain, ApplicationId, userId);
            var xml = getUserRole.Post().CreateNavigator();

            // User info
            var returnedUserId = xml.SelectSingleNode("/qdbapi/user").GetAttribute("id", String.Empty);
            var name = xml.SelectSingleNode("/qdbapi/user/name").Value;
            var userRoleInfo = new UserRoleInfo(returnedUserId, name);

            // Role info
            var roleNodes = xml.Select("/qdbapi/user/roles/role");
            foreach (XPathNavigator node in roleNodes)
            {
                var roleId = int.Parse(node.GetAttribute("id", String.Empty));
                var roleName = node.SelectSingleNode("name").Value;
                var accessNode = node.SelectSingleNode("access");
                var roleAccessId = int.Parse(accessNode.GetAttribute("id", String.Empty));
                var roleAccess = accessNode.Value;
                userRoleInfo.AddRole(roleId, roleName, roleAccessId, roleAccess);
            }
            return userRoleInfo;
        }

        public List<UserRoleInfo> UserRoles()
        {
            var userRoles = new UserRoles(Client.Ticket, Token, Client.AccountDomain, ApplicationId);
            var xml = userRoles.Post().CreateNavigator();
            var userRoleInfos = new List<UserRoleInfo>();

            var userNodes = xml.Select("/qdbapi/users/user");
            foreach (XPathNavigator user in userNodes)
            {
                // User info
                var userId = user.GetAttribute("id", String.Empty);
                var name = user.SelectSingleNode("name").Value;
                var userRoleInfo = new UserRoleInfo(userId, name);

                // Role info
                var roleNodes = user.Select("roles/role");
                foreach (XPathNavigator node in roleNodes)
                {
                    var roleId = int.Parse(node.GetAttribute("id", String.Empty));
                    var roleName = node.SelectSingleNode("name").Value;
                    var accessNode = node.SelectSingleNode("access");
                    var roleAccessId = int.Parse(accessNode.GetAttribute("id", String.Empty));
                    var roleAccess = accessNode.Value;
                    userRoleInfo.AddRole(roleId, roleName, roleAccessId, roleAccess);
                }
                userRoleInfos.Add(userRoleInfo);
            }
            return userRoleInfos;
        }

        public IQTable NewTable(string tableName, string pNoun)
        {
            var table = TableFactory.CreateInstance(this, tableName, pNoun);
            _qbDataTables.Add(table.TableId, table);
            return table;
        }

        public void DeleteTable(IQTable table)
        {
            var tableId = table.TableId;
            if (!_qbDataTables.ContainsKey(tableId))
            {
                throw new TableDoesNotExistInQuickBase("Table does not exist in this instance of QApplication.");
            }
            var deleteTbl = new DeleteDatabase(Client.Ticket, Token, Client.AccountDomain, tableId);
            deleteTbl.Post();

            _qbDataTables.Remove(tableId);
        }

        public IQTable GetTable(string dbid)
        {
            return _qbDataTables.ContainsKey(dbid) ? _qbDataTables[dbid] : null;
        }

        public Dictionary<string, IQTable> GetTables()
        {
            return _qbDataTables;
        }

        public AppDtm GetApplicationDataTimeInfo()
        {
            return GetApplicationDataTimeInfo(Client.AccountDomain, ApplicationId);
        }

        internal static AppDtm GetApplicationDataTimeInfo(string accountDomain, string applicationId)
        {
            var getAppDtmInfo = new GetAppDtmInfo(accountDomain, applicationId);
            var xml = getAppDtmInfo.Get().CreateNavigator();

            // Request info
            var requestTime = long.Parse(xml.SelectSingleNode("/qdbapi/RequestTime").Value);
            var requestNextAllowedTime = long.Parse(xml.SelectSingleNode("/qdbapi/RequestNextAllowedTime").Value);
            // App info
            var appNode = xml.SelectSingleNode("/qdbapi/app");
            var appDbid = appNode.GetAttribute("id", String.Empty);
            var appLastModifiedTime = long.Parse(appNode.SelectSingleNode("lastModifiedTime").Value);
            var appLastRecModTime = long.Parse(appNode.SelectSingleNode("lastRecModTime").Value);
            var appDtmInfo = new AppDtm(appDbid, appLastModifiedTime, appLastRecModTime, requestTime, requestNextAllowedTime);

            // Table info
            var tableNodes = xml.Select("/qdbapi/tables/table");
            foreach (XPathNavigator node in tableNodes)
            {
                var tableId = node.GetAttribute("id", String.Empty);
                var tableLastModifiedTime = long.Parse(node.SelectSingleNode("lastModifiedTime").Value);
                var tableLastRecModTime = long.Parse(node.SelectSingleNode("lastRecModTime").Value);
                appDtmInfo.AddTable(tableId, tableLastModifiedTime, tableLastRecModTime);
            }
            return appDtmInfo;
        }

        public List<GrantedAppsInfo> GrantedDBs()
        {
            var grantedDBs = new GrantedDBs.Builder(Client.Ticket, Token, Client.AccountDomain)
                .SetWithEmbeddedTables(true).Build();
            var xml = grantedDBs.Post().CreateNavigator();

            var dbinfoNodes = xml.Select("/qdbapi/databases/dbinfo");
            GrantedAppsInfo grantedApps = null;
            var grantedAppsInfos = new List<GrantedAppsInfo>();
            foreach (XPathNavigator dbinfo in dbinfoNodes)
            {
                if (!dbinfo.SelectSingleNode("dbname").Value.Contains(":"))
                {
                    var appName = dbinfo.SelectSingleNode("dbname").Value;
                    var appDbid = dbinfo.SelectSingleNode("dbid").Value;
                    grantedApps = new GrantedAppsInfo(appName, appDbid);
                    grantedAppsInfos.Add(grantedApps);
                }
                else
                {
                    var tableName = dbinfo.SelectSingleNode("dbname").Value;
                    var tableDbid = dbinfo.SelectSingleNode("dbid").Value;
                    if (grantedApps != null) grantedApps.AddTable(tableName, tableDbid);
                }
            }
            return grantedAppsInfos;
        }

        public override string ToString()
        {
            return ApplicationName;
        }

        internal void LoadTables()
        {
            var xmlApp = GetApplicationSchema(Client.AdminUserName, Client.AdminPassword, Client.AccountDomain).CreateNavigator();
            var nodes = xmlApp.Select("/qdbapi/table/chdbids/chdbid");
            foreach (XPathNavigator node in nodes)
            {
                var dbid = node.Value;

                try
                {
                    var qDataTable = TableFactory.CreateInstance(this, dbid);
                    _qbDataTables.Add(qDataTable.TableId, qDataTable);
                }
                catch (InsufficientPermissionsException) { }
            }
        }

        internal void SetApplicationName()
        {
            ApplicationName = GetApplicationInfo().DbName;
        }

        public void ClearTables()
        {
            _qbDataTables.Clear();
        }
    }
}
