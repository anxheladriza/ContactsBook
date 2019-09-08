using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsBook.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public int ContactId { get; set; }

        // navigation properties
        public Contact Contact { get; set; }
        
    }
}