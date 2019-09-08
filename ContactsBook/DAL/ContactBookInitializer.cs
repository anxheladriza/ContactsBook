using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsBook.DAL
{
    public class ContactBookInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ContactBookContexts>
    {
    }
}