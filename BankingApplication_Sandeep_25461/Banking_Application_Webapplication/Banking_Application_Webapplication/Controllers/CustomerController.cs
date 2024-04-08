using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Banking_Application_Webapplication;

namespace Banking_Application_Webapplication.Controllers
{
    //StudentName -: Sandeep Sandeep
    //Student Id -: 25461
    //Student Email -: 25461@student.dorset-college.ie 
    public class CustomerController : Controller
    {
        private readonly dbBankingApplicationEntities _context;

        public CustomerController()
        {
            _context = new dbBankingApplicationEntities();
        }

        // Opening menu for Customer login
        public ActionResult Login()
        {
            return View();
        }

        // Action for customer login
        [HttpPost]
        public ActionResult Login(string firstName, string lastName, string accountNumber, string pin)
        {
            var customer = _context.Customers.FirstOrDefault(c =>
                c.FirstName == firstName && c.LastName == lastName && c.Accounts.Any(a => a.AccountNumber == accountNumber));

            if (customer != null && pin == customer.PIN)
            {
                return RedirectToAction("Dashboard", new { id = customer.Id });
            }

            ViewBag.ErrorMessage = "Invalid credentials.";
            return View();
        }

        // Action for customer dashboard
        public ActionResult Dashboard(int id)
        {
            // Retrieve the customer with associated accounts
            var customer = _context.Customers.Include(c => c.Accounts).FirstOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }



        // Action for depositing money
        [HttpPost]
        public ActionResult Deposit(int customerId, string accountNumber, decimal amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.CustomerId == customerId && a.AccountNumber == accountNumber);
            if (account == null)
                return HttpNotFound();

            // Perform deposit operation
            account.Balance += amount;
            _context.SaveChanges();

            // Log transaction
            LogTransaction(customerId, accountNumber, "Deposit", amount);

            return RedirectToAction("Dashboard", new { id = customerId });
        }

        // Action for withdrawing money
        [HttpPost]
        public ActionResult Withdraw(int customerId, string accountNumber, decimal amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.CustomerId == customerId && a.AccountNumber == accountNumber);
            if (account == null)
                return HttpNotFound();

            if (account.Balance < amount)
            {
                ViewBag.ErrorMessage = "Insufficient funds.";
                return RedirectToAction("Dashboard", new { id = customerId });
            }

            // Perform withdrawal operation
            account.Balance -= amount;
            _context.SaveChanges();

            // Log transaction
            LogTransaction(customerId, accountNumber, "Withdrawal", amount);

            return RedirectToAction("Dashboard", new { id = customerId });
        }

        // Helper method to log transactions
        private void LogTransaction(int customerId, string accountNumber, string action, decimal amount)
        {
            var transaction = new Transaction
            {
                CustomerId = customerId,
                AccountNumber = accountNumber,
                Action = action,
                Amount = amount,
                TransactionDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public ActionResult TransactionReport(int customerId)
        {
            // Query the database to fetch transaction records for the specified customer ID
            var transactions = _context.Transactions
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.TransactionDate) // Optionally, order transactions by date
                .ToList();

            // Pass the fetched transactions to the view
            return View(transactions);
        }
    }
}
