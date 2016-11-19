using System.Diagnostics;

namespace App2Night.Service.Helper
{
    public static class DebugHelper
    {
        public static void PrintTime(this Stopwatch stopwatch, string taskDescription)
        {
            Debug.WriteLine("TIMER\n\t" +
                                 $"{taskDescription} took { stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}