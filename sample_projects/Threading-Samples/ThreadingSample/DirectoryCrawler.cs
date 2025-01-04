/******************************************************************************
 * Filename    = DirectoryCrawler.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = IIT-Samples
 * 
 * Project     = ThreadingSample
 *
 * Description = Defines the DirectoryCrawler class that searches directories to compare files.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ThreadingSample
{
    /// <summary>
    /// Searches directories to compare files.
    /// </summary>
    internal class DirectoryCrawler
    {
        readonly object _lock;              // Lock for synchronization.
        Dictionary<string, bool> _tasksMap; // Map for task versus is-completed result.

        /// <summary>
        /// Creates an instance of the directory crawler class.
        /// </summary>
        public DirectoryCrawler()
        {
            _lock = new();
            _tasksMap = new();
        }

        /// <summary>
        /// Gets the largest file in the given directory and its sub-directories.
        /// </summary>
        /// <param name="dir">The directory to search for.</param>
        /// <returns>The largest file in the given directory and its sub-directories.</returns>
        private string GetLargestFile(string dir)
        {
            // Add the given directory to the map.
            lock (_lock)
            {
                if (_tasksMap.ContainsKey(dir))
                {
                    _tasksMap[dir] = false;
                }
                else
                {
                    _tasksMap.Add(dir, false);
                }
            }

            int workerThreadId = Environment.CurrentManagedThreadId;

            Console.WriteLine($"Worker Thread with Id = {workerThreadId} on {dir} starting at {DateTime.Now.ToLongTimeString()}");

            string big = string.Empty;
            long size = 0;
            var files = Directory.EnumerateFiles(dir, "*", new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            });

            // Find the largest file.
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                if (info.Length > size)
                {
                    size = info.Length;
                    big = file;
                }
            }

            Console.WriteLine($"Worker Thread with Id = {workerThreadId} on {dir} ending at {DateTime.Now.ToLongTimeString()}");

            // Update the given directory entry in the map.
            lock (_lock)
            {
                _tasksMap[dir] = true;
            }

            return big;
        }

        /// <summary>
        /// Crawls the given directories to find the largest file in each.
        /// </summary>
        /// <param name="dirs"></param>
        public void CrawlToFindLargestFiles(string[] dirs)
        {
            int mainThreadId = Environment.CurrentManagedThreadId;
            Console.WriteLine($"Main Thread with Id = {mainThreadId} starting at {DateTime.Now.ToLongTimeString()} with {dirs.Length} dirs to crawl");

            // Create an async task each for crawling every directory.
            List<Task<string>> tasks = new();
            foreach (string dir in dirs)
            {
                Task<string> task = Task.Run(() => GetLargestFile(dir));
                tasks.Add(task);
            }

            // Wait for all the tasks to complete.
            // TO DO: See if you can do better here by printing the results soon as each task finishes.
            foreach (Task<string> task in tasks)
            {
                task.Wait();
                string file = task.Result;
                FileInfo info = new FileInfo(file);
                Console.WriteLine($"Largest file found by one of the tasks is {info.FullName} with size: {(double)info.Length / (1024.0 * 1024.0)} MB");
            }

            lock (_tasksMap)
            {
                foreach (string dir in _tasksMap.Keys)
                {
                    Console.WriteLine($"Task on {dir} completed? {_tasksMap[dir]}");
                }
            }
        }
    }
}
