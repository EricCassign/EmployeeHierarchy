using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmpHierarchy
{
    public class EmpHierarchyProgram
    {
        public List<Employee> ValidEmployees;
        public EmpHierarchyProgram(string csv)
        {

            var employees = Employees(csv);
            if (employees.Any())
            {

                foreach (var employee in employees)
                {
                    if (!ManagerIsEmployee(employees, employee.ManagerId))
                    {
                        Console.WriteLine($"Manager {employee.ManagerId} is not an employee");
                        employees.Remove(employee);
                    }

                    if (HasReportLoop(employees, employee.EmployeeId, employee.ManagerId))
                    {
                        Console.WriteLine($"Reporting loops exists for employee {employee.EmployeeId}");
                        employees.Remove(employee);
                    }
                }
            }

            ValidEmployees = employees;

            var salaryTotal = employees.Sum(e => e.Salary);
            Console.WriteLine($"Total valid employees {employees.Count} with total salary {salaryTotal}");
        }

        public long ManagerBudget(List<Employee> employees, string employeeId)
        {
            return employees.Where(e => e.ManagerId.Equals(employeeId)).Sum(e => e.Salary);
        }

        private string ReportsTo(List<Employee> employees, string employeeId)
        {
            return employees.First(e => e.EmployeeId.Equals(employeeId)).ManagerId;
        }

        private static bool ManagerIsEmployee(List<Employee> employees, string managerId)
        {
            return employees.Any(e => e.EmployeeId.Equals(managerId));
        }

        private static bool HasReportLoop(List<Employee> employees, string employeeId, string managerId)
        {
            var managerReportsTo = employees.First(e => e.EmployeeId.Equals(managerId)).ManagerId;
            return managerReportsTo.Equals(employeeId);
        }

        private static List<Employee> Employees(string fileString)
        {
            var employees = new List<Employee>();
            try
            {
                //var reader = new StreamReader(File.OpenRead(@"" + fileString));
                var reader = new StreamReader(fileString);
                var ceos = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var lineData = line.Split(",");
                    if (lineData.Length != 2) continue;

                    var managerId = lineData[1];
                    if (string.IsNullOrWhiteSpace(managerId))
                        ceos += 1;

                    if (ceos > 1)
                    {
                        Console.WriteLine("Maximum number of CEOs exceeded");
                        continue;
                    }

                    var employeeId = lineData[0];
                    if (employees.Any(e => e.EmployeeId.Equals(employeeId)))
                    {
                        Console.WriteLine($"Employee {employeeId} already added");
                        continue;
                    }

                    var isInt = int.TryParse(lineData[2], out var salary);
                    if (!isInt)
                    {
                        Console.WriteLine($"Salary filed for {employeeId} is invalid");
                        continue;
                    }

                    var employee = new Employee
                    {
                        EmployeeId = employeeId,
                        ManagerId = managerId,
                        Salary = salary
                    };
                    employees.Add(employee);
                }

                return employees;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured: {e.Message}");
                return employees;
            }
        }
    }
}