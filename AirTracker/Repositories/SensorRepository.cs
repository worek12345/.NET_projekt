using System.Collections.Generic;
using System.Threading.Tasks;
using AirTracker.Models;
using AirTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace AirTracker.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ApplicationDbContext _context;
        public SensorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sensor>> GetAllAsync()
            => await _context.Sensors.OrderBy(s => s.Name).ToListAsync();

        public async Task<Sensor?> GetByIdAsync(int id)
            => await _context.Sensors.FindAsync(id);
        public async Task<List<Sensor>> GetByLocationIdAsync(int locationId)
            => await _context.Sensors
                              .Where(s => s.LocationId == locationId)
                              .OrderBy(s => s.Name)
                              .ToListAsync();
        public async Task AddAsync(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sensor sensor)
        {
            _context.Sensors.Update(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor != null)
            {
                _context.Sensors.Remove(sensor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.Sensors.AnyAsync(s => s.Id == id);
    }
}
