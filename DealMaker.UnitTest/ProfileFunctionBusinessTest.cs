using KK.DealMaker.Business.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ProfileFunctionBusinessTest and is intended
    ///to contain all ProfileFunctionBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProfileFunctionBusinessTest
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
        ///A test for ProfileFunctionBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void ProfileFunctionBusinessConstructorTest()
        {
            ProfileFunctionBusiness target = new ProfileFunctionBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateProfileFunction
        ///</summary>
        [TestMethod()]
        public void CreateProfileFunctionTest()
        {
            ProfileFunctionBusiness target = new ProfileFunctionBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PROFILE_FUNCTIONAL profilefunction = null; // TODO: Initialize to an appropriate value
            MA_PROFILE_FUNCTIONAL expected = null; // TODO: Initialize to an appropriate value
            MA_PROFILE_FUNCTIONAL actual;
            actual = target.CreateProfileFunction(sessioninfo, profilefunction);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProfileFunctionByFilter
        ///</summary>
        [TestMethod()]
        public void GetProfileFunctionByFilterTest()
        {
            ProfileFunctionBusiness target = new ProfileFunctionBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string strprofile = string.Empty; // TODO: Initialize to an appropriate value
            string strfunction = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_PROFILE_FUNCTIONAL> expected = null; // TODO: Initialize to an appropriate value
            List<MA_PROFILE_FUNCTIONAL> actual;
            actual = target.GetProfileFunctionByFilter(sessioninfo, strprofile, strfunction, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateProfileFunction
        ///</summary>
        [TestMethod()]
        public void UpdateProfileFunctionTest()
        {
            ProfileFunctionBusiness target = new ProfileFunctionBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_PROFILE_FUNCTIONAL profilefunction = null; // TODO: Initialize to an appropriate value
            MA_PROFILE_FUNCTIONAL expected = null; // TODO: Initialize to an appropriate value
            MA_PROFILE_FUNCTIONAL actual;
            actual = target.UpdateProfileFunction(sessioninfo, profilefunction);
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
            ProfileFunctionBusiness_Accessor target = new ProfileFunctionBusiness_Accessor(); // TODO: Initialize to an appropriate value
            MA_PROFILE_FUNCTIONAL data = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ValidateProfileFunction(data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
