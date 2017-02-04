using System.Threading.Tasks;
using DotCore.Portable.DataAccess.Entities;
using JetBrains.Annotations;

namespace DotCore.Portable.DataAccess.Repositories
{
    /// <summary>
    /// <see cref="Answer" /> entity repository.
    /// </summary>
    public interface IAnswerRepository
    {
        #region Methods

        /// <summary>
        /// Loads specified <see cref="Answer" /> entity.
        /// <see cref="Answer.User" /> and <see cref="Answer.Question" />.<see cref="Question.User" /> are included.
        /// </summary>
        /// <param name="answerId">Answer id.</param>
        /// <returns>Specified <see cref="Answer" /> entity.</returns>
        Task<Answer> GetAnswerAsync(int answerId);

        /// <summary>
        /// Adds new <see cref="Answer"/> entity.
        /// </summary>
        /// <param name="answer"><see cref="Answer" /> entity.</param>
        void AddAnswer([NotNull] Answer answer);

        #endregion
    }
}