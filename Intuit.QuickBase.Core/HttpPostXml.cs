/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Intuit.QuickBase.Core
{
    internal class HttpPostXml : HttpPost
    {
        private const string CONTENT_TYPE = "application/xml";
        private const string METHOD = "POST";
        private const string QUICKBASE_HEADER = "QUICKBASE-ACTION: ";

        internal override void Post(IQObject qObject)
        {
            XElement parent = new XElement("qdbapi"); ;
            qObject.BuildXmlPayload(ref parent);
            var bytes = Encoding.UTF8.GetBytes(parent.ToString());
            //File.AppendAllText(@"C:\Temp\QBDebugLog.txt", "**Sent->>" + qObject.Uri + " " + QUICKBASE_HEADER + qObject.Action + "\r\n" + parent + "\r\n");
            Stream requestStream = null;
            WebResponse webResponse = null;
            Stream responseStream = null;
            XElement xml;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(qObject.Uri);
                request.Method = METHOD;
                request.ProtocolVersion = HttpVersion.Version11;
                request.ContentType = CONTENT_TYPE;
                request.ContentLength = bytes.Length;
                request.KeepAlive = false;
                request.Timeout = 300000;
                request.Headers.Add(QUICKBASE_HEADER + qObject.Action);

                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);

                webResponse = request.GetResponse();
                responseStream = webResponse.GetResponseStream();
                xml = XElement.Load(responseStream, LoadOptions.PreserveWhitespace);
                //File.AppendAllText(@"C:\Temp\QBDebugLog.txt", "**Received-<<\r\n" + xml.CreateNavigator().InnerXml + "\r\n");
            }
            finally
            {
                requestStream?.Close();
                responseStream?.Close();
                webResponse?.Close();
            }
        
            Http.CheckForException(xml);
            Response = xml;
        }

        internal override XElement Response { get; set; }
    }
}
