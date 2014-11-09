using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace org.ncore.Ioc
{
    public class LocatorConfiguration : ConfigurationSection
    {
        // Declare the types collection property.
        // Note: the "IsDefaultCollection = false" instructs 
        // .NET Framework to build a nested section of 
        // the kind <types>...</types>.
        [ConfigurationProperty( "types", IsDefaultCollection = false )]
        [ConfigurationCollection( typeof( TypeElementCollection ),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove" )]
        public TypeElementCollection Types
        {
            get
            {
                TypeElementCollection typeElementCollection = (TypeElementCollection)base[ "types" ];
                return typeElementCollection;
            }
        }
    }

    public class TypeElementCollection : ConfigurationElementCollection
    {
        public TypeElementCollection()
        {
            // When the collection is created, always add one element 
            // with the default values. (This is not necessary; it is
            // here only to illustrate what can be done; you could 
            // also create additional elements with other hard-coded 
            // values here.)
            TypeElement type = (TypeElement)CreateNewElement();
            Add( type );
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeElement();
        }

        protected override Object GetElementKey( ConfigurationElement element )
        {
            return ( (TypeElement)element ).Name;
        }

        public TypeElement this[ int index ]
        {
            get
            {
                return (TypeElement)BaseGet( index );
            }
            set
            {
                if( BaseGet( index ) != null )
                {
                    BaseRemoveAt( index );
                }
                BaseAdd( index, value );
            }
        }

        new public TypeElement this[ string Name ]
        {
            get
            {
                return (TypeElement)BaseGet( Name );
            }
        }

        public int IndexOf( TypeElement type )
        {
            return BaseIndexOf( type );
        }

        public void Add( TypeElement type )
        {
            BaseAdd( type );
        }
        protected override void BaseAdd( ConfigurationElement element )
        {
            BaseAdd( element, false );
        }

        public void Remove( TypeElement type )
        {
            if( BaseIndexOf( type ) >= 0 )
                BaseRemove( type.Name );
        }

        public void RemoveAt( int index )
        {
            BaseRemoveAt( index );
        }

        public void Remove( string name )
        {
            BaseRemove( name );
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class TypeElement : ConfigurationElement
    {
        [ConfigurationProperty( "name", IsKey = true, IsRequired = true )]
        public string Name
        {
            get { return (string)this[ "name" ]; }
        }

        [ConfigurationProperty( "assembly", DefaultValue = "---missing---" )]
        public string Assembly
        {
            get { return (string)this[ "assembly" ]; }
            set { this[ "assembly" ] = value; }
        }

        [ConfigurationProperty( "typeName", DefaultValue = "---missing---" )]
        public string TypeName
        {
            get { return (string)this[ "typeName" ]; }
            set { this[ "typeName" ] = value; }
        }

        [ConfigurationProperty( "allowSave", DefaultValue = "false" )]
        public bool AllowSave
        {
            get { return (bool)this[ "allowSave" ]; }
            set { this[ "allowSave" ] = value; }
        }
    }
}
