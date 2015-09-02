using KK.DealMaker.Business.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for LimitProductBusinessTest and is intended
    ///to contain all LimitProductBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LimitProductBusinessTest
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
        ///A test for LimitProductBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void LimitProductBusinessConstructorTest()
        {
            LimitProductBusiness target = new LimitProductBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateLimitProduct
        ///</summary>
        [TestMethod()]
        public void CreateLimitProductTest()
        {
            LimitProductBusiness target = new LimitProductBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_LIMIT_PRODUCT limitproduct = null; // TODO: Initialize to an appropriate value
            MA_LIMIT_PRODUCT expected = null; // TODO: Initialize to an appropriate value
            MA_LIMIT_PRODUCT actual;
            actual = target.CreateLimitProduct(sessioninfo, limitproduct);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetLimitProductByFilter
        ///</summary>
        [TestMethod()]
        public void GetLimitProductByFilterTest()
        {
            LimitProductBusiness target = new LimitProductBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string strproduct = string.Empty; // TODO: Initialize to an appropriate value
            string strlimit = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_LIMIT_PRODUCT> expected = null; // TODO: Initialize to an appropriate value
            List<MA_LIMIT_PRODUCT> actual;
            actual = target.GetLimitProductByFilter(sessioninfo, strproduct, strlimit, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateLimitProduct
        ///</summary>
        [TestMethod()]
        public void UpdateLimitProductTest()
        {
            LimitProductBusiness target = new LimitProductBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_LIMIT_PRODUCT limitproduct = null; // TODO: Initialize to an appropriate value
            MA_LIMIT_PRODUCT expected = null; // TODO: Initialize to an appropriate value
            MA_LIMIT_PRODUCT actual;
            actual = target.UpdateLimitProduct(sessioninfo, limitproduct);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ValidateProfileFunction
        ///</summary>
        [TestMethod()]
        [DeploymentItem("KK.DealMaker.Business.dll")]
        public void ValidateProfileFunctionTest()
        {
            LimitProductBusiness_Accessor target = new LimitProductBusiness_Accessor(); // TODO: Initialize to an appropriate value
            MA_LIMIT_PRODUCT data = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ValidateProfileFunction(data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
