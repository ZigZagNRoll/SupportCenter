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
    public class ChatController : Controller
    {
        // GET: Chat
        [Authorize(Roles = "User, Admin")]
        public ActionResult Chat()
        {
            ActiveUserViewModel model = new ActiveUserViewModel(){
                UserName = WebSecurity.CurrentUserName,
                UserID = WebSecurity.CurrentUserId.ToString()
            };
            ViewBag.naam = "naam";
            return View(model);
        }
    }
}