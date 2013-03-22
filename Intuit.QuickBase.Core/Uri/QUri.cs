/*
 * Copyright © 2013 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Core.Uri
{
    internal abstract class QUri : IQUri
    {
        protected const string PROTOCOL = "https://";
        protected const string SCOPE = "db";

        protected System.Uri BaseUri(string accountDomain)
        {
            return new System.Uri(PROTOCOL + accountDomain + "/" + SCOPE + "/");
        }

        public abstract System.Uri GetQUri();
    }
}