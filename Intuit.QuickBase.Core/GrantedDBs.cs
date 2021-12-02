/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */

using System.Xml.Linq;
using Intuit.QuickBase.Core.Payload;
using Intuit.QuickBase.Core.Uri;

namespace Intuit.QuickBase.Core
{
    public class GrantedDBs : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GrantedDBs";
        private readonly Payload.Payload _grantDBsPayload;
        private readonly IQUri _uri;

        public class Builder
        {
            internal string Ticket { get; set; }
            internal string UserToken { get; set; }
            internal string AppToken { get; set; }
            internal string AccountDomain { get; set; }

            public Builder(string ticket, string appToken, string accountDomain, string userToken = "")
            {
                Ticket = ticket;
                UserToken = userToken;
                AppToken = appToken;
                AccountDomain = accountDomain;
            }

            internal bool ExcludedParents { get; private set; }

            public Builder SetExcludedParents(bool val)
            {
                ExcludedParents = val;
                return this;
            }

            internal bool WithEmbeddedTables { get; private set; }

            public Builder SetWithEmbeddedTables(bool val)
            {
                WithEmbeddedTables = val;
                return this;
            }

            internal bool AdminOnly { get; private set; }

            public Builder SetAdminOnly(bool val)
            {
                AdminOnly = val;
                return this;
            }

            public GrantedDBs Build()
            {
                return new GrantedDBs(this);
            }
        }

        private GrantedDBs(Builder builder)
        {
            _grantDBsPayload = new GrantDBsPayload.Builder()
                .SetExcludedParents(builder.ExcludedParents)
                .SetWithEmbeddedTables(builder.WithEmbeddedTables)
                .SetAdminOnly(builder.AdminOnly)
                .Build();
            //If a user token is provided, use it instead of a ticket
            if (builder.UserToken.Length > 0)
            {
                _grantDBsPayload = new ApplicationUserToken(_grantDBsPayload, builder.UserToken);
            }
            else
            {
                _grantDBsPayload = new ApplicationTicket(_grantDBsPayload, builder.Ticket);
            }
            _grantDBsPayload = new ApplicationToken(_grantDBsPayload, builder.AppToken);
            _grantDBsPayload = new WrapPayload(_grantDBsPayload);
            _uri = new QUriMain(builder.AccountDomain);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _grantDBsPayload.GetXmlPayload(ref parent);
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
