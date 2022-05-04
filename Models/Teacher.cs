using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Teacher
    {
        [Key]
        public int Id {get; set;}
        public String firstName {get; set;}
        public String lastname {get; set;}
        public String grade {get; set;}
        public User user {get; set;}
    }
}