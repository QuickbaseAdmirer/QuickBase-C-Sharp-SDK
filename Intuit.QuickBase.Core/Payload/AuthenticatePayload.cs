﻿/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class AuthenticatePayload : Payload
    {
        private string _username;
        private string _password;
        private int _hours;

        internal AuthenticatePayload(string username, string password, int hours)
            : this(username, password)
        {
            Hours = hours;
        }

        internal AuthenticatePayload(string username, string password)
        {
            Username = username;
            Password = password;
        }

        private string Username
        {
            get { return _username; }
            set
            {
                if (value == null) throw new ArgumentNullException("username");
                if (value.Trim() == String.Empty) throw new ArgumentException("username");
                _username = value;
            }
        }

        private string Password
        {
            get { return _password; }
            set
            {
                if (value == null) throw new ArgumentNullException("password");
                if (value.Trim() == String.Empty) throw new ArgumentException("password");
                _password = value;
            }
        }

        private int Hours
        {
            get { return _hours; }
            set
            {
                if (value < 0) throw new ArgumentException("hours");
                _hours = value;
            }
        }

        internal override void GetXmlPayload(ref XElement parent)
        {
            parent.Add(new XElement("username", Username));
            parent.Add(new XElement("password", Password));
            if (Hours > 0) parent.Add(new XElement("hours", Hours));
        }
    }
}
