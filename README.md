Add an instance by loading the blazor page, clicking on "Counter", putting a name such as "dev.blah.com" in the text box and clicking "Add". You can add as many instances as you want but for now it will crash if you try to add the same name more than once.

To see the hierarchy after you create some instances, install the Petabridge.cmd global tool:

    dotnet tool install --global pbm

When you start the app, in the console output you should see a message similar to:
> [INFO][7/22/2022 4:00:11 PM][Thread 0009][akka://akkasimulation/user/petabridge.cmd] petabridge.cmd host bound to [0.0.0.0:9110]

From a command line, run:

    pbm localhost:9110

After it connects, run the following to see the hierarchy of actors created which should show an InstanceCoordinator and any instances you created:

    actor hierarchy