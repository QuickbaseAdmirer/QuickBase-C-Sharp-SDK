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
    public class GetDBvar : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetDBVar";
        private readonly Payload.Payload _getDBvarPayload;
        private readonly IQUri _uri;

        public GetDBvar(string ticket, string appToken, string accountDomain, string dbid, string varName)
        {
            _getDBvarPayload = new GetDBvarPayload(varName);
            _getDBvarPayload = new ApplicationTicket(_getDBvarPayload, ticket);
            _getDBvarPayload = new ApplicationToken(_getDBvarPayload, appToken);
            _getDBvarPayload = new WrapPayload(_getDBvarPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _getDBvarPayload.GetXmlPayload();
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
