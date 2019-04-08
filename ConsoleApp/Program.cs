using DAL.JecaestevezApp;
using System;

namespace ConsoleApp.Jecaestevez
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var context = new EfDbContext())
            {
            }
        }
    }
}
