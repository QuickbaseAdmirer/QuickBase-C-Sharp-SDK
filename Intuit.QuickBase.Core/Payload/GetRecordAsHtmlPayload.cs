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
    internal class GetRecordAsHtmlPayload : Payload
    {
        private int _rid;

        internal GetRecordAsHtmlPayload(int rid, bool jht)
            : this(rid)
        {
            Jht = jht;
        }

        internal GetRecordAsHtmlPayload(int rid)
        {
            Rid = rid;
        }

        private int Rid
        {
            get { return _rid; }
            set
            {
                if (value < 1) throw new ArgumentException("rid");
                _rid = value;
            }
        }

        private bool Jht { get; set; }

        internal override void GetXmlPayload(ref XElement parent)
        {
            parent.Add(new XElement("rid", Rid));
            if (Jht) parent.Add(new XElement("jht", 1));
        }
    }
}
