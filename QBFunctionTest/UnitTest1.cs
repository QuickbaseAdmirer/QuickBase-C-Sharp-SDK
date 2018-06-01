using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Intuit.QuickBase.Client;
using Intuit.QuickBase.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QBFunctionTest
{
    [TestClass]
    public class UnitTest1
    {
        private IQApplication qbApp = null;
        private Dictionary<string, string> qbSettings = new Dictionary<string, string>();


        private void LoadSettings()
        {
            try
            {
                XDocument setDoc = XDocument.Load("TestConfigs.xml");
                foreach (XElement xNod in setDoc.Root.Descendants())
                {
                    qbSettings.Add(xNod.Attribute("name").Value, xNod.Value);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Can't load TestConfigs.xml file: " + ex.Message, ex);
            }
        }

        public void InitConnection()
        {
            if (qbApp == null)
            {
                LoadSettings();
                var client = QuickBase.Login(qbSettings["qbUser"], qbSettings["qbPass"], qbSettings["qbSiteURL"]);
                qbApp = client.Connect(qbSettings["qbAppDBID"], qbSettings["qbAppToken"]);
            }
        }

        class TestRecord
        {
            public string textVal;
            public decimal floatVal;
            public bool checkboxVal;
            public DateTime dateVal;
            public DateTime timeStampVal;
            public TimeSpan timeOfDayVal;
            public decimal currencyVal;
            public TimeSpan durationVal;
            public string emailVal;
            public string phoneVal;
            public decimal percentVal;
            public string urlVal;

            public TestRecord()
            {
            }

            public void SetupTestValues()
            {
                textVal = "Test string #1";
                floatVal = 3452.54m;
                checkboxVal = true;
                dateVal = new DateTime(2015, 04, 15);
                timeStampVal = new DateTime(1970, 02, 28, 23, 55, 00, DateTimeKind.Local);
                timeOfDayVal = new TimeSpan(0, 12, 34, 0);
                durationVal = new TimeSpan(0, 4, 5, 6);
                currencyVal = 50.50m;
                emailVal = "test@example.com";
                phoneVal = "(303) 555-1212";
                percentVal = 95.5m;
                urlVal = "http://www.example.com";
            }

            public void Setup2ndValues()
            {
                textVal = "Test string #2 & \"an ampersand\".";
                floatVal = 1234.56m;
                checkboxVal = false;
                dateVal = new DateTime(2010, 01, 12);
                timeStampVal = new DateTime(1971, 03, 24, 23, 55, 11, DateTimeKind.Local);
                timeOfDayVal = new TimeSpan(0, 23, 45, 0);
                durationVal = new TimeSpan(1, 2, 3, 4);
                currencyVal = 25.25m;
                emailVal = "test2@sample.com";
                phoneVal = "(719) 555-1212";
                percentVal = 95.5m;
                urlVal = "http://www.sample.com";
            }
        }

        [TestMethod]
        public void DeletionTest()
        {
            InitConnection();
            List<GrantedAppsInfo> appsLst = qbApp.GrantedDBs();
            foreach (var app in appsLst)
            {
                foreach (var tab in app.GrantedTables)
                {
                    if (tab.Name == "APITestApp: APIDelTestTable")
                    {
                        IQTable tbl = qbApp.GetTable(tab.Dbid);
                        qbApp.DeleteTable(tbl);
                        break;
                    }
                }
            }
            IQTable testTable = qbApp.NewTable("APIDelTestTable", "dummyRec");
            testTable.Columns.Add(new QColumn("NumberValue", FieldType.@float));
            testTable.Columns.Add(new QColumn("TextValue", FieldType.text));

            IQRecord newRec = testTable.NewRecord();
            newRec["NumberValue"] = 0;
            newRec["TextValue"] = "Zeroeth";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 1;
            newRec["TextValue"] = "First";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 2;
            newRec["TextValue"] = "Second";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 3;
            newRec["TextValue"] = "Third";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 4;
            newRec["TextValue"] = "Fourth";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 5;
            newRec["TextValue"] = "Fifth";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 6;
            newRec["TextValue"] = "Sixth";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 7;
            newRec["TextValue"] = "Seventh";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 8;
            newRec["TextValue"] = "Eighth";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 9;
            newRec["TextValue"] = "Ninth";
            testTable.Records.Add(newRec);
            newRec = testTable.NewRecord();
            newRec["NumberValue"] = 10;
            newRec["TextValue"] = "Tenth";
            testTable.Records.Add(newRec);
            testTable.AcceptChanges();

            testTable.Records.RemoveAt(10);
            testTable.Records.RemoveAt(8);
            testTable.Records.RemoveAt(7);
            testTable.Records.RemoveAt(6);
            testTable.Records.RemoveAt(3);
            testTable.Records.RemoveAt(1);
            testTable.AcceptChanges();

            testTable.Query();
            Assert.AreEqual(testTable.Records.Count, 5, "Record deletion fails");
        }

        [TestMethod]
        public void LargeDeleteHandling()
        {
            InitConnection();
            List<GrantedAppsInfo> appsLst = qbApp.GrantedDBs();
            foreach (var app in appsLst)
            {
                foreach (var tab in app.GrantedTables)
                {
                    if (tab.Name == "APITestApp: APIBigDelTestTable")
                    {
                        IQTable tbl = qbApp.GetTable(tab.Dbid);
                        qbApp.DeleteTable(tbl);
                        break;
                    }
                }
            }
            IQTable testTable = qbApp.NewTable("APIBigDelTestTable", "dummyRec");
            testTable.Columns.Add(new QColumn("NumberValue", FieldType.@float));
            testTable.Columns.Add(new QColumn("TextValue", FieldType.text));

            for (int i = 0; i < 500; i++)
            {
                IQRecord newRec = testTable.NewRecord();
                newRec["NumberValue"] = i;
                newRec["TextValue"] = "Record " + i;
                testTable.Records.Add(newRec);
            }
            testTable.AcceptChanges();

            List<int> delList = new List<int>();
            delList.Add(5);
            delList.Add(6);
            delList.Add(7);
            delList.Add(8);
            delList.Add(9);
            delList.Add(10);
            Random rndSrc = new Random();
            while (delList.Count < 120)
            {
                int addVal = rndSrc.Next(500);
                if (!delList.Contains(addVal)) delList.Add(addVal);
            }
            foreach (int i in delList.OrderByDescending(x => x))
            {
                testTable.Records.RemoveAt(i);
            }
            testTable.AcceptChanges();

            testTable.Query();
            Assert.AreEqual(testTable.Records.Count, 380, "Big Record deletion fails");
        }

#if false           //Turning off this test as it requires external setup... will try to make a randomly generated huge table later
        [TestMethod]
        public void LargeTableHandling()
        {
            InitConnection();
            IQTable orderTable = qbApp.GetTable(qbSettings["qbBigTable"]);
            Query qry = new Query();
            QueryStrings lstQry = new QueryStrings(1, ComparisonOperator.IR, "last 60 d",
                LogicalOperator.NONE);
            qry.Add(lstQry);
            int maxRec = 100000;
            orderTable.Query(qry, string.Format("skp-10.num-{0}", maxRec));
            Assert.AreEqual(maxRec, orderTable.Records.Count);
            HashSet<string> idLst = new HashSet<string>();
            foreach (QRecord rec in orderTable.Records)
            {
                string id = (string)rec["Record ID#"];
                if (idLst.Contains(id))
                    Assert.Fail("Duplicate ID found!");
                else
                    idLst.Add(id);
            }
        }
#endif

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
            //testTable.Columns.Add(new QColumn("DurationTest", FieldType.duration));
            testTable.Columns.Add(new QColumn("CurrencyTest", FieldType.currency));
            testTable.Columns.Add(new QColumn("PercentTest", FieldType.percent));
            testTable.Columns.Add(new QColumn("EmailTest", FieldType.email));
            testTable.Columns.Add(new QColumn("PhoneTest", FieldType.phone));
            testTable.Columns.Add(new QColumn("UrlTest", FieldType.url));
            //testTable.Columns.Add(new QColumn("FileTest", FieldType.file));

            TestRecord exemplar = new TestRecord();
            exemplar.SetupTestValues();

            IQRecord inRec = testTable.NewRecord();
            inRec["TextTest"] = exemplar.textVal;
            inRec["FloatTest"] = exemplar.floatVal;
            inRec["CheckboxTest"] = exemplar.checkboxVal;
            inRec["DateTest"] = exemplar.dateVal;
            inRec["TimeStampTest"] = exemplar.timeStampVal;
            inRec["TimeOfDayTest"] = exemplar.timeOfDayVal;
            //inRec["DurationTest"] = exemplar.durationVal;
            inRec["CurrencyTest"] = exemplar.currencyVal;
            inRec["PercentTest"] = exemplar.percentVal;
            inRec["EmailTest"] = exemplar.emailVal;
            inRec["PhoneTest"] = exemplar.phoneVal;
            inRec["UrlTest"] = exemplar.urlVal;

            Assert.AreEqual(exemplar.textVal, inRec["TextTest"], "Strings setter fails");
            Assert.AreEqual(exemplar.floatVal, inRec["FloatTest"], "Floats setter fails");
            Assert.AreEqual(exemplar.checkboxVal, inRec["CheckboxTest"], "Checkboxes setter fails");
            Assert.AreEqual(exemplar.dateVal, inRec["DateTest"], "Dates setter fails");
            Assert.AreEqual(exemplar.timeStampVal, inRec["TimeStampTest"], "TimeStamps setter fails");
            Assert.AreEqual(exemplar.timeOfDayVal, inRec["TimeOfDayTest"], "TimeOfDays setter fails");
            //Assert.AreEqual(exemplar.durationVal, inRec["DurationTest"], "Durations setter fails");
            Assert.AreEqual(exemplar.currencyVal, inRec["CurrencyTest"], "Currency setter fails");
            Assert.AreEqual(exemplar.percentVal, inRec["PercentTest"], "Percent setter fails");
            Assert.AreEqual(exemplar.emailVal, inRec["EmailTest"], "Email setter fails");
            Assert.AreEqual(exemplar.phoneVal, inRec["PhoneTest"], "Phone setter fails");
            Assert.AreEqual(exemplar.urlVal, inRec["UrlTest"], "Url setter fails");

            testTable.Records.Add(inRec);
            testTable.AcceptChanges();

            Assert.AreEqual(exemplar.textVal, inRec["TextTest"], "Strings wrong post upload");
            Assert.AreEqual(exemplar.floatVal, inRec["FloatTest"], "Floats wrong post upload");
            Assert.AreEqual(exemplar.checkboxVal, inRec["CheckboxTest"], "Checkboxes wrong post upload");
            Assert.AreEqual(exemplar.dateVal, inRec["DateTest"], "Dates wrong post upload");
            Assert.AreEqual(exemplar.timeStampVal, inRec["TimeStampTest"], "TimeStamps wrong post upload");
            Assert.AreEqual(exemplar.timeOfDayVal, inRec["TimeOfDayTest"], "TimeOfDays wrong post upload");
            //Assert.AreEqual(exemplar.durationVal, inRec["DurationTest"], "Durations wrong post upload");
            Assert.AreEqual(exemplar.currencyVal, inRec["CurrencyTest"], "Currency wrong post upload");
            Assert.AreEqual(exemplar.percentVal, inRec["PercentTest"], "Percent wrong post upload");
            Assert.AreEqual(exemplar.emailVal, inRec["EmailTest"], "Email wrong post upload");
            Assert.AreEqual(exemplar.phoneVal, inRec["PhoneTest"], "Phone wrong post upload");
            Assert.AreEqual(exemplar.urlVal, inRec["UrlTest"], "Url wrong post upload");

            testTable.Records.Clear();
            testTable.Query();

            IQRecord outRec = testTable.Records[0];
            Assert.AreEqual(exemplar.textVal, outRec["TextTest"], "Strings don't roundtrip");
            Assert.AreEqual(exemplar.floatVal, outRec["FloatTest"], "Floats don't roundtrip");
            Assert.AreEqual(exemplar.checkboxVal, outRec["CheckboxTest"], "Checkboxes don't roundtrip");
            Assert.AreEqual(exemplar.dateVal, outRec["DateTest"], "Dates don't roundtrip");
            Assert.AreEqual(exemplar.timeStampVal, outRec["TimeStampTest"], "TimeStamps don't roundtrip");
            Assert.AreEqual(exemplar.timeOfDayVal, outRec["TimeOfDayTest"], "TimeOfDays don't roundtrip");
            //Assert.AreEqual(exemplar.durationVal, outRec["DurationTest"], "Durations don't roundtrip");
            Assert.AreEqual(exemplar.currencyVal, outRec["CurrencyTest"], "Currencies don't roundtrip");
            Assert.AreEqual(exemplar.percentVal, outRec["PercentTest"], "Percents don't roundtrip");
            Assert.AreEqual(exemplar.emailVal, outRec["EmailTest"], "Emails don't roundtrip");
            Assert.AreEqual(exemplar.phoneVal, outRec["PhoneTest"], "Phones don't roundtrip");
            Assert.AreEqual(exemplar.urlVal, outRec["UrlTest"], "Url don't roundtrip");

            exemplar.Setup2ndValues();
            outRec["TextTest"] = exemplar.textVal;
            outRec["FloatTest"] = exemplar.floatVal;
            outRec["CheckboxTest"] = exemplar.checkboxVal;
            outRec["DateTest"] = exemplar.dateVal;
            outRec["TimeStampTest"] = exemplar.timeStampVal;
            outRec["TimeOfDayTest"] = exemplar.timeOfDayVal;
            //outRec["DurationTest"] = exemplar.durationVal;
            outRec["CurrencyTest"] = exemplar.currencyVal;
            outRec["PercentTest"] = exemplar.percentVal;
            outRec["EmailTest"] = exemplar.emailVal;
            outRec["PhoneTest"] = exemplar.phoneVal;
            outRec["UrlTest"] = exemplar.urlVal;

            testTable.AcceptChanges();
            testTable.Query();

            IQRecord outRec2 = testTable.Records[0];
            Assert.AreEqual(exemplar.textVal, outRec2["TextTest"], "Strings don't update");
            Assert.AreEqual(exemplar.floatVal, outRec2["FloatTest"], "Floats don't update");
            Assert.AreEqual(exemplar.checkboxVal, outRec2["CheckboxTest"], "Checkboxes don't update");
            Assert.AreEqual(exemplar.dateVal, outRec2["DateTest"], "Dates don't update");
            Assert.AreEqual(exemplar.timeStampVal, outRec2["TimeStampTest"], "TimeStamps don't update");
            Assert.AreEqual(exemplar.timeOfDayVal, outRec2["TimeOfDayTest"], "TimeOfDays don't update");
            //Assert.AreEqual(exemplar.durationVal, outRec2["DurationTest"], "Durations don't update");
            Assert.AreEqual(exemplar.currencyVal, outRec2["CurrencyTest"], "Currencies don't update");
            Assert.AreEqual(exemplar.percentVal, outRec2["PercentTest"], "Percents don't update");
            Assert.AreEqual(exemplar.emailVal, outRec2["EmailTest"], "Emails don't update");
            Assert.AreEqual(exemplar.phoneVal, outRec2["PhoneTest"], "Phones don't update");
            Assert.AreEqual(exemplar.urlVal, outRec2["UrlTest"], "Url don't update");

        }
    }
}
