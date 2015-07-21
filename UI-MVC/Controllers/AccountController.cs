using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SC.UI.Web.MVC.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace SC.UI.Web.MVC.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerData)
        {
            if (!WebSecurity.UserExists(registerData.UserName))
            {
                WebSecurity.CreateUserAndAccount(registerData.UserName, registerData.PassWord);
                
                Roles.AddUserToRole(registerData.UserName, "User");
                
                this.Login(new LoginViewModel(registerData.UserName, registerData.PassWord));
                return Redirect("~/");
            }

            ModelState.AddModelError("","Deze gebruikersaccount bestaat al!");
            return View(registerData);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginData)
        {
            if (WebSecurity.Login(loginData.UserName, loginData.PassWord))
            {
                return Redirect("~/");
            }
            else
            {
                ModelState.AddModelError("", "De opgegeven gebruikersnaam of wachtwoord is ongeldig!");
            }
            return View(loginData);
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return Redirect("~/");
        }
    }
}