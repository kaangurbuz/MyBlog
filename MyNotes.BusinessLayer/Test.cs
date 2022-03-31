using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.DataAccessLayer;

namespace MyNotes.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            MyNoteContext db;
            db = new MyNoteContext();
            db.Categories.ToList();
        }
    }
}
