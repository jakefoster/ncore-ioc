// TODO: Add more structure to config sections stuff.  RIght now it's like this:
/*
    <configSections>
        <section name="kernel" type="org.ncore.Ioc.KernelConfiguration,org.ncore.Ioc"/>
    </configSections>
    <kernel>
        <types>
            <clear/>
            <add name="org.ncore.ServicedApi.Persistence.IObjectStore" assembly="MyApp.Orm.LinqToSql" typeName="MyApp.Orm.LinqToSql.EntityDataContext"/>
        </types>
    </kernel>
*/
//  but it would be much better to have something like this:
/*
    <configSections>
        <sectionGroup name="org.ncore.ServicedApi" type="org.ncore.ServicedApi.ConfigurationSectionGroup,org.ncore.ServicedApi">
            <sectionGroup name="container" type="org.ncore.Ioc.ConfigurationSectionGroup,org.ncore.ServicedApi">
                <section name="kernel" type="org.ncore.Ioc.KernelConfiguration,org.ncore.ServicedApi"/>
            </sectionGroup>
        </sectionGroup>
    </configSections>
    <org.ncore.ServicedApi>
        <container>
            <kernel>
                <types>
                    <clear/>
                    <add name="org.ncore.ServicedApi.Persistence.IObjectStore" assembly="MyApp.Orm.LinqToSql" typeName="MyApp.Orm.LinqToSql.EntityDataContext"/>
                </types>
            </kernel>
        </container>
    </org.ncore.ServicedApi>
*/