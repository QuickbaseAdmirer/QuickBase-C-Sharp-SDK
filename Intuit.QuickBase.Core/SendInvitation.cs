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
    public class SendInvitation : IQObject
    {
        private const string QUICKBASE_ACTION = "API_SendInvitation";
        private Payload.Payload _sendInvitationPayload;
        private IQUri _uri;

        public SendInvitation(string ticket, string appToken, string accountDomain, string dbid, string userId, string userText)
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new SendInvitationPayload(userId, userText));
        }

        public SendInvitation(string ticket, string appToken, string accountDomain, string dbid, string userId)
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new SendInvitationPayload(userId));
        }

        private void CommonConstruction(string ticket, string appToken, string accountDomain, string dbid, Payload.Payload payload)
        {
            _sendInvitationPayload = new ApplicationTicket(payload, ticket);
            _sendInvitationPayload = new ApplicationToken(_sendInvitationPayload, appToken);
            _sendInvitationPayload = new WrapPayload(_sendInvitationPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _sendInvitationPayload.GetXmlPayload(ref parent);
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
