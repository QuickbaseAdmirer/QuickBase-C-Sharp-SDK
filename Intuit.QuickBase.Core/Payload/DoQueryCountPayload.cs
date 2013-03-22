/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Text;

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

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            sb.Append(!String.IsNullOrEmpty(_query) ? String.Format("<query>{0}</query>", _query) : String.Empty);
            sb.Append(_qid > 0 ? String.Format("<qid>{0}</qid>", _qid) : String.Empty);
            sb.Append(!String.IsNullOrEmpty(_qName) ? String.Format("<qname>{0}</qname>", _qName) : String.Empty);
            return sb.ToString();
        }
    }
}
