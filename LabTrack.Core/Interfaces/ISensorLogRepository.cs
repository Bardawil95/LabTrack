using LabTrack.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabTrack.Core.Interfaces
{
    public interface ISensorLogRepository
    {
        Task AddAsync(SensorLog log);
        Task<IEnumerable<SensorLog>> GetByLabRunIdAsync(int labRunId);
    }
}
