using ESLifyEverythingGlobalDataLibrary;
using System.Text.Json;

namespace Test
{
    public class Program
    {
        public static string TestJsonOutputPath = "TestJson\\";

        public static JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World! Test");
            GF.GetStringResources();
            Directory.CreateDirectory(TestJsonOutputPath);

            TestESLifyEverything.TestOBodyESLify();
        }
    }
}