/******************************************************************************
 * Filename    = Program.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = IIT-Samples
 * 
 * Project     = ThreadingSample
 *
 * Description = Defines the Program class that tests the threading sample.
 *****************************************************************************/

using System;

namespace ThreadingSample
{
    /// <summary>
    /// The Executive class.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main entry point for the function.
        /// </summary>
        /// <param name="_">The command line arguments.</param>
        static void Main(string[] _)
        {
            try
            {
                // Run the directory crawler on the various directories.
                DirectoryCrawler crawler = new();
                string[] dirs = new string[]
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    Environment.GetFolderPath(Environment.SpecialFolder.System),
                };

                crawler.CrawlToFindLargestFiles(dirs);

                Console.WriteLine("Press <Return> key to continue");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
