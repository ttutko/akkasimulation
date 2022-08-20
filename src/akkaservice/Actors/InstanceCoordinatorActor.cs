using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScope _scope;

        public InstanceCoordinatorActor(IServiceProvider sp)
        {
            _scope = sp.CreateScope();

            Receive<CreateInstanceMessage>(message =>
            {
                log.Info("Received create instance message: {0}", message.Name);
                var _instance = Context.ActorOf(InstanceActor.Props(message.Name, message.Config), message.Name);
                _instance.Tell(new StartInstanceMessage() { StartMessage = "Starting up!" });
            });            
        }

        protected override void PostStop()
        {
            _scope.Dispose();

            base.PostStop();
        }
    }
}
