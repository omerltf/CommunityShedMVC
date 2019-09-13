using CommunityToolShedMVC.Data;
using CommunityToolShedMVC.Models;
using CommunityToolShedMVC.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Data.SqlClient;
using System.Threading;

namespace CommunityToolShedMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest()
        {
            IPrincipal user = HttpContext.Current.User;
            if (user.Identity.IsAuthenticated && user.Identity.AuthenticationType == "Forms")
            {
                FormsIdentity formsIdentity = (FormsIdentity)user.Identity;
                FormsAuthenticationTicket ticket = formsIdentity.Ticket;

                CustomIdentity customIdentity = new CustomIdentity(ticket);
                string currentUserEmail = ticket.Name;
                Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    select PersonId, Name, Email
                    from Person
                    where Email=@Email
                ",
                    new SqlParameter("@Email", currentUserEmail));

                person.Roles=DatabaseHelper.Retrieve<CommunityRole>(@"
                    select pc.CommunityId, r.RoleName
                    from PersonCommunity pc
                    join PersonCommunityRole pcr on pcr.PersonCommunityId = pc.PersonCommunityId
                    join Role r on r.RoleId = pcr.RoleId
                    where pc.PersonId = @PersonId
                    order by pc.CommunityId, pcr.RoleId 
                ",
                new SqlParameter("@PersonId", person.PersonId));

                CustomPrincipal customPrincipal = new CustomPrincipal(customIdentity, person);
                HttpContext.Current.User = customPrincipal;
                Thread.CurrentPrincipal = customPrincipal;
            }
        }
    }
}
