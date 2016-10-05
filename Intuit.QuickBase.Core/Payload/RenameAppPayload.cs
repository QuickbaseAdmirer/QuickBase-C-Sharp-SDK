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
    internal class RenameAppPayload : Payload
    {
        private string _newAppName;

        internal RenameAppPayload(string newAppName)
        {
            NewAppName = newAppName;
        }

        private string NewAppName
        {
            get { return _newAppName; }
            set
            {
                if (value == null) throw new ArgumentNullException("newAppName");
                if (value.Trim() == String.Empty) throw new ArgumentException("newAppName");
                _newAppName = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return new XElement("newappname", NewAppName).ToString();
        }
    }
}
