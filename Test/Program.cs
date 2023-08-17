using ESLifyEverythingGlobalDataLibrary;
using System.Text.Json;

namespace Test
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World! Test");
            GF.GetStringResources();
            Directory.CreateDirectory(TestESLifyEverything.TestOutputPath);

            TestESLifyEverything.TestOBodyESLify();
        }
    }
}