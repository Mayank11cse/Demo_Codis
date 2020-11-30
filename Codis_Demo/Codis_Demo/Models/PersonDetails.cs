using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Codis_Demo.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        [Required]
        public string Line1 { get; set; }
        [RegularExpression("^[a-zA-Z ]*$")]
        public string Line2 { get; set; }
        [Display(Name = "Country Name")]
        public int CountryId { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = "Special character should not be entered")]
        public int PostCode { get; set; }
    }
    public class PersonAddress
    {
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public List<Address> Address { get; set; }
    }
    public class PersonDetails
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression("^[a-zA-Z ]*$"),Required]

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [RegularExpression("^[a-zA-Z ]*$"), Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [RegularExpression("^[a-zA-Z ]*$")]
        public string NickName { get; set; }
        //public List<Address> Address { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
    }

}