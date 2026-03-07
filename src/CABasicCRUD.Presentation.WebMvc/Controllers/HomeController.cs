using CABasicCRUD.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

// ========================================================================================================================
// ========================================================================================================================

public class HomeController : Controller
{
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ICurrentUser currentUser, ILogger<HomeController> logger)
    {
        _currentUser = currentUser;
        _logger = logger;
    }

    // ========================================================================================================================

    public IActionResult Index()
    {
        if (_currentUser.IsAuthenticated)
        {
            return View("Index");
        }

        return View("Landing");
    }

    // ========================================================================================================================

    public new IActionResult NotFound()
    {
        Response.StatusCode = 404;
        return View();
    }

    // ========================================================================================================================

    [Route("Home/Error")]
    public IActionResult Error()
    {
        Response.StatusCode = 500;
        return View();
    }

    // ========================================================================================================================

    [Route("Home/StatusCode")]
    public new IActionResult StatusCode(int code)
    {
        _logger.LogInformation("Status code: {code}", code);
        Response.StatusCode = code;
        return code == 404 ? View("NotFound") : View("Error");
    }
}

// ========================================================================================================================
// ========================================================================================================================
