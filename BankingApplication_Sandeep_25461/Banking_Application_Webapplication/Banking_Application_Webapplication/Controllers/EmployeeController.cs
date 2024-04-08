using Banking_Application_Webapplication;
using System;
using System.Linq;
using System.Web.Mvc;

public class EmployeeController : Controller
{

    //StudentName -: Sandeep Sandeep
    //Student Id -: 25461
    //Student Email -: 25461@student.dorset-college.ie 


    private readonly dbBankingApplicationEntities _context;

    public EmployeeController()
    {
        _context = new dbBankingApplicationEntities();
    }

    // Opening menu for Bank Employee login
    public ActionResult Index()
    {
        return View();
    }

    // Action for creating a new customer
    public ActionResult CreateCustomer()
    {
        return View();
    }

    [HttpPost]
    public ActionResult CreateCustomer(Customer customer)
    {
        if (ModelState.IsValid)
        {
            // Generate unique account numbers for savings and current accounts
            var savingsAccountNumber = GenerateAccountNumber(customer.FirstName, customer.LastName, "Savings");
            var currentAccountNumber = GenerateAccountNumber(customer.FirstName, customer.LastName, "Current");

            // Create savings account
            var savingsAccount = new Account { Type = "Savings", Balance = 0, AccountNumber = savingsAccountNumber };

            // Create current account
            var currentAccount = new Account { Type = "Current", Balance = 0, AccountNumber = currentAccountNumber };

            // Add accounts to the customer
            customer.Accounts.Add(savingsAccount);
            customer.Accounts.Add(currentAccount);

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("Index", "Employee");
        }

        return View(customer);
    }

    [HttpPost]
   
    public ActionResult DeleteCustomer(int id)
    {
        var customer = _context.Customers.Find(id);

        if (customer == null)
        {
            return HttpNotFound();
        }

        // Check if any accounts are associated with the customer
        var accounts = _context.Accounts.Where(a => a.CustomerId == id).ToList();

        foreach (var account in accounts)
        {
            // Delete associated transactions
            var transactions = _context.Transactions.Where(t => t.AccountNumber == account.AccountNumber).ToList();
            _context.Transactions.RemoveRange(transactions);

            // Delete the account
            _context.Accounts.Remove(account);
        }

        // Delete the customer
        _context.Customers.Remove(customer);

        try
        {
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Customer and associated accounts deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
        }

        return RedirectToAction("CustomerList");
    }


    // Action for listing customers and their account numbers
    public ActionResult CustomerList()
    {
        var customers = _context.Customers.ToList();
        return View(customers);
    }

    // Generate unique account number based on customer's name and account type
    private string GenerateAccountNumber(string firstName, string lastName, string accountType)
    {
        // Generate a unique account number using some logic
        // Example: Concatenate initials, customer ID, and timestamp
        var initials = firstName.Substring(0, 1) + lastName.Substring(0, 1);
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var accountId = Guid.NewGuid().ToString().Substring(0, 4); // Generate a unique ID
        return $"{initials}-{accountId}-{accountType}-{timestamp}";
    }

}
