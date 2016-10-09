using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace YouJuku.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "first_name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "last_name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "address")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Phone]
        [JsonProperty(PropertyName = "home_phone")]
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Phone]
        [JsonProperty(PropertyName = "cell_phone")]
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Required]
        [JsonProperty(PropertyName = "email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty(PropertyName = "start_date")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Age")]
        [JsonProperty(PropertyName = "age")]
        public string Age { get; set; }

        [Display(Name = "Grade")]
        [JsonProperty(PropertyName = "grade")]
        public string Grade { get; set; }

        [Display(Name = "Sibling")]
        [JsonProperty(PropertyName = "sibling")]
        public string Sibling { get; set; }

        [Required]
        [JsonProperty(PropertyName = "is_active")]
        public bool IsActive { get; set; }
    }
}