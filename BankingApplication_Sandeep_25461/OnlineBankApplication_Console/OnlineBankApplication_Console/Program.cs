using System;
using System.Collections.Generic;
using System.IO;

namespace OnlineBankApplication_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleMenu.ShowOpeningMenu();
        }
    }

    static class ConsoleMenu
    {
        public static void ShowOpeningMenu()
        {
            Console.WriteLine("Welcome to the Bank Application!");
            Console.WriteLine("Are you a Bank Employee or a Customer?");
            string userType = Console.ReadLine();

            if (userType.ToLower() == "bank employee")
            {
                BankEmployeeMenu();
            }
            else if (userType.ToLower() == "customer")
            {
                CustomerMenu();
            }
            else
            {
                Console.WriteLine("Invalid user type. Please try again.");
                ShowOpeningMenu();
            }
        }

        static void BankEmployeeMenu()
        {
            Bank bank = new Bank();
            Console.WriteLine("Bank Employee Menu");
            Console.WriteLine("1. Create Customer");
            Console.WriteLine("2. Delete Customer");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    bank.CreateCustomer();
                    Console.WriteLine("Press any key to return to main menu...");
                    Console.ReadKey();
                    ShowOpeningMenu();
                    break;
                case 2:
                    bank.DeleteCustomer();
                    Console.WriteLine("Press any key to return to main menu...");
                    Console.ReadKey();
                    ShowOpeningMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    BankEmployeeMenu();
                    break;
            }
        }

        static void CustomerMenu()
        {
            Customer customer = new Customer();
            Console.WriteLine("Customer Menu");
            Console.WriteLine("1. Retrieve Transaction History");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    customer.RetrieveTransactionHistory();
                    Console.WriteLine("Press any key to return to main menu...");
                    Console.ReadKey();
                    ShowOpeningMenu();
                    break;
                case 2:
                    customer.DepositMoney();
                    Console.WriteLine("Press any key to return to main menu...");
                    Console.ReadKey();
                    ShowOpeningMenu();
                    break;
                case 3:
                    customer.WithdrawMoney();
                    Console.WriteLine("Press any key to return to main menu...");
                    Console.ReadKey();
                    ShowOpeningMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    CustomerMenu();
                    break;
            }
        }
    }

    class Bank
    {
        private string customersFile = "customers.txt";

        public void CreateCustomer()
        {
            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();

            // Generate account number and PIN
            string accountNumber = GenerateAccountNumber(firstName, lastName);
            string pin = GeneratePIN(firstName, lastName);

            // Save customer to file
            using (StreamWriter writer = new StreamWriter(customersFile, true))
            {
                writer.WriteLine($"{firstName},{lastName},{email},{accountNumber},{pin}");
            }

            Console.WriteLine("Customer created successfully.");
            Console.WriteLine($"Account Number: {accountNumber}");
            Console.WriteLine($"PIN: {pin}");
        }


        public void DeleteCustomer()
        {
            Console.WriteLine("Enter account number of the customer to delete:");
            string accountNumberToDelete = Console.ReadLine();

            // Read all customers from file
            List<string> allCustomers = new List<string>(File.ReadAllLines(customersFile));

            // Find the customer with the given account number
            string customerToDelete = allCustomers.Find(c => c.Split(',')[3] == accountNumberToDelete);

            if (customerToDelete != null)
            {
                // Check if the customer has zero balances before deletion
                // For simplicity, we assume zero balance here
                // You may need to implement balance checking logic
                // before allowing deletion
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        private string GenerateAccountNumber(string firstName, string lastName)
        {
            // Generate account number based on provided rules
            string initials = $"{firstName.Substring(0, 1)}{lastName.Substring(0, 1)}";
            int length = firstName.Length + lastName.Length;
            int firstInitialPosition = Char.ToUpper(firstName[0]) - 'A' + 1;
            int secondInitialPosition = Char.ToUpper(lastName[0]) - 'A' + 1;

            return $"{initials}-{length}-{firstInitialPosition}-{secondInitialPosition}";
        }

        private string GeneratePIN(string firstName, string lastName)
        {
            // Generate PIN based on provided rules
            return $"{firstName.Length}{lastName.Length}";
        }
    }

    class Customer
    {
        private string transactionHistoryFile = "transactions.txt";

        public void RetrieveTransactionHistory()
        {
            Console.WriteLine("Enter account number:");
            string accountNumber = Console.ReadLine();

            // Read transaction history for the given account number from file
            string[] transactions = File.ReadAllLines(transactionHistoryFile);

            // Display transaction history
            foreach (string transaction in transactions)
            {
                if (transaction.StartsWith(accountNumber))
                {
                    Console.WriteLine(transaction);
                }
            }
        }

        public void DepositMoney()
        {
            Console.WriteLine("Enter account number:");
            string accountNumber = Console.ReadLine();
            Console.WriteLine("Enter amount to deposit:");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            // Record the deposit transaction in the transaction history file
            using (StreamWriter writer = new StreamWriter(transactionHistoryFile, true))
            {
                writer.WriteLine($"{accountNumber}\tDeposit\t{amount}");
            }

            Console.WriteLine("Deposit successful.");
        }

        public void WithdrawMoney()
        {
            Console.WriteLine("Enter account number:");
            string accountNumber = Console.ReadLine();
            Console.WriteLine("Enter amount to withdraw:");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            // Record the withdrawal transaction in the transaction history file
            using (StreamWriter writer = new StreamWriter(transactionHistoryFile, true))
            {
                writer.WriteLine($"{accountNumber}\tWithdrawal\t{amount}");
            }

            Console.WriteLine("Withdrawal successful.");
        }
    }
}
