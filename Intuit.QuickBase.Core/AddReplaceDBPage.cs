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
    public class AddReplaceDBPage : IQObject
    {
        private const string QUICKBASE_ACTION = "API_AddReplaceDBPage";
        private Payload.Payload _addReplaceDBPagePayload;
        private IQUri _uri;

        public AddReplaceDBPage(string ticket, string appToken, string accountDomain, string dbid, string pageName, PageType pageType, string pageBody, string userToken = "")
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new AddReplaceDBPagePayload(pageName, pageType, pageBody), userToken);
        }

        public AddReplaceDBPage(string ticket, string appToken, string accountDomain, string dbid, int pageId, PageType pageType, string pageBody, string userToken = "")
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new AddReplaceDBPagePayload(pageId, pageType, pageBody), userToken);
        }

        private void CommonConstruction(string ticket, string appToken, string accountDomain, string dbid, Payload.Payload payload, string userToken = "")
        {
            if (userToken.Length > 0)
            {
                _addReplaceDBPagePayload = new ApplicationUserToken(payload, userToken);
            }
            else
            {
                _addReplaceDBPagePayload = new ApplicationTicket(payload, ticket);
            }
            _addReplaceDBPagePayload = new ApplicationToken(_addReplaceDBPagePayload, appToken);
            _addReplaceDBPagePayload = new WrapPayload(_addReplaceDBPagePayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _addReplaceDBPagePayload.GetXmlPayload(ref parent);
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
