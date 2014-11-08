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
    /// Summary description for KernelTests
    /// </summary>
    [TestClass]
    public class KernelTests
    {
        public static int APPCONFIG_REG_COUNT = 2;

        public KernelTests()
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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Kernel.Registry.Reset();
        }
        
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateObject_works()
        {
            ISampleInterfaceA sampleA = Kernel.CreateObject<ISampleInterfaceA>();

            string whoami = sampleA.WhoAmI();

            Assert.AreEqual( "I am a SampleClassA", whoami );
            Assert.AreEqual( APPCONFIG_REG_COUNT, Kernel.Registry.Count );
        }

        [TestMethod]
        public void GetOrCreateObject_works()
        {
            ISampleInterfaceA sampleA = Kernel.GetOrCreateObject<ISampleInterfaceA>();

            string whoami = sampleA.WhoAmI();

            Assert.AreEqual( "I am a SampleClassA", whoami );
            Assert.AreEqual( APPCONFIG_REG_COUNT, Kernel.Registry.Count );
        }

        [TestMethod]
        [ExpectedException( typeof( ApplicationException ), "The specified entry in the KernalRegistry does not have a saved instance." )]
        public void GetObject_throws_when_mising()
        {
            ISampleInterfaceA sampleA = Kernel.GetObject<ISampleInterfaceA>();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "The 'saveInRegistry' parameter was true but the underlying RegistryEntry for this type does not allow saving an instance to the registry.")]
        public void GetObject_throws_on_save_not_allowed()
        {
            ISampleInterfaceA saved = Kernel.GetOrCreateObject<ISampleInterfaceA>(true);
        }

        [TestMethod]
        public void GetObject_after_Create_works()
        {
            ISampleInterfaceA saved = Kernel.GetOrCreateObject<ISampleInterfaceA>( "ArbitraryName", true );

            ISampleInterfaceA sampleB = Kernel.GetObject<ISampleInterfaceA>( "ArbitraryName" );

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );
            Assert.AreSame( saved, sampleB );
            Assert.AreEqual( APPCONFIG_REG_COUNT, Kernel.Registry.Count );
            Assert.AreSame( sampleB, Kernel.Registry[ "ArbitraryName" ].Instance );
        }

        [TestMethod]
        [ExpectedException( typeof( ApplicationException ), "The 'saveInRegistry' parameter was true but there is already an instance saved in this RegistryEntry." )]
        public void GetObject_throws_on_already_have_saved_instance()
        {
            ISampleInterfaceA saved = Kernel.GetOrCreateObject<ISampleInterfaceA>( "ArbitraryName", true );

            ISampleInterfaceA sampleB = Kernel.CreateObject<ISampleInterfaceA>( "ArbitraryName", true );
        }

        [TestMethod]
        public void CreateObject_string_name_works()
        {
            ISampleInterfaceA sampleB = Kernel.CreateObject<ISampleInterfaceA>("ArbitraryName");

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );

            Assert.AreEqual( APPCONFIG_REG_COUNT, Kernel.Registry.Count );
        }

        [TestMethod]
        public void GetOrCreateObject_string_name_works()
        {
            ISampleInterfaceA sampleB = Kernel.GetOrCreateObject<ISampleInterfaceA>( "ArbitraryName" );

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );

            Assert.AreEqual( APPCONFIG_REG_COUNT, Kernel.Registry.Count );
        }

        [TestMethod]
        public void AddRegistryEntry_string_name_then_CreateObject_works()
        {
            Kernel.Registry.Add( new KernelType("AnotherArbitraryName", typeof(SampleClassB) ));

            ISampleInterfaceA sampleB = Kernel.GetOrCreateObject<ISampleInterfaceA>( "AnotherArbitraryName" );

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );

            Assert.AreEqual( APPCONFIG_REG_COUNT + 1, Kernel.Registry.Count );
        }

        [TestMethod]
        public void AddRegistryEntry_type_name_then_CreateObject_works()
        {
            Kernel.Registry.Add( new KernelType( typeof( SampleClassB ), typeof( SampleClassB ) ) );

            SampleClassB sampleB = Kernel.GetOrCreateObject<SampleClassB>();

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );

            Assert.AreEqual( APPCONFIG_REG_COUNT + 1, Kernel.Registry.Count );
        }

        [TestMethod]
        public void AddRegistryEntry_instance_then_GetObject_works()
        {
            SampleClassB instance = new SampleClassB();

            Kernel.Registry.Add( new KernelType( instance ) );

            SampleClassB sampleB = Kernel.GetObject<SampleClassB>();

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );
            Assert.AreSame( instance, sampleB );
            Assert.AreEqual( APPCONFIG_REG_COUNT + 1, Kernel.Registry.Count );
        }

        [TestMethod]
        public void AddRegistryEntry_instance_with_name_then_GetObject_works()
        {
            SampleClassB instance = new SampleClassB();

            Kernel.Registry.Add( new KernelType( "AnotherArbitraryName", instance ) );

            SampleClassB sampleB = Kernel.GetObject<SampleClassB>( "AnotherArbitraryName" );

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );
            Assert.AreSame( instance, sampleB );
            Assert.AreEqual( APPCONFIG_REG_COUNT + 1, Kernel.Registry.Count );
        }

        [TestMethod]
        public void AddRegistryEntry_instance_with_type_name_then_GetObject_works()
        {
            SampleClassB instance = new SampleClassB();

            Kernel.Registry.Add( new KernelType( typeof(SampleClassB), instance ) );

            SampleClassB sampleB = Kernel.GetObject<SampleClassB>();

            string whoami = sampleB.WhoAmI();

            Assert.AreEqual( "I am a SampleClassB", whoami );
            Assert.AreSame( instance, sampleB );
            Assert.AreEqual( APPCONFIG_REG_COUNT + 1, Kernel.Registry.Count );
        }
    }
}
