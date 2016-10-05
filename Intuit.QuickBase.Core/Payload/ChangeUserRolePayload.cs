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
    internal class ChangeUserRolePayload : Payload
    {
        private string _userId;
        private int _roleId;
        private int _newRoleId;

        internal ChangeUserRolePayload(string userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        internal ChangeUserRolePayload(string userId, int roleId, int newRoldId)
            : this(userId, roleId)
        {
            NewRoleId = newRoldId;
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
                // value of 0 okay
                if (value < 0) throw new ArgumentException("roleId");
                _roleId = value;
            }
        }
        
        private int NewRoleId
        {
            get { return _newRoleId; }
            set
            {
                if (value < 1) throw new ArgumentException("newRoleId");
                _newRoleId = value;
            }
        }

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            sb.Append(new XElement("userid", UserId));
            sb.Append(new XElement("roleid", RoleId));
            if (NewRoleId > 0)
                sb.Append(new XElement("newRoleid", NewRoleId));
            else
                sb.Append(new XElement("newRoleid"));
            return sb.ToString();
        }
    }
}
