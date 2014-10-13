/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.IO;
using Intuit.QuickBase.Core;

namespace Intuit.QuickBase.Client
{
    internal class QField
    {
        private static readonly DateTime qbOffset = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        // Instance fields
        private string _value;

        internal QField(int fieldId)
            : this(fieldId, null, FieldType.empty, null)
        {
        }

        // Constructors
        internal QField(int fieldId, string value, FieldType type, IQRecord record) : this(fieldId, value, type, record, false)
        {
        }

        internal QField(int fieldId, string value, FieldType type, IQRecord record, bool QBinternal)
        {
            FieldId = fieldId;
            Type = type;
            Record = record; // needs to be before Value.
            if (QBinternal)
            {
                _value = value;
            }
            else
            {
                Value = value;
                Update = false;
            }
        }

        // Properties
        internal int FieldId { get; private set; }

        internal string QBValue
        {
            get { return _value; }
        }

        internal string Value
        {
            get
            {
                switch (Type)
                {
                    case FieldType.timestamp:
                    case FieldType.date:
                    case FieldType.timeofday:
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
                switch (Type)
                {
                    case FieldType.timestamp:
                    case FieldType.date:
                    case FieldType.timeofday:
                        _value = ConvertDateTimeStringToQBMilliseconds(value);
                        break;
                    default:
                        _value = value;
                        break;
                }
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
            return new DateTime(qbOffset.Ticks + (Int64.Parse(milliseconds) * TimeSpan.TicksPerMillisecond));
        }

        private static string ConvertDateTimeStringToQBMilliseconds(string inDateStr)
        {
            DateTime inDate;
            if (DateTime.TryParse(inDateStr, out inDate))
            {
                return ((inDate.Ticks - qbOffset.Ticks)/TimeSpan.TicksPerMillisecond).ToString();
            }
            else
            {
                throw new InvalidDataException(String.Format("Can't parse '{0}' into a date format", inDateStr));
            }
        }
    }
}
