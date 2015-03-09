using System;
using System.Collections.Generic;

namespace CSharpSixify
{
    class Program
    {
        static void Main(string[] args)
        {
            var dog = SetupDog();

            Console.WriteLine(String.Format("Meet {0} the dog.", dog.Name));
            Console.WriteLine(String.Format("He is {0} years old.", dog.Age));

            string siblings;
            if (dog != null && dog.Mother != null && dog.Mother.Children != null)
            {
                siblings = (dog.Mother.Children.Count - 1).ToString();
            }
            else
            {
                siblings = "unknown";
            }

            Console.WriteLine(String.Format("He has {0} siblings.", siblings));
            Console.WriteLine("Let's give him some food!");

            try
            {
                dog.GiveFood("salad");
            }
            catch(ArgumentException e)
            {
                if(e.ParamName == "food")
                {
                    Console.WriteLine("Ooops! Dogs doesn't like salad.");
                }
                else
                {
                    Console.WriteLine("Something unexpected happened with the dog!");
                }
            }
            catch
            {
                Console.WriteLine("Something unexpected happened with the dog!");
            }

            Console.Read();
        }

        private static Dog SetupDog()
        {
            var mother = new Dog();
            mother.DateOfBirth = new DateTime(1987, 3, 25);

            var puppy1 = new Dog();
            puppy1.Name = "Fido";
            puppy1.DateOfBirth = new DateTime(1999, 4, 1);
            puppy1.Mother = mother;

            var puppy2 = new Dog();
            puppy2.Name = "Coco";
            puppy2.DateOfBirth = new DateTime(2004, 11, 12);
            puppy2.Mother = mother;

            var puppy3 = new Dog();
            puppy3.Name = "Max";
            puppy3.DateOfBirth = new DateTime(2001, 10, 10);
            puppy3.Mother = mother;

            mother.Children = new Dictionary<string, Dog>
            {
                { puppy1.Name, puppy1 },
                { puppy2.Name, puppy2 },
                { puppy3.Name, puppy3 }
            };

            return puppy1;
        }
    }
}
