using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Marcos_Leonel_Lopez.Models;

namespace tl2_tp6_2024_Marcos_Leonel_Lopez.Controllers;

public class PresupuestoController : Controller
{
    private readonly PresupuestoRepo.PresupuestoRepository _presupuestoRepository;

    public PresupuestoController(PresupuestoRepo.PresupuestoRepository presupuestoRepository)
    {
        _presupuestoRepository = presupuestoRepository;
    }

    // ejecutar con: dotnet watch
    public IActionResult Index() // Igual al nombre de los archivos en carpeta "Views"
    {
        return View();
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var presupuestos = _presupuestoRepository.GetAll();
        return View(presupuestos);
    }
    [HttpGet]
    public IActionResult GetById(int id)
    {
        var presupuestos = _presupuestoRepository.GetById(id);
        if (presupuestos == null)
        {
            ViewData["ErrorMessage"] = "El presupuesto con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Presupuesto());
    }

    [HttpPost]
    public IActionResult Create(Presupuesto nuevoPresupuesto)
    {
        var producto = _presupuestoRepository.Create(nuevoPresupuesto);
        return RedirectToAction("GetAll");
    }

    // [HttpGet]
    // public IActionResult Update(int id)
    // {
    //     var producto = _productoRepository.GetById(id); // Busca el producto por ID
    //     if (producto == null)
    //     {
    //         ViewData["ErrorMessage"] = "El producto con el ID proporcionado no existe.";
    //         return View("Error"); // Si el producto no se encuentra, redirige a una página de error
    //     }
    //     return View(producto); // Si se encuentra, lo pasa a la vista para su edición
    // }

    // [HttpPost]
    // public IActionResult Update(Producto productoEditad, int id)
    // {
    //     var producto = _productoRepository.Update(productoEditad, id);
    //     return RedirectToAction("GetAll");
    // }

    // [HttpGet]
    // public IActionResult Delete(int id)
    // {
    //     var producto = _productoRepository.GetById(id); // Busca el producto por ID
    //     if (producto == null)
    //     {
    //         ViewData["ErrorMessage"] = "El producto con el ID proporcionado no existe.";
    //         return View("Error"); // Si el producto no se encuentra, redirige a una página de error
    //     }
    //     return View(producto); // Si se encuentra, lo pasa a la vista para su edición
    // }

    // [HttpPost]
    // public IActionResult Delete(Producto producto , int id)
    // {
    //     _productoRepository.Delete(id);
    //     return RedirectToAction("GetAll");
        
    // }



    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
