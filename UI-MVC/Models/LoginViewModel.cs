using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SC.UI.Web.MVC.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }

        public LoginViewModel()
        {

        }

        public LoginViewModel(string un, string pw)
        {
            this.UserName = un;
            this.PassWord = pw;
        }
    }
}