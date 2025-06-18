using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AirTracker.Data;
using AirTracker.Models.ViewModels;

namespace AirTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
            => _context = context;

        // GET: /Home?selectedSensorId=5
        public IActionResult Index(int? selectedSensorId)
        {
            var sensors = _context.Sensors.OrderBy(s => s.Name).ToList();
            if (!selectedSensorId.HasValue && sensors.Any())
                selectedSensorId = sensors[0].Id;

            var vm = new HomeIndexViewModel
            {
                Sensors = sensors,
                SelectedSensorId = selectedSensorId
            };
            return View(vm);
        }


        public IActionResult Privacy()
            => View();
    }
}
