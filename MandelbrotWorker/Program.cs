using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelbrotWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 3)
                {
                    Application app = new Application(args[0], args[1], args[2]);
                    app.Start();
                }
                else if (args.Length == 0)
                {
                    Console.WriteLine("Type in server ip address:");
                    var ip = Console.ReadLine();
                    Console.WriteLine("Type in server push port:");
                    var push = Console.ReadLine();
                    Console.WriteLine("Type in server pull port:");
                    var pull = Console.ReadLine();

                    Application app = new Application(ip, push, pull);
                    app.Start();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong arguments! The following parameters are needed: ip address, push port, pull port. \n" +
                        "If you are not sure, please start the application without any parameters.");
            }
            
        }
    }
}
