namespace MyMicrocontroller
{
    public class Message
    {
        public Message(string type, string text)
        {
            Type = type;
            Text = text;
        }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
