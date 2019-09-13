using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityToolShedMVC.ViewModels
{
    public class BorrowToolViewModel
    {
        [Required]
        public int ToolId { get; set; }
        public string ToolName { get; set; }
        [Required]
        public int BorrowerPersonId { get; set; }
        public string BorrowerName { get; set; }
        [Required]
        public DateTime CheckedOutOn { get; set; }
        [Required]
        public DateTime DueOn { get; set; }
        public DateTime ReturnedOn { get; set; }
    }
}