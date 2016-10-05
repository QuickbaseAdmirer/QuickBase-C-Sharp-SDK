/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class DoQueryPayload : Payload
    {
        private readonly string _query;
        private readonly int _qid;
        private readonly string _qName;
        private readonly string _cList;
        private readonly string _sList;
        private string _options;
        private readonly bool _fmt;

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

            internal string CList { get; private set; }
            internal Builder SetCList(string val)
            {
                CList = val;
                return this;
            }

            internal string SList { get; private set; }
            internal Builder SetSList(string val)
            {
                SList = val;
                return this;
            }

            internal bool Fmt { get; private set; }
            internal Builder SetFmt(bool val)
            {
                Fmt = val;
                return this;
            }

            internal string Options { get; private set; }
            internal Builder SetOptions(string val)
            {
                Options = val;
                return this;
            }

            internal DoQueryPayload Build()
            {
                return new DoQueryPayload(this);
            }
        }

        private DoQueryPayload(Builder builder)
        {
            _query = builder.Query;
            _qid = builder.Qid;
            _qName = builder.QName;
            _cList = builder.CList;
            _sList = builder.SList;
            _options = builder.Options;
            _fmt = builder.Fmt;
        }

        public string Options
        {
            get { return _options; }
            set { _options = value; }
        }

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(_query)) sb.Append(new XElement("query", _query));
            if (_qid > 0) sb.Append(new XElement("qid", _qid));
            if (!string.IsNullOrEmpty(_qName)) sb.Append(new XElement("qname", _qName));
            if (!string.IsNullOrEmpty(_cList)) sb.Append(new XElement("clist", _cList));
            if (!string.IsNullOrEmpty(_sList)) sb.Append(new XElement("slist", _sList));
            if (!string.IsNullOrEmpty(_options)) sb.Append(new XElement("options", _options));
            sb.Append(new XElement("fmt", "structured"));
            return sb.ToString();
        }
    }
}
