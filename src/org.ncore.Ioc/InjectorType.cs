using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public class InjectorType
    {
        public static readonly object Null = new object();

        // NOTE: Non-obvious, but setting either .Assembly or .TypeName wipes out .Type
        //  and conversely, setting .Type sets the underlying values _assembly and 
        //  _typeName so that the class can never really be in an inconsistent state.  JF
        private string _assembly;
        public string Assembly 
        { 
            get
            {
                return _assembly;
            }

            set
            {
                _assembly = value;
                _type = null;
            }
        }

        private string _typeName;
        public string TypeName
        {
            get
            {
                return _typeName;
            }

            set
            {
                _typeName = value;
                _type = null;
            }
        }

        private Type _type;
        public Type Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
                _assembly = _type.Assembly.FullName;
                _typeName = _type.FullName;
            }
        }
        
        public InjectorType(){}
        public InjectorType( Type type ) 
        {
            this.Type = type;
        }

        public InjectorType( string assembly, string typeName )
        {
            this.Assembly = assembly;
            this.TypeName = typeName;
        }
    }
}
