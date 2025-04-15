using CrudDemo.Models;
using CrudDemo.Models.ViewModels;
using CrudDemo.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CrudDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public readonly EmployeeContext _context;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(EmployeeContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public List<EmployeeModel> GetEmployees()
        {
            List<EmployeeModel> res = new List<EmployeeModel>();
            try
            {
                string connectionString = _configuration["ConnectionStrings:DBContext"];

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetEmployee", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        conn.Open();

                        // Execute the command and read the results
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Loop through the results and map them to your EmployeeModel
                            while (reader.Read())
                            {
                                EmployeeModel employee = new EmployeeModel
                                {
                                    EmpId = reader.GetInt32(reader.GetOrdinal("EmpId")),
                                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                    Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                                    CreatedDate = reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString(reader.GetOrdinal("Department"))
                                };
                                res.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public EmployeeModel GetSingleEmployee(int EmpId)
        {
            EmployeeModel res = new EmployeeModel();
            try
            {
                res = _context.TblEmployees.Where(z => z.EmpId == EmpId).Select(z => new EmployeeModel()
                {
                    CreatedDate = z.CreatedDate,
                    Department = z.Department,
                    EmpId = z.EmpId,
                    Email = z.Email,
                    Image = z.Image,
                    Name = z.Name
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public CommonResModel SaveEmployee(SaveEmployeeModel req)
        {
            CommonResModel res = new CommonResModel();
            try
            {
                string Image = "";
                if (req.Image != null)
                {
                    var directorypath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                    if (!Directory.Exists(directorypath))
                    {
                        Directory.CreateDirectory(directorypath);

                    }
                    var filePath = Path.Combine(directorypath, req.Image.FileName);
                    using (Stream stream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        req.Image.CopyTo(stream);
                        Image = req.Image.FileName;
                    }
                }
                if (req.EmpId > 0)
                {
                    var exists = _context.TblEmployees.Where(z => z.EmpId == req.EmpId).FirstOrDefault();
                    if (exists != null)
                    {
                        exists.Email = req.Email;
                        exists.Name = req.Name;
                        exists.Department = req.Department;
                        exists.Image = Image != "" ? Image : exists.Image;

                        _context.TblEmployees.Update(exists);
                        _context.SaveChanges();
                        res.success = true;
                        res.message = "Employee Updated successfully";
                    }
                    else
                    {
                        res.success = false;
                        res.message = "Employee Not found";
                    }
                }
                else
                {
                    TblEmployee emp = new TblEmployee()
                    {
                        CreatedDate = DateTime.Now,
                        Image = Image,
                        Department = req.Department,
                        Email = req.Email,
                        Name = req.Name
                    };
                    _context.TblEmployees.Add(emp);
                    _context.SaveChanges();

                    res.success = true;
                    res.message = "Employee Added successfully";
                }
            }
            catch (Exception ex)
            {
                res.success = false;
                res.message = ex.Message;
            }
            return res;
        }
        public bool DeleteEmployee(int id)
        {
            var employee = _context.TblEmployees.Find(id);
            if (employee == null)
            {
                return false;

            }

            _context.TblEmployees.Remove(employee);
            _context.SaveChanges();
            return true;
        }
    }
}
