using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telekomunikacije.ViewModels
{
    public class NewPostViewModel
    {
        public string TopicName { get; set; }
        public string AuthorName { get; set; }
        public int TopicId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

    }
}