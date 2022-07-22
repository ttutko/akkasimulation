using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace akkaservice
{
    public class InstanceCoordinatorActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();

        public InstanceCoordinatorActor()
        {
            Receive<CreateInstanceMessage>(message =>
            {
                log.Info("Received create instance message: {0}", message.Name);
                var _instance = Context.ActorOf(InstanceActor.Props(message.Name), message.Name);
                _instance.Tell(new StartInstanceMessage() { StartMessage = "Starting up!" });
            });            
        }
    }
}
