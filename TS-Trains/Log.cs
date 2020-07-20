using System;
using CitizenFX.Core;

namespace OldWestRPClient.Utilities
{
    public static class Log
    {
        public static void Success(string msg) => Debug.WriteLine($"^5[{GetLogTimeStamp()}]^2[LOG/Success] {msg}");

        public static void ToChat(string msg)
        {
            BaseScript.TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                args = new[] { "[DEBUG]", $"{msg}" }
            });
        }

        public static void Error(string msg) => Debug.WriteLine($"^5[{GetLogTimeStamp()}]^1 [LOG/Error] {msg}");

        public static void Info(string msg) => Debug.WriteLine($"^5[{GetLogTimeStamp()}]^5 [LOG/Info] {msg}");

        public static void Warn(string msg) => Debug.WriteLine($"^5[{GetLogTimeStamp()}]^3 [LOG/Warn] {msg}");

        private static string GetLogTimeStamp() => DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss:ffff");
    }
}

