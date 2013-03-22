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
    /// You invoke this call on a table-level dbid to add a new field to the table 
    /// you specify. After you add the field, you’ll need to use 
    /// com.intuit.quickbase.API_SetFieldProperties class to set the properties of the 
    /// new field and any default values. Once you pick the field type using this call, 
    /// you won’t be able to change the field type later using 
    /// com.intuit.quickbase.API_SetFieldProperties. You’ll only be able to change the 
    /// field type from the QuickBase UI. Unsupported tags: &lt;mode&gt;&lt;/mode&gt; 
    /// and &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public sealed class AddField : IQObject
    {
        private const string QUICKBASE_ACTION = "API_AddField";
        private Payload.Payload _addFieldPayload;
        private IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_AddField class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="appToken">Supply application token that is assigned to your QuickBase Application. See QuickBase Online help to obtain an application token.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbid">Supply table-level dbid.</param>
        /// <param name="label"></param>
        /// <param name="type"></param>
        /// <param name="mode"></param>
        public AddField(string ticket, string appToken, string accountDomain, string dbid, string label, FieldType type, Mode mode)
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new AddFieldPayload(label, type, mode));
        }

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_AddField class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="appToken">Supply application token that is assigned to your QuickBase Application. See QuickBase Online help to obtain an application token.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbid">Supply table-level dbid.</param>
        /// <param name="label"></param>
        /// <param name="type"></param>
        public AddField(string ticket, string appToken, string accountDomain, string dbid, string label, FieldType type)
        {
            CommonConstruction(ticket, appToken, accountDomain, dbid, new AddFieldPayload(label, type));
        }

        private void CommonConstruction(string ticket, string appToken, string accountDomain, string dbid, Payload.Payload payload)
        {
            _addFieldPayload = new ApplicationTicket(payload, ticket);
            _addFieldPayload = new ApplicationToken(_addFieldPayload, appToken);
            _addFieldPayload = new WrapPayload(_addFieldPayload);
            _uri = new QUriDbid(accountDomain, dbid);
        }

        public string XmlPayload
        {
            get
            {
                return _addFieldPayload.GetXmlPayload();
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