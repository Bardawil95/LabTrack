using LabTrack.Core.Models;

namespace LabTrack.Core.Interfaces
{
    public interface ILabRunRepository
    {
        Task<IEnumerable<LabRun>> GetAllAsync();
        Task<LabRun?> GetByIdAsync(int id);
        Task AddAsync(LabRun labRun);
        Task UpdateAsync(LabRun labRun);
        Task DeleteAsync(int id);
    }
}