/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Intuit.QuickBase.Core.Payload
{
    internal class FieldChoicesPayload : Payload
    {
        private int _fid;
        private List<string> _choices;

        internal FieldChoicesPayload(int fid, List<string> choices)
        {
            Fid = fid;
            Choices = choices;
        }

        private int Fid
        {
            get { return _fid; }
            set
            {
                if (value < 1) throw new ArgumentException("fid");
                _fid = value;
            }
        }

        private List<string> Choices
        {
            get { return _choices; }
            set
            {
                if (value == null) throw new ArgumentNullException("choices");
                _choices = value;
            }
        }

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            sb.Append(String.Format("<fid>{0}</fid>", Fid));
            foreach(var choice in Choices)
            {
                sb.Append(String.Format("<choice>{0}</choice>", choice));
            }
            return sb.ToString();
        }
    }
}
