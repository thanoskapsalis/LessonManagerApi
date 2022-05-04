using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Backend.Controllers
{
  public class ClassController : Controller
  {
    [HttpPost]
    [Route("class/new")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult newClass([FromQuery] int teacherId, [FromQuery] int lessonId, [FromBody] TeachClass? teachClassRemote)
    {

      using (var db = new DataContext())
      {
        TeachClass teachClass = new TeachClass();

        if (teachClassRemote != null)
        {
          teachClass = teachClassRemote;
        }

        teachClass.lesson = db.Lessons.FirstOrDefault(sh => sh.Id == lessonId);
        teachClass.teacher = db.Teachers.FirstOrDefault(sh => sh.Id == teacherId);

        db.Update(teachClass);
        db.SaveChanges();
        return Json(new { status = "Saved" });
      }
    }

    [HttpPost]
    [Route("class/delete")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool removeClass([FromQuery] int id)
    {
      using (var db = new DataContext())
      {
        // First we delete the dilosis assosiated with that class
        List<Dilosi> dilosisToDelete = db.Dilosis
        .Include(sh => sh.teachClass)
        .Where(sh => sh.teachClass.Id == id)
        .ToList();

        // Then we delete the class
        dilosisToDelete.ForEach(sh => db.Dilosis.Remove(sh));
        db.TeachClasses.Remove(db.TeachClasses.First(sh=> sh.Id == id));
        db.SaveChanges();
        return true;
      }
    }

    [HttpGet]
    [Route("class/get")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public List<TeachClass> GetTeachClasses([FromQuery] int? id = 0, [FromQuery] int? userId = 0)
    {
      using (var db = new DataContext())
      {
        if (id != 0)
        {
          TeachClass teachClass = db.TeachClasses
            .Include(sh => sh.lesson)
            .Include(sh => sh.teacher)
            .First(sh => sh.Id == id);

          List<TeachClass> tc = new List<TeachClass>();
          tc.Add(teachClass);
          return tc;
        }

        if (userId != 0)
        {
          return db.TeachClasses
              .Include(sh => sh.teacher)
              .Include(sh => sh.lesson)
              .Include(sh => sh.teacher.user)
              .Where(sh => sh.teacher.Id == userId)
              .ToList();
        }
        return db.TeachClasses
            .Include(sh => sh.teacher)
            .Include(sh => sh.lesson)
            .ToList();
      }

    }
  }
}