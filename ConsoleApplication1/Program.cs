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
            Sequence seq = new Sequence();

            Scalar str1 = new Scalar("Yo momma");
            Console.WriteLine(str1);
            Console.WriteLine(str1.Tag);
            Console.WriteLine();

            Scalar str2 = new Scalar("Hello World");
            Scalar str3 = new Scalar("Fee Fi Fo Fum");

            seq.Add(str1);
            seq.Add(str2);
            seq.Add(str3);
            Console.WriteLine(seq);
            Console.WriteLine(seq.Tag);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
