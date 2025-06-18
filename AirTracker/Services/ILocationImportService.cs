using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirTracker.Services
{
    public interface ILocationImportService
    {
        /// <summary>
        /// Pobiera z OpenAQ do 10 najbliższych lokalizacji (w zadanym promieniu) dla każdej z miast
        /// i zapisuje je wraz z sensorami do bazy.
        /// </summary>
        /// <param name="cities">Lista krotek: (nazwa miasta, szerokość, długość)</param>
        Task ImportCitiesAsync(IEnumerable<(string Name, double Lat, double Lon)> cities);
    }
}
