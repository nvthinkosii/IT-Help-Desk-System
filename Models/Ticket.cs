using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDeskSystem.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Status { get; set; } = "";
        public int CreatedBy { get; set; }
        public int? AssignedTo { get; set; }
        public string ResolutionNotes { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; } = "";
        public string AssignedToName { get; set; } = "";
    }
}
