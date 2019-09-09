using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telekomunikacije.Models;
using Telekomunikacije.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Net;

namespace Telekomunikacije.Controllers
{
    public class MembersController : Controller
    {
        // GET: Students
        private ApplicationDbContext _Context;

        public MembersController()
        {
            _Context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _Context.Dispose();
        }
        public ActionResult Index()
        {

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));
            var users = manager.Users.ToList();

            var usersViewModel = new List<UserRoleViewModel>();

            foreach (var item in users)
            {
                var pom = new UserRoleViewModel
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Roles = manager.GetRoles(item.Id).ToList()
                };

                usersViewModel.Add(pom);
            }

            return View(usersViewModel);

        }


        public ActionResult MembersInfo(string id)
        {
            var user = _Context.Users.Single(x => x.Id == id);
            return View(user);
        }

        public ActionResult RoleInfo(string Id)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));

            var user = manager.Users.Single(x => x.Id == Id);

            var roles = manager.GetRoles(Id);



            var userVM = new UserRoleViewModel
            {
                Id = Id,
                Roles = roles.ToList(),
                UserName = user.UserName
            };
            var rolesAll = new List<string>();


            foreach (var item in _Context.Roles)
            {
                var pom = 0;
                foreach (var item1 in roles)
                    if (item.Name == item1)
                    {
                        pom = 1;


                    }
                if (pom == 0)
                    rolesAll.Add(item.Name);

            }

            var list = rolesAll.ToList().Select(r => new SelectListItem { Value = r, Text = r }).ToList();

            //var list = _Context.Roles.OrderBy(x => x.Name).ToList().Select(r => new SelectListItem { Value = r.Name.ToString(), Text = r.Name }).ToList();

            ViewBag.blabla = list;
            return View(userVM);
        }

        public ActionResult AddRole(String RoleName, String Id)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));

            manager.AddToRole(Id, RoleName);



            //  System.Web.Security.Roles.AddUserToRole(usernameID, choosenRole);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteRole(String RoleName, String Id)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));



            manager.RemoveFromRole(Id, RoleName);

            return RedirectToAction("Index");
        }


        public async Task<ActionResult> DeleteUser(string id)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));
            var user = await manager.FindByIdAsync(id);

            var postReplies = _Context.PostReplies.Where(x => x.User.Id == user.Id).ToList();
            foreach (var postReply in postReplies)
            {
                _Context.PostReplies.Remove(postReply);
            }

            var posts = _Context.Posts.Where(x => x.User.Id == user.Id).ToList();
            foreach (var post in posts)
            {
                _Context.Posts.Remove(post);
            }

            var logins = user.Logins;

            var rolesForUser = await manager.GetRolesAsync(id);
            using (var transaction = _Context.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await manager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        var result = await manager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await manager.DeleteAsync(user);

                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(ApplicationUser user)
        {
            var userInDb = _Context.Users.SingleOrDefault(x => x.Id == user.Id);
            _Context.Users.SingleOrDefault(x => x.Id == user.Id).UserName = user.UserName;
            _Context.Users.SingleOrDefault(x => x.Id == user.Id).IndexNumber = user.IndexNumber;
            _Context.Users.SingleOrDefault(x => x.Id == user.Id).Email = user.Email;
            _Context.Users.SingleOrDefault(x => x.Id == user.Id).PhoneNumber = user.PhoneNumber;
            _Context.SaveChanges();
            return RedirectToAction("Index", "Members");

        }

        public ActionResult Pomocni()
        {

            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(_Context));
            //var user = manager.FindById(_Context.Users.Single(x => x.UserName == "Damjan").Id);

            //manager.RemoveFromRole(user.Id, "CanTeach");
            //_Context.SaveChanges();
            ////if (roles.Count() > 0)
            ////{
            ////    foreach (var item in roles.ToList())
            ////    {

            ////        manager.RemoveFromRole(user.Id, item);
            ////    }
            ////}
            //var roles = manager.GetRoles(user.Id);
            return View();

        }
    }
    //REAL SHIT
}