using System;
using System.Collections.Generic;
using System.Text;

namespace TODO_V2.Shared.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }

        public string State { get; set; }
        public string TaskName { get; set; }
        public DateOnly CreationDate { get; set; }
        public DateOnly ExpirationDate { get; set; }
    }
}
