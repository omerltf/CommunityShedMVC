using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityToolShedMVC.Models
{
    public class Community
    {
        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string CreatorPersonId { get; set; }
        public string CreatorPersonName { get; set;}
        public bool IsOpen { get; set; }
    }
}