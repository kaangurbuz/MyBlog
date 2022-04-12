using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyNotes.BusinessLayer.Models;
using MyNotes.CommonLayer;

namespace MyNotes.WebMVC.Init
{
    public class WebCommon:ICommon
    {
        public string GetCurrentUsername()
        {
            if (CurrentSession.User != null)
            {
                var user = CurrentSession.User;
                return user.UserName;
            }

            return "system";
        }
    }
}