using System;

namespace FileSystemVisitor
{
    public class MainConsole
    {
        static void Main(string[] args)
        {
            string startingPoint = "D:\\Терри Пратчетт Собрание сочинений";

            var visitor = new FileSystemVisitor(startingPoint, (info) => true);

            visitor.Start += (s, e) => {Console.WriteLine("Start iteration");};

            visitor.Finish += (s, e) => {Console.WriteLine("Finish iteration");};

            visitor.FileFinded += (s, e) => {Console.WriteLine("Finded file: " + e.FindedItem.Name);};

            visitor.FilteredFileFinded += (s, e) => {Console.WriteLine("Finded filtered file: " + e.FindedItem.Name);};

            visitor.DirectoryFinded += (s, e) => {Console.WriteLine("Finded directory: " + e.FindedItem.Name);};

            visitor.FilteredDirectoryFinded += (s, e) => {Console.WriteLine("Finded filtered directory: " + e.FindedItem.Name);};

            foreach (var fileSysInfo in visitor.GetFileInfoSequence())
            {
                Console.WriteLine(fileSysInfo);
                Console.ReadKey();
            }
        }
    }
}
