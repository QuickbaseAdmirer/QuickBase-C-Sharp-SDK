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
    public class AddUserToRole : IQObject
    {
        private const string QUICKBASE_ACTION = "API_AddUserToRole";
        private readonly Payload.Payload _addUserToRolePayload;
        private readonly IQUri _uri;

        public AddUserToRole(string ticket, string appToken, string accountDomain, string dbid, string userId, int roleId, string userToken = "")
        {
            _addUserToRolePayload = new AddUserToRolePayload(userId, roleId);
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _addUserToRolePayload = new ApplicationUserToken(_addUserToRolePayload, userToken);
            }
            else
            {
                _addUserToRolePayload = new ApplicationTicket(_addUserToRolePayload, ticket);
            }
            _addUserToRolePayload = new ApplicationToken(_addUserToRolePayload, appToken);
            _addUserToRolePayload = new WrapPayload(_addUserToRolePayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _addUserToRolePayload.GetXmlPayload(ref parent);
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
