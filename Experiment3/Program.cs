using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ExpenseTracker
{
    public enum ExpenseCategory
    {
        Food = 1,
        Travel,
        Utilities,
        Entertainment,
        Miscellaneous
    }

    public enum PaymentMethod
    {
        Cash = 1,
        UPI,
        DebitCard,
        CreditCard,
        NetBanking
    }

    public class ExpenseTrackerException : Exception
    {
        public ExpenseTrackerException(string message) : base(message) { }
    }

    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public ExpenseCategory Category { get; set; }
        public PaymentMethod Payment { get; set; }

        public Expense(int id, string desc, double amount,
                       DateTime date, ExpenseCategory cat,
                       PaymentMethod pay)
        {
            Id = id;
            Description = desc;
            Amount = amount;
            Date = date;
            Category = cat;
            Payment = pay;
        }

        public override string ToString()
        {
            return $"| {Id,-3} | {Date:dd-MM-yyyy} | {Category,-15} | " +
                   $"{Payment,-12} | {Description,-20} | Rs. {Amount,10:N2} |";
        }
    }

    class Program
    {
        static int _nextId = 1;

        static void Main()
        {
            List<Expense> expenseList = new List<Expense>();

            while (true)
            {
                Console.WriteLine("\n=================================================");
                Console.WriteLine("           EXPENSE TRACKING MODULE");
                Console.WriteLine("=================================================");
                Console.WriteLine("1. Add New Expense");
                Console.WriteLine("2. View All Expenses");
                Console.WriteLine("3. View Expenses by Month & Year");
                Console.WriteLine("4. Edit Expense");
                Console.WriteLine("5. Delete Expense");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option (1-6): ");

                string choice = Console.ReadLine() ?? "";

                if (choice == "1")
                {
                    try
                    {
                        Console.Write("Enter Description: ");
                        string desc = Console.ReadLine() ?? "";

                        if (string.IsNullOrWhiteSpace(desc))
                            throw new ExpenseTrackerException(
                                "Description cannot be empty!");

                        if (!Regex.IsMatch(desc, @"[a-zA-Z]"))
                            throw new ExpenseTrackerException(
                                "Description cannot be purely numeric or symbols.");

                        Console.Write("Enter Amount (Rs.): ");
                        double amount = Convert.ToDouble(Console.ReadLine() ?? "");

                        if (amount <= 0)
                            throw new ExpenseTrackerException(
                                "Expense amount must be strictly greater than zero.");

                        Console.Write("Enter Date (DD-MM-YYYY): ");

                        string[] formats =
                        {
                            "dd-MM-yyyy",
                            "d-M-yyyy",
                            "d-MM-yyyy",
                            "dd-M-yyyy"
                        };

                        DateTime date = DateTime.ParseExact(
                            Console.ReadLine() ?? "",
                            formats,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None);

                        if (date > DateTime.Today)
                            throw new ExpenseTrackerException(
                                "Expense date cannot be in the future.");

                        Console.WriteLine(
                            "Categories: 1.Food, 2.Travel, 3.Utilities, " +
                            "4.Entertainment, 5.Miscellaneous");

                        Console.Write("Select Category (1-5): ");
                        int catChoice =
                            Convert.ToInt32(Console.ReadLine() ?? "");

                        if (!Enum.IsDefined(typeof(ExpenseCategory), catChoice))
                            throw new ExpenseTrackerException(
                                "Invalid Category selected.");

                        Console.WriteLine(
                            "Payment Methods: 1.Cash, 2.UPI, 3.DebitCard, " +
                            "4.CreditCard, 5.NetBanking");

                        Console.Write("Select Payment Method (1-5): ");
                        int payChoice =
                            Convert.ToInt32(Console.ReadLine() ?? "");

                        if (!Enum.IsDefined(typeof(PaymentMethod), payChoice))
                            throw new ExpenseTrackerException(
                                "Invalid Payment Method selected.");

                        Expense newExp = new Expense(
                            _nextId++,
                            desc,
                            amount,
                            date,
                            (ExpenseCategory)catChoice,
                            (PaymentMethod)payChoice);

                        expenseList.Add(newExp);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(
                            "\n[SUCCESS] Expense added successfully!");
                        Console.ResetColor();
                    }
                    catch (FormatException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "\n[ERROR] Invalid format! Please enter " +
                            "correct numeric values and date format.");
                        Console.ResetColor();
                    }
                    catch (ExpenseTrackerException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"\n[VALIDATION ERROR] {ex.Message}");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"\n[SYSTEM ERROR] Something went wrong: " +
                            $"{ex.Message}");
                        Console.ResetColor();
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("\n--- COMPLETE EXPENSE LIST ---");

                    if (expenseList.Count == 0)
                    {
                        Console.WriteLine("No expenses recorded yet.");
                    }
                    else
                    {
                        PrintLedgerHeader();

                        double total = 0;

                        var sortedList = expenseList
                            .OrderBy(e => e.Date)
                            .ToList();

                        foreach (var exp in sortedList)
                        {
                            Console.WriteLine(exp.ToString());
                            total += exp.Amount;
                        }

                        PrintLedgerFooter(total);
                    }
                }
                else if (choice == "3")
                {
                    try
                    {
                        Console.Write("Enter Month Number (1-12): ");
                        int month =
                            Convert.ToInt32(Console.ReadLine() ?? "");

                        if (month < 1 || month > 12)
                            throw new ExpenseTrackerException(
                                "Invalid month! Must be between 1 and 12.");

                        Console.Write("Enter Year: ");
                        int year =
                            Convert.ToInt32(Console.ReadLine() ?? "");

                        Console.WriteLine(
                            $"\n--- MONTHLY REPORT FOR {month:D2}/{year} ---");

                        var monthlyList = expenseList
                            .Where(e => e.Date.Month == month &&
                                        e.Date.Year == year)
                            .OrderBy(e => e.Date)
                            .ToList();

                        if (monthlyList.Count == 0)
                        {
                            Console.WriteLine(
                                "No expenses found for this month and year.");
                        }
                        else
                        {
                            PrintLedgerHeader();

                            double monthTotal = 0;

                            foreach (var exp in monthlyList)
                            {
                                Console.WriteLine(exp.ToString());
                                monthTotal += exp.Amount;
                            }

                            PrintLedgerFooter(monthTotal);
                        }
                    }
                    catch (FormatException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "\n[ERROR] Please enter a valid number " +
                            "for month and year.");
                        Console.ResetColor();
                    }
                    catch (ExpenseTrackerException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"\n[VALIDATION ERROR] {ex.Message}");
                        Console.ResetColor();
                    }
                }
                else if (choice == "4")
                {
                    try
                    {
                        Console.Write("Enter Expense ID to Edit: ");
                        int editId =
                            Convert.ToInt32(Console.ReadLine() ?? "");

                        Expense? expToEdit =
                            expenseList.FirstOrDefault(e => e.Id == editId);

                        if (expToEdit == null)
                            throw new ExpenseTrackerException(
                                "No expense found with this ID.");

                        Console.Write(
                            $"Enter New Description (Current: " +
                            $"{expToEdit.Description}) [Press Enter to skip]: ");

                        string editDesc = Console.ReadLine() ?? "";

                        if (string.IsNullOrWhiteSpace(editDesc))
                            editDesc = expToEdit.Description;
                        else if (!Regex.IsMatch(editDesc, @"[a-zA-Z]"))
                            throw new ExpenseTrackerException(
                                "Description cannot be purely numeric or symbols.");

                        Console.Write(
                            $"Enter New Amount (Current: Rs. " +
                            $"{expToEdit.Amount}) [Press Enter to skip]: ");

                        string editAmtStr = Console.ReadLine() ?? "";
                        double editAmount = expToEdit.Amount;

                        if (!string.IsNullOrWhiteSpace(editAmtStr))
                        {
                            editAmount = Convert.ToDouble(editAmtStr);

                            if (editAmount <= 0)
                                throw new ExpenseTrackerException(
                                    "Expense amount must be strictly greater than zero.");
                        }

                        Console.Write(
                            $"Enter New Date (Current: " +
                            $"{expToEdit.Date:dd-MM-yyyy}) " +
                            "[Press Enter to skip]: ");

                        string editDateStr = Console.ReadLine() ?? "";
                        DateTime editDate = expToEdit.Date;

                        if (!string.IsNullOrWhiteSpace(editDateStr))
                        {
                            string[] formats =
                            {
                                "dd-MM-yyyy",
                                "d-M-yyyy",
                                "d-MM-yyyy",
                                "dd-M-yyyy"
                            };

                            editDate = DateTime.ParseExact(
                                editDateStr,
                                formats,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None);

                            if (editDate > DateTime.Today)
                                throw new ExpenseTrackerException(
                                    "Expense date cannot be in the future.");
                        }

                        Console.WriteLine(
                            "Categories: 1.Food, 2.Travel, 3.Utilities, " +
                            "4.Entertainment, 5.Miscellaneous");

                        Console.Write(
                            $"Select New Category (Current: " +
                            $"{expToEdit.Category}) [Press Enter to skip]: ");

                        string editCatStr = Console.ReadLine() ?? "";
                        ExpenseCategory editCat = expToEdit.Category;

                        if (!string.IsNullOrWhiteSpace(editCatStr))
                        {
                            int catIdx =
                                Convert.ToInt32(editCatStr);

                            if (!Enum.IsDefined(
                                typeof(ExpenseCategory), catIdx))
                                throw new ExpenseTrackerException(
                                    "Invalid Category selected.");

                            editCat = (ExpenseCategory)catIdx;
                        }

                        Console.WriteLine(
                            "Payment Methods: 1.Cash, 2.UPI, 3.DebitCard, " +
                            "4.CreditCard, 5.NetBanking");

                        Console.Write(
                            $"Select New Payment Method (Current: " +
                            $"{expToEdit.Payment}) [Press Enter to skip]: ");

                        string editPayStr = Console.ReadLine() ?? "";
                        PaymentMethod editPay = expToEdit.Payment;

                        if (!string.IsNullOrWhiteSpace(editPayStr))
                        {
                            int payIdx =
                                Convert.ToInt32(editPayStr);

                            if (!Enum.IsDefined(
                                typeof(PaymentMethod), payIdx))
                                throw new ExpenseTrackerException(
                                    "Invalid Payment Method selected.");

                            editPay = (PaymentMethod)payIdx;
                        }

                        expToEdit.Description = editDesc;
                        expToEdit.Amount = editAmount;
                        expToEdit.Date = editDate;
                        expToEdit.Category = editCat;
                        expToEdit.Payment = editPay;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(
                            "\n[SUCCESS] Expense updated successfully!");
                        Console.ResetColor();
                    }
                    catch (FormatException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "\n[ERROR] Invalid format entered during update.");
                        Console.ResetColor();
                    }
                    catch (ExpenseTrackerException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"\n[VALIDATION ERROR] {ex.Message}");
                        Console.ResetColor();
                    }
                }
                else if (choice == "5")
                {
                    try
                    {
                        Console.Write("Enter Expense ID to Delete: ");
                        int delId =
                            Convert.ToInt32(Console.ReadLine() ?? "");

                        Expense? expToDel =
                            expenseList.FirstOrDefault(e => e.Id == delId);

                        if (expToDel == null)
                            throw new ExpenseTrackerException(
                                "No expense found with this ID.");

                        Console.Write(
                            $"Are you sure you want to delete " +
                            $"'{expToDel.Description}' of Rs. " +
                            $"{expToDel.Amount}? (Y/N): ");

                        string confirm =
                            Console.ReadLine()?.ToUpper() ?? "";

                        if (confirm == "Y" || confirm == "YES")
                        {
                            expenseList.Remove(expToDel);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(
                                "\n[SUCCESS] Expense deleted successfully!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("\nDeletion cancelled.");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            "\n[ERROR] Invalid ID format.");
                        Console.ResetColor();
                    }
                    catch (ExpenseTrackerException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(
                            $"\n[VALIDATION ERROR] {ex.Message}");
                        Console.ResetColor();
                    }
                }
                else if (choice == "6")
                {
                    Console.WriteLine("Exiting... Have a good day!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }

        static void PrintLedgerHeader()
        {
            Console.WriteLine(new string('-', 93));
            Console.WriteLine(
                $"| {"ID",-3} | {"Date",-10} | {"Category",-15} | " +
                $"{"Payment",-12} | {"Description",-20} | {"Amount",-14} |");
            Console.WriteLine(new string('-', 93));
        }

        static void PrintLedgerFooter(double total)
        {
            Console.WriteLine(new string('-', 93));
            Console.WriteLine(
                $"| {"TOTAL",-72} | Rs. {total,10:N2} |");
            Console.WriteLine(new string('-', 93));
        }
    }
}