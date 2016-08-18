using System;
using System.Collections.Generic;
using System.Linq;
using Intuit.QuickBase.Client;
using Intuit.QuickBase.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QBFunctionTest.Properties;

namespace QBFunctionTest
{
    [TestClass]
    public class UnitTest1
    {
        private IQApplication qbApp;

        public void InitConnection()
        {
            var client = QuickBase.Login(Settings.Default.qbUser, Settings.Default.qbPass, Settings.Default.qbSiteURL);
            qbApp = client.Connect(Settings.Default.qbAppDBID, Settings.Default.qbAppToken);

        }

        class TestRecord
        {
            public string textVal;
            public decimal floatVal;
            public bool checkboxVal;
            public DateTime dateVal;
            public DateTime timeStampVal;
            public TimeSpan timeOfDayVal;
            public QAddress addressVal;
            public decimal currencyVal;
            public TimeSpan durationVal;
            public string emailVal;
            public string phoneVal;
            public decimal percentVal;

            public TestRecord()
            {
                addressVal = new QAddress();
            }
            public void SetupTestValues()
            {
                textVal = "Test string #1";
                floatVal = 3452.54m;
                checkboxVal = true;
                dateVal = new DateTime(2015, 04, 15);
                timeStampVal = new DateTime(1970, 02, 28, 23, 55, 00, DateTimeKind.Local);
                timeOfDayVal = new DateTime(2000, 1, 1, 12, 34, 56).TimeOfDay;
                durationVal = new TimeSpan(0, 4, 5, 6);
                addressVal.Line1 = "1313 Mockingbird Ln";
                addressVal.City = "Culver City";
                addressVal.Province = "CA";
                addressVal.PostalCode = "90120";
                addressVal.Country = "USA";
                currencyVal = 50.50m;
                emailVal = "test@example.com";
                phoneVal = "(303) 555-1212";
                percentVal = 95.5m;
            }
        }

