using System;

namespace Experiment2
{
    // Interface
    interface IPayroll
    {
        decimal CalculateSalary();
    }

    // Abstract Base Class
    abstract class Employee : IPayroll
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }

        protected Employee(int employeeId, string name)
        {
            EmployeeId = employeeId;
            Name = name;
        }

        public abstract decimal CalculateSalary();

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"ID: {EmployeeId} | Name: {Name}");
            Console.WriteLine($"Calculated Net Salary: {CalculateSalary():C}");
        }
    }

    // Full-Time Employee Class
    class FullTimeEmployee : Employee
    {
        public decimal MonthlySalary { get; set; }

        public FullTimeEmployee(int id, string name, decimal monthlySalary) 
            : base(id, name)
        {
            MonthlySalary = monthlySalary;
        }

        public override decimal CalculateSalary() => MonthlySalary;
    }

    // Part-Time Employee Class
    class PartTimeEmployee : Employee
    {
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }

        public PartTimeEmployee(int id, string name, decimal hoursWorked, decimal hourlyRate) 
            : base(id, name)
        {
            HoursWorked = hoursWorked;
            HourlyRate = hourlyRate;
        }

        public override decimal CalculateSalary() => HoursWorked * HourlyRate;
    }

    // Main Execution
    class Program
    {
        static void Main(string[] args)
        {
            Employee[] employees = new Employee[]
            {
                new FullTimeEmployee(101, "Alice", 50000m),
                new PartTimeEmployee(102, "Bob", 80m, 500m)
            };

            Console.WriteLine("=======================================");
            Console.WriteLine("    EMPLOYEE PAYROLL SYSTEM (EXP 2)    ");
            Console.WriteLine("=======================================");

            foreach (Employee emp in employees)
            {
                emp.DisplayDetails();
                Console.WriteLine("---------------------------------------");
            }
        }
    }
}