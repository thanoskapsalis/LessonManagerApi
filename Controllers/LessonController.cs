using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Controllers
{
  public class LessonController : Controller
  {
    [HttpPost]
    [Route("lesson/add")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult NewLesson([FromBody] Lesson lesson)
    {
      using (var db = new DataContext())
      {
        db.Lessons.Update(lesson);
        db.SaveChanges();
        Response.StatusCode = (int)HttpStatusCode.OK;
        return Json(new { status = "Lesson Created" });
      }
    }

    [HttpGet]
    [Route("lesson/get")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public List<Lesson> GetLessons([FromQuery] int? id = 0, [FromQuery] int? userId = 0)
    {
      using (var db = new DataContext())
      {
        if (userId != 0)
        {
          List<TeachClass> tc = db.TeachClasses
           .Include(sh => sh.lesson)
           .Include(sh => sh.teacher)
           .Where(sh => sh.teacher.Id == userId)
           .ToList();
          List<Lesson> filteredLessons = new List<Lesson>();
          foreach (var teachClass in tc)
          {
            filteredLessons.Add(teachClass.lesson);
          }

          return filteredLessons;
        }
        if (id == 0 || userId == 0)
        {
          return db.Lessons.ToList();
        }
        else
        {
          return db.Lessons.Where(sh => sh.Id == id).ToList();
        }
      }
    }

    [HttpPost]
    [Route("lesson/delete")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult deleteLesson([FromQuery] int id)
    {
      using (var db = new DataContext())
      {
        // First we remove all the dilosis where the lesson is placced
        List<Dilosi> dilosisToDelete = db.Dilosis
        .Include(sh => sh.teachClass)
        .Include(sh => sh.teachClass.lesson)
        .Where(sh => sh.teachClass.lesson.Id == id)
        .ToList();
        dilosisToDelete.ForEach(sh => db.Dilosis.Remove(sh));

        // First we remove all the teachClasses where the lesson is placed
        List<TeachClass> teachClassesToDelete = db.TeachClasses
        .Where(sh => sh.lesson.Id == id)
        .Include(sh => sh.lesson)
        .ToList();
        teachClassesToDelete.ForEach(sh => db.TeachClasses.Remove(sh));

        // Finally we remove the lesson.
        Lesson lesson = db.Lessons.First(sh => sh.Id == id);
        db.Lessons.Remove(lesson);
        db.SaveChanges();
        return Json(new { status = "Deleted Successfully" });
      }
    }

  }
}