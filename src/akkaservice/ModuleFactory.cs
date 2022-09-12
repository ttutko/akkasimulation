using System;
using Akka;

namespace akkaservice;

public class ModuleFactory
{
    private readonly IServiceProvider sp;

    public ModuleFactory(IServiceProvider sp)
    {
        this.sp = sp;
    }

    public Akka.Actor.Props CreateModule(string moduleType, string config)
    {
        switch(moduleType)
        {
            case "ModuleA":
                return Akka.Actor.Props.Create<ModuleAActor>(sp, config);
            case "ModuleB":
                return Akka.Actor.Props.Create<ModuleBActor>(sp, config);
            default:
                return null;
        }
    }
}