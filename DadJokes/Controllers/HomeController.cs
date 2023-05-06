using Microsoft.AspNetCore.Mvc;

namespace DadJokes.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

