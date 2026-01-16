using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Modelos;

namespace Presentacion.Controllers;

public class InicioController : Controller
{
    public InicioController()
    {
    }

    public IActionResult Inicio()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ModeloError { idSolicitud = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
