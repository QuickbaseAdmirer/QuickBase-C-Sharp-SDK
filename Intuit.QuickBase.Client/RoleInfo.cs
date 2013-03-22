/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public class RoleInfo
    {
        public const string ACCESS_NONE = "None";
        public const string ACCESS_BASIC = "Basic Access";
        public const string ACCESS_SHARING = "Basic Access with Share";
        public const string ACCESS_FULL = "Administrator";

        public RoleInfo(int roleId, string name, int accessId, string access)
        {
            RoleId = roleId;
            Name = name;
            AccessId = accessId;
            Access = access;
        }

        public int RoleId { get; private set ; }
        public string Name { get; private set; }
        public int AccessId { get; private set; }
        public string Access { get; private set; }
    }
}