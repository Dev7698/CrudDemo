 

$(document).ready(function () {

  
    $("#frmEmployee").validate({
        errorElement: 'div',
        rules: {
            Name: {
                required: true
            },
            Email: {
                required: true,
                email:true
            },
            Image: {
                required: function () {
                    return !(parseInt($("#EmpId").val()) > 0)
                }
            },
            Department: {
                required: true
            }

        },
        messages: {
            Name: "Please Enter Name",
            Email: {
                required:"Please Enter Email",
                email:"Please Enter valid Email",
            },
            Image: "Please Select Image",
            Department: "Please Select Department",
        }
    });

    $("#btnSave").on('click', function () {
        var valid = $("#frmEmployee").valid();
        if (valid) {
            SaveEmployee();
        }
    });


});
function SaveEmployee() {
   
    var formData = new FormData();

    var files = $('#Image')[0].files;
    if (files.length > 0) {
        formData.append('Image', files[0]);
    }
    formData.append('EmpId', parseInt($("#EmpId").val()));
    formData.append('Name', $("#Name").val());
    formData.append('Email', $("#Email").val());
    formData.append('Department', $("#Department").val());
  
    $.ajax({
        type: "POST",
        url: "/Home/SaveEmployee",  

        data: formData, 
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {

                toastr.success(data.message, 'Success');

                setTimeout(function () {
                    window.location.href = '/'; 
 
                }, 1000); 
            } else {
               
                toastr.error(data.message || 'Something went wrong!');
            }
        },
        error: function (error) {
            console.log(error);
            toastr.error('An error occurred while processing your request.');
        }
    });
}

//delate in short

//function deleteEmployee(empId) {
//    fetch(`/Employee/Delete/${empId}`, { method: 'DELETE' })
//        .then(response => response.ok && location.reload());
//}

function deleteEmployee(empId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {  
            fetch(`/Employee/Delete/${empId}`, { method: 'DELETE' }) 
                .then((response) => {
                    if (response.ok) {
                        Swal.fire('Deleted!', 'Employee has been deleted.', 'success');
                        setTimeout(() => location.reload(), 1000); 
                    } else {
                        Swal.fire('Error!', 'There was an issue deleting the employee.', 'error');
                    }
                }) 
                .catch(() => {
                    Swal.fire('Error!', 'An error occurred while deleting the employee.', 'error');
                }); 
        }
    });
}

