using KK.DealMaker.Business.External;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KK.DealMaker.Core.OpicsData;
using System.Collections.Generic;
using KK.DealMaker.Core.Helper;

namespace DealMaker.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for OpicsBusinessTest and is intended
    ///to contain all OpicsBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OpicsBusinessTest
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
        ///A test for OpicsBusiness Constructor
        ///</summary>
        [TestMethod()]
        public void OpicsBusinessConstructorTest()
        {
            OpicsBusiness target = new OpicsBusiness();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetOPICSCashflow
        ///</summary>
        [TestMethod()]
        public void GetOPICSCashflowTest()
        {
            OpicsBusiness target = new OpicsBusiness(); // TODO: Initialize to an appropriate value
            List<CASHFLOWModel> expected = null; // TODO: Initialize to an appropriate value
            List<CASHFLOWModel> actual;
            actual = target.GetOPICSCashflow();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetOPICSCustomerByName
        ///</summary>
        [TestMethod()]
        public void GetOPICSCustomerByNameTest()
        {
            OpicsBusiness target = new OpicsBusiness(); // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            List<CUSTModel> expected = null; // TODO: Initialize to an appropriate value
            List<CUSTModel> actual;
            actual = target.GetOPICSCustomerByName(name);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetOPICSDealExternal
        ///</summary>
        [TestMethod()]
        public void GetOPICSDealExternalTest()
        {
            OpicsBusiness target = new OpicsBusiness(); // TODO: Initialize to an appropriate value
            List<DEALModel> expected = null; // TODO: Initialize to an appropriate value
            List<DEALModel> actual;
            actual = target.GetOPICSDealExternal();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetOPICSInstrumentByLabel
        ///</summary>
        [TestMethod()]
        public void GetOPICSInstrumentByLabelTest()
        {
            OpicsBusiness target = new OpicsBusiness(); // TODO: Initialize to an appropriate value
            string label = string.Empty; // TODO: Initialize to an appropriate value
            List<SECMModel> expected = null; // TODO: Initialize to an appropriate value
            List<SECMModel> actual;
            actual = target.GetOPICSInstrumentByLabel(label);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Oracle
        ///</summary>
        [TestMethod()]
        public void OracleTest()
        {
            OpicsBusiness target = new OpicsBusiness(); // TODO: Initialize to an appropriate value
            OracleHelper actual;
            actual = target.Oracle;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
