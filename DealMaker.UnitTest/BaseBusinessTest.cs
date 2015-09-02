using KK.DealMaker.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.SystemFramework;
using System.Data;
using System.Collections.Generic;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for BaseBusinessTest and is intended
    ///to contain all BaseBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BaseBusinessTest
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
        ///A test for BaseBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void BaseBusinessConstructorTest()
        {
            BaseBusiness target = new BaseBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateException
        ///</summary>
        [TestMethod()]
        public void CreateExceptionTest()
        {
            BaseBusiness target = new BaseBusiness(); // TODO: Initialize to an appropriate value
            Exception ex = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            BusinessWorkflowsException expected = null; // TODO: Initialize to an appropriate value
            BusinessWorkflowsException actual;
            actual = target.CreateException(ex, message);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCurrentDateTime
        ///</summary>
        [TestMethod()]
        public void GetCurrentDateTimeTest()
        {
            BaseBusiness target = new BaseBusiness(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetCurrentDateTime();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for MapDataReaderToList
        ///</summary>
        public void MapDataReaderToListTestHelper<T>()
        {
            BaseBusiness_Accessor target = new BaseBusiness_Accessor(); // TODO: Initialize to an appropriate value
            IDataReader dr = null; // TODO: Initialize to an appropriate value
            List<T> expected = null; // TODO: Initialize to an appropriate value
            List<T> actual;
            actual = target.MapDataReaderToList<T>(dr);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        [DeploymentItem("KK.DealMaker.Business.dll")]
        public void MapDataReaderToListTest()
        {
            MapDataReaderToListTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for MapDataToBusinessEntityCollection
        ///</summary>
        public void MapDataToBusinessEntityCollectionTestHelper<T>()
            where T : new()
        {
            BaseBusiness_Accessor target = new BaseBusiness_Accessor(); // TODO: Initialize to an appropriate value
            IDataReader dr = null; // TODO: Initialize to an appropriate value
            List<T> expected = null; // TODO: Initialize to an appropriate value
            List<T> actual;
            actual = target.MapDataToBusinessEntityCollection<T>(dr);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        [DeploymentItem("KK.DealMaker.Business.dll")]
        public void MapDataToBusinessEntityCollectionTest()
        {
            MapDataToBusinessEntityCollectionTestHelper<GenericParameterHelper>();
        }
    }
}
