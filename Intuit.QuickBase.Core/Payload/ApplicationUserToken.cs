/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class ApplicationUserToken : PayloadDecorator
    {
        internal ApplicationUserToken(Payload payload, string token)
        {
            Payload = payload;
            Token = token;
        }

        private string Token { get; set; }

        internal override void GetXmlPayload(ref XElement parent)
        {
            Payload.GetXmlPayload(ref parent);
            if (!string.IsNullOrEmpty(Token)) parent.Add(new XElement("usertoken", Token));
        }
    }
}