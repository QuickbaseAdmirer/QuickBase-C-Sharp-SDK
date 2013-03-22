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
    public enum CreateApplicationToken
    {
        No = 0,
        Yes
    }

    public class QClient : IQClient
    {
        internal QClient(QApplicationFactoryBase applicationFactory, string clientUserName, string clientPassword, string adminUserName, string adminPassword, string accountDomain, int hours)
        {
            ApplicationFactory = applicationFactory;
            ClientUserName = clientUserName;
            ClientPassword = clientPassword;
            AdminUserName = adminUserName;
            AdminPassword = adminPassword;
            AccountDomain = accountDomain;
            Hours = hours;

            Login();
        }

        public string Ticket { get; private set; }
        public string ClientUserName { get; private set; }
        public string ClientPassword { get; private set; }
        public string AdminUserName { get; private set; }
        public string AdminPassword { get; private set; }
        public string AccountDomain { get; private set; }
        public int Hours { get; private set; }
        private QApplicationFactoryBase ApplicationFactory { get; set; }

        private void Login()
        {
            var signin = new Authenticate(ClientUserName, ClientPassword, AccountDomain, Hours);
            var xml = signin.Post().CreateNavigator();

            Ticket = xml.SelectSingleNode("/qdbapi/ticket").Value;
        }


        public void Logout()
        {
            var signout = new SignOut(AccountDomain);
            signout.Post();
            Ticket = null;
            ClientUserName = null;
            ClientPassword = null;
            AdminUserName = null;
            AdminPassword = null;
            AccountDomain = null;
            Hours = 0;
        }

        public IQApplication Connect(string applicationId, string token)
        {
            return ApplicationFactory.CreateInstance(this, applicationId, token);
        }

        public IQApplication Connect(string applicationId)
        {
            return Connect(applicationId, null);
        }

        public void Disconnect(IQApplication application)
        {
            application.Disconnect();
        }

        public IQApplication CreateApplication(string qbName, string qbDescription, CreateApplicationToken createApplicationToken)
        {
            var createToken = Convert.ToBoolean(createApplicationToken);

            var createDb = new CreateDatabase(Ticket, AccountDomain, qbName, qbDescription, createToken);
            var xml = createDb.Post().CreateNavigator();

            string token = null;
            if (createToken)
            {
                token = xml.SelectSingleNode("/qdbapi/apptoken").Value;
            }
            var applicationId = xml.SelectSingleNode("/qdbapi/appdbid").Value;
            return Connect(applicationId, token);
        }

        public List<string> FindApplication(string qbName)
        {
            var findDb = new FindDbByName(Ticket, AccountDomain, qbName);
            var xml = findDb.Post().CreateNavigator();

            var nodes = xml.Select("/qdbapi/dbid");
            var dbids = new List<string>();
            foreach (XPathNavigator node in nodes)
            {
                dbids.Add(node.Value);
            }
            return dbids;
        }
    }
}
