using CrudDemo.Models.ViewModels;

namespace CrudDemo.Repository.Interface
{
    public interface IEmployeeRepository
    {
        bool DeleteEmployee(int id);
        List<EmployeeModel> GetEmployees();
        EmployeeModel GetSingleEmployee(int EmpId);
        CommonResModel SaveEmployee(SaveEmployeeModel req);
    }
}
