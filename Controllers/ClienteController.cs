using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Marcos_Leonel_Lopez.Models;

namespace tl2_tp6_2024_Marcos_Leonel_Lopez.Controllers;
public class ClienteController : Controller
{
    private readonly ClienteRepo.ClienteRepository _clienteRepository;
    public ClienteController(ClienteRepo.ClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }
    public IActionResult Index() // Igual al nombre de los archivos en carpeta "Views"
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return View(_clienteRepository.GetAll());
    }
    [HttpGet]
    public IActionResult GetById(int id)
    {
        var cliente = _clienteRepository.GetById(id);
        if (cliente == null)
        {
            ViewData["ErrorMessage"] = "El cliente con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(cliente);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Cliente());
    }
    [HttpPost]
    public IActionResult Create(Cliente nuevoCliente)
    {
        var cliente = _clienteRepository.Create(nuevoCliente);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var cliente = _clienteRepository.GetById(id); // Busca el producto por ID
        if (cliente == null)
        {
            ViewData["ErrorMessage"] = "El cliente con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(cliente);
    }

    [HttpPost]
    public IActionResult Delete(Cliente cliente, int id)
    {
        _clienteRepository.Delete(id);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var cliente = _clienteRepository.GetById(id);
        if (cliente == null)
        {
            ViewData["ErrorMessage"] = "El cliente con el ID proporcionado no existe.";
            return View("Error");
        }
        return View(cliente);
    }

    [HttpPost]
    public IActionResult Update(Cliente cliente,int id)
    {
        _clienteRepository.Update(cliente,id);
        return RedirectToAction("GetAll");
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}