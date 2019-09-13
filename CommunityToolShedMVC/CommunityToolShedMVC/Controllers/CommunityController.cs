using CommunityToolShedMVC.Data;
using CommunityToolShedMVC.Models;
using CommunityToolShedMVC.Security;
using CommunityToolShedMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace CommunityToolShedMVC.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private List<Community> MyCommunityList = new List<Community>();
        private List<Community> SearchCommunityList = new List<Community>();
        // GET: Community
        //Maybe Index will contain MyCommunities
        public ActionResult Index()
        {
            CommunityViewModel communityDetails = new CommunityViewModel();
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int currentUserPersonId = currentUser.Person.PersonId;
            communityDetails.person = currentUser.Person;
            MyCommunityList = DatabaseHelper.Retrieve<Community>(@"
                    select pc.CommunityId, pc.PersonCommunityStatusId, c.CommunityName, c.IsOpen
                    from PersonCommunity pc
                    join Community c on c.CommunityId=pc.CommunityId
                    where pc.PersonId=@PersonId
            ", new SqlParameter("@PersonId", currentUserPersonId));
            communityDetails.myCommunities = MyCommunityList;

            return View(communityDetails);
        }


        //GET
        public ActionResult Create()
        {
            var viewModel = new CreateCommunityViewModel();
            return View(viewModel);
        }

        //POST
        [HttpPost]
        public ActionResult Create (CreateCommunityViewModel viewModel)
        {
            //Get CurrentUserPersonId
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int currentUserPersonId = currentUser.Person.PersonId;

            if (ModelState.IsValid)
            {
                DatabaseHelper.Insert(@"
                    insert into Community (CommunityName, CreatorPersonId, IsOpen)
                    values (@CommunityName, @CreatorPersonId, @IsOpen)
                ",
                new SqlParameter("@CommunityName", viewModel.CommunityName),
                new SqlParameter("@CreatorPersonId", currentUserPersonId),
                new SqlParameter("@IsOpen", viewModel.IsOpen));

                return RedirectToAction("Index", "Community");
            }

            return View(viewModel);
        }

        //GET
        public ActionResult Join()
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int currentUserPersonId = currentUser.Person.PersonId;

            SearchCommunityList = DatabaseHelper.Retrieve<Community>(@"
                    select * from Community where CommunityId not in (
	                select CommunityId from PersonCommunity
	                where PersonId = @PersonId
            )",
            new SqlParameter("@PersonId", currentUserPersonId));
            return View(SearchCommunityList);
        }

        //POST
        [HttpPost]
        public ActionResult Join(int id)
        {
            CustomPrincipal currentUser = (CustomPrincipal)User;
            int currentUserPersonId = currentUser.Person.PersonId;

            DatabaseHelper.Execute(@"
            begin tran;

            insert into PersonCommunity (
                PersonId,
                CommunityId,
                PersonCommunityStatusId
            ) values (
                @PersonId,
                @CommunityId,
                2 -- Approved
            );

            declare @PersonCommunityId int;
            set @PersonCommunityId = cast(scope_identity() as int);

            insert into PersonCommunityRole (
                PersonCommunityId,
                RoleId
            ) values (
                @PersonCommunityId,
                2 -- Member
            );

            commit tran;
            ",
            new SqlParameter("@PersonId", currentUserPersonId),
            new SqlParameter("@CommunityId", id));

            return RedirectToAction("Index");
        }

        //GET
        public ActionResult Details (int id)
        {
            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                    select c.CommunityName, c.CreatorPersonId, c.IsOpen, c.CommunityId, p.Name as CreatorPersonName
                    from Community c
					left join Person p on p.PersonId = c.CreatorPersonId
                    where c.CommunityId=@CommunityId
                ", new SqlParameter("@CommunityId", id));

            return View(community);
        }

    }
}