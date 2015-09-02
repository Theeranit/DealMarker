using KK.DealMaker.Business.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for UserBusinessTest and is intended
    ///to contain all UserBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserBusinessTest
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
        ///A test for UserBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void UserBusinessConstructorTest()
        {
            UserBusiness target = new UserBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateUser
        ///</summary>
        [TestMethod()]
        public void CreateUserTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_USER user = null; // TODO: Initialize to an appropriate value
            MA_USER expected = null; // TODO: Initialize to an appropriate value
            MA_USER actual;
            actual = target.CreateUser(sessioninfo, user);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DeleteUser
        ///</summary>
        [TestMethod()]
        public void DeleteUserTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid ID = new Guid(); // TODO: Initialize to an appropriate value
            target.DeleteUser(sessioninfo, ID);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetAll
        ///</summary>
        [TestMethod()]
        public void GetAllTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            List<MA_USER> expected = null; // TODO: Initialize to an appropriate value
            List<MA_USER> actual;
            actual = target.GetAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByFilter
        ///</summary>
        [TestMethod()]
        public void GetByFilterTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int startIndex = 0; // TODO: Initialize to an appropriate value
            int count = 0; // TODO: Initialize to an appropriate value
            string sorting = string.Empty; // TODO: Initialize to an appropriate value
            List<MA_USER> expected = null; // TODO: Initialize to an appropriate value
            List<MA_USER> actual;
            actual = target.GetByFilter(sessioninfo, name, startIndex, count, sorting);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByID
        ///</summary>
        [TestMethod()]
        public void GetByIDTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            Guid ID = new Guid(); // TODO: Initialize to an appropriate value
            MA_USER expected = null; // TODO: Initialize to an appropriate value
            MA_USER actual;
            actual = target.GetByID(sessioninfo, ID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetByUserCode
        ///</summary>
        [TestMethod()]
        public void GetByUserCodeTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            string usercode = string.Empty; // TODO: Initialize to an appropriate value
            MA_USER expected = null; // TODO: Initialize to an appropriate value
            MA_USER actual;
            actual = target.GetByUserCode(sessioninfo, usercode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LogOn
        ///</summary>
        [TestMethod()]
        public void LogOnTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            string username = string.Empty; // TODO: Initialize to an appropriate value
            string password = string.Empty; // TODO: Initialize to an appropriate value
            SessionInfo expected = null; // TODO: Initialize to an appropriate value
            SessionInfo actual;
            actual = target.LogOn(username, password);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdateUser
        ///</summary>
        [TestMethod()]
        public void UpdateUserTest()
        {
            UserBusiness target = new UserBusiness(); // TODO: Initialize to an appropriate value
            SessionInfo sessioninfo = null; // TODO: Initialize to an appropriate value
            MA_USER user = null; // TODO: Initialize to an appropriate value
            MA_USER expected = null; // TODO: Initialize to an appropriate value
            MA_USER actual;
            actual = target.UpdateUser(sessioninfo, user);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
