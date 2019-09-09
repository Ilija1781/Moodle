using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Telekomunikacije.Models;
using System.Data.Entity;

namespace Telekomunikacije.Controllers.Api
{
    public class DiscussionController : ApiController
    {
        private ApplicationDbContext _Context;

        public DiscussionController()
        {
            _Context = new ApplicationDbContext();
        }
        //GET /api/data/1
        public Topic GetById(int Id)
        {
            var topic = _Context.Topics.SingleOrDefault(x => x.Id == Id);
            if (topic == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return topic;
        }
        //GET /api/data
        public IEnumerable<Topic> GetAll()
        {

            return _Context.Topics.Include(x => x.Posts);

        }
        //GET / api/data
        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            return _Context.Users;
        }
        //POST /api/data
        [HttpPost]
        public Topic Create(Topic topic)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            _Context.Topics.Add(topic);
            _Context.SaveChanges();

            return topic;

        }

        //DELETE /api/data/1
        [HttpDelete]
        public void Delete(int topicId)
        {
            var topicInDb = _Context.Topics.SingleOrDefault(x => x.Id == topicId);
            if (topicInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _Context.Topics.Remove(topicInDb);
            _Context.SaveChanges();

        }
        //PUT /api/data/1
        [HttpPut]
        public void UpdateTopicTitle(int topicId, string newTitle)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var topicInDb = _Context.Topics.SingleOrDefault(x => x.Id == topicId);
            if (topicInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            topicInDb.Title = newTitle;

            _Context.SaveChanges();

        }
        [HttpPut]
        public void UpdateTopicDescription(int topicId, string newDescription)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var topicInDb = _Context.Topics.SingleOrDefault(x => x.Id == topicId);
            if (topicInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            topicInDb.Description = newDescription;

            _Context.SaveChanges();

        }
    }
}
