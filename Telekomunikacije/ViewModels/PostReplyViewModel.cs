using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telekomunikacije.ViewModels
{
    public class PostReplyViewModel
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName  { get; set; }
        public DateTime Created { get; set; }
        public string ReplyContent { get; set; }
        public int PostId { get; set; }

    }
}