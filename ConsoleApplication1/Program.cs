using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YICS.Representation;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Scalar s = new Scalar("Yo momma");
            Console.WriteLine(s);
            Console.WriteLine(s.Tag);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
