using System;
using System.Collections.Generic;

namespace CSharpSixify
{
    class Dog
    {
        public string Name { get; set; } = "Lassie"; // Auto property initializer
        public DateTime DateOfBirth { get; set; }
        public int Age => new DateTime(DateTime.Now.Subtract(DateOfBirth).Ticks).Year - 1; // Expression bodied member
        public Dog Mother { get; set; }
        public Dictionary<string, Dog> Children { get; set; }

        public void GiveFood(string food)
        {
            if(food == "salad")
            {
                throw new ArgumentException("The dog doesn't like \{food}", nameof(food)); // String interpolation and nameof expression
            }
        }
    }
}
