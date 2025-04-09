using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanMngement
{
    public class Loan
    {
        private int loanId;
        private Customer customer;
        private decimal principalAmount;
        private decimal interestRate;
        private int loanTerm;
        private string loanType;
        private string loanStatus;

        public Loan() { }

        public Loan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus)
        {
            this.loanId = loanId;
            this.customer = customer;
            this.principalAmount = principalAmount;
            this.interestRate = interestRate;
            this.loanTerm = loanTerm;
            this.loanType = loanType;
            this.loanStatus = loanStatus;
        }
        public int LoanId { get => loanId; set => loanId = value; }
        public Customer Customer { get => customer; set => customer = value; }
        public decimal PrincipalAmount { get => principalAmount; set => principalAmount = value; }
        public decimal InterestRate { get => interestRate; set => interestRate = value; }
        public int LoanTerm { get => loanTerm; set => loanTerm = value; }
        public string LoanType { get => loanType; set => loanType = value; }
        public string LoanStatus { get => loanStatus; set => loanStatus = value; }

        public virtual void DisplayLoanInfo()
        {
            Console.WriteLine("=====Loan Details=====");
            Console.WriteLine($"Loan ID: {LoanId}");
            Console.WriteLine($" Type: {LoanType}");
            Console.WriteLine($"Principal: {PrincipalAmount}");
            Console.WriteLine($"Rate: {InterestRate}%");
            Console.WriteLine($"Term: {LoanTerm} months");
            Console.WriteLine($"Status: {LoanStatus}");
        }
    }
    public class HomeLoan : Loan
    {
        private string propertyAddress;
        private int propertyValue;
        public HomeLoan() { }
        public HomeLoan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, "HomeLoan", loanStatus)
        {
            this.propertyAddress = propertyAddress;
            this.propertyValue = propertyValue;
        }

        public string PropertyAddress { get => propertyAddress; set => propertyAddress = value; }
        public int PropertyValue { get => propertyValue; set => propertyValue = value; }

        public override void DisplayLoanInfo()
        {
            base.DisplayLoanInfo();
            Console.WriteLine($"Property Address: {PropertyAddress}, Property Value: {PropertyValue}");
        }
    }
    public class CarLoan : Loan
    {
        private string carModel;
        private int carValue;

        public CarLoan() { }
        public CarLoan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanStatus, string carModel, int carValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, "CarLoan", loanStatus)
        {
            this.carModel = carModel;
            this.carValue = carValue;
        }
        public string CarModel { get => carModel; set => carModel = value; }
        public int CarValue { get => carValue; set => carValue = value; }
        public override void DisplayLoanInfo()
        {
            base.DisplayLoanInfo();
            Console.WriteLine($"Car Model: {CarModel}, Car Value: {CarValue}");
        }
    }
}
