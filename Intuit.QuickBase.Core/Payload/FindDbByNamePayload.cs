/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class FindDbByNamePayload : Payload
    {
        private string _dbName;

        internal FindDbByNamePayload(string dbName)
        {
            DbName = dbName;
        }

        private string DbName
        {
            get { return _dbName; }
            set
            {
                if (value == null) throw new ArgumentNullException("dbName");
                if (value.Trim() == String.Empty) throw new ArgumentException("dbName");
                _dbName = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return new XElement("dbname", DbName).ToString();
        }
    }
}
