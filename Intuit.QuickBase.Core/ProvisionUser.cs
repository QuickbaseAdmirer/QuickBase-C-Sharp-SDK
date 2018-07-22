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
    public class ProvisionUser : IQObject
    {
        private const string QUICKBASE_ACTION = "API_ProvisionUser";
        private Payload.Payload _provisionUserPayload;
        private IQUri _uri;

        public ProvisionUser(string ticket, string appToken, string accountDomain, string dbid, string email, int roleId, string firstName, string lastName)
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new ProvisionUserPayload(email, roleId, firstName, lastName));
        }

        public ProvisionUser(string ticket, string appToken, string accountDomain, string dbid, string email, string firstName, string lastName)
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new ProvisionUserPayload(email, firstName, lastName));
        }

        private void CommonConstruction(string ticket, string appToken, string accountDomain, string dbid, Payload.Payload payload)
        {
            _provisionUserPayload = new ApplicationTicket(payload, ticket);
            _provisionUserPayload = new ApplicationToken(_provisionUserPayload, appToken);
            _provisionUserPayload = new WrapPayload(_provisionUserPayload);

            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _provisionUserPayload.GetXmlPayload(ref parent);
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
