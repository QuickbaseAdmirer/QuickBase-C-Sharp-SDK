/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    internal class QTableFactory : QTableFactoryBase
    {
        private static QTableFactoryBase _instance;

        private QTableFactory() { }

        internal static QTableFactoryBase GetInstance()
        {
            if(_instance == null)
            {
                _instance = new QTableFactory();
            }
            return _instance;
        }

        // Creates a new table in QuickBase
        internal override IQTable CreateInstance(IQApplication application, string tableName, string pNoun)
        {
            QColumnFactoryBase columnFactory = QColumnFactory.GetInstance();
            QRecordFactoryBase recordFactory = QRecordFactory.GetInstance();
            return new QTable(columnFactory, recordFactory, application, tableName, pNoun);
        }
        
        // Doesn't create a table in QuickBase
        internal override IQTable CreateInstance(IQApplication application, string tableId)
        {
            QColumnFactoryBase columnFactory = QColumnFactory.GetInstance();
            QRecordFactoryBase recordFactory = QRecordFactory.GetInstance();
            return new QTable(columnFactory, recordFactory, application, tableId);
        }

        internal override IQTable CreateInstanceLazy(IQApplication application, string tableId)
        {
            return new QTable(null, null, application, tableId);
        }

    }
}