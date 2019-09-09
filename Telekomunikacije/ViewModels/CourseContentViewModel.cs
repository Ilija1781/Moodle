using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telekomunikacije.Models;

namespace Telekomunikacije.ViewModels
{
    public class CourseContentViewModel
    {
        public Course Course { get; set; }
        public FileModel FileModel { get; set; }
    }
}