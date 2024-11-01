using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Marcos_Leonel_Lopez.Models;

namespace tl2_tp6_2024_Marcos_Leonel_Lopez.Controllers;

public class ProductController : Controller
{
    // private readonly ILogger<HomeController> _logger;

    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }

    private readonly ProductRepository _productoRepository;
    private readonly ProductValidator _productValidator;
    private List<Product> productos;
    private Product producto;
    public ProductController(ProductRepository productoRepository, ProductValidator productValidator)
    {
        _productoRepository = productoRepository; // Usa la instancia inyectada
        _productValidator = productValidator;
        productos = new List<Product>();
    }

    // ejecutar con: dotnet watch
    public IActionResult Index() // Igual al nombre de los archivos en carpeta "Views"
    {
        return View();
    }
    public IActionResult GetAll()
    {
        var productos = _productoRepository.GetAll();
        return View(productos); 
    }
    public IActionResult GetById(int id)
    {
        producto = _productoRepository.GetById(id);
        if (producto == null)
        {
            return NotFound(); 
        }
        return View(producto); 
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
