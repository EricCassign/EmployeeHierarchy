using System.Collections.Generic;
using NUnit.Framework;

namespace EmpHierarchy.Tests
{
    public class ProgramTests
    {
        private readonly EmpHierarchyProgram _program;
        private List<Employee> employees;
        public ProgramTests(string csv)
        {
            _program = new EmpHierarchyProgram(csv);
        }

        [SetUp]
        public void Setup()
        {
            employees = _program.ValidEmployees;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
