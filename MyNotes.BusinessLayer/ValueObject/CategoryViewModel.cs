using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.EntityLayer;

namespace MyNotes.BusinessLayer.ValueObject
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [DisplayName("Baslik")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedUserName { get; set; }
    }
}
