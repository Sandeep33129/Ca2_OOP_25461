using System.Linq;
using System.Web.Mvc;

namespace Banking_Application_Webapplication.Controllers
{
    public class HomeController : Controller
    {
        // Main menu and user login functionality

        //StudentName -: Sandeep Sandeep
        //Student Id -: 25461
        //Student Email -: 25461@student.dorset-college.ie 
        dbBankingApplicationEntities db = new dbBankingApplicationEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userType)
        {
            if (userType == "BankEmployee")
            {
                return RedirectToAction("EmployeeLogin");
            }
            else if (userType == "Customer")
            {
                return RedirectToAction("CustomerLogin");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid user type.";
                return View();
            }
        }

        // Bank employee login
        public ActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeLogin(string pin)
        {
            if (pin == "A1234")
            {
                return RedirectToAction("Index","Employee");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid PIN.";
                return View();
            }
        }

        // Customer login
        public ActionResult CustomerLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerLogin(string firstName, string lastName, string accountNumber, string pin)
        {
            // Validate customer login
            var customer = db.Customers.FirstOrDefault(c =>
                c.FirstName == firstName && c.LastName == lastName &&
                c.PIN == pin);

            if (customer != null)
            {
                return RedirectToAction("Dashboard", "Customer", new { id = customer.Id });
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login credentials.";
                return View();
            }
        }

    }
}
