using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telekomunikacije.Models;
using System.Data.Entity;
using Telekomunikacije.ViewModels;

namespace Telekomunikacije.Controllers
{
    public class DiscussionController : Controller
    {
        // GET: Discussion
        private ApplicationDbContext _Context;

        public DiscussionController()
        {
            _Context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _Context.Dispose();
        }

        public ActionResult Index()
        {
            var forums = _Context.Topics.Select(x => new TopicListingViewModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Title
            });
            var model = new DiscussionIndexViewModel
            {
                TopicList = forums
            };
            return View(model);
        }

        public ActionResult Topic(int Id)
        {
            //.Include(x => x.Posts)
            var topic = _Context.Topics.Single(x => x.Id == Id);

            var posts = GetPosts(topic).ToList();

            //foreach (var item in GetPosts(topic))
            //{
            //    //.Include(x => x.User).Include(x=>x.Replies)
            //    posts.Add(_Context.Posts.SingleOrDefault(x => x.Id == item.Id));
            //}
            var postsVM = new List<PostListingViewModel>();


            //foreach (var x in posts)
            //{
            //    var postListing = new PostListingViewModel
            //    {


            //        Id = x.Id,
            //        AuthorId = x.User.Id,
            //        AuthorName = x.User.UserName,
            //        Title = x.Title,
            //        DatePosted = x.Created,
            //        TopicId = x.Topic.Id,
            //        Topic = BuildTopicListing(x),
            //        RepliesCount = x.Replies.Count()


            //    };
            //    postsVM.Add(postListing);
            //}
            
            var postListings = posts.Select(x => new PostListingViewModel
            {
                Id = x.Id,
                AuthorId = x.User.Id,
                AuthorName = x.User.UserName,
                Title = x.Title,
                DatePosted = x.Created,
                TopicId = x.Topic.Id,
                Topic = BuildTopicListing(x),
               RepliesCount = _Context.PostReplies.Where(r=>r.Post.Id == x.Id).Count()


            }).ToList();
            var model = new TopicPostsViewModel
            {
                Posts = postListings,
                Topic = BuildTopicListing(topic)
            };

            return View(model);
        }

        private IEnumerable<Post> GetPosts(Topic topic)
        {
            
            return _Context.Posts.Include(x=>x.Topic).Include(x=>x.User).Where(x => x.Topic.Id == topic.Id).ToList();
        }

        private TopicListingViewModel BuildTopicListing(Post x)
        {
            var topic = x.Topic;
            return BuildTopicListing(topic);
        }
        private TopicListingViewModel BuildTopicListing(Topic x)
        {
            
            return new TopicListingViewModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Title
            };
        }
    }
}