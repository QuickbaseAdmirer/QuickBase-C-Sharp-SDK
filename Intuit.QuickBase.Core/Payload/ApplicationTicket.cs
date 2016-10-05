/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Text;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class ApplicationTicket : PayloadDecorator
    {
        private string _ticket;

        internal ApplicationTicket(Payload payload, string ticket)
        {
            Payload = payload;
            Ticket = ticket;
        }

        private string Ticket
        {
            get { return _ticket; }
            set
            {
                if (value == null) throw new ArgumentNullException("ticket");
                if (value.Trim() == String.Empty) throw new ArgumentException("ticket");
                _ticket = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return Payload.GetXmlPayload() + new XElement("ticket", Ticket);
        }
    }
}