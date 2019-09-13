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
            CustomPrincipal currentUser = (CustomPrincipal)User;
            ViewBag.isReviewer = currentUser.IsInRole(communityId, "Reviewer");

            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                select CommunityName
                from Community
                where CommunityId = @CommunityId
            ",
            new SqlParameter("@CommunityId", communityId));
            ViewBag.CommunityName = community.CommunityName;
            
            tools = DatabaseHelper.Retrieve<Tool>(@"
                    select t.ToolId, t.ToolName, t.Purpose, t.Age, t.Warnings, t.CommunityId,
	                case when bt.CheckedOutOn is not null and bt.ReturnedOn is null
	                then cast(1 as bit) else cast (0 as bit)
	                end as IsOut, p.Name as BorrowerName, bt.DueOn
                    from Tool t
                    left join BorrowedTool bt on bt.ToolId = t.ToolId
	                and bt.ReturnedOn is null
                    left join Person p on p.PersonId = bt.BorrowerPersonId
                    where t.CommunityId=@CommunityId
                ", new SqlParameter("@CommunityId", communityId));
            return View(tools);
        }

        //post
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Add Code to Delete here
            return RedirectToAction("Owner");
        }

        //Get
        public ActionResult BorrowedTools(int? id, int communityId)
        {
            tools = DatabaseHelper.Retrieve<Tool>(@"
                    select t.ToolId, t.ToolName, bt.Id as BorrowedToolId,
	                case when bt.CheckedOutOn is not null and bt.ReturnedOn is null
	                then cast(1 as bit) else cast (0 as bit)
	                end as IsOut, p.Name as BorrowerName, bt.DueOn
                    from Tool t
                    left join BorrowedTool bt on bt.ToolId = t.ToolId
	                and bt.ReturnedOn is null
                    left join Person p on p.PersonId = bt.BorrowerPersonId
                    where t.CommunityId=@CommunityId
                ", new SqlParameter("@CommunityId", communityId));
            return View(tools);
        }

        //POST
        [HttpPost]
        public ActionResult Return(int id)
        {
            DatabaseHelper.Update(@"
                update BorrowedTool set
                    ReturnedOn = @ReturnedOn
                where Id = @Id
            ",
                new SqlParameter("@Id", id),
                new SqlParameter("@ReturnedOn", DateTime.Now));
            return RedirectToAction("Index");

        }

        //Get
        public ActionResult Borrow(int? id)
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int currentUserId = currentUser.Person.PersonId;
            BorrowToolViewModel viewModel = new BorrowToolViewModel();
            if (id != null)
            {
                Tool tool = DatabaseHelper.RetrieveSingle<Tool>(@"
                    select ToolName
                    from Tool
                    where ToolId=@ToolId
                ",
                new SqlParameter("@ToolId", id));

                viewModel.ToolId = (int)id;
                viewModel.ToolName = tool.ToolName;
                viewModel.BorrowerPersonId = currentUserId;
                viewModel.BorrowerName = currentUser.Person.Name;
                viewModel.CheckedOutOn = DateTime.Now;
                viewModel.DueOn = DateTime.Now.AddDays(7);
            }
            return View(viewModel);
        }

        //POST
        [HttpPost]
        public ActionResult Borrow(BorrowToolViewModel viewModel, int? id)
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int currentUserId = currentUser.Person.PersonId;
            if (id != null)
            {
                Tool tool = DatabaseHelper.RetrieveSingle<Tool>(@"
                    select ToolName
                    from Tool
                    where ToolId=@ToolId
                ",
                new SqlParameter("@ToolId", id));

                viewModel.ToolId = (int)id;
                viewModel.ToolName = tool.ToolName;
                viewModel.BorrowerPersonId = currentUserId;
                viewModel.BorrowerName = currentUser.Person.Name;
                viewModel.CheckedOutOn = DateTime.Now;
                viewModel.DueOn = DateTime.Now.AddDays(7);
            }

            if (ModelState.IsValid)
            {
                DatabaseHelper.Insert(@"
                insert into BorrowedTool (ToolId, BorrowerPersonId, CheckedOutOn, DueOn)
                values (@ToolId, @BorrowerPersonId, @CheckedOutOn, @DueOn)
                
            ",
                new SqlParameter("@ToolId", viewModel.ToolId),
                new SqlParameter("@BorrowerPersonId", viewModel.BorrowerPersonId),
                new SqlParameter("@CheckedOutOn", viewModel.CheckedOutOn),
                new SqlParameter("@DueOn", viewModel.DueOn));

                return RedirectToAction("Index", "Tools");
            }
            return View(viewModel);
        }

        //Get:
        public ActionResult Owner(int? id, int communityId)
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int currentUserId = currentUser.Person.PersonId;
            tools = DatabaseHelper.Retrieve<Tool>(@"
                select ToolName, ToolId, Age
                from Tool
                where CommunityId = @CommunityId
                and OwnerPersonId = @OwnerPersonId
            ",
            new SqlParameter("@CommunityId", communityId),
            new SqlParameter("@OwnerPersonId", currentUserId));


            return View(tools);
        }

        //Post
        [HttpPost]
        public ActionResult Delete(int? id, int toolId)
        {

            return View();
        }

        //Get
        public ActionResult Create(int communityId)
        {
            CreateToolViewModel viewModel = new CreateToolViewModel();
            viewModel.CommunityId = communityId;
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