using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OireachtasAPI;

namespace TestOireachtasAPI
{
    [TestClass]
    public class LoadDatasetTest
    {
        dynamic expected;
        [TestInitialize]
        public void SetUp()
        {
            using (StreamReader r = new StreamReader(OireachtasAPI.Program.MEMBERS_DATASET))
            {
                string json = r.ReadToEnd();
                expected = JsonConvert.DeserializeObject(json);
            }
        }
        [TestMethod]
        public void TestLoadFromFile()
        {
            var loaded = OireachtasAPI.Program.load<Members>(OireachtasAPI.Program.MEMBERS_DATASET);
            Assert.AreEqual(loaded.results.Length, expected["results"].Count);

        }

        [TestMethod]
        public void TestLoadFromUrl()
        {
            var loaded = OireachtasAPI.Program.load<Members>("https://api.oireachtas.ie/v1/members?limit=50");
            Assert.AreEqual(loaded.results.Length, expected["results"].Count);

        }
    }
    [TestClass]
    public class FilterBillsSponsoredByTest
    {
        [TestMethod]
        public void TestSponsor()
        {
            IList<Bill> results = OireachtasAPI.Program.filterBillsSponsoredBy("IvanaBacik");
            Assert.IsTrue(results.Count>=2);
        }
    }

    [TestClass]
    public class FilterBillsByLastUpdatedTest
    {
        [TestMethod]
        public void Testlastupdated()
        {
            List<string> expected = new List<string>(){
                "77", "101", "58", "141", "55", "133", "132", "131",
                "111", "135", "134", "91", "129", "103", "138", "106", "139"
            };

            List<string> received = new List<string>();

            DateTime since = new DateTime(2018, 12, 1);
            DateTime until = new DateTime(2019, 1, 1);

            foreach (var bill in OireachtasAPI.Program.filterBillsByLastUpdated(since, until))
            {
                received.Add(bill.billNo);
            }

            CollectionAssert.AreEquivalent(expected, received);
        }
    }
}
