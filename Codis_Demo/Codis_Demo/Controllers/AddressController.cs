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
    public class AddressController : Controller
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AddressData.json");
       
        // GET: Address/Details/5
        public ActionResult Details(int id)
        {
            List<SelectListItem> countryList = GetCountries();
            ViewData["countryList"] = countryList;
            TempData["PersonId"] = id;
            List<PersonAddress> personsAddress = JsonConvert.DeserializeObject<List<PersonAddress>>(System.IO.File.ReadAllText(path));
            List<Address> address = personsAddress!=null?  personsAddress.Where(data => data.PersonId == id).Select(ab => ab.Address).SingleOrDefault():null;

            return View("Details", address);
        }

        // GET: Address/Create
        public ActionResult Create()
        {
            List<SelectListItem> countryList = GetCountries();
            ViewData["countryList"] = countryList;
            return View();
        }

        // POST: Address/Create
        [HttpPost]
        public ActionResult Create(Address address)
        {
            try
            {
                List<PersonAddress> personsAddress = JsonConvert.DeserializeObject<List<PersonAddress>>(System.IO.File.ReadAllText(path));
                int personId = Convert.ToInt32(TempData.Peek("PersonId"));
                var result = personsAddress ==null || personsAddress.Count==0?null : personsAddress.Where(data => data.PersonId == personId).SingleOrDefault().Address.OrderByDescending(x => x.AddressId).FirstOrDefault();
               
                address.AddressId = result != null ? result.AddressId + 1 : 1;
                if (personsAddress != null && personsAddress.Count != 0) {
                    personsAddress.Where(data => data.PersonId == personId).Select(ab => ab.Address).SingleOrDefault().Add(address);
                }
                else
                {
                    PersonAddress pa = new PersonAddress();
                    pa.PersonId = personId;
                    pa.Address = new List<Address>();
                    pa.Address.Add(address);

                    personsAddress = new List<PersonAddress>();
                    personsAddress.Add(pa);
                }
                
                SearlizeObject(personsAddress);
                return RedirectToAction("Details", new { id = personId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Address/Edit/5
        public ActionResult Edit(int id,int personId)
        {
            TempData["PersonId"] = personId;
            List<SelectListItem> countryList = GetCountries();

            List<PersonAddress> personsAddress = JsonConvert.DeserializeObject<List<PersonAddress>>(System.IO.File.ReadAllText(path));
            Address address = personsAddress.Where(data => data.PersonId == personId).Select(ab => ab.Address).SingleOrDefault().Where(data=>data.AddressId==id).SingleOrDefault();
            countryList.Where(x => x.Value == address.CountryId.ToString()).ToList().ForEach(x => x.Selected = true);
            ViewData["countryList"] = countryList;
            return View(address);
        }

        // POST: Address/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,  Address address)
        {
            try
            {
                int personId = Convert.ToInt32( TempData.Peek("PersonId"));
                List<PersonAddress> personsAddress = JsonConvert.DeserializeObject<List<PersonAddress>>(System.IO.File.ReadAllText(path));
                personsAddress.Where(data => data.PersonId == personId).Select(ab => ab.Address).SingleOrDefault().Where(data => data.AddressId == id).ToList()
                    .ForEach(x => { x.CountryId = address.CountryId;
                        x.Line1 = address.Line1;
                        x.Line2 = address.Line2;
                        x.PostCode = address.PostCode;
                    });
                SearlizeObject(personsAddress);
                return RedirectToAction("Details", new { id = personId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Address/Delete/5
        public ActionResult Delete(int id, int personId)
        {
            TempData["PersonId"] = personId;
            List<SelectListItem> countryList = GetCountries();

            List<PersonAddress> personsAddress = JsonConvert.DeserializeObject<List<PersonAddress>>(System.IO.File.ReadAllText(path));
            Address address = personsAddress.Where(data => data.PersonId == personId).Select(ab => ab.Address).SingleOrDefault().Where(data => data.AddressId == id).SingleOrDefault();
            ViewData["countryList"] = countryList;
            return View(address);
        }

        // POST: Address/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int personId = Convert.ToInt32(TempData.Peek("PersonId"));
                List<PersonAddress> personsAddress = JsonConvert.DeserializeObject<List<PersonAddress>>(System.IO.File.ReadAllText(path));
                personsAddress.Where(data => data.PersonId == personId).Select(ab => ab.Address).SingleOrDefault().RemoveAll(data => data.AddressId == id);
                SearlizeObject(personsAddress);
                return RedirectToAction("Details", new { id = personId });
            }
            catch
            {
                return View();
            }
        }

        #region private methods

        private List<SelectListItem>  GetCountries()
        {
            List<SelectListItem> countryList = new List<SelectListItem>();
            countryList.Add(new SelectListItem { Text = "Russia", Value = "1" });
            countryList.Add(new SelectListItem { Text = "France", Value = "2" });
            countryList.Add(new SelectListItem { Text = "Germany", Value = "3" });
            countryList.Add(new SelectListItem { Text = "United Kingdom", Value = "4" });
            return countryList;
        }
        private void SearlizeObject(List<PersonAddress> obj)
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(path, output);
        }
        #endregion
    }
}
