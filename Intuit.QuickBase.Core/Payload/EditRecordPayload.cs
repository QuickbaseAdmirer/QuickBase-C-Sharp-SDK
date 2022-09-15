/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class EditRecordPayload : Payload
    {
        private readonly int _rid;
        private readonly string _updateId;
        private readonly List<IField> _fields;
        private readonly bool _disprec;
        private readonly bool _timeInUtc;
        private readonly bool _fform;

        internal class Builder
        {
            internal Builder(int rid, List<IField> fields)
            {
                Rid = rid;
                Fields = fields;
            }

            internal int Rid { get; set; }
            internal List<IField> Fields { get; set; }

            internal string UpdateId { get; private set; }
            internal Builder SetUpdateId(string val)
            {
                UpdateId = val;
                return this;
            }

            internal bool Disprec { get; private set; }
            internal Builder SetDisprec(bool val)
            {
                Disprec = val;
                return this;
            }

            internal bool TimeInUtc { get; private set; }
            internal Builder SetTimeInUtc(bool val)
            {
                TimeInUtc = val;
                return this;
            }

            internal bool Fform { get; private set; }
            internal Builder SetFform(bool val)
            {
                Fform = val;
                return this;
            }

            internal EditRecordPayload Build()
            {
                return new EditRecordPayload(this);
            }
        }

        private EditRecordPayload(Builder builder)
        {
            _rid = builder.Rid;
            _updateId = builder.UpdateId;
            _fields = builder.Fields;
            _disprec = builder.Disprec;
            _timeInUtc = builder.TimeInUtc;
            _fform = builder.Fform;
        }

        internal override void GetXmlPayload(ref XElement parent)
        {
            parent.Add(new XElement("rid", _rid));
            if (!string.IsNullOrEmpty(_updateId)) parent.Add(new XElement("update_id", _updateId));
            foreach (var field in _fields)
            {
                if (field.Type == FieldType.file)
                {
                    XElement xelm = new XElement("field", EncodeFile(field.File));
                    xelm.SetAttributeValue("fid", field.Fid);
                    xelm.SetAttributeValue("filename", field.Value);
                    parent.Add(xelm);
                }
                else
                {
                    XElement xelm = new XElement("field", field.Value);
                    xelm.SetAttributeValue("fid", field.Fid);
                    parent.Add(xelm);
                }
            }
            if (_disprec) parent.Add(new XElement("disprec"));
            if (_timeInUtc) parent.Add(new XElement("msInUtc", 1));
            if (_fform) parent.Add(new XElement("fform"));
        }

        private static string EncodeFile(string file)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                var filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                return Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
    }
}
