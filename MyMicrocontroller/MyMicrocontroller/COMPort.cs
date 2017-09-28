using System.IO.Ports;

namespace MyMicrocontroller
{
    public class COMPort
    {
        private SerialPort serial;

        public SerialPort Serial { get => serial; set => serial = value; }

        public COMPort(string name)
        {
            serial = new SerialPort(name, 38400, Parity.None, 8, StopBits.One);
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
}