using System.ComponentModel.DataAnnotations;

namespace DotCore.Portable.BusinessLogic.Models
{
    public class AskQuestionModel
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}