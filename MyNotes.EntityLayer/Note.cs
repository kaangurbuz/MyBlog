using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer
{
    [Table("tblNotes")]
    public class Note:BaseEntity
    {
        [MaxLength,Required]
        public string Title { get; set; }
        [MaxLength, Required]
        public string Text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        public virtual MyNotesUser Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

        public virtual List<Liked> Likeds { get; set; } = new List<Liked>();
        //eager loading: olusturacagim nesnede alt baglamlar var ise hepsini getirir
        // lazy loading . olusturulan nesne getirilir ilgili baglamlar yuklenmez
    }
}
