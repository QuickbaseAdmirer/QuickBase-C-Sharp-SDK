﻿/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    internal abstract class QTableFactoryBase
    {
        internal abstract IQTable CreateInstance(IQApplication application, string tableName, string pNoun);

        internal abstract IQTable CreateInstance(IQApplication application, string tableId);

        internal abstract IQTable CreateInstanceLazy(IQApplication application, string tableId);
    }
}
