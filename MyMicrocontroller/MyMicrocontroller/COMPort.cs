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

        public void Execute(string command)
        {

        }

        public void Run()
        {

        }
        /*
         * serial.Open();
         * Поле Serial.isOpen позволяет проверить нам, открыт ли COM порт.
         * Работать с COM портом мы можем как по таймеру, с определенным интервалом, так и по событию DataReceived.
         * serial.Read(byte[] Buffer,int offset,int count) — считать count байт в массив Buffer, со смещением offset.
         * serial.ReadByte() — считать один байт.
         * serial.ReadChar() — считать один символ.
         * serial.ReadExisting() — считать строкой все содержимое буфера.
         * serial.ReadLine() — считать строку из буфера до символа Serial.NewLine .
         * serial.ReadTo(String str) — считать содержимое буфера, до достижения строки str.
         * serial.Write(byte[] Buffer,int offset,int count) — записать count байт массива Buffer, со смещением offset.
         * serial.WriteLine(string text) — записать строку text в порт.
         * serial.Close() - по окончанию.
         */
    }

    public enum EventMessage
    {
        PortOpened = 0,
        PortClosed = 1,
        PortOpeningError = 2,
        PortClosingError = 3,
        PortAlreadyOpen = 4,
        PortAlreadyClose = 5
    }
}