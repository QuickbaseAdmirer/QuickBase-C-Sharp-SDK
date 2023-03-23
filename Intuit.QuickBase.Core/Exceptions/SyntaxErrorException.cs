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
    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException() : base() { }

        public SyntaxErrorException(string message) : base(message) { }

        public SyntaxErrorException(string message, Exception innerException) : base(message, innerException) { }

        protected SyntaxErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}