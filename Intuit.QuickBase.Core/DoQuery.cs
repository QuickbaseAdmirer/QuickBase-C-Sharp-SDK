/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Xml.XPath;
using Intuit.QuickBase.Core.Payload;
using Intuit.QuickBase.Core.Uri;

namespace Intuit.QuickBase.Core
{
    /// <summary>
    /// You invoke this call on a table-level dbid to get records from the table. You can use this call 
    /// to get all the records and all the fields, but typically you would want to get only some 
    /// of the records and only those fields you happen to care about, ordered and sorted the way 
    /// you need them to be. Unsupported tags: &lt;query&gt;&lt;/query&gt;, &lt;qname&gt;&lt;/qname&gt;, 
    /// &lt;clist&gt;&lt;/clist&gt;, &lt;slist&gt;&lt;/slist&gt;,  
    /// &lt;options&gt;&lt;/options&gt; and &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class DoQuery : IQObject
    {
        private const string QUICKBASE_ACTION = "API_DoQuery";
        private readonly Payload.Payload _doQueryPayload;
        private readonly IQUri _uri;

        public class Builder
        {
            internal string Ticket { get; set; }
            internal string AppToken { get; set; }
            internal string AccountDomain { get; set; }
            internal string Dbid { get; set; }

            public Builder(string ticket, string appToken, string accountDomain, string dbid)
            {
                Ticket = ticket;
                AppToken = appToken;
                AccountDomain = accountDomain;
                Dbid = dbid;
            }

            internal string Query { get; private set; }

            public Builder SetQuery(string val)
            {
                if (val == null) throw new ArgumentNullException("query");
                if (val.Trim() == String.Empty) throw new ArgumentException("query");
                Query = val;
                return this;
            }

            internal int Qid { get; private set; }

            public Builder SetQid(int val)
            {
                if (val < 1) throw new ArgumentException("qid");
                Qid = val;
                return this;
            }

            internal string QName { get; private set; }

            public Builder SetQName(string val)
            {
                if (val == null) throw new ArgumentNullException("qName");
                if (val.Trim() == String.Empty) throw new ArgumentException("qName");
                QName = val;
                return this;
            }

            internal string CList { get; private set; }

            public Builder SetCList(string val)
            {
                if (val == null) throw new ArgumentNullException("cList");
                if (val.Trim() == String.Empty) throw new ArgumentException("cList");
                CList = val;
                return this;
            }

            internal string SList { get; private set; }

            public Builder SetSList(string val)
            {
                if (val == null) throw new ArgumentNullException("sList");
                if (val.Trim() == String.Empty) throw new ArgumentException("sList");
                SList = val;
                return this;
            }

            internal bool Fmt { get; private set; }

            public Builder SetFmt(bool val)
            {
                Fmt = val;
                return this;
            }

            internal string Options { get; private set; }

            public Builder SetOptions(string val)
            {
                if (val == null) throw new ArgumentNullException("options");
                if (val.Trim() == String.Empty) throw new ArgumentException("options");
                Options = val;
                return this;
            }

            public DoQuery Build()
            {
                return new DoQuery(this);
            }
        }

        private DoQuery(Builder builder)
        {
            _doQueryPayload = new DoQueryPayload.Builder()
                .SetQuery(builder.Query)
                .SetQid(builder.Qid)
                .SetQName(builder.QName)
                .SetCList(builder.CList)
                .SetSList(builder.SList)
                .SetFmt(builder.Fmt)
                .SetOptions(builder.Options)
                .Build();
            _doQueryPayload = new ApplicationTicket(_doQueryPayload, builder.Ticket);
            _doQueryPayload = new ApplicationToken(_doQueryPayload, builder.AppToken);
            _doQueryPayload = new WrapPayload(_doQueryPayload);
            _uri = new QUriDbid(builder.AccountDomain, builder.Dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _doQueryPayload.GetXmlPayload();
            }
        }

        public System.Uri Uri
        {
            get
            {
                return _uri.GetQUri();
            }
        }

        public string Action
        {
            get
            {
                return QUICKBASE_ACTION;
            }
        }

        public XPathDocument Post()
        {
            HttpPost httpXml = new HttpPostXml();
            httpXml.Post(this);
            return httpXml.Response;
        }
    }
}