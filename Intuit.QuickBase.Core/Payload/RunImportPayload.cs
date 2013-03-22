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
    internal class RunImportPayload : Payload
    {
        private int _id;

        internal RunImportPayload(int id)
        {
            Id = id;
        }

        private int Id
        {
            get { return _id; }
            set
            {
                if (value < 1) throw new ArgumentException("id");
                _id = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return String.Format("<id>{0}</id>", Id);
        }
    }
}
