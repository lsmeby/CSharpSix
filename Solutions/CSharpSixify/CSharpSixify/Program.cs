using System;
using System.Collections.Generic;
using System.Console; // Static using statement

namespace CSharpSixify
{
    class Program
    {
        static void Main(string[] args)
        {
            var dog = SetupDog();
            WriteLine("Meet \{dog.Name} the dog."); // String interpolation and static using statement
            WriteLine("He is \{dog.Age} years old."); // String interpolation and static using statement
            WriteLine("He has \{(dog?.Mother?.Children?.Count - 1)?.ToString() ?? "unknown"} siblings."); // String interpolation, static using statement and conditional access
            WriteLine("Let's give him some food!"); // String interpolation

            try
            {
                dog.GiveFood("salad");
            }
            catch(ArgumentException e) if(e.ParamName == "food") // Exception filter
            {
                WriteLine("Ooops! Dogs doesn't like salad."); // String interpolation and static using statment
            }
            catch
            {
                WriteLine("Something unexpected happened with the dog!"); // String interpolation and static using statement
            }

            Read(); // Static using statement
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

            // Dictionary initializer
            mother.Children = new Dictionary<string, Dog>
            {
                [puppy1.Name] = puppy1,
                [puppy2.Name] = puppy2,
                [puppy3.Name] = puppy3
            };

            return puppy1;
        }
    }
}
