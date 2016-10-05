/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Text.RegularExpressions;

namespace Intuit.QuickBase.Core
{
    public class Field : IField
    {
        private int _fid;
        private string _file;
        private string _value;

        public Field(int fid, string value)
        {
            Fid = fid;
            Value = value;
        }

        public Field(int fid, FieldType type, string value)
            : this(fid, value)
        {
            Type = type;
        }

        public Field(int fid, FieldType type, string value, string file)
            : this(fid, type, value)
        {
            File = file;
        }

        public int Fid
        {
            get { return _fid; }
            private set
            {
                if (value < 1) throw new ArgumentException("fid");
                _fid = value;
            }
        }

        public FieldType Type { get; private set; }

        public string Value
        {
            get
            {
                return _value;
            }
            private set
            {
                if (value == null) throw new ArgumentNullException("value");
                // Okay for value to be an empty string.  That's how to erase values in QB.
                _value = value;
            }
        }

        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("file");
                if (value.Trim() == String.Empty) throw new ArgumentException("file");
                _file = value;
            }
        }
    }
}