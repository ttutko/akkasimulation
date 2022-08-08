using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akkaservice
{
    public class InstanceActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        public string Name { get; private set; }
        
        
        public InstanceActor(string name)
        {
            Name = name;

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

        public static Props Props(string name)
        {
            return Akka.Actor.Props.Create(() => new InstanceActor(name));
        }
    }
}
