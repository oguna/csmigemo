using System;
using System.IO;
using System.Reflection;

namespace CsMigemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("CsMigemo.migemo-compact-dict");
            var migemo = new Migemo(stream, RegexOperator.DEFAULT);
            string line;
            while ((line = Console.ReadLine()) != null && line.Length > 0)
            {
                Console.WriteLine(migemo.Query(line));
            }
        }
    }
}
