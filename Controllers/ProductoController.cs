using System.Diagnostics;
using IProductoRepo;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Marcos_Leonel_Lopez.Models;

namespace tl2_tp6_2024_Marcos_Leonel_Lopez.Controllers;

public class ProductoController : Controller
{
    //private readonly ProductRepo.ProductoRepository _productoRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly ProductValidator _productValidator;
    private List<Producto> productos;
    private Producto producto;
    public ProductoController(IProductoRepository productoRepository, ProductValidator productValidator)
    {
        _productoRepository = productoRepository; // Usa la instancia inyectada
        _productValidator = productValidator;
        productos = new List<Producto>();
    }

    // ejecutar con: dotnet watch
    public IActionResult Index() // Igual al nombre de los archivos en carpeta "Views"
    {
        return View();
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var productos = _productoRepository.GetAll();
        return View(productos);
    }
    [HttpGet]
    public IActionResult GetById(int id)
    {
        producto = _productoRepository.GetById(id);
        if (producto == null)
        {
            ViewData["ErrorMessage"] = "El producto con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(producto);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Producto());
    }

    [HttpPost]
    public IActionResult Create(Producto nuevoProducto)
    {
        var producto = _productoRepository.Create(nuevoProducto);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var producto = _productoRepository.GetById(id); // Busca el producto por ID
        if (producto == null)
        {
            ViewData["ErrorMessage"] = "El producto con el ID proporcionado no existe.";
            return View("Error"); // Si el producto no se encuentra, redirige a una página de error
        }
        return View(producto); // Si se encuentra, lo pasa a la vista para su edición
    }

    [HttpPost]
    public IActionResult Update(Producto productoEditad, int id)
    {
        var producto = _productoRepository.Update(productoEditad, id);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = _productoRepository.GetById(id); // Busca el producto por ID
        if (producto == null)
        {
            ViewData["ErrorMessage"] = "El producto con el ID proporcionado no existe.";
            return View("Error"); // Si el producto no se encuentra, redirige a una página de error
        }
        return View(producto); // Si se encuentra, lo pasa a la vista para su edición
    }

    [HttpPost]
    public IActionResult Delete(Producto producto , int id)
    {
        _productoRepository.Delete(id);
        return RedirectToAction("GetAll");
        
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
