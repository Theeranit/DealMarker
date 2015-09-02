using KK.DealMaker.Business.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for FunctionBusinessTest and is intended
    ///to contain all FunctionBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FunctionBusinessTest
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
        ///A test for FunctionBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void FunctionBusinessConstructorTest()
        {
            FunctionBusiness target = new FunctionBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateFunction
        ///</summary>
        [TestMethod()]
        public void CreateFunctionTest()
        {
            FunctionBusiness target = new FunctionBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_FUNCTIONAL function = null; // TODO: Initialize to an appropriate value
            MA_FUNCTIONAL expected = null; // TODO: Initialize to an appropriate value
            MA_FUNCTIONAL actual;
            actual = target.CreateFunction(sessioninfo, function);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetFunctionByFilter
        ///</summary>
        [TestMethod()]
        public void GetFunctionByFilterTest()
        {
            FunctionBusiness target = new FunctionBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_FUNCTIONAL> expected = null; // TODO: Initialize to an appropriate value
            List<MA_FUNCTIONAL> actual;
            actual = target.GetFunctionByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetFunctionOptions
        ///</summary>
        [TestMethod()]
        public void GetFunctionOptionsTest()
        {
            FunctionBusiness target = new FunctionBusiness(); // TODO: Initialize to an appropriate value
            List<MA_FUNCTIONAL> expected = null; // TODO: Initialize to an appropriate value
            List<MA_FUNCTIONAL> actual;
            actual = target.GetFunctionOptions();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateFunction
        ///</summary>
        [TestMethod()]
        public void UpdateFunctionTest()
        {
            FunctionBusiness target = new FunctionBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_FUNCTIONAL function = null; // TODO: Initialize to an appropriate value
            MA_FUNCTIONAL expected = null; // TODO: Initialize to an appropriate value
            MA_FUNCTIONAL actual;
            actual = target.UpdateFunction(sessioninfo, function);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
