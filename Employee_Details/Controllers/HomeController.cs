using Employee_Details.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Employee_Details.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeDbContext _employee;

        public HomeController(EmployeeDbContext employee)
        {
            _employee = employee;
        }


        public IActionResult MainPage()
        {
          return View();
        }

        public IActionResult Index()
        {
            var r = _employee.Employees.ToList();
            return View(r);
            
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var renter = _employee.Employees.FirstOrDefault(r => r.Email == email);
                if (renter != null && VerifyPassword(renter.Password, password))
                {
                    // Authentication successful
                    // Set authentication cookie or session

                    return RedirectToAction("Index", "Home");
                }
            }

            // If authentication fails or ModelState is invalid, display an error message
            TempData["Message"] = "Invalid email or password";
            return RedirectToAction("Login");
        }

        private bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // Compare the hashed password with the provided password
            // Implement your password hashing and comparison logic here
            // Example: return hashedPassword == Hash(providedPassword);
            return hashedPassword == providedPassword; // Example (not recommended for production)
        }


        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(Employee e)
        {
            _employee.Employees.Add(e);
            _employee.SaveChanges();
            return RedirectToAction("Login");
        }


        public IActionResult Edit(int? id)
        {
            var r = _employee.Employees.Find(id);
            return View(r);
        }
        [HttpPost]
        public IActionResult Edit(Employee e)
        {
            _employee.Employees.Update(e);
            _employee.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            var e = _employee.Employees.Find(id);
            return View(e);
        }


        public IActionResult Delete(int? id)
        {
            var r = _employee.Employees.Find(id);
            return View(r);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var r = _employee.Employees.Find(id);
            _employee.Employees.Remove(r);
            _employee.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            // Perform logout logic (clear session, authentication tokens, etc.)

            // Example: Clearing authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to a different page after logout
            return RedirectToAction("Mainpage");
        }

        public IActionResult LoggedOut()
        {
            return View(); // You can create a view for logged out confirmation
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
