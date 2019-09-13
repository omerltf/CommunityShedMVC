using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMVC.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        public List<CommunityRole> Roles { get; set; } 
    }
}