using KK.DealMaker.Business.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for LookupBusinessTest and is intended
    ///to contain all LookupBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LookupBusinessTest
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
        ///A test for LookupBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void LookupBusinessConstructorTest()
        {
            LookupBusiness target = new LookupBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateFreqType
        ///</summary>
        [TestMethod()]
        public void CreateFreqTypeTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_FREQ_TYPE freqtype = null; // TODO: Initialize to an appropriate value
            MA_FREQ_TYPE expected = null; // TODO: Initialize to an appropriate value
            MA_FREQ_TYPE actual;
            actual = target.CreateFreqType(sessioninfo, freqtype);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateLimit
        ///</summary>
        [TestMethod()]
        public void CreateLimitTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_LIMIT limit = null; // TODO: Initialize to an appropriate value
            MA_LIMIT expected = null; // TODO: Initialize to an appropriate value
            MA_LIMIT actual;
            actual = target.CreateLimit(sessioninfo, limit);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateProduct
        ///</summary>
        [TestMethod()]
        public void CreateProductTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PRODUCT product = null; // TODO: Initialize to an appropriate value
            MA_PRODUCT expected = null; // TODO: Initialize to an appropriate value
            MA_PRODUCT actual;
            actual = target.CreateProduct(sessioninfo, product);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateProfolio
        ///</summary>
        [TestMethod()]
        public void CreateProfolioTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PORTFOLIO profolio = null; // TODO: Initialize to an appropriate value
            MA_PORTFOLIO expected = null; // TODO: Initialize to an appropriate value
            MA_PORTFOLIO actual;
            actual = target.CreateProfolio(sessioninfo, profolio);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateStatus
        ///</summary>
        [TestMethod()]
        public void CreateStatusTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_STATUS status = null; // TODO: Initialize to an appropriate value
            MA_STATUS expected = null; // TODO: Initialize to an appropriate value
            MA_STATUS actual;
            actual = target.CreateStatus(sessioninfo, status);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetFreqTypeAll
        ///</summary>
        [TestMethod()]
        public void GetFreqTypeAllTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            List<MA_FREQ_TYPE> expected = null; // TODO: Initialize to an appropriate value
            List<MA_FREQ_TYPE> actual;
            actual = target.GetFreqTypeAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetFreqTypeByFilter
        ///</summary>
        [TestMethod()]
        public void GetFreqTypeByFilterTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_FREQ_TYPE> expected = null; // TODO: Initialize to an appropriate value
            List<MA_FREQ_TYPE> actual;
            actual = target.GetFreqTypeByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetLimitAll
        ///</summary>
        [TestMethod()]
        public void GetLimitAllTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            List<MA_LIMIT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_LIMIT> actual;
            actual = target.GetLimitAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetLimitByFilter
        ///</summary>
        [TestMethod()]
        public void GetLimitByFilterTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_LIMIT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_LIMIT> actual;
            actual = target.GetLimitByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPortfolioAll
        ///</summary>
        [TestMethod()]
        public void GetPortfolioAllTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            List<MA_PORTFOLIO> expected = null; // TODO: Initialize to an appropriate value
            List<MA_PORTFOLIO> actual;
            actual = target.GetPortfolioAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPortfolioByFilter
        ///</summary>
        [TestMethod()]
        public void GetPortfolioByFilterTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_PORTFOLIO> expected = null; // TODO: Initialize to an appropriate value
            List<MA_PORTFOLIO> actual;
            actual = target.GetPortfolioByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProductAll
        ///</summary>
        [TestMethod()]
        public void GetProductAllTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            List<MA_PRODUCT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_PRODUCT> actual;
            actual = target.GetProductAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProductByFilter
        ///</summary>
        [TestMethod()]
        public void GetProductByFilterTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_PRODUCT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_PRODUCT> actual;
            actual = target.GetProductByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetStatusAll
        ///</summary>
        [TestMethod()]
        public void GetStatusAllTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            List<MA_STATUS> expected = null; // TODO: Initialize to an appropriate value
            List<MA_STATUS> actual;
            actual = target.GetStatusAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetStatusByFilter
        ///</summary>
        [TestMethod()]
        public void GetStatusByFilterTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_STATUS> expected = null; // TODO: Initialize to an appropriate value
            List<MA_STATUS> actual;
            actual = target.GetStatusByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateFreqType
        ///</summary>
        [TestMethod()]
        public void UpdateFreqTypeTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_FREQ_TYPE freqtype = null; // TODO: Initialize to an appropriate value
            MA_FREQ_TYPE expected = null; // TODO: Initialize to an appropriate value
            MA_FREQ_TYPE actual;
            actual = target.UpdateFreqType(sessioninfo, freqtype);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateLimit
        ///</summary>
        [TestMethod()]
        public void UpdateLimitTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_LIMIT limit = null; // TODO: Initialize to an appropriate value
            MA_LIMIT expected = null; // TODO: Initialize to an appropriate value
            MA_LIMIT actual;
            actual = target.UpdateLimit(sessioninfo, limit);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdatePorfolio
        ///</summary>
        [TestMethod()]
        public void UpdatePorfolioTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PORTFOLIO profolio = null; // TODO: Initialize to an appropriate value
            MA_PORTFOLIO expected = null; // TODO: Initialize to an appropriate value
            MA_PORTFOLIO actual;
            actual = target.UpdatePorfolio(sessioninfo, profolio);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateProduct
        ///</summary>
        [TestMethod()]
        public void UpdateProductTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PRODUCT product = null; // TODO: Initialize to an appropriate value
            MA_PRODUCT expected = null; // TODO: Initialize to an appropriate value
            MA_PRODUCT actual;
            actual = target.UpdateProduct(sessioninfo, product);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateStatus
        ///</summary>
        [TestMethod()]
        public void UpdateStatusTest()
        {
            LookupBusiness target = new LookupBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_STATUS status = null; // TODO: Initialize to an appropriate value
            MA_STATUS expected = null; // TODO: Initialize to an appropriate value
            MA_STATUS actual;
            actual = target.UpdateStatus(sessioninfo, status);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
