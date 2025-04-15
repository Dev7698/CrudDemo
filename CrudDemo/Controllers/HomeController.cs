using CrudDemo.Models;
using CrudDemo.Models.ViewModels;
using CrudDemo.Repository;
using CrudDemo.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CrudDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        EmployeeContext _context = new EmployeeContext();
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _employeeRepository = new EmployeeRepository(_context, _configuration);
        }

        public IActionResult Index()
        {
            var data = _employeeRepository.GetEmployees();
            return View(data);
        }

        [Route("addeditemployee")]
        public IActionResult addeditemployee(int? EmpId)
        {
            EmployeeModel model = new EmployeeModel();
            ViewBag.Department = new List<SelectListItem>()
            {
                new SelectListItem(){Text="Developer",Value = "Developer"},
                new SelectListItem(){Text="Designer",Value = "Designer"},
                new SelectListItem(){Text="Marketing",Value = "Marketing"}
            };
            if(EmpId > 0)
            {
                model = _employeeRepository.GetSingleEmployee(EmpId??0);
            }
            return View(model);
        }
        [HttpDelete]
        [Route("Employee/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                // Call the repository to delete the employee
                bool isDeleted = _employeeRepository.DeleteEmployee(id);

                if (isDeleted)
                {
                    return Ok(new { message = "Employee deleted successfully." });
                }
                else
                {
                    return NotFound(new { message = "Employee not found." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost]
        public CommonResModel SaveEmployee([FromForm]SaveEmployeeModel req)
        {
            return _employeeRepository.SaveEmployee(req);
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
