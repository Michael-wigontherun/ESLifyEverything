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
            Directory.CreateDirectory(TestJsonOutputPath);

            TestESLifyEverything.TestCustomSkillsFrameWorkReader();
        }
    }
}