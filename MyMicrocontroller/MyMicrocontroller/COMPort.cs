using System.IO.Ports;
using System;

namespace MyMicrocontroller
{
    public class COMPort
    {
        private SerialPort serial;

        public SerialPort Serial { get => serial; set => serial = value; }

        public COMPort() { }

        public COMPort(string name)
        {
            serial = new SerialPort(name, 38400, Parity.None, 8, StopBits.One);
        }

        public string GetName()
        {
            return serial.PortName;
        }

        public bool IsOpen()
        {
            return serial.IsOpen;
        }

        public EventMessage Open(string name)
        {
            try
            {
                if (serial?.PortName.Equals(name) == true && serial.IsOpen)
                {
                    return EventMessage.PortAlreadyOpen;
                }
                serial = new SerialPort(name, 38400, Parity.None, 8, StopBits.One);
                serial.Open();
                return EventMessage.PortOpened;
            }
            catch (Exception)
            {
                return EventMessage.PortOpeningError;
            }
        }

        public EventMessage Close()
        {
            try
            {
                if (!IsOpen())
                {
                    return EventMessage.PortAlreadyClose;
                }
                serial.Close();
                return EventMessage.PortClosed;
            }
            catch (Exception)
            {
                return EventMessage.PortClosingError;
            }
        }

        public EventMessage Execute(string command)
        {
            if(!IsOpen())
            {
                return EventMessage.PortNotOpened;
            }
            return EventMessage.OperationComplited;
        }

        public EventMessage Run()
        {
            if (!IsOpen())
            {
                return EventMessage.PortNotOpened;
            }
            return EventMessage.MainOperationComplited;
        }
    }

    public enum EventMessage
    {
        PortOpened = 0,
        PortClosed = 1,
        PortOpeningError = 2,
        PortClosingError = 3,
        PortAlreadyOpen = 4,
        PortAlreadyClose = 5,
        PortNotOpened = 6,
        MainOperationNotComplited = 7,
        MainOperationComplited = 8,
        OperationNotComplited = 9,
        OperationComplited = 10
    }
}