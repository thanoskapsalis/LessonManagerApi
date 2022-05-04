using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
  public class DilosiController : Controller
  {
    [HttpPost]
    [Route("dilosi/new")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool newDilosi([FromQuery] int studentId, [FromBody] List<int> teachClasses)
    {
      using (var db = new DataContext())
      {
        // Find the student that made the new request at the server
        Student user = db.Students.First(sh => sh.Id == studentId);

        // Find the classes requested to make the appropriate dilosis
        foreach (int id in teachClasses)
        {
          TeachClass teachClass = db.TeachClasses.First(sh => sh.Id == id);
          Dilosi dilosi = new Dilosi()
          {
            student = user,
            teachClass = teachClass
          };
          db.Add(dilosi);
          db.SaveChanges();
        }
        return true;
      }
    }

    [HttpPost]
    [Route("dilosi/update")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool updateDilosi([FromBody] List<Dilosi> dilosis)
    {
      using (var db = new DataContext())
      {
        dilosis.ForEach(sh => db.Update(sh));
        db.SaveChanges();
      }
      return true;

    }

    [HttpGet]
    [Route("dilosi/get")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public List<Dilosi> GetDilosis([FromQuery] int? studentId = 0, [FromQuery] int? teacherId = 0, [FromQuery] int ? lessonId = 0)
    {
      using (var db = new DataContext())
      {
        if (teacherId != 0)
        {
          return db.Dilosis
          .Include(sh => sh.student)
          .Include(sh => sh.teachClass)
          .Include(sh => sh.teachClass.lesson)
          .Include(sh => sh.teachClass.teacher)
          .Where(sh => sh.teachClass.teacher.Id == teacherId)
          .ToList();
        }

        if (studentId != 0)
        {
          return db.Dilosis
              .Include(sh => sh.student)
              .Include(sh => sh.teachClass)
              .Include(sh => sh.teachClass.lesson)
              .Where(sh => sh.student.Id == studentId)
              .ToList();
        }

        if (lessonId !=0)
        {
          return db.Dilosis
          .Include(sh => sh.teachClass)
          .Include(sh => sh.teachClass.lesson)
          .Where(sh => sh.teachClass.lesson.Id == lessonId)
          .ToList();
        }

        return db.Dilosis
          .Include(sh => sh.student)
          .Include(sh => sh.teachClass)
          .Include(sh => sh.teachClass.lesson)
          .Include(sh => sh.teachClass.teacher)
          .ToList();
      }
    }
  }
}