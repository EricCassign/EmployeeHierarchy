using System;
using System.Collections.Generic;
using System.IO;

namespace EmpHierarchy
{
    public class Program
    {
        public Program(string csv)
        {
            var employees = new List<Employee>();
            var reader = new StreamReader(File.OpenRead(@"" + csv));
            var ceos = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var lineData = line.Split(",");
                if (lineData.Length != 2) continue;

                var empId = lineData[0];
                var manId = lineData[1];
                if (string.IsNullOrWhiteSpace(manId))
                    ceos += 1;

                if (ceos > 1)
                {
                    Console.WriteLine("Maximum number of CEOs exceeded");
                    continue;
                }

                var isInt = int.TryParse(lineData[2], out var salary);
                if (!isInt)
                {
                    Console.WriteLine($"Salary filed for {empId} is invalid");
                    continue;
                }

                var employee = new Employee
                {
                    EmployeeId = empId,
                    ManagerId = manId,
                    Salary = salary
                };
                employees.Add(employee);
            }

        }
    }
}