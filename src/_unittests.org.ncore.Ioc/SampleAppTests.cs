using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using org.ncore.Ioc;
using _unittests.org.ncore.Ioc.SampleApp.RealImp;
using _unittests.org.ncore.Ioc.SampleApp.Interfaces;

namespace _unittests.org.ncore.Ioc
{
    /// <summary>
    /// Summary description for SampleAppTests
    /// </summary>
    [TestClass]
    public class SampleAppTests
    {
        public SampleAppTests()
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
        public void Expository()
        {
            // NOTE: BIG FAT DISCLAIMER HERE. THIS WHOLE THING IS FOR EXPOSITORY PURPOSES ONLY.
            //  THIS IS IN NO WAY INTENDED TO BE A FUNCTION APPLICATION.  IT'S UTTERLY NON-
            //  THREAD-SAFE AND WOULD NEVER WORK UNDER MULTI-THREADED PURPOSES.  IT'S NOT MEANT
            //  TO DEMONSTRATE ANY ARCHITECTURAL PATTERNS EXCEPT FOR NCORE'S FLAVOR OF IOC.
            //  PLEASE DON'T EMAIL ME SAYING "WHAT'S WRONG WITH YOU?! THAT SAMPLE APP IS TOTALLY
            //  NOT THREAD SAFE!! HOW CAN YOU POSSIBLY BE CLAIMING THIS IS A VALID ARCHITECTURE?!
            //  YOU'RE GOING TO CONFUSE A LOT OF NEWBIES!"  SERIOUSLY, I GET THIS AND I'M ONLY
            //  DEMOING IOC PATTERNS USING THE SIMPLEST (BUT NOT HORRIBLY OVER-SIMPLIFIED) 
            //  SAMPLE IMPLEMENTATION.  OK.  NOW THAT WE HAVE THAT OUT OF THE WAY...

            // NOTE: This is essentially exactly what the KernelConfiguration does when
            //  it bootstraps from app.config.  -JF
            Kernel.Registry.Add(
                new KernelType( "WidgetService",
                    "_unittests.org.ncore.Ioc",
                    "_unittests.org.ncore.Ioc.SampleApp.RealImp.WidgetService" ) );

            // In the old days we would've done the following.  Note that we have to have
            //  the _unittests.org.ncore.Ioc.SampleApp.RealImp imported in order to do this
            //  which is already tightly binding us to a specific implementation.  Have a
            //  quick look at the implementation of WidgetService.Create - this signature:
            //      public static Widget Create( Widget widget )
            //  This should help you to get a feel for what's going on under the covers -
            //  really just a simple Repo kind of pattern for storing entities of a 
            //  particular type.  And here's how we call it:
            Widget widget1 = WidgetService.CreateFromConcrete( new Widget()
            {
                Name = "Nike FuelBand",
                Color = "red",
                Size = 1
            } );

            Debug.WriteLine( "widget.Id: " + widget1.Id );
            Assert.AreEqual( 1, widget1.Id );

            // If we were to peek at the "RealImp" version of the WidgetStore (which is 
            //  the "backing store" for the WidgetService) we'd see this Widget instance
            //  in there.  Mission accomplished...

            // The problem here is all of the compile-time binding.  We know about the
            //  _unittests.org.ncore.Ioc.SampleApp.RealImp namespace, and we know about
            //  Widgets and WidgetServices.  This means that changing implementations
            //  is a compile-time activity and doing it dynamically at runtime would
            //  require some kind of abstraction layer.  This is where IoC comes in.

            //  Here's the same code (functionaly) with the NCore Service shim used
            //  to produce a dyamic wrapper class for the underlying service, effectively
            //  de-coupling it from the concrete type.  And yes, this is a bit of a 
            //  trick and results in an abscence of compile-time checking because of the
            //  use of dynamics.  Some .NET developers may be uncomfortable with this
            //  but lots of excellent enterprise and web-scale code has been written
            //  without the benefit of strong typing and compile-time type linking
            //  of types (any interpretted framework, e.g. Rails, Node.js, etc.)
            dynamic WidgetSvc = New.Service( "WidgetService" );
            Widget widget2 = WidgetSvc.CreateFromConcrete( new Widget()
            {
                Name = "Nike FuelBand",
                Color = "purple",
                Size = 2
            } );

            Debug.WriteLine( "widget2.Id: " + widget2.Id );
            Assert.AreEqual( 2, widget2.Id );

            // An improvement from a coupling standpoint, but we're still tightly 
            //  coupled to the RealImp version of Widget and WidgetStore.  How do 
            //  we approach solving this using DI?  For the answer we need to drill 
            //  down into the implementation of WidgetService but first, we need
            //  to add a couple more types to our service locator (again, this can
            //  be done programmatically at runtime OR in app.config at deploy-time.

            Kernel.Registry.Add(
                new KernelType( "WidgetStore",
                    "_unittests.org.ncore.Ioc",
                    "_unittests.org.ncore.Ioc.SampleApp.RealImp.WidgetStore" ) );

            Kernel.Registry.Add(
                new KernelType( "_unittests.org.ncore.Ioc.SampleApp.Interfaces.IWidget",
                    "_unittests.org.ncore.Ioc",
                    "_unittests.org.ncore.Ioc.SampleApp.RealImp.Widget" ) );

            // We're going to do the long form here so that everything is really obvious
            //  and explicit, but this code can be condensed down considerably so that
            //  it looks more reminiscent of the above versions.
            Injector w3injector = new Injector( 
                new InjectorRegistry{
                    { "Name", "Nike FuelBand" },
                    { "Color", "teal" },
                    { "Size", 3 }
                });

            IWidget widget3 = New.Instance<IWidget>( w3injector );
            // Two more minor notes:
            //      1) I'm reusing the WidgetSvc we created above
            //      2) In real-world use we wouldn't call these 
            //         CreateFromConcrete and CreateFromInterface.
            //         We would just have a single Create method
            //         that would look like the CreateFromInterface
            //         method.
            widget3 = WidgetSvc.CreateFromInterface( widget3 );

            Debug.WriteLine( "widget3.Id: " + widget3.Id );
            Assert.AreEqual( 3, widget3.Id );

            // Consolidated down a bit it looks like this:
            IWidget widget4 = New.Instance<IWidget>( 
                new InjectorRegistry{
                    { "Name", "Nike FuelBand" },
                    { "Color", "yellow" },
                    { "Size", 4 }
                } );
            widget4 = WidgetSvc.CreateFromInterface( widget4 );

            Debug.WriteLine( "widget4.Id: " + widget4.Id );
            Assert.AreEqual( 4, widget4.Id );

            // TODO: Look into doing the following (see commented out constructor on InjectorRegistry):
           InjectorRegistry r = new InjectorRegistry(
                new
                {
                    Name = "Nike FuelBand",
                    Color = "teal",
                    Size = 3
                }
            );
            // Which would allow us add an override on Injector to do this:
            //Injector myInjector = new Injector(
            //    new
            //    {
            //        Name = "Nike FuelBand",
            //        Color = "teal",
            //        Size = 3
            //    } );

           // The super-terse version.  Nice!
           IWidget widget5 = New.Instance<IWidget>( new{
                    Name = "Nike FuelBand",
                    Color = "pink",
                    Size = 5
                });
           widget5 = WidgetSvc.CreateFromInterface( widget5 );

           Debug.WriteLine( "widget5.Id: " + widget5.Id );
           Assert.AreEqual( 5, widget5.Id );


            // TODO: Imporant next-step refactor here is to change the Kernel to 
            //  use New.Instance(useKernel=false) for it's object creation so that
            //  (interestingly) the Kernal uses New.Instance() and New.Instance()
            //  can optionally use the Kernal.  Right now there's total duplication
            //  of object creation responsibilities here.  That code should be
            //  solely the responsibility of New.Instance() [though as a side
            //  note the Kernal should NEVER call New.Instance(useKernel=true)
            //  because that would just be weird, right?  And looking at the 
            //  code I think the Kernel becomes a lot simpler and is really just
            //  about service location.  Though maybe we enrich it by somehow 
            //  supporting constructor and property injection from the registry
            //  (though how we would do that from a config file is a puzzle)
            //  and New.Instance() would need to a new override that accepts
            //  Assembly and TypeName (like the kernel) for direct creation of a
            //  type.  I don't know.  I guess I'm confusing myself here... -JF
            // UPDATE: Yeah - this thinking is probably messed up.  There's 
            //  definitely confusion about the roles of the various objects in
            //  this library and how they should and shouldn't be used that really
            //  need to be sorted out.  Right now my goal is to ephasize the New
            //  object and see if there's really an ongoing need for the Kernel
            //  to be about more than service location (and shared instance storage
            //  though frankly I'm a little wigged out by that too...)  Anyway it 
            //  seems entirely possible that you could create a config file structure
            //  that incorporated the injection of constructor and/or member params
            //  on object creation such that Kernel.GetOrCreate() would still make
            //  some sense (and maybe even be uniquely useful) because the creation
            //  of both shared and unique instances could also include config-file
            //  file driven pre-configuration of instance, but lazily (on-demand)
            //  I just don't know how useful this really is so I'm not particularly
            //  keen on pushing too hard on it until I'm more confident that there's
            //  a purpose to it and I'm not just muddying the waters further.  It's
            //  worth pointing out that I *may* very well realize that the confusion
            //  is simply because Kernel should actually be folded into New entirely
            //  or that it should be divided up between New and some other classes,
            //  possibly a pure "Locator" for the service location and KernelRegistry
            //  stuff, and then a new Get class that is semantically similar to the 
            //  New class (e.g. Get.Instance<T>() ) except that it only retrieves 
            //  shared instances from the Locator's registry.  The possible exception
            //  being that it could also have the GetOrCreate() behavior, in which 
            //  case it would be most sensibe to call New.Instance internally anyway.
            //  So maybe something like:
            //      Get.OrCreate(), 
            //  or 
            //      Get.Instance( bool allowCreate)
            //  Anyway, very confusing.  -JF
        }
    }
}
