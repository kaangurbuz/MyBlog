using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.EntityLayer;

namespace MyNotes.DataAccessLayer
{
    public class MyNoteContext:DbContext
    {
        public DbSet<MyNotesUser> MyNotesUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Liked> Likeds { get; set; }
        public DbSet<Category> Categories { get; set; }

        public MyNoteContext():base("SqlConDb")
        {
            Database.SetInitializer(new MyInitializer());
        }

    }
}
