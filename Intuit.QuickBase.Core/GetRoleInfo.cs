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
    public class GetRoleInfo : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetRoleInfo";
        private readonly Payload.Payload _getRoleInfoPayload;
        private readonly IQUri _uri;

        public GetRoleInfo(string ticket, string appToken, string accountDomain, string dbid)
        {
            _getRoleInfoPayload = new GetRoleInfoPayload();
            _getRoleInfoPayload = new ApplicationTicket(_getRoleInfoPayload, ticket);
            _getRoleInfoPayload = new ApplicationToken(_getRoleInfoPayload, appToken);
            _getRoleInfoPayload = new WrapPayload(_getRoleInfoPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _getRoleInfoPayload.GetXmlPayload(ref parent);
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
