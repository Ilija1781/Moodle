using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Telekomunikacije.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }

        
    }
}