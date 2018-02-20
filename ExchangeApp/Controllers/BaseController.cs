using ExchangeApp.Models;
using ExchangeApp.Toastr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ExchangeApp.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string role)
        {
            return User.IsInRole(role);
        }

        protected void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
            this.AddToastMessage(msgText, "", ToastType.Success);
        }

        protected void DisplayErrorMessage()
        {
            DisplayErrorMessage("Error while saving changes");
        }

        protected void DisplayErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
            this.AddToastMessage("Error", TempData["ErrorMessage"].ToString(), ToastType.Error);
        }

        protected string GetModelStateErrors()
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in ModelState)
            {
                var errors = item.Value.Errors;
                foreach (var error in errors)
                {
                    result.Append(error.ErrorMessage);
                }
            }
            return result.ToString();
        }

        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    if (User != null)
        //    {
        //        var username = User.Identity.Name;

        //        if (!string.IsNullOrEmpty(username))
        //        {
        //            var user = db.Users.SingleOrDefault(u => u.UserName == username);
        //            string fullName = string.Concat(new string[] { user.LastName, ", ", user.FirstName });
        //            ViewData.Add("FullName", fullName);
        //        }
        //    }
        //    base.OnActionExecuted(filterContext);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected bool IsMember(string role)
        {
            string query = string.Format(@"SELECT IS_MEMBER('{0}')", role);

            var result = db.Database.SqlQuery<int>(query).SingleOrDefault();

            return result == 1;
        }
    }
}