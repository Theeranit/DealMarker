using KK.DealMaker.Business.Deal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for StaticDataBusinessTest and is intended
    ///to contain all StaticDataBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StaticDataBusinessTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for StaticDataBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void StaticDataBusinessConstructorTest()
        {
            StaticDataBusiness target = new StaticDataBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreatePCCF
        ///</summary>
        [TestMethod()]
        public void CreatePCCFTest()
        {
            StaticDataBusiness target = new StaticDataBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PCCF pccf = null; // TODO: Initialize to an appropriate value
            MA_PCCF expected = null; // TODO: Initialize to an appropriate value
            MA_PCCF actual;
            actual = target.CreatePCCF(sessioninfo, pccf);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByBondMkt
        ///</summary>
        [TestMethod()]
        public void GetByBondMktTest()
        {
            StaticDataBusiness target = new StaticDataBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string strBondMkt = string.Empty; // TODO: Initialize to an appropriate value
            MA_PCCF expected = null; // TODO: Initialize to an appropriate value
            MA_PCCF actual;
            actual = target.GetByBondMkt(sessioninfo, strBondMkt);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByLabel
        ///</summary>
        [TestMethod()]
        public void GetByLabelTest()
        {
            StaticDataBusiness target = new StaticDataBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string strLabel = string.Empty; // TODO: Initialize to an appropriate value
            MA_PCCF expected = null; // TODO: Initialize to an appropriate value
            MA_PCCF actual;
            actual = target.GetByLabel(sessioninfo, strLabel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetFIPCCF
        ///</summary>
        [TestMethod()]
        public void GetFIPCCFTest()
        {
            StaticDataBusiness target = new StaticDataBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid guID = new Guid(); // TODO: Initialize to an appropriate value
            DateTime dteStart = new DateTime(); // TODO: Initialize to an appropriate value
            Decimal expected = new Decimal(); // TODO: Initialize to an appropriate value
            Decimal actual;
            actual = target.GetFIPCCF(sessioninfo, guID, dteStart);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPCCFByFilter
        ///</summary>
        [TestMethod()]
        public void GetPCCFByFilterTest()
        {
            StaticDataBusiness target = new StaticDataBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string label = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_PCCF> expected = null; // TODO: Initialize to an appropriate value
            List<MA_PCCF> actual;
            actual = target.GetPCCFByFilter(sessioninfo, label, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdatePCCF
        ///</summary>
        [TestMethod()]
        public void UpdatePCCFTest()
        {
            StaticDataBusiness target = new StaticDataBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PCCF pccf = null; // TODO: Initialize to an appropriate value
            target.UpdatePCCF(sessioninfo, pccf);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
