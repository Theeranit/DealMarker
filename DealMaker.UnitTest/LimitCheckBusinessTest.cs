using KK.DealMaker.Business.Deal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for LimitCheckBusinessTest and is intended
    ///to contain all LimitCheckBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LimitCheckBusinessTest
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
        ///A test for LimitCheckBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void LimitCheckBusinessConstructorTest()
        {
            LimitCheckBusiness target = new LimitCheckBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetLimitByCriteria
        ///</summary>
        [TestMethod()]
        public void GetLimitByCriteriaTest()
        {
            LimitCheckBusiness target = new LimitCheckBusiness(); // TODO: Initialize to an appropriate value
            Guid CTPY_ID = new Guid(); // TODO: Initialize to an appropriate value
            Guid ProductID = new Guid(); // TODO: Initialize to an appropriate value
            List<LimitCheckModel> expected = null; // TODO: Initialize to an appropriate value
            List<LimitCheckModel> actual;
            actual = target.GetLimitByCriteria(CTPY_ID, ProductID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
