/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using Intuit.QuickBase.Core;

namespace Intuit.QuickBase.Client
{
    public interface IQColumn
    {
        int ColumnId { get; set; }
        string ColumnName { get; set; }
        FieldType ColumnType { get; set; }
        bool ColumnVirtual { get; set; }
        bool Equals(IQColumn column);
        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}
