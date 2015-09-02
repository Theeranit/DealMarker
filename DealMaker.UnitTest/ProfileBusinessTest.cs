using KK.DealMaker.Business.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ProfileBusinessTest and is intended
    ///to contain all ProfileBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProfileBusinessTest
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
        ///A test for ProfileBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void ProfileBusinessConstructorTest()
        {
            ProfileBusiness target = new ProfileBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateUserProfile
        ///</summary>
        [TestMethod()]
        public void CreateUserProfileTest()
        {
            ProfileBusiness target = new ProfileBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_USER_PROFILE userprofile = null; // TODO: Initialize to an appropriate value
            MA_USER_PROFILE expected = null; // TODO: Initialize to an appropriate value
            MA_USER_PROFILE actual;
            actual = target.CreateUserProfile(sessioninfo, userprofile);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProfileByFilter
        ///</summary>
        [TestMethod()]
        public void GetProfileByFilterTest()
        {
            ProfileBusiness target = new ProfileBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_USER_PROFILE> expected = null; // TODO: Initialize to an appropriate value
            List<MA_USER_PROFILE> actual;
            actual = target.GetProfileByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProfileOptions
        ///</summary>
        [TestMethod()]
        public void GetProfileOptionsTest()
        {
            ProfileBusiness target = new ProfileBusiness(); // TODO: Initialize to an appropriate value
            List<MA_USER_PROFILE> expected = null; // TODO: Initialize to an appropriate value
            List<MA_USER_PROFILE> actual;
            actual = target.GetProfileOptions();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateUserProfile
        ///</summary>
        [TestMethod()]
        public void UpdateUserProfileTest()
        {
            ProfileBusiness target = new ProfileBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_USER_PROFILE userprofile = null; // TODO: Initialize to an appropriate value
            MA_USER_PROFILE expected = null; // TODO: Initialize to an appropriate value
            MA_USER_PROFILE actual;
            actual = target.UpdateUserProfile(sessioninfo, userprofile);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
