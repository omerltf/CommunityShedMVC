using CommunityToolShedMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            var viewModel = new LoginViewModel();
            return View(viewModel);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel viewModel)
        {

            return View(viewModel);
        }
    }
}