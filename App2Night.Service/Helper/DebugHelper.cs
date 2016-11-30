using System;
using System.Diagnostics;

namespace App2Night.Service.Helper
{
    /// <summary>
    /// Helper class to print debug information in a well formatted way.
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// Print a stopwatch time.
        /// </summary>
        /// <param name="stopwatch">A running <see cref="Stopwatch"/></param>
        /// <param name="taskDescription">A description of the task.</param>
        public static void PrintTime(this Stopwatch stopwatch, string taskDescription)
        {
            PrintDebug(DebugType.Time, $"{taskDescription}. Took {stopwatch.ElapsedMilliseconds} ms."); 
        }
         
        /// <summary>
        /// Prints a debug message, consisting of a header and a message.
        /// </summary>
        /// <param name="debugType">Will be shown as header.</param>
        /// <param name="description">Will be shown as description.</param>
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

    /// <summary>
    /// Used for <see cref="DebugHelper.PrintDebug"/>
    /// </summary>
    public enum DebugType
    {
        Error,
        Info,
        Time
    }
}