
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Hotel_Pet.Models.ViewModel
{
    public class PetDetails
    {
        public System.Guid ID_pet { get; set; }
        [Required]
        [DisplayName("Кличка")]
        [StringLength(50, MinimumLength = 2)]
        public string Nickname { get; set; }
        [Required]
        [DisplayName("Вид")]
        [StringLength(50, MinimumLength = 2)]
        public string Type { get; set; }
        [Required]
        [DisplayName("Владелец")]
        [StringLength(50, MinimumLength = 2)]
        public string Customer_FIO { get; set; }
        [Required]
        [DisplayName("Номер комнаты")]
        public int Number_room { get; set; }
        [Required]
        [DisplayName("Номер телефона владельца")]
        public long Customer_Phone { get; set; }
        [Required]
        [DisplayName("Дата въезда")]
        public DateTime Date_arrival { get; set; }

        [DisplayName("Дата выезда")]
        public DateTime Date_leaving { get; set; }
    }

}