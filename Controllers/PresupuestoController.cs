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

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = _presupuestoRepository.GetById(id); // Busca el producto por ID
        if (presupuesto == null)
        {
            ViewData["ErrorMessage"] = "El presupuesto con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(presupuesto);
    }

    // [HttpPost]
    // public IActionResult Update(Producto productoEditad, int id)
    // {
    //     var producto = _productoRepository.Update(productoEditad, id);
    //     return RedirectToAction("GetAll");
    // }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuestos = _presupuestoRepository.GetById(id); // Busca el producto por ID
        if (presupuestos == null)
        {
            ViewData["ErrorMessage"] = "El presupuesto con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(presupuestos);
    }

    [HttpPost]
    public IActionResult Delete(Presupuesto presupuesto , int id)
    {
        _presupuestoRepository.Remove(id);
        return RedirectToAction("GetAll");
    }








[HttpPost]
    public IActionResult RemoveProduct(int idPresupuesto, int idProducto)
    {
        bool result = _presupuestoRepository.RemoveProductFromDetails(idPresupuesto, idProducto);
        if (!result)
        {
            ViewData["ErrorMessage"] = "No se pudo eliminar el producto.";
            return View("Error");
        }
        return RedirectToAction("Edit", new { id = idPresupuesto });
    }

    // Acción para actualizar la cantidad de un producto en el detalle del presupuesto
    [HttpPost]
    public IActionResult UpdateProductQuantity(int idPresupuesto, int idProducto, int nuevaCantidad)
    {
        bool result = _presupuestoRepository.UpdateProductQuantityInDetails(idPresupuesto, idProducto, nuevaCantidad);
        if (!result)
        {
            ViewData["ErrorMessage"] = "No se pudo actualizar la cantidad.";
            return View("Error");
        }
        return RedirectToAction("Edit", new { id = idPresupuesto });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
