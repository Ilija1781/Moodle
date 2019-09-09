using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telekomunikacije.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Telekomunikacije.ViewModels
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [Display(Name ="Role")]
        public  List<string> Roles { get; set; }
    }
}