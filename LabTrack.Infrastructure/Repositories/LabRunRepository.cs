using LabTrack.Core.Interfaces;
using LabTrack.Core.Models;
using LabTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LabTrack.Infrastructure.Repositories
{
    public class LabRunRepository : ILabRunRepository
    {
        private readonly AppDbContext _context;

        public LabRunRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LabRun>> GetAllAsync()
        {
            return await _context.LabRuns
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<LabRun?> GetByIdAsync(int id)
        {
            return await _context.LabRuns.FindAsync(id);
        }

        public async Task AddAsync(LabRun labRun)
        {
            await _context.LabRuns.AddAsync(labRun);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LabRun labRun)
        {
            _context.LabRuns.Update(labRun);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var labRun = await _context.LabRuns.FindAsync(id);
            if (labRun != null)
            {
                _context.LabRuns.Remove(labRun);
                await _context.SaveChangesAsync();
            }
        }
    }
}