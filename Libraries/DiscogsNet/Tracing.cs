using System;

namespace DiscogsNet.Utils
{
    public static class Tracing
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        //[System.Diagnostics.Conditional("Trace")]
        private static void SetupDebugConsole()
        {
            if (!AttachConsole(-1))  // Attach to a parent process console
                AllocConsole(); // Alloc a new console

            _process = System.Diagnostics.Process.GetCurrentProcess();
            System.Console.WriteLine();
            _initialized = true;
        }

        [System.Diagnostics.Conditional("Trace")]
        public static void Trace(string format, params object[] args)
        {
            if (!_initialized)
            {
                SetupDebugConsole();
            }

            System.Console.Write("{0:D5} ", _process.Id);
            System.Console.WriteLine(format, args);
        }

        private static System.Diagnostics.Process _process;
        private static bool _initialized = false;

    }
}