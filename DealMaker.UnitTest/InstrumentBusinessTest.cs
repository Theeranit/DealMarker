using KK.DealMaker.Business.Deal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;
using KK.DealMaker.Core.Constraint;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for InstrumentBusinessTest and is intended
    ///to contain all InstrumentBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InstrumentBusinessTest
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
        ///A test for InstrumentBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void InstrumentBusinessConstructorTest()
        {
            InstrumentBusiness target = new InstrumentBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT instrument = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT expected = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT actual;
            actual = target.Create(sessioninfo, instrument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByFilter
        ///</summary>
        [TestMethod()]
        public void GetByFilterTest()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string label = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> actual;
            actual = target.GetByFilter(sessioninfo, label, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByID
        ///</summary>
        [TestMethod()]
        public void GetByIDTest()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid guID = new Guid(); // TODO: Initialize to an appropriate value
            MA_INSTRUMENT expected = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT actual;
            actual = target.GetByID(sessioninfo, guID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByLabel
        ///</summary>
        [TestMethod()]
        public void GetByLabelTest()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string strLabel = string.Empty; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT expected = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT actual;
            actual = target.GetByLabel(sessioninfo, strLabel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByProduct
        ///</summary>
        [TestMethod()]
        public void GetByProductTest()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            ProductCode productcode = new ProductCode(); // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> actual;
            actual = target.GetByProduct(sessioninfo, productcode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByProduct
        ///</summary>
        [TestMethod()]
        public void GetByProductTest1()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid productID = new Guid(); // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> actual;
            actual = target.GetByProduct(sessioninfo, productID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetInstrumentAll
        ///</summary>
        [TestMethod()]
        public void GetInstrumentAllTest()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_INSTRUMENT> actual;
            actual = target.GetInstrumentAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            InstrumentBusiness target = new InstrumentBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT instrument = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT expected = null; // TODO: Initialize to an appropriate value
            MA_INSTRUMENT actual;
            actual = target.Update(sessioninfo, instrument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
