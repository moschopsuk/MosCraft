using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MosCraft.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var client = new Client())
                client.Run(30);
        }
    }
}
