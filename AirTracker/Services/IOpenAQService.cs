using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirTracker.Models;

namespace AirTracker.Services
{
    public interface IOpenAQService
    {
        /// <summary>
        /// Pobiera najnowsze (sort desc po datetime) pomiary dla danego sensora.
        /// </summary>
        /// <param name="openAQSensorId">ID sensora w OpenAQ</param>
        /// <param name="limit">Ile rekordów pobrać</param>
        /// <param name="dateFrom">Opcjonalnie od jakiej daty (yyyy-MM-dd) filtrować</param>
        Task<List<AirQuality>> GetLatestMeasurementsAsync(int openAQSensorId, int limit = 20, DateTime? dateFrom = null, DateTime? dateTo = null);

    }
}
