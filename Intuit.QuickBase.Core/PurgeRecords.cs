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
    /// Use this call carefully. If you do not supply a query parameter (query or qid or qname), ALL of the table records 
    /// will be deleted! If you supply an empty query parameter (&lt;query/&gt;, or &lt;qid/&gt; or &lt;qname/&gt;) all of 
    /// the table records will be deleted as well. You invoke this call on the table-level dbid if you need to delete a 
    /// bunch of records that meet your query criteria. (If you only need to delete one record, 
    /// com.intuit.quickbase.API_DeleteRecord would be a better choice.) All of the records meeting your query criteria 
    /// will be deleted. For the query, you can construct your own query string, in which case you use the &lt;query&gt; element 
    /// in the call. Alternatively you can use a saved query for the table, in which case you use &lt;qid&gt; or &lt;qname&gt;, 
    /// whichever is handier for you. See com.intuit.quickbase.API_DoQuery for information on saved queries or for 
    /// instructions on building the query string. Unsupported tags: &lt;query&gt;&lt;/query&gt;, &lt;qname&gt;&lt;/qname&gt; and 
    /// &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class PurgeRecords : IQObject
    {
        private const string QUICKBASE_ACTION = "API_PurgeRecords";
        private readonly Payload.Payload _purgeRecordsPayload;
        private readonly IQUri _uri;

        public class Builder
        {
            internal string Ticket { get; set; }
            internal string UserToken { get; set; }
            internal string AppToken { get; set; }
            internal string AccountDomain { get; set; }
            internal string Dbid { get; set; }

            public Builder(string ticket, string appToken, string accountDomain, string dbid, string userToken = "")
            {
                Ticket = ticket;
                UserToken = userToken;
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
                if (val == null) throw new ArgumentNullException("qname");
                if (val.Trim() == String.Empty) throw new ArgumentException("qname");
                QName = val;
                return this;
            }

            public PurgeRecords Build()
            {
                return new PurgeRecords(this);
            }
        }

        private PurgeRecords(Builder builder)
        {
            _purgeRecordsPayload = new PurgeRecordsPayload.Builder()
                .SetQuery(builder.Query)
                .SetQid(builder.Qid)
                .SetQName(builder.QName)
                .Build();
            //If a user token is provided, use it instead of a ticket
            if (builder.UserToken.Length > 0)
            {
                _purgeRecordsPayload = new ApplicationUserToken(_purgeRecordsPayload, builder.UserToken);
            }
            else
            {
                _purgeRecordsPayload = new ApplicationTicket(_purgeRecordsPayload, builder.Ticket);
            }
            _purgeRecordsPayload = new ApplicationToken(_purgeRecordsPayload, builder.AppToken);
            _purgeRecordsPayload = new WrapPayload(_purgeRecordsPayload);
            _uri = new QUriDbid(builder.AccountDomain, builder.Dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _purgeRecordsPayload.GetXmlPayload();
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