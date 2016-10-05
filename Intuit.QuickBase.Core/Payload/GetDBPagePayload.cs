/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class GetDBPagePayload : Payload
    {
        private int _pageId;

        internal GetDBPagePayload(int pageId)
        {
            PageId = pageId;
        }

        private int PageId
        {
            get { return _pageId; }
            set
            {
                if (value < 1) throw new ArgumentException("pageId");
                _pageId = value;
            }
        }

        internal override string GetXmlPayload()
        {
            return new XElement("pageID", PageId).ToString();
        }
    }
}
