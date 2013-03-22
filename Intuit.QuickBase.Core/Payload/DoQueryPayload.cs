/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Text;

namespace Intuit.QuickBase.Core.Payload
{
    internal class DoQueryPayload : Payload
    {
        private readonly string _query;
        private readonly int _qid;
        private readonly string _qName;
        private readonly string _cList;
        private readonly string _sList;
        private readonly string _options;
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

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            sb.Append(!String.IsNullOrEmpty(_query) ? String.Format("<query>{0}</query>", _query) : String.Empty);
            sb.Append(_qid > 0 ? String.Format("<qid>{0}</qid>", _qid) : String.Empty);
            sb.Append(!String.IsNullOrEmpty(_qName) ? String.Format("<qname>{0}</qname>", _qName) : String.Empty);
            sb.Append(!String.IsNullOrEmpty(_cList) ? String.Format("<clist>{0}</clist>", _cList) : String.Empty);
            sb.Append(!String.IsNullOrEmpty(_sList) ? String.Format("<slist>{0}</slist>", _sList) : String.Empty);
            sb.Append(!String.IsNullOrEmpty(_options) ? String.Format("<options>{0}</options>", _options) : String.Empty);
            sb.Append(_fmt ? "<fmt>structured</fmt>" : String.Empty);
            return sb.ToString();
        }
    }
}
