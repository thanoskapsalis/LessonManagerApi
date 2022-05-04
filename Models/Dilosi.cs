using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
  public class Dilosi
  {
    [Key]
    public int Id { get; set; }
    public TeachClass teachClass { get; set; }
    public Student student { get; set; }
    public float examMark { get; set; }
    public float labMark { get; set; }
    public float finalMark { get; set; }

  }
}