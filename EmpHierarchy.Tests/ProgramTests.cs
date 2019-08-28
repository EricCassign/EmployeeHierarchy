using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace EmpHierarchy.Tests
{
    public class ProgramTests
    {
        private readonly string _csvData = "Employee4,Employee2,500\r\nEmployee3,Employee1,800\r\nEmployee1,,1000\r\nEmployee5,Employee1,500\r\nEmployee2,Employee1,500\r\n";

        private readonly EmpHierarchyProgram _program;
        private List<Employee> _employees;
        public ProgramTests()
        {
            _program = new EmpHierarchyProgram(_csvData);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _employees = _program.ValidEmployees;
        }

        [Test]
        public void Salaries_are_valid_integer_numbers()
        {
            var count = _employees.Count(e => !IsValidInteger(e.Salary.ToString()));
            Assert.AreEqual(0, count);
        }

        [Test]
        public void Only_one_ceo()
        {
            var count = _employees.Count(e => string.IsNullOrWhiteSpace(e.ManagerId));
            Assert.AreEqual(1, count);
        }

        [Test]
        public void All_managers_are_employees()
        {
            var managers = _employees.Select(e => e.ManagerId).ToList();
            var count = _employees.Count(e => _employees.Any(l => managers.Contains(e.EmployeeId)));
            Assert.AreEqual(managers.Count, count);
        }

        [Test]
        public void Employee_reports_to_one_manager()
        {
            var duplicates = _employees
                .GroupBy(e => e.EmployeeId)
                .Where(g => g.Count() > 1)
                .ToList();
            Assert.AreEqual(0, duplicates.Count);
        }

        #region HelperMethods

        private static bool IsValidInteger(string number)
        {
            var isValid = int.TryParse(number, out var validResult);
            Console.WriteLine($"input: {number} output: {validResult} ");
            return isValid;
        }


        #endregion
    }
}
