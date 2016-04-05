using Carsale.Web.Authorization;
using Carsale.Security;
using Carsale.Security.Enum;
using Carsale.Security.Interface;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Carsale.Web.Models;
using System.Text;

namespace Carsale.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        IUserRepository UserRepository { get; set; }

        public ActionResult Login()
        {
            RemoveAuthenticationTicket();
            return View();
        }
        public ActionResult LogOff()
        {
            RemoveAuthenticationTicket();
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult Login(LoginDetailsModel loginDetailsModel)
        {
            if (ModelState.IsValid)
            {
                if (UserRepository.ValidateUser(loginDetailsModel.UserName, loginDetailsModel.Password, Server.MapPath("~/Data/UserDetails.json")))
                {
                    var userDetails = UserRepository.GetUserDetails(loginDetailsModel.UserName, Server.MapPath("~/Data/UserDetails.json"));
                    //create the FormsAuthenticationTicket
                    CreateFormsAuthenticationTicket(userDetails);
                    return RedirectToAction("CarList", "Car");
                }
                loginDetailsModel.ErrorMessage = "The username or password is not correct";
            }

            return View(loginDetailsModel);
        }
        #region Private methods
        private void CreateFormsAuthenticationTicket(IUserDetails userDetails)
        {
            var serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(userDetails);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userDetails.UserName, DateTime.Now, DateTime.Now.AddDays(30),
                                                                              true, userData, FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);         
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }
        public void RemoveAuthenticationTicket()
        {
            this.Session.Abandon();            
            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
        }

        #endregion

    }
}
