using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telekomunikacije.ViewModels
{
    public class PostIndexViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }

        public DateTime Created { get; set; }
        public string PostContent { get; set; }

        public IEnumerable<PostReplyViewModel> Replies { get; set; }
    }
}