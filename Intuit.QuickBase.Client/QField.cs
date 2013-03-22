/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using Intuit.QuickBase.Core;

namespace Intuit.QuickBase.Client
{
    internal class QField
    {
        // Instance fields
        private string _value;

        internal QField(int fieldId)
            : this(fieldId, null, FieldType.empty, null)
        {
        }

        // Constructors
        internal QField(int fieldId, string value, FieldType type, IQRecord record)
        {
            FieldId = fieldId;
            Type = type;
            Record = record; // needs to be before Value.
            Value = value;
        }

        // Properties
        internal int FieldId { get; private set; }
        internal string Value
        {
            get
            {
                switch (Type)
                {
                    case FieldType.timestamp:
                        return ConvertQBMillisecondsToDateTime(_value).ToString();
                    default:
                        return _value;
                }
            }
            set
            {
                if (_value != null)
                {
                    Update = true;
                }
                _value = value;
            }
        }
        internal FieldType Type { get; set; }
        internal IQRecord Record { get; set; }
        internal string FullName { get; set; }
        internal bool Update { get; set; }

        public bool Equals(QField other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.FieldId == FieldId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (QField)) return false;
            return Equals((QField) obj);
        }

        public override int GetHashCode()
        {
            return FieldId;
        }

        public override string ToString()
        {
            return Value;
        }

        private static DateTime ConvertQBMillisecondsToDateTime(string milliseconds)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return new DateTime(date.Ticks + (Int64.Parse(milliseconds) * TimeSpan.TicksPerMillisecond));
        }
    }
}
