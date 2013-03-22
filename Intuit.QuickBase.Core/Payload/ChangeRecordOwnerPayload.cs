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
            return String.Format("<rid>{0}</rid><newowner>{1}</newowner>", Rid, NewOwner);
        }
    }
}
