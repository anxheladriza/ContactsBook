using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsBook.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string  Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}