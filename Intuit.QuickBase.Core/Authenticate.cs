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
    /// You invoke this call on \db\main (no dbid) to get a ticket. This call validates 
    /// the supplied user name and password, and, if successful, returns a ticket that 
    /// is supplied in subsequent API calls. The ticket is valid for eight hours, unless 
    /// you specify a different value in the &lt;hours&gt; parameter. In addition to the 
    /// ticket, a cookie is also returned with the name TICKET. This is the only API 
    /// call that returns a ticket cookie. API_Authenticate is the equivalent of 
    /// logging into QuickBase. Remember that merely logging into QuickBase by itself 
    /// does not give you access rights to even a single QuickBase application. You 
    /// must first be assigned a role in the application by someone who has administrator 
    /// rights. The same holds true with the ticket you get from this call. By itself, the 
    /// ticket returned from this call does not confer ANY access rights to any QuickBase 
    /// application. The user has to be given a role first, either by 
    /// com.intuit.quickbase.API_AddUserToRole or by com.intuit.quickbase.API_ProvisionUser.
    /// Unsupported tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class Authenticate : IQObject
    {
        private const string QUICKBASE_ACTION = "API_Authenticate";
        private Payload.Payload _authenticatePayload;
        private IQUri _uri;


        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_Authenticate class.
        /// </summary>
        /// <param name="username">Supply QuickBase login username.</param>
        /// <param name="password">Supply QuickBase login password.</param>
        /// <param name="accountDomain">Supply QuickBase account domain.</param>
        /// <param name="hours">Supply hours for a ticket to be valid for.</param>
        public Authenticate(string username, string password, string accountDomain, int hours)
        {
            CommonConstruction(new AuthenticatePayload(username, password, hours), accountDomain);
        }

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_Authenticate class.
        /// </summary>
        /// <param name="username">Supply QuickBase login username.</param>
        /// <param name="password">Supply QuickBase login password.</param>
        /// <param name="accountDomain">Supply QuickBase account domain.</param>
        public Authenticate(string username, string password, string accountDomain)
        {
            CommonConstruction(new AuthenticatePayload(username, password), accountDomain);
        }

        private void CommonConstruction(Payload.Payload payload, string accountDomain)
        {
            _authenticatePayload = new WrapPayload(payload);
            _uri = new QUriMain(accountDomain);
        }

        public string XmlPayload
        {
            get
            {
                return _authenticatePayload.GetXmlPayload();
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