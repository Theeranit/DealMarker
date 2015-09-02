using KK.DealMaker.Business.Deal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for CounterpartyBusinessTest and is intended
    ///to contain all CounterpartyBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CounterpartyBusinessTest
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
        ///A test for CounterpartyBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void CounterpartyBusinessConstructorTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_COUTERPARTY counterparty = null; // TODO: Initialize to an appropriate value
            MA_COUTERPARTY expected = null; // TODO: Initialize to an appropriate value
            MA_COUTERPARTY actual;
            actual = target.Create(sessioninfo, counterparty);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateCounterpartyLimit
        ///</summary>
        [TestMethod()]
        public void CreateCounterpartyLimitTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_CTPY_LIMIT counterpartyLimit = null; // TODO: Initialize to an appropriate value
            MA_CTPY_LIMIT expected = null; // TODO: Initialize to an appropriate value
            MA_CTPY_LIMIT actual;
            actual = target.CreateCounterpartyLimit(sessioninfo, counterpartyLimit);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DeleteCounterpartyLimitByID
        ///</summary>
        [TestMethod()]
        public void DeleteCounterpartyLimitByIDTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid ID = new Guid(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.DeleteCounterpartyLimitByID(sessioninfo, ID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByFilter
        ///</summary>
        [TestMethod()]
        public void GetByFilterTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_COUTERPARTY> expected = null; // TODO: Initialize to an appropriate value
            List<MA_COUTERPARTY> actual;
            actual = target.GetByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCounterpartyAll
        ///</summary>
        [TestMethod()]
        public void GetCounterpartyAllTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            List<MA_COUTERPARTY> expected = null; // TODO: Initialize to an appropriate value
            List<MA_COUTERPARTY> actual;
            actual = target.GetCounterpartyAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCounterpartyLimitByCtpyID
        ///</summary>
        [TestMethod()]
        public void GetCounterpartyLimitByCtpyIDTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid ID = new Guid(); // TODO: Initialize to an appropriate value
            List<MA_CTPY_LIMIT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_CTPY_LIMIT> actual;
            actual = target.GetCounterpartyLimitByCtpyID(sessioninfo, ID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_COUTERPARTY counterparty = null; // TODO: Initialize to an appropriate value
            MA_COUTERPARTY expected = null; // TODO: Initialize to an appropriate value
            MA_COUTERPARTY actual;
            actual = target.Update(sessioninfo, counterparty);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateCounterpartyLimit
        ///</summary>
        [TestMethod()]
        public void UpdateCounterpartyLimitTest()
        {
            CounterpartyBusiness target = new CounterpartyBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_CTPY_LIMIT counterpartyLimit = null; // TODO: Initialize to an appropriate value
            MA_CTPY_LIMIT expected = null; // TODO: Initialize to an appropriate value
            MA_CTPY_LIMIT actual;
            actual = target.UpdateCounterpartyLimit(sessioninfo, counterpartyLimit);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
