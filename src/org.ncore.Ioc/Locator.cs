using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    // NOTE: Some references for singleton...
    // http://csharpindepth.com/Articles/General/Singleton.aspx
    /*
    public sealed class Singleton
    {
        private static readonly Lazy<Singleton> lazy =
            new Lazy<Singleton>( () => new Singleton() );

        public static Singleton Instance { get { return lazy.Value; } }

        private Singleton()
        {
        }
    }
    */

    // NOTE: Some references for the ConcurrentDictionary:
    // http://msdn.microsoft.com/en-us/library/dd997369(v=vs.110).aspx
    public class Locator
    {
        public static LocatorRegistry Registry = new LocatorRegistry();

        public static void Add( LocatorType locatorType )
        {
            Registry.Add( locatorType );
        }

        public static void Clear()
        {
            Registry.Clear();
        }

        public static void Reset()
        {
            Registry = new LocatorRegistry();
        }

        // TODO: Any point to this?  -JF
        public static void Initialize( Action initializer )
        {
            initializer();
        }
    }
}