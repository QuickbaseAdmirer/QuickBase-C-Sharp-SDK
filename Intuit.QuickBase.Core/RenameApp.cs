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
    /// <summary>
    /// You invoke this call on an application-level dbid to change the application name. No dbids, fids, or 
    /// anything other than the application name is affected. You must have administrator rights to call this. 
    /// Unsupported tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class RenameApp : IQObject
    {
        private const string QUICKBASE_ACTION = "API_RenameApp";
        private readonly Payload.Payload _renameAppPayload;
        private readonly IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_RenameApp class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="appToken">Supply application token that is assigned to your QuickBase Application. See QuickBase Online help to obtain an application token.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbid">Supply application-level dbid.</param>
        /// <param name="newAppName">Supply a new application name.</param>
        public RenameApp(string ticket, string appToken, string accountDomain, string dbid, string newAppName)
        {
            _renameAppPayload = new RenameAppPayload(newAppName);
            _renameAppPayload = new ApplicationTicket(_renameAppPayload, ticket);
            _renameAppPayload = new ApplicationToken(_renameAppPayload, appToken);
            _renameAppPayload = new WrapPayload(_renameAppPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _renameAppPayload.GetXmlPayload();
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