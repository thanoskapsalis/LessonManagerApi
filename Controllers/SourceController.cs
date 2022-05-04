using System.Collections.Generic;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  /**
   * Responsible for filling selectors and other type of sources
   */
  public class SourceController : Controller
  {
    [HttpGet]
    [Route("source/teachers")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public List<Object> getSelect2Teachers()
    {
      List<Object> formattedTeachers = new List<Object>();
      using (var db = new DataContext())
      {
        foreach (var teacher in db.Teachers.ToList())
        {
          formattedTeachers.Add(new { value = teacher.Id, label = teacher.firstName + " " + teacher.lastname });
        }
        return formattedTeachers;
      }
    }

    [HttpGet]
    [Route("source/lessons")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public List<Object> getSelect2Lessons()
    {
      List<Object> formattedLessons = new List<object>();
      using (var db = new DataContext())
      {
          foreach(var lesson in db.Lessons.ToList()) {
              formattedLessons.Add(new {value = lesson.Id, label = lesson.name});
          }
          return formattedLessons;
      }
    }
  }
}