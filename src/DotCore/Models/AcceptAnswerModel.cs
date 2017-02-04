using System.ComponentModel.DataAnnotations;

namespace DotCore.Models
{
    public class AcceptAnswerModel
    {
        [Required]
        public int AnswerId { get; set; }
    }
}