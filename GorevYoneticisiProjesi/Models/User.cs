namespace GorevYoneticisiProjesi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<TaskReport> TaskReports { get; set; }

    }
}
