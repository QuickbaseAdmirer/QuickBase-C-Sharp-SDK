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
    public class GetDBPage : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetDBPage";
        private readonly Payload.Payload _getDbPagePayload;
        private readonly IQUri _uri;

        public GetDBPage(string ticket, string appToken, string accountDomain, string dbid, int pageId)
        {
            _getDbPagePayload = new GetDBPagePayload(pageId);
            _getDbPagePayload = new ApplicationTicket(_getDbPagePayload, ticket);
            _getDbPagePayload = new ApplicationToken(_getDbPagePayload, appToken);
            _getDbPagePayload = new WrapPayload(_getDbPagePayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _getDbPagePayload.GetXmlPayload(ref parent);
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
