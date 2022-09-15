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
    internal class CreateTablePayload : Payload
    {
        private readonly string _tName;
        private readonly string _pNoun;

        internal class Builder
        {
            internal string TName { get; private set; }
            internal Builder SetTName(string val)
            {
                TName = val;
                return this;
            }

            internal string PNoun { get; private set; }
            internal Builder SetPNoun(string val)
            {
                PNoun = val;
                return this;
            }

            internal CreateTablePayload Build()
            {
                return new CreateTablePayload(this);
            }
        }

        internal CreateTablePayload(Builder builder)
        {
            _tName = builder.TName;
            _pNoun = builder.PNoun;
        }

        internal override void GetXmlPayload(ref XElement parent)
        {
            if (!string.IsNullOrEmpty(_tName)) parent.Add(new XElement("tname", _tName));
            if (!string.IsNullOrEmpty(_pNoun)) parent.Add(new XElement("pnoun", _pNoun));
        }
    }
}
