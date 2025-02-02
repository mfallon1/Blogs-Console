﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogsConsole.Models;
using NLog;

namespace BlogsConsole
{
    public class ProcessChoice
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static string s;
        internal static void DisplayBlogs()
        {
            Console.WriteLine("** Here are your blogs:\n");     // Display all Blogs from the database
            using (var db = new BloggingContext())              // this is a connection to the db
            {
                var query = db.Blogs.OrderBy(b => b.Name);
                foreach (var item in query)
                {
                    Console.WriteLine("\t" + item.Name);
                }
                Console.ReadLine();
            }

        }

        public static void EnterBlogs()                         // prompt for Blog info
        {
            string name;
            do   
            {
                Console.Write("Enter a name for a new Blog: ");     // Create and save a new Blog
                name = Console.ReadLine();
                s = name;
                if (CustomMethod.IsBlank(s))
                {
                    Console.WriteLine("\t**Must enter something");
                    logger.Info("blank Blog entered");
                }
            }
            while (CustomMethod.IsBlank(s));

            var blog = new Blog { Name = name };

            var db = new BloggingContext(); // this is a connection to the db

            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
        }

        public static void EnterPosts()        // create Post
        {
            int selcount = 0;
            string content;
            string title;

            do
            {
                Console.WriteLine("Which Blog would you like to create a POST in?");      
                using (var db = new BloggingContext()) // this is a connection to the db
                {
                    Console.WriteLine();
                    var query = db.Blogs.Select(b => new { ID = b.BlogId,    b.Name }).OrderBy(i => i).ToList(); // first show the blogs
                    foreach (var item in query)
                    {
                        Console.WriteLine(item);
                    }

                    do
                    {
                        Console.WriteLine("Enter the BlogId for the new POST");     // choose the blog for the post
                        try
                        {
                            int bchoice = Int32.Parse(Console.ReadLine());
                            var bc = query.Where(c => c.ID == bchoice).SingleOrDefault();   // verify blogid entered by checking the above list of valid BlogId's it will only 
                            if (bc == null)                                                 // be null if the ID didn't exist
                            {
                                Console.WriteLine($"Not a valid ID Press ENTER to continue");
                                logger.Info("invalid BlogID entered");
                            }

                            else if (bc != null)                        // good blog choice  Prompt for Post Information
                            {
                                do
                                {
                                    Console.Write("Enter a TITLE for a new POST: ");
                                    title = Console.ReadLine();
                                    s = title;
                                    if (CustomMethod.IsBlank(s))
                                    {
                                        Console.WriteLine("\t**Must enter something");
                                        logger.Info("blank post title entered");
                                    }
                                }
                                while (CustomMethod.IsBlank(s));

                                do
                                {
                                    Console.Write("Enter post CONTENT: ");
                                    content = Console.ReadLine();
                                    s = content;
                                    if (CustomMethod.IsBlank(s))
                                    {
                                        Console.WriteLine("\t**Must enter something");
                                        logger.Info("blank content entered");
                                    }
                                }
                                while (CustomMethod.IsBlank(s));

                                var post = new Post { Title = title, Content = content, BlogId = bc.ID };
                                var dbase = new BloggingContext(); // this is a connection to the db

                                dbase.AddPost(post);
                                logger.Info("Post added - {Title}", title);
                            }
                                logger.Info("Blog choice: {BChoice}", bchoice);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"ID must be a number Press ENTER to continue");
                        }

                    }
                    while (CustomMethod.IsBlank(s));
                    break;

 //                   Console.ReadLine();
                }
            } while (selcount == 0);
        

    }
}
}
