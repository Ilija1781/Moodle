using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telekomunikacije.ViewModels
{
    public class PostListingViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        
        public DateTime DatePosted { get; set; }
        public int TopicId { get; set; }
        public int RepliesCount { get; set; }
        public TopicListingViewModel Topic{get;set;}
    }

}