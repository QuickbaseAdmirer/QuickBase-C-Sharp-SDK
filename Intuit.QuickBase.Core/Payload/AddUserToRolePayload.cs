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
    internal class AddUserToRolePayload : Payload
    {
        private string _userId;
        private int _roleId;

        internal AddUserToRolePayload(string userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        private string UserId
        {
            get { return _userId; }
            set
            {
                if (value == null) throw new ArgumentNullException("userId");
                if (value.Trim() == String.Empty) throw new ArgumentException("userId");
                _userId = value;
            }
        }

        private int RoleId
        {
            get { return _roleId; }
            set
            {
                if (value < 0) throw new ArgumentException("roleId");
                _roleId = value;
            }
        }

        internal override void GetXmlPayload(ref XElement parent)
        {
            parent.Add(new XElement("userid", UserId));
            parent.Add(new XElement("roleid", RoleId));
        }
    }
}
