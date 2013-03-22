/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public class UserInfo
    {
        public UserInfo(string userId, string firstName, string lastName, string login,
                        string email, string screenName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            Email = email;
            ScreenName = screenName;
        }

        public string UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Login { get; private set; }
        public string Email { get; private set; }
        public string ScreenName { get; private set; }
    }
}