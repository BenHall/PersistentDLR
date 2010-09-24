using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistentDlr.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting...");
            Agent agent = new Agent();
            agent.Start();

            System.Console.WriteLine("Started");

            System.Console.ReadLine();
            agent.Stop();
        }
    }
}
