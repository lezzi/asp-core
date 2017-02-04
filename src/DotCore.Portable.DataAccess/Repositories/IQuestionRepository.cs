using System.Collections.Generic;
using System.Threading.Tasks;
using DotCore.Portable.DataAccess.Entities;
using JetBrains.Annotations;

namespace DotCore.Portable.DataAccess.Repositories
{
    /// <summary>
    /// <see cref="Question" /> entity repository.
    /// </summary>
    public interface IQuestionRepository
    {
        #region Methods

        /// <summary>
        /// Loads active questions and sorts result set by <see cref="Question.CreatedOn" /> in descending order.
        /// <see cref="Question.User" /> is included.
        /// </summary>
        /// <returns>Active questions sorted by <see cref="Question.CreatedOn" /> in descending order.</returns>
        [ItemNotNull]
        Task<IList<Question>> GetActiveQuestionsAsync();

        /// <summary>
        /// Loads answered questions and sorts result set by <see cref="Question.CreatedOn" /> in descending order.
        /// <see cref="Question.User" /> is included.
        /// </summary>
        /// <returns>Answered questions sorted by <see cref="Question.CreatedOn" /> in descending order.</returns>
        [ItemNotNull]
        Task<IList<Question>> GetAnsweredQuestionsAsync();

        /// <summary>
        /// Loads all questions and sorts result set by <see cref="Question.CreatedOn" /> in descending order.
        /// <see cref="Question.User" /> is included.
        /// </summary>
        /// <returns>All questions sorted by <see cref="Question.CreatedOn" /> in descending order.</returns>
        [ItemNotNull]
        Task<IList<Question>> GetAllQuestionsAsync();

        /// <summary>
        /// Loads the specified question with all available answers.
        /// <see cref="Question.User" />, <see cref="Question.AcceptedAnswer" />, <see cref="Question.Answers" /> and
        /// <see cref="Answer" />.<see cref="Answer.User" /> are included.
        /// </summary>
        /// <returns>Question with all available answers.</returns>
        [ItemCanBeNull]
        Task<Question> GetQuestionWithAnswersAsync(int questionId);

        /// <summary>
        /// Loads the specified <see cref="Question" /> entity.
        /// <see cref="Question.User" /> is included.
        /// </summary>
        /// <param name="questionId">Question <see cref="Question.Id" /> value.</param>
        /// <returns>Specified <see cref="Question" /> entity.</returns>
        [ItemCanBeNull]
        Task<Question> GetQuestionAsync(int questionId);

        /// <summary>
        /// Adds new <see cref="Question" /> entity.
        /// </summary>
        /// <param name="question"><see cref="Question" /> entity.</param>
        void AddQuestion([NotNull] Question question);

        #endregion
    }
}