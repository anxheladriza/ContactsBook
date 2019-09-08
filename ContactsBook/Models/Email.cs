using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactsBook.Models
{
    public class Email
    {
        public int Id { get; set; }

       
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Email can not be empty")]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        public int ContactId { get; set; }

        // navigation properties
        public Contact Contact { get; set; }
    }
}