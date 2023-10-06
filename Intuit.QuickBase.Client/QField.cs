/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Intuit.QuickBase.Core;
using Intuit.QuickBase.Core.Exceptions;

namespace Intuit.QuickBase.Client
{
    internal class QField
    {
        private static readonly Regex CSVUncleanRegEx = new Regex(@"[\r\n]");
        private static readonly DateTime TSOffset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        // Instance fields
        private object _value;

        internal QField(int fieldId)
            : this(fieldId, null, FieldType.empty, null, null)
        {
        }

        // Constructors
        internal QField(int fieldId, object value, FieldType type, IQRecord record, IQColumn column) : this(fieldId, value, type, record, column, false)
        {
        }

        internal QField(int fieldId, object value, FieldType type, IQRecord record, IQColumn column,  bool QBinternal)
        {
            FieldId = fieldId;
            Type = type;
            Record = record; // needs to be before Value.
            Column = column;
            if (QBinternal)
            {
                QBValue = (string)value;
            }
            else
            {
                Value = value;
            }
            if (type == FieldType.text && value != null)
                UncleanText = CSVUncleanRegEx.IsMatch((string)value);
            else
                UncleanText = false;
        }

        // Properties
        internal int FieldId { get; private set; }

        //This gets around a bug in QB's uploadCSV api that will interpret certain characters as EndOfRecord even when surround by quotes
        internal bool UncleanText { get; private set; }

        internal string QBValue
        {
            get
            {
                if (_value == null) return String.Empty;
                switch (Type)
                {
                    case FieldType.address:
                        return string.Empty;
                    case FieldType.timestamp:
                        return ConvertDateTimeToQBMilliseconds((DateTime)_value);
                    case FieldType.date:
                        return ConvertDateToQBMilliseconds((DateTime)_value);
                    case FieldType.timeofday:
                        return ConvertTimeSpanToQBTimeOfDay((TimeSpan)_value);
                    case FieldType.duration:
                        return ConvertTimeSpanToQBDuration((TimeSpan)_value);
                    case FieldType.checkbox:
                        return (bool)_value ? "1" : "0";
                    case FieldType.multitext:
                        return ConvertHashSetToQBMultiSelect((HashSet<int>)_value);
                    default:
                        return _value.ToString();
                }
            }
            set
            {
                UncleanText = false;
                switch (Type)
                {
                    case FieldType.address:
                        // do nothing: child columns will fill this out
                        break;
                    case FieldType.date:
                        _value = String.IsNullOrEmpty(value) ? new DateTime?() : ConvertQBMillisecondsToDate(value);
                        break;
                    case FieldType.timeofday:
                        _value = String.IsNullOrEmpty(value) ? new TimeSpan?() : ConvertQBTimeOfDayToTime(value);
                        break;
                    case FieldType.duration:
                        _value = String.IsNullOrEmpty(value) ? new TimeSpan?() : ConvertQBDurationToTimeSpan(value);
                        break;
                    case FieldType.timestamp:
                        _value = String.IsNullOrEmpty(value) ? new DateTime?() : ConvertQBMillisecondsToDateTime(value);
                        break;
                    case FieldType.checkbox:
                        _value = String.IsNullOrEmpty(value) ? new bool?() : (value == "1" || value == "true");
                        break;
                    case FieldType.rating:
                        _value = String.IsNullOrEmpty(value) ? new float?() : float.Parse(value);
                        break;
                    case FieldType.percent:
                        _value = String.IsNullOrEmpty(value) ? new decimal?() : decimal.Parse(value) * 100; //get around roundtrip bug
                        break;
                    case FieldType.@float:
                    case FieldType.currency:
                        _value = String.IsNullOrEmpty(value) ? new decimal?() : decimal.Parse(value);
                        break;
                    case FieldType.recordid:
                        _value = String.IsNullOrEmpty(value) ? new int?() : int.Parse(value);
                        break;
                    case FieldType.multitext:
                        _value = String.IsNullOrEmpty(value) ? new HashSet<int>() : ConvertQBStringToHashSet(value);
                        break;
                    case FieldType.text:
                        _value = value;
                        UncleanText = CSVUncleanRegEx.IsMatch((string)value);
                        break;
                    default:
                        _value = value;
                        break;
                }
            }
        }

