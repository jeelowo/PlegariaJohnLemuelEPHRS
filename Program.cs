using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    public int Id { get; set; }                  
    public string FirstName { get; set; } = "";  
    public string LastName { get; set; } = "";  
    public string Department { get; set; } = "";
    public string JobTitle { get; set; } = "";   
    public decimal MonthlySalary { get; set; }  
    public double TaxRate { get; set; }      
    public DateTime HireDate { get; set; }   
    public bool IsActive { get; set; }         
    public string Email { get; set; } = "";      
}

class Program
{
    static List<Employee> employees = new List<Employee>();
    static int nextId = 1001;

    static void Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== EPHRS - FEATURE BRANCH ===");
            Console.WriteLine("1. Create (Add Employee)");
            Console.WriteLine("2. Read (View All Employees)");
            Console.WriteLine("3. Search / Filter");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.Write("\nSelect an option: ");
            string choice = Console.ReadLine() ?? "";
            Console.WriteLine();

            switch (choice)
            {
                case "1": CreateEmployee(); break;
                case "2": ReadEmployees(); break;
                case "3": SearchEmployees(); break;
                case "4": UpdateEmployee(); break;
                case "5": DeleteEmployee(); break;
                case "6": running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }

            if (running) { Console.WriteLine("\nPress Enter to continue..."); Console.ReadLine(); }
        }
    }

    // ---------- CREATE ----------
    static void CreateEmployee()
    {
        Console.WriteLine("--- Add New Employee ---");
        Console.Write("First Name: ");
        string first = Console.ReadLine() ?? "";
        Console.Write("Last Name: ");
        string last = Console.ReadLine() ?? "";
        Console.Write("Department (IT, HR, Sales, Finance): ");
        string dept = Console.ReadLine() ?? "";
        Console.Write("Job Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Monthly Salary: ");
        decimal.TryParse(Console.ReadLine(), out decimal salary);

        Console.Write("Tax Rate (e.g., 0.12 for 12%): ");
        double.TryParse(Console.ReadLine(), out double tax);

        Console.Write("Hire Date (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime hire)) hire = DateTime.Today;

        Console.Write("Active? (y/n): ");
        bool active = (Console.ReadLine() ?? "").Trim().ToLower() == "y";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        employees.Add(new Employee
        {
            Id = nextId++,
            FirstName = first,
            LastName = last,
            Department = dept,
            JobTitle = title,
            MonthlySalary = salary,
            TaxRate = tax,
            HireDate = hire,
            IsActive = active,
            Email = email
        });

        Console.WriteLine("\nEmployee added successfully!");
    }

    // ---------- READ ----------
    static void ReadEmployees()
    {
        Console.WriteLine("--- Employee Records ---");
        PrintTable(employees);
    }

    static void PrintTable(IEnumerable<Employee> list)
    {
        if (!list.Any())
        {
            Console.WriteLine("No employees found.");
            return;
        }

        Console.WriteLine($"{"ID",-6}{"First",-10}{"Last",-10}{"Dept",-9}{"Title",-17}{"Salary",11}{"Tax",6}  {"Hired",-11}{"Active",-7}{"Email",-25}");
        Console.WriteLine(new string('-', 112));
        foreach (var e in list)
        {
            Console.WriteLine($"{e.Id,-6}{Cut(e.FirstName, 9),-10}{Cut(e.LastName, 9),-10}{Cut(e.Department, 8),-9}{Cut(e.JobTitle, 16),-17}{e.MonthlySalary,11:F2}{e.TaxRate,6:P0}  {e.HireDate:yyyy-MM-dd,-11}{(e.IsActive ? "Yes" : "No"),-7}{Cut(e.Email, 24),-25}");
        }
        Console.WriteLine($"\nTotal: {list.Count()} employee(s)");
    }

    static string Cut(string s, int max)
    {
        return s.Length > max ? s.Substring(0, max - 1) + "." : s;
    }

    // ---------- SEARCH / FILTER (LINQ) ----------
    static void SearchEmployees()
    {
        Console.WriteLine("--- Search / Filter ---");
        Console.WriteLine("1. By name");
        Console.WriteLine("2. By department");
        Console.WriteLine("3. By status (active / inactive)");
        Console.WriteLine("4. By minimum monthly salary");
        Console.Write("\nChoose filter: ");
        string opt = Console.ReadLine() ?? "";
        Console.WriteLine();

        IEnumerable<Employee> results;

        switch (opt)
        {
            case "1":
                Console.Write("Enter name keyword: ");
                string key = (Console.ReadLine() ?? "").Trim().ToLower();
                results = employees.Where(e => e.FirstName.ToLower().Contains(key) || e.LastName.ToLower().Contains(key));
                break;
            case "2":
                Console.Write("Enter department: ");
                string dept = (Console.ReadLine() ?? "").Trim().ToLower();
                results = employees.Where(e => e.Department.ToLower() == dept);
                break;
            case "3":
                Console.Write("Show active employees? (y/n): ");
                bool active = (Console.ReadLine() ?? "").Trim().ToLower() == "y";
                results = employees.Where(e => e.IsActive == active);
                break;
            case "4":
                Console.Write("Minimum monthly salary: ");
                decimal.TryParse(Console.ReadLine(), out decimal min);
                results = employees.Where(e => e.MonthlySalary >= min).OrderByDescending(e => e.MonthlySalary);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                return;
        }

        Console.WriteLine();
        PrintTable(results.ToList());
    }

    // ---------- UPDATE ----------
    static void UpdateEmployee()
    {
        ReadEmployees();
        if (employees.Count == 0) return;

        Console.Write("\nEnter the ID of the employee to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("\nInvalid ID format.");
            return;
        }

        Employee? emp = employees.FirstOrDefault(e => e.Id == id);
        if (emp == null)
        {
            Console.WriteLine("\nEmployee not found.");
            return;
        }

        Console.WriteLine("\n(Press Enter to keep the current value)\n");

        Console.Write($"First Name ({emp.FirstName}): ");
        string s = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(s)) emp.FirstName = s;

        Console.Write($"Last Name ({emp.LastName}): ");
        s = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(s)) emp.LastName = s;

        Console.Write($"Department ({emp.Department}): ");
        s = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(s)) emp.Department = s;

        Console.Write($"Job Title ({emp.JobTitle}): ");
        s = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(s)) emp.JobTitle = s;

        Console.Write($"Monthly Salary ({emp.MonthlySalary}): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal newSalary)) emp.MonthlySalary = newSalary;

        Console.Write($"Tax Rate ({emp.TaxRate}): ");
        if (double.TryParse(Console.ReadLine(), out double newTax)) emp.TaxRate = newTax;

        Console.Write($"Hire Date ({emp.HireDate:yyyy-MM-dd}): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime newHire)) emp.HireDate = newHire;

        Console.Write($"Active? y/n ({(emp.IsActive ? "y" : "n")}): ");
        s = (Console.ReadLine() ?? "").Trim().ToLower();
        if (s == "y") emp.IsActive = true;
        else if (s == "n") emp.IsActive = false;

        Console.Write($"Email ({emp.Email}): ");
        s = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(s)) emp.Email = s;

        Console.WriteLine("\nEmployee updated successfully!");
    }

    // ---------- DELETE ----------
    static void DeleteEmployee()
    {
        ReadEmployees();
        if (employees.Count == 0) return;

        Console.Write("\nEnter the ID of the employee to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            int removed = employees.RemoveAll(e => e.Id == id);
            Console.WriteLine(removed > 0
                ? "\nEmployee deleted successfully!"
                : "\nEmployee not found.");
        }
        else
        {
            Console.WriteLine("\nInvalid ID format.");
        }
    }
}