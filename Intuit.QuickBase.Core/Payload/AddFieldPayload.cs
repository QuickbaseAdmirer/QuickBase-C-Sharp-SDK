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
    internal class AddFieldPayload : Payload
    {
        private string _label;

        internal AddFieldPayload(string label, FieldType type, Mode mode)
            : this(label, type)
        {
            Mode = mode;
        }

        internal AddFieldPayload(string label, FieldType type)
        {
            Label = label;
            Type = type;
        }

        private string Label
        {
            get { return _label; }
            set
            {
                if (value == null) throw new ArgumentNullException("label");
                if (value.Trim() == String.Empty) throw new ArgumentException("label");
                _label = value;
            }
        }

        private FieldType Type { get; set; }

        private Mode Mode { get; set; }

        internal override void GetXmlPayload(ref XElement parent)
        {
            parent.Add(new XElement("label", Label));
            parent.Add(new XElement("type", Type));
            if (Mode != Mode.none) parent.Add(new XElement("mode", Mode));
        }
    }
}