/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
            var xml = getDbInfo.Post();

            var dbName = xml.Element("dbname").Value;
            var lastRecModTime = long.Parse(xml.Element("lastRecModTime").Value);
            var lastModifiedTime = long.Parse(xml.Element("lastModifiedTime").Value);
            var createTime = long.Parse(xml.Element("createdTime").Value);
            var numRecords = int.Parse(xml.Element("numRecords").Value);
            var mgrId = xml.Element("mgrID").Value;
            var mgrName = xml.Element("mgrName").Value;
            var version = xml.Element("version").Value;

            return new AppInfo(dbName, lastRecModTime, lastModifiedTime, createTime, numRecords, mgrId, mgrName,
                                    version);
        }

        public XElement GetApplicationSchema()
        {
            return GetApplicationSchema(Client.ClientUserName, Client.ClientPassword, Client.AccountDomain);
        }

        private XElement GetApplicationSchema(string username, string password, string accountDomain)
        {
            var signin = new Authenticate(username, password, accountDomain, 1);
            var xml = signin.Post();
            var adminTicket = xml.Element("ticket").Value;

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
            var xml = cloneApp.Post();

            return xml.Element("newdbid").Value;
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
            var xml = getUserInfo.Post();
            XElement userElm = xml.Element("user");
            var userId = xml.Element("user").Attribute("id").Value;
            var firstName = userElm.Element("firstName").Value;
            var lastName = userElm.Element("lastName").Value;
            var login = userElm.Element("login").Value;
            var emailAddress = userElm.Element("email").Value;
            var screenName = userElm.Element("screenName").Value;
            return new UserInfo(userId, firstName, lastName, login, emailAddress, screenName);
        }

        public UserRoleInfo GetUserRole(string userId)
        {
            var getUserRole = new GetUserRole(Client.Ticket, Token, Client.AccountDomain, ApplicationId, userId);
            var xml = getUserRole.Post();
            XElement userElm = xml.Element("user");
            // User info
            var returnedUserId = userElm.Attribute("id").Value;
            var name = userElm.Element("name").Value;
            var userRoleInfo = new UserRoleInfo(returnedUserId, name);

            // Role info
            foreach (XElement node in userElm.Element("roles").Elements("role"))
            {
                var roleId = int.Parse(node.Attribute("id").Value);
                var roleName = node.Element("name").Value;
                var accessNode = node.Element("access");
                var roleAccessId = int.Parse(accessNode.Attribute("id").Value);
                var roleAccess = accessNode.Value;
                userRoleInfo.AddRole(roleId, roleName, roleAccessId, roleAccess);
            }
            return userRoleInfo;
        }

        public List<UserRoleInfo> UserRoles()
        {
            var userRoles = new UserRoles(Client.Ticket, Token, Client.AccountDomain, ApplicationId);
            var xml = userRoles.Post();
            var userRoleInfos = new List<UserRoleInfo>();

            foreach (XElement user in xml.Element("users").Elements("user"))
            {
                // User info
                var userId = user.Attribute("id").Value;
                var name = user.Element("name").Value;
                var userRoleInfo = new UserRoleInfo(userId, name);

                // Role info
                foreach (XElement node in user.Element("roles").Elements("role"))
                {
                    var roleId = int.Parse(node.Attribute("id").Value);
                    var roleName = node.Element("name").Value;
                    var accessNode = node.Element("access");
                    var roleAccessId = int.Parse(accessNode.Attribute("id").Value);
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
            XElement xml = getAppDtmInfo.Get();

            // Request info
            var requestTime = long.Parse(xml.Element("RequestTime").Value);
            var requestNextAllowedTime = long.Parse(xml.Element("RequestNextAllowedTime").Value);
            // App info
            XElement appNode = xml.Element("app");
            string appDbid = appNode.Attribute("id").Value;
            long appLastModifiedTime = long.Parse(appNode.Element("lastModifiedTime").Value);
            long appLastRecModTime = long.Parse(appNode.Element("lastRecModTime").Value);
            var appDtmInfo = new AppDtm(appDbid, appLastModifiedTime, appLastRecModTime, requestTime, requestNextAllowedTime);

            // Table info
            foreach (XElement node in xml.Element("tables").Elements("table"))
            {
                var tableId = node.Attribute("id").Value;
                var tableLastModifiedTime = long.Parse(node.Element("lastModifiedTime").Value);
                var tableLastRecModTime = long.Parse(node.Element("lastRecModTime").Value);
                appDtmInfo.AddTable(tableId, tableLastModifiedTime, tableLastRecModTime);
            }
            return appDtmInfo;
        }

        public List<GrantedAppsInfo> GrantedDBs()
        {
            var grantedDBs = new GrantedDBs.Builder(Client.Ticket, Token, Client.AccountDomain)
                .SetWithEmbeddedTables(true).Build();
            var xml = grantedDBs.Post();

            GrantedAppsInfo grantedApps = null;
            var grantedAppsInfos = new List<GrantedAppsInfo>();
            foreach (XElement dbinfo in xml.Element("databases").Elements("dbinfo"))
            {
                if (!dbinfo.Element("dbname").Value.Contains(":"))
                {
                    var appName = dbinfo.Element("dbname").Value;
                    var appDbid = dbinfo.Element("dbid").Value;
                    grantedApps = new GrantedAppsInfo(appName, appDbid);
                    grantedAppsInfos.Add(grantedApps);
                }
                else
                {
                    var tableName = dbinfo.Element("dbname").Value;
                    var tableDbid = dbinfo.Element("dbid").Value;
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
            var xmlApp = GetApplicationSchema(Client.AdminUserName, Client.AdminPassword, Client.AccountDomain);
            foreach (XElement node in xmlApp.Element("table").Element("chdbids").Elements("chdbid"))
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
