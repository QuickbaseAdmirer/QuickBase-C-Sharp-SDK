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
    public class Query
    {
        private readonly List<QueryStrings> _queryStrings = new List<QueryStrings>();

        public void Add(QueryStrings query)
        {
            _queryStrings.Add(query);
        }

        public override string ToString()
        {
            var querys = string.Empty;
            foreach (var query in _queryStrings)
            {
                querys += query.ToString();
            }
            return querys;
        }
    }
}
