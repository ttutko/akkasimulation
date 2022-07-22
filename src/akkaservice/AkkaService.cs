using Akka.Actor;
using Akka.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akkaservice
{
    public class AkkaService : IHostedService
    {
        private ActorSystem? _actorSystem;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        private readonly IHostApplicationLifetime _applicationLifetime;

        public AkkaService(IConfiguration configuration, IServiceProvider serviceProvider, IHostApplicationLifetime applicationLifetime)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _applicationLifetime = applicationLifetime; 
        }

        public async Task StartAsync(CancellationToken cancellation)
        {
            var bootstrap = BootstrapSetup.Create();

            // enable DI support inside this ActorSystem, if needed
            var disetup = DependencyResolverSetup.Create(_serviceProvider);

            // merge this setup (and any others) together into ActorSystemSetup
            var actorSystemSetup = bootstrap.And(disetup);

            // start ActorSystem
            _actorSystem = ActorSystem.Create("akkasimulator", actorSystemSetup);

#pragma warning disable CA2016 // Forward the 'CancellationToken' parameter to methods
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _actorSystem.WhenTerminated.ContinueWith(tr =>
            {
                _applicationLifetime.StopApplication();
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning restore CA2016 // Forward the 'CancellationToken' parameter to methods

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // strictly speaking this may not be necessary - termination the ActorSystem would also work
            // but this call guarantees that the shutdown of the cluster is graceful regardless
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
