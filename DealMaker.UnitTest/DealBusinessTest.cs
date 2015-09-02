using KK.DealMaker.Business.Deal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for DealBusinessTest and is intended
    ///to contain all DealBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DealBusinessTest
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
        ///A test for DealBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void DealBusinessConstructorTest()
        {
            DealBusiness target = new DealBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CancelDeal
        ///</summary>
        [TestMethod()]
        public void CancelDealTest()
        {
            DealBusiness target = new DealBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid guTrnID = new Guid(); // TODO: Initialize to an appropriate value
            target.CancelDeal(sessioninfo, guTrnID);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetAll
        ///</summary>
        [TestMethod()]
        public void GetAllTest()
        {
            DealBusiness target = new DealBusiness(); // TODO: Initialize to an appropriate value
            List<DA_TRN> expected = null; // TODO: Initialize to an appropriate value
            List<DA_TRN> actual;
            actual = target.GetAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDealByProcessDate
        ///</summary>
        [TestMethod()]
        public void GetDealByProcessDateTest()
        {
            DealBusiness target = new DealBusiness(); // TODO: Initialize to an appropriate value
            DateTime processdate = new DateTime(); // TODO: Initialize to an appropriate value
            List<DA_TRN> expected = null; // TODO: Initialize to an appropriate value
            List<DA_TRN> actual;
            actual = target.GetDealByProcessDate(processdate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetDealInquiryByFilter
        ///</summary>
        [TestMethod()]
        public void GetDealInquiryByFilterTest()
        {
            DealBusiness target = new DealBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string strDMKNo = string.Empty; // TODO: Initialize to an appropriate value
            string strOPICNo = string.Empty; // TODO: Initialize to an appropriate value
            string strProduct = string.Empty; // TODO: Initialize to an appropriate value
            string strCtpy = string.Empty; // TODO: Initialize to an appropriate value
            string strPortfolio = string.Empty; // TODO: Initialize to an appropriate value
            string strTradeDate = string.Empty; // TODO: Initialize to an appropriate value
            string strEffDate = string.Empty; // TODO: Initialize to an appropriate value
            string strMatDate = string.Empty; // TODO: Initialize to an appropriate value
            string strInstrument = string.Empty; // TODO: Initialize to an appropriate value
            string strUser = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<DA_TRN> expected = null; // TODO: Initialize to an appropriate value
            List<DA_TRN> actual;
            actual = target.GetDealInquiryByFilter(sessioninfo, strDMKNo, strOPICNo, strProduct, strCtpy, strPortfolio, strTradeDate, strEffDate, strMatDate, strInstrument, strUser, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetNewDealNo
        ///</summary>
        [TestMethod()]
        public void GetNewDealNoTest()
        {
            DealBusiness target = new DealBusiness(); // TODO: Initialize to an appropriate value
            DateTime dteEngine = new DateTime(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetNewDealNo(dteEngine);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SubmitFIDeal
        ///</summary>
        [TestMethod()]
        public void SubmitFIDealTest()
        {
            DealBusiness target = new DealBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string strCtpy = string.Empty; // TODO: Initialize to an appropriate value
            string strTradeDate = string.Empty; // TODO: Initialize to an appropriate value
            string strSettlementDate = string.Empty; // TODO: Initialize to an appropriate value
            string strBuySell = string.Empty; // TODO: Initialize to an appropriate value
            string strInstrument = string.Empty; // TODO: Initialize to an appropriate value
            string strPortfolio = string.Empty; // TODO: Initialize to an appropriate value
            string strNotional = string.Empty; // TODO: Initialize to an appropriate value
            string strCCY = string.Empty; // TODO: Initialize to an appropriate value
            string strLimitApp = string.Empty; // TODO: Initialize to an appropriate value
            Decimal decOverAmount = new Decimal(); // TODO: Initialize to an appropriate value
            string strOverComment = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.SubmitFIDeal(sessioninfo, strCtpy, strTradeDate, strSettlementDate, strBuySell, strInstrument, strPortfolio, strNotional, strCCY, strLimitApp, decOverAmount, strOverComment);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
