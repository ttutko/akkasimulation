using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace akkaservice
{
    public class InstanceActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        private Dictionary<string, ModuleActor> _installedModules = new Dictionary<string, ModuleActor>();
        public string Name { get; private set; }

        private readonly string _initialConfig;

        public InstanceActor(string name, string initialConfiguration)
        {
            Name = name;
            _initialConfig = initialConfiguration;

            log.Info($"Initial configuration: {initialConfiguration}");

            Receive<StartInstanceMessage>(message =>
            {
                log.Info("Instance {0} started.", Name);
            });

            Receive<CommunicationMessage>(message =>
            {
                log.Info("Received Communication Message!");
            });


        }

        protected override void PreStart()
        {
            // start two recurring timers
            // both timers will be automatically disposed when actor is stopped
            Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(0.1),
                TimeSpan.FromSeconds(5), Self, new CommunicationMessage(), ActorRefs.NoSender);
        }

        public static Props Props(string name, string config)
        {
            var spExtension = DependencyResolver.For(Context.System);
            var props = spExtension.Props<InstanceActor>(name, config);
            return props;
        }
    }
}
