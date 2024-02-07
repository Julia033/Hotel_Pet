using Hotel_Pet.Models.ViewModel;
using Hotel_Pet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace Hotel_Pet.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Choice()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListOfCare()
        {
            List<Care> care = new List<Care>();
            using (var db = new Nazarenko_IDZEntities())
            {
                care = db.Cares.OrderByDescending(x => x.Price)
                                  .ThenBy(x => x.Type).ToList();
            }
            return View(care);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateCare()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult CreateCare(CareVM newCare)
        {
            if (ModelState.IsValid)
            {
                using (var context = new Nazarenko_IDZEntities())
                {
                    Care care = new Care
                    {
                        ID_care = Guid.NewGuid(),
                        Type = newCare.Type,
                        Price = newCare.Price,
                    };
                    context.Cares.Add(care);
                    context.SaveChanges();
                }
                return RedirectToAction("ListOFCare");
            }
            return View(newCare);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditCare(Guid care_id)
        {
            CareVM model;
            using (var context = new Nazarenko_IDZEntities())
            {
                Care care = context.Cares.Find(care_id);
                model = new CareVM
                {
                    ID_care = care.ID_care,
                    Type = care.Type,
                    Price = care.Price
                };
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditCare(CareVM model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new Nazarenko_IDZEntities())
                {
                    Care editedCare = new Care
                    {
                        ID_care = model.ID_care,
                        Type = model.Type,
                        Price = model.Price
                    };
                    context.Cares.Attach(editedCare);
                    context.Entry(editedCare).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                return RedirectToAction("ListOFCare");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCare(Guid care_ID)
        {
            Care careToDelete;
            using (var context = new Nazarenko_IDZEntities())
            {
                careToDelete = context.Cares.Find(care_ID);
            }
            return View(careToDelete);
        }

        [HttpPost, ActionName("DeleteCare")]
        public ActionResult DeleteConfirmed(Guid care_ID)
        {
            using (var context = new Nazarenko_IDZEntities())
            {
                Care careToDelete = new Care
                { ID_care = care_ID };
                context.Entry(careToDelete).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
            return RedirectToAction("ListOFCare");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListOfPets()
        {
            List<Pet> pets = new List<Pet>();
            using (var db = new Nazarenko_IDZEntities())
            {
                pets = db.Pets.OrderBy(x => x.Number_room)
                                  .ThenBy(x => x.Type)
                                  .ThenBy(x => x.Nickname).ToList(); ;
            }
            return View(pets);
        }

        List<(Guid ID_pet, DateTime Date_arrival, DateTime? Date_leaving)> GetHabitationList()
        {
            List<(Guid ID_pet, DateTime Date_arrival, DateTime? Date_leaving)> habitation = new List<(Guid, DateTime, DateTime?)>();
            using (var db = new Nazarenko_IDZEntities())
            {
                foreach (Habitation habitations in db.Habitations)
                {
                    habitation.Add((habitations.ID_pet, habitations.Date_arrival, habitations.Date_leaving));
                }
            }
            return habitation;
        }

        List<(Guid ID_customer, string LastName, string FirstName, string Patronymic, long Phone)> GetCustomersList()
        {
            List<(Guid ID_customer, string LastName, string FirstName, string Patronymic, long Phone)> customers = new List<(Guid, string, string, string, long)>();
            using (var db = new Nazarenko_IDZEntities())
            {
                foreach (Customer customer in db.Customers)
                {
                    customers.Add((customer.ID_customer, customer.Last_name, customer.First_name, customer.Patronymic, customer.Phone));
                }
            }
            return customers;
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult PetDetails(Guid ID_pet)
        {
            PetDetails model;
            using (var context = new Nazarenko_IDZEntities())
            {
                Pet pet = context.Pets.Find(ID_pet);
                Guid ID_customer = pet.ID_customer;

                // Получаем список клиентов
                var customersList = GetCustomersList();

                // Получаем список проживаний
                var habitationList = GetHabitationList();

                // Ищем соответствующего клиента по ID_customer
                var customer = customersList.FirstOrDefault(c => c.ID_customer == ID_customer);

                // Ищем соответствующее проживание по ID_pet
                var habitation = habitationList.FirstOrDefault(h => h.ID_pet == ID_pet);

                if (customer != default)
                {
                    model = new PetDetails()
                    {
                        ID_pet = Guid.NewGuid(),
                        Nickname = pet.Nickname,
                        Type = pet.Type,
                        Number_room = pet.Number_room,
                        Customer_FIO = customer.LastName + " " + customer.FirstName + " " + customer.Patronymic,
                        Customer_Phone = customer.Phone,
                        Date_arrival = habitation.Date_arrival,
                        Date_leaving = habitation.Date_leaving.HasValue ? habitation.Date_leaving.Value : DateTime.MinValue,
                    };
                }
                else
                {
                    // Обработка ошибки, если клиент не найден
                    model = new PetDetails()
                    {
                        ID_pet = Guid.NewGuid(),
                        Nickname = pet.Nickname,
                        Type = pet.Type,
                        Number_room = pet.Number_room,
                        Customer_FIO = "N/A",
                        Customer_Phone = 0,
                        Date_arrival = DateTime.MinValue,
                    };
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreatePet()
        {
            using (var context = new Nazarenko_IDZEntities())
            {
                var rooms = context.Rooms.Select(r => r.Number_room).ToList();

                var model = new PetVM
                {
                    AvailableRooms = rooms
                };

                return View(model);
            }
        }


        [HttpPost]
        public ActionResult CreatePet(PetVM model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new Nazarenko_IDZEntities())
                {
                    // Создаем нового владельца
                    Customer newCustomer = new Customer
                    {
                        ID_customer = Guid.NewGuid(),
                        Last_name = model.Customer_LastName,
                        First_name = model.Customer_FirstName,
                        Patronymic = model.Customer_Patronymic,
                        Phone = model.Customer_Phone,
                    };

                    // Создаем нового питомца
                    Pet newPet = new Pet
                    {
                        ID_pet = Guid.NewGuid(),
                        Nickname = model.Nickname,
                        Type = model.Type,
                        Number_room = model.Number_room,
                        ID_customer = newCustomer.ID_customer // Связываем питомца с владельцем
                    };

                    // Создаем новую запись
                    Habitation newlist = new Habitation
                    {
                        ID_list = Guid.NewGuid(),
                        Date_arrival = model.Date_arrival,
                        Date_leaving = model.Date_leaving.HasValue ? model.Date_leaving.Value : (DateTime?)null,
                        ID_pet = newPet.ID_pet
                    };

                    // Сохраняем владельца и питомца в базе данных
                    context.Customers.Add(newCustomer);
                    context.Pets.Add(newPet);
                    context.Habitations.Add(newlist);
                    context.SaveChanges();

                    // Перенаправляем пользователя на страницу с информацией о новом питомце
                    return RedirectToAction("ListOfPets", new { ID_pet = newPet.ID_pet });
                }
            }

            // Если модель не прошла валидацию, возвращаем пользователя на страницу с формой
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePet(Guid ID_pet)
        {
            using (var context = new Nazarenko_IDZEntities())
            {
                // Находим питомца по ID
                Pet petToDelete = context.Pets.Find(ID_pet);

                if (petToDelete != null)
                {
                    // Находим и удаляем связанные записи в таблицах Customers и Habitations
                    Customer customerToDelete = context.Customers.Find(petToDelete.ID_customer);
                    Habitation habitationToDelete = context.Habitations.FirstOrDefault(h => h.ID_pet == ID_pet);

                    if (customerToDelete != null)
                    {
                        context.Customers.Remove(customerToDelete);
                    }

                    if (habitationToDelete != null)
                    {
                        context.Habitations.Remove(habitationToDelete);
                    }

                    // Удаляем питомца
                    context.Pets.Remove(petToDelete);

                    // Сохраняем изменения
                    context.SaveChanges();
                }
            }

            // Перенаправляем пользователя на страницу со списком питомцев или другую подходящую страницу
            return RedirectToAction("ListOfPets");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPet(Guid ID_pet)
        {
            using (var context = new Nazarenko_IDZEntities())
            {
                // Находим питомца по ID
                Pet petToEdit = context.Pets.Find(ID_pet);
                Habitation habitation = context.Habitations.FirstOrDefault(h => h.ID_pet == ID_pet);
                Guid ID_customer = petToEdit.ID_customer;
                Customer customer = context.Customers.FirstOrDefault(c => c.ID_customer == ID_customer);

                // Проверка на наличие питомца
                if (petToEdit == null)
                {
                    return HttpNotFound();
                }

                // Создаем модель представления для редактирования
                PetVM model = new PetVM
                {
                    ID_pet = petToEdit.ID_pet,
                    Nickname = petToEdit.Nickname,
                    Type = petToEdit.Type,
                    Number_room = petToEdit.Number_room,
                    Date_arrival = habitation.Date_arrival,
                    Date_leaving = habitation?.Date_leaving ?? DateTime.MinValue,
                    Customer_LastName = customer.Last_name,
                    Customer_FirstName = customer.First_name,
                    Customer_Patronymic = customer.Patronymic,
                    Customer_Phone = customer.Phone
                };

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditPet(PetVM model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new Nazarenko_IDZEntities())
                {
                    // Находим питомца по ID
                    Pet petToEdit = context.Pets.Find(model.ID_pet);
                    Guid ID_customer = petToEdit.ID_customer;
                    

                    // Проверка на наличие питомца
                    if (petToEdit == null)
                    {
                        return HttpNotFound();
                    }

                    // Обновляем данные питомца
                    petToEdit.Number_room = model.Number_room;

                    // Обновляем данные по проживанию
                    Habitation habitationToEdit = context.Habitations
                        .FirstOrDefault(h => h.ID_pet == model.ID_pet);

                    Customer customer = context.Customers.FirstOrDefault(c => c.ID_customer == ID_customer);

                    if (habitationToEdit != null)
                    {
                        habitationToEdit.Date_leaving = model.Date_leaving.HasValue ? model.Date_leaving.Value : (DateTime?)null;
                    }

                    // Сохраняем изменения
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при сохранении изменений: {ex.Message}");

                        throw;
                    }

                    // Перенаправляем пользователя на страницу с информацией о питомце
                    return RedirectToAction("ListOfPets");
                }
            }

            // Если модель не прошла валидацию, возвращаем пользователя на страницу с формой
            return View(model);
        }










    }

}