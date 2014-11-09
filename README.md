ncore-ioc
=========

A lean little inversion control of library providing service location and dependency injection supporting both controller and property injection.

To Install
----------

Install from NuGet:  

`PM> Install-Package Newtonsoft.Json`

Some Important Notes
--------------------

Note #1: This library is still in its earyly stages and may change slightly as the patterns demand refinement.  

Note #2: There are a decent number of unit tests but, frankly, they're all over the place and need a lot of work.  There are also a bunch of tests that are inspired by the Ninject samples and that are pretty corny.  Still, they illurstrate the concepts.  Anyway, the unit tests need a lot of cleanup.

To understand the basics of what's provided here look at the unit tests (as scattered as they may be).  Specifically, the Expository() test in SampleAppTests gives a good bit of insight into New.Instance() and the intended usage (along with some colorful commentary).  Over time this library and especially the tests will get cleaned up to the point where they're really presentable.
