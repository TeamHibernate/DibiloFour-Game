using DibiloFour.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DibiloFour.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            DibiloFourContext context = new DibiloFourContext();

            context.Chests.Count();
        }
    }
}
