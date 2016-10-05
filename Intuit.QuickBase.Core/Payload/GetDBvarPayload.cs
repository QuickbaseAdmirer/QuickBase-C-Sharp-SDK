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
    internal class GetDBvarPayload : Payload
    {
        private string _varName;

        internal GetDBvarPayload(string varName)
        {
            VarName = varName;
        }

        private string VarName
        {
            get { return _varName; }
            set
            {
                if (value == null) throw new ArgumentNullException("varName");
                if (value.Trim() == String.Empty) throw new ArgumentException("varName");
                _varName = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return new XElement("varname", VarName).ToString();
        }
    }
}
