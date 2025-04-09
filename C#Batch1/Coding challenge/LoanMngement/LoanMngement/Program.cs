namespace LoanMngement
{
    public class InvalidLoanException : Exception
    {
        public InvalidLoanException(string message) : base(message) { }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ILoanRepositoryImpl repo = new ILoanRepositoryImpl();

            while (true)
            {
                Console.WriteLine("\n===== Loan Management Menu =====");
                Console.WriteLine("1. Apply for a Loan");
                Console.WriteLine("2. Calculate Interest");
                Console.WriteLine("3. Calculate EMI");
                Console.WriteLine("4. Check Loan Status");
                Console.WriteLine("5. Get All Loans");
                Console.WriteLine("6. Get Loan by ID");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter Loan Type (HomeLoan/CarLoan):");
                        string loanType = Console.ReadLine();
                        Console.WriteLine("Enter loan ID");
                        int loanId = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Customer Id:");
                        int customerId = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Principal Amount:");
                        decimal amount = decimal.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Interest Rate:");
                        decimal rate = decimal.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Loan Term (months):");
                        int term = int.Parse(Console.ReadLine());

                        Loan loan;
                        if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Enter Property Address:");
                            string address = Console.ReadLine();

                            Console.WriteLine("Enter Property Value:");
                            int propValue = int.Parse(Console.ReadLine());

                            loan = new HomeLoan
                            {
                                Customer = new Customer { CustomerId = customerId },
                                PrincipalAmount = amount,
                                InterestRate = rate,
                                LoanTerm = term,
                                LoanType = "HomeLoan",
                                PropertyAddress = address,
                                PropertyValue = propValue
                            };
                        }
                        else if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Enter Car Model:");
                            string model = Console.ReadLine();

                            Console.WriteLine("Enter Car Value:");
                            int carValue = int.Parse(Console.ReadLine());

                            loan = new CarLoan
                            {
                                Customer = new Customer { CustomerId = customerId },
                                PrincipalAmount = amount,
                                InterestRate = rate,
                                LoanTerm = term,
                                LoanType = "CarLoan",
                                CarModel = model,
                                CarValue = carValue
                            };
                        }
                        else
                        {
                            Console.WriteLine("Invalid loan type.");
                            break;
                        }

                        repo.ApplyLoan(loan);
                        break;

                    case 2:
                        Console.WriteLine("Enter Loan ID to calculate interest:");
                        int loanId1 = int.Parse(Console.ReadLine());
                        decimal interest = repo.CalculateInterest(loanId1);
                        Console.WriteLine($"Interest: {interest:C}");
                        break;

                    case 3:
                        Console.WriteLine("Enter Loan ID to calculate EMI:");
                        int loanId2 = int.Parse(Console.ReadLine());
                        decimal emi = repo.CalculateEMI(loanId2);
                        Console.WriteLine($"EMI: {emi:C}");
                        break;

                    case 4:
                        Console.WriteLine("Enter Loan ID to check status:");
                        int loanId3 = int.Parse(Console.ReadLine());
                        repo.LoanStatus(loanId3);
                        break;

                    case 5:
                        repo.GetAllLoan();
                        break;

                    case 6:
                        Console.WriteLine("Enter Loan ID to view:");
                        int loanId4 = int.Parse(Console.ReadLine());
                        repo.GetLoanById(loanId4);
                        break;

                    case 7:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }

}
