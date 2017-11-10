using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MyMicrocontroller
{
    public static class Trace
    {
        private static string resources = "Resources";
        private static List<Message> messages = new List<Message>();

        public static void Log(
            string key,
            ListView control,
            MessageType type = MessageType.Success,
            string portName = null)
        {
            messages.Add(new Message($"{resources}/{type.ToString().ToUpper()}.png",
                $"{(portName != null ? $"[{portName}]: " : string.Empty)}{GetStringByKey(key) ?? key}"));
            control.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                control.DataContext = null;
                control.DataContext = messages;
                control.SelectedIndex = messages.Count - 1;
                control.ScrollIntoView(control.SelectedItem);
            }));
        }

        public static string GetStringByKey(string key)
        {
            return (string)Application.Current.Resources[key];
        }

        public static void Clear(ListView control)
        {
            messages.Clear();
            control.DataContext = null;
        }
    }

    public enum MessageType
    {
        Error,
        Warning,
        Info,
        Success
    }
}
