using CommunityToolShedMVC.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public CustomPrincipal CustomUser
        {
            get
            {
                return (CustomPrincipal)User;
            }
        }


        // GET: Home
        public ActionResult Index()
        {

            bool isApprover = CustomUser.IsInRole(1,"Approver");
            bool isMember = CustomUser.IsInRole(1, "Member");

            return View();
        }
    }
}