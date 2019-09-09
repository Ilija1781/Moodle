using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telekomunikacije.ViewModels;
using Telekomunikacije.Models;

namespace Telekomunikacije.ViewModels
{
    public class TopicPostsViewModel
    {
     public TopicListingViewModel Topic { get; set; }
    public IEnumerable<PostListingViewModel> Posts { get; set; }

    }
}