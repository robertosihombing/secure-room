using System;
using Microsoft.SPOT;
using System.Net;
using System.Net.Sockets;

namespace SecureRoom.Ntp
{
    /// <summary>
    /// Encapsulates all NTP related code
    /// </summary>
    public static class NtpHelper
    {
        public static bool UpdateTimeFromNtpServer(string server, int timeZoneOffset)
        {
            try
            {
                var currentTime = GetNtpTime(server, timeZoneOffset);
                Microsoft.SPOT.Hardware.Utility.SetLocalTime(currentTime);

                return true;
            }
            // there is no need to log the exception details
            // if the time could be updated it is logged on the upper level
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get DateTime from NTP Server
        /// Based on:
        /// http://weblogs.asp.net/mschwarz/archive/2008/03/09/wrong-datetime-on-net-micro-framework-devices.aspx
        /// </summary>
        /// <param name="timeServer">Time Server (NTP) address</param>
        /// <param name="timeZoneOffset">Difference in hours from UTC</param>
        /// <returns>Local NTP Time</returns>
        private static DateTime GetNtpTime(String timeServer, int timeZoneOffset)
        {
            // Find endpoint for TimeServer
            var timerServerEndPoint = new IPEndPoint(Dns.GetHostEntry(timeServer).AddressList[0], 123);

            // Make send/receive buffer
            var ntpData = new byte[48];

            // Connect to TimeServer
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                // Set 10s send/receive timeout and connect
                socket.SendTimeout = socket.ReceiveTimeout = 10000; // 10,000 ms
                socket.Connect(timerServerEndPoint);

                // Set protocol version
                ntpData[0] = 0x1B;

                socket.Send(ntpData);

                // Receive Time
                socket.Receive(ntpData);

                // Close the socket
                socket.Close();
            }

            const byte offsetTransmitTime = 40;

            ulong intpart = 0;
            ulong fractpart = 0;

            for (int i = 0; i <= 3; i++)
            { 
                intpart = (intpart << 8) | ntpData[offsetTransmitTime + i]; 
            }

            for (int i = 4; i <= 7; i++)
            { 
                fractpart = (fractpart << 8) | ntpData[offsetTransmitTime + i]; 
            }

            ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);

            var timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);
            var dateTime = new DateTime(1900, 1, 1);
            dateTime += timeSpan;

            var offsetAmount = new TimeSpan(timeZoneOffset, 0, 0);
            var networkDateTime = (dateTime + offsetAmount);

            return networkDateTime;
        }
    }
}
