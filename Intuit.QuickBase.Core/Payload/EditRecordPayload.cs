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
using System.Text;

namespace Intuit.QuickBase.Core.Payload
{
    internal class EditRecordPayload : Payload
    {
        private readonly int _rid;
        private readonly string _updateId;
        private readonly List<IField> _fields;
        private readonly bool _disprec;
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
            _fform = builder.Fform;
        }

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            sb.Append(String.Format("<rid>{0}</rid>", _rid));
            sb.Append(!String.IsNullOrEmpty(_updateId) ? String.Format("<update_id>{0}</update_id>", _updateId) : String.Empty);
            foreach (var field in _fields)
            {
                if (field.Type == FieldType.file)
                {
                    sb.Append(String.Format(
                        "<field fid=\"{0}\" filename=\"{1}\">{2}</field>",
                        field.Fid, field.Value, EncodeFile(field.File)));
                }
                else
                {
                    sb.Append(String.Format(
                        "<field fid=\"{0}\">{1}</field>",
                        field.Fid, field.Value));
                }
            }
            sb.Append(_disprec ? "<disprec/>" : String.Empty);
            sb.Append(_fform ? "<fform/>" : String.Empty);
            return sb.ToString();
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
