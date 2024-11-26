using Microsoft.AspNetCore.Mvc;

namespace Transaction.Controllers
{
    public class WithdrawController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
