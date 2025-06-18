using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AirTracker.Data;
using AirTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace AirTracker.Services
{
    public class LocationImportService : ILocationImportService
    {
        private readonly ApplicationDbContext _ctx;
        private readonly HttpClient _http;

        public LocationImportService(ApplicationDbContext ctx, HttpClient httpClient)
        {
            _ctx = ctx;
            _http = httpClient;
        }

        public async Task ImportCitiesAsync(IEnumerable<(string Name, double Lat, double Lon)> cities)
        {
            foreach (var (cityName, lat, lon) in cities)
            {
                // Formatowanie z kropką (InvariantCulture)
                var latStr = lat.ToString("F6", CultureInfo.InvariantCulture);
                var lonStr = lon.ToString("F6", CultureInfo.InvariantCulture);
                var url = $"locations?coordinates={latStr},{lonStr}&radius=25000&limit=10&page=1";

                HttpResponseMessage resp;
                try
                {
                    resp = await _http.GetAsync(url);
                    resp.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{cityName}] HTTP error: {ex.Message}");
                    await Task.Delay(200);
                    continue;
                }

                using var stream = await resp.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                if (!doc.RootElement.TryGetProperty("results", out var results) ||
                    results.ValueKind != JsonValueKind.Array)
                {
                    Console.WriteLine($"[{cityName}] no results array");
                    await Task.Delay(200);
                    continue;
                }

                foreach (var locEl in results.EnumerateArray())
                {
                    var name = locEl.GetProperty("name").GetString()!;
                    var coords = locEl.GetProperty("coordinates");
                    var latitude = coords.GetProperty("latitude").GetDouble();
                    var longitude = coords.GetProperty("longitude").GetDouble();

                    // Znajdź lub dodaj Location
                    var existingLoc = await _ctx.Locations
                        .FirstOrDefaultAsync(l =>
                            l.City == cityName &&
                            l.Name == name &&
                            Math.Abs(l.Latitude - latitude) < 1e-6 &&
                            Math.Abs(l.Longitude - longitude) < 1e-6);

                    if (existingLoc == null)
                    {
                        existingLoc = new Location
                        {
                            City = cityName,
                            Name = name,
                            Latitude = latitude,
                            Longitude = longitude
                        };
                        _ctx.Locations.Add(existingLoc);
                        await _ctx.SaveChangesAsync();
                    }

                    // Teraz sensorki
                    if (locEl.TryGetProperty("sensors", out var sensorsEl) &&
                        sensorsEl.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var sEl in sensorsEl.EnumerateArray())
                        {
                            var openAQId = sEl.GetProperty("id").GetInt32();
                            var sensorName = sEl.GetProperty("name").GetString()!;

                            // Czy mamy już sensora w bazie?
                            var existingSensor = await _ctx.Sensors
                                .FirstOrDefaultAsync(s => s.OpenAQSensorId == openAQId);

                            if (existingSensor != null)
                            {
                                // Jeżeli LocationId jest inny lub nieustawiony, nadpisz
                                if (existingSensor.LocationId != existingLoc.Id)
                                {
                                    existingSensor.LocationId = existingLoc.Id;
                                    _ctx.Sensors.Update(existingSensor);
                                    await _ctx.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                // Dodaj nowy sensor
                                var newSensor = new Sensor
                                {
                                    Name = sensorName,
                                    OpenAQSensorId = openAQId,
                                    LocationId = existingLoc.Id
                                };
                                _ctx.Sensors.Add(newSensor);
                                await _ctx.SaveChangesAsync();
                            }
                        }
                    }
                }

                // Krótka pauza, by nie walić wszystkiego na raz
                await Task.Delay(200);
            }
        }
    }
}
