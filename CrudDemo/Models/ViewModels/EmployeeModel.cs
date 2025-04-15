namespace CrudDemo.Models.ViewModels
{
    public class EmployeeModel
    {
        public int EmpId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Image { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Department { get; set; }
    }

    public class SaveEmployeeModel
    {
        public int EmpId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public IFormFile? Image { get; set; }
        public string? Department { get; set; }
    }

    public class CommonResModel
    {
        public bool? success { get; set; }
        public string? message { get; set; }
    }
}
