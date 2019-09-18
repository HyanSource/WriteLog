using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyan.IO;

namespace WriteLog
{
    class Program
    {
        static void Main(string[] args)
        {
            LogIO.GetInstance().Start();

            Console.ReadKey();
        }
    }
}
