using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AirTracker.Models;

namespace AirTracker.Services
{
    public class OpenAQService : IOpenAQService
    {
        private readonly HttpClient _http;

        public OpenAQService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AirQuality>> GetLatestMeasurementsAsync(
    int openAQSensorId,
    int limit = 20,
    DateTime? dateFrom = null,
    DateTime? dateTo = null)
        {
            // Domyślne wartości: ostatnie 2 dni
            dateFrom ??= DateTime.UtcNow.AddDays(-2);
            dateTo ??= DateTime.UtcNow;

            // Budujemy URL z datami w ISO8601
            var url = $"sensors/{openAQSensorId}/measurements?order_by=datetime&sort=desc&limit={limit}" +
                      $"&date_from={dateFrom.Value:yyyy-MM-ddTHH:mm:ssZ}&date_to={dateTo.Value:yyyy-MM-ddTHH:mm:ssZ}";

            var resp = await _http.GetAsync(url);
            if (resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                return new List<AirQuality>();
            }

            resp.EnsureSuccessStatusCode();
            await using var stream = await resp.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var root = doc.RootElement;
            var result = new List<AirQuality>();

            if (root.TryGetProperty("results", out var results) && results.ValueKind == JsonValueKind.Array)
            {
                foreach (var m in results.EnumerateArray())
                {
                    // Parsowanie czasu pomiaru
                    DateTime measurementTime = DateTime.UtcNow;
                    if (m.TryGetProperty("period", out var periodEl)
                        && periodEl.TryGetProperty("datetimeFrom", out var dtEl))
                    {
                        if (dtEl.TryGetProperty("local", out var localEl)
                            && localEl.ValueKind == JsonValueKind.String
                            && DateTime.TryParse(localEl.GetString(), out var lp))
                        {
                            measurementTime = lp;
                        }
                        else if (dtEl.TryGetProperty("utc", out var utcEl)
                                 && utcEl.ValueKind == JsonValueKind.String
                                 && DateTime.TryParse(utcEl.GetString(), out var up))
                        {
                            measurementTime = up.ToLocalTime();
                        }
                    }

                    // Surowa wartość
                    double value = 0;
                    if (m.TryGetProperty("value", out var vEl) && vEl.ValueKind == JsonValueKind.Number)
                    {
                        value = vEl.GetDouble();
                    }

                    // Parametr i jednostki
                    string parameter = "", units = "";
                    if (m.TryGetProperty("parameter", out var pEl) && pEl.ValueKind == JsonValueKind.Object)
                    {
                        if (pEl.TryGetProperty("name", out var n) && n.ValueKind == JsonValueKind.String)
                        {
                            parameter = n.GetString()!;
                        }
                        if (pEl.TryGetProperty("units", out var u) && u.ValueKind == JsonValueKind.String)
                        {
                            units = u.GetString()!;
                        }
                    }

                    result.Add(new AirQuality
                    {
                        SensorId = openAQSensorId,
                        RetrievedAt = measurementTime,
                        Value = value,
                        Parameter = parameter,
                        Units = units
                    });
                }
            }

            return result;
        }

    }
}
