// Controllers/TaskReportsController.cs
using GorevYoneticisiProjesi.Models;
using GorevYoneticisiProjesi.Services;
using GorevYoneticisiProjesi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GorevYoneticisiProjesi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskReportsController : ControllerBase
    {
        private readonly ITaskReportService _reportService;

        public TaskReportsController(ITaskReportService reportService)
        {
            _reportService = reportService;
        }

        // Helper: JWT'deki "id" claim'ini güvenle parse eder
        private bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var claim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out userId))
                return false;
            return true;
        }

        // GET: api/TaskReports
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var reports = await _reportService.GetAllAsync(userId);
            // Projection: yalnızca gerekli alanları dön
            var result = reports.Select(r => new {
                r.Id,
                r.Title,
                r.Description,
                r.Period,
                r.ReportDate
            });
            return Ok(result);
        }

        // GET: api/TaskReports/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var report = await _reportService.GetByIdAsync(userId, id);
            if (report == null)
                return NotFound();

            var result = new
            {
                report.Id,
                report.Title,
                report.Description,
                report.Period,
                report.ReportDate
            };
            return Ok(result);
        }

        // POST: api/TaskReports
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var entity = new TaskReport
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Period = dto.Period
                // Id ve ReportDate servis içinde atanıyor (CreateAsync)
            };

            var created = await _reportService.CreateAsync(entity);
            var result = new
            {
                created.Id,
                created.Title,
                created.Description,
                created.Period,
                created.ReportDate
            };
            // CreatedAtAction ile 201 ve Location header
            return CreatedAtAction(nameof(Get), new { id = created.Id }, result);
        }

        // PUT: api/TaskReports/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var existing = await _reportService.GetByIdAsync(userId, id);
            if (existing == null)
                return NotFound();

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Period = dto.Period;
            // Eğer ReportDate’i güncellemek istemiyorsanız buraya dokunmayın.
            await _reportService.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE: api/TaskReports/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var existing = await _reportService.GetByIdAsync(userId, id);
            if (existing == null)
                return NotFound();

            await _reportService.DeleteAsync(userId, id);
            return NoContent();
        }
    }
}
