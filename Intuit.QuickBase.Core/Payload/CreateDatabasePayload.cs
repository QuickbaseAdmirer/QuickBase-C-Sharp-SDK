/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Text;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class CreateDatabasePayload : Payload
    {
        private string _dbName;
        private string _dbDesc;

        internal CreateDatabasePayload(string dbName, string dbDesc, bool createAppToken)
        {
            DbName = dbName;
            DBDesc = dbDesc;
            CreateAppToken = createAppToken;
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

        private string DBDesc
        {
            get { return _dbDesc; }
            set
            {
                if (value == null) throw new ArgumentNullException("dbDesc");
                if (value.Trim() == String.Empty) throw new ArgumentException("dbDesc");
                _dbDesc = value;
            }
        }

        private bool CreateAppToken { get; set; }

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            sb.Append(new XElement("dbname", DbName));
            sb.Append(new XElement("dbdesc", DBDesc));
            if (CreateAppToken) sb.Append(new XElement("createapptoken", 1));
            return sb.ToString();
        }
    }
}
