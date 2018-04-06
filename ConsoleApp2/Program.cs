using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public static class StringExtension
    {
        public static string Move(this string str)
        {
            return str + "1";
        }
    }
    class Program
    {

        static void Main(string[] args)
        {

            string a = "hello";
            string b = "hello";
            Console.WriteLine(a.GetHashCode());
            Console.WriteLine(b.GetHashCode());

            try
            {
                List<int> vs = new List<int>();
                List<int> ais = new List<int>();
                vs = ais ?? throw new Exception("Divided by zero");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            switch (Console.ReadLine())
            {
                case String u when (u.Move().Last().ToString() == "1"):
                    Console.WriteLine("daw");
                    break;
            }
            var data = SplitName("hello world");
            Console.WriteLine(data.second_name);
        }
        public static (string first_name, string second_name) SplitName(string main)
        {
            string[] arr = main.Split(' ');
            return (arr[0], arr[1]);
        }
    }
    class User
    {
        ~User()
        {

            Console.WriteLine("Deleted");
        }
        public int Age { get; set; }
        public int Method()
        {


            return Sum(1, 5);
            int Sum(int a, int b) => a + b;
        }
        public int OutMethod(int a, out int b)
        {
            b = 2;

            return b + a;
        }
    }
}
