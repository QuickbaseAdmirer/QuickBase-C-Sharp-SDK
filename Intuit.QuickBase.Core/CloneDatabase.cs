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
    /// This call makes a complete clone of the specified source application, schema, 
    /// views, users, and so for. Optionally, using the keepData parameter, you can clone 
    /// the data as well. If you clone data, you can optionally use the excludefiles param 
    /// if you don’t want file attachments to be cloned. If you don’t want to carry over 
    /// some or any of the users in the original application, after you clone the 
    /// database you can invoke com.intuit.quickbase.API_RemoveUserFromRole class on the 
    /// clone to remove any user you want omitted. Unsupported tags: &lt;excludefiles fid=""&gt;&lt;/excludefiles&gt; 
    /// and &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class CloneDatabase : IQObject
    {
        private const string QUICKBASE_ACTION = "API_CloneDatabase";
        private readonly Payload.Payload _cloneDatabasePayload;
        private readonly IQUri _uri;

        public class Builder
        {
            private string _newDBName;
            private string _newDBDesc;

            internal string Ticket { get; set; }
            internal string UserToken { get; set; }
            internal string AppToken { get; set; }
            internal string AccountDomain { get; set; }
            internal string Dbid { get; set; }
            internal string NewDBName
            {
                get
                {
                    return _newDBName;
                }
                set
                {
                    if (value == null) throw new ArgumentNullException("newDBName");
                    if (value.Trim() == String.Empty) throw new ArgumentException("newDBName");
                    _newDBName = value;
                }
            }

            internal string NewDBDesc
            {
                get
                {
                    return _newDBDesc;
                }
                set
                {
                    if (value == null) throw new ArgumentNullException("newDBDesc");
                    if (value.Trim() == String.Empty) throw new ArgumentException("newDBDesc");
                    _newDBDesc = value;
                }
            }

            public Builder(string ticket, string appToken, string accountDomain, string dbid, string newDBName, string newDBDesc, string userToken = "")
            {
                Ticket = ticket;
                UserToken = userToken;
                AppToken = appToken;
                AccountDomain = accountDomain;
                Dbid = dbid;
                NewDBName = newDBName;
                NewDBDesc = newDBDesc;
            }

            internal bool KeepData { get; private set; }

            public Builder SetKeepData(bool val)
            {
                KeepData = val;
                return this;
            }

            internal bool ExcludeFiles { get; private set; }

            public Builder SetExcludeFiles(bool val)
            {
                ExcludeFiles = val;
                return this;
            }

            public CloneDatabase Build()
            {
                return new CloneDatabase(this);
            }
        }

        private CloneDatabase(Builder builder)
        {
            _cloneDatabasePayload = new CloneDatabasePayload.Builder(builder.NewDBName, builder.NewDBDesc)
                .SetKeepData(builder.KeepData)
                .SetExcludeFiles(builder.ExcludeFiles)
                .Build();
            //If a user token is provided, use it instead of a ticket
            if (builder.UserToken.Length > 0)
            {
                _cloneDatabasePayload = new ApplicationUserToken(_cloneDatabasePayload, builder.UserToken);
            }
            else
            {
                _cloneDatabasePayload = new ApplicationTicket(_cloneDatabasePayload, builder.Ticket);
            }
            _cloneDatabasePayload = new ApplicationToken(_cloneDatabasePayload, builder.AppToken);
            _cloneDatabasePayload = new WrapPayload(_cloneDatabasePayload);
            _uri = new QUriDbid(builder.AccountDomain, builder.Dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _cloneDatabasePayload.GetXmlPayload();
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