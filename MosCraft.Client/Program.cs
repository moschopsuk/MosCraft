using System;
using System.Collections.Generic;
using System.Linq;

namespace MosCraft.Client
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Client())
                game.Run(30.0);
        }
    }
}
