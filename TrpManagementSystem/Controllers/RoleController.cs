using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrpManagementSystem.DTO;
using TrpManagementSystem.EF;

namespace TrpManagementSystem.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        DotnetDBEntities db = new DotnetDBEntities();


        [HttpGet]
        public ActionResult Login()
        {
            return View(new RoleDTO());
        }
        [HttpPost]
        public ActionResult Login(RoleDTO log)
        {
            if (ModelState.IsValid)
            {
                var user = (from u in db.loginTbls
                            where u.Username.Equals(log.Username)
                            && u.Password.Equals(log.Password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user;
                    return RedirectToAction("ChannelList", "TRP");
                }


            }
            return View(log);



        }
    }
}