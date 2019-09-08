using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactsBook.Models
{
    public class PhoneNumber
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(12, ErrorMessage = "The field Number must  have a maximum length of '12'")]
        [MinLength(9, ErrorMessage = "The field Number must  have a minimum length of '9'")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The field Number must be numeric")]
        public string Number { get; set; }

        public int ContactId { get; set; }

        // navigation properties
        public Contact Contact { get; set; }
    }
}