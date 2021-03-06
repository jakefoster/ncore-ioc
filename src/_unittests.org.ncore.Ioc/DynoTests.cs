﻿using System;
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
    public class DynoTests
    {
        public DynoTests()
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
        public void Static_New_from_name_works()
        {
            // REGISTER OUR TYPE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( "MyService", typeof( MockSampleClassC ) ) );

            string greeting = Dyno.New( "MyService" ).Greet( "Hello" );

            // I think this is beautiful, and the signatures are still available to you, just type:
            //  TaskList. and get intellisense!

            Assert.AreEqual( "Hello, I am a MockSampleClassC", greeting );
        }

        [TestMethod]
        public void Static_New_from_type_mapped_in_registry_works()
        {
            // REGISTER OUR TYPE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( typeof( SampleClassC ), typeof( MockSampleClassC ) ) );

            string greeting = Dyno.New<SampleClassC>().Greet( "Hello" );

            Assert.AreEqual( "Hello, I am a MockSampleClassC", greeting );
        }

        [TestMethod]
        public void New_works()
        {
            // REGISTER OUR TYPE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( "MyService", typeof( MockSampleClassC ) ) );

            dynamic myService = new Dyno( "MyService" );
            string greeting = myService.Greet( "Hello" );

            // I think this is beautiful, and the signatures are still available to you, just type:
            //  TaskList. and get intellisense!

            // TERSE: (I don't really like this syntax)
            //string greeting = ( (dynamic)Dyno.New( "MyService" ) ).Greet( "Hello" );

            Assert.AreEqual( "Hello, I am a MockSampleClassC", greeting );
        }

        [TestMethod]
        [ExpectedException( typeof( ApplicationException ), "The specified name does not refer to a Type object in the Registry." )]
        public void New_from_name_throws_not_in_registry()
        {
            // REGISTER OUR TYPE
            Locator.Registry.Clear();

            dynamic myService = new Dyno( "MyService" );
        }

        [TestMethod]
        public void New_from_type_mapped_in_registry_works()
        {
            // REGISTER OUR TYPE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( typeof( SampleClassC ), typeof( MockSampleClassC ) ) );

            // HMM. Something is confusing here. Are we trying to fully use the kernel registry or not?
            dynamic myService = new Dyno( typeof( SampleClassC ) );
            string greeting = myService.Greet( "Hello" );

            Assert.AreEqual( "Hello, I am a MockSampleClassC", greeting );
        }

        [TestMethod]
        public void New_from_type_not_mapped_in_registry_works()
        {
            // ARRANGE
            Locator.Registry.Clear();

            // ACT
            dynamic myService = new Dyno( typeof( SampleClassC ) );
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
