using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using org.ncore.Ioc;
using _unittests.org.ncore.Ioc;
using _unittests.org.ncore.Ioc.SampleApp.Interfaces;

namespace _unittests.org.ncore.Ioc.SampleApp.RealImp
{
    public static class WidgetService
    {
        private static int _nextId = 1;

        public static Widget CreateFromConcrete( Widget widget )
        {
            WidgetStore store = WidgetStore.Current;
            if(widget.Id != 0 || WidgetStore.Current.ContainsKey(widget.Id))
            {
                throw new ApplicationException("Can't create, this widget already exists or already has an Id assigned!" );
            }
            widget.Id = _nextId;
            _nextId++;
            WidgetStore.Current.Add( widget.Id, widget );
            return widget;
        }

        public static IWidget CreateFromInterface( IWidget widget )
        {
            dynamic WidgetStore = Dyno.New( "WidgetStore" );
            IWidgetStore store = WidgetStore.Current;
            if( widget.Id != 0 || store.ContainsKey( widget.Id ) )
            {
                throw new ApplicationException( "Can't create, this widget already exists or already has an Id assigned!" );
            }
            widget.Id = _nextId;
            _nextId++;
            store.Add( widget.Id, widget );
            return widget;
        }
    }
}
