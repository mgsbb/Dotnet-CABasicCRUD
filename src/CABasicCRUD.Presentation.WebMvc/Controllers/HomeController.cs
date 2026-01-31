using CABasicCRUD.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ICurrentUser _currentUser;

    public HomeController(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public IActionResult Index()
    {
        if (_currentUser.IsAuthenticated)
        {
            return View("Index");
        }

        return View("Landing");
    }
}
