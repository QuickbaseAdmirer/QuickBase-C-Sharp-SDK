/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Xml.Linq;

namespace Intuit.QuickBase.Client
{
    internal class QRecordFactory : QRecordFactoryBase
    {
        private static QRecordFactoryBase _instance;

        private QRecordFactory() { }

        internal static QRecordFactoryBase GetInstance()
        {
            if(_instance == null)
            {
                _instance = new QRecordFactory();
            }
            return _instance;
        }

        internal override IQRecord CreateInstance(IQApplication application, IQTable table, QColumnCollection columns)
        {
            return new QRecord(application, table, columns);
        }

        internal override IQRecord CreateInstance(IQApplication application, IQTable table, QColumnCollection columns, XElement recordNode)
        {
            return new QRecord(application, table, columns, recordNode);
        }
    }
}
