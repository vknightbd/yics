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
            Scalar str1 = new Scalar("Yo momma");
            Console.WriteLine(str1);
            Console.WriteLine(str1.Tag);
            Console.WriteLine();

            Scalar str2 = new Scalar("Hello World");
            Scalar str3 = new Scalar("Fee Fi Fo Fum");

            Sequence seq = new Sequence() { str1, str2, str3 };
            Console.WriteLine(seq);
            Console.WriteLine(seq.Tag);
            Console.WriteLine();

            Mapping map = new Mapping() { { seq, str1 }, { str2, seq } };
            Console.WriteLine(map);
            Console.WriteLine(map.Tag);
            Console.WriteLine();

            /* stack overflow
            seq.Add(map);
            Console.WriteLine(seq);
             */

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
