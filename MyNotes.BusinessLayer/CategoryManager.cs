using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.DataAccessLayer;
using MyNotes.EntityLayer;

namespace MyNotes.BusinessLayer
{
    public class CategoryManager:ManagerBase<Category>
    {
        private Repository<Category> repo = new Repository<Category>();

        public List<Category> IndexCat()
        {
            return repo.List();
        }

        public int InsertCat(CategoryViewModel cat)
        {
            Category entity = new Category();
            entity.Title = cat.Title;
            entity.Description = cat.Description;
            entity.CreatedOn = null;
            entity.ModifiedUserName = null;
            entity.ModifiedOn = null;
            return repo.Insert(entity);
        }
        public CategoryViewModel FindCat(int? id)
        {
            var cat = repo.QList().FirstOrDefault(x => x.Id == id);
            CategoryViewModel cvm = new CategoryViewModel();
            if (cat != null)
            {
                cvm.Id = cat.Id;
                cvm.Title = cat.Title;
                cvm.Description = cat.Description;
                cvm.CreatedOn = cat.CreatedOn;
                cvm.ModifiedOn = cat.ModifiedOn;
                cvm.ModifiedUserName = cat.ModifiedUserName;
            }

            return cvm;
        }

        public int DeleteCat(int? id)
        {
            var cat = repo.Find(x => x.Id == id);
            
            return repo.Delete(cat);
        }
    }
}
