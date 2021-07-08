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
    /// <summary>
    /// This call is used for those implementations that make use of the TICKET cookie rather than the &lt;ticket&gt; 
    /// parameter. Invoking this call returns a null ticket cookie (with the name TICKET) that in some cases results in 
    /// preventing applications at the local machine from accessing QuickBase applications until 
    /// com.intuit.quickbase.API_Authenticate is called for a new ticket cookie. However, this call does not invalidate 
    /// any tickets, nor log off the caller from any QuickBase applications, nor prevent further access of QuickBase 
    /// applications. If the caller has saved a valid ticket, the user can continue to use it. 
    /// Unsupported tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class SignOut : IQObject
    {
        private const string QUICKBASE_ACTION = "API_SignOut";
        private readonly Payload.Payload _signOutPayload;
        private readonly IQUri _uri;

        public SignOut(string accountDomain)
        {
            _signOutPayload = new SignOutPayload();
            _signOutPayload = new WrapPayload(_signOutPayload);
            _uri = new QUriMain(accountDomain);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _signOutPayload.GetXmlPayload(ref parent);
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