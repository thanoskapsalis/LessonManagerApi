using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
  public class TeachClass
  {
    [Key]
    public int Id { get; set; }
    public Lesson lesson { get; set; }
    public Teacher teacher { get; set; }
    public int year { get; set; }
    public int semester { get; set; }
    public float examWeight { get; set; }
    public float labWeight { get; set; }
    public bool examMandatory { get; set; }
    public bool labMandatory { get; set; }
  }
}