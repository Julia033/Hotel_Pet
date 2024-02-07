using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Hotel_Pet.Models.ViewModel
{
    public class UserVM
    {
        [Required]
        [DisplayName("Логин")]
        public string Login { get; set; }
        [Required]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}