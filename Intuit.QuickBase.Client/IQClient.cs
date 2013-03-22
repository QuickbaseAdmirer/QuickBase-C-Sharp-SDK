/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System.Collections.Generic;

namespace Intuit.QuickBase.Client
{
    public interface IQClient
    {
        IQApplication Connect(string applicationId, string token);
        IQApplication Connect(string applicationId);
        void Disconnect(IQApplication application);
        void Logout();
        IQApplication CreateApplication(string qbName, string qbDescription, CreateApplicationToken createApplicationToken);
        List<string> FindApplication(string qbName);
        string Ticket { get; }
        string ClientUserName { get; }
        string ClientPassword { get; }
        string AdminUserName { get; }
        string AdminPassword { get; }
        string AccountDomain { get; }
    }
}