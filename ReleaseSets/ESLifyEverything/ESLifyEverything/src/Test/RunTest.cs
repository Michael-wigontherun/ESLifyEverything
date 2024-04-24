using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything.Test
{
    public static class RunTest
    {
        public static void Run()
        {
            Directory.CreateDirectory(TestMethods.TestOutputPath);

            TestMethods.TestCustomSkillsFrameWorkReader();
            TestMethods.TestOBodyESLify();
        }
    }
}
