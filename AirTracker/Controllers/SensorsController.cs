using System.Linq;
using System.Threading.Tasks;
using AirTracker.Data;
using AirTracker.Models;
using AirTracker.Models.ViewModels;
using AirTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SensorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOpenAQService _openAQ;

        public SensorsController(ApplicationDbContext context, IOpenAQService openAQService)
        {
            _context = context;
            _openAQ = openAQService;
        }
        // GET: /Sensors
        public async Task<IActionResult> Index()
        {
            var sensors = await _context.Sensors.OrderBy(s => s.Name).ToListAsync();
            return View(sensors);
        }

        // GET: /Sensors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sensor = await _context.Sensors.FindAsync(id.Value);
            if (sensor == null) return NotFound();

            // pobierz pomiary
            var measurements = await _openAQ.GetLatestMeasurementsAsync(sensor.OpenAQSensorId, limit: 20);

            // oblicz statystyki
            var values = measurements.Select(m => m.Value).OrderBy(v => v).ToArray();
            int n = values.Length;
            double Percentile(double p)
            {
                if (n == 0) return 0;
                var idx = p * (n - 1);
                var lo = (int)System.Math.Floor(idx);
                var hi = (int)System.Math.Ceiling(idx);
                return lo == hi
                    ? values[lo]
                    : values[lo] + (values[hi] - values[lo]) * (idx - lo);
            }
            double avg = n > 0 ? values.Average() : 0;
            double sd = n > 1 ? System.Math.Sqrt(values.Sum(v => (v - avg) * (v - avg)) / (n - 1)) : 0;

            var summary = new AirQualitySummary
            {
                Min = values.DefaultIfEmpty(0).First(),
                Q02 = Percentile(0.02),
                Q25 = Percentile(0.25),
                Median = Percentile(0.50),
                Q75 = Percentile(0.75),
                Q98 = Percentile(0.98),
                Max = values.DefaultIfEmpty(0).Last(),
                Avg = avg,
                SD = sd
            };

            var vm = new SensorDetailsViewModel
            {
                Sensor = sensor,
                Data = measurements,
                Summary = summary
            };

            return View(vm);
        }
        // GET: Sensors/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,OpenAQSensorId")] Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sensor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(sensor);
        }

        // ... pozostałe akcje (Index, Create, Edit, Delete) bez zmian
    }
}
