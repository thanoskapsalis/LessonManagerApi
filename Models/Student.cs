using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Student
    {
        [Key]
        public int Id {get; set;}
        public string firstName {get; set;}
        public string lastname {get; set;}
        public int yearEntered {get; set;}
        public User user {get; set;}
        

    }
}