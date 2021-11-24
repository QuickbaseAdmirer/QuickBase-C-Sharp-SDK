/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Xml.Linq;
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
        private readonly string _options;
        private readonly string _query;
        private readonly string _collist;

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

            public Builder SetQuery(string query)
            {
                if (query == null) throw new ArgumentNullException(nameof(query));
                if (query.Trim() == String.Empty) throw new ArgumentException("query is empty after whitespace trim");
                Query = query;
                return this;
            }

            internal int Qid { get; private set; }

            public Builder SetQid(int qid)
            {
                if (qid < 1) throw new ArgumentException("Invalid QB Id");
                Qid = qid;
                return this;
            }

            internal string QName { get; private set; }

            public Builder SetQName(string qName)
            {
                if (qName == null) throw new ArgumentNullException(nameof(qName));
                if (qName.Trim() == String.Empty) throw new ArgumentException("qName is empty after whitespace trim");
                QName = qName;
                return this;
            }

            internal string CList { get; private set; }

            public Builder SetCList(string cList)
            {
                if (cList == null) throw new ArgumentNullException(nameof(cList));
                if (cList.Trim() == String.Empty) throw new ArgumentException("cList is empty after whitespace trim");
                CList = cList;
                return this;
            }

            internal string SList { get; private set; }

            public Builder SetSList(string sList)
            {
                if (sList == null) throw new ArgumentNullException(nameof(sList));
                if (sList.Trim() == String.Empty) throw new ArgumentException("sList is empty after whitespace trim");
                SList = sList;
                return this;
            }

            internal bool Fmt { get; private set; }

            public Builder SetFmt(bool val)
            {
                Fmt = val;
                return this;
            }

            internal string Options { get; private set; }

            public Builder SetOptions(string opts)
            {
                if (opts == null) throw new ArgumentNullException(nameof(opts));
                if (opts.Trim() == String.Empty) throw new ArgumentException("opts empty after whitespace trim");
                Options = opts;
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
            _options = builder.Options;
            _query = builder.Query;
            _collist = builder.CList;
            _doQueryPayload = new ApplicationTicket(_doQueryPayload, builder.Ticket);
            _doQueryPayload = new ApplicationToken(_doQueryPayload, builder.AppToken);
            _doQueryPayload = new WrapPayload(_doQueryPayload);
            _uri = new QUriDbid(builder.AccountDomain, builder.Dbid);
        }

        public string Options
        {
            get { return _options; }
        }

        public string Query
        {
            get { return _query; }
        }

        public string Collist
        {
           get { return _collist; }
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _doQueryPayload.GetXmlPayload(ref parent);
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

        public XElement Post()
        {
            HttpPost httpXml = new HttpPostXml();
            httpXml.Post(this);
            return httpXml.Response;
        }
    }
}