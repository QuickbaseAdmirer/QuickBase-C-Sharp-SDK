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
    internal class ProvisionUserPayload : Payload
    {
        private string _email;
        private int _roleId;
        private string _firstName;
        private string _lastName;

        internal ProvisionUserPayload(string email, int roleId, string firstName, string lastName)
            : this(email, firstName, lastName)
        {
            RoleId = roleId;
        }

        internal ProvisionUserPayload(string email, string firstName, string lastName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        private string Email
        {
            get { return _email; }
            set
            {
                if (value == null) throw new ArgumentNullException("email");
                if (value.Trim() == String.Empty) throw new ArgumentException("email");
                _email = value;
            }
        }

        private int RoleId
        {
            get { return _roleId; }
            set
            {
                if (value < 1) throw new ArgumentException("roleId");
                _roleId = value;
            }
        }

        private string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == null) throw new ArgumentNullException("firstName");
                if (value.Trim() == String.Empty) throw new ArgumentException("firstName");
                _firstName = value;
            }
        }

        private string LastName
        {
            get { return _lastName; }
            set
            {
                if (value == null) throw new ArgumentNullException("lastName");
                if (value.Trim() == String.Empty) throw new ArgumentException("lastName");
                _lastName = value;
            }
        }

        internal override void GetXmlPayload(ref XElement parent)
        {
            parent.Add(new XElement("email", Email));
            parent.Add(new XElement("fname", FirstName));
            parent.Add(new XElement("lname", LastName));
            if (RoleId > 0) parent.Add(new XElement("roleid", RoleId));
        }
    }
}
