/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public enum ComparisonOperator
    {
        ///<summary>
        /// Contains
        ///</summary>
        CT,
        ///<summary>
        /// Does not contain
        ///</summary>
        XCT,
        ///<summary>
        /// Is
        ///</summary>
        EX,
        ///<summary>
        /// True Value
        ///</summary>
        TV,
        ///<summary>
        /// Is not
        ///</summary>
        XEX,
        ///<summary>
        /// Starts with
        ///</summary>
        SW,
        ///<summary>
        /// Does not start with
        ///</summary>
        XSW,
        ///<summary>
        /// Is before
        ///</summary>
        BF,
        ///<summary>
        /// Is on or before
        ///</summary>
        OBF,
        ///<summary>
        /// Is after
        ///</summary>
        AF,
        ///<summary>
        /// Is on or after
        ///</summary>
        OAF,
        ///<summary>
        /// Is less than
        ///</summary>
        LT,
        ///<summary>
        /// Is less than or equal to
        ///</summary>
        LTE,
        ///<summary>
        /// Is greater than
        ///</summary>
        GT,
        ///<summary>
        /// Is greater than or equal to
        ///</summary>
        GTE,
        /// <summary>
        /// Is in range
        /// </summary>
        IR,
        /// <summary>
        /// Is not in range
        /// </summary>
        XIR
    }
}