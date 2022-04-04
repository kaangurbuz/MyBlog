using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;

namespace MyNotes.BusinessLayer
{
    public class MyNotesUserManager:ManagerBase<MyNotesUser>
    {
        //Kullanici username kontrolü yapmaliyim
        //kullanici email kontrolü yapmaliyim
        //kayit islemi gerceklestirmeliyim
        //aktivasyon e-posta gonderimi
        private BusinessLayerResult<MyNotesUser> res = new BusinessLayerResult<MyNotesUser>();

        public BusinessLayerResult<MyNotesUser> LoginUser(LoginViewModel data)
        {
            res.Result = Find(x => x.UserName == data.Username&&x.Password==data.Password) ;
            if (res.Result!=null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive,"Kullanici adi aktiflestirilmemis...");
                    res.AddError(ErrorMessageCode.CheckYourEmail,"Emailinzi kontrol ediniz..!");
                }
              
            }  
            else
            {
                    res.AddError(ErrorMessageCode.UsernameOrPassWrong,"Kullanici adi yada sifreniz yanlis lutfen kontrol edin..!");
            }
            return res;
        }
    }
}
