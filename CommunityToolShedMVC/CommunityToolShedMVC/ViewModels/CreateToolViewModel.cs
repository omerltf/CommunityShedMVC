using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityToolShedMVC.ViewModels
{
    public class CreateToolViewModel
    {
        public int ToolId { get; set; }
        public int OwnerPersonId { get; set; }
        public int CommunityId { get; set; }
        public string Purpose { get; set; }
        public string Age { get; set; }
        public string Warnings { get; set; }
        [Required]
        public string ToolName { get; set; }
        
    }
}