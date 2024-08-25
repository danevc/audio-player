using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(AudioPlayerWCF.AudioPlayerService)))
            {
                host.Open();
                Console.WriteLine("Host Started...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
