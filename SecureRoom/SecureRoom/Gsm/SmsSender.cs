using System;
using System.IO.Ports;
using System.Text;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;
using SecureRoom.Domain;

namespace SecureRoom.Gsm
{
    public class SmsSender : ISender
    {
        static SerialPort serialPort;

        private const int bufferMax = 1024;
        private static byte[] buffer = new Byte[bufferMax];
        private static string output = "";

        public SmsSender(string portName = SerialPorts.COM1, int baudRate = 19200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.ReadTimeout = -1;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort.Open();
        }

        // construct output string to print to the debug line
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bufferData = new byte[20];
            serialPort.Read(bufferData, 0, 20);
            char[] charArray = System.Text.UTF8Encoding.UTF8.GetChars(bufferData);
            for (int i = 0; i < charArray.Length - 1; i++)
            {
                if (charArray[i].ToString() == "\r")
                {
                    Debug.Print(output);
                    output = "";
                }
                else
                {
                    output += charArray[i];
                }
            }

        }

        private void Print(string line)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            byte[] bytesToSend = encoder.GetBytes(line);
            serialPort.Write(bytesToSend, 0, bytesToSend.Length);
        }

        private void PrintLine(string line)
        {
            Print(line + "\r");
        }

        private void PrintEnd()
        {
            byte[] bytesToSend = new byte[1];
            bytesToSend[0] = 26;
            serialPort.Write(bytesToSend, 0, 1);
            Thread.Sleep(200);
        }

        public void Send(string to, Message message)
        {
            PrintLine("AT+CMGF=1");
            Thread.Sleep(100);
            PrintLine("AT+CMGS=\"" + to + "\"");
            Thread.Sleep(100);
            PrintLine(message.ToString());
            Thread.Sleep(100);
            PrintEnd();
            Thread.Sleep(100);
            Debug.Print("SMS Sent!");
        }
    }
}
