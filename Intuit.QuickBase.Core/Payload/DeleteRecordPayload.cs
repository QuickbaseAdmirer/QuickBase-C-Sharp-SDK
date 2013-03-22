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
    internal class DeleteRecordPayload : Payload
    {
        private int _rid;

        internal DeleteRecordPayload(int rid)
        {
            Rid = rid;
        }

        private int Rid
        {
            get { return _rid; }
            set
            {
                if (value < 0) throw new ArgumentException("rid");
                _rid = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return String.Format("<rid>{0}</rid>", Rid);
        }
    }
}
