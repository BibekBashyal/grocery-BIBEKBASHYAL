using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace grocery.Models
{
    public class MyMenu
    {
        public static List<tblCategory> GetMenus()
        {
            using (var context = new KantipurDBEntities())
            {
                return context.tblCategories.ToList();
            }
        }
      
    }
}