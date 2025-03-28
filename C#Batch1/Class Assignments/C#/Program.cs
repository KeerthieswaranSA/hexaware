using System;


namespace ClassAssignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program p= new Program();
            equal();
            p.sign();
            p.add();
            tables();
            sum();
        }
        //1. Write a C# Sharp program to accept two integers and check whether they are equal or not.
        static void equal()
        {
            int a, b;
            Console.WriteLine("Enter 1st input:");
            a= Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter 2nd input:");
            b=Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(a == b ? "{0} and {1} are equal" : "{0} and {1} are not equal", a, b);
        }
        //2. Write a C# Sharp program to check whether a given number is positive or negative.
        public void sign()
        {
            Console.WriteLine("Enter the number:");
            int a= Convert.ToInt32(Console.ReadLine());
            if (a >= 0)
                Console.WriteLine("{0} is positive", a);
            else
                Console.WriteLine("{0} is negative", a);
        }
        //3. Write a C# Sharp program that takes two numbers as input and performs
        //all operations (+,-,*,/) on them and displays the result of that operation.
        public void add()
        {
            Console.WriteLine("Enter 1st number:");
            int a= Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter 2nd number:");
            int b= Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("{0}+{1}={2}",a,b,a+b);
            Console.WriteLine("{0}-{1}={2}",a,b,a-b);
            Console.WriteLine("{0}*{1}={2}",a,b,a*b);
            Console.WriteLine("{0}/{1}={2}",a,b,a/b);
        }
        //4. Write a C# Sharp program that prints the multiplication table of a number as input.
        static void tables()
        {
            Console.WriteLine("Enter the number:");
            int a = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < a; i++)
            {
                Console.WriteLine("{0}*{1}={2}", a, i, a * i);
            }
        }
        //5.  Write a C# program to compute the sum of two given integers.
        //If two values are the same, return the triple of their sum.
        static void sum()
        {
            Console.WriteLine("Enter 1st number:");
            int a=Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter 2nd number:");
            int b=Convert.ToInt32(Console.ReadLine());
            if (a == b)
                Console.WriteLine("Result={0}", 3 * (a + b));
            else
                Console.WriteLine("Result={0}", a + b);
        }
    }
}