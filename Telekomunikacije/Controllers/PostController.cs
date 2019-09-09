using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telekomunikacije.Models;
using Telekomunikacije.ViewModels;

namespace Telekomunikacije.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        private ApplicationDbContext _Context;

        public PostController()
        {
            _Context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _Context.Dispose();
        }
        public ActionResult Index(int Id)
        {
            var post = _Context.Posts.Single(x => x.Id == Id);

            
            var replies = BuildPostReplies(Id);
         

            var model = new PostIndexViewModel
            {
                Id=post.Id,
                AuthorId=post.User.Id,
                AuthorName=post.User.UserName,
                Created=post.Created,
                PostContent=post.Content,
                Replies=replies,
                Title=post.Title

            };
            return View(model);
        }
        public ActionResult Create(int Id)
        {

            //Id je za Topic
            var topic = _Context.Topics.Single(x => x.Id == Id);

            var model = new NewPostViewModel
            {
                TopicId = Id,
                AuthorName = User.Identity.Name,
                TopicName = topic.Title
            };
            return View(model);


        }
        public ActionResult CreateReply(int Id)
        {
            var post = _Context.Posts.Single(x => x.Id == Id);
            var model = new NewReplyNewModel
            {
                TopicId=Id,
                AuthorName=User.Identity.Name,
                PostName=post.Title
            };
            return View(model);
        }
        //[HttpPost]
        //public async Task<ActionResult> AddPostReply(NewReplyNewModel model)
        //{
        //    var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));
        //    var name = User.Identity.Name;
        //    var userId = _Context.Users.Single(x => x.UserName == User.Identity.Name).Id;
        //    var user = await manager.FindByIdAsync(userId);

        //    var post = BuildPostReply(model, user);

        //    _Context.Posts.Add(post);
        //    _Context.SaveChanges();
        //    var newId = _Context.Posts.Single(x => x.Title == post.Title).Id;
        //    return RedirectToAction("Index", "Post", new { Id = newId });

    //ovo treba da vratis
        //}

        private object BuildPostReply(NewReplyNewModel model, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> AddPost(NewPostViewModel model)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));
            var name = User.Identity.Name;
            var userId = _Context.Users.Single(x => x.UserName == User.Identity.Name).Id;
            var user = await manager.FindByIdAsync(userId);

            var post = BuildPost(model, user);

             _Context.Posts.Add(post);
            _Context.SaveChanges();
            var newId= _Context.Posts.Single(x => x.Title == post.Title).Id;
            return RedirectToAction("Index", "Post",new { Id = newId });

      
        }

        private Post BuildPost(NewPostViewModel model, ApplicationUser user)
        {
            var topic = _Context.Topics.Single(x => x.Id == model.TopicId);
            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Topic= topic
                
            };
        }

        private IEnumerable<PostReplyViewModel> BuildPostReplies(int Id)
        {
            return _Context.PostReplies.Where(x=>x.Post.Id==Id).Select(x=> new PostReplyViewModel
            {
                Id = x.Id,
                AuthorId = x.User.Id,
                AuthorName = x.User.UserName,
                Created = x.Created,
                PostId = x.Id,
                ReplyContent = x.Content
            });
           

        }
    }
}