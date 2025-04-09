using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LoanMngement
{
    public class ILoanRepositoryImpl : ILoanRepository
    {
        public void ApplyLoan(Loan loan)
        {
            using (SqlConnection conn = DBUtil.GetDBConn())
            {


                string query = @"
                    INSERT INTO Loan 
                    (LoanId,CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus)
                    VALUES 
                    (LoanId,CustomerId, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);
                cmd.Parameters.AddWithValue("@CustomerId", loan.Customer.CustomerId);
                cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                cmd.Parameters.AddWithValue("@LoanStatus", "Pending");

                if (loan is HomeLoan homeLoan)
                {
                    cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);
                    cmd.Parameters.AddWithValue("@PropertyAddress", homeLoan.PropertyAddress);

                    cmd.Parameters.AddWithValue("@PropertyValue", homeLoan.PropertyValue);
                    cmd.Parameters.AddWithValue("@CarModel", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CarValue", DBNull.Value);
                }
                else if (loan is CarLoan carLoan)
                {
                    cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);
                    cmd.Parameters.AddWithValue("@CarModel", carLoan.CarModel);
                    cmd.Parameters.AddWithValue("@CarValue", carLoan.CarValue);
                    cmd.Parameters.AddWithValue("@PropertyAddress", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PropertyValue", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PropertyAddress", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PropertyValue", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CarModel", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CarValue", DBNull.Value);
                }

                cmd.ExecuteNonQuery();
                Console.WriteLine("Loan application submitted successfully!");
            }
        }
        public decimal CalculateInterest(int loanId)
        {
            using (SqlConnection conn = DBUtil.GetDBConn())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    Console.WriteLine("Enter the Loan Id:");
                    cmd.Parameters.AddWithValue("@LoanId", loanId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal principal = Convert.ToDecimal(reader["PrincipalAmount"]);
                            decimal rate = Convert.ToDecimal(reader["InterestRate"]);
                            int term = Convert.ToInt32(reader["LoanTerm"]);

                            decimal interest = (principal * rate * term) / (12 * 100);
                            return interest;
                        }
                        else
                        {
                            throw new InvalidLoanException($"Loan with ID {loanId} not found.");
                            return 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }
        public void LoanStatus(int loanId)
        {
            using (SqlConnection conn = DBUtil.GetDBConn())
            {
                conn.Open();
                string query = @"SELECT l.LoanId, l.CustomerId, c.CreditScore 
                                 FROM Loan l JOIN Customer c ON l.CustomerId = c.CustomerId 
                                 WHERE l.LoanId = @LoanId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int creditScore = Convert.ToInt32(reader["CreditScore"]);
                        string status = creditScore > 650 ? "Approved" : "Rejected";

                        reader.Close();

                        SqlCommand update = new SqlCommand("UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId", conn);
                        update.Parameters.AddWithValue("@Status", status);
                        update.Parameters.AddWithValue("@LoanId", loanId);
                        update.ExecuteNonQuery();

                        Console.WriteLine($"Loan {loanId} has been {status} based on credit score.");
                    }
                    else
                    {
                        throw new InvalidLoanException($"Loan with ID {loanId} not found.");
                    }
                }
            }
        }

        public decimal CalculateEMI(int loanId)
        {
            using (SqlConnection conn = DBUtil.GetDBConn())
            {
                string query = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@LoanId", loanId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal principal = Convert.ToDecimal(reader["PrincipalAmount"]);
                                decimal annualRate = Convert.ToDecimal(reader["InterestRate"]);
                                int term = Convert.ToInt32(reader["LoanTerm"]);

                                decimal monthlyRate = annualRate / 12 / 100;
                                decimal emi = principal * monthlyRate * (decimal)Math.Pow(1 + (double)monthlyRate, term)
                                             / (decimal)(Math.Pow(1 + (double)monthlyRate, term) - 1);

                                return Math.Round(emi, 2);
                            }
                            else
                            {
                                throw new InvalidLoanException($"Loan with ID {loanId} not found.");
                                return 0;
                            }
                        }
                    }
                    catch (InvalidLoanException e)
                    {
                        Console.WriteLine(e.Message);
                        return 0;
                    }
                }
            }
        }
        public void LoanRepayment(int loanId, decimal amount) { }
        public void GetAllLoan()
        {
            using (SqlConnection conn = DBUtil.GetDBConn())
            {
                conn.Open();
                Console.WriteLine("====Loan Details=====");
                SqlCommand cmd = new SqlCommand("SELECT * FROM Loan", conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"\nLoan ID: {reader["LoanId"]}");
                        Console.WriteLine($"Customer ID: {reader["CustomerId"]}");
                        Console.WriteLine($"Type: {reader["LoanType"]}");
                        Console.WriteLine($"Amount: {reader["PrincipalAmount"]}");
                        Console.WriteLine($"Rate: {reader["InterestRate"]}%");
                        Console.WriteLine($"Term: {reader["LoanTerm"]} months");
                        Console.WriteLine($"Status: {reader["LoanStatus"]}");

                        if (reader["LoanType"].ToString() == "HomeLoan")
                        {
                            Console.WriteLine($"Property: {reader["PropertyAddress"]}, Value: {reader["PropertyValue"]}");
                        }
                        else if (reader["LoanType"].ToString() == "CarLoan")
                        {
                            Console.WriteLine($"Car: {reader["CarModel"]}, Value: {reader["CarValue"]}");
                        }
                    }
                }
            }
        }

        public void GetLoanById(int loanId)
        {
            using (SqlConnection conn = DBUtil.GetDBConn())
            {
                try 
                { 
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Loan WHERE LoanId = @LoanId", conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                Console.WriteLine("=====Loan Deatails=====");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"\nLoan ID: {reader["LoanId"]}");
                            Console.WriteLine($"Customer ID: {reader["CustomerId"]}");
                            Console.WriteLine($"Type: {reader["LoanType"]}");
                            Console.WriteLine($"Amount: {reader["PrincipalAmount"]}");
                            Console.WriteLine($"Rate: {reader["InterestRate"]}%");
                            Console.WriteLine($"Term: {reader["LoanTerm"]} months");
                            Console.WriteLine($"Status: {reader["LoanStatus"]}");

                            if (reader["LoanType"].ToString() == "HomeLoan")
                            {
                                Console.WriteLine($"Property: {reader["PropertyAddress"]}, Value: {reader["PropertyValue"]}");
                            }
                            else if (reader["LoanType"].ToString() == "CarLoan")
                            {
                                Console.WriteLine($"Car: {reader["CarModel"]}, Value: {reader["CarValue"]}");
                            }
                        }
                        else
                        {
                            throw new InvalidLoanException($"Loan with ID {loanId} not found.");
                        }
                    }
                }
                catch(InvalidLoanException ex) 
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
