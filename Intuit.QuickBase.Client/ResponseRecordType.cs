/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    internal enum ResponseRecordType
    {
        /// <summary>
        /// Text
        /// </summary>
        text,

        /// <summary>
        /// File Attachment
        /// </summary>
        fileattachment,

        /// <summary>
        /// Numeric
        /// </summary>
        numeric,

        /// <summary>
        /// Date and Time
        /// </summary>
        datetime,

        /// <summary>
        /// Record ID
        /// </summary>
        recordid,

        /// <summary>
        /// URL-Link
        /// </summary>
        url,

        /// <summary>
        /// URL-Link
        /// </summary>
        phonenumber,

        /// <summary>
        /// User
        /// </summary>
        user,

        /// <summary>
        /// Date
        /// </summary>
        date,

        /// <summary>
        /// Time of Day
        /// </summary>
        timeofday,

        /// <summary>
        /// Duration
        /// </summary>
        duration,

        /// <summary>
        /// Checkbox
        /// </summary>
        checkbox,

        /// <summary>
        /// Email Address
        /// </summary>
        email,

        /// <summary>
        /// Report Link
        /// </summary>
        reportlink,

        /// <summary>
        /// ICalendar
        /// </summary>
        icalendar,

        /// <summary>
        /// vCard
        /// </summary>
        vcard,

        /// <summary>
        /// Predecessor
        /// </summary>
        predecessor,

        /// <summary>
        /// Work Date
        /// </summary>
        workdate
    }
}
