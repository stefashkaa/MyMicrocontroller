using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
