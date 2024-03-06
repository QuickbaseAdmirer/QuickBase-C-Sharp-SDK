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
        private QTableFactoryBase TableFactory { get; }

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
            GetDbInfo getDbInfo = new GetDbInfo(Client.Ticket, Token, Client.AccountDomain, ApplicationId);
            XElement xml = getDbInfo.Post();

            string dbName = xml.Element("dbname").Value;
            long lastRecModTime = long.Parse(xml.Element("lastRecModTime").Value);
            long lastModifiedTime = long.Parse(xml.Element("lastModifiedTime").Value);
            long createTime = long.Parse(xml.Element("createdTime").Value);
            int numRecords = int.Parse(xml.Element("numRecords").Value);
            string mgrId = xml.Element("mgrID").Value;
            string mgrName = xml.Element("mgrName").Value;
            string version = xml.Element("version").Value;

            return new AppInfo(dbName, lastRecModTime, lastModifiedTime, createTime, numRecords, mgrId, mgrName,
                                    version);
        }

        public XElement GetApplicationSchema()
        {
            return GetApplicationSchema(Client.ClientUserName, Client.ClientPassword, Client.AccountDomain);
        }

        private XElement GetApplicationSchema(string username, string password, string accountDomain)
        {
            Authenticate signin = new Authenticate(username, password, accountDomain, 1);
            XElement xml = signin.Post();
            string adminTicket = xml.Element("ticket").Value;

            GetSchema appSchema = new GetSchema(adminTicket, Token, accountDomain, ApplicationId);
            XElement xmlSchema = appSchema.Post();

            SignOut signout = new SignOut(accountDomain);
            signout.Post();

            return xmlSchema;
        }

        public string CloneApplication(string qbNewName, string qbNewDescription, CloneData cloneData)
        {
            CloneDatabase cloneApp = new CloneDatabase.Builder(Client.Ticket, Token, Client.AccountDomain, ApplicationId, qbNewName, qbNewDescription)
                .SetKeepData(Convert.ToBoolean(cloneData))
                .Build();
            XElement xml = cloneApp.Post();

            return xml.Element("newdbid").Value;
        }

        public void RenameApplication(string qbNewName)
        {
            RenameApp renameApp = new RenameApp(Client.Ticket, Token, Client.AccountDomain, ApplicationId, qbNewName);
            renameApp.Post();

            ApplicationName = qbNewName;
        }

        public void DeleteApplication()
        {
            DeleteDatabase deleteApp = new DeleteDatabase(Client.Ticket, Token, Client.AccountDomain, ApplicationId);
            deleteApp.Post();

            Token = null;
            ApplicationId = null;
            ApplicationName = null;
            _qbDataTables.Clear();
        }

        public UserInfo GetUserInfo(string email)
        {
            GetUserInfo getUserInfo = new GetUserInfo(Client.Ticket, Token, Client.AccountDomain, email);
            XElement xml = getUserInfo.Post();
            XElement userElm = xml.Element("user");
            string userId = xml.Element("user").Attribute("id").Value;
            string firstName = userElm.Element("firstName").Value;
            string lastName = userElm.Element("lastName").Value;
            string login = userElm.Element("login").Value;
            string emailAddress = userElm.Element("email").Value;
            string screenName = userElm.Element("screenName").Value;
            return new UserInfo(userId, firstName, lastName, login, emailAddress, screenName);
        }

        public UserRoleInfo GetUserRole(string userId)
        {
            GetUserRole getUserRole = new GetUserRole(Client.Ticket, Token, Client.AccountDomain, ApplicationId, userId);
            XElement xml = getUserRole.Post();
            XElement userElm = xml.Element("user");
            // User info
            string returnedUserId = userElm.Attribute("id").Value;
            string name = userElm.Element("name").Value;
            UserRoleInfo userRoleInfo = new UserRoleInfo(returnedUserId, name);

            // Role info
            foreach (XElement node in userElm.Element("roles").Elements("role"))
            {
                int roleId = int.Parse(node.Attribute("id").Value);
                string roleName = node.Element("name").Value;
                XElement accessNode = node.Element("access");
                int roleAccessId = int.Parse(accessNode.Attribute("id").Value);
                string roleAccess = accessNode.Value;
                userRoleInfo.AddRole(roleId, roleName, roleAccessId, roleAccess);
            }
            return userRoleInfo;
        }

        public List<UserRoleInfo> UserRoles()
        {
            UserRoles userRoles = new UserRoles(Client.Ticket, Token, Client.AccountDomain, ApplicationId);
            XElement xml = userRoles.Post();
            List<UserRoleInfo> userRoleInfos = new List<UserRoleInfo>();

            foreach (XElement user in xml.Element("users").Elements("user"))
            {
                // User info
                string userId = user.Attribute("id").Value;
                string name = user.Element("name").Value;
                UserRoleInfo userRoleInfo = new UserRoleInfo(userId, name);

                // Role info
                foreach (XElement node in user.Element("roles").Elements("role"))
                {
                    int roleId = int.Parse(node.Attribute("id").Value);
                    string roleName = node.Element("name").Value;
                    XElement accessNode = node.Element("access");
                    int roleAccessId = int.Parse(accessNode.Attribute("id").Value);
                    string roleAccess = accessNode.Value;
                    userRoleInfo.AddRole(roleId, roleName, roleAccessId, roleAccess);
                }
                userRoleInfos.Add(userRoleInfo);
            }
            return userRoleInfos;
        }

        public IQTable NewTable(string tableName, string pNoun)
        {
            IQTable table = TableFactory.CreateInstance(this, tableName, pNoun);
            _qbDataTables.Add(table.TableId, table);
            return table;
        }

        public void DeleteTable(IQTable table)
        {
            string tableId = table.TableId;
            if (!_qbDataTables.ContainsKey(tableId))
            {
                throw new TableDoesNotExistInQuickBase("Table does not exist in this instance of QApplication.");
            }
            DeleteDatabase deleteTbl = new DeleteDatabase(Client.Ticket, Token, Client.AccountDomain, tableId);
            deleteTbl.Post();

            _qbDataTables.Remove(tableId);
        }

        public IQTable GetTable(string dbid)
        {
            QTable tbl = (QTable)(_qbDataTables.ContainsKey(dbid) ? _qbDataTables[dbid] : null);
            if (tbl == null)
            {
                throw new TableDoesNotExistInQuickBase("Table does not exist in this instance of QApplication.");
            }
            if (!tbl.IsLoaded) tbl.Load();
            return tbl;
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
            GetAppDtmInfo getAppDtmInfo = new GetAppDtmInfo(accountDomain, applicationId);
            XElement xml = getAppDtmInfo.Get();

            // Request info
            long requestTime = long.Parse(xml.Element("RequestTime").Value);
            long requestNextAllowedTime = long.Parse(xml.Element("RequestNextAllowedTime").Value);
            // App info
            XElement appNode = xml.Element("app");
            string appDbid = appNode.Attribute("id").Value;
            long appLastModifiedTime = long.Parse(appNode.Element("lastModifiedTime").Value);
            long appLastRecModTime = long.Parse(appNode.Element("lastRecModTime").Value);
            AppDtm appDtmInfo = new AppDtm(appDbid, appLastModifiedTime, appLastRecModTime, requestTime, requestNextAllowedTime);

            // Table info
            foreach (XElement node in xml.Element("tables").Elements("table"))
            {
                string tableId = node.Attribute("id").Value;
                long tableLastModifiedTime = long.Parse(node.Element("lastModifiedTime").Value);
                long tableLastRecModTime = long.Parse(node.Element("lastRecModTime").Value);
                appDtmInfo.AddTable(tableId, tableLastModifiedTime, tableLastRecModTime);
            }
            return appDtmInfo;
        }

        public List<GrantedAppsInfo> GrantedDBs()
        {
            GrantedDBs grantedDBs = new GrantedDBs.Builder(Client.Ticket, Token, Client.AccountDomain)
                .SetWithEmbeddedTables(true).Build();
            XElement xml = grantedDBs.Post();

            GrantedAppsInfo grantedApps = null;
            List<GrantedAppsInfo> grantedAppsInfos = new List<GrantedAppsInfo>();
            foreach (XElement dbinfo in xml.Element("databases").Elements("dbinfo"))
            {
                if (!dbinfo.Element("dbname").Value.Contains(":"))
                {
                    string appName = dbinfo.Element("dbname").Value;
                    string appDbid = dbinfo.Element("dbid").Value;
                    grantedApps = new GrantedAppsInfo(appName, appDbid);
                    grantedAppsInfos.Add(grantedApps);
                }
                else
                {
                    string tableName = dbinfo.Element("dbname").Value;
                    string tableDbid = dbinfo.Element("dbid").Value;
                    grantedApps?.AddTable(tableName, tableDbid);
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
            XElement xmlApp = GetApplicationSchema(Client.AdminUserName, Client.AdminPassword, Client.AccountDomain);
            foreach (XElement node in xmlApp.Element("table").Element("chdbids").Elements("chdbid"))
            {
                string dbid = node.Value;

                try
                {
                    IQTable qDataTable = TableFactory.CreateInstanceLazy(this, dbid);
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
