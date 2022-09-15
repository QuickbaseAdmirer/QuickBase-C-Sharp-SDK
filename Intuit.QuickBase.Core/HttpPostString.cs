﻿/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core
{
    internal class HttpPostString : HttpPost
    {
        private const string CONTENT_TYPE = "application/xml";
        private const string METHOD = "POST";
        private const string QUICKBASE_HEADER = "QUICKBASE-ACTION: ";

        internal override void Post(IQObject apiAction)
        {
            XElement parent = new XElement("qdbapi");
            apiAction.BuildXmlPayload(ref parent);
            var bytes = Encoding.UTF8.GetBytes(parent.ToString());
            Stream requestStream = null;
            WebResponse webResponse = null;
            Stream responseStream = null;
            string text;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(apiAction.Uri);
                request.Method = METHOD;
                request.ProtocolVersion = HttpVersion.Version10;
                request.ContentType = CONTENT_TYPE;
                request.ContentLength = bytes.Length;
                request.KeepAlive = false;
                request.Timeout = 300000;
                request.Headers.Add(QUICKBASE_HEADER + apiAction.Action);

                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);

                webResponse = request.GetResponse();
                responseStream = webResponse.GetResponseStream();
                text = new StreamReader(responseStream).ReadToEnd();
            }
            finally
            {
                if (requestStream != null) requestStream.Close();
                if (responseStream != null) responseStream.Close();
                if (webResponse != null) webResponse.Close();
            }
            Response = XElement.Parse("<?xml version=\"1.0\"?><response_data><![CDATA[" + text + "]]></response_data>");
        }

        internal override XElement Response { get; set; }
    }
}
