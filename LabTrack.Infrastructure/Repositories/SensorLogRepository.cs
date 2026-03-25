using LabTrack.Core.Interfaces;
using LabTrack.Core.Models;
using MongoDB.Driver;

namespace LabTrack.Infrastructure.Repositories
{
    public class SensorLogRepository : ISensorLogRepository
    {
        private readonly IMongoCollection<SensorLog> _collection;

        public SensorLogRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<SensorLog>("SensorLogs");
        }

        public async Task AddAsync(SensorLog log)
        {
            await _collection.InsertOneAsync(log);
        }

        public async Task<IEnumerable<SensorLog>> GetByLabRunIdAsync(int labRunId)
        {
            return await _collection
                .Find(l => l.LabRunId == labRunId)
                .SortBy(l => l.Timestamp)
                .ToListAsync();
        }
    }
}