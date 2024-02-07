using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Hotel_Pet.Models.ViewModel
{
    public class PetVM
    {
        public Guid ID_pet { get; set; }
        public Guid ID_customer { get; set; }

       // [Required(ErrorMessage = "Обязательное поле")]
        [DisplayName("Кличка")]
        public string Nickname { get; set; }
        
        [DisplayName("Вид")]
        //[Required(ErrorMessage = "Обязательное поле")]
        public string Type { get; set; }

        //[Required(ErrorMessage = "Обязательное поле")]
        [DisplayName("Номер комнаты")]
        public int Number_room { get; set; }
        public List<int> AvailableRooms { get; set; }

        [DisplayName("Фамилия")]
        //[Required(ErrorMessage = "Обязательное поле")]
        public string Customer_LastName { get; set; }

        [DisplayName("Имя")]
        //[Required(ErrorMessage = "Обязательное поле")]
        public string Customer_FirstName { get; set; }

        [DisplayName("Отчество")]
        public string Customer_Patronymic { get; set; }

        [DisplayName("Номер телефона")]
        //[Required(ErrorMessage = "Обязательное поле")]
        public long Customer_Phone { get; set; }

        //[Required(ErrorMessage = "Обязательное поле")]
        [DisplayName("Дата въезда")]
        public DateTime Date_arrival { get; set; }

        [DisplayName("Дата выезда")]
        public DateTime? Date_leaving { get; set; }
    }
}