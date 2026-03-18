
using EventGo.Models;
using System.ComponentModel.DataAnnotations;

namespace EventGo.Models

{
    public class RegisterViewModel
    {
        [Key]
        public int id { get; set; }
        [Display (Name ="English Full Name")]
        public string? FullName { get; set; }
        [Display(Name ="User Name")]
        [Required(ErrorMessage = "Please Enter User Name")]
        public string Username { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage ="Please Enter Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Retype Password")]
        [Compare("Password",ErrorMessage ="Not Matched")]
        public string ConfirmPassword { get; set; }
}


public class LoginViewModel1
{
    [Key]
    public int id { get; set; }
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Please Enter Email Address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Please Enter Password")]
    [DataType(DataType.Password)]

    public string Password { get; set; }
    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }


}
public class RoleViewModel
{
    [Key] // For Usage of Scaffolding
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
}

//--------------Create Class For Assign role to User
public class UserRoleViewModel
{
    [Key]
    public string UserID { get; set; }
    public string RoleId { get; set; }
    public string UserName { get; set; }
    public MyUser Users { get; set; }
    public MyRole Roles { get; set; }
}


}


