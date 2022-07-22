namespace akkaservice
{
    public class CreateInstanceMessage
    {
        public string Name { get; internal set; }

        public CreateInstanceMessage(string name)
        {
            Name = name;
        }
    }
}