        [TestMethod]
        public void BasicCreationAndRoundTripTest()
        {
            InitConnection();
            List<GrantedAppsInfo> appsLst = qbApp.GrantedDBs();
            foreach (var app in appsLst)
            {
                foreach (var tab in app.GrantedTables)
                {
                    if (tab.Name == "APITestApp: APITestTable")
                    {
                        IQTable tbl = qbApp.GetTable(tab.Dbid);
                        qbApp.DeleteTable(tbl);
                        break;
                    }
                }
            }

            IQTable testTable = qbApp.NewTable("APITestTable", "dummyRec");
            testTable.Columns.Add(new QColumn("TextTest", FieldType.text));
            testTable.Columns.Add(new QColumn("FloatTest", FieldType.@float));
            testTable.Columns.Add(new QColumn("CheckboxTest", FieldType.checkbox));
            testTable.Columns.Add(new QColumn("DateTest", FieldType.date));
            testTable.Columns.Add(new QColumn("TimeStampTest", FieldType.timestamp));
            testTable.Columns.Add(new QColumn("TimeOfDayTest", FieldType.timeofday));
            testTable.Columns.Add(new QColumn("DurationTest", FieldType.duration));
            //testTable.Columns.Add(new QColumn("AddressTest", FieldType.address));
            testTable.Columns.Add(new QColumn("CurrencyTest", FieldType.currency));
            testTable.Columns.Add(new QColumn("PercentTest", FieldType.percent));
            testTable.Columns.Add(new QColumn("EmailTest", FieldType.email));
            testTable.Columns.Add(new QColumn("PhoneTest", FieldType.phone));

            TestRecord exemplar = new TestRecord();
            exemplar.SetupTestValues();

            IQRecord inRec = testTable.NewRecord();
            inRec["TextTest"] = exemplar.textVal;
            inRec["FloatTest"] = exemplar.floatVal;
            inRec["CheckboxTest"] = exemplar.checkboxVal;
            inRec["DateTest"] = exemplar.dateVal;
            inRec["TimeStampTest"] = exemplar.timeStampVal;
            inRec["TimeOfDayTest"] = exemplar.timeOfDayVal;
            inRec["DurationTest"] = exemplar.durationVal;
            //inRec["AddressTest"] = exemplar.addressVal;
            inRec["CurrencyTest"] = exemplar.currencyVal;
            inRec["PercentTest"] = exemplar.percentVal;
            inRec["EmailTest"] = exemplar.emailVal;
            inRec["PhoneTest"] = exemplar.phoneVal;

            Assert.AreEqual(exemplar.textVal, inRec["TextTest"], "Strings setter fails");
            Assert.AreEqual(exemplar.floatVal, inRec["FloatTest"], "Floats setter fails");
            Assert.AreEqual(exemplar.checkboxVal, inRec["CheckboxTest"], "Checkboxes setter fails");
            Assert.AreEqual(exemplar.dateVal, inRec["DateTest"], "Dates setter fails");
            Assert.AreEqual(exemplar.timeStampVal, inRec["TimeStampTest"], "TimeStamps setter fails");
            Assert.AreEqual(exemplar.timeOfDayVal, inRec["TimeOfDayTest"], "TimeOfDays setter fails");
            Assert.AreEqual(exemplar.durationVal, inRec["DurationTest"], "Durations setter fails");
            Assert.AreEqual(exemplar.currencyVal, inRec["CurrencyTest"], "Currency setter fails");
            Assert.AreEqual(exemplar.percentVal, inRec["PercentTest"], "Percent setter fails");
            Assert.AreEqual(exemplar.emailVal, inRec["EmailTest"], "Email setter fails");
            Assert.AreEqual(exemplar.phoneVal, inRec["PhoneTest"], "Phone setter fails");

            testTable.Records.Add(inRec);
            testTable.AcceptChanges();

            Assert.AreEqual(exemplar.textVal, inRec["TextTest"], "Strings wrong post upload");
            Assert.AreEqual(exemplar.floatVal, inRec["FloatTest"], "Floats wrong post upload");
            Assert.AreEqual(exemplar.checkboxVal, inRec["CheckboxTest"], "Checkboxes wrong post upload");
            Assert.AreEqual(exemplar.dateVal, inRec["DateTest"], "Dates wrong post upload");
            Assert.AreEqual(exemplar.timeStampVal, inRec["TimeStampTest"], "TimeStamps wrong post upload");
            Assert.AreEqual(exemplar.timeOfDayVal, inRec["TimeOfDayTest"], "TimeOfDays wrong post upload");
            Assert.AreEqual(exemplar.durationVal, inRec["DurationTest"], "Durations wrong post upload");
            Assert.AreEqual(exemplar.currencyVal, inRec["CurrencyTest"], "Currency wrong post upload");
            Assert.AreEqual(exemplar.percentVal, inRec["PercentTest"], "Percent wrong post upload");
            Assert.AreEqual(exemplar.emailVal, inRec["EmailTest"], "Email wrong post upload");
            Assert.AreEqual(exemplar.phoneVal, inRec["PhoneTest"], "Phone wrong post upload");

            testTable.Records.Clear();
            testTable.Query();

            IQRecord outRec = testTable.Records[0];
            Assert.AreEqual(exemplar.textVal, outRec["TextTest"], "Strings don't roundtrip");
            Assert.AreEqual(exemplar.floatVal, outRec["FloatTest"], "Floats don't roundtrip");
            Assert.AreEqual(exemplar.checkboxVal, outRec["CheckboxTest"], "Checkboxes don't roundtrip");
            Assert.AreEqual(exemplar.dateVal, outRec["DateTest"], "Dates don't roundtrip");
            Assert.AreEqual(exemplar.timeStampVal, outRec["TimeStampTest"], "TimeStamps don't roundtrip");
            Assert.AreEqual(exemplar.timeOfDayVal, outRec["TimeOfDayTest"], "TimeOfDays don't roundtrip");
            Assert.AreEqual(exemplar.durationVal, outRec["DurationTest"], "Durations don't roundtrip");
            Assert.AreEqual(exemplar.currencyVal, outRec["CurrencyTest"], "Currencies don't roundtrip");
            Assert.AreEqual(exemplar.percentVal, outRec["PercentTest"], "Percents don't roundtrip");
            Assert.AreEqual(exemplar.emailVal, outRec["EmailTest"], "Emails don't roundtrip");
            Assert.AreEqual(exemplar.phoneVal, outRec["PhoneTest"], "Phones don't roundtrip");
        }
    }
}
