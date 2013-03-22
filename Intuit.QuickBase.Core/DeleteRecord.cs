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
    /// If you have application administration rights, you can use this call to delete a table record. You have to use a 
    /// table-level dbid. If you use an application level dbid you’ll get an error. If you want to delete several records at 
    /// one time, you might want to use com.intuit.quickbase.API_PurgeRecords. Unsupported tag: &lt;qname&gt;&lt;/qname&gt;
    /// </summary>
    public class DeleteRecord : IQObject
    {
        private const string QUICKBASE_ACTION = "API_DeleteRecord";
        private readonly Payload.Payload _deleteRecordPayload;
        private readonly IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_DeleteRecord class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="appToken">Supply application token that is assigned to your QuickBase Application. See QuickBase Online help to obtain an application token.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbid">Supply table-level dbid.</param>
        /// <param name="rid">Supply a record object.</param>
        public DeleteRecord(string ticket, string appToken, string accountDomain, string dbid, int rid)
        {
            _deleteRecordPayload = new DeleteRecordPayload(rid);
            _deleteRecordPayload = new ApplicationTicket(_deleteRecordPayload, ticket);
            _deleteRecordPayload = new ApplicationToken(_deleteRecordPayload, appToken);
            _deleteRecordPayload = new WrapPayload(_deleteRecordPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _deleteRecordPayload.GetXmlPayload();
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