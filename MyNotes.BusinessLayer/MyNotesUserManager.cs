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
    public class MyNotesUserManager : ManagerBase<MyNotesUser>
    {
        //Kullanici username kontrolü yapmaliyim
        //kullanici email kontrolü yapmaliyim
        //kayit islemi gerceklestirmeliyim
        //aktivasyon e-posta gonderimi
        private readonly BusinessLayerResult<MyNotesUser> res = new BusinessLayerResult<MyNotesUser>();

        public BusinessLayerResult<MyNotesUser> LoginUser(LoginViewModel data)
        {
            res.Result = Find(x => x.UserName == data.Username && x.Password == data.Password && x.IsDeleted != true);
            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanici adi aktiflestirilmemis...");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Emailinzi kontrol ediniz..!");
                }

            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanici adi yada sifreniz yanlis lutfen kontrol edin..!");
            }
            return res;
        }

        public BusinessLayerResult<MyNotesUser> RegisterUser(RegisterViewModel data)
        {
            res.Result = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            if (res.Result != null)
            {
                if (res.Result.UserName == data.UserName)
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
                    IsActive = false,
                    IdAdmin = false,
                    ActivateGuid = Guid.NewGuid(),
                    ProfileImageFileName = "user.png"
                };

                if (base.Insert(res.Result) > 0)
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

        public BusinessLayerResult<MyNotesUser> ActivateUser(Guid id)
        {
            res.Result = Find(x => x.ActivateGuid == id);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanici zaten aktif..!");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
                Save();
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Girdiginiz aktivasyon kodu yanlis..!");
            }
            return res;
        }

        public new BusinessLayerResult<MyNotesUser> Insert(MyNotesUser data)
        {
            MyNotesUser user = Find(s => s.UserName == data.UserName || s.Email == data.Email);
            res.Result = data;
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Boyle bir kullanici zaten mevcut..!");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Boyle bir email zaten mevcut..!");
                }

            }
            else
            {
                res.Result.ActivateGuid = Guid.NewGuid();
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanici eklenemedi..!");
                }
            }

            return res;
        }
        public new BusinessLayerResult<MyNotesUser> Update(MyNotesUser data)
        {
            MyNotesUser user = Find(s => (s.UserName == data.UserName || s.Email == data.Email) && s.Id != data.Id);
            if (user != null && user.Id != data.Id)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Bu kullanici adini alamazsiniz..!");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu email adresini alamazsiniz..!");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.LastName = data.LastName;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            res.Result.IsDeleted = data.IsDeleted;
            res.Result.IsActive = data.IsActive;
            res.Result.IdAdmin = data.IdAdmin;
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanici guncellenemedi..!");
            }

            return res;



        }

        public BusinessLayerResult<MyNotesUser> UpdateProfile(MyNotesUser data)
        {
            MyNotesUser user = Find(s => s.Id != data.Id && (s.UserName == data.UserName || s.Email == data.Email));
            if (user != null && user.Id != data.Id)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu kullanici adi daha once kaydedilmis.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu email daha once kaydedilmis.");
                }
                return res;
            }
            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.LastName = data.LastName;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            if (!string.IsNullOrEmpty(data.ProfileImageFileName))
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil guncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<MyNotesUser> GetUserById(int id)
        {
            res.Result = Find(x => x.Id == id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanici bulunamadi");

            }

            return res;
        }

        public BusinessLayerResult<MyNotesUser> RemoveUserById(int id)
        {
            res.Result = Find(s => s.Id == id);
            if (res.Result != null)
            {
                res.Result.IsDeleted = true;
                if (base.Update(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanici silinemedi.");
                }


            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotRemove, "Boyle bir kullanici bulunamadi.");

            }
            return res;
        }

        public BusinessLayerResult<MyNotesUser> SendMail(LoginViewModel data)
        {
            res.Result = Find(x => x.UserName == data.Username && x.Password == data.Password);
            string siteUri = ConfigHelper.Get<string>("SiteRootUri");
            string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
            string body = $"Merhaba {res.Result.Name} {res.Result.LastName} <br/> Hesabinizi aktif etmek icin <a href='{activateUri}' target='_blank'>tiklayiniz</a>";
            MailHelper.SendMail(body, res.Result.Email, "MyNotes Hesabi Aktivasyon");

            return res;
        }
    }
}

//mynotesuser manager içinde register user'ı ekledim
//homecontroller içinde register ı ekledim
//register içinde kullanıcı adı ve email kontrolü yapıyorum