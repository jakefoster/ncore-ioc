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
    /// Summary description for NewTests
    /// </summary>
    [TestClass]
    public class NewTests
    {
        public NewTests()
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
        public void Instance_dynamic_works_creates_new()
        {
            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( "MyClass", typeof( MyClassA ) ) { AllowSave = false } );

            // ACT
            dynamic myClass = New.Instance( "MyClass" );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.IsNull( Locator.Registry[ "MyClass" ].Instance );
        }

        [TestMethod]
        public void Instance_dynamic_works_creates_new_with_anonymous_injector_registry()
        {
            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( "MyClass", typeof( MyClassA ) ) { AllowSave = false } );

            // ACT
            dynamic myClass = New.Instance( "MyClass", new { FieldA = "Inject this value", ParamB = "Inject this too" } );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.IsNull( Locator.Registry[ "MyClass" ].Instance );
        }

        
        [TestMethod]
        public void Instance_dynamic_works_creates_new_with_typed_injector_registry()
        {
            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( "MyClass", typeof( MyClassA ) ) { AllowSave = false } );

            // ACT
            dynamic myClass = New.Instance( "MyClass", 
                new InjectorRegistry{
                    { "FieldA", "Inject this value" },
                    { "PropertyB", "Inject this too" }
                } );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.IsNull( Locator.Registry[ "MyClass" ].Instance );
        }



        [TestMethod]
        public void Instance_dynamic_works_creates_new_with_constructor_params()
        {
            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( "MyClass", typeof( MyClassA ) ) { AllowSave = false } );

            // ACT
            dynamic myClass = New.Instance( "MyClass", constructorParams: new object[] { "My ParamA value", "My ParamB value" } );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.IsNull( Locator.Registry[ "MyClass" ].Instance );
        }

        [TestMethod]
        public void Instance_typed_from_type_works_creates_new()
        {
            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( typeof( IMyClass ), typeof( MyClassA ) ) { AllowSave = false } );

            // ACT
            IMyClass myClass = New.Instance<IMyClass>();
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.IsNull( Locator.Registry[ typeof( IMyClass ).FullName ].Instance );
        }

        [TestMethod]
        public void Instance_typed_from_string_works_creates_new()
        {
            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( "MyClass", typeof( MyClassA ) ) { AllowSave = false } );

            // ACT
            IMyClass myClass = New.Instance<IMyClass>("MyClass");
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.IsNull( Locator.Registry[ "MyClass" ].Instance );
        }

        public interface IMyClass
        {
            string Greet( string name );
        }

        public class MyClassA : IMyClass
        {
            public string Greeter = "MyClassA";
            public string FieldA;
            public string PropertyB {get;set;}

            public string Greet( string name )
            {
                return "Hello " + name + " from " + Greeter;
            }

            public MyClassA()
            { }

            public MyClassA( string paramA, string paramB )
            {
                FieldA = paramA;
                PropertyB = paramB;
            }
        }
    }
}
