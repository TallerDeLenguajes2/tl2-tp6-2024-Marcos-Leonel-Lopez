using System.Diagnostics;
using IClienteRepo;
using IPresupuestoRepo;
using IProductoRepo;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Marcos_Leonel_Lopez.Models;




namespace tl2_tp6_2024_Marcos_Leonel_Lopez.Controllers;

public class PresupuestoController : Controller
{
    private readonly IPresupuestoRepository _presupuestoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IProductoRepository _proroductoRepository;
    public PresupuestoController(IPresupuestoRepository presupuestoRepository, IClienteRepository clienteRepository, IProductoRepository productoRepository)
    {
        _presupuestoRepository = presupuestoRepository;
        _clienteRepository = clienteRepository;
        _proroductoRepository = productoRepository;
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
        var viewModel = new CrearPresupuestoViewModel(_clienteRepository.GetAll());
        return View(viewModel);
    }
    [HttpPost]
    public IActionResult Create(CrearPresupuestoViewModel viewModel)
    {
        // Asignar el cliente seleccionado al destinatario del presupuesto
        viewModel.Presupuesto.Destinatario = new Cliente { IdCliente = viewModel.Presupuesto.Destinatario.IdCliente };

        // Crear el presupuesto usando el repositorio
        _presupuestoRepository.Create(viewModel.Presupuesto);
        return RedirectToAction("GetAll");
    }
    [HttpGet]
    public IActionResult Update(int id)
    {
        var presupuesto = _presupuestoRepository.GetById(id); // Busca el producto por ID
        if (presupuesto == null)
        {
            ViewData["ErrorMessage"] = "El presupuesto con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(presupuesto);
    }
    [HttpPost]
    public IActionResult RemoveProducto(int idPresupuesto, int idProducto)
    {
        bool result = _presupuestoRepository.RemoveProducto(idPresupuesto, idProducto);
        if (!result)
        {
            ViewData["ErrorMessage"] = "No se pudo eliminar el producto.";
            return View("Error");
        }
        return RedirectToAction("Update", new { id = idPresupuesto });
    }

    // Acción para actualizar la cantidad de un producto en el detalle del presupuesto
    [HttpPost]
    public IActionResult UpdateCantidad(int idPresupuesto, int idProducto, int nuevaCantidad)
    {
        bool result = _presupuestoRepository.UpdateCantidadEnDetalle(idPresupuesto, idProducto, nuevaCantidad);
        if (!result)
        {
            ViewData["ErrorMessage"] = "No se pudo actualizar la cantidad.";
            return View("Error");
        }
        return RedirectToAction("Update", new { id = idPresupuesto });
    }
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
    public IActionResult Delete(Presupuesto presupuesto, int id)
    {
        _presupuestoRepository.Remove(id);
        return RedirectToAction("GetAll");
    }
    [HttpGet]
    public IActionResult AddProductoEnPresupuesto(int id)
    {
        var viewModel = new AgregarProductoViewModel(_presupuestoRepository.GetById(id), _proroductoRepository.GetAll());

        if (viewModel.Presupuesto == null)
        {
            ViewData["ErrorMessage"] = "El presupuesto con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(viewModel);
    }
    [HttpPost]
    public IActionResult AddProductoEnPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        Presupuesto result = _presupuestoRepository.AddProductoEnPresupuesto(idPresupuesto, idProducto, cantidad);
        return RedirectToAction("AddProductoEnPresupuesto", new { id = idPresupuesto });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
