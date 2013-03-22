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
    internal class SetDBvarPayload : Payload
    {
        private string _varName;
        private string _value;

        internal SetDBvarPayload(string varName, string value)
        {
            VarName = varName;
            Value = value;
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

        private string Value
        {
            get { return _value; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value.Trim() == String.Empty) throw new ArgumentException("value");
                this._value = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return String.Format("<varname>{0}</varname><value>{1}</value>", VarName, Value);
        }
    }
}
