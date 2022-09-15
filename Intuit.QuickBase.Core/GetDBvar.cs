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
    public class GetDBvar : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetDBVar";
        private readonly Payload.Payload _getDBvarPayload;
        private readonly IQUri _uri;

        public GetDBvar(string ticket, string appToken, string accountDomain, string dbid, string varName, string userToken = "")
        {
            _getDBvarPayload = new GetDBvarPayload(varName);
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _getDBvarPayload = new ApplicationUserToken(_getDBvarPayload, userToken);
            }
            else
            {
                _getDBvarPayload = new ApplicationTicket(_getDBvarPayload, ticket);
            }
            _getDBvarPayload = new ApplicationToken(_getDBvarPayload, appToken);
            _getDBvarPayload = new WrapPayload(_getDBvarPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _getDBvarPayload.GetXmlPayload(ref parent);
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
