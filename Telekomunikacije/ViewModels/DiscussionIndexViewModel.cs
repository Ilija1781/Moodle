using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telekomunikacije.ViewModels
{
    public class DiscussionIndexViewModel
    {
        public IEnumerable<TopicListingViewModel> TopicList { get; set; }
    }
}