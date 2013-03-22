/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;

namespace Intuit.QuickBase.Core.Uri
{
    internal class QUriMain : QUri
    {
        private const string RESOURCE = "main";
        private string _accountDomain;

        public QUriMain(string accountDomain)
        {
            AccountDomain = accountDomain;
        }

        private string AccountDomain
        {
            get { return _accountDomain; }
            set
            {
                if (value == null) throw new ArgumentNullException("accountDomain");
                if (value.Trim() == String.Empty) throw new ArgumentException("accountDomain");
                _accountDomain = value;
            }
        }

        public override System.Uri GetQUri()
        {
            return new System.Uri(BaseUri(AccountDomain) + RESOURCE); 
        }
    }
}