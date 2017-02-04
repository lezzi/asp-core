using System.ComponentModel.DataAnnotations;

namespace DotCore.Portable.BusinessLogic.Models
{
    public class NewAnswer
    {
        [Required]
        public string Description { get; set; }
    }
}