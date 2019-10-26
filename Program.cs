using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;

// Mary Fallon 10/22/19  This program is used for Blogging - the user can display the blogs/enter a blog/enter a post

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string choice = "";
            char selection;

            do
            {
                var menu = new Menu();  // display the menu and return the selection
                selection = menu.GetInput();
                if (selection.Equals('4'))
                {
                    break;
                }
                choice = selection.ToString();
                logger.Info("User choice: {Choice}", choice);


                if (choice == "1")
                {
                    ProcessChoice.DisplayBlogs(); // display the blogs
                }


                else if (choice == "2")
                {
                    ProcessChoice.EnterBlogs(); // Enter a new blog
                }


                else if (choice == "3")
                {
                    ProcessChoice.EnterPosts();  // Enter a post
                }


            } while (choice == "1" || choice == "2" || choice == "3");
            logger.Info("Program ended");
        }
    }
}
