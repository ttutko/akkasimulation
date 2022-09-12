using System;
using System.Text.Json;

namespace akkaservice;

public class ModuleBActor : ModuleActor
{
    private readonly ModuleBAconfiguration? _config;
    public ModuleBActor(IServiceProvider sp, string config) : base(sp)
    {
        _config = JsonSerializer.Deserialize<ModuleBAconfiguration>(config);
    }
}