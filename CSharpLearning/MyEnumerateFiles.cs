// https://docs.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-5.0

namespace CSharpLearning
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    public class MyEnumerateFiles
    {
        public static void main()
        {
            string sourceDirectory = @"C:\Users\t-caitaozhan\source\repos\CSharpLearning\CSharpLearning\files\source";
            string archiveDirectory = @"C:\Users\t-caitaozhan\source\repos\CSharpLearning\CSharpLearning\files\destination";

            try
            {
                var txtFiles = Directory.EnumerateFiles(sourceDirectory, "*.txt");
                foreach (string currentFile in txtFiles)
                {
                    string fileName = currentFile.Substring(sourceDirectory.Length + 1);
                    Directory.Move(currentFile, Path.Combine(archiveDirectory, fileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void enumerable()
        {
            string sourceDirectory = @"C:\Users\t-caitaozhan\source\repos\CSharpLearning\CSharpLearning\files\source";
            var fileEnumerable = Directory.EnumerateFiles(sourceDirectory, "*.txt");
            foreach (var file in fileEnumerable)
            {
                Console.WriteLine(file);
            }
        }

        public static void enumerator()
        {
            string sourceDirectory = @"C:\Users\t-caitaozhan\source\repos\CSharpLearning\CSharpLearning\files\source";
            var fileEnumerator = Directory.EnumerateFiles(sourceDirectory, "*.txt").GetEnumerator();
            Console.WriteLine(fileEnumerator.Current);
            fileEnumerator.MoveNext();
            Console.WriteLine(fileEnumerator.Current);
            fileEnumerator.MoveNext();
            Console.WriteLine(fileEnumerator.Current);
            fileEnumerator.MoveNext();
            Console.WriteLine(fileEnumerator.Current);
            fileEnumerator.MoveNext();
            Console.WriteLine(fileEnumerator.Current);
        }
    }


}
