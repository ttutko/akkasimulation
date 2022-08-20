using System;
using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.DependencyInjection;

namespace akkaservice;

public abstract class ModuleActor : ReceiveActor
{
    protected readonly ILoggingAdapter _log = Context.GetLogger();
    protected readonly IServiceScope _scope;
    
    public ModuleActor(IServiceProvider sp)
    {
        _scope = sp.CreateScope();


    }

    protected override void PostStop()
    {
        _scope.Dispose();

        base.PostStop();
    }
}