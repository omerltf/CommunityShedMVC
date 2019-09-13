using CommunityToolShedMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMVC.ViewModels
{
    public class CommunityViewModel
    {
        public Person person { get; set; }
        public List<Community> myCommunities { get; set; }
    }
}