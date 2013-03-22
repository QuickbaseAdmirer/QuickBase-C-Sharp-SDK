/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Collections.Generic;

namespace Intuit.QuickBase.Client
{
    public class UserRoleInfo
    {
        private readonly List<RoleInfo> _roleInfos;

        public UserRoleInfo(string userId, string name)
        {
            UserId = userId;
            Name = name;
            _roleInfos = new List<RoleInfo>();
        }

        public string UserId { get; private set; }
        public string Name { get; private set; }

        public void AddRole(int roleId, string name, int accessId, string access)
        {
            _roleInfos.Add(new RoleInfo(roleId, name, accessId, access));
        }

        public List<RoleInfo> Roles
        {
            get
            {
                return _roleInfos;
            }
        }
    }
}