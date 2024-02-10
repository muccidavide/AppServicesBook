using System.Diagnostics;
using static System.Diagnostics.Process;
using static System.Console;

namespace MonitoringLib
{
    /*
        The Start method of the Recorder class uses the GC type (garbage collector) 
        to ensure that any currently allocated but not referenced memory is collected 
        before recording the amount of used memory. This is an advanced technique that you should almost never use in application code, 
        because the thr garbage collector understands memory usage better than a programmer would and 
        should be trusted to make decisions about when to collect unused memory itself.
        Our need to take control in this scenario is exceptional.
     */
    public class Recorder
    {
        private static Stopwatch timer = new();

        private static long bytesPhysicalBefore = 0;
        private static long bytesVirtualBefore = 0;

        public static void Start()
        {
            // force some garbage collections to release memory that is
            // no longer referenced but has not been released yet
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // store the current physical and virtual memory use 
            bytesPhysicalBefore = GetCurrentProcess().WorkingSet64;
            bytesVirtualBefore = GetCurrentProcess().VirtualMemorySize64;

            timer.Restart();
        }

        public static void Stop()
        {
            timer.Stop();

            long bytesPhysicalAfter = GetCurrentProcess().WorkingSet64;

            long bytesVirtualAfter = GetCurrentProcess().VirtualMemorySize64;

            WriteLine("{0:N0} physical bytes used.",
                bytesPhysicalAfter - bytesPhysicalBefore);

            WriteLine("{0:N0} virtual bytes used.",
              bytesVirtualAfter - bytesVirtualBefore);

            WriteLine("{0} time span elapsed.", timer.Elapsed);

            WriteLine("{0:N0} total milliseconds elapsed.",
              timer.ElapsedMilliseconds);
        }

    }
}
