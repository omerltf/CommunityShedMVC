using CommunityToolShedMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CommunityToolShedMVC.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity identity;
        private Person person;

        public CustomPrincipal(CustomIdentity identity, Person person)
        {
            this.identity = identity;
            this.person = person;
        }

        public Person Person
        {
            get
            {
                return person;
            }
        }

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsInRole (int communityId, string role)
        {
            return person.Roles.Exists(r => r.CommunityId == communityId && r.RoleName == role);            //where is this looping inside person.roles????

            //Option 1
            //bool roleFound = false;

            //foreach(var communityRole in person.Roles)
            //{
            //    if (communityRole.CommunityId==communityId && communityRole.RoleName == role)
            //    {
            //        roleFound = true;
            //    }
            //}

            //return roleFound;
        }
    }
}