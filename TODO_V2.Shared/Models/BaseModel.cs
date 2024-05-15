using System;
using System.Collections.Generic;
using System.Text;

namespace TODO_V2.Shared.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateOnly Registro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly Modificacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
