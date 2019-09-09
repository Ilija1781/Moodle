using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telekomunikacije.ViewModels
{
    public class NewReplyNewModel
    {
        public int TopicId { get; set; }
        public string AuthorName { get; set; }
        public string PostName { get; set; }

        public string Content { get; set; }
        public string Title { get; set; }

    }
}