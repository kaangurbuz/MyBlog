using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Models;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;

namespace MyNotes.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyNotesUserManager mum = new MyNotesUserManager();
        private BusinessLayerResult<MyNotesUser> res;
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
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
            Test test = new Test();


            return View();
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


                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}