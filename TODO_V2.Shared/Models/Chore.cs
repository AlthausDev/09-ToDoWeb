using System;
using System.Collections.Generic;
using System.Text;

namespace TODO_V2.Shared.Models
{
    public class Chore : BaseModel
    {       
        public int CategoryID { get; set; }
        public int UserID { get; set; }

        public string State { get; set; }
        public string TaskName { get; set; }
        public DateOnly ExpirationDate { get; set; }

        public Chore()
        {
        }

        public Chore(int categoryID, int userID, string state, string taskName, DateOnly expirationDate)
        {
            CategoryID = categoryID;
            UserID = userID;
            State = state;
            TaskName = taskName;
            ExpirationDate = expirationDate;
        }
    }
}
