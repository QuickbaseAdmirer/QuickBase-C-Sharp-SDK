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
    public class SignInRequiredException : Exception
    {
        public SignInRequiredException() : base() { }

        public SignInRequiredException(string message) : base(message) { }

        public SignInRequiredException(string message, Exception innerException) : base(message, innerException) { }

        protected SignInRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}