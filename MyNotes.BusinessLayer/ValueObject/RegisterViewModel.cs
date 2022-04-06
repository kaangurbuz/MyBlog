using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer.ValueObject
{
    public class RegisterViewModel
    {
        [DisplayName("Adi"),Required(ErrorMessage = "{0} alani bos gecilemez"),StringLength(30,ErrorMessage = "{0} alani {1} karakterden fazla olamaz!")]
        public string Name { get; set; }
        [DisplayName("Soyadi"), Required(ErrorMessage = "{0} alani bos gecilemez"), StringLength(30, ErrorMessage = "{0} alani {1} karakterden fazla olamaz!")]
        public string LastName { get; set; }
        [DisplayName("Kullanici Adi"), Required(ErrorMessage = "{0} alani bos gecilemez"), StringLength(30, ErrorMessage = "{0} alani {1} karakterden fazla olamaz!")]
        public string UserName { get; set; }
        [DisplayName("Email"), Required(ErrorMessage = "{0} alani bos gecilemez"), StringLength(100, ErrorMessage = "{0} alani {1} karakterden fazla olamaz!"),EmailAddress(ErrorMessage = "{0} alani icin gecerli bir email adresi giriniz.")]
        public string Email { get; set; }
        [DisplayName("Sifre"), Required(ErrorMessage = "{0} alani bos gecilemez"), StringLength(30,MinimumLength = 3, ErrorMessage = "{0} alani {2} karakterden az {1} karakterden fazla olamaz!"), DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Sifre"), Required(ErrorMessage = "{0} alani bos gecilemez"), StringLength(30, MinimumLength = 3, ErrorMessage = "{0} alani {2} karakterden az {1} karakterden fazla olamaz!"), DataType(DataType.Password), Compare("Password", ErrorMessage = "{0} ile {1} uyusmuyor.")]
        public string RePassword { get; set; }

    }
}
