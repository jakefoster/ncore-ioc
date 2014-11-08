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
    [TestClass]
    public class GetTests
    {
        [TestMethod]
        public void Instance_dynamic_from_name_works()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            MyClassA classA = new MyClassA() { Greeter = "Changed" };
            Kernel.Registry.Add( new KernelType( "MyClass", classA ) );

            // ACT
            dynamic myClass = Get.Instance("MyClass");
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from Changed", greeting );
            Assert.AreSame( classA, myClass );
        }

        [TestMethod]
        public void Instance_dynamic_from_name_works_creates_new()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( "MyClass", typeof( MyClassA ) ) { AllowSave = true } );

            // ACT
            dynamic myClass = Get.Instance( "MyClass", true );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.AreSame( Kernel.Registry["MyClass"].Instance, myClass );
        }

        [TestMethod]
        public void Instance_dynamic_from_type_works()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            MyClassA classA = new MyClassA() { Greeter = "Changed" };
            Kernel.Registry.Add( new KernelType( typeof(IMyClass), classA ) );

            // ACT
            dynamic myClass = Get.Instance( typeof( IMyClass ) );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from Changed", greeting );
            Assert.AreSame( classA, myClass );
        }

        [TestMethod]
        public void Instance_dynamic_from_type_works_creates_new()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( IMyClass ), typeof( MyClassA ) ) { AllowSave = true } );

            // ACT
            dynamic myClass = Get.Instance( typeof( IMyClass ), true );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.AreSame( Kernel.Registry[typeof( IMyClass ).FullName].Instance, myClass );
        }

        [TestMethod]
        public void Instance_typed_from_name_works()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            MyClassA classA = new MyClassA() { Greeter = "Changed" };
            Kernel.Registry.Add( new KernelType( "MyClass", classA ) );

            // ACT
            IMyClass myClass = Get.Instance( "MyClass" );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from Changed", greeting );
            Assert.AreSame( classA, myClass );
        }

        [TestMethod]
        public void Instance_typed_from_name_works_creates_new()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( "MyClass", typeof( MyClassA ) ) { AllowSave = true } );

            // ACT
            IMyClass myClass = Get.Instance( "MyClass", true );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.AreSame( Kernel.Registry["MyClass"].Instance, myClass );
        }

        [TestMethod]
        public void Instance_typed_from_type_works()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            MyClassA classA = new MyClassA() { Greeter = "Changed" };
            Kernel.Registry.Add( new KernelType( typeof(IMyClass), classA ) );

            // ACT
            IMyClass myClass = Get.Instance( typeof(IMyClass) );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from Changed", greeting );
            Assert.AreSame( classA, myClass );
        }

        [TestMethod]
        public void Instance_typed_from_type_works_creates_new()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( IMyClass ), typeof( MyClassA ) ) { AllowSave = true } );

            // ACT
            IMyClass myClass = Get.Instance( typeof( IMyClass ), true );
            string greeting = myClass.Greet( "Uni" );

            // ASSERT
            Assert.AreEqual( "Hello Uni from MyClassA", greeting );
            Assert.AreSame( Kernel.Registry[typeof(IMyClass).FullName].Instance, myClass );
        }

        [TestMethod]
        [ExpectedException( typeof( ApplicationException ), "The specified entry in the KernalRegistry does not exist." )]
        public void Instance_pops_when_not_in_registry()
        {
            // ARRANGE
            Kernel.Registry.Reset();

            // ACT
            IMyClass myClass = Get.Instance( typeof( IMyClass ) );

            // ASSERT
        }

        [TestMethod]
        [ExpectedException( typeof( ApplicationException ), 
            "The specified entry in the KernalRegistry does not does not allow a saved instance or does not have one." )]
        public void Instance_pops_when_registry_instance_empty_and_allowCreate_is_false()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( IMyClass ), typeof( MyClassA ) ) );

            // ACT
            IMyClass myClass = Get.Instance( typeof( IMyClass ) );

            // ASSERT
        }

        [TestMethod]
        [ExpectedException( typeof( ApplicationException ),
            "The specified entry in the KernalRegistry does not does not allow a saved instance or does not have one." )]
        public void Instance_pops_when_registry_instance_empty_and_allowSave_is_false_even_though_allowCreate_is_true()
        {
            // ARRANGE
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( IMyClass ), typeof( MyClassA ) ) { AllowSave = false } );

            // ACT
            IMyClass myClass = Get.Instance( typeof( IMyClass ), true );

            // ASSERT
        }

        public interface IMyClass
        {
            string Greet( string name );
        }

        public class MyClassA : IMyClass
        {
            public string Greeter = "MyClassA";

            public string Greet( string name )
            {
                return "Hello " + name + " from " + Greeter;
            }
        }
    }
}
