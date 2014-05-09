using System;
using Microsoft.SPOT;
using System.IO;
using SecureRoom.Config;

namespace SecureRoom.Logging
{
    /// <summary>
    /// This logger writes both in the file and Debug window
    /// </summary>
    public class CombinedLogger : ILogger
    {
        private string GetLogString(LogLevel level, string message)
        {
            return level.ToString() + ": " + message;
        }

        public void Log(LogLevel level, string message)
        {
            Debug.Print(GetLogString(level, message));
     
            using (var fileStream = new FileStream(Settings.LoggerFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(GetLogString(level, message));
                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }
}
