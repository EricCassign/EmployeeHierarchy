using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace EmpHierarchy.Tests
{
    public class ProgramTests
    {
        private readonly EmpHierarchyProgram _program;
        private List<Employee> _employees;
        public ProgramTests(string csv)
        {
            _program = new EmpHierarchyProgram(csv);
        }

        [SetUp]
        public void Setup()
        {
            _employees = _program.ValidEmployees;
        }

        [Test]
        public void Salaries_are_valid_integer_numbers()
        {
            var count = _employees.Count(e => !IsValidInteger(e.Salary.ToString()));
            Assert.Equals(count, 0);
        }

        [Test]
        public void Only_one_ceo()
        {
            var count = _employees.Count(e => string.IsNullOrWhiteSpace(e.ManagerId));
            Assert.Equals(count, 1);
        }

        [Test]
        public void All_managers_are_employees()
        {
            var managers = _employees.Select(e => e.ManagerId).ToList();
            var count = _employees.Count(e => _employees.Any(l => managers.Contains(e.EmployeeId)));
            Assert.Equals(count, managers.Count);
        }

        [Test]
        public void Employee_reports_to_one_manager()
        {
            var duplicates = _employees
                .GroupBy(e => e.EmployeeId)
                .Where(g => g.Count() > 1)
                .ToList();
            Assert.Equals(duplicates.Count, 0);
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
