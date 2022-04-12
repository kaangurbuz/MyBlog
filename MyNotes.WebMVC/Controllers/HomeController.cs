using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Models;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;
using MyNotes.WebMVC.ViewModel;

namespace MyNotes.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyNotesUserManager mum = new MyNotesUserManager();
        private readonly NoteManager nm = new NoteManager();
        private BusinessLayerResult<MyNotesUser> res;

        public ActionResult ByCategoryId(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Note> notes = nm.QList().Where(x => x.Category.Id == id && x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList();
            ViewBag.CategoryId2 = id;
            
            return View("Index", notes);
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //TempData["uname"] = model.Username;
            //TempData["pass"] = model.Password;
            TempData["mod"] = model;
            if (ModelState.IsValid)
            {
                res = mum.LoginUser(model);
                if (res.Errors.Count>0)
                {
                    if ((res.Errors.Find(x=>x.Code==ErrorMessageCode.UserIsNotActive)!=null))
                    {
                        ViewBag.SetLink = "http://Home/UserActivate/1234-2345-3456789";
                    }
                    res.Errors.ForEach(x=>ModelState.AddModelError("",x.Message));
                    return View(model);
                }
                CurrentSession.Set("Login",res.Result);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            //Test test = new Test();

            return View(nm.QList().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                res = mum.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderilen aktivasyon linkine tıklayarak hesabınızı aktive ediniz.");
                return View("Ok", notifyObj);
                
            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            res = mum.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }
        
        public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }

        public ActionResult ShowProfile()
        {
            if (CurrentSession.User is MyNotesUser currentUser)
                res = mum.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            if (CurrentSession.User is MyNotesUser currentUser) 
                res = mum.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Olustu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(MyNotesUser model,HttpPostedFileBase profileImage)
        {
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            ModelState.Remove("CreatedOn");
            if (ModelState.IsValid)
            {
                if (profileImage != null && ( profileImage.ContentType == "image/jpeg" || profileImage.ContentType == "image/jpg" || profileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{profileImage.ContentType.Split('/')[1]}";
                    profileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }
                //
                res = mum.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profil Guncellenemedi",
                        Items = res.Errors,
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }
                CurrentSession.Set("login", res.Result);
                //return RedirectToAction("ShowProfile");
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Profil Güncellendi",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                notifyObj.Items.Add("Profil bilgileriniz başarıyla güncellendi.");
                return View("Ok", notifyObj);
            }
            return View(model);
        }

        public ActionResult DeleteProfile()
        {
            if (CurrentSession.User is MyNotesUser currentUser)
            {
                res = mum.RemoveUserById(currentUser.Id);
            }

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Profil Silinemedi",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            
            CurrentSession.Clear();

            OkViewModel notifyObj = new OkViewModel()
            {
                Title = "Profil Silindi",
                RedirectingUrl = "/Home/ShowProfile"
            };
            notifyObj.Items.Add("Profil bilgileriniz başarıyla silindi.");
            return View("Ok", notifyObj);


        }

        public ActionResult SendEmail(LoginViewModel model)
        {

            //model.Username = TempData["uname"].ToString();
            //model.Password = TempData["pass"].ToString();
            model = (LoginViewModel) TempData["mod"];

            mum.SendMail(model);

            OkViewModel notifyObj = new OkViewModel()
            {
                Title = "Email Gönderildi",
                RedirectingUrl = "/Home/Login"

            };
            notifyObj.Items.Add("Lütfen e-posta adresinize gönderilen aktivasyon linkine tıklayarak hesabınızı aktive ediniz.");
            return View("Ok", notifyObj);
        }
    }

}