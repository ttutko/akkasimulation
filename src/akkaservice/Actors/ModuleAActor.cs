using System;

namespace akkaservice;

public class ModuleAActor : ModuleActor
{
    private readonly ModuleAAconfiguration _config;
    public ModuleAActor(IServiceProvider sp, string config) : base(sp)
    {
        //_config = config;
    }
}