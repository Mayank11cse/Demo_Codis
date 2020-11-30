using Codis_Demo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codis_Demo.Controllers
{
    public class PersonDetailsController : Controller
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\PersonData.json");
        // GET: PersonDetails
        public ActionResult Index()
        {
          
            List<PersonDetails> persons = JsonConvert.DeserializeObject<List<PersonDetails>>(System.IO.File.ReadAllText(path));
            TempData["PersonList"] = persons;
           
            return View("Details",persons);
        }

        // GET: PersonDetails
        public ActionResult Search(string filter)
        {

            List<PersonDetails> persons = JsonConvert.DeserializeObject<List<PersonDetails>>(System.IO.File.ReadAllText(path));
            List<PersonDetails>  filteredPersons = persons.Where(data => data.FirstName.ToLower() == filter.ToLower() || data.LastName.ToLower() == filter.ToLower()).ToList();
            return PartialView("Search", filteredPersons);
        }

        // GET: PersonDetails/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PersonDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonDetails/Create
        [HttpPost]
        public ActionResult Create(PersonDetails p)
        {
            try
            {
                List<PersonDetails> obj = TempData.Peek("PersonList") as List<PersonDetails>;
                var result = obj.OrderByDescending(x => x.Id).FirstOrDefault();
                p.Id = result!=null? result.Id + 1:1;
                obj.Add(p);
                SearlizeObject(obj);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonDetails/Edit/5
        public ActionResult Edit(int id)
        {
            List<PersonDetails> obj = TempData.Peek("PersonList") as List<PersonDetails>;
            return View(obj.Where(data => data.Id == id).First());
        }

        // POST: PersonDetails/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PersonDetails p)
        {
            try
            {
                List<PersonDetails> obj = TempData["PersonList"] as List<PersonDetails>;
                var index = obj.FindIndex(c => c.Id == id);
                obj[index] = new PersonDetails{ DateOfBirth=p.DateOfBirth,FirstName=p.FirstName,LastName=p.LastName,NickName=p.NickName,Id=id};
                SearlizeObject(obj);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonDetails/Delete/5
        public ActionResult Delete(int id)
        {
            List<PersonDetails> obj = TempData.Peek("PersonList") as List<PersonDetails>;
            return View(obj.Where(data => data.Id == id).First());
        }

        // POST: PersonDetails/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                List<PersonDetails> obj = TempData["PersonList"] as List<PersonDetails>;
                var itemToRemove = obj.Single(r => r.Id == id);
                obj.Remove(itemToRemove);
                SearlizeObject(obj);
                DeleteAddress(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #region private methods
        
        private void SearlizeObject(List<PersonDetails> obj)
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(path, output);
        }

        private void DeleteAddress(int personId)
        {
            string pathAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AddressData.json");
            List<PersonAddress> personsAddress = JsonConvert.DeserializeObject<List<PersonAddress>>(System.IO.File.ReadAllText(pathAddress));
            personsAddress.RemoveAll(data => data.PersonId == personId);
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(personsAddress, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(pathAddress, output);
        }
        #endregion
    }
}
