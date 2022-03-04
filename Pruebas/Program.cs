using System;
using System.Collections.Generic;
using System.IO;

namespace Pruebas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string docPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName);
            Console.WriteLine(docPath);
            string cancion;

            List<string> dirs = new List<String>(Directory.EnumerateFiles(@"..\..\..\Assets\"));

            foreach (var dir in dirs)
            {
                cancion = $"{dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)}";
                Console.WriteLine($"{dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)}");
            }
        
        }
    }
}
