using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using ExchangeApp.Models;
using System.Text;
//using ExchangeApp.ViewModel;
using System.Data.Entity.Infrastructure;

namespace ExchangeApp.Helpers
{
    public static class helpers
    {
        public static DateTime GlobalSystemTime()
        {
            return DateTime.Now;
        }

        public static string GetCurrentUserID()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null)
                return System.Web.HttpContext.Current.User.Identity.GetUserId();
            return null;
        }


        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                             string text, string title, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = text;
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }



        public static bool IsMember(string role)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string query = string.Format(@"SELECT IS_MEMBER('{0}')", role);
            var result = db.Database.SqlQuery<int>(query).SingleOrDefault();

            return result == 1;
        }

        public static int GetAccessFailedCount()
        {
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    var l = db.Global.Where(p => p.Id == "AccessFailedCount").ToList();
        //    if (l.Count > 0)
        //        return int.Parse(l.First().Val);
        //    else
                return 2;
        }

        public static int GetLockedAccountTime()
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            //var l = db.Global.Where(p => p.Id == "LockedAccountTime").ToList();
            //if (l.Count > 0)
            //    return int.Parse(l.First().Val);
            //else
                return 20;
        }

        public static int GetNumberRecentlyUsedPasswords()
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            //var l = db.Global.Where(p => p.Id == "NrRecentlyUserPwd").ToList();
            //if (l.Count > 0)
            //    return int.Parse(l.First().Val);
            //else
                return 5;
        }

        public static int GetPasswordExpirationPeriod()
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            //var l = db.Global.Where(p => p.Id == "PasswordExpiration").ToList();
            //if (l.Count > 0)
            //    return int.Parse(l.First().Val);
            //else
                return 12;
        }


        public static string GetValueFromGlobal(string id)
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            //var l = db.Global.Where(p => p.Id == id).ToList();
            //if (l.Count > 0)
            //    return l.First().Val;
            //else
                return string.Empty;
        }

        public static bool IsPasswordRecentlyUsed(string userId, string password)
        {
            //ApplicationDbContext db = new ApplicationDbContext();
            //var recentPasswords = db.UserPasswordHistory.Where(p => p.UserId == userId).OrderByDescending(p => p.DateChanged).Take(GetNumberRecentlyUsedPasswords()).ToList();
            //imis.ApplicationUserManager.PasswordHasherSHA1 pwdHasher = new imis.ApplicationUserManager.PasswordHasherSHA1();
            //string hashedPassword = pwdHasher.HashPassword(password);
            //foreach (UserPasswordHistory uph in recentPasswords)
            //{
            //    if (uph.PasswordHash == hashedPassword)
            //        return true;
            //}
            return false;
        }

    }

    public static class MyHtmlHelpers
    {
        

       
    }
}

