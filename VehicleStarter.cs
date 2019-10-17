using System;

namespace ConsoleApp_Assignment2
{
    class VehicleStarter
    {
        static void Main(string[] args)
        {
            Vehicle v = new Vehicle("Ford", "T812", 2014);

            // Vehicle sample distance
            v.addFuel(new Random().NextDouble() * 10, 1.3);

            v.printDetails();
            Console.WriteLine("\n\n");
            Console.ReadLine();
        }
    }
}
