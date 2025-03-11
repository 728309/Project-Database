namespace C__and_Project.Models
{
    public class SimpleMessage
    {
        public string Name { get; set; }
        public string MessageText { get; set; }

        public SimpleMessage()
        {
            Name = "";
            MessageText = "";
        }

        public SimpleMessage(string name, string messageText)
        {
            Name = name;
            MessageText = messageText;
        }
    }

}

