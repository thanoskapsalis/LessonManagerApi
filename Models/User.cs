using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
  public class User
  {
    [Key]
     public int Id {get; set;}
     public string username {get; set;}
     public string password {get; set;}
     public string role {get; set;}

     public string email {get; set;}
  }
}