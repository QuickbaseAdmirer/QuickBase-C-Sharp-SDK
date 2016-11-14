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
    public class ChangeUserRole : IQObject
    {
        private const string QUICKBASE_ACTION = "API_ChangeUserRole";
        private Payload.Payload _changeUserRolePayload;
        private IQUri _uri;

        public ChangeUserRole(string ticket, string appToken, string accountDomain, string dbid, string userId, int currentRoleId, int newRoldId, string userToken = "")
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new ChangeUserRolePayload(userId, currentRoleId, newRoldId), userToken);

        }

        public ChangeUserRole(string ticket, string appToken, string accountDomain, string dbid, string userId, int currentRoleId, string userToken = "")
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new ChangeUserRolePayload(userId, currentRoleId), userToken);
        }

        private void CommonConstruction(string ticket, string appToken, string accountDomain, string dbid, Payload.Payload payload, string userToken = "")
        {
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _changeUserRolePayload = new ApplicationUserToken(payload, userToken);
            }
            else
            {
                _changeUserRolePayload = new ApplicationTicket(payload, ticket);
            }
            _changeUserRolePayload = new ApplicationToken(_changeUserRolePayload, appToken);
            _changeUserRolePayload = new WrapPayload(_changeUserRolePayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _changeUserRolePayload.GetXmlPayload();
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
