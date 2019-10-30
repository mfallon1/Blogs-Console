using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
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

        //var query;
        static string s;
        internal static void DisplayBlogs()
        {
            Console.WriteLine("** Here are your blogs:\n");     // Display all Blogs from the database
            using (var db = new BloggingContext())              // this is a connection to the db
            {
                var query = db.Blogs.OrderBy(b => b.Name);
                Console.WriteLine("You have " + query.Count() + " Blogs");
                foreach (var item in query)
                {
                    Console.WriteLine("\t" + item.Name);
                }
                Console.WriteLine("\t" + "Press ENTER to continue");
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

        public static void DisplayPosts()
        {
            Console.WriteLine("Which Posts would you like to Display?");
            using (var db = new BloggingContext()) // this is a connection to the db
            {
                Console.WriteLine($"   0) Posts from all blogs");

                {
                    var query = db.Blogs.OrderBy(b => b.BlogId);
                    foreach (var item in query)
                    {
                        Console.WriteLine($"   {item.BlogId}) Posts from {item.Name}");

                    }
                }

                try
                {
                    int bchoice = Int32.Parse(Console.ReadLine());

                    if (bchoice == 0) // all posts
                    {
                        var cquery = db.Posts;
                        Console.WriteLine($" {cquery.Count()} Post(s) returned\n");

                        var query = db.Blogs.
                                     Join(db.Posts, bl => bl.BlogId, pos => pos.BlogId,
                                     (bl, pos) => new { bl.BlogId, bl.Name }).Distinct(); 

                        foreach (var item in query)  // display blog name and the posts under it
                        {
                            Console.WriteLine($"Blog: {item.Name}\n");
                            GetPost(item.BlogId);
                        }
                    }

                    else if(bchoice != 0)
                    { 
                        var exist = db.Blogs.Any(c => c.BlogId == bchoice);   // verify blogid entered by checking the above list of valid BlogId's it will only 
                        //var bc = blogList.Where(c => BlogId == bchoice).SingleOrDefault();
                        if (!exist)                                                 // be null if the ID didn't exist
                        {
                            Console.WriteLine($"Not a valid ID Press ENTER to continue");
                            logger.Info("invalid BlogID entered");
                        }

                        else if (exist)                        // good blog choice  Prompt for Post Information
                        {
                            var cquery = db.Posts.Where(p => p.BlogId == bchoice).Count();
                            Console.WriteLine($" {cquery} Post(s) returned\n");
                            if (cquery != 0)
                            { 
                                var query = db.Blogs.
                                         Join(db.Posts, bl => bl.BlogId, pos => pos.BlogId,
                                         (bl, pos) => new { bl.BlogId, bl.Name }).Distinct();

                                foreach (var item in query)  // display blog name and the posts under it
                                {
                                Console.WriteLine($"Blog: {item.Name}\n");
                                GetPost(item.BlogId);
                                }
                            }
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine($"ID must be a number Press ENTER to continue");
                }
          //      Console.ReadLine();
            }
        }

        private static void GetPost(int Bid)
        {
            using (var db = new BloggingContext()) // this is a connection to the db
                //var query = (from bl in db.Blogs
                //             join pos in db.Posts on bl.BlogId equals pos.BlogId
                //             select new { Bid = bl.BlogId, Nm = bl.Name });
            {
                var postquery = (from bl in db.Blogs
                                 join pos in db.Posts on bl.BlogId equals pos.BlogId
                                 where bl.BlogId == Bid
                                 select new { pTtl = pos.Title, pContent = pos.Content });
                //select new {p.Title, p.Content };
                foreach (var pitem in postquery)
                {
                    Console.WriteLine($"   Title: {pitem.pTtl}");
                    Console.WriteLine($"   Content: {pitem.pContent}\n");
                }
            }
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
                    // Console.WriteLine();
                    //var query = db.Blogs.Select(b => new { ID = SqlFunctions.StringConvert((double)b.BlogId),    b.Name }).OrderBy(i => i).Count(); // first show the blogs
                    var blogList = db.Blogs.Select(b=>b).ToList();
                    foreach (var item in blogList)
                    {
                    //    Console.WriteLine("\t" + item.BlogId + ") " + item.Name);
                        Console.WriteLine($"   {item.BlogId})  {item.Name}");
                    }
                   // Console.ReadLine();
                    //OutputResults(query); // write to the ALL.csv from the query

                    //Console.WriteLine();
                    //var query = db.Blogs.Select(b => new { ID = b.BlogId,    b.Name }).OrderBy(i => i).ToList(); // first show the blogs
                    //foreach (var item in query)
                    //{
                    //    Console.WriteLine(item);
                    //}

                    do
                    {
                        Console.WriteLine("Enter the BlogId for the new POST");     // choose the blog for the post
                        try
                        {
                            int bchoice = Int32.Parse(Console.ReadLine());
                            var bc = blogList.Where(c => c.BlogId == bchoice).SingleOrDefault();   // verify blogid entered by checking the above list of valid BlogId's it will only 
  //                          var bc = blogList.Where(c => BlogId == bchoice).SingleOrDefault();
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

                                //var post = new Post { Title = title, Content = content, BlogId = bc.ID };
                                var post = new Post { Title = title, Content = content, BlogId = bc.BlogId };

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
