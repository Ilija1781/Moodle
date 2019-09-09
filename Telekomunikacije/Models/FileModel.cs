using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Telekomunikacije.Models;

namespace Telekomunikacije.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string Description { get; set; }
        public Purpose Purpose { get; set; }
       public byte PurposeId { get; set; }


    }
}