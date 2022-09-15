/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
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
    internal class DoQueryCountPayload : Payload
    {
        private readonly string _query;
        private readonly int _qid;
        private readonly string _qName;

        internal class Builder
        {
            internal string Query { get; private set; }
            internal Builder SetQuery(string val)
            {
                Query = val;
                return this;
            }

            internal int Qid { get; private set; }
            internal Builder SetQid(int val)
            {
                Qid = val;
                return this;
            }

            internal string QName { get; private set; }
            internal Builder SetQName(string val)
            {
                QName = val;
                return this;
            }

            internal DoQueryCountPayload Build()
            {
                return new DoQueryCountPayload(this);
            }
        }

        private DoQueryCountPayload(Builder builder)
        {
            _query = builder.Query;
            _qid = builder.Qid;
            _qName = builder.QName;
        }

        internal override void GetXmlPayload(ref XElement parent)
        {
            if (!string.IsNullOrEmpty(_query)) parent.Add(new XElement("query", _query));
            if (_qid > 0) parent.Add(new XElement("qid", _qid));
            if (!string.IsNullOrEmpty(_qName)) parent.Add(new XElement("qname", _qName));
        }
    }
}
