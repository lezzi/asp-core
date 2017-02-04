using System;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        [NotNull]
        public UserModel User { get; set; }

        public bool IsAccepted { get; set; }

        [CanBeNull]
        public QuestionDetailsModel Question { get; set; }
    }
}