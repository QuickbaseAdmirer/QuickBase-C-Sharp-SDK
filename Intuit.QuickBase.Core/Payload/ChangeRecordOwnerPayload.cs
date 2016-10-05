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
    internal class ChangeRecordOwnerPayload : Payload
    {
        private int _rid;
        private string _newOwner;

        internal ChangeRecordOwnerPayload(int rid, string newOwner)
        {
            Rid = rid;
            NewOwner = newOwner;
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

        private string NewOwner
        {
            get { return _newOwner; }
            set
            {
                if (value == null) throw new ArgumentNullException("newOwner");
                if (value.Trim() == String.Empty) throw new ArgumentException("newOwner");
                _newOwner = value;
            }
        }

        internal override string GetXmlPayload()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new XElement("rid", Rid));
            sb.Append(new XElement("newowner", NewOwner));
            return sb.ToString();
        }
    }
}
