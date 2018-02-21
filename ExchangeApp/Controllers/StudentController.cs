using System.Web.Mvc;

namespace ExchangeApp.Controllers
{
    public class StudentController : BaseController
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
    }
}