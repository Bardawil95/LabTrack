using LabTrack.Core.Interfaces;
using LabTrack.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabTrack.Web.Controllers
{
    public class LabRunsController : Controller
    {
        private readonly ILabRunRepository _labRunRepository;
        private readonly ISensorLogRepository _sensorLogRepository;

        public LabRunsController(ILabRunRepository labRunRepository, ISensorLogRepository sensorLogRepository)
        {
            _labRunRepository = labRunRepository;
            _sensorLogRepository = sensorLogRepository;
        }

        // GET: /LabRuns
        public async Task<IActionResult> Index()
        {
            var labRuns = await _labRunRepository.GetAllAsync();
            return View(labRuns);
        }

        // GET: /LabRuns/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var labRun = await _labRunRepository.GetByIdAsync(id);
            if (labRun == null) return NotFound();

            var sensorLogs = await _sensorLogRepository.GetByLabRunIdAsync(id);
            ViewBag.SensorLogs = sensorLogs;

            return View(labRun);
        }

        // GET: /LabRuns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /LabRuns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LabRun labRun)
        {
            if (!ModelState.IsValid) return View(labRun);

            labRun.CreatedAt = DateTime.UtcNow;
            labRun.Status = LabRunStatus.Pending;
            await _labRunRepository.AddAsync(labRun);

            return RedirectToAction(nameof(Index));
        }

        // GET: /LabRuns/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var labRun = await _labRunRepository.GetByIdAsync(id);
            if (labRun == null) return NotFound();
            return View(labRun);
        }

        // POST: /LabRuns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LabRun labRun)
        {
            if (id != labRun.Id) return BadRequest();
            if (!ModelState.IsValid) return View(labRun);

            await _labRunRepository.UpdateAsync(labRun);
            return RedirectToAction(nameof(Index));
        }

        // GET: /LabRuns/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var labRun = await _labRunRepository.GetByIdAsync(id);
            if (labRun == null) return NotFound();
            return View(labRun);
        }

        // POST: /LabRuns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _labRunRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: /LabRuns/UpdateStatus/5
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var labRun = await _labRunRepository.GetByIdAsync(id);
            if (labRun == null) return NotFound();

            labRun.Status = status;

            if (status == LabRunStatus.Running)
                labRun.StartedAt = DateTime.UtcNow;
            else if (status == LabRunStatus.Completed || status == LabRunStatus.Failed)
                labRun.CompletedAt = DateTime.UtcNow;

            await _labRunRepository.UpdateAsync(labRun);
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /LabRuns/AddSensorLog
        [HttpPost]
        public async Task<IActionResult> AddSensorLog(int labRunId, string sensorName, double value, string unit)
        {
            var log = new SensorLog
            {
                LabRunId = labRunId,
                SensorName = sensorName,
                Value = value,
                Unit = unit,
                Timestamp = DateTime.UtcNow
            };

            await _sensorLogRepository.AddAsync(log);
            return RedirectToAction(nameof(Details), new { id = labRunId });
        }

        // GET: /LabRuns/SensorLogsJson/5
        public async Task<IActionResult> SensorLogsJson(int id)
        {
            var logs = await _sensorLogRepository.GetByLabRunIdAsync(id);
            return Json(logs);
        }
    }
}