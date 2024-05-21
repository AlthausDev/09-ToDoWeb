using System;
using System.ComponentModel.DataAnnotations;

namespace TODO_V2.Shared.Models.Enum
{
    public enum UserTypeEnum
    {
        [Display(Name = "ADMINISTRADOR")]
        ADMINISTRADOR,

        [Display(Name = "USUARIO")]
        USUARIO
    }
}
