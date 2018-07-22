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
    /// You can invoke this on the application-level dbid or on a table-level dbid to get metadata information, 
    /// such as the last time the table was modified. For example, you might use this function to find out if 
    /// the table has changed since you last used it, or to find out if a new record has been added to the table. 
    /// Unsupported tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class GetDbInfo : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetDBInfo";
        private readonly Payload.Payload _getDbInfoPayload;
        private readonly IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_GetDBInfo class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="appToken">Supply application token that is assigned to your QuickBase Application. See QuickBase Online help to obtain an application token.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbid">Supply application-level or table-level dbid.</param>
        public GetDbInfo(string ticket, string appToken, string accountDomain, string dbid)
        {
            _getDbInfoPayload = new GetDbInfoPayload();
            _getDbInfoPayload = new ApplicationTicket(_getDbInfoPayload, ticket);
            _getDbInfoPayload = new ApplicationToken(_getDbInfoPayload, appToken);
            _getDbInfoPayload = new WrapPayload(_getDbInfoPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _getDbInfoPayload.GetXmlPayload(ref parent);
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