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
    /// You invoke this call on a table-level dbid to get all of the fields in a record. You could 
    /// use com.intuit.quickbase.API_DoQuery to do the same thing, but this call is lots easier if 
    /// you want all the fields. Unsupported tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class GetRecordInfo : IQObject
    {
        private const string QUICKBASE_ACTION = "API_GetRecordInfo";
        private readonly Payload.Payload _getRecordInfoPayload;
        private readonly IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_GetRecordInfo class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="appToken">Supply application token that is assigned to your QuickBase Application. See QuickBase Online help to obtain an application token.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbid">Supply table-level dbid.</param>
        /// <param name="rid">Supply a record object.</param>
        /// <param name="userToken">option user token that can be used instead of a ticket</param>
        public GetRecordInfo(string ticket, string appToken, string accountDomain, string dbid, int rid, string userToken = "")
        {
            _getRecordInfoPayload = new GetRecordInfoPayload(rid);
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _getRecordInfoPayload = new ApplicationUserToken(_getRecordInfoPayload, userToken);
            }
            else
            {
                _getRecordInfoPayload = new ApplicationTicket(_getRecordInfoPayload, ticket);
            }
            _getRecordInfoPayload = new ApplicationToken(_getRecordInfoPayload, appToken);
            _getRecordInfoPayload = new WrapPayload(_getRecordInfoPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _getRecordInfoPayload.GetXmlPayload(ref parent);
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