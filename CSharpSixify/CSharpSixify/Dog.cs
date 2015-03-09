using System;
using System.Collections.Generic;

namespace CSharpSixify
{
    class Dog
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                return new DateTime(DateTime.Now.Subtract(DateOfBirth).Ticks).Year - 1;
            }
        }

        public Dog Mother { get; set; }
        public Dictionary<string, Dog> Children { get; set; }

        public Dog()
        {
            Name = "Lassie";
        }

        public void GiveFood(string food)
        {
            if(food == "salad")
            {
                throw new ArgumentException(String.Format("The dog doesn't like {0}", food), "food");
            }
        }
    }
}
