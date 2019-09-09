using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telekomunikacije.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Telekomunikacije.Controllers
{
    [Authorize]
    public class CommunicationController : Controller
    {
        // GET: Communication
        private ApplicationDbContext _context;

        public CommunicationController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var topics = _context.Topics.ToList();
            if (User.IsInRole("CanDoEverything"))
                return View(topics);

            return View("IndexStudent", topics);
        }
        public ActionResult New()
        {
            return View();
        }
        public ActionResult Create(Topic topic)
        {

            topic.Created = DateTime.Now;
            _context.Topics.Add(topic);
            _context.SaveChanges();
            return RedirectToAction("Index", "Communication");
        }
        public ActionResult Delete(int id)
        {
            var postReplies = _context.PostReplies.Where(x => x.Post.Topic.Id == id);
            foreach (var postReply in postReplies)
            {

                _context.PostReplies.Remove(postReply);
            }
            var posts = _context.Posts.Where(x => x.Topic.Id == id);
            foreach (var post in posts)
            {

                _context.Posts.Remove(post);
            }
            _context.Topics.Remove(_context.Topics.SingleOrDefault(x => x.Id == id));
            _context.SaveChanges();
            return RedirectToAction("Index", "Communication");
        }
        public ActionResult Posts(int id)
        {

            //  var topic = _context.Topics.Include(x=>x.Posts).SingleOrDefault(x => x.Id == id);
            var posts = _context.Posts.Where(x => x.Topic.Id == id).ToList();
            ViewBag.Topic = _context.Topics.SingleOrDefault(x => x.Id == id);
            return View(posts);
        }
        public ActionResult NewPost(int id)
        {
            var topic = _context.Topics.Single(x => x.Id == id);
            return View(topic);
        }
        public ActionResult Talk(int id)
        {

            var postReplies = _context.PostReplies.Include(x => x.User).Where(x => x.Post.Id == id);
            ViewBag.Post = _context.Posts.SingleOrDefault(x => x.Id == id);
            ViewBag.UserLogedIn = User.Identity.Name;
            return View(postReplies);
        }
        public ActionResult DeletePost(int id)
        {
            var topicId = _context.Posts.SingleOrDefault(x => x.Id == id).Topic.Id;
            var postReplies = _context.PostReplies.Include(x => x.Post).Where(x => x.Post.Id == id);
            foreach (var postReply in postReplies)
            {

                _context.PostReplies.Remove(postReply);
            }
            //_context.SaveChanges();   

            _context.Posts.Remove(_context.Posts.SingleOrDefault(x => x.Id == id));

            _context.SaveChanges();
            return RedirectToAction("Posts", "Communication", new { id = topicId });

        }
        public ActionResult CreatePost(string naslov, string sadrzaj, int topicId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var post = new Post
            {
                Title = naslov,
                Content = sadrzaj,
                Created = DateTime.Now,
                Topic = _context.Topics.SingleOrDefault(x => x.Id == topicId),
                User = _context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name)

            };
            _context.Posts.Add(post);
            _context.SaveChanges();
            return RedirectToAction("Posts", "Communication", new { id = topicId });
        }
        public ActionResult NewPostReply(int id)
        {
            //var postReply = _context.PostReplies.SingleOrDefault(x => x.Id == id);
            var post = _context.Posts.SingleOrDefault(x => x.Id == id);


            return View(post);
        }
        public ActionResult CreatePostReply(int postId, string sadrzaj)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));


            var postReply = new PostReply
            {
                Content = sadrzaj,
                Created = DateTime.Now,
                User = _context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name),
                Post = _context.Posts.SingleOrDefault(x => x.Id == postId)
            };
            _context.PostReplies.Add(postReply);
            _context.SaveChanges();

            return RedirectToAction("Talk", "Communication", new { id = postId });
        }
        public ActionResult DeletePostReply(int id)
        {
            var postId = _context.PostReplies.SingleOrDefault(x => x.Id == id).Post.Id;
            _context.PostReplies.Remove(_context.PostReplies.SingleOrDefault(x => x.Id == id));
            _context.SaveChanges();
            return RedirectToAction("Talk", "Communication", new { id = postId });
        }
    }
}