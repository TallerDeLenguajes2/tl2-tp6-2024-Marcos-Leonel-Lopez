using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Marcos_Leonel_Lopez.Models;

namespace tl2_tp6_2024_Marcos_Leonel_Lopez.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // ejecutar con: dotnet watch
    public IActionResult Index() // Igual al nombre de los archivos en carpeta "Views"
    {
        return View();
    }

    // public IActionResult Privacy()
    // {
    //     System.Console.WriteLine("entra aqui");
    //     return View();
    // }
        public IActionResult Producto()
    {
        System.Console.WriteLine("entra aqui en product");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
