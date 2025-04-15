using System;
using System.Collections.Generic;

namespace CrudDemo.Models;

public partial class TblEmployee
{
    public int EmpId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Image { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Department { get; set; }
}
