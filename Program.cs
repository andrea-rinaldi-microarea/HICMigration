using System;
using System.Linq;
using HICMigration.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HICMigration
{
    class Options
    {
        public bool all = false;
        public string singlePage = string.Empty;
        public string textFile = string.Empty;
    }

    class Program
    {
        static public IConfiguration configuration;
        static Options options = new Options();
        static MicroareaWikiContext context = null;

        static void LoadConfiguration()
        {
            Program.configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json")
                .Build();    
        }

        static bool readOptions(string[] args)
        {
            for (int a = 0; a < args.Length; a++ )
            {
                if (args[a] == "-all")
                    options.all = true;
                else if (args[a] == "-single")
                    options.singlePage = args[++a];
                else if (args[a] == "-file")
                    options.textFile = args[++a];
                else
                {
                    Console.WriteLine("Bad parameters");
                    return false;                     
                }
            }
            return true;
        }

        static void readSingle(string pageName) {
            var page = context.PageContent.Where(page => page.Page == pageName).ToList();
            Console.WriteLine(Formatter.Format(page.First().Content));
        }

        static void readFile(string fileName) {
            string pathName = Path.Combine(Directory.GetCurrentDirectory(),$"{fileName}");
            if (!System.IO.File.Exists(pathName))
                return ;
            using(TextReader reader = new StreamReader(pathName)) {
                var content = reader.ReadToEnd();
                Console.WriteLine(Formatter.Format(content));
            }            
        }

        static void readAll() {
            var pages = context.PageContent.ToList();
            // foreach (var page in pages)
            // {
            //     Console.WriteLine(page.Title.ToString());
            // } 
        }

        static int Main(string[] args)
        {
            LoadConfiguration();
            if (!readOptions(args))
                return -1;
            context = new MicroareaWikiContext(configuration);

            if (options.singlePage != string.Empty) readSingle(options.singlePage);

            if (options.textFile != string.Empty) readFile(options.textFile);

            if (options.all) readAll();

            return 0;
        }
    }
}
