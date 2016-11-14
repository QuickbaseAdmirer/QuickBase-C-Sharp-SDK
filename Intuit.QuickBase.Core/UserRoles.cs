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
    public class UserRoles : IQObject
    {
        private const string QUICKBASE_ACTION = "API_UserRoles";
        private readonly Payload.Payload _userRolesPayload;
        private readonly IQUri _uri;

        public UserRoles(string ticket, string appToken, string accountDomain, string dbid, string userToken = "")
        {
            _userRolesPayload = new UserRolesPayload();
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _userRolesPayload = new ApplicationUserToken(_userRolesPayload, userToken);
            }
            else
            {
                _userRolesPayload = new ApplicationTicket(_userRolesPayload, ticket);
            }
            _userRolesPayload = new ApplicationToken(_userRolesPayload, appToken);
            _userRolesPayload = new WrapPayload(_userRolesPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _userRolesPayload.GetXmlPayload();
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
