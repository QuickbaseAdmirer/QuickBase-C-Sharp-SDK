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
    /// You use this call to get the application-level dbid of an application whose name you know. Only those applications 
    /// that granted you access rights will be searched. Because you can have multiple applications with the same name, 
    /// you should be aware that more than one application dbid can be returned. Unsupported tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class FindDbByName : IQObject
    {
        private const string QUICKBASE_ACTION = "API_FindDBByName";
        private readonly Payload.Payload _findDbByNamePayload;
        private readonly IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_FindDBByName class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbName">Supply application name to search for.</param>
        /// <param name="userToken">option user token that can be used instead of ticket</param>
        public FindDbByName(string ticket, string accountDomain, string dbName, string userToken = "")
        {
            _findDbByNamePayload = new FindDbByNamePayload(dbName);
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _findDbByNamePayload = new ApplicationUserToken(_findDbByNamePayload, userToken);
            }
            else
            {
                _findDbByNamePayload = new ApplicationTicket(_findDbByNamePayload, ticket);
            }
            _findDbByNamePayload = new WrapPayload(_findDbByNamePayload);
            _uri = new QUriMain(accountDomain);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _findDbByNamePayload.GetXmlPayload(ref parent);
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