/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
using System;
using System.Text;
using System.Xml.Linq;

namespace Intuit.QuickBase.Core.Payload
{
    internal class AddReplaceDBPagePayload : Payload
    {
        private string _pageName;
        private int _pageId;
        private string _pageBody;

        internal AddReplaceDBPagePayload(string pageName, PageType pageType, string pageBody)
        {
            PageName = pageName;
            CommonConstruction(pageType, pageBody);
        }

        internal AddReplaceDBPagePayload(int pageId, PageType pageType, string pageBody)
        {
            PageId = pageId;
            CommonConstruction(pageType, pageBody);
        }

        private void CommonConstruction(PageType pageType, string pageBody)
        {
            PageBody = pageBody;
            PageType = pageType;
        }

        private string PageName
        {
            get { return _pageName; }
            set
            {
                if (value == null) throw new ArgumentNullException("pageName");
                if (value.Trim() == String.Empty) throw new ArgumentException("pageName");
                _pageName = value;
            }
        }

        private int PageId
        {
            get { return _pageId; }
            set
            {
                if (value < 1) throw new ArgumentException("_pageId");
                _pageId = value;
            }
        }

        private PageType PageType { get; set; }

        private string PageBody
        {
            get { return _pageBody; }
            set
            {
                if (value == null) throw new ArgumentNullException("pageBody");
                if (value.Trim() == String.Empty) throw new ArgumentException("pageBody");
                _pageBody = value;
            }
        }

        internal override string GetXmlPayload()
        {
            var sb = new StringBuilder();
            if (PageId > 0) sb.Append(new XElement("pageid", PageId));
            if (!string.IsNullOrEmpty(PageName)) sb.Append(new XElement("pagename", PageName));
            sb.Append(new XElement("pagetype", (int)PageType));
            sb.Append(new XElement("pagebody", PageBody));
            return sb.ToString();
        }
    }
}
