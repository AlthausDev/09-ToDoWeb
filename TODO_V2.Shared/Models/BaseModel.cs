using System;
using System.Collections.Generic;
using System.Text;

namespace TODO_V2.Shared.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Update
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }


        //Delete
        public bool IsDeleted { get; set; } = false;
        public DateOnly? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
