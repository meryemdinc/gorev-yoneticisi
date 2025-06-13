using GorevYoneticisiProjesi.Data;
using GorevYoneticisiProjesi.Models;
using Microsoft.EntityFrameworkCore;

namespace GorevYoneticisiProjesi.Services
{
    public class TaskReportService : ITaskReportService
    {

        private readonly AppDbContext _context;

        public TaskReportService(AppDbContext context)
        {
            _context = context;
        }
        //Belirli bir kullanıcıya (userId) ait tüm görev raporlarını getirir.
        public async Task<IEnumerable<TaskReport>> GetAllAsync(Guid userId)
        {
            return await _context.TaskReports
                                 .Where(r => r.UserId == userId)
                                 .ToListAsync();
        }
        //Belirli bir kullanıcı ve belirli bir rapor ID’sine göre tek bir görev raporunu getirir.
       // Eğer rapor yoksa null döner.
        public async Task<TaskReport?> GetByIdAsync(Guid userId, Guid reportId)
        {
            return await _context.TaskReports
                                 .FirstOrDefaultAsync(r => r.UserId == userId && r.Id == reportId);
        }

        public async Task<TaskReport> CreateAsync(TaskReport report)
        {
            report.Id = Guid.NewGuid();
            report.ReportDate = DateTime.Now;

            _context.TaskReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }
        //Var olan bir görev raporunu günceller
        public async Task UpdateAsync(TaskReport report)
        {
            _context.TaskReports.Update(report);
            await _context.SaveChangesAsync();
        }
        //Belirtilen kullanıcıya ait belirli bir raporu siler.
        public async Task DeleteAsync(Guid userId, Guid reportId)
        {
            var entity = await GetByIdAsync(userId, reportId);
            if (entity != null)
            {
                _context.TaskReports.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
