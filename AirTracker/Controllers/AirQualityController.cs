using System;
using System.Linq;
using System.Threading.Tasks;
using AirTracker.Data;
using AirTracker.Models;

using AirTracker.Models.ViewModels;
using AirTracker.Repositories;
using AirTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirTracker.Controllers
{
    public class AirQualityController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ISensorRepository _sensorRepo;
        private readonly IOpenAQService _openAQ;

        public AirQualityController(
            ApplicationDbContext context,
            ISensorRepository sensorRepo,
            IOpenAQService openAQService)
        {
            _ctx = context;
            _sensorRepo = sensorRepo;
            _openAQ = openAQService;
        }

        public async Task<IActionResult> Index(
      string? selectedCity,
      int? selectedLocationId,
      int? selectedSensorId)
        {
            var vm = new AirQualitySelectionViewModel();

            // Wszystkie lokalizacje do mapy
            vm.AllLocations = await _ctx.Locations
                                        .OrderBy(l => l.City)
                                        .ThenBy(l => l.Name)
                                        .ToListAsync();

            // Lista miast
            vm.Cities = await _ctx.Locations
                                  .Select(l => l.City)
                                  .Distinct()
                                  .OrderBy(c => c)
                                  .ToListAsync();

            Location? selectedLocation = null;

            // 👉 Jeśli podano lokalizację, załaduj ją (niezależnie od miasta)
            if (selectedLocationId.HasValue)
            {
                selectedLocation = await _ctx.Locations
                                             .FirstOrDefaultAsync(l => l.Id == selectedLocationId.Value);
                vm.SelectedLocationId = selectedLocationId;

                if (selectedLocation != null && string.IsNullOrEmpty(selectedCity))
                {
                    selectedCity = selectedLocation.City;
                }
            }

            // 👉 Ustaw wybrane miasto
            if (!string.IsNullOrEmpty(selectedCity))
            {
                vm.SelectedCity = selectedCity;

                // Załaduj wszystkie lokalizacje z tego miasta
                vm.Locations = await _ctx.Locations
                                         .Where(l => l.City == selectedCity)
                                         .OrderBy(l => l.Name)
                                         .ToListAsync();

                // Jeśli wybrana lokalizacja NIE jest z tego miasta, dodaj ją do listy ręcznie
                if (selectedLocation != null && !vm.Locations.Any(l => l.Id == selectedLocation.Id))
                {
                    vm.Locations.Add(selectedLocation);
                }
            }

            // 👉 Pobierz sensory
            if (selectedLocationId.HasValue)
            {
                vm.Sensors = await _sensorRepo.GetByLocationIdAsync(selectedLocationId.Value);
            }

            vm.SelectedSensorId = selectedSensorId;

            // 👉 Pobierz dane sensora
            if (selectedSensorId.HasValue)
            {
                var selectedSensor = vm.Sensors?.FirstOrDefault(s => s.Id == selectedSensorId.Value);
                if (selectedSensor != null)
                {
                    var fromUtc = DateTime.Today.AddDays(-1).ToUniversalTime();
                    var toUtc = DateTime.UtcNow;

                    vm.Data = await _openAQ.GetLatestMeasurementsAsync(
                        selectedSensor.OpenAQSensorId,
                        limit: 100,
                        dateFrom: fromUtc,
                        dateTo: toUtc
                    );
                }
            }

            return View(vm);
        }
        public async Task<IActionResult> Compare(int sensorId, int locationId1, int locationId2)
        {
            var sensor = await _ctx.Sensors.FindAsync(sensorId);
            var location1 = await _ctx.Locations.FindAsync(locationId1);
            var location2 = await _ctx.Locations.FindAsync(locationId2);

            if (sensor == null || location1 == null || location2 == null)
                return NotFound("Nieprawidłowe dane porównania.");

            var fromUtc = DateTime.Today.AddDays(-1).ToUniversalTime();
            var toUtc = DateTime.UtcNow;

            // POBIERAMY osobno dane jakości powietrza dla lokalizacji 1
            var sensor1 = await _sensorRepo.GetByLocationIdAsync(locationId1);
            var sensor2 = await _sensorRepo.GetByLocationIdAsync(locationId2);

            var selectedSensor1 = sensor1.FirstOrDefault(s => s.Name == sensor.Name);
            var selectedSensor2 = sensor2.FirstOrDefault(s => s.Name == sensor.Name);

            if (selectedSensor1 == null || selectedSensor2 == null)
                return NotFound("Nie znaleziono sensora w jednej z lokalizacji.");

            var data1 = await _openAQ.GetLatestMeasurementsAsync(
                selectedSensor1.OpenAQSensorId, 100, fromUtc, toUtc);

            var data2 = await _openAQ.GetLatestMeasurementsAsync(
                selectedSensor2.OpenAQSensorId, 100, fromUtc, toUtc);

            var viewModel = new AirQualityCompareViewModel
            {
                SensorName = sensor.Name,
                Location1 = location1,
                Location2 = location2,
                Data1 = data1,
                Data2 = data2
            };

            return View("Compare", viewModel);
        }
        public async Task<IActionResult> CompareStart(int sensorId, int locationId1)
        {
            var location1 = await _ctx.Locations.FindAsync(locationId1);
            var selectedSensor = await _ctx.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);

            if (location1 == null || selectedSensor == null)
            {
                return NotFound("Nieprawidłowe dane.");
            }

            // Szukaj lokalizacji, które mają sensor o dokładnie tej samej nazwie
            var matchingLocationIds = await _ctx.Sensors
                .Where(s => s.Name == selectedSensor.Name && s.LocationId != locationId1)
                .Select(s => s.LocationId)
                .Distinct()
                .ToListAsync();

            var matchingLocations = await _ctx.Locations
                .Where(l => matchingLocationIds.Contains(l.Id))
                .OrderBy(l => l.City)
                .ThenBy(l => l.Name)
                .ToListAsync();

            ViewBag.SensorId = sensorId;
            ViewBag.LocationId1 = locationId1;
            ViewBag.LocationName1 = location1.Name;

            return View("CompareStart", matchingLocations);
        }

        public async Task<IActionResult> ExportCsv(int sensorId, int locationId)
        {
            var sensor = await _ctx.Sensors.FindAsync(sensorId);
            var location = await _ctx.Locations.FindAsync(locationId);

            if (sensor == null || location == null)
                return NotFound("Nieprawidłowe dane eksportu.");

            var fromUtc = DateTime.Today.AddDays(-1).ToUniversalTime();
            var toUtc = DateTime.UtcNow;

            var data = await _openAQ.GetLatestMeasurementsAsync(sensor.OpenAQSensorId, 100, fromUtc, toUtc);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Data,Parametr,Wartość,Jednostka");

            foreach (var item in data.OrderBy(d => d.RetrievedAt))
            {
                var row = $"{item.RetrievedAt:yyyy-MM-dd HH:mm},{item.Parameter},{item.Value},{item.Units}";
                sb.AppendLine(row);
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"pomiar_{sensor.Name}_{location.City}_{DateTime.Now:yyyyMMddHHmm}.csv";

            return File(bytes, "text/csv", fileName);
        }




        public async Task<IActionResult> ExportComparisonCsv(string sensorId, int locationId1, int locationId2)
        {
            var loc1 = await _ctx.Locations.FindAsync(locationId1);
            var loc2 = await _ctx.Locations.FindAsync(locationId2);

            if (loc1 == null || loc2 == null)
                return NotFound("Nieprawidłowe lokalizacje.");

            var sensors1 = await _sensorRepo.GetByLocationIdAsync(locationId1);
            var sensors2 = await _sensorRepo.GetByLocationIdAsync(locationId2);

            var sensor1 = sensors1.FirstOrDefault(s => s.Name == sensorId);
            var sensor2 = sensors2.FirstOrDefault(s => s.Name == sensorId);

            if (sensor1 == null || sensor2 == null)
                return NotFound("Nie znaleziono odpowiednich sensorów.");

            var fromUtc = DateTime.Today.AddDays(-1).ToUniversalTime();
            var toUtc = DateTime.UtcNow;

            var data1 = await _openAQ.GetLatestMeasurementsAsync(sensor1.OpenAQSensorId, 100, fromUtc, toUtc);
            var data2 = await _openAQ.GetLatestMeasurementsAsync(sensor2.OpenAQSensorId, 100, fromUtc, toUtc);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Data;Parametr;" +
                $"{loc1.City} - {loc1.Name};" +
                $"{loc2.City} - {loc2.Name}");

            var grouped = data1.Concat(data2)
                .GroupBy(x => new { x.RetrievedAt, x.Parameter })
                .OrderBy(g => g.Key.RetrievedAt)
                .ThenBy(g => g.Key.Parameter);

            foreach (var g in grouped)
            {
                var d1 = data1.FirstOrDefault(d => d.RetrievedAt == g.Key.RetrievedAt && d.Parameter == g.Key.Parameter);
                var d2 = data2.FirstOrDefault(d => d.RetrievedAt == g.Key.RetrievedAt && d.Parameter == g.Key.Parameter);

                var row = $"{g.Key.RetrievedAt:yyyy-MM-dd HH:mm};{g.Key.Parameter};" +
                          $"{(d1 != null ? d1.Value.ToString("F2") : "-")};" +
                          $"{(d2 != null ? d2.Value.ToString("F2") : "-")}";
                sb.AppendLine(row);
            }

            var encoding = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
            var bytes = encoding.GetBytes(sb.ToString());

            var fileName = $"porownanie_{sensorId}_{DateTime.Now:yyyyMMddHHmm}.csv";
            return File(bytes, "text/csv", fileName);
        }



    }
}
