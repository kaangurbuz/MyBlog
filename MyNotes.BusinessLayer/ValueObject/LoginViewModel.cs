using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer.ValueObject
{
    public class LoginViewModel
    {
        [DisplayName("Kullanici Adi"),Required(ErrorMessage = "{0} alan bos gecilemez!"),StringLength(30,ErrorMessage = "{0} max {1} karakter olmali.")]
        public string Username { get; set; }
        [DisplayName("Sifre"), Required(ErrorMessage = "{0} alan bos gecilemez!"), StringLength(30, ErrorMessage = "{0} max {1} karakter olmali."),DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
