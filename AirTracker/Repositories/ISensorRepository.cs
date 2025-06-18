using AirTracker.Models;

namespace AirTracker.Repositories
{
    public interface ISensorRepository
    {
        Task<List<Sensor>> GetAllAsync();
        Task<Sensor?> GetByIdAsync(int id);

        /// <summary>
        /// Zwraca wszystkie sensory przypisane do danej lokalizacji.
        /// </summary>
        Task<List<Sensor>> GetByLocationIdAsync(int locationId);

        Task AddAsync(Sensor sensor);
        Task UpdateAsync(Sensor sensor);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
