using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public class Service
    {
        public static T New<T>()
        {
            return Get.Instance<T>( true );
        }

        public static T New<T>( string name )
        {
            return Get.Instance<T>( name, true );
        }

        public static T Of<T>()
        {
            return Get.Instance<T>();
        }

        public static T Of<T>( string name )
        {
            return Get.Instance<T>( name );
        }
    }
}
