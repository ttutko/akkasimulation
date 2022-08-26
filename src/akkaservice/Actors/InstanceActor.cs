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
        private Dictionary<string, IActorRef> _installedModules = new Dictionary<string, IActorRef>();
        public string Name { get; private set; }
        private readonly ModuleFactory factory;
        private readonly IServiceProvider sp;

        private readonly string _initialConfig;

        public InstanceActor(string name, string initialConfiguration, ModuleFactory factory, IServiceProvider sp)
        {
            Name = name;
            _initialConfig = initialConfiguration;
            this.factory = factory;
            this.sp = sp;

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

            using (JsonDocument document = JsonDocument.Parse(_initialConfig))
            {
                JsonElement root = document.RootElement;
                JsonElement modules = root.GetProperty("modules");
                foreach (JsonElement module in modules.EnumerateArray())
                {
                    var prop = factory.CreateModule(module.GetProperty("type").GetString(), module.GetProperty("config").GetRawText());
                    var module_actor = Context.ActorOf(prop, module.GetProperty("type").GetString());
                    _installedModules.Add(module.GetProperty("type").GetString(), module_actor);
                }
            }
        }

        public static Props Props(string name, string config)
        {
            var spExtension = DependencyResolver.For(Context.System);
            var props = spExtension.Props<InstanceActor>(name, config);
            return props;
        }
    }
}
