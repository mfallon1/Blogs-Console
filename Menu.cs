using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace BlogsConsole
{
    internal class Menu
    {
        public Menu()
        {
            // display choices to user
            Console.WriteLine();
            Console.WriteLine("WELCOME to your Blog - What would you like to do?");
            Console.WriteLine();
            Console.WriteLine("1) Display All Blogs");
            Console.WriteLine("2) Add Blog");
            Console.WriteLine("3) Create Post");
            Console.WriteLine("4) Display Posts");
            Console.WriteLine("E) Exit");
            Console.WriteLine();

            // input selection
        }

        public char GetInput()
        {
            char selection;

            while (!IsSelValid(Console.ReadKey(true).KeyChar, out selection))
            {

                Console.WriteLine($"Invalid input: {selection}");
                Console.WriteLine();
                Console.WriteLine("Please enter (1) Display All Blogs, (2)Add Blog, (3)Create Post, (4) Display Post, (E) Exit");
                Console.Write("");
            }

            Console.WriteLine();
            return selection;

        }

        private bool IsSelValid(char input, out char selection) //validation
        {
            char[] validValues = { '1', '2', '3', '4','E','e' };
            selection = input;
            if (validValues.Contains(input))
            {
                return true;
            }

            return false;
        }

    }
}