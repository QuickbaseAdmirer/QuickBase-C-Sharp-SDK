/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Xml.XPath;
using Intuit.QuickBase.Core.Payload;
using Intuit.QuickBase.Core.Uri;

namespace Intuit.QuickBase.Core
{
    public class RemoveUserFromRole : IQObject
    {
        private const string QUICKBASE_ACTION = "API_RemoveUserFromRole";
        private readonly Payload.Payload _removeUserFromRolePayload;
        private readonly IQUri _uri;

        public RemoveUserFromRole(string ticket, string appToken, string accountDomain, string dbid, string userId, int roleId)
        {
            _removeUserFromRolePayload = new RemoveUserFromRolePayload(userId, roleId);
            _removeUserFromRolePayload = new ApplicationTicket(_removeUserFromRolePayload, ticket);
            _removeUserFromRolePayload = new ApplicationToken(_removeUserFromRolePayload, appToken);
            _removeUserFromRolePayload = new WrapPayload(_removeUserFromRolePayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _removeUserFromRolePayload.GetXmlPayload();
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
