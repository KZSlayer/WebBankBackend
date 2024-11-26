using Microsoft.AspNetCore.Mvc;

namespace Transaction.Controllers
{
    public class TransferController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
