using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
  public class Lesson
  {
      [Key]
    public int Id { get; set; }
    public String name {get; set;}
    public String description {get; set;}
    public List<Lesson> required {get; set;}  
  }
}