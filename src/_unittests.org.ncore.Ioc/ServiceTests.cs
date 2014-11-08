using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.Ioc;

namespace _unittests.org.ncore.Ioc
{
    /// <summary>
    /// Summary description for ServiceTests
    /// </summary>
    [TestClass]
    public class ServiceTests
    {
        public ServiceTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void New_from_name_works()
        {
            // REGISTER OUR TYPE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( "MyService", typeof( MockSampleClassC ) ) );

            dynamic myService = new Service( "MyService" );
            string greeting = myService.Greet( "Hello" );

            // I think this is beautiful, and the signatures are still available to you, just type:
            //  TaskList. and get intellisense!

            // TERSE: (I don't really like this syntax)
            //string greeting = ( (dynamic)Service.New( "MyService" ) ).Greet( "Hello" );

            Assert.AreEqual( "Hello, I am a MockSampleClassC", greeting );
        }

        [TestMethod]
        [ExpectedException( typeof( ApplicationException ), "The specified name does not refer to a Type object in the Registry." )]
        public void New_from_name_throws_not_in_registry()
        {
            // REGISTER OUR TYPE
            Kernel.Registry.Reset();

            dynamic myService = new Service( "MyService" );
        }

        [TestMethod]
        public void New_from_type_mapped_in_registry_works()
        {
            // REGISTER OUR TYPE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( SampleClassC ), typeof( MockSampleClassC ) ) );

            // HMM. Something is confusing here. Are we trying to fully use the kernel registry or not?
            dynamic myService = new Service( typeof( SampleClassC ) );
            string greeting = myService.Greet( "Hello" );

            Assert.AreEqual( "Hello, I am a MockSampleClassC", greeting );
        }

        [TestMethod]
        public void New_from_type_not_mapped_in_registry_works()
        {
            // ARRANGE
            Kernel.Registry.Reset();

            // ACT
            dynamic myService = new Service( typeof( SampleClassC ) );
            string greeting = myService.Greet( "Hello" );

            // ASSERT
            Assert.AreEqual( "Hello, I am a SampleClassC", greeting );
        }
    }

    public class MockSampleClassC
    {
        public static string Greet( string greeting )
        {
            return greeting + ", I am a MockSampleClassC";
        }
    }
}
