using GorevYoneticisiProjesi.Models;

namespace GorevYoneticisiProjesi.Services
{
    public interface ITaskReportService
    {
        Task<IEnumerable<TaskReport>> GetAllAsync(Guid userId);
        Task<TaskReport?> GetByIdAsync(Guid userId, Guid reportId);
        Task<TaskReport> CreateAsync(TaskReport report);
        Task UpdateAsync(TaskReport report);
        Task DeleteAsync(Guid userId, Guid reportId);
    }
}
