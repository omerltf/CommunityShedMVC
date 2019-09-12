using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMVC.Models
{
    public class Tool
    {
        public int ToolId { get; set; }
        public string ToolName { get; set; }
        public int OwnerPersonId { get; set; }
        public int CommunityId { get; set; }
        public string Purpose { get; set; }
        public string Age { get; set; }
        public string Warnings { get; set; }
    }
}