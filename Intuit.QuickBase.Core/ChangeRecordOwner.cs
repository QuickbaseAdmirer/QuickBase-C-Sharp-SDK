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
    public class ChangeRecordOwner : IQObject
    {
        private const string QUICKBASE_ACTION = "API_ChangeRecordOwner";
        private readonly Payload.Payload _changeRecordOwnerPayload;
        private readonly IQUri _uri;

        public ChangeRecordOwner(string ticket, string appToken, string accountDomain, string dbid, int rid, string newOwner, string userToken = "")
        {
            _changeRecordOwnerPayload = new ChangeRecordOwnerPayload(rid, newOwner);
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _changeRecordOwnerPayload = new ApplicationUserToken(_changeRecordOwnerPayload, userToken);
            }
            else
            {
                _changeRecordOwnerPayload = new ApplicationTicket(_changeRecordOwnerPayload, ticket);
            }
            _changeRecordOwnerPayload = new ApplicationToken(_changeRecordOwnerPayload, appToken);
            _changeRecordOwnerPayload = new WrapPayload(_changeRecordOwnerPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _changeRecordOwnerPayload.GetXmlPayload(ref parent);
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
