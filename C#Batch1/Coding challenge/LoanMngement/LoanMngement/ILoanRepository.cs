using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanMngement
{
    interface ILoanRepository
    {
        void ApplyLoan(Loan Loan);
        decimal CalculateInterest(int loanId);
        void LoanStatus(int loanId);
        decimal CalculateEMI(int loanId); 
        void LoanRepayment(int loanId, decimal amount);
        void GetAllLoan();
        void GetLoanById(int loanId);
    } 
}
