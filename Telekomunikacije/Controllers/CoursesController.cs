using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telekomunikacije.Models;
using Telekomunikacije.ViewModels;
using System.Data.Entity;

namespace Telekomunikacije.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {

        private ApplicationDbContext _context;

        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        public ActionResult Index()
        {

            var courses = _context.Courses.ToList();


            if (User.IsInRole("CanDoEverything") || User.IsInRole("CanMakeCourse"))
                return View(courses);


            return View("IndexStudent", courses);
        }


        [HttpPost]
        public ActionResult Create(Course course)
        {

            if (!ModelState.IsValid)
            {

                return View("New", course);
            }
            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction("Index", "Courses");
        }

        public ActionResult Enrolment(int id)
        {


            var course = _context.Courses.Single(c => c.Id == id);

            var user = _context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);

            var CourseContentVM = new CourseContentViewModel();

            var users = _context.Courses.Single(c => c.Id == course.Id).ApplicationUsers.ToList();


            ViewBag.users = users;


            var files = GetFiles(course);

            //foreach (var item in course.FileModels)
            //{

            //    files.Add(item);
            //}



            foreach (var item in course.ApplicationUsers)
            {
                if (item.UserName == user.UserName)
                {

                    ViewBag.Files = files;
                    return RedirectToAction("CourseStudent", course);
                }
            }

            //  ViewBag.blabla = _context.FileModels.Include(x => x.Purpose).ToList();
            return View(course);
        }



        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var fileModels = _context.Courses.Single(x => x.Id == Id).FileModels.ToList();
            foreach (var fileModel in fileModels)
            {
                _context.FileModels.Remove(fileModel);
            }
            var course = _context.Courses.Single(x => x.Id == Id);
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("Index", "Courses");
        }



        [HttpPost]
        public ActionResult EnrolStudent(Course course)
        {
            var courseDb = _context.Courses.Single(c => c.Id == course.Id);

            if (courseDb.Password != course.Password)
                ModelState.AddModelError("Password", "Pogresna Lozinka,Molimo Vas Da Unesete Tacnu");


            var files = new List<FileModel>();




            if (!ModelState.IsValid)
            {

                return View("Enrolment", courseDb);
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _context.Courses.Single(x => x.Id == course.Id).ApplicationUsers.Add(_context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name));

            _context.SaveChanges();

            return RedirectToAction("CourseStudent", courseDb);

        }
        public ActionResult CourseStudent(Course course)
        {
            var users = _context.Courses.Single(c => c.Id == course.Id).ApplicationUsers.ToList();
            ViewBag.users = users;

            var filesCourse = new List<FileModel>();
            var files = GetFiles(course);


            ViewBag.Files = files.OrderBy(x => x.Id);

            if (User.IsInRole("CanDoEverything") || User.IsInRole("CanTeach"))
                return View(course);

            return View("CourseStudentStudent", course);
        }
        public ActionResult RemoveFromCourse(int Id)
        {

            _context.Courses.Single(x => x.Id == Id).ApplicationUsers.Remove(_context.Users.Single(x => x.UserName == User.Identity.Name));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult New()
        {



            return View();
        }




        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile, int id, string description, int purposeId)
        {


            var course = _context.Courses.SingleOrDefault(x => x.Id == id);



            var users = _context.Courses.Single(c => c.Id == course.Id).ApplicationUsers.ToList();
            ViewBag.users = users;



            if (postedFile == null)
                ModelState.AddModelError("postedFile", "Fajl nije izabran.");
            if (description == "")
                ModelState.AddModelError("Description", "Opis fajla je obavezan.");


            if (!ModelState.IsValid)
            {

                return View("CourseStudent", course);
            }

            byte[] bytes;

            // var url = Url.RequestContext.RouteData.Values["id"];

            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "INSERT INTO FileModels VALUES (@Name, @ContentType, @Data, @Course_Id,@Description,@PurposeId)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Name", Path.GetFileName(postedFile.FileName));
                    cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                    cmd.Parameters.AddWithValue("@Data", bytes);
                    cmd.Parameters.AddWithValue("@Course_Id", id);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@PurposeId", purposeId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }






            var files = GetFiles(course);


            ViewBag.Files = files;


            return View("CourseStudent", course);
        }


        [HttpGet]
        public FileResult DownloadFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Data, ContentType FROM FileModels WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Data"];
                        contentType = sdr["ContentType"].ToString();
                        fileName = sdr["Name"].ToString();
                    }
                    con.Close();
                }
            }

            return File(bytes, contentType, fileName);
        }
        public ActionResult DeleteFile(int fileId, int id)
        {

            var course = _context.Courses.SingleOrDefault(x => x.Id == id);
            var file = _context.FileModels.Single(x => x.Id == fileId);
            _context.FileModels.Remove(file);
            _context.SaveChanges();

            var users = _context.Courses.Single(c => c.Id == course.Id).ApplicationUsers.ToList();
            ViewBag.users = users;

            var files = GetFiles(course);
            ViewBag.Files = files;
            return View("CourseStudent", course);
        }

        private static List<FileModel> GetFiles(Course course)
        {
            List<FileModel> files = new List<FileModel>();
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, Course_Id,PurposeId,Description FROM FileModels"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            if (Convert.ToInt32(sdr["Course_Id"]) == course.Id)
                            {
                                files.Add(new FileModel
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    Name = sdr["Name"].ToString(),
                                    PurposeId = (byte)(sdr["PurposeId"]),
                                    Description = sdr["Description"].ToString()
                                });
                            }

                        }
                    }
                    con.Close();
                }
            }
            return files;
        }
    }
}

