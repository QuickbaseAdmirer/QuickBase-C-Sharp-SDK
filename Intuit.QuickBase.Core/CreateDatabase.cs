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
    /// This call creates a new QuickBase application with the main application table 
    /// populated only with the built in fields and optionally generates and returns 
    /// an app token for your API application(s) to use. You’ll need to add desired 
    /// fields using com.intuit.quickbase.API_AddField, set field properties using 
    /// com.intuit.quickbase.API_SetProperties, and optionally add more tables to your 
    /// application using com.intuit.quickbase.API_CreateTable. (Or, just use the 
    /// QuickBase UI to do all this.) Notice the appdbid that gets returned in the 
    /// response, if all goes well. This is the application-level dbid. Some API 
    /// calls require the use of the application-level dbid, and some require a 
    /// table-level dbid. Calls requiring application-level dbids are application wide 
    /// calls, such as com.intuit.quickbase.API_ChangeUserRole. Calls requiring 
    /// table-level dbid are those that manipulate tables, such as 
    /// com.intuit.quickbase.API_AddField or com.intuit.quickbase.API_AddRecord. How do 
    /// you get the table level dbid? com.intuit.quickbase.API_GetSchema returns the 
    /// dbids of all child tables (if you use the application-level dbid!). Typically, 
    /// these dbids differ significantly from the main application dbid. However, the main 
    /// child table, which is built with the application by the API_CreateDatabase call, 
    /// can be nearly identical to the application-level dbid. They might only differ 
    /// by one terminal character, which can be confusing. Unsupported tag: &lt;udata&gt;&lt;/udata&gt;
    /// </summary>
    public class CreateDatabase : IQObject
    {
        private const string QUICKBASE_ACTION = "API_CreateDatabase";
        private readonly Payload.Payload _createDatabasePayload;
        private readonly IQUri _uri;

        /// <summary>
        /// Initializes a new instance of the com.intuit.quickbase.API_CreateDatabase class.
        /// </summary>
        /// <param name="ticket">Supply auth ticket for application access. See com.intuit.quickbase.API_Authenticate class to obtain a ticket.</param>
        /// <param name="accountDomain"></param>
        /// <param name="dbName">Supply a new application name.</param>
        /// <param name="dbDesc">Supply an application description.</param>
        /// <param name="createAppToken">Supply "true" to create a new token, "false" otherwise.</param>
        /// <param name="userToken">a user token that can be used instead of a ticket</param>
        public CreateDatabase(string ticket, string accountDomain, string dbName, string dbDesc, bool createAppToken, string userToken = "")
        {
            _createDatabasePayload = new CreateDatabasePayload(dbName, dbDesc, createAppToken);
            //If a user token is provided, use it instead of a ticket
            if (userToken.Length > 0)
            {
                _createDatabasePayload = new ApplicationUserToken(_createDatabasePayload, userToken);
            }
            else
            {
                _createDatabasePayload = new ApplicationTicket(_createDatabasePayload, ticket);
            }
            _createDatabasePayload = new WrapPayload(_createDatabasePayload);
            _uri = new QUriMain(accountDomain);
        }

        public void BuildXmlPayload(ref XElement parent)
        {
            _createDatabasePayload.GetXmlPayload(ref parent);
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