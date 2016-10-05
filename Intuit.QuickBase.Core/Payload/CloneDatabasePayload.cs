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
    internal class CloneDatabasePayload : Payload
    {
        private readonly string _newDBName;
        private readonly string _newDBDesc;
        private readonly bool _keepData;
        private readonly bool _excludeFiles;

        internal class Builder
        {
            internal Builder(string newDBName, string newDBDesc)
            {
                NewDBName = newDBName;
                NewDBDesc = newDBDesc;
            }

            internal string NewDBName { get; set; }
            internal string NewDBDesc { get; set; }

            internal bool KeepData { get; private set; }
            internal Builder SetKeepData(bool val)
            {
                KeepData = val;
                return this;
            }

            internal bool ExcludeFiles { get; private set; }
            internal Builder SetExcludeFiles(bool val)
            {
                ExcludeFiles = val;
                return this;
            }

            internal CloneDatabasePayload Build()
            {
                return new CloneDatabasePayload(this);
            }
        }

        private CloneDatabasePayload(Builder builder)
        {
            _newDBName = builder.NewDBName;
            _newDBDesc = builder.NewDBDesc;
            _keepData = builder.KeepData;
            _excludeFiles = builder.ExcludeFiles;
        }

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            sb.Append(new XElement("newdbname", _newDBName));
            sb.Append(new XElement("newdbdesc", _newDBDesc));
            if (_keepData) sb.Append(new XElement("keepData", 1));
            if (_excludeFiles) sb.Append(new XElement("excludefiles", 1));
            return sb.ToString();
        }
    }
}
