using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using lessonApi.libs;

namespace Backend.Controllers
{
  public class UserController : Controller
  {

     public UserController() {
       // If no admin User exist in database then create one by default
       using(var db = new DataContext()) {
         PasswordManager pm = new PasswordManager();
         var admin = db.Users.FirstOrDefault(sh => sh.username == "admin");
         if (admin == null) {
           // then no admin is in the app 
           User user = new User() {
             username = "admin",
             password = pm.EncryptPassword("admin"),
             role = "admin",
             email= "admin@admin.gr"
           };
           db.Add(user);
           db.SaveChanges();
         }
       }

     }

    [HttpPost]
    [Route("User/register")]
    [ApiExplorerSettings(IgnoreApi = true)]
    /**
     * Adds a new user to the system and returns his id 
     * the Creation fails if user already exists
     */
    public JsonResult AddNewUser([FromBody] User user)
    {
      string password = user.password;
      PasswordManager pm = new PasswordManager();
      user.password = pm.EncryptPassword(password);

      User? finalUser = null;
      // first we check if this user already exists in database
      using (var db = new DataContext())
      {

        if (db.Users.FirstOrDefault(sh => sh.username == user.username) != null)
        {
          Response.StatusCode = (int)HttpStatusCode.Forbidden;
          return Json(new { });
        }
        // Then we add the new User
        db.Users.Add(user);
        db.SaveChanges();

        // Now load the user in order to get the id that the database provider
        finalUser = db.Users.First(sh => sh.username == user.username);
      }

      // User has craeted successfully so we have to send the id back to the user 
      Response.StatusCode = (int)HttpStatusCode.Accepted;
      return Json(new { userId = finalUser.Id });
    }

    [HttpPost]
    [Route("user/newTeacher")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult CreateTecher([FromBody] Teacher teacher, [FromQuery] int? id = 0)
    {
      PasswordManager pm = new PasswordManager();
      using (var db = new DataContext())
      {

        if (id != 0)
        {
          // First we load the user so we can match it on the teacher that is meant to be created
          User user = db.Users.First(sh => sh.Id == id);
          teacher.user = user;
        }
        else
        {
          // Create a new user for the Student in order to login when he gets the account 
          Random random = new Random();
          string username = "teacher" + random.Next();
          User user = new User()
          {
            username = username,
            password =  pm.EncryptPassword("temppasswd"),
            email = username + "@aegean.gr",
            role = "teacher"
          };
          db.Users.Add(user);
          db.SaveChanges();

          teacher.user = user;
          teacher.grade = "epikouros";
        }
        // Then we save the new teacher with the user that goes with 
        db.Teachers.Add(teacher);
        db.SaveChanges();
      }

      Response.StatusCode = (int)HttpStatusCode.Accepted;
      return Json(new { });
    }

    [HttpPost]
    [Route("teachers/set")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool setMultipleTeachers([FromBody] List<Teacher> teachers)
    {
      using (var db = new DataContext())
      {
        teachers.ForEach(sh => db.Teachers.Update(sh));
        db.SaveChanges();
        return true;
      }
    }

    [HttpPost]
    [Route("teacher/delete")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool deleteTeacher([FromQuery] int? id)
    {
      using (var db = new DataContext())
      {
        Teacher todelete = db.Teachers.Include(sh => sh.user).First(sh => sh.Id == id);
        db.Users.Remove(todelete.user);
        db.Teachers.Remove(todelete);
        db.SaveChanges();
        return true;
      }
    }


    [HttpPost]
    [Route("user/newStudent")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult CreateStudent([FromBody] Student student, [FromQuery] int? id = 0)
    {
      PasswordManager pm = new PasswordManager();
      using (var db = new DataContext())
      {
        if (id != 0)
        {
          User user = db.Users.First(sh => sh.Id == id);
          student.user = user;
        }
        else
        {
          // Create a new user for the Student in order to login when he gets the account 
          Random random = new Random();
          string username = "icsd" + random.Next();
          User user = new User()
          {
            username = username,
            password = pm.EncryptPassword("temppasswd"),
            email = username + "@aegean.gr",
            role = "student"
          };
          db.Users.Add(user);
          db.SaveChanges();

          // Pass the new User to the student
          student.user = db.Users.First(sh => sh.username == username);
        }

        db.Students.Add(student);
        db.SaveChanges();
      }

      Response.StatusCode = (int)HttpStatusCode.Accepted;
      return Json(new { });
    }

    [HttpPost]
    [Route("students/set")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool setMultipleStudents([FromBody] List<Student> students)
    {
      using (var db = new DataContext())
      {
        students.ForEach(sh => db.Students.Update(sh));
        db.SaveChanges();
        return true;
      }
    }

    [HttpPost]
    [Route("student/delete")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool deleteStudent([FromQuery] int? id)
    {
      using (var db = new DataContext())
      {
        Student todelete = db.Students.Include(sh => sh.user).First(sh => sh.Id == id);
        db.Users.Remove(todelete.user);
        db.Students.Remove(todelete);
        db.SaveChanges();
        return true;
      }
    }

    [Route("User/Login")]
    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult Login([FromBody] User user)
    {
      PasswordManager pm = new PasswordManager();
      User? selectedUser;
      using (var db = new DataContext())
      {
        selectedUser = db.Users.FirstOrDefault(sh =>
                 sh.username == user.username &&
                 sh.password == pm.EncryptPassword(user.password)
                 );
        if (selectedUser == null)
        {
          Response.StatusCode = (int)HttpStatusCode.Forbidden;
          return Json(new { message = "Wrong username or password" });
        }

        Response.StatusCode = (int)HttpStatusCode.OK;

        // Getting Data from teacher/student/admin
        switch (selectedUser.role)
        {
          case "teacher":
            return Json(new { role = "teacher", info = db.Teachers.First(sh => sh.user.Id == selectedUser.Id) });
          case "student":
            return Json(new { role = "student", info = db.Students.First(sh => sh.user.Id == selectedUser.Id) });
        }
        return Json(new { role = "admin", info = "all" });

      }
    }

    [HttpGet]
    [Produces("appliation/json")]
    [Route("user/teachers")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult GetAllTeachers([FromQuery] int? id = 0)
    {
      List<Teacher>? results = null;
      using (var db = new DataContext())
      {
        if (id != 0)
        {
          return Json(db.Teachers.Include(sh=>sh.user).First(sh => sh.Id == id));
        }
        results = db.Teachers.Include(sh => sh.user).ToList();
      }

      return Json(new { results });
    }

    [HttpGet]
    [Produces("appliation/json")]
    [Route("user/students")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public JsonResult getAllStudents([FromQuery] int? id = 0)
    {
      List<Student>? results = null;
      using (var db = new DataContext())
      {
        if (id != 0)
        {
          return Json(db.Students.Include(sh=>sh.user).First(sh => sh.Id == id));
        }
        results = db.Students.Include(sh => sh.user).ToList();
      }

      return Json(new { results });
    }
  }
}