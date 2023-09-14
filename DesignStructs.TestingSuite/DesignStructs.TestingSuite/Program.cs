using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using OpenSTAADUI;

using ReInvented.Shared.Extensions;

namespace ReInvented.TestingSuite
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Thread.Sleep(TimeSpan.FromSeconds(8));
            watch.Stop();

            Console.WriteLine($"{watch.Elapsed.FormatTime()}");
            Console.ReadLine();
        }

    }
}
