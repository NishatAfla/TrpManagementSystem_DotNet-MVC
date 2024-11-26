using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrpManagementSystem.EF;

namespace TrpManagementSystem.Auth
{
    public class Admin : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = (loginTbl)httpContext.Session["user"];
            if (user != null && user.Role == 2)
            {
                return true;
            }
            return false;

        }
    }
}