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
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class InjectorTests
    {
        public InjectorTests()
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
        public void New_Instance_constructor_and_property_injection_works()
        {
            // ARRANGE
            Kernel.Registry.Add( new KernelType( typeof( Foo ), typeof( Foo ) ) );

            // ACT
            Injector injector = new Injector()
            {
                MemberRegistry = new InjectorRegistry { { "SomethingElse", "I got injected!" } },
                ConstructorParams = new object[] { new Bar() { What = "Yow!" }, 2, "For sure!!", "Hot damn!" }
            };

            Foo foo = New.Instance<Foo>( injector );

            // NOTE: There's a nice wrapper on New.Instance that allows you to pass in a registry and/or
            //  array of constructor params and the Injector will be created for you internally.  -JF
            //Foo foo = New.Instance<Foo>(
            //    new InjectorRegistry { { "SomethingElse", "I got injected!" } },
            //    new object[] { new Bar() { What = "Yow!" }, 2, "For sure!!", "Hot damn!" } );

            // ASSERT
            Assert.AreEqual( "I got injected!", foo.SomethingElse );
            Assert.AreEqual( "Yow!", foo.TheBar.What );
            Assert.AreEqual( 2, foo.Count );
            Assert.AreEqual( "For sure!!", foo.Something );
        }

        [TestMethod]
        public void Kernel_constructor_and_property_injection_works()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( Foo ), typeof( Foo ) ) );

            // ACT
            Foo foo = Kernel.CreateObject<Foo>( new object[] { new Bar() { What = "Yow!" }, 2, "For sure!!", "Hot damn!" } );

            // ASSERT
            Assert.AreEqual( "Yow!", foo.TheBar.What );
            Assert.AreEqual( 2, foo.Count );
            Assert.AreEqual( "For sure!!", foo.Something );
            Assert.AreEqual( "Hot damn!", foo.SomethingElse );
        }

        [TestMethod]
        public void Works_Expository()
        {
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( Fighter ), typeof( Samurai ) ) );

            Injector injector = new Injector( new InjectorRegistry{
                { typeof(IThrowableWeapon), typeof(ThrowingStar) },
                { "Weapon", new Naginata(2) },
                { "AlternateWeapon", new Katana(){SliceCount = 3} },
                { "_secretPower", typeof(SheerTerror) },
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            Fighter myFighter = New.Instance<Fighter>( injector );
            Assert.AreEqual( typeof( Samurai ), myFighter.GetType() );
            Assert.AreEqual( "Whizzz, Thud!", myFighter.ThrowableWeapon.Throw() );
            Assert.AreEqual( "Stab! Stab!", myFighter.Weapon.Use() );
            Assert.AreEqual( "Slice! Slice! Slice!", myFighter.AlternateWeapon.Use() );
            if( myFighter is Ninja )
            {
                // NOTE: If you've got a secret power, use it!
                Assert.AreEqual( "Stop! You're scaring me!", ( (Ninja)myFighter ).UseSecretPower() );
            }
            Assert.AreEqual( "Hey! Who turned out the lights!", myFighter.SpecialPower.Use() );
        }

        [TestMethod]
        public void Works_dynamic_expository()
        {
            Kernel.Registry.Reset();
            //Kernel.Registry.Add( new KernelType( "Fighter", typeof( Samurai ) ) );
            Kernel.Registry.Add( new KernelType( typeof( Fighter ), typeof( Samurai ) ) );

            Injector injector = new Injector( new InjectorRegistry{
                { typeof(IThrowableWeapon), typeof(ThrowingStar) },
                { "Weapon", new Naginata(2) },
                { "AlternateWeapon", new Katana(){SliceCount = 3} },
                { "_secretPower", typeof(SheerTerror) },
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            //dynamic myInstance = New.Instance( "Fighter", injector );
            dynamic myFighter = New.Instance( typeof( Fighter ), injector );
            Assert.AreEqual( typeof( Samurai ), myFighter.GetType() );
            Assert.AreEqual( "Whizzz, Thud!", myFighter.ThrowableWeapon.Throw() );
            Assert.AreEqual( "Stab! Stab!", myFighter.Weapon.Use() );
            Assert.AreEqual( "Slice! Slice! Slice!", myFighter.AlternateWeapon.Use() );
            if( myFighter is Ninja )
            {
                // NOTE: If you've got a secret power, use it!
                Assert.AreEqual( "Stop! You're scaring me!", ( (Ninja)myFighter ).UseSecretPower() );
            }
            Assert.AreEqual( "Hey! Who turned out the lights!", myFighter.SpecialPower.Use() );
        }

        [TestMethod]
        public void Works_Ninja_Expository()
        {
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( Fighter ), typeof( Ninja ) ) );

            Injector injector = new Injector( new InjectorRegistry{
                { typeof(IThrowableWeapon), typeof(GlassDust) },
                { "Weapon", new Naginata(2) },
                { "AlternateWeapon", new Katana(){SliceCount = 3} },
                { "_secretPower", typeof(SheerTerror) },
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            Fighter myFighter = New.Instance<Fighter>( injector );
            Assert.AreEqual( typeof( Ninja ), myFighter.GetType() );
            Assert.AreEqual( "Puff... Gah! My eyes!!", myFighter.ThrowableWeapon.Throw() );
            Assert.AreEqual( "Stab! Stab!", myFighter.Weapon.Use() );
            Assert.AreEqual( "Slice! Slice! Slice!", myFighter.AlternateWeapon.Use() );
            Assert.AreEqual( "Stop! You're scaring me!", ( (Ninja)myFighter ).UseSecretPower() );
            Assert.AreEqual( "Hey! Who turned out the lights!", myFighter.SpecialPower.Use() );
        }


        [TestMethod]
        public void New_on_instance_with_dynamic_field()
        {
            Kernel.Registry.Reset();

            Injector injector = new Injector( new InjectorRegistry{
                { "_secretPower", typeof(TemporaryBlindness) }
            } );

            Ninja myNinja = New.Instance<Ninja>( injector );
            Assert.AreEqual( "Hey! Who turned out the lights!", myNinja.UseSecretPower() );
        }

        [TestMethod]
        public void New_on_instance_with_dynamic_property()
        {
            Kernel.Registry.Reset();

            Injector injector = new Injector( new InjectorRegistry{
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            Samurai mySamurai = New.Instance<Samurai>( injector );
            Assert.AreEqual( "Hey! Who turned out the lights!", mySamurai.SpecialPower.Use() );
        }
    }


    /* Injection play */
    public interface IBladedWeapon
    {
        string Use();
    }

    public class Nagimaka : IBladedWeapon
    {
        public string Use()
        {
            return "Big Slice!";
        }
    }

    public class Naginata : IBladedWeapon
    {
        public int StabCount { get; set; }
        public string Use()
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 1; i <= StabCount; i++ )
            {
                builder.Append( "Stab!" );
                if( i < StabCount )
                {
                    builder.Append( " " );
                }
            }
            return builder.ToString();
        }

        public Naginata()
        {
        }

        public Naginata( int stabCount )
        {
            StabCount = stabCount;
        }
    }

    public class Katana : IBladedWeapon
    {
        public int SliceCount { get; set; }
        public string Use()
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 1; i <= SliceCount; i++ )
            {
                builder.Append( "Slice!" );
                if( i < SliceCount )
                {
                    builder.Append( " " );
                }
            }
            return builder.ToString();
        }

        public Katana()
        {
            SliceCount = 1;
        }

        public Katana( int stabCount )
        {
            SliceCount = stabCount;
        }
    }

    public interface IThrowableWeapon
    {
        string Throw();
    }

    public class ThrowingStar : IThrowableWeapon
    {
        public string Throw()
        {
            return "Whizzz, Thud!";
        }
    }

    public class GlassDust : IThrowableWeapon
    {
        public string Throw()
        {
            return "Puff... Gah! My eyes!!";
        }
    }

    public class TemporaryBlindness
    {
        public string Use()
        {
            return "Hey! Who turned out the lights!";
        }
    }

    public class SheerTerror
    {
        public string Use()
        {
            return "Stop! You're scaring me!";
        }
    }

#pragma warning disable 649 // Field '_unittests.org.ncore.Ioc.Ninja._secretPower' is never assigned to, and will always have its default value null
    public class Ninja : Fighter
    {
        private dynamic _secretPower;

        public string UseSecretPower()
        {
            return _secretPower.Use();
        }
    }
#pragma warning restore 649

    public class Samurai : Fighter
    {
    }

    public class Fighter
    {

        public dynamic SpecialPower { get; private set; }
        public IBladedWeapon Weapon { get; set; }
        public IBladedWeapon AlternateWeapon { get; set; }
        public IThrowableWeapon ThrowableWeapon { get; set; }

        public Fighter()
        {

        }
    }

    public class Bar
    {
        public string What { get; set; }
        public Bar()
        {

        }
    }

    public class Foo
    {
        public Bar TheBar { get; set; }
        public int Count { get; set; }
        public string Something { get; set; }
        public string SomethingElse { get; set; }

        public Foo() { }

        public Foo( Bar theBar, int count, string something )
        {
            TheBar = theBar;
            Count = count;
            Something = something;
        }

        public Foo( Bar theBar, int count, string something, string somethingElse )
        {
            TheBar = theBar;
            Count = count;
            Something = something;
            SomethingElse = somethingElse;
        }
    }
}
