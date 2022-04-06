using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer
{
    [Table("tblMyNotesUsers")]
    public class MyNotesUser:BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50),Required]
        public string UserName { get; set; }
        [StringLength(100),Required]
        public string Email { get; set; }
        [StringLength(150),Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public Guid ActivateGuid { get; set; }=Guid.NewGuid();
        public bool IdAdmin { get; set; }
        public virtual ICollection<Note> Notes { get; set; }=new List<Note>();
        public virtual ICollection<Comment> Comments { get; set; }= new List<Comment>();
        public virtual ICollection<Liked> Likeds { get; set; } = new List<Liked>();

    }
}
