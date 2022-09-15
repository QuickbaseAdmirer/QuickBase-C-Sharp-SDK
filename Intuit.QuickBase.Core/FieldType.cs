/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Core
{
    /// <summary>
    /// Specify the QuickBase field type. The eligible type names differ slightly from their 
    /// counterparts in the QuickBase UI.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// Empty Type
        /// </summary>
        empty,

        /// <summary>
        /// CheckBox
        /// </summary>
        checkbox,

        /// <summary>
        /// Database Link
        /// </summary>
        dblink,

        /// <summary>
        /// Date
        /// </summary>
        date,

        /// <summary>
        /// Duration
        /// </summary>
        duration,

        /// <summary>
        /// Email Address
        /// </summary>
        email,

        /// <summary>
        /// Float
        /// </summary>
        @float,

        /// <summary>
        /// Numeric-Currency
        /// </summary>
        currency,

        /// <summary>
        /// Percent
        /// </summary>
        percent,
        
        /// <summary>
        /// Rating
        /// </summary>
        rating,

        /// <summary>
        /// Phone Number
        /// </summary>
        phone,

        /// <summary>
        /// Text
        /// </summary>
        text,

        /// <summary>
        ///  DateTime
        /// </summary>
        timestamp,

        /// <summary>
        /// Time Of Day
        /// </summary>
        timeofday,

        /// <summary>
        /// URL-Link
        /// </summary>
        url,

        /// <summary>
        /// UserID
        /// </summary>
        userid,

        /// <summary>
        /// Multi User Field
        /// </summary>
        multiuserid,

        /// <summary>
        /// address
        /// </summary>
        address,

        /// <summary>
        /// Record ID
        /// </summary>
        recordid,

        /// <summary>
        /// Multi-Select Text
        /// </summary>
        multitext,

        //TODO: not yet supported
        /// <summary>
        /// File Attachment
        /// </summary>
        file,

        /// <summary>
        /// ICalendar entry
        /// </summary>
        icalendarbutton,

        /// <summary>
        /// VCard package
        /// </summary>
        vcardbutton,

        /// <summary>
        /// Project predecessor
        /// </summary>
        predecessor
    }
}