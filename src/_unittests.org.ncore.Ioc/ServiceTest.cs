using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using org.ncore.Ioc;

namespace _unittests.org.ncore.Ioc
{
    /// <summary>
    /// Summary description for ServiceTest
    /// </summary>
    [TestClass]
    public class ServiceTest
    {
        public ServiceTest()
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
        public void Expository()
        {
            // NOTE: The Service class is a VERY thin convenience wrapper around the Get service.
            //  Allows us to very easily get and use a previously created instance of a "service" object
            //  which is probably a class with static methods and instance wrappers.  Here's an example:

            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( typeof( IClassA ), typeof( ClassA ), true ) );

            // NOTE: You would do this somewhere in startup of your app (and configure it too.  Be thread-safe though!)
            IClassA registryInstance = Get.Instance<IClassA>( true );
            // NOTE: That the true causes it (the new instance) to be saved in the Locator registry.

            // ACT
            IClassA serviceInstance = Service.Of<IClassA>();
            string oped = serviceInstance._Op_( "Cornelius" );

            // ASSERT
            Assert.AreSame( registryInstance, serviceInstance );
            Assert.AreEqual( oped, "Hi Cornelius" );
        }

        [TestMethod]
        public void Expository2()
        {
            // NOTE: Here's a variation.

            // ARRANGE
            Locator.Registry.Clear();
            Locator.Add( new LocatorType( typeof( IClassA ), typeof( ClassA ), true ) );

            // NOTE: You would do this somewhere in startup of your app (and configure it too.  Be thread-safe though!)
            IClassA registryInstance = Service.New<IClassA>();
            registryInstance._Greeting_ = "Hola";

            // ACT
            IClassA serviceInstance = Service.Of<IClassA>();
            string oped = serviceInstance._Op_( "Sophia" );

            // ASSERT
            Assert.AreSame( registryInstance, serviceInstance );
            Assert.AreEqual( oped, "Hola Sophia" );
        }
    }

    public class ClassA : IClassA
    {
        public static string Greeting { get; set; }

        public static string Op( string name )
        {
            if( string.IsNullOrEmpty( Greeting ) )
            {
                return "Hi " + name;
            }
            else
            {
                return Greeting + " " + name;
            }
        }

        public string _Greeting_ 
        {
            get { return ClassA.Greeting; }
            set { ClassA.Greeting = value; }
        }

        public string _Op_(string name)
        {
            return ClassA.Op( name );
        }
    }

    public interface IClassA
    {
        string _Greeting_ { get; set; }
        string _Op_( string name );
    }
}
