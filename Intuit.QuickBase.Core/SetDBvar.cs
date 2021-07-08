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
    public class SetDBvar : IQObject
    {
        private const string QUICKBASE_ACTION = "API_SetDBVar";
        private readonly Payload.Payload _setDbVarPayload;
        private readonly IQUri _uri;

        public SetDBvar(string ticket, string appToken, string accountDomain, string dbid, string varName, string value)
        {
            _setDbVarPayload = new SetDBvarPayload(varName, value);
            _setDbVarPayload = new ApplicationTicket(_setDbVarPayload, ticket);
            _setDbVarPayload = new ApplicationToken(_setDbVarPayload, appToken);
            _setDbVarPayload = new WrapPayload(_setDbVarPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _setDbVarPayload.GetXmlPayload(ref parent);
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
