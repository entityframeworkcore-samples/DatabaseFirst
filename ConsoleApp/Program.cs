using DAL.JecaestevezApp.Models;
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
                var Item = new Item()
                {
                    Name = "Ron Palido",
                    Description = "Drink",
                    Expiration = DateTime.Now.AddYears(1)

                };
                Console.WriteLine($"Item NOT saved -> Id {Item.Id} {Item.Name} {Item.Expiration}");

                context.Add(Item);
                context.SaveChanges();

                Console.WriteLine($"Item saved -> Id {Item.Id} {Item.Name} {Item.Expiration}");
                Console.ReadKey();
            }
        }
    }
}
