/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;

namespace Intuit.QuickBase.Core.Payload
{
    internal class ApplicationToken : PayloadDecorator
    {
        internal ApplicationToken(Payload payload, string token)
        {
            Payload = payload;
            Token = token;
        }

        private string Token { get; set; }

        internal override string GetXmlPayload()
        {
            return !String.IsNullOrEmpty(Token) ? String.Format("{0}<apptoken>{1}</apptoken>", Payload.GetXmlPayload(), Token) : Payload.GetXmlPayload();
        }
    }
}