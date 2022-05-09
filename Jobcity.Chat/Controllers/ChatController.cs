using System.Web.Mvc;

namespace Jobcity.Chat.Mvc.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
    }
}