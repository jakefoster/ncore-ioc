using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using org.ncore.Ioc;

namespace _unittests.org.ncore.Ioc
{
    /// <summary>
    /// Summary description for LocatorTests
    /// </summary>
    [TestClass]
    public class LocatorTests
    {
        public LocatorTests()
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

        // TODO: Maybe move this test to LocatorConfigurationTests?  -JF 
        [TestMethod]
        public void Registry_populates_from_config()
        {
            // ARRANGE
            Locator.Reset();
            // NOTE: The locator registry is populated in app.config.  -JF

            // ACT
            LocatorType type1 = Locator.Registry["_unittests.org.ncore.Ioc.ISampleInterfaceA"];
            LocatorType type2 = Locator.Registry["ArbitraryName"];

            // ASSERT
            //Assert.AreEqual( 2, Locator.Registry.Count() );
            
            Assert.AreEqual( "_unittests.org.ncore.Ioc", type1.Assembly );
            Assert.AreEqual( "_unittests.org.ncore.Ioc.SampleClassA", type1.TypeName );
            Assert.AreEqual( false, type1.AllowSave );
            Assert.IsNull( type1.Instance );

            Assert.AreEqual( "_unittests.org.ncore.Ioc", type2.Assembly );
            Assert.AreEqual( "_unittests.org.ncore.Ioc.SampleClassB", type2.TypeName );
            Assert.AreEqual( true, type2.AllowSave );
            Assert.IsNull( type2.Instance );
        }

        // TODO: Maybe move this test to LocatorConfigurationTests?  -JF 
        [TestMethod]
        public void Add_expand_wildcard()
        {
            // ARRANGE
            // NOTE: The locator registry is populated in app.config so let's wipe it out.  -JF
            Locator.Clear();
            
            // ACT
            Locator.Add( new LocatorType( "S_*", "_unittests.org.ncore.Ioc", "_unittests.org.ncore.Ioc.SampleApp.RealImp" ) );

            LocatorType widgetType = Locator.Registry[ "S_Widget" ];

            // ASSERT
            Assert.AreEqual( "_unittests.org.ncore.Ioc", widgetType.Assembly );
            Assert.AreEqual( "_unittests.org.ncore.Ioc.SampleApp.RealImp.Widget", widgetType.TypeName );
            Assert.AreEqual( false, widgetType.AllowSave );
            Assert.IsNull( widgetType.Instance );
        }

        // TODO: Get rid of this test. Just spitballing. -JF
        [TestMethod]
        public void Locator_initialize()
        {
            // ARRANGE
            // NOTE: The locator registry is populated in app.config so let's wipe it out.  -JF
            Locator.Clear();

            /*
            Action<LocatorRegistry> initializer;

            string foo = "bar";

            initializer = registry =>
            {
                string moo = foo;
                Debug.WriteLine( this.GetType().Name );
                Debug.WriteLine( moo + " = " + registry.Count.ToString() );
            };

            Locator.Initialize( initializer );
            */

            string foo = "bar";
            Locator.Initialize( 
                registry =>
                {
                    string moo = foo;
                    Debug.WriteLine( this.GetType().Name );
                    Debug.WriteLine( moo + " = " + registry.Count.ToString() );
                }
            );


            // ACT

            // ASSERT
        }
    }
}
