using GorevYoneticisiProjesi.Models;
using System.ComponentModel.DataAnnotations;


namespace GorevYoneticisiProjesi.Dto
{
    public class TaskReportDto
    {
        [Required]
        public string Title { get; set; }=string.Empty;

        [Required]
        public string Description { get; set; }=string.Empty;

        [Required]
        [EnumDataType(typeof(Period))]
        public Period Period { get; set; }
    }
}
