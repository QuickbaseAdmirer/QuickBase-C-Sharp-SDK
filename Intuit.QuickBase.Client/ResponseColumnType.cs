/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    internal enum ResponseColumnType
    {
        /// <summary>
        /// Date and Time
        /// </summary>
        datetime,

        /// <summary>
        /// File Attachment
        /// </summary>
        file,

        /// <summary>
        /// File Attachment
        /// </summary>
        fileattachment,

        /// <summary>
        /// Numeric
        /// </summary>
        numeric,

        /// <summary>
        /// Currency
        /// </summary>
        currency,

        /// <summary>
        /// Float
        /// </summary>
        @float,

        /// <summary>
        /// Phone Number
        /// </summary>
        phone,

        /// <summary>
        /// Phone Number
        /// </summary>
        phonenumber,

        /// <summary>
        /// Text
        /// </summary>
        text,

        /// <summary>
        /// URL-Link
        /// </summary>
        url,

        /// <summary>
        /// Time-Stamp
        /// </summary>
        timestamp,

        /// <summary>
        /// Record ID
        /// </summary>
        recordid,

        /// <summary>
        /// User ID
        /// </summary>
        userid,

        /// <summary>
        /// User
        /// </summary>
        user,

        ///<summary>
        /// Percent
        ///</summary>
        percent
    }
}
