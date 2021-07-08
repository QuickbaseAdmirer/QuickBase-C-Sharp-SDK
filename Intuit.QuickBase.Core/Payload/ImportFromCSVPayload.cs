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
    internal class ImportFromCSVPayload : Payload
    {
        private readonly string _recordsCsv;
        private readonly string _cList;
        private readonly bool _skipFirst;
        private readonly bool _timeInUtc;

        internal class Builder
        {
            internal Builder(string recordsCsv)
            {
                RecordsCsv = recordsCsv;
            }

            internal string RecordsCsv { get; set; }

            internal string CList { get; private set; }
            internal Builder SetCList(string val)
            {
                CList = val;
                return this;
            }

            internal bool SkipFirst { get; private set; }
            internal Builder SetSkipFirst(bool val)
            {
                SkipFirst = val;
                return this;
            }

            internal bool TimeInUtc { get; private set; }
            internal Builder SetTimeInUtc(bool val)
            {
                TimeInUtc = val;
                return this;
            }

            internal ImportFromCSVPayload Build()
            {
                return new ImportFromCSVPayload(this);
            }
        }

        private ImportFromCSVPayload(Builder builder)
        {
            _recordsCsv = builder.RecordsCsv;
            _cList = builder.CList;
            _skipFirst = builder.SkipFirst;
            _timeInUtc = builder.TimeInUtc;
        }

        internal override void GetXmlPayload(ref XElement parent)
        {
            parent.Add(new XElement("records_csv", new XCData(_recordsCsv)));
            if (!string.IsNullOrEmpty(_cList)) parent.Add(new XElement("clist", _cList));
            if (_skipFirst) parent.Add(new XElement("skipfirst", 1));
            if (_timeInUtc) parent.Add(new XElement("msInUTC", 1));
        }
    }
}
