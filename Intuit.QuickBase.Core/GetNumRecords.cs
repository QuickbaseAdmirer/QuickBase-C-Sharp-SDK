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
    /// Returns the number of records in the table. You need to specify a table-level dbid. Unsupported 
    /// tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class GetNumRecords : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetNumRecords";
        private readonly Payload.Payload _getNumRecordsPayload;
        private readonly IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_GetNumRecords class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="appToken">Supply application token that is assigned to your QuickBase Application. See QuickBase Online help to obtain an application token.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbid">Supply table-level dbid.</param>
        public GetNumRecords(string ticket, string appToken, string accountDomain, string dbid)
        {
            _getNumRecordsPayload = new GetNumRecordsPayload();
            _getNumRecordsPayload = new ApplicationTicket(_getNumRecordsPayload, ticket);
            _getNumRecordsPayload = new ApplicationToken(_getNumRecordsPayload, appToken);
            _getNumRecordsPayload = new WrapPayload(_getNumRecordsPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _getNumRecordsPayload.GetXmlPayload();
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