using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YICS.Representation;
using YICS.Serialization;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Example: Scalar");
            Scalar str1 = new Scalar("Yo momma");
            Console.WriteLine(str1);
            Console.WriteLine(str1.Tag);
            Console.WriteLine();

            Scalar str2 = new Scalar("Hello World");
            Scalar str3 = new Scalar("Fee Fi Fo Fum");

            Console.WriteLine("Example: Sequence");
            Sequence seq = new Sequence() { str1, str2, str3 };
            Console.WriteLine(seq);
            Console.WriteLine(seq.Tag);
            Console.WriteLine(seq.GetHashCode());
            Console.WriteLine();

            Console.WriteLine("Example: Mapping");
            Mapping map = new Mapping() { { seq, str1 }, { str2, seq } };
            Console.WriteLine(map);
            Console.WriteLine(map.Tag);
            Console.WriteLine(map.GetHashCode());
            Mapping map2 = new Mapping() { { seq, str1 }, { str2, seq } };
            Console.WriteLine(map2.GetHashCode()); // hmm... same value, but different hash code?
            Console.WriteLine(map.Equals(map2)); // we get the expected true for same values
            Console.WriteLine();

            Console.WriteLine("Example: Cyclic");
            //seq.Add(map);
            /* &seq
             *   - &str1 Yo momma
             *   - &str2 Hello World
             *   - &str3 Fee Fi Fo Fum
             *   - &map
             *     ? *seq  : *str1
             *     ? *str2 : *seq
             */
            //Console.WriteLine(seq);
            //Console.WriteLine(seq.GetHashCode());
            //Console.WriteLine(map.GetHashCode());

            Sequence seq2 = new Sequence() { "See Saw", "Winter" };
            seq.Add(seq2);
            seq.Add(seq2);
            seq.Add(map);

            Serializer serializer = new Serializer(map);
            serializer.CreateEventTree();
            Console.WriteLine(serializer.GetCharacterStream());

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
