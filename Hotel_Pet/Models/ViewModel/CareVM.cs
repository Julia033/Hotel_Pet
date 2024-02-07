using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Pet.Models.ViewModel
{
    public class CareVM
    {
        public System.Guid ID_care { get; set; }
        //[Required(ErrorMessage = "Поле Название не должно быть пустым")]
        [DisplayName("Название")]
        [StringLength(100)]
        public string Type { get; set; }
        [Required(ErrorMessage = "Поле Цена не должно быть пустым")]
        [DisplayName("Цена")]
        [Range(100, 1000)]
        public string Price { get; set; }
    }
}