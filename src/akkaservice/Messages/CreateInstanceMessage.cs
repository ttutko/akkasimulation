namespace akkaservice
{
    public class CreateInstanceMessage
    {
        public string Name { get; internal set; }
        public string Config {get; internal set; }

        public CreateInstanceMessage(string name, string config)
        {
            Name = name;
            Config = config;
        }
    }
}