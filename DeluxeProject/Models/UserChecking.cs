using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeluxeModel;

namespace DeluxeProject.Models
{
    public class UserChecking
    {
        DeluxeShoppingEntities db = new DeluxeShoppingEntities();
        public bool CheckInUser(string mail)
        {
            var selection =( from a in db.users
                            where a.emails == mail
                            select a).Any();

            if (selection == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}