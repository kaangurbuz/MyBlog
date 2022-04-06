using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.CommonLayer.Helper;
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

        public BusinessLayerResult<MyNotesUser> RegisterUser(RegisterViewModel data)
        {
            res.Result = Find(x => x.UserName == data.UserName||x.Email==data.Email);
            if (res.Result != null)
            {
                if (res.Result.UserName==data.UserName)
                {
                      res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Boyle bir kullanici zaten mevcut..!");
                }
                else if (res.Result.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Boyle bir email zaten mevcut..!");
                }
              
                
            }
            else
            {
                res.Result = new MyNotesUser()
                {
                    Name = data.Name,
                    LastName = data.LastName,
                    UserName = data.UserName,
                    Password = data.Password,
                    Email = data.Email,
                    IsActive = true,
                    IdAdmin = false,
                    ActivateGuid = Guid.NewGuid()
                    //img eklenecek
                };

                if (Insert(res.Result)>0)
                {
                    Save();
                    res.Result = Find(x => x.UserName == data.UserName && x.Email == data.Email);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Name} {res.Result.LastName} <br/> Hesabinizi aktif etmek icin <a href='{activateUri}' target='_blank'>tiklayiniz</a>";
                    MailHelper.SendMail(body, res.Result.Email, "MyNotes Hesabi Aktivasyon");
                    
                }
                
            }
            return res;
        }        
    }
}

//mynotesuser manager içinde register user'ı ekledim
//homecontroller içinde register ı ekledim
//