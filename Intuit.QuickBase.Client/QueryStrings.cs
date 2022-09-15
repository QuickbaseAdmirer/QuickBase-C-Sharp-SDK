/*
 * Copyright © 2010 Intuit Inc. All rights reserved.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/eclipse-1.0.php
 */
namespace Intuit.QuickBase.Client
{
    public class QueryStrings
    {
        // Constructors
        public QueryStrings(int fieldId, ComparisonOperator comparisonOp, string matchingValue, LogicalOperator logicalOp)
        {
            FieldId = fieldId;
            ComparisonOp = comparisonOp;
            MatchingValue = matchingValue;
            LogicalOp = logicalOp;
        }

        // Properties
        public int FieldId { get; set; }
        public ComparisonOperator ComparisonOp { get; set;}
        public string MatchingValue { get; set; }
        public LogicalOperator LogicalOp { get; set; }

        // Methods
        public override string ToString()
        {
            string logicalOp = string.Empty;

            switch (LogicalOp)
            {
                case LogicalOperator.AND:
                    {
                        logicalOp = LogicalOp.ToString("F");
                        break;
                    }
                case LogicalOperator.OR:
                    {
                        logicalOp = LogicalOp.ToString("F");
                        break;
                    }
            }

            return "{" +
                   "'" + FieldId + "'" +
                   "." +
                   ComparisonOp.ToString("F") +
                   "." +
                   "'" + MatchingValue + "'" +
                   "}" +
                   logicalOp;
        }
    }
}
