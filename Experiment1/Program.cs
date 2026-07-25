using System;

class Student
{
    // Private Data Members
    private int studentId;
    private string name;
    private string gender;
    private double pcmPercentage;
    private string branch;
    private double courseFee;
    private double scholarship;
    private double busFee;
    private double hostelFee;

    // Constructor
    public Student(int studentId, string name, string gender, double pcmPercentage)
    {
        this.studentId = studentId;
        this.name = name;
        this.gender = gender;
        this.pcmPercentage = pcmPercentage;
        this.branch = "";
        this.courseFee = 0;
        this.scholarship = 0;
        this.busFee = 0;
        this.hostelFee = 0;
    }

    // Check Eligibility
    public bool IsEligible()
    {
        return pcmPercentage >= 40;
    }

    // Branch Selection
    public void SelectBranch()
    {
        while (true)
        {
            Console.WriteLine("\n------ AVAILABLE BRANCHES ------");
            Console.WriteLine("1. Computer Science Engineering - Rs.100000");
            Console.WriteLine("2. Mechanical Engineering       - Rs.80000");
            Console.WriteLine("3. Civil Engineering            - Rs.70000");
            Console.WriteLine("4. Electrical Engineering       - Rs.75000");

            Console.Write("Select Branch (1-4): ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    branch = "Computer Science Engineering";
                    courseFee = 100000;
                    return;

                case 2:
                    branch = "Mechanical Engineering";
                    courseFee = 80000;
                    return;

                case 3:
                    branch = "Civil Engineering";
                    courseFee = 70000;
                    return;

                case 4:
                    branch = "Electrical Engineering";
                    courseFee = 75000;
                    return;

                default:
                    Console.WriteLine("Invalid Choice! Please Try Again.");
                    break;
            }
        }
    }

    // Scholarship
    public void CalculateScholarship()
    {
        if (pcmPercentage >= 90)
            scholarship = 100;
        else if (pcmPercentage >= 80)
            scholarship = 50;
        else if (pcmPercentage >= 70)
            scholarship = 30;
        else if (pcmPercentage >= 60)
            scholarship = 20;
        else
            scholarship = 0;
    }

    // Bus Facility
    public void AddBusFacility()
    {
        Console.Write("\nDo you want Bus Facility? (yes/no): ");
        string choice = Console.ReadLine().ToLower();

        if (choice == "yes")
        {
            Console.Write("Enter Distance from College (KM): ");
            double distance = Convert.ToDouble(Console.ReadLine());

            if (distance <= 5)
                busFee = 5000;
            else if (distance <= 10)
                busFee = 8000;
            else if (distance <= 20)
                busFee = 12000;
            else if (distance <= 30)
                busFee = 15000;
            else
                busFee = 20000;
        }
    }

    // Hostel Facility
    public void AddHostelFacility()
    {
        Console.Write("\nDo you want Hostel Facility? (yes/no): ");
        string choice = Console.ReadLine().ToLower();

        if (choice == "yes")
            hostelFee = 30000;
    }

    // Display Details
    public void DisplayAdmissionDetails()
    {
        double scholarshipAmount = courseFee * scholarship / 100;
        double feeAfterScholarship = courseFee - scholarshipAmount;
        double finalFee = feeAfterScholarship + busFee + hostelFee;

        Console.WriteLine("\n===========================================");
        Console.WriteLine("        STUDENT ADMISSION RECEIPT");
        Console.WriteLine("===========================================");
        Console.WriteLine("College Name        : Marwadi University");
        Console.WriteLine("Student ID          : " + studentId);
        Console.WriteLine("Student Name        : " + name);
        Console.WriteLine("Gender              : " + gender);
        Console.WriteLine("Admission Date      : " + DateTime.Now.ToShortDateString());
        Console.WriteLine("PCM Percentage      : " + pcmPercentage + "%");
        Console.WriteLine("Selected Branch     : " + branch);
        Console.WriteLine("Course Duration     : 4 Years");
        Console.WriteLine("Course Fee          : Rs. " + courseFee);
        Console.WriteLine("Scholarship         : " + scholarship + "%");
        Console.WriteLine("Scholarship Amount  : Rs. " + scholarshipAmount);
        Console.WriteLine("Bus Fee             : Rs. " + busFee);
        Console.WriteLine("Hostel Fee          : Rs. " + hostelFee);
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("Final Payable Fee   : Rs. " + finalFee);
        Console.WriteLine("Payment Status      : Pending");
        Console.WriteLine("===========================================");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("      STUDENT ADMISSION MANAGEMENT");
        Console.WriteLine("===========================================");

        Console.Write("Enter Student ID: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Student Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Gender (Male/Female): ");
        string gender = Console.ReadLine();

        Console.Write("Enter 12th PCM Percentage: ");
        double percentage = Convert.ToDouble(Console.ReadLine());

        Student student = new Student(id, name, gender, percentage);

        if (!student.IsEligible())
        {
            Console.WriteLine("\nAdmission Rejected!");
            Console.WriteLine("Minimum 40% PCM is required.");
            return;
        }

        Console.WriteLine("\nCongratulations! You are Eligible for Admission.");

        student.SelectBranch();
        student.CalculateScholarship();
        student.AddBusFacility();
        student.AddHostelFacility();
        student.DisplayAdmissionDetails();

        Console.WriteLine("\nAdmission Process Completed Successfully!");
        Console.ReadKey();
    }
}