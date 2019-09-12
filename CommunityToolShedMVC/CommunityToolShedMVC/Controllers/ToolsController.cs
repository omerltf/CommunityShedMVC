using CommunityToolShedMVC.Data;
using CommunityToolShedMVC.Models;
using CommunityToolShedMVC.Security;
using CommunityToolShedMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMVC.Controllers
{
    [Authorize]
    public class ToolsController : Controller
    {
        private List<Tool> tools = new List<Tool>(); 
        // GET: Tools
        public ActionResult Index(int? id, int communityId)
        {
            tools = DatabaseHelper.Retrieve<Tool>(@"
                    select ToolName, ToolId
                    from Tool
                    where CommunityId=@CommunityId
                ", new SqlParameter("@CommunityId", communityId));
            return View(tools);
        }

        //Get
        public ActionResult Create(int id)
        {
            CreateToolViewModel viewModel = new CreateToolViewModel();
            viewModel.CommunityId = id;
            return View(viewModel);
        }

        //post 
        [HttpPost]
        public ActionResult Create(CreateToolViewModel viewModel)
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int ownerPersonId = currentUser.Person.PersonId;

            if (ModelState.IsValid)
            {
                DatabaseHelper.Insert(@"
                    insert into Tool (ToolName, OwnerPersonId, CommunityId, Purpose, Age, Warnings)
                    values (@ToolName, @OwnerPersonId, @CommunityId, @Purpose, @Age, @Warnings)
                ",
                new SqlParameter("@ToolName", viewModel.ToolName),
                new SqlParameter("@OwnerPersonId", ownerPersonId),
                new SqlParameter("@CommunityId", viewModel.CommunityId),
                new SqlParameter("@Purpose", viewModel.Purpose),
                new SqlParameter("@Age", viewModel.Age),
                new SqlParameter("@Warnings", viewModel.Warnings));

                return RedirectToAction("Index", "Tools");
            }

            return View(viewModel);
        }
    }
}