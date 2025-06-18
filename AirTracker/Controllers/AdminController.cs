using AirTracker.Data;
using Microsoft.AspNetCore.Mvc;
using AirTracker.Models.ViewModels;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.TotalLocations = _context.Locations.Count();
        ViewBag.TotalSensors = _context.Sensors.Count();
        ViewBag.MostUsedSensor = "PM2.5"; // <- statycznie przypisany typ sensora

        return View();
    }


}
