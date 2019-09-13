using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityToolShedMVC.ViewModels
{
    public class CreateCommunityViewModel
    {
        [Required]
        public string CommunityName { get; set; }
        public string CreatorPersonId { get; set; }
        [Required]
        public bool IsOpen { get; set; }
    }
}