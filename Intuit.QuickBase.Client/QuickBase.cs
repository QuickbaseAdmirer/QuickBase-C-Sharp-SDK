/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public static class QuickBase
    {
        private const int HOURS = 8;

        public static AppDtm GetApplicationDataTimeInfo(string accountDomain, string applicationId)
        {
            return QApplication.GetApplicationDataTimeInfo(accountDomain, applicationId);
        }

        public static IQClient Login(string clientUserName, string clientPassword, string adminUserName, string adminPassword, string accountDomain, int hours)
        {
            QClientFactoryBase clientFactory = QClientFactory.GetInstance();
            return clientFactory.CreateInstance(clientUserName, clientPassword, adminUserName, adminPassword, accountDomain, hours);
        }

        public static IQClient Login(string clientUserName, string clientPassword, string adminUserName, string adminPassword, string accountDomain)
        {
            return Login(clientUserName, clientPassword, adminUserName, adminPassword, accountDomain, HOURS);
        }

        public static IQClient Login(string adminUserName, string adminPassword, string accountDomain, int hours)
        {
            return Login(adminUserName, adminPassword, adminUserName, adminPassword, accountDomain, hours);
        }

        public static IQClient Login(string adminUserName, string adminPassword, string accountDomain)
        {
            return Login(adminUserName, adminPassword, adminUserName, adminPassword, accountDomain, HOURS);
        }

        public static void Logout(IQClient client)
        {
            client.Logout();
        }
    }
}