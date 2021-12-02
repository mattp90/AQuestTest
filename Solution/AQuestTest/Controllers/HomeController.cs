using AQuestTest.Models;
using Database;
using System.Web.Mvc;

namespace AQuestTest.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(new ListOrderModel() { Orders = OrdersQueries.Orders() });
        }
    }
}