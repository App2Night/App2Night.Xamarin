using System;
using System.Diagnostics;

namespace App2Night.Service.Helper
{
    public static class DebugHelper
    {
        public static void PrintTime(this Stopwatch stopwatch, string taskDescription)
        {
            PrintDebug(DebugType.Time, $"{taskDescription} took {stopwatch.ElapsedMilliseconds} ms."); 
        }

        public static void PrintDebug(DebugType debugType, string description)
        {
            string header = String.Empty;
            switch (debugType)
            {
                case DebugType.Error:
                    header = "ERROR";
                    break;
                case DebugType.Info:
                    header = "INFO"; 
                    break;
                case DebugType.Time:
                    header = "TIMER";
                    break;
            }
            Debug.WriteLine(header+"\n\t" +
                                 description);
        }
    }

    public enum DebugType
    {
        Error,
        Info,
        Time
    }
}