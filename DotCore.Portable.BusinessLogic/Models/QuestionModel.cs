using System;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsAnswered { get; set; }

        [NotNull]
        public UserModel User { get; set; }
    }
}