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
    public class GetUserRole : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetUserRole";
        private readonly Payload.Payload _getUserRolePayload;
        private readonly IQUri _uri;

        public GetUserRole(string ticket, string appToken, string accountDomain, string dbid, string userId)
        {
            _getUserRolePayload = new GetUserRolePayload(userId);
            _getUserRolePayload = new ApplicationTicket(_getUserRolePayload, ticket);
            _getUserRolePayload = new ApplicationToken(_getUserRolePayload, appToken);
            _getUserRolePayload = new WrapPayload(_getUserRolePayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _getUserRolePayload.GetXmlPayload(ref parent);
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
