namespace GorevYoneticisiProjesi.Models
{
    public enum Period { Daily, Weekly, Monthly }
    public class TaskReport
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Period Period { get; set; }
        public DateTime ReportDate { get; set; }
        public User User { get; set; }

    }
}
