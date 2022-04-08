using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using MyNotes.EntityLayer;

namespace MyNotes.BusinessLayer.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("category-cache");
            if (result == null)
            {
                CategoryManager cm = new CategoryManager();
                result = cm.List();
                WebCache.Set("category-cache", result, 30, true);
            }
            return result;
        }

        public static void RemoveCategoriesCache()
        {
            WebCache.Remove("category-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}
