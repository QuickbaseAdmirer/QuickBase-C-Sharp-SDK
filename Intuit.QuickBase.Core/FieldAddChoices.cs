/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Collections.Generic;
using System.Xml.XPath;
using Intuit.QuickBase.Core.Payload;
using Intuit.QuickBase.Core.Uri;

namespace Intuit.QuickBase.Core
{
    public class FieldAddChoices : IQObject
    {
        private const string QUICKBASE_ACTION = "API_FieldAddChoices";
        private readonly Payload.Payload _fieldAddChoicesPayload;
        private readonly IQUri _uri;

        public FieldAddChoices(string ticket, string appToken, string accountDomain, string dbid, int fid, List<string> choices)
        {
            _fieldAddChoicesPayload = new FieldChoicesPayload(fid, choices);
            _fieldAddChoicesPayload = new ApplicationTicket(_fieldAddChoicesPayload, ticket);
            _fieldAddChoicesPayload = new ApplicationToken(_fieldAddChoicesPayload, appToken);
            _fieldAddChoicesPayload = new WrapPayload(_fieldAddChoicesPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _fieldAddChoicesPayload.GetXmlPayload();
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
