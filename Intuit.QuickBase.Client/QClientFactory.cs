/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    internal class QClientFactory : QClientFactoryBase
    {
        private static QClientFactoryBase _instance;

        private QClientFactory() { }

        internal static QClientFactoryBase GetInstance()
        {
            if(_instance == null)
            {
                _instance = new QClientFactory();
            }
            return _instance;
        }

        internal override IQClient CreateInstance(string clientUserName, string clientPassword, string adminUserName, string adminPassword, string accountDomain, int hours)
        {
            QApplicationFactoryBase applicationFactory = QApplicationFactory.GetInstance();
            return new QClient(applicationFactory, clientUserName, clientPassword, adminUserName, adminPassword, accountDomain, hours);
        }
    }
}
