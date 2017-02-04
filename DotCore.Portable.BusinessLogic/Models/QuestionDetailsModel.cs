using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DotCore.Portable.BusinessLogic.Models
{
    public class QuestionDetailsModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        [NotNull]
        public UserModel User { get; set; }

        [NotNull]
        public IList<AnswerModel> Answers { get; set; }

        public bool IsAnswered { get; set; }
    }
}