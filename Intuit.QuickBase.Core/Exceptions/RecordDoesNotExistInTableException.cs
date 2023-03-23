/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Runtime.Serialization;

namespace Intuit.QuickBase.Core.Exceptions
{
    [Serializable]
    public class RecordDoesNotExistInTableException : Exception
    {
        public RecordDoesNotExistInTableException() : base() { }

        public RecordDoesNotExistInTableException(string message) : base(message) { }

        public RecordDoesNotExistInTableException(string message, Exception innerException) : base(message, innerException) { }

        protected RecordDoesNotExistInTableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}