        internal object Value
        {
            get
            {
                switch (Type)
                {
                    case FieldType.address:
                        Dictionary<string, int> colDict = ((IQColumn_int) Column).GetComposites();
                        return new QAddress(
                            (string)Record[colDict["street"]],
                            (string)Record[colDict["street2"]],
                            (string)Record[colDict["city"]],
                            (string)Record[colDict["region"]],
                            (string)Record[colDict["postal"]],
                            (string)Record[colDict["country"]]
                            );
                    default:
                        return _value;
                }
            }
            set
            {
                if (value == null)
                {
                    if (_value != null)
                    {
                        UncleanText = false;
                        _value = null;
                        Update = true;
                    }
                }
                else
                {
                    if (Column.ColumnSummary || Column.ColumnVirtual || Column.ColumnLookup)
                        throw new ArgumentException("Cannot set values of lookup, summary or virtual fields.");
                    if (_value == null || !_value.Equals(value))
                    {
                        UncleanText = false;
                        Update = true;
                        switch (Type)
                        {
                            case FieldType.address:
                                if (value.GetType() != typeof (QAddress))
                                    throw new ArgumentException(
                                        $"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                Dictionary<string, int> colDict = ((IQColumn_int) Column).GetComposites();
                                Record[colDict["street"]] = ((QAddress) value).Line1;
                                Record[colDict["street2"]] = ((QAddress) value).Line2;
                                Record[colDict["city"]] = ((QAddress) value).City;
                                Record[colDict["region"]] = ((QAddress) value).Province;
                                Record[colDict["postal"]] = ((QAddress) value).PostalCode;
                                Record[colDict["country"]] = ((QAddress) value).Country;
                                break;
                            case FieldType.rating:
                                if (value.GetType() != typeof(float) || (float) value < 0 || (float) value > 5)
                                    throw new ArgumentException("Invalid value for 'rating' field.");
                                _value = value;
                                break;
                            case FieldType.date:
                                if (value.GetType() != typeof(DateTime))
                                    throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                _value = ((DateTime) value).Date;
                                break;
                            case FieldType.timestamp:
                                if (value.GetType() != typeof(DateTime))
                                    throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                _value = value;
                                break;
                            case FieldType.duration:
                            case FieldType.timeofday:
                                if (value.GetType() != typeof(TimeSpan))
                                    throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                _value = value;
                                break;
                            case FieldType.@float:
                            case FieldType.currency:
                            case FieldType.percent:
                                decimal? val = value as decimal?;
                                Int32? val2 = value as Int32?;
                                if (val == null && val2 == null)
                                    throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                _value = val ?? val2.Value;
                                break;
                            case FieldType.checkbox:
                                if (value.GetType() != typeof(bool))
                                    throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                _value = value;
                                break;
                            case FieldType.recordid:
                                if (value.GetType() != typeof(int?))
                                   throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                _value = value;
                                break;
                            case FieldType.multitext:
                                if (value.GetType() != typeof(HashSet<int>))
                                    throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                _value = value;
                                break;
                            default:
                                if (value.GetType() != typeof(string))
                                    throw new ArgumentException($"Can't supply type of {value.GetType()} to a {this.Type} field.");
                                if (Type == FieldType.text)
                                    UncleanText = CSVUncleanRegEx.IsMatch((string)value);
                                _value = value;
                                break;
                        }
                    }
                }
            }
        }
        internal FieldType Type { get; set; }
        internal IQRecord Record { get; set; }
        internal IQColumn Column { get; set; }
        internal string FullName { get; set; }
        internal bool Update { get; set; }

        public bool Equals(QField other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.FieldId == FieldId;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
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
            return Value.ToString();
        }

        private static DateTime ConvertQBMillisecondsToDateTime(string milliseconds)
        {
            DateTime dtu = new DateTime(long.Parse(milliseconds) * TimeSpan.TicksPerMillisecond + TSOffset.Ticks, DateTimeKind.Utc);
            return dtu.ToLocalTime();
        }

        private static DateTime ConvertQBMillisecondsToDate(string milliseconds)
        {
            DateTime dtu = new DateTime(long.Parse(milliseconds) * TimeSpan.TicksPerMillisecond + TSOffset.Ticks, DateTimeKind.Utc);
            //return dtu.ToLocalTime().Date;
            return dtu.Date;
        }
        private static TimeSpan ConvertQBDurationToTimeSpan(string milliseconds)
        {
            return new TimeSpan(long.Parse(milliseconds) * TimeSpan.TicksPerMillisecond);
        }

        private static TimeSpan ConvertQBTimeOfDayToTime(string milliseconds)
        {
            return new TimeSpan(long.Parse(milliseconds) * TimeSpan.TicksPerMillisecond);
        }

        private static string ConvertDateTimeToQBMilliseconds(DateTime inDT)
        {
            DateTime dte = inDT.ToUniversalTime();
            return ((dte.Ticks - TSOffset.Ticks) / TimeSpan.TicksPerMillisecond).ToString();
        }

        private static string ConvertDateToQBMilliseconds(DateTime inDT)
        {
            DateTime dte = inDT.Date;
            return ((dte.Ticks - TSOffset.Ticks) / TimeSpan.TicksPerMillisecond).ToString();
        }

        private static string ConvertTimeSpanToQBTimeOfDay(TimeSpan inTime)
        {
            DateTime dt = new DateTime() + inTime;
            return dt.ToString("h:mm tt");
        }

        private static string ConvertTimeSpanToQBDuration(TimeSpan inTime)
        {
            return ($"{inTime.TotalMilliseconds} milliseconds");
        }

        private HashSet<int> ConvertQBStringToHashSet(string value)
        {
            HashSet<int> retVal = new HashSet<int>();
            string[] vals = value.Split(';');
            List<object> optionList = Column.GetChoices();
            if (optionList == null)
                throw new InvalidChoiceException($"No options in field {Column.ColumnName}");
            foreach (string val in vals)
            {
                bool found = false;
                for (int i = 0; i < optionList.Count; i++)
                {
                    if (optionList[i].Equals(val))
                    {
                        retVal.Add(i);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (Column.ColumnSummary || Column.ColumnLookup)
                    {
                        // QB doesn't send options for summary or lookup fields
                        ((IQColumn_int)Column).AddChoice(val, true);
                        retVal.Add(optionList.Count);
                        optionList = Column.GetChoices();
                    }
                    else
                        throw new InvalidChoiceException($"Specified multitext option '{val}' not found in field {Column.ColumnName} in record downloaded from QB?");
                }
            }
            return retVal;
        }

        private string ConvertHashSetToQBMultiSelect(HashSet<int> value)
        {
            List<object> optList = Column.GetChoices();
            List<string> strList = new List<string>();
            foreach (int i in value)
            {
                strList.Add(optList[i].ToString());
            }
            return string.Join(";", strList);
        }
    }
}
