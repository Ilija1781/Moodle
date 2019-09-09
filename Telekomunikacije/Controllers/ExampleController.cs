using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telekomunikacije.Models;
using System.Data.Entity;

namespace Telekomunikacije.Controllers
{
    public class ExampleController : Controller
    {
        private ApplicationDbContext _context;

        public ExampleController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var topics = _context.Topics.Include(x=>x.Posts).ToList();
            

            return View(topics);
        }
    }